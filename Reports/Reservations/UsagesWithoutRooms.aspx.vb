Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class Reports_Reservations_UsagesWithoutRooms
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then

            Dim ds = New clsComboItems().Load_ComboItems("ReservationType")
            Dim dt = CType(ds.Select(DataSourceSelectArguments.Empty), DataView).ToTable

            For Each dr As DataRow In dt.Rows
                lbL.Items.Add(dr("ComboItem").ToString())
            Next
        End If
    End Sub

    Protected Sub Report_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Report.Click

        If lbR.Items.Count = 0 Then Return
        Dim l = New List(Of String)

        For Each li As ListItem In lbR.Items
            l.Add(String.Format("'{0}'", li.Text))
        Next

        Get_List("select con.contractid, c.comboitem as Status, ut.comboitem as Type,con.contractnumber as KCP, u.UsageID, convert(varchar(10),InDate, 101) InDate, '' As Note from t_Usage  u left outer join  t_Roomallocationmatrix r on u.usageid = r.usageid left outer join t_Comboitems c on c.comboitemid = u.statusid left outer join t_Comboitems ut on ut.comboitemid = u.typeid left outer join t_Contract con on con.contractid = u.contractid where r.usageid is null " & " and ut.ComboItem in (" & String.Join(",", l.ToArray()) & ") and u.indate between '" & sdate.Selected_Date & "' and '" & edate.Selected_Date & "' and u.statusid in (select comboitemid from t_Comboitems c inner join t_combos d on c.comboid = d.comboid where d.comboname = 'usagestatus' and c.comboitem = 'used')")
    End Sub

    Private Sub Get_List(ByVal sSQL As String)
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim dr As SqlDataReader
        Dim ds As New SqlDataSource

        Try
            ds.connectionstring = Resources.Resource.cns
            ds.selectcommand = sSQL
            gvResults.datasource = ds
            Dim ka(0) As String
            ka(0) = "UsageID"
            gvResults.datakeynames = ka
            gvResults.databind()
        Catch ex As Exception
            lblErr.text = ex.Message
        Finally
            If cn.State <> Data.ConnectionState.Closed Then
                cn = Nothing
                cm = Nothing
                dr = Nothing
            End If

        End Try
    End Sub

    Protected Sub gvResults_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResults.SelectedIndexChanged
        Dim oUsage As New clsUsage

        oUsage.UsageID = gvResults.SelectedValue
        oUsage.Load()

        Response.Redirect("~/marketing/editContract.aspx?contractid=" & oUsage.ContractID & "&usageID=" & oUsage.UsageID)
        oUsage = Nothing
    End Sub

    Protected Sub btL_Click(sender As Object, e As System.EventArgs) Handles btL.Click
        Dim item = lbL.SelectedItem

        lbR.Items.Add(item)
        lbL.Items.Remove(item)

        lbL.ClearSelection()
        lbR.ClearSelection()
    End Sub

    Protected Sub btR_Click(sender As Object, e As System.EventArgs) Handles btR.Click
        Dim item = lbR.SelectedItem

        lbR.Items.Remove(item)
        lbL.Items.Add(item)

        lbL.ClearSelection()
        lbR.ClearSelection()
    End Sub

    Protected Sub btLAll_Click(sender As Object, e As System.EventArgs) Handles btLAll.Click

        If lbL.Items.Count = 0 Then Return
        For Each li As ListItem In lbL.Items
            lbR.Items.Add(li)
        Next

        lbL.Items.Clear()

    End Sub

    Protected Sub btRAll_Click(sender As Object, e As System.EventArgs) Handles btRAll.Click

        If lbR.Items.Count = 0 Then Return
        For Each li As ListItem In lbR.Items
            lbL.Items.Add(li)
        Next

        lbR.Items.Clear()
    End Sub

    Protected Sub gvResults_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResults.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(String.Format("select note from t_Note where keyfield='usageID' and keyvalue={0}", e.Row.Cells(5).Text), cn)
                    Try
                        cn.Open()
                        Dim dt = New DataTable
                        dt.Load(cm.ExecuteReader())
                        For Each dr As DataRow In dt.Rows
                            If e.Row.Cells(7).Text.Length = 0 Then
                                e.Row.Cells(7).Text += String.Format("{0}", dr(0).ToString())
                            Else
                                e.Row.Cells(7).Text += String.Format("<br/>{0}", dr(0).ToString())
                            End If
                        Next
                    Catch ex As Exception
                        cn.Close()
                    End Try
                End Using
            End Using
        End If
    End Sub
End Class
