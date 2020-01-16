Imports System.Data
Partial Class wizards_ExitTourWizard
    Inherits System.Web.UI.Page
    Dim eTourID As Integer = 0


    Protected Sub exitTourChk_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles exitTourChk.SelectedIndexChanged
        If exitTourChk.SelectedIndex = 0 Then
            ddExitReps.visible = True
            lblExitReps.visible = True
        Else
            ddExitReps.visible = False
            lblExitReps.visible = False
        End If
    End Sub

    Protected Sub purchaseChk_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles purchaseChk.SelectedIndexChanged
        If purchaseChk.SelectedIndex = 0 Then
            purchaselbl.visible = True
            contractTxt.Visible = True
            'purchaseChk.SelectedIndex = 0
        Else
            purchaselbl.visible = False
            contractTxt.visible = False
            'purchaseChk.SelectedIndex = 1
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If CheckSecurity("Tours", "Edit", , , Session("UserDBID")) Then
                Dim oTour As New clsTour
                lbltStatus.Text = oTour.get_Tour_Status(Request("TourID"))
                Load_Lookups()
                Get_Premiums()
                MultiView1.ActiveViewIndex = 0
            Else
                MultiView1.ActiveViewIndex = 2
            End If
        End If
    End Sub


    Protected Sub Get_Premiums()
        Dim oPrem As New clsPremiumIssued
        gvPremiums.DataSource = oPrem.Get_Prepared_Premiums(Request("TourID"))
        gvPremiums.DataBind()
        oPrem = Nothing
    End Sub
    Protected Sub Load_Lookups()
        siTourStatus.Connection_String = Resources.Resource.cns
        siTourStatus.Label_Caption = ""
        siTourStatus.ComboItem = "TourStatus"
        siTourStatus.Load_Items()
        Dim oPers As New clsPersonnel
        ddTO.Items.Add(New ListItem("No TO", 0))
        ddTO.AppendDataBoundItems = True
        ddTO.DataSource = oPers.List_TOs()
        ddTO.DataTextField = "Personnel"
        ddTO.DataValueField = "PersonnelID"
        ddTO.DataBind()
        ddExitReps.DataSource = oPers.List_Exit_Reps()
        ddExitReps.DataTextField = "Personnel"
        ddExitReps.DataValueField = "PersonnelID"
        ddExitReps.DataBind()
        lblErr.Text = oPers.Err
        oPers = Nothing
    End Sub

    Protected Sub gvPremiums_OnRowCreated(ByVal sender As Object, ByVal e As Web.UI.WebControls.GridViewRowEventArgs) Handles gvPremiums.RowCreated

        'Those columns you don't want to display you config here, 

        'you could use a for statement if you have many 
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheckOut.Click
        Dim valPage As String
        valPage = Validate_Page()
        If valPage <> "" Then
            'btnCheckOut.Enabled = True
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & valPage & "');ctl00_ContentPlaceHolder1_btnCheckOut.disabled=false;", True)
        Else
            Update_TourStatus()
            If ddTO.SelectedValue > 0 Then
                Assign_TO()
            End If
            If exitTourChk.SelectedIndex = 0 Then
                Create_Exit_Tour()
                Assign_Exit_Rep()
                If purchaseChk.SelectedIndex = 0 Then
                    Dim oCon As New clsContract
                    oCon.ContractID = oCon.Get_Contract_ID(contractTxt.Text)
                    oCon.Load()
                    oCon.TourID = eTourID
                    oCon.Save()
                    oCon = Nothing
                End If
            End If
            Issue_Premiums()
            'Remove VC tour part per work order 40153 10/28/13
            'If vClubChk.SelectedIndex = 0 Then
            '    'hfTourID.Value = eTourID
            '    Dim dt As New DataTable
            '    dt.Columns.Add("PremiumID")
            '    dt.Columns.Add("PremiumName")
            '    dt.Columns.Add("Qty")
            '    dt.Columns.Add("Amount")

            '    Session("vcp") = dt
            '    gvVCPremiums.DataSource = Session("vcp")
            '    gvVCPremiums.DataBind()

            '    Dim oPrem As New clsPremium
            '    ddPremiums.DataSource = oPrem.List_Active()
            '    ddPremiums.DataTextField = "PremiumName"
            '    ddPremiums.DataValueField = "PremiumID"
            '    ddPremiums.DataBind()
            '    oPrem = Nothing

            '    Dim oPers As New clsPersonnel
            '    ddExit.DataSource = oPers.List_Exit_Reps()
            '    ddExit.DataTextField = "Personnel"
            '    ddExit.DataValueField = "PersonnelID"
            '    ddExit.DataBind()
            '    oPers = Nothing

            '    For i = 1 To 50
            '        ddQty.Items.Add(i)
            '    Next
            '    siTourTime.Connection_String = Resources.Resource.cns
            '    siTourTime.ComboItem = "TourTime"
            '    siTourTime.Label_Caption = ""
            '    siTourTime.Load_Items()
            '    MultiView1.ActiveViewIndex = 1
            '    lblErr2.Text = "TourID: " & hfTourID.Value
            'Else
            Response.Redirect("ExitTours.aspx")
            'End If
        End If
        'lblErr.Text = "HI"
    End Sub

    Private Function Validate_Page() As String
        Dim eMsg As String = ""
        If siTourStatus.Selected_ID = 0 Then
            eMsg = "Please Select A Tour Status For the Current Tour."
        End If
        If eMsg = "" Then
            If exitTourChk.SelectedIndex < 0 Then
                eMsg = "Please Make Sure To Answer All Questions."
            End If
        End If
        If eMsg = "" Then
            If purchaseChk.SelectedIndex < 0 Then
                eMsg = "Please Make Sure to Answer All Questions."
            ElseIf purchaseChk.SelectedIndex = 0 Then
                If contractTxt.Text = "" Then
                    eMsg = "Please Fill In The Contract Number."
                Else
                    Dim oCon As New clsContract
                    If oCon.Verify_Contract(contractTxt.Text) = False Then
                        eMsg = "Please Enter a Valid Contract Number."
                    End If
                    oCon = Nothing
                End If
            End If
        End If
        'Remove Vacation Club Check Per Work Order 40153 10/28/13
        'If eMsg <> "" Then
        '    If vClubChk.SelectedIndex < 0 Then
        '        eMsg = "Please Make Sure to Answer All Questions."
        '    ElseIf vClubChk.SelectedIndex = 0 Then
        '        If purchaseChk.SelectedIndex = 0 Then
        '            eMsg = "Customer Can Not Tour With Vaction Club If They Purchased."
        '        ElseIf exitTourChk.SelectedIndex = 1 Then
        '            eMsg = "Customer Must Tour With Exit Before Touring With Vacation Club."
        '        End If
        '    End If
        'End If
        Return eMsg
    End Function

    Private Sub Update_TourStatus()
        Dim oTour As New clsTour
        oTour.TourID = Request("TourID")
        oTour.Load()
        oTour.UserID = Session("UserDBID")
        oTour.CheckedIn = False
        oTour.StatusID = siTourStatus.Selected_ID
        oTour.Save()
        oTour = Nothing
    End Sub

    Private Sub Assign_TO()
        Dim oPers As New clsPersonnelTrans
        Dim oCombo As New clsComboItems
        oPers.UserID = Session("UserDBID")
        oPers.KeyField = "TourID"
        oPers.KeyValue = Request("TourID")
        oPers.TitleID = oCombo.Lookup_ID("PersonnelTitle", "TO")
        oPers.PersonnelID = ddTO.SelectedValue ' ddTO.SelectedIndex
        oPers.Created_By_ID = Session("UserDBID")
        oPers.Save()
        oPers = Nothing
        oCombo = Nothing
    End Sub

    Private Sub Create_Exit_Tour()
        Dim oTour As New clsTour
        Dim oeTour As New clsTour
        Dim oCombo As New clsComboItems
        oTour.TourID = Request("TourID")
        oTour.Load()
        oeTour.UserID = Session("UserDBID")
        oeTour.TourID = 0
        oeTour.Load()
        oeTour.CampaignID = oTour.CampaignID
        oeTour.TourTime = oTour.TourTime
        oeTour.ProspectID = oTour.ProspectID
        oeTour.TourLocationID = oTour.TourLocationID
        oeTour.SubTypeID = oCombo.Lookup_ID("TourSubType", "Exit")
        oeTour.StatusID = oCombo.Lookup_ID("TourStatus", "Showed")
        oeTour.TourDate = System.DateTime.Now.ToShortDateString
        oeTour.Save()
        eTourID = oeTour.TourID
        hfTourID.Value = eTourID
        oTour = Nothing
        oeTour = Nothing
        oCombo = Nothing
    End Sub

    Private Sub Assign_Exit_Rep()
        Dim oPers As New clsPersonnelTrans
        Dim oCombo As New clsComboItems
        oPers.UserID = Session("UserDBID")
        oPers.KeyField = "TourID"
        oPers.KeyValue = eTourID
        oPers.TitleID = oCombo.Lookup_ID("PersonnelTitle", "Exit Sales Executive")
        oPers.PersonnelID = ddExitReps.SelectedValue
        oPers.Created_By_ID = Session("UserDBID")
        oPers.Save()
        oPers = Nothing
        oCombo = Nothing
    End Sub

    Private Function Issue_Premiums()
        For Each row As GridViewRow In gvPremiums.Rows
            Dim cb As CheckBox = row.FindControl("PremSelect")
            If cb.Checked Then
                Dim oPremIss As New clsPremiumIssued
                Dim oCombo As New clsComboItems
                Dim oPrem As New clsPremium
                oPremIss.UserID = Session("UserDBID")
                oPremIss.PremiumIssuedID = row.Cells(1).Text
                oPremIss.Load()
                oPremIss.StatusID = oCombo.Lookup_ID("PremiumStatus", "Issued")
                oPremIss.DateIssued = System.DateTime.Now.ToShortDateString
                oPremIss.IssuedByID = Session("UserDBID")
                oPremIss.QtyIssued = oPremIss.QtyAssigned
                oPremIss.TotalCost = oPremIss.QtyAssigned * oPremIss.CostEA
                oPrem.PremiumID = oPremIss.PremiumID
                oPrem.UserID = Session("UserDBID")
                oPrem.Load()
                oPrem.QtyOnHand = oPrem.QtyOnHand - oPremIss.QtyIssued
                oPrem.Save()
                oPremIss.Save()
                oPremIss = Nothing
                oCombo = Nothing
            End If
        Next
    End Function

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        If ddPremiums.SelectedItem.Text = "Check" Then
            If txtAmount.Text = "" Or Not (IsNumeric(txtAmount.Text)) Then
                bProceed = False
                sErr = "Please Enter A Check Amount."
            End If
        End If

        If bProceed Then
            Dim dt As New DataTable
            Dim dr As DataRow
            dt = Session("vcp")
            dr = dt.NewRow
            dr("PremiumID") = ddPremiums.SelectedValue
            dr("PremiumName") = ddPremiums.SelectedItem.Text
            dr("Qty") = ddQty.SelectedValue
            If ddPremiums.SelectedItem.Text = "Check" Then
                dr("Amount") = txtAmount.Text
            Else
                dr("Amount") = ""
            End If
            dt.Rows.Add(dr)
            Session("vcp") = dt
            gvVCPremiums.DataSource = Session("vcp")
            gvVCPremiums.DataBind()
        Else
            lblErr2.Text = sErr
        End If
    End Sub

    Protected Sub gvVCPremiums_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(2).Text <> "" Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub gvVCPremiums_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvVCPremiums.RowDeleting
        Dim dt As New DataTable
        Dim dRow As DataRow
        dt = Session("vcp")
        dRow = dt.Rows(e.RowIndex)
        dt.Rows.Remove(dRow)
        Session("vcp") = dt
        gvVCPremiums.DataSource = Session("vcp")
        gvVCPremiums.DataBind()
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        If dteTourDate.Selected_Date = "" Then
            bProceed = False
            sErr += "Please Select A Tour Date. \n"
        End If
        If siTourTime.Selected_ID < 1 Then
            bProceed = False
            sErr += "Please Select A Tour Time. \n"
        End If

        If bProceed Then
            Dim oTourA As New clsTour
            Dim oTourB As New clsTour
            Dim oCombo As New clsComboItems
            Dim tID As Integer = 0
            oTourA.TourID = hfTourID.Value
            oTourA.Load()
            oTourB.TourID = 0
            oTourB.Load()
            oTourB.UserID = Session("UserDBID")
            oTourB.ProspectID = oTourA.ProspectID
            oTourB.TourDate = dteTourDate.Selected_Date
            oTourB.TourTime = siTourTime.Selected_ID
            oTourB.CampaignID = oTourA.CampaignID
            oTourB.SourceID = oTourA.SourceID
            oTourB.TypeID = oTourA.TypeID
            oTourB.StatusID = oCombo.Lookup_ID("TourStatus", "Booked")
            oTourB.TourLocationID = oCombo.Lookup_ID("TourLocation", "VacationClub")
            oTourB.Save()
            tID = oTourB.TourID
            oTourA = Nothing
            oTourB = Nothing

            Dim oPersTrans As New clsPersonnelTrans
            oPersTrans.Personnel_Trans_ID = 0
            oPersTrans.Load()
            oPersTrans.UserID = Session("UserDBID")
            oPersTrans.PersonnelID = ddExit.SelectedValue
            oPersTrans.KeyField = "TourID"
            oPersTrans.KeyValue = tID
            oPersTrans.Fixed_Amount = 50
            oPersTrans.Date_Created = System.DateTime.Now.ToShortDateString
            oPersTrans.TitleID = oCombo.Lookup_ID("PersonnelTitle", "Solicitor")
            oPersTrans.Created_By_ID = Session("UserDBID")
            oPersTrans.Save()

            Dim oPremIssued As New clsPremiumIssued
            Dim oPremium As New clsPremium
            For Each row As GridViewRow In gvVCPremiums.Rows
                oPremIssued.UserID = Session("UserDBID")
                oPremIssued.PremiumIssuedID = 0
                oPremIssued.Load()
                oPremIssued.PremiumID = row.Cells(1).Text
                oPremIssued.KeyField = "TourID"
                oPremIssued.KeyValue = tID
                oPremIssued.DateCreated = System.DateTime.Now
                oPremIssued.CreatedByID = Session("UserDBID")
                oPremIssued.StatusID = oCombo.Lookup_ID("PremiumStatus", "Prepared")
                If row.Cells(2).Text = "Check" Then
                    oPremIssued.CostEA = row.Cells(4).Text
                    oPremIssued.TotalCost = CInt(row.Cells(3).Text) * CInt(row.Cells(4).Text)
                Else
                    oPersTrans.UserID = Session("UserDBID")
                    oPremium.PremiumID = row.Cells(1).Text
                    oPremium.Load()
                    oPremIssued.CostEA = oPremium.Cost
                    oPremIssued.TotalCost = CInt(row.Cells(3).Text) * oPremium.Cost
                End If
                oPremIssued.QtyAssigned = row.Cells(3).Text
                oPremIssued.Save()
            Next

            oPremium = Nothing
            oPremIssued = Nothing
            oPersTrans = Nothing
            oTourA = Nothing
            oTourB = Nothing
            Response.Redirect("ExitTours.aspx")
        Else
            lblErr2.Text = sErr
        End If
    End Sub
End Class
