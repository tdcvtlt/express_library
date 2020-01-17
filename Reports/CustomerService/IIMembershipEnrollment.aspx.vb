Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data.Linq.Mapping
Imports System.Data.Linq
Imports System.Web.Services
Imports System.IO
Imports System.Web.Script.Serialization


Imports ClosedXML.Excel

Partial Class Reports_Accounting_IIMembershipEnrollment
    Inherits System.Web.UI.Page

    Private DEFAULT_STATUSES As String() = {"16585", "16571", "16582", "17277"}
    Private STATE_DIC As IDictionary(Of Int32, String)
    Private COUNTRY_DIC As IDictionary(Of Int32, String)
    Private STATUS_DIC As IDictionary(Of Int32, String)
    Private COLUMN_MAX As Integer

    Private CRMSNET_CONNECTION_STRING As String = Resources.Resource.cns


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lit_view_6_button.Attributes.Remove("AnniversayDateFromProspect")


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

        Dim sq = String.Format(
        "insert into t_iimembershipenrollment(contractid, prospectID) " &
        "select contractid, prospectID " &
        "from t_contract c " &
        "inner join t_comboitems cst on c.subtypeid = cst.comboitemid " &
        "inner join t_comboitems cs on c.statusid = cs.comboitemid " &
        "where cst.comboitem in ('POINTS') " &
        "and cs.comboitem not in ('PENDER', 'KICK', 'PENDER-INV', 'PENDING REDEED', 'RES-PENDER', 'RESCINDED', 'VOID') " &
        "and c.contractid not in " &
        "( select contractid from t_IIMembershipEnrollment) ")

        Using con = New SqlConnection(Resources.Resource.cns)
            Using cmd = New SqlCommand(sq, con)
                Try
                    con.Open()
                    Dim recordsAff = cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Response.Write(String.Format("<br/><strong>{0}</strong>", ex.Message))
                Finally
                    con.Close()
                End Try
            End Using
        End Using
    End Sub

    Private Function get_Default_Contract_Statuses() As DataTable
        Dim sql = String.Format("select * from t_combos ct inner join t_comboitems " & _
                                                  "ci on ct.comboid = ci.comboid where " & _
                                                  "ct.comboname = 'contractstatus' and ci.comboItem not in ({0}) and ci.active = 1 order by ComboItem", _
                                                  String.Join(",", (New String() {"'Suspense'", "'Active'", "'Redeed'", "'Developer', 'PENDER', 'KICK', 'PENDER-INV','PENDING REDEED','RES-PENDER','RESCINDED','VOID'"})))
        Dim dt = New DataTable()
        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using ada As New SqlDataAdapter(sql, cnn)
                ada.Fill(dt)
            End Using
        End Using
        Return dt
    End Function

    Private Sub load_UI()
        Using con As New SqlConnection(Resources.Resource.cns)
            Dim sql = String.Format( _
                "select * from t_combos c inner join t_comboitems ci on c.comboId = ci.comboId " & _
                "where c.comboname = 'contractstatus' and (comboItem like 'cxl%' or comboitem = 'canceled' or comboitem='Rescinded') and ci.active = 1 order by comboItem;" & _
                "select * from t_combos c inner join t_comboitems ci on c.comboid = ci.comboid " & _
                "where c.comboname = 'contractsubstatus' and ci.active = 1 order by comboitem")

            Using ada As New SqlDataAdapter(sql, con)
                Dim ds = New DataSet()
                ada.Fill(ds)

                With cbl_cxl
                    .DataSource = ds.Tables(1)
                    .DataTextField = "ComboItem"
                    .DataValueField = "ComboItemID"
                    .DataBind()
                End With

                With cxl_sub_status
                    .Items.Add(New ListItem("NONE", 0))
                    .AppendDataBoundItems = True
                    .DataSource = ds.Tables(2)
                    .DataTextField = "comboitem"
                    .DataValueField = "comboitemid"
                    .DataBind()
                End With
            End Using

        End Using
    End Sub




    ''' <summary>
    ''' Get points contracts new owners just purchased, not upgrades or additionals
    ''' </summary> 
    Private Sub get_points_contracts()

        Dim selected = checkBoxList1.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).Select(Function(x) x.Value)

        Dim sqlText = String.Format( _
            "select distinct p.prospectid, c.AnniversaryDate, c.ContractDate, p.FirstName, p.LastName, c.ContractNumber, c.ContractID, " & _
            "(select comboitem from t_ComboItems where comboitemid = c.statusid) Status " & _
            "from t_Contract c inner join t_Prospect p on c.ProspectID = p.prospectid " & _
            "where c.contractid not in (select contractid from t_iiMembershipEnrollment where dateExported is not null and exportstatusid = 1) " & _
            "and c.statusid in ({0}) and c.subtypeid = 18179  " & _
            "and c.prospectid not in  ( " & _
            "   select distinct c.prospectid from t_contract c inner join t_IIMembershipEnrollment ii on ii.ContractID = c.ContractID  " & _
            "   where dateExported is not null and exportstatusid = 1 ) " & _
            "and (p.FirstName <> 'Resort' and p.LastName <>  'Finance') " & _
            "and (select comboitem from t_ComboItems where comboitemid = c.statusid) <> 'Redeed' " & _
            "order by c.ContractDate desc", String.Join(",", selected.Concat(DEFAULT_STATUSES).ToArray()))

        Using ada As New SqlDataAdapter(sqlText, New SqlConnection(Resources.Resource.cns))
            Dim dt = New DataTable()
            ada.Fill(dt)

            gvExport.DataSource = dt
            gvExport.DataBind()

            Submit.Visible = IIf(dt.Rows.Count > 0, True, False)
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

        hf_iiMembershipEnrollmentID.Value = String.Empty
        view2_h5.InnerHtml = String.Empty
        CheckToExport.Visible = False
    End Sub

    Protected Sub lnk_btn_non_ii_report_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_btn_non_ii_report.Click

        multi_view_main.SetActiveView(View3)
        load_UI()

        ll_NonII.Text = String.Empty
        btn_ExportCancelled_Contracts.Visible = False
        gv_02.DataSource = Nothing
        gv_02.DataBind()

    End Sub

    Protected Sub lnk_btn_report_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_btn_report.Click
        multi_view_main.ActiveViewIndex = 3
        mvView4.SetActiveView(v3View4)
        btnIIRatesView4.Visible = True
        btnBackView4.Visible = False
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

                                ada.Update(dt)
                            End If
                        End If
                    Next

                End Using
            End Using

            html_export_excel(gv_00)
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

            gvExport.DataSource = Nothing
            gvExport.DataBind()

            btn_Retrieve_Click(Nothing, EventArgs.Empty)

        End If
    End Sub

    Protected Sub btn_Retrieve_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Retrieve.Click
        get_points_contracts()
    End Sub

    Protected Sub btn_retrieve_report_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_retrieve_report.Click

        print_new_and_upgrades(get_New_Point_Contracts(), "new")
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

        Dim list = From li In cbl_cxl.Items.OfType(Of ListItem)() _
                   Where li.Selected = True _
                   Select li.Value

        Dim sub_status = cxl_sub_status.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).Select(Function(x) x.Value)
        Dim sql_append = ""
        If sub_status.Count > 0 Then sql_append = String.Format("and c.substatusid in ({0})", String.Join(",", sub_status.ToArray()))

        Dim sql = String.Format("select *, (select comboitem from t_comboitems where comboitemid = c.statusid) 'Cancels', (select comboitem from t_comboitems where comboitemid = c.substatusid) [sub-status] from t_contract c " & _
                                "inner join t_prospect p on c.prospectid = p.prospectid " & _
                "where c.contractid in (select contractid from t_iimembershipenrollment " & _
                "where exportstatusid = 1 and dateexported is not null) and statusdate between '{1}' and '{2}' " & _
                "and c.statusid in ({0}) " & sql_append, String.Join(",", list.ToArray()), sdate.Selected_Date, edate.Selected_Date)

        Dim _datatable As New DataTable()

        _datatable.Columns.Add("Prospect ID", GetType(String))
        _datatable.Columns.Add("Last Name", GetType(String))
        _datatable.Columns.Add("First Name", GetType(String))
        _datatable.Columns.Add("Contract Number", GetType(String))
        _datatable.Columns.Add("Contract Status", GetType(String))
        _datatable.Columns.Add("Contract SubStatus", GetType(String))
        _datatable.Columns.Add("Resort Code", GetType(String))
        _datatable.Columns.Add("Unit Size", GetType(String))
        _datatable.Columns.Add("Unit", GetType(String))
        _datatable.Columns.Add("Unit Check-In", GetType(String))
        _datatable.Columns.Add("Week", GetType(String))

        _datatable.Columns.Add("Frequency", GetType(String))

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
                                             .ContractID = r("ContractID").ToString(), _
                                            .SubStatus = r("sub-status").ToString})
                    Next

                    Dim dc = New DataContext(Resources.Resource.cns)
                    Dim tb_contract As Table(Of Contract) = dc.GetTable(Of Contract)()

                    For Each ii_owner As IIOwner In ii_owners.OrderBy(Function(x) x.LastName).ThenBy(Function(x) x.ProspectID)
                        Dim o = ii_owner
                        Dim ii_contracts As New List(Of Contract)

                        ii_contracts.Add(tb_contract.Single(Function(x) x.ContractID = o.ContractID))

                        For Each _contract As Contract In ii_contracts

                            Dim row = _datatable.NewRow()

                            row("Prospect ID") = o.ProspectID
                            row("Last Name") = o.LastName
                            row("First Name") = o.FirstName

                            row("Contract Number") = _contract.ContractNumber
                            row("Contract Status") = get_Contract_Status(_contract.Status)
                            row("Contract SubStatus") = o.SubStatus
                            row("Frequency") = get_Frequency(_contract.FrequencyID)

                            Dim _inventories As New Inventories(_contract.ContractID)

                            Dim _inventory_3 = _inventories.OfType(Of Inventory).Take(3)
                            Dim _inventories_first = _inventory_3.FirstOrDefault()
                            Dim _continued As Boolean = False

                            If _inventory_3.Count() > 0 Then

                                If _inventory_3.Select(Function(x) x.Week).Distinct().Count() = 1 Then                                    

                                    label_inventories(_inventory_3, 1, row)
                                Else

                                    label_inventories(_inventory_3.First(), row)
                                    _continued = True
                                End If
                            End If

                            'start new row
                            If _continued Then
                                For i = 1 To _inventory_3.Count() - 1                                    
                                    label_inventories(_inventory_3.ElementAt(i), row)
                                Next
                            End If

                            '
                            'and each line afterwards if still has more inventories...
                            If _inventories.OfType(Of Inventory).Count() > 3 Then

                                For i = 1 To _inventories.OfType(Of Inventory).Count() - 1

                                    _inventory_3 = _inventories.OfType(Of Inventory).Skip(i * 3).Take(3)

                                    If _inventory_3.Count() > 0 Then

                                        If _inventory_3.Select(Function(x) x.Week).Distinct().Count() = 1 Then
                                            label_inventories(_inventory_3, 0, row)
                                        Else

                                            For j = 0 To _inventory_3.Count() - 1
                                                label_inventories(_inventory_3.ElementAt(j), row)
                                            Next
                                        End If
                                    End If
                                Next
                            End If
                            _datatable.Rows.Add(row)
                        Next
                    Next

                    btn_ExportCancelled_Contracts.Visible = True
                    gv_02.DataSource = _datatable
                    gv_02.DataBind()

                    ll_NonII.Text = String.Empty
                    btn_ExportCancelled_Contracts.Visible = True

                Else
                    ll_NonII.Text = String.Format("<strong>No Records</strong>")
                    btn_ExportCancelled_Contracts.Visible = False
                End If
            End Using

        End Using
    End Sub


    Protected Sub btn_fin_submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_fin_submit.Click       
        Dim II_MEMBER, II_PAYBACK, II_RATE As Decimal

        If String.IsNullOrEmpty(dt_fin_end.Selected_Date) Or String.IsNullOrEmpty(dt_fin_start.Selected_Date) Then Return

        Dim dic = New Dictionary(Of String, String)
        Dim tblIIRate = New DataTable()

        Dim sql = String.Format("select c.frequencyid, c.contractid, c.prospectid, ii.IIMembershipEnrollmentId, coalesce(v.conversionid, 0) conversionid from t_contract c inner join t_IIMembershipEnrollment ii on c.contractid = ii.contractid left join t_conversion v on v.contractid = c.contractid where c.contractid in (select contractid from t_IIMembershipEnrollment where exportstatusid = 1 and dateExported between '{0}' and '{1}') group by c.frequencyid, c.contractid, c.prospectid, ii.iiMembershipEnrollmentId, v.ConversionID order by ii.IIMembershipEnrollmentId, c.prospectid, c.frequencyid", dt_fin_start.Selected_Date, dt_fin_end.Selected_Date)

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using ada = New SqlDataAdapter(sql, cnn)
                Dim dt = New DataTable()
                ada.Fill(dt)

                For Each row As DataRow In dt.Rows
                    If dic.ContainsKey(row("iiMembershipEnrollmentID").ToString()) = False Then
                        dic.Add(row("iiMembershipEnrollmentID").ToString(), _
                                String.Format("{0},{1},{2},{3}", _
                                              row("frequencyid").ToString(), _
                                              row("conversionid").ToString(), _
                                              row("prospectid").ToString(), _
                                              row("contractid").ToString()))
                        hfd_keys_billed.Value += String.Format("{0} ", row("iiMembershipEnrollmentID").ToString())
                    End If
                Next

                Using cm = cnn.CreateCommand()
                    cm.CommandText = String.Format("select iiMemberRateID, II_Membership [II Membership], II_Payback [II Payback], II_Rate [Reservation Fee], case frequency when 1 then 'Annual' when 2 then 'Biennial'	when 3 then 'Triennial' end [Frequency] from t_IIMembershipRate")
                    cm.CommandType = CommandType.Text
                    Try
                        cnn.Open()
                        tblIIRate.Load(cm.ExecuteReader())                       
                    Catch ex As Exception
                        Response.Write(ex.Message)
                    Finally
                        cnn.Close()
                    End Try
                End Using
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

                If tblIIRate.Rows.Count > 0 And s <> "TOTAL" Then
                    II_MEMBER = tblIIRate.Rows.OfType(Of DataRow).SingleOrDefault(Function(x) x("Frequency").ToString() = s)("II Membership").ToString()
                    II_PAYBACK = tblIIRate.Rows.OfType(Of DataRow).SingleOrDefault(Function(x) x("Frequency").ToString() = s)("II Payback").ToString()
                    II_RATE = tblIIRate.Rows.OfType(Of DataRow).SingleOrDefault(Function(x) x("Frequency").ToString() = s)("Reservation Fee").ToString()
                End If

                Select Case s
                    Case "Annual"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_a * II_MEMBER, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_a * II_PAYBACK, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_a * II_RATE, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", c_a)

                    Case "Biennial"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_b * II_MEMBER, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_b * II_PAYBACK, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_b * II_RATE, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", c_b)

                    Case "Triennial"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_t * II_MEMBER, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_t * II_PAYBACK, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_t * II_RATE, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", c_t)

                    Case "TOTAL"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round((c_a * II_MEMBER) + (c_b * II_MEMBER) + (c_t * II_MEMBER), 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round((c_a * II_PAYBACK) + (c_b * II_PAYBACK) + (c_t * II_PAYBACK), 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round((c_a * II_RATE) + (c_b * II_RATE) + (c_t * II_RATE), 2, MidpointRounding.AwayFromZero).ToString("N2"))
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
                If tblIIRate.Rows.Count > 0 And s <> "TOTAL" Then
                    II_MEMBER = tblIIRate.Rows.OfType(Of DataRow).SingleOrDefault(Function(x) x("Frequency").ToString() = s)("II Membership").ToString()
                    II_PAYBACK = tblIIRate.Rows.OfType(Of DataRow).SingleOrDefault(Function(x) x("Frequency").ToString() = s)("II Payback").ToString()
                    II_RATE = tblIIRate.Rows.OfType(Of DataRow).SingleOrDefault(Function(x) x("Frequency").ToString() = s)("Reservation Fee").ToString()
                End If

                Select Case s
                    Case "Annual"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_a * II_MEMBER, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_a * II_PAYBACK, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_a * II_RATE, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", c_a)

                    Case "Biennial"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_b * II_MEMBER, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_b * II_PAYBACK, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_b * II_RATE, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", c_b)

                    Case "Triennial"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_t * II_MEMBER, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_t * II_PAYBACK, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round(c_t * II_RATE, 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", c_t)

                    Case "TOTAL"

                        sb.AppendFormat("<td>{0}</td>", Decimal.Round((c_a * II_MEMBER) + (c_b * II_MEMBER) + (c_t * II_MEMBER), 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round((c_a * II_PAYBACK) + (c_b * II_PAYBACK) + (c_t * II_PAYBACK), 2, MidpointRounding.AwayFromZero).ToString("N2"))
                        sb.AppendFormat("<td>{0}</td>", Decimal.Round((c_a * II_RATE) + (c_b * II_RATE) + (c_t * II_RATE), 2, MidpointRounding.AwayFromZero).ToString("N2"))
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


    Private Function Get_Latest_Cancelled_Contract(prospectId As Int32) As String
        Dim contract_no = String.Empty

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Dim sql = String.Format("select top 1 c.contractNumber from t_contract c left join t_ComboItems cs on c.StatusID = cs.ComboItemID " & _
                                    "where ProspectID = {0} and cs.ComboItem = 'Canceled' order by c.ContractDate", prospectId)
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


        Return t()

        Dim res_table = New DataTable()

        res_table.Columns.Add("ContractID", GetType(Int32))
        res_table.Columns.Add("ProspectID", GetType(Int32))
        res_table.Columns.Add("FirstName", GetType(String))
        res_table.Columns.Add("LastName", GetType(String))


        Dim dt = New DataTable()
        Dim sql = String.Format("select * from t_contract c where c.contractid in " & _
                                "(select contractid from t_IImembershipEnrollment where " & _
                                "exportstatusid = 1 and dateexported is not null)")

        Dim y = 0



        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using ada = New SqlDataAdapter(sql, cnn)
                ada.Fill(dt)

                For Each g As IGrouping(Of String, DataRow) In dt.Rows.OfType(Of DataRow).GroupBy(Function(x) x("ProspectID").ToString())
                   
                    Dim c_date As DateTime = g.Max(Function(x) DateTime.Parse(x("ContractDate").ToString()))
                    Dim p_id As String = g.Key

                    sql = String.Format("select * from t_contract c inner join t_prospect p on c.prospectid = p.prospectid where c.subtypeid = 18179 " & _
                                        "and c.statusid not in (select comboitemid from t_comboitems where comboid = 236 and " & _
                                        "comboitem in ('pender', 'kick', 'pender-inv', 'pending-redeed', 'rescinded', 'void')) " & _
                                    "and convert(smalldatetime, contractdate) > '{0}' and p.prospectid = {1} and p.prospectid <> 6199439 " & _
                                    "and contractid not in (select contractid from t_iimembershipenrollment where exportstatusid in (1,3) and dateexported is not null)", c_date, p_id)



                    ada.SelectCommand = New SqlCommand(sql, cnn)
                    Dim tmp_table As New DataTable()

                    ada.Fill(tmp_table)

                    If p_id = 6962039 And y = 0 Then
                        'Response.Write(String.Format("<br/>{0}", sql))
                        y += 1
                    End If

                    For Each tmp_row As DataRow In tmp_table.Rows

                        Dim res_row = res_table.NewRow()

                        res_row("ContractID") = tmp_row("ContractID").ToString()
                        res_row("ProspectID") = tmp_row("ProspectID").ToString()
                        res_row("FirstName") = tmp_row("FirstName").ToString()
                        res_row("LastName") = tmp_row("LastName").ToString()

                        res_table.Rows.Add(res_row)

                    Next
                Next

                'Response.Write(String.Format("<br/>Rows Count: {0}", res_table.Rows.Count))
                'Response.Write(String.Format("<br/>{0}", String.Join(",", res_table.Rows.OfType(Of DataRow).Select(Function(x) x("contractid").ToString()).ToArray())))

                'res_table.Clear()
            End Using
        End Using

        Return res_table
    End Function

    Private Function t() As DataTable

        Dim dt = New DataTable()
        Dim sql = String.Format("select d.contractid, p.prospectid, p.lastName, p.firstName, p.AnniversaryDate " & _
                                "from ( " & _
                                "select contractid from t_contract c " & _
                                "where c.prospectid in ( " & _
                                "select c.prospectid from t_IIMembershipEnrollment ii inner join t_contract c on ii.contractid = c.contractid " & _
                                "where ii.dateExported is not null and ii.exportStatusId in (1, 3) " & _
                                "group by c.prospectid) " & _
                                "and c.subtypeid = (select comboitemid from t_comboitems a inner join t_combos b on a.ComboID = b.ComboID " & _
                                "where b.ComboName='ContractSubType' and a.ComboItem = 'points') and c.statusid in (select comboitemid from t_comboItems a inner join t_combos b on a.comboid = b.ComboID " & _
                                " where b.ComboName = 'contractstatus' and  " & _
                                "comboItem in ('active', 'redeed', 'suspense', 'developer')) and c.prospectid <> 6199439 " & _
                                "EXCEPT " & _
                                "select ii.contractid from t_iiMembershipEnrollment ii " & _
                                "where ii.dateExported is not null and ii.exportStatusId in (1,3)) d " & _
                                "inner join t_contract c on c.contractid = d.contractid " & _
                                "inner join t_prospect p on p.prospectid = c.prospectid")

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using ada = New SqlDataAdapter(sql, cnn)
                ada.Fill(dt)
            End Using
        End Using

        Return dt
    End Function


    ''' <summary>
    ''' Abbreviate unit types Townes, Estates, Cottage to KCT, KCE, KCP respectively.
    ''' </summary>
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


    ''' <summary>
    ''' This function is used when a group of 3 weeks are different from one another.
    ''' </summary>    
    Private Function get_Unit_Number(unitName As String) As String

        Dim names() As String = New String() {unitName}
        Return String.Format("{0}{1}", get_StreetName_Abbreviation(unitName), get_UnitNumber_Abbreviation(names))
    End Function

    ''' <summary>
    ''' This function is used when a group of 3 weeks are the same.
    ''' </summary> 
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
        If parts(1).ToString.ToUpper = "CAROUSEL" And tmp = "CC" Then tmp = "CCT"
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

   
    Private Function label_emails(emails As String(), row As DataRow) As DataRow

        row("Email 1") = emails(0)
        row("Email 2") = emails(1)

        Return row
    End Function

    Private Function label_phones(phones As String(), row As DataRow) As DataRow

        row("Cell Phone") = phones(0)
        row("Work Phone") = phones(1)
        row("Home Phone") = phones(2)
        Return row
    End Function

    Private Function label_addresses(ByVal addresses As List(Of Address), row As DataRow) As DataRow

        Try

            For i = 0 To addresses.OfType(Of Address).Count() - 1

                Dim a As Address = addresses.OfType(Of Address).ElementAt(i)

                row(String.Format("Street Name {0}", i + 1)) = a.Address1
                row(String.Format("City {0}", i + 1)) = a.City
                row(String.Format("State {0}", i + 1)) = get_State_Name(a.State)
                row(String.Format("Zip {0}", i + 1)) = a.Zip
                row(String.Format("Country {0}", i + 1)) = get_Country_Name(a.Country)
            Next
        Catch ex As Exception
            Response.Write(String.Format("<br/><strong>{0}</strong>", ex.Message))
        End Try
        Return row
    End Function


    Private Function label_inventories(_inventory As Inventory, row As DataRow) As DataRow

        row("Resort Code") = get_Resort_Code(_inventory.Type)
        row("Unit Size") = _inventory.Size
        row("Unit") = get_Unit_Number(_inventory.Name)
        row("Unit Check-In") = get_Unit_CheckInDay(_inventory.SoldInventoryID)
        row("Week") = _inventory.Week

        Return row
    End Function

    Private Function label_inventories(_inventories As IEnumerable(Of Inventory), time As Integer, row As DataRow) As DataRow

        Dim _inventories_first = _inventories.OfType(Of Inventory).FirstOrDefault()
        Dim names = From n In _inventories.OfType(Of Inventory)().Take(3) Select n.Name

        Dim size = From n In _inventories.OfType(Of Inventory)().Take(1) Where String.IsNullOrEmpty(n.Size) = False Select n.Size.Substring(0, 1)

        Dim unit_type = From n In _inventories.OfType(Of Inventory)().Take(1) Select n.Type

        Dim wk = From n In _inventories.OfType(Of Inventory)().Take(1) Select n.Week

        row("Resort Code") = get_Resort_Code(_inventories_first.Type)
        row("Unit Size") = _inventories_first.Size
        row("Unit") = get_Unit_Number(names.ToArray(), unit_type.FirstOrDefault(), size.Sum(Function(x) x))
        row("Unit Check-In") = get_Unit_CheckInDay(_inventories_first.SoldInventoryID)
        row("Week") = wk.FirstOrDefault()

        Return row
    End Function



