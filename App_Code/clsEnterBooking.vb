Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsEnterBooking

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
    End Sub

#Region "Fields & properties"
    Private _vendorID As Int32    
    Private _address As clsAddress
    Private _phoneID As Int32

    Private cn As SqlConnection
    Private cm As SqlCommand
    Private da As SqlDataAdapter
    Private ds As DataSet
    
    Public Property VendorID As Int32
        Get
            Return _vendorID
        End Get
        Set(value As Int32)
            _vendorID = value
        End Set
    End Property

    Public Property PhoneID As Int32
        Get
            Return _phoneID
        End Get
        Set(value As Int32)
            _phoneID = value
        End Set
    End Property
    Public ReadOnly Property Campaigns As SqlDataSource
        Get
            Return New SqlDataSource(cn.ConnectionString, String.Format("select vc.VendorID, c.Name, vc.CRMSCampID, vc.CampaignID, convert(varchar(50), vc.campaignid) + ';' + convert(varchar(50), crmscampid) [Expression1]  from t_VendorCampaigns vc " & _
                                    "inner join t_Campaign c on c.CampaignID = vc.CRMSCampID where vc.VendorID = {0} and c.Active = 1 and vc.Active = 1 order by name", VendorID))
        End Get
    End Property

    Public ReadOnly Property TourLocations As SqlDataSource
        Get
            Return New SqlDataSource(cn.ConnectionString, String.Format("select ci.comboitemid, ci.comboitem from t_vendor2tourloc vtl inner join t_comboitems ci on vtl.tourlocid = ci.comboitemid where vtl.active=1 and ci.active=1 and vtl.vendorid = {0}", VendorID))
        End Get
    End Property

    Public ReadOnly Property BookingLocations As SqlDataSource
        Get
            Return New SqlDataSource(cn.ConnectionString, String.Format("select * from t_vendorsaleslocations where vendorid = {0} and active =1 order by location", VendorID))
        End Get
    End Property

    Public ReadOnly Property TourTimes As SqlDataSource
        Get
            Return New SqlDataSource(cn.ConnectionString, String.Format("select ci.comboitemid, ci.description from t_combos co inner join t_comboitems ci on " & _
                                        "co.comboid = ci.comboid where co.comboname = 'tourtime' and active = 1 order by convert(int, comboitem)"))
        End Get
    End Property

    Public ReadOnly Property OPCReps As SqlDataSource
        Get
            Return New SqlDataSource(cn.ConnectionString, String.Format("select p.personnelid, p.firstname + ' ' + p.lastname [rep] from t_vendor2personnel v2p inner join t_personnel p on v2p.personnelid = p.personnelid where vendorid = {0} and p.active=1 order by p.firstname, p.lastname", VendorID))
        End Get
    End Property

    Public Property Address As clsAddress
        Get
            Return _address
        End Get
        Set(value As clsAddress)
            _address = value
        End Set
    End Property


#End Region

  
#Region "Functions"
    Public Function GetPremiumsByCampaign(campaignID As Int32) As SqlDataSource
        Return New SqlDataSource(cn.ConnectionString, String.Format("select * from t_vendorpremium2campaign v2c inner join t_premium p on v2c.premiumid = p.premiumid " & _
                "where campaignid in (select  vc.CampaignId  from t_VendorCampaigns vc inner join t_Campaign c on c.CampaignID = vc.CRMSCampID " & _
                "where vc.VendorID = {0} and c.Active = 1 and vc.Active = 1) and campaignID = {1} order by p.premiumName", VendorID, campaignID))
    End Function

    Public Function IsOwner(prospectID As Int32) As Boolean
        With New clsContract
            .Load()
            Dim ds = .List_Pros_Contracts(prospectID)
            Dim dv = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Return IIf(dv.Table.Rows.Count > 0, True, False)
        End With
    End Function

    Public Function Count(prospectID As Int32) As Integer
        With New clsContract
            .Load()
            Dim ds = .List_Pros_Contracts(prospectID)
            Dim dv = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Return dv.Table.Rows.Count
        End With
    End Function

    Public Function SearchByPhone(number As String) As DataRow
        Dim row As DataRow = Nothing
        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = New SqlCommand(String.Format("select top 1 * from t_prospectphone  where number = '{0}' and active =1 order by phoneid desc", number.Trim()), cn)
                Try
                    cn.Open()
                    Dim dt = New DataTable()
                    dt.Load(cm.ExecuteReader(CommandBehavior.CloseConnection))
                    If dt.Rows.Count = 1 Then
                        row = dt.Rows(0)
                    End If
                Catch ex As Exception
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        Return row
    End Function

    Public Function SearchByLeadID(leadID As Int32) As Integer
        Dim lead_id = 0
        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = New SqlCommand(String.Format("select leadid from t_leads where leadid={0}", leadID), cn)
                Try
                    cn.Open()
                    Dim dt = New DataTable()
                    dt.Load(cm.ExecuteReader(CommandBehavior.CloseConnection))
                    If dt.Rows.Count = 1 Then lead_id = dt.Rows(0)(0)
                Catch ex As Exception
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        Return lead_id
    End Function
#End Region

End Class
