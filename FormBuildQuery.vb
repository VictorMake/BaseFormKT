' Использование основного проекта – StageEdit. В нем находится целых пять public-классов:
' PassportCellFind. 
' PassportConverter. 
' StageEdit. 
' DataGridViewStageCell. 
' DataGridStageColumn. 

' Кидаем на форму через ManagerGrids сетки grid. Через smart tag последнего вызываем диалоговое окно 'Add Columns'. В нем, прежде всего, 
' добавляем обычных текстовых колонок, в которых будут отображаться поля ввода, и колонку типа DataGridStageColumn. 
' дизайнер увидел определенный в ней тип колонки (DataGridStageColumn) и теперь мы можем работать с ней, как со встроенной колонкой.

Imports System.Text

Friend Class FormBuildQuery
    Public mFormParrent As FormHierarchicalTable
    Private mManagerGrids As ManagerGrids

    ''' <summary>
    ''' Тип Изделия
    ''' </summary>
    Private _WhereTypeEngine As String
    ''' <summary>
    ''' Номер Изделия
    ''' </summary>
    Private _WhereNumberEngine As String
    ''' <summary>
    ''' Номер Сборки
    ''' </summary>
    Private _WhereNumberBuild As String
    ''' <summary>
    ''' Номер Постановки
    ''' </summary>
    Private _WhereNumberStage As String
    ''' <summary>
    ''' Номер Запуска
    ''' </summary>
    Private _WhereNumberStarting As String
    ''' <summary>
    ''' Номер Контр Точки
    ''' </summary>
    Private _WhereNumberKT As String
    ''' <summary>
    ''' Измеренные
    ''' </summary>
    Private _WhereMeasurement As String
    ''' <summary>
    ''' Приведенные
    ''' </summary>
    Private _WhereCast As String
    ''' <summary>
    ''' Пересчитанные
    ''' </summary>
    Private _WhereConverting As String

    ''' <summary>
    ''' Тип Изделия
    ''' </summary>
    Private _SortTypeEngine As String
    ''' <summary>
    ''' Номер Изделия
    ''' </summary>
    Private _SortNumberEngine As String
    ''' <summary>
    ''' Номер Сборки
    ''' </summary>
    Private _SortNumberBuild As String
    ''' <summary>
    ''' Номер Постановки
    ''' </summary>
    Private _SortNumberStage As String
    ''' <summary>
    ''' Номер Запуска
    ''' </summary>
    Private _SortNumberStarting As String
    ''' <summary>
    ''' Номер Контр Точки
    ''' </summary>
    Private _SortNumberKT As String
    ''' <summary>
    ''' Измеренные
    ''' </summary>
    Private _SortMeasurement As String
    ''' <summary>
    ''' Приведенные
    ''' </summary>
    Private _SortCast As String
    ''' <summary>
    ''' Пересчитанные
    ''' </summary>
    Private _SortConverting As String

    Public Sub New(ByVal inFormParrent As FormHierarchicalTable)
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'Me.MdiParent = FormParrent
        mFormParrent = inFormParrent
        InitializeGrids()
    End Sub

    ''' <summary>
    ''' подписка на  событие которое генерирует StageGrid когда происходит завершение редактирование пользовательской ячейки
    ''' </summary>
    ''' <param name="changeType"></param>
    ''' <param name="SelectWhereSQL"></param>
    ''' <param name="SelectSortSQL"></param>
    ''' <remarks></remarks>
    Public Sub OptionsChangedEventHandler(ByVal changeType As StageGridType, ByVal SelectWhereSQL As String, ByVal SelectSortSQL As String)
        Select Case changeType
            Case StageGridType.ТипыИзделия1
                _WhereTypeEngine = SelectWhereSQL
                _SortTypeEngine = SelectSortSQL
            Case StageGridType.НомерИзделия2
                _WhereNumberEngine = SelectWhereSQL
                _SortNumberEngine = SelectSortSQL
            Case StageGridType.НомерСборки3
                _WhereNumberBuild = SelectWhereSQL
                _SortNumberBuild = SelectSortSQL
            Case StageGridType.НомерПостановки4
                _WhereNumberStage = SelectWhereSQL
                _SortNumberStage = SelectSortSQL
            Case StageGridType.НомерЗапуска5
                _WhereNumberStarting = SelectWhereSQL
                _SortNumberStarting = SelectSortSQL
            Case StageGridType.НомерКТ6
                _WhereNumberKT = SelectWhereSQL
                _SortNumberKT = SelectSortSQL
            Case StageGridType.Измеренные
                _WhereMeasurement = SelectWhereSQL
                _SortMeasurement = SelectSortSQL
            Case StageGridType.Приведенные
                _WhereCast = SelectWhereSQL
                _SortCast = SelectSortSQL
            Case StageGridType.Пересчитанные
                _WhereConverting = SelectWhereSQL
                _SortConverting = SelectSortSQL
        End Select

        'MessageBox.Show("Type = " & changeType.ToString & vbCrLf & "SelectWhereSQL = " & SelectWhereSQL & vbCrLf & "SelectSortSQL = " & SelectSortSQL, "Обработчик OptionsChangedEventHandler")

        ' вызвать запрос на загрузку
        PopulateUpdateQueries()
    End Sub

    ''' <summary>
    ''' Заполнить Запросы На Обновление
    ''' </summary>
    Private Sub PopulateUpdateQueries()
        With mFormParrent
            .SetQueryTypeEngine(_WhereTypeEngine)
            .SetQueryNumberEngine(_WhereNumberEngine)
            .SetQueryNumberBuild(_WhereNumberBuild)
            .SetQeryNumberStage(_WhereNumberStage)
            .SetQeryNumberStarting(_WhereNumberStarting)
            .SetQueryNumberKT(_WhereNumberKT)
            .SetQueryMeasurement(_WhereMeasurement)
            .SetQeryCast(_WhereCast)
            .SetQueryConverting(_WhereConverting)

            .SetSortTypeEngine(_SortTypeEngine)
            .SetSortNumberEngine(_SortNumberEngine)
            .SetSortNumberBuild(_SortNumberBuild)
            .SetSortNumberStage(_SortNumberStage)
            .SetSortNumberStarting(_SortNumberStarting)
            .SetSortyNumberKT(_SortNumberKT)
            .SetSortMeasurement(_SortMeasurement)
            .SetSortCast(_SortCast)
            .SetSortConverting(_SortConverting)
        End With

        UpdateQueryReportParameter()
        mFormParrent.ShowQuery()
    End Sub

    ''' <summary>
    ''' Сформировать строку параметра отчёта, содержащую суммарную строку фильтра построенного запроса.
    ''' </summary>
    Private Sub UpdateQueryReportParameter()
        Dim mQueries As New StringBuilder

        If _WhereTypeEngine <> String.Empty Then
            mQueries.AppendLine(_WhereTypeEngine)
        End If
        If _WhereNumberEngine <> String.Empty Then
            mQueries.AppendLine(_WhereNumberEngine)
        End If
        If _WhereNumberBuild <> String.Empty Then
            mQueries.AppendLine(_WhereNumberBuild)
        End If
        If _WhereNumberStage <> String.Empty Then
            mQueries.AppendLine(_WhereNumberStage)
        End If
        If _WhereNumberStarting <> String.Empty Then
            mQueries.AppendLine(_WhereNumberStarting)
        End If
        If _WhereNumberKT <> String.Empty Then
            mQueries.AppendLine(_WhereNumberKT)
        End If
        If _WhereMeasurement <> String.Empty Then
            mQueries.AppendLine(_WhereMeasurement)
        End If
        If _WhereCast <> String.Empty Then
            mQueries.AppendLine(_WhereCast)
        End If
        If _WhereConverting <> String.Empty Then
            mQueries.AppendLine(_WhereConverting)
        End If

        If mQueries.Length = 0 Then
            mFormParrent.ReportParameter = "Все записи"
        Else
            mFormParrent.ReportParameter = mQueries.ToString
        End If
    End Sub

    ''' <summary>
    ''' Заполнить Запросы На Обновление После Восстановления
    ''' из каждого StageGrid заново считать значения настроек
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateUpdateQueriesAfterRestore()
        'в форме frmИерархический присвоить всем фильтрам этапов соответствующие фильтры из сеток
        For Each itemStageGrid As StageGrid In mManagerGrids.ListStageGrids
            Select Case itemStageGrid.TypeStageGrid
                Case StageGridType.ТипыИзделия1
                    _WhereTypeEngine = itemStageGrid.SelectWhereSQL
                    _SortTypeEngine = itemStageGrid.SelectSortSQL
                Case StageGridType.НомерИзделия2
                    _WhereNumberEngine = itemStageGrid.SelectWhereSQL
                    _SortNumberEngine = itemStageGrid.SelectSortSQL
                Case StageGridType.НомерСборки3
                    _WhereNumberBuild = itemStageGrid.SelectWhereSQL
                    _SortNumberBuild = itemStageGrid.SelectSortSQL
                Case StageGridType.НомерПостановки4
                    _WhereNumberStage = itemStageGrid.SelectWhereSQL
                    _SortNumberStage = itemStageGrid.SelectSortSQL
                Case StageGridType.НомерЗапуска5
                    _WhereNumberStarting = itemStageGrid.SelectWhereSQL
                    _SortNumberStarting = itemStageGrid.SelectSortSQL
                Case StageGridType.НомерКТ6
                    _WhereNumberKT = itemStageGrid.SelectWhereSQL
                    _SortNumberKT = itemStageGrid.SelectSortSQL
                Case StageGridType.Измеренные
                    _WhereMeasurement = itemStageGrid.SelectWhereSQL
                    _SortMeasurement = itemStageGrid.SelectSortSQL
                Case StageGridType.Приведенные
                    _WhereCast = itemStageGrid.SelectWhereSQL
                    _SortCast = itemStageGrid.SelectSortSQL
                Case StageGridType.Пересчитанные
                    _WhereConverting = itemStageGrid.SelectWhereSQL
                    _SortConverting = itemStageGrid.SelectSortSQL
            End Select
        Next

        PopulateUpdateQueries()
    End Sub

    Private Sub ButtonSaveQuery_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveQuery.Click
        mManagerGrids.SaveFormGrid()
    End Sub

    Private Sub ButtonRestoreQuery_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonRestoreQuery.Click
        mManagerGrids.RestoreFormGrid()
        PopulateUpdateQueriesAfterRestore()
    End Sub

    Private Sub ButtonResetQuery_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonResetQuery.Click
        mManagerGrids.ResetALL()
    End Sub

    Private Sub ButtonRefreshQuery_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonRefreshQuery.Click
        RefreshGrids()
    End Sub

    ''' <summary>
    ''' вызывается при смене базы данных и по кнопке
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RefreshGrids()
        InitializeGrids()
        PopulateUpdateQueriesAfterRestore()
    End Sub

    ''' <summary>
    ''' вызывается из конструктора 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeGrids()
        _WhereTypeEngine = String.Empty
        _WhereNumberEngine = String.Empty
        _WhereNumberBuild = String.Empty
        _WhereNumberStage = String.Empty
        _WhereNumberStarting = String.Empty
        _WhereNumberKT = String.Empty
        _WhereMeasurement = String.Empty
        _WhereCast = String.Empty
        _WhereConverting = String.Empty

        _SortTypeEngine = String.Empty
        _SortNumberEngine = String.Empty
        _SortNumberBuild = String.Empty
        _SortNumberStage = String.Empty
        _SortNumberStarting = String.Empty
        _SortNumberKT = String.Empty
        _SortMeasurement = String.Empty
        _SortCast = String.Empty
        _SortConverting = String.Empty

        mManagerGrids = New ManagerGrids(Me)
        mManagerGrids.InitializeAllStageGrids(Me)
    End Sub

    Private Sub OK_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OK_Button.Click
        'Me.DialogResult = System.Windows.Forms.DialogResult.OK
        'mФормаРодителя.ОтобразитьЗапрос() ' здесь не надо, по закрытию диалога основное окно выполняет обновление
        'Me.Hide()  ' не работает (в псевдо модальном режиме глюка с таблицей)
        'Me.Close()
        HideDialogBuildQuery()
    End Sub

    ''' <summary>
    ''' Закрыть диалог Построить Запрос
    ''' </summary>
    Private Sub HideDialogBuildQuery()
        Me.DialogResult = Windows.Forms.DialogResult.OK
        UpdateQueryReportParameter()
        mFormParrent.ShowQuery() ' здесь не надо, по закрытию диалога основное окно выполняет обновление
        Me.Hide()  ' не работает (в псевдо модальном режиме глюка с таблицей)
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Hide() ' не работает (в псевдо модальном режиме глюка с таблицей)
        'Me.Close()
    End Sub

    ' не использовать в модальном режиме
    Private Sub FormBuildQuery_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not CType(mFormParrent.ParentForm, frmBaseKT).IsWindowClosed Then
            'HideDialogПостроитьЗапроса()
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Hide() ' не работает (в псевдо модальном режиме глюка с таблицей)
            e.Cancel = True
        End If
    End Sub
End Class

''а здесь вместо этих переменных сделать строки - фильтры этапов
'Private flags As TextFormatFlags
'Private format As Drawing.StringFormat
'Private drawingFont As Drawing.Font
'Private drawingText As String
'Private blnAutosize As Boolean
'Private useBindingBox As Boolean

''можно сделать перечислитель этапов и генерировать событие изменения ячеек для этого контрола-этапа
'''' <summary>
''''     Enum defining the different options that can be changed from the Options form.
'''' </summary>
'Public Enum OptionType
'    Font
'    Text
'    TextFormatFlags
'    StringFormat
'    AutoSize
'    UseBindingBox
'End Enum

