Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization

Partial Class register_EditRawItem
    Inherits System.Web.UI.Page

    Private cnx_string As String = "data source=sql-01;initial catalog=register;user=asp;password=aspnet;"
    Private ada As SqlDataAdapter

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim itemID As String = Request.QueryString("itemID")
        If Me.IsPostBack = False Then
            Me.multiviewmain.ActiveViewIndex = 0

            Dim ds = Data_Load()

            Me.dropdownlistBrand.DataSource = ds.Tables("Brand")
            Me.dropdownlistBrand.DataTextField = "Name"
            Me.dropdownlistBrand.DataValueField = "BrandID"
            Me.dropdownlistBrand.DataBind()

            Me.dropdownlistUnit.DataSource = ds.Tables("Measure")
            Me.dropdownlistUnit.DataTextField = "Name"
            Me.dropdownlistUnit.DataValueField = "MeasurementUnitID"
            Me.dropdownlistUnit.DataBind()

            Dim itemRow As DataRow = ds.Tables("Items").Rows.Find(itemID)
            If itemRow IsNot Nothing Then
                Me.textboxDescription.Text = itemRow.Item("Item").ToString().Trim()
                Me.textboxUPC.Text = IIf(itemRow.Item("SUPC").Equals(DBNull.Value), "", itemRow.Item("SUPC").ToString().Trim())
                Me.dropdownlistBrand.SelectedValue = IIf(itemRow.Item("BrandID").Equals(DBNull.Value), "", itemRow.Item("BrandID"))
                Me.dropdownlistUnit.SelectedValue = IIf(itemRow.Item("MeasurementUnitID").Equals(DBNull.Value), "", itemRow.Item("MeasurementUnitID"))
            End If
        End If
    End Sub

    Private Function Data_Load() As DataSet

        Dim cnn As New SqlConnection(cnx_string)
        Dim sql As String = String.Empty

        Dim ds As New DataSet()
        Dim brandTable As DataTable = ds.Tables.Add("Brand")
        Dim measureTable As DataTable = ds.Tables.Add("Measure")
        Dim itemTable As DataTable = ds.Tables.Add("Items")

        Dim cmd As New SqlCommand("select * from t_brand order by name", cnn)
        ada = New SqlDataAdapter(cmd)
        ada.Fill(ds, "Brand")

        cmd.CommandText = "select * from t_measurementunit order by name"
        ada.SelectCommand = cmd
        ada.Fill(ds, "Measure")

        cmd.CommandText = "select * from t_items order by item"
        ada.SelectCommand = cmd
        Dim builder As SqlCommandBuilder = New SqlCommandBuilder(ada)

        ada.FillSchema(ds, SchemaType.Source, "Items")
        ada.Fill(ds, "Items")
        cnn.Close()

        Return ds
    End Function

    Private Function Data_Save() As Integer

        Dim ds = Data_Load()

        If String.IsNullOrEmpty(Request.QueryString("itemID")) Then

            Dim row As DataRow = ds.Tables("Items").NewRow()

            row.SetField("supc", Me.textboxUPC.Text.Trim())
            row.SetField("item", Me.textboxDescription.Text.Trim())
            row.SetField("brandid", Me.dropdownlistBrand.SelectedItem.Value)
            row.SetField("measurementunitid", Me.dropdownlistUnit.SelectedItem.Value)

            ds.Tables("Items").Rows.Add(row)
            ada.Update(ds, "Items")
            ds.AcceptChanges()
        Else
            Dim row As DataRow = ds.Tables("Items").Rows.Find(Request.QueryString("itemID"))

            If row IsNot Nothing Then
                row.SetField("supc", Me.textboxUPC.Text.Trim())
                row.SetField("item", Me.textboxDescription.Text.Trim())
                row.SetField("brandid", Me.dropdownlistBrand.SelectedItem.Value)
                row.SetField("measurementunitid", Me.dropdownlistUnit.SelectedItem.Value)

                ada.Update(ds, "Items")
                ds.AcceptChanges()
            End If            
        End If

        Return 0
    End Function


    Protected Sub buttonSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonSubmit.Click
        Data_Save()
    End Sub
End Class
