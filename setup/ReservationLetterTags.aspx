<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReservationLetterTags.aspx.vb" Inherits="setup_ReservationLetterTags" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
                <td class="style1">&lt;COUNTRY&gt;</td>
                <td>Prospect Country</td>
            </tr>
            <tr>
                <td class="style1">&lt;EMAIL&gt;</td>
                <td>Prospect Email Address</td>
            </tr>
                        <tr>
                <td class="style1">&lt;TOURDATE&gt;</td>
                <td>Tour Date</td>
            </tr>
            <tr>
                <td class="style1">&lt;TOURTIME&gt;</td>
                <td>Tour Time</td>
            </tr>
            <tr>
                <td class="style1">&lt;GIFTS&gt;</td>
                <td>Premiums assigned to Tour</td>
            </tr>

            <tr>
                <td class="style1">&lt;RESID&gt;</td>
                <td>Reservation ID</td>
            </tr>
            <tr>
                <td class="style1">&lt;RESNUMBER&gt;</td>
                <td>Reservation Number</td>
            </tr>
            <tr>
                <td class="style1">&lt;SOURCE&gt;</td>
                <td>Reservation Source</td>
            </tr>
            <tr>
                <td class="style1">&lt;CHECKIN&gt;</td>
                <td>Reservation Check-In Date</td>
            </tr>
            <tr>
                <td class="style1">&lt;CHECKOUT&gt;</td>
                <td>Reservation Check-Out Date</td>
            </tr>
            <tr>
                <td class="style1">&lt;DAYS&gt;</td>
                <td>Reservation Number of Days</td>
            </tr>
            <tr>
                <td class="style1">&lt;NIGHTS&gt;</td>
                <td>Reservation Number of Nights</td>
            </tr>
            <tr>
                <td class="style1">&lt;BALANCE&gt;</td>
                <td>Reservation Balance</td>
            </tr>
            <tr>
                <td class="style1">&lt;DEPOSIT&gt;</td>
                <td>Reservation Amount Paid</td>
            </tr>
            <tr>
                <td class="style1">&lt;TOTAL&gt;</td>
                <td>Reservation Total Cost</td>
            </tr>
            <tr>
                <td class="style1">&lt;SIZE&gt;</td>
                <td>Reservation Unit Size (KCP ONLY)</td>
            </tr>
            <tr>
                <td class="style1">&lt;ACCOMNAME&gt;</td>
                <td>Reservation Accommodation Name</td>
            </tr>
            <tr>
                <td class="style1">&lt;ACCOMSIZE&gt;</td>
                <td>Reservation Accommodation Size (Non KCP)</td>
            </tr>
            <tr>
                <td class="style1">&lt;ACCOMADDRESS&gt;</td>
                <td>Reservation Accommodation Address</td>
            </tr>
            <tr>
                <td class="style1">&lt;ACCOMCITY&gt;</td>
                <td>Reservation Accommodation City</td>
            </tr>
            <tr>
                <td class="style1">&lt;ACCOMSTATE&gt;</td>
                <td>Reservation Accommodation State</td>
            </tr>
            <tr>
                <td class="style1">&lt;ACCOMZIP&gt;</td>
                <td>Reservation Accommodation Zip Code</td>
            </tr>
            <tr>
                <td class="style1">&lt;ACCOMDIRECTIONS&gt;</td>
                <td>Reservation Accommodation Directions</td>
            </tr>
            <tr>
                <td class="style1">&lt;CHECKINDIRECTIONS&gt;</td>
                <td>Reservation Accommodation Check-In Location Directions</td>
            </tr>
            <tr>
                <td colspan = "2">To insert VRC Logo use this address:</td>
            </tr>
            <tr>
                <td colspan = "2">http://vendors.kingscreekplantation.com/vendors/test.net/images/vrclogo.png</td>
            </tr>
                        <tr>
                <td colspan = "2">To insert KCP Logo use this address:</td>
            </tr>
            <tr>
                <td colspan = "2">http://vendors.kingscreekplantation.com/vendors/test.net/images/kcplogo.bmp</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
