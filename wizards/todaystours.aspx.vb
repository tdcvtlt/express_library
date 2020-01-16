
Partial Class wizards_todaystours
    Inherits System.Web.UI.Page

 
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        MultiView1.ActiveViewIndex = 0
        Dim oTours As New clsTour
        gvTours.DataSource() = oTours.todays_Tours("KCP")
        gvTours.DataBind()
    End Sub

    Protected Sub LinkButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton2.Click
        MultiView1.ActiveViewIndex = 0
        Dim oTours As New clsTour
        gvTours.DataSource() = oTours.todays_Tours("VacationClub")
        gvTours.DataBind()
    End Sub

    Protected Sub gvTours_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvTours.RowCommand
        If e.CommandName = "Select" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = gvTours.Rows(index)
            Dim oTour As New clsTour
            Dim oDNS As New clsDoNotSellList
            oTour.TourID = row.Cells(2).Text
            oTour.Load()
            If oDNS.Get_Status(oTour.ProspectID) = "Remove" Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "modal.mwindow.open('../marketing/DoNotSellListOverRide.aspx?KeyField=Tour&KeyValue=" & oTour.TourID & "&Source=TourWizard','win01',350,350);", True)
            Else
                Response.Redirect("~/wizards/tourcheckin.aspx?TourID=" & row.Cells(2).Text)
            End If
            oTour = Nothing
            oDNS = Nothing
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            MultiView1.ActiveViewIndex = 0
            Dim oTours As New clsTour
            gvTours.DataSource() = oTours.todays_Tours("KCP")
            gvTours.DataBind()
        End If
    End Sub

    Protected Sub Unnamed3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oPros As New clsProspect
        gvPros.DataSource() = oPros.List_For_TourWizard(txtPhoneNumber.Text)
        Dim ka(0) As String
        ka(0) = "ProspectID"
        gvPros.DataKeyNames = ka
        gvPros.DataBind()
        oPros = Nothing
    End Sub

    Protected Sub LinkButton3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton3.Click
        MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub gvPros_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPros.SelectedIndexChanged
        Dim oDNS As New clsDoNotSellList
        If oDNS.Get_Status(gvPros.SelectedValue) = "Remove" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "modal.mwindow.open('../marketing/DoNotSellListOverRide.aspx?KeyField=Tour&KeyValue=" & gvPros.SelectedValue & "&Source=TourWizardNew','win01',350,350);", True)
        Else
            Response.Redirect("tourcheckin.aspx?TourID=0&ProspectID=" & gvPros.SelectedValue)
        End If
        oDNS = Nothing
    End Sub

    Protected Sub LinkButton4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton4.Click
        Response.Redirect("tourcheckin.aspx?TourID=0&ProspectID=0")
    End Sub

    Protected Sub lbWoodbridge_Click(sender As Object, e As EventArgs) Handles lbWoodbridge.Click
        MultiView1.ActiveViewIndex = 0
        Dim oTours As New clsTour
        gvTours.DataSource() = oTours.todays_Tours("Woodbridge")
        gvTours.DataBind()
    End Sub

    Protected Sub gvTours_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvTours.SelectedIndexChanged

    End Sub
End Class
