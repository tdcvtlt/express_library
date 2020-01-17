Imports System.Data.SqlClient
Partial Class Reports_Accounting_BoonesZReport
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim rpt As String = ""
        Dim cnString As String = ""
        Dim cn As New SqlConnection(Resources.Resource.Boones)
        cn.Open()
        Dim cm As New SqlCommand("", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        Dim dr As SqlDataReader
        Dim taxTotal As Double = 0
        Dim saleTotal As Double = 0
        Dim refTotal As Double = 0
        Dim openingTotal As Double = 0
        Dim cashTotal As Double = 0
        rpt = rpt & "<Table>"
        rpt = rpt & "<tr><td colspan = '2'>Z REPORT</td></tr>"
        rpt = rpt & "<tr><td>Report Date:</td><td>" & System.DateTime.Now.ToShortDateString & "</td></tr>" '"Report Date" & vbTab & vbTab & System.DateTime.Now.ToShortDateString & vbCrLf
        rpt = rpt & "<tr><td>Report Date:</td><td>" & System.DateTime.Now.ToLongTimeString & "</td></tr>"  '"Report Time" & vbTab & vbTab & System.DateTime.Now.ToLongTimeString & vbCrLf
        rpt = rpt & "<tr><td><br></td></tr>"

        cm.CommandText = "Select BatchID, Case when CLoseAmount is null then 0 else closeamount end as CloseAmount, OpenAmount, StartDate, Closed from t_Batch where batchID = " & ddBatch.SelectedValue
        da.Fill(ds, "Batch")
        rpt = rpt & "<tr><td>Register #:</td><td>" & 1 & "</td></tr>" 'format_Line("Register #", LocationID, lineLength) & vbCrLf '"Register #" & vbTab & vbTab & LocationID & vbCrLf
        rpt = rpt & "<tr><td>Batch #:</td><td>" & ddBatch.SelectedValue & "</td></tr>" 'format_Line("Batch #", BatchID, lineLength) & vbCrLf '"Batch #" & vbTab & vbTab & BatchID & vbCrLf
        rpt = rpt & "<tr><td>Batch Status:</td><td>" & IIf(IsDBNull(ds.Tables("Batch").Rows(0).Item("Closed")) Or Not (ds.Tables("Batch").Rows(0).Item("Closed")), "Open", "Closed") & "</td></tr>" 'format_Line("Batch Status", IIf(ds.Tables("Batch").Rows(0).Item("Closed"), "Closed", "Open"), lineLength) & vbCrLf '"Batch Status" & vbTab & vbTab & IIf(ds.Tables("Batch").Rows(0).Item("Closed"), "Closed", "Open") & vbCrLf
        rpt = rpt & "<tr><td>Start Date:</td><td>" & CDate(ds.Tables("Batch").Rows(0).Item("StartDate")).ToShortDateString & "</td></tr>" 'format_Line("Start Date", CDate(ds.Tables("Batch").Rows(0).Item("StartDate")).ToShortDateString, lineLength) & vbCrLf '"Start Date" & vbTab & vbTab & CDate(ds.Tables("Batch").Rows(0).Item("StartDate")).ToShortDateString & vbCrLf
        rpt = rpt & "<tr><td>Start Time:</td><td>" & CDate(ds.Tables("Batch").Rows(0).Item("StartDate")).ToLongTimeString & "</td></tr>" 'format_Line("Start Time", CDate(ds.Tables("Batch").Rows(0).Item("StartDate")).ToLongTimeString, lineLength) & vbCrLf '"Start Date" & vbTab & vbTab & CDate(ds.Tables("Batch").Rows(0).Item("StartDate")).ToLongTimeString & vbCrLf

        rpt = rpt & "<tr><td><br></td></tr>"
        rpt = rpt & "<tr><td>Open Amount:</td><td>" & FormatCurrency(ds.Tables("Batch").Rows(0).Item("OpenAmount"), 2) & "</td></tr>" ''format_Line("Opening Total", FormatCurrency(ds.Tables("Batch").Rows(0).Item("OpenAmount"), 2), lineLength) & vbCrLf '"Opening Total" & vbTab & vbTab & FormatCurrency(ds.Tables("Batch").Rows(0).Item("OpenAmount"), 2) & vbCrLf
        openingTotal = ds.Tables("Batch").Rows(0).Item("OpenAmount")
        cm.CommandText = "Select Case when Sum(SubTotal) is null then 0 else Sum(Subtotal) end as SalesTotal from t_Sales where (HasReturns is null or HasReturns = 0) and batchID = " & ddBatch.SelectedValue
        dr = cm.ExecuteReader
        dr.Read()
        rpt = rpt & "<tr><td>Sales:</td><td>" & FormatCurrency(dr("SalesTotal"), 2) & "</td></tr>" 'format_Line("Sales", FormatCurrency(dr("SalesTotal"), 2), lineLength) & vbCrLf '"Sales" & vbTab & vbTab & vbTab & FormatCurrency(dr("SalesTotal"), 2) & vbCrLf
        saleTotal = dr("SalesTotal")
        dr.Close()
        cm.CommandText = "Select Case when Sum(SubTotal) is null then 0 else Sum(Subtotal) end as SalesTotal from t_Sales where HasReturns = 1 and batchID = " & ddBatch.SelectedValue
        dr = cm.ExecuteReader
        dr.Read()
        rpt = rpt & "<tr><td>Returns:</td><td>" & FormatCurrency(dr("SalesTotal"), 2) & "</td></tr>" 'format_Line("Returns", FormatCurrency(dr("SalesTotal"), 2), lineLength) & vbCrLf '"Returns" & vbTab & vbTab & vbTab & FormatCurrency(dr("SalesTotal"), 2) & vbCrLf
        refTotal = dr("SalesTotal")
        dr.Close()
        cm.CommandText = "SELECT Case when (Tax - ReturnTax) is null then 0 else Tax - ReturnTax end AS TaxTotal FROM (SELECT (SELECT Case when SUM(GrandTotal) is null then 0 else SUM(GrandTotal) end - case when SUM(SubTotal) is null then 0 else SUM(SubTotal) end AS Expr1 FROM t_Sales WHERE (HasReturns IS NULL OR HasReturns = 0) AND (BatchID = " & ddBatch.SelectedValue & ")) AS Tax, (SELECT Case when SUM(GrandTotal) is null then 0 else Sum(GrandTotal) end - Case when SUM(SubTotal) is null then 0 else Sum(Subtotal) end AS Expr1 FROM t_Sales AS t_Sales_2 WHERE (HasReturns = 1) AND (BatchID = " & ddBatch.SelectedValue & ")) AS ReturnTax) AS xx"
        dr = cm.ExecuteReader
        dr.Read()
        rpt = rpt & "<tr><td>Tax Total:</td><td>" & FormatCurrency(dr("TaxTotal"), 2) & "</td></tr>" 'format_Line("Tax", FormatCurrency(dr("TaxTotal"), 2), lineLength) & vbCrLf '"Tax" & vbTab & vbTab & vbTab & FormatCurrency(dr("TaxTotal"), 2) & vbCrLf
        taxTotal = dr("TaxTotal")
        dr.Close()
        rpt = rpt & "<tr><td>Total:</td><td>" & FormatCurrency(openingTotal + saleTotal - refTotal + taxTotal, 2) & "</td></tr>" 'format_Line("Total", FormatCurrency(openingTotal + saleTotal - refTotal + taxTotal, 2), lineLength) & vbCrLf ' "Total" & vbTab & vbTab & vbTab & FormatCurrency(openingTotal + saleTotal - refTotal + taxTotal, 2) & vbCrLf
        rpt = rpt & vbCrLf

        cm.CommandText = "Select (CashAmt - CashRefAmt) as Total from  (Select (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'Cash' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 0) as CashAmt, (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'Cash' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 1) as CashRefAmt) xx"
        dr = cm.ExecuteReader
        dr.Read()
        cashTotal = CDbl(dr("Total"))
        rpt = rpt & "<tr><td>Cash:</td><td>" & FormatCurrency(dr("Total"), 2) & "</td></tr>" 'format_Line("Cash", FormatCurrency(dr("Total"), 2), lineLength) & vbCrLf '"Cash" & vbTab & vbTab & vbTab & FormatCurrency(dr("Total"), 2) & vbCrLf
        dr.Close()

        cm.CommandText = "Select (CashAmt - CashRefAmt) as Total from  (Select (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where Upper(p.PaymentMethod) = 'GIFTCARD' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 0) as CashAmt, (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where UPPER(p.PaymentMethod) = 'GIFTCARD' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 1) as CashRefAmt) xx"
        dr = cm.ExecuteReader
        dr.Read()
        cashTotal = CDbl(dr("Total"))
        rpt = rpt & "<tr><td>Gift Cards:</td><td>" & FormatCurrency(dr("Total"), 2) & "</td></tr>" 'format_Line("Cash", FormatCurrency(dr("Total"), 2), lineLength) & vbCrLf '"Cash" & vbTab & vbTab & vbTab & FormatCurrency(dr("Total"), 2) & vbCrLf
        dr.Close()

        cm.CommandText = "Select (CashAmt - CashRefAmt) as Total from  (Select (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'Check' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 0) as CashAmt, (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'Check' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 1) as CashRefAmt) xx"
        dr = cm.ExecuteReader
        dr.Read()
        rpt = rpt & "<tr><td>Check:</td><td>" & FormatCurrency(dr("Total"), 2) & "</td></tr>" 'format_Line("Check", FormatCurrency(dr("Total"), 2), lineLength) & vbCrLf '"Check" & vbTab & vbTab & vbTab & FormatCurrency(dr("Total"), 2) & vbCrLf
        dr.Close()

        cm.CommandText = "Select (CashAmt - CashRefAmt) as Total from  (Select (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'TravlersCheck' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 0) as CashAmt, (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'TravlersCheck' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 1) as CashRefAmt) xx"
        dr = cm.ExecuteReader
        dr.Read()
        rpt = rpt & "<tr><Td>Travelers Checks:</td><td>" & FormatCurrency(dr("Total"), 2) & "</td></tr>" 'format_Line("Travelers Checks", FormatCurrency(dr("Total"), 2), lineLength) & vbCrLf '"Travelers Checks" & vbTab & vbTab & FormatCurrency(dr("Total"), 2) & vbCrLf
        dr.Close()

        cm.CommandText = "Select (CashAmt - CashRefAmt) as Total from  (Select (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'American Express' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 0 and reference not like 'N%') as CashAmt, (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'American Express' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 1 and reference not like 'N%') as CashRefAmt) xx"
        dr = cm.ExecuteReader
        dr.Read()
        rpt = rpt & "<tr><td>American Express: </td><td>" & FormatCurrency(dr("Total"), 2) & "</td></tr>" 'format_Line("American Express", FormatCurrency(dr("Total"), 2), lineLength) & vbCrLf '"American Express" & vbTab & vbTab & FormatCurrency(dr("Total"), 2) & vbCrLf
        dr.Close()

        cm.CommandText = "Select (CashAmt - CashRefAmt) as Total from  (Select (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'Discover' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 0 and reference not like 'N%') as CashAmt, (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'Discover' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 1 and reference not like 'N%') as CashRefAmt) xx"
        dr = cm.ExecuteReader
        dr.Read()
        rpt = rpt & "<tr><td>Discover:</td><td>" & FormatCurrency(dr("Total"), 2) & "</td></tr>" 'format_Line("Discover", FormatCurrency(dr("Total"), 2), lineLength) & vbCrLf '"Discover" & vbTab & vbTab & vbTab & FormatCurrency(dr("Total"), 2) & vbCrLf
        dr.Close()

        cm.CommandText = "Select (CashAmt - CashRefAmt) as Total from  (Select (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'Mastercard' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 0 and reference not like 'N%') as CashAmt, (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'Mastercard' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 1 and reference not like 'N%') as CashRefAmt) xx"
        dr = cm.ExecuteReader
        dr.Read()
        rpt = rpt & "<tr><td>Mastercard:</td><td>" & FormatCurrency(dr("Total"), 2) & "</td></tr>" 'format_Line("Mastercard", FormatCurrency(dr("Total"), 2), lineLength) & vbCrLf '"Mastercard" & vbTab & vbTab & FormatCurrency(dr("Total"), 2) & vbCrLf
        dr.Close()

        cm.CommandText = "Select (CashAmt - CashRefAmt) as Total from  (Select (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'VISA' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 0 and reference not like 'N%') as CashAmt, (Select Case when Sum(Amount) is null then 0 else Sum(Amount) end from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'VISA' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 1 and reference not like 'N%') as CashRefAmt) xx"
        dr = cm.ExecuteReader
        dr.Read()
        rpt = rpt & "<tr><td>VISA:</td><td>" & FormatCurrency(dr("Total"), 2) & "</td></tr>" 'format_Line("VISA", FormatCurrency(dr("Total"), 2), lineLength) & vbCrLf '"VISA" & vbTab & vbTab & vbTab & FormatCurrency(dr("Total"), 2) & vbCrLf
        dr.Close()

        cm.CommandText = "Select Case when Sum(Amount) is null then 0 else Sum(Amount) end as Total from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'Voucher' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 0"
        dr = cm.ExecuteReader
        dr.Read()
        rpt = rpt & "<tr><td>Voucher:</td><td>" & FormatCurrency(dr("Total"), 2) & "</td></tr>" 'format_Line("VISA", FormatCurrency(dr("Total"), 2), lineLength) & vbCrLf '"VISA" & vbTab & vbTab & vbTab & FormatCurrency(dr("Total"), 2) & vbCrLf
        dr.Close()

        cm.CommandText = "Select Case when Sum(Amount) is null then 0 else Sum(Amount) end as Total from t_Payment p inner join t_Sales s on p.SalesID = s.SaleID where p.PaymentMethod = 'Adj' and s.BatchID = " & ddBatch.SelectedValue & "  and Refunded = 0"
        dr = cm.ExecuteReader
        dr.Read()
        rpt = rpt & "<tr><td>Adjustment:</td><td>" & FormatCurrency(dr("Total"), 2) & "</td></tr>" 'format_Line("VISA", FormatCurrency(dr("Total"), 2), lineLength) & vbCrLf '"VISA" & vbTab & vbTab & vbTab & FormatCurrency(dr("Total"), 2) & vbCrLf
        dr.Close()

        rpt = rpt & "<tr><td><br></td></tr>"

        rpt = rpt & "<tr><td>Closing Total:</td><td>" & FormatCurrency(ds.Tables("Batch").Rows(0).Item("CloseAmount"), 2) & "</td></tr>" 'format_Line("Closing Total", FormatCurrency(ds.Tables("Batch").Rows(0).Item("CloseAmount"), 2), lineLength) & vbCrLf '"Closing Total:" & vbTab & vbTab & FormatCurrency(ds.Tables("Batch").Rows(0).Item("CloseAmount"), 2) & vbCrLf
        rpt = rpt & "<tr><td>Closing Cash:</td><td>" & FormatCurrency(cashTotal, 2) & "</td></tr>" 'format_Line("Closing Cash", FormatCurrency(cashTotal, 2), lineLength) & vbCrLf '"Closing Cash:" & vbTab & vbTab & FormatCurrency(cashTotal, 2) & vbCrLf
        rpt = rpt & "<tr><td>Over/Under:</td><td>" & FormatCurrency(cashTotal - CDbl(ds.Tables("Batch").Rows(0).Item("CloseAmount")), 2) & "</td></tr>" 'format_Line("Over/Under", FormatCurrency(cashTotal - CDbl(ds.Tables("Batch").Rows(0).Item("CloseAmount")), 2), lineLength) & vbCrLf '"Over/Under:" & vbTab & vbTab & FormatCurrency(cashTotal - CDbl(ds.Tables("Batch").Rows(0).Item("CloseAmount")), 2) & vbCrLf
        rpt = rpt & "</table>"
        da.Dispose()
        cn.Close()
        lblReport.Text = rpt
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim cn As New SqlConnection(Resources.Resource.Boones)
            cn.Open()
            Dim cm As New SqlCommand("Select * from t_Batch", cn)
            Dim dr As SqlDataReader
            dr = cm.ExecuteReader
            Do While dr.Read()
                ddBatch.Items.Add(New ListItem(dr("BatchID") & " - " & dr("StartDate") & " - " & IIf(IsDBNull(dr("Closed")) Or Not (dr("Closed")), "Open", "Closed"), dr("BatchID")))
            Loop
            dr.Close()
            cn.Close()
        End If
    End Sub
End Class
