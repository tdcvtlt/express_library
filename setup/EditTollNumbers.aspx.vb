
Partial Class setup_EditTollNumbers
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Set_Values()
        End If
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Set_Values()
        Dim oTollNumbers As New clsTollNumbers
        oTollNumbers.TollID = Request("TollID")
        oTollNumbers.Load()

        txtCarrier.Text = oTollNumbers.CarrierID
        txtTollNumber.Text = oTollNumbers.TollNumber
        txtTollID.Text = oTollNumbers.TollID
    End Sub

    Protected Sub DID_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DID.Click
        MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub Triggers_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Triggers.Click
        MultiView1.ActiveViewIndex = 2
    End Sub

    Protected Sub Notes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Notes.Click
        MultiView1.ActiveViewIndex = 3
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtTollNumber.Text <> "" Then
            Dim oToll As New clsTollNumbers
            oToll.TollID = txtTollID.Text
            oToll.Load()
            oToll.CarrierID = txtCarrier.Text
            oToll.TollNumber = txtTollNumber.Text
            oToll.Save()
            Response.Write(txtTollNumber.Text & " " & oToll.TollNumber)
            'Response.Redirect("editTollNumbers.aspx?ID=" & oToll.TollID)
            oToll = Nothing
            'ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('request(oToll.TollID)');", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Enter a Toll Number Value');", True)

        End If

    End Sub
End Class
