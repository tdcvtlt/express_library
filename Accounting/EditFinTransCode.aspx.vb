Imports System.Data.SqlClient
Imports System.Data

Partial Class Accounting_EditFinTransCode
    Inherits System.Web.UI.Page
    Dim oTransCode As New clsFinancialTransactionCodes

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            oTransCode.FinTransID = Request("ID")
            oTransCode.Load()
            Load_Items()
            Set_Values()
        End If
    End Sub

    Private Sub Load_Items()
        siTransCode.Connection_String = Resources.Resource.cns
        siTransType.Connection_String = Resources.Resource.cns
        ' siTransSubType.Connection_String = Resources.Resource.cns

        siTransCode.Selected_ID = oTransCode.TransCodeID
        siTransType.Selected_ID = oTransCode.TransTypeID
        'siTransSubType.Selected_ID = 0

        siTransCode.ComboItem = "TransCode"
        siTransType.ComboItem = "TransCodeType"
        'siTransSubType.ComboItem = "TransSubType"

        siTransCode.Load_Items()
        siTransType.Load_Items()
        'siTransSubType.Load_Items()

        siTransCode.Selected_ID = oTransCode.TransCodeID
        siTransType.Selected_ID = oTransCode.TransTypeID
        ' siTransSubType.Selected_ID = 0

        txtID.Text = oTransCode.FinTransID
        txtDesc.Text = oTransCode.Description
        txtAmount.Text = oTransCode.Amount

        Dim ds As New SqlDataSource(Resources.Resource.cns, "Select * from t_CCMerchantAccount order by description")
        ddAccount.DataSource = ds
        ddAccount.DataTextField = "Description"
        ddAccount.DataValueField = "AccountID"
        ddAccount.DataBind()
        ds = Nothing

        For i = 0 To ddAccount.Items.Count - 1
            If ddAccount.Items(i).Value = oTransCode.MerchantAccountID Then
                ddAccount.SelectedIndex = i
                Exit For
            End If
        Next

        ckActive.Checked = oTransCode.Active
        'ckPayment.Checked=oTransCode.
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        oTransCode.FinTransID = txtID.Text
        oTransCode.Load()
        oTransCode.TransCodeID = siTransCode.Selected_ID
        oTransCode.Description = txtDesc.Text
        oTransCode.Amount = txtAmount.Text
        oTransCode.TransTypeID = siTransType.Selected_ID
        oTransCode.MerchantAccountID = ddAccount.SelectedValue
        oTransCode.Active = ckActive.Checked
        If oTransCode.Save Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "window.opener.__doPostBack('" & Request("LinkID") & "','');", True)
            Close()
        Else
            lblError.Text = oTransCode.Error_Message
        End If
    End Sub

    Private Sub Set_Values()
        txtID.Text = oTransCode.FinTransID
        siTransCode.Selected_ID = oTransCode.TransCodeID
        txtDesc.Text = oTransCode.Description
        txtAmount.Text = oTransCode.Amount
        siTransType.Selected_ID = oTransCode.TransTypeID
        For i = 0 To ddAccount.Items.Count - 1
            If ddAccount.Items(i).Value = oTransCode.MerchantAccountID Then
                ddAccount.SelectedIndex = i
                Exit For
            End If
        Next
        ckActive.Checked = oTransCode.Active
    End Sub

    Private Sub Close()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        Close()
    End Sub
End Class
