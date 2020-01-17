Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data
Imports System.Data.SqlClient

Partial Class Reports_Default
    Inherits System.Web.UI.Page


#Region "Page Variables"
    Private reportPath As String = "REPORTFILES/TOURDETAILFORVENDORS.RPT"
    Private report As New ReportDocument
#End Region


#Region "Page Events And Handlers"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Me.IsPostBack = False) Then
            Using cnn As New SqlConnection(Resources.Resource.cns)
                Dim sql As String = String.Format("Select ComboItemID, ComboItem from t_Comboitems i inner join t_Combos c on c.comboid = " & _
                                                  "i.comboid where (active = 1) and comboname = 'tourlocation' order by comboitem")
                Using cmd As New SqlCommand(sql, cnn)
                    cnn.Open()
                    Dim rdr As IDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    TourLocation.Items.Add(New ListItem("All"))
                    TourLocation.AppendDataBoundItems = True

                    Dim all() As String = New String() {""}
                    Dim i As Integer = 0
                    Do While (rdr.Read())
                        TourLocation.Items.Add(New ListItem(rdr.Item(1), rdr.Item(0)))
                        i += 1
                        Array.Resize(all, i)
                        all(i - 1) = rdr.Item(0)                        
                    Loop
                    TourLocation.Items(0).Value = String.Join(New Char() {","}, all)
                    TourLocation.DataTextField = "ComboItemID"
                    TourLocation.DataValueField = "ComboItem"
                End Using

                Me.DataBind()
            End Using
        End If

        If Not Session("Crystal") Is Nothing Then
            If Not Session("UserID") Is Nothing And Me.IsPostBack = True Then
                CrystalViewer.ReportSource = Session("Crystal")
            Else
                If Session("Crystal") Is Nothing Then
                    Response.Redirect("http://crms.kingscreekplantation.com/crmsnet/logon.aspx")
                End If

            End If
        End If
    End Sub


    Protected Sub RunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunReport.Click
        If (Not Session("Crystal") Is Nothing) Then
            Session("Crystal") = Nothing
            CrystalViewer.Visible = False
        End If

        If (String.IsNullOrEmpty(Me.SDATE.Selected_Date) Or String.IsNullOrEmpty(Me.EDATE.Selected_Date)) Then Return

        report.FileName = Server.MapPath(reportPath)
        report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        report.SetParameterValue("SDATE", Me.SDATE.Selected_Date.Trim())
        report.SetParameterValue("EDATE", Me.EDATE.Selected_Date.Trim())
        report.SetParameterValue("TOURLOCATION", New String() {Me.TourLocation.SelectedValue})

        report.Load(Server.MapPath(reportPath))

        Session("Crystal") = report
        CrystalViewer.ReportSource = Session("Crystal")

        CrystalViewer.Visible = True
    End Sub

#End Region



End Class
