
Partial Class setup_EditHotelAccom
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            siRoomType.Label_Caption = ""
            siRoomType.Connection_String = Resources.Resource.cns
            siRoomType.ComboItem = "AccomRoomType"
            siRoomType.Load_Items()
            Dim oRateTable As New clsRateTable
            ddRateTable.Items.Add(New ListItem("", 0))
            ddRateTable.DataSource = oRateTable.Get_Rate_Tables
            ddRateTable.DataTextField = "Name"
            ddRateTable.DataValueField = "RateTableID"
            ddRateTable.AppendDataBoundItems = True
            ddRateTable.DataBind()
            oRateTable = Nothing
            If Request("ID") > 0 Then
                Load_Items(Request("ID"))
            End If
        End If
    End Sub

    Private Sub Load_Items(ByVal ID As Integer)
        Dim oAccom2Hotel As New clsAccom2RoomType
        oAccom2Hotel.Accom2RoomTypeID = ID
        oAccom2Hotel.Load()
        siRoomType.Selected_ID = oAccom2Hotel.RoomTypeID
        txtMaxOcc.Text = oAccom2Hotel.MaxOccupancy
        ddRateTable.SelectedValue = oAccom2Hotel.RateTableID
        oAccom2Hotel = Nothing
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        If siRoomType.Selected_ID < 1 Or txtMaxOcc.Text = "" Or Not (IsNumeric(txtMaxOcc.Text)) Then
            bProceed = False
            sErr = "Please Fill in All Values"
        End If
        If bProceed Then
            Dim oAccom2Hotel As New clsAccom2RoomType
            oAccom2Hotel.Accom2RoomTypeID = Request("ID")
            oAccom2Hotel.Load()
            oAccom2Hotel.UserID = Session("UserDBID")
            oAccom2Hotel.RoomTypeID = siRoomType.Selected_ID
            oAccom2Hotel.MaxOccupancy = txtMaxOcc.Text
            oAccom2Hotel.RateTableID = ddRateTable.SelectedValue
            oAccom2Hotel.Save()
            oAccom2Hotel = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Hotel();window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)
        End If
    End Sub
End Class
