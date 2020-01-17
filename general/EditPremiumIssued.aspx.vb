
Partial Class general_EditPremiumIssued
    Inherits System.Web.UI.Page
    Dim oPI As clsPremiumIssued
    Dim bLoading As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request("kf") = "" Or Not (IsNumeric(Request("kv"))) Or Request("kv") = "0" Or Request("kv") = 0 Or Request("kv") = "" Then
                Close()
            Else
                hfKeyField.Value = Request("kf")
                hfKeyValue.Value = Request("kv")
                hfPIID.Value = Request("piid")
                hfPB.Value = Request("pb")

            End If
            Load_Items()
        End If
    End Sub

    Private Sub Load_Items()
        oPI = New clsPremiumIssued
        oPI.PremiumIssuedID = Request("piid")
        oPI.Load()
        siStatus.Selected_ID = oPI.StatusID
        siStatus.Connection_String = Resources.Resource.cns
        siStatus.ComboItem = "PremiumStatus"
        siStatus.Load_Items()
        siStatus.Selected_ID = oPI.StatusID
        If hfKeyField.Value.ToString.ToUpper = "TOURID" And hfKeyValue.Value.ToString <> "0" Then
            Dim oTour As New clsTour
            oTour.TourID = hfKeyValue.Value
            oTour.Load()
            If oTour.TourDate <> "" Then
                If DateDiff(DateInterval.Day, CDate(oTour.TourDate), Date.Today) > 5 Then
                    If Not (CheckSecurity("Tours", "ModifyTourStatusExtended", , , Session("UserDBID"))) Then
                        siStatus.Read_Only = True
                        ddQuantity.Enabled = False
                        ddPremium.Enabled = False
                    End If
                End If

                If DateDiff(DateInterval.Day, Date.Today, CDate(oTour.TourDate)) >= -29 And DateDiff(DateInterval.Day, Date.Today, CDate(oTour.TourDate)) <= 0 Then
                    If CheckSecurity("Tours", "EditToBeMailed", , , Session("UserDBID")) Then
                        If New clsComboItems().Lookup_ID("PremiumStatus", "To Be Mailed") = oPI.StatusID Then
                            siStatus.Read_Only = False
                        End If
                    End If
                End If
            End If
            oTour = Nothing
        End If
        Load_Premiums()
        txtPremium.Text = Lookup_Premium(oPI.PremiumID)
        txtCertNumber.Text = oPI.CertificateNumber
        If CheckSecurity("Tours", "EditPremiumAmount", , , Session("UserDBID")) Then
            txtAmount.ReadOnly = False
            txtCBAmount.ReadOnly = False
        End If
        txtAmount.Text = oPI.CostEA
        txtCBAmount.Text = oPI.CBCostEA
        Load_Qty()
        Set_Qty(oPI.QtyAssigned)
        txtPremium.Visible = Not (txtPremium.Text = "")
        ddPremium.Visible = (txtPremium.Text = "")

        siStatus.Read_Only = False
        ddQuantity.Enabled = True
        ddPremium.Enabled = True
    End Sub

    Private Sub Load_Qty()
        Dim item As New ListItem("", 0)
        ddQuantity.Items.Add(item)
        For i = 1 To 10
            item = New ListItem(i, i)
            ddQuantity.Items.Add(item)
        Next
        item = Nothing
    End Sub

    Private Sub Set_Qty(ByVal iQty As Integer)
        For i = 0 To ddQuantity.Items.Count - 1
            If ddQuantity.Items(i).Value = iQty Then
                ddQuantity.SelectedIndex = i
                Exit For
            End If
        Next
    End Sub

    Private Sub Load_Premiums()
        Dim ds As New SqlDataSource(Resources.Resource.cns, "Select * from t_Premium where active = 1 or premiumid = " & oPI.PremiumID & " order by premiumname")
        Dim itmBlank As New ListItem("", 0)
        ddPremium.Items.Add(itmBlank)
        ddPremium.DataSource = ds
        ddPremium.DataTextField = "PremiumName"
        ddPremium.DataValueField = "PremiumID"
        ddPremium.AppendDataBoundItems = True
        ddPremium.DataBind()
        ds = Nothing
    End Sub

    Private Function Lookup_Premium(ByVal iID As Integer) As String
        Dim sRet As String = ""
        For i = 0 To ddPremium.Items.Count - 1
            If ddPremium.Items(i).Value = iID Then
                bLoading = True
                ddPremium.SelectedIndex = i
                bLoading = False
                sRet = ddPremium.Items(i).Text
                Exit For
            End If
        Next
        Return sRet
    End Function

    Private Sub Close()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        oPI = New clsPremiumIssued
        Dim oPrem As New clsPremium
        Dim oCombo As New clsComboItems
        oPI.PremiumIssuedID = hfPIID.Value
        oPI.Load()
        oPI.UserID = Session("UserDBID")
        oPrem.UserID = Session("UserDBID")
        If hfPIID.value = 0 Then
            oPI.CreatedByID = Session("UserDBID")
        End If
        oPI.PremiumID = ddPremium.SelectedValue
        oPI.CertificateNumber = txtCertNumber.Text
        oPI.CostEA = txtAmount.Text
        oPI.CBCostEA = txtCBAmount.Text
        oPI.QtyAssigned = ddQuantity.SelectedValue

        If hfPIID.Value = 0 And siStatus.SelectedName = "Issued" Then
            oPI.QtyIssued = IIf(siStatus.SelectedName = "Issued", ddQuantity.SelectedValue, 0)
            oPrem.PremiumID = ddPremium.SelectedValue
            oPrem.Load()
            If hfKeyField.Value.ToLower() = "tourid" Then oPrem.QtyOnHand = oPrem.QtyOnHand - ddQuantity.SelectedValue
            oPI.DateIssued = System.DateTime.Now.ToShortDateString
            oPI.IssuedByID = Session("UserDBID")
            oPrem.Save()
        ElseIf hfPIID.Value > 0 And (siStatus.SelectedName = "Issued" And oPI.StatusID <> siStatus.Selected_ID) Then
            oPI.QtyIssued = IIf(siStatus.SelectedName = "Issued", ddQuantity.SelectedValue, 0)
            oPrem.PremiumID = ddPremium.SelectedValue
            oPrem.Load()
            If hfKeyField.Value.ToLower() = "tourid" Then oPrem.QtyOnHand = oPrem.QtyOnHand - oPI.QtyIssued
            oPI.DateIssued = System.DateTime.Now.ToShortDateString
            oPI.IssuedByID = Session("UserDBID")
            oPrem.Save()
        ElseIf hfPIID.Value > 0 And (siStatus.Selectedname <> "Issued" And oCombo.Lookup_ComboItem(oPI.StatusID) = "Issued") Then
            oPrem.PremiumID = ddPremium.SelectedValue
            oPrem.Load()
            If hfKeyField.Value.ToLower() = "tourid" Then oPrem.QtyOnHand = oPrem.QtyOnHand + oPI.QtyIssued
            oPrem.Save()
            oPI.DateIssued = ""
            oPI.IssuedByID = 0
            oPI.QtyIssued = IIf(siStatus.SelectedName = "Issued", ddQuantity.SelectedValue, 0)
        ElseIf hfPIID.Value > 0 And (siStatus.Selectedname = "Issued" And siStatus.Selected_ID = oPI.StatusID And ddQuantity.SelectedValue <> oPI.QtyIssued) Then
            Dim diff As Integer = 0
            diff = ddQuantity.SelectedValue - oPI.QtyIssued
            oPrem.PremiumID = ddPremium.SelectedValue
            oPrem.Load()
            If hfKeyField.Value.ToLower() = "tourid" Then oPrem.QtyOnHand = oPrem.QtyOnHand - diff
            oPrem.Save()
            oPI.QtyIssued = IIf(siStatus.SelectedName = "Issued", ddQuantity.SelectedValue, 0)
        Else
            oPI.QtyIssued = IIf(siStatus.SelectedName = "Issued", ddQuantity.SelectedValue, 0)
        End If
        oPI.TotalCost = ddQuantity.SelectedValue * txtAmount.Text
        oPI.StatusID = siStatus.Selected_ID
        oPI.KeyField = hfKeyField.Value
        oPI.KeyValue = hfKeyValue.Value
        lblErr.Text = oPI.Error_Message '& oPrem.Error_Message
        If oPI.Save() Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "window.opener.__doPostBack('" & hfPB.Value & "','');", True)
            Close()
        Else
            lblErr.Text = oPI.Error_Message '& oPrem.Error_Message
        End If
        oPI = Nothing
        oPrem = Nothing
        oCombo = Nothing
    End Sub

    Protected Sub ddPremium_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddPremium.SelectedIndexChanged
        Dim oPrem As New clsPremium
        oPrem.PremiumID = ddPremium.SelectedValue
        oPrem.Load()
        txtAmount.Text = oPrem.Cost
        txtCBAmount.Text = oPrem.CBCost
        lblErr.Text = oPrem.Error_Message
        oPrem = Nothing
    End Sub
End Class
