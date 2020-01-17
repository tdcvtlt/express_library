Imports System.Data
Partial Class Accounting_PurchaseRequestForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Print", "window.print();", True)
            Dim oPreq As New clsPurchaseRequest
            oPreq.PurchaseRequestID = Request("PurchaseRequestID")
            oPreq.Load()
            Label1.Text = oPreq.PurchaseRequestID
            If oPreq.DateCreated = "" Then
                lblDateCreated.Text = ""
            Else
                lblDateCreated.Text = CDate(oPreq.DateCreated).ToShortDateString
            End If

            If oPreq.CreatedById = 0 Then
                lblOrderedBy.Text = "System Generated"
            Else
                Dim opers As New clsPersonnel
                opers.PersonnelID = oPreq.CreatedById
                opers.Load()
                lblOrderedBy.Text = opers.UserName
                opers = Nothing
            End If
            If oPreq.VendorID = "0" Then
                lblVendor.Text = ""
            Else
                lblVendor.Text = oPreq.VendorID
            End If

            Dim opReqItems As New clsPurchaseRequestItems
            Dim dt As DataTable
            Dim i As Integer = 0
            Dim gTotal As Double = 0
            dt = opReqItems.List_PRItems(oPreq.PurchaseRequestID)
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    Dim tr As New TableRow
                    tr.BorderStyle = BorderStyle.Solid
                    Dim td As New TableCell
                    td.BorderStyle = BorderStyle.Solid
                    td.Text = i + 1
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.BorderStyle = BorderStyle.Solid
                    If IsDBNull(dt.Rows(i).Item("ItemNumber")) Then
                        td.Text = "&nbsp;"
                    Else
                        td.Text = dt.Rows(i).Item("ItemNumber")
                    End If
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.BorderStyle = BorderStyle.Solid
                    If IsDBNull(dt.Rows(i).Item("Qty")) Then
                        td.Text = "&nbsp;"
                    Else
                        td.Text = dt.Rows(i).Item("Qty")
                    End If
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.BorderStyle = BorderStyle.Solid
                    If IsDBNull(dt.Rows(i).Item("Description")) Then
                        td.Text = "&nbsp;"
                    Else
                        td.Text = dt.Rows(i).Item("Description")
                    End If
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.BorderStyle = BorderStyle.Solid
                    If IsDBNull(dt.Rows(i).Item("Location")) Then
                        td.Text = "&nbsp;"
                    Else
                        td.Text = dt.Rows(i).Item("Location")
                    End If
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.BorderStyle = BorderStyle.Solid
                    If IsDBNull(dt.Rows(i).Item("Purpose")) Then
                        td.Text = "&nbsp;"
                    Else
                        td.Text = dt.Rows(i).Item("Purpose")
                    End If
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.BorderStyle = BorderStyle.Solid
                    td.HorizontalAlign = HorizontalAlign.Right
                    If IsDBNull(dt.Rows(i).Item("Amount")) Then
                        td.Text = "&nbsp;"
                    Else
                        td.Text = dt.Rows(i).Item("Amount")
                    End If
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.BorderStyle = BorderStyle.Solid
                    td.HorizontalAlign = HorizontalAlign.Right
                    td.Text = FormatCurrency(dt.Rows(i).Item("Qty") * dt.Rows(i).Item("Amount"), 2)
                    tr.Cells.Add(td)
                    Table1.Rows.Add(tr)
                    gTotal = gTotal + dt.Rows(i).Item("Qty") * dt.Rows(i).Item("Amount")
                Next
            End If
            Dim tRow As New TableRow
            tRow.BorderStyle = BorderStyle.Solid
            Dim tCell As New TableCell
            tCell.BorderStyle = BorderStyle.Solid
            tCell.ColumnSpan = 6
            tCell.HorizontalAlign = HorizontalAlign.Right
            tCell.Font.Bold = True
            tCell.Text = "Grand Total"
            tRow.Cells.Add(tCell)
            tCell = New TableCell
            tCell.HorizontalAlign = HorizontalAlign.Right
            tCell.BorderStyle = BorderStyle.Solid
            tCell.ColumnSpan = 2
            tCell.Text = FormatCurrency(gTotal, 2)
            tCell.Font.Bold = True
            tRow.Cells.Add(tCell)
            Table1.Rows.Add(tRow)
            oPreq = Nothing
            opReqItems = Nothing
        End If
    End Sub
End Class
