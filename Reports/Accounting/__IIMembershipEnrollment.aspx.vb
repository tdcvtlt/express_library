Imports System
Imports System.Data
Imports System.Data.SqlClient


Partial Class Reports_Accounting_IIMembershipEnrollment1
    Inherits System.Web.UI.Page

    Private CONTRACT_STATUSES As String() = {"16585", "16571", "16582", "17277"}

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim ignP As New Prospect(15143473)
        For Each a As StreetAddress In ignP.StreetAddresses
            'Response.Write(String.Format("<strong>{0}, <span>{1}</span></strong></br>", a.Street, a.Active))
        Next

        If Me.IsPostBack = False Then
            setCheckBoxList()
            AddPointsContracts()

            multi_view_main.SetActiveView(View1)

        End If
    End Sub


#Region "EVENT HANDLERS"

    Protected Sub lnk_btn_points_owners_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_btn_points_owners.Click
        lblError.Text = ""
        multi_view_main.ActiveViewIndex = 0
    End Sub

    Protected Sub lnk_btn_ii_report_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_btn_ii_report.Click
        lblError.Text = ""
        multi_view_main.ActiveViewIndex = 1
        litResult.Text = String.Empty
    End Sub

    Protected Sub lnk_btn_non_ii_report_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_btn_non_ii_report.Click

        multi_view_main.SetActiveView(View3)

        cbl_cxl.DataSource = New ContractCancelsStatus().Status
        cbl_cxl.DataTextField = "ComboItem"
        cbl_cxl.DataValueField = "ComboItemID"
        cbl_cxl.DataBind()

        Dim sb = New StringBuilder()
        Dim sql = String.Format( _
            "select p.prospectId, p.LastName, p.FirstName, c.ContractId, c.ContractNumber, c.ContractDate, c.AnniversaryDate from t_prospect p " & _
            "inner join t_contract c on p.prospectid = c.prospectid " & _
            "where p.prospectid in ( " & _
            "select distinct prospectid " & _
            "from t_contract where contractid in " & _
            "   (   " & _
            "       select contractid from t_IIMembershipEnrollment " & _
            "       where exportStatusId = 1 " & _
            "   ) " & _
            ") " & _
            "and c.contractid not in " & _
            "( " & _
            "   select contractid from t_IIMembershipEnrollment " & _
            "   where exportStatusId = 1 " & _
            ")  " & _
            "and c.statusid not in (select comboItemId from t_ComboItems where comboItem in ('pender', 'kick', 'pender-inv', 'res-pender', 'rescinded', 'void') and comboid = 236) " & _
            "and c.subtypeid in (select comboItemId from t_ComboItems where comboItem in ('points') and comboid = 398)  " & _
            "and p.prospectid <> 6199439")


        Using con = New SqlConnection(Resources.Resource.cns)
            Using ada = New SqlDataAdapter(sql, con)

                Dim dt = New DataTable()
                ada.Fill(dt)

                If dt.Rows.Count > 0 Then
                    sb.Append("<table>")
                End If

                Dim v = New InventoryView()
                Dim m_sb = New StringBuilder()

                For Each dr In dt.Rows

                    v.ContractID = dr("contractid").ToString()
                    v.GetInventories(con, v.ContractID)

                    sb.Append("<tr>")
                    sb.AppendFormat("<td>{0}</td>", dr("prospectid").ToString())
                    sb.AppendFormat("<td>{0}</td>", dr("lastname").ToString())
                    sb.AppendFormat("<td>{0}</td>", dr("firstname").ToString())
                    sb.AppendFormat("<td>{0}</td>", dr("contractnumber").ToString())

                    If v.Inventories.Rows.Count >= 1 Then
                        sb.AppendFormat("<td>{0}</td>", v.Resorts(0))
                        sb.AppendFormat("<td>{0}</td>", v.Inventories.Rows(0)("StreetAbb"))
                        sb.AppendFormat("<td>{0}</td>", v.Inventories.Rows(0)("Week"))
                    Else
                        sb.AppendFormat("<td>&nbsp;</td>")
                        sb.AppendFormat("<td>&nbsp;</td>")
                        sb.AppendFormat("<td>&nbsp;</td>")
                    End If

                    sb.AppendFormat("<td>{0}</td>", get_Frequency(v.FrequencyID))

                    If v.Inventories.Rows.Count >= 1 Then
                        sb.AppendFormat("<td>{0}</td>", get_G(DateTime.Parse(dr("contractdate").ToString()).ToString("MM/dd/yyyy"), _
                                                         String.Format("{0}-{1}", v.Resorts(0), v.Inventories.Rows(0)("size").ToString())))
                    Else
                        sb.AppendFormat("<td>{0}</td>", get_G(DateTime.Parse(dr("contractdate").ToString()).ToString("MM/dd/yyyy"), _
                                                  String.Format("{0}-{1}", "", "")))
                    End If

                    sb.AppendFormat("<td>{0}</td>", get_ClubUsage(dr("contractid").ToString()))
                    Try
                        sb.AppendFormat("<td>{0:MM/dd/yyyy}</td>", DateTime.Parse(dr("anniversarydate").ToString()))
                    Catch ex As Exception

                    End Try


                    sb.Append("</tr>")

                    If v.Inventories.Rows.Count > 1 Then

                        For i = 1 To v.Inventories.Rows.Count - 1
                            sb.Append("<tr>")
                            sb.AppendFormat("<td colspan=4>&nbsp;</td>")
                            sb.AppendFormat("<td>{0}</td>", v.Resorts(i))
                            sb.AppendFormat("<td>{0}</td>", v.Inventories.Rows(i)("StreetAbb"))
                            sb.AppendFormat("<td>{0}</td>", v.Inventories.Rows(i)("Week"))
                            sb.Append("<td colspan=4>&nbsp;</td")
                            sb.Append("</tr>")
                        Next
                    End If
                Next

                If dt.Rows.Count > 0 Then

                    sb.Append("</table>")

                    sb.Append("<br/>")
                End If
            End Using
        End Using

        ll_NonReport_0.Text = sb.ToString()
    End Sub

    Protected Sub lnk_btn_report_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_btn_report.Click
        multi_view_main.ActiveViewIndex = 3
    End Sub

    Protected Sub CheckToExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckToExport.Click

        If hf_iiMembershipEnrollmentID.Value.Length > 0 Then

            Dim ar() = hf_iiMembershipEnrollmentID.Value.Trim().Split(New Char() {" "c})

            'Dim sq = String.Format("select * from t_IIMembershipEnrollment where IIMembershipEnrollmentID in ({0})", _
            '                       String.Join(New Char() {","}, ar))

            '09/27
            'Operation got timed out so a temporary change of coding to rid of the error
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

            Dim re = litResult.Text
            litResult.Text = String.Empty

            Response.Clear()
            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", String.Format("attachment; filename=IIMebershipEnrollment_{0}_{1}.xls", DateTime.Now.ToLongTimeString(), DateTime.Now.Millisecond))
            Response.Write(re)
            Response.End()

            getUnits()

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
            btn_Retrieve_Click(Nothing, EventArgs.Empty)
        End If
    End Sub

    Protected Sub btn_Retrieve_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Retrieve.Click
        View_PointsOwners()
    End Sub

    Protected Sub btn_retrieve_report_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_retrieve_report.Click
        litResult.Text = String.Empty
        getUnits()
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


        GetBecameCancelled()



        Dim sb = New StringBuilder()
        Dim arrList = New List(Of String)

        If cbl_cxl.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).Count() = 0 Then Return

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Dim sql = "select contractID from t_IIMembershipEnrollment"

            Using ada = New SqlDataAdapter(sql, cnn)
                Dim dt = New DataTable()

                ada.Fill(dt)

                For Each dr As DataRow In dt.Rows
                    Dim cc As CancelledContract = New CancelledContract(dr.Item(0).ToString(), StartDateCancel, EndDateCancel, _
                                                cbl_cxl.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).Select(Function(x) x.Text).ToArray())

                    If cc.ContractID > 0 Then

                        Dim status = New CurrentStatusContract(cnn, cc.ContractID).Status.ToUpper()

                        If Array.IndexOf(New String() {"ACTIVE", "REDEED", "SUSPENSE", "DEVELOPER", "Deed-in-lieu", "exit in-house", _
                                                       "in bankcruptcy", "in foreclosure", "inactive", _
                                                       "incoll-active", "incoll-developer", "reverter", "uncollectible"}, status) < 0 Then
                            arrList.Add(cc.ContractID)
                        End If
                    End If
                Next

                If arrList.Count > 0 Then

                    sb.Append("<table>")

                    Dim dt_result = New OwnerNonII(cnn, String.Join(",", arrList.ToArray())).Details

                    For Each dr As DataRow In dt_result.Rows

                        sb.Append("<tr>")

                        sb.AppendFormat("<td>{0}</td>", dr("prospectid").ToString())
                        sb.AppendFormat("<td>{0}</td>", dr("lastname").ToString())
                        sb.AppendFormat("<td>{0}</td>", dr("firstname").ToString())
                        sb.AppendFormat("<td>{0}</td>", dr("contractnumber").ToString())

                        Dim view = New InventoryView()

                        view.ContractID = dr("contractid").ToString()
                        view.GetInventories(cnn, view.ContractID)


                        'sb.AppendFormat("<td>{0}</td>", dr("resort").ToString())
                        'sb.AppendFormat("<td>{0}</td>", dr("unit").ToString())
                        'sb.AppendFormat("<td>{0}</td>", dr("week").ToString())
                        'sb.AppendFormat("<td>{0}</td>", dr("frequency").ToString())
                        sb.AppendFormat("<td>{0}</td>", dr("status").ToString())

                        sb.Append("</tr>")
                    Next
                    sb.Append("</table>")
                End If
            End Using
        End Using

        'll_NonII.Text = sb.ToString()
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


