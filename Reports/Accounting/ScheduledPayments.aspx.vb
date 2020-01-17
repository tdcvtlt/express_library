Imports System.Data
Imports System.Data.SqlClient
Partial Class Reports_Accounting_Scheduled_Payments
    Inherits System.Web.UI.Page

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        If Not (dfSDate.Selected_Date <> "" And dfEDate.Selected_Date <> "") Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "warning", "alert('Please select both a Starting and Ending Date.');", True)
        Else
            gvReport.Visible = True
            Dim ds As New SqlDataSource(Resources.Resource.cns, Get_SQL)
            gvReport.DataSource = ds
            gvReport.DataBind()
            ds = Nothing
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim ds As New SqlDataSource(Resources.Resource.cns, "Select * from t_CCMerchantAccount")
            ddAccount.DataSource = ds
            ddAccount.DataValueField = "AccountName"
            ddAccount.DataTextField = "Description"
            ddAccount.DataBind()
            ds = Nothing
        End If
    End Sub

    Private Function Get_SQL() As String
        Dim sql As String = ""
        If hfScheduled.Value = "1" Then 'Looking at Scheduled Payments
            lblType.Text = "Schedule Payments"
            Select Case ddAccount.SelectedValue
                Case "~rify~"
                    sql = "select pr.ProspectID, c.ContractID, c.contractnumber as KCP, p.Invoice,pr.FirstName + ' ' + pr.Lastname as Owner, CONVERT(varchar, CONVERT(money, p.amount), 1) as Amount, [Scheduled Date], p.Method, p.CCNumber " & _
                            "from v_SchedPayments p " & _
                            "	inner join t_Contract c on c.contractid = p.keyvalue and p.keyfield = 'contractid' " & _
                            "	inner join t_Prospect pr on pr.prospectid = c.prospectid " & _
                            "where scheddate between '" & dfSDate.Selected_Date & "' and '" & dfEDate.Selected_Date & "' and accountname = '" & ddAccount.SelectedValue & "' order by SchedDate"
                Case "~0001~", "~0002~", "~0015~", "~0013~"
                    sql = "select pr.ProspectID, r.ReservationID, p.Invoice,pr.FirstName + ' ' + pr.Lastname as Owner, CONVERT(varchar, CONVERT(money, p.amount), 1) as Amount, [Scheduled Date], p.Method, p.CCNumber " & _
                            "from v_SchedPayments p " & _
                            "	inner join t_Reservations r on r.reservationid = p.keyvalue and p.keyfield = 'reservationid' " & _
                            "	inner join t_Prospect pr on pr.prospectid = r.prospectid " & _
                            "where scheddate between '" & dfSDate.Selected_Date & "' and '" & dfEDate.Selected_Date & "' and accountname = '" & ddAccount.SelectedValue & "' order by SchedDate"
                Case "~0004~", "~0005~", "~0006~"
                    sql = "select pr.ProspectID,m.MortgageID, c.ContractID, c.contractnumber as KCP, p.Invoice,pr.FirstName + ' ' + pr.Lastname as Owner, CONVERT(varchar, CONVERT(money, p.amount), 1) as Amount, [Scheduled Date], p.Method, p.CCNumber " & _
                            "from v_SchedPayments p " & _
                            "   inner join t_Mortgage m on m.mortgageid = p.keyvalue and p.keyfield = 'mortgagedp' " & _
                            "	inner join t_Contract c on c.contractid = m.contractid " & _
                            "	inner join t_Prospect pr on pr.prospectid = c.prospectid " & _
                            "where scheddate between '" & dfSDate.Selected_Date & "' and '" & dfEDate.Selected_Date & "' and accountname = '" & ddAccount.SelectedValue & "' order by SchedDate"
                Case "~0007~"
                    sql = "select pr.ProspectID,m.ConversionID, c.ContractID, c.contractnumber as KCP, p.Invoice,pr.FirstName + ' ' + pr.Lastname as Owner, CONVERT(varchar, CONVERT(money, p.amount), 1) as Amount, [Scheduled Date], p.Method, p.CCNumber " & _
                            "from v_SchedPayments p " & _
                            "   inner join t_Conversion m on m.Conversionid = p.keyvalue and p.keyfield = 'Conversiondp' " & _
                            "	inner join t_Contract c on c.contractid = m.contractid " & _
                            "	inner join t_Prospect pr on pr.prospectid = c.prospectid " & _
                            "where scheddate between '" & dfSDate.Selected_Date & "' and '" & dfEDate.Selected_Date & "' and accountname = '" & ddAccount.SelectedValue & "' order by SchedDate"

                Case Else
                    sql = "select pr.ProspectID, c.ContractID, c.contractnumber as KCP, p.Invoice,pr.FirstName + ' ' + pr.Lastname as Owner, CONVERT(varchar, CONVERT(money, p.amount), 1) as Amount, [Scheduled Date], p.Method, p.CCNumber " & _
                            "from v_SchedPayments p " & _
                            "	inner join t_Contract c on c.contractid = p.keyvalue and p.keyfield = 'contractid' " & _
                            "	inner join t_Prospect pr on pr.prospectid = c.prospectid " & _
                            "where scheddate between '" & dfSDate.Selected_Date & "' and '" & dfEDate.Selected_Date & "' and accountname = '" & ddAccount.SelectedValue & "' order by SchedDate"
            End Select



        Else 'Looking at Received Payments
            lblType.Text = "Received Payments"
            Select Case ddAccount.SelectedValue
                Case "~rify~"
                    sql = "select pr.ProspectID, c.ContractID, c.contractnumber as KCP, p.Invoice,pr.FirstName + ' ' + pr.Lastname as Owner, CONVERT(varchar, CONVERT(money, p.amount), 1) as Amount, [Scheduled Date], p.Method, p.CCNumber " & _
                            "from v_SchedPayments_Processed p " & _
                            "	inner join t_Contract c on c.contractid = p.keyvalue and p.keyfield = 'contractid' " & _
                            "	inner join t_Prospect pr on pr.prospectid = c.prospectid " & _
                            "where scheddate between '" & dfSDate.Selected_Date & "' and '" & dfEDate.Selected_Date & "' and accountname = '" & ddAccount.SelectedValue & "' order by SchedDate"
                Case "~0001~", "~0002~", "~0013~", "~0015~"
                    sql = "select pr.ProspectID, r.ReservationID, p.Invoice,pr.FirstName + ' ' + pr.Lastname as Owner, CONVERT(varchar, CONVERT(money, p.amount), 1) as Amount, [Scheduled Date], p.Method, p.CCNumber " & _
                            "from v_SchedPayments_Processed p " & _
                            "	inner join t_Reservations r on r.reservationid = p.keyvalue and p.keyfield = 'reservationid' " & _
                            "	inner join t_Prospect pr on pr.prospectid = r.prospectid " & _
                            "where scheddate between '" & dfSDate.Selected_Date & "' and '" & dfEDate.Selected_Date & "' and accountname = '" & ddAccount.SelectedValue & "' order by SchedDate"
                Case "~0004~", "~0005~", "~0006~"
                    sql = "select pr.ProspectID,m.MortgageID, c.ContractID, c.contractnumber as KCP, p.Invoice,pr.FirstName + ' ' + pr.Lastname as Owner, CONVERT(varchar, CONVERT(money, p.amount), 1) as Amount, [Scheduled Date], p.Method, p.CCNumber " & _
                            "from v_SchedPayments_Processed p " & _
                            "   inner join t_Mortgage m on m.mortgageid = p.keyvalue and p.keyfield = 'mortgagedp' " & _
                            "	inner join t_Contract c on c.contractid = m.contractid " & _
                            "	inner join t_Prospect pr on pr.prospectid = c.prospectid " & _
                            "where scheddate between '" & dfSDate.Selected_Date & "' and '" & dfEDate.Selected_Date & "' and accountname = '" & ddAccount.SelectedValue & "' order by SchedDate"
                Case "~0007~"
                    sql = "select pr.ProspectID,m.ConversionID, c.ContractID, c.contractnumber as KCP, p.Invoice,pr.FirstName + ' ' + pr.Lastname as Owner, CONVERT(varchar, CONVERT(money, p.amount), 1) as Amount, [Scheduled Date], p.Method, p.CCNumber " & _
                            "from v_SchedPayments_Processed p " & _
                            "   inner join t_Conversion m on m.Conversionid = p.keyvalue and p.keyfield = 'Conversiondp' " & _
                            "	inner join t_Contract c on c.contractid = m.contractid " & _
                            "	inner join t_Prospect pr on pr.prospectid = c.prospectid " & _
                            "where scheddate between '" & dfSDate.Selected_Date & "' and '" & dfEDate.Selected_Date & "' and accountname = '" & ddAccount.SelectedValue & "' order by SchedDate"

                Case Else
                    sql = "select pr.ProspectID, c.ContractID, c.contractnumber as KCP, p.Invoice,pr.FirstName + ' ' + pr.Lastname as Owner, CONVERT(varchar, CONVERT(money, p.amount), 1) as Amount, [Scheduled Date], p.Method, p.CCNumber " & _
                            "from v_SchedPayments_Processed p " & _
                            "	inner join t_Contract c on c.contractid = p.keyvalue and p.keyfield = 'contractid' " & _
                            "	inner join t_Prospect pr on pr.prospectid = c.prospectid " & _
                            "where scheddate between '" & dfSDate.Selected_Date & "' and '" & dfEDate.Selected_Date & "' and accountname = '" & ddAccount.SelectedValue & "' order by SchedDate"
            End Select
        End If

        Return sql
    End Function

    Protected Sub lbScheduled_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbScheduled.Click
        hfScheduled.Value = "1"
        gvReport.Visible = False
        lblType.Text = "Scheduled Payments"
    End Sub

    Protected Sub lbReceived_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbReceived.Click
        hfScheduled.Value = "0"
        gvReport.Visible = False
        lblType.Text = "Received Payments"
    End Sub

    Protected Sub gvReport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReport.RowDataBound

    End Sub

    Protected Sub gvReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvReport.SelectedIndexChanged

    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & IIf(hfScheduled.Value = "1", "Scheduled_Payments", "Received Payments") & ".xls")
        Response.Write("<table>")
        For Each oRow As GridViewRow In gvReport.Rows
            Response.Write("<tr>")
            For i = 0 To oRow.Cells.Count - 1
                Response.Write("<td>" & oRow.Cells(i).Text & "</td>")
            Next
            Response.Write("</tr>")
        Next
        Response.Write("</table>")
        Response.End()
    End Sub
End Class
