Imports System.Data.Linq


Public Interface ICombo

    ReadOnly Property GetDataComboItem As IQueryable(Of ComboItem)
End Interface


