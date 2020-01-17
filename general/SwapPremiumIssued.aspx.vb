
Imports System.Data
Imports System.Data.SqlClient

Partial Class general_SwapPremiumIssued
    Inherits System.Web.UI.Page

    Private Sub Load_DropDowns()
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("select i.PremiumIssuedID, p.PremiumName + ' : ' + cast(i.QtyIssued as varchar(2)) as PremiumName from t_PremiumIssued i inner join t_Premium p on p.PremiumID=i.PremiumID inner join t_ComboItems pis on pis.ComboItemID=i.StatusID where pis.ComboItem='Issued' and i.QtyIssued > 0 and i.keyfield = '" & Request("Field") & "' and i.keyvalue = '" & Request("ID") & "' and p.PremiumName in (select premiumname from (select p.PremiumName, sum(i.QtyIssued) issued from t_PremiumIssued i inner join t_Premium p on p.PremiumID=i.PremiumID inner join t_ComboItems pis on pis.ComboItemID=i.StatusID where pis.ComboItem='Issued' and i.QtyIssued <> 0 and i.keyfield = '" & Request("Field") & "' and i.KeyValue = '" & Request("id") & "'  group by p.PremiumName)a where a.issued > 0)  order by p.PremiumName", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        da.Fill(ds, "Issued")
        cm.CommandText = "select p.PremiumID, p.PremiumName from t_Premium p where p.active = 1 order by p.PremiumName"
        da.Fill(ds, "Prems")
        ddIssued.DataSource = ds.Tables("Issued")
        ddIssued.DataTextField = "PremiumName"
        ddIssued.DataValueField = "PremiumIssuedID"
        ddIssued.DataBind()
        ddNew.DataSource = ds.Tables("Prems")
        ddNew.DataTextField = "PremiumName"
        ddNew.DataValueField = "PremiumID"
        ddNew.DataBind()
        For i = 1 To 10
            ddIssuedQty.Items.Add(New ListItem With {.Text = i, .Value = i})
            ddNewQty.Items.Add(New ListItem With {.Text = i, .Value = i})
        Next
        ddNew.SelectedIndex = ddNew.Items.IndexOf(New ListItem("None", "0"))
        cn = Nothing
        cm = Nothing
        da = Nothing
        ds = Nothing
    End Sub

    Protected Sub btnSwap_Click(sender As Object, e As EventArgs) Handles btnSwap.Click
        If ddIssued.Items.Count > 0 Then
            If lbNew.Items.Count > 0 Then
                'Lookup the issued record and confirm the qty <= qtyissued
                Dim cn As New SqlConnection(Resources.Resource.cns)
                Dim cm As New SqlCommand("Select * From t_PremiumIssued where premiumissuedid = " & ddIssued.SelectedValue, cn)
                Dim da As New SqlDataAdapter(cm)
                Dim ds As New DataSet

                da.Fill(ds, "Issued")
                Dim cost As Decimal = ddIssuedQty.SelectedValue * ds.Tables("Issued").Rows(0)("CostEa")
                Dim newCost As Decimal = 0

                Dim sIDs As String = ""
                For Each item As ListItem In lbNew.Items
                    sIDs &= IIf(sIDs = "", item.Value, "," & item.Value)
                Next
                cm.CommandText = "Select * From t_Premium where premiumid in (" & sIDs & ")"
                da.Fill(ds, "Check")
                Dim keys(0) As DataColumn
                keys(0) = ds.Tables("Check").Columns(0)
                ds.Tables("Check").PrimaryKey = keys
                For Each item As ListItem In lbNew.Items
                    Dim qty As Integer = item.Text.Split(":")(1)
                    newCost += qty * ds.Tables("Check").Rows.Find(item.Value)("Cost")
                Next



                lblErr.Text = ""
                If ddIssuedQty.SelectedValue > ds.Tables("Issued").Rows(0)("QtyIssued")  Then
                    lblErr.Text = "Cannot swap more " & IIf(Request("Field").ToLower() = "reservationid", "Gifts", "Premiums") & " than were issued"
                ElseIf cost < newCost and 1 = 0 Then
                    lblErr.Text = "" & IIf(Request("Field").ToLower() = "reservationid", "Gifts", "Premiums") & " can only be swapped for those with equal or lessor value.(" & cost & " - " & newCost & ")"
                Else
                    'Create the negative entry
                    Dim oPremIss As New clsPremiumIssued
                    Dim oPrem As New clsPremium
                    Dim row As Data.DataRow = ds.Tables("Issued").Rows(0)
                    Dim statusID As Integer = 0
                    Dim locationid As Integer = 0
                    With oPremIss
                        .QtyAssigned = -1 * ddIssuedQty.SelectedValue
                        .QtyIssued = -1 * ddIssuedQty.SelectedValue
                        .CBCostEA = .QtyIssued * row("CBCostEA")
                        .CertificateNumber = row("CertificateNumber") & ""
                        .CostEA = row("CostEA")
                        .CreatedByID = Session("UserDBID")
                        .CRMSID = 0
                        .DateCreated = Date.Now
                        .DateIssued = Date.Now
                        .KeyField = row("Keyfield")
                        .KeyValue = row("KeyValue")
                        .LocationID = row("LocationID")
                        .PremiumID = row("PremiumID")
                        .StatusID = row("StatusID")
                        .TotalCost = .QtyIssued * .CostEA
                        .UserID = Session("UserDBID")
                        .Save()
                        oPrem.PremiumID = .PremiumID
                        oPrem.Load()
                        If .KeyField.ToLower() = "tourid" Then oPrem.QtyOnHand = oPrem.QtyOnHand - .QtyIssued
                        oPrem.UserID = Session("UserDBID")
                        oPrem.Save()
                        statusID = .StatusID
                        locationid = .LocationID
                    End With
                    oPremIss = Nothing
                    oPrem = Nothing
                    For Each item As ListItem In lbNew.Items
                        If Not IsNothing(ds.Tables("New")) Then ds.Tables("New").Clear()
                        cm.CommandText = "Select * From t_Premium where premiumid = " & item.Value
                        da.Fill(ds, "New")
                        row = ds.Tables("New").Rows(0)
                        oPrem = New clsPremium
                        oPremIss = New clsPremiumIssued
                        With oPremIss
                            .QtyAssigned = item.Text.Split(":")(1)
                            .QtyIssued = item.Text.Split(":")(1)
                            .CBCostEA = .QtyIssued * row("CBCost")
                            .CertificateNumber = ""
                            .CostEA = row("Cost")
                            .CreatedByID = Session("UserDBID")
                            .CRMSID = 0
                            .DateCreated = Date.Now
                            .DateIssued = Date.Now
                            .KeyField = Request("Field")
                            .KeyValue = Request("ID")
                            .LocationID = locationid
                            .PremiumID = item.Value
                            .StatusID = statusID
                            .TotalCost = .QtyIssued * .CostEA
                            .UserID = Session("UserDBID")
                            .IssuedByID = Session("UserDBID")
                            .Save()
                            oPrem.PremiumID = .PremiumID
                            oPrem.Load()
                            If .KeyField.ToLower() = "tourid" Then oPrem.QtyOnHand = oPrem.QtyOnHand - .QtyIssued
                            oPrem.UserID = Session("UserDBID")
                            oPrem.Save()
                        End With
                        oPrem = Nothing
                        oPremIss = Nothing
                    Next
                    cn = Nothing
                    cm = Nothing
                    da = Nothing
                    ds = Nothing
                    row = Nothing
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "window.opener.__doPostBack('" & Request("pb") & "','');", True)
                    ClientScript.RegisterClientScriptBlock(Me.GetType(), "Close2", "window.close();", True)
                End If
            Else
                lblErr.Text = "Please Add at least one new " & IIf(Request("Field").ToLower() = "reservationid", "Gift", "Premium") & ""
            End If
        Else
            lblErr.Text = "There are no issued " & IIf(Request("Field").ToLower() = "reservationid", "Gifts", "Premiums") & " to swap"
        End If

    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Close", "window.close();", True)
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If Request("ID") <> "" And Request("Field") <> "" Then
                Load_DropDowns()
            End If
        End If
    End Sub
    Protected Sub ddIssued_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddIssued.SelectedIndexChanged
        Dim issued As Integer = (ddIssued.Items(ddIssued.SelectedIndex).Text.Split(":"))(1)
        ddIssuedQty.SelectedIndex = ddIssuedQty.Items.IndexOf(ddIssuedQty.Items.FindByValue(issued))
    End Sub
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim item As New ListItem
        item.Value = ddNew.SelectedValue
        item.Text = ddNew.SelectedItem.Text & " Qty:" & ddNewQty.SelectedValue
        lbNew.Items.Add(item)
    End Sub
    Protected Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If lbNew.Items.Count > 0 Then
            If Not IsNothing(lbNew.SelectedItem) Then
                lbNew.Items.Remove(lbNew.SelectedItem)
            End If
        End If
    End Sub
End Class
