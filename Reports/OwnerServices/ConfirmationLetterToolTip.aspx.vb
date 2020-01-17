Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.Security
Imports System.Web
Imports System.Configuration
Imports System.Data
Imports System


Partial Class Reports_OwnerServices_ConfirmationLetterToolTip
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim KeyId As String = Request.QueryString("KeyId")
        

        If String.IsNullOrEmpty(KeyId) = False Then

            Using cnn As New SqlConnection(Resources.Resource.cns)

                Dim sql As String = String.Format("SELECT P.FirstName + ' ' + P.LastName Name, cbi.ComboItem " & _
                    "FROM t_Prospect P " & _
                    "INNER JOIN t_Contract C ON P.ProspectID = C.ProspectID " & _
                    "INNER JOIN t_Usage U ON C.ContractID = U.ContractID " & _
                    "LEFT OUTER JOIN T_ComboItems cbi ON cbi.ComboItemID = U.SubTypeID " & _
                    "WHERE U.UsageID = {0}", KeyId)

                Using cmd As New SqlCommand(sql, cnn)
                    cnn.Open()

                    Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    If (rdr.HasRows = True) Then
                        rdr.Read()
                        Response.Write("Owner Name: <strong>" + rdr.Item(0) + "</strong><br/>Membership: " + rdr.Item(1))
                    Else

                        rdr.Close()
                        cnn.Open()
                        sql = String.Format("SELECT P.FirstName + ' ' + P.LastName Name FROM t_Reservations R " & _
                            "INNER JOIN t_Prospect P ON R.ProspectId = P.ProspectId " & _
                            "WHERE R.Reservationid = {0}", KeyId)

                        cmd.CommandText = sql
                        rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                        If (rdr.HasRows = True) Then
                            rdr.Read()
                            Response.Write("Owner Name: <strong>" + rdr.Item(0) + "</strong><br/>Membership: KCP")
                        Else
                            Response.Write("<strong>Reservation or Usage ID ???</strong>")
                        End If

                    End If

                End Using
            End Using

        End If



    End Sub
End Class
