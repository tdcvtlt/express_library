
Partial Class Payroll_PALFormDetails
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oPALDates As New clsPALRequestDates
            gvPALDetails.DataSource = oPALDates.Get_PAL_Dates(Request("PALRequestID"))
            gvPALDetails.DataBind()
            oPALDates = Nothing
        End If
    End Sub
End Class
