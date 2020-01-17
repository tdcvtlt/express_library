
Partial Class setup_EditRateTable
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oRateTable As New clsRateTable
            oRateTable.RateTableID = Request("ID")
            oRateTable.Load()
            txtID.Text = oRateTable.RateTableID
            txtName.Text = oRateTable.Name
            oRateTable = Nothing
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If txtName.Text <> "" Then
            Dim oRateTable As New clsRateTable
            oRateTable.RateTableID = txtID.Text
            oRateTable.Load()
            oRateTable.UserID = Session("UserDBID")
            oRateTable.Name = txtName.Text
            oRateTable.Save()
            Response.Redirect("editRateTable.aspx?ID=" & oRateTable.RateTableID)
            oRateTable = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Enter a Name Value');", True)
        End If
    End Sub
End Class
