
Partial Class general_CXLSchedPayments
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oSchedPayments As New clsPaymentsScheduled
            gvSchedPayments.DataSource = oSchedPayments.List_Scheduled_Payments(Request("KeyField"), Request("KeyValue"))
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvSchedPayments.DataKeyNames = sKeys
            gvSchedPayments.DataBind()
        End If
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If CheckSecurity("Payments", "CXLScheduledPayments", , , Session("UserDBID")) Then
            Dim oSchedPayment As New clsPaymentsScheduled
            For Each row As GridViewRow In gvSchedPayments.Rows
                Dim cb As CheckBox = row.FindControl("cbID")
                If cb.Checked Then
                    oSchedPayment.SchedPayID = row.Cells(1).text
                    oSchedPayment.Load()
                    oSchedPayment.Cancelled = True
                    'oSchedPayment.Description = txtDescription.Text
                    oSchedPayment.Save()
                End If
            Next
            oSchedPayment = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "refresh", "window.opener.__doPostBack('" & Request("linkid") & "','');", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "refresh", "alert('You Do Not Have Permission To Cancel Scheduled Payments.');", True)
        End If
    End Sub
End Class
