Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Partial Class marketing_editcontract
    Inherits System.Web.UI.Page

    Dim oContract As New clsContract

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If CheckSecurity("Contracts", "View", , , Session("UserDBID")) Then

                '*** Create view events *** '
                If IsNumeric(Request("ContractID")) Then
                    If CInt(Request("ContractID")) > 0 Then
                        Dim oE As New clsEvents
                        Dim sErr As String = ""

                        If Not (oE.Find_View_Event("ContractID", Request("ContractID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                            If sErr <> "" Then lblContractError.Text = sErr
                            sErr = ""
                            If Not (oE.Create_View_Event("ContractID", Request("ContractID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                                If sErr <> "" Then lblContractError.Text &= "<br />" & sErr
                            End If
                        End If
                        oE = Nothing
                    End If
                End If

                '*** End View Events *** '

                MultiView1.ActiveViewIndex = 0


                oContract.ContractID = IIf(IsNumeric(Request("ContractID")), Request("ContractID"), 0)
                oContract.Load()
                Load_Lookups()
                Dim oPros As New clsProspect
                oPros.Prospect_ID = IIf(IsNumeric(Request("ProspectID")), Request("ProspectID"), oContract.ProspectID)
                oPros.Load()
                lbProspect.Text = oPros.Last_Name & ", " & oPros.First_Name '& " .. " & oPros.Prospect_ID & " .. " & oContract.ProspectID
                oPros = Nothing
                If oContract.ProspectID = 0 And IsNumeric(Request("ProspectID")) Then oContract.ProspectID = Request("ProspectID")

                Dim newItem As New System.Web.UI.WebControls.ListItem("", 0)
                ddOccupanyYear.Items.Add(newItem)
                For i = 1998 To Date.Now.Year + 8
                    newItem = New System.Web.UI.WebControls.ListItem(i, i)
                    ddOccupanyYear.Items.Add(newItem)
                Next
                Set_Fields()
                If Request("UsageID") <> "" Then
                    MultiView1.ActiveViewIndex = 4
                    Dim oUsage As New clsUsage
                    gvUsages.DataSource = oUsage.List(Request("ContractID"))
                    gvUsages.DataBind()
                    oUsage = Nothing
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/editUsage.aspx?UsageID=" & Request("UsageID") & "','win01',690,450);", True)
                End If
            Else
                MultiView1.ActiveViewIndex = 13
                txtContractID.Text = -1
            End If
        End If
    End Sub

    Private Sub Set_Fields()
        txtProspectID.Text = oContract.ProspectID
        txtLocationID.Text = oContract.LocationID
        txtContractID.Text = oContract.ContractID
        txtContractNumber.Text = oContract.ContractNumber
        dfContractDate.Selected_Date = oContract.ContractDate
        dteAnniversaryDate.Selected_Date = oContract.AnniversaryDate
        ckTrust.Checked = oContract.TrustFlag
        txtTrustName.Text = oContract.TrustName
        ckCompany.Checked = oContract.CompanyFlag
        txtCompanyName.Text = oContract.CompanyName
        siContractType.Selected_ID = oContract.TypeID
        siContractSubType.Selected_ID = oContract.SubTypeID
        siSaleType.Selected_ID = oContract.SaleTypeID
        siSaleSubType.Selected_ID = oContract.SaleSubTypeID
        siWeekType.Selected_ID = oContract.WeekTypeID
        siSeason.Selected_ID = oContract.SeasonID
        siBillingCode.Selected_ID = oContract.BillingCodeID
        siStatus.Selected_ID = oContract.StatusID
        siSubStatus.Selected_ID = oContract.SubStatusID
        siMaintenanceFeeStatus.Selected_ID = oContract.MaintenanceFeeStatusID
        'Response.Write(oContract.StatusID & " : " & siStatus.Selected_ID)
        txtStatusDate.Text = oContract.StatusDate
        txtPropertyTax.Text = oContract.PropertyTaxAmount
        txtTourID.Text = oContract.TourID
        txtMaintenanceFee.Text = oContract.MaintenanceFeeAmount
        For i = 0 To ddOccupanyYear.Items.Count - 1
            If oContract.OccupancyDate <> "" Then
                If ddOccupanyYear.Items(i).Value = Year(oContract.OccupancyDate) Then
                    ddOccupanyYear.SelectedIndex = i
                    Exit For
                End If
            End If
        Next
        Dim ds As New SqlDataSource(Resources.Resource.cns, "Select * from t_Frequency order by Frequency")
        ddFrequency.DataSource = ds
        ddFrequency.DataValueField = "FrequencyID"
        ddFrequency.DataTextField = "Frequency"
        ddFrequency.DataBind()
        For i = 0 To ddFrequency.Items.Count - 1
            If ddFrequency.Items(i).Value = oContract.FrequencyID Then ddFrequency.SelectedIndex = i
        Next
        ds = Nothing
        Campaign1.Load_Items()
        Campaign1.Selected_ID = oContract.CampaignID
        ddMFCode.SelectedValue = oContract.MaintenanceFeeCodeID
    End Sub

    Private Sub Load_Lookups()
        Dim sErr As String = ""
        siContractType.Connection_String = Resources.Resource.cns
        siContractType.Label_Caption = ""
        siContractType.ComboItem = "ContractType"
        siContractType.Selected_ID = oContract.TypeID
        siContractType.Load_Items()
        siMaintenanceFeeStatus.Connection_String = Resources.Resource.cns
        siMaintenanceFeeStatus.Label_Caption = ""
        siMaintenanceFeeStatus.ComboItem = "MaintenanceFeeStatus"
        siMaintenanceFeeStatus.Selected_ID = oContract.MaintenanceFeeStatusID
        siMaintenanceFeeStatus.Load_Items()
        siContractSubType.Connection_String = Resources.Resource.cns
        siContractSubType.Label_Caption = ""
        siContractSubType.ComboItem = "Contractsubtype"
        siContractSubType.Selected_ID = oContract.SubTypeID
        siContractSubType.Load_Items()
        siSaleType.Connection_String = Resources.Resource.cns
        siSaleType.Label_Caption = ""
        siSaleType.ComboItem = "ContractSaleType"
        siSaleType.Selected_ID = oContract.SaleTypeID
        siSaleType.Load_Items()
        siSaleSubType.Connection_String = Resources.Resource.cns
        siSaleSubType.Label_Caption = ""
        siSaleSubType.ComboItem = "ContractSaleSubType"
        siSaleSubType.Selected_ID = oContract.SaleSubTypeID
        siSaleSubType.Load_Items()
        siWeekType.Connection_String = Resources.Resource.cns
        siWeekType.Label_Caption = ""
        siWeekType.ComboItem = "WeekType"
        siWeekType.Selected_ID = oContract.WeekTypeID
        siWeekType.Load_Items()
        siSeason.Connection_String = Resources.Resource.cns
        siSeason.Label_Caption = ""
        siSeason.ComboItem = "Season"
        siSeason.Selected_ID = oContract.SeasonID
        siSeason.Load_Items()
        siBillingCode.Connection_String = Resources.Resource.cns
        siBillingCode.Label_Caption = ""
        siBillingCode.ComboItem = "BillingCode"
        siBillingCode.Selected_ID = oContract.BillingCodeID
        siBillingCode.Load_Items()
        siStatus.Connection_String = Resources.Resource.cns
        siStatus.Label_Caption = ""
        siStatus.ComboItem = "ContractStatus"
        siStatus.Selected_ID = oContract.StatusID
        siStatus.Load_Items()
        siSubStatus.Connection_String = Resources.Resource.cns
        siSubStatus.Label_Caption = ""
        siSubStatus.ComboItem = "ContractSubStatus"
        siSubStatus.Selected_ID = oContract.SubStatusID
        siSubStatus.Load_Items()
        Dim oMFCodes As New clsMaintenanceFeeCodes
        ddMFCode.DataSource = oMFCodes.List(True)
        ddMFCode.DataTextField = "Code"
        ddMFCode.DataValueField = "MaintenanceFeeCodeID"
        ddMFCode.DataBind()
        oMFCodes = Nothing
    End Sub

    Protected Sub Contract_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Contract_Link.Click
        If txtContractID.text > 0 Then
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub CoOwner_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CoOwner_Link.Click
        If txtContractID.Text > 0 Then
            MultiView1.ActiveViewIndex = 1
            Dim oCoOwner As New clsContractCoOwner
            If IsNumeric(txtContractID.Text) And txtContractID.Text <> "" Then oCoOwner.ContractID = txtContractID.Text
            gvCoOwners.DataSource = oCoOwner.List
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvCoOwners.DataKeyNames = sKeys
            gvCoOwners.DataBind()
            oCoOwner = Nothing
        End If
    End Sub

    Protected Sub Inventory_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Inventory_Link.Click
        If txtContractID.Text > 0 Then
            MultiView1.ActiveViewIndex = 2
            ucsoldinventory.contractid = IIf(IsNumeric(txtContractID.Text) And txtContractID.Text <> "", txtContractID.Text, 0)
            ucsoldinventory.display()
        End If
    End Sub

    Protected Sub Mortgage_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Mortgage_Link.Click
        If txtContractID.Text > 0 Then
            MultiView1.ActiveViewIndex = 3
            Dim oMort As New clsMortgage

            gvMortgage.DataSource = oMort.List(, "ContractID", txtContractID.Text, "MortgageID desc")
            gvMortgage.DataBind()
            oMort = Nothing
        End If
    End Sub

    Protected Sub Usage_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Usage_Link.Click
        If txtContractID.Text > 0 Then
            MultiView1.ActiveViewIndex = 4
            Dim oUsage As New clsUsage
            gvUsages.DataSource = oUsage.List(Request("ContractID"))
            gvUsages.DataBind()
            oUsage = Nothing
        End If
    End Sub

    Protected Sub UploadedDocs_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UploadedDocs_Link.Click
        If txtContractID.Text > 0 Then
            MultiView1.ActiveViewIndex = 5
            ucDocs.KeyField = "ContractID"
            ucDocs.KeyValue = IIf(IsNumeric(txtContractID.Text), IIf(CLng(txtContractID.Text) > 0, txtContractID.Text, 0), 0)
            ucDocs.List()
        End If
    End Sub

    Protected Sub Events_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events_Link.Click
        If txtContractID.Text > 0 Then
            MultiView1.ActiveViewIndex = 6
            ucEvents.KeyField = "ContractID"
            ucEvents.KeyValue = txtContractID.Text
            ucEvents.List()
        End If
    End Sub

    Protected Sub Notes_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Notes_Link.Click
        If txtContractID.text > 0 Then
            MultiView1.ActiveViewIndex = 7
            ucNotes.KeyField = "ContractID"
            ucNotes.KeyValue = txtContractID.Text
            ucNotes.Display()
        End If
    End Sub

    Protected Sub Personnel_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Personnel_Link.Click
        If txtContractID.Text > 0 Then
            If CheckSecurity("Contracts", "ViewPersonnel", , , Session("UserDBID")) Then
                MultiView1.ActiveViewIndex = 8
                PersonnelTrans1.KeyValue = txtContractID.Text
                PersonnelTrans1.Load_Trans()
            Else
                MultiView1.ActiveViewIndex = 13
            End If
        End If
    End Sub

    Protected Sub UserFields_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UserFields_Link.Click
        If txtContractID.text > 0 Then
            MultiView1.ActiveViewIndex = 9
            UF.KeyField = "Contract"
            UF.KeyValue = CInt(txtContractID.Text)
            UF.Load_List()
        End If
    End Sub

    Protected Sub Usage_Restrict_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Usage_Restrict_Link.Click
        If txtContractID.Text > 0 Then
            Dim ouRestrictor As New clsUsageRestriction2Contract
            gvUsageRestrictions.DataSource = ouRestrictor.List(txtContractID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "UsageRestriction2ContractID"
            gvUsageRestrictions.DataKeyNames = sKeys
            gvUsageRestrictions.DataBind()
            MultiView1.ActiveViewIndex = 10
        End If
    End Sub
    Protected Sub gvUsageRestrictions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub
    Protected Sub gvUsageRestrictions_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvUsageRestrictions.RowCommand
        Dim partID As Integer
        partID = Convert.ToInt32(gvUsageRestrictions.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        If e.CommandName.CompareTo("Remove") = 0 Then
            If CheckSecurity("UsageRestriction", "Remove", , , Session("UserDBID")) Then
                Dim ouRestriction As New clsUsageRestriction2Contract
                If ouRestriction.Remove_Restrictor(partID) Then
                    gvUsageRestrictions.DataSource = ouRestriction.List(txtContractID.Text)
                    Dim sKeys(0) As String
                    sKeys(0) = "UsageRestriction2ContractID"
                    gvUsageRestrictions.DataKeyNames = sKeys
                    gvUsageRestrictions.DataBind()
                End If
                ouRestriction = Nothing
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
            End If
        End If
    End Sub

    Protected Sub Auth_Users_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Auth_Users_Link.Click
        If txtContractID.Text > 0 Then
            Dim oAuthUser As New clsContractAuthorizedUsers
            gvAuthUser.DataSource = oAuthUser.List_AuthUsers(txtContractID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvAuthUser.DataKeyNames = sKeys
            gvAuthUser.DataBind()
            MultiView1.ActiveViewIndex = 11
        End If
    End Sub
    Protected Sub gvAuthUser_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(0).Visible = False
        End If
    End Sub
    Protected Sub gvAuthUser_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvAuthUser.RowCommand
        Dim userID As Integer
        userID = Convert.ToInt32(gvAuthUser.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        If e.CommandName.CompareTo("Remove") = 0 Then
            If CheckSecurity("Contracts", "RemoveAuthorizedUsers", , , Session("UserDBID")) Then
                Dim oAuthUser As New clsContractAuthorizedUsers
                If oAuthUser.Remove_Auth_User(userID) Then
                    gvAuthUser.DataSource = oAuthUser.List_AuthUsers(txtContractID.Text)
                    Dim sKeys(0) As String
                    sKeys(0) = "ID"
                    gvAuthUser.DataKeyNames = sKeys
                    gvAuthUser.DataBind()
                End If
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
            End If
        End If
    End Sub
    Protected Sub lbProspect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbProspect.Click
        Response.Redirect(Request.ApplicationPath & "/marketing/editprospect.aspx?prospectid=" & IIf(IsNumeric(Request("ProspectID")) And Request("ProspectID") <> "0" And Request("ProspectID") <> 0 And Request("ProspectID") <> "", Request("ProspectID"), txtProspectID.Text))
    End Sub

    Protected Sub btnAddMortgage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddMortgage.Click
        Response.Redirect(Request.ApplicationPath & "/marketing/editmortgage.aspx?contractid=" & txtContractID.Text & "&mortgageid=0")
    End Sub

    Protected Sub gvUsages_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                If e.Row.Cells(7).Text <> "&nbsp;" Then
                    e.Row.Cells(7).Text = CStr(CDate(e.Row.Cells(7).Text).ToShortDateString)
                End If
                If e.Row.Cells(8).Text <> "&nbsp;" Then
                    e.Row.Cells(8).Text = CStr(CDate(e.Row.Cells(8).Text).ToShortDateString)
                End If
            End If
        End If
    End Sub
    Protected Sub gvUsages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvUsages.SelectedIndexChanged
        Dim row As gridviewrow = gvUsages.selectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/editUsage.aspx?UsageID=" & row.Cells(1).Text & "','win01',690,450);", True)
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        'added code per work order #49557
        'check for yellow season and display a prompt if so
        Dim oCon As New clsContract
        oCon.ContractID = Request("ContractID")
        oCon.Load()
        If oCon.SeasonID = 16961 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall1", "alert('THIS IS A YELLOW SEASON OWNER');", True)
        End If
        'end of code added

        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/editUsage.aspx?UsageID=0&ContractID=" & Request("ContractID") & "','win01',690,450);", True)
    End Sub

    Protected Sub gvConversions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvConversions.SelectedIndexChanged
        Dim row As GridViewRow = gvConversions.SelectedRow
        Response.Redirect("editConversion.aspx?ConversionID=" & row.Cells(1).Text)
    End Sub

    Protected Sub Conversion_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Conversion_Link.Click
        If txtContractID.Text > 0 Then
            Dim oConversion As New clsConversion
            gvConversions.DataSource = oConversion.List_Conversions(txtContractID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ConversionID"
            gvConversions.DataKeyNames = sKeys
            gvConversions.DataBind()
            MultiView1.ActiveViewIndex = 12
        End If
    End Sub

    Protected Sub btnSave3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave3.Click
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        If txtContractID.Text > 0 Then
            If CheckSecurity("Contracts", "Edit", , , Session("UserDBID")) Then
                'CHECK INVENTORY
                If siStatus.Selected_ID < 1 Then
                    bProceed = False
                    sErr = "You must select a Contract Status."
                End If
            Else
                bProceed = False
                sErr = "You Do Not Have Persmission to Edit A Contract."
            End If
        Else
            If Not (CheckSecurity("Contracts", "Create", , , Session("UserDBID"))) Then
                bProceed = False
                sErr = "You Do Not Have Persmission to Create A Contract."
            Else
                If siStatus.Selected_ID < 1 Then
                    bProceed = False
                    sErr = "You must select a Contract Status."
                End If
            End If
        End If
        If bProceed Then
            With oContract
                .ContractID = txtContractID.Text
                .UserID = Session("UserDBID")
                .Load()
                .ContractDate = dfContractDate.Selected_Date
                .ContractNumber = txtContractNumber.Text
                .OccupancyDate = IIf(ddOccupanyYear.SelectedValue > 0, "1/1/" & ddOccupanyYear.SelectedValue, "")
                .TrustFlag = ckTrust.Checked
                .TrustName = txtTrustName.Text
                .CompanyFlag = ckCompany.Checked
                .CompanyName = txtCompanyName.Text
                .TypeID = siContractType.Selected_ID
                .SubTypeID = siContractSubType.Selected_ID
                .SaleTypeID = siSaleType.Selected_ID
                .SaleSubTypeID = siSaleSubType.Selected_ID
                If .ContractDate <> "" Then
                    If CDate(.ContractDate) > CDate("7/9/2015") Then
                        If InStr(siSaleType.SelectedName.toUpper, "DWN") > 0 Then
                            If Not (IsNumeric(Left(txtContractNumber.Text, 1))) Then
                                .ContractNumber = Replace(txtContractNumber.Text, "A", "F")
                            Else
                                .ContractNumber = "F" & txtContractNumber.Text
                            End If
                        ElseIf InStr(siSaleType.SelectedName.toUpper, "UPGRADE") > 0 Then
                            If Not (IsNumeric(Left(txtContractNumber.Text, 1))) Then
                                .ContractNumber = Replace(txtContractNumber.Text, "F", "A")
                            Else
                                .ContractNumber = "A" & txtContractNumber.Text
                            End If
                        ElseIf InStr(siSaleType.SelectedName.ToUpper, "REWRITE") Then
                            .ContractNumber = txtContractNumber.Text
                        Else
                            If Not (IsNumeric(Left(txtContractNumber.Text, 1))) Then
                                .ContractNumber = Replace(txtContractNumber.Text, "F", "")
                                .ContractNumber = Replace(txtContractNumber.Text, "A", "")
                            End If
                        End If
                    End If
                End If
                .WeekTypeID = siWeekType.Selected_ID
                .SeasonID = siSeason.Selected_ID
                .BillingCodeID = siBillingCode.Selected_ID
                .MaintenanceFeeStatusID = siMaintenanceFeeStatus.Selected_ID
                .FrequencyID = IIf(IsNumeric(ddFrequency.SelectedValue), ddFrequency.SelectedValue, 0)
                If .ContractID > 0 Then
                    Dim oCombo As New clsComboItems
                    If (oCombo.Lookup_ComboItem(siStatus.Selected_ID) = "Active" Or oCombo.Lookup_ComboItem(siStatus.Selected_ID) = "Suspense") And (oCombo.Lookup_ComboItem(.StatusID) = "Pender" Or oCombo.Lookup_ComboItem(.StatusID) = "Pender-INV" Or oCombo.Lookup_ComboItem(.StatusID) = "Developer") Then
                        .OriginallyWrittenDate = System.DateTime.Now
                        .MaintenanceFeeStatusID = oCombo.Lookup_ID("MaintenanceFeeStatus", "Active")
                    End If
                    oCombo = Nothing
                End If
                .StatusID = siStatus.Selected_ID
                .AnniversaryDate = dteAnniversaryDate.Selected_Date.ToString & ""
                .SplitMF = ckSplitMF.Checked
                .PropertyTaxAmount = txtPropertyTax.Text
                .TourID = txtTourID.Text
                .CampaignID = Campaign1.Selected_ID
                .ProspectID = txtProspectID.Text
                .LocationID = txtLocationID.Text
                .SubStatusID = siSubStatus.Selected_ID
                .MaintenanceFeeAmount = txtMaintenanceFee.Text
                .MaintenanceFeeCodeID = ddMFCode.SelectedValue
                If txtContractID.Text = 0 Then
                    .OriginallyWrittenDate = System.DateTime.Now
                End If
                If .ContractID = 0 Then bProceed = False
                .Save()
                If Not bProceed Then
                    Dim oE As New clsEvents
                    oE.Create_Create_Event("ContractID", Request("ContractID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)
                    oE = Nothing
                End If
                txtContractID.Text = .ContractID
                lblContractError.Text = .Error_Message
                Response.Redirect("editContract.aspx?ContractID=" & .ContractID)
            End With
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub MultiView1_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MultiView1.ActiveViewChanged

    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oDNS As New clsDoNotSellList
        Dim oCon As New clsContract
        oCon.ContractID = txtContractID.Text
        oCon.Load()
        If oDNS.Get_Status(oCon.ProspectID) = "Remove" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('This Prospect Is On The Do Not Sell List And Is Unable To Convert.');", True)
        Else
            Response.Redirect("editConversion.aspx?ConversionID=0&ContractID=" & txtContractID.Text)
        End If
        oCon = Nothing
        oDNS = Nothing
    End Sub

    Protected Sub Unnamed1_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/editUsageRestrictor.aspx?ContractID=" & Request("ContractID") & "','win01',690,450);", True)
    End Sub

    Protected Sub lbRefreshCoOwner_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRefreshCoOwner.Click
        CoOwner_Link_Click(sender, e)
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        If CheckSecurity("Contracts", "AddAuthorizedUsers", , , Session("UserDBID")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/addAuthUser.aspx?ContractID=" & txtContractID.Text & "','win01',350,350);", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
        End If
    End Sub

    Protected Sub btnRePrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRePrint.Click
        If txtContractID.Text <> "" Then
            If txtContractID.Text > 0 Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "none", "javascript:modal.mwindow.open('../general/printcontracttest.aspx?contractid=" & txtContractID.Text & "','win01',1050,1050);", True)
            End If
        End If
    End Sub

    Protected Sub Remove_CoOwner()


    End Sub

 
    Protected Sub gvCoOwners_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCoOwners.RowCommand
        Dim CoOwnID As Integer = Convert.ToInt32(gvCoOwners.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        Dim oCoOwn As New clsContractCoOwner

        If oCoOwn.Remove(CoOwnID) Then
            If IsNumeric(txtContractID.Text) And txtContractID.Text <> "" Then oCoOwn.ContractID = txtContractID.Text
            gvCoOwners.DataSource = oCoOwn.List
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvCoOwners.DataKeyNames = sKeys
            gvCoOwners.DataBind()
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "notice", "alert('" & oCoOwn.Error_Message & "');", True)
        End If
        oCoOwn = Nothing
    End Sub
End Class
