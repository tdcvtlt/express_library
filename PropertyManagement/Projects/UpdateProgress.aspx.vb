
Imports System.Data

Partial Class PropertyManagement_Projects_UpdateProgress
    Inherits System.Web.UI.Page

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim counter As Integer = 0
        For Each row As TableRow In tblAreas.Rows
            If row.Cells(0).Text <> "ID" Then
                Dim oDet As New clsProjectValues
                oDet.Load_Details(Request("PID"), Request("RoomID"), row.Cells(0).Text)
                If (oDet.ProjectStatusID = 0 And (CType(row.Cells(2).FindControl("value" & counter), CheckBox).Checked Or CType(row.Cells(3).FindControl("comments" & counter), TextBox).Text <> "")) Then
                    oDet.RoomID = Request("RoomID")
                    oDet.ProjectID = Request("PID")
                    oDet.AreaID = row.Cells(0).Text
                    oDet.Value = IIf(CType(row.Cells(2).FindControl("value" & counter), CheckBox).Checked, 1, 0)
                    oDet.Comments = CType(row.Cells(3).FindControl("comments" & counter), TextBox).Text
                    oDet.UserID = Session("UserDBID")
                    oDet.DateUpdated = Date.Now
                    oDet.Save()
                ElseIf oDet.ProjectStatusID > 0 And (CType(row.Cells(2).FindControl("value" & counter), CheckBox).Checked <> IIf(oDet.Value = 1, True, False) Or oDet.Comments <> CType(row.Cells(3).FindControl("comments" & counter), TextBox).Text) Then
                    oDet.RoomID = Request("RoomID")
                    oDet.ProjectID = Request("PID")
                    oDet.AreaID = row.Cells(0).Text
                    oDet.Value = IIf(CType(row.Cells(2).FindControl("value" & counter), CheckBox).Checked, 1, 0)
                    oDet.Comments = CType(row.Cells(3).FindControl("comments" & counter), TextBox).Text
                    oDet.UserID = Session("UserDBID")
                    oDet.DateUpdated = Date.Now
                    oDet.Save()
                End If
                oDet = Nothing
                counter += 1
            End If
        Next

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "close", "window.opener.Refresh_Progress();window.close();", True)

    End Sub

    Private Sub PropertyManagement_Projects_UpdateProgress_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Not (IsPostBack) Then
        Dim oP As New clsProjects
        oP.ProjectID = Request("PID")
        oP.Load()
        Dim oRooms As New clsRooms
        oRooms.RoomID = Request("RoomID")
        oRooms.Load()

        lblProject.Text = oP.Name
        lblRoom.Text = oRooms.RoomNumber
        oP = Nothing
        oRooms = Nothing

        Dim dtAreas As New System.Data.DataTable
        Dim ds As SqlDataSource = (New clsProject2Area).List(Request("PID"))
        Dim view As DataView = ds.Select(New DataSourceSelectArguments)
        dtAreas = view.ToTable
        view = Nothing
        ds = Nothing

        Dim thr As New TableHeaderRow
        Dim counter As Integer = 0
        thr.Cells.Add(New TableHeaderCell With {.Text = "ID"})
        thr.Cells.Add(New TableHeaderCell With {.Text = "Area"})
        thr.Cells.Add(New TableHeaderCell With {.Text = "Complete"})
        thr.Cells.Add(New TableHeaderCell With {.Text = "Comments"})
        tblAreas.Rows.Add(thr)

        thr = Nothing
        For Each row As DataRow In dtAreas.Rows
            Dim tr As New TableRow
            Dim oDet As New clsProjectValues
            oDet.Load_Details(Request("PID"), Request("RoomID"), row("AreaID"))
            tr.Cells.Add(New TableCell With {.Text = row("AreaID")})
            tr.Cells.Add(New TableCell With {.Text = row("Area")})
            Dim tc As New TableCell
            tc.Controls.Add(New CheckBox With {.ID = "value" & counter, .Checked = IIf(oDet.Value = 1, True, False)})
            tr.Cells.Add(tc)
            tc = New TableCell
            tc.Controls.Add(New TextBox With {.ID = "comments" & counter, .Text = oDet.Comments})
            tr.Cells.Add(tc)
            tr.EnableViewState = True
            tc = Nothing
            oDet = Nothing
            tblAreas.Rows.Add(tr)
            tr = Nothing
            counter += 1
        Next

        'End If
    End Sub
End Class
