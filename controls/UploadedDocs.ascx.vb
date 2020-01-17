

Partial Class controls_UploadedDocs
    Inherits System.Web.UI.UserControl
    Dim _FileID As Integer = 0
    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0

    Public Sub List()
        Dim sErr_Message As String = ""
        If UCase(_KeyField) = "PERSONNELID" Then
            If Not (CheckSecurity("PERSONNEL", "EDIT", , , Session("UserDBID"), sErr_Message)) Then GoTo Exiting
        End If
        Dim ds As New SqlDataSource(Resources.Resource.cns, "Select FileID as ID, Name, DateUploaded, Path from t_UploadedDocs where keyfield = '" & _KeyField & "' and keyvalue = " & _KeyValue & " order by DateUploaded")
        gvDocs.DataSource = ds
        gvDocs.DataBind()
Exiting:
        lblError.Text = sErr_Message
    End Sub

#Region "Properties"
    Public Property FileID As Integer
        Get
            Return _FileID
        End Get
        Set(ByVal value As Integer)
            _FileID = value
            hfFileID.Value = _FileID
        End Set
    End Property

    Public Property KeyField As String
        Get
            Return _KeyField
        End Get
        Set(ByVal value As String)
            _KeyField = value
            hfKeyField.Value = _KeyField
        End Set
    End Property

    Public Property KeyValue As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
            hfKeyValue.Value = _KeyValue
        End Set
    End Property
#End Region

    Protected Sub Upload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Upload.Click
        If fu.FileName = "" Then Exit Sub
        Dim sParentPath As String = "\\rs-fs-01\UploadedContracts\"
        Dim sFolder As String = ""
        Dim sMid As String = ""
        Dim sFileName As String = Left(fu.FileName, InStr(fu.FileName, ".") - 1)
        Dim sExt As String = Right(fu.FileName, Len(fu.FileName) - InStr(fu.FileName, ".") + 1)
        'save the file
        'Response.Write(UCase(_KeyField))
        Select Case UCase(_KeyField)
            Case "PERSONNELID"
                sParentPath = "\\rs-fs-01\UploadedContracts\"
                sFolder = "HRDocs\"
            Case "CONTRACTID"
                sFolder = "scannedcontracts\"
            Case "PROSPECTID"
                sFolder = "ProspectFiles\"
            Case "RESERVATIONID"
                sFolder = "scannedcontracts\"
                '                sFolder = "ReservationFiles\"
            Case "TOURID"
                sFolder = "tourfiles\"
            Case "MISPROJECT"
                sParentPath = "\\rs-fs-01\UploadedContracts\"
                sFolder = "misprojects\"
            Case "WORKORDERID"
                sFolder = "workorders\"
            Case Else
                sFolder = "scannedcontracts\"
        End Select
        'sFolder = ""
        Dim i As Integer = 0
        Do While FileIO.FileSystem.FileExists(sParentPath & sFolder & sFileName & sMid & sExt)
            i += 1
            sMid = i.ToString
        Loop
        fu.SaveAs(sParentPath & sFolder & sFileName & sMid & sExt)
        'place entry into table
        Dim oUD As New clsUploadedDocs
        oUD.KeyField = _KeyField
        oUD.KeyValue = _KeyValue
        oUD.Load()
        oUD.Name = sFileName & sMid & sExt
        oUD.Path = sParentPath & sFolder & sFileName & sMid & sExt
        oUD.UploadedByID = Session("UserDBID")
        oUD.DateUploaded = Date.Now
        oUD.Save()
        oUD = Nothing

        List()

        'lblError.Text = sFileName & sExt

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            _FileID = IIf(IsNumeric(hfFileID.Value), hfFileID.Value, _FileID)
            _KeyField = IIf(hfKeyField.Value <> "", hfKeyField.Value, _KeyField)
            _KeyValue = IIf(IsNumeric(hfKeyValue.Value), hfKeyValue.Value, _KeyValue)
        End If
    End Sub

    Protected Sub gvDocs_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDocs.SelectedIndexChanged

    End Sub
End Class
