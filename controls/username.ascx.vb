
Partial Class controls_username
    Inherits System.Web.UI.UserControl
    Dim oUser As New clsPersonnel
    Dim _UserID As Integer = 0
    Dim _UserName As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub Lookup_User()
        If _UserID = 0 Then Exit Sub
        oUser.Lookup_User(_UserID)
        _UserName = oUser.UserName
        txtUserName.Text = _UserName
    End Sub

    Public Property UserID As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
            Lookup_User()
        End Set
    End Property

    Public ReadOnly Property UserName As String
        Get
            Return _UserName
        End Get
    End Property
End Class
