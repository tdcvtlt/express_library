
Partial Class Add_Ins_MultiContractRestrictor
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oRestrictor As New clsUsageRestriction
            ddRestrictor.DataSource = oRestrictor.Get_Restrictions()
            ddRestrictor.DataTextField = "Name"
            ddRestrictor.DataValueField = "UsageRestrictionID"
            ddRestrictor.DataBind()

            oRestrictor = Nothing

            rblType.SelectedIndex = 1
            mvContainer.SetActiveView(vwContract)
        End If

        lblErr.Text = ""
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        lbRestrictor.Items.Add(New ListItem(ddRestrictor.SelectedItem.Text, ddRestrictor.SelectedValue))
        ddRestrictor.Items.Remove(ddRestrictor.SelectedItem)
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        If lbRestrictor.SelectedValue <> "" Then
            ddRestrictor.Items.Add(New ListItem(lbRestrictor.SelectedItem.Text, lbRestrictor.SelectedValue))
            lbRestrictor.Items.Remove(lbRestrictor.SelectedItem)
        End If
    End Sub

    Protected Sub AddCon_Click(sender As Object, e As System.EventArgs) Handles AddCon.Click
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

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        If lbRestrictor.Items.Count = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Select A Restrictor.');", True)
        ElseIf lbContract.Items.Count = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Contract.');", True)
        Else
            Dim oRestrict As New clsUsageRestriction2Contract
            For i = 0 To lbRestrictor.Items.Count - 1
                For j = 0 To lbContract.Items.Count - 1
                    oRestrict.UsageRestriction2ContractID = 0
                    oRestrict.Load()
                    oRestrict.UsageRestrictionID = lbRestrictor.Items(i).Value
                    oRestrict.ContractID = lbContract.Items(j).Value
                    oRestrict.PersonnelID = Session("UserDBID")
                    oRestrict.DateCreated = System.DateTime.Now
                    oRestrict.Save()
                Next j
            Next i
            oRestrict = Nothing
            lbRestrictor.Items.Clear()
            lbContract.Items.Clear()
            Dim oRestrictor As New clsUsageRestriction
            ddRestrictor.DataSource = oRestrictor.Get_Restrictions()
            ddRestrictor.DataTextField = "Name"
            ddRestrictor.DataValueField = "UsageRestrictionID"
            ddRestrictor.DataBind()
            oRestrictor = Nothing
        End If
    End Sub

    Protected Sub RemCon_Click(sender As Object, e As System.EventArgs) Handles RemCon.Click
        If lbContract.SelectedValue <> "" Then
            lbContract.Items.Remove(lbContract.SelectedItem)
        End If
    End Sub

    Protected Sub rblType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblType.SelectedIndexChanged
        Select Case rblType.SelectedValue.ToUpper
            Case "CSV"
                mvContainer.SetActiveView(vwCsv)
            Case "KCP"
                mvContainer.SetActiveView(vwContract)
            Case Else
        End Select
    End Sub

    Private Function Validate_KCP(ByRef err As String) As Boolean
        If mvContainer.GetActiveView().Equals(vwCsv) Then
            If (cbKCP.Checked And cbKCPID.Checked) Or (cbKCP.Checked = False And cbKCPID.Checked = False) Then
                err = "Select either ContractID Column OR Contract # Column, not both."
                Return False
            Else
                Return True
            End If
        Else
            Return True
        End If
    End Function

    Protected Sub lbProcess_Click(sender As Object, e As EventArgs) Handles lbProcess.Click
        If mvContainer.GetActiveView().Equals(vwCsv) Then
            If Validate_KCP(lblErr.Text) Then
                Process_File()
            End If
        ElseIf mvContainer.GetActiveView().Equals(vwContract) Then
            Unnamed3_Click(Nothing, e)
        End If
    End Sub

    Private Sub Process_File()
        If FileIO.FileSystem.FileExists(hfFile.Value) Then
            Dim counter As Long = 0
            Dim ts As IO.StreamReader = FileIO.FileSystem.OpenTextFileReader(hfFile.Value)
            Try

                Dim line1() As String = ts.ReadLine.Split(",")

                While Not (ts.EndOfStream)
                    Dim line() As String = ts.ReadLine.Split(",")
                    
                    Try
                        For Each li As ListItem In ddRestrictor.Items
                            If li.Text.Trim().ToLower() = line(ddlRestrictor.SelectedValue).ToLower() Then
                                With New clsUsageRestriction2Contract
                                    .UsageRestriction2ContractID = 0

                                    If cbKCPID.Checked Then
                                        .ContractID = New clsContract().Get_Contract_ID(line(ddKCP.SelectedValue))
                                    Else
                                        .ContractID = New clsContract().Get_Contract_ID(line(ddKCPID.SelectedValue))
                                    End If

                                    .DateCreated = DateTime.Now
                                    .UsageRestrictionID = li.Value
                                    .PersonnelID = Session("UserDBID")

                                    .Save()
                                    counter += 1
                                    Exit For
                                End With
                            End If
                        Next
                    Catch ex As Exception
                    End Try
                End While
            Catch ex As Exception
                lblErr.Text = ex.Message
            Finally
                ts.Close()
                lblErr.Text = "Completed (" & counter & " contracts updated)"
            End Try
        End If
    End Sub

    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        If file1.FileName = "" Then
            lblErr.Text = "No File"
            Return
        End If

        Dim sParentPath As String = "\\nndc\UploadedContracts\"
        Dim sFolder As String = ""
        Dim sMid As String = ""
        Dim sFileName As String = Left(file1.FileName, InStr(file1.FileName, ".") - 1)
        Dim sExt As String = Right(file1.FileName, Len(file1.FileName) - InStr(file1.FileName, ".") + 1)

        If sExt.ToUpper <> ".CSV" Then            
            lblErr.Text = "Please select only comma separated value (CSV) files"
            Exit Sub
        End If
       
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
    End Sub

    Private Sub Read_First_Line()
        lblErr.Text = ""
        If FileIO.FileSystem.FileExists(hfFile.Value) Then
            Dim ts As IO.StreamReader = FileIO.FileSystem.OpenTextFileReader(hfFile.Value)
            Dim line1() As String = ts.ReadLine.Split(",")
            Dim li As New ListItem("Clear", -1)
            ddKCP.Items.Clear()
            ddKCPID.Items.Clear()
            ddlRestrictor.Items.Clear()

            For i = 0 To UBound(line1)
                ddKCPID.Items.Add(New ListItem(line1(i), i))
                ddKCP.Items.Add(New ListItem(line1(i), i))
                ddlRestrictor.Items.Add(New ListItem(line1(i), i))
            Next
            ts.Close()
            ts = Nothing
        End If
    End Sub
End Class
