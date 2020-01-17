
Partial Class controls_UserFields
    Inherits System.Web.UI.UserControl
    Private _sKeyField As String = "" 'Table Name
    Private _iKeyValue As Integer = 0 'Record
    Private _UserID As Integer = 0

    Public Property KeyField() As String
        Get
            Return _sKeyField
        End Get
        Set(ByVal value As String)
            _sKeyField = value
            hfUFTable.Value = _sKeyField
            'Update_Add_Link()
        End Set
    End Property

    Public Property KeyValue() As Integer
        Get
            Return _iKeyValue
        End Get
        Set(ByVal value As Integer)
            _iKeyValue = value
            hfKeyValue.Value = _iKeyValue
            'Update_Add_Link()
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

    Public Sub Load_List()
        Dim oUF As New clsUserFields
        oUF.UserID = _UserID
        oUF.KeyValue = _iKeyValue
        oUF.TableName = _sKeyField
        gvUserFields.DataSource = oUF.List
        gvUserFields.DataBind()
        oUF = Nothing
    End Sub

    'Private Sub Update_Add_Link()
    '    hlAdd.NavigateUrl = "javascript:modal.mwindow.open('" & Request.ApplicationPath & "/general/edituserfield.aspx?id=0&UFID=0&KeyValue=" & _iKeyValue & "&UFTable=" & _sKeyField & "','New',350,350);"
    'End Sub

    Protected Sub lbRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRefresh.Click
        _iKeyValue = hfKeyValue.Value
        _sKeyField = hfUFTable.Value
        Load_List()
    End Sub
End Class
