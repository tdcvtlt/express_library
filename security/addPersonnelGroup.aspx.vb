
Partial Class security_addPersonnelGroup
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oPers2Group As New clsPersonnel2Group
            ddGroups.DataSource = oPers2Group.List_Pers_Groups(Request("PersonnelID"))
            ddGroups.DataTextField = "GroupName"
            ddGroups.DataValueField = "ID"
            ddGroups.DataBind()
            oPers2Group = Nothing
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        If CheckSecurity("Security", "ViewPersonnelSettings", , , Session("UserDBID")) Then
            Dim oPers2Group As New clsPersonnel2Group
            oPers2Group.Add_Member(Request("PersonnelID"), ddGroups.SelectedValue)
            oPers2Group = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Sec_Groups();window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
        End If
    End Sub

End Class
