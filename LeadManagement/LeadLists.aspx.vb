
Partial Class LeadManagement_LeadLists
    Inherits System.Web.UI.Page

    Protected Sub lbAdd_Click(sender As Object, e As System.EventArgs) Handles lbAdd.Click
        Response.Redirect("BuildLeadList.aspx")
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oLeadList As New clsLeadLists
            gvLeadLists.DataSource = oLeadList.Get_Lead_Lists()
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvLeadLists.DataKeyNames = sKeys
            gvLeadLists.DataBind()
        End If
    End Sub

    Protected Sub gvLeadLists_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If Not (e.Row Is Nothing) And e.Row.Cells.Count > 6 Then
            If e.Row.Cells(0).Text <> "No Records" Then
                'If e.Row.RowIndex > -1 Then
                If e.Row.Cells(2).Text.ToString.Length > 8 Then
                    'e.Row.Cells(6).Text = "HI" & e.Row.Cells(2).Text.ToString.Length
                    e.Row.Cells(6).Visible = False
                Else
                    'e.Row.Cells(6).Text = "BI"
                    e.Row.Cells(6).Visible = True
                End If
                'End If
            End If
        End If
    End Sub
    Protected Sub gvLeadLists_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvLeadLists.RowCommand
        Dim listID As Integer
        listID = Convert.ToInt32(gvLeadLists.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        If e.CommandName.CompareTo("Revoke List") = 0 Then
            If CheckSecurity("LeadManagement", "RevokeLeads", , , Session("UserDBID")) Then
                Dim oLeadList As New clsLeadLists
                oLeadList.LeadListID = listID
                oLeadList.Load()
                oLeadList.UserID = Session("UserDBID")
                oLeadList.DateRevoked = System.DateTime.Now
                oLeadList.RevokedByID = Session("UserDBID")
                oLeadList.Save()
                gvLeadLists.DataSource = oLeadList.Get_Lead_Lists()
                Dim sKeys(0) As String
                sKeys(0) = "ID"
                gvLeadLists.DataKeyNames = sKeys
                gvLeadLists.DataBind()
                oLeadList = Nothing
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('You Do Not Have Permission to Revoke Lead Lists.');", True)
            End If
        End If
    End Sub
End Class
