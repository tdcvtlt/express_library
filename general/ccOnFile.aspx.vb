Imports System.Data

Partial Class general_ccOnFile
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If Request("ID") = "-1" And CheckSecurity("CreditCards", "Add", , , Session("UserDBID")) Then

                txtName.ReadOnly = False
                txtNumber.ReadOnly = False
                txtCVV.ReadOnly = False

                txtCity.Visible = True
                txtPostalCode.Visible = True
                txtAddress.Visible = True

                city_label.Visible = True
                address_label.Visible = True
                postal_label.Visible = True
                state_label.Visible = True


                siState.Connection_String = Resources.Resource.cns
                siState.Label_Caption = ""
                siState.ComboItem = "State"
                siState.Load_Items()

                siState.Visible = True

                For i = Year(System.DateTime.Now) - 7 To Year(System.DateTime.Now) + 7
                    ddYear.Items.Add(New ListItem(CStr(Right(i, 2)), CStr(Right(i, 2))))
                Next

                For i = 1 To 12
                    If i < 10 Then
                        ddMonth.Items.Add(New ListItem("0" & i, "0" & i))
                    Else
                        ddMonth.Items.Add(New ListItem(i, i))
                    End If
                Next

                ddYear.SelectedValue = (DateTime.Now.Year + 2).ToString().Substring(2, 2)


            Else

                Dim oCC As New clsCreditCard
                Dim oCombo As New clsComboItems
                oCC.CreditCardID = Request("ID")
                oCC.Load()
                lblType.Text = oCombo.Lookup_ComboItem(oCC.TypeID)
                txtNumber.Text = oCC.Number
                txtCVV.Text = oCC.Security
                txtName.Text = oCC.NameOnCard
                Dim i As Integer = 0
                For i = Year(System.DateTime.Now) - 7 To Year(System.DateTime.Now) + 7
                    ddYear.Items.Add(New ListItem(CStr(Right(i, 2)), CStr(Right(i, 2))))
                Next
                For i = 1 To 12
                    If i < 10 Then
                        ddMonth.Items.Add(New ListItem("0" & i, "0" & i))
                    Else
                        ddMonth.Items.Add(New ListItem(i, i))
                    End If
                Next

                ddMonth.SelectedValue = Left(oCC.Expiration, 2)
                ddYear.SelectedValue = Right(oCC.Expiration, 2)

                oCC = Nothing
                oCombo = Nothing

            End If
            Dim sds As SqlDataSource = (New clsCCMerchantAccount).List_Accounts()
            Dim dv As DataView = CType(sds.Select(DataSourceSelectArguments.Empty), DataView)
            Dim code As String = "function Get_Key(index){switch (index){ "
            For Each dvr As DataRowView In dv
                code &= "case """ & dvr("AccountID") & """: pkey='" & dvr("publictoken") & "'; break; "
            Next
            code &= "default: pkey=''; break; };};"
            sds = Nothing
            dv = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "pk", code, True)
            ddAcct.Items.Add(New ListItem("KCP Escrow (5)", 5))
            If CheckSecurity("CreditCards", "MultipleAccountsOnFile", , , Session("UserDBID")) Then
                'If ((Session.Item("LastName") = "Hill" And Session.Item("FirstName") = "Richard") Or (Session.Item("LastName") = "Wyatt" And Session.Item("FirstName") = "Alison") Or (Session.Item("LastName") = "Benton" And Session.Item("FirstName") = "Matt") Or Session.Item("LastName") = "Nguyen" And Session.Item("FirstName") = "Trevor") Then
                ddAcct.Items.Add(New ListItem("VRC (4)", 4))
                ddAcct.Items.Add(New ListItem("King's Creek Club Explore Points (8)", 8))
                ddAcct.Items.Add(New ListItem("KCP Maintenance Fees (1)", 1))
                ddAcct.Items.Add(New ListItem("VRC Vacations (10)", 10))
                ddAcct.Items.Add(New ListItem("KCPOA Lodging (13)", 13))
                ddAcct.Items.Add(New ListItem("KCPOA Club Dues (14)", 14))
                ddAcct.Items.Add(New ListItem("KCPOA Rentals (15)", 15))
                'End If
            End If
        End If
    End Sub


    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click

        If Request("ID") = "-1" And CheckSecurity("CreditCards", "Add", , , Session("UserDBID")) Then

            Dim c = New clsCreditCard()
            c.CreditCardID = 0
            c.Load()

            c.Expiration = ddMonth.SelectedValue & ddYear.SelectedValue
            c.UpdateCard = False
            c.Number = txtNumber.Text.Trim()
            c.ProspectID = Request("prospectid")
            'c.Security = txtCVV.Text.Trim()

            c.NameOnCard = txtName.Text.Trim()
            c.Address = txtAddress.Text.Trim()
            c.PostalCode = txtPostalCode.Text.Trim()
            c.City = txtCity.Text.Trim()
            c.StateID = siState.Selected_ID
            c.Token = hfTokenValue.Value
            c.TypeID = (New clsComboItems).Lookup_ID("CreditCardType", hfCardType.Value)
            c.UpdateCard = False
            c.ReadyToImport = True
            c.ImportedID = (ddAcct.SelectedValue + 100) * -1
            If Not c.Save() Then
                lblErr.Text = c.Error_Message
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "window.opener.Refresh_CC();window.close();", True)
            End If

        ElseIf Request("ID") <> "-1" And Request("ID") <> "0" And CheckSecurity("CreditCards", "EditExp", , , Session("UserDBID")) Then
            Dim oCC As New clsCreditCard
            oCC.CreditCardID = Request("ID")
            oCC.Load()
            If Left(oCC.Expiration, 2) <> ddMonth.SelectedValue Or Right(oCC.Expiration, 2) <> ddYear.SelectedValue Then
                oCC.Expiration = ddMonth.SelectedValue & ddYear.SelectedValue
                oCC.UpdateCard = True
                oCC.CCString = ""
                oCC.Save()
            End If
            oCC = Nothing
            'ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "window.opener.Refresh_CC();window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('Access Denied');", True)

        End If

    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "window.close();", True)
    End Sub
End Class
