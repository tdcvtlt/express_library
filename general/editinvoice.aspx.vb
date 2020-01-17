
Partial Class general_editinvoice
    Inherits System.Web.UI.Page
    Dim oInv As clsInvoices
    Dim oFTC As clsFinancialTransactionCodes

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        oInv = New clsInvoices
        If Not IsPostBack Then
            If (Request("InvoiceID") = "" And Request("KeyField") = "") Or Request("ListFilter") = "" Then ' Or Request("KeyValue") = "0" Then
                'Close()
            Else

                oFTC = New clsFinancialTransactionCodes
                oFTC.ListFilter = Request("ListFilter")
                ddTransCodes.DataSource = oFTC.List
                ddTransCodes.DataTextField = "Code"
                ddTransCodes.DataValueField = "ID"
                ddTransCodes.DataBind()
                If IsNumeric(Request("InvoiceID")) Then
                    oInv.InvoiceID = CInt(Request("InvoiceID"))
                    oInv.Load()
                    Set_Fields()
                Else
                    txtAmount.Text = "No InvoiceID"
                End If
                lblErr.Text = oInv.Error_Message
            End If
        End If
    End Sub

    Protected Sub Set_Fields()
        txtAmount.Text = FormatCurrency(oInv.Amount)
        txtReference.Text = oInv.Reference
        dtTD.Selected_Date = oInv.TransDate
        dtDD.Selected_Date = oInv.DueDate
        For i = 0 To ddTransCodes.Items.Count - 1
            If ddTransCodes.Items(i).Value = oInv.FinTransID Then
                ddTransCodes.SelectedIndex = i
                Exit For
            End If
        Next
        cnUserName.UserID = oInv.UserID
        If Request("InvoiceID") <> "0" And Request("InvoiceID") <> 0 Then
            If (oInv.UserID = Session("UserDBID") And DateDiff(DateInterval.Minute, CDate(oInv.TransDate), Date.Now) <= 1440) Or oInv.KeyField = "MortgageDP" Then
                btnSave.Enabled = True
            ElseIf (CheckSecurity("Payments", "EditInvoices", lUserID:=Session("UserDBID"))) Then
                btnSave.Enabled = True
            Else
                btnSave.Enabled = False
            End If
            'Response.Write(DateDiff(DateInterval.Minute, CDate(oInv.TransDate), Date.Now).ToString)
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        oInv.InvoiceID = CInt(Request("InvoiceID"))
        oInv.Load()
        oInv.TransDate = IIf(dtTD.Selected_Date <> "", dtTD.Selected_Date, Date.Now)
        oInv.DueDate = IIf(dtDD.Selected_Date <> "", dtDD.Selected_Date, DateAdd("m", 1, CDate(oInv.TransDate)))
        oInv.FinTransID = ddTransCodes.SelectedItem.Value
        
        oInv.Amount = txtAmount.Text
        oInv.UserID = IIf(oInv.InvoiceID = 0, Session("UserDBID"), oInv.UserID)
        If oInv.InvoiceID = 0 Then
            'If Request("KeyField") = "MortgageDP" Or Request("KeyField") = "MortgageCC" Or Request("KeyField") = "MortgageMem" Then
            '    Dim oftc As New clsFinancialTransactionCodes
            '    oftc.FinTransID = ddTransCodes.SelectedItem.Value
            '    oftc.Load()
            '    Dim oCI As New clsComboItems
            '    oCI.ID = oftc.TransTypeID
            '    oCI.Load()
            '    oInv.KeyField = oCI.Comboitem
            '    oCI = Nothing
            '    oftc = Nothing
            'Else
            oInv.Reference = txtReference.Text
            oInv.KeyField = Request("KeyField")
            'End If
            If Request("KeyField") = "ProspectID" And Request("KeyValue") = 0 Then
                oInv.KeyValue = Request("ProspectID")
            Else
                oInv.KeyValue = Request("KeyValue")
            End If
            oInv.ProspectID = Request("ProspectID")
        End If
        If Not (oInv.Save()) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "reload", "alert('" & oInv.Error_Message & "');", True)
        Else
            If LCase(oInv.KeyField) = "mortgagedp" Then
                Dim oFT As New clsFinancialTransactionCodes
                Dim oCI As New clsComboItems
                oFT.FinTransID = oInv.FinTransID
                oFT.Load()
                oCI.ComboID = oFT.TransCodeID
                oCI.Load()
                Dim oMort As New clsMortgage
                oMort.MortgageID = oInv.KeyValue
                oMort.UserID = Session("UserDBID")
                oMort.Load()
                If InStr(LCase(oCI.Comboitem), "down pay") > 0 Then
                    If oMort.DPInvoiceID = oInv.InvoiceID Or oMort.DPInvoiceID = 0 Then
                        oMort.DPTotal = oInv.Amount
                        oMort.DPInvoiceID = oInv.InvoiceID
                        oMort.Save()
                    End If
                    If Not (Session("Mortgage") Is Nothing) Then
                        oMort = Session("mortgage")
                        oMort.DPTotal = oInv.Amount
                        oMort.DPInvoiceID = oInv.InvoiceID
                        Session("Mortgage") = oMort
                    End If
                ElseIf InStr(LCase(oCI.Comboitem), "closing") > 0 Then
                    If oMort.CCInvoiceID = oInv.InvoiceID Or oMort.CCInvoiceID = 0 Then
                        oMort.CCTotal = oInv.Amount
                        oMort.CCInvoiceID = oInv.InvoiceID
                        oMort.Save()
                    End If
                    If Not (Session("Mortgage") Is Nothing) Then
                        oMort = Session("mortgage")
                        oMort.CCTotal = oInv.Amount
                        oMort.CCInvoiceID = oInv.InvoiceID
                        oMort.CCFinanced = IIf(oMort.CCFinanced > 0, oMort.CCTotal, oMort.CCFinanced)
                        Session("Mortgage") = oMort
                    End If
                End If
                oMort = Nothing
                oFT = Nothing
                oCI = Nothing
            End If
            'ClientScript.RegisterClientScriptBlock(Me.GetType, "reload", "window.opener.document.getElementById('" & Replace(Request("linkid"), "$", "_") & "').value='Update';", True)
            ClientScript.RegisterClientScriptBlock(Me.GetType, "refresh", "window.opener.__doPostBack('" & Request("linkid") & "','');", True)
            Close()
        End If
    End Sub

    Private Sub Close()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "close", "window.close();", True)
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Close()
    End Sub
End Class
