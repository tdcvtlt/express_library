
Partial Class online_CalendarEvents
    Inherits System.Web.UI.Page

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("EditCalendarEvents.aspx?id=0")
    End Sub

    Protected Sub gvCal_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCal.PageIndexChanging
        gvCal.DataSource = Session("CalDS")
        gvCal.PageIndex = e.NewPageIndex
        gvCal.DataBind()
    End Sub

    Protected Sub gvCal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCal.SelectedIndexChanged
        Response.Redirect("editcalendarevents.aspx?id=" & gvCal.Rows(gvCal.SelectedIndex).Cells(1).Text)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("CalDS") = Nothing
            Dim oCI As New clsComboItems
            ddCal.DataSource = oCI.Load_ComboItems("EventCalendar")
            ddCal.DataValueField = "Comboitemid"
            ddCal.DataTextField = "Comboitem"
            ddCal.DataBind()
            ddCal.SelectedIndex = 0
            Load_List()
            oCI = Nothing
        End If
    End Sub

    Protected Sub ddCal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddCal.SelectedIndexChanged
        Load_List()
    End Sub

    Private Sub Load_List()
        Dim ds As New SqlDataSource(Resources.Resource.cns, "Select * from t_CalendarEvents where calendarid = '" & ddCal.SelectedValue & "' order by [date], cast('1/1/12 ' + time as smalldatetime)")
        Session("CalDS") = ds
        gvCal.DataSource = Session("CalDS")
        gvCal.DataBind()
        ds = Nothing
    End Sub
End Class
