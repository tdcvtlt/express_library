Imports System.Linq
Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml.Linq


Partial Class Reports_Sales_CZAR_SalesSummary
    Inherits System.Web.UI.Page

    Private Structure Record
        Public Source As String
        Public Tours As Int32
        Public Sales As Int32
        Public SalesVolume As Decimal
        Public Ow As Int32
        Public OwVolume As Decimal
        Public SalesTotal As Int32
        Public VolumeTotal As Decimal
        Public VPG As Decimal
        Public Penders As Int32
        Public PendersVolume As Decimal
        Public Active As Boolean
    End Structure


    Private CZarRecord As List(Of Record) = New List(Of Record)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



    End Sub


    Private Sub ToClient(ByVal date_start As String, ByVal date_end As String, ByVal ow_included As Boolean)

        Dim con_string = "data source=RS-SQL-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096; "

        con_string = Resources.Resource.cns

        Dim cnn = New SqlConnection(con_string)

        Dim sql_tour = String.Format( _
            "select DISTINCT(T.TOURID), Prospect.SourceID, (Select ComboItem from t_ComboItems where ComboItemId = Prospect.SourceID) [Source] " & _
            "FROM T_TOUR T " & _
           "INNER JOIN T_COMBOITEMS TS ON TS.COMBOITEMID = T.STATUSID " & _
           "LEFT OUTER JOIN T_CONTRACT C ON C.TOURID = T.TOURID " & _
           "LEFT OUTER JOIN T_COMBOITEMS TST ON TST.COMBOITEMID = T.SUBTYPEID " & _
           "INNER JOIN T_PROSPECT PROSPECT " & _
           "ON T.PROSPECTID = PROSPECT.PROSPECTID		" & _
           "WHERE T.TOURDATE BETWEEN '{0}' AND '{1}' " & _
           "AND TST.COMBOITEM <> 'EXIT'" & _
           "AND (TS.COMBOITEM IN ('SHOWED', 'ONTOUR', 'NO TOUR - OVERAGE') OR C.CONTRACTID IS NOT NULL) " & _
           "AND T.CAMPAIGNID = 5420	", date_start, date_end)

        cnn.Open()
        Dim cmd = New SqlCommand(sql_tour, cnn)
        Dim rdr = cmd.ExecuteReader()
        Dim dic_sourceId_tourId As Dictionary(Of String, String()) = _
            New Dictionary(Of String, String())

        Dim html_table As New StringBuilder()


        Dim __Tours = String.Format( _
            "SELECT DISTINCT T.TOURID " & _
             "FROM T_TOUR T " & _
             "	INNER JOIN T_COMBOITEMS TS ON TS.COMBOITEMID = T.STATUSID " & _
             "	LEFT OUTER JOIN T_CONTRACT C ON C.TOURID = T.TOURID " & _
             "WHERE T.TOURDATE BETWEEN '{0}' AND '{1}' " & _
             "	AND (TS.COMBOITEM IN ('SHOWED', 'ONTOUR', 'NO TOUR - OVERAGE') OR C.CONTRACTID IS NOT NULL) " & _
             "	AND T.CAMPAIGNID = 5420", date_start, date_end)

        Dim vb_contract_sold_count = String.Format("SELECT COUNT(DISTINCT C.CONTRACTID) " & _
                               "FROM T_CONTRACT C " & _
                                "INNER JOIN T_COMBOITEMS CS ON CS.COMBOITEMID = C.STATUSID " & _
                                "INNER JOIN T_SOLDINVENTORY S ON S.CONTRACTID = C.CONTRACTID " & _
                                "INNER JOIN T_PROSPECT Prospect ON " & _
                                "C.ProspectID = Prospect.ProspectID " & _
                               "WHERE C.CONTRACTDATE BETWEEN '{0}' AND '{1}' " & _
                                "AND C.TOURID IN ( {2} " & _
                                ") AND CS.COMBOITEM IN ('ACTIVE','SUSPENSE','DEVELOPER')", date_start, date_end, __Tours)



        Dim vb_contract_ow_count = String.Format("SELECT COUNT(DISTINCT C.CONTRACTID) " & _
                        "FROM T_CONTRACT C " & _
                        "	INNER JOIN T_TOUR T ON T.TOURID = C.TOURID " & _
                        "	INNER JOIN T_COMBOITEMS CS ON CS.COMBOITEMID = C.STATUSID " & _
                        "	INNER JOIN T_SOLDINVENTORY S ON S.CONTRACTID = C.CONTRACTID " & _
                        " INNER JOIN T_PROSPECT PROSPECT ON C.PROSPECTID = PROSPECT.PROSPECTID " & _
                        "WHERE C.CONTRACTDATE < '{0}' " & _
                        "	AND OriginallyWrittenDate BETWEEN '{0}' AND '{1}' " & _
                        "	AND C.STATUSDATE BETWEEN '{0}' AND '{1}' " & _
                        "	AND T.CAMPAIGNID = 5420 " & _
                        "	AND CS.COMBOITEM IN ('ACTIVE', 'SUSPENSE','DEVELOPER') " & _
                        "	AND t.SUBTYPEID <> '17179' ", date_start, date_end)



        Dim vb_contract_sold_vol = String.Format("SELECT COALESCE(SUM(M.SALESVOLUME), 0) " & _
                   "FROM T_MORTGAGE M " & _
                   "WHERE M.CONTRACTID IN ( " & _
                   "	SELECT DISTINCT C.CONTRACTID " & _
                   "	FROM T_CONTRACT C " & _
                   "		INNER JOIN T_COMBOITEMS CS ON CS.COMBOITEMID = C.STATUSID " & _
                   "		INNER JOIN T_SOLDINVENTORY S ON S.CONTRACTID = C.CONTRACTID " & _
                    "INNER JOIN T_PROSPECT Prospect ON " & _
                    "C.ProspectID = Prospect.ProspectID " & _
                   "	WHERE C.CONTRACTDATE BETWEEN '{0}' AND '{1}' " & _
                   "		AND C.TOURID IN ( {2} " & _
                   "		) AND CS.COMBOITEM IN ('ACTIVE','SUSPENSE','DEVELOPER'))", date_start, date_end, __Tours)



        Dim vb_contract_ow_vol = String.Format("SELECT COALESCE(SUM(M.SALESVOLUME), 0) " & _
                "FROM T_MORTGAGE M " & _
                "WHERE M.CONTRACTID IN ( " & _
                "	SELECT DISTINCT C.CONTRACTID " & _
                "	FROM T_CONTRACT C  " & _
                "		INNER JOIN T_TOUR T ON T.TOURID = C.TOURID " & _
                "		INNER JOIN T_COMBOITEMS CS ON CS.COMBOITEMID = C.STATUSID " & _
                "		INNER JOIN T_SOLDINVENTORY S ON S.CONTRACTID = C.CONTRACTID " & _
                " INNER JOIN T_PROSPECT PROSPECT ON C.PROSPECTID = PROSPECT.PROSPECTID " & _
                "	WHERE C.CONTRACTDATE < '{0}'  " & _
                "		AND OriginallyWrittenDate BETWEEN '{0}' AND '{1}' " & _
                "		AND C.STATUSDATE BETWEEN '{0}' AND '{1}' " & _
                "		AND T.CAMPAIGNID = 5420 " & _
                "		AND CS.COMBOITEM IN ('ACTIVE', 'SUSPENSE','DEVELOPER') ", date_start, date_end)


        Dim vb_contract_pender_count = String.Format("SELECT COUNT(DISTINCT C.CONTRACTID) " & _
               "FROM T_CONTRACT C " & _
               "	INNER JOIN T_COMBOITEMS CS ON CS.COMBOITEMID = C.STATUSID " & _
               " INNER JOIN T_PROSPECT prospect ON C.ProspectID = prospect.ProspectID " & _
               "WHERE C.CONTRACTDATE BETWEEN '{0}' AND '{1}' " & _
               "	AND C.TOURID IN ( {2} " & _
               "	) AND (CS.COMBOITEM = 'PENDER' OR CS.COMBOITEM = 'PENDER-INV') " & _
               "	AND C.CONTRACTNUMBER NOT LIKE 'T%' " & _
               "	AND C.CONTRACTNUMBER NOT LIKE 'U%'", date_start, date_end, __Tours)


        Dim vb_contract_pender_vol = String.Format("SELECT COALESCE(SUM(M.SALESVOLUME), 0) " & _
               "FROM T_MORTGAGE M " & _
               "WHERE M.CONTRACTID IN ( " & _
               "	SELECT DISTINCT C.CONTRACTID " & _
               "	FROM T_CONTRACT C " & _
               "		INNER JOIN T_COMBOITEMS CS ON CS.COMBOITEMID = C.STATUSID " & _
               " INNER JOIN T_PROSPECT prospect On prospect.prospectID = c.ProspectID " & _
               "	WHERE C.CONTRACTDATE BETWEEN '{0}' AND '{1}' " & _
               "		AND C.TOURID IN ( {2} " & _
               "		) AND (CS.COMBOITEM = 'PENDER' OR CS.COMBOITEM = 'PENDER-INV') " & _
               "		AND C.CONTRACTNUMBER NOT LIKE 'T%' " & _
               "		AND C.CONTRACTNUMBER NOT LIKE 'U%' ", date_start, date_end, __Tours)

        If rdr.HasRows = True Then

            Dim tours_id() As String = Nothing
            html_table.AppendFormat("<table border=1 id=table_result>")
            html_table.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td>" & _
                                    "<td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td></tr>", _
                                    "Source", "Tours", "Sales", "Sales Volumn", "OW", "OW Volumn", "Total Sales", _
                                    "Total Volumn", "VPG", "Penders", "Pender Volumn")
            Do While rdr.Read()

                Dim key As String = Nothing

                If IsDBNull(rdr("SourceID")) Then
                    key = "0"
                Else
                    key = rdr("SourceID")
                End If


                If dic_sourceId_tourId.ContainsKey(key) Then

                    tours_id = dic_sourceId_tourId(key)

                    Array.Resize(tours_id, tours_id.GetLength(0) + 1)
                    tours_id(tours_id.GetLength(0) - 1) = rdr("TourID").ToString()

                    dic_sourceId_tourId(key) = tours_id
                Else

                    dic_sourceId_tourId.Add(key, New String() {rdr("TourID")})
                End If
            Loop



            Dim grand_sold_count As Integer = 0
            Dim grand_sold_vol As Decimal = 0
            Dim grand_ow_count As Integer = 0
            Dim grand_ow_vol As Decimal = 0
            Dim grand_pender_count As Integer = 0
            Dim grand_pender_vol As Decimal = 0



            Dim grand_total_sales As Integer = 0
            Dim grand_total_volumn As Decimal = 0
            Dim grand_vpg As Decimal = 0



            For Each row As KeyValuePair(Of String, String()) In dic_sourceId_tourId

                Literal_Result.Text += String.Format("</br>{0} - {1}", row.Key, String.Join(",", _
                                                                    row.Value.Cast(Of String)().ToArray()))

                Dim cnn_sold_count = New SqlConnection(con_string)
                cnn_sold_count.Open()

                Dim source = row.Key

                If (String.IsNullOrEmpty(source)) Then
                    source = 0
                End If



                Dim r = New Record()
                r.Source = row.Key
                r.Tours = row.Value.Count





                Dim sql_01 = String.Format("{0} AND Prospect.SourceID = {1}", _
                                        vb_contract_sold_count, source)

                Dim cmd_sold_count = New SqlCommand(sql_01, cnn_sold_count)


                Dim sql_02 = String.Format("SELECT COALESCE(SUM(M.SALESVOLUME), 0) " & _
                                           "FROM T_MORTGAGE M " & _
                   "WHERE M.CONTRACTID IN ( " & _
                   "	SELECT DISTINCT C.CONTRACTID " & _
                   "	FROM T_CONTRACT C " & _
                   "		INNER JOIN T_COMBOITEMS CS ON CS.COMBOITEMID = C.STATUSID " & _
                   "		INNER JOIN T_SOLDINVENTORY S ON S.CONTRACTID = C.CONTRACTID " & _
                    "INNER JOIN T_PROSPECT Prospect ON " & _
                    "C.ProspectID = Prospect.ProspectID " & _
                   "	WHERE C.CONTRACTDATE BETWEEN '{0}' AND '{1}' " & _
                   "		AND C.TOURID IN ( {2} " & _
                    "		) AND CS.COMBOITEM IN ('ACTIVE','SUSPENSE','DEVELOPER') " & _
                    "AND PROSPECT.SourceID = {3} )", date_start, date_end, __Tours, row.Key)


                Dim cmd_sold_vol = New SqlCommand(sql_02, cnn_sold_count)

                Dim sql_03 = String.Format("{0} And prospect.SourceID = {1}", _
                                           vb_contract_ow_count, row.Key)


                Dim cmd_ow_count = New SqlCommand(sql_03, cnn_sold_count)

                Dim sql_04 = String.Format("{0} And prospect.SourceID = {1})", _
                                           vb_contract_ow_vol, row.Key)

                Dim cmd_ow_vol = New SqlCommand(sql_04, cnn_sold_count)


                Dim sql_05 = String.Format("{0} And Prospect.SourceID = {1}", _
                                vb_contract_pender_count, _
                                row.Key)

                Dim cmd_pender_count = New SqlCommand(sql_05, cnn_sold_count)

                Dim sql_06 = String.Format("{0} AND Prospect.SourceID = {1})", _
                                           vb_contract_pender_vol, row.Key)

                Dim cmd_pender_vol = New SqlCommand(sql_06, cnn_sold_count)
                
                r.Sales = cmd_sold_count.ExecuteScalar()

                If ow_included Then

                    r.Ow = cmd_ow_count.ExecuteScalar()
                    r.OwVolume = Decimal.Round(cmd_ow_vol.ExecuteScalar(), 2, MidpointRounding.AwayFromZero)

                End If


                r.SalesVolume = Decimal.Round(cmd_sold_vol.ExecuteScalar(), 2, MidpointRounding.AwayFromZero)

                r.SalesTotal = r.Sales + r.Ow
                r.VolumeTotal = r.SalesVolume + r.OwVolume
                r.VPG = Decimal.Round((r.SalesVolume + r.OwVolume) / r.Tours, 2, MidpointRounding.AwayFromZero)

                r.Penders = cmd_pender_count.ExecuteScalar()
                r.PendersVolume = Decimal.Round(cmd_pender_vol.ExecuteScalar(), 2, MidpointRounding.AwayFromZero)


                Dim cmd_source_lookup = New SqlCommand(String.Format( _
                    "select top 1 ComboItem, Active from t_ComboItems where comboItemid in (" + row.Key + ")"), cnn_sold_count)

                Dim source_name = cmd_source_lookup.ExecuteReader()

                If source_name.HasRows Then
                    source_name.Read()

                    If source_name("Active") = True Then
                        r.Active = True
                    Else
                        r.Active = False
                    End If

                    r.Source = source_name("ComboItem")
                Else
                    r.Active = False
                    r.Source = "N/A"
                End If

                source_name.Close()
                cnn_sold_count.Close()


                CZarRecord.Add(r)
            Next



            For Each g In CZarRecord.OrderBy(Function(x) x.Active).GroupBy(Function(x) x.Active)

                Dim source As String = "N/A"

                If g.Key.Equals(True) Then
                    source = g.First().Source
                End If

                html_table.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td>" & _
                                        "<td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td></tr>", _
                                source, _
                                g.Sum(Function(x) x.Tours), _
                                g.Sum(Function(x) x.Sales), _
                                String.Format("{0:N2}", g.Sum(Function(x) x.SalesVolume)), _
                                g.Sum(Function(x) x.Ow), _
                                String.Format("{0:N2}", g.Sum(Function(x) x.OwVolume)), _
                                g.Sum(Function(x) x.SalesTotal), _
                                String.Format("{0:N2}", g.Sum(Function(x) x.VolumeTotal)), _
                                String.Format("{0:N2}", Decimal.Round(g.Sum(Function(x) x.VolumeTotal / g.Sum(Function(y) y.Tours)), 2, MidpointRounding.AwayFromZero)), _
                                g.Sum(Function(x) x.Penders), _
                                String.Format("{0:N2}", g.Sum(Function(x) x.PendersVolume)))

            Next

            html_table.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td>" & _
                                       "<td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td></tr>", _
                               String.Empty, _
                               CZarRecord.Sum(Function(x) x.Tours), _
                               CZarRecord.Sum(Function(x) x.Sales), _
                               String.Format("{0:N2}", CZarRecord.Sum(Function(x) x.SalesVolume)), _
                               CZarRecord.Sum(Function(x) x.Ow), _
                               String.Format("{0:N2}", CZarRecord.Sum(Function(x) x.OwVolume)), _
                               CZarRecord.Sum(Function(x) x.SalesTotal), _
                               String.Format("{0:N2}", CZarRecord.Sum(Function(x) x.VolumeTotal)), _
                               String.Format("{0:N2}", Decimal.Round(CZarRecord.Sum(Function(x) x.VolumeTotal / CZarRecord.Sum(Function(y) y.Tours)), 2, MidpointRounding.AwayFromZero)), _
                               CZarRecord.Sum(Function(x) x.Penders), _
                               String.Format("{0:N2}", CZarRecord.Sum(Function(x) x.PendersVolume)))

            html_table.AppendFormat("</table>")

            Literal_Result.Text = html_table.ToString()

        Else
            Literal_Result.Text = "<h1>Records Not Found!</h1>"
        End If

        cnn.Close()
    End Sub

    Protected Sub btRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRunReport.Click

        If String.IsNullOrEmpty(dpFrom.Selected_Date) = False And _
            String.IsNullOrEmpty(dpTo.Selected_Date) = False Then

            ToClient(dpFrom.Selected_Date, dpTo.Selected_Date, CheckBoxOW.Checked)
        End If
    End Sub
End Class




