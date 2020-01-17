
Partial Class setup_Letters_LetterTags
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("TypeID") = "" Or Not (IsNumeric(Request("TypeID"))) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
        Else
            GridView1.DataSource = (New clsLetterTags).List(Request("TypeID"))
            GridView1.DataBind()
        End If

    End Sub
End Class
