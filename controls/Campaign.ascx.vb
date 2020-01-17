Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class Campaign
    Inherits System.Web.UI.UserControl

    Dim _CampaignName As String = ""
    Dim _CampaignID As Integer = 0
    
    Public Event Index_Changed()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Public Property CampaignName() As String
        Get
            Return DropDownList1.SelectedItem.Text '_CampaignName
        End Get
        Set(ByVal value As String)
            _CampaignName = value
        End Set
    End Property

    Public ReadOnly Property SelectedName() As String
        Get
            Return DropDownList1.SelectedItem.Text
        End Get
    End Property

    Public Property Selected_ID() As Integer
        Get
            Return DropDownList1.SelectedValue
        End Get
        Set(ByVal value As Integer)
            _CampaignID = value
            Change_Index()
        End Set
    End Property

    Public Sub Load_Items()
        Dim oCamp As New clsCampaign
        oCamp.CampaignID = _CampaignID
        DropDownList1.DataSource = oCamp.List_Lookup
        DropDownList1.DataTextField = "Name"
        DropDownList1.DataValueField = "CampaignID"
        DropDownList1.AppendDataBoundItems = True
        DropDownList1.DataBind()
        Change_Index()
        lblErr.Text = oCamp.error_message
        oCamp = Nothing
    End Sub

    Private Sub Change_Index()
        For i = 0 To DropDownList1.Items.Count - 1
            If DropDownList1.Items(i).Value = _CampaignID Then
                DropDownList1.SelectedIndex = i
                Exit For
            End If
        Next
        If DropDownList1.Items.Count > 1 Then
            _CampaignID = DropDownList1.SelectedValue
            _CampaignName = DropDownList1.SelectedItem.Text
        End If
    End Sub


    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        _CampaignID = DropDownList1.SelectedItem.Value
        _CampaignName = DropDownList1.SelectedItem.Text
        RaiseEvent Index_Changed()
    End Sub
End Class
