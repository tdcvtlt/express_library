Imports System.Data
Partial Class wizards_WizEditNote
    Inherits System.Web.UI.Page
    Dim dt As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oSecurity As New Security
        Dim oNote As New clsNotes
        If Not (oSecurity.Is_Logged_On(Session)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "security", "window.close();", True)
            oSecurity = Nothing
            Exit Sub
        End If
        dt = Session("Notes_Table")

        oSecurity = Nothing

        If Not IsPostBack Then
            If Request("type") <> "" Then
                siReason.Connection_String = Resources.Resource.cns
                siReason.Label_Caption = ""
                siReason.ComboItem = Request("type")
                siReason.Load_Items()
            End If
            Fill_Object()
        End If
    End Sub

    Private Sub Fill_Object()
        Dim bNewRow As Boolean = False
        Dim row As DataRow
        txtNoteID.Text = IIf(IsNumeric(Request("NoteID")), Request("NoteID"), "0")
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                row = dt.Rows(i)
                If CInt(row("ID")) = CInt(txtNoteID.Text) Then
                    bNewRow = False
                    Exit For
                Else
                    bNewRow = True
                End If
            Next
        Else
            bNewRow = True
        End If
        If bNewRow Then
            row = dt.NewRow
            row("ID") = 0
            row("CreatedByID") = Session("UserDBID")
            row("UserName") = Session("UserName")
            row("DateCreated") = System.DateTime.Now
            dt.Rows.Add(row)
        End If

        txtNote.Text = row("Note") & ""
        Session("Notes_Table") = dt
    End Sub

    Private Sub Save_Values()
        Dim dt As DataTable = Session("Notes_Table")
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
                If CInt(row("ID")) = CInt(txtNoteID.Text) Then
                    bNewRow = False
                    Exit For
                Else
                    bNewRow = True
                End If
            Next
        End If

        If bNewRow Then row = dt.NewRow
        If txtNoteID.Text = 0 Then
            row("ID") = holder
        Else
            row("ID") = txtNoteID.Text
        End If
        If Request("type") <> "" Then
            If Request("subType") = "Status" Then
                row("Note") = "Tour Marked " & Request("type") & ". Reason: " & siReason.SelectedName & ". " & txtNote.Text
            Else
                row("Note") = "Sales Rep Marked " & Request("type") & ". Reason " & siReason.SelectedName & ". " & txtNote.Text
            End If
        Else
            row("Note") = txtNote.Text
        End If
        row("CreatedById") = Session("UserDBID")
        row("UserName") = Session("UserName")
        row("DateCreated") = System.DateTime.Now
        row("Dirty") = True
        If bNewRow Then dt.Rows.Add(row)
        Session("EditNotes") = dt
        Session("Notes_Table") = Session("EditNotes")
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Save_Values()
        'Nothing
        If Request("type") <> "" Then
            If Request("subType") = "Status" Then
                If Request("redirect") = "true" Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lblRefresh6','');window.close();", True)
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lblRefresh4','');window.close();", True)
                End If
            Else
                If Request("redirect") = "true" Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lblRefresh6','');window.close();", True)
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lblRefresh5','');window.close();", True)
                End If
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lblRefresh3','');window.close();", True)
        End If
    End Sub
End Class
