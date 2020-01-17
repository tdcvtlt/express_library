
Partial Class setup_RateTableRates
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oRateTable As New clsRateTable
            ddRateTable.DataSource = oRateTable.Get_Rate_Tables
            ddRateTable.DataTextField = "Name"
            ddRateTable.DataValueField = "RateTableID"
            ddRateTable.DataBind()
            ddRateTables.DataSource = oRateTable.Get_Rate_Tables
            ddRateTables.DataTextField = "Name"
            ddRateTables.DataValueField = "RateTableID"
            ddRateTables.DataBind()
            oRateTable = Nothing
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click
        If dteStartDate.Selected_Date = "" Or dteEndDate.Selected_Date = "" Then

        Else
            Dim oRates As New clsRateTableRates
            gvRates.DataSource = oRates.Get_Rates(ddRateTable.SelectedValue, dteStartDate.Selected_Date, dteEndDate.Selected_Date)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvRates.DataKeyNames = sKeys
            gvRates.DataBind()
            oRates = Nothing
        End If
    End Sub

    Protected Sub gvRates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(2).Text = CDate(e.Row.Cells(2).Text).ToShortDateString
                e.Row.Cells(3).Text = FormatCurrency(e.Row.Cells(3).Text, 2)
            End If
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub LinkButtonView_Click(sender As Object, e As System.EventArgs) Handles LinkButtonView.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub LinkButtonBuild_Click(sender As Object, e As System.EventArgs) Handles LinkButtonBuild.Click
        litResult.Text = ""
        MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If dteSDate.Selected_Date = "" Or dteEDate.Selected_Date = "" Or Not (IsNumeric(txtWeekdayRate.Text)) Or Not (IsNumeric(txtWeekendRate.Text)) Or Not (IsNumeric(txtWeekdayCost.Text)) Or Not (IsNumeric(txtWeekendCost.Text)) Or Not (IsNumeric(txtWeekDayRental.Text)) Or Not (IsNumeric(txtWeekendRental.Text)) Or Not (IsNumeric(txtWeekDayTS.Text)) Or Not (IsNumeric(txtWeekEndTS.Text)) Or Not (IsNumeric(txtWeekDayTP.Text)) Or Not (IsNumeric(txtWeekEndTP.Text)) Then
            litResult.Text = "Please Fill in All Fields"
        Else
            Dim oRates As New clsRateTableRates
            oRates.UserID = Session("UserDBID")
            If oRates.Build_Rates(ddRateTables.SelectedValue, dteSDate.Selected_Date, dteEDate.Selected_Date, txtWeekdayRate.Text, txtWeekendRate.Text, txtWeekDayRental.Text, txtWeekendRental.Text, txtWeekdayCost.Text, txtWeekendCost.Text, txtWeekDayOwner.Text, txtWeekEndOwner.Text, txtWeekDayTS.Text, txtWeekEndTS.Text, txtWeekDayTP.Text, txtWeekEndTP.Text) Then
                litResult.Text = "Success"
            Else
                litResult.Text = oRates.Err
            End If
            oRates = Nothing
        End If
    End Sub

    Protected Sub gvRates_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvRates.SelectedIndexChanged

    End Sub
End Class
