
Partial Class marketing_contracts
    Inherits System.Web.UI.Page

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If CheckSecurity("Contracts", "List", , , Session("UserDBID")) Then
            Dim oContracts As New clsContract
            gvContracts.DataSource = oContracts.Query(100, ddFilter.SelectedValue, txtFilter.Text, "")
            gvContracts.DataBind()

            lblError.Text = oContracts.Error_Message
            oContracts = Nothing
        Else
            lblError.Text = "ACCESS DENIED"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            btnSearch_Click(sender, e)
        End If
    End Sub
End Class
