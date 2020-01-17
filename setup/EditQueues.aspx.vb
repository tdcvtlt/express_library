
Partial Class setup_EditQueues
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Set_Values()
        End If
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Set_Values()
        Dim oPers As New clsPersonnel
        Dim oQueue As New clsQueues
        ddRequestedBy.DataSource = oPers.List
        ddRequestedBy.DataTextField = "Name"
        ddRequestedBy.DataValueField = "ID"
        ddRequestedBy.DataBind()
        oQueue.QueueID = Request("QueueID")
        oQueue.Load()
        ddRequestedBy.SelectedValue = oQueue.RequestedByID

        txtQueueID.Text = oQueue.QueueID
        txtName.Text = oQueue.Name
        txtCreatedDate.Text = oQueue.CreatedDate
        txtDepartment.Text = oQueue.DepartmentID

        oQueue = Nothing
        oPers = Nothing
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text <> "" Then
            Dim oQueue As New clsQueues
            oQueue.QueueID = txtQueueID.Text
            oQueue.Load()
            oQueue.RequestedByID = Session("UserDBID")
            oQueue.Name = txtName.Text
            oQueue.CreatedDate = Date.Now
            oQueue.DepartmentID = txtDepartment.Text
            oQueue.Save()
            Response.Redirect("editQueues.aspx?ID=" & oQueue.QueueID)
            oQueue = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Enter a Name Value');", True)
        End If
        Set_Values()
    End Sub

    Protected Sub Scripts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Scripts.Click
        If txtQueueID.Text <> "" Then
            MultiView1.ActiveViewIndex = 1

            Dim oscript As New clsScripts
            oscript.QueueID = txtQueueID.Text
            GridView1.DataSource = oscript.Get_Queue_List(txtQueueID.Text)
            GridView1.DataBind()
            oscript = Nothing

        End If

    End Sub

    Protected Sub Notes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Notes.Click
        If txtQueueID.Text > 0 Then
            MultiView1.ActiveViewIndex = 3
            Notes1.KeyField = "QueueID"
            Notes1.KeyValue = txtQueueID.Text
            Notes1.Display()
        End If


    End Sub

    Protected Sub Triggers_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Triggers.Click
        MultiView1.ActiveViewIndex = 2
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddRequestedBy.SelectedIndexChanged

    End Sub
End Class
