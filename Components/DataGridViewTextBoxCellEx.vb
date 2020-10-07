'Imports SpannedDataGridView

Imports System.Drawing
Imports System.Windows.Forms

Public Class DataGridViewTextBoxCellEx
    Inherits DataGridViewTextBoxCell
    Implements ISpannedCell

#Region "Fields"
    Private m_ColumnSpan As Integer = 1
    Private m_RowSpan As Integer = 1
    Private m_OwnerCell As DataGridViewTextBoxCellEx
#End Region

#Region "Properties"
    Public Property ColumnSpan As Integer Implements ISpannedCell.ColumnSpan
        Get
            Return m_ColumnSpan
        End Get
        Set(ByVal value As Integer)
            If DataGridView Is Nothing OrElse m_OwnerCell IsNot Nothing Then Return
            If value < 1 OrElse ColumnIndex + value - 1 >= DataGridView.ColumnCount Then Throw New ArgumentOutOfRangeException("value")
            If m_ColumnSpan <> value Then SetSpan(value, m_RowSpan)
        End Set
    End Property

    Public Property RowSpan As Integer Implements ISpannedCell.RowSpan
        Get
            Return m_RowSpan
        End Get
        Set(ByVal value As Integer)
            If DataGridView Is Nothing OrElse m_OwnerCell IsNot Nothing Then Return
            If value < 1 OrElse RowIndex + value - 1 >= DataGridView.RowCount Then Throw New ArgumentOutOfRangeException("value")
            If m_RowSpan <> value Then SetSpan(m_ColumnSpan, value)
        End Set
    End Property

    Public Property OwnerCell As DataGridViewCell Implements ISpannedCell.OwnerCell
        Get
            Return m_OwnerCell
        End Get
        Private Set(ByVal value As DataGridViewCell)
            m_OwnerCell = TryCast(value, DataGridViewTextBoxCellEx)
        End Set
    End Property

    Public Overrides Property [ReadOnly] As Boolean
        Get
            Return MyBase.ReadOnly
        End Get
        Set(ByVal value As Boolean)
            MyBase.ReadOnly = value

            If m_OwnerCell Is Nothing AndAlso (m_ColumnSpan > 1 OrElse m_RowSpan > 1) AndAlso DataGridView IsNot Nothing Then
                For Each col In Enumerable.Range(ColumnIndex, m_ColumnSpan)
                    For Each row In Enumerable.Range(RowIndex, m_RowSpan)
                        If col <> ColumnIndex OrElse row <> RowIndex Then
                            DataGridView(col, row).[ReadOnly] = value
                        End If
                    Next
                Next
            End If
        End Set
    End Property
#End Region

