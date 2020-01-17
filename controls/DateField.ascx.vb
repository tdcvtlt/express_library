
Partial Class controls_DateField
    Inherits System.Web.UI.UserControl
    Dim _Date As String = ""
    Dim _PostBack As String = ""
    Dim _sDate As Integer = 1910
    Public Event Date_Updated()

    Public Property oPostBack As String
        Get
            Return _PostBack
        End Get
        Set(ByVal value As String)
            _PostBack = value
            hfPostBack.Value = _PostBack
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            hfPostBack.Value = _PostBack
            For i = _sDate To Year(Date.Today) + 10
                Dim lItem As New ListItem
                lItem.Value = i
                lItem.Text = i
                ddYear.Items.Add(lItem)
            Next
            lbToday.Text = "Today: " & Month(Date.Today).ToString & "-" & Day(Date.Today).ToString & "-" & Year(Date.Today).ToString
            Set_DropDowns()
        Else
            _PostBack = hfPostBack.Value
        End If
    End Sub

    Public Property sDate() As Integer
        Get
            Return _sDate
        End Get
        Set(ByVal value As Integer)
            _sDate = value
        End Set
    End Property

    Public Property Selected_Date() As String
        Get
            Return txtDate.Text
        End Get
        Set(ByVal value As String)
            _Date = value
            txtDate.Text = _Date
            Update_Date()
        End Set
    End Property

    Private Sub Update_Date()
        If _Date = "" Then
            txtDate.Text = ""
        ElseIf IsDate(_Date) Then
            txtDate.Text = _Date
            Calendar1.SelectedDate = _Date
            Calendar1.VisibleDate = _Date
            RaiseEvent Date_Updated()
        End If
    End Sub

    Protected Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDate.TextChanged

        RaiseEvent Date_Updated()
    End Sub

    Protected Sub btnSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        Panel1.Visible = Not (Panel1.Visible)
        If IsDate(txtDate.Text) Then
            Calendar1.SelectedDate = CDate(txtDate.Text)
        Else
            Calendar1.SelectedDate = Date.Today
        End If
        txtDate.Text = Calendar1.SelectedDate
        Set_DropDowns()
        If _PostBack <> "" Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "PB", "__doPostBack('" & _PostBack & "','');", True)
        End If
        RaiseEvent Date_Updated()
    End Sub

    Protected Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
        txtDate.Text = Calendar1.SelectedDate.ToShortDateString
        Panel1.Visible = False
        If _PostBack <> "" Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "PB", "__doPostBack('" & _PostBack & "','');", True)
        End If
        RaiseEvent Date_Updated()
    End Sub

    Private Sub Set_DropDowns()
        If txtDate.Text <> "" Or IsDate(Calendar1.SelectedDate) Then
            Dim dDate As Date
            If IsDate(txtDate.Text) Then
                dDate = Convert.ToDateTime(txtDate.Text)
            Else
                dDate = Convert.ToDateTime(Calendar1.SelectedDate)
            End If
            For i = 0 To ddMonth.Items.Count - 1
                If CInt(ddMonth.Items(i).Value) = Month(dDate) Then
                    ddMonth.SelectedIndex = i
                    Exit For
                End If
            Next
            For i = 0 To ddYear.Items.Count - 1
                If ddYear.Items(i).Value = Year(dDate).ToString Then
                    ddYear.SelectedIndex = i
                    Exit For
                End If
            Next
        End If

    End Sub

    Protected Sub ddYearSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Set_Cal()
    End Sub

    Protected Sub ddMonthSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Set_Cal()
    End Sub

    Private Sub Set_Cal()
        Dim sYear As Int32 = DateTime.Now.Year
        Dim sMonth As Int32 = DateTime.Now.Month
        Dim sDay As Int32 = DateTime.Now.Day
        If ddMonth.SelectedValue <> "00" Then
            sMonth = Convert.ToInt32(ddMonth.SelectedValue)
        End If
        If ddYear.SelectedValue <> "*Year*" Then
            sYear = Convert.ToInt32(ddYear.SelectedValue)
        End If
        Dim dDate As Date = Convert.ToDateTime(IIf(IsDate(sMonth & "/" & DateTime.Now.Day & "/" & sYear), sMonth & "/" & DateTime.Now.Day & "/" & sYear, sMonth & "/1/" & sYear))
        Calendar1.VisibleDate = dDate
    End Sub

    Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
        If ddMonth.SelectedIndex <> ddMonth.Items.Count - 1 Then
            ddMonth.SelectedIndex += 1
            Set_Cal()
        ElseIf ddYear.SelectedIndex < ddYear.Items.Count - 1 Then
            ddYear.SelectedIndex += 1
            ddMonth.SelectedIndex = 0
            Set_Cal()
        End If
    End Sub

    Protected Sub btnPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrev.Click
        If ddMonth.SelectedIndex > 0 Then
            ddMonth.SelectedIndex -= 1
            Set_Cal()
        ElseIf ddYear.SelectedIndex > 0 Then
            ddYear.SelectedIndex -= 1
            ddMonth.SelectedIndex = ddMonth.Items.Count - 1
            Set_Cal()
        End If
    End Sub

    Protected Sub lbToday_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbToday.Click
        txtDate.Text = Date.Today.ToShortDateString
        Panel1.Visible = False
        If _PostBack <> "" Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "PB", "__doPostBack('" & _PostBack & "','');", True)
        End If
        RaiseEvent Date_Updated()
    End Sub

    Protected Sub lbClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbClear.Click
        txtDate.Text = ""
        Panel1.Visible = False
        If _PostBack <> "" Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "PB", "__doPostBack('" & _PostBack & "','');", True)
        End If
        RaiseEvent Date_Updated()
    End Sub
End Class
