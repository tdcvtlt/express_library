
Partial Class Reports_Accounting_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Server.MapPath(Request("report")) <> "" Then
            Dim ta() As String
            ta = Split(Session("Groups"), "|")
            'If ta.Contains("Rental Reports") Then
            'Response.Redirect(Request("report"))
            'Else
            '    Response.Write("Access Denied")
            'End If

            If Request("report").IndexOf("-") > 0 Then
                ''//== path format: reports/rentals/default.aspx?report=88-LeadsPackCancelled-VST.aspx
                ''//==
                ''//== 
                Dim aspx As String = Request("report").Substring(Request("report").IndexOf("-") + 1)
                Dim vendorID As Int16 = 0
                If Integer.TryParse(Request("report").Substring(0, Request("report").IndexOf("-")), vendorID) Then
                    Response.Redirect(aspx + "?vendorid=" & vendorID)
                Else
                    Response.Redirect(Request("report"))
                End If               
            Else
                Response.Redirect(Request("report"))
            End If

        Else
            Response.Write("This Report cannot be found.")
        End If
    End Sub
End Class
