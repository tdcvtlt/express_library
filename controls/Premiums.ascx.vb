
Partial Class controls_Premiums
    Inherits System.Web.UI.UserControl
    Private _KeyField As String = ""
    Private _KeyValue As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub Display()

        Dim oPrem As New clsPremium
        oPrem.KeyField = _KeyField
        oPrem.KeyValue = _KeyValue
        gvPremiums.DataSource = oPrem.List_Issued
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvPremiums.DataKeyNames = sKeys
        gvPremiums.DataBind()
        lblErr.Text = oPrem.Error_Message
        oPrem = Nothing

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
            hfKeyValue.value = _KeyValue
        End Set
    End Property

    Protected Sub lbRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRefresh.Click
        If _KeyField = "" Then _KeyField = hfKeyField.Value
        If _KeyValue = 0 Then _KeyValue = hfkeyvalue.value
        Display()
    End Sub

    Protected Sub gvPremiums_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                If e.Row.Cells(7).Text = "Do Not Issue" Then
                    e.Row.Visible = False
                End If
            End If
        End If
    End Sub
End Class
