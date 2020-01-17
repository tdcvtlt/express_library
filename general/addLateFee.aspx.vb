Imports Microsoft.VisualBasic
Partial Class general_addLateFee
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oInv As New clsInvoices
            oInv.KeyField = Request("KeyField")
            oInv.KeyValue = Request("KeyValue")
            oInv.ProspectID = Request("ProspectID")
            gvInvoices.DataSource = oInv.List_Invoices
            gvInvoices.DataBind()
            oInv = Nothing
        End If
    End Sub

    Protected Sub cb_CheckedChanged(sender As Object, e As System.EventArgs)
        ' Dim x As New RadioButton
        'lblErr.Text = e.GetType.ToString
        hfInvoiceID.Value = CType(sender.Parent.parent, GridViewRow).Cells(1).Text
        For Each i As Control In Me.Controls
            'lblErr.Text &= "<br />" & i.GetType.ToString
            If TypeOf i Is HtmlForm Then
                For Each x As Control In i.Controls
                    If TypeOf x Is GridView Then
                        For Each y As Control In x.Controls
                            For Each z As Control In y.Controls
                                For Each f As Control In z.Controls
                                    If f.HasControls Then
                                        For Each r As Control In f.Controls
                                            If TypeOf r Is RadioButton And r IsNot sender Then
                                                If r.ID = "cb" Then
                                                    CType(r, RadioButton).Checked = False

                                                End If
                                            ElseIf r Is sender Then
                                                'lblErr.Text = CType(sender.Parent.parent, GridViewRow).Cells(1).Text
                                            End If
                                        Next
                                    End If
                                Next
                            Next
                        Next
                    End If
                Next
            End If
        Next
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim sErr As String = ""
        Dim bProceed As Boolean = True
        If txtAmount.Text = "" Or Not IsNumeric(txtAmount.Text) Then
            sErr += "Please Enter an Amount. \n"
            bProceed = False
        End If
        If dteDueDate.Selected_Date = "" Then
            sErr += "Please Enter a Due Date. \n."
            bProceed = False
        End If
        If txtDesc.Text = "" Then
            sErr += "Please Enter a Description. \n"
            bProceed = False
        End If
        If txtReference.Text = "" Then
            sErr += "Please Enter a Reference. \n"
            bProceed = False
        End If
        If hfInvoiceID.Value = 0 Then
            sErr += "Please Select an Invoice to Apply the Late Fee to. \n"
            bProceed = False
        End If
        If bProceed Then
            Dim oInvoice As New clsInvoices
            Dim oOldInvoice As New clsInvoices
            Dim oFinTrans As New clsFinancialTransactionCodes
            Dim oCombo As New clsComboItems
            Dim invID As Integer = 0
            oInvoice.InvoiceID = 0
            oOldInvoice.InvoiceID = hfInvoiceID.Value
            oOldInvoice.Load()
            oInvoice.Load()
            oInvoice.UserID = Session("UserDBID")
            oInvoice.Amount = txtAmount.Text
            oInvoice.TransDate = System.DateTime.Now.ToShortDateString
            oInvoice.DueDate = dteDueDate.Selected_Date
            oInvoice.KeyField = oOldInvoice.KeyField
            oInvoice.KeyValue = oOldInvoice.KeyValue
            oInvoice.ProspectID = oOldInvoice.ProspectID
            oInvoice.Adjustment = False
            oInvoice.PosNeg = False
            oInvoice.FinTransID = oFinTrans.Find_Fin_Trans("LFTrans", "LATE FEE")
            oInvoice.Reference = txtReference.Text
            oInvoice.Description = txtDesc.Text
            oInvoice.Save()
            invID = oInvoice.InvoiceID

            oInvoice.InvoiceID = hfInvoiceID.Value
            oInvoice.Load()
            oFinTrans.FinTransID = oInvoice.FinTransID
            oFinTrans.Load()
            If oCombo.Lookup_ComboItem(oFinTrans.TransTypeID) = "MFTrans" Then
                Dim oLF2MF As New clsLF2MF
                oLF2MF.LF2MFID = 0
                oLF2MF.Load()
                oLF2MF.LFInvoiceID = invID
                oLF2MF.MFInvoiceID = hfInvoiceID.Value
                oLF2MF.Save()
                oLF2MF = Nothing
            End If
            oOldInvoice = Nothing
            oFinTrans = Nothing
            oInvoice = Nothing
            oCombo = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "refresh", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "refresh", "alert('" & sErr & "');", True)
        End If
    End Sub
End Class
