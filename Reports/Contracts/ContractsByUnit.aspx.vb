Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.Security
Imports System.Web
Imports System.Configuration
Imports System.Data
Imports System

Partial Class Reports_Contracts_ContractsByUnit
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            BindData()
            
        End If
    End Sub

    Private Sub BindData()
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "select UnitID,Name from t_Unit order by Name asc"

        liUnit.DataSource = ds
        liUnit.DataTextField = "Name"
        liUnit.DataValueField = "UnitID"
        liUnit.DataBind()

        ds = Nothing
    End Sub

    Public Sub Get_Report()
        Dim cn As Object
        Dim rs As Object
        Dim sql As String

        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")

        cn.Open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        sql = "SELECT h.*, i.UFValue AS Deed FROM (SELECT g.ContractID, g.ContractNumber, g.ContractDate, g.FirstName, g.LastName, g.Week, g.OccupancyYear, h.Frequency AS Frequency FROM (SELECT e.*, f.FirstName AS FirstName, f.LastName AS LastName FROM (SELECT c.*, d .ProspectID AS ProspectID, d .ContractNumber, d .ContractDate FROM (SELECT a.ContractID, a.FrequencyID, a.OccupancyYear, b.UnitID, b.Week FROM t_SoldInventory a INNER JOIN t_SalesInventory b ON a.SalesInventoryID = b.SalesInventoryID WHERE (b.UnitID = '" & liUnit.SelectedValue & "')) c INNER JOIN t_Contract d ON c.ContractID = d .ContractID) e INNER JOIN t_Prospect f ON e.ProspectID = f.ProspectID) g INNER JOIN t_Frequency h ON g.FrequencyID = h.FrequencyID) h LEFT OUTER JOIN (SELECT KeyValue, UFValue FROM t_UF_Value WHERE UFID = (SELECT UFID FROM t_UFields WHERE UFName = 'Deed Instrument #')) i ON h.ContractID = i.KeyValue ORDER BY h.Week"
        'sql = "SELECT h.*, i.UFValue AS Deed FROM (SELECT g.ContractID, g.ContractNumber, g.ContractDate, g.FirstName, g.LastName, g.Week, g.OccupancyYear, h.Frequency AS Frequency FROM (SELECT e.*, f.FirstName AS FirstName, f.LastName AS LastName FROM (SELECT c.*, d .ProspectID AS ProspectID, d .ContractNumber, d .ContractDate FROM (SELECT a.ContractID, a.FrequencyID, a.OccupancyYear, b.UnitID, b.Week FROM t_SoldInventory a INNER JOIN t_SalesInventory b ON a.SalesInventoryID = b.SalesInventoryID WHERE (b.UnitID = '" & liUnit.SelectedValue & "')) c INNER JOIN t_Contract d ON c.ContractID = d .ContractID) e INNER JOIN t_Prospect f ON e.ProspectID = f.ProspectID) g INNER JOIN t_Frequency h ON g.FrequencyID = h.FrequencyID) h LEFT OUTER JOIN (SELECT UFValueID, UFValue FROM t_UF_Value WHERE UFID = (SELECT UFID FROM t_UFields WHERE UFName = 'Deed Instrument #')) i ON h.ContractID = i.UFValueID ORDER BY h.Week"
        rs.open(sql, cn, 0, 1)
        If rs.EOF And rs.BOF Then
            litReport.Text &= "No Contracts Tied to this Unit"
        Else
            litReport.Text &= "<table>"
            litReport.Text &= "<tr><th>Week</th><th>Contract Number</th><th>Owner</th><th>Contract Date</th><th>Frequency</th><th>OccupancyYear</th><th>Deed #</th></tr>"
            Do While Not rs.EOF
                litReport.Text &= "<tr>"
                litReport.Text &= "<td>" & rs.Fields("Week").value & "</td>"
                litReport.Text &= "<td>" & rs.Fields("ContractNumber").value & "</td>"
                litReport.Text &= "<td>" & rs.Fields("FirstName").value & " " & rs.Fields("LastName").value & "</td>"
                litReport.Text &= "<td>" & rs.Fields("ContractDate").value & "</td>"
                litReport.Text &= "<td>" & rs.Fields("Frequency").value & "</td>"
                litReport.Text &= "<td>" & rs.Fields("OccupancyYear").value & "</td>"
                litReport.Text &= "<td>" & rs.Fields("Deed").value & "</td>"
                litReport.Text &= "</tr>"
                rs.MoveNext()
            Loop
            litReport.Text &= "</table>"
        End If
        rs.Close()
        cn.Close()
        rs = Nothing
        cn = Nothing
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        litReport.Text &= "ITs runs"
        Get_Report()
    End Sub
End Class
