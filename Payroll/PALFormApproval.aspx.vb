
Partial Class Payroll_PALFormApproval
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oPers2Dept As New clsPersonnel2Dept
            Dim oPalRequest As New clsPALRequest
            If oPers2Dept.Member_Of(Session("UserDBID"), "HR") Then

                gvPendingRequests.DataSource = oPalRequest.Get_PAL_Requests()
                gvPendingRequests.DataBind()

                MultiView1.ActiveViewIndex = 0
            Else
                gvPendingRequests.DataSource = oPalRequest.Get_Dept_PAL_Requests(Session("UserDBID"))
                gvPendingRequests.DataBind()
                MultiView1.ActiveViewIndex = 0
            End If
            oPers2Dept = Nothing
            oPalRequest = Nothing
        End If
    End Sub

    Protected Sub lbDetails_Click(sender As Object, e As System.EventArgs)
        Dim row As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Payroll/PALFormDetails.aspx?PALRequestID=" & row.Cells(0).Text & "','win01',450,450);", True)
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
            Dim oPALRequest As New clsPALRequest
            For Each row As GridViewRow In gvPendingRequests.Rows
                Dim cbApprove As CheckBox = row.FindControl("cbApprove")
                Dim cbDeny As CheckBox = row.FindControl("cbDeny")
                If cbApprove.Checked Then
                    oPalRequest.PALRequestID = row.Cells(0).Text
                    oPalRequest.Load()
                    oPalRequest.ManagerApproved = 1
                    oPalRequest.ManagerID = Session("UserDBID")
                    oPalRequest.DateApproved = System.DateTime.Now
                    oPalRequest.Save()
                ElseIf cbDeny.Checked Then
                    oPALRequest.PALRequestID = row.Cells(0).Text
                    oPALRequest.Load()
                    oPALRequest.ManagerApproved = -1
                    oPALRequest.ManagerID = Session("UserDBID")
                    oPALRequest.DateApproved = System.DateTime.Now
                    oPALRequest.Save()
                End If
            Next
            Dim oPers2Dept As New clsPersonnel2Dept
            If oPers2Dept.Member_Of(Session("UserDBID"), "HR") Then
                gvPendingRequests.DataSource = oPALRequest.Get_PAL_Requests()
                gvPendingRequests.DataBind()
            Else
                gvPendingRequests.DataSource = oPALRequest.Get_Dept_PAL_Requests(Session("UserDBID"))
                gvPendingRequests.DataBind()
            End If
            oPers2Dept = Nothing
            oPALRequest = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub
End Class
