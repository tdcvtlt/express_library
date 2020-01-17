Imports System.Data.SqlClient
Partial Class marketing_EditSalesInventory
    Inherits System.Web.UI.Page
    Dim oSI As clsSalesInventory
    Dim oUnit As clsUnit
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If CheckSecurity("Inventory", "View", , , Session("UserDBID")) Then
                oSI = New clsSalesInventory
                oUnit = New clsUnit
                Load_Controls()
                MultiView1.ActiveViewIndex = 0
                oSI.SalesInventoryID = IIf(IsNumeric(Request("SalesInventoryID")) And Request("SalesInventoryID") <> "", Request("SalesInventoryID"), 0)
                oSI.Load()
                oUnit.UnitID = IIf(oSI.SalesInventoryID > 0, oSI.UnitID, IIf(IsNumeric(Request("UnitID")) And Request("UnitID") <> "", Request("UnitID"), 0))
                oUnit.Load()
                Set_Values()
                oSI = Nothing
                oUnit = Nothing
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Denied", "alert('You do not have permission to view inventory');", True)
            End If
        End If

    End Sub

    Private Sub Load_Controls()
        siStatus.Connection_String = Resources.Resource.cns
        siSeason.Connection_String = Resources.Resource.cns
        siWeekType.Connection_String = Resources.Resource.cns
        siInventoryType.Connection_String = Resources.Resource.cns
        siInventorySubType.Connection_String = Resources.Resource.cns
        siStatus.ComboItem = "InventoryStatus"
        siSeason.ComboItem = "Season"
        siWeekType.ComboItem = "WeekType"
        siInventoryType.ComboItem = "InventoryType"
        siInventorySubType.ComboItem = "InventorySubType"
        siStatus.Label_Caption = ""
        siSeason.Label_Caption = ""
        siWeekType.Label_Caption = ""
        siInventoryType.Label_Caption = ""
        siInventorySubType.Label_Caption = ""
        siStatus.Load_Items()
        siSeason.Load_Items()
        siWeekType.Load_Items()
        siInventoryType.Load_Items()
        siInventorySubType.Load_Items()

    End Sub

    Private Sub Set_Values()
        lblUnit.Text = oUnit.Name
        lblWeek.Text = oSI.Week
        siSeason.Selected_ID = oSI.SeasonID
        siStatus.Selected_ID = oSI.StatusID
        dfStatusDate.Selected_Date = oSI.StatusDate
        siWeekType.Selected_ID = oSI.WeekTypeID
        siInventoryType.Selected_ID = oSI.TypeID
        siInventorySubType.Selected_ID = oSI.SubTypeID
        txtBudgetedPrice.Text = oSI.BudgetedPrice
        txtPoints.Text = oSI.Points
        lblID.Text = oSI.SalesInventoryID
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CheckSecurity("Inventory", "Edit", , , Session("UserDBID")) Then
            oSI = New clsSalesInventory
            oSI.SalesInventoryID = IIf(lblID.Text = "" Or lblID.Text = "0", 0, lblID.Text)
            oSI.Load()
            oSI.UnitID = IIf(oSI.UnitID > 0, oSI.UnitID, IIf(IsNumeric(Request("UnitID")) And Request("UnitID") <> "", Request("UnitID"), 0))
            oSI.Week = oSI.Week
            oSI.StatusDate = IIf(dfStatusDate.Selected_Date = "", Date.Today, IIf(oSI.StatusID <> siStatus.Selected_ID, Date.Today, dfStatusDate.Selected_Date))
            dfStatusDate.Selected_Date = oSI.StatusDate
            oSI.SeasonID = siSeason.Selected_ID
            oSI.WeekTypeID = siWeekType.Selected_ID
            oSI.TypeID = siInventoryType.Selected_ID
            oSI.SubTypeID = siInventorySubType.Selected_ID
            oSI.BudgetedPrice = IIf(txtBudgetedPrice.Text <> "" And IsNumeric(txtBudgetedPrice.Text), txtBudgetedPrice.Text, 0)
            oSI.Points = IIf(txtPoints.Text <> "" And IsNumeric(txtPoints.Text), txtPoints.Text, 0)
            oSI.Save()
            oSI = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Denied", "alert('You do not have permission to edit inventory');", True)
        End If
    End Sub

    Protected Sub SalesInventory_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalesInventory_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Contracts_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Contracts_Link.Click
        MultiView1.ActiveViewIndex = 1
        Load_Contracts()
    End Sub

    Protected Sub History_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles History_Link.Click
        MultiView1.ActiveViewIndex = 2
        Load_History()
    End Sub

    Protected Sub Events_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events_Link.Click
        MultiView1.ActiveViewIndex = 3
        cEvents.KeyField = "SalesInventoryID"
        cEvents.KeyValue = lblID.Text
        cEvents.List()
    End Sub

    Private Sub Load_Contracts()
        Dim con As New SqlDataSource
        con.ConnectionString = Resources.Resource.cns
        con.SelectCommand = "Select c.contractid as ID, c.contractnumber as KCP, f.Frequency,s.OccupancyYear, p.LastName as [Last Name], p.FirstName as [First Name] from t_Contract c inner join t_Prospect p on p.prospectid = c.prospectid inner join t_SoldInventory s on s.contractid = c.contractid inner join t_SalesInventory si on si.salesinventoryid = s.salesinventoryid inner join t_Frequency f on f.frequencyid = s.frequencyid where s.salesinventoryid = '" & Request("SalesInventoryID") & "'"
        gvContracts.DataSource = con
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvContracts.DataKeyNames = sKeys
        gvContracts.DataBind()
        con = Nothing
    End Sub

    Private Sub Load_History()
        Dim con As New SqlDataSource
        con.ConnectionString = Resources.Resource.cns
        con.SelectCommand = "Select c.contractid as ID, c.contractnumber as KCP, f.Frequency, p.LastName as [Last Name], p.FirstName as [First Name], s.DateAdded as [Date Added], s.DateRemoved as [Date Removed], cs.comboitem as Status from t_Contract c left outer join t_Comboitems cs on cs.comboitemid = c.statusid inner join t_Prospect p on p.prospectid = c.prospectid inner join t_SalesInventory2ContractHist s on s.contractid = c.contractid inner join t_SalesInventory si on si.salesinventoryid = s.salesinventoryid inner join t_Frequency f on f.frequencyid = s.frequencyid where s.salesinventoryid = '" & Request("SalesInventoryID") & "'"
        gvHistory.DataSource = con
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvHistory.DataKeyNames = sKeys
        gvHistory.DataBind()
        con = Nothing
    End Sub

    Protected Sub gvContracts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvContracts.SelectedIndexChanged
        Response.Redirect("editcontract.aspx?contractid=" & gvContracts.SelectedValue)
    End Sub

    Protected Sub gvHistory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvHistory.SelectedIndexChanged
        Response.Redirect("editcontract.aspx?contractid=" & gvHistory.SelectedValue)
    End Sub
End Class
