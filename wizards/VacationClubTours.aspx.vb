
Partial Class wizards_VacationClubTours
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oTour As New clsTour
            gvVCTours.DataSource = oTour.get_VacationClub_Tours()
            Dim sKeys(0) As String
            sKeys(0) = "TourID"
            gvVCTours.DataKeynames = sKeys
            gvVCTours.DataBind()
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub gvVCTours_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvVCTours.RowCommand
        If e.CommandName = "Select" Then
            If CheckSecurity("Tours", "Edit", , , Session("UserDBID")) Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = gvVCTours.Rows(index)
                siTourStatus.Connection_String = Resources.Resource.cns
                siTourStatus.Label_Caption = ""
                siTourStatus.ComboItem = "TourStatus"
                siTourStatus.Load_Items()
                Dim oPrem As New clsPremiumIssued
                gvPremiums.DataSource = oPrem.Get_Prepared_Premiums(row.Cells(1).Text)
                gvPremiums.DataBind()
                oPrem = Nothing
                hfTourID.value = row.cells(1).text
                MultiView1.ActiveViewIndex = 1
            Else
                MultiView1.ActiveViewIndex = 2
            End If
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oTour As New clsTour
        Dim oPremIss As New clsPremiumIssued
        Dim oCombo As New clsComboItems
        oTour.TourID = hfTourID.Value
        oTour.Load()
        oTour.StatusID = siTourStatus.Selected_ID
        oTour.CheckedIn = False
        oTour.Save()
        For Each row As GridViewRow In gvPremiums.Rows
            Dim cb As CheckBox = row.FindControl("PremSelect")
            If cb.Checked Then
                oPremIss.PremiumIssuedID = row.Cells(1).Text
                oPremIss.Load()
                oPremIss.StatusID = oCombo.Lookup_ID("PremiumStatus", "Issued")
                oPremIss.DateIssued = System.DateTime.Now.ToShortDateString
                oPremIss.IssuedByID = Session("UserDBID")
                oPremIss.Save()
            End If
        Next
        oTour = Nothing
        oPremIss = Nothing
        oCombo = Nothing
        Response.Redirect("VacationClubTours.aspx")
    End Sub
End Class
