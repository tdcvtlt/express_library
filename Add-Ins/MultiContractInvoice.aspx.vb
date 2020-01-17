
Partial Class Add_Ins_MultiContractInvoice
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

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim ofTrans As New clsFinancialTransactionCodes
            ddInvoice.DataSource = ofTrans.List_Trans_Codes("ContractTrans")
            ddInvoice.DataTextField = "TransCode"
            ddInvoice.DataValueField = "FinTransID"
            ddInvoice.DataBind()
            ofTrans = Nothing
        End If
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        If lbContract.SelectedValue <> "" Then
            lbContract.Items.Remove(lbContract.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        If lbContract.Items.Count = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Valid Contract');", True)
        ElseIf txtAmount.Text = "" Or Not (isNumeric(txtAmount.Text)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Valid Amount');", True)
        ElseIf txtReference.Text = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Reference');", True)
        ElseIf txtDesc.Text = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Description');", True)
        Else
            Dim oInvoice As New clsInvoices
            Dim oCon As New clsContract
            For i = 0 To lbContract.Items.Count - 1
                oInvoice.InvoiceID = 0
                oInvoice.Load()
                oInvoice.KeyField = "ContractID"
                oInvoice.KeyValue = lbContract.items(i).Value
                oCon.ContractID = lbContract.items(i).Value
                oCon.Load()
                oInvoice.ProspectID = oCon.ProspectID
                oInvoice.FinTransID = ddInvoice.SelectedValue
                oInvoice.Amount = txtAmount.Text
                oInvoice.Reference = txtReference.Text
                oInvoice.Description = txtDesc.Text
                oInvoice.UserID = Session("UserDBID")
                oInvoice.TransDate = System.DateTime.Now
                oInvoice.DueDate = System.DateTime.Now.AddDays(30)
                oInvoice.Save()
            Next
            oInvoice = Nothing
            oCon = Nothing
            lbContract.Items.Clear()
            txtAmount.Text = ""
            txtDesc.Text = ""
            txtReference.Text = ""
            txtContract.text = ""
        End If
    End Sub
End Class
