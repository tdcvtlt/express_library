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

Partial Class Reports_OwnerServices_OutOfService
    Inherits System.Web.UI.Page


#Region "Page Variables"

#End Region

#Region "Page Events"


    Protected Sub RunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunReport.Click

        If (String.IsNullOrEmpty(SDate.Selected_Date) Or String.IsNullOrEmpty(EDate.Selected_Date)) Then Return
        Dim html As New StringBuilder()

        Using cnn As New SqlConnection(Resources.Resource.cns)
            Dim sql As String = String.Format( _
                "select x.dateallocated,r.roomid,  r.RoomNumber, " & _
                    "(select top 1 n.Note from t_Note n left join t_Personnel p on n.CreatedByID = p.PersonnelID  where n.KeyField='RoomID' and n.KeyValue = r.roomid and Note like 'Taken offline from % to %' order by n.DateCreated desc) as Note, " & _
                    "(select top 1 p.FirstName + ' ' + p.LastName as Name from t_Note n left join t_Personnel p on n.CreatedByID = p.PersonnelID where n.KeyField='RoomID' and n.KeyValue = r.roomid and Note like 'Taken offline from % to %' order by n.DateCreated desc) as CreatedBy, " & _
                    "(select top 1 n.DateCreated from t_Note n left join t_Personnel p on n.CreatedByID = p.PersonnelID where n.KeyField='RoomID' and n.KeyValue = r.roomid and Note like 'Taken offline from % to %' order by n.DateCreated desc) as DateCreated " & _
                "from t_room r inner join t_roomallocationmatrix x on x.roomid = r.roomid " & _
                "where x.reservationid = -1 and x.dateallocated between '{0}' and '{1}' and " & _
                    "r.RoomNumber not in ('1-109A', '1-109B', '1-200A', '1-200B', '12-117B', " & _
                    "'19-101A','19-101B','19-101C') " & _
                "order by x.DateAllocated, CharIndex('-',r.RoomNumber), r.RoomNumber ", SDate.Selected_Date.Trim(), _
                EDate.Selected_Date.Trim())

            Using cmd As New SqlCommand(sql, cnn)
                cnn.Open()
                Dim rdr As SqlDataReader = cmd.ExecuteReader()
                Dim dt As New DataTable
                dt.Load(rdr)
                gvReport.DataSource = dt
                gvReport.DataBind()
                BtnExcel.Enabled = (gvReport.Rows.Count > 0)
                rdr.Close()
            End Using
            If cnn.State <> ConnectionState.Closed Then cnn.Close()
        End Using


    End Sub


#End Region

    Protected Sub BtnExcel_Click(sender As Object, e As EventArgs) Handles BtnExcel.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=Out Of Service.xls")
        Response.Write("<table><tr>")
        For i = 0 To gvReport.HeaderRow.Cells.Count - 1
            Response.Write("<th>" & gvReport.HeaderRow.Cells(i).Text & "</th>")
        Next
        Response.Write("</tr>")

        For Each row As GridViewRow In gvReport.Rows
            Response.Write("<tr>")
            For i = 0 To gvReport.HeaderRow.Cells.Count - 1
                Dim h As New HyperLink

                If i = 1 Then
                    h = CType(row.Cells(1).Controls(0), HyperLink)
                    Response.Write("<td>" & h.Text & "</td>")
                Else
                    Response.Write("<td>" & row.Cells(i).Text & "</td>")
                End If

            Next
            Response.Write("</tr>")
        Next
        Response.Write("</table>")
        Response.End()
    End Sub
End Class
