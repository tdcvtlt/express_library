
Partial Class Reports_SalesRotor
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oRotor As New clsSalesRotor
            SalesRotor.Text = oRotor.Build_Rotor("Dayline 1", False, True) & "</table>"
            lblErr.Text = oRotor.Err
            oRotor = Nothing
        End If
    End Sub

    Protected Sub InHouse_Link_Click(sender As Object, e As System.EventArgs) Handles InHouse_Link.Click
        Dim oRotor As New clsSalesRotor
        SalesRotor.Text = oRotor.Build_Rotor("In-House", False, True) & "</table>"
        lblErr.Text = oRotor.Err
        oRotor = Nothing
    End Sub

    Protected Sub DayLine1_Link_Click(sender As Object, e As System.EventArgs) Handles DayLine1_Link.Click
        Dim oRotor As New clsSalesRotor
        SalesRotor.Text = oRotor.Build_Rotor("Dayline 1", False, True) & "</table>"
        lblErr.Text = oRotor.Err
        oRotor = Nothing
    End Sub

    Protected Sub DayLine1Weekly_Link_Click(sender As Object, e As System.EventArgs) Handles DayLine1Weekly_Link.Click
        Dim oRotor As New clsSalesRotor
        SalesRotor.Text = oRotor.Build_Rotor("Dayline 1", True, True) & "</table>"
        lblErr.Text = oRotor.Err
        oRotor = Nothing
    End Sub

    Protected Sub InHouseWeekly_Link_Click(sender As Object, e As System.EventArgs) Handles InHouseWeekly_Link.Click
        Dim oRotor As New clsSalesRotor
        SalesRotor.Text = oRotor.Build_Rotor("In-House", True, True) & "</table>"
        lblErr.Text = oRotor.Err
        oRotor = Nothing
    End Sub

    Protected Sub NOVA_Link_Click(sender As Object, e As System.EventArgs) Handles NOVA_Link.Click
        Server.ScriptTimeout = 10000
        Dim oRotor As New clsSalesRotor
        SalesRotor.Text = oRotor.Build_Rotor("Offsite Sales", False, False) & "</table>"
        lblErr.Text = oRotor.Err
        oRotor = Nothing
    End Sub

    Protected Sub NOVAWeekly_Link_Click(sender As Object, e As System.EventArgs) Handles NOVAWeekly_Link.Click
        Server.ScriptTimeout = 10000
        Dim oRotor As New clsSalesRotor
        SalesRotor.Text = oRotor.Build_Rotor("Offsite Sales", True, False) & "</table>"
        lblErr.Text = oRotor.Err
        oRotor = Nothing
    End Sub
End Class
