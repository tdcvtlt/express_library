
Partial Class LeadManagement_EditLead
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            ddVendors.DataSource = (New clsVendor).List_Vendors
            ddVendors.DataTextField = "Vendor"
            ddVendors.DataValueField = "VendorID"
            ddVendors.DataBind()
            If Request("LeadID") <> "" And Request("LeadID") <> "0" And Request("LeadID") <> 0 Then
                Dim oLead As New clsLeads
                oLead.LeadID = Request("LeadID")
                oLead.Load()
                txtLeadID.Text = oLead.LeadID
                txtFName.Text = oLead.FirstName
                txtLName.Text = oLead.LastName
                txtSpouse.Text = oLead.SpouseName
                txtAddress1.Text = oLead.Address1
                txtAddress2.Text = oLead.Address2
                txtCity.Text = oLead.City
                txtState.Text = oLead.State
                txtZip.Text = oLead.PostalCode
                txtPhone.Text = oLead.PhoneNumber
                txtEmail.Text = oLead.EmailAddress
                txtAge.Text = oLead.Age
                txtMS.Text = oLead.MaritalStatus
                txtIncome.Text = oLead.IncomeRange
                txtOwn.Text = oLead.OwnRent
                ckSigned.Checked = oLead.Signed
                txtFileID.Text = oLead.LeadFileID
                txtSource.Text = oLead.Source
                dteSigned.Selected_Date = oLead.SignDate
                txtDateEntered.Text = oLead.DateEntered
                txtMF.Text = oLead.MaleFemale
                ddVendors.SelectedValue = oLead.VendorID
                oLead = Nothing
            End If
        End If
    End Sub

    Protected Sub lbSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSave.Click
        If Not (CheckSecurity("Leads", "Save", , , Session("UserDBID")) And txtLeadID.Text.ToString <> "0") Or _
            (CheckSecurity("Leads", "Add", , , Session("UserDBID")) And txtLeadID.Text.ToString = "0") Then
            Dim oLead As New clsLeads
            With oLead
                .LeadID = IIf(txtLeadID.Text = "", 0, txtLeadID.Text)
                .Load()
                .FirstName = txtFName.Text
                .LastName = txtLName.Text
                .SpouseName = txtSpouse.Text
                .Address1 = txtAddress1.Text
                .Address2 = txtAddress2.Text
                .City = txtCity.Text
                .State = txtState.Text
                .PostalCode = txtZip.Text
                .PhoneNumber = txtPhone.Text
                .EmailAddress = txtEmail.Text
                .Age = txtAge.Text
                .MaritalStatus = txtMS.Text
                .IncomeRange = txtIncome.Text
                .OwnRent = txtOwn.Text
                .Signed = ckSigned.Checked
                .LeadFileID = IIf(txtFileID.Text = "", 0, txtFileID.Text)
                .SignDate = dteSigned.Selected_Date
                .Source = txtSource.Text
                .DateEntered = IIf(.LeadID = 0, Date.Now, .DateEntered)
                .MaleFemale = txtMF.Text
                .VendorID = ddVendors.SelectedValue
                .Save()
                txtLeadID.Text = .LeadID
            End With
            oLead = Nothing
            Response.Redirect("editlead.aspx?leadid=" & txtLeadID.Text)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AD", "alert('Access Denied');", True)
        End If
    End Sub

    Protected Sub lbCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCancel.Click
        Response.Redirect("Leads.aspx")
    End Sub
End Class
