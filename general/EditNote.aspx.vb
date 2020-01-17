
Partial Class general_EditNote
    Inherits System.Web.UI.Page
    Dim oNote As New clsNotes

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Load_Note()
            If Request("NoteID") > 0 And Not (oNote.CreatedByID = CInt(Session("UserDBID")) And Math.Abs(DateDiff(DateInterval.Hour, Date.Now, CDate(IIf(IsDate(oNote.DateCreated), oNote.DateCreated, Date.Now)))) < 24) Then
                txtNote.ReadOnly = True
                btnSave.Enabled = False
            End If
                ' Response.Write(oNote.CreatedByID & ":" & Session("UserDBID") & "<br />" & Math.Abs(DateDiff(DateInterval.Hour, Date.Now, CDate(oNote.DateCreated))))
            End If
    End Sub

    Private Sub Load_Note()
        oNote.NoteID = Request("NoteID")
        oNote.Load()
        txtNote.Text = oNote.Note
        txtUser.Text = oNote.CreatedBy
        txtDate.Text = oNote.DateCreated
        Label1.Text = oNote.Error_Message
        ddNoteTemplate.DataSource = (New clsNoteTemplates).List(Session("UserDBID"), True, True)
        ddNoteTemplate.DataValueField = "NoteTemplateID"
        ddNoteTemplate.DataTextField = "Title"
        ddNoteTemplate.DataBind()
        ddNoteTemplate.SelectedValue = 0
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim kf_new = String.Empty

        If Request("KeyField").IndexOf(",") > -1 Then
            kf_new = Request("KeyField").ToLower().Substring(0, Request("KeyField").IndexOf(","))
        Else
            kf_new = Request("KeyField")
        End If



        oNote.NoteID = Request("NoteID")
        oNote.Note = txtNote.Text
        oNote.CreatedByID = Session("UserDBID")
        'oNote.KeyField = Request("KeyField")

        oNote.KeyField = kf_new

        oNote.KeyValue = Request("KeyValue")
        oNote.DateCreated = Date.Now
        oNote.UserID = Session("UserDBID")
        oNote.Save()
        If kf_new = "ContractID" Then
            If cbOwner.Checked Then
                Dim oContract As New clsContract
                oContract.ContractID = Request("KeyValue")
                oContract.Load()
                oNote.NoteID = 0
                oNote.Load()
                oNote.Note = txtNote.Text
                oNote.KeyField = "ProspectID"
                oNote.KeyValue = oContract.ProspectID
                oNote.DateCreated = Date.Now
                oNote.UserID = Session("UserDBID")
                oNote.Save()
                oContract = Nothing
            End If
        End If
        If kf_new.ToUpper = "RESERVATIONID" Then
            If cbTour.Checked Then
                Dim oReservation As New clsReservations
                Dim oTour As New clsTour
                Dim tID As Integer = 0
                tID = oTour.Get_Tour_By_Res(Request("KeyValue"))
                If tID > 0 Then
                    oNote.NoteID = 0
                    oNote.Load()
                    oNote.Note = txtNote.Text
                    oNote.KeyField = "TourID"
                    oNote.KeyValue = tID
                    oNote.DateCreated = Date.Now
                    oNote.UserID = Session("UserDBID")
                    oNote.Save()
                Else
                    oReservation.ReservationID = Request("KeyValue")
                    oReservation.Load()
                    If oReservation.TourID > 0 Then
                        oNote.NoteID = 0
                        oNote.Load()
                        oNote.Note = txtNote.Text
                        oNote.KeyField = "TourID"
                        oNote.KeyValue = oReservation.TourID
                        oNote.DateCreated = Date.Now
                        oNote.UserID = Session("UserDBID")
                        oNote.Save()
                    End If
                End If
                oReservation = Nothing
                oTour = Nothing
            End If
        End If
        Label1.Text = oNote.Error_Message
        If oNote.Error_Message = "" Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
        Else
            Label1.Text = oNote.Error_Message
        End If
        oNote = Nothing
    End Sub
    Protected Sub ddNoteTemplate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddNoteTemplate.SelectedIndexChanged
        If Not (IsNothing(ddNoteTemplate.SelectedValue)) Then
            If ddNoteTemplate.SelectedValue > 0 Then
                Dim oNT As New clsNoteTemplates
                oNT.NoteTemplateID = ddNoteTemplate.SelectedValue
                oNT.Load()
                txtNote.Text = oNT.Note
                oNT = Nothing
            End If
        End If
    End Sub
End Class
