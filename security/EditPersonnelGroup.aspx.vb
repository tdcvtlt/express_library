
Partial Class security_EditPersonnelGroup
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If CheckSecurity("Security", "ViewPersonnelSettings", , , Session("UserDBID")) Then
                Dim oPersGroup As New clsPersonnelGroup
                oPersGroup.PersonnelGroupID = Request("GroupID")
                oPersGroup.Load()
                txtGroupID.Text = oPersGroup.PersonnelGroupID
                txtGroupName.Text = oPersGroup.GroupName
                oPersGroup = Nothing
                MultiView1.ActiveViewIndex = 0
            Else
                MultiView1.ActiveViewIndex = 3
            End If
        End If
    End Sub

    Protected Sub Personnel_Link_Click(sender As Object, e As System.EventArgs) Handles Personnel_Link.Click
        If CheckSecurity("Security", "ViewPersonnelSettings", , , Session("UserDBID")) Then
            MultiView1.ActiveViewIndex = 0
        Else
            MultiView1.ActiveViewIndex = 3
        End If
    End Sub

    Protected Sub Security_Link_Click(sender As Object, e As System.EventArgs) Handles Security_Link.Click
        If CheckSecurity("Security", "ViewPersonnelSettings", , , Session("UserDBID")) Then
            If txtGroupID.Text > 0 Then
                Dim oSecGroup As New clsSecurityGroups
                lbGroups.DataSource = oSecGroup.List()
                lbGroups.DataTextField = "Name"
                lbGroups.DataValueField = "GroupID"
                lbGroups.DataBind()
                oSecGroup = Nothing
                MultiView1.ActiveViewIndex = 1
            End If
        Else
            MultiView1.ActiveViewINdex = 3
        End If
    End Sub

    Protected Sub Groups_Link_Click(sender As Object, e As System.EventArgs) Handles Groups_Link.Click
        If CheckSecurity("Security", "ViewPersonnelSettings", , , Session("UserDBID")) Then
            If txtGroupID.Text > 0 Then
                Dim oPers2Group As New clsPersonnel2Group
                gvMembers.DataSource = oPers2Group.List_Group_Members(txtGroupID.Text)
                Dim sKeys(0) As String
                sKeys(0) = "ID"
                gvMembers.DataKeyNames = sKeys
                gvMembers.DataBind()
                MultiView1.ActiveViewINdex = 2
            End If
        Else
            MultiView1.ActiveViewIndex = 3
        End If
    End Sub
    Protected Sub gvMembers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(0).Visible = False
        End If
    End Sub
    Protected Sub gvMembers_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvMembers.RowCommand
        Dim ID As String
        Dim IDs(1) As String
        ID = gvMembers.DataKeys(Convert.ToInt32(e.CommandArgument)).Value
        IDs = ID.Split("-")
        If e.CommandName.CompareTo("Remove") = 0 Then
            Dim oPers2Group As New clsPersonnel2Group
            oPers2Group.Remove_Member(CInt(IDs(0)), CInt(IDs(1)))
            gvMembers.DataSource = oPers2Group.List_Group_Members(txtGroupID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvMembers.DataKeyNames = sKeys
            gvMembers.DataBind()
            oPers2Group = Nothing
            MultiView1.ActiveViewIndex = 2
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oPersGroup As New clsPersonnelGroup
        oPersGroup.PersonnelGroupID = txtGroupID.Text
        oPersGroup.Load()
        oPersGroup.GroupName = txtGroupName.Text
        oPersGroup.Save()
        
        Dim id As Integer = oPersGroup.PersonnelGroupID
        oPersGroup = Nothing
        Response.Redirect("EditPersonnelGroup.aspx?GroupID=" & id)
    End Sub

    Protected Sub lbGroups_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbGroups.SelectedIndexChanged
        Dim oSecItems As New clsSecurityItem
        gvSecItems.DataSource = oSecItems.List_Items_By_Group(lbGroups.SelectedValue)
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvSecItems.DataKeyNames = sKeys
        gvSecItems.DataBind()
        oSecItems = Nothing
        MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub gvSecItems_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                Dim oSecItem As New clsSecurityItem2Group
                Dim cb As CheckBox
                cb = e.Row.FindControl("cbAllow")
                If oSecItem.Check_Item2Group(e.Row.Cells(1).Text, txtGroupID.Text) Then
                    cb.Checked = True
                End If
                oSecItem = Nothing
            End If
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub cbAllow_CheckedChanged(sender As Object, e As System.EventArgs)
        Dim row As GridViewRow = CType(sender.Parent.parent, GridViewRow)
        Dim cb As CheckBox = row.FindControl("cbAllow")
        Dim oItem2User As New clsSecurityItem2Group
        If cb.Checked Then
            If Not (oItem2User.Add_Item(row.Cells(1).Text, txtGroupID.Text)) Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Error');", True)
            End If
        Else
            If Not (oItem2User.Remove_Item(row.Cells(1).Text, txtGroupID.Text)) Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Error');", True)
            End If
        End If
        oItem2User = Nothing
        MultiView1.ActiveViewIndex = 1
    End Sub
    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        If txtArea.Text <> "" Then
            Dim oSecGroup As New clsSecurityGroups
            If oSecGroup.Group_Exists(txtArea.Text) Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('This Area Exisits');", True)
            Else
                Dim ID As Integer = 0
                oSecGroup.GroupID = 0
                oSecGroup.Load()
                oSecGroup.Name = txtArea.Text
                oSecGroup.Save()
                If lbGroups.SelectedValue <> "" Then
                    ID = lbGroups.SelectedValue
                End If
                lbGroups.DataSource = oSecGroup.List()
                lbGroups.DataTextField = "Name"
                lbGroups.DataValueField = "GroupID"
                lbGroups.DataBind()
                If ID > 0 Then
                    lbGroups.SelectedValue = ID
                End If
                txtArea.Text = ""
            End If
        End If
    End Sub

    Protected Sub Unnamed3_Click1(sender As Object, e As System.EventArgs)
        If txtItem.Text <> "" Then
            If lbGroups.SelectedValue <> "" Then
                Dim oSecItem As New clsSecurityItem
                If oSecItem.Item_Exists(txtItem.Text, lbGroups.SelectedValue) Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('This Item Exisits');", True)
                Else
                    oSecItem.ItemID = 0
                    oSecItem.GroupID = lbGroups.SelectedValue
                    oSecItem.Item = txtItem.Text
                    oSecItem.Save()
                    gvSecItems.DataSource = oSecItem.List_Items_By_Group(lbGroups.SelectedValue)
                    Dim sKeys(0) As String
                    sKeys(0) = "ID"
                    gvSecItems.DataKeyNames = sKeys
                    gvSecItems.DataBind()
                    txtItem.Text = ""
                End If
                oSecItem = Nothing
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Select An Area To Add The Item.');", True)
            End If
        End If
    End Sub

    Protected Sub Unnamed4_Click(sender As Object, e As System.EventArgs)
        If CheckSecurity("Security", "ViewPersonnelSettings", , , Session("UserDBID")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Security/addSecGroupMember.aspx?GroupID=" & txtGroupID.Text & "','win01',450,450);", True)
        End If
    End Sub
End Class
