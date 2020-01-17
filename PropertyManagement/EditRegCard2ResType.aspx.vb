
Partial Class PropertyManagement_EditRegCard2ResType
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oResType As New clsRegCard2ResType
            oResType.RegCard2TypeID = Request("ID")
            oResType.Load()
            cbActive.Checked = oResType.Active
            siResType.Selected_ID = oResType.ResTypeID
            siResType.Connection_String = Resources.Resource.cns
            siResType.Label_Caption = ""
            siResType.ComboItem = "ReservationType"
            siResType.Load_Items()
            oResType = Nothing
        End If
    End Sub

    Protected Sub Save_Click(sender As Object, e As System.EventArgs) Handles Save.Click
        Dim oResType As New clsRegCard2ResType
        oResType.RegCard2TypeID = Request("ID")
        oResType.Load()
        If Request("ID") = 0 Then
            oResType.RegCardID = Request("RegCardID")
        End If
        oResType.Active = cbActive.Checked
        oResType.ResTypeID = siResType.Selected_ID
        oResType.UserID = Session("UserDBID")
        oResType.Save()
        oResType = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_ResTypes();window.close();", True)

    End Sub
End Class
