<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Spider.aspx.vb" Inherits="online_Spider" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <table border=1 width=500px  bordercolor='#808080' bgcolor='#F3F3F3' style='border-collapse: collapse' cellpadding="5" cellspacing="3" >
        <tr>
            <td>
                <form method="POST" action='http://www.webconfs.com/search-engine-spider-simulator.php'>
                    <p><center><font style='font-size: 11pt; font-family: "Verdana, Arial";'><b>Search Engine Spider Simulator</b></font></center></p>
                    <p><font class='font-size: 11pt; font-family: "Verdana, Arial";'><b>Enter URL to Spider</b></font></p>
                    <p><input type=text name="url" size=60></p>
                    <input type=hidden name="submit" value="submit">
                    <p><input type="submit" value="submit" name="submit"></p>
                </form>
            </td>
        </tr>
    </table>
</body>
</html>
