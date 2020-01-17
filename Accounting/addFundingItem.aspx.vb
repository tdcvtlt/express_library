Imports Microsoft.VisualBasic
Partial Class Accounting_addFundingItem
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oContract As New clsContract
        Dim oFunding As New clsFunding
        Dim oFundingItem As New clsFundingItems
        Dim sErr As String = ""
        Dim bProceed As Boolean = True
        oFunding.FundingID = Request("FundingID")
        oFunding.Load()
        If oFunding.ExitFunding Then
            If Left(txtKCP.Text, 1) <> "T" And Left(txtKCP.Text, 1) <> "U" Then
                bProceed = False
                sErr = "Only Exit Deals May Be Added To This Funding."
            Else
                If oContract.Verify_Contract(txtKCP.Text) = False Then
                    bProceed = False
                    sErr = txtKCP.Text & " Is Not A Valid Contract."
                Else
                    If Request("cxl") = "False" Then
                        oContract.ContractID = oContract.Get_Contract_ID(txtKCP.Text)
                        oContract.Load()
                        Dim oCombo As New clsComboItems
                        If oCombo.Lookup_ComboItem(oContract.StatusID) <> "Active" And oCombo.Lookup_ComboItem(oContract.StatusID) <> "Suspense" And oCombo.Lookup_ComboItem(oContract.StatusID) <> "Developer" Then
                            bProceed = False
                            sErr = "Contracts with a Status of " & oCombo.Lookup_ComboItem(oContract.StatusID) & " Can Not Be Added to Funding."
                        End If
                        oCombo = Nothing
                    End If
                End If
            End If
        Else
            If Left(txtKCP.Text, 1) = "T" Or Left(txtKCP.Text, 1) = "U" Then
                bProceed = False
                sErr = "Exit Deals May Not Be Added To This Funding."
            Else
                If oContract.Verify_Contract(txtKCP.Text) = False Then
                    bProceed = False
                    sErr = txtKCP.Text & " Is Not A Valid Contract."
                Else
                    If Request("cxl") = "False" Then
                        oContract.ContractID = oContract.Get_Contract_ID(txtKCP.Text)
                        oContract.Load()
                        Dim oCombo As New clsComboItems
                        If oCombo.Lookup_ComboItem(oContract.StatusID) <> "Active" And oCombo.Lookup_ComboItem(oContract.StatusID) <> "Suspense" And oCombo.Lookup_ComboItem(oContract.StatusID) <> "Developer" Then
                            bProceed = False
                            sErr = "Contracts with a Status of " & oCombo.Lookup_ComboItem(oContract.StatusID) & " Can Not Be Added to Funding."
                        End If
                        oCombo = Nothing
                    End If
                End If
            End If
        End If

        If bProceed Then
            If oFundingItem.Validate_Deal_On_Funding(txtKCP.Text, Request("FundingID")) Then

            Else
                bProceed = False
                sErr = "This Contract Is Already on This Funding And Can Not Be Added Again."
            End If
        End If

        If bProceed Then
            Dim preSale As String = "False"
            If chkPresales.Checked Then
                preSale = "True"
            End If
            oFundingItem.UserID = Session("UserDBID")
            If oFundingItem.Add_Deal(txtKCP.Text, Request("cxl"), preSale, Request("FundingID")) Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.Refresh_Funding();window.close();", True)
            Else
                lblErr.Text = oFundingItem.Err
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)
        End If
        oContract = Nothing
        oFunding = Nothing
        oFundingItem = Nothing
    End Sub
End Class
