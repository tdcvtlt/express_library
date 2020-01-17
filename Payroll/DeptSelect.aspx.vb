
Partial Class Payroll_DeptSelect
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oPers2Dept As New clsPersonnel2Dept
            cbDepts.DataSource = oPers2Dept.Get_Depts(Request("PersonnelID"))
            cbDepts.DataTextField = "Department"
            cbDepts.DataValueField = "DepartmentID"
            cbDepts.DataBind()
            oPers2Dept = Nothing
        End If
    End Sub

    Protected Sub cbDepts_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cbDepts.SelectedIndexChanged
        'ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert(" & cbDepts.SelectedItem.Value & ");", True)
        If Request("ClockType") = "ClockIn" Then
            'ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert(" & cbDepts.SelectedItem.Value & ");", True)
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Punch_In('" & cbDepts.SelectedItem.Value & "');window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Punch_Out('" & cbDepts.SelectedItem.Value & "');window.close();", True)
        End If
    End Sub
End Class
