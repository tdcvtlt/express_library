Imports System
Imports System.Data
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.Text
Imports System.Linq



Partial Class Reports_Sales_SalesPersonEfficiencyReport
    Inherits System.Web.UI.Page


    Private ReadOnly cnx As String = Resources.Resource.cns




    Protected Class Efficiency
        Dim _name As String
        Dim _tours As String
        Dim _penders As String
        Dim _contracts() As Integer
        Dim _volume() As Integer


        Public Sub Efficiency()
        End Sub


        Public Property Name As String
            Set(ByVal value As String)
                _name = value
            End Set
            Get
                Return _name
            End Get
        End Property

        Public Property Tours As String
            Set(ByVal value As String)
                _tours = value
            End Set
            Get
                Return _tours
            End Get
        End Property

        Public Property Pender As String
            Set(ByVal value As String)
                _penders = value
            End Set
            Get
                Return _penders
            End Get
        End Property


        Public Property Contracts As Integer()
            Set(ByVal value As Integer())
                _contracts = value
            End Set
            Get
                Return _contracts
            End Get
        End Property


        Public Property Volume As Integer()
            Set(ByVal value As Integer())
                _volume = value
            End Set
            Get
                Return _volume
            End Get
        End Property

    End Class







    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            BindControl()
        Else

        End If

    End Sub





    Private Sub BindControl()

        Dim sql As String = "Select Distinct(ComboItem), COMBOITEMID from t_ComboItems a inner join T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelTitle' order by ComboItem asc"

        Using cnn As New SqlConnection(cnx)
            Using cmd As New SqlCommand(sql, cnn)
                cnn.Open()

                Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                If rdr.HasRows Then
                    ddTitle.DataSource = rdr
                    ddTitle.DataTextField = "COMBOITEM"
                    ddTitle.DataValueField = "COMBOITEMID"
                    ddTitle.DataBind()
                End If
            End Using
        End Using
    End Sub




    Protected Sub btRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRunReport.Click

        'If String.IsNullOrEmpty(ucStartDate.Selected_Date) Or String.IsNullOrEmpty(ucEndDate.Selected_Date) Then Return

        Dim start As String = ucStartDate.Selected_Date
        Dim finish As String = ucEndDate.Selected_Date
        Dim title As String = ddTitle.SelectedValue


        Dim sql(7) As String

        sql(0) = String.Format( _
            "Select Distinct p.personnelid, p.lastname, p.firstname from t_Personnel p " & _
            "inner join t_PersonnelTrans pt on pt.personnelid = p.personnelid " & _
            "where (pt.datecreated >  '{0}' and pt.datecreated < '{1}' " & _
            "and pt.titleid in ( {2})) or " & _
            "(pt.titleid in ({2}) and PT.KEYFIELD = 'CONTRACTID' " & _
            "AND PT.KEYVALUE in (" & _
            "select contractid from t_Contract c where c.statusdate " & _
            "between '{0}' and '{0}' and c.statusid in  " & _
            "(Select Comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON " & _
            "A.COMBOID = B.COMBOID where ComboName = 'ContractStatus' and comboitem in ('Active','Suspense') " & _
            "and c.contractid in (select contractid from t_Event where KEYFIELD='CONTRACTID' AND KEYVALUE > 0 " & _
            "and (oldvalue = 'pender' or oldvalue = 'Pender-inv'))))) order by p.lastname asc", _
                start, finish, title)




        sql(1) = String.Format( _
            "Select p.personnelID, Count(Distinct pt.KEYVALUE) as Tours from t_Personnel p inner join t_PersonnelTrans pt on pt.personnelid = p.personnelid  " & _
            "inner join t_Tour t on t.tourid = pt.KEYVALUE AND PT.KEYFIELD = 'TOURID' " & _
            "where pt.datecreated > '{0}' and pt.datecreated < '{1}' and  pt.KEYVALUE > 0  AND PT.KEYFIELD = 'TOURID' " & _
            "and pt.titleid in  ({2})  and pt.KEYFIELD = 'TOURID' AND PT.KEYVALUE not in  " & _
            "	(Select TourID from t_Tour where subtypeid in (select comboitemid from t_ComboItems " & _
            "	A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'toursubtype' and comboitem = 'exit')  " & _
            "and tourdate >= '{0}' and tourdate < '{1}')  " & _
            "and t.statusid in  (select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID " & _
            "where comboname = 'TourStatus' and comboitem in ('Showed', 'OnTour', 'Be Back'))  group by p.personnelid", _
                start, finish, title)


        sql(2) = String.Format( _
            "Select p.personnelID, Count(Distinct pt.KEYVALUE) as Contracts from t_Personnel p inner join t_PersonnelTrans pt on pt.personnelid = p.personnelid  " & _
            "inner join t_Contract c on c.contractid = pt.KEYVALUE AND PT.KEYFIELD = 'CONTRACTID'  " & _
            "where ((c.contractdate < '{0}' and c.statusdate between '{0}' and '{1}' and c.contractid in " & _
         "(select contractid from t_Event where KEYFIELD = 'contractid' AND KEYVALUE > 0 and (oldvalue = 'pender' or oldvalue = 'Pender-Inv'))) or " & _
         "(c.contractdate between '{0}' and  '{1}')) and  pt.KEYVALUE > 0  " & _
            "and pt.titleid in  ({2})  and c.statusid in " & _
            "(Select Comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID " & _
            "where ComboName = 'ContractStatus' and comboitem in ('Active','Suspense')) and c.contractid not in  " & _
            "(Select Contractid from t_Contract where contractnumber like 't%' and contractnumber not like 'T%')  group by p.personnelid", _
                start, finish, title)

        sql(3) = String.Format( _
            "Select p.personnelID, Count(Distinct pt.KEYVALUE) as Contracts from t_Personnel p inner join t_PersonnelTrans pt on pt.personnelid = p.personnelid  " & _
            "inner join t_Contract c on c.contractid = pt.KEYVALUE AND PT.KEYFIELD = 'CONTRACTID'  " & _
            "where (c.contractdate between '{0}' and '{1}') and  PT.KEYFIELD = 'CONTRACTID' AND  pt.KEYVALUE > 0  " & _
            "and pt.titleid in  ({2})  and c.statusid in  " & _
            "(Select Comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where ComboName = 'ContractStatus' and comboitem in ('Active','Suspense')) " & _
            "and c.contractid not in (Select Contractid from t_Contract where contractnumber like 't%' and contractnumber not like 'T%')  " & _
            "and c.contractid not in  " & _
            "(select contractid from t_Event E where E.KEYVALUE > 0 AND E.KEYFIELD = 'CONTRACTID' and (oldvalue = 'pender' or oldvalue = 'Pender-Inv'))  group by p.personnelid", _
                start, finish, title)

        sql(4) = String.Format( _
            "Select p.personnelID, Count(Distinct pt.KEYVALUE) as Penders from t_Personnel p inner join t_PersonnelTrans pt on pt.personnelid = p.personnelid  " & _
            "inner join t_Contract c on c.contractid = pt.KEYVALUE AND PT.KEYFIELD = 'CONTRACTID' " & _
            "where c.contractdate between '{0}' and '{1}' and  pt.KEYVALUE > 0  " & _
            "and pt.titleid in  ({2})  and c.statusid in  " & _
            "(Select Comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where ComboName = 'ContractStatus' and comboitem in " & _
            "('Pender','Developer','Pender-Inv'))  and c.contractid not in  " & _
            "(select contractid from t_Event E where E.KEYVALUE > 0 AND E.KEYFIELD = 'CONTRACTID' and " & _
            "(oldvalue = 'pender' or oldvalue = 'Pender-Inv'))   group by p.personnelid", _
            start, finish, title)


        sql(5) = String.Format( _
            "Select p.personnelID, (Select Sum(salesvolume) from t_Mortgage where " & _
            "mortgageid in (select distinct mortgageid from t_contract c inner join t_Personneltrans t " & _
            "on t.keyvalue = c.contractid and t.keyfield = 'CONTRACTID' inner join t_Mortgage m on m.contractid = c.contractid " & _
            "where (c.contractdate between ' {0} ' and ' {1} ' or (c.statusdate between ' {0} ' and '{1}' and " & _
            "c.contractdate < '{0} ' and c.contractid in (select E.KEYVALUE from t_Event E " & _
            "where E.KEYVALUE > 0 AND E.KEYFIELD = 'CONTRACTID' and (oldvalue = 'pender' or oldvalue = 'Pender-Inv')))) and t.titleid in  " & _
            "({2})   and c.statusid in  " & _
            "(Select Comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID " & _
            "where ComboName = 'ContractStatus' and comboitem in ('Active','Suspense')) and t.personnelid = p.personnelid and  " & _
            "c.contractid not in  " & _
            "(Select Contractid from t_Contract where contractnumber like 't%' and contractnumber not like 'T%') )) as [OW_CONTRACT_VOLUMNE_SELECT]  " & _
            "from t_Personnel p inner join t_PersonnelTrans pt on pt.personnelid = p.personnelid  " & _
            "          inner join t_Contract c on c.contractid = pt.KEYVALUE AND PT.KEYFIELD = 'CONTRACTID'  " & _
            "inner join t_Mortgage m on m.contractid = c.contractid  " & _
            "          where ((pt.datecreated >= '{0}' " & _
            "and pt.datecreated < '{1}' and c.statusdate < '{1}') or " & _
            "(c.contractdate < ' {0}' and c.statusdate between '{0}' and  " & _
            "dateadd(day, 1, '{0}') and c.contractid in " & _
            "(select E.KEYVALUE from t_Event E where E.KEYVALUE > 0 AND E.KEYFIELD = 'CONTRACTID' " & _
            "and (oldvalue = 'pender' or oldvalue = 'Pender-Inv')))) and  pt.KEYVALUE > 0  " & _
            "          and pt.titleid in  " & _
            "({2})  and c.statusid in  " & _
            "(Select Comboitemid from t_ComboItems A INNER JOIN " & _
            "T_COMBOS B ON A.COMBOID = B.COMBOID where ComboName = 'ContractStatus' " & _
            "and comboitem in ('Active','Suspense'))  and c.contractid not in  " & _
            "(Select Contractid from t_Contract where contractnumber like 't%' and contractnumber not like 'T%')  group by p.personnelid", _
            start, finish, title)



        sql(6) = String.Format( _
            "Select p.personnelID, (Select Sum(salesvolume) from t_Mortgage " & _
            "where mortgageid in (select distinct mortgageid from t_contract c " & _
            "inner join t_Personneltrans t on t.KEYVALUE = c.contractid AND T.KEYFIELD = 'CONTRACTID' inner join " & _
            "t_Mortgage m on m.contractid = c.contractid where (c.contractdate " & _
            "        between '{0} ' and '{1}' ) and " & _
            "t.titleid in  " & _
            "({2})   and c.statusid in  " & _
            "(Select Comboitemid from t_ComboItems A INNER JOIN T_COMBOS B " & _
            "ON A.COMBOID = B.COMBOID where ComboName = 'ContractStatus' and comboitem in ('Active','Suspense'))  " & _
            "and t.personnelid = p.personnelid and c.contractid not in  " & _
            "(Select Contractid from t_Contract where contractnumber like 't%' and contractnumber not like 'T%'))) " & _
            "as [OW_CONTRACT_VOLUMNE_NOSELECT] " & _
            "from t_Personnel p inner join " & _
            "t_PersonnelTrans pt on pt.personnelid = p.personnelid  " & _
            "          inner join t_Contract c on c.contractid = pt.KEYVALUE AND PT.KEYFIELD = 'CONTRACTID'  " & _
            "inner join t_Mortgage m on m.contractid = c.contractid  " & _
            "          where ((pt.datecreated >= '{0}' and " & _
            "pt.datecreated < '{1}' and c.statusdate < '{1}') or " & _
            "(c.contractdate < ' {0}' and c.statusdate >= '{0}' and " & _
            "c.statusdate < '{1}' and c.contractid in " & _
            "(select E.KEYVALUE from t_Event E where E.KEYVALUE > 0 AND E.KEYFIELD = 'contractid' and " & _
            "(oldvalue = 'pender' or oldvalue = 'Pender-Inv')))) and  pt.KEYVALUE > 0  " & _
            "          and pt.titleid in  " & _
            " ({2})  and c.statusid in  " & _
            "(Select Comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID " & _
            "where ComboName = 'ContractStatus' and comboitem in ('Active','Suspense'))  and c.contractid not in  " & _
            "(Select Contractid from t_Contract where contractnumber like 't%' and contractnumber not like 'T%')  " & _
            "and c.contractid not in  " & _
            "(select E.KEYVALUE from t_Event E where E.KEYVALUE > 0 AND E.KEYFIELD = 'CONTRACTID' " & _
            "and (oldvalue = 'pender' or oldvalue = 'Pender-Inv'))  group by p.personnelid", _
            start, finish, title)


        sql(7) = String.Format( _
            "Select p.personnelID, (Select Sum(salesvolume) from t_Mortgage where " & _
            "mortgageid in (select distinct mortgageid from t_contract c inner join " & _
            "t_Personneltrans t on t.KEYVALUE = c.contractid AND T.KEYFIELD = 'CONTRACTID' inner join " & _
            "t_Mortgage m on m.contractid = c.contractid where c.statusdate " & _
            "        between '{0}' and ' {1}' and t.titleid in  " & _
            " ({2})   and c.statusid in  " & _
            "(Select Comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID " & _
            "where ComboName = 'ContractStatus' and comboitem in ('Pender','Developer','Pender-Inv'))  " & _
            "and t.personnelid = p.personnelid and c.contractid not in  " & _
            "(Select Contractid from t_Contract where contractnumber like 't%' and contractnumber not like 'T%') )) as [PENDER_CONTRACT_VOLUME] " & _
            "from t_Personnel p inner join t_PersonnelTrans pt on " & _
            "        pt.personnelid = p.personnelid " & _
            "          inner join t_Contract c on c.contractid = pt.KEYVALUE AND PT.KEYFIELD = 'CONTRACTID'  " & _
            "inner join t_Mortgage m on m.contractid = c.contractid  " & _
            "          where c.contractdate between  '{0}' and '{1}' and  pt.KEYVALUE > 0 AND PT.KEYFIELD = 'CONTRACTID'  " & _
            "          and pt.titleid in  " & _
            " ({2})  and c.statusid in  " & _
            "(Select Comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where ComboName = 'ContractStatus' " & _
            "and comboitem in ('Pender','Developer','Pender-Inv'))  " & _
            "and c.contractid not in  " & _
            "(Select Contractid from t_Contract where contractnumber like 't%' and contractnumber not like 'T%')  group by p.personnelid", _
            start, finish, title)



        Dim list As IDictionary(Of Integer, Efficiency) = New Dictionary(Of Integer, Efficiency)()

        Using cnn As New SqlConnection(cnx)
            cnn.Open()

            For x As Integer = 0 To sql.Length - 1
                Using cmd As New SqlCommand(sql(x), cnn)

                    Dim rdr As SqlDataReader = cmd.ExecuteReader()

                    If x = 0 Then

                        While rdr.Read()

                            Dim id As String = rdr.Item(0)
                            list.Add(rdr.Item(0), New Efficiency() _
                                     With {.Name = rdr.Item(1) & ", " & rdr.Item(2), _
                                            .Contracts = New Integer() {0, 0}, .Volume = New Integer() {0, 0, 0}})
                        End While

                    Else                        
                        If rdr.HasRows Then list = LoopRows(x, rdr, list)
                    End If

                    rdr.Close()
                End Using
            Next
        End Using




        Dim html As New StringBuilder()


        html.Append("<table style='border-collapse:collapse' border='1' id='reportSales'>")
        html.Append("<tr><td>Personnel</td><td>Tours</td><td>Active Contracts</td><td>OW Contracts</td><td>Pending</td><td>AC Volume</td><td>VPG</td><td>Pending CV</td></tr>")

        For Each ele As KeyValuePair(Of Integer, Efficiency) In list

            Dim ncy As Efficiency = ele.Value


            html.Append("<tr>")
            html.AppendFormat("<td>{0}</td>", ncy.Name)
            html.Append(String.Format("<td>{0}</td>", ncy.Tours))
            html.AppendFormat("<td>{0}</td>", ncy.Contracts(0))
            html.AppendFormat("<td>{0}</td>", ncy.Contracts(1))
            html.AppendFormat("<td>{0}</td>", ncy.Pender)
            html.AppendFormat("<td>{0:C2}</td>", ncy.Volume(0))
            html.AppendFormat("<td>{0:C2}</td>", ncy.Volume(0) / IIf(String.IsNullOrEmpty(ncy.Tours), 0, ncy.Tours))
            html.AppendFormat("<td>{0:C2}</td>", ncy.Volume(2))

            html.Append("</tr>")

        Next

        If list.Count > 0 Then
            html.Append(String.Format( _
                "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5:C2}</td><td>{6:C2}</td><td>{7:C2}</td></tr></strong>", _
                "Total", list.Sum(Function(x) x.Value.Tours), _
                        list.Sum(Function(x) x.Value.Contracts(0)), _
                        list.Sum(Function(x) x.Value.Contracts(1)), _
                        list.Sum(Function(x) x.Value.Pender), _
                        list.Sum(Function(x) x.Value.Volume(0)), _
                        list.Sum(Function(x) x.Value.Volume(1)) / list.Sum(Function(x) x.Value.Tours), _
                        list.Sum(Function(x) x.Value.Volume(2))))

            html.Append("</table>")

            Lit1.Text = html.ToString()
        Else
            Lit1.Text = "No Matching Records"
        End If


    End Sub


    Private Function LoopRows(ByRef index As Integer, ByRef rdr As SqlDataReader, _
                              ByRef l As IDictionary(Of Integer, Efficiency)) _
                                As IDictionary(Of Integer, Efficiency)

        Do While rdr.Read()

            Dim key As String = rdr.Item(0)
            Dim e As Efficiency = l.Item(key)

            If IsDBNull(rdr.Item(1)) = False Then

                If index = 1 Then e.Tours = rdr.Item(1)
                If index = 2 Then e.Contracts(0) = rdr.Item(1)
                If index = 3 Then e.Contracts(1) = rdr.Item(1)
                If index = 4 Then e.Pender = rdr.Item(1)
                If index = 5 Then e.Volume(0) = rdr.Item(1)
                If index = 6 Then e.Volume(1) = rdr.Item(1)
                If index = 7 Then e.Volume(2) = rdr.Item(1)
            End If
        Loop

        Return l
    End Function





End Class
