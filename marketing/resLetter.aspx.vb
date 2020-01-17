
Partial Class marketing_resLetter
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim ores As New clsReservations
            litBody.Text = ores.Conf_Letter_Body(Request("ReservationID"))
            If Request("function") = "Email" Then
                If litBody.Text <> "ERROR" And litBody.Text <> "EMPTY" Then
                    Dim oEmail As New clsEmail
                    Dim email As String = ""
                    ores.ReservationID = Request("ReservationID")
                    ores.Load()
                    email = oEmail.Get_Primary_Email(ores.ProspectID)
                    If email <> "" Then
                        Dim oNote As New clsNotes
                        oNote.NoteID = 0
                        oNote.Load()
                        oNote.UserID = Session("UserDBID")
                        oNote.KeyField = "ProspectID"
                        oNote.KeyValue = ores.ProspectID
                        oNote.Note = "Emailed confirmation letter to prospect. ReservationID: " & ores.ReservationID
                        oNote.DateCreated = System.DateTime.Now
                        oNote.Save()
                        Dim conID As Integer = 0
                        conID = ores.Get_Res_Contract(ores.ReservationID)
                        If conID > 0 Then
                            oNote.NoteID = 0
                            oNote.Load()
                            oNote.UserID = Session("USERDBID")
                            oNote.KeyField = "ContractID"
                            oNote.KeyValue = conID
                            oNote.Note = "Emailed confirmation letter to prospect. ReservationID: " & ores.ReservationID
                            oNote.DateCreated = System.DateTime.Now
                            oNote.Save()
                        End If
                        oNote = Nothing
                        Send_Mail(email, "OwnerServices@kingscreekplantation.com", "Your Reservation Confirmation Letter", litBody.Text, True)

                         '
                        'added April 4, 2017 work order 47480
                        Dim oUpload As New clsUploadedDocs
                        oUpload.FileID = 0
                        oUpload.Load()
                        oUpload.KeyField = "ReservationID"
                        oUpload.KeyValue = ores.ReservationID
                        oUpload.Name = "Confirmation Letter Sent " & System.DateTime.Now.ToShortDateString
                        oUpload.DateUploaded = System.DateTime.Now
                        oUpload.ContentText = litBody.Text
                        oUpload.UploadedByID = Session("UserDBID")
                        oUpload.Save()
                        oUpload = Nothing

                    Else
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('No Active Email Address On File.');", True)
                    End If
                    oEmail = Nothing
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Print", "window.print();", True)
                End If
            Else
                If litBody.Text = "EMPTY" Then
                    litBody.Text = "Insufficient Information to Print Letter."
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Print", "window.print();", True)
                End If
            End If
            ores = Nothing
        End If
    End Sub
End Class
