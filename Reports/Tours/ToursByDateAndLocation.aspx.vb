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
Imports System.Collections.Generic
Imports System.Linq



Partial Class Reports_Tours_ToursByDateAndLocation
    Inherits System.Web.UI.Page



#Region "Page Variables"
    Private reportPath As String = "REPORTFILES/ToursByDateAndLocation.rpt"
    Private documentX As New ReportDocument
    Private reportX As Report
#End Region


#Region "Page Events & Handlers"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Me.IsPostBack = False) Then

            Dim items = New SqlComboItemsRepository().GetComboItems _
                        .Where(Function(x) x.ComboName.Equals("TourLocation")) _
                        .OrderBy(Function(x) x.ComboItem)

            For Each i In items
                Location.Items.Add(New ListItem(i.ComboItem, i.ComboItemID))
            Next

            CrystalViewer.Visible = False
            Me.DataBind()

        ElseIf (Me.IsPostBack = True) Then
            If Not Session("Crystal") Is Nothing Then
                If Not Session("UserID") Is Nothing Then
                    CrystalViewer.ReportSource = DirectCast(Session("Crystal"), ReportDocument)
                Else
                    Response.Redirect("http://crms.kingscreekplantation.com/crmsnet/logon.aspx")
                End If
            End If
        End If

    End Sub

    Protected Sub RunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunReport.Click

        If (Not Session("Crystal") Is Nothing) Then Session("Crystal") = Nothing

        Dim l As IList(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))()
        l.Add(New KeyValuePair(Of String, String)("SDATE", Me.SDate.Selected_Date))
        l.Add(New KeyValuePair(Of String, String)("EDATE", Me.EDate.Selected_Date))
        l.Add(New KeyValuePair(Of String, String)("LOCATION", Me.Location.SelectedItem.Value))


        reportX = New Report With {.Database = Resources.Resource.DATABASE, _
                                                      .ServerName = Resources.Resource.SERVER, _
                                                      .User = Resources.Resource.USERNAME, _
                                                      .Password = Resources.Resource.PASSWORD, _
                                                      .Path = Server.MapPath(reportPath), _
                                                      .Parameters = l, _
                                                      .HttpCurrent = HttpContext.Current _
                                  }

        Dim context As HttpContext = DirectCast(reportX, ICrystal).DoReport()
        Session("Crystal") = context.Session("Crystal")

        CrystalViewer.ReportSource = Session("Crystal")
        CrystalViewer.Visible = True

    End Sub

#End Region

End Class
