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
Imports System.Linq

Partial Class Reports_Marketing_MarketingReservations
    Inherits System.Web.UI.Page


    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/MarketingReservations.rpt"


    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        If Not Page.IsPostBack() Then
            BindListControls()
        Else

        End If



    End Sub





    Private Function ReservationTypesAsString(ByRef dd As DropDownList) As String

        If dd.Items.Count() > 0 Then

            Dim values = String.Empty

            If dd.SelectedValue.Contains("All") Then
                For Each r As ListItem In dd.Items
                    If r.Text.Trim().Contains("All") Then
                        values &= IIf(values = "", "'" & r.Value, "," & r.Value)
                    End If
                Next

                values &= "'"
            Else
                values = "'" & dd.SelectedValue & "'"
            End If

            Return values

        Else
            Return String.Empty
        End If

    End Function


    Private Function ReservationStatusesAsString() As String


        Dim values As String = String.Empty
        Dim containsAll As Boolean = False

        For Each l As ListItem In lbStatus.Items
            If l.Text.Contains("All") = False Then

                values &= IIf(values = "", "'" & l.Value, "," & l.Value)
            Else
                containsAll = True
            End If
        Next

        values &= "'"

        If containsAll Then

            values = String.Empty

            For Each l As ListItem In ddStatus.Items
                values &= IIf(values = "", "'" & l.Value, "," & l.Value)
            Next

            values &= "'"
        End If

        Return values

    End Function

    Private Sub LoadReport()


        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)

        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)


        Report.SetParameterValue("startDate", ucStartDate.Selected_Date)
        Report.SetParameterValue("endDate", ucEndDate.Selected_Date)
        Report.SetParameterValue("reservationTypes", ReservationTypesAsString(ddReservationType))
        Report.SetParameterValue("statuses", ReservationStatusesAsString())

        Response.Write(ReservationTypesAsString(ddReservationType) & "|" & ReservationStatusesAsString())

        CrystalReportViewer1.ReportSource = Report


    End Sub



    Protected Sub btExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btExcel.Click

    End Sub





    Private Sub BindListControls()

        Dim ds As New SqlDataSource

        ds.ConnectionString = Resources.Resource.cns

        Try
            ds.SelectCommand = "select * from t_combos a " & _
                                "inner join t_comboitems b " & _
                                "on a.comboID = b.ComboID " & _
                                "where comboname = 'ReservationType' " & _
                                "and ComboItem IN ('Marketing', 'Rental') "

            ddReservationType.DataSource = ds
            ddReservationType.DataTextField = "ComboItem"
            ddReservationType.DataValueField = "ComboItemID"

            ddReservationType.Items.Add(New ListItem("All", "0"))

            ddReservationType.AppendDataBoundItems = True

            ddReservationType.DataBind()




            ''''
            ''''// Drop Down Statuses

            ds.SelectCommand = "select  * from t_comboitems a " & _
                                "inner join t_combos b on a.ComboID = b.ComboID  " & _
                                "where comboname = 'ReservationStatus' and active = 1"

            ddStatus.DataSource = ds
            ddStatus.DataTextField = "ComboItem"
            ddStatus.DataValueField = "ComboItemID"


            ddStatus.Items.Add(New ListItem("All", "0"))

            ddStatus.AppendDataBoundItems = True
            ddStatus.DataBind()


        Catch ex As Exception

            Response.Write(ex.Message)
        End Try


    End Sub

    Protected Sub btRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRun.Click

        If lbStatus.Items.Count = 0 Then Return

        If String.IsNullOrEmpty(ucStartDate.Selected_Date) Or String.IsNullOrEmpty(ucEndDate.Selected_Date) Then Return

        LoadReport()

    End Sub





    Protected Sub btAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btAdd.Click


        For Each l As ListItem In lbStatus.Items
            If l.Text.Contains("All") Then Return
        Next

        'Reservation Status ...
        If ddStatus.Items.Count > 0 Then

            Dim li As ListItem = ddStatus.SelectedItem
            lbStatus.ClearSelection()

            lbStatus.Items.Add(li)
            ddStatus.Items.Remove(li)

        End If


    End Sub

    Protected Sub btRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRemove.Click

        If lbStatus.Items.Count > 0 And lbStatus.SelectedIndex <> -1 Then

            Dim li As ListItem = lbStatus.SelectedItem

            ddStatus.ClearSelection()

            ddStatus.Items.Add(li)
            lbStatus.Items.Remove(li)

        End If

    End Sub


    Protected Sub btRemoveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRemoveAll.Click

        If lbStatus.Items.Count > 0 Then

            For Each l As ListItem In lbStatus.Items
                ddStatus.Items.Add(l)
            Next

            For i As Integer = lbStatus.Items.Count - 1 To 0
                ddStatus.ClearSelection()
                lbStatus.ClearSelection()
                lbStatus.Items.RemoveAt(i)
            Next


        End If
    End Sub
End Class
