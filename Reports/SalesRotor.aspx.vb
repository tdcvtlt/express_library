
Partial Class Reports_SalesRotor
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim oRotor As New clsSalesRotor
        SalesRotor.Text = oRotor.Build_Rotor("", False, False)
        lblErr.Text = oRotor.Err
        oRotor = Nothing
    End Sub
End Class
