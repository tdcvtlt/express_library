<%@ Page Language="VB" AutoEventWireup="false" CodeFile="NextArchives.aspx.vb" Inherits="mis_NextArchives" AspCompat="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <%
        Dim cn
        Dim rs
        Dim rs2
	
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")

        cn.open("CRMSNet", "asp", "aspnet")
	
        Response.Write("<h2>Archives</h2>")
	
        rs.open("Select c.contractnumber, o.lastname, o.firstname,a.arlineitemid, a.transactioncode as [Trans Code], a.Amount, a.Balance, a.Reference, a.transactiondate as [Trans Date], a.posteddate as [Posted Date], Applytolineid  from [wsql\wsql].tswdata.dbo.t_ARLineitem a left outer join [wsql\wsql].tswdata.dbo.t_Owner o on o.ownerid = a.ownerid left outer join [wsql\wsql].tswdata.dbo.t_Contract c on c.contractid = a.contractid where a.contractid in (select contractid from [wsql\wsql].tswdata.dbo.t_Contract where contractnumber in (select contractnumber from t_Contract where contractid = '" & Request("Contract") & "'))  order by a.transactiondate", cn, 0, 1)
        If rs.eof And rs.bof Then
        Else
            Response.Write("<h3>Owner: " & Trim(rs.fields("LastName").value & "") & ", " & rs.fields("Firstname").value & "</h3>")
            Response.Write("<h3>Contract Number: " & rs.fields("ContractNumber").value & "</h3>")
        End If
        Response.Write("<table border = 1>")
        Response.Write("<tr>")
        For i = 4 To rs.fields.count - 2
            Response.Write("<th>" & rs.fields(i).name & "</th>")
        Next
        Response.Write("</tr>")
        Do While Not rs.eof
            If rs.fields("Applytolineid").value = 0 Then
                Response.Write("<tr>")
                Response.Write("<td align='left'>" & rs.fields(4).value & "</td>")
                For i = 5 To rs.fields.count - 2
                    Response.Write("<td align = 'right'>&nbsp;" & rs.fields(i).value & "</td>")
                Next
                Response.Write("</tr>")
                rs2.open("Select a.arlineitemid, a.transactioncode as [Trans Code], a.Amount, a.Balance, a.Reference, a.transactiondate as [Trans Date], a.posteddate as [Posted Date], Applytolineid  from [wsql\wsql].tswdata.dbo.t_ARLineitem a where applytolineid = '" & rs.fields("arlineitemid").value & "' order by a.transactiondate", cn, 0, 1)
                Do While Not rs2.eof
                    Response.Write("<tr>")
                    For i = 1 To rs2.fields.count - 2
                        Response.Write("<td align = 'right'>&nbsp;" & rs2.fields(i).value & "</td>")
                    Next
                    Response.Write("</tr>")
                    rs2.movenext()
                Loop
                rs2.close()
            End If
            rs.movenext()
        Loop
        rs.close()
        cn.close()
        Response.Write("</table>")
        rs = Nothing
        rs2 = Nothing
        cn = Nothing
     %>
    </div>
    </form>
</body>
</html>
