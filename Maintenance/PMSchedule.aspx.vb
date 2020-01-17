
Partial Class Maintenance_PMSchedule
    Inherits System.Web.UI.Page


    Protected Sub Get_nextPM_Date()

    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Populate_DDItems()
        End If
    End Sub

    Protected Sub Populate_DDItems()
        Dim ddItem As ListItem
        Dim ddItem2 As ListItem
        Dim ddItem3 As ListItem

        Dim a() As Integer = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}



        DropDownList1.Items.Clear()
        DropDownList2.Items.Clear()
        DropDownList3.Items.Clear()

        ddItem = New ListItem
        ddItem.Text = "Weekly"
        ddItem.Value = "Weekly"
        DropDownList1.Items.Add(ddItem)

        ddItem = New ListItem
        ddItem.Text = "Monthly"
        ddItem.Value = "Monthly"
        DropDownList1.Items.Add(ddItem)

        ddItem = New ListItem
        ddItem.Text = "Quarterly"
        ddItem.Value = "Quarterly"
        DropDownList1.Items.Add(ddItem)

        ddItem = New ListItem
        ddItem.Text = "Annually"
        ddItem.Value = "Annually"
        DropDownList1.Items.Add(ddItem)

        ddItem2 = New ListItem
        ddItem2.Text = "All"
        ddItem2.Value = "All"
        DropDownList2.Items.Add(ddItem2)

        ddItem2 = New ListItem
        ddItem2.Text = "Something"
        ddItem2.Value = "Something"
        DropDownList2.Items.Add(ddItem2)

        ddItem2 = New ListItem
        ddItem2.Text = "Something Else"
        ddItem2.Value = "Something Else"
        DropDownList2.Items.Add(ddItem2)

        ddItem3 = New ListItem
        ddItem3.Text = "All"
        ddItem3.Value = "All"
        DropDownList3.Items.Add(ddItem3)

        ddItem3 = New ListItem
        ddItem3.Text = "Something"
        ddItem3.Value = "Something"
        DropDownList3.Items.Add(ddItem3)

        ddItem3 = New ListItem
        ddItem3.Text = "Something Else"
        ddItem3.Value = "Something Else"
        DropDownList3.Items.Add(ddItem3)
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        If DropDownList1.SelectedValue = "Quarterly" Then
            Response.Write("Adrian We Did It")
        Else
            Response.Write("I think I messed up!")
        End If
    End Sub

    Protected Sub Do_Something()
        'This is where I do something
    End Sub
End Class
