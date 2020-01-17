Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Web


Partial Class PropertyManagement_EditCommentCards
    Inherits System.Web.UI.Page

    Dim oCommentCards As New clsCommentCards
    Dim oCommentCardFields As New clsCommentCardFields
    Dim oCommentCardFieldValues As New clsCommentCardFieldValues


#Region "Global Variables"

    Public CARDID As Integer = 0
    Public PROSPECTID As Integer = 0
    Public ROOMID As Integer = 0

    Private Structure PageCardFields
        Public Hidden As HiddenField
        Public Caption As Label
        Public Selected As CheckBox
        Public Required As CheckBox
        Public Text As TextBox
    End Structure

    Private TableColumnHeader() As String = New String() {"FrontDesk", "HouseKeeping", _
                                                          "ResortAmmentities", "Maintenance", "Hospitality", _
                                                          "UnitQuality", "SPA", "OverAllEval"}
    Private PageColumnHeader() As String = New String() {"Front Desk", "House Keeping", "Resort Ammenities", _
                                                        "Maintenance", "Hospitality", "Unit Quality", _
                                                        "SPA", "Overall Eval"}

    Private PageColumnText() As String = New String() {"Area", "EX", "AA", "Ave", "BA", "Poor"}

    Private PageCardFieldsDataRow As IDictionary(Of Integer, PageCardFields) = New Dictionary(Of Integer, PageCardFields)

    Private PageRadioButtonList As IDictionary(Of String, RadioButtonList) = New Dictionary(Of String, RadioButtonList)

#End Region

