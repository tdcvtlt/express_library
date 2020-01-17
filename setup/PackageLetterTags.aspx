<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PackageLetterTags.aspx.vb" Inherits="setup_PackageLetterTags" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 54px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Insert the Following Tags to Have these values inserted into letter: 
        
        
        <table>
            <tr>
                <td class="style1">&lt;DATE&gt;</td>
                <td>Current Date</td>
            </tr>
            <tr>
                <td class="style1">&lt;NAME&gt;</td>
                <td>Prospect Name (First + Last)</td>
            </tr>
            <tr>
                <td class="style1">&lt;ADDRESS&gt;</td>
                <td>Prospect Street Address</td>
            </tr>
            <tr>
                <td class="style1">&lt;CITY&gt;</td>
                <td>Prospect City</td>
            </tr>
            <tr>
                <td class="style1">&lt;STATE&gt;</td>
                <td>Prospect State</td>
            </tr>
            <tr>
                <td class="style1">&lt;ZIP&gt;</td>
                <td>Prospect Zip Code</td>
            </tr>
            <tr>
                <td class="style1">&lt;SALEAMOUNT&gt;</td>
                <td>Package Price</td>
            </tr>
            <tr>
                <td class="style1">&lt;PACKAGEID&gt;</td>
                <td>Package ID</td>
            </tr>
            <tr>
                <td class="style1">&lt;SALEDATE&gt;</td>
                <td>Package Sale Date</td>
            </tr>
            <tr>
                <td class="style1">&lt;EMAIL&gt;</td>
                <td>Prospect Email Address</td>
            </tr>
            <tr>
                <td class="style1">&lt;COUNTRY&gt;</td>
                <td>Prospect Country</td>
            </tr>
            <tr>
                <td class="style1">&lt;EXPDATE&gt;</td>
                <td>Package Expiration Date</td>
            </tr>
            <tr>
                <td class="style1">&lt;RESLOCATION&gt;</td>
                <td>Reservation Location</td>
            </tr>
            <tr>
                <td colspan = "2">To insert VRC Logo use this address:</td>
            </tr>
            <tr>
                <td colspan = "2">http://vendors.kingscreekplantation.com/vendors/test.net/images/vrclogo.png</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
