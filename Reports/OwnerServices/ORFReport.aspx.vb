Imports System.Data
Partial Class Reports_OwnerServices_ORFReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim ds As New SqlDataSource
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select p.FirstName, p.LastName, c.ContractNumber, i.Amount, i.Balance, i.Reference from v_invoices i inner join t_prospect p on i.ProspectiD = p.ProspectID inner join t_Contract c on i.KeyValue = c.ContractID where balance > 0 and invoice = 'ORF'"
            gvReport.DataSource = ds
            gvReport.DataBind()
            ds = Nothing
        End If
    End Sub
End Class
