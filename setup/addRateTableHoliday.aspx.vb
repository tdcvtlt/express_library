
Partial Class setup_addRateTableHoliday
    Inherits System.Web.UI.Page

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim oRate As New clsRateTableHolidays
        Dim sDate As DateTime = dteSDate.Selected_Date
        Do While sDate.CompareTo(CDate(dteEDate.Selected_Date)) <= 0
            If Not (oRate.Date_Exists(sDate)) Then
                oRate.ID = 0
                oRate.Load()
                oRate.HolidayDate = sDate
                oRate.HolidayRate = txtAmount.Text
                oRate.Save()
            End If
            sDate = sDate.AddDays(1)
        Loop
        oRate = Nothing
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "window.opener.Refresh_Dates();window.close();", True)
    End Sub
End Class
