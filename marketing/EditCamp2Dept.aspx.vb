
Partial Class marketing_EditCamp2Dept
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oCamp2Dept As New clsCampaign2Department
            oCamp2Dept.ID = Request("ID")
            oCamp2Dept.Load()
            siDept.Connection_String = Resources.Resource.cns
            siDept.Label_Caption = ""
            siDept.ComboItem = "Department"
            siDept.Selected_ID = oCamp2Dept.DepartmentID
            siDept.Load_Items()
            siLoc.Connection_String = Resources.Resource.cns
            siLoc.Label_Caption = ""
            siLoc.ComboItem = "TourLocation"
            siLoc.Selected_ID = oCamp2Dept.TourLocationID
            siLoc.Load_Items()

            siDept.Selected_ID = oCamp2Dept.DepartmentID
            siLoc.Selected_ID = oCamp2Dept.TourLocationID
            oCamp2Dept = Nothing
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim oCamp2Dept As New clsCampaign2Department
        oCamp2Dept.ID = Request("ID")
        oCamp2Dept.Load()
        oCamp2Dept.UserID = Session("UserDBID")
        If Request("ID") = 0 Then
            oCamp2Dept.CampaignID = Request("CampaignID")
        End If
        oCamp2Dept.TourLocationID = siLoc.Selected_ID
        oCamp2Dept.DepartmentID = siDept.Selected_ID
        oCamp2Dept.Save()
        oCamp2Dept = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Departments();window.close();", True)
    End Sub
End Class
