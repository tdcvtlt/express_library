Imports System.Data
Imports System.Reflection
Imports System.Globalization

Imports clsReservationWizard
Imports System.Web.Script.Serialization

Partial Class wizard_Reservations_Prospect
    Inherits System.Web.UI.Page

    Private package_base As New Base_Package
    Private wiz As New Wizard

#Region "Subs/Functions"

    Private Sub Navigate(bt As Button)
        Dim gt = Me.Master.GetType()
        Dim mi As MethodInfo = gt.GetMethod("Navigate")
        Dim parameters = mi.GetParameters()

        Dim dir = Convert.ToInt32(bt.Attributes("nav"))
        Dim parametersArray() As Object = New Object() {Request.PhysicalPath.Substring(Request.PhysicalPath.LastIndexOf("\") + 1).ToLower(), dir}
        Dim r = mi.Invoke(Me.Master, parametersArray)
        Response.Redirect(r.ToString())
    End Sub


    Private Sub Form_Refresh()

        Dim addressesRef As Func(Of Boolean) =
            Function()

                Dim dt = New DataTable
                dt.Columns.Add("ID", GetType(Int32))
                dt.Columns.Add("Address1", GetType(String))
                dt.Columns.Add("Address2", GetType(String))
                dt.Columns.Add("City", GetType(String))
                dt.Columns.Add("State", GetType(String))
                dt.Columns.Add("Zip", GetType(String))
                dt.Columns.Add("Active", GetType(Boolean))

                ddCountry.ClearSelection()

                For Each e In wiz.Prospect.Addresses
                    Dim r = dt.NewRow
                    r("ID") = e.AddressID
                    r("Address1") = e.Address1
                    r("Address2") = e.Address2
                    r("City") = e.City
                    r("State") = New clsComboItems().Lookup_ComboItem(e.StateID)
                    r("Zip") = e.PostalCode
                    r("Active") = e.ActiveFlag

                    dt.Rows.Add(r)
                Next
                gridview3.DataSource = dt
                gridview3.DataBind()

                Return True

            End Function

        Dim emailRef As Func(Of Boolean) =
            Function()

                Dim dt = New DataTable
                dt.Columns.Add("ID", GetType(Int32))
                dt.Columns.Add("Email", GetType(String))
                dt.Columns.Add("Primary", GetType(Boolean))
                dt.Columns.Add("Active", GetType(Boolean))

                For Each e In wiz.Prospect.Emails
                    Dim r = dt.NewRow
                    r("ID") = e.EmailID
                    r("Email") = e.Email.Trim()
                    r("Primary") = e.IsPrimary
                    r("Active") = e.IsActive
                    dt.Rows.Add(r)
                Next
                gridview1.DataSource = dt
                gridview1.DataBind()

                Return True

            End Function

        Dim phonesRef As Func(Of Boolean) =
            Function()

                Dim dt = New DataTable
                dt.Columns.Add("ID", GetType(Int32))
                dt.Columns.Add("Number", GetType(String))
                dt.Columns.Add("Extension", GetType(String))
                dt.Columns.Add("Type", GetType(String))
                dt.Columns.Add("Active", GetType(Boolean))

                For Each e In wiz.Prospect.Phones
                    Dim r = dt.NewRow
                    r("ID") = e.PhoneID
                    r("Number") = e.Number
                    r("Extension") = e.Extension
                    r("Type") = New clsComboItems().Lookup_ComboItem(e.TypeID)
                    r("Active") = e.Active
                    dt.Rows.Add(r)
                Next
                gridview2.DataSource = dt
                gridview2.DataBind()

                Return True

            End Function

        addressesRef()
        emailRef()
        phonesRef()

    End Sub

    Private Sub Address_Save(addressID As Int32)

        Dim address = wiz.Prospect.Addresses.Where(Function(x) x.AddressID = addressID).SingleOrDefault()

        If address Is Nothing Then
            address = New clsAddress()
            address.AddressID = -wiz.Prospect.Addresses.Count() - 1
            wiz.Prospect.Addresses.Add(address)
        End If

        With address
            .ActiveFlag = cbActive_A.Checked
            .Address1 = New CultureInfo("en-US").TextInfo.ToTitleCase(txAddress1.Text.Trim())
            .Address2 = New CultureInfo("en-US").TextInfo.ToTitleCase(txAddress2.Text.Trim())
            .City = New CultureInfo("en-US").TextInfo.ToTitleCase(txCity.Text.Trim())
            .PostalCode = txZip.Text.Trim()
            .Region = New CultureInfo("en-US").TextInfo.ToTitleCase(txRegion.Text.Trim())
            .StateID = ddState.SelectedItem.Value

            If ddCountry.SelectedItem.Value.Length > 0 Then
                .CountryID = ddCountry.SelectedItem.Value
            End If
            If ddType_A.SelectedItem.Value.Length > 0 Then
                .TypeID = ddType_A.SelectedItem.Value
            End If

            .ContractAddress = cbContractAddress.Checked
        End With

        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)

        Form_Refresh()

    End Sub
    Private Sub Email_Save(emailID As Int32)

        Dim email = wiz.Prospect.Emails.Where(Function(x) x.EmailID = emailID).SingleOrDefault()

        If email Is Nothing Then
            email = New clsEmail()
            email.EmailID = -wiz.Prospect.Emails.Count() - 1
            wiz.Prospect.Emails.Add(email)
        End If

        With email
            .Email = txEmail.Text.Trim()
            .IsActive = cbActive_E.Checked
            .IsPrimary = cbPrimary_E.Checked
        End With
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Form_Refresh()
    End Sub
    Private Sub Phone_Save(phoneID As Int32)

        Dim phone = wiz.Prospect.Phones.Where(Function(x) x.PhoneID = phoneID).SingleOrDefault()

        If phone Is Nothing Then
            phone = New clsPhone()
            phone.PhoneID = -wiz.Prospect.Phones.Count() - 1
            wiz.Prospect.Phones.Add(phone)
        End If

        With phone
            .Number = txNumber.Text.Trim()
            .Extension = txExtension.Text.Trim()
            .Active = cbActive_P.Checked

            If ddType_P.SelectedItem.Value.Length > 0 Then
                .TypeID = ddType_P.SelectedItem.Value
            End If
        End With

        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Form_Refresh()
    End Sub

    Private Sub Address_Show(addressID As Int32)
        Dim ClearForm As Func(Of Boolean) =
            Function()

                txAddressID.Text = ""
                txProspectID_A.Text = ""
                txAddress1.Text = ""
                txAddress2.Text = ""
                cbActive_A.Checked = False
                cbContractAddress.Checked = False
                txCity.Text = ""
                txZip.Text = ""
                txRegion.Text = ""
                ddState.ClearSelection()
                ddCountry.ClearSelection()
                ddType_A.ClearSelection()

                Return True
            End Function

        ClearForm()

        Dim address = wiz.Prospect.Addresses.Where(Function(x) x.AddressID = addressID).Single()
        With address
            txAddressID.Text = .AddressID
            txProspectID_A.Text = wiz.Prospect.Prospect_ID
            txAddress1.Text = .Address1
            txAddress2.Text = .Address2
            cbActive_A.Checked = .ActiveFlag
            cbContractAddress.Checked = .ContractAddress
            txCity.Text = .City
            txZip.Text = .PostalCode
            txRegion.Text = .Region

            ddState.ClearSelection()
            ddCountry.ClearSelection()
            ddType_A.ClearSelection()

            If .StateID > 0 Then
                ddState.Items.FindByValue(.StateID).Selected = True
            End If

            If .CountryID > 0 Then
                ddCountry.Items.FindByValue(.CountryID).Selected = True
            End If
            If .TypeID > 0 Then
                ddType_A.Items.FindByValue(.TypeID).Selected = True
            End If

        End With
    End Sub
    Private Sub Email_Show(emailID As Int32)

        Dim ClearForm As Func(Of Boolean) =
            Function()

                txEmailID.Text = ""
                txProspectID_E.Text = ""
                txEmail.Text = ""
                cbActive_E.Checked = False
                cbPrimary_E.Checked = False

                Return True
            End Function

        ClearForm()

        Dim email = wiz.Prospect.Emails.Where(Function(x) x.EmailID = emailID).Single()
        With email
            txEmailID.Text = .EmailID
            txProspectID_E.Text = wiz.Prospect.Prospect_ID
            txEmail.Text = .Email
            cbActive_E.Checked = .IsActive
            cbPrimary_E.Checked = .IsPrimary
        End With
    End Sub
    Private Sub Phone_Show(phoneID As Int32)

        Dim ClearForm As Func(Of Boolean) =
            Function()

                txPhoneID.Text = ""
                txProspectID_P.Text = ""
                txNumber.Text = ""
                txExtension.Text = ""
                ddType_P.ClearSelection()
                cbActive_P.Checked = False

                Return True
            End Function

        ClearForm()


        Dim phone = wiz.Prospect.Phones.Where(Function(x) x.PhoneID = phoneID).Single()
        With phone
            txPhoneID.Text = .PhoneID
            txProspectID_P.Text = wiz.Prospect.Prospect_ID
            txNumber.Text = .Number
            txExtension.Text = .Extension
            If .TypeID > 0 Then
                ddType_P.Items.FindByValue(.TypeID).Selected = True
            End If

            cbActive_P.Checked = .Active
        End With
    End Sub

    Private Sub Prospect_Save()

        With wiz.Prospect
            .First_Name = New CultureInfo("en-US").TextInfo.ToTitleCase(txFirstName.Text.Trim())
            .Last_Name = New CultureInfo("en-US").TextInfo.ToTitleCase(txLastName.Text.Trim())

            .MaritalStatusID = ddMarital.SelectedItem.Value
            .SpouseFirstName = New CultureInfo("en-US").TextInfo.ToTitleCase(txSpouseFirstName.Text.Trim())
            .SpouseLastName = New CultureInfo("en-US").TextInfo.ToTitleCase(txSpouseLastName.Text.Trim())
        End With
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
    End Sub

    Private Sub Prospect_Show()
        With wiz.Prospect
            txProspectID.Text = .Prospect_ID
            txFirstName.Text = .First_Name
            txLastName.Text = .Last_Name

            txSpouseFirstName.Text = .SpouseFirstName
            txSpouseLastName.Text = .SpouseLastName

            ddMarital.ClearSelection()

            Dim m_s = 0

            If Int32.TryParse(.MaritalStatusID, m_s) Then
                If Not ddMarital.Items.FindByValue(m_s) Is Nothing Then
                    ddMarital.Items.FindByValue(m_s).Selected = True
                End If
            End If
        End With
        Form_Refresh()
    End Sub
#End Region
#Region "Event Handlers"
    Private Sub wizard_Reservations_Prospect_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim ph = CType(Me.Master.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        Dim wiz_data = CType(ph.FindControl("LB_WIZ_DATA"), Label)

        If Session("wizData" + Session("wizGuid")) <> Nothing Then
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(Session("wizData" + Session("wizGuid")))
        Else
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(wiz_data.Text)
        End If
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Session("wizGuid") = wiz.GUID_TIMESTAMP


        Dim InitDefaults As Func(Of Boolean) =
            Function()

                With ddMarital
                    .AppendDataBoundItems = True
                    .Items.Add(New ListItem("(Empty)", ""))
                    .DataSource = New clsComboItems().Load_ComboItems("MaritalStatus")
                    .DataTextField = "ComboItem"
                    .DataValueField = "ComboItemID"
                    .DataBind()
                End With

                With ddType_P
                    .AppendDataBoundItems = True
                    .Items.Add(New ListItem("(Empty)", ""))
                    .DataSource = New clsComboItems().Load_ComboItems("Phone")
                    .DataTextField = "ComboItem"
                    .DataValueField = "ComboItemID"
                    .DataBind()
                End With

                With ddCountry
                    .AppendDataBoundItems = True
                    .Items.Add(New ListItem("(Empty)", ""))
                    .DataSource = New clsComboItems().Load_ComboItems("Country")
                    .DataTextField = "ComboItem"
                    .DataValueField = "ComboItemID"
                    .DataBind()
                End With

                With ddState
                    .AppendDataBoundItems = True
                    .Items.Add(New ListItem("(Empty)", ""))
                    .DataSource = New clsComboItems().Load_ComboItems("State")
                    .DataTextField = "ComboItem"
                    .DataValueField = "ComboItemID"
                    .DataBind()
                End With
                With ddType_A
                    .AppendDataBoundItems = True
                    .Items.Add(New ListItem("(Empty)", ""))
                    .DataSource = New clsComboItems().Load_ComboItems("AddressType")
                    .DataTextField = "ComboItem"
                    .DataValueField = "ComboItemID"
                    .DataBind()
                End With

                Return True
            End Function

        If IsPostBack = False Then

            InitDefaults()
            Prospect_Show()
            Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        End If
    End Sub

    Protected Sub btNext_Click(sender As Object, e As EventArgs) Handles btNext.Click

        Prospect_Save()

        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 1)
        Navigate(bt)
    End Sub
    Protected Sub btPrevious_Click(sender As Object, e As EventArgs) Handles btPrevious.Click
        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 0)
        Navigate(CType(sender, Button))
    End Sub
    Protected Sub btCancel_E_Click(sender As Object, e As EventArgs) Handles btCancel_E.Click
        multiview1.SetActiveView(view1)
    End Sub
    Protected Sub btSubmit_E_Click(sender As Object, e As EventArgs) Handles btSubmit_E.Click
        Email_Save(txEmailID.Text)
        multiview1.SetActiveView(view1)
    End Sub
    Protected Sub lbAddEmail_Click(sender As Object, e As EventArgs) Handles lbAddEmail.Click

        Dim ClearForm As Func(Of Boolean) =
            Function()

                txEmailID.Text = ""
                txProspectID_E.Text = ""
                txEmail.Text = ""
                cbActive_E.Checked = False
                cbPrimary_E.Checked = False

                Return True
            End Function

        ClearForm()

        txEmailID.Text = 0
        txProspectID_E.Text = wiz.Prospect.Prospect_ID
        multiview1.SetActiveView(view2)
    End Sub
    Protected Sub lbAddPhone_Click(sender As Object, e As EventArgs) Handles lbAddPhone.Click
        Dim ClearForm As Func(Of Boolean) =
            Function()

                txPhoneID.Text = ""
                txProspectID_P.Text = ""
                txNumber.Text = ""
                txExtension.Text = ""
                ddType_P.ClearSelection()
                cbActive_P.Checked = False

                Return True
            End Function

        ClearForm()
        txPhoneID.Text = 0
        txProspectID_P.Text = wiz.Prospect.Prospect_ID
        multiview1.SetActiveView(view3)
    End Sub
    Protected Sub lbAddAddress_Click(sender As Object, e As EventArgs) Handles lbAddAddress.Click
        Dim ClearForm As Func(Of Boolean) =
            Function()

                txAddressID.Text = ""
                txProspectID_A.Text = ""
                txAddress1.Text = ""
                txAddress2.Text = ""
                cbActive_A.Checked = False
                cbContractAddress.Checked = False
                txCity.Text = ""
                txZip.Text = ""
                txRegion.Text = ""
                ddState.ClearSelection()
                ddCountry.ClearSelection()
                ddType_A.ClearSelection()

                Return True
            End Function

        ClearForm()

        txAddressID.Text = 0
        txProspectID_A.Text = wiz.Prospect.Prospect_ID
        multiview1.SetActiveView(view4)
    End Sub

    Protected Sub btCancel_A_Click(sender As Object, e As EventArgs) Handles btCancel_A.Click
        multiview1.SetActiveView(view1)
    End Sub
    Protected Sub btSubmit_A_Click(sender As Object, e As EventArgs) Handles btSubmit_A.Click
        Address_Save(txAddressID.Text)
        multiview1.SetActiveView(view1)
    End Sub
    Protected Sub btCancel_P_Click(sender As Object, e As EventArgs) Handles btCancel_P.Click
        multiview1.SetActiveView(view1)
    End Sub
    Protected Sub btSubmit_P_Click(sender As Object, e As EventArgs) Handles btSubmit_P.Click
        Phone_Save(txPhoneID.Text)
        multiview1.SetActiveView(view1)
    End Sub
    Protected Sub gridview1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview1.RowCommand
        Email_Show(e.CommandArgument)
        multiview1.SetActiveView(view2)
    End Sub
    Protected Sub gridview2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview2.RowCommand
        Phone_Show(e.CommandArgument)
        multiview1.SetActiveView(view3)
    End Sub
    Protected Sub gridview3_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview3.RowCommand
        Address_Show(e.CommandArgument)
        multiview1.SetActiveView(view4)
    End Sub
#End Region



End Class
