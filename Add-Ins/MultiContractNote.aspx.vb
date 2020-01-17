
Partial Class Add_Ins_MultiContractNote
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oCon As New clsContract
        If txtContract.Text = "" Or Not (oCon.Verify_Contract(txtContract.Text)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Valid Contract');", True)
        Else
            Dim conID As Integer
            Dim bProceed As Boolean = True
            conID = oCon.Get_Contract_ID(txtContract.Text)
            For i = 0 To lbContract.Items.Count - 1
                If lbContract.Items(i).Value = conID Then
                    bProceed = False
                    Exit For
                End If
            Next
            If bProceed Then
                lbContract.Items.Add(New ListItem(txtContract.Text, oCon.Get_Contract_ID(txtContract.Text)))
                txtContract.Text = ""
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('This Contract Has Already Been Added.');", True)
            End If
        End If
        oCon = Nothing
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        If lbContract.SelectedValue <> "" Then
            lbContract.Items.Remove(lbContract.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        If lbContract.Items.Count = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Valid Contract');", True)
        ElseIf txtNote.Text = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Valid Note');", True)
        ElseIf cbOwner.Checked = False And cbContract.Checked = False Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Select Where to Add The Note.');", True)
        Else
            Dim oCon As New clsContract
            Dim oNote As New clsNotes
            For i = 0 To lbContract.Items.Count - 1
                If cbContract.Checked Then
                    oNote.NoteID = 0
                    oNote.Load()
                    oNote.KeyField = "ContractID"
                    oNote.KeyValue = lbContract.Items(i).value
                    oNote.Note = txtNote.Text
                    oNote.UserID = Session("UserDBID")
                    oNote.DateCreated = System.DateTime.Now
                    oNote.Save()
                End If
                If cbOwner.Checked Then
                    oCon.ContractID = lbContract.Items(i).Value
                    oCon.Load()
                    oNote.NoteID = 0
                    oNote.Load()
                    oNote.KeyField = "ProspectID"
                    oNote.KeyValue = oCon.ProspectID
                    oNote.Note = txtNote.Text
                    oNote.UserID = Session("UserDBID")
                    oNote.DateCreated = System.DateTime.Now
                    oNote.Save()
                End If
            Next
            oCon = Nothing
            oNote = Nothing
            txtNote.Text = ""
            lbContract.Items.Clear()
            txtContract.Text = ""
        End If
    End Sub

    Protected Sub rblType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblType.SelectedIndexChanged
        Select Case rblType.SelectedValue.ToUpper
            Case "CSV"
                MultiView1.ActiveViewIndex = 1
                MultiView2.ActiveViewIndex = 0
            Case "KCP"
                MultiView1.ActiveViewIndex = 0
            Case Else

        End Select
    End Sub

    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        If file1.FileName = "" Then
            lblStatus.Text = "No File"
            Exit Sub
        End If
        Dim sParentPath As String = "\\nndc\UploadedContracts\"
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
        sParentPath = "\\nndc\UploadedContracts\"
        sFolder = "misprojects\"

        Dim i As Integer = 0
        Do While FileIO.FileSystem.FileExists(sParentPath & sFolder & sFileName & sMid & sExt)
            i += 1
            sMid = i.ToString
        Loop
        file1.SaveAs(sParentPath & sFolder & sFileName & sMid & sExt)
        hfFile.Value = sParentPath & sFolder & sFileName & sMid & sExt
        Read_First_Line()
        MultiView2.ActiveViewIndex = 0
    End Sub

    Private Sub Read_First_Line()
        lblStatus.Text = ""
        If FileIO.FileSystem.FileExists(hfFile.Value) Then
            Dim ts As IO.StreamReader = FileIO.FileSystem.OpenTextFileReader(hfFile.Value)
            Dim line1() As String = ts.ReadLine.Split(",")
            Dim li As New ListItem("Clear", -1)
            ddKCP.Items.Clear()
            ddNote.Items.Clear()
            ddKCPID.Items.Clear()
            For i = 0 To UBound(line1)
                ddKCPID.Items.Add(New ListItem(line1(i), i))
                ddKCP.Items.Add(New ListItem(line1(i), i))
                ddNote.Items.Add(New ListItem(line1(i), i))               
            Next
            ts.Close()
            ts = Nothing
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            MultiView1.ActiveViewIndex = 0
            rblType.SelectedIndex = 1
        End If
    End Sub

    Protected Sub lbProcess_Click(sender As Object, e As EventArgs) Handles lbProcess.Click
        Dim sErr As String = ""

        Select Case rblType.SelectedValue.ToUpper
            Case "KCP"
                Unnamed3_Click(Nothing, EventArgs.Empty)
            Case "CSV"
                If Not (Validate_CSV(sErr)) Then
                    'ClientScript.RegisterClientScriptBlock(Me.GetType, "Error", "alert('" & sErr & "');", True)
                    lblStatus.Text = sErr
                Else
                    Process_File()
                End If
            Case Else
                'ClientScript.RegisterClientScriptBlock(Me.GetType, "Error", "alert('Please select either Upload CSV or Enter Contract Numbers');", True)
                lblStatus.Text = "Please select either Upload CSV or Enter Contract Numbers"
        End Select
    End Sub

    Private Function Validate_CSV(ByRef sErr As String) As Boolean
        If hfFile.Value = "" Then
            sErr = "Please upload a file."
        ElseIf (cbKCPID.Checked And cbKCP.Checked) Or (cbKCPID.Checked = False And cbKCP.Checked = False) Then
            sErr = "Please use either ContractID or Contract #."
        ElseIf cbNote.Checked Then
            sErr = "Must leave Contract Note checkbox un-checked"
        End If
        Return sErr = ""
    End Function

    Private Sub Process_File()
        If FileIO.FileSystem.FileExists(hfFile.Value) Then
            Dim ts As IO.StreamReader = FileIO.FileSystem.OpenTextFileReader(hfFile.Value)
            Dim line1() As String = ts.ReadLine.Split(",")
            Dim counter As Long = 0
            While Not (ts.EndOfStream)
                Dim line() As String = ts.ReadLine.Split(",")
                Dim oCont As New clsContract
                Dim oNote As New clsNotes

                If cbKCPID.Checked Then
                    oCont.ContractID = oCont.Get_Contract_ID(line(ddKCP.SelectedValue))                   
                Else
                    oCont.ContractID = line(ddKCPID.SelectedValue)                    
                End If

                oCont.Load()
                
                If oCont.ContractID > 0 Then

                    With oNote
                        .NoteID = 0
                        .CreatedByID = Session("UserDBID")
                        .KeyField = "contractid"
                        .KeyValue = oCont.ContractID
                        .Note = IIf(cbNote.Checked = False, line(ddNote.SelectedValue), tbNote.Text.Trim())
                        .UserID = Session("UserDBID")
                        .DateCreated = DateTime.Now
                        .Save()
                    End With
                    counter += 1
                    oNote = Nothing

                    If cbOwner2.Checked Then
                        oNote = New clsNotes

                        oNote.NoteID = 0
                        oNote.Load()
                        oNote.KeyField = "ProspectID"
                        oNote.KeyValue = oCont.ProspectID
                        oNote.Note = IIf(cbNote.Checked = False, line(ddNote.SelectedValue), tbNote.Text.Trim())
                        oNote.UserID = Session("UserDBID")
                        oNote.DateCreated = System.DateTime.Now
                        oNote.Save()

                        oNote = Nothing
                    End If

                End If

              

               

             
                oCont = Nothing
            End While
            ts.Close()
            ts = Nothing
            lblStatus.Text = "Completed (" & counter & " contracts updated)"
        End If
    End Sub
End Class
