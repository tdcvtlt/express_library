Imports System.Data.SqlClient
Imports System.Data

Partial Class Maintenance_PMBuilding
    Inherits System.Web.UI.Page

    Protected Sub bt_submit_Click(sender As Object, e As System.EventArgs) Handles bt_submit.Click
        With New clsPMBuilding()
            .pmbuildingID = hd_pmbuildingID.Value
            .Load()
            .Name = tb_name.Text.Trim()
            .Description = tb_desc.Text.Trim()
            .Active = cb_active.Checked            
            .Save()
        End With      
        GridView_Refresh()
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GridView_Refresh()
            mvMultiView.SetActiveView(mvBuilding)
        End If
    End Sub

    Protected Sub gv_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv.RowDataBound
        Dim gvr As GridViewRow = e.Row
        Dim gv As GridView = CType(gvr.NamingContainer, GridView)
        Dim a As HtmlAnchor
        If e.Row.RowType = DataControlRowType.DataRow Then
            a = CType(gvr.FindControl("editpmbuilding"), HtmlAnchor)
            a.HRef = "#"
            a.Attributes.Add("name", e.Row.Cells(2).Text.Trim())
            a.Attributes.Add("description", e.Row.Cells(3).Text.Trim())
            a.Attributes.Add("active", Convert.ToBoolean(e.Row.Cells(4).Text))
            a.Attributes.Add("pmbuildingID", gv.DataKeys(e.Row.RowIndex).Value)

            a = CType(gvr.FindControl("linkpmbuilding"), HtmlAnchor)
            a.HRef = "#"

            a.Attributes.Add("linkpmbuilding", gv.DataKeys(e.Row.RowIndex).Value)
            a.Attributes.Add("name", e.Row.Cells(2).Text.Trim())
            a.Attributes.Add("description", e.Row.Cells(3).Text.Trim())
        End If
    End Sub

    Private Sub GridView_Refresh()        
        gv.DataSource = New clsPreventiveMaintenance().Buildings
        gv.DataBind()              
        gvRoom.DataSource = New clsPreventiveMaintenance().Rooms
        gvRoom.DataBind()        
        Using cn = New SqlConnection(Resources.Resource.cns)
            Using ad = New SqlDataAdapter("select LEFT(name, charindex(' ', name)) + ' ' + SUBSTRING(name, CHARINDEX(' ', Name), 50) name,  UnitID from t_Unit " & _
                                          "order by SUBSTRING(name, CHARINDEX(' ', Name), 50), LEFT(name, charindex(' ', name))", cn)
                Dim dt = New DataTable()
                ad.Fill(dt)
                cblSource.DataSource = dt
                cblSource.DataTextField = "Name"
                cblSource.DataValueField = "UnitID"
                cblSource.DataBind()                            
            End Using
        End Using
    End Sub

    Protected Sub toUpdatePanel1_Click(sender As Object, e As System.EventArgs) Handles toUpdatePanel1.Click

        Dim selections = From li As ListItem In cblTarget.Items _
                         Where li.Selected = True _
                         Select li

        cblSource.Items.AddRange(selections.ToArray())
        For Each li As ListItem In selections.ToArray()
            li.Selected = False
            cblTarget.Items.Remove(li)
        Next

        cblSource.Items.OfType(Of ListItem).OrderBy(Function(x) x.Text)
        Dim l As New List(Of ListItem)

        For i = cblSource.Items.Count - 1 To 0 Step -1
            cblSource.Items(i).Selected = False
        Next
        l.AddRange(cblSource.Items.OfType(Of ListItem).OrderBy(Function(x) x.Text.Substring(x.Text.IndexOf(" "))).ThenBy(Function(x) x.Text.Substring(0, x.Text.IndexOf(" "))).ToArray())

        For i = cblSource.Items.Count - 1 To 0 Step -1
            cblSource.Items.RemoveAt(i)
        Next
        cblSource.Items.AddRange(l.ToArray())
    End Sub

    Protected Sub toUpdatePanel2_Click(sender As Object, e As System.EventArgs) Handles toUpdatePanel2.Click
        Dim selections = From li As ListItem In cblSource.Items _
                         Where li.Selected = True _
                         Select li

        cblTarget.Items.AddRange(selections.ToArray())
        For Each li As ListItem In selections.ToArray()
            li.Selected = False
            cblSource.Items.Remove(li)
        Next
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click        
        Dim selected = (From li As ListItem In cblTarget.Items Select li).ToArray()
        Dim o = New clsPMBuilding()
        Dim prevLi = cblTarget.Attributes("prevLi")
        If prevLi.Length > 0 Then
            Dim ar = prevLi.Split(",")
            o.Update_PMBuildingID(hd_pmbuildingID.Value, New String() {"0"}.ToArray())
        End If
        If selected.Count() > 0 Then            
            o.Update_PMBuildingID(hd_pmbuildingID.Value, selected.Select(Function(x) x.Value).ToArray())
        End If
        btnCancel_Click(sender, e)
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        Dim selections = From li As ListItem In cblTarget.Items Select li
        cblSource.Items.AddRange(selections.ToArray())
        For Each li As ListItem In selections.ToArray()
            li.Selected = False
            cblTarget.Items.Remove(li)
        Next
        ScriptManager.RegisterClientScriptBlock(updatePanel1, updatePanel1.GetType(), DateTime.Now.ToLongTimeString(), "$('#ctl00_ContentPlaceHolder1_hd_pmbuildingID').val('0');$('#links').fadeOut('slow');", True)
    End Sub

    Protected Sub btnInit_Click(sender As Object, e As System.EventArgs) Handles btnInit.Click
        lblError.Text = DateTime.Now.ToShortDateString
        Dim ar = New clsPMBuilding().Retrieve_PMBuildingID(hd_pmbuildingID.Value)
        For Each s As String In ar
            Dim temp = s
            Dim li = cblSource.Items.OfType(Of ListItem).Single(Function(x) x.Value = temp)            
            cblTarget.Items.Add(li)            
            cblSource.Items.Remove(li)
        Next
        cblTarget.Attributes.Add("prevLi", String.Join(",", ar))
    End Sub

    Protected Sub lnkRoom_Click(sender As Object, e As System.EventArgs) Handles lnkRoom.Click
        mvMultiView.SetActiveView(mvRoom)
    End Sub

    Protected Sub lnkBuilding_Click(sender As Object, e As System.EventArgs) Handles lnkBuilding.Click
        mvMultiView.SetActiveView(mvBuilding)
    End Sub

    Protected Sub OnClickHandler(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        Select Case lnk.CommandName
            Case "select"
                Server.Transfer(String.Format("~/marketing/editroom.aspx?roomid={0}", lnk.CommandArgument), False)
        End Select
    End Sub
End Class
