
Partial Class Add_Ins_MultiContractStatusUpdate
    Inherits System.Web.UI.Page

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
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
            ddMF.Items.Clear()
            ddNCS.Items.Clear()
            ddNCSS.Items.Clear()
            ddMS.Items.Clear()
            ddCvS.Items.Clear()
            ddKCPID.Items.Clear()
            For i = 0 To UBound(line1)
                ddKCPID.Items.Add(New ListItem(line1(i), i))
                ddKCP.Items.Add(New ListItem(line1(i), i))
                ddMF.Items.Add(New ListItem(line1(i), i))
                ddNCS.Items.Add(New ListItem(line1(i), i))
                ddNCSS.Items.Add(New ListItem(line1(i), i))
                ddMS.Items.Add(New ListItem(line1(i), i))
                ddCvS.Items.Add(New ListItem(line1(i), i))
            Next
            ddMF.Items.Add(li)
            ddNCSS.Items.Add(li)
            ts.Close()
            ts = Nothing
        End If
    End Sub

    Private Sub Process_File()
        If FileIO.FileSystem.FileExists(hfFile.Value) Then
            Dim ts As IO.StreamReader = FileIO.FileSystem.OpenTextFileReader(hfFile.Value)
            Dim line1() As String = ts.ReadLine.Split(",")
            Dim counter As Long = 0
            While Not (ts.EndOfStream)
                Dim line() As String = ts.ReadLine.Split(",")
                Dim oCont As New clsContract
                Dim oCI As New clsComboItems
                If cbKCPID.Checked Then
                    oCont.ContractID = oCont.Get_Contract_ID(line(ddKCP.SelectedValue))
                Else
                    oCont.ContractID = line(ddKCPID.SelectedValue)
                End If

                oCont.Load()
                oCont.IgnoreStatusDate = cbIgnoreStatusDate.Checked
                oCont.UserID = Session("UserDBID")
                If Not (cbNCS.Checked) Then
                    If oCI.Lookup_ID("ContractStatus", line(ddNCS.SelectedValue)) <> 0 Then
                        oCont.StatusID = oCI.Lookup_ID("ContractStatus", line(ddNCS.SelectedValue))
                    End If
                End If
                If Not (cbNCSS.Checked) Then
                    If ddNCSS.SelectedValue = "-1" Then
                        oCont.SubStatusID = 0
                    Else
                        oCont.SubStatusID = oCI.Lookup_ID("ContractSubStatus", line(ddNCSS.SelectedValue))
                    End If
                End If
                If Not (cbNMF.Checked) Then
                    If ddMF.SelectedValue = "-1" Then
                        oCont.MaintenanceFeeStatusID = 0
                    Else
                        oCont.MaintenanceFeeStatusID = oCI.Lookup_ID("MaintenanceFeeStatus", line(ddMF.SelectedValue))
                    End If
                End If
                If Not (cbNMS.Checked) Then
                    Dim oMort As New clsMortgage
                    oMort.ContractID = oCont.ContractID
                    oMort.MortgageID = 0
                    oMort.Load()
                    oMort.UserID = Session("UserDBID")
                    oMort.IgnoreStatusDate = cbIgnoreStatusDate.Checked
                    If ddMS.SelectedValue = -1 Then
                        oMort.StatusID = 0
                    Else
                        oMort.StatusID = oCI.Lookup_ID("MortgageStatus", line(ddMS.SelectedValue))
                    End If
                    oMort.Save()
                    oMort = Nothing
                End If
                If Not (cbNCvS.Checked) Then
                    Dim oConv As New clsConversion
                    If oConv.get_Conversion_ID(oCont.ContractID) > 0 Then
                        oConv.ConversionID = oConv.get_Conversion_ID(oCont.ContractID)
                        oConv.Load()
                        If ddCvS.SelectedValue = "-1" Then
                            oConv.StatusID = 0
                        Else
                            oConv.StatusID = oCI.Lookup_ID("ConversionStatus", line(ddCvS.SelectedValue))
                        End If
                        oConv.UserID = Session("UserDBID")
                        oConv.Save()
                    End If
                    oConv = Nothing
                End If
                oCont.Save()
                counter += 1
                oCI = Nothing
                oCont = Nothing
            End While
            ts.Close()
            ts = Nothing
            lblStatus.Text = "Completed (" & counter & " contracts updated)"
        End If
    End Sub

    Private Sub Process_Manually()
        Dim counter As Long = 0
        For i = 0 To lstKCP.Items.Count - 1
            Dim oCont As New clsContract
            oCont.ContractID = oCont.Get_Contract_ID(lstKCP.Items(i).Text)
            If oCont.ContractID > 0 Then
                oCont.Load()
                oCont.UserID = Session("UserDBID")
                If cbCS.Checked Then oCont.StatusID = siContractStatus.Selected_ID
                If cbCSS.Checked Then oCont.SubStatusID = siContractSubStatus.Selected_ID
                If cbMFS.Checked Then oCont.MaintenanceFeeStatusID = siMaintenanceFeeStatus.Selected_ID

                If cbMS.Checked Then
                    With New clsMortgage
                        .ContractID = oCont.ContractID
                        .Load()
                        .StatusID = siMortgageStatus.Selected_ID
                        .UserID = Session("UserDBID")
                        .Save()
                    End With
                End If

                If cbCvS.Checked Then
                    Dim oConv As New clsConversion
                    If oConv.get_Conversion_ID(oCont.ContractID) > 0 Then
                        oConv.ConversionID = oConv.get_Conversion_ID(oCont.ContractID)
                        oConv.Load()
                        oConv.StatusID = siConversionStatus.Selected_ID
                        oConv.UserID = Session("UserDBID")
                        oConv.Save()
                    End If
                    oConv = Nothing
                End If
                oCont.Save()
                counter += 1
            End If
            oCont = Nothing
        Next
        lblStatus.Text = "Completed (" & counter & " contracts updated)"
    End Sub

    Protected Sub lbProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbProcess.Click
        Dim sErr As String = ""

        Select Case rblType.SelectedValue.ToUpper
            Case "KCP"
                If Not (Validate_KCP(sErr)) Then
                    'ClientScript.RegisterClientScriptBlock(Me.GetType, "Error", "alert('" & sErr & "');", True)
                    lblStatus.Text = sErr
                Else
                    Process_Manually()
                End If
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
        If lstKCP.Items.Count > 0 Then

        End If

    End Sub

    Private Function Validate_CSV(ByRef sErr As String) As Boolean
        If hfFile.Value = "" Then
            sErr = "Please upload a file."
        ElseIf cbKCPID.Checked And cbKCP.Checked Then
            sErr = "Please use either ContractID or Contract #."
        ElseIf cbNCS.Checked And cbNCSS.Checked And cbNMF.Checked And cbNMS.Checked And cbNCvS.Checked Then
            sErr = "Must have at least 1 status to update."
        Else
            Dim kcp() As String
            kcp = Split(ddKCPID.SelectedValue & "," & ddKCP.SelectedValue & "," & ddNCS.SelectedValue & "," & ddNCSS.SelectedValue & "," & ddMF.SelectedValue & "," & ddMS.SelectedValue & "," & ddCvS.SelectedValue, ",")

            Dim KCPID As Boolean = cbKCPID.Checked
            Dim NMS As Boolean = cbNMS.Checked
            Dim NCS As Boolean = cbNCS.Checked
            Dim NCSS As Boolean = cbNCSS.Checked
            Dim NMF As Boolean = cbNMF.Checked
            Dim NCVS As Boolean = cbNCvS.Checked

            If NMS And NCS And NCSS And NMF And NCVS Then
                'Ignore all -- Caught above

            ElseIf NMS And NCS And NCSS And NMF And Not NCVS Then
                'Update Conversion Status only
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddCvS.SelectedValue, ",")
            ElseIf NMS And NCS And NCSS And Not NMF And NCVS Then
                'Update MF Status Only
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddMF.SelectedValue, ",")
            ElseIf NMS And NCS And NCSS And Not NMF And Not NCVS Then
                'Update MF Status and Conversion Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddMS.SelectedValue & "," & ddCvS.SelectedValue, ",")
            ElseIf NMS And NCS And Not NCSS And NMF And NCVS Then
                'Update Contract Sub Status Only
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCSS.SelectedValue, ",")
            ElseIf NMS And NCS And Not NCSS And NMF And Not NCVS Then
                'Update Contract Sub Status and Conversion Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCSS.SelectedValue & "," & ddCvS.SelectedValue, ",")
            ElseIf NMS And NCS And Not NCSS And Not NMF And NCVS Then
                'Update Constract Sub Status and MF Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCSS.SelectedValue & "," & ddMF.SelectedValue, ",")
            ElseIf NMS And NCS And Not NCSS And Not NMF And Not NCVS Then
                'Update Constract Sub Status, MF Status, and Conversion Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCSS.SelectedValue & "," & ddMF.SelectedValue & "," & ddCvS.SelectedValue, ",")
            ElseIf NMS And Not NCS And NCSS And NMF And NCVS Then
                'Update Contract Status Only
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue, ",")
            ElseIf NMS And Not NCS And NCSS And NMF And Not NCVS Then
                'Update Contract Status and Conversion Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue & "," & ddCvS.SelectedValue, ",")
            ElseIf NMS And Not NCS And NCSS And Not NMF And NCVS Then
                'Update Contract Status and MF Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue & "," & ddMF.SelectedValue, ",")
            ElseIf NMS And Not NCS And NCSS And Not NMF And Not NCVS Then
                'Update Contract Status, MF Status, and Conversion Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue & "," & ddMF.SelectedValue & "," & ddCvS.SelectedValue, ",")
            ElseIf NMS And Not NCS And Not NCSS And NMF And NCVS Then
                'Update Contract Status, Contract Sub Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue & "," & ddNCSS.SelectedValue, ",")
            ElseIf NMS And Not NCS And Not NCSS And NMF And Not NCVS Then
                'Update Contract Status, Contract Sub Status and Conversion Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue & "," & ddNCSS.SelectedValue & ddCvS.SelectedValue, ",")
            ElseIf NMS And Not NCS And Not NCSS And Not NMF And NCVS Then
                'Update Contract Status, Contract Sub Status, and MF Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue & "," & ddNCSS.SelectedValue & "," & ddMF.SelectedValue, ",")
            ElseIf NMS And Not NCS And Not NCSS And Not NMF And Not NCVS Then
                'Update Contract Status, Contract Sub Status, MF Status, and Conversion Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue & "," & ddNCSS.SelectedValue & "," & ddMF.SelectedValue & "," & ddCvS.SelectedValue, ",")
            ElseIf Not NMS And NCS And NCSS And NMF And NCVS Then
                'Update Mortgage Status only
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddMS.SelectedValue, ",")
            ElseIf Not NMS And NCS And NCSS And NMF And Not NCVS Then
                'Update Mortgage Status and Conversion Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddMS.SelectedValue & "," & ddCvS.SelectedValue, ",")
            ElseIf Not NMS And NCS And NCSS And Not NMF And NCVS Then
                'Update Mortgage Status and MF Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddMF.SelectedValue & "," & ddMS.SelectedValue, ",")
            ElseIf Not NMS And NCS And NCSS And Not NMF And Not NCVS Then
                'Update Mortgage Status, MF Status and Conversion Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddMF.SelectedValue & "," & ddMS.SelectedValue & "," & ddCvS.SelectedValue, ",")
            ElseIf Not NMS And NCS And Not NCSS And NMF And NCVS Then
                'Update Mortgage Status and Contract Sub Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCSS.SelectedValue & "," & ddMS.SelectedValue, ",")
            ElseIf Not NMS And NCS And Not NCSS And NMF And Not NCVS Then
                'Update Mortgage Status, Contract Sub Status and Conversion Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCSS.SelectedValue & "," & ddMS.SelectedValue & "," & ddCvS.SelectedValue, ",")
            ElseIf Not NMS And NCS And Not NCSS And Not NMF And NCVS Then
                'Update Mortgage Status, Contract Sub Status and MF Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCSS.SelectedValue & "," & ddMF.SelectedValue & "," & ddMS.SelectedValue, ",")
            ElseIf Not NMS And NCS And Not NCSS And Not NMF And Not NCVS Then
                'Update Mortgage Status, Contract Sub Status, MF Status, and Conversion Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCSS.SelectedValue & "," & ddMF.SelectedValue & "," & ddMS.SelectedValue & "," & ddCvS.SelectedValue, ",")
            ElseIf Not NMS And Not NCS And NCSS And NMF And NCVS Then
                'Update Mortgage Status and Contract Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue & "," & ddMS.SelectedValue, ",")
            ElseIf Not NMS And Not NCS And NCSS And NMF And Not NCVS Then
                'Update Mortgage Status, Contract Status and Conversion Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue & "," & ddMS.SelectedValue & "," & ddCvS.SelectedValue, ",")
            ElseIf Not NMS And Not NCS And NCSS And Not NMF And NCVS Then
                'Update Mortgage Status, Contract Status and MF Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue & "," & ddMF.SelectedValue & "," & ddMS.SelectedValue, ",")
            ElseIf Not NMS And Not NCS And NCSS And Not NMF And Not NCVS Then
                'Update Mortgage Status, Contract Status, MF Status and Conversion Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue & "," & ddMF.SelectedValue & "," & ddMS.SelectedValue & "," & ddCvS.SelectedValue, ",")
            ElseIf Not NMS And Not NCS And Not NCSS And NMF And NCVS Then
                'Update Mortgage Status, Contract Status and Contract Sub Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue & "," & ddNCSS.SelectedValue & "," & ddMS.SelectedValue, ",")
            ElseIf Not NMS And Not NCS And Not NCSS And NMF And Not NCVS Then
                'Update Mortgage Status, Contract Status, Contract Sub Status and Conversion Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue & "," & ddNCSS.SelectedValue & "," & ddMS.SelectedValue & "," & ddCvS.SelectedValue, ",")
            ElseIf Not NMS And Not NCS And Not NCSS And Not NMF And NCVS Then
                'Update Mortgage Status, Contract Status, Contract Sub Status and MF Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue & "," & ddNCSS.SelectedValue & "," & ddMF.SelectedValue & "," & ddMS.SelectedValue, ",")
            ElseIf Not NMS And Not NCS And Not NCSS And Not NMF And Not NCVS Then
                'Update Mortgage Status, Contract Status, Contract Sub Status, MF Status and Conversion Status
                kcp = Split(IIf(KCPID, ddKCP.SelectedValue, ddKCPID.SelectedValue) & "," & ddNCS.SelectedValue & "," & ddNCSS.SelectedValue & "," & ddMF.SelectedValue & "," & ddMS.SelectedValue & "," & ddCvS.SelectedValue, ",")
            Else
                'Don't know what they did
            End If
            
            For i = 0 To UBound(kcp)
                For x = i + 1 To UBound(kcp)
                    If kcp(i) = kcp(x) And kcp(x) <> "-1" And kcp(x) <> -1 Then sErr = "All field mapping selections must be unique"
                Next
            Next
            End If
            Return sErr = ""
    End Function

    Private Function Validate_KCP(ByRef sErr As String) As Boolean
        If Not (cbCS.Checked) And Not (cbCSS.Checked) And Not (cbMFS.Checked) And Not (cbMS.Checked) And Not (cbCvS.Checked) Then
            sErr = "Must have at least 1 status to update"
        ElseIf Not (lstKCP.Items.Count > 0) Then
            sErr = "Must have at least 1 contract to update"
        Else
            'sErr = "Things are enabled.. check selections"
            If cbCS.Checked And siContractStatus.Selected_ID = 0 Then
                sErr = "Must have a new status chosen"
            ElseIf cbCSS.Checked Then

            ElseIf cbMFS.Checked Then

            ElseIf cbMS.Checked Then

            Else
                'sErr = "Things appear to be ok to continue"
            End If
        End If
        Return sErr = ""
    End Function

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtKCP.Text <> "" Then
            Dim oCont As New clsContract
            If oCont.Get_Contract_ID(txtKCP.Text) > 0 Then
                If lstKCP.Items.Count > 0 Then
                    For i = 0 To lstKCP.Items.Count - 1
                        If lstKCP.Items(i).Text = txtKCP.Text Then
                            'ClientScript.RegisterClientScriptBlock(Me.GetType, "Exists", "alert('" & txtKCP.Text & " is already in the list.');", True)
                            lblStatus.Text = txtKCP.Text & " is already in the list."
                            Exit Sub
                        End If
                    Next
                End If
                lstKCP.Items.Add(txtKCP.Text)
                txtKCP.Text = ""
            Else
                'ClientScript.RegisterClientScriptBlock(Me.GetType, "Exists", "alert('" & txtKCP.Text & " is not a valid contract number.');", True)
                lblStatus.Text = txtKCP.Text & " is not a valid contract number."
            End If
        End If
            Toggle_DropDowns()
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        If lstKCP.Items.Count > 0 Then
            If lstKCP.SelectedIndex > -1 Then
                lstKCP.Items.Remove(lstKCP.SelectedItem)
            End If
        End If
        Toggle_DropDowns()
    End Sub

    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblType.SelectedIndexChanged
        Select Case rblType.SelectedValue.ToUpper
            Case "CSV"
                MultiView1.ActiveViewIndex = 1
            Case "KCP"
                MultiView1.ActiveViewIndex = 0
            Case Else

        End Select
    End Sub

    Protected Sub cbCS_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbCS.CheckedChanged
        Toggle_DropDowns()
    End Sub

    Protected Sub cbCSS_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbCSS.CheckedChanged
        Toggle_DropDowns()
    End Sub

    Protected Sub cbMFS_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbMFS.CheckedChanged
        Toggle_DropDowns()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Load_Items()
            Toggle_DropDowns()
        End If
        lblStatus.Text = ""
        
    End Sub

    Private Sub Load_Items()
        siContractStatus.Connection_String = Resources.Resource.cns
        siContractSubStatus.Connection_String = Resources.Resource.cns
        siMaintenanceFeeStatus.Connection_String = Resources.Resource.cns
        siMortgageStatus.Connection_String = Resources.Resource.cns
        siConversionStatus.Connection_String = Resources.Resource.cns

        siContractStatus.ComboItem = "ContractStatus"
        siContractSubStatus.ComboItem = "ContractSubStatus"
        siMaintenanceFeeStatus.ComboItem = "MaintenanceFeeStatus"
        siMortgageStatus.ComboItem = "MortgageStatus"
        siConversionStatus.ComboItem = "ConversionStatus"

        siContractStatus.Load_Items()
        siContractSubStatus.Load_Items()
        siMaintenanceFeeStatus.Load_Items()
        siMortgageStatus.Load_Items()
        siConversionStatus.Load_Items()
    End Sub

    Private Sub Toggle_DropDowns()
        siContractStatus.Read_Only = Not (cbCS.Checked)
        siContractSubStatus.Read_Only = Not (cbCSS.Checked)
        siMaintenanceFeeStatus.Read_Only = Not (cbMFS.Checked)
        siMortgageStatus.Read_Only = Not (cbMS.Checked)
        siConversionStatus.Read_Only = Not (cbCvS.Checked)
        btnRemove.Enabled = lstKCP.Items.Count > 0
    End Sub

    Protected Sub cbMS_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbMS.CheckedChanged
        Toggle_DropDowns()
    End Sub

    Protected Sub cbCvS_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbCvS.CheckedChanged
        Toggle_DropDowns()
    End Sub
End Class
