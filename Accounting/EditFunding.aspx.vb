Imports System.IO

Partial Class Accounting_EditFunding
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oFundingItems As New clsFundingItems
            gvFundingItems.DataSource = oFundingItems.List_FundingItems(Request("FundingID"))
            Dim sKeys(0) As String
            sKeys(0) = "FundingItemID"
            gvFundingItems.DataKeyNames = sKeys
            gvFundingItems.DataBind()
            MultiView1.ActiveViewIndex = 0
            oFundingItems = Nothing
        End If
    End Sub

    Protected Sub gvFundingItems_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFundingItems.SelectedIndexChanged

    End Sub

    Protected Sub gvFundingItems_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.Cells(3).Text = "True" Then
                e.Row.ForeColor = System.Drawing.Color.Red
            End If
            e.Row.Cells(1).Visible = False
            e.Row.Cells(2).Visible = False
            e.Row.Cells(3).Visible = False
        End If
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Dim oFundingItem As New clsFundingItems
        For Each row As GridViewRow In gvFundingItems.Rows
            Dim cb As CheckBox = row.FindControl("ItemSelect")
            If cb.Checked Then
                If oFundingItem.Remove_FundingItem(row.Cells(1).Text) Then
                Else
                    lblErr.Text = oFundingItem.Err
                End If
            End If
        Next
        gvFundingItems.DataSource = oFundingItem.List_FundingItems(Request("FundingID"))
        Dim sKeys(0) As String
        sKeys(0) = "FundingItemID"
        gvFundingItems.DataKeyNames = sKeys
        gvFundingItems.DataBind()
        oFundingItem = Nothing
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim oFunding As New clsFunding
        oFunding.UserID = Session("UserDBID")
        If oFunding.Submit_To_Payroll(Request("FundingID")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Submitted');", True)
        Else
            lblErr.Text = oFunding.Err
        End If
        oFunding = Nothing
    End Sub

    Protected Sub LinkButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton2.Click
        Dim oFundingItem As New clsFundingItems
        gvKCP.DataSource = oFundingItem.Funding_Report(Request("FundingID"), "kcp")
        Dim sKeys(0) As String
        sKeys(0) = "FundingItemID"
        gvKCP.DataKeyNames = sKeys
        gvKCP.DataBind()
        oFundingItem = Nothing
        MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub LinkButton3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton3.Click
        Dim oFundingItem As New clsFundingItems
        gvKCPPS.DataSource = oFundingItem.Funding_Report(Request("FundingID"), "kcpps")
        Dim sKeys(0) As String
        sKeys(0) = "FundingItemID"
        gvKCPPS.DataKeyNames = sKeys
        gvKCPPS.DataBind()
        oFundingItem = Nothing
        MultiView1.ActiveViewIndex = 2
    End Sub

    Protected Sub LinkButton4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton4.Click
        Dim oFundingItem As New clsFundingItems
        gvWF.DataSource = oFundingItem.Funding_Report(Request("FundingID"), "wf")
        Dim sKeys(0) As String
        sKeys(0) = "FundingItemID"
        gvWF.DataKeyNames = sKeys
        gvWF.DataBind()
        oFundingItem = Nothing
        MultiView1.ActiveViewIndex = 3
    End Sub

    Protected Sub LinkButton5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton5.Click
        Dim oFundingItem As New clsFundingItems
        gvWFPS.DataSource = oFundingItem.Funding_Report(Request("FundingID"), "wfps")
        Dim sKeys(0) As String
        sKeys(0) = "FundingItemID"
        gvWFPS.DataKeyNames = sKeys
        gvWFPS.DataBind()
        oFundingItem = Nothing
        MultiView1.ActiveViewIndex = 4
    End Sub

    Protected Sub LinkButton6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton6.Click
        Dim oFundingItem As New clsFundingItems
        gvAtl.DataSource = oFundingItem.Funding_Report(Request("FundingID"), "atl")
        Dim sKeys(0) As String
        sKeys(0) = "FundingItemID"
        gvAtl.DataKeyNames = sKeys
        gvAtl.DataBind()
        oFundingItem = Nothing
        MultiView1.ActiveViewIndex = 5
    End Sub

    Protected Sub LinkButton7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton7.Click
        Dim oFundingItem As New clsFundingItems
        gvAtlPS.DataSource = oFundingItem.Funding_Report(Request("FundingID"), "atlps")
        Dim sKeys(0) As String
        sKeys(0) = "FundingItemID"
        gvAtlPS.DataKeyNames = sKeys
        gvAtlPS.DataBind()
        oFundingItem = Nothing
        MultiView1.ActiveViewIndex = 6
    End Sub

    Protected Sub gvKCP_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.Cells(2).Text = "True" Then
                e.Row.ForeColor = System.Drawing.Color.Red
            End If
            e.Row.Cells(0).Visible = False
            e.Row.Cells(1).Visible = False
            e.Row.Cells(2).Visible = False
        End If
    End Sub
    Protected Sub gvKCPPS_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.Cells(2).Text = "True" Then
                e.Row.ForeColor = System.Drawing.Color.Red
            End If
            e.Row.Cells(0).Visible = False
            e.Row.Cells(1).Visible = False
            e.Row.Cells(2).Visible = False
        End If
    End Sub
    Protected Sub gvWF_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.Cells(2).Text = "True" Then
                e.Row.ForeColor = System.Drawing.Color.Red
            End If
            e.Row.Cells(0).Visible = False
            e.Row.Cells(1).Visible = False
            e.Row.Cells(2).Visible = False
        End If
    End Sub
    Protected Sub gvWFPS_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.Cells(2).Text = "True" Then
                e.Row.ForeColor = System.Drawing.Color.Red
            End If
            e.Row.Cells(0).Visible = False
            e.Row.Cells(1).Visible = False
            e.Row.Cells(2).Visible = False
        End If
    End Sub
    Protected Sub gvAtl_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.Cells(2).Text = "True" Then
                e.Row.ForeColor = System.Drawing.Color.Red
            End If
            e.Row.Cells(0).Visible = False
            e.Row.Cells(1).Visible = False
            e.Row.Cells(2).Visible = False
        End If
    End Sub
    Protected Sub gvAtlPS_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.Cells(2).Text = "True" Then
                e.Row.ForeColor = System.Drawing.Color.Red
            End If
            e.Row.Cells(0).Visible = False
            e.Row.Cells(1).Visible = False
            e.Row.Cells(2).Visible = False
        End If
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        Dim oFundingItems As New clsFundingItems
        gvFundingItems.DataSource = oFundingItems.List_FundingItems(Request("FundingID"))
        Dim sKeys(0) As String
        sKeys(0) = "FundingItemID"
        gvFundingItems.DataKeyNames = sKeys
        gvFundingItems.DataBind()
        MultiView1.ActiveViewIndex = 0
        oFundingItems = Nothing
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Accounting/addFundingItem.aspx?FundingID=" & Request("FundingID") & "&cxl=False','win01',690,450);", True)
    End Sub

    Protected Sub Unnamed2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Accounting/addFundingItem.aspx?FundingID=" & Request("FundingID") & "&cxl=True','win01',690,450);", True)
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        Dim sw As New StringWriter()
        Dim hw As New System.Web.UI.HtmlTextWriter(sw)
        Dim frm As HtmlForm = New HtmlForm()
        Page.Response.AddHeader("content-disposition", "attachment;filename=Funding.xls")
        Page.Response.ContentType = "application/vnd.ms-excel"
        Page.Response.Charset = ""
        Page.EnableViewState = False
        frm.Attributes("runat") = "server"
        Controls.Add(frm)
        frm.Controls.Add(gvFundingItems)
        frm.RenderControl(hw)
        Response.Write(sw.ToString())
        Response.End()
    End Sub
    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub


End Class