#Region "Painting"
    Protected Overrides Sub Paint(ByVal graphics As Graphics, ByVal clipBounds As Rectangle, ByVal cellBounds As Rectangle, ByVal rowIndex As Integer, ByVal cellState As DataGridViewElementStates, ByVal value As Object, ByVal formattedValue As Object, ByVal errorText As String, ByVal cellStyle As DataGridViewCellStyle, ByVal advancedBorderStyle As DataGridViewAdvancedBorderStyle, ByVal paintParts As DataGridViewPaintParts)
        If m_OwnerCell IsNot Nothing AndAlso m_OwnerCell.DataGridView Is Nothing Then m_OwnerCell = Nothing

        If DataGridView Is Nothing OrElse (m_OwnerCell Is Nothing AndAlso m_ColumnSpan = 1 AndAlso m_RowSpan = 1) Then
            ' owner cell was removed.
            MyBase.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts)
            Return
        End If

        Dim ownerCell = Me
        Dim mcolumnIndex = ColumnIndex
        Dim columnSpan = m_ColumnSpan
        Dim rowSpan = m_RowSpan

        If m_OwnerCell IsNot Nothing Then
            ownerCell = m_OwnerCell
            mcolumnIndex = m_OwnerCell.ColumnIndex
            rowIndex = m_OwnerCell.RowIndex
            columnSpan = m_OwnerCell.ColumnSpan
            rowSpan = m_OwnerCell.RowSpan
            value = m_OwnerCell.GetValue(rowIndex)
            errorText = m_OwnerCell.GetErrorText(rowIndex)
            cellState = m_OwnerCell.State
            cellStyle = m_OwnerCell.GetInheritedStyle(Nothing, rowIndex, True)
            formattedValue = m_OwnerCell.GetFormattedValue(value, rowIndex, cellStyle, Nothing, Nothing, DataGridViewDataErrorContexts.Display)
        End If

        If CellsRegionContainsSelectedCell(mcolumnIndex, rowIndex, columnSpan, rowSpan) Then cellState = cellState Or DataGridViewElementStates.Selected
        Dim cellBounds2 = DataGridViewCellExHelper.GetSpannedCellBoundsFromChildCellBounds(Me, cellBounds, DataGridView.SingleVerticalBorderAdded(), DataGridView.SingleHorizontalBorderAdded())
        clipBounds = DataGridViewCellExHelper.GetSpannedCellClipBounds(ownerCell, cellBounds2, DataGridView.SingleVerticalBorderAdded(), DataGridView.SingleHorizontalBorderAdded())

        Using g = DataGridView.CreateGraphics()
            g.SetClip(clipBounds)
            ' Paint the content.
            advancedBorderStyle = DataGridViewCellExHelper.AdjustCellBorderStyle(ownerCell)
            ownerCell.NativePaint(g, clipBounds, cellBounds2, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts And Not DataGridViewPaintParts.Border)

            ' Paint the borders.
            If (paintParts And DataGridViewPaintParts.Border) <> DataGridViewPaintParts.None Then
                Dim leftTopCell = ownerCell
                Dim advancedBorderStyle2 = New DataGridViewAdvancedBorderStyle With {
                    .Left = advancedBorderStyle.Left,
                    .Top = advancedBorderStyle.Top,
                    .Right = DataGridViewAdvancedCellBorderStyle.None,
                    .Bottom = DataGridViewAdvancedCellBorderStyle.None
                }
                leftTopCell.PaintBorder(g, clipBounds, cellBounds2, cellStyle, advancedBorderStyle2)
                Dim rightBottomCell = If(TryCast(DataGridView(mcolumnIndex + columnSpan - 1, rowIndex + rowSpan - 1), DataGridViewTextBoxCellEx), Me)
                Dim advancedBorderStyle3 = New DataGridViewAdvancedBorderStyle With {
                    .Left = DataGridViewAdvancedCellBorderStyle.None,
                    .Top = DataGridViewAdvancedCellBorderStyle.None,
                    .Right = advancedBorderStyle.Right,
                    .Bottom = advancedBorderStyle.Bottom
                }
                rightBottomCell.PaintBorder(g, clipBounds, cellBounds2, cellStyle, advancedBorderStyle3)
            End If
        End Using
    End Sub

    Private Sub NativePaint(ByVal graphics As Graphics, ByVal clipBounds As Rectangle, ByVal cellBounds As Rectangle, ByVal rowIndex As Integer, ByVal cellState As DataGridViewElementStates, ByVal value As Object, ByVal formattedValue As Object, ByVal errorText As String, ByVal cellStyle As DataGridViewCellStyle, ByVal advancedBorderStyle As DataGridViewAdvancedBorderStyle, ByVal paintParts As DataGridViewPaintParts)
        MyBase.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts)
    End Sub
#End Region

#Region "Spanning."
    Private Sub SetSpan(ByVal columnSpan As Integer, ByVal rowSpan As Integer)
        Dim prevColumnSpan As Integer = m_ColumnSpan
        Dim prevRowSpan As Integer = m_RowSpan

        m_ColumnSpan = columnSpan
        m_RowSpan = rowSpan

        If DataGridView IsNot Nothing Then
            ' clear.
            For Each rowIndexRange As Integer In Enumerable.Range(RowIndex, prevRowSpan)
                For Each columnIndex As Integer In Enumerable.Range(columnIndex, prevColumnSpan)
                    Dim cell = TryCast(DataGridView(columnIndex, rowIndexRange), DataGridViewTextBoxCellEx)
                    If cell IsNot Nothing Then cell.OwnerCell = Nothing
                Next
            Next

            ' set.
            For Each rowIndexRange As Integer In Enumerable.Range(RowIndex, m_RowSpan)
                For Each columnIndexRange As Integer In Enumerable.Range(ColumnIndex, m_ColumnSpan)
                    Dim cell = TryCast(DataGridView(columnIndexRange, rowIndexRange), DataGridViewTextBoxCellEx)

                    If cell IsNot Nothing AndAlso cell IsNot Me Then
                        If cell.ColumnSpan > 1 Then cell.ColumnSpan = 1
                        If cell.RowSpan > 1 Then cell.RowSpan = 1
                        cell.OwnerCell = Me
                    End If
                Next
            Next

            OwnerCell = Nothing
            DataGridView.Invalidate()
        End If
    End Sub
#End Region

