
Partial Class general_MoveDoc
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oUpload As New clsUploadedDocs
            oUpload.FileID = Request("ID")
            oUpload.Load()
            Select Case UCase(oUpload.KeyField)
                Case "CONTRACTID"
                    Label1.Text = "Move to Contract Number:"
                Case "RESERVATIONID"
                    Label1.Text = "Move to ReservationID:"
                Case "PERSONNELID"
                    Label1.Text = "Move to PersonnelID:"
                Case "TOURID"
                    Label1.Text = "Move to TourID:"
                Case Else
                    Label1.Text = "N/A"
            End Select
            oUpload = Nothing
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim sAns As String = ""
        Dim keyValue As Integer = 0
        Dim oUpload As New clsUploadedDocs
        oUpload.FileID = Request("ID")
        oUpload.Load()

        Select Case UCase(oUpload.KeyField)
            Case "CONTRACTID"
                Dim oCon As New clsContract
                If Not (oCon.Verify_Contract(txtMoveTo.Text)) Then
                    sAns = "Please Enter a Valid Contract Number."
                End If
                keyValue = oCon.Get_Contract_ID(txtMoveTo.Text)
                oCon = Nothing
            Case "RESERVATIONID"
                Dim oRes As New clsReservations
                If Not (oRes.val_ResID(txtMoveTo.Text)) Then
                    sAns = "Please Enter a Valid ReservationID."
                End If
                oRes = Nothing
                keyValue = txtMoveTo.Text
            Case "PERSONNELID"
                Dim oPers As New clsPersonnel
                If Not (oPers.Validate_PersID(txtMoveTo.Text)) Then
                    sAns = "Please Enter a Valid PersonnelID."
                End If
                keyValue = txtMoveTo.Text
                oPers = Nothing
            Case "TOURID"
                Dim oPers As New clsTour
                oPers.TourID = txtMoveTo.Text
                oPers.Load()
                If oPers.ProspectID = 0 Then
                    sAns = "Please Enter a Valid TourID."
                End If
                keyValue = txtMoveTo.Text
                oPers = Nothing
            Case Else
                sAns = "ERROR"
        End Select
        If sAns = "" Then
            oUpload.UserID = Session("UserDBID")
            oUpload.KeyValue = keyValue
            oUpload.Save()
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Docs();window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sAns & "');", True)
        End If
        oUpload = Nothing
    End Sub
End Class
