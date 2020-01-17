
Partial Class Maintenance_EditRefurbHist
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request("ID") <> "" And Request("ID") > 0 Then
                siStatus.Connection_String = Resources.Resource.cns
                siStatus.ComboItem = "RefurbStatus"
                siStatus.Label_Caption = ""
                siStatus.Load_Items()
                Dim oQuickCheckHist As New clsRefurbHist
                Dim oPers As New clsPersonnel
                Dim oQC As New clsRefurb
                With oQuickCheckHist
                    .RefurbHistID = Request("ID")
                    .Load()
                    siStatus.Selected_ID = .StatusID
                    oPers.Lookup_User(.CreateById)
                    lblCreatedBy.Text = oPers.UserName
                    lblDateCreated.Text = .DateCreated
                    lblRefurbHistID.Text = .RefurbHistID
                    oQC.RefurbID = .RefurbID
                    oQC.Load()
                    lblRefurbName.Text = oQC.Name
                    lblStatusDate.Text = .StatusDate
                    oPers.Lookup_User(.UpdatedById)
                    lblUpdatedBy.Text = oPers.UserName
                    lvItems.DataSource = (New clsRefurbHist2Item).List(.RefurbHistID)
                    lvItems.DataBind()
                End With
                oQC = Nothing
                oPers = Nothing
                oQuickCheckHist = Nothing
            End If

        End If
    End Sub

    Private Sub lvItems_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles lvItems.ItemDataBound
        Dim rowView As System.Data.DataRowView
        rowView = CType(e.Item.DataItem, System.Data.DataRowView)
        CType(e.Item.FindControl("Checkbox"), CheckBox).Checked = rowView("Checked")

    End Sub

    Dim lastSupplierNameValue As String = Nothing
    Protected Function AddGroupingRowIfSupplierHasChanged() As String
        'Get the data field value of interest for this row
        Dim currentSupplierNameValue As String = Eval("Area").ToString()

        'Specify name to display if dataFieldValue is a database NULL
        If currentSupplierNameValue.Length = 0 Then
            currentSupplierNameValue = "Unknown"
        End If

        'See if there's been a change in value
        If lastSupplierNameValue <> currentSupplierNameValue Then
            'There's been a change! Record the change and emit the table row
            lastSupplierNameValue = currentSupplierNameValue

            Return String.Format("<tr class=""group""><td colspan=""5"">Area: {0}</td></tr><tr class=""header""><th>Checked</th><th>ID</th><th>Description</th><th>Checked By</th><th>Date Checked</th></tr>", currentSupplierNameValue)
        Else
            'No change, return an empty string
            Return String.Empty
        End If
    End Function
    Protected Sub btnComplete_Click(sender As Object, e As EventArgs) Handles btnComplete.Click
        Dim oQC As New clsRefurbHist
        oQC.RefurbHistID = Request("ID")
        oQC.Load()
        If siStatus.Selected_ID <> oQC.StatusID Then
            oQC.StatusDate = Date.Now
            oQC.StatusID = siStatus.Selected_ID
            oQC.UpdatedById = Session("UserDBID")
            oQC.Save()
        End If
        For Each item As ListViewItem In lvItems.Items
            If Not (IsNothing(item.dataitem)) Then
                Dim ck As CheckBox = item.FindControl("Checkbox")
                Dim rowView As System.Data.DataRowView = CType(item.dataitem, System.Data.DataRowView)
                Dim oQCHI As New clsRefurbHist2Item
                With oQCHI
                    .RefurbHist2Item = rowView("RefurbHist2Item")
                    .Load()
                    If .Checked <> ck.Checked Then
                        .Checked = ck.Checked
                        .CheckedById = Session("UserDBID")
                        .DateChecked = Date.Now
                        .Save()
                    End If
                End With
                oQCHI = Nothing
                rowView = Nothing
                ck = Nothing
                item = Nothing
            End If
        Next
    End Sub

End Class
