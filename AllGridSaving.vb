'Imports System
'Imports System.Configuration
'Imports System.Data
'Imports System.Windows.Forms


'''' <summary>
'''' Метод расширения для класса System.Windows.Forms.Form 
'''' реализующий запоминание местоположения и размеров формы 
'''' и ширины колонок всех гридов на форме.
'''' Значения запоминаются по именом формы, грида и колонки;
'''' Для использования метода расширения во всех формах нужно здесь поменять 
'''' namespace Nordwind2 на namespace ИмяТекущегоПроекта
'''' Ширина запоминается по имени колонки;
'''' Для включения механизма сохранения и восстановления достаточно
'''' в конструктор формы добавить строку
''''             this.SavingOn();
'''' </summary>
'Public NotInheritable Class AllGridSaving
'    Private Sub New()
'    End Sub
'    ''' <summary>
'    ''' Класс-оболочка для пользовательских параметров приложения.
'    ''' его использование ограничено статическим классом eeSaveCW
'    ''' </summary>
'    Private Class AllSizeUserSettings
'        Inherits ApplicationSettingsBase
'        ''' <summary>
'        ''' Единственный параметр с именем dsAllFG типа DataSet
'        ''' для хранения настроек всех форм и гридов
'        ''' </summary>
'        <UserScopedSetting()> _
'        <DefaultSettingValue(Nothing)> _
'        Public Property dsAllStageGrigUserSettings() As DataSet
'            Get
'                Return DirectCast(Me("dsAllStageGrigUserSettings"), DataSet)
'            End Get
'            Set(ByVal value As DataSet)
'                Me("dsAllStageGrigUserSettings") = DirectCast(value, DataSet)
'            End Set
'        End Property
'    End Class
'    ''' <summary>
'    ''' Хранение настроек в статическом классе
'    ''' </summary>
'    Private Shared _settingsSaver As New AllSizeUserSettings()
'    ''' <summary>
'    ''' Сброс всех сохранённых настроек
'    ''' </summary>
'    Public Shared Sub ResetALL()
'        _settingsSaver.dsAllStageGrigUserSettings = Nothing
'    End Sub

'    ''' <summary>
'    ''' Формируем имя таблицы (DataTable) из имён Формы и Грида
'    ''' </summary>
'    ''' <param name="UserForm">Форма</param>
'    ''' <param name="UserGrid">Грид</param>
'    ''' <returns></returns>
'    Private Shared Function GridTableName(ByVal UserForm As Form, ByVal UserGrid As DataGridView) As String
'        Return UserForm.Name & "__" & UserGrid.Name
'    End Function

