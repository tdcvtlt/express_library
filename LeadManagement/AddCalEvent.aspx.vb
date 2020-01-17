
Partial Class LeadManagement_AddCalEvent
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Select Case Request("ID")
                Case "0"
                    MultiView1.ActiveViewIndex = 0
                    Setup_View1()
                Case Else
                    MultiView1.ActiveViewIndex = 1
            End Select
            Setup()
        End If
    End Sub

    Private Sub Setup_View1()
        Dim oPersonnel As New clsPersonnel
        ddWho.DataSource = oPersonnel.List_Reps_For_Department("Reservations")
        ddWho.DataValueField = "PersonnelID"
        ddWho.DataTextField = "Name"
        ddWho.DataBind()
        oPersonnel = Nothing
    End Sub

    Private Sub Setup()
        siEventType.Connection_String = Resources.Resource.cns
        siEventType.ComboItem = "EventType"
        siEventType.Label_Caption = ""
        siEventType.Load_Items()
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Close()
    End Sub

    Private Sub Close()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim oGC As New clsGlobalCalendar
        oGC.CalendarEntryID = Request("ID")
        oGC.Load()
        oGC.PersonnelID = ddWho.SelectedValue
        oGC.Scheduled = True
        oGC.EventDate = dfEventDate.Selected_Date
        oGC.Event_Description = txtDesc.Text
        oGC.EventTypeID = siEventType.Selected_ID
        oGC.Save()
        oGC = Nothing
        Close()
    End Sub
End Class
