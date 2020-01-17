Imports System.Data.SqlClient

Partial Class general_PrintContract
    Inherits System.Web.UI.Page
    Public Docs As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request("ContractID") = "" Or Request("ContractID") = "0" Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
                Exit Sub
            End If
            Dim oDocs As New clsDocumentGroups
            gvGroups.DataSource = oDocs.List("Word")
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvGroups.DataKeyNames = sKeys
            gvGroups.DataBind()
        End If
    End Sub

    Protected Sub gvGroups_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGroups.SelectedIndexChanged
        Lit.Text = ""

        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select * from t_Contract  where contractid = '" & Request("ContractID") & "'", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        Dim Trustee As String = ""
        Dim bCCFinanced As Boolean = False

        da.Fill(ds, "Contract")

        If ds.Tables("Contract").Rows.Count = 0 Then
            Lit.Text &= ("No Contract Provided")
            GoTo CleanUp
        End If

        cm.CommandText = "Select Path From t_DocumentGroupItems where active = 1 and documentgroupid in (select documentgroupid from t_DocumentGroups where documentgroupid = " & gvGroups.SelectedValue & ")"
        da.Fill(ds, "Documents")
        If ds.Tables("Documents").Rows.Count = 0 Then
            Lit.Text &= ("No Contract Documents Found")
            GoTo CleanUp
        End If
        cn.Open()
        cm.CommandText = "Insert into t_ContractMerge (Username, ContractID, ContractNumber, DateCreated, Printed) values ('" & Session("UserName") & "', '" & ds.Tables("Contract").Rows(0)("ContractID").ToString & "', '" & ds.Tables("Contract").Rows(0)("ContractNumber").ToString & "','" & Date.Today & "',0)"
        cm.ExecuteNonQuery()

        Dim dr As System.Data.DataRow
        Docs = ""
        Dim i As Integer = 0
        Dim j As Integer = 1
        For Each dr In ds.Tables("Documents").Rows
            If dr("Path") <> "" Then
                Docs &= IIf(Docs = "", "'" & dr("Path") & "'", ",'" & dr("Path") & "'")
                If InStr(dr("Path"), "Request for TIN Form") > 0 Then
                    cm.CommandText = "Select * from t_ContractCoOwner where contractid = '" & ds.Tables("Contract").Rows(0).Item("ContractID") & "' order by prospectid"
                    da.Fill(ds, "CoOwners")
                    If ds.Tables("CoOwners").Rows.Count > 0 Then
                        For i = 0 To ds.Tables("CoOwners").Rows.Count - 1
                            If j < 21 Then
                                Docs &= IIf(Docs = "", "'" & Replace(dr("Path"), "Request for TIN Form", "Request for TIN Form" & j) & "'", ",'" & Replace(dr("Path"), "Request for TIN Form", "Request for TIN Form" & j) & "'")
                                j = j + 1
                            End If
                        Next
                    End If
                End If
            End If
        Next
        cn.Close()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Scripting", Get_Script, True)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Execute", "setTimeout('mPrinting()',5000);", True)
        

        Dim oCM As New clsContractMerge
        oCM.ContractID = Request("ContractID")
        oCM.ContractNumber = ds.Tables("Contract").Rows(0)("ContractNumber")
        oCM.DateCreated = Now
        oCM.UserName = Session("UserName")
        oCM.Printed = False
        oCM.Save()
        Response.Write(oCM.Error_Message)
        oCM = Nothing

CleanUp:
        If cn.State <> Data.ConnectionState.Closed Then cn.Close()
        cn = Nothing
        cm = Nothing
        da = Nothing
        ds = Nothing
    End Sub

    Private Function Get_Script() As String
        Dim sScript As String = ""
        sScript &= "function mPrinting(){" & vbCrLf
        sScript &= "var Docs = new Array(" & Replace(Docs, "\", "\\") & ");" & vbCrLf
        sScript &= "var temp = document.getElementById('ScriptControl1');" & vbCrLf

        sScript &= "var mstatement = 'Dim w:';" & vbCrLf
        sScript &= "mstatement += 'set w = createobject(""Word.application""):';" & vbCrLf
        'sScript &= "mstatement += 'set ac = createobject(""Wscript.Shell""):';" & vbCrLf
        sScript &= "for(i=0;i<Docs.length;i++){" & vbCrLf
        'sScript &= "alert(Docs[i]);" & vbCrLf
        'sScript &= "if (Docs[i] == 'T:\\CRMS\\Merge\\Thin\\Conversion\\PointsResortFinanceCCFinanced\\KCP-Personal Entity Guaranty.pdf'){" & vbCrLf
        'sScript &= "mstatement += 'ac.run ""C:\\Program Files (x86)\\Adobe\\Reader 10.0\\Reader\\AcroRd32.exe "" ""' + Docs[i] + '"":';" & vbCrLf
        'sScript &= "mstatement += 'ac.run ""AcroRd32.exe /p /h""' + Docs[i] + '"",,true:';" & vbCrLf
        'sScript &= "}else{" & vbCrLf
        sScript &= "mstatement += 'w.application.documents.open ""' + Docs[i] + '"":w.Visible=true:';		" & vbCrLf
        sScript &= "//alert(nodes[i].selectSingleNode('Path').text);" & vbCrLf
        'sScript &= "}" & vbCrLf
        sScript &= "}" & vbCrLf
        sScript &= "mstatement += 'set w = nothing:';" & vbCrLf
        'sScript &= "alert(mstatement);" & vbCrLf
        sScript &= "temp.ExecuteStatement(mstatement);" & vbCrLf
        sScript &= "window.opener.navigate('" & Request.ApplicationPath & "/marketing/editcontract.aspx?contractid=" & Request("contractid") & "');" & vbCrLf
        sScript &= "window.close();" & vbCrLf
        sScript &= "}" & vbCrLf

        Return sScript
    End Function
End Class
