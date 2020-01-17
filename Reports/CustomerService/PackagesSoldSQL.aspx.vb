Imports System
Imports System.Linq
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Script.Serialization

Partial Class Reports_CustomerService_PackagesSoldSQL
    Inherits System.Web.UI.Page

    Dim cnx As String = Resources.Resource.cns


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim reportName As String = Request.QueryString("reportname")
        Dim dateStart As String = Request.QueryString("dateStart")
        Dim dateEnd As String = Request.QueryString("dateEnd")
       
        If String.IsNullOrEmpty(reportName) = False Then
            If reportName.Equals("PackageSold") _
                And String.IsNullOrEmpty(dateStart) = False And String.IsNullOrEmpty(dateEnd) = False Then
                Response.Write(GetPackagesSold(dateStart, dateEnd))
            End If
        End If

    End Sub

    Private Function GetPackagesSold(ByVal dateStart As DateTime, ByVal dateEnd As DateTime) As String

        Dim sql As String = String.Format( _
                        "Select g.*, h.VSNumber " & _
                        "from ( " & _
                        "    Select e.*, f.ComboItem as PackageStatus " & _
                        "        from (Select c.*, d.FirstName, d.LastName " & _
                        "            from (Select a.PackageIssuedID, a.PackageID, a.ProspectID, " & _
                        " a.PurchaseDate, a.StatusID, b.Package " & _
                        "            from t_PackageIssued a inner join t_Package b on a.PackageID = b.PackageID " & _
                        "    where a.PurchaseDate between '{0}' and '{1}') c " & _
                        "left outer join t_Prospect d on c.prospectid = d.prospectid) e left outer join " & _
                        "t_ComboItems f on e.Statusid = f.comboitemid) g left outer join " & _
                        "t_VoiceStamps h on g.PackageIssuedID = h.KeyValue and h.KeyField = 'PackageIssuedID' " & _
                        "order by g.PurchaseDate asc", dateStart, dateEnd)

        Dim html As New StringBuilder()
        Using cnn As New SqlConnection(cnx)
            Using cmd As New SqlCommand(Sql, cnn)

                cnn.Open()

                Dim column As String = "<table style='border-collapse:collapse;' id='htmlReport' border='1px'>"
                html.Append(column)

                html.AppendFormat("<tr><td>Package Issued ID</td><td>Prospect</td><td>Package</td><td>Date Purchased</td><td style='width:100px;text-align:right'>VS #</td></tr>")
                Dim rdr As SqlDataReader = cmd.ExecuteReader()
                If rdr.HasRows Then
                    Do While rdr.Read()
                        column = String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td style='width:100px;text-align:right'>{4}</td></tr>", _
                                rdr.Item("PackageIssuedID"), _
                                rdr.Item("FirstName") & " " & rdr.Item("LastName"), _
                                rdr.Item("Package"), _
                                DateTime.Parse(rdr.Item("PurchaseDate")).ToShortDateString(), _
                                rdr.Item("VSNumber"))
                        html.Append(column)
                    Loop
                End If

                html.Append("</table>")
                Return IIf(rdr.HasRows, html.ToString(), "No Records")
            End Using
        End Using
    End Function

End Class
