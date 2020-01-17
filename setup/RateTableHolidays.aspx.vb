
Partial Class setup_RateTableHolidays
    Inherits System.Web.UI.Page

    Protected Sub btnList_Click(sender As Object, e As EventArgs) Handles btnList.Click
        Dim oRate As New clsRateTableHolidays
        gvHolidays.DataSource = oRate.List_Holiday_Rates
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvHolidays.DataKeyNames = sKeys
        gvHolidays.DataBind()
        oRate = Nothing
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/addRateTableHoliday.aspx','win01',350,350);", True)
    End Sub

    Protected Sub gvHolidays_RowDeleting(sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvHolidays.RowDeleting
        Dim oHol As New clsRateTableHolidays
        oHol.Delete_Record(gvHolidays.Rows(e.RowIndex).Cells(1).Text)
        gvHolidays.DataSource = oHol.List_Holiday_Rates
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvHolidays.DataKeyNames = sKeys
        gvHolidays.DataBind()
        oHol = Nothing
    End Sub
End Class