#Region "Private functions"


    Private Function get_New_Point_Contracts() As List(Of IIOwner)

        Dim sb = New StringBuilder()
        Dim dc = New DataContext(Resources.Resource.cns)
        Dim tb_contract As Table(Of Contract) = dc.GetTable(Of Contract)()
        Dim ii_owners = New List(Of IIOwner)

        Dim sql = String.Format("select * from (SELECT * FROM V_IICOOWNERENROLLMENT " & _
                                                       "UNION " & _
                                                       "SELECT * FROM V_IIOWNERENROLLMENT) a " & _
                                                       "where AnniversaryDate <= convert(datetime, '{0}')  " & _
                                                       "order by contractnumber, [PRIMARY] desc ", dtAnniversaryDate.Selected_Date)

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using ada = New SqlDataAdapter(sql, cnn)

                Dim dt = New DataTable()
                ada.Fill(dt)

                For Each r As DataRow In dt.Rows.OfType(Of DataRow).Where(Function(x) x("Primary") = 1)
                    ii_owners.Add(New IIOwner() With {.FirstName = r("FirstName").ToString(), _
                   .LastName = r("LastName").ToString(), _
                   .ProspectID = r("ProspectID").ToString(), _
                   .ContractID = r("ContractID").ToString()})

                    hf_iiMembershipEnrollmentID.Value += String.Format("{0} ", r("IIMembershipEnrollmentID").ToString())
                Next
            End Using
        End Using

        __owners_ii = ii_owners

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

    Private Function get_Unit_CheckInDay(ByVal soldInventoryId As String) As String
        Dim s = String.Empty

        Using cnn As New SqlConnection(Resources.Resource.cns)
            s = String.Format("select top 1 ISNULL(cck.comboitem, '') [Check-In Date] " & _
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
                               .Week = Convert.ToInt16(row.Field(Of Integer)("Week")), _
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
                              .Week = Convert.ToInt16(row.Field(Of Integer)("Week")), _
                              .WeekType = row.Field(Of String)("WeekType"), _
                              .MF = row.Field(Of Decimal)("MF"), _
                              .SoldInventoryID = row.Field(Of Int32)("SoldInventoryID3")})

                    End If
                Next
            End Using
        End Using
        Return list.ToArray()
    End Function



    Private Sub get_Dictionaries()
        Using cnn = New SqlConnection(Resources.Resource.cns)
            ''countries
            Using ada = New SqlDataAdapter(String.Format("select * from t_comboItems where comboid = 239"), cnn)
                Dim dt = New DataTable()

                ada.Fill(dt)
                COUNTRY_DIC = New Dictionary(Of Int32, String)()
                For Each row In dt.Rows
                    COUNTRY_DIC.Add(Int32.Parse(row("ComboItemID").ToString()), row("ComboItem").ToString())
                Next

                dt.Clear()
                ''states...
                ada.SelectCommand = New SqlCommand(String.Format("select * from t_comboItems where comboid = 281"), cnn)
                ada.Fill(dt)
                STATE_DIC = New Dictionary(Of Int32, String)()
                For Each row In dt.Rows
                    STATE_DIC.Add(Int32.Parse(row("ComboItemID").ToString()), row("ComboItem").ToString())
                Next

                dt.Clear()
                ''contract statuses
                ada.SelectCommand = New SqlCommand(String.Format("select * from t_comboItems where comboid = 236"), cnn)
                ada.Fill(dt)

                STATUS_DIC = New Dictionary(Of Int32, String)()
                For Each row In dt.Rows
                    STATUS_DIC.Add(Int32.Parse(row("ComboItemID").ToString()), row("ComboItem").ToString())
                Next

                ada.Dispose()
                dt.Clear()
                dt.Dispose()

            End Using
        End Using
    End Sub

    Private Function get_State_Name(ByVal stateID As Int32) As String
        If STATE_DIC Is Nothing Then get_Dictionaries()
        If STATE_DIC.ContainsKey(stateID) Then
            Return STATE_DIC(stateID)
        Else
            Return String.Empty
        End If
    End Function

    Private Function get_Country_Name(ByVal countryID As Int32) As String
        If COUNTRY_DIC Is Nothing Then get_Dictionaries()
        If COUNTRY_DIC.ContainsKey(countryID) Then
            Return COUNTRY_DIC(countryID)
        Else
            Return String.Empty
        End If
    End Function

    Private Function get_Contract_Status(statusId As Int32) As String
        If STATUS_DIC Is Nothing Then get_Dictionaries()
        If STATUS_DIC.ContainsKey(statusId) Then
            Return STATUS_DIC(statusId)
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
        Private _substatus As String
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

        Public Property SubStatus As String
            Get
                Return _substatus
            End Get
            Set(value As String)
                _substatus = value
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
        Private _statusID As Int32
        Private _contractDate As DateTime


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
        Public Property ContractDate As DateTime?
            Get
                Return _contractDate
            End Get
            Set(ByVal value As DateTime?)
                _contractDate = value
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

        <System.Data.Linq.Mapping.Column(Name:="StatusID")> _
        Public Property Status As Int32
            Get
                Return _statusID
            End Get
            Set(ByVal value As Int32)
                _statusID = value
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



    Private __owners_ii As List(Of IIOwner)

    Private Property Owners_II As List(Of IIOwner)
        Get
            Return IIf(__owners_ii Is Nothing, New List(Of IIOwner), __owners_ii)
        End Get
        Set(value As List(Of IIOwner))
            __owners_ii = value
        End Set
    End Property

    Private Sub print_new_and_upgrades(ByVal ii_owners As List(Of IIOwner), ByVal reportType As String)

        view2_lbl_msg.InnerText = String.Empty

        If ii_owners.Count = 0 Then
            view2_lbl_msg.InnerText = "No Points contracts found."
            Return
        End If

        Dim dc = New DataContext(Resources.Resource.cns)
        Dim tb_contract As Table(Of Contract) = dc.GetTable(Of Contract)()

        Dim CO_MAX = ii_owners.Max(Function(x) x.CoOwners.Count())
        Dim ADDRESS_MAX = ii_owners.Max(Function(x) x.Addresses.OfType(Of Address).Count())


        Dim _datatable As New DataTable()

        _datatable.Columns.Add("Anniversary Date", GetType(String))
        _datatable.Columns.Add("ProspectID", GetType(String))
        _datatable.Columns.Add("Member Last Name", GetType(String))
        _datatable.Columns.Add("Member First Name", GetType(String))
        _datatable.Columns.Add("OWNER KCP", GetType(String))
        _datatable.Columns.Add("Resort Code", GetType(String))
        _datatable.Columns.Add("Unit Size", GetType(String))
        _datatable.Columns.Add("Unit", GetType(String))
        _datatable.Columns.Add("Unit Check-In", GetType(String))
        _datatable.Columns.Add("Week", GetType(String))
        _datatable.Columns.Add("Season", GetType(String))
        _datatable.Columns.Add("Frequency", GetType(String))
        _datatable.Columns.Add("1St Club Usage", GetType(String))
        _datatable.Columns.Add("Status", GetType(String))

        For i = 0 To CO_MAX - 1
            _datatable.Columns.Add(String.Format("Co-Owner {0}", i + 1), GetType(String))
        Next

        _datatable.Columns.Add("Email 1", GetType(String))
        _datatable.Columns.Add("Email 2", GetType(String))

        _datatable.Columns.Add("Cell Phone", GetType(String))
        _datatable.Columns.Add("Work Phone", GetType(String))
        _datatable.Columns.Add("Home Phone", GetType(String))

        For i = 0 To ADDRESS_MAX - 1
            _datatable.Columns.Add(String.Format("Street Name {0}", i + 1), GetType(String))
            _datatable.Columns.Add(String.Format("City {0}", i + 1), GetType(String))
            _datatable.Columns.Add(String.Format("State {0}", i + 1), GetType(String))
            _datatable.Columns.Add(String.Format("Zip {0}", i + 1), GetType(String))
            _datatable.Columns.Add(String.Format("Country {0}", i + 1), GetType(String))
        Next


        _datatable.Columns.Add("II Membership", GetType(String))
        _datatable.Columns.Add("M&T Amount", GetType(String))
        _datatable.Columns.Add("M&T", GetType(String))
        _datatable.Columns.Add("Float Or Fixed", GetType(String))
        _datatable.Columns.Add("Original KCP", GetType(String))
        _datatable.Columns.Add("Contract Date", GetType(String))
        

        For Each ii_owner As IIOwner In ii_owners.OrderBy(Function(x) x.LastName).ThenBy(Function(x) x.ProspectID)
            Dim o = ii_owner
            Dim ii_contracts As New List(Of Contract)

            ii_contracts.Add(tb_contract.Single(Function(x) x.ContractID = o.ContractID))

            For Each _contract As Contract In ii_contracts

                Dim row = _datatable.NewRow()

                If String.IsNullOrEmpty(lit_view_6_button.Attributes("AnniversayDateFromProspect")) = False Then

                    Using cn = New SqlConnection(Resources.Resource.cns)
                        Using cm = New SqlCommand(String.Format("select anniversaryDate from t_prospect where prospectid = {0}", o.ProspectID), cn)
                            Try
                                cn.Open()
                                Dim ad As DateTime
                                If DateTime.TryParse(cm.ExecuteScalar(), ad) Then
                                    row("Anniversary Date") = ad.ToShortDateString()
                                End If
                            Catch ex As Exception
                                Response.Write(ex.Message)
                            Finally
                                cn.Close()
                            End Try
                        End Using
                    End Using                   
                Else
                    If _contract.AnniversaryDate.HasValue Then
                        row("Anniversary Date") = _contract.AnniversaryDate.Value.ToShortDateString()
                    Else
                        row("Anniversary Date") = String.Empty
                    End If
                End If
               
                row("ProspectID") = o.ProspectID
                row("Member Last Name") = o.LastName
                row("Member First Name") = o.FirstName
                row("Owner KCP") = _contract.ContractNumber


                Dim _inventories As New Inventories(_contract.ContractID)

                Dim _inventory_3 = _inventories.OfType(Of Inventory).Take(3)
                Dim _inventories_first = _inventory_3.FirstOrDefault()
                Dim _continued As Boolean = False

                If _inventory_3.Count() > 0 Then
                    If _inventory_3.Select(Function(x) x.Week).Distinct().Count() = 1 Then
                        label_inventories(_inventory_3, 1, row)

                    Else
                        label_inventories(_inventory_3.First(), row)
                        _continued = True
                    End If
                End If

                row("Season") = get_Season(_contract.ContractID)
                row("Frequency") = get_Frequency(_contract.FrequencyID)
                row("1St Club Usage") = _contract.ClubUsage


                For i As Integer = 0 To CO_MAX - 1
                    Try                        
                        row(String.Format("Co-Owner {0}", i + 1)) = String.Format("{0}, {1}", ii_owner.CoOwners(i).LastName, ii_owner.CoOwners(i).FirstName)
                    Catch ex As Exception
                    End Try
                Next

 
                label_emails(New String() {ii_owner.Emails.OfType(Of Email).ElementAt(0).Address, _
                                            ii_owner.Emails.OfType(Of Email).ElementAt(1).Address}, row)

                label_phones(New String() {ii_owner.Phones.OfType(Of PhoneNumber).Single(Function(x) x.Type = "Mobile").Number, _
                                                                  ii_owner.Phones.OfType(Of PhoneNumber).Single(Function(x) x.Type = "Work").Number, _
                                                                  ii_owner.Phones.OfType(Of PhoneNumber).Single(Function(x) x.Type = "Home").Number}, row)

                label_addresses(ii_owner.Addresses.OfType(Of Address).ToList(), row)

                row("II Membership") = get_II_Membership(ii_owner.ProspectID)

                If _inventories_first IsNot Nothing Then
                    row("M&T") = get_G(_inventories_first.ContractDate, _
                                                              String.Format("{0}-{1}", _inventories_first.Type, _inventories_first.SubType))
                    row("M&T Amount") = _inventories_first.MF
                    row("Float Or Fixed") = _inventories_first.WeekType
                End If


                'row("Original KCP") = get_II_ContractNumber(o.ProspectID)

                row("Original KCP") = Get_Latest_Cancelled_Contract(o.ProspectID)

                If _contract.ContractDate.HasValue Then
                    row("Contract Date") = _contract.ContractDate.Value.ToShortDateString()
                End If
                row("Status") = (New clsComboItems).Lookup_ComboItem(_contract.Status)

                _datatable.Rows.Add(row)

                If _continued Then

                    For i = 1 To _inventory_3.Count() - 1
                        
                        row = _datatable.NewRow()
                        label_inventories(_inventory_3.ElementAt(i), row)

                        _datatable.Rows.Add(row)
                    Next
                End If


                If _inventories.OfType(Of Inventory).Count() > 3 Then

                    For i = 1 To _inventories.OfType(Of Inventory).Count() - 1

                        _inventory_3 = _inventories.OfType(Of Inventory).Skip(i * 3).Take(3)

                        If _inventory_3.Count() > 0 Then

                            If _inventory_3.Select(Function(x) x.Week).Distinct().Count() = 1 Then

                                row = _datatable.NewRow()
                                label_inventories(_inventory_3, 0, row)
                                _datatable.Rows.Add(row)

                            Else

                                For j = 0 To _inventory_3.Count() - 1                                    
                                    row = _datatable.NewRow()
                                    label_inventories(_inventory_3.ElementAt(j), row)
                                    _datatable.Rows.Add(row)
                                Next
                            End If
                        End If
                    Next
                End If

            Next
        Next

        If reportType = "new" Then

            If ii_owners.Count > 0 Then

                view2_h5.InnerHtml = "To open in Excel and send to II, click Export button."
                CheckToExport.Visible = True

                _datatable.Columns.Remove("Original KCP")
                _datatable.Columns.Remove("Contract Date")

                gv_00.DataSource = _datatable
                gv_00.DataBind()

            Else

            End If

        ElseIf reportType = "upgrade" Then

            If ii_owners.Count > 0 Then

                lit_view_6_h5.InnerHtml = "To open in Excel and send to II, click Export button."
                view_6_export.Visible = True

                gv_01.DataSource = _datatable
                gv_01.DataBind()
            Else

            End If
        End If
    End Sub

    Private Sub html_Additional_Point_Contracts()

        Dim dt = get_Additional_Point_Contracts()

        If dt.Rows.Count = 0 Then Return

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
        Owners_II = ii_owners
        print_new_and_upgrades(ii_owners, "upgrade")

    End Sub

    Private Sub html_export_excel(ByVal gv As GridView)

        Dim sb = New StringBuilder()
        sb.Append("<table>")

        For Each gvr As GridViewRow In gv.Rows
            sb.AppendFormat("<tr>")
            For Each c As TableCell In gvr.Cells
                sb.AppendFormat("<td>{0}</td>", c.Text)
            Next
            sb.AppendFormat("</tr>")
        Next

        sb.Append("</table>")

        gv.DataSource = Nothing
        gv.DataBind()

        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", String.Format("attachment; filename=IIMebershipEnrollment_additionals{0}_{1}.xls", DateTime.Now.ToLongTimeString(), DateTime.Now.Millisecond))
        Response.Write(sb.ToString())
        Response.End()
    End Sub

    Protected Sub lnk_view_5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_view_5.Click
        multi_view_main.SetActiveView(View6)        
        view_6_export.Visible = False
        lit_view_6_h5.InnerHtml = "Click Retrieve button to view the records. Process can be time consuming..."
        gv_01.DataSource = Nothing
        gv_01.DataBind()
    End Sub


    Protected Sub lit_view_6_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lit_view_6_button.Click
        lit_view_6_button.Attributes.Add("AnniversayDateFromProspect", "1")
        html_Additional_Point_Contracts()
    End Sub

    Protected Sub view_6_export_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles view_6_export.Click
        view_6_export.Visible = False

        Using cnn = New SqlConnection(Resources.Resource.cns)

            For Each gvr As GridViewRow In gv_01.Rows
                Dim cb = DirectCast(gvr.Cells(0).FindControl("asp_cb"), CheckBox)

                '' watch out for row that is just an inventory line 
                If cb.Checked = True And gvr.Cells(1).Text.Length > 0 Then

                    Dim sql = String.Format("select contractid from t_contract where contractnumber = '{0}'", gvr.Cells(4).Text)
                    Dim pk As Int32 = 0

                    Using cmd = New SqlCommand(sql, cnn)
                        Try
                            cnn.Open()
                            pk = cmd.ExecuteScalar()

                        Catch ex As Exception
                        Finally
                            cnn.Close()
                        End Try

                        Using ada = New SqlDataAdapter(String.Format("select top 1 * from t_IIMembershipEnrollment where contractid = {0}", pk), cnn)
                            Dim dt = New DataTable()
                            Dim cr As New SqlCommandBuilder(ada)
                            ada.Fill(dt)

                            If dt.Rows.Count = 1 Then
                                If dt.Rows(0)("dateexported").Equals(DBNull.Value) Then
                                    dt.Rows(0)("dateexported") = DateTime.Now
                                    dt.Rows(0)("exportstatusid") = 3

                                    ada.Update(dt)
                                End If
                            End If
                        End Using
                    End Using
                End If
            Next


            Dim sb = New StringBuilder()
            sb.Append("<table>")

            For Each row As GridViewRow In gv_01.Rows
                Dim cb = DirectCast(row.Cells(0).FindControl("asp_cb"), CheckBox)

                If cb.Checked = True Then
                    sb.AppendFormat("<tr>")
                    For Each c As TableCell In row.Cells
                        sb.AppendFormat("<td>{0}</td>", c.Text)
                    Next
                    sb.AppendFormat("</tr>")                    
                End If
            Next

            sb.Append("</table>")

            gv_01.DataSource = Nothing
            gv_01.DataBind()

            Response.Clear()
            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", String.Format("attachment; filename=IIMebershipEnrollment_additionals{0}_{1}.xls", DateTime.Now.ToLongTimeString(), DateTime.Now.Millisecond))
            Response.Write(sb.ToString())
            Response.End()

            hf_iiMembershipEnrollmentID_upgrades.Value = String.Empty
            lit_view_6_h5.InnerHtml = ""
        End Using

        
    End Sub
  
    Protected Sub gvExport_PreRender(sender As Object, e As System.EventArgs) Handles gvExport.PreRender
        
        Try
            With gvExport
                .UseAccessibleHeader = True
                .HeaderRow.TableSection = TableRowSection.TableHeader
                .HeaderRow.CssClass = "clickable"
                .HeaderRow.Cells(1).CssClass = "sort-date"
                .HeaderRow.Cells(3).CssClass = "sort-alpha"
                .HeaderRow.Cells(4).CssClass = "sort-alpha"
                .HeaderRow.Cells(5).CssClass = "sort-alpha"
            End With
        Catch ex As Exception           
        End Try        
    End Sub

   
    Protected Sub btnIIRatesView4_Click(sender As Object, e As EventArgs) Handles btnIIRatesView4.Click
        '//-- nested view
        btnIIRatesView4.Visible = False
        btnBackView4.Visible = True
        mvView4.SetActiveView(v1View4)
        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = cn.CreateCommand()
                cn.Open()
                cm.CommandText = String.Format("select iiMemberRateID, II_Membership [II Membership], II_Payback [II Payback], II_Rate [Reservation Fee], case frequency when 1 then 'Annual' when 2 then 'Biennial'	when 3 then 'Triennial' end [Frequency] from t_IIMembershipRate")
                cm.CommandType = CommandType.Text
                Dim dt = New DataTable()
                dt.Load(cm.ExecuteReader())
                gv1.DataSource = dt
                gv1.DataBind()
                cn.Close()
            End Using
        End Using
    End Sub

    Protected Sub gv1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv1.SelectedIndexChanged
        Try
            txtB1.Text = gv1.SelectedRow.Cells(2).Text
            txtB2.Text = gv1.SelectedRow.Cells(3).Text
            txtB3.Text = gv1.SelectedRow.Cells(4).Text

            btnSaveV2.Attributes.Remove("iiMemberRateID")
            btnSaveV2.Attributes.Add("iiMemberRateID", gv1.SelectedRow.Cells(1).Text)

            ddlFrequency.SelectedValue = gv1.SelectedRow.Cells(5).Text
            mvView4.SetActiveView(v2View4)
            btnBackView4.Visible = False
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btnBackV2_Click(sender As Object, e As EventArgs) Handles btnBackV2.Click
        btnIIRatesView4_Click(Nothing, e)
    End Sub

    Protected Sub btnSaveV2_Click(sender As Object, e As EventArgs) Handles btnSaveV2.Click
        Dim membeDec, paybackDec, feeDec As Decimal
        If Decimal.TryParse(txtB1.Text, membeDec) = False Or Decimal.TryParse(txtB2.Text, paybackDec) = False Or Decimal.TryParse(txtB3.Text, feeDec) = False Then
            btnIIRatesView4_Click(Nothing, e)
            Return
        End If

        With New clsIIMembershipRate()
            .IIMemberRateID = btnSaveV2.Attributes("iiMemberRateID")
            Try
                .Load()
                .II_Membership = txtB1.Text.Trim()
                .II_Payback = txtB2.Text.Trim()
                .II_Rate = txtB3.Text.Trim()
                .Save()
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try
        End With
        btnIIRatesView4_Click(Nothing, e)
    End Sub

    Protected Sub btnFinancialsV2_Click(sender As Object, e As EventArgs) Handles btnFinancialsV2.Click        
        lnk_btn_report_Click(sender, e)
    End Sub

    Protected Sub btnBackView4_Click(sender As Object, e As EventArgs) Handles btnBackView4.Click
        lnk_btn_report_Click(sender, e)
    End Sub

    Protected Sub btRenewalListUpload_Click(sender As Object, e As System.EventArgs) Handles btRenewalListUpload.Click
        If fuRenewalList.FileName.Length = 0 Then Return

        Dim parentPath = "\\nndc\UploadedContracts\", folder = "misprojects\", fileName = String.Empty
        fileName = parentPath + folder + fuRenewalList.FileName.Trim()
        Try
            Dim i As Integer = 1
            While File.Exists(fileName)
                fileName = parentPath + folder + Replace(fuRenewalList.FileName.Trim(), ".csv", i & ".csv")
                i += 1
            End While
            fuRenewalList.SaveAs(fileName)
            If FileIO.FileSystem.FileExists(fileName) Then
                Dim newfile As String = Replace(fileName, ".csv", "") & "_new" & ".csv"
                Dim fs As StreamReader = File.OpenText(fileName)
                Dim f As StreamWriter = File.CreateText(newfile)
                Dim firstLine As Boolean = True
                While Not fs.EndOfStream
                    Dim line As String = fs.ReadLine
                    f.Write(IIf(Not firstLine, vbCrLf & line, line))
                    firstLine = False
                End While
                f.Close()
                fs.Close()
                fileName = newfile

                Dim dt = New DataTable
                Using cn = New SqlConnection(Resources.Resource.cns)
                    Using cm = New SqlCommand("sp_IIBulkUpdate", cn)
                        cm.CommandTimeout = 0
                        cm.CommandType = CommandType.StoredProcedure
                        cm.Parameters.AddWithValue("@FileName", fileName)
                        Try
                            cn.Open()
                            Dim rd = cm.ExecuteReader()
                            If rd.HasRows Then
                                dt.Load(rd)
                                gvRenewalList.DataSource = dt
                                gvRenewalList.DataBind()
                            End If
                        Catch ex As Exception
                            Response.Write(ex.Message)
                        Finally
                            cn.Close()
                        End Try
                    End Using
                End Using
                File.Delete(fileName)
                File.Delete(Replace(fileName, "_new", ""))
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub


    

