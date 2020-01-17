
Partial Class marketing_Usages
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            siUnitType.Connection_String = Resources.Resource.cns
            siUnitType.ComboItem = "UnitType"
            siUnitType.Label_Caption = ""
            siUnitType.Load_Items()
            siUsageType.Connection_String = Resources.Resource.cns
            siUsageType.ComboItem = "ReservationType"
            siUsageType.Label_Caption = ""
            siUsageType.Load_Items()
            siUsageSubType.Connection_String = Resources.Resource.cns
            siUsageSubType.ComboItem = "ReservationSubType"
            siUsageSubType.Label_Caption = ""
            siUsageSubType.Load_Items()
            Dim oComboItems As New clsComboItems
            ddRoomType.DataSource = oComboItems.Load_ComboItems("RoomType")
            ddRoomType.DataTextField = "ComboItem"
            ddRoomType.DataValueField = "ComboItemID"
            ddRoomType.DataBind()
            oComboItems = Nothing

            ddContracts.Items.Add("")
            ddContracts.Items.Add("IIBULKBANK")
            ddContracts.Items.Add("IIPOINTSBULKBANK")
            ddContracts.Items.Add("KCP DEVELOPER")
            ddContracts.Items.Add("KCP POOL")
            ddContracts.Items.Add("KCPSPARE")
            ddContracts.Items.Add("PLAN WITH TAN")
            ddContracts.Items.Add("RCIBULKBANK")
            ddContracts.Items.Add("TRANSTAX")
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        lbRoomTypes.Items.Add(New listItem(ddRoomType.SelectedItem.Text, ddRoomType.SelectedValue))
        ddRoomType.Items.Remove(ddRoomType.SelectedItem)

    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        If lbRoomTypes.SelectedValue <> "" Then
            ddRoomType.Items.Add(New listItem(lbRoomTypes.SelectedItem.Text, lbRoomTypes.SelectedValue))
            lbRoomTypes.Items.Remove(lbRoomTypes.SelectedItem)
        End If
    End Sub

    Protected Sub gvUsages_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Usages" Then
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(10).Text = CDate(e.Row.Cells(10).Text).ToShortDateString
            End If
        End If
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        Dim rmType As String = "''"
        Dim conNum As String = ""
        If lbRoomTypes.Items.Count > 0 Then
            For i = 0 To lbRoomTypes.Items.Count - 1
                If rmType = "''" Then
                    rmType = "'" & lbRoomTypes.Items(i).Value & "'"
                Else
                    rmType = rmType & ",'" & lbRoomTypes.Items(i).value & "'"
                End If
            Next i
        End If
        If ddContracts.SelectedValue = "" And txtConNum.Text <> "" Then
            conNum = txtConNum.Text
        Else
            conNum = ddContracts.SelectedValue
        End If
        Dim oUsages As New clsUsage
        gvUsages.DataSource = oUsages.Search_Usages(txtUsageID.Text, siUnitType.Selected_ID, siUsageType.Selected_ID, siUsageSubType.Selected_ID, rmType, dteStartDate.Selected_Date.toString, dteEndDate.Selected_Date.toString, conNum)
        gvUsages.DataBind()
        lblErr.Text = oUsages.Err
        oUsages = Nothing

    End Sub

    Protected Sub gvUsages_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvUsages.SelectedIndexChanged
        Dim row As gridViewRow = gvUsages.SelectedRow
        Dim oUsage As New clsUsage
        oUsage.UsageID = row.Cells(1).Text
        oUsage.Load()
        Response.Redirect("EditContract.aspx?ContractID=" & oUsage.ContractID & "&UsageID=" & oUsage.UsageID)
        oUsage = Nothing
    End Sub
End Class
