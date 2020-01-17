Imports formsauth
Imports System.Data
Partial Class security_ITSecurityRequest
    Inherits System.Web.UI.Page

    Protected Sub rbEmailType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles rbEmailType.SelectedIndexChanged
        If rbEmailType.SelectedIndex = 0 Then
            Table1.Rows(1).Visible = False
            Table1.Rows(2).Visible = False
            Table1.Rows(3).Visible = False
        ElseIf rbEmailType.SelectedIndex = 1 Then
            Table1.Rows(1).Visible = True
            Table1.Rows(2).Visible = False
            Table1.Rows(3).Visible = False
        ElseIf rbEmailType.SelectedIndex = 2 Then
            Table1.Rows(1).Visible = False
            Table1.Rows(2).Visible = True
            Table1.Rows(3).Visible = True
        Else
            Table1.Rows(1).Visible = True
            Table1.Rows(2).Visible = True
            Table1.Rows(3).Visible = True
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oCombo As New clsComboItems
            ddReqType.DataSource = oCombo.Load_ComboItems("ITSecRequestType")
            ddReqType.DataTextField = "ComboItem"
            ddReqType.DataValueField = "ComboItemID"
            ddReqType.DataBind()
            ddEmailDomain.DataSource = oCombo.Load_ComboItems("EmailDomain")
            ddEmailDomain.DataTextField = "ComboItem"
            ddEmailDomain.DataValueField = "ComboItemID"
            ddEmailDomain.DataBind()
            ddPhoneSetup.DataSource = oCombo.Load_ComboItems("ITSecRequestPhoneType")
            ddPhoneSetup.DataTextField = "ComboItem"
            ddPhoneSetup.DataValueField = "ComboItemID"
            ddPhoneSetup.DataBind()
            Dim oPersGroup As New clsPersonnelGroup
            lbCRMSGroups.DataSource = oPersGroup.List("")
            lbCRMSGroups.DataTextField = "GroupName"
            lbCRMSGroups.DataValueField = "ID"
            lbCRMSGroups.DataBind()
            oPersGroup = Nothing
            Dim adAuth As New LdapAuthentication("LDAP://DC=kcp,DC=local")
            Dim lGroups As List(Of String)
            lGroups = adAuth.Get_All_Groups("Security")
            For Each Group As String In lGroups
                lbWindowsGroups.Items.Add(New ListItem(Replace(Group, "CN=", ""), Group))
            Next
            lGroups = adAuth.Get_All_Groups("Distribution")
            For Each Group As String In lGroups
                lbDistGroups.Items.Add(New ListItem(Replace(Group, "CN=", ""), Group))
            Next

            If Request("ID") > 0 Then
                Dim oSecReq As New clsPersonnelSecurityRequest
                oSecReq.RequestID = Request("ID")
                oSecReq.Load()
                ddReqType.SelectedValue = oSecReq.TypeID
                dteDueDate.Selected_Date = oSecReq.RequestedDueDate
                rbEmailType.SelectedIndex = oSecReq.EmailOption
                Select Case oSecReq.EmailOption
                    Case 0
                        Table1.Rows(1).Visible = False
                        Table1.Rows(2).Visible = False
                        Table1.Rows(3).Visible = False
                    Case 1
                        Table1.Rows(1).Visible = True
                        Table1.Rows(2).Visible = False
                        Table1.Rows(3).Visible = False
                        ddEmailDomain.SelectedValue = oSecReq.EmailDomainID
                    Case 2
                        Table1.Rows(1).Visible = False
                        Table1.Rows(2).Visible = True
                        Table1.Rows(3).Visible = True
                    Case 3
                        Table1.Rows(1).Visible = True
                        Table1.Rows(2).Visible = True
                        Table1.Rows(3).Visible = True
                        ddEmailDomain.SelectedValue = oSecReq.EmailDomainID
                End Select
                ddPhoneSetup.SelectedValue = oSecReq.PhoneTypeID
                cbDID.Checked = oSecReq.DID
                If oCombo.Lookup_ComboItem(oSecReq.PhoneTypeID) <> "No Phone" Then
                    Table2.Rows(1).Visible = True
                Else
                    Table2.Rows(1).Visible = False
                End If

                If oCombo.Lookup_ComboItem(oSecReq.StatusID) <> "Pending" Then
                    btnSave.Visible = False
                End If
            End If
            If oCombo.Lookup_ComboItem(ddReqType.SelectedValue) = "New User" Then
                If Request("ID") > 0 Then
                    Preload_Sec(Request("ID"), IIf(Request("PersonnelID") = "", 0, Request("PersonnelID")))
                Else
                    Reset_Page()
                End If
                MultiView1.ActiveViewIndex = 0
            ElseIf oCombo.Lookup_ComboItem(ddReqType.SelectedValue) = "Edit User" Then
                Preload_Sec(Request("ID"), IIf(Request("PersonnelID") = "", 0, Request("PersonnelID")))
                MultiView1.ActiveViewIndex = 0
            ElseIf oCombo.Lookup_ComboItem(ddReqType.SelectedValue) = "Disable User" Then
                MultiView1.ActiveViewIndex = 1
            Else
                MultiView1.ActiveViewIndex = 2
            End If

            oCombo = Nothing
            If Not (CheckSecurity("Personnel", "Edit", , , Session("UserDBID"))) Then
                btnSave.Visible = False
            End If
        End If
    End Sub

    Protected Sub ddReqType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddReqType.SelectedIndexChanged
        Dim oCombo As New clsComboItems
        If oCombo.Lookup_ComboItem(ddReqType.SelectedValue) = "New User" Then
            Reset_Page()
            MultiView1.ActiveViewIndex = 0
        ElseIf oCombo.Lookup_ComboItem(ddReqType.SelectedValue) = "Edit User" Then
            Preload_Sec(Request("ID"), IIf(Request("PersonnelID") = "", 0, Request("PersonnelID")))
            MultiView1.ActiveViewIndex = 0
        ElseIf oCombo.Lookup_ComboItem(ddReqType.SelectedValue) = "Disable User" Then
            MultiView1.ActiveViewIndex = 1
        Else
            MultiView1.ActiveViewIndex = 2
        End If
        oCombo = Nothing
    End Sub

    Protected Sub ddPhoneSetup_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddPhoneSetup.SelectedIndexChanged
        Dim oCombo As New clsComboItems
        If oCombo.Lookup_ComboItem(ddPhoneSetup.SelectedValue) = "No Phone" Then
            Table2.Rows(1).Visible = False
        Else
            Table2.Rows(1).Visible = True
        End If
        oCombo = Nothing
    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click
        If lbCRMSGroups.SelectedIndex > -1 Then
            lbCRMSGroupsAdd.Items.Add(New ListItem(lbCRMSGroups.SelectedItem.Text, lbCRMSGroups.SelectedValue))
            lbCRMSGroups.Items.Remove(lbCRMSGroups.SelectedItem)
        End If
    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        If lbCRMSGroupsAdd.SelectedIndex > -1 Then
            lbCRMSGroups.Items.Add(New ListItem(lbCRMSGroupsAdd.SelectedItem.Text, lbCRMSGroupsAdd.SelectedValue))
            lbCRMSGroupsAdd.Items.Remove(lbCRMSGroupsAdd.SelectedItem)
        End If
    End Sub

    Protected Sub Button4_Click(sender As Object, e As System.EventArgs) Handles Button4.Click
        If lbWindowsGroups.SelectedIndex > -1 Then
            lbWindowsGroupsAdd.Items.Add(New ListItem(lbWindowsGroups.SelectedItem.Text, lbWindowsGroups.SelectedValue))
            lbWindowsGroups.Items.Remove(lbWindowsGroups.SelectedItem)
        End If
    End Sub

    Protected Sub Button5_Click(sender As Object, e As System.EventArgs) Handles Button5.Click
        If lbWindowsGroupsAdd.SelectedIndex > -1 Then
            lbWindowsGroups.Items.Add(New ListItem(lbWindowsGroupsAdd.SelectedItem.Text, lbWindowsGroupsAdd.SelectedValue))
            lbWindowsGroupsAdd.Items.Remove(lbWindowsGroupsAdd.SelectedItem)
        End If
    End Sub
    Protected Sub Button6_Click(sender As Object, e As System.EventArgs) Handles Button6.Click
        If lbDistGroups.SelectedIndex > -1 Then
            lbDistGroupsAdd.Items.Add(New ListItem(lbDistGroups.SelectedItem.Text, lbDistGroups.SelectedValue))
            lbDistGroups.Items.Remove(lbDistGroups.SelectedItem)
        End If
    End Sub
    Protected Sub Button7_Click(sender As Object, e As System.EventArgs) Handles Button7.Click
        If lbDistGroupsAdd.SelectedIndex > -1 Then
            lbDistGroups.Items.Add(New ListItem(lbDistGroupsAdd.SelectedItem.Text, lbWindowsGroupsAdd.SelectedValue))
            lbDistGroupsAdd.Items.Remove(lbDistGroupsAdd.SelectedItem)
        End If
    End Sub

    Private Sub Preload_Sec(ByVal id As Integer, ByVal persID As Integer)
        Dim oLDAP As New LdapAuthentication("LDAP://DC=kcp,DC=local")
        Dim oSecRequest2Group As New clsPersonnelSecurityRequest2Group
        Dim oSecReq As New clsPersonnelSecurityRequest
        Dim oCombo As New clsComboItems
        Dim oPers As New clsPersonnel
        Dim oSecGroups As New clsPersonnel2Group
        Dim grps As String = ""
        Dim bFound As Boolean = False
        If id > 0 Then
            oSecReq.RequestID = id
            oSecReq.Load()
            oPers.PersonnelID = oSecReq.PersonnelID
        Else
            oPers.PersonnelID = persID
        End If
        oPers.Load()
        If oPers.UserName <> "" Then
            If id = 0 Then
                grps = oLDAP.Get_Groups(oPers.UserName)
            Else
                grps = oSecRequest2Group.get_Groups(id, oCombo.Lookup_ID("ITSecRequestGroupType", "Windows"))
                grps = grps & "|" & oSecRequest2Group.get_Groups(id, oCombo.Lookup_ID("ITSecRequestGroupType", "Distribution"))
            End If

            Dim uGroups() As String = grps.Split("|")

            For i = 0 To UBound(uGroups)
                bFound = False
                For j = 0 To lbWindowsGroups.Items.Count - 1
                    If uGroups(i) = lbWindowsGroups.Items(j).Value Then
                        lbWindowsGroupsAdd.Items.Add(New ListItem(lbWindowsGroups.Items(j).Text, lbWindowsGroups.Items(j).Value))
                        lbWindowsGroups.Items.Remove(lbWindowsGroups.Items(j))
                        bFound = True
                        Exit For
                    End If
                Next
                If Not (bFound) Then
                    For j = 1 To lbDistGroups.Items.Count - 1
                        If uGroups(i) = lbDistGroups.Items(j).Value Then
                            lbDistGroupsAdd.Items.Add(New ListItem(lbDistGroups.Items(j).Text, lbDistGroups.Items(j).Value))
                            lbDistGroups.Items.Remove(lbDistGroups.Items(j))
                            Table1.Rows(2).Visible = True
                            Table1.Rows(3).Visible = True
                            rbEmailType.SelectedIndex = 2
                            Exit For
                        End If
                    Next
                End If
            Next

            If Request("ID") = 0 Then
                grps = oSecGroups.Get_Pers_Groups(oPers.PersonnelID)
            Else
                grps = oSecRequest2Group.get_Groups(id, oCombo.Lookup_ID("ITSecRequestGroupType", "CRMS"))
            End If
            uGroups = grps.Split("|")
            For i = 0 To UBound(uGroups)
                For j = 0 To lbCRMSGroups.Items.Count - 1
                    If uGroups(i) = lbCRMSGroups.Items(j).Value Then
                        lbCRMSGroupsAdd.Items.Add(New ListItem(lbCRMSGroups.Items(j).Text, lbCRMSGroups.Items(j).Value))
                        lbCRMSGroups.Items.Remove(lbCRMSGroups.Items(j))
                        bFound = True
                        Exit For
                    End If
                Next
            Next

            If Request("ID") = 0 Then
                Dim email As String = oLDAP.get_Email(oPers.UserName)
                If email <> "" Then
                    Dim index As Integer = email.IndexOf("@") + 1
                    ddEmailDomain.SelectedValue = oCombo.Lookup_ID("EmailDomain", Right(email, email.Length - index))
                    Table1.Rows(1).Visible = True
                    If Table1.Rows(2).Visible = True Then
                        rbEmailType.SelectedIndex = 3
                    Else
                        rbEmailType.SelectedIndex = 1
                    End If
                End If
            End If
        End If
            oPers = Nothing
            oLDAP = Nothing
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If dteDueDate.Selected_Date = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Select a Due Date.');", True)
        Else
            Dim oPersSecRequest As New clsPersonnelSecurityRequest
            Dim oCombo As New clsComboItems
            Dim oPersRequestGroup As New clsPersonnelSecurityRequest2Group
            oPersSecRequest.RequestID = Request("ID")
            oPersSecRequest.Load()
            If Request("ID") = 0 Then
                oPersSecRequest.PersonnelID = Request("PersonnelID")
                oPersSecRequest.RequestedByID = Session("UserDBID")
                oPersSecRequest.StatusID = oCombo.Lookup_ID("ITSecRequestStatus", "Pending")
                oPersSecRequest.DateCreated = System.DateTime.Now
            End If
            oPersSecRequest.RequestedDueDate = dteDueDate.Selected_Date
            oPersSecRequest.TypeID = ddReqType.SelectedValue
            oPersSecRequest.Save()
            Dim dt As DataTable
            If oCombo.Lookup_ComboItem(ddReqType.SelectedValue) = "Disable User" Then
                dt = oPersRequestGroup.Get_Groups_By_Type(oPersSecRequest.RequestID, "ALL")
                If dt.Rows.Count > 0 Then
                    For i = 0 To dt.Rows.Count - 1
                        oPersRequestGroup.Request2GroupID = dt.Rows(i).Item("ID")
                        oPersRequestGroup.Load()
                        oPersRequestGroup.Active = False
                        oPersRequestGroup.UserID = Session("UserDBID")
                        oPersRequestGroup.Save()
                    Next
                End If
                oPersSecRequest.EmailOption = 0
                oPersSecRequest.EmailDomainID = 0
            Else
                oPersSecRequest.PhoneTypeID = ddPhoneSetup.SelectedValue
                If oCombo.Lookup_ComboItem(ddPhoneSetup.SelectedValue) = "No Phone" Then
                    oPersSecRequest.DID = False
                Else
                    oPersSecRequest.DID = cbDID.Checked
                End If
                Dim bFound As Boolean = False
                dt = oPersRequestGroup.Get_Groups_By_Type(oPersSecRequest.RequestID, "CRMS")
                If dt.Rows.Count > 0 Then
                    For i = 0 To dt.Rows.Count - 1
                        bFound = False
                        For j = 0 To lbCRMSGroupsAdd.Items.Count - 1
                            If lbCRMSGroupsAdd.Items(j).Value = dt.Rows(i).Item("GroupID") Then
                                oPersRequestGroup.Request2GroupID = dt.Rows(i).Item("ID")
                                oPersRequestGroup.Load()
                                oPersRequestGroup.UserID = Session("UserDBID")
                                oPersRequestGroup.Active = True
                                oPersRequestGroup.Save()
                                bFound = True
                                Exit For
                            End If
                        Next
                        If Not (bFound) Then
                            oPersRequestGroup.Request2GroupID = dt.Rows(i).Item("ID")
                            oPersRequestGroup.Load()
                            oPersRequestGroup.UserID = Session("UserDBID")
                            oPersRequestGroup.Active = False
                            oPersRequestGroup.Save()
                        End If
                    Next
                End If

                For i = 0 To lbCRMSGroupsAdd.Items.Count - 1
                    If dt.Rows.Count > 0 Then
                        For j = 0 To dt.Rows.Count - 1
                            bFound = False
                            If dt.Rows(j).Item("GroupID") = lbCRMSGroupsAdd.Items(i).Value Then
                                bFound = True
                                Exit For
                            End If
                        Next
                        If Not (bFound) Then
                            oPersRequestGroup.Request2GroupID = 0
                            oPersRequestGroup.Load()
                            oPersRequestGroup.RequestID = oPersSecRequest.RequestID
                            oPersRequestGroup.Active = True
                            oPersRequestGroup.GroupID = lbCRMSGroupsAdd.Items(i).Value
                            oPersRequestGroup.GroupTypeID = oCombo.Lookup_ID("ITSecRequestGroupType", "CRMS")
                            oPersRequestGroup.UserID = Session("UserDBID")
                            oPersRequestGroup.Save()
                        End If
                    Else
                        oPersRequestGroup.Request2GroupID = 0
                        oPersRequestGroup.Load()
                        oPersRequestGroup.RequestID = oPersSecRequest.RequestID
                        oPersRequestGroup.Active = True
                        oPersRequestGroup.GroupID = lbCRMSGroupsAdd.Items(i).Value
                        oPersRequestGroup.GroupTypeID = oCombo.Lookup_ID("ITSecRequestGroupType", "CRMS")
                        oPersRequestGroup.UserID = Session("UserDBID")
                        oPersRequestGroup.Save()
                    End If
                Next

                'Add/Remove Windows Groups
                dt.Clear()
                dt = oPersRequestGroup.Get_Groups_By_Type(oPersSecRequest.RequestID, "Windows")
                If dt.Rows.Count > 0 Then
                    For i = 0 To dt.Rows.Count - 1
                        bFound = False
                        For j = 0 To lbWindowsGroupsAdd.Items.Count - 1
                            If lbWindowsGroupsAdd.Items(j).Value = dt.Rows(i).Item("GroupID") Then
                                oPersRequestGroup.Request2GroupID = dt.Rows(i).Item("ID")
                                oPersRequestGroup.Load()
                                oPersRequestGroup.UserID = Session("UserDBID")
                                oPersRequestGroup.Active = True
                                oPersRequestGroup.Save()
                                bFound = True
                                Exit For
                            End If
                        Next
                        If Not (bFound) Then
                            oPersRequestGroup.Request2GroupID = dt.Rows(i).Item("ID")
                            oPersRequestGroup.Load()
                            oPersRequestGroup.UserID = Session("UserDBID")
                            oPersRequestGroup.Active = False
                            oPersRequestGroup.Save()
                        End If
                    Next
                End If

                For i = 0 To lbWindowsGroupsAdd.Items.Count - 1
                    If dt.Rows.Count > 0 Then
                        For j = 0 To dt.Rows.Count - 1
                            bFound = False
                            If dt.Rows(j).Item("GroupID") = lbWindowsGroupsAdd.Items(i).Value Then
                                bFound = True
                                Exit For
                            End If
                        Next
                        If Not (bFound) Then
                            oPersRequestGroup.Request2GroupID = 0
                            oPersRequestGroup.Load()
                            oPersRequestGroup.RequestID = oPersSecRequest.RequestID
                            oPersRequestGroup.Active = True
                            oPersRequestGroup.GroupID = lbWindowsGroupsAdd.Items(i).Value
                            oPersRequestGroup.GroupTypeID = oCombo.Lookup_ID("ITSecRequestGroupType", "Windows")
                            oPersRequestGroup.UserID = Session("UserDBID")
                            oPersRequestGroup.Save()
                        End If
                    Else
                        oPersRequestGroup.Request2GroupID = 0
                        oPersRequestGroup.Load()
                        oPersRequestGroup.RequestID = oPersSecRequest.RequestID
                        oPersRequestGroup.GroupID = lbWindowsGroupsAdd.Items(i).Value
                        oPersRequestGroup.Active = True
                        oPersRequestGroup.GroupTypeID = oCombo.Lookup_ID("ITSecRequestGroupType", "Windows")
                        oPersRequestGroup.UserID = Session("UserDBID")
                        oPersRequestGroup.Save()
                    End If
                Next

                dt.Clear()
                oPersSecRequest.EmailOption = rbEmailType.SelectedIndex
                dt = oPersRequestGroup.Get_Groups_By_Type(oPersSecRequest.RequestID, "Distribution")
                If rbEmailType.SelectedIndex = 0 Then
                    If dt.Rows.Count > 0 Then
                        For i = 0 To dt.Rows.Count - 1
                            oPersRequestGroup.Request2GroupID = dt.Rows(i).Item("ID")
                            oPersRequestGroup.Load()
                            oPersRequestGroup.UserID = Session("UserDBID")
                            oPersRequestGroup.Active = False
                            oPersRequestGroup.Save()
                        Next
                    End If

                ElseIf rbEmailType.SelectedIndex = 1 Then
                    oPersSecRequest.EmailDomainID = ddEmailDomain.SelectedValue
                    If dt.Rows.Count > 0 Then
                        For i = 0 To dt.Rows.Count - 1
                            oPersRequestGroup.Request2GroupID = dt.Rows(i).Item("ID")
                            oPersRequestGroup.Load()
                            oPersRequestGroup.UserID = Session("UserDBID")
                            oPersRequestGroup.Active = False
                            oPersRequestGroup.Save()
                        Next
                    End If
                ElseIf rbEmailType.SelectedIndex = 2 Then
                    oPersSecRequest.EmailDomainID = 0
                    If dt.Rows.Count > 0 Then
                        For i = 0 To dt.Rows.Count - 1
                            bFound = False
                            For j = 0 To lbDistGroupsAdd.Items.Count - 1
                                If lbDistGroupsAdd.Items(j).Value = dt.Rows(i).Item("GroupID") Then
                                    oPersRequestGroup.Request2GroupID = dt.Rows(i).Item("ID")
                                    oPersRequestGroup.Load()
                                    oPersRequestGroup.UserID = Session("UserDBID")
                                    oPersRequestGroup.Active = True
                                    oPersRequestGroup.Save()
                                    bFound = True
                                    Exit For
                                End If
                            Next
                            If Not (bFound) Then
                                oPersRequestGroup.Request2GroupID = dt.Rows(i).Item("ID")
                                oPersRequestGroup.Load()
                                oPersRequestGroup.UserID = Session("UserDBID")
                                oPersRequestGroup.Active = False
                                oPersRequestGroup.Save()
                            End If
                        Next
                    End If

                    For i = 0 To lbDistGroupsAdd.Items.Count - 1
                        If dt.Rows.Count > 0 Then
                            For j = 0 To dt.Rows.Count - 1
                                bFound = False
                                If dt.Rows(j).Item("GroupID") = lbDistGroupsAdd.Items(i).Value Then
                                    bFound = True
                                    Exit For
                                End If
                            Next
                            If Not (bFound) Then
                                oPersRequestGroup.Request2GroupID = 0
                                oPersRequestGroup.Load()
                                oPersRequestGroup.RequestID = oPersSecRequest.RequestID
                                oPersRequestGroup.Active = True
                                oPersRequestGroup.GroupID = lbDistGroupsAdd.Items(i).Value ' & " - x - " & i
                                oPersRequestGroup.GroupTypeID = oCombo.Lookup_ID("ITSecRequestGroupType", "Distribution")
                                'oPersRequestGroup.UserID = Session("UserDBID")
                                oPersRequestGroup.Save()
                            End If
                        Else
                            oPersRequestGroup.Request2GroupID = 0
                            oPersRequestGroup.Load()
                            oPersRequestGroup.RequestID = oPersSecRequest.RequestID
                            oPersRequestGroup.GroupID = lbDistGroupsAdd.Items(i).Value
                            oPersRequestGroup.Active = True
                            oPersRequestGroup.GroupTypeID = oCombo.Lookup_ID("ITSecRequestGroupType", "Distribution")
                            oPersRequestGroup.UserID = Session("UserDBID")
                            oPersRequestGroup.Save()
                        End If
                    Next
                ElseIf rbEmailType.SelectedIndex = 3 Then
                    oPersSecRequest.EmailDomainID = ddEmailDomain.SelectedValue
                    If dt.Rows.Count > 0 Then
                        For i = 0 To dt.Rows.Count - 1
                            bFound = False
                            For j = 0 To lbDistGroupsAdd.Items.Count - 1
                                If lbDistGroupsAdd.Items(j).Value = dt.Rows(i).Item("GroupID") Then
                                    oPersRequestGroup.Request2GroupID = dt.Rows(i).Item("ID")
                                    oPersRequestGroup.Load()
                                    oPersRequestGroup.UserID = Session("UserDBID")
                                    oPersRequestGroup.GroupID = oPersRequestGroup.GroupID
                                    oPersRequestGroup.Active = True
                                    oPersRequestGroup.Save()
                                    bFound = True
                                    Exit For
                                End If
                            Next
                            If Not (bFound) Then
                                oPersRequestGroup.Request2GroupID = dt.Rows(i).Item("ID")
                                oPersRequestGroup.Load()
                                oPersRequestGroup.UserID = Session("UserDBID")
                                oPersRequestGroup.Active = False
                                oPersRequestGroup.Save()
                            End If
                        Next
                    End If

                    For i = 0 To lbDistGroupsAdd.Items.Count - 1
                        If dt.Rows.Count > 0 Then
                            For j = 0 To dt.Rows.Count - 1
                                bFound = False
                                If dt.Rows(j).Item("GroupID") = lbDistGroupsAdd.Items(i).Value Then
                                    bFound = True
                                    Exit For
                                End If
                            Next
                            If Not (bFound) Then
                                oPersRequestGroup.Request2GroupID = 0
                                oPersRequestGroup.Load()
                                oPersRequestGroup.RequestID = oPersSecRequest.RequestID
                                oPersRequestGroup.Active = True
                                oPersRequestGroup.GroupID = lbDistGroupsAdd.Items(i).Value
                                oPersRequestGroup.GroupTypeID = oCombo.Lookup_ID("ITSecRequestGroupType", "Distribution")
                                oPersRequestGroup.UserID = Session("UserDBID")
                                oPersRequestGroup.Save()
                            End If
                        Else
                            oPersRequestGroup.Request2GroupID = 0
                            oPersRequestGroup.Load()
                            oPersRequestGroup.RequestID = oPersSecRequest.RequestID
                            oPersRequestGroup.GroupID = lbDistGroupsAdd.Items(i).Value
                            oPersRequestGroup.Active = True
                            oPersRequestGroup.GroupTypeID = oCombo.Lookup_ID("ITSecRequestGroupType", "Distribution")
                            oPersRequestGroup.UserID = Session("UserDBID")
                            oPersRequestGroup.Save()
                        End If
                    Next
                Else
                    oPersSecRequest.EmailDomainID = 0
                    If dt.Rows.Count > 0 Then
                        For i = 0 To dt.Rows.Count - 1
                            oPersRequestGroup.Request2GroupID = dt.Rows(i).Item("ID")
                            oPersRequestGroup.Load()
                            oPersRequestGroup.UserID = Session("UserDBID")
                            oPersRequestGroup.Active = False
                            oPersRequestGroup.Save()
                        Next
                    End If
                End If
            End If
            oPersSecRequest.Save()
            Dim sBody As String = ""
            Dim oPers As New clsPersonnel
            oPers.PersonnelID = oPersSecRequest.PersonnelID
            oPers.Load()
            sBody = "RequestID: " & oPersSecRequest.RequestID & "<br>"
            sBody = sBody & "Personnel: " & oPers.FirstName & " " & oPers.LastName & "<br>"
            sBody = sBody & "Request Type: " & oCombo.Lookup_ComboItem(oPersSecRequest.TypeID) & "<br>"
            sBody = sBody & "Status: " & oCombo.Lookup_ComboItem(oPersSecRequest.StatusID) & "<br>"
            oPers.PersonnelID = oPersSecRequest.RequestedByID
            oPers.Load()
            Send_Mail("MISDept@kingscreekplantation.com", oPers.Email, "Security Request Submitted", sBody, True)
            oPers = Nothing
            oPersSecRequest = Nothing
            oPersRequestGroup = Nothing
            oCombo = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Sec_Request();window.close();", True)

        End If
    End Sub
    Private Sub Reset_Page()
        rbEmailType.SelectedIndex = 0
        If lbDistGroupsAdd.Items.Count > 0 Then
            For i = 0 To lbDistGroupsAdd.Items.Count - 1
                lbDistGroups.Items.Add(New ListItem(lbDistGroupsAdd.Items(i).Text, lbDistGroupsAdd.Items(i).Value))
            Next
            For i = lbDistGroupsAdd.Items.Count - 1 To 0 Step -1
                lbDistGroupsAdd.Items.Remove(lbDistGroupsAdd.Items(i))
            Next
        End If
        Table1.Rows(1).Visible = False
        Table1.Rows(2).Visible = False
        Table1.Rows(3).Visible = False

        If lbCRMSGroupsAdd.Items.Count > 0 Then
            For i = 0 To lbCRMSGroupsAdd.Items.Count - 1
                lbCRMSGroups.Items.Add(New ListItem(lbCRMSGroupsAdd.Items(i).Text, lbCRMSGroupsAdd.Items(i).Value))
            Next
            For i = lbCRMSGroupsAdd.Items.Count - 1 To 0 Step -1
                lbCRMSGroupsAdd.Items.Remove(lbCRMSGroupsAdd.Items(i))
            Next
        End If

        If lbWindowsGroupsAdd.Items.Count > 0 Then
            For i = 0 To lbWindowsGroupsAdd.Items.Count - 1
                lbWindowsGroups.Items.Add(New ListItem(lbWindowsGroupsAdd.Items(i).Text, lbWindowsGroupsAdd.Items(i).Value))
            Next
            For i = lbWindowsGroupsAdd.Items.Count - 1 To 0 Step -1
                lbWindowsGroupsAdd.Items.Remove(lbWindowsGroupsAdd.Items(i))
            Next
        End If

    End Sub

End Class
