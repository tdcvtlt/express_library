
Partial Class PropertyManagement_RentalPoolLimits
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oPool As New clsRentalPoolLimits
            ddYears.DataSource = oPool.List_Limit_Years()
            ddYears.DataBind()
            oPool = Nothing
        End If
    End Sub

    Protected Sub Unnamed2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oPool As New clsRentalPoolLimits
        If oPool.Add_Pool_Year(ddYears.Items(ddYears.Items.Count - 1).Value + 1) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Receipt", "alert('" & ddYears.Items(ddYears.Items.Count - 1).Value + 1 & " Added');", True)
            ddYears.DataSource = oPool.List_Limit_Years()
            ddYears.DataBind()
        Else
            lblErr.Text = oPool.Error_Message
        End If
        oPool = Nothing
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oPool As New clsRentalPoolLimits
        gvPoolLimits.DataSource = oPool.List_Limits_By_Year(ddYears.SelectedValue)
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvPoolLimits.DataKeyNames = sKeys
        gvPoolLimits.DataBind()
        oPool = Nothing
        If CheckSecurity("Rentals", "SetPoolLimits", , , Session("UserDBID")) Then
            btnSave.Visible = True
        End If
    End Sub

    Protected Sub gvPoolLimits_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                Dim tb As TextBox
                tb = e.Row.FindControl("txtQtyAllowed")
                tb.Text = e.Row.Cells(4).Text
            End If
            e.Row.Cells(0).Visible = False
            e.Row.Cells(4).Visible = False
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        For Each row As GridViewRow In gvPoolLimits.Rows
            Dim tb As TextBox = row.FindControl("txtQtyAllowed")
            If Not (isNumeric(tb.Text)) Or tb.Text = "" Then
                bProceed = False
                sErr = "Please Make Sure the Qty Allowed is a Valid Number."
                Exit For
            End If
        Next

        If bProceed Then
            Dim oPool As New clsRentalPoolLimits
            oPool.UserID = Session("UserDBID")
            For Each row As GridViewRow In gvPoolLimits.Rows
                Dim tb As TextBox = row.FindControl("txtQtyAllowed")
                oPool.RentalPoolID = row.Cells(0).Text

                oPool.Load()
                oPool.Qty = tb.Text
                oPool.Save()
            Next
            oPool = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "aler", "alert('" & sErr & "');", True)
        End If
    End Sub
End Class