#Region "Editing."
    Public Overrides Function PositionEditingPanel(ByVal cellBounds As Rectangle, ByVal cellClip As Rectangle, ByVal cellStyle As DataGridViewCellStyle, ByVal singleVerticalBorderAdded As Boolean, ByVal singleHorizontalBorderAdded As Boolean, ByVal isFirstDisplayedColumn As Boolean, ByVal isFirstDisplayedRow As Boolean) As Rectangle
        If m_OwnerCell Is Nothing AndAlso m_ColumnSpan = 1 AndAlso m_RowSpan = 1 Then
            Return MyBase.PositionEditingPanel(cellBounds, cellClip, cellStyle, singleVerticalBorderAdded, singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow)
        End If

        Dim ownerCell = Me

        If m_OwnerCell IsNot Nothing Then
            Dim rowIndex = m_OwnerCell.RowIndex
            cellStyle = m_OwnerCell.GetInheritedStyle(Nothing, rowIndex, True)
            m_OwnerCell.GetFormattedValue(m_OwnerCell.Value, rowIndex, cellStyle, Nothing, Nothing, DataGridViewDataErrorContexts.Formatting)
            Dim editingControl = TryCast(DataGridView.EditingControl, IDataGridViewEditingControl)

            If editingControl IsNot Nothing Then
                editingControl.ApplyCellStyleToEditingControl(cellStyle)
                Dim editingPanel = DataGridView.EditingControl.Parent
                If editingPanel IsNot Nothing Then editingPanel.BackColor = cellStyle.BackColor
            End If

            ownerCell = m_OwnerCell
        End If

        cellBounds = DataGridViewCellExHelper.GetSpannedCellBoundsFromChildCellBounds(Me, cellBounds, singleVerticalBorderAdded, singleHorizontalBorderAdded)
        cellClip = DataGridViewCellExHelper.GetSpannedCellClipBounds(ownerCell, cellBounds, singleVerticalBorderAdded, singleHorizontalBorderAdded)

        Return MyBase.PositionEditingPanel(cellBounds, cellClip, cellStyle, singleVerticalBorderAdded, singleHorizontalBorderAdded, ownerCell.InFirstDisplayedColumn(), ownerCell.InFirstDisplayedRow())
    End Function

    Protected Overrides Function GetValue(ByVal rowIndex As Integer) As Object
        If m_OwnerCell IsNot Nothing Then Return m_OwnerCell.GetValue(m_OwnerCell.RowIndex)
        Return MyBase.GetValue(rowIndex)
    End Function

    Protected Overrides Function SetValue(ByVal rowIndex As Integer, ByVal value As Object) As Boolean
        If m_OwnerCell IsNot Nothing Then Return m_OwnerCell.SetValue(m_OwnerCell.RowIndex, value)
        Return MyBase.SetValue(rowIndex, value)
    End Function
#End Region

#Region "Other overridden"
    Protected Overrides Sub OnDataGridViewChanged()
        MyBase.OnDataGridViewChanged()

        If DataGridView Is Nothing Then
            m_ColumnSpan = 1
            m_RowSpan = 1
        End If
    End Sub

    Protected Overrides Function BorderWidths(ByVal advancedBorderStyle As DataGridViewAdvancedBorderStyle) As Rectangle
        If m_OwnerCell Is Nothing AndAlso m_ColumnSpan = 1 AndAlso m_RowSpan = 1 Then
            Return MyBase.BorderWidths(advancedBorderStyle)
        End If

        If m_OwnerCell IsNot Nothing Then Return m_OwnerCell.BorderWidths(advancedBorderStyle)

        Dim leftTop = MyBase.BorderWidths(advancedBorderStyle)
        Dim rightBottomCell = TryCast(DataGridView(ColumnIndex + ColumnSpan - 1, RowIndex + RowSpan - 1), DataGridViewTextBoxCellEx)
        Dim rightBottom = If(rightBottomCell IsNot Nothing, rightBottomCell.NativeBorderWidths(advancedBorderStyle), leftTop)

        Return New Rectangle(leftTop.X, leftTop.Y, rightBottom.Width, rightBottom.Height)
    End Function

    Private Function NativeBorderWidths(ByVal advancedBorderStyle As DataGridViewAdvancedBorderStyle) As Rectangle
        Return MyBase.BorderWidths(advancedBorderStyle)
    End Function

    Protected Overrides Function GetPreferredSize(ByVal graphics As Graphics, ByVal cellStyle As DataGridViewCellStyle, ByVal rowIndex As Integer, ByVal constraintSize As Size) As Size
        If OwnerCell IsNot Nothing Then Return New Size(0, 0)

        Dim size = MyBase.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize)
        Dim grid = DataGridView
        Dim width = size.Width - Enumerable.Range(ColumnIndex + 1, ColumnSpan - 1).[Select](Function(index) grid.Columns(index).Width).Sum()
        Dim height = size.Height - Enumerable.Range(rowIndex + 1, RowSpan - 1).[Select](Function(index) grid.Rows(index).Height).Sum()

        Return New Size(width, height)
    End Function
#End Region

#Region "Private Methods"
    Private Function CellsRegionContainsSelectedCell(ByVal columnIndex As Integer, ByVal rowIndex As Integer, ByVal columnSpan As Integer, ByVal rowSpan As Integer) As Boolean
        If DataGridView Is Nothing Then Return False

        Return (From col In Enumerable.Range(columnIndex, columnSpan)
                From row In Enumerable.Range(rowIndex, rowSpan)
                Where DataGridView(col, row).Selected
                Select col).Any()
    End Function
#End Region
End Class

Public Class DataGridViewTextBoxColumnEx
    Inherits DataGridViewColumn

    Public Sub New()
        MyBase.New(New DataGridViewTextBoxCellEx())
    End Sub
End Class
