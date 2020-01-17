Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class Reports_Contracts_Default2
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Get_List("Select a.ContractID, a.ContractNumber, b.FirstName, b.LastName from t_Contract a inner join t_Prospect b on a.prospectid = b.prospectid where a.statusid in (Select comboitemid from t_ComboItems i inner join t_combos b on b.comboid = i.comboid where b.comboname = 'ContractStatus' and i.comboitem = 'ReDeed') order by a.contractnumber asc")
        End If
    End Sub

    Private Sub Get_List(ByVal sSQL As String)
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim dr As SqlDataReader
        Dim ds As New SqlDataSource

        Try
            ds.connectionstring = Resources.Resource.cns
            ds.selectcommand = sSQL
            gvResults.datasource = ds
            Dim ka(0) As String
            ka(0) = "ContractID"
            gvResults.datakeynames = ka
            gvResults.databind()
        Catch ex As Exception

        Finally
            If cn.State <> Data.ConnectionState.Closed Then
                cn = Nothing
                cm = Nothing
                dr = Nothing
            End If

        End Try
    End Sub

    Protected Sub gvResults_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResults.SelectedIndexChanged
        Response.Redirect("~/marketing/editContract.aspx?contractid=" & gvResults.SelectedIndex)
    End Sub
End Class
