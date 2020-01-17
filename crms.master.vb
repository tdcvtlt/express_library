Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class crms
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserID") = "" Then ' Or LCase(Request.ServerVariables("URL")) <> "/crmsnet/logon.aspx" Then
            'Response.Write(Request.ServerVariables("URL"))
            Response.Redirect("~/logon.aspx?url=" & Request.ServerVariables("URL"))
        End If
        If Session("TreeViewState") IsNot Nothing Then
            Dim list As ArrayList = CType(Session("TreeViewState"), ArrayList)

            RestoreTreeViewState(TreeView1.Nodes, list)
            Me.TreeView1.ExpandDepth = 1
        End If
        'If Not IsPostBack Then
        If ((Session.Item("LastName") = "Hill" And Session.Item("FirstName") = "Richard") Or (Session.Item("LastName") = "Benton" And Session.Item("FirstName") = "Matt")) And ddusers.items.count = 0 Then
            ddUsers.Visible = True
            Dim oUsers As New clsPersonnel
            ddUsers.DataSource = oUsers.List_Active_Accounts
            ddUsers.DataTextField = "Name"
            ddUsers.DataValueField = "ID"
            ddUsers.DataBind()
            oUsers = Nothing
            ddUsers.ClearSelection()
            ddUsers.Items.FindByValue(Session("UserDBID")).Selected = True
        End If
        If Not IsPostBack Then
            If Session("Report") IsNot Nothing Then
                Try

                    If Me.Page.FindControl("CrystalReportViewer1") Is Nothing Then
                        Dim rep As ReportDocument = Session("Report")
                        rep.Close()
                        rep.Dispose()
                        rep = Nothing
                        Session("Report") = Nothing
                    End If

                Catch ex As Exception

                End Try

            End If
        End If
        'End If
    End Sub

    Protected Sub TreeView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles TreeView1.DataBound
        If Session("TreeViewState") Is Nothing Then

            ' Record the TreeViews current expand/collapse state.

            Dim list As ArrayList = New ArrayList

            SaveTreeViewState(TreeView1.Nodes, list)

            Session("TreeViewState") = list

        Else

            'Apply the recorded expand/collapse state to the TreeView.

            Dim list As ArrayList = CType(Session("TreeViewState"), ArrayList)

            RestoreTreeViewState(TreeView1.Nodes, list)

        End If


    End Sub

    Protected Sub TreeView1_TreeNodeCollapsed(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles TreeView1.TreeNodeCollapsed

        If IsPostBack Then

            Dim list As ArrayList = New ArrayList

            SaveTreeViewState(TreeView1.Nodes, list)

            Session("TreeViewState") = list

        End If

    End Sub

    Protected Sub TreeView1_TreeNodeExpanded(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles TreeView1.TreeNodeExpanded

        If IsPostBack Then

            Dim list As ArrayList = New ArrayList

            SaveTreeViewState(TreeView1.Nodes, list)

            Session("TreeViewState") = list

        End If

    End Sub



    Private Sub SaveTreeViewState(ByVal nodes As TreeNodeCollection, ByVal list As ArrayList)

        ' Recursivley record all expanded nodes in the List.

        For Each node As TreeNode In nodes

            If (node.ChildNodes IsNot Nothing And node.ChildNodes.Count <> 0) Then

                If (node.Expanded.HasValue AndAlso CBool(node.Expanded) AndAlso Not String.IsNullOrEmpty(node.Text)) Then

                    list.Add(node.Text)

                    SaveTreeViewState(node.ChildNodes, list)

                End If

            End If

        Next

    End Sub

    Private Sub RestoreTreeViewState(ByVal nodes As TreeNodeCollection, ByVal list As ArrayList)

        For Each node As TreeNode In nodes

            ' Restore the state of one node.

            If list.Contains(node.Text) Then

                If (node.ChildNodes IsNot Nothing AndAlso (node.ChildNodes.Count <> 0) AndAlso node.Expanded.HasValue AndAlso node.Expanded.GetValueOrDefault(False)) Then

                    node.Expand()

                End If

            ElseIf (node.ChildNodes IsNot Nothing AndAlso node.ChildNodes.Count <> 0 AndAlso node.Expanded.HasValue AndAlso node.Expanded.GetValueOrDefault(True)) Then

                node.Collapse()

            End If

            ' If the node has child nodes, restore their states, too.

            If (node.ChildNodes IsNot Nothing AndAlso node.ChildNodes.Count <> 0) Then

                RestoreTreeViewState(node.ChildNodes, list)

            End If

        Next



    End Sub


    Protected Sub ddUsers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddUsers.SelectedIndexChanged
        Session("UserDBID") = ddUsers.SelectedValue
    End Sub
End Class

