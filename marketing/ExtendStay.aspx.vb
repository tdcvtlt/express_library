Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls
Partial Class marketing_ExtendStay
    Inherits System.Web.UI.Page

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        If dteExtend.Selected_Date = "" Then
            lblErr.Text = "Please Select A New Check-Out Date."
        Else
            'Try
            lblErr.Text = ""
            lblSpares.visible = False
            gvSpares.visible = False
            gvRooms.visible = False
            Dim daysDiff As Integer = 0
            Dim oRes As New clsReservations
            oRes.ReservationID = Request("ReservationID")
            oRes.Load()
            Dim oCombo As New clsComboItems
            If Date.Compare(CDate(oRes.CheckOutDate), CDate(dteExtend.Selected_Date)) >= 0 Then
                lblErr.Text = "Make Sure the New Out Date is Greater than the Current Out Date."
            Else
                If (siInvType.SelectedName = "Rental" Or siInvType.SelectedName = "Marketing" Or siInvType.SelectedName = "Developer" Or siInvType.SelectedName = "Vendor") And Not (oRes.val_Res_Amt(oRes.ReservationID, daysDiff * 2)) Then
                    lblErr.Text = "Insufficient Room Charge Amount"
                Else
                    Dim cn As New SqlConnection(Resources.resource.cns)
                    Dim cm As New SqlCommand("", cn)
                    Dim dr As SqlDataReader
                    Dim da As SqlDataAdapter
                    Dim ds As DataSet
                    cm.CommandText = "Select * from ufn_RoomsAvailable('" & ddBedrooms.selectedvalue & "'," & siUnitType.Selected_ID & ",'" & CDate(oRes.CheckOutDate) & "','" & CDate(dteExtend.Selected_Date).AddDays(-1) & "'," & siInvType.Selected_ID & ",'" & siUnitType.SelectedName & "','" & siInvType.SelectedName & "',0) where available = 'available'"
                    cn.Open()
                    dr = cm.ExecuteReader
                    Dim row As DataRow
                    Dim dt As New DataTable
                    If dr.HasRows Then
                        dt.Columns.Add("RoomID")
                        dt.Columns.Add("RoomNumber")
                        dt.Columns.Add("RoomType")
                        dt.Columns.Add("RoomSubType")
                        While dr.Read
                            row = dt.NewRow
                            row("RoomID") = dr("RoomID") & IIF(isnumeric(dr("RoomID2")), "-" & dr("RoomID2") & iif(isnumeric(dr("RoomID3")), "-" & dr("RoomID3"), ""), "")
                            row("RoomNumber") = dr("RoomNumber") & IIF(isnumeric(dr("RoomID2")), "$" & dr("RoomNumber2") & iif(isnumeric(dr("RoomID3")), "$" & dr("RoomNumber3"), ""), "")
                            row("RoomType") = dr("RoomType1") & IIF(isnumeric(dr("RoomID2")), "$" & dr("RoomType2") & iif(isnumeric(dr("RoomID3")), "$" & dr("RoomType3"), ""), "")
                            row("RoomSubType") = dr("RoomSubType1")
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

                    If chkSpares.Checked Then
                        Dim oRoom As New clsRooms
                        gvSpares.datasource = oRoom.search_Spares(CDate(oRes.CheckOutDate), CDate(dteExtend.Selected_Date))
                        gvSpares.databind()
                        lblSpares.visible = True
                        gvSpares.visible = True
                        oRoom = Nothing
                    Else
                        gvSpares.visible = False
                        lblSpares.visible = False
                    End If
                    RoomSelectBtn.visible = True
                    cn.Close()
                    cn = Nothing
                    cm = Nothing
                    da = Nothing
                    ds = Nothing
                End If
            End If
            oRes = Nothing
            'Catch ex As Exception
            'lblErr.Text = ex.Message
            'End Try
        End If
    End Sub

    Protected Sub RoomSelectBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RoomSelectBtn.Click
        Try
            Dim bRoom As Boolean = False
            For Each row As GridViewRow In gvRooms.Rows
                Dim cb As CheckBox = row.FindControl("RoomSelect")

                If cb.Checked Then
                    bRoom = True
                    Dim oRes As New clsReservations
                    oRes.ReservationID = Request("ReservationID")
                    oRes.UserID = Session("UserDBID")
                    oRes.Load()
                    oRes.Add_Room(row.Cells(1).Text, oRes.ReservationID, CDate(oRes.CheckOutDate), CDate(dteExtend.Selected_Date).AddDays(-1))
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
                        oRes.UserID = Session("UserDBID")
                        oRes.ReservationID = Request("ReservationID")
                        oRes.Load()
                        Dim oCombo As New clsComboItems
                        Dim oUsage As New clsUsage
                        Dim oContract As New clsContract
                        oUsage.UserID = Session("UserDBID")
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
                        lblErr.Text = oUsage.Err
                        oRes.Add_Spare_Room(row.Cells(1).Text, oRes.ReservationID, CDate(oRes.CheckOutDate), CDate(dteExtend.Selected_Date).AddDays(-1), usageID)
                        lblErr.Text = oRes.Err
                        oRes = Nothing
                        oUsage = Nothing
                        oCombo = Nothing
                        oContract = Nothing
                    End If
                Next
            End If

            If Not bRoom Then
                lblErr.Text = "No Room Selected"
            Else
                Dim oRes As New clsReservations
                oRes.ReservationID = Request("ReservationID")
                oRes.UserID = Session("UserDBID")
                oRes.Load()
                oRes.CheckOutDate = dteExtend.Selected_Date
                oRes.Save()
                oRes = Nothing
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "window.opener.navigate('" & Request.ApplicationPath & "/marketing/editReservation.aspx?ReservationID=" & Request("ReservationID") & "');window.close();", True)
            End If
        Catch ex As Exception
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & ex.Message & "');", True)
            lblErr.Text = ex.Message
        Finally
        End Try
    End Sub
    Protected Sub gvRooms_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            Dim i As Integer
            Dim rooms() As String = e.Row.Cells(2).Text.Split("$")
            Dim roomTypes() As String = e.Row.Cells(3).Text.Split("$")
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
        End If
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If CheckSecurity("Reservations", "ExtendStay", , , Session("UserDBID")) Then
                Load_SIs()
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.close();", True)
            End If
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

End Class
