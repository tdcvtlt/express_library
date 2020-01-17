Imports System.Data.SqlClient

Partial Class setup_CombineProspects
    Inherits System.Web.UI.Page

    Protected Sub btnCombine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCombine.Click
        If CheckSecurity("Prospects", "Merge", , , Session("UserDBID")) Then
            If txtKeep.Text <> "" And lstRemove.Items.Count > 0 Then
                If Verify_ID(txtKeep.Text) Then
                    Dim liItem As New ListItem(txtKeep.Text)
                    If Not (lstRemove.Items.Contains(liItem)) Then
                        Combine()
                    Else
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "Error", "alert('Cannot remove and keep the same ProspectID');", True)
                    End If
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Error", "alert('Unable to find ProspectID: " & txtKeep.Text & "');", True)
                End If
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Error", "alert('Please provide both the ProspectID to keep and ProspectID(s) to remove.');", True)
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Error", "alert('Access Denied');", True)
        End If
    End Sub

    Private Sub Combine()
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("exec sp_MergeProspects('')", cn)
        Try
            cn.Open()
            Dim sRemove As String = "'"
            For i = 0 To lstRemove.Items.Count - 1
                sRemove &= IIf(sRemove = "'", lstRemove.Items(i).Text, "," & lstRemove.Items(i).Text)
            Next
            sRemove &= "'"
            Dim sql As String = "exec sp_MergeProspects " & txtKeep.Text & "," & sRemove & ""
            cm.CommandText = sql
            cm.ExecuteNonQuery()
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Error", "alert('Combined  Prospect(s)');", True)
            Reset()
            cn.Close()
        Catch ex As Exception
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Error", "alert('" & ex.ToString.Replace("'", "\'") & "');", True)
        Finally
            If cn.State <> System.Data.ConnectionState.Closed Then cn.Close()
            cm = Nothing
            cn = Nothing
        End Try
    End Sub

    Private Sub Reset()
        txtKeep.Text = ""
        lstRemove.Items.Clear()
        txtRemove.Text = ""
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Reset()
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtRemove.Text <> "" Then
            If IsNumeric(txtRemove.Text) Then
                If Verify_ID(txtRemove.Text) Then
                    lstRemove.Items.Add(txtRemove.Text)
                    txtRemove.Text = ""
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "ProsErr", "alert('Unable to find ProspectID: " & txtRemove.Text & "');", True)
                End If
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "ProsErr", "alert('Please enter numeric ProspectIDs only.');", True)
            End If
        End If
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        If lstRemove.SelectedIndex > -1 Then
            lstRemove.Items.Remove(lstRemove.SelectedItem)
        End If
    End Sub

    Private Function Verify_ID(ByVal id As String) As Boolean
        Dim oPros As New clsProspect
        Dim bAns As Boolean = False
        oPros.Prospect_ID = txtKeep.Text
        oPros.Load()
        If Not (oPros.Last_Name = "") Then
            bAns = True
        End If
        oPros = Nothing
        Return bAns
    End Function
End Class
