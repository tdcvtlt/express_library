Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.Security
Imports System.Web
Imports System.Configuration
Imports System.Data
Imports System

Partial Class Reports_CustomerService_ReservationAccommodation
    Inherits System.Web.UI.Page

    Private cnn As SqlConnection = Nothing
    Private cmd As SqlCommand = Nothing
    Private rdr As SqlDataReader = Nothing
    Private ada As SqlDataAdapter = Nothing
    Private ds As DataSet = Nothing


    Private tableNames() As String = New String() _
                                     { _
                                        "AccomName", _
                                        "ReservationStatus", _
                                        "ReservationLocation", _
                                        "ReservationSource", _
                                        "TourTime" _
                                     }

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            
            Dim ds As DataSet = Data_Load(Nothing)

            DropDownList_Bind(DDL_Location, ds, tableNames(2))
            DropDownList_Bind(DDL_Hotel, ds, tableNames(0))
            DropDownList_Bind(DDL_Status, ds, tableNames(1))
        End If
    End Sub

    Private Function Data_Load(ByVal keyValue As KeyValuePair(Of String, String())) As DataSet

        Dim cns As String = Resources.Resource.cns
        Dim sql As String = String.Empty
        If ds Is Nothing Then ds = New DataSet()
        cnn = New SqlConnection(cns)

        If keyValue.Key Is Nothing Then

            For Each s As String In tableNames
                ds.Tables.Add(s)
            Next


            ada = New SqlDataAdapter("select * from t_comboitems where comboid in (318) and active = 1 order by comboitem", cnn)
            ada.FillSchema(ds, SchemaType.Source, tableNames(2))
            ada.Fill(ds, tableNames(2))

            ada = New SqlDataAdapter("select * from t_comboitems a inner join t_combos " & _
                                     "b on a.comboid = b.comboid where comboname = 'ReservationStatus' " & _
                                     "order by comboitem", cnn)

            ada.FillSchema(ds, SchemaType.Source, tableNames(1))
            ada.Fill(ds, tableNames(1))

            'ada = New SqlDataAdapter("select * from t_comboitems where comboid in (230) and active = 1 order by comboitem", cnn)

            ada = New SqlDataAdapter("select AccomID as ComboItemID, AccomName as ComboItem from t_Accom where active = 1", cnn)
            ada.FillSchema(ds, SchemaType.Source, tableNames(0))
            ada.Fill(ds, tableNames(0))

            ada = New SqlDataAdapter("select * from t_comboitems where comboid in (361) and active = 1 order by comboitem", cnn)

            ada.FillSchema(ds, SchemaType.Source, tableNames(3))
            ada.Fill(ds, tableNames(3))


            ada = New SqlDataAdapter("select * from t_comboitems where comboid in (846) and active = 1 order by comboitem", cnn)

            ada.FillSchema(ds, SchemaType.Source, tableNames(4))
            ada.Fill(ds, tableNames(4))


            'Tour required...
        ElseIf keyValue.Key.Equals("0") Then

            sql = String.Format( _
                "select distinct(b.prospectid), c.checkindate, a.TourDate, a.TourTime, c.resLocationID, c.ReservationID, c.SourceID, d.AccomLocationID, d.LodgingID, c.ReservationNumber, c.CheckOutDate, " & _
                "datediff(d, c.CheckinDate, c.CheckoutDate) [Nights], c.DateBooked, e.Name, f.LastName + ', ' + f.FirstName [Personnel],  " & _
                "(select top 1 number from t_ProspectPhone where prospectid = b.prospectid order by active desc) [Phone] " & _
                "from t_tour a " & _
                "left join t_prospect b on a.prospectid = b.prospectid " & _
                "left join t_reservations c on c.tourid = a.tourid " & _
                "left join t_accommodation d on d.reservationid = c.reservationid " & _
                "left join t_campaign e on e.campaignid = a.campaignid " & _
                "left join (select b.KeyValue, a.* from t_personnel a left join t_personneltrans b " & _
                "on a.personnelid = b.personnelid where b.titleid in (select comboitemid from t_combos x " & _
                "inner join t_comboItems y on x.comboid = y.comboid where x.comboname = 'personneltitle' " & _
                "and y.comboitem = 'team leader')) f   " & _
                "on f.KeyValue = c.reservationid " & _
                "where c.statusid in ({0}) " & _
                "and c.checkindate between '{1}' and '{2}' " & _
                "and c.datebooked between '{3}' and '{4}' " & _
                "and c.reslocationid in ({5}) " & _
                "and d.AccomID in ({6}) " & _
                "and d.accommodationid <> '' " & _
                "order by c.checkindate ", _
                DDL_Status.SelectedItem.Value, _
                DF_CheckIn.Selected_Date, _
                DF_CheckOut.Selected_Date, _
                DF_BookedFrom.Selected_Date, _
                DF_BookedTo.Selected_Date, _
                DDL_Location.SelectedItem.Value, _
                DDL_Hotel.SelectedItem.Value)

            ds.Tables.Add("Result")
            ada = New SqlDataAdapter(sql, cnn)
            ada.Fill(ds, "Result")

        ElseIf keyValue.Key.Equals("Prospect") Then

            sql = String.Format( _
                "select * from t_Prospect where prospectid in ({0})", String.Join(",", keyValue.Value))

            ds.Tables.Add("Prospect")
            ada = New SqlDataAdapter(sql, cnn)
            ada.FillSchema(ds, SchemaType.Source, "Prospect")
            ada.Fill(ds, "Prospect")


            'No tour is required
        ElseIf keyValue.Key.Equals("1") Then

            sql = String.Format( _
                "select distinct(p.prospectid), *, " & _
                "datediff(d, r.CheckinDate, r.CheckoutDate) [Nights], " & _
                "(select top 1 number from t_ProspectPhone where prospectid = p.prospectid order by active desc) [Phone]  " & _
                "	from t_reservations r  " & _
                "		left outer join t_prospect p on r.prospectid = p.prospectid  " & _
                "		left outer join t_accommodation a on a.reservationid = r.reservationid  " & _
                "		left outer join (select pt.KeyValue, per.lastname + ', ' + per.firstname as name " & _
                "						from t_Personnel per Left outer join t_PersonnelTrans pt on pt.keyvalue = per.personnelid " & _
                "						where pt.titleid in (select comboitemid from t_Comboitems where comboid = 258 and comboitemid = 16800)) b  " & _
                "		on b.KeyValue=r.reservationid  " & _
                "where r.statusid in ({0}) and r.checkindate between '{1}' " & _
                "and '{2}' and r.datebooked between '{3}' and '{4}' " & _
                "and r.reslocationid in ({5}) and a.AccomID in ({6}) " & _
                "and a.accommodationid <> '' and (r.tourid is null or r.tourid = '0') order by r.checkindate", _
                DDL_Status.SelectedItem.Value, _
                DF_CheckIn.Selected_Date, _
                DF_CheckOut.Selected_Date, _
                DF_BookedFrom.Selected_Date, _
                DF_BookedTo.Selected_Date, _
                DDL_Location.SelectedItem.Value, _
                DDL_Hotel.SelectedItem.Value)

            ds.Tables.Add("Result")
            ada = New SqlDataAdapter(sql, cnn)
            ada.Fill(ds, "Result")

        End If



        cnn.Close()
        Return ds
    End Function


    Private Sub DropDownList_Bind(ByVal ddl As DropDownList, ByVal ds As DataSet, ByVal tableName As String)
        ddl.DataSource = ds.Tables(tableName)
        ddl.DataTextField = "ComboItem"
        ddl.DataValueField = "ComboItemID"       
        ddl.DataBind()
    End Sub


    Private Sub Report_Run()

        Dim html As New StringBuilder()
        ds = Data_Load(Nothing)

        If CheckBoxIncludeTour.Checked Then
            Data_Load(New KeyValuePair(Of String, String())("0", New String() {}))
        Else
            Data_Load(New KeyValuePair(Of String, String())("1", New String() {}))
        End If


        Me.reportsection.InnerHtml = String.Empty


        If ds.Tables("Result").Rows.Count > 0 Then
            Dim col = ds.Tables("Result").AsEnumerable().AsQueryable().Select(Function(x) x.Item(0).ToString())
            Data_Load(New KeyValuePair(Of String, String())("Prospect", col.ToArray()))

            html.AppendFormat("<table>")

            If CheckBoxIncludeTour.Checked Then

                html.AppendFormat( _
                    "<tr>" & _
                    "<td {0}>Reservation ID</td>" & _
                    "<td {0}>Reservation #</td>" & _
                    "<td {0}>Date Booked</td>" & _
                    "<td {0}>Check In</td>" & _
                    "<td {0}>Check Out</td>" & _
                    "<td {0}>Nights #</td>" & _
                    "<td {0}>Hotel</td>" & _
                    "<td {0}>Status</td>" & _
                    "<td {0}>Prospect</td>" & _
                    "<td {0}>Source</td>" & _
                    "<td {0}>Campaign</td>" & _
                    "<td {0}>Tour Date</td>" & _
                    "<td {0}>Tour Time</td>" & _
                    "<td {0}>Personnel</td>" & _
                    "</tr>", _
                    "style=font-family:cambria;font-weight:bold;font-size:18px;")
            Else

                html.AppendFormat( _
                                 "<tr>" & _
                                    "<td {0}>Reservation ID</td>" & _
                                    "<td {0}>Reservation #</td>" & _
                                    "<td {0}>Date Booked</td>" & _
                                    "<td {0}>Check In</td>" & _
                                    "<td {0}>Check Out</td>" & _
                                    "<td {0}>Nights #</td>" & _
                                    "<td {0}>Hotel</td>" & _
                                    "<td {0}>Status</td>" & _
                                    "<td {0}>Prospect</td>" & _
                                    "<td {0}>Source</td>" & _
                                    "</tr>", _
                                    "style=font-family:cambria;font-weight:bold;font-size:18px;")
            End If


            

            For Each r As DataRow In ds.Tables("Result").Rows

                Dim prospect As String = String.Empty                
                Dim source As String = String.Empty
                Dim personnel As String = String.Empty
                Dim tourdate As String = String.Empty
                Dim tourtime As String = String.Empty
                Dim campaign As String = String.Empty


                Dim row As DataRow = Nothing

                row = ds.Tables("Prospect").Rows.Find(r.Item("prospectID").ToString())

                If row IsNot Nothing Then
                    prospect = String.Format("{0}, {1}", row.Item("LastName").ToString(), row.Item("FirstName").ToString())
                End If

                row = ds.Tables("ReservationSource").Rows.Find(r.Item("sourceid").ToString())
                If row IsNot Nothing Then
                    source = String.Format("{0}", row.Item("comboitem").ToString().Trim())
                End If


                If CheckBoxIncludeTour.Checked Then

                    personnel = String.Format("{0}", _
                                                    IIf(r.Item("Personnel") Is DBNull.Value, "", r.Item("Personnel")))
                    tourdate = String.Format("{0}", _
                                IIf(r.Item("tourdate") Is DBNull.Value, "", r.Item("tourdate")))

                    row = ds.Tables("TourTime").Rows.Find(r.Item("tourtime").ToString())
                    If row IsNot Nothing Then
                        tourtime = String.Format("{0}", row.Item("comboitem").ToString().Trim())
                    End If                    

                    campaign = String.Format("{0}", _
                                IIf(r.Item("name") Is DBNull.Value, "", r.Item("name")))



                    html.AppendFormat( _
                        "<tr>" & _
                        "<td {12}><a href=../../Marketing/EditReservation.aspx?ReservationID={0}>{0}</a></td>" & _
                        "<td>{1}</td>" & _
                        "<td>{2}</td>" & _
                        "<td>{3}</td>" & _
                        "<td {13}>{4}</td>" & _
                        "<td>{5}</td>" & _
                        "<td {15}>{6}</td>" & _
                        "<td {14}>{7}</td>" & _
                        "<td>{8}</td>" & _
                        "<td {16}>{9}</td>" & _
                        "<td>{10}</td>" & _
                        "<td>{11}</td>" & _
                        "<td>{12}</td>" & _
                        "<td>{13}</td>" & _
                        "</tr>", _
                        r.Item("ReservationID").ToString().Trim(), _
                        r.Item("ReservationNumber").ToString().Trim(), _
                        DateTime.Parse(r.Item("DateBooked")).ToShortDateString(), _
                        DateTime.Parse(r.Item("CheckInDate")).ToShortDateString(), _
                        DateTime.Parse(r.Item("CheckOutDate")).ToShortDateString(), _
                        r.Item("Nights"), _
                        DDL_Hotel.SelectedItem.Text, _
                        DDL_Status.SelectedItem.Text, _
                        prospect, _
                        source, _
                        campaign, _
                        DateTime.Parse(tourdate).ToShortDateString(), _
                        tourtime, _
                        personnel, _
                        "style=font-family:cambria;font-weight:bold;", _
                        "style=font-family:cambria;font-weight:bold;color:red;", _
                        "style=font-family:cambria;font-weight:bold;color:#153e7e;width:120px;", _
                        "style=font-family:cambria;font-weight:bold;color:#800517;width:120px;", _
                        "style=font-family:cambria;font-weight:bold;color:#48cccd;width:120px;")


                Else

                    html.AppendFormat( _
                        "<tr>" & _
                        "<td {12}><a href=../../Marketing/EditReservation.aspx?ReservationID={0}>{0}</a></td>" & _
                        "<td>{1}</td>" & _
                        "<td>{2}</td>" & _
                        "<td>{3}</td>" & _
                        "<td {13}>{4}</td>" & _
                        "<td>{5}</td>" & _
                        "<td {12}>{6}</td>" & _
                        "<td {14}>{7}</td>" & _
                        "<td>{8}</td>" & _
                        "<td>{9}</td>" & _
                        "</tr>", _
                        r.Item("ReservationID").ToString().Trim(), _
                        r.Item("ReservationNumber").ToString().Trim(), _
                        DateTime.Parse(r.Item("DateBooked")).ToShortDateString(), _
                        DateTime.Parse(r.Item("CheckInDate")).ToShortDateString(), _
                        DateTime.Parse(r.Item("CheckOutDate")).ToShortDateString(), _
                        r.Item("Nights"), _
                        DDL_Hotel.SelectedItem.Text, _
                        DDL_Status.SelectedItem.Text, _
                        prospect, _
                        source, _
                        "style=font-family:cambria;font-weight:bold;", _
                        "style=font-family:cambria;font-weight:bold;color:red;", _
                        "style=font-family:cambria;font-weight:bold;color:#153e7e;width:120px;", _
                        "style=font-family:cambria;font-weight:bold;color:#800517;width:120px;", _
                        "style=font-family:cambria;font-weight:bold;color:#48cccd;width:120px;")
                End If
                




            Next
            
            html.AppendFormat("</table>")
            Me.reportsection.InnerHtml = html.ToString()
        Else
            Me.reportsection.InnerHtml = String.Format("<h1>{0}</h1>", "No matches")
        End If
    End Sub
  
    Protected Sub BTN_Run_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Run.Click

        Me.reportsection.InnerHtml = String.Empty

        If String.IsNullOrEmpty(DF_CheckIn.Selected_Date) = False And _
            String.IsNullOrEmpty(DF_CheckOut.Selected_Date) = False And _
            String.IsNullOrEmpty(DF_BookedFrom.Selected_Date) = False And _
            String.IsNullOrEmpty(DF_BookedTo.Selected_Date) = False Then

            Report_Run()
        Else
            Me.reportsection.InnerHtml = "<h2 style=color:red>All fields are required.</h2>"
        End If
    End Sub
End Class
