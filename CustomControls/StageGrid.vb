Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports MathematicalLibrary
Imports MDBControlLibrary.UserControl

<ToolboxItem(True)>
<ToolboxBitmap(GetType(StageGrid))>
<Description("DataGridView с пользовательской ячейкой")>
Friend Class StageGrid
    Inherits UserControl

    ' здесь подписаться на событие окончания измененния ячейки
    ' а затем вызвать метод в родительской форме которая будет делать новый запрос и интерактивное обновления

#Region "Property"
    ''' <summary>
    ''' Делегат и событие определенные для события изменения пользователем фильтра строки фильтра
    ''' </summary>
    ''' <param name="GridType"></param>
    ''' <param name="SelectWhereSQL"></param>
    ''' <param name="SelectSortSQL"></param>
    ''' <remarks></remarks>
    Public Delegate Sub FilterChangedCallback(ByVal GridType As StageGridType, ByVal SelectWhereSQL As String, ByVal SelectSortSQL As String)
    Private Event FilterChangedHandlers As FilterChangedCallback
    ''' <summary>
    ''' Нестандартное событие используется для подписки формы к мзменению пользователем фильтра
    ''' </summary>    
    Public Custom Event FindOptionsChanged As FilterChangedCallback
        AddHandler(ByVal value As FilterChangedCallback)
            AddHandler FilterChangedHandlers, value
        End AddHandler

        RemoveHandler(ByVal value As FilterChangedCallback)
            RemoveHandler FilterChangedHandlers, value
        End RemoveHandler

        RaiseEvent(ByVal optionType As StageGridType, ByVal SelectWhereSQL As String, ByVal SelectSortSQL As String)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' Приостановить Events
    ''' </summary>
    ''' <returns></returns>
    Public Property IsStopEvents() As Boolean

    ''' <summary>
    ''' тип сетки зависит от этапа
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TypeStageGrid() As StageGridType

    Private mParentForm As FormBuildQuery

    ''' <summary>
    ''' Родительская Форма
    ''' </summary>
    Private ReadOnly Property ParentFormBuildQuery() As FormBuildQuery
        Get
            If mParentForm Is Nothing OrElse mParentForm.IsDisposed Then
                InitializeHandlerBuildQuery()
            End If

            Return mParentForm
        End Get
    End Property

    Private mSelectWhereSQL As String = String.Empty
    ''' <summary>
    ''' SQL условие WHERE фильтра для этапа
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SelectWhereSQL() As String
        Get
            Return mSelectWhereSQL
        End Get
    End Property

    Private mSelectSortSQL As String = String.Empty
    Public ReadOnly Property SelectSortSQL() As String
        Get
            Return mSelectSortSQL
        End Get
    End Property

    Public WriteOnly Property Caption() As String
        Set(ByVal value As String)
            lblCaption.Text = value
        End Set
    End Property

    Public WriteOnly Property CaptionColor() As Color
        Set(ByVal value As Color)
            lblCaption.BackColor = value
        End Set
    End Property

    Public Property StageGridName() As String
#End Region

    Private mFieldManager As ManagerFieldComboBoxColumn
    Private WithEvents TimerCellParsing As Timer
    ''' <summary>
    ''' запомнить ячейку покинутую из _dgv_CellParsing
    ''' </summary>
    Private rowIdxCellParsing As Integer

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal name As String, ByVal stageType As StageGridType, ByVal owner As FormBuildQuery)
        MyBase.New()
        Me.Name = name
        StageGridName = name
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        mParentForm = owner
        TypeStageGrid = stageType
    End Sub

    ''' <summary>
    ''' Метод вызываемый после загрузки
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitializeStageGrid()
        'OptionsChangedПриЗагрузке()
        'DrawingSurface.Show()'в методе Show происодит вызов свойства DrawingSurface и затем InitializeDrawingForm

        ' сделать резмер родителя под размер сетки
        'Dim pe As New StageEdit()
        'Me._dgv.Columns(_dgv.Columns.Count - 1).Width = pe.Width + 2
        'Dim totalWidth As Integer = 0
        'For Each col As DataGridViewColumn In Me._dgv.Columns
        '    totalWidth += (col.Width + col.DividerWidth)
        'Next
        'Me.ClientSize = New Size(totalWidth + Me._dgv.RowHeadersWidth + 5, Me.ClientSize.Height)

        IsStopEvents = True
        mFieldManager = New ManagerFieldComboBoxColumn(TypeStageGrid, mParentForm.mFormParrent.FormParrent.Manager.PathKT)
        CType(_dgv.Columns(0), DataGridViewComboBoxColumn).Items.AddRange(mFieldManager.ArrStringName)
        TimerCellParsing = New Timer
        IsStopEvents = False
    End Sub

    ''' <summary>
    ''' отслеживание выбора пользователем выбора типа строки в первом столце
    ''' для определения какого типа будет следующая ячейка StageEdit
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub _dgv_EditingControlShowing(ByVal sender As Object, ByVal e As DataGridViewEditingControlShowingEventArgs) Handles _dgv.EditingControlShowing
        If _dgv.CurrentCellAddress.X = 0 Then
            Dim combo As DataGridViewComboBoxEditingControl = CType(e.Control, DataGridViewComboBoxEditingControl)
            'Dim combo As ComboBox = CType(e.Control, ComboBox)
            'RemoveHandler CType(combo, DataGridViewComboBoxEditingControl).SelectedIndexChanged, AddressOf ComboBox_SelectedIndexChanged
            AddHandler combo.SelectedIndexChanged, AddressOf ComboBox_SelectedIndexChanged
        End If
    End Sub

    ''' <summary>
    ''' При смене типа выбора поля из списка создается новая ячейка StageEdit и в конструкторе инициализация внутренних хранилищ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        '_dgv.Rows(_dgv.CurrentCellAddress.Y).Cells(0).Value = _dgv.CurrentCell.EditedFormattedValue.ToString()
        Static OldValue As String

        If OldValue <> _dgv.CurrentCell.EditedFormattedValue.ToString() Then
            OldValue = _dgv.CurrentCell.EditedFormattedValue.ToString()
            ' сбоит mRowIndex
            'If CType(_dgv.Rows(mRowIndex).Cells(0), DataGridViewComboBoxCell).Value IsNot Nothing Then
            'If CType(_dgv.Rows(mRowIndex).Cells(0), DataGridViewComboBoxCell).Value.ToString = "N2" Then
            'If CType(_dgv.CurrentCell, DataGridViewComboBoxCell).Value IsNot Nothing Then
            '    If CType(_dgv.CurrentCell, DataGridViewComboBoxCell).Value.ToString = "N2" Then
            'If CType(_dgv.Rows(mRowIndex).Cells(0), DataGridViewComboBoxEditingControl).ToString = "N2" Then

            _dgv.Rows(_dgv.CurrentCell.RowIndex).Cells(1).Value = New PassportCellFind(mFieldManager.GetFieldFromName(OldValue).ControlType,
                                                                                       "", "", "", "", "", "", "",
                                                                                       Date.Now, Date.Now, Date.Now,
                                                                                       mFieldManager.GetFieldFromName(OldValue).Values)
            OldValue = ""
            'Debug.WriteLine(_dgv.CurrentCell.EditedFormattedValue.ToString())
        End If
    End Sub

    ''' <summary>
    ''' Восстановление строки сетки и ячейки PassportCellFind из ManagerGrids->RestoreFormGrid->RestoreGridViewRows
    ''' </summary>
    ''' <param name="fieldColumn"></param>
    ''' <param name="type"></param>
    ''' <param name="condition1"></param>
    ''' <param name="value1"></param>
    ''' <param name="condition2"></param>
    ''' <param name="value2"></param>
    ''' <param name="condition3"></param>
    ''' <param name="value3"></param>
    ''' <param name="sort"></param>
    ''' <remarks></remarks>
    Public Sub RestoreGridViewRow(ByVal fieldColumn As String,
                                  ByVal type As String,
                                  ByVal condition1 As String,
                                  ByVal value1 As String,
                                  ByVal condition2 As String,
                                  ByVal value2 As String,
                                  ByVal condition3 As String,
                                  ByVal value3 As String,
                                  ByVal sort As String)

        If mFieldManager.GetFieldFromName(fieldColumn) IsNot Nothing Then
            ' в коллекции есть такое поле, а значит имя есть в DataGridViewComboBoxCel
            _dgv.Rows.Add(1) ' добавить строку
            '_dgv.Rows.Insert(0, 1)
            CType(_dgv.Rows(_dgv.RowCount - 2).Cells(0), DataGridViewComboBoxCell).Value = fieldColumn ' выбрать поле в ComboBox

            Dim dateTimePicker1 As DateTime = Date.Now
            Dim dateTimePicker2 As DateTime = Date.Now
            Dim dateTimePicker3 As DateTime = Date.Now

            If type = ControlStageType.TimeBox.ToString OrElse type = ControlStageType.DateBox.ToString Then
                If DateTime.TryParse(value1, dateTimePicker1) Then dateTimePicker1 = DateTime.Parse(value1)
                If DateTime.TryParse(value2, dateTimePicker2) Then dateTimePicker2 = DateTime.Parse(value2)
                If DateTime.TryParse(value3, dateTimePicker3) Then dateTimePicker3 = DateTime.Parse(value3)
            End If

            _dgv.Rows(_dgv.RowCount - 2).Cells(1).Value = New PassportCellFind(mFieldManager.GetFieldFromName(fieldColumn).ControlType,
                                                                               sort, condition1, condition2, condition3,
                                                                               value1, value2, value3,
                                                                               dateTimePicker1, dateTimePicker2, dateTimePicker3,
                                                                               mFieldManager.GetFieldFromName(fieldColumn).Values)
        Else
            MessageBox.Show($"Имя {fieldColumn} отсутствует в списке полей.",
                            $"Function {NameOf(RestoreGridViewRow)}", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub


    Private Sub _dgv_RowsRemoved(ByVal sender As Object, ByVal e As DataGridViewRowsRemovedEventArgs) Handles _dgv.RowsRemoved
        If Not IsStopEvents Then
            If Me._dgv.RowCount <> 0 Then
                If e.RowIndex = 0 AndAlso Me._dgv.RowCount > 1 Then
                    ' удаляется первая строка из списка присвоить rowIdxCellParsing=0
                    rowIdxCellParsing = e.RowIndex
                    TimerCellParsing.Interval = 100
                    TimerCellParsing.Enabled = True
                ElseIf e.RowIndex <> 0 AndAlso Me._dgv.RowCount > 1 Then
                    ' удаляется не первая строка из списка присвоить rowIdxCellParsing на единицу меньше
                    rowIdxCellParsing = e.RowIndex - 1
                    TimerCellParsing.Interval = 100
                    TimerCellParsing.Enabled = True
                Else
                    ' условие Me._dgv.RowCount > 1 не выполнилось значит удаляется последняя оставшеяся строка
                    mSelectWhereSQL = ""
                    mSelectSortSQL = ""
                    OnFilterGridChanged(TypeStageGrid, mSelectWhereSQL, mSelectSortSQL)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' завершение редактирования ячейки, составление фильтра и вызов генерации события изменения фильтра
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub _dgv_CellParsing(ByVal sender As Object, ByVal e As DataGridViewCellParsingEventArgs) Handles _dgv.CellParsing
        ' После редактирования, если последнее завершилось чем угодно, кроме нажатия на ESC, генерируется событие DataGridView.CellParsing. 
        If e.ColumnIndex = 1 Then
            Dim rowIdx As Integer = Me._dgv.CurrentCell.RowIndex
            Dim pas As PassportCellFind = TryCast(Me._dgv.Rows(rowIdx).Cells(1).Value, PassportCellFind)
            'Dim tempDataGridViewStageCell As DataGridViewStageCell = CType(_dgv.Rows(rowIdx).Cells(1), DataGridViewStageCell)
            'Dim pas As PassportCellFind = TryCast(tempDataGridViewStageCell.Value, PassportCellFind)
            Dim info As String

            If pas Is Nothing Then
                info = "Ячейка не содержит объект типа PassportCellFind!"
            Else
                'info = "Ячейка содержит объект типа Passport с реквизитами:" + Environment.NewLine + "Серия - " + pas.Series + Environment.NewLine + "№ - " + pas.Number + Environment.NewLine + "ДатаВыдачи - " + pas.IssueDate.ToShortDateString()
                ' и если все корректно выдача строки для последующего генерации запроса 
                rowIdxCellParsing = rowIdx
                TimerCellParsing.Interval = 100
                TimerCellParsing.Enabled = True
            End If
        End If
    End Sub

    Private Sub TimerCellParsing_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles TimerCellParsing.Tick
        TimerCellParsing.Enabled = False
        If rowIdxCellParsing >= 0 AndAlso rowIdxCellParsing < Me._dgv.RowCount Then
            If Me._dgv.Rows(rowIdxCellParsing).ErrorText = "" AndAlso CheckBildQueryStage() Then
                ' и если все корректно выдача строки для последующего генерации запроса 
                OnFilterGridChanged(TypeStageGrid, mSelectWhereSQL, mSelectSortSQL)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Проверка Фильтра Для Этапа
    ''' </summary>
    ''' <returns></returns>
    Private Function CheckBildQueryStage() As Boolean
        ' проверить первую ячейку в строке чтобы она не была пустой, а то возможно войти в редактирование в StageEdit хотя тип в первой ячейки не определен
        Dim success As Boolean = False

        If CStr(Me._dgv.Rows(rowIdxCellParsing).Cells("FieldColumn").Value) <> "" Then ' чтобы не попали пустые по первому столбцу строки 
            BildQueryStage()
            success = True
        End If

        Return success
    End Function

    ''' <summary>
    ''' Составить Фильтр Для Этапа
    ''' Обновляется при изменении в ячейке и при восстановлении из RestoreFormGrid
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BildQueryStage()
        ' здесь в цикле по всем ячейкам проверка и создание _SelectWhereSQL строки

        '--- Access ---------------------------------------
        '    WHERE (((БазаСнимков.KeyID)=1.1) AND ((БазаСнимков.Тбокса)=2.1) AND ((БазаСнимков.Режим)="3.1")) OR (((БазаСнимков.KeyID)=1.2) AND ((БазаСнимков.Тбокса)=2.2) AND ((БазаСнимков.Режим)="3.2")) OR (((БазаСнимков.KeyID)=1.3) AND ((БазаСнимков.Тбокса)=2.3) AND ((БазаСнимков.Режим)="3.3"))
        'ORDER BY БазаСнимков.KeyID, БазаСнимков.Тбокса DESC;

        'SELECT [7ЗначенияПараметровКТ].Значение
        'FROM 7ЗначенияПараметровКТ
        'WHERE ((([7ЗначенияПараметровКТ].ИмяПараметра)="alfa пер"))
        'ORDER BY [7ЗначенияПараметровКТ].Значение;

        '--- VisualStudio -----------------------------------------------------
        '    WHERE        (OrderID = 1.1) AND (CustomerID = '2.1') AND (EmployeeID = 3.1) OR
        '                         (OrderID = 1.2) AND (CustomerID = '2.2') AND (EmployeeID = 3.2) OR
        '                         (OrderID = 1.3) AND (CustomerID = '2.3') AND (EmployeeID = 3.3)
        'ORDER BY OrderID, CustomerID DESC

        ' цикл для составления _SelectWhereSQL
        Dim sb As StringWriter = New StringWriter()
        Dim fieldName As String
        Dim conditionsOnColunms As New List(Of String) ' условия По Столбцам
        Dim sortOnColunms As New List(Of String) ' сортировка По Столбцам

        For indexColumn As Integer = 1 To 3
            Dim conditionOnColunm As String = Nothing ' условие По Столбцу

            For Each rowgrid As DataGridViewRow In Me._dgv.Rows
                If CStr(rowgrid.Cells("FieldColumn").Value) <> "" Then ' чтобы не попали пустые по первому столбцу строки 

                    fieldName = $"[{rowgrid.Cells("FieldColumn").Value}]" ' выбранный элемент ComboBox в первом столбце
                    Dim pas As PassportCellFind = TryCast(rowgrid.Cells("Stage").Value, PassportCellFind)

                    If pas IsNot Nothing Then
                        Dim condition As String = Nothing
                        Dim valueString As String = Nothing

                        Select Case pas.TypeControlStage
                            Case ControlStageType.DigitalBox
                                If indexColumn = 1 Then
                                    valueString = pas.ComboBoxText1
                                ElseIf indexColumn = 2 Then
                                    valueString = pas.ComboBoxText2
                                ElseIf indexColumn = 3 Then
                                    valueString = pas.ComboBoxText3
                                End If
                            Case ControlStageType.DateBox
                                ' не получилось 
                                'fieldName = $"CDate([{rowgrid.Cells("FieldColumn").Value}])"
                                'If indexColumn = 1 Then
                                '    valueString = $"'{pas.DateTimePickerText1.ToShortDateString}'"
                                'ElseIf indexColumn = 2 Then
                                '    valueString = $"'{pas.DateTimePickerText2.ToShortDateString}'"
                                'ElseIf indexColumn = 3 Then
                                '    valueString = $"'{pas.DateTimePickerText3.ToShortDateString}'"
                                'End If

                                ' вводится =#16.04.2010#
                                ' SQL должен быть WHERE (((БазаСнимков.Дата)=#4/16/2010#)
                                ' возможно здесь нужна замена точки на /
                                If indexColumn = 1 Then
                                    valueString = $"'{pas.DateTimePickerText1.ToShortDateString}'"
                                ElseIf indexColumn = 2 Then
                                    valueString = $"'{pas.DateTimePickerText2.ToShortDateString}'"
                                ElseIf indexColumn = 3 Then
                                    valueString = $"'{pas.DateTimePickerText3.ToShortDateString}'"
                                End If
                            Case ControlStageType.TimeBox
                                ' WHERE (((БазаСнимков.ВремяНачалаСбора)<#14:15:08#));
                                ' знак равенства не использовать
                                ' возможно здесь нужна замена точки на :
                                If indexColumn = 1 Then
                                    valueString = $"'{pas.DateTimePickerText1.ToLongTimeString}'"
                                ElseIf indexColumn = 2 Then
                                    valueString = $"'{pas.DateTimePickerText2.ToLongTimeString}'"
                                ElseIf indexColumn = 3 Then
                                    valueString = $"'{pas.DateTimePickerText3.ToLongTimeString}'"
                                End If
                            Case Else
                                If indexColumn = 1 Then
                                    valueString = $"'{pas.ComboBoxText1}'"
                                ElseIf indexColumn = 2 Then
                                    valueString = $"'{pas.ComboBoxText2}'"
                                ElseIf indexColumn = 3 Then
                                    valueString = $"'{pas.ComboBoxText3}'"
                                End If
                        End Select

                        If indexColumn = 1 Then
                            If pas.IsSetUpCondition1 Then condition = ConditionConvert(pas.Condition1)
                        ElseIf indexColumn = 2 Then
                            If pas.IsSetUpCondition2 Then condition = ConditionConvert(pas.Condition2)
                        ElseIf indexColumn = 3 Then
                            If pas.IsSetUpCondition3 Then condition = ConditionConvert(pas.Condition3)
                        End If

                        If (condition IsNot Nothing) AndAlso (valueString IsNot Nothing) Then
                            If conditionOnColunm Is Nothing Then ' первый без AND
                                conditionOnColunm = $"({fieldName} {condition} {valueString})"
                            Else
                                conditionOnColunm &= $" AND ({fieldName} {condition} {valueString})"
                            End If
                        End If

                        ' сортировка проверяется только в первый проход
                        If indexColumn = 1 Then
                            If pas.Sort Then
                                ' (отсутствует)
                                ' по возрастанию
                                ' по убыванию
                                If Not (pas.SortValue = "(отсутствует)" OrElse pas.SortValue = String.Empty) Then
                                    If pas.SortValue = "по возрастанию" Then
                                        sortOnColunms.Add(fieldName)
                                    ElseIf pas.SortValue = "по убыванию" Then
                                        sortOnColunms.Add(fieldName & " DESC")
                                    End If
                                End If
                            End If
                        End If 'сортировка
                    End If 'pas IsNot Nothing
                End If 'rowgrid.Cells("FieldColumn").Value <> ""
            Next
            If conditionOnColunm IsNot Nothing Then conditionsOnColunms.Add(conditionOnColunm)
        Next

        For Each itemConditionOnColunm As String In conditionsOnColunms
            If sb.ToString = "" Then
                sb.Write($"({itemConditionOnColunm})")
            Else
                sb.Write($" OR ({itemConditionOnColunm})")
            End If
        Next

        Dim strSort As String = Nothing
        For Each itemSortOnColunms As String In sortOnColunms
            If strSort Is Nothing Then
                strSort = itemSortOnColunms
            Else
                strSort &= ", " & itemSortOnColunms
            End If
        Next

        If strSort IsNot Nothing Then
            mSelectSortSQL = strSort
        Else
            strSort = ""
        End If

        If sb.ToString <> "" Then
            mSelectWhereSQL = sb.ToString
        Else
            mSelectWhereSQL = ""
        End If

        'If strSort IsNot Nothing Then
        '    strSort = " ORDER BY " & strSort
        'Else
        '    strSort = ""
        'End If

        'If sb.ToString <> "" Then
        '    _SelectWhereSQL = "WHERE " & sb.ToString & strSort
        'Else
        '    _SelectWhereSQL = ""
        'End If
    End Sub

    ''' <summary>
    ''' Вызвать событие на которое родительская форма подписалась
    ''' </summary>
    ''' <param name="optionType"></param>
    ''' <param name="SelectWhereSQL"></param>
    ''' <param name="SelectSortSQL"></param>
    ''' <remarks></remarks>
    Private Sub OnFilterGridChanged(ByVal optionType As StageGridType, ByVal SelectWhereSQL As String, ByVal SelectSortSQL As String)
        RaiseEvent FilterChangedHandlers(optionType, SelectWhereSQL, SelectSortSQL)
    End Sub

    ''' <summary>
    ''' подписаться на событие FindOptionsChanged и назначить для него обработчик mParentForm.OptionsChangedEventHandler
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitializeHandlerBuildQuery()
        'm_drawingSurface = New SurfaceForm(flags, Me.format, Me.m_drawingFont, Me.drawingText, Me.checkBoxAutoSize.Checked, Me.checkBoxUseBindingBox.Checked)
        mParentForm.SuspendLayout()
        'Me.m_drawingSurface.Owner = Me
        AddHandler FindOptionsChanged, New FilterChangedCallback(AddressOf mParentForm.OptionsChangedEventHandler)
        Me.mParentForm.ResumeLayout()
    End Sub
End Class

''' <summary>
''' класс управления полями ComboBoxColumn
''' </summary>
''' <remarks></remarks>
Friend Class ManagerFieldComboBoxColumn
    Private mFields As List(Of Field)
    Public ReadOnly Property Fields() As List(Of Field)
        Get
            Return mFields
        End Get
    End Property
    Public ReadOnly Property ArrStringName() As String()

    ''' <summary>
    ''' тип сетки зависит от этапа
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property StGridType() As StageGridType

    Private mPathKT As String
    Public WriteOnly Property PathKT() As String
        Set(ByVal value As String)
            mPathKT = value
        End Set
    End Property

    Private ReadOnly Phases As String() = {TypeEngine, NumberEngine, NumberBuild, NumberStage, NumberStarting, NumberKT}

    ''' <summary>
    ''' конструктор выполняет запрос для заполнения листа с названиями контролов настроек этапа
    ''' </summary>
    ''' <param name="inStageType"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal inStageType As StageGridType, ByVal inPathKT As String)
        Dim I As Integer

        StGridType = inStageType
        Me.PathKT = inPathKT
        PopulateFieldsOnPhaseControls()
        'ReDim_ArrStringName(mFields.Count - 1)
        Re.Dim(ArrStringName, mFields.Count - 1)

        For Each itemField As Field In mFields
            ArrStringName(I) = itemField.NameField
            I += 1
        Next
    End Sub

    ''' <summary>
    ''' Заполнить Значения Поля По Контролам Этапов
    ''' для обновления внутреннего массива значений выполняется повторное заполнение
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub PopulateFieldsOnPhaseControls()
        ' этот метод может вызываться к базе данных повторно при Refresh когда КТ добавились или была очистка базы или смена базы
        Dim strSQL As String = String.Empty
        Dim rdr As OleDbDataReader
        Dim cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mPathKT))
        Dim cmd As OleDbCommand = cn.CreateCommand
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable

        Dim numberPhase As Integer = StGridType - 1
        Dim strPhase As String
        Dim valuesString As List(Of String)
        Dim values As List(Of Double) ' для накапливания
        Dim valuesToArray As Double() = New Double() {0} ' для сортировки

        mFields = New List(Of Field) ' обнулить список
        Try
            cn.Open()
            If StGridType = StageGridType.Измеренные OrElse StGridType = StageGridType.Приведенные OrElse StGridType = StageGridType.Пересчитанные Then
                Select Case StGridType
                    Case StageGridType.Измеренные
                        ' измеренные параметры
                        strSQL = "TRANSFORM First([7ЗначенияПараметровКТ].Значение) AS [First-Значение] " &
                        "SELECT [6НомерКТ].НомерКТ " &
                        "FROM 6НомерКТ RIGHT JOIN (ИзмеренныеПараметры INNER JOIN 7ЗначенияПараметровКТ ON ИзмеренныеПараметры.ИмяПараметра = [7ЗначенияПараметровКТ].ИмяПараметра) ON [6НомерКТ].keyНомерКТ = [7ЗначенияПараметровКТ].keyНомерКТ " &
                        "WHERE ((([7ЗначенияПараметровКТ].Значение)<>0)) " &
                        "GROUP BY [6НомерКТ].НомерКТ " &
                        "PIVOT ИзмеренныеПараметры.ИмяПараметра;"
                    Case StageGridType.Приведенные
                        ' Приведеный Параметр
                        strSQL = "TRANSFORM First([7ЗначенияПараметровКТ].Значение) AS [First-Значение] " &
                        "SELECT [6НомерКТ].НомерКТ " &
                        "FROM 6НомерКТ RIGHT JOIN (РасчетныеПараметры INNER JOIN 7ЗначенияПараметровКТ ON РасчетныеПараметры.ИмяПараметра = [7ЗначенияПараметровКТ].ИмяПараметра) ON [6НомерКТ].keyНомерКТ = [7ЗначенияПараметровКТ].keyНомерКТ " &
                        "WHERE ((([7ЗначенияПараметровКТ].Значение)<>0) AND ((РасчетныеПараметры.ПриведеныйПараметр)=True)) " &
                        "GROUP BY [6НомерКТ].НомерКТ " &
                        "PIVOT РасчетныеПараметры.ИмяПараметра;"
                    Case StageGridType.Пересчитанные
                        ' Пересчитанные параметры
                        strSQL = "TRANSFORM First([7ЗначенияПараметровКТ].Значение) AS [First-Значение] " &
                        "SELECT [6НомерКТ].НомерКТ " &
                        "FROM 6НомерКТ RIGHT JOIN (РасчетныеПараметры INNER JOIN 7ЗначенияПараметровКТ ON РасчетныеПараметры.ИмяПараметра = [7ЗначенияПараметровКТ].ИмяПараметра) ON [6НомерКТ].keyНомерКТ = [7ЗначенияПараметровКТ].keyНомерКТ " &
                        "WHERE ((([7ЗначенияПараметровКТ].Значение)<>0) AND ((РасчетныеПараметры.ПриведеныйПараметр)=False)) " &
                        "GROUP BY [6НомерКТ].НомерКТ " &
                        "PIVOT РасчетныеПараметры.ИмяПараметра;"
                End Select

                odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
                odaDataAdapter.Fill(dtDataTable)

                If dtDataTable.Rows.Count > 0 Then
                    For Each dtColumn As DataColumn In dtDataTable.Columns
                        If dtColumn.ColumnName <> "НомерКТ" Then
                            ' вначале обнулить
                            valuesString = New List(Of String) From {
                                "" ' должен быть один пустой
                                }

                            values = New List(Of Double)
                            If valuesToArray.Length <> 0 Then Array.Clear(valuesToArray, 0, valuesToArray.Length)

                            For Each drDataRow As DataRow In dtDataTable.Rows
                                If Not IsDBNull(drDataRow(dtColumn.ColumnName)) Then
                                    values.Add(CDbl(drDataRow(dtColumn.ColumnName)))
                                End If
                            Next

                            If values.Count > 0 Then
                                valuesToArray = values.Distinct.ToArray
                                Array.Sort(valuesToArray)

                                For Each item As Double In valuesToArray
                                    valuesString.Add(item.ToString)
                                Next
                            End If

                            mFields.Add(New Field(dtColumn.ColumnName, ControlStageType.DigitalBox, valuesString))
                        End If
                    Next
                End If
            Else
                strPhase = "КонтролыДля" & Phases(numberPhase)
                strSQL = "SELECT ТипКонтрола.ТипКонтрола, " & strPhase & ".keyКонтролДляУровня, " & strPhase & ".МестоНаПанели, " & strPhase & ".Name, " & strPhase & ".Text, " & strPhase & ".Описание, " & strPhase & ".InputOrOutput, " & strPhase & ".Value, " & strPhase & ".Query, " & strPhase & ".ЛогическоеЗначение" &
                    " FROM ТипКонтрола RIGHT JOIN " & strPhase & " ON ТипКонтрола.keyТипКонтрола = " & strPhase & ".keyТипКонтрола" &
                    " ORDER BY " & strPhase & ".МестоНаПанели;"
                cmd.CommandText = strSQL
                rdr = cmd.ExecuteReader

                Do While rdr.Read()
                    valuesString = New List(Of String) From {
                        "" ' должен быть один пустой
                        }

                    Select Case CStr(rdr("ТипКонтрола"))
                        Case EnumTypeOfControls.CheckBox.ToString
                            valuesString.Add("True")
                            valuesString.Add("False")
                            mFields.Add(New Field(GetControlName(rdr), CType(EnumTypeOfControls.CheckBox, ControlStageType), valuesString))
                            Exit Select
                        Case EnumTypeOfControls.ComboBox.ToString
                            ' если ComboBox 2 источника или запрос rdr("Value").ToString.IndexOf("SELECT") на заполнение из другой таблицы 
                            If Not IsDBNull(rdr("Value")) AndAlso rdr("Value").ToString.IndexOf("SELECT") <> -1 Then
                                PopulateFieldOnSelectSQL(cn, CStr(rdr("Value")), valuesString)
                            Else
                                ' или делать выборку для всех значений по данному полю из всех КТ - это номера сборок, постановок и т.д.
                                SelectPhaseControlsValues(cn, numberPhase, CStr(rdr("Name")), valuesString)
                            End If

                            mFields.Add(New Field(GetControlName(rdr), CType(EnumTypeOfControls.ComboBox, ControlStageType), valuesString))
                            Exit Select
                        Case EnumTypeOfControls.DateBox.ToString
                            mFields.Add(New Field(GetControlName(rdr), CType(EnumTypeOfControls.DateBox, ControlStageType), valuesString))
                            Exit Select
                        Case EnumTypeOfControls.DigitalBox.ToString
                            ' единственный контрол с таким стилем
                            'ComboBoxValue1.DropDownStyle = ComboBoxStyle.DropDown
                            ' делать выборку для всех значений по данному полю из всех КТ
                            SelectPhaseControlsValues(cn, numberPhase, CStr(rdr("Name")), valuesString)
                            mFields.Add(New Field(GetControlName(rdr), CType(EnumTypeOfControls.DigitalBox, ControlStageType), valuesString))
                            Exit Select
                        Case EnumTypeOfControls.ListBox.ToString 'фиксированный список значений
                            mFields.Add(New Field(GetControlName(rdr), CType(EnumTypeOfControls.ListBox, ControlStageType), valuesString))
                            ' если ListBox то только 2 источника  или запрос rdr("Value").ToString.IndexOf("SELECT") на заполнение из другой таблицы
                            If Not IsDBNull(rdr("Value")) Then
                                If rdr("Value").ToString.IndexOf("SELECT") <> -1 Then
                                    PopulateFieldOnSelectSQL(cn, CStr(rdr("Value")), valuesString)
                                Else
                                    ' или строка со значениями
                                    Dim value As String = CStr(rdr("Value"))
                                    If value IsNot Nothing Then
                                        valuesString.AddRange(value.Split(";"c))
                                    End If
                                End If
                            End If
                            Exit Select
                            'Case EnumTypeOfControls.TextBox.ToString
                            '    Exit Select
                        Case EnumTypeOfControls.TimeBox.ToString
                            mFields.Add(New Field(GetControlName(rdr), CType(EnumTypeOfControls.TimeBox, ControlStageType), valuesString))
                            Exit Select
                    End Select
                Loop
                rdr.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString,
                            $"Ошибка определения полей для этапов <{NameOf(PopulateFieldsOnPhaseControls)}>!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Close()
        End Try
    End Sub

    ''' <summary>
    ''' Выдать Имя Контрола
    ''' </summary>
    ''' <param name="rdr"></param>
    ''' <returns></returns>
    Private Function GetControlName(ByRef rdr As OleDbDataReader) As String
        Return CStr(rdr("Name"))
    End Function

    ''' <summary>
    ''' Выборка Всех Значений
    ''' </summary>
    ''' <param name="cn"></param>
    ''' <param name="iNumberPhase"></param>
    ''' <param name="nameControl"></param>
    ''' <param name="refValuesString"></param>
    Private Sub SelectPhaseControlsValues(ByRef cn As OleDbConnection,
                                    ByVal iNumberPhase As Integer,
                                    ByVal nameControl As String,
                                    ByRef refValuesString As List(Of String))
        Dim rdrGetValues As OleDbDataReader
        Dim cmdGetValues As OleDbCommand = cn.CreateCommand
        Dim values As List(Of Double) = New List(Of Double) ' для накапливания
        Dim valuesToArraySort As Double() = New Double() {0} ' для сортировки

        ' првоначальный запрос
        'SELECT  DISTINCT ЗначенияКонтроловДляНомерКТ.ЗначениеПользователя
        'FROM КонтролыДляНомерКТ RIGHT JOIN ЗначенияКонтроловДляНомерКТ ON КонтролыДляНомерКТ.keyКонтролДляУровня = ЗначенияКонтроловДляНомерКТ.keyКонтролДляУровня
        'WHERE (((КонтролыДляНомерКТ.Name)="Т4огр"))
        'ORDER BY ЗначенияКонтроловДляНомерКТ.ЗначениеПользователя;

        Dim strSQL As String = "SELECT DISTINCT ЗначенияКонтроловДля" & Phases(iNumberPhase) & ".ЗначениеПользователя " &
        "FROM КонтролыДля" & Phases(iNumberPhase) & " RIGHT JOIN ЗначенияКонтроловДля" & Phases(iNumberPhase) & " ON КонтролыДля" & Phases(iNumberPhase) & ".keyКонтролДляУровня = ЗначенияКонтроловДля" & Phases(iNumberPhase) & ".keyКонтролДляУровня " &
        "WHERE(((КонтролыДля" & Phases(iNumberPhase) & ".Name) = '" & nameControl & "')) " &
        "ORDER BY ЗначенияКонтроловДля" & Phases(iNumberPhase) & ".ЗначениеПользователя;"

        cmdGetValues.CommandText = strSQL
        rdrGetValues = cmdGetValues.ExecuteReader

        Do While rdrGetValues.Read()
            values.Add(CDbl(Val(rdrGetValues("ЗначениеПользователя")).ToString))
        Loop

        rdrGetValues.Close()

        ' сделать здесь сортировку по цифре
        If values.Count > 0 Then
            valuesToArraySort = values.Distinct.ToArray
            Array.Sort(valuesToArraySort)

            For Each Item As Double In valuesToArraySort
                refValuesString.Add(Item.ToString)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Заполнить Запросом Значение Поля
    ''' </summary>
    ''' <param name="cn"></param>
    ''' <param name="commandText"></param>
    ''' <param name="refValuesString"></param>
    Private Sub PopulateFieldOnSelectSQL(ByRef cn As OleDbConnection,
                                         ByVal commandText As String,
                                         ByRef refValuesString As List(Of String))
        Dim rdrSelect As OleDbDataReader
        Dim cmd As OleDbCommand = cn.CreateCommand

        cmd.CommandText = commandText
        rdrSelect = cmd.ExecuteReader

        Do While rdrSelect.Read()
            refValuesString.Add(CStr(rdrSelect(0)))
        Loop

        rdrSelect.Close()
    End Sub

    ''' <summary>
    ''' индексатор поиска Field в коллекции по имени
    ''' </summary>
    ''' <param name="name"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFieldFromName(ByVal name As String) As Field
        For Each itemField As Field In mFields
            If itemField.NameField = name Then
                Return itemField
                Exit Function
            End If
        Next

        Return Nothing
    End Function
End Class


'Private Sub StageGrid_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'    Dim pe As New StageEdit()
'    Me._dgv.Rows.Add(2)
'    Me._dgv.Rows.Add("Петр", "Петров", New Passport("55", "001287", New DateTime(1991, 8, 19)))
'    Me._dgv.Rows.Add()
'    Me._dgv.Columns(2).Width = pe.Width + 2
'    Dim totalWidth As Integer = 0
'    For Each col As DataGridViewColumn In Me._dgv.Columns
'        totalWidth += (col.Width + col.DividerWidth)
'    Next
'    Me.ClientSize = New Size(totalWidth + Me._dgv.RowHeadersWidth + 5, Me.ClientSize.Height)
'End Sub

'Private m_drawingFont As Font

'''' <summary>
''''     Cached shared arrays containing the name of the different StringFormat options.
''''     Used when setting/getting the values in/from the StringFormat option boxes.
'''' </summary>
'Shared ReadOnly textFlagNames As String()
'Private flags As TextFormatFlags

''''// Events
'''' <summary>
''''     Event raised when the selected font is changed in the OptionsForm.
'''' </summary>
'Private Sub OptionsChangedПриЗагрузке()
'    'Me.linkLabelFont.Text = [String].Format("&Font: Name={0}, Style={1}, Size={2}", Me.m_drawingFont.Name, Me.m_drawingFont.Style, Me.m_drawingFont.SizeInPoints)
'    'Dim txtBoxFont As Font = Me.textBoxDrawingText.Font
'    'Me.textBoxDrawingText.Font = New Font(Me.m_drawingFont.FontFamily, txtBoxFont.Size, txtBoxFont.Style, txtBoxFont.Unit)
'    'OnOptionsChanged(frmDialogПостроитьЗапроса.OptionType.Font, Me.m_drawingFont)
'    OnOptionsChanged(_StGridType, _SelectWhereSQL)
'End Sub

'''' <summary>
''''    Этот метод настраивает CheckChanged event в различных  options boxes (CheckBox/RadioButton objects).
'''' </summary>
'''' <param name="container"></param>
'Private Sub ПодключитьСобытиеИзмененияЯчейки(ByVal container As GroupBox)
'    If container Is Nothing Then
'        Return
'    End If

'    For Each c As Control In container.Controls
'        If TypeOf c Is GroupBox Then 'рекурсивный вызов
'            ПодключитьСобытиеИзмененияЯчейки(TryCast(c, GroupBox))
'        End If

'        Dim cb As CheckBox = TryCast(c, CheckBox)

'        If cb IsNot Nothing Then 'элемент CheckBox
'            AddHandler cb.CheckedChanged, New EventHandler(AddressOf CellsChanged)
'        Else 'элемент RadioButton
'            Dim rb As RadioButton = TryCast(c, RadioButton)

'            If rb IsNot Nothing Then
'                AddHandler rb.CheckedChanged, New EventHandler(AddressOf CellsChanged)
'            End If
'        End If
'    Next
'End Sub

'''' <summary>
''''     Event handler for the different option boxes in the OptionsForm form.  This method raises the 
''''     appropriate event according to the option that has changed.
'''' </summary>
'''' <param name="sender"></param>
'''' <param name="e"></param>
'Private Sub CellsChanged(ByVal sender As Object, ByVal e As EventArgs)
'    If ПриостановитьEvents Then
'        Return
'    End If

'    Dim container As Control = TryCast(sender, Control)

'    While True
'        container = container.Parent

'        If container Is Nothing Then
'            Exit While
'        End If

'        If container.Text = "TextFormatFlags" Then
'            OnTextFormatFlagsChanged(sender)
'            Exit While
'            'ElseIf container.Text = "StringFormat" Then
'            '    OnStringFormatChanged(sender)
'            '    Exit While
'        End If
'    End While
'End Sub

'''' <summary>
''''     Event raised when one of the TextFormatFlags flags is changed in the OptionsForm.
''''     This method updates the current TextForamtFlags flags and triggers the OptionsChanged event.
'''' </summary>
'''' <param name="sender"></param>
'Private Sub OnTextFormatFlagsChanged(ByVal sender As Object)
'    ' Determine what option changed in the TextFormatFlags options and parse its value and set the 
'    ' TextFormatFlags object accordingly.