#Region "Properties"
    Public ReadOnly Property StartDateCancel As DateTime
        Get
            If String.IsNullOrEmpty(sdate.Selected_Date) Then
                Return DateTime.MaxValue
            Else
                Return DateTime.Parse(sdate.Selected_Date)
            End If
        End Get
    End Property

    Public ReadOnly Property EndDateCancel As DateTime
        Get
            If String.IsNullOrEmpty(edate.Selected_Date) Then
                Return DateTime.MaxValue
            Else
                Return DateTime.Parse(edate.Selected_Date)
            End If
        End Get
    End Property


#End Region

#Region "PRIVATE FUNCTIONS"

    Private Function GetBecameCancelled() As DataTable

        If cbl_cxl.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).Count() = 0 Or _
            String.IsNullOrEmpty(sdate.Selected_Date) Or String.IsNullOrEmpty(edate.Selected_Date) Then Return New DataTable()

        Dim cancels() = cbl_cxl.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).Select(Function(x) x.Text).ToArray()
        For i = 0 To cancels.Count() - 1
            cancels(i) = String.Format("'{0}'", cancels(i))
        Next

        Dim sb = New StringBuilder()
        Dim sql = String.Format("select (select comboItem from t_ComboItems where comboItemid = c.statusId) Status, * " & _
                                "from t_Contract c inner join t_IIMembershipEnrollment ii on c.contractid = ii.contractid " & _
                                "inner join t_Prospect p on p.ProspectId = c.ProspectId " & _
                                "where c.contractid in (select distinct keyvalue from t_event where keyfield = 'contractid' " & _
                                "and fieldname = 'statusid' and datecreated between '{0}' and '{1}' " & _
                                "and type = 'change' and oldvalue in ('active', 'suspense', 'redeed', 'developer', " & _
                                "'Deed-In-Lieu', 'Exit In-House', 'In Bankruptcy', 'In Foreclosure', 'Inactive', " & _
                                "'InColl-Active', 'InColl-Developer', 'Reverter', 'Uncollectible') " & _
                                "and newvalue in ({2})) and c.statusid in (select comboitemid from t_comboItems " & _
                                "where comboItem in ({2})) ", _
                                sdate.Selected_Date, edate.Selected_Date, String.Join(",", cancels))


        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using ada = New SqlDataAdapter(sql, cnn)

                Dim dt = New DataTable()
                ada.Fill(dt)

                If dt.Rows.Count > 0 Then
                    sb.Append("<table border=1>")

                    For Each dr As DataRow In dt.Rows

                        sb.Append("<tr>")

                        sb.AppendFormat("<td>{0}</td>", dr("prospectid").ToString())
                        sb.AppendFormat("<td>{0}</td>", dr("lastname").ToString())
                        sb.AppendFormat("<td>{0}</td>", dr("firstname").ToString())
                        sb.AppendFormat("<td>{0}</td>", dr("contractnumber").ToString())

                        Dim view = New InventoryView()

                        view.ContractID = dr("contractid").ToString()
                        view.GetInventories(cnn, view.ContractID)

                        If view.Inventories.Rows.Count > 0 Then
                            sb.AppendFormat("<td>{0}</td>", view.Resorts(0))
                            sb.AppendFormat("<td>{0}</td>", view.Inventories.Rows(0)("StreetAbb"))
                            sb.AppendFormat("<td>{0}</td>", view.Inventories.Rows(0)("Week"))
                            sb.AppendFormat("<td>{0}</td>", get_Frequency(view.FrequencyID))
                        Else
                            For i = 1 To 4
                                sb.AppendFormat("<td>&nbsp;</td>")
                            Next
                        End If
                        sb.AppendFormat("<td>{0}</td>", dr("status").ToString())

                        sb.Append("</tr>")
                    Next
                    sb.Append("</table>")

                Else

                    sb.AppendFormat("No contracts with status(es) <strong>{0}</strong> found.", _
                                    String.Join(", ", cbl_cxl.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).Select(Function(x) x.Text).ToArray()))
                End If

            End Using
        End Using


        ll_NonII.Text = sb.ToString()

        Return Nothing
    End Function

#Region "Revision 1"

