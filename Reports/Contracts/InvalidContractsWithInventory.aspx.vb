Imports Microsoft
Imports Microsoft.VisualBasic

Partial Class Reports_Contracts_InvalidContractsWithInventory
    Inherits System.Web.UI.Page

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Get_Report()
            
        End If

    End Sub

    Private Sub Get_Report()
        Dim Status As String = ""
        Dim cn As Object
        Dim rs As Object
        Dim sAns As String
        Dim sql As String = ""

        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        Status = "'InColl-Active','Active','Suspense','ReDeed', 'In Bankruptcy', 'In Foreclosure','Developer','Reverter','Pender','InColl-Developer', 'Deed-In-Lieu','MF Foreclosure','On Hold'"
        Dim stmp As Array
        stmp = Split(Status, ",")
        sAns = ""
        litReport.Text = ""
        litReport.Text &= "Excludes the following:<br> "
        For i = 0 To UBound(stmp)
            litReport.Text &= stmp(i)
            litReport.Text &= ", "
        Next
        litReport.Text &= "<br><hr>"

        sql = "Select count(s.soldinventoryid) as Number, p.LastName, c.contractid, contractnumber, stat.comboitem, css.ComboItem as SubStatus, c.statusdate,sum(cast(left(ust.comboitem,1) as int)) as Rooms, f.frequency, seas.comboitem as season, st.comboitem as saletype  from t_Contract c inner join t_Prospect p on c.prospectID = p.ProspectID inner join t_Soldinventory s on s.contractid  = c.contractid inner join t_Frequency f on f.frequencyid = s.frequencyid inner join t_Salesinventory si on si.salesinventoryid = s.salesinventoryid inner join t_Unit u on u.unitid = si.unitid inner join (select i.* from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname='UnitSubType') ust on ust.comboitemid = u.subtypeid left outer join t_Comboitems stat on stat.comboitemid = c.statusid left outer join t_CombOitems css on c.SubStatusID  = css.CombOitemID left outer join t_Comboitems seas on seas.comboitemid = c.seasonid left outer join t_ComboItems st on st.comboitemid = c.saletypeid left outer join t_Mortgage m on m.contractid = c.contractid where (stat.comboitem not in (" & Status & ") or ( m.contractid is null)) group by stat.comboitem, contractnumber, p.LastName, c.contractid, f.frequency, seas.comboitem, st.comboitem,c.statusdate,css.comboitem"
        rs.open(sql, cn, 0, 1)
        litReport.Text &= "<table><tr><th><u>KCP Number</u></th><th><u>Last Name</u></th><th><u>Status</u></th><th><u>SubStatus</u></th><th><u>Status Date</u></th><th><u>Pieces of Inventory</u></th><th><u>Frequency</u></th><th><u>Season</u></th><th><u>Rooms</u></th><th><u>Phase</u></th><th><u>Intervals</u></th></tr>"
        Do While Not rs.eof
            litReport.Text &= "<tr><td>"
            litReport.Text &= "<a target='_blank' href = '../../marketing/editcontract.aspx?contractid=" & rs.fields("ContractID").value & "'>" & rs.fields("ContractNumber").value & "</a>"
            'litReport.Text &= rs.fields("ContractNumber").value
            litReport.Text &= "</td><td>"
            litReport.Text &= rs.Fields("Lastname").value
            litReport.Text &= "</td><td>"
            litReport.Text &= rs.fields("ComboItem").value
            litReport.Text &= "</td><td>"
            litReport.Text &= rs.Fields("SubStatus").value
            litReport.Text &= "</td><td>"
            litReport.Text &= rs.fields("StatusDate").value
            litReport.Text &= "</td><td align='center'>"
            litReport.Text &= rs.fields("Number").value
            litReport.Text &= "</td><td>"
            litReport.Text &= rs.fields("Frequency").value
            litReport.Text &= "</td><td>"
            litReport.Text &= rs.fields("Season").value
            litReport.Text &= "</td><td align='center'>"
            litReport.Text &= rs.fields("Rooms").value
            litReport.Text &= "</td><td>"

            If Left(rs.Fields("SaleType").value.ToString, 4) = "Cott" Then
                litReport.Text &= "Cottage"
            ElseIf Left(rs.FIelds("SaleType").value.ToString, 4) = "Town" Then
                litReport.Text &= "Townes"
            ElseIf Left(rs.Fields("SaleType").value.ToString, 4) = "Esta" Then
                litReport.Text &= "Estates"
            Else
                litReport.Text &= "N/A"
            End If

            litReport.Text &= "</td><td align = 'center'>"

            If rs.Fields("Rooms").value.ToString = 3 And Left(rs.Fields("SaleType").value.ToString, 4) = "Cott" And rs.Fields("Frequency").value.ToString = "Annual" Then
                litReport.Text &= "1"
            ElseIf rs.Fields("Rooms").value.ToString = 3 And Left(rs.Fields("SaleType").value.ToString, 4) = "Cott" And rs.Fields("Frequency").value.ToString = "Biennial" Then
                litReport.Text &= ".5"
            ElseIf rs.Fields("Rooms").value.ToString = 2 And Left(rs.Fields("SaleType").value.ToString, 4) = "Town" And rs.Fields("Frequency").value.ToString = "Annual" Then
                litReport.Text &= "1"
            ElseIf rs.Fields("Rooms").value.ToString = 2 And Left(rs.Fields("SaleType").value.ToString, 4) = "Town" And rs.Fields("Frequency").value.ToString = "Biennial" Then
                litReport.Text &= ".5"
            ElseIf rs.Fields("Rooms").value.ToString = 4 And Left(rs.Fields("SaleType").value.ToString, 4) = "Town" And rs.Fields("Frequency").value.ToString = "Annual" Then
                litReport.Text &= "2"
            ElseIf rs.Fields("Rooms").value.ToString = 4 And Left(rs.Fields("SaleType").value.ToString, 4) = "Town" And rs.Fields("Frequency").value.ToString = "Biennial" Then
                litReport.Text &= "1"
            ElseIf rs.Fields("Rooms").value.ToString = 1 And Left(rs.Fields("SaleType").value.ToString, 4) = "Esta" And rs.Fields("Frequency").value.ToString = "Annual" Then
                litReport.Text &= ".25"
            ElseIf rs.Fields("Rooms").value.ToString = 1 And Left(rs.Fields("SaleType").value.ToString, 4) = "Esta" And rs.Fields("Frequency").value.ToString = "Biennial" Then
                litReport.Text &= ".125"
            ElseIf rs.Fields("Rooms").value.ToString = 2 And Left(rs.Fields("SaleType").value.ToString, 4) = "Esta" And rs.Fields("Frequency").value.ToString = "Annual" Then
                litReport.Text &= ".5"
            ElseIf rs.Fields("Rooms").value.ToString = 2 And Left(rs.Fields("SaleType").value.ToString, 4) = "Esta" And rs.Fields("Frequency").value.ToString = "Biennial" Then
                litReport.Text &= ".25"
            ElseIf rs.Fields("Rooms").value.ToString = 3 And Left(rs.Fields("SaleType").value.ToString, 4) = "Esta" And rs.Fields("Frequency").value.ToString = "Annual" Then
                litReport.Text &= ".75"
            ElseIf rs.Fields("Rooms").value.ToString = 3 And Left(rs.Fields("SaleType").value.ToString, 4) = "Esta" And rs.Fields("Frequency").value.ToString = "Biennial" Then
                litReport.Text &= ".375"
            ElseIf rs.Fields("Rooms").value.ToString = 4 And Left(rs.Fields("SaleType").value.ToString, 4) = "Esta" And rs.Fields("Frequency").value.ToString = "Annual" Then
                litReport.Text &= "1"
            ElseIf rs.Fields("Rooms").value.ToString = 4 And Left(rs.Fields("SaleType").value.ToString, 4) = "Esta" And rs.Fields("Frequency").value.ToString = "Biennial" Then
                litReport.Text &= ".5"
            ElseIf rs.Fields("Rooms").value.ToString = 4 And Left(rs.Fields("SaleType").value.ToString, 4) = "Esta" And rs.Fields("Frequency").value.ToString = "Triennial" Then
                litReport.Text &= ".333"
            Else
                litReport.Text &= "0"
            End If
            litReport.Text &= "</td></tr>"

            rs.movenext()
        Loop
        litReport.Text &= "</table>"
        rs.close()
        cn.close()
        rs = Nothing
        cn = Nothing

        'Me.litReport.Text = sAns
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=InvalidContractsWithInventory.xls")
        Response.Write(litReport.Text)
        Response.End()
    End Sub
End Class
