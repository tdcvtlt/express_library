
Partial Class setup_Kiosks_editKiosk
    Inherits System.Web.UI.Page

    Dim oKiosk As New clsLGKiosk

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If CheckSecurity("Kiosks", "View", , , Session("UserDBID")) Then

                '*** Create view events *** '
                If IsNumeric(Request("KioskID")) Then
                    If CInt(Request("KioskID")) > 0 Then
                        Dim oE As New clsEvents
                        Dim sErr As String = ""

                        If Not (oE.Find_View_Event("KioskID", Request("KioskID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                            If sErr <> "" Then lblKioskError.Text = sErr
                            sErr = ""
                            If Not (oE.Create_View_Event("KioskID", Request("KioskID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                                If sErr <> "" Then lblKioskError.Text &= "<br />" & sErr
                            End If
                        End If
                        oE = Nothing
                    End If
                End If

                '*** End View Events *** '

                MultiView1.ActiveViewIndex = 0

                Load_Lookups()
                oKiosk.KioskID = IIf(IsNumeric(Request("kioskid")), Request("kioskid"), 0)
                oKiosk.Load()

                Set_Fields()
            Else
                MultiView1.ActiveViewIndex = 6
                txtKioskID.Text = -1
            End If
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub


    Private Sub Set_Fields()
        txtKioskID.Text = oKiosk.KioskID
        txtName.Text = oKiosk.Name
        txtLicense.Text = oKiosk.License
        ckActive.Checked = oKiosk.Active
        dteExpiration.Selected_Date = oKiosk.Expiration
        siLocation.Selected_ID = oKiosk.LocationID
        txtKey.Text = oKiosk.Key
    End Sub

    Private Sub Load_Lookups()
        Dim sErr As String = ""
        siLocation.Connection_String = Resources.Resource.cns
        siLocation.Label_Caption = ""
        siLocation.ComboItem = "KioskLocation"
        siLocation.Load_Items()

    End Sub

    Protected Sub Events_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events_Link.Click
        MultiView1.ActiveViewIndex = 3
        ucEvents.KeyField = "KioskID"
        ucEvents.KeyValue = IIf(Request("kioskid") = "", 0, Request("kioskid"))
        ucEvents.List()

    End Sub

    Protected Sub UserFields_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UserFields_Link.Click
        MultiView1.ActiveViewIndex = 5
        UF.KeyField = "KioskID"
        UF.KeyValue = IIf(Request("kioskid") = "", 0, Request("kioskid"))
        UF.Load_List()
    End Sub

    Protected Sub Notes_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Notes_Link.Click
        MultiView1.ActiveViewIndex = 4
        ucNotes.KeyField = "KioskID"
        ucNotes.KeyValue = IIf(Request("kioskid") = "", 0, Request("kioskid"))
        ucNotes.Display()
    End Sub

    Protected Sub Kiosk_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Kiosk_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub btnSave3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave3.Click
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        If txtKioskID.Text > 0 Then
            If CheckSecurity("Kiosks", "Edit", , , Session("UserDBID")) Then

            Else
                bProceed = False
                sErr = "You Do Not Have Persmission to Edit A Kiosk."
            End If
        Else
            If Not (CheckSecurity("Kiosks", "Create", , , Session("UserDBID"))) Then
                bProceed = False
                sErr = "You Do Not Have Persmission to Create A Kiosk."
            End If
        End If
        If bProceed Then
            With oKiosk
                .KioskID = txtKioskID.Text
                .UserID = Session("UserDBID")
                .Load()
                .Expiration = dteExpiration.Selected_Date
                .Active = ckActive.Checked
                .LocationID = siLocation.Selected_ID
                .Name = txtName.Text
                If CheckSecurity("Kiosks", "License", , , Session("UserDBID")) Then .License = txtLicense.Text

                If .KioskID = 0 Then bProceed = False
                .Save()
                If Not bProceed Then
                    Dim oE As New clsEvents
                    oE.Create_Create_Event("KioskID", Request("KioskID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)
                    oE = Nothing
                End If
                txtKioskID.Text = .KioskID
                lblKioskError.Text = .Error_Message
            End With
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub Configuration_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Configuration_Link.Click
        MultiView1.ActiveViewIndex = 1
    End Sub
End Class
