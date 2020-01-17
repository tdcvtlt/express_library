Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class Select_Item
    Inherits System.Web.UI.UserControl

    Dim _ComboItem As String = ""
    Dim _ComboItemID As Integer = 0
    Dim _CNS As String = ""
    Dim _Label_Caption As String = ""
    Dim _SelectedName As String = ""
    Public Event Index_Changed()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _CNS = IIf(_CNS = "", Resources.Resource.cns, _CNS)
        Label1.Visible = IIf(_Label_Caption = "", True, False)
        If Not (IsPostBack) Then

        End If
    End Sub

    Public Property Read_Only As Boolean
        Get
            Return DropDownList1.Enabled
        End Get
        Set(ByVal value As Boolean)
            DropDownList1.Enabled = Not (value)
        End Set
    End Property

    Public Property Label_Caption() As String
        Get
            Return _Label_Caption
        End Get
        Set(ByVal value As String)
            _Label_Caption = value
            Label1.Text = _Label_Caption
        End Set
    End Property

    Public Property Connection_String() As String
        Get
            Return _CNS
        End Get
        Set(ByVal value As String)
            _CNS = value
        End Set
    End Property

    Public Property ComboItem() As String
        Get
            Return _ComboItem
        End Get
        Set(ByVal value As String)
            _ComboItem = value
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
            _ComboItemID = value
            Change_Index()
        End Set
    End Property

    Public Function Load_Items() As Boolean
        If _ComboItem <> "" And _CNS <> "" Then
            Dim cn As New SqlConnection(_CNS)
            Dim cm As New SqlCommand("Select 0 as ComboItemID, '(empty)' as Comboitem union Select i.ComboItemID, i.Comboitem from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where (i.active=1 or i.comboitemid = " & _ComboItemID & ") and c.ComboName = '" & _ComboItem & "' order by comboitem", cn)
            Dim dr As SqlDataReader
            Try
                cn.Open()
                dr = cm.ExecuteReader
                DropDownList1.DataSource = dr
                DropDownList1.DataValueField = "ComboitemID"
                DropDownList1.DataTextField = "Comboitem"

                DropDownList1.DataBind()
                Change_Index()
                cn.Close()
                Return True
            Catch ex As Exception
                lblErr.Text = ex.ToString
                Return False
            Finally
                If cn.State <> ConnectionState.Closed Then cn.Close()
                cn = Nothing
                cm = Nothing
                dr = Nothing
            End Try
        Else
            Return False
        End If
    End Function

    Public Sub Select_Item(ByVal sItem As String)
        For i = 0 To DropDownList1.Items.Count - 1
            If DropDownList1.Items(i).Text = sItem Then
                DropDownList1.SelectedIndex = i
                Exit For
            End If
        Next
        _ComboItemID = DropDownList1.SelectedValue
        _SelectedName = DropDownList1.SelectedItem.Text
    End Sub

    Private Sub Change_Index()
        For i = 0 To DropDownList1.Items.Count - 1
            If DropDownList1.Items(i).Value = CStr(_ComboItemID) Then
                DropDownList1.SelectedIndex = i
                Exit For
            End If
        Next
        If DropDownList1.Items.Count > 1 Then
            _ComboItemID = DropDownList1.SelectedValue
            _SelectedName = DropDownList1.SelectedItem.Text
        End If
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        _ComboItemID = DropDownList1.SelectedItem.Value
        _SelectedName = DropDownList1.SelectedItem.Text
        RaiseEvent Index_Changed()
    End Sub
End Class
