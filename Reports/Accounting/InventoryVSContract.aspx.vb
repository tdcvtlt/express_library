Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports ClosedXML.Excel

Partial Class Reports_Accounting_OpenWorkOrders
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim ds As New SqlDataSource(Resources.Resource.cns, Get_SQL(1))
        gbWorkOrders.DataSource = ds
        gbWorkOrders.DataBind()
        ds = Nothing
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gbWorkOrders.Rows.Count > 0 Then
            Generate_Report()
        End If
    End Sub

    Private Function Get_SQL(index As Integer) As String
        Return "select c.ContractNumber, st.ComboItem as SubType, se.ComboItem as Season, f.Frequency,si.week, si.UnitType, si.frequency as InventoryFrequency, cast(si.Size as varchar) + 'BD' as Size, si.Season as InvSeason " & _
            "from t_Contract c " & _
                "left outer join t_ComboItems s on s.ComboItemID = c.StatusID " & _
                "left outer join t_ComboItems st on st.ComboItemID = c.SaleSubTypeID " & _
                "left outer join t_ComboItems se on se.ComboItemID = c.SeasonID " & _
                "left outer join t_Frequency f on f.FrequencyID = c.FrequencyID " & _
                "inner join (select distinct min(si.Week) as Week, SUM(cast(LEFT(ust.comboitem,1) as Int)) as Size,f.Frequency, case when min(si.Week) < 9 then 'Yellow' else 'Red' end as Season, so.ContractID, ut.ComboItem as UnitType from t_SalesInventory si inner join t_Unit u on u.UnitID=si.UnitID left outer join t_ComboItems ust on ust.ComboItemID = u.SubTypeID inner join t_ComboItems ut on ut.ComboItemID = u.TypeID inner join t_SoldInventory so on so.SalesInventoryID=si.SalesInventoryID inner join t_Frequency f on f.frequencyid = so.frequencyid group by so.contractid,f.Frequency, ut.ComboItem) si on si.ContractID = c.ContractID " & _
            "where s.ComboItem in ('Active','OnHold') " & _
                "and ( " & _
                    "(st.ComboItem is null or st.ComboItem <> case when si.UnitType = 'Townes' or si.UnitType = 'Estates' then cast(si.Size as varchar) + ' - ' + si.UnitType else si.UnitType end) " & _
                    "or (si.Frequency is null or si.Frequency <> f.Frequency) " & _
                    "or (si.Season <> case when st.comboitem = 'cottage' and c.contractdate < '6/24/2000' then si.season else se.ComboItem end) " & _
                ") and st.comboitem <> 'Combo' " & _
            "order by c.ContractNumber"
    End Function

    Private Sub Loop_Records(ByRef dr As SqlDataReader, ByRef ws As IXLWorksheet)
        Dim row As Integer = 1
        While dr.Read
            If row = 1 Then
                For col = 1 To dr.VisibleFieldCount
                    ws.Cell(row, col).SetValue(dr.GetName(col - 1))
                Next
                row += 1
            End If
            For col = 1 To dr.VisibleFieldCount
                ws.Cell(row, col).SetValue(dr.Item(col - 1))
            Next
            row += 1
        End While
        dr.Close()
    End Sub


    Private Sub Generate_Report()
        Dim res As HttpResponse = Response
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand(Get_SQL(1), cn)
        Dim xlWB As New XLWorkbook
        cm.CommandTimeout = 10000
        cn.Open()

        cm.CommandText = Get_SQL(1)
        Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("Sheet1"))
        cn.Close()
        cn = Nothing
        cm = Nothing

        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        res.AddHeader("content-disposition", "attachment;filename=""InventoryVsContract.xlsx""")
        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()
    End Sub
End Class
