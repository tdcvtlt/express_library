Imports Microsoft.VisualBasic
Imports System.Data

Partial Class general_PAPage
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If CheckSecurity("CreditCards", "VRCPreAuth", "", "", Session("UserDBID")) Then
                Dim acct As String = "'~0003~'"
                Dim oFinTrans As New clsFinancialTransactionCodes
                ddMerchantAccount.DataSource = oFinTrans.List_Trans_BY_Acct(acct)
                ddMerchantAccount.DataTextField = "TransCode"
                ddMerchantAccount.DataValueField = "AcctID"
                ddMerchantAccount.DataBind()
                ddMerchantAccount.AppendDataBoundItems = True
                ddMerchantAccount.DataSource = oFinTrans.List_Trans_BY_Acct("'~0009~'")
                ddMerchantAccount.DataTextField = "TransCode"
                ddMerchantAccount.DataValueField = "AcctID"
                ddMerchantAccount.DataBind()

            Else
                btnSave.Enabled = False
                lblWaiting.Text = "Access Denied"
            End If
                Dim sds As SqlDataSource = (New clsCCMerchantAccount).List_Accounts()
                Dim dv As DataView = CType(sds.Select(DataSourceSelectArguments.Empty), DataView)
                Dim code As String = "function Get_Key(index){switch (index){ "
                For Each dvr As DataRowView In dv
                    code &= "case """ & dvr("AccountID") & """: pkey='" & dvr("publictoken") & "'; break; "
                Next
                code &= "default: pkey=''; break; };};"
                sds = Nothing
                dv = Nothing
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "pk", code, True)
        End If
    End Sub
    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim oFin As New clsFinancials
        Dim oCC As New clsCreditCard
        Dim oCCTrans As New clsCCTrans
        Dim oCombo As New clsComboItems
        Dim ccID As Integer = 0
        ccID = oFin.Get_CreditCard(txtCCNumber.Text, txtExpiration.Text, txtCVV.Text, "", "", 0, txtPostalCode.Text, txtName.Text, "", 0, hfTokenValue.Value, hfCardType.value)
        If ccID = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Uhoh", "alert('" & oFin.Err & "');", True)
        Else
            oCCTrans.CCTransID = 0
            oCCTrans.Load()
            oCCTrans.CreditCardID = ccID
            oCCTrans.TransTypeID = oCombo.Lookup_ID("CCTransType", "PreAuth")
            oCCTrans.Imported = 0
            oCCTrans.Approved = 1
            oCCTrans.ApprovedBy = "AutoApprove"
            oCCTrans.DateApproved = System.DateTime.Now
            oCCTrans.ClientIP = Request.ServerVariables("REMOTE_HOST")
            oCCTrans.CreatedByID = Session("UserDBID")
            oCCTrans.DateCreated = System.DateTime.Now
            oCCTrans.Amount = CDbl(txtAmount.Text)
            oCCTrans.AccountID = ddMerchantAccount.SelectedValue
            oCCTrans.Token = hfTokenValue.Value
            oCCTrans.Save()

            hfCCTransID.Value = oCCTrans.CCTransID
            oCCTrans = Nothing
            oCC.CreditCardID = ccID
            oCC.Load()
            oCC.ReadyToImport = True
            oCC.Save()

            tmrCheck.Interval = 100
            tmrCheck.Enabled = True
            btnSave.Visible = False
        End If
        oCC = Nothing
        oCCTrans = Nothing
        oCombo = Nothing
        oFin = Nothing
    End Sub

    Protected Sub tmrCheck_Tick(sender As Object, e As System.EventArgs) Handles tmrCheck.Tick
        tmrCheck.Interval = 2000
        lblWaiting.Text = "Processing ... Please Wait <br />"
        lblWaiting.Text += hfTickCounter.Value + 1 & " Seconds " & System.DateTime.Now & " " & hfTickCounter.Value & " " & hfCCTransID.Value
        hfTickCounter.Value += 1
        If Not (Check_Status()) Or hfTickCounter.Value >= 50 Then
            tmrCheck.Enabled = False
            If hfTickCounter.Value >= 10 And txtResponse.Text = "" Then txtResponse.Text = "Timer Expired Contact IT"
            btnSave.Visible = True
            lblWaiting.Text = "Processed"
            hfTickCounter.Value = 0
        End If
    End Sub

    Private Function Check_Status() As Boolean
        Dim oCCTrans As New clsCCTrans
        oCCTrans.CCTransID = hfCCTransID.Value
        oCCTrans.Load()
        If oCCTrans.ICVResponse & "" <> "" Then
            If Left(oCCTrans.ICVResponse, 1) = "N" Then
                txtResponse.Text = oCCTrans.ICVResponse
            Else
                txtResponse.Text = Right(Left(oCCTrans.ICVResponse, 7), 6)
            End If
            Return False
        Else
            Return True
        End If
        oCCTrans = Nothing
    End Function
End Class
