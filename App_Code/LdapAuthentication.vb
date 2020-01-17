Imports Microsoft.VisualBasic

Imports System
Imports System.Text
Imports System.Collections
Imports System.DirectoryServices

Namespace formsauth
    Public Class LdapAuthentication

        Dim _path As String
        Dim _filterAttribute As String
        Dim _dn As String
        Dim _sid As String
        Dim _fname As String
        Dim _lname As String
        Dim i As Integer

        Public Sub New(ByVal path As String)
            _path = path
        End Sub

        Public Function IsAuthenticated(ByVal domain As String, ByVal username As String, ByVal pwd As String) As Boolean

            Dim domainAndUsername As String = domain & "\" & username
            Dim entry As DirectoryEntry = New DirectoryEntry(_path, domainAndUsername, pwd)

            Try
                'Bind to the native AdsObject to force authentication.			
                Dim obj As Object = entry.NativeObject
                Dim search As DirectorySearcher = New DirectorySearcher(entry)

                search.Filter = "(SAMAccountName=" & username & ")"
                search.PropertiesToLoad.Add("cn")
                search.PropertiesToLoad.Add("objectSid")
                search.PropertiesToLoad.Add("givenname")
                search.PropertiesToLoad.Add("sn")

                Dim result As SearchResult = search.FindOne()

                If (result Is Nothing) Then
                    Return False
                End If

                'Update the new path to the user in the directory.
                _path = result.Path
                _filterAttribute = CType(result.Properties("cn")(0), String)
                _sid = ConvertSIDByteToString(result.Properties("objectSid")(0))
                _fname = CType(result.Properties("givenname")(0), String)
                If result.Properties("sn").Count > 0 Then
                    _lname = CType(result.Properties("sn")(0), String)
                Else
                    _lname = ""
                End If

            Catch ex As Exception
                Throw New Exception("Error authenticating user. " & ex.Message)

            End Try

            Return True
        End Function

        Public ReadOnly Property First_Name() As String
            Get
                Return _fname
            End Get
        End Property

        Public ReadOnly Property Last_Name() As String
            Get
                Return _lname
            End Get
        End Property

        Public ReadOnly Property Distinguished_Name() As String
            Get
                Return _path
            End Get
        End Property

        Public ReadOnly Property SID() As String
            Get
                Return _sid
            End Get
        End Property

        Public Function GetGroups() As String