#Region "Code 1"

    Private Function getUnits() As IList(Of Unit)

        Dim ts = DateTime.Now

        Dim ctx = New CrmsnetContext()
        ctx.AnniversaryCutDate = Me.AnniversayDate

        Dim OwnersList = New List(Of Owner)
        Dim l = New List(Of Unit)
        Dim owner_pr As Owner = Nothing, owner_co As Owner = Nothing

        Dim cnn = New SqlConnection(Resources.Resource.cns)
        Dim ada = New SqlDataAdapter()

        For Each dr As DataRow In ctx.GetOwners.Rows

            owner_pr = New Owner()

            Dim phones() As Phone = {New CellPhone(IIf(dr("MOBILEPHONE").Equals(DBNull.Value), String.Empty, dr("MOBILEPHONE").ToString())), _
                                    New WorkPhone(IIf(dr("WORKPHONE").Equals(DBNull.Value), String.Empty, dr("WORKPHONE").ToString())), _
                                    New HomePhone(IIf(dr("HOMEPHONE").Equals(DBNull.Value), String.Empty, dr("HOMEPHONE").ToString()))}

            owner_pr.ProspectId = dr.Item("ProspectID")
            owner_pr.FirstName = IIf(dr("FirstName").Equals(DBNull.Value), String.Empty, dr("FirstName").ToString())
            owner_pr.LastName = IIf(dr("LastName").Equals(DBNull.Value), String.Empty, dr("LastName").ToString())

            owner_pr.Phones = phones.ToList()

            owner_pr.Emails.Add(New Owner.EMail() With {.Address = IIf(dr("Email1").Equals(DBNull.Value), String.Empty, dr("Email1").ToString())})
            owner_pr.Emails.Add(New Owner.EMail() With {.Address = IIf(dr("Email2").Equals(DBNull.Value), String.Empty, dr("Email2").ToString())})

            If dr.Item("Primary").ToString() = "1" Then
                OwnersList.Add(owner_pr)
                owner_co = owner_pr

                owner_pr.iiMemberID = dr.Item("IIMembershipEnrollmentID").ToString()

                owner_pr.Contract = New Contract() With { _
                    .KCP = dr.Item("ContractNumber"), _
                    .ContractID = dr.Item("contractid"), _
                    .Anniversary = IIf(dr.Item("AnniversaryDate").Equals(DBNull.Value), DateTime.MaxValue, dr.Item("AnniversaryDate")), _
                    .ContractDate = dr.Item("ContractDate"), _
                    .MF = dr.Item("MF"), _
                    .Season = dr.Item("Season"), _
                    .WeekType = dr.Item("WeekType")}


                Dim dt = New DataTable()
                ada.SelectCommand = New SqlCommand(String.Format("select distinct Address1,  " & _
                                         "(select comboItem from t_ComboItems where comboItemID = pa.stateID) [State], " & _
                                         "(select comboItem from t_ComboItems where comboItemID = pa.countryID) [Country],  Address2, ActiveFlag, PostalCode, City	" & _
                                         "from t_prospectaddress pa where contractaddress = 1 and activeflag = 1 and prospectid in ({0})", _
                                         owner_pr.ProspectId), cnn)
                ada.Fill(dt)

                For Each r In dt.Rows
                    owner_pr.Addresses.Add(New Address _
                        With { _
                            .Address1 = IIf(r.Item("Address1").Equals(DBNull.Value), String.Empty, r.Item("Address1")), _
                            .Address2 = IIf(r.Item("Address2").Equals(DBNull.Value), String.Empty, r.Item("Address2")), _
                            .City = IIf(r.Item("City").Equals(DBNull.Value), String.Empty, r.Item("City")), _
                            .State = IIf(r.Item("State").Equals(DBNull.Value), String.Empty, r.Item("State")), _
                            .Zip = IIf(r.Item("PostalCode").Equals(DBNull.Value), String.Empty, r.Item("PostalCode")), _
                            .Country = IIf(r.Item("Country").Equals(DBNull.Value), String.Empty, r.Item("Country")) _
                        })
                Next

                owner_pr.View = New InventoryView()
                owner_pr.View.GetInventories(cnn, owner_pr.Contract.ContractID)

            Else
                owner_co.Add(owner_pr)
            End If
        Next

        lblError.Text = ""

        Dim te = DateTime.Now
        Dim diff As TimeSpan = te - ts

        If ctx.GetOwners.Rows.Count > 0 Then
            printHTML(OwnersList)
        Else
            lblError.Text = "No records."
        End If

        CheckToExport.Visible = IIf(ctx.GetOwners.Rows.Count > 0, True, False)
        Return l
    End Function


    Private Sub printHTML(ByVal OwnersList As List(Of Owner))

        Dim MAX_COOWNERS = OwnersList.Max(Function(x) x.CoOwners.Count)
        Dim MAX_ADDRESSES = OwnersList.Max(Function(x) x.Addresses.Count)


        Dim sb = New StringBuilder()
        sb.Append("<table border=1 style=border-collapse:collapse;>")
        sb.Append("<tr>")

        For Each h In New String() {"Anniversary Date", "Member Last Name", "Member First Name", "Owner #"}
            sb.AppendFormat("<td><strong>{0}</strong></td>", h.ToUpper())
        Next


        For Each h In New String() {"Resort Code", "Unit Size", "Unit #", "Unit Check-In", "Week #"}
            sb.AppendFormat("<td><strong>{0}</strong></td>", h.ToUpper())
        Next

        sb.AppendFormat("<td><strong>{0}</strong></td>", "SEASON")
        sb.AppendFormat("<td><strong>{0}</strong></td>", "FREQUENCY")
        sb.AppendFormat("<td><strong>{0}</strong></td>", "1ST CLUB USAGE")

        For i = 0 To MAX_COOWNERS - 1
            sb.AppendFormat("<td><strong>CO-OWNER {0}</strong></td>", i + 1)
        Next


        For Each h In New String() {"Email-1", "Email-2"}
            sb.AppendFormat("<td><strong>{0}</strong></td>", h.ToUpper())
        Next


        For Each h In New String() {"Cell Phone", "Work Phone", "Home Phone"}
            sb.AppendFormat("<td><strong>{0}</strong></td>", h.ToUpper())
        Next


        For i = 0 To MAX_ADDRESSES - 1
            For Each h In New String() {"Street Name", "City", "State", "Zip"}
                sb.AppendFormat("<td><strong>{0}</strong></td>", h.ToUpper())
            Next
        Next


        sb.AppendFormat("<td><strong>{0}</strong></td>", "II MEMBERSHIP #")
        sb.AppendFormat("<td><strong>{0}</strong></td>", "M&T AMOUNT")
        sb.AppendFormat("<td><strong>{0}</strong></td>", "M&T")

        sb.AppendFormat("<td><strong>{0}</strong></td>", "FLOAT/FIXED")

        sb.Append("</tr>")


        'Output data...
        '





        hf_iiMembershipEnrollmentID.Value = String.Empty

        Dim cnn = New SqlConnection(Resources.Resource.cns)

        For Each list_owner As Owner In OwnersList

            sb.Append("<tr>")

            '
            'store primary key into page's hidden field separated by a space
            hf_iiMembershipEnrollmentID.Value += String.Format("{0} ", list_owner.iiMemberID)

            sb.AppendFormat("<td>{0}</td>", list_owner.Contract.Anniversary.ToShortDateString())
            sb.AppendFormat("<td>{0}</td>", list_owner.LastName)
            sb.AppendFormat("<td>{0}</td>", list_owner.FirstName)
            sb.AppendFormat("<td>{0}</td>", list_owner.Contract.KCP)

            Dim view = list_owner.View
            'view.GetInventories(cnn, list_owner.Contract.ContractID)

            If view.Inventories.Rows.Count >= 1 Then
                sb.AppendFormat("<td>{0}</td>", view.Resorts(0))
                sb.AppendFormat("<td>{0}</td>", view.Inventories.Rows(0)("Size"))
                sb.AppendFormat("<td>{0}</td>", view.Inventories.Rows(0)("StreetAbb"))
                sb.AppendFormat("<td>{0}</td>", get_UnitCheckInDay(view.Inventories.Rows(0)("SoldInventoryID1").ToString()))
                sb.AppendFormat("<td>{0}</td>", view.Inventories.Rows(0)("Week"))

            Else
                sb.Append("<td>&nbsp;</td>")
                sb.Append("<td>&nbsp;</td>")
                sb.Append("<td>&nbsp;</td>")
                sb.Append("<td>&nbsp;</td>")
                sb.Append("<td>&nbsp;</td>")
            End If

            sb.AppendFormat("<td>{0}</td>", list_owner.Contract.Season)

            If view.Inventories.Rows.Count >= 1 Then
                sb.AppendFormat("<td>{0}</td>", get_Frequency(view.FrequencyID))
            Else
                sb.Append("<td>&nbsp;</td>")
            End If


            '
            'Get 1st Club Usage
            sb.AppendFormat("<td>{0}</td>", get_ClubUsage(list_owner.Contract.ContractID))


            '
            'Co-Owners...
            For i = 0 To MAX_COOWNERS - 1
                Try
                    Dim kcp_coOwner = list_owner.CoOwners(i)
                    sb.AppendFormat("<td>{0}</td>", String.Format("{0},{1}", kcp_coOwner.LastName, kcp_coOwner.FirstName))
                Catch ex As Exception
                    sb.Append("<td>&nbsp;</td>")
                End Try
            Next


            '
            'Emails...
            For Each email In list_owner.Emails
                sb.AppendFormat("<td>{0}</td>", email.Address)
            Next

            '
            'Phones...
            For Each phone In list_owner.Phones
                sb.AppendFormat("<td>{0}</td>", phone.Number.Trim())
            Next

            '
            'Addresses
            For i = 0 To MAX_ADDRESSES - 1
                Try

                    Dim kcp_address As Address = list_owner.Addresses(i)

                    sb.AppendFormat("<td>{0}</td>", kcp_address.Address1)
                    sb.AppendFormat("<td>{0}</td>", kcp_address.City)
                    sb.AppendFormat("<td>{0}</td>", kcp_address.State)
                    sb.AppendFormat("<td>{0}</td>", kcp_address.Zip)

                Catch ex As Exception
                    sb.Append("<td colspan=4>&nbsp;</td>")
                End Try
            Next

            sb.AppendFormat("<td>{0}</td>", list_owner.IIMembership.Trim())

            sb.AppendFormat("<td>{0}</td>", list_owner.Contract.MF)


            If view.Resorts.Count() > 0 Then
                sb.AppendFormat("<td>{0}</td>", _
                                get_G(list_owner.Contract.ContractDate, _
                                     String.Format("{0}-{1}", _
                                                view.Resorts(0), _
                                                view.Inventories.Rows(0)("Size").ToString())))
            Else
                sb.Append("<td>&nbsp;</td>")
            End If



            sb.AppendFormat("<td>{0}</td>", list_owner.Contract.WeekType)

            sb.Append("</tr>")



            '
            'Print unit on next row if there are more than 1 inventories
            '
            If view.Inventories.Rows.Count > 1 Then
                For i = 1 To view.Inventories.Rows.Count - 1

                    sb.Append("<tr>")
                    sb.Append("<td colspan=4>&nbsp;</td>")

                    sb.AppendFormat("<td>{0}</td>", view.Resorts(0))
                    sb.AppendFormat("<td>{0}</td>", view.Inventories.Rows(i)("Size"))
                    sb.AppendFormat("<td>{0}</td>", view.Inventories.Rows(i)("StreetAbb"))
                    sb.AppendFormat("<td>{0}</td>", get_UnitCheckInDay(view.Inventories.Rows(i)("SoldInventoryID1").ToString()))
                    sb.AppendFormat("<td>{0}</td>", view.Inventories.Rows(i)("Week"))

                    sb.Append("</tr>")
                Next
            End If
        Next


        sb.Append("</table>")
        litResult.Text = String.Format("{0}", sb.ToString())

    End Sub
