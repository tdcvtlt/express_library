
Partial Class mis_leaderredirect
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Response.Redirect("http://www.eaglestrikefantasy.com/model/getLeaderboardData.php")
    End Sub
End Class
