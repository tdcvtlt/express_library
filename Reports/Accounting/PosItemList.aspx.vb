Imports System.Data.SqlClient
Partial Class Reports_Accounting_PosItemList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then

        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim rpt As String = ""
        Dim category As String = ""
        Dim itemQtyTotal As Integer = 0
        Dim itemCostTotal As Double = 0
        Dim itemPriceTotal As Double = 0
        rpt = "<table>"
        If RadioButtonList1.SelectedValue = "Boones" Then
            rpt = rpt & "<tr><th>Item</th><th>Item Lookup</th><th>Item Description</th><th>Quantity</th><th>Cost</th><th>Price</th><th>Extended Cost</th><th>Extended Price</th></tr>"
        Else
            rpt = rpt & "<tr><th>Category</th><th>Item Lookup</th><th>Item Description</th><th>Quantity</th><th>Cost</th><th>Price</th><th>Extended Cost</th><th>Extended Price</th></tr>"
        End If

        Dim cn As New SqlConnection(Resources.Resource.GIFTSHOP)

        Dim cm As New SqlCommand("  Select c.Name, i.ItemLookupcode, i.Description, i.Quantity, i.Cost, i.Price, (i.Cost * i.Quantity) as ExtendedCost, (i.Price * i.Quantity) as ExtendedPrice from t_Item i inner join t_Category c on i.CategoryID = c.CategoryID order by c.name, description", cn)
        If RadioButtonList1.SelectedValue = "Boones" Then
            cn.ConnectionString = Resources.Resource.Boones
            cm.CommandText = " select *,'' as ItemLookupCode, quantity * cost as ExtendedCost, Quantity * Price as ExtendedPrice " & _
                                "from ( " & _
                                "select Name, Description, case when QtyOnHand is null then 0 else QtyOnHand end as Quantity, case when cost is null then 0 else cost end as Cost, case when Price is null then 0 else Price end as Price " & _
                                "from ( " & _
                                "    select coalesce(mi.Name,'') as Name, coalesce(mi.description,'') as Description,  " & _
                                "       (select min(case when i.qtyonhand is null then 0 else i.qtyonhand end) as Qty from t_Items i inner join t_Menuitem2item m2i on m2i.itemid = i.itemid where m2i.menuitemid = mi.menuitemid) as QtyOnHand, " & _
                                "       (select sum(case when i.Ordercost is null then 0 else i.Ordercost end) as Qty from t_Items i inner join t_Menuitem2item m2i on m2i.itemid = i.itemid where m2i.menuitemid = mi.menuitemid) as Cost, " & _
                                "       coalesce(mi.cost,0) as Price " & _
                                "    from t_Menuitems mi " & _
                                "   ) a " & _
                                ") b"
        End If
        cn.Open()
        Dim dr As SqlDataReader
        dr = cm.ExecuteReader
        Do While dr.Read()
            If category = "" Then
                category = dr("Name")
            End If
            If category <> dr("Name") And RadioButtonList1.SelectedValue <> "Boones" Then
                rpt = rpt & "<tr>"
                rpt = rpt & "<td><B>" & category & "</B></td>"
                rpt = rpt & "<td></td><td></td>"
                rpt = rpt & "<td><B>" & itemQtyTotal & "</B></td>"
                rpt = rpt & "<td></td><td></td>"
                rpt = rpt & "<td><B>" & FormatCurrency(itemCostTotal, 2) & "</B></td>"
                rpt = rpt & "<td><B>" & FormatCurrency(itemPriceTotal, 2) & "</B></td>"
                rpt = rpt & "</tr>"
                itemQtyTotal = 0
                itemCostTotal = 0
                itemPriceTotal = 0
                category = dr("Name")
            End If
            rpt = rpt & "<tr>"
            rpt = rpt & "<td>" & dr("Name") & "</td>"
            rpt = rpt & "<td>" & dr("ItemLookupCode") & "</td>"
            rpt = rpt & "<td>" & dr("Description") & "</td>"
            rpt = rpt & "<td>" & dr("Quantity") & "</td>"
            rpt = rpt & "<td>" & FormatCurrency(dr("Cost"), 2) & "</td>"
            rpt = rpt & "<td>" & FormatCurrency(dr("Price"), 2) & "</td>"
            rpt = rpt & "<td>" & FormatCurrency(dr("ExtendedCost"), 2) & "</td>"
            rpt = rpt & "<td>" & FormatCurrency(dr("ExtendedPrice"), 2) & "</td>"
            rpt = rpt & "</tr>"
            itemQtyTotal += dr("Quantity")
            itemCostTotal += dr("ExtendedCost")
            itemPriceTotal += dr("ExtendedPrice")
        Loop
        rpt = rpt & "<tr>"
        rpt = rpt & "<td><B>" & IIf(RadioButtonList1.SelectedValue <> "Boones", category, "Total") & "</B></td>"
        rpt = rpt & "<td></td><td></td>"
        rpt = rpt & "<td><B>" & itemQtyTotal & "</B></td>"
        rpt = rpt & "<td></td><td></td>"
        rpt = rpt & "<td><B>" & FormatCurrency(itemCostTotal, 2) & "</B></td>"
        rpt = rpt & "<td><B>" & FormatCurrency(itemPriceTotal, 2) & "</B></td>"
        rpt = rpt & "</tr>"
        rpt = rpt & "</table>"
        dr.Close()
        cn.Close()
        litReport.Text = rpt
    End Sub
End Class