#Region "Event Handlers"
    
    Protected Sub btIIBulkUpdate_Click(sender As Object, e As System.EventArgs) Handles btIIBulkUpdate.Click        
        Dim dt = New DataTable
        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = New SqlCommand("sp_IIBulkUpdate", cn)
                cm.CommandType = CommandType.StoredProcedure
                cm.CommandTimeout = 0
                cm.Parameters.AddWithValue("@FileName", "\\rs-fs-01\g drive\MIS Shared\DNC\II.csv")
                Try
                    cn.Open()
                    Dim rd = cm.ExecuteReader()
                    If rd.HasRows Then
                        dt.Load(rd)
                        gvRenewalList.DataSource = dt
                        gvRenewalList.DataBind()
                    End If
                Catch ex As Exception
                    Response.Write(ex.Message)
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
    End Sub
    Protected Sub lnkRenewalListExcelUpload_Click(sender As Object, e As System.EventArgs) Handles lnkRenewalListExcelUpload.Click
        multi_view_main.SetActiveView(View8)
    End Sub
    Protected Sub gvRenewalList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvRenewalList.RowDataBound
        Dim kcp As String = e.Row.Cells(10).Text
        If e.Row.RowType = DataControlRowType.DataRow And kcp.ToLower() <> "owner number" Then
            
            For i = 0 To 4
                If e.Row.Cells(i).Text.ToLower().Replace(" ", "") <> e.Row.Cells(i + 12).Text.ToLower().Replace(" ", "") Then
                    e.Row.Cells(i).BackColor = Drawing.Color.Aqua
                    e.Row.Cells(i + 12).BackColor = Drawing.Color.Aqua
                Else
                    e.Row.Cells(i).BackColor = Drawing.Color.Beige
                    e.Row.Cells(i + 12).BackColor = Drawing.Color.Beige
                End If
            Next
            If e.Row.Cells(6).Text.ToLower().Replace(" ", "") <> e.Row.Cells(6 + 11).Text.ToLower().Replace(" ", "") Then
                e.Row.Cells(6).BackColor = Drawing.Color.Aqua
                e.Row.Cells(6 + 11).BackColor = Drawing.Color.Aqua
            Else
                e.Row.Cells(6).BackColor = Drawing.Color.Beige
                e.Row.Cells(6 + 11).BackColor = Drawing.Color.Beige
            End If
        End If

    End Sub
