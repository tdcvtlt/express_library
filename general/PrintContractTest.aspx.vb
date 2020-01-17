Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports PdfSharp
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO
Imports System.Web.UI.WebControls
Partial Class general_PrintContractTest
    Inherits System.Web.UI.Page
    Public Docs As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request("ContractID") = "" Or Request("ContractID") = "0" Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
                Exit Sub
            End If
            Dim oDocs As New clsDocumentGroups
            gvWordGroups.DataSource = oDocs.List("Word")
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvWordGroups.DataKeyNames = sKeys
            gvWordGroups.DataBind()
            gvPDFGroups.DataSource = oDocs.List("PDF")
            gvPDFGroups.DataKeyNames = sKeys
            gvPDFGroups.DataBind()
            oDocs = Nothing
            MultiView1.ActiveViewIndex = 0
            MultiView2.Visible = False
        End If
        If Request("File") <> "" Then
            'MultiView1.ActiveViewIndex = 1
            hfSelected.Value = Request("File")
            Generate_PDF()
            'Dim oCon As New clsContract
            'oCon.ContractID = Request("ContractID")
            'oCon.Load()
            'Response.WriteFile("\\rs-fs-01\G Drive\Daily upload\MIS\" & oCon.ContractNumber & ".pdf")
            'oCon = Nothing
        End If

    End Sub

    Sub View4_Activate(ByVal sender As Object, e As EventArgs)
        Generate_PDF()
    End Sub

    Sub View3_Activate(ByVal sender As Object, e As EventArgs)
        MultiView1.ActiveViewIndex = 3

    End Sub

    Protected Sub gvPDFGroups_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvPDFGroups.SelectedIndexChanged
        'Generate_PDF()
        hfSelected.Value = gvPDFGroups.SelectedValue
        MultiView1.ActiveViewIndex = 1

    End Sub


    Private Sub Generate_PDF()
        Dim oCon As New clsContract
        oCon.ContractID = Request("ContractID")
        oCon.Load()
        Dim oGroupItems As New clsDocumentGroupItems
        Dim row As GridViewRow = gvPDFGroups.SelectedRow
        Dim dt As DataTable = oGroupItems.Get_Group_Items(hfSelected.Value)
        Dim ds As DataSet = Load_Contract_Data(Request("ContractID"))  '282258
        Dim dsData = New ContractDocData
        dsData.Merge(ds)

        Dim output As New PdfDocument()
        For i = 0 To dt.Rows.Count - 1
            Dim Report As New ReportDocument
            Report.Load(Server.MapPath("..\" & dt.Rows(i).Item("Path")))
            Report.FileName = Server.MapPath("..\" & dt.Rows(i).Item("Path"))
            Report.SetDataSource(dsData)
            Report.ExportToDisk(ExportFormatType.PortableDocFormat, "\\rs-fs-01\G Drive\Daily Upload\MIS\" & oCon.ContractNumber & dt.Rows(i).Item("DocumentName") & ".pdf")
            Dim f1 As PdfDocument = PdfReader.Open("\\rs-fs-01\G Drive\Daily Upload\MIS\" & oCon.ContractNumber & dt.Rows(i).Item("DocumentName") & ".pdf", PdfDocumentOpenMode.Import)
            For j = 0 To f1.PageCount - 1
                output.AddPage(f1.Pages(j))
            Next
            File.Delete("\\rs-fs-01\G Drive\Daily Upload\MIS\" & oCon.ContractNumber & dt.Rows(i).Item("DocumentName") & ".pdf")
            Report.Dispose()
            Report = Nothing
            output.Save("\\rs-fs-01\G Drive\Daily upload\MIS\" & oCon.ContractNumber & ".pdf")
        Next

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "attachment;filename=""ContractDocument.pdf""")
        Response.WriteFile("\\rs-fs-01\G Drive\Daily upload\MIS\" & oCon.ContractNumber & ".pdf")
        Response.End()
        'Response.Redirect("PrintContractPDF.aspx?ContractNumber=" & oCon.ContractNumber)
        '        Response.WriteFile("\\rs-fs-01\G Drive\Daily upload\MIS\" & oCon.ContractNumber & ".pdf")
    End Sub

    Protected Sub gvWordGroups_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvWordGroups.SelectedIndexChanged
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

        cm.CommandText = "Select Path From t_DocumentGroupItems where active = 1 and documentgroupid in (select documentgroupid from t_DocumentGroups where documentgroupid = " & gvWordGroups.SelectedValue & ")"
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

    Protected Sub PDF_Link_Click(sender As Object, e As EventArgs) Handles PDF_Link.Click
        MultiView2.Visible = False
        MultiView1.ActiveViewIndex = 0
        MultiView1.Visible = True
    End Sub
    Protected Sub Word_Link_Click(sender As Object, e As EventArgs) Handles Word_Link.Click
        MultiView1.Visible = False
        MultiView2.ActiveViewIndex = 0
        MultiView2.Visible = True
    End Sub
End Class
