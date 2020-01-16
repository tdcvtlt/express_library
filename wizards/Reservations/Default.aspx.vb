Imports System.Data
Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Web.Script.Serialization
Imports clsReservationWizard

Partial Class wizard_Reservations_Default
    Inherits System.Web.UI.Page

    Private wiz As New Wizard

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

#Region "Event Handlers"
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Session("Search" + wiz.GUID_TIMESTAMP) = Nothing
        Session("ListReservations" + wiz.GUID_TIMESTAMP) = Nothing
        Session("List" + wiz.GUID_TIMESTAMP) = Nothing

        Dim ph = CType(Me.Master.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        Dim wiz_data = CType(ph.FindControl("LB_WIZ_DATA"), Label)

        If Session("wizData" + Session("wizGuid")) <> Nothing Then
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(Session("wizData" + Session("wizGuid")))
        ElseIf wiz_data.Text.Length = 0 Then
            wiz = New Wizard()
            wiz.GUID_TIMESTAMP = DateTime.Now.Ticks.ToString()
            wiz.USER_ID = Session("UserDBID")
        Else
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(wiz_data.Text)
        End If

        If Session("ActiveVendorID") Is Nothing Then
            'rblOptions.SelectedIndex = 0
            'rblOptions.Items.RemoveAt(3)
            'rblOptions.DataBind()
        End If
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Session("wizGuid") = wiz.GUID_TIMESTAMP



    End Sub

    Protected Sub btNext_Click(sender As Object, e As System.EventArgs) Handles btNext.Click

        Dim ph = CType(Me.Master.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        Dim wiz_guid = CType(ph.FindControl("LB_WIZ_GUID"), Label)
        Dim enum_scenario As EnumScenario

        If rblOptions.SelectedValue = "1" Then
            enum_scenario = EnumScenario.One

        ElseIf rblOptions.SelectedValue = "2" Then
            enum_scenario = EnumScenario.Two

        ElseIf rblOptions.SelectedValue = "3" Then
            enum_scenario = EnumScenario.Three

        ElseIf rblOptions.SelectedValue = "4" Then
            enum_scenario = EnumScenario.Four

        ElseIf rblOptions.SelectedValue = 10 Then
            enum_scenario = EnumScenario.Ten
        End If

        If wiz.Scenario <> enum_scenario Then
            wiz = New Wizard()
            wiz.GUID_TIMESTAMP = DateTime.Now.Ticks.ToString()
            wiz.USER_ID = Session("UserDBID")
            Session("wizGuid") = wiz.GUID_TIMESTAMP
        End If
        wiz.Scenario = enum_scenario
        Session("wizData" + Session("wizGuid")) = New JavaScriptSerializer().Serialize(wiz)

        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 1)
        Navigate(CType(sender, Button))
    End Sub
#End Region

End Class
