Imports System.IO
Imports System.Drawing

Partial Class marketing_DNC
    Inherits System.Web.UI.Page

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtNew.Text <> "" And Len(txtNew.Text) > 9 Then
            Dim oP As New clsProspect
            Dim oPP As New clsPhone
            Dim oCI As New clsComboItems
            oP.Prospect_ID = oPP.Lookup_ProspectID(Get_Number(txtNew.Text))
            If oP.Prospect_ID > 0 Then
                oP.Load()
                If oCI.Lookup_ComboItem(oP.StatusID).ToUpper = "DO NOT CALL" Or oCI.Lookup_ComboItem(oP.StatusID).ToUpper = "DONOTCALL" Then
                    'Exists
                    lblAddStatus.Text = "This number is already on the list."
                    txtCheck.Text = Get_Number(txtNew.Text)
                    Lookup()
                Else
                    Dim newid As Integer = oCI.Lookup_ID("ProspectStatus", "DO NOT CALL")
                    If newid = 0 Then newid = oCI.Lookup_ID("ProspectStatus", "DONOTCALL")
                    If newid > 0 Then
                        oP.StatusID = newid
                        oP.UserID = Session("UserDBID")
                        oP.Save()
                        lblAddStatus.Text = "The number has been added."
                        txtCheck.Text = Get_Number(txtNew.Text)
                        Lookup()
                    Else
                        'Cannot find StatusID
                        lblAddStatus.Text = "Unable to locate the appropriate record. Please contact support (Reason: Status 0 by Number)"
                    End If
                End If
            Else
                'Find the DO NOT CALL Prospect
                oP.Last_Name = "DO NOT CALL"
                oP.Load_By_LastName()
                If oP.Prospect_ID > 0 Then
                    'Add the number
                    oPP.ProspectID = oP.Prospect_ID
                    oPP.Number = Get_Number(txtNew.Text)
                    oPP.UserID = Session("UserDBID")
                    oPP.Save()
                    lblAddStatus.Text = "The number has been added."
                    txtCheck.Text = Get_Number(txtNew.Text)
                    Lookup()
                Else
                    'Cannot find the prospect record
                    lblAddStatus.Text = "Unable to locate the appropriate record. Please contact support (Reason: ProsID 0 by Last)"
                End If
            End If
            oP = Nothing
            oPP = Nothing
            oCI = Nothing
        Else
            lblAddStatus.Text = "Please enter the complete number. Format: 7573456760"
        End If
    End Sub

    Protected Sub btnCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheck.Click
        If txtCheck.Text <> "" And txtCheck.Text.Length > 9 Then
            Lookup()
        Else
            lblAddStatus.Text = "Please enter the complete number. Format: 7573456760"
        End If
    End Sub

    Private Sub Lookup()
        Dim oPP As New clsPhone
        btnExport.Enabled = False
        oPP.Number = Get_Number(txtCheck.Text)
        gvResults.DataSource = oPP.Lookup_By_Number
        gvResults.DataBind()
        lblAddStatus.Text = oPP.Error_Message
        oPP = Nothing
        MultiView1.ActiveViewIndex = 0
    End Sub
    Private Function Get_Number(ByVal sNumber As String) As String
        Dim ret As String = ""
        For i = 1 To Len(sNumber)
            If IsNumeric(Right(Left(sNumber, i), 1)) Then
                ret &= Right(Left(sNumber, i), 1)
            End If
        Next
        Return ret
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblAddStatus.Text = ""
    End Sub

    Protected Sub gvResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResults.RowDataBound
        Dim gvr As GridViewRow = e.Row
        If gvr.Cells.Count > 2 Then
            If gvr.Cells(gvr.Cells.Count - 3).Text <> "Do Not Call" And gvr.Cells(gvr.Cells.Count - 3).Text <> "FED Do Not Call" And gvr.Cells(gvr.Cells.Count - 3).Text <> "State Do Not Call" And gvr.Cells(gvr.Cells.Count - 3).Text <> "Status" Then
                e.Row.Visible = False
            Else
                If gvr.Cells(gvr.Cells.Count - 3).Text <> "Status" Then
                    btnExport.Enabled = True
                End If
            End If
        End If
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Response.ClearHeaders()
        Response.AppendHeader("Content-Disposition", "attachment; filename=DNC.xls")
        Response.ContentType = "application/vnd.ms-excel"
        Response.Write("<html><head><title>Do Not Call</title></head><body><table>")

        Response.Write("<tr>")
        For x = 0 To gvResults.HeaderRow.Cells.Count - 1
            Response.Write("<th>" & gvResults.HeaderRow.Cells(x).Text & "</th>")
        Next
        Response.Write("</tr>")

        For i = 0 To gvResults.Rows.Count - 1
            If gvResults.Rows(i).Visible Then
                Response.Write("<tr>")
                For x = 0 To gvResults.Rows(i).Cells.Count - 1
                    Response.Write("<td>" & gvResults.Rows(i).Cells(x).Text & "</td>")
                Next
                Response.Write("</tr>")
            End If
        Next
        Response.Write("</table></body></html>")
        Response.End()
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        Dim cn As New System.Data.SqlClient.SqlConnection(Resources.Resource.cns)
        Dim cm As New System.Data.SqlClient.SqlCommand("Select pp.Number from t_Prospect p inner join t_ProspectPhone pp on pp.prospectid = p.prospectid where  p.statusid in (Select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname = 'ProspectStatus' and i.comboitem in('Do Not Call','DoNotCall')) and LEN(number)=10", cn)
        Dim dr As System.Data.SqlClient.SqlDataReader
        Try
            cn.Open()
            dr = cm.ExecuteReader
            If dr.HasRows Then
                Write_Export_Header()
                While dr.Read
                    Write_Export_Line(dr("Number"))
                End While
                Write_Export_Footer()
            End If
            dr.Close()
            cn.Close()
        Catch ex As Exception
            lblAddStatus.Text = ex.ToString
        Finally
            If Not (dr Is Nothing) Then
                If Not (dr.IsClosed) Then dr.Close()
            End If
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            dr = Nothing
            cm = Nothing
            cn = Nothing
        End Try
    End Sub

    Private Sub Write_Export_Header()
        Response.ClearHeaders()
        Response.AppendHeader("Content-Disposition", "attachment; filename=DNC.csv")
        Response.ContentType = "text/plain"
        Response.Write("Number" & vbCrLf)
    End Sub

    Private Sub Write_Export_Line(ByVal sLine As String)
        Response.Write(sLine & vbCrLf)
    End Sub

    Private Sub Write_Export_Footer()
        Response.End()
    End Sub
End Class
