Imports System.Data
Imports System.Data.SqlClient

Partial Class Reports_CustomerService_CzarInvoice
    Inherits System.Web.UI.Page


    Private Sub Old_Code()

    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        Dim rs3 As Object
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")
        rs3 = Server.CreateObject("ADODB.Recordset")
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim sAns As String = ""
        Dim totalPaymentDue As Double = 0
        Dim amtCollected As Double = 0
        Dim amtRefunded As Double = 0
        sAns = "<H2>Czar Invoice " & sdate & " - " & edate & "</H2>"
        sAns = sAns & "<table>"
        sAns = sAns & "<tr>"
        sAns = sAns & "<th><u>PackageID</u></th>"
        sAns = sAns & "<th><u>Prospect</u></th>"
        sAns = sAns & "<th><u>HomePhone</u></th>"
        sAns = sAns & "<th><u>DateCreated</u></th>"
        sAns = sAns & "<th><u>Status</u></th>"
        sAns = sAns & "<th><u>Amt Invoiced</u></th>"
        sAns = sANs & "<th><u>Amt Refunded</u></th>"
        sAns = sAns & "<th><u>Amt Collected</u></th>"
        sAns = sAns & "<th><u>Total Collected</u></th>"
        sAns = sAns & "<th><u>Payment Due</u></th>"
        sAns = sAns & "</tr>"
        Server.ScriptTimeout = 10000

        Dim DBNAME = "CRMSNet"
        Dim DBUser = "asp"
        Dim DBpass = "aspnet"

        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 10000

        rs.Open("SELECT xx.* FROM " & _
          "(SELECT a.PackageIssuedID, p.LastName + ', ' + p.Firstname as Prospect, (Select top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and active = 1) as HomePhone, convert(varchar(10), e.DateCreated) DateCreated, ps.ComboItem AS Status " & _
          "FROM t_PackageIssued a INNER JOIN t_Prospect p ON a.ProspectID = p.ProspectID INNER JOIN t_Package b ON a.PackageID = b.PackageID INNER JOIN t_Event e ON a.PackageIssuedID = e.KeyValue INNER JOIN t_ComboItems ps ON a.StatusID = ps.ComboItemID " & _
                   "WHERE (b.Package LIKE 'CZAR%' or b.package like 'flo%' or b.package like 'cali%' or b.Package like 'Daytona%' or b.Package like 'ORL%') AND (e.Type = 'Create') and e.KeyField = 'PackageIssuedID' and e.DateCreated between '" & sdate & "' and '" & CDate(edate).AddDays(1) & "') xx ORDER BY xx.PackageIssuedID", cn, 3, 3)

        If rs.EOF And rs.BOF Then
            sAns = sAns & "<tr><td colspan = '8'>No Packages Created in this Date Range.</td></tr>"
        Else
            Do While Not rs.EOF
                amtCollected = 0
                amtRefunded = 0
                sAns = sAns & "<tr>"
                sAns = sAns & "<td>" & rs.Fields("PackageIssuedID").Value
                sAns = sAns & "<td>" & rs.Fields("Prospect").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("HomepHone").Value & "</td>"
                sAns = sAns & "<td>" & CDate(rs.Fields("DateCreated").Value) & "</td>"
                sAns = sAns & "<td>" & rs.Fields("Status").Value & "</td>"
                rs2.open("Select Case when Sum(Amount) is null then 0 else sum(amount) end as Amt from UFN_Financials(0,'PackageIssuedID'," & rs.Fields("PackageIssuedID").Value & ",0) where invoice like 'CZAR%' or invoice like 'flo%' or invoice like 'cali%'", cn, 0, 1)
                sAns = sAns & "<td>" & FormatCurrency(rs2.Fields("Amt").Value, 2) & "</td>"
                If rs2.FIelds("Amt").Value = 0 Then
                    sAns = sAns & "<td>" & FormatCurrency(0, 2) & "</td>"
                    sAns = sAns & "<td>" & FormatCurrency(0, 2) & "</td>"
                    sAns = sAns & "<td>" & FormatCurrency(0, 2) & "</td>"
                    sAns = sAns & "<td>" & FormatCurrency(0, 2) & "</td>"
                    rs2.Close()
                Else
                    rs2.Close()
                    rs2.Open("Select p.PaymentID, p.Amount from t_Invoices i inner join t_Payment2Invoice pi on i.InvoiceID = pi.InvoiceID inner join t_Payments p on pi.PaymentID = p.PaymentID inner join t_FinTransCodes f on i.Fintransid = f.FintransID inner join t_ComboItems c on f.TransCodeID = c.ComboItemID where i.KeyField = 'PackageIssuedID' and i.KeyValue = " & rs.Fields("PackageIssuedID").Value & " and p.Adjustment = 0 and p.PosNeg = 1 and (c.ComboItem like 'CZAR%' or c.comboitem like 'flo%' or c.comboitem like 'cali%' )", cn, 3, 3, )
                    Do While Not rs2.EOF
                        amtCollected = amtCollected + rs2.Fields("Amount").Value
                        rs3.Open("Select Case when Sum(Amount) is null then 0 else Sum(Amount) end as Amount from t_payments where applytoid = " & rs2.Fields("PaymentID").Value & " and posNeg = 0 and adjustment = 0", cn, 3, 3)
                        If rs3.EOF And rs3.BOF Then
                        Else
                            amtRefunded = amtRefunded + rs3.Fields("Amount").Value
                        End If
                        rs3.Close()
                        rs2.MoveNext()
                    Loop
                    rs2.Close()
                    sAns = sAns & "<td>" & FormatCurrency(amtRefunded, 2) & "</td>"
                    sAns = sAns & "<td>" & FormatCurrency(amtCollected, 2) & "</td>"
                    sAns = sAns & "<td>" & FormatCurrency(amtCollected - amtRefunded, 2) & "</td>"
                    sAns = sAns & "<td align = 'right'>" & FormatCurrency((amtCollected - amtRefunded) * 0.9, 2) & "</td>"
                End If
                sAns = sAns & "</tr>"
                totalPaymentDue = totalPaymentDue + CDbl((amtCollected - amtRefunded) * 0.9)
                rs.moveNext()
            Loop
        End If
        rs.Close()
        sAns = sAns & "<tr><td colspan = '7'></td><td><B>Total Due:</b></td><td colspan = '2' align = 'right'><B>" & FormatCurrency(totalPaymentDue, 2) & "</td></tr>"
        sAns = sAns & "</table>"
        cn.Close()
        rs = Nothing
        rs2 = Nothing
        rs3 = Nothing
        cn = Nothing
        litReport.Text = sAns

    End Sub


    Private ReadOnly Property ConnectionString As String
        Get
            Return Resources.Resource.cns
        End Get
    End Property

    Protected Sub btn_Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Submit.Click

        If String.IsNullOrEmpty(dteSDate.Selected_Date) Or String.IsNullOrEmpty(dteEDate.Selected_Date) Then Return

        Dim col_headers() As String = {"Row No.", "Package ID", "Package", "Prospect", "Phone", "Date Created", "Status", "Amount Invoiced", "Amount Refunded", "Amount Collected", "Total Collected", "% Withheld", "Deduction", "Payment Due"}

        Dim sql As String = String.Empty
        Dim sd As DateTime = DateTime.Parse(dteSDate.Selected_Date)
        Dim ed As DateTime = DateTime.Parse(dteEDate.Selected_Date)
        Dim sb As New StringBuilder()

        Using con As New SqlConnection(ConnectionString)

            sql = String.Format( _
                "SELECT rowNumber = row_number() over (order by xx.packageissuedid), xx.* , (select comboItem from t_ComboItems where comboItemID = r.ResLocationID) 'Location' FROM " & _
                "(SELECT a.PackageIssuedID, p.LastName + ', ' + p.Firstname as Prospect, b.Package, " & _
                "(Select top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and active = 1) as Phone, convert(varchar(10), e.DateCreated, 101) DateCreated, ps.ComboItem AS Status " & _
                "FROM t_PackageIssued a INNER JOIN t_Prospect p ON a.ProspectID = p.ProspectID INNER JOIN t_Package b ON a.PackageID = b.PackageID " & _
                "INNER JOIN t_Event e ON a.PackageIssuedID = e.KeyValue INNER JOIN t_ComboItems ps ON a.StatusID = ps.ComboItemID " & _
                "   WHERE (b.Package LIKE 'CZAR%' or b.package like 'flo%' or b.package like 'cali%' or b.Package like 'Daytona%' or b.Package like 'ORL%') AND (e.Type = 'Create') and e.KeyField = 'PackageIssuedID' and " & _
                "e.DateCreated between '{0}' and '{1}') xx inner join t_reservations r on xx.packageissuedid = r.packageissuedid and r.prospectid > 0 " & _
                "and r.ResLocationID  in ( " & _
                "select comboitemid from t_Combos a inner join t_ComboItems b on a.ComboID = b.ComboID where a.ComboName = 'reservationlocation' and b.ComboItem in ('kcp', 'williamsburg') " & _
                ") and r.ResortCompanyID  in ( " & _
                "select comboitemid from t_Combos a inner join t_ComboItems b on a.ComboID = b.ComboID where a.ComboName = 'resortcompany' and b.ComboItem in ('kcp')) " & _
                "ORDER BY xx.PackageIssuedID", sd.ToShortDateString(), CDate(ed).AddDays(1).ToShortDateString())

            Dim dt As New DataTable()

            Using ada As New SqlDataAdapter(sql, con)
                ada.Fill(dt)

                If dt.Rows.Count > 0 Then

                    Dim kcp_packages() As String = dt.AsEnumerable().Where(Function(y) y.Item("Location").ToString().ToUpper().Equals("KCP") Or _
                                                                               y.Item("Location").ToString().ToUpper().Equals("WILLIAMSBURG")).Select(Function(x) x.Item("Location".ToUpper()).ToString()).Distinct().ToArray()

                    dt.PrimaryKey = New DataColumn() {dt.Columns(0)}
                    dt.Columns.Add("InvoiceAmount", GetType(Decimal))
                    dt.Columns.Add("PaymentAmount", GetType(Decimal))
                    dt.Columns.Add("WithholdAmount", GetType(Decimal))
                    dt.Columns.Add("Deduction", GetType(Decimal))
                    dt.Columns.Add("RefundAmount", GetType(Decimal))
                    dt.Columns.Add("PaymentDue", GetType(Decimal))
                    dt.Columns.Add("TotalCollected", GetType(Decimal))

                    sb.Append("<table border=1>")
                    sb.Append("<tr>")

                    For Each h As String In col_headers
                        sb.AppendFormat("<td><strong>{0}</strong></td>", h.ToUpper())
                    Next
                    sb.Append("</tr>")

                    For Each dr As DataRow In dt.Rows

                        sb.Append("<tr>")

                        Dim package As String = dr.Item("Package").ToString()

                        ' Amount invoiced
                        sql = String.Format( _
                                      "Select Case " & _
                                      "When Sum(Amount) IS NULL then 0 else Sum(amount) end as Amt " & _
                                      "from UFN_Financials(0,'PackageIssuedID',{0},0) where (invoice like 'CZAR%' or invoice like 'flo%' or invoice like 'cali%') ", dr.Item("PackageIssuedID").ToString())

                        con.Open()

                        Using cmd As New SqlCommand(sql, con)
                            dr.Item("InvoiceAmount") = cmd.ExecuteScalar()
                        End Using
                        con.Close()

                        sql = String.Format( _
                                                   "Select p.paymentId, coalesce(p.Amount, 0) Amount from t_Invoices i  " & _
                                                   "inner join t_Payment2Invoice pi on i.InvoiceID = pi.InvoiceID inner join t_Payments p on pi.PaymentID = p.PaymentID  " & _
                                                   "inner join t_FinTransCodes f on i.Fintransid = f.FintransID inner join t_ComboItems c on f.TransCodeID = c.ComboItemID " & _
                                                   "where i.KeyField = 'PackageIssuedID' and i.KeyValue = {0} and p.Adjustment = 0 and p.PosNeg = 1 and (c.ComboItem like 'CZAR%' or c.ComboItem like 'flo%' or c.ComboItem like 'cali%' ) ", _
                                                   dr.Item("PackageIssuedID").ToString())

                        Using adp As New SqlDataAdapter(sql, con)
                            Dim tb As New DataTable()
                            adp.Fill(tb)

                            For Each r As DataRow In tb.Rows
                                sql = String.Format( _
                                             "Select Case when Sum(Amount) is null then 0 else Sum(Amount) end as Amount " & _
                                             "from t_payments where applytoid = {0} and posNeg = 0 and adjustment = 0", r.Item("PaymentID").ToString())

                                Using cmd As New SqlCommand(sql, con)
                                    con.Open()
                                    Dim ref As Decimal = IIf(dr.Item("RefundAmount").Equals(DBNull.Value), 0, dr.Item("RefundAmount"))
                                    ref += cmd.ExecuteScalar()
                                    dr.Item("RefundAmount") = ref
                                    con.Close()
                                End Using

                            Next

                            dr.Item("PaymentAmount") = tb.AsEnumerable().Sum(Function(x) Convert.ToDecimal(x.Item("Amount").ToString()))
                        End Using

                        If dr.Item("RefundAmount").Equals(DBNull.Value) Then dr.Item("RefundAmount") = "0.00"

                        If DateTime.Compare(DateTime.Parse(dr.Item("DateCreated").ToString()), DateTime.Parse("08/19/2013")) >= 0 Then

                            Dim location As String = dr.Item("Location").ToString().ToUpper() '& "per work order 42215"

                            If kcp_packages.Any(Function(x) x.ToUpper().Contains(location)) Then
                                dr.Item("Deduction") = "0.00"
                                dr.Item("TotalCollected") = Decimal.Parse(dr.Item("PaymentAmount").ToString()) - Decimal.Parse(dr.Item("RefundAmount").ToString())
                                dr.Item("WithholdAmount") = Decimal.Round(Convert.ToDecimal(dr.Item("TotalCollected").ToString()) * 0.1, 2, MidpointRounding.AwayFromZero)

                                dr.Item("PaymentDue") = Decimal.Parse(dr.Item("TotalCollected").ToString()) - Decimal.Parse(dr.Item("WithholdAmount").ToString()) - _
                                                                                                                           Decimal.Parse(dr.Item("Deduction"))
                            Else

                                dr.Item("Deduction") = "14.00"
                                dr.Item("TotalCollected") = Decimal.Parse(dr.Item("PaymentAmount").ToString()) - Decimal.Parse(dr.Item("RefundAmount").ToString())
                                dr.Item("WithholdAmount") = Decimal.Round(Convert.ToDecimal(dr.Item("TotalCollected").ToString()) * 0.025, 2, MidpointRounding.AwayFromZero)

                                dr.Item("PaymentDue") = Decimal.Parse(dr.Item("TotalCollected").ToString()) - Decimal.Parse(dr.Item("WithholdAmount").ToString()) - _
                                                                                                                                                       Decimal.Parse(dr.Item("Deduction"))
                            End If
                        Else

                            dr.Item("Deduction") = "0.00"
                            dr.Item("TotalCollected") = Decimal.Parse(dr.Item("PaymentAmount").ToString()) - Decimal.Parse(dr.Item("RefundAmount").ToString())
                            dr.Item("WithholdAmount") = Decimal.Round(Convert.ToDecimal(dr.Item("TotalCollected").ToString()) * 0.1, 2, MidpointRounding.AwayFromZero)

                            dr.Item("PaymentDue") = Decimal.Parse(dr.Item("TotalCollected").ToString()) - Decimal.Parse(dr.Item("WithholdAmount").ToString()) - _
                                                                                                                       Decimal.Parse(dr.Item("Deduction"))

                        End If

                        sb.AppendFormat("<td>{0}</td>", dr.Item("rowNumber").ToString())
                        sb.AppendFormat("<td>{0}</td>", dr.Item("PackageIssuedID").ToString())
                        sb.AppendFormat("<td>{0}</td>", dr.Item("Prospect").ToString())
                        sb.AppendFormat("<td>{0}</td>", package)
                        sb.AppendFormat("<td>{0}</td>", dr.Item("Phone").ToString())
                        sb.AppendFormat("<td>{0}</td>", dr.Item("DateCreated").ToString())
                        sb.AppendFormat("<td>{0}</td>", dr.Item("Status").ToString())

                        sb.AppendFormat("<td>{0:N2}</td>", dr.Item("InvoiceAmount"))
                        sb.AppendFormat("<td>{0:N2}</td>", dr.Item("RefundAmount"))
                        sb.AppendFormat("<td>{0:N2}</td>", Decimal.Parse(dr.Item("PaymentAmount").ToString()))
                        sb.AppendFormat("<td>{0:N2}</td>", dr.Item("TotalCollected"))
                        sb.AppendFormat("<td>{0:N2}</td>", dr.Item("WithholdAmount"))
                        sb.AppendFormat("<td>{0:N2}</td>", dr.Item("Deduction"))
                        sb.AppendFormat("<td>{0:N2}</td>", dr.Item("PaymentDue"))

                        sb.Append("</tr>")
                    Next

                    sb.Append("</tr>")
                    sb.Append("<td colspan=13>&nbsp;</td>")
                    sb.AppendFormat("<td><strong>Total: ${0:N2}<strong></td>", dt.AsEnumerable().Sum(Function(x) Convert.ToDecimal(x.Item("PaymentDue").ToString())))
                    sb.Append("</tr>")

                    sb.Append("</table>")
                Else
                    sb.AppendFormat("<strong>No packages between {0} - {1} found.</strong>", sd.ToShortDateString(), ed.ToShortDateString())
                End If
            End Using
        End Using

        litReport.Text = sb.ToString()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        btn_Unnamed1.Visible = False

        If DateTime.Compare(DateTime.Now, DateTime.Parse("08/19/2013")) <= 0 Then
            btn_Unnamed1.Visible = True
        End If
    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=CzarInvoice.xls")
        Response.Write(litReport.Text)
        Response.End()
    End Sub
End Class
