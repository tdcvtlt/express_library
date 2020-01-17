
Imports System

Partial Class controls_EquiantLoanInformation
    Inherits System.Web.UI.UserControl
    Private Equiant As New clsEquiant
    Private _Account As String = ""
    Public Property EquiantAccount() As String
        Get
            Return _Account
        End Get
        Set(ByVal value As String)
            _Account = value
        End Set
    End Property
    Private Sub controls_EquiantLoanInformation_Load(sender As Object, e As EventArgs) Handles Me.Load
        If _Account <> "" Then Display()
    End Sub

    Private Sub Display()
        Try
            Dim info As clsEquiant.Loan = Equiant.LoanInformation(_Account)
            If info Is Nothing Then
                'If Equiant.Err <> "" Then Response.Write(Equiant.Err)
            Else
                lblPB.Text = info.PrincipalBalance.ToString("c")
                lblAI.Text = info.AccruedInterest.ToString("c")
                lblLCB.Text = info.LateCharges.ToString("c")
                lblOB.Text = "$0.00"
                lblID.Text = "$0.00"
                lblPO1.Text = info.PayoffAmount.ToString("c")
                lblPO2.Text = (info.PayoffAmount + (10 * info.PerDiem)).ToString("c")
                lblBC.Text = info.BringCurrent.ToString("c")
                lblNPD.Text = info.NextPaymentDueDate
                lblRPA.Text = (info.CPAT - info.ImpoundAmount).ToString("c")
                lblIA.Text = info.ImpoundAmount.ToString("c")
                lblTP.Text = (info.CPAT).ToString("c")
                lblPP.Text = info.PartialPaymentAmount.ToString("c")
                If info.RemainingTerm = 0 Then
                    lblPMRT.Text = "0/" & info.RemainingTerm
                Else
                    lblPMRT.Text = (info.Term - info.RemainingTerm).ToString & "/" & info.RemainingTerm
                End If
                lblNSF.Text = "0"
                lblEq.Text = info.EquityBalance.ToString("c")
                lblLPD.Text = info.LastApplyDate.ToString("d")
                lblLPA.Text = info.LastApplyAmount.ToString("c")
                If Equiant.Err <> "" Then Response.Write(Equiant.Err)
            End If
        Catch ex As Exception

        End Try


    End Sub

    Public Sub Set_Account(ByVal ContractID As Integer, Conversion As Boolean)
        Dim cn As New System.Data.SqlClient.SqlConnection(Resources.Resource.cns)
        Dim cm As New System.Data.SqlClient.SqlCommand("Select * from v_ContractInventory where contractid = " & ContractID, cn)
        Dim dr As Data.SqlClient.SqlDataReader
        Try
            cn.Open()
            dr = cm.ExecuteReader
            If Not dr.HasRows Then
                dr.Close()
                cm.CommandText = "Select * from v_ContractInventoryHistory where contractid = " & ContractID
                dr = cm.ExecuteReader
            End If
            If dr.HasRows Then
                dr.Read()
                Select Case dr("SaleType")
                    Case "Estates"
                        _Account = "131" & If(Conversion, "000", "100") & ContractID.ToString
                    Case "Townes"
                        _Account = "133" & If(Conversion, "000", "100") & ContractID.ToString
                    Case "Cottage"
                        _Account = "132" & If(Conversion, "000", "100") & ContractID.ToString
                    Case "Combo"
                        _Account = "132" & If(Conversion, "000", "100") & ContractID.ToString
                    Case Else
                        _Account = ""
                End Select
            End If
        Catch ex As Exception
        Finally
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            dr = Nothing
            cm = Nothing
            cn = Nothing
        End Try
        Display()
    End Sub
End Class
