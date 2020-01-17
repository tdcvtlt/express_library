Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Linq
Imports System.Data.SqlClient
Imports System.Data.Linq


Public Class RoutinesRepository

    Private Shared cnn As SqlConnection
    Private Shared cmd As SqlCommand
    Private Shared ada As SqlDataAdapter
    Private Shared ds As DataSet

    Public Shared Function GetTourCampaigns() As IEnumerable(Of TourCampaign)

        Dim cnn As SqlConnection = Nothing
        Dim ada As SqlDataAdapter = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim ds As DataSet = Nothing
        Dim rdr As SqlDataReader = Nothing


        cnn = New SqlConnection(Resources.Resource.cns)
        cmd = New SqlCommand("SELECT * FROM T_CAMPAIGN", cnn)

        cnn.Open()

        rdr = cmd.ExecuteReader()

        Dim l As IList(Of TourCampaign) = New List(Of TourCampaign)


        Do While (rdr.Read())

            l.Add(New TourCampaign With {.ID = rdr.Item("CampaignID"), .Name = rdr.Item("Name")})
        Loop


        cnn.Close()
        cnn.Dispose()


        Return l.ToArray()


    End Function


End Class
