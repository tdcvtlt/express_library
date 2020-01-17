
Partial Class PropertyManagement_RegCards
    Inherits System.Web.UI.Page


    Protected Sub gvRegCards_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvRegCards.SelectedIndexChanged
        Dim row As GridViewRow = gvRegCards.SelectedRow
        Response.Redirect("EditRegCard.aspx?RegCardID=" & row.Cells(1).Text)
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Response.Redirect("EditRegCard.aspx?RegCardID=0")
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oRegCard As New clsRegCards
            gvRegCards.DataSource = oRegCard.Get_Reg_Cards()
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvRegCards.DataKeyNames = sKeys
            gvRegCards.DataBind()
            oRegCard = Nothing
        End If
    End Sub
End Class
