Imports System.IO
Partial Class frontdesk_ConciergeReport
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If dteStart.Selected_Date = "" Or dteEnd.Selected_Date = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "DateError", "alert('Please select a date range.');", True)
        Else
            Dim oRes As New clsReservations
            gvRes.DataSource = oRes.Concierge_Rpt(dteStart.Selected_Date, dteEnd.Selected_Date, "")
            gvRes.DataBind()
            lblErr.Text = oRes.Err
            oRes = Nothing
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub gvRes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                If e.Row.Cells(10).Text = "NO" Then
                    e.Row.Cells(14).Visible = False
                Else
                    e.Row.Cells(13).Visible = False
                End If
            End If
            e.Row.Cells(10).Visible = False
        End If
    End Sub

    Protected Sub gvRes_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvRes.RowCommand
        Dim row As GridViewRow = gvRes.Rows(Convert.ToInt32(e.CommandArgument))
        If e.CommandName.CompareTo("BookTour") = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/wizards/addOPCOSTour.aspx?ReservationID=" & row.Cells(1).Text & "','win01',690,450);", True)
        ElseIf e.CommandName.CompareTo("ExtraTour") = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/wizards/addOPCOSProspect.aspx','win01',690,450);", True)
        ElseIf e.CommandName.CompareTo("AddComment") = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/general/addComments.aspx?KeyField=Reservation&KeyValue=" & row.Cells(1).Text & "','win01',300,300);", True)
        ElseIf e.CommandName.CompareTo("ViewComment") = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/general/Comment.aspx?KeyField=Reservation&KeyValue=" & row.Cells(1).Text & "','win01',300,300);", True)
        End If
    End Sub

    Protected Sub lbReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbReport.Click
        If dteStart.Selected_Date = "" Or dteEnd.Selected_Date = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "DateError", "alert('Please select a date range.');", True)
        Else
            Dim oRes As New clsReservations
            gvRes.DataSource = oRes.Concierge_Rpt(dteStart.Selected_Date, dteEnd.Selected_Date, "")
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
