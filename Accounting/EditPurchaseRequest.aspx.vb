Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Partial Class Accounting_EditPurchaseRequest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Set_Values()
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub Set_Values()
        If Request("ID") = 0 Then
            txtStatus.Text = "Pending"
            btnVendor.Visible = True
            txtPurchaseReqID.Text = 0
        Else
            Dim opReq As New clsPurchaseRequest
            opReq.PurchaseRequestID = Request("ID")
            opReq.Load()
            txtPurchaseReqID.Text = opReq.PurchaseRequestID
            txtDateCreated.Text = opReq.DateCreated
            txtPurchaseOrderID.Text = opReq.POID
            If opReq.VendorID = "0" Or opReq.VendorID = "-1" Then
                txtVendor.Text = ""
            Else
                txtVendor.Text = opReq.VendorID
            End If
            If opReq.VendorID = "-1" Then
                txtVendorDesc.Text = opReq.VendorDescription
                Table1.Rows(3).Visible = True
            End If
            If opReq.Approved = "0" Then
                btnVendor.Visible = True
                txtStatus.text = "Pending"
                btnPrint.Visible = True
                btnApprove.Visible = True
                btnDeny.Visible = True
            ElseIf opReq.Approved = "-1" Then
                txtStatus.text = "Denied"
                btnSave.Visible = False
                btnPrint.Visible = False
                btnApprove.Visible = False
                btnDeny.Visible = False
            Else
                txtStatus.text = "Approved"
                btnSave.Visible = False
                btnPrint.Visible = False
                btnApprove.Visible = False
                btnDeny.Visible = False
            End If
            opReq = Nothing
        End If
    End Sub

    Protected Sub Request_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Request_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Items_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Items_Link.Click
        If txtPurchaseReqID.Text > 0 Then
            Dim opReqItems As New clsPurchaseRequestItems
            gvPRItems.DataSource = opReqItems.List_PRItems(txtPurchaseReqID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "Item2RequestID"
            gvPRItems.DataKeyNames = sKeys
            gvPRItems.DataBind()
            MultiView1.ActiveViewIndex = 1
        End If
    End Sub


    Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprove.Click
        If CheckSecurity("PurchaseRequests", "ApprovePR", , , Session("UserDBID")) Then
            Dim opreq As New clsPurchaseRequest
            Dim bProceed As Boolean = True
            Dim err As String = ""
            opreq.PurchaseRequestID = txtPurchaseReqID.Text
            opreq.Load()
            opreq.UserID = Session("UserDBID")
            '*****Vendor Check'
            If bProceed Then
                If opreq.VendorID = "0" Or opreq.VendorID = "-1" Then
                    bProceed = False
                    err = "Please Assign A Vendor."
                End If
            End If

            '****Items Check
            If bProceed Then
                If opreq.Get_TotalItem_Count(txtPurchaseReqID.Text) = 0 Then
                    bProceed = False
                    err = "Please Add Items to Purchase Request Before Approving."
                End If
            End If

            '****Approve
            If bProceed Then
                opreq.Approved = "1"
                opreq.Save()
                Response.Redirect("EditPurchaseRequest.aspx?ID=" & opreq.PurchaseRequestID)
                'Set_Values()
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & err & "');", True)
            End If
            opreq = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Access Denied.');", True)
        End If
    End Sub

    Protected Sub btnDeny_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeny.Click
        If CheckSecurity("PurchaseRequests", "ApprovePR", , , Session("UserDBID")) Then
            Dim opreq As New clsPurchaseRequest
            Dim bProceed As Boolean = True
            Dim err As String = ""
            opreq.PurchaseRequestID = txtPurchaseReqID.Text
            opreq.Load()
            opreq.UserID = Session("UserDBID")
            If bProceed Then
                opreq.Approved = "-1"
                opreq.Save()
                If opreq.Cancel_PR_Parts(txtPurchaseReqID.Text) Then
                    Response.Redirect("EditPurchaseRequest.aspx?ID=" & opreq.PurchaseRequestID)
                    '                    Set_Values()
                Else
                    '*****Error
                End If
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & err & "');", True)
            End If
            opreq = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Access Denied.');", True)
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim bProceed As Boolean = True
        If CheckSecurity("PurchaseRequests", "EditPR", , , Session("UserDBID")) Then
            If bProceed = True Then
                Dim opReq As New clsPurchaseRequest
                opReq.PurchaseRequestID = Request("ID")
                opReq.Load()
                opReq.UserID = Session("UserDBID")
                If Request("ID") = "0" Then
                    opReq.DateCreated = System.DateTime.Now
                    opReq.CreatedById = Session("UserDBID")
                    opReq.Approved = "0"
                    opReq.POID = 0
                    'opReq.Save()
                End If
                Response.Write(txtVendor.Text)
                If hfVendorID.Value = "" Then 'txtVendor.Text = "" Then
                    opReq.VendorID = "0"
                Else
                    opReq.VendorID = hfVendorID.Value
                    'opReq.VendorID = txtVendor.Text
                End If
                opReq.VendorDescription = txtVendorDesc.Text
                opReq.Save()
                lblPRErr.Text = opReq.Err
                'If Request("ID") = "0" Then
                Response.Redirect("EditPurchaseRequest.aspx?ID=" & opReq.PurchaseRequestID)
                'Else
                '   Set_Values()
                'End If
                opReq = Nothing
            Else
                '****Alert Permission
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Access Denied');", True)
        End If

    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim opReq As New clsPurchaseRequest
        opReq.PurchaseRequestID = txtPurchaseReqID.Text
        opReq.Load()
        If opReq.Approved = "0" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Maintenance/partfilter.aspx?PurchaseRequestID=" & txtPurchaseReqID.Text & "&transType=pReq','win01',690,450);", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Items May Only Be Added To Pending Purchase Requests.');", True)
        End If
        opReq = Nothing
    End Sub

    Protected Sub gvPRItems_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                If e.Row.Cells(7).Text = "TRUE" Then
                    e.Row.Cells(8).Visible = True
                Else
                    e.Row.Cells(8).Visible = False
                End If
                If e.Row.Cells(9).Text = "TRUE" Then
                    e.Row.Cells(10).Visible = True
                Else
                    e.Row.Cells(10).Visible = False
                End If
            End If
            e.Row.Cells(0).Visible = False
            e.Row.Cells(7).Visible = False
            e.Row.Cells(9).Visible = False
        End If
    End Sub
    Protected Sub btnVendor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVendor.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Accounting/vendorFilter.aspx?ID=" & txtPurchaseReqID.Text & "','win01',690,450);", True)
    End Sub
    Protected Sub gvPRItems_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvPRItems.RowCommand
        Dim partID As Integer
        partID = Convert.ToInt32(gvPRItems.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        If e.CommandName.CompareTo("EditItem") = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Accounting/editPRItem.aspx?ID=" & partID & "','win01',350,350);", True)
        ElseIf e.CommandName.CompareTo("RemovePart") = 0 Then
            Dim bProceed As Boolean = True
            Dim sErr As String = ""
            Dim opRequest As New clsPurchaseRequest
            Dim opRequestItem As New clsPurchaseRequestItems

            '*****Security Check

            If bProceed Then
                Dim qtyOrdered As Integer = 0
                Dim qtyRequested As Integer = 0
                opRequestItem.Item2RequestID = partID
                opRequestItem.Load()
                opRequest.PurchaseRequestID = opRequestItem.PurchaseRequestID
                opRequest.Load()
                If opRequest.Approved = "1" Then
                    bProceed = False
                    sErr = "Items Can Not Be Removed From Approved Purchase Requests."
                End If
                If bProceed Then
                    qtyOrdered = opRequest.Get_Ordered_Count_PR(opRequestItem.ItemNumber, opRequestItem.PurchaseRequestID, opRequestItem.Item2RequestID)
                    qtyRequested = opRequest.Get_Requested_Count_PR(opRequestItem.ItemNumber, opRequestItem.PurchaseRequestID)
                    If qtyRequested = 0 Or (qtyOrdered - qtyRequested) >= 0 Then
                        bProceed = True
                    Else
                        bProceed = False
                        sErr = "Item Is Tied To Pending Maintenance Requests And Can Not Be Removed."
                    End If
                    If bProceed Then
                        opRequestItem.Remove_PRItem(opRequestItem.Item2RequestID)
                        gvPRItems.DataSource = opRequestItem.List_PRItems(txtPurchaseReqID.Text)
                        Dim sKeys(0) As String
                        sKeys(0) = "Item2RequestID"
                        gvPRItems.DataKeyNames = sKeys
                        gvPRItems.DataBind()
                    Else
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
                    End If
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
                End If
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
            End If
        End If


    End Sub

    Protected Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Accounting/PurchaseRequestForm.aspx?PurchaseRequestID=" & Request("ID") & "&cxl=True','win01',800,600);", True)
    End Sub

    Protected Sub Events_Link_Click(sender As Object, e As EventArgs) Handles Events_Link.Click
        If txtPurchaseReqID.Text > 0 Then
            MultiView1.ActiveViewIndex = 2
            Events1.KeyField = "PurchaseRequestID"
            Events1.KeyValue = txtPurchaseReqID.Text
            Events1.List()
        End If
    End Sub
End Class
