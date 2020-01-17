<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditReservation.aspx.vb" Inherits="marketing_EditReservation" title="Editing A Reservation" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
<%@ Register src="../controls/UserFields.ascx" tagname="UserFields" tagprefix="uc3" %>


<%@ Register src="../controls/Financials.ascx" tagname="Financials" tagprefix="uc4" %>


<%@ Register src="../controls/Notes.ascx" tagname="Notes" tagprefix="uc5" %>
<%@ Register src="../controls/PersonnelTrans.ascx" tagname="PersonnelTrans" tagprefix="uc6" %>

<%@ Register src="../controls/VoiceStamps.ascx" tagname="VoiceStamps" tagprefix="uc7" %>

<%@ Register src="../controls/SpecialNeeds.ascx" tagname="SpecialNeeds" tagprefix="uc8" %>

<%@ Register src="../controls/UploadedDocs.ascx" tagname="UploadedDocs" tagprefix="uc9" %>

<%@ Register src="../controls/SyncDateField.ascx" tagname="SyncDateField" tagprefix="uc10" %>
<%@ Register Src="~/controls/Premiums.ascx" TagPrefix="uc1" TagName="Premiums" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!--<script type="text/javascript" language="javascript" src = "<%=request.applicationpath%>/scripts/reservations.js"></script>
    <script type="text/javascript" language="javascript" src = "<%=request.applicationpath%>/scripts/general.js"></script>-->
    <script language=javascript type ="text/javascript">
    function Refresh_Notes()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$Notes_Link','');
    }
    function Save_Res()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$lbSaveRes', '');
    
    }
    function Refresh_Res()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$Reservation_Link','');
    }
    function Refresh_VS() {
        __doPostBack('ctl00$ContentPlaceHolder1$VoiceStamps_Link', '');
    }
    function Refresh_SpecialNeeds() {
        __doPostBack('ctl00$ContentPlaceHolder1$SpecialNeeds_Link', '');
    }
    function Refresh_Rooms() {
        __doPostBack('ctl00$ContentPlaceHolder1$Rooms_Link', '');
    }
    function Refresh_Accom() {
        __doPostBack('ctl00$ContentPlaceHolder1$HotelAccom_Link', '');
    }
    function Refresh_Usages() {
        __doPostBack('ctl00$ContentPlaceHolder1$lnkUsages', '');
    }
    function Refresh_Docs() {
        __doPostBack('ctl00$ContentPlaceHolder1$UploadedDocs_Link', '');
    }
    function Refresh_Premiums() {
        __doPostBack('ctl00$ContentPlaceHolder1$Premiums_Link', '');
    }
    function Refresh_Financials() {
        __doPostBack('ctl00$ContentPlaceHolder1$Financials_Link', '');
    }
    function Refresh_Golf() {
        __doPostBack('ctl00$ContentPlaceHolder1$Golf_Link', '');
    }
    function val_Res_Checkout(ID, fName, lName, outDate, early) {
        var response;
        if (early == true) {
            response = confirm("This Reservation is Not Scheduled to Check Out Until " + outDate + ". \n Do you wish to Check Out ReservationID " + ID + " - " + fName + " " + lName + "?");
        }
        else {
            response = confirm("Do you wish to Check Out ReservationID " + ID + " - " + fName + " " + lName + "?");
        }
        if (response == true) {
            __doPostBack('ctl00$ContentPlaceHolder1$COLink','');
        }

    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:LinkButton runat="server" id = "lnkProspect">LinkButton</asp:LinkButton>
    <ul id = "menu">
        <li <%if  MultiView1.ActiveViewIndex = 6 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="UserFields_Link" runat="server">User Fields</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 7 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Personnel_Link" runat="server">Personnel</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 8 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Rooms_Link" runat="server">Rooms</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 9 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="VoiceStamps_Link" runat="server">Voice Stamps</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 10 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="HotelAccom_Link" runat="server">Hotel Accom</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 11 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="SpecialNeeds_Link" runat="server">Special Needs</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 14 Then : Response.Write("class=""current""") : End If %>><asp:LinkButton ID="Golf_Link" runat="server">Golf</asp:LinkButton></li>

   </ul>
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Reservation_Link" runat="server">Reservation</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Tours_Link" runat="server">Tours</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Financials_Link" runat="server">Financials</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 3 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Notes_Link" runat="server">Notes</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 4 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events_Link" runat="server">Events</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 5 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="UploadedDocs_Link" runat="server">Uploaded Docs</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 13 Then : Response.Write("class=""current""") : End If %>><asp:LinkButton ID="Premiums_Link" runat="server">Gifts</asp:LinkButton></li>
    </ul>
    
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="Res_View" runat="server">
                    <table>
                        <tr>
                            <td>ReservationID:</td>
                            <td><asp:TextBox ID="txtReservationID" runat="server" ReadOnly></asp:TextBox></td>
                            <td>Status:</td>
                            <td>
                                <uc2:Select_Item ID="siStatus" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Reservation Number:</td>
                            <td><asp:TextBox ID="txtReservationNumber" runat="server"></asp:TextBox></td>
                            <td>Status Date:</td>
                            <td><asp:textBox id = "txtStatusDate" runat="server" readonly></asp:textBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Location:</td>
                            <td><uc2:Select_Item ID="siLocation" runat="server" /></td>
                            <td>Type:</td>
                            <td><uc2:Select_Item ID="siResType" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Check-In:</td>
                            <td><div style="display:none;"><asp:TextBox runat="server" id = "txtCheckInDate" readonly></asp:TextBox>
                                <asp:Button runat="server" Text="..." onclick="Unnamed1_Click1"></asp:Button><asp:Calendar runat="server" id = "Calendar1" visible = "false"></asp:Calendar>
                                </div>
                                <uc10:SyncDateField ID="SyncDateField1" runat="server" />
                            </td>
                            <td>Sub-Type:</td>
                            <td><uc2:Select_Item ID="siResSubType" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Check-Out:</td>
                            <td><asp:TextBox runat="server" id = "txtCheckOutDate" readonly></asp:TextBox></td>
                            <td>#Adults:</td>
                            <td>
                                <asp:DropDownList ID="ddAdults" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Total Nights:</td>
                            <td><asp:DropDownList ID="ddNights" runat="server" autopostback = "true" onSelectedIndexChanged = "ddNights_SelectedIndexChanged"></asp:DropDownList></td>
                            <td>#Children:</td>
                            <td>
                                <asp:DropDownList ID="ddChildren" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Date Booked:</td>
                            <td><uc1:DateField ID="dteBookedDate" runat="server" /></td>
                            <td>Source:</td>
                            <td><uc2:Select_Item ID="siResSource" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Lock Inventory:</td>
                            <td><asp:CheckBox ID="ckLockedInventory" runat="server" /></td>
                            <td>Usage(s):</td>
                            <td><asp:Label runat="server" id = "lblUsages"></asp:Label>
                                <asp:LinkButton ID="lnkUsages" runat="server"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>Resort Company:</td>
                            <td><uc2:Select_Item ID="siCompany" runat="server" /></td>
                        </tr>
                    </table>
                    <asp:TextBox ID="prospectID" Visible="False" runat="server"></asp:TextBox>
                    <asp:TextBox ID="packageIssuedID" Visible="False" runat="server"></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" Text="Save" /><asp:Button ID="CIButton" runat="server" Text="Check-In" Visible="False"/><asp:Button id="COButton" runat="server" Text="Check-Out" visible = "false" autopostback = "false"></asp:Button><asp:Button id = "extStayBtn" runat="server" Text="Extend Stay" visible = "false"></asp:Button><asp:Button id = "intCIBtn" runat="server" Text="Interface Check-In" visible = "false"></asp:Button>
                    <asp:Button runat="server" Text="Print Letter" id = "btnPrintLtr"></asp:Button>
                    <asp:Button runat="server" Text="Email Letter" id = "btnEmailLtr"></asp:Button>
                    <asp:Button runat="server" Text = "Print Reg Card" ID = "btnRegCard"  />
                    <asp:Button runat="server" Text = "Email Rental Letter" ID = "btnEmailRental"  />
                    <asp:Button runat="server" Text = "Print Rental Letter" ID = "btnPrintRental"  />
                    <asp:LinkButton ID="lbSaveRes" runat="server"></asp:LinkButton>

                    <asp:Label ID="lblError" runat="server"></asp:Label>
                </asp:View>
                <asp:View ID="Tours_View" runat="server">
                    <asp:GridView ID="gvTours" runat="server" EmptyDataText = "No Tours" AutoGenerateSelectButton="True" onRowDataBound = "gvTours_RowDataBound">
                    </asp:GridView>
                    <asp:Button ID="Button2" runat="server" Text="Add Tour" />
                </asp:View>
                <asp:View ID="Financials_View" runat="server">
                    <uc4:Financials ID="Financials1" runat="server" KeyField="ReservationID" />
                </asp:View>
                <asp:View ID="Notes_View" runat="server">
                    <uc5:Notes ID="Notes1" runat="server" KeyField="ReservationID" />
                </asp:View>
                <asp:View ID="Events_View" runat="server">
                    <asp:GridView ID="gvEvents" runat="server"  EmptyDataText = "No Records" AutoGenerateSelectButton = "true">
                    </asp:GridView>
                </asp:View>
                <asp:View ID="UploadedDocs_View" runat="server">
                    <uc9:UploadedDocs ID="UploadedDocs1" runat="server" />
                </asp:View>
                <asp:View ID="UserFields_View" runat="server">
                    <uc3:UserFields ID="UF" runat="server" />
                </asp:View>   
                <asp:View ID="Personnel_View" runat="server">

                    <uc6:PersonnelTrans ID="PersonnelTrans1" runat="server" />

                </asp:View>
                <asp:View ID="Rooms_View" runat="server">
                    <asp:GridView id = "gvRoomList1" runat="server" EnableModelValidation="True" Autogeneratecolumns = "False" emptyDataText = "No Records" OnRowDataBound = "gvRoomList1_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="RoomID" DataField="RoomID"></asp:BoundField>
                            <asp:BoundField HeaderText="Room" DataField="RoomNumber"></asp:BoundField>
                            <asp:BoundField HeaderText="Type" DataField="Type"></asp:BoundField>
                            <asp:BoundField HeaderText="In Date" DataField="In Date"></asp:BoundField>
                            <asp:BoundField HeaderText="Out Date" DataField="Out Date"></asp:BoundField>
                            <asp:BoundField HeaderText="Removable" DataField="Removable"></asp:BoundField>
                            <asp:ButtonField CommandName="RemoveRoom" Text="Remove"></asp:ButtonField>
                            <asp:BoundField HeaderText="Swappable" DataField="Swappable"></asp:BoundField>
                            <asp:ButtonField CommandName="SwapRoom" Text="Swap Room"></asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                    <asp:Button ID="Button3" runat="server" Text="Add Room" />
                    <br />
                    <asp:Label ID="lblRoomError" runat="server" Text=""></asp:Label>
                </asp:View>
                <asp:View ID="VS_View" runat="server">
                    <uc7:VoiceStamps ID="VoiceStamps1" runat="server" KeyField = "ReservationID" />
                </asp:View>
                <asp:View ID="Hotel_View" runat="server">
                    <asp:GridView ID="gvAccom" runat="server"  EmptyDataText = "No Records" AutoGenerateSelectButton = "true"  onRowDataBound = "gvAccom_RowDataBound">
                    </asp:GridView>
                    <asp:Button runat="server" Text="Add" onclick="Unnamed1_Click"></asp:Button>
                </asp:View>
                <asp:View ID="SpecialNeeds_View" runat="server">
                    <uc8:SpecialNeeds ID="SpecialNeeds1" runat="server" KeyField = "ReservationID" />
                </asp:View>
                <asp:View ID="DENIED" runat="server">
                    ACCESS DENIED
                </asp:View>
                <asp:View ID="Premiums_View" runat="server">
                    <uc1:Premiums runat="server" ID="Premiums" />
                </asp:View>
                <asp:View ID="Golf_View" runat="server">
                    <asp:GridView ID="gvGolf" runat="server" EmptyDataText = "No Tee Times" AutoGenerateSelectButton="True" OnRowDataBound="gvGolf_RowDataBound">
                    </asp:GridView>
                    <ul id="menu">
                        <li>
                    <asp:LinkButton ID="btnAddGolf" runat="server" Text="Add Tee Time" /></li></ul>
                </asp:View>
            </asp:MultiView>
    </asp:Content>


