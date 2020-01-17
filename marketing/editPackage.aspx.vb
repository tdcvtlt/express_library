Imports System.Data.SqlClient
Imports System.Data
Imports System.IO

Partial Class marketing_editPackage
    Inherits System.Web.UI.Page
    Dim oPackage As New clsPackageIssued
    Dim oPkg As New clsPackage

    Protected Sub Package_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Package_Link.Click
        If txtPackageID.Text >= 0 Then
            MultiView1.ActiveViewIndex = 5
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not (IsPostBack) Then
            If CheckSecurity("TourPackages", "View", , , Session("UserDBID")) Then
                '*** Create view events *** '
                If IsNumeric(Request("PackageIssuedID")) Then
                    If CInt(Request("PackageIssuedID")) > 0 Then
                        Dim oE As New clsEvents
                        Dim sErr As String = ""

                        If Not (oE.Find_View_Event("PackageIssuedID", Request("PackageIssuedID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                            If sErr <> "" Then Response.Write(sErr)
                            If Not (oE.Create_View_Event("PackageIssuedID", Request("PackageIssuedID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                                If sErr <> "" Then Response.Write(sErr)
                            End If
                        End If
                        oE = Nothing
                    End If
                End If

                ddVendors.DataSource = (New clsVendor).List_Active_Vendors
                ddVendors.DataValueField = "VendorID"
                ddVendors.DataTextField = "Vendor"
                ddVendors.DataBind()

                '*** End View Events *** '
                MultiView1.ActiveViewIndex = 5
                Load_SIs()
                oPackage.PackageIssuedID = IIf(IsNumeric(Request("PackageIssuedID")), CInt(Request("PackageIssuedID")), 0)
                oPackage.ProspectID = Request("ProspectID")
                oPackage.Load()
                ddVendors.SelectedValue = oPackage.VendorID
                Dim oPros As New clsProspect
                oPros.Prospect_ID = IIf(IsNumeric(Request("ProspectID")), Request("ProspectID"), oPackage.ProspectID)
                oPros.Load()
                lbProspect.Text = oPros.Last_Name & ", " & oPros.First_Name '& " .. " & oPros.Prospect_ID & " .. " & oContract.ProspectID
                oPros = Nothing
                If Session("Vendors") <> "0" And Request("PackageIssuedID") > 0 Then
                    Dim oVendor As New clsPackage
                    If Not (oVendor.Validate_Vendor(oPackage.PackageID, Session("Vendors"))) Then
                        MultiView1.ActiveViewIndex = 11
                        txtPackageID.Text = -1
                        Exit Sub
                    End If
                End If
                Set_Values()
                oPkg.Fill_DD(ddPackage, oPackage.PackageID)
                Change_Index()
                LblPackageError.Text = oPackage.Error_Message
                If Request("PackageIssuedID") = 0 Then
                    lbEmail.Visible = False
                End If
            Else
                MultiView1.ActiveViewIndex = 11
                txtPackageID.Text = -1
            End If
        End If

        btnTransfer.Style.Add("display", "none")
        tbxProspectID.Style.Add("display", "none")
    End Sub

    Private Sub Load_SIs()
        siStatus.Connection_String = Resources.Resource.cns
        siStatus.ComboItem = "PackageStatus"
        siStatus.Label_Caption = ""
        siStatus.Load_Items()
    End Sub

    Private Sub Set_Values()
        txtPackageID.Text = oPackage.PackageIssuedID
        prospectID.Text = oPackage.ProspectID
        txtCost.Text = oPackage.Cost
        txtStatusDate.Text = oPackage.StatusDate
        siStatus.Selected_ID = oPackage.StatusID
        If oPackage.ExpirationDate <> "" Then dteExpirationDate.Selected_Date = oPackage.ExpirationDate
        If oPackage.PurchaseDate <> "" Then dtePurchaseDate.Selected_Date = oPackage.PurchaseDate
    End Sub

    Protected Sub UserFields_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UserFields_Link.Click
        If txtPackageID.Text > 0 Then
            MultiView1.ActiveViewIndex = 2
            UF.KeyField = "PackageIssued"
            UF.KeyValue = CInt(txtPackageID.Text)
            UF.Load_List()
        End If
    End Sub
    Private Sub Change_Index()
        For i = 0 To ddPackage.Items.Count - 1
            If ddPackage.Items(i).Value = oPackage.PackageID Then
                ddPackage.SelectedIndex = i
                Exit For
            End If
        Next
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtPackageID.Text = 0 Then
            If CheckSecurity("TourPackages", "Add", , , Session("UserDBID")) Then
                Update_Values()
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('You Do Not Have Permission to Create A Package.');", True)
            End If
        ElseIf txtPackageID.Text > 0 Then
            If CheckSecurity("TourPackages", "Edit", , , Session("UserDBID")) Then
                Update_Values()
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('You Do Not Have Permission to Edit A Package');", True)
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Access Denied.');", True)
        End If
    End Sub

    Private Sub Update_Values()
        oPackage.PackageIssuedID = txtPackageID.Text
        oPackage.Load()
        oPackage.PackageID = CInt(ddPackage.SelectedValue)
        oPackage.StatusID = siStatus.Selected_ID
        oPackage.PurchaseDate = dtePurchaseDate.Selected_date
        oPackage.ExpirationDate = dteExpirationDate.Selected_date
        oPackage.Cost = txtCost.Text
        oPackage.ProspectID = prospectID.Text
        oPackage.UserID = Session("UserDBID")
        oPackage.VendorID = ddVendors.SelectedValue
        oPackage.Save()
        Set_Values()
        LblPackageError.Text = oPackage.Error_Message
        txtPackageID.Text = oPackage.PackageIssuedID
    End Sub

    Protected Sub Events_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events_Link.Click
        If txtPackageID.Text > 0 Then
            MultiView1.ActiveViewIndex = 3
            Events1.KeyField = "PackageIssuedID"
            Events1.KeyValue = txtPackageID.Text
            Events1.List()
        End If
    End Sub

    Protected Sub Notes_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Notes_Link.Click
        If txtPackageID.Text > 0 Then
            MultiView1.ActiveViewIndex = 1
            Notes1.KeyValue = txtPackageID.Text
            Notes1.Display()
        End If
    End Sub

    Protected Sub Reservations_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Reservations_Link.Click
        If txtPackageID.Text > 0 Then
            MultiView1.ActiveViewIndex = 6
            Dim res As New clsReservations
            Dim dr As SqlDataSource
            dr = res.List(0, "PackageIssuedID", txtPackageID.Text)
            gvReservations.DataSource = dr
            gvReservations.DataBind()
        End If
    End Sub

    Protected Sub Tours_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Tours_Link.Click
        If txtPackageID.Text > 0 Then
            MultiView1.ActiveViewIndex = 7
            Dim tours As New clsTour
            Dim dr As SqlDataSource
            dr = tours.List(0, "PackageIssuedID", txtPackageID.Text)
            gvTours.DataSource = dr
            gvTours.DataBind()
        End If
    End Sub

    Protected Sub gvTours_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTours.SelectedIndexChanged
        Dim row As GridViewRow = gvTours.SelectedRow
        Response.Redirect("editTour.aspx?tourid=" & row.Cells(1).Text)
    End Sub

    Protected Sub gvReservations_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvReservations.SelectedIndexChanged
        Dim row As GridViewRow = gvReservations.SelectedRow
        Response.Redirect("editReservation.aspx?reservationid=" & row.Cells(1).Text)
    End Sub

    Protected Sub Personnel_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Personnel_Link.Click
        If txtPackageID.Text > 0 Then
            MultiView1.ActiveViewIndex = 0
            PersonnelTrans1.KeyValue = txtPackageID.Text
            PersonnelTrans1.Load_Trans()
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Response.Redirect("editReservation.aspx?reservationid=0&packageissuedid=" & txtPackageID.Text)
    End Sub

    Protected Sub Financials_Link_Click(sender As Object, e As System.EventArgs) Handles Financials_Link.Click
        If txtPackageID.Text > 0 Then
            'Please Set the prospectid
            Dim oPkg As New clsPackageIssued
            oPkg.PackageIssuedID = txtPackageID.Text
            oPkg.Load()
            Financials1.ProspectID = oPkg.ProspectID
            Financials1.KeyValue = txtPackageID.Text
            Financials1.KeyField = "PackageIssuedID"
            Financials1.View = "packageissued"
            Financials1.Display()
            MultiView1.ActiveViewIndex = 8
            oPkg = Nothing
        End If
    End Sub

    Protected Sub VoiceStamps_Link_Click(sender As Object, e As System.EventArgs) Handles VoiceStamps_Link.Click
        If txtPackageID.Text > 0 Then
            VoiceStamps1.KeyValue = txtPackageID.Text
            VoiceStamps1.Display()
            MultiView1.ActiveViewIndex = 4
        End If
    End Sub

    Protected Sub Premiums_Link_Click(sender As Object, e As System.EventArgs) Handles Premiums_Link.Click
        If txtPackageID.Text > 0 Then
            MultiView1.ActiveViewIndex = 9
            Premiums1.KeyField = "PackageIssuedID"
            Premiums1.KeyValue = txtPackageID.Text
            Premiums1.Display()
        End If
    End Sub

    Protected Sub lbProspect_Click(sender As Object, e As System.EventArgs) Handles lbProspect.Click
        If txtPackageID.Text > 0 Then
            Dim opkgIss As New clsPackageIssued
            opkgIss.PackageIssuedID = txtPackageID.Text
            opkgIss.Load()
            Response.Redirect(Request.ApplicationPath & "/marketing/editprospect.aspx?prospectid=" & opkgIss.ProspectID) ')
            opkgIss = Nothing
        Else
            Response.Redirect(Request.ApplicationPath & "/marketing/editprospect.aspx?prospectid=" & Request("ProspectID")) ')
        End If
    End Sub

    Protected Sub lbEmail_Click(sender As Object, e As System.EventArgs) Handles lbEmail.Click
        Dim oPkG As New clsPackageIssued
        Dim oPackage As New clsPackage
        Dim package As String = ""
        Dim email As String = ""
        oPkG.PackageIssuedID = txtPackageID.Text
        oPkG.Load()
        package = oPackage.Get_Pkg_Name(oPkG.PackageID)
        'If package.ToUpper <> "CZAR-W" And package.ToUpper <> "CZAR2" And package.ToUpper <> "CZAR-LV" And package.ToUpper <> "CZAR-O" And package.ToUpper <> "CZAR-M" And package.ToUpper <> "CZAR-WB" And package.ToUpper <> "CZAR-WB3" And package.ToUpper <> "CZAR-WG" And package.ToUpper <> "CZAR-WG3" And package.ToUpper <> "CZAR-WM" And package.ToUpper <> "CZAR-WM3" And package.ToUpper <> "CZAR-WMB" And package.ToUpper <> "CZAR-WMB3" And package.ToUpper <> "CZAR-WLV" And package.ToUpper <> "CZAR-WLV3" And package.ToUpper <> "CZAR-WO" And package.ToUpper <> "CZAR-WO3" And package.ToUpper <> "CZAR-GOLF" And package.ToUpper <> "CZAR-GCB" And package.ToUpper <> "CZAR-GCTN" And package.ToUpper <> "CZAR-RQWB" And package.ToUpper <> "CZAR-RQWB3" And package.ToUpper <> "CZAR-RQWG" And package.ToUpper <> "CZAR-RQWG3" And package.ToUpper <> "CZAR-RQWLV" And package.ToUpper <> "CZAR-RQWLV3" And package.ToUpper <> "CZAR-RQWM" And package.ToUpper <> "CZAR-RQWM3" And package.ToUpper <> "CZAR-RQWMB" And package.ToUpper <> "CZAR-RQWMB3" And package.ToUpper <> "CZAR-RQWO" And package.ToUpper <> "CZAR-RQWO3" Then
        'ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('This Package Does Not Have a Confirmation Letter.');", True)
        'Else
        Dim oPEmail As New clsEmail
        email = oPEmail.Get_Primary_Email(oPkG.ProspectID)
        If email <> "" Then
            Dim oPkg2Letter As New clsPackage2Letter
            Dim dt As New DataTable
            dt = oPkg2Letter.Get_Letter_Content(oPkG.PackageID)
            If dt.Rows.Count > 0 Then
                Dim body As String = ""
                Dim accom As String = ""
                Dim subj As String = ""
                Dim oPros As New clsProspect
                Dim oAdd As New clsAddress
                Dim ocombo As New clsComboItems
                Dim oPkgLetter As New clsPackageLetters
                Dim oRes As New clsReservations

                oPros.Prospect_ID = oPkG.ProspectID
                oPros.Load()
                oAdd.AddressID = oAdd.Get_Address_ID(oPros.Prospect_ID)
                oAdd.Load()
                oRes.ReservationID = oRes.Get_ResID_ByPkg(oPkG.PackageIssuedID)
                oRes.Load()
                oPackage.PackageID = oPkG.PackageID
                oPackage.Load()
                For i = 0 To dt.Rows.Count - 1
                    body = "<html><body>" & Server.HtmlDecode(dt.Rows(i).Item("Content"))
                    body = Replace(body, "<DATE>", DateTime.Now.ToShortDateString)
                    body = Replace(body, "<NAME>", oPros.First_Name & " " & oPros.Last_Name)
                    body = Replace(body, "<ADDRESS>", oAdd.Address1)
                    body = Replace(body, "<CITY>", oAdd.City)
                    body = Replace(body, "<STATE>", ocombo.Lookup_ComboItem(oAdd.StateID))
                    body = Replace(body, "<COUNTRY>", ocombo.Lookup_ComboItem(oAdd.CountryID))
                    body = Replace(body, "<ZIP>", oAdd.PostalCode)
                    body = Replace(body, "<SALEAMOUNT>", oPkG.Cost)
                    body = Replace(body, "<PACKAGEID>", oPkG.PackageIssuedID)
                    body = Replace(body, "<EXPDATE>", oPkG.ExpirationDate)
                    body = Replace(body, "<COST>", FormatCurrency(oPkG.Cost, 2))
                    If IsDBNull(oPackage.Bedrooms) Then
                        body = Replace(body, "<UNITSIZE>", "N/A")
                    Else
                        body = Replace(body, "<UNITSIZE>", Left(oPackage.Bedrooms, 1) & "BD")
                    End If
                    If oRes.ReservationID > 0 Then
                        body = Replace(body, "<SOURCE>", ocombo.Lookup_ComboItem(oRes.SourceID))
                        body = Replace(body, "<RESID>", oRes.ReservationID)
                        body = Replace(body, "<RESLOCATION>", ocombo.Lookup_ComboItem(oRes.ResLocationID))
                        If IsDBNull(oRes.CheckInDate) Or oRes.CheckInDate.ToString = "" Then
                            body = Replace(body, "<CHECKIN>", "N/A")
                            body = Replace(body, "<CHECKOUT>", "N/A")
                            body = Replace(body, "<DAYS>", "N/A")
                            body = Replace(body, "<NIGHTS>", "N/A")
                        Else
                            body = Replace(body, "<CHECKIN>", CDate(oRes.CheckInDate).ToShortDateString)
                            body = Replace(body, "<CHECKOUT>", CDate(oRes.CheckOutDate).ToShortDateString)
                            body = Replace(body, "<DAYS>", DateDiff(DateInterval.Day, CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate)) + 1)
                            body = Replace(body, "<NIGHTS>", DateDiff(DateInterval.Day, CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate)))
                        End If
                    End If


                    If IsDBNull(oPkG.PurchaseDate) Then
                        body = Replace(body, "<SALEDATE>", System.DateTime.Now.ToShortDateString)
                    ElseIf oPkG.PurchaseDate = "" Then
                        body = Replace(body, "<SALEDATE>", System.DateTime.Now.ToShortDateString)
                    Else
                        body = Replace(body, "<SALEDATE>", CDate(oPkG.PurchaseDate).ToShortDateString)
                    End If
                    'body = Replace(body, "<SALEDATE>", IIf((oPkG.PurchaseDate = "" Or IsDBNull(oPkG.PurchaseDate)), System.DateTime.Now.ToShortDateString, CDate(oPkG.PurchaseDate).ToShortDateString))
                    body = Replace(body, "<EMAIL>", email)
                    If (ddPackage.SelectedItem.Text.ToUpper.Contains("CZAR") Or ddPackage.SelectedItem.Text.ToUpper.Contains("CALI") Or ddPackage.SelectedItem.Text.ToUpper.Contains("DAYTONA") Or ddPackage.SelectedItem.Text.ToUpper.Contains("FLO") Or ddPackage.SelectedItem.Text.ToUpper.Contains("ORL") Or ddPackage.SelectedItem.Text.ToUpper.Contains("MHM") Or ddPackage.SelectedItem.Text.ToUpper.Contains("TSS") Or ddPackage.SelectedItem.Text.ToUpper.Contains("VST")) And ocombo.Lookup_ComboItem(oAdd.StateID).ToUpper = "OH" Then
                        body = body & "<div>The following link is the <a href=""http://www.vrcvacations.com/cancellation.aspx"">Ohio Written Notice Of Cancellation Rights</a>. Please read the notice. If you wish to cancel this transaction print out and sign two copies, keep one for your records and return the other to Vacation Reservation Center at the address listed in the notice.</div>"
                    End If
                    If ocombo.Lookup_ComboItem(oAdd.StateID).ToUpper = "FL" Then
                        body = body & "<div>VACATION RESERVATION CENTER, LLC is registered with the State of Florida as a Seller of Travel. Registration No. ST36388.</div>"
                    End If
                    body = body & "</body></html>"
                    Send_Mail(email, "Reservations@vrcvacations.com", dt.Rows(i).Item("Subject"), body, True)
                    Dim oNote As New clsNotes
                    oNote.NoteID = 0
                    oNote.Load()
                    oNote.KeyField = "PackageIssuedID"
                    oNote.KeyValue = oPkG.PackageIssuedID
                    oNote.CreatedByID = Session("UserDBID")
                    oNote.UserID = Session("UserDBID")
                    oNote.DateCreated = System.DateTime.Now
                    oNote.Note = "Package Confirmation Email Sent."
                    oNote.Save()
                    oNote = Nothing
                Next
                oPros = Nothing
                oAdd = Nothing
                ocombo = Nothing
                oPkgLetter = Nothing
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Email Sent.');", True)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('This Package Does Not Have a Confirmation Letter.');", True)
            End If
            oPkg2Letter = Nothing

            'Dim body As String = ""
            'Dim accom As String = ""
            'Dim subj As String = ""
            'Dim oPros As New clsProspect
            'Dim oAdd As New clsAddress
            'Dim ocombo As New clsComboItems
            'oPros.Prospect_ID = oPkG.ProspectID
            'oPros.Load()
            'oAdd.AddressID = oAdd.Get_Address_ID(oPros.Prospect_ID)
            'oAdd.Load()

            'body = "<html><body>"
            'body = body & "<img src = " & Chr(10) & "http://vendors.kingscreekplantation.com/vendors/test.net/images/vrc.bmp" & Chr(10) & "><br><br><br><br>"
            'body = body & "<table width=614>"
            'body = body & "<tr><td  width = 307 valign = top><p><span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">" & oPros.First_Name & " " & oPros.Last_Name & "</span><br>"
            'body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">" & oAdd.Address1 & "</span><br>"
            'body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">" & oAdd.City & ", " & ocombo.Lookup_ComboItem(oAdd.StateID) & " " & oAdd.PostalCode & "</span><br>"
            'body = body & "</td>"
            'If package.ToUpper = "CZAR-LV" Then
            '    body = body & "<td width = 307 valign = top><span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Destination: Las Vegas, NV</span><br>"
            'ElseIf package.ToUpper = "CZAR-O" Then
            '    body = body & "<td width = 307 valign = top><span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Destination: Orlando, FL</span><br>"
            'ElseIf package.ToUpper = "CZAR-W" Or package.ToUpper = "CZAR2" Then
            '    body = body & "<td width = 307 valign = top><span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Destination: Williamsburg, VA</span><br>"
            'ElseIf package.ToUpper = "CZAR-M" Then
            '    body = body & "<td width = 307 valign = top><span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">2 Destination Getaway</span><br>"
            'ElseIf package.ToUpper = "CZAR-GCB" Or package.ToUpper = "CZAR-GCTN" Or package.ToUpper = "CZAR-FAB" Or package.ToUpper = "CZAR-FAB3" Or package.ToUpper = "CZAR-FC" Or package.ToUpper = "CZAR-FC3" Or package.ToUpper = "CZAR-FMB" Or package.ToUpper = "CZAR-FMB3" Or package.ToUpper = "CZAR-FB" Or package.ToUpper = "CZAR-FB3" Then
            '    body = body & "<td width = 307 valign = top><span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Vacation Getaway</span><br>"
            'Else
            '    body = body & "<td width = 307 valign = top>"
            'End If
            'body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Sale Amount: " & FormatCurrency(oPkG.Cost, 2) & "</span><br>"
            'body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Account#: " & oPkG.PackageIssuedID & "</span><br>"
            'body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Sale Date: " & CDate(oPkG.PurchaseDate).ToShortDateString & "</span><br>"
            'body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Email: " & email & "</span><br>"
            'body = body & "</td></tr></table>"
            'body = body & "<br><br>"
            'body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Dear " & oPros.First_Name & " " & oPros.Last_Name & "</span><br>"
            'body = body & "<br>"
            'body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">"
            'If package.ToUpper = "CZAR-LV" Then
            '    accom = "Hotel"
            '    subj = "VRC Vacations – Las Vegas Package Confirmation"
            '    body = body & "Congratulations on purchasing your exciting vacation to Las Vegas, Nevada! Are you packed yet? We’re not kidding. We know you’re anxious to experience the charms of Las Vegas. That’s why our Travel Specialists are standing by.  Simply call 866-651-0261 from Monday thru Friday 9 – 8 and Saturday 9 – 1 E.S.T. to reserve your vacation."
            'ElseIf package.ToUpper = "CZAR-O" Then
            '    accom = "Hotel"
            '    subj = "VRC Vacations – Orlando Package Confirmation"
            '    body = body & "Congratulations on purchasing your exciting vacation to Orlando, Florida!  Are you packed yet? We’re not kidding. We know you’re anxious to experience the charms of Orlando. That’s why our Travel Specialists are standing by.  Simply call 866-651-0261 from Monday thru Friday 9 – 8 and Saturday 9 – 1 E.S.T. to reserve your vacation."
            'ElseIf package.ToUpper = "CZAR-W" Or package.ToUpper = "CZAR2" Then
            '    accom = "Resort"
            '    subj = "VRC Vacations – Williamsburg Package Confirmation"
            '    body = body & "Congratulations on purchasing your exciting vacation to Williamsburg, Virginia! Are you packed yet? We’re not kidding. We know you’re anxious to experience the charms of Williamsburg. That’s why our Travel Specialists are standing by.  Simply call 866-651-0261 from Monday thru Friday 9 – 8 and Saturday 9 – 1 E.S.T. to reserve your vacation."
            'ElseIf package.ToUpper = "CZAR-M" Then
            '    subj = "VRC Vacations - Multi Destination Package Confirmation"
            '    body = body & "Congratulations on purchasing your getaway with Vacation Reservation Center!  Are you packed yet?  We’re not kidding.  We know you’re anxious to get away to spend quality time with your loved ones.  That’s why our Travel Specialists are standing by to answer any questions you may have and get your reservation confirmed."
            'ElseIf package.ToUpper = "CZAR-GOLF" Then
            '    subj = "VRC Vacations - Williamsburg Golf Package Confirmation"
            '    body = body & "Congratulations on purchasing your golf getaway with Vacation Reservation Center!  Are you packed yet?  We’re not kidding.  We know you’re anxious to get away to spend quality time with your loved ones.  That’s why our Travel Specialists are standing by to answer any questions you may have and get your reservation confirmed."
            'Else
            '    If package.ToUpper = "CZAR-WB" Or package.ToUpper = "CZAR-WB3" Or package.ToUpper = "CZAR-GCB" Or package.ToUpper = "CZAR-FB" Or package.ToUpper = "CZAR-FB3" Or package.ToUpper = "CZAR-RQWB" Or package.ToUpper = "CZAR-RQWB3" Then
            '        subj = "VRC Vacations – Branson Package Confirmation"
            '    ElseIf package.ToUpper = "CZAR-FAB" Or package.ToUpper = "CZAR-FAB3" Then
            '        subj = "VRC Vacations – Atlantic Beach Package Confirmation"
            '    ElseIf package.ToUpper = "CZAR-FC" Or package.ToUpper = "CZAR-FC3" Then
            '        subj = "VRC Vacations – Charleston Package Confirmation"
            '    ElseIf package.ToUpper = "CZAR-WG" Or package.ToUpper = "CZAR-WG3" Or package.ToUpper = "CZAR-GCTN" Or package.ToUpper = "CZAR-RQWG" Or package.ToUpper = "CZAR-RQWG3" Then
            '        subj = "VRC Vacations – Gatlinburg Package Confirmation"
            '    ElseIf package.ToUpper = "CZAR-WLV" Or package.ToUpper = "CZAR-WLV3" Or package.ToUpper = "CZAR-RQWLV" Or package.ToUpper = "CZAR-RQWLV3" Then
            '        subj = "VRC Vacations – Las Vegas Package Confirmation"
            '    ElseIf package.ToUpper = "CZAR-WM" Or package.ToUpper = "CZAR-WM3" Or package.ToUpper = "CZAR-RQWM" Or package.ToUpper = "CZAR-RQWM3" Then
            '        subj = "VRC Vacations – Miami Package Confirmation"
            '    ElseIf package.ToUpper = "CZAR-WO" Or package.ToUpper = "CZAR-WO3" Or package.ToUpper = "CZAR-RQWO" Or package.ToUpper = "CZAR-RQWO3" Then
            '        subj = "VRC Vacations – Orlando Package Confirmation"
            '    ElseIf package.ToUpper = "CZAR-WMB" Or package.ToUpper = "CZAR-WMB3" Or package.ToUpper = "CZAR-FMB" Or package.ToUpper = "CZAR-FMB3" Or package.ToUpper = "CZAR-RQWMB" Or package.ToUpper = "CZAR-RQWMB3" Then
            '        subj = "VRC Vacations – Myrtle Beach Package Confirmation"
            '    End If
            '    body = body & "Congratulations on purchasing your getaway with Vacation Reservation Center!  Are you packed yet?  We’re not kidding.  We know you’re anxious to get away to spend quality time with your loved ones.  That’s why our Travel Specialists are standing by to answer any questions you may have and get your reservation confirmed."

            'End If
            'body = body & "</span><br><br>"
            'If package.ToUpper = "CZAR-M" Then
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">"
            '    body = body & "Simply call 877-557-3529 from Monday thru Friday 9am – 8pm (EST) or Saturday 9am – 1pm (EST) to reserve your getaway."
            '    body = body & "</span><br><br>"
            'ElseIf package.ToUpper <> "CZAR-LV" And package.ToUpper <> "CZAR-O" And package.ToUpper <> "CZAR-W" And package.ToUpper <> "CZAR2" Then
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">"
            '    body = body & "Simply call 866-651-0261 from Monday thru Friday 9am – 8pm (EST) or Saturday 9am – 1pm (EST) to reserve your getaway."
            '    body = body & "</span><br><br>"
            'End If
            'body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">TRAVEL TIPS:</span><br>"
            'body = body & "<span style=" & Chr(39) & "font-size:11.0pt;font-family:Symbol" & Chr(39) & ">·</span>"
            'If package.ToUpper = "CZAR-M" Then
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;We can book you now. Just call 877-557-3529 to reserve your travel dates.</span><br>"
            'ElseIf package.ToUpper = "CZAR-LV" Or package.ToUpper = "CZAR-O" Or package.ToUpper = "CZAR-W" Or package.ToUpper = "CZAR2" Then
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;We can book you now. Just call the toll free number to reserve your vacation.</span><br>"
            'Else
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;We can book you now. Just call 866-651-0261 to reserve your vacation.</span><br>"
            'End If
            'body = body & "<span style=" & Chr(39) & "font-size:11.0pt;font-family:Symbol" & Chr(39) & ">·</span>"
            'body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;You have 12 months to book your vacation and 15 months to travel. Start planning today.</span><br>"
            'body = body & "<span style=" & Chr(39) & "font-size:11.0pt;font-family:Symbol" & Chr(39) & ">·</span>"
            'If package.ToUpper = "CZAR-M" Then
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;You will receive your bonus gift after attending a 90 – 120 minute sales presentation during your first visit; you are under no obligation to purchase.  This gift is our way of saying thanks for your time and is yours to keep.</span><br>"
            'ElseIf package.ToUpper = "CZAR-LV" Or package.ToUpper = "CZAR-O" Or package.ToUpper = "CZAR-W" Or package.ToUpper = "CZAR2" Then
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;You will receive your bonus gifts after attending a 90 – 120 minute sales presentation during your visit; you are under no obligation to purchase.  These gifts are our way of saying thanks for your time and are yours to keep.</span><br>"
            'Else
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;You will receive your bonus gift after attending a 90 – 120 minute sales presentation during your visit; you are under no obligation to purchase.  This gift is our way of saying thanks for your time and is yours to keep.</span><br>"
            'End If
            'body = body & "<br>"
            'body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Thank you for choosing to vacation with Vacation Reservation Center(VRC). Your package details are below. We look forward to making your vacation dreams come true!</span></p>"
            'body = body & "<br><br>"
            'body = body & "<table>"
            'If package.ToUpper = "CZAR-M" Then
            '    body = body & "<tr><td  width = 400 valign = top>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & "><b>ORLANDO PACKAGE DETAILS</b></span><br>"
            '    body = body & "<br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3-Day/2-Night Weekend Hotel Stay</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Or 4-Day/3-Night Weekday Hotel Stay</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Plus a $50 Visa Incentive Gift Card</span></td>"
            '    body = body & "<td  width = 400 valign = top>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & "><b>BONUS GETAWAY:</b></span><br>"
            '    body = body & "<br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Your choice to any one of the 7 destinations below:</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3-Day/2-Night Weekend Hotel Stay</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">OR 4-Day/3-Night Weekday Hotel Stay*</span><br>"
            '    body = body & "<br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Destinations include: Murrells Inlet, SC; New Orleans, LA;</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Portland, ME; Branson, MO; Banner Elk, NC; Charleston, SC;</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Atlantic Beach, NC.;</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">*New Orleans stay does not include additional night for</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">weekday arrival</span></td></tr>"
            'ElseIf package.ToUpper = "CZAR-FAB" Or package.ToUpper = "CZAR-FAB3" Then
            '    body = body & "<tr><td width = 800 valign = top>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & "><b>ATLANTIC BEACH PACKAGE DETAILS</b></span><br>"
            '    body = body & "<br>"
            '    If package.ToUpper = "CZAR-FAB" Then
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3-Day/2-Night Hotel/Resort Stay</span><br>"
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Sunday - Tuesday night check in will receive additional night free</span><br>"
            '    Else
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">4-Day/3-Night Hotel/Resort Stay</span><br>"
            '    End If
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3 Night Cruise – choice of 3 exciting cruise destinations</span><br></td></tr>"
            'ElseIf package.ToUpper = "CZAR-FC" Or package.ToUpper = "CZAR-FC3" Then
            '    body = body & "<tr><td width = 800 valign = top>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & "><b>CHARLESTON PACKAGE DETAILS</b></span><br>"
            '    body = body & "<br>"
            '    If package.ToUpper = "CZAR-FC" Then
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3-Day/2-Night Hotel/Resort Stay</span><br>"
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Sunday - Tuesday night check in will receive additional night free</span><br>"
            '    Else
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">4-Day/3-Night Hotel/Resort Stay</span><br>"
            '    End If
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3 Night Cruise – choice of 3 exciting cruise destinations</span><br></td></tr>"
            'ElseIf package.ToUpper = "CZAR-WB" Or package.ToUpper = "CZAR-WB3" Or package.ToUpper = "CZAR-GCB" Or package.ToUpper = "CZAR-FB" Or package.ToUpper = "CZAR-FB3" Or package.ToUpper = "CZAR-RQWB" Or package.ToUpper = "CZAR-RQWB3" Then
            '    body = body & "<tr><td width = 800 valign = top>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & "><b>BRANSON PACKAGE DETAILS</b></span><br>"
            '    body = body & "<br>"
            '    If package.ToUpper = "CZAR-WB" Or package.ToUpper = "CZAR-FB" Or package.ToUpper = "CZAR-RQWB" Then
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3-Day/2-Night Hotel/Resort Stay</span><br>"
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Sunday - Tuesday night check in will receive additional night free</span><br>"
            '        If package.ToUpper = "CZAR-GCB" Then
            '            body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">$50 Visa</span><br>"
            '        End If
            '    ElseIf package.ToUpper = "CZAR-FB3" Then
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">4-Day/3-Night Hotel/Resort Stay</span><br>"
            '    Else
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">4-Day/3-Night Weekend Hotel/Resort Stay</span><br>"
            '    End If
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3 Night Cruise – choice of 3 exciting cruise destinations</span><br></td></tr>"
            'ElseIf package.ToUpper = "CZAR-WG" Or package.ToUpper = "CZAR-WG3" Or package.ToUpper = "CZAR-GCTN" Or package.ToUpper = "CZAR-RQWG" Or package.ToUpper = "CZAR-RQWG3" Then
            '    body = body & "<tr><td width = 800 valign = top>"
            '    If package.ToUpper = "CZAR-GCTN" Then
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & "><b>SMOKEY MOUNTAIN PACKAGE DETAILS</b></span><br>"
            '    Else
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & "><b>GATLINBURG PACKAGE DETAILS</b></span><br>"
            '    End If
            '    body = body & "<br>"
            '    If package.ToUpper = "CZAR-WG" Or package.ToUpper = "CZAR-RQWG3" Then
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3-Day/2-Night Hotel/Resort Stay</span><br>"
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Sunday - Tuesday night check in will receive additional night free</span><br>"
            '        If package.ToUpper = "CZAR-GCTN" Then
            '            body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">$50 Visa</span><br>"
            '        End If
            '    Else
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">4-Day/3-Night Weekend Hotel/Resort Stay</span><br>"
            '    End If
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3 Night Cruise – choice of 3 exciting cruise destinations</span><br></td></tr>"
            'ElseIf package.ToUpper = "CZAR-WLV" Or package.ToUpper = "CZAR-WLV3" Or package.ToUpper = "CZAR-RQWLV" Or package.ToUpper = "CZAR-RQWLV3" Then
            '    body = body & "<tr><td width = 800 valign = top>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & "><b>LAS VEGAS PACKAGE DETAILS</b></span><br>"
            '    body = body & "<br>"
            '    If package.ToUpper = "CZAR-WLV" Or package.ToUpper = "CZAR-RQWLV" Then
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3-Day/2-Night Hotel/Resort Stay</span><br>"
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Sunday - Tuesday night check in will receive additional night free</span><br>"
            '    Else
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">4-Day/3-Night Weekend Hotel/Resort Stay</span><br>"
            '    End If
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3 Night Cruise – choice of 3 exciting cruise destinations</span><br></td></tr>"
            'ElseIf package.ToUpper = "CZAR-WM" Or package.ToUpper = "CZAR-WM3" Or package.ToUpper = "CZAR-RQWM" Or package.ToUpper = "CZAR-RQWM3" Then
            '    body = body & "<tr><td width = 800 valign = top>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & "><b>MIAMI PACKAGE DETAILS</b></span><br>"
            '    body = body & "<br>"
            '    If package.ToUpper = "CZAR-WM" Or package.ToUpper = "CZAR-RQWM" Then
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3-Day/2-Night Hotel/Resort Stay</span><br>"
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Sunday - Tuesday night check in will receive additional night free</span><br>"
            '    Else
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">4-Day/3-Night Weekend Hotel/Resort Stay</span><br>"
            '    End If
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3 Night Cruise – choice of 3 exciting cruise destinations</span><br></td></tr>"
            'ElseIf package.ToUpper = "CZAR-WMB" Or package.ToUpper = "CZAR-WMB3" Or package.ToUpper = "CZAR-FMB" Or package.ToUpper = "CZAR-FMB3" Or package.ToUpper = "CZAR-RQWMB" Or package.ToUpper = "CZAR-RQWMB3" Then
            '    body = body & "<tr><td width = 800 valign = top>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & "><b>MYRTLE BEACH PACKAGE DETAILS</b></span><br>"
            '    body = body & "<br>"
            '    If package.ToUpper = "CZAR-WMB" Or package.ToUpper = "CZAR-FMB" Or package.ToUpper = "CZAR-RQWMB" Then
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3-Day/2-Night Hotel/Resort Stay</span><br>"
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Sunday - Tuesday night check in will receive additional night free</span><br>"
            '    ElseIf package.ToUpper = "CZAR-FMB3" Then
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">4-Day/3-Night Hotel/Resort Stay</span><br>"
            '    Else
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">4-Day/3-Night Weekend Hotel/Resort Stay</span><br>"
            '    End If
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3 Night Cruise – choice of 3 exciting cruise destinations</span><br></td></tr>"
            'ElseIf package.ToUpper = "CZAR-WO" Or package.ToUpper = "CZAR-WO3" Or package.ToUpper = "CZAR-RQWO" Or package.ToUpper = "CZAR-RQWO3" Then
            '    body = body & "<tr><td width = 800 valign = top>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & "><b>ORLANDO PACKAGE DETAILS</b></span><br>"
            '    body = body & "<br>"
            '    If package.ToUpper = "CZAR-WO" Or package.ToUpper = "CZAR-RQWO" Then
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3-Day/2-Night Hotel/Resort Stay</span><br>"
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Sunday - Tuesday night check in will receive additional night free</span><br>"
            '    Else
            '        body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">4-Day/3-Night Weekend Hotel/Resort Stay</span><br>"
            '    End If
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3 Night Cruise – choice of 3 exciting cruise destinations</span><br></td></tr>"
            'ElseIf package.ToUpper = "CZAR-GOLF" Then
            '    body = body & "<tr><td width = 800 valign = top>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & "><b>WILLIAMSBURG GOLF PACKAGE DETAILS</b></span><br>"
            '    body = body & "<br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">4-Day/3-Night Hotel/Resort Stay</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Your choice of 2 Attraction Tickets or Dining Certificate</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Up to 4 Rounds of Golf per day (pay cart fee only)</span><br></td></tr>"
            'Else
            '    body = body & "<tr><td  width = 400 valign = top>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">PACKAGE DETAILS</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Your Choice:</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3 - Day/2 - Night Weekend " & accom & "Stay</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Or 4 - Day/3 - Night Weekday " & accom & " Stay</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Plus a $100 Visa Incentive Gift Card</span></td>"
            '    body = body & "<td  width = 400 valign = top>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">YOUR BONUS GIFTS</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Your Choice:</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">3 Night Cruise for 2 Adults to</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">the Bahamas or Mexico</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">(you only pay taxes or port fees)</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">OR additional Land Bonus Vacation</span></td></tr>"
            'End If
            'body = body & "</table>"
            'body = body & "<br>"
            'If package.ToUpper = "CZAR-M" Then
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">We hope you enjoy your visit, and look forward to your arrival!</span><br><br><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & "><b><i>VRC Reservations Team</b></i></span><br>"
            '    body = body & "<br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:9.0pt;" & Chr(10) & ">Details of Participation</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:9.0pt;" & Chr(10) & ">This promotion is sponsored by Vacation Reservation Center, the developer/seller (" & Chr(34) & "VRC" & Chr(34) & "), in order to introduce you to vacation clubs. Everyone over the age of 28 is welcome to visit our Resort sales office and purchase; however, in order to participate in this offer you must meet these requirements. If you do not meet the requirements below you will not be eligible to receive any gifts or accommodations in connection with this offer. If you do not meet these requirements, but have received gifts or accommodations, you will be charged the verifiable retail value for such gifts and accommodations, plus all applicable taxes, less any payments made by you. In order to participate in this offer, you must be 28 years of age or older, have a gross annual income of $50,000.00 or more and be creditworthy. You must have a valid major credit card bearing a Visa, MasterCard, American Express, or Discover logo. Debit cards will not be accepted.  If you do not meet these qualifications you will not be eligible for any gifts, premiums or promotional items offered in exchange for participation in this promotion.  " & Chr(34) & "To be eligible, you must be married or cohabitating and you must travel with your spouse/cohabitant." & Chr(34) & "</span></p>"
            'ElseIf package.ToUpper = "CZAR-LV" Or package.ToUpper = "CZAR-O" Or package.ToUpper = "CZAR-W" Or package.ToUpper = "CZAR2" Then
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">Happy Travels,</span><br><br><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">VRC Reservation Team</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">866-651-0261</span><br>"
            '    body = body & "<br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:9.0pt;" & Chr(10) & ">Details of Participation</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:9.0pt;" & Chr(10) & ">This promotion is sponsored by Vacation Reservation Center, the developer/seller (" & Chr(34) & "VRC" & Chr(34) & "), in order to introduce you to vacation ownership. Everyone over the age of 25 is welcome to visit our Resort sales office and purchase; however, in order to participate in this offer you must meet these requirements. If you do not meet the requirements below you will not be eligible to receive any gifts or accommodations in connection with this offer. If you do not meet these requirements, but have received gifts or accommodations, you will be charged the verifiable retail value for such gifts and accommodations, plus all applicable taxes, less any payments made by you. In order to participate in this offer, you must be 25 years of age or older, have a gross annual income of $50,000.00 or more and be creditworthy .You must have a valid major credit card bearing a Visa, MasterCard, American Express, or Discover logo. Debit cards will not be accepted.  To be eligible, you must be a single female, or, if married or cohabitating, you must travel with your spouse/cohabitant.  If you do not meet these qualifications you will not be eligible for any gifts, premiums or promotional items offered in exchange for participation in this promotion.</span></p>"
            'ElseIf package.ToUpper = "CZAR-GCTN" Or package.ToUpper = "CZAR-GCB" Then
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">We hope you enjoy your visit, and look forward to your arrival!</span><br><br><br>"
            '    'body = body & "<img src = " & Chr(10) & "http://vendors.kingscreekplantation.com/vendors/test.net/images/kcplogo.bmp" & chr(10) & "><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & "><b><i>VRC Reservations Team</b></i></span><br>"
            '    body = body & "<br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:9.0pt;" & Chr(10) & ">Details of Participation</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:9.0pt;" & Chr(10) & ">This promotion is sponsored by Vacation Reservation Center, the developer/seller (" & Chr(34) & "VRC" & Chr(34) & "), in order to introduce you to vacation clubs. Everyone over the age of 28 is welcome to visit our Resort sales office and purchase; however, in order to participate in this offer you must meet these requirements. If you do not meet the requirements below you will not be eligible to receive any gifts or accommodations in connection with this offer. If you do not meet these requirements, but have received gifts or accommodations, you will be charged the verifiable retail value for such gifts and accommodations, plus all applicable taxes, less any payments made by you. In order to participate in this offer, you must be 28 years of age or older, have a gross annual income of $50,000.00 or more and be creditworthy. You must have a valid major credit card bearing a Visa, MasterCard, American Express, or Discover logo. Debit cards will not be accepted.  To be eligible, you must be married or cohabitating, and you must travel with your spouse/cohabitant. Single females are eligible to attend if they meet all other qualifications, but single men are not eligible to receive a promotional offer.  If you do not meet these qualifications you will not be eligible for any gifts, premiums or promotional items offered in exchange for participation in this promotion.</span></p>"
            'Else
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & ">We hope you enjoy your visit, and look forward to your arrival!</span><br><br><br>"
            '    'body = body & "<img src = " & Chr(10) & "http://vendors.kingscreekplantation.com/vendors/test.net/images/kcplogo.bmp" & chr(10) & "><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:11.0pt;" & Chr(10) & "><b><i>VRC Reservations Team</b></i></span><br>"
            '    body = body & "<br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:9.0pt;" & Chr(10) & ">Details of Participation</span><br>"
            '    body = body & "<span style=" & Chr(10) & "font-size:9.0pt;" & Chr(10) & ">This promotion is sponsored by Vacation Reservation Center, the developer/seller (" & Chr(34) & "VRC" & Chr(34) & "), in order to introduce you to vacation clubs. Everyone over the age of 28 is welcome to visit our Resort sales office and purchase; however, in order to participate in this offer you must meet these requirements. If you do not meet the requirements below you will not be eligible to receive any gifts or accommodations in connection with this offer. If you do not meet these requirements, but have received gifts or accommodations, you will be charged the verifiable retail value for such gifts and accommodations, plus all applicable taxes, less any payments made by you. In order to participate in this offer, you must be 28 years of age or older, have a gross annual income of $50,000.00 or more and be creditworthy. You must have a valid major credit card bearing a Visa, MasterCard, American Express, or Discover logo. Debit cards will not be accepted.  If you do not meet these qualifications you will not be eligible for any gifts, premiums or promotional items offered in exchange for participation in this promotion.  " & Chr(34) & "To be eligible, you must be married or cohabitating and you must travel with your spouse/cohabitant." & Chr(34) & "</span></p>"
            'End If
            'body = body & "</body></html>"
            'Send_Mail(email, "Reservations@vrcvacations.com", subj, body, True)
            'oPros = Nothing
            'oAdd = Nothing
            'ocombo = Nothing

        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('This Prospect Does Not Have A Primary Email Address.');", True)
            'End If

        End If
        oPEmail = Nothing
        oPackage = Nothing
        oPkG = Nothing
    End Sub

    Protected Sub btnTransfer_Click(sender As Object, e As EventArgs) Handles btnTransfer.Click
        Dim prospect_id = -1
        Dim returnValue = 0

        If Integer.TryParse(tbxProspectID.Text.Trim(), prospect_id) = False Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Bad Prospect ID!');", True)
            Return
        End If

        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = New SqlCommand(String.Format("select count(*) from t_prospect where prospectid={0}", prospect_id), cn)
                Try
                    cn.Open()
                    returnValue = cm.ExecuteScalar()
                Catch ex As Exception
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Critical error!');", True)
                Finally
                    cn.Close()
                End Try
            End Using
        End Using

        If returnValue = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Prospect ID was not found!');", True)
            Return
        End If

        Dim package_issued_id = CInt(Request("PackageIssuedID"))
        oPackage.PackageIssuedID = package_issued_id
        oPackage.Load()
        returnValue = oPackage.Transfer_Package(prospect_id, package_issued_id, oPackage.ProspectID, Session("UserDBID"))
        tbxProspectID.Text = ""

        If returnValue > 0 Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), DateTime.Now.ToLongTimeString(), "alert('Package Transfer was successful!');", True)
            Response.Redirect(Request.Url.ToString())
        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType(), DateTime.Now.ToLongTimeString(), "alert('Package Transfer was not successful!');", True)
        End If


    End Sub

    Protected Sub tbxAlert_Click(sender As Object, e As EventArgs) Handles tbxAlert.Click
        Page.ClientScript.RegisterStartupScript(Me.GetType(), DateTime.Now.ToLongTimeString(), "alertTransferConfirmation();", True)
    End Sub

    Protected Sub MultiView1_ActiveViewChanged(sender As Object, e As System.EventArgs) Handles MultiView1.ActiveViewChanged        
        If MultiView1.ActiveViewIndex = 5 Then
            Dim re = clsEmailTourPackage.ShouldEmail(Request("PackageIssuedID"))
            lbEmail.Visible = IIf(re = False, False, True)
        End If
    End Sub
End Class
