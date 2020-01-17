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

Partial Class Reports_CustomerService_VCInvoice
    Inherits System.Web.UI.Page

    Protected Sub RunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunReport.Click

        If (String.IsNullOrEmpty(SDATE.Selected_Date) Or String.IsNullOrEmpty(EDATE.Selected_Date)) Then Return

        Dim sql As String = String.Format( _
            "Select b.FirstName, b.Lastname, a.TourID, a.TourDate, c.Name, d.ComboItem " & _
            "as TourStatus from t_Tour a inner join t_Prospect b on a.ProspectID = b.ProspectiD " & _
            "LEFT outer join t_Campaign c on a.Campaignid = c.campaignid left outer join t_ComboItems d on a.StatusID = d.ComboItemID " & _
            "where a.TourDate between '{0}' and '{1}' and " & _           
            "tourlocationid = (Select comboitemid from t_ComboItems a " & _
            "inner join t_combos b on a.comboid = b.comboid " & _
            "where comboname = 'TourLocation' and comboitem = 'VacationClub') order by tourdate, tourid", SDATE.Selected_Date, EDATE.Selected_Date)

        Dim html As New StringBuilder()

        Using cnn As New SqlConnection(Resources.Resource.cns)
            Using cmd As New SqlCommand(sql, cnn)

                cnn.Open()

                Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If rdr.HasRows = True Then

                    Dim rh() As String = {"Tour #ID", "Date", "Status", "Campaign", "Prospect", "Total"}
                    html.Append("<table style='border-collapse:collapse;' border=1px><tr>")

                    For Each s As String In rh
                        html.AppendFormat("<td><strong>{0}</strong></td>", s)
                    Next
                    html.Append("</tr>")



                    Do While rdr.Read()

                        Dim campaign As String = rdr.Item("Name").ToString().Trim()
                        If (campaign.Equals("4K")) Then
                            campaign = "FourK"
                        End If

                        Dim subtotal As String = BusCampaignPriceForVC.GetCampaignPrice(campaign.Trim())

                        If (rdr.Item("TourStatus").ToString().Equals("Showed") = False) Then
                            subtotal = "0"
                        End If

                        html.AppendFormat("<tr><td style='width:100px'>{0}</td><td style='width:100px'>{1}</td><td style='width:100px'>" & _
                                          "{2}</td><td style='width:100px'>{3}</td><td style='width:150px'>{4}</td><td style='width:100px'>{5}</td></tr>", _
                                          rdr.Item("TourID"), _
                                          String.Format("{0:d}", rdr.Item("TourDate")), _
                                          rdr.Item("TourStatus"), _
                                          rdr.Item("Name"), _
                                          rdr.Item("FirstName") & " " & rdr.Item("LastName"), _
                                          String.Format("{0:c}", Convert.ToDecimal(subtotal.ToString())))


                    Loop

                End If

                html.Append("</table>")
            End Using
        End Using


        If (html.ToString().Length > 0) Then
            LIT.Text = html.ToString()
        End If
    End Sub
End Class
