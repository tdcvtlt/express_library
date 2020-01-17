Imports System.Data.SqlClient

Partial Class marketing_Campaigns
    Inherits System.Web.UI.Page
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If CheckSecurity("Campaigns", "List", , , Session("UserDBID")) Then
            lblErr.Text = ""
            Select Case LCase(ddFilter.Text)
                Case "campaign"
                    If filter.Text = "" Then
                        Run_Query("Select top 100 c.campaignid as ID, c.Name, c.Description, ct.Comboitem as Type, cd.comboitem as Dept from t_Campaign c left outer join t_Comboitems ct on ct.comboitemid = c.TypeID left outer join t_Comboitems cd on cd.comboitemid = c.departmentid where c.Name like '" & filter.Text & "%' order by c.Name")
                    Else
                        Run_Query("Select top 100 c.campaignid as ID, c.Name, c.Description, ct.Comboitem as Type, cd.comboitem as Dept from t_Campaign c left outer join t_Comboitems ct on ct.comboitemid = c.TypeID left outer join t_Comboitems cd on cd.comboitemid = c.departmentid where c.Name like '" & filter.Text & "%' order by c.Name")
                    End If

                Case "dept"
                    If filter.Text = "" Then
                        Run_Query("Select top 100 c.campaignid as ID, c.Name, c.Description, ct.Comboitem as Type, cd.comboitem as Dept from t_Campaign c left outer join t_Comboitems ct on ct.comboitemid = c.TypeID left outer join t_Comboitems cd on cd.comboitemid = c.departmentid where cd.comboitem like '" & filter.Text & "%' order by c.Name")
                    Else
                        Run_Query("Select top 100 c.campaignid as ID, c.Name, c.Description, ct.Comboitem as Type, cd.comboitem as Dept from t_Campaign c left outer join t_Comboitems ct on ct.comboitemid = c.TypeID left outer join t_Comboitems cd on cd.comboitemid = c.departmentid where cd.comboitem like '" & filter.Text & "%' order by c.Name")
                    End If

                Case "type"
                    If filter.Text = "" Then
                        Run_Query("Select top 100 c.campaignid as ID, c.Name, c.Description, ct.Comboitem as Type, cd.comboitem as Dept from t_Campaign c left outer join t_Comboitems ct on ct.comboitemid = c.TypeID left outer join t_Comboitems cd on cd.comboitemid = c.departmentid where ct.comboitem like '" & filter.Text & "%' order by c.Name")
                    Else
                        Run_Query("Select top 100 c.campaignid as ID, c.Name, c.Description, ct.Comboitem as Type, cd.comboitem as Dept from t_Campaign c left outer join t_Comboitems ct on ct.comboitemid = c.TypeID left outer join t_Comboitems cd on cd.comboitemid = c.departmentid where ct.comboitem like '" & filter.Text & "%' order by c.Name")
                    End If

                Case "id"
                    If filter.Text = "" Then
                        Run_Query("Select top 100 c.campaignid as ID, c.Name, c.Description, ct.Comboitem as Type, cd.comboitem as Dept from t_Campaign c left outer join t_Comboitems ct on ct.comboitemid = c.TypeID left outer join t_Comboitems cd on cd.comboitemid = c.departmentid where c.campaignid like '" & filter.Text & "%' order by c.Name")
                    Else
                        Run_Query("Select top 100 c.campaignid as ID, c.Name, c.Description, ct.Comboitem as Type, cd.comboitem as Dept from t_Campaign c left outer join t_Comboitems ct on ct.comboitemid = c.TypeID left outer join t_Comboitems cd on cd.comboitemid = c.departmentid where c.campaignid like '" & filter.Text & "%' order by c.Name")
                    End If

                Case Else
                    If filter.Text = "" Then
                        Run_Query("Select top 100 c.campaignid as ID, c.Name, c.Description, ct.Comboitem as Type, cd.comboitem as Dept from t_Campaign c left outer join t_Comboitems ct on ct.comboitemid = c.TypeID left outer join t_Comboitems cd on cd.comboitemid = c.departmentid where c.Name like '" & filter.Text & "%' order by c.Name")
                    Else
                        Run_Query("Select top 100 c.campaignid as ID, c.Name, c.Description, ct.Comboitem as Type, cd.comboitem as Dept from t_Campaign c left outer join t_Comboitems ct on ct.comboitemid = c.TypeID left outer join t_Comboitems cd on cd.comboitemid = c.departmentid where c.Name like '" & filter.Text & "%' order by c.Name")
                    End If

            End Select
        Else
            lblErr.Text = "ACCESS DENIED"
        End If
    End Sub
    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Response.Redirect("editcampaign.aspx?campaignid=" & GridView1.SelectedValue)
    End Sub

    Sub Run_Query(ByVal sSQL As String)
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Replace This", cn)
        Dim dr As SqlDataReader
        Try
            cn.Open()
            cm.CommandText = sSQL
            dr = cm.ExecuteReader
            GridView1.DataSource = dr
            Dim ka(0) As String
            ka(0) = "ID"
            GridView1.DataKeyNames = ka
            GridView1.DataBind()
            cn.Close()
        Catch ex As Exception
            Label2.Text = ex.ToString
        Finally
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If CheckSecurity("Campaigns", "Add", , , Session("UserDBID")) Then
            Response.Redirect("editcampaign.aspx?campaignid=0")
        Else
            lblErr.Text = "ACCESS DENIED"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            Button1_Click(sender, e)
        End If
    End Sub
End Class
