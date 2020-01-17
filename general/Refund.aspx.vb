
Partial Class general_Refund
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("ProspectID") = "" Or Request("KeyField") = "" Or Request("Keyvalue") = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Closing", "var mcounter=5; var id=0; function Count_Down() { mcounter--; document.getElementById('timer').innerHTML=mcounter; if (mcounter==0)  window.close();  id=setTimeout('Count_Down()',1000); } id=setTimeout('Count_Down()',1000);", True)
            Response.Write("Missing Parameters<br />This page will close in <span id='timer'>10</span> seconds")
        End If
    End Sub
End Class
