Imports System.Reflection
Imports System.Data

Imports clsReservationWizard
Imports System.Web.Script.Serialization

Partial Class wizard_Reservations_Note
    Inherits System.Web.UI.Page

    Private package_base As New Base_Package
    Private wiz As New Wizard

#Region "Event Handlers"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim ph = CType(Me.Master.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        Dim wiz_data = CType(ph.FindControl("LB_WIZ_DATA"), Label)

        If Session("wizData" + Session("wizGuid")) <> Nothing Then
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(Session("wizData" + Session("wizGuid")))
        Else
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(wiz_data.Text)
        End If
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Session("wizGuid") = wiz.GUID_TIMESTAMP

        hfKeyValue.Value = wiz.Reservation.ReservationID

        If IsPostBack = False Then lbRefresh_Click(Nothing, EventArgs.Empty)
    End Sub
    Protected Sub lbRefresh_Click(sender As Object, e As EventArgs) Handles lbRefresh.Click
        Dim n = New clsNotes()
        n.KeyField = "RESERVATIONID"
        n.KeyValue = wiz.Reservation.ReservationID
        If n.Get_Notes_Table().Rows.Count > 0 Then
            Dim dt = n.Get_Notes_Table().Rows.OfType(Of DataRow).OrderByDescending(Function(x) Convert.ToDateTime(x.Field(Of String)("DateCreated"))).CopyToDataTable
            dt.Columns.Remove("CreatedByID")
            dt.Columns.Remove("Dirty")
            gvNotes.DataSource = dt
            gvNotes.DataKeyNames = New String() {"ID"}
            gvNotes.DataBind()
        End If
    End Sub
    Protected Sub btNext_Click(sender As Object, e As EventArgs) Handles btNext.Click
        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 1)
        Navigate(bt)
    End Sub
#End Region
#Region "Subs/Functions"
    Private Sub Navigate(bt As Button)
        Dim gt = Me.Master.GetType()
        Dim mi As MethodInfo = gt.GetMethod("Navigate")
        Dim parameters = mi.GetParameters()

        Dim dir = Convert.ToInt32(bt.Attributes("nav"))
        Dim parametersArray() As Object = New Object() {Request.PhysicalPath.Substring(Request.PhysicalPath.LastIndexOf("\") + 1).ToLower(), dir}
        Dim r = mi.Invoke(Me.Master, parametersArray)
        Response.Redirect(r.ToString())
    End Sub

#End Region



End Class