return ""
            Dim search As DirectorySearcher = New DirectorySearcher(_path)
            search.Filter = "(cn=" & _filterAttribute & ")"
            search.PropertiesToLoad.Add("memberOf")
            Dim groupNames As StringBuilder = New StringBuilder()
            'Return search.SearchRoot.Path
            Try
                Dim result As SearchResult = search.FindOne()
                Dim propertyCount As Integer = result.Properties("memberOf").Count

                Dim dn As String
                Dim equalsIndex, commaIndex As Integer

                Dim propertyCounter As Integer

                For propertyCounter = 0 To propertyCount - 1
                    dn = CType(result.Properties("memberOf")(propertyCounter), String)

                    equalsIndex = dn.IndexOf("=", 1)
                    commaIndex = dn.IndexOf(",", 1)
                    If (equalsIndex = -1) Then
                        Return Nothing
                    End If

                    groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1))
                    groupNames.Append("|")
                Next

            Catch ex As Exception
                Throw New Exception("Error obtaining group names. " & ex.ToString)
            End Try

            Return groupNames.ToString()
        End Function

        Public Function Change_Password(ByVal sUser As String, ByVal sOld As String, ByVal sNew As String) As Boolean
            Return False
        End Function

        Private Function ConvertSIDByteToString(ByRef sidBytes As Byte()) As String
            Dim strSID As New StringBuilder
            strSID.Append("S-")
            Try
                strSID.Append(sidBytes(0).ToString)
                'Next Six bytes are SID authority value
                If sidBytes(6) <> 0 Or sidBytes(5) <> 0 Then
                    Dim strAuth As String = String.Format("0x{0:2x}{1:2x}{2:2x}{3:2x}{4:2x}{5:2x}", _
                            CType(sidBytes(1), Int16), _
                            CType(sidBytes(2), Int16), _
                            CType(sidBytes(3), Int16), _
                            CType(sidBytes(4), Int16), _
                            CType(sidBytes(5), Int16), _
                            CType(sidBytes(6), Int16))
                    strSID.Append("-")
                    strSID.Append(strAuth)
                Else
                    Dim iVal As Int64 = CType(sidBytes(1), Int32) + _
                                        CType(sidBytes(2) << 8, Int32) + _
                                        CType(sidBytes(3) << 16, Int32) + _
                                        CType(sidBytes(4) << 24, Int32)
                    strSID.Append("-")
                    strSID.Append(iVal.ToString)
                End If

                'Get sub authority count ...
                Dim iSubCount As Int32 = Convert.ToInt32(sidBytes(7))
                Dim idxAuth = 0
                For Me.i = 0 To iSubCount - 1
                    idxAuth = 8 + i * 4
                    Dim iSubAuth As UInt32 = BitConverter.ToUInt32(sidBytes, idxAuth)
                    strSID.Append("-")
                    strSID.Append(iSubAuth.ToString())
                Next
            Catch ex As Exception
                Return ""
            End Try

            Return strSID.ToString
        End Function

        Public Function Get_All_Groups(ByVal grptype As String) As List(Of String)
            Dim objADAM As DirectoryEntry
            Dim objGroupEntry As DirectoryEntry
            Dim objSearchADAM As DirectorySearcher
            Dim objSearchResults As SearchResultCollection
            Dim strPath As String
            Dim result As New List(Of String)

            strPath = _path '"LDAP://kcp.local"

            objADAM = New DirectoryEntry(strPath)
            objADAM.RefreshCache()

            objSearchADAM = New DirectorySearcher(objADAM)
            If grptype = "Security" Then
                'Security Groups
                objSearchADAM.Filter = "(&(objectClass=group)(objectCategory=group)(groupType:1.2.840.113556.1.4.803:=-2147483646))"
            ElseIf grptype = "Distribution" Then
                'Email Distribution Groups
                objSearchADAM.Filter = "(&(objectClass=group)(!(groupType:1.2.840.113556.1.4.803:=2147483648)))"
            End If
            objSearchADAM.SearchScope = SearchScope.Subtree

            objSearchResults = objSearchADAM.FindAll
            If objSearchResults.Count <> 0 Then
                Dim objResult As SearchResult
                For Each objResult In objSearchResults
                    objGroupEntry = objResult.GetDirectoryEntry
                    result.Add(objGroupEntry.Name)

                Next objResult
            Else
                Throw New Exception("No groups found")
            End If

            result.Sort()

            Return result
            'For Each group As String In result
            'DropDownList2.Items.Add(Group)
            'Next
        End Function
        Public Function Get_Groups(ByVal userName As String) As String
            Dim grps As String = ""
            Dim objADAM As DirectoryEntry
            Dim objSearchADAM As New DirectorySearcher

            Try
                objADAM = New DirectoryEntry(_path)
                objSearchADAM.SearchScope = SearchScope.Subtree
                objSearchADAM.Filter = "(&(objectCategory=user)(SAMAccountName=" & userName & "))"
                Dim res As SearchResult = objSearchADAM.FindOne
                Dim equalsIndex, commaIndex As Integer
                If Not (IsDBNull(res)) Then
                    If res.Properties("memberOf").Count > 0 Then
                        For i As Integer = 0 To res.Properties("memberOf").Count - 1
                            equalsIndex = res.Properties("memberOf")(i).ToString.IndexOf("=", 1)
                            commaIndex = res.Properties("memberOf")(i).ToString.IndexOf(",", 1)
                            '                If (equalsIndex = -1) Then
                            'Return Nothing
                            'End If

                            'If Left(res.Properties("memberOf")(i).ToString, 2) = "CN" Then
                            If grps = "" Then
                                grps = res.Properties("memberOf")(i).ToString.Substring(0, commaIndex)
                                'grps = res.Properties("memberOf")(i).ToString
                            Else
                                grps = grps & "|" & res.Properties("memberOf")(i).ToString.Substring(0, commaIndex)
                            End If
                            'End If
                        Next
                    End If
                End If
            Catch ex As Exception
                '                grps = ""
            End Try
            Return grps
        End Function

        Public Function get_Email(ByVal userName As String) As String
            Dim email As String = ""
            Dim objADAM As DirectoryEntry
            Dim objSearchADAM As New DirectorySearcher
            Try
                objADAM = New DirectoryEntry(_path)
                objSearchADAM.SearchScope = SearchScope.Subtree
                objSearchADAM.Filter = "(&(objectCategory=user)(SAMAccountName=" & userName & "))"
                Dim res As SearchResult = objSearchADAM.FindOne
                If res.Properties("mail").Count > 0 Then
                    email = res.Properties("mail")(0).ToString
                End If
            Catch

            End Try
            Return email
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class
End Namespace