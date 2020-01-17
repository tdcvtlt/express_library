Imports System.Data.Linq


Public Class SqlComboItemsRepository
    Implements ICombo

    Private comboitem As System.Data.Linq.Table(Of ComboItems)
    Private combos As System.Data.Linq.Table(Of Combos)
    Private items As IQueryable(Of ComboItem)
    Private cnx As String = Resources.Resource.cns

    Public Sub New()
        Dim dc As New DataContext(cnx)

        items = From c In dc.GetTable(Of Combos)() _
                Join b In dc.GetTable(Of ComboItems)() _
                On c.ComboID Equals b.ComboID _
                Select New ComboItem With { _
                    .Active = b.Active, _
                    .ComboID = c.ComboID, _
                    .ComboItem = b.ComboItem, _
                    .ComboItemID = b.ComboItemID, _
                    .ComboName = c.ComboName, _
                    .Description = c.Description, _
                    .Locationid = b.LocationID _
                }

        '-----------------------------
        '
        '
        comboitem = dc.GetTable(Of ComboItems)()
        combos = dc.GetTable(Of Combos)()

    End Sub

    ReadOnly Property GetComboItems As IQueryable(Of ComboItem) Implements ICombo.GetDataComboItem
        Get
            Return items
        End Get
    End Property
End Class
