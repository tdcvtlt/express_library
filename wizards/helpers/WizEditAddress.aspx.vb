Imports System.Data

Partial Class wizards_EditAddress
    Inherits System.Web.UI.Page
    Dim oAddress As clsAddress


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oSecurity As New Security
        If Not (oSecurity.Is_Logged_On(Session)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "security", "window.close();", True)
            oSecurity = Nothing
            Exit Sub
        End If
        Dim dt As DataTable
        dt = Session(Request("table"))
        Dim iIndex As Integer = IIf(IsNumeric(Request("AddressID")) And Request("AddressID") < 0, CInt(Request("AddressID")) - 1, -1)
        'oAddress = IIf(Not (Session("EditAddress") Is Nothing), Session("EditAddress"), New clsAddress)

        oSecurity = Nothing

        If Not IsPostBack Then
            'oAddress.Load()
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
            Fill_Object()
            'Load_Values()
            'Session("EditAddress") = oAddress
        End If
    End Sub

    Private Sub Fill_Object()
        Dim bNewRow As Boolean = False
        Dim row As DataRow
        Dim dt As New DataTable
        If Session(Request("Table")) Is Nothing Then
        Else
            dt = Session(Request("Table"))
        End If
        txtAddressID.Text = IIf(IsNumeric(Request("AddressID")), Request("AddressID"), "0")
        If dt Is Nothing Then
            dt.Columns.Add("ID")
            dt.Columns.Add("Active")
            dt.Columns.Add("Address1")
            dt.Columns.Add("Address2")
            dt.Columns.Add("City")
            dt.Columns.Add("StateID")
            dt.Columns.Add("State")
            dt.Columns.Add("Zip")
            dt.Columns.Add("Region")
            dt.Columns.Add("CountryID")
            dt.Columns.Add("Country")
            dt.Columns.Add("Contract")
            dt.Columns.Add("TypeID")
            dt.Columns.Add("Type")
            dt.Columns.Add("Dirty")
            bNewRow = True
        Else
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    row = dt.Rows(i)
                    If CInt(row("ID")) = CInt(txtAddressID.Text) Then
                        bNewRow = False
                        Exit For
                    Else
                        bNewRow = True
                    End If
                Next
            Else
                If dt.columns.count < 2 Then
                    dt.Columns.Add("ID")
                    dt.Columns.Add("Active")
                    dt.Columns.Add("Address1")
                    dt.Columns.Add("Address2")
                    dt.Columns.Add("City")
                    dt.Columns.Add("StateID")
                    dt.Columns.Add("State")
                    dt.Columns.Add("Zip")
                    dt.Columns.Add("Region")
                    dt.Columns.Add("CountryID")
                    dt.Columns.Add("Country")
                    dt.Columns.Add("Contract")
                    dt.Columns.Add("TypeID")
                    dt.Columns.Add("Type")
                    dt.Columns.Add("Dirty")
                End If
                bNewRow = True
            End If

        End If
        If bNewRow Then
            row = dt.NewRow
            row("ID") = 0
            row("Active") = False
            row("StateID") = 0
            row("CountryID") = 0
            row("TypeID") = 0
            row("Contract") = False
            row("Address1") = ""
            row("Address2") = ""
            row("City") = ""
            row("Zip") = ""
            row("Region") = ""
            dt.Rows.Add(row)
        End If

        ckActive.Checked = row("Active")
        txtAddress1.Text = row("Address1") & ""
        txtAddress2.Text = row("Address2") & ""
        txtCity.Text = row("City") & ""
        siState.Selected_ID = row("StateID")
        txtZip.Text = row("Zip") & ""
        txtRegion.Text = row("Region") & ""
        siCountry.Selected_ID = iif(isNumeric(row("CountryID")),row("CountryID"),0)
        siType.Selected_ID = row("TypeID")
        ckContractAddress.Checked = row("Contract")
        Session(Request("Table")) = dt


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
        Dim dt As DataTable = Session(Request("Table"))
        'Dim oAdd As clsAddress = Session("EditAddress")
        Dim row As DataRow
        Dim bNewRow As Boolean = False
        Dim holder As Integer = 0

        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                row = dt.Rows(i)
                If CInt(row("ID")) <= 0 Then
                    If holder = 0 Then
                        holder = -1
                    Else
                        holder = holder - 1
                    End If
                End If
                If CInt(row("ID")) = CInt(txtAddressID.Text) Then
                    bNewRow = False
                    Exit For
                Else
                    bNewRow = True
                End If
            Next
        End If

        If bNewRow Then row = dt.NewRow
        If txtAddressID.Text = 0 Then
            row("ID") = holder
        Else
            row("ID") = txtAddressID.Text
        End If
        'row("ProspectID") = txtProspectID.Text
        row("Address1") = txtAddress1.Text
        row("Address2") = txtAddress2.Text
        row("City") = txtCity.Text
        row("StateID") = siState.Selected_ID
        row("State") = siState.SelectedName
        row("Zip") = txtZip.Text
        row("CountryID") = siCountry.Selected_ID
        row("Country") = siCountry.SelectedName
        row("Active") = ckActive.Checked
        row("Region") = txtRegion.Text
        row("TypeID") = siType.Selected_ID
        row("Contract") = ckContractAddress.Checked
        'row("UserID") = Session("UserDBID")
        row("Dirty") = True
        If bNewRow Then dt.Rows.Add(row)

        Session(Request("Table")) = dt
        Session("EditAddress") = dt
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Save_Values()
        'Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lbRefresh','');window.close();", True)

    End Sub
End Class
