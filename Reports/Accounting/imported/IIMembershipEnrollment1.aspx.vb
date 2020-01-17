Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data.Linq.Mapping
Imports System.Data.Linq


Partial Class Reports_Accounting_IIMembershipEnrollment
    Inherits System.Web.UI.Page

    Private DEFAULT_II_CONTRACT_STATUSES As String() = {"16585", "16571", "16582", "17277"}
    Private STATE_DIC As IDictionary(Of Int32, String)
    Private COUNTRY_DIC As IDictionary(Of Int32, String)
    Private COLUMN_MAX As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then

            checkBoxList1.DataSource = get_Default_Contract_Statuses()
            checkBoxList1.DataTextField = "ComboItem"
            checkBoxList1.DataValueField = "ComboItemID"
            checkBoxList1.DataBind()

            Insert_New_PointsContracts()

            multi_view_main.SetActiveView(View1)
        Else
           
        End If
    End Sub



#Region ""
    ''' <summary>
    ''' Import points contracts that never sent to II to table
    ''' </summary>
    Private Sub Insert_New_PointsContracts()

        Dim sq = String.Format( _
        "insert into t_iimembershipenrollment(contractid) " & _
        "select contractid " & _
        "from t_contract c " & _
        "inner join t_comboitems cst on c.subtypeid = cst.comboitemid " & _
        "inner join t_comboitems cs on c.statusid = cs.comboitemid " & _
        "where cst.comboitem in ('POINTS') " & _
        "and cs.comboitem not in ('PENDER', 'KICK', 'PENDER-INV', 'PENDING REDEED', 'RES-PENDER', 'RESCINDED', 'VOID') " & _
        "and c.contractid not in " & _
        "( select contractid from t_IIMembershipEnrollment) ")

        Using con = New SqlConnection(Resources.Resource.cns)
            Using cmd = New SqlCommand(sq, con)
                con.Open()
                Dim recordsAff = cmd.ExecuteNonQuery()
            End Using
            con.Close()
        End Using
    End Sub

    Private Function get_Default_Contract_Statuses() As DataTable
        Dim sql = String.Format("select * from t_combos ct inner join t_comboitems " & _
                                                  "ci on ct.comboid = ci.comboid where " & _
                                                  "ct.comboname = 'contractstatus' and ci.comboItem not in ({0}) order by ComboItem", _
                                                  String.Join(",", (New String() {"'Suspense'", "'Active'", "'Redeed'", "'Developer', 'PENDER', 'KICK', 'PENDER-INV','PENDING REDEED','RES-PENDER','RESCINDED','VOID'"})))
        Dim dt = New DataTable()
        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using ada As New SqlDataAdapter(sql, cnn)
                ada.Fill(dt)
            End Using
        End Using
        Return dt
    End Function
    Private Function get_CXL_Contract_Statuses() As DataTable
        Dim dt = New DataTable()

        Using con As New SqlConnection(Resources.Resource.cns)
            Dim sql = String.Format( _
                "select * from t_combos c inner join t_comboitems ci on c.comboId = ci.comboId " & _
                "where c.comboname = 'contractstatus' and (comboItem like 'cxl%' or comboitem = 'canceled') order by comboItem")

            Using ada As New SqlDataAdapter(sql, con)
                ada.Fill(dt)
            End Using
            Return dt
        End Using
    End Function
    ''' <summary>
    ''' Get points contracts new owners just purchased, not upgrades or additionals
    ''' </summary> 
    Private Sub get_points_contracts()

        Dim q As Queue(Of String) = New Queue(Of String)
        For Each li As ListItem In checkBoxList1.Items
            If li.Selected Then
                q.Enqueue(li.Value.ToString())
            End If
        Next

        Dim sq = String.Format( _
                            "select p.prospectid, ii.dateADDED, p.FirstName, p.LastName, c.ContractNumber, p.AnniversaryDate, " & _
                            "(select comboitem from t_ComboItems where comboitemid = c.statusid) Status, c.ContractID " & _
                            "from t_IIMembershipEnrollment ii inner join t_contract c on ii.contractid = c.contractid " & _
                            "inner join t_prospect p on p.prospectid = c.prospectid " & _
                            "where c.contractid in " & _
                            "( " & _
                            "select ii.contractid from t_IIMembershipEnrollment ii inner join t_contract c " & _
                            "on ii.contractid = c.contractid where statusid in  ({0}) " & _
                            ") " & _
                            "order by p.prospectID", String.Join(",", q.ToArray().Concat(DEFAULT_II_CONTRACT_STATUSES).ToArray()))

        Using ada As New SqlDataAdapter(sq, New SqlConnection(Resources.Resource.cns))
            Dim dt = New DataTable()
            ada.Fill(dt)

            Dim members = New DataTable()

            Dim col_contract_id = members.Columns.Add("ContractID", GetType(String))
            members.Columns.Add("ProspectID", GetType(String))
            members.Columns.Add("AnniversaryDate", GetType(String))
            members.Columns.Add("FirstName", GetType(String))
            members.Columns.Add("LastName", GetType(String))
            members.Columns.Add("KCP", GetType(String))
            members.Columns.Add("Status", GetType(String))

            members.PrimaryKey = New DataColumn() {col_contract_id}

            For Each g As IGrouping(Of String, DataRow) In dt.AsEnumerable().GroupBy(Function(x) x.Item("PROSPECTID").ToString())

                Dim shd_inc = True

                For Each r As DataRow In g
                    If r.Item("DateAdded").Equals(DBNull.Value) = False Then
                        shd_inc = False
                    End If
                Next

                If shd_inc Then
                    For Each dr As DataRow In g

                        Dim datarow = members.NewRow()

                        datarow.Item("ContractID") = dr.Item("ContractID")
                        datarow.Item("ProspectID") = dr.Item("ProspectID")
                        Try
                            datarow.Item("AnniversaryDate") = DateTime.Parse(dr("AnniversaryDate").ToString()).ToString("MM-dd-yyyy")
                        Catch ex As FormatException
                            datarow.Item("AnniversaryDate") = "N/A"
                        End Try

                        datarow.Item("FirstName") = dr.Item("FirstName")
                        datarow.Item("LastName") = dr.Item("LastName")
                        datarow.Item("KCP") = dr.Item("ContractNumber")
                        datarow.Item("Status") = dr.Item("Status")

                        members.Rows.Add(datarow)
                    Next
                End If
            Next

            gvExport.DataSource = members
            gvExport.DataBind()

            Submit.Visible = IIf(members.Rows.Count > 0, True, False)
            btn_ToExcel.Visible = Submit.Visible

            If Submit.Visible Then
                view1_h5.InnerHtml = "Make selection(s) then click Submit to forward them to II report. Just want to view in Excel, click Export."
            Else
                view1_h5.InnerHtml = ""
            End If

        End Using
    End Sub
