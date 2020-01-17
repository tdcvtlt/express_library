Imports System.Data
Imports System.IO
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.Linq



Partial Class Reports_Tours_TourDetail
    Inherits System.Web.UI.Page

    Private cnx As String = Resources.Resource.cns
    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then

            Dim list() As ListItem = { _
                New ListItem With {.Text = "KCP", .Value = "16975"}, _
                New ListItem With {.Text = "Richmond", .Value = "16983"}, _
                New ListItem With {.Text = "Vacation Club", .Value = "18070"}, _
                New ListItem With {.Text = "Woodbridge", .Value = "24282"} _
                }

            ddLocation.Items.AddRange(list.ToArray())
            ddLocation.DataBind()

        End If
    End Sub

    Protected Sub btRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRunReport.Click

        Dim date_start As DateTime = IIf(String.IsNullOrEmpty(ucDateStart.Selected_Date), DateTime.MaxValue, ucDateStart.Selected_Date)
        Dim date_end As DateTime = IIf(String.IsNullOrEmpty(ucDateEnd.Selected_Date), DateTime.MaxValue, ucDateEnd.Selected_Date)

        If date_start = DateTime.MaxValue Or date_end = DateTime.MaxValue Then Return

        Dim sql As String = String.Empty
        Dim locationId As String = ddLocation.SelectedItem.Value
        Dim sb As New StringBuilder
        Dim html As New StringBuilder()

        html.Append("<table style='border-collapse:collapse' border='1px'>")

        Do While DateTime.Compare(date_start, date_end) <= 0

            sql = String.Format( _
                     "Select z.*, y.ComboItem as tourstatus from " & _
                     "(Select a.*, b.Name from " & _
                     "(Select t.tourid, t.statusid, tt.comboItem [Tourtime], t.tourdate, " & _
                     "p.FirstName, p.lastname, t.CampaignID from t_Tour t " & _
                     "inner join t_Prospect p on t.prospectid = p.prospectid " & _
                     "left join t_comboItems tt on tt.comboitemid = t.tourtime where " & _
                     "tourdate = '{0}' and tourLocationid IN ({1})  " & _
                     "and T.subtypeid <> '17179') a inner join " & _
                     "t_Campaign b on a.CampaignID = b.CampaignID) z  " & _
                     "inner join t_ComboItems y on z.statusid = y.comboitemid " & _
                     "where z.name <> 'Outfield' order by z.TourDate, z.Name, z.tourid", _
                     date_start, locationId)

            Using cnn As New SqlConnection(Resources.Resource.cns)
                Using ada As New SqlDataAdapter(sql, cnn)

                    Dim ds As New DataSet()
                    ada.Fill(ds, "tours")

                    Dim tours_id As IEnumerable(Of String) = From t As DataRow In ds.Tables("tours").AsEnumerable() _
                                                             Select New String(t.Item("tourid").ToString())

                    Dim tours_id_str As String = String.Join(",", tours_id.ToArray())

                    If tours_id.Count() > 0 Then

                        sql = "Select A.KEYVALUE, p.FirstName, p.LastName from t_Personnel p inner join " & _
                                "t_PersonnelTrans a on a.personnelid = p.personnelid " & _
                                "where a.titleid = '16798' and " & _
                                "a.KEYFIELD = 'TOURID' AND A.KEYVALUE IN (" & tours_id_str & ")"

                        ada.SelectCommand = New SqlCommand(sql, cnn)
                        ada.Fill(ds, "Personnels")

                        Dim dr_tours_personnels As New DataRelation("dr_tours_personnels", _
                                                   ds.Tables("Tours").Columns("TourID"), _
                                                   ds.Tables("Personnels").Columns("KeyValue"))

                        ds.Relations.Add(dr_tours_personnels)

                        sql = "Select i.*, Case when i.DateIssued is null then '' else i.DateIssued end as PremDateIssued, (select comboitem from t_comboitems cb where cb.comboitemid = i.statusid) Status, " & _
                                "P.PremiumName from t_PremiumIssued i " & _
                                "inner join t_Premium p on i.premiumid = p.premiumid  " & _
                                "where I.KEYFIELD = 'TOURID' AND I.KEYVALUE in ( " & tours_id_str & ")"

                        ada.SelectCommand = New SqlCommand(sql, cnn)
                        ada.Fill(ds, "Premiums")

                        Dim dr_tours_premiums As New DataRelation("dr_tours_premiums", _
                                                                  ds.Tables("Tours").Columns("tourid"), _
                                                                  ds.Tables("Premiums").Columns("keyvalue"))

                        ds.Relations.Add(dr_tours_premiums)

                        Dim g_date = From xl In ds.Tables("Tours").AsEnumerable().GroupBy(Function(x) x.Field(Of Date)("TourDate"))

                        For Each d In g_date

                            html.AppendFormat("<tr><td colspan='6' style='color:red'><h3>{0}</h3></td></tr>", Date.Parse(d.Key).ToShortDateString())

                            Dim g_campaign = From c In d.GroupBy(Function(x) x.Field(Of String)("Name"))

                            For Each campaigns As IGrouping(Of String, DataRow) In g_campaign

                                html.AppendFormat("<tr><td colspan='6' style='color:blue'><h5>{0}</h5></td></tr>", campaigns.Key)

                                For Each r As DataRow In campaigns
                                    html.AppendFormat("<tr><td colspan='2'>{0}</td>", _
                                                r.Field(Of Integer)("TourID"))

                                    html.AppendFormat("<td>{0}</td>", r.Field(Of String)("LastName") & ",  " & _
                                                        r.Field(Of String)("FirstName"))
                                    html.AppendFormat("<td>{0}</td>", r.Field(Of String)("TourStatus"))

                                    html.AppendFormat("<td>{0}</td>", r.Field(Of String)("TourTime"))

                                    If r.GetChildRows(dr_tours_personnels).Any() Then

                                        html.AppendFormat("<td>{0}</td>", _
                                                            r.GetChildRows(dr_tours_personnels).ElementAt(0).Item("FirstName") & ", " & _
                                                            r.GetChildRows(dr_tours_personnels).ElementAt(0).Item("LastName"))
                                    Else
                                        html.Append("<td>No Rep Assigned</td>")
                                    End If


                                    ' Premiums
                                    '

                                    Dim issDate As String
                                    If r.GetChildRows(dr_tours_premiums).Any() Then
                                        For Each dr_premium As DataRow In r.GetChildRows(dr_tours_premiums)

                                            Dim tmp As String = String.Empty

                                            If dr_premium.Field(Of Integer)("PremiumID") = 1606 Then
                                                tmp = String.Format("<td>Amount: {0:C2}  Number: {1}</td>", _
                                                    dr_premium.Field(Of Decimal)("TotalCost"), _
                                                    dr_premium.Field(Of String)("CertificateNumber"))
                                            End If

                                            If tmp.Length = 0 Then
                                                tmp = "<td/>"
                                            End If


                                            If dr_premium.Field(Of String)("Status") <> "Issued" Then
                                                issDate = ""
                                            Else
                                                issDate = dr_premium.Field(Of Date)("PremDateIssued").ToShortDateString
                                            End If

                                            html.AppendFormat("<tr><td colspan='2'/><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", _
                                                              dr_premium.Field(Of String)("PremiumName"), _
                                                              dr_premium.Field(Of Integer)("QtyIssued"), _
                                                              dr_premium.Field(Of String)("Status"), _
                                                               issDate)

                                            'tmp)
                                        Next
                                    Else
                                        html.AppendFormat("<tr><td>&nbsp;</td><td colspan='4'>{0}</td></tr>", "No Premium")
                                    End If
                                Next
                            Next
                        Next
                    End If
                End Using
            End Using

            date_start = date_start.AddDays(1)
        Loop

        html.Append("</table>")

        LIT.Text = html.ToString()

    End Sub

    Protected Sub btPrintable_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btPrintable.Click
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Printable", "var win=window.open();win.document.write('" & LIT.Text.Replace("'", "\'") & "');", True)
    End Sub
End Class
