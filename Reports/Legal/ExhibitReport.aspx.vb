Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
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
Imports System.IO


Partial Class Reports_Legal_SpreaderReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ddExhibit.DataSource = New SqlDataSource(Resources.Resource.cns, String.Format("select distinct v.UFValue " & _
                                    "from t_UF_Value v inner join t_UFields f on v.UFID = f.UFID " & _
                                    "where f.UFName in ('exhibit number') " & _
                                    "and len(v.UFValue) > 0 " & _
                                    "order by v.UFValue"))
            ddExhibit.DataTextField = "UFVALUE"
            ddExhibit.DataValueField = "UFVALUE"
            ddExhibit.DataBind()
        End If
    End Sub

    Protected Sub btRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRunReport.Click
        Dim sql As String =
                "SELECT Distinct t_Contract.ContractID, P.PROSPECTID, t_Contract.ContractNumber as KCP#, p.LastName, p.FirstName, p.MiddleInit, t_Contract.occupancydate, (select comboitem from t_comboitems where comboitemid = t_mortgage.statusid) [Reason], " &
             "COALESCE(P.SPOUSELASTNAME, SPOUSEFIRSTNAME) as [Co-Owner], 		" &
             "case when p.SSN is null then 'N/A' when p.SSN = '' then 'N/A' else p.SSN end as SSN, " &
             "case when p.SpouseSSN is null then 'N/A' when p.SpouseSSN = '' then 'N/A' else p.SpouseSSN end as [Co-Owner SSN], " &
             "t_Mortgage.SalesPrice as [Sales Price], t_Mortgage.DPTotal as [Total Down], " &
             "t_Mortgage.SalesPrice - t_Mortgage.DPTotal as [Face Value], t_Mortgage.TotalFinanced As [Amount on Note], t_unit.Name + '/' + cast(t_SalesInventory.Week as varchar) as UnitWeek, " &
             "f.frequency as Frequency, t_Contract.ContractDate as [Contract Date],v_ContractUserFields.DeedOfTrustInstrumentNumber " &
                 "FROM   ((((((t_Contract t_Contract " &
                 "	LEFT OUTER JOIN t_SoldInventory t_SoldInventory ON t_SoldInventory.ContractID = t_Contract.ContractID) " &
                 "		LEFT OUTER JOIN t_SalesInventory t_SalesInventory ON t_SoldInventory.SalesInventoryID = t_SalesInventory.SalesInventoryID) " &
                 "			INNER JOIN t_Mortgage t_Mortgage ON t_Contract.ContractID = t_Mortgage.ContractID)    " &
                 "				INNER JOIN t_Prospect p ON t_Contract.ProspectID = p.ProspectID) " &
                 "			INNER JOIN v_ContractUserFields v_ContractUserFields ON t_Contract.ContractID = v_ContractUserFields.ContractID) " &
                 "		LEFT OUTER JOIN t_Frequency f on f.frequencyid = t_Contract.frequencyid) " &
                 "	LEFT OUTER JOIN t_unit t_unit ON t_SalesInventory.UnitID = t_unit.UnitID " &
                "WHERE  v_ContractUserFields.ExhibitNumber = '" & ddExhibit.SelectedItem.Text & "'" &
                "ORDER BY t_Contract.ContractNumber"
        If r1.Checked = False And r2.Checked = False Then Return
        If r1.Checked Then originalVersion(sql)
        If r2.Checked Then editedVersion(sql)
    End Sub


    Private Sub originalVersion(sql As String)
        Dim sb As New StringBuilder()

        Using cnn As New SqlConnection(Resources.Resource.cns)
            Using cmd As New SqlCommand(Sql, cnn)

                cnn.Open()

                Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                If rdr.HasRows Then

                    Dim tmp As String = String.Empty

                    sb.Append("<table style='border-collapse:collapse;'  id='htmlTable' border='1'>")

                    sb.Append("<tr>")
                    For Each s As String In New String() {"KCP", "First Name", "Last Name", _
                                                          "Co-Owner", "Telephone-1", "Telephone-2", _
                                                          "Telephone-3", "Email", "SSN", _
                                                          "Co-Owner SSN", "Address", "City", _
                                                          "State", "Zip", "Sales Price", _
                                                          "Total Down", "Face Value", _
                                                          "Amount On Note", "Unit Week", "Frequency", _
                                                          "Contract Date", "Deed Of Trust Instructment #"}
                        sb.AppendFormat("<td>{0}</td>", s)

                    Next
                    sb.Append("</tr>")

                    Do While rdr.Read()

                        sb.Append("<tr>")
                        sb.AppendFormat("<td>{0}</td>", rdr.Item("KCP#"))
                        sb.AppendFormat("<td>{0}</td>", rdr.Item("FirstName"))
                        sb.AppendFormat("<td>{0}</td>", rdr.Item("LastName"))

                        Dim id_contract As String = rdr.Item("CONTRACTID").ToString()
                        Using cn As New SqlConnection(Resources.Resource.cns)
                            Using cm As New SqlCommand(String.Format("select * from t_contractcoowner c inner join t_prospect p on p.prospectid = c.prospectid where c.contractid = {0}", id_contract), cn)

                                cn.Open()
                                Dim rd As SqlDataReader = cm.ExecuteReader(CommandBehavior.CloseConnection)

                                If rd.HasRows = False Then
                                    sb.Append("<td>&nbsp;</td>")
                                Else
                                    sb.Append("<td>")
                                    While rd.Read()
                                        If String.IsNullOrEmpty(rd("SpouseLastName").ToString()) = False Or String.IsNullOrEmpty(rd("SpouseFirstName").ToString()) = False Then
                                            sb.AppendFormat("{0} {1}", rd.Item("SpouseFirstName"), rd.Item("SpouseLastName"))
                                        End If
                                    End While

                                    sb.Append("</td>")
                                End If
                            End Using

                        End Using



                        Using cn = New SqlConnection(Resources.Resource.cns)
                            Using cd = New SqlCommand(String.Format("select top 3 number from t_prospectphone where prospectid = {0}", _
                                                                     rdr.Item("ProspectID")), cn)

                                cn.Open()

                                Dim rd As SqlDataReader = cd.ExecuteReader(CommandBehavior.CloseConnection)
                                Dim numbers(2) As String

                                If rd.HasRows Then

                                    Dim j As Integer = 0
                                    Do While rd.Read()
                                        numbers(j) = rd.Item("Number") & ""
                                        j += 1
                                    Loop
                                End If

                                sb.AppendFormat("<td>{0}</td>", numbers(0))
                                sb.AppendFormat("<td>{0}</td>", numbers(1))
                                sb.AppendFormat("<td>{0}</td>", numbers(1))

                                rd.Close()

                                cn.Open()

                                'Email
                                rd = New SqlCommand(String.Format("select top 1 email from t_prospectemail where prospectid = '{0}' " & _
                                                                  "order by emailID desc", rdr.Item("ProspectID")), cn) _
                                .ExecuteReader(CommandBehavior.CloseConnection)

                                Dim email As String = String.Empty

                                If rd.HasRows Then
                                    rd.Read()
                                    If IsDBNull(rd.Item(0)) Then
                                        email = ""
                                    Else
                                        email = rd.Item(0)
                                    End If
                                End If

                                sb.AppendFormat("<td>{0}</td>", email)
                            End Using
                        End Using

                        sb.AppendFormat("<td>{0}</td>", rdr.Item("SSN"))
                        sb.AppendFormat("<td>{0}</td>", rdr.Item("Co-Owner SSN"))

                        Using cn As New SqlConnection(Resources.Resource.cns)
                            Using cm As New SqlCommand( _
                                String.Format("select top 1 Address1 + ' ' + Case when Address2 is null then '' else Address2 end AS Street, City, B.COMBOITEM, PostalCode  " & _
                                              "from t_prospectaddress A LEFT OUTER JOIN T_COMBOITEMS B ON A.STATEID = B.COMBOITEMID " & _
                                              "where prospectid = {0} And ActiveFlag = 1 order by ADDRESSID desc", rdr.Item("ProspectID")), cn)
                                cn.Open()
                                Dim rd As SqlDataReader = cm.ExecuteReader(CommandBehavior.CloseConnection)

                                If rd.HasRows Then
                                    rd.Read()
                                    For i As Integer = 0 To 3
                                        sb.AppendFormat("<td>{0}</td>", rd.Item(i))
                                    Next
                                Else
                                    sb.Append("<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>")
                                End If
                            End Using
                        End Using

                        sb.AppendFormat("<td>{0:C2}</td>", rdr.Item("Sales Price"))
                        sb.AppendFormat("<td>{0:C2}</td>", rdr.Item("Total Down"))
                        sb.AppendFormat("<td>{0:C2}</td>", rdr.Item("Face Value"))
                        sb.AppendFormat("<td>{0:C2}</td>", rdr.Item("Amount On Note"))
                        sb.AppendFormat("<td>{0}</td>", rdr.Item("UnitWeek"))
                        sb.AppendFormat("<td>{0}</td>", rdr.Item("Frequency"))
                        sb.AppendFormat("<td>{0}</td>", DateTime.Parse(rdr.Item("Contract Date")).ToShortDateString())
                        sb.AppendFormat("<td>{0}</td>", rdr.Item("DeedOfTrustInstrumentNumber"))

                        sb.Append("</tr>")
                    Loop
                    sb.Append("</table>")
                End If
            End Using
        End Using
        LIT.Text = sb.ToString()
    End Sub

    Private Sub editedVersion(sql As String)
        Dim sb As New StringBuilder()
        Using cnn As New SqlConnection(Resources.Resource.cns)
            Using cmd As New SqlCommand(sql, cnn)
                Try
                    Dim reader As SqlDataReader = Nothing, dt = New DataTable()
                    cnn.Open()
                    reader = cmd.ExecuteReader()
                    dt.Load(reader)

                    If dt.Rows.Count > 0 Then
                        sb.Append("<table style='border-collapse:collapse;'  id='table1' border='1'>")
                        sb.Append("<thead><tr>")
                        For Each s As String In New String() {"KCP", "First Name", "Last Name", _
                                                              "Co-Owner", "Address", "City", _
                                                              "State", "Zip", "Amount On Note", "Unit Week", _
                                                              "Frequency", "Reason", _
                                                              "Contract Date", "Deed Of Trust Instructment #"}
                            sb.AppendFormat("<th>{0}</th>", s)
                        Next
                        sb.Append("</tr></thead><tbody>")
                    End If

                    For Each dr As DataRow In dt.Rows
                        sb.Append("<tr>")
                        sb.AppendFormat("<td>{0}</td>", dr.Item("KCP#"))
                        sb.AppendFormat("<td>{0} {1}</td>", dr.Item("FirstName"), dr.Item("MiddleInit").ToString())
                        sb.AppendFormat("<td>{0}</td>", dr.Item("LastName"))

                        Dim contractID As String = dr.Item("contractID").ToString()

                        Using cm As New SqlCommand(String.Format("select p.* from t_contractcoowner o inner join t_prospect p on o.prospectid = p.prospectid where o.contractid = {0}", contractID), cnn)

                            If cnn.State = ConnectionState.Closed Then cnn.Open()
                            Dim prospectReader As SqlDataReader = cm.ExecuteReader()

                            If prospectReader.HasRows = False Then
                                sb.Append("<td>&nbsp;</td>")
                            Else
                                sb.Append("<td>")
                                Do While prospectReader.Read()
                                    sb.AppendFormat("{0} {1}<br/>", prospectReader.Item("SpouseFirstName").ToString().Trim(), prospectReader.Item("SpouseLastName").ToString().Trim())
                                Loop
                                sb.Append("</td>")
                            End If
                            prospectReader.Close()
                            prospectReader = Nothing
                        End Using

                        Using cm As New SqlCommand( _
                               String.Format("select top 1 Address1 + ' ' + Case when Address2 is null then '' else Address2 end AS Street, City, B.COMBOITEM, PostalCode  " & _
                                             "from t_prospectaddress A LEFT OUTER JOIN T_COMBOITEMS B ON A.STATEID = B.COMBOITEMID " & _
                                             "where prospectid = {0} And ActiveFlag = 1 order by ADDRESSID desc", dr.Item("ProspectID")), cnn)

                            Dim addressReader As SqlDataReader = Nothing
                            If cnn.State = ConnectionState.Closed Then cnn.Open()
                            addressReader = cm.ExecuteReader(CommandBehavior.CloseConnection)

                            If addressReader.HasRows Then
                                addressReader.Read()
                                For i As Integer = 0 To 3
                                    sb.AppendFormat("<td>{0}</td>", addressReader.Item(i).ToString())
                                Next
                            Else
                                sb.Append("<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>")
                            End If

                            addressReader.Close()
                            addressReader = Nothing
                        End Using


                        sb.AppendFormat("<td>{0:C2}</td>", dr.Item("Amount On Note"))
                        sb.AppendFormat("<td>{0}</td>", dr.Item("UnitWeek"))

                        If dr.Item("occupancydate").Equals(DBNull.Value) = False And dr.Item("frequency").Equals(DBNull.Value) = False Then
                            If dr.Item("frequency").ToString().ToLower() = "biennial" Then
                                sb.AppendFormat("<td>{0} ({1})</td>", dr.Item("Frequency"), IIf(Convert.ToDateTime(dr.Item("occupancydate").ToString()).Year Mod 2 = 0, "even", "odd"))
                            Else
                                sb.AppendFormat("<td>{0}</td>", dr.Item("Frequency"))
                            End If
                        Else
                            sb.AppendFormat("<td>{0}</td>", dr.Item("Frequency"))
                        End If
                        sb.AppendFormat("<td>{0}</td>", dr.Item("reason").ToString())
                        sb.AppendFormat("<td>{0}</td>", DateTime.Parse(dr.Item("Contract Date")).ToShortDateString())
                        sb.AppendFormat("<td>{0}</td>", dr.Item("DeedOfTrustInstrumentNumber"))
                        sb.Append("</tr>")
                    Next
                    sb.Append("</tbody></table>")
                Catch ex As Exception
                    Response.Write(ex.Message)
                Finally
                    cnn.Close()
                End Try
            End Using
        End Using
        LIT.Text = sb.ToString()
    End Sub

    Protected Sub btPrintable_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btPrintable.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Printable", "var mwin = window.open();mwin.document.write('" & Replace(LIT.Text, "'", "\'") & "');", True)
    End Sub
End Class
