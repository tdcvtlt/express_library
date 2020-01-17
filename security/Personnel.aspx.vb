
Partial Class security_Personnel
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        lblErr.Text = ""
        If CheckSecurity("Personnel", "List", , , Session("UserDBID")) Then
            Dim sql As String = ""
            filter.Text = filter.Text.Replace("'", "''")
            Select Case LCase(ddFilter.SelectedValue)
                Case "id"
                    If filter.Text = "" Then
                        sql = "Select top 100 p.personnelid as ID, p.LastName, p.FirstName, p.Active from t_Personnel p order by personnelid"
                    Else
                        sql = "Select top 100 p.personnelid as ID, p.LastName, p.FirstName, p.Active from t_Personnel p where p.personnelid like '" & filter.Text & "%' order by personnelid"
                    End If
                Case "name"
                    If filter.Text = "" Then
                        sql = "Select top 100 p.personnelid as ID, p.LastName, p.FirstName, p.Active from t_Personnel p order by p.lastname, p.firstname"
                        'sql = "select top 1 personnelid as id, * from t_Personnel"
                    Else
                        If InStr(filter.Text, ",") > 0 And InStr(filter.Text, ", ") < 1 Then filter.Text = Replace(filter.Text, ",", ", ")
                        If InStr(filter.Text, ",") > 0 Then
                            sql = "Select top 100 p.personnelid as ID, p.LastName, p.FirstName, p.Active from t_Personnel p where p.lastname + ', ' + p.firstname like '" & filter.Text & "%' order by p.lastname, p.firstname"
                        Else
                            sql = "Select top 100 p.personnelid as ID, p.LastName, p.FirstName, p.Active from t_Personnel p where p.lastname like '" & filter.Text & "%' order by p.lastname, p.firstname"
                        End If
                    End If
                Case "ssn"
                    If filter.Text = "" Then
                        sql = "Select top 100 p.personnelid as ID, p.LastName, p.FirstName, p.Active from t_Personnel p order by p.ssn"
                    Else
                        sql = "Select top 100 p.personnelid as ID, p.LastName, p.FirstName, p.Active from t_Personnel p where p.ssn like '" & filter.Text & "%' order by p.ssn"
                    End If
                Case Else
                    If filter.Text = "" Then
                        sql = "Select top 100 p.personnelid as ID, p.LastName, p.FirstName, p.Active from t_Personnel p order by personnelid"
                    Else
                        sql = "Select top 100 p.personnelid as ID, p.LastName, p.FirstName, p.Active from t_Personnel p order by personnelid"
                    End If
            End Select

            Dim ds As New SqlDataSource
            ds.SelectCommand = sql
            ds.ConnectionString = Resources.resource.cns
            gvPersonnel.DataSource = ds
            gvPersonnel.DataBind()

            ds = Nothing
        Else
            lblErr.Text = "ACCESS DENIED"
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            Button1_Click(sender, e)
        End If
    End Sub

    Protected Sub gvPersonnel_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvPersonnel.SelectedIndexChanged
        Dim row As GridViewRow = gvPersonnel.SelectedRow
        Response.Redirect("EditPersonnel.aspx?PersonnelID=" & row.Cells(1).Text)
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        lblErr.Text = ""
        If CheckSecurity("Personnel", "Edit", , , Session("UserDBID")) Then
            Response.Redirect("EditPersonnel.aspx?PersonnelID=0")
        Else
            lblErr.Text = "ACCESS DENIED"
        End If
    End Sub
End Class
