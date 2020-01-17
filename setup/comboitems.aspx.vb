
Partial Class setup_comboitems
    Inherits System.Web.UI.Page
    Dim sSelected As String = ""
    Dim sSelectedParent As String = ""

    Protected Sub gvCombos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCombos.SelectedIndexChanged
        Dim dbSrc As New SqlDataSource
        dbSrc.ConnectionString = Resources.Resource.cns ' ConfigurationManager.ConnectionStrings("NorthwindConnectionString2").ConnectionString
        Dim row As GridViewRow = gvCombos.SelectedRow
        dbSrc.SelectCommand = "SELECT ComboItemID as ID, ComboItem as Item, Description, Active FROM  t_Comboitems where comboid=" & row.Cells(2).Text & " order by ComboItem"
        sSelected = row.Cells(3).Text
        sSelectedParent = row.Cells(2).Text

        gvItems.DataSource = dbSrc
        gvItems.DataBind()
    End Sub

    Public ReadOnly Property SelectedText As String
        Get
            Return sSelected
        End Get
    End Property

    Public ReadOnly Property SelectedParent As String
        Get
            Return sSelectedParent
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim dbSrc As New SqlDataSource
            dbSrc.ConnectionString = Resources.Resource.cns ' ConfigurationManager.ConnectionStrings("NorthwindConnectionString2").ConnectionString
            dbSrc.SelectCommand = "SELECT ComboID as ID, ComboName, Description FROM  t_Combos order by Comboname"
            Dim kf(0) As String
            kf(0) = "ID"
            gvCombos.DataKeyNames = kf
            gvCombos.DataSource = dbSrc
            gvCombos.DataBind()
        End If
    End Sub
End Class
