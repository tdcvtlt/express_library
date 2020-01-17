
Partial Class Reports_Accounting_InactiveContractswithbalance
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Run_Report()
    End Sub

    Private Sub Run_Report()
        Lit1.Text = ""
        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")

        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        rs.open("select distinct p.prospectid,c.contractid, c.contractnumber,p.lastname + ', ' + p.firstname as Owner,c.contractdate, s.comboitem as Status from t_contract c inner join t_prospect p on p.prospectid = c.prospectid inner join t_comboitems s on c.statusid = s.comboitemid where c.contractid in (select distinct keyvalue from t_Invoices where keyfield = 'contractid') and c.statusid in (select comboitemid from t_comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'ContractStatus' and (comboitem like 'Can%' or comboitem like 'CXL%')) ", cn, 3, 3)



        Lit1.Text &= ("<table>")
        Lit1.Text &= ("<tr>")
        Lit1.Text &= ("<th><b>Contract Number</b></th>")
        Lit1.Text &= ("<th><b>Owner</b></th>")
        Lit1.Text &= ("<th><b>Contract Status</b></th>")
        Lit1.Text &= ("<th><b>Contract Date</b></th>")
        Lit1.Text &= ("<th><b>ARBalance</b></th>")
        Lit1.Text &= ("</tr>")

        Do While Not rs.eof
            If rs.eof Then
                Lit1.Text &= ("<b>No In-Active Contracts Have Balances.</b>")
            Else
                rs2.open("select case when sum(case when balance is null then 0 else balance end) is null then 0 else sum(case when balance is null then 0 else balance end) end as Balance from ufn_Financials(" & rs.fields("ProspectiD").value & ",'ContractID'," & rs.fields("ContractID").value & ",0)", cn, 0, 1)
                If Not (rs2.eof And rs2.bof) Then
                    If rs2.fields("Balance").value > 0 Then

                        Lit1.Text &= ("<tr>")
                        Lit1.Text &= ("<td><a href='../../editcontract.asp?contractid==" & rs.fields("contractid").value & "'>" & rs.fields("ContractNumber").value & "</a></td>")
                        Lit1.Text &= ("<td>" & rs.fields("owner").value & "</td>")
                        Lit1.Text &= ("<td>" & rs.fields("Status").value & "</td>")
                        Lit1.Text &= ("<td>" & rs.fields("contractdate").value & "</td>")
                        Lit1.Text &= ("<td>" & FormatCurrency(rs2.fields("Balance").value) & "</td>")
                        Lit1.Text &= ("</tr>")
                    End If
                End If
                rs2.close()
            End If
            rs.movenext()
        Loop
        Lit1.Text &= ("</table>")

        rs.close()
        cn.close()

        rs = Nothing
        cn = Nothing
        rs2 = Nothing

        Lit1.Text &= ("</table>")
    End Sub
End Class
