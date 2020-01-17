
Partial Class Reports_Accounting_TransactionSummary
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim acct As Integer = Me.ddAccounts.SelectedValue
        Dim tc As Integer = Me.ddTransCode.SelectedValue
        Dim sAns As String = ""
        Dim sTotal As Double = 0
        Dim sSQL As String = ""
        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0
        

        'Response.ContentType = "application/vnd.ms-excel"
        'Response.AddHeader("Content-Disposition", "attachment; filename=transactionsummary.xls")
        sAns = "<table>"
        sTotal = 0
        If acct = 11 Or acct = 12 Then
            Dim salesTax As Double = 0
            Dim foodTax As Double = 0
            Dim alcTax As Double = 0
            Dim salestaxTotal As Double = 0
            Dim foodTaxTotal As Double = 0
            Dim alcTaxtotal As Double = 0
            Dim salesID As Double = 0
            Dim salesDate As String = ""
            Dim category As String = ""
            If acct = 11 Then
                sSQL = "select * from ( " & _
                            "select Case when t.Description like '%Food%' then 'Food' when t.Description like '%Alcohol%' then 'Alcohol' when t.Description like '%Sales%' or t.Description is null then 'Retail' else t.Description end as Category, s.SalesID, s.SalesDate, i.Description, case when i.cost is null then 0 else i.Cost end as Cost,count(si.sales2itemid) as Qty, sum(Case when s.HasReturns = 1 then si.Amount * -1 else si.Amount end) as Amount " & _
                            "from [GIFTSHOP].[dbo].[t_Sales] s " & _
                            "   inner join [GIFTSHOP].[dbo].[t_Sales2Item] si on si.salesid = s.salesid " & _
                            "    inner join [GIFTSHOP].[dbo].[t_Item] i on i.itemid = si.itemid " & _
                            "    left outer join [GIFTSHOP].[dbo].[t_Tax] t on t.taxid = i.taxid " & _
                            "where s.salesdate between '" & sdate & "' and '" & edate & "' " & _
                            "group by s.salesid, s.salesdate, i.description, s.HasReturns, t.Description,i.Cost " & _
                            "union " & _
                            "select 'Tax' as Category, s.SalesID, s.SalesDate, t.Description,0 as Cost,1 as Qty,  Case when s.HasReturns = 1 then round(sum(taxrate * amount),2) * -1 else round(sum(taxrate * amount),2) end as Amount " & _
                            "from [GIFTSHOP].[dbo].[t_Sales2Item] si " & _
                            "    inner join [GIFTSHOP].[dbo].[t_Item] i on i.itemid = si.itemid " & _
                            "    inner join [GIFTSHOP].[dbo].[t_Tax] t on t.taxid = i.taxid " & _
                            "    inner join [GIFTSHOP].[dbo].[t_Sales] s on s.salesid = si.salesid " & _
                            "where s.salesdate between '" & sdate & "' and '" & edate & "' " & _
                            "group by s.salesid, t.description, s.salesdate, s.HasReturns " & _
                        ") a " & _
                        "order by Category, salesid"
            Else
                If edate <> "" Then
                    edate = CDate(edate).AddDays(1).ToString
                End If
                sSQL = "select * from ( " & _
                            "select Case when t.TaxName like '%Food%' then 'Food' when t.TaxName like '%Alcohol%' then 'Alcohol' when t.TaxName like '%Sales%' or t.TaxName is null then 'Retail' else t.TaxName end as Category, s.saleid as SalesID, s.saledate as SalesDate, i.Name As Description, " & _
                            "    case when (select sum(case when ordercost is null then 0 else ordercost end) from [Register].[dbo].t_Items where itemid in (select distinct itemid from [Register].[dbo].t_Menuitem2item where menuitemid = i.menuitemid)) is null then 0 else (select sum(case when ordercost is null then 0 else ordercost end) from [Register].[dbo].t_Items where itemid in (select distinct itemid from [Register].[dbo].t_Menuitem2item where menuitemid = i.menuitemid)) end as Cost,sum(si.Qty) as Qty, sum(Case when s.HasReturns = 1 then si.Amount * si.Qty * -1 else si.Amount * si.Qty end) as Amount " & _
                            "from [Register].[dbo].[t_Sales] s " & _
                            "    inner join [Register].[dbo].[t_SalesItem] si on si.salesid = s.saleid " & _
                            "    inner join [Register].[dbo].[t_MenuItems] i on i.Menuitemid = si.menuitemid " & _
                            "    left outer join [Register].[dbo].[t_TaxRate] t on t.taxid = i.taxid " & _
                            "where s.saledate between '" & sdate & "' and '" & edate & "' " & _
                            "group by s.saleid, s.saledate, i.Name, s.HasReturns, t.TaxName,i.menuitemid " & _
                            "union " & _
                            "select 'Tax' as Category, s.saleid as SalesID, s.saledate as SalesDate, t.TaxName,0 as Cost, 1 as Qty, sum((t.Rate/100) * s.Subtotal) as Amount " & _
                            "from [Register].[dbo].[t_SalesItem] si " & _
                            "    inner join [Register].[dbo].[t_MenuItems] i on i.Menuitemid = si.Menuitemid " & _
                            "    inner join [Register].[dbo].[t_TaxRate] t on t.taxid = i.taxid " & _
                            "    inner join [Register].[dbo].[t_Sales] s on s.saleid = si.salesid " & _
                            "where s.saledate between '" & sdate & "' and '" & edate & "' " & _
                            "group by s.saleid, t.TaxName, s.saledate, s.HasReturns " & _
                        ") a " & _
                        "order by Category, salesid"
            End If
            rs.open(sSQL, cn, 0, 1)
            If rs.EOF And rs.BOF Then
                sAns = sAns & "<tr><td>No Records</td></tr>"
            Else
                sAns = sAns & "<tr>"
                For i = 0 To rs.fields.count - 1
                    sAns = sAns & "<th>" & rs.fields(i).name & "</th>"
                Next
                Do While Not rs.EOF
                    If salesID = 0 Then
                        salesID = rs.Fields("SalesID").Value
                        salesDate = rs.Fields("SalesDate").value
                        category = rs.Fields("Category").value & ""
                    End If

                    'If salesID <> rs.Fields("SalesID").value Then
                    '    If salesTax > 0 Then
                    '        sAns = sAns & "<tr><td>" & category & "</td>"
                    '        sAns = sAns & "<td>" & salesID & "</td>"
                    '        sAns = sAns & "<td>" & salesDate & "</td>"
                    '        sAns = sAns & "<td>VA Sales Tax</td>"
                    '        sAns = sAns & "<td></td><td align = 'right'>" & FormatCurrency(salesTax, 2) & "</td></tr>"
                    '    End If
                    '    If foodTax > 0 Then
                    '        sAns = sAns & "<tr><td>" & category & "</td>"
                    '        sAns = sAns & "<td>" & salesID & "</td>"
                    '        sAns = sAns & "<td>" & salesDate & "</td>"
                    '        sAns = sAns & "<td>VA Food Tax</td>"
                    '        sAns = sAns & "<td></td><td align = 'right'>" & FormatCurrency(foodTax, 2) & "</td></tr>"
                    '    End If
                    '    If alcTax > 0 Then
                    '        sAns = sAns & "<tr><td>" & category & "</td>"
                    '        sAns = sAns & "<td>" & salesID & "</td>"
                    '        sAns = sAns & "<td>" & salesDate & "</td>"
                    '        sAns = sAns & "<td>VA Alcohol Tax</td>"
                    '        sAns = sAns & "<td></td><td align = 'right'>" & FormatCurrency(alcTax, 2) & "</td></tr>"
                    '    End If
                    '    salesTax = 0
                    '    foodTax = 0
                    '    alcTax = 0
                    '    salesID = rs.Fields("SalesID").value
                    '    salesDate = rs.Fields("SalesDate").value
                    '    category = rs.Fields("Category").value
                    'End If

                    'If InStr(rs.Fields("Description").value, "Tax") <> 0 Then
                    If InStr(rs.Fields("Description").value, "VA Food Tax") <> 0 Then
                        foodTax += rs.Fields("Amount").value
                        foodTaxTotal += rs.fields("Amount").Value
                    End If
                    If InStr(rs.Fields("Description").value, "VA Sales Tax") <> 0 Then
                        salesTax += rs.Fields("Amount").value
                        salestaxTotal += rs.Fields("Amount").Value
                    End If
                    If InStr(rs.Fields("Description").value, "VA Alcohol Tax") <> 0 Then
                        alcTax += rs.Fields("Amount").value
                        alcTaxtotal += rs.Fields("Amount").Value
                    End If
                    'Else
                    sAns = sAns & "<tr><td>" & rs.Fields("Category").value & "</td>"
                    sAns = sAns & "<td>" & rs.Fields("SalesID").value & "</td>"
                    sAns = sAns & "<td>" & rs.Fields("salesdate").value & "</td>"
                    sAns = sAns & "<td>" & rs.Fields("Description").value & "</td>"
                    sAns = sAns & "<td align = 'right'>" & FormatCurrency(rs.Fields("Cost").value, 2) & "</td>"
                    sAns = sAns & "<td align = 'right'>" & rs.Fields("Qty").value & "</td><td align = 'right'>" & FormatCurrency(rs.Fields("Amount").Value, 2) & "</td></tr>"
                    sTotal += rs.Fields("Amount").value
                    'End If
                    rs.movenext()
                Loop
                'If salesTax > 0 Then
                '    sAns = sAns & "<tr><td>" & salesID & "</td>"
                '    sAns = sAns & "<td>" & salesDate & "</td>"
                '    sAns = sAns & "<td>VA Sales Tax</td>"
                '    sAns = sAns & "<td></td><td align = 'right'>" & FormatCurrency(salesTax, 2) & "</td></tr>"
                'End If
                'If foodTax > 0 Then
                '    sAns = sAns & "<tr><td>" & salesID & "</td>"
                '    sAns = sAns & "<td>" & salesDate & "</td>"
                '    sAns = sAns & "<td>VA Food Tax</td>"
                '    sAns = sAns & "<td></td><td align = 'right'>" & FormatCurrency(foodTax, 2) & "</td></tr>"
                'End If
                'If alcTax > 0 Then
                '    sAns = sAns & "<tr><td>" & category & "</td>"
                '    sAns = sAns & "<td>" & salesID & "</td>"
                '    sAns = sAns & "<td>" & salesDate & "</td>"
                '    sAns = sAns & "<td>VA Alcohol Tax</td>"
                '    sAns = sAns & "<td></td><td align = 'right'>" & FormatCurrency(alcTax, 2) & "</td></tr>"
                'End If
                sAns = sAns & "<tr><td colspan = '5' align='right'>Alcohol Tax Total:</td><td align = 'right'>" & FormatCurrency(alcTaxtotal) & "</td><td>&nbsp;</td><td>&nbsp;</td></tr>"
                sAns = sAns & "<tr><td colspan = '5' align='right'>Sales Tax Total:</td><td align = 'right'>" & FormatCurrency(salestaxTotal) & "</td><td>&nbsp;</td><td>&nbsp;</td></tr>"
                sAns = sAns & "<tr><td colspan = '5' align='right'>Food Tax Total:</td><td align = 'right'>" & FormatCurrency(foodTaxTotal) & "</td><td>&nbsp;</td><td>&nbsp;</td></tr>"
                sAns = sAns & "<tr><td colspan = '5' align='right'>Sales Total:</td><td align = 'right'>" & FormatCurrency(sTotal) & "</td><td>&nbsp;</td><td>&nbsp;</td></tr>"
                sAns = sAns & "<tr><td colspan = '5' align='right'>Grand Total:</td><td align = 'right'>" & FormatCurrency(sTotal + foodTaxTotal + salestaxTotal + alcTaxtotal) & "</td><td>&nbsp;</td><td>&nbsp;</td></tr>"

            End If

        Else
            If acct = 0 Then
                If tc = 0 Then
                    sSQL = "select i.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.KeyValue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown ' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, i.Amount, per.username as Personnel, case when UPPER(i.KeyField) = 'PACKAGEISSUEDID' then (select stat.comboitem from t_Comboitems stat inner join t_Packageissued mort on mort.statusid = stat.comboitemid where mort.packageissuedid=i.Keyvalue) when UPPER(i.KeyField) = 'TOURID' then (select stat.comboitem from t_Comboitems stat inner join t_Tour mort on mort.statusid = stat.comboitemid where mort.tourid=i.keyvalue) when UPPER(i.KeyField) = 'RESERVATIONID' then (select stat.comboitem from t_Comboitems stat inner join t_Reservations mort on mort.statusid = stat.comboitemid where mort.reservationid=i.KeyValue) when UPPER(i.KeyField) = 'CONTRACTID' then (select stat.comboitem from t_Comboitems stat inner join t_Contract mort on mort.statusid = stat.comboitemid where mort.contractid=i.keyvalue) when UPPER(i.KeyField) = 'MORTGAGEID' then (select stat.comboitem from t_Comboitems stat inner join t_Mortgage mort on mort.statusid = stat.comboitemid where mort.mortgageid=i.KeyValue) else '' end as Status from t_Invoices i inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = i.userid where i.transdate between '" & sdate & "' and '" & edate & "' and  i.ApplytoID =0  " & _
                        "union " & _
                            "select ia.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.KeyValue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown ' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, case when ia.posneg = 0 then ia.Amount else ia.Amount * -1 end as Amount, per.username as Personnel, case when UPPER(i.KeyField) = 'PACKAGEISSUEDID' then (select stat.comboitem from t_Comboitems stat inner join t_Packageissued mort on mort.statusid = stat.comboitemid where mort.packageissuedid=i.Keyvalue) when UPPER(i.KeyField) = 'TOURID' then (select stat.comboitem from t_Comboitems stat inner join t_Tour mort on mort.statusid = stat.comboitemid where mort.tourid=i.keyvalue) when UPPER(i.KeyField) = 'RESERVATIONID' then (select stat.comboitem from t_Comboitems stat inner join t_Reservations mort on mort.statusid = stat.comboitemid where mort.reservationid=i.KeyValue) when UPPER(i.KeyField) = 'CONTRACTID' then (select stat.comboitem from t_Comboitems stat inner join t_Contract mort on mort.statusid = stat.comboitemid where mort.contractid=i.keyvalue) when UPPER(i.KeyField) = 'MORTGAGEID' then (select stat.comboitem from t_Comboitems stat inner join t_Mortgage mort on mort.statusid = stat.comboitemid where mort.mortgageid=i.KeyValue) else '' end as Status from t_Invoices i inner join t_Invoices ia on ia.applytoid = i.invoiceid inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = ia.userid where ia.transdate between '" & sdate & "' and '" & edate & "' and  i.ApplytoID =0 "
                    rs.open(sSQL, cn, 0, 1)
                    'rs.open("select i.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.KeyValue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown ' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, i.Amount, per.username as Personnel, case when UPPER(i.KeyField) = 'PACKAGEISSUEDID' then (select stat.comboitem from t_Comboitems stat inner join t_Packageissued mort on mort.statusid = stat.comboitemid where mort.packageissuedid=i.Keyvalue) when UPPER(i.KeyField) = 'TOURID' then (select stat.comboitem from t_Comboitems stat inner join t_Tour mort on mort.statusid = stat.comboitemid where mort.tourid=i.keyvalue) when UPPER(i.KeyField) = 'RESERVATIONID' then (select stat.comboitem from t_Comboitems stat inner join t_Reservations mort on mort.statusid = stat.comboitemid where mort.reservationid=i.KeyValue) when UPPER(i.KeyField) = 'CONTRACTID' then (select stat.comboitem from t_Comboitems stat inner join t_Contract mort on mort.statusid = stat.comboitemid where mort.contractid=i.keyvalue) when UPPER(i.KeyField) = 'MORTGAGEID' then (select stat.comboitem from t_Comboitems stat inner join t_Mortgage mort on mort.statusid = stat.comboitemid where mort.mortgageid=i.KeyValue) else '' end as Status from t_Invoices i inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = i.userid where i.transdate between '" & sdate & "' and '" & edate & "' and  i.ApplytoID >-1 order by i.transdate", cn, 0, 1)
                Else
                    sSQL = "select i.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.KeyValue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, i.Amount, per.username as Personnel from t_invoices i inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = i.UserID left outer join t_Contract c on c.contractid = i.keyvalue where i.transdate between '" & sdate & "' and '" & edate & "' and  i.ApplyToID =0 and i.fintransid = '" & tc & "' " & _
                        "union " & _
                        "select ia.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.KeyValue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, case when ia.posneg = 0 then ia.Amount else ia.Amount * -1 end as Amount, per.username as Personnel from t_invoices i inner join t_Invoices ia on ia.applytoid = i.invoiceid inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = ia.UserID left outer join t_Contract c on c.contractid = i.keyvalue where ia.transdate between '" & sdate & "' and '" & edate & "' and  i.ApplyToID <>0 and i.fintransid = '" & tc & "' "
                    rs.open(sSQL, cn, 0, 1)
                    'rs.open("select i.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.KeyValue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, i.Amount, per.username as Personnel from t_invoices i inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = i.UserID left outer join t_Contract c on c.contractid = i.contractid where i.transdate between '" & sdate & "' and '" & edate & "' and  i.ApplyToID >-1 and i.fintransid = '" & tc & "' order by i.transdate", cn, 0, 1)
                End If
            Else
                If tc = 0 Then
                    sSQL = "select i.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.Keyvalue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, i.Amount, per.username as Personnel from t_Invoices i inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = i.UserID where i.transdate between '" & sdate & "' and '" & edate & "' and f.MerchantAccountID = '" & acct & "' and i.ApplyToID =0 " & _
                        "union " & _
                            "select ia.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.Keyvalue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, case when ia.posneg = 0 then ia.Amount else ia.amount * -1 end as Amount, per.username as Personnel from t_Invoices i inner join t_Invoices ia on ia.applytoid = i.invoiceid inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = ia.UserID where ia.transdate between '" & sdate & "' and '" & edate & "' and f.MerchantAccountID = '" & acct & "' and i.ApplyToID =0 "
                    rs.open(sSQL, cn, 0, 1)
                    'rs.open("select i.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.Keyvalue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, i.Amount, per.username as Personnel from t_Invoices i inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = i.UserID where i.transdate between '" & sdate & "' and '" & edate & "' and f.MerchantAccountID = '" & acct & "' and i.ApplyToID >-10 order by i.transdate", cn, 0, 1)
                Else
                    sSQL = "select i.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.Keyvalue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, i.Amount, per.username as Personnel from t_Invoices i inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = i.UserID where i.transdate between '" & sdate & "' and '" & edate & "' and f.MerchantAccountID = '" & acct & "' and i.ApplyToID =0 and i.fintransid = '" & tc & "' " & _
                        "union " & _
                            "select ia.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.Keyvalue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, case when ia.posneg = 0 then ia.Amount else ia.Amount * -1 end as Amount, per.username as Personnel from t_Invoices i inner join t_Invoices ia on ia.applytoid = i.invoiceid inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = ia.UserID where ia.transdate between '" & sdate & "' and '" & edate & "' and f.MerchantAccountID = '" & acct & "' and i.ApplyToID =0 and i.fintransid = '" & tc & "' "
                    rs.open(sSQL, cn, 0, 1)
                    'rs.open("select i.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.Keyvalue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, i.Amount, per.username as Personnel from t_Invoices i inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = i.UserID where i.transdate between '" & sdate & "' and '" & edate & "' and f.MerchantAccountID = '" & acct & "' and i.ApplyToID > -10 and i.fintransid = '" & tc & "' order by i.transdate", cn, 0, 1)
                End If
            End If
            If rs.EOF And rs.BOF Then
                sAns = sAns & "<tr><td>No Records</td></tr>"
            Else
                sAns = sAns & "<tr>"
                For i = 0 To rs.fields.count - 1
                    sAns = sAns & "<th>" & rs.fields(i).name & "</th>"
                Next
                sAns = sAns & "<td>Phase</td>"
                sAns = sAns & "</tr>"
                Dim sPhase As String = "&nbsp;"
                Do While Not rs.EOF
                    sAns = sAns & "<tr>"
                    For i = 0 To rs.fields.count - 1
                        If rs.fields(i).name = "Amount" Then
                            sAns = sAns & "<td align='right'>" & FormatCurrency(rs.fields(i).value) & "</td>"
                            sTotal = sTotal + rs.fields(i).value
                        Else
                            sAns = sAns & "<td>" & rs.fields(i).value & "</td>"
                        End If
                    Next
                    If Left(rs.fields("Field").value, 3) = "KCP" Then
                        rs2.open("Select case when charindex(' ',const.comboitem) <2 then const.comboitem else left(const.comboitem, charindex(' ', const.comboitem)) end as Phase from t_Contract c left outer join t_Comboitems const on const.comboitemid = c.saletypeid where c.contractnumber = '" & Right(rs.fields("Field").value, Len(rs.fields("Field").value) - InStr(rs.fields("Field").value, ":")) & "'", cn, 0, 1)
                        If rs2.eof And rs2.bof Then
                            sPhase = "&nbsp;"
                        Else
                            sPhase = rs2.fieldS("Phase").value & ""
                        End If
                        rs2.close()
                    End If

                    sAns = sAns & "<td>" & sPhase & "</td>"
                    sAns = sAns & "</tr>"
                    rs.movenext()
                Loop
                sAns = sAns & "<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td align='right'>Total:</td><td>" & FormatCurrency(sTotal) & "</td><td>&nbsp;</td><td>&nbsp;</td></tr>"
            End If
        End If
            sAns = sAns & "</table>"
            rs.Close()
            cn.Close()
            rs = Nothing
            cn = Nothing
            litReport.Text = sAns
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oAccounts As New clsCCMerchantAccount
            Dim oFinTrans As New clsFinancialTransactionCodes
            ddAccounts.Items.Add(New ListItem("ALL", 0))
            ddAccounts.DataSource = oAccounts.List_Accounts()
            ddAccounts.DataTextField = "Description"
            ddAccounts.DataValueField = "AccountID"
            ddAccounts.AppendDataBoundItems = True
            ddAccounts.DataBind()
            ddTransCode.Items.Add(New ListItem("ALL", 0))
            ddTransCode.DataSource = oFinTrans.List_Trans_Codes("")
            ddTransCode.DataTextField = "TransCode"
            ddTransCode.DataValueField = "FinTransID"
            ddTransCode.AppendDataBoundItems = True
            ddTransCode.DataBind()
            oAccounts = Nothing
            oFinTrans = Nothing
        End If
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=Transaction Summary.xls")
        Response.Write(litReport.Text)
        Response.End()
    End Sub
End Class
