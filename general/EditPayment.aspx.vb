
Partial Class general_EditPayment
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If IsNumeric(Request("paymentid")) Then
                Dim oPayment As New clsPayments
                oPayment.PaymentID = Request("paymentid")
                oPayment.Load()
                If oPayment.Adjustment Then
                    Dim oAdjustments As New clsPaymentMethod2Adjustment
                    If oPayment.PosNeg = True Then
                        ddAdjustments.DataSource = oAdjustments.Sort_Adjustments(1)
                    Else
                        ddAdjustments.DataSource = oAdjustments.Sort_Adjustments(0)
                    End If
                    ddAdjustments.DataTextField = "Adjustment"
                    ddAdjustments.DataValueField = "ID"
                    ddAdjustments.DataBind()
                    oAdjustments = Nothing
                    ddAdjustments.SelectedValue = oPayment.MethodID
                    lblPM.Visible = False
                Else
                    Dim oCombo As New clsComboItems
                    lblPM.Text = oCombo.Lookup_ComboItem(oPayment.MethodID)
                    ddAdjustments.Visible = False
                    oCombo = Nothing
                End If
                oPayment = Nothing
                Set_Values()

            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "none", "window.close();", True)
            End If
        End If
    End Sub

    Private Sub Set_Values()
        Dim oPayment As New clsPayments
        oPayment.PaymentID = Request("PaymentID")
        oPayment.Load()
        txtDesc.Text = oPayment.Description
        txtAmount.Text = oPayment.Amount
        txtRef.Text = oPayment.Reference
        Dim oCCTransAPply As New clsCCTransApplyTo
        If oCCTransAPply.Is_CCPayment(oPayment.PaymentID) Then
            txtRef.Enabled = False
            txtAmount.Enabled = False
            oCCTransAPply = Nothing
        End If
        txtTransDate.Text = oPayment.TransDate
        rbPos.Checked = Not (oPayment.PosNeg)
        rbNeg.Checked = oPayment.PosNeg
        Dim oPers As New clsPersonnel
        oPers.PersonnelID = oPayment.UserID
        oPers.Load()
        txtUserName.Text = oPers.UserName
        oPers = Nothing
        rbpos.Enabled = False
        rbNeg.Enabled = False
        oPayment = Nothing
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
    End Sub

    Protected Sub Update_Fields()
        Dim oPayment As New clsPayments
        oPayment.PaymentID = Request("paymentID")
        oPayment.Load()
        oPayment.Description = txtDesc.Text
        oPayment.Amount = txtAmount.Text
        oPayment.Reference = txtRef.Text
        oPayment.PosNeg = Not (rbPos.Checked)
        oPayment = Nothing
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim oPayment As New clsPayments
        Dim oPmt2Adj As New clsPaymentMethod2Adjustment
        Dim bProceed As Boolean = True
        oPayment.PaymentID = Request("PaymentID")
        oPayment.UserID = Session("UserDBID")
        oPayment.Load()
        If DateDiff(DateInterval.Minute, CDate(oPayment.TransDate), Date.Now) <= 1440 And oPayment.UserID = Session("UserDBID") Then
            '            Update_Fields()
            If oPayment.Adjustment Then
                If oPmt2Adj.Require_Permissions(ddAdjustments.SelectedValue, 1) Then
                    If CheckSecurity("Payments", "Receive" & Replace(ddAdjustments.SelectedItem.Text, " ", "") & "Payments", , , Session("UserDBID")) Then
                        oPayment.MethodID = ddAdjustments.SelectedValue
                        bProceed = True
                    Else
                        bProceed = False
                    End If
                Else
                    oPayment.MethodID = ddAdjustments.SelectedValue
                End If
            End If
            If bProceed = True Then
                oPayment.Description = txtDesc.Text
                oPayment.Amount = txtAmount.Text
                oPayment.Reference = txtRef.Text
                oPayment.PosNeg = Not (rbPos.Checked)
                If oPayment.Save Then
                    Dim oPayment2Inv As New clsPayment2Invoice
                    oPayment2Inv.Update_Pmt_Amt(oPayment.PaymentID, txtAmount.Text)
                    oPayment2Inv = Nothing
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "refresh", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
                    oPayment = Nothing
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Sorry", "alert('" & oPayment.Error_Message & "');", True)
                End If
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "ajax", "alert('You Do Not Have Permission to Receive this Adjustment Type.');", True)
            End If
        ElseIf oPayment.Adjustment And CheckSecurity("Payments", "EditAdjustments", , , Session("UserDBID")) Then
            'Update_Fields()

            If oPmt2Adj.Require_Permissions(ddAdjustments.SelectedValue, 1) Then
                If CheckSecurity("Payments", "Receive" & Replace(ddAdjustments.SelectedItem.Text, " ", "") & "Payments", , , Session("UserDBID")) Then
                    bProceed = True
                Else
                    bProceed = False
                End If
            End If
            If bProceed = True Then
                oPayment.Description = txtDesc.Text
                oPayment.Amount = txtAmount.Text
                oPayment.Reference = txtRef.Text
                oPayment.PosNeg = Not (rbPos.Checked)
                oPayment.MethodID = ddAdjustments.SelectedValue
                If oPayment.Save Then
                    Dim oPayment2Inv As New clsPayment2Invoice
                    oPayment2Inv.Update_Pmt_Amt(oPayment.PaymentID, txtAmount.Text)
                    oPayment2Inv = Nothing
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "refresh", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
                    oPayment = Nothing
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Sorry", "alert('" & oPayment.Error_Message & "');", True)
                End If
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "ajax", "alert('You Do Not Have Permission to Receive this Adjustment Type.');", True)
            End If

        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Sorry", "alert('This payment is not editable.');", True)
        End If
        oPmt2Adj = Nothing
    End Sub
End Class