#End Region


    Protected Sub btnExportList_Click(sender As Object, e As EventArgs) Handles btnExportList.Click
        Generate_Report()
    End Sub

    Private Sub Loop_Records(ByRef dr As GridView, ByRef ws As IXLWorksheet)
        Dim row As Integer = 1
        Dim col As Integer = 1
        For Each cell In dr.HeaderRow.Cells
            ws.Cell(row, col).SetValue(cell.Text)
            ws.Cell(row, col).Style.Fill.SetBackgroundColor(IIf(cell.BackColor = Drawing.Color.Empty, XLColor.FromColor(Drawing.Color.White), XLColor.FromColor(cell.BackColor)))
            'ws.Cell(row, col).AddConditionalFormat().WhenNotBlank().Fill.SetBackgroundColor(IIf(cell.BackColor = Drawing.Color.Empty, XLColor.FromColor(Drawing.Color.White), XLColor.FromColor(cell.BackColor)))
            col += 1
        Next
        row += 1
        For Each gRow As GridViewRow In dr.Rows
            If gRow.Cells(0).Text <> "First Name" Then
                col = 1
                For Each cell As TableCell In gRow.Cells
                    ws.Cell(row, col).SetValue(Replace(cell.Text, "&nbsp;", ""))
                    ws.Cell(row, col).Style.Fill.SetBackgroundColor(IIf(cell.BackColor = Drawing.Color.Empty, XLColor.FromColor(Drawing.Color.White), XLColor.FromColor(cell.BackColor)))
                    'ws.Cell(row, col).AddConditionalFormat().WhenNotBlank().Fill.SetBackgroundColor(IIf(cell.BackColor = Drawing.Color.Empty, XLColor.FromColor(Drawing.Color.White), XLColor.FromColor(cell.BackColor)))
                    col += 1
                Next
                row += 1
            End If
        Next

    End Sub

    Private Sub Generate_Report()
        Dim res As HttpResponse = Response
        Dim xlWB As New XLWorkbook
        
        Loop_Records(gvRenewalList, xlWB.Worksheets.Add("Renewals"))

        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        res.AddHeader("content-disposition", "attachment;filename=""IIRenewalList.xlsx""")
        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()
    End Sub
    Protected Sub gvRenewalList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvRenewalList.SelectedIndexChanged

    End Sub
End Class