#End Region


    Private Sub AddPointsContracts()

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


    Private ReadOnly Property NewContracts As DataTable
        Get
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
                    "order by p.prospectID", String.Join(",", getCheckBoxListItemValues))

            Using ada As New SqlDataAdapter(sq, New SqlConnection(Resources.Resource.cns))
                Dim dt = New DataTable()
                ada.Fill(dt)

                Return dt
            End Using
        End Get
    End Property

    Private Sub View_PointsOwners()

        Dim members = New DataTable()

        Dim col_contract_id = members.Columns.Add("ContractID", GetType(String))
        members.Columns.Add("ProspectID", GetType(String))
        members.Columns.Add("AnniversaryDate", GetType(String))
        members.Columns.Add("FirstName", GetType(String))
        members.Columns.Add("LastName", GetType(String))
        members.Columns.Add("KCP", GetType(String))
        members.Columns.Add("Status", GetType(String))

        members.PrimaryKey = New DataColumn() {col_contract_id}

        Dim dt = NewContracts

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

    End Sub

    Private Function get_G(ByVal dt As DateTime, Optional ByVal u_type As String = "") As String
        If dt.CompareTo(DateTime.Parse("11/18/2005")) < 0 Then
            Return "G4"
        ElseIf dt.CompareTo(DateTime.Parse("11/18/2005")) >= 0 And dt.CompareTo(DateTime.Parse("02/25/2008")) < 0 Then
            Return "G3"
        ElseIf dt.CompareTo(DateTime.Parse("02/25/2008")) >= 0 And dt.CompareTo(DateTime.Parse("12/14/2009")) < 0 Then
            If u_type.CompareTo("KCT-4BED") = 0 Then
                Return "G2"
            Else
                Return "G1"
            End If
        ElseIf dt.CompareTo(DateTime.Parse("12/14/2009")) >= 0 Then
            Return "G1"
        Else
            Return "N/A"
        End If
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

    Private Function get_UnitCheckInDay(ByVal id As String) As String
        Dim s = String.Empty

        Using cnn As New SqlConnection(Resources.Resource.cns)
            s = String.Format("select top 1 cck.comboitem [Check-In Date] " & _
                            "from t_soldinventory so " & _
                            "inner join t_salesinventory si on so.salesinventoryid = si.salesinventoryid " & _
                            "inner join t_unit u on u.unitid = si.unitid " & _
                            "inner join t_room r on r.unitid = u.unitid " & _
                            "left outer join t_ComboItems cck on cck.comboItemId = r.subTypeId " & _
                            "where so.soldinventoryid = {0}", id)
            Using cmd As New SqlCommand(s, cnn)
                cnn.Open()
                s = cmd.ExecuteScalar()
            End Using
        End Using
        Return IIf(String.IsNullOrEmpty(s), "N/A", s)
    End Function


#End Region

    Private Sub setCheckBoxList()

        checkBoxList1.DataSource = New ContractStatus().Statuses
        checkBoxList1.DataTextField = "ComboItem"
        checkBoxList1.DataValueField = "ComboItemID"
        checkBoxList1.DataBind()
    End Sub

    Private Function selectCheckBoxListItems() As Queue(Of String)
        Dim q As Queue(Of String) = New Queue(Of String)
        For Each li As ListItem In checkBoxList1.Items
            If li.Selected Then
                q.Enqueue(li.Value.ToString())
            End If
        Next
        Return q
    End Function





#End Region

#Region "Private Properties"

    Private ReadOnly Property AnniversayDate As String
        Get
            Return dtAnniversaryDate.Selected_Date
        End Get
    End Property

    Private ReadOnly Property getCheckBoxListItemValues As String()
        Get
            Return selectCheckBoxListItems().ToArray().Concat(CONTRACT_STATUSES).ToArray()
        End Get
    End Property
#End Region



