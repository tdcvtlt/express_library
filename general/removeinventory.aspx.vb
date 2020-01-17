
Partial Class general_removeinventory
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not (IsNumeric(Request("ID"))) Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "close", "window.close();", True)
            Else
                Dim oSI As New clsSalesInventory2ContractHist
                oSI.SalesInventoryContractHistID = Request("ID")
                oSI.Load()
                Dim oSales As New clsSalesInventory
                oSales.SalesInventoryID = oSI.SalesInventoryID
                oSales.Load()
                Dim oUnit As New clsUnit
                oUnit.UnitID = oSales.UnitID
                oUnit.Load()
                Response.Write("Please select whether to Release or Delete <br />" & oUnit.Name & " week " & oSales.Week)
                oUnit = Nothing
                oSales = Nothing
                oSI = Nothing

            End If
        End If
    End Sub

    Protected Sub btnRelease_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRelease.Click
        If CheckSecurity("Inventory", "Release", , , Session("UserDBID")) Then
            Update(False)
            ClientScript.RegisterClientScriptBlock(Me.GetType, "CloseWindow", "window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "alert('You Do Not Have Permission To Release Inventory');", True)
        End If
    End Sub

    Private Sub Update(ByVal bRemove As Boolean)
        Dim oHist As New clsSalesInventory2ContractHist
        oHist.SalesInventoryContractHistID = Request("ID")
        oHist.UserID = Session("UserDBID")
        oHist.Load()
        oHist.HideFromContract = bRemove
        oHist.Active = False
        oHist.DateRemoved = Date.Now
        oHist.Save()
        Dim osi As New clsSoldInventory
        osi.ContractID = oHist.ContractID
        osi.SalesInventoryID = oHist.SalesInventoryID
        osi.SoldInventoryID = 0
        osi.Load()
        Response.Write(osi.Error_Message)
        Response.Write("<br>")
        Response.Write(oHist.ContractID)
        Response.Write("<br>")
        Response.Write(oHist.SalesInventoryID)
        Response.Write("<br>")
        Response.Write(osi.SoldInventoryID)
        osi.Delete()
        Response.Write("<br>")
        Response.Write(osi.Error_Message)
        Response.Write("<br>")
        osi = Nothing
        oHist = Nothing

        'Close()
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If CheckSecurity("Inventory", "Delete", , , Session("UserDBID")) Then
            Update(True)
            ClientScript.RegisterClientScriptBlock(Me.GetType, "CloseWindow", "window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "alert('You Do Not Have Permission to Delete Inventory');", True)
        End If
    End Sub

    Protected Sub Close()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "UpdateRefresh", "window.opener.__doPostBack('" & Request("LinkID") & "','');", True)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
    End Sub
End Class
