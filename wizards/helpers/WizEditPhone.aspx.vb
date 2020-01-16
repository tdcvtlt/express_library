Imports System.Data
Partial Class wizards_WizEditPhone
    Inherits System.Web.UI.Page
    Dim oAddress As clsPhone


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oSecurity As New Security
        If Not (oSecurity.Is_Logged_On(Session)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "security", "window.close();", True)
            oSecurity = Nothing
            Exit Sub
        End If
        Dim dt As DataTable
        dt = Session(Request("Table"))
        Dim iIndex As Integer = IIf(IsNumeric(Request("PhoneID")) And Request("PhoneID") < 0, CInt(Request("PhoneID")) - 1, -1)

        oSecurity = Nothing

        If Not IsPostBack Then
            siType.Connection_String = Resources.Resource.cns
            siType.Label_Caption = ""
            siType.ComboItem = "Phone"
            siType.Load_Items()
            Fill_Object()
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
        txtPhoneID.Text = IIf(IsNumeric(Request("PhoneID")), Request("PhoneID"), "0")
        If dt Is Nothing Then
            dt.Columns.Add("ID")
            dt.Columns.Add("Active")
            dt.Columns.Add("Number")
            dt.Columns.Add("Extension")
            dt.Columns.Add("TypeID")
            dt.Columns.Add("Type")
            dt.Columns.Add("Dirty")
            bNewRow = True
        Else
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    row = dt.Rows(i)
                    If CInt(row("ID")) = CInt(txtPhoneID.Text) Then
                        bNewRow = False
                        Exit For
                    Else
                        bNewRow = True
                    End If
                Next
            Else
                dt.Columns.Add("ID")
                dt.Columns.Add("Active")
                dt.Columns.Add("Number")
                dt.Columns.Add("Extension")
                dt.Columns.Add("TypeID")
                dt.Columns.Add("Type")
                dt.Columns.Add("Dirty")
                bNewRow = True
            End If
        End If
        If bNewRow Then
            row = dt.NewRow
            row("ID") = 0
            row("Active") = False
            row("TypeID") = 0
            dt.Rows.Add(row)
        End If

        ckActive.Checked = row("Active")
        txtExtension.Text = row("Extension") & ""
        txtNumber.Text = row("Number") & ""
        siType.Selected_ID = row("TypeID")
        Session(Request("Table")) = dt
    End Sub

    Private Sub Save_Values()
        Dim dt As DataTable = Session(Request("Table"))
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
                If CInt(row("ID")) = CInt(txtPhoneID.Text) Then
                    bNewRow = False
                    Exit For
                Else
                    bNewRow = True
                End If
            Next
        End If

        If bNewRow Then row = dt.NewRow
        If txtPhoneID.text = 0 Then
            row("ID") = holder
        Else
            row("ID") = txtPhoneID.text
        End If
        'row("ProspectID") = txtProspectID.Text
        row("Number") = txtNumber.Text & ""
        row("Extension") = txtExtension.Text & ""
        row("Active") = ckActive.Checked
        row("TypeID") = siType.Selected_ID
        row("Dirty") = True
        If bNewRow Then dt.Rows.Add(row)

        Session(Request("Table")) = dt
        Session("EditPhone") = dt
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Save_Values()
        'Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lbRefresh','');window.close();", True)

    End Sub
End Class
