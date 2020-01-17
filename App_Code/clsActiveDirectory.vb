Imports Microsoft.VisualBasic
Imports System.DirectoryServices
Imports System.Data.SqlClient
Imports System.Security.Principal
Imports System.Threading
Imports System.Globalization
Imports System
Imports System.Collections


Public Class clsActiveDirectory
    Dim _path As String
    Dim _pwd As String

    Public Function List_Users(ByVal domain As String) As String ' SqlDataReader

        Dim entry As DirectoryEntry = New DirectoryEntry(_path, domain, _pwd)
        Dim dread As String = ""
        Try
            'Bind to the native AdsObject to force authentication.			
            Dim obj As Object = entry.NativeObject
            Dim search As DirectorySearcher = New DirectorySearcher(entry)

            'search.Filter = "(SAMAccountName=" & username & ")"
            search.PropertiesToLoad.Add("cn")
            search.PropertiesToLoad.Add("objectSid")
            search.PropertiesToLoad.Add("givenname")
            search.PropertiesToLoad.Add("sn")

            Dim result As SearchResult = search.FindOne()

            If (result Is Nothing) Then
                'Return False
            End If

            'Update the new path to the user in the directory.
            _path = result.Path
            '_filterAttribute = CType(result.Properties("cn")(0), String)
            '_sid = ConvertSIDByteToString(result.Properties("objectSid")(0))
            '_fname = CType(result.Properties("givenname")(0), String)
            If result.Properties("sn").Count > 0 Then
                ' _lname = CType(result.Properties("sn")(0), String)
            Else
                '_lname = ""
            End If

        Catch ex As Exception
            Throw New Exception("Error authenticating user. " & ex.Message)

        End Try
        Return dread
        'Return True
    End Function


    ''' <summary>
    ''' Method used to create an entry to the AD.
    ''' Replace the path, username, and password with the ones specific to you 
    '''and your network. 
    '''Hardcoding a username & password can be viewed as a security risk
    '''    so you may want to store this in an encrypted file.
    ''' </summary>
    ''' <returns> A DirectoryEntry </returns>
    Public Shared Function GetDirectoryEntry() As DirectoryEntry
        Dim dirEntry As DirectoryEntry = New DirectoryEntry()
        dirEntry.Path = "LDAP://192.168.1.1/CN=Users;DC=Yourdomain"
        dirEntry.Username = "yourdomain\sampleuser"
        dirEntry.Password = "samplepassword"
        Return dirEntry
    End Function

    ''' <summary>
    ''' Helper method that sets properties for AD users.
    ''' </summary>
    ''' <param name="de">DirectoryEntry to use</param>
    ''' <param name="pName">Property name to set</param>
    ''' <param name="pValue">Value of property to set</param>
    Public Shared Sub SetADProperty(ByVal de As DirectoryEntry, _
    ByVal pName As String, ByVal pValue As String)
        'First make sure the property value isnt "nothing"
        If Not pValue Is Nothing Then
            'Check to see if the DirectoryEntry contains this property already
            If de.Properties.Contains(pName) Then 'The DE contains this property
                'Update the properties value
                de.Properties(pName)(0) = pValue
            Else    'Property doesnt exist
                'Add the property and set it's value
                de.Properties(pName).Add(pValue)
            End If
        End If
    End Sub

    '' <summary>
    ''' Establish identity (principal) and culture for a thread.
    ''' </summary>
    Public Shared Sub SetCultureAndIdentity()
        AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal)
        Dim principal As WindowsPrincipal = CType(Thread.CurrentPrincipal, WindowsPrincipal)
        Dim identity As WindowsIdentity = CType(principal.Identity, WindowsIdentity)
        System.Threading.Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")
    End Sub

    ''' <summary>
    ''' Procedure to create a new Active Directory account
    ''' </summary>
    ''' <param name="sUserName">Username for the new account</param>
    ''' <param name="sPassword">Password for the new account</param>
    ''' <param name="sFirstName">First name of the user</param>
    ''' <param name="sLastName">Last name of the user</param>
    ''' <param name="sGroupName">Group to add the user to</param>
    ''' <remarks></remarks>
    Public Sub CreateAdAccount(ByVal sUserName As String, _
           ByVal sPassword As String, _
           ByVal sFirstName As String, ByVal sLastName As String, _
           ByVal sGroupName As String)
        'Dim catalog As Catalog = New Catalog()
        Dim dirEntry As New DirectoryEntry()
        ' 1. Create user account
        Dim adUsers As DirectoryEntries = dirEntry.Children
        Dim newUser As DirectoryEntry = adUsers.Add("CN=" & sUserName, "user")
        ' 2. Set properties
        SetADProperty(newUser, "givenname", sFirstName)
        SetADProperty(newUser, "sn", sLastName)
        SetADProperty(newUser, "SAMAccountName", sUserName)
        SetADProperty(newUser, "userPrincipalName", sUserName)
        newUser.CommitChanges()
        ' 3. Set the password
        SetPassword(newUser, sPassword)
        ' 5. Add the user to the specified group
        AddUserToGroup(dirEntry, newUser, sGroupName)
        ' 6. Enable the account
        EnableAccount(newUser)
        ' 7. Close & clean-up
        newUser.Close()
        dirEntry.Close()
    End Sub

    '' <summary>
    ''' Method to set a user's password
    ''' <param name="dEntry">DirectoryEntry to use</param>
    ''' <param name="sPassword">Password for the new user</param>
    Private Shared Sub SetPassword(ByVal dEntry As DirectoryEntry, _
    ByVal sPassword As String)
        Dim oPassword As Object() = New Object() {sPassword}
        Dim ret As Object = dEntry.Invoke("SetPassword", oPassword)
        dEntry.CommitChanges()
    End Sub

    ''' <summary>
    ''' Method to enable a user account in the AD.
    ''' </summary>
    ''' <param name="de"></param>
    Private Shared Sub EnableAccount(ByVal de As DirectoryEntry)
        'UF_DONT_EXPIRE_PASSWD 0x10000
        Dim exp As Integer = CInt(de.Properties("userAccountControl").Value)
        de.Properties("userAccountControl").Value = exp Or &H1
        de.CommitChanges()
        'UF_ACCOUNTDISABLE 0x0002
        Dim val As Integer = CInt(de.Properties("userAccountControl").Value)
        de.Properties("userAccountControl").Value = val And Not &H2
        de.CommitChanges()
    End Sub

    '' <summary>
    ''' Method to add a user to a group
    ''' </summary>
    ''' <param name="de">DirectoryEntry to use</param>
    ''' <param name="deUser">User DirectoryEntry to use</param>
    ''' <param name="GroupName">Group Name to add user to</param>
    Public Shared Sub AddUserToGroup(ByVal de As DirectoryEntry, _
    ByVal deUser As DirectoryEntry, ByVal GroupName As String)
        Dim deSearch As DirectorySearcher = New DirectorySearcher()
        deSearch.SearchRoot = de
        deSearch.Filter = "(&(objectClass=group) (cn=" & GroupName & "))"
        Dim results As SearchResultCollection = deSearch.FindAll()
        Dim isGroupMember As Boolean = False
        If results.Count > 0 Then
            Dim group As New DirectoryEntry(results(0).Path)
            Dim members As Object = group.Invoke("Members", Nothing)
            For Each member As Object In CType(members, IEnumerable)
                Dim x As DirectoryEntry = New DirectoryEntry(member)
                Dim name As String = x.Name
                If name <> deUser.Name Then
                    isGroupMember = False
                Else
                    isGroupMember = True
                    Exit For
                End If
            Next member
            If (Not isGroupMember) Then
                group.Invoke("Add", New Object() {deUser.Path.ToString()})
            End If
            group.Close()
        End If
        Return
    End Sub

    ''' <summary>
    ''' Method that disables a user account in the AD 
    ''' and hides user's email from Exchange address lists.
    ''' </summary>
    ''' <param name="sLogin">Login of the user to disable</param>
    'Public Sub DisableAccount(ByVal sLogin As String)
    '    '   1. Search the Active Directory for the desired user
    '    Dim dirEntry As DirectoryEntry = GetDirectoryEntry()
    '    Dim dirSearcher As DirectorySearcher = New DirectorySearcher(dirEntry)
    '    dirSearcher.Filter = "(&(objectCategory=Person)(objectClass=user)_(SAMAccountName=" & sLogin & "))"
    '    dirSearcher.SearchScope = SearchScope.Subtree
    '    Dim results As SearchResult = dirSearcher.FindOne()
    '    '   2. Check returned results
    '    If Not results Is Nothing Then
    '        '   2a. User was returned
    '        Dim dirEntryResults As DirectoryEntry = _
    '                            GetDirectoryEntry(results.Path)
    '        Dim iVal As Integer = _
    '            CInt(dirEntryResults.Properties("userAccountControl").Value)
    '        '   3. Disable the users account
    '        dirEntryResults.Properties("userAccountControl").Value = iVal Or &H2
    '        '   4. Hide users email from all Exchange Mailing Lists
    '        dirEntryResults.Properties("msExchHideFromAddressLists").Value = "TRUE"
    '        dirEntryResults.CommitChanges()
    '        dirEntryResults.Close()
    '    End If
    '    dirEntry.Close()
    'End Sub

    ''' <summary>
    ''' Method that updates user's properties
    ''' </summary>
    ''' <param name="userLogin">Login of the user to update</param>
    ''' <param name="userDepartment">New department of the specified user</param>
    ''' <param name="userTitle">New title of the specified user</param>
    ''' <param name="userPhoneExt">New phone extension of the specified user</param>
    'Public Sub UpdateUserADAccount(ByVal userLogin As String, _
    '           ByVal userDepartment As String, _
    '           ByVal userTitle As String, ByVal userPhoneExt As String)
    '    Dim dirEntry As DirectoryEntry = GetDirectoryEntry()
    '    Dim dirSearcher As DirectorySearcher = New DirectorySearcher(dirEntry)
    '    '   1. Search the Active Directory for the speied user
    '    dirSearcher.Filter = "(&(objectCategory=Person)(objectClass=user) _(SAMAccountName=" & userLogin & "))"
    '    dirSearcher.SearchScope = SearchScope.Subtree
    '    Dim searchResults As SearchResult = dirSearcher.FindOne()
    '    If Not searchResults Is Nothing Then
    '        Dim dirEntryResults As New DirectoryEntry(results.Path)
    '        'The properties listed here may be different then the 
    '        'properties in your Active Directory so they may need to be 
    '        'changed according to your network
    '        '   2. Set the new property values for the specified user
    '        SetADProperty(dirEntryResults, "department", userDepartment)
    '        SetADProperty(dirEntryResults, "title", userTitle)
    '        SetADProperty(dirEntryResults, "phone", userPhoneExt)
    '        '   3. Commit the changes
    '        dirEntryResults.CommitChanges()
    '        '   4. Close & Cleanup
    '        dirEntryResults.Close()
    '    End If
    '    '   4a. Close & Cleanup
    '    dirEntry.Close()
    'End Sub

    ''' <summary>
    ''' Function to query the Active Directory and return all the computer names 
    '''on the network
    ''' </summary>
    ''' <returns>A collection populated with all the computer names</returns>
    Public Shared Function ListAllADComputers() As Collection
        Dim dirEntry As DirectoryEntry = GetDirectoryEntry()
        Dim pcList As New Collection()
        '   1. Search the Active Directory for all objects with type of computer
        Dim dirSearcher As DirectorySearcher = New DirectorySearcher(dirEntry)
        dirSearcher.Filter = ("(objectClass=computer)")
        '   2. Check the search results
        Dim dirSearchResults As SearchResult
        '   3. Loop through all the computer names returned
        For Each dirSearchResults In dirSearcher.FindAll()
            '   4. Check to ensure the computer name isnt already listed in 
            'the collection
            If Not pcList.Contains(dirSearchResults.GetDirectoryEntry().Name.ToString()) Then
                '   5. Add the computer name to the collection (since 
                'it dont already exist)
                pcList.Add(dirSearchResults.GetDirectoryEntry().Name.ToString())
            End If
        Next
        '   6. Return the results
        Return pcList
    End Function

    ''' <summary>
    ''' Function to return all the groups the user is a member od
    ''' </summary>
    ''' <param name="_path">Path to bind to the AD</param>
    ''' <param name="username">Username of the user</param>
    ''' <param name="password">password of the user</param>
    Private Function GetGroups(ByVal _path As String, ByVal username As String, _
                     ByVal password As String) As Collection
        Dim Groups As New Collection
        Dim dirEntry As New  _
            System.DirectoryServices.DirectoryEntry(_path, username, password)
        Dim dirSearcher As New DirectorySearcher(dirEntry)
        dirSearcher.Filter = String.Format("(sAMAccountName={0}))", username)
        dirSearcher.PropertiesToLoad.Add("memberOf")
        Dim propCount As Integer
        Try
            Dim dirSearchResults As SearchResult = dirSearcher.FindOne()
            propCount = dirSearchResults.Properties("memberOf").Count
            Dim dn As String
            Dim equalsIndex As String
            Dim commaIndex As String
            For i As Integer = 0 To propCount - 1
                dn = dirSearchResults.Properties("memberOf")(i)
                equalsIndex = dn.IndexOf("=", 1)
                commaIndex = dn.IndexOf(",", 1)
                If equalsIndex = -1 Then
                    Return Nothing
                End If
                If Not Groups.Contains(dn.Substring((equalsIndex + 1), _
                                      (commaIndex - equalsIndex) - 1)) Then
                    Groups.Add(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1))
                End If
            Next
        Catch ex As Exception
            If ex.GetType Is GetType(System.NullReferenceException) Then
                'MessageBox.Show("Selected user isn't a member of any groups " & _
                '               "at this time.", "No groups listed", _
                '               MessageBoxButtons.OK, MessageBoxIcon.Error)
                'they are still a good user just does not
                'have a "memberOf" attribute so it errors out.
                'code to do something else here if you want
            Else
                'MessageBox.Show(ex.Message.ToString, "Search Error", & _
                'MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
        Return Groups
    End Function
    ''' <summary>
    ''' This will perfrom a logical operation on the userAccountControl values
    ''' to see if the user account is enabled or disabled.
    ''' The flag for determining if the
    ''' account is active is a bitwise value (decimal =2)
    ''' </summary>
    ''' <param name="userAccountControl"></param>
    ''' <returns></returns>
    'Public Shared Function IsAccountActive(ByVal userAccountControl As Integer) _
    '                       As Boolean
    'Dim accountDisabled As Integer = & _
    '    Convert.ToInt32(ADAccountOptions.UF_ACCOUNTDISABLE)
    '    Dim flagExists As Integer = userAccountControl And accountDisabled
    '    'if a match is found, then the disabled 
    '    'flag exists within the control flags
    '    If flagExists > 0 Then
    '        Return False
    '    Else
    '        Return True
    '    End If
    'End Function

    ''' <summary>
    ''' This will perform the removal of a user from the specified group
    ''' </summary>
    ''' <param name="UserName">Username of the user to remove</param>
    ''' <param name="GroupName">Groupname to remove them from</param>
    ''' <remarks></remarks>
    'Public Shared Sub RemoveUserFromGroup(ByVal UserName As String, _
    '                  ByVal GroupName As String)

    '    Dim Domain As New String("")

    '    'get reference to group
    '    Domain = "/CN=" + GroupName + ",CN=Users," + GetLDAPDomain()
    '    Dim oGroup As DirectoryEntry = GetDirectoryObject(Domain)

    '    'get reference to user
    '    Domain = "/CN=" + UserName + ",CN=Users," + GetLDAPDomain()
    '    Dim oUser As DirectoryEntry = GetDirectoryObject(Domain)

    '    'Add the user to the group via the invoke method
    '    oGroup.Invoke("Remove", New Object() {oUser.Path.ToString()})

    '    oGroup.Close()
    '    oUser.Close()
    'End Sub

    ''' <summary>
    ''' This method will not actually log a user in, but will perform tests to ensure
    ''' that the user account exists (matched by both the username and password), and also
    ''' checks if the account is active.
    ''' </summary>
    ''' <param name="UserName"></param>
    ''' <param name="Password"></param>
    'Public Shared Function Login(ByVal UserName As String, ByVal Password As String) _
    '              As ADWrapper.LoginResult
    '    'first, check if the logon exists based on the username and password
    '    If IsUserValid(UserName, Password) Then
    '        Dim dirEntry As DirectoryEntry = GetUser(UserName)
    '        If Not dirEntry Is Nothing Then
    '            'convert the accountControl value so that a logical 
    '            'operation can be performed
    '            'to check of the Disabled option exists.
    '        Dim accountControl As Integer = & _
    '        Convert.ToInt32(dirEntry.Properties("userAccountControl")(0))
    '            dirEntry.Close()

    '            'if the disabled item does not exist then the account is active
    '            If Not IsAccountActive(accountControl) Then
    '                Return LoginResult.LOGIN_USER_ACCOUNT_INACTIVE
    '            Else
    '                Return LoginResult.LOGIN_OK

    '            End If
    '        Else
    '            Return LoginResult.LOGIN_USER_DOESNT_EXIST
    '        End If
    '    Else
    '        Return LoginResult.LOGIN_USER_DOESNT_EXIST
    '    End If
    'End Function

    ''' <summary>
    ''' This will return a DirectoryEntry object if the user does exist
    ''' </summary>
    ''' <param name="UserName"></param>
    ''' <returns></returns>
    'Public Shared Function GetUser(ByVal UserName As String) As DirectoryEntry
    '    'create an instance of the DirectoryEntry
    '    Dim dirEntry As DirectoryEntry = GetDirectoryObject("/" + GetLDAPDomain())

    '    'create instance fo the direcory searcher
    '    Dim dirSearch As New DirectorySearcher(dirEntry)

    '    dirSearch.SearchRoot = dirEntry
    '    'set the search filter
    '    dirSearch.Filter = "(&(objectCategory=user)(cn=" + UserName + "))"
    '    'deSearch.SearchScope = SearchScope.Subtree;

    '    'find the first instance
    '    Dim searchResults As SearchResult = dirSearch.FindOne()

    '    'if found then return, otherwise return Null
    '    If Not searchResults Is Nothing Then
    '        'de= new DirectoryEntry(results.Path,ADAdminUser, _
    '         ADAdminPassword,AuthenticationTypes.Secure);
    '        'if so then return the DirectoryEntry object
    '        Return searchResults.GetDirectoryEntry()
    '    Else
    '        Return Nothing
    '    End If
    'End Function


    ''' <summary>
    ''' Override method which will perfrom query based on combination of username 
    '''and password This is used with the login process to validate the user 
    '''credentials and return a user object for further validation. This is 
    '''slightly different from the other GetUser... methods as this will use the 
    '''UserName and Password supplied as the authentication to check if the user 
    '''exists, if so then  the users object will be queried using these 
    '''credentials
    ''' </summary>
    ''' <param name="UserName"></param>
    ''' <param name="password"></param>
    ''' <returns></returns>
    'Public Shared Function GetUser(ByVal UserName As String, ByVal Password _
    '                               As String) As DirectoryEntry
    '    'create an instance of the DirectoryEntry
    '    Dim dirEntry As DirectoryEntry = GetDirectoryObject(UserName, Password)

    '    'create instance fo the direcory searcher
    '    Dim dirSearch As New DirectorySearcher()

    '    dirSearch.SearchRoot = dirEntry
    '    'set the search filter
    '    dirSearch.Filter = "(&(objectClass=user)(cn=" + UserName + "))"
    '    dirSearch.SearchScope = SearchScope.Subtree

    '    'set the property to return
    '    'deSearch.PropertiesToLoad.Add("givenName");

    '    'find the first instance
    '    Dim searchResults As SearchResult = dirSearch.FindOne()

    '    'if a match is found, then create directiry object and return, 
    '    'otherwise return Null
    '    If Not searchResults Is Nothing Then
    '        'create the user object based on the admin priv.
    '        dirEntry = New DirectoryEntry(searchResults.Path, ADAdminUser, _
    '                       ADAdminPassword, AuthenticationTypes.Secure)
    '        Return dirEntry
    '    Else
    '        Return Nothing
    '    End If
    'End Function

    ''' <summary>
    ''' This method will attempt to log in a user based on the username and 
    ''' password
    ''' to ensure that they have been set up within the Active Directory. 
    '''This is the basic UserName, Password
    ''' check.
    ''' </summary>
    ''' <param name="UserName"></param>
    ''' <param name="Password"></param>
    'Public Shared Function IsUserValid(ByVal UserName As String, _
    '                       ByVal Password As String) As Boolean
    '    Try
    '        'if the object can be created then return true
    '        Dim dirUser As DirectoryEntry = GetUser(UserName, Password)
    '        dirUser.Close()
    '        Return True
    '    Catch generatedExceptionName As Exception
    '        'otherwise return false
    '        Return False
    '    End Try
    'End Function

    ''' <summary>
    ''' This will perfrom a logical operation on the userAccountControl values
    ''' to see if the user account is enabled or disabled. The flag for determining if the
    ''' account is active is a bitwise value (decimal =2)
    ''' </summary>
    ''' <param name="userAccountControl"></param>
    ''' <returns></returns>
    'Public Shared Function IsAccountActive(ByVal userAccountControl As Integer) _
    '                                       As Boolean
    '    Dim accountDisabled As Integer = _
    '        Convert.ToInt32(ADAccountOptions.UF_ACCOUNTDISABLE)
    '    Dim flagExists As Integer = userAccountControl And accountDisabled
    '    'if a match is found, then the disabled flag exists within the control flags
    '    If flagExists > 0 Then
    '        Return False
    '    Else
    '        Return True
    '    End If
    'End Function



End Class




