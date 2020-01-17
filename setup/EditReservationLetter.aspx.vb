
Partial Class setup_EditReservationLetter
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oResLetter As New clsReservationLetters
            oResLetter.ReservationLetterID = Request("ID")
            oResLetter.Load()
            txtID.Text = oResLetter.ReservationLetterID
            txtDesc.Text = oResLetter.Description
            cbTour.Checked = oResLetter.BookedTour
            CKEditor1.Text = oResLetter.LetterText
            txtSubject.Text = oResLetter.Subject
            txtEmail.Text = oResLetter.EmailAddress
            siCompany.Connection_String = Resources.Resource.cns
            siCompany.Label_Caption = ""
            siCompany.ComboItem = "ResortCompany"
            siCompany.Selected_ID = oResLetter.ResortCompanyID
            siCompany.Load_Items()
            oResLetter = Nothing
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim oResLetter As New clsReservationLetters
        oResLetter.ReservationLetterID = txtID.Text
        oResLetter.Load()
        oResLetter.Description = txtDesc.Text
        oResLetter.ResortCompanyID = siCompany.Selected_ID
        oResLetter.BookedTour = cbTour.Checked
        oResLetter.LetterText = CKEditor1.Text
        oResLetter.Subject = txtSubject.Text
        oResLetter.EmailAddress = txtEmail.Text
        oResLetter.UserID = Session("UserDBID")
        oResLetter.Save()
        Response.Redirect("EditReservationLetter.aspx?ID=" & oResLetter.ReservationLetterID)
        oResLetter = Nothing
    End Sub

    Protected Sub Source_Link_Click(sender As Object, e As System.EventArgs) Handles Source_Link.Click
        If txtID.Text > 0 Then
            Dim oResSource As New clsReservationLetter2Source
            gvSources.DataSource = oResSource.List_Sources(txtID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvSources.DataKeyNames = sKeys
            gvSources.DataBind()
            MultiView1.ActiveViewIndex = 1
            oResSource = Nothing
        End If
    End Sub

    Protected Sub Letter_Link_Click(sender As Object, e As System.EventArgs) Handles Letter_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub gvSources_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvSources.SelectedIndexChanged
        Dim row As GridViewRow = gvSources.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditReservationLetterSource.aspx?ID=" & row.Cells(1).Text & "','win01',450,450);", True)
    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditReservationLetterSource.aspx?ID=0&LetterID=" & txtID.Text & "','win01',450,450);", True)
    End Sub

    Protected Sub Locations_Link_Click(sender As Object, e As System.EventArgs) Handles Locations_Link.Click
        If txtID.Text > 0 Then
            Dim oRes2Loc As New clsReservationLetter2Location
            gvLocations.DataSource = oRes2Loc.List_Locations(txtID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvLocations.DataKeyNames = sKeys
            gvLocations.DataBind()
            MultiView1.ActiveViewIndex = 2
            oRes2Loc = Nothing
        End If
    End Sub

    Protected Sub gvLocations_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvLocations.SelectedIndexChanged
        Dim row As GridViewRow = gvLocations.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditReservationLetter2Location.aspx?ID=" & row.Cells(1).Text & "','win01',450,450);", True)
    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditReservationLetter2Location.aspx?ID=0&LetterID=" & txtID.Text & "','win01',450,450);", True)
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) Handles LinkButton1.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/ReservationLetterTags.aspx','win01',450,450);", True)

    End Sub
End Class
