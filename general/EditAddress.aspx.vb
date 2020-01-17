
Partial Class general_EditAddress
    Inherits System.Web.UI.Page
    Dim oAddress As New clsAddress

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oSecurity As New Security
        If Not (oSecurity.Is_Logged_On(Session)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "security", "window.close();", True)
            oSecurity = Nothing
            Exit Sub
        End If
        oSecurity = Nothing
        oAddress.AddressID = IIf(IsNumeric(Request("AddressID")), Request("AddressID"), 0)
        oAddress.ProspectID = IIf(IsNumeric(Request("ProspectID")), Request("ProspectID"), 0)
        If Not IsPostBack Then
            oAddress.Load()
            siState.Connection_String = Resources.Resource.cns
            siState.Label_Caption = ""
            siState.ComboItem = "State"
            siState.Load_Items()
            siCountry.Connection_String = Resources.Resource.cns
            siCountry.Label_Caption = ""
            siCountry.ComboItem = "Country"
            siCountry.Load_Items()
            siType.Connection_String = Resources.Resource.cns
            siType.Label_Caption = ""
            siType.ComboItem = "AddressType"
            siType.Load_Items()
            Load_Values()
        End If
    End Sub

    Private Sub Load_Values()
        With oAddress
            txtAddressID.Text = .AddressID
            txtProspectID.Text = .ProspectID
            ckActive.Checked = .ActiveFlag
            txtAddress1.Text = .Address1
            txtAddress2.Text = .Address2
            txtCity.Text = .City
            siState.Selected_ID = .StateID
            txtZip.Text = .PostalCode
            txtRegion.Text = .Region
            siCountry.Selected_ID = .CountryID
            siType.Selected_ID = .TypeID
            ckContractAddress.Checked = .ContractAddress
        End With
    End Sub

    Private Sub Save_Values()
        With oAddress
            .AddressID = txtAddressID.Text
            .ProspectID = txtProspectID.Text
            .ActiveFlag = ckActive.Checked
            .Address1 = txtAddress1.Text
            .Address2 = txtAddress2.Text
            .City = txtCity.Text
            .StateID = siState.Selected_ID
            .PostalCode = txtZip.Text
            .Region = txtRegion.Text
            .CountryID = siCountry.Selected_ID
            .TypeID = siType.Selected_ID
            .ContractAddress = ckContractAddress.Checked
            .UserID = Session("UserDBID")
            .Save()
            txtAddressID.Text = .AddressID
            lblError.Text = .Error_Message
        End With
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim prevAddress = New clsAddress
        prevAddress.ProspectID = oAddress.ProspectID
        prevAddress.AddressID = oAddress.AddressID
        prevAddress.Load()

        Save_Values()

        Dim diff = False

        If prevAddress.Address1.Trim <> oAddress.Address1.Trim() Then
            oAddress.Address1 = "* " + oAddress.Address1
            diff = True
        End If

        If prevAddress.Address2.Trim <> oAddress.Address2.Trim() Then
            oAddress.Address2 = "* " + oAddress.Address2.Trim()
            diff = True
        End If

        If prevAddress.City.Trim <> oAddress.City.Trim() Then
            oAddress.City = "* " + oAddress.City.Trim()
            diff = True
        End If

        If prevAddress.StateID <> oAddress.StateID Then
            oAddress.StateID = oAddress.StateID
            diff = True
        End If

        If prevAddress.PostalCode.Trim <> oAddress.PostalCode.Trim() Then
            oAddress.PostalCode = "* " + oAddress.PostalCode.Trim()
            diff = True
        End If

        If diff Then
            Dim l = New List(Of String)
            l.Add("AddressChange@kingscreekplantation.com")
            prevAddress.Notify_Address_Change_By_Owner(oAddress, l)
        End If


        If oAddress.Error_Message = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.Refresh_Address();window.close();", True)
        End If
    End Sub
End Class