'    Dim cb As CheckBox = TryCast(sender, CheckBox)

'    If cb IsNot Nothing Then
'        For Each flagName As String In textFlagNames
'            If cb.Text = flagName Then 'текст контрола должен точно соответствовать свойству, которым он управляет
'                '[Enum].Parse(GetType(TextFormatFlags), flagName)
'                ' конвертирует строку представляющую имя или число
'                'Преобразовывает представление последовательности названия или числовое значение один или более перечисленные константы к эквивалентному перечисленному объекту.
'                Dim flag As TextFormatFlags = CType([Enum].Parse(GetType(TextFormatFlags), flagName), TextFormatFlags)

'                If cb.Checked Then
'                    flags = flags Or flag
'                Else
'                    flags = flags And Not flag
'                End If

'                OnOptionsChanged(frmDialogПостроитьЗапроса.OptionType.TextFormatFlags, flags)

'                Exit For
'            End If
'        Next
'    End If
'End Sub

'''' <summary>
''''     Raises the OptionsChanged event.
'''' </summary>
'''' <param name="optionType">The type of the option that changed</param>
'''' <param name="newValue">The new value of the changing option</param>
'Private Sub OnOptionsChanged(ByVal optionType As frmDialogПостроитьЗапроса.OptionType, ByVal newValue As Object)
'    RaiseEvent optionsChangedHandlers(optionType, newValue)
'End Sub

'''' <summary>
''''     Updates the TextFormatFlags option boxes according to the current TextFormatFlags value.
'''' </summary>
'Private Sub UpdateTextFormatFlagsBoxes()
'    Dim flagBoxes As New List(Of CheckBox)()

