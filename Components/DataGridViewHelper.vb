Imports System.Runtime.CompilerServices
Imports System.Windows.Forms

Module DataGridViewHelper
    <Extension()>
    Function SingleHorizontalBorderAdded(ByVal dataGridView As DataGridView) As Boolean
        Return Not dataGridView.ColumnHeadersVisible AndAlso
            (dataGridView.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single OrElse
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal)
    End Function

    <Extension()>
    Function SingleVerticalBorderAdded(ByVal dataGridView As DataGridView) As Boolean
        Return Not dataGridView.RowHeadersVisible AndAlso
            (dataGridView.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single OrElse
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical)
    End Function
End Module
