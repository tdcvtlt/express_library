
Partial Class setup_EditReservationLetterSource
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim oResLetter2Source As New clsReservationLetter2Source
        oResLetter2Source.ResLetter2SourceID = Request("ID")
        oResLetter2Source.UserID = Session("UserDBID")
        oResLetter2Source.Load()
        If Request("ID") = 0 Then
            oResLetter2Source.ResLetterID = Request("LetterID")
        End If
        oResLetter2Source.SourceID = siSource.Selected_ID
        oResLetter2Source.Active = cbActive.Checked
        oResLetter2Source.Save()
        oResLetter2Source = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Sources();window.close();", True)
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oResLetter2Source As New clsReservationLetter2Source
            oResLetter2Source.ResLetter2SourceID = Request("ID")
            oResLetter2Source.Load()
            cbActive.Checked = oResLetter2Source.Active
            siSource.Connection_String = Resources.Resource.cns
            siSource.ComboItem = "ReservationSource"
            siSource.Selected_ID = oResLetter2Source.SourceID
            siSource.Label_Caption = ""
            siSource.Load_Items()
            oResLetter2Source = Nothing
        End If
    End Sub
End Class