#Region "Inner Classes"

    Public MustInherit Class Fullname
        Private last_name As String
        Private first_name As String


        Public Property LastName As String
            Get
                Return last_name
            End Get
            Set(ByVal value As String)
                last_name = value
            End Set
        End Property

        Public Property FirstName As String
            Get
                Return first_name
            End Get
            Set(ByVal value As String)
                first_name = value
            End Set
        End Property



    End Class


    Public Class Contract

        Private _owner As Owner
        Private _kcp As String
        Private _anniversary As DateTime
        Private _season As String
        Private _contractId As String
        Private _weekType As String

        Private _contractDate As String
        Private _MF As String

        Private _frequencyId As Integer
        Private _statusId As String

        Public Sub New()
        End Sub

     
        Public Property MF As String
            Get
                Return _MF
            End Get
            Set(ByVal value As String)
                _MF = Convert.ToDecimal(value).ToString("N2")
            End Set
        End Property
        Public Property ContractDate As String
            Get
                Return _contractDate
            End Get
            Set(ByVal value As String)
                _contractDate = value
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
        Public Property ContractID As String
            Get
                Return _contractId
            End Get
            Set(ByVal value As String)
                _contractId = value
            End Set
        End Property
        Public Property KCP As String
            Get
                Return _kcp
            End Get
            Set(ByVal value As String)
                _kcp = value
            End Set
        End Property
        Public Property Anniversary As DateTime
            Get
                Return _anniversary
            End Get
            Set(ByVal value As DateTime)
                _anniversary = value
            End Set
        End Property
        Public Property Season As String
            Get
                Return _season
            End Get
            Set(ByVal value As String)
                _season = value
            End Set
        End Property
        Public Property Primary As Owner
            Get
                Return _owner
            End Get
            Set(ByVal value As Owner)
                _owner = value
            End Set
        End Property
        Public Property FrequencyID As Integer
            Set(ByVal value As Integer)
                _frequencyId = value
            End Set
            Get
                Return _frequencyId
            End Get
        End Property
     
        Public Property Status As String
            Get
                Return _statusId
            End Get
            Set(ByVal value As String)
                _statusId = value
            End Set
        End Property
    End Class

    Public Class Unit
        Private _street As String
        Private _unitId As String
        Private _subType As String
        Private _type As String

        Public Property UnitID As String
            Get
                Return _unitId
            End Get
            Set(ByVal value As String)
                _unitId = value
            End Set
        End Property
        Public Property Street As String
            Get
                Return _street
            End Get
            Set(ByVal value As String)
                _street = value
            End Set
        End Property
        ''' <summary>
        ''' 1BD-DWN, 1BD-UP
        ''' </summary>    
        Public Property SubType As String
            Get
                Return _subType
            End Get
            Set(ByVal value As String)
                _subType = value
            End Set
        End Property
        ''' <summary>
        ''' Estate, Cottage, Towne
        ''' </summary>   
        Public Property Type As String
            Get
                Return _type
            End Get
            Set(ByVal value As String)
                _type = value
            End Set
        End Property
        ''' <summary>
        ''' KCP, KCE, or KCP
        ''' </summary>
        Public ReadOnly Property ResortCode As String
            Get
                If String.IsNullOrEmpty(_type) Then Return String.Empty

                Dim c As Char = _type.Substring(0, 1).Trim().ToUpper()
                If c.CompareTo("E"c) = 0 Then
                    Return "KCE"
                ElseIf c.CompareTo("T"c) = 0 Then
                    Return "KCT"
                ElseIf c.CompareTo("C"c) = 0 Then
                    Return "KCP"
                Else
                    Return "N/A"
                End If
            End Get
        End Property
        ''' <summary>
        ''' Unit number such as DD11B
        ''' </summary>
        Public ReadOnly Property StreetConcatenation As String
            Get
                Dim arr As String() = Street.Split(New Char() {" "c})
                Dim tmp As String = String.Empty
                For i As Integer = 1 To arr.Length - 1
                    tmp &= arr(i).Substring(0, 1)
                Next
                'append unit numeral to the end of the string
                tmp &= arr(0)
                Return tmp
            End Get
        End Property
    End Class

    Public Class SoldInventory
        Inherits Unit

        Private _soldInventoryId As String
        Private _size As String
        Private _week As Integer
        Private _frequencyId As Integer
        Private _usage As String


        Public Property Usage As String
            Get
                Return _usage
            End Get
            Set(ByVal value As String)
                _usage = value
            End Set
        End Property

        Public Property FrequencyID As Integer
            Get
                Return _frequencyId
            End Get
            Set(ByVal value As Integer)
                _frequencyId = value
            End Set
        End Property

        Public Property SoldInventoryID As String
            Get
                Return _soldInventoryId
            End Get
            Set(ByVal value As String)
                _soldInventoryId = value
            End Set
        End Property

        Public Property Size As String
            Get
                Dim tmp As Int16 = 0
                If Int16.TryParse(_size, tmp) Then
                    Return String.Format("{0}BED", tmp)
                Else
                    Return _size
                End If
            End Get
            Set(ByVal value As String)
                _size = value
            End Set
        End Property

        Public Property Week As Integer
            Get
                Return _week
            End Get
            Set(ByVal value As Integer)
                _week = value
            End Set
        End Property

       
    End Class

    Public Class Owner
        Inherits Fullname


        Private _prospectid As String
        Private _isPrimary As Boolean = False
        Private _addresses As IList(Of Address) = New List(Of Address)
        Private _emails As IList(Of EMail)
        Private _contract As Contract
        Private _coOwners As List(Of Owner)
        Private _phones As List(Of Phone)
        Private _iiMemberID As String

        Private _view As InventoryView


        Public Property iiMemberID As String
            Get
                Return _iiMemberID
            End Get
            Set(ByVal value As String)
                _iiMemberID = value
            End Set
        End Property

        Public Structure EMail
            Public Address As String
            Public Sub New(ByVal e As String)
                Me.Address = e
            End Sub
        End Structure


        Public Property Addresses As IList(Of Address)
            Get
                Return _addresses
            End Get
            Set(ByVal value As IList(Of Address))
                _addresses = value
            End Set
        End Property


        Public Property Emails As IList(Of EMail)
            Get
                Return _emails
            End Get
            Set(ByVal value As IList(Of EMail))
                _emails = value
            End Set
        End Property


        Public Sub Add(ByVal o As Owner)
            _coOwners.Add(o)
        End Sub

        Public ReadOnly Property CoOwners As List(Of Owner)
            Get
                Return _coOwners
            End Get
        End Property

        Public Property Contract As Contract
            Get
                Return _contract
            End Get
            Set(ByVal value As Contract)
                _contract = value
            End Set
        End Property

        Public Property ProspectId As String
            Get
                Return _prospectid
            End Get
            Set(ByVal value As String)
                _prospectid = value
            End Set
        End Property

        Public Property IsPrimary As Boolean
            Get
                Return _isPrimary
            End Get
            Set(ByVal value As Boolean)
                _isPrimary = value
            End Set
        End Property

        Public ReadOnly Property IIMembership As String
            Get
                Return New IIMembershipNumber(_prospectid).Membership
            End Get
        End Property

        Public Sub New()
            _phones = New List(Of Phone)
            _coOwners = New List(Of Owner)
            _emails = New List(Of EMail)

        End Sub


        Public Property Phones As List(Of Phone)
            Get
                Return _phones
            End Get
            Set(ByVal value As List(Of Phone))
                _phones = value
            End Set
        End Property

        Public Property View As InventoryView
            Get
                Return _view
            End Get
            Set(ByVal value As InventoryView)
                _view = value
            End Set
        End Property
    End Class
#Region "Phone Classes"

    Public MustInherit Class Phone
        Private n As String

        Public Property Number As String
            Get
                Return n
            End Get
            Set(ByVal value As String)
                n = value
            End Set
        End Property
    End Class

    Public Class WorkPhone
        Inherits Phone
        Public Sub New(ByVal num As String)
            MyBase.Number = num
        End Sub
    End Class

    Public Class HomePhone
        Inherits Phone

        Public Sub New(ByVal num As String)
            MyBase.Number = num
        End Sub
    End Class

    Public Class CellPhone
        Inherits Phone
        Public Sub New(ByVal num As String)
            MyBase.Number = num
        End Sub
    End Class

    Public Class OtherPhone
        Inherits Phone

        Public Sub New(ByVal number As String)
            MyBase.Number = number
        End Sub
    End Class


#End Region
#Region "Address Class"
    Public Class Address

        Private _address1 As String
        Private _address2 As String
        Private _city As String
        Private _state As String
        Private _zip As String
        Private _country As String

        Public Property Address1 As String
            Get
                Return _address1
            End Get
            Set(ByVal value As String)
                _address1 = value
            End Set
        End Property

        Public Property Address2 As String
            Get
                Return _address2
            End Get
            Set(ByVal value As String)
                _address2 = value
            End Set
        End Property

        Public Property City As String
            Get
                Return _city
            End Get
            Set(ByVal value As String)
                _city = value
            End Set
        End Property

        Public Property State As String
            Get
                Return _state
            End Get
            Set(ByVal value As String)
                _state = value
            End Set
        End Property

        Public Property Zip As String
            Get
                Return _zip
            End Get
            Set(ByVal value As String)
                _zip = value
            End Set
        End Property


        Public Property Country As String
            Get
                Return _country
            End Get
            Set(ByVal value As String)
                _country = value
            End Set
        End Property
    End Class

