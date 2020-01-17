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

Imports Resources.Resource

Partial Class Reports_OwnerServices_BankReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Me.IsPostBack = False) Then

            Dim ddl() As DropDownList = {Me.Type, Me.Exchange, Me.Season, Me.Status, Me.Size, Me.Deposit, Me.Used, Me.Usage}

            For Each ctr As DropDownList In ddl
                ctr.Items.Add(String.Empty)
                ctr.AppendDataBoundItems = True
                ctr.DataBind()
            Next

            Me.Size.ClearSelection()
            Me.Type.ClearSelection()
            Me.Deposit.ClearSelection()
            Me.Used.ClearSelection()
            Me.Status.ClearSelection()
            Me.Exchange.ClearSelection()
            Me.Season.ClearSelection()

            Me.Size.DataSource = CType((New clsComboItems().Load_ComboItems("RoomType")).Select(DataSourceSelectArguments.Empty), DataView).ToTable
            Me.Size.DataTextField = "ComboItem"
            Me.Size.DataValueField = "ComboItemID"
            Me.Size.DataBind()

            Me.Type.DataSource = CType((New clsComboItems().Load_ComboItems("InventoryType")).Select(DataSourceSelectArguments.Empty), DataView).ToTable
            Me.Type.DataTextField = "ComboItem"
            Me.Type.DataValueField = "ComboItemID"
            Me.Type.DataBind()

            'Deposit Year
            Dim now As DateTime = DateTime.Now
            For i As Integer = -4 To 2 Step 1
                Me.Deposit.Items.Add(New ListItem(now.AddYears(i).Year, now.AddYears(i).Year))
            Next

            'Year Used
            For i As Integer = -4 To 2 Step 1
                Me.Used.Items.Add(New ListItem(now.AddYears(i).Year, now.AddYears(i).Year))
            Next

            'Usage - Hardcoded for Frequency table
            Dim usage() = {"Annual", "Biennial", "Triennial"}
            For Each s In usage
                Me.Usage.Items.Add(New ListItem(s, Array.IndexOf(usage, s) + 1))
            Next

            Me.Exchange.DataSource = CType((New clsComboItems().Load_ComboItems("ReservationSubType")).Select(DataSourceSelectArguments.Empty), DataView).ToTable
            Me.Exchange.DataTextField = "ComboItem"
            Me.Exchange.DataValueField = "ComboItemID"
            Me.Exchange.DataBind()

            Me.Season.DataSource = CType((New clsComboItems().Load_ComboItems("Season")).Select(DataSourceSelectArguments.Empty), DataView).ToTable
            Me.Season.DataTextField = "ComboItem"
            Me.Season.DataValueField = "ComboItemID"
            Me.Season.DataBind()

            Me.Status.DataSource = CType((New clsComboItems().Load_ComboItems("BankingStatus")).Select(DataSourceSelectArguments.Empty), DataView).ToTable
            Me.Status.DataTextField = "ComboItem"
            Me.Status.DataValueField = "ComboItemID"
            Me.Status.DataBind()
        End If
    End Sub

    Protected Sub btSubmit_Click(sender As Object, e As EventArgs) Handles btSubmit.Click


        Dim sql As String = String.Format("select * from t_bankedunits where statusid like '{0}%' and unitsize like '{1}" &
                         "%' and Exchangeid like '{2}%' and usageyear like '{3}" &
                         "%' and seasonid like '{4}%' and unittypeid like '{5}" &
                         "%' and deposityear like '{6}%' and frequencyid like '{7}%'",
                        Status.SelectedValue,
                        Size.SelectedItem.Text,
                        Exchange.SelectedValue,
                        Used.SelectedValue,
                        Season.SelectedValue,
                        Type.SelectedValue,
                        Deposit.SelectedValue,
                        Usage.SelectedValue)

        'Status, UnitSize, Exchange, YearUsed, Season, UnitType, DepositYear, Usage)
        Using cn As New SqlConnection(cns)
            Using ada As New SqlDataAdapter(sql, cn)
                Dim dt As New DataTable()
                ada.Fill(dt)

                Me.ResultCount.Text = dt.Rows.Count.ToString()
            End Using
        End Using

    End Sub
End Class
