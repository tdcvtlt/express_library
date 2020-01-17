
Imports System.Data.SqlClient

Partial Class marketing_PremiumSignatureReceipt
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Int32.TryParse(Request("PremiumIssuedID"), 0) Then

            Dim sql = String.Format("select top 1 s.Signature, p.Description, s.DateUploaded, p.PremiumName from t_PremiumIssued2Signature pi2s " _
                & "inner join t_Signature s on s.SignatureID = pi2s.SignatureID " _
                & "inner join t_PremiumIssued pi on pi.PremiumIssuedID = pi2s.PremiumIssuedID " _
                & "inner join t_Tour t on t.TourID = pi.KeyValue and pi.KeyField = 'tourId' " _
                & "inner join t_Premium p on p.PremiumID = pi.PremiumID " _
                & "where pi.PremiumIssuedID = {0} " _
                & "order by s.SignatureID desc", Request("PremiumIssuedID"))

            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        Dim reader = cm.ExecuteReader()
                        reader.Read()

                        If reader.HasRows = False Then
                            cn.Close()
                            ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
                        Else
                            Dim img = CType(reader(0), Byte())
                            Dim base64String = Convert.ToBase64String(img)

                            signature.ImageUrl = String.Format("data:image/jpg;base64,{0}", base64String)

                            [date].Text = String.Format("Date signed: {0}", DateTime.Parse(reader(2).ToString).ToLongDateString())
                            premium.Text = reader(1).ToString()
                        End If
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using

            End Using
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
        End If




    End Sub


End Class
