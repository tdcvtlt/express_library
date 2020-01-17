
Partial Class marketing_EditVendorRep2Tour
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim vt As New clsVendorRep2Tour
            vt.RepTourID = Request("ID")
            vt.Load()
            Dim trID As Integer = 0
            If Request("ID") = "0" Then
                trID = Request("TourID")
            Else
                trID = vt.TourID
            End If
            ddLocations.Items.Add(New ListItem("", 0))
            ddLocations.AppendDataBoundItems = True
            ddLocations.DataSource = vt.List_Locations(trID)
            ddLocations.DataTextField = "Location"
            ddLocations.DataValueField = "ID"
            ddLocations.DataBind()
            ddReps.Items.Add(New ListItem("", 0))
            ddReps.AppendDataBoundItems = True
            ddReps.DataSource = vt.List_Reps(trID)
            ddReps.DataTextField = "Rep"
            ddReps.DataValueField = "PersonnelID"
            ddReps.DataBind()


            ddReps.SelectedValue = vt.SolID
            ddLocations.SelectedValue = vt.SaleLocID
            vt = Nothing
        End If
    End Sub
    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim vt As New clsVendorRep2Tour
        vt.UserID = Session("UserDBID")
        vt.RepTourID = Request("ID")
        vt.Load()
        vt.SaleLocID = ddLocations.SelectedValue
        vt.SolID = ddReps.SelectedValue
        If vt.RepTourID = 0 Then
            vt.DateCreated = System.DateTime.Now
            vt.TourID = Request("TourID")
        End If
        vt.Save()
        vt = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Vendor();window.close();", True)

    End Sub
End Class
