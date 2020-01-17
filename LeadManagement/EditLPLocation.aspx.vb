
Partial Class LeadManagement_EditLPLocation
    Inherits System.Web.UI.Page


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim oLP As New clsLeadProgram2Location
        oLP.ID = Request("ID")
        oLP.Load()
        oLP.UserID = Session("UserDBID")
        oLP.LocationID = siLocation.Selected_ID
        oLP.StartDate = dteSDate.Selected_Date
        oLP.EndDate = dteEDate.Selected_Date
        oLP.Active = cbActive.Checked
        If Request("ID") = 0 Then
            oLP.LeadProgramID = Request("LPID")
            oLP.Version = 1.0
        End If
        oLP.Save()
        oLP = Nothing
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.Refresh_Locations();window.close();", True)

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then

            Dim oLP As New clsLeadProgram2Location
            oLP.ID = Request("ID")
            oLP.Load()
            siLocation.Connection_String = Resources.Resource.cns
            siLocation.Label_Caption = ""
            siLocation.ComboItem = "LeadProgramLocation"
            siLocation.Selected_ID = oLP.LocationID
            siLocation.Load_Items()
            siLocation.Selected_ID = oLP.LocationID
            dteSDate.Selected_Date = oLP.StartDate
            dteEDate.Selected_Date = oLP.EndDate
            cbActive.Checked = oLP.Active
            txtVersion.Text = oLP.Version
            oLP = Nothing
        End If
    End Sub
End Class
