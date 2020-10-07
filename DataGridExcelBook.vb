Option Strict Off

Imports System.Drawing
Imports System.Drawing.Printing
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Office.Interop

Public Class DataGridExcelBook
    ''' <summary>
    ''' Тип Параметра
    ''' </summary>
    Private Enum TypeParameter
        ''' <summary>
        ''' Измерение
        ''' </summary>
        Measurement
        ''' <summary>
        ''' Приведение
        ''' </summary>
        PhysicalCast
        ''' <summary>
        ''' Пересчет
        ''' </summary>
        Converting
    End Enum

    ''' <summary>
    ''' Типы формата ячеек
    ''' </summary>
    Private Enum TypeCellStyle
        ''' <summary>
        ''' 1 - имя этапа
        ''' </summary>
        StageName
        ''' <summary>
        ''' 2 - описание этапа
        ''' </summary>
        DescriptionStage
        ''' <summary>
        ''' 3 - текст контрола
        ''' </summary>
        ControlText
        ''' <summary>
        ''' 4 - значение контрола ' .ЗначениеПользователя
        ''' </summary>
        ControlValue
        ''' <summary>
        ''' 5 - заголовок Исходные данные (измеренная информация)
        ''' </summary>
        MeasurementTitle
        ''' <summary>
        ''' 6 - заголовок Физические и приведенные параметр
        ''' </summary>
        PhysicalCastTitle
        ''' <summary>
        ''' 7 - заголовок Пересчитанные параметры
        ''' </summary>
        ConvertingTitle
        ''' <summary>
        ''' 8 - заголовок пустой
        ''' </summary>
        EmptyTitle
        ''' <summary>
        ''' 9- заголовок: Параметр, Значение, Размерность
        ''' </summary>
        Caption
        ''' <summary>
        ''' 10- имя параметра (текст контрола)
        ''' </summary>
        ParameterName
        ''' <summary>
        ''' 11- значение параметра
        ''' </summary>
        MeasurementParameterValue
        ''' <summary>
        ''' 12- значение параметра
        ''' </summary>
        PhysicalCastParameterValue
        ''' <summary>
        ''' 13- значение параметра
        ''' </summary>
        ConvertingParameterValue
        ''' <summary>
        ''' 14- значение размерности
        ''' </summary>
        ParameterUnit
        ''' <summary>
        ''' 15- пустой квадрат
        ''' </summary>
        ColumnsRowsEmptyTitle
    End Enum

    Private currentColumn As Integer
    Private currentRow As Integer
    Private startBlock As Integer
    Private indexRowValue As Integer
    Private colorIndex As Integer
    Private Const TitleMeasurement As String = "Исходные данные (измеренная информация)"
    Private Const TitlePhysicalAndCast As String = "Физические и приведенные параметры"
    Private Const TitleConverting As String = "Пересчитанные параметры"

    Private WithEvents mDataGridViewReportKT As DataGridView
    Private Const LimitControls As Integer = 9 ' только LimitControls контролов в строке
    Private DictionaryOfCellStyle As Dictionary(Of TypeCellStyle, SpanDataGridViewCellStyle)

    Private specialCells As Boolean(,) ' отметка ячеек для контроля цифрового значения
    Private Const OfsetExelRow As Integer = 2 ' смещения для протокола Exel
    Private Const OfsetExelColunm As Integer = 2 ' смещения для протокола Exel

    Public Sub New(dataGridViewReportKT As DataGridView)
        mDataGridViewReportKT = dataGridViewReportKT
        InitializeDataGridView()
        PopulateDictionaryOfCellStyle()
    End Sub

    ''' <summary>
    ''' Конфигурировать Таблицу
    ''' </summary>
    Private Sub InitializeDataGridView()
        'mDataGridViewReportKT.Name = "mDataGridViewReportKT"
        'mDataGridViewReportKT.Location = New Point(8, 8)
        'mDataGridViewReportKT.Size = New Size(500, 250)

        mDataGridViewReportKT.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        mDataGridViewReportKT.CellBorderStyle = DataGridViewCellBorderStyle.Sunken
        mDataGridViewReportKT.GridColor = Color.Black
        mDataGridViewReportKT.RowHeadersVisible = True ' False
        mDataGridViewReportKT.RowHeadersWidth = 60
        mDataGridViewReportKT.ColumnHeadersVisible = False

        'AddHandler mDataGridViewReportKT.CellFormatting, AddressOf New DataGridViewCellFormattingEventHandler(mDataGridViewReportKT_CellFormatting)

        ' инициализировать основные DataGridView свойства.
        'mDataGridViewReportKT.Dock = DockStyle.Fill
        mDataGridViewReportKT.BackgroundColor = Color.WhiteSmoke 'LightGray ' WhiteSmoke
        mDataGridViewReportKT.BorderStyle = BorderStyle.Fixed3D

        ' установить свойство подходящее для только чтения экрана ограничивающее интерактивность
        mDataGridViewReportKT.AllowUserToAddRows = False
        mDataGridViewReportKT.AllowUserToDeleteRows = False
        mDataGridViewReportKT.AllowUserToOrderColumns = False
        'mDataGridViewReportKT.ReadOnly = True
        mDataGridViewReportKT.SelectionMode = DataGridViewSelectionMode.CellSelect 'DataGridViewSelectionMode.FullRowSelect
        mDataGridViewReportKT.MultiSelect = False
        mDataGridViewReportKT.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells 'DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders
        mDataGridViewReportKT.AllowUserToResizeColumns = False
        mDataGridViewReportKT.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        mDataGridViewReportKT.AllowUserToResizeRows = False
        mDataGridViewReportKT.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing

        ' Задать фоновый цвет выделения для всех ячеек.
        mDataGridViewReportKT.DefaultCellStyle.SelectionBackColor = Color.LightGray 'Color.White
        mDataGridViewReportKT.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Установить RowHeadersDefaultCellStyle.SelectionBackColor так чтобы по умолчанию
        mDataGridViewReportKT.RowHeadersDefaultCellStyle.BackColor = Color.Black
        mDataGridViewReportKT.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty

        ' Задать цвет фона альтернативных строк. 
        mDataGridViewReportKT.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke 'Color.LightGray
        'mDataGridViewReportKT.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray

        ' Задать стиль заговочных строк и столбцов.
        mDataGridViewReportKT.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        mDataGridViewReportKT.ColumnHeadersDefaultCellStyle.BackColor = Color.Black ' Color.Navy
        mDataGridViewReportKT.ColumnHeadersDefaultCellStyle.Font = New Font(mDataGridViewReportKT.Font, FontStyle.Bold)
    End Sub

    Private Sub PopulateDictionaryOfCellStyle()
        DictionaryOfCellStyle = New Dictionary(Of TypeCellStyle, SpanDataGridViewCellStyle) From {
            {TypeCellStyle.StageName, New StageNameSpanDataGridViewCellStyle},
            {TypeCellStyle.DescriptionStage, New DescriptionStageSpanDataGridViewCellStyle},
            {TypeCellStyle.ControlText, New ControlTextSpanDataGridViewCellStyle},
            {TypeCellStyle.ControlValue, New ControlValueSpanDataGridViewCellStyle},
            {TypeCellStyle.MeasurementTitle, New MeasurementTitleSpanDataGridViewCellStyle},
            {TypeCellStyle.PhysicalCastTitle, New PhysicalCastTitleSpanDataGridViewCellStyle},
            {TypeCellStyle.ConvertingTitle, New ConvertingTitleSpanDataGridViewCellStyle},
            {TypeCellStyle.EmptyTitle, New EmptyTitleSpanDataGridViewCellStyle},
            {TypeCellStyle.Caption, New CaptionSpanDataGridViewCellStyle},
            {TypeCellStyle.ParameterName, New ParameterNameSpanDataGridViewCellStyle},
            {TypeCellStyle.MeasurementParameterValue, New MeasurementParameterValueSpanDataGridViewCellStyle},
            {TypeCellStyle.PhysicalCastParameterValue, New PhysicalCastParameterSpanDataGridViewCellStyle},
            {TypeCellStyle.ConvertingParameterValue, New ConvertingParameterValueSpanDataGridViewCellStyle},
            {TypeCellStyle.ParameterUnit, New ParameterUnitSpanDataGridViewCellStyle},
            {TypeCellStyle.ColumnsRowsEmptyTitle, New ColumnsRowsEmptyTitleSpanDataGridViewCellStyle}
        }
    End Sub

#Region "CreateDataGridReport"
    Private Function GetNewRow() As DataGridViewRow
        Dim newRow As DataGridViewRow = New DataGridViewRow()
        newRow.CreateCells(mDataGridViewReportKT)
        newRow.ReadOnly = True
        'newRow.Resizable = DataGridViewTriState.True
        newRow.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        currentRow = mDataGridViewReportKT.Rows.Add(newRow)
        mDataGridViewReportKT.Rows(currentRow).HeaderCell.Value = currentRow.ToString '"ok" '

        Return newRow
    End Function

    Private Function GetNewCell(inCurrentColumn As Integer,
                                inCurrentRow As Integer,
                                inTypeCellStyle As TypeCellStyle,
                                inCellValue As Object) As DataGridViewTextBoxCellEx
        Dim newCell As DataGridViewTextBoxCellEx = CType(mDataGridViewReportKT(inCurrentColumn, inCurrentRow), DataGridViewTextBoxCellEx)
        Dim cellStyle As SpanDataGridViewCellStyle = DictionaryOfCellStyle(inTypeCellStyle)

        newCell.Style = cellStyle.TypeDataGridViewCellStyle.Clone
        newCell.ReadOnly = cellStyle.IsReadOnly
        newCell.ColumnSpan = cellStyle.ColumnSpan
        newCell.RowSpan = cellStyle.RowSpan
        newCell.Value = inCellValue

        Return newCell
    End Function

    ''' <summary>
    ''' Создаёт пустой шаблон отчёта и отображает его но сетке
    ''' </summary>
    Public Sub GreateEmptyReport(ByVal StageConstNames() As String,
                                 ByVal StageNames() As String,
                                 ByVal varProjectManager As ProjectManager)

        RemoveHandler mDataGridViewReportKT.CellValidating, AddressOf mDataGridViewReportKT_CellValidating
        mDataGridViewReportKT.Columns.Clear()

        ' динамическое добавление столбцов
        For I As Integer = 1 To LimitControls
            Dim newColumn As DataGridViewTextBoxColumnEx = New DataGridViewTextBoxColumnEx() With {
                .HeaderText = "Колонка_" + I.ToString,
                .Width = 100,
                .Name = "Column" + I.ToString,
                .Frozen = False,
                .CellTemplate = New DataGridViewTextBoxCellEx(),
                .SortMode = DataGridViewColumnSortMode.NotSortable,
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            }
            '.ReadOnly = False,
            '.FillWeight = 100,
            '.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            mDataGridViewReportKT.Columns.Add(newColumn)
        Next

        Dim J As Integer
        Dim controlsCount As Integer
        Dim MeasurementRows As New List(Of DataRow) ' Измерение
        Dim CastRows As New List(Of DataRow) ' Приведение
        Dim ConvertingRows As New List(Of DataRow) ' Пересчет
        Dim newCell As DataGridViewTextBoxCellEx
        Dim description As String = String.Empty

        For I As Integer = 0 To StageNames.Count - 1
            'I здесь этапы
            currentColumn = 0
            ' Тип изделия
            Dim newRow As DataGridViewRow = GetNewRow()
            '"Имя этапа"
            newCell = GetNewCell(currentColumn, currentRow, TypeCellStyle.StageName, StageConstNames(I))
            ' Примечание
            currentColumn = 1
            For Each keyControl As MDBControlLibrary.IUserControl In varProjectManager.ControlsForPhase.Item(StageNames(I)).Values
                If keyControl.Name.IndexOf("Описание") <> -1 Then
                    description = "0" 'keyControl.ЗначениеПользователя '"Примечание этапа"
                    keyControl.Row = currentRow
                    keyControl.Col = currentColumn
                    Exit For
                End If
            Next
            newCell = GetNewCell(currentColumn, currentRow, TypeCellStyle.DescriptionStage, description)

            newRow = GetNewRow()
            newRow = GetNewRow()
            J = 0
            currentColumn = 0
            ' по количеству контролов в этапе
            controlsCount = varProjectManager.ControlsForPhase.Item(StageNames(I)).Values.Where(Function(keyControl) keyControl.Name.IndexOf("Описание") = -1).Count

            If controlsCount > 0 Then
                For Each keyControl As MDBControlLibrary.IUserControl In varProjectManager.ControlsForPhase.Item(StageNames(I)).Values
                    If keyControl.Name.IndexOf("Описание") = -1 Then
                        '"Имя контрола"
                        newCell = GetNewCell(currentColumn, currentRow - 1, TypeCellStyle.ControlText, keyControl.Text)
                        ' значение канала
                        newCell = GetNewCell(currentColumn, currentRow, TypeCellStyle.ControlValue, "0") 'keyControl.ЗначениеПользователя 'значение контрола

                        'для возможности редактирования значения контролов через таблицу только для контролов этапа КТ
                        If StageNames(I) = NumberKT Then newCell.ReadOnly = False

                        keyControl.Row = currentRow
                        keyControl.Col = currentColumn

                        J += 1
                        currentColumn += 1

                        ' только LimitControls контролов в строке
                        If J Mod LimitControls = 0 AndAlso J <> controlsCount Then
                            currentColumn = 0
                            newRow = GetNewRow()
                            newRow = GetNewRow()
                        End If
                    End If
                Next

                If currentColumn <> LimitControls Then
                    ' заполнить пустое место после контролов
                    newCell = GetNewCell(currentColumn, currentRow - 1, TypeCellStyle.ColumnsRowsEmptyTitle, "")
                    newCell.ColumnSpan = mDataGridViewReportKT.Columns.Count - currentColumn
                    newCell.RowSpan = 2
                End If

                newRow = GetNewRow()
                currentColumn = 0
                newCell = GetNewCell(currentColumn, currentRow, TypeCellStyle.EmptyTitle, "")
            End If
        Next

        ' --- Измеренные ------------------------------------------------------
        Dim resultMeasurementRows = From row In varProjectManager.MeasurementDataTable Select row

        For Each itemDataRow In resultMeasurementRows
            MeasurementRows.Add(itemDataRow)
        Next

        PrintParametersRows(MeasurementRows, TypeParameter.Measurement, TitleMeasurement)

        ' скролить к этой позиции
        mDataGridViewReportKT.FirstDisplayedCell = mDataGridViewReportKT.Rows(currentRow).Cells(0)

        ' --- Приведение  -----------------------------------------------------
        Dim resultCastRows = From row In varProjectManager.CalculatedDataTable
                             Where row.ПриведеныйПараметр = True
                             Select row

        For Each itemDataRow In resultCastRows
            CastRows.Add(itemDataRow)
        Next

        PrintParametersRows(CastRows, TypeParameter.PhysicalCast, TitlePhysicalAndCast)

        ' --- Пересчет --------------------------------------------------------
        Dim resultConvertingRows = From row In varProjectManager.CalculatedDataTable
                                   Where row.ПриведеныйПараметр = False
                                   Select row

        For Each itemDataRow In resultConvertingRows
            ConvertingRows.Add(itemDataRow)
        Next

        PrintParametersRows(ConvertingRows, TypeParameter.Converting, TitleConverting)

        ' заполнить вспомогательный массив для отслеживания корректного ввода чисел при редактировании разрешенных ячеек
        specialCells = New Boolean(mDataGridViewReportKT.Columns.Count - 1, mDataGridViewReportKT.Rows.Count - 1) {}
        For row As Integer = 0 To mDataGridViewReportKT.Rows.Count - 1
            For col As Integer = 0 To mDataGridViewReportKT.Columns.Count - 1
                specialCells(col, row) = CBool(If(mDataGridViewReportKT(col, row)?.Tag, False))
            Next
        Next

        'mDataGridViewReportKT.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader)

        AddHandler mDataGridViewReportKT.CellValidating, AddressOf mDataGridViewReportKT_CellValidating
    End Sub

    ''' <summary>
    ''' Обработка Собранных Параметров
    ''' </summary>
    ''' <param name="DataRowParameters"></param>
    ''' <param name="inTypeParameter"></param>
    Private Sub PrintParametersRows(ByVal DataRowParameters As List(Of DataRow), ByVal inTypeParameter As TypeParameter, InTitle As String)
        Dim J As Integer
        Dim rowCount As Integer = DataRowParameters.Count ' Количество Параметров Подсчета

        If rowCount = 0 Then Exit Sub

        currentColumn = 0
        startBlock = currentRow
        ' заголовок текущего блока
        Dim newRow As DataGridViewRow = GetNewRow()
        Dim titleCellStyle, valueCellStyle As TypeCellStyle

        Select Case inTypeParameter
            Case TypeParameter.PhysicalCast
                titleCellStyle = TypeCellStyle.PhysicalCastTitle
                valueCellStyle = TypeCellStyle.PhysicalCastParameterValue
            Case TypeParameter.Converting
                titleCellStyle = TypeCellStyle.ConvertingTitle
                valueCellStyle = TypeCellStyle.ConvertingParameterValue
            Case Else
                titleCellStyle = TypeCellStyle.MeasurementTitle
                valueCellStyle = TypeCellStyle.MeasurementParameterValue
        End Select

        Dim newCell As DataGridViewTextBoxCellEx = GetNewCell(currentColumn, currentRow, titleCellStyle, InTitle)
        Dim unit As String = String.Empty

        PrintTitleParameterValueUnit()
        currentColumn = 1

        For Each dataRowParameter As DataRow In DataRowParameters
            J += 1
            ' Имя параметра
            newCell = GetNewCell(currentColumn, currentRow, TypeCellStyle.ParameterName, dataRowParameter("ИмяПараметра"))
            ' иэмеренное значение
            newCell = GetNewCell(currentColumn, currentRow + 1, valueCellStyle, 0.00)
            ' отметить индексы ячеек для последующего заполнения реальными данными
            dataRowParameter("Row") = currentRow + 1
            dataRowParameter("Col") = currentColumn

            ' размерность измеренной величины
            If inTypeParameter = TypeParameter.Measurement Then
                unit = CStr(dataRowParameter("РазмерностьВходная"))
                newCell.Tag = True ' признак возможности редактирования значения Double
            Else
                unit = CStr(dataRowParameter("РазмерностьВыходная"))
            End If

            newCell = GetNewCell(currentColumn, currentRow + 2, TypeCellStyle.ParameterUnit, unit)

            currentColumn += 1
            ' не более LimitControls-1 параметров в строке
            If J Mod (LimitControls - 1) = 0 AndAlso J <> rowCount Then
                currentRow += 2
                PrintTitleParameterValueUnit()
                currentColumn = 1
            End If
        Next

        If currentColumn <> LimitControls Then
            ' заполнить пустое место после контролов
            newCell = GetNewCell(currentColumn, currentRow, TypeCellStyle.ColumnsRowsEmptyTitle, "")
            newCell.ColumnSpan = mDataGridViewReportKT.Columns.Count - currentColumn
            newCell.RowSpan = 3
        End If

        ' пустая строка
        currentRow += 3
        newRow = GetNewRow()
        currentColumn = 0
        newCell = GetNewCell(currentColumn, currentRow, TypeCellStyle.EmptyTitle, "")
    End Sub

    ''' <summary>
    ''' Заголовок Параметр Значение Размерность
    ''' </summary>
    Private Sub PrintTitleParameterValueUnit()
        Dim newRow As DataGridViewRow
        Dim newCell As DataGridViewTextBoxCellEx

        currentColumn = 0

        newRow = GetNewRow()
        newCell = GetNewCell(currentColumn, currentRow, TypeCellStyle.Caption, "Параметр")
        indexRowValue = currentRow ' запомнить позицию

        newRow = GetNewRow()
        newCell = GetNewCell(currentColumn, currentRow, TypeCellStyle.Caption, "Значение")

        newRow = GetNewRow()
        newCell = GetNewCell(currentColumn, currentRow, TypeCellStyle.Caption, "Размерность")
        currentRow = indexRowValue ' восстановить позицию
    End Sub



    ''' <summary>
    ''' Задать цифровое значение в ячейке таблицы DataGridView
    ''' </summary>
    ''' <param name="rowIndex"></param>
    ''' <param name="columnIndex"></param>
    ''' <param name="value"></param>
    Public Sub SetDoubleRC(rowIndex As Integer, columnIndex As Integer, value As String)
        'If specialCells(columnIndex, rowIndex) Then
        Dim newDouble As Double

        If Double.TryParse(value, newDouble) Then
            mDataGridViewReportKT.Rows(rowIndex).Cells(columnIndex).Value = newDouble
        Else
            MessageBox.Show($"Для ячейки с адресом <row={rowIndex} : column={columnIndex}> разрешено вводить только цифровые значения!",
                            "Ошибка ввода значения", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        'Else
        '    MessageBox.Show($"Для ячейки с адресом <row={rowIndex} : column={columnIndex}> запрещено редактирование!",
        '                        "Ошибка ввода значения", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'End If
    End Sub

    ''' <summary>
    ''' Задать текстовое значение в ячейке таблицы DataGridView
    ''' </summary>
    ''' <param name="rowIndex"></param>
    ''' <param name="columnIndex"></param>
    ''' <param name="value"></param>
    Public Sub SetTextRC(rowIndex As Integer, columnIndex As Integer, value As String)
        mDataGridViewReportKT.Rows(rowIndex).Cells(columnIndex).Value = value
    End Sub

    ''' <summary>
    ''' Получить цифровое значение в ячейке таблицы DataGridView
    ''' </summary>
    ''' <param name="rowIndex"></param>
    ''' <param name="columnIndex"></param>
    ''' <returns></returns>
    Public Function GetToDoubleRC(rowIndex As Integer, columnIndex As Integer) As Double
        If specialCells(columnIndex, rowIndex) Then
            'Double.Parse(mDataGridViewReportKT.Rows(rowIndex).Cells(columnIndex).Value, CultureInfo.CurrentCulture)
            Return CDbl(mDataGridViewReportKT.Rows(rowIndex).Cells(columnIndex).Value)
        Else
            MessageBox.Show($"Для ячейки с адресом <row={rowIndex} : column={columnIndex}> запрещено редактирование!",
                                "Ошибка ввода значения", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return 9999999.0
        End If
    End Function

    ''' <summary>
    ''' Получить цифровое значение в ячейке таблицы DataGridView
    ''' для протокола Exel
    ''' </summary>
    ''' <param name="rowIndex"></param>
    ''' <param name="columnIndex"></param>
    ''' <returns></returns>
    Private Function GetToDoubleRCForExcel(rowIndex As Integer, columnIndex As Integer) As Double
        ' currentRow и currentColumn уменьшил т.к. индекс с 0 и еще в Excel со 2-го столбца 
        Return CDbl(mDataGridViewReportKT.Rows(rowIndex - OfsetExelRow).Cells(columnIndex - OfsetExelColunm).Value)
    End Function

    ''' <summary>
    ''' Получить текстовое значение в ячейке таблицы DataGridView
    ''' </summary>
    ''' <param name="rowIndex"></param>
    ''' <param name="columnIndex"></param>
    ''' <returns></returns>
    Public Function GetToStringRC(rowIndex As Integer, columnIndex As Integer) As String
        Return mDataGridViewReportKT.Rows(rowIndex).Cells(columnIndex).Value.ToString
    End Function

    Private Sub mDataGridViewReportKT_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) ' Handles mDataGridViewReportKT.CellValidating
        mDataGridViewReportKT.Rows(e.RowIndex).ErrorText = ""
        Dim newDouble As Double

        If e.FormattedValue Is Nothing OrElse String.IsNullOrEmpty(e.FormattedValue.ToString()) OrElse Not specialCells(e.ColumnIndex, e.RowIndex) Then Return

        If Not Double.TryParse(e.FormattedValue.ToString(), newDouble) Then
            mDataGridViewReportKT.Rows(e.RowIndex).ErrorText = "Разрешены только цифры!"
            'mDataGridViewReportKT.Rows(e.RowIndex).Cells(e.ColumnIndex).ErrorText = "Разрешены только цифры!"
            e.Cancel = True
        End If
    End Sub

    Private Sub mDataGridViewReportKT_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles mDataGridViewReportKT.CellEndEdit
        mDataGridViewReportKT.Rows(e.RowIndex).ErrorText = ""
        'mDataGridViewReportKT.Rows(e.RowIndex).Cells(e.ColumnIndex).ErrorText = ""
    End Sub
#End Region

#Region "CreateExelReport"
    Public Sub CreateExcelBookForKT(ByVal StageConstNames() As String,
                                    ByVal StageNames() As String,
                                    ByVal varProjectManager As ProjectManager,
                                    isPrintSheetExcel As Boolean,
                                    Optional pathExcelProtocolCalculateKT As String = "")
        Dim J As Integer
        Dim controlsCount As Integer
        Dim MeasurementRows As New List(Of DataRow) ' Измерение
        Dim PhysicalAndCastRows As New List(Of DataRow) ' Приведение
        Dim ConvertingRows As New List(Of DataRow) ' Пересчет

        Dim excelApp As New Excel.Application()
        Dim excelBook As Excel.Workbook = excelApp.Workbooks.Add
        Dim excelWorksheet As Excel.Worksheet = CType(excelBook.Worksheets(1), Excel.Worksheet)

        Try
            excelApp.Visible = False

            'With excelWorksheet
            '    ' Set the column headers and desired formatting for the spreadsheet.
            '    .Columns().ColumnWidth = 21.71
            '    .Range("A1").Value = "Item"
            '    .Range("A1").Font.Bold = True
            '    .Range("B1").Value = "Price"
            '    .Range("B1").Font.Bold = True
            '    .Range("C1").Value = "Calories"
            '    .Range("C1").Font.Bold = True

            '    ' Start the counter on the second row, following the column headers
            '    Dim i As Integer = 2
            '    ' Loop through the Rows collection of the DataSet and write the data
            '    ' in each row to the cells in Excel. 
            '    Dim dr As DataRow
            '    For Each dr In dsMenu.Tables(0).Rows
            '        .Range("A" & i.ToString).Value = dr("Item")
            '        .Range("B" & i.ToString).Value = dr("Price")
            '        .Range("C" & i.ToString).Value = dr("Calories")
            '        i += 1
            '    Next

            '    ' Select and apply formatting to the cell that will display the calorie
            '    ' average, then call the Average formula.  Note that the AVERAGE function
            '    ' is localized, so the below code may need to be updated based on the 
            '    ' locale the application is deployed to.
            '    .Range("C7").Select()
            '    .Range("C7").Font.Color = RGB(255, 0, 0)
            '    .Range("C7").Font.Bold = True
            '    excelApp.ActiveCell.FormulaR1C1 = "=AVERAGE(R[-5]C:R[-1]C)"
            'End With

            With excelApp
                .Cells.Select()
                .Selection.Clear()
                .Columns("A:A").ColumnWidth = 0.67
                .Rows("1:1").RowHeight = 4.5
                .Application.ScreenUpdating = False

                currentRow = OfsetExelRow
                For I As Integer = 0 To StageNames.Count - 1
                    'I здесь этапы
                    currentColumn = OfsetExelColunm
                    ' Тип изделия
                    .Range("B" & CStr(currentRow)).Select()
                    With .Selection
                        .HorizontalAlignment = Excel.Constants.xlCenter
                        .VerticalAlignment = Excel.Constants.xlTop
                        .WrapText = True
                        .Orientation = 0
                        .AddIndent = False
                        .IndentLevel = 0
                        .ShrinkToFit = False
                        .ReadingOrder = Excel.Constants.xlContext
                        .MergeCells = False
                    End With

                    .ActiveCell.FormulaR1C1 = StageConstNames(I) '"Имя этапа"

                    With .ActiveCell.Characters(Start:=1, Length:=Len(CStr(.ActiveCell.Value))).Font
                        .Name = "Arial"
                        .FontStyle = "обычный"
                        .Size = 10
                        .Strikethrough = False
                        .Superscript = False
                        .Subscript = False
                        .OutlineFont = False
                        .Shadow = False
                        .Underline = Excel.XlUnderlineStyle.xlUnderlineStyleNone
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With

                    ' Примечание
                    .Range("C" & CStr(currentRow) & ":J" & CStr(currentRow)).Select()
                    With .Selection
                        .HorizontalAlignment = Excel.Constants.xlCenter
                        .VerticalAlignment = Excel.Constants.xlCenter
                        .WrapText = True
                        .Orientation = 0
                        .AddIndent = False
                        .IndentLevel = 0
                        .ShrinkToFit = False
                        .ReadingOrder = Excel.Constants.xlContext
                        .MergeCells = True
                    End With

                    For Each keyControl As MDBControlLibrary.IUserControl In varProjectManager.ControlsForPhase.Item(StageNames(I)).Values
                        If keyControl.Name.IndexOf("Описание") <> -1 Then
                            .ActiveCell.FormulaR1C1 = keyControl.UserValue '"Примечание этапа"
                            Exit For
                        End If
                    Next

                    With .ActiveCell.Characters(Start:=1, Length:=Len(CStr(.ActiveCell.Value))).Font
                        .Name = "Arial"
                        .FontStyle = "обычный"
                        .Size = 10
                        .Strikethrough = False
                        .Superscript = False
                        .Subscript = False
                        .OutlineFont = False
                        .Shadow = False
                        .Underline = Excel.XlUnderlineStyle.xlUnderlineStyleNone
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With

                    ' выделить область жирным
                    .Range("B" & CStr(currentRow) & ":I" & CStr(currentRow)).Select()
                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone

                    With .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With

                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone

                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlMedium
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlMedium
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlMedium
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlMedium
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With

                    ''''''''''''''''''''''''''''''''''
                    currentRow += 1
                    J = 0
                    ' по количеству контролов в этапе
                    controlsCount = varProjectManager.ControlsForPhase.Item(StageNames(I)).Values.Where(Function(keyControl) keyControl.Name.IndexOf("Описание") = -1).Count

                    If controlsCount > 0 Then
                        For Each keyControl As MDBControlLibrary.IUserControl In varProjectManager.ControlsForPhase.Item(StageNames(I)).Values
                            If keyControl.Name.IndexOf("Описание") = -1 Then
                                J += 1
                                ' имя канала
                                .Range(.Cells(currentRow, currentColumn), .Cells(currentRow, currentColumn)).Select()
                                With .Selection.Interior
                                    .ColorIndex = 15
                                    .Pattern = Excel.Constants.xlSolid
                                End With

                                With .Selection
                                    .HorizontalAlignment = Excel.Constants.xlCenter
                                    .VerticalAlignment = Excel.Constants.xlTop
                                    .WrapText = True
                                    .Orientation = 0
                                    .AddIndent = False
                                    .IndentLevel = 0
                                    .ShrinkToFit = False
                                    .ReadingOrder = Excel.Constants.xlContext
                                    .MergeCells = False
                                End With
                                .ActiveCell.FormulaR1C1 = keyControl.Text '"Имя контрола"

                                With .ActiveCell.Characters(Start:=1, Length:=Len(CStr(.ActiveCell.Value))).Font
                                    .Name = "Arial"
                                    .FontStyle = "обычный"
                                    .Size = 10
                                    .Strikethrough = False
                                    .Superscript = False
                                    .Subscript = False
                                    .OutlineFont = False
                                    .Shadow = False
                                    .Underline = Excel.XlUnderlineStyle.xlUnderlineStyleNone
                                    .ColorIndex = Excel.Constants.xlAutomatic
                                End With

                                ' значение канала
                                .Range(.Cells(currentRow + 1, currentColumn), .Cells(currentRow + 1, currentColumn)).Select()
                                .ActiveCell.FormulaR1C1 = keyControl.UserValue 'значение контрола
                                ' другой поток или отсутствует значение

                                ' выделенный блок жирным
                                .Range(.Cells(currentRow + 1, currentColumn), .Cells(currentRow, currentColumn)).Select()
                                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
                                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone

                                With .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
                                    .LineStyle = Excel.XlLineStyle.xlContinuous
                                    .Weight = Excel.XlBorderWeight.xlThin
                                    .ColorIndex = Excel.Constants.xlAutomatic
                                End With
                                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                                    .LineStyle = Excel.XlLineStyle.xlContinuous
                                    .Weight = Excel.XlBorderWeight.xlThin
                                    .ColorIndex = Excel.Constants.xlAutomatic
                                End With
                                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                                    .LineStyle = Excel.XlLineStyle.xlContinuous
                                    .Weight = Excel.XlBorderWeight.xlThin
                                    .ColorIndex = Excel.Constants.xlAutomatic
                                End With
                                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                                    .LineStyle = Excel.XlLineStyle.xlContinuous
                                    .Weight = Excel.XlBorderWeight.xlThin
                                    .ColorIndex = Excel.Constants.xlAutomatic
                                End With
                                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                                    .LineStyle = Excel.XlLineStyle.xlContinuous
                                    .Weight = Excel.XlBorderWeight.xlThin
                                    .ColorIndex = Excel.Constants.xlAutomatic
                                End With

                                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
                                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
                                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                                    .LineStyle = Excel.XlLineStyle.xlContinuous
                                    .Weight = Excel.XlBorderWeight.xlMedium
                                    .ColorIndex = Excel.Constants.xlAutomatic
                                End With
                                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                                    .LineStyle = Excel.XlLineStyle.xlContinuous
                                    .Weight = Excel.XlBorderWeight.xlMedium
                                    .ColorIndex = Excel.Constants.xlAutomatic
                                End With
                                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                                    .LineStyle = Excel.XlLineStyle.xlContinuous
                                    .Weight = Excel.XlBorderWeight.xlMedium
                                    .ColorIndex = Excel.Constants.xlAutomatic
                                End With
                                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                                    .LineStyle = Excel.XlLineStyle.xlContinuous
                                    .Weight = Excel.XlBorderWeight.xlMedium
                                    .ColorIndex = Excel.Constants.xlAutomatic
                                End With

                                currentColumn += 1

                                ' только LimitControls контролов в строке
                                If J Mod LimitControls = 0 AndAlso J <> controlsCount Then
                                    currentColumn = 2
                                    currentRow += 2
                                End If
                            End If
                        Next
                        currentRow += 3
                    End If
                Next I

                ' --- Измеренные --------------------------------------------------
                colorIndex = 4 'Зеленый'  33 'синий
                Dim resultMeasurementRows = From row In varProjectManager.MeasurementDataTable Select row

                For Each itemDataRow In resultMeasurementRows
                    MeasurementRows.Add(itemDataRow)
                Next

                PrintParameters(excelApp, MeasurementRows, TypeParameter.Measurement, TitleMeasurement)

                ' --- Приведение  -------------------------------------------------
                colorIndex = 8 'голубой
                Dim resultCastRows = From row In varProjectManager.CalculatedDataTable
                                     Where row.ПриведеныйПараметр = True
                                     Select row

                For Each itemDataRow In resultCastRows
                    PhysicalAndCastRows.Add(itemDataRow)
                Next

                PrintParameters(excelApp, PhysicalAndCastRows, TypeParameter.PhysicalCast, TitlePhysicalAndCast)

                ' --- Пересчет ----------------------------------------------------
                colorIndex = 45 'оранжевый
                Dim resultConvertingRows = From row In varProjectManager.CalculatedDataTable
                                           Where row.ПриведеныйПараметр = False
                                           Select row

                For Each itemDataRow In resultConvertingRows
                    ConvertingRows.Add(itemDataRow)
                Next

                PrintParameters(excelApp, ConvertingRows, TypeParameter.Converting, TitleConverting)

                ' оставить позицию курсора в конце
                .Range(.Cells(currentRow, 1), .Cells(currentRow, 1)).Select()
                ' колонтитулы если есть принтер
                With .ActiveSheet.PageSetup
                    .LeftHeader = "Производственный комплекс ""Салют"" АО ""ОДК"", цех №6"
                    .CenterHeader = "Подсчет параметров"
                    .RightHeader = "&D  &T"
                    .LeftFooter = ""
                    .CenterFooter = "&P из &N"
                    .RightFooter = ""
                    .LeftMargin = .Application.InchesToPoints(0.78740157480315)
                    .RightMargin = .Application.InchesToPoints(0.78740157480315)
                    .TopMargin = .Application.InchesToPoints(0.984251968503937)
                    .BottomMargin = .Application.InchesToPoints(0.984251968503937)
                    .HeaderMargin = .Application.InchesToPoints(0.511811023622047)
                    .FooterMargin = .Application.InchesToPoints(0.511811023622047)
                    .PrintHeadings = False
                    .PrintGridlines = False
                    .PrintComments = Excel.XlPrintLocation.xlPrintNoComments
                    '.PrintQuality = 600
                    .CenterHorizontally = False
                    .CenterVertically = False
                    .Orientation = Excel.XlPageOrientation.xlPortrait
                    .Draft = False
                    .PaperSize = Excel.XlPaperSize.xlPaperA4
                    .FirstPageNumber = Excel.Constants.xlAutomatic
                    .Order = Excel.XlOrder.xlDownThenOver
                    .BlackAndWhite = False
                    .Zoom = 100
                    .PrintErrors = Excel.XlPrintErrors.xlPrintErrorsDisplayed
                End With
            End With
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Работа с листом Excel", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            With excelApp
                For I As Integer = .Sheets.Count To 2 Step -1
                    .Sheets(I).Select()
                    .ActiveWindow.SelectedSheets.Delete()
                Next

                With .ActiveWindow
                    .DisplayGridlines = False
                    .DisplayHeadings = False
                End With

                .Application.ScreenUpdating = True

                If isPrintSheetExcel Then
                    PrintSheetExcel(excelApp) ' печать
                Else
                    '.ActiveWorkbook.SaveAs(Filename:=PathTemplateCalculateKT,
                    '    FileFormat:=Excel.XlWindowState.xlNormal, Password:="", WriteResPassword:="",
                    '    ReadOnlyRecommended:=False, CreateBackup:=False)

                    'If Me.FileFormat = Excel.XlFileFormat.xlWorkbookNormal Then
                    '    Me.SaveAs(Me.Path & "\XMLCopy.xls", _
                    '        Excel.XlFileFormat.xlXMLSpreadsheet, _
                    '        AccessMode:=Excel.XlSaveAsAccessMode.xlNoChange)
                    'End If

                    ' записать созданный файл
                    Dim templateCalculateKT As New FileInfo(pathExcelProtocolCalculateKT)
                    If templateCalculateKT.Exists Then templateCalculateKT.Delete()

                    .ActiveWorkbook.SaveAs(Filename:=pathExcelProtocolCalculateKT, FileFormat:=Excel.XlFileFormat.xlOpenXMLWorkbook, CreateBackup:=False)
                End If

                .ActiveWorkbook.Close(SaveChanges:=False)
                .Quit()
            End With
        End Try

        excelApp = Nothing
        GC.Collect()
    End Sub

    ''' <summary>
    ''' Обработка Собранных Параметров
    ''' </summary>
    ''' <param name="excelApp"></param>
    ''' <param name="DataRowParameters"></param>
    ''' <param name="inTypeParameter"></param>
    Private Sub PrintParameters(ByRef excelApp As Excel.Application,
                                ByRef DataRowParameters As List(Of DataRow),
                                ByVal inTypeParameter As TypeParameter,
                                InTitle As String)
        Dim J As Integer
        Dim rowCount As Integer = DataRowParameters.Count ' Количество Параметров Подсчета

        If rowCount > 0 Then
            With excelApp
                currentColumn = OfsetExelColunm
                startBlock = currentRow
                ' заголовок текущего блока
                .Range(.Cells(currentRow, currentColumn), .Cells(currentRow, currentColumn + LimitControls - 1)).Select()
                With .Selection
                    .HorizontalAlignment = Excel.Constants.xlCenter
                    .VerticalAlignment = Excel.Constants.xlCenter
                    .WrapText = False
                    .Orientation = 0
                    .AddIndent = False
                    .IndentLevel = 0
                    .ShrinkToFit = False
                    .ReadingOrder = Excel.Constants.xlContext
                    .MergeCells = True
                End With
                With .Selection.Font
                    .Name = "Arial"
                    .FontStyle = "полужирный"
                    .Size = 10
                    .Strikethrough = False
                    .Superscript = False
                    .Subscript = False
                    .OutlineFont = False
                    .Shadow = False
                    .Underline = Excel.XlUnderlineStyle.xlUnderlineStyleNone
                    .ColorIndex = Excel.Constants.xlAutomatic
                End With
                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                    .LineStyle = Excel.XlLineStyle.xlContinuous
                    .Weight = Excel.XlBorderWeight.xlThin
                    .ColorIndex = Excel.Constants.xlAutomatic
                End With
                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                    .LineStyle = Excel.XlLineStyle.xlContinuous
                    .Weight = Excel.XlBorderWeight.xlThin
                    .ColorIndex = Excel.Constants.xlAutomatic
                End With
                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                    .LineStyle = Excel.XlLineStyle.xlContinuous
                    .Weight = Excel.XlBorderWeight.xlThin
                    .ColorIndex = Excel.Constants.xlAutomatic
                End With
                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                    .LineStyle = Excel.XlLineStyle.xlContinuous
                    .Weight = Excel.XlBorderWeight.xlThin
                    .ColorIndex = Excel.Constants.xlAutomatic
                End With
                .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.Constants.xlNone
                With .Selection.Interior
                    '            .ColorIndex = 4
                    .Pattern = Excel.Constants.xlSolid
                    .PatternColorIndex = Excel.Constants.xlAutomatic
                End With
                ' цвет и заголовок
                .Selection.Interior.ColorIndex = colorIndex
                .ActiveCell.FormulaR1C1 = InTitle

                PrintTitleParameterValueUnit(excelApp)
                currentColumn = 3
                currentRow = indexRowValue

                For Each dataRowParameter As DataRow In DataRowParameters
                    J += 1
                    ' Имя параметра
                    .Range(.Cells(currentRow, currentColumn), .Cells(currentRow, currentColumn)).Select()
                    With .Selection
                        .HorizontalAlignment = Excel.Constants.xlCenter
                        .VerticalAlignment = Excel.Constants.xlTop
                        .WrapText = True
                        .Orientation = 0
                        .AddIndent = False
                        .IndentLevel = 0
                        .ShrinkToFit = False
                        .ReadingOrder = Excel.Constants.xlContext
                        .MergeCells = False
                    End With
                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Interior
                        .ColorIndex = 15
                        .Pattern = Excel.Constants.xlSolid
                        .PatternColorIndex = Excel.Constants.xlAutomatic
                    End With

                    ' ИмяПараметра
                    .ActiveCell.FormulaR1C1 = dataRowParameter("ИмяПараметра")
                    With .ActiveCell.Characters(Start:=1, Length:=LimitControls - 1).Font
                        .Name = "Arial"
                        .FontStyle = "обычный"
                        .Size = 10
                        .Strikethrough = False
                        .Superscript = False
                        .Subscript = False
                        .OutlineFont = False
                        .Shadow = False
                        .Underline = Excel.XlUnderlineStyle.xlUnderlineStyleNone
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    '           иэмеренное значение
                    .Range(.Cells(currentRow + 1, currentColumn), .Cells(currentRow + 1, currentColumn)).Select()
                    With .Selection
                        .HorizontalAlignment = Excel.Constants.xlCenter
                        .VerticalAlignment = Excel.Constants.xlCenter
                        .WrapText = False
                        .Orientation = 0
                        .AddIndent = False
                        .IndentLevel = 0
                        .ShrinkToFit = False
                        .ReadingOrder = Excel.Constants.xlContext
                        .MergeCells = False
                    End With
                    With .Selection.Font
                        .Name = "Arial"
                        .FontStyle = "полужирный"
                        .Size = 10
                        .Strikethrough = False
                        .Superscript = False
                        .Subscript = False
                        .OutlineFont = False
                        .Shadow = False
                        .Underline = Excel.XlUnderlineStyle.xlUnderlineStyleNone
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Interior
                        .ColorIndex = 36
                        .Pattern = Excel.Constants.xlSolid
                        .PatternColorIndex = Excel.Constants.xlAutomatic
                    End With

                    .ActiveCell.FormulaR1C1 = GetToDoubleRCForExcel(currentRow + 1, currentColumn)

                    ' размерность измеренной величины
                    .Range(.Cells(currentRow + 2, currentColumn), .Cells(currentRow + 2, currentColumn)).Select()

                    With .Selection
                        .HorizontalAlignment = Excel.Constants.xlCenter
                        .VerticalAlignment = Excel.Constants.xlCenter
                        .WrapText = False
                        .Orientation = 0
                        .AddIndent = False
                        .IndentLevel = 0
                        .ShrinkToFit = False
                        .ReadingOrder = Excel.Constants.xlContext
                        .MergeCells = False
                    End With
                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With

                    If inTypeParameter = TypeParameter.Measurement Then
                        .ActiveCell.FormulaR1C1 = dataRowParameter("РазмерностьВходная")
                    Else
                        .ActiveCell.FormulaR1C1 = dataRowParameter("РазмерностьВыходная")
                    End If

                    ' форматировать выделенный блок
                    .Range(.Cells(currentRow, currentColumn), .Cells(currentRow + 2, currentColumn)).Select()

                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With
                    With .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
                        .LineStyle = Excel.XlLineStyle.xlContinuous
                        .Weight = Excel.XlBorderWeight.xlThin
                        .ColorIndex = Excel.Constants.xlAutomatic
                    End With

                    currentColumn = currentColumn + 1
                    ' не более LimitControls параметров в строке
                    If J Mod (LimitControls - 1) = 0 AndAlso J <> rowCount Then
                        currentRow += 2
                        PrintTitleParameterValueUnit(excelApp)
                        currentColumn = 3
                        currentRow = indexRowValue
                    End If
                Next

                currentRow += 3
                ' пустая строка
                .Range(.Cells(currentRow, 2), .Cells(currentRow, LimitControls + 1)).Select()
                .Selection.Merge()
                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                    .LineStyle = Excel.XlLineStyle.xlContinuous
                    .Weight = Excel.XlBorderWeight.xlThin
                    .ColorIndex = Excel.Constants.xlAutomatic
                End With
                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                    .LineStyle = Excel.XlLineStyle.xlContinuous
                    .Weight = Excel.XlBorderWeight.xlThin
                    .ColorIndex = Excel.Constants.xlAutomatic
                End With
                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                    .LineStyle = Excel.XlLineStyle.xlContinuous
                    .Weight = Excel.XlBorderWeight.xlThin
                    .ColorIndex = Excel.Constants.xlAutomatic
                End With
                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                    .LineStyle = Excel.XlLineStyle.xlContinuous
                    .Weight = Excel.XlBorderWeight.xlThin
                    .ColorIndex = Excel.Constants.xlAutomatic
                End With
                .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.Constants.xlNone

                ' форматировать блок
                .Range(.Cells(startBlock, 2), .Cells(currentRow, LimitControls + 1)).Select()
                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                    .LineStyle = Excel.XlLineStyle.xlContinuous
                    .Weight = Excel.XlBorderWeight.xlMedium
                    .ColorIndex = Excel.Constants.xlAutomatic
                End With
                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                    .LineStyle = Excel.XlLineStyle.xlContinuous
                    .Weight = Excel.XlBorderWeight.xlMedium
                    .ColorIndex = Excel.Constants.xlAutomatic
                End With
                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                    .LineStyle = Excel.XlLineStyle.xlContinuous
                    .Weight = Excel.XlBorderWeight.xlMedium
                    .ColorIndex = Excel.Constants.xlAutomatic
                End With
                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                    .LineStyle = Excel.XlLineStyle.xlContinuous
                    .Weight = Excel.XlBorderWeight.xlMedium
                    .ColorIndex = Excel.Constants.xlAutomatic
                End With

                currentRow += 1
            End With
        End If
    End Sub

    ''' <summary>
    ''' Заголовок Параметр Значение Размерность
    ''' </summary>
    ''' <param name="excelApp"></param>
    Private Sub PrintTitleParameterValueUnit(ByRef excelApp As Excel.Application)
        currentColumn = OfsetExelColunm
        currentRow += 1
        indexRowValue = currentRow

        With excelApp
            .Range(.Cells(currentRow, currentColumn), .Cells(currentRow, currentColumn)).Select()
            .ActiveCell.FormulaR1C1 = "Параметр"
            With .Selection
                .HorizontalAlignment = Excel.Constants.xlCenter
                .VerticalAlignment = Excel.Constants.xlCenter
                .WrapText = False
                .Orientation = 0
                .AddIndent = False
                .IndentLevel = 0
                .ShrinkToFit = False
                .ReadingOrder = Excel.Constants.xlContext
                .MergeCells = False
            End With

            currentRow += 1
            .Range(.Cells(currentRow, currentColumn), .Cells(currentRow, currentColumn)).Select()
            .ActiveCell.FormulaR1C1 = "Значение"

            currentRow += 1
            .Range(.Cells(currentRow, currentColumn), .Cells(currentRow, currentColumn)).Select()
            .ActiveCell.FormulaR1C1 = "Размерность"

            .Range(.Cells(currentRow - 2, currentColumn), .Cells(currentRow, currentColumn)).Select()
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
        End With
    End Sub

    ''' <summary>
    ''' Печать листов Excel
    ''' </summary>
    ''' <param name="excelApp"></param>
    Private Sub PrintSheetExcel(ByRef excelApp As Excel.Application)
        Dim dlg As New PrintDialog
        Dim pd As PrintDocument = New PrintDocument

        dlg.Document = pd

        If dlg.ShowDialog() = DialogResult.OK Then
            Dim printerName As String = dlg.PrinterSettings.PrinterName ' "\\PENTIUM4\HP DeskJet 1220C (Ne01:)"

            If dlg.PrinterSettings.IsValid Then
                Try
                    With excelApp
                        .Sheets("Лист1").Select()
                        .ActiveWindow.SelectedSheets.PrintOut(Copies:=1, ActivePrinter:=printerName, Collate:=True)
                    End With
                Catch ex As Exception
                    Const caption As String = "Печать"
                    Dim text As String = ex.ToString
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    'RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                    If MessageBox.Show("Повторить?", "Печать", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                        Exit Sub
                    Else
                        PrintSheetExcel(excelApp)
                    End If
                End Try
            Else
                MessageBox.Show("Принтер не установлен.", "Печать", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub
#End Region

End Class

'Private Sub Test()
'    '' работает
'    'For row As Integer = 1 To 5
'    '    Dim newRow As DataGridViewRow = New DataGridViewRow()
'    '    newRow.HeaderCell.Value = "Row" & row.ToString
'    '    newRow.CreateCells(mDataGridViewReportKT)
'    '    newRow.SetValues(New Object() {1, 2, 3, 4, 5, 6, 7, 8})
'    '    'newRow.SetValues({"Meatloaf", "Main Dish", "ground beef", New DateTime(2000, 3, 23), "*"})
'    '    newRow.ReadOnly = True
'    '    newRow.Cells(0).ReadOnly = False
'    '    newRow.Cells(0).Style.SelectionBackColor = Color.Aqua
'    '    mDataGridViewReportKT.Rows.Add(newRow)
'    '    '' Добавляем строку, указывая значения колонок поочереди слева направо
'    '    'mDataGridViewReportKT.Rows.Add(1, 2, 3, 4, 5, 6, 7, 8)
'    '    mDataGridViewReportKT.Rows(mDataGridViewReportKT.Rows.Count - 1).ReadOnly = True
'    'Next


'    ' работает
'    'For row As Integer = 1 To 5
'    '    ' Добавляем строку, указывая значения каждой ячейки по имени (можно использовать индекс 0, 1, 2 вместо имен)
'    '    Dim newRow As DataGridViewRow = New DataGridViewRow()
'    '    newRow.CreateCells(mDataGridViewReportKT)
'    '    mDataGridViewReportKT.Rows.Add(newRow)

'    '    Dim currentRow As Integer = mDataGridViewReportKT.Rows.Count - 1 ' вместе с строкой ввода новой записи
'    '    'Dim currentRow As Integer = mDataGridViewReportKT.Rows.Add(newRow)

'    '    For col As Integer = 0 To mDataGridViewReportKT.Columns.Count - 1
'    '        'newRow.Cells(col).Value = $"row={row}; column={col}" ' выдает исключения

'    '        Dim newCell As DataGridViewTextBoxCellEx = CType(mDataGridViewReportKT(col, currentRow), DataGridViewTextBoxCellEx)
'    '        'Dim newCell As DataGridViewTextBoxCellEx = CType(newRow.Cells(col), DataGridViewTextBoxCellEx) ' выдает исключения
'    '        newCell.Value = $"row={row}; column={col}"
'    '    Next
'    'Next

'    '' работает
'    'For row As Integer = 1 To 5
'    '    ' Добавляем строку, указывая значения каждой ячейки по имени (можно использовать индекс 0, 1, 2 вместо имен)
'    '    mDataGridViewReportKT.Rows.Add()

'    '    Dim currentRow As Integer = mDataGridViewReportKT.Rows.Count - 1 ' вместе с строкой ввода новой записи
'    '    Dim column As Integer = 1

'    '    mDataGridViewReportKT("Column" + 1.ToString, currentRow).Value = $"row={row}; column={column}" : column += 1
'    '    mDataGridViewReportKT("Column" + 2.ToString, currentRow).Value = $"row={row}; column={column}" : column += 1
'    '    mDataGridViewReportKT("Column" + 3.ToString, currentRow).Value = $"row={row}; column={column}" : column += 1
'    '    mDataGridViewReportKT("Column" + 4.ToString, currentRow).Value = $"row={row}; column={column}" : column += 1
'    '    mDataGridViewReportKT("Column" + 5.ToString, currentRow).Value = $"row={row}; column={column}" : column += 1
'    '    mDataGridViewReportKT("Column" + 6.ToString, currentRow).Value = $"row={row}; column={column}" : column += 1
'    '    mDataGridViewReportKT("Column" + 7.ToString, currentRow).Value = $"row={row}; column={column}" : column += 1
'    '    mDataGridViewReportKT("Column" + 8.ToString, currentRow).Value = $"row={row}; column={column}" : column += 1
'    '    'mDataGridViewReportKT.Rows(currentRow).ReadOnly = True
'    'Next

'    '' работает
'    'mDataGridViewReportKT.RowCount = 5

'    'For row As Integer = 0 To mDataGridViewReportKT.Rows.Count - 1
'    '    For col As Integer = 0 To mDataGridViewReportKT.Columns.Count - 1
'    '        mDataGridViewReportKT(col, row).Value = $"row={row}; column={col}"
'    '    Next
'    'Next

'    'Dim cell As DataGridViewTextBoxCellEx = CType(mDataGridViewReportKT(1, 1), DataGridViewTextBoxCellEx)
'    'cell.ColumnSpan = 3
'    'cell.RowSpan = 2
'    'cell.ReadOnly = True

'    'mDataGridViewReportKT.Rows(1).DividerHeight = 2

'    'Dim refcell = CType(mDataGridViewReportKT(1, 5), DataGridViewTextBoxCellEx)
'    'refcell.ColumnSpan = 3

'    For Each itemCellStyle As KeyValuePair(Of TypeCellStyle, SpanDataGridViewCellStyle) In DictionaryOfCellStyle
'        Dim newRow As DataGridViewRow = GetNewRow()

'        'Dim currentRow As Integer = mDataGridViewReportKT.Rows.Count - 1 ' вместе с строкой ввода новой записи
'        Dim newCell As DataGridViewTextBoxCellEx = CType(mDataGridViewReportKT(0, currentRow), DataGridViewTextBoxCellEx)

'        newCell.Style = itemCellStyle.Value.TypeDataGridViewCellStyle.Clone
'        newCell.ReadOnly = itemCellStyle.Value.IsReadOnly
'        newCell.ColumnSpan = itemCellStyle.Value.ColumnSpan
'        newCell.RowSpan = itemCellStyle.Value.RowSpan
'        'newCell.Value = CInt(itemCellStyle.Key)

'        Select Case itemCellStyle.Key
'            Case TypeCellStyle.StageName
'                newCell.Value = " 1 - имя этапа"
'            Case TypeCellStyle.DescriptionStage
'                newCell.Value = " 2 - описание этапа"
'            Case TypeCellStyle.ControlText
'                newCell.Value = " 3 - текст контрола"
'            Case TypeCellStyle.ControlValue
'                newCell.Value = " 4 - значение контрола ' .ЗначениеПользователя"
'            Case TypeCellStyle.MeasurementTitle
'                newCell.Value = " 5 - заголовок Исходные данные (измеренная информация)"
'            Case TypeCellStyle.PhysicalCastTitle
'                newCell.Value = " 6 - заголовок Физические и приведенные параметр"
'            Case TypeCellStyle.ConvertingTitle
'                newCell.Value = " 7 - заголовок Пересчитанные параметры"
'            Case TypeCellStyle.EmptyTitle
'                newCell.Value = " 8 - заголовок пустой"
'            Case TypeCellStyle.Caption
'                newCell.Value = " 9- заголовок: Параметр, Значение, Размерность"
'            Case TypeCellStyle.ParameterName
'                newCell.Value = " 10- имя параметра (текст контрола)"
'            Case TypeCellStyle.MeasurementParameterValue
'                newCell.Value = 11.001 '" 11- значение параметра"
'                newCell.Tag = True
'            Case TypeCellStyle.PhysicalCastParameterValue
'                newCell.Value = 12.001 ' " 12- значение параметра"
'                newCell.Tag = True
'            Case TypeCellStyle.ConvertingParameterValue
'                newCell.Value = 13.001 ' " 13- значение параметра"
'                newCell.Tag = True
'            Case TypeCellStyle.ParameterUnit
'                newCell.Value = " 14- значение размерности"
'            Case TypeCellStyle.ColumnsRowsEmptyTitle
'                Dim currentColumn As Integer = 1
'                Dim newEmptyCell As DataGridViewTextBoxCellEx = CType(mDataGridViewReportKT(currentColumn, currentRow - 3), DataGridViewTextBoxCellEx)

'                newEmptyCell.Style = itemCellStyle.Value.TypeDataGridViewCellStyle.Clone
'                newEmptyCell.ReadOnly = itemCellStyle.Value.IsReadOnly
'                newEmptyCell.ColumnSpan = (mDataGridViewReportKT.Columns.Count - 1) - currentColumn
'                newEmptyCell.RowSpan = 3
'                newEmptyCell.Value = " 15- пустой квадрат"
'        End Select

'        'Dim colunm = mDataGridViewReportKT.Columns.GetLastColumn(DataGridViewElementStates.Displayed, DataGridViewElementStates.Visible)
'        'colunm.Index
'    Next

'    specialCells = New Boolean(mDataGridViewReportKT.Columns.Count - 1, mDataGridViewReportKT.Rows.Count - 1) {}
'    For row As Integer = 0 To mDataGridViewReportKT.Rows.Count - 1
'        For col As Integer = 0 To mDataGridViewReportKT.Columns.Count - 1
'            specialCells(col, row) = If(mDataGridViewReportKT(col, row)?.Tag, False)
'        Next
'    Next
'End Sub

'Private dt_source As DataTable

'''' <summary>
'''' Создать колонки и загрузить данные
'''' </summary>
'Private Sub PopulateDataGridView()
'    ' Установить имена колонок.
'    mDataGridViewReportKT.ColumnCount = 5

'    mDataGridViewReportKT.Columns(0).Name = "Recipe"
'    mDataGridViewReportKT.Columns(1).Name = "Category"
'    mDataGridViewReportKT.Columns(2).Name = "Main Ingredients"
'    mDataGridViewReportKT.Columns(3).Name = "Last Prepared"
'    mDataGridViewReportKT.Columns(4).Name = "Rating"
'    mDataGridViewReportKT.Columns(4).DefaultCellStyle.Font = New Font(mDataGridViewReportKT.DefaultCellStyle.Font, FontStyle.Italic)

'    ' заполнить строки
'    'Dim row1() As As Object = {"Meatloaf", "Main Dish", "ground beef", New DateTime(2000, 3, 23), "*"}
'    Dim row1 As DataGridViewRow = New DataGridViewRow()
'    row1.CreateCells(mDataGridViewReportKT)
'    row1.SetValues({"Meatloaf", "Main Dish", "ground beef", New DateTime(2000, 3, 23), "*"})

'    ' Добавить строки
'    'Dim rows() As DataGridViewRow = {row1} ', row2, row3, row4, row5, row6}

'    'For Each rowArray As Object In rows
'    '    mDataGridViewReportKT.Rows.Add(rowArray)
'    'Next

'    mDataGridViewReportKT.Rows.AddRange({row1})

'    ' Подстроить высоту строк так чтобы всё содержимое было видимо
'    mDataGridViewReportKT.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders)

'    ' задать Format свойство "Last Prepared" колонки 
'    ' как DateTime чтобы форматировалось как "Month, Year".
'    mDataGridViewReportKT.Columns("Last Prepared").DefaultCellStyle.Format = "y"

'    ' установить крупный шрифт "Ratings" колонки. 
'    Dim font As New Font(mDataGridViewReportKT.DefaultCellStyle.Font.FontFamily, 25, FontStyle.Bold)
'    Try
'        mDataGridViewReportKT.Columns("Rating").DefaultCellStyle.Font = font
'    Finally
'        font.Dispose()
'    End Try
'End Sub

'''' <summary>
'''' Создаёт пустой шаблон отчёта и отображает его но сетке
'''' </summary>
'Public Sub GreateEmptyReport(ByRef StageConstNames() As String,
'                             ByRef StageNames() As String,
'                             ByVal varProjectManager As ProjectManager)
'    mDataGridViewReportKT.Columns.Clear()

'    For I As Integer = 1 To LimitControls
'        Dim newColumn As DataGridViewTextBoxColumnEx = New DataGridViewTextBoxColumnEx()
'        mDataGridViewReportKT.Columns.AddRange(New DataGridViewColumn() {newColumn})
'        newColumn.HeaderText = "Column" & I.ToString
'        newColumn.Name = "Column" & I.ToString
'    Next


'    mDataGridViewReportKT.RowCount = 10

'    Dim cell As DataGridViewTextBoxCellEx = CType(mDataGridViewReportKT(1, 1), DataGridViewTextBoxCellEx)
'    cell.ColumnSpan = 3
'    cell.RowSpan = 2
'    mDataGridViewReportKT.Rows(1).DividerHeight = 2

'    'Dim refcell = CType(mDataGridViewReportKT(1, 5), DataGridViewTextBoxCellEx)
'    'refcell.ColumnSpan = 3


'    mDataGridViewReportKT.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader)
'    mDataGridViewReportKT.FirstDisplayedCell = mDataGridViewReportKT.Rows(3).Cells(0)
'    ShowRepotrNewKT()
'End Sub


'Private Sub BindingWithDataTable()
'    ' Здесь лист преобразуется в datatable и источник datagridview обновляется.
'    Try
'        'dt_source = New DataTable("Sheet1")

'        'Dim totalRows As Integer = oSheet.Dimension.End.Row
'        Dim totalCols As Integer = LimitControls
'        'Dim dr As DataRow = Nothing

'        'For I As Integer = 1 To totalCols
'        '    dt_source.Columns.Add("Column_" + I.ToString()) ' добавление именнованой колонки
'        'Next

'        'For i As Integer = 1 To totalRows
'        '    dr = dt_source.Rows.Add()
'        '    For j As Integer = 1 To totalCols
'        '        dr(j - 1) = oSheet.Cells(i, j).Value
'        '    Next
'        'Next

'        For column = 0 To mDataGridViewReportKT.ColumnCount - 1 ' COLUMNS OF dt_source
'            dt_source.Columns.Add(mDataGridViewReportKT.Columns(column).Name.ToString)
'        Next

'        For row As Integer = 0 To 10 ' mDataGridViewReportKT.Rows.Count - 1 ' FILL dt_source WITH DATAGRIDVIEW
'            Dim newDataRow As DataRow = dt_source.NewRow

'            For column As Integer = 0 To totalCols - 1 'mDataGridViewReportKT.ColumnCount - 1
'                'Try
'                newDataRow(dt_source.Columns(column).ColumnName.ToString) = $"row={row}; column={column}" '  mDataGridViewReportKT.Rows(row).Cells(column).Value.ToString
'                'Catch ex As Exception
'                'End Try
'            Next

'            dt_source.Rows.Add(newDataRow)
'        Next

'        dt_source.AcceptChanges()

'        If dt_source.Rows.Count = 0 Then
'            MessageBox.Show("Нет ни каких данных в выделенном листе.")
'        End If

'        mDataGridViewReportKT.DataSource = dt_source
'    Catch lException As Exception
'        MessageBox.Show(lException.Message)
'    End Try
'End Sub


'''' <summary>
'''' Загрузить значения последней КТ или после пересчёта КТ
'''' </summary>
'Private Sub ShowRepotrNewKT()
'    ' А теперь простой пройдемся циклом по всем ячейкам
'    For i As Integer = 0 To mDataGridViewReportKT.Rows.Count - 1
'        For j As Integer = 0 To mDataGridViewReportKT.Columns.Count - 1
'            Dim o As Object = mDataGridViewReportKT(j, i).Value
'        Next
'    Next

'    Dim data As String

'    For i As Integer = 0 To mDataGridViewReportKT.Rows.Count - 1 ' - 1
'        If mDataGridViewReportKT.Rows(i).Cells(1).Value IsNot Nothing Then data = mDataGridViewReportKT.Rows(i).Cells(1).Value.ToString()
'    Next

'    For Each row As DataGridViewRow In mDataGridViewReportKT.Rows
'        If row.IsNewRow Then Continue For

'        'Debug.WriteLine(row.Index)

'        For i As Integer = 0 To row.Cells.Count - 1
'            If row.Cells(i).Value IsNot Nothing Then data = row.Cells(i).Value.ToString()
'        Next
'    Next

'    '    For column As Integer = 0 To totalCols - 1 'mDataGridViewReportKT.ColumnCount - 1
'    '        'Try
'    '        newDataRow(dt_source.Columns(column).ColumnName.ToString) = $"row={row}; column={column}" '  mDataGridViewReportKT.Rows(row).Cells(column).Value.ToString
'    '        'Catch ex As Exception
'    '        'End Try
'    '    Next
'End Sub

'Private Sub mDataGridViewReportKT_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles mDataGridViewReportKT.CellFormatting
'    If e IsNot Nothing Then
'        If mDataGridViewReportKT.Columns(e.ColumnIndex).Name = "Column_11" Then
'            If e.Value IsNot Nothing Then
'                Try
'                    e.Value = DateTime.Parse(e.Value.ToString()).ToLongDateString()
'                    e.FormattingApplied = True
'                Catch ex As FormatException
'                    Console.WriteLine("{0} is not a valid date.", e.Value.ToString())
'                End Try
'            End If
'        End If
'    End If

'    If e.ColumnIndex = mDataGridViewReportKT.Columns("Column1").Index AndAlso e.Value IsNot Nothing Then
'        Select Case e.Value.ToString().Length
'            Case 1
'                e.CellStyle.SelectionForeColor = Color.Red
'                e.CellStyle.ForeColor = Color.Red
'            Case 2
'                e.CellStyle.SelectionForeColor = Color.Yellow
'                e.CellStyle.ForeColor = Color.Yellow
'            Case 3
'                e.CellStyle.SelectionForeColor = Color.Green
'                e.CellStyle.ForeColor = Color.Green
'            Case 4
'                e.CellStyle.SelectionForeColor = Color.Blue
'                e.CellStyle.ForeColor = Color.Blue
'        End Select
'    End If
'End Sub
