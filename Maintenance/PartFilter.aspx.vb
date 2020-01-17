Imports System.Data
Imports System.Data.SqlClient
Partial Class Maintenance_PartFilter
    Inherits System.Web.UI.Page
    Dim manualSearch As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Load_DDs()
            If Request("transType") = "pReq" Then
                table1.Rows(0).visible = True
                table2.Visible = True
            ElseIf Request("transType") = "move" Then
                    table1.Rows(5).visible = True
            ElseIf Request("transType") = "loan" Then
                    table1.Rows(6).visible = True
                    table1.Rows(7).visible = True
                Table1.Rows(8).Visible = True
            End If
        End If
    End Sub

    Protected Sub Load_DDs()
        Dim oRooms As New clsRooms
        ddMoveFrom.DataSource = oRooms.List_Rooms()
        ddMoveFrom.DataValueField = "RoomID"
        ddMoveFrom.DataTextField = "RoomNumber"
        ddMoveFrom.DataBind()
        ddRooms.DataSource = oRooms.List_Rooms()
        ddRooms.DataValueField = "RoomID"
        ddRooms.DataTextField = "RoomNumber"
        ddRooms.DataBind()
        oRooms = Nothing

        For i = 1 To 999
            ddQty.Items.Add(i)
            ddManualQty.Items.Add(i)
        Next i

        Dim oRequest As New clsRequest
        If Request("transType") = "pReq" Then
            ddFilter0.Items.Add(New ListItem("", "0"))
            ddFilter1.Items.Add(New ListItem("", "0"))
            ddFilter2.Items.Add(New ListItem("", "0"))
            ddFilter3.Items.Add(New ListItem("", "0"))
            ddFilter0.DataSource = oRequest.List_Categories(0, "")
            ddFilter0.DataValueField = "Category"
            ddFilter0.DataTextField = "Category"
            ddFilter0.AppendDataBoundItems = True
            ddFilter0.DataBind()
        Else
            oRequest.RequestID = Request("RequestID")
            oRequest.Load()
            ddFilter1.Items.Add(New ListItem("", "0"))
            ddFilter2.Items.Add(New ListItem("", "0"))
            ddFilter3.Items.Add(New ListItem("", "0"))
            If Request("partLocation") = "Project" Then
                ddFilter1.DataSource = oRequest.List_Categories(1, "MAINT")
            Else
                ddFilter1.DataSource = oRequest.List_Categories(1, oRequest.CategoryID)
            End If

            ddFilter1.DataValueField = "Category"
            ddFilter1.DataTextField = "Category"
            ddFilter1.AppendDataBoundItems = True
            ddFilter1.DataBind()
        End If
        oRequest = Nothing

    End Sub

    Protected Sub ddFilter0_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFilter0.SelectedIndexChanged
        Dim oRequest As New clsRequest
        ddFilter1.Items.Clear()
        ddFilter2.Items.Clear()
        ddFilter3.Items.Clear()
        ddFilter1.Items.Add(New ListItem("", "0"))
        ddFilter2.Items.Add(New ListItem("", "0"))
        ddFilter3.Items.Add(New ListItem("", "0"))
        ddFilter1.DataSource = oRequest.List_Categories(1, ddFilter0.SelectedValue)
        ddFilter1.DataValueField = "Category"
        ddFilter1.DataTextField = "Category"
        ddFilter1.AppendDataBoundItems = True
        ddFilter1.DataBind()
        oRequest = Nothing
    End Sub

    Protected Sub ddFilter1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFilter1.SelectedIndexChanged
        Dim oRequest As New clsRequest
        ddFilter2.Items.Clear()
        ddFilter3.Items.Clear()
        ddFilter2.Items.Add(New ListItem("", "0"))
        ddFilter3.Items.Add(New ListItem("", "0"))
        If Request("transType") = "pReq" Then
            ddFilter2.DataSource = oRequest.List_Categories(2, ddFilter0.SelectedValue, ddFilter1.SelectedValue)
        Else
            oRequest.RequestID = Request("RequestID")
            oRequest.Load()
            ddFilter2.DataSource = oRequest.List_Categories(2, oRequest.CategoryID, ddFilter1.SelectedValue)
        End If
        ddFilter2.DataValueField = "Category"
        ddFilter2.DataTextField = "Category"
        ddFilter2.AppendDataBoundItems = True
        ddFilter2.DataBind()
        oRequest = Nothing
    End Sub

    Protected Sub ddFilter2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFilter2.SelectedIndexChanged
        Dim oRequest As New clsRequest
        ddFilter3.Items.Clear()
        ddFilter3.Items.Add(New ListItem("", "0"))
        If Request("transType") = "pReq" Then
            ddFilter3.DataSource = oRequest.List_Categories(3, ddFilter1.SelectedValue, ddFilter1.SelectedValue, ddFilter2.SelectedValue)
        Else
            oRequest.RequestID = Request("RequestID")
            oRequest.Load()
            ddFilter3.DataSource = oRequest.List_Categories(3, oRequest.CategoryID, ddFilter1.SelectedValue, ddFilter2.SelectedValue)
        End If
        ddFilter3.DataValueField = "Category"
        ddFilter3.DataTextField = "Category"
        ddFilter3.AppendDataBoundItems = True
        ddFilter3.DataBind()
        oRequest = Nothing
    End Sub
    Protected Sub btnManual_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim opRequestParts As New clsRequestParts
        Dim opRequestItems As New clsPurchaseRequestItems
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        If opRequestParts.Validate_GP_Part(txtItem.Text) > 0 Then
            If opRequestParts.Get_MinOrder_Amt(txtItem.Text) > ddManualQty.SelectedValue Then
                bProceed = False
                sErr = "Minimum Order Amount For This Item is " & opRequestParts.Get_MinOrder_Amt(txtItem.Text)
            End If
        End If
        If bProceed Then
            opRequestItems.Item2RequestID = 0
            opRequestItems.Load()
            opRequestItems.PurchaseRequestID = Request("PurchaseRequestID")
            opRequestItems.ItemNumber = txtItem.Text
            opRequestItems.Qty = ddManualQty.SelectedValue
            opRequestItems.Amount = txtCostEA.text
            opRequestItems.Save()
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Parts('" & Request("transType") & "');window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
        opRequestItems = Nothing
        opRequestParts = Nothing
    End Sub

    Protected Sub Unnamed3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oRequest As New clsRequest
        If Request("transType") = "pReq" Then
            If ddFilter0.SelectedValue = "0" Then
                gvParts.DataSource = oRequest.Search_Parts(0)
            ElseIf ddFilter0.SelectedValue <> "0" And ddFilter1.SelectedValue = "0" Then
                gvParts.DataSource = oRequest.Search_Parts(1, ddFilter0.SelectedValue)
            ElseIf ddFilter0.SelectedValue <> "0" And ddFilter1.SelectedValue <> "0" And ddFilter2.SelectedValue = "0" Then
                gvParts.DataSource = oRequest.Search_Parts(2, ddFilter0.SelectedValue, ddFilter1.SelectedValue)
            ElseIf ddFilter0.SelectedValue <> "0" And ddFilter1.SelectedValue <> "0" And ddFilter2.SelectedValue <> "0" And ddFilter3.SelectedValue = "0" Then
                gvParts.DataSource = oRequest.Search_Parts(3, ddFilter0.SelectedValue, ddFilter1.SelectedValue, ddFilter2.SelectedValue)
            Else
                gvParts.DataSource = oRequest.Search_Parts(4, ddFilter0.SelectedValue, ddFilter1.SelectedValue, ddFilter2.SelectedValue, ddFilter3.SelectedValue)
            End If
        Else
            oRequest.RequestID = Request("RequestID")
            oRequest.Load()
            If ddFilter1.SelectedValue = "0" Then
                gvParts.DataSource = oRequest.Search_Parts(1, oRequest.CategoryID)
            ElseIf ddFilter1.SelectedValue <> "0" And ddFilter2.SelectedValue = "0" Then
                gvParts.DataSource = oRequest.Search_Parts(2, oRequest.CategoryID, ddFilter1.SelectedValue)
            ElseIf ddFilter1.SelectedValue <> "0" And ddFilter2.SelectedValue <> "0" And ddFilter3.SelectedValue = "0" Then
                gvParts.DataSource = oRequest.Search_Parts(3, oRequest.CategoryID, ddFilter1.SelectedValue, ddFilter2.SelectedValue)
            Else
                gvParts.DataSource = oRequest.Search_Parts(4, oRequest.CategoryID, ddFilter1.SelectedValue, ddFilter2.SelectedValue, ddFilter3.SelectedValue)
            End If
        End If
        Dim sKeys(0) As String
        sKeys(0) = "ITEMNMBR"
        gvParts.DataKeynames = sKeys
        gvParts.DataBind()
    End Sub

    Protected Sub gvParts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvParts.SelectedIndexChanged
        Dim row As GridViewRow = gvParts.SelectedRow
        If Request("partLocation") = "Project" Then
            Dim oPrPart As New clsProject2Item
            oPrPart.ItemNumber = row.Cells(1).Text
            oPrPart.Qty = ddQty.SelectedValue
            oPrPart.ProjectID = Request("RequestID")
            oPrPart.Save()
            oPrPart = Nothing
        Else
            If Request("transType") = "addPart" Then
                Dim trueOnHand As Integer = 0
                Dim availOnHand As Integer = 0
                Dim amtAfter As Integer = 0
                Dim underParAmt As Integer = 0
                Dim itemParAmt As Integer
                Dim oRequestPart As New clsRequestParts
                Dim oLoanPart As New clsRequestPartLoan
                Dim oPReqItem As New clsPurchaseRequestItems
                Dim pRequest As New clsPurchaseRequest
                Dim oComboItem As New clsComboItems
                Dim minOrderAmt As Integer = 0
                trueOnHand = oRequestPart.Get_GP_Count(row.Cells(1).Text) - oRequestPart.Get_Installed_Count(row.Cells(1).Text, 0) - oLoanPart.Get_Loaned_Count(row.Cells(1).text)
                availOnHand = trueOnHand - oRequestPart.Get_Assigned_Count(row.Cells(1).Text)


                If availOnHand >= ddQty.SelectedValue Then
                    itemParAmt = oRequestPart.Get_GP_Par(row.Cells(1).Text)
                    amtAfter = (availOnHand - ddQty.SelectedValue) + oPReqItem.Get_ItemOrdered_Amount(row.Cells(1).Text)
                    If amtAfter < itemParAmt Then
                        underParAmt = itemParAmt - amtAfter
                        minOrderAmt = oRequestPart.Get_MinOrder_Amt(row.Cells(1).Text)
                        pRequest.PurchaseRequestID = 0
                        pRequest.Load()
                        pRequest.Approved = "0"
                        pRequest.POID = 0
                        pRequest.VendorID = "0"
                        pRequest.CreatedById = Session("UserDBID")
                        pRequest.DateCreated = System.DateTime.Now
                        pRequest.Save()
                        Dim prID As Integer = pRequest.PurchaseRequestID
                        oPReqItem.Item2RequestID = 0
                        oPReqItem.Load()
                        oPReqItem.ItemNumber = row.Cells(1).Text
                        oPReqItem.Note = "Avail on hand greater than qty"
                        oPReqItem.PurchaseRequestID = prID
                        If underParAmt > minOrderAmt Then
                            oPReqItem.Qty = underParAmt
                        Else
                            oPReqItem.Qty = minOrderAmt
                        End If
                        oPReqItem.Amount = oRequestPart.Get_MinOrder_Amt(row.Cells(1).text)
                        oPReqItem.Save()
                    End If
                    oRequestPart.Part2RequestID = 0
                    oRequestPart.Load()
                    oRequestPart.RequestID = Request("RequestID")
                    oRequestPart.ItemNumber = row.Cells(1).Text
                    oRequestPart.Qty = ddQty.SelectedValue
                    oRequestPart.StatusID = oComboItem.Lookup_ID("PartStatus", "Assigned")
                    oRequestPart.PRID = 0
                    oRequestPart.Save()
                Else
                    Dim totalQty As Integer = 0
                    totalQty = pRequest.PR_Part_Search(row.Cells(1).Text, ddQty.SelectedValue, availOnHand, Request("RequestID"))
                    If totalQty > 0 Then
                        oRequestPart.Part2RequestID = 0
                        oRequestPart.Load()
                        oRequestPart.RequestID = Request("RequestID")
                        oRequestPart.ItemNumber = row.Cells(1).Text
                        oRequestPart.Qty = totalQty
                        oRequestPart.StatusID = oComboItem.Lookup_ID("PartStatus", "Assigned")
                        oRequestPart.PRID = 0
                        oRequestPart.Save()
                    End If
                End If
                oRequestPart = Nothing
                oLoanPart = Nothing
                oPReqItem = Nothing
                pRequest = Nothing
                oComboItem = Nothing
            ElseIf Request("transType") = "move" Then
                Dim oRequest As New clsRequest
                Dim orequestMove As New clsRequestPartMoves
                Dim oCombo As New clsComboItems
                oRequest.RequestID = Request("RequestID")
                oRequest.Load()
                orequestMove.RequestPartMoveID = 0
                orequestMove.Load()
                orequestMove.RequestID = Request("RequestID")
                orequestMove.ItemNumber = row.Cells(1).text
                orequestMove.Qty = ddQty.SelectedValue
                orequestMove.RoomFrom = ddMoveFrom.SelectedValue
                orequestMove.RoomTo = oRequest.KeyValue
                orequestMove.StatusID = oCombo.Lookup_ID("PartMoveStatus", "Requested")
                orequestMove.Save()
                oRequest = Nothing
                orequestMove = Nothing
                oCombo = Nothing
            ElseIf Request("transType") = "loan" Then
                Dim oRes As New clsReservations
                Dim bProceed As Boolean = True
                Dim sErr As String = ""
                If Not (oRes.val_ResID(txtReservationID.Text)) Then
                    bProceed = False
                    sErr = "Please Enter A Valid ReservationID."
                ElseIf dteReturnDate.Selected_Date = "" Then
                    bProceed = False
                    sErr = "Please Enter A Return Date."
                End If
                If bProceed Then
                    Dim oRequestPart As New clsRequestParts
                    Dim oLoanPart As New clsRequestPartLoan
                    Dim oCombo As New clsComboItems
                    Dim trueOnHand As Integer = 0
                    trueOnHand = oRequestPart.Get_GP_Count(row.Cells(1).Text) - oRequestPart.Get_Installed_Count(row.Cells(1).Text, 0) - oLoanPart.Get_Loaned_Count(row.Cells(1).text)
                    If trueOnHand - ddQty.SelectedValue > 0 Then
                        oLoanPart.RequestPartLoanID = 0
                        oLoanPart.Load()
                        oLoanPart.ItemNumber = row.Cells(1).Text
                        oLoanPart.RequestID = Request("RequestID")
                        oLoanPart.RoomID = ddRooms.SelectedValue
                        oLoanPart.ReservationID = txtReservationID.Text
                        oLoanPart.Qty = ddQty.SelectedValue
                        oLoanPart.DateLoaned = System.DateTime.Now
                        oLoanPart.DatePickedUp = dteReturnDate.Selected_Date
                        oLoanPart.StatusID = oCombo.Lookup_ID("PartLoanStatus", "OnLoan")
                        oLoanPart.Save()
                    Else
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('The qty requested is not available to be loaned.');", True)
                    End If
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
                End If
            ElseIf Request("transType") = "pReq" Then
                Dim bProceed As Boolean = True
                Dim oRequestPart As New clsRequestParts
                Dim sErr As String = ""
                If oRequestPart.Get_MinOrder_Amt(row.Cells(1).Text) > ddQty.SelectedValue Then
                    bProceed = False
                    sErr = "Miniumum order Amount for this Item Is " & oRequestPart.Get_MinOrder_Amt(row.Cells(1).Text) & "."
                End If
                If bProceed Then
                    Dim opRequestItems As New clsPurchaseRequestItems
                    opRequestItems.Item2RequestID = 0
                    opRequestItems.Load()
                    opRequestItems.PurchaseRequestID = Request("PurchaseRequestID")
                    opRequestItems.Qty = ddQty.SelectedValue
                    opRequestItems.ItemNumber = row.Cells(1).Text
                    opRequestItems.Amount = oRequestPart.Get_ItemOrder_Amt(row.Cells(1).Text)
                    opRequestItems.Save()
                    opRequestItems = Nothing
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
                End If
            End If
        End If
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Parts('" & Request("transType") & "');window.close();", True)
    End Sub
End Class
