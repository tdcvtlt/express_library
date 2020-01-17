Imports System
Imports System.Linq
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Script.Serialization

Partial Class Reports_CustomerService_RecGov_TSPackages
    Inherits System.Web.UI.Page

    Dim cnx As String = Resources.Resource.cns


    Private Function GetRecGovTSPackages(ByVal dateStart As DateTime, ByVal dateEnd As DateTime) As String

        Dim sql As String = String.Format("Select pi.PackageIssuedID, p.Firstname + ' ' + p.LastName as Prospect, " & _
                                        "ps.ComboItem as PackageStatus, pi.PurchaseDate, " & _
                                        "(Select FirstName + ' ' + LastName from t_PersonnelTrans a " & _
                                        "inner join t_Personnel b on a.personnelid = b.personnelid " & _
                                        "        where(a.KeyValue = pi.packageissuedid) " & _
                                        "and a.titleid = (Select comboitemid from t_ComboItems a inner join t_combos b " & _
                                        "on a.comboid = b.comboid " & _
                                        "where comboname = 'PersonnelTitle' and comboitem = 'Tradeshow Solicitor')) OPC " & _
                                        " from t_packageIssued pi " & _
                                        "inner join t_Prospect p on pi.ProspectID = p.ProspectID left outer join " & _
                                        "t_Comboitems ps on pi.StatusID = ps.ComboItemID where pi.PackageID in " & _
                                        "(Select PackageID from t_Package where package = 'RecGov-TS') " & _
                                        "and pi.PurchaseDate between '{0}' and '{1}' order by purchasedate desc", dateStart, dateEnd)
        Dim html As New StringBuilder()

        Using cnn As New SqlConnection(cnx)
            Using ada As New SqlDataAdapter(sql, cnn)


                ada.SelectCommand.CommandTimeout = 120

                Dim ds As New DataSet

                ada.Fill(ds, "Packages")
                html.Append("<table style='border-collapse:collapse;' id='htmlReport' border='1px'>")

                Dim groupedOpc = ds.Tables("Packages").AsEnumerable().GroupBy(Function(g) g.Field(Of String)("OPC"))
                For Each g As IGrouping(Of String, DataRow) In groupedOpc
                    html.Append("<tr><td>Package ID</td><td>Prospect</td><td>Status</td><td>Date Sold</td></tr>")
                    html.AppendFormat("<tr><td colspan='4'>{0}</td></tr>", g.Key)
                    For Each r As DataRow In g
                        html.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", _
                                          r.Field(Of Integer)("PackageIssuedID"), _
                                          r.Field(Of String)("Prospect"), _
                                          r.Field(Of String)("PackageStatus"), _
                                          r.Field(Of DateTime)("PurchaseDate"))
                    Next
                    html.AppendFormat("<tr><td colspan='3'>&nbsp;</td><td>{0}</td>", g.Count())
                Next
            End Using

            Return IIf(String.IsNullOrEmpty(html.ToString()), "No Records", html.ToString())
        End Using
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Write(GetRecGovTSPackages("3/1/11", "4/27/12"))

    End Sub
End Class
