
Partial Class Accounting_Funding
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub
    Protected Sub btnListFunding_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnListFunding.Click
        Dim oFunding As New clsFunding
        gvFundings.DataSource = oFunding.List(0, txtFunding.Text)
        Dim sKeys(0) As String
        sKeys(0) = "FundingID"
        gvFundings.DataKeyNames = sKeys
        gvFundings.DataBind()
    End Sub

    Protected Sub btnListExitFunding_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnListExitFunding.Click
        Dim oFunding As New clsFunding
        gvExitFundings.DataSource = oFunding.List(1, txtExitFunding.Text)
        Dim sKeys(0) As String
        sKeys(0) = "FundingID"
        gvExitFundings.DataKeyNames = sKeys
        gvExitFundings.DataBind()
    End Sub

    Protected Sub gvFundings_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub gvExitFundings_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub LinkButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton2.Click
        MultiView1.ActiveViewIndex = 1
    End Sub


    Protected Sub gvFundings_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFundings.SelectedIndexChanged
        Dim row As GridViewRow = gvFundings.SelectedRow
        Response.Redirect("EditFunding.aspx?FundingID=" & row.Cells(1).Text)
    End Sub

    Protected Sub gvExitFundings_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvExitFundings.SelectedIndexChanged
        Dim row As GridViewRow = gvExitFundings.SelectedRow
        Response.Redirect("EditFunding.aspx?FundingID=" & row.Cells(1).Text)
    End Sub

    Protected Sub btnNewFunding_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewFunding.Click
        If txtFunding.Text <> "" Then
            Dim oFunding As New clsFunding
            If oFunding.Validate(txtFunding.Text) Then
                Dim newID As Integer
                oFunding.FundingID = 0
                oFunding.Load()
                oFunding.Name = txtFunding.Text
                oFunding.CreatedByID = Session("UserDBID")
                oFunding.DateCreated = System.DateTime.Now
                oFunding.Save()
                newID = oFunding.FundingID
                Response.Redirect("EditFunding.aspx?FundingID=" & newID)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Funding " & txtFunding.Text & " Already Exists.');", True)
            End If
            oFunding = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter A Funding Number.');", True)
        End If
    End Sub

    Protected Sub btnNewExitFunding_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewExitFunding.Click
        If txtExitFunding.Text <> "" Then
            Dim oFunding As New clsFunding
            If oFunding.Validate(txtExitFunding.Text) Then
                Dim newID As Integer
                oFunding.FundingID = 0
                oFunding.Load()
                oFunding.Name = txtExitFunding.Text
                oFunding.CreatedByID = Session("UserDBID")
                oFunding.DateCreated = System.DateTime.Now
                oFunding.ExitFunding = 1
                oFunding.Save()
                newID = oFunding.FundingID
                Response.Redirect("EditFunding.aspx?FundingID=" & newID)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Funding " & txtExitFunding.Text & " Already Exists.');", True)
            End If
            oFunding = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter A Funding Number.');", True)
        End If
    End Sub
End Class
