Imports Microsoft.VisualBasic
Imports formsauth
Partial Class marketing_storecreditcard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oRes As New clsReservations
            Dim oCC As New clsCreditCard
            oRes.ReservationID = Request("ReservationID")
            oRes.Load()
            gvCCOnFile.DataSource = oCC.Get_Card_OnFile(oRes.ProspectID)
            Dim sKeys(0) As String
            sKeys(0) = "CreditCardID"
            gvCCOnFile.DataKeyNames = sKeys
            gvCCOnFile.DataBind()
            oCC = Nothing
        End If
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub lbCardOnFile_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiView1.ActiveViewIndex = 2
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Unnamed2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub Unnamed4_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Unnamed3_Click1(ByVal sender As Object, ByVal e As System.EventArgs) handles btnSave.click
        If hfTokenValue.Value = "0" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Press the Save Button');", True)
        Else
            Dim bProceed As Boolean = True
            Dim sErr As String = ""
            If (ccnumberTxt.Text = "" Or Len(ccnumberTxt.Text) < 13 Or Len(ccnumberTxt.Text) > 16) Then
                bProceed = False
                sErr = "Enter A Valid Credit Card Number."
            ElseIf (expTxt.Text = "" Or InStr(expTxt.Text, "/") <> 0 Or IsNumeric(expTxt.Text) = False Or Len(expTxt.Text) <> 4) Then
                bProceed = False
                sErr = "Please Enter A Valid Expiration Date."
            ElseIf (nameTxt.Text = "") Then
                bProceed = False
                sErr = "Please Enter The Name Found On The Card."
            ElseIf (postalCodeTxt.Text = "") Then
                bProceed = False
                sErr = "Please Enter A Postal Code."
            End If

            If bProceed Then
                Dim oFin As New clsFinancials
                Dim oRes As New clsReservations
                Dim oPros As New clsProspect
                Dim oCombo As New clsComboItems
                Dim oCC As New clsCreditCard
                Dim oCCTrans As New clsCCTrans
                Dim ccID As Integer = 0
                Dim oCCM As New clsCCMerchantAccount
                Dim prosName As String = ""
                oRes.ReservationID = Request("ReservationID")
                oRes.Load()
                ccID = oFin.Get_CreditCard(ccnumberTxt.Text, expTxt.Text, cvvTxt.Text, "", "", 0, postalCodeTxt.Text, nameTxt.Text, swipeTxt.Text, oRes.ProspectID, hfTokenValue.Value, hfCardType.Value)
                oCCTrans.CCTransID = 0
                oCCTrans.Load()
                oCCTrans.CreditCardID = ccID
                oCCTrans.TransTypeID = oCombo.Lookup_ID("CCTransType", "PreAuth")
                oCCTrans.Imported = 0
                oCCTrans.Approved = 1
                oCCTrans.ApprovedBy = "AutoApprove"
                oCCTrans.DateApproved = System.DateTime.Now
                oCCTrans.ClientIP = Request.ServerVariables("REMOTE_HOST")
                oCCTrans.CreatedByID = Session("UserDBID")
                oCCTrans.DateCreated = System.DateTime.Now
                oCCTrans.Amount = 75
                oCCTrans.AccountID = oCCM.Lookup_By_AcctName("~0013~") ' ddMerchantAccount.SelectedValue
                oCCTrans.Token = hfTokenValue.Value
                oCCTrans.Save()
                hfCCTransID.Value = oCCTrans.CCTransID
                hfCCID.Value = ccID
                oCC.CreditCardID = ccID
                oCC.Load()
                oCC.ReadyToImport = True
                oCC.Save()
                lblStatus.Text = "Validating Card... Please Wait"
                Timer1.Interval = 1000
                Timer1.Enabled = True
                btnSave.Enabled = False
                oRes = Nothing
                oCombo = Nothing
                oFin = Nothing
                oPros = Nothing
                oCCM = Nothing
                oCC = Nothing
                oCCTrans = Nothing
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)
            End If
        End If

    End Sub


    Protected Sub Unnamed5_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblErr.Text = ""
        Dim adPath As String = "LDAP://DC=kcp,DC=local" 'Path to your LDAP directory server
        Dim adAuth As New LdapAuthentication(adPath)
        Try
            If (True = adAuth.IsAuthenticated("KCP", userNameTxt.Text, passwordTxt.Text)) Then
                Dim oPers As New clsPersonnel
                Dim persID As Integer = 0
                persID = oPers.get_PersID(userNameTxt.Text)
                If oPers.chk_Mgr_Status(persID) Then
                    Dim oRes As New clsReservations
                    Dim oPros As New clsProspect
                    Dim oCombo As New clsComboItems
                    Dim prosName As String = ""
                    oRes.ReservationID = Request("ReservationID")
                    oRes.Load()
                    oPros.Prospect_ID = oRes.ProspectID
                    oPros.Load()
                    prosName = Left(oPros.First_Name, 10) & " " & Left(oPros.Last_Name, 10)
                    oRes.UserID = Session("UserDBID")
                    If oRes.Check_In(oRes.ReservationID, prosName) Then
                        oRes.StatusID = oCombo.Lookup_ID("ReservationStatus", "In-House")
                        oRes.Save()
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.navigate('" & Request.ApplicationPath & "/marketing/editReservation.aspx?ReservationID=" & oRes.ReservationID & "');window.close();", True)
                    Else
                        lblErr.Text = "CheckIn Error"
                        MultiView1.ActiveViewIndex = 1
                    End If
                    oRes = Nothing
                    oPros = Nothing
                    oCombo = Nothing
                Else
                    lblErr.Text = "This User Does Not Have Persmission to OverRide. " & oPers.Err
                    MultiView1.ActiveViewIndex = 1
                End If
            Else
                lblErr.Text = "Error Authenticating User. Please Try Again."
                MultiView1.ActiveViewIndex = 1
            End If
        Catch ex As Exception
            lblErr.Text = ex.Message
            MultiView1.ActiveViewIndex = 1
        End Try
    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Timer1.Enabled = False
        Timer1.Interval = 2000
        Dim resp As String = ""
        resp = Check_Status()
        'ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & hfCCTransID.Value & ": " & resp & "');", True)

        If Left(resp, 1) <> "" Then
            Timer1.Enabled = False
            If Left(resp, 1) = "N" Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Credit Card Declined');", True)
                lblStatus.Text = ""
                btnSave.Enabled = True
                hfTokenValue.Value = "0"
                hfCardType.Value = ""
                hfCCTransID.Value = 0
            Else
                Dim oRes As New clsReservations
                Dim oPros As New clsProspect
                Dim oCombo As New clsComboItems
                Dim prosName As String = ""
                oRes.ReservationID = Request("ReservationID")
                oRes.Load()
                oPros.Prospect_ID = oRes.ProspectID
                oPros.Load()
                prosName = Left(oPros.First_Name, 10) & " " & Left(oPros.Last_Name, 10)
                oRes.UserID = Session("UserDBID")
                If oRes.Check_In(oRes.ReservationID, prosName) Then
                    oRes.StatusID = oCombo.Lookup_ID("ReservationStatus", "In-House")
                    oRes.Save()
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.document.location.href = '" & Request.ApplicationPath & "/marketing/editReservation.aspx?ReservationID=" & oRes.ReservationID & "';window.close();", True)
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('CHECKEDINERROR');", True)
                    lblStatus.Text = ""
                    btnSave.Enabled = True
                    hfTokenValue.Value = "0"
                    hfCardType.Value = ""
                    hfCCTransID.Value = 0
                End If
                oRes = Nothing
                oPros = Nothing
                oCombo = Nothing
            End If
        End If





        'If Not (Check_Status()) Or hfTickCounter.Value >= 50 Then
        '    Timer1.Enabled = False
        '    lblWaiting.Text = "Processed"
        '    hfTickCounter.Value = 0
        'End If

        'If hfCCID.Value <> "0" Then
        '    Dim oCC As New clsCreditCard
        '    Dim ccID As Integer = hfCCID.Value
        '    oCC.CreditCardID = ccID
        '    oCC.Load()
        '    If oCC.Token.ToString.Substring(0, 4) = "supt" And oCC.ImportedID = 0 Then
        '        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Credit Card Declined');", True)
        '        lblStatus.Text = ""
        '        btnSave.Enabled = True
        '        hfTokenValue.Value = "0"
        '        hfCardType.Value = ""
        '    ElseIf oCC.ImportedID = 0 Then
        '        Dim oFin As New clsFinancials
        '        Dim oRes As New clsReservations
        '        Dim oPros As New clsProspect
        '        Dim oCombo As New clsComboItems
        '        Dim prosName As String = ""
        '        oRes.ReservationID = Request("ReservationID")
        '        oRes.Load()
        '        If ccID > 0 Then
        '            oPros.Prospect_ID = oRes.ProspectID
        '            oPros.Load()
        '            prosName = Left(oPros.First_Name, 10) & " " & Left(oPros.Last_Name, 10)
        '            oRes.UserID = Session("UserDBID")
        '            If oRes.Check_In(oRes.ReservationID, prosName) Then
        '                oRes.StatusID = oCombo.Lookup_ID("ReservationStatus", "In-House")
        '                oRes.Save()
        '                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.document.location.href = '" & Request.ApplicationPath & "/marketing/editReservation.aspx?ReservationID=" & oRes.ReservationID & "';window.close();", True)
        '            Else
        '                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('CHECKEDINERROR');", True)
        '                lblStatus.Text = ""
        '                btnSave.Enabled = True
        '                hfTokenValue.Value = "0"
        '                hfCardType.Value = ""
        '            End If

        '        Else
        '            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Error');", True)
        '            lblStatus.Text = ""
        '            btnSave.Enabled = True
        '            hfTokenValue.Value = "0"
        '            hfCardType.Value = ""
        '        End If
        '        oRes = Nothing
        '        oPros = Nothing
        '        oFin = Nothing
        '        oCombo = Nothing
        '    Else
        '        Timer1.Enabled = True
        '    End If
        '    oCC = Nothing
        'End If
    End Sub

    Private Function Check_Status() As String
        Dim response As String = ""
        Dim oCCTrans As New clsCCTrans
        oCCTrans.CCTransID = hfCCTransID.Value
        oCCTrans.Load()
        response = oCCTrans.ICVResponse & ""
        oCCTrans = Nothing
        Return response
    End Function

    Protected Sub gvCCOnFile_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvCCOnFile.SelectedIndexChanged
        Dim row As GridViewRow = gvCCOnFile.SelectedRow
        hfCCID.Value = row.Cells(1).Text
        ccnumberTxt.Text = row.Cells(2).Text & ""
        cvvTxt.Text = Replace(row.Cells(4).Text, "&nbsp;", "") & ""
        expTxt.Text = row.Cells(3).Text & ""
        nameTxt.Text = Replace(row.Cells(5).Text, "&nbsp;", "") & ""
        postalCodeTxt.Text = Replace(row.Cells(9).Text, "&nbsp;", "") & ""
        hfTokenValue.Value = row.Cells(10).Text & ""
        If hfTokenValue.Value = "" Then hfTokenValue.Value = "0"
        MultiView1.ActiveViewIndex = 0
    End Sub
End Class
