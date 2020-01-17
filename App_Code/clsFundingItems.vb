Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsFundingItems
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _FundingID As Integer = 0
    Dim _DateCreated As String = ""
    Dim _CreatedByID As Integer = 0
    Dim _ContractID As Integer = 0
    Dim _ContractNumber As String = ""
    Dim _LastName As String = ""
    Dim _FirstName As String = ""
    Dim _Customer2 As String = ""
    Dim _CommissionVolume As Decimal = 0
    Dim _Credit As String = ""
    Dim _Term As Integer = 0
    Dim _Rate As Decimal = 0
    Dim _Season As String = ""
    Dim _UnitWeek As String = ""
    Dim _ContractDate As String = ""
    Dim _Debit As String = ""
    Dim _Resides As String = ""
    Dim _CancelType As String = ""
    Dim _ActualDown As Decimal = 0
    Dim _Payments As Decimal = 0
    Dim _EquityCCMF As Decimal = 0
    Dim _EquityDP As Decimal = 0
    Dim _EquityExitDP As Decimal = 0
    Dim _EquityExitCC As Decimal = 0
    Dim _DirectDeposit As Boolean = False
    Dim _OWDate As String = ""
    Dim _MonthlyPaymentPlus As Decimal = 0
    Dim _EOY As String = ""
    Dim _OccupancyYear As Integer = 0
    Dim _DownPaymentCCCK As Decimal = 0
    Dim _ClosingCosts As Decimal = 0
    Dim _CottageTownes As String = ""
    Dim _Bedrooms As Integer = 0
    Dim _IHLN As String = ""
    Dim _Intervals As Decimal = 0
    Dim _TourCampaign As String = ""
    Dim _PreSale As Boolean = False
    Dim _SalesPrice As Decimal = 0
    Dim _TotalDown As Decimal = 0
    Dim _TotalFinanced As Decimal = 0
    Dim _Frequency As String = ""
    Dim _DeedBook As String = ""
    Dim _Page As String = ""
    Dim _SubtractionFlag As Boolean = False
    Dim _DownPaymentCCCKType As String = ""
    Dim _ClosingCostCCCKType As String = ""
    Dim _CCFinanced As Decimal = 0
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_FundingItems where FundingItemID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_FundingItems where FundingItemID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_FundingItems")
            If ds.Tables("t_FundingItems").Rows.Count > 0 Then
                dr = ds.Tables("t_FundingItems").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("FundingID") Is System.DBNull.Value) Then _FundingID = dr("FundingID")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ContractID = dr("ContractID")
        If Not (dr("ContractNumber") Is System.DBNull.Value) Then _ContractNumber = dr("ContractNumber")
        If Not (dr("LastName") Is System.DBNull.Value) Then _LastName = dr("LastName")
        If Not (dr("FirstName") Is System.DBNull.Value) Then _FirstName = dr("FirstName")
        If Not (dr("Customer2") Is System.DBNull.Value) Then _Customer2 = dr("Customer2")
        If Not (dr("CommissionVolume") Is System.DBNull.Value) Then _CommissionVolume = dr("CommissionVolume")
        If Not (dr("Credit") Is System.DBNull.Value) Then _Credit = dr("Credit")
        If Not (dr("Term") Is System.DBNull.Value) Then _Term = dr("Term")
        If Not (dr("Rate") Is System.DBNull.Value) Then _Rate = dr("Rate")
        If Not (dr("Season") Is System.DBNull.Value) Then _Season = dr("Season")
        If Not (dr("UnitWeek") Is System.DBNull.Value) Then _UnitWeek = dr("UnitWeek")
        If Not (dr("ContractDate") Is System.DBNull.Value) Then _ContractDate = dr("ContractDate")
        If Not (dr("Debit") Is System.DBNull.Value) Then _Debit = dr("Debit")
        If Not (dr("Resides") Is System.DBNull.Value) Then _Resides = dr("Resides")
        If Not (dr("CancelType") Is System.DBNull.Value) Then _CancelType = dr("CancelType")
        If Not (dr("ActualDown") Is System.DBNull.Value) Then _ActualDown = dr("ActualDown")
        If Not (dr("Payments") Is System.DBNull.Value) Then _Payments = dr("Payments")
        If Not (dr("EquityCCMF") Is System.DBNull.Value) Then _EquityCCMF = dr("EquityCCMF")
        If Not (dr("EquityDP") Is System.DBNull.Value) Then _EquityDP = dr("EquityDP")
        If Not (dr("EquityExitDP") Is System.DBNull.Value) Then _EquityExitDP = dr("EquityExitDP")
        If Not (dr("EquityExitCC") Is System.DBNull.Value) Then _EquityExitCC = dr("EquityExitCC")
        If Not (dr("DirectDeposit") Is System.DBNull.Value) Then _DirectDeposit = dr("DirectDeposit")
        If Not (dr("OWDate") Is System.DBNull.Value) Then _OWDate = dr("OWDate")
        If Not (dr("MonthlyPaymentPlus") Is System.DBNull.Value) Then _MonthlyPaymentPlus = dr("MonthlyPaymentPlus")
        If Not (dr("EOY") Is System.DBNull.Value) Then _EOY = dr("EOY")
        If Not (dr("OccupancyYear") Is System.DBNull.Value) Then _OccupancyYear = dr("OccupancyYear")
        If Not (dr("DownPaymentCCCK") Is System.DBNull.Value) Then _DownPaymentCCCK = dr("DownPaymentCCCK")
        If Not (dr("ClosingCosts") Is System.DBNull.Value) Then _ClosingCosts = dr("ClosingCosts")
        If Not (dr("CottageTownes") Is System.DBNull.Value) Then _CottageTownes = dr("CottageTownes")
        If Not (dr("Bedrooms") Is System.DBNull.Value) Then _Bedrooms = dr("Bedrooms")
        If Not (dr("IHLN") Is System.DBNull.Value) Then _IHLN = dr("IHLN")
        If Not (dr("Intervals") Is System.DBNull.Value) Then _Intervals = dr("Intervals")
        If Not (dr("TourCampaign") Is System.DBNull.Value) Then _TourCampaign = dr("TourCampaign")
        If Not (dr("PreSale") Is System.DBNull.Value) Then _PreSale = dr("PreSale")
        If Not (dr("SalesPrice") Is System.DBNull.Value) Then _SalesPrice = dr("SalesPrice")
        If Not (dr("TotalDown") Is System.DBNull.Value) Then _TotalDown = dr("TotalDown")
        If Not (dr("TotalFinanced") Is System.DBNull.Value) Then _TotalFinanced = dr("TotalFinanced")
        If Not (dr("Frequency") Is System.DBNull.Value) Then _Frequency = dr("Frequency")
        If Not (dr("DeedBook") Is System.DBNull.Value) Then _DeedBook = dr("DeedBook")
        If Not (dr("Page") Is System.DBNull.Value) Then _Page = dr("Page")
        If Not (dr("SubtractionFlag") Is System.DBNull.Value) Then _SubtractionFlag = dr("SubtractionFlag")
        If Not (dr("DownPaymentCCCKType") Is System.DBNull.Value) Then _DownPaymentCCCKType = dr("DownPaymentCCCKType")
        If Not (dr("ClosingCostCCCKType") Is System.DBNull.Value) Then _ClosingCostCCCKType = dr("ClosingCostCCCKType")
        If Not (dr("CCFinanced") Is System.DBNull.Value) Then _CCFinanced = dr("CCFinanced")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_FundingItems where FundingItemID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_FundingItems")
            If ds.Tables("t_FundingItems").Rows.Count > 0 Then
                dr = ds.Tables("t_FundingItems").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_FundingItemsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@FundingID", SqlDbType.int, 0, "FundingID")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.int, 0, "ContractID")
                da.InsertCommand.Parameters.Add("@ContractNumber", SqlDbType.varchar, 0, "ContractNumber")
                da.InsertCommand.Parameters.Add("@LastName", SqlDbType.varchar, 0, "LastName")
                da.InsertCommand.Parameters.Add("@FirstName", SqlDbType.varchar, 0, "FirstName")
                da.InsertCommand.Parameters.Add("@Customer2", SqlDbType.varchar, 0, "Customer2")
                da.InsertCommand.Parameters.Add("@CommissionVolume", SqlDbType.money, 0, "CommissionVolume")
                da.InsertCommand.Parameters.Add("@Credit", SqlDbType.varchar, 0, "Credit")
                da.InsertCommand.Parameters.Add("@Term", SqlDbType.int, 0, "Term")
                da.InsertCommand.Parameters.Add("@Rate", SqlDbType.float, 0, "Rate")
                da.InsertCommand.Parameters.Add("@Season", SqlDbType.varchar, 0, "Season")
                da.InsertCommand.Parameters.Add("@UnitWeek", SqlDbType.varchar, 0, "UnitWeek")
                da.InsertCommand.Parameters.Add("@ContractDate", SqlDbType.datetime, 0, "ContractDate")
                da.InsertCommand.Parameters.Add("@Debit", SqlDbType.varchar, 0, "Debit")
                da.InsertCommand.Parameters.Add("@Resides", SqlDbType.varchar, 0, "Resides")
                da.InsertCommand.Parameters.Add("@CancelType", SqlDbType.varchar, 0, "CancelType")
                da.InsertCommand.Parameters.Add("@ActualDown", SqlDbType.money, 0, "ActualDown")
                da.InsertCommand.Parameters.Add("@Payments", SqlDbType.money, 0, "Payments")
                da.InsertCommand.Parameters.Add("@EquityCCMF", SqlDbType.money, 0, "EquityCCMF")
                da.InsertCommand.Parameters.Add("@EquityDP", SqlDbType.money, 0, "EquityDP")
                da.InsertCommand.Parameters.Add("@EquityExitDP", SqlDbType.money, 0, "EquityExitDP")
                da.InsertCommand.Parameters.Add("@EquityExitCC", SqlDbType.Money, 0, "EquityExitCC")
                da.InsertCommand.Parameters.Add("@DirectDeposit", SqlDbType.bit, 0, "DirectDeposit")
                da.InsertCommand.Parameters.Add("@OWDate", SqlDbType.datetime, 0, "OWDate")
                da.InsertCommand.Parameters.Add("@MonthlyPaymentPlus", SqlDbType.money, 0, "MonthlyPaymentPlus")
                da.InsertCommand.Parameters.Add("@EOY", SqlDbType.varchar, 0, "EOY")
                da.InsertCommand.Parameters.Add("@OccupancyYear", SqlDbType.int, 0, "OccupancyYear")
                da.InsertCommand.Parameters.Add("@DownPaymentCCCK", SqlDbType.money, 0, "DownPaymentCCCK")
                da.InsertCommand.Parameters.Add("@ClosingCosts", SqlDbType.money, 0, "ClosingCosts")
                da.InsertCommand.Parameters.Add("@CottageTownes", SqlDbType.varchar, 0, "CottageTownes")
                da.InsertCommand.Parameters.Add("@Bedrooms", SqlDbType.int, 0, "Bedrooms")
                da.InsertCommand.Parameters.Add("@IHLN", SqlDbType.varchar, 0, "IHLN")
                da.InsertCommand.Parameters.Add("@Intervals", SqlDbType.money, 0, "Intervals")
                da.InsertCommand.Parameters.Add("@TourCampaign", SqlDbType.varchar, 0, "TourCampaign")
                da.InsertCommand.Parameters.Add("@PreSale", SqlDbType.bit, 0, "PreSale")
                da.InsertCommand.Parameters.Add("@SalesPrice", SqlDbType.money, 0, "SalesPrice")
                da.InsertCommand.Parameters.Add("@TotalDown", SqlDbType.money, 0, "TotalDown")
                da.InsertCommand.Parameters.Add("@TotalFinanced", SqlDbType.money, 0, "TotalFinanced")
                da.InsertCommand.Parameters.Add("@Frequency", SqlDbType.varchar, 0, "Frequency")
                da.InsertCommand.Parameters.Add("@DeedBook", SqlDbType.varchar, 0, "DeedBook")
                da.InsertCommand.Parameters.Add("@Page", SqlDbType.varchar, 0, "Page")
                da.InsertCommand.Parameters.Add("@SubtractionFlag", SqlDbType.bit, 0, "SubtractionFlag")
                da.InsertCommand.Parameters.Add("@DownPaymentCCCKType", SqlDbType.varchar, 0, "DownPaymentCCCKType")
                da.InsertCommand.Parameters.Add("@ClosingCostCCCKType", SqlDbType.varchar, 0, "ClosingCostCCCKType")
                da.InsertCommand.Parameters.Add("@CCFinanced", SqlDbType.money, 0, "CCFinanced")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@FundingItemID", SqlDbType.Int, 0, "FundingItemID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_FundingItems").NewRow
            End If
            Update_Field("FundingID", _FundingID, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("ContractID", _ContractID, dr)
            Update_Field("ContractNumber", _ContractNumber, dr)
            Update_Field("LastName", _LastName, dr)
            Update_Field("FirstName", _FirstName, dr)
            Update_Field("Customer2", _Customer2, dr)
            Update_Field("CommissionVolume", _CommissionVolume, dr)
            Update_Field("Credit", _Credit, dr)
            Update_Field("Term", _Term, dr)
            Update_Field("Rate", _Rate, dr)
            Update_Field("Season", _Season, dr)
            Update_Field("UnitWeek", _UnitWeek, dr)
            Update_Field("ContractDate", _ContractDate, dr)
            Update_Field("Debit", _Debit, dr)
            Update_Field("Resides", _Resides, dr)
            Update_Field("CancelType", _CancelType, dr)
            Update_Field("ActualDown", _ActualDown, dr)
            Update_Field("Payments", _Payments, dr)
            Update_Field("EquityCCMF", _EquityCCMF, dr)
            Update_Field("EquityDP", _EquityDP, dr)
            Update_Field("EquityExitDP", _EquityExitDP, dr)
            Update_Field("EquityExitCC", _EquityExitCC, dr)
            Update_Field("DirectDeposit", _DirectDeposit, dr)
            Update_Field("OWDate", _OWDate, dr)
            Update_Field("MonthlyPaymentPlus", _MonthlyPaymentPlus, dr)
            Update_Field("EOY", _EOY, dr)
            Update_Field("OccupancyYear", _OccupancyYear, dr)
            Update_Field("DownPaymentCCCK", _DownPaymentCCCK, dr)
            Update_Field("ClosingCosts", _ClosingCosts, dr)
            Update_Field("CottageTownes", _CottageTownes, dr)
            Update_Field("Bedrooms", _Bedrooms, dr)
            Update_Field("IHLN", _IHLN, dr)
            Update_Field("Intervals", _Intervals, dr)
            Update_Field("TourCampaign", _TourCampaign, dr)
            Update_Field("PreSale", _PreSale, dr)
            Update_Field("SalesPrice", _SalesPrice, dr)
            Update_Field("TotalDown", _TotalDown, dr)
            Update_Field("TotalFinanced", _TotalFinanced, dr)
            Update_Field("Frequency", _Frequency, dr)
            Update_Field("DeedBook", _DeedBook, dr)
            Update_Field("Page", _Page, dr)
            Update_Field("SubtractionFlag", _SubtractionFlag, dr)
            Update_Field("DownPaymentCCCKType", _DownPaymentCCCKType, dr)
            Update_Field("ClosingCostCCCKType", _ClosingCostCCCKType, dr)
            Update_Field("CCFinanced", _CCFinanced, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_FundingItems").Rows.Count < 1 Then ds.Tables("t_FundingItems").Rows.Add(dr)
            da.Update(ds, "t_FundingItems")
            _ID = ds.Tables("t_FundingItems").Rows(0).Item("FundingItemID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Function

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "FundingItemID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Function List_FundingItems(ByVal fundingID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select fundingitemid,fundingid,Case when SubtractionFlag = 0 then 'False' else 'True' end as SubtractionFlag,f.ContractID, 'N/A' as [NEW/OLD KCP],contractnumber as [KCP #], " &
                                " lastname + ', ' + firstname as Owner, SalesPrice as [Sales Price],totaldown as [Total Down Payment], " &
                                " DownPaymentCCCKType as [Down Payment Type], " &
                                " ccfinanced as [Closing Cost Financed], totalfinanced as Financed, " &
                                " equitydp as [Equity], Term,Rate,Season," &
                                "case when (select top 1 Creditscore from t_Prospect p inner join t_Contract c on c.ProspectID=p.ProspectID where c.ContractID = f.contractid) IS null then 0 else (select top 1 Creditscore from t_Prospect p inner join t_Contract c on c.ProspectID=p.ProspectID where c.ContractID = f.contractid) end as FICO," &
                                "case when (select top 1 case when co.prospectid = c.prospectid then p.SpouseCreditScore else CreditScore end  from t_Prospect p inner join t_ContractCoOwner co on co.ProspectID=p.ProspectID inner join t_Contract c on c.contractid = co.contractid where co.ContractID = f.contractid) is null then 0 else (select top 1 case when co.prospectid = c.prospectid then p.SpouseCreditScore else CreditScore end  from t_Prospect p inner join t_ContractCoOwner co on co.ProspectID=p.ProspectID inner join t_Contract c on c.contractid = co.contractid where co.ContractID = f.contractid) end as FICO2," &
                                "left(convert(varchar(50), round(cast ((case when (select top 1 Creditscore from t_Prospect p inner join t_Contract c on c.ProspectID=p.ProspectID where c.ContractID = f.contractid) IS null then 0 else (select top 1 Creditscore from t_Prospect p inner join t_Contract c on c.ProspectID=p.ProspectID where c.ContractID = f.contractid) end + case when (select top 1 case when co.prospectid = c.prospectid then p.SpouseCreditScore else CreditScore end  from t_Prospect p inner join t_ContractCoOwner co on co.ProspectID=p.ProspectID inner join t_Contract c on c.contractid = co.contractid where co.ContractID = f.contractid) is null then 0 else (select top 1 case when co.prospectid = c.prospectid then p.SpouseCreditScore else CreditScore end  from t_Prospect p inner join t_ContractCoOwner co on co.ProspectID=p.ProspectID inner join t_Contract c on c.contractid = co.contractid where co.ContractID = f.contractid) end) as money) / cast (case when case when (select top 1 Creditscore from t_Prospect p inner join t_Contract c on c.ProspectID=p.ProspectID where c.ContractID = f.contractid) IS null then 0 else (select top 1 Creditscore from t_Prospect p inner join t_Contract c on c.ProspectID=p.ProspectID where c.ContractID = f.contractid) end = 0 OR case when (select top 1 case when co.prospectid = c.prospectid then p.SpouseCreditScore else CreditScore end  from t_Prospect p inner join t_ContractCoOwner co on co.ProspectID=p.ProspectID inner join t_Contract c on c.contractid = co.contractid where co.ContractID = f.contractid) is null then 0 else (select top 1 case when co.prospectid = c.prospectid then p.SpouseCreditScore else CreditScore end  from t_Prospect p inner join t_ContractCoOwner co on co.ProspectID=p.ProspectID inner join t_Contract c on c.contractid = co.contractid where co.ContractID = f.contractid) end = 0 then 1 else 2 end  as money),1)), len(convert(varchar(50), round(cast ((case when (select top 1 Creditscore from t_Prospect p inner join t_Contract c on c.ProspectID=p.ProspectID where c.ContractID = f.contractid) IS null then 0 else (select top 1 Creditscore from t_Prospect p inner join t_Contract c on c.ProspectID=p.ProspectID where c.ContractID = f.contractid) end + case when (select top 1 case when co.prospectid = c.prospectid then p.SpouseCreditScore else CreditScore end  from t_Prospect p inner join t_ContractCoOwner co on co.ProspectID=p.ProspectID inner join t_Contract c on c.contractid = co.contractid where co.ContractID = f.contractid) is null then 0 else (select top 1 case when co.prospectid = c.prospectid then p.SpouseCreditScore else CreditScore end  from t_Prospect p inner join t_ContractCoOwner co on co.ProspectID=p.ProspectID inner join t_Contract c on c.contractid = co.contractid where co.ContractID = f.contractid) end) as money) / cast (case when case when (select top 1 Creditscore from t_Prospect p inner join t_Contract c on c.ProspectID=p.ProspectID where c.ContractID = f.contractid) IS null then 0 else (select top 1 Creditscore from t_Prospect p inner join t_Contract c on c.ProspectID=p.ProspectID where c.ContractID = f.contractid) end = 0 OR case when (select top 1 case when co.prospectid = c.prospectid then p.SpouseCreditScore else CreditScore end  from t_Prospect p inner join t_ContractCoOwner co on co.ProspectID=p.ProspectID inner join t_Contract c on c.contractid = co.contractid where co.ContractID = f.contractid) is null then 0 else (select top 1 case when co.prospectid = c.prospectid then p.SpouseCreditScore else CreditScore end  from t_Prospect p inner join t_ContractCoOwner co on co.ProspectID=p.ProspectID inner join t_Contract c on c.contractid = co.contractid where co.ContractID = f.contractid) end = 0 then 1 else 2 end  as money),1)))-1) as AVGFICO," &
                                "unitweek as [Unit/Wk],contractdate as [Contract Date], " &
                                " equitydp as [DP Equity], equityccmf as [CC Equity], " &
                                " equityexitdp as [DP Exit Equity],  owdate as [OW Date], MonthlyPaymentPlus, " &
                                " EOY, occupancyyear as Occupancy,Downpaymentccck as [Down Pymt CC/CK], closingcosts as [C Costs], " &
                                " cottagetownes as [Phase],bedrooms as [BR],IHLN as [IH/LN], " &
                                " TourCampaign as Tour, customer2 as CustomerName2, Intervals from t_Fundingitems f where fundingid = '" & fundingID & "' order by subtractionflag desc"
            'Removed equityexitcc as [CC Exit Equity], 3/10/16
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Remove_FundingItem(ByVal ID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_FundingItems where fundingItemID = '" & ID & "'"
            cm.ExecuteNonQuery()
        Catch ex As Exception
            bValid = False
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Function Funding_Report(ByVal fundingID As Integer, ByVal category As String) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            Select Case category
                Case "kcp"
                    ds.SelectCommand = "Select fundingitemid,fundingid,Case when SubtractionFlag = 0 then 'False' else 'True' end as SubtractionFlag,'N/A' as [NEW/OLD KCP],contractnumber as [KCP #], " &
                                       " lastname + ', ' + firstname as Owner, SalesPrice as [Sales Price],totaldown as [Total Down Payment], " &
                                       " DownPaymentCCCKType as [Down Payment Type], " &
                                       " ccfinanced as [Closing Cost Financed], totalfinanced as Financed, " &
                                       " equitydp as [Equity], Term,Rate,Season,unitweek as [Unit/Wk],contractdate as [Contract Date], " &
                                       " equitydp as [DP Equity], equityccmf as [CC Equity], " &
                                       " equityexitdp as [DP Exit Equity],  owdate as [OW Date], MonthlyPaymentPlus, " &
                                       " EOY, occupancyyear as Occupancy,Downpaymentccck as [Down Pymt CC/CK], closingcosts as [C Costs], " &
                                       " cottagetownes as [Phase],bedrooms as [BR],IHLN as [IH/LN], " &
                                       " TourCampaign as Tour, customer2 as CustomerName2, Intervals " &
                                       " from t_FundingItems where fundingid = '" & fundingID & "' and presale <> 1 order by subtractionflag desc, contractdate, contractnumber"
                    'Removed equityexitcc as [CC Exit Equity], 3-10-16
                Case "kcpps"
                    ds.SelectCommand = "Select fundingitemid,fundingid,Case when SubtractionFlag = 0 then 'False' else 'True' end as SubtractionFlag,'N/A' as [NEW/OLD KCP],contractnumber as [KCP #], " &
                                       " lastname + ', ' + firstname as Owner, SalesPrice as [Sales Price],totaldown as [Total Down Payment], " &
                                       " DownPaymentCCCKType as [Down Payment Type], " &
                                       " ccfinanced as [Closing Cost Financed], totalfinanced as Financed, " &
                                       " equitydp as [Equity], Term,Rate,Season,unitweek as [Unit/Wk],contractdate as [Contract Date], " &
                                       " equitydp as [DP Equity], equityccmf as [CC Equity], " &
                                       " equityexitdp as [DP Exit Equity], owdate as [OW Date], MonthlyPaymentPlus, " &
                                       " EOY, occupancyyear as Occupancy,Downpaymentccck as [Down Pymt CC/CK], closingcosts as [C Costs], " &
                                       " cottagetownes as [Phase],bedrooms as [BR],IHLN as [IH/LN], " &
                                       " TourCampaign as Tour, customer2 as CustomerName2, Intervals " &
                                       " from t_FundingItems where fundingid = '" & fundingID & "' and presale = 1 order by subtractionflag desc, contractdate, contractnumber"
                    'Removed equityexitcc as [CC Exit Equity], 3-10-16
                Case "wf"
                    ds.SelectCommand = "Select fundingitemid,fundingid,Case when SubtractionFlag = 0 then 'False' else 'True' end as SubtractionFlag,'Kings Creek' as DEVELOPER, " & _
                                       " 'N/A' as [LOAN FACILITY], (select name from t_Funding where fundingid = '" & fundingID & "') as [FUNDING #], " & _
                                       " contractnumber as [CONTRACT #], " & _
                                       " lastname AS [CUSTOMER LAST NAME], firstname AS [CUSTOMER FIRST NAME], customer2 AS [CUSTOMER NAME 2], " & _
                                       " SalesPrice as [PURCHASE PRICE],totaldown as [DOWN PAYMENT], " & _
                                       " totalfinanced as [ORIGINAL LOAN AMOUNT], " & _
                                       " TERM AS TERMS,Rate AS [INTEREST RATE], EOY AS [ANNUAL/ BIENNIAL], unitweek as [UNIT/WEEK],contractdate as [Date], " & _
                                       " MonthlyPaymentPlus, 'Kings Creek Plantation' as [RESORT OR TIMESHARE PLAN] " & _
                                       " from t_FundingItems where fundingid = '" & fundingID & "' and presale <> 1 and subtractionflag <> 1 order by subtractionflag desc, contractdate, contractnumber"

                Case "wfps"
                    ds.SelectCommand = "Select fundingitemid,fundingid,Case when SubtractionFlag = 0 then 'False' else 'True' end as SubtractionFlag,'Kings Creek' as DEVELOPER, " & _
                                       " 'N/A' as [LOAN FACILITY], (select name from t_Funding where fundingid = '" & fundingID & "') as [FUNDING #], " & _
                                       " contractnumber as [CONTRACT #], " & _
                                       " lastname AS [CUSTOMER LAST NAME], firstname AS [CUSTOMER FIRST NAME], customer2 AS [CUSTOMER NAME 2], " & _
                                       " SalesPrice as [PURCHASE PRICE],totaldown as [DOWN PAYMENT], " & _
                                       " totalfinanced as [ORIGINAL LOAN AMOUNT], " & _
                                       " TERM AS TERMS,Rate AS [INTEREST RATE], EOY AS [ANNUAL/ BIENNIAL], unitweek as [UNIT/WEEK],contractdate as [Date], " & _
                                       " MonthlyPaymentPlus, 'Kings Creek Plantation' as [RESORT OR TIMESHARE PLAN] " & _
                                       " from t_FundingItems where fundingid = '" & fundingID & "' and presale = 1 and subtractionflag <> 1 order by subtractionflag desc, contractdate, contractnumber"

                Case "atl"
                    ds.SelectCommand = "Select fundingitemid,fundingid,Case when SubtractionFlag = 0 then 'False' else 'True' end as SubtractionFlag, contractnumber as [#], " & _
                                       " lastname + ', ' + firstname AS [Exhibit 'A' Miscellaneous], SalesPrice as [Price],totaldown as [Down Payment], " & _
                                       " totalfinanced as [Note/Deed], " & _
                                       " unitweek as [UNIT/WEEK] " & _
                                       " from t_FundingItems where fundingid = " & fundingID & " and presale <> 1 order by subtractionflag desc, contractdate, contractnumber"

                Case "atlps"
                    ds.SelectCommand = "Select fundingitemid,fundingid,Case when SubtractionFlag = 0 then 'False' else 'True' end as SubtractionFlag, contractnumber as [#], " & _
                                       " lastname + ', ' + firstname AS [Exhibit 'A' Miscellaneous], SalesPrice as [Price],totaldown as [Down Payment], " & _
                                       " totalfinanced as [Note/Deed], " & _
                                       " unitweek as [UNIT/WEEK] " & _
                                       " from t_FundingItems where fundingid = " & fundingID & " and presale = 1 order by subtractionflag desc, contractdate, contractnumber"

            End Select
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Add_Deal(ByVal conNumber As String, ByVal cxl As String, ByVal preSales As String, ByVal fundingID As Integer) As Boolean
        Dim bAdded As Boolean = True
        'Try
        Dim oContract As New clsContract
        Dim cte As String = oContract.List_InventoryType(conNumber)
        Dim coOwners As String = oContract.List_CoOwners(conNumber)
        Dim oFundingItem As New clsFundingItems

        If cn.State <> ConnectionState.Open Then cn.Open()
        If cte = "" Then
            cm.CommandText = "select * from v_ContractInventoryHistory where contractnumber = '" & conNumber & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                cte = dread("SaleType") & ""
            End If
            dread.Close()
        End If

        oFundingItem.FundingItemID = 0
        oFundingItem.Load()
        oFundingItem.ContractNumber = conNumber
        oFundingItem.UnitWeek = oContract.Get_UnitWeek(conNumber)
        oFundingItem.ContractID = oContract.Get_Contract_ID(conNumber)
        oFundingItem.Customer2 = coOwners
        oFundingItem.DownPaymentCCCKType = oContract.Get_DP_Payment_Types(conNumber)
        oFundingItem.ClosingCostCCCKType = oContract.Get_CC_Payment_Types(conNumber)
        oFundingItem.CottageTownes = cte
        oFundingItem.FundingID = fundingID
        oFundingItem.DateCreated = System.DateTime.Now
        oFundingItem.CreatedByID = _UserID

        cm.CommandText = "select 	c.contractid, c.contractnumber, p.lastname ,p.firstname , p.CreditScore, " & _
                            "		 (select case when comboitem is null then '' else comboitem end from t_Comboitems where comboitemid = c.statusid ) as CancelType," & _
                            "			Case when m.commvolume is null then 0 else m.commvolume end as commvolume,  " & _
                            "			m.terms,  " & _
                            "			m.apr,  " & _
                            "			(  " & _
                            "				select case when i.comboitem is null then '' when i.comboitem = 'Red' then 'Plat' else 'Bronze' end from t_Contract c2 left outer join t_ComboItems i on c2.seasonid = i.comboitemid where c2.contractid = c.contractid  " & _
                            "			) as Season,  " & _
                            "			c.contractdate,  " & _
                            "			(select case   " & _
                            "				when i.comboitem is null then  " & _
                            "					'no'  " & _
                            "				when i.comboitem='Auto Debit' then  " & _
                            "					'Yes'  " & _
                            "				when i.comboitem='Mothly Bill' then  " & _
                            "					'No'  " & _
                            "				end as PaymentType from t_Mortgage m left outer join t_Comboitems i on i.comboitemid = m.paymenttypeid where m.contractid = c.contractid  " & _
                            "			) as Debit,  " & _
                            "			'N/A' as Resides, " & _
                            "			m.dptotal,  " & _
                            "			m.totalfinanced, " & _
                            "		 	m.paymentfee,  " & _
                            "			m.ccfinanced,  " & _
                            "           (select Case when sum(applied) is null then 0 else sum(applied) end from ( " & _
                            "               select case when pi.posneg = 0 then pi.amount else pi.amount * -1 end + case when a.amount is null then 0 else a.amount end as applied " & _
                            "               from t_Payments p  " & _
                            "       		inner join t_Payment2Invoice pi on pi.paymentid = p.paymentid " & _
                            "       		inner join t_Invoices i on i.invoiceid = pi.invoiceid " & _
                            "       		inner join t_FinTransCodes f on f.fintransid = i.fintransid " & _
                            "       		inner join t_Comboitems tc on tc.comboitemid = f.transcodeid " & _
                            "       		inner join t_Comboitems opm on opm.comboitemid = p.methodid " & _
                            "       		left outer join t_Comboitems pm on pm.comboitemid = p.methodid " & _
                            "       		left outer join ( " & _
                            "       			select sum(case when posneg = 0 then amount else amount * -1 end) as amount, applytoid " & _
                            "   				from t_Payments " & _
                            "   				where applytoid > 0 " & _
                            "   				group by applytoid " & _
                            "          		) a on a.applytoid = p.paymentid " & _
                            "       	where p.applytoid = 0 and tc.comboitem like '%closing cost%' and opm.comboitem = 'equity' and UPPER(i.keyfield) = 'MortgageDP' and i.Keyvalue = m.MortgageID " & _
                            "           ) a) as EquityCCMF,  " & _
                            "           (select Case when sum(applied) is null then 0 else sum(applied) end from ( " & _
                            "               select case when pi.posneg = 0 then pi.amount else pi.amount * -1 end + case when a.amount is null then 0 else a.amount end as applied " & _
                            "	            from t_Payments p " & _
                            "		        inner join t_Payment2Invoice pi on pi.paymentid = p.paymentid " & _
                            "		        inner join t_Invoices i on i.invoiceid = pi.invoiceid " & _
                            "		        inner join t_FinTransCodes f on f.fintransid = i.fintransid " & _
                            "		        inner join t_Comboitems tc on tc.comboitemid = f.transcodeid " & _
                            "		        inner join t_Comboitems opm on opm.comboitemid = p.methodid " & _
                            "		        left outer join t_Comboitems pm on pm.comboitemid = p.methodid " & _
                            "		        left outer join ( " & _
                            "				    select sum(case when posneg = 0 then amount else amount * -1 end) as amount, applytoid " & _
                            "                   from t_Payments " & _
                            "                   where applytoid > 0 " & _
                            "				    group by applytoid " & _
                            "		        ) a on a.applytoid = p.paymentid " & _
                            "	        where p.applytoid = 0 and tc.comboitem like '%down payment%' and opm.comboitem = 'equity' and UPPER(i.keyfield) = 'MortgageDP' and i.Keyvalue = m.MortgageID " & _
                            "           ) a) EquityDP,  " & _
                            "           (select Case when sum(applied) is null then 0 else sum(applied) end from ( " & _
                            "               select case when pi.posneg = 0 then pi.amount else pi.amount * -1 end + case when a.amount is null then 0 else a.amount end as applied " & _
                            "	            from t_Payments p " & _
                            "		        inner join t_Payment2Invoice pi on pi.paymentid = p.paymentid " & _
                            "		        inner join t_Invoices i on i.invoiceid = pi.invoiceid " & _
                            "		        inner join t_FinTransCodes f on f.fintransid = i.fintransid " & _
                            "		        inner join t_Comboitems tc on tc.comboitemid = f.transcodeid " & _
                            "		        inner join t_Comboitems opm on opm.comboitemid = p.methodid " & _
                            "		        left outer join t_Comboitems pm on pm.comboitemid = p.methodid " & _
                            "		        left outer join ( " & _
                            "				    select sum(case when posneg = 0 then amount else amount * -1 end) as amount, applytoid " & _
                            "                   from t_Payments " & _
                            "                   where applytoid > 0 " & _
                            "		            group by applytoid " & _
                            "               ) a on a.applytoid = p.paymentid " & _
                            "               where p.applytoid = 0 and tc.comboitem like '%closing cost%' and opm.comboitem = 'Exit Equity' and UPPER(i.keyfield) = 'MortgageDP' and i.Keyvalue = m.MortgageID " & _
                            "           ) a) as EquityExitCC,  " & _
                            "           (select Case when sum(applied) is null then 0 else sum(applied) end from ( " & _
                            "               select case when pi.posneg = 0 then pi.amount else pi.amount * -1 end + case when a.amount is null then 0 else a.amount end as applied " & _
                            "	            from t_Payments p " & _
                            "		        inner join t_Payment2Invoice pi on pi.paymentid = p.paymentid " & _
                            "		        inner join t_Invoices i on i.invoiceid = pi.invoiceid " & _
                            "		        inner join t_FinTransCodes f on f.fintransid = i.fintransid " & _
                            "		        inner join t_Comboitems tc on tc.comboitemid = f.transcodeid " & _
                            "		        inner join t_Comboitems opm on opm.comboitemid = p.methodid " & _
                            "		        left outer join t_Comboitems pm on pm.comboitemid = p.methodid " & _
                            "		        left outer join ( " & _
                            "				    select sum(case when posneg = 0 then amount else amount * -1 end) as amount, applytoid " & _
                            "                   from t_Payments " & _
                            "                   where applytoid > 0 " & _
                            "		            group by applytoid " & _
                            "               ) a on a.applytoid = p.paymentid " & _
                            "               where p.applytoid = 0 and tc.comboitem like '%down payment%' and opm.comboitem = 'Exit Equity' and UPPER(i.keyfield) = 'MortgageDP' and i.Keyvalue = m.MortgageID " & _
                            "           ) a) as EquityExitDP,  " & _
                            "			0 as DirectDeposit,  " & _
                            "			case when c.statusdate is null then '' else c.statusdate end as owdate,  " & _
                            "			(  " & _
                            "				select  case   " & _
                            "						when f.interval = 1 then  " & _
                            "							'Annual'  " & _
                            "						when f.interval = 2 then  " & _
                            "							case   " & _
                            "								when year(c2.occupancydate) % 2 = 0 then  " & _
                            "									'Biennial'  " & _
                            "								when year(c2.occupancydate) % 2 <> 0 then  " & _
                            "									'Biennial'  " & _
                            "							end   " & _
                            "						when f.interval = 3 then  " & _
                            "							'Triennial'  " & _
                            "					end from t_contract c2 left outer join t_Frequency f on f.frequencyid = c2.frequencyid where c2.contractid = c.contractid  " & _
                            "			) as EOY,  " & _
                            "			year(c.occupancydate) as OccYear,  " & _
                            "           (select sum(amount + adjustment) from v_Payments where invoice like '%down payment%' and method not like '%equity%' and keyfield = 'MortgageDP' and keyvalue = m.MortgageID " & _
                            "           ) as DownPaymentCCCKP,  " & _
                            "           (Select 0 ) as DownPaymentCCCKN, " & _
                            "			(  " & _
                            "				select m.cctotal from t_Mortgage m where m.contractid = c.contractid  " & _
                            "			) as ClosingCosts,  " & _
                            "			( " & _
                            "				select sum(cast(left(rt.comboitem,1) as int)) as rooms from t_Comboitems rt inner join t_Unit r on r.subtypeid = rt.comboitemid inner join t_salesinventory s on s.unitid = r.unitid inner join t_Soldinventory i on i.salesinventoryid = s.salesinventoryid where i.contractid = c.contractid " & _
                            "			) as rooms,  " & _
                            "			(  " & _
                            "				select case   " & _
                            "					when left(i.comboitem,8) = 'In House' or left(i.comboitem,8) = 'In-House' then  " & _
                            "						'IH'  " & _
                            "					when left(i.comboitem, 4) = 'Exit' then  " & _
                            "						'Exit'					" & _
                            "                   when i.comboitem like '%NOVA%' then " & _
                            "                       'NOVA' " & _
                            "					else   " & _
                            "						'LN' " & _
                            "					end from t_Contract c2 left outer join t_Comboitems i on i.comboitemid = c2.billingcodeid where c2.contractid = c.contractid  " & _
                            "			) as IHLN, " & _
                            "			(  select case when f.interval = 1 and cst.comboitem = '1 - Estates' then " & _
                            "								1.00/4.00 " & _
                            "							when f.interval = 2 and cst.comboitem = '1 - Estates' then " & _
                            "								1.00/8.00 " & _
                            "							when f.interval = 1 and cst.comboitem = '2 - Estates' then " & _
                            "								1.00/2.00 " & _
                            "							when f.interval = 2 and cst.comboitem = '2 - Estates' then " & _
                            "								1.00/4.00 " & _
                            "							when f.interval = 1 and cst.comboitem = '3 - Estates' then " & _
                            "								3.00/4.00 " & _
                            "							when f.interval = 2 and cst.comboitem = '3 - Estates' then " & _
                            "								3.00/8.00 " & _
                            "							when f.interval = 1 and cst.comboitem = '4 - Estates' then " & _
                            "								1.00 " & _
                            "							when f.interval = 2 and cst.comboitem = '4 - Estates' then " & _
                            "								1.00/2.00 " & _
                            "							when f.interval = 3 and cst.comboitem = '4 - Estates' then " & _
                            "								1.00/3.00 " & _
                            "							when f.interval = 1 and cst.comboitem = '2 - Townes' then " & _
                            "								1.00 " & _
                            "							when f.interval = 2 and cst.comboitem = '2 - Townes' then " & _
                            "								1.00/2.00 " & _
                            "							when f.interval = 1 and cst.comboitem = '4 - Townes' then " & _
                            "								2.00 " & _
                            "							when f.interval = 2 and cst.comboitem = '4 - Townes' then " & _
                            "								1.00 " & _
                            "							when f.interval = 1 and cst.comboitem = 'Cottage' then " & _
                            "								1.00 " & _
                            "							when f.interval = 2 and cst.comboitem = 'Cottage' then " & _
                            "								1.00/2.00 " & _
                            "							else " & _
                            "								0.00 " & _
                            "						end  " & _
                            "					from t_Contract c2 left outer join t_Frequency f on f.frequencyid = c2.frequencyid left outer join t_Comboitems cst on cst.comboitemid = c2.salesubtypeid where c2.contractid = c.contractid " & _
                            "			) as Intervals,  " & _
                            "			(  " & _
                            "				select case when name is null then '' else name end from t_Contract c2 left outer join t_Tour t on c2.tourid = t.tourid left outer join t_Campaign camp on t.campaignid = camp.campaignid where c2.contractid = c.contractid  " & _
                            "			) as Campaign,   " & _
                            "			m.salesprice,  " & _
                            "			(  " & _
                            "				select frequency from t_Frequency f inner join t_Contract c2 on c2.frequencyid = f.frequencyid where c2.contractid = c.contractid  " & _
                            "			) as frequency  " & _
                            "	from t_Contract c  " & _
                            "	inner join t_Mortgage m on m.contractid = c.contractid  " & _
                            "	inner join t_Prospect p on p.prospectid = c.prospectid where c.contractnumber = '" & conNumber & "'"
        dread = cm.ExecuteReader
        If dread.HasRows Then
            dread.Read()
            If Not (dread("LastName") Is System.DBNull.Value) Then oFundingItem.LastName = dread("LastName")
            If Not (dread("FirstName") Is System.DBNull.Value) Then oFundingItem.FirstName = dread("FirstName")
            If Not (dread("Terms") Is System.DBNull.Value) Then oFundingItem.Term = dread("Terms")
            If Not (dread("CreditScore") Is System.DBNull.Value) Then oFundingItem.Credit = dread("CreditScore")
            If Not (dread("APR") Is System.DBNull.Value) Then oFundingItem.Rate = dread("APR")
            If Not (dread("Season") Is System.DBNull.Value) Then oFundingItem.Season = dread("Season")
            If Not (dread("ContractDate") Is System.DBNull.Value) Then oFundingItem.ContractDate = dread("ContractDate")
            If Not (dread("Debit") Is System.DBNull.Value) Then oFundingItem.Debit = dread("Debit")
            If Not (dread("Resides") Is System.DBNull.Value) Then oFundingItem.Resides = dread("Resides")
            If Not (dread("DirectDeposit") Is System.DBNull.Value) Then oFundingItem.DirectDeposit = dread("DirectDeposit")
            If Not (dread("owdate") Is System.DBNull.Value) Then oFundingItem.OWDate = dread("owdate")
            If Not (dread("EOY") Is System.DBNull.Value) Then oFundingItem.EOY = dread("EOY")
            If Not (dread("OccYear") Is System.DBNull.Value) Then oFundingItem.OccupancyYear = dread("OccYear")
            If Not (dread("rooms") Is System.DBNull.Value) Then oFundingItem.Bedrooms = dread("rooms")
            If Not (dread("IHLN") Is System.DBNull.Value) Then oFundingItem.IHLN = dread("IHLN")
            If Not (dread("Campaign") Is System.DBNull.Value) Then oFundingItem.TourCampaign = dread("Campaign")
            If Not (dread("Frequency") Is System.DBNull.Value) Then oFundingItem.Frequency = dread("Frequency")
            If preSales = "True" Then
                oFundingItem.PreSale = True
            End If
            Dim oMortgage As New clsMortgage
            oMortgage.TotalFinanced = dread("TotalFinanced")
            oMortgage.APR = dread("APR")
            oMortgage.Terms = dread("Terms")
            If cxl = "True" Then
                Dim oEq As New clsEquiant
                Dim acctNum As String = oEq.Get_Account(oContract.Get_Contract_ID(conNumber), False)
                Dim info As clsEquiant.Loan = oEq.LoanInformation(acctNum)
                oFundingItem.CommissionVolume = dread("CommVolume") * -1
                oFundingItem.CancelType = dread("CancelType")
                oFundingItem.ActualDown = dread("dptotal") * -1
                oFundingItem.Payments = dread("TotalFinanced") * -1
                oFundingItem.EquityCCMF = dread("EquityCCMF") * -1
                oFundingItem.EquityDP = dread("EquityDP") * -1
                oFundingItem.EquityExitDP = dread("EquityExitDP") * -1
                oFundingItem.EquityExitCC = dread("EquityExitCC") * -1
                oFundingItem.MonthlyPaymentPlus = (oMortgage.PMT + dread("PaymentFee")) * -1
                If dread("DownPaymentCCCKN") & "" = "" Then
                    oFundingItem.DownPaymentCCCK = dread("DownPaymentCCCKP") * -1
                Else
                    If dread("DownPaymentCCCKP") & "" = "" Then
                        oFundingItem.DownPaymentCCCK = (0 - dread("DownPaymentCCCKN")) * -1
                    Else
                        oFundingItem.DownPaymentCCCK = (dread("DownPaymentCCCKP") - dread("DownPaymentCCCKN")) * -1
                    End If

                End If
                oFundingItem.ClosingCosts = dread("ClosingCosts") * -1
                oFundingItem.SalesPrice = dread("SalesPrice") * -1
                oFundingItem.TotalDown = dread("dpTotal") * -1
                If info Is Nothing Then
                    oFundingItem.TotalFinanced = 0 'dread("totalFinanced") * -1
                Else
                    oFundingItem.TotalFinanced = CDec(info.PrincipalBalance) * -1
                End If
                'oFundingItem.TotalFinanced = dread("totalFinanced") * -1
                oFundingItem.SubtractionFlag = True
                oFundingItem.Intervals = dread("Intervals") * -1
                oFundingItem.CCFinanced = dread("CCFinanced") * -1
                info = Nothing
                oEq = Nothing
            Else
                oFundingItem.CommissionVolume = dread("CommVolume")
                oFundingItem.ActualDown = dread("dptotal")
                oFundingItem.Payments = dread("TotalFinanced")
                oFundingItem.EquityCCMF = dread("EquityCCMF")
                oFundingItem.EquityDP = dread("EquityDP")
                oFundingItem.EquityExitDP = dread("EquityExitDP")
                oFundingItem.EquityExitCC = dread("EquityExitCC")
                oFundingItem.MonthlyPaymentPlus = (oMortgage.PMT + dread("PaymentFee"))
                If dread("DownPaymentCCCKN") & "" = "" Then
                    oFundingItem.DownPaymentCCCK = dread("DownPaymentCCCKP")
                Else
                    If dread("DownPaymentCCCKP") & "" = "" Then
                        oFundingItem.DownPaymentCCCK = (0 - dread("DownPaymentCCCKN")) * -1
                    Else
                        oFundingItem.DownPaymentCCCK = (dread("DownPaymentCCCKP") - dread("DownPaymentCCCKN"))
                    End If
                End If
                oFundingItem.ClosingCosts = dread("ClosingCosts")
                oFundingItem.SalesPrice = dread("SalesPrice")
                oFundingItem.TotalDown = dread("dpTotal")
                oFundingItem.TotalFinanced = dread("totalFinanced")
                oFundingItem.SubtractionFlag = False
                oFundingItem.Intervals = dread("Intervals")
                oFundingItem.CCFinanced = dread("CCFinanced")
            End If
            oMortgage = Nothing
        End If
            oFundingItem.Save()
            dread.Close()

            oFundingItem = Nothing
            oContract = Nothing
            'Catch ex As Exception
            'bAdded = False
            ' _Err = ex.Message
            'Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            ' End Try
            Return bAdded
    End Function
    Public Function Validate_Deal_On_Funding(ByVal conNum As String, ByVal fundingID As Integer) As Boolean
        Dim bProceed As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end As Deals from t_FundingItems where contractnumber = '" & conNum & "' and fundingID = '" & FundingID & "'"
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Deals") > 0 Then
                bProceed = False
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
            bProceed = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bProceed
    End Function
    Public Property FundingID() As Integer
        Get
            Return _FundingID
        End Get
        Set(ByVal value As Integer)
            _FundingID = value
        End Set
    End Property

    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property

    Public Property CreatedByID() As Integer
        Get
            Return _CreatedByID
        End Get
        Set(ByVal value As Integer)
            _CreatedByID = value
        End Set
    End Property

    Public Property ContractID() As Integer
        Get
            Return _ContractID
        End Get
        Set(ByVal value As Integer)
            _ContractID = value
        End Set
    End Property

    Public Property ContractNumber() As String
        Get
            Return _ContractNumber
        End Get
        Set(ByVal value As String)
            _ContractNumber = value
        End Set
    End Property

    Public Property LastName() As String
        Get
            Return _LastName
        End Get
        Set(ByVal value As String)
            _LastName = value
        End Set
    End Property

    Public Property FirstName() As String
        Get
            Return _FirstName
        End Get
        Set(ByVal value As String)
            _FirstName = value
        End Set
    End Property

    Public Property Customer2() As String
        Get
            Return _Customer2
        End Get
        Set(ByVal value As String)
            _Customer2 = value
        End Set
    End Property

    Public Property CommissionVolume() As Decimal
        Get
            Return _CommissionVolume
        End Get
        Set(ByVal value As Decimal)
            _CommissionVolume = value
        End Set
    End Property

    Public Property Credit() As String
        Get
            Return _Credit
        End Get
        Set(ByVal value As String)
            _Credit = value
        End Set
    End Property

    Public Property Term() As Integer
        Get
            Return _Term
        End Get
        Set(ByVal value As Integer)
            _Term = value
        End Set
    End Property

    Public Property Rate() As Decimal
        Get
            Return _Rate
        End Get
        Set(ByVal value As Decimal)
            _Rate = value
        End Set
    End Property

    Public Property Season() As String
        Get
            Return _Season
        End Get
        Set(ByVal value As String)
            _Season = value
        End Set
    End Property

    Public Property UnitWeek() As String
        Get
            Return _UnitWeek
        End Get
        Set(ByVal value As String)
            _UnitWeek = value
        End Set
    End Property

    Public Property ContractDate() As String
        Get
            Return _ContractDate
        End Get
        Set(ByVal value As String)
            _ContractDate = value
        End Set
    End Property

    Public Property Debit() As String
        Get
            Return _Debit
        End Get
        Set(ByVal value As String)
            _Debit = value
        End Set
    End Property

    Public Property Resides() As String
        Get
            Return _Resides
        End Get
        Set(ByVal value As String)
            _Resides = value
        End Set
    End Property

    Public Property CancelType() As String
        Get
            Return _CancelType
        End Get
        Set(ByVal value As String)
            _CancelType = value
        End Set
    End Property

    Public Property ActualDown() As Decimal
        Get
            Return _ActualDown
        End Get
        Set(ByVal value As Decimal)
            _ActualDown = value
        End Set
    End Property

    Public Property Payments() As Decimal
        Get
            Return _Payments
        End Get
        Set(ByVal value As Decimal)
            _Payments = value
        End Set
    End Property

    Public Property EquityCCMF() As Decimal
        Get
            Return _EquityCCMF
        End Get
        Set(ByVal value As Decimal)
            _EquityCCMF = value
        End Set
    End Property

    Public Property EquityDP() As Decimal
        Get
            Return _EquityDP
        End Get
        Set(ByVal value As Decimal)
            _EquityDP = value
        End Set
    End Property

    Public Property EquityExitDP() As Decimal
        Get
            Return _EquityExitDP
        End Get
        Set(ByVal value As Decimal)
            _EquityExitDP = value
        End Set
    End Property

    Public Property EquityExitCC() As Decimal
        Get
            Return _EquityExitCC
        End Get
        Set(ByVal value As Decimal)
            _EquityExitCC = value
        End Set
    End Property

    Public Property DirectDeposit() As Boolean
        Get
            Return _DirectDeposit
        End Get
        Set(ByVal value As Boolean)
            _DirectDeposit = value
        End Set
    End Property

    Public Property OWDate() As String
        Get
            Return _OWDate
        End Get
        Set(ByVal value As String)
            _OWDate = value
        End Set
    End Property

    Public Property MonthlyPaymentPlus() As Decimal
        Get
            Return _MonthlyPaymentPlus
        End Get
        Set(ByVal value As Decimal)
            _MonthlyPaymentPlus = value
        End Set
    End Property

    Public Property EOY() As String
        Get
            Return _EOY
        End Get
        Set(ByVal value As String)
            _EOY = value
        End Set
    End Property

    Public Property OccupancyYear() As Integer
        Get
            Return _OccupancyYear
        End Get
        Set(ByVal value As Integer)
            _OccupancyYear = value
        End Set
    End Property

    Public Property DownPaymentCCCK() As Decimal
        Get
            Return _DownPaymentCCCK
        End Get
        Set(ByVal value As Decimal)
            _DownPaymentCCCK = value
        End Set
    End Property

    Public Property ClosingCosts() As Decimal
        Get
            Return _ClosingCosts
        End Get
        Set(ByVal value As Decimal)
            _ClosingCosts = value
        End Set
    End Property

    Public Property CottageTownes() As String
        Get
            Return _CottageTownes
        End Get
        Set(ByVal value As String)
            _CottageTownes = value
        End Set
    End Property

    Public Property Bedrooms() As Integer
        Get
            Return _Bedrooms
        End Get
        Set(ByVal value As Integer)
            _Bedrooms = value
        End Set
    End Property

    Public Property IHLN() As String
        Get
            Return _IHLN
        End Get
        Set(ByVal value As String)
            _IHLN = value
        End Set
    End Property

    Public Property Intervals() As Decimal
        Get
            Return _Intervals
        End Get
        Set(ByVal value As Decimal)
            _Intervals = value
        End Set
    End Property

    Public Property TourCampaign() As String
        Get
            Return _TourCampaign
        End Get
        Set(ByVal value As String)
            _TourCampaign = value
        End Set
    End Property

    Public Property PreSale() As Boolean
        Get
            Return _PreSale
        End Get
        Set(ByVal value As Boolean)
            _PreSale = value
        End Set
    End Property

    Public Property SalesPrice() As Decimal
        Get
            Return _SalesPrice
        End Get
        Set(ByVal value As Decimal)
            _SalesPrice = value
        End Set
    End Property

    Public Property TotalDown() As Decimal
        Get
            Return _TotalDown
        End Get
        Set(ByVal value As Decimal)
            _TotalDown = value
        End Set
    End Property

    Public Property TotalFinanced() As Decimal
        Get
            Return _TotalFinanced
        End Get
        Set(ByVal value As Decimal)
            _TotalFinanced = value
        End Set
    End Property

    Public Property Frequency() As String
        Get
            Return _Frequency
        End Get
        Set(ByVal value As String)
            _Frequency = value
        End Set
    End Property

    Public Property DeedBook() As String
        Get
            Return _DeedBook
        End Get
        Set(ByVal value As String)
            _DeedBook = value
        End Set
    End Property

    Public Property Page() As String
        Get
            Return _Page
        End Get
        Set(ByVal value As String)
            _Page = value
        End Set
    End Property

    Public Property SubtractionFlag() As Boolean
        Get
            Return _SubtractionFlag
        End Get
        Set(ByVal value As Boolean)
            _SubtractionFlag = value
        End Set
    End Property

    Public Property DownPaymentCCCKType() As String
        Get
            Return _DownPaymentCCCKType
        End Get
        Set(ByVal value As String)
            _DownPaymentCCCKType = value
        End Set
    End Property

    Public Property ClosingCostCCCKType() As String
        Get
            Return _ClosingCostCCCKType
        End Get
        Set(ByVal value As String)
            _ClosingCostCCCKType = value
        End Set
    End Property

    Public Property CCFinanced() As Decimal
        Get
            Return _CCFinanced
        End Get
        Set(ByVal value As Decimal)
            _CCFinanced = value
        End Set
    End Property

    Public Property CRMSID() As Integer
        Get
            Return _CRMSID
        End Get
        Set(ByVal value As Integer)
            _CRMSID = value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property FundingItemID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public ReadOnly Property Err() As String
        Get
            Return _Err
        End Get
    End Property
End Class
