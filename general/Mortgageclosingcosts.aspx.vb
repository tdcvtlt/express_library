Imports System.Data

Partial Class general_Mortgageclosingcosts
    Inherits System.Web.UI.Page
    Dim oMCC As clsMortgageClosingCosts
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNumeric(Request("MortgageID")) Then
            Close()
        ElseIf Not IsPostBack Then
            Refresh_Grid()
        End If
    End Sub

    Private Sub Close()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
    End Sub

    Public Sub ShowEdit()
        Panel1.Visible = False
        For Each row As GridViewRow In gvMCC.Rows
            Dim cb As CheckBox = row.FindControl("ckEdit")
            If cb.Checked Then
                Panel1.Visible = True
                Label1.Text = row.Cells(1).Text
                TextBox1.Text = row.Cells(2).Text
                hfRow.Value = row.RowIndex
                Exit For
            End If
        Next
    End Sub
    Protected Sub gvMCC_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(3).Visible = False
        End If
    End Sub
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        oMCC = New clsMortgageClosingCosts
        Dim dv As DataView = CType(oMCC.List(Request("MortgageID")).Select(DataSourceSelectArguments.Empty), DataView)
        If dv.Table.Rows(hfRow.Value)("Optional") = True Then
            oMCC.UserID = Session("UserDBID")
            oMCC.MortgageClosingCostID = dv.Table.Rows(hfRow.Value)("ID")
            oMCC.Load()
            oMCC.Amount = IIf(IsNumeric(TextBox1.Text), TextBox1.Text, oMCC.Amount)
            oMCC.Save()
            Dim oMortgage As New clsMortgage
            Dim oInvoice As New clsInvoices
            oMortgage.UserID = Session("UserDBID")
            oInvoice.UserID = Session("UserDBID")
            oMortgage.MortgageID = Request("MortgageID")
            oMortgage.Load()
            oInvoice.InvoiceID = oMortgage.CCInvoiceID
            oInvoice.Load()
            oInvoice.Amount = oMCC.Get_Total_CC(oMortgage.MortgageID)
            oInvoice.Save()
            oMortgage.CCTotal = oMCC.Get_Total_CC(oMortgage.MortgageID)
            If oMortgage.CCFinanced <> 0 Then
                oMortgage.CCFinanced = oMCC.Get_Total_CC(oMortgage.MortgageID)
                oMortgage.Save()
            End If
            oMortgage = Nothing
            oInvoice = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$DP_Financials$lbRefresh','');", True)
        End If
        oMCC = Nothing
        Refresh_Grid()
    End Sub

    Private Sub Refresh_Grid()
        oMCC = New clsMortgageClosingCosts
        gvMCC.DataSource = oMCC.List(Request("MortgageID"))
        gvMCC.DataBind()
        Panel1.Visible = False
        oMCC = Nothing
    End Sub
End Class
