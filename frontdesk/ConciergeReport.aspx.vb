Imports System.IO
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

Partial Class frontdesk_ConciergeReport
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If dteStart.Selected_Date = "" Or dteEnd.Selected_Date = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "DateError", "alert('Please select a date range.');", True)
        Else
            Dim oRes As New clsReservations
            gvRes.DataSource = oRes.Concierge_Rpt(dteStart.Selected_Date, dteEnd.Selected_Date, rbStatus.SelectedValue)
            gvRes.DataBind()
            lblErr.Text = oRes.Err
            oRes = Nothing

        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub gvRes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex < 1 Then
                e.Row.Cells(27).Visible = False
                e.Row.Cells(21).Visible = False
                If rbStatus.SelectedValue = "Other" Then
                    e.Row.Cells(18).Visible = False
                    e.Row.Cells(19).Visible = False
                End If
            End If
            If e.Row.RowIndex > -1 Then
                'If e.Row.Cells(26).Text = "NO" Then
                e.Row.Cells(21).Visible = False
                'Else
                '   e.Row.Cells(20).Visible = False
                'End If
                If e.Row.Cells(2).Text = "Y" Or rbStatus.SelectedValue = "Other" Or e.Row.Cells(3).Text = "Y" Then
                    e.Row.Cells(18).Visible = False
                    e.Row.Cells(19).Visible = False
                End If
                If rbStatus.SelectedValue = "Booked" Then
                    e.Row.Cells(17).Visible = False
                End If
                e.Row.Cells(27).Visible = False
            End If
            If rbStatus.SelectedValue = "Booked" Then
                e.Row.Cells(17).Visible = False
                e.Row.Cells(18).Visible = True
                e.Row.Cells(19).Visible = True

            End If
            e.Row.Cells(27).Visible = False
        End If
    End Sub

    Protected Sub gvRes_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvRes.RowCommand
        Dim row As GridViewRow = gvRes.Rows(Convert.ToInt32(e.CommandArgument))
        If e.CommandName.CompareTo("BookTour") = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/wizards/addOPCOSTour.aspx?ReservationID=" & row.Cells(4).Text & "','win01',690,450);", True)
        ElseIf e.CommandName.CompareTo("ExtraTour") = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/wizards/addOPCOSProspect.aspx','win01',690,450);", True)
        ElseIf e.CommandName.CompareTo("AddComment") = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/general/addComments.aspx?KeyField=Reservation&KeyValue=" & row.Cells(4).Text & "','win01',300,300);", True)
        ElseIf e.CommandName.CompareTo("ViewComment") = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/general/Comment.aspx?KeyField=Reservation&KeyValue=" & row.Cells(4).Text & "','win01',300,300);", True)
        End If
    End Sub

    Protected Sub lbReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbReport.Click
        If dteStart.Selected_Date = "" Or dteEnd.Selected_Date = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "DateError", "alert('Please select a date range.');", True)
        Else
            Dim oRes As New clsReservations
            gvRes.DataSource = oRes.Concierge_Rpt(dteStart.Selected_Date, dteEnd.Selected_Date, rbStatus.SelectedValue)
            gvRes.DataBind()
            lblErr.Text = oRes.Err
            oRes = Nothing
        End If
    End Sub

    Protected Sub btnExcel_Click(sender As Object, e As System.EventArgs) Handles btnExcel.Click
        Dim sw As New StringWriter()
        Dim hw As New System.Web.UI.HtmlTextWriter(sw)
        Dim frm As HtmlForm = New HtmlForm()
        Page.Response.AddHeader("content-disposition", "attachment;filename=ConciergeRpt.xls")
        Page.Response.ContentType = "application/vnd.ms-excel"
        Page.Response.Charset = ""
        Page.EnableViewState = False
        frm.Attributes("runat") = "server"
        Controls.Add(frm)
        frm.Controls.Add(gvRes)
        frm.RenderControl(hw)
        Response.Write(sw.ToString())
        Response.End()
    End Sub

    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

End Class
