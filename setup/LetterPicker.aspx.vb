
Partial Class setup_LetterPicker
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            With gv1
                .DataKeyNames = New String() {"LetterID"}
                .DataSource = New clsLetters().List(0)
                .DataBind()
                multiview1.SetActiveView(view1)
            End With
        End If
    End Sub

    Protected Sub gv1_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv1.RowCommand
        If e.CommandName.ToLower() = "select" Then
            Dim letterID = DirectCast(e.CommandSource, LinkButton).CommandArgument
           
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), _
                    String.Format("$(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_txtID').value={0});" & _
                                  "$(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_txtID_Hidden').value={0});window.close();", letterID), True)
        End If
    End Sub
End Class
