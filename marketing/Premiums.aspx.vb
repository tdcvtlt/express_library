Imports System.Data.SqlClient

Partial Class marketing_Premiums
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If CheckSecurity("Premiums", "List", , , Session("UserDBID")) Then
            lblErr.Text = ""
            Select Case LCase(ddFilter.Text)
                Case "premium"
                    If filter.Text = "" Then
                        Run_Query("Select top 100 p.PremiumID as ID, p.PremiumName, p.Description, pt.Comboitem as Type, p.QtyOnHand as Qty from t_Premium p left outer join t_Comboitems pt on pt.comboitemid = p.TypeID where p.PremiumName like '" & filter.Text & "%' order by p.PremiumName")
                    Else
                        Run_Query("Select top 100 p.PremiumID as ID, p.PremiumName, p.Description, pt.Comboitem as Type, p.QtyOnHand as Qty from t_Premium p left outer join t_Comboitems pt on pt.comboitemid = p.TypeID where p.PremiumName like '" & filter.Text & "%' order by p.PremiumName")
                    End If

                Case "type"
                    If filter.Text = "" Then
                        Run_Query("Select top 100 p.PremiumID as ID, p.PremiumName, p.Description, pt.Comboitem as Type, p.QtyOnHand as Qty from t_Premium p left outer join t_Comboitems pt on pt.comboitemid = p.TypeID where pt.Comboitem like '" & filter.Text & "%' order by p.PremiumName")
                    Else
                        Run_Query("Select top 100 p.PremiumID as ID, p.PremiumName, p.Description, pt.Comboitem as Type, p.QtyOnHand as Qty from t_Premium p left outer join t_Comboitems pt on pt.comboitemid = p.TypeID where pt.Comboitem like '" & filter.Text & "%' order by p.PremiumName")
                    End If

                Case "id"
                    If filter.Text = "" Then
                        Run_Query("Select top 100 p.PremiumID as ID, p.PremiumName, p.Description, pt.Comboitem as Type, p.QtyOnHand as Qty from t_Premium p left outer join t_Comboitems pt on pt.comboitemid = p.TypeID where p.PremiumID like '" & filter.Text & "%' order by p.PremiumName")
                    Else
                        Run_Query("Select top 100 p.PremiumID as ID, p.PremiumName, p.Description, pt.Comboitem as Type, p.QtyOnHand as Qty from t_Premium p left outer join t_Comboitems pt on pt.comboitemid = p.TypeID where p.PremiumID like '" & filter.Text & "%' order by p.PremiumName")
                    End If

                Case Else
                    If filter.Text = "" Then
                        Run_Query("Select top 100 p.PremiumID as ID, p.PremiumName, p.Description, pt.Comboitem as Type, p.QtyOnHand as Qty from t_Premium p left outer join t_Comboitems pt on pt.comboitemid = p.TypeID order by p.PremiumName")
                    Else
                        Run_Query("Select top 100 p.PremiumID as ID, p.PremiumName, p.Description, pt.Comboitem as Type, p.QtyOnHand as Qty from t_Premium p left outer join t_Comboitems pt on pt.comboitemid = p.TypeID where p.PremiumName like '" & filter.Text & "%' order by p.PremiumName")
                    End If

            End Select
        Else
            lblErr.Text = "ACCESS DENIED"
        End If
    End Sub
    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Response.Redirect("editpremium.aspx?premiumid=" & GridView1.SelectedValue)
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
        If CheckSecurity("Premiums", "Add", , , Session("UserDBID")) Then
            Response.Redirect("editpremium.aspx?premiumid=0")
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
