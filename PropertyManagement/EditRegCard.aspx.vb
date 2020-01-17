
Partial Class setup_EditRegCard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oRegCard As New clsRegCards
            oRegCard.RegCardID = Request("RegCardID")
            oRegCard.Load()
            txtID.Text = oRegCard.RegCardID
            txtDesc.Text = oRegCard.Description
            oRegCard = Nothing
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub RegCard_Link_Click(sender As Object, e As System.EventArgs) Handles RegCard_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub RegCardItems_Link_Click(sender As Object, e As System.EventArgs) Handles RegCardItems_Link.Click
        If txtID.Text > 0 Then
            Dim oRegItems As New clsRegCardItems
            gvItems.DataSource = oRegItems.List_RegCard_Items(txtID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvItems.DataKeyNames = sKeys
            gvItems.DataBind()
            oRegItems = Nothing
            MultiView1.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub ResTypes_Link_Click(sender As Object, e As System.EventArgs) Handles ResTypes_Link.Click
        If txtID.Text > 0 Then
            Dim oRegCardTypes As New clsRegCard2ResType
            gvResTypes.DataSource = oRegCardTypes.List_RegCard_Types(txtID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvResTypes.DataKeyNames = sKeys
            gvResTypes.DataBind()
            oRegCardTypes = Nothing
            MultiView1.ActiveViewIndex = 2
        End If
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) Handles LinkButton1.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/PropertyManagement/editRegCardItem.aspx?ID=0&RegCardID=" & txtID.Text & "','win01',450,450);", True)
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As System.EventArgs) Handles LinkButton2.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/PropertyManagement/editRegCard2ResType.aspx?ID=0&RegCardID=" & txtID.Text & "','win01',450,450);", True)
    End Sub

    Protected Sub gvItems_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvItems.SelectedIndexChanged
        Dim row As GridViewRow = gvItems.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/PropertyManagement/editRegCardItem.aspx?ID=" & row.Cells(1).Text & "&RegCardID=" & txtID.Text & "','win01',450,450);", True)
    End Sub

    Protected Sub gvResTypes_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvResTypes.SelectedIndexChanged
        Dim row As GridViewRow = gvResTypes.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/PropertyManagement/editRegCard2ResType.aspx?ID=" & row.Cells(1).Text & "&RegCardID=" & txtID.Text & "','win01',450,450);", True)
    End Sub

    Protected Sub gvItems_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowIndex > 0 Then
            e.Row.Cells(2).Text = Server.HtmlDecode(e.Row.Cells(2).Text) '.Replace("<br>", vbNewLine)
        End If

    End Sub

    Protected Sub LinkButton3_Click(sender As Object, e As System.EventArgs) Handles LinkButton3.Click
        Dim oRegCard As New clsRegCards
        oRegCard.RegCardID = txtID.Text
        oRegCard.UserID = Session("UserDBID")
        oRegCard.Load()
        oRegCard.Description = txtDesc.Text
        oRegCard.Save()
        Response.Redirect("EditRegCard.aspx?RegCardID=" & oRegCard.RegCardID)
        oRegCard = Nothing

    End Sub
End Class
