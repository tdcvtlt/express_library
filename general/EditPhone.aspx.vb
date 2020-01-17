
Partial Class general_EditPhone
    Inherits System.Web.UI.Page
    Dim oPhone As New clsPhone

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        oPhone.PhoneID = IIf(IsNumeric(Request("PhoneID")), Request("PhoneID"), 0)
        oPhone.ProspectID = IIf(IsNumeric(Request("ProspectID")), Request("ProspectID"), 0)
        If Not IsPostBack Then
            siType.ComboItem = "Phone"
            siType.Connection_String = Resources.Resource.cns
            siType.Load_Items()
            siType.Label_Caption = ""

            oPhone.Load()
            Load_Values()
            lblError.Text = oPhone.Error_Message
        End If
    End Sub

    Private Sub Load_Values()
        txtPhoneID.Text = oPhone.PhoneID
        txtProspectID.Text = oPhone.ProspectID
        txtNumber.Text = oPhone.Number
        txtExtension.Text = oPhone.Extension
        siType.Selected_ID = oPhone.TypeID
        ckActive.Checked = oPhone.Active
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        With oPhone
            .ProspectID = txtProspectID.Text
            .Number = txtNumber.Text
            .Extension = txtExtension.Text
            .TypeID = siType.Selected_ID
            .Active = ckActive.Checked
            .UserID = Session("UserDBID")
            .Save()
            lblError.Text = .Error_Message
        End With
        If oPhone.Error_Message = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.Refresh_Phone();window.close();", True)
        End If
    End Sub

End Class
