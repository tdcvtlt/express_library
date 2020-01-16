Imports System.Data
Partial Class wizards_Contracts_redeedwiz
    Inherits System.Web.UI.Page
    Dim aCoOwners() As Structures.CoOwner
    Dim iCoOwners As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Session("Prospect") = Nothing
            Session("Email_Table") = Nothing
            Session("Phone_Table") = Nothing
            Session("Address_Table") = Nothing
            Session("CoOwners") = Nothing
            Session("iCoOwners") = iCoOwners
            siMortgageTitleType.ComboItem = "MortgageTitleType"
            siMortgageTitleType.Connection_String = Resources.Resource.cns
            siMortgageTitleType.Label_Caption = ""
            siMortgageTitleType.Load_Items()
            Dim i As Integer = 0
            Dim iYear As Integer = Year(Date.Now)
            For i = -2 To 8
                Dim item As New ListItem
                item.Text = iYear + i
                item.Value = iYear + i
                ddOccYear.Items.Add(item)
            Next
            MultiView1.ActiveViewIndex = 0
            'Check to see if existing redeed was passed in
            If IsNumeric(Request("ReDeedID")) Then
                'An Existing one was passed in
                'Load it and fill the boxes
                Dim oReDeed As New clsReDeeds
                oReDeed.ReDeedID = Request("ReDeedID")
                oReDeed.Load()
                txtOldKCP.Text = oReDeed.ReDeededOldKCP
                txtNewKCP.Text = oReDeed.ReDeededNewKCP
                If oReDeed.ReDeedDate <> "" Then dfReDeedDate.Selected_Date = oReDeed.ReDeedDate
                siMortgageTitleType.Selected_ID = oReDeed.TitleTypeID
                For i = 0 To ddOccYear.Items.Count - 1
                    If ddOccYear.Items(i).Value = oReDeed.FirstOccupancyYear Then
                        ddOccYear.SelectedIndex = i
                        Exit For
                    End If
                Next
                For i = 0 To rblDeedType.Items.Count - 1
                    If UCase(rblDeedType.Items(i).Value) = UCase(oReDeed.DeedType) Then
                        rblDeedType.Items(i).Selected = True
                        Exit For
                    End If
                Next
                If UCase(oReDeed.DeedType) = "EXECUTOR" Then
                    txtExecutor.Text = oReDeed.Executor
                    If oReDeed.DeceasedDate <> "" Then dfDeceasedDate.Selected_Date = oReDeed.DeceasedDate
                    pExecutor.Visible = True
                End If
                txtTransferFee.Text = oReDeed.TransferFee
                hfReDeedID.Value = oReDeed.ReDeedID
                If oReDeed.CurrentStep >= 1 Then
                    Dim oContract As New clsContract
                    Dim oPros As New clsProspect
                    Dim oAdd As New clsAddress
                    Dim oPhone As New clsPhone
                    Dim oEmail As New clsEmail
                    Dim oCoOwner As New clsContractCoOwner
                    oContract.ContractID = oReDeed.ReDeededTo
                    oContract.Load()
                    oPros.Prospect_ID = oContract.ProspectID
                    oPros.Load()
                    txtPOLastName.Text = oPros.Last_Name
                    txtPOFirstName.Text = oPros.First_Name
                    txtPOMI.Text = oPros.MiddleInit
                    txtPOSSN.Text = oPros.SSN
                    txtSLastName.Text = oPros.SpouseLastName
                    txtSFirstName.Text = oPros.SpouseFirstName
                    txtSSSN.Text = oPros.SpouseSSN
                    cbSpouseCoOwns.Checked = oCoOwner.Spouse_CoOwns(oContract.ContractID, oPros.Prospect_ID)
                    oEmail.ProspectID = oPros.Prospect_ID
                    oPhone.ProspectID = oPros.Prospect_ID
                    oAdd.ProspectID = oPros.Prospect_ID
                    Session("Address_Table") = oAdd.Get_Table
                    Session("Phone_Table") = oPhone.Get_Table
                    Session("Email_Table") = oEmail.Get_Table
                    gvAddress.DataSource = Session("Address_Table")
                    gvAddress.DataBind()
                    gvEmail.DataSource = Session("Email_Table")
                    gvEmail.DataBind()
                    gvPhone.DataSource = Session("Phone_Table")
                    gvPhone.DataBind()
                    Session("Prospect") = oPros
                    If oCoOwner.Co_Owner_Count(oContract.ContractID, oPros.Prospect_ID) > 0 Then
                        '************BUILD COOWNERS TABLE***********'
                        iCoOwners = oCoOwner.Co_Owner_Count(oContract.ContractID, oPros.Prospect_ID)
                        Session("iCoOwners") = iCoOwners
                        ReDim aCoOwners(iCoOwners)
                        Dim dta As New DataTable
                        dta = oCoOwner.List_ReDeed_CoOwners(oContract.ContractID, oPros.Prospect_ID)
                        i = 0
                        For Each row As DataRow In dta.Rows
                            aCoOwners(i).oPros = New clsProspect
                            aCoOwners(i).oPros.Prospect_ID = row("ProspectID")
                            aCoOwners(i).oPros.Load()
                            aCoOwners(i).oEmail = New clsEmail
                            aCoOwners(i).oEmail.EmailID = row("EmailID")
                            aCoOwners(i).oEmail.Load()
                            aCoOwners(i).oPhone = New clsPhone
                            aCoOwners(i).oPhone.PhoneID = row("PhoneID")
                            aCoOwners(i).oPhone.Load()
                            aCoOwners(i).oAdd = New clsAddress
                            aCoOwners(i).oAdd.AddressID = row("AddressID")
                            aCoOwners(i).oAdd.Load()
                            i += 1
                        Next

                        Session("aCoOwners") = aCoOwners 'oCoOwner.List_ReDeed_CoOwners(oContract.ContractID, oPros.Prospect_ID)
                        'Array.Copy(Session("aCoOwners"), aCoOwners, CInt(Session("iCoOwners")))
                        Dim dtCoOwners As New DataTable
                        dtCoOwners.Columns.Add("ID")
                        dtCoOwners.Columns.Add("FirstName")
                        dtCoOwners.Columns.Add("LastName")
                        Dim dr As DataRow
                        If dta.Rows.Count > 0 Then
                            For i = 0 To UBound(aCoOwners) - 1
                                dr = dtCoOwners.NewRow
                                dr("ID") = i
                                dr("Firstname") = aCoOwners(i).oPros.First_Name
                                dr("LastName") = aCoOwners(i).oPros.Last_Name
                                dtCoOwners.Rows.Add(dr)
                            Next
                        Else
                            Response.Write(oCoOwner.Error_Message)
                        End If
                        Session("CoOwners") = dtCoOwners
                        gvMultiOwner.DataSource = dtCoOwners
                        gvMultiOwner.DataBind()
                        rbMultiOwner.Checked = True
                        mvContractType.ActiveViewIndex = 0
                    ElseIf oContract.TrustFlag Then
                        txtTrust.Text = oContract.TrustName
                        rbTrust.Checked = True
                        mvContractType.ActiveViewIndex = 1
                    ElseIf oContract.CompanyFlag Then
                        txtCompanyName.Text = oContract.CompanyName
                        rbCompanyName.Checked = True
                        mvContractType.ActiveViewIndex = 2
                    Else
                        rbNone.Checked = True
                    End If
                    Dim oInvoices As New clsInvoices
                    gvInvoices.DataSource = oInvoices.List_Transfer_Invoices("ContractID", oReDeed.ReDeededFrom, False)
                    gvInvoices.DataBind()
                    oInvoices = Nothing
                    oPros = Nothing
                    oPhone = Nothing
                    oEmail = Nothing
                    oAdd = Nothing
                    oCoOwner = Nothing
                    oContract = Nothing
                End If
                hfStep.Value = oReDeed.CurrentStep
                MultiView1.ActiveViewIndex = oReDeed.CurrentStep + 1

                oReDeed = Nothing
            End If
        End If
    End Sub

    Protected Sub btnStep1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep1.Click
        If txtOldKCP.ReadOnly Then
            MultiView1.ActiveViewIndex = 1
            Exit Sub
        End If
        Dim oReDeedWiz As New clsReDeedWiz
        Dim oReDeed As New clsReDeeds
        oReDeed.ReDeedID = hfReDeedID.Value
        oReDeed.Load()
        oReDeed.UserID = Session("UserDBID")
        If oReDeedWiz.Check_Contract(txtOldKCP.Text, oReDeed.ReDeededFrom) Then
            'Create a New KCP Number
            Dim sExt As String = ""
            Dim i As Integer = 0
            If InStr(UCase(txtOldKCP.Text), "R") > 0 Then
                If Right(UCase(txtOldKCP.Text), 1) = "R" Then
                    i = 1
                    sExt = UCase(txtOldKCP.Text)
                Else
                    i = CInt(Right(txtOldKCP.Text, Len(txtOldKCP.Text) - InStr(UCase(txtOldKCP.Text), "R")))
                    sExt = Left(UCase(txtOldKCP.Text), InStr(UCase(txtOldKCP.Text), "R"))
                End If
            Else
                i = 0
                sExt = UCase(txtOldKCP.Text) & "R"
            End If
                Do While Not (oReDeedWiz.Validate_ContractNumber(sExt & IIf(i > 0, i, "")))
                    i += 1
                    If i > 100 Then Exit Do 'just to prevent infinite loop.. can increase if needed.
                Loop
                txtNewKCP.Text = sExt & IIf(i > 0, i, "")
                oReDeed.Save()
                hfReDeedID.Value = oReDeed.ReDeedID
                hfStep.Value = 1
                MultiView1.ActiveViewIndex = 1
            Else
                lblStep0Err.Text = oReDeedWiz.Error_Message
            End If
        oReDeed = Nothing
        oReDeedWiz = Nothing
    End Sub

    Protected Sub btnPStep1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPStep1.Click
        MultiView1.ActiveViewIndex = 0
        txtOldKCP.ReadOnly = True
    End Sub

    Protected Sub rblDeedType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblDeedType.SelectedIndexChanged
        pExecutor.Visible = IIf(rblDeedType.SelectedValue = "Executor", True, False)
    End Sub

    Protected Sub btnStep2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep2.Click
        'Check to make sure all is filled in
        Dim sErr As String = ""
        Dim oCombo As New clsComboItems
        If txtNewKCP.Text = "" Then
            sErr = "Please enter a new KCP#\n"
        ElseIf dfReDeedDate.Selected_Date = "" Then
            sErr += "Please select a ReDeed Date.\n"
        ElseIf siMortgageTitleType.Selected_ID < 1 Then
            sErr += "Please select a Title Type.\n"
        ElseIf ddOccYear.SelectedValue < 1 Then
            sErr += "Please select an Occupancy Year"
        ElseIf Not IsNumeric(txtTransferFee.Text) Then
            sErr += "Please enter a Transfer Fee.\n"
        ElseIf rblDeedType.SelectedIndex < 0 Then
            sErr += "Please select a Deed type"
        ElseIf rblDeedType.SelectedValue = "Executor" And txtExecutor.Text = "" Then
            sErr += "Please enter an Executor"
        ElseIf rblDeedType.SelectedValue = "Executor" And dfDeceasedDate.Selected_Date = "" Then
            sErr += "Please enter a Deceased Date."
        End If
        If sErr = "" Then
            Dim oReDeedWiz As New clsReDeedWiz
            Dim oReDeed As New clsReDeeds
            oReDeed.ReDeedID = hfReDeedID.Value
            oReDeed.Load()
            oReDeed.UserID = Session("UserDBID")
            If Request("RedeedID") = "" Then
                oReDeed.CurrentStep = hfStep.Value
            End If
            oReDeed.Save()
            hfReDeedID.Value = oReDeed.ReDeedID
            If oReDeed.ReDeededTo = 0 Then
                'oReDeedWiz.Save_Step2(hfReDeedID.Value, oReDeed.ReDeededFrom, txtNewKCP.Text, ddOccYear.SelectedValue, siMortgageTitleType.Selected_ID, rblDeedType.SelectedValue, Session("UserDBID"), dfReDeedDate.Selected_Date, txtExecutor.Text, txtTransferFee.Text, dfDeceasedDate.Selected_Date)
            Else
                oReDeed.ReDeedDate = dfRedeeddate.Selected_Date
                oReDeed.TransferFee = txtTransferFee.Text
                oReDeed.ReDeedTypeID = oCombo.Lookup_ID("ReDeedType", rblDeedType.SelectedValue)
                If rblDeedType.SelectedValue = "Executor" Then
                    oReDeed.Executor = txtExecutor.Text
                    oReDeed.DeceasedDate = dfDeceasedDate.Selected_Date
                End If
                oReDeed.Save()
            End If
            oReDeed = Nothing
            oReDeedWiz = Nothing
            txtNewKCP.ReadOnly = True
            MultiView1.ActiveViewIndex = 2
            hfStep.Value = 1
            Dim oPros As New clsProspect
            Session("Prospect") = oPros
            oPros = Nothing
            If IsNothing(Session("CoOwners")) Then
                'build CoOwners Table
                Dim dtCoOwners As New DataTable ' = New DataTable
                dtCoOwners.Columns.Add("ID")
                dtCoOwners.Columns.Add("FirstName")
                dtCoOwners.Columns.Add("LastName")
                Session("CoOwners") = dtCoOwners
            End If
        Else
            lblStep1Err.Text = sErr
        End If
    End Sub

    Protected Sub lbRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRefresh.Click
        If Not (Session("Prospect") Is Nothing) Then
            Dim opros = New clsProspect
            opros = Session("Prospect")

            If txtPOLastName.Text <> opros.Last_Name And txtPOFirstName.Text <> opros.First_Name Then
                txtPOFirstName.Text = opros.First_Name
                txtPOLastName.Text = opros.Last_Name
                txtPOMI.Text = opros.MiddleInit
                txtPOSSN.Text = opros.SSN
                txtSFirstName.Text = opros.SpouseFirstName
                txtSLastName.Text = opros.SpouseLastName
                txtSSSN.Text = opros.SpouseSSN
            End If
            gvMultiOwner.DataSource = Session("CoOwners")
            gvMultiOwner.DataBind()
            If opros.Prospect_ID = 0 Then
                gvAddress.DataSource = Session("Address_Table")
                gvAddress.DataBind()
                gvEmail.DataSource = Session("Email_Table")
                gvEmail.DataBind()
                gvPhone.DataSource = Session("Phone_Table")
                gvPhone.DataBind()
            Else
                Dim oAdd As New clsAddress
                Dim oEmail As New clsEmail
                Dim oPhone As New clsPhone
                oAdd.ProspectID = opros.Prospect_ID
                oEmail.ProspectID = opros.Prospect_ID
                oPhone.ProspectID = opros.Prospect_ID
                gvAddress.DataSource = Session("Address_Table") 'oAdd.List
                gvAddress.DataBind()
                gvEmail.DataSource = Session("Email_Table") 'oEmail.List
                gvEmail.DataBind()
                gvPhone.DataSource = Session("Phone_Table") 'oPhone.List
                gvPhone.DataBind()
                oAdd = Nothing
                oEmail = Nothing
                oPhone = Nothing
            End If
            opros = Nothing
        End If
    End Sub

    Protected Sub btnExistingOwner_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExistingOwner.Click
        Dim oCon As New clsContract
        oCon.ContractNumber = txtNewKCP.Text
        oCon.Load()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "none", "javascript:modal.mwindow.open('../../wizards/helpers/selectspouse.aspx?isPri=1&ContractID=" & oCon.ContractID & "','win01',550,350);", True)
        oCon = Nothing
    End Sub

    Protected Sub gvEmail_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvEmail.SelectedIndexChanged

    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim sErr As String = ""
        If txtPOFirstName.Text = "" Then
            sErr += "Please Enter a FirstName."
        ElseIf txtPOLastName.Text = "" Then
            sErr += "Please Enter a LastName."
        ElseIf gvAddress.Rows.Count = 0 Then
            sErr += "Please Enter an Address."
        ElseIf rbMultiOwner.Checked = False And rbTrust.Checked = False And rbCompanyName.Checked = False And rbNone.Checked = False Then
            sErr += "Please Select a Contract Type."
        ElseIf rbTrust.Checked And txtTrust.Text = "" Then
            sErr += "Please Enter a Trust Name."
        ElseIf rbCompanyName.Checked And txtCompanyName.Text = "" Then
            sErr += "Please Enter a Company Name."
        End If
        If sErr = "" Then
            Dim oPros As New clsProspect
            Dim prosID As Integer
            Dim oAdd As New clsAddress
            Dim oPhone As New clsPhone
            Dim oEmail As New clsEmail
            oPros = Session("Prospect")
            oPros.Load()
            oPros.First_Name = txtPOFirstName.Text
            oPros.Last_Name = txtPOLastName.Text
            oPros.MiddleInit = txtPOMI.Text
            oPros.SSN = txtPOSSN.Text
            oPros.SpouseFirstName = txtSFirstName.Text
            oPros.SpouseLastName = txtSLastName.Text
            oPros.SpouseSSN = txtSSSN.Text
            oPros.Save()
            prosID = oPros.Prospect_ID
            For Each row As GridViewRow In gvAddress.Rows
                If row.Cells(15).Text = True Then
                    oAdd.AddressID = row.Cells(1).Text
                    oAdd.Load()
                    oAdd.ProspectID = oPros.Prospect_ID
                    oAdd.ActiveFlag = row.Cells(2).Text
                    oAdd.StateID = row.Cells(6).Text
                    oAdd.CountryID = row.Cells(10).Text
                    oAdd.TypeID = row.Cells(13).text
                    oAdd.ContractAddress = row.Cells(12).Text
                    oAdd.Address1 = row.Cells(3).Text
                    oAdd.Address2 = row.Cells(4).Text
                    oAdd.City = row.Cells(5).Text
                    oAdd.PostalCode = row.cells(8).Text
                    oAdd.Region = row.Cells(9).Text
                    oAdd.Save()
                    row.Cells(1).Text = oAdd.addressid
                End If
            Next
            For Each row As GridViewRow In gvEmail.Rows
                If row.Cells(5).Text = True Then
                    oEmail.EmailID = row.Cells(1).Text
                    oEmail.Load()
                    oEmail.ProspectID = oPros.Prospect_ID
                    oEmail.IsActive = row.Cells(2).Text
                    oEmail.IsPrimary = row.Cells(3).Text
                    oEmail.Email = row.Cells(4).Text
                    oEmail.Save()
                    row.Cells(1).Text = oEmail.emailid
                End If
            Next
            For Each row As GridViewRow In gvPhone.Rows
                If row.Cells(7).Text = True Then
                    oPhone.PhoneID = row.Cells(1).Text
                    oPhone.Load()
                    oPhone.ProspectID = oPros.Prospect_ID
                    oPhone.Active = row.Cells(2).text
                    oPhone.TypeID = row.Cells(5).Text
                    oPhone.Extension = row.Cells(4).Text
                    oPhone.Number = row.Cells(3).Text
                    oPhone.Save()
                    row.Cells(1).Text = oPhone.phoneid
                End If
            Next
            Dim oContract As New clsContract
            Dim oRedeed As New clsReDeeds
            oRedeed.ReDeedID = hfReDeedID.Value
            oRedeed.Load()
            oContract.ContractID = oRedeed.ReDeededTo
            oContract.Load()
            oContract.ProspectID = oPros.Prospect_ID
            Dim iProsID As Integer = 0
            If rbMultiOwner.Checked Then
                If IsNumeric(Session("iCoOwners")) Then
                    If CInt(Session("iCoOwners")) > 0 Then
                        ReDim aCoOwners(CInt(Session("iCoOwners")))
                        'Dim t As Type = Session("aCoOwners").GetType
                        Array.Copy(Session("aCoOwners"), aCoOwners, CInt(Session("iCoOwners")))
                        'aCoOwners = Session("aCoOwners")
                    Else
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If
                'If ProspectID > 0 then existing Prospect

                For i = 0 To UBound(aCoOwners) - 1
                    If aCoOwners(i).oPros.Prospect_ID = 0 Then
                        aCoOwners(i).oPros.Save()
                        aCoOwners(i).oAdd.ProspectID = aCoOwners(i).oPros.Prospect_ID
                        aCoOwners(i).oAdd.Save()
                        aCoOwners(i).oPhone.ProspectID = aCoOwners(i).oPros.Prospect_ID
                        aCoOwners(i).oPhone.Save()
                        aCoOwners(i).oEmail.ProspectID = aCoOwners(i).oPros.Prospect_ID
                        aCoOwners(i).oEmail.Save()
                    End If
                    iProsID = aCoOwners(i).oPros.Prospect_ID
                    Dim oCoOwners As New clsContractCoOwner
                    oCoOwners.ContractID = oRedeed.ReDeededTo
                    oCoOwners.ProspectID = iProsID
                    oCoOwners.Save()
                    oCoOwners = Nothing
                Next
                
            ElseIf rbTrust.Checked Then
                oContract.TrustFlag = True
                oContract.TrustName = txtTrust.Text
            ElseIf rbCompanyName.Checked Then
                oContract.CompanyFlag = True
                oContract.CompanyName = txtCompanyName.Text

            End If
            If cbSpouseCoOwns.Checked Then
                iProsID = oPros.Prospect_ID
                Dim oCoOwners As New clsContractCoOwner
                oCoOwners.ContractID = oRedeed.ReDeededTo
                oCoOwners.ProspectID = iProsID
                oCoOwners.Save()
                oCoOwners = Nothing
            End If
            oContract.Save()
            If Request("RedeedID") = "" Then
                oRedeed.CurrentStep = 2
            End If
            oRedeed.UserID = Session("UserDBID")
            oRedeed.Save()
            'Dim oInvoices As New clsInvoices
            'gvInvoices.DataSource = oInvoices.List_Transfer_Invoices("ContractID", oRedeed.ReDeededFrom, False)
            'gvInvoices.DataBind()
            'oInvoices = Nothing
            oPros = Nothing
            oAdd = Nothing
            oEmail = Nothing
            oPhone = Nothing
            oRedeed = Nothing
            oContract = Nothing
            MultiView1.ActiveViewIndex = 3
            hfStep.Value = 2
        Else
            lblStep2Err.Text = sErr
        End If
    End Sub

    Protected Sub btnSavePrint_Click(sender As Object, e As System.EventArgs) Handles btnSavePrint.Click
        Dim oInvoice As New clsInvoices
        Dim oRedeed As New clsReDeeds
        Dim oContract As New clsContract
        Dim oOldContract As New clsContract
        Dim oSoldInv As New clsSoldInventory
        Dim oPersTrans As New clsPersonnelTrans
        Dim oFinTrans As New clsFinancialTransactionCodes
        Dim oCombo As New clsComboItems
        Dim oPros As New clsProspect
        oRedeed.ReDeedID = hfRedeedID.Value
        oRedeed.Load()
        oOldContract.ContractID = oRedeed.ReDeededFrom
        oOldContract.Load()
        oOldContract.UserID = Session("UserDBID")
        oOldContract.MaintenanceFeeStatusID = 0
        oContract.ContractID = oRedeed.ReDeededTo
        oContract.Load()
        oContract.UserID = Session("UserDBID")
        oContract.ContractDate = dfReDeedDate.Selected_date 'System.DateTime.Now.ToShortDateString
        oPros.Prospect_ID = oContract.ProspectID
        oPros.Load()
        If oPros.First_Name = "Resort" And oPros.Last_Name = "Finance" Then
            oContract.StatusID = oCombo.Lookup_ID("ContractStatus", "On Hold")
            oContract.SubStatusID = oCombo.Lookup_ID("ContractSubStatus", "Resort Finance")
            oContract.MaintenanceFeeStatusID = 0
            oOldContract.StatusID = oCombo.Lookup_ID("ContractStatus", "Canceled")
            oOldContract.SubStatusID = oCombo.Lookup_ID("ContractSubStatus", "Resort Finance")
            oOldContract.MaintenanceFeeStatusID = 0
        ElseIf oPros.First_Name = "King's Creek Plantation" And oPros.Last_Name = "Owner's Association" Then
            oContract.StatusID = oCombo.Lookup_ID("ContractStatus", "On Hold")
            oContract.SubStatusID = oCombo.Lookup_ID("ContractSubStatus", "POA Surrender")
            oContract.MaintenanceFeeStatusID = 0
            oOldContract.StatusID = oCombo.Lookup_ID("ContractStatus", "Canceled")
            oOldContract.SubStatusID = oCombo.Lookup_ID("ContractSubStatus", "POA Surrender")
            oOldContract.MaintenanceFeeStatusID = oCombo.Lookup_ID("MaintenanceFeeStatus", "POA Surrender")
        Else
            oContract.StatusID = oCombo.Lookup_ID("ContractStatus", "Active")
            oContract.SubStatusID = oCombo.Lookup_ID("ContractSubStatus", "Redeed")
            oContract.MaintenanceFeeStatusID = oCombo.Lookup_ID("MaintenanceFeeStatus", "Active")
            oOldContract.StatusID = oCombo.Lookup_ID("ContractStatus", "Canceled")
            oOldContract.SubStatusID = oCombo.Lookup_ID("ContractSubStatus", "Redeed")
            oOldContract.MaintenanceFeeStatusID = 0
        End If
        'For Each row As GridViewRow In gvInvoices.Rows
        '    If CType(row.FindControl("chkTransfer"), CheckBox).Checked Then
        '        oInvoice.InvoiceID = row.Cells(1).Text
        '        oInvoice.Load()
        '        oInvoice.KeyValue = oContract.ContractID
        '        oInvoice.ProspectID = oContract.ProspectID
        '        oInvoice.Save()
        '    End If
        'Next
        oContract.SubTypeID = oOldContract.SubTypeID
        oContract.SeasonID = oOldContract.SeasonID
        oContract.MaintenanceFeeCodeID = oOldContract.MaintenanceFeeCodeID
        oSoldInv.Move_Inventory(oRedeed.ReDeededFrom, oRedeed.ReDeededTo)
        oPersTrans.Created_By_ID = Session("UserDBID")
        oPersTrans.Move_Trans(oRedeed.ReDeededFrom, oRedeed.ReDeededTo, "ContractID")
        If oRedeed.TransferFee > 0 Then
            oInvoice.InvoiceID = 0
            oInvoice.Load()
            oInvoice.Amount = oRedeed.TransferFee
            oInvoice.KeyField = "ContractID"
            oInvoice.KeyValue = oRedeed.ReDeededTo
            oInvoice.Reference = "Transfer Fee"
            oInvoice.UserID = Session("UserDBID")
            oInvoice.TransDate = System.DateTime.Now
            oInvoice.DueDate = System.DateTime.Now.AddDays(10)
            oInvoice.ProspectID = oContract.ProspectID
            oInvoice.FinTransID = oFinTrans.Find_Fin_Trans("ContractTrans", "TransferFee")
            oInvoice.Save()
        End If
        oRedeed.Finalized = True
        oRedeed.FinalizedDate = System.DateTime.Now
        oRedeed.UserID = Session("UserDBID")
        oRedeed.Save()
        oContract.Save()
        oOldContract.Save()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "none", "javascript:modal.mwindow.open('../../general/printcontract.aspx?contractid=" & oRedeed.ReDeededTo & "','win01',550,550);", True)
        oRedeed = Nothing
        oContract = Nothing
        oOldContract = Nothing
        oInvoice = Nothing
        oSoldInv = Nothing
        oPersTrans = Nothing
        oFinTrans = Nothing
    End Sub

    Protected Sub rbMultiOwner_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbMultiOwner.CheckedChanged
        mvContractType.ActiveViewIndex = 0
    End Sub

    Protected Sub rbTrust_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbTrust.CheckedChanged
        mvContractType.ActiveViewIndex = 1
    End Sub

    Protected Sub rbCompanyName_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbCompanyName.CheckedChanged
        mvContractType.ActiveViewIndex = 2
    End Sub

    Protected Sub rbNone_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbNone.CheckedChanged
        mvContractType.ActiveViewIndex = 3
    End Sub

    Protected Sub btnAddCo_Click(sender As Object, e As System.EventArgs) Handles btnAddCo.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "none", "javascript:modal.mwindow.open('../helpers/selectspouse.aspx?isWiz=1','win01',550,550);", True)
    End Sub

    Protected Sub Prev2_Click(sender As Object, e As System.EventArgs) Handles Prev2.Click
        If Request("RedeedID") <> "" Then
            MultiView1.ActiveViewIndex = 1
        Else
            MultiView1.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub btnPrev3_Click(sender As Object, e As System.EventArgs) Handles btnPrev3.Click
        If Request("ReDeedID") <> "" Then
            MultiView1.ActiveViewIndex = 2
        Else
            MultiView1.ActiveViewIndex = 2
        End If
    End Sub

    Protected Sub btnStep3Save_Click(sender As Object, e As System.EventArgs) Handles btnStep3Save.Click
        Dim sErr As String = ""
        If txtPOFirstName.Text = "" Then
            sErr += "Please Enter a FirstName."
        ElseIf txtPOLastName.Text = "" Then
            sErr += "Please Enter a LastName."
        ElseIf gvAddress.Rows.Count = 0 Then
            sErr += "Please Enter an Address."
        ElseIf rbMultiOwner.Checked = False And rbTrust.Checked = False And rbCompanyName.Checked = False And rbNone.Checked = False Then
            sErr += "Please Select a Contract Type."
        ElseIf rbTrust.Checked And txtTrust.Text = "" Then
            sErr += "Please Enter a Trust Name."
        ElseIf rbCompanyName.Checked And txtCompanyName.Text = "" Then
            sErr += "Please Enter a Company Name."
        End If
        If sErr = "" Then
            Dim oPros As New clsProspect
            Dim prosID As Integer
            Dim oAdd As New clsAddress
            Dim oPhone As New clsPhone
            Dim oEmail As New clsEmail
            oPros = Session("Prospect")
            oPros.Load()
            oPros.First_Name = txtPOFirstName.Text
            oPros.Last_Name = txtPOLastName.Text
            oPros.MiddleInit = txtPOMI.Text
            oPros.SSN = txtPOSSN.Text
            oPros.SpouseFirstName = txtSFirstName.Text
            oPros.SpouseLastName = txtSLastName.Text
            oPros.SpouseSSN = txtSSSN.Text
            oPros.Save()
            prosID = oPros.Prospect_ID
            For Each row As GridViewRow In gvAddress.Rows
                If row.Cells(15).Text = True Then
                    oAdd.AddressID = row.Cells(1).Text
                    oAdd.Load()
                    oAdd.ProspectID = oPros.Prospect_ID
                    oAdd.ActiveFlag = row.Cells(2).Text
                    oAdd.StateID = row.Cells(6).Text
                    oAdd.CountryID = row.Cells(10).Text
                    oAdd.TypeID = row.Cells(13).text
                    oAdd.ContractAddress = row.Cells(12).Text
                    oAdd.Address1 = row.Cells(3).Text
                    oAdd.Address2 = row.Cells(4).Text
                    oAdd.City = row.Cells(5).Text
                    oAdd.PostalCode = row.cells(8).Text
                    oAdd.Region = row.Cells(9).Text
                    oAdd.Save()
                    row.Cells(1).Text = oAdd.addressid
                End If
            Next
            For Each row As GridViewRow In gvEmail.Rows
                If row.Cells(5).Text = True Then
                    oEmail.EmailID = row.Cells(1).Text
                    oEmail.Load()
                    oEmail.ProspectID = oPros.Prospect_ID
                    oEmail.IsActive = row.Cells(2).Text
                    oEmail.IsPrimary = row.Cells(3).Text
                    oEmail.Email = row.Cells(4).Text
                    oEmail.Save()
                    row.Cells(1).Text = oEmail.emailid
                End If
            Next
            For Each row As GridViewRow In gvPhone.Rows
                If row.Cells(7).Text = True Then
                    oPhone.PhoneID = row.Cells(1).Text
                    oPhone.Load()
                    oPhone.ProspectID = oPros.Prospect_ID
                    oPhone.Active = row.Cells(2).text
                    oPhone.TypeID = row.Cells(5).Text
                    oPhone.Extension = row.Cells(4).Text
                    oPhone.Number = row.Cells(3).Text
                    oPhone.Save()
                    row.Cells(1).Text = oPhone.phoneid
                End If
            Next
            Dim oContract As New clsContract
            Dim oRedeed As New clsReDeeds
            oRedeed.ReDeedID = hfReDeedID.Value
            oRedeed.Load()
            oContract.ContractID = oRedeed.ReDeededTo
            oContract.Load()
            oContract.ProspectID = oPros.Prospect_ID
            Dim iProsID As Integer = 0
            If rbMultiOwner.Checked Then
                If IsNumeric(Session("iCoOwners")) Then
                    If CInt(Session("iCoOwners")) > 0 Then
                        ReDim aCoOwners(CInt(Session("iCoOwners")))
                        'Dim t As Type = Session("aCoOwners").GetType
                        Array.Copy(Session("aCoOwners"), aCoOwners, CInt(Session("iCoOwners")))
                        'aCoOwners = Session("aCoOwners")
                    Else
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If
                'If ProspectID > 0 then existing Prospect

                For i = 0 To UBound(aCoOwners) - 1
                    If aCoOwners(i).oPros.Prospect_ID = 0 Then
                        aCoOwners(i).oPros.Save()
                    End If
                    iProsID = aCoOwners(i).oPros.Prospect_ID
                    Dim oCoOwners As New clsContractCoOwner
                    oCoOwners.ContractID = oRedeed.ReDeededTo
                    oCoOwners.ProspectID = iProsID
                    oCoOwners.Save()
                    oCoOwners = Nothing
                Next
                
            ElseIf rbTrust.Checked Then
                oContract.TrustFlag = True
                oContract.TrustName = txtTrust.Text
            ElseIf rbCompanyName.Checked Then
                oContract.CompanyFlag = True
                oContract.CompanyName = txtCompanyName.Text

            End If
            If cbSpouseCoOwns.Checked Then
                iProsID = oPros.Prospect_ID
                Dim oCoOwners As New clsContractCoOwner
                oCoOwners.ContractID = oRedeed.ReDeededTo
                oCoOwners.ProspectID = iProsID
                oCoOwners.Save()
                oCoOwners = Nothing
            End If
        oContract.Save()
        If Request("RedeedID") = "" Then
            oRedeed.CurrentStep = 2
        End If
        oRedeed.Save()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "none", "javascript:modal.mwindow.open('../../general/printcontract.aspx?contractid=" & oRedeed.ReDeededTo & "','win01',550,550);", True)
        oPros = Nothing
        oAdd = Nothing
        oEmail = Nothing
        oPhone = Nothing
        oRedeed = Nothing
        oContract = Nothing
        Else
        lblStep2Err.Text = sErr
        End If
    End Sub
End Class
