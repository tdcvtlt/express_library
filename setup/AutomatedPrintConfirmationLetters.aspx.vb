Imports System
Imports System.Data
Imports System.Data.SqlClient

Partial Class setup_AutomatedPrintConfirmationLetters
    Inherits System.Web.UI.Page

    Protected Sub btn_submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_submit.Click

        Dim sb = New StringBuilder()
        If lbx_To.Items.OfType(Of ListItem).Count() = 0 Then Return

        If ClientScript.IsClientScriptBlockRegistered("popupPrintWindow") = False Then

            Dim listItems() = lbx_To.Items.OfType(Of ListItem).Select(Function(x) String.Format("'{0}'", x.Text)).ToArray()

            Dim sql = String.Format( _
                "select (select top 1 Email from t_prospectEmail where prospectid = pr.prospectid and " & _
                "isprimary = 1 and isactive = 1 order by emailid desc) Email, " & _
                "(select top 1 address1 from t_prospectaddress where activeflag = 1 and prospectid = pr.prospectid  order by addressid desc) StreetAddress, " & _
                "(select top 1 city from t_prospectaddress where activeflag = 1 and prospectid = pr.prospectid  order by addressid desc) City, " & _
                "(select top 1 postalcode from t_prospectaddress where activeflag = 1 and prospectid = pr.prospectid  order by addressid desc) PostalCode, " & _
                "(select comboItem from t_comboItems where comboItemId = (select top 1 stateid from t_prospectaddress where activeflag = 1 and " & _
                "prospectid = pr.prospectid  order by addressid desc)) State, " & _
                "(select comboItem from t_comboItems where comboItemId = (select top 1 countryid from t_prospectaddress where activeflag = 1 and " & _
                "prospectid = pr.prospectid order by addressid desc)) Country, *, rl.ComboItem as ResLocation, rs.Comboitem as ResSource  " & _
                "from t_packageissued pi inner join t_package p on pi.packageid = p.packageid " & _
                "inner join t_prospect pr on pr.prospectid = pi.prospectid " & _
                "inner join t_package2letter p2l on p2l.packageid = p.packageid " & _
                "inner join t_packageLetters pl on pl.PackageLetterID = p2l.PackageLetterID " & _
                "left outer join t_Reservations r on pi.PackageIssuedID = r.PackageIssuedID " & _
                "left outer join t_ComboItems rl on r.ResLocationID = rl.ComboItemID " & _
                "left outer join t_Comboitems rs on r.SOurceID = rs.ComboItemID " & _
                "left outer join t_ComboItems ps on pi.StatusID = ps.CombOitemID " & _
                "where purchasedate between '{0}' and '{1}' and p2l.Active = 1 and ps.COmboItem not in  ('Kicked','Cancelled') " & _
                "and pi.packageid in (select packageid from t_package where active = 1) and pl.name in ({2})", _
                sdate.Selected_Date, edate.Selected_Date, String.Join(",", listItems))
            '"and pi.packageid in (select packageid from t_package where package like 'czar%' and active = 1) and pl.name in ({2})", _
            '"and pi.packageid in (select packageid from t_package where active = 1) and pl.name in ({2})", _

            Using cnn = New SqlConnection(Resources.Resource.cns)
                Using ada = New SqlDataAdapter(sql, cnn)

                    Dim dt = New DataTable()
                    ada.Fill(dt)

                    If dt.Rows.Count = 0 Then
                    Else

                        Dim html = String.Empty
                        Dim cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
                        Dim textInfo = cultureInfo.TextInfo

                        For Each dr As DataRow In dt.Rows

                            html = Server.HtmlDecode(dr("LetterContent").ToString())

                            html = html.Replace("<DATE>", DateTime.Now.ToString("MM/dd/yyyy"))


                            html = html.Replace(String.Format("<img src={0} style={1} />", """http://vendors.kingscreekplantation.com/vendors/test.net/images/vrclogo.png""", """width: 300px; height: 116px;"""),
                                                "<div style=height:116px;>&nbsp;</div>")

                            html = html.Replace(String.Format("<img src={0} style={1} />", """http://vendors.kingscreekplantation.com/vendors/test.net/images/vrclogo.png""", """width: 300px; height: 116px; cursor:default;"""),
                                                "<div style=height:116px;>&nbsp;</div>")


                            html = html.Replace("Dear <NAME>", String.Format("Dear {0} {1}", textInfo.ToTitleCase(dr("firstName").ToString()), textInfo.ToTitleCase(dr("lastName").ToString())))
                            html = html.Replace("<NAME>", String.Format("<span style=margin-left:76px;>{0} {1}</span>", textInfo.ToTitleCase(dr("firstName").ToString()), textInfo.ToTitleCase(dr("lastName").ToString())))
                            html = html.Replace("<ADDRESS>", String.Format("<span style=margin-left:76px;>{0}</span>", textInfo.ToTitleCase(dr("streetAddress").ToString())))
                            html = html.Replace("<CITY>", String.Format("<span style=margin-left:76px;>{0}</span>", textInfo.ToTitleCase(dr("City").ToString().Trim())))
                            html = html.Replace("<EXPDATE>", DateTime.Parse(dr("ExpirationDate").ToString()).ToString("MM/dd/yyyy"))
                            html = html.Replace("<STATE>", dr("State").ToString().ToUpper())
                            html = html.Replace("<ZIP>", dr("postalCode").ToString())
                            html = html.Replace("<COUNTRY>", String.Format("<span style=margin-left:76px;>{0}</span>", textInfo.ToTitleCase(dr("Country").ToString().Trim())))
                            html = html.Replace("<COST>", FormatCurrency(dr("Cost"), 2))
                            html = html.Replace("<SALEAMOUNT>", Decimal.Parse(dr("Cost").ToString()).ToString("N2"))
                            html = html.Replace("<PACKAGEID>", dr("packageIssuedID").ToString())
                            html = html.Replace("<SALEDATE>", DateTime.Parse(dr("PurchaseDate").ToString()).ToString("MM/dd/yyyy"))
                            html = html.Replace("<EMAIL>", dr("Email").ToString().Trim().ToLower())
                            html = html.Replace("Details of Participation:", "<div style=display:none;></div><div style=margin-top:96px><br/><br/><br/></div>")
                            If IsDBNull(dr("Bedrooms")) Then
                                html = html.Replace("<UNITSIZE>", "N/A")
                            Else
                                html = html.Replace("<UNITSIZE>", Left(dr("Bedrooms").ToString(), 1) & "BD")
                            End If

                            Try
                                If dr("ReservationID") > 0 Then
                                    html = html.Replace("<SOURCE>", dr("ResSource").ToString)
                                    html = html.Replace("<RESID>", dr("ReservationID"))
                                    html = html.Replace("<RESLOCATION>", dr("ResLocation").ToString())

                                    If IsDBNull(dr("CheckInDate")) Or dr("CheckInDate").ToString = "" Then
                                        html = html.Replace("<CHECKIN>", "N/A")
                                        html = html.Replace("<CHECKOUT>", "N/A")
                                        html = html.Replace("<DAYS>", "N/A")
                                        html = html.Replace("<NIGHTS>", "N/A")
                                    Else
                                        html = html.Replace("<CHECKIN>", CDate(dr("CheckInDate")).ToShortDateString)
                                        html = html.Replace("<CHECKOUT>", CDate(dr("CheckOutDate")).ToShortDateString)
                                        html = html.Replace("<DAYS>", DateDiff(DateInterval.Day, CDate(dr("CheckInDate")), CDate(dr("CheckOutDate"))) + 1)
                                        html = html.Replace("<NIGHTS>", DateDiff(DateInterval.Day, CDate(dr("CheckInDate")), CDate(dr("CheckOutDate"))))

                                    End If
                                End If

                                If dr("State").ToString().ToUpper() = "OH" Then
                                    html &= "<p style='text-align:center;'>" &
                                            "<h1>NOTICE OF CANCELLATION RIGHTS</h1></p>"
                                    html &= "<p style='text-align:left;'>"
                                    html &= "-(a) Name of Solicitor: Vacation Reservation  Center, LLC<br />"
                                    html &= "-(b) Solicitor's Certificate of Registration #20160302,  pursuant to Section 47109.03 of the Revised Code of Ohio.<br />"
                                    html &= "-(c) Please call during normal business hours (9:00 AM to 5:00 PM) @ 757-873-4005, this telephone is located at Vacation Reservation Center, LLC, 263 McLaws Circle, Suite 202,Williamsburg, VA 23185.<br />"
                                    html &= "-(d) Please see your confirmation e-mail with the itemized List of all prices or fees being charged, including, but not limited to, handling, shipping, delivery, Virginia state taxes or any other charges of any nature.<br />"
                                    html &= "-The approximate total retail value of this offer is $160.00 to $1,157.00,depending on destination selected and time of travel.<br />"
                                    html &= "-(e) Date of Transaction is noted on your confirmation e-mail.<br />"
                                    html &= "-(f) Description of Goods & Services: Vacation Reservation Center offers discounted accommodations and trips to a wide variety of vacation destinations.<br />"
                                    html &= "-That the purpose of this phone call is to make a sale of a travel package or packages to one, or more, of the available destinations; further, the odds of receiving the items offered, are one hundred (100%) percent---all persons receive all elements of the offer that they have agreed to.<br />"
                                    html &= "-Please Note that attendance at a 90-minute timeshare presentation is required.<br />"
                                    html &= "-(g) NOTICE OF CANCELLATION RIGHTS:<br />"
                                    html &= "Because you agreed to buy these goods (or services) as a result of a telephone solicitation, Ohio law gives you seven (7) days to cancel your purchase.  If you cancel we must provide you a full refund within thirty(30) days. If you want to cancel, you must sign your name below and return a copy of this Notice, together with any goods you have received, so they are post-marked no later than midnight of the seventh day following the date you received the goods or agreed to the services, or the seventh day following the date you received this notice, whichever is later. The notice and goods must be addressed as follows: Vacation Reservation Center, LLC @ 263 McLaws Circle, Suite #202, Williamsburg, VA 23185. I want to cancel my agreement to purchase.<br /><br />"
                                    html &= "_______________________________________&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" & Chr(9) & Chr(9) & "_______________________________________<br />"
                                    html &= "Signature&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp(Name of Purchaser – printed)<br /><br />"
                                    html &= "_______________________________________&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp_______________________________________<br />"
                                    html &= "(Address of Purchaser – printed)&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp(Address – city, state, zip)<br /><br />"
                                    html &= "_______________________________________<br />"
                                    html &= "(Telephone #)&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp(Date)"
                                    html &= "</p>"
                                    html &= "<div style=page-break-before:always;position:relative;></div>"
                                    html &= "<br><br><br><p style='text-align:center;'>" &
                                            "<h1>NOTICE OF CANCELLATION RIGHTS</h1></p>"
                                    html &= "<p style='text-align:left;'>"
                                    html &= "-(a) Name of Solicitor: Vacation Reservation  Center, LLC<br />"
                                    html &= "-(b) Solicitor's Certificate of Registration #20160302,  pursuant to Section 47109.03 of the Revised Code of Ohio.<br />"
                                    html &= "-(c) Please call during normal business hours (9:00 AM to 5:00 PM) @ 757-873-4005, this telephone is located at Vacation Reservation Center, LLC, 263 McLaws Circle, Suite 202,Williamsburg, VA 23185.<br />"
                                    html &= "-(d) Please see your confirmation e-mail with the itemized List of all prices or fees being charged, including, but not limited to, handling, shipping, delivery, Virginia state taxes or any other charges of any nature.<br />"
                                    html &= "-The approximate total retail value of this offer is $160.00 to $1,157.00,depending on destination selected and time of travel.<br />"
                                    html &= "-(e) Date of Transaction is noted on your confirmation e-mail.<br />"
                                    html &= "-(f) Description of Goods & Services: Vacation Reservation Center offers discounted accommodations and trips to a wide variety of vacation destinations.<br />"
                                    html &= "-That the purpose of this phone call is to make a sale of a travel package or packages to one, or more, of the available destinations; further, the odds of receiving the items offered, are one hundred (100%) percent---all persons receive all elements of the offer that they have agreed to.<br />"
                                    html &= "-Please Note that attendance at a 90-minute timeshare presentation is required.<br />"
                                    html &= "-(g) NOTICE OF CANCELLATION RIGHTS:<br />"
                                    html &= "Because you agreed to buy these goods (or services) as a result of a telephone solicitation, Ohio law gives you seven (7) days to cancel your purchase.  If you cancel we must provide you a full refund within thirty(30) days. If you want to cancel, you must sign your name below and return a copy of this Notice, together with any goods you have received, so they are post-marked no later than midnight of the seventh day following the date you received the goods or agreed to the services, or the seventh day following the date you received this notice, whichever is later. The notice and goods must be addressed as follows: Vacation Reservation Center, LLC @ 263 McLaws Circle, Suite #202, Williamsburg, VA 23185. I want to cancel my agreement to purchase.<br /><br />"
                                    html &= "_______________________________________&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" & Chr(9) & Chr(9) & "_______________________________________<br />"
                                    html &= "Signature&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp(Name of Purchaser – printed)<br /><br />"
                                    html &= "_______________________________________&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp_______________________________________<br />"
                                    html &= "(Address of Purchaser – printed)&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp(Address – city, state, zip)<br /><br />"
                                    html &= "_______________________________________<br />"
                                    html &= "(Telephone #)&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp(Date)"
                                    html &= "</p>"
                                    html &= "<div style=page-break-before:always;position:relative;></div><div>&nbsp;</div>"
                                End If

                                If dr("State").ToString().ToUpper() = "FL" Then
                                    html &= "<p>VACATION RESERVATION CENTER, LLC is registered with the State of Florida as a Seller of Travel. Registration No. ST36388.</p>"
                                End If

                                If sb.ToString() = String.Empty Then
                                    sb.AppendFormat("<div>{0}</div>", html)
                                Else
                                    sb.AppendFormat("<div style=page-break-before:always;position:relative;>{0}</div>", html)
                                End If

                                html = String.Empty

                            Catch ex As Exception
                                Throw New PackageWithoutReservationException(dr("PackageIssuedID").ToString(), dr("ProspectID").ToString())
                            End Try

                        Next

                        Response.Write("<div id = 'content' style='display:none;'>" & sb.ToString() & "</div>")

                        html = String.Format("var win=window.open();")
                        html += String.Format("win.document.write(document.getElementById('content').innerHTML);")

                        Me.ClientScript.RegisterClientScriptBlock(Me.GetType(), "popupPrintWindow", html, True)
                    End If
                End Using
            End Using
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Using cnn = New SqlConnection(Resources.Resource.cns)
                Using ada = New SqlDataAdapter("select distinct name from t_PackageLetters order by name", cnn)

                    Dim dt = New DataTable()
                    ada.Fill(dt)

                    lbx_from.DataSource = dt
                    lbx_from.DataTextField = "Name"
                    lbx_from.DataValueField = "Name"
                    lbx_from.DataBind()

                End Using
            End Using
        End If
    End Sub

    Protected Sub btn_to_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_to.Click       

        lbx_To.Items.AddRange(lbx_from.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).ToArray())

        For Each li In lbx_from.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).ToArray()
            lbx_from.Items.Remove(li)
        Next
       
        lbx_from.ClearSelection()
        lbx_To.ClearSelection()

    End Sub

    Protected Sub btn_back_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_back.Click

        lbx_from.Items.AddRange(lbx_To.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).ToArray())
        For Each li In lbx_To.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).ToArray()
            lbx_To.Items.Remove(li)
        Next

        Dim all = lbx_from.Items.OfType(Of ListItem).OrderBy(Function(x) x.Text).ToList().ToArray()

        lbx_from.Items.Clear()

        lbx_from.Items.AddRange(all)
        lbx_from.ClearSelection()
        lbx_To.ClearSelection()

    End Sub

    Private Class PackageWithoutReservationException
        Inherits Exception

        Private package_issued_id As Int32
        Private prospect_id As Int32

        Public Sub New(pi_id As Int32, p_id As Int32)
            Me.package_issued_id = pi_id
            Me.prospect_id = p_id
        End Sub

        Public Overrides ReadOnly Property Message As String
            Get
                Return String.Format("Package ID {0} belonged to Prospect {1} has no reservation. Please set it up to prevent the error.", package_issued_id, prospect_id)
            End Get
        End Property
    End Class
End Class
