
Partial Class wizards_addOPCOSTour
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oPers As New clsPersonnel
            ddPersonnel.DataSource = oPers.Get_Personnel_By_Dept("OPC-OS")
            ddPersonnel.DataTextField = "Personnel"
            ddPersonnel.DataValueField = "PersonnelID"
            ddPersonnel.DataBind()
            siTourTime.Connection_String = Resources.Resource.cns
            siTourTime.ComboItem = "TourTime"
            siTourTime.Label_Caption = ""
            siTourTime.Load_Items()
            Dim oPrem As New clsPremium
            gvPremiums.DataSource = oPrem.List_Active
            Dim sKeys(0) As String
            sKeys(0) = "PremiumID"
            gvPremiums.DataKeyNames = sKeys
            gvPremiums.DataBind()
            oPrem = Nothing
        End If
    End Sub

    Protected Sub gvPremiums_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                Dim ddTest As DropDownList = e.Row.FindControl("ddQty")
                Dim i As Integer
                For i = 1 To 10
                    ddTest.Items.Add(i)
                Next
            End If
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim err As String = ""
        Dim bProceed As Boolean = True
        Dim tID As Integer = 0
        Dim oTour As New clsTour
        Dim oPremiumIssued As New clsPremiumIssued
        Dim oPremium As New clsPremium
        Dim oRes As New clsReservations
        Dim oCamp As New clsCampaign
        Dim oCombo As New clsComboItems
        Dim oPersTrans As New clsPersonnelTrans
        Dim oPers As New clsPersonnel
        For Each row As GridViewRow In gvPremiums.Rows
            Dim cb As CheckBox = row.FindControl("chkPremium")
            If cb.Checked Then
                Dim tb As TextBox = row.FindControl("txtAmt")
                If row.Cells(2).Text = "Check" And (tb.Text = "" Or Not (isNumeric(tb.Text))) Then
                    bProceed = False
                    err = "Please Enter a Valid Check Amount."
                    Exit For
                End If
            End If
        Next
        If bProceed Then
            oTour.UserID = Session("UserDBID")
            oTour.TourID = 0
            oTour.Load()
            If Request("ReservationID") = 0 Then
                oTour.ProspectID = Request("ProspectID")
            Else
                oRes.ReservationID = Request("ReservationID")
                oRes.Load()
                oTour.ProspectID = oRes.ProspectID
                oTour.ReservationID = Request("ReservationID")
            End If
            oTour.TourDate = dteDateBooked.Selected_Date
            oTour.CampaignID = oCamp.Lookup_ID("OPC-OS")
            oTour.BookingDate = System.DateTime.Now.ToShortDateString
            oTour.TypeID = oCombo.Lookup_ID("TourType", "OPC")
            oTour.StatusID = oCombo.Lookup_ID("TourStatus", "Booked")
            oTour.TourLocationID = oCombo.Lookup_ID("TourLocation", "KCP")
            oTour.TourTime = siTourTime.Selected_ID
            oTour.Save()
            tID = oTour.TourID
            oPersTrans.Personnel_Trans_ID = 0
            oPersTrans.UserID = Session("UserDBID")
            oPersTrans.Load()
            oPersTrans.PersonnelID = ddPersonnel.SelectedValue
            oPersTrans.TitleID = oCombo.Lookup_ID("PersonnelTitle", "OnSite Solicitor")
            oPersTrans.KeyField = "TourID"
            oPersTrans.KeyValue = tID
            oPersTrans.Created_By_ID = Session("UserDBID")
            oPersTrans.Date_Created = System.DateTime.Now.ToShortDateString
            oPersTrans.Save()
            '** Insert Karen Solderitch as Team Lead
            oPersTrans.Personnel_Trans_ID = 0
            oPersTrans.Load()
            oPersTrans.PersonnelID = oPers.Lookup_By_Name("Karen", "Alrihawi")
            oPersTrans.TitleID = oCombo.Lookup_ID("PersonnelTitle", "OnSite Team Lead")
            oPersTrans.KeyField = "TourID"
            oPersTrans.KeyValue = tID
            oPersTrans.Created_By_ID = Session("UserDBID")
            oPersTrans.Date_Created = System.DateTime.Now.ToShortDateString
            oPersTrans.Save()
            For Each row As GridViewRow In gvPremiums.Rows
                Dim cb As CheckBox = row.FindControl("chkPremium")
                If cb.Checked Then
                    oPremiumIssued.PremiumIssuedID = 0
                    oPremiumIssued.UserID = Session("UserDBID")
                    oPremiumIssued.Load()
                    oPremiumIssued.PremiumID = row.Cells(1).Text
                    Dim dd As DropDownList = row.FindControl("ddQty")
                    oPremiumIssued.QtyAssigned = dd.SelectedValue
                    oPremiumIssued.KeyField = "TourID"
                    oPremiumIssued.KeyValue = tID
                    oPremiumIssued.CreatedByID = Session("UserDBID")
                    oPremiumIssued.StatusID = oCombo.Lookup_ID("PremiumStatus", "Not Issued")
                    If row.Cells(2).Text = "Check" Then
                        Dim tb As TextBox = row.FindControl("txtAmt")
                        oPremiumIssued.CostEA = tb.Text
                        oPremiumIssued.TotalCost = CInt(tb.Text) * dd.SelectedValue
                    Else
                        oPremium.PremiumID = row.Cells(1).Text
                        oPremium.Load()
                        oPremiumIssued.CostEA = oPremium.Cost
                        oPremiumIssued.TotalCost = oPremium.Cost & dd.SelectedValue
                    End If
                    oPremiumIssued.Save()
                End If
            Next
            'ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('DONE');", True)

            'per work order 39235, the new tour ID should be shown in a popup
            '
            Dim msg As String = String.Format("Your new tour id is {0}.", tID)
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & msg & "')", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & err & "');", True)
        End If
        oTour = Nothing
        oPremiumIssued = Nothing
        oPremium = Nothing
        oRes = Nothing
        oCamp = Nothing
        oCombo = Nothing
        oPersTrans = Nothing
        oPers = Nothing
    End Sub
End Class
