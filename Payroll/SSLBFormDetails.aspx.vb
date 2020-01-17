
Partial Class Payroll_SSLBFormDetails
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oSSLBDates As New clsSSLBRequestDates
            gvSSLBDetails.DataSource = oSSLBDates.Get_SSLB_Dates(Request("PALRequestID"))
            gvSSLBDetails.DataBind()
            oSSLBDates = Nothing
        End If
    End Sub
End Class
