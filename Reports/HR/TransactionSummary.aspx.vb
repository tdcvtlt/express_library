
Partial Class Reports_HR_TradeShowPayroll
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim acct As Integer = 4
        Dim tc As String = "(78,519)"
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



        sSQL = "select i.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.Keyvalue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, i.Amount, per.username as Personnel, sol.lastname + ', ' + sol.firstname as Solicitor, sl.Location as [Sales Location], (select top 1 Email from t_prospectEmail where prospectid = p.prospectId and isPrimary = 1 and isActive = 1 order by emailID desc) Email from t_Invoices i inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = i.UserID left outer join (select p.lastname, p.firstname, pt.*, ti.comboitem from t_Personneltrans pt inner join t_Personnel p on p.personnelid = pt.personnelid inner join t_Comboitems ti on ti.comboitemid = pt.titleid where ti.comboitem = 'Tradeshow Solicitor' and pt.keyfield='packageissuedid') sol on sol.keyvalue = i.keyvalue left outer join (select vsl.Location, t.packageissuedid from t_VendorSalesLocations vsl inner join t_VendorRep2Tour vrt on vrt.salelocid = vsl.saleslocationid inner join t_Tour t on t.tourid = vrt.tourid) sl on sl.packageissuedid = i.keyvalue where i.transdate between '" & sdate & "' and '" & edate & "' and f.MerchantAccountID = '" & acct & "' and i.ApplyToID =0 and i.fintransid in " & tc & " " & _
            "union " & _
                "select ia.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.Keyvalue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, case when ia.posneg = 0 then ia.Amount else ia.Amount * -1 end as Amount, per.username as Personnel, sol.lastname + ', ' + sol.firstname as Solicitor, sl.Location as [Sales Location], (select top 1 Email from t_prospectEmail where prospectid = p.prospectId and isPrimary = 1 and isActive = 1 order by emailID desc) Email from t_Invoices i inner join t_Invoices ia on ia.applytoid = i.invoiceid inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = ia.UserID  left outer join (select  p.lastname, p.firstname, pt.*, ti.comboitem from t_Personneltrans pt inner join t_Personnel p on p.personnelid = pt.personnelid inner join t_Comboitems ti on ti.comboitemid = pt.titleid where ti.comboitem = 'Tradeshow Solicitor' and pt.keyfield='packageissuedid') sol on sol.keyvalue = i.keyvalue left outer join (select vsl.Location, t.packageissuedid from t_VendorSalesLocations vsl inner join t_VendorRep2Tour vrt on vrt.salelocid = vsl.saleslocationid inner join t_Tour t on t.tourid = vrt.tourid) sl on sl.packageissuedid = i.keyvalue   where ia.transdate between '" & sdate & "' and '" & edate & "' and f.MerchantAccountID = '" & acct & "' and i.ApplyToID =0 and i.fintransid in " & tc & " "
        rs.open(sSQL, cn, 0, 1)
        'rs.open("select i.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.Keyvalue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, i.Amount, per.username as Personnel from t_Invoices i inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = i.UserID where i.transdate between '" & sdate & "' and '" & edate & "' and f.MerchantAccountID = '" & acct & "' and i.ApplyToID > -10 and i.fintransid = '" & tc & "' order by i.transdate", cn, 0, 1)

        If rs.EOF And rs.BOF Then
            sAns = sAns & "<tr><td>No Records</td></tr>"
        Else
            sAns = sAns & "<tr>"
            For i = 0 To rs.fields.count - 1
                sAns = sAns & "<th>" & rs.fields(i).name & "</th>"
            Next
            'sAns = sAns & "<td>Phase</td>"
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

                'sAns = sAns & "<td>" & sPhase & "</td>"
                sAns = sAns & "</tr>"
                rs.movenext()
            Loop
            sAns = sAns & "<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td align='right'>Total:</td><td>" & FormatCurrency(sTotal) & "</td><td>&nbsp;</td><td>&nbsp;</td></tr>"
        End If
        sAns = sAns & "</table>"
        rs.Close()
        cn.Close()
        rs = Nothing
        cn = Nothing
        litReport.Text = sAns
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then

        End If
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=Trade Show Payroll Report.xls")
        Response.Write(litReport.Text)
        Response.End()
    End Sub
End Class
