
Partial Class general_WebSiteInfo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oAcct As New clsOwnerAccounts
            hfAcctID.Value = oAcct.Lookup_Acct(Request("ProspectID"))
            If hfAcctID.Value > 0 Then
                oAcct.OwnerAccountID = hfAcctID.Value
                oAcct.Load()
                txtUName.Text = oAcct.UserName
                Dim pw As String = ""
                For i = 1 To oAcct.password.Length
                    pw = pw & "X"
                Next
                txtPassword.Attributes.Add("value", pw)
                txtEmail.Text = oAcct.emailname
                txtDateCreated.Text = oAcct.DateCreated
                txtConfirmed.Text = oAcct.Confirmed
                txtValCode.Text = oAcct.ConfirmationSessionID
                MultiView1.ActiveViewIndex = 0
            Else
                MultiView1.ActiveViewIndex = 1
            End If
            oAcct = Nothing
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oAcct As New clsOwnerAccounts
        oAcct.OwnerAccountID = hfAcctID.Value
        oAcct.Load()
        oAcct.UserName = txtUName.Text
        oAcct.emailname = txtEmail.Text
        oAcct.password = txtPassword.Text
        oAcct.Save()
        oAcct = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('Account Updated.');window.close();", True)
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "window.close();", True)
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "window.close();", True)
    End Sub
End Class
