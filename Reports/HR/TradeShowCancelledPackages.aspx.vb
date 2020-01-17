Imports System.Data
Imports System.Data.SqlClient

Partial Class Reports_HR_TradeShowCancelledPackages
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim sb As New StringBuilder()

        If DateTime.Compare(getStartDate, DateTime.MaxValue) = 0 Or DateTime.Compare(getEndDate, DateTime.MaxValue) = 0 Then
            sb.AppendFormat("<br/>")
            sb.AppendFormat("{0}Date range is not valid.{1}", "<h2>", "</h2>")
        Else

            Dim re_headers() As String = {"Status Date", "Status", "Trans Date", "Trans Code", "Field", "Prospect", "Amount", "Personnel", "Solicitor", "Sales Location"}
            Using ada As New SqlDataAdapter(getSQL, New SqlConnection(Resources.Resource.cns))
                Dim dt As New DataTable()

                ada.Fill(dt)

                If dt.Rows.Count = 0 Then
                    sb.AppendFormat("{0}No Trade Show Packages got cancelled during {2} and {3} {1}", "<h2>", "</h2>", getStartDate.ToShortDateString(), getEndDate.AddDays(-1).ToShortDateString())
                Else

                    sb.AppendFormat("<table border=1 style=border-collapse:collapse;>")
                    sb.Append("<tr>")
                    For Each h As String In re_headers
                        sb.AppendFormat("<td><h3>{0}</h3></td>", h.ToUpper())
                    Next
                    sb.Append("</tr>")

                    For Each dr As DataRow In dt.Rows
                        sb.Append("<tr>")

                        sb.AppendFormat("<td>{0}</td>", DateTime.Parse(dr.Item("StatusDate").ToString()).ToShortDateString())
                        sb.AppendFormat("<td>{0}</td>", dr.Item("Status").ToString())
                        sb.AppendFormat("<td>{0}</td>", DateTime.Parse(dr.Item("TransDate").ToString()).ToShortDateString())
                        sb.AppendFormat("<td>{0}</td>", dr.Item("ComboItem").ToString())
                        sb.AppendFormat("<td>PkgID: {0}</td>", dr.Item("PackageIssuedID").ToString())
                        sb.AppendFormat("<td>{0} {1}</td>", dr.Item("FirstName").ToString(), dr.Item("LastName").ToString())
                        sb.AppendFormat("<td>{0:N2}</td>", Decimal.Parse(dr.Item("Amount").ToString()))
                        sb.AppendFormat("<td>{0}</td>", dr.Item("UserName").ToString())
                        sb.AppendFormat("<td>{0}, {1}</td>", dr.Item("solicitor_LastName").ToString(), dr.Item("solicitor_FirstName").ToString())
                        sb.AppendFormat("<td>{0}</td>", dr.Item("Location").ToString())
                        sb.Append("</tr>")
                    Next


                    sb.AppendFormat("</table>")

                End If
            End Using
        End If

        litReport.Text = sb.ToString()
    End Sub

    Private ReadOnly Property getStartDate As DateTime
        Get
            If String.IsNullOrEmpty(dteSDate.Selected_Date) Then
                Return DateTime.MaxValue
            Else
                Return DateTime.Parse(dteSDate.Selected_Date).ToShortDateString
            End If
        End Get
    End Property


    Private ReadOnly Property getEndDate As DateTime
        Get
            If String.IsNullOrEmpty(dteEDate.Selected_Date) Then
                Return DateTime.MaxValue
            Else
                Return DateTime.Parse(dteEDate.Selected_Date).AddDays(1).ToShortDateString()
            End If
        End Get
    End Property

    Private ReadOnly Property getSQL As String
        Get
            Return String.Format( _
                "select (select comboItem from t_ComboItems where comboitemid = pi.statusid) [Status], " & _
                "sol.LastName [solicitor_LastName], sol.FirstName [solicitor_FirstName], * " & _
                "from t_packageissued pi " & _
                "left join t_invoices i on pi.packageissuedid = i.keyvalue and i.keyfield =  'packageissuedid' " & _
                "inner join t_Prospect p on p.prospectid = i.prospectid " & _
                "inner join t_Fintranscodes f on f.fintransid = i.fintransid " & _
                "inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid " & _
                "left outer join t_Personnel per on per.personnelid = i.UserID " & _
                "left outer join ( " & _
                "	select p.lastname, p.firstname, pt.*, ti.comboitem from t_Personneltrans pt " & _
                "	inner join t_Personnel p on p.personnelid = pt.personnelid " & _
                "	inner join t_Comboitems ti on ti.comboitemid = pt.titleid " & _
                "	where ti.comboitem = 'Tradeshow Solicitor' and pt.keyfield='packageissuedid') sol " & _
                "	on sol.keyvalue = i.keyvalue left outer join (select vsl.Location, t.packageissuedid from t_VendorSalesLocations vsl " & _
                "	inner join t_VendorRep2Tour vrt on vrt.salelocid = vsl.saleslocationid inner join t_Tour t on t.tourid = vrt.tourid) sl " & _
                "	on sl.packageissuedid = i.keyvalue " & _
                "where pi.statusid in " & _
                "(select comboitemid from t_comboitems where comboid = 322 and comboitem like 'can%') " & _
                "and pi.statusdate between '{0}' AND '{1}' " & _
                "and f.MerchantAccountID = 4 and i.ApplyToID =0 and i.fintransid = 78 " & _
                "order by pi.StatusDate desc", getStartDate, getEndDate)
        End Get
    End Property
End Class
