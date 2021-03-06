﻿Imports System.Data
Partial Class wizards_WizEditEmail
    Inherits System.Web.UI.Page
    Dim oEmail As clsEmail


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oSecurity As New Security
        If Not (oSecurity.Is_Logged_On(Session)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "security", "window.close();", True)
            oSecurity = Nothing
            Exit Sub
        End If
        Dim dt As DataTable
        dt = Session(Request("Table"))
        oSecurity = Nothing

        If Not IsPostBack Then
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
        ' = Session(Request("Table"))
        txtEmailID.Text = IIf(IsNumeric(Request("EmailID")), Request("EmailID"), 0)
        If dt Is Nothing Then
            dt.Columns.Add("ID")
            dt.Columns.Add("Active")
            dt.Columns.Add("IsPrimary")
            dt.Columns.Add("Email")
            dt.Columns.Add("Dirty")
            bNewRow = True
        Else
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    row = dt.Rows(i)
                    If CInt(row("ID")) = CInt(txtEmailID.Text) Then
                        bNewRow = False
                        Exit For
                    Else
                        bNewRow = True
                    End If
                Next
            Else
                dt.Columns.Add("ID")
                dt.Columns.Add("Active")
                dt.Columns.Add("IsPrimary")
                dt.Columns.Add("Email")
                dt.Columns.Add("Dirty")
                bNewRow = True
            End If
        End If
        If bNewRow Then
            row = dt.NewRow
            row("ID") = 0
            row("Active") = False
            row("IsPrimary") = False
            dt.Rows.Add(row)
        End If

        ckActive.Checked = row("Active")
        ckPrimary.Checked = row("IsPrimary")
        txtEmail.Text = row("Email") & ""
        Session(Request("Table")) = dt
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
                If CInt(row("ID")) = CInt(txtEmailID.Text) Then
                    bNewRow = False
                    Exit For
                Else
                    bNewRow = True
                End If
            Next
        End If

        If bNewRow Then row = dt.NewRow
        If txtEmailID.Text = 0 Then
            row("ID") = holder
        Else
            row("ID") = txtEmailID.Text
        End If
        'row("ProspectID") = txtProspectID.Text
        row("Email") = txtEmail.Text
        row("Active") = ckActive.Checked
        row("IsPrimary") = ckPrimary.Checked
        'row("UserID") = Session("UserDBID")
        row("Dirty") = True
        If bNewRow Then dt.Rows.Add(row)

        Session(Request("Table")) = dt
        Session("EditEmail") = dt
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Save_Values()
        'Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lbRefresh','');window.close();", True)
    End Sub
End Class
