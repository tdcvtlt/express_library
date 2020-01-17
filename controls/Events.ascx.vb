
Partial Class controls_Events
    Inherits System.Web.UI.UserControl
    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0

    Public Property KeyField() As String
        Get
            Return _KeyField
        End Get
        Set(ByVal value As String)
            _KeyField = value
            hfKeyField.value = _KeyField
        End Set
    End Property

    Public Property KeyValue() As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
            hfKeyValue.value = _KeyValue
        End Set
    End Property

    Public Sub List()
        Dim Events As New clsEvents
        Events.KeyField = _KeyField
        Events.KeyValue = _KeyValue
        gvEvents.DataSource = Events.List
        Dim sArr(0) As String
        sArr(0) = "EventID"
        gvEvents.DataKeyNames = sArr
        gvEvents.DataBind()
        lblError.Text = Events.Error_Message
        Events = Nothing
    End Sub
End Class
