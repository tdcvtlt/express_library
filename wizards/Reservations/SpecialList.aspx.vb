Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Data
Imports clsReservationWizard
Imports System.Web.Script.Serialization
Imports System.Net.Mail
Partial Class wizard_Reservations_SpecialList
    Inherits System.Web.UI.Page

    Private package_base As New Base_Package
    Private wiz As New Wizard

#Region "Event Handlers"

    Private Sub wizard_Reservations_List_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim ph = CType(Me.Master.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        Dim wiz_data = CType(ph.FindControl("LB_WIZ_DATA"), Label)

        If Session("wizData" + Session("wizGuid")) <> Nothing Then
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(Session("wizData" + Session("wizGuid")))
        Else
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(wiz_data.Text)
        End If
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Session("wizGuid") = wiz.GUID_TIMESTAMP

        btCreate.Visible = IIf(wiz.Scenario = EnumScenario.One, True, False)

        If IsPostBack = False Then
            If wiz.Scenario = EnumScenario.Ten Then
                txSearch.Attributes("Placeholder") = "Phone"
            End If
            Dim s() As String = IIf(wiz.Scenario = EnumScenario.Ten, {"Lead", "Email", "Phone"}, {"Address", "City", "Email", "ID", "Name", "Phone", "Postal Code", "Spouse SSN", "SSN", "State"})
            ddSubject.DataSource = s.OrderBy(Function(x) x)
            ddSubject.DataBind()
            ddSubject.ClearSelection()
            ddSubject.Items.FindByText("Phone").Selected = True

            If Session("ListSpecial" + wiz.GUID_TIMESTAMP) IsNot Nothing Then
                gvProspectSearch.DataSource = CType(Session("ListSpecial" + wiz.GUID_TIMESTAMP), DataTable)
                gvProspectSearch.DataBind()
            End If
        End If
        With wiz
            btNext.Enabled = IIf(.Prospect.Prospect_ID = 0, False, True)
        End With
        lbError.Text = String.Empty
    End Sub
    Protected Sub btPrevious_Click(sender As Object, e As EventArgs) Handles btPrevious.Click
        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 0)
        Navigate(CType(sender, Button))
    End Sub
    Protected Sub btNext_Click(sender As Object, e As EventArgs) Handles btNext.Click
        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 1)
        Navigate(bt)
    End Sub
    Protected Sub btSearch_Click(sender As Object, e As EventArgs) Handles btSearch.Click
        Session("ListSpecial" + wiz.GUID_TIMESTAMP) = Nothing
        If txSearch.Text.Length = 0 Then Return
        Dim search_category = ddSubject.SelectedItem.Text
        Dim text_search = txSearch.Text.Trim()

        If search_category = "Email" Then
            Try
                Dim email As New MailAddress(text_search)
            Catch ex As Exception
                lbError.Text = ex.Message
                Return
            End Try
        End If

        If search_category = "Phone" Then
            Dim regEx_pattern = "^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$"
            Dim regEx = New System.Text.RegularExpressions.Regex(regEx_pattern)
            If regEx.IsMatch(text_search) = False Then
                lbError.Text = "Please enter a valid number"
                Return
            End If
        End If

        Dim dt = Search_Text(search_category, text_search)
        gvProspectSearch.DataSource = dt
        gvProspectSearch.DataKeyNames = New String() {"LEAD"}
        gvProspectSearch.DataBind()
        gvProspectSearch.Enabled = True

        With wiz
            btNext.Enabled = IIf(.Lead_ID > 0 Or .Prospect.Prospect_ID > 0, True, False)
        End With

        btCreate.Visible = IIf(dt.Rows.Count = 0, True, False)

        Session("List" + wiz.GUID_TIMESTAMP) = dt
        Session("ListSpecial" + wiz.GUID_TIMESTAMP) = Nothing
        Session("Search" + wiz.GUID_TIMESTAMP) = Nothing
    End Sub
    Protected Sub gvProspectSearch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvProspectSearch.SelectedIndexChanged
        If wiz.Scenario <> EnumScenario.Ten Then
            wiz.Prospect.Prospect_ID = gvProspectSearch.DataKeys(gvProspectSearch.SelectedRow.RowIndex).Value.ToString()
        End If
        If wiz.Scenario = EnumScenario.One Then
            wiz.Load_All()
            Dim rnd_int32 = -Convert.ToInt32(DateTime.Now.Ticks.ToString().Substring(9))
            wiz.Reservation.ReservationID = rnd_int32
            wiz.Tour.TourID = rnd_int32
        End If

        If wiz.Scenario = EnumScenario.Ten Then

            wiz.Prospect.Prospect_ID = IIf(Integer.Parse(gvProspectSearch.Rows(gvProspectSearch.SelectedRow.RowIndex).Cells(4).Text) > 0,
                                           Integer.Parse(gvProspectSearch.Rows(gvProspectSearch.SelectedRow.RowIndex).Cells(4).Text), 0)

            Dim rnd_int32 = -Convert.ToInt32(DateTime.Now.Ticks.ToString().Substring(9))
            wiz.Lead_ID = IIf(Integer.Parse(gvProspectSearch.Rows(gvProspectSearch.SelectedRow.RowIndex).Cells(1).Text) > 0, Integer.Parse(gvProspectSearch.Rows(gvProspectSearch.SelectedRow.RowIndex).Cells(1).Text), rnd_int32)
            wiz.Load_All()
            wiz.Reservation.ReservationID = rnd_int32
            wiz.Tour.TourID = rnd_int32
        End If
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        btNext_Click(CType(btNext, Object), EventArgs.Empty)
    End Sub
    Protected Sub gvProspectSearch_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvProspectSearch.RowDataBound
        'If e.Row.RowType = DataControlRowType.Header Then
        '    e.Row.Cells(1).Text = "Last Name"
        '    e.Row.Cells(2).Text = "First Name"
        '    e.Row.Cells(3).Text = "Prospect ID"
        '    If wiz.Scenario = EnumScenario.Ten Then
        '        e.Row.Cells(4).Text = "Lead ID"
        '    Else
        '        e.Row.Cells(4).Text = "Phone"
        '    End If
        'End If

        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim dataItem = CType(e.Row.DataItem, DataRowView)
        'End If
    End Sub
    Protected Sub btCreate_Click(sender As Object, e As EventArgs) Handles btCreate.Click

        Dim rnd_int32 = -Convert.ToInt32(DateTime.Now.Ticks.ToString().Substring(9))
        wiz.Prospect.Prospect_ID = rnd_int32
        wiz.Reservation.ReservationID = rnd_int32
        wiz.Tour.TourID = rnd_int32
        wiz.Prospect.Addresses.Clear()
        wiz.Prospect.Emails.Clear()
        wiz.Prospect.Phones.Clear()
        wiz.Lead_ID = rnd_int32

        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        btNext_Click(CType(btNext, Object), e)
    End Sub

