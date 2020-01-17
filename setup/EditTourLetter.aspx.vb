Imports System.Data
Imports System.Data.SqlClient

Partial Class setup_EditTourLetter
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            mv.SetActiveView(view1)
            Dim oTourLetter = New clsTourLetters()
            With oTourLetter
                .TourLetterID = Request("ID")
                .Load()

                txtID.Text = .LetterID
                txtDesc.Text = .Description.Trim()
                txtSubject.Text = .Subject.Trim()
                txtEmail.Text = .EmailAddress.Trim()
                txtID_Hidden.Value = Request("ID")
            End With
        End If

    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) Handles LinkButton1.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, DateTime.Now.ToLongTimeString(), "modal.mwindow.open('" & Request.ApplicationPath & "/setup/LetterPicker.aspx','win01',450,450);", True)
    End Sub



    Protected Sub Letter_Link_Click(sender As Object, e As System.EventArgs) Handles Letter_Link.Click
        mv.SetActiveView(view1)
    End Sub

    Protected Sub Campaign_Link_Click(sender As Object, e As System.EventArgs) Handles Campaign_Link.Click
        mv.SetActiveView(view2)
        With New clsTourLetter2Campaign() With {.TourLetterID = txtID_Hidden.Value}
            gvCampaigns.DataSource = .List()
            gvCampaigns.DataBind()
        End With
    End Sub

    Protected Sub Location_Link_Click(sender As Object, e As System.EventArgs) Handles Location_Link.Click
        mv.SetActiveView(view3)
        With New clsTourLetter2Location() With {.TourLetterID = txtID_Hidden.Value}
            gvLocations.DataSource = .List()
            gvLocations.DataBind()
        End With
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        If txtID_Hidden.Value.Length = 0 Then Return
        With New clsTourLetters()
            .TourLetterID = Request("ID")
            .LetterID = txtID_Hidden.Value
            .Description = txtDesc.Text.Trim()
            .Subject = txtSubject.Text.Trim()
            .EmailAddress = txtEmail.Text.Trim()
            .Save()

            txtID.Text = txtID_Hidden.Value
        End With
    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click
        If txtID_Hidden.Value.Length = 0 Or txtID_Hidden.Value = "0" Then Return
        ClientScript.RegisterClientScriptBlock(Me.GetType, DateTime.Now.ToLongTimeString(), "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditTourLetterLookup.aspx?view=1&ID=0&LetterID=" & txtID_Hidden.Value & "','win01',450,450);", True)
    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        If txtID_Hidden.Value.Length = 0 Or txtID_Hidden.Value = "0" Then Return
        ClientScript.RegisterClientScriptBlock(Me.GetType, DateTime.Now.ToLongTimeString(), "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditTourLetterLookup.aspx?view=2&ID=0&LetterID=" & txtID_Hidden.Value & "','win01',450,450);", True)
    End Sub

    Protected Sub gvCampaigns_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvCampaigns.SelectedIndexChanged
        Dim row As GridViewRow = gvCampaigns.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditTourLetterLookup.aspx?view=1&ID=" & row.Cells(1).Text & "&LetterID=" & txtID_Hidden.Value & "','win01',450,450);", True)
    End Sub

    Protected Sub gvLocations_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvLocations.SelectedIndexChanged
        Dim row As GridViewRow = gvLocations.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditTourLetterLookup.aspx?view=2&ID=" & row.Cells(1).Text & "&LetterID=" & txtID_Hidden.Value & "','win01',450,450);", True)
    End Sub

    Protected Sub gvCampaigns_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCampaigns.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dv As DataView = CType(New clsTourLetter2Campaign().Campaigns.Select(DataSourceSelectArguments.Empty), DataView)
            Dim campaign As String = dv.OfType(Of DataRowView).Single(Function(x) x.Row("campaignid").ToString() = e.Row.Cells(3).Text.Trim()).Row("name").ToString()
            e.Row.Cells(3).Text = campaign
        End If
    End Sub

    Protected Sub gvLocations_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLocations.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dv As DataView = CType(New clsTourLetter2Location().Locations.Select(DataSourceSelectArguments.Empty), DataView)
            Dim location As String = dv.OfType(Of DataRowView).Single(Function(x) x.Row("comboitemid").ToString() = e.Row.Cells(3).Text.Trim()).Row("comboitem").ToString()
            e.Row.Cells(3).Text = location
        End If
    End Sub
End Class
