<%@ Page Language="VB" AutoEventWireup="false" CodeFile="vendorRequest.aspx.vb" Inherits="Accounting_vendorRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    		<table border="1" bordercolor="#000000">
			<tr>
				<th colspan="4" bgcolor="#FFFF00" bordercolor="#000000"><i>
				<font size="6">KING'S CREEK PLANTATION</font></i></th>
			</tr>
			<tr>
				<td colspan="4">&nbsp;</td>
			</tr>
			<tr>
				<th colspan="4">VENDOR APPLICATION / CHANGE FORM</th>
			</tr>
			<tr>
				<td colspan="4"><font color="#0000FF"><i>The information provided on this form is required by management in order to process invoices.</i></font></td>
			</tr>
			<tr>
				<th colspan="4" height="22">&nbsp;</th>
			</tr>
			<tr>
				<th colspan="4" bgcolor="#C0C0C0" bordercolor="#000000">
				<p align="left"><b>COMPANY DETAILS</b></th>
			</tr>
			<tr>
				<td align="center" colspan="2"><input type="checkbox" name="newvendor">&nbsp;<b>New Vendor</b></td>
				<td align="center" colspan="2"><input type="checkbox" name="changevendor">&nbsp;<b>Change to Existing Vendor</b></td>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0"><b>Company Name:</b></td>
				<td colspan="3"><input type="text" name="vendorname" value="" size="46"></td>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0"><b>Tax ID:</b></td>
				<td colspan="3"><input type="text" name="taxid" value="" size="46"></td>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0"><b>Vendor Type:</b></td>
				<td>Supplier&nbsp;<input type="checkbox" name="Supplier"></td>
				<td>Service -- On Site &nbsp; <input type="checkbox" name="serviceonsite">(Insurance)</td>
				<td>Service -- Off Site &nbsp; <input type="checkbox" name = "serviceoffsite"></td>
			</tr>
			<tr>
				<th colspan="4" bgcolor="#C0C0C0">
				<p align="left"><b>BUSINESS ADDRESS:</b></th>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0"><b>Street:</b></td>
				<td colspan="3"><input type="text" name = "streetaddress" value="" size="46"></td>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0"><b>State:</b></td>
				<td><select name="stateid"><option value=0></option></select></td>
				<td bgcolor="#C0C0C0"><b>Zip Code:</b></td>
				<td><input type="text" value="" name="zip"></td>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0"><b>Phone No:</b></td>
				<td><input type="text" name="phone1" value=""></td>
				<td bgcolor="#C0C0C0"><b>Facsimile No:</b></td>
				<td><input type="text" value="" name="fax"></td>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0"><b>Contact Person:</b></td>
				<td><input type="text" name="contact" value=""></td>
				<td bgcolor="#C0C0C0"><b>Mobile No:</b></td>
				<td><input type="text" value="" name ="mobile"></td>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0"><b>Email Address:</b></td>
				<td colspan="3"><input type="text" name = "email" value = "" size = "46"></td>
			</tr>
			<tr>
				<td colspan="4">&nbsp;</td>
			</tr>
			<tr>
				<th bgcolor="#C0C0C0" colspan="4">
				<p align="left">FURTHER INFORMATION REQUIRED</th>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0" colspan="2"><b>W-9 attached (mandatory for all vendors).</b></td>
				<td>Yes&nbsp;<input type="checkbox" name = "w9yes"></td>
				<td>No&nbsp;<input type="checkbox" name = "w9no"></td>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0" colspan="2"><b>Certificate of Insurance (for service vendors):</b></td>
				<td>Vorkers Compensation&nbsp;<input type="checkbox" name = "workerscomp"></td>
				<td>General Liability&nbsp;<input type="checkbox" name = "generalliability"></td>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0" colspan="2"><b>Special Payment Instructions (if any):</b></td>
				<td colspan="2">
				<textarea name="specialpaymentinstructions" rows="2" cols="39"></textarea></td>
			</tr>
			<tr>
				<td colspan="4">&nbsp;</td>
			</tr>
			<tr>
				<th bgcolor="#C0C0C0" colspan="4">
				<p align="left">SUBMITTED BY</th>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0" colspan="4"><b>APPROVAL -- Department Manager</b></td>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0"><b>Name:</b></td>
				<td><input type="text" name="submittedby"></td>
				<td bgcolor="#C0C0C0"><b>Signature:</b></td>
				<td><input type="text" name="signature"></td>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0"><b>Position:</b></td>
				<td><input type="text" name="submittedposition"></td>
				<td bgcolor="#C0C0C0"><b>Date:</b></td>
				<td><input type="text" name="datesubmitted"></td>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0" colspan="4"><b>APPROVAL -- Finance Department</b></td>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0"><b>Name:</b></td>
				<td><input type="text" name="approvedby"></td>
				<td bgcolor="#C0C0C0"><b>Signature:</b></td>
				<td><input type="text" name="signature"></td>
			</tr>
			<tr>
				<td bgcolor="#C0C0C0"><b>Position:</b></td>
				<td><input type="text" name="approvedposition"></td>
				<td bgcolor="#C0C0C0"><b>Date:</b></td>
				<td><input type="text" name="dateapproved"></td>
			</tr>
			<tr>
				<td colspan="4" align="center"><input type="submit" value="Save"><input type="reset" value="Reset Form"><input type="button" value = "Cancel" /></td>
			</tr>
		</table>
    </div>
    </form>
</body>
</html>
