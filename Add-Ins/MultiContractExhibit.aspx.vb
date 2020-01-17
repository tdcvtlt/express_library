
Partial Class Add_Ins_MultiContractExhibit
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oCon As New clsContract
        If txtContract.Text = "" Or Not (oCon.Verify_Contract(txtContract.Text)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Valid Contract');", True)
        Else
            Dim conID As Integer
            Dim bProceed As Boolean = True
            conID = oCon.Get_Contract_ID(txtContract.Text)
            For i = 0 To lbContract.Items.Count - 1
                If lbContract.Items(i).Value = conID Then
                    bProceed = False
                    Exit For
                End If
            Next
            If bProceed Then
                lbContract.Items.Add(New ListItem(txtContract.Text, oCon.Get_Contract_ID(txtContract.Text)))
                txtContract.Text = ""
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('This Contract Has Already Been Added.');", True)
            End If
        End If
        oCon = Nothing

    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        If lbContract.SelectedValue <> "" Then
            lbContract.Items.Remove(lbContract.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        If lbContract.Items.Count = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Valid Contract');", True)
        ElseIf txtExhibit.Text = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter an Exhibit Number');", True)
        Else
            Dim oUserField As New clsUserFields
            For i = 0 To lbContract.Items.Count - 1
                oUserField.ID = 0
                oUserField.Load()
                oUserField.KeyValue = lbContract.items(i).Value
                oUserField.UFValue = txtExhibit.Text
                oUserField.UFID = oUserField.Get_UserFieldID(oUserField.Get_GroupID("Contract"), "Exhibit Number")
                oUserField.Save()
            Next
            oUserField = Nothing
            txtExhibit.Text = ""
            lbContract.Items.Clear()
            txtContract.Text = ""
        End If
    End Sub
End Class
