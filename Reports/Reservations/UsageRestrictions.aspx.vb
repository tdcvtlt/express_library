
Partial Class Reports_Reservations_UsageRestrictions
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim cn As Object
        Dim rs As Object
        Dim sAns As String = ""
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0
        rs.open("Select * from t_Prospect p inner join t_Contract c on c.prospectid = p.prospectid inner join t_Comboitems cs on cs.comboitemid = c.statusid where c.contractid in (select contractid from t_UsageRestriction2Contract where usagerestrictionid = '" & Me.ddRestrictions.SelectedValue & "')", cn, 0, 1)
        If rs.eof And rs.bof Then
            sAns = "No Records"
        Else
            sAns = "<table>"
            sAns = sAns & "<tr><th>First Name</th><th>Last Name</th><th>Contract Number</th><th>Status</th></tr>"
            Do While Not rs.eof
                sAns = sAns & "<tr><td>" & rs.fields("Firstname").value & "</td><td>" & rs.fields("Lastname").value & "</td><td>" & rs.fields("ContractNumber").value & "</td><td>" & rs.fields("Comboitem").value & "</td></tr>"
                rs.movenext()
            Loop
            sAns = sAns & "</table>"
        End If
        rs.close()
        cn.close()
        rs = Nothing
        cn = Nothing
        litReport.Text = sAns
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oUsageRes As New clsUsageRestriction
            ddRestrictions.DataSource = oUsageRes.Get_Restrictions()
            ddRestrictions.DataTextField = "Name"
            ddRestrictions.DataValueField = "UsageRestrictionID"
            ddRestrictions.DataBind()
            oUsageRes = Nothing
        End If
    End Sub
End Class