'    '''' <summary>
'    '''' Создание таблицы для хранения ширины колонок
'    '''' </summary>
'    '''' <param name="TableName">Имя таблицы</param>
'    '''' <returns>Готовая пустая таблица</returns>
'    'Private Shared Function MakeColWidthTable(ByVal TableName As String) As DataTable
'    '    Dim dt As New DataTable(TableName)
'    '    Dim column As New DataColumn()
'    '    column.DataType = System.Type.[GetType]("System.String")
'    '    column.ColumnName = "GridColumnName"
'    '    dt.Columns.Add(column)
'    '    column = New DataColumn()
'    '    column.DataType = System.Type.[GetType]("System.Int32")
'    '    column.ColumnName = "GridColumnWidth"
'    '    dt.Columns.Add(column)
'    '    Return dt
'    'End Function


'    Private Shared Function MakeColNameTypeTable(ByVal TableName As String) As DataTable
'        Dim dt As New DataTable(TableName)
'        Dim column As New DataColumn()
'        column.DataType = System.Type.[GetType]("System.String")
'        column.ColumnName = "Field"
'        dt.Columns.Add(column)
'        column = New DataColumn()
'        column.DataType = System.Type.[GetType]("System.String")
'        column.ColumnName = "Type"
'        dt.Columns.Add(column)

'        column = New DataColumn()
'        column.DataType = System.Type.[GetType]("System.String")
'        column.ColumnName = "Condition1"
'        dt.Columns.Add(column)

'        column = New DataColumn()
'        column.DataType = System.Type.[GetType]("System.String")
'        column.ColumnName = "Value1"
'        dt.Columns.Add(column)

'        column = New DataColumn()
'        column.DataType = System.Type.[GetType]("System.String")
'        column.ColumnName = "Condition2"
'        dt.Columns.Add(column)

'        column = New DataColumn()
'        column.DataType = System.Type.[GetType]("System.String")
'        column.ColumnName = "Value2"
'        dt.Columns.Add(column)

'        column = New DataColumn()
'        column.DataType = System.Type.[GetType]("System.String")
'        column.ColumnName = "Condition3"
'        dt.Columns.Add(column)

'        column = New DataColumn()
'        column.DataType = System.Type.[GetType]("System.String")
'        column.ColumnName = "Value3"
'        dt.Columns.Add(column)

'        column = New DataColumn()
'        column.DataType = System.Type.[GetType]("System.String")
'        column.ColumnName = "Sort"
'        dt.Columns.Add(column)

'        Return dt
'    End Function


'    '''' <summary>
'    '''' Создание таблицы для размеров и позиции формы
'    '''' </summary>
'    '''' <param name="TableName">Имя (формы)</param>
'    '''' <returns>Готовая пустая таблица</returns>
'    'Private Shared Function MakeFormTable(ByVal TableName As String) As DataTable
'    '    Dim column As DataColumn
'    '    Dim dt As New DataTable(TableName)
'    '    column = New DataColumn()
'    '    column.DataType = System.Type.[GetType]("System.Int32")
'    '    column.ColumnName = "Location_X"
'    '    dt.Columns.Add(column)
'    '    column = New DataColumn()
'    '    column.DataType = System.Type.[GetType]("System.Int32")
'    '    column.ColumnName = "Location_Y"
'    '    dt.Columns.Add(column)
'    '    column = New DataColumn()
'    '    column.DataType = System.Type.[GetType]("System.Int32")
'    '    column.ColumnName = "Height"
'    '    dt.Columns.Add(column)
'    '    column = New DataColumn()
'    '    column.DataType = System.Type.[GetType]("System.Int32")
'    '    column.ColumnName = "Width"
'    '    dt.Columns.Add(column)
'    '    column = New DataColumn()
'    '    column.DataType = System.Type.[GetType]("System.Int32")
'    '    column.ColumnName = "WindowState"
'    '    dt.Columns.Add(column)
'    '    Return dt
'    'End Function


'    '''' <summary>
'    '''' Заполнение таблицы грида размерами колонок
'    '''' </summary>
'    '''' <param name="dt">таблица</param>
'    '''' <param name="grid">грид</param>
'    'Private Shared Sub FillGrideTable(ByVal dt As DataTable, ByVal grid As DataGridView)
'    '    For Each c As DataGridViewColumn In grid.Columns
'    '        Dim r As DataRow = dt.NewRow()
'    '        r("GridColumnName") = c.Name
'    '        r("GridColumnWidth") = c.Width
'    '        dt.Rows.Add(r)
'    '    Next
'    'End Sub

'    Private Shared Sub FillGrideTable(ByVal dt As DataTable, ByVal grid As DataGridView)
'        For Each rowgrid As DataGridViewRow In grid.Rows
'            Dim r As DataRow = dt.NewRow
'            r("Field") = rowgrid.Cells("Field").Value
'            r("Type") = rowgrid.Cells("Field").ValueType 'на самом деле вернуть тип контрола
'            r("Condition1") = rowgrid.Cells("Condition1").Value
'            r("Value1") = rowgrid.Cells("Value1").Value

'            r("Condition2") = rowgrid.Cells("Condition2").Value
'            r("Value2") = rowgrid.Cells("Value2").Value

'            r("Condition3") = rowgrid.Cells("Condition3").Value
'            r("Value3") = rowgrid.Cells("Value3").Value

'            r("Sort") = rowgrid.Cells("Sort").Value
'            dt.Rows.Add(r)
'        Next
'    End Sub

'    '''' <summary>
'    '''' Заполнение таблицы формы
'    '''' </summary>
'    '''' <param name="dt">Таблица</param>
'    '''' <param name="f">Форма</param>
'    'Private Shared Sub FillFormTable(ByVal dt As DataTable, ByVal f As Form)
'    '    Dim r As DataRow = dt.NewRow()
'    '    r("Location_X") = f.Location.X
'    '    r("Location_Y") = f.Location.Y
'    '    r("Height") = f.Size.Height
'    '    r("Width") = f.Size.Width
'    '    r("WindowState") = f.WindowState
'    '    dt.Rows.Add(r)
'    'End Sub


