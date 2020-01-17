Imports System.Data
Imports System.Data.SqlClient

Partial Class Reports_Accounting_ClubFeeRollForward
    Inherits System.Web.UI.Page

    Private ds As SqlDataSource = Nothing
    Private InvoiceID() As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            ds = New SqlDataSource()

            ds.ConnectionString = Resources.Resource.cns

            ds.SelectCommand = "select * from t_comboitems a inner join t_fintranscodes b on a.comboitemid = b.transcodeid where b.merchantaccountid = 14 order by a.comboItem"
            CheckBoxList1.AppendDataBoundItems = True
            CheckBoxList1.DataSource = ds
            CheckBoxList1.DataTextField = "ComboItem"
            CheckBoxList1.DataValueField = "ComboItemID"
            CheckBoxList1.DataBind()
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Reports()
    End Sub


    Private Sub Reports()
        
        If EnsureInputRequired() = False Then Return
        Dim paymentDateInput As Boolean = IIf(String.IsNullOrEmpty(DateField1.Selected_Date) = False And _
                                              String.IsNullOrEmpty(DateField2.Selected_Date) = False, _
                                              True, False)



        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim sb As New StringBuilder()
        Dim ad As SqlDataAdapter = Nothing
        Dim tbInvoice As New DataTable
        Dim tbPayment As New DataTable
        Dim tbValues As New DataTable

        Dim pk As DataColumn = tbValues.Columns.Add("InvoiceID", GetType(String))

        tbValues.Columns.Add("ProspectID", GetType(String))
        tbValues.Columns.Add("Prospect Name", GetType(String))
        tbValues.Columns.Add("InvoiceName", GetType(String))

        tbValues.Columns.Add("Invoice", GetType(String))
        tbValues.Columns.Add("InvoiceDate", GetType(String))
        tbValues.Columns.Add("Adjustment", GetType(String))
        tbValues.Columns.Add("AdjustmentDate", GetType(String))
        tbValues.Columns.Add("Payment", GetType(String))
        tbValues.Columns.Add("PaymentDate", GetType(String))
        tbValues.Columns.Add("Refund", GetType(String))
        tbValues.Columns.Add("RefundDate", GetType(String))  
        tbValues.Columns.Add("Balance", GetType(String))

        tbValues.PrimaryKey = New DataColumn() {pk}

        Dim sql As String = String.Format( _
            "select v.ProspectID, p.firstName + ' ' + p.lastName [Prospect Name], v.Invoice, v.TransDate, v.ID,  * " & _
            "from v_invoices v left join t_prospect p on v.prospectid = p.prospectid " & _
            "where v.invoice in ({0}) " & _
            "and transdate between '{1}' and convert(varchar(16), '{2}', 101) " & _
            "order by v.Invoice, p.ProspectID", String.Join(",", InvoiceID), dteSDate.Selected_Date, dteEDate.Selected_Date)

        ad = New SqlDataAdapter(sql, cn)
        ad.Fill(tbInvoice)

        If tbInvoice.Rows.Count > 0 Then
            For Each row As DataRow In tbInvoice.Rows
                Dim tmp As DataRow = tbValues.NewRow()

                tmp.Item("InvoiceID") = row.Item("ID")
                tmp.Item("ProspectID") = row.Item("ProspectID")
                tmp.Item("Prospect Name") = row.Item("Prospect Name")
                tmp.Item("InvoiceName") = row.Item("Invoice")
                tmp.Item("Invoice") = Convert.ToDecimal(row.Item("Amount").ToString()).ToString("N2")
                tmp.Item("InvoiceDate") = DateTime.Parse(row.Item("TransDate").ToString()).ToShortDateString()
                tbValues.Rows.Add(tmp)
            Next
        Else

            label1.Text = "No records!"
            Return

        End If

        If paymentDateInput Then
            sql = String.Format( _
                "select  *  from v_payments y " & _
                "where invoice in ({0}) " & _
                "and y.transDate between '{1}' and convert(varchar(24), '{2}', 101) " & _
                "order by invoice, keyvalue", String.Join(",", InvoiceID), DateField1.Selected_Date, DateTime.Parse(DateField2.Selected_Date).AddDays(1))
            ad.Dispose()
            ad = New SqlDataAdapter(sql, cn)
            ad.Fill(tbPayment)
            For Each r As DataRow In tbValues.Rows
                Calculate(tbValues, tbPayment, r.Item("InvoiceID").ToString())
            Next        
        End If

        Dim columnInvoice() As String = {"Prospect ID", "Prospect", "Invoice", "Amount", "Date"}
        Dim columnPayment() As String = {"Adjustment", "Date", "Payment", "Date", "Refund", "Date", "Balance"}

        sb.AppendFormat("<table border=1 style=border-collapse:collapse>")
        sb.AppendFormat("<tr>")
        If paymentDateInput = False Then
            For Each s As String In columnInvoice
                sb.AppendFormat("<td><strong>{0}</strong></td>", s.ToUpper())
            Next
        Else
            For Each s As String In columnInvoice.Concat(columnPayment)
                sb.AppendFormat("<td><strong>{0}</strong></td>", s.ToUpper())
            Next
        End If
        sb.AppendFormat("</tr>")



        For Each g In tbValues.AsEnumerable().GroupBy(Function(x) x.Item("InvoiceName"))
            For Each r As DataRow In g
                sb.AppendFormat("<tr>")

                For i As Integer = 1 To 5
                    If r.Item(i).ToString().Equals("InvoiceID") = False Then
                        sb.AppendFormat("<td>{0}</td>", r.Item(i).ToString())
                    End If
                Next
                If paymentDateInput = True Then
                    For i As Integer = 6 To tbValues.Columns.Count - 1
                        sb.AppendFormat("<td>{0}</td>", r.Item(i).ToString())
                    Next
                End If
                sb.AppendFormat("</tr>")
            Next

            If paymentDateInput = False Then
                sb.AppendFormat("<tr><td/><td/><td>{0}</td><td>{1}</td><td/></tr>", g.Count(), g.Sum(Function(x) x.Item("Invoice")).ToString("N2"))
            Else

                sb.AppendFormat("<tr><td/><td/><td>{0}</td><td>{1}</td><td/><td>{2}</td><td/><td>{3}</td><td/><td>{4}</td><td/><td>{5}</td></tr>", _
                                g.Count(), g.Sum(Function(x) x.Item("Invoice")).ToString("N2"), _
                                g.Sum(Function(x) IIf(x.Item("Adjustment").Equals(DBNull.Value), "0", x.Item("Adjustment"))).ToString("N2"), _
                                g.Sum(Function(x) IIf(x.Item("Payment").Equals(DBNull.Value), "0", x.Item("Payment"))).ToString("N2"), _
                                g.Sum(Function(x) IIf(x.Item("Refund").Equals(DBNull.Value), "0", x.Item("Refund"))).ToString("N2"), _
                                g.Sum(Function(x) IIf(x.Item("Balance").Equals(DBNull.Value), "0", x.Item("Balance"))).ToString("N2"))
            End If
        Next
       

        sb.AppendFormat("</table>")


    
        literal1.Text = sb.ToString()
    End Sub

    Private Function EnsureInputRequired() As Boolean

        Dim l As Label = label1
        Dim cbl As CheckBoxList = CheckBoxList1
        Dim i As Integer = 0, j As Integer = 0

        Array.Resize(InvoiceID, cbl.Items.Count)

        For Each li As ListItem In cbl.Items
            If li.Selected = True Then
                InvoiceID(i) = String.Format("'{0}'", li.Text)
                j += 1
            Else
                InvoiceID(i) = String.Format("'{0}'", "0")
            End If
            i += 1
        Next

        l.Text = String.Empty

        If j = 0 Then
            label1.Text = String.Format("<br/><strong>Please select at least one invoice name.</strong>")
            Return False
        Else
            If String.IsNullOrEmpty(dteSDate.Selected_Date) Or String.IsNullOrEmpty(dteEDate.Selected_Date) Then
                label1.Text = String.Format("<br/><strong>Invoice date range is not complete.</strong>")
                Return False
            Else
                Return True
            End If
        End If
    End Function


    Private Sub Calculate(ByVal valueTable As DataTable, ByVal paymentTable As DataTable, ByVal invoiceId As String)

        Dim rows() As DataRow = paymentTable.Select(String.Format("invoiceid={0}", invoiceId))
        Dim amount As Decimal = 0
        Dim maxDate As String = String.Empty
        Dim tmp As DataRow = Nothing

        Dim valueRow As DataRow = valueTable.Rows.Find(invoiceId)

        valueRow.Item("Balance") = valueRow.Item("Invoice")

        If rows.Count() > 0 Then

            '
            'Adjustments
            amount = rows.Where(Function(x) Convert.ToBoolean(x.Item("isAdjustment").ToString()) = True).Sum(Function(x) Convert.ToDecimal(x.Item("Amount").ToString()))
            tmp = rows.Where(Function(x) Convert.ToBoolean(x.Item("isAdjustment").ToString()) = True).OrderByDescending(Function(x) x.Item("TransDate")).Max()            

            If tmp IsNot Nothing Then
                maxDate = tmp.Item("TransDate").ToString()
                valueRow.Item("AdjustmentDate") = DateTime.Parse(maxDate).ToShortDateString()
            End If

            If Decimal.Parse(amount) <> 0 Then
                valueRow.Item("Adjustment") = amount.ToString("N2")
                valueRow.Item("Balance") = Decimal.Parse(valueRow.Item("Balance").ToString()) + Decimal.Parse(amount.ToString("N2"))
            End If



            '
            'Payments
            amount = rows.Where(Function(x) Convert.ToBoolean(x.Item("isAdjustment").ToString()) = False _
                                    And x.Item("Method").ToString().ToLower().IndexOf("refund", 0) = -1).Sum(Function(x) _
                    Math.Abs(Convert.ToDecimal(x.Item("Amount").ToString()))) - _
                    rows.Where(Function(x) Convert.ToBoolean(x.Item("isAdjustment").ToString()) = False _
                                   And x.Item("Method").ToString().ToLower().IndexOf("refund", 0) = -1).Sum(Function(x) _
                    Math.Abs(Convert.ToDecimal(x.Item("Adjustment").ToString())))


            tmp = rows.Where(Function(x) Convert.ToBoolean(x.Item("isAdjustment").ToString()) = False _
                                 And x.Item("Method").ToString().ToLower().IndexOf("refund", 0) = -1).OrderByDescending( _
                                 Function(x) x.Item("TransDate")).FirstOrDefault()

            If tmp IsNot Nothing Then
                valueRow.Item("Payment") = amount.ToString("N2")
                valueRow.Item("PaymentDate") = DateTime.Parse(tmp.Item("TransDate").ToString()).ToShortDateString()
                valueRow.Item("Balance") = Decimal.Parse(valueRow.Item("Balance").ToString()) - Decimal.Parse(amount.ToString("N2"))
            End If

            '
            'Refunds

            amount = rows.Where(Function(x) Convert.ToBoolean(x.Item("isAdjustment").ToString()) = False And _
                                    x.Item("Method").ToString().ToLower().IndexOf("refund", 0) > 0).Sum(Function(x) _
                                    Convert.ToDecimal(x.Item("Amount").ToString()))

            If amount > 0 Then
                valueRow.Item("Refund") = amount.ToString("N2")

                tmp = rows.Where(Function(x) Convert.ToBoolean(x.Item("isAdjustment").ToString()) = False And _
                                        x.Item("Method").ToString().ToLower().IndexOf("refund", 0) > 0).OrderByDescending( _
                                     Function(x) x.Item("TransDate")).FirstOrDefault()

                If tmp IsNot Nothing Then
                    valueRow.Item("RefundDate") = DateTime.Parse(tmp.Item("TransDate").ToString()).ToShortDateString()
                End If
                valueRow.Item("Balance") = Decimal.Parse(valueRow.Item("Balance").ToString()) + Decimal.Parse(amount.ToString("N2"))
            End If
        End If
    End Sub



    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=" + String.Format("ClubFeeRollForward-{0}.xls", DateTime.Now.ToLongTimeString()))
        Response.Write(literal1.Text)
        Response.End()
    End Sub
End Class
