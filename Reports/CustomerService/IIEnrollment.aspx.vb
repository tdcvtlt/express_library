Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Imports Resources.Resource

Partial Class Reports_CustomerService_IIEnrollment
    Inherits System.Web.UI.Page

    Private Sub Reports_CustomerService_IIEnrollment_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsPostBack = False Then
            With cblPointsStatuses
                .DataSource = MyPoints.ListDefaultStatuses()
                .DataTextField = "ComboItem"
                .DataValueField = "ComboItemID"
                .DataBind()
            End With
            With cblStatus
                Dim ds = New clsComboItems().Load_ComboItems("ContractStatus")
                Dim dv = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)

                .DataSource = dv.ToTable().AsEnumerable().Where(Function(x) (x("ComboItem").ToString() = "Canceled" Or x("ComboItem").ToString() = "Rescinded" Or x("ComboItem").ToString() = "CXL-Pender" Or x("ComboItem").ToString() = "Kick")).CopyToDataTable()
                .DataTextField = "ComboItem"
                .DataValueField = "ComboItemID"
                .DataBind()

            End With
            With cblSubStatus
                Dim ds = New clsComboItems().Load_ComboItems("ContractSubStatus")
                Dim dv = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)

                .Items.Add(New ListItem("NONE", 0))
                .AppendDataBoundItems = True
                .DataSource = dv.ToTable().AsEnumerable().OrderBy(Function(x) x("ComboItem").ToString()).CopyToDataTable()
                .DataTextField = "ComboItem"
                .DataValueField = "ComboItemID"
                .DataBind()
            End With
            With New IIEnrollment
                .Insert_New_Points_Contracts()
            End With
        End If
        lbInfo_View1.Text = String.Empty
        lbInfo_View2.Text = String.Empty
        lbInfo_View3.Text = String.Empty
        lbInfo_View5.Text = String.Empty
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
    End Sub
    Protected Sub lnkView1_Click(sender As Object, e As EventArgs) Handles lnkView1.Click
        MultiView1.SetActiveView(View1)
    End Sub

    Protected Sub lnkView2_Click(sender As Object, e As EventArgs) Handles lnkView2.Click
        MultiView1.SetActiveView(View2)
    End Sub

    Protected Sub lnkView3_Click(sender As Object, e As EventArgs) Handles lnkView3.Click
        MultiView1.SetActiveView(View3)
    End Sub

    Protected Sub lnkView4_Click(sender As Object, e As EventArgs) Handles lnkView4.Click
        MultiView1.SetActiveView(View4)
        mvView4.SetActiveView(v3View4)
        btnIIRatesView4.Visible = True
        btnBackView4.Visible = False
    End Sub

    Protected Sub lnkView5_Click(sender As Object, e As EventArgs) Handles lnkView5.Click
        MultiView1.SetActiveView(View5)
    End Sub

    Protected Sub lnkView6_Click(sender As Object, e As EventArgs) Handles lnkView6.Click
        MultiView1.SetActiveView(View6)
    End Sub

    Protected Sub btPointsRetrieve_Click(sender As Object, e As EventArgs) Handles btPointsRetrieve.Click
        Dim ar = cblPointsStatuses.Items.OfType(Of ListItem).
                                        Where(Function(x) x.Selected = True).Select(Function(x) x.Value).ToArray()
        Dim dt = New IIEnrollment().List_New_Points_Contracts(ar)
        If dt.Rows.Count > 0 Then
            gvPointsNew.DataSource = dt
            gvPointsNew.DataBind()
            btPointsSubmit.Enabled = True
        Else
            btPointsSubmit.Enabled = False
        End If
    End Sub

    Protected Sub btPointsSubmit_Click(sender As Object, e As EventArgs) Handles btPointsSubmit.Click
        Dim arr As New List(Of String)
        For Each dgv As GridViewRow In gvPointsNew.Rows
            Dim cb As CheckBox = DirectCast(dgv.Cells(0).FindControl("cb"), CheckBox)
            If cb.Checked Then
                arr.Add(gvPointsNew.DataKeys(dgv.RowIndex)("IIMembershipEnrollmentID").ToString())
            End If
        Next
        Dim counts = New IIEnrollment().Update_Points_Contracts_As_Added(arr.ToArray())
        lbInfo_View1.Text = String.Format("{0} Points contracts were added to the list", counts)
        If counts > 0 Then
            btPointsRetrieve_Click(Nothing, EventArgs.Empty)
        End If
    End Sub

    Protected Sub btIIMemberRetrieve_Click(sender As Object, e As EventArgs) Handles btIIMemberRetrieve.Click
        If String.IsNullOrEmpty(dtAnniversaryDate.Selected_Date) Then Return
        Dim l = New IIEnrollment().Retrieve_Most_Recent_Points_Contracts(dtAnniversaryDate.Selected_Date)
        If l.Count > 0 Then
            gvIIMembersNew.Enabled = True
            gvIIMembersNew.DataSource = New IIEnrollment().Show_Most_Recent_Points_Contracts(l)
            gvIIMembersNew.DataBind()
        Else
            lbInfo_View2.Text = "No new Points Contracts prior to " & dtAnniversaryDate.Selected_Date
        End If
        btIIMemberUpdateAndExport.Enabled = IIf(l.Count > 0, True, False)
    End Sub

    Protected Sub btIIMemberUpdateAndExport_Click(sender As Object, e As EventArgs) Handles btIIMemberUpdateAndExport.Click
        Dim arr As New List(Of String)
        For Each dgv As GridViewRow In gvIIMembersNew.Rows
            arr.Add(gvIIMembersNew.DataKeys(dgv.RowIndex)("IIMembershipEnrollmentID").ToString())
        Next
        With New IIEnrollment
            Dim counts = .Update_Brand_New_Points_Contracts_As_Exported(arr.ToArray())
            If counts > 0 Then
                btIIMemberUpdateAndExport.Enabled = False
                ExportTo_Excel(gvIIMembersNew)
                gvIIMembersNew.Enabled = False
            End If
        End With
    End Sub
    Protected Sub btIIMemberCancel_Click(sender As Object, e As EventArgs) Handles btIIMemberCancel.Click
        If sdate.Selected_Date.Length = 0 Or edate.Selected_Date.Length = 0 Then Return
        Dim cancels = From li In cblStatus.Items.OfType(Of ListItem)()
                      Where li.Selected = True
                      Select li.Value

        gvCancelsNew.DataSource = Nothing
        gvCancelsNew.DataBind()
        Dim subs = cblSubStatus.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True And x.Text <> "NONE").Select(Function(x) x.Value)
        Dim s = ""
        If subs.Count > 0 Then
            s = String.Format("and c.substatusid in ({0})", String.Join(",", subs.ToArray()))
        End If


        Dim l = New IIEnrollment().Retrieve_Cancelled_Points_Contracts(s, cancels.ToArray(), sdate.Selected_Date, edate.Selected_Date)
        If l.Count > 0 Then
            gvCancelsNew.DataSource = New IIEnrollment().Show_Cancelled_Points_Contracts(l)
            gvCancelsNew.DataBind()
            btIIMemberCancelUpdateAndExport.Enabled = True
        Else
            lbInfo_View3.Text = String.Format("No Points contracts became canceled during {0} and {1}", sdate.Selected_Date, edate.Selected_Date)
        End If
    End Sub
    Protected Sub btIIMemberCancelUpdateAndExport_Click(sender As Object, e As EventArgs) Handles btIIMemberCancelUpdateAndExport.Click
        Dim arr As New List(Of String)
        For Each dgv As GridViewRow In gvCancelsNew.Rows
            Dim cb As CheckBox = DirectCast(dgv.Cells(0).FindControl("cb"), CheckBox)
            If cb.Checked Then
                arr.Add(gvCancelsNew.DataKeys(dgv.RowIndex)("IIMembershipEnrollmentID").ToString())
                cb.Enabled = False
            End If
        Next
        If arr.Count > 0 Then
            With New IIEnrollment
                Dim counts = .Update_Points_Contracts_As_Cancelled(arr.ToArray())
                If counts > 0 Then
                    btIIMemberCancelUpdateAndExport.Enabled = False
                    gvCancelsNew.Columns(0).Visible = False
                    For Each dgv As GridViewRow In gvCancelsNew.Rows
                        Dim cb As CheckBox = DirectCast(dgv.Cells(0).FindControl("cb"), CheckBox)
                        If cb.Checked = False Then
                            dgv.Visible = False
                        End If
                    Next
                    ExportTo_Excel(gvCancelsNew)
                End If
            End With
        End If
    End Sub
    Protected Sub btIIMemberUpgrade_Click(sender As Object, e As EventArgs) Handles btIIMemberUpgrade.Click
        Dim l = New IIEnrollment().Retrieve_Upgraded_Points_Contracts
        gvIIMembersUpgrade.DataSource = Nothing
        gvIIMembersUpgrade.DataBind()
        If l.Count > 0 Then
            gvIIMembersUpgrade.DataSource = New IIEnrollment().Show_Upgraded_Points_Contracts(l)
            gvIIMembersUpgrade.DataBind()
            btIIMemberUpgradeAndExport.Enabled = True
        Else
            lbInfo_View5.Text = String.Format("No Points contracts became upgaded.")
        End If
    End Sub
    Protected Sub btIIMemberUpgradeAndExport_Click(sender As Object, e As EventArgs) Handles btIIMemberUpgradeAndExport.Click
        Dim arr As New List(Of String)
        For Each dgv As GridViewRow In gvIIMembersUpgrade.Rows
            Dim cb As CheckBox = DirectCast(dgv.Cells(0).FindControl("cb"), CheckBox)
            If cb.Checked Then
                arr.Add(gvIIMembersUpgrade.DataKeys(dgv.RowIndex)("IIMembershipEnrollmentID").ToString())
                cb.Enabled = False
            End If
        Next
        If arr.Count > 0 Then
            With New IIEnrollment
                Dim counts = .Update_Points_Contracts_As_Upgraded(arr.ToArray())
                If counts > 0 Then
                    gvIIMembersUpgrade.Columns(0).Visible = False
                    For Each dgv As GridViewRow In gvIIMembersUpgrade.Rows
                        Dim cb As CheckBox = DirectCast(dgv.Cells(0).FindControl("cb"), CheckBox)
                        If cb.Checked = False Then
                            dgv.Visible = False
                        End If
                    Next
                    btIIMemberCancelUpdateAndExport.Enabled = False
                    ExportTo_Excel(gvIIMembersUpgrade)
                End If
            End With
        End If
    End Sub

    Protected Sub btn_fin_submit_Click(sender As Object, e As EventArgs) Handles btn_fin_submit.Click
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
                        dic.Add(row("iiMembershipEnrollmentID").ToString(),
                                String.Format("{0},{1},{2},{3}",
                                              row("frequencyid").ToString(),
                                              row("conversionid").ToString(),
                                              row("prospectid").ToString(),
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

            sb.AppendFormat("<h3>{0} - {1} KCP SALES</h3>", DateTime.Parse(dt_fin_start.Selected_Date).ToString("MM/dd/yyyy"),
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

            sb.AppendFormat("<h3>{0} - {1} CONVERSIONS</h3>", DateTime.Parse(dt_fin_start.Selected_Date).ToString("MM/dd/yyyy"),
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

    Protected Sub btn_bill_Click(sender As Object, e As EventArgs) Handles btn_bill.Click
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
        lnkView4_Click(Nothing, EventArgs.Empty)
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

    Protected Sub btnBackView4_Click(sender As Object, e As EventArgs) Handles btnBackView4.Click
        lnkView4_Click(Nothing, EventArgs.Empty)
    End Sub
    Protected Sub btIIBulkUpdate_Click(sender As Object, e As EventArgs) Handles btIIBulkUpdate.Click
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
    Protected Sub btRenewalListUpload_Click(sender As Object, e As EventArgs) Handles btRenewalListUpload.Click

        If fuRenewalList.FileName.Length = 0 Then Return

        'Dim parentPath = "\\nndc\UploadedContracts\", folder = "misprojects\", fileName = String.Empty
        Dim parentPath = "\\SQL1\", folder = "Uploads\", fileName = String.Empty
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
                i = 1
                While Not fs.EndOfStream
                    Dim line As String = fs.ReadLine
                    If line.ToLower().Substring(0, 9).Equals("firstname") = False Then
                        f.Write(line & vbCrLf)
                    End If
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
                                'Generate_Report(rd)
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
    Private Sub Loop_Records(ByRef dr As SqlDataReader, ByRef ws As IXLWorksheet)
        Dim row As Integer = 1
        While dr.Read
            If row = 1 Then
                For col = 1 To dr.VisibleFieldCount
                    ws.Cell(row, col).SetValue(dr.GetName(col - 1))
                Next
                row += 1
            End If
            For col = 1 To dr.VisibleFieldCount
                If dr.GetName(col - 1) = "SSN1" Or dr.GetName(col - 1) = "SSN2" Or dr.GetName(col - 1) = "SSN3" Then
                    Dim val As String = dr.Item(col - 1) & ""
                    val = val.Replace("-", "")
                    If val.Length = 9 Then
                        val = val.Insert(5, "-").Insert(3, "-")
                    End If
                    ws.Cell(row, col).SetValue(val)
                Else
                    ws.Cell(row, col).SetValue(dr.Item(col - 1))
                End If
            Next
            row += 1

            GC.Collect()
        End While
        dr.Close()
    End Sub
    Private Sub Generate_Report(dr As SqlDataReader)
        Dim res As HttpResponse = Response
        Dim xlWB As New XLWorkbook
        Loop_Records(dr, xlWB.Worksheets.Add("Renewals"))

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
    Protected Sub gvPointsNew_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPointsNew.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Visible = False
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Visible = False
            e.Row.Cells(4).Text = DateTime.Parse(e.Row.Cells(4).Text).ToShortDateString()
        End If
    End Sub
    Protected Sub gvIIMembersNew_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvIIMembersNew.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Visible = False
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = False
        End If
    End Sub
    Protected Sub gvCancelsNew_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvCancelsNew.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Visible = False
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub
    Protected Sub gvIIMembersUpgrade_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvIIMembersUpgrade.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Visible = False
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub
    Protected Sub gvRenewalList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRenewalList.RowDataBound
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
                    If cell.Controls.Count > 0 Then
                        If cell.Controls(0).GetType() Is GetType(CheckBox) Then
                            ws.Cell(row, col).SetValue(CType(cell.Controls(0), CheckBox).Checked.ToString)
                        Else
                            ws.Cell(row, col).SetValue(cell.Text.ToString.Replace("&nbsp;", ""))
                        End If
                    Else
                        ws.Cell(row, col).SetValue(cell.Text.ToString.Replace("&nbsp;", ""))
                    End If
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

    Private Sub ExportTo_Excel(gv As GridView)
        Dim fn = String.Empty
        If gv.ID.ToLower() = "gvIIMembersNew".ToLower Then fn = "IiReport"
        If gv.ID.ToLower() = "gvCancelsNew".ToLower() Then fn = "CxlReport"
        If gv.ID.ToLower() = "gvIIMembersUpgrade".ToLower() Then fn = "UpgradesAndAdditionals"
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition",
        String.Format("attachment; filename=IIMebershipEnrollment_{2}({0}_{1}).xls", DateTime.Now.ToLongTimeString(), DateTime.Now.Millisecond, fn))
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        gv.AllowPaging = False
        gv.RenderControl(hw)

        'style to format numbers to string 
        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub

    Private Class MyPoints
        Public Shared Function ListDefaultStatuses() As DataTable
            Dim sql = String.Format("select * from t_combos ct inner join t_comboitems " &
                                               "ci on ct.comboid = ci.comboid where " &
                                               "ct.comboname = 'contractstatus' and ci.comboItem not in ({0}) and ci.active = 1 order by ComboItem",
                                               String.Join(",", (New String() {"'Suspense'", "'Active'", "'Redeed'", "'Developer', 'PENDER', 'KICK', 'PENDER-INV','PENDING REDEED','RES-PENDER','RESCINDED','VOID'"})))
            Dim dt = New DataTable()
            Using cnn = New SqlConnection(Resources.Resource.cns)
                Using ada As New SqlDataAdapter(sql, cnn)
                    ada.Fill(dt)
                End Using
            End Using
            Return dt
        End Function
    End Class

    Protected Sub btnExportList_Click(sender As Object, e As EventArgs) Handles btnExportList.Click
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
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
    End Sub

End Class

Public Class IIEnrollment

    Public Sub Insert_New_Points_Contracts()
        Dim sql = String.Format(
               "insert into t_iimembershipenrollment(ContractID, prospectID) " &
               "select contractid, ProspectID " &
               "from t_contract c " &
               "inner join t_comboitems cst on c.subtypeid = cst.comboitemid " &
               "inner join t_comboitems cs on c.statusid = cs.comboitemid " &
               "where cst.comboitem in ('POINTS') " &
               "and cs.comboitem not in ('PENDER', 'KICK', 'PENDER-INV', 'PENDING REDEED', 'RES-PENDER', 'RESCINDED', 'VOID') " &
               "and c.contractid not in " &
               "( select contractid from t_IIMembershipEnrollment) ")

        Using cn = New SqlConnection(cns)
            Using cmd = New SqlCommand(sql, cn)
                Try
                    cn.Open()
                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    cn.Close()
                    Throw ex
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
    End Sub
    Public Function List_New_Points_Contracts(statuses() As String) As DataTable
        Dim CONTRACT_STATUSES As String() = {"16585", "16571", "16582", "17277"}
        Dim sql = String.Format("select ii.IIMembershipEnrollmentID, p.prospectid [Prospect ID], CONVERT(VARCHAR(10), c.AnniversaryDate, 101) [Anniversary Date], CONVERT(VARCHAR(10), c.ContractDate, 101) [Contract Date], p.FirstName [First Name], p.LastName [Last Name], c.ContractNumber [Contract Number], c.ContractID [Contract ID],cs.ComboItem [Contract Status] " _
                & "from t_IIMembershipEnrollment ii inner join t_Contract c on ii.ContractID = c.ContractID inner join t_ComboItems cs on cs.ComboItemID = c.StatusID " _
                & "inner join t_ComboItems st on st.ComboItemID = c.SubTypeID inner join t_Prospect p on p.ProspectID = c.ProspectID " _
                & "where DateAdded Is null and DateExported is null and ExportStatusID is null and st.ComboItem = 'Points' " _
                & "and cs.ComboItemID in ({0}) and not (p.FirstName = 'Resort' and p.LastName = 'Finance') " _
                & "and c.ProspectID not in ( " _
                & "     	select distinct c.ProspectID from t_Contract c inner join t_IIMembershipEnrollment ii on c.ContractID = ii.ContractID	" _
                & "        where DateAdded Is not null and DateExported is not null and ExportStatusID = 1) order by p.ProspectID",
                String.Join(",", statuses.Concat(CONTRACT_STATUSES).ToArray()))

        'modified on 9/27/2018
        sql = String.Format("select ii.IIMembershipEnrollmentID, p.prospectid [Prospect ID], CONVERT(VARCHAR(10), c.AnniversaryDate, 101) [Anniversary Date], " _
            & "c.ContractDate [Contract Date], p.FirstName [First Name], p.LastName [Last Name], c.ContractNumber [Contract Number], c.ContractID [Contract ID],cs.ComboItem [Contract Status] " _
            & "from t_Contract c inner join t_IIMembershipEnrollment ii on ii.ContractID = c.ContractID inner join t_Prospect p on c.ProspectID = p.ProspectID inner join t_ComboItems cs on cs.ComboItemID = c.StatusID " _
            & "inner join t_ComboItems cst on cst.ComboItemID = c.SubTypeID where cst.ComboItem in ('Points') and cs.ComboItemID in ({0}) " _
            & "and c.ProspectID in (select c.ProspectID " _
            & "from t_Contract c inner join t_Prospect p on p.prospectid = c.ProspectID inner join t_ComboItems cs on cs.ComboItemID = c.StatusID " _
            & "inner join t_ComboItems cst on cst.ComboItemID = c.SubTypeID where c.SubTypeID = 18179 and not (p.firstname = 'resort' and p.lastname = 'finance') " _
            & "and c.ProspectID not in (select c.ProspectID from t_IIMembershipEnrollment ii inner join t_Contract c on ii.ContractID = c.ContractID " _
            & "where DateAdded is not null and DateExported is not null and ExportStatusID is not null) and c.statusid in ({0}) " _
            & "group by c.ProspectID having count(*) > 1) UNION ALL select ii.IIMembershipEnrollmentID, p.prospectid [Prospect ID], CONVERT(VARCHAR(10), c.AnniversaryDate, 101) [Anniversary Date], " _
            & "c.ContractDate [Contract Date], p.FirstName [First Name], p.LastName [Last Name], c.ContractNumber [Contract Number], c.ContractID [Contract ID],cs.ComboItem [Contract Status] " _
            & "from t_Contract c inner join t_IIMembershipEnrollment ii on ii.ContractID = c.ContractID inner join t_Prospect p on c.ProspectID = p.ProspectID inner join t_ComboItems cs on cs.ComboItemID = c.StatusID " _
            & "inner join t_ComboItems cst on cst.ComboItemID = c.SubTypeID where cst.ComboItem in ('Points') and cs.ComboItemID in ({0}) " _
            & "and c.ProspectID in (select c.ProspectID " _
            & "from t_Contract c inner join t_Prospect p on p.prospectid = c.ProspectID inner join t_ComboItems cs on cs.ComboItemID = c.StatusID " _
            & "inner join t_ComboItems cst on cst.ComboItemID = c.SubTypeID where c.SubTypeID = 18179 and not (p.firstname = 'resort' and p.lastname = 'finance') " _
            & "and c.ProspectID not in (select c.ProspectID from t_IIMembershipEnrollment ii inner join t_Contract c on ii.ContractID = c.ContractID " _
            & "where DateAdded is not null and DateExported is not null and ExportStatusID is not null) and c.statusid in ({0}) " _
            & "group by c.ProspectID having count(*) = 1) order by c.ContractDate desc, p.FirstName, p.LastName", String.Join(",", statuses.Concat(CONTRACT_STATUSES).ToArray()))

        Dim dt = New DataTable()
        Using cn = New SqlConnection(cns)
            Using ad As New SqlDataAdapter(sql, cn)
                Try
                    ad.Fill(dt)
                Catch ex As Exception
                    cn.Close()
                    Throw ex
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        Return dt
    End Function
    Public Function Update_Points_Contracts_As_Added(ByVal iiMembershipEnrollmentID() As String) As Int32
        Dim rows_count = 0
        Using cn = New SqlConnection(cns)
            Try
                cn.Open()
                For Each iiMembershipID As String In iiMembershipEnrollmentID
                    Using cm = New SqlCommand(String.Format("update t_IIMembershipEnrollment set ProspectID = (select c.ProspectID from t_Contract c inner join t_IIMembershipEnrollment ii on c.ContractID = ii.ContractID where ii.IIMembershipEnrollmentID = {0}), DateAdded = getdate() where IIMembershipEnrollmentID = {0}", iiMembershipID), cn)
                        rows_count += Convert.ToInt32(cm.ExecuteNonQuery())
                    End Using
                Next
            Catch ex As Exception
                Throw ex
            Finally
                cn.Close()
            End Try
        End Using
        Return rows_count
    End Function
    Public Function Update_Brand_New_Points_Contracts_As_Exported(iiMembershipEnrollmentID() As String) As Int32
        Dim rows_count = 0
        Using cn = New SqlConnection(cns)
            Using cm = New SqlCommand(String.Format("Update t_IIMembershipEnrollment Set DateExported = GETDATE(), ExportStatusID = 1 Where IIMembershipEnrollmentID in ({0})", String.Join(",", iiMembershipEnrollmentID)), cn)
                Try
                    cn.Open()
                    rows_count = cm.ExecuteNonQuery()
                Catch ex As Exception
                    cn.Close()
                    Throw ex
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        Return rows_count
    End Function
    Public Function Update_Points_Contracts_As_Cancelled(ByVal iiMembershipEnrollmentID() As String) As Int32
        Dim rows_count = 0
        Using cn = New SqlConnection(cns)
            Using cm = New SqlCommand(String.Format("Update t_IIMembershipEnrollment set DateCanceled = GETDATE(), ExportStatusID = 2 where IImembershipEnrollmentID in ({0})", String.Join(",", iiMembershipEnrollmentID)), cn)
                Try
                    cn.Open()
                    rows_count = cm.ExecuteNonQuery()
                Catch ex As Exception
                    cn.Close()
                    Throw ex
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        Return rows_count
    End Function
    Public Function Update_Points_Contracts_As_Upgraded(ByVal iiMembershipEnrollmentID() As String) As Int32
        Dim rows_count = 0
        Using cn = New SqlConnection(cns)
            Using cm = New SqlCommand(String.Format("Update t_IIMembershipEnrollment Set DateAdded=GETDATE(), DateExported = GETDATE(), ExportStatusID = 3 Where IIMembershipEnrollmentID in ({0})", String.Join(",", iiMembershipEnrollmentID)), cn)
                Try
                    cn.Open()
                    rows_count = cm.ExecuteNonQuery()
                Catch ex As Exception
                    cn.Close()
                    Throw ex
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        Return rows_count
    End Function
    Public Function Retrieve_Most_Recent_Points_Contracts(anniversaryDate As DateTime) As List(Of Member)
        Dim l = New List(Of Member)
        Dim sql = String.Format("select * from (SELECT * FROM V_IICOOWNERENROLLMENT " &
                                                           "UNION " &
                                                           "SELECT * FROM V_IIOWNERENROLLMENT) a " &
                                                           "where AnniversaryDate <= convert(datetime, '{0}')  " &
                                                           "order by contractnumber, [PRIMARY] desc ", anniversaryDate.ToShortDateString())

        Using cn = New SqlConnection(cns)
            Using ad = New SqlDataAdapter(sql, cn)
                Try
                    Dim dt = New DataTable
                    ad.Fill(dt)

                    For Each g As IGrouping(Of String, DataRow) In dt.AsEnumerable().GroupBy(Function(x) x("ContractNumber").ToString())
                        Dim m = New Member
                        Dim o = New Person
                        For Each dr As DataRow In g
                            If Int32.Parse(dr("Primary").ToString) = 1 Then
                                m.Owner.ProspectID = dr("ProspectID").ToString
                                m.Owner.LastName = dr("LastName").ToString
                                m.Owner.FirstName = dr("FirstName").ToString
                                m.Owner.IsPrimary = True
                                m.Owner.Email1 = dr("Email1").ToString
                                m.Owner.Email2 = dr("Email2").ToString
                                m.Owner.HomePhone = dr("HomePhone").ToString
                                m.Owner.WorkPhone = dr("WorkPhone").ToString
                                m.Owner.MobilePhone = dr("MobilePhone").ToString
                                m.Owner.OtherPhone = dr("OtherPhone").ToString

                                With New clsAddress
                                    .ProspectID = m.Owner.ProspectID
                                    For Each r As DataRow In .Get_Table().AsEnumerable().Where(Function(x) Boolean.Parse(x("Active").ToString()) = True)
                                        m.Owner.Addresses.Add(New Address With {
                                                      .Street = r("Address1").ToString(),
                                                      .State = r("State").ToString(),
                                                      .City = r("City").ToString(),
                                                      .Zip = r("Zip").ToString(),
                                                      .Country = r("Country").ToString()})
                                    Next
                                End With
                            Else
                                m.CoOwners.Add(New Person With {
                                                   .ProspectID = dr("ProspectID").ToString,
                                                   .LastName = dr("LastName").ToString,
                                                   .FirstName = dr("FirstName").ToString,
                                                   .IsPrimary = False,
                                                   .Email1 = dr("Email1").ToString,
                                                   .Email2 = dr("Email2").ToString,
                                                   .HomePhone = dr("HomePhone").ToString,
                                                   .WorkPhone = dr("WorkPhone").ToString,
                                                   .MobilePhone = dr("MobilePhone").ToString,
                                                   .OtherPhone = dr("OtherPhone").ToString
                                               })
                            End If
                        Next
                        Dim row = g.First()
                        m.IIMembershipEnrollmentID = row("IIMembershipEnrollmentID").ToString()
                        With m.Contract
                            .ContractID = row("ContractID").ToString()
                            .ContractNumber = row("ContractNumber").ToString()
                            .Season = row("Season").ToString()
                            .WeekType = row("WeekType").ToString()
                            .ContractStatus = row("Status").ToString
                            .MF = Decimal.Parse(row("MF").ToString)
                            If String.IsNullOrEmpty(row("ContractDate").ToString) = False Then
                                .ContractDate = DateTime.Parse(row("ContractDate").ToString)
                            End If
                            If String.IsNullOrEmpty(row("AnniversaryDate").ToString) = False Then
                                .AnniversaryDate = DateTime.Parse(row("AnniversaryDate").ToString)
                            End If
                        End With
                        ad.SelectCommand.CommandText = String.Format("select * from ufn_ContractInventory({0})", m.Contract.ContractID)
                        Dim t = New DataTable
                        ad.Fill(t)
                        For Each dr As DataRow In t.Rows
                            If dr("OccupancyYear").Equals(DBNull.Value) = False Then
                                m.Contract.OccupancyYear = dr("OccupancyYear").ToString()
                            End If

                            m.Contract.FrequencyID = dr("FrequencyID").ToString()
                            Dim n = New Inventories
                            n.Week = dr("Week").ToString()
                            n.Add(New Inventory With {
                                                           .SoldInventoryID = dr("SoldInventoryID1").ToString(),
                                                           .RoomSize = dr("UnitSubType1").ToString(),
                                                           .UnitName = dr("Name1").ToString(),
                                                           .UnitType = dr("UnitType1").ToString()})

                            If dr("SoldInventoryID2").Equals(DBNull.Value) = False Then
                                n.Add(New Inventory With {
                                                    .SoldInventoryID = dr("SoldInventoryID2").ToString(),
                                                    .RoomSize = dr("UnitSubType2").ToString(),
                                                    .UnitName = dr("Name2").ToString(),
                                                    .UnitType = dr("UnitType2").ToString()})
                            End If
                            If dr("SoldInventoryID3").Equals(DBNull.Value) = False Then
                                n.Add(New Inventory With {
                                                   .SoldInventoryID = dr("SoldInventoryID3").ToString(),
                                                   .RoomSize = dr("UnitSubType3").ToString(),
                                                   .UnitName = dr("Name3").ToString(),
                                                   .UnitType = dr("UnitType3").ToString()})
                            End If
                            m.Contract.Inventories.Add(n)
                        Next
                        l.Add(m)
                    Next
                Catch ex As Exception
                    cn.Close()
                    Throw ex
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        Return l
    End Function
    Public Function Retrieve_Upgraded_Points_Contracts() As List(Of Member)
        Dim sql = String.Format("select c.ContractID, c.ProspectID, c.ContractNumber, c.ContractDate, p.FirstName, p.LastName, ii.IIMembershipEnrollmentID, cs.ComboItem " _
                            & "from t_Contract c inner join t_ComboItems cst on cst.ComboItemID = c.SubTypeID " _
                            & "inner join t_ComboItems cs on cs.ComboItemID = c.StatusID inner join t_IIMembershipEnrollment ii on ii.ContractID = c.ContractID " _
                            & "inner join t_Prospect p on p.ProspectID = c.ProspectID " _
                            & "where c.ProspectID in ( " _
                            & "select ProspectID from t_Contract c where c.ContractID in (select c.ContractID from t_iiMembershipEnrollment ii " _
                            & "inner join t_Contract c on c.ContractID = ii.ContractID inner join t_ComboItems cs on c.StatusID = cs.ComboItemID " _
                            & "where dateExported is not null and exportStatusId in (1,3) and DateCanceled is not null and cs.ComboItem not in ('active', 'redeed', 'suspense', 'developer'))) " _
                            & "and cst.ComboItem = 'Points' and cs.ComboItem in ('active', 'redeed', 'suspense', 'developer') and ii.DateExported is null and ExportStatusID is null")

        sql = String.Format("select p.FirstName, p.LastName, p.ProspectID, ii.ContractID, c.ContractNumber, c.ContractDate, ii.IIMembershipEnrollmentID, cs.ComboItem from t_IIMembershipEnrollment ii inner join (select ProspectID from t_IIMembershipEnrollment where DateAdded is null and DateExported is not null and ExportStatusID in (2)) ii2 on ii.ProspectID = ii2.ProspectID inner join t_Contract c on c.ContractID = ii.ContractID inner join t_ComboItems cs on cs.ComboItemID = c.StatusID inner join t_ComboItems cst on cst.ComboItemID = c.SubTypeID inner join t_Prospect p on p.ProspectID = ii2.ProspectID where ii.DateAdded is null and ii.DateExported is null and ii.ExportStatusID is null and cs.ComboItem in ('active', 'redeed', 'suspense', 'developer') and cst.ComboItem in ('points') ")
        sql += String.Format("union all select distinct p.FirstName, p.LastName, p.ProspectID, ii.ContractID, c.ContractNumber, c.ContractDate, ii.IIMembershipEnrollmentID, cs.ComboItem from t_IIMembershipEnrollment ii inner join (select ProspectID from t_IIMembershipEnrollment where DateAdded is not null and DateExported is not null and ExportStatusID in (1,2)) ii2 on ii.ProspectID = ii2.ProspectID inner join t_Contract c on c.ContractID = ii.ContractID inner join t_ComboItems cs on cs.ComboItemID = c.StatusID inner join t_ComboItems cst on cst.ComboItemID = c.SubTypeID inner join t_Prospect p on p.ProspectID = ii2.ProspectID where ii.DateAdded is null and ii.DateExported is null and ii.ExportStatusID is null and cs.ComboItem in ('active', 'redeed', 'suspense', 'developer') and cst.ComboItem in ('points') order by ContractID")
        Dim l = New List(Of Member)
        Using cn = New SqlConnection(cns)
            Using cm = New SqlCommand(sql, cn)
                Try
                    cn.Open()
                    Dim dt = New DataTable
                    dt.Load(cm.ExecuteReader())
                    For Each row As DataRow In dt.Rows
                        Dim m = New Member
                        With m
                            .IIMembershipEnrollmentID = row("IIMembershipEnrollmentID").ToString()
                            .Owner.ProspectID = row("ProspectID").ToString()
                            .Owner.FirstName = row("FirstName").ToString()
                            .Owner.LastName = row("LastName").ToString()
                            .Contract.ContractID = row("ContractID").ToString()
                            .Contract.ContractNumber = row("ContractNumber").ToString()
                            .Contract.ContractDate = DateTime.Parse(row("ContractDate").ToString()).ToShortDateString()

                            With New clsContract
                                .ContractID = m.Contract.ContractID
                                .Load()
                                m.Contract.Season = New clsComboItems().Lookup_ComboItem(.SeasonID)
                                If String.IsNullOrEmpty(.AnniversaryDate) = False Then
                                    m.Contract.AnniversaryDate = DateTime.Parse(.AnniversaryDate).ToShortDateString()
                                End If
                                m.Contract.WeekType = New clsComboItems().Lookup_ComboItem(.WeekTypeID)
                            End With

                            cm.CommandText = String.Format("Select top 1 c.ContractNumber " _
                                & "from t_IIMembershipEnrollment ii inner Join (Select p.ProspectID, c.ContractID " _
                                & "from t_Prospect p inner join t_Contract c On p.ProspectID = c.ProspectID " _
                                & "where c.ContractID In (Select ContractID from t_IIMembershipEnrollment where ExportStatusID = 3)) a " _
                                & "On a.ContractID = ii.ContractID " _
                                & "inner join t_Contract c On c.ContractID = ii.ContractID " _
                                & "inner join t_ComboItems cs On cs.ComboItemID = c.StatusID " _
                                & "where cs.ComboItem = 'canceled' and c.ProspectID = {0} " _
                                & "order by ii.DateExported desc;", .Owner.ProspectID)

                            cm.CommandText = String.Format("select top 1 c.ContractNumber from t_IIMembershipEnrollment ii inner join t_Contract c on c.ContractID = ii.ContractID " _
                                & "where ii.ContractID in (select ContractID from t_Contract where ProspectID in (select ProspectID from t_Contract where ContractID = {0})) " _
                                & "and DateAdded is not null and DateCanceled is not null and ExportStatusID in (2) order by c.ContractID desc;", m.Contract.ContractID)
                            .Contract.CancelledContractNumber = cm.ExecuteScalar()

                            cm.CommandText = String.Format("select * from ufn_ContractInventory({0})", row("ContractID").ToString())
                            Dim t = New DataTable
                            t.Load(cm.ExecuteReader())
                            For Each dr As DataRow In t.Rows.OfType(Of DataRow).AsEnumerable().Distinct()

                                If dr("OccupancyYear").Equals(DBNull.Value) = False Then
                                    m.Contract.OccupancyYear = dr("OccupancyYear").ToString()
                                End If

                                m.Contract.FrequencyID = dr("FrequencyID").ToString()
                                Dim n = New Inventories
                                n.Week = dr("Week").ToString()
                                n.Add(New Inventory With {
                                                               .SoldInventoryID = dr("SoldInventoryID1").ToString(),
                                                               .RoomSize = dr("UnitSubType1").ToString(),
                                                               .UnitName = dr("Name1").ToString(),
                                                               .UnitType = dr("UnitType1").ToString()})

                                If dr("SoldInventoryID2").Equals(DBNull.Value) = False Then
                                    n.Add(New Inventory With {
                                                        .SoldInventoryID = dr("SoldInventoryID2").ToString(),
                                                        .RoomSize = dr("UnitSubType2").ToString(),
                                                        .UnitName = dr("Name2").ToString(),
                                                        .UnitType = dr("UnitType2").ToString()})
                                End If
                                If dr("SoldInventoryID3").Equals(DBNull.Value) = False Then
                                    n.Add(New Inventory With {
                                                       .SoldInventoryID = dr("SoldInventoryID3").ToString(),
                                                       .RoomSize = dr("UnitSubType3").ToString(),
                                                       .UnitName = dr("Name3").ToString(),
                                                       .UnitType = dr("UnitType3").ToString()})
                                End If
                                m.Contract.Inventories.Add(n)
                            Next
                            l.Add(m)
                        End With
                    Next
                Catch ex As Exception
                    cn.Close()
                    Throw ex
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        Return l
    End Function
    Public Function Retrieve_Cancelled_Points_Contracts(ByVal s As String, ByVal statuses() As String, ByVal sd As DateTime, ByVal ed As DateTime) As List(Of Member)
        Dim l = New List(Of Member)
        Dim sql = String.Format("select p.ProspectID, p.FirstName, p.LastName, c.ContractNumber, c.ContractID, cs.ComboItem [Contract Status], " _
                                 & "css.ComboItem [Contract Substatus], c.FrequencyID, ii.IIMembershipEnrollmentID " _
                                 & "from t_IIMembershipEnrollment ii " _
                                 & "inner join t_Contract c on c.ContractID = ii.ContractID " _
                                 & "inner join t_Prospect p on p.ProspectID = c.ProspectID " _
                                 & "left join t_ComboItems css on css.ComboItemID = c.SubStatusID " _
                                 & "inner join t_ComboItems cs on cs.ComboItemID = c.StatusID " _
                                 & "where  exportstatusid in  (1,3) and dateexported is not null and datecanceled is null " _
                                 & "and ii.ContractID in (" _
                                 & "select c.ContractID from t_Contract c " _
                                 & "inner join t_ComboItems cs on c.StatusID = cs.ComboItemID " _
                                 & "inner join t_ComboItems cst on cst.ComboItemID = c.SubTypeID " _
                                 & "where c.StatusDate between '{2}' and '{3}' " _
                                 & "and cs.ComboItemID in ({1}) " _
                                 & "and cst.ComboItem = 'points') {0} order by p.ProspectID ", s, String.Join(",", statuses.ToArray()), sd.ToShortDateString(), ed.ToShortDateString())
        Using cn = New SqlConnection(cns)
            Using cm = New SqlCommand(sql, cn)
                Try
                    cn.Open()
                    Dim dt = New DataTable
                    dt.Load(cm.ExecuteReader())
                    For Each row As DataRow In dt.Rows
                        Dim m = New Member
                        With m
                            .IIMembershipEnrollmentID = row("IIMembershipEnrollmentID").ToString()
                            .Owner.ProspectID = row("ProspectID").ToString()
                            .Owner.FirstName = row("FirstName").ToString()
                            .Owner.LastName = row("LastName").ToString()
                            .Contract.ContractID = row("ContractID").ToString()
                            .Contract.ContractNumber = row("ContractNumber").ToString()
                            .Contract.ContractStatus = row("Contract Status").ToString()
                            .Contract.ContractSubStatus = row("Contract Substatus").ToString()
                            .Contract.FrequencyID = row("FrequencyID").ToString()
                            cm.CommandText = String.Format("select * from ufn_ContractInventory({0})", row("ContractID").ToString())
                            Dim t = New DataTable
                            t.Load(cm.ExecuteReader())
                            For Each dr As DataRow In t.Rows
                                If dr("OccupancyYear").Equals(DBNull.Value) = False Then
                                    m.Contract.OccupancyYear = dr("OccupancyYear").ToString()
                                End If

                                m.Contract.FrequencyID = dr("FrequencyID").ToString()
                                Dim n = New Inventories
                                n.Week = dr("Week").ToString()
                                n.Add(New Inventory With {
                                                               .SoldInventoryID = dr("SoldInventoryID1").ToString(),
                                                               .RoomSize = dr("UnitSubType1").ToString(),
                                                               .UnitName = dr("Name1").ToString(),
                                                               .UnitType = dr("UnitType1").ToString()})

                                If dr("SoldInventoryID2").Equals(DBNull.Value) = False Then
                                    n.Add(New Inventory With {
                                                        .SoldInventoryID = dr("SoldInventoryID2").ToString(),
                                                        .RoomSize = dr("UnitSubType2").ToString(),
                                                        .UnitName = dr("Name2").ToString(),
                                                        .UnitType = dr("UnitType2").ToString()})
                                End If
                                If dr("SoldInventoryID3").Equals(DBNull.Value) = False Then
                                    n.Add(New Inventory With {
                                                       .SoldInventoryID = dr("SoldInventoryID3").ToString(),
                                                       .RoomSize = dr("UnitSubType3").ToString(),
                                                       .UnitName = dr("Name3").ToString(),
                                                       .UnitType = dr("UnitType3").ToString()})
                                End If
                                m.Contract.Inventories.Add(n)
                            Next
                            l.Add(m)
                        End With
                    Next
                Catch ex As Exception
                    cn.Close()
                    Throw ex
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        Return l
    End Function
    Public Function Show_Most_Recent_Points_Contracts(l As List(Of Member)) As DataTable
        Dim dt = New DataTable()
        With dt
            .Columns.Add("IIMembershipEnrollmentID", GetType(String))
            .Columns.Add("Anniversary Date", GetType(String))
            .Columns.Add("Prospect ID", GetType(String))
            .Columns.Add("Member Last Name", GetType(String))
            .Columns.Add("Member First Name", GetType(String))
            .Columns.Add("OWNER KCP", GetType(String))
            .Columns.Add("Resort Code", GetType(String))
            .Columns.Add("Unit Size", GetType(String))
            .Columns.Add("Unit", GetType(String))
            .Columns.Add("Unit Check-In", GetType(String))
            .Columns.Add("Week", GetType(String))
            .Columns.Add("Season", GetType(String))
            .Columns.Add("Frequency", GetType(String))
            .Columns.Add("1St Club Usage", GetType(String))
            .Columns.Add("Status", GetType(String))

            Dim max_coos = l.Max(Function(x) x.CoOwners.Count)
            For i = 0 To max_coos - 1
                .Columns.Add(String.Format("Co-Owner {0}", i + 1), GetType(String))
            Next

            .Columns.Add("Email-1", GetType(String))
            .Columns.Add("Email-2", GetType(String))
            .Columns.Add("Cell Phone", GetType(String))
            .Columns.Add("Work Phone", GetType(String))
            .Columns.Add("Home Phone", GetType(String))
            .Columns.Add("Other Phone", GetType(String))

            Dim max_addr = l.Max(Function(x) x.Owner.Addresses.Count)
            For i = 0 To max_addr - 1
                .Columns.Add(String.Format("Street Name-{0}", i + 1), GetType(String))
                .Columns.Add(String.Format("City-{0}", i + 1), GetType(String))
                .Columns.Add(String.Format("State-{0}", i + 1), GetType(String))
                .Columns.Add(String.Format("Zip-{0}", i + 1), GetType(String))
                .Columns.Add(String.Format("Country-{0}", i + 1), GetType(String))
            Next

            .Columns.Add("II Membership", GetType(String))
            .Columns.Add("M&T Amount", GetType(String))
            .Columns.Add("M&T", GetType(String))
            .Columns.Add("Float Or Fixed", GetType(String))
            .Columns.Add("Contract Date", GetType(String))
        End With
        For Each m As Member In l
            Dim newRow = dt.NewRow
            With m
                If .Contract.AnniversaryDate.HasValue Then
                    newRow("Anniversary Date") = .Contract.AnniversaryDate.Value.ToShortDateString()
                End If
                newRow("IIMembershipEnrollmentID") = .IIMembershipEnrollmentID
                newRow("Prospect ID") = .Owner.ProspectID
                newRow("Member Last Name") = .Owner.LastName.Trim()
                newRow("Member First Name") = .Owner.FirstName.Trim()
                newRow("OWNER KCP") = .Contract.ContractNumber.Trim()

                Dim col_weeks() As String = {}

                If .Contract.Inventories.Count > 0 Then
                    newRow("M&T") = .Contract.Get_G(String.Format("{0}-{1}", .Contract.Inventories.First().Select(Function(x) x.UnitType).First(), .Contract.Inventories.First().Select(Function(x) x.RoomSize).First()))
                    newRow("Resort Code") = .Contract.Get_Resort_Code(.Contract.Inventories.First().Select(Function(x) x.UnitType).First())
                    newRow("Unit Check-In") = .Contract.Get_CheckIn_Day(.Contract.Inventories.First().Select(Function(x) x.SoldInventoryID).First())
                    newRow("Unit Size") = .Contract.Inventories.Sum(Function(x) x.Sum(Function(y) Int32.Parse(y.RoomSize.Substring(0, 1))))
                    'newRow("Week") = .Contract.Inventories.First().Week
                    'newRow("Week") = .Contract.Inventories(0).Week                    

                    For i = 0 To .Contract.Inventories.Count - 1
                        If i = 0 Then
                            newRow("Unit") = String.Format("{0}{1}", .Contract.Get_Street_Abbr(.Contract.Inventories(i).First().UnitName), .Contract.Get_Unit_Number_Abbr(.Contract.Inventories(i).Select(Function(x) x.UnitName).ToArray()))
                        Else
                            newRow("Unit") += String.Format(",{0}{1}", .Contract.Get_Street_Abbr(.Contract.Inventories(i).First().UnitName), .Contract.Get_Unit_Number_Abbr(.Contract.Inventories(i).Select(Function(x) x.UnitName).ToArray()))
                        End If

                        Array.Resize(col_weeks, col_weeks.Length + 1)
                        col_weeks(col_weeks.Length - 1) = .Contract.Inventories(i).Week
                    Next

                    newRow("Week") = String.Join(",", col_weeks)
                End If

                For i = 0 To .CoOwners.Count - 1
                    newRow(String.Format("Co-Owner {0}", i + 1)) = String.Format("{0}, {1}", .CoOwners(i).LastName, .CoOwners(i).FirstName)
                Next
                For i = 0 To .Owner.Addresses.Count - 1
                    newRow(String.Format("Street Name-{0}", i + 1)) = .Owner.Addresses(i).Street
                    newRow(String.Format("City-{0}", i + 1)) = .Owner.Addresses(i).City
                    newRow(String.Format("State-{0}", i + 1)) = .Owner.Addresses(i).State
                    newRow(String.Format("Zip-{0}", i + 1)) = .Owner.Addresses(i).Zip
                    newRow(String.Format("Country-{0}", i + 1)) = .Owner.Addresses(i).Country
                Next

                newRow("Email-1") = .Owner.Email1
                newRow("Email-2") = .Owner.Email2
                newRow("Cell Phone") = .Owner.MobilePhone
                newRow("Work Phone") = .Owner.WorkPhone
                newRow("Home Phone") = .Owner.HomePhone
                newRow("Other Phone") = .Owner.OtherPhone

                newRow("Season") = .Contract.Season
                newRow("Frequency") = .Contract.Frequency
                newRow("1St Club Usage") = .Contract.Club_Usage()
                newRow("Status") = .Contract.ContractStatus
                newRow("II Membership") = .IINumber()
                newRow("M&T Amount") = .Contract.MF.ToString("C")
                newRow("Float Or Fixed") = .Contract.WeekType
                newRow("Contract Date") = .Contract.ContractDate.Value.ToShortDateString()
            End With

            dt.Rows.Add(newRow)
        Next
        Return dt
    End Function
    Public Function Show_Upgraded_Points_Contracts(l As List(Of Member)) As DataTable
        Dim dt = New DataTable()
        With dt
            .Columns.Add("IIMembershipEnrollmentID", GetType(String))
            .Columns.Add("Anniversary Date", GetType(String))
            .Columns.Add("Prospect ID", GetType(String))
            .Columns.Add("Member Last Name", GetType(String))
            .Columns.Add("Member First Name", GetType(String))
            .Columns.Add("OWNER KCP", GetType(String))
            .Columns.Add("Resort Code", GetType(String))
            .Columns.Add("Unit Size", GetType(String))
            .Columns.Add("Unit", GetType(String))
            .Columns.Add("Unit Check-In", GetType(String))
            .Columns.Add("Week", GetType(String))
            .Columns.Add("Season", GetType(String))
            .Columns.Add("Frequency", GetType(String))
            .Columns.Add("1St Club Usage", GetType(String))


            .Columns.Add("M&T Amount", GetType(String))
            .Columns.Add("M&T", GetType(String))
            .Columns.Add("Float Or Fixed", GetType(String))
            .Columns.Add("Original KCP", GetType(String))
            .Columns.Add("Canceled Upgrade", GetType(String))
            .Columns.Add("Contract Date", GetType(String))
        End With

        For Each m As Member In l
            Dim newRow = dt.NewRow
            With m
                newRow("IIMembershipEnrollmentID") = .IIMembershipEnrollmentID
                With New clsContract
                    .ContractID = m.Contract.ContractID
                    .Load()
                    newRow("Prospect ID") = .ProspectID
                    m.Contract.MF = .MaintenanceFeeAmount
                End With

                If .Contract.AnniversaryDate.HasValue Then
                    newRow("Anniversary Date") = .Contract.AnniversaryDate.Value.ToShortDateString()
                Else
                    newRow("Anniversary Date") = "N/A"
                End If


                newRow("Member Last Name") = .Owner.LastName.Trim()
                newRow("Member First Name") = .Owner.FirstName.Trim()
                newRow("OWNER KCP") = .Contract.ContractNumber.Trim()

                If .Contract.Inventories.Count > 0 Then
                    newRow("M&T") = .Contract.Get_G(String.Format("{0}-{1}", .Contract.Inventories.First().Select(Function(x) x.UnitType).First(), .Contract.Inventories.First().Select(Function(x) x.RoomSize).First()))
                    newRow("Unit Check-In") = .Contract.Get_CheckIn_Day(.Contract.Inventories.First().Select(Function(x) x.SoldInventoryID).First())
                    newRow("Week") = .Contract.Inventories.First().Week
                    'newRow("Unit Size") = .Contract.Inventories.Sum(Function(x) x.Sum(Function(y) Int32.Parse(y.RoomSize.Substring(0, 1))))
                    newRow("Week") = .Contract.Inventories(0).Week

                    For i = 0 To .Contract.Inventories.Count - 1
                        If i = 0 Then
                            newRow("Resort Code") = .Contract.Get_Resort_Code(.Contract.Inventories(i).Select(Function(x) x.UnitType).First())
                            newRow("Unit Size") = .Contract.Inventories(i).Sum(Function(x) Int32.Parse(x.RoomSize.Substring(0, 1)))
                            'newRow("Week") = .Contract.Inventories(i).Week
                            newRow("Unit") = String.Format("{0}{1}", .Contract.Get_Street_Abbr(.Contract.Inventories(i).First().UnitName), .Contract.Get_Unit_Number_Abbr(.Contract.Inventories(i).Select(Function(x) x.UnitName).ToArray()))
                        Else
                            newRow("Resort Code") += "," & .Contract.Get_Resort_Code(.Contract.Inventories(i).Select(Function(x) x.UnitType).First())
                            newRow("Unit Size") += "," & .Contract.Inventories(i).Sum(Function(x) Int32.Parse(x.RoomSize.Substring(0, 1)))
                            'newRow("Week") += "," & .Contract.Inventories(i).Week
                            newRow("Unit") += String.Format(",{0} {1}", .Contract.Get_Street_Abbr(.Contract.Inventories(i).First().UnitName), .Contract.Get_Unit_Number_Abbr(.Contract.Inventories(i).Select(Function(x) x.UnitName).ToArray()))
                        End If
                    Next
                End If

                Dim sql = String.Format("select top 1 c.ContractNumber from t_IIMembershipEnrollment ii inner join t_Contract c on ii.ContractID = c.ContractID " _
                    & "inner join t_ComboItems cs on cs.ComboItemID = c.StatusID where ii.ProspectID = {0} and ii.ExportStatusID = 1 and cs.ComboItem <> 'Active' order by ii.IIMembershipEnrollmentID desc;", newRow("Prospect ID").ToString())
                Using cn = New SqlConnection(cns)
                    Using cm = New SqlCommand(sql, cn)
                        Try
                            cn.Open()
                            newRow("Original KCP") = cm.ExecuteScalar()
                        Catch ex As Exception
                            cn.Close()
                            Throw ex
                        Finally
                            cn.Close()
                        End Try
                    End Using
                End Using

                newRow("Season") = .Contract.Season
                newRow("Frequency") = .Contract.Frequency
                newRow("1St Club Usage") = .Contract.Club_Usage()
                newRow("M&T Amount") = .Contract.MF.ToString("C")
                newRow("Float Or Fixed") = .Contract.WeekType
                newRow("Canceled Upgrade") = .Contract.CancelledContractNumber
                newRow("Contract Date") = .Contract.ContractDate.Value.ToShortDateString()

                dt.Rows.Add(newRow)
            End With
        Next
        Return dt
    End Function
    Public Function Show_Cancelled_Points_Contracts(l As List(Of Member)) As DataTable
        Dim dt = New DataTable()
        With dt
            .Columns.Add("IIMembershipEnrollmentID", GetType(String))
            .Columns.Add("Prospect ID", GetType(String))
            .Columns.Add("Member Last Name", GetType(String))
            .Columns.Add("Member First Name", GetType(String))
            .Columns.Add("Contract Number", GetType(String))
            .Columns.Add("Contract Status", GetType(String))
            .Columns.Add("Contract Substatus", GetType(String))
            .Columns.Add("Resort Code", GetType(String))
            .Columns.Add("Unit Size", GetType(String))
            .Columns.Add("Unit", GetType(String))
            .Columns.Add("Unit Check-In", GetType(String))
            .Columns.Add("Week", GetType(String))
            .Columns.Add("Frequency", GetType(String))
        End With

        For Each m As Member In l
            Dim newRow = dt.NewRow
            With m
                newRow("IIMembershipEnrollmentID") = .IIMembershipEnrollmentID
                newRow("Prospect ID") = .Owner.ProspectID
                newRow("Member Last Name") = .Owner.LastName.Trim()
                newRow("Member First Name") = .Owner.FirstName.Trim()
                newRow("Contract Number") = .Contract.ContractNumber.Trim()
                newRow("Contract Status") = .Contract.ContractStatus
                newRow("Contract Substatus") = .Contract.ContractSubStatus

                If .Contract.Inventories.Count > 0 Then
                    newRow("Unit Check-In") = .Contract.Get_CheckIn_Day(.Contract.Inventories.First().Select(Function(x) x.SoldInventoryID).First())
                    For i = 0 To .Contract.Inventories.Count - 1
                        If i = 0 Then
                            newRow("Resort Code") = .Contract.Get_Resort_Code(.Contract.Inventories(i).Select(Function(x) x.UnitType).First())
                            newRow("Unit Size") = .Contract.Inventories(i).Sum(Function(x) Int32.Parse(x.RoomSize.Substring(0, 1)))
                            newRow("Week") = .Contract.Inventories(i).Week
                            newRow("Unit") = String.Format("{0}{1}", .Contract.Get_Street_Abbr(.Contract.Inventories(i).First().UnitName), .Contract.Get_Unit_Number_Abbr(.Contract.Inventories(i).Select(Function(x) x.UnitName).ToArray()))
                        Else
                            newRow("Resort Code") += "," & .Contract.Get_Resort_Code(.Contract.Inventories(i).Select(Function(x) x.UnitType).First())
                            newRow("Unit Size") += "," & .Contract.Inventories(i).Sum(Function(x) Int32.Parse(x.RoomSize.Substring(0, 1)))
                            newRow("Week") += "," & .Contract.Inventories(i).Week
                            newRow("Unit") += String.Format(",{0} {1}", .Contract.Get_Street_Abbr(.Contract.Inventories(i).First().UnitName), .Contract.Get_Unit_Number_Abbr(.Contract.Inventories(i).Select(Function(x) x.UnitName).ToArray()))
                        End If
                    Next
                End If

                newRow("Frequency") = .Contract.Frequency
                dt.Rows.Add(newRow)
            End With
        Next
        Return dt
    End Function
End Class
Public Class Person
    Public ProspectID As Int32
    Public FirstName As String
    Public LastName As String
    Public IsPrimary As Boolean
    Public Email1 As String
    Public Email2 As String
    Public HomePhone As String
    Public MobilePhone As String
    Public WorkPhone As String
    Public OtherPhone As String
    Public Addresses As New List(Of Address)
End Class
Public Class Member
    Public Owner As New Person
    Public CoOwners As New List(Of Person)
    Public Contract As New Contract

    Public IIMembershipEnrollmentID As Int32
    Public Function IINumber() As String
        Dim sql = String.Format("select top 1 UFValue from t_uf_value where ufid = 362 and keyvalue = {0} order by keyvalue desc ", Owner.ProspectID)
        Using cn = New SqlConnection(cns)
            Using cm = New SqlCommand(sql, cn)
                Try
                    cn.Open()
                    sql = cm.ExecuteScalar()
                Catch ex As Exception
                    cn.Close()
                    Throw ex
                Finally
                    cn.Close()
                End Try
                cn.Open()
            End Using
        End Using
        Return IIf(String.IsNullOrEmpty(sql), String.Empty, sql)
    End Function

End Class
Public Class Contract
    Public ContractNumber As String
    Public ContractID As Int32
    Public ContractStatus As String
    Public ContractSubStatus As String
    Public ContractDate As DateTime?
    Public AnniversaryDate As DateTime?
    Public OccupancyYear As Int32
    Public Season As String
    Public WeekType As String
    Public MF As Decimal
    Public FrequencyID As Int32
    Public CancelledContractNumber As String
    Public Inventories As New List(Of Inventories)

    Public Function Frequency() As String
        If FrequencyID = 1 Then
            Return "Annual"
        ElseIf FrequencyID = 2 Then
            Return "Biennial"
        ElseIf FrequencyID = 3 Then
            Return "Triennial"
        Else
            Return "N/A"
        End If
    End Function
    Public Function Club_Usage() As Int32
        Dim sq = String.Format(
               "select (select top 1 occupancyYear from t_conversion where contractid = {0} order by conversionid desc) 'OCCUPANCY-YEAR', " &
               "occupancyDate 'OCCUPANCY-DATE' from t_contract where contractid = {0}", ContractID)
        Using ad = New SqlDataAdapter(sq, New SqlConnection(cns))
            Dim dt = New DataTable()
            ad.Fill(dt)
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
    End Function
    Public Function Get_G(Optional ByVal u_type As String = "") As String
        If DateTime.Parse(ContractDate.Value).CompareTo(DateTime.Parse("11/18/2005")) < 0 Then
            Return "G4"
        ElseIf DateTime.Parse(ContractDate.Value).CompareTo(DateTime.Parse("11/18/2005")) >= 0 And DateTime.Parse(ContractDate.Value).CompareTo(DateTime.Parse("02/25/2008")) < 0 Then
            Return "G3"
        ElseIf DateTime.Parse(ContractDate.Value).CompareTo(DateTime.Parse("02/25/2008")) >= 0 And DateTime.Parse(ContractDate.Value).CompareTo(DateTime.Parse("12/14/2009")) < 0 Then
            If u_type.CompareTo("KCT-4BED") = 0 Then
                Return "G2"
            Else
                Return "G1"
            End If
        ElseIf DateTime.Parse(ContractDate.Value).CompareTo(DateTime.Parse("12/14/2009")) >= 0 Then
            Return "G1"
        Else
            Return "N/A"
        End If
    End Function
    Public Function Get_Resort_Code(unitType As String) As String
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
    Public Function Get_CheckIn_Day(soldInventoryID As Int32) As String
        Dim sql = String.Format("select top 1 ISNULL(cck.comboitem, '') [Check-In Date] " &
                                "from t_soldinventory so " &
                                "inner join t_salesinventory si on so.salesinventoryid = si.salesinventoryid " &
                                "inner join t_unit u on u.unitid = si.unitid " &
                                "inner join t_room r on r.unitid = u.unitid " &
                                "left outer join t_ComboItems cck on cck.comboItemId = r.subTypeId " &
                                "where so.soldinventoryid = {0}", soldInventoryID)
        Using cn As New SqlConnection(cns)
            Using cmd As New SqlCommand(sql, cn)
                Try
                    cn.Open()
                    sql = cmd.ExecuteScalar()
                Catch ex As Exception
                    cn.Close()
                    Throw ex
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        Return IIf(String.IsNullOrEmpty(sql), "N/A", sql)
    End Function
    Public Function Get_Unit_Number_Abbr(unitNames As String()) As String
        Dim street_numbers = unitNames.Select(Function(x) x.Split(New Char() {" "c})(0))
        Dim l As New List(Of String)
        For Each n As String In street_numbers
            l.Add(n.Substring(0, n.Length - 1))
        Next
        If l.Distinct().Count() = 1 Then
            Dim tmp = l.First()
            For Each s As String In street_numbers
                tmp += s.Substring(s.Length - 1, 1)
            Next
            Return tmp
        Else
            Return String.Join(",", street_numbers.ToArray())
        End If
    End Function
    Public Function Get_Street_Abbr(streetName As String) As String
        Dim parts = streetName.Trim().Split(New Char() {" "c})
        Dim tmp = String.Empty
        For i As Integer = 1 To parts.Length - 1
            tmp += parts(i).Substring(0, 1)
        Next
        If parts(1).ToString.ToUpper = "CAROUSEL" And tmp = "CC" Then tmp = "CCT"
        Return tmp
    End Function
End Class
Public Class Inventory
    Public SoldInventoryID As Int32
    Public UnitName As String
    Public UnitType As String
    Public RoomSize As String
End Class
Public Class Inventories
    Inherits List(Of Inventory)

    Public Week As Int32
End Class
Public Class Address
    Public Street As String
    Public City As String
    Public State As String
    Public Zip As String
    Public Country As String
End Class

