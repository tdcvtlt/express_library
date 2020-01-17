
Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            leads2.view = "AssignedLeads"
            'leads3.view = "MyLeadsWithoutTasks"
            tasks1.MyTasks = True
            tasks1.Completed = False
            'tasks2.MyTasks = False
            'tasks2.Completed = False
            'tasks3.MyTasks = True
            'tasks3.Completed = True
            'tasks4.MyTasks = False
            'tasks4.Completed = True
        End If
    End Sub
End Class
