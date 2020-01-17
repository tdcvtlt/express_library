Imports System
Imports System.IO
Imports System.Data
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.Text
Imports System.Linq



Partial Class Reports_Tours_ToursByDateCreated
    Inherits System.Web.UI.Page

    Private cnx As String = Resources.Resource.cns

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            BindAtStart()
        Else

        End If
    End Sub

    Private Function GetCampaignsByName() As IEnumerable(Of IdName)

        Dim sql As String = "Select CampaignID, Name from t_Campaign where active = '1' order by name asc"
        Dim l As IList(Of IdName) = New List(Of IdName)

        Using cnn As New SqlConnection(cnx)
            Using cmd As New SqlCommand(sql, cnn)
                cnn.Open()

                Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                If (rdr.HasRows) Then
                    Do While rdr.Read()
                        l.Add(New IdName With {.ID = rdr.Item(0), .Name = rdr.Item(1)})
                    Loop
                End If
            End Using
        End Using
        Return l
    End Function

    Private Sub BindAtStart()

        ddCampaign.Items.Add(New ListItem("All", 0))
        ddCampaign.AppendDataBoundItems = True

        ddCampaign.DataSource = GetCampaignsByName().AsEnumerable()
        ddCampaign.DataTextField = "Name"
        ddCampaign.DataValueField = "ID"
        ddCampaign.DataBind()
    End Sub





    Protected Sub btRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRunReport.Click

        LIT.Text = String.Empty

        If String.IsNullOrEmpty(ucDateStart.Selected_Date) Or String.IsNullOrEmpty(ucDateEnd.Selected_Date) Then Return

        Dim dateStart As String = ucDateStart.Selected_Date
        Dim dateEnd As String = DateAdd(DateInterval.Day, 1, Date.Parse(ucDateEnd.Selected_Date))
        Dim path As String = "GetReportForToursByDateCreatedIncludesProspectNames.txt"
        Dim rows As IList(Of Guest) = New List(Of Guest)
        Dim sql As String = String.Empty

        Dim campaign As String = String.Empty

        If ddCampaign.SelectedItem.Text.Contains("All") Then
            Dim str As IEnumerable(Of String) = From x As ListItem In ddCampaign.Items _
                                                Where x.Text.Contains("All") = False _
                                                Select x.Value

            Dim arr(str.Count) As String
            Dim z As Integer = 0
            For Each s As String In str
                arr(z) = s
                z += 1
            Next

            campaign = String.Join(",", arr)
            campaign = campaign.Substring(0, campaign.Length - 1)
        Else
            campaign = ddCampaign.SelectedItem.Value
        End If


        Using r As New StreamReader(HttpRuntime.AppDomainAppPath & _
                                    "\Sql\GetReportForToursByDateCreatedIncludesProspectNames.txt")
            sql = r.ReadToEnd()
            r.Close()
            r.Dispose()
        End Using


        Dim sb As New StringBuilder()
        sb.AppendFormat(sql, dateStart, dateEnd, campaign)

        Using cnn As New SqlConnection(cnx)
            Using cmd As New SqlCommand(sb.ToString(), cnn)
                cnn.Open()
                Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If rdr.HasRows Then
                    While rdr.Read()
                        rows.Add(New Guest With { _
                            .TourID = rdr.Item("TourID"), _
                            .DateTour = IIf(IsDBNull(rdr.Item("TourDate")), String.Empty, rdr.Item("TourDate")), _
                            .TimeTour = IIf(IsDBNull(rdr.Item("TourTime")), String.Empty, rdr.Item("TourTime")), _
                            .Campaign = IIf(IsDBNull(rdr.Item("Name")), String.Empty, rdr.Item("Name")), _
                            .FirstName = IIf(IsDBNull(rdr.Item("FirstName")), String.Empty, rdr.Item("FirstName")), _
                            .DateCreated = IIf(IsDBNull(rdr.Item("DateCreated")), String.Empty, rdr.Item("DateCreated")), _
                            .LastName = IIf(IsDBNull(rdr.Item("LastName")), String.Empty, rdr.Item("LastName")), _
                            .UserName = IIf(IsDBNull(rdr.Item("UserName")), String.Empty, rdr.Item("UserName"))})
                    End While
                End If

                ' Very weird...
                '    If rdr.HasRows Then
                '        While rdr.Read()
                '            rows.Add(New Guest() With { _
                '                     .TourID = rdr.Item("TourID"), _
                '                     .LastName = IIf(IsDBNull(rdr.Item("LastName")), String.Empty, rdr.Item("LastName")), _
                '                     .FirstName = IIf(IsDBNull(rdr.Item("FirstName")), String.Empty, rdr.Item("FirstName")), _
                '                     .DateTour = IIf(IsDBNull(rdr.Item("TourDate")), String.Empty, rdr.Item("TourDate")), _
                '                     .TimeTour = IIf(IsDBNull(rdr.Item("TourTime")), String.Empty, rdr.Item("TourTime")) _
                '                     .DateCreated = IIf(IsDBNull(rdr.Item("DateCreated")), String.Empty, rdr.Item("DateCreated")), _
                '                     .Campaign = IIf(IsDBNull(rdr.Item("Name")), String.Empty, rdr.Item("Name"))})
                '        End While
                '    End If
            End Using
        End Using



        Dim list As IEnumerable(Of Guest) = rows.OrderBy(Function(x) x.Campaign) _
                                            .ThenBy(Function(x) x.DateCreated) _
                                            .ThenBy(Function(x) x.LastName)
        Dim Campaigns = From k In list _
                        Group k By CampaignGroup = k.Campaign Into Group _
                        Select CampaignGroup

        If list.Count > 0 Then

            Dim html As New StringBuilder()
            html.Append("<table style='border-collapse:collapse' border='1px'>")           

            For Each c In Campaigns

                Dim campaignName As String = c
                html.AppendFormat("<tr><td colspan='6'><h2>{0}</h2></td></tr>", campaignName)


                Dim d = list.Where(Function(x) x.Campaign.Contains(campaignName)) _
                        .GroupBy(Function(x) New With {Key Date.Parse(x.DateCreated).ToShortDateString()}) _
                        .Select(Function(x) New With {Key .Blue = x.Key, .Green = x.ToList()})


                For Each ele In d
                    html.AppendFormat("<tr><td colspan='6'><h3 style='color:red'>{0}</h3></td></tr>", ele.Blue.ToShortDateString)
                    html.Append("<tr><td><strong>Tour ID</strong></td><td><strong>Guest</strong></td>" & _
                                "<td><strong>Time</strong></td><td><strong>Date Created</strong></td><td><strong>UserName</strong></td></tr>")


                    For Each x In ele.Green.OrderBy(Function(z) z.LastName)

                        html.AppendFormat("<tr>")

                        html.AppendFormat("<td width='70px'>{0}</td>", x.TourID)
                        html.AppendFormat("<td width='230px'>{0}</td>", x.LastName.Trim() & ", " & x.FirstName.Trim())
                        html.AppendFormat("<td width='90px'>{0}</td>", x.TimeTour.Trim())
                        html.AppendFormat("<td width='180px'>{0}</td>", x.DateCreated)
                        html.AppendFormat("<td>{0}</td>", x.UserName)

                        html.AppendFormat("</tr>")


                    Next

                    html.AppendFormat("<tr><td colspan='4'><h5>{0}</h5></td><td style='text-align:right'><h2>Count:  {1}</h2></td></tr>", campaignName, ele.Green.Count)
                Next


            Next

            html.Append("</table>")
            LIT.Text = html.ToString()
        Else

            LIT.Text = "<div><p>No Matching Records Found</p></div>"
        End If



    End Sub

    Protected Class Guest

        Private id As String
        Public datet As String
        Public timet As String
        Public name As String
        Public datec As String
        Public first As String
        Public last As String
        Public user As String


        Public Property TimeTour As String
            Set(ByVal value As String)
                timet = value
            End Set
            Get
                Return timet
            End Get
        End Property
        Public Property Campaign As String
            Set(ByVal value As String)
                name = value
            End Set
            Get
                Return name
            End Get
        End Property
        Public Property FirstName As String
            Set(ByVal value As String)
                first = value
            End Set
            Get
                Return first
            End Get
        End Property
        Public Property LastName As String
            Set(ByVal value As String)
                last = value
            End Set
            Get
                Return last
            End Get
        End Property
        Public Property DateCreated As String
            Set(ByVal value As String)
                datec = value
            End Set
            Get
                Return datec
            End Get
        End Property
        Public Property UserName As String
            Set(ByVal value As String)
                user = value
            End Set
            Get
                Return user
            End Get
        End Property

        Public Property DateTour As String
            Set(ByVal value As String)
                datet = value
            End Set
            Get
                Return datet
            End Get
        End Property

        Public Property TourID As String
            Set(ByVal value As String)
                id = value
            End Set
            Get
                Return id
            End Get
        End Property


    End Class




    Protected Sub btPrintable_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btPrintable.Click


    End Sub





    Protected Sub btToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btToExcel.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=Pender Review.xls")
        Response.Write(LIT.Text)
        Response.End()
    End Sub
End Class
