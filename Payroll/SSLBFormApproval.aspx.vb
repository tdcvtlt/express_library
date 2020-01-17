
Partial Class Payroll_SSLBFormApproval
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oPers2Dept As New clsPersonnel2Dept
            Dim oSSLBRequest As New clsSSLBRequest
            If oPers2Dept.Member_Of(Session("UserDBID"), "HR") Then

                gvPendingRequests.DataSource = oSSLBRequest.Get_SSLB_Requests()
                gvPendingRequests.DataBind()

                MultiView1.ActiveViewIndex = 0
            Else
                btnProcessPending.Visible = False
            End If
            oPers2Dept = Nothing
            oSSLBRequest = Nothing
        End If
    End Sub

    Protected Sub lbDetails_Click(sender As Object, e As System.EventArgs)
        Dim row As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Payroll/SSLBFormDetails.aspx?SSLBRequestID=" & row.Cells(0).Text & "','win01',450,450);", True)
    End Sub


    Protected Sub btnProcessPending_Click(sender As Object, e As System.EventArgs) Handles btnProcessPending.Click
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        Dim oPers As New clsPersonnel
        oPers.PersonnelID = Session("UserDBID")
        oPers.Load()
        For Each row As GridViewRow In gvPendingRequests.Rows
            Dim cbApprove As CheckBox = row.FindControl("cbApprove")
            Dim cbDeny As CheckBox = row.FindControl("cbDeny")
            If cbApprove.Checked And cbDeny.Checked Then
                bProceed = False
                sErr = "You Can Only Select Approve OR Deny. Not Both."
            End If
            If cbApprove.Checked Or cbDeny.Checked Then
                If row.Cells(1).Text = oPers.LastName & ", " & oPers.FirstName Then
                    sErr = "You Can Not Approve/Deny Your Own PAL Request Forms"
                    bProceed = False
                End If
            End If
        Next
        oPers = Nothing
        If bProceed Then
            Dim oSSLBRequest As New clsSSLBRequest
            oSSLBRequest.UserID = Session("UserDBID")
            For Each row As GridViewRow In gvPendingRequests.Rows
                Dim cbApprove As CheckBox = row.FindControl("cbApprove")
                Dim cbDeny As CheckBox = row.FindControl("cbDeny")
                If cbApprove.Checked Then
                    oSSLBRequest.SSLBRequestID = row.Cells(0).Text
                    oSSLBRequest.Load()
                    oSSLBRequest.HRApproved = 1
                    oSSLBRequest.HRID = Session("UserDBID")
                    oSSLBRequest.DateApproved = System.DateTime.Now
                    oSSLBRequest.Save()
                ElseIf cbDeny.Checked Then
                    oSSLBRequest.SSLBRequestID = row.Cells(0).Text
                    oSSLBRequest.Load()
                    oSSLBRequest.HRApproved = -1
                    oSSLBRequest.HRID = Session("UserDBID")
                    oSSLBRequest.DateApproved = System.DateTime.Now
                    oSSLBRequest.Save()
                End If
            Next
            Dim oPers2Dept As New clsPersonnel2Dept
            If oPers2Dept.Member_Of(Session("UserDBID"), "HR") Then
                gvPendingRequests.DataSource = oSSLBRequest.Get_SSLB_Requests()
                gvPendingRequests.DataBind()
            End If
            oPers2Dept = Nothing
            oSSLBRequest = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub
End Class
