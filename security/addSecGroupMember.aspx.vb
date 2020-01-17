
Partial Class security_addSecGroupMember
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oPers2Group As New clsPersonnel2Group
            ddMembers.DataSource = oPers2Group.List_Available_Members(Request("GroupID"))
            ddMembers.DataTextField = "Personnel"
            ddMembers.DataValueField = "PersonnelID"
            ddMembers.DataBind()
            oPers2Group = Nothing
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        If CheckSecurity("Security", "ViewPersonnelSettings", , , Session("UserDBID")) Then
            Dim oPers2Group As New clsPersonnel2Group
            oPers2Group.Add_Member(ddMembers.SelectedValue, Request("GroupID"))
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Members();window.close();", True)
            oPers2Group = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
        End If
    End Sub


End Class
