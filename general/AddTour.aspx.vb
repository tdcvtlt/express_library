
Partial Class general_AddTour
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            MultiView1.ActiveViewIndex = 0
            Dim oPrem As New clsPremium
            cblPremiums.DataSource = oPrem.List_Active
            cblPremiums.DataValueField = "PremiumID"
            cblPremiums.DataTextField = "PremiumName"
            cblPremiums.RepeatColumns = 3
            cblPremiums.DataBind()
            oPrem = Nothing
            Dim oPers As New clsPersonnel
            ddBookedBy.DataSource = oPers.List_Reps_For_Department("OPC-OS")
            ddBookedBy.DataTextField = "Name"
            ddBookedBy.DataValueField = "PersonnelID"
            ddBookedBy.DataBind()
            oPers = Nothing
            siTourTime.Connection_String = Resources.Resource.cns
            siTourTime.ComboItem = "TourTime"
            siTourTime.Label_Caption = ""
            siTourTime.Load_Items()
        End If
    End Sub

    Protected Sub btnAssign_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAssign.Click
        If Save_Tour() Then
            MultiView1.ActiveViewIndex = 1
            Dim oPI As New clsPremiumIssued
            Repeater1.DataSource = oPI.Get_Assigned_Premiums(hfTourID.Value)
            Repeater1.DataBind()
            oPI = Nothing
        End If
    End Sub

    Private Function Save_Tour() As Boolean
        Dim oCI As New clsComboItems
        Dim oTour As New clsTour
        Dim oRes As New clsReservations
        Dim oPT As New clsPersonnelTrans
        Dim oPI As New clsPremiumIssued
        Dim oCamp As New clsCampaign
        Dim oPer As New clsPersonnel


        Dim campid As Integer = 0
        Dim tTypeid As Integer = 0
        Dim TitleID As Integer = 0
        Dim bookedID As Integer = 0
        Dim pstatusid As Integer = 0
        Dim tLocID As Integer = 0
        Dim prosid As Integer = 0
        Dim tourid As Integer = 0
        Try

            'Get CampaignID
            campid = oCamp.Lookup_ID("OPC-OS")

            'Get Tour Type
            tTypeid = oCI.Lookup_ID("TourType", "OPC")

            'Get TitleID 
            TitleID = oCI.Lookup_ID("PersonnelTitle", "OnSite Solicitor")

            'Get Booked Status
            bookedID = oCI.Lookup_ID("TourStatus", "Booked")

            'Get Premium Not Issued Status
            pstatusid = oCI.Lookup_ID("Premiumstatus", "Not Issued")

            'Get KCP Tour LocationID
            tLocID = oCI.Lookup_ID("TourLocation", "KCP")

            'Get ProspectID
            If Request("reservationid") = "0" Or Request("reservationid") = "" Then
                prosid = 0 'IIf(IsNumeric(Request("prospectid")), Request("prospectid"), 0)
            Else
                oRes.ReservationID = Request("ReservationID")
                oRes.Load()
                prosid = oRes.ProspectID
            End If
            'Insert Tour
            oTour.UserID = Session("UserDBID")
            oTour.ProspectID = prosid
            oTour.StatusID = bookedID
            oTour.TourLocationID = tLocID
            oTour.CampaignID = campid
            oTour.TypeID = tTypeid
            oTour.TourDate = dfTourDate.Selected_Date
            oTour.TourTime = siTourTime.Selected_ID
            oTour.BookingDate = Date.Today.ToShortDateString
            oTour.Save()

            tourid = oTour.TourID


            If Not (Request("reservationid") = "" Or Request("reservationid") = "0") Then
                'Update ReservationID to link Tour
                oRes.ReservationID = Request("ReservationID")
                oRes.Load()
                oRes.TourID = oTour.TourID
            End If

            'Insert Personnel Trans
            oPT.KeyField = "TourID"
            oPT.KeyValue = tourid
            oPT.PersonnelID = ddBookedBy.SelectedValue
            oPT.TitleID = TitleID
            oPT.Date_Created = Date.Now.ToString
            oPT.Created_By_ID = Session("UserDBID")
            oPT.Save()

            'Insert Karen Solderitch as OnSite Team Leader
            Dim persID As Integer = oPer.Lookup_By_Name("Karen", "Solderitch")
            Dim leadID As Integer = oCI.Lookup_ID("PersonnelTitle", "OnSite Team Lead")
            oPT.Personnel_Trans_ID = 0
            oPT.Load()
            oPT.KeyField = "TourID"
            oPT.KeyValue = tourid
            oPT.PersonnelID = persID
            oPT.TitleID = leadID
            oPT.Date_Created = Date.Now.ToString
            oPT.Created_By_ID = Session("UserDBID")
            oPT.Save()


            'Insert Premiums
            For i = 0 To cblPremiums.Items.Count - 1
                If cblPremiums.Items(i).Selected Then
                    oPI.PremiumIssuedID = 0
                    oPI.Load()
                    oPI.KeyField = "TourID"
                    oPI.KeyValue = tourid
                    oPI.StatusID = pstatusid
                    oPI.PremiumID = cblPremiums.Items(i).Value
                    oPI.CreatedByID = Session("UserDBID")
                    oPI.Save()
                End If
            Next

            hfTourID.Value = oTour.TourID
            Return True
        Catch ex As Exception
            lblErr.Text = ex.ToString
            Return False
        End Try

        oCI = Nothing
        oTour = Nothing
        oRes = Nothing
        oPT = Nothing
        oPI = Nothing
        oCamp = Nothing
        oPer = Nothing
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        For Each item As RepeaterItem In Repeater1.Items
            item.FindControl("PIID")

        Next
    End Sub

    Protected Sub Repeater1_DataBound(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Dim dd As DropDownList = e.Item.FindControl("ddQTY")
        Dim hf As HiddenField = e.Item.FindControl("PIID")
        Dim v As System.Data.DataRowView = e.Item.DataItem

        
        dd.Items.Clear()
        For i As Integer = 1 To 10
            Dim lItem As New ListItem(i, i)
            dd.Items.Add(lItem)
        Next
    End Sub

    Protected Sub Repeater1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles Repeater1.ItemCommand

    End Sub
End Class