'    '''' <summary>
'    '''' Запись изменений в таблицу формы.
'    '''' Специфика обусловлена сохранением данных при максимального-минимального состояния формы
'    '''' </summary>
'    '''' <param name="dt">таблица</param>
'    '''' <param name="f">форма</param>
'    'Private Shared Sub UpdateFormSizeTable(ByVal dt As DataTable, ByVal f As Form)
'    '    Dim r As DataRow = dt.Rows(0)
'    '    If f.WindowState = FormWindowState.Normal Then
'    '        r("Location_X") = f.Location.X
'    '        r("Location_Y") = f.Location.Y
'    '        r("Height") = f.Size.Height
'    '        r("Width") = f.Size.Width
'    '    End If
'    '    r("WindowState") = f.WindowState
'    'End Sub


'    '''' <summary>
'    '''' Восстановление ширины колонок грида
'    '''' </summary>
'    '''' <param name="dt">Таблица</param>
'    '''' <param name="grid">Грид</param>
'    'Private Shared Sub RestoreGridViewColWidths(ByVal dt As DataTable, ByVal grid As DataGridView)
'    '    For Each r As DataRow In dt.Rows
'    '        Try
'    '            grid.Columns(DirectCast(r("GridColumnName"), String)).Width = CInt(r("GridColumnWidth"))
'    '        Catch ex As Exception
'    '        End Try
'    '    Next
'    'End Sub

'    Private Shared Sub RestoreGridViewRows(ByVal dt As DataTable, ByVal grid As DataGridView)
'        grid.Rows.Clear()

'        For Each r As DataRow In dt.Rows
'            Try
'                'grid.Columns(DirectCast(r("GridColumnName"), String)).Width = CInt(r("GridColumnWidth"))

'                Dim newRow As New DataGridViewRow
'                ' Создаем ячейку типа CheckBox
'                Dim TextCell As New DataGridViewTextBoxCell
'                TextCell.Value = DirectCast(r("Field"), String)
'                'checkCell.Value = True
'                ' Добавляем в качестве первой ячейки новой строки ячейку типа CheckBox
'                newRow.Cells.Add(TextCell)
'                ' Остальные ячейки заполняем ячейками типа TextBox
'                'newRow.Cells.Add(New DataGridViewTextBoxCell())
'                'newRow.Cells(1).Value = "Условие1" & grid.Rows.Count
'                'newRow.Cells.Add(New DataGridViewTextBoxCell())
'                'newRow.Cells(2).Value = "Значение1" & grid.Rows.Count
'                ' эта строчка будет с переключателем в первой колонке

'                'Здесь создается новый тип пользовательской ячейки и в ней уже настройки
'                'TextCell = New DataGridViewTextBoxCell
'                'TextCell.Value = DirectCast(r("Type"), String)
'                'newRow.Cells.Add(TextCell)

'                TextCell = New DataGridViewTextBoxCell
'                TextCell.Value = DirectCast(r("Condition1"), String)
'                newRow.Cells.Add(TextCell)
'                TextCell = New DataGridViewTextBoxCell
'                TextCell.Value = DirectCast(r("Value1"), String)
'                newRow.Cells.Add(TextCell)

'                TextCell = New DataGridViewTextBoxCell
'                TextCell.Value = DirectCast(r("Condition2"), String)
'                newRow.Cells.Add(TextCell)
'                TextCell = New DataGridViewTextBoxCell
'                TextCell.Value = DirectCast(r("Value2"), String)
'                newRow.Cells.Add(TextCell)

'                TextCell = New DataGridViewTextBoxCell
'                TextCell.Value = DirectCast(r("Condition3"), String)
'                newRow.Cells.Add(TextCell)
'                TextCell = New DataGridViewTextBoxCell
'                TextCell.Value = DirectCast(r("Value3"), String)
'                newRow.Cells.Add(TextCell)

'                TextCell = New DataGridViewTextBoxCell
'                TextCell.Value = DirectCast(r("Sort"), String)
'                newRow.Cells.Add(TextCell)

'                grid.Rows.Add(newRow)

'            Catch ex As Exception
'            End Try
'        Next
'    End Sub

'    '''' <summary>
'    '''' Восстановление местоположения и размеры формы
'    '''' </summary>
'    '''' <param name="dt">Таблица</param>
'    '''' <param name="f">Грид</param>
'    'Private Shared Sub RestoreForm(ByVal dt As DataTable, ByVal f As Form)
'    '    Dim r As DataRow = dt.Rows(0)
'    '    Try
'    '        f.Location = New System.Drawing.Point(CInt(r("Location_X")), CInt(r("Location_Y")))
'    '        f.Size = New System.Drawing.Size(CInt(r("Width")), CInt(r("Height")))
'    '        f.WindowState = CType(r("WindowState"), FormWindowState)
'    '    Catch ex As Exception
'    '    End Try
'    'End Sub


