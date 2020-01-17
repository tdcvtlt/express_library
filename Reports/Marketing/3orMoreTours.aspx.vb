Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Partial Class Reports_Marketing_3orMoreTours
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/3orMoreTours.rpt"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Session("Report") = Nothing
        Setup_Report()
        CrystalReportViewer1.ReportSource = Session("Report")
    End Sub

    Private Sub Setup_Report()
        If Session("Report") Is Nothing Then
            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            'Dim categoryID As Integer = Convert.ToInt32(ddlCategory.SelectedValue)
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
            Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            Session.Add("Report", Report)
        Else
            Report = Session("Report")
            If Report.FileName <> Server.MapPath(sReport) Then
                Session("Report") = Nothing
                Setup_Report()
            End If
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim sTable As String = ""
        sTable = "<table><th>FirstName</th><th>LastName</th><th>Address</th><th>City</th><th>State</th><th>PostalCode</th><th>Phone(s)</th></tr>"
        Dim cn As New SqlConnection(Resources.Resource.cns)
        If cn.State <> ConnectionState.Open Then cn.Open()
        Dim cm As New SqlCommand("Select y.ProspectID, y.FirstName, y.LastName, pa.Address1, pa.City, pas.ComboItem as State, pa.PostalCode, pp.Number from (Select p.ProspectID, p.FirstName, p.LastName, (Select top 1 AddressID from t_ProspectAddress where prospectid = x.ProspectID) as AddressID, (Select top 1 phoneid from t_prospectPhone where prospectid = x.prospectid and active = 1) as PhoneID  from (Select Distinct(ProspectID), Count(*) as Tours from t_Tour t left outer join t_ComboItems tl on t.TourLocationID = tl.ComboItemID left outer join t_ComboItems ts on t.StatusID = ts.ComboItemID left outer join t_ComboItems tst on t.SubTypeID = tst.CombOItemID where ts.ComboItem in ('Showed', 'OnTour','NQ-Toured') and tst.ComboItem <> 'Exit' and tl.ComboItem = 'KCP' and prospectid not in (Select prospectid from t_Contract) group by t.ProspectID) x inner join t_Prospect p on x.ProspectID = p.ProspectID where tours > 2) y left outer join t_ProspectAddress pa on y.AddressID = pa.AddressID left outer join t_comboitems pas on pa.StateID = pas.ComboitemID left outer join t_prospectphone pp on y.PhoneID = pp.PhoneID", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        Dim dr As SqlDataReader
        da.Fill(ds, "Prospects")
        If ds.Tables("Prospects").Rows.Count > 0 Then
            For i = 0 To ds.Tables("Prospects").Rows.Count - 1
                sTable &= "<tr>"
                sTable &= "<td>" & ds.Tables("Prospects").Rows(i).Item("FirstName") & "</td>"
                sTable &= "<td>" & ds.Tables("Prospects").Rows(i).Item("LastName") & "</td>"
                sTable &= "<td>" & ds.Tables("Prospects").Rows(i).Item("Address1") & "</td>"
                sTable &= "<td>" & ds.Tables("Prospects").Rows(i).Item("City") & "</td>"
                sTable &= "<td>" & ds.Tables("Prospects").Rows(i).Item("State") & "</td>"
                sTable &= "<td>" & ds.Tables("Prospects").Rows(i).Item("PostalCode") & "</td>"
                sTable &= "<td>"
                cm.CommandText = "Select pp.Number +  Case when (pt.ComboItem) is null then '' else ' - ' + pt.ComboItem end as PhoneNumber from t_ProspectPhone pp left outer join t_ComboItems pt on pp.TypeID = pt.ComboitemID where pp.ProspectID = " & ds.Tables("Prospects").Rows(i).Item("ProspectID") & " and pp.Active = 1 and pp.number is not null"
                dr = cm.ExecuteReader
                If dr.HasRows Then
                    Do While dr.Read()
                        sTable &= dr("PhoneNumber") & "<br>"
                    Loop
                End If
                dr.Close()
                sTable &= "</td>"
            Next
        End If
        da.Dispose()
        If cn.State <> ConnectionState.Closed Then cn.Close()
        da = Nothing
        cm = Nothing
        cn = Nothing
        sTable &= "</table>"
        Literal1.Text = sTable
    End Sub


End Class
