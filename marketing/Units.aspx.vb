Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class marketing_Units
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If CheckSecurity("Units", "List", , , Session("UserDBID")) Then
            Select Case LCase(ddFilter.Text)
                Case "room number"
                    If filter.Text <> "" Then
                        Run_Query("select Name, UnitID from t_unit where unitid in (select unitid from t_room where roomnumber like '%" & filter.Text & "%')")
                    Else
                        Run_Query("select Name, UnitID from t_Unit")
                    End If
                Case "unit number"
                    If filter.Text <> "" Then
                        Run_Query("select Name,UnitID from t_unit where name like '%" & filter.Text & "%'")
                    Else
                        Run_Query("select Name, UnitID from t_Unit")
                    End If
                Case Else
                    lblErr.Text = ddFilter.Text
            End Select
        Else
            lblErr.Text = "ACCESS DENIED"
        End If
    End Sub

    Sub Run_Query(ByVal sSQL As String)
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim dr As SqlDataReader

        Try
            cn.Open()
            cm.CommandText = sSQL
            dr = cm.ExecuteReader
            gridview1.datasource = dr
            Dim ka(0) As String
            ka(0) = "unitid"
            gridview1.datakeynames = ka
            gridview1.databind()
            cn.Close()
        Catch ex As Exception
            lblErr.text = ex.Message
        Finally
            If cn.State <> Data.ConnectionState.Closed Then
                cn = Nothing
                cm = Nothing
                dr = Nothing
            End If

        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If IsPostBack Then
        'Button1_Click(sender, e)
        'End If
        ddFilter_SelectedIndexChanged(sender, e)
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If CheckSecurity("Units", "Add", , , Session("UserDBID")) Then
            Response.Redirect("editUnit.aspx?unitid=0")
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Access Denied');", True)
        End If
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Response.Redirect("editUnit.aspx?unitid=" & gridview1.selectedValue)
    End Sub

    Protected Sub ddFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFilter.SelectedIndexChanged
        'If ddFilter.SelectedItem.Text = "Unit Number" Then
        'Label1.Text = "Enter a Unit Number"
        'Else
        'Label1.Text = "Enter a Room Number"
        'End If
        Label1.Text = "Enter a " & ddFilter.SelectedItem.Text
    End Sub
End Class
