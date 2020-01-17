Imports System.Data
Imports System.Linq
Imports System.Data.SqlClient

Imports CrystalDecisions.CrystalReports.Engine



Partial Class Reports_Sales_OPCSolicitorSalesSummary
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Bind()
        Else

        End If


    End Sub



    Private Sub Bind()

        Dim l As List(Of IdName) = GetVendors()


        ddVendor.DataSource = l.ToArray()
        ddVendor.DataTextField = "VendorID"
        ddVendor.DataValueField = "OPC"
        ddVendor.DataBind()





    End Sub


    Public Function GetVendors() As IEnumerable(Of IdName)

        Dim cnn As New SqlConnection(Resources.Resource.cns)
        Dim cmd As New SqlCommand("SELECT * FROM CRMSNET.DBO.T_VENDOR WHERE VENDOR = 'DAVE SHELTON' OR VENDOR = 'OPC'", cnn)

        cnn.Open()

        Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

        Dim l As IList(Of IdName) = New List(Of IdName)

        While rdr.Read()

            l.Add(New IdName With {.ID = rdr.Item("VendorID"), .Name = rdr.Item("OPC")})
        End While

        cnn.Close()
        cnn.Dispose()


        Return l.ToList()
    End Function

End Class
