Imports Microsoft.VisualBasic
Partial Class marketing_addSalesInventory
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        If CInt(ddStartWeek.SelectedValue) > CInt(ddEndWeek.SelectedValue) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('The Start Date Can Not Be Greater than the End Date.');", True)
        ElseIf siSeason.Selected_ID = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Select A Season.');", True)
        ElseIf siInventoryType.Selected_ID = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Select An Inventory Type.');", True)
        ElseIf txtBudgetedPrice.Text = "" Or Not (IsNumeric(txtBudgetedPrice.Text)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Enter a Valid Budgeted Price.');", True)
        ElseIf txtPoints.Text = "" Or Not (IsNumeric(txtPoints.Text)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Enter a Valid Points Value.');", True)
        Else
            Dim oInventory As New clsSalesInventory
            Dim oCombo As New clsComboItems
            Dim i As Integer = 0
            For i = CInt(ddStartWeek.SelectedValue) To CInt(ddEndWeek.SelectedValue)
                If oInventory.Item_Exists(Request("UnitID"), i) Then
                    oInventory.SalesInventoryID = 0
                    oInventory.Load()
                    oInventory.SeasonID = siSeason.Selected_ID
                    oInventory.TypeID = siInventoryType.Selected_ID
                    oInventory.Week = i
                    oInventory.UnitID = Request("UnitID")
                    oInventory.StatusID = oCombo.Lookup_ID("InventoryStatus", "Available")
                    oInventory.WeekTypeID = oCombo.Lookup_ID("WeekType", "Float")
                    oInventory.StatusDate = Date.Now
                    oInventory.Points = txtPoints.Text
                    oInventory.Save()
                End If
            Next i
            oInventory = Nothing
            oCombo = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Inventory();window.close();", True)
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            For i = 1 To 52
                ddStartWeek.Items.Add(i)
                ddEndWeek.Items.Add(i)
            Next
            siSeason.Connection_String = Resources.Resource.cns
            siSeason.ComboItem = "Season"
            siSeason.Label_Caption = ""
            siSeason.Load_Items()
            siInventoryType.Connection_String = Resources.Resource.cns
            siInventoryType.ComboItem = "InventoryType"
            siInventoryType.Label_Caption = ""
            siInventoryType.Load_Items()

        End If
    End Sub
End Class