#End Region


    Public Class ContractStatus        

        Public ReadOnly Property Statuses As DataTable
            Get
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
            End Get
        End Property
    End Class


    Class ContractCancelsStatus        

        ReadOnly Property Status As DataTable
            Get
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
            End Get
        End Property
    End Class

    Class ContractsCancelled        

        Public Function GetCancels(ByVal s As DateTime, ByVal e As DateTime) As DataTable

            Using con = New SqlConnection(Resources.Resource.cns)

                Dim sql = String.Format( _
                    "select p.PROSPECTID, p.LASTNAME, p.FIRSTNAME, c.CONTRACTID, c.CONTRACTNUMBER, " & _
                    "(select comboItem from t_comboItems where comboitemid = c.statusid) STATUS " & _
                    "from t_contract c inner join t_prospect p on c.prospectid = p.prospectid " & _
                    "where contractid in ( select distinct(keyvalue) from t_IIMembershipEnrollment ii " & _
                    "inner join t_Event e on e.keyValue = ii.contractid where " & _
                    "convert(varchar(10), datecreated, 101) between '{0}' and'{1}' " & _
                    "and e.keyValue = 'contractid' and e.type = 'change' and e.fieldname = 'statusid' " & _
                    "and oldValue in ('active', 'suspense', 'redeed', 'developer') and " & _
                    "(newValue like 'cxl%' or newValue = 'canceled')", s, e)

                Using ada = New SqlDataAdapter(sql, con)

                    Dim dt_cancels = New DataTable()
                    ada.Fill(dt_cancels)

                    dt_cancels.Columns.Add("RESORT", GetType(String))
                    dt_cancels.Columns.Add("UNIT", GetType(String))
                    dt_cancels.Columns.Add("WEEK", GetType(String))
                    dt_cancels.Columns.Add("FREQUENCY", GetType(String))
                    dt_cancels.Columns.Add("STATUS", GetType(String))
                    dt_cancels.Columns.Add("NOTES", GetType(String))

                    For Each row As DataRow In dt_cancels.Rows

                        Dim list_so = New SoldInventoriesList(row.Item("PROSPECTID"))
                        row("RESORT") = list_so(0).ResortCode
                    Next

                    Return dt_cancels
                End Using
            End Using
        End Function
    End Class



    Public Class IIMembershipNumber        

        Private _prospect_id As String = String.Empty

        Public Sub New(ByVal prospectId As String)
            _prospect_id = prospectId
        End Sub

        Public ReadOnly Property Membership As String
            Get                
                Dim sql = String.Format("select top 1 UFValue from t_uf_value where ufid = 362 and keyvalue = {0} order by keyvalue desc ", _prospect_id)

                Using cnn = New SqlConnection(Resources.Resource.cns)
                    Using cmd = New SqlCommand(Sql, cnn)
                        cnn.Open()
                        sql = cmd.ExecuteScalar()
                    End Using
                End Using                                
                Return IIf(String.IsNullOrEmpty(sql), String.Empty, sql)
            End Get
        End Property
    End Class

    Public Class CrmsnetContext        

        Private _anniversaryCutDate As DateTime

        Public Sub New()
        End Sub

        Public ReadOnly Property GetOwners As DataTable
            Get
                Dim dt = New DataTable()

                If String.IsNullOrEmpty(AnniversaryCutDate) Then Return dt

                Using cnn = New SqlConnection(Resources.Resource.cns)
                    Dim sql = String.Format("select * from (SELECT * FROM V_IICOOWNERENROLLMENT " & _
                                                       "UNION " & _
                                                       "SELECT * FROM V_IIOWNERENROLLMENT) a " & _
                                                       "where AnniversaryDate <= Convert(Datetime, '{0}') " & _
                                                       "order by contractnumber, [PRIMARY] desc", AnniversaryCutDate)
                    Using ada = New SqlDataAdapter(sql, cnn)
                        ada.Fill(dt)
                    End Using
                End Using
                Return dt
            End Get
        End Property

        Public Property AnniversaryCutDate As DateTime
            Get
                Return _anniversaryCutDate
            End Get
            Set(ByVal value As DateTime)
                _anniversaryCutDate = value
            End Set
        End Property
    End Class


    Public Class SoldInventoriesList
        Inherits List(Of SoldInventory)

        Public Sub New(ByVal contractId As Int32)

            Dim con = New SqlConnection(Resources.Resource.cns)
            Dim sql = "select * from ufn_contractinventory(" & contractId & ")"

            Using ada As New SqlDataAdapter(sql, con)
                Dim dt As New DataTable()
                ada.Fill(dt)

                For Each dr As DataRow In dt.Rows
                    Dim so As New SoldInventory()

                    so.SoldInventoryID = IIf(dr.Item("SoldInventoryID1").Equals(DBNull.Value), 0, dr.Item("SoldInventoryID1"))
                    so.SubType = IIf(dr.Item("UnitSubType1").Equals(DBNull.Value), String.Empty, dr.Item("UnitSubType1"))
                    so.Type = IIf(dr.Item("UnitType1").Equals(DBNull.Value), String.Empty, dr.Item("UnitType1"))
                    so.Street = IIf(dr.Item("Name1").Equals(DBNull.Value), String.Empty, dr.Item("Name1"))
                    so.Size = IIf(dr.Item("Size").Equals(DBNull.Value), 0, dr.Item("Size"))
                    so.Week = IIf(dr.Item("Week").Equals(DBNull.Value), 0, dr.Item("Week"))
                    so.FrequencyID = IIf(dr.Item("FrequencyID").Equals(DBNull.Value), 0, dr.Item("FrequencyID"))
                    so.Usage = IIf(dr.Item("OccupancyYear").Equals(DBNull.Value), dr.Item("AnniversaryDate"), dr.Item("OccupancyYear"))

                    Add(so)

                    If dr.Item("SoldInventoryID2").Equals(DBNull.Value) = False Then
                        so = New SoldInventory()

                        so.SoldInventoryID = IIf(dr.Item("SoldInventoryID2").Equals(DBNull.Value), 0, dr.Item("SoldInventoryID2"))
                        so.SubType = IIf(dr.Item("UnitSubType2").Equals(DBNull.Value), String.Empty, dr.Item("UnitSubType2"))
                        so.Type = IIf(dr.Item("UnitType2").Equals(DBNull.Value), String.Empty, dr.Item("UnitType2"))
                        so.Street = IIf(dr.Item("Name2").Equals(DBNull.Value), String.Empty, dr.Item("Name2"))
                        so.Size = IIf(dr.Item("Size").Equals(DBNull.Value), 0, dr.Item("Size"))
                        so.Week = IIf(dr.Item("Week").Equals(DBNull.Value), 0, dr.Item("Week"))
                        so.FrequencyID = IIf(dr.Item("FrequencyID").Equals(DBNull.Value), 0, dr.Item("FrequencyID"))
                        so.Usage = IIf(dr.Item("OccupancyYear").Equals(DBNull.Value), dr.Item("AnniversaryDate"), dr.Item("OccupancyYear"))

                        Add(so)
                    End If

                    If dr.Item("SoldInventoryID3").Equals(DBNull.Value) = False Then
                        so = New SoldInventory()

                        so.SoldInventoryID = IIf(dr.Item("SoldInventoryID3").Equals(DBNull.Value), 0, dr.Item("SoldInventoryID3"))
                        so.SubType = IIf(dr.Item("UnitSubType3").Equals(DBNull.Value), String.Empty, dr.Item("UnitSubType3"))
                        so.Type = IIf(dr.Item("UnitType3").Equals(DBNull.Value), String.Empty, dr.Item("UnitType3"))
                        so.Street = IIf(dr.Item("Name3").Equals(DBNull.Value), String.Empty, dr.Item("Name3"))
                        so.Size = IIf(dr.Item("Size").Equals(DBNull.Value), 0, dr.Item("Size"))
                        so.Week = IIf(dr.Item("Week").Equals(DBNull.Value), 0, dr.Item("Week"))
                        so.FrequencyID = IIf(dr.Item("FrequencyID").Equals(DBNull.Value), 0, dr.Item("FrequencyID"))
                        so.Usage = IIf(dr.Item("OccupancyYear").Equals(DBNull.Value), dr.Item("AnniversaryDate"), dr.Item("OccupancyYear"))

                        Add(so)
                    End If
                Next
            End Using
        End Sub

    End Class


    


  


    Class CancelledContract

        Private _contractId As Int32 = -1

        Public Sub New(ByVal key As Int32, ByVal s As DateTime, ByVal e As DateTime, ByVal statuses As String())

            Dim sql = String.Format( _
                "select top 1 KeyValue from t_Event e " & _
                "where convert(varchar(10), datecreated, 101) between '{0}' and '{1}' " & _
                "and keyfield = 'contractid' " & _
                "and e.type = 'change' and e.fieldname = 'statusid' " & _
                "and e.oldValue in ('active', 'suspense', 'developer', 'redeed') " & _
                "and (e.newValue in ('{2}')) and keyvalue = {3} order by eventId", s.ToString("MM/dd/yyyy"), e.ToString("MM/dd/yyyy"), String.Join(",", statuses), key)

            Using cnn As New SqlConnection(Resources.Resource.cns)
                cnn.Open()
                Using cmd As New SqlCommand(sql, cnn)
                    _contractId = cmd.ExecuteScalar()
                End Using
                cnn.Close()
            End Using
        End Sub

        Public ReadOnly Property ContractID As Int32
            Get
                Return _contractId
            End Get
        End Property
    End Class

    Class CurrentStatusContract
        Private _status As String

        Public Sub New(ByVal cnn As SqlConnection, ByVal key As Int32)
            Dim sql = String.Format("select coalesce((select comboItem from t_ComboItems where comboItemId = c.statusid), '') STATUS " & _
                                              "from t_Contract c where contractid = {0}", key)
            Using cmd As New SqlCommand(sql, cnn)

                If cnn.State = ConnectionState.Closed Then
                    cnn.Open()
                End If

                _status = cmd.ExecuteScalar()

                If ConnectionState.Closed > 0 Then
                    cnn.Close()
                End If
            End Using
        End Sub

        Public ReadOnly Property Status As String
            Get
                Return _status
            End Get
        End Property
    End Class


    Class OwnerNonII

        Private _dt As DataTable = New DataTable()

        Public Sub New(ByVal cnn As SqlConnection, ByVal p_id As String)
            Dim sql = String.Format( _
                "select lastname, firstname, p.prospectid, c.contractid, c.contractnumber, c.anniversaryDate, " & _
                "(select comboItem from t_comboItems where comboitemid = c.statusid) STATUS, " & _
                "MaintenanceFeeAmount MF " & _
                "from t_prospect p inner join t_contract c on p.prospectid = c.prospectid " & _
                "where contractid in ({0})", p_id)

            Using ada As New SqlDataAdapter(sql, cnn)
                ada.Fill(_dt)

                '_dt.Columns.Add("resort", GetType(String))
                '_dt.Columns.Add("unit", GetType(String))
                '_dt.Columns.Add("week", GetType(String))
                '_dt.Columns.Add("frequency", GetType(String))

             


                '    For Each dr As DataRow In _dt.Rows
                '        Dim l_sold As List(Of SoldInventory) = New SoldInventoriesList(dr.Item("contractid").ToString())

                '        If l_sold.Count > 0 Then
                '            dr("resort") = l_sold(0).ResortCode

                '            Select Case l_sold(0).FrequencyID
                '                Case "1"
                '                    dr("frequency") = "Annual"
                '                Case "2"
                '                    dr("frequency") = "Biennial"
                '                Case "3"
                '                    dr("frequency") = "Triennial"
                '                Case Else
                '                    dr("frequency") = "N/A"
                '            End Select

                '            If l_sold.Count = 1 Then

                '                dr("week") = l_sold(0).Week
                '                dr("unit") = l_sold(0).StreetConcatenation
                '            Else
                '                Dim tmpArr As IEnumerable(Of String) = l_sold.Select(Function(x) x.StreetConcatenation.Substring(0, x.StreetConcatenation.Length - 2))

                '                If tmpArr.Distinct().Count() = 1 Then

                '                    dr("unit") = tmpArr(0)
                '                    dr("week") = l_sold.First().Week

                '                    For Each s In l_sold.Select(Function(x) x.StreetConcatenation.Substring(x.StreetConcatenation.Length - 2, 1))
                '                        dr("unit") &= s
                '                    Next
                '                Else

                '                    dr("week") = l_sold.First().Week
                '                    dr("unit") = String.Join(",", l_sold.Select(Function(x) x.StreetConcatenation).ToArray())
                '                End If
                '            End If
                '        End If
                '    Next
            End Using

        End Sub

        Public ReadOnly Property Details As DataTable
            Get
                Return _dt
            End Get
        End Property

    End Class


    Class InventoryView

        Private _contractNumber As String
        Private _frequencyId As String

        Private _contractId As String
        Private _resorts As String() = New String() {}
        Private _inventories As DataTable

        Public ReadOnly Property ContractNumber As String
            Get
                Return IIf(String.IsNullOrEmpty(_contractNumber), "N/A", _contractNumber)
            End Get
        End Property

        Public Property ContractID As String
            Get
                Return _contractId
            End Get
            Set(ByVal value As String)
                _contractId = value
            End Set
        End Property


        Public Sub GetInventories(ByVal cnn As SqlConnection, ByVal contract_id As String)

            Using ada As New SqlDataAdapter("select * from ufn_contractInventory(" & contract_id & ")", cnn)

                _inventories = New DataTable()
                ada.Fill(_inventories)

                _inventories.Columns.Add("StreetAbb", GetType(String))

                If _inventories.Rows.Count > 0 Then
                    _frequencyId = _inventories.AsEnumerable().Select(Function(x) x("FREQUENCYID").ToString).First()
                    _contractNumber = _inventories.AsEnumerable().Select(Function(x) x("CONTRACTNUMBER").ToString()).First()

                    For Each dr As DataRow In _inventories.Rows
                        Resort = dr("UNITTYPE1").ToString()
                        Dim suffix = String.Empty

                        If dr("UNITTYPE1").ToString().ToUpper().Equals("ESTATES") Then

                            If dr("NAME3").Equals(DBNull.Value) = False Then
                                Dim ar() = dr("NAME3").ToString().Split(New Char() {" "c})
                                suffix = ar(0).Substring(ar(0).Length - 1, 1)
                            End If
                            If dr("NAME2").Equals(DBNull.Value) = False Then
                                Dim ar() = dr("NAME2").ToString().Split(New Char() {" "c})
                                suffix = ar(0).Substring(ar(0).Length - 1, 1) & suffix
                            End If

                            Dim parts() = dr("NAME1").ToString().Split(New Char() {" "c})
                            Dim street = String.Empty

                            For i = 1 To parts.Length - 1
                                street &= parts(i).Substring(0, 1)
                            Next

                            street &= parts(0) & suffix
                            dr("StreetAbb") = street

                        ElseIf dr("UNITTYPE1").ToString().ToUpper().Equals("COTTAGE") Then
                            Dim parts() = dr("NAME1").ToString().Split(New Char() {" "c})
                            Dim street = String.Empty

                            For i = 1 To parts.Length - 1
                                street &= parts(i).Substring(0, 1)
                            Next

                            street &= parts(0)
                            dr("StreetAbb") = street

                        ElseIf dr("UNITTYPE1").ToString().ToUpper().Equals("TOWNES") Then

                            If dr("NAME3").Equals(DBNull.Value) = False Then
                                Dim ar() = dr("NAME3").ToString().Split(New Char() {" "c})
                                Dim tmp = String.Empty

                                For i = 1 To ar.Length - 1
                                    tmp &= ar(i).Substring(0, 1)
                                Next

                                suffix = tmp & ar(0).Substring(ar(0).Length - 1, 1)

                            End If

                            If dr("NAME2").Equals(DBNull.Value) = False Then
                                Dim ar() = dr("NAME2").ToString().Split(New Char() {" "c})
                                Dim tmp = String.Empty

                                For i = 1 To ar.Length - 1
                                    tmp &= ar(i).Substring(0, 1)
                                Next

                                If String.IsNullOrEmpty(suffix) Then
                                    suffix = tmp & ar(0)
                                Else
                                    suffix = tmp & ar(0) & "," & suffix
                                End If

                            End If

                            Dim parts() = dr("NAME1").ToString().Split(New Char() {" "c})
                            Dim street = String.Empty

                            For i = 1 To parts.Length - 1
                                street &= parts(i).Substring(0, 1)
                            Next

                            If String.IsNullOrEmpty(suffix) Then
                                suffix = street & parts(0)
                            Else
                                suffix = street & parts(0) & "," & suffix
                            End If

                            dr("StreetAbb") = suffix
                        End If
                    Next
                End If
            End Using
        End Sub

        Public ReadOnly Property Inventories As DataTable
            Get
                Return _inventories
            End Get
        End Property

        Private WriteOnly Property Resort As String
            Set(ByVal value As String)

                Dim s = _resorts.Length
                Array.Resize(_resorts, s + 1)

                If String.IsNullOrEmpty(value) Then _resorts(s) = String.Empty

                Dim c As Char = value.Substring(0, 1).Trim().ToUpper()

                If c.CompareTo("E"c) = 0 Then
                    _resorts(s) = "KCE"
                ElseIf c.CompareTo("T"c) = 0 Then
                    _resorts(s) = "KCT"
                ElseIf c.CompareTo("C"c) = 0 Then
                    _resorts(s) = "KCP"
                Else
                    _resorts(s) = "N/A"
                End If
            End Set
        End Property

        Public ReadOnly Property Resorts As String()
            Get
                Return _resorts
            End Get
        End Property

        Public Property FrequencyID As String
            Get
                Return _frequencyId
            End Get
            Set(ByVal value As String)
                _frequencyId = value
            End Set
        End Property

    End Class
