Imports System.Data.SqlClient
Imports System.Data

Partial Class marketing_TourLetter
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Dim o_letter As New clsLetters
            Dim content = String.Format("{0}", o_letter.Get_Letter_Text(Request("letterid"), Request("tourid")))

            'if parameters are letterid & tourid, print the letter(s)
            'if parameters are letterid, subject & email, email the letters

            Dim ds As SqlDataSource = New clsTourLetters().Get_Email(Request("tourid"))
            Dim dv As DataView = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)

            If dv.Count = 1 Then
                If Not Request("function") Is Nothing Then
                    If Request("function").Equals("email") Then
                        Send_Mail(dv(0)("email").ToString(), Request("fromemail"), Request("subject"), content, True)
                        Dim ud = New clsUploadedDocs()
                        ud.DateUploaded = DateTime.Now
                        ud.KeyField = "tourid"
                        ud.KeyValue = Request("tourid")
                        ud.Name = "confirmation email"
                        ud.ContentText = content

                        ud.Path = content
                        ud.Save()
                    End If
                End If
                lit1.Text = content

            End If

        End If
    End Sub


   
End Class
