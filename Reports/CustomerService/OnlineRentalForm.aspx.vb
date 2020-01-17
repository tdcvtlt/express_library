Imports System.Data
Partial Class Reports_CustomerService_OnlineRentalForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Bind()
        End If
    End Sub

    Private Sub Bind()
        Dim ds As New SqlDataSource
        ' & "Connection Timeout=60000;"
        ds.ConnectionString = Resources.Resource.cns & "Connection Timeout=60000;"
        ds.SelectCommand = "Select a.rentalagreementid as ID,contractnumber as KCP, Owner, " & _
            " '(' + left(HomePhone,3) + ')' + right(left(HomePhone,6),3) + '-' + right(HomePhone,4) as HomePhone, " & _
            " '(' + left(AltPhone,3) + ')' + right(left(AltPhone,6),3) + '-' + right(AltPhone,4) as AltPhone, " & _
            " Email, DateSubmitted, s.Type, s.Size, orf.Balance as ORFBalance, mf.Balance as [MF" & Right(Year(Date.Today) + 1, 2) & " Balance], " & _
            " se.comboitem as Season, cst.comboitem ContractType, cs.comboitem as Status, year(c.occupancydate) as OccYear, f.Frequency, case when year(c.occupancydate) % f.interval = " & Year(Date.Today) + 1 & " % f.interval then 'Yes' else 'No' end as HasOccupancy,  " & _
            " case when rest.Balance is null then 0 else rest.balance end + case when rest2.balance is null then 0 else rest2.balance end as OtherDue, a.CheckInDate " & _
            " from t_OnlineRentalAgreement a inner join t_Contract c on c.contractid = a.contractid " & _
            "   left outer join t_Comboitems se on se.comboitemid = c.seasonid " & _
            "   left outer join t_Comboitems cst on cst.comboitemid = c.subtypeid " & _
            "   left outer join t_comboitems cs on cs.comboitemid = c.statusid " & _
            "   left outer join t_Frequency f on f.frequencyid = c.frequencyid " & _
            "   left outer join (select keyvalue, sum(balance) as Balance from v_Invoices where keyfield = 'contractid' and invoice = 'orf' group by keyvalue) orf on orf.keyvalue = c.contractid " & _
            "   left outer join (select keyvalue, sum(balance) as Balance from v_Invoices where keyfield = 'contractid' and invoice = 'mf" & Right(Year(Date.Today) + 1, 2) & "' group by keyvalue) mf on mf.keyvalue = c.contractid " & _
            "   left outer join (select keyvalue, sum(balance) as Balance from v_Invoices where keyfield = 'contractid' and invoice <> 'orf' and invoice <> 'mf" & Right(Year(Date.Today) + 1, 2) & "' and duedate < '" & Date.Today.ToShortDateString & "' group by keyvalue) rest on rest.keyvalue = c.contractid " & _
            "   left outer join (select keyvalue, sum(balance) as Balance from v_Invoices where keyfield = 'prospectid' and invoice <> 'orf' and invoice <> 'mf" & Right(Year(Date.Today) + 1, 2) & "' and duedate < '" & Date.Today.ToShortDateString & "' group by keyvalue) rest2 on rest2.keyvalue = c.prospectid " & _
            "   left outer join (" & _
                    "select rentalagreementid, case when type is null then '' else type end as type, " & _
                    "   case when type = 'Cottage' then case when Cottagesize is null then 0 else CottageSize end " & _
                    "       when type = 'Townes' then case when TownesSize is null then 0 else TownesSize end " & _
                    "       when type = 'Estates' then case when EstatesSize is null then 0 else EstatesSize end  " & _
                    "       else '' end as Size " & _
                    "from (select rentalagreementid, contractid, " & _
                     "	case   " & _
                     "		when cottage3 = 1 or cottage2 = 1 or cottage1 = 1 then 'Cottage'   " & _
                     "		when townes4 = 1 or townes2 = 1 then 'Townes'   " & _
                     "		when estates4 = 1 or estates3 = 1 or estates2 = 1 or estates1up = 1 or estates1down = 1 then 'Estates'   " & _
                     "		else ''  " & _
                     "	end as Type,  " & _
                     "	sum(cottage3*3 + cottage2*2 + cottage1*1) as CottageSize,  " & _
                     "	sum(townes4*4 + townes2*2 ) as TownesSize,  " & _
                     "	sum(estates4*4 + estates3*3 + estates2*2 + estates1up*1 + estates1down*1) as EstatesSize  " & _
                     "from t_OnlineRentalagreement  " & _
                     "group by rentalagreementid, contractid, case   " & _
                     "	when cottage3 = 1 or cottage2 = 1 or cottage1 = 1 then 'Cottage'   " & _
                     "	when townes4 = 1 or townes2 = 1 then 'Townes'   " & _
                     "	when estates4 = 1 or estates3 = 1 or estates2 = 1 or estates1up = 1 or estates1down = 1 then 'Estates'   " & _
                     "	else ''  " & _
                     "	end  )c1) s on s.rentalagreementid = a.rentalagreementid " & _
            " where created = 0 order by a.rentalagreementid"
        gvRentals.DataSource = ds
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvRentals.DataKeyNames = sKeys
        gvRentals.DataBind()
        ds = Nothing
    End Sub


    Protected Sub gvRentals_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvRentals.RowCommand
        Dim cn As New Data.SqlClient.SqlConnection(Resources.Resource.cns)
        Dim cm As New Data.SqlClient.SqlCommand("Update t_OnlineRentalAgreement set created = 1 where rentalagreementid = '" & gvRentals.DataKeys(e.CommandArgument).Value & "'", cn)
        Try
            cn.Open()
            cm.ExecuteNonQuery()
        Catch ex As Exception
            
            ClientScript.RegisterClientScriptBlock(Me.GetType, "test", "alert('" & Replace(ex.ToString, "'", "\'") & "');", True)
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try

        cn = Nothing
        cm = Nothing
        
        Bind()
    End Sub


    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Response.AddHeader("content-disposition", "attachment;filename=FileName.xls")
        Response.Charset = String.Empty
        Response.ContentType = "application/vnd.xls"
        Response.Write("<table><tr>")
        For i = 1 To gvRentals.HeaderRow.Cells.Count - 1
            Response.Write("<th>" & gvRentals.HeaderRow.Cells(i).Text & "</th>")
        Next
        Response.Write("</tr>")
        For i = 0 To gvRentals.Rows.Count - 1
            Response.Write("<tr>")
            For x = 1 To gvRentals.Rows(i).Cells.Count - 1
                Response.Write("<td>" & gvRentals.Rows(i).Cells(x).Text & "</td>")
            Next
        Next
        Response.Write("</tr></table>")
        Response.End()
    End Sub

    Protected Sub btnComplete_Click(sender As Object, e As EventArgs) Handles btnComplete.Click
        Dim cn As New Data.SqlClient.SqlConnection(Resources.Resource.cns)
        Dim cm As New Data.SqlClient.SqlCommand("", cn)

        Try
            cn.Open()
            For Each row As GridViewRow In gvRentals.Rows
                cm.CommandText = "Update t_OnlineRentalAgreement set created = 1 where rentalagreementid = '" & row.Cells(1).Text & "'"
                cm.ExecuteNonQuery()
            Next
        Catch ex As Exception
            ClientScript.RegisterClientScriptBlock(Me.GetType, "test", "alert('" & Replace(ex.ToString, "'", "\'") & "');", True)
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Bind()
    End Sub
End Class
