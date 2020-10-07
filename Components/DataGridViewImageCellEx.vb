Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Public Class DataGridViewImageCellEx
    Inherits DataGridViewImageCell
    Implements ISpannedCell

#Region "Fields"
    Private m_ColumnSpan As Integer = 1
    Private m_RowSpan As Integer = 1
    Private m_OwnerCell As DataGridViewImageCellEx
#End Region

    Public Sub New()
    End Sub

    Public Sub New(ByVal valueIsIcon As Boolean)
        MyBase.New(valueIsIcon)
    End Sub

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
            m_OwnerCell = TryCast(value, DataGridViewImageCellEx)
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
                            DataGridView(col, row).ReadOnly = value
                        End If
                    Next
                Next
            End If
        End Set
    End Property
#End Region

#Region "Painting."
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
                Dim rightBottomCell = If(TryCast(DataGridView(mcolumnIndex + columnSpan - 1, rowIndex + rowSpan - 1), DataGridViewImageCellEx), Me)
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

        ' clear.
        If DataGridView IsNot Nothing Then
            For Each rowIndexRange As Integer In Enumerable.Range(RowIndex, prevRowSpan)
                For Each columnIndexRange As Integer In Enumerable.Range(ColumnIndex, prevColumnSpan)
                    Dim cell = TryCast(DataGridView(columnIndexRange, rowIndexRange), DataGridViewImageCellEx)
                    If cell IsNot Nothing Then cell.OwnerCell = Nothing
                Next
            Next

            ' set.
            For Each rowIndexRange As Integer In Enumerable.Range(RowIndex, m_RowSpan)
                For Each columnIndexRange As Integer In Enumerable.Range(ColumnIndex, m_ColumnSpan)
                    Dim cell = TryCast(DataGridView(columnIndexRange, rowIndexRange), DataGridViewImageCellEx)

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
        Dim rightBottomCell = TryCast(DataGridView(ColumnIndex + ColumnSpan - 1, RowIndex + RowSpan - 1), DataGridViewImageCellEx)
        Dim rightBottom = If(rightBottomCell IsNot Nothing, rightBottomCell.NativeBorderWidths(advancedBorderStyle), leftTop)

        Return New Rectangle(leftTop.X, leftTop.Y, rightBottom.Width, rightBottom.Height)
    End Function

    Private Function NativeBorderWidths(ByVal advancedBorderStyle As DataGridViewAdvancedBorderStyle) As Rectangle
        Return MyBase.BorderWidths(advancedBorderStyle)
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

Public NotInheritable Class DataGridViewImageColumnEx
    Inherits DataGridViewColumn

#Region "Fields"
    Shared errorBmp As Bitmap
    Shared errorIco As Icon
#End Region

#Region "Properties"
    Private Shared ReadOnly Property ErrorBitmap As Bitmap
        Get
            'Return If(errorBmp, (CSharpImpl.__Assign(errorBmp, New Bitmap(GetType(DataGridView), "ImageInError.bmp"))))
            If errorBmp Is Nothing Then
                errorBmp = New Bitmap(GetType(DataGridView), "ImageInError.bmp")
            End If

            Return errorBmp
        End Get
    End Property

    Private Shared ReadOnly Property ErrorIcon As Icon
        Get
            'Return If(errorIco, (CSharpImpl.__Assign(errorIco, New Icon(GetType(DataGridView), "IconInError.ico"))))
            If errorIco Is Nothing Then
                errorIco = New Icon(GetType(DataGridView), "IconInError.ico")
            End If

            Return errorIco
        End Get
    End Property

    Private ReadOnly Property ImageCellTemplate As DataGridViewImageCellEx
        Get
            Return CType(Me.CellTemplate, DataGridViewImageCellEx)
        End Get
    End Property

    <DefaultValue(1)>
    Public Property ImageLayout As DataGridViewImageCellLayout
        Get

            If Me.CellTemplate Is Nothing Then
                Throw New InvalidOperationException()
            End If

            Dim mimageLayout As DataGridViewImageCellLayout = Me.ImageCellTemplate.ImageLayout

            If mimageLayout = DataGridViewImageCellLayout.NotSet Then
                mimageLayout = DataGridViewImageCellLayout.Normal
            End If

            Return mimageLayout
        End Get
        Set(ByVal value As DataGridViewImageCellLayout)
            If Me.ImageLayout = value Then Return
            Me.ImageCellTemplate.ImageLayout = value
            If DataGridView Is Nothing Then Return
            Dim rows = DataGridView.Rows
            Dim count = rows.Count

            For i = 0 To count - 1
                Dim cell = TryCast(rows.SharedRow(i).Cells(Index), DataGridViewImageCell)

                If cell IsNot Nothing Then
                    cell.ImageLayout = value
                End If
            Next
        End Set
    End Property

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ValuesAreIcons As Boolean
        Get

            If Me.ImageCellTemplate Is Nothing Then
                Throw New InvalidOperationException()
            End If

            Return Me.ImageCellTemplate.ValueIsIcon
        End Get
        Set(ByVal value As Boolean)
            If Me.ValuesAreIcons = value Then Return
            Me.ImageCellTemplate.ValueIsIcon = value

            If DataGridView IsNot Nothing Then
                Dim rows = DataGridView.Rows
                Dim count = rows.Count

                For i = 0 To count - 1
                    Dim cell = TryCast(rows.SharedRow(i).Cells(Index), DataGridViewImageCell)

                    If cell IsNot Nothing Then
                        cell.ValueIsIcon = value
                    End If
                Next
            End If

            If (value AndAlso (TypeOf Me.DefaultCellStyle.NullValue Is Bitmap)) AndAlso (Me.DefaultCellStyle.NullValue Is ErrorBitmap) Then
                Me.DefaultCellStyle.NullValue = ErrorIcon
            ElseIf (Not value AndAlso (TypeOf Me.DefaultCellStyle.NullValue Is Icon)) AndAlso (Me.DefaultCellStyle.NullValue Is ErrorIcon) Then
                Me.DefaultCellStyle.NullValue = ErrorBitmap
            End If
        End Set
    End Property

#End Region

#Region "ctor"
    Public Sub New()
        Me.New(False)
    End Sub

    Public Sub New(ByVal valuesAreIcons As Boolean)
        MyBase.New(New DataGridViewImageCellEx(valuesAreIcons))
        Dim style = New DataGridViewCellStyle With {
            .Alignment = DataGridViewContentAlignment.MiddleCenter
        }

        If valuesAreIcons Then
            style.NullValue = ErrorIcon
        Else
            style.NullValue = ErrorBitmap
        End If

        Me.DefaultCellStyle = style
    End Sub

    'Private Class CSharpImpl
    '    <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
    '    Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
    '        target = value
    '        Return value
    '    End Function
    'End Class
#End Region

End Class


