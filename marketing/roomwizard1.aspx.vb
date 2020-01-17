Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls
Imports Microsoft.VisualBasic

Partial Class marketing_roomwizard1
    Inherits System.Web.UI.Page

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
         Dim resType As String = ""
        resType = get_Res_Type()
        If resType = "Owner" Or resType = "TrialOwner" Or resType = "PlanWithTan" Then
            MultiView1.ActiveViewIndex = 1
            MultiView2.ActiveViewIndex = 0
            gvRooms.visible = False
            chkSpares.Checked = False
            gvSpares.visible = False
            lblSPares.visible = False
            RoomSelectBtn.visible = False
        ElseIf resType = "Exchange" Or resType = "Points" Or resType = "NALJR" Or resType = "PointsExchange" Then
            MultiView1.ActiveViewIndex = 1
            MultiView2.ActiveViewIndex = 1
            gvRooms.visible = False
            chkSpares.Checked = False
            gvSpares.visible = False
            lblSPares.visible = False
            RoomSelectBtn.visible = False
        Else
            MultiView1.ActiveViewIndex = 1
            MultiView2.ActiveViewIndex = 2
            gvRooms.visible = False
            chkSpares.Checked = False
            gvSpares.visible = False
            lblSPares.visible = False
            RoomSelectBtn.visible = False
        End If
    End Sub

    Protected Sub LinkButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton2.Click
        MultiView1.ActiveViewIndex = 0
        'MultiView2.ActiveViewIndex = 0
        gvRooms.visible = False
        chkSpares.Checked = False
        gvSpares.visible = False
        lblSPares.visible = False
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Load_SIs()
            For i = (System.DateTime.Now.Year - 4) To (System.DateTime.Now.Year + 4)
                dd1.Items.Add(i)
                dd2.Items.Add(i)
                dd3.Items.Add(i)
                dd4.Items.Add(i)
                dd5.Items.Add(i)
                dd6.Items.Add(i)
                dd7.Items.Add(i)
                dd8.Items.Add(i)
            Next
        End If
    End Sub

    Private Sub Load_SIs()
        siUnitType.Connection_String = Resources.Resource.cns
        siUnitType.ComboItem = "UnitType"
        siUnitType.Label_Caption = ""
        siUnitType.Load_Items()
        siInvType.Connection_String = Resources.Resource.cns
        siInvType.ComboItem = "ReservationType"
        siInvType.Label_Caption = ""
        siInvType.Load_Items()
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        lblRoomErr.Text = ""
        lblSpareErr.Text = ""
        labelErr.Text = ""
        gvRooms.Visible = False
        Dim sErr As String = ""
        Dim bRoom As Boolean = True
        Dim bSpare As Boolean = True
        Dim oRes As New clsReservations
        oRes.ReservationID = Request("ReservationID")
        oRes.Load()
        If CheckSecurity("Reservations", "Add" & siInvType.SelectedName & "Room", , , Session("UserDBID")) Then
            Dim cn As New SqlConnection(Resources.resource.cns)
            Dim cm As New SqlCommand("", cn)
            Dim dr As SqlDataReader
            Dim da As SqlDataAdapter
            Dim ds As DataSet
            Dim oCombo As New clsComboItems
            Dim iDate As String = ""
            Dim category As String = ""
            Dim oUsage As New clsUsage
            Dim resType As String = oCombo.Lookup_ComboItem(siInvType.Selected_ID)



            If Date.Compare(System.DateTime.Now, CDate(oRes.CheckInDate)) > 0 Then
                cm.CommandText = "Select * from ufn_RoomsAvailable('" & ddBedrooms.SelectedValue & "'," & siUnitType.Selected_ID & ",'" & System.DateTime.Now.ToShortDateString & "','" & CDate(oRes.CheckOutDate).AddDays(-1) & "'," & siInvType.Selected_ID & ",'" & siUnitType.SelectedName & "','" & siInvType.SelectedName & "',0) where available = 'available'"
                
                iDate = System.DateTime.Now.ToShortDateString
            Else
                cm.CommandText = "Select * from ufn_RoomsAvailable('" & ddBedrooms.selectedvalue & "'," & siUnitType.Selected_ID & ",'" & oRes.CheckInDate & "','" & CDate(oRes.CheckOutDate).AddDays(-1) & "'," & siInvType.Selected_ID & ",'" & siUnitType.SelectedName & "','" & siInvType.SelectedName & "',0) where available = 'available'"
                iDate = oRes.CheckInDate
            End If

            cn.Open()
            dr = cm.ExecuteReader
            Dim row As DataRow
            Dim dt As New DataTable
            If dr.HasRows Then
                dt.Columns.Add("RoomID")
                dt.Columns.Add("RoomNumber")
                dt.Columns.Add("RoomType")
                dt.Columns.Add("RoomSubType")
                dt.Columns.Add("Category")
                While dr.Read
                    category = ""
                    row = dt.NewRow
                    row("RoomID") = dr("RoomID") & IIF(isnumeric(dr("RoomID2")), "-" & dr("RoomID2") & iif(isnumeric(dr("RoomID3")), "-" & dr("RoomID3"), ""), "")
                    row("RoomNumber") = dr("RoomNumber") & IIF(isnumeric(dr("RoomID2")), "$" & dr("RoomNumber2") & iif(isnumeric(dr("RoomID3")), "$" & dr("RoomNumber3"), ""), "")
                    row("RoomType") = dr("RoomType1") & IIF(isnumeric(dr("RoomID2")), "$" & dr("RoomType2") & iif(isnumeric(dr("RoomID3")), "$" & dr("RoomType3"), ""), "")
                    row("RoomSubType") = dr("RoomSubType1")
                    If resType = "Rental" Then
                        If IsNumeric(dr("RoomID3")) Then
                            category = oUsage.Get_Categories(dr("RoomID"), CDate(iDate), CDate(oRes.CheckOutDate).AddDays(-1))
                            category = category & "$" & oUsage.Get_Categories(dr("RoomID2"), CDate(iDate), CDate(oRes.CheckOutDate).AddDays(-1))
                            category = category & "$" & oUsage.Get_Categories(dr("RoomID3"), CDate(iDate), CDate(oRes.CheckOutDate).AddDays(-1))
                        ElseIf IsNumeric(dr("RoomID2")) Then
                            category = oUsage.Get_Categories(dr("RoomID"), CDate(iDate), CDate(oRes.CheckOutDate).AddDays(-1))
                            category = category & "$" & oUsage.Get_Categories(dr("RoomID2"), CDate(iDate), CDate(oRes.CheckOutDate).AddDays(-1))
                        Else
                            category = oUsage.Get_Categories(dr("RoomID"), CDate(iDate), CDate(oRes.CheckOutDate).AddDays(-1))
                        End If
                        row("Category") = category '& oUsage.Err '& "N/AA"
                    Else
                        row("Category") = "N/A"
                    End If
                    'row("Rooms") = row("Rooms").ToString.Replace("<br>", CHr(13))
                    dt.Rows.Add(row)
                End While
            Else
                dt.Columns.Add("NoRecs")
            End If
            gvRooms.datasource = dt
            Dim ky(0) As String
            ky(0) = "RoomID"
            gvRooms.datakeynames = ky
            gvRooms.databind()
            gvRooms.visible = True
            cn.Close()
            'oRes = Nothing
            cn = Nothing
            cm = Nothing
            da = Nothing
            ds = Nothing
            oUsage = Nothing
        Else
            bRoom = False
            sErr = "You Do Not Have Permission to Add " & siInvType.SelectedName & " Rooms."
        End If

        If chkSpares.Checked Then
            If CheckSecurity("Reservations", "AddSpareRoom", , , Session("UserDBID")) Then
                Dim oRoom As New clsRooms
                If oRes.CheckInDate < Date.Today Then
                    gvSpares.DataSource = oRoom.search_Spares(CDate(Date.Today.ToShortDateString), CDate(oRes.CheckOutDate))
                Else
                    gvSpares.DataSource = oRoom.search_Spares(CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate))
                End If
                gvSpares.DataBind()
                lblSpares.visible = True
                gvSpares.visible = True
                oRoom = Nothing
            Else
                bSpare = False
                gvSpares.visible = False
                lblSpares.visible = False
            End If
        Else
            gvSpares.visible = False
            lblSpares.visible = False
        End If
        If bRoom = False Then
            If chkSpares.Checked And bSpare Then
                lblRoomErr.Text = sErr
                RoomSelectBtn.visible = True
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "alert('" & sErr & "');", True)
                gvRooms.visible = False
                RoomSelectBtn.visible = False
            End If
        Else
            If chkSpares.Checked And bSpare = False Then
                lblSpareErr.Text = "You Do Not Have Permission To Add Spare Rooms."
            End If
            RoomSelectBtn.visible = True
        End If
        oRes = Nothing
        'Catch ex As Exception
        'labelErr.Text = ex.Message
        'End Try
    End Sub

    Protected Sub gvRooms_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            Dim i As Integer
            Dim rooms() As String = e.Row.Cells(2).Text.Split("$")
            Dim roomTypes() As String = e.Row.Cells(3).Text.Split("$")
            Dim categories() As String = e.Row.Cells(5).Text.Split("$")
            For i = 0 To UBound(rooms)
                If i = 0 Then
                    e.Row.Cells(2).Text = rooms(i)
                Else
                    e.Row.Cells(2).Text = e.Row.Cells(2).Text & "<br/>" & rooms(i)
                End If
            Next i
            For i = 0 To UBound(roomTypes)
                If i = 0 Then
                    e.Row.Cells(3).Text = roomTypes(i)
                Else
                    e.Row.Cells(3).Text = e.Row.Cells(3).Text & "<br/>" & roomTypes(i)
                End If
            Next i
            For i = 0 To UBound(categories)
                If i = 0 Then
                    e.Row.Cells(5).Text = categories(i)
                Else
                    e.Row.Cells(5).Text = e.Row.Cells(5).Text & "<br/>" & categories(i)
                End If
            Next i
        End If
    End Sub

    Protected Sub gvRooms_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvRooms.SelectedIndexChanged
        Try
            Dim row As GridViewRow = gvRooms.SelectedRow
            'Response.Write("you chose this: " & row.Cells(1).Text)
            Dim oRes As New clsReservations
            oRes.ReservationID = Request("ReservationID")
            oRes.Load()
            oRes.UserID = Session("UserDBID")
            If oRes.CheckInDate < Date.Today Then
                oRes.Add_Room(row.Cells(1).Text, oRes.ReservationID, CDate(Date.Today), CDate(oRes.CheckOutDate).AddDays(-1))
            Else
                oRes.Add_Room(row.Cells(1).Text, oRes.ReservationID, CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate).AddDays(-1))
            End If
            oRes = Nothing
        Catch ex As Exception
            labelErr.text = ex.Message

        Finally
            '            ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "window.opener.Refresh_Rooms();window.close();", True)
        End Try
    End Sub
    Protected Sub gvRooms_OnRowCreated(ByVal sender As Object, ByVal e As Web.UI.WebControls.GridViewRowEventArgs) Handles gvRooms.RowCreated

        'Those columns you don't want to display you config here, 

        'you could use a for statement if you have many 
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub gvSpares_OnRowCreated(ByVal sender As Object, ByVal e As Web.UI.WebControls.GridViewRowEventArgs) Handles gvSpares.RowCreated
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Private Function get_Res_Type() As String
        Dim oRes As New clsReservations
        Dim resType As String = ""
        resType = oRes.Get_ResType(Request("ReservationID"))
        oRes = Nothing
        Return resType
    End Function
    Protected Sub linkButton3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkButton3.Click
        MultiView3.ActiveViewIndex = 0
        gvRooms.visible = False
    End Sub

    Protected Sub linkButton4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkButton4.Click
        MultiView3.ActiveViewIndex = 1
        gvRooms.visible = False
    End Sub

    Protected Sub linkButton10_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkButton10.Click
        MultiView4.ActiveViewIndex = 0
        gvRooms.visible = False
    End Sub

    Protected Sub linkButton11_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkButton11.Click
        MultiView4.ActiveViewIndex = 1
        gvRooms.visible = False
    End Sub

    Protected Sub linkButton5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkButton5.Click
        MultiView5.ActiveViewIndex = 0
        gvRooms.visible = False
    End Sub

    Protected Sub linkButton6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkButton6.Click
        MultiView5.ActiveViewIndex = 1
        gvRooms.visible = False
    End Sub

    Protected Sub linkButton7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkButton7.Click
        MultiView5.ActiveViewIndex = 2
        gvRooms.visible = False
    End Sub

    Protected Sub linkButton8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkButton8.Click
        MultiView5.ActiveViewIndex = 3
        gvRooms.visible = False
    End Sub

    Protected Sub linkButton9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkButton9.Click
        MultiView5.ActiveViewIndex = 4
        gvRooms.visible = False
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oUsage As New clsUsage
        Dim oRes1 As New clsReservations
        oRes1.ReservationID = Request("ReservationID")
        oRes1.Load()
        gvRooms.DataSource = oUsage.get_Usage_ID("OwnerName", OwnerName.Text, dd2.SelectedValue, get_Res_Type(), oRes1.CheckInDate, oRes1.CheckOutDate, oRes1.ProspectID)
        gvRooms.DataBind()
        gvRooms.Visible = True
        RoomSelectBtn.visible = True
        labelErr.Text = oUsage.Err
        oRes1 = Nothing
        oUsage = Nothing
    End Sub

    Private Function Add_Rooms() As Boolean
        Try
            Dim sErr As String = ""
            Dim bRoom As Boolean = False
            For Each row As GridViewRow In gvRooms.Rows
                Dim cb As CheckBox = row.FindControl("RoomSelect")

                If cb.Checked Then
                    bRoom = True
                    Dim oRes As New clsReservations
                    oRes.ReservationID = Request("ReservationID")
                    oRes.Load()
                    oRes.UserID = Session("UserDBID")
                    If oRes.CheckInDate < Date.Today Then
                        oRes.Add_Room(row.Cells(1).Text, oRes.ReservationID, CDate(Date.Today), CDate(oRes.CheckOutDate).AddDays(-1))
                    Else
                        oRes.Add_Room(row.Cells(1).Text, oRes.ReservationID, CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate).AddDays(-1))
                    End If
                    oRes = Nothing
                End If
            Next

            If chkSpares.Checked = True Then
                Dim usageID As Integer
                For Each row As GridViewRow In gvSpares.Rows
                    Dim cb As CheckBox = row.FindControl("SpareSelect")
                    If cb.Checked Then
                        bRoom = True
                        Dim oRes As New clsReservations
                        oRes.ReservationID = Request("ReservationID")
                        oRes.Load()
                        oRes.UserID = Session("UserDBID")
                        Dim oCombo As New clsComboItems
                        Dim oUsage As New clsUsage
                        Dim oContract As New clsContract
                        oUsage.TypeID = oRes.TypeID
                        oUsage.ContractID = oContract.Get_Contract_ID("KCPSPare")
                        oUsage.UsageYear = Year(System.DateTime.Now)
                        oUsage.RoomTypeID = oCombo.Lookup_ID("RoomType", row.Cells(3).Text)
                        oUsage.UnitTypeID = oCombo.Lookup_ID("UnitType", row.Cells(4).Text)
                        If oRes.CheckInDate < Date.Today Then
                            oUsage.InDate = CDate(Date.Today)
                        Else
                            oUsage.InDate = CDate(oRes.CheckInDate)
                        End If
                        oUsage.OutDate = oRes.CheckOutDate
                        oUsage.StatusID = oCombo.Lookup_ID("UsageStatus", "Used")
                        oUsage.Save()
                        usageID = oUsage.UsageID
                        labelErr.Text = oUsage.Err
                        If oRes.CheckInDate < Date.Today Then
                            oRes.Add_Spare_Room(row.Cells(1).Text, oRes.ReservationID, CDate(Date.Today), CDate(oRes.CheckOutDate).AddDays(-1), usageID)
                        Else
                            oRes.Add_Spare_Room(row.Cells(1).Text, oRes.ReservationID, CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate).AddDays(-1), usageID)
                        End If
                        labelErr.Text = oRes.Err
                        oRes = Nothing
                        oUsage = Nothing
                        oCombo = Nothing
                        oContract = Nothing
                    End If
                Next
            End If

            If Not bRoom Then
                labelErr.Text = "No Room Selected"
            Else
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType, "alert", "window.opener.Refresh_Rooms();window.close();", True)

                '                ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "window.opener.Refresh_Rooms();window.close();", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType, "alert", "alert('" & ex.Message & "');", True)
            '            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & ex.Message & "');", True)
            labelErr.Text = ex.Message
        Finally
        End Try
    End Function

    Protected Sub RoomSelectBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RoomSelectBtn.Click
        Try
            Dim bProceed As Boolean = True
            If MultiView1.ActiveViewIndex = 0 Then
                Dim usageBD As Integer = 0
                Dim x As Integer = 0
                For Each row As GridViewRow In gvRooms.Rows
                    Dim cb As CheckBox = row.FindControl("RoomSelect")
                    If cb.Checked Then
                        Dim oUsage As New clsUsage
                        Dim oRes As New clsReservations
                        Dim i As Integer = 0
                        oRes.ReservationID = Request("ReservationID")
                        oRes.Load()
                        Dim rooms As String() = row.Cells(1).Text.Split("-")
                        x = UBound(rooms) - 2
                        x = CInt(Left(ddBedrooms.SelectedValue, 1))
                        For i = 0 To UBound(rooms)
                            If oRes.CheckInDate < Date.Today Then
                                usageBD = oUsage.Usage_BD_Count(CInt(rooms(i)), CDate(Date.Today), CDate(oRes.CheckOutDate).AddDays(-1))
                            Else
                                usageBD = oUsage.Usage_BD_Count(CInt(rooms(i)), CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate).AddDays(-1))
                            End If
                            If usageBD > CInt(Left(ddBedrooms.SelectedValue, 1)) Then
                                bProceed = False
                                Exit For
                            End If
                        Next
                        If bProceed = False Then
                            Exit For
                        End If
                    End If
                Next
            End If

            If bProceed Then
                Add_Rooms()

                'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType, "alert", "alert('" & usageBD & " " & x & "');", True)

                'ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & usageBD & " " & x & "');", True)
            Else
                '                ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "var r=confirm('The Room You Are Trying To Add Belongs To a Larger Bedroom Usage. Do You Want To Proceed?');if(r==true){__doPostBack('LinkButton12', '');}", True)
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType, "alert", "var r=confirm('The Room You Are Trying To Add Belongs To a Larger Bedroom Usage. Do You Want To Proceed?');if(r==true){__doPostBack('LinkButton12', '');}", True)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType, "alert", "alert('ERR: " & ex.Message & "');", True)

            '            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('ERR: " & ex.Message & "');", True)

        End Try

        'Try
        '    Dim sErr As String = ""
        '    Dim bRoom As Boolean = False
        '    For Each row As GridViewRow In gvRooms.Rows
        '        Dim cb As CheckBox = row.FindControl("RoomSelect")

        '        If cb.Checked Then
        '            bRoom = True
        '            Dim oRes As New clsReservations
        '            oRes.ReservationID = Request("ReservationID")
        '            oRes.Load()
        '            oRes.UserID = Session("UserDBID")
        '            If oRes.CheckInDate < Date.Today Then
        '                oRes.Add_Room(row.Cells(1).Text, oRes.ReservationID, CDate(Date.Today), CDate(oRes.CheckOutDate).AddDays(-1))
        '            Else
        '                oRes.Add_Room(row.Cells(1).Text, oRes.ReservationID, CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate).AddDays(-1))
        '            End If
        '            oRes = Nothing
        '        End If
        '    Next

        '    If chkSpares.Checked = True Then
        '        Dim usageID As Integer
        '        For Each row As GridViewRow In gvSpares.Rows
        '            Dim cb As CheckBox = row.FindControl("SpareSelect")
        '            If cb.Checked Then
        '                bRoom = True
        '                Dim oRes As New clsReservations
        '                oRes.ReservationID = Request("ReservationID")
        '                oRes.Load()
        '                oRes.UserID = Session("UserDBID")
        '                Dim oCombo As New clsComboItems
        '                Dim oUsage As New clsUsage
        '                Dim oContract As New clsContract
        '                oUsage.TypeID = oRes.TypeID
        '                oUsage.ContractID = oContract.Get_Contract_ID("KCPSPare")
        '                oUsage.UsageYear = Year(System.DateTime.Now)
        '                oUsage.RoomTypeID = oCombo.Lookup_ID("RoomType", row.Cells(3).Text)
        '                oUsage.UnitTypeID = oCombo.Lookup_ID("UnitType", row.Cells(4).Text)
        '                If oRes.CheckInDate < Date.Today Then
        '                    oUsage.InDate = CDate(Date.Today)
        '                Else
        '                    oUsage.InDate = CDate(oRes.CheckInDate)
        '                End If
        '                oUsage.OutDate = oRes.CheckOutDate
        '                oUsage.StatusID = oCombo.Lookup_ID("UsageStatus", "Used")
        '                oUsage.Save()
        '                usageID = oUsage.UsageID
        '                labelErr.Text = oUsage.Err
        '                If oRes.CheckInDate < Date.Today Then
        '                    oRes.Add_Spare_Room(row.Cells(1).Text, oRes.ReservationID, CDate(Date.Today), CDate(oRes.CheckOutDate).AddDays(-1), usageID)
        '                Else
        '                    oRes.Add_Spare_Room(row.Cells(1).Text, oRes.ReservationID, CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate).AddDays(-1), usageID)
        '                End If
        '                labelErr.Text = oRes.Err
        '                oRes = Nothing
        '                oUsage = Nothing
        '                oCombo = Nothing
        '                oContract = Nothing
        '            End If
        '        Next
        '    End If

        '    If Not bRoom Then
        '        labelErr.Text = "No Room Selected"
        '    Else
        '        ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "window.opener.Refresh_Rooms();window.close();", True)
        '    End If
        'Catch ex As Exception
        '    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & ex.Message & "');", True)
        '    labelErr.Text = ex.Message
        'Finally
        'End Try
    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim oUsage As New clsUsage
        Dim oRes1 As New clsReservations
        oRes1.ReservationID = Request("ReservationID")
        oRes1.Load()
        gvRooms.DataSource() = oUsage.get_Usage_ID("KCPNumber", KCPNum.Text, dd3.SelectedValue, get_Res_Type(), oRes1.CheckInDate, oRes1.CheckOutDate, oRes1.ProspectID)
        gvRooms.DataBind()
        gvRooms.visible = True
        RoomSelectBtn.visible = True
        labelErr.Text = oUsage.Err
        oRes1 = Nothing
        oUsage = Nothing
    End Sub

    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim oUsage As New clsUsage
        Dim oRes1 As New clsReservations
        oRes1.ReservationID = Request("ReservationID")
        oRes1.Load()
        gvRooms.DataSource = oUsage.get_Usage_ID("OwnerName", txtOwnerName.Text, dd4.SelectedValue, get_Res_Type(), oRes1.CheckInDate, oRes1.CheckOutDate, oRes1.ProspectID)
        gvRooms.DataBind()
        gvRooms.visible = True
        RoomSelectBtn.visible = True
        labelErr.Text = oUsage.Err
        oRes1 = Nothing
        oUsage = Nothing
    End Sub

    Protected Sub Button5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim oUsage As New clsUsage
        Dim oRes1 As New clsReservations
        oRes1.ReservationID = Request("ReservationID")
        oRes1.Load()
        gvRooms.DataSource = oUsage.get_Usage_ID("IINumber", txtIINumber.Text, dd5.SelectedValue, get_Res_Type(), oRes1.CheckInDate, oRes1.CheckOutDate, oRes1.ProspectID)
        gvRooms.DataBind()
        gvRooms.visible = True
        RoomSelectBtn.visible = True
        labelErr.Text = oUsage.Err
        oRes1 = Nothing
        oUsage = Nothing
    End Sub

    Protected Sub Button6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim oUsage As New clsUsage
        Dim oRes1 As New clsReservations
        oRes1.ReservationID = Request("ReservationID")
        oRes1.Load()
        gvRooms.DataSource = oUsage.get_Usage_ID("RCINumber", txtRCINumber.Text, dd6.SelectedValue, get_Res_Type(), oRes1.CheckInDate, oRes1.CheckOutDate, oRes1.ProspectID)
        gvRooms.DataBind()
        gvRooms.visible = True
        RoomSelectBtn.visible = True
        labelErr.Text = oUsage.Err
        oRes1 = Nothing
        oUsage = Nothing
    End Sub

    Protected Sub Button7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button7.Click
        Dim oUsage As New clsUsage
        Dim oRes1 As New clsReservations
        oRes1.ReservationID = Request("ReservationID")
        oRes1.Load()
        gvRooms.DataSource = oUsage.get_Usage_ID("ICENumber", txtICENumber.Text, dd7.SelectedValue, get_Res_Type(), oRes1.CheckInDate, oRes1.CheckOutDate, oRes1.ProspectID)
        gvRooms.DataBind()
        gvRooms.visible = True
        RoomSelectBtn.visible = True
        labelErr.Text = oUsage.Err
        oRes1 = Nothing
        oUsage = Nothing
    End Sub

    Protected Sub Button8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button8.Click
        Dim oUsage As New clsUsage
        Dim oRes1 As New clsReservations
        oRes1.ReservationID = Request("ReservationID")
        oRes1.Load()
        gvRooms.DataSource = oUsage.get_Usage_ID("RCIPoints", txtRCIPoints.Text, dd8.SelectedValue, get_Res_Type(), oRes1.CheckInDate, oRes1.CheckOutDate, oRes1.ProspectID)
        gvRooms.DataBind()
        gvRooms.visible = True
        RoomSelectBtn.visible = True
        labelErr.Text = oUsage.Err
        oRes1 = Nothing
        oUsage = Nothing
    End Sub

    Protected Sub Button9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button9.Click
        Dim oUsage As New clsUsage
        Dim oRes1 As New clsReservations
        oRes1.ReservationID = Request("ReservationID")
        oRes1.Load()
        gvRooms.DataSource = oUsage.get_Usage_ID("Year", "", dd1.SelectedValue, get_Res_Type(), oRes1.CheckInDate, oRes1.CheckOutDate, oRes1.ProspectID)
        gvRooms.DataBind()
        gvRooms.visible = True
        RoomSelectBtn.visible = True
        labelErr.Text = oUsage.Err
        oRes1 = Nothing
        oUsage = Nothing
    End Sub

    Protected Sub Unnamed1_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oUsage As New clsUsage
        Dim oRes1 As New clsReservations
        oRes1.ReservationID = Request("ReservationID")
        oRes1.Load()
        gvRooms.DataSource = oUsage.get_Usage_ID("OwnerName", txtOwnerName.Text, dd4.SelectedValue, get_Res_Type(), oRes1.CheckInDate, oRes1.CheckOutDate, oRes1.ProspectID)
        gvRooms.DataBind()
        gvRooms.visible = True
        RoomSelectBtn.visible = True
        labelErr.Text = oUsage.Err
        oRes1 = Nothing
        oUsage = Nothing
    End Sub

    Protected Sub LinkButton12_Click(sender As Object, e As System.EventArgs) Handles LinkButton12.Click
        Add_Rooms()
    End Sub
End Class
