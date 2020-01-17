Imports System.Data.SqlClient

Partial Class controls_LeadManagement_leads
    Inherits System.Web.UI.UserControl

    Public view As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If view = "" Or LCase(view) = "leads" Then
                MultiView1.ActiveViewIndex = 0
                Run_Query("Select * from t_Prospect where 1=2", gvLeads)
            ElseIf view = "AssignedLeads" Then
                MultiView1.ActiveViewIndex = 2
                MyLeads()
            Else
                MultiView1.ActiveViewIndex = 1
                MyLeadsWithoutTasks()
            End If
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Dim f As String = ddFilter.SelectedItem.Text
        Dim fv As String = txtFilterValue.Text
        Dim sql As String = ""
        Select Case LCase(f)
            Case "phone"
                If fv = "" Then
                    Run_Query("Select top 50 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & fv & "%' order by ph.number", gvLeads)
                Else
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & fv & "%'", gvLeads)
                End If

            Case "address1"
                If fv = "" Then
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.Address1 from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid order by Address1", gvLeads)
                Else
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.Address1 from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid where a.address1 like '" & fv & "%'", gvLeads)
                End If

            Case "city"
                If fv = "" Then
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.City from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid order by a.City", gvLeads)
                Else
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.City from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid where a.City like '" & fv & "%'", gvLeads)
                End If
            Case "state"
                If fv = "" Then
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, s.comboitem as State from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_Comboitems s on s.comboitemid = a.stateid order by s.Comboitem", gvLeads)
                Else
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, s.comboitem as State from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_Comboitems s on s.comboitemid = a.stateid where s.comboitem like '" & fv & "%'", gvLeads)
                End If
            Case "email"
                If fv = "" Then
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, e.Email from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid order by e.Email", gvLeads)
                Else
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, e.Email from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid where e.Email like '" & fv & "%'  order by e.Email", gvLeads)
                End If

            Case "name"
                If fv = "" Then
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by Lastname, Firstname", gvLeads)
                Else
                    'If InStr(fv, ",") > 0 And InStr(fv, ", ") < 1 Then fv = Replace(fv, ",", ", ")
                    If InStr(fv, ",") > 0 Then
                        Dim sName(2) As String
                        sName = fv.Split(",")
                        Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.lastname like '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' and p.firstname like '" & Trim(sName(1)).Replace(New Char() {"'"}, "''") & "%'", gvLeads)
                    Else
                        Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.lastname  like '" & fv.Replace(New Char() {"'"}, "''") & "%'", gvLeads)
                    End If

                End If

            Case "id"
                If fv = "" Then
                    Run_Query("Select top 100 p.ProspectID,p.LastName, p.FirstName, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by p.Prospectid", gvLeads)
                Else
                    Run_Query("Select top 100 p.ProspectID,p.LastName, p.FirstName, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.Prospectid like '" & fv & "%'", gvLeads)
                End If

            Case "postalcode"
                If fv = "" Then
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid order by a.PostalCode", gvLeads)
                Else
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid where a.PostalCode like '" & fv & "%'", gvLeads)
                End If

            Case "ssn"
                If fv = "" Then
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by p.SSN", gvLeads)
                Else
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.SSN like '" & fv & "%'", gvLeads)
                End If

            Case "spousessn"
                If fv = "" Then
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by p.SpouseSSN", gvLeads)
                Else
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.SpouseSSN like '" & fv & "%'", gvLeads)
                End If

            Case Else
                If fv = "" Then
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by ph.number", gvLeads)
                Else
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.Number like '" & fv & "%'", gvLeads)
                End If

        End Select

        

    End Sub

    Sub Run_Query(ByVal sSQL As String, ByRef GridView1 As GridView)
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Replace This", cn)
        Dim dr As SqlDataReader
        Try
            cn.Open()
            cm.CommandText = sSQL
            dr = cm.ExecuteReader
            GridView1.DataSource = dr
            Dim ka(0) As String
            ka(0) = "prospectid"
            GridView1.DataKeyNames = ka
            GridView1.DataBind()
            cn.Close()
        Catch ex As Exception
            lblException.Text = ex.ToString
        Finally
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try
    End Sub

    Private Sub MyLeads()
        Run_Query("Select distinct  p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_Rep2Pros r on r.prospectid = p.prospectid where r.personnelid = '" & Session("UserDBID") & "' order by ph.number", gvMyLeads)
    End Sub

    Protected Sub gvMyLeads_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvMyLeads.SelectedIndexChanged
        Response.Redirect("~/marketing/editprospect.aspx?prospectid=" & gvMyLeads.SelectedValue)
    End Sub

    Protected Sub gvLeadsWithoutTasks_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvLeadsWithoutTasks.SelectedIndexChanged
        Response.Redirect("~/marketing/editprospect.aspx?prospectid=" & gvLeadsWithoutTasks.SelectedValue)
    End Sub


    Protected Sub gvLeads_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvLeads.SelectedIndexChanged
        Response.Redirect("~/marketing/editprospect.aspx?prospectid=" & gvLeads.SelectedValue)
    End Sub

    Private Sub MyLeadsWithoutTasks()
        Run_Query("Select (select top 1 Number from t_ProspectPhone pp where pp.active = 1 and pp.prospectid = p.prospectid) as Phone, LastName, FirstName,  p.ProspectID from t_Prospect p inner join (select * from t_Rep2Pros where dateremoved is null and personnelid='" & Session("UserDBID") & "') r on r.prospectid = p.prospectid where r.personnelid = '" & Session("UserDBID") & "' and p.prospectid not in (select prospectid from t_GlobalCalendar where Completed <> 1)", gvLeadsWithoutTasks)
    End Sub
End Class
