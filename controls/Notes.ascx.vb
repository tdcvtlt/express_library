
Partial Class controls_Notes
    Inherits System.Web.UI.UserControl

    Private _KeyField As String = ""
    Private _KeyValue As Integer = 0
    Private _Err As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub Display()
        'If _KeyField <> "" Then
        Try
            Dim notes As New clsNotes
            notes.KeyField = _KeyField
            notes.KeyValue = _KeyValue
            gvNotes.DataSource = notes.List
            Dim sArr(0) As String
            sArr(0) = "NoteID"
            gvNotes.DataKeyNames = sArr
            gvNotes.DataBind()
            
            notes = Nothing
        Catch ex As Exception
            _Err = ex.ToString
        End Try

        ' End If
    End Sub

    Public Property KeyField() As String
        Get
            Return _KeyField
        End Get
        Set(ByVal value As String)
            _KeyField = value
            hfKeyField.Value = _KeyField
        End Set
    End Property

    Public Property KeyValue() As Integer
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
        _KeyField = hfKeyField.Value
        _KeyValue = hfKeyValue.Value
        Display()
    End Sub

    Protected Sub gvNotes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvNotes.RowDataBound
        If _KeyField = "Comments" Then
            e.Row.Cells(0).Visible = False
        End If
    End Sub
End Class