#End Region
#Region "EVENT HANDLERS"

    Protected Sub lnk_btn_points_owners_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_btn_points_owners.Click
        lblError.Text = ""
        multi_view_main.ActiveViewIndex = 0
    End Sub

    Protected Sub lnk_btn_ii_report_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_btn_ii_report.Click
        lblError.Text = ""
        multi_view_main.ActiveViewIndex = 1
        litResult.Text = String.Empty

        hf_iiMembershipEnrollmentID.Value = String.Empty
        view2_h5.InnerHtml = String.Empty
        CheckToExport.Visible = False

    End Sub

    Protected Sub lnk_btn_non_ii_report_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_btn_non_ii_report.Click

        multi_view_main.SetActiveView(View3)

        cbl_cxl.DataSource = get_CXL_Contract_Statuses()
        cbl_cxl.DataTextField = "ComboItem"
        cbl_cxl.DataValueField = "ComboItemID"
        cbl_cxl.DataBind()

    End Sub

    Protected Sub lnk_btn_report_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_btn_report.Click
        multi_view_main.ActiveViewIndex = 3
    End Sub

    Protected Sub CheckToExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckToExport.Click

        If hf_iiMembershipEnrollmentID.Value.Length > 0 Then

            Dim ar() = hf_iiMembershipEnrollmentID.Value.Trim().Split(New Char() {" "c})

            Using cnn = New SqlConnection(Resources.Resource.cns)
                Using ada = New SqlDataAdapter
                    For Each s In ar
                        Dim sq = String.Format("select * from t_IIMembershipEnrollment where IIMembershipEnrollmentID in ({0})", s)

                        ada.SelectCommand = New SqlCommand(sq, cnn)
                        Dim cmd = New SqlCommandBuilder(ada)

                        Dim dt = New DataTable()
                        ada.Fill(dt)

                        If dt.Rows.Count = 1 Then
                            If dt.Rows(0)("dateexported").Equals(DBNull.Value) Then
                                dt.Rows(0)("dateexported") = DateTime.Now
                                dt.Rows(0)("exportstatusid") = 1

                                'ada.Update(dt)
                            End If
                        End If
                    Next

                End Using
            End Using

            Dim re = litResult.Text
            litResult.Text = String.Empty

            html_export_excel(litResult)
        End If
    End Sub

    Protected Sub Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Submit.Click
        Dim arr As New ArrayList()

        For Each dgv As GridViewRow In gvExport.Rows
            Dim cbx As CheckBox = DirectCast(dgv.Cells(0).FindControl("cbx"), CheckBox)
            If cbx.Checked Then
                arr.Add(gvExport.DataKeys(dgv.RowIndex)("ContractID").ToString())
            End If
        Next

        If arr.Count > 0 Then
            Dim sq = String.Format("select * from t_IIMembershipEnrollment WHERE contractID in ({0})", _
                                   String.Join(",", DirectCast(arr.ToArray(GetType(String)), String())))

            Using ada = New SqlDataAdapter(sq, New SqlConnection(Resources.Resource.cns))
                Dim cb = New SqlCommandBuilder(ada)
                Dim dt = New DataTable()

                ada.Fill(dt)
                For Each dr As DataRow In dt.Rows
                    dr.Item("DateAdded") = DateTime.Now
                Next
                ada.Update(dt)
            End Using

            'a36550
            gvExport.DataSource = Nothing
            gvExport.DataBind()

            btn_Retrieve_Click(Nothing, EventArgs.Empty)

        End If
    End Sub

    Protected Sub btn_Retrieve_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Retrieve.Click
        get_points_contracts()
    End Sub

    Protected Sub btn_retrieve_report_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_retrieve_report.Click
        litResult.Text = String.Empty

        print_new_and_upgrades(get_New_Point_Contracts(), litResult, "new")

    End Sub

    Protected Sub btn_ToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ToExcel.Click

        Dim sb As New StringBuilder()

        sb.Append("<table>")

        For Each gvr As GridViewRow In gvExport.Rows
            sb.Append("<tr>")
            For Each tc As TableCell In gvr.Cells
                sb.AppendFormat("<td>{0}</td>", tc.Text)
            Next
            sb.Append("</tr>")
        Next

        sb.Append("</table>")

        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", String.Format("attachment; filename=IIMebershipEnrollment_{0}_{1}.xls", DateTime.Now.ToLongTimeString(), DateTime.Now.Millisecond))
        Response.Write(sb.ToString())
        Response.End()

    End Sub


    Protected Sub btn_ExportNonIIContract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ExportNonIIContract.Click
        Dim sb = New StringBuilder()
        Dim list = From li In cbl_cxl.Items.OfType(Of ListItem)() _
                   Where li.Selected = True _
                   Select li.Value

        Dim sql = String.Format("select *, (select comboitem from t_comboitems where comboitemid = c.statusid) 'Cancels' from t_contract c " & _
                                "inner join t_prospect p on c.prospectid = p.prospectid " & _
                "where c.contractid in (select contractid from t_iimembershipenrollment " & _
                "where exportstatusid = 1 and dateexported is not null) and statusdate between '{1}' and '{2}' " & _
                "and c.statusid in ({0})", String.Join(",", list.ToArray()), sdate.Selected_Date, edate.Selected_Date)


        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using ada = New SqlDataAdapter(sql, cnn)
                Dim dt = New DataTable()

                ada.Fill(dt)
                Dim ii_owners = New List(Of IIOwner)

                If dt.Rows.Count > 0 Then
                    For Each r As DataRow In dt.Rows

                        ii_owners.Add(New IIOwner() With {.FirstName = r("FirstName").ToString(), _
                                             .LastName = r("LastName").ToString(), _
                                             .ProspectID = r("ProspectID").ToString(), _
                                             .ContractID = r("ContractID").ToString()})
                    Next

                    Dim dc = New DataContext(Resources.Resource.cns)
                    Dim tb_contract As Table(Of Contract) = dc.GetTable(Of Contract)()

                    sb.Append("<table border=1 style=border-collapse:collapse; class=f>")
                    sb.AppendFormat("{0}", label_report_headers())

                    For Each ii_owner As IIOwner In ii_owners.OrderBy(Function(x) x.LastName).ThenBy(Function(x) x.ProspectID)
                        Dim o = ii_owner
                        Dim ii_contracts As New List(Of Contract)

                        ii_contracts.Add(tb_contract.Single(Function(x) x.ContractID = o.ContractID))

                        For Each _contract As Contract In ii_contracts
                            Dim tmp = String.Empty
                            If _contract.AnniversaryDate.HasValue Then
                                tmp = _contract.AnniversaryDate.Value.ToShortDateString()
                            End If

                            sb.AppendFormat("<tr>")
                            sb.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>", _
                                tmp, _
                                o.LastName, o.FirstName, _contract.ContractNumber)

                            Dim _inventories As New Inventories(_contract.ContractID)
                            Dim _inventories_first = _inventories.OfType(Of Inventory).FirstOrDefault()

                            If _inventories_first IsNot Nothing Then
                                sb.AppendFormat("{0}", label_inventories(_inventories, 1))
                            Else

                                For i As Integer = 0 To 4
                                    sb.Append("<td>&nbsp;</td>")
                                Next

                            End If
                            '
                            '
                            'Print 2 or more rows of inventories belonged to same owner
                            If _inventories.OfType(Of Inventory).Count() > 3 Then
                                For i As Integer = 3 To _inventories.OfType(Of Inventory).Count() - 1 Step 3
                                    sb.AppendFormat("<tr>")
                                    For j = 1 To 4
                                        sb.AppendFormat("<td>&nbsp;</td>")
                                    Next
                                    sb.AppendFormat("{0}", label_inventories(_inventories, 1))

                                    sb.AppendFormat("</tr>")
                                Next
                            End If

                        Next
                    Next

                    sb.AppendFormat("</table>")
                    ll_NonII.Text = sb.ToString()
                    btn_ExportCancelled_Contracts.Visible = True

                Else
                    ll_NonII.Text = String.Format("<strong>No Records</strong>")
                    btn_ExportCancelled_Contracts.Visible = False
                End If
            End Using

        End Using
    End Sub


    Protected Sub btn_fin_submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_fin_submit.Click

        If String.IsNullOrEmpty(dt_fin_end.Selected_Date) Or String.IsNullOrEmpty(dt_fin_start.Selected_Date) Then Return

        Dim dic = New Dictionary(Of String, String)

        Dim sql = String.Format("select c.frequencyid, c.contractid, c.prospectid, ii.iiMembershipEnrollmentId, coalesce(v.conversionid, 0) conversionid " & _
                                          "from t_contract c inner join t_IIMembershipEnrollment ii on c.contractid = ii.contractid " & _
                                          "left join t_conversion v on v.contractid = c.contractid " & _
                                          "where c.contractid in " & _
                                          "(select contractid from t_IIMembershipEnrollment where exportstatusid = 1 and dateExported between '{0}' and '{1}') " & _
                                          "order by c.prospectid, c.frequencyid", _
                                          dt_fin_start.Selected_Date, _
                                          dt_fin_end.Selected_Date)

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using ada = New SqlDataAdapter(sql, cnn)

                Dim dt = New DataTable()
                ada.Fill(dt)

                For Each g As IGrouping(Of String, DataRow) In dt.AsEnumerable().GroupBy(Function(x) x("prospectid").ToString())
                    For Each dr As DataRow In g
                        dic.Add(dr("iiMembershipEnrollmentID").ToString(), _
                                String.Format("{0},{1},{2},{3}", dr("frequencyid").ToString(), _
                                              dr("conversionid").ToString(), _
                                              g.Key, _
                                              dr("contractid").ToString()))
                        hfd_keys_billed.Value += String.Format("{0} ", dr("iiMembershipEnrollmentID").ToString())
                        'get first row then break out of group
                        '
                        Exit For
                    Next
                Next
            End Using
        End Using


        Dim sb = New StringBuilder()
        If dic.Count > 0 Then

            sb.AppendFormat("<h3>{0} - {1} KCP SALES</h3>", DateTime.Parse(dt_fin_start.Selected_Date).ToString("MM/dd/yyyy"), _
                            DateTime.Parse(dt_fin_end.Selected_Date).ToString("MM/dd/yyyy"))

            sb.Append("<table id=table_conversion border=1>")

            sb.Append("<tr>")
            For Each s In New String() {"", "II Membership", "II Payback", "Reservation Fee", "# of Memberships"}
                sb.AppendFormat("<td><strong>{0}</strong></td>", s)
            Next
            sb.Append("</tr>")

            Dim values() = dic.Values.Where(Function(x) x.Split(",")(1) = "0").ToArray()
            Dim c_a = values.Where(Function(x) x.Split(",")(0) = "1").Count()
            Dim c_b = values.Where(Function(x) x.Split(",")(0) = "2").Count()
            Dim c_t = values.Where(Function(x) x.Split(",")(0) = "3").Count()

            For Each s In New String() {"Annual", "Biennial", "Triennial", "TOTAL"}
                sb.Append("<tr>")

                sb.AppendFormat("<td><strong>{0}</strong></td>", s)

                Select Case s
                    Case "Annual"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_a * 52.46, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_a * 10, 2, MidpointRounding.AwayFromZero).ToString("N2"))

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_a * 57.2, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", c_a)

                    Case "Biennial"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_b * 52.46, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_b * 10, 2, MidpointRounding.AwayFromZero).ToString("N2"))

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_b * 28.6, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", c_b)

                    Case "Triennial"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_t * 52.46, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_t * 10, 2, MidpointRounding.AwayFromZero).ToString("N2"))

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_t * 19.41, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", c_t)


                    Case "TOTAL"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round((c_a * 52.46) + (c_b * 52.46) + (c_t * 52.46), 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round((c_a * 10) + (c_b * 10) + (c_t * 10), 2, MidpointRounding.AwayFromZero).ToString("N2"))

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round((c_a * 57.2) + (c_b * 28.6) + (c_t * 19.41), 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", c_a + c_b + c_t)

                End Select

                sb.Append("</tr>")
            Next

            sb.Append("</table><br/></br></br>")

            lit_conversion.Text = sb.ToString()
        End If


        If dic.Count > 0 Then
            sb = New StringBuilder()

            sb.AppendFormat("<h3>{0} - {1} CONVERSIONS</h3>", DateTime.Parse(dt_fin_start.Selected_Date).ToString("MM/dd/yyyy"), _
                            DateTime.Parse(dt_fin_end.Selected_Date).ToString("MM/dd/yyyy"))

            sb.Append("<table  id=table_non_conversion border=1>")

            sb.Append("<tr>")
            For Each s In New String() {"", "II Membership", "II Payback", "Reservation Fee", "# of Memberships"}
                sb.AppendFormat("<td><strong>{0}</strong></td>", s)
            Next
            sb.Append("</tr>")

            Dim values() = dic.Values.Where(Function(x) x.Split(",")(1) <> "0").ToArray()
            Dim c_a = values.Where(Function(x) x.Split(",")(0) = "1").Count()
            Dim c_b = values.Where(Function(x) x.Split(",")(0) = "2").Count()
            Dim c_t = values.Where(Function(x) x.Split(",")(0) = "3").Count()

            For Each s In New String() {"Annual", "Biennial", "Triennial", "TOTAL"}
                sb.Append("<tr>")

                sb.AppendFormat("<td><strong>{0}</strong></td>", s)

                Select Case s
                    Case "Annual"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_a * 52.46, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_a * 10, 2, MidpointRounding.AwayFromZero).ToString("N2"))

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_a * 57.2, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", c_a)
                    Case "Biennial"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_b * 52.46, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_b * 10, 2, MidpointRounding.AwayFromZero).ToString("N2"))

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_b * 28.6, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", c_b)
                    Case "Triennial"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_t * 52.46, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_t * 10, 2, MidpointRounding.AwayFromZero).ToString("N2"))

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_t * 19.41, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", c_t)

                    Case "TOTAL"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round((c_a * 52.46) + (c_b * 52.46) + (c_t * 52.46), 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round((c_a * 10) + (c_b * 10) + (c_t * 10), 2, MidpointRounding.AwayFromZero).ToString("N2"))

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round((c_a * 57.2) + (c_b * 28.6) + (c_t * 19.41), 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", c_a + c_b + c_t)

                End Select
                sb.Append("</tr>")
            Next

            sb.Append("</table>")
            lit_non_conversions.Text = sb.ToString()

            btn_bill.Visible = IIf(dic.Count > 0, True, False)
        End If

    End Sub

    Protected Sub btn_bill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_bill.Click

        If hfd_keys_billed.Value <> String.Empty Then

            Dim ar() = hfd_keys_billed.Value.Trim().Split(New Char() {" "c})

            Using cnn = New SqlConnection(Resources.Resource.cns)
                Dim sql As String = String.Format("select * from t_IIMembershipEnrollment where iiMembershipEnrollmentID in ({0})", String.Join(New Char() {","}, ar))

                Using ada = New SqlDataAdapter(sql, cnn)
                    Dim bdr = New SqlCommandBuilder(ada)

                    Dim dt = New DataTable()
                    ada.Fill(dt)

                    For Each dr As DataRow In dt.Rows
                        dr("dateBilled") = DateTime.Now
                    Next

                    ada.Update(dt)
                End Using
            End Using

            btn_bill.Visible = False
        End If

    End Sub
#End Region



#Region "PRIVATE FUNCTIONS"

    


    ''' <summary>
    ''' For linking additional point contracts to originally submitted contract to II IntervalNational
    ''' </summary>    
    Private Function get_II_ContractNumber(prospectId As Int32) As String
        Dim contract_no = String.Empty

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Dim sql = String.Format("select top 1 c.contractNumber from t_contract c inner join t_IImembershipEnrollment ii " & _
                                    "on c.contractid = ii.contractid where c.prospectid = {0} and ii.exportStatusId = 1 and dateExported Is Not Null", prospectId)
            Using cmd = New SqlCommand(sql, cnn)
                Try
                    cnn.Open()
                    contract_no = CType(cmd.ExecuteScalar(), String)
                Catch ex As Exception
                    Response.Write(ex.Message)
                Finally
                    cmd.Dispose()
                    cnn.Close()
                    cnn.Dispose()
                End Try
            End Using
        End Using
        Return contract_no
    End Function


    Private Function get_II_MembershipEnrollmentID(contractId As Int32) As String
        Dim id = String.Empty

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Dim sql = String.Format("select IIMembershipEnrollmentID from t_IImembershipEnrollment where contractid = {0} ", contractId)
            Using cmd = New SqlCommand(sql, cnn)
                Try
                    cnn.Open()
                    id = CType(cmd.ExecuteScalar(), String)
                Catch ex As Exception
                    Response.Write(ex.Message)
                Finally
                    cmd.Dispose()
                    cnn.Close()
                    cnn.Dispose()
                End Try
            End Using
        End Using
        Return id
    End Function

    Private Function get_Season(contractId As Int32) As String

        Dim season = String.Empty

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Dim sql = String.Format("select coalesce((select comboitem from t_comboitems  where comboitemid = c.seasonid), '') season from t_contract c where contractid = {0}", contractId)
            Using cmd = New SqlCommand(sql, cnn)
                Try
                    cnn.Open()
                    season = CType(cmd.ExecuteScalar(), String)
                Catch ex As Exception
                    Response.Write(ex.Message)
                Finally
                    cmd.Dispose()
                    cnn.Close()
                    cnn.Dispose()
                End Try
            End Using
        End Using
        Return season
    End Function

    ''' <summary>
    ''' Owner purchased additional point contracts
    ''' </summary>
    Private Function get_Additional_Point_Contracts() As DataTable
        Dim dt = New DataTable()
        Dim sql = String.Format("select distinct prospectid from t_contract c where c.contractid in " & _
                                "(select contractid from t_IImembershipEnrollment where " & _
                                "exportstatusid = 1 and dateexported is not null)")
        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using ada = New SqlDataAdapter(sql, cnn)
                ada.Fill(dt)

                sql = String.Format("select * from t_contract c inner join t_prospect p on c.prospectid = p.prospectid " & _
                                    "where c.subtypeid = 18179 and c.prospectid in ({0}) " & _
                                    "and c.contractid not in (select contractid from t_iiMembershipEnrollment " & _
                                    "where exportstatusid = 1 and dateexported is not null) and p.prospectid <> 6199439", _
                                    String.Join(",", dt.AsEnumerable().Select(Function(x) x("prospectid").ToString()).ToArray()))
                ada.SelectCommand = New SqlCommand(sql, cnn)
                dt.Clear()
                ada.Fill(dt)
            End Using
        End Using
        Return dt
    End Function


    Private Function get_Resort_Code(unitType As String) As String

        Dim c As Char = unitType.Substring(0, 1).Trim().ToUpper()
        If c.CompareTo("E"c) = 0 Then
            Return "KCE"
        ElseIf c.CompareTo("T"c) = 0 Then
            Return "KCT"
        ElseIf c.CompareTo("C"c) = 0 Then
            Return "KCP"
        Else
            Return "N/A"
        End If
    End Function

    Private Function get_Unit_Number(unitName As String(), unitType As String, unitSize As Integer) As String

        Select Case unitType
            Case "Townes"
                If unitSize = 2 Then
                    Return String.Format("{0}{1}", get_StreetName_Abbreviation(unitName.First()), _
                                         unitName.First().Split(New Char() {" "c})(0))
                ElseIf unitSize = 4 Then

                    Return String.Format("{0}{1}", get_StreetName_Abbreviation(unitName.First()), _
                                         get_UnitNumber_Abbreviation(unitName))
                Else
                    Return "N/A"
                End If
            Case "Estates"

                If unitSize = 1 Then
                    Return String.Format("{0}{1}", get_StreetName_Abbreviation(unitName.First()), _
                            unitName.First().Split(New Char() {" "c})(0))

                ElseIf unitSize = 2 Then

                    Return String.Format("{0}{1}", get_StreetName_Abbreviation(unitName.First()), _
                    unitName.First().Split(New Char() {" "c})(0))

                ElseIf unitSize = 3 Then

                    Return String.Format("{0}{1}", get_StreetName_Abbreviation(unitName.First()), _
                                         get_UnitNumber_Abbreviation(unitName))
                ElseIf unitSize = 4 Then
                    
                    Return String.Format("{0}{1}", get_StreetName_Abbreviation(unitName.First()), _
                                         get_UnitNumber_Abbreviation(unitName))
                Else
                    Return "N/A"
                End If


            Case "Cottage"

                If unitSize = 3 Then
                    Return String.Format("{0}{1}", get_StreetName_Abbreviation(unitName.First()), _
                      unitName.First().Split(New Char() {" "c})(0))
                Else
                    Return "Cottage/NA"
                End If

            Case Else
                Return "Unknown"
        End Select

    End Function

    Private Function get_StreetName_Abbreviation(unitName As String) As String

        Dim parts = unitName.Trim().Split(New Char() {" "c})
        Dim tmp = String.Empty

        For i As Integer = 1 To parts.Length - 1
            tmp += parts(i).Substring(0, 1)
        Next
        Return tmp
    End Function

    Private Function get_UnitNumber_Abbreviation(unitName As String()) As String

        Dim numbers = unitName.Select(Function(x) x.Split(New Char() {" "c})(0))
        Dim l As New List(Of String)

        For Each n As String In numbers
            l.Add(n.Substring(0, n.Length - 1))
        Next

        If l.Distinct().Count() = 1 Then
            Dim tmp = l.First()

            For Each s As String In numbers
                tmp += s.Substring(s.Length - 1, 1)
            Next

            Return tmp
        Else
            Return String.Join(",", numbers.ToArray())
        End If

    End Function


    Private Function label_report_headers() As String
        Dim tmp = String.Empty

        For Each h In New String() {"Anniversary Date", "Member Last Name", "Member First Name", "Owner #"}
            tmp += String.Format("<th><strong>{0}</strong></th>", h.ToUpper())
        Next

        For Each h In New String() {"Resort Code", "Unit Size", "Unit #", "Unit Check-In", "Week #"}
            tmp += String.Format("<th><strong>{0}</strong></th>", h.ToUpper())
        Next

        Return tmp
    End Function

    Private Function label_report_headers(ByVal co_count As Int16, ByVal add_count As Int16, Optional ByVal show_orginal As Boolean = True) As String
        Dim tmp = String.Empty

        For Each h In New String() {"Anniversary Date", "Member Last Name", "Member First Name", "Owner #"}
            tmp += String.Format("<th><strong>{0}</strong></th>", h.ToUpper())
            COLUMN_MAX += 1
        Next

        For Each h In New String() {"Resort Code", "Unit Size", "Unit #", "Unit Check-In", "Week #"}
            tmp += String.Format("<th><strong>{0}</strong></th>", h.ToUpper())
            COLUMN_MAX += 1
        Next

        For Each h In New String() {"Season", "Frequency", "1st Club Usage"}
            tmp += String.Format("<th><strong>{0}</strong></th>", h.ToUpper())
            COLUMN_MAX += 1
        Next

        For i = 0 To co_count - 1
            tmp += String.Format("<th><strong>CO-OWNER {0}</strong></th>", i + 1)
            COLUMN_MAX += 1
        Next


        For Each h In New String() {"Email-1", "Email-2"}
            tmp += String.Format("<th><strong>{0}</strong></th>", h.ToUpper())
            COLUMN_MAX += 1
        Next

        For Each h In New String() {"Cell Phone", "Work Phone", "Home Phone"}
            tmp += String.Format("<th><strong>{0}</strong></th>", h.ToUpper())
            COLUMN_MAX += 1
        Next

        For i = 0 To add_count - 1
            For Each h In New String() {"Street Name", "City", "State", "Zip"}
                tmp += String.Format("<th><strong>{0}</strong></th>", h.ToUpper())
                COLUMN_MAX += 1
            Next
        Next

        For Each h In New String() {"II Membership #", "M&T Amount", "M&T", "Float/Fixed"}
            tmp += String.Format("<th><strong>{0}</strong></th>", h.ToUpper())
            COLUMN_MAX += 1
        Next

        If show_orginal Then
            tmp += String.Format("<th><strong>Original Contract</strong></th>")
        End If

        Return tmp
    End Function

    Private Function label_emails(ByVal addresses As String()) As String
        Dim tmp = String.Empty

        For Each s As String In addresses
            tmp += String.Format("<td>{0}</td>", s)
        Next
        Return tmp
    End Function

    Private Function label_phones(ByVal phones As String()) As String
        Dim tmp = String.Empty

        For Each p As String In phones
            tmp += String.Format("<td>{0}</td>", p)
        Next
        Return tmp
    End Function

    Private Function label_addresses(ByVal addresses As List(Of Address)) As String
        Dim tmp = String.Empty

        For i = 0 To addresses.OfType(Of Address).Count() - 1
            Dim a As Address = addresses.OfType(Of Address).ElementAt(i)
            tmp += String.Format("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>", a.Address1, a.City, get_State(a.State), get_Country(a.Country))
        Next
        Return tmp
    End Function


    Private Function label_inventories(ByVal _inventories As Inventories, ByVal time As Integer) As String
        Dim tmp = String.Empty

        'first time...
        If time = 1 Then

            Dim _inventories_first = _inventories.OfType(Of Inventory).FirstOrDefault()
            Dim names = From n In _inventories.OfType(Of Inventory)().Take(3) Select n.Name

            Dim size = From n In _inventories.OfType(Of Inventory)().Take(1) Where String.IsNullOrEmpty(n.Size) = False Select n.Size.Substring(0, 1)

            Dim unit_type = From n In _inventories.OfType(Of Inventory)().Take(1) Select n.Type

            Dim wk = From n In _inventories.OfType(Of Inventory)().Take(1) Select n.Week


            tmp += String.Format("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td>", get_Resort_Code(_inventories_first.Type), _
                             _inventories_first.Size, _
                             get_Unit_Number(names.ToArray(), unit_type.FirstOrDefault(), size.Sum(Function(x) x)), _
                             get_Unit_CheckInDay(_inventories_first.SoldInventoryID), _
                             wk.FirstOrDefault())

        Else

            For i As Integer = 3 To _inventories.OfType(Of Inventory).Count() - 1 Step 3

                Dim _inventories_next = _inventories.OfType(Of Inventory).Skip(i).Take(1).Single()

                Dim names = From n In _inventories.OfType(Of Inventory).Skip(i).Take(3) Select n.Name

                Dim size = From n In _inventories.OfType(Of Inventory).Skip(i).Take(1) Where String.IsNullOrEmpty(n.Size) = False Select n.Size.Substring(0, 1)

                Dim unit_type = From n In _inventories.OfType(Of Inventory)().Skip(i).Take(1) Select n.Type

                Dim wk = From n In _inventories.OfType(Of Inventory)().Skip(i).Take(1) Select n.Week

                tmp += String.Format("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td>", get_Resort_Code(_inventories_next.Type), _
                             _inventories_next.Size, _
                             get_Unit_Number(names.ToArray(), unit_type.FirstOrDefault(), size.Sum(Function(x) x)), _
                             get_Unit_CheckInDay(_inventories_next.SoldInventoryID), _
                             wk.FirstOrDefault())

            Next
        End If

        Return tmp
    End Function


#Region "Revision 1"



    Private Function get_New_Point_Contracts() As List(Of IIOwner)
        Dim sb = New StringBuilder()
        Dim dc = New DataContext(Resources.Resource.cns)
        Dim tb_contract As Table(Of Contract) = dc.GetTable(Of Contract)()
        Dim ii_owners = New List(Of IIOwner)

        Dim sql = String.Format("select * from (SELECT * FROM V_IICOOWNERENROLLMENT " & _
                                                       "UNION " & _
                                                       "SELECT * FROM V_IIOWNERENROLLMENT) a " & _
                                                       "where AnniversaryDate is null  " & _
                                                       "order by contractnumber, [PRIMARY] desc ")

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using ada = New SqlDataAdapter(sql, cnn)

                Dim dt = New DataTable()
                ada.Fill(dt)

                For Each r As DataRow In dt.Rows
                    ii_owners.Add(New IIOwner() With {.FirstName = r("FirstName").ToString(), _
                   .LastName = r("LastName").ToString(), _
                   .ProspectID = r("ProspectID").ToString(), _
                   .ContractID = r("ContractID").ToString()})

                    hf_iiMembershipEnrollmentID.Value += String.Format("{0} ", r("IIMembershipEnrollmentID").ToString())
                Next
            End Using
        End Using

        Return ii_owners
    End Function


    Private Shared Function get_G(ByVal contract_date As DateTime, Optional ByVal u_type As String = "") As String
        If contract_date.CompareTo(DateTime.Parse("11/18/2005")) < 0 Then
            Return "G4"
        ElseIf contract_date.CompareTo(DateTime.Parse("11/18/2005")) >= 0 And contract_date.CompareTo(DateTime.Parse("02/25/2008")) < 0 Then
            Return "G3"
        ElseIf contract_date.CompareTo(DateTime.Parse("02/25/2008")) >= 0 And contract_date.CompareTo(DateTime.Parse("12/14/2009")) < 0 Then
            If u_type.CompareTo("KCT-4BED") = 0 Then
                Return "G2"
            Else
                Return "G1"
            End If
        ElseIf contract_date.CompareTo(DateTime.Parse("12/14/2009")) >= 0 Then
            Return "G1"
        Else
            Return "N/A"
        End If
    End Function
    Private Shared Function get_II_Membership(ByVal prospectId As Int32) As String
        Dim sql = String.Format("select top 1 UFValue from t_uf_value where ufid = 362 and keyvalue = {0} order by keyvalue desc ", prospectId)

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using cmd = New SqlCommand(sql, cnn)
                cnn.Open()
                sql = cmd.ExecuteScalar()
            End Using
        End Using
        Return IIf(String.IsNullOrEmpty(sql), String.Empty, sql)
    End Function
    Private Function get_Frequency(ByVal id As Integer) As String
        If id = 1 Then
            Return "Annual"
        ElseIf id = 2 Then
            Return "Biennial"
        ElseIf id = 3 Then
            Return "Triennial"
        Else
            Return "N/A"
        End If
    End Function
    Private Function get_ClubUsage(ByVal con_id As String) As String

        Dim sq As String = String.Format( _
                        "select (select top 1 occupancyYear from t_conversion where contractid = {0} order by conversionid desc) 'OCCUPANCY-YEAR', " & _
                        "occupancyDate 'OCCUPANCY-DATE' from t_contract where contractid = {0}", con_id)

        Using ada As New SqlDataAdapter(sq, New SqlConnection(Resources.Resource.cns))
            Dim dt = New DataTable()
            ada.Fill(dt)

            If dt.Rows.Count = 1 Then
                If dt.Rows(0).Item("OCCUPANCY-YEAR").Equals(DBNull.Value) = False Then
                    Return dt.Rows(0).Item("OCCUPANCY-YEAR")
                Else
                    Return IIf(dt.Rows(0)("OCCUPANCY-DATE").Equals(DBNull.Value), "", DateTime.Parse(dt.Rows(0).Item("OCCUPANCY-DATE").ToString()).Year)
                End If
            Else
                Return String.Empty
            End If
        End Using
    End Function
#End Region

#Region "Private functions"
    Private Function get_Unit_CheckInDay(ByVal soldInventoryId As String) As String
        Dim s = String.Empty

        Using cnn As New SqlConnection(Resources.Resource.cns)
            s = String.Format("select top 1 cck.comboitem [Check-In Date] " & _
                            "from t_soldinventory so " & _
                            "inner join t_salesinventory si on so.salesinventoryid = si.salesinventoryid " & _
                            "inner join t_unit u on u.unitid = si.unitid " & _
                            "inner join t_room r on r.unitid = u.unitid " & _
                            "left outer join t_ComboItems cck on cck.comboItemId = r.subTypeId " & _
                            "where so.soldinventoryid = {0}", soldInventoryId)
            Using cmd As New SqlCommand(s, cnn)
                cnn.Open()
                s = cmd.ExecuteScalar()
            End Using
        End Using
        Return IIf(String.IsNullOrEmpty(s), "N/A", s)
    End Function
    Private Function get_ContractNumber(ByVal contractid As Int32) As String

        Dim contractNumber = String.Empty

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using cmd = New SqlCommand(String.Format("select contractnumber from t_contract where contractid = {0}", contractid), cnn)

                Try
                    cnn.Open()
                    contractNumber = CType(cmd.ExecuteScalar(), String)
                Catch ex As Exception
                    Response.Write(String.Format("<strong>{0}</strong>", ex.Message))
                Finally
                    cnn.Close()
                    cmd.Dispose()

                End Try

                Return contractNumber
            End Using
        End Using
    End Function
    Private Shared Function get_Inventories(ByVal contractId As Int32) As Inventory()

        Dim list As New List(Of Inventory)

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using ada = New SqlDataAdapter(String.Format("select * from ufn_contractInventory({0})", contractId), cnn)

                Dim dt = New DataTable()
                ada.Fill(dt)

                For Each row As DataRow In dt.Rows
                    If String.IsNullOrEmpty(row.Field(Of String)("Name1")) = False Then

                        list.Add(New Inventory() With { _
                                 .Name = row.Field(Of String)("Name1"), _
                                 .Size = row.Field(Of String)("Size"), _
                                 .Type = row.Field(Of String)("UnitType1"), _
                                 .SubType = row.Field(Of String)("UnitSubType1"), _
                                 .ContractDate = row.Field(Of DateTime)("ContractDate"), _
                                 .Week = Convert.ToInt16(row.Field(Of Integer)("Week")), _
                                 .WeekType = row.Field(Of String)("WeekType"), _
                                 .MF = row.Field(Of Decimal)("MF"), _
                                 .SoldInventoryID = row.Field(Of Int32)("SoldInventoryID1")})

                    End If

                    If String.IsNullOrEmpty(row.Field(Of String)("Name2")) = False Then
                        list.Add(New Inventory() With { _
                               .Name = row.Field(Of String)("Name2"), _
                               .Size = String.Empty, _
                               .Type = row.Field(Of String)("UnitType2"), _
                               .SubType = row.Field(Of String)("UnitSubType2"), _
                               .ContractDate = row.Field(Of DateTime)("ContractDate"), _
                               .Week = 0, _
                               .WeekType = row.Field(Of String)("WeekType"), _
                               .MF = row.Field(Of Decimal)("MF"), _
                               .SoldInventoryID = row.Field(Of Int32)("SoldInventoryID2")})

                    End If

                    If String.IsNullOrEmpty(row.Field(Of String)("Name3")) = False Then

                        list.Add(New Inventory() With { _
                              .Name = row.Field(Of String)("Name3"), _
                              .Size = String.Empty, _
                              .Type = row.Field(Of String)("UnitType3"), _
                              .SubType = row.Field(Of String)("UnitSubType3"), _
                              .ContractDate = row.Field(Of DateTime)("ContractDate"), _
                              .Week = 0, _
                              .WeekType = row.Field(Of String)("WeekType"), _
                              .MF = row.Field(Of Decimal)("MF"), _
                              .SoldInventoryID = row.Field(Of Int32)("SoldInventoryID3")})

                    End If
                Next
            End Using
        End Using
        Return list.ToArray()
    End Function
    Private Function get_State(ByVal stateID As Int32) As String

        If STATE_DIC Is Nothing Then
            Using cnn = New SqlConnection(Resources.Resource.cns)
                Using ada = New SqlDataAdapter(String.Format("select * from t_comboItems where comboid = 281"), cnn)
                    Dim dt = New DataTable()
                    ada.Fill(dt)
                    STATE_DIC = New Dictionary(Of Int32, String)()
                    For Each row In dt.Rows
                        STATE_DIC.Add(Int32.Parse(row("ComboItemID").ToString()), row("ComboItem").ToString())
                    Next
                End Using
            End Using
        End If

        If STATE_DIC.ContainsKey(stateID) Then
            Return STATE_DIC(stateID)
        Else
            Return String.Empty
        End If

    End Function
    Private Function get_Country(ByVal countryID As Int32) As String

        If COUNTRY_DIC Is Nothing Then
            Using cnn = New SqlConnection(Resources.Resource.cns)
                Using ada = New SqlDataAdapter(String.Format("select * from t_comboItems where comboid = 239"), cnn)
                    Dim dt = New DataTable()
                    ada.Fill(dt)
                    COUNTRY_DIC = New Dictionary(Of Int32, String)()
                    For Each row In dt.Rows
                        COUNTRY_DIC.Add(Int32.Parse(row("ComboItemID").ToString()), row("ComboItem").ToString())
                    Next
                End Using
            End Using
        End If
        If COUNTRY_DIC.ContainsKey(countryID) Then
            Return COUNTRY_DIC(countryID)
        Else
            Return String.Empty
        End If

    End Function
#End Region



#End Region

    Private Class IIOwner

        Private _lastname As String
        Private _firstname As String
        Private _prospectID As Int32
        Private _contractID As Int32
        Private _emails As Emails
        Private _phones As PhoneNumbers
        Private _addresses As Addresses

        Public ReadOnly Property Emails As Emails
            Get
                Return _emails
            End Get
        End Property

        Public ReadOnly Property Phones As PhoneNumbers
            Get
                Return _phones
            End Get
        End Property

        Public ReadOnly Property Addresses As Addresses
            Get
                Return _addresses
            End Get
        End Property

        Public ReadOnly Property CoOwners As IIOwner()
            Get

                Dim l = New List(Of IIOwner)

                Using cnn = New SqlConnection(Resources.Resource.cns)
                    Using ada = New SqlDataAdapter(String.Format("select * from ufn_owner_coOwners_Phones_Emails({0})", _prospectID), cnn)
                        Dim dt = New DataTable()
                        ada.Fill(dt)

                        For Each r As DataRow In dt.Rows.OfType(Of DataRow).Where(Function(x) x.Field(Of Integer)("primary") = 0)
                            l.Add(New IIOwner() With {.FirstName = r("FirstName").ToString(), _
                                                      .LastName = r("LastName").ToString()})
                        Next

                        Dim row = dt.Rows.OfType(Of DataRow).Where(Function(x) x.Field(Of Integer)("primary") = 1).Single()
                        Dim e As New List(Of Email)
                        e.Add(New Email() With {.Address = IIf(row("email1").Equals(DBNull.Value), "", row("email1").ToString())})
                        e.Add(New Email() With {.Address = IIf(row("email2").Equals(DBNull.Value), "", row("email2").ToString())})

                        _emails = New Emails(e.ToArray())

                        Dim p As New List(Of PhoneNumber)
                        p.Add(New PhoneNumber() With {.Number = IIf(row("homephone").Equals(DBNull.Value), "", row("homephone").ToString()), .Type = "Home"})
                        p.Add(New PhoneNumber() With {.Number = IIf(row("workphone").Equals(DBNull.Value), "", row("workphone").ToString()), .Type = "Work"})
                        p.Add(New PhoneNumber() With {.Number = IIf(row("mobilphone").Equals(DBNull.Value), "", row("mobilphone").ToString()), .Type = "Mobile"})

                        _phones = New PhoneNumbers(p.ToArray())
                        _addresses = New Addresses(_prospectID)
                    End Using
                End Using

                Return l.ToArray()
            End Get
        End Property

        Public Property LastName As String
            Get
                Return _lastname
            End Get
            Set(ByVal value As String)
                _lastname = value
            End Set
        End Property

        Public Property FirstName As String
            Get
                Return _firstname
            End Get
            Set(ByVal value As String)
                _firstname = value
            End Set
        End Property

        Public Property ProspectID As Int32
            Get
                Return _prospectID
            End Get
            Set(ByVal value As Int32)
                _prospectID = value
            End Set
        End Property

        Public Property ContractID As Int32
            Get
                Return _contractID
            End Get
            Set(ByVal value As Int32)
                _contractID = value
            End Set
        End Property
    End Class

    <System.Data.Linq.Mapping.Table(Name:="t_contract")> _
    Private Class Contract

        Private _contractID As Int32
        Private _contractNumber As String
        Private _frequencyID As Integer
        Private _anniversaryDate As DateTime?
        Private _inventories As Inventories
        Private _seasonID As Integer

        Public ReadOnly Property Inventories As Inventories
            Get
                _inventories = New Inventories(_contractID)
                Return _inventories
            End Get
        End Property

        <System.Data.Linq.Mapping.Column()> _
        Public Property AnniversaryDate As DateTime?
            Get
                Return _anniversaryDate
            End Get
            Set(ByVal value As DateTime?)
                _anniversaryDate = value
            End Set
        End Property

        <System.Data.Linq.Mapping.Column()> _
        Public Property FrequencyID As Integer
            Get
                Return _frequencyID
            End Get
            Set(ByVal value As Integer)
                _frequencyID = value
            End Set
        End Property

        <System.Data.Linq.Mapping.Column()> _
        Public Property SeasonID As Integer
            Get
                Return _seasonID
            End Get
            Set(ByVal value As Integer)
                _seasonID = value
            End Set
        End Property

        <System.Data.Linq.Mapping.Column()> _
        Public Property ContractID As Int32
            Get
                Return _contractID
            End Get
            Set(ByVal value As Int32)
                _contractID = value
            End Set
        End Property

        <System.Data.Linq.Mapping.Column()> _
        Public Property ContractNumber As String
            Get
                Return _contractNumber
            End Get
            Set(ByVal value As String)
                _contractNumber = value
            End Set
        End Property

        Public ReadOnly Property ClubUsage As Int16
            Get
                Dim sq = String.Format( _
                 "select (select top 1 occupancyYear from t_conversion where contractid = {0} order by conversionid desc) 'OCCUPANCY-YEAR', " & _
                 "occupancyDate 'OCCUPANCY-DATE' from t_contract where contractid = {0}", _contractID)

                Using ada = New SqlDataAdapter(sq, New SqlConnection(Resources.Resource.cns))
                    Dim dt = New DataTable()
                    ada.Fill(dt)

                    If dt.Rows.Count = 1 Then
                        If dt.Rows(0).Item("OCCUPANCY-YEAR").Equals(DBNull.Value) = False Then
                            Return Convert.ToInt16(dt.Rows(0).Item("OCCUPANCY-YEAR"))
                        Else
                            Return IIf(dt.Rows(0)("OCCUPANCY-DATE").Equals(DBNull.Value), 0, DateTime.Parse(dt.Rows(0).Item("OCCUPANCY-DATE").ToString()).Year)
                        End If
                    Else
                        Return 0
                    End If
                End Using
            End Get
        End Property
    End Class

    Private Class Contracts
        Implements IEnumerable

        Private list() As Contract

        Public Sub New(ByVal l() As Contract)
            list = l
        End Sub

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return New MyEnumerator(list)
        End Function

        Private Class MyEnumerator
            Implements IEnumerator

            Private position As Integer = -1
            Private list() As Contract

            Public Sub New(ByVal l() As Contract)
                list = l
            End Sub

            Public ReadOnly Property Current As Object Implements System.Collections.IEnumerator.Current
                Get
                    Try
                        Return list(position)
                    Catch ex As IndexOutOfRangeException
                        Throw New InvalidOperationException()
                    End Try
                End Get
            End Property

            Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
                position += 1
                Return position < list.Length
            End Function

            Public Sub Reset() Implements System.Collections.IEnumerator.Reset
                position = -1
            End Sub
        End Class
    End Class

    <System.Data.Linq.Mapping.Table(Name:="t_prospectaddress")> _
    Private Class Address

        Private _address1 As String
        Private _address2 As String
        Private _city As String
        Private _state As Int32
        Private _zip As String
        Private _country As Int32
        Private _prospectid As Int32
        Private _activeFlag As Boolean


        <System.Data.Linq.Mapping.Column()> _
        Public Property ProspectID As Int32
            Get
                Return _prospectid
            End Get
            Set(ByVal value As Int32)
                _prospectid = value
            End Set
        End Property

        <System.Data.Linq.Mapping.Column()> _
        Public Property ActiveFlag As Boolean
            Get
                Return _activeFlag
            End Get
            Set(ByVal value As Boolean)
                _activeFlag = value
            End Set
        End Property


        <System.Data.Linq.Mapping.Column()> _
        Public Property Address1 As String
            Get
                Return _address1
            End Get
            Set(ByVal value As String)
                _address1 = value
            End Set
        End Property

        <System.Data.Linq.Mapping.Column()> _
        Public Property Address2 As String
            Get
                Return _address2
            End Get
            Set(ByVal value As String)
                _address2 = value
            End Set
        End Property

        <System.Data.Linq.Mapping.Column()> _
        Public Property City As String
            Get
                Return _city
            End Get
            Set(ByVal value As String)
                _city = value
            End Set
        End Property

        <System.Data.Linq.Mapping.Column(Name:="StateID")> _
        Public Property State As Int32
            Get
                Return _state
            End Get
            Set(ByVal value As Int32)
                _state = value
            End Set
        End Property

        <System.Data.Linq.Mapping.Column(Name:="PostalCode")> _
        Public Property Zip As String
            Get
                Return _zip
            End Get
            Set(ByVal value As String)
                _zip = value
            End Set
        End Property

        <System.Data.Linq.Mapping.Column(Name:="CountryID")> _
        Public Property Country As Int32
            Get
                Return _country
            End Get
            Set(ByVal value As Int32)
                _country = value
            End Set
        End Property

    End Class

    ''' <summary>
    ''' Custom enumerable class over class Adress
    ''' </summary>
    Private Class Addresses
        Implements IEnumerable

        Private _prospectID As Int32

        Public Sub New(ByVal prospectId As Int32)
            Me._prospectID = prospectId
        End Sub

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Dim dc As New DataContext(Resources.Resource.cns)
            Dim tb As Table(Of Address) = dc.GetTable(Of Address)()
            Return New Enumerator(tb.Where(Function(x) x.ProspectID.Equals(_prospectID) And x.ActiveFlag = True).ToArray())
        End Function

        Private Class Enumerator
            Implements IEnumerator

            Private position As Integer = -1
            Private list As Address()

            Public Sub New(ByVal a() As Address)
                list = a
            End Sub

            Public ReadOnly Property Current As Object Implements System.Collections.IEnumerator.Current
                Get
                    Try
                        Return list(position)
                    Catch ex As IndexOutOfRangeException
                        Throw New InvalidOperationException()
                    End Try
                End Get
            End Property

            Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
                position += 1
                Return position < list.Length
            End Function

            Public Sub Reset() Implements System.Collections.IEnumerator.Reset
                position = -1
            End Sub
        End Class
    End Class

    ''' <summary>
    ''' A custom class by choosing few columns from function ufn_contractinventory
    ''' </summary>    
    Private Class Inventory

        Private _name As String
        Private _size As String
        Private _soldInventoryId As Int32
        Private _type As String
        Private _subType As String
        Private _week As Int16
        Private _contractDate As String
        Private _mf As Decimal
        Private _weekType As String

        Public Property ContractDate As String
            Get
                Return _contractDate
            End Get
            Set(ByVal value As String)
                _contractDate = value
            End Set
        End Property

        Public Property Week As Int16
            Get
                Return _week
            End Get
            Set(ByVal value As Int16)
                _week = value
            End Set
        End Property

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property Size As String
            Get
                Return _size
            End Get
            Set(ByVal value As String)
                _size = value
            End Set
        End Property

        Public Property Type As String
            Get
                Return _type
            End Get
            Set(ByVal value As String)
                _type = value
            End Set
        End Property

        Public Property SubType As String
            Get
                Return _subType
            End Get
            Set(ByVal value As String)
                _subType = value
            End Set
        End Property

        Public Property SoldInventoryID As Int32
            Get
                Return _soldInventoryId
            End Get
            Set(ByVal value As Int32)
                _soldInventoryId = value
            End Set
        End Property

        Public Property MF As Decimal
            Get
                Return _mf.ToString("c")
            End Get
            Set(ByVal value As Decimal)
                _mf = value
            End Set
        End Property

        Public Property WeekType As String
            Get
                Return _weekType
            End Get
            Set(ByVal value As String)
                _weekType = value
            End Set
        End Property

    End Class

    Private Class Inventories
        Implements IEnumerable

        Private _contractId As Int32
        Public Sub New(ByVal contractId As Int32)
            Me._contractId = contractId
        End Sub

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return New Enumerator(get_Inventories(_contractId))
        End Function

        Private Class Enumerator
            Implements IEnumerator

            Private position As Integer = -1
            Private l() As Inventory
            Public Sub New(ByVal inventories() As Inventory)
                l = inventories
            End Sub

            Public ReadOnly Property Current As Object Implements System.Collections.IEnumerator.Current
                Get
                    Try
                        Return l(position)
                    Catch ex As IndexOutOfRangeException
                        Throw New InvalidOperationException()
                    End Try
                End Get
            End Property

            Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
                position += 1
                Return position < l.Length
            End Function

            Public Sub Reset() Implements System.Collections.IEnumerator.Reset
                position = -1
            End Sub
        End Class
    End Class

    Private Class Email

        Private _address As String
        Public Property Address As String
            Get
                Return _address
            End Get
            Set(ByVal value As String)
                _address = value
            End Set
        End Property
    End Class

    Private Class Emails
        Implements IEnumerable

        Private list() As Email

        Public Sub New(ByVal l() As Email)
            list = l
        End Sub

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return New MyEnumerator(list)
        End Function

        Private Class MyEnumerator
            Implements IEnumerator

            Private position As Integer = -1
            Private list() As Email

            Public Sub New(ByVal l() As Email)
                list = l
            End Sub

            Public ReadOnly Property Current As Object Implements System.Collections.IEnumerator.Current
                Get
                    Try
                        Return list(position)
                    Catch ex As IndexOutOfRangeException
                        Throw New InvalidOperationException()
                    End Try
                End Get
            End Property

            Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
                position += 1
                Return position < list.Length
            End Function

            Public Sub Reset() Implements System.Collections.IEnumerator.Reset
                position = -1
            End Sub
        End Class
    End Class

    Private Class PhoneNumber

        Private _number As String
        Private _type As String

        Public Property Number As String
            Get
                Return _number
            End Get
            Set(ByVal value As String)
                _number = value
            End Set
        End Property

        Public Property Type As String
            Get
                Return _type
            End Get
            Set(ByVal value As String)
                _type = value
            End Set
        End Property

    End Class

    Private Class PhoneNumbers
        Implements IEnumerable

        Private list() As PhoneNumber
        Public Sub New(ByVal l() As PhoneNumber)
            list = l
        End Sub

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return New MyEnumerator(list)
        End Function

        Private Class MyEnumerator
            Implements IEnumerator

            Private position As Integer = -1
            Private list() As PhoneNumber

            Public Sub New(ByVal l() As PhoneNumber)
                list = l
            End Sub

            Public ReadOnly Property Current As Object Implements System.Collections.IEnumerator.Current
                Get
                    Try
                        Return list(position)
                    Catch ex As IndexOutOfRangeException
                        Throw New InvalidOperationException()
                    End Try
                End Get
            End Property

            Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
                position += 1
                Return position < list.Length
            End Function

            Public Sub Reset() Implements System.Collections.IEnumerator.Reset
                position = -1
            End Sub
        End Class
    End Class



    Private Sub print_new_and_upgrades(ByVal ii_owners As List(Of IIOwner), ByVal lit As Literal, ByVal reportType As String)

        Dim sb = New StringBuilder()
        Dim dc = New DataContext(Resources.Resource.cns)
        Dim tb_contract As Table(Of Contract) = dc.GetTable(Of Contract)()

        Dim CO_MAX = ii_owners.Max(Function(x) x.CoOwners.Count())
        Dim ADDRESS_MAX = ii_owners.Max(Function(x) x.Addresses.OfType(Of Address).Count())

        sb.Append("<table border=1 style=border-collapse:collapse; class=f>")
        sb.AppendFormat("<tr>{0}</tr>", label_report_headers(CO_MAX, ADDRESS_MAX))

        For Each ii_owner As IIOwner In ii_owners.OrderBy(Function(x) x.LastName).ThenBy(Function(x) x.ProspectID)
            Dim o = ii_owner
            Dim ii_contracts As New List(Of Contract)

            ii_contracts.Add(tb_contract.Single(Function(x) x.ContractID = o.ContractID))

            For Each _contract As Contract In ii_contracts

                Dim tmp = String.Empty
                If _contract.AnniversaryDate.HasValue Then
                    tmp = _contract.AnniversaryDate.Value.ToShortDateString()
                End If

                sb.AppendFormat("<tr>")
                sb.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>", _
                    tmp, _
                    o.LastName, o.FirstName, _contract.ContractNumber)

                Dim _inventories As New Inventories(_contract.ContractID)
                Dim _inventories_first = _inventories.OfType(Of Inventory).FirstOrDefault()

                If _inventories_first IsNot Nothing Then

                    sb.AppendFormat("{0}", label_inventories(_inventories, 1))
                Else

                    For i As Integer = 0 To 4
                        sb.Append("<td>&nbsp;</td>")
                    Next

                End If

                sb.AppendFormat("<td>{0}</td>", get_Season(_contract.ContractID))
                sb.AppendFormat("<td>{0}</td>", get_Frequency(_contract.FrequencyID))
                sb.AppendFormat("<td>{0}</td>", _contract.ClubUsage)


                For i As Integer = 0 To CO_MAX - 1
                    Try
                        sb.AppendFormat("<td>{0}, {1}</td>", ii_owner.CoOwners(i).LastName, ii_owner.CoOwners(i).FirstName)
                    Catch ex As Exception
                        sb.Append("<td>&nbsp;</td>")
                    End Try

                Next

                sb.AppendFormat("{0}", label_emails(New String() {ii_owner.Emails.OfType(Of Email).ElementAt(0).Address, _
                                                                  ii_owner.Emails.OfType(Of Email).ElementAt(1).Address}))


                sb.AppendFormat("{0}", label_phones(New String() {ii_owner.Phones.OfType(Of PhoneNumber).Single(Function(x) x.Type = "Mobile").Number, _
                                                                  ii_owner.Phones.OfType(Of PhoneNumber).Single(Function(x) x.Type = "Work").Number, _
                                                                  ii_owner.Phones.OfType(Of PhoneNumber).Single(Function(x) x.Type = "Home").Number}))


                sb.AppendFormat("{0}", label_addresses(ii_owner.Addresses.OfType(Of Address).ToList()))

                For i = ii_owner.Addresses.OfType(Of Address).Count() To ADDRESS_MAX - 1
                    sb.AppendFormat("<td colspan=4>&nbsp;</td>")
                Next


                sb.AppendFormat("<td>{0}</td>", get_II_Membership(ii_owner.ProspectID))

                If _inventories_first Is Nothing Then

                    sb.AppendFormat("<td>{0}</td>", "")
                    sb.AppendFormat("<td>{0}</td>", "")
                    sb.AppendFormat("<td>{0}</td>", "")
                Else

                    sb.AppendFormat("<td>{0}</td>", get_G(_inventories_first.ContractDate, _
                                                          String.Format("{0}-{1}", _inventories_first.Type, _inventories_first.SubType)))

                    sb.AppendFormat("<td>{0}</td>", _inventories_first.MF)
                    sb.AppendFormat("<td>{0}</td>", _inventories_first.WeekType)

                End If

                sb.AppendFormat("<td>{0}</td>", get_II_ContractNumber(o.ProspectID))
                sb.AppendFormat("</tr>")

                If _inventories.OfType(Of Inventory).Count() > 3 Then
                    For i As Integer = 3 To _inventories.OfType(Of Inventory).Count() - 1 Step 3
                        sb.AppendFormat("<tr>")
                        For j = 1 To 4
                            sb.AppendFormat("<td>&nbsp;</td>")
                        Next
                        sb.AppendFormat("{0}", label_inventories(_inventories, 1))

                        For j = 5 To COLUMN_MAX - (4 * CO_MAX)
                            sb.AppendFormat("<td>&nbsp;</td>")
                        Next

                        sb.AppendFormat("</tr>")
                    Next
                End If

            Next
        Next

        sb.Append("</table>")



        If reportType = "new" Then

            If ii_owners.Count > 0 Then

                view2_h5.InnerHtml = "To open in Excel and send to II, click Export button."
                CheckToExport.Visible = True

                lit.Text = sb.ToString()

            Else
                lit.Text = String.Format("<h1>No contracts found!</h1>")
            End If

        ElseIf reportType = "upgrade" Then
            If ii_owners.Count > 0 Then

                lit.Text = sb.ToString()
                lit_view_6_h5.InnerHtml = "To open in Excel and send to II, click Export button."
                view_6_export.Visible = True

            Else
                lit_view_6.Text = String.Format("<h1>No contracts found!</h1>")
            End If
        End If
    End Sub

    Private Sub html_Additional_Point_Contracts()

        Dim dt = get_Additional_Point_Contracts()
        Dim ii_owners = New List(Of IIOwner)

        For Each r As DataRow In dt.Rows

            ii_owners.Add(New IIOwner() With {.FirstName = r("FirstName").ToString(), _
                                              .LastName = r("LastName").ToString(), _
                                              .ProspectID = r("ProspectID").ToString(), _
                                              .ContractID = r("ContractID").ToString()})

            Dim ii_id As String = get_II_MembershipEnrollmentID(r("ContractID").ToString())
            If String.IsNullOrEmpty(ii_id) = False Then
                hf_iiMembershipEnrollmentID_upgrades.Value += String.Format("{0} ", ii_id)
            End If

        Next

        print_new_and_upgrades(ii_owners, lit_view_6, "upgrade")



    End Sub

    Private Sub html_export_excel(ByVal lit As Literal)

        Dim re = lit.Text
        lit.Text = String.Empty

        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", String.Format("attachment; filename=IIMebershipEnrollment_additionals{0}_{1}.xls", DateTime.Now.ToLongTimeString(), DateTime.Now.Millisecond))
        Response.Write(re)
        Response.End()
    End Sub

  
    


   
    Protected Sub lnk_view_5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_view_5.Click
        multi_view_main.SetActiveView(View6)
        lit_view_6.Text = ""
        view_6_export.Visible = False
        lit_view_6_h5.InnerHtml = "Click Retrieve button to view the records. Process can be time consuming..."
    End Sub


    Protected Sub lit_view_6_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lit_view_6_button.Click
        html_Additional_Point_Contracts()
    End Sub

    Protected Sub view_6_export_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles view_6_export.Click
        view_6_export.Visible = False

        If hf_iiMembershipEnrollmentID_upgrades.Value.Length > 0 Then

            Dim ar() = hf_iiMembershipEnrollmentID_upgrades.Value.Trim().Split(New Char() {" "c})

            Using cnn = New SqlConnection(Resources.Resource.cns)
                Using ada = New SqlDataAdapter
                    For Each s In ar
                        Dim sq = String.Format("select * from t_IIMembershipEnrollment where IIMembershipEnrollmentID in ({0})", s)

                        ada.SelectCommand = New SqlCommand(sq, cnn)
                        Dim cmd = New SqlCommandBuilder(ada)

                        Dim dt = New DataTable()
                        ada.Fill(dt)

                        If dt.Rows.Count = 1 Then
                            If dt.Rows(0)("dateexported").Equals(DBNull.Value) Then
                                dt.Rows(0)("dateexported") = DateTime.Now
                                dt.Rows(0)("exportstatusid") = 3

                                'ada.Update(dt)
                            End If
                        End If
                    Next

                End Using
            End Using

            html_export_excel(lit_view_6)

            hf_iiMembershipEnrollmentID_upgrades.Value = String.Empty
            lit_view_6_h5.InnerHtml = ""
        End If
    End Sub
End Class


