Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System
Partial Class wizards_tourCheckIn2
    Inherits System.Web.UI.Page
    Dim prosID As Integer
    Dim tourID As Integer
    Dim rep As String = ""
    Dim status As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If Not (CheckSecurity("Tours", "Edit", , , Session("UserDBID"))) Then
                MultiView1.ActiveViewIndex = 8
            Else
                Session("Address_Table") = Nothing
                Session("Email_Table") = Nothing
                Session("Premiums_Table") = Nothing
                Session("Phone_Table") = Nothing
                Session("Notes_Table") = Nothing
                Session("EditPremiums") = Nothing
                Session("EditAddress") = Nothing
                Session("EditPhone") = Nothing
                Session("EditEmail") = Nothing
                Session("EditNotes") = Nothing
                Session("OPCRep") = Nothing
                Session("OPCLocation") = Nothing
                MultiView1.ActiveViewIndex = 4
                Load_Lookups()
                Set_Prospect_Values()
                Set_Tour_Values()
                get_Premiums()
                get_Notes()
            End If
        End If
    End Sub

    Protected Sub Load_Lookups()
        If Not (IsPostBack) Then
            state.Connection_String = Resources.Resource.cns
            state.Label_Caption = ""
            state.ComboItem = "State"
            state.Load_Items()
            mStatus.Connection_String = Resources.Resource.cns
            mStatus.Label_Caption = ""
            mStatus.ComboItem = "MaritalStatus"
            mStatus.Load_Items()
            tStatus.Connection_String = Resources.Resource.cns
            tStatus.Label_Caption = ""
            tStatus.ComboItem = "TourStatus"
            tStatus.Load_Items()
            tLocation.Connection_String = Resources.Resource.cns
            tLocation.ComboItem = "TourLocation"
            tLocation.Label_Caption = ""
            tLocation.Load_Items()
            tTime.Connection_String = Resources.Resource.cns
            tTime.Label_Caption = ""
            tTime.ComboItem = "TourTime"
            tTime.Load_Items()
            siTourSubType.Connection_String = Resources.Resource.cns
            siTourSubType.Label_Caption = ""
            siTourSubType.ComboItem = "TourSubType"
            siTourSubType.Load_Items()
            siMS.Connection_String = Resources.Resource.cns
            siMS.Load_Items()

            If Request("TourID") = "0" Then
                campaignID.Load_Items()
            End If
        End If
    End Sub

    Protected Sub Set_Prospect_Values()
        Dim oTour As New clsTour
        Dim oPros As New clsProspect
        Dim oProsSpouse As New clsProspect
        If Request("TourID") <> 0 Then
            oPros.Prospect_ID = oTour.get_Prospect_ID(Request("TourID"))
        Else
            oPros.Prospect_ID = Request("ProspectID")
        End If
        oPros.Load()
        firstnameTxt.Text = oPros.First_Name
        lastNameTxt.Text = oPros.Last_Name
        mStatus.Selected_ID = oPros.MaritalStatusID
        'If oPros.SpouseID <> 0 Then
        'oProsSpouse.Prospect_ID = oPros.SpouseID
        'oProsSpouse.Load()
        'spouseFN.Text = oProsSpouse.First_Name
        'spouseLN.Text = oProsSpouse.Last_Name
        'End If
        spouseFN.Text = oPros.SpouseFirstName
        spouseLN.Text = oPros.SpouseLastName
        txtFN.Text = oPros.First_Name
        txtLN.Text = oPros.Last_Name
        siMS.Selected_ID = oPros.MaritalStatusID
        hfProsID.Value = oPros.Prospect_ID
        Get_Store_Address()
        Get_Store_Phone()
        Get_Store_Email()
        oProsSpouse = Nothing
        oPros = Nothing
        oTour = Nothing
    End Sub

    Protected Sub Get_Store_Email()
        Dim oEmail As New clsEmail
        Dim oTour As New clsTour
        Dim dt As DataTable
        If Request("TourID") <> "0" Then
            oEmail.ProspectID = oTour.get_Prospect_ID(Request("TourID"))
        Else
            oEmail.ProspectID = Request("ProspectID")
        End If
        dt = oEmail.Get_Table
        Session("Email_Table") = dt
        gvEmail.DataSource = Session("Email_Table")
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvEmail.DataKeyNames = sKeys
        gvEmail.DataBind()
        lblEmailError.Text = oEmail.Error_Message
    End Sub
    Protected Sub Get_Store_Phone()
        Dim oPhone As New clsPhone
        Dim oTour As New clsTour
        Dim dt As DataTable
        If Request("TourID") <> "0" Then
            oPhone.ProspectID = oTour.get_Prospect_ID(Request("TourID"))
        Else
            oPhone.ProspectID = Request("ProspectID")
        End If
        dt = oPhone.Get_Table
        Session("Phone_Table") = dt
        gvPhone.DataSource = Session("Phone_Table")
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvPhone.DataKeyNames = sKeys
        gvPhone.DataBind()
        lblPhoneError.Text = oPhone.Error_Message
    End Sub
    Protected Sub Get_Store_Address()
        Dim oAdd As New clsAddress
        Dim oTour As New clsTour
        Dim dt As DataTable

        If Request("TourID") <> "0" Then
            oAdd.ProspectID = oTour.get_Prospect_ID(Request("TourID"))
        Else
            oAdd.ProspectID = Request("ProspectID")
        End If
        dt = oAdd.Get_Table
        Session("Address_Table") = dt
        gvAddress.DataSource = Session("Address_Table")
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvAddress.DataKeyNames = sKeys
        gvAddress.DataBind()
        lblAddressError.Text = oAdd.Error_Message
    End Sub

    Protected Sub Set_Tour_Values()
        If Request("TourID") <> 0 Then
            Dim oTour As New clsTour
            oTour.TourID = Request("TourID")
            oTour.Load()
            tourIDTxt.Text = oTour.TourID
            tStatus.Selected_ID = oTour.StatusID
            campaignTxt.Text = oTour.get_Campaign(oTour.TourID)
            ckDebit.Checked = oTour.DebitCard
            tTime.Selected_ID = oTour.TourTime
            If oTour.TourDate <> "" Then tDate.Selected_Date = oTour.TourDate
            tLocation.Selected_ID = oTour.TourLocationID
            oTour = Nothing
        End If
    End Sub
    Protected Sub get_Notes()
        Dim dt As New DataTable
        Dim oNotes As New clsNotes
        oNotes.KeyField = "TourID"
        oNotes.KeyValue = Request("TourID")
        dt = oNotes.Get_Notes_Table
        Session("Notes_Table") = dt
        gvNotes.DataSource = Session("Notes_Table")
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvNotes.DataKeyNames = sKeys
        gvNotes.DataBind()
    End Sub
    Protected Sub get_Premiums()
        Dim dt As New DataTable
        Dim oPrem As New clsPremium
        dt = oPrem.List_TourWizard_Premiums(Request("TourID"))
        Session("Premiums_Table") = dt
        gvPremiums.DataSource = Session("Premiums_Table")
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvPremiums.DataKeyNames = sKeys
        gvPremiums.DataBind()
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub Unnamed2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTourOriginal.Click
        If validate_Tour() Then
            If tStatus.SelectedName = "NQ - No Tour" Or tStatus.SelectedName = "NQ - Toured" Or tStatus.SelectedName = "Be Back" Or tStatus.SelectedName = "No Tour" Or tStatus.SelectedName = "Safehouse" Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "popup", "modal.mwindow.open('/crmsnet/wizards/wizeditnote.aspx?NoteID=0&type=" & tStatus.SelectedName & "&subType=Status','win01',350,350);", True)
            Else
                If campaignID.CampaignName = "MAL" Or campaignID.CampaignName = "ART" Or campaignID.CampaignName = "DST-DYD" Or
                    campaignID.CampaignName = "DM-DYD" Or campaignID.CampaignName = "MAL-TS" Or campaignID.CampaignName = "FAITH" Or
                    campaignID.CampaignName = "FAITH-TS" Or campaignID.CampaignName = "G&P-TS" Or
                    campaignID.CampaignName = "JW-OPC" Then
                    If Request("TourID") = "0" Then
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "popup", "modal.mwindow.open('/crmsnet/wizards/wizeditopcrep.aspx','win01',350,350);", True)
                    Else
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & campaignID.CampaignName & "');", True)
                        '                        MultiView1.ActiveViewIndex = 2
                    End If
                Else
                    MultiView1.ActiveViewIndex = 2
                End If
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Make Sure All Fields Are Populated.');", True)
        End If
    End Sub

    Private Function validate_Tour() As Boolean
        Dim verified As Boolean = True
        If Request("TourID") = "0" Then
            If campaignID.Selected_ID = 0 Then
                verified = False
            End If
        End If
        If tStatus.Selected_ID = 0 Or tDate.Selected_Date = "" Or tTime.Selected_ID = 0 Or tLocation.Selected_ID = 0 Then
            verified = False
        End If
        Return verified
    End Function

    Protected Sub lbRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRefresh.Click
        'Update Address View
        If Not (Session("EditAddress") Is Nothing) Then
            Dim dt As DataTable = Session("EditAddress")
            gvAddress.DataSource = dt
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvAddress.DataKeyNames = sKeys
            gvAddress.DataBind()
            Session("Address_Table") = dt
            'Response.Write("Should Be Updated")
        End If

        If Not (Session("EditEmail") Is Nothing) Then
            Dim dt As DataTable = Session("EditEmail")
            gvEmail.DataSource = dt
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvEmail.DataKeyNames = sKeys
            gvEmail.DataBind()
            Session("Email_Table") = dt
        End If

        If Not (Session("EditPhone") Is Nothing) Then
            Dim dt As DataTable = Session("EditPhone")
            gvPhone.DataSource = dt
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvPhone.DataKeyNames = sKeys
            gvPhone.DataBind()
            Session("Phone_Table") = dt
        End If
    End Sub


    Protected Sub btnProsOriginal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProsOriginal.Click
        If validate_Prospect() Then
            MultiView1.ActiveViewIndex = 1
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Make Sure All Fields Are Populated.');", True)
        End If
    End Sub

    Private Function validate_Prospect() As Boolean
        Dim verified As Boolean = True
        If txtLN.Text = "" Or txtFN.Text = "" Or siMS.Selected_ID = "0" Then
            verified = False
        End If
        Return verified
    End Function

    Protected Sub lblRefresh2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblRefresh2.Click
        If Not (Session("EditPremiums") Is Nothing) Then
            Dim dt As DataTable = Session("EditPremiums")
            gvPremiums.DataSource = dt
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvPremiums.DataKeyNames = sKeys
            gvPremiums.DataBind()
            Session("Premiums_Table") = Session("EditPremiums")
            MultiView1.ActiveViewIndex = 2
        End If
    End Sub

    Protected Sub Unnamed3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPremOriginal.Click
        If Not (Session("EditNotes") Is Nothing) Then
            Dim dt As DataTable = Session("EditNotes")
            gvNotes.DataSource = dt
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvNotes.DataKeyNames = sKeys
            gvNotes.DataBind()
            Session("Notes_Table") = dt
        End If
        If tLocation.SelectedName = "VacationClub" Then
            MultiView1.ActiveViewIndex = 7
        Else
            Dim oPers As New clsPersonnel
            ddPersonnelID.Items.Add(New ListItem("NONE", 0))
            ddPersonnelID.DataSource = oPers.List_Sales_Reps(tLocation.SelectedName)
            ddPersonnelID.DataTextField = "Personnel"
            ddPersonnelID.DataValueField = "PersonnelID"
            ddPersonnelID.AppendDataBoundItems = True
            ddPersonnelID.DataBind()
            oPers = Nothing
            MultiView1.ActiveViewIndex = 3
        End If
    End Sub

    Protected Sub Unnamed4_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Submit_Prospect_Info()
        Submit_Address_Info()
        Submit_Tour_Info()
        Submit_Phone_Info()
        Submit_Email_Info()
        Submit_Premium_Info()
        If tLocation.SelectedName <> "VacationClub" Then
            Submit_Personnel_Info()
        End If
        Submit_Notes()
        lblTourID.Text = tourID
        MultiView1.ActiveViewIndex = 6
    End Sub
    Protected Sub Submit_Notes()
        Dim dt As DataTable
        Dim row As DataRow

        If Not (Session("EditNotes") Is Nothing) Then
            dt = Session("EditNotes")
        Else
            dt = Session("Notes_Table")
        End If

        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                row = dt.Rows(i)
                If Not (row("Dirty") Is System.DBNull.Value) And row("Dirty") Then
                    Dim oNotes As New clsNotes
                    oNotes.NoteID = row("ID")
                    oNotes.Note = row("Note")
                    oNotes.KeyField = "TourID"
                    oNotes.KeyValue = tourID
                    oNotes.UserID = row("CreatedByID")
                    oNotes.DateCreated = System.DateTime.Now
                    oNotes.Save()
                    oNotes = Nothing
                End If
            Next
        End If

    End Sub
    Protected Sub Submit_Premium_Info()
        Dim dt As DataTable
        Dim row As DataRow
        If Not (Session("EditPremiums") Is Nothing) Then
            dt = Session("EditPremiums")
        Else
            dt = Session("Premiums_Table")
        End If
        If dt.Rows.Count > 0 Then
            Dim oPrem As New clsPremium
            oPrem.UserID = Session("UserDBID")
            Dim oCombo As New clsComboItems
            For i = 0 To dt.Rows.Count - 1
                row = dt.Rows(i)
                If row("Dirty") & "" <> "" Then
                    If row("Dirty") Then
                        Dim oPremIss As New clsPremiumIssued
                        oPremIss.PremiumIssuedID = row("ID")
                        oPremIss.Load()
                        oPremIss.UserID = Session("UserDBID")
                        oPremIss.PremiumID = row("PremiumID")
                        oPremIss.KeyField = "TourID"
                        oPremIss.KeyValue = tourID
                        If row("ID") < 0 Then
                            oPremIss.CreatedByID = Session("UserDBID")
                            oPrem.PremiumID = row("PremiumID")
                            oPrem.Load()
                            oPremIss.CBCostEA = oPrem.CBCost
                        End If
                        If row("ID") < 0 And row("Status") = "Issued" Then
                            oPrem.PremiumID = row("PremiumID")
                            oPrem.Load()
                            oPrem.QtyOnHand = oPrem.QtyOnHand - row("QtyAssigned")
                            oPremIss.DateIssued = System.DateTime.Now.ToShortDateString
                            oPremIss.IssuedByID = Session("UserDBID")
                            '// Added per work order = 43336; and watched via shadowing Suzzie walking throught the screens to learn the issues
                            '// QtyAssigned is always 0 after clicking submit button
                            oPremIss.QtyAssigned = row("QtyAssigned").ToString()
                            oPrem.Save()
                        ElseIf row("ID") > 0 And row("Status") = "Issued" And oCombo.Lookup_ComboItem(oPremIss.StatusID) <> "Issued" Then
                            oPrem.PremiumID = row("PremiumID")
                            oPrem.Load()
                            oPrem.QtyOnHand = oPrem.QtyOnHand - row("QtyAssigned")
                            oPrem.Save()
                            oPremIss.DateIssued = System.DateTime.Now.ToShortDateString
                            oPremIss.IssuedByID = Session("UserDBID")
                        ElseIf row("ID") > 0 And row("Status") <> "Issued" And oCombo.Lookup_ComboItem(oPremIss.StatusID) = "Issued" Then
                            oPrem.PremiumID = row("PremiumID")
                            oPrem.Load()
                            oPrem.QtyOnHand = oPrem.QtyOnHand + row("QtyAssigned")
                            oPremIss.DateIssued = ""
                            oPremIss.IssuedByID = 0
                            oPrem.Save()
                        ElseIf row("ID") > 0 And row("Status") = "Issued" And oCombo.Lookup_ComboItem(oPremIss.StatusID) = "Issued" And oPremIss.QtyIssued <> row("QtyAssigned") Then
                            Dim diff As Integer
                            diff = row("QtyAssigned") - oPremIss.QtyIssued
                            oPrem.PremiumID = row("PremiumID")
                            oPrem.Load()
                            oPrem.QtyOnHand = oPrem.QtyOnHand - diff
                            oPrem.Save()
                        End If
                        oPremIss.QtyIssued = row("QtyAssigned")
                        oPremIss.CertificateNumber = row("Certificate")
                        oPremIss.StatusID = row("StatusID")
                        oPremIss.CostEA = row("CostEA")
                        oPremIss.TotalCost = CInt(row("QtyAssigned")) * CDbl(row("CostEA"))
                        oPremIss.Save()
                        oPremIss = Nothing
                    End If
                End If
            Next
            oPrem = Nothing
            oCombo = Nothing
        End If

    End Sub
    Protected Sub Submit_Personnel_Info()
        Dim oComboItem As New clsComboItems
        Dim oPersTrans As New clsPersonnelTrans
        Dim oUF As New clsUserFields
        oPersTrans.UserID = Session("UserDBID")
        Dim titleID As Integer
        oUF.ID = oUF.Get_UserField_Value_ID(oUF.Get_UserFieldID(oUF.Get_GroupID("Tour"), "Podium"), tourID)
        oUF.Load()
        oUF.UFID = oUF.Get_UserFieldID(oUF.Get_GroupID("Tour"), "Podium")
        oUF.KeyValue = tourID
        oUF.UFValue = ddPodium.SelectedValue
        oUF.Save()
        oUF = Nothing
        If ddPersonnelID.SelectedValue > 0 Then
            titleID = oComboItem.Lookup_ID("PersonnelTitle", "Sales Executive")
            oPersTrans.Created_By_ID = Session("UserDBID")
            oPersTrans.Personnel_Trans_ID = 0
            oPersTrans.PersonnelID = ddPersonnelID.SelectedItem.Value
            oPersTrans.KeyField = "TourID"
            oPersTrans.KeyValue = tourID
            oPersTrans.TitleID = titleID
            oPersTrans.Save()
        End If
        If Request("TourID") = "0" Then
            If campaignID.CampaignName = "MAL" Or campaignID.CampaignName = "ART" Or campaignID.CampaignName = "DST-DYD" Or campaignID.CampaignName = "DM-DYD" Then
                titleID = oComboItem.Lookup_ID("PersonnelTitle", "Solicitor")
                oPersTrans.Personnel_Trans_ID = 0
                oPersTrans.PersonnelID = Session("OPCRep")
                oPersTrans.KeyField = "TourID"
                oPersTrans.KeyValue = tourID
                oPersTrans.Created_By_ID = Session("UserDBID")
                oPersTrans.TitleID = titleID
                oPersTrans.Save()
            End If
        End If
        oPersTrans = Nothing
        oComboItem = Nothing
    End Sub
    Protected Sub Submit_Prospect_Info()
        Dim oPros As New clsProspect
        Dim oTour As New clsTour
        If Request("TourID") <> "0" Then
            oPros.Prospect_ID = oTour.get_Prospect_ID(Request("TourID"))
        Else
            oPros.Prospect_ID = Request("ProspectID")
        End If
        If Not (oPros.Check_Owner(oPros.Prospect_ID)) Then
            oPros.Load()
            oPros.UserID = Session("UserDBID")
            oPros.First_Name = txtFN.Text
            oPros.Last_Name = txtLN.Text
            oPros.SpouseFirstName = spouseFN.Text
            oPros.SpouseLastName = spouseLN.Text
            oPros.MaritalStatusID = siMS.Selected_ID
            oPros.Save()
            prosID = oPros.Prospect_ID
        End If
        'Save_Spouse()
        oPros = Nothing
        oTour = Nothing
    End Sub
    Protected Sub Save_Spouse()
        Dim oPros As New clsProspect
        Dim oSpouse As New clsProspect

        oPros.Prospect_ID = prosID
        oPros.Load()
        oPros.UserID = Session("UserDBID")
        oSpouse.Prospect_ID = oPros.SpouseID
        oSpouse.Load()
        If oPros.SpouseID <> 0 Then
            oSpouse.First_Name = spouseFN.Text
            oSpouse.Last_Name = spouseLN.Text
            oSpouse.Save()
        Else
            If spouseFN.Text <> "" And spouseLN.Text <> "" Then
                oSpouse.First_Name = spouseFN.Text
                oSpouse.Last_Name = spouseLN.Text
                oSpouse.Save()
                oPros.SpouseID = oSpouse.Prospect_ID
                oPros.Save()
            End If
        End If
        oPros = Nothing
        oSpouse = Nothing
    End Sub

    Protected Sub Submit_Address_Info()
        Dim dt As DataTable
        Dim row As DataRow
        Dim i As Integer = 0
        Dim oPros As New clsProspect
        Dim oTour As New clsTour
        If Request("TourID") <> "0" Then
            oPros.Prospect_ID = oTour.get_Prospect_ID(Request("TourID"))
        Else
            If Request("ProspectID") = "0" Then
                oPros.Prospect_ID = prosID
            Else
                oPros.Prospect_ID = Request("ProspectID")
            End If
        End If
        If Not (Session("EditAddress") Is Nothing) Then
            dt = Session("EditAddress")
        Else
            dt = Session("Address_Table")
        End If
        If dt.Rows.Count > 0 Then
            If Not (oPros.Check_Owner(oPros.Prospect_ID)) Then
                For i = 0 To dt.Rows.Count - 1
                    row = dt.Rows(i)
                    If Not (row("Dirty") Is System.DBNull.Value) And row("Dirty") Then
                        Dim oAdd As New clsAddress
                        oAdd.AddressID = row("ID")
                        oAdd.Load()
                        oAdd.UserID = Session("UserDBID")
                        oAdd.ProspectID = oPros.Prospect_ID
                        oAdd.Address1 = row("Address1")
                        oAdd.Address2 = row("Address2")
                        oAdd.City = row("City")
                        oAdd.StateID = row("StateID")
                        oAdd.PostalCode = row("Zip")
                        oAdd.CountryID = row("CountryID")
                        oAdd.ActiveFlag = row("Active")
                        oAdd.TypeID = row("TypeID")
                        oAdd.Save()
                        oAdd = Nothing
                    End If
                Next
            End If
        End If
        oPros = Nothing
        oTour = Nothing
    End Sub
    Protected Sub Submit_Phone_Info()
        Dim dt As DataTable
        Dim row As DataRow
        Dim i As Integer = 0
        Dim oPros As New clsProspect
        Dim oTour As New clsTour
        If Request("TourID") <> "0" Then
            oPros.Prospect_ID = oTour.get_Prospect_ID(Request("TourID"))
        Else
            If Request("ProspectID") = "0" Then
                oPros.Prospect_ID = prosID
            Else
                oPros.Prospect_ID = Request("ProspectID")
            End If
        End If
        If Not (Session("EditPhone") Is Nothing) Then
            dt = Session("EditPhone")
        Else
            dt = Session("Phone_Table")
        End If
        If dt.Rows.Count > 0 Then
            If Not (oPros.Check_Owner(oPros.Prospect_ID)) Then
                For i = 0 To dt.Rows.Count - 1
                    row = dt.Rows(i)
                    If Not (row("Dirty") Is System.DBNull.Value) And row("Dirty") Then
                        Dim oPhone As New clsPhone
                        oPhone.PhoneID = row("ID")
                        oPhone.Load()
                        oPhone.UserID = Session("UserDBID")
                        oPhone.ProspectID = oPros.Prospect_ID
                        oPhone.Number = row("Number") & ""
                        oPhone.Extension = row("Extension").ToString & ""
                        oPhone.TypeID = row("TypeID")
                        oPhone.Active = row("Active")
                        oPhone.Save()
                        oPhone = Nothing
                    End If
                Next
            End If
        End If
        oPros = Nothing
        oTour = Nothing
    End Sub
    Protected Sub Submit_Email_Info()
        Dim dt As DataTable
        Dim row As DataRow
        Dim i As Integer = 0
        Dim oPros As New clsProspect
        Dim oTour As New clsTour
        If Request("TourID") <> "0" Then
            oPros.Prospect_ID = oTour.get_Prospect_ID(Request("TourID"))
        Else
            If Request("ProspectID") = "0" Then
                oPros.Prospect_ID = prosID
            Else
                oPros.Prospect_ID = Request("ProspectID")
            End If
        End If
        If Not (Session("EditEmail") Is Nothing) Then
            dt = Session("EditEmail")
        Else
            dt = Session("Email_Table")
        End If
        If dt.Rows.Count > 0 Then
            If Not (oPros.Check_Owner(oPros.Prospect_ID)) Then
                For i = 0 To dt.Rows.Count - 1
                    row = dt.Rows(i)
                    If Not (row("Dirty") Is System.DBNull.Value) And row("Dirty") Then
                        Dim oEmail As New clsEmail
                        oEmail.EmailID = row("ID")
                        oEmail.Load()
                        oEmail.UserID = Session("UserDBID")
                        oEmail.ProspectID = oPros.Prospect_ID
                        oEmail.Email = row("Email")
                        oEmail.IsActive = row("Active")
                        oEmail.IsPrimary = row("IsPrimary")
                        oEmail.Save()
                        oEmail = Nothing
                    End If
                Next
            End If
        End If
        oPros = Nothing
        oTour = Nothing
    End Sub

    Protected Sub Submit_Tour_Info()
        Dim oTour As New clsTour
        Dim oCombo As New clsComboItems
        oTour.UserID = Session("UserDBID")
        oTour.TourID = Request("TourID")
        oTour.Load()
        If Request("TourID") = "0" Then
            If Request("ProspectID") = "0" Then
                oTour.ProspectID = prosID
            Else
                oTour.ProspectID = Request("ProspectID")
            End If
        End If
        oTour.TourTime = tTime.Selected_ID
        oTour.StatusID = tStatus.Selected_ID
        oTour.TourDate = tDate.Selected_Date
        oTour.SubTypeID = siTourSubType.Selected_ID
        oTour.DebitCard = ckDebit.Checked
        If Request("TourID") = "0" Then
            oTour.CampaignID = campaignID.Selected_ID
        End If
        'If oCombo.Lookup_ComboItem(siTourSubType.selected_ID) <> "In-House" Then
        oTour.CheckedIn = True
        'End If
        oTour.Save()
        tourID = oTour.TourID
        oTour = Nothing
        oCombo = Nothing
        If Request("TourID") = "0" Then
            If campaignID.CampaignName = "MAL" Or campaignID.CampaignName = "ART" Or campaignID.CampaignName = "DST-DYD" Or campaignID.CampaignName = "DM-DYD" Or campaignID.CampaignName = "JW-OPC" Then
                Dim vendorTour As New clsVendorRep2Tour
                vendorTour.TourID = tourID
                vendorTour.RepID = Session("OPCRep")
                vendorTour.SaleLocID = Session("OPCLocation")
                vendorTour.DateCreated = System.DateTime.Now
                vendorTour.Save()
                vendorTour = Nothing
            End If
        End If
    End Sub
    Protected Sub Unnamed4_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRepOriginal.Click
        If validate_Personnel() Then
            If ddPersonnelID.SelectedItem.Text = "House, House" Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "popup", "modal.mwindow.open('/crmsnet/wizards/wizeditnote.aspx?NoteID=0&type=House&subType=Rep','win01',350,350);", True)
            ElseIf ddPersonnelID.SelectedItem.Text = "NONE" Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "popup", "modal.mwindow.open('/crmsnet/wizards/wizeditnote.aspx?NoteID=0&type=None&subType=Rep','win01',350,350);", True)
            Else
                MultiView1.ActiveViewIndex = 7
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Make Sure All Fields Are Populated.');", True)
        End If
    End Sub
    Private Function validate_Personnel() As Boolean
        Dim verified As Boolean = True
        If siTourSubType.Selected_ID = 0 Then
            verified = False
        End If
        Return verified
    End Function
    Protected Sub Unnamed5_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        btnProsOriginal.Visible = False
        btnProsRedirect.Visible = True
        MultiView1.ActiveViewIndex = 4
    End Sub

    Protected Sub Unnamed6_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        btnTourOriginal.Visible = False
        btnTourRedirect.Visible = True
        If Request("TourID") = "0" Then
            lblCamp.Text = campaignID.CampaignName
        Else
            lblCamp.Text = campaignTxt.Text
        End If
        statusLbl.Text = tStatus.SelectedName
        MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub Unnamed7_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not (Session("EditPremiums") Is Nothing) Then
            Dim dt As DataTable = Session("EditPremiums")
            gvPremiums.DataSource = dt
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvPremiums.DataKeyNames = sKeys
            gvPremiums.DataBind()
            Session("Premiums_Table") = dt
            MultiView1.ActiveViewIndex = 2
        End If
        btnPremOriginal.Visible = False
        btnPremRedirect.Visible = True
        MultiView1.ActiveViewIndex = 2
    End Sub

    Protected Sub Unnamed8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditRep.Click
        repLbl.Text = ddPersonnelID.SelectedItem.Text
        'repLbl.Visible = True
        btnRepOriginal.Visible = False
        btnRepRedirect.Visible = True
        MultiView1.ActiveViewIndex = 3
    End Sub

    Protected Sub Unnamed10_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("todaystours.aspx")
    End Sub

    Protected Sub Unnamed11_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNotesOriginal.Click
        populate_Verify()
        MultiView1.ActiveViewIndex = 5
    End Sub

    Protected Sub lblRefresh3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblRefresh3.Click
        If Not (Session("EditNotes") Is Nothing) Then
            Dim dt As DataTable = Session("EditNotes")
            gvNotes.DataSource = dt
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvNotes.DataKeyNames = sKeys
            gvNotes.DataBind()
            Session("Notes_Table") = Session("EditNotes")
            MultiView1.ActiveViewIndex = 7
        End If

    End Sub

    Protected Sub Unnamed9_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiView1.ActiveViewIndex = 7
    End Sub

    Protected Sub Unnamed3_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblRefresh4.Click
        If Request("TourID") = "0" Then
            If campaignID.CampaignName = "MAL" Or campaignID.CampaignName = "ART" Or campaignID.CampaignName = "DST-DYD" Or campaignID.CampaignName = "DM-DYD" Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "popup", "modal.mwindow.open('/crmsnet/wizards/wizeditopcrep.aspx','win01',350,350);", True)
            Else
                MultiView1.ActiveViewIndex = 2
            End If
        Else
            MultiView1.ActiveViewIndex = 2
        End If
    End Sub

    Protected Sub lblRefresh5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblRefresh5.Click
        If Not (Session("EditNotes") Is Nothing) Then
            Dim dt As DataTable = Session("EditNotes")
            gvNotes.DataSource = dt
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvNotes.DataKeyNames = sKeys
            gvNotes.DataBind()
            Session("Notes_Table") = Session("EditNotes")
            MultiView1.ActiveViewIndex = 7
        End If
    End Sub
    Protected Sub populate_Verify()
        lblName.Text = txtFN.Text + " " + txtLN.Text
        lblSpouse.Text = spouseFN.Text + " " + spouseLN.Text
        If Not (Session("EditAddress") Is Nothing) Then
            gvAddressVal.DataSource = Session("EditAddress")
        Else
            gvAddressVal.DataSource = Session("Address_Table")
        End If
        gvAddressVal.DataBind()
        If Not (Session("EditPhone") Is Nothing) Then
            gvPhoneVal.DataSource = Session("EditPhone")
        Else
            gvPhoneVal.DataSource = Session("Phone_Table")
        End If
        gvPhoneVal.DataBind()
        lblTourTime.Text = tTime.SelectedName
        lblTourDate.Text = tDate.Selected_Date
        lblTourStatus.Text = tStatus.SelectedName
        If Request("TourID") = "0" Then
            lblCampaign.Text = campaignID.CampaignName
        Else
            lblCampaign.Text = campaignTxt.Text
        End If
        If Not (Session("EditPremiums") Is Nothing) Then
            gvPremiumVal.DataSource = Session("EditPremiums")
            gvPremiumVal.DataBind()
            Session("Premiums_Table") = Session("EditPremiums")
        Else
            gvPremiumVal.DataSource = Session("Premiums_Table")
            gvPremiumVal.DataBind()
        End If
        If tLocation.SelectedName = "VacationClub" Then
            lblSalesRep.Text = "N/A"
            lnkEditRep.Text = ""
        Else
            lblSalesRep.Text = ddPersonnelID.SelectedItem.Text
            lblPodium.Text = ddPodium.SelectedItem.Text
        End If
        If Not (Session("EdiNotes") Is Nothing) Then
            gvNotesVal.DataSource = Session("EditNotes")
            gvNotesVal.DataBind()
            Session("Notes_Table") = Session("EditNotes")
        Else
            gvNotesVal.DataSource = Session("Notes_Table")
            gvNotesVal.DataBind()
        End If
    End Sub

    Protected Sub btnTourRedirect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTourRedirect.Click
        If validate_Tour() Then
            If (tStatus.SelectedName = "NQ - No Tour" Or tStatus.SelectedName = "NQ - Toured" Or tStatus.SelectedName = "Be Back" Or tStatus.SelectedName = "No Tour" Or tStatus.SelectedName = "Safehouse") And statusLbl.Text <> tStatus.SelectedName Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "popup", "modal.mwindow.open('/crmsnet/wizards/wizeditnote.aspx?NoteID=0&type=" & tStatus.SelectedName & "&subType=Status&redirect=true&test=" & status & "','win01',350,350);", True)
            Else
                If (campaignID.CampaignName = "G&P-TS" Or campaignID.CampaignName = "FAITH-TS" Or campaignID.CampaignName = "FAITH" Or campaignID.CampaignName = "MAL-TS" Or campaignID.CampaignName = "MAL" Or campaignID.CampaignName = "ART" Or campaignID.CampaignName = "DST-DYD" Or campaignID.CampaignName = "DM-DYD") And lblCamp.Text <> campaignID.CampaignName Then
                    If Request("TourID") = "0" Then
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "popup", "modal.mwindow.open('/crmsnet/wizards/wizeditopcrep.aspx?redirect=true','win01',350,350);", True)
                    Else
                        populate_Verify()
                        MultiView1.ActiveViewIndex = 5
                    End If
                Else
                    populate_Verify()
                    MultiView1.ActiveViewIndex = 5
                End If
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Make Sure All Fields Are Populated.');", True)
        End If
    End Sub

    Protected Sub btnProsRedirect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProsRedirect.Click
        If validate_Prospect() Then
            populate_Verify()
            MultiView1.ActiveViewIndex = 5
        End If
    End Sub

    Protected Sub btnRepRedirect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRepRedirect.Click
        If validate_Personnel() Then
            If ddPersonnelID.SelectedItem.Text = "House, House" And repLbl.Text <> ddPersonnelID.SelectedItem.Text Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "popup", "modal.mwindow.open('/crmsnet/wizards/wizeditnote.aspx?NoteID=0&type=House&subType=Rep','win01',350,350);", True)
            ElseIf ddPersonnelID.SelectedItem.Text = "NONE" And repLbl.Text <> ddPersonnelID.SelectedItem.Text Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "popup", "modal.mwindow.open('/crmsnet/wizards/wizeditnote.aspx?NoteID=0&type=None&subType=Rep','win01',350,350);", True)
            Else
                populate_Verify()
                MultiView1.ActiveViewIndex = 5
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Make Sure All Fields Are Populated.');", True)
        End If
    End Sub

    Protected Sub btnPremRedirect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPremRedirect.Click
        populate_Verify()
        MultiView1.ActiveViewIndex = 5
    End Sub

    Protected Sub btnNoteRedirect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNoteRedirect.Click
        populate_Verify()
        MultiView1.ActiveViewIndex = 5
    End Sub

    Protected Sub lblRefresh6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblRefresh6.Click
        If Request("TourID") = "0" Then
            If (campaignID.CampaignName = "MAL" Or campaignID.CampaignName = "ART" Or campaignID.CampaignName = "DST-DYD" Or campaignID.CampaignName = "DM-DYD") And lblCamp.Text <> campaignID.CampaignName Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "popup", "modal.mwindow.open('/crmsnet/wizards/wizEditOPCRep.aspx?redirect=true','win01',350,350);", True)
            Else
                populate_Verify()
                MultiView1.ActiveViewIndex = 5
            End If
        Else
            populate_Verify()
            MultiView1.ActiveViewIndex = 5
        End If
    End Sub
    Protected Sub lblRefresh6a_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblRefresh6a.Click
        populate_Verify()
        MultiView1.ActiveViewIndex = 5
    End Sub

    Protected Sub lblRefresh4a_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblRefresh4a.Click
        MultiView1.ActiveViewIndex = 2
    End Sub
End Class
