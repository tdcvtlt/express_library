Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Data
Imports clsReservationWizard
Imports System.Web.Script.Serialization

Partial Class wizard_Reservations_List
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
                txSearch.Attributes("Placeholder") = "Vendor #ID"
            Else

            End If
            Dim s() As String = IIf(wiz.Scenario = EnumScenario.Ten, {"LeadID"}, {"Address", "City", "Email", "ID", "Name", "Phone", "Postal Code", "Spouse SSN", "SSN", "State"})
            ddSubject.DataSource = s.OrderBy(Function(x) x)
            ddSubject.DataBind()
            ddSubject.ClearSelection()
            If wiz.Scenario <> EnumScenario.Ten Then
                ddSubject.Items.FindByText("Phone").Selected = True
            End If

            If Session("List" + wiz.GUID_TIMESTAMP) IsNot Nothing Then
                    gvProspectSearch.DataSource = CType(Session("List" + wiz.GUID_TIMESTAMP), DataTable)
                    gvProspectSearch.DataBind()
                End If
            End If
            With wiz
            btNext.Enabled = IIf(.Prospect.Prospect_ID = 0, False, True)
        End With
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
        Session("ListReservations" + wiz.GUID_TIMESTAMP) = Nothing
        Search_Text(txSearch, 0)
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

            If Integer.Parse(gvProspectSearch.Rows(gvProspectSearch.SelectedRow.RowIndex).Cells(3).Text) > 0 Then
                wiz.Prospect.Prospect_ID = Integer.Parse(gvProspectSearch.Rows(gvProspectSearch.SelectedRow.RowIndex).Cells(3).Text)
            Else
                wiz.Prospect.Prospect_ID = 0
            End If

            wiz.Lead_ID = Integer.Parse(gvProspectSearch.Rows(gvProspectSearch.SelectedRow.RowIndex).Cells(4).Text)
            wiz.Load_All()
            Dim rnd_int32 = -Convert.ToInt32(DateTime.Now.Ticks.ToString().Substring(9))
            wiz.Reservation.ReservationID = rnd_int32
            wiz.Tour.TourID = rnd_int32
        End If
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        btNext_Click(CType(btNext, Object), EventArgs.Empty)
    End Sub
    Protected Sub gvProspectSearch_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvProspectSearch.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = "Last Name"
            e.Row.Cells(2).Text = "First Name"
            e.Row.Cells(3).Text = "Prospect ID"
            If wiz.Scenario = EnumScenario.Ten Then
                e.Row.Cells(4).Text = "Lead ID"
            Else
                e.Row.Cells(4).Text = "Phone"
            End If
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dataItem = CType(e.Row.DataItem, DataRowView)
        End If
    End Sub
    Protected Sub btCreate_Click(sender As Object, e As EventArgs) Handles btCreate.Click

        Dim rnd_int32 = -Convert.ToInt32(DateTime.Now.Ticks.ToString().Substring(9))
        wiz.Prospect.Prospect_ID = rnd_int32
        wiz.Reservation.ReservationID = rnd_int32
        wiz.Tour.TourID = rnd_int32

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
