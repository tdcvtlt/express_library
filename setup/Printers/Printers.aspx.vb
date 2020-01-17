
Partial Class setup_Printers_Printers
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            gvPrinters.DataSource = (New clsPrinters).List
            gvPrinters.DataBind()
        End If
    End Sub

    Protected Sub Add_Click(sender As Object, e As EventArgs) Handles Add.Click
        Response.Redirect("editprinter.aspx?printerid=0")
    End Sub
End Class
