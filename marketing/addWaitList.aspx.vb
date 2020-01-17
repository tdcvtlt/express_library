
Partial Class marketing_addWaitList
    Inherits System.Web.UI.Page

    Protected Sub ddFilter_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddFilter.SelectedIndexChanged
        If ddFilter.Selectedvalue = "Phone" Then
            lblFilter.Text = "Enter Phone Number:"
        ElseIf ddFilter.SelectedValue = "Name" Then
            lblFilter.Text = "Enter Name:"
        Else
            lblFilter.Text = "Enter Contract Number:"
        End If
        txtFilter.Text = ""
        txtFilter.Focus()
        gvResults.Visible = False
        MultiView1.ActiveViewindex = 0
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            ddFilter.Items.Add("Phone")
            ddFilter.Items.Add("Name")
            ddFilter.Items.Add("Contract")
            ddBD.Items.Add(1)
            ddBD.Items.Add(2)
            ddBD.Items.Add(3)
            ddBD.Items.Add(4)
            siSeason.Connection_String = Resources.Resource.cns
            siSeason.ComboItem = "Season"
            siSeason.Label_Caption = ""
            siSeason.Load_Items()
            siUnitType.Connection_String = Resources.Resource.cns
            siUnitType.ComboItem = "UnitType"
            siUnitType.Label_Caption = ""
            siUnitType.Load_Items()
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oWaitList As New clsReservationWaitlist
        gvResults.DataSource = oWaitList.Con_Filter(ddFilter.SelectedValue, txtFilter.Text)
        gvResults.DataBind()
        oWaitList = Nothing
        gvResults.Visible = True
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub gvResults_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvResults.SelectedIndexChanged
        Dim row As GridViewRow = gvResults.SelectedRow
        Dim oPros As New clsProspect
        oPros.Prospect_ID = row.Cells(1).Text
        oPros.Load()
        lblProspect.Text = oPros.First_Name & " " & oPros.Last_Name
        oPros = Nothing
        Dim oContract As New clsContract
        ddContract.DataSource = oContract.List_Pros_Contracts(row.Cells(1).text)
        ddContract.DataTextField = "ContractNumber"
        ddContract.DataValueField = "ContractID"
        ddContract.DataBind()
        hfProsID.Value = row.cells(1).Text
        MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        If siSeason.Selected_ID < 1 Or siunitType.Selected_ID < 1 Or dteStartDate.Selected_Date = "" Or dteEndDate.Selected_Date = "" Then
            bProceed = False
            sErr = "Please Fill In All Fields."
        End If
        If bProceed Then
            If Date.Compare(CDate(dteStartDate.Selected_Date), CDate(dteEndDate.Selected_Date)) >= 0 Then
                bProceed = False
                sErr = "Start Date Must Be Earlier Than End Date."
            End If
        End If
        If bProceed = True Then
            Dim oWaitList As New clsReservationWaitlist
            oWaitList.WaitListID = 0
            oWaitList.Load()
            oWaitList.ProspectID = hfProsID.Value
            oWaitList.ContractID = ddContract.SelectedValue
            oWaitList.StartDate = dteStartDate.Selected_Date
            oWaitList.EndDate = dteEndDate.Selected_Date
            oWaitList.UnitTypeID = siUnitType.Selected_ID
            oWaitList.SeasonID = siSeason.Selected_ID
            oWaitList.CreatedByID = Session("UserDBID")
            oWaitList.DateCreated = System.DateTime.Now
            oWaitList.BedRooms = ddBD.SelectedValue
            oWaitList.Active = True
            oWaitList.Save()
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub gvResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub
End Class
