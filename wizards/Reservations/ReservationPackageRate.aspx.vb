
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services
Imports System.Web.Services

Partial Class ReservationPackageRate
    Inherits System.Web.UI.Page

    Private cx As String = "data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;"
    Private Sub ReservationPackageRate_Load(sender As Object, e As EventArgs) Handles Me.Load

        If IsPostBack = False Then
            Using cn = New SqlConnection(cx)
                Using cm = New SqlCommand("select distinct TypeID, (select ComboItem from t_ComboItems where ComboItemID = p.TypeID) Type from t_Package p where p.TypeID > 0 order by Type", cn)
                    Try
                        cn.Open()
                        rblPackageTypeID.DataSource = cm.ExecuteReader()
                        rblPackageTypeID.DataTextField = "Type"
                        rblPackageTypeID.DataValueField = "TypeID"
                        rblPackageTypeID.DataBind()
                    Catch ex As Exception
                        cn.Close()
                    End Try
                End Using
            End Using
        End If
    End Sub


    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True)>
    Public Shared Function AjaxListPackagesByTypeID(typeID As String, type As String) As String
        Dim l = New List(Of AjaxPackage)
        Using cn = New SqlConnection("data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
            Dim sql = String.Format(
                "select p.PackageID, p.Description [Package]  from t_Package p inner join t_PackageReservation pr on p.PackageID = pr.PackageID " _
                & "inner join t_Accom acc on acc.AccomID = p.AccomID " _
                & "inner join t_PackageReservationFinTransCode prftc on prftc.PackageID = p.PackageID " _
                & "where p.Active = 1 and p.TypeID in ({0}) and acc.AccomID in (72) and len(p.Description) > 0 order by p.Package", typeID)

            Using cm = New SqlCommand(sql, cn)
                Try
                    cn.Open()
                    Dim dt = New DataTable
                    dt.Load(cm.ExecuteReader())
                    For Each dr As DataRow In dt.Rows
                        l.Add(New AjaxPackage With {.ID = dr("PackageID").ToString(), .Package = dr("Package").ToString()})
                    Next
                Catch ex As Exception
                    cn.Close()
                End Try
            End Using
        End Using
        Return New JavaScriptSerializer().Serialize(l.ToArray())
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True)>
    Public Shared Function AjaxGetPackageDetail(packageID As String) As String
        Dim d = New AjaxPackageDetail()
        Using cn = New SqlConnection("data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
            Dim sql = String.Format(
                "select pr.PackageReservationID, (select ComboItem from t_ComboItems where ComboItemID in (p.UnitTypeID)) [UnitType], p.Bedrooms, p.MinNights, " _
                & "(select top 1 v.Vendor from t_Vendor v inner join t_Vendor2Package v2p on v.VendorID = v2p.VendorID where v2p.PackageID = p.PackageID) [Vendor], " _
                & "CONVERT(varchar(10), p.StartDate, 101) StartDate, CONVERT(varchar(10), p.EndDate, 101) EndDate, (select isnull((Select count(*) from t_PackageReservation2CheckInDay where PackageReservationID= pr.PackageReservationID group by PackageReservationID), 0)) [AllowExtraNight], " _
                & "(select ComboItem from t_ComboItems where ComboItemID in (pr.SourceID))  [Source], " _
                & "(select ComboItem from t_ComboItems where ComboItemID in (pr.TypeID))  [ReservationType], pr.PromoNights, pr.PromoRate, coalesce(p.DefaultInvoiceAmt, 0) DefaultInvoiceAmt " _
                & "from t_Package p inner join t_PackageReservation pr on p.PackageID = pr.PackageID " _
                & "inner join t_Accom acc on acc.AccomID = p.AccomID " _
                & "left join t_PackageReservationFinTransCode prftc on prftc.PackageID = p.PackageID " _
                & "left join t_PackageReservationPayment prp on prp.PackageReservationFinTransID = prftc.PackageReservationFinTransCodeID " _
                & "where p.Active = 1 and p.PackageID in ({0}) and acc.AccomID in (72) and len(p.Package) > 0 order by p.Package", packageID)

            Using cm = New SqlCommand(sql, cn)
                Try
                    cn.Open()
                    Dim dt = New DataTable
                    dt.Load(cm.ExecuteReader())
                    For Each dr As DataRow In dt.Rows
                        With d
                            .ID = packageID
                            .PackageReservationID = dr("PackageReservationID").ToString()
                            .UnitType = dr("UnitType").ToString()
                            .BedRooms = dr("Bedrooms").ToString()
                            .MinNights = dr("MinNights").ToString()
                            .Vendor = dr("Vendor").ToString()
                            .StartDate = dr("StartDate").ToString()
                            .EndDate = dr("EndDate").ToString()
                            .AllowExtraNight = IIf(Int32.Parse(dr("AllowExtraNight").ToString()) = 0, False, True)
                            .Source = dr("Source").ToString()
                            .ReservationType = dr("ReservationType").ToString()
                            .PromoNights = dr("PromoNights").ToString()
                            .PromoRate = dr("PromoRate").ToString()
                            .DefaultInvoiceAmt = dr("DefaultInvoiceAmt").ToString()
                        End With
                    Next

                    sql = String.Format(
                        "select pt.TourLocationID , (select ComboItem from t_ComboItems where ComboItemID = pt.TourLocationID) [TourLocation], " _
                        & "pt.CampaignID, (select Name from t_Campaign where campaignid = pt.CampaignID) [Campaign], " _
                        & "(select ComboItem from t_ComboItems where ComboItemID = pt.TourSourceID) [TourSource], " _
                        & "(select ComboItem from t_ComboItems where ComboItemID = pt.TourTypeID) [TourType] " _
                        & "from t_PackageReservation pr inner join t_PackageTour pt on pr.PackageReservationID = pt.PackageReservationID " _
                        & "where pr.PackageID = {0} and pt.PackageReservationID = {1} and pt.TourLocationID is not null and pt.CampaignID is not null and pt.TourSourceID is not null", d.ID, d.PackageReservationID)

                    cm.CommandText = sql
                    dt.Dispose()
                    dt = New DataTable()
                    dt.Load(cm.ExecuteReader())

                    If dt.Rows.Count = 1 Then

                        Dim dr = dt.Rows(0)
                        With d
                            d.Campaign = dr("Campaign").ToString()
                            d.CampaignID = dr("CampaignID").ToString()
                            d.TourLocation = dr("TourLocation").ToString()
                            d.TourLocationID = dr("TourLocationID").ToString()
                        End With
                    End If


                Catch ex As Exception
                    cn.Close()
                End Try
            End Using
        End Using
        Return New JavaScriptSerializer().Serialize(d)
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True)>
    Public Shared Function CheckToursAvailability(dateCheckIn As String, nights As Int16, campaignID As Int32, tourLocationID As Int32) As String

        Dim ta As New List(Of AjaxTourAvailability)
        dateCheckIn = DateTime.Parse(dateCheckIn.Replace("|", "/"))

        Dim dt = New DataTable
        Using cn = New SqlConnection("data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
            Using cm = New SqlCommand("select convert(varchar(10), tourdate, 101) TourDate, TourTime from dbo.ufn_TourAvailability(@StartDate, @Days, @CampID, @LocID) order by TOURDATE", cn)
                cm.CommandType = CommandType.Text
                cm.Parameters.AddWithValue("@StartDate", DateTime.Parse(dateCheckIn).AddDays(1).ToShortDateString())
                cm.Parameters.AddWithValue("@Days", nights - 2)
                cm.Parameters.AddWithValue("@CampID", campaignID)
                cm.Parameters.AddWithValue("@LocID", tourLocationID)
                Try
                    cn.Open()
                    dt.Load(cm.ExecuteReader())
                    For Each dr As DataRow In dt.Rows
                        ta.Add(New AjaxTourAvailability With {.TourDate = dr("TourDate").ToString(), .TourTime = dr("TourTime").ToString()})
                    Next
                Catch ex As Exception
                    Dim er = ex.Message
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        Return New JavaScriptSerializer().Serialize(ta.ToList())
    End Function


    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True)>
    Public Shared Function AjaxGetInvoices(packageID As Int32, packageReservationID As Int32, dateCheckIn As String, nights As Int16) As String
        dateCheckIn = DateTime.Parse(dateCheckIn.Replace("|", "/"))
        Dim packageType = Utility.GetPackageType(packageID)
        Dim l = New List(Of AjaxInvoice)
        If packageType = "Tour Promotion" Then
            Dim i = New TourPromotionInvoice().Calculate(packageID, packageReservationID, dateCheckIn, nights)
            Dim p = New List(Of AjaxPayment)
            p.Add(New AjaxPayment With {.PaymentLine = "Discount", .PaymentAmount = i.First().Discount})
            l.Add(New AjaxInvoice With {.InvoiceLine = "ON-HOA", .InvoiceAmount = i.First().ONHOA, .AjaxPayments = p})
        ElseIf packageType = "Tradeshow" Then
            Dim i = New TradeshowInvoice().Calculate(packageID, packageReservationID, dateCheckIn, nights)
            Dim p = New List(Of AjaxPayment)
            p.Add(New AjaxPayment With {.PaymentLine = "Chargeback", .PaymentAmount = i.First().ChargeBack})
            l.Add(New AjaxInvoice With {.InvoiceLine = "ON-HOA", .InvoiceAmount = i.First().ONHOA, .AjaxPayments = p})
            l.Add(New AjaxInvoice With {.InvoiceLine = "Resort Fee", .InvoiceAmount = i.First().ResortFee, .AjaxPayments = New List(Of AjaxPayment)})
        ElseIf packageType = "Tour Package" Then
            Dim i = New TourPackageInvoice().Calculate(packageID, packageReservationID, dateCheckIn, nights)
            Dim p = New List(Of AjaxPayment)
            p.Add(New AjaxPayment With {.PaymentLine = "Chargeback", .PaymentAmount = i.First().ChargeBack})
            p.Add(New AjaxPayment With {.PaymentLine = "Discount", .PaymentAmount = i.First().Discount})
            l.Add(New AjaxInvoice With {.InvoiceLine = "ON-HOA", .InvoiceAmount = i.First().ONHOA, .AjaxPayments = p})
        ElseIf packageType = "Owner Getaway" Then
            Dim i = New OwnerGetAwayInvoice().Calculate(packageID, packageReservationID, dateCheckIn, nights)
            Dim p = New List(Of AjaxPayment)
            l.Add(New AjaxInvoice With {.InvoiceLine = "ON-HOA", .InvoiceAmount = i.First().ONHOA, .AjaxPayments = p})
        ElseIf packageType = "" Then
            Dim i = New RentalInvoice().Calculate(packageID, packageReservationID, dateCheckIn, nights)
            Dim p = New List(Of AjaxPayment)
            l.Add(New AjaxInvoice With {.InvoiceLine = "ON-HOA", .InvoiceAmount = i.First().ONHOA, .AjaxPayments = p})
        End If
        Return New JavaScriptSerializer().Serialize(l)
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True)>
    Public Shared Function AjaxIsExtraNightAllowed(packageReservationID As Int32, dateCheckIn As String) As String
        dateCheckIn = DateTime.Parse(dateCheckIn.Replace("|", "/"))
        Dim allowed = Utility.IsExtraNightAllowed(packageReservationID, dateCheckIn)
        Dim l = New List(Of AjaxExtraNightAllowed)
        If Utility.IsExtraNightAllowed(packageReservationID, dateCheckIn) Then
            l.Add(New AjaxExtraNightAllowed With {.IsAllowed = True})
        Else
            l.Add(New AjaxExtraNightAllowed With {.IsAllowed = False})
        End If
        Return New JavaScriptSerializer().Serialize(l)
    End Function

    Public Structure AjaxPackage
        Public ID As Int32
        Public Package As String
    End Structure

    Public Structure AjaxPackageDetail
        Public ID As Int32
        Public PackageReservationID As Int32
        Public UnitType As String
        Public BedRooms As String
        Public MinNights As Int32
        Public Vendor As String
        Public StartDate As String
        Public EndDate As String
        Public AllowExtraNight As Boolean
        Public Source As String
        Public PromoNights As Int32
        Public PromoRate As Decimal
        Public DefaultInvoiceAmt As Decimal
        Public ReservationType As String
        Public TourLocation As String
        Public Campaign As String
        Public TourSource As String
        Public TourType As String

        Public CampaignID As Int32
        Public TourLocationID As Int32
    End Structure

    Public Structure AjaxTourAvailability
        Public TourDate As String
        Public TourTime As String
    End Structure

    Public Structure AjaxExtraNightAllowed
        Public IsAllowed As Boolean
    End Structure

    Public Structure AjaxFinancialBalances
        Public Current As Decimal
        Public Future As Decimal
        Public Flag As String
    End Structure

    Public Class Invoice
    End Class

    Public Class ONHoaInvoice
        Inherits Invoice

        Public ONHOA As Decimal
    End Class

    Public Class OnHoaChargeBackInvoice
        Inherits ONHoaInvoice

        Public ChargeBack As Decimal
    End Class

    Public Class OnHoaChargeBackResortFeeInvoice
        Inherits OnHoaChargeBackInvoice

        Public ResortFee As Decimal
    End Class

    Public Class OnHoaDiscountInvoice
        Inherits ONHoaInvoice

        Public Discount As Decimal
    End Class

    Public Class OnHoaChargeBackDiscountInvoice
        Inherits OnHoaChargeBackInvoice

        Public Discount As Decimal
    End Class

    Public Class Utility
        Public Shared Function GetPackagePromoRate(packageReservationID As Int32) As Decimal
            Dim amt = 0D
            Using cn = New SqlConnection("data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
                Dim sql = String.Format("select coalesce(PromoRate, 0) PromoRate from t_PackageReservation where PackageReservationID = {0}", packageReservationID)
                Using cm = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        amt = CType(cm.ExecuteScalar(), Decimal)
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cm.Dispose()
                        cn.Close()
                    End Try
                End Using
            End Using
            Return amt
        End Function
        Public Shared Function GetPackagePromoNights(packageReservationID As Int32) As Int32
            Dim nights = 0
            Using cn = New SqlConnection("data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
                Dim sql = String.Format("select coalesce(PromoNights, 0) PromoNights from t_PackageReservation where PackageReservationID = {0}", packageReservationID)
                Using cm = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        nights = CType(cm.ExecuteScalar(), Int32)
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cm.Dispose()
                        cn.Close()
                    End Try
                End Using
            End Using
            Return nights
        End Function
        Public Shared Function GetPackageDefaultInvoiceAmt(packageID As Int32) As Decimal
            Dim amt = 0D
            Using cn = New SqlConnection("data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
                Dim sql = String.Format("select coalesce(DefaultInvoiceAmt, 0) DefaultInvoiceAmt from t_Package where PackageID = {0}", packageID)
                Using cm = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        amt = CType(cm.ExecuteScalar(), Decimal)
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cm.Dispose()
                        cn.Close()
                    End Try
                End Using
            End Using
            Return amt
        End Function
        Public Shared Function GetPackageType(packageID As Int32) As String
            Dim s As String = String.Empty
            Using cn = New SqlConnection("data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
                Dim sql = String.Format("select coalesce(pt.ComboItem, '') PackageType  from t_ComboItems pt inner join t_Package p on p.TypeID = pt.ComboItemID where p.PackageID = {0}", packageID)
                Using cm = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        s = CType(cm.ExecuteScalar(), String)
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cm.Dispose()
                        cn.Close()
                    End Try
                End Using
            End Using
            Return s
        End Function

        Public Shared Function GetResortFee(packageID As Integer) As Decimal
            Dim amt = 0D
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(String.Format("select top 1 isnull(prftc.Amount, 0) Amount from t_FinTransCodes ftc inner join t_ComboItems ft " _
                                          & "on ftc.TransCodeID = ft.ComboItemID inner join t_PackageReservationFinTransCode prftc " _
                                          & "on prftc.FinTransCodeID = ftc.FinTransID inner join t_PackageReservation pr " _
                                          & "on pr.PackageReservationID = prftc.PackageReservationID " _
                                          & "where ft.ComboItem = 'resort fee' and pr.PackageID = {0}", packageID), cn)
                    Try
                        cn.Open()
                        amt = cm.ExecuteScalar()
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return amt
        End Function

        Public Shared Function GetPackageBedrooms(packageID As Int32) As String
            Dim s = String.Empty
            Using cn = New SqlConnection("data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
                Dim sql = String.Format("select coalesce(Bedrooms, '') BedRooms from t_Package where PackageID = {0}", packageID)
                Using cm = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        s = cm.ExecuteScalar()
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cm.Dispose()
                        cn.Close()
                    End Try
                End Using
            End Using
            Return s
        End Function
        Public Shared Function IsExtraNightAllowed(packageReservationID As Integer, dateCheckedIn As Date) As Boolean
            Dim l = New List(Of CheckInDay)
            Using cn = New SqlConnection("data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
                Using cm = New SqlCommand(String.Format("Select * from t_PackageReservation2CheckInDay where PackageReservationID={0}", packageReservationID), cn)
                    Try
                        cn.Open()
                        Dim dt = New DataTable()
                        Dim rd = cm.ExecuteReader()
                        dt.Load(rd)
                        For Each dr As DataRow In dt.Rows

                            Dim dow As String = String.Empty
                            If dr("CheckInDay").ToString() = "0" Then
                                dow = "Sunday"
                            ElseIf dr("CheckInDay").ToString() = "1" Then
                                dow = "Monday"
                            ElseIf dr("CheckInDay").ToString() = "2" Then
                                dow = "Tuesday"
                            ElseIf dr("CheckInDay").ToString() = "3" Then
                                dow = "Wednesday"
                            ElseIf dr("CheckInDay").ToString() = "4" Then
                                dow = "Thursday"
                            ElseIf dr("CheckInDay").ToString() = "5" Then
                                dow = "Friday"
                            ElseIf dr("CheckInDay").ToString() = "6" Then
                                dow = "Saturday"
                            End If
                            l.Add(New CheckInDay With {.ID = dr("ID").ToString(), .DayOfWeek = dow})
                        Next

                    Catch ex As Exception
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return IIf(l.Where(Function(x) x.DayOfWeek = [Enum].GetName(GetType(DayOfWeek), dateCheckedIn.DayOfWeek)).Count() = 0, False, True)
        End Function

        Public Shared Function GetPackage1BedRoomCottage() As Int32

            Dim ID = 0
            Using cn = New SqlConnection("data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
                Using cm = New SqlCommand(String.Format("select top 1 p.PackageID from t_package p inner join t_comboitems pt on p.TypeID = pt.ComboItemID " _
                                                        & "inner join t_ComboItems ut on ut.ComboItemID = p.UnitTypeID " _
                                                        & "where pt.ComboItem = 'tour package' " _
                                                        & "and ut.ComboItem = 'cottage' and left(p.Bedrooms, 1) = 1 and p.Active = 1"), cn)

                    Try
                        cn.Open()
                        ID = cm.ExecuteScalar()
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return ID
        End Function


        Public Shared Function GetHolidayFee(dateCheckIn As Date, nights As Short) As Decimal
            Dim hf = 0D
            For i As Int16 = 0 To nights - 1
                If New clsRateTableHolidays().Date_Exists(dateCheckIn.AddDays(i).ToShortDateString()) Then
                    With New clsRateTableHolidays
                        .ID = New clsRateTableHolidays().Check_Date(dateCheckIn.AddDays(i).ToShortDateString())
                        .Load()
                        hf += .HolidayRate
                    End With
                End If
            Next
            Return hf
        End Function
    End Class

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True)>
    Public Shared Function GetFinancialDetails(prospectID As Integer, reservationID As Integer, packageID As Integer, packageReservationID As Integer, dateCheckIn As String, nights As Int16, userID As Int32) As String

        If packageID > 0 And dateCheckIn.Length > 0 Then
            Using cn = New SqlConnection("data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
                Using cm = New SqlCommand("sp_RW_Synch_Invoice", cn)

                    Try

                        dateCheckIn = DateTime.Parse(dateCheckIn.Replace("|", "/"))

                        Dim discount_amt = 0
                        Dim onhoa_amt = 0
                        Dim resort_fee_amt = 0
                        Dim charge_back_amt = 0
                        Dim packageType = Utility.GetPackageType(packageID)
                        If packageType = "Tour Promotion" Then

                            Dim i = New TourPromotionInvoice().Calculate(packageID, packageReservationID, dateCheckIn, nights)
                            discount_amt = i.First().Discount
                            onhoa_amt = i.First().ONHOA

                        ElseIf packageType = "Tradeshow" Then

                            Dim i = New TradeshowInvoice().Calculate(packageID, packageReservationID, dateCheckIn, nights)

                            resort_fee_amt = i.First().ResortFee
                            charge_back_amt = i.First().ChargeBack
                            onhoa_amt = i.First().ONHOA

                        ElseIf packageType = "Tour Package" Then

                            Dim i = New TourPackageInvoice().Calculate(packageID, packageReservationID, dateCheckIn, nights)

                            discount_amt = i.First().Discount
                            charge_back_amt = i.First().ChargeBack
                            onhoa_amt = i.First().ONHOA

                        ElseIf packageType = "Owner Getaway" Then

                            Dim i = New OwnerGetAwayInvoice().Calculate(packageID, packageReservationID, dateCheckIn, nights)
                            onhoa_amt = i.First().ONHOA
                        End If

                        If packageType = "Tradeshow" Or packageType = "Tour Package" Or packageType = "Tour Promotion" Or packageType = "Owner Getaway" Then
                            cm.CommandType = CommandType.StoredProcedure
                            cm.Parameters.Add("@RESERVATION_ID", SqlDbType.Int).Value = reservationID
                            cm.Parameters.Add("@PACKAGE_ID", SqlDbType.Int).Value = packageID
                            cm.Parameters.Add("@ONHOA_AMOUNT", SqlDbType.SmallMoney).Value = onhoa_amt
                            cm.Parameters.Add("@CHARGE_BACK_AMOUNT", SqlDbType.SmallMoney).Value = charge_back_amt
                            cm.Parameters.Add("@DISCOUNT_AMOUNT", SqlDbType.SmallMoney).Value = discount_amt
                            cm.Parameters.Add("@RESORT_FEE_AMOUNT", SqlDbType.SmallMoney).Value = resort_fee_amt
                            cm.Parameters.Add("@USER_ID", SqlDbType.Int).Value = userID
                            cn.Open()
                            cm.ExecuteNonQuery()
                        End If

                    Catch ex As Exception
                        cn.Close()
                    End Try
                End Using
            End Using
        End If



        Dim l = New List(Of AjaxFinancial)
        Using cn = New SqlConnection("data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
            Using cm = New SqlCommand(String.Format("select * from ufn_financials({0}, 'reservationid',{1},0) order by transdate desc;", prospectID, reservationID), cn)
                Try
                    cn.Open()
                    Dim dt = New DataTable()
                    Dim rd = cm.ExecuteReader()
                    dt.Load(rd)
                    For Each dr As DataRow In dt.Rows
                        l.Add(New AjaxFinancial With {.ID = dr("ID").ToString(),
                            .Invoice = dr("Invoice").ToString(),
                            .TransDate = Convert.ToDateTime(dr("TransDate").ToString()).ToShortDateString(),
                            .InvoiceAmount = dr("InvoiceAmount").ToString(),
                            .Balance = dr("Balance").ToString()})
                    Next
                Catch ex As Exception
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        Return New JavaScriptSerializer().Serialize(l.ToArray())
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True)>
    Public Shared Function GetCurrentFinancialBalance(packageID As Int32, packageReservationID As Int32, dateCheckIn As String, nights As Int16, prospectID As Integer, reservationID As Integer) As String
        Dim s = 0D
        Using cn = New SqlConnection("data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
            Using cm = New SqlCommand(String.Format("Select Case when Sum(Balance) is Null then 0 else Sum(Balance) end as Balance from v_Invoices where keyfield = 'reservationid' and keyvalue = {0}", reservationID), cn)
                Try
                    cn.Open()
                    s = cm.ExecuteScalar()
                    Dim r1 = AjaxGetInvoices(packageID, packageReservationID, dateCheckIn, nights)
                    Dim r2 = GetFinancialDetails(prospectID, reservationID, 0, 0, "", 0, 0)
                    Dim l1 = New JavaScriptSerializer().Deserialize(Of List(Of AjaxInvoice))(r1)
                    Dim l2 = New JavaScriptSerializer().Deserialize(Of List(Of AjaxFinancial))(r2)
                    Dim amtI_G = 0D
                    Dim amtP_G = 0D
                    For Each i In l1
                        Dim amtI = i.InvoiceAmount
                        Dim amtP = i.AjaxPayments.Sum(Function(x) x.PaymentAmount)
                        If amtP = 0 Then amtP = amtI
                        For Each p In l2.Where(Function(x) x.InvoiceAmount > 0)
                            If i.InvoiceLine.Substring(0, 3) = p.Invoice.Substring(0, 3) Then
                                Dim diff = 0
                                If amtP = amtI And i.AjaxPayments.Count = 0 Then
                                    diff = p.InvoiceAmount - amtP
                                Else
                                    amtP = p.InvoiceAmount
                                End If
                                amtI_G += (amtI - amtP + p.Balance + diff)
                            Else
                                amtP_G += p.Balance
                            End If
                        Next
                    Next

                    s = amtI_G + amtP_G
                Catch ex As Exception
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        Return New JavaScriptSerializer().Serialize(s)
    End Function

    Public Class OwnerGetAwayInvoice

        Public Function Calculate(packageID As Int32, packageReservationID As Int32, dateCheckIn As DateTime, nights As Int16) As List(Of ONHoaInvoice)
            Dim l As New List(Of ONHoaInvoice)
            Dim OnHoa As New ONHoaInvoice
            l.Add(OnHoa)
            Dim i = From e As Invoice In l
                    Where TypeOf e Is ONHoaInvoice

            Dim rateTableID = 0

            Using cn = New SqlConnection("data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
                Using cm = New SqlCommand("sp_RW_Get_RateTableID", cn)
                    cm.CommandType = CommandType.StoredProcedure
                    Try
                        cm.Parameters.Add("@PACKAGE_ID", SqlDbType.Int).Value = packageID
                        Dim paramOut = New SqlParameter("@RATE_TABLE_ID", SqlDbType.Int)
                        paramOut.Direction = ParameterDirection.Output
                        cm.Parameters.Add(paramOut)
                        cn.Open()
                        cm.ExecuteNonQuery()
                        rateTableID = cm.Parameters("@RATE_TABLE_ID").Value.ToString()
                        cn.Close()

                    Catch ex As Exception
                        cn.Close()
                    End Try
                End Using
            End Using
            Dim sql = String.Format("select (select sum(OwnerAmount) from t_RateTableRates " _
                    & "where Date between '{1}' and DATEADD(d, coalesce((select MinNights from t_Package where PackageID = {0}), 0) -1, '{1}') " _
                    & "and RateTableID = {3} ) + (select coalesce(sum(Amount), 0) from t_RateTableRates " _
                    & "where Date between DATEADD(d, coalesce((select MinNights from t_Package where PackageID = {0}), 0), '{1}') and " _
                    & "dateadd(d, ({2} - coalesce((select MinNights from t_Package where PackageID = {0}), 0)), DATEADD(d, coalesce((select MinNights from t_Package where PackageID = {0}), 0)-1, '{1}')) " _
                    & "and RateTableID = {3})", packageID, dateCheckIn.ToShortDateString(), nights, rateTableID)

            Using cn = New SqlConnection("data source=rs-sql-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
                Using cm = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        CType(i.First(), ONHoaInvoice).ONHOA = CType(cm.ExecuteScalar(), Decimal)
                    Catch ex As Exception
                        cn.Close()
                    End Try
                End Using
            End Using
            Return l
        End Function
    End Class

    Public Class RentalInvoice

        Public Function Calculate(packageID As Int32, packageReservationID As Int32, dateCheckIn As DateTime, nights As Int16) As List(Of ONHoaInvoice)
            Dim l As New List(Of ONHoaInvoice)
            Dim OnHoa As New ONHoaInvoice
            l.Add(OnHoa)
            Dim i = From e As Invoice In l
                    Where TypeOf e Is ONHoaInvoice


            Return l
        End Function
    End Class



    Public Class TourPromotionInvoice
        Public Function Calculate(packageID As Int32, packageReservationID As Int32, dateCheckIn As DateTime, nights As Int16) As List(Of OnHoaDiscountInvoice)
            Dim l As New List(Of OnHoaDiscountInvoice)
            Dim OnHoa As New OnHoaDiscountInvoice
            l.Add(OnHoa)
            Dim i = From e As Invoice In l
                    Where TypeOf e Is OnHoaDiscountInvoice

            Dim promoRate = Utility.GetPackagePromoRate(packageReservationID)
            Dim promoNights = Utility.GetPackagePromoNights(packageReservationID)
            Dim defaultInvoiceAmount = Utility.GetPackageDefaultInvoiceAmt(packageID)
            Dim holidayFee = Utility.GetHolidayFee(dateCheckIn.ToShortDateString(), nights)

            If defaultInvoiceAmount > 0 Then       'marketing
                If nights = promoNights Then
                    If promoRate >= defaultInvoiceAmount Then
                        CType(i.First(), ONHoaInvoice).ONHOA = promoRate + holidayFee
                        CType(i.First(), OnHoaDiscountInvoice).Discount = 0
                    Else
                        If (promoRate + holidayFee) >= defaultInvoiceAmount Then
                            CType(i.First(), ONHoaInvoice).ONHOA = promoRate + holidayFee
                            CType(i.First(), OnHoaDiscountInvoice).Discount = 0
                        Else
                            CType(i.First(), ONHoaInvoice).ONHOA = defaultInvoiceAmount
                            CType(i.First(), OnHoaDiscountInvoice).Discount = defaultInvoiceAmount - promoRate - holidayFee

                        End If
                    End If
                Else

                    CType(i.First(), ONHoaInvoice).ONHOA = New clsReservationWizard().Get_Package_Rate(packageID,
                                                dateCheckIn.ToShortDateString(),
                                                dateCheckIn.AddDays(nights - 1).ToShortDateString, True)

                    CType(i.First(), OnHoaDiscountInvoice).Discount = 0
                End If
            Else
                Dim calculatedRate = New clsReservationWizard().Get_Invoice_Amt(
                    New Int32() {packageID}.ToArray(), 0D, dateCheckIn, promoNights)

                If nights = promoNights Then

                    CType(i.First(), ONHoaInvoice).ONHOA = calculatedRate

                    If calculatedRate > promoRate Then
                        CType(i.First(), OnHoaDiscountInvoice).Discount = calculatedRate - promoRate - holidayFee
                    Else
                        CType(i.First(), OnHoaDiscountInvoice).Discount = 0
                    End If

                Else
                    Dim calculatedRateAdditional = New clsReservationWizard().Get_Invoice_Amt(
                        New Int32() {packageID}.ToArray(), 0D, dateCheckIn.ToShortDateString(), nights) - calculatedRate

                    CType(i.First(), ONHoaInvoice).ONHOA = calculatedRate + calculatedRateAdditional

                    If (calculatedRate + calculatedRateAdditional) > promoRate Then
                        CType(i.First(), OnHoaDiscountInvoice).Discount = CType(i.First(), ONHoaInvoice).ONHOA - (promoRate + calculatedRateAdditional) - holidayFee
                    Else
                        CType(i.First(), OnHoaDiscountInvoice).Discount = 0
                    End If
                End If
            End If
            Return l
        End Function
    End Class

    Public Class TradeshowInvoice
        Public Function Calculate(packageID As Int32, packageReservationID As Int32, dateCheckIn As DateTime, nights As Int16) As List(Of OnHoaChargeBackResortFeeInvoice)
            Dim l As New List(Of OnHoaChargeBackResortFeeInvoice)
            Dim OnHoa As New OnHoaChargeBackResortFeeInvoice
            l.Add(OnHoa)
            Dim i = From e As Invoice In l
                    Where TypeOf e Is OnHoaChargeBackResortFeeInvoice

            Dim promoRate = Utility.GetPackagePromoRate(packageReservationID)
            Dim promoNights = Utility.GetPackagePromoNights(packageReservationID)
            Dim defaultInvoiceAmount = Utility.GetPackageDefaultInvoiceAmt(packageID)
            If defaultInvoiceAmount > 0 Then    'marketing inventory
                If promoNights = nights Then
                    CType(i.First(), OnHoaChargeBackInvoice).ONHOA = defaultInvoiceAmount
                    CType(i.First(), OnHoaChargeBackInvoice).ChargeBack = CType(i.First(), OnHoaChargeBackInvoice).ONHOA
                Else
                    CType(i.First(), OnHoaChargeBackInvoice).ONHOA = New clsReservationWizard().Get_Invoice_Amt(
                                New Int32() {packageID}.ToArray(), 0D,
                                dateCheckIn.ToShortDateString(), nights)

                    CType(i.First(), OnHoaChargeBackInvoice).ChargeBack = defaultInvoiceAmount
                End If
            Else    'rental inventory
                CType(i.First(), OnHoaChargeBackInvoice).ONHOA = New clsReservationWizard().Get_Invoice_Amt(
                             New Int32() {packageID}.ToArray(), 0D,
                             dateCheckIn.ToShortDateString(), nights)

                If promoNights = nights Then

                    CType(i.First(), OnHoaChargeBackInvoice).ChargeBack = CType(i.First(), OnHoaChargeBackInvoice).ONHOA
                Else
                    'passing 0 as the last parameter has no effect on the calculation
                    CType(i.First(), OnHoaChargeBackInvoice).ChargeBack = New clsReservationWizard().Get_Invoice_Amt(
                            New Int32() {packageID}.ToArray(), 0D,
                            dateCheckIn.ToShortDateString(), 0)
                End If
            End If
            CType(i.First(), OnHoaChargeBackResortFeeInvoice).ResortFee = Utility.GetResortFee(packageID)
            Return l
        End Function
    End Class

    Public Class TourPackageInvoice
        Public Function Calculate(packageID As Int32, packageReservationID As Int32, dateCheckIn As DateTime, nights As Int16) As List(Of OnHoaChargeBackDiscountInvoice)
            Dim l As New List(Of OnHoaChargeBackDiscountInvoice)
            Dim OnHoa As New OnHoaChargeBackDiscountInvoice
            l.Add(OnHoa)
            Dim i = From e As Invoice In l
                    Where TypeOf e Is OnHoaChargeBackDiscountInvoice

            Dim promoRate = Utility.GetPackagePromoRate(packageReservationID)
            Dim promoNights = Utility.GetPackagePromoNights(packageReservationID)
            Dim defaultInvoiceAmount = Utility.GetPackageDefaultInvoiceAmt(packageID)
            Dim bedrooms = Utility.GetPackageBedrooms(packageID)
            If bedrooms.Length > 0 Then bedrooms = bedrooms.Substring(0, 1)
            Dim ex_night = Utility.IsExtraNightAllowed(packageReservationID, dateCheckIn.ToShortDateString())
            If ex_night = False Then
                If bedrooms = 2 Then

                    Dim cottage1BrID = Utility.GetPackage1BedRoomCottage()

                    'calculates tour package's 3 nights rate for any 1 bedroom cottage 
                    Dim cb_2_nights = New clsReservationWizard().Get_Invoice_Amt(
                                New Int32() {cottage1BrID}.ToArray(), 0D,
                                dateCheckIn.ToShortDateString(), 2)
                    Dim cb_per_night = cb_2_nights / 2
                    CType(i.First(), OnHoaChargeBackInvoice).ChargeBack = cb_per_night * promoNights

                    'calculates current tour package's 3 nights rate for the 2 bedroom cottage
                    CType(i.First(), ONHoaInvoice).ONHOA = New clsReservationWizard().Get_Invoice_Amt(
                                 New Int32() {packageID}.ToArray(), 0D,
                                 dateCheckIn.ToShortDateString(), nights)


                    CType(i.First(), OnHoaChargeBackDiscountInvoice).Discount = CType(i.First(), ONHoaInvoice).ONHOA - CType(i.First(), OnHoaChargeBackInvoice).ChargeBack


                    '// commented on 8/8/2017
                    ''calculates tour package's 3 nights rate for any 1 bedroom cottage 
                    'CType(i.First(), OnHoaChargeBackInvoice).ChargeBack = New clsReservationWizard().Get_Invoice_Amt(
                    '    New Int32() {cottage1BrID}.ToArray(), 0D,
                    '    dateCheckIn.ToShortDateString(), promoNights)

                    ''calculates current tour package's 3 nights rate for the 2 bedroom cottage
                    'CType(i.First(), ONHoaInvoice).ONHOA = New clsReservationWizard().Get_Invoice_Amt(
                    '     New Int32() {packageID}.ToArray(), 0D,
                    '     dateCheckIn.ToShortDateString(), nights)

                    'If nights = (promoNights + 0) Then
                    '    CType(i.First(), OnHoaChargeBackDiscountInvoice).Discount = CType(i.First(), ONHoaInvoice).ONHOA - CType(i.First(), OnHoaChargeBackInvoice).ChargeBack
                    'Else
                    '    'column 0: MethodID, column 1: Amount
                    '    Dim dt = New clsReservationWizard().Get_Discounts(New Int32() {packageID}.ToArray(), 0, 0, 0, nights, dateCheckIn.ToShortDateString())
                    '    If dt.Rows.Count > 0 Then
                    '        CType(i.First(), OnHoaChargeBackDiscountInvoice).Discount = dt.Rows.OfType(Of DataRow).Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("Amount")))
                    '    End If
                    'End If

                Else

                    Dim inv_amt = New clsReservationWizard().Get_Invoice_Amt(
                       New Int32() {packageID}.ToArray(), 0D,
                       dateCheckIn.ToShortDateString(), nights)

                    CType(i.First(), OnHoaChargeBackInvoice).ChargeBack = New clsReservationWizard().Get_Invoice_Amt(
                        New Int32() {packageID}.ToArray(), 0D,
                        dateCheckIn.ToShortDateString(), promoNights)

                    CType(i.First(), ONHoaInvoice).ONHOA = inv_amt
                End If
            Else    'ex_night = true

                Dim cottage1BrID = Utility.GetPackage1BedRoomCottage()

                If promoNights = 2 Then
                    If bedrooms = 2 Then
                        'calculates tour package's 3 nights rate for any 1 bedroom cottage 
                        Dim cb_2_nights = New clsReservationWizard().Get_Invoice_Amt(
                                New Int32() {cottage1BrID}.ToArray(), 0D,
                                dateCheckIn.ToShortDateString(), promoNights)
                        cb_2_nights = cb_2_nights / promoNights

                        CType(i.First(), OnHoaChargeBackInvoice).ChargeBack = cb_2_nights * (promoNights + 1)

                        'calculates current tour package's 3 nights rate for the 2 bedroom cottage
                        CType(i.First(), ONHoaInvoice).ONHOA = New clsReservationWizard().Get_Invoice_Amt(
                                 New Int32() {packageID}.ToArray(), 0D,
                                 dateCheckIn.ToShortDateString(), nights)


                        If nights = (promoNights + 1) Then
                            CType(i.First(), OnHoaChargeBackDiscountInvoice).Discount = CType(i.First(), ONHoaInvoice).ONHOA - CType(i.First(), OnHoaChargeBackInvoice).ChargeBack
                        Else
                            'fixed on 7/6
                            CType(i.First(), OnHoaChargeBackDiscountInvoice).Discount = New clsReservationWizard().Get_Invoice_Amt(
                                 New Int32() {packageID}.ToArray(), 0D,
                                 dateCheckIn.ToShortDateString(), promoNights + 1) - New clsReservationWizard().Get_Invoice_Amt(
                                New Int32() {cottage1BrID}.ToArray(), 0D,
                                dateCheckIn.ToShortDateString(), promoNights + 1)

                        End If


                    Else

                        CType(i.First(), ONHoaInvoice).ONHOA = New clsReservationWizard().Get_Invoice_Amt(
                                                       New Int32() {packageID}.ToArray(), 0D,
                                                       dateCheckIn.ToShortDateString(), nights)

                        If (promoNights + 1) = nights Then
                            CType(i.First(), OnHoaChargeBackInvoice).ChargeBack = CType(i.First(), ONHoaInvoice).ONHOA
                        Else
                            CType(i.First(), OnHoaChargeBackInvoice).ChargeBack = New clsReservationWizard().Get_Invoice_Amt(
                                  New Int32() {packageID}.ToArray(), 0D,
                                  dateCheckIn.ToShortDateString(), promoNights + 1)
                        End If
                    End If

                ElseIf promoNights = 3 Then
                    If bedrooms = 2 Then

                        'calculates tour package's 3 nights rate for any 1 bedroom cottage 
                        CType(i.First(), OnHoaChargeBackInvoice).ChargeBack = New clsReservationWizard().Get_Invoice_Amt(
                                New Int32() {cottage1BrID}.ToArray(), 0D,
                                dateCheckIn.ToShortDateString(), promoNights)

                        'calculates current tour package's 4 nights rate for the 2 bedroom cottage
                        CType(i.First(), ONHoaInvoice).ONHOA = New clsReservationWizard().Get_Invoice_Amt(
                                 New Int32() {packageID}.ToArray(), 0D,
                                 dateCheckIn.ToShortDateString(), nights)

                        CType(i.First(), OnHoaChargeBackDiscountInvoice).Discount = CType(i.First(), ONHoaInvoice).ONHOA - CType(i.First(), OnHoaChargeBackInvoice).ChargeBack
                    Else

                        CType(i.First(), ONHoaInvoice).ONHOA = New clsReservationWizard().Get_Invoice_Amt(
                            New Int32() {packageID}.ToArray(), 0D,
                            dateCheckIn.ToShortDateString(), nights)

                        CType(i.First(), OnHoaChargeBackInvoice).ChargeBack = New clsReservationWizard().Get_Invoice_Amt(
                                   New Int32() {packageID}.ToArray(), 0D,
                                   dateCheckIn.ToShortDateString(), promoNights)

                        If nights <= (promoNights + 1) Then
                            CType(i.First(), OnHoaChargeBackDiscountInvoice).Discount = CType(i.First(), ONHoaInvoice).ONHOA - CType(i.First(), OnHoaChargeBackInvoice).ChargeBack
                        Else
                            'column 0: MethodID, column 1: Amount
                            Dim dt = New clsReservationWizard().Get_Discounts(New Int32() {packageID}.ToArray(), 0, 0, 0, nights, dateCheckIn.ToShortDateString())
                            If dt.Rows.Count > 0 Then
                                CType(i.First(), OnHoaChargeBackDiscountInvoice).Discount = dt.Rows.OfType(Of DataRow).Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("Amount")))
                            End If
                        End If
                    End If
                End If
            End If
            Return l
        End Function
    End Class



    Public Class AjaxInvoice
        Public InvoiceLine As String
        Public InvoiceAmount As Decimal
        Public AjaxPayments As New List(Of AjaxPayment)
    End Class

    Public Class AjaxPayment
        Public PaymentLine As String
        Public PaymentAmount As Decimal
    End Class
    Public Class CheckInDay
        Public ID As Int32
        Public DayOfWeek As String
    End Class

    Public Class AjaxFinancial
        Public ID As Int32
        Public Invoice As String
        Public TransDate As String
        Public InvoiceAmount As Decimal
        Public Balance As Decimal
    End Class
End Class
