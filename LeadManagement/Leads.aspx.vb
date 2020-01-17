
Partial Class LeadManagement_Leads
    Inherits System.Web.UI.Page

    Protected Sub lbFiles_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFiles.Click
        MultiView2.ActiveViewIndex = 1
        MultiView1.ActiveViewIndex = 1
        gvFiles.DataSource = (New clsLeadFiles).List
        gvFiles.DataBind()
    End Sub

    Protected Sub gvFiles_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFiles.RowDataBound
        If Not (e.Row Is Nothing) And e.Row.Cells.Count > 1 Then
            If e.Row.Cells(4).Text <> "Completed" And e.Row.Cells(4).Text <> "Status" Then
                e.Row.Cells(0).Text = ""
                tmrRefresh.Enabled = True
            ElseIf e.Row.Cells(4).Text <> "Status" Then
                e.Row.Cells(0).Text = "<a href='leadlistdownload.aspx?fileid=" & e.Row.Cells(1).Text & "'>Download</a>"
            End If
            If e.Row.Cells(3).Text <> "FileName" Then
                Dim tmp() As String = Split(e.Row.Cells(3).Text, "\")
                e.Row.Cells(3).Text = tmp(UBound(tmp))
            End If
        End If
    End Sub

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        If file1.FileName = "" Then
            lblStatus.Text = "No File"
            Exit Sub
        End If
        Dim sParentPath As String = "\\rs-fs-01\uploads\" '"\\nndc\UploadedContracts\"
        Dim sFolder As String = ""
        Dim sMid As String = ""
        Dim sFileName As String = Left(file1.FileName, InStr(file1.FileName, ".") - 1)
        Dim sExt As String = Right(file1.FileName, Len(file1.FileName) - InStr(file1.FileName, ".") + 1)
        If sExt.ToUpper <> ".CSV" Then
            'ClientScript.RegisterClientScriptBlock(Me.GetType, "Type", "alert('Please select only comma separated value (CSV) files');", True)
            lblStatus.Text = "Please select only comma separated value (CSV) files"
            Exit Sub
        End If
        'save the file
        'Response.Write(UCase(_KeyField))
        sParentPath = "\\rs-fs-01\uploads\" '"\\nndc\UploadedContracts\"
        sFolder = "misprojects\"

        Dim i As Integer = 0
        Do While FileIO.FileSystem.FileExists(sParentPath & sFolder & sFileName & sMid & sExt)
            i += 1
            sMid = i.ToString
        Loop
        File1.SaveAs(sParentPath & sFolder & sFileName & sMid & sExt)
        hfFile.Value = sParentPath & sFolder & sFileName & sMid & sExt
        Read_First_Line()
        MultiView2.ActiveViewIndex = 0
    End Sub

    Private Sub Read_First_Line()
        lblStatus.Text = ""
        If FileIO.FileSystem.FileExists(hfFile.Value) Then
            Dim ts As IO.StreamReader = FileIO.FileSystem.OpenTextFileReader(hfFile.Value)
            Dim line1() As String = ts.ReadLine.Split(",")
            Dim li As New ListItem("Choose", -1)
            ddphone.Items.Clear()
            ddphone.items.add(li)
            For i = 0 To UBound(line1)
                ddphone.Items.Add(New ListItem(line1(i), i))
            Next
            ts.Close()
            ts = Nothing
        End If
    End Sub

    Private Sub Fill_DropDowns()
        ddListTemplate.DataSource = (New clsLeadTemplates).List(True)
        ddListTemplate.DataTextField = "Name"
        ddListTemplate.DataValueField = "LeadTemplateID"
        ddListTemplate.DataBind()

        Dim sArray() As String = {"PhoneNumber", "LastName", "FirstName", "LeadID"}
        For i = 0 To UBound(sArray)
            ddFilter.Items.Add(sArray(i))
        Next
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Fill_DropDowns()
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        If ddPhone.SelectedItem.Text <> "Choose" And ddListTemplate.SelectedValue <> 0 Then
            Dim oDNC As New clsLeadFiles
            oDNC.FileID = 0
            oDNC.Load()
            oDNC.FileName = hfFile.Value.ToString
            oDNC.PhoneColumn = ddPhone.SelectedValue
            oDNC.Header = cbHeaders.Checked
            oDNC.Status = "Uploaded"
            oDNC.UserDBID = Session("UserDBID")
            oDNC.UserID = Session("UserDBID")
            oDNC.DateUploaded = Date.Now
            oDNC.TemplateID = ddListTemplate.SelectedValue
            oDNC.Save()
            MultiView1.ActiveViewIndex = 1
            MultiView2.ActiveViewIndex = 1
            gvFiles.DataSource = oDNC.List()
            gvFiles.DataBind()
            oDNC = Nothing
            ddPhone.Items.Clear()
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "col", "alert('Please select a list template and the column containing the phone number.');", True)
        End If
    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        gvLeads.DataSource = (New clsLeads).List(ddFilter.SelectedItem.Text, txtFilter.Text)
        gvLeads.DataBind()
    End Sub

    Protected Sub lbLeads_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLeads.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub lbAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAdd.Click
        Response.Redirect("editlead.aspx?leadid=0")
    End Sub
End Class
