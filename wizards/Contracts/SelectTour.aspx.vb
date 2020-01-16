
Partial Class wizards_Contracts_SelectTour
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ds As New SqlDataSource(Resources.Resource.cns, "Select t.TourID, p.lastname + ', ' + p.firstname as Name from t_Tour t inner join t_Prospect p on p.prospectid = t.prospectid where t.subtypeid not in (select c.comboitemid from t_Comboitems c inner join t_Combos co on c.ComboID = co.CombOID where co.Comboname = 'TourSubType' and c.comboitem = 'Exit') and t.tourdate = '" & Date.Today & "' order by p.lastname, p.firstname")
        gvTours.DataSource = ds
        Dim aKeys(0) As String
        aKeys(0) = "TourID"
        gvTours.DataKeyNames = aKeys
        gvTours.DataBind()
        lblDate.Text = CStr(Date.Today)
    End Sub

    Protected Sub gvTours_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTours.SelectedIndexChanged
        ClientScript.RegisterClientScriptBlock(Me.GetType, "SetValue", "window.opener.document.getElementById('ctl00_ContentPlaceHolder1_txtTourID').value = '" & gvTours.SelectedValue & "';", True)
        Close()
    End Sub

    Private Sub Close()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
    End Sub

End Class
