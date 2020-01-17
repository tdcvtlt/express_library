Imports System.Data.SqlClient
Imports System.Data
Partial Class Default12
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim items = New List(Of ListItem)
        'items.Add(New ListItem With {.Text = "ABC"})
        'items.Add(New ListItem With {.Text = "DEWS"})

        'items.OrderByDescending(Function(x) x.Text).ToList()



        For Each l As ListItem In dd1.Items
            items.Add(l)
        Next
        dd1.Items.Clear()
        dd1.Items.Add("None")
        For Each l In items.OrderByDescending(Function(x) x.Text).ToList()
            dd1.Items.Add(l)
        Next


    End Sub
End Class
