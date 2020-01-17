Imports System.IO
Imports System.Drawing

Partial Class marketing_DNC
    Inherits System.Web.UI.Page

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtNew.Text <> "" And Len(txtNew.Text) > 9 Then
            Dim oP As New clsProspect
            Dim oPP As New clsPhone
            Dim oCI As New clsComboItems
            oP.Prospect_ID = oPP.Lookup_ProspectID(Get_Number(txtNew.Text))
            If oP.Prospect_ID > 0 Then
                oP.Load()
                If oCI.Lookup_ComboItem(oP.StatusID).ToUpper = "DO NOT CALL" Or oCI.Lookup_ComboItem(oP.StatusID).ToUpper = "DONOTCALL" Then
                    'Exists
                    lblAddStatus.Text = "This number is already on the list."
                    txtCheck.Text = Get_Number(txtNew.Text)
                    Lookup()
                Else
                    Dim newid As Integer = oCI.Lookup_ID("ProspectStatus", "DO NOT CALL")
                    If newid = 0 Then newid = oCI.Lookup_ID("ProspectStatus", "DONOTCALL")
                    If newid > 0 Then
                        oP.StatusID = newid
                        oP.UserID = Session("UserDBID")
                        oP.Save()
                        lblAddStatus.Text = "The number has been added."
                        txtCheck.Text = Get_Number(txtNew.Text)
                        Lookup()
                    Else
                        'Cannot find StatusID
                        lblAddStatus.Text = "Unable to locate the appropriate record. Please contact support (Reason: Status 0 by Number)"
                    End If
                End If
            Else
                'Find the DO NOT CALL Prospect
                oP.Last_Name = "DO NOT CALL"
                oP.Load_By_LastName()
                If oP.Prospect_ID > 0 Then
                    'Add the number
                    oPP.ProspectID = oP.Prospect_ID
                    oPP.Number = Get_Number(txtNew.Text)
                    oPP.UserID = Session("UserDBID")
                    oPP.Save()
                    lblAddStatus.Text = "The number has been added."
                    txtCheck.Text = Get_Number(txtNew.Text)
                    Lookup()
                Else
                    'Cannot find the prospect record
                    lblAddStatus.Text = "Unable to locate the appropriate record. Please contact support (Reason: ProsID 0 by Last)"
                End If
            End If
            oP = Nothing
            oPP = Nothing
            oCI = Nothing
        Else
            lblAddStatus.Text = "Please enter the complete number. Format: 7573456760"
        End If
    End Sub

    Protected Sub btnCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheck.Click
        If txtCheck.Text <> "" And txtCheck.Text.Length > 9 Then
            Lookup()
        Else
            lblAddStatus.Text = "Please enter the complete number. Format: 7573456760"
        End If
    End Sub

    Private Sub Lookup()
        Dim oPP As New clsPhone
        btnExport.Enabled = False
        oPP.Number = Get_Number(txtCheck.Text)
        gvResults.DataSource = oPP.Lookup_By_Number
        gvResults.DataBind()
        lblAddStatus.Text = oPP.Error_Message
        oPP = Nothing
        MultiView1.ActiveViewIndex = 0
    End Sub
    Private Function Get_Number(ByVal sNumber As String) As String
        Dim ret As String = ""
        For i = 1 To Len(sNumber)
            If IsNumeric(Right(Left(sNumber, i), 1)) Then
                ret &= Right(Left(sNumber, i), 1)
            End If
        Next
        Return ret
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblAddStatus.Text = ""
        If Not (IsPostBack) Then
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub gvResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResults.RowDataBound
        Dim gvr As GridViewRow = e.Row
        If gvr.Cells.Count > 2 Then
            If gvr.Cells(gvr.Cells.Count - 3).Text <> "Do Not Call" And gvr.Cells(gvr.Cells.Count - 3).Text <> "Status" Then
                e.Row.Visible = False
            Else
                If gvr.Cells(gvr.Cells.Count - 3).Text <> "Status" Then
                    btnExport.Enabled = True
                End If
            End If
            
        End If
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Response.ClearHeaders()
        Response.AppendHeader("Content-Disposition", "attachment; filename=DNC.xls")
        Response.ContentType = "application/vnd.ms-excel"
        Response.Write("<html><head><title>Do Not Call</title></head><body><table>")

        Response.Write("<tr>")
        For x = 0 To gvResults.HeaderRow.Cells.Count - 1
            Response.Write("<th>" & gvResults.HeaderRow.Cells(x).Text & "</th>")
        Next
        Response.Write("</tr>")

        For i = 0 To gvResults.Rows.Count - 1
            If gvResults.Rows(i).Visible Then
                Response.Write("<tr>")
                For x = 0 To gvResults.Rows(i).Cells.Count - 1
                    Response.Write("<td>" & gvResults.Rows(i).Cells(x).Text & "</td>")
                Next
                Response.Write("</tr>")
            End If
        Next
        Response.Write("</table></body></html>")
        Response.End()
    End Sub

   
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        Dim cn As New System.Data.SqlClient.SqlConnection(Resources.Resource.cns)
        Dim cm As New System.Data.SqlClient.SqlCommand("Select pp.Number from t_Prospect p inner join t_ProspectPhone pp on pp.prospectid = p.prospectid where  p.statusid in (Select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname = 'ProspectStatus' and i.comboitem in('Do Not Call','DoNotCall')) and LEN(number)=10", cn)
        Dim dr As System.Data.SqlClient.SqlDataReader
        Dim bFinish As Boolean = False
        Try
            cn.Open()
            dr = cm.ExecuteReader
            If dr.HasRows Then
                Write_Export_Header()
                Response.Write("Number" & vbCrLf)
                While dr.Read
                    Write_Export_Line(dr("Number"))
                End While
                bFinish = True
            End If
            dr.Close()
            cn.Close()
        Catch ex As Exception
            lblAddStatus.Text = ex.ToString
        Finally
            If Not (dr Is Nothing) Then
                If Not (dr.IsClosed) Then dr.Close()
            End If
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            dr = Nothing
            cm = Nothing
            cn = Nothing
            If bFinish Then
                Dim oEvent As New clsEvents
                Dim sErr As String = ""
                oEvent.Create_View_Event("InternalDNC", 0, 0, Session("UserDBID"), sErr)
                oEvent = Nothing
                Write_Export_Footer()
            End If
        End Try
    End Sub

    Private Sub Write_Export_Header()
        Response.ClearHeaders()
        Response.AppendHeader("Content-Disposition", "attachment; filename=DNC.csv")
        Response.ContentType = "text/plain"
    End Sub

    Private Sub Write_Export_Line(ByVal sLine As String)
        Response.Write(sLine & vbCrLf)
    End Sub

    Private Sub Write_Export_Footer()
        Response.End()
    End Sub

    'Protected Sub LinkButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton2.Click
    '    MultiView1.ActiveViewIndex = 1
    'End Sub

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        If file1.FileName = "" Then
            lblStatus.Text = "No File"
            Exit Sub
        End If
        Dim sParentPath As String = "\\rs-fs-01\uploads\" '"\\nndc\UploadedContracts\"
        Dim sFolder As String = ""
        Dim sMid As String = ""
        Dim sFileName As String = Left(file1.FileName, InStr(file1.FileName, ".") - 1)
        Dim sExt As String = Right(file1.FileName, Len(file1.FileName) - InStr(file1.FileName, ".") + 1)
        If sExt.ToUpper <> ".CSV" Then
            'ClientScript.RegisterClientScriptBlock(Me.GetType, "Type", "alert('Please select only comma separated value (CSV) files');", True)
            lblStatus.Text = "Please select only comma separated value (CSV) files"
            Exit Sub
        End If
        'save the file
        'Response.Write(UCase(_KeyField))
        sParentPath = "\\rs-fs-01\uploads\" '"\\nndc\UploadedContracts\"
        sFolder = "misprojects\"

        Dim i As Integer = 0
        Do While FileIO.FileSystem.FileExists(sParentPath & sFolder & sFileName & sMid & sExt)
            i += 1
            sMid = i.ToString
        Loop
        File1.SaveAs(sParentPath & sFolder & sFileName & sMid & sExt)
        hfFile.Value = sParentPath & sFolder & sFileName & sMid & sExt
        Read_First_Line()
        MultiView2.ActiveViewIndex = 0
    End Sub

    Private Sub Read_First_Line()
        lblStatus.Text = ""
        If FileIO.FileSystem.FileExists(hfFile.Value) Then
            Dim ts As IO.StreamReader = FileIO.FileSystem.OpenTextFileReader(hfFile.Value)
            Dim line1() As String = ts.ReadLine.Split(",")
            Dim li As New ListItem("Choose", -1)
            ddphone.Items.Clear()
            ddphone.items.add(li)
            For i = 0 To UBound(line1)
                ddphone.Items.Add(New ListItem(line1(i), i))
            Next
            ts.Close()
            ts = Nothing
        End If
    End Sub

    Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        'If ddPhone.SelectedItem.Text <> "Choose" Then
        '    If hfState.Value = 1 Or hfState.Value = "1" Then
        '        If siState.Selected_ID > 0 Then
        '            Dim oState As New clsStateDNCList
        '            oState.FileID = 0
        '            oState.Load()
        '            oState.FileName = hfFile.Value.ToString
        '            oState.PhoneColumn = ddPhone.SelectedValue
        '            oState.Header = cbHeaders.Checked
        '            oState.Replace = cbReplace.Checked
        '            oState.StateID = siState.Selected_ID
        '            oState.Status = "Uploaded"
        '            oState.UserDBID = CType(Session("User"), User).PersonnelID
        '            oState.UserID = CType(Session("User"), User).PersonnelID
        '            oState.DateUploaded = Date.Now
        '            oState.Save()
        '            MultiView1.ActiveViewIndex = 1
        '            MultiView2.ActiveViewIndex = 1
        '            gvFiles.DataSource = oState.List_files("")
        '            gvFiles.DataBind()
        '            oState = Nothing
        '            ddPhone.Items.Clear()
        '        Else
        '            ClientScript.RegisterClientScriptBlock(Me.GetType, "col", "alert('Please select a state.');", True)
        '        End If
        '    Else
        '        Dim oDNC As New clsDNCScrub
        '        oDNC.FileID = 0
        '        oDNC.Load()
        '        oDNC.FileName = hfFile.Value.ToString
        '        oDNC.PhoneColumn = ddPhone.SelectedValue
        '        oDNC.Header = cbHeaders.Checked
        '        oDNC.Status = "Uploaded"
        '        oDNC.UserDBID = CType(Session("User"), User).PersonnelID
        '        oDNC.UserID = CType(Session("User"), User).PersonnelID
        '        oDNC.DateUploaded = Date.Now
        '        oDNC.Save()
        '        MultiView1.ActiveViewIndex = 1
        '        MultiView2.ActiveViewIndex = 1
        '        gvFiles.DataSource = oDNC.List_files("")
        '        gvFiles.DataBind()
        '        oDNC = Nothing
        '        ddPhone.Items.Clear()
        '    End If

        '    Else
        '        ClientScript.RegisterClientScriptBlock(Me.GetType, "col", "alert('Please select the column containing the phone number.');", True)
        '    End If
    End Sub

    Private Function Build_SQL() As String
        Dim sRet As String = "CREATE TABLE #TempTable ("
        'get columns
        If FileIO.FileSystem.FileExists(hfFile.Value) Then
            For i = 0 To ddPhone.Items.Count - 1
                If ddPhone.Items(i).Text <> "Choose" Then sRet &= IIf(i > 0 And Right(sRet, 1) <> "(", ",", "") & "[" & ddPhone.Items(i).Text.Replace(" ", "_") & "] nvarchar(max)"
            Next
        End If
        sRet &= "); " & _
            "BULK INSERT #TempTable  " & _
            "        FROM '" & hfFile.Value & "'  " & _
            "WITH ( " & _
                "FIELDTERMINATOR = ',', " & _
                "ROWTERMINATOR = '\n', " & _
                "ERRORFILE = 'C:\myRubbishData.log'  " & _
                "); " & _
            "update #TempTable set [" & ddPhone.SelectedItem.Text.Replace(" ", "_") & "] = REPLACE([" & ddPhone.SelectedItem.Text.Replace(" ", "_") & "],'(',''); " & _
            "update #TempTable set [" & ddPhone.SelectedItem.Text.Replace(" ", "_") & "] = REPLACE([" & ddPhone.SelectedItem.Text.Replace(" ", "_") & "],')',''); " & _
            "update #TempTable set [" & ddPhone.SelectedItem.Text.Replace(" ", "_") & "] = REPLACE([" & ddPhone.SelectedItem.Text.Replace(" ", "_") & "],'-',''); " & _
            "update #TempTable set [" & ddPhone.SelectedItem.Text.Replace(" ", "_") & "] = REPLACE([" & ddPhone.SelectedItem.Text.Replace(" ", "_") & "],'.',''); " & _
            "update #TempTable set [" & ddPhone.SelectedItem.Text.Replace(" ", "_") & "] = LEFT(RTRIM(ltrim([" & ddPhone.SelectedItem.Text.Replace(" ", "_") & "])),10); " & _
            "         " & _
            "select distinct t.*, " & _
            "	case when f.areacode IS null then 0 else 1 end as FedDNC, " & _
            "	case when internal.Number IS null then 0 else 1 end as InternalDNC " & _
            "from #TempTable t " & _
            "	left outer join t_FedDNC2 f  on f.areacode = LEFT(t.[" & ddPhone.SelectedItem.Text.Replace(" ", "_") & "],3) and f.number = RIGHT(t.[" & ddPhone.SelectedItem.Text.Replace(" ", "_") & "],len(t.[" & ddPhone.SelectedItem.Text.Replace(" ", "_") & "])-3) " & _
            "	left outer join ( " & _
            "  Select pp.Number " & _
            "		from t_Prospect p  " & _
            "			inner join t_ProspectPhone pp on pp.prospectid = p.prospectid  " & _
            "		where  p.statusid in ( " & _
            "				select comboitemid  " & _
            "				from t_Comboitems i  " & _
            "					inner join t_Combos c on c.comboid = i.comboid  " & _
            "				where c.comboname = 'ProspectStatus'  " & _
            "					and i.comboitem in('Do Not Call','DoNotCall') " & _
            "				) and LEN(number)=10 " & _
            "		) internal on rtrim(ltrim(internal.Number)) = t." & ddPhone.SelectedItem.Text.Replace(" ", "_") & " "

        Return sRet
    End Function

    Protected Sub lbIndividual_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbIndividual.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub lbFiles_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFiles.Click
        MultiView1.ActiveViewIndex = 1
        hfState.Value = 0
    End Sub

    Protected Sub gvFiles_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFiles.RowDataBound
        If e.Row.Cells(4).Text <> "Completed" And e.Row.Cells(4).Text <> "Status" Then
            e.Row.Cells(0).Text = ""
            tmrRefresh.Enabled = True
        ElseIf e.Row.Cells(4).Text <> "Status" Then
            e.Row.Cells(0).Text = "<a href='dncdownload.aspx?fileid=" & e.Row.Cells(1).Text & "'>Download</a>"
        End If
        If e.Row.Cells(3).Text <> "FileName" Then
            Dim tmp() As String = Split(e.Row.Cells(3).Text, "\")
            e.Row.Cells(3).Text = tmp(UBound(tmp))
        End If
    End Sub

    Protected Sub gvFiles_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFiles.SelectedIndexChanged

    End Sub

    Protected Sub lbCheckStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCheckStatus.Click
        RefreshStatus()
    End Sub

    Protected Sub tmrRefresh_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrRefresh.Tick
        tmrRefresh.Enabled = False
        RefreshStatus()
    End Sub

    Private Sub RefreshStatus()
        MultiView1.ActiveViewIndex = 1
        MultiView2.ActiveViewIndex = 1
        'If hfState.Value = 1 Or hfState.Value = "1" Then
        '    Dim oDNC As New clsStateDNCList
        '    oDNC.UserID = CType(Session("User"), User).PersonnelID
        '    gvFiles.DataSource = oDNC.List_files("")
        '    gvFiles.DataBind()
        '    oDNC = Nothing
        'Else
        '    Dim oDNC As New clsDNCScrub
        '    oDNC.UserID = CType(Session("User"), User).PersonnelID
        '    gvFiles.DataSource = oDNC.List_files("")
        '    gvFiles.DataBind()
        '    oDNC = Nothing
        'End If
    End Sub

    Protected Sub lbRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRefresh.Click
        RefreshStatus()
    End Sub

    Protected Sub lbUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbUpload.Click
        If CheckSecurity("DNC", "StateUpload", , , Session("UserDBID")) Then
            hfState.Value = 1
            MultiView1.ActiveViewIndex = 1
            siState.Connection_String = Resources.Resource.cns
            siState.ComboItem = "State"
            siState.Load_Items()
        Else
            hfState.Value = 0
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AD", "alert('Access Denied');", True)
        End If
    End Sub
End Class
