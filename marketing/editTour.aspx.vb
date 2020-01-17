Imports System.Data
Imports System.Data.SqlClient

Partial Class marketing_editTour
    Inherits System.Web.UI.Page
    Dim oTour As New clsTour

    Protected Sub Tour_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Tour_Link.Click
        If txtTourID.Text > 0 Then
            oTour.TourID = txtTourID.Text
            oTour.Load()
            Set_Values()
            MultiView1.ActiveViewIndex = 4
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If CheckSecurity("Tours", "View", , , Session("UserDBID")) Then
                '*** Create view events *** '
                If IsNumeric(Request("TourID")) Then
                    If CInt(Request("TourID")) > 0 Then
                        Dim oE As New clsEvents
                        Dim sErr As String = ""

                        If Not (oE.Find_View_Event("TourID", Request("TourID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                            If sErr <> "" Then Response.Write(sErr)
                            If Not (oE.Create_View_Event("TourID", Request("TourID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                                If sErr <> "" Then Response.Write(sErr)
                            End If
                        End If
                        oE = Nothing
                    End If
                End If

                '*** End View Events *** '
                oTour.TourID = IIf(IsNumeric(Request("TourID")), CInt(Request("TourID")), 0)
                oTour.Load()
                If oTour.TourID = 0 Then
                    oTour.ProspectID = IIf(IsNumeric(Request("ProspectID")), CInt(Request("ProspectID")), 0)
                    oTour.Load()
                End If
                MultiView1.ActiveViewIndex = 4

                siSource.Connection_String = Resources.Resource.cns
                siSource.ComboItem = "TourSource"
                siSource.Selected_ID = oTour.SourceID
                siSource.Label_Caption = ""
                siSource.Load_Items()
                Load_SIs()
                Set_Values()

                LblTourError.Text = oTour.Error_Message
            Else
                MultiView1.ActiveViewIndex = 8
                txtTourID.Text = -1
            End If
        End If

     
    End Sub

    Private Sub Load_SIs()

        siStatus.Connection_String = Resources.Resource.cns
        siStatus.ComboItem = "TourStatus"
        siStatus.Label_Caption = ""
        siStatus.Load_Items()
        siType.Connection_String = Resources.Resource.cns
        siType.ComboItem = "TourType"
        siType.Label_Caption = ""
        siType.Load_Items()
        siSubType.Connection_String = Resources.Resource.cns
        siSubType.ComboItem = "TourSubType"
        siSubType.Label_Caption = ""
        siSubType.Load_Items()
        siLocation.Connection_String = Resources.Resource.cns
        siLocation.ComboItem = "TourLocation"
        siLocation.Label_Caption = ""
        siLocation.Load_Items()
        siTourTime.Connection_String = Resources.Resource.cns
        siTourTime.ComboItem = "TourTime"
        siTourTime.Label_Caption = ""
        siTourTime.Load_Items()
        Campaign1.Selected_ID = oTour.CampaignID
        Campaign1.Load_Items()
    End Sub

    Private Sub Set_Values()
        txtTourID.Text = oTour.TourID
        If oTour.TourDate <> "" Then dteTourDate.Selected_Date = oTour.TourDate

        ''//--
        ''//-- 01-22-2015
        dteTourDate.Attributes.Remove("Date-Tour")
        If oTour.TourDate <> "" Then
            dteTourDate.Attributes.Add("Date-Tour", DateTime.Parse(oTour.TourDate).ToShortDateString())
        End If

        siStatus.Selected_ID = oTour.StatusID
        If oTour.TourDate <> "" Then
            If DateDiff(DateInterval.Day, CDate(oTour.TourDate), Date.Today) > 5 Then
                If Not (CheckSecurity("Tours", "ModifyTourStatusExtended", , , Session("UserDBID"))) Then
                    siStatus.Read_Only = True
                End If
            End If
        End If

        siLocation.Selected_ID = oTour.TourLocationID
        siType.Selected_ID = oTour.TypeID
        siSubType.Selected_ID = oTour.SubTypeID
        siSource.Selected_ID = oTour.SourceID
        siTourTime.Selected_ID = oTour.TourTime
        Campaign1.Selected_ID = oTour.CampaignID
        If oTour.BookingDate <> "" Then dteBookingDate.Selected_Date = oTour.BookingDate
        If Request("TourID") = 0 Then
            Dim oPros As New clsProspect
            If Request("ProspectID") > 0 Then
                oPros.Prospect_ID = Request("ProspectID")
                oPros.Load()
                hlPros.Text = oPros.Last_Name & ", " & oPros.First_Name
                hlPros.NavigateUrl = "editprospect.aspx?prospectid=" & Request("ProspectID")
            ElseIf Request("reservationID") > 0 Then
                Dim ores As New clsReservations
                ores.ReservationID = Request("reservationID")
                ores.Load()
                oPros.Prospect_ID = ores.ProspectID
                oPros.Load()
                hlPros.Text = oPros.Last_Name & ", " & oPros.First_Name
                hlPros.NavigateUrl = "editprospect.aspx?prospectid=" & oPros.Prospect_ID
                ores = Nothing
            Else
                Dim opkg As New clsPackageIssued
                opkg.PackageIssuedID = Request("PackageIssuedID")
                opkg.Load()
                oPros.Prospect_ID = opkg.ProspectID
                oPros.Load()
                hlPros.Text = oPros.Last_Name & ", " & oPros.First_Name
                hlPros.NavigateUrl = "editprospect.aspx?prospectid=" & opkg.ProspectID
                opkg = Nothing
            End If
            oPros = Nothing
        Else
            hlPros.Text = oTour.Get_Prospect_Name(oTour.TourID)
            hlPros.NavigateUrl = "editprospect.aspx?prospectid=" & oTour.ProspectID
        End If
    End Sub

    Protected Sub UserFields_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UserFields_Link.Click
        If txtTourID.Text > 0 Then
            MultiView1.ActiveViewIndex = 2
            UF.KeyField = "Tour"
            UF.KeyValue = CInt(txtTourID.Text)
            UF.Load_List()
        End If
    End Sub

    Protected Sub Events_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events_Link.Click
        If txtTourID.Text > 0 Then
            MultiView1.ActiveViewIndex = 1
            Events1.KeyField = "TourID"
            Events1.KeyValue = txtTourID.Text
            Events1.List()
        End If
    End Sub

    Protected Sub Personnel_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Personnel_Link.Click
        If txtTourID.Text > 0 Then
            MultiView1.ActiveViewIndex = 5
            PersonnelTrans1.KeyField = "TourID"
            PersonnelTrans1.KeyValue = txtTourID.Text
            PersonnelTrans1.Load_Trans()
        End If
    End Sub


    Private Sub Update_Values()
        If IsNumeric(Request("TourID")) Or IsNumeric(txtTourID.Text) Then
            oTour.TourID = IIf(CInt(Request("TourID")) > 0, Request("TourID"), txtTourID.Text)
            oTour.Load()
        End If
        If Request("TourID") = 0 Then
            If Request("ReservationID") > 0 Then
                Dim oRes As New clsReservations
                oRes.ReservationID = Request("ReservationID")
                oRes.Load()
                oTour.ProspectID = oRes.ProspectID
                oTour.ReservationID = Request("ReservationID")
                oRes = Nothing
            ElseIf Request("PackageIssuedID") > 0 Then
                Dim opkg As New clsPackageIssued
                opkg.PackageIssuedID = Request("PackageIssuedID")
                opkg.Load()
                oTour.ProspectID = opkg.ProspectID
                oTour.PackageIssuedID = Request("PackageIssuedID")
            ElseIf Request("ProspectID") > 0 Then
                oTour.ProspectID = Request("ProspectID")
            End If
        End If
        oTour.TourDate = dteTourDate.Selected_Date
        oTour.StatusID = siStatus.Selected_ID
        oTour.TourLocationID = siLocation.Selected_ID
        oTour.TypeID = siType.Selected_ID
        oTour.SubTypeID = siSubType.Selected_ID
        oTour.SourceID = siSource.Selected_ID
        oTour.BookingDate = dteBookingDate.Selected_Date
        oTour.CampaignID = Campaign1.Selected_ID
        oTour.TourTime = siTourTime.Selected_ID
        oTour.UserID = Session("UserDBID")
        
        If String.IsNullOrEmpty(oTour.TourDate) = False And _
            oTour.CampaignID > 0 And oTour.TourTime > 0 And (New clsComboItems).Lookup_ComboItem(oTour.StatusID) = "Booked" _
            And oTour.TourLocationID > 0 Then

            Dim twl = New clsTourWaveLimits()
            Try
                Dim ret_c = twl.IsLimitReached(twl.GetCampaignTypeID(oTour.CampaignID), _
                                            oTour.TourTime, oTour.TourDate, oTour.TourDate, oTour.TourLocationID)

                If ret_c = Integer.MinValue Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('Tour Wave/Date unavailable. Please choose another!');", True)
                    Return
                ElseIf ret_c < 1 Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('The maximum limit set for the wave has been reached. Please choose another!');", True)
                    Return
                End If
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try
        End If


        ''//--
        ''//-- 01/22/15 new special permission prohibits editing passed tour up to today'date
        If CheckSecurity("Tours", "ProhibitEditPassedTour", , , Session("UserDBID")) = True Then
            If String.IsNullOrEmpty(dteTourDate.Attributes("Date-Tour")) = False Then
                If DateDiff(DateInterval.Day, DateTime.Today, DateTime.Parse(dteTourDate.Attributes("Date-Tour"))) < 1 Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "popup", "alert('You can only modify tour having future tour date!');", True)
                    Return
                End If
            End If
        End If

        oTour.Save()
        siStatus.Read_Only = False
        If oTour.TourDate <> "" Then
            If DateDiff(DateInterval.Day, CDate(oTour.TourDate), Date.Today) > 5 Then
                If Not (CheckSecurity("Tours", "ModifyTourStatusExtended", , , Session("UserDBID"))) Then
                    siStatus.Read_Only = True
                End If
            End If
        End If

        If Request("TourID") = 0 Then
            If Request("ReservationID") > 0 Then
                Dim oRes As New clsReservations
                oRes.ReservationID = Request("ReservationID")
                oRes.Load()
                oRes.UserID = Session("UserDBID")
                oRes.TourID = oTour.TourID
                oRes.Save()
                oRes = Nothing
            End If
            Response.Redirect("editTour.aspx?TourID=" & oTour.TourID)
        Else
            txtTourID.Text = oTour.TourID
            LblTourError.Text = oTour.Error_Message
            hlPros.Text = oTour.Get_Prospect_Name(oTour.TourID)
            hlPros.NavigateUrl = "editprospect.aspx?prospectid=" & oTour.ProspectID
            'Label6.Text += "Complete"
        End If
    End Sub

    Protected Sub Notes_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Notes_Link.Click
        If txtTourID.Text > 0 Then
            MultiView1.ActiveViewIndex = 0
            Notes1.KeyValue = txtTourID.Text
            Notes1.Display()
        End If
    End Sub

    Protected Sub Financials_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Financials_Link.Click
        If txtTourID.Text > 0 Then
            oTour.TourID = txtTourID.Text
            oTour.Load()
            MultiView1.ActiveViewIndex = 3
            Financials1.KeyField = "TourID"
            Financials1.KeyValue = txtTourID.Text
            Financials1.View = "Tour"
            Financials1.ProspectID = oTour.ProspectID
            Financials1.Display()
        End If
    End Sub

    Protected Sub Premiums_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Premiums_Link.Click
        If txtTourID.Text > 0 Then
            MultiView1.ActiveViewIndex = 6
            Premiums1.KeyField = "TourID"
            Premiums1.KeyValue = txtTourID.Text
            Premiums1.Display()
        End If
    End Sub

    Protected Sub lbSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSave.Click
        If txtTourID.Text > 0 Then
            Dim oTour As New clsTour
            Dim oDNS As New clsDoNotSellList
            Dim oCombo As New clsComboItems
            oTour.TourID = txtTourID.Text
            oTour.Load()
            If oCombo.Lookup_ComboItem(siStatus.Selected_ID) = "Booked" And oCombo.Lookup_ComboItem(oTour.StatusID) <> "Booked" Then
                If oDNS.Get_Status(oTour.ProspectID) = "Remove" Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "modal.mwindow.open('DoNotSellListOverRide.aspx?KeyField=Tour&KeyValue=" & txtTourID.Text & "&Source=BookTour','win01',350,350);", True)
                Else
                    Save_Tour()
                End If
            Else
                Save_Tour()
            End If
            oTour = Nothing
            oDNS = Nothing
            oCombo = Nothing
        Else
            Save_Tour()
        End If
    End Sub

    Private Sub Save_Tour()
        If txtTourID.Text > 0 Then
            If CheckSecurity("Tours", "Edit", , , Session("UserDBID")) Then
                Update_Values()
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('You Do Not Have Permission To Edit A Tour Record.');", True)
            End If
        Else
            If CheckSecurity("Tours", "Add", , , Session("UserDBID")) Then
                Update_Values()
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('You Do Not Have Permission To Create A Tour Record.');", True)
            End If
        End If
    End Sub
    Protected Sub Accom_Link_Click(sender As Object, e As System.EventArgs) Handles Accom_Link.Click
        If txtTourID.Text > 0 Then

        End If
    End Sub

    Protected Sub Uploaded_Docs_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Uploaded_Docs_Link.Click
        If txtTourID.Text > 0 Then
            MultiView1.ActiveViewIndex = 9
            ucUploadedDocs.KeyField = "TourID"
            ucUploadedDocs.KeyValue = txtTourID.Text
            ucUploadedDocs.List()
        End If
    End Sub

    Protected Sub gvChargeBacks_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvChargeBacks.SelectedIndexChanged
        Dim row As GridViewRow = gvChargeBacks.SelectedRow
        Response.Redirect("../Payroll/EditChargeBackRequest.aspx?CBID=" & row.Cells(1).Text)
    End Sub

    Protected Sub Chargebacks_Link_Click(sender As Object, e As System.EventArgs) Handles Chargebacks_Link.Click
        If txtTourID.Text > 0 Then
            'Dim cb As New clsPayrollChargebackRequest
            'gvChargeBacks.DataSource = cb.List_CBs("TourID", txtTourID.Text)
            'gvChargeBacks.DataBind()
            'cb = Nothing
            MultiView1.ActiveViewIndex = 10
        End If
    End Sub

    Protected Sub lbLetterPrint_Click(sender As Object, e As System.EventArgs) Handles lbLetterPrint.Click
        With New clsTourLetters()
            Dim ds As SqlDataSource = .Get_Letters(Campaign1.Selected_ID, siLocation.Selected_ID)
            Dim dv As DataView = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)

            For Each dvr As DataRowView In dv
                ClientScript.RegisterClientScriptBlock(Me.GetType, DateTime.Now.ToShortDateString(), "modal.mwindow.open('" & _
                    Request.ApplicationPath & "/marketing/tourLetter.aspx?TourID=" & txtTourID.Text & _
                    "&letterid=" & dvr("letterid").ToString() & _
                    "&subject=" & dvr("subject").ToString().Trim().Replace("'", "\'") & _
                    "','win01','status=1,menubar=1,resizable=1,width=690,height=1400')", True)
            Next
        End With
    End Sub

    Protected Sub lbLetterEmail_Click(sender As Object, e As System.EventArgs) Handles lbLetterEmail.Click
        With New clsTourLetters()
            Dim ds As SqlDataSource = .Get_Letters(Campaign1.Selected_ID, siLocation.Selected_ID)
            Dim dv As DataView = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)

            For Each dvr As DataRowView In dv
                ClientScript.RegisterClientScriptBlock(Me.GetType, DateTime.Now.ToShortDateString(), "modal.mwindow.open('" & _
                    Request.ApplicationPath & "/marketing/tourLetter.aspx?TourID=" & txtTourID.Text & _
                    "&letterid=" & dvr("letterid").ToString() & _
                    "&subject=" & dvr("subject").ToString().Trim().Replace("'", "\'") & _
                    "&fromemail=" & dvr("emailaddress").ToString().Trim() & _
                    "&function=email','win01','status=1,menubar=1,resizable=1,width=690,height=1400')", True)
            Next
        End With
    End Sub

   
    Protected Sub lbSaveTour_Click(sender As Object, e As EventArgs) Handles lbSaveTour.Click
        Save_Tour()
    End Sub

    Protected Sub lbPrintTourSlip_Click(sender As Object, e As System.EventArgs) Handles lbPrintTourSlip.Click
        If CInt(Request("TourID")) > 0 Then
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand("sp_OPCosTourSlip", cn)

                    cm.CommandType = CommandType.StoredProcedure
                    cm.Parameters.AddWithValue("@tourId", SqlDbType.Int).Value = CInt(Request("TourID"))

                    Try
                        cn.Open()
                        Dim dt = New DataTable()
                        dt.Load(cm.ExecuteReader())


                        If dt.Rows.Count = 1 Then
                            Dim cr = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                            cr.Load(Server.MapPath("~/Reports/OPCOS_TourSlip.rpt"))                            
                            cr.SetDataSource(dt)

                            cr.Subreports(0).DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
                            cr.Subreports(0).DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
                            cr.Subreports(1).DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
                            cr.Subreports(1).DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
                            cr.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, True, "PrintOPC-OSTourSlip")
                        End If                    
                    Catch ex As Exception
                        Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
        End If
    End Sub
    Protected Sub Vendor_Link_Click(sender As Object, e As EventArgs) Handles Vendor_Link.Click
        If txtTourID.Text > 0 Then
            Dim vt As New clsVendorRep2Tour
            gvRep2Tour.DataSource = vt.List_Display(txtTourID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvRep2Tour.DataKeyNames = sKeys
            gvRep2Tour.DataBind()
            MultiView1.ActiveViewIndex = 11
            If gvRep2Tour.Rows.Count > 1 Then
                lbAddVT.Enabled = False
            End If
        End If
    End Sub
    Protected Sub lbAddVT_Click(sender As Object, e As EventArgs) Handles lbAddVT.Click
        Response.Write(gvRep2Tour.Rows.Count)
        If gvRep2Tour.Rows.Count > 0 Then
            '            ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "modal.mwindow.open('EditVendorRep2Tour.aspx?TourID=" & txtTourID.Text & "&ID=0','win01',350,350);", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "modal.mwindow.open('EditVendorRep2Tour.aspx?TourID=" & txtTourID.Text & "&ID=0','win01',350,350);", True)
        End If
    End Sub
    Protected Sub gvRep2Tour_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvRep2Tour.SelectedIndexChanged
        Dim row As GridViewRow = gvRep2Tour.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "modal.mwindow.open('EditVendorRep2Tour.aspx?TourID=" & txtTourID.Text & "&ID=" & row.Cells(1).Text & "','win01',350,350);", True)
    End Sub
End Class
