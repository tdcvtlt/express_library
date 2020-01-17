Imports System.Data
Imports System.Data.SqlClient

Partial Class register_Default
    Inherits System.Web.UI.Page

    Private cnx_string As String = "data source=sql-01;initial catalog=register;user=asp;password=aspnet;"
    Dim tables() As String = {"Items", "Brand", "Unit"}

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString.Count = 0 Then
            Dim ds As DataSet = Data_Load(0)
            Gridview_Bind(Me.gridviewItems, ds.Tables(tables(0)), "ItemID")
            Me.multiviewmain.ActiveViewIndex = 0
        End If
    End Sub

  
    Protected Sub gridviewRawItems_PageIndexChanging(ByVal sender As Object, _
                                                     ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) _
                                                    Handles gridviewItems.PageIndexChanging
        Me.gridviewItems.PageIndex = e.NewPageIndex
        Dim ds As DataSet = Data_Load(0)
        Gridview_Bind(Me.gridviewItems, ds.Tables(tables(0)), "ItemID")
    End Sub


    Private Function Data_Load(ByVal viewIndex As Integer) As DataSet
        Dim sq As String = String.Empty
        Dim cn As New SqlConnection(cnx_string)
        Dim cm As SqlCommand = Nothing
        Dim ad As SqlDataAdapter = Nothing
        Dim tb As DataTable = New DataTable()
        Dim ds As New DataSet()

        For Each s As String In tables
            ds.Tables.Add(s)
        Next

        If viewIndex = 0 Then
            sq = _
                "SELECT a.ItemId, a.supc, a.Item, b.Name [Brand], UnitOfMeasure + ' ' + c.Name [Type]  from t_items a left join t_Brand b on a.brandid = b.brandid " & _
                "left join t_MeasurementUnit c on a.MeasurementUnitId = c.MeasurementUnitId " & _
                "order by a.Item"
        ElseIf viewIndex = 1 Then
            sq = "select * from t_Brand union select 0, ''order by Name"
        ElseIf viewIndex = 2 Then
            sq = "select * from t_MeasurementUnit union select 0, '' order by Name"        
        End If

        If viewIndex >= 0 And viewIndex <= 2 Then
            ad = New SqlDataAdapter(sq, cn)

            ad.FillSchema(ds, SchemaType.Source, tables(viewIndex))
            ad.Fill(ds, tables(viewIndex))

        ElseIf viewIndex = 3 Then

            ds.Tables.Add("MenuItems")
            ds.Tables.Add("SubItems")
            ds.Tables.Add("MenuItems2Items")

            ad = New SqlDataAdapter("select * from t_MenuItems order by name", cn)
            ad.FillSchema(ds, SchemaType.Source, "MenuItems")
            ad.Fill(ds, "MenuItems")

            ad = New SqlDataAdapter("select * from t_Items", cn)
            ad.FillSchema(ds, SchemaType.Source, "SubItems")
            ad.Fill(ds, "SubItems")


            ad = New SqlDataAdapter("select * from t_menuItem2Item", cn)
            ad.FillSchema(ds, SchemaType.Source, "MenuItems2Items")
            ad.Fill(ds, "MenuItems2Items")

        End If
        cn.Close()
        Return IIf(viewIndex >= 0 And viewIndex <= 3, ds, Nothing)
    End Function

    Private Sub Gridview_Bind(ByRef gv As GridView, ByVal dt As DataTable, ByVal key As String)
        gv.DataSource = dt
        gv.DataKeyNames = New String() {key}
        gv.DataBind()
    End Sub

    Protected Sub linkbuttonItemsList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkbuttonItemsList.Click
        Me.multiviewmain.ActiveViewIndex = 0
        Dim ds As DataSet = Data_Load(Me.multiviewmain.ActiveViewIndex)

        Gridview_Bind(Me.gridviewItems, ds.Tables(tables(Me.multiviewmain.ActiveViewIndex)), "ItemID")
    End Sub

    Protected Sub linkbuttonBrandList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkbuttonBrandList.Click
        Me.multiviewmain.ActiveViewIndex = 1
        Dim ds As DataSet = Data_Load(Me.multiviewmain.ActiveViewIndex)
        Gridview_Bind(Me.gridviewBrandList, ds.Tables(tables(Me.multiviewmain.ActiveViewIndex)), "BrandID")

    End Sub

    Protected Sub linkbuttonMeasurementList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkbuttonMeasurementList.Click
        Me.multiviewmain.ActiveViewIndex = 2
        Dim ds As DataSet = Data_Load(Me.multiviewmain.ActiveViewIndex)
        Gridview_Bind(Me.gridviewMeasurementList, ds.Tables(tables(Me.multiviewmain.ActiveViewIndex)), "MeasurementUnitID")
    End Sub

    Protected Sub linkbuttonMenuItem2ItemList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkbuttonMenuItem2ItemList.Click
        Me.multiviewmain.ActiveViewIndex = 3
        Dim ds As DataSet = Data_Load(Me.multiviewmain.ActiveViewIndex)

        Dim html As New StringBuilder()
        html.AppendFormat("<table>")

        For Each r As DataRow In ds.Tables("MenuItems").Rows

            html.AppendFormat("<tr><td><img alt='' src=../Images/expand_button.png class='dummyarrow' /></td><td>{0}</td></tr>", r.Item("Name").ToString())
            Dim rows As DataRow() = ds.Tables("MenuItems2Items").Select("MenuItemID = " & r.Item("MenuItemID").ToString())

            If rows.Count > 0 Then
                html.AppendFormat("<tr style=display:none><td></td><td><table>")
                For Each row In rows

                    Dim irow As DataRow = ds.Tables("SubItems").Rows.Find(row.Item("ItemID").ToString())
                    If irow IsNot Nothing Then
                        html.AppendFormat("<tr><td></td><td>{0}</td></tr>", irow.Item("item").ToString())
                    End If
                Next

                html.AppendFormat("</table></td></tr>")
            End If

        Next
        html.AppendFormat("</table>")
        Me.divMenuItem2ItemList.InnerHtml = html.ToString()
    End Sub


    Protected Sub linkbuttonItemsAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkbuttonItemsAdd.Click
        Response.Redirect( _
            String.Format("EditItem.aspx?View=Items"))
    End Sub

    Protected Sub linkbuttonBrandListAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkbuttonBrandListAdd.Click
        Response.Redirect( _
            String.Format("EditItem.aspx?View=Brand"))
    End Sub

    Protected Sub linkbuttonMeasurementListAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkbuttonMeasurementListAdd.Click
        Response.Redirect( _
            String.Format("EditItem.aspx?View=Unit"))
    End Sub

   
End Class
