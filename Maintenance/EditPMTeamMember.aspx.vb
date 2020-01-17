
Partial Class Maintenance_EditPMTeamMember
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oPMTeamMem As New clsPMTeamMembers
            oPMTeamMem.Load()
            If Request("ID") = 0 Then
                Dim oReq As New clsRequest
                ddPersonnel.DataSource = oReq.List_Maint_Reps("KCP", 0)
                ddPersonnel.DataValueField = "PersonnelID"
                ddPersonnel.DataTextField = "Personnel"
                ddPersonnel.DataBind()
                oReq = Nothing
            Else
                Dim oPers As New clsPersonnel
                oPers.PersonnelID = oPMTeamMem.PersonnelID
                oPers.Load()
                ddPersonnel.Items.Add(New ListItem(oPers.FirstName & " " & oPers.LastName, oPMTeamMem.PersonnelID))
                oPers = Nothing
            End If
            cbLeader.Checked = oPMTeamMem.TeamLeader
            oPMTeamMem = Nothing
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim oPMTeamMem As New clsPMTeamMembers
        oPMTeamMem.PMTeamMemberID = Request("ID")
        oPMTeamMem.Load()
        If Request("ID") = 0 Then
            oPMTeamMem.TeamID = Request("TeamID")
        End If
        oPMTeamMem.UserID = Session("UserDBID")
        oPMTeamMem.TeamLeader = cbLeader.Checked
        oPMTeamMem.PersonnelID = ddPersonnel.SelectedValue
        oPMTeamMem.Save()
        oPMTeamMem = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Members();window.close();", True)
    End Sub
End Class
