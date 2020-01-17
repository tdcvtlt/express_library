
Partial Class setup_Kiosks_kiosks
    Inherits System.Web.UI.Page
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If CheckSecurity("Kiosks", "List", , , Session("UserDBID")) Then
            Dim oKiosks As New clsLGKiosk
            gvKiosks.DataSource = oKiosks.Query(100, ddFilter.SelectedValue, txtFilter.Text, "")
            gvKiosks.DataBind()

            lblError.Text = oKiosks.Error_Message
            oKiosks = Nothing
        Else
            lblError.Text = "ACCESS DENIED"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            btnSearch_Click(sender, e)
        End If
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("Editkiosk.aspx?kioskid=-1")
    End Sub
End Class
