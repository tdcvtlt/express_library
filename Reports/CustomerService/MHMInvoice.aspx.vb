Imports System.Data
Imports System.Data.SqlClient
Partial Class Reports_CustomerService_MHMInvoice
    Inherits System.Web.UI.Page

    Protected Sub btn_Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Submit.Click

        If String.IsNullOrEmpty(dteSDate.Selected_Date) Or String.IsNullOrEmpty(dteEDate.Selected_Date) Then Return
        Dim sqlText = String.Empty
        Dim sd As DateTime = DateTime.Parse(dteSDate.Selected_Date)
        Dim ed As DateTime = DateTime.Parse(dteEDate.Selected_Date)

        Using cn As New SqlConnection(Resources.Resource.cns)
            sqlText = String.Format( _
                 "SELECT rowNumber = row_number() over (order by xx.packageissuedid), xx.* , (select comboItem from t_ComboItems where comboItemID = r.ResLocationID) 'Location' FROM " & _
                 "(SELECT a.PackageIssuedID, p.LastName + ', ' + p.Firstname as Prospect, b.Package, " & _
                 "(Select top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and active = 1) as Phone, convert(varchar(10), e.DateCreated, 101) DateCreated, ps.ComboItem AS Status, " & _
                 "'0.00'[Amount Invoiced], '0.00'[Amount Refunded], '0.00'[Amount Collected], '0.00'[Total Collected], " & _
                 "'0.00'[10% Withholding],'0.00'[Deduction], '0.00'[Payment Due] " & _
                 "FROM t_PackageIssued a INNER JOIN t_Prospect p ON a.ProspectID = p.ProspectID INNER JOIN t_Package b ON a.PackageID = b.PackageID " & _
                 "INNER JOIN t_Event e ON a.PackageIssuedID = e.KeyValue INNER JOIN t_ComboItems ps ON a.StatusID = ps.ComboItemID " & _
                 "   WHERE (b.Package LIKE 'MHM%' and b.Package <> 'mhm-ts') AND (e.Type = 'Create') and e.KeyField = 'PackageIssuedID' and " & _
                 "e.DateCreated between '{0}' and '{1}') xx inner join t_reservations r on xx.packageissuedid = r.packageissuedid and r.prospectid > 0 ORDER BY xx.PackageIssuedID", sd.ToShortDateString(), CDate(ed).AddDays(1).ToShortDateString())

            Dim dt As New DataTable()
            Using ad = New SqlDataAdapter(sqlText, cn)
                Try
                    ad.Fill(dt)
                    gvR.DataSource = dt
                    gvR.DataBind()

                Catch ex As Exception
                    Response.Write(ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Protected Sub gvR_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvR.RowDataBound
        Dim gvr As GridViewRow = e.Row
        Dim gv As GridView = CType(gvr.NamingContainer, GridView)

        If gvr.RowType = DataControlRowType.DataRow Then
            Dim id_package_issued = gvr.Cells(1).Text.Trim()
            Dim id_payment = 0

            'amount invoiced
            Using cn = New SqlConnection(Resources.Resource.cns)
                Dim sqlText = String.Format( _
                                "Select Case " & _
                                "When Sum(Amount) IS NULL then 0 else Sum(amount) end as Amt " & _
                                "from UFN_Financials(0,'PackageIssuedID',{0},0) where (invoice like 'MHM%') ", id_package_issued)

                Using cm = New SqlCommand(sqlText, cn)
                    Try
                        If cn.State = ConnectionState.Closed Then cn.Open()
                        'write amount invoiced to cell
                        '
                        gvr.Cells(7).Text = CType(cm.ExecuteScalar(), Decimal).ToString("N2")

                        'get amount collected per package
                        '
                        sqlText = String.Format( _
                                    "Select p.paymentId, coalesce(p.Amount, 0) Amount from t_Invoices i  " & _
                                    "inner join t_Payment2Invoice pi on i.InvoiceID = pi.InvoiceID inner join t_Payments p on pi.PaymentID = p.PaymentID  " & _
                                    "inner join t_FinTransCodes f on i.Fintransid = f.FintransID inner join t_ComboItems c on f.TransCodeID = c.ComboItemID " & _
                                    "where i.KeyField = 'PackageIssuedID' and i.KeyValue = {0} and p.Adjustment = 0 and p.PosNeg = 1 " & _
                                    "and (c.ComboItem like 'MHM%') ", _
                                                   id_package_issued)

                        Using ad = New SqlDataAdapter(sqlText, cn)
                            Dim dt = New DataTable()
                            ad.Fill(dt)

                            'write amount collected to cell
                            '
                            gvr.Cells(9).Text = dt.AsEnumerable().Sum(Function(x) Convert.ToDecimal(x.Item("Amount").ToString())).ToString("N2")

                            For Each dr As DataRow In dt.Rows

                                id_payment = dr.Item("PaymentID").ToString()

                                'get amount refunded
                                '
                                sqlText = String.Format( _
                                            "Select Case when Sum(Amount) is null then 0 else Sum(Amount) end as Amount " & _
                                            "from t_payments where applytoid = {0} and posNeg = 0 and adjustment = 0", id_payment)

                                If cn.State = ConnectionState.Closed Then cn.Open()
                                Dim amt_refunded As Decimal = Convert.ToDecimal(gvr.Cells(8).Text)
                                amt_refunded += CType(New SqlCommand(sqlText, cn).ExecuteScalar(), Decimal)
                                '
                                'write amount refunded to cell
                                gvr.Cells(8).Text = amt_refunded
                            Next

                            gvr.Cells(8).Text = Convert.ToDecimal(gvr.Cells(8).Text).ToString("N2")
                        End Using


                        If Array.IndexOf(New String() {"KCP", "WILLIAMSBURG"}, gvr.Cells(14).Text.ToUpper().Trim()) < 0 Then
                            gvr.Cells(12).Text = "14.00"
                        End If

                        ' total collected
                        '
                        gvr.Cells(10).Text = (Decimal.Parse(gvr.Cells(9).Text) - Decimal.Parse(gvr.Cells(8).Text)).ToString("N2")

                        '10 % withheld
                        gvr.Cells(11).Text = (Decimal.Round(Convert.ToDecimal(gvr.Cells(10).Text) * 0.1, 2, MidpointRounding.AwayFromZero)).ToString("N2")

                        '
                        'amoutn due
                        gvr.Cells(13).Text = (Decimal.Parse(gvr.Cells(10).Text) - Decimal.Parse(gvr.Cells(11).Text) - _
                                                                                                    Decimal.Parse(gvr.Cells(12).Text)).ToString("N2")


                    Catch ex As Exception
                        Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using

            End Using


        End If

        If gvr.RowType = DataControlRowType.Footer Then
            Dim v = gv.Rows.OfType(Of GridViewRow).Where(Function(x) x.RowType = DataControlRowType.DataRow).Select(Function(x) Convert.ToDecimal(x.Cells(13).Text))
            gvr.Cells(13).Text = v.Sum().ToString("c")
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


    Public Overrides Sub VerifyRenderingInServerForm(control As System.Web.UI.Control)
        'MyBase.VerifyRenderingInServerForm(control)
        'Must have this line of code to prevent runtime error when exporting the gridview to excel.
    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        Response.Clear()

        Response.AddHeader("content-disposition", "attachment;filename=MHMinvoice.xls")
        Response.Charset = String.Empty
        Response.ContentType = "application/vnd.xls"

        Dim sw = New System.IO.StringWriter()
        Dim htmlTW = New System.Web.UI.HtmlTextWriter(sw)
        gvR.RenderControl(htmlTW)
        Response.Write(sw.ToString())
        Response.End()

    End Sub



End Class
