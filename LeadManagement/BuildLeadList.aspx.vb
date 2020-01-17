Imports System.Data
Partial Class LeadManagement_BuildLeadList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim dt As New DataTable
            dt.Columns.Add("Field")
            dt.Columns.Add("Value")
            Session("QualTable") = dt
            dt = New DataTable
            dt.Columns.Add("Field")
            dt.Columns.Add("Value")
            Session("ExcludeTable") = dt
            dt = New DataTable
            dt.Columns.Add("AreaCode")
            Session("ACTable") = dt

            ddFNameLength.Items.Add(New ListItem(0, 0))
            ddFNameLength.Items.Add(New ListItem(1, 1))
            ddFNameLength.Items.Add(New ListItem(2, 2))

            ddLNameLength.Items.Add(New ListItem(0, 0))
            ddLNameLength.Items.Add(New ListItem(1, 1))
            ddLNameLength.Items.Add(New ListItem(2, 2))

            For i = 0 To 36
                ddMonthsOut.Items.Add(New ListItem(i, i))
            Next

            Dim oLeadList As New clsLeadLists
            ddQualField.Items.Add(New ListItem("", 0))
            ddQualField.DataSource = oLeadList.get_Qualification_Headers
            ddQualField.DataTextField = "Name"
            ddQualField.DataValueField = "Name"
            ddQualField.AppendDataBoundItems = True
            ddQualField.DataBind()
            ddExcludeField.Items.Add(New ListItem("", 0))
            ddExcludeField.DataSource = oLeadList.get_Qualification_Headers
            ddExcludeField.DataTextField = "Name"
            ddExcludeField.DataValueField = "Name"
            ddExcludeField.AppendDataBoundItems = True
            ddExcludeField.DataBind()
            oLeadList = Nothing
            Dim oVendor As New clsVendor
            ddVendors.DataSource = oVendor.List_Vendors
            ddVendors.DataValueField = "VendorID"
            ddVendors.DataTextField = "Vendor"
            ddVendors.DataBind()
            oVendor = Nothing
            Dim oLead As New clsLeads
            ddAreaCodes.Items.Add(New ListItem("", 0))
            ddAreaCodes.DataSource = oLead.Get_Area_Codes()
            ddAreaCodes.DataValueField = "AreaCode"
            ddAreaCodes.DataTextField = "AreaCode"
            ddAreaCodes.DataBind()
            oLead = Nothing
        End If
    End Sub

    Protected Sub ddQualField_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddQualField.SelectedIndexChanged
        ddQualValue.Items.Clear()
        Dim oLeadList As New clsLeadLists
        ddQualValue.Items.Add(New ListItem("ALL", "ALLITEM$"))
        ddQualValue.DataSource = oLeadList.get_Qualification_Values(ddQualField.SelectedValue)
        ddQualValue.DataTextField = "Value"
        ddQualValue.DataValueField = "Value"
        ddQualValue.AppendDataBoundItems = True
        ddQualValue.DataBind()
        oLeadList = Nothing
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim dt As New DataTable
        dt = Session("QualTable")
        If ddQualValue.SelectedValue = "ALLITEM$" Then
            For i = 1 To ddQualValue.Items.Count - 1
                Dim dr As DataRow = dt.NewRow
                dr("Field") = ddQualField.SelectedValue
                dr("Value") = ddQualValue.Items(i).Value
                dt.Rows.Add(dr)
            Next
        Else
            Dim dr As DataRow = dt.NewRow
            dr("Field") = ddQualField.SelectedValue
            dr("Value") = ddQualValue.SelectedValue
            dt.Rows.Add(dr)
        End If
        Session("QualTable") = dt
        gvQualifications.DataSource = Session("QualTable")
        gvQualifications.DataBind()
        'Dim wClause As String = Build_Query()
        'Dim oLeadList As New clsLeadLists
        'lblNewQtyAvail.Text = oLeadList.Get_Lead_Count(True, wClause)
        'lblOldQtyAvail.Text = oLeadList.Get_Lead_Count(False, wClause)
        'oLeadList = Nothing
        'lblNewQtyAvail.Text = wClause
    End Sub

    Private Sub Show_Lead_Counts()
        Dim wClause As String = Build_Query()
        Dim oLeadList As New clsLeadLists
        lblNewQtyAvail.Text = oLeadList.Get_Lead_Count(True, wClause)
        lblOldQtyAvail.Text = oLeadList.Get_Lead_Count(False, wClause)
        Label2.Text = oLeadList.Err
        oLeadList = Nothing
    End Sub

    Private Function Build_Query() As String
        Dim sSQL As String = ""
        Dim fieldNames As String = ""
        Dim x As Integer = 0

        'Qualifications
        If gvQualifications.Rows.Count > 0 Then
            For Each row As GridViewRow In gvQualifications.Rows
                If fieldNames = "" Then
                    fieldNames = row.Cells(1).Text
                Else
                    If InStr(fieldNames, row.Cells(1).Text) = 0 Then
                        fieldNames = fieldNames & "," & row.Cells(1).Text
                    End If
                End If
            Next
            Dim fNames() As String = fieldNames.Split(",")
            For i = 0 To UBound(fNames)
                If sSQL = "" Then
                    sSQL = "case when [" & fNames(i) & "] is null then '' else [" & fNames(i) & "] end in ("
                Else
                    sSQL = sSQL & " and case when [" & fNames(i) & "] is null then '' else [" & fNames(i) & "] end in ("
                End If
                x = 0
                For Each row As GridViewRow In gvQualifications.Rows
                    If row.Cells(1).Text = fNames(i) Then
                        If x = 0 Then
                            sSQL = sSQL & "'" & Server.HtmlDecode(Replace(Trim(row.Cells(2).Text.Replace("&nbsp;", "")), "'", "''")) & "'"
                        Else
                            sSQL = sSQL & ",'" & Server.HtmlDecode(Replace(Trim(row.Cells(2).Text.Replace("&nbsp;", "")), "'", "''")) & "'"

                        End If
                        x += 1
                    End If
                Next
                sSQL = sSQL & ")"
            Next
        End If


        'Exclusions
        fieldNames = ""
        x = 0
        If gvExclusions.Rows.Count > 0 Then
            If sSQL = "" Then
                sSQL = "l.LeadID not in (Select leadid from v_Leads where "
            Else
                sSQL = sSQL & " and l.LeadID not in (Select leadid from v_Leads where "
            End If
            For Each row As GridViewRow In gvExclusions.Rows
                If fieldNames = "" Then
                    fieldNames = row.Cells(1).Text
                Else
                    If InStr(fieldNames, row.Cells(1).Text) = 0 Then
                        fieldNames = fieldNames & "," & row.Cells(1).Text
                    End If
                End If
            Next
            Dim xFNames() As String = fieldNames.Split(",")
            For i = 0 To UBound(xFNames)
                If i = 0 Then
                    sSQL = sSQL & " case when [" & xFNames(i) & "] is null then '' else [" & xFNames(i) & "] end in ("
                Else
                    sSQL = sSQL & " and case when [" & xFNames(i) & "] is null then '' else [" & xFNames(i) & "] end in ("
                End If
                x = 0
                For Each row As GridViewRow In gvExclusions.Rows
                    If row.Cells(1).Text = xFNames(i) Then
                        If x = 0 Then
                            sSQL = sSQL & "'" & Server.HtmlDecode(Replace(Trim(row.Cells(2).Text.Replace("&nbsp;", "")), "'", "''")) & "'"
                        Else
                            sSQL = sSQL & ",'" & Server.HtmlDecode(Replace(Trim(row.Cells(2).Text.Replace("&nbsp;", "")), "'", "''")) & "'"

                        End If
                        x += 1
                    End If
                Next
                sSQL = sSQL & ")"
            Next
            sSQL = sSQL & ")"
        End If
        sSQL = sSQL & " and MonthsOut >= " & ddMonthsOut.SelectedValue
        sSQL = sSQL & " and Len(FirstName) >= " & ddFNameLength.SelectedValue & " and Len(LastName) >= " & ddLNameLength.SelectedValue
        If cbPhoneReq.Checked Then
            sSQL = sSQL & " and Len(PhoneNumber) = 10"
            If cbFilterAreaCode.Checked Then
                If gvAreaCodes.Rows.Count > 0 Then
                    sSQL = sSQL & " and Left(Replace(PhoneNumber, ' ',''), 3) in ("
                    x = 0
                    For Each row In gvAreaCodes.Rows
                        If x = 0 Then
                            sSQL = sSQL & "'" & row.Cells(1).Text & "'"
                        Else
                            sSQL = sSQL & ",'" & row.Cells(1).Text & "'"
                        End If
                        x = x + 1
                    Next
                    sSQL = sSQL & ")"
                End If
            End If
        End If
        'If sSQL = "" Then
        '    sSQL = sSQL & " phonenumber not in (select phonenumber from t_Leads l inner join t_Lead2List ll on ll.leadid = l.leadid inner join t_LeadLists ls on ls.leadlistid = ll.listid where daterevoked is null)"
        'Else
        '    sSQL = sSQL & " and phonenumber not in (select phonenumber from t_Leads l inner join t_Lead2List ll on ll.leadid = l.leadid inner join t_LeadLists ls on ls.leadlistid = ll.listid where daterevoked is null)"
        'End If
        Label1.Text = sSQL
        Return sSQL
    End Function

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim bVal As Boolean = True
        Dim sErr As String = ""
        'ErrorChecking

        If txtDesc.Text = "" Then
            sErr = "Please Enter a List Description."
            bVal = False
        End If

        If lblNewQtyAvail.Text = 0 And lblOldQtyAvail.Text = 0 Then
            sErr = "No Leads Available To Build List."
            bVal = False
        End If

        If Not (IsNumeric(txtNewLeadsQty.Text)) Or Not (IsNumeric(txtOldLeadsQty.Text)) Then
            sErr = "Please Select a Valid Number of Leads to Assign to List."
            bVal = False
        End If

        If CInt(txtNewLeadsQty.Text) > CInt(lblNewQtyAvail.Text) Or CInt(txtNewLeadsQty.Text) < 0 Or CInt(txtOldLeadsQty.Text) > CInt(lblOldQtyAvail.Text) Or CInt(txtOldLeadsQty.Text) < 0 Then
            sErr = "Please Select a Valid Number of Leads to Assign to List."
            bVal = False
        End If

        If bVal Then
            Dim listID As Integer = 0
            Dim oLeadList As New clsLeadLists
            oLeadList.LeadListID = 0
            oLeadList.Load()
            oLeadList.DateCreated = System.DateTime.Now
            oLeadList.CreatedByID = Session("UserDBID")
            oLeadList.VendorID = ddVendors.SelectedValue
            oLeadList.Description = txtDesc.Text
            oLeadList.Save()
            listID = oLeadList.LeadListID
            oLeadList = Nothing
            Dim oLead2List As New clsLead2List
            oLead2List.Build_List(txtNewLeadsQty.Text, txtOldLeadsQty.Text, Build_Query, listID)
            Response.Write(oLead2List.Err)
            oLeadList = Nothing
            oLead2List = Nothing
            'Response.Redirect("BuildLeadList.aspx")
        Else
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "Alerts", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub gvQualifications_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvQualifications.RowDeleting
        Dim dt As New DataTable
        Dim dRow As DataRow
        dt = Session("QualTable")
        dRow = dt.Rows(e.RowIndex)
        dt.Rows.Remove(dRow)
        Session("QualTable") = dt
        gvQualifications.DataSource = Session("QualTable")
        gvQualifications.DataBind()
        If gvQualifications.Rows.Count = 0 And gvExclusions.Rows.Count = 0 Then
            lblNewQtyAvail.Text = 0
            lblOldQtyAvail.Text = 0
        Else
            'Dim wClause As String = Build_Query()
            'Dim oLeadList As New clsLeadLists
            'lblNewQtyAvail.Text = oLeadList.Get_Lead_Count(True, wClause)
            'lblOldQtyAvail.Text = oLeadList.Get_Lead_Count(False, wClause)
            'oLeadList = Nothing
        End If
    End Sub

    Protected Sub cbPhoneReq_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbPhoneReq.CheckedChanged
        If cbPhoneReq.Checked Then
            tblAreaCodes.Visible = True
            cbFilterAreaCode.Checked = False
            TableRow2.Visible = False
            Dim dt As New DataTable
            dt.Columns.Add("AreaCode")
            Session("ACTable") = dt
            gvAreaCodes.DataSource = Session("ACTable")
            gvAreaCodes.DataBind()
            gvAreaCodes.Visible = False
        Else
            tblAreaCodes.Visible = False
        End If
        If gvQualifications.Rows.Count > 0 Or gvExclusions.Rows.Count > 0 Then
            'Dim wClause As String = Build_Query()
            'Dim oLeadList As New clsLeadLists
            'lblNewQtyAvail.Text = oLeadList.Get_Lead_Count(True, wClause)
            'lblOldQtyAvail.Text = oLeadList.Get_Lead_Count(False, wClause)
            'oLeadList = Nothing
        End If
    End Sub

    Protected Sub btnAddAC_Click(sender As Object, e As System.EventArgs) Handles btnAddAC.Click
        Dim dt As New DataTable
        dt = Session("ACTable")
        If ddAreaCodes.SelectedValue = "ALL" Then
            For i = 1 To ddAreaCodes.Items.Count - 1
                Dim dr As DataRow = dt.NewRow
                dr("AreaCode") = ddAreaCodes.SelectedValue
                dt.Rows.Add(dr)
            Next
        Else
            Dim dr As DataRow = dt.NewRow
            dr("AreaCode") = ddAreaCodes.SelectedValue
            dt.Rows.Add(dr)
        End If
        Session("ACTable") = dt
        gvAreaCodes.DataSource = Session("ACTable")
        gvAreaCodes.DataBind()
        If gvQualifications.Rows.Count > 0 Or gvExclusions.Rows.Count > 0 Then
            'Dim wClause As String = Build_Query()
            'Dim oLeadList As New clsLeadLists
            'lblNewQtyAvail.Text = oLeadList.Get_Lead_Count(True, wClause)
            'lblOldQtyAvail.Text = oLeadList.Get_Lead_Count(False, wClause)
            'oLeadList = Nothing
        End If
    End Sub

    Protected Sub gvAreaCodes_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvAreaCodes.RowDeleting
        Dim dt As New DataTable
        Dim dRow As DataRow
        dt = Session("ACTable")
        dRow = dt.Rows(e.RowIndex)
        dt.Rows.Remove(dRow)
        Session("ACTable") = dt
        gvAreaCodes.DataSource = Session("ACTable")
        gvAreaCodes.DataBind()
        If gvQualifications.Rows.Count > 0 Or gvExclusions.Rows.Count > 0 Then
            'Dim wClause As String = Build_Query()
            'Dim oLeadList As New clsLeadLists
            'lblNewQtyAvail.Text = oLeadList.Get_Lead_Count(True, wClause)
            'lblOldQtyAvail.Text = oLeadList.Get_Lead_Count(False, wClause)
            'oLeadList = Nothing
        End If
    End Sub

    Protected Sub cbFilterAreaCode_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbFilterAreaCode.CheckedChanged
        If cbFilterAreaCode.Checked Then
            Dim dt As New DataTable
            dt.Columns.Add("AreaCode")
            Session("ACTable") = dt
            gvAreaCodes.DataSource = Session("ACTable")
            gvAreaCodes.DataBind()
            gvAreaCodes.Visible = True
            TableRow2.Visible = True
        Else
            TableRow2.Visible = False
        End If
        If gvQualifications.Rows.Count > 0 Or gvExclusions.Rows.Count > 0 Then
            'Dim wClause As String = Build_Query()
            'Dim oLeadList As New clsLeadLists
            'lblNewQtyAvail.Text = oLeadList.Get_Lead_Count(True, wClause)
            'lblOldQtyAvail.Text = oLeadList.Get_Lead_Count(False, wClause)
            'oLeadList = Nothing
        End If
    End Sub

    Protected Sub ddLNameLength_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddLNameLength.SelectedIndexChanged
        If gvQualifications.Rows.Count > 0 Or gvExclusions.Rows.Count > 0 Then
            'Dim wClause As String = Build_Query()
            'Dim oLeadList As New clsLeadLists
            'lblNewQtyAvail.Text = oLeadList.Get_Lead_Count(True, wClause)
            'lblOldQtyAvail.Text = oLeadList.Get_Lead_Count(False, wClause)
            'oLeadList = Nothing
        End If
    End Sub

    Protected Sub ddFNameLength_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddFNameLength.SelectedIndexChanged
        If gvQualifications.Rows.Count > 0 Or gvExclusions.Rows.Count > 0 Then
            'Dim wClause As String = Build_Query()
            'Dim oLeadList As New clsLeadLists
            'lblNewQtyAvail.Text = oLeadList.Get_Lead_Count(True, wClause)
            'lblOldQtyAvail.Text = oLeadList.Get_Lead_Count(False, wClause)
            'oLeadList = Nothing
        End If
    End Sub

    Protected Sub ddMonthsOut_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddMonthsOut.SelectedIndexChanged
        If gvQualifications.Rows.Count > 0 Or gvExclusions.Rows.Count > 0 Then
            'Dim wClause As String = Build_Query()
            'Dim oLeadList As New clsLeadLists
            'lblNewQtyAvail.Text = oLeadList.Get_Lead_Count(True, wClause)
            'lblOldQtyAvail.Text = oLeadList.Get_Lead_Count(False, wClause)
            'oLeadList = Nothing
        End If
    End Sub

    Protected Sub ddVendors_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddVendors.SelectedIndexChanged

    End Sub

    Protected Sub ddExcludeField_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddExcludeField.SelectedIndexChanged
        ddExcludeValue.Items.Clear()
        Dim oLeadList As New clsLeadLists
        ddExcludeValue.Items.Add(New ListItem("ALL", "ALLITEM$"))
        ddExcludeValue.DataSource = oLeadList.get_Qualification_Values(ddExcludeField.SelectedValue)
        ddExcludeValue.DataTextField = "Value"
        ddExcludeValue.DataValueField = "Value"
        ddExcludeValue.AppendDataBoundItems = True
        ddExcludeValue.DataBind()
        oLeadList = Nothing
    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click
        Dim dt As New DataTable
        dt = Session("ExcludeTable")
        If ddExcludeValue.SelectedValue = "ALLITEM$" Then
            For i = 1 To ddExcludeValue.Items.Count - 1
                Dim dr As DataRow = dt.NewRow
                dr("Field") = ddExcludeField.SelectedValue
                dr("Value") = ddExcludeValue.Items(i).Value
                dt.Rows.Add(dr)
            Next
        Else
            Dim dr As DataRow = dt.NewRow
            dr("Field") = ddExcludeField.SelectedValue
            dr("Value") = ddExcludeValue.SelectedValue
            dt.Rows.Add(dr)
        End If
        Session("ExcludeTable") = dt
        gvExclusions.DataSource = Session("ExcludeTable")
        gvExclusions.DataBind()
        'Dim wClause As String = Build_Query()
        'Dim oLeadList As New clsLeadLists
        'lblNewQtyAvail.Text = oLeadList.Get_Lead_Count(True, wClause)
        'lblOldQtyAvail.Text = oLeadList.Get_Lead_Count(False, wClause)
        'oLeadList = Nothing
        'lblNewQtyAvail.Text = wClause
    End Sub
    Protected Sub gvExclusions_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvExclusions.RowDeleting
        Dim dt As New DataTable
        Dim dRow As DataRow
        dt = Session("ExcludeTable")
        dRow = dt.Rows(e.RowIndex)
        dt.Rows.Remove(dRow)
        Session("ExcludeTable") = dt
        gvExclusions.DataSource = Session("ExcludeTable")
        gvExclusions.DataBind()
        If gvExclusions.Rows.Count = 0 And gvQualifications.Rows.Count = 0 Then
            lblNewQtyAvail.Text = 0
            lblOldQtyAvail.Text = 0
        Else
            'Dim wClause As String = Build_Query()
            'Dim oLeadList As New clsLeadLists
            'lblNewQtyAvail.Text = oLeadList.Get_Lead_Count(True, wClause)
            'lblOldQtyAvail.Text = oLeadList.Get_Lead_Count(False, wClause)
            'oLeadList = Nothing
        End If
    End Sub

    Protected Sub btnCounts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCounts.Click
        Show_Lead_Counts()
    End Sub
End Class
