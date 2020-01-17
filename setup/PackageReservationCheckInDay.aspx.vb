
Partial Class setup_PackageReservationCheckInDay
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            ddCheckInDay.Items.Add(New ListItem("Sunday", 0))
            ddCheckInDay.Items.Add(New ListItem("Monday", 1))
            ddCheckInDay.Items.Add(New ListItem("Tuesday", 2))
            ddCheckInDay.Items.Add(New ListItem("Wednesday", 3))
            ddCheckInDay.Items.Add(New ListItem("Thursday", 4))
            ddCheckInDay.Items.Add(New ListItem("Friday", 5))
            ddCheckInDay.Items.Add(New ListItem("Saturday", 6))
        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim oPkgResNight As New clsPackageReservation2CheckInDay
        oPkgResNight.ID = 0
        oPkgResNight.Load()
        oPkgResNight.PackageReservationID = Request("ID")
        oPkgResNight.CheckInDay = ddCheckInDay.SelectedValue
        oPkgResNight.Save()
        oPkgResNight = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.refreshCheckInDays();window.close();", True)
    End Sub
End Class
