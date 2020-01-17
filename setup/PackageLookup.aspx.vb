Imports System.Data
Imports System.Data.SqlClient

Partial Class setup_PackageLookup
    Inherits System.Web.UI.Page

   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Run_SQL()
 
    End Sub

    Private Sub Run_SQL()

        Dim sql_text = "select PackageId, Package, coalesce(Description, '') [Description], (select ComboItem from t_ComboItems " & _
                        "where ComboItemId = AccomRoomTypeId) [Accommodation] " & _
                        "from t_package where left(package, 3) <> 'tre' and active = 1 order by package"

        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand(sql_text, cn)
        Dim ad As SqlDataAdapter = New SqlDataAdapter(cm)

        Dim tb As DataTable = New DataTable()

        Try

            cn.Open()

            ad.Fill(tb)


            Me.GridViewPackages.DataSource = tb
            Me.GridViewPackages.DataKeyNames = New String() {"PACKAGEID"}
            Me.GridViewPackages.DataBind()


        Catch ex As Exception

            p_error.InnerText = ex.Message
        Finally

            cn.Close()

            cn = Nothing
            cm = Nothing
            ad = Nothing
        End Try

    End Sub


    
    Protected Sub GridViewPackages_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridViewPackages.PageIndexChanging

        If CheckSecurity("", "", , , Session("UserDBID")) Then
           
        End If


        Me.GridViewPackages.PageIndex = e.NewPageIndex
        Run_SQL()
    End Sub

   
    Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) Handles LinkButton1.Click
        Response.Redirect("editPackage.aspx?PackageID=0")
    End Sub
End Class
