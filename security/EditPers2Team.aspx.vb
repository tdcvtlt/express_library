
Partial Class security_EditPers2Team
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            siSalesTeam.Connection_String = Resources.Resource.cns
            siSalesTeam.ComboItem = "SalesTeam"
            siSalesTeam.Label_Caption = ""
            siSalesTeam.Load_Items()
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        If siSalesTeam.Selected_ID < 1 Then
            lblErr.Text = "Please Select a Sales Team."
        Else
            Dim oPers2Team As New clsPersonnel2Teams
            oPers2Team.Pers2TeamID = 0
            oPers2Team.Load()
            oPers2Team.TeamID = siSalesTeam.Selected_ID
            oPers2Team.PersonnelID = Request("PersonnelID")
            oPers2Team.Save()
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Teams();window.close();", True)
        End If
    End Sub
End Class
