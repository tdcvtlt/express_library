
Partial Class controls_LeadManagement_GlobalCalendar
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Load_Calendar()
        End If
    End Sub

    Private Sub Load_Calendar()
        Dim ds As New System.Data.DataSet
        Dim dt As New System.Data.DataTable
        dt.Columns.Add("Date")
        dt.Columns.Add("Event Type")
        dt.Columns.Add("Assigned To")
        dt.Columns.Add("Event")
        dt.Columns.Add("")
        ds.Tables.Add(dt)
        GridView1.DataSource = ds
        GridView1.DataBind()
        dt = Nothing
        ds = Nothing
    End Sub

    Protected Sub lbRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRefresh.Click
        Load_Calendar()
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        Load_Calendar2()
    End Sub

    Private Sub Load_Calendar2()
        Dim ds As New System.Data.DataSet
        Dim dt As New System.Data.DataTable
        dt.Columns.Add("Date")
        dt.Columns.Add("Event Type")
        dt.Columns.Add("Assigned To")
        dt.Columns.Add("Event2")
        dt.Columns.Add("")
        ds.Tables.Add(dt)
        GridView1.EmptyDataText = "No Events (Range: " & DropDownList1.SelectedValue & ")"
        GridView1.DataSource = ds
        GridView1.DataBind()
        dt = Nothing
        ds = Nothing
    End Sub
End Class
