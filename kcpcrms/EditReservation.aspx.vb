Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Partial Class marketing_EditReservation
    Inherits System.Web.UI.Page
    Dim oRes As New clsReservations
This is a really important file
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not (IsPostBack) Then
            btnPrintLtr.Visible = False
            btnEmailLtr.Visible = False
            btnPrintRental.Visible = False
            btnEmailRental.Visible = False

            'DateField1.oPostBack = "ctl00$ContentPlaceHolder1$ddNights"
            If CheckSecurity("Reservations", "View", , , CType(Session("User"), User).PersonnelID) Then
                MultiView1.ActiveViewIndex = 0
                '*** Create view events I want to do this very much *** '
                If IsNumeric(Request("ReservationID")) Then
                    If CInt(Request("ReservationID")) > 0 Then
                        Dim oE As New clsEvents
                        Dim sErr As String = "I am a string error"
			Dim Google_Chrome As String = "Google browser"

                        If Not (oE.Find_View_Event("ReservationID", Request("ReservationID"), Resources.Resource.ViewEventTime, CType(Session("User"), User).PersonnelID, sErr)) Then
                            If sErr <> "" Then Response.Write(sErr)
                            If Not (oE.Create_View_Event("ReservationID", Request("ReservationID"), Resources.Resource.ViewEventTime, CType(Session("User"), User).PersonnelID, sErr)) Then
                                If sErr <> "" Then Response.Write(sErr)
                            End If
                        End If
                        oE = Nothing
                    End If
                End If

                '*** End View Events *** '
                oRes.ReservationID = IIf(IsNumeric(Request("ReservationID")), CInt(Request("ReservationID")), 0)
                oRes.ProspectID = Request("ProspectID")
                oRes.PackageIssuedID = Request("PackageIssuedID")
                oRes.Load()
                Dim oPros As New clsProspect
                If Request("ReservationID") = 0 Then
                    btnRentalLtr.Visible = False
                    btnPrintLtr.Visible = False
                    btnEmailLtr.Visible = False
                    If Request("packageissuedid") > 0 Then
                        Dim oPkg As New clsPackageIssued
                        oPkg.PackageIssuedID = Request("packageissuedid")
                        oPkg.Load()
                        oRes.ProspectID = oPkg.ProspectID
                        oPros.Prospect_ID = oPkg.ProspectID
                    Else
                        oPros.Prospect_ID = Request("ProspectID")
                    End If
                Else
                    oPros.Prospect_ID = oRes.ProspectID
                End If
                oPros.Load()
                lnkProspect.Text = oPros.Last_Name & ", " & oPros.First_Name
                prospectID.Text = oPros.Prospect_ID
                oPros = Nothing
                For i = 0 To 50
                    ddAdults.Items.Add(i)
                    ddChildren.Items.Add(i)
                    ddNights.Items.Add(i)
                Next i
                Load_SIs()
                Set_Values()
                Personnel_Link.Enabled = CheckSecurity("Reservations", "AddPersonnel", , , CType(Session("User"), User).PersonnelID)
                'UploadedDocs_Link
                Financials_Link.Enabled = CheckSecurity("Reservations", "ViewFinancials", , , CType(Session("User"), User).PersonnelID)
                'Rooms_Link.Enabled
            Else
                txtReservationID.Text = -1
                MultiView1.ActiveViewIndex = 12
            End If

            Logic_PrintLtr_EmailLtr()
            Logic_Print_Rental_Ltr_Email_Rental_Ltr()
        End If
    End Sub

    Private Sub Set_Values()
        txtReservationID.Text = oRes.ReservationID
        txtReservationNumber.Text = oRes.ReservationNumber
        prospectID.Text = oRes.ProspectID
        packageIssuedID.Text = oRes.PackageIssuedID
        txtStatusDate.Text = oRes.StatusDate
        If oRes.CheckInDate <> "" Then txtCheckInDate.Text = CDate(oRes.CheckInDate).ToShortDateString 'dteCheckInDate.Selected_Date = CDate(oRes.CheckInDate).ToShortDateString
        If oRes.CheckInDate <> "" Then SyncDateField1.Selected_Date = CDate(oRes.CheckInDate).ToShortDateString 'dteCheckInDate.Selected_Date = CDate(oRes.CheckInDate).ToShortDateString
        If oRes.CheckOutDate <> "" Then txtCheckOutDate.Text = CDate(oRes.CheckOutDate).ToShortDateString 'dteCheckOutDate.Selected_Date = CDate(oRes.CheckOutDate).ToShortDateString
        If oRes.DateBooked <> "" Then dteBookedDate.Selected_Date = CDate(oRes.DateBooked).ToShortDateString
        If oRes.CheckInDate <> "" And oRes.CheckOutDate <> "" Then
            ddNights.SelectedValue = DateDiff("d", oRes.CheckInDate, oRes.CheckOutDate)
        End If
        siStatus.Selected_ID = oRes.StatusID
        siLocation.Selected_ID = oRes.ResLocationID
        siResType.Selected_ID = oRes.TypeID
        siResSubType.Selected_ID = oRes.SubTypeID
        siResSource.Selected_ID = oRes.SourceID
        ckLockedInventory.Checked = oRes.LockInventory
        ddAdults.SelectedValue = oRes.NumberAdults
        ddChildren.SelectedValue = oRes.NumberChildren
        siCompany.Selected_ID = oRes.ResortCompanyID
        Dim oCombo As New clsComboItems
        If oRes.CheckInDate <> "" And oRes.CheckOutDate <> "" Then
            If Date.Today >= CDate(oRes.CheckInDate) And Date.Today < CDate(oRes.CheckOutDate) And oCombo.Lookup_ComboItem(oRes.StatusID) = "Booked" And oCombo.Lookup_ComboItem(oRes.ResLocationID) = "KCP" Then
                CIButton.visible = True
                COButton.visible = False
                intCIBtn.visible = False
                extStayBtn.visible = False
            ElseIf oCombo.Lookup_ComboItem(oRes.StatusID) = "In-House" And oCombo.Lookup_ComboItem(oRes.ResLocationID) = "KCP" Then
                COButton.visible = True
                extStayBtn.visible = True
                intCIBtn.visible = True
                CIButton.visible = False
            End If
        End If
        If Request("ReservationID") <> "0" Then
            List_Usages()
        Else
            ddNights.SelectedValue = 7
        End If
    End Sub
    Private Sub Load_SIs()
        siStatus.Connection_String = Resources.Resource.cns
        siStatus.ComboItem = "ReservationStatus"
        siStatus.Selected_ID = oRes.StatusID
        siStatus.Label_Caption = ""
        siStatus.Load_Items()
        siLocation.Connection_String = Resources.Resource.cns
        siLocation.ComboItem = "ReservationLocation"
        siLocation.Selected_ID = oRes.ResLocationID
        siLocation.Label_Caption = ""
        siLocation.Load_Items()
        siResType.Connection_String = Resources.Resource.cns
        siResType.ComboItem = "ReservationType"
        siResType.Label_Caption = ""
        siResType.Load_Items()
        siResSubType.Connection_String = Resources.Resource.cns
        siResSubType.ComboItem = "ReservationSubType"
        siResSubType.Label_Caption = ""
        siResSubType.Load_Items()
        siResSource.Connection_String = Resources.Resource.cns
        siResSource.ComboItem = "ReservationSource"
        siResSource.Selected_ID = oRes.SourceID
        siResSource.Label_Caption = ""
        siResSource.Load_Items()
        siCompany.Connection_String = Resources.Resource.cns
        siCompany.ComboItem = "ResortCompany"
        siCompany.Selected_ID = oRes.ResortCompanyID
        siCompany.Label_Caption = ""
        siCompany.Load_Items()
    End Sub

    Protected Sub List_Usages()
        Dim usages() As String
        Dim uDisplay As String = ""
        Dim i As Integer
        usages = oRes.Get_Usages(Request("ReservationID")).Split("|")
        If usages(0) <> "" Then
            For i = 0 To UBound(usages) Step 4
                If uDisplay = "" Then
                    uDisplay = "<a href = " & Chr(10) & "javascript:modal.mwindow.open('" & Request.ApplicationPath & "/marketing/editUsage.aspx?UsageID=" & usages(i) & "','win01',690,450);" & Chr(10) & ">" & usages(i + 3) & " - " & usages(i + 2) & "</a>"
                Else
                    uDisplay = uDisplay & "<br /><a href = " & Chr(10) & "javascript:modal.mwindow.open('" & Request.ApplicationPath & "/marketing/editUsage.aspx?UsageID=" & usages(i) & "','win01',690,450);" & Chr(10) & ">" & usages(i + 3) & " - " & usages(i + 2) & "</a>"
                End If
            Next i
        End If
        lblUsages.Text = uDisplay
    End Sub
    Protected Sub Notes_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Notes_Link.Click
        If txtReservationID.Text > 0 Then
            MultiView1.ActiveViewIndex = 3
            Notes1.KeyField = Notes1.KeyField + ",reservationbookingstatusid,reservationconfirmationstatusid"
            Notes1.KeyValue = txtReservationID.Text
            Notes1.Display()
        End If
    End Sub

    Protected Sub Reservation_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Reservation_Link.Click
        If txtReservationID.text > 0 Then
            List_Usages()
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub Events_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events_Link.Click
        If txtReservationID.Text > 0 Then
            MultiView1.ActiveViewIndex = 4
            Dim events As New clsEvents
            events.KeyField = "ReservationID"
            events.KeyValue = txtReservationID.text
            gvEvents.DataSource = events.List
            gvEvents.DataBind()
        End If
    End Sub

    Protected Sub Personnel_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Personnel_Link.Click
        If txtReservationID.text > 0 Then
            MultiView1.ActiveViewIndex = 7
            PersonnelTrans1.KeyField = "ReservationID"
            PersonnelTrans1.KeyValue = txtReservationID.Text
            PersonnelTrans1.Load_Trans()
        End If
    End Sub

    Protected Sub Tours_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Tours_Link.Click
        If txtReservationID.Text > 0 Then
            MultiView1.ActiveViewIndex = 1
            Dim tours As New clsTour
            Dim dr As SqlDataSource
            dr = tours.List(0, "ReservationID", txtReservationID.text)
            gvTours.DataSource = dr
            gvTours.DataBind()
        End If
    End Sub
    Protected Sub gvTours_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Tours" Then
            If e.Row.RowIndex > -1 Then
                If e.Row.Cells(2).Text <> "&nbsp;" Then
                    e.Row.Cells(2).Text = CDate(e.Row.Cells(2).Text).ToShortDateString
                End If
            End If
        End If
    End Sub
    Protected Sub UserFields_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UserFields_Link.Click
        If txtReservationID.Text > 0 Then
            MultiView1.ActiveViewIndex = 6
            UF.KeyField = "Reservation"
            UF.KeyValue = CInt(txtReservationID.Text)
            UF.Load_List()
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim oRes As New clsReservations
        Dim bProceed As Boolean = True
        Dim err As String = ""
        If txtReservationID.Text > 0 Then
            If CheckSecurity("Reservations", "Edit", , , CType(Session("User"), User).PersonnelID) Then
                oRes.ReservationID = txtReservationID.Text
                oRes.Load()
                If oRes.CheckInDate & "" <> "" And oRes.CheckOutDate & "" <> "" Then
                    If Date.Compare(CDate(txtCheckInDate.Text), CDate(oRes.CheckInDate)) <> 0 Or Date.Compare(CDate(txtCheckOutDate.Text), oRes.CheckOutDate) <> 0 Or siResType.Selected_ID <> oRes.TypeID Or siLocation.Selected_ID <> oRes.ResLocationID Then
                        If oRes.Get_Room_Count(Request("ReservationID")) > 0 Then
                            bProceed = False
                            err = "There is a Room Assigned to This Reservation. Please Remove The Room Before Making Changes."
                        End If
                    End If
                End If
                If oRes.LockInventory And Not (ckLockedInventory.Checked) Then
                    If Not (CheckSecurity("Reservations", "UnLockInventory", , , CType(Session("User"), User).PersonnelID)) Then
                        bProceed = False
                        err = "You Do Not Have Persmission to Unlock Inventory."
                    End If
                End If
                Dim oCombo As New clsComboItems
                If (oCombo.Lookup_ComboItem(oRes.StatusID) <> "Cancelled" And oCombo.Lookup_ComboItem(oRes.StatusID) <> "Reset") And (oRes.Get_Room_Count(Request("ReservationID")) > 0) And (oCombo.Lookup_ComboItem(siStatus.Selected_ID) = "Cancelled" Or oCombo.Lookup_ComboItem(siStatus.Selected_ID) = "Reset") Then
                    bProceed = False
                    err = "You Must Remove all Rooms Before Cancelling or Resetting a Reservation."
                End If

                If oCombo.Lookup_ComboItem(oRes.StatusID) = "Completed" And oCombo.Lookup_ComboItem(siStatus.Selected_ID) <> "Completed" Then
                    bProceed = False
                    err = "You Can Not Change the Status of A Completed Reservation."
                End If

                Dim oTour As New clsTour
                If oCombo.Lookup_ComboItem(siStatus.Selected_ID) = "Booked" And oCombo.Lookup_ComboItem(oRes.StatusID) <> "Booked" And (oCombo.Lookup_ComboItem(siResType.Selected_ID) = "Marketing" Or oTour.Get_Tour_By_Res(oRes.ReservationID) > 0) Then
                    Dim oDNS As New clsDoNotSellList
                    If oDNS.Get_Status(oRes.ProspectID) = "Remove" Then
                        bProceed = False
                        err = "This Customer is on the Do Not Sell List and Can Not Book a Reservation."
                    End If
                    oDNS = Nothing
                End If
                oTour = Nothing
                oCombo = Nothing
            Else
                bProceed = False
                err = "You Do Not Have Permission to Edit a Reservation Record."
            End If
        Else
            If Not (CheckSecurity("Reservations", "Add", , , CType(Session("User"), User).PersonnelID)) Then
                bProceed = False
                err = "You Do Not Have Permission to Create A Reservation Record."
            Else
                Dim oCombo As New clsComboItems
                If oCombo.Lookup_ComboItem(siStatus.Selected_ID) = "Booked" and oCombo.Lookup_ComboItem(siResType.Selected_ID) = "Marketing" Then
                    Dim oDNS As New clsDoNotSellList
                    If oDNS.Get_Status(Request("ProspectID")) = "Remove" Then
                        bProceed = False
                        err = "This Customer is on the Do Not Sell List and Can Not Book a Reservation."
                    End If
                    oDNS = Nothing
                End If
                oCombo = Nothing
            End If
        End If

        If bProceed Then
            Update_Values()
        Else
            If txtReservationID.Text > 0 Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & err & "');window.navigate('editReservation.aspx?reservationid=" & txtReservationID.Text & "');", True)
                '                Response.Redirect("editReservation.aspx?reservationid=" & txtReservationID.Text)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & err & "');", True)
                'txtCHeckInDate.text = oRes.CheckInDate
                'txtCheckOutDate.text = oRes.CheckOutDate
                'ddNights.SelectedValue = DateDiff("d", oRes.CheckInDate, oRes.CheckOutDate)
            End If
        End If
        oRes = Nothing
    End Sub

    Private Sub Update_Values()
        oRes.ReservationID = txtReservationID.text
        oRes.Load()
        oRes.ReservationNumber = txtReservationNumber.Text
        oRes.ResLocationID = siLocation.Selected_ID
        oRes.TypeID = siResType.Selected_ID
        oRes.SubTypeID = siResSubType.Selected_ID
        oRes.StatusID = siStatus.Selected_ID
        oRes.SourceID = siResSource.Selected_ID
        oRes.CheckInDate = IIf(IsDate(txtCheckInDate.Text), txtCheckInDate.Text, "")
        oRes.CheckOutDate = IIf(IsDate(txtCheckOutDate.Text), txtCheckOutDate.Text, "")
        oRes.DateBooked = dteBookedDate.Selected_date
        oRes.ProspectID = prospectID.text
        oRes.PackageIssuedID = packageIssuedID.text
        oRes.LockInventory = ckLockedInventory.checked
        oRes.NumberChildren = ddChildren.SelectedValue
        oRes.NumberAdults = ddAdults.SelectedValue
        oRes.ResortCompanyID = siCompany.Selected_ID
        oRes.UserID = CType(Session("User"), User).PersonnelID
        oRes.Save()

        Logic_PrintLtr_EmailLtr()
        Logic_Print_Rental_Ltr_Email_Rental_Ltr()

        txtReservationID.Text = oRes.ReservationID
        lblError.Text = oRes.Err
        If Request("ReservationID") = 0 Then
            Response.Redirect("editReservation.aspx?reservationid=" & oRes.ReservationID)
        Else
            Set_Values()
        End If
    End Sub

    Protected Sub gvTours_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTours.SelectedIndexChanged
        Dim row As gridviewrow = gvTours.selectedRow
        Response.Redirect("editTour.aspx?tourid=" & row.Cells(1).Text)
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim oDNS As New clsDoNotSellList
        Dim oRes As New clsReservations
        oRes.ReservationID = txtReservationID.Text
        oRes.Load()
        If oDNS.Get_Status(oRes.ProspectID) = "Remove" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('This Prospect Is On The Do Not Sell List And Is Unable To Tour.');", True)
        Else
            Response.Redirect("editTour.aspx?tourid=0&reservationID=" & txtReservationID.Text)
        End If
        oRes = Nothing
        oDNS = Nothing
    End Sub

    Protected Sub Rooms_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Rooms_Link.Click
        If txtReservationID.Text > 0 Then
            MultiView1.ActiveViewIndex = 8
            Dim oRes As New clsReservations
            oRes.ReservationID = txtReservationID.Text
            oRes.Load()
            gvRoomList1.DataSource = oRes.List_Rooms(oRes.ReservationID)
            Dim sKeys(0) As String
            sKeys(0) = "RoomID"
            gvRoomList1.DataKeynames = sKeys
            gvRoomList1.DataBind()
            lblRoomError.Text = oRes.Err
            oRes = Nothing
        End If
    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        If txtReservationID.text > 0 Then
            Dim oRes As New clsReservations
            Dim daysDiff As Long = 0
            oRes.ReservationID = txtReservationID.Text
            oRes.Load()
            If oRes.CheckInDate <> "" And oRes.CheckOutDate <> "" Then
                daysDiff = DateDiff("d", CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate))
                If oRes.val_Res_Location(oRes.ReservationID) Then
                    If oRes.TypeID = 0 Then
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Select A Reservation Type.');", True)
                    Else
                        If oRes.val_Res_Amt(oRes.ReservationID, daysDiff * 2) Then
                            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/roomwizard1.aspx?ReservationID=" & oRes.ReservationID & "','win01',690,450);", True)
                            'ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "get_Res_Rooms(" & Request("ReservationID") & ");val_ResLoc(" & Request("ReservationID") & ");", True)
                        Else
                            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Insufficient Amount Invoiced to Add Room.');", True)
                        End If

                    End If
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Location Must Be Set to KCP in Order to Add Room.');", True)
                End If
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Both the Check-In Date and the Check-Out Date must be saved before adding a room.');", True)
            End If
            oRes = Nothing
        End If
    End Sub

    Protected Sub CIButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CIButton.Click
        If txtReservationID.text > 0 Then
            Dim oRes As New clsReservations
            If oRes.Get_Room_Count(txtReservationID.Text) = 0 Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Rooms Must Be Added Prior to Checking In.');", True)
            Else
                If CheckSecurity("Reservations", "CheckIn", , , CType(Session("User"), User).PersonnelID) Then
                    If oRes.val_CheckIn_Financials(txtReservationID.Text) Then
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/storeCreditCard.aspx?ReservationID=" & txtReservationID.Text & "','win01',350,350);", True)
                    Else
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Reservation Balance Must Be Paid Before Checking In.');", True)
                    End If
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('You Do Not Have Permission To Check In Reservations.');", True)
                End If
            End If
        End If
    End Sub

    Protected Sub Financials_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Financials_Link.Click
        If txtReservationID.text > 0 Then
            'Please Set the prospectid
            Dim oRes As New clsReservations
            oRes.ReservationID = txtReservationID.Text
            oRes.Load()
            Financials1.ProspectID = oRes.ProspectID
            Financials1.KeyValue = txtReservationID.Text
            Financials1.KeyField = "ReservationID"
            Financials1.View = "reservation"
            Financials1.Display()
            MultiView1.ActiveViewIndex = 2
            oRes = Nothing
        End If
    End Sub

    Protected Sub gvRoomList1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.Cells(5).Text = "NO" Then
                e.Row.Cells(6).Visible = False
            End If
            e.Row.Cells(5).Visible = False
            If e.Row.Cells(7).Text = "NO" Then
                e.Row.Cells(8).Visible = False
            End If
            e.Row.Cells(7).Visible = False
        End If
    End Sub
    Protected Sub gvRoomList1_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvRoomList1.RowCommand
        Dim roomID As Integer
        roomID = Convert.ToInt32(gvRoomList1.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        If e.CommandName.CompareTo("RemoveRoom") = 0 Then
            Dim oRes As New clsReservations
            Dim oCombo As New clsComboItems
            oRes.ReservationID = txtReservationID.Text
            oRes.Load()
            oRes.UserID = CType(Session("User"), User).PersonnelID
            If CheckSecurity("Reservations", "Remove" & oCombo.Lookup_ComboItem(oRes.TypeID) & "Room", , , CType(Session("User"), User).PersonnelID) Then
                If oRes.LockInventory Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('The Inventory For this Reservation Is Locked And Can Not Be Removed.');", True)
                Else
                    If oCombo.Lookup_ComboItem(oRes.StatusID) = "In-House" Then
                        oRes.Remove_Room(roomID, oRes.ReservationID, CDate(System.DateTime.Now.ToShortDateString), oRes.CheckOutDate)
                    Else
                        oRes.Remove_Room(roomID, oRes.ReservationID, oRes.CheckInDate, oRes.CheckOutDate)
                    End If
                    gvRoomList1.DataSource = oRes.List_Rooms(oRes.ReservationID)
                    Dim sKeys(0) As String
                    sKeys(0) = "RoomID"
                    gvRoomList1.DataKeyNames = sKeys
                    gvRoomList1.DataBind()
                End If
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('You Do Not Have Permission to Remove a Room from a " & oCombo.Lookup_ComboItem(oRes.TypeID) & " Reservation.');", True)
            End If
            oRes = Nothing
            oCombo = Nothing
        ElseIf e.CommandName.CompareTo("SwapRoom") = 0 Then
            Dim oRes As New clsReservations
            oRes.ReservationID = txtReservationID.Text
            oRes.Load()
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/roomswap.aspx?ReservationID=" & oRes.ReservationID & "&RoomID=" & roomID & "','win01',690,450);", True)
            oRes = Nothing
        End If
    End Sub

    Protected Sub extStayBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles extStayBtn.Click
        If CheckSecurity("Reservations", "ExtendStay", , , CType(Session("User"), User).PersonnelID) Then
            Dim oRes As New clsReservations
            Dim daysDiff As Integer
            oRes.ReservationID = txtReservationID.Text
            oRes.Load()
            daysDiff = DateDiff("d", CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate))
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/extendstay.aspx?ReservationID=" & oRes.ReservationID & "','win01',690,450);", True)
            oRes = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('You Do Not Have Persmission to Extend Stays.');", True)
        End If
    End Sub

    Protected Sub HotelAccom_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HotelAccom_Link.Click
        If txtReservationID.Text > 0 Then
            MultiView1.ActiveViewIndex = 10
            Dim oAccom As New clsAccommodation
            gvAccom.DataSource = oAccom.List_Accoms("Reservation", txtReservationID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "AccommodationID"
            gvAccom.DataKeynames = sKeys
            gvAccom.DataBind()
        End If
    End Sub

    Protected Sub gvAccom_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(1).Visible = False
            If e.Row.RowIndex > -1 Then
                If Trim(e.Row.Cells(3).Text & "") <> "" Then
                    Dim sDate() As String = e.Row.Cells(3).Text.Split(" ")
                    e.Row.Cells(3).Text = sDate(0) 'e.Row.Cells(3).Text & " HI" ' CDate(Trim(e.Row.Cells(3).Text)).ToShortDateString
                End If
                If Trim(e.Row.Cells(4).Text & "") <> "" Then
                    Dim sDate() As String = e.Row.Cells(4).Text.Split(" ")
                    e.Row.Cells(4).Text = sDate(0) 'e.Row.Cells(3).Text & " HI" ' CDate(Trim(e.Row.Cells(3).Text)).ToShortDateString
                End If
            End If
        End If
    End Sub

    Protected Sub gvAccom_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAccom.SelectedIndexChanged
        Dim row As GridViewrow = gvAccom.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/editAccommodation.aspx?AccommodationID=" & row.Cells(1).Text & "','win01',690,450);", True)
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If txtReservationID.Text > 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/editAccommodation.aspx?AccommodationID=0&ReservationID=" & txtReservationID.Text & "','win01',690,450);", True)
        End If
    End Sub

    Protected Sub intCIBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles intCIBtn.Click
        Dim oPros As New clsProspect
        Dim oRes As New clsReservations
        Dim prosName As String = ""
        oRes.ReservationID = txtReservationID.Text
        oRes.Load()
        oPros.Prospect_ID = oRes.ProspectID
        oPros.Load()
        prosName = Left(oPros.First_Name, 10) & " " & Left(oPros.Last_Name, 10)
        oPros = Nothing
        If oRes.Interface_CheckIn(oRes.ReservationID, prosName) Then
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & oRes.Err & "');", True)
        End If
        oRes = Nothing
    End Sub

    Protected Sub COButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles COButton.Click
        If CheckSecurity("Reservations", "CheckOut", , , CType(Session("User"), User).PersonnelID) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/confirmResCO.aspx?ReservationID=" & txtReservationID.Text & "','win01',150,175);", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('You Do Not Have Persmission to Check Out Reservations.');", True)
        End If
    End Sub

    Protected Sub lnkProspect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkProspect.Click
        Response.Redirect(Request.ApplicationPath & "/marketing/editprospect.aspx?prospectid=" & prospectID.Text) 'Request("ProspectID"))
    End Sub

    Protected Sub VoiceStamps_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles VoiceStamps_Link.Click
        If txtReservationID.Text > 0 Then
            MultiView1.ActiveViewIndex = 9
            VoiceStamps1.KeyValue = txtReservationID.Text
            VoiceStamps1.Display()
        End If
    End Sub

    Protected Sub lnkUsages_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkUsages.Click
        List_Usages()
    End Sub

    Protected Sub Unnamed1_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        Calendar1.Visible = Not (Calendar1.Visible)
        If IsDate(txtCheckInDate.Text) Then
            Calendar1.SelectedDate = CDate(txtCheckInDate.Text)
        Else
            Calendar1.SelectedDate = Date.Today
        End If
        txtCheckInDate.Text = Calendar1.SelectedDate
    End Sub

    Protected Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
        txtCheckInDate.Text = Calendar1.SelectedDate.ToShortDateString
        txtCheckOutDate.Text = CDate(txtCheckInDate.Text).AddDays(ddNights.SelectedValue).ToShortDateString
        Calendar1.Visible = False

    End Sub

    Protected Sub ddNights_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddNights.SelectedIndexChanged
        If txtCheckInDate.Text <> "" Then
            txtCheckOutDate.Text = CDate(txtCheckInDate.Text).AddDays(ddNights.SelectedValue).ToShortDateString
        End If
    End Sub

    Protected Sub SpecialNeeds_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SpecialNeeds_Link.Click
        If txtReservationID.Text > 0 Then
            SpecialNeeds1.KeyValue = txtReservationID.Text
            SpecialNeeds1.Display()
            MultiView1.ActiveViewIndex = 11
        End If
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs) Handles btnRentalLtr.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/confLetter.aspx?ReservationID=" & txtReservationID.Text & "','win01',490,450);", True)
    End Sub

    Protected Sub btnPrintLtr_Click(sender As Object, e As System.EventArgs) Handles btnPrintLtr.Click

        If CheckSecurity("Owners", "Print Letter", , , CType(Session("User"), User).PersonnelID) Then
            With New clsReservations
                If .Get_Room_Count(Request("ReservationID")) < 1 Then
                    Logic_PrintLtr_EmailLtr()
                    ClientScript.RegisterClientScriptBlock(Me.GetType, DateTime.Now.Millisecond, "alert('Letter cannot be sent, please review your reservation for errors.');", True)
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/resLetter.aspx?ReservationID=" & txtReservationID.Text & "','win01',690,450);", True)
                End If
            End With
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Access Denied.');", True)
        End If
    End Sub

    Protected Sub btnEmailLtr_Click(sender As Object, e As System.EventArgs) Handles btnEmailLtr.Click

        If CheckSecurity("Owners", "Email Letter", , , CType(Session("User"), User).PersonnelID) Then
            With New clsReservations
                If .Get_Room_Count(Request("ReservationID")) < 1 Then
                    Logic_PrintLtr_EmailLtr()
                    ClientScript.RegisterClientScriptBlock(Me.GetType, DateTime.Now.Millisecond, "alert('Email cannot be sent, please review your reservation for errors.');", True)
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/resLetter.aspx?ReservationID=" & txtReservationID.Text & "&function=Email','win01',690,450);", True)
                End If
            End With
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Access Denied.');", True)
        End If
    End Sub

    Protected Sub UploadedDocs_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UploadedDocs_Link.Click
        If txtReservationID.Text > 0 Then
            MultiView1.ActiveViewIndex = 5
            UploadedDocs1.KeyField = "ReservationID"
            UploadedDocs1.KeyValue = txtReservationID.Text
            UploadedDocs1.List()
        End If
    End Sub

    Protected Sub btnRegCard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRegCard.Click
        If txtReservationID.Text > 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Pop", "var mwin = window.open('" & Request.ApplicationPath & "/frontdesk/singleregcard.aspx?reservationid=" & txtReservationID.Text & "');", True)
        End If
    End Sub

    'Protected Sub DateField1_Date_Updated() Handles DateField1.Date_Updated
    '    txtCheckOutDate.Text = CDate(DateField1.Selected_Date).AddDays(ddNights.SelectedValue).ToShortDateString
    'End Sub

    Protected Sub gvRoomList1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvRoomList1.SelectedIndexChanged

    End Sub

    Protected Sub SyncDateField1_Date_Updated() Handles SyncDateField1.Date_Updated
        txtCheckInDate.Text = SyncDateField1.Selected_Date
        txtCheckOutDate.Text = CDate(txtCheckInDate.Text).AddDays(ddNights.SelectedValue).ToShortDateString
        Calendar1.Visible = False
    End Sub

    Protected Sub btnEmailRental_Click(sender As Object, e As System.EventArgs) Handles btnEmailRental.Click
        If Logic_Ensure_Reservation_Has_Room_And_Tour_Within_CheckIn_And_CheckOut() Then
            Dim oEmail As New clsEmail
            Dim oRes As New clsReservations
            Dim letterID As Integer = 0
            Dim emailTo As String = ""
            oRes.ReservationID = txtReservationID.Text
            oRes.Load()
            emailTo = oEmail.Get_Primary_Email(oRes.ProspectID)
            If emailTo <> "" Then
                Dim oLetter As New clsReservationLetters
                letterID = oLetter.Find_Letter(txtReservationID.Text)
                If letterID > 0 Then
                    Dim body As String = ""
                    Dim oPros As New clsProspect
                    Dim oAdd As New clsAddress
                    Dim oCombo As New clsComboItems
                    Dim oAccom As New clsAccom
                    Dim oAcc As New clsAccommodation
                    Dim oTour As New clsTour
                    Dim size As Integer = 0
                    Dim rmSize As String = ""
                    oPros.Prospect_ID = oRes.ProspectID
                    oPros.Load()
                    oAdd.AddressID = oAdd.Get_Address_ID(oPros.Prospect_ID)
                    oAdd.Load()
                    oLetter.ReservationLetterID = letterID
                    oLetter.Load()
                    body = "<html><body>" & Server.HtmlDecode(oLetter.LetterText)
                    body = Replace(body, "<DATE>", DateTime.Now.ToShortDateString)
                    body = Replace(body, "<NAME>", oPros.First_Name & " " & oPros.Last_Name)
                    body = Replace(body, "<ADDRESS>", oAdd.Address1)
                    body = Replace(body, "<CITY>", oAdd.City)
                    body = Replace(body, "<STATE>", oCombo.Lookup_ComboItem(oAdd.StateID))
                    body = Replace(body, "<ZIP>", oAdd.PostalCode)
                    body = Replace(body, "<COUNTRY>", oCombo.Lookup_ComboItem(oAdd.CountryID))
                    body = Replace(body, "<SOURCE>", oCombo.Lookup_ComboItem(oRes.SourceID))
                    body = Replace(body, "<RESID>", oRes.ReservationID)
                    body = Replace(body, "<RESNUMBER>", oRes.ReservationNumber)
                    If IsDBNull(oRes.CheckInDate) Or oRes.CheckInDate.ToString = "" Then
                        body = Replace(body, "<CHECKIN>", "N/A")
                        body = Replace(body, "<CHECKOUT>", "N/A")
                        body = Replace(body, "<DAYS>", "N/A")
                        body = Replace(body, "<NIGHTS>", "N/A")
                    Else
                        body = Replace(body, "<CHECKIN>", CDate(oRes.CheckInDate).ToShortDateString)
                        body = Replace(body, "<CHECKOUT>", CDate(oRes.CheckOutDate).ToShortDateString)
                        body = Replace(body, "<DAYS>", DateDiff(DateInterval.Day, CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate)) + 1)
                        body = Replace(body, "<NIGHTS>", DateDiff(DateInterval.Day, CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate)))
                    End If
                    size = oRes.Get_BD_Count(oRes.ReservationID)
                    If size = 0 Then
                        rmSize = "N/A"
                    Else
                        rmSize = size & "BD"
                    End If
                    body = Replace(body, "<SIZE>", rmSize)
                    body = Replace(body, "<BALANCE>", oRes.Get_Total_Balance(oRes.ReservationID))
                    body = Replace(body, "<DEPOSIT>", oRes.Get_Total_Payments(oRes.ReservationID))
                    body = Replace(body, "<TOTAL>", oRes.Get_Inv_Amount(oRes.ReservationID) - oRes.Get_Total_Adjustments(oRes.ReservationID))
                    If oAcc.Get_Accom_By_Res(oRes.ReservationID) = 0 Then
                        body = Replace(body, "<ACCOMSIZE>", "N/A")
                        body = Replace(body, "<ACCOMNAME>", "N/A")
                        body = Replace(body, "<ACCOMADDRESS>", "N/A")
                        body = Replace(body, "<ACCOMCITY>", "N/A")
                        body = Replace(body, "<ACCOMSTATE>", "N/A")
                        body = Replace(body, "<ACCOMZIP>", "N/A")
                        body = Replace(body, "<ACCOMDIRECTIONS>", "XX")
                        body = Replace(body, "<CHECKINDIRECTIONS>", "XX")
                    Else
                        oAcc.AccommodationID = oAcc.Get_Accom_By_Res(oRes.ReservationID)
                        oAcc.Load()
                        body = Replace(body, "<ACCOMSIZE>", oCombo.Lookup_ComboItem(oAcc.RoomTypeID))
                        oAccom.AccomID = oAcc.AccomID
                        oAccom.Load()
                        body = Replace(body, "<ACCOMNAME>", oAccom.AccomName)
                        body = Replace(body, "<ACCOMADDRESS>", oAccom.Address)
                        body = Replace(body, "<ACCOMCITY>", oAccom.City)
                        body = Replace(body, "<ACCOMSTATE>", oCombo.Lookup_ComboItem(oAccom.StateID))
                        body = Replace(body, "<ACCOMZIP>", oAccom.PostalCode)
                        body = Replace(body, "<ACCOMDIRECTIONS>", Server.HtmlDecode(oAccom.Directions) & " " & oAccom.AccomID)
                        If oAcc.CheckInLocationID = 0 Then
                            body = Replace(body, "<CHECKINDIRECTIONS>", "XX")
                        Else
                            Dim oAccomCheckIN As New clsAccomCheckInLocations
                            If oAccomCheckIN.Lookup_By_ID(oAcc.CheckInLocationID) = 0 Then
                                body = Replace(body, "<CHECKINDIRECTIONS>", "XXX")
                            Else
                                oAccomCheckIN.ID = oAccomCheckIN.Lookup_By_ID(oAcc.CheckInLocationID)
                                oAccomCheckIN.Load()
                                body = Replace(body, "<CHECKINDIRECTIONS>", Server.HtmlDecode(oAccomCheckIN.Directions))
                            End If
                            oAccomCheckIN = Nothing
                        End If

                    End If
                    If oTour.Get_Tour_By_Res(oRes.ReservationID) > 0 Then
                        oTour.TourID = oTour.Get_Tour_By_Res(oRes.ReservationID)
                        oTour.Load()
                        If IsDBNull(oTour.TourDate) Or oTour.TourDate.ToString = "" Then
                            body = Replace(body, "<TOURDATE>", "N/A")
                            body = Replace(body, "<TOURTIME>", "N/A")
                        Else
                            body = Replace(body, "<TOURDATE>", CDate(oTour.TourDate).ToShortDateString)
                            Dim tTime As String
                            tTime = oCombo.Lookup_ComboItem(oTour.TourTime)
                            If CInt(tTime) > 1259 Then
                                tTime = CInt(tTime) - 1200 & " PM"
                            ElseIf CInt(tTime) > 1259 Then
                                tTime = CInt(tTime) & " PM"
                            Else
                                tTime = tTime & " AM"
                            End If
                            body = Replace(body, "<TOURTIME>", tTime)
                        End If
                        Dim oPremIss As New clsPremiumIssued
                        body = Replace(body, "<GIFTS>", oPremIss.Get_Gifts(oTour.TourID))
                        oPremIss = Nothing
                    End If
                    body = body & "</body></html>"
                    Send_Mail(emailTo, oLetter.EmailAddress, oLetter.Subject, body, True)
                    Dim oUpload As New clsUploadedDocs
                    oUpload.FileID = 0
                    oUpload.Load()
                    oUpload.KeyField = "ReservationID"
                    oUpload.KeyValue = oRes.ReservationID
                    oUpload.Name = "Confirmation Letter Sent " & System.DateTime.Now.ToShortDateString
                    oUpload.DateUploaded = System.DateTime.Now
                    oUpload.ContentText = body
                    oUpload.UploadedByID = CType(Session("User"), User).PersonnelID
                    oUpload.Save()
                    oUpload = Nothing
                    Dim oNote As New clsNotes
                    oNote.NoteID = 0
                    oNote.Load()
                    oNote.UserID = CType(Session("User"), User).PersonnelID
                    oNote.CreatedByID = CType(Session("User"), User).PersonnelID
                    oNote.DateCreated = System.DateTime.Now
                    oNote.KeyField = "ReservationID"
                    oNote.KeyValue = oRes.ReservationID
                    oNote.Note = "Emailed Confirmation Letter to " & emailTo & " on " & System.DateTime.Now.ToShortDateString & "."
                    oNote.Save()
                    oNote = Nothing
                    oAdd = Nothing
                    oTour = Nothing
                    oCombo = Nothing
                    oAccom = Nothing
                    oAcc = Nothing
                    oPros = Nothing
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Confirmation Letter Sent.');", True)
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('This Reservation Does Not Have a Confirmation Letter.');", True)
                End If
                oLetter = Nothing
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('This Prospect Does Not Have A Primary Email Address.');", True)
            End If
            oRes = Nothing
            oEmail = Nothing
        Else
            Logic_Print_Rental_Ltr_Email_Rental_Ltr()
            ClientScript.RegisterClientScriptBlock(Me.GetType, DateTime.Now.Millisecond, "alert('Letter cannot be sent, please review your reservation for errors.');", True)
        End If
    End Sub

    Protected Sub btnPrintRental_Click(sender As Object, e As System.EventArgs) Handles btnPrintRental.Click
        If Logic_Ensure_Reservation_Has_Room_And_Tour_Within_CheckIn_And_CheckOut() Then
            Dim letterID As Integer = 0
            Dim oLetter As New clsReservationLetters
            letterID = oLetter.Find_Letter(txtReservationID.Text)
            If letterID > 0 Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/PrintResConfLetter.aspx?ResID=" & txtReservationID.Text & "&LetterID=" & letterID & "','win01',690,450);", True)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('This Reservation Does Not Have a Confirmation Letter.');", True)
            End If
            oLetter = Nothing
        Else
            Logic_Print_Rental_Ltr_Email_Rental_Ltr()
            ClientScript.RegisterClientScriptBlock(Me.GetType, DateTime.Now.Millisecond, "alert('Letter cannot be sent, please review your reservation for errors.');", True)
        End If
    End Sub
    Private Sub Logic_PrintLtr_EmailLtr()
        If String.IsNullOrEmpty(siStatus.SelectedName) = False And String.IsNullOrEmpty(siResType.SelectedName) = False _
            And String.IsNullOrEmpty(txtCheckInDate.Text) = False Then
            If siStatus.SelectedName.Equals("Booked") And siResType.SelectedName.Equals("Owner") _
                And DateTime.Compare(DateTime.Parse(txtCheckInDate.Text).ToShortDateString, DateTime.Now.ToShortDateString) >= 0 Then

                btnPrintLtr.Visible = True
                btnEmailLtr.Visible = True
            End If
        End If
    End Sub
    Private Sub Logic_Print_Rental_Ltr_Email_Rental_Ltr()
        If String.IsNullOrEmpty(siStatus.SelectedName) = False And String.IsNullOrEmpty(siResType.SelectedName) = False _
           And String.IsNullOrEmpty(txtCheckInDate.Text) = False Then

            Dim reservation_id = Request("ReservationID")
            Dim tf = False


            With New clsReservations
                .ReservationID = reservation_id
                .Load()

                Dim package_issued_id = .PackageIssuedID
                Dim package_type = String.Empty
                Dim package_id = 0
                With New clsPackageIssued
                    .PackageIssuedID = package_issued_id
                    .Load()
                    package_id = .PackageID
                End With
                With New clsPackage
                    .PackageID = package_id
                    .Load()
                    package_type = New clsComboItems().Lookup_ComboItem(.TypeID)
                End With

                If package_type.Length > 0 Then
                    If package_type = "Tradeshow" Or package_type = "Tour Promotion" Or package_type = "Tour Package" Then
                        If (New clsTour).Get_Tour_By_Res(reservation_id) > 0 And .CheckInDate.Length > 0 And .CheckOutDate.Length > 0 Then
                            Dim tour = New clsTour

                            tour.TourID = (New clsTour).Get_Tour_By_Res(reservation_id)
                            tour.Load()
                            If tour.get_Tour_Status(tour.TourID).Equals("Booked") Then
                                If String.IsNullOrEmpty(tour.TourDate) = False Then

                                    If DateTime.Compare(DateTime.Parse(tour.TourDate).ToShortDateString(), DateTime.Parse(.CheckInDate).ToShortDateString) > 0 And
                                         DateTime.Compare(DateTime.Parse(tour.TourDate).ToShortDateString(), DateTime.Parse(.CheckOutDate).ToShortDateString) < 0 Then
                                        tf = True
                                    End If
                                End If
                            End If
                        End If
                    Else
                        tf = True
                    End If
                Else
                    tf = True
                End If

            End With

            If (siStatus.SelectedName.Equals("Booked") Or siStatus.SelectedName.Equals("Pending Payment")) And siResType.SelectedName.Equals("Marketing") _
                And DateTime.Compare(DateTime.Parse(txtCheckInDate.Text).ToShortDateString, DateTime.Now.ToShortDateString) >= 0 And tf = True Then

                btnEmailRental.Visible = True
                btnPrintRental.Visible = True
            End If
        End If
    End Sub
    Private Function Logic_Ensure_Reservation_Has_Room_And_Tour_Within_CheckIn_And_CheckOut() As Boolean
        Dim tf = False
        Dim reservation_id = Request("ReservationID")
        With New clsReservations
            .ReservationID = reservation_id
            .Load()
            '//- check room availability
            If .Get_Room_Count(reservation_id) > 0 Then
                '//- if reservation has a tour
                If (New clsTour).Get_Tour_By_Res(reservation_id) > 0 Then
                    '\\- ensure tour is booked and falls with checkin and checkout
                    Dim tour = New clsTour
                    tour.TourID = (New clsTour).Get_Tour_By_Res(reservation_id)
                    tour.Load()
                    If tour.get_Tour_Status(tour.TourID).Equals("Booked") Then
                        If String.IsNullOrEmpty(tour.TourDate) = False Then

                            If DateTime.Compare(DateTime.Parse(tour.TourDate).ToShortDateString(), DateTime.Parse(.CheckInDate).ToShortDateString) > 0 And
                                 DateTime.Compare(DateTime.Parse(tour.TourDate).ToShortDateString(), DateTime.Parse(.CheckOutDate).ToShortDateString) < 0 Then
                                tf = True
                            End If
                        End If
                    End If
                Else
                    tf = True
                End If
            End If
        End With
        Return tf
    End Function
End Class

