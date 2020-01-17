
Imports ClosedXML.Excel
Imports System.IO
Imports System.Data.SqlClient

Partial Class PropertyManagement_Projects_EditProject
    Inherits System.Web.UI.Page

    Protected Sub Project_Link_Click(sender As Object, e As EventArgs) Handles Project_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Values_Link_Click(sender As Object, e As EventArgs) Handles Values_Link.Click
        If txtProjectID.Text > 0 Then 'Progress
            MultiView1.ActiveViewIndex = 1
            gvProgress.DataSource = (New clsProjects).Get_Progress(txtProjectID.Text)
            gvProgress.DataBind()
        End If
    End Sub

    Protected Sub Areas_Link_Click(sender As Object, e As EventArgs) Handles Areas_Link.Click
        If txtProjectID.Text > 0 Then
            MultiView1.ActiveViewIndex = 2
            gvAreas.DataSource = (New clsProject2Area).List(txtProjectID.Text)
            gvAreas.DataBind()
        End If

    End Sub

    Protected Sub Rooms_Link_Click(sender As Object, e As EventArgs) Handles Rooms_Link.Click
        If txtProjectID.Text > 0 Then
            MultiView1.ActiveViewIndex = 3
            gvRooms.DataSource = (New clsProject2Room).List(txtProjectID.Text)
            gvRooms.DataBind()
        End If
    End Sub

    Protected Sub UploadedDocs_Link_Click(sender As Object, e As EventArgs) Handles UploadedDocs_Link.Click
        If txtProjectID.Text > 0 Then
            MultiView1.ActiveViewIndex = 4
            ucDocs.KeyField = "ProjectID"
            ucDocs.KeyValue = txtProjectID.Text
            ucDocs.List()
        End If
    End Sub

    Protected Sub Events_Link_Click(sender As Object, e As EventArgs) Handles Events_Link.Click
        If txtProjectID.Text > 0 Then
            MultiView1.ActiveViewIndex = 5
            ucevents.keyfield = "ProjectID"
            ucevents.keyvalue = IIf(IsNumeric(txtProjectID.Text), IIf(CLng(txtProjectID.Text) > 0, txtProjectID.Text, 0), 0)
            ucevents.list()
        End If
    End Sub

    Protected Sub Notes_Link_Click(sender As Object, e As EventArgs) Handles Notes_Link.Click
        If txtProjectID.Text > 0 Then
            MultiView1.ActiveViewIndex = 6
            ucNotes.KeyField = "ProjectID"
            ucNotes.KeyValue = txtProjectID.Text
            ucNotes.Display()
        End If
    End Sub

    Protected Sub Personnel_Link_Click(sender As Object, e As EventArgs) Handles Personnel_Link.Click
        If txtProjectID.Text > 0 Then
            MultiView1.ActiveViewIndex = 7
            personneltrans1.keyfield = "ProjectID"
            PersonnelTrans1.KeyValue = txtProjectID.Text
            PersonnelTrans1.Load_Trans()
        End If
    End Sub

    Protected Sub UserFields_Link_Click(sender As Object, e As EventArgs) Handles UserFields_Link.Click
        If txtProjectID.Text > 0 Then
            MultiView1.ActiveViewIndex = 8
            UF.KeyField = "ProjectID"
            UF.KeyValue = CInt(txtProjectID.Text)
            UF.Load_List()
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            MultiView1.ActiveViewIndex = 0
            txtProjectID.Text = Request("ID")
            Dim oP As New clsProjects
            oP.ProjectID = txtProjectID.Text
            oP.Load()
            txtName.Text = oP.Name
            dfDateCreated.selected_date = oP.DateCreated & ""
            siStatus.Connection_String = Resources.Resource.cns
            siStatus.Label_Caption = ""
            siStatus.ComboItem = "ProjectStatus"
            siStatus.Selected_ID = oP.StatusID
            siStatus.Load_Items()
            oP = Nothing
        End If

    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If txtProjectID.Text > 0 Then
            Generate_Report()
        End If
    End Sub

    Private Sub Loop_Records(ByRef dr As SqlDataReader, ByRef ws As IXLWorksheet)
        Dim row As Integer = 1
        Dim colOffset As Integer = 2

        While dr.Read
            If row = 1 Then
                For col = 3 To dr.VisibleFieldCount
                    ws.Cell(row, col - colOffset).SetValue(dr.GetName(col - 1))
                Next
                row += 1
            End If
            For col = 3 To dr.VisibleFieldCount
                If col > 4 And col Mod 2 = 0 Then
                    ws.Cell(row, col - colOffset).AddConditionalFormat().WhenIsBlank().Fill.SetBackgroundColor(XLColor.Red)
                    ws.Column(col - colOffset).Cells.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center

                    If dr.Item(col - 1) = 0 Then
                        ws.Cell(row, col).SetValue("")
                    Else
                        ws.Cell(row, col - colOffset).SetValue("X")
                    End If
                Else
                    ws.Cell(row, col - colOffset).SetValue(dr.Item(col - 1))
                End If

            Next
            row += 1
        End While
        dr.Close()
        ws.Columns.AdjustToContents()
    End Sub

    Private Sub Generate_Report()
        Dim res As HttpResponse = Response
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim xlWB As New XLWorkbook
        cm.CommandTimeout = 10000
        cn.Open()

        cm.CommandText = "exec sp_ProjectProgress " & txtProjectID.Text
        Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add(txtName.Text))

        cn.Close()
        cn = Nothing
        cm = Nothing

        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        res.AddHeader("content-disposition", "attachment;filename=""" & txtName.Text & ".xlsx""")
        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()
    End Sub

    Protected Sub btnSave3_Click(sender As Object, e As EventArgs) Handles btnSave3.Click
        If txtName.Text <> "" Then
            Dim oP As New clsProjects
            oP.ProjectID = txtProjectID.Text
            oP.Load()
            oP.Name = txtName.Text.Replace("\", "-").Replace(".", "-")
            oP.DateCreated = IIf(dfdatecreated.selected_Date = "", Date.Now, dfdatecreated.selected_date)
            oP.StatusID = sistatus.selected_ID
            oP.Save()
            txtProjectID.Text = oP.ProjectID
            oP = Nothing
            Response.Redirect("Editproject.aspx?ID=" & txtProjectID.Text)
        End If
    End Sub

    Private Sub Items_Link_Click(sender As Object, e As EventArgs) Handles Items_Link.Click
        If txtProjectID.Text > 0 Then
            MultiView1.ActiveViewIndex = 9

        End If
    End Sub
End Class
