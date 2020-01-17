
Imports formsauth
Imports System.Data
Imports System.Data.SqlClient

Partial Class Logon
    Inherits System.Web.UI.Page

    Sub Login_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        'Dim adPath As String = "LDAP://192.168.100.40/DC=kcp,DC=local" 'Path to your LDAP directory server
        Dim adPath As String = "LDAP://172.16.55.68/DC=kcp,DC=local" 'Path to your LDAP directory server
        Dim adAuth As New LdapAuthentication(adPath)
        Session("UserDBID") = 8227
        Try
            If (True = adAuth.IsAuthenticated(txtDomain.Text, txtUsername.Text, txtPassword.Text)) Then
                'adAuth = Nothing
                'adAuth = New LdapAuthentication(adPath)
                'adAuth.IsAuthenticated(txtDomain.Text, txtUsername.Text, txtPassword.Text)
                Dim groups As String = "" ' adAuth.GetGroups()

                'Create the ticket, and add the groups.
                Dim isCookiePersistent As Boolean = chkPersist.Checked
                Dim authTicket As FormsAuthenticationTicket = New FormsAuthenticationTicket(1, _
                     txtUsername.Text, DateTime.Now, DateTime.Now.AddMinutes(60), isCookiePersistent, groups)

                'Encrypt the ticket.
                Dim encryptedTicket As String = FormsAuthentication.Encrypt(authTicket)

                'Create a cookie, and then add the encrypted ticket to the cookie as data.
                Dim authCookie As HttpCookie = New HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)

                If (isCookiePersistent = True) Then
                    authCookie.Expires = authTicket.Expiration
                End If
                'Add the cookie to the outgoing cookies collection.
                Response.Cookies.Add(authCookie)
                Dim cn As New SqlConnection(Resources.Resource.cns)
                Dim cm As New SqlCommand("Select * from t_Personnel where Username = '" & txtUsername.Text & "'", cn)
                Dim da As New SqlDataAdapter(cm)
                Dim ds As New DataSet
                Dim dr As DataRow
                Dim sqlCmdBuild As New SqlCommandBuilder(da)
                Try
                    Dim bNew As Boolean = False
                    da.Fill(ds, "Personnel")
                    If ds.Tables("Personnel").Rows.Count > 0 Then
                        dr = ds.Tables("Personnel").Rows(0)
                    Else
                        dr = ds.Tables("Personnel").NewRow
                        dr("UserName") = txtUsername.Text
                        bNew = True
                    End If
                    dr("ADSID") = adAuth.SID
                    dr("Active") = 1
                    dr("LastName") = adAuth.Last_Name
                    dr("FirstName") = adAuth.First_Name

                    If bNew Then ds.Tables("Personnel").Rows.Add(dr)

                    da.Update(ds, "Personnel")
                    ds.Clear()
                    da.Fill(ds, "Personnel")
                    dr = ds.Tables("Personnel").Rows(0)
                    Session("UserDBID") = dr("PersonnelID")
                    
                Catch ex As Exception
                    Session("UserDBID") = 0

                    'Session("FirstName") = ex.ToString
                Finally
                    If cn.State <> ConnectionState.Closed Then cn.Close()
                    cn = Nothing
                    cm = Nothing
                    da = Nothing
                    ds = Nothing
                    dr = Nothing
                    sqlCmdBuild = Nothing
                End Try

                Session("UserID") = adAuth.SID
                Session("Groups") = adAuth.GetGroups
                Session("UserName") = txtUsername.Text
                Session("FirstName") = adAuth.First_Name
                Session("LastName") = adAuth.Last_Name
                Session("LDAP") = adAuth.Distinguished_Name
                If InStr(Session("Groups"), "Outside Vendors") > 0 And Not (Session("Groups").ToString.Contains("MIS Dept")) Then
                    Dim oVendor As New clsVendor2Personnel
                    Session("Vendors") = oVendor.Get_Vendors(Session("UserDBID"))
                Else
                    Session("Vendors") = "0"
                End If
                Dim bCont As Boolean = True
                Try
                    'If Application("Logins").ToString & "" = "" Or Application("Logins") Is Nothing Then
                    '    Dim a(0) As Integer
                    '    a(0) = Session("UserDBID")
                    '    Application.Add("Logins", a)
                    'Else
                    '    Dim a() As Integer = Application("Logins")
                    '    If Array.IndexOf(a, Session("UserDBID")) > -1 Then
                    '    Else
                    '        ReDim Preserve a(UBound(a) + 1)
                    '        a(UBound(a)) = Session("UserDBID")
                    '        Application("Logins") = a
                    '    End If
                    'End If
                Catch ex As Exception
                    Response.Write(ex.ToString)
                    bCont = False
                End Try

                'You can redirect now.
                'If Request("URL") <> "" Then
                'Response.Redirect(Request("URL"))
                ' Else
                If bCont Then Response.Redirect(FormsAuthentication.GetRedirectUrl(txtUsername.Text, False))
                'End If

            Else
                errorLabel.Text = "Authentication did not succeed. Check user name and password."
            End If

        Catch ex As Exception
            errorLabel.Text = "Error authenticating. " & ex.Message
        End Try
        adAuth = Nothing
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("UserID") = ""
        Session("Groups") = ""
        Session("FirstName") = ""
        Session("LastName") = ""
        Session("UserName") = ""
        If Not (IsPostBack) Then Session.Abandon()
        'If Left(Request.ServerVariables("REMOTE_ADDR"), 3) = "172" Or Left(Request.ServerVariables("REMOTE_ADDR"), 3) = "192" Then
        '    If Request.ServerVariables("server_port") = "443" Then
        '        Response.Redirect("http://crms.kingscreekplantation.com" & Request.ServerVariables("URL"))
        '    End If
        'End If

        txtUsername.Focus()
    End Sub
End Class
