
Partial Class LeadManagement_EditLeadProgram
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oLPE As New clsLeadProgramEntryForm
            ddEntryForm.DataSource = oLPE.List_Forms()
            ddEntryForm.DataTextField = "Description"
            ddEntryForm.DataValueField = "ID"
            ddEntryForm.DataBind()
            oLPE = Nothing
            Dim oVendor As New clsVendor
            ddVendor.DataSource = oVendor.List_Vendors
            ddVendor.DataTextField = "Vendor"
            ddVendor.DataValueField = "VendorID"
            ddVendor.DataBind()
            oVendor = Nothing
            Dim oLP As New clsLeadProgram
            oLP.LeadProgramID = Request("ID")
            oLP.Load()
            txtID.Text = Request("ID")
            txtDesc.Text = oLP.Description
            txtExeScript.Text = oLP.exeScript
            txtExeScriptTimer.Text = oLP.exetimer
            txtPicInterval.Text = oLP.PictureInterval
            txtRegistration.Text = oLP.registration
            txtScreenSaver.Text = oLP.screensaver
            txtScreenTimer.Text = oLP.screentimer
            txtTermsTimer.Text = oLP.termstimer
            txtURL.Text = oLP.url
            ddEntryForm.SelectedValue = oLP.EntryFormID
            ddVendor.SelectedValue = oLP.VendorID
            cbUsePics.Checked = oLP.UsePictures
            oLP = Nothing
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub LeadProgram_Link_Click(sender As Object, e As EventArgs) Handles LeadProgram_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Locations_Link_Click(sender As Object, e As EventArgs) Handles Locations_Link.Click
        If txtID.Text > 0 Then
            Dim oLP2Loc As New clsLeadProgram2Location
            gvLocations.DataSource = oLP2Loc.List_Locations(txtID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvLocations.DataKeyNames = sKeys
            gvLocations.DataBind()
            MultiView1.ActiveViewIndex = 1
            oLP2Loc = Nothing
        End If
    End Sub

    Protected Sub Images_Link_Click(sender As Object, e As EventArgs) Handles Images_Link.Click
        If txtID.Text > 0 Then
            Dim oLPI As New clsLeadProgram2Image
            gvImages.DataSource = oLPI.List_Images(txtID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvImages.DataKeyNames = sKeys
            gvImages.DataBind()
            MultiView1.ActiveViewIndex = 2
            oLPI = Nothing
        End If
    End Sub

    'Protected Sub HTML_Link_Click(sender As Object, e As EventArgs) Handles HTML_Link.Click
    '    If txtID.Text > 0 Then
    '        Dim oLPI As New clsLeadProgram2HTML
    '        gvURLS.DataSource = oLPI.List_URLS(txtID.Text)
    '        Dim sKeys(0) As String
    '        sKeys(0) = "ID"
    '        gvURLS.DataKeyNames = sKeys
    '        gvURLS.DataBind()
    '        oLPI = Nothing
    '        MultiView1.ActiveViewIndex = 3
    '    End If
    'End Sub


    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim oLP As New clsLeadProgram
        oLP.LeadProgramID = txtID.Text
        oLP.Load()
        If txtID.Text > 0 Then
            If txtDesc.Text <> oLP.Description Or txtExeScript.Text <> oLP.exeScript Or _
                txtExeScriptTimer.Text <> oLP.exetimer Or txtPicInterval.Text <> oLP.PictureInterval Or _
                txtRegistration.Text <> oLP.registration Or txtScreenSaver.Text <> oLP.screensaver Or _
                txtScreenTimer.Text <> oLP.screentimer Or txtTermsTimer.Text <> oLP.termstimer Or _
                txtURL.Text <> oLP.url Or cbUsePics.Checked <> oLP.UsePictures Or ddEntryForm.SelectedValue <> oLP.EntryFormID Then
                oLP.Update_All_Versions(txtID.Text, 0.01)
                'UPdate all versions    
            End If
        End If
        oLP.Description = txtDesc.Text
        oLP.exeScript = txtExeScript.Text
        oLP.exetimer = txtExeScriptTimer.Text
        oLP.PictureInterval = txtPicInterval.Text
        oLP.registration = txtRegistration.Text
        oLP.screensaver = txtScreenSaver.Text
        oLP.screentimer = txtScreenTimer.Text
        oLP.termstimer = txtTermsTimer.Text
        oLP.url = txtURL.Text
        oLP.UsePictures = cbUsePics.Checked
        oLP.EntryFormID = ddEntryForm.SelectedValue
        oLP.VendorID = ddVendor.SelectedValue
        oLP.Save()
        Response.Redirect("EditLeadProgram.aspx?ID=" & oLP.LeadProgramID)
        oLP = Nothing
    End Sub

    Protected Sub gvLocations_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvLocations.SelectedIndexChanged
        Dim row As GridViewRow = gvLocations.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/LeadManagement/editLPLocation.aspx?ID=" & row.Cells(1).Text & "','win01',450,450);", True)
    End Sub

    Protected Sub gvImages_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvImages.SelectedIndexChanged
        Dim row As GridViewRow = gvImages.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/LeadManagement/editLPImage.aspx?ID=" & row.Cells(1).Text & "','win01',450,450);", True)
    End Sub

    Protected Sub gvURLS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvURLS.SelectedIndexChanged
        Dim row As GridViewRow = gvURLS.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/LeadManagement/editLPHTML.aspx?ID=" & row.Cells(1).Text & "','win01',450,450);", True)
    End Sub

    Protected Sub lbNewLoc_Click(sender As Object, e As EventArgs) Handles lbNewLoc.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/LeadManagement/editLPLocation.aspx?ID=0&LPID=" & txtID.Text & "','win01',450,450);", True)
    End Sub

    Protected Sub lbNewImage_Click(sender As Object, e As EventArgs) Handles lbNewImage.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/LeadManagement/editLPImage.aspx?ID=0&LPID=" & txtID.Text & "','win01',450,450);", True)
    End Sub

    Protected Sub lbNewURL_Click(sender As Object, e As EventArgs) Handles lbNewURL.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/LeadManagement/editLPHTML.aspx?ID=0&LPID=" & txtID.Text & "','win01',450,450);", True)
    End Sub

    Protected Sub Devices_Link_Click(sender As Object, e As EventArgs) Handles Devices_Link.Click
        If txtID.Text > 0 Then
            Dim oLPD As New clsLeadProgram2Device
            gvDevices.DataSource = oLPD.List_Devices(txtID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvDevices.DataKeyNames = sKeys
            gvDevices.DataBind()
            oLPD = Nothing
            MultiView1.ActiveViewIndex = 4
        End If
    End Sub

    Protected Sub gvDevices_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvDevices.SelectedIndexChanged
        Dim row As GridViewRow = gvDevices.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/LeadManagement/editLPDevice.aspx?ID=" & row.Cells(1).Text & "','win01',450,450);", True)
    End Sub

    Protected Sub lbNewDevice_Click(sender As Object, e As EventArgs) Handles lbNewDevice.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/LeadManagement/editLPDevice.aspx?ID=0&LPID=" & txtID.Text & "','win01',450,450);", True)
    End Sub


End Class
