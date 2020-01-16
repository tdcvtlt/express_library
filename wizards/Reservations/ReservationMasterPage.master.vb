Imports System.Web.Script.Serialization
Imports System.Data

Imports clsReservationWizard

Partial Class wizard_Reservations_ReservationMasterPage
    Inherits System.Web.UI.MasterPage

    Private wiz As New Wizard
    Private package_base As New Base_Package

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(Session("wizData" + Session("wizGuid")))
        If wiz.Scenario = EnumScenario.Ten Then tbSummary.Visible = False
    End Sub

    Public Function Navigate(curPage As String, dir As Int32) As String
        Dim pages As List(Of Page) = New List(Of Page)
        Dim nexPage = curPage.ToLower()
        Dim accom_name = String.Empty
        Dim defaultPackageID = 0

        wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(Session("wizData" + Session("wizGuid")))

        With wiz
            If wiz.Scenario = EnumScenario.One Then
                pages.Add(New clsReservationWizard.Page With {.Name = "Default.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Search.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "List.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditProspect.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditReservation.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Allocation.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditTour.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Payment.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Note.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Confirmation.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Finish.aspx".ToLower(), .Visible = True})

                If wiz.Packages.Count = wiz.Packages.Where(Function(x) package_base.Get_Package_Type(x.PackageID) = "Rental" _
                                                                Or package_base.Get_Package_Type(x.PackageID) = "Owner Getaway").Count Then
                    pages.Where(Function(x) x.Name = "EditTour.aspx".ToLower()).Single().Visible = False
                End If

            ElseIf wiz.Scenario = EnumScenario.Two Then

                pages.Add(New clsReservationWizard.Page With {.Name = "Default.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "List.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "ListReservations.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditProspect.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Search.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditReservation.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Allocation.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditAccommodation.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditTour.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Payment.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Note.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Confirmation.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Finish.aspx".ToLower(), .Visible = True})

                If wiz.Packages.Count = 1 Then
                    accom_name = package_base.Get_Accom_Name(wiz.Packages(0).PackageID)
                End If

                If accom_name.ToLower() = "kcp" Or String.IsNullOrEmpty(accom_name) Then
                    pages.Where(Function(x) x.Name = "EditAccommodation.aspx".ToLower()).Single().Visible = False
                Else
                    pages.Where(Function(x) x.Name = "Allocation.aspx".ToLower()).Single().Visible = False
                End If

            ElseIf wiz.Scenario = EnumScenario.Three Then
                pages.Add(New clsReservationWizard.Page With {.Name = "Default.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "List.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "ListReservations.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditProspect.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Search.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditReservation.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Allocation.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditAccommodation.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditTour.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Payment.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "RequestRefund.aspx".ToLower(), .Visible = False})
                pages.Add(New clsReservationWizard.Page With {.Name = "Note.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Confirmation.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Finish.aspx".ToLower(), .Visible = True})

                If wiz.Packages.Count = 1 Then
                    If wiz.Packages(0).Package IsNot Nothing Then
                        accom_name = package_base.Get_Accom_Name(wiz.Packages(0).Package.PackageID)
                    End If
                End If

                If accom_name.ToLower() = "kcp" Or String.IsNullOrEmpty(accom_name) Then
                    pages.Where(Function(x) x.Name = "EditAccommodation.aspx".ToLower()).Single().Visible = False
                Else
                    pages.Where(Function(x) x.Name = "Allocation.aspx".ToLower()).Single().Visible = False
                End If

                If wiz.Packages.Count = wiz.Packages.Where(Function(x) package_base.Get_Package_Type(x.PackageID) = "Rental" _
                                                               Or package_base.Get_Package_Type(x.PackageID) = "Owner Getaway").Count Then
                    pages.Where(Function(x) x.Name = "EditTour.aspx".ToLower()).Single().Visible = False
                End If

            ElseIf wiz.Scenario = EnumScenario.Four Then

                pages.Add(New clsReservationWizard.Page With {.Name = "Default.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "List.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "ListReservations.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditProspect.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditReservation.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Allocation.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditTour.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Payment.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "RequestRefund.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Note.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Confirmation.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Finish.aspx".ToLower(), .Visible = True})

            ElseIf wiz.Scenario = EnumScenario.Ten Then

                pages.Add(New clsReservationWizard.Page With {.Name = "Default.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "SearchSpecial.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "SpecialList.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditProspect.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditReservation.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "AllocationSpecial.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "EditTourSpecial.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Payment.aspx".ToLower(), .Visible = True})
                pages.Add(New clsReservationWizard.Page With {.Name = "Up4ForceConfirmation.aspx".ToLower(), .Visible = True})

            End If
        End With

        Dim Goto_NextPage As Func(Of String) =
            Function()
                Dim steps = (From p In pages Where p.Visible = True Select p).ToList()
                For i = 0 To steps.Count - 1
                    If i = 0 And (steps(0).Name = curPage) Then
                        If dir = 1 Then
                            nexPage = steps(1).Name
                            Exit For
                        End If
                    ElseIf i = steps.Count - 1 And (steps(steps.Count - 1).Name = curPage) Then
                        If dir = 0 Then
                            nexPage = steps(steps.Count - 2).Name
                            Exit For
                        End If
                    Else
                        If steps(i).Name = curPage Then
                            If dir = 1 Then
                                nexPage = steps(i + 1).Name
                                Exit For
                            ElseIf dir = 0 Then
                                nexPage = steps(i - 1).Name
                                Exit For
                            End If
                        End If
                    End If
                Next
                Return nexPage
            End Function

        Dim np = Goto_NextPage(), save_throws_exception = False
        If np.ToLower() = "Payment.aspx".ToLower() Then
            Try
                If wiz.Accom_Name <> "KCP" Then
                    wiz.Save_Hotel()
                Else
                    wiz.Save_All()
                End If

                Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)

            Catch ex As Exception
                LB_ERR_MASTERPAGE.Text = ex.Message
                save_throws_exception = True
                Session("LB_ERR_MASTERPAGE") = ex.Message
            End Try
        End If

        If save_throws_exception Then Return curPage
        If wiz.Scenario = EnumScenario.One Or wiz.Scenario = EnumScenario.Ten Then
            Return np
        ElseIf wiz.Scenario = EnumScenario.Two Then
            If np.ToLower() = "Payment.aspx".ToLower() And (New clsFinancials().Get_Balance("ReservationID", wiz.Reservation.ReservationID, 0)) = 0 Then
                pages.Where(Function(x) x.Name = "Payment.aspx".ToLower()).Single().Visible = False
                Return Goto_NextPage()
            Else
                Return np
            End If
        ElseIf wiz.Scenario = EnumScenario.Three Then
            If np.ToLower() = "Payment.aspx".ToLower() Then
                Dim amtDue = New clsFinancials().Get_Balance("ReservationID", wiz.Reservation.ReservationID, 0)
                If amtDue = 0 Then
                    'Skip to the Note page
                    pages.Where(Function(x) x.Name = "Payment.aspx".ToLower()).Single().Visible = False
                    Return Goto_NextPage()
                ElseIf amtDue < 0 Then
                    'Skip to Refund page
                    pages.Where(Function(x) x.Name = "Payment.aspx".ToLower()).Single().Visible = False
                    pages.Where(Function(x) x.Name = "RequestRefund.aspx".ToLower()).Single().Visible = True
                    Return Goto_NextPage()
                Else
                    Return np
                End If
            ElseIf np.ToLower() = "RequestRefund.aspx".ToLower() Then
                Dim amt_due = New clsFinancials().Get_Balance("ReservationID", wiz.Reservation.ReservationID, 0)
                If amt_due >= 0 Then
                    pages.Where(Function(x) x.Name = "RequestRefund.aspx".ToLower()).Single().Visible = True
                    Return Goto_NextPage()
                Else
                    Return np
                End If
            Else
                'Go to Payment page
                Return np
            End If
        Else
            Return np
        End If
    End Function


    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        wiz =New JavaScriptSerializer().Deserialize(Of Wizard)(Session("wizData" + Session("wizGuid")))
        If Request.Url.PathAndQuery.IndexOf("default0") > 0 Then
            tbSummary.Visible = False
        Else
            With wiz.Prospect
                If .Prospect_ID <> 0 Then
                    If .First_Name.Length > 0 And .Last_Name.Length > 0 Then
                        lbProspect.Text = String.Format("{0} {1} (<strong style='color:blue;'>{2}</strong>)",
                                                        .First_Name.Trim(), .Last_Name.Trim(), .Prospect_ID)
                    End If
                End If
            End With
            lbPackage.Text = String.Empty
            lbVendor.Text = String.Empty
            lbReservation.Text = String.Empty
            lbInvoice.Visible = False
            lbPackageCost.Visible = False

            If wiz.Packages.Count > 0 Then
                Dim package_id = wiz.Packages(0).PackageID
                If wiz.Scenario <> EnumScenario.One Then
                    If wiz.Packages.First().Package IsNot Nothing Then
                        package_id = wiz.Packages.First().Package.PackageID
                    End If
                End If

                Dim reservation_id = wiz.Reservation.ReservationID
                Dim checkin_date As DateTime? = Nothing
                Dim checkout_date As DateTime? = Nothing

                If wiz.Search_CheckIn_Date.HasValue Then
                    checkin_date = wiz.Search_CheckIn_Date
                    checkout_date = wiz.Search_CheckOut_date
                ElseIf wiz.Reservation.CheckInDate.Length > 0 Then
                    checkin_date = DateTime.Parse(wiz.Reservation.CheckInDate)
                    checkout_date = DateTime.Parse(wiz.Reservation.CheckOutDate)
                End If

                If checkin_date.HasValue Then
                    lbReservation.Text = String.Format("{0} - {1} <strong style='color:blue;'> ({2})</strong>",
                          DateTime.Parse(checkin_date).ToShortDateString(),
                          DateTime.Parse(checkout_date).ToShortDateString(),
                          reservation_id)

                    lbPackage.Text = package_base.Get_Package_Description(package_id)
                    lbVendor.Text = package_base.Get_Package_Vendor_By_VendorID(package_id)


                    lbInvoice.Text = String.Format("{0:C2}", wiz.Packages_Price)
                    lbInvoice.Visible = True
                    lbPackageCost.Visible = True
                End If
            End If

            lbInvoice.Text = String.Format("{0:C2}", wiz.Packages_Price)
            lbInvoice.Visible = True
            lbPackageCost.Visible = True

        End If


        If Session("wizInventories" + wiz.GUID_TIMESTAMP) <> Nothing Then
            wiz.Inventories_Available = New JavaScriptSerializer().Deserialize(Of List(Of List(Of Wizard_Room)))(Session("wizInventories" + wiz.GUID_TIMESTAMP))
        End If
        If Session("wizPackages_Ghost" + wiz.GUID_TIMESTAMP) <> Nothing Then
            wiz.Packages = New JavaScriptSerializer().Deserialize(Of List(Of Wizard_Package))(Session("wizPackages_Ghost" + wiz.GUID_TIMESTAMP))
        End If


        LB_WIZ_DATA.Text = New JavaScriptSerializer().Serialize(wiz)
        Session("wizData" + Session("wizGuid")) = Nothing
        Session("wizInventories" + wiz.GUID_TIMESTAMP) = Nothing
        Session("wizPackages_Ghost" + wiz.GUID_TIMESTAMP) = Nothing
    End Sub
End Class

