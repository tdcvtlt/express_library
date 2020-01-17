
Partial Class Accounting_editEquiantMapping
    Inherits System.Web.UI.Page

    Protected Sub ddAction_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddAction.SelectedIndexChanged
        'ret.Add(New CodeMapping With {.Code = "PMT", .SourceCode = "A", .Category = "MF", .PosNeg = 1, 
        '.Action = 1, .PaymentMethodID = 32894, .InvoiceType = "Maintenance Fee", .CreateInvoice = True})
        ddInvoiceType.Enabled = False
        ddKeyfield.Enabled = False
        ddPaymentMethod.Enabled = False
        ckCreateInvoice.Enabled = False
        ddInvoice.Enabled = False
        ddInvoiceType.SelectedIndex = 0
        ddKeyfield.SelectedIndex = 0
        ddPaymentMethod.SelectedIndex = 0
        ckCreateInvoice.Checked = False
        ddInvoice.SelectedIndex = 0

        Select Case ddAction.SelectedValue
            Case 1 'Payment
                ddPaymentMethod.Enabled = True
                ddInvoiceType.Enabled = True
                ckCreateInvoice.Enabled = True
                ddKeyfield.Enabled = True

            Case 2 'Reversal
                ddPaymentMethod.Enabled = True
                ddInvoiceType.Enabled = True
                ddKeyfield.Enabled = True

            Case 3 'Invoice
                ckCreateInvoice.Enabled = True
                ckCreateInvoice.Checked = True
                ddInvoice.Enabled = True
                ddKeyfield.Enabled = True

            Case 4 'Dump/Ignore

            Case 5
                ddInvoiceType.Enabled = True
                ddKeyfield.Enabled = True
                ddPaymentMethod.Enabled = True
                ddPaymentMethod.SelectedValue = (New clsComboItems).Lookup_ID("PaymentMethod", "Adjustment")

            Case Else

        End Select
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If Request("ID") <> "" And Request("func") = "Ignore" Then
                Dim cn As New Data.SqlClient.SqlConnection(Resources.Resource.cns)
                Dim cm As New Data.SqlClient.SqlCommand("Update t_EquiantCategoryDescriptions set processed=1, duplicate=1, dateprocessed=null where id=" & Request("ID"), cn)
                cn.Open()
                cm.ExecuteNonQuery()
                cn.Close()
                cm = Nothing
                cn = Nothing
                Response.Redirect("EquiantExceptions.aspx")
            ElseIf Request("ID") <> "" And Request("func") = "ack" Then
                Dim cn As New Data.SqlClient.SqlConnection(Resources.Resource.cns)
                Dim cm As New Data.SqlClient.SqlCommand("Update t_EquiantCategoryDescriptions set dupacknowledged=1 where id=" & Request("ID"), cn)
                cn.Open()
                cm.ExecuteNonQuery()
                cn.Close()
                cm = Nothing
                cn = Nothing
                Response.Redirect("EquiantExceptions.aspx")
            End If
            ddInvoiceType.Items.Add(New ListItem With {.Value = "", .Text = "Select One"})

            'Action = 1 (Payment)
            'Action = 2 (Reversal/Credit)
            'Action = 3 (Create Invoice)
            'Action = 4 (Dump/Ignore)
            'Action = 5 (Invoice Adjustment)
            ddAction.Items.Add(New ListItem With {.Value = 0, .Text = "Select One"})
            ddAction.Items.Add(New ListItem With {.Value = 1, .Text = "Apply As Payment"})
            ddAction.Items.Add(New ListItem With {.Value = 2, .Text = "Apply As Reversal"})
            ddAction.Items.Add(New ListItem With {.Value = 3, .Text = "Apply as Invoice"})
            ddAction.Items.Add(New ListItem With {.Value = 5, .Text = "Apply as Invoice Adjustment"})
            ddAction.Items.Add(New ListItem With {.Value = 4, .Text = "Dump / Ignore"})



            ddKeyfield.Items.Add(New ListItem With {.Value = "", .Text = "Select one"})
            ddKeyfield.Items.Add(New ListItem With {.Value = "ContractID", .Text = "Contract Financials"})
            ddKeyfield.Items.Add(New ListItem With {.Value = "ProspectID", .Text = "Owner/Prospect Financials"})

            ddPaymentMethod.DataSource = (New clsComboItems).Load_ComboItems("PaymentMethod")
            ddPaymentMethod.DataValueField = "Comboitemid"
            ddPaymentMethod.DataTextField = "ComboItem"
            ddPaymentMethod.DataBind()

            ddInvoice.DataSource = (New clsFinancialTransactionCodes).List_Trans_Codes("")
            ddInvoice.DataValueField = "FinTransID"
            ddInvoice.DataTextField = "TransCode"
            ddInvoice.DataBind()
            If Request("transid") & "" = "" Then
                Response.Redirect("EquiantExceptions.aspx")
            Else
                hfTransID.Value = Request("TransID")
            End If
            If Request("ID") & "" = "" Then
                txtCode.Text = Request("Code")
                txtSourceCode.Text = Request("SourceCode")
                txtCategory.Text = Request("Category")
                txtPosNeg.Text = If(Request("PosNeg") = "0", "Positive", "Negative")
                txtID.Text = "0"
            Else
                Dim oMapping As New clsEquiantCodeMapping
                oMapping.ID = Request("ID")
                oMapping.Load()
                txtCode.Text = oMapping.Code
                txtSourceCode.Text = oMapping.SourceCode
                txtCategory.Text = oMapping.Category
                txtPosNeg.Text = If(oMapping.PosNeg = 0, "Positive", "Negative")
                txtID.Text = Request("ID")
                ddAction.SelectedValue = oMapping.Action
                ddAction_SelectedIndexChanged(sender, e)
                ddKeyfield.SelectedValue = oMapping.KeyField
                ddKeyfield_SelectedIndexChanged(sender, e)
                ddInvoiceType.SelectedValue = oMapping.InvoiceType
                ddInvoiceType_SelectedIndexChanged(sender, e)
                ddPaymentMethod.SelectedValue = oMapping.PaymentMethodID
                ckCreateInvoice.Checked = oMapping.CreateInvoice
                ddInvoice.SelectedValue = oMapping.InvoiceID

            End If
        End If
    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("EquiantExceptions.aspx")
    End Sub
    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If txtCode.Text = "" Or txtSourceCode.Text = "" Or txtCategory.Text = "" Or txtPosNeg.Text = "" Then
            lblError.Text = "Code, Sourcecode, Category or Positive/Negative Value is incorrect."
        ElseIf ddAction.SelectedIndex = 0 Then
            lblError.Text = "Please select an Action to take"
        ElseIf ddKeyfield.Enabled And ddKeyfield.SelectedIndex = 0 Then
            lblError.Text = "Please select a Transaction Type"
        ElseIf ddInvoiceType.Enabled And ddInvoiceType.SelectedIndex = 0 Then
            lblError.Text = "Please select an Invoice Type"
        ElseIf ddPaymentMethod.Enabled And ddPaymentMethod.SelectedIndex = 0 Then
            lblError.Text = "Please select a Payment Method"
        ElseIf ddInvoice.Enabled And ddInvoice.SelectedIndex = 0 Then
            lblError.Text = "Please select an Invoice to use"
        Else
            Dim oCM As New clsEquiantCodeMapping
            oCM.ID = txtID.Text
            oCM.Load()
            oCM.Action = ddAction.SelectedValue
            oCM.Category = txtCategory.Text
            oCM.Code = txtCode.Text
            oCM.CreateInvoice = ckCreateInvoice.Checked
            If ddInvoice.Enabled Then oCM.InvoiceID = ddInvoice.SelectedValue
            If ddInvoiceType.Items.Count > 0 Then
                oCM.InvoiceType = ddInvoiceType.SelectedValue
            End If
            oCM.KeyField = ddKeyfield.SelectedValue
            oCM.PaymentMethodID = If(ddPaymentMethod.Enabled = True, ddPaymentMethod.SelectedValue, 0)
            oCM.PosNeg = If(txtPosNeg.Text = "Positive", 0, 1)
            oCM.SourceCode = txtSourceCode.Text
            oCM.Active = True
            oCM.OneTime = True
            oCM.TransID = hfTransID.Value
            oCM.Save()
            'txtID.Text = oCM.ID
            oCM = Nothing
            Response.Redirect("EquiantExceptions.aspx")
        End If
    End Sub
    Protected Sub ddKeyfield_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddKeyfield.SelectedIndexChanged
        ddInvoiceType.Items.Clear()
        ddInvoiceType.Items.Add(New ListItem With {.Value = "", .Text = "Select One"})
        If ddKeyfield.SelectedValue = "ContractID" Then
            ddInvoiceType.Items.Add(New ListItem With {.Value = "Maintenance Fee", .Text = "Maintenance Fee"})
            ddInvoiceType.Items.Add(New ListItem With {.Value = "Late Fee", .Text = "Late Fee"})
        ElseIf ddKeyfield.SelectedValue = "ProspectID" Then
            ddInvoiceType.Items.Add(New ListItem With {.Value = "Club Dues", .Text = "Club Dues"})
            ddInvoiceType.Items.Add(New ListItem With {.Value = "Owner Rental Fee", .Text = "Owner Rental Fee"})
        End If
        ddInvoiceType.Items.Add(New ListItem With {.Value = "Other", .Text = "Other"})

    End Sub
    Protected Sub ddInvoiceType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddInvoiceType.SelectedIndexChanged
        If ddInvoiceType.SelectedIndex > 0 Then ddInvoice.Enabled = ddInvoiceType.SelectedItem.Text = "Other"
    End Sub
End Class
