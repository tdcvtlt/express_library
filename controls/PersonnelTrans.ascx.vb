
Partial Class controls_PersonnelTrans
    Inherits System.Web.UI.UserControl
    Private _KeyField As String = ""
    Private _KeyValue As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Load_Trans()
        End If
    End Sub

    Public Property KeyField As String
        Get
            Return _KeyField
        End Get
        Set(ByVal value As String)
            _KeyField = value
            hfKeyField.Value = _KeyField
        End Set
    End Property

    Public Property KeyValue As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
            hfKeyValue.Value = _KeyValue
        End Set
    End Property

    Public Sub Load_Trans()
        Dim oPT As New clsPersonnelTrans
        oPT.KeyField = _KeyField
        oPT.KeyValue = _KeyValue
        gvPersonnelTrans.DataSource = oPT.List
        gvPersonnelTrans.DataBind()
        lblError.Text = oPT.Error_Message
    End Sub

    Protected Sub lbRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRefresh.Click
        _KeyField = hfKeyField.Value
        _KeyValue = hfKeyValue.Value
        Load_Trans()
    End Sub
End Class
