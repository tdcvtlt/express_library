
Partial Class mis_EditITSecRequest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oSecRequest As New clsPersonnelSecurityRequest
            Dim oPers As New clsPersonnel
            Dim oCombo As New clsComboItems
            oSecRequest.RequestID = Request("ID")
            oSecRequest.Load()
            oPers.PersonnelID = oSecRequest.PersonnelID
            oPers.Load()
            lblPersonnel.Text = oPers.FirstName & " " & oPers.LastName
            lblRequest.Text = oCombo.Lookup_ComboItem(oSecRequest.TypeID)
            siStatus.Label_Caption = ""
            siStatus.Connection_String = Resources.Resource.cns
            siStatus.ComboItem = "ITSecRequestStatus"
            siStatus.Load_Items()
            siStatus.Selected_ID = oSecRequest.StatusID
            lbDateCreated.Text = CDate(oSecRequest.DateCreated).ToShortDateString
            lbDueDate.Text = CDate(oSecRequest.RequestedDueDate).ToShortDateString
            oPers.PersonnelID = oSecRequest.RequestedByID
            oPers.Load()
            lblRequestedBy.Text = oPers.FirstName & " " & oPers.LastName
            If oCombo.Lookup_ComboItem(oSecRequest.TypeID) = "Disable User" Then
                lbCRMSGroups.Text = "N/A"
                lblDistGroups.Text = "N/A"
                lblDomain.Text = "N/A"
                lblWindowsGroups.Text = "N/A"
            Else
                Dim oSecGroups As New clsPersonnelSecurityRequest2Group
                lbCRMSGroups.Text = oSecGroups.Get_Group_Display(oSecRequest.RequestID, "CRMS")
                lblWindowsGroups.Text = oSecGroups.Get_Group_Display(oSecRequest.RequestID, "Windows")
                If oSecRequest.EmailOption = 0 Then
                    lblDistGroups.Text = "N/A"
                    lblDomain.Text = "N/A"
                ElseIf oSecRequest.EmailOption = 1 Then
                    lblDistGroups.Text = "N/A"
                    lblDomain.Text = oCombo.Lookup_ComboItem(oSecRequest.EmailDomainID)
                ElseIf oSecRequest.EmailOption = 2 Then
                    lblDomain.Text = "N/A"
                    lblDistGroups.Text = oSecGroups.Get_Group_Display(oSecRequest.RequestID, "Distribution")
                Else
                    lblDomain.Text = oCombo.Lookup_ComboItem(oSecRequest.EmailDomainID)
                    lblDistGroups.Text = oSecGroups.Get_Group_Display(oSecRequest.RequestID, "Distribution")
                End If
                oSecGroups = Nothing
            End If
            lblPhone.Text = oCombo.Lookup_ComboItem(oSecRequest.PhoneTypeID)
            If oSecRequest.DID = False Then
                lblDID.Text = "No"
            Else
                lblDID.Text = "Yes"
            End If
            oSecRequest = Nothing
            oCombo = Nothing
            oPers = Nothing
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim oSecRequest As New clsPersonnelSecurityRequest
        Dim oPersonnel As New clsPersonnel
        Dim oCombo As New clsComboItems
        Dim sBody As String = ""
        oSecRequest.RequestID = Request("ID")
        oSecRequest.Load()
        oSecRequest.UserID = Session("UserDBID")
        oSecRequest.StatusID = siStatus.Selected_ID
        oSecRequest.Save()
        oPersonnel.PersonnelID = oSecRequest.RequestedByID
        oPersonnel.Load()
        sBody = sBody & "RequestID: " & oSecRequest.RequestID
        sBody = sBody & "Personnel: " & lblPersonnel.Text & "<br>"
        sBody = sBody & "Request Type: " & lblRequest.Text & "<br>"
        sBody = sBody & "Status: " & oCombo.Lookup_ComboItem(siStatus.Selected_ID) & "<br>"
        Send_Mail(oPersonnel.Email, "MISDept@kingscreekplantation.com", "Security Request", sBody, True)
        oCombo = Nothing
        oPersonnel = Nothing
        oSecRequest = Nothing
        Response.Redirect("ITSecRequests.aspx")
    End Sub
End Class
