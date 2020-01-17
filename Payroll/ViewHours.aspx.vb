
Partial Class Payroll_ViewHours
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        If dteStartDate.Selected_Date = "" Or dteEndDate.Selected_Date = "" Then

        Else
            Dim oPersonnelPunch As New clsPersonnelPunch
            litHrs.Text = oPersonnelPunch.get_Hours(dteStartDate.Selected_Date, dteEndDate.Selected_Date, Request("PersonnelID"))
            lblErr.Text = oPersonnelPunch.Err
            oPersonnelPunch = Nothing
        End If
    End Sub
End Class
