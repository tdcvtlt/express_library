
Partial Class general_receivePayment
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            'Dim oCCtrans As New clsCCTrans
            'oCCtrans.CCTransID = Request("ID")
            'oCCtrans.Load()
            'If oCCtrans.Approved <> 0 Then
            'lblWaiting.Text = "Charge has already been processed."
            'Else
            'oCCtrans.Approved = 1
            'oCCtrans.ApprovedBy = Session("UserDBID")
            'oCCtrans.DateApproved = System.DateTime.Now
            'oCCtrans.Save()
            hfCCTransID.Value = Request("ID")
            tmrCheck.Interval = 100
            tmrCheck.Enabled = True
            'End If
            'oCCtrans = Nothing
        End If
    End Sub

    Protected Sub tmrCheck_Tick(sender As Object, e As System.EventArgs) Handles tmrCheck.Tick
        tmrCheck.Interval = 2000
        lblWaiting.Text = "Processing ... Please Wait <br />"
        lblWaiting.Text += hfTickCounter.Value + 1 & " Seconds " & System.DateTime.Now & " " & hfTickCounter.Value & " " & hfCCTransID.Value
        hfTickCounter.Value += 1
        If Not (Check_Status()) Or hfTickCounter.Value >= 50 Then
            tmrCheck.Enabled = False
            If hfTickCounter.Value >= 10 And lblResponse.Text = "" Then lblResponse.Text = "Timer Expired Contact IT"
            lblWaiting.Text = "Processed"
            hfTickCounter.Value = 0
            If Left(lblResponse.Text, 1) = "N" Then
                Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType, "AjaxCall", "alert('Card Declined.');", True)
            ElseIf lblResponse.Text <> "Timer Expired Contract IT" Then
                Dim oCCtrans As New clsCCTransApplyTo
                Dim IDs As String = oCCtrans.List_Payments(hfCCTransID.Value)
                Response.Redirect("Receipt.aspx?ID=" & IDs)
                oCCtrans = Nothing
            End If
        End If
    End Sub

    Private Function Check_Status() As Boolean
        Dim oCCTrans As New clsCCTrans
        oCCTrans.CCTransID = hfCCTransID.Value
        oCCTrans.Load()
        If oCCTrans.ICVResponse & "" <> "" Then
            If Left(oCCTrans.ICVResponse, 1) = "N" Then
                lblResponse.Text = oCCTrans.ICVResponse
            Else
                lblResponse.Text = Right(Left(oCCTrans.ICVResponse, 7), 6)
            End If
            Return False
        Else
            Return True
        End If
        oCCTrans = Nothing
    End Function

End Class