'''' <summary>
''''     Handler for the frmDialogПостроитьЗапроса.OptionsChanged event.  When this event happens the drawing surface form
''''     is repainted with the new values for the options that changed.
'''' </summary>
'''' <param name="changeType"></param>
'''' <param name="newValue"></param>
'Public Sub OptionsChangedEventHandler(ByVal changeType As frmDialogПостроитьЗапроса.OptionType, ByVal newValue As Object)
'    Select Case changeType
'        Case frmDialogПостроитьЗапроса.OptionType.AutoSize
'            Me.blnAutosize = CBool(newValue)
'            Exit Select
'        Case frmDialogПостроитьЗапроса.OptionType.Font
'            Me.drawingFont = TryCast(newValue, Drawing.Font)
'            Exit Select
'        Case frmDialogПостроитьЗапроса.OptionType.StringFormat
'            Me.format = TryCast(newValue, Drawing.StringFormat)
'            Exit Select
'        Case frmDialogПостроитьЗапроса.OptionType.Text
'            Me.drawingText = newValue.ToString()
'            Exit Select
'        Case frmDialogПостроитьЗапроса.OptionType.TextFormatFlags
'            Me.flags = CType(newValue, TextFormatFlags)
'            Exit Select
'        Case frmDialogПостроитьЗапроса.OptionType.UseBindingBox
'            Me.useBindingBox = CBool(newValue)
'            Exit Select
'        Case Else
'            Return
'    End Select

'    'Refresh()
'    'вызвать запрос на загрузку
'End Sub

