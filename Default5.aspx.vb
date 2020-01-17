Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class Default5
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "SELECT * FROM [CRMSNet].[dbo].[UFN_Invoice_Aging] ('1/1/12', 'MF07','ContractID')"
        GridView1.DataSource = ds
        GridView1.DataBind()
        ds = Nothing
    End Sub
End Class
