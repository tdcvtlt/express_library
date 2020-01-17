Imports Microsoft.VisualBasic
Partial Class marketing_Overviews
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oOV As New clsCombinedOverview
        Dim sDate As String
        If dteOverView.Selected_Date = "" Then
            sDate = ""
        Else
            sDate = CDate(dteOverView.Selected_Date).ToString
        End If
        gvOverViews.dataSource = oOV.List(sDate, ddLocation.SelectedValue)
        gvOverViews.DataBind()
        oOV = Nothing
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        If CheckSecurity("Overview", "Create", , , Session("UserDBID")) Then
            If dteOverView.Selected_Date = "" Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('You Must Select A Date.');", True)
            Else
                Dim oOV As New clsCombinedOverview
                If oOV.Check_Exists(dteOverView.Selected_Date, ddLocation.SelectedValue) Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('This OverView Already Exists.');", True)
                Else
                    oOV.CombinedOverviewID = 0
                    oOV.Load()
                    oOV.CreatedByID = Session("UserDBID")
                    oOV.OverviewLocation = ddLocation.SelectedValue
                    oOV.DateCreated = System.DateTime.Now
                    oOV.OverviewDate = CDate(dteOverView.Selected_Date).ToShortDateString
                    oOV.Save()
                    Response.Redirect("CombinedOverview.aspx?OverViewID=" & oOV.CombinedOverviewID)
                End If
                oOV = Nothing
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('You Do Not Have Permission To Create an Overview.');", True)
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub gvOverViews_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvOverViews.SelectedIndexChanged
        Dim row As gridViewrow = gvOverViews.SelectedRow
        Response.Redirect("CombinedOverView.aspx?overviewid=" & row.Cells(1).Text)
    End Sub

    Protected Sub gvOverViews_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(2).Text = CDate(e.Row.Cells(2).Text).ToShortDateString
            End If
            e.Row.Cells(1).Visible = False
        End If
    End Sub
End Class
