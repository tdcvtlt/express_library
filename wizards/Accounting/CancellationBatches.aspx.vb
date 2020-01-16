
Imports System.Data.SqlClient

Partial Class wizards_Accounting_CancellationBatches
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            List()
        End If
    End Sub

    Private Sub List()
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select top 100 BatchID, i.comboitem as Type, s.Comboitem as Status, Convert(VARCHAR(8), HearingDate,1) as HearingDate, p.Username as [Created By] from t_CancellationBatch b inner join t_Comboitems i on i.comboitemid=b.typeid left outer join t_Comboitems s on s.comboitemid=b.statusid left outer join t_Personnel p on p.personnelid=b.CreatedByID order by batchid desc", cn)
        If txtQuery.Text <> "" Then
            cm.CommandText = "Select top 100 BatchID, i.comboitem as Type, s.Comboitem as Status, Convert(VARCHAR(8), HearingDate,1) as HearingDate, p.Username as [Created By] from t_CancellationBatch b inner join t_Comboitems i on i.comboitemid=b.typeid left outer join t_Comboitems s on s.comboitemid=b.statusid left outer join t_Personnel p on p.personnelid=b.CreatedByID where BatchID = '" & txtQuery.Text & "' order by batchid desc"
        End If
        cn.Open()
        gvBatches.DataSource = cm.ExecuteReader
        gvBatches.DataBind()
        cn.Close()
        cm = Nothing
        cn = Nothing
    End Sub
    Protected Sub gvBatches_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvBatches.SelectedIndexChanged

    End Sub
    Protected Sub btnQuery_Click(sender As Object, e As EventArgs) Handles btnQuery.Click
        List()
    End Sub
    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Response.Redirect("EditCancellationBatch.aspx?bid=0")
    End Sub
End Class
