
Partial Class wizards_addOPCOSProspect
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sErr As String = ""
        If txtFName.Text = "" Then
            sErr = sErr & "Please Enter as First Name. \n"
        End If
        If txtLName.Text = "" Then
            sErr = sErr & "Please Enter a Last Name. \n"
        End If
        If txtHPhone.Text = "" Then
            sErr = sErr & "Please Enter a Home Phone. \n"
        End If
        If txtAddress.Text = "" Then
            sErr = sErr & "Please Enter An Address. \n"
        End If
        If txtCity.Text = "" Then
            sErr = sErr & "Please Enter a City. \n"
        End If
        If txtPostalCode.Text = "" Then
            sErr = sErr & "Please Enter a Postal Code. \n"
        End If

        If sErr = "" Then
            Dim oPros As New clsProspect
            Dim oProsAdd As New clsAddress
            Dim oProsPhone As New clsPhone
            Dim prosID As Integer = 0
            Dim oCombo As New clsComboItems
            oPros.UserID = Session("UserDBID")
            oPros.First_Name = txtFName.Text
            oPros.Last_Name = txtLName.Text
            oPros.Save()
            prosID = oPros.Prospect_ID
            oProsAdd.AddressID = 0
            oProsAdd.UserID = Session("UserDBID")
            oProsAdd.Load()
            oProsAdd.ProspectID = prosID
            oProsAdd.Address1 = txtAddress.Text
            oProsAdd.City = txtCity.Text
            oProsAdd.StateID = siState.Selected_ID
            oProsAdd.PostalCode = txtPostalCode.Text
            oProsAdd.TypeID = oCombo.Lookup_ID("AddressType", "Familiar")
            oProsAdd.ActiveFlag = True
            oProsAdd.Save()
            oProsPhone.PhoneID = 0
            oProsPhone.UserID = Session("UserDBID")
            oProsPhone.Load()
            oProsPhone.TypeID = oCombo.Lookup_ID("Phone", "HOME")
            oProsPhone.Number = txtHPhone.Text
            oProsPhone.ProspectID = prosID
            oProsPhone.Save()
            oPros = Nothing
            oProsAdd = Nothing
            oProsPhone = Nothing
            oCombo = Nothing
            Response.Redirect("addOPCOSTour.aspx?ProspectID=" & prosID & "&ReservationID=0")
            '*********************NEED TO DO EMAIL TO WC IF ITS AN OWNER*****************************
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            siState.Connection_String = Resources.Resource.cns
            siState.Label_Caption = ""
            siState.ComboItem = "State"
            siState.Load_Items()
        End If
    End Sub
End Class
