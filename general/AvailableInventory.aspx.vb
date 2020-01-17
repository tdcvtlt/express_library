Imports System.Data
Partial Class general_AvailableInventory
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If (IsNumeric(Request("FrequencyID")) And IsNumeric(Request("OccYear")) And IsNumeric(Request("SeasonID"))) Or IsNumeric(Request("ContractID")) Then
                Load_Inventory()
            Else
                Close()
            End If
        End If
    End Sub

    Private Sub Load_Inventory()
        Dim oInv As New clsSalesInventory
        If Request("ContractID") <> "" And IsNumeric(Request("ContractID")) Then
            Dim oCont As New clsContract
            oCont.ContractID = Request("ContractID")
            oCont.Load()
            gvInventory.DataSource = oInv.Get_Available_Inventory(oCont.FrequencyID, Year(oCont.OccupancyDate), oCont.SeasonID, txtFilter.Text)
        Else
            gvInventory.DataSource = oInv.Get_Available_Inventory(Request("FrequencyID"), Request("OccYear"), Request("SeasonID"), txtFilter.Text)
        End If
        Dim sKeys(0) As String
        sKeys(0) = "SalesInventoryID"
        gvInventory.DataKeyNames = sKeys
        gvInventory.DataBind()
    End Sub

    Private Sub Close()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Load_Inventory()
    End Sub

    Protected Sub gvInventory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvInventory.SelectedIndexChanged
        If Request("ContractID") = "" Then
            Dim dt As DataTable = Session("Inventory_Table")
            Dim row As DataRow
            Dim bAdd As Boolean = True
            For i = 0 To dt.Rows.Count - 1
                If CInt(dt.Rows(i)("SalesInventoryID")) = CInt(gvInventory.SelectedValue) Then bAdd = False
            Next
            If bAdd Then
                row = dt.NewRow
                row("SalesInventoryID") = gvInventory.SelectedValue
                row("Name") = gvInventory.SelectedRow.Cells(3).Text
                row("Week") = gvInventory.SelectedRow.Cells(1).Text
                row("Dirty") = True
                dt.Rows.Add(row)
                Session("Inventory_Table") = dt

                ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lbRefresh','');window.close();", True)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "alert('This piece of inventory has already been added to this contract.\nPlease select a different item.');", True)
            End If
        Else
            If CheckSecurity("Inventory", "Assign", , , Session("UserDBID")) Then
                Dim oCont As New clsContract
                oCont.ContractID = Request("ContractID")
                oCont.Load()
                Dim oSold As New clsSoldInventory
                Dim oSI As New clsSalesInventory
                oSI.SalesInventoryID = gvInventory.SelectedValue
                oSI.Load()
                oSold.UserID = Session("UserDBID")
                oSold.ContractID = oCont.ContractID
                oSold.FrequencyID = oCont.FrequencyID
                oSold.OccupancyYear = Year(oCont.OccupancyDate)
                oSold.SalesInventoryID = gvInventory.SelectedValue
                oSold.SalesPrice = 0
                oSold.Points = oSI.Points
                oSold.Save()
                Dim oHist As New clsSalesInventory2ContractHist
                oHist.UserID = Session("UserDBID")
                oHist.ContractID = oCont.ContractID
                oHist.Active = True
                oHist.DateAdded = Date.Now
                oHist.FrequencyID = oCont.FrequencyID
                oHist.HideFromContract = False
                oHist.OccupancyYear = Year(oCont.OccupancyDate)
                oHist.SeasonID = oCont.SeasonID
                oHist.SalesInventoryID = gvInventory.SelectedValue
                oHist.Save()
                oHist = Nothing
                oSI = Nothing
                oSold = Nothing
                oCont = Nothing
                ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('" & Request("linkid") & "','');", True)
                ClientScript.RegisterClientScriptBlock(Me.GetType, "close", "window.close();", True)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "close", "alert('You Do Not Have Permission To Assign Inventory');", True)
            End If
        End If
    End Sub
End Class
