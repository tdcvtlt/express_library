
Partial Class general_EditPoints
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request("ID") <> "" And Request("ID") <> 0 And Request("ID") <> "0" And Not (IsPostBack) Then
            Dim o As New clsPointsTracking
            Dim p As New clsPersonnel
            ddTransType.DataSource = (New clsComboItems).Load_ComboItems("PointsTransactionType")
            ddTransType.DataTextField = "Comboitem"
            ddTransType.DataValueField = "comboitemid"
            ddTransType.DataBind()
            o.ID = Request("ID")
            o.Load()
            txtID.Text = o.ID
            txtPoints.Text = o.Points
            txtExp.Text = o.ExpirationDate
            ddTransType.SelectedValue = o.TransTypeID
            p.PersonnelID = o.CreatedByID
            p.Load()
            txtCreatedBy.Text = p.UserName
            txtTransDate.Text = o.TransDate
            txtComments.Text = o.Comments
            txtConf.Text = o.ConfirmationNumber
            Fill_Usage_Years(o.ContractID, ddUsageYear)
            ddUsageYear.SelectedValue = o.UsageYear
            ddAvailYear.SelectedValue = o.AvailYear
            If o.PosNeg Then
                rblPosNeg.SelectedIndex = 1
            Else
                rblPosNeg.SelectedIndex = 0
            End If
            If Not (o.TransDate = "") Then
                btnSave.Enabled = IIf(CheckSecurity("PointsTracking", "Edit", , , Session("UserDBID")) Or (o.UserID = Session("UserDBID") And Math.Abs(DateDiff(DateInterval.Hour, CDate(o.TransDate), Date.Now)) < 24), True, False)
            Else
                btnSave.Enabled = False
            End If
            o = Nothing
            p = Nothing
        End If
    End Sub

    Private Sub Fill_Usage_Years(contractid As Integer, ByRef ddYear As DropDownList)
        Dim con As New clsContract
        Dim freq As New clsFrequency
        con.ContractID = contractid
        con.Load()
        freq.FrequencyID = con.FrequencyID
        freq.Load()
        Dim year As Integer = DatePart(DateInterval.Year, Date.Now)
        If con.OccupancyDate <> "" Then
            year = DatePart(DateInterval.Year, CDate(con.OccupancyDate))
        End If
        ddYear.Items.Clear()
        If freq.Interval > 0 Then
            While ddYear.Items.Count < 10 Or year + (freq.Interval * ddYear.Items.Count) < DatePart(DateInterval.Year, Date.Now) + 3
                ddYear.Items.Add(year + (freq.Interval * ddYear.Items.Count))
            End While
            Sort_DropDown(ddYear)
        Else
            ddYear.Items.Add(year)
        End If
        con = Nothing
        freq = Nothing
        ddAvailYear.Items.Clear()
        For i = Date.Today.AddYears(2).Year To Date.Today.AddYears(-5).Year Step -1
            ddAvailYear.Items.Add(i)
        Next
    End Sub

    Private Sub Sort_DropDown(ByRef ddSort As DropDownList)
        Dim ar As ListItem()
        Dim i As Long = 0
        For Each li As ListItem In ddSort.Items
            ReDim Preserve ar(i)
            ar(i) = li
            i += 1
        Next
        Dim ar1 As Array = ar

        ar1.Sort(ar1, New ListItemComparer)
        ddSort.Items.Clear()
        ddSort.Items.AddRange(ar1)
    End Sub

    Private Class ListItemComparer
        Implements IComparer
        Public Function Compare(ByVal x As Object, _
              ByVal y As Object) As Integer _
              Implements System.Collections.IComparer.Compare
            Dim a As ListItem = x
            Dim b As ListItem = y
            Dim c As New CaseInsensitiveComparer
            Return c.Compare(b.Text, a.Text)
        End Function
    End Class

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim o As New clsPointsTracking
        o.ID = txtID.Text
        o.Load()
        o.Points = txtPoints.Text
        o.ConfirmationNumber = txtConf.Text
        o.Comments = txtComments.Text

        o.Save()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
        'Response.Write(o.Err)
    End Sub
End Class
