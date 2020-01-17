
Partial Class Accounting_CCLookup
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oCCTrans As New clsCCTrans
        gvCCTrans.DataSource = oCCTrans.CC_Lookup(txtNumber.text, txtExp.Text)
        gvCCTrans.DataBind()
        oCCTrans = Nothing
    End Sub
End Class
