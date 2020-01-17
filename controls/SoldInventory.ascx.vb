
Partial Class controls_SoldInventory
    Inherits System.Web.UI.UserControl

    Private _KeyField As String = "ContractID"
    Private _KeyValue As Integer = 0
    Private _Err As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub Display()
        'If _KeyField <> "" Then
        Try
            Dim SoldInventory As New clsSalesInventory2ContractHist
            SoldInventory.ContractID = _KeyValue
            gvSoldInventory.DataSource = SoldInventory.List()
            Dim sArr(0) As String
            sArr(0) = "ID"
            gvSoldInventory.DataKeyNames = sArr
            gvSoldInventory.DataBind()

            SoldInventory = Nothing
        Catch ex As Exception
            _Err = ex.ToString
            lblErr.Text = _Err
        End Try

        ' End If
    End Sub

    Public Property ContractID() As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
            hfKeyValue.Value = _KeyValue
        End Set
    End Property

    Public ReadOnly Property Error_Message() As String
        Get
            Return _Err
        End Get
    End Property

    Protected Sub lbRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRefresh.Click
        _KeyValue = hfKeyValue.Value
        Display()
    End Sub


End Class
