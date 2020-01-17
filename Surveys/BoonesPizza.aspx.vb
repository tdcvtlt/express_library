
Partial Class Surveys_BoonesPizza
    Inherits System.Web.UI.Page
    Dim oSurvey As clsBoonesReview

    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblToppings.SelectedIndexChanged

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Clear()
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        oSurvey = New clsBoonesReview
        With oSurvey
            .DateReviewed = Date.Now
            .Toppings = rblToppings.SelectedValue
            .Crust = rblCrust.SelectedValue
            .Sauce = rblSauce.SelectedValue
            .Cheese = rblCheese.SelectedValue
            .Presentation = rblPresentation.SelectedValue
            .Comments = tbComments.Text
            .Save()
        End With
        oSurvey = Nothing
        Clear()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Clear()
        rblCheese.ClearSelection()
        rblCrust.ClearSelection()
        rblPresentation.ClearSelection()
        rblSauce.ClearSelection()
        rblToppings.ClearSelection()
        tbComments.Text = ""
    End Sub
End Class
