
Imports System.Data.SqlClient

Partial Class security_EditPersonnel
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If CheckSecurity("Personnel", "View", , , Session("UserDBID")) Then
                siStatusID.Connection_String = Resources.Resource.cns
                siStatusID.ComboItem = "PersonnelStatus"
                siStatusID.Label_Caption = ""
                siStatusID.Load_Items()
                siRotorTypeID.Connection_String = Resources.Resource.cns
                siRotorTypeID.ComboItem = "SalesRotorType"
                siRotorTypeID.Label_Caption = ""
                siRotorTypeID.Load_Items()
                Dim oPersonnel As New clsPersonnel
                ddReportsToID.DataSource = oPersonnel.List_Managers
                ddReportsToID.DataValueField = "PersonnelID"
                ddReportsToID.DataTextField = "Name"
                ddReportsToID.DataBind()
                oPersonnel = Nothing
                If Request("PersonnelID") <> 0 Then
                    Load_Personnel()
                End If
                MultiView1.ActiveViewIndex = 0
            Else
                MultiView1.ActiveViewIndex = 12
            End If
        End If
    End Sub

    Protected Sub Load_Personnel()
        Dim oPersonnel As New clsPersonnel
        oPersonnel.PersonnelID = Request("PersonnelID")
        oPersonnel.Load()
        txtPersonnelID.Text = oPersonnel.PersonnelID
        txtFirstName.Text = oPersonnel.FirstName
        txtLastName.Text = oPersonnel.LastName
        txtSSN.Text = oPersonnel.SSN
        txtEmail.Text = oPersonnel.Email
        txtUserName.Text = oPersonnel.UserName
        dteHireDate.Selected_Date = oPersonnel.HireDate
        dteTermDate.Selected_Date = oPersonnel.TermDate
        cbActive.Checked = oPersonnel.Active
        cbTO.Checked = oPersonnel.ActAsTO
        siStatusID.Selected_ID = oPersonnel.StatusID
        siRotorTypeID.Selected_ID = oPersonnel.SalesRotorTypeID
        ddReportsToID.SelectedValue = oPersonnel.ReportsToID
        oPersonnel = Nothing
    End Sub

    Protected Sub Notes_Link_Click(sender As Object, e As System.EventArgs) Handles Notes_Link.Click
        If txtPersonnelID.Text > 0 Then
            MultiView1.ActiveViewIndex = 6
            Notes1.KeyValue = txtPersonnelID.Text
            Notes1.Display()
        End If
    End Sub

    Protected Sub UserFields_Link_Click(sender As Object, e As System.EventArgs) Handles UserFields_Link.Click
        If txtPersonnelID.Text > 0 Then
            MultiView1.ActiveViewIndex = 8
            UF.KeyField = "Personnel"
            UF.KeyValue = CInt(txtPersonnelID.Text)
            UF.Load_List()
        End If
    End Sub

    Protected Sub Event_Link_Click(sender As Object, e As System.EventArgs) Handles Event_Link.Click
        If txtPersonnelID.Text > 0 Then
            MultiView1.ActiveViewIndex = 7
            upEvents.KeyField = "PersonnelID"
            upEvents.KeyValue = txtPersonnelID.Text
            upEvents.List()
        End If
    End Sub

    Protected Sub Dept_Link_Click(sender As Object, e As System.EventArgs) Handles Dept_Link.Click
        If txtPersonnelID.Text > 0 Then
            MultiView1.ActiveViewIndex = 4
            Dim oPers2Dept As New clsPersonnel2Dept
            gvPersDept.DataSource = oPers2Dept.List_Depts(txtPersonnelID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvPersDept.DataKeyNames = sKeys
            gvPersDept.DataBind()
            oPers2Dept = Nothing
        End If
    End Sub

    Protected Sub gvPersDept_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Departments Assigned" Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub gvPersDept_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvPersDept.SelectedIndexChanged
        Dim row As GridViewRow = gvPersDept.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Security/EditPersDept.aspx?PersDeptID=" & row.Cells(1).Text & "','win01',450,450);", True)
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        If CheckSecurity("Personnel", "Edit", , , Session("UserDBID")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Security/EditPersDept.aspx?PersDeptID=0&PersonnelID=" & txtPersonnelID.Text & "','win01',450,450);", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
        End If
    End Sub

    Protected Sub Teams_Link_Click(sender As Object, e As System.EventArgs) Handles Teams_Link.Click
        If txtPersonnelID.Text > 0 Then
            Dim oPers2Team As New clsPersonnel2Teams
            gvSalesTeams.DataSource = oPers2Team.List_Teams(txtPersonnelID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvSalesTeams.DataKeyNames = sKeys
            gvSalesTeams.DataBind()
            oPers2Team = Nothing
            MultiView1.ActiveViewIndex = 3
        End If
    End Sub

    Protected Sub gvSalesTeams_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvSalesTeams.RowCommand
        Dim ID As Integer
        ID = Convert.ToInt32(gvSalesTeams.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        If e.CommandName.CompareTo("Remove") = 0 Then
            Dim oPers2Team As New clsPersonnel2Teams
            oPers2Team.Remove_Team(ID)
            gvSalesTeams.DataSource = oPers2Team.List_Teams(txtPersonnelID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvSalesTeams.DataKeyNames = sKeys
            gvSalesTeams.DataBind()
            oPers2Team = Nothing
        End If
    End Sub

    Protected Sub gvSalesTeams_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Teams Assigned" Then
            e.Row.Cells(0).Visible = False
        End If
    End Sub


    Protected Sub Unnamed1_Click1(sender As Object, e As System.EventArgs)
        If CheckSecurity("Personnel", "Edit", , , Session("UserDBID")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Security/EditPers2Team.aspx?PersonnelID=" & txtPersonnelID.Text & "','win01',450,450);", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
        End If
    End Sub

    Protected Sub Personnel_Link_Click(sender As Object, e As System.EventArgs) Handles Personnel_Link.Click
        If txtPersonnelID.Text > 0 Then
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub TimeCard_Link_Click(sender As Object, e As System.EventArgs) Handles TimeCard_Link.Click
        If txtPersonnelID.Text > 0 Then
            Dim oTimeCard As New clsPersonnelTimeCards
            gvTimeCards.DataSource = oTimeCard.List_Cards(txtPersonnelID.Text)
            gvTimeCards.DataBind()
            oTimeCard = Nothing
            MultiView1.ActiveViewIndex = 5
        End If
    End Sub

    Protected Sub gvTimeCards_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvTimeCards.SelectedIndexChanged
        Dim row As GridViewRow = gvTimeCards.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Security/EditTimeCard.aspx?CardID=" & row.Cells(1).Text & "&PersonnelID=" & txtPersonnelID.Text & "','win01',450,450);", True)
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        If CheckSecurity("Personnel", "Edit", , , Session("UserDBID")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Security/EditTimeCard.aspx?CardID=0&PersonnelID=" & txtPersonnelID.Text & "','win01',450,450);", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
        End If
    End Sub

    Protected Sub TimeClock_Link_Click(sender As Object, e As System.EventArgs) Handles TimeClock_Link.Click
        If txtPersonnelID.Text > 0 Then
            Dim oPersPunch As New clsPersonnelPunch
            gvTimeClock.DataSource = oPersPunch.Get_Punches(txtPersonnelID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "PunchID"
            gvTimeClock.DataKeyNames = sKeys
            gvTimeClock.DataBind()
            oPersPunch = Nothing
            MultiView1.ActiveViewIndex = 10
        End If
    End Sub

    Protected Sub gvTimeClock_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Punches" Then
            If e.Row.RowIndex > -1 Then
                If e.Row.Cells(4).Text = "True" Then
                    e.Row.Cells(2).ForeColor = System.Drawing.Color.Red
                    e.Row.Cells(3).ForeColor = System.Drawing.Color.Red
                End If
                If e.Row.Cells(7).Text = "True" Then
                    e.Row.Cells(5).ForeColor = System.Drawing.Color.Red
                    e.Row.Cells(6).ForeColor = System.Drawing.Color.Red
                End If
                If e.Row.Cells(10).Text = "InApproved" Then
                    e.Row.Cells(2).ForeColor = System.Drawing.Color.Green
                    e.Row.Cells(3).ForeColor = System.Drawing.Color.Green
                ElseIf e.Row.Cells(10).Text = "OutApproved" Then
                    e.Row.Cells(5).ForeColor = System.Drawing.Color.Green
                    e.Row.Cells(6).ForeColor = System.Drawing.Color.Green
                ElseIf e.Row.Cells(10).Text = "BothApproved" Then
                    e.Row.Cells(2).ForeColor = System.Drawing.Color.Green
                    e.Row.Cells(3).ForeColor = System.Drawing.Color.Green
                    e.Row.Cells(5).ForeColor = System.Drawing.Color.Green
                    e.Row.Cells(6).ForeColor = System.Drawing.Color.Green
                End If
                If e.Row.Cells(2).Text <> "&nbsp;" Then
                    e.Row.Cells(2).Text = CDate(e.Row.Cells(2).Text).ToShortDateString
                End If
                If e.Row.Cells(5).Text <> "&nbsp;" Then
                    e.Row.Cells(5).Text = CDate(e.Row.Cells(5).Text).ToShortDateString
                End If
                Dim sDate() As String
                If e.Row.Cells(3).Text <> "&nbsp;" Then
                    sDate = e.Row.Cells(3).Text.Split(" ")
                    e.Row.Cells(3).Text = sDate(1) & " " & sDate(2)
                End If
                If e.Row.Cells(6).Text <> "&nbsp;" Then
                    sDate = e.Row.Cells(6).Text.Split(" ")
                    e.Row.Cells(6).Text = sDate(1) & " " & sDate(2)
                End If
            End If

            e.Row.Cells(4).Visible = False
            e.Row.Cells(7).Visible = False
            e.Row.Cells(1).Visible = False
            e.Row.Cells(10).Visible = False
        End If
    End Sub

    Protected Sub gvTimeClock_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvTimeClock.RowCommand
        Dim punchID As Integer
        punchID = Convert.ToInt32(gvTimeClock.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        If e.CommandName.CompareTo("DeletePunch") = 0 Then
            Dim oPersPunch As New clsPersonnelPunch
            If oPersPunch.Delete_Punch(punchID) Then
                gvTimeClock.DataSource = oPersPunch.Get_Punches(txtPersonnelID.Text)
                Dim sKeys(0) As String
                sKeys(0) = "PunchID"
                gvTimeClock.DataKeyNames = sKeys
                gvTimeClock.DataBind()
            End If
            oPersPunch = Nothing
        End If
    End Sub

    Protected Sub gvTimeClock_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvTimeClock.SelectedIndexChanged
        Dim row As GridViewRow = gvTimeClock.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Payroll/EditPunch.aspx?PunchID=" & row.Cells(1).Text & "','win01',450,450);", True)
    End Sub

    Protected Sub Unnamed4_Click(sender As Object, e As System.EventArgs)
        If CheckSecurity("Personnel", "Edit", , , Session("UserDBID")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Payroll/EditPunch.aspx?PunchID=0&PersonnelID=" & txtPersonnelID.Text & "','win01',450,450);", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
        End If
    End Sub

    Protected Sub Unnamed1_Click2(sender As Object, e As System.EventArgs)
        If CheckSecurity("Personnel", "Edit", , , Session("UserDBID")) Then
            Dim oPersonnel As New clsPersonnel
            If Request("PersonnelID") = 0 Then
                oPersonnel.PersonnelID = 0
            Else
                oPersonnel.PersonnelID = txtPersonnelID.Text
            End If
            oPersonnel.Load()
            oPersonnel.FirstName = txtFirstName.Text
            oPersonnel.LastName = txtLastName.Text
            'SSN CHECK
            oPersonnel.SSN = txtSSN.Text
            oPersonnel.StatusID = siStatusID.Selected_ID
            oPersonnel.SalesRotorTypeID = siRotorTypeID.Selected_ID
            oPersonnel.HireDate = dteHireDate.Selected_Date
            oPersonnel.TermDate = dteTermDate.Selected_Date
            oPersonnel.ActAsTO = cbTO.Checked
            oPersonnel.Active = cbActive.Checked
            oPersonnel.Email = txtEmail.Text
            oPersonnel.ReportsToID = ddReportsToID.SelectedValue
            oPersonnel.UserID = Session("UserDBID")

            If txtUserName.Text.Length = 0 Then oPersonnel.UserName = ""

            oPersonnel.Save()

            If String.IsNullOrEmpty(txtUserName.Text) = False Then
                Using cn As New SqlConnection(Resources.Resource.cns)
                    Using cm = New SqlCommand(String.Format("select count(*) Username from t_Personnel where Username = '{0}' and personnelID <> {1} group by Username", txtUserName.Text.Trim(), oPersonnel.PersonnelID), cn)
                        Try
                            cn.Open()
                            If Convert.ToInt32(cm.ExecuteScalar()) >= 1 Then
                                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Username is already taken.');", True)
                            Else
                                oPersonnel.UserName = txtUserName.Text
                                oPersonnel.Save()
                            End If
                        Catch ex As Exception
                            cn.Close()
                            Throw ex
                        Finally
                            cn.Close()
                        End Try
                    End Using
                End Using
            End If

            If Request("PersonnelID") = 0 Then
                Response.Redirect("EditPersonnel.aspx?PersonnelID=" & oPersonnel.PersonnelID)
            End If
            oPersonnel = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
        End If
    End Sub

    Protected Sub Vendor_Link_Click(sender As Object, e As System.EventArgs) Handles Vendor_Link.Click
        If txtPersonnelID.Text > 0 Then
            Dim oVendor As New clsVendor2Personnel
            gvVendors.DataSource = oVendor.List_Vendors(txtPersonnelID.Text)
            Dim skeys(0) As String
            skeys(0) = "Vendor2PersonnelID"
            gvVendors.DataKeyNames = skeys
            gvVendors.DataBind()
            oVendor = Nothing
            MultiView1.ActiveViewIndex = 11
        End If
    End Sub

    Protected Sub UploadedDocs_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UploadedDocs_Link.Click
        If txtPersonnelID.Text > 0 Then
            MultiView1.ActiveViewIndex = 9
            ucDocs.KeyValue = Request("Personnelid")
            ucDocs.KeyField = "PersonnelID"
            ucDocs.List()
        End If
    End Sub

    Protected Sub Security_Link_Click(sender As Object, e As System.EventArgs) Handles Security_Link.Click
        If txtPersonnelID.Text > 0 Then
            If CheckSecurity("Security", "ViewPersonnelSettings", , , Session("UserDBID")) Then
                Dim oSecGroup As New clsSecurityGroups
                lbGroups.DataSource = oSecGroup.List()
                lbGroups.DataTextField = "Name"
                lbGroups.DataValueField = "GroupID"
                lbGroups.DataBind()
                oSecGroup = Nothing
                MultiView1.ActiveViewIndex = 1
            Else
                MultiView1.ActiveViewIndex = 12
            End If
        End If
    End Sub

    Protected Sub lbGroups_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbGroups.SelectedIndexChanged
        Dim oSecItems As New clsSecurityItem
        gvSecItems.DataSource = oSecItems.List_Items_By_Group(lbGroups.SelectedValue)
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvSecItems.DataKeyNames = sKeys
        gvSecItems.DataBind()
        oSecItems = Nothing
    End Sub

    Protected Sub gvSecItems_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                Dim oSecItem As New clsSecurityItem2User
                Dim cb As CheckBox
                cb = e.Row.FindControl("cbAllow")
                If oSecItem.Check_Item2User(e.Row.Cells(1).Text, txtPersonnelID.Text) Then
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
        Dim oItem2User As New clsSecurityItem2User
        If cb.Checked Then
            If Not (oItem2User.Add_Item(row.Cells(1).Text, txtPersonnelID.Text)) Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Error');", True)
            End If
        Else
            If Not (oItem2User.Remove_Item(row.Cells(1).Text, txtPersonnelID.Text)) Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Error');", True)
            End If
        End If
        oItem2User = Nothing
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

    Protected Sub Unnamed8_Click(sender As Object, e As System.EventArgs)
        If CheckSecurity("Personnel", "Edit", , , Session("UserDBID")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Security/EditVendor.aspx?ID=0&PersonnelID=" & txtPersonnelID.Text & "','win01',450,450);", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
        End If
    End Sub

    Protected Sub Groups_Link_Click(sender As Object, e As System.EventArgs) Handles Groups_Link.Click
        If CheckSecurity("Security", "ViewPersonnelSettings", , , Session("UserDBID")) Then
            Dim oSecGroup As New clsPersonnel2Group
            gvPersonnelGroups.DataSource = oSecGroup.List_Groups(txtPersonnelID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvPersonnelGroups.DataKeyNames = sKeys
            gvPersonnelGroups.DataBind()
            oSecGroup = Nothing
            MultiView1.ActiveViewIndex = 2
        Else
            MultiView1.ActiveViewIndex = 12
        End If
    End Sub

    Protected Sub gvPersonnelGroups_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(0).Visible = False
        End If
    End Sub

    Protected Sub gvPersonnelGroups_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvPersonnelGroups.RowCommand
        Dim ID As String
        Dim IDs(1) As String
        ID = gvPersonnelGroups.DataKeys(Convert.ToInt32(e.CommandArgument)).Value
        IDs = ID.Split("-")
        If e.CommandName.CompareTo("Remove") = 0 Then
            Dim oPers2Group As New clsPersonnel2Group
            oPers2Group.Remove_Member(CInt(IDs(0)), CInt(IDs(1)))
            gvPersonnelGroups.DataSource = oPers2Group.List_Groups(txtPersonnelID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvPersonnelGroups.DataKeyNames = sKeys
            gvPersonnelGroups.DataBind()
            oPers2Group = Nothing
        End If
    End Sub

    Protected Sub Unnamed4_Click1(sender As Object, e As System.EventArgs)
        If CheckSecurity("Security", "ViewPersonnelSettings", , , Session("UserDBID")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "javascript:modal.mwindow.open('" & Request.ApplicationPath & "/Security/addPersonnelGroup.aspx?PersonnelID=" & txtPersonnelID.Text & "','win01',450,450);", True)
        End If
    End Sub

    Protected Sub gvVendors_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub gvVendors_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvVendors.RowCommand
        Dim ID As Integer = 0
        ID = Convert.ToInt32(gvVendors.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        If e.CommandName.CompareTo("Remove") = 0 Then
            Dim oPers2Vendor As New clsVendor2Personnel
            oPers2Vendor.Remove_Vendor(ID)
            gvVendors.DataSource = oPers2Vendor.List_Vendors(txtPersonnelID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "Vendor2PersonnelID"
            gvVendors.DataKeyNames = sKeys
            gvVendors.DataBind()
            oPers2Vendor = Nothing
        End If
    End Sub

    Protected Sub gvVendors_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvVendors.SelectedIndexChanged
        Dim row As GridViewRow = gvVendors.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Security/editVendor.aspx?ID=" & row.Cells(1).Text & "','win01',450,450);", True)
    End Sub


    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        If CheckSecurity("Personnel", "Edit", , , Session("UserDBID")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Security/ITSecurityRequest.aspx?ID=0&PersonnelID=" & txtPersonnelID.Text & "','win01',650,650);", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
        End If
    End Sub

    Protected Sub ITSec_Link_Click(sender As Object, e As System.EventArgs) Handles ITSec_Link.Click
        If txtPersonnelID.Text > 0 Then
            Dim oSecRequest As New clsPersonnelSecurityRequest
            gvITRequests.DataSource = oSecRequest.List_Requests(txtPersonnelID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvITRequests.DataKeyNames = sKeys
            gvITRequests.DataBind()
            MultiView1.ActiveViewIndex = 13
            oSecRequest = Nothing
        End If
    End Sub

    Protected Sub gvITRequests_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvITRequests.SelectedIndexChanged
        Dim row As GridViewRow = gvITRequests.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Security/ITSecurityRequest.aspx?ID=" & row.Cells(1).Text & "','win01',650,650);", True)
    End Sub

    'Protected Sub CB_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CB_Link.Click
    '    If txtPersonnelID.Text > 0 Then
    '        Dim oCB As New clsPayrollChargebackRequest
    '        gvChargeBacks.DataSource = oCB.List_CBs("PersonnelID", txtPersonnelID.Text)
    '        gvChargeBacks.DataBind()
    '        oCB = Nothing
    '        MultiView1.ActiveViewIndex = 14
    '    End If
    'End Sub

    'Protected Sub gvChargeBacks_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvChargeBacks.SelectedIndexChanged
    '    Dim row As GridViewRow = gvChargeBacks.SelectedRow
    '    Response.Redirect("../Payroll/EditChargeBackRequest.aspx?CBID=" & row.Cells(1).Text)
    'End Sub
End Class
