Imports System.Data
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.Linq


Partial Class Reports_Sales_ClosingOfficerReport
    Inherits System.Web.UI.Page


    Private Structure Personnel


        Dim _id As String
        Public Property ID() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property

        Dim _name As String
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property
    End Structure


    Dim cnn As SqlConnection
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            ControlBind()
        Else

        End If
    End Sub


    Private Function GetPersonnelTitles() As IList(Of Personnel)

        Dim sb As New StringBuilder()

        sb.Append("Select p.PersonnelID, ISNULL(p.lastname, '') [LastName], ISNULL(p.firstname, '')[FirstName] from t_Personnel p " & _
            "where p.personnelid in (select personnelid from t_PersonnelTrans where KEYVALUE > 0 AND KEYFIELD = 'CONTRACTID' " & _
            "and titleid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'personneltitle' " & _
            "and comboitemID in (16785))) and statusid in  " & _
            "(Select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelStatus' and comboitem = 'Active') order by lastname, firstname")

        Dim List As IList(Of Personnel) = New List(Of Personnel)

        Using cnn As New SqlConnection(Resources.Resource.cns)
            Using cmd As New SqlCommand(sb.ToString(), cnn)

                cnn.Open()

                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If rdr.HasRows Then
                    Do While rdr.Read

                        List.Add(New Personnel() With { _
                            .ID = rdr.Item("PERSONNELID").ToString(), _
                            .Name = String.Format(rdr.Item("LastName").ToString & "{0}" & rdr.Item("FirstName").ToString(), ", ") _
                        })
                    Loop
                End If
            End Using
        End Using

        Return List

    End Function

    Private Sub ControlBind()

        Dim List As IList(Of Personnel) = GetPersonnelTitles()

        ddPersonnelList.Items.Add(New ListItem("All", "0"))
        ddPersonnelList.AppendDataBoundItems = True


        ddPersonnelList.DataValueField = "ID"
        ddPersonnelList.DataTextField = "Name"


        ddPersonnelList.DataSource = List.ToArray
        ddPersonnelList.DataBind()

    End Sub




    Protected Sub btAddPersonnel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btAddPersonnel.Click

        If ddPersonnelList.Items.Count > 0 Then
            
            lbSelectedPersonnel.Items.Add(ddPersonnelList.SelectedItem)
            ddPersonnelList.Items.Remove(ddPersonnelList.SelectedItem)

            lbSelectedPersonnel.ClearSelection()

        End If

    End Sub



    Protected Sub btRemovePersonel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRemovePersonel.Click

        If lbSelectedPersonnel.Items.Count > 0 And lbSelectedPersonnel.SelectedIndex > -1 Then

            ddPersonnelList.Items.Add(lbSelectedPersonnel.SelectedItem)
            lbSelectedPersonnel.Items.Remove(lbSelectedPersonnel.SelectedItem)

            ddPersonnelList.ClearSelection()
        End If
    End Sub




    Protected Sub btRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRunReport.Click

        If String.IsNullOrEmpty(ucDateStart.Selected_Date) Or _
            String.IsNullOrEmpty(ucDateEnd.Selected_Date) Then Return



        Dim inSelected As String = String.Empty
        Dim i As Integer = 0


        Dim selected As IEnumerable(Of ListItem) = From index As ListItem In lbSelectedPersonnel.Items Select index
        
        Dim All As ListItem = selected.Where(Function(x) x.Text.Contains("All")).SingleOrDefault()


        If All Is Nothing Then

            Dim tmpArr(selected.Count - 1) As String

            For Each l In selected
                tmpArr(i) = l.Value
                i += 1
            Next

            inSelected = String.Join(",", tmpArr)

        Else

            Dim whole As IList(Of Personnel) = GetPersonnelTitles()
            Dim tmpArr(whole.Count - 1) As String

            For Each l In whole

                tmpArr(i) = l.ID
                i += 1
            Next

            inSelected = String.Join(",", tmpArr)
        End If

        Dim PersonnelSQL As String

        If inSelected.Length = 0 Then
            PersonnelSQL = " where f.personnelid in (select personnelid " & _
                            " from t_PersonnelTrans where KeyValue > 0 and Keyfield = 'contractid' " & _
                            "and  titleid in (select comboitemid from t_Comboitems A " & _
                            "INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'personneltitle' and comboitem in (16785))) order by f.lastname, f.firstname {12}"
        Else
            PersonnelSQL = "where f.personnelid in ({12}) order by f.lastname, f.firstname"
        End If

 
        Dim sb As New StringBuilder()

        sb.Append(String.Format("select	b.total as gdc, " & _
                      "c.total as gdv, " & _
                      "d.total as rc, " & _
                      "e.total as rv, " & _
                      "b.personnelid, f.lastname, f.firstname, " & _
                      "g.total as gdrc, " & _
                      "h.total as gdrv from  " & _
                      "(  " & _
                      "	select lastname, firstname,personnelid from t_Personnel  " & _
                      ") f left outer join  " & _
                      "(  " & _
                      "	select count(distinct a.contractid)as total, t.personnelid " & _
                      "		from (  " & _
                      "			Select distinct contractid from t_Contract where (statusid in (select comboitemid from t_Comboitems a " & _
                      "					inner join t_combos b on a.comboid = b.comboid where comboname = 'contractstatus' and comboitem in ('Active','Suspense'))  " & _
                      "				or contractid in (select e.KEYVALUE from t_Event e where e.KEYFIELD = 'CONTRACTID' and e.KEYVALUE > 0 " & _
                      "									and oldvalue in ('Active', 'Suspense') and newvalue not in ('kick','pender', 'pender-inv','developer','active','suspense'))) " & _
                      "			and contractdate between '{0}' and '{1}'    " & _
                      "			and typeid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contracttype' and comboitem = 'full down') " & _
                      "			and contractnumber not like 't%' and contractnumber not like 'u%'  " & _
                      "	) a inner join t_Personneltrans t on t.KEYVALUE = a.contractid AND T.KEYFIELD = 'CONTRACTID' where t.titleid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B " & _
                      "																						ON A.COMBOID = B.COMBOID where comboname = 'personneltitle' and comboitemid in (16785 ))  " & _
                      "	group by t.personnelid  " & _
                      ") b on b.personnelid = f.personnelid left outer join (  " & _
                      "	select sum(a.salesvolume) as total, t.personnelid from (  " & _
                      "								Select distinct c.contractid, m.salesvolume  from t_Contract c inner join t_Mortgage m " & _
                      "								on m.contractid = c.contractid " & _
                      "								where (c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B " & _
                      "									ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in ('Active','Suspense'))    " & _
                      "							or c.contractid in (select KEYVALUE from t_Event E where E.KEYFIELD = 'CONTRACTID' AND E.KEYVALUE > 0 and " & _
                      "							oldvalue in ('Active', 'Suspense') and newvalue not in ('kick','pender','pender-inv','developer','active','suspense'))) " & _
                      "		and contractdate between '{2}' and '{3}'    " & _
                      "		and typeid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contracttype' and comboitem = 'full down') " & _
                      "		and contractnumber not like 't%' and contractnumber not like 'u%' " & _
                      "	) a inner join t_Personneltrans t on t.KEYVALUE = a.contractid AND T.KEYFIELD = 'CONTRACTID' where t.titleid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID " & _
                      "		where comboname = 'personneltitle' and comboitemid in ( 16785 ))  " & _
                      "	group by t.personnelid  " & _
                      ") c on c.personnelid = f.personnelid left outer join (  " & _
                      "		select count(distinct a.contractid) as total, " & _
                      "		t.personnelid from (select distinct contractid from t_Contract where contractdate between '{4}' and '{5}' and   " & _
                      "		contractid in (select KEYVALUE from t_Event E where E.KEYFIELD = 'contractid' AND E.KEYVALUE > 0 and oldvalue in ('Active', 'Suspense') " & _
                      "		and newvalue not in ('kick','pender','pender-inv','developer','active','suspense'))   " & _
                      "	and typeid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contracttype' and comboitem = 'full down') " & _
                      "	and contractnumber not like 't%' and contractnumber not like 'u%'  " & _
                      "	) a inner join t_Personneltrans t on t.KEYVALUE = a.contractid AND T.KEYFIELD = 'CONTRACTID' where t.titleid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B " & _
                      "		ON A.COMBOID = B.COMBOID where comboname = 'personneltitle' and comboitemid in ( 16785 ))  " & _
                      "	group by t.personnelid  " & _
                      ") d on d.personnelid = f.personnelid left outer join (  " & _
                      "	select sum(a.salesvolume) as total, t.personnelid " & _
                      "	from (select distinct c.contractid, m.salesvolume from t_Contract c inner join t_Mortgage m on m.contractid = c.contractid where contractdate between '{6}' and '{7}' and   " & _
                      "	c.contractid in (SELECT KEYVALUE from t_Event E where E.KEYFIELD = 'contractid' AND E.KEYVALUE > 0 and oldvalue in ('Active', 'Suspense') " & _
                      "	and newvalue not in ('kick','pender','pender-inv','developer','active','suspense'))   " & _
                      "	and typeid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contracttype' and " & _
                      "	comboitem = 'full down') and contractnumber not like 't%' and contractnumber not like 'u%'  " & _
                      "	) a inner join t_Personneltrans t on t.KEYVALUE = a.contractid AND T.KEYFIELD = 'CONTRACTID' where t.titleid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B " & _
                      "		ON A.COMBOID = B.COMBOID where comboname = 'personneltitle' and comboitemid in ( 16785 ))  " & _
                      "	group by t.personnelid  " & _
                      ") e on e.personnelid = f.personnelid left outer join (  " & _
                      "	select count(distinct a.contractid) as total, t.personnelid from (select distinct contractid from t_Contract where contractdate between '{8}' and '{9}' and    " & _
                      "	contractid in (select e.KEYVALUE from t_Event e inner join t_Contract c on c.contractid = e.KEYVALUE AND E.KEYFIELD = 'CONTRACTID' where e.KEYVALUE > 0 and oldvalue in ('Active', 'Suspense') " & _
                      "	and datecreated between c.contractdate and c.contractdate + 7 and newvalue not in ('kick','pender','pender-inv','developer','active','suspense'))   " & _
                      "	and typeid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B " & _
                      "		ON A.COMBOID = B.COMBOID where comboname = 'contracttype' and comboitem = 'full down') and contractnumber not like 't%' and contractnumber not like 'u%' " & _
                      "	) a inner join t_Personneltrans t on t.KEYVALUE = a.contractid AND T.KEYFIELD = 'CONTRACTID' where t.titleid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B " & _
                      "		ON A.COMBOID = B.COMBOID where comboname = 'personneltitle' and comboitemid in ( 16785 ))   " & _
                      "	group by t.personnelid   " & _
                      ") g on g.personnelid = f.personnelid left outer join (   " & _
                      "	select sum(a.salesvolume) as total, t.personnelid from (select distinct c.contractid, m.salesvolume " & _
                      "	from t_Contract c inner join t_Mortgage m on m.contractid = c.contractid where contractdate between '{10}' and '{11}' and    " & _
                      "	c.contractid in (select e.KEYVALUE from t_Event e inner join t_Contract c on c.contractid = e.KEYVALUE AND E.KEYFIELD = 'CONTRACTID'  " & _
                      "	where e.KEYVALUE > 0 and oldvalue in ('Active', 'Suspense') and e.datecreated between c.contractdate and c.contractdate + 7 and newvalue not in ('kick','pender','pender-inv','developer','active','suspense'))    " & _
                      "	and typeid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B " & _
                      "		ON A.COMBOID = B.COMBOID where comboname = 'contracttype' and comboitem = 'full down') and contractnumber not like 't%' and contractnumber not like 'u%'  " & _
                      "	) a inner join t_Personneltrans t on t.KEYVALUE = a.contractid AND T.KEYFIELD = 'CONTRACTID' where t.titleid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B " & _
                      "		ON A.COMBOID = B.COMBOID where comboname = 'personneltitle' and comboitemid in ( 16785 ))  " & _
                      "	group by t.personnelid   " & _
                      ") h on h.personnelid = f.personnelid " & PersonnelSQL, _
                                                                ucDateStart.Selected_Date, ucDateEnd.Selected_Date, _
                                                                ucDateStart.Selected_Date, ucDateEnd.Selected_Date, _
                                                                ucDateStart.Selected_Date, ucDateEnd.Selected_Date, _
                                                                ucDateStart.Selected_Date, ucDateEnd.Selected_Date, _
                                                                ucDateStart.Selected_Date, ucDateEnd.Selected_Date, _
                                                                ucDateStart.Selected_Date, ucDateEnd.Selected_Date, _
                                                                inSelected))



        Dim output As New StringBuilder()

        output.Append("<table style='border-collapse:collapse' border=1>")
        output.Append("<tr><td align='center'><b>Name</b></td>")
        output.Append("<td align='center'><b>Full Downs</b></td>")
        output.Append("<td align='center'><b>FD Volume</b></td>")
        output.Append("<td align='center'><b>Rescinds</b></td>")
        output.Append("<td align='center'><b>Res Volume</b></td>")
        output.Append("<td align='center'><b>Rescind %</b></td>")
        output.Append("<td align='center'><b>Within 7 Day Rescinds</b></td>")
        output.Append("<td align='center'><b>Within 7 Day Res Volume</b></td>")
        output.Append("<td align='center'><b>Within 7 Day Rescind %</b></td>")
        output.Append("<td align='center'><b>Within 7 Day Res Volume %</b></td></tr>")



        Using cnn As New SqlConnection(Resources.Resource.cns)
            Using cmd As New SqlCommand(sb.ToString(), cnn)

                cnn.Open()

                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If rdr.HasRows Then
                    Do While rdr.Read()

                        Dim fn As String = rdr.Item("LastName") & ", " & rdr.Item("FirstName")
                        Dim gdc As String = IIf(IsDBNull(rdr.Item("gdc")), 0, rdr.Item("gdc"))
                        Dim gdv As String = IIf(IsDBNull(rdr.Item("gdv")), 0, rdr.Item("gdv"))
                        Dim rc As String = IIf(IsDBNull(rdr.Item("rc")), 0, rdr.Item("rc"))
                        Dim rv As String = IIf(IsDBNull(rdr.Item("rv")), 0, rdr.Item("rv"))



                        Dim gdrc As String = IIf(IsDBNull(rdr.Item("gdrc")), 0, rdr.Item("gdrc").ToString())
                        Dim gdrv As String = IIf(IsDBNull(rdr.Item("gdrv")), 0, rdr.Item("gdrv").ToString())


                        output.Append("<tr>")

                        output.Append(String.Format("<td>{0}</td>", fn))
                        output.Append(String.Format("<td>{0}</td>", gdc))
                        output.Append(String.Format("<td>{0:C2}</td>", Single.Parse(gdv)))

                        output.Append(String.Format("<td>{0}</td>", Single.Parse(rc)))
                        output.Append(String.Format("<td>{0:C2}</td>", Single.Parse(rv)))


                        output.Append(String.Format("<td>{0}</td>", _
                            IIf(Single.Parse(gdc) > 0 And Single.Parse(rc) > -1, _
                                FormatPercent(Single.Parse(rc) / Single.Parse(gdc), 2), _
                                FormatPercent(0, 2))))



                        output.Append(String.Format("<td>{0}</td>", Single.Parse(gdrc)))

                        output.Append(String.Format("<td>{0:C2}</td>", Single.Parse(gdrv)))



                        output.Append(String.Format("<td>{0}</td>", _
                            IIf(Single.Parse(gdc) > 0 And Single.Parse(gdrc) > -1, _
                                FormatPercent(Single.Parse(gdrc) / Single.Parse(gdc), 2), _
                                FormatPercent(0, 2))))


                        output.Append(String.Format("<td>{0}</td>", _
                            IIf(Single.Parse(gdv) > 0 And Single.Parse(gdrv) > -1, _
                                FormatPercent(Single.Parse(gdrv) / Single.Parse(gdv), 2), _
                                FormatPercent(0, 2))))


                        output.Append("</tr>")


                    Loop

                End If

            End Using

        End Using



        Dim sql As String = _
            "Select x.*, y.ComboItem as ContractStatus from (select p.personnelid, p.lastname, p.firstname, a.* from (  " & _
              "Select distinct * from t_Contract where (statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B " & _
              "		ON A.COMBOID = B.COMBOID  where comboname = 'contractstatus' and comboitem in ('Active','Suspense'))    " & _
              "or contractid in (select E.KEYVALUE from t_Event E where E.KEYVALUE > 0 AND E.KEYFIELD = 'CONTRACTID' and oldvalue in ('Active', 'Suspense') " & _
              "and newvalue not in ('kick','pender','pender-inv','developer','active','suspense'))) and contractdate between '1/1/12' and '3/31/12'    " & _
              "and typeid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B " & _
              "		ON A.COMBOID = B.COMBOID  where comboname = 'contracttype' and comboitem = 'full down') and contractnumber not like 't%' and contractnumber not like 'u%' " & _
              ") a inner join t_Personneltrans t on T.KEYVALUE = a.contractid AND T.KEYFIELD = 'CONTRACTID' inner join t_Personnel p " & _
              "on p.personnelid = t.personnelid where t.titleid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B " & _
              "		ON A.COMBOID = B.COMBOID where comboname = 'personneltitle' and comboitemid in ( 16785 ))) x " & _
              "left outer join t_ComboItems y on x.statusid = y.comboitemid  " & _
             "where x.personnelid in ({0}) order by x.lastname, x.firstname"

        sb = New StringBuilder()

        sb.Append(String.Format(sql, inSelected))






        Dim htmlDetail As New StringBuilder()

        If cbShowDetail.Checked = True Then


            Using cnn As New SqlConnection(Resources.Resource.cns)
                Using cmd As New SqlCommand(sb.ToString(), cnn)

                    cnn.Open()

                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                    
                    htmlDetail.Append("<table style='border-collapse:collapse;border:1px sold blue;' border='1px'>")

                    If rdr.HasRows Then

                        htmlDetail.Append("<tr>")
                        For j As Integer = 0 To rdr.FieldCount - 1
                            htmlDetail.AppendFormat("<td>{0}</td>", rdr.GetName(j).ToUpper())
                        Next

                        htmlDetail.Append("</tr>")


                        Do While rdr.Read()

                            htmlDetail.Append("<tr>")
                            For j As Integer = 0 To rdr.FieldCount - 1
                                htmlDetail.Append(String.Format("<td>{0}</td>", IIf(IsDBNull(rdr.Item(j)), _
                                                                                    String.Empty, rdr.Item(j).ToString())))
                            Next

                            htmlDetail.Append("</tr>")
                        Loop

                    End If

                    htmlDetail.Append("</table>")

                End Using
            End Using



        End If





        Lit1.Text = Server.HtmlDecode("<br/><br/><hr/><br/>") & output.ToString() & htmlDetail.ToString()

    End Sub
End Class
