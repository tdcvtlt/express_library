
Partial Class marketing_dncdownload
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("FileID") = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
        ElseIf Request("FileID") = "0" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
        ElseIf Request("FileID") = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
        Else
            get_file()
        End If
    End Sub

    Private Sub Get_File()
        Dim cn As New Data.SqlClient.SqlConnection(Resources.Resource.cns)
        Dim cm As New Data.SqlClient.SqlCommand("Select * from t_LeadFileItems where fileid = " & Request("FileID") & " order by HeaderRow desc", cn)
        cn.Open()
        Dim bFinish As Boolean = False
        Dim dr As Data.SqlClient.SqlDataReader = cm.ExecuteReader
        If dr.HasRows Then
            Write_Export_Header()
            While dr.Read
                Dim sLine As String = dr("Row_Text")
                
                Write_Export_Line(sLine)
            End While
            bFinish = True
        End If
        dr.Close()
        dr = Nothing
        If cn.State <> Data.ConnectionState.Closed Then cn.Close()
        cm = Nothing
        cn = Nothing
        If bFinish Then
            Dim oDNC As New clsLeadFiles
            oDNC.FileID = Request("FileID")
            oDNC.Load()
            oDNC.DateDownloaded = Date.Now
            oDNC.Save()
            oDNC = Nothing
            Write_Export_Footer()
        End If

    End Sub

    Private Sub Write_Export_Header()
        Dim oDNC As New clsLeadFiles
        oDNC.FileID = Request("FileID")
        oDNC.Load()
        Dim tmp() As String = Split(oDNC.FileName, "\")
        Response.ClearHeaders()
        Response.AppendHeader("Content-Disposition", "attachment; filename=" & tmp(UBound(tmp)))
        Response.ContentType = "text/plain"
        oDNC = Nothing
    End Sub

    Private Sub Write_Export_Line(ByVal sLine As String)
        Response.Write(sLine & vbCrLf)
    End Sub

    Private Sub Write_Export_Footer()
        Response.End()
    End Sub
End Class
