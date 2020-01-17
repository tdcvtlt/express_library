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

Partial Class Reports_Contracts_ContractsWithChangedStatus
    Inherits System.Web.UI.Page

#Region "Page Variables"
    Private reportPath As String = "REPORTFILES/ContractsWithChangedStatus.rpt"
    Private report As New ReportDocument
#End Region

#Region "Page Events & Handlers"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Me.IsPostBack = False) Then            
            Me.ContractStatus.DataSource = (DirectCast(New ComboItem(), IComboItems)).Load()
            Me.ContractStatus.DataTextField = "NAME"
            Me.ContractStatus.DataValueField = "ID"
            Me.ContractStatus.DataBind()
        End If

        If (String.IsNullOrEmpty(Me.SDATE.Selected_Date) Or _
            String.IsNullOrEmpty(Me.EDATE.Selected_Date)) Then

            Session("Crystal") = Nothing
        End If



        If Not Session("Crystal") Is Nothing Then
            If Not Session("UserID") Is Nothing Then
                CrystalViewer.ReportSource = Session("Crystal")
            Else
                Response.Redirect("http://crms.kingscreekplantation.com/crmsnet/logon.aspx")
            End If
        End If


    End Sub

    Protected Sub RunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunReport.Click

        If (Not Session("Crystal") Is Nothing) Then
            Session("Crystal") = Nothing
            CrystalViewer.Visible = False
        End If

        If (String.IsNullOrEmpty(Me.SDATE.Selected_Date) Or String.IsNullOrEmpty(Me.EDATE.Selected_Date)) Then Return
        If (SelectStatus.GetLength(0) = 0) Then Return


        report.FileName = Server.MapPath(reportPath)
        report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        report.SetParameterValue("SDATE", Me.SDATE.Selected_Date.Trim())
        report.SetParameterValue("EDATE", Me.EDATE.Selected_Date.Trim())
        report.SetParameterValue("STATUS", String.Join(",", SelectStatus))

        report.Load(Server.MapPath(reportPath))

        Session("Crystal") = report
        CrystalViewer.ReportSource = Session("Crystal")

        CrystalViewer.Visible = True

    End Sub


#End Region



#Region "User Routines"

    Private Function SelectStatus() As String()
        Dim list() As String = Nothing
        Dim i As Integer = 0
        For Each li As ListItem In ContractStatus.Items
            If (li.Selected = True) Then
                Array.Resize(list, i + 1)
                list(i) = "'" & li.Text & "'"
                i += 1
            End If
        Next

        Return list
    End Function


#End Region
End Class





#Region "Interfaces & Classes"

Interface IComboItems
    Function Load() As ArrayList
End Interface

Class ComboItem : Implements IComboItems

    Dim cnn As SqlConnection
    Dim cmd As SqlCommand
    Dim rdr As SqlDataReader
    Dim cnx As String = Resources.Resource.cns
    Dim sql As String = String.Empty
    Dim arl As ArrayList

    Public Function Load() As ArrayList Implements IComboItems.Load
        arl = New ArrayList()

        Using cnn As New SqlConnection(cnx)
            sql = "Select ComboItemID, ComboItem from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID " & _
                    "where comboname = 'contractstatus' and active = 1 ORDER BY ComboItem"

            cnn.Open()
            cmd = New SqlCommand(sql, cnn)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            If (rdr.HasRows = True) Then
                Do While (rdr.Read())
                    arl.Add(New IdName With {.ID = rdr.Item(0), .Name = rdr.Item(1)})
                Loop
            End If
        End Using
        Return arl
    End Function
End Class

#End Region
