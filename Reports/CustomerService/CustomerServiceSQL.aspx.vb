Imports System
Imports System.Linq
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Script.Serialization

Partial Class Reports_CustomerService_CustomerServiceSQL
    Inherits System.Web.UI.Page

    Dim cnx As String = Resources.Resource.cns


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim reportName As String = Request.QueryString("reportname")
        Dim year As String = Request.QueryString("Year")

        If String.IsNullOrEmpty(reportName) = False Then
            If reportName.Equals("UnUsedTime") And Integer.Parse(year) > 0 Then

                Response.Write(GetUnUsedTime(Integer.Parse(year)))
            End If
        End If
    End Sub




    Private Function GetUnUsedTime(ByVal year As Integer) As String

        Dim sql As String = String.Format("select co.ContractNumber, a.SaleType,s.comboitem as Season,co.ContractDate, a.Rooms, b.RoomsUsed, Case when b.RoomsUsed is null then 0 else b.roomsused end as roomsused, " & _
             "	(select top 1 Email from t_ProspectEmail where ProspectID = p.ProspectID And IsPrimary = 1 And IsActive = 1) As " & _
             " EmailAddress from ( " & _
             "select c.contractid,v.saletype,sum(cast( " & _
             "						case when v.bd = 'Combo' then " & _
             "							5 " & _
             "						when v.bd = 'Unknown' then " & _
             "							0 " & _
             "						else  " & _
             "							left(v.bd,1) " & _
             "						end " & _
             "						 as int)) as Rooms " & _
             "from t_Contract c " & _
             "	inner join t_Frequency f on f.frequencyid = c.frequencyid " & _
             "	inner join v_Contractinventory v on v.contractid = c.contractid " & _
             "where year(c.occupancydate) <= '{0}' " & _
             "	and c.contractnumber not like 't%' and c.contractnumber not like 'u%' and ({0} - year(c.occupancydate)) % f.interval = 0 " & _
             "group by c.contractid, v.saletype " & _
             ") a left outer join ( " & _
             "select left(ut.ComboItem, 1) as roomsused, u.contractid " & _
             "from t_Usage u " & _
             "	inner join t_Comboitems ut on ut.comboitemid = u.roomtypeid " & _
			 "  inner join t_Combos co on ut.ComboID = co.ComboID " & _
             "	inner join t_Comboitems us on us.comboitemid = u.statusid " & _
             "where u.usageyear = '{0}' and us.comboitem = 'used' and ut.Active = 1 and co.ComboName = 'RoomType' " & _
             "group by ut.ComboItem, u.contractid " & _
             ") b on b.contractid = a.contractid  " & _
             "inner join t_contract co on co.contractid = a.contractid " & _
             "inner join t_Prospect p on p.prospectid = co.prospectid " & _
             "left outer join t_Comboitems s on s.comboitemid = co.seasonid " & _
             "where b.roomsused is null or a.rooms <> b.roomsused " & _
             "order by saletype", year)

        Dim html As New StringBuilder()

        Using cnn As New SqlConnection(cnx)
            Using ada As New SqlDataAdapter(sql, cnn)

                Dim ds As New DataSet()
                Dim summary As String = String.Empty

                ada.Fill(ds, "UnUsed")

                html.Append("<table style='border-collapse:collapse;' id='htmlReport' border='1px'>")
                Dim rows = ds.Tables("UnUsed").AsEnumerable().GroupBy(Function(e) e.Field(Of String)("SaleType"))
                For Each r As IGrouping(Of String, DataRow) In rows

                    html.AppendFormat("<tr><td>&nbsp;</td><td colspan='6'><h2>{0}</h2></td></tr>", r.Key)

                    Dim num As Integer = 1

                    For Each c As DataRow In r

                        Dim rooms As String = String.Empty
                        Dim roomsUsed As String = String.Empty
                        Dim contractDate As String = String.Empty


                        If IsDBNull(c.Item("Rooms")) = False Then
                            rooms = c.Field(Of Integer)("Rooms")
                        End If

                        If IsDBNull(c.Item("RoomsUsed")) = False Then
                            roomsUsed = c.Field(Of Integer)("RoomsUsed")
                        End If


                        If IsDBNull(c.Item("ContractDate")) = False Then
                            contractDate = DateTime.Parse(c.Field(Of DateTime)("ContractDate")).ToShortDateString()
                        End If

                        Dim column As String = String.Format("<tr><td>{6}</td><td>{0}</td><td>{1}</td><td>{2}</td>" & _
                                                             "<td>{3}</td><td>{4}</td><td>{5}</td></tr>", _
                                                             c.Item("ContractNumber"), _
                                                             c.Item("Season"), _
                                                             contractDate, _
                                                             rooms, _
                                                             roomsUsed, _
                                                             c.Item("EmailAddress"), _
                                                             num.ToString())
                        html.Append(column)

                        num += 1
                    Next
                    summary += String.Format("<span style='width:50px;padding:25px;'>{0}</span><span>{1}</span><br/>", r.Key, r.Count())
                Next

                summary = String.Format("<br/></br/><div id='summary'>{0}</div><br/>", summary)
                html.Append(summary)                                  
            End Using

            Return IIf(String.IsNullOrEmpty(html.ToString()), "No Records", html.ToString())
        End Using
    End Function




End Class
