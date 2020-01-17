
Imports System.Data
Imports System.Data.SqlClient

Partial Class PropertyManagement_RoomMatrix
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblErr.Text = ""
        If Not IsDate(dfStart.Text) Then dfStart.Text = Date.Today.ToShortDateString
        If Not IsPostBack Then
            If dfStart.Text = "" Then dfStart.Text = Date.Today.ToShortDateString
            Fill_Types()
            Bind_GridView()
        End If
    End Sub

    Private Sub Fill_Types()
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("select '' as Type union select distinct left(rt.comboitem,3) + ' - ' + coalesce(rst.ComboItem,'') as Type from t_Room r inner join t_ComboItems rt on rt.ComboItemID=r.TypeID left outer join t_ComboItems rst on rst.ComboItemID=r.SubTypeID", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        Try
            da.Fill(ds, "Types")
            cm.CommandText = "select '' as Comboitem union Select comboitem from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname='UnitType' and Active = 1 "
            da.Fill(ds, "UnitTypes")
            cm.CommandText = "select '' as Comboitem union select distinct left(rt.comboitem,3)  as Comboitem from t_Room r inner join t_ComboItems rt on rt.ComboItemID=r.TypeID left outer join t_ComboItems rst on rst.ComboItemID=r.SubTypeID"
            da.Fill(ds, "Size")

            ddTypes.DataSource = ds.Tables("Types")
            ddTypes.DataTextField = "Type"
            ddTypes.DataValueField = "Type"
            ddTypes.DataBind()

            ddSize.DataSource = ds.Tables("Size")
            ddSize.DataTextField = "Comboitem"
            ddSize.DataValueField = "Comboitem"
            ddSize.DataBind()

            ddUT.DataSource = ds.Tables("UnitTypes")
            ddUT.DataTextField = "Comboitem"
            ddUT.DataValueField = "Comboitem"
            ddUT.DataBind()

        Catch ex As Exception
            lblErr.Text = ex.Message
        Finally
            ds = Nothing
            da = Nothing
            cm = Nothing
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            cn = Nothing
        End Try
    End Sub

    Private Function Get_Filters() As String
        Dim ret As String = ""
        ret &= IIf(ddTypes.SelectedValue.ToString <> "", " Type='" & ddTypes.SelectedValue.ToString & "' ", "")
        ret &= IIf(ret <> "" And ddSize.SelectedValue.ToString <> "", " and ", "") & IIf(ddSize.SelectedValue.ToString <> "", " Size='" & ddSize.SelectedValue & "' ", "")
        ret &= IIf(ret <> "" And ddUT.SelectedValue.ToString <> "", " and ", "") & IIf(ddUT.SelectedValue.ToString <> "", " UnitType='" & ddUT.SelectedValue & "' ", "")
        Return ret
    End Function

    Private Sub Bind_GridView()
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("exec sp_RoomMatrix '" & dfStart.Text & "', '" & If(Year(dfStart.Text) Mod 4 = 0, CDate(dfStart.Text).AddDays(366).ToShortDateString, CDate(dfStart.Text).AddDays(365).ToShortDateString) & "'", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        Dim sFilter As String = Get_Filters()
        Try
            da.Fill(ds, "Matrix")
            If sFilter <> "" Then
                Dim dv As DataView
                dv = New DataView(ds.Tables("Matrix"), sFilter, "", DataViewRowState.CurrentRows)
                If txtRoom.Text <> "" Then FindRoom(dv)
                gvRoomMatrix.DataSource = dv
            Else
                If txtRoom.Text <> "" Then FindRoom(New DataView(ds.Tables("Matrix"), "", "", DataViewRowState.CurrentRows))
                gvRoomMatrix.DataSource = ds.Tables("Matrix")
            End If
            gvRoomMatrix.DataBind()
            If gvRoomMatrix.Rows.Count < 1 Then
                lblErr.Text = "Please check your filters and/or room allocations, since I couldn't find anything that matches."
            End If
        Catch ex As Exception
            lblErr.Text = ex.Message
        Finally
            ds = Nothing
            da = Nothing
            cm = Nothing
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            cn = Nothing
        End Try
    End Sub

    Private Function Get_TitleText(txt As String) As String
        Dim ret As String = ""
        If InStr(txt, "|") > 0 Then
            If txt.Split("|")(0) <> "-1" And txt.Split("|")(0) <> "0" Then
                ret = txt.Split("|")(0) 'Reservation ID
                ret &= "<br /> Guest: " & txt.Split("|")(5)
                ret &= " <br /> In-Date: " & CDate(txt.Split("|")(3).Substring(4, 2) & "/" & txt.Split("|")(3).Substring(6, 2) & "/" & txt.Split("|")(3).Substring(0, 4)).ToShortDateString
                ret &= " <br /> Out-Date: " & CDate(txt.Split("|")(4).Substring(4, 2) & "/" & txt.Split("|")(4).Substring(6, 2) & "/" & txt.Split("|")(4).Substring(0, 4)).ToShortDateString
            End If
        End If
        Return ret
    End Function

    Private Sub gvRoomMatrix_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvRoomMatrix.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            If e.Row.Cells(0).Text = "roomid" Then
                For i = e.Row.Cells.Count - 1 To 6 Step -1
                    e.Row.Cells(i).Text = e.Row.Cells(i).Text.Replace(" ", "<br />")
                    If InStr(e.Row.Cells(i).Text, "-") Then
                        Dim t = e.Row.Cells(i).Text.Split("-")
                        e.Row.Cells(i).Text = t(UBound(t))
                    End If
                Next
            Else
                For i = e.Row.Cells.Count - 1 To 6 Step -1
                    If InStr(e.Row.Cells(i).Text, "|") > 0 Then
                        Dim sBorder As String = "black"
                        Dim sTitle As String = Get_TitleText(e.Row.Cells(i).Text)
                        Dim cellText As String = ""
                        Dim sFontColor As String = "black"
                        If e.Row.Cells(i).Text = e.Row.Cells(i - 1).Text Then
                            If e.Row.Cells(i).Text.Split("|")(0) = "-1" Then
                                e.Row.Cells(i).BackColor = Drawing.Color.Yellow
                            ElseIf e.Row.Cells(i).Text.Split("|")(0) = "0" Then
                                e.Row.Cells(i).BackColor = Drawing.Color.White
                                If ckShowUsage.Checked Then e.Row.Cells(i).BackColor = Get_BackColor(e.Row.Cells(i).Text.Split("|")(6))
                            Else
                                If e.Row.Cells(i).Text.Split("|")(2) = "In-House" Then
                                    e.Row.Cells(i).BackColor = Drawing.Color.Purple
                                    e.Row.Cells(i).ForeColor = Drawing.Color.White
                                    sFontColor = "white"
                                ElseIf e.Row.Cells(i).Text.Split("|")(2) = "Completed" Then
                                    e.Row.Cells(i).BackColor = Drawing.Color.Black
                                    sBorder = "white"
                                    e.Row.Cells(i).ForeColor = Drawing.Color.White
                                    sFontColor = "white"
                                Else
                                    e.Row.Cells(i).BackColor = Drawing.Color.Red
                                End If
                                cellText = "<div class='tooltip'><a href='../marketing/editreservation.aspx?reservationid=" & e.Row.Cells(i).Text.Split("|")(0) & "' class='link_" & sFontColor & "'>&nbsp;</a><span class='tooltiptext'>" & sTitle & "</span></div>"
                            End If
                            e.Row.Cells(i).Text = cellText
                        Else 'New Reservation Begining
                            If e.Row.Cells(i).Text.Split("|")(0) = "-1" Then
                                e.Row.Cells(i).Text = "X"
                                e.Row.Cells(i).BackColor = Drawing.Color.Yellow
                            ElseIf e.Row.Cells(i).Text.Split("|")(0) = "0" Then
                                e.Row.Cells(i).BackColor = Drawing.Color.White
                                If ckShowUsage.Checked Then e.Row.Cells(i).BackColor = Get_BackColor(e.Row.Cells(i).Text.Split("|")(6))
                                e.Row.Cells(i).Text = ""
                            Else
                                If e.Row.Cells(i).Text.Split("|")(2) = "In-House" Then
                                    e.Row.Cells(i).BackColor = Drawing.Color.Purple
                                    e.Row.Cells(i).ForeColor = Drawing.Color.White
                                    sFontColor = "white"
                                ElseIf e.Row.Cells(i).Text.Split("|")(2) = "Completed" Then
                                    e.Row.Cells(i).BackColor = Drawing.Color.Black
                                    sBorder = "white"
                                    e.Row.Cells(i).ForeColor = Drawing.Color.White
                                    sFontColor = "white"
                                Else
                                    e.Row.Cells(i).BackColor = Drawing.Color.Red
                                End If
                                e.Row.Cells(i).Style.Add("Border-Left", "2px solid " & sBorder)
                                e.Row.Cells(i).Text = "<div class='tooltip'><a href='../marketing/editreservation.aspx?reservationid=" & e.Row.Cells(i).Text.Split("|")(0) & "'  class='link_" & sFontColor & "'>" & e.Row.Cells(i).Text.Split("|")(1).Substring(0, 1).ToUpper & "</a><span class='tooltiptext'>" & sTitle & "</span></div>"
                            End If
                        End If

                        e.Row.Cells(i).Style.Add("Border-Top", "2px solid " & sBorder)
                        e.Row.Cells(i).Style.Add("border-bottom", "2px solid " & sBorder)
                    End If
                Next
            End If
            e.Row.Cells(0).Visible = False
            e.Row.Cells(3).Visible = False
            e.Row.Cells(4).Visible = False
            e.Row.Cells(5).Visible = False
        End If

    End Sub

    Private Function Get_BackColor(type As String) As Drawing.Color
        Select Case type
            Case "Exchange"
                Return Drawing.Color.Orange
            Case "Marketing"
                Return Drawing.Color.LightPink
            Case "Rental"
                Return Drawing.Color.Blue
            Case "Owner"
                Return Drawing.Color.PaleGreen
            Case "Developer"
                Return Drawing.Color.LightBlue
            Case "Points"
                Return Drawing.Color.PaleVioletRed
            Case "TrialOwner"
                Return Drawing.Color.Green
            Case "PointsExchange"
                Return Drawing.Color.Brown
            Case Else
                Return Drawing.Color.White
        End Select
    End Function

    Protected Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        If dfStart.Text <> "" Then Bind_GridView()
    End Sub
    Protected Sub btnPrev30_Click(sender As Object, e As EventArgs) Handles btnPrev30.Click
        If dfStart.Text <> "" Then
            dfStart.Text = CDate(dfStart.Text).AddDays(-30)
            Bind_GridView()
        End If
    End Sub
    Protected Sub btnPrev7_Click(sender As Object, e As EventArgs) Handles btnPrev7.Click
        If dfStart.Text <> "" Then
            dfStart.Text = CDate(dfStart.Text).AddDays(-7)
            Bind_GridView()
        End If
    End Sub
    Protected Sub btnNext7_Click(sender As Object, e As EventArgs) Handles btnNext7.Click
        If dfStart.Text <> "" Then
            dfStart.Text = CDate(dfStart.Text).AddDays(7)
            Bind_GridView()
        End If
    End Sub
    Protected Sub btnNext30_Click(sender As Object, e As EventArgs) Handles btnNext30.Click
        If dfStart.Text <> "" Then
            dfStart.Text = CDate(dfStart.Text).AddDays(30)
            Bind_GridView()
        End If
    End Sub

    Protected Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        Bind_GridView()
    End Sub

    Private Sub gvRoomMatrix_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvRoomMatrix.PageIndexChanging
        gvRoomMatrix.PageIndex = e.NewPageIndex
        Bind_GridView()
    End Sub
    Protected Sub btnGotoRoom_Click(sender As Object, e As EventArgs) Handles btnGotoRoom.Click
        Bind_GridView()
    End Sub

    Protected Sub FindRoom(v As DataView)
        Dim rowCount As Integer = 0
        Dim page As Integer = 0
        For Each row As DataRow In v.ToTable().Rows
            rowCount += 1
            If Left(row("Room").ToString.ToUpper, Len(txtRoom.Text)) = txtRoom.Text.ToUpper Then
                While rowCount > gvRoomMatrix.PageSize
                    rowCount -= gvRoomMatrix.PageSize
                    page += 1
                End While
                gvRoomMatrix.PageIndex = page
                txtRoom.Text = ""
                Exit Sub
            End If
        Next

        lblErr.Text = txtRoom.Text + " not found"
        txtRoom.Text = ""

    End Sub
    Protected Sub ckShowUsage_CheckedChanged(sender As Object, e As EventArgs) Handles ckShowUsage.CheckedChanged
        Bind_GridView()
    End Sub
End Class
