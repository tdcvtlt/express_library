Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Threading

Partial Class Maintenance_PMTrackSchedulingItems
    Inherits System.Web.UI.Page

    Protected Sub submit_Click(sender As Object, e As System.EventArgs) Handles submit.Click


       

    End Sub


    Private Sub Temp()
        Using cn = New SqlConnection(Resources.Resource.gp)
            Dim sqlText = ""

            If itemNumberSearch.Checked Then
                sqlText = String.Format("select * from iv00101 where ITEMNMBR like '{0}%' order by ITEMNMBR", searchText.Value.Trim())
            ElseIf descriptionSearch.Checked Then
                sqlText = String.Format("select * from iv00101 where ITEMDESC like '%{0}%' order by ITEMDESC", searchText.Value.Trim())
            End If



            Dim connection = New SqlConnection(Resources.Resource.cns)

            Using ad = New SqlDataAdapter(sqlText, cn)
                Dim dt = New DataTable()
                ad.Fill(dt)

                Using ada = New SqlDataAdapter("select * from t_PMTrackItem", connection)
                    Dim bd = New SqlCommandBuilder(ada)


                    Dim newTable = New DataTable()
                    ada.Fill(newTable)

                    For Each dr As DataRow In dt.Rows

                        Dim newRow = newTable.NewRow()
                        newRow(1) = dr("ITEMNMBR")
                        newRow(2) = dr("ITEMDESC")


                        newTable.Rows.Add(newRow)

                    Next

                    ada.Update(newTable)
                End Using



                gvResults.DataSource = Nothing
                gvResults.DataBind()

                gvResults.DataSource = dt
                gvResults.DataBind()
            End Using
        End Using
    End Sub


    Private Class ServiceItem
        Private _itemNumber As String
        Private _itemDescription As String

        Public Property ItemNumber As String
            Get
                Return _itemNumber
            End Get
            Set(value As String)
                _itemNumber = value
            End Set
        End Property

        Public Property ItemDescription As String
            Get
                Return _itemDescription
            End Get
            Set(value As String)
                _itemDescription = value
            End Set
        End Property


    End Class

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load


    


    End Sub

  
    Protected Sub gvTracking_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTracking.RowDataBound
        Dim gvr As GridViewRow = e.Row
        Dim gv As GridView = CType(gvr.NamingContainer, GridView)

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dt = CType(gv.DataSource, DataTable)
            dt.PrimaryKey = New DataColumn() {dt.Columns(0)}

            Dim dr As DataRow = dt.Rows.Find(gv.DataKeys(e.Row.RowIndex).Value.ToString())           
            e.Row.CssClass = ""

            If DBNull.Equals(dr("Due"), DBNull.Value) Then
                e.Row.CssClass = "un-set"

            ElseIf DateTime.Compare(DateTime.Parse(dr("Due").ToString()).ToShortDateString(), DateTime.Now.ToShortDateString()) = 0 Then
                e.Row.CssClass = "present"

            ElseIf DateTime.Compare(DateTime.Parse(dr("Due").ToString()).ToShortDateString(), DateTime.Now.ToShortDateString()) < 0 Then

                e.Row.CssClass = "past"
            ElseIf DateTime.Compare(DateTime.Parse(dr("Due").ToString()).ToShortDateString(), DateTime.Now.ToShortDateString()) > 0 Then

                e.Row.CssClass = "future"

            Else
                e.Row.CssClass = ""
            End If

        End If
    End Sub

    Protected Sub refresh_Click(sender As Object, e As System.EventArgs) Handles refresh.Click
        Using cn = New SqlConnection(Resources.Resource.cns)
            Dim sqlText = "select ti.ItemID, ti.ItemNmbr, ItemDesc, sc.ScheduleID, " & _
                        "DATEDIFF(d, getdate(), sc.date) [Days], sc.Date [Due] " & _
                        "from t_PmTrackItem ti left join t_PMSchedules sc " & _
                        "on ti.ItemID = sc.TrackItemID order by [Due] asc"

            Using ad = New SqlDataAdapter(sqlText, cn)
                Dim dt = New DataTable()
                ad.Fill(dt)

                gvTracking.DataSource = Nothing
                gvTracking.DataBind()

                gvTracking.DataSource = dt
                gvTracking.DataBind()

                Try
                    gvTracking.UseAccessibleHeader = True
                    gvTracking.HeaderRow.TableSection = TableRowSection.TableHeader
                Catch ex As Exception
                    ex = Nothing
                End Try

            End Using
        End Using
    End Sub
End Class
