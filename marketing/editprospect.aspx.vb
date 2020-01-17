Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class marketing_editprospect
    Inherits System.Web.UI.Page

    Dim oPros As New clsProspect
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If CheckSecurity("Prospects", "View", , , Session("UserDBID")) Then
                '*** Create view events *** '
                If IsNumeric(Request("ProspectID")) Then
                    If CInt(Request("ProspectID")) > 0 Then
                        Dim oE As New clsEvents
                        Dim sErr As String = ""

                        If Not (oE.Find_View_Event("ProspectID", Request("ProspectID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                            If sErr <> "" Then Response.Write(sErr)
                            If Not (oE.Create_View_Event("ProspectID", Request("ProspectID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                                If sErr <> "" Then Response.Write(sErr)
                            End If
                        End If
                        oE = Nothing
                    End If
                End If
                dteBirthDate.sDate = CInt(Year(System.DateTime.Now)) - 100
                '*** End View Events *** '
                MultiView1.ActiveViewIndex = 0
                
                oPros.Prospect_ID = IIf(IsNumeric(Request("ProspectID")), Request("ProspectID"), 0)
                oPros.Load()
                If Session("Vendors") <> "0" Then
                    Dim oPackage As New clsPackage
                    If Not (oPackage.Validate_Vendor_By_Prospect(oPros.Prospect_ID, Session("Vendors"))) Then
                        MultiView1.ActiveViewIndex = 12
                        txtProspectID.Text = -1
                        Exit Sub
                    End If
                End If
                Set_Fields()
                Load_Lookups()
                Set_Fields()
                If oPros.Prospect_ID > 0 Then
                    Dim oEmail As New clsEmail
                    oEmail.ProspectID = oPros.Prospect_ID
                    gvEmail.DataSource = oEmail.List
                    gvEmail.DataBind()
                    Dim oDNS As New clsDoNotSellList
                    If oDNS.Get_Status(oPros.Prospect_ID) = "Add" Then
                        btnDNS.Text = "Add to Do Not Sell List"
                    Else
                        btnDNS.Text = "Remove From Do Not Sell List"
                        lblDNS.Text = "**Customer is on the Do Not Sell List. Do Not Tour.**"
                    End If
                    btnDNS.Visible = True
                    oDNS = Nothing
                Else
                    btnDNS.Visible = False
                End If
                'Label6.Text = oPros.Error_Message
            Else
                MultiView1.ActiveViewIndex = 12
                txtProspectID.Text = -1
            End If
        End If

        If CheckSecurity("CreditCards", "Add", , , Session("UserDBID")) Then add_creditcard_linkbutton.Visible = True

    End Sub

    Private Sub Set_Fields()
        type.Selected_ID = oPros.TypeID
        subtype.Selected_ID = oPros.SubTypeID
        status.Selected_ID = oPros.StatusID
        salutation.Selected_ID = oPros.Salutation
        txtMiddleInit.Text = oPros.MiddleInit
        txtCompany.Text = oPros.CompanyName
        txtTitle.Text = oPros.Title
        txtReferrer.Text = oPros.ReferringProspectID
        dteDateReferred.Selected_Date = oPros.DateReferred
        siSource.Selected_ID = oPros.SourceID
        cbFedDNC.Checked = oPros.FedDNCFlag
        txtProspectID.Text = oPros.Prospect_ID
        txtLastName.Text = oPros.Last_Name
        txtFirstName.Text = oPros.First_Name
        Label4.Text = txtLastName.Text & ", " & txtFirstName.Text

        dteBirthDate.Selected_Date = oPros.BirthDate
        dteSpouseBirthDate.Selected_Date = oPros.SpouseBirthDate
        siMaritalStatus.Selected_ID = oPros.MaritalStatusID
        txtSpouse.Text = oPros.SpouseID
        siOccupation.Selected_ID = oPros.Occupation
        txtIncome.Text = oPros.Income
        txtIncomeDebt.Text = oPros.IncomeDebt
        txtCreditScore.Text = oPros.CreditScore
        txtSpouseCreditScore.Text = oPros.SpouseCreditScore
        If CheckSecurity("Prospects", "ViewSSN", , , Session("UserDBID")) Then
            txtSSN.Text = oPros.SSN
            txtSpouseSSN.Text = oPros.SpouseSSN
        Else
            txtSSN.Text = "XXXXXXXXXX"
            txtSpouseSSN.Text = "XXXXXXXXXX"
        End If
        txtDriversLicense.Text = oPros.DriversLicense
        siDriversLicenseState.Selected_ID = oPros.DriversLicenseStateID
        txtSpouseFirstName.Text = oPros.SpouseFirstName
        txtSpouseLastName.Text = oPros.SpouseLastName
        dteAnniversaryDate.Selected_Date = oPros.AnniversaryDate
    End Sub

    Private Sub Load_Lookups()
        Dim sErr As String = ""
        type.Connection_String = Resources.Resource.cns
        type.Label_Caption = "Type:"
        type.ComboItem = "ProspectType"
        type.Load_Items()
        subtype.Connection_String = Resources.Resource.cns
        subtype.Label_Caption = "Sub-Type:"
        subtype.ComboItem = "Prospectsubtype"
        subtype.Load_Items()
        status.Connection_String = Resources.Resource.cns
        status.Label_Caption = "Status:"
        status.ComboItem = "Prospectstatus"
        status.Load_Items()
        salutation.Connection_String = Resources.Resource.cns
        'salutation.Label_Caption = "Salutation:"
        salutation.ComboItem = "Salutations"
        salutation.Load_Items()
        siMaritalStatus.Connection_String = Resources.Resource.cns
        siMaritalStatus.Label_Caption = "Marital Status:"
        siMaritalStatus.ComboItem = "MaritalStatus"
        siMaritalStatus.Load_Items()
        siOccupation.Connection_String = Resources.Resource.cns
        siOccupation.Label_Caption = "Occupation:"
        siOccupation.ComboItem = "Occupation"
        siOccupation.Load_Items()
        siDriversLicenseState.Connection_String = Resources.Resource.cns
        siDriversLicenseState.Label_Caption = "Drivers License State:"
        siDriversLicenseState.ComboItem = "State"
        siDriversLicenseState.Load_Items()
        siSource.Connection_String = Resources.Resource.cns
        siSource.Label_Caption = "Source:"
        siSource.ComboItem = "ProspectSource"
        siSource.Load_Items()
    End Sub

    Protected Sub Prospect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Prospect.Click
        If CInt(txtProspectID.Text) > 0 Then
            MultiView1.ActiveViewIndex = 0
            Dim oEmail As New clsEmail
            oEmail.ProspectID = CInt(txtProspectID.Text)
            gvEmail.DataSource = oEmail.List
            gvEmail.DataBind()
            lblEmailError.Text = oEmail.Error_Message
        End If
    End Sub

    Protected Sub Demographics_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Demographics.Click
        If txtProspectID.Text > 0 Then

            MultiView1.ActiveViewIndex = 1

            Dim oPhone As New clsPhone
            oPhone.ProspectID = txtProspectID.Text
            gvPhone.DataSource = oPhone.List
            gvPhone.DataBind()
            lblPhoneError.Text = oPhone.Error_Message
            oPhone = Nothing

            Dim oAddress As New clsAddress
            oAddress.ProspectID = txtProspectID.Text
            gvAddress.DataSource = oAddress.List
            gvAddress.DataBind()
            lblAddressError.Text = oAddress.Error_Message
            oAddress = Nothing
        End If
    End Sub

    
    Protected Sub Tour_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Tour_Link.Click
        If CInt(txtProspectID.Text) > 0 Then
            MultiView1.ActiveViewIndex = 2
            Dim oTours As New clsTour
            gvTours.DataSource = oTours.List(0, "ProspectID", txtProspectID.Text, "TourID desc")
            gvTours.DataBind()
            lblTourError.Text = oTours.Error_Message
        End If
    End Sub

    Protected Sub TourPKG_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TourPKG_Link.Click
        If CInt(txtProspectID.Text) > 0 Then
            MultiView1.ActiveViewIndex = 3
            Dim pkgs As New clsPackageIssued
            gvPackages.DataSource = pkgs.List(txtProspectID.Text)
            gvPackages.DataBind()
        End If
    End Sub

    Protected Sub UF_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UF_Link.Click
        If txtProspectID.Text > 0 Then
            MultiView1.ActiveViewIndex = 4
            With UserFields1
                '.UserID = Session("UserID")
                .KeyField = "Prospect"
                .KeyValue = IIf(CInt(txtProspectID.Text) > 0, CInt(txtProspectID.Text), 0)
                .Load_List()
            End With
        End If
    End Sub


    Protected Sub Fin_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Fin_Link.Click
        If txtProspectID.text > 0 Then
            MultiView1.ActiveViewIndex = 5
            Financials1.ProspectID = txtProspectID.Text
            Financials1.KeyField = "ProspectID"
            Financials1.KeyValue = 0 'txtProspectID.Text '0
            Financials1.View = "prospect"
            Financials1.Display()
        End If
    End Sub

    Protected Sub Con_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Con_Link.Click
        If CInt(txtProspectID.Text) > 0 Then
            MultiView1.ActiveViewIndex = 6
            Dim oContracts As New clsContract
            gvContracts.DataSource = oContracts.List_OwnContracts(0, "ProspectID", txtProspectID.Text, "ContractID")
            gvContracts.DataBind()
        End If
    End Sub

    Protected Sub Res_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Res_Link.Click
        If CInt(txtProspectID.text) > 0 Then
            MultiView1.ActiveViewIndex = 7
            Dim res As New clsReservations
            gvReservations.DataSource = res.List(0, "ProspectID", txtProspectID.Text, " r.checkindate desc ")
            gvReservations.DataBind()
            res = Nothing
        End If
    End Sub


    Protected Sub Events_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events_Link.Click
        If txtProspectID.Text > 0 Then
            MultiView1.ActiveViewIndex = 8
            Events1.KeyField = "ProspectID"
            Events1.KeyValue = txtProspectID.Text
            Events1.List()
        End If
    End Sub

    Protected Sub Notes_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Notes_Link.Click
        If txtProspectID.Text > 0 Then
            MultiView1.ActiveViewIndex = 9
            Notes1.KeyField = "ProspectID"
            Notes1.KeyValue = txtProspectID.Text
            Notes1.Display()
            lblNotesError.Text = Notes1.Error_Message
        End If
    End Sub

    Protected Sub Pers_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Pers_Link.Click
        If txtProspectID.Text > 0 Then
            MultiView1.ActiveViewIndex = 10
            PersonnelTrans1.KeyValue = txtProspectID.Text
            PersonnelTrans1.Load_Trans()
        End If
    End Sub

    Protected Sub Ref_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Ref_Link.Click
        If txtProspectID.text > 0 Then
            MultiView1.ActiveViewIndex = 11
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles Button1.Click
        Dim row As GridViewRow = sender.namingcontainer
        Label9.Text = row.Cells(2).Text
    End Sub

    Private Sub Update_Values()
        oPros.Prospect_ID = txtProspectID.Text
        If oPros.Prospect_ID <> 0 Then oPros.Load()
        oPros.Last_Name = txtLastName.Text
        oPros.First_Name = txtFirstName.Text
        oPros.MiddleInit = txtMiddleInit.Text
        oPros.Salutation = salutation.Selected_ID
        oPros.CompanyName = txtCompany.Text
        oPros.Title = txtTitle.Text
        oPros.ReferringProspectID = txtReferrer.Text
        oPros.DateReferred = dteDateReferred.Selected_Date
        oPros.SourceID = siSource.Selected_ID
        oPros.FedDNCFlag = cbFedDNC.Checked
        oPros.TypeID = type.Selected_ID
        oPros.SubTypeID = subtype.Selected_ID
        oPros.StatusID = status.Selected_ID

        oPros.BirthDate = dteBirthDate.Selected_Date
        oPros.SpouseBirthDate = dteSpouseBirthDate.Selected_Date
        oPros.MaritalStatusID = siMaritalStatus.Selected_ID
        oPros.SpouseID = txtSpouse.Text
        oPros.Occupation = siOccupation.Selected_ID
        oPros.Income = txtIncome.Text
        oPros.IncomeDebt = txtIncomeDebt.Text
        oPros.CreditScore = txtCreditScore.Text
        oPros.DriversLicense = txtDriversLicense.Text
        oPros.DriversLicenseStateID = siDriversLicenseState.Selected_ID
        If CheckSecurity("Prospects", "ViewSSN", , , Session("UserDBID")) Then
            oPros.SSN = txtSSN.Text
            oPros.SpouseSSN = txtSpouseSSN.Text
        End If
        oPros.SpouseFirstName = txtSpouseFirstName.Text
        oPros.SpouseLastName = txtSpouseLastName.Text
        oPros.SpouseCreditScore = IIf(txtSpouseCreditScore.Text = "", 0, txtSpouseCreditScore.Text)
        oPros.AnniversaryDate = dteAnniversaryDate.Selected_Date.ToString & ""

        oPros.UserID = Session("UserDBID")
        If Not oPros.Save() Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('" & oPros.Error_Message & "');", True)
        End If
        Response.Write(oPros.Error_Message)
        txtProspectID.Text = oPros.Prospect_ID
        Label4.Text = oPros.Last_Name & ", " & oPros.First_Name


        'Label6.Text += "Complete"
    End Sub

    Protected Sub btnNewContract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewContract.Click
        Dim oDNS As New clsDoNotSellList
        If oDNS.Get_Status(txtProspectID.Text) = "Remove" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "modal.mwindow.open('DoNotSellListOverRide.aspx?KeyField=Prospect&KeyValue=" & txtProspectID.Text & "&Source=AddContract','win01',350,350);", True)
            'ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('This Prospect Is On The Do Not Sell List And Is Unable To Purchase A Contract.');", True)
        Else
            Response.Redirect(Request.ApplicationPath & "/marketing/editcontract.aspx?prospectid=" & txtProspectID.Text)
        End If
        oDNS = Nothing

    End Sub

    
    Protected Sub gvPackages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPackages.SelectedIndexChanged
        Dim row As GridViewRow = gvPackages.SelectedRow
        Response.Redirect("editPackage.aspx?packageissuedid=" & row.Cells(1).text)
    End Sub

    Protected Sub Button1_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim oDNS As New clsDoNotSellList
        If oDNS.Get_Status(txtProspectID.Text) = "Remove" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "modal.mwindow.open('DoNotSellListOverRide.aspx?KeyField=Prospect&KeyValue=" & txtProspectID.Text & "&Source=AddPackage','win01',350,350);", True)
            'ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('This Prospect Is On The Do Not Sell List And Is Unable To Purchase A Package.');", True)
        Else
            Response.Redirect("editPackage.aspx?packageissuedid=0&prospectid=" & txtProspectID.Text)
        End If
        oDNS = Nothing
    End Sub

    Protected Sub gvReservations_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvReservations.SelectedIndexChanged
        Dim row As gridviewrow = gvReservations.selectedRow
        Response.Redirect("editReservation.aspx?reservationid=" & row.Cells(1).Text)
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Response.Redirect("editReservation.aspx?reservationid=0&prospectid=" & txtProspectID.text)
    End Sub

    Protected Sub lbSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSave.Click
        If txtProspectID.Text > -1 Then
            If txtProspectID.Text = 0 And CheckSecurity("Prospects", "Add", , , Session("UserDBID")) Then
                Update_Values()
                ClientScript.RegisterClientScriptBlock(Me.GetType, "reload", "window.location.href='editprospect.aspx?prospectid=" & oPros.Prospect_ID & "';", True)
            Else
                If CheckSecurity("Prospects", "Edit", , , Session("UserDBID")) Then
                    If txtProspectID.Text > 0 Then
                        Dim oPros As New clsProspect
                        If oPros.Check_Owner(txtProspectID.Text) Then
                            If CheckSecurity("Owners", "Edit", , , Session("UserDBID")) Then
                                Update_Values()
                            Else
                                ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('You Do Not Have Persmission To OverWrite an Owner Record.');", True)
                            End If
                        Else
                            Update_Values()
                        End If
                        oPros = Nothing
                    Else
                        Update_Values()
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "reload", "window.location.href='editprospect.aspx?prospectid=" & oPros.Prospect_ID & "';", True)
                    End If
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('Access Denied');", True)
                End If
            End If
        End If
    End Sub

    Protected Sub btnSpouse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSpouse.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "modal.mwindow.open('../general/selectspouse.aspx?ProspectID=" & txtProspectID.Text & "','win01',350,350);", True)
    End Sub

    Protected Sub btnWebSite_Click(sender As Object, e As System.EventArgs) Handles btnWebSite.Click
        If txtProspectID.Text > 0 Then
            If CheckSecurity("Owners", "Edit", , , Session("UserDBID")) Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "modal.mwindow.open('../general/webSiteInfo.aspx?ProspectID=" & txtProspectID.Text & "','win01',350,350);", True)
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('Please Save Prospect Record.');", True)
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oPros As New clsProspect
        Dim oAdd As New clsAddress
        Dim oCombo As New clsComboItems
        oPros.Prospect_ID = txtProspectID.Text
        oPros.Load()
        oAdd.AddressID = oAdd.Get_Address_ID(oPros.Prospect_ID)
        If oAdd.AddressID = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('No Active Addresses.');", True)
        Else
            oAdd.Load()
            ClientScript.RegisterClientScriptBlock(Me.GetType, "popAdd", "Print_Envelope('" & oPros.First_Name & "', '" & oPros.Last_Name & "', '" & oAdd.Address1 & "', '" & oAdd.City & "', '" & oCombo.Lookup_ComboItem(oAdd.StateID) & "', '" & oAdd.PostalCode & "');", True)
        End If
        oAdd = Nothing
        oPros = Nothing
        oCombo = Nothing
    End Sub

    Protected Sub gvReservations_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Reservations" Then
            If e.Row.RowIndex > -1 Then
                If e.Row.Cells(2).Text <> "&nbsp;" Then
                    e.Row.Cells(2).Text = CDate(e.Row.Cells(2).Text).ToShortDateString
                End If
                If e.Row.Cells(3).Text <> "&nbsp;" Then
                    e.Row.Cells(3).Text = CDate(e.Row.Cells(3).Text).ToShortDateString
                End If
                If e.Row.Cells(5).Text <> "&nbsp;" Then
                    e.Row.Cells(5).Text = CDate(e.Row.Cells(5).Text).ToShortDateString
                End If
            End If
        End If
    End Sub

    Protected Sub gvTours_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Tours" Then
            If e.Row.RowIndex > -1 Then
                If e.Row.Cells(2).Text <> "&nbsp;" Then
                    e.Row.Cells(2).Text = CDate(e.Row.Cells(2).Text).ToShortDateString
                End If
            End If
        End If
    End Sub

    Protected Sub Docs_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Docs_Link.Click
        If txtProspectID.Text > 0 Then
            MultiView1.ActiveViewIndex = 13
            UploadedDocs1.KeyField = "ProspectID"
            UploadedDocs1.KeyValue = txtProspectID.Text
            UploadedDocs1.List()
        End If
    End Sub

    Protected Sub CC_Link_Click(sender As Object, e As System.EventArgs) Handles CC_Link.Click
        If txtProspectID.Text > 0 Then
            Dim oCC As New clsCreditCard
            gvCC.DataSource = oCC.Get_Cards_By_Prospect(txtProspectID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvCC.DataKeyNames = sKeys
            gvCC.DataBind()
            oCC = Nothing
            MultiView1.ActiveViewIndex = 14
        End If
    End Sub

    Protected Sub gvCC_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvCC.SelectedIndexChanged
        Dim row As GridViewRow = gvCC.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "modal.mwindow.open('../general/cconFile.aspx?ID=" & row.Cells(1).Text & "','win01',350,350);", True)
    End Sub

    Protected Sub add_creditcard_linkbutton_Click(sender As Object, e As System.EventArgs) Handles add_creditcard_linkbutton.Click

        If txtProspectID.Text > 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "modal.mwindow.open('../general/cconFile.aspx?ID=-1&ProspectID=" & txtProspectID.Text.Trim() & "','win01',350,350);", True)
        End If
    End Sub

    Protected Sub Points_Link_Click(sender As Object, e As EventArgs) Handles Points_Link.Click
        If txtProspectID.Text > 0 Then
            ucPoints.ProspectID = txtProspectID.Text
            ucPoints.display()
            MultiView1.ActiveViewIndex = 15
        End If
    End Sub

    Protected Sub btnDNS_Click(sender As Object, e As EventArgs) Handles btnDNS.Click

        If btnDNS.Text = "Remove From Do Not Sell List" Then
            If CheckSecurity("Prospects", "RemoveFromDoNotSell", , , Session("UserDBID")) Then
                Dim oDNS As New clsDoNotSellList
                oDNS.ID = oDNS.Get_DNS_Entry(txtProspectID.Text)
                oDNS.Load()
                oDNS.UserID = Session("UserDBID")
                oDNS.DateRemoved = System.DateTime.Now
                oDNS.RemovedByID = Session("UserDBID")
                oDNS.Save()
                oDNS = Nothing
                Response.Redirect("EditProspect.aspx?ProspectID=" & txtProspectID.Text)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('You Do Not Have Permission To Remove From The Do Not Sell List.');", True)
            End If
        Else
            If CheckSecurity("Prospects", "AddToDoNotSell", , , Session("UserDBID")) Then
                Dim oDNS As New clsDoNotSellList
                oDNS.ID = 0
                oDNS.UserID = Session("UserDBID")
                oDNS.Load()
                oDNS.ProspectID = txtProspectID.Text
                oDNS.DateAdded = System.DateTime.Now
                oDNS.AddedByID = Session("UserDBID")
                oDNS.Save()
                oDNS = Nothing
                Dim cn As New SqlConnection(Resources.Resource.cns)
                Dim cm As New SqlCommand("select prospectid from t_ContractCoowner where prospectid <> " & txtProspectID.Text & " and contractid in (select contractid from t_Contract where prospectid = " & txtProspectID.Text & ")", cn)
                Dim da As New SqlDataAdapter(cm)
                Dim ds As New DataSet
                da.Fill(ds, "CoOwners")
                For Each row In ds.Tables("CoOwners").Rows
                    oDNS = New clsDoNotSellList
                    oDNS.ID = 0
                    oDNS.UserID = Session("UserDBID")
                    oDNS.Load()
                    oDNS.ProspectID = row("ProspectID")
                    oDNS.DateAdded = System.DateTime.Now
                    oDNS.AddedByID = Session("UserDBID")
                    oDNS.Save()
                    oDNS = Nothing
                Next
                ds = Nothing
                da = Nothing
                cm = Nothing
                cn = Nothing
                Response.Redirect("EditProspect.aspx?ProspectID=" & txtProspectID.Text)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('You Do Not Have Permission To Add To The Do Not Sell List.');", True)
            End If
        End If
    End Sub

    Protected Sub lbAddTour_Click(sender As Object, e As EventArgs) Handles lbAddTour.Click
        Dim oDNS As New clsDoNotSellList
        If oDNS.Get_Status(txtProspectID.Text) = "Remove" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "modal.mwindow.open('DoNotSellListOverRide.aspx?KeyField=Prospect&KeyValue=" & txtProspectID.Text & "&Source=AddTour','win01',350,350);", True)
            'ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('This Prospect Is On The Do Not Sell List And Is Unable To Tour.');", True)
        Else
            Response.Redirect("EditTour.aspx?TourID=0&ProspectID=" & txtProspectID.Text)
        End If
        oDNS = Nothing
    End Sub
End Class