'    ПриостановитьEvents = True

'    Try
'        ' Get check boxes and reset them.groupBoxTextFormatFlag
'        For Each c1 As Control In Me.groupBoxTextFormatFlags.Controls
'            ' Get flag groups.
'            Dim gb As GroupBox = TryCast(c1, GroupBox)

'            If gb IsNot Nothing Then
'                For Each c2 As Control In gb.Controls
'                    Dim cb As CheckBox = TryCast(c2, CheckBox)

'                    If cb IsNot Nothing Then
'                        cb.Checked = False
'                        flagBoxes.Add(cb)
'                    End If
'                Next
'            End If
'        Next

'        ' update check boxes.
'        For Each flag As TextFormatFlags In [Enum].GetValues(GetType(TextFormatFlags))
'            If (flag And flags) <> 0 Then
'                Dim flagName As String = [Enum].GetName(GetType(TextFormatFlags), flag)

'                For Each cb As CheckBox In flagBoxes
'                    If cb.Text = flagName Then
'                        cb.Checked = True
'                    End If
'                Next
'            End If
'        Next
'    Finally
'        Me.ПриостановитьEvents = False
'    End Try
'End Sub

'''' <summary>
''''     Forces an update of (Synchronizes) the TextFormatFlags options based on the current StringFormat options.
'''' </summary>
'Private Sub ButtonUpdateTextFormat_Click(ByVal sender As Object, ByVal e As EventArgs)
'    'Может быть вставить где-то сохранение значений ячеек
'    'получить значения из хранилища
'    'и сравнить или заново происвоить текущим ячейкам
'    '
'    'Dim newFlags As TextFormatFlags = GetTextFormatFlagsFromStringFormat(Me.format)
'    'OnTextFormatFlagsUpdate(newFlags)
'End Sub

'''' <summary>
''''     Event raised when the TextFormatFlags option boxes in the OptionsForm need to be updated according 
''''     to the value of the TextFormatFlags object.
'''' </summary>
'Private Sub OnTextFormatFlagsUpdate(ByVal newFlags As TextFormatFlags)
'    If flags <> newFlags Then
'        flags = newFlags
'        UpdateTextFormatFlagsBoxes()

'        OnOptionsChanged(frmDialogПостроитьЗапроса.OptionType.TextFormatFlags, flags)
'    End If
'End Sub


''*************************************
''пример создания пользовательского контрола с событием
'Public Event DoughnutComplete()

'Public Enum DoughnutType
'    Glazed
'    Sugar
'    Chocolate
'    ChocolateCake
'    Custard
'    Grape
'    Lemon
'    PlainCake
'    SugarCake
'End Enum

'Private mFlavor As DoughnutType
'Public Property Flavor() As DoughnutType
'    Get
'        Return mFlavor
'    End Get
'    Set(ByVal Value As DoughnutType)
'        mFlavor = Value
'    End Set
'End Property

'Private mDoughnuts As New System.Collections.ArrayList()
'Default Public Property Doughnuts(ByVal Index As Integer) As Doughnut
'    Get
'        Return CType(mDoughnuts(Index), Doughnut)
'    End Get
'    Set(ByVal Value As Doughnut)
'        mDoughnuts(Index) = Value
'    End Set
'End Property

'Private Sub Timer1_Tick1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
'    Dim mDoughnut As New Doughnut(Me.Flavor)
'    mDoughnuts.Add(mDoughnut)
'    RaiseEvent DoughnutComplete()
'End Sub

'Private WithEvents Timer1 As Timer

'Public WriteOnly Property EnabledTimer() As Boolean
'    Set(ByVal Value As Boolean)
'        Timer1.Enabled = Value
'    End Set
'End Property

'Public WriteOnly Property Interval() As Integer
'    Set(ByVal Value As Integer)
'        Timer1.Interval = Value
'    End Set
'End Property

'Public Sub MakeDoughnuts(ByVal dFlavor As DoughnutType)
'    Flavor = dFlavor
'    Select Case dFlavor
'        Case DoughnutType.Chocolate
'            Interval = 15000
'        Case DoughnutType.ChocolateCake
'            Interval = 12000
'        Case DoughnutType.Custard
'            Interval = 10000
'        Case DoughnutType.Glazed
'            Interval = 10000
'        Case DoughnutType.Grape
'            Interval = 10000
'        Case DoughnutType.Lemon
'            Interval = 10000
'        Case DoughnutType.PlainCake
'            Interval = 5000
'        Case DoughnutType.Sugar
'            Interval = 8000
'        Case DoughnutType.SugarCake
'            Interval = 6000
'    End Select
'    EnabledTimer = True
'End Sub

'Public Class Doughnut
'    'Это переменные для хранения значений свойств.
'    Private mFlavor As DoughnutType
'    'Цена по умолчанию,
'    Private mPrice As Single = 0.5
'    Private ReadOnly mTirneOfCreation As Date
'    'Это свойства класса,
'    Public Property Flavor() As DoughnutType
'        Get
'            Return mFlavor
'        End Get
'        Set(ByVal Value As DoughnutType)
'            mFlavor = Value
'        End Set
'    End Property
'    Public Property PriceO() As Single
'        Get
'            Return mPrice
'        End Get
'        Set(ByVal Value As Single)
'            mPrice = Value
'        End Set
'    End Property
'    Public ReadOnly Property TimeOfCreation() As Date
'        Get
'            Return mTirneOfCreation
'        End Get
'    End Property
'    'Это конструктор, з котором устанавливается значение
'    ' свойства mTirneOfCreation.
'    Public Sub New(ByVal Flavor As DoughnutType)
'        'Date.Now - это свойство класса Date,
'        'возвращающее текущее время.
'        mTirneOfCreation = Date.Now
'        mFlavor = Flavor
'    End Sub
'End Class

'Private myDoughnutMachine As DoughnutMachine
'Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As
'System. EventArgs) Handles MyBase.LoacI
'    myDoughnutMachine = New DoughnutMachine()
'End Sub
'Private mRaisedGlazed As Integer
'Private mRaisedSugar As Integer
'Private mRaisedChocolate As Integer
'Private mCakePlain As Integer
'Private mCakeChocolate As Integer
'Private mCakeSugar As Integer
'Private mFilledLemon As Integer
'Private mFilledGrape As Integer
'Private mFilledCustard As Integer

'Private Sub DoughnijtCompleteHandler()
'Select rnyDoughnutMachine.Flavor
'        Case DoughnutMachine.DoughnntType.Glazed
'            mRaisedGlazed += 1
'            txtGlazedRaised.Text = mRaisedGlazed.ToString
'        Case DoughnutMachine.DoughnutType.Sugar
'            mRaisedSugar += 1
'            txtSugarRaised.Text = mRaisedSugar.ToString
'        Case DoughnutMachine.DoughnutType.Chocolate
'            TiRaisedChocolate += 1
'            txtChocolateRaised.Text=roRaisedChocolate.ToString
'    End Select
'End Sub

'Добавьте к обработчику события Forml_Load следующий код, связывающий об-
'работчик события Doughnut Complete, созданный ранее, с событием myDoughnut-
'Machine. Doughnut Complete:
'Visual Basic .NET
'AddHandler nyDoughnutMachine. DoughnutComplete, AddressOf DoughnutCompleteHandler

'Private Sub mnuStop__Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuStop.Click
'    rnyDoughnutMachine.EnabledTimer = False
'End Sub
