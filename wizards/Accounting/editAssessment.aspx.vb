
Imports System.Data.SqlClient

Partial Class wizards_Accounting_editAssessment
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Fill_Statuses()
            ddType.Items.Add("Choose")
            ddType.Items.Add("Maintenance Fees")
            ddType.Items.Add("Maintenance Fee Late Fee")
            ddType.Items.Add("Club Dues")
            ddType.Items.Add("Club Due Late Fee")
            If Request("BatchID") <> "0" And IsNumeric(Request("BatchID")) And Request("BatchID") <> "" Then
                'Load Existing
            Else
                'Load New
                tblDetails.Visible = False
            End If
            mvMain.ActiveViewIndex = 0
        End If

    End Sub
    Protected Sub ddType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddType.SelectedIndexChanged
        tblDetails.Visible = True
        trY2A.Visible = True
        Select Case ddType.SelectedItem.Text
            Case "Maintenance Fees"
                ddY2A.DataSource = (New SqlDataSource With {.ConnectionString = Resources.Resource.cns, .SelectCommand = "Select FintransID, Comboitem as Year from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid inner join t_FinTranscodes f on f.transcodeid = i.comboitemid where i.comboitem like 'MF%' and c.ComboName = 'TransCode' order by cast(right(i.comboitem, 2) as int)"})
                ddY2A.DataTextField = "Year"
                ddY2A.DataValueField = "FinTransID"
                ddY2A.DataBind()
                trCutOffDate.Visible = True
                trDueDate.Visible = True
                trLFAmount.Visible = False
                trDaysLate.Visible = False
                trStatuses.Visible = True
            Case "Maintenance Fee Late Fee"
                ddY2A.DataSource = (New SqlDataSource With {.ConnectionString = Resources.Resource.cns, .SelectCommand = "Select FintransID, Comboitem as Year from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid inner join t_FinTranscodes f on f.transcodeid = i.comboitemid where i.comboitem like 'MF%' and c.ComboName = 'TransCode' order by cast(right(i.comboitem, 2) as int)"})
                ddY2A.DataTextField = "Year"
                ddY2A.DataValueField = "FinTransID"
                ddY2A.DataBind()
                trCutOffDate.Visible = False
                trDueDate.Visible = False
                trLFAmount.Visible = True
                trDaysLate.Visible = True
                trStatuses.Visible = False
            Case "Club Dues"
                ddY2A.DataSource = (New SqlDataSource With {.ConnectionString = Resources.Resource.cns, .SelectCommand = "Select FintransID, i.Comboitem as Year from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid inner join t_FinTranscodes f on f.transcodeid = i.comboitemid inner join t_ComboItems tt on f.TransTypeID = tt.ComboItemID where tt.ComboItem = 'ProspectTrans' and i.comboitem like 'CD%' order by cast(right(i.comboitem, 2) as int)"})
                ddY2A.DataTextField = "Year"
                ddY2A.DataValueField = "FinTransID"
                ddY2A.DataBind()
                trCutOffDate.Visible = True
                trDueDate.Visible = True
                trLFAmount.Visible = False
                trDaysLate.Visible = False
                trStatuses.Visible = True
            Case "Club Due Late Fee"
                ddY2A.DataSource = (New SqlDataSource With {.ConnectionString = Resources.Resource.cns, .SelectCommand = "Select FintransID, i.Comboitem as Year from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid inner join t_FinTranscodes f on f.transcodeid = i.comboitemid inner join t_ComboItems tt on f.TransTypeID = tt.ComboItemID where tt.ComboItem = 'ProspectTrans' and i.comboitem like 'CD%' order by cast(right(i.comboitem, 2) as int)"})
                ddY2A.DataTextField = "Year"
                ddY2A.DataValueField = "FinTransID"
                ddY2A.DataBind()
                trCutOffDate.Visible = False
                trDueDate.Visible = False
                trLFAmount.Visible = True
                trDaysLate.Visible = True
                trStatuses.Visible = False
            Case Else
                tblDetails.Visible = False
        End Select
    End Sub
    Private Sub Fill_Statuses()
        Dim cn As New System.Data.SqlClient.SqlConnection(Resources.Resource.cns)
        Dim cm As New System.Data.SqlClient.SqlCommand("Select comboitemid, comboitem from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname = 'ContractStatus' order by comboitem", cn)
        Dim dr As System.Data.SqlClient.SqlDataReader

        cn.Open()
        dr = cm.ExecuteReader
        If dr.HasRows Then
            While dr.Read
                Dim item As New ListItem
                item.Value = dr("ComboitemID")
                item.Text = dr("Comboitem")
                If item.Text = "Active" Or item.Text = "On Hold" Or item.Text = "OnHold" Then
                    lstStatus.ClearSelection()
                    lstStatus.Items.Add(item)
                Else
                    lstNewStatus.ClearSelection()
                    lstNewStatus.Items.Add(item)
                End If
            End While
        End If
        dr.Close()
        cn.Close()
        cn = Nothing
        cm = Nothing
        dr = Nothing
    End Sub

    Protected Sub btnRight_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If lstNewStatus.SelectedIndex > -1 Then
            Dim item As ListItem = lstNewStatus.SelectedItem
            lstStatus.ClearSelection()
            lstStatus.Items.Add(item)
            lstNewStatus.Items.Remove(item)
        End If
    End Sub

    Protected Sub btnLeft_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        If lstStatus.SelectedIndex > -1 Then
            Dim item As ListItem = lstStatus.SelectedItem
            lstNewStatus.ClearSelection()
            lstNewStatus.Items.Add(item)
            lstStatus.Items.Remove(item)
        End If
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        cm.CommandText = Get_SQL()
        cm.CommandTimeout = 0
        cn.Open()
        cm.ExecuteNonQuery()
        cn.Close()
        cm = Nothing
        cn = Nothing
        Load_GridView()
    End Sub

    Protected Sub btnAssess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAssess.Click

    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Delete from t_AssessmentBatchItem where BatchItemID=" & GridView1.SelectedValue.ToString, cn)
        cn.Open()
        cm.ExecuteNonQuery()
        cn.Close()
        cn = Nothing
        cm = Nothing
        Load_GridView()
    End Sub
    Protected Sub Setup_Click(sender As Object, e As EventArgs) Handles Setup.Click
        mvMain.ActiveViewIndex = 0
    End Sub
    Protected Sub Assessment_Click(sender As Object, e As EventArgs) Handles Assessment.Click
        Load_GridView()
    End Sub
    Protected Sub Print_Click(sender As Object, e As EventArgs) Handles Print.Click
        mvMain.ActiveViewIndex = 2
    End Sub

    Private Sub Load_GridView()
        Dim ds As New SqlDataSource
        Dim sSQL As String = ""
        Select Case ddType.SelectedItem.Text
            Case "Maintenance Fees"
                sSQL = "Select BatchItemID, BatchID, ProspectID, ContractID, LastName, Firstname, KCP, AmountBilled, Type, Status, SubStatus, MFStatus from t_AssessmentBatchItem where batchid=" & Request("BatchID")
            Case "Maintenance Fee Late Fee"
                sSQL = "Select BatchItemID,BatchID, ProspectID, ContractID, LastName, Firstname, KCP, AmountBilled, Type, Status, SubStatus, MFStatus from t_AssessmentBatchItem where batchid=" & Request("BatchID")
            Case "Club Dues"
                sSQL = "Select BatchItemID,BatchID, ProspectID, LastName, Firstname, AnniversaryDate, Frequency, Status, AmountBilled from t_AssessmentBatchItem where batchid=" & Request("BatchID")
            Case "Club Due Late Fee"
                sSQL = "Select BatchItemID,BatchID, ProspectID, LastName, Firstname, AmountBilled from t_AssessmentBatchItem where batchid=" & Request("BatchID")
            Case Else
                sSQL = "Select * from t_AssessmentBatchItem where batchid=" & Request("BatchID")
        End Select
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = sSQL
        GridView1.DataSource = ds
        GridView1.DataBind()
        mvMain.ActiveViewIndex = 1
    End Sub

    Private Function Get_SQL() As String
        Dim ret As String = ""
        Dim sStatus As String = ""
        Dim MFYear As String = Right(ddY2A.SelectedItem.Text, 2)

        Dim sDate As String = dfCutoffDate.Selected_Date
        Dim dDate As String = dfDueDate.Selected_Date
        Dim sYear As String = "20" + ddY2A.SelectedItem.Text.Substring(2, 2)
        MFYear = IIf(CInt(MFYear) < 0, "19" & MFYear, "20" & MFYear)
        For i = 0 To lstStatus.Items.Count - 1
            sStatus &= IIf(sStatus = "", lstStatus.Items(i).Value, "," & lstStatus.Items(i).Value)
        Next
        Select Case ddType.SelectedItem.Text
            Case "Maintenance Fees"
                ret = "insert into t_AssessmentBatchItem (BatchID, ProspectID, ContractID, LastName, Firstname, KCP, AmountBilled, Type, Status, SubStatus, MFStatus, Address1, City, State, PostalCode, Country, BalanceForward, TotalDue, Frequency, Invoice) " &
                    "select 0 as BatchID, ProspectID, ContractID, Lastname, Firstname, KCP#, [To Be Assessed], Type, Status, SubStatus, MFStatus, Address1, City, State, PostalCode, Country, BalanceForward, Coalesce(BalanceForward,0) + coalesce([To Be Assessed],0) as TotalDue, Frequency, '" & ddY2A.SelectedItem.Text & "' as Invoice  from (	 " &
                        "select p.Prospectid,p.lastname , p.firstname , c.contractnumber as [KCP#],  " &
                        "(select top 1 address1 from t_ProspectAddress where activeflag=1 And prospectid = p.prospectid) as address1,  " &
                        "(select top 1 city from t_ProspectAddress where activeflag=1 And prospectid = p.prospectid) as city,  " &
                        "(select top 1 comboitem from t_ProspectAddress a left outer join t_Comboitems s on s.comboitemid = a.stateid where activeflag=1 And prospectid = p.prospectid) as  State,  " &
                        "(select top 1 postalcode from t_ProspectAddress where activeflag=1 And prospectid = p.prospectid) as postalcode,  " &
                        "(select top 1 comboitem from t_ProspectAddress a left outer join t_Comboitems co on co.comboitemid = a.countryid where activeflag=1 And prospectid = p.prospectid) as  country, " &
                        "	case when mfc.Amount Is null then 0 else mfc.amount end AS MFAmount, " &
                        "	case when i.Total Is null then 0 else i.Total end as [Previously Assessed], " &
                        "	case when case when i.Total Is null then 0 else i.Total end - case when mfc.Amount Is null then 0 else mfc.amount end  < 0 then " &
                        "		case when i.Total Is null then 0 else i.Total end - case when mfc.Amount Is null then 0 else mfc.amount end else " &
                        "0 " &
                        "	end  * -1 as [To Be Assessed], " &
                        "	case when charindex(' ', st.comboitem) > 0 then left(st.comboitem, charindex(' ',st.comboitem)) else st.comboitem end as Type " &
                        "   ,cs.comboitem as Status, css.comboitem as SubStatus, mfs.comboitem as MFStatus, c.contractid, f.Frequency, bf.BalanceForward " &
                        "from t_Contract c  " &
                        "	left outer join t_Comboitems st on st.comboitemid = c.saletypeid  " &
                        "   left outer join (Select keyvalue, sum(balance)as Balanceforward from v_invoices where keyfield = 'contractid' and duedate <= '" & dfCutoffDate.Selected_Date & "' and invoice <> '" & ddY2A.SelectedItem.Text & " ' group by keyvalue) bf on bf.keyvalue = c.contractid " &
                        "   inner join t_MaintenanceFeeCode2FinTrans mfc on mfc.MaintenanceFeeCodeID = c.MaintenanceFeeCodeID " &
                        "	inner join t_FinTransCodes ft on ft.FinTransID = mfc.FinTransID " &
                        "	inner join t_ComboItems tc on tc.ComboItemID = ft.TransCodeID and tc.ComboItem = '" & ddY2A.SelectedItem.Text & "' " &
                        "	inner join t_Prospect p on p.prospectid = c.prospectid " &
                        "    left outer join t_Frequency f on f.frequencyid = c.frequencyid  " &
                        "   left outer join ( " &
                        "		select keyvalue as Contractid, sum(case when i.posneg = 0 then i.Amount else i.amount * -1 end + case when a.adj is null then 0 else a.adj end) as Total " &
                        "		from t_Invoices i " &
                        "           left outer join ( " &
                        "               select a.applytoid, sum(case when posneg = 0 then amount else amount * -1 end) as Adj " &
                        "               from t_Invoices a " &
                        "               where a.applytoid > 0 " &
                        "               Group by a.applytoid " &
                        "           ) a on a.applytoid = i.invoiceid " &
                        "           inner join t_FintransCodes f on f.fintransid = i.fintransid " &
                        "           inner join t_Comboitems tc on tc.comboitemid = f.transcodeid " &
                        "		where i.keyfield = 'contractid' and i.applytoid =0  " &
                        "			and tc.comboitem = '" & ddY2A.SelectedItem.Text & "' " &
                        "		group by keyvalue) as i on i.contractid = c.contractid " &
                        "   left outer join t_Comboitems cs on cs.comboitemid = c.statusid " &
                        "   left outer join t_Comboitems css on css.comboitemid = c.substatusid " &
                        "   left outer join t_Comboitems mfs on mfs.comboitemid = c.maintenancefeestatusid " &
                        "where c.prospectid not in (15175110,15157867, 15187541) and c.statusid in (" & sStatus & ")  " &
                        "	and (" & MFYear & "-year(c.occupancydate))%f.interval = 0  " &
                        "	and year(c.occupancydate)<= " & MFYear & " " &
                        "    and st.comboitem <> 'trial' " &
                        "	and c.contractdate <='" & dfCutoffDate.Selected_Date & "' " &
                        ")a where [To Be Assessed]>0"
            Case "Maintenance Fee Late Fee"
                ret = "insert into t_AssessmentBatchItem (BatchID, ProspectID, ContractID, LastName, Firstname, KCP, AmountBilled, Type, Status, SubStatus, MFStatus) select 0 as BatchID, ProspectID, ContractID, Lastname, Firstname, KCP#, [LFAmount], Type, Status, SubStatus, MFStatus  from (" &
                        "select p.prospectid, p.lastname, p.firstname, c.contractnumber as [KCP#], i.Invoice as TransCode, i.amount as [AmountInvoiced], i.balance as Balance, " & txtLFAmount.Text & " as [LFAmount],  inv.saletype as Type, cs.comboitem as Status, css.comboitem as SubStatus, mfs.comboitem as MFStatus, c.ContractID " &
                            "from v_Invoices i  " &
                            "	inner join t_Contract c on c.contractid = i.keyvalue " &
                            "	inner join t_Prospect p on p.prospectid = c.prospectid " &
                            "	left outer join v_ContractInventory inv on inv.contractid = c.contractid " &
                            "   left outer join t_Comboitems cs on cs.comboitemid = c.statusid " &
                            "   left outer join t_Comboitems css on css.comboitemid = c.substatusid " &
                            "   left outer join t_Comboitems mfs on mfs.comboitemid = c.maintenancefeestatusid " &
                            "where  c.statusid not in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname='ContractStatus' and i.comboitem like 'C%') and i.invoice = '" & ddY2A.SelectedItem.Text & "'  " &
                            "	and  i.balance -100 >  " &
                            "		case when inv.frequency <> 'Triennial' then  " &
                            "           0 " &
                            "		else  " &
                            "			case when i.amount = i.balance then 0 else i.amount - ((i.amount / 3.00) * (year(getdate())- (2000 + cast(right(i.invoice,2) as int)-1))) end  " &
                            "        End " &
                            "	and i.id not in ( " &
                            "  Select m.mfinvoiceid " &
                            "		from t_lf2mf m inner join t_Invoices i on i.invoiceid = m.lfinvoiceid  " &
                            "           inner join t_Contract c on c.contractid = i.keyvalue " &
                            "           inner join t_Frequency f on f.frequencyid = c.frequencyid " &
                            "		where year(i.transdate) = case when f.frequency = 'Triennial' then year(getdate()) else 2000 + cast(right('" & ddY2A.SelectedItem.Text & "',2) as int) end " &
                            "	)  " &
                            "	and datediff(dd,duedate,getdate())>= " & txtDaysLate.Text &
                            " union " &
                            "select p.prospectid, p.lastname, p.firstname, c.contractnumber as [KCP#], i.Invoice as TransCode, i.amount as [AmountInvoiced], i.balance as Balance, " & txtLFAmount.Text & " as [LFAmount],  inv.saletype as Type, cs.comboitem as Status, css.comboitem as SubStatus, mfs.comboitem as MFStatus, c.ContractID " &
                            "from v_Invoices i  " &
                            "	inner join t_Contract c on c.contractid = i.keyvalue " &
                            "	inner join t_Prospect p on p.prospectid = c.prospectid " &
                            "	left outer join v_ContractInventory inv on inv.contractid = c.contractid " &
                            "   left outer join t_Comboitems cs on cs.comboitemid = c.statusid " &
                            "   left outer join t_Comboitems css on css.comboitemid = c.substatusid " &
                            "   left outer join t_Comboitems mfs on mfs.comboitemid = c.maintenancefeestatusid " &
                            "where c.statusid not in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname='ContractStatus' and i.comboitem like 'C%') and i.invoice = 'MF' + cast(cast(right('" & ddY2A.SelectedItem.Text & "', 2) as int) -1 as varchar)  " &
                            "	and  i.balance -100 >  " &
                            "		case when inv.frequency <> 'Triennial' then  " &
                            "           0 " &
                            "		else  " &
                            "			case when i.amount = i.balance then 0 else i.amount - ((i.amount / 3.00) * (year(getdate())- (2000 + cast(right(i.invoice,2) as int)-1))) end  " &
                            "        End " &
                            "	and i.id not in ( " &
                            "  Select m.mfinvoiceid " &
                            "		from t_lf2mf m inner join t_Invoices i on i.invoiceid = m.lfinvoiceid  " &
                            "           inner join t_Contract c on c.contractid = i.keyvalue " &
                            "           inner join t_Frequency f on f.frequencyid = c.frequencyid " &
                            "		where year(i.transdate) = case when f.frequency = 'Triennial' then year(getdate()) else 2000 + cast(right('MF' + cast(cast(right('" & ddY2A.SelectedItem.Text & "', 2) as int) -1 as varchar),2) as int) end " &
                            "	)  " &
                            "	and datediff(dd,duedate,getdate())>= " & txtDaysLate.Text &
                            " union " &
                            "select p.prospectid, p.lastname , p.firstname , c.contractnumber as [KCP#], i.Invoice as TransCode, i.amount as [AmountInvoiced], i.balance as Balance, " & txtLFAmount.Text & " as [LFAmount],  inv.saletype as Type, cs.comboitem as Status, css.comboitem as SubStatus, mfs.comboitem as MFStatus, c.ContractID " &
                            "from v_Invoices i  " &
                            "	inner join t_Contract c on c.contractid = i.keyvalue " &
                            "	inner join t_Prospect p on p.prospectid = c.prospectid " &
                            "	left outer join v_ContractInventory inv on inv.contractid = c.contractid " &
                            "   left outer join t_Comboitems cs on cs.comboitemid = c.statusid " &
                            "   left outer join t_Comboitems css on css.comboitemid = c.substatusid " &
                            "   left outer join t_Comboitems mfs on mfs.comboitemid = c.maintenancefeestatusid " &
                            "where c.prospectid not in (15175110,15157867, 15187541) and c.statusid not in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname='ContractStatus' and i.comboitem like 'C%') and i.invoice = 'MF' + cast(cast(right('" & ddY2A.SelectedItem.Text & "', 2) as int) -2 as varchar)  " &
                            "	and  i.balance -100 >  " &
                            "		case when inv.frequency <> 'Triennial' then  " &
                            "           0 " &
                            "		else  " &
                            "			case when i.amount = i.balance then 0 else i.amount - ((i.amount / 3.00) * (year(getdate())- (2000 + cast(right(i.invoice,2) as int)-1))) end  " &
                            "        End " &
                            "	and i.id not in ( " &
                            "  Select m.mfinvoiceid " &
                            "		from t_lf2mf m inner join t_Invoices i on i.invoiceid = m.lfinvoiceid  " &
                            "           inner join t_Contract c on c.contractid = i.keyvalue " &
                            "           inner join t_Frequency f on f.frequencyid = c.frequencyid " &
                            "		where year(i.transdate) = case when f.frequency = 'Triennial' then year(getdate()) else 2000 + cast(right('MF' + cast(cast(right('" & ddY2A.SelectedItem.Text & "', 2) as int) -2 as varchar),2) as int) end " &
                            "	)  " &
                            "	and datediff(dd,duedate,getdate())>= " & txtDaysLate.Text &
                            " ) a"
            Case "Club Dues"
                ret = "insert into t_AssessmentBatchItem (BatchID, ProspectID, LastName, Firstname, AnniversaryDate, Frequency, Status, AmountBilled) select 0 as BatchID,ProspectID, Lastname, Firstname, AnniversaryDate, Frequency, MostRecentContractStatus, case when Frequency = 'Annual' then 179 when Frequency = 'Biennial' then 132 else 117 end as [AmounttoAssess] " &
                            "from ( " &
                             "select distinct p.ProspectID, p.Lastname as [LastName], p.firstname as [FirstName],  " &
                              "a.an as [AnniversaryDate], " &
                              "( " &
                               "select top 1 f.frequency " &
                               "from t_Contract c " &
                                "inner join t_Comboitems cs on cs.comboitemid = c.statusid " &
                                "inner join t_Soldinventory s on s.contractid = c.contractid " &
                                "inner join t_Frequency f on f.frequencyid = s.frequencyid " &
                               "where cs.comboitem in ('Active', 'Suspense','Developer','ReDeed','In Bankruptcy','In Foreclosure','Reverter','InColl-Active', 'Deed-In-Lieu', 'InColl-Developer','Pending ReDeed') " &
                                "and c.prospectid = p.prospectid " &
                               "order by f.Frequency  " &
                              ") as Frequency, " &
                              "( " &
                              "	select top 1 cs.comboitem " &
                              "	from t_Contract c " &
                              "		inner join t_Comboitems cs on cs.comboitemid = c.statusid " &
                              "		inner join t_Soldinventory s on s.contractid = c.contractid " &
                              "		inner join t_Frequency f on f.frequencyid = s.frequencyid " &
                              "	where cs.comboitem in ('Active', 'Suspense','Developer','ReDeed','In Bankruptcy','In Foreclosure','Reverter','InColl-Active', 'Deed-In-Lieu', 'InColl-Developer','Pending ReDeed') " &
                              "		and c.prospectid = p.prospectid " &
                              "	order by c.contractid desc " &
                              ") as MostRecentContractStatus " &
                             "from t_Prospect p  " &
                              "inner join ( " &
                                "select distinct p.prospectid, min(p.anniversarydate) as AN " &
                                "from t_Prospect p  " &
                                 "inner join t_Contract c on c.prospectid = p.prospectid " &
                                 "inner join t_Comboitems cs on cs.comboitemid = c.statusid " &
                                 "inner join t_Comboitems st on st.comboitemid = c.subtypeid " &
                                "where p.anniversarydate is not null " &
                                 "and cs.comboitem not in ('Rescinded','CXL-Pender') " &
                                 "and st.comboitem = 'Points' " &
                                "group by p.prospectid " &
                               ") a on a.prospectid = p.prospectid " &
                             "where p.prospectid not in (15175110,15157867, 15187541) and year(a.an) < " & sYear & " " &
                              "and cast(cast(datepart(mm, a.an) as varchar) + '/1/' + cast(" & sYear & " as varchar) as datetime) < cast('" & sDate & "' as datetime) " &
                              "and p.prospectid not in ( " &
                                "select prospectid  " &
                                "from v_Invoices  " &
                                "where invoice = 'CD' + right(cast(" & sYear & " as varchar),2) and amount > 0 " &
                               ") " &
                              "and p.prospectid in ( " &
                                "select distinct prospectid  " &
                                "from t_Contract c  " &
                                 "inner join t_Comboitems cs on cs.comboitemid = c.statusid  " &
                                 "inner join t_Soldinventory s on s.contractid = c.contractid " &
                                 "inner join t_Comboitems st on st.comboitemid = c.subtypeid " &
                                "where c.prospectid not in (15175110,15157867, 15187541) and cs.comboitem in ('Active', 'Suspense','Developer','ReDeed','In Bankruptcy','In Foreclosure','Reverter','InColl-Active', 'Deed-In-Lieu', 'InColl-Developer','Pending ReDeed') " &
                                 "and st.comboitem = 'Points' " &
                               ") " &
                            ") a"
            Case "Club Due Late Fee"
                ret = " insert into t_AssessmentBatchItem (BatchID, ProspectID, LastName, Firstname, AmountBilled) select 0 as BatchID, p.ProspectID, p.lastname as [Last Name], p.firstname as [First Name],  " & txtLFAmount.Text & " as [LFAmount] " &
                            "from v_Invoices i  " &
                            "	inner join t_Prospect p on p.prospectid = i.prospectid " &
                            "   where p.prospectid Not in (15175110,15157867, 15187541) And i.invoice = '" & ddY2A.SelectedItem.Text & "'  " &
                            "	and i.balance > 0 " &
                            "	and i.id not in ( " &
                            "  Select cfinvoiceid " &
                            "		from t_lf2cf " &
                            "	)  " &
                            "	and datediff(dd,duedate,getdate())>= " & txtDaysLate.Text &
                            " order by p.lastname, p.firstname"

            Case Else
                ret = ""
        End Select
        Response.Write(ret)
        Return ret
    End Function
End Class
