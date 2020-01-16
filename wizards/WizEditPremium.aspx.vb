Imports System.Data
Partial Class wizards_WizEditPremium
    Inherits System.Web.UI.Page
    Dim dt As DataTable


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oSecurity As New Security
        Dim oPrem As New clsPremium
        If Not (oSecurity.Is_Logged_On(Session)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "security", "window.close();", True)
            oSecurity = Nothing
            Exit Sub
        End If
        dt = Session("Premiums_Table")

        oSecurity = Nothing

        If Not IsPostBack Then

            For i = 1 To 10
                qtyAssigned.Items.Add(i)
            Next
            ddPremiumID.DataSource = oPrem.List_Active()
            ddPremiumID.DataTextField = "PremiumName"
            ddPremiumID.DataValueField = "PremiumID"
            ddPremiumID.DataBind()
            siStatus.Connection_String = Resources.Resource.cns
            siStatus.Label_Caption = ""
            siStatus.ComboItem = "PremiumStatus"
            siStatus.Load_Items()
            Fill_Object()
            If CheckSecurity("Tours", "EditPremiumAmount", , , Session("UserDBID")) Then
                txtCostEA.ReadOnly = False
            End If
        End If
    End Sub

    Private Sub Fill_Object()
        Dim bNewRow As Boolean = False
        Dim row As DataRow
        txtPremiumIssuedID.Text = IIf(IsNumeric(Request("PremiumIssuedID")), Request("PremiumIssuedID"), "0")
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                row = dt.Rows(i)
                If CInt(row("ID")) = CInt(txtPremiumIssuedID.Text) Then
                    bNewRow = False
                    Exit For
                Else
                    bNewRow = True
                End If
            Next
        Else
            bNewRow = True
        End If
        If bNewRow Then
            row = dt.NewRow
            row("ID") = 0
            row("QtyAssigned") = 0
            row("PremiumID") = ddPremiumID.SelectedValue
            row("StatusID") = 0
            Dim oPrem As New clsPremium
            oPrem.PremiumID = ddPremiumID.SelectedValue
            oPrem.Load()
            row("CostEA") = CDbl(oPrem.Cost)
            oPrem = Nothing
            dt.Rows.Add(row)
        End If

        txtCertificate.Text = row("Certificate") & ""
        txtCostEA.Text = CDbl(row("CostEA")) & ""
        QtyAssigned.SelectedValue = row("QtyAssigned")
        ddPremiumID.SelectedValue = row("PremiumID")
        siStatus.Selected_ID = row("StatusID")
        Session("Premiums_Table") = dt
    End Sub

    Private Sub Save_Values()
        Dim dt As DataTable = Session("Premiums_Table")
        Dim row As DataRow
        Dim bNewRow As Boolean = False
        Dim holder As Integer = 0

        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                row = dt.Rows(i)
                If CInt(row("ID")) <= 0 Then
                    If holder = 0 Then
                        holder = -1
                    Else
                        holder = holder - 1
                    End If
                End If
                If CInt(row("ID")) = CInt(txtPremiumIssuedID.Text) Then
                    bNewRow = False
                    Exit For
                Else
                    bNewRow = True
                End If
            Next
        End If

        If bNewRow Then row = dt.NewRow
        If txtPremiumIssuedID.Text = 0 Then
            row("ID") = holder
        Else
            row("ID") = txtPremiumIssuedID.Text
        End If
        'row("ProspectID") = txtProspectID.Text
        row("Certificate") = txtCertificate.Text
        row("CostEA") = txtCostEA.Text
        row("QtyAssigned") = qtyAssigned.SelectedValue
        row("PremiumID") = ddPremiumID.SelectedValue
        row("Premium") = ddPremiumID.SelectedItem.Text
        row("StatusID") = siStatus.Selected_ID
        row("Status") = siStatus.SelectedName
        row("TotalCost") = CDbl(txtCostEA.Text) * CInt(qtyAssigned.SelectedValue)
        row("Dirty") = True
        If bNewRow Then dt.Rows.Add(row)

        '        Session("Premiums_Table") = dt
        Session("EditPremiums") = dt
        Session("Premiums_Table") = Session("EditPremiums")
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Save_Values()
        'Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lblRefresh2','');window.close();", True)

    End Sub

    Protected Sub ddPremiumID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddPremiumID.SelectedIndexChanged
        Dim oPrem As New clsPremium
        Try
            oPrem.PremiumID = ddPremiumID.SelectedValue
            oPrem.Load()
            txtCostEA.Text = CDbl(oPrem.Cost)
        Catch ex As Exception
            lblPremErr.Text = oPrem.Error_Message
        End Try
        oPrem = Nothing
    End Sub
End Class