#Region "Events & Handlers"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim htmTable As New HtmlTable

        htmTable.ID = "htmRadioButtonListTable"

        Dim rowText As New HtmlTableRow()
        rowText.Cells.Add(New HtmlTableCell With {.InnerText = PageColumnText(0)})
        rowText.Cells.Add(New HtmlTableCell With {.InnerText = PageColumnText(1) & Server.HtmlDecode("&nbsp;&nbsp;&nbsp;") & PageColumnText(2) & Server.HtmlDecode("&nbsp;&nbsp;&nbsp;") & PageColumnText(3) & Server.HtmlDecode("&nbsp;&nbsp;&nbsp;") & PageColumnText(4) & Server.HtmlDecode("&nbsp;&nbsp;&nbsp;") & PageColumnText(5)})

        htmTable.Rows.Add(rowText)

        '8 rows of RadioButtonList by 6 columns
        For i As Integer = 0 To TableColumnHeader.Length - 1

            Dim rbl As New RadioButtonList With {.ID = TableColumnHeader(i), .RepeatDirection = RepeatDirection.Horizontal}
            rbl.Items.AddRange(New ListItem() {New ListItem With {.Text = "&nbsp;&nbsp;&nbsp;", .Value = "1"}, _
                                              New ListItem With {.Text = "&nbsp;&nbsp;&nbsp;", .Value = "2"}, _
                                              New ListItem With {.Text = "&nbsp;&nbsp;&nbsp;", .Value = "3"}, _
                                              New ListItem With {.Text = "&nbsp;&nbsp;&nbsp;", .Value = "4"}, _
                                              New ListItem With {.Text = "&nbsp;&nbsp;&nbsp;", .Value = "5"}})
            PageRadioButtonList.Add(TableColumnHeader(i), rbl)

            Dim htmRow As HtmlTableRow = New HtmlTableRow()

            htmRow.Cells.Add(New HtmlTableCell With {.InnerText = PageColumnHeader(i) + Server.HtmlDecode("&nbsp;&nbsp;&nbsp;")})


            Dim htmCell As New HtmlTableCell()
            htmCell.Controls.Add(rbl)

            htmRow.Cells.Add(htmCell)

            htmTable.Rows.Add(htmRow)
        Next

        phContainer.Controls.Add(htmTable)


        'Retrieve all card field rows
        Using cnn As New SqlConnection(Resources.Resource.cns)
            Using cmd As New SqlCommand("select * from t_CommentCardFields order by FieldName", cnn)

                cnn.Open()
                Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If rdr.HasRows = True Then

                    Do While rdr.Read()
                        Dim field As New PageCardFields With { _
                            .Hidden = New HiddenField With {.ID = "Hidden" + rdr("FieldID").ToString()}, _
                            .Caption = New Label With {.Text = rdr("FieldName")}, _
                            .Selected = New CheckBox With {.ID = "Selected" + rdr("FieldID").ToString()}, _
                            .Required = New CheckBox With {.ID = "Required" + rdr("FieldID").ToString()}, _
                            .Text = New TextBox With {.ID = "Text" + rdr("FieldID").ToString(), .Width = Unit.Pixel(340)}}

                        PageCardFieldsDataRow.Add(Convert.ToInt32(rdr("FieldID").ToString()), field)
                    Loop

                    phContainer.Controls.Add(New HtmlGenericControl("hr"))

                    htmTable = New HtmlTable()

                    Dim keys(PageCardFieldsDataRow.Count - 1) As Integer
                    PageCardFieldsDataRow.Keys.CopyTo(keys, 0)


                    Dim words() As String = New String() {"Flower", "July", "Autumn", "December"}

                  


                    For Each i As Integer In keys
                        Dim htmRow As New HtmlTableRow()
                        Dim htmCell As HtmlTableCell = Nothing

                        htmRow.Cells.Add(New HtmlTableCell With {.Width = 0, .InnerText = PageCardFieldsDataRow(i).Caption.Text.Trim()})

                        htmCell = New HtmlTableCell()
                        htmCell.Controls.Add(PageCardFieldsDataRow(i).Selected)
                        htmRow.Cells.Add(htmCell)

                        htmCell = New HtmlTableCell()
                        htmCell.Controls.Add(PageCardFieldsDataRow(i).Required)
                        htmRow.Cells.Add(htmCell)

                        htmCell = New HtmlTableCell()
                        htmCell.Controls.Add(PageCardFieldsDataRow(i).Text)
                        htmRow.Cells.Add(htmCell)

                        htmTable.Rows.Add(htmRow)
                    Next
                    phContainer.Controls.Add(htmTable)

                End If
            End Using
        End Using



    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If String.IsNullOrEmpty(Request.QueryString("cardid")) = False Then
            CARDID = Request.QueryString("cardid")
        End If



        If Page.IsPostBack = True Then


        Else
            If Not Request.UrlReferrer Is Nothing Then
                If String.IsNullOrEmpty(hlBack.NavigateUrl) = True Then
                    hlBack.NavigateUrl = Request.UrlReferrer.PathAndQuery
                    hlBack.Text = "Back"
                End If
            Else
                hlBack.Visible = False
            End If

            If String.IsNullOrEmpty(Request.QueryString("cardid")) = False Then
                CARDID = Request.QueryString("cardid")

                GetCardFieldValue(CARDID)
            End If


        End If




        'Prevent this block from running
        If Page.IsPostBack = False And Page.IsPostBack = True Then
            oCommentCards.Load()
            oCommentCardFields.Load()
            oCommentCardFieldValues.Load()

            If Request("CardID") = 0 Then
                'Set_Values(0)
            Else
                Set_Values(Request("CardID"))
            End If
        End If

    End Sub

    Protected Sub btnFields_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFields.Click


        Return

        'Build_Report(Request("CardID"))
    End Sub

    Protected Sub btnAddField_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddField.Click
        Dim CardID As String = Request("CardID")
        If CardID = 0 Then
            Response.Redirect("AddField.aspx?Fieldid =0")
        Else
            Response.Redirect("AddField.aspx?Fieldid =" & CardID)
        End If
    End Sub

    Protected Sub btnGuest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuest.Click
        Dim CardID As String = Request("CardID")
        Dim sSQL As String = ""
        If CardID = 0 Then
            If txtGuest.Text = "" Then
                ' Page.RegisterStartupScript("alert", "<script language='javascript'>Get_Window('You Must Enter a Guest Name');</script>")
            Else
                Dim ds As New System.Web.UI.WebControls.SqlDataSource
                ds.ConnectionString = Resources.Resource.cns
                If InStr(txtGuest.Text, ",") Then
                    ds.SelectCommand = "Select top 500 p.prospectid, r.roomid, p.lastname, p.firstname, r.roomnumber, res.checkoutdate from t_Prospect p inner join t_Reservations res on res.prospectid = p.prospectid inner join (Select distinct Roomid, reservationid from t_RoomallocationMatrix) m on m.reservationid = res.reservationid inner join t_Room r on r.roomid = m.roomid where ltrim(rtrim(p.lastname)) = '" & Left(txtGuest.Text, InStr(txtGuest.Text, ",") - 1) & "' and ltrim(rtrim(p.firstname)) like '" & Trim(Right(txtGuest.Text, Len(txtGuest.Text) - InStr(txtGuest.Text, ","))) & "%' order by p.lastname, p.firstname, r.roomnumber, res.checkoutdate desc"
                Else
                    ds.SelectCommand = "Select top 500 p.prospectid, r.roomid, p.lastname, p.firstname, r.roomnumber, res.checkoutdate from t_Prospect p inner join t_Reservations res on res.prospectid = p.prospectid inner join (Select distinct Roomid, reservationid from t_RoomallocationMatrix) m on m.reservationid = res.reservationid inner join t_Room r on r.roomid = m.roomid where ltrim(rtrim(p.lastname)) like '" & txtGuest.Text & "%' order by res.checkoutdate desc"
                End If
                Session("RoomSet") = ds
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/propertymanagement/SearchGuestRoom.aspx?CardID=" & Request("CardID") & "','win01',690,450);", True)
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/propertymanagement/SearchGuestRoom.aspx?CardID=" & Request("CardID") & "','win01',690,450);", True)
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim cph As ContentPlaceHolder = DirectCast(Me.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        Dim container As PlaceHolder = DirectCast(cph.FindControl("phContainer"), PlaceHolder)
        Dim hidden As HiddenField = DirectCast(container.FindControl("ValueIdFieldId"), HiddenField)



        Dim cnx As String = Resources.Resource.cns

        If (CARDID = 0) Then

            If (String.IsNullOrEmpty(txtGuestID.Value) Or String.IsNullOrEmpty(txtRoomID.Value)) Then Return

            Dim prospectid As Integer = txtGuestID.Value
            Dim roomid As Integer = txtRoomID.Value


            Using cnn As New SqlConnection(cnx)
                Dim sql As String = String.Format("INSERT INTO T_COMMENTCARDS (ProspectId, RoomId, CardDate, " & _
                "DateEntered, EnteredById, FrontDesk, HouseKeeping, ResortAmmentities, " & _
                "Maintenance, Hospitality, UnitQuality, Spa, OverAllEval) VALUES ({0}, {1}, " & _
                "{2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12});SELECT @@IDENTITY", _
                prospectid, roomid, "GETDATE()", "GETDATE()", Session("USERDBID"), _
                IIf(PageRadioButtonList("FrontDesk").SelectedIndex > -1, PageRadioButtonList("FrontDesk").SelectedIndex + 1, "null"), _
                IIf(PageRadioButtonList("HouseKeeping").SelectedIndex > -1, PageRadioButtonList("HouseKeeping").SelectedIndex + 1, "null"), _
                IIf(PageRadioButtonList("ResortAmmentities").SelectedIndex > -1, PageRadioButtonList("ResortAmmentities").SelectedIndex + 1, "null"), _
                IIf(PageRadioButtonList("Maintenance").SelectedIndex > -1, PageRadioButtonList("Maintenance").SelectedIndex + 1, "null"), _
                IIf(PageRadioButtonList("Hospitality").SelectedIndex > -1, PageRadioButtonList("Hospitality").SelectedIndex + 1, "null"), _
                IIf(PageRadioButtonList("UnitQuality").SelectedIndex > -1, PageRadioButtonList("UnitQuality").SelectedIndex + 1, "null"), _
                IIf(PageRadioButtonList("SPA").SelectedIndex > -1, PageRadioButtonList("SPA").SelectedIndex + 1, "null"), _
                IIf(PageRadioButtonList("OverAllEval").SelectedIndex > -1, PageRadioButtonList("OverAllEval").SelectedIndex + 1, "null"))


                Dim cmd As New SqlCommand(sql, cnn)
                cnn.Open()

                Dim j As Integer = 0
                Dim cardPkId As Integer = 0
                If j = 0 Then
                    Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    'Retrieve newly inserted row from the T_CommentCards
                    Do While (rdr.Read())
                        cardPkId = Convert.ToInt32(rdr.GetValue(0).ToString())
                        CARDID = cardPkId
                    Loop

                    rdr.Close()
                End If

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''Insert new note
                update_Click(DBNull.Value, EventArgs.Empty)



                Dim selectedValue As IEnumerable(Of CheckBox) = PageCardFieldsDataRow _
                                                                .Where(Function(x) x.Value.Selected.Checked = True) _
                                                                .Select(Function(x) x.Value.Selected)
                cnn.Open()

                If selectedValue.Count() > 0 Then
                    For Each cb As CheckBox In selectedValue

                        Dim pk As String = cb.ID.Substring(("Selected").Length)
                        Dim required As Boolean = PageCardFieldsDataRow(Integer.Parse(pk)).Required.Checked
                        Dim text As String = PageCardFieldsDataRow(Integer.Parse(pk)).Text.Text.Trim()

                        sql = String.Format("INSERT INTO T_CommentCardFieldValues(FieldID, CardID, MaintReq, ValueName) " & _
                                            "VALUES({0},{1},{2},'{3}')", _
                                            pk, _
                                            cardPkId, _
                                            IIf(required = True And text.Length > 0, 1, "null"), _
                                            IIf(required = True And text.Length > 0, text, "null"))
                        cmd.CommandText = sql
                        Dim recordsAffected As Integer = cmd.ExecuteNonQuery()

                        If PageCardFieldsDataRow(Integer.Parse(pk)).Required.Checked = True Then
                            Create_MaintReq(prospectid, roomid, text, PageCardFieldsDataRow(Integer.Parse(pk)).Caption.Text)
                        End If
                    Next
                End If
            End Using
        Else

            Dim newlyAddId = From ele In PageCardFieldsDataRow _
                             Where ele.Value.Selected.Checked = True _
                             Select Integer.Parse(ele.Value.Selected.ID.Substring("Selected".Length))

            Dim separator() As Char = New Char() {"f"}
            Dim valuePiped() As String = hidden.Value.Split(separator)
            Dim valueInner(valuePiped.Length - 1) As String

            For i As Integer = 0 To valuePiped.Length - 1
                valueInner(i) = valuePiped(i).Split("v")(0)
            Next


            Using cnn As New SqlConnection(cnx)

                Dim param As String = "0"

                If (valueInner.Length >= 1 And valueInner(0) <> String.Empty) Then
                    If (Integer.Parse(valueInner(0)) > 0) Then
                        param = String.Join(",", valueInner)
                    End If
                End If


                Dim sql As String = String.Format("select FieldID, CardID, ValueName, ValueID from t_CommentCardFieldValues where valueid in ({0})", _
                                                  param)


                Using ada As New SqlDataAdapter(sql, cnn)
                    Dim cbd As New SqlCommandBuilder(ada)
                    Dim dt As New DataTable()

                    ada.FillSchema(dt, SchemaType.Source)
                    ada.Fill(dt)

                    If (param.Equals("0") = False And dt.Rows.Count > 0) Then
                        For i As Integer = 0 To valueInner.Length - 1
                            dt.Rows.Find(valueInner(i)).Delete()
                        Next
                    End If

                    For Each i As Integer In newlyAddId
                        dt.Rows.Add(i, CARDID, PageCardFieldsDataRow(i).Text.Text.Trim())
                    Next

                    ada.UpdateBatchSize = valueInner.Length + newlyAddId.Count()
                    ada.Update(dt)

                    valueInner(0) = ""

                    dt = Nothing
                    dt = New DataTable()

                    ada.SelectCommand.CommandText = "SELECT * FROM T_CommentCards WHERE CardID = " & CARDID
                    cbd = New SqlCommandBuilder(ada)

                    ada.FillSchema(dt, SchemaType.Source)
                    ada.Fill(dt)

                    Dim rw As DataRow = dt.Rows.Find(CARDID)
                    For Each inner As String In TableColumnHeader
                        If PageRadioButtonList(inner).SelectedIndex > -1 Then
                            rw.SetField(inner, PageRadioButtonList(inner).SelectedIndex + 1)
                        End If
                    Next

                    ada.Update(dt)
                End Using
            End Using
        End If

        GetCardFieldValue(CARDID)

        Return

        With oCommentCards
            oCommentCards.CardDate = Date.Now
        End With
        With oCommentCardFields
            'oCommentCardFields.CardDate = Date.Now
            'oCommentCardFields.RoomID = txtRoom.text
            'oCommentCardFields.EnteredByID = Session.UserID
        End With
        With oCommentCardFieldValues
            'oCommentCardFieldValues.RoomID = txtRoom.text
        End With
    End Sub

    Protected Sub btnComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComment.Click

        Return

    End Sub

    Protected Sub btnRoom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRoom.Click
        Dim CardID As String = Request("CardID")
        Dim sSQL As String = ""
        If CardID = 0 Then
            If txtRoom.Text = "" Then
                'Page.RegisterStartupScript("alert", "<script language='javascript'>Get_Window('You Must Enter a Room Number');</script>")
            Else
                Dim ds As New System.Web.UI.WebControls.SqlDataSource
                ds.ConnectionString = Resources.Resource.cns
                ds.SelectCommand = "select top 500 p.prospectid, r.roomid, p.lastname, p.firstname, r.roomnumber, res.checkoutdate from t_prospect p inner join t_reservations res on res.prospectid = p.prospectid inner join (select distinct roomid, reservationid from t_roomallocationmatrix) m on m.reservationid = res.reservationid inner join t_room r on r.roomid = m.roomid where r.roomnumber like '" & txtRoom.Text & "%' order by res.checkoutdate desc"
                Session("RoomSet") = ds
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/propertymanagement/SearchGuestRoom.aspx?CardID=" & Request("CardID") & "','win01',690,450);", True)
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/propertymanagement/SearchGuestRoom.aspx?CardID=" & Request("CardID") & "','win01',690,450);", True)
        End If
    End Sub


    Protected Sub update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles update.Click


        If CARDID > 0 And Comment.InnerText.Length > 0 Then
            Dim sql As String = String.Format("UPDATE T_CommentCards SET Comments = '{0}'  WHERE CardID = {1}", _
                                              Comment.InnerText.Trim(), CARDID)
            Dim cnx As String = Resources.Resource.cns

            Using cnn As New SqlConnection(cnx)
                Using cmd As New SqlCommand(sql, cnn)

                    cnn.Open()

                    cmd.ExecuteNonQuery()

                    cnn.Close()
                End Using
            End Using
        End If
    End Sub


#End Region

#Region "Functions/Routines"


    Protected Sub Create_MaintReq(ByVal ProsID As Integer, ByVal RoomID As Integer, ByVal ValueName As String, ByVal FieldName As String)
        Dim oMaintReq As New clsRequest
        Dim oCombo As New clsComboItems

        oMaintReq.RequestID = 0
        oMaintReq.Load()
        oMaintReq.EnteredByID = Session("UserDBID")
        oMaintReq.EntryDate = DateTime.Now
        oMaintReq.Description = ValueName
        oMaintReq.RoomID = RoomID
        oMaintReq.StatusID = 17603 'Need comboitemid for request status of Not Started
        oMaintReq.RequestAreaID = 11572 'Need comboitemid for request Area of Guest
        oMaintReq.CategoryID = 17719 'Need comboitemid for request Category for Service
        oMaintReq.Subject = FieldName
        oMaintReq.Save()

    End Sub

    Private Sub Set_Values(ByRef sender As String)
        lblAlert.Text = Request("CardID")
        Dim sql As String = ""
        If Request("CardID") = "0" Then
            sql = "Select * from t_CommentCards"
        Else
            sql = "Select c.CardID, p.ProspectID, r.RoomID, P.lastName + ', ' + p.firstname as Guest, r.RoomNumber, Housekeeping, Maintenance, FrontDesk, ResortAmmentities, Hospitality, UnitQuality, Spa, OverallEval from t_CommentCards c inner join t_prospect p on p.prospectid = c.prospectid inner join t_Room r on r.roomid = c.roomid where c.Cardid = '" & Request("CardID") & "'"
        End If

        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand(sql, cn)
        Dim rs As SqlDataReader
        cn.Open()
        rs = cm.ExecuteReader

        If Not rs.HasRows Then
            lblAlert.Text = "No Records"
        Else
            rs.Read()
            txtGuest.Text = rs("Guest")
            txtGuest.ID = rs("Prospectid")
            txtGuest.Text = rs("Guest")
            txtGuest.ID = rs("Prospectid")
            txtRoom.Text = rs("Roomnumber")
            txtRoom.ID = rs("RoomID")
            lblAlert.Text = txtGuest.ID & ", (" & Request("CardID") & ")"
        End If
        rs.Close()
        cn.Close()
    End Sub

    Private Sub Build_Report(ByVal sender As String)
        Dim sql As String = ""
        Dim sql2 As String = ""

        sql = "Select * from t_CommentCardFields order by FieldName"

        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand(sql, cn)
        Dim rs As SqlDataReader
        cn.Open()
        rs = cm.ExecuteReader
        lit1.Text = ""
        lit1.Text &= ("<table>")
        If Not rs.HasRows Then
            lit1.Text &= ("<tr><td>NO RECORDS</td></tr>")
        Else
            lit1.Text &= ("<tr><th>Filed Name</th><th>Sel Field</th><th>W.O.</th><th>Description</th></tr>")
            Do While rs.Read
                sql2 = "select * from t_commentcardfieldvalues where fieldid = '" & rs("fieldid") & "' and cardid = '" & Request("Cardid") & "'"
                Dim cn2 As New SqlConnection(Resources.Resource.cns)
                Dim cm2 As New SqlCommand(sql2, cn2)
                Dim rs2 As SqlDataReader
                cn2.Open()
                rs2 = cm2.ExecuteReader

                lit1.Text &= ("<tr>")
                lit1.Text &= ("<td>" & rs("FieldName") & "</td>")
                If Not rs2.HasRows Then
                    lit1.Text &= ("<td><input type='Checkbox' name = 'field' value = '" & rs("fieldid") & "'></td>")
                    lit1.Text &= ("<td><input type = checkbox name = 'maintreq' value='' ></td>")
                    lit1.Text &= ("<td><input type = text name = 'descript' value = ''></td>")
                Else
                    rs2.Read()
                    'lblAlert.text = rs2("description")
                    lit1.Text &= ("<td><input type='Checkbox' name = 'field' value = '" & rs("fieldid") & "' Checked></td>")
                    lit1.Text &= ("<td><input type = checkbox name = 'maintreq' value='' " & IIf(rs2("MaintReq") = True, "Checked", "") & "></td>")
                    lit1.Text &= ("<td><input type = text name = 'descript' value = '" & rs2("ValueName") & "'></td>")
                End If
                lit1.Text &= ("</tr>")
                rs2.Close()
                cn2.Close()
            Loop
        End If
        lit1.Text &= ("</table>")
        lit1.Text &= ("<br>")
        rs.Close()
        cn.Close()
    End Sub


    Private Sub GetCardFieldValue(ByVal byCardID As String)

        'Reset all checkboxes and radio option buttons
        For Each st As String In TableColumnHeader
            PageRadioButtonList(st).SelectedIndex = -1
        Next

        For Each j As Integer In PageCardFieldsDataRow.Keys
            PageCardFieldsDataRow(j).Selected.Checked = False
            PageCardFieldsDataRow(j).Required.Checked = False
            PageCardFieldsDataRow(j).Text.Text = String.Empty
        Next

        ''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''
        If String.IsNullOrEmpty(byCardID) Or Integer.Parse(byCardID) = 0 Then Return

        CARDID = byCardID


        Dim cnx As String = Resources.Resource.cns
        Dim sql As String = String.Format( _
            "select CardId, ProspectID, RoomID, Comments, FrontDesk, HouseKeeping, " & _
            "ResortAmmentities, Maintenance, Hospitality, UnitQuality, SPA, OverAllEval from t_CommentCards WHERE CardId = {0}", CARDID)

        Using cnn As New SqlConnection(cnx)
            Using cmd As New SqlCommand(sql, cnn)

                cnn.Open()
                Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If rdr.HasRows = True Then
                    Dim readOnce As Boolean = False
                    Do While rdr.Read()

                        If readOnce = False Then

                            PROSPECTID = rdr.Item("ProspectID")
                            ROOMID = rdr.Item("RoomID")

                            Comment.InnerText = IIf(rdr.Item("Comments").Equals(DBNull.Value) = True, String.Empty, rdr.Item("Comments"))

                            For Each title As String In TableColumnHeader
                                If rdr(rdr.GetOrdinal(title)).Equals(DBNull.Value) = False Then
                                    If rdr(rdr.GetOrdinal(title)) > 0 Then
                                        PageRadioButtonList(title).SelectedIndex = rdr(rdr.GetOrdinal(title)) - 1
                                    End If
                                End If
                            Next
                            readOnce = True

                        End If
                    Loop
                    rdr.Close()
                End If

                ''
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''
                sql = String.Format("SELECT * FROM T_CommentCardFieldValues WHERE CardID = " & CARDID)
                cmd.CommandText = sql
                cnn.Open()

                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)


                If (rdr.HasRows = True) Then
                    Dim cph As ContentPlaceHolder = DirectCast(Me.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
                    Dim container As PlaceHolder = DirectCast(cph.FindControl("phContainer"), PlaceHolder)
                    Dim hidden As HiddenField = DirectCast(container.FindControl("ValueIdFieldId"), HiddenField)

                    hidden.Value = String.Empty
                    Do While (rdr.Read())

                        'Comboboxes & Textbox

                        PageCardFieldsDataRow(Convert.ToInt32(rdr.Item("FieldID").ToString())).Required.Checked = IIf(rdr.Item("MaintReq").Equals(DBNull.Value), False, rdr.Item("MaintReq"))
                        PageCardFieldsDataRow(Convert.ToInt32(rdr.Item("FieldID").ToString())).Text.Text = rdr.Item("ValueName")

                        PageCardFieldsDataRow(Convert.ToInt32(rdr.Item("FieldID").ToString())).Selected.Checked = True

                        If hidden.Value = String.Empty Then
                            hidden.Value = rdr.Item("ValueId") & "v" & rdr.Item("FieldId")
                        Else
                            hidden.Value += "f" & rdr.Item("ValueId") & "v" & rdr.Item("FieldId")
                        End If
                    Loop
                End If


            End Using
        End Using

    End Sub
#End Region


End Class
