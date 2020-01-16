
Partial Class wizards_ExitTours
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            MultiView1.ActiveViewIndex = 0
            Dim oTours As New clsTour
            gvTours.DataSource() = oTours.get_Exit_Tours("KCP")
            gvTours.DataBind()
            lblErr.Text = "HERE"
        End If
    End Sub


    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        MultiView1.ActiveViewIndex = 0
        Dim oTours As New clsTour
        gvTours.DataSource() = oTours.get_Exit_Tours("KCP")
        gvTours.DataBind()
    End Sub


    Protected Sub LinkButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton2.Click
        MultiView1.ActiveViewIndex = 1
        Dim oTours As New clsTour
        gvOutstandingTours.DataSource() = oTours.get_Outstanding_Tours("KCP")
        gvOutstandingTours.DataBind()
    End Sub

    Protected Sub gvTours_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvTours.RowCommand
        If e.CommandName = "Select" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = gvTours.Rows(index)
            Response.Redirect("~/wizards/ExitTourWizard.aspx?TourID=" & row.Cells(1).Text)
        End If
    End Sub

    Protected Sub gvOutstandingTours_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvOutstandingTours.RowCommand
        If e.CommandName = "Select" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = gvOutstandingTours.Rows(index)
            Response.Redirect("~/wizards/ExitTourWizard.aspx?TourID=" & row.Cells(1).Text)
        End If
    End Sub

    Protected Sub gvTours_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTours.SelectedIndexChanged

    End Sub
End Class
