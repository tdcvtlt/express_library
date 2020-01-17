Imports System
Imports System.Data
Imports System.Data.SqlClient


Partial Class marketing_TourPromotion
    Inherits System.Web.UI.Page


   
    Private Sub Home_Load()

        hfd_promotioncontentid.Value = String.Empty
        hfd_promotionid.Value = String.Empty

        GridView1.DataSource = New Promotion().List()
        GridView1.DataKeyNames = New String() {"promotionid"}

        ddl_inventory.DataSource = New Content(0).InventoryDataSource
        ddl_inventory.DataTextField = "name"
        ddl_inventory.DataValueField = "id"

        ddl_unit.DataSource = New Content(0).UnitDataSource
        ddl_unit.DataTextField = "name"
        ddl_unit.DataValueField = "id"

        ddl_nights.DataSource = Array.ConvertAll(Enumerable.Range(1, 6).ToArray(), New Converter(Of Integer, String)(Function(x As Integer) x.ToString())).Concat(New String() {" "}).OrderBy(Function(x) x)
        ddl_size.DataSource = Array.ConvertAll(Enumerable.Range(1, 4).ToArray(), New Converter(Of Integer, String)(Function(x As Integer) x.ToString)).Concat(New String() {" "}).OrderBy(Function(x) x)

        Me.DataBind()

        multi_view_main.SetActiveView(View1)

        Dim p1 = New Premiums()
        p1.Add(1, "Captain George", 100D, 150)
        Dim s1 As String()
        s1 = p1.Current(1)
        Dim isf = p1.Find("Captain Georges")
        lbl_purpose.Text = s1(0)



        Dim state As New States()
        state.Add(2, "Virginia", True)
        state.Add(3, "California", False)
        state.Add(4, "Florida", True)
        state.Add(5, "New York", True)

        Dim xf = state.Find(True)
        lbl_purpose.Text = xf(2).Name


        Dim products = New Products()



        products.Add(1, "Cottage", 1)
        products.Add(2, "Estates", 1)

        lbl_purpose.Text = products.Distinct(1)


        ddl_premiums_qty.DataSource = Enumerable.Range(1, 9)
        ddl_premiums_qty.DataBind()

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Dim sql = String.Format("select * from t_premium where active = 1 order by premiumname")
            Using ada = New SqlDataAdapter(sql, cnn)

                Dim dt = New DataTable
                ada.Fill(dt)

                lsb_premium_1.DataSource = dt
                lsb_premium_1.DataTextField = "premiumName"
                lsb_premium_1.DataValueField = "premiumid"

                lsb_premium_1.DataBind()

                sql = String.Format("select * from t_comboitems where comboid = 361 and active=1 order by comboItem")
                ada.SelectCommand = New SqlCommand(sql, cnn)

                dt.Clear()
                ada.Fill(dt)

                ddl_reservation_source.DataSource = dt
                ddl_reservation_source.DataTextField = "comboItem"
                ddl_reservation_source.DataValueField = "comboItemid"
                ddl_reservation_source.DataBind()


                sql = String.Format("select * from t_campaign where active=1 order by name")
                ada.SelectCommand = New SqlCommand(sql, cnn)

                dt.Clear()
                ada.Fill(dt)

                cbl_campaigns.DataSource = dt
                cbl_campaigns.DataTextField = "name"
                cbl_campaigns.DataValueField = "campaignid"
                cbl_campaigns.DataBind()

                sql = String.Format("select * from t_comboitems where comboid=281 and comboitem is not null order by comboitem")
                ada.SelectCommand = New SqlCommand(sql, cnn)

                dt.Clear()
                ada.Fill(dt)

                cbl_states.DataSource = dt
                cbl_states.DataTextField = "comboItem"
                cbl_states.DataValueField = "comboItemID"
                cbl_states.DataBind()

                sql = String.Format("select * from t_comboitems where comboid=319 order by comboitem")
                ada.SelectCommand = New SqlCommand(sql, cnn)

                dt.Clear()
                ada.Fill(dt)

                cbl_reservation_types.DataSource = dt
                cbl_reservation_types.DataTextField = "comboItem"
                cbl_reservation_types.DataValueField = "comboItemID"
                cbl_reservation_types.DataBind()

            End Using
        End Using

        Dim inventory_source() = New String() {"1BR Cottage", "2BR Cottage", "3BR Cottage", _
                                               "2BR Townes", "4BR Townes", _
                                               "1BR Estates", "2BR Estates", "3BR Estates", "4BR Estates"}

        lsb_inventory_1.DataSource = inventory_source
        lsb_inventory_1.DataBind()


        ddl_inout.DataSource = New String() {"In", "Out"}
        ddl_inout.DataBind()


        ddl_promo_nights.DataSource = Enumerable.Range(2, 7)
        ddl_promo_nights.DataBind()

        tbx_inventory_price.Text = "Enter price"

        ddl_numeric.DataSource = Enumerable.Range(1, 12)
        ddl_numeric.DataBind()
    End Sub

    Private Sub Control_Load(ByVal promo As Promotion)

        promo.List(promo.PromotionID)

        tbx_name.Text = promo.Name
        tbx_promocode.Text = promo.Code
        tbx_price.Text = promo.Price
        uc1_bookingF.Selected_Date = IIf(promo.BookFrom.Equals(Nothing), String.Empty, promo.BookFrom)
        uc1_bookingT.Selected_Date = IIf(promo.BookTo.Equals(Nothing), String.Empty, promo.BookTo)
        uc1_checkingF.Selected_Date = IIf(promo.CheckinFrom.Equals(Nothing), String.Empty, promo.CheckinFrom)
        uc1_checkingT.Selected_Date = IIf(promo.CheckinTo.Equals(Nothing), String.Empty, promo.CheckinTo)

    End Sub


    Private Sub Control_Load(content As Content)

        content.List(content.PromotionContentId)

        ddl_inventory.SelectedValue = content.Inventory
        ddl_unit.SelectedValue = content.Unit
        ddl_nights.SelectedValue = content.Nights
        ddl_size.SelectedValue = content.Size
        cbx_upgraded.Checked = content.UpgradeAllowed

        hfd_promotioncontentid.Value = content.PromotionContentId
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Home_Load()
        End If
    End Sub

    Protected Sub btn_Back_1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Back_1.Click
        multi_view_main.SetActiveView(View1)
    End Sub


    Protected Sub btn_Cancel_1_Click(sender As Object, e As System.EventArgs) Handles btn_Cancel_1.Click
        multi_view_main.SetActiveView(View2)
    End Sub

    Protected Sub btn_Save_1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save_1.Click

        Dim promo = New Promotion()

        promo.Name = IIf(tbx_name.Text.Trim().CompareTo("") = 0, "", tbx_name.Text.Trim())
        promo.Price = IIf(tbx_price.Text.CompareTo("") = 0, 0, tbx_price.Text)
        promo.Code = tbx_promocode.Text.Trim()

        If String.IsNullOrEmpty(uc1_bookingF.Selected_Date) Then
            promo.BookFrom = Nothing
        Else
            promo.BookFrom = DateTime.Parse(uc1_bookingF.Selected_Date)
        End If

        If String.IsNullOrEmpty(uc1_bookingT.Selected_Date) Then
            promo.BookTo = Nothing
        Else
            promo.BookTo = DateTime.Parse(uc1_bookingT.Selected_Date)
        End If

        If String.IsNullOrEmpty(uc1_checkingF.Selected_Date) Then
            promo.CheckinFrom = Nothing
        Else
            promo.CheckinFrom = DateTime.Parse(uc1_checkingF.Selected_Date)
        End If

        If String.IsNullOrEmpty(uc1_checkingT.Selected_Date) Then
            promo.CheckinTo = Nothing
        Else
            promo.CheckinTo = DateTime.Parse(uc1_checkingT.Selected_Date)
        End If


        If String.IsNullOrEmpty(hfd_promotionid.Value) Then
            promo.PromotionID = 0
        Else
            promo.PromotionID = Integer.Parse(hfd_promotionid.Value)
        End If
        promo.Save(promo.PromotionID)
        Home_Load()
    End Sub

    Protected Sub btn_Add1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Add1.Click
        Dim promo = New Promotion()
        promo.PromotionID = 0
        Control_Load(promo)

        btn_Add_2.Visible = False
        GridView2.Visible = False

        multi_view_main.SetActiveView(View2)
    End Sub


    Protected Sub btn_Add_2_Click(sender As Object, e As System.EventArgs) Handles btn_Add_2.Click

        Dim content = New Content(0)

        ddl_inventory.SelectedValue = 0
        ddl_unit.SelectedValue = 0
        ddl_nights.DataSource = Array.ConvertAll(Enumerable.Range(1, 6).ToArray(), New Converter(Of Integer, String)(Function(x As Integer) x.ToString())).Concat(New String() {" "}).OrderBy(Function(x) x)
        ddl_size.DataSource = Array.ConvertAll(Enumerable.Range(1, 4).ToArray(), New Converter(Of Integer, String)(Function(x As Integer) x.ToString)).Concat(New String() {" "}).OrderBy(Function(x) x)
        cbx_upgraded.Checked = False

        hfd_promotioncontentid.Value = 0
        multi_view_main.SetActiveView(View3)
    End Sub

    Protected Sub btn_Save_2_Click(sender As Object, e As System.EventArgs) Handles btn_Save_2.Click

        Dim con = New Content(0)

        con.PromotionContentId = hfd_promotioncontentid.Value
        con.PromotionId = hfd_promotionid.Value
        con.Inventory = ddl_inventory.SelectedItem.Value
        con.Unit = ddl_unit.SelectedItem.Value
        con.Nights = ddl_nights.SelectedItem.Text
        con.Size = ddl_size.SelectedItem.Text
        con.UpgradeAllowed = cbx_upgraded.Checked

        hfd_promotioncontentid.Value = con.Save(hfd_promotioncontentid.Value)

        con.PromotionContentId = hfd_promotioncontentid.Value
        con.PromotionId = hfd_promotionid.Value
        GridView2.DataSource = con.List()
        GridView2.DataKeyNames = New String() {"promotioncontentid"}
        GridView2.DataBind()

        multi_view_main.SetActiveView(View2)
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

        If e.CommandName.CompareTo("select") = 0 Then

            Dim promo_id = Integer.Parse(e.CommandArgument)
            Dim promo = New Promotion()
            promo.PromotionID = promo_id

            Control_Load(promo)

            Dim con = New Content(promo_id)

            GridView2.DataSource = con.List()
            GridView2.DataKeyNames = New String() {"promotioncontentid"}
            GridView2.DataBind()

            btn_Add_2.Visible = Not False
            GridView2.Visible = Not False

            multi_view_main.SetActiveView(View2)

            hfd_promotionid.Value = promo_id
        End If
    End Sub

    Protected Sub GridView2_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView2.RowCommand

        If e.CommandName.CompareTo("select") = 0 Then

            Dim key = Integer.Parse(e.CommandArgument)
            Dim con = New Content(0)
            con.PromotionContentId = key
            Control_Load(con)

            hfd_promotioncontentid.Value = key
            multi_view_main.SetActiveView(View3)
        End If
    End Sub

    Private Interface IAccess

        Function List(Optional ByVal id As Integer = 0) As DataTable
        Function Save(Optional ByVal id As Integer = 0) As Integer
    End Interface


    Private Class Promotion
        Implements IAccess

        Public Sub New()
        End Sub

        Private _promotionId As Integer
        Private _name As String
        Private _code As String
        Private _price As Decimal
        Private _bookF As DateTime?
        Private _bookT As DateTime?
        Private _checkF As DateTime?
        Private _checkT As DateTime?

        Public Property PromotionID As Integer
            Get
                Return _promotionId
            End Get
            Set(ByVal value As Integer)
                _promotionId = value
            End Set
        End Property

        Public Property Code As String
            Get
                Return _code
            End Get
            Set(ByVal value As String)
                _code = value
            End Set
        End Property

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property Price As Decimal
            Get
                Return _price
            End Get
            Set(ByVal value As Decimal)
                _price = value
            End Set
        End Property

        Public Property BookFrom As DateTime?
            Get
                Return _bookF
            End Get
            Set(ByVal value As DateTime?)
                _bookF = value
            End Set
        End Property

        Public Property BookTo As DateTime?
            Get
                Return _bookT
            End Get
            Set(ByVal value As DateTime?)
                _bookT = value
            End Set
        End Property

        Public Property CheckinFrom As DateTime?
            Get
                Return _checkF
            End Get
            Set(ByVal value As DateTime?)
                _checkF = value
            End Set
        End Property

        Public Property CheckinTo As DateTime?
            Get
                Return _checkT
            End Get
            Set(ByVal value As DateTime?)
                _checkT = value
            End Set
        End Property

        Public Function List(Optional ByVal id As Integer = 0) As System.Data.DataTable Implements IAccess.List
            Dim sql = IIf(id = 0, String.Format("select * from t_promotion order by DateCreated DESC"), _
                                      String.Format("select * from t_promotion where promotionid in ({0})", id))

            Using cnn = New SqlConnection(Resources.Resource.cns)
                Using ada = New SqlDataAdapter(sql, cnn)

                    Dim dt = New DataTable()
                    ada.Fill(dt)

                    If id > 0 Then
                        Dim r = dt.Rows(0)

                        Name = r.Field(Of String)("Name")
                        Price = r.Field(Of Decimal)("Price")

                        BookFrom = r.Field(Of DateTime?)("BookingDateFrom")
                        BookTo = r.Field(Of DateTime?)("BookingDateTo")
                        CheckinFrom = r.Field(Of DateTime?)("CheckingDateFrom")
                        CheckinTo = r.Field(Of DateTime?)("CheckingDateTo")
                        Code = r.Field(Of String)("code")

                        If String.IsNullOrEmpty(Code) = False Then Code.Trim()
                    End If
                    Return dt
                End Using
            End Using
        End Function

        Public Function Save(Optional ByVal id As Integer = 0) As Integer Implements IAccess.Save
            Dim sql = IIf(id = 0, String.Format("select * from t_promotion where 1=2"), _
               String.Format("select * from t_promotion where promotionid in ({0})", id))

            Using cnn = New SqlConnection(Resources.Resource.cns)
                Using ada = New SqlDataAdapter(sql, cnn)

                    Dim cbd = New SqlCommandBuilder(ada)
                    Dim dt = New DataTable()
                    ada.FillSchema(dt, SchemaType.Source)
                    ada.Fill(dt)

                    Dim r As DataRow = Nothing

                    If id = 0 Then
                        r = dt.NewRow()
                        r.SetField("DateCreated", DateTime.Now)
                    Else
                        r = dt.Rows(0)
                    End If

                    r.SetField("Name", Name)
                    r.SetField("Price", Price)
                    r.SetField("BookingDateFrom", BookFrom)
                    r.SetField("BookingDateTo", BookTo)
                    r.SetField("CheckingDateFrom", CheckinFrom)
                    r.SetField("CheckingDateTo", CheckinTo)
                    r.SetField("code", Code)

                    If id = 0 Then
                        dt.Rows.Add(r)
                    End If
                    ada.Update(dt)

                End Using
            End Using
            Return 0

        End Function
    End Class

    Private Class Content
        Implements IAccess

        Private _unit As String
        Private _inventory As String
        Private _size As Integer
        Private _nights As Integer
        Private _upgradeallowed As Boolean
        Private _promotionid As Integer
        Private _promotioncontentid As Integer

        Public Sub New(ByVal id As Integer)
            _promotionid = id
        End Sub


        Public Function List(Optional ByVal id As Integer = 0) As System.Data.DataTable Implements IAccess.List

            Dim sql = IIf(id = 0, String.Format("select promotionContentID, Size, Nights, UpgradeAllowed, PromotionID, " & _
                                                "case unitTypeId " & _
                                                "		when 969 then 'Cottage' " & _
                                                "		when 970 then 'Townes' " & _
                                                "		when 17264 then 'Estates' " & _
                                                "		end Unit, " & _
                                                "case inventoryid " & _
                                                "	    when  987 then 'Rental' " & _
                                                "	    when 17623 then 'Marketing' " & _
                                                "end Inventory from t_promotioncontents where promotionid in ({0})", _promotionid), _
                          String.Format("select * from t_promotioncontents where promotioncontentid in ({0})", id))

            Using cnn = New SqlConnection(Resources.Resource.cns)
                Using ada = New SqlDataAdapter(sql, cnn)

                    Dim dt = New DataTable()
                    ada.Fill(dt)

                    If id > 0 Then

                        Dim r = dt.AsEnumerable().FirstOrDefault()

                        If r.Equals(Nothing) = False Then
                            Inventory = r("inventoryid")
                            Unit = r("unittypeid")
                            PromotionContentId = r("promotioncontentid")
                            PromotionId = r("promotionid")
                            Size = r("size")
                            Nights = r("nights")
                            UpgradeAllowed = r("UpgradeAllowed")
                        End If
                    End If
                    Return dt
                End Using
            End Using
        End Function

        Public Function Save(Optional ByVal id As Integer = 0) As Integer Implements IAccess.Save

            Dim sql = IIf(id = 0, String.Format("select * from t_promotioncontents where 1=2"), _
                                String.Format("select * from t_promotioncontents where promotioncontentid = {0}", id))

            Using cnn = New SqlConnection(Resources.Resource.cns)
                Using ada = New SqlDataAdapter(sql, cnn)

                    Dim cbd = New SqlCommandBuilder(ada)
                    Dim dt = New DataTable()
                    Dim dr As DataRow = Nothing

                    ada.Fill(dt)

                    If dt.Rows.Count = 0 Then
                        dr = dt.NewRow()
                    Else
                        dr = dt.Rows(0)
                    End If

                    dr("Inventoryid") = Inventory
                    dr("unittypeid") = Unit
                    dr("size") = Size
                    dr("nights") = Nights
                    dr("upgradeAllowed") = UpgradeAllowed
                    dr("promotionid") = PromotionId

                    If id = 0 Then
                        dt.Rows.Add(dr)
                    End If

                    ada.Update(dt)

                    Return 0
                End Using
            End Using
        End Function


        Public Property Unit As String
            Get
                Return _unit
            End Get
            Set(ByVal value As String)
                _unit = value
            End Set
        End Property

        Public Property Inventory As String
            Get
                Return _inventory
            End Get
            Set(ByVal value As String)
                _inventory = value
            End Set
        End Property

        Public Property Size As Integer
            Get
                Return _size
            End Get
            Set(ByVal value As Integer)
                _size = value
            End Set
        End Property

        Public Property Nights As Integer
            Get
                Return _nights
            End Get
            Set(ByVal value As Integer)
                _nights = value
            End Set
        End Property

        Public Property UpgradeAllowed As Boolean
            Get
                Return _upgradeallowed
            End Get
            Set(ByVal value As Boolean)
                _upgradeallowed = value
            End Set
        End Property

        Public Property PromotionContentId As Integer
            Get
                Return _promotioncontentid
            End Get
            Set(value As Integer)
                _promotioncontentid = value
            End Set
        End Property

        Public Property PromotionId As Integer
            Get
                Return _promotionid
            End Get
            Set(value As Integer)
                _promotionid = value
            End Set
        End Property

        Public ReadOnly Property UnitDataSource As DataTable
            Get
                Dim _tableUnit = New DataTable()
                _tableUnit.Columns.AddRange(New DataColumn() {New DataColumn("id", GetType(Integer)), New DataColumn("name", GetType(String))})
                _tableUnit.PrimaryKey = New DataColumn() {_tableUnit.Columns(0)}

                Dim dr As DataRow = _tableUnit.NewRow()

                dr.SetField("id", 0)
                dr.SetField("name", String.Empty)

                _tableUnit.Rows.Add(dr)
                dr = _tableUnit.NewRow()

                dr.SetField("id", 969)
                dr.SetField("name", "Cottage")

                _tableUnit.Rows.Add(dr)
                dr = _tableUnit.NewRow()

                dr.SetField("id", 970)
                dr.SetField("name", "Townes")

                _tableUnit.Rows.Add(dr)
                dr = _tableUnit.NewRow()

                dr.SetField("id", 17264)
                dr.SetField("name", "Estates")

                _tableUnit.Rows.Add(dr)

                Return _tableUnit
            End Get
        End Property

        Public ReadOnly Property InventoryDataSource As DataTable
            Get
                Dim _tableInventory = New DataTable()

                _tableInventory.Columns.AddRange(New DataColumn() {New DataColumn("id", GetType(Integer)), New DataColumn("name", GetType(String))})
                _tableInventory.PrimaryKey = New DataColumn() {_tableInventory.Columns(0)}

                Dim dr As DataRow = _tableInventory.NewRow()

                dr = _tableInventory.NewRow()

                dr.SetField("id", 0)
                dr.SetField("name", String.Empty)

                _tableInventory.Rows.Add(dr)
                dr = _tableInventory.NewRow()

                dr.SetField("id", 987)
                dr.SetField("name", "Rental")

                _tableInventory.Rows.Add(dr)
                dr = _tableInventory.NewRow()

                dr.SetField("id", 17623)
                dr.SetField("name", "Marketing")

                _tableInventory.Rows.Add(dr)

                Return _tableInventory
            End Get
        End Property
    End Class



    Private Class Premiums

        Private _table As DataTable

        Public Sub New()
            _table = New DataTable()

            Dim dc = _table.Columns.Add("PremiumID", GetType(Int32))
            _table.Columns.Add("Name", GetType(String))
            _table.Columns.Add("Value", GetType(Decimal))
            _table.Columns.Add("Quantity", GetType(Int16))

            _table.PrimaryKey = New DataColumn() {dc}

        End Sub

        Public Function Add(id As Integer, name As String, value As Decimal, qty As Int16) As Boolean

            If Find(id) = True Or Find(name.Trim()) = True Or qty <= 0 Then
                Return False
            Else
                Dim dr = _table.NewRow()
                dr.SetField("PremiumID", id)
                dr.SetField("Name", name)
                dr.SetField("Value", value)
                dr.SetField("Quantity", qty)
                _table.Rows.Add(dr)

                Return True
            End If
        End Function

        Public Function Remove(id As Integer) As Boolean

            Dim dr = _table.Rows.Find(id)
            If dr Is Nothing Then
                Return False
            Else
                _table.Rows.Remove(dr)
                Return True
            End If
        End Function

        Public ReadOnly Property Count As Int16
            Get
                Return _table.Rows.Count
            End Get
        End Property

        Public Function Current(id As Integer) As String()
            Dim dr = _table.Rows.Find(id)
            If dr Is Nothing Then
                Return Nothing
            Else
                Return New String() {dr.Field(Of Integer)("PremiumID"), dr.Field(Of String)("Name"), dr.Field(Of Decimal)("Value"), dr.Field(Of Int16)("Quantity")}
            End If
        End Function

        Public Function Find(id As Integer) As Boolean
            Dim dr = _table.Rows.Find(id)
            Return IIf(dr Is Nothing, False, True)
        End Function

        Public Function Find(name As String) As Boolean
            Dim f = _table.AsEnumerable().SingleOrDefault(Function(x) x.Field(Of String)("Name").Equals(name.Trim()))
            Return IIf(f Is Nothing, False, True)
        End Function
    End Class



    Private Class Campaigns
        Private _table As DataTable

        Public Sub New()
            _table = New DataTable
            Dim dc = _table.Columns.Add("CampaignID", GetType(Int32))
            _table.Columns.Add("Name", GetType(String))

            _table.PrimaryKey = New DataColumn() {dc}
        End Sub

        Public Function Add(id As Integer, name As String) As Boolean

            If Find(id) Or Find(name) Then
                Return False
            Else

                Dim dr = _table.NewRow()
                dr.SetField("CampaignID", id)
                dr.SetField("Name", name)

                _table.Rows.Add(dr)
                Return True
            End If

        End Function

        Public Function Current(id As Integer) As String()
            Dim dr = _table.Rows.Find(id)
            If dr Is Nothing Then
                Return Nothing
            Else
                Return New String() {dr.Field(Of Integer)("CampaignID"), dr.Field(Of String)("Name")}
            End If
        End Function

        Public Function Remove(id As Integer) As Boolean

            Dim dr = _table.Rows.Find(id)
            If dr Is Nothing Then
                Return False
            Else
                _table.Rows.Remove(dr)
                Return True
            End If
        End Function

        Public ReadOnly Property Count As Int16
            Get
                Return _table.Rows.Count
            End Get
        End Property
        Public Function Find(id As Integer) As Boolean
            Dim dr = _table.Rows.Find(id)
            Return IIf(dr Is Nothing, False, True)
        End Function

        Public Function Find(name As String) As Boolean
            Dim f = _table.AsEnumerable().SingleOrDefault(Function(x) x.Field(Of String)("Name").Equals(name.Trim()))
            Return IIf(f Is Nothing, False, True)
        End Function
    End Class


    Private Class KeyValue
        Private _id As Integer
        Private _name As String

        Public Property ID As Integer
            Get
                Return _id
            End Get
            Set(value As Integer)
                _id = value
            End Set
        End Property

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

    End Class
    Private Class States

        Private _table As DataTable

        Public Sub New()
            _table = New DataTable()

            Dim dc = _table.Columns.Add("StateID", GetType(Int32))
            _table.Columns.Add("Name", GetType(String))
            _table.Columns.Add("Checked", GetType(Boolean))

            _table.PrimaryKey = New DataColumn() {dc}
        End Sub

        Public Function Add(id As Integer, name As String, checked As Boolean) As Boolean

            If Find(id) Or Find(name) Then
                Return False
            Else

                Dim dr = _table.NewRow()
                dr.SetField("StateID", id)
                dr.SetField("Name", name)
                dr.SetField("Checked", checked)

                _table.Rows.Add(dr)
                Return True
            End If
        End Function

        Public Function Current(id As Integer) As String()
            Dim dr = _table.Rows.Find(id)
            If dr Is Nothing Then
                Return Nothing
            Else
                Return New String() {dr.Field(Of Integer)("StateID"), dr.Field(Of String)("Name")}
            End If
        End Function
        Public ReadOnly Property Count As Int16
            Get
                Return _table.Rows.Count
            End Get
        End Property
        Public Function Find(id As Integer) As Boolean
            Dim dr = _table.Rows.Find(id)
            Return IIf(dr Is Nothing, False, True)
        End Function

        Public Function Find(name As String) As Boolean
            Dim f = _table.AsEnumerable().SingleOrDefault(Function(x) x.Field(Of String)("Name").Equals(name.Trim()))
            Return IIf(f Is Nothing, False, True)
        End Function

        Public Function Find(checked As Boolean) As List(Of KeyValue)

            Return _table.AsEnumerable().Where(Function(x) x.Field(Of Boolean)("Checked") = checked).Select(Function(x) New KeyValue With { _
                                                                                                                .ID = x.Field(Of Integer)("StateID"), _
                                                                                                                .Name = x.Field(Of String)("Name")}).ToList()
        End Function
    End Class

    Private Class Products

        Private _table As DataTable

        Public Sub New()
            _table = New DataTable()
            Dim dc = _table.Columns.Add("id", GetType(Int32))
            _table.Columns.Add("Unit", GetType(String))
            _table.Columns.Add("Size", GetType(Int16))

            _table.PrimaryKey = New DataColumn() {dc}
        End Sub


        Public Function Add(id As Integer, unit As String, size As Int16) As Boolean

            If Find(size, unit) Then
                Return False
            Else

                Dim dr = _table.NewRow()
                dr.SetField("id", id)
                dr.SetField("Unit", unit)
                dr.SetField("Size", size)

                _table.Rows.Add(dr)
                Return True
            End If
        End Function

        Public Function Find(id As Integer) As Boolean
            Dim dr = _table.Rows.Find(id)
            Return IIf(dr Is Nothing, False, True)
        End Function

        Public Function Find(size As Int16, unit As String) As Boolean
            Dim f = _table.AsEnumerable().Where(Function(x) x.Field(Of Int16)("Size").CompareTo(size) And x.Field(Of String)("Unit").CompareTo(unit))
            Return IIf(f.Count() = 0, False, True)
        End Function

        Public ReadOnly Property Distinct As String()
            Get
                Return _table.AsEnumerable().Select(Function(x) x.Field(Of String)("Unit")).Distinct().ToArray()
            End Get
        End Property
    End Class



    Private Class Requirements

        Private _table As DataTable

        Private _ages As String
        Private _income As Decimal
        Private _maritalDic As IDictionary(Of Integer, String)
        Private _liveinDic As IDictionary(Of Integer, String)
        Private _liveotDic As IDictionary(Of Integer, String)
        
        Public Sub New()
            _table = New DataTable()

            Dim dc = _table.Columns.Add("id", GetType(Int32))
            _table.Columns.Add("Ages", GetType(String))
            _table.Columns.Add("Income", GetType(Decimal))
            _table.Columns.Add("MaritalStatus", GetType(String))
            _table.Columns.Add("LiveIn", GetType(String))
            _table.Columns.Add("LiveOut", GetType(String))

            _table.PrimaryKey = New DataColumn() {dc}

            _maritalDic = New Dictionary(Of Integer, String)()
            _maritalDic.Add(16657, "Co-Habitant")
            _maritalDic.Add(16658, "Divorced")
            _maritalDic.Add(16659, "Married")
            _maritalDic.Add(16660, "Partner")
            _maritalDic.Add(16661, "Separated")
            _maritalDic.Add(16663, "Widowed")
            _maritalDic.Add(16662, "Single")

            _liveinDic = New Dictionary(Of Integer, String)()
            _liveotDic = New Dictionary(Of Integer, String)()

        End Sub

        Public Property Ages As String
            Get
                Return _table.Rows(0).Field(Of String)("Ages")
            End Get
            Set(value As String)
                _table.Rows(0).SetField("Ages", value)
            End Set
        End Property

        Public Property Income As Decimal
            Get
                Return _table.Rows(0).Field(Of Decimal)("Income")
            End Get
            Set(value As Decimal)
                _table.Rows(0).SetField("Income", value)
            End Set
        End Property

        Public WriteOnly Property LiveInStates As IDictionary(Of Integer, String)
            Set(value As IDictionary(Of Integer, String))
                _liveinDic = value
                _liveotDic = New Dictionary(Of Integer, String)()
            End Set
        End Property

        Public WriteOnly Property LiveNotInStates As IDictionary(Of Integer, String)
            Set(value As IDictionary(Of Integer, String))
                _liveotDic = value
                _liveinDic = New Dictionary(Of Integer, String)()
            End Set
        End Property

        Public Property MaritalStatus As List(Of KeyValue)
            Get
                Dim l As New List(Of KeyValue)
                Dim arr As String() = _table.Rows(0).Field(Of String)("MaritalStatus").Split(New Char() {","}).ToArray()
                For Each s In arr
                    Dim kv As New KeyValue With {.ID = Integer.Parse(s), .Name = _maritalDic(Integer.Parse(s))}
                    l.Add(kv)
                Next
                Return l
            End Get
            Set(value As List(Of KeyValue))
                Dim v = value.Select(Function(x) x.ID.ToString()).ToArray()
                _table.Rows(0).SetField("MaritalStatus", String.Join(",", v))
            End Set
        End Property

        Public Property LiveIn As List(Of KeyValue)
            Get
                Dim l As New List(Of KeyValue)
                Dim arr As String() = _table.Rows(0).Field(Of String)("LiveIn").Split(New Char() {","}).ToArray()
                For Each s In arr
                    Dim kv As New KeyValue With {.ID = Integer.Parse(s), .Name = _liveinDic(Integer.Parse(s))}
                    l.Add(kv)
                Next
                Return l
            End Get
            Set(value As List(Of KeyValue))
                Dim v = value.Select(Function(x) x.ID.ToString()).ToArray()
                _table.Rows(0).SetField("LiveIn", String.Join(",", v))
            End Set
        End Property

        Public Property LiveOut As List(Of KeyValue)
            Get
                Dim l As New List(Of KeyValue)
                Dim arr As String() = _table.Rows(0).Field(Of String)("LiveOut").Split(New Char() {","}).ToArray()
                For Each s In arr
                    Dim kv As New KeyValue With {.ID = Integer.Parse(s), .Name = _liveotDic(Integer.Parse(s))}
                    l.Add(kv)
                Next
                Return l
            End Get
            Set(value As List(Of KeyValue))
                Dim v = value.Select(Function(x) x.ID.ToString()).ToArray()
                _table.Rows(0).SetField("LiveOut", String.Join(",", v))
            End Set
        End Property



        Public Function ExceedIncome(income As Decimal) As Boolean
            Return IIf(_table.Rows(0).Field(Of Decimal)("Income") < income, False, True)
        End Function

        Public Function ResidencyMet(state As String) As Boolean

            If LiveIn.Count > 0 Then
                Dim v = LiveIn.FirstOrDefault(Function(x) x.Name.Trim().CompareTo(state) = 0)
                Return IIf(String.IsNullOrEmpty(v.Name), False, True)
            ElseIf LiveOut.Count > 0 Then
                Dim v = LiveOut.FirstOrDefault(Function(x) x.Name.Trim().CompareTo(state) = 0)
                Return IIf(String.IsNullOrEmpty(v.Name), False, True)
            Else
                Return False
            End If
            Return False
        End Function

        Public Function AgeMet(age As Int16) As Boolean

            Dim idx = Ages.IndexOf("-")
            If idx < 0 Then
                Return False
            Else
                Dim min = Ages.Substring(0, idx - 1)
                Dim max = Ages.Substring(idx + 1, Ages.Length - 1)
                Return IIf(Integer.Parse(min) >= age And Integer.Parse(max) <= age, True, False)
            End If
        End Function


    End Class

    Private Class Offer
        Private _price As Decimal
        Private _nights As Int16
        Private _promocode As String
        Private _source As String
        Private _in As Int16
        Private _kcpStatus As String

        Private _campaigns As List(Of Campaigns)
        Private _premiums As List(Of Premiums)
        Private _products As List(Of Products)

        Public Property Price As Decimal
            Get
                Return _price
            End Get
            Set(value As Decimal)
                _price = value
            End Set
        End Property

        Public Property Nights As Int16
            Get
                Return _nights
            End Get
            Set(value As Int16)
                _nights = value
            End Set
        End Property

        Public Property Promocode As String
            Get
                Return _promocode
            End Get
            Set(value As String)
                _promocode = value
            End Set
        End Property

        Public Property Source As String
            Get
                Return _source
            End Get
            Set(value As String)
                _source = value
            End Set
        End Property

        Public Property InState As Int16
            Get
                Return _in
            End Get
            Set(value As Int16)
                _in = value
            End Set
        End Property

        Public Property KcpStatus As String
            Get
                Return _kcpStatus
            End Get
            Set(value As String)
                _kcpStatus = value
            End Set
        End Property





    End Class

    Protected Sub btn_premium_add_Click(sender As Object, e As System.EventArgs) Handles btn_premium_add.Click

        If lsb_premium_2.Items.Count = 0 Then Return
        Dim li = lsb_premium_2.Items.OfType(Of ListItem).Select(Function(x) x.Value).ToArray()

        lsb_premium_3.Items.Add(String.Join(";", li))
        lsb_premium_2.Items.Clear()

        Dim dic As New Dictionary(Of Integer, String)

        For Each item As ListItem In lsb_premium_1.Items
            dic.Add(Integer.Parse(item.Value), item.Text)
        Next

        Dim sb = New StringBuilder()
        sb.AppendFormat("<ul>")
        For Each item As ListItem In lsb_premium_3.Items
            If item.Text.IndexOf(";") > 0 Then
                Dim ar() = item.Text.Split(";")
                Dim tmp = String.Empty
                For i = 0 To ar.Length - 1
                    If i = 0 Then
                        Dim arr() = ar(i).Split("-")
                        tmp = String.Format("<strong>{0} {1}</strong>", arr(0), dic(arr(1)))
                    Else
                        Dim arr() = ar(i).Split("-")
                        tmp += String.Format(" and <strong>{0} {1}</strong>", arr(0), dic(arr(1)))
                    End If
                Next
                
                sb.AppendFormat("<li>{0}</li>", tmp)
            Else
                Dim arr() = item.Text.Split("-")
                sb.AppendFormat("<li><strong>{0} {1}</strong></li>", arr(0), dic(arr(1)))
            End If
        Next
        sb.AppendFormat("</ul>")
        lit_premiums.Text = sb.ToString()
    End Sub

    Protected Sub btn_transfer_to_Click(sender As Object, e As System.EventArgs) Handles btn_transfer_to.Click

        Dim ch = lsb_premium_1.Items.OfType(Of ListItem).FirstOrDefault(Function(x) x.Selected)
        If ch IsNot Nothing Then
            Dim li As New ListItem(String.Format("{0}- {1}", ddl_premiums_qty.SelectedItem.Text, ch.Text), String.Format("{0}-{1}", ddl_premiums_qty.SelectedItem.Text, ch.Value))
            lsb_premium_2.Items.Add(li)
        End If

        lsb_premium_1.SelectedIndex = -1
        lsb_premium_2.SelectedIndex = -1
        ddl_premiums_qty.SelectedIndex = 0
    End Sub

    Protected Sub btn_inventory_add_Click(sender As Object, e As System.EventArgs) Handles btn_inventory_add.Click
        Dim sb = New StringBuilder()
        Dim tm1 = String.Empty, tm2 = String.Empty

        For Each item As ListItem In cbl_reservation_types.Items.OfType(Of ListItem).Where(Function(x) x.Selected)
            tm1 += IIf(String.IsNullOrEmpty(tm1), item.Value, String.Format(";{0}", item.Value))
        Next

        For Each item As ListItem In lsb_inventory_1.Items.OfType(Of ListItem).Where(Function(x) x.Selected)
            tm2 += IIf(String.IsNullOrEmpty(tm2), item.Text, String.Format(";{0}", item.Text))
        Next

        
        Dim value = String.Format("{0}-{1}-{2}-{3}", ddl_promo_nights.SelectedItem.Text, _
                                tbx_inventory_price.Text, _
                                tm1, _
                                tm2)

        hfd_inventory_choices.Value += IIf(String.IsNullOrEmpty(hfd_inventory_choices.Value), value, String.Format("|{0}", value))

        lit_inventory_choices.Text = hfd_inventory_choices.Value
    End Sub
End Class
