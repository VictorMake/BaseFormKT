Imports System.Configuration
Imports System.Windows.Forms

' ManagerGrids это класс управляющий контролами содержащие сетки
' содержит словарь коллекцию с контролами
' в конструкторе создаются 6 контролов этапов и 3 контрола КТ
' есть 9 колекций класса с именем списков каждого контрола
' после создания в конструкторе все контролы добавляются в панель TableLayoutPanelStageGrid frmDialogПостроитьЗапроса
' есть метод для сохранения состояния всех сеток и зеркальный восстановления сеток 
' после восстановления родитель должен заново обновить запрос 
' при смене базы заново делать полное обновление ЗаполнитьКонтролыЭтапов
' повторно при Refresh когда КТ добавились или была очистка базы или смена базы

''' <summary>
''' Класс управляющий контролами содержащие сетки
''' </summary>
Friend Class ManagerGrids
    'Implements ILocationProvider
    Friend WithEvents StageGrid As StageGrid
    Public ReadOnly Property ListStageGrids() As List(Of StageGrid)

    ''' <summary>
    ''' Родительская Форма
    ''' </summary>
    Private ReadOnly Property ParrentForm() As FormBuildQuery

    Public Sub New(ByVal frmOwner As FormBuildQuery)
        ParrentForm = frmOwner

        ListStageGrids = New List(Of StageGrid)

        StageGrid = New StageGrid("ТипыИзделия1", StageGridType.ТипыИзделия1, frmOwner) With {
            .Caption = "Фильтр для настроек параметров для Типы Изделия"
        }
        ListStageGrids.Add(StageGrid)

        StageGrid = New StageGrid("НомерИзделия2", StageGridType.НомерИзделия2, frmOwner) With {
            .Caption = "Фильтр для настроек параметров для Номер Изделия"
        }
        ListStageGrids.Add(StageGrid)

        StageGrid = New StageGrid("НомерСборки3", StageGridType.НомерСборки3, frmOwner) With {
            .Caption = "Фильтр для настроек параметров для Номер Сборки"
        }
        ListStageGrids.Add(StageGrid)

        StageGrid = New StageGrid("НомерПостановки4", StageGridType.НомерПостановки4, frmOwner) With {
            .Caption = "Фильтр для настроек параметров для Номер Постановки"
        }
        ListStageGrids.Add(StageGrid)

        StageGrid = New StageGrid("НомерЗапуска5", StageGridType.НомерЗапуска5, frmOwner) With {
            .Caption = "Фильтр для настроек параметров для Номер Запуска"
        }
        ListStageGrids.Add(StageGrid)

        StageGrid = New StageGrid("НомерКТ", StageGridType.НомерКТ6, frmOwner) With {
            .Caption = "Фильтр для настроек параметров для Номер КТ"
        }
        ListStageGrids.Add(StageGrid)

        StageGrid = New StageGrid("Измеренные", StageGridType.Измеренные, frmOwner) With {
            .Caption = "Фильтр для параметров: Измеренные"
        }
        ListStageGrids.Add(StageGrid)

        StageGrid = New StageGrid("Приведенные", StageGridType.Приведенные, frmOwner) With {
            .Caption = "Фильтр для параметров: Физические и Приведенные"
        }
        ListStageGrids.Add(StageGrid)

        StageGrid = New StageGrid("Пересчитанные", StageGridType.Пересчитанные, frmOwner) With {
            .Caption = "Фильтр для параметров: Пересчитанные"
        }
        ListStageGrids.Add(StageGrid)

        'StageGrid.Flavor = StageEditControl_Test.StageGrid.DoughnutType.Glazed
    End Sub

    Public Sub InitializeAllStageGrids(ByVal frmOwner As FormBuildQuery)
        Dim I As Integer

        frmOwner.TableLayoutPanelStageGrid.Controls.Clear()

        For Each TempStageGrid As StageGrid In ListStageGrids
            frmOwner.TableLayoutPanelStageGrid.Controls.Add(TempStageGrid, 0, I) 'column, row)
            I += 1
            TempStageGrid.CaptionColor = ColorsCaptionGrid(I Mod ColorsCaptionGrid.Length)
        Next

        frmOwner.TableLayoutPanelStageGrid.RowCount = ListStageGrids.Count

        For Each TempStageGrid As StageGrid In ListStageGrids
            frmOwner.TableLayoutPanelStageGrid.RowStyles.Add(New RowStyle(SizeType.Absolute, TempStageGrid.Height + 5))
            TempStageGrid.InitializeHandlerBuildQuery()
            TempStageGrid.InitializeStageGrid()
        Next
    End Sub

    ''' <summary>
    ''' Класс-оболочка для пользовательских параметров приложения.
    ''' его использование ограничено статическим классом eeSaveCW
    ''' </summary>
    Private Class AllStageGrigUserSettings
        Inherits ApplicationSettingsBase
        ''' <summary>
        ''' Единственный параметр с именем dsAllFG типа DataSet
        ''' для хранения настроек всех форм и гридов
        ''' </summary>
        <UserScopedSetting()>
        <DefaultSettingValue(Nothing)>
        Public Property DataSetAllStageGrigUserSettings() As DataSet
            Get
                Return DirectCast(Me("DataSetAllStageGrigUserSettings"), DataSet)
            End Get
            Set(ByVal value As DataSet)
                Me("DataSetAllStageGrigUserSettings") = DirectCast(value, DataSet)
            End Set
        End Property
    End Class

    ''' <summary>
    ''' Хранение настроек в статическом классе
    ''' </summary>
    Private _settingsSaver As New AllStageGrigUserSettings()
    ''' <summary>
    ''' Сброс всех сохранённых настроек
    ''' </summary>
    Public Sub ResetALL()
        _settingsSaver.DataSetAllStageGrigUserSettings = Nothing
    End Sub

    ''' <summary>
    ''' Формируем имя таблицы (DataTable) из имён Формы и Грида
    ''' </summary>
    ''' <param name="UserForm">Форма</param>
    ''' <param name="UserGrid">Грид</param>
    ''' <returns></returns>
    Private Function GridTableName(ByVal UserForm As FormBuildQuery, ByVal UserGrid As StageGrid) As String
        'Private Function GridTableName(ByVal UserForm As Form, ByVal UserGrid As DataGridView) As String
        Return $"{UserForm.Name}__{UserGrid.StageGridName}" 'Name
    End Function

    ''' <summary>
    ''' Создать таблицу с предопределёнными колонками
    ''' </summary>
    ''' <param name="tableName"></param>
    ''' <returns></returns>
    Private Function MakeColNameTypeTable(ByVal tableName As String) As DataTable
        Dim dt As New DataTable(tableName)
        ' выбранный элемент ComboBox в первом столбце 
        Dim column As New DataColumn With {
            .DataType = Type.[GetType]("System.String"),
            .ColumnName = "FieldColumn"
        }
        dt.Columns.Add(column)

        ' далее все ко второму столбцу с типом PassportCellFind
        column = New DataColumn With {
            .DataType = Type.[GetType]("System.String"),
            .ColumnName = "Type"
        }
        dt.Columns.Add(column)

        column = New DataColumn With {
            .DataType = Type.[GetType]("System.String"),
            .ColumnName = "Condition1"
        }
        dt.Columns.Add(column)

        column = New DataColumn With {
            .DataType = Type.[GetType]("System.String"),
            .ColumnName = "Value1"
        }
        dt.Columns.Add(column)

        column = New DataColumn With {
            .DataType = Type.[GetType]("System.String"),
            .ColumnName = "Condition2"
        }
        dt.Columns.Add(column)

        column = New DataColumn With {
            .DataType = Type.[GetType]("System.String"),
            .ColumnName = "Value2"
        }
        dt.Columns.Add(column)

        column = New DataColumn With {
            .DataType = Type.[GetType]("System.String"),
            .ColumnName = "Condition3"
        }
        dt.Columns.Add(column)

        column = New DataColumn With {
            .DataType = Type.[GetType]("System.String"),
            .ColumnName = "Value3"
        }
        dt.Columns.Add(column)

        column = New DataColumn With {
            .DataType = Type.[GetType]("System.String"),
            .ColumnName = "Sort"
        }
        dt.Columns.Add(column)

        Return dt
    End Function

    ''' <summary>
    ''' Заполнить таблицу данными с сетки по каждому фильтру
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="grid"></param>
    Private Sub FillGrideTable(ByVal dt As DataTable, ByVal grid As DataGridView)
        For Each itemRow As DataGridViewRow In grid.Rows
            If CStr(itemRow.Cells("FieldColumn").Value) <> "" Then ' чтобы не попали пустые по первому столбцу строки 
                Dim newRow As DataRow = dt.NewRow

                newRow("FieldColumn") = itemRow.Cells("FieldColumn").Value ' выбранный элемент ComboBox в первом столбце
                'newRow("Type") = itemRow.Cells("Stage").ValueType ' на самом деле вернуть тип контрола
                Dim pas As PassportCellFind = TryCast(itemRow.Cells("Stage").Value, PassportCellFind)

                If pas IsNot Nothing Then
                    newRow("Type") = pas.TypeControlStage
                    Select Case pas.TypeControlStage
                        Case ControlStageType.DateBox
                            newRow("Value1") = pas.DateTimePickerText1.ToShortDateString
                            newRow("Value2") = pas.DateTimePickerText2.ToShortDateString
                            newRow("Value3") = pas.DateTimePickerText3.ToShortDateString
                        Case ControlStageType.TimeBox
                            newRow("Value1") = pas.DateTimePickerText1.ToLongTimeString
                            newRow("Value2") = pas.DateTimePickerText2.ToLongTimeString
                            newRow("Value3") = pas.DateTimePickerText3.ToLongTimeString
                        Case Else
                            newRow("Value1") = pas.ComboBoxText1
                            newRow("Value2") = pas.ComboBoxText2
                            newRow("Value3") = pas.ComboBoxText3
                    End Select

                    newRow("Condition1") = pas.Condition1
                    newRow("Condition2") = pas.Condition2
                    newRow("Condition3") = pas.Condition3
                    newRow("Sort") = pas.SortValue

                    dt.Rows.Add(newRow)
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Воссановить значения по каждому фильтру из сохранённой таблицы
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="itemStageGrid"></param>
    Private Sub RestoreGridViewRows(ByVal dt As DataTable, ByVal itemStageGrid As StageGrid)
        'Dim grid As DataGridView

        Dim type, fieldColumn As String
        Dim value1, value2, value3 As String
        Dim condition1, condition2, condition3 As String
        Dim Sort As String

        itemStageGrid._dgv.Rows.Clear()
        'For I As Integer = ItemStageGrid._dgv.Rows.Count - 2 To 0
        '    ItemStageGrid._dgv.Rows.RemoveAt(I)
        'Next
        For Each itemRow As DataRow In dt.Rows
            Try
                ''grid.Columns(DirectCast(r("GridColumnName"), String)).Width = CInt(r("GridColumnWidth"))

                'Dim newRow As New DataGridViewRow
                '' Создаем ячейку типа CheckBox
                'Dim TextCell As New DataGridViewTextBoxCell
                'TextCell.Value = DirectCast(r("FieldColumn"), String)
                ''checkCell.Value = True
                '' Добавляем в качестве первой ячейки новой строки ячейку типа CheckBox
                'newRow.Cells.Add(TextCell)
                '' Остальные ячейки заполняем ячейками типа TextBox
                ''newRow.Cells.Add(New DataGridViewTextBoxCell())
                ''newRow.Cells(1).Value = "Условие1" & grid.Rows.Count
                ''newRow.Cells.Add(New DataGridViewTextBoxCell())
                ''newRow.Cells(2).Value = "Значение1" & grid.Rows.Count
                '' эта строчка будет с переключателем в первой колонке

                ''Здесь создается новый тип пользовательской ячейки и в ней уже настройки
                ''TextCell = New DataGridViewTextBoxCell
                ''TextCell.Value = DirectCast(r("Type"), String)
                ''newRow.Cells.Add(TextCell)

                'TextCell = New DataGridViewTextBoxCell
                'TextCell.Value = DirectCast(r("Condition1"), String)
                'newRow.Cells.Add(TextCell)
                'TextCell = New DataGridViewTextBoxCell
                'TextCell.Value = DirectCast(r("Value1"), String)
                'newRow.Cells.Add(TextCell)

                'TextCell = New DataGridViewTextBoxCell
                'TextCell.Value = DirectCast(r("Condition2"), String)
                'newRow.Cells.Add(TextCell)
                'TextCell = New DataGridViewTextBoxCell
                'TextCell.Value = DirectCast(r("Value2"), String)
                'newRow.Cells.Add(TextCell)

                'TextCell = New DataGridViewTextBoxCell
                'TextCell.Value = DirectCast(r("Condition3"), String)
                'newRow.Cells.Add(TextCell)
                'TextCell = New DataGridViewTextBoxCell
                'TextCell.Value = DirectCast(r("Value3"), String)
                'newRow.Cells.Add(TextCell)

                'TextCell = New DataGridViewTextBoxCell
                'TextCell.Value = DirectCast(r("Sort"), String)
                'newRow.Cells.Add(TextCell)

                'grid.Rows.Add(newRow)

                fieldColumn = DirectCast(itemRow("FieldColumn"), String)
                type = DirectCast(itemRow("Type"), String)

                condition1 = DirectCast(itemRow("Condition1"), String)
                value1 = DirectCast(itemRow("Value1"), String)

                condition2 = DirectCast(itemRow("Condition2"), String)
                value2 = DirectCast(itemRow("Value2"), String)

                condition3 = DirectCast(itemRow("Condition3"), String)
                value3 = DirectCast(itemRow("Value3"), String)

                Sort = DirectCast(itemRow("Sort"), String)

                itemStageGrid.RestoreGridViewRow(fieldColumn, type, condition1, value1, condition2, value2, condition3, value3, Sort)
            Catch ex As Exception
            End Try
        Next
    End Sub

    ''' <summary>
    ''' Сохранение для каждого StageGrid значений строк для ячеек полей и пользовательской ячейки
    ''' </summary>
    Public Sub SaveFormGrid()
        Dim ds As DataSet

        If _settingsSaver.DataSetAllStageGrigUserSettings IsNot Nothing Then
            ds = _settingsSaver.DataSetAllStageGrigUserSettings
        Else
            ds = New DataSet()
        End If

        For Each TempStageGrid As StageGrid In ListStageGrids
            Dim name As String = GridTableName(ParrentForm, TempStageGrid)

            If ds.Tables.IndexOf(name) > -1 Then
                ds.Tables.Remove(name)
            End If

            Dim dt As DataTable = MakeColNameTypeTable(name)
            FillGrideTable(dt, TempStageGrid._dgv)
            ds.Tables.Add(dt)
        Next

        _settingsSaver.DataSetAllStageGrigUserSettings = ds
        _settingsSaver.Save()
    End Sub

    ''' <summary>
    ''' Восстановление для каждого StageGrid значений строк для ячеек полей и пользовательской ячейки
    ''' </summary>
    Public Sub RestoreFormGrid()
        If _settingsSaver.DataSetAllStageGrigUserSettings Is Nothing Then
            Return
        End If

        Dim ds As DataSet = _settingsSaver.DataSetAllStageGrigUserSettings

        For Each itemStageGrid As StageGrid In ListStageGrids
            Dim name As String = GridTableName(ParrentForm, itemStageGrid)
            If ds.Tables.IndexOf(name) > -1 Then

                itemStageGrid.IsStopEvents = True
                RestoreGridViewRows(ds.Tables(name), itemStageGrid)
                itemStageGrid.BildQueryStage()
                itemStageGrid.IsStopEvents = False
            End If
        Next
    End Sub
End Class


'    Private ReadOnly _providers As New List(Of ILocationProvider)()
'    Private _lastProviderName As String = Nothing

'    Public Sub ClearProviders()
'        _providers.Clear()
'    End Sub

'    Public Sub RegisterProvider(ByVal provider As ILocationProvider)
'        If _providers.Contains(provider) = False Then
'            _providers.Add(provider)
'        End If
'    End Sub

'    Public Sub RemoveProvider(ByVal provider As ILocationProvider)
'        If _providers.Contains(provider) = True Then
'            _providers.Remove(provider)
'        End If
'    End Sub

'#Region "ILocationProvider Members"

'    Public Function GetLocation() As GpsLocation Implements ILocationProvider.GetLocation
'        Dim result As GpsLocation = Nothing
'        _lastProviderName = Nothing

'        For Each provider In _providers
'            Try
'                result = provider.GetLocation()

'                If result IsNot Nothing Then
'                    _lastProviderName = provider.ProviderName
'                    Exit Try
'                End If
'            Catch ex As Exception
'                Continue For
'            End Try
'        Next

'        Return result
'    End Function

'    Public ReadOnly Property ProviderName() As String Implements ILocationProvider.ProviderName
'        Get
'            Return _lastProviderName
'        End Get
'    End Property

'#End Region

'Public Interface ILocationProvider
'    Function GetLocation() As GpsLocation
'    ReadOnly Property ProviderName() As String

'End Interface

'Public Class GpsLocation

'End Class

'пример запуска
'Public Sub Запуск()
'    Dim manager As New ManagerGrids()

'    'manager.RegisterProvider(New GpsLocationProvider())
'    'manager.RegisterProvider(New CellLocationProvider(New OpenCellIdProvider()))
'    'manager.RegisterProvider(New GeoIpLocationProvider())

'    Dim location = manager.GetLocation()
'End Sub