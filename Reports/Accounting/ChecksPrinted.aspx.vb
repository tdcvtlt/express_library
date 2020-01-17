
Partial Class Reports_Accounting_ChecksPrinted
    Inherits System.Web.UI.Page

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        If dfStartDate.Selected_Date = "" Or dfEndDate.Selected_Date = "" Then
            Lit1.Text = "Please select a Date Range"
            Exit Sub
        End If
        Dim cn As Object
        Dim rs As Object
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")

        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)


        rs.open("Select * from t_Checks c inner join t_PremiumIssued i on i.premiumissuedid = c.premiumissuedid inner join t_Tour t on t.tourid = i.keyvalue inner join t_Prospect p on p.prospectid = t.prospectid where c.voided <> 1 and c.dateprinted between '" & dfStartDate.Selected_Date & "' and '" & dfEndDate.Selected_Date & "'", cn, 0, 1)
        Lit1.Text = ""
        If rs.eof And rs.bof Then
            Lit1.Text &= ("No Records")
        Else

            Lit1.Text &= ("<table border = 1>")
            Lit1.Text &= ("<tr>")
            Lit1.Text &= ("<th>type_descr</th><th>type</th><th>Date</th><th>Num</th><th>Description</th><th>account</th><th>Amount</th><th>Date</th>")
            Lit1.Text &= ("</tr>")
            Do While Not rs.eof
                Lit1.Text &= ("<tr>")
                Lit1.Text &= ("<td>check</td>")
                Lit1.Text &= ("<td>1</td>")
                Lit1.Text &= ("<td>")
                If InStr(rs.fields("DatePrinted").value & "", " ") > 0 Then
                    Lit1.Text &= (Left(rs.fields("DatePrinted").value, InStr(rs.fields("DatePrinted").value, " ")))
                Else
                    Lit1.Text &= (rs.fields("DatePrinted").value)
                End If
                Lit1.Text &= ("</td>")
                Lit1.Text &= ("<td>" & rs.fields("CheckNo").value & "</td>")
                Lit1.Text &= ("<td>" & rs.fields("FirstName").value & " " & rs.fields("LastName").value & "</td>")
                Lit1.Text &= ("<td>6420-10-00-0-00-000</td>")
                Lit1.Text &= ("<td align='right'>" & FormatCurrency(rs.fields("CostEa").value) & "</td>")
                Lit1.Text &= ("<td>")
                If InStr(rs.fields("DatePrinted").value & "", " ") > 0 Then
                    Lit1.Text &= (Left(rs.fields("DatePrinted").value, InStr(rs.fields("DatePrinted").value, " ")))
                Else
                    Lit1.Text &= (rs.fields("DatePrinted").value)
                End If
                Lit1.Text &= ("</td>")
                Lit1.Text &= ("</tr>")
                rs.movenext()
            Loop
            Lit1.Text &= ("</table>")
        End If
        rs.close()


        cn.close()
        cn = Nothing
        rs = Nothing
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'Response.Clear()
        If dfStartDate.Selected_Date = "" Or dfEndDate.Selected_Date = "" Then
            Lit1.Text = "Please select a Date Range"
            Exit Sub
        End If
        Dim cn As Object
        Dim rs As Object
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")

        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)


        rs.open("Select * from t_Checks c inner join t_PremiumIssued i on i.premiumissuedid = c.premiumissuedid inner join t_Tour t on t.tourid = i.keyvalue inner join t_Prospect p on p.prospectid = t.prospectid where c.voided <> 1 and c.dateprinted between '" & dfStartDate.Selected_Date & "' and '" & dfEndDate.Selected_Date & "'", cn, 0, 1)
        Lit1.Text = ""
        If rs.eof And rs.bof Then
            Lit1.Text = ("No Records")
        Else
            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", "attachment; filename=checkexport.xls")
            Response.Write("<table border = 1>")
            Response.Write("<tr>")
            Response.Write("<th>type_descr</th><th>type</th><th>Date</th><th>Num</th><th>Description</th><th>account</th><th>Amount</th><th>Date</th>")
            Response.Write("</tr>")
            Do While Not rs.eof
                Response.Write("<tr>")
                Response.Write("<td>check</td>")
                Response.Write("<td>1</td>")
                Response.Write("<td>")
                If InStr(rs.fields("DatePrinted").value & "", " ") > 0 Then
                    Response.Write(Left(rs.fields("DatePrinted").value, InStr(rs.fields("DatePrinted").value, " ")))
                Else
                    Response.Write(rs.fields("DatePrinted").value)
                End If
                Response.Write("</td>")
                Response.Write("<td>" & rs.fields("CheckNo").value & "</td>")
                Response.Write("<td>" & rs.fields("FirstName").value & " " & rs.fields("LastName").value & "</td>")
                Response.Write("<td>6420-10-00-0-00-000</td>")
                Response.Write("<td align='right'>" & FormatCurrency(rs.fields("CostEa").value) & "</td>")
                Response.Write("<td>")
                If InStr(rs.fields("DatePrinted").value & "", " ") > 0 Then
                    Response.Write(Left(rs.fields("DatePrinted").value, InStr(rs.fields("DatePrinted").value, " ")))
                Else
                    Response.Write(rs.fields("DatePrinted").value)
                End If
                Response.Write("</td>")
                Response.Write("</tr>")
                rs.movenext()
            Loop
            Response.Write("</table>")
        End If
        rs.close()


        cn.close()
        cn = Nothing
        rs = Nothing
        

        Response.End()
    End Sub
End Class
