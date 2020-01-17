
Partial Class controls_PointsTracking
    Inherits System.Web.UI.UserControl
    Dim _ProspectID As Integer = 0


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If _ProspectID > 0 And Not (IsPostBack) Then
            Display()
        ElseIf IsPostBack Then
            _ProspectID = hfProsID.Value
        End If

    End Sub

    Public Property ProspectID As Integer
        Get
            Return _ProspectID
        End Get
        Set(value As Integer)
            _ProspectID = value
            hfProsID.Value = _ProspectID
        End Set
    End Property

    Public Sub Display()
        If _ProspectID > 0 Then
            Fill_Contracts()
            Fill_Years()
            MultiView1.ActiveViewIndex = 0
            Load_View()
            Load_Contracts()
        End If
    End Sub

    Protected Sub gv_PointAdjustments()

    End Sub

    Private Sub Fill_Contracts()
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select 0 as ID, 'ALL' as Contractnumber union Select ContractID as ID, ContractNumber from t_Contract c inner join t_Comboitems cst on cst.comboitemid = c.subtypeid where cst.comboitem='points' and prospectid = " & _ProspectID & " order by Contractnumber"
        ddContracts.DataSource = ds
        ddContracts.DataTextField = "ContractNumber"
        ddContracts.DataValueField = "ID"
        'ddContracts.Items.Add(
        ddContracts.DataBind()
        For i = 0 To ddContracts.Items.Count - 1
            If ddContracts.Items(i).Value = 0 Then
                ddContracts.SelectedIndex = i
                Exit For
            End If
        Next
    End Sub

    Private Sub Fill_Years()
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        If ddYears.SelectedValue = 0 Then
            ds.SelectCommand = "select Distinct(AvailYear) as AvailYear from t_PointsTracking where ContractID = '" & ddYears.SelectedValue & "' union select distinct YEAR(ExpirationDate) from t_PointsTracking  where ContractID = '" & ddYears.SelectedValue & "'"
        Else
            ds.SelectCommand = "select Distinct(AvailYear) as AvailYear from t_PointsTracking where ProspectID = '" & _ProspectID & "' union select distinct YEAR(ExpirationDate) from t_PointsTracking  where ProspectID = '" & _ProspectID & "'"
        End If
        ddYears.DataSource = ds
        ddYears.DataTextField = "AvailYear"
        ddYears.DataValueField = "AvailYear"
        ddYears.Items.Add(New ListItem("ALL", 0))
        ddYears.AppendDataBoundItems = True
        ddYears.DataBind()
        For i = 0 To ddYears.Items.Count - 1
            If ddYears.Items(i).Value = 0 Then
                ddYears.SelectedIndex = i
                Exit For
            End If
        Next
    End Sub

    Protected Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        Load_View()
    End Sub

    Private Sub Load_View()
        Dim ds As New SqlDataSource With {.ConnectionString = Resources.Resource.cns}
        Dim Group As String = "availyear"
        If ddGroup.SelectedValue = "UY" Then Group = "UsageYear"

        ds.SelectCommand = "Select " & Group & " as ID, " & Group & " as Year, sum(Points) as Deposits, sum(Balance) as Balance from v_PointsTracking "
        If ddContracts.SelectedItem.Text.ToUpper <> "ALL" Then
            ds.SelectCommand &= " where contractid = " & ddContracts.SelectedValue
        Else
            ds.SelectCommand &= " where contractid in (select contractid from t_Contract where prospectid = " & _ProspectID & ")"
        End If
        If ddYears.SelectedItem.Text.ToUpper <> "ALL" Then
            If ds.SelectCommand.Contains(" where ") Then
                ds.SelectCommand &= " and " & Group & " = " & ddYears.SelectedValue
            Else
                ds.SelectCommand &= " where " & Group & " = " & ddYears.SelectedValue
            End If
        End If
        ds.SelectCommand &= " group by " & Group & " order by " & Group
        gvYears.DataSource = ds
        gvYears.DataBind()
    End Sub

    Protected Sub gvYears_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvYears.RowDataBound
        If IsNumeric(e.Row.Cells(1).Text) Then
            Dim ds As New SqlDataSource With {.ConnectionString = Resources.Resource.cns}
            Dim Group As String = "availyear"
            If ddGroup.SelectedValue = "UY" Then Group = "UsageYear"
            ds.SelectCommand = "select id, tt.comboitem as TransType, ConfirmationNumber, AvailYear, UsageYear,TransDate, case when posneg = 1 then Points * -1 else points end as Points, ExpirationDate from t_PointsTracking t left outer join t_Comboitems tt on tt.comboitemid = t.transtypeid "
            ds.SelectCommand &= " where " & Group & " = " & e.Row.Cells(1).Text

            If ddContracts.SelectedItem.Text.ToUpper <> "ALL" Then
                ds.SelectCommand &= " and contractid = " & ddContracts.SelectedValue
            Else
                ds.SelectCommand &= " and contractid in (select contractid from t_Contract where prospectid = " & _ProspectID & ")"
            End If
            Dim gv1 As GridView = e.Row.FindControl("GridView2")
            gv1.DataSource = ds
            gv1.DataBind()



        End If
    End Sub

    Private Sub Clear()
        txtAmount.Text = "0"
        txtAvailable.Text = "0"
        rblEntryType0.SelectedIndex = -1
        rblUsageType0.SelectedIndex = -1
        dteStay.Selected_Date = ""
        txtConfNum.Text = ""
    End Sub

    Protected Sub lbCancel_Click(sender As Object, e As EventArgs) Handles lbCancel.Click
        Clear()
    End Sub

    Protected Sub lbSave_Click(sender As Object, e As EventArgs) Handles lbSave.Click
        'Check validation
        If Valid() Then
            Dim cn As New Data.SqlClient.SqlConnection(Resources.Resource.cns)
            Dim cm As New Data.SqlClient.SqlCommand("", cn)
            Dim dr As Data.SqlClient.SqlDataReader

            Dim prosid As Integer = Request("PID")
            Select Case rblEntryType0.SelectedValue
                Case "Bank"
                    'Create both withdrawal and deposit
                    Dim depositID As Integer = 0
                    Dim freq As Integer = 1

                    cm.CommandText = "select ID,interval,c.prospectid from t_PointsTracking t inner join t_Contract c on c.contractid = t.contractid inner join t_Frequency f on f.frequencyid=c.frequencyid where c.contractid=" & ddContract.SelectedValue & " and t.usageyear=" & ddUsageYear.SelectedValue & " and t.TransTypeID=" & (New clsComboItems).Lookup_ID("PointsTransactionType", "Deposit")
                    cn.Open()
                    dr = cm.ExecuteReader
                    If dr.HasRows Then
                        dr.Read()
                        depositID = dr("ID")
                        freq = dr("interval")
                    End If
                    dr.Close()
                    cn.Close()
                    Create_Entry(ddContract.SelectedValue, (New clsComboItems).Lookup_ID("PointsTransactionType", rblEntryType0.SelectedValue), _
                                 txtAmount.Text, ddUsageYear.SelectedValue, ddUsageYear.SelectedValue, "12/31/" & ddUsageYear.SelectedValue, 0, _
                                 IIf(rblUsageType0.Visible, rblUsageType0.SelectedValue, rblEntryType0.SelectedValue), depositID, True, prosid, "")
                    Create_Entry(ddContract.SelectedValue, (New clsComboItems).Lookup_ID("PointsTransactionType", rblEntryType0.SelectedValue), _
                                 txtAmount.Text, ddUsageYear.SelectedValue + 1, ddUsageYear.SelectedValue, "12/31/" & IIf(freq > 2, ddUsageYear.SelectedValue + freq - 1, ddUsageYear.SelectedValue + 1), 0, _
                                 IIf(rblUsageType0.Visible, rblUsageType0.SelectedValue, rblEntryType0.SelectedValue), 0, False, prosid, "")

                Case "ReBank"
                    'Create both withdrawal and deposit
                    Dim depositID As Integer = 0
                    Dim freq As Integer = 1

                    cm.CommandText = "select ID,interval, c.prospectid from t_PointsTracking t inner join t_Contract c on c.contractid = t.contractid inner join t_Frequency f on f.frequencyid=c.frequencyid where c.contractid=" & ddContract.SelectedValue & " and t.usageyear=" & ddUsageYear.SelectedValue & " and t.TransTypeID=" & (New clsComboItems).Lookup_ID("PointsTransactionType", "Bank") & " and PosNeg=0"
                    cn.Open()
                    dr = cm.ExecuteReader
                    If dr.HasRows Then
                        dr.Read()
                        depositID = dr("ID")
                        freq = dr("interval")
                    End If
                    dr.Close()
                    cn.Close()
                    Create_Entry(ddContract.SelectedValue, (New clsComboItems).Lookup_ID("PointsTransactionType", rblEntryType0.SelectedValue), _
                                 txtAmount.Text, ddAvailYear.SelectedValue, ddUsageYear.SelectedValue, "12/31/" & ddAvailYear.SelectedValue, 0, _
                                 IIf(rblUsageType0.Visible, rblUsageType0.SelectedValue, rblEntryType0.SelectedValue), depositID, True, prosid, "")
                    Create_Entry(ddContract.SelectedValue, (New clsComboItems).Lookup_ID("PointsTransactionType", rblEntryType0.SelectedValue), _
                                 txtAmount.Text, IIf(freq = 3, ddAvailYear.SelectedValue + 2, ddAvailYear.SelectedValue + 1), ddUsageYear.SelectedValue, "12/31/" & IIf(freq = 3, ddAvailYear.SelectedValue + 2, ddAvailYear.SelectedValue + 1), 0, _
                                 IIf(rblUsageType0.Visible, rblUsageType0.SelectedValue, rblEntryType0.SelectedValue), 0, False, prosid, "")
                Case "Borrow"
                    'Create both Withdrawal and deposit
                    'Create both withdrawal and deposit
                    Dim depositID As Integer = 0
                    Dim freq As Integer = 1

                    cm.CommandText = "select * from v_PointsTracking where TransType='Deposit' and usageyear=" & ddUsageYear.SelectedValue & " and contractid = " & ddContract.SelectedValue
                    cn.Open()
                    dr = cm.ExecuteReader
                    If dr.HasRows Then
                        dr.Read()
                        depositID = dr("ID")
                        freq = dr("interval")

                    Else
                        'Create the deposit for the usage year
                        dr.Close()
                        cm.CommandText = "insert into t_PointsTracking (ContractID,TransTypeID,Points,AvailYear,UsageYear,ExpirationDate,StayLocID,CreatedByID,TransDate,Comments,ApplyToID,PosNeg,ProspectID,ConfirmationNumber)" & _
                                        " values (" & ddContract.SelectedValue & "," & (New clsComboItems).Lookup_ID("PointsTransactionType", "Deposit") & "," & (New clsContract).Get_Point_Value(ddContract.SelectedValue) & ", " & ddUsageYear.SelectedValue & "," & ddUsageYear.SelectedValue & ",'12/31/" & ddUsageYear.SelectedValue & "',0," & Session("UserDBID") & ",GETDATE(),'Deposit',0,0," & prosid & ",'')"

                        cm.ExecuteNonQuery()
                        cm.CommandText = "select * from v_PointsTracking where TransType='Deposit' and usageyear=" & ddUsageYear.SelectedValue & " and contractid=" & ddContract.SelectedValue
                        dr = cm.ExecuteReader
                        If dr.HasRows Then
                            dr.Read()
                            depositID = dr("ID")
                            freq = dr("interval")

                        End If
                    End If
                    dr.Close()
                    cn.Close()
                    Create_Entry(ddContract.SelectedValue, (New clsComboItems).Lookup_ID("PointsTransactionType", rblEntryType0.SelectedValue), _
                                    txtAmount.Text, ddUsageYear.SelectedValue, ddUsageYear.SelectedValue, "12/31/" & ddUsageYear.SelectedValue, 0, _
                                    IIf(rblUsageType0.Visible, rblUsageType0.SelectedValue, rblEntryType0.SelectedValue), depositID, True, prosid, "")
                    Create_Entry(ddContract.SelectedValue, (New clsComboItems).Lookup_ID("PointsTransactionType", rblEntryType0.SelectedValue), _
                                    txtAmount.Text, ddAvailYear.SelectedValue, ddUsageYear.SelectedValue, "12/31/" & ddUsageYear.SelectedValue, 0, _
                                    IIf(rblUsageType0.Visible, rblUsageType0.SelectedValue, rblEntryType0.SelectedValue), 0, False, prosid, "")
                Case "Usage"
                    'Set Applyto
                    'cm.CommandText = "Select * from v_PointsTracking where balance>0 and contractid = " & ddContract.SelectedValue & " and usageyear=" & ddUsageYear.SelectedValue & " and (availyear <= year(getdate()) and year(expirationdate) >= year(1999)) order by expirationdate, id"
                    cm.CommandText = "Select * from v_PointsTracking where balance>0 and contractid = " & ddContract.SelectedValue & " and usageyear=" & ddUsageYear.SelectedValue & " and (availyear = " & ddAvailYear.SelectedValue & " and year(expirationdate) >= year(1999)) order by expirationdate, id"
                    cn.Open()
                    dr = cm.ExecuteReader
                    Dim amount As Integer = 0
                    If dr.HasRows Then
                        While dr.Read()
                            If dr("Balance") > CInt(txtAmount.Text) Then
                                Create_Entry(ddContract.SelectedValue, (New clsComboItems).Lookup_ID("PointsTransactionType", rblUsageType0.SelectedValue), _
                                            txtAmount.Text, ddAvailYear.SelectedValue, ddUsageYear.SelectedValue, "12/31/" & ddUsageYear.SelectedValue, 0, _
                                            IIf(rblUsageType0.Visible, rblUsageType0.SelectedValue, rblEntryType0.SelectedValue), dr("ID"), True, prosid, txtConfNum.Text)
                                Exit While
                            Else
                                Create_Entry(ddContract.SelectedValue, (New clsComboItems).Lookup_ID("PointsTransactionType", rblUsageType0.SelectedValue), _
                                            dr("Balance"), ddAvailYear.SelectedValue, ddUsageYear.SelectedValue, "12/31/" & ddUsageYear.SelectedValue, 0, _
                                            IIf(rblUsageType0.Visible, rblUsageType0.SelectedValue, rblEntryType0.SelectedValue), dr("ID"), True, prosid, txtConfNum.Text)
                                txtAmount.Text = CInt(txtAmount.Text) - dr("Balance")
                            End If
                        End While
                    End If
                    dr.Close()
                    cn.Close()
                Case "Rent"
                    Create_Entry(ddContract.SelectedValue, (New clsComboItems).Lookup_ID("PointsTransactionType", rblEntryType0.SelectedValue), _
                                            txtAmount.Text, Date.Now.Year, ddUsageYear.SelectedValue, "12/31/" & Date.Now.Year, 0, _
                                            IIf(rblUsageType0.Visible, rblUsageType0.SelectedValue, rblEntryType0.SelectedValue), -1, True, prosid, txtConfNum.Text)
                Case "Deposit"
                    Dim depositID As Integer = 0
                    Dim freq As Integer = 1

                    cm.CommandText = "select * from v_PointsTracking where TransType='Deposit' and contractid = " & ddContract.SelectedValue & " And usageyear = " & ddUsageYear.SelectedValue
                    cn.Open()
                    dr = cm.ExecuteReader
                    If dr.HasRows Then
                        dr.Read()
                        depositID = dr("ID")
                        freq = dr("interval")

                    Else
                        'Create the deposit for the usage year
                        dr.Close()
                        cm.CommandText = "insert into t_PointsTracking (ContractID,TransTypeID,Points,AvailYear,UsageYear,ExpirationDate,StayLocID,CreatedByID,TransDate,Comments,ApplyToID,PosNeg,ProspectID,ConfirmationNumber)" & _
                                        " values (" & ddContract.SelectedValue & "," & (New clsComboItems).Lookup_ID("PointsTransactionType", "Deposit") & "," & (New clsContract).Get_Point_Value(ddContract.SelectedValue) & ", " & ddUsageYear.SelectedValue & "," & ddUsageYear.SelectedValue & ",'12/31/" & ddUsageYear.SelectedValue & "',0," & Session("UserDBID") & ",GETDATE(),'Deposit',0,0," & _ProspectID & ",'')"

                        cm.ExecuteNonQuery()
                        cm.CommandText = "select * from v_PointsTracking where TransType='Deposit' and usageyear=" & ddUsageYear.SelectedValue & " and contractid = " & ddContract.SelectedValue
                        dr = cm.ExecuteReader
                        If dr.HasRows Then
                            dr.Read()
                            depositID = dr("ID")
                            freq = dr("interval")

                        End If
                    End If
                    dr.Close()
                    cn.Close()
            End Select

            dr = Nothing
            cm = Nothing
            cn = Nothing
            Clear()
            Load_View()
        End If
    End Sub

    Private Sub Create_Entry(contractid As Integer, transtypeid As Integer, amount As Integer, availyear As Integer, usageyear As Integer, expdate As String, stayLocation As Integer, comments As String, applyto As Integer, posNeg As Boolean, prospectid As Integer, ConfNum As String)
        Dim points As New clsPointsTracking
        With points
            .ContractID = contractid
            .TransTypeID = transtypeid
            .Points = amount
            .AvailYear = availyear
            .UsageYear = usageyear
            .ExpirationDate = expdate
            .StayLocID = stayLocation
            .CreatedByID = Session("UserDBID")
            .TransDate = IIf(dteStay.Visible, dteStay.Selected_Date, Date.Now) 'Modified from just Date.Now per Workorder 48390
            .Comments = comments
            .ApplyToID = applyto
            .PosNeg = posNeg
            .ProspectID = prospectid
            .ConfirmationNumber = ConfNum
            If .PosNeg And (New clsComboItems).Lookup_ID("PointsTransactionType", rblEntryType0.SelectedValue) <> (New clsComboItems).Lookup_ID("PointsTransactionType", "ReBank") Then
                'find usage
                Dim u As New clsUsage
                u.UsageID = u.Find_UsageID_By_Contract_Year(contractid, usageyear)
                u.Load()
                u.TypeID = (New clsComboItems).Lookup_ID("ReservationType", "PointTracking")
                u.UsageYear = usageyear
                u.ContractID = contractid
                u.Points = u.Points + amount
                u.DateCreated = Date.Now
                u.UserID = Session("UserDBID")
                u.Save()
                u = Nothing
            End If
            .Save()
        End With
        points = Nothing
        GC.Collect()
    End Sub

    Private Function Valid() As Boolean
        Dim ret As Boolean = True
        If rblEntryType0.SelectedIndex < 0 Then
            ret = False
        ElseIf rblEntryType0.SelectedValue = "Deposit" Then
            ret = True
        ElseIf rblEntryType0.SelectedValue = "Usage" And rblUsageType0.SelectedIndex < 0 Then
            ret = False
        ElseIf txtAmount.Text = "" Or Not (IsNumeric(txtAmount.Text)) Then
            ret = False
        ElseIf CDbl(txtAmount.Text) > CDbl(txtAvailable.Text) Then
            ret = False
        ElseIf rblEntryType0.SelectedValue = "Usage" And dteStay.Selected_Date = "" Then
            ret = False
        End If
        
        Return ret
    End Function

    Protected Sub rblEntryType0_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblEntryType0.SelectedIndexChanged
        If rblEntryType0.SelectedValue = "Usage" Then
            rblUsageType0.Visible = True
            dteStay.Visible = True
        Else
            rblUsageType0.Visible = False
            dteStay.Visible = False
        End If
        If rblEntryType0.SelectedValue = "Deposit" Then
            txtAmount.Visible = False
            txtAvailable.Visible = False
            ddAvailYear.Visible = False
        Else
            txtAmount.Visible = True
            txtAvailable.Visible = True
            ddAvailYear.Visible = True
        End If
        Get_Balance(0)
    End Sub


    Private Sub Load_Contracts()
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select ContractID, ContractNumber from t_Contract c inner join t_Comboitems cst on cst.comboitemid = c.subtypeid where cst.comboitem='points' and prospectid = " & _ProspectID & " order by Contractnumber"
        ddContract.DataSource = ds
        ddContract.DataTextField = "ContractNumber"
        ddContract.DataValueField = "ContractID"
        ddContract.DataBind()
        If ddContract.Items.Count > 0 Then
            Fill_Usage_Years(ddContract.SelectedValue, ddUsageYear)
        End If
    End Sub

    Protected Sub ddContract0_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddContract.SelectedIndexChanged
        Fill_Usage_Years(ddContract.SelectedValue, ddUsageYear)
        Get_Balance(0)
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

    Private Sub Get_Available(ContID As Integer, Year As Integer, ByRef txtBalance As TextBox, EntryType As String, availYear As Integer)
        If ContID = 0 Or Year = 0 Or EntryType = "" Then
            txtBalance.Text = 0
            Exit Sub
        End If
        Dim selectCommand As String = "select * from v_PointsTracking where  contractid = " & ContID '& " and expirationdate >= getdate() "
        Dim cn As New Data.SqlClient.SqlConnection(Resources.Resource.cns)
        Dim cm As New Data.SqlClient.SqlCommand(selectCommand, cn)
        Dim dr As Data.SqlClient.SqlDataReader
        Select Case EntryType
            Case "Bank"
                cm.CommandText &= " and TransType='Deposit'" & " and usageyear=" & Year
            Case "ReBank"
                cm.CommandText &= " and TransType='Bank'" & " and usageyear=" & Year
            Case "Usage"
                cm.CommandText &= " and availyear = " & availYear & " and usageyear=" & Year '& " and year(expirationdate) >= year(getdate())) "
            Case "Borrow"
                cm.CommandText &= " and TransType='Deposit'" & " and usageyear=" & Year

            Case "Rent"
                txtBalance.Text = 99999999
        End Select
        If EntryType <> "Rent" And EntryType <> "Borrow" Then
            cn.Open()
            dr = cm.ExecuteReader
            txtBalance.Text = 0
            While dr.Read
                txtBalance.Text = CInt(txtBalance.Text) + dr("Balance")
            End While
            dr.Close()
            cn.Close()
        ElseIf EntryType = "Borrow" Then
            cn.Open()
            dr = cm.ExecuteReader
            txtBalance.Text = 0
            If dr.HasRows Then
                While dr.Read
                    txtBalance.Text = CInt(txtBalance.Text) + dr("Balance")
                End While
            Else
                dr.Close()
                cm.CommandText = selectCommand & " and TransType='Deposit'" & " and usageyear=" & Year & " -interval"
                dr = cm.ExecuteReader
                While dr.Read
                    txtBalance.Text = CInt(txtBalance.Text) + dr("Points")
                End While
            End If
            dr.Close()
            cn.Close()
        End If
        dr = Nothing
        cm = Nothing
        cn = Nothing
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

    Protected Sub ddUsageYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddUsageYear.SelectedIndexChanged
        Get_Balance(0)
    End Sub

    Private Sub Get_Balance(index As Integer)
        Select Case index
            Case 0
                Get_Available(ddContract.SelectedValue, ddUsageYear.SelectedValue, txtAvailable, rblEntryType0.SelectedValue, ddAvailYear.SelectedValue)
        End Select
    End Sub

    Protected Sub ddAvailYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddAvailYear.SelectedIndexChanged
        Get_Balance(0)
    End Sub
End Class
