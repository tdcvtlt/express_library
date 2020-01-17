Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.Security
Imports System.Web
Imports System.Configuration
Imports System.Data
Imports System

Partial Class Reports_Accounting_DepositSummary
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/DepositSummary2.rpt"

    Private Sub BindData()
        Dim ds As New SqlDataSource
        Dim ds2 As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds2.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select * from t_CCMerchantAccount order by AccountName desc"
            ddMA.DataSource = ds
            ddMA.DataTextField = "Description"
            ddMA.DataValueField = "Accountname"
            ddMA.DataBind()
            'ds2.SelectCommand = "select 0 as id, '- ALL -' as item union select * from (Select fintransid as id, tc.comboitem as item from t_Fintranscodes f inner join t_Comboitems tc on tc.comboitemid = f.transcodeid and f.active = 1) a order by item"
            'ddTC.DataSource = ds2
            'ddTC.DataTextField = "Item"
            'ddTC.DataValueField = "ID"
            'ddTC.DataBind()
            siPM.ComboItem = "PaymentMethod"
            siPM.Connection_String = Resources.Resource.cns
            siPM.Load_Items()
            Update_TransCodes()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
        ds = Nothing
        ds2 = Nothing
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then

        Else
            Session("Report") = Nothing
            BindData()
            Setup_Report()
        End If

        If hfShowReport.Value = 1 Then CrystalReportViewer1.ReportSource = Session("Report")


    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            'Report.Close()
            'Report.Dispose()
        Catch ex As Exception

        End Try



    End Sub



    Private Sub Setup_Report()
        If Session("Report") Is Nothing Then
            Server.ScriptTimeout = 10000
            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            'Dim categoryID As Integer = Convert.ToInt32(ddlCategory.SelectedValue)


            ''Dim ds As New SqlDataSource
            ''ds.ConnectionString = Resources.Resource.cns
            'Dim cn As New SqlConnection(Resources.Resource.cns)
            'Dim cm As New SqlCommand("Exec sp_DailyDeposits '" & ddMA.SelectedValue & "','" & dfTransDate.Selected_Date & "','" & dfEndDate.Selected_Date & "'", cn)
            'cm.CommandTimeout = 120
            'Dim da As New SqlDataAdapter(cm)
            'Dim ds As New DataSet
            'da.Fill(ds,"Rpt")
            ''            Dim d As SqlDataReader = cm.ExecuteReader
            ''ds.SelectCommand = "Exec sp_DailyDeposits '" & ddMA.SelectedValue & "','" & dfTransDate.Selected_Date & "','" & dfEndDate.Selected_Date & "'"
            'Report.SetDataSource(ds)
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
            Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            Set_Params()
            Session.Add("Report", Report)
        Else
            Report = Session("Report")
            If Report.FileName <> Server.MapPath(sReport) Then
                Session("Report") = Nothing
                Setup_Report()
            End If
        End If
    End Sub

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRunReport.Click
        If dfTransDate.Selected_Date <> "" And lbSelectedTC.Items.Count > 0 And lbSelectedUser.Items.Count > 0 Then
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Set_Params()
        Dim tc As String = ""
        Dim user As String = ""
        For Each item As ListItem In lbSelectedTC.Items
            tc &= If(tc = "", item.Text, "," & item.Text)
        Next
        For Each item As ListItem In lbSelectedUser.Items
            user &= If(user = "", item.Text, "," & item.Text)
        Next
        Report.SetParameterValue("Date", CDate(IIf(dfTransDate.Selected_Date <> "", dfTransDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("EDate", CDate(IIf(dfenddate.Selected_Date <> "", dfenddate.Selected_Date, Date.Today)))
        Report.SetParameterValue("Acct", ddMA.SelectedValue)
        Report.SetParameterValue("TC", tc)
        Report.SetParameterValue("USR", user)
        Report.SetParameterValue("PM", siPM.SelectedName)
    End Sub

    Private Sub Update_TransCodes()
        Dim lst As New List(Of ListItem)
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet

        cm.CommandText = "select 0 as id, '- ALL -' as item union select * from (Select fintransid as id, tc.comboitem as item from t_Fintranscodes f inner join t_Comboitems tc on tc.comboitemid = f.transcodeid and f.active = 1 left outer join t_CCMerchantAccount m on m.AccountID = f.MerchantAccountID where m.AccountName='" & ddMA.SelectedValue & "') a order by item"
        da.Fill(ds, "TC")
        lbSelectedTC.Items.Clear()
        lbTC.Items.Clear()
        For Each dr As DataRow In ds.Tables("TC").Rows
            lst.Add(New ListItem With {.Value = dr("id"), .Text = dr("item")})
        Next
        lbTC.DataSource = lst
        lbTC.DataBind()
        lst.Clear()
        Dim tcList As String = ""
        For Each item As ListItem In lbTC.Items
            tcList &= If(tcList = "", item.Text, "','" & item.Text)
        Next
        cm.CommandText = "Select 0 as id, '- ALL -' as [User] union select * from (Select p.personnelid as id, p.username as [User] from t_Personnel p where p.Active = 1 and p.personnelid in (select distinct userid from v_Invoices where invoice in ('" & tcList & "'))) a order by [User]"
        lbSelectedUser.Items.Clear()
        lbUser.Items.Clear()
        da.Fill(ds, "User")
        For Each dr As DataRow In ds.Tables("User").Rows
            lst.Add(New ListItem With {.Value = dr("id"), .Text = dr("User")})
        Next
        lbUser.DataSource = lst
        lbUser.DataBind()
        ds = Nothing
        da = Nothing
        cm = Nothing
        cn = Nothing
    End Sub
    Protected Sub ddMA_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddMA.SelectedIndexChanged
        Update_TransCodes()
    End Sub

    Protected Sub addTC_Click(sender As Object, e As EventArgs) Handles addTC.Click
        If Not (IsNothing(lbTC.SelectedItem)) Then
            lbSelectedTC.Items.Add(lbTC.SelectedItem)
            lbTC.Items.Remove(lbTC.SelectedItem)
            SortList(lbSelectedTC)
        End If
    End Sub

    Protected Sub remTC_Click(sender As Object, e As EventArgs) Handles remTC.Click
        If Not (IsNothing(lbSelectedTC.SelectedItem)) Then
            lbTC.Items.Add(lbSelectedTC.SelectedItem)
            lbSelectedTC.Items.Remove(lbSelectedTC.SelectedItem)
            SortList(lbTC)
        End If
    End Sub

    Protected Sub addUser_Click(sender As Object, e As EventArgs) Handles addUser.Click
        If Not (IsNothing(lbUser.SelectedItem)) Then
            lbSelectedUser.Items.Add(lbUser.SelectedItem)
            lbUser.Items.Remove(lbUser.SelectedItem)
            SortList(lbSelectedUser)
        End If
    End Sub

    Protected Sub remUser_Click(sender As Object, e As EventArgs) Handles remUser.Click
        If Not IsNothing(lbSelectedUser.SelectedItem) Then
            lbUser.Items.Add(lbSelectedUser.SelectedItem)
            lbSelectedUser.Items.Remove(lbSelectedUser.SelectedItem)
            SortList(lbUser)
        End If
    End Sub

    Private Sub SortList(ByRef lb As ListBox)
        If lb.Items.Count > 1 Then
            Dim items As New List(Of ListItem)
            For Each item In lb.Items
                items.Add(item)
            Next
            lb.Items.Clear()
            items.Sort(Function(x As ListItem, y As ListItem)
                                           Return x.Text.CompareTo(y.Text)
                                       End Function)
            lb.DataSource = items
            lb.DataBind()
        End If
    End Sub
End Class
