
Partial Class Reports_Rentals_OnlineInventoryQuickCheck
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oCombo As New clsComboItems
            Dim i As Integer = 0
            For i = 1 To 30
                ddNights.Items.Add(i)
            Next
            ddUsageType.Items.Add(New ListItem("ALL", 0))
            ddUsageType.DataSource = oCombo.Load_ComboItems("ReservationType")
            ddUsageType.DataTextField = "ComboItem"
            ddUsageType.DataValueField = "ComboItemID"
            ddUsageType.AppendDataBoundItems = True
            ddUsageType.DataBind()
            ddUnitType.Items.Add(New ListItem("ALL", 0))
            ddUnitType.DataSource = oCombo.Load_ComboItems("UnitType")
            ddUnitType.DataTextField = "ComboItem"
            ddUnitType.DataValueField = "ComboItemID"
            ddUnitType.AppendDataBoundItems = True
            ddUnitType.DataBind()
            ddRoomType.Items.Add(New ListItem("ALL", 0))
            ddRoomType.Items.Add(New ListItem("1 BD", 1))
            ddRoomType.Items.Add(New ListItem("1BD-DWN", "1BD-DWN"))
            ddRoomType.Items.Add(New ListItem("1BD-UP", "1BD-UP"))
            ddRoomType.Items.Add(New ListItem("2 BD", 2))
            ddRoomType.Items.Add(New ListItem("3 BD", 3))
            ddRoomType.Items.Add(New ListItem("4 BD", 4))


            ddRoomSubType.Items.Add(New ListItem("ALL", 0))
            ddRoomSubType.DataSource = oCombo.Load_ComboItems("RoomSubType")
            ddRoomSubType.DataTextField = "ComboItem"
            ddRoomSubType.DataValueField = "ComboItemID"
            ddRoomSubType.AppendDataBoundItems = True
            ddRoomSubType.DataBind()
            oCombo = Nothing
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim oQC As New clsOnlineInventoryQuickCheck
        Dim opt As String = ""
        If rbType.SelectedIndex = 0 Then
            opt = "res"
        Else
            opt = "usage"
        End If
        litQC.Text = oQC.Run_Report(dteSDate.Selected_Date, dteEDate.Selected_Date, opt, ddNights.SelectedValue, ddUsageType.SelectedValue, ddUnitType.SelectedValue, ddRoomType.SelectedValue, ddRoomSubType.SelectedValue)
        oQC = Nothing
    End Sub
End Class