'    ''' <summary>
'    ''' Сохранение местоположения и размеры формы и ширина всех колонок всех гридов
'    ''' </summary>
'    Public Shared Sub SaveFormGrid(ByVal sender As Form) ', ByVal e As EventArgs)
'        'Private Shared Sub SaveFormGrid(ByVal sender As Object, ByVal e As EventArgs)
'        Dim senderForm As Form = DirectCast(sender, Form)
'        Dim ds As DataSet
'        If _settingsSaver.dsAllStageGrigUserSettings IsNot Nothing Then
'            ds = _settingsSaver.dsAllStageGrigUserSettings
'        Else
'            ds = New DataSet()
'        End If

'        'If ds.Tables.IndexOf(senderForm.Name) = -1 Then
'        '    Dim dt As DataTable = MakeFormTable(senderForm.Name)
'        '    FillFormTable(dt, senderForm)
'        '    ds.Tables.Add(dt)
'        'Else
'        '    UpdateFormSizeTable(ds.Tables(senderForm.Name), senderForm)
'        'End If

'        For Each c As Control In senderForm.Controls 'здесь надо senderForm.Controls(cKey) key имя панели
'            If TypeOf c Is DataGridView Then
'                Dim grid As DataGridView = DirectCast(c, DataGridView)
'                Dim name As String = GridTableName(senderForm, grid)
'                If ds.Tables.IndexOf(name) > -1 Then
'                    ds.Tables.Remove(name)
'                End If
'                Dim dt As DataTable = MakeColNameTypeTable(name) 'MakeColWidthTable(name)
'                FillGrideTable(dt, grid)
'                ds.Tables.Add(dt)
'            End If
'        Next
'        _settingsSaver.dsAllStageGrigUserSettings = ds
'        _settingsSaver.Save()
'    End Sub

'    ''' <summary>
'    ''' Восстановление местоположения и размеры формы и ширина всех колонок всех гридов
'    ''' </summary>
'    Public Shared Sub RestoreFormGrid(ByVal sender As Form) ', ByVal e As EventArgs)
'        'Private Shared Sub RestoreFormGrid(ByVal sender As Object, ByVal e As EventArgs)
'        If _settingsSaver.dsAllStageGrigUserSettings Is Nothing Then
'            Return
'        End If
'        Dim f As Form = DirectCast(sender, Form)
'        Dim ds As DataSet = _settingsSaver.dsAllStageGrigUserSettings
'        If ds.Tables.IndexOf(f.Name) = -1 Then
'            Return
'        End If
'        'RestoreForm(ds.Tables(f.Name), f)
'        For Each c As Control In f.Controls
'            If TypeOf c Is DataGridView Then
'                Dim grid As DataGridView = DirectCast(c, DataGridView)
'                Dim name As String = GridTableName(f, grid)
'                If ds.Tables.IndexOf(name) > -1 Then
'                    'RestoreGridViewColWidths(ds.Tables(name), grid)
'                    RestoreGridViewRows(ds.Tables(name), grid)
'                End If
'            End If
'        Next
'    End Sub

'    '<System.Runtime.CompilerServices.Extension()> _

'    '''' <summary>
'    '''' Включение механизма запоминания и восстановления.
'    '''' Добавляет обработку событий Load и FormClosing
'    '''' </summary>
'    '''' <param name="f">Форма (неявно)</param>
'    'Public Shared Sub SavingOn(ByVal f As Form)
'    '    AddHandler f.Load, AddressOf RestoreFormGrid
'    '    AddHandler f.FormClosing, AddressOf SaveFormGrid
'    'End Sub
'End Class


