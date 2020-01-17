
Partial Class security_EditPersDept
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If Request("PersDeptID") <> 0 Then
                Dim oPers2Dept As New clsPersonnel2Dept
                Dim oCombo As New clsComboItems
                oPers2Dept.Personnel2Dept = Request("PersDeptID")
                oPers2Dept.Load()
                txtDept.Text = oCombo.Lookup_ComboItem(oPers2Dept.DepartmentID)
                txtCompany.Text = oCombo.Lookup_ComboItem(oPers2Dept.CompanyID)
                cbMgr.Checked = oPers2Dept.isManager
                cbActive.Checked = oPers2Dept.Active
                cbClockIn.Checked = oPers2Dept.clockIn
                oPers2Dept = Nothing
            Else
                siDept.Connection_String = Resources.Resource.cns
                siDept.ComboItem = "Department"
                siDept.Label_Caption = ""
                siDept.Load_Items()

                siCompany.Connection_String = Resources.Resource.cns
                siCompany.ComboItem = "PayrollCompany"
                siCompany.Label_Caption = ""
                siCompany.Load_Items()
            End If
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oPers2Dept As New clsPersonnel2Dept
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        If Request("PersDeptID") = 0 Then
            If siDept.Selected_ID < 1 Or siCompany.Selected_ID < 1 Then
                sErr = "Please Fill In All Fields."
                bProceed = False
            Else
                oPers2Dept.Personnel2Dept = 0
                oPers2Dept.Load()
                oPers2Dept.UserID = Session("UserDBID")
                oPers2Dept.PersonnelID = Request("PersonnelID")
                oPers2Dept.DepartmentID = siDept.Selected_ID
                oPers2Dept.CompanyID = siCompany.Selected_ID
                oPers2Dept.Active = cbActive.Checked
                oPers2Dept.isManager = cbMgr.Checked
                oPers2Dept.clockIn = cbClockIn.Checked
                oPers2Dept.Save()
            End If
        Else
            oPers2Dept.Personnel2Dept = Request("PersDeptID")
            oPers2Dept.Load()
            oPers2Dept.UserID = Session("UserDBID")
            oPers2Dept.Active = cbActive.Checked
            oPers2Dept.isManager = cbMgr.Checked
            oPers2Dept.clockIn = cbClockIn.Checked
            oPers2Dept.Save()
        End If
        If bProceed Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Dept();window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub
End Class
