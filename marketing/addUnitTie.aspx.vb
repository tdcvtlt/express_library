
Partial Class marketing_addUnitTie
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oUnitTie As New clsUnit
            ddUnits.DataSource = oUnitTie.List_For_Tie(Request("UnitID"))
            ddUnits.DataTextField = "Unit"
            ddUnits.DataValueField = "UnitID"
            ddUnits.DataBind()
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oUnitTies As New clsUnit2Unit
        oUnitTies.Unit2UnitID = 0
        oUnitTies.Load()
        oUnitTies.UnitID = Request("UnitID")
        oUnitTies.Unit2ID = ddUnits.SelectedValue
        oUnitTies.Save()
        oUnitTies = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Ties();window.close();", True)
    End Sub
End Class