#End Region

#Region "Subs/Functions"

    Private Sub Navigate(bt As Button)
        Dim gt = Me.Master.GetType()
        Dim mi As MethodInfo = gt.GetMethod("Navigate")
        Dim parameters = mi.GetParameters()

        Dim dir = Convert.ToInt32(bt.Attributes("nav"))
        Dim parametersArray() As Object = New Object() {Request.PhysicalPath.Substring(Request.PhysicalPath.LastIndexOf("\") + 1).ToLower(), dir}
        Dim r = mi.Invoke(Me.Master, parametersArray)
        Response.Redirect(r.ToString())
    End Sub
    Private Sub Search_Query(sql As String)
        Using cn = New SqlConnection(Resources.Resource.cns)
            Try
                Using cm = New SqlCommand(sql, cn)
                    cn.Open()
                    Dim dr = cm.ExecuteReader()
                    Dim dt = New DataTable
                    dt.Load(dr)
                    gvProspectSearch.DataSource = dt
                    gvProspectSearch.DataKeyNames = New String() {"PROSPECTID"}
                    gvProspectSearch.DataBind()

                    gvProspectSearch.Enabled = True

                    With wiz
                        btNext.Enabled = IIf(.Prospect.Prospect_ID = 0, False, True)
                    End With

                    Session("List" + wiz.GUID_TIMESTAMP) = dt
                    Session("ListReservations" + wiz.GUID_TIMESTAMP) = Nothing
                    Session("Search" + wiz.GUID_TIMESTAMP) = Nothing

                End Using
            Catch ex As Exception

                Response.Write(ex.Message)
            Finally
                cn.Close()
            End Try
        End Using
    End Sub

    Private Function Search_Text(search_category As String, text_search As String) As DataTable
        Dim sql = String.Empty

        Select Case search_category
            Case "Phone"
                sql = String.Format(" select distinct top 100 l.LeadID [Lead], l.LastName [Last Name], l.FirstName [First Name], coalesce(l.ProspectID, 0) [Prospect ID], l.PhoneNumber as Phone, l.EmailAddress as Email from t_Leads l where l.PhoneNumber like '{0}%'", text_search)
            Case "Lead"

                Dim lead_id = 0
                If Integer.TryParse(text_search, lead_id) Then
                    With New clsLeads
                        .LeadID = text_search
                        .Load()
                        If .ProsectID < 1 Then
                            sql = String.Format("select l.LeadID [Lead], LastName [Last Name], FirstName [First Name], Coalesce(ProspectID, 0) [Prospect ID], l.PhoneNumber as Phone, l.EmailAddress as Email from t_Leads l where leadID = {0}", text_search)
                        Else
                            sql = String.Format("select top 1 l.LeadID [Lead], p.LastName [Last Name], p.FirstName [First Name], Coalesce(p.ProspectID, 0) [Prospect ID], l.PhoneNumber as Phone, l.EmailAddress as Email from t_Prospect p inner join t_Leads l on p.ProspectID = l.ProspectID where p.ProspectID = {0} order by l.LeadID desc;", .ProsectID)
                        End If
                    End With
                End If

            Case "Email"
                sql = String.Format("select distinct top 100 l.LeadID [Lead], l.LastName [Last Name], l.FirstName [First Name], coalesce(l.ProspectID, 0) ProspectID, l.PhoneNumber as Phone, l.EmailAddress as Email from t_Leads l where l.EmailAddress like '{0}%'", text_search)
            Case "Name"
                Dim last_name = text_search.Split(",")(0)
                Dim first_name = String.Empty
                If text_search.IndexOf(",") > 0 Then
                    first_name = text_search.Split(",")(1)
                End If
                sql = String.Format("select distinct top 100 l.LeadID [Lead], l.LastName [Last Name], l.FirstName [First Name], coalesce(l.ProspectID, 0) [Prospect ID], l.PhoneNumber as Phone, l.EmailAddress as Email from t_Leads l where l.LastName like '{0}%' and l.FirstName like '{1}%' order by l.LastName, l.FirstName, Email", Trim(last_name).Replace(New Char() {"'"}, "''"), Trim(first_name).Replace(New Char() {"'"}, "''"))
            Case Else
        End Select
        Dim dt = New DataTable
        If sql.Length > 0 Then
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        Dim dr = cm.ExecuteReader()
                        dt.Load(dr)
                        If dt.Rows.Count = 0 Then
                            Select Case search_category
                                Case "Phone"
                                    sql = String.Format("Select distinct top 100 0 As Lead, p.LastName [Last Name], p.FirstName [First Name], p.ProspectID [Prospect ID], ph.Number as Phone, e.Email from t_Prospect p inner join t_ProspectPhone ph on ph.prospectid = p.prospectid left join t_ProspectEmail e on e.ProspectID = p.ProspectID where ph.number like '{0}%' order by p.LastName, p.FirstName", text_search)
                                Case "Email"
                                    sql = String.Format("Select distinct top 100 0 As Lead, p.LastName [Last Name], p.FirstName [First Name], p.ProspectID [Prospect ID], ph.Number as Phone, e.Email from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_ProspectEmail e on e.prospectid = p.prospectid where e.Email like '{0}%' and (len(p.LastName) > 0  and p.LastName is not null) order by p.LastName, p.FirstName ", text_search)
                                Case Else
                            End Select
                            dt.Dispose()
                            dt = New DataTable
                            cm.CommandText = sql
                            dr = cm.ExecuteReader()
                            dt.Load(dr)
                        End If
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
        End If
        Return dt
    End Function
    Private Sub Search_Text(Filter As TextBox, Optional vendorID As Integer = 0)

        Select Case LCase(ddSubject.Text)
            Case "phone"
                If Filter.Text = "" Then
                    If vendorID = 0 Then
                        Search_Query("Select top 50 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & Filter.Text & "%' order by ph.number")
                    Else
                        Search_Query("Select top 50 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where ph.number like '" & Filter.Text & "%' and vp.VendorID in (" & vendorID = 0 & ") order by ph.number")
                    End If
                Else
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & Filter.Text & "%'")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where ph.number like '" & Filter.Text & "%' and vp.VendorID in (" & vendorID = 0 & ")")
                    End If
                End If

            Case "address1"
                If Filter.Text = "" Then
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.Address1 from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid order by Address1")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.Address1 from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by Address1")
                    End If
                Else
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.Address1 from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid where a.address1 like '" & Filter.Text & "%'")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.Address1 from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where a.address1 like '" & Filter.Text & "%' and vp.VendorID in (" & vendorID = 0 & ")")
                    End If
                End If

            Case "city"
                If Filter.Text = "" Then
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.City from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid order by a.City")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.City from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by a.City")
                    End If
                Else
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.City from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid where a.City like '" & Filter.Text & "%'")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.City from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and a.City like '" & Filter.Text & "%'")
                    End If
                End If
            Case "state"
                If Filter.Text = "" Then
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, s.comboitem as State from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_Comboitems s on s.comboitemid = a.stateid order by s.Comboitem")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, s.comboitem as State from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_Comboitems s on s.comboitemid = a.stateid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by s.Comboitem")
                    End If
                Else
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, s.comboitem as State from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_Comboitems s on s.comboitemid = a.stateid where s.comboitem like '" & Filter.Text & "%'")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, s.comboitem as State from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_Comboitems s on s.comboitemid = a.stateid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and s.comboitem like '" & Filter.Text & "%'")
                    End If
                End If
            Case "email"
                If Filter.Text = "" Then
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, e.Email from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid order by e.Email")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, e.Email from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by e.Email")
                    End If
                Else
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, e.Email from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid where e.Email like '" & Filter.Text & "%'  order by e.Email")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, e.Email from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and e.Email like '" & Filter.Text & "%'  order by e.Email")
                    End If
                End If

            Case "name"
                If Filter.Text = "" Then
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by Lastname, Firstname")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by Lastname, Firstname")
                    End If
                Else
                    If InStr(Filter.Text, ",") > 0 Then
                        Dim sName(2) As String
                        sName = Filter.Text.Split(",")
                        If vendorID = 0 Then
                            Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.lastname like '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' and p.firstname like '" & Trim(sName(1)).Replace(New Char() {"'"}, "''") & "%'")
                        Else
                            Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and p.lastname like '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' and p.firstname like '" & Trim(sName(1)).Replace(New Char() {"'"}, "''") & "%'")
                        End If
                    Else
                        If vendorID = 0 Then
                            Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.lastname  like '" & Filter.Text.Replace(New Char() {"'"}, "''") & "%'")
                        Else
                            Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and p.lastname  like '" & Filter.Text.Replace(New Char() {"'"}, "''") & "%'")
                        End If
                    End If

                End If

            Case "id"
                If Filter.Text = "" Then
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.ProspectID,p.LastName, p.FirstName, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by p.Prospectid")
                    Else
                        Search_Query("Select top 100 p.ProspectID,p.LastName, p.FirstName, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by p.Prospectid")
                    End If
                Else
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.ProspectID,p.LastName, p.FirstName, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.Prospectid like '" & Filter.Text & "%'")
                    Else
                        Search_Query("Select top 100 p.ProspectID,p.LastName, p.FirstName, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and p.Prospectid like '" & Filter.Text & "%'")
                    End If
                End If

            Case "postalcode"
                If Filter.Text = "" Then
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid order by a.PostalCode")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by a.PostalCode")
                    End If
                Else
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid where a.PostalCode like '" & Filter.Text & "%'")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and a.PostalCode like '" & Filter.Text & "%'")
                    End If
                End If

            Case "ssn"
                If Filter.Text = "" Then
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by p.SSN")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by p.SSN")
                    End If
                Else
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.SSN like '" & Filter.Text & "%'")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and p.SSN like '" & Filter.Text & "%'")
                    End If
                End If

            Case "spousessn"
                If Filter.Text = "" Then
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by p.SpouseSSN")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by p.SpouseSSN")
                    End If
                Else
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.SpouseSSN like '" & Filter.Text & "%'")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and p.SpouseSSN like '" & Filter.Text & "%'")
                    End If
                End If

            Case "LeadID".ToLower()
                If Filter.Text.Length > 0 Then
                    If Integer.TryParse(Filter.Text, 0) Then
                        With New clsLeads
                            .LeadID = Filter.Text
                            .Load()
                            If .ProsectID < 1 Then
                                Search_Query(String.Format("select LastName, FirstName, Coalesce(ProspectID, 0), LeadID [ProspectID] from t_Leads where leadID={0}", Filter.Text))
                            Else
                                Search_Query(String.Format("select top 1 p.LastName, p.FirstName, Coalesce(p.ProspectID, 0) ProspectID, l.LeadID  from t_Prospect p inner join t_Leads l on p.ProspectID = l.ProspectID where p.ProspectID = {0} order by l.LeadID desc;", .ProsectID))
                            End If
                        End With

                    End If
                End If

            Case Else
                If Filter.Text = "" Then
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by ph.number")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by ph.number")
                    End If
                Else
                    If vendorID = 0 Then
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.Number like '" & Filter.Text & "%'")
                    Else
                        Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and ph.Number like '" & Filter.Text & "%'")
                    End If
                End If


        End Select
    End Sub

#End Region
End Class