#End Region
 






    Private Class Prospect


        Public Sub New(ProspectID As Int32)

            _prospectID = ProspectID

            Dim sql = String.Format("select * from t_Prospect where prospectid = {0}", _prospectID)

            Using cnn = New SqlConnection(Resources.Resource.cns)
                Using ada = New SqlDataAdapter(sql, cnn)
                    Dim dt = New DataTable()

                    ada.Fill(dt)

                    If dt.Rows.Count = 1 Then
                        _lastName = dt.Rows(0)("LastName")
                        _firstName = dt.Rows(0)("FirstName")

                        sql = String.Format("select addressID from t_ProspectAddress where prospectID = {0}", _prospectID)
                        ada.SelectCommand = New SqlCommand(sql, cnn)

                        dt.Clear()
                        ada.Fill(dt)

                        Dim l As New List(Of StreetAddress)
                        For Each d As DataRow In dt.Rows

                            Dim st As New StreetAddress(d.Field(Of Int32)("AddressID")) With {.Prospect = Me}
                            l.Add(st)
                        Next
                        _streetAddresses = New StreetAddresses(l.ToArray())
                    End If
                End Using
            End Using




        End Sub




        Private _prospectID As Int32
        Private _lastName As String
        Private _firstName As String

        Private _streetAddresses As StreetAddresses

        Public Property LastName As String
            Get
                Return _lastName
            End Get
            Set(value As String)
                _lastName = value
            End Set
        End Property

        Public Property FirstName As String
            Get
                Return _firstName
            End Get
            Set(value As String)
                _firstName = value
            End Set
        End Property

        Public ReadOnly Property StreetAddresses As StreetAddresses
            Get
                Return _streetAddresses
            End Get
        End Property


    End Class

    Private Class StreetAddress

        Public Sub New(AddressID As Int32, Optional ProspectID As Int32 = 0)

            Dim sql = String.Format("select * from t_ProspectAddress where (activeflag = 1 or activeflag = 0) and addressid = {0}", AddressID)

            Using cnn = New SqlConnection(Resources.Resource.cns)
                Using ada = New SqlDataAdapter(sql, cnn)

                    Dim dt = New DataTable()
                    ada.Fill(dt)

                    If dt.Rows.Count = 1 Then
                        _Street = dt.Rows(0)("Address1")
                        _AddressID = dt.Rows(0)("AddressID")
                        _Active = dt.Rows(0)("activeflag")

                        If ProspectID > 0 Then
                            _prospect = New Prospect(ProspectID)
                        End If
                    End If

                End Using
            End Using
        End Sub

        Private _prospect As Prospect
        Private _AddressID As Int32
        Private _Street As String
        Private _State As String
        Private _City As String
        Private _Zip As String
        Private _Active As Boolean

        Public Property Prospect As Prospect
            Get
                Return _prospect
            End Get
            Set(value As Prospect)
                _prospect = value
            End Set
        End Property

        Public ReadOnly Property AddressID As Int32
            Get
                Return _AddressID
            End Get
        End Property

        Public Property Street As String
            Get
                Return _Street
            End Get
            Set(value As String)
                _Street = value
            End Set
        End Property

        Public Property Active As Boolean
            Get
                Return _Active
            End Get
            Set(value As Boolean)
                _Active = value
            End Set
        End Property

    End Class


    Private Class StreetAddresses
        Implements IEnumerable


        Private _list As StreetAddress()

        Public Sub New(l As StreetAddress())
            _list = l
        End Sub

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return New MyI(_list)
        End Function

        Private Class MyI
            Implements IEnumerator

            Private position As Integer = -1
            Private list As StreetAddress()

            Public Sub New(l As StreetAddress())
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

    Protected Sub lnk_view_4_Click(sender As Object, e As System.EventArgs) Handles lnk_view_4.Click
        multi_view_main.SetActiveView(View5)

        Dim sql = String.Format("select * from t_reservations where checkindate between '10/01/2013' and '10/31/2013'")
        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using ada = New SqlDataAdapter(sql, cnn)

                Dim dt = New DataTable()
                ada.Fill(dt)

                gvCancells.DataSource = dt
                gvCancells.DataBind()
            End Using
        End Using

    End Sub

    

    Protected Sub get_current_time_Click(sender As Object, e As System.EventArgs) Handles get_current_time.Click

        show_current_time.Text = DateTime.Now.ToShortTimeString()
    End Sub

    Protected Sub gvCancells_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCancells.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            Using cnn = New SqlConnection(Resources.Resource.cns)
                Using ada = New SqlDataAdapter("select lastname + ' ' + firstname from t_prospect p where prospectid = " + e.Row.Cells(0).Text, cnn)

                    Dim lb As Label = DirectCast(e.Row.FindControl("lbl_balance"), Label)

                    Try
                        cnn.Open()
                        lb.Text = DirectCast(ada.SelectCommand.ExecuteScalar(), String)
                    Catch ex As Exception
                    Finally
                        cnn.Close()
                    End Try


                End Using
            End Using


        End If
    End Sub
End Class


