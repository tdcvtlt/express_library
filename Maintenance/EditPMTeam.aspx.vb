
Partial Class Maintenance_EditPMTeam
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oPMTeam As New clsPMTeams
            oPMTeam.PMTeamID = Request("TeamID")
            oPMTeam.Load()
            txtID.Text = Request("TeamID")
            txtName.Text = oPMTeam.Name
            cbActive.Checked = oPMTeam.Active
            oPMTeam = Nothing
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub gvMembers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub
    Protected Sub gvMembers_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvMembers.RowCommand
        Dim ID As Integer = 0
        ID = gvMembers.DataKeys(Convert.ToInt32(e.CommandArgument)).Value
        If e.CommandName.CompareTo("Remove") = 0 Then
            Dim oPMMember As New clsPMTeamMembers
            oPMMember.Remove_Member(ID)
            gvMembers.DataSource = oPMMember.List_Group_Members(txtID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvMembers.DataKeyNames = sKeys
            gvMembers.DataBind()
            oPMMember = Nothing
            MultiView1.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub gvMembers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvMembers.SelectedIndexChanged
        Dim row As GridViewRow = gvMembers.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Maintenance/EditPMTeamMember.aspx?ID=" & row.Cells(1).Text & ",'win01',350,350);", True)
    End Sub


    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim oPMTeam As New clsPMTeams
        oPMTeam.PMTeamID = txtID.Text
        oPMTeam.Load()
        oPMTeam.UserID = Session("UserDBID")
        oPMTeam.Active = cbActive.Checked
        oPMTeam.Name = txtName.Text
        oPMTeam.Save()
        Response.Redirect("EditPMTeam.aspx?TeamID=" & oPMTeam.PMTeamID)
        oPMTeam = Nothing
    End Sub



    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Maintenance/EditPMTeamMember.aspx?ID=0&TeamID=" & txtID.Text & "','win01',350,350);", True)
    End Sub

    Protected Sub Members_Link_Click(sender As Object, e As EventArgs) Handles Members_Link.Click
        If txtID.Text > 0 Then
            Dim oPMTeamMembers As New clsPMTeamMembers
            gvMembers.DataSource = oPMTeamMembers.List_Group_Members(txtID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvMembers.DataKeyNames = sKeys
            gvMembers.DataBind()
            oPMTeamMembers = Nothing
            MultiView1.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub Team_Link_Click(sender As Object, e As EventArgs) Handles Team_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub
End Class
