Imports System
Imports System.Linq
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Script.Serialization

Partial Class Reports_CustomerService_UnUsedTime
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            ddlYear.Items.Clear()
            ddlYear.Items.Add("Select Year")
            ddlYear.AppendDataBoundItems = True
            For i = DateTime.Now.Year - 3 To DateTime.Now.Year + 1
                ddlYear.Items.Add(i.ToString())
            Next
        Else
            gvRpt.DataSource = Nothing
            gvRpt.DataBind()
        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As System.EventArgs) Handles btnSubmit.Click

        If ddlYear.SelectedValue = "Select Year" Then Return
        Dim sql As String = String.Format("select co.ContractNumber, a.SaleType,s.comboitem as Season, convert(varchar(10), co.ContractDate, 101) ContractDate, a.Rooms, Case when b.RoomsUsed is null then 0 else b.roomsused end as RoomsUsed, " & _
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
      "select sum(cast(left(ut.comboitem,1) as int)) as roomsused, u.contractid " & _
      "from t_Usage u " & _
      "inner join t_Comboitems ut on ut.comboitemid = u.roomtypeid " & _
      "inner join t_Combos co on ut.ComboID = co.ComboID " & _
      "inner join t_Comboitems us on us.comboitemid = u.statusid " & _
      "where u.usageyear = '{0}' and us.comboitem = 'used' and ut.Active = 1 and co.ComboName = 'RoomType' " & _
      "group by ut.ComboItem, u.contractid) b on b.contractid = a.contractid " & _
      "inner join t_contract co on co.contractid = a.contractid " & _
      "inner join t_Prospect p on p.prospectid = co.prospectid " & _
      "left outer join t_Comboitems s on s.comboitemid = co.seasonid " & _
      "where b.roomsused is null or a.rooms <> b.roomsused " & _
      "order by saletype", ddlYear.SelectedValue)

        Using cn As New SqlConnection(Resources.Resource.cns)
            Using ada As New SqlDataAdapter(sql, cn)
                Try
                    Dim dt = New DataTable()
                    ada.Fill(dt)
                    gvRpt.DataSource = dt
                    gvRpt.DataBind()
                Catch ex As Exception
                    Response.Write(ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Protected Sub btnExcel_Click(sender As Object, e As System.EventArgs) Handles btnExcel.Click
        Response.ClearContent()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=UnUsedTime.xls")
        gvRpt.AllowPaging = False
        btnSubmit_Click(Nothing, EventArgs.Empty)
        For Each gvr As GridViewRow In gvRpt.Rows
            For Each tc As TableCell In gvr.Cells
                If gvr.RowIndex Mod 2 = 0 Then
                    tc.BackColor = gvRpt.AlternatingRowStyle.BackColor
                Else
                    tc.BackColor = gvRpt.RowStyle.BackColor
                End If
                tc.CssClass = "textmode"
            Next
        Next
        Dim style = "<style>.textmode{}</style>"
        Response.Write(style)
        Dim sw = New StringWriter
        Dim htmlWriter = New HtmlTextWriter(sw)
        gvRpt.RenderControl(htmlWriter)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As System.Web.UI.Control)
        'needed for the btnExcel_Click to work
    End Sub
End Class
