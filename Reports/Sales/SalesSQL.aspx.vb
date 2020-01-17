Imports System
Imports System.Linq
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Script.Serialization



Partial Public Class Reports_Sales_SalesSQL
    Inherits System.Web.UI.Page

    Private cnx As String = Resources.Resource.cns

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim titles As String = Request.QueryString("titles")
        Dim startDate As String = Request.QueryString("startDate")
        Dim endDate As String = Request.QueryString("endDate")
        Dim title As String = Request.QueryString("title")
        Dim tourLocationID As String = Request.QueryString("tourLocationID")

        If String.IsNullOrEmpty(titles) = False Then
            Response.Write(GetTitles())
        ElseIf String.IsNullOrEmpty(startDate) = False And String.IsNullOrEmpty(endDate) = False _
            And String.IsNullOrEmpty(title) = False Then

            GetReport(startDate, endDate, title, tourLocationID)
        End If

    End Sub


    Private Function GetTitles() As String

        Dim sql As String = "Select Distinct(A.ComboItem), ComboItemID from t_ComboItems A INNER JOIN T_COMBOS B " & _
                            "ON A.COMBOID = B.COMBOID where comboname = 'PersonnelTitle' order by ComboItem asc"

        Dim js As String = String.Empty

        Using cnn As New SqlConnection(cnx)
            Using cmd As New SqlCommand(sql, cnn)

                cnn.Open()
                Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                Dim list As IList(Of IdName) = New List(Of IdName)
                If rdr.HasRows Then
                    While rdr.Read()
                        list.Add(New IdName With {.ID = rdr.Item("COMBOITEMID"), .Name = rdr.Item("COMBOITEM")})
                    End While
                End If

                js = New JavaScriptSerializer().Serialize(list.ToArray())
            End Using
        End Using

        Return js.ToString()
    End Function

    Private Sub GetReport(ByVal dateStart As DateTime, ByVal dateEnd As DateTime, ByVal titleId As Integer, tourLocationID As Integer)

        Dim path As String = HttpRuntime.AppDomainAppPath + "SQL\GetReportPersonnelPerformanceReport.txt"
        Dim sql As String = File.ReadAllText(path)

        sql = String.Format(sql, dateStart.ToShortDateString(), _
                                dateEnd.ToShortDateString(), _
                                titleId, tourLocationID)

        Dim html As New StringBuilder()
        Dim tableHeader() As String = {"ID", "Personnel", "Tours", "Active Contracts", _
                                       "Active Volume", "VPG", "Pending Contracts", _
                                       "Pending Contract Volume", "Closing %", "Rescinds", "Rescinded Volume"}

        html.AppendFormat("<table id='reportHtml' style='border-collapse:collapse;' border='1px'><caption>Sales Efficiency</caption>")
        html.Append("<tr>")
        For Each s As String In tableHeader
            html.AppendFormat("<td>{0}</td>", s)
        Next
        html.Append("</tr>")

        Using cnn As New SqlConnection(cnx)
            Using ada As New SqlDataAdapter(sql, cnn)

                Dim ds As New DataSet()

                ada.Fill(ds, "efficiency")

                Dim rows = From e In ds.Tables("efficiency").AsEnumerable() _
                           Select e

                For Each r As DataRow In rows
                    html.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>" & _
                                      "<td>{4:n2}</td><td>{5:n2}</td><td>{6}</td><td>{7:n2}</td>" & _
                                      "<td>{8:0.0%}</td><td>{9}</td><td>{10:n2}</td></tr>", _
                                      r.Field(Of Integer)("PersonnelID"), _
                                      r.Field(Of String)("Personnel"), _
                                      r.Field(Of Integer)("Tours"), _
                                      r.Field(Of Integer)("Contracts"), _
                                      r.Field(Of Decimal)("ActiveVolume"), _
                                      r.Field(Of Decimal)("VPG"), _
                                      r.Field(Of Integer)("PenderCount"), _
                                      r.Field(Of Decimal)("PenderVolume"), _
                                      r.Field(Of Integer)("Contracts") / IIf(r.Field(Of Integer)("Tours") = 0, 1, r.Field(Of Integer)("Tours")), _
                                      r.Field(Of Integer)("Rescinds"), _
                                      r.Field(Of Decimal)("RescindVolume"))
                Next


                If rows.Count() > 0 Then

                    html.AppendFormat("<tr><td>&nbsp;</td><td>&nbsp;</td><td>{0}</td>" & _
                                      "<td>{1}</td><td>{2:c2}</td><td>{3:c2}</td><td>{4}</td><td>{5:c2}</td><td>{6:0.0%}</td><td>{7}</td><td>{8:c2}</td></tr>", _
                                      rows.Sum(Function(x) x.Field(Of Integer)("Tours")), _
                                      rows.Sum(Function(x) x.Field(Of Integer)("Contracts")), _
                                      rows.Sum(Function(x) x.Field(Of Decimal)("ActiveVolume")), _
                                      rows.Sum(Function(x) x.Field(Of Decimal)("ActiveVolume")) / _
                                      IIf(rows.Sum(Function(x) x.Field(Of Integer)("Tours")) = 0, 1, rows.Sum(Function(x) x.Field(Of Integer)("Tours"))), _
                                      rows.Sum(Function(x) x.Field(Of Integer)("PenderCount")), _
                                      rows.Sum(Function(x) x.Field(Of Decimal)("PenderVolume")), _
                                      rows.Sum(Function(x) x.Field(Of Integer)("Contracts")) / _
                                      IIf(rows.Sum(Function(x) x.Field(Of Integer)("Tours")) = 0, 1, rows.Sum(Function(x) x.Field(Of Integer)("Tours"))), _
                                      rows.Sum(Function(x) x.Field(Of Integer)("Rescinds")), _
                                      rows.Sum(Function(x) x.Field(Of Decimal)("RescindVolume")))
                End If

                Response.Write(IIf(rows.Count() = 0, "<h2>No Records</h2>", html.ToString()))
            End Using
        End Using

    End Sub

End Class
