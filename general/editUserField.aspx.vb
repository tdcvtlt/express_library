
Partial Class general_editUserField
    Inherits System.Web.UI.Page
    Dim _KeyValue As Integer = 0
    Dim _TableName As String = ""
    Dim _DataType As Integer = 0
    Dim _Value As String = ""
    Dim _ID As Integer = 0
    Dim _UFID As Integer = 0
    Dim _View As Integer = 0
    Dim oUF As New clsUserFields

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _UFID = IIf(IsNumeric(Request("UFID")), CInt(Request("UFID")), 0)
        _ID = IIf(IsNumeric(Request("ID")), CInt(Request("ID")), 0)
        _TableName = IIf(Request("KeyField") <> "", Request("KeyField"), Request("UFTable"))
        _KeyValue = IIf(IsNumeric(Request("KeyValue")), CInt(Request("KeyValue")), 0)
        If _UFID = 0 Then
            _View = 2 'New Field
        ElseIf _ID = 0 Then
            _View = 1 'New Entry
        Else
            _View = 0 'Edit Existing
        End If
        If Not (IsPostBack) Then

            If _View = 2 Then 'New Field
                Load_Lookup()
            ElseIf _View = 1 Then 'New Entry
                Fill_Object()
            ElseIf _View = 0 Then 'Edit Existing
                Fill_Object()
            End If
            Flip_Views()
        Else
            If _View = 0 Or _View = 1 Then
                Fill_Object(False)
            End If
        End If

    End Sub

    Protected Sub Flip_Views()
        MultiView1.ActiveViewIndex = IIf(_View < 2, 0, _View)

    End Sub

    Protected Sub Set_Fields()

        lblUFName.Text = oUF.FieldName
        Select Case oUF.DataType
            Case "CheckBox"
                ckUFValue.Visible = True
                txtUFValue.Visible = False
                dteUFValue.Visible = False
                If oUF.UFValue = "1" Or LCase(oUF.UFValue) = "true" Then
                    ckUFValue.Checked = True
                End If
                '                ckUFValue.Checked = IIf(IsNumeric(oUF.UFValue), CBool(oUF.UFValue), IIf(LCase(oUF.UFValue) = "true", True, False))
            Case "Date"
                ckUFValue.Visible = False
                txtUFValue.Visible = False
                dteUFValue.Visible = True
                If (oUF.UFValue <> "") Then dteUFValue.Selected_Date = oUF.UFValue
            Case "Text"
                ckUFValue.Visible = False
                txtUFValue.Visible = True
                dteUFValue.Visible = False
                txtUFValue.Text = oUF.UFValue
            Case Else
                lblError.Text = "Unable to find the userfield: ID=" & _UFID
        End Select
        lblError.Text = "Datatype:" & oUF.DataType
    End Sub

    Protected Sub Load_Lookup()
        siDataType.Connection_String = Resources.Resource.cns
        siDataType.ComboItem = "UserFieldType"
        siDataType.Label_Caption = "Type: "
        siDataType.Load_Items()
    End Sub

    Protected Sub Fill_Object(Optional ByVal bSet As Boolean = True)
        Select Case _View
            Case 0 'Edit
                oUF.ID = _ID
                oUF.Load()
                If bSet Then Set_Fields()
                lblError.Text = oUF.Error_Message
            Case 1 'New Entry
                oUF.ID = 0
                oUF.UFID = _UFID
                oUF.TableName = _TableName
                oUF.Load()
                If bSet Then Set_Fields()
                lblError.Text = oUF.Error_Message
                'If lblError.Text = "" Then lblError.Text = oUF.TableName & "=Tablename:" & _TableName & "<br />UFID=" & _UFID
            Case 2 'New Field

        End Select
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Select Case _View
            Case 0, 1 'Edit, New Entry
                oUF.ID = _ID
                oUF.UFID = _UFID
                oUF.KeyValue = _KeyValue
                Select Case oUF.DataType
                    Case "CheckBox"
                        oUF.UFValue = ckUFValue.Checked
                    Case "Date"
                        oUF.UFValue = dteUFValue.Selected_Date
                    Case "Text"
                        oUF.UFValue = txtUFValue.Text
                    Case Else

                End Select
                oUF.TableName = _TableName
                oUF.UserID = Session("UserDBID")
                oUF.Save()
                'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Notify", "alert('Should have saved: Edit, New Entry: " & oUF.DataType & "');", True)
            Case 2 'New Field
                If siDataType.Selected_ID = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Notify", "alert('Please select a Data Type.');", True)
                    Exit Sub
                End If
                oUF.UserID = Session("UserDBID")
                oUF.UFID = _UFID
                oUF.TableName = _TableName
                oUF.DataTypeID = siDataType.Selected_ID
                oUF.FieldName = txtUFName.Text
                oUF.Save_New_Field()
                'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Notify", "alert('Should have saved: New Field');", True)
            Case Else
                'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Notify", "alert('Went to Else');", True)
        End Select
        If oUF.Error_Message = "" Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
        Else
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Alert", "alert('An Error Occured.');", True)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Error_Source", "var sTemp = '" & oUF.Error_Message & "';", True)
        End If

    End Sub

End Class
