Imports System
Imports System.Data
Imports System.Data.SqlClient

Partial Class general_Receipt
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            ClientScript.RegisterClientScriptBlock(Me.GetType, "Print", "window.print();", True)
            Dim aItems() As String = Request("ID").Split(",")
            Try
                Dim oPayments As New clsPayments
                Select Case (oPayments.Get_Merchant_Acct(aItems(0), Request("Scheduled")))
                    Case "~0003~", "~0009~"
                        MultiView1.ActiveViewIndex = 1
                    Case "~0006~"
                        MultiView1.ActiveViewIndex = 0
                    Case "~0007~", "~0008~", "~0014~"
                        MultiView1.ActiveViewIndex = 2
                    Case Else
                        MultiView1.ActiveViewIndex = 0
                End Select
                recpt.Text = oPayments.Print_Receipt(Request("ID"), Request("Scheduled"))

            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

        End If
    End Sub
End Class
