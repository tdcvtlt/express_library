Imports System
Imports System.Linq
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Script.Serialization


Partial Class Reports_Contracts_ContractSQL
    Inherits System.Web.UI.Page

    Private cnx As String = Resources.Resource.cns.ToString()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.ContentType = "plain/text"

        Dim dateStart As String = Request.QueryString("start")
        Dim dateEnd As String = Request.QueryString("end")

        Dim statuses As String = Request.QueryString("Statuses")
        Dim personnels As String = Request.QueryString("Personnels")

        If String.IsNullOrEmpty(dateStart) = False And String.IsNullOrEmpty(dateEnd) = False And _
            String.IsNullOrEmpty(statuses) = False And String.IsNullOrEmpty(personnels) = False Then

            Report(statuses, personnels, dateStart, dateEnd)

        ElseIf String.IsNullOrEmpty(Request.QueryString("GetStatus")) Then
            Response.Write(GetContractStatus)
        ElseIf String.IsNullOrEmpty(Request.QueryString("GetPersonnel")) Then
            Response.Write(GetPersonnel)
        End If






    End Sub



    Private Function GetContractStatus() As String

        Dim sql As String = "select * from t_ComboItems a inner join t_combos b on a.comboid = " & _
                            "b.comboid where comboname = 'ContractStatus' order by ComboName "

        Dim js As String = String.Empty
        Using cnn As New SqlConnection(cnx)
            Using ada As New SqlDataAdapter(sql, cnn)
                Dim dt As New DataTable()
                ada.Fill(dt)

                Dim anonym As IEnumerable(Of IdName) = From e In dt.AsEnumerable() _
                                                       Select New IdName With {.ID = e.Field(Of Integer)("ComboItemID"), _
                                                                              .Name = e.Field(Of String)("ComboItem")}

                Dim seri As New JavaScriptSerializer()
                js = seri.Serialize(anonym.ToList())
            End Using
        End Using

        Return js
    End Function

    Private Function GetPersonnel() As String

        Dim sql As String = "select p.firstname + ' ' + p.lastname as Name, p.personnelid from " & _
                            "t_Personnel p where p.personnelid in (select distinct personnelid from t_PersonnelTrans " & _
                            "where titleid in (select comboitemid from t_Comboitems a inner join t_combos " & _
                            "b on a.comboid = b.comboid where comboname = 'personneltitle' and comboitem in ('Closer'))) " & _
                            "order by p.lastname, p.firstname"

        Dim js As String = String.Empty
        Using cnn As New SqlConnection(cnx)
            Using ada As New SqlDataAdapter(sql, cnn)
                Dim dt As New DataTable()
                ada.Fill(dt)

                Dim anonym As IEnumerable(Of IdName) = From e In dt.AsEnumerable() _
                                                       Select New IdName With {.ID = e.Field(Of Integer)("PERSONNELID"), _
                                                                              .Name = e.Field(Of String)("NAME")}

                Dim seri As New JavaScriptSerializer()
                js = seri.Serialize(anonym.ToList())
            End Using
        End Using

        Return js

    End Function







    Private Sub Report(ByVal status As String, _
                       ByVal person As String, _
                       ByVal startDate As DateTime, _
                       ByVal endDate As DateTime)

        Dim sql As String = String.Empty
        Using cnn As New SqlConnection(cnx)

            sql = String.Format( _
            "select c.contractnumber, c.contractdate, st.comboitem as SaleType, ct.comboitem as ContractType, " & _
            "p.lastname + ', ' + p.firstname as OwnerName, M.MORTGAGEID, " & _
            "m.salesvolume, m.salesprice, m.dptotal,  m.dptotal+(select convert(money, ISNULL(sum(amount), 0)) from v_payments where keyvalue=m.mortgageid) " & _
            "as balance, m.totalfinanced, status.comboitem as status, per.lastname + ', ' + per.firstname as Closer  " & _
           "from t_PersonnelTrans pt inner join t_Personnel per on per.personnelid = pt.personnelid  " & _
           "inner join t_ComboItems Title on Title.comboitemid = pt.titleid  " & _
           "inner join t_Contract c on c.contractid = pt.KEYVALUE AND PT.KEYFIELD = 'CONTRACTID'  " & _
           "inner join t_Mortgage m on m.contractid = c.contractid  " & _
           "left outer join t_ComboItems st on st.comboitemid = c.saletypeid  " & _
           "left outer join t_ComboItems ct on ct.comboitemid = c.typeid  " & _
           "inner join t_Prospect p on p.prospectid = c.prospectid  " & _
           "left outer join t_ComboItems status on status.comboitemid = c.statusid  " & _
           "where Title.comboitem in ( 'CLOSER' )  " & _
           "and pt.personnelid in ( {0} )  " & _
           "and (c.contractdate between '{1}' and '{2}' or c.statusdate between '{1}' and '{2}')  " & _
           "and status.comboitemid in ({3}) order by  per.lastname, per.firstname, status.comboitem", _
           status, startDate.ToShortDateString(), endDate.ToShortDateString(), person)


            Dim ada As New SqlDataAdapter(sql, cnn)
            Dim ds As New DataSet()

            ada.Fill(ds, "Parent")

            Dim rowsDistinct = ds.Tables("Parent").AsEnumerable() _
                               .Distinct(DataRowComparer.Default) _
                               .OrderBy(Function(e) e.Field(Of String)("Closer"))


            Dim closerGrouped = rowsDistinct.GroupBy(Function(e) e.Field(Of String)("Closer"))


            Dim unitsById = rowsDistinct.Select(Function(e) e.Field(Of String)("ContractNumber")).ToArray()

            Dim joined As String = String.Join("','", unitsById)

            sql = String.Format( _
            "Select C.ContractNumber, u.name, i.week from t_Unit u inner join " & _
            "t_Salesinventory i on i.unitid = u.unitid inner join " & _
            "t_Soldinventory s on s.salesinventoryid = i.salesinventoryid " & _
            "INNER JOIN t_Contract C ON C.ContractID = S.ContractID " & _
            "WHERE C.ContractNumber IN ('{0}')", joined)

            ada.SelectCommand = New SqlCommand(sql, cnn)
            ada.Fill(ds, "Child")


            Dim html As New StringBuilder()


            Dim closers = From index In ds.Tables("Parent") _
                          .AsEnumerable() _
                          .Select(Function(e) e.Field(Of String)("Closer")) _
                          .Distinct()

            Dim columns() As String = {"Contract Date", "KCP#", "Sale Type", "Contract Type", "Owner", _
                                       "Sales Volume", "Sales Price", "Down Payment", "Down Balance", "Amount Financed"}

            html.Append("<table style='border-collapse:collapse;' border='1px' id='resultpane'>")
            For Each g As IGrouping(Of String, DataRow) In closerGrouped


                html.AppendFormat("<tr><td colspan='11'><h2>{0}</h2></td></tr>", g.Key)

                Dim statusGrouped = g.GroupBy(Function(el) el.Field(Of String)("status"))

                For Each statusGr As IGrouping(Of String, DataRow) In statusGrouped

                    html.AppendFormat("<tr><td>&nbsp;</td><td colspan='10'><strong>{0}</strong></td></tr>", statusGr.Key)

                    html.Append("<tr class='bold'>")
                    For Each c As String In columns
                        html.AppendFormat("<td>{0}</td>", c)
                    Next
                    html.Append("</tr>")

                    For Each outer As DataRow In statusGr
                        Dim volumne As Decimal = 0
                        Dim price As Decimal = 0
                        Dim down As Decimal = 0
                        Dim balance As Decimal = 0
                        Dim financed As Decimal = 0


                        If outer.Field(Of Decimal)("SalesVolume").Equals(DBNull.Value) = False Then
                            volumne = outer.Field(Of Decimal)("SalesVolume")
                        End If

                        If outer.Field(Of Decimal)("salesprice").Equals(DBNull.Value) = False Then
                            price = outer.Field(Of Decimal)("salesprice")
                        End If

                        If outer.Field(Of Decimal)("dptotal").Equals(DBNull.Value) = False Then
                            down = outer.Field(Of Decimal)("dptotal")
                        End If

                        If outer.Field(Of Decimal)("BALANCE").Equals(DBNull.Value) = False Then
                            balance = outer.Field(Of Decimal)("balance")
                        End If

                        If outer.Field(Of Decimal)("totalfinanced").Equals(DBNull.Value) = False Then
                            financed = outer.Field(Of Decimal)("totalfinanced")
                        End If

                        html.AppendFormat("<tr><td>{0}</td>", outer.Field(Of DateTime)("ContractDate").ToShortDateString())
                        html.AppendFormat("<td>{0}</td>", outer.Field(Of String)("ContractNumber"))
                        html.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3:c2}</td>", outer.Field(Of String)("SaleType"), _
                                            outer.Field(Of String)("ContractType"), outer.Field(Of String)("OwnerName"), volumne)
                        html.AppendFormat("<td>{0:c2}</td><td>{1:c2}</td><td>{2:c2}</td>", price, down, balance)
                        html.AppendFormat("<td>{0:c2}</td>", financed)

                        Dim tmpRow As DataRow = outer
                        Dim inner = ds.Tables("Child").AsEnumerable().Where(Function(e) e.Field(Of String)("ContractNumber") = tmpRow.Field(Of String)("ContractNumber"))


                        For Each row As DataRow In inner
                            html.AppendFormat("<tr><td>&nbsp;</td><td colspan='10'>{0}</td></tr>", row.Field(Of String)("Name"))
                        Next

                        html.AppendFormat("</tr>")
                    Next

                    html.AppendFormat("<tr><td colspan='4'/><td><h4>{0}<h4></td><td colspan='6'><h4>{1:C2}<h4></td></tr>", statusGr.Count(), _
                                                     statusGr.Sum(Function(e) e.Field(Of Decimal)("SalesVolume")))

                Next

            Next

            html.Append("</table>")

            Response.Write(IIf(closerGrouped.Count = 0, "No Records", html.ToString()))
        End Using



    End Sub

End Class
