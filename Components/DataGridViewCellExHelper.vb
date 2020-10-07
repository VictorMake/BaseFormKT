Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms

Module DataGridViewCellExHelper
    Function GetSpannedCellClipBounds(Of TCell As {DataGridViewCell, ISpannedCell})(ByVal ownerCell As TCell,
                                                                                    ByVal cellBounds As Rectangle,
                                                                                    ByVal singleVerticalBorderAdded As Boolean,
                                                                                    ByVal singleHorizontalBorderAdded As Boolean) As Rectangle
        Dim dataGridView = ownerCell.DataGridView
        Dim clipBounds = cellBounds

        ' Setting X (skip invisible columns).
        For Each columnIndex In Enumerable.Range(ownerCell.ColumnIndex, ownerCell.ColumnSpan)
            Dim column = dataGridView.Columns(columnIndex)

            If Not column.Visible Then Continue For

            If column.Frozen OrElse columnIndex > dataGridView.FirstDisplayedScrollingColumnIndex Then
                Exit For
            End If

            If columnIndex = dataGridView.FirstDisplayedScrollingColumnIndex Then
                clipBounds.Width -= dataGridView.FirstDisplayedScrollingColumnHiddenWidth

                If dataGridView.RightToLeft <> RightToLeft.Yes Then
                    clipBounds.X += dataGridView.FirstDisplayedScrollingColumnHiddenWidth
                End If

                Exit For
            End If

            clipBounds.Width -= column.Width

            If dataGridView.RightToLeft <> RightToLeft.Yes Then
                clipBounds.X += column.Width
            End If
        Next

        ' Setting Y.
        For Each rowIndex In Enumerable.Range(ownerCell.RowIndex, ownerCell.RowSpan)
            Dim row = dataGridView.Rows(rowIndex)

            If Not row.Visible Then Continue For

            If row.Frozen OrElse rowIndex >= dataGridView.FirstDisplayedScrollingRowIndex Then
                Exit For
            End If

            clipBounds.Y += row.Height
            clipBounds.Height -= row.Height
        Next

        ' exclude borders.
        If dataGridView.BorderStyle <> BorderStyle.None Then
            Dim clientRectangle = dataGridView.ClientRectangle
            clientRectangle.Width -= 1
            clientRectangle.Height -= 1

            If dataGridView.RightToLeft = RightToLeft.Yes Then
                clientRectangle.X += 1
                clientRectangle.Y += 1
            End If

            clipBounds.Intersect(clientRectangle)
        End If

        Return clipBounds
    End Function

    Function GetSpannedCellBoundsFromChildCellBounds(Of TCell As {DataGridViewCell, ISpannedCell})(ByVal childCell As TCell,
                                                                                                   ByVal childCellBounds As Rectangle,
                                                                                                   ByVal singleVerticalBorderAdded As Boolean,
                                                                                                   ByVal singleHorizontalBorderAdded As Boolean) As Rectangle
        Dim dataGridView = childCell.DataGridView
        Dim ownerCell = If(TryCast(childCell.OwnerCell, TCell), childCell)
        Dim spannedCellBounds = childCellBounds
        Dim firstVisibleColumnIndex = Enumerable.Range(ownerCell.ColumnIndex, ownerCell.ColumnSpan).First(Function(i) dataGridView.Columns(i).Visible)

        If dataGridView.Columns(firstVisibleColumnIndex).Frozen Then
            spannedCellBounds.X = dataGridView.GetColumnDisplayRectangle(firstVisibleColumnIndex, False).X
        Else
            Dim dx = Enumerable.Range(firstVisibleColumnIndex, childCell.ColumnIndex - firstVisibleColumnIndex).
                [Select](Function(i) dataGridView.Columns(i)).
                Where(Function(columnItem) columnItem.Visible).
                Sum(Function(columnItem) columnItem.Width)

            spannedCellBounds.X = If(dataGridView.RightToLeft = RightToLeft.Yes, spannedCellBounds.X + dx, spannedCellBounds.X - dx)
        End If

        Dim firstVisibleRowIndex = Enumerable.Range(ownerCell.RowIndex, ownerCell.RowSpan).First(Function(i) dataGridView.Rows(i).Visible)

        If dataGridView.Rows(firstVisibleRowIndex).Frozen Then
            spannedCellBounds.Y = dataGridView.GetRowDisplayRectangle(firstVisibleRowIndex, False).Y
        Else
            spannedCellBounds.Y -= Enumerable.Range(firstVisibleRowIndex, childCell.RowIndex - firstVisibleRowIndex).
                [Select](Function(i) dataGridView.Rows(i)).
                Where(Function(rowItem) rowItem.Visible).
                Sum(Function(rowItem) rowItem.Height)
        End If

        Dim spannedCellWidth = Enumerable.Range(ownerCell.ColumnIndex, ownerCell.ColumnSpan).
            [Select](Function(columnIndex) dataGridView.Columns(columnIndex)).
            Where(Function(column) column.Visible).
            Sum(Function(column) column.Width)

        If dataGridView.RightToLeft = RightToLeft.Yes Then
            spannedCellBounds.X = spannedCellBounds.Right - spannedCellWidth
        End If

        spannedCellBounds.Width = spannedCellWidth
        spannedCellBounds.Height = Enumerable.Range(ownerCell.RowIndex, ownerCell.RowSpan).
            [Select](Function(rowIndex) dataGridView.Rows(rowIndex)).
            Where(Function(row) row.Visible).Sum(Function(row) row.Height)

        If singleVerticalBorderAdded AndAlso InFirstDisplayedColumn(ownerCell) Then
            spannedCellBounds.Width += 1

            If dataGridView.RightToLeft <> RightToLeft.Yes Then

                If childCell.ColumnIndex <> dataGridView.FirstDisplayedScrollingColumnIndex Then
                    spannedCellBounds.X -= 1
                End If
            Else

                If childCell.ColumnIndex = dataGridView.FirstDisplayedScrollingColumnIndex Then
                    spannedCellBounds.X -= 1
                End If
            End If
        End If

        If singleHorizontalBorderAdded AndAlso InFirstDisplayedRow(ownerCell) Then
            spannedCellBounds.Height += 1

            If childCell.RowIndex <> dataGridView.FirstDisplayedScrollingRowIndex Then
                spannedCellBounds.Y -= 1
            End If
        End If

        Return spannedCellBounds
    End Function

    Function AdjustCellBorderStyle(Of TCell As {DataGridViewCell, ISpannedCell})(ByVal cell As TCell) As DataGridViewAdvancedBorderStyle
        Dim dataGridViewAdvancedBorderStylePlaceholder = New DataGridViewAdvancedBorderStyle()
        Dim dataGridView = cell.DataGridView

        Return cell.AdjustCellBorderStyle(dataGridView.AdvancedCellBorderStyle,
                                          dataGridViewAdvancedBorderStylePlaceholder,
                                          dataGridView.SingleVerticalBorderAdded(),
                                          dataGridView.SingleHorizontalBorderAdded(),
                                          InFirstDisplayedColumn(cell),
                                          InFirstDisplayedRow(cell))
    End Function

    <Extension()>
    Function InFirstDisplayedColumn(Of TCell As {DataGridViewCell, ISpannedCell})(ByVal cell As TCell) As Boolean
        Dim dataGridView = cell.DataGridView
        Return dataGridView.FirstDisplayedScrollingColumnIndex >= cell.ColumnIndex AndAlso
            dataGridView.FirstDisplayedScrollingColumnIndex < cell.ColumnIndex + cell.ColumnSpan
    End Function

    <Extension()>
    Function InFirstDisplayedRow(Of TCell As {DataGridViewCell, ISpannedCell})(ByVal cell As TCell) As Boolean
        Dim dataGridView = cell.DataGridView
        Return dataGridView.FirstDisplayedScrollingRowIndex >= cell.RowIndex AndAlso
            dataGridView.FirstDisplayedScrollingRowIndex < cell.RowIndex + cell.RowSpan
    End Function
End Module
