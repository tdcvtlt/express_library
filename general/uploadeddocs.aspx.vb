Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine

Partial Class general_uploadeddocs
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If UCase(Request("f")) = "VIEW" And Request("path") <> "" Then
            Dim Name As String = ""
            For i = 1 To Len(CStr(Request("Path")))
                If Left(Right(CStr(Request("Path")), i), 1) = "\" Then
                    Name = Right(CStr(Request("Path")), i - 1)
                    Exit For
                End If
            Next
            Dim path As String = Request.ApplicationPath
            If File.Exists(Request("Path").ToLower.Replace("\\nndc", "\\rs-fs-01")) Then
                'Response.Write("New Location")
                path &= Request("path").ToLower.Replace("\\nndc\uploadedcontracts", "/hrdocs")
                path = path.ToLower.Replace("\\rs-fs-01\uploadedcontracts", "/hrdocs")
                'Response.Write("<br />" & path & "<br />" & Request("Path") & "<br />" & Request.ApplicationPath)
                'Response.End()
            Else
                path &= Request("path").ToLower.Replace("\\nndc\uploadedcontracts", "/scannedcontracts/")
            End If


            path.Replace("\", "/")

            If UCase(Request("d")) = "CONTRACTID" Then
                If Right(Name, 4) = ".pfl" Then
                    Response.AppendHeader("contect-disposition", "attachment; filename=" & Name)
                    Response.ContentType = "application/pfl"
                    'Response.WriteFile(Request.ApplicationPath & "/scannedcontracts/scannedcontracts/" & Name)
                    Response.WriteFile(path)
                    Response.End()
                ElseIf Right(LCase(Name), 4) = ".msg" Then
                    'Response.AppendHeader("contect-disposition", "attachment; filename=" & Name)
                    'Response.ContentType = "application/msg"
                    'Response.WriteFile(Request.ApplicationPath & "/scannedcontracts/scannedcontracts/" & Name)
                    Response.WriteFile(path)
                    Response.End()
                Else
                    'Response.Redirect(Request.ApplicationPath & "/scannedcontracts/scannedcontracts/" & Name)
                    Response.Redirect(path)
                End If

            ElseIf UCase(Request("d")) = "PERSONNELID" Then
                Response.Redirect(path) 'Request.ApplicationPath & "/scannedcontracts/hrdocs/" & Name)
            ElseIf UCase(Request("d")) = "TOURID" Then
                Response.Redirect(path) 'Request.ApplicationPath & "/scannedcontracts/TourFiles/" & Name)
            ElseIf UCase(Request("d")) = "WORKORDERID" Then
                Response.Redirect(path) 'Request.ApplicationPath & "/scannedcontracts/WorkOrders/" & Name)
            Else
                'Response.Redirect(Request.ApplicationPath & "/scannedcontracts/scannedcontracts/" & Name)
                Response.Redirect(path)
            End If
            'Close()
        ElseIf UCase(Request("f")) = "VIEW" Then
            Dim oUD As New clsUploadedDocs
            oUD.FileID = Request("id")
            oUD.Load()
            If oUD.ContentText <> "" Then
                If Left(oUD.ContentText, 7).ToUpper = "REPORT:" Then
                    Dim Report As New ReportDocument
                    Dim tmp() As String = oUD.ContentText.Split(":")
                    Dim sReport As String = "../wizards/accounting/invoices/" & tmp(1)
                    'ltContentText.Text = "1:" & tmp(1) & " 2:" & tmp(2) & " 3:" & tmp(3)
                    CrystalReportViewer1.Visible = True
                    Session("Report") = Nothing
                    Report.Load(Server.MapPath(sReport))
                    Report.FileName = Server.MapPath(sReport)
                    'Response.Write("HERE")
                    'Dim categoryID As Integer = Convert.ToInt32(ddlCategory.SelectedValue)
                    Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
                    'Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
                    Report.SetParameterValue("@SID", tmp(2)) 'Session("SessionID"))
                    Report.SetParameterValue("@ID", tmp(3))
                    Session.Add("Report", Report)
                    CrystalReportViewer1.ReportSource = Session("Report")
                    CrystalReportViewer1.Visible = True
                Else
                    ltContentText.Text = oUD.ContentText
                End If
            Else
                Close()
            End If
            oUD = Nothing
        ElseIf UCase(Request("f")) = "DELETE" Then
            If CheckSecurity("UploadedDocs", "Delete", , , Session("UserDBID")) Then
                System.IO.File.Delete(Request("Path"))
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "access", "alert('Access Denied');", True)
            End If
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Docs();window.close();", True)
            Close()
        Else
            Close()
        End If
    End Sub

    Private Sub Close()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "close", "window.close();", True)
    End Sub
End Class
