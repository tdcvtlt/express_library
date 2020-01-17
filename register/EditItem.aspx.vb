Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization

Partial Class register_EditItem
    Inherits System.Web.UI.Page

    Private cnx_string As String = "data source=sql-01;initial catalog=register;user=asp;password=aspnet;"
    Private adaItem As SqlDataAdapter
    Private adaBrand As SqlDataAdapter
    Private adaUnit As SqlDataAdapter
    Private tables() As String = {"Items", "Brand", "Unit"}


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString.Count > 0 Then
            If Me.IsPostBack = False Then
                Dim ds = Data_Load()
                Dim row As DataRow = Nothing

                If Array.IndexOf(tables, Request.QueryString("View")) = 0 Then
                    Me.dropdownlistBrand.DataSource = ds.Tables("Brand")
                    Me.dropdownlistBrand.DataTextField = "Name"
                    Me.dropdownlistBrand.DataValueField = "BrandID"
                    Me.dropdownlistBrand.DataBind()

                    Me.dropdownlistUnit.DataSource = ds.Tables("Unit")
                    Me.dropdownlistUnit.DataTextField = "Name"
                    Me.dropdownlistUnit.DataValueField = "MeasurementUnitID"
                    Me.dropdownlistUnit.DataBind()

                    row = ds.Tables("Items").Rows.Find(Request.QueryString("ItemID"))
                    If row IsNot Nothing Then
                        Me.textboxDescription.Text = row.Item("Item").ToString().Trim()
                        Me.textboxUPC.Text = IIf(row.Item("SUPC").Equals(DBNull.Value), "", row.Item("SUPC").ToString().Trim())
                        Me.dropdownlistBrand.SelectedValue = IIf(row.Item("BrandID").Equals(DBNull.Value), "", row.Item("BrandID"))
                        Me.dropdownlistUnit.SelectedValue = IIf(row.Item("MeasurementUnitID").Equals(DBNull.Value), "", row.Item("MeasurementUnitID"))
                        Me.textboxQuantity.Text = IIf(row.Item("unitofmeasure").Equals(DBNull.Value), "", row.Item("unitofmeasure").ToString().Trim())
                    End If
                ElseIf Array.IndexOf(tables, Request.QueryString("View")) = 1 Then

                    row = ds.Tables("Brand").Rows.Find(Request.QueryString("BrandID"))

                    If row IsNot Nothing Then
                        Me.textboxBrandName.Text = IIf(row.Item("Name").Equals(DBNull.Value), "", row.Item("Name").ToString().Trim())
                    End If

                ElseIf Array.IndexOf(tables, Request.QueryString("View")) = 2 Then

                    row = ds.Tables("Unit").Rows.Find(Request.QueryString("MeasurementUnitID"))

                    If row IsNot Nothing Then
                        Me.textboxMeasurementUnitName.Text = IIf(row.Item("Name").Equals(DBNull.Value), "", row.Item("Name").ToString().Trim())
                    End If

                End If
            End If
            Me.multiviewmain.ActiveViewIndex = Array.IndexOf(tables, Request.QueryString("View"))
        End If
    End Sub

    Private Function Data_Load() As DataSet

        Dim cnn As New SqlConnection(cnx_string)
        Dim sql As String = String.Empty
        Dim builder As SqlCommandBuilder = Nothing
        Dim ds As New DataSet()

        For Each s As String In tables
            ds.Tables.Add(s)
        Next

        adaBrand = New SqlDataAdapter(New SqlCommand("select * from t_Brand union select 0, ''order by Name", cnn))
        builder = New SqlCommandBuilder(adaBrand)
        adaBrand.FillSchema(ds, SchemaType.Source, "Brand")
        adaBrand.Fill(ds, "Brand")

        adaUnit = New SqlDataAdapter(New SqlCommand("select * from t_MeasurementUnit union select 0, '' order by Name", cnn))
        builder = New SqlCommandBuilder(adaUnit)

        adaUnit.FillSchema(ds, SchemaType.Source, "Unit")
        adaUnit.Fill(ds, "Unit")

        adaItem = New SqlDataAdapter(New SqlCommand("select * from t_items order by item", cnn))
        builder = New SqlCommandBuilder(adaItem)

        adaItem.FillSchema(ds, SchemaType.Source, "Items")
        adaItem.Fill(ds, "Items")
        cnn.Close()

        Return ds
    End Function

    Private Function Data_Save(ByVal viewIndex As Integer, ByVal keyValue As KeyValuePair(Of String, String)) As Integer
        Dim row As DataRow = Nothing
        Dim ds = Data_Load()

        If viewIndex >= 0 And viewIndex <= 2 Then
            If viewIndex = 0 Then
                If String.IsNullOrEmpty(keyValue.Value) Then

                    row = ds.Tables("Items").NewRow()

                    row.SetField("supc", Me.textboxUPC.Text.Trim())
                    row.SetField("item", Me.textboxDescription.Text.Trim())
                    row.SetField("brandid", Me.dropdownlistBrand.SelectedItem.Value)
                    row.SetField("measurementunitid", Me.dropdownlistUnit.SelectedItem.Value)

                    Dim unit As Nullable(Of Decimal) = 0

                    If Decimal.TryParse(Me.textboxQuantity.Text, unit) Then
                        row.SetField("unitofmeasure", unit)                       
                    End If

                    ds.Tables("Items").Rows.Add(row)
                    adaItem.Update(ds, "Items")
                    ds.AcceptChanges()
                Else
                    row = ds.Tables("Items").Rows.Find(keyValue.Value)
                    If row IsNot Nothing Then
                        row.SetField("supc", Me.textboxUPC.Text.Trim())
                        row.SetField("item", Me.textboxDescription.Text.Trim())
                        If Me.dropdownlistBrand.SelectedItem IsNot Nothing Then
                            row.SetField("brandid", Me.dropdownlistBrand.SelectedItem.Value)
                        End If
                        If Me.dropdownlistUnit.SelectedItem IsNot Nothing Then
                            row.SetField("measurementunitid", Me.dropdownlistUnit.SelectedItem.Value)
                        End If

                        Dim unit As Nullable(Of Decimal) = 0

                        If Decimal.TryParse(Me.textboxQuantity.Text, unit) Then
                            row.SetField("unitofmeasure", unit)
                        End If

                        adaItem.Update(ds, "Items")
                        ds.AcceptChanges()
                    End If
                End If
            ElseIf viewIndex = 1 Then
                If String.IsNullOrEmpty(keyValue.Value) Then
                    row = ds.Tables("Brand").NewRow()
                    row.SetField("Name", Me.textboxBrandName.Text.Trim())

                    ds.Tables("Brand").Rows.Add(row)
                    adaBrand.Update(ds, "Brand")
                    ds.AcceptChanges()
                Else
                    row = ds.Tables("Brand").Rows.Find(keyValue.Value)
                    Response.Write(keyValue.Value)
                    If row IsNot Nothing Then
                        row.SetField("Name", Me.textboxBrandName.Text.Trim())

                        adaBrand.Update(ds, "Brand")
                        ds.AcceptChanges()
                    End If
                End If
            ElseIf viewIndex = 2 Then
                If String.IsNullOrEmpty(keyValue.Value) Then
                    row = ds.Tables("Unit").NewRow()
                    row.SetField("Name", Me.textboxMeasurementUnitName.Text.Trim())

                    ds.Tables("Unit").Rows.Add(row)
                    adaUnit.Update(ds, "Unit")
                    ds.AcceptChanges()
                Else
                    row = ds.Tables("Unit").Rows.Find(keyValue.Value)
                    If row IsNot Nothing Then
                        row.SetField("Name", Me.textboxMeasurementUnitName.Text.Trim())

                        adaUnit.Update(ds, "Unit")
                        ds.AcceptChanges()
                    End If
                End If
            End If

        End If


        Return 0
    End Function


   
    Protected Sub buttonItemSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonItemSubmit.Click
        If Request.QueryString.Count > 0 Then
            Data_Save(Array.IndexOf(tables, Request.QueryString("View")), _
                      New KeyValuePair(Of String, String)( _
                          "ItemID", _
                          Request.QueryString("ItemID")))
        End If
    End Sub

    Protected Sub buttonBrandSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonBrandSubmit.Click
        If Request.QueryString.Count > 0 Then
            Data_Save(Array.IndexOf(tables, Request.QueryString("View")), _
                      New KeyValuePair(Of String, String)( _
                          "BrandID", _
                          Request.QueryString("BrandID")))
        End If
    End Sub

    Protected Sub buttonMeasureSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonMeasureSubmit.Click
        If Request.QueryString.Count > 0 Then
            Data_Save(Array.IndexOf(tables, Request.QueryString("View")), _
                      New KeyValuePair(Of String, String)( _
                          "MeasurementUnitID", _
                          Request.QueryString("MeasurementUnitID")))
        End If
    End Sub
End Class
