Imports Microsoft.VisualBasic
Imports System.Data
Partial Class PropertyManagement_BulkBankWizard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Session("bbt") = Nothing
            For i = (Year(System.DateTime.Now) - 3) To (Year(System.DateTime.Now) + 3)
                ddYear.Items.Add(i)
            Next
            For i = 7 To 365 Step 7
                ddNights.Items.Add(New ListItem(i, i / 7))
            Next
            ddExchCompany.Items.Add(New ListItem("II", "IIBULKBANK"))
            ddExchCompany.Items.Add(New ListItem("II POINTS", "IIPOINTSBULKBANK"))
            ddExchCompany.Items.Add(New ListItem("RCI", "RCIBULKBANK"))
            ddExchCompany.Items.Add(New ListItem("PLAN WITH TAN", "PLANWITHTAN"))
            ddExchCompany.Items.Add(New ListItem("KCP DEVELOPER", "KCP DEVELOPER"))
            ddExchCompany.Items.Add(New ListItem("KCP POOL - RENTAL", "KCP POOL"))
            siUnitType.Connection_String = Resources.Resource.cns
            siUnitType.ComboItem = "UnitType"
            siUnitType.Label_Caption = ""
            siUnitType.Load_Items()
            For i = 1 To 4
                If i = 1 Then
                    ddBedrooms.Items.Add(i)
                    ddBedRooms.Items.Add(New ListItem("1BD-UP", "1BD-UP"))
                    ddBedRooms.Items.Add(New ListItem("1BD-DWN", "1BD-DWN"))
                Else
                    ddBedRooms.Items.Add(i)
                End If
            Next
            siRoomSubType.Connection_String = Resources.Resource.cns
            siRoomSubType.ComboItem = "RoomSubType"
            siRoomSubType.Label_Caption = ""
            siRoomSubType.Load_Items()

            Dim dt As New DataTable
            dt.Columns.Add("Usages")
            dt.Columns.Add("Year")
            dt.Columns.Add("BD")
            dt.Columns.Add("UnitType")
            dt.Columns.Add("UnitTypeID")
            dt.Columns.Add("Start Date")
            dt.Columns.Add("End Date")
            dt.Columns.Add("Exhange Company")
            dt.Columns.Add("ContractNumber")
            dt.Columns.Add("CheckIn")
            dt.Columns.Add("RoomSubTypeID")

            Session("bbt") = dt
            gvBulkBank.DataSource = dt
            gvBulkBank.DataBind()
        Else
            gvBulkBank.DataSource = Session("bbt")
            gvBulkBank.DataBind()
        End If
    End Sub

    Protected Sub gvBulkBank_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        e.Row.Cells(5).Visible = False
        e.Row.Cells(9).Visible = False
        e.Row.Cells(11).Visible = False
    End Sub


    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""

        '******Error Checking *******'
        If txtNumUsages.Text = "" Or Not (IsNumeric(txtNumUsages.Text)) Then
            bProceed = False
            sErr += "Please Enter a Valid Number of Usages. \n"
        End If
        If dteStartDate.Selected_Date = "" Then
            bProceed = False
            sErr += "Please Enter a Valid Start Date. \n"
        End If
        If siUnitType.Selected_ID < 1 Then
            bProceed = False
            sErr += "Please Select a Unit Type. \n"
        End If
        If siRoomSubType.Selected_ID < 1 Then
            bProceed = False
            sErr += "Please Select a Check-In Day. \n"
        End If
        If siUnitType.SelectedName = "Cottage" And (ddBedrooms.SelectedValue = "4" Or ddBedrooms.SelectedValue = "1BD-DWN" Or ddBedrooms.SelectedValue = "1BD-UP") Then
            bProceed = False
            sErr += "Please Select a Valid Unit Type/Bedroom Combination. \n"
        ElseIf siUnitType.SelectedName = "Townes" And (ddBedrooms.SelectedValue <> "2" And ddBedrooms.SelectedValue <> "4") Then
            bProceed = False
            sErr += "Please Select a Valid Unit Type/Bedroom Combination. \n"
        ElseIf siUnitType.SelectedName = "Estates" And (ddBedRooms.SelectedValue = "1") Then
            bProceed = False
            sErr += "Please Select a Valid Unit Type/Bedroom Combination. \n"
        End If
        '****** End Error Checking *******'

        If bProceed Then
            Dim dt As New DataTable
            dt = Session("bbt")
            Dim dr As DataRow
            Dim sDate As Date = dteStartDate.Selected_Date
            Dim i As Integer = 0
            For i = 1 To ddNights.SelectedValue
                dr = dt.NewRow
                dr("Usages") = txtNumUsages.Text
                dr("Year") = ddYear.SelectedValue
                dr("BD") = ddBedRooms.SelectedValue
                dr("UnitType") = siUnitType.SelectedName
                dr("UnitTypeID") = siUnitType.Selected_ID
                dr("Start Date") = sDate.ToShortDateString
                dr("End Date") = sDate.AddDays(7).ToShortDateString
                dr("Exhange Company") = ddExchCompany.SelectedItem.Text
                dr("ContractNumber") = ddExchCompany.SelectedValue
                dr("CheckIn") = siRoomSubType.SelectedName
                dr("RoomSubTypeID") = siRoomSubType.Selected_ID
                dt.Rows.Add(dr)
                sDate = sDate.AddDays(7).ToShortDateString
            Next
            Session("bbt") = dt
            gvBulkBank.DataSource = dt
            gvBulkBank.DataBind()
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "alert", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub gvBulkBank_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvBulkBank.RowDeleting
        Dim dt As New DataTable
        Dim dRow As DataRow
        dt = Session("bbt")
        dRow = dt.Rows(e.RowIndex)
        dt.Rows.Remove(dRow)
        Session("bbt") = dt
        gvBulkBank.DataSource = Session("bbt")
        gvBulkBank.DataBind()
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        lblErr.Text = ""
        Dim bProceed As Boolean = True
        Dim oUsage As New clsUsage
        Dim oCombo As New clsComboItems
        Dim oContract As New clsContract
        Dim usageAID As Integer = 0
        Dim usageBID As Integer = 0
        Dim usageCID As Integer = 0
        Dim conID As Integer = 0
        Dim uSubTypeID As Integer = 0
        Dim uTypeID As Integer = 0
        Dim uStatusID As Integer = 0
        Dim badUsages As String = ""
        '****Error Checking

        '****End Error Check

        If bProceed Then
            oUsage.UserID = Session("UserDBID")
            uStatusID = oCombo.Lookup_ID("UsageStatus", "Used")
            For Each Row As GridViewRow In gvBulkBank.Rows

                conID = oContract.Get_Contract_ID(Row.Cells(9).Text)
                If Row.Cells(9).Text = "PLANWITHTAN" Or Row.Cells(9).Text = "KCP POOL" Then
                    uTypeID = oCombo.Lookup_ID("ReservationType", "Rental")
                ElseIf Row.Cells(9).Text = "KCP DEVELOPER" Then
                    uTypeID = oCombo.Lookup_ID("ReservationTYpe", "Marketing")
                ElseIf Row.Cells(9).Text = "IIPOINTSBULKBANK" Then
                    uTypeID = oCombo.Lookup_ID("ReservationType", "PointsExchange")
                ElseIf Row.Cells(9).Text = "IIBULKBANK" Then
                    uTypeID = oCombo.Lookup_ID("ReservationType", "Exchange")
                    uSubTypeID = oCombo.Lookup_ID("ReservationSubType", "II")
                Else
                    uTypeID = oCombo.Lookup_ID("ReservationType", "Exchange")
                    uSubTypeID = oCombo.Lookup_ID("ReservationSubType", "RCI")
                End If
                usageAID = 0
                usageBID = 0
                usageCID = 0
                For i = 1 To CInt(Row.Cells(1).Text)
                    If Left(Row.Cells(3).text, 1) = "1" Or Row.Cells(3).text = "2" Then
                        oUsage.UsageID = 0
                        oUsage.Load()
                        oUsage.UsageYear = Row.Cells(2).text
                        oUsage.InDate = Row.Cells(6).text
                        oUsage.OutDate = Row.Cells(7).text
                        oUsage.UnitTypeID = Row.Cells(5).text
                        oUsage.Days = 7
                        oUsage.ContractID = conID
                        oUsage.TypeID = uTypeID
                        oUsage.SubTypeID = uSubTypeID
                        oUsage.StatusID = uStatusID
                        oUsage.DateCreated = System.DateTime.Now
                        If Row.Cells(3).Text = "1" Then
                            oUsage.RoomTypeID = oCombo.Lookup_ID("RoomType", "1BD")
                        ElseIf Row.Cells(3).Text = "2" Then
                            oUsage.RoomTypeID = oCombo.Lookup_ID("RoomType", "2BD")
                        Else
                            oUsage.RoomTypeID = oCombo.Lookup_ID("RoomType", Row.Cells(3).Text)
                        End If
                        oUsage.Save()
                        usageAID = oUsage.UsageID
                    ElseIf Row.Cells(3).Text = "3" And (Row.Cells(4).Text = "Cottage" Or Row.Cells(4).Text = "Estates") Then
                        'Create 3BD Usage
                        oUsage.UsageID = 0
                        oUsage.Load()
                        oUsage.UsageYear = Row.Cells(2).Text
                        oUsage.InDate = Row.Cells(6).Text
                        oUsage.OutDate = Row.Cells(7).Text
                        oUsage.UnitTypeID = Row.Cells(5).Text
                        oUsage.Days = 7
                        oUsage.ContractID = conID
                        oUsage.TypeID = uTypeID
                        oUsage.SubTypeID = uSubTypeID
                        oUsage.StatusID = uStatusID
                        oUsage.DateCreated = System.DateTime.Now
                        oUsage.RoomTypeID = oCombo.Lookup_ID("RoomType", "3BD")
                        oUsage.Save()
                        usageAID = oUsage.UsageID

                        ''Create 1BD Usage
                        'oUsage.UsageID = 0
                        'oUsage.Load()
                        'oUsage.UsageYear = Row.Cells(2).text
                        'oUsage.InDate = Row.Cells(6).text
                        'oUsage.OutDate = Row.Cells(7).text
                        'oUsage.UnitTypeID = Row.Cells(5).text
                        'oUsage.Days = 7
                        'oUsage.ContractID = conID
                        'oUsage.TypeID = uTypeID
                        'oUsage.SubTypeID = uSubTypeID
                        'oUsage.StatusID = uStatusID
                        'oUsage.DateCreated = System.DateTime.Now
                        'oUsage.RoomTypeID = oCombo.Lookup_ID("RoomType", "1BD")
                        'oUsage.Save()
                        'usageBID = oUsage.UsageID

                    ElseIf Row.Cells(3).Text = "4" And Row.Cells(4).Text = "Townes" Then
                        'Create 4BD Usage
                        oUsage.UsageID = 0
                        oUsage.Load()
                        oUsage.UsageYear = Row.Cells(2).text
                        oUsage.InDate = Row.Cells(6).text
                        oUsage.OutDate = Row.Cells(7).text
                        oUsage.UnitTypeID = Row.Cells(5).text
                        oUsage.Days = 7
                        oUsage.ContractID = conID
                        oUsage.TypeID = uTypeID
                        oUsage.SubTypeID = uSubTypeID
                        oUsage.StatusID = uStatusID
                        oUsage.DateCreated = System.DateTime.Now
                        oUsage.RoomTypeID = oCombo.Lookup_ID("RoomType", "4BD")
                        oUsage.Save()
                        usageAID = oUsage.UsageID

                        ''Create 2BD Usage
                        'oUsage.UsageID = 0
                        'oUsage.Load()
                        'oUsage.UsageYear = Row.Cells(2).text
                        'oUsage.InDate = Row.Cells(6).text
                        'oUsage.OutDate = Row.Cells(7).text
                        'oUsage.UnitTypeID = Row.Cells(5).text
                        'oUsage.Days = 7
                        'oUsage.ContractID = conID
                        'oUsage.TypeID = uTypeID
                        'oUsage.SubTypeID = uSubTypeID
                        'oUsage.StatusID = uStatusID
                        'oUsage.DateCreated = System.DateTime.Now
                        'oUsage.RoomTypeID = oCombo.Lookup_ID("RoomType", "2BD")
                        'oUsage.Save()
                        'usageBID = oUsage.UsageID

                    ElseIf Row.Cells(3).Text = "4" And Row.Cells(4).Text = "Estates" Then
                        'Create 4BD Usage
                        oUsage.UsageID = 0
                        oUsage.Load()
                        oUsage.UsageYear = Row.Cells(2).text
                        oUsage.InDate = Row.Cells(6).text
                        oUsage.OutDate = Row.Cells(7).text
                        oUsage.UnitTypeID = Row.Cells(5).text
                        oUsage.Days = 7
                        oUsage.ContractID = conID
                        oUsage.TypeID = uTypeID
                        oUsage.SubTypeID = uSubTypeID
                        oUsage.StatusID = uStatusID
                        oUsage.DateCreated = System.DateTime.Now
                        oUsage.RoomTypeID = oCombo.Lookup_ID("RoomType", "4BD")
                        oUsage.DateCreated = System.DateTime.Now
                        oUsage.Save()
                        usageAID = oUsage.UsageID

                        ''Create 1BD-DWN Usage
                        'oUsage.UsageID = 0
                        'oUsage.Load()
                        'oUsage.UsageYear = Row.Cells(2).text
                        'oUsage.InDate = Row.Cells(6).text
                        'oUsage.OutDate = Row.Cells(7).text
                        'oUsage.UnitTypeID = Row.Cells(5).text
                        'oUsage.Days = 7
                        'oUsage.ContractID = conID
                        'oUsage.TypeID = uTypeID
                        'oUsage.SubTypeID = uSubTypeID
                        'oUsage.StatusID = uStatusID
                        'oUsage.DateCreated = System.DateTime.Now
                        'oUsage.RoomTypeID = oCombo.Lookup_ID("RoomType", "1BD-DWN")
                        'oUsage.Save()
                        'usageBID = oUsage.UsageID

                        ''Create 1BD-DWN Usage
                        'oUsage.UsageID = 0
                        'oUsage.Load()
                        'oUsage.UsageYear = Row.Cells(2).text
                        'oUsage.InDate = Row.Cells(6).text
                        'oUsage.OutDate = Row.Cells(7).text
                        'oUsage.UnitTypeID = Row.Cells(5).text
                        'oUsage.Days = 7
                        'oUsage.ContractID = conID
                        'oUsage.TypeID = uTypeID
                        'oUsage.SubTypeID = uSubTypeID
                        'oUsage.StatusID = uStatusID
                        'oUsage.DateCreated = System.DateTime.Now
                        'oUsage.RoomTypeID = oCombo.Lookup_ID("RoomType", "1BD-UP")
                        'oUsage.Save()
                        'usageCID = oUsage.UsageID
                    End If

                    If Not (oUsage.Add_BulkBank_Room(usageAID, usageBID, usageCID, CDate(Row.Cells(6).Text), CDate(Row.Cells(7).Text).AddDays(-1), Row.Cells(3).Text, Row.Cells(4).Text, Row.Cells(5).Text, uTypeID, Row.Cells(11).Text)) Then
                        If oUsage.Err <> "" Then
                            lblErr.Text = oUsage.Err
                            Exit Sub
                        Else
                            'If usageBID = 0 Then
                            If badUsages = "" Then
                                badUsages = usageAID
                            Else
                                badUsages = badUsages & ", " & usageAID
                            End If
                            'ElseIf usageCID = 0 Then
                            'If badUsages = "" Then
                            'badUsages = usageAID & ", " & usageBID
                            'Else
                            'badUsages = badUsages & ", " & usageAID & ", " & usageBID
                            'End If
                            'Else
                            'If badUsages = "" Then
                            'badUsages = usageAID & ", " & usageBID & ", " & usageCID
                            'Else
                            'badUsages = badUsages & ", " & usageAID & ", " & usageBID & ", " & usageCID
                            'End If
                            'End If
                        End If
                    End If
                Next
            Next
            If badUsages <> "" Then
                badUsages = "Rooms Were Not Added to the Following Usages: <br />" & badUsages
            Else
                badUsages = "Bulk Bank Complete. All Rooms Added."
            End If
            lblErr.Text = badUsages
        End If
        btnSubmit.Enabled = True
    End Sub

End Class
