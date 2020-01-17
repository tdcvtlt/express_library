Imports System.Data.SqlClient

Partial Class setup_SendEmail
    Inherits System.Web.UI.Page
    Dim bTesting As Boolean = False

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Get_Leads()
        'Send_Mail(txtEmail.Text, "rhill@kingscreekplantation.com", txtSubject.Text, txtMessage.Text, True)
    End Sub

    Private Sub Get_Leads()
        Dim cn As New SqlConnection("data source=RS-SQL-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;")
        Dim cm As New SqlCommand(txtSQL.Text, cn)
        Dim dr As SqlDataReader
        Dim sEmail As String = ""
        Try
            cn.Open()
            dr = cm.ExecuteReader
            If dr.HasRows Then
                While dr.Read
                    sEmail = IIf(bTesting, "rhill@kingscreekplantation.com", dr.Item("Email"))
                    Send_Mail(sEmail, "ownerservices@kingscreekplantation.com", txtSubject.Text, txtMessage.Text.Replace(txtTag.Text, dr.Item(txtTagValue.Text)), True)
                    If bTesting Then Exit While
                End While
            End If
        Catch ex As Exception

        Finally
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            dr = Nothing
            cm = Nothing
            cn = Nothing
        End Try

    End Sub
End Class
