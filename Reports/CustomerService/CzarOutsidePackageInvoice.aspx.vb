Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Threading

Partial Class Reports_CustomerService_CzarOutsidePackageInvoice
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    End Sub

    Protected Sub gvReport_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReport.RowDataBound
        If e.Row.RowType = DataControlRowType.Footer Then
            Dim dt As DataTable = CType(gvReport.DataSource, DataTable)
            e.Row.Cells(12).Text = String.Format("{0:F2}", dt.AsEnumerable().Sum(Function(x) x("Payment Due").ToString()))
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(8).Text = String.Format("{0:F2}", Convert.ToDecimal(e.Row.Cells(8).Text))
            e.Row.Cells(9).Text = String.Format("{0:F2}", Convert.ToDecimal(e.Row.Cells(9).Text))
            e.Row.Cells(10).Text = String.Format("{0:F2}", Convert.ToDecimal(e.Row.Cells(10).Text))
            e.Row.Cells(11).Text = String.Format("{0:F2}", Convert.ToDecimal(e.Row.Cells(11).Text))
            e.Row.Cells(12).Text = String.Format("{0:F2}", Convert.ToDecimal(e.Row.Cells(12).Text))
        End If
    End Sub

    Protected Sub btn_Submit_Click(sender As Object, e As System.EventArgs) Handles btn_Submit.Click
        Dim sd As DateTime = New DateTime(2013, 8, 1)
        Dim ed As DateTime = sd.AddMonths(16)
        sd = dteSDate.Selected_Date
        ed = dteEDate.Selected_Date

        If dteEDate.Selected_Date.Length = 0 Or dteSDate.Selected_Date.Length = 0 Then Return

        Dim sqlText = String.Format( _
                "SELECT ROW = row_number() over (order by xx.packageissuedid), xx.* , (select comboItem from t_ComboItems where comboItemID = r.ResLocationID) 'Location' FROM " & _
                "(SELECT a.PackageIssuedID, p.LastName + ', ' + p.Firstname as Prospect, b.Package, " & _
                "(Select top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and active = 1) as Phone, convert(varchar(10), e.DateCreated, 101) [Date Created], ps.ComboItem AS Status " & _
                "FROM t_PackageIssued a INNER JOIN t_Prospect p ON a.ProspectID = p.ProspectID INNER JOIN t_Package b ON a.PackageID = b.PackageID " & _
                "INNER JOIN t_Event e ON a.PackageIssuedID = e.KeyValue INNER JOIN t_ComboItems ps ON a.StatusID = ps.ComboItemID " & _
                "WHERE (b.Package LIKE 'CZAR%' or b.package like 'flo%' or b.package like 'cali%' or b.Package like 'ORL%' or b.package like 'Daytona%') AND (e.Type = 'Create') and e.KeyField = 'PackageIssuedID' and " & _
                "e.DateCreated between '{0}' and '{1}') xx inner join t_reservations r on xx.packageissuedid = r.packageissuedid and r.prospectid > 0 " & _
                "and r.ResLocationID not in ( " & _
                "select comboitemid from t_Combos a inner join t_ComboItems b on a.ComboID = b.ComboID where a.ComboName = 'reservationlocation' and b.ComboItem in ('kcp', 'williamsburg') " & _
                ") and r.ResortCompanyID not in ( " & _
                "select comboitemid from t_Combos a inner join t_ComboItems b on a.ComboID = b.ComboID where a.ComboName = 'resortcompany' and b.ComboItem in ('kcp')) " & _
                "ORDER BY xx.PackageIssuedID;", sd.ToShortDateString(), CDate(ed).AddDays(1).ToShortDateString())

        'code change per wo# 43712
        sqlText = String.Format( _
             "SELECT ROW = row_number() over (order by xx.packageissuedid), xx.* , (select comboItem from t_ComboItems where comboItemID = r.ResLocationID) 'Location' FROM " & _
                "(SELECT a.PackageIssuedID, p.LastName + ', ' + p.Firstname as Prospect, b.Package,  " & _
                "(Select top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and active = 1) as Phone, convert(varchar(10), e.DateCreated, 101) [Date Created], ps.ComboItem AS Status " & _
                "FROM t_PackageIssued a INNER JOIN t_Prospect p ON a.ProspectID = p.ProspectID INNER JOIN t_Package b ON a.PackageID = b.PackageID " & _
                "INNER JOIN t_Event e ON a.PackageIssuedID = e.KeyValue INNER JOIN t_ComboItems ps ON a.StatusID = ps.ComboItemID " & _
                "left join t_PackageFinTransCode pftc on pftc.PackageID = a.PackageID left join t_FinTransCodes ftc on ftc.FinTransID = pftc.FinTransCodeID " & _
                "left join t_ComboItems tc on tc.ComboItemID = ftc.TransCodeID " & _
                "WHERE (b.Package LIKE 'CZAR%' or b.package like 'flo%' or b.package like 'cali%' or b.Package like 'ORL%' or b.package like 'Daytona%') AND (e.Type = 'Create') and e.KeyField = 'PackageIssuedID' and " & _
                "e.DateCreated between '{0}' and '{1}' and tc.ComboItem = 'czar outside') xx inner join t_reservations r on xx.packageissuedid = r.packageissuedid and r.prospectid > 0 " & _
                "ORDER BY xx.PackageIssuedID;", sd.ToShortDateString(), CDate(ed).AddDays(1).ToShortDateString())

        Using cn = New SqlConnection(Resources.Resource.cns)
            Using ad = New SqlDataAdapter(sqlText, cn)
                Try
                    Dim dt = New DataTable()
                    ad.Fill(dt)

                    Dim invoicedCol As New DataColumn() With {.Caption = "Amount Invoiced", .DefaultValue = "0.00", .ColumnName = "Amount Invoiced", .DataType = System.Type.GetType("System.Decimal")}
                    Dim refundedCol As New DataColumn() With {.Caption = "Amount Refunded", .DefaultValue = "0.00", .ColumnName = "Amount Refunded", .DataType = System.Type.GetType("System.Decimal")}
                    Dim collectedCol As New DataColumn() With {.Caption = "Amount Collected", .DefaultValue = "0.00", .ColumnName = "Amount Collected", .DataType = System.Type.GetType("System.Decimal")}
                    Dim collectedTotalCol As New DataColumn() With {.Caption = "Total Collected", .DefaultValue = "0.00", .ColumnName = "Total Collected", .DataType = System.Type.GetType("System.Decimal")}
                    Dim paymentDueCol As New DataColumn() With {.Caption = "Payment Due", .DefaultValue = "0.00", .ColumnName = "Payment Due", .DataType = System.Type.GetType("System.Decimal")}

                    dt.Columns.AddRange(New DataColumn() {invoicedCol, refundedCol, collectedCol, collectedTotalCol, paymentDueCol})

                    If cn.State = ConnectionState.Closed Then cn.Open()

                    For Each dr As DataRow In dt.Rows
                        ad.SelectCommand.CommandText = String.Format( _
                                "Select Case " & _
                                "When Sum(Amount) IS NULL then 0 else Sum(amount) end as Amt " & _
                                "from UFN_Financials(0,'PackageIssuedID',{0},0) where (invoice like 'CZAR%' or invoice like 'flo%' or invoice like 'cali%') ", dr.Item("PackageIssuedID").ToString())
                        dr(invoicedCol) = ad.SelectCommand.ExecuteScalar()
                        ad.SelectCommand.CommandText = String.Format( _
                                "Select p.paymentId, coalesce(p.Amount, 0) Amount from t_Invoices i  " & _
                                "inner join t_Payment2Invoice pi on i.InvoiceID = pi.InvoiceID inner join t_Payments p on pi.PaymentID = p.PaymentID  " & _
                                "inner join t_FinTransCodes f on i.Fintransid = f.FintransID inner join t_ComboItems c on f.TransCodeID = c.ComboItemID " & _
                                "where i.KeyField = 'PackageIssuedID' and i.KeyValue = {0} and p.Adjustment = 0 and p.PosNeg = 1 and (c.ComboItem like 'CZAR%' or c.ComboItem like 'flo%' or c.ComboItem like 'cali%' ) ", _
                                dr.Item("PackageIssuedID").ToString())

                        Dim rd As SqlDataReader = ad.SelectCommand.ExecuteReader()
                        Dim l As New List(Of Int32)
                        While rd.Read()
                            l.Add(rd("paymentid").ToString)
                            dr(collectedCol) = (Convert.ToDecimal(dr(collectedCol.ToString()) + rd("Amount").ToString()))
                        End While
                        rd.Close()
                        For Each i In l
                            Dim cm = New SqlCommand(String.Format( _
                                "Select Case when Sum(Amount) is null then 0 else Sum(Amount) end as Amount " & _
                                "from t_payments where applytoid = {0} and posNeg = 0 and adjustment = 0", i), cn)
                            dr(refundedCol) += (Convert.ToDecimal(dr(refundedCol).ToString()) + cm.ExecuteScalar())
                        Next
                        dr(collectedTotalCol) = dr(collectedCol) - dr(refundedCol)
                        dr(paymentDueCol) = dr(collectedTotalCol)
                    Next

                    gvReport.DataSource = dt
                    gvReport.DataBind()
                Catch ex As Exception
                    Response.Write(ex.Message)
                Finally
                    cn.Close()
                End Try
            End Using
        End Using

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As System.Web.UI.Control)
    End Sub

    Protected Sub btn_Excel_Click(sender As Object, e As System.EventArgs) Handles btn_Excel.Click
        Response.AddHeader("content-disposition", "attachment;filename=FileName.xls")
        Response.Charset = String.Empty
        Response.ContentType = "application/vnd.xls"
        Dim sw = New System.IO.StringWriter()
        Dim tw As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(sw)
        gvReport.RenderControl(tw)
        Response.Write(sw.ToString())
        Response.End()
    End Sub
End Class
