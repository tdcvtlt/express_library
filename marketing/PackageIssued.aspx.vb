Imports System.Data.Sql
Imports System.Data.SqlClient
Partial Class marketing_PackageIssued
    Inherits System.Web.UI.Page
    Dim oPkg As New clsPackageIssued


    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim bProceed As Boolean = True
        If CheckSecurity("TourPackages", "List", , , Session("UserDBID")) Then
            If ddFilter.SelectedValue = "ID" And pkgFilter.Text <> "" Then
                If Not (IsNumeric(pkgFilter.Text)) Then
                    bProceed = False
                    lblErr.Text = "Invalid Search Value."
                End If
            End If
            If bProceed Then
                Dim dr As SqlDataSource
                dr = oPkg.Search(pkgFilter.Text, ddFilter.SelectedValue, Session("Vendors"))
                GridView1.DataSource = dr
                Dim ka(0) As String
                ka(0) = "packageissuedid"
                GridView1.DataKeyNames = ka
                GridView1.DataBind()
                lblErr.Text = oPkg.Error_Message
            End If
        Else
            lblErr.Text = "ACCESS DENIED"
        End If
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Response.Redirect("editpackage.aspx?packageissuedid=" & GridView1.SelectedValue)
    End Sub
End Class
