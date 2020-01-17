Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class Reports_Accounting_Events
    Inherits System.Web.UI.Page

    Private reportPath As String = "REPORTFILES/EVENTS.RPT"
    Private report As New ReportDocument
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            If IsPostBack = False Then
                Dim ds As New SqlDataSource(Resources.Resource.cns, "Select Distinct(b.PersonnelID) as ID, b.FirstName + ' ' + b.LastName as Name from t_Personnel b left outer join t_Personnel2Dept a on a.personnelid = b.personnelid where (a.DepartmentID in (Select Departmentid from t_Personnel2Dept where personnelid = '" & Session("UserDBID") & "' and isManager = '1' and active = '1') or b.PersonnelID in (Select Distinct(pp.PersonnelID) from t_Personnel pp inner join t_Vendor2Personnel vp on vp.PersonnelID = pp.PersonnelID where pp.Active = 1 and vendorid in (Select Distinct(VendorID) from t_Vendor2Personnel where PersonnelID = " & Session("UserDBID") & " and Manager = 1))) order by b.firstname + ' ' + b.LastName asc")
                lbPersonnel2.DataSource = ds ' (New clsPersonnel).List_Active_Flag
                lbPersonnel2.DataValueField = "ID"
                lbPersonnel2.DataTextField = "Name"
                lbPersonnel2.DataBind()
                ds = Nothing
                'siTourLocation.Connection_String = Resources.Resource.cns
                'siTourLocation.ComboItem = "TourLocation"
                'siTourLocation.Label_Caption = ""
                'siTourLocation.Load_Items()
            End If
        End If

        If Not Session("Crystal") Is Nothing Then
            If Not Session("UserID") Is Nothing And Me.IsPostBack = True Then
                CrystalViewer.ReportSource = Session("Crystal")
            Else
                If Session("Crystal") Is Nothing Then
                    Response.Redirect("http://crms.kingscreekplantation.com/crmsnet/logon.aspx")
                End If

            End If
        End If
    End Sub

    Protected Sub RunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunReport.Click
        If (Not Session("Crystal") Is Nothing) Then
            Session("Crystal") = Nothing
            CrystalViewer.Visible = False
        End If

        If (String.IsNullOrEmpty(SDATE.Selected_Date) Or String.IsNullOrEmpty(EDATE.Selected_Date)) Then Return
        Dim IDs As String = ""
        For i = 0 To lbSelected2.Items.Count - 1
            IDs &= IIf(i = 0, lbSelected2.Items(i).Value, "," & lbSelected2.Items(i).Value)
        Next
        report.FileName = Server.MapPath(reportPath)
        report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        report.SetParameterValue("SDate", SDATE.Selected_Date.Trim())
        report.SetParameterValue("EDATE", EDATE.Selected_Date.Trim())
        report.SetParameterValue("PersonnelID", IDs)
        'report.SetParameterValue("tourLocationID", siTourLocation.Selected_ID)

        report.Load(Server.MapPath(reportPath))

        Session("Crystal") = report
        CrystalViewer.ReportSource = Session("Crystal")

        CrystalViewer.Visible = True
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not (lbPersonnel2.SelectedItem Is Nothing) Then
            lbSelected2.Items.Add(lbPersonnel2.SelectedItem)
            lbPersonnel2.Items.Remove(lbPersonnel2.SelectedItem)
            lbPersonnel2.SelectedIndex = -1
            lbSelected2.SelectedIndex = -1
        End If
    End Sub

    Protected Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If Not (lbSelected2.SelectedItem Is Nothing) Then
            lbPersonnel2.Items.Add(lbSelected2.SelectedItem)
            lbSelected2.Items.Remove(lbSelected2.SelectedItem)
            lbSelected2.SelectedIndex = -1
            lbPersonnel2.SelectedIndex = -1
        End If
    End Sub
End Class
