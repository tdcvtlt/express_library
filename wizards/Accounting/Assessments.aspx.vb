
Imports System.Data.SqlClient

Partial Class wizards_Accounting_Assessments
    Inherits System.Web.UI.Page

    Protected Sub btnQuery_Click(sender As Object, e As EventArgs) Handles btnQuery.Click
        If filter.Text <> "" Then
            Run_Query("Select * from t_AssessmentBatch where " & ddFilter.SelectedValue & " = '" & filter.Text & "' order by BatchID desc")

        Else
            Run_Query("Select * from t_AssessmentBatch order by BatchID desc")
        End If
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
            ka(0) = "BatchID"
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
    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Response.Redirect("editAssessment.aspx?batchid=0")
    End Sub
End Class
