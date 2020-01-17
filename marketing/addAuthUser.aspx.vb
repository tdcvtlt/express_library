
Partial Class marketing_addAuthUser
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oAuthUser As New clsContractAuthorizedUsers
        oAuthUser.AuthorizedUserID = 0
        oAuthUser.ContractID = Request("ContractID")
        oAuthUser.FirstName = txtFirstName.Text
        oAuthUser.LastName = txtLastName.Text
        oAuthUser.Active = True
        oAuthUser.DateCreated = System.DateTime.Now
        oAuthUser.CreatedByID = Session("UserDBID")
        oAuthUser.Save()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Auth_Users();window.close();", True)
    End Sub
End Class
