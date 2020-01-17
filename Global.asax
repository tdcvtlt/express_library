<%@ Application Language="VB" %>

<script runat="server">
    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
        Dim cookieName As String = FormsAuthentication.FormsCookieName
        Dim authCookie As HttpCookie = Context.Request.Cookies(cookieName)

        If (authCookie Is Nothing) Then
            'There is no authentication cookie.
            Return
        End If

        Dim authTicket As FormsAuthenticationTicket = Nothing

        Try
            authTicket = FormsAuthentication.Decrypt(authCookie.Value)
        Catch ex As Exception
            'Write the exception to the Event Log.
            Return
        End Try

        If (authTicket Is Nothing) Then
            'Cookie failed to decrypt.
            Return
        End If

        'When the ticket was created, the UserData property was assigned a
        'pipe-delimited string of group names.
        Dim groups As String() = authTicket.UserData.Split(New Char() {"|"})

        'Create an Identity.
        Dim id As System.Security.Principal.GenericIdentity = New System.Security.Principal.GenericIdentity(authTicket.Name, "LdapAuthentication")

        'This principal flows throughout the request.
        Dim principal As System.Security.Principal.GenericPrincipal = New System.Security.Principal.GenericPrincipal(id, groups)

        Context.User = principal

    End Sub
    Protected Sub Application_EndRequest(ByVal sender As Object, ByVal e As EventArgs)
        HttpContext.Current.Response.CacheControl = "no-cache"
    End Sub
       

    
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
        Try
            If Application("Logins") = "" Or Application("Logins") Is Nothing Then
            Else
                Dim a() As Integer = Application("Logins")
                If System.Array.Exists(a, Session("UserDBID")) Then
                    RemoveAt(a, System.Array.IndexOf(a, Session("UserDBID")))
                    Application("Logins") = a
                End If
            End If
        Catch ex As Exception

        End Try
        
    End Sub
    
    Sub RemoveAt(Of T)(ByRef arr As T(), ByVal index As Integer)
        Dim uBound = arr.GetUpperBound(0)
        Dim lBound = arr.GetLowerBound(0)
        Dim arrLen = uBound - lBound

        If index < lBound OrElse index > uBound Then
            Throw New ArgumentOutOfRangeException( _
            String.Format("Index must be from {0} to {1}.", lBound, uBound))

        Else
            'create an array 1 element less than the input array
            Dim outArr(arrLen - 1) As T
            'copy the first part of the input array
            Array.Copy(arr, 0, outArr, 0, index)
            'then copy the second part of the input array
            Array.Copy(arr, index + 1, outArr, index, uBound - index)

            arr = outArr
        End If
    End Sub

</script>