Imports System.Data.OleDb
Imports System.IO
Imports System.Math
Imports System.Text
Imports System.Windows.Forms
Imports MathematicalLibrary
Imports MDBControlLibrary.UserControl

Public Class ProjectManager
    'Delegate Sub DataErrorventHandler(ByVal sender As Object, ByVal e As DataErrorEventArgs)
    Public Event DataError(ByVal sender As Object, ByVal e As IClassCalculation.DataErrorEventArgs) ' Implements BaseForm.IClassCalculation.DataError

    ''' <summary>
    ''' Отслеживает изменение в таблице пользователем значений ячеек.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NeedToRewrite As Boolean

    Private mPathSettingMdb As String '= "G:\DiskD\ПрограммыVBNET\RegistrationНаследование\bin\Store\МодулиСбораКТ\BaseFormKT.mdb" ' "Определить базу параметров" '"BaseForm.mdb" '"G:\DiskD\ПрограммыVBNET\RegistrationНаследование\bin\Ресурсы\МодулиРасчета\BaseForm.mdb"
    Private Const PatternCaculateKTExcel As String = "ШаблонПодсчетаКТ.xlsx"

    ''' <summary>
    ''' Путь к базе Access с настройками.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PathSettingMdb() As String
        Get
            Return mPathSettingMdb
        End Get
        Set(ByVal value As String)
            mPathSettingMdb = value
            If Not File.Exists(value) Then
                MessageBox.Show($"В каталоге нет файла <{value}>!", "Запуск модуля расчета", MessageBoxButtons.OK, MessageBoxIcon.Error)
                'System.Environment.Exit(0) 'End
                'System.Windows.Forms.Application.Exit()
            Else
                '    Directory.SetCurrentDirectory(Application.StartupPath)
                PathCatalog = Path.GetDirectoryName(mPathSettingMdb)
                PathSettingXml = Path.Combine(PathCatalog, Path.GetFileNameWithoutExtension(mPathSettingMdb)) & ".xml"
                PathResource = Path.GetDirectoryName(PathCatalog)
                PathKT = value
                'PathTemplateCalculateKT = Path.Combine(Path.Combine(PathCatalog, Path.GetFileNameWithoutExtension(mPathSettingMdb)), PatternCaculateKTExcel)

                'If Not File.Exists(PathTemplateCalculateKT) Then
                '    MessageBox.Show($"В каталоге нет файла <{PathTemplateCalculateKT}>!", "Запуск приложения", MessageBoxButtons.OK, MessageBoxIcon.Error)
                'End If
            End If
        End Set
    End Property
    Public Property PathResource() As String
    Public Property PathKT() As String = Nothing

    Private mPathSettingXml As String = "Определить путь к файлу настроек .xml" ' "...\Ресурсы\МодулиРасчета\BaseForm.xml"
    ''' <summary>
    ''' Путь к файлу настроек .xml
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PathSettingXml() As String
        Get
            Return mPathSettingXml
        End Get
        Set(ByVal value As String)
            mPathSettingXml = value

            If Not File.Exists(mPathSettingXml) Then
                CreateDocumentSettings(mPathSettingXml)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Путь к каталогу с дополнительными файлами к программе.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PathCatalog() As String
    Public Property NameRegistrationParameters() As String() = {"Отсутствует", "One", "Two", "N1", "Вк", "Вб"}

    Private mIndexRegistrationParameters As Integer() = {1, 2, 3, 4, 5}
    ''' <summary>
    ''' Массив индексов к значениям доступных каналов (соответствует именам).
    ''' </summary>
    ''' <param name="inIndexRegistrationParameters"></param>
    ''' <remarks></remarks>
    Public Sub SetIndexRegistrationParameters(ByVal inIndexRegistrationParameters As Integer())
        mIndexRegistrationParameters = inIndexRegistrationParameters
    End Sub

    Private mIsEnabledTuningForms As Boolean = True
    ''' <summary>
    ''' Управление делегируется главному приложению.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsEnabledTuningForms() As Boolean
        Get
            Return mIsEnabledTuningForms
        End Get
        Set(ByVal value As Boolean)
            mIsEnabledTuningForms = value

            fMainFormBase.varMeasurementParameters.Enabled = value
            fMainFormBase.varCalculatedParameters.Enabled = value
            fMainFormBase.varMeasurementParameters.DataGridViewMeasurement.Visible = value
            fMainFormBase.varCalculatedParameters.DataGridViewCalculated.Visible = value
        End Set
    End Property

    Private mIsCheckPassed As Boolean
    ''' <summary>
    ''' Флаг прохождения всех проверок.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsCheckPassed() As Boolean
        Get
            Return mIsCheckPassed
        End Get
    End Property

    ''' <summary>
    ''' Типизированная таблица (DataTable) ИзмеренныеПараметры.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MeasurementDataTable() As BaseFormDataSet.ИзмеренныеПараметрыDataTable

    ''' <summary>
    ''' Типизированная таблица (DataTable) РасчетныеПараметрыDataTable.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CalculatedDataTable() As BaseFormDataSet.РасчетныеПараметрыDataTable

    Private mChannelsValue As Double()
    ''' <summary>
    ''' Значения Всех Каналов
    ''' </summary>
    ''' <returns></returns>
    Public Property ChannelsValue() As Double()
        Set(ByVal value As Double())
            mChannelsValue = value
            ' сбросить флаг varЗапросВсехЗначений
            mIsQueryChannelsValue = False
        End Set
        Get
            Return mChannelsValue
        End Get
    End Property
    ''' <summary>
    ''' Счетчик Накоплений
    ''' </summary>
    ''' <returns></returns>
    Public Property CounterAppend() As Integer
    ''' <summary>
    ''' Накоплений КТ
    ''' </summary>
    ''' <returns></returns>
    Public Property CounterKT() As Integer
    ''' <summary>
    ''' Счетчик Свечения Графика Параметров
    ''' </summary>
    ''' <returns></returns>
    Public Property CounterTailGraphByParameters() As Integer
    Public Property CounterGraph() As Integer

    Public Property KeyConfiguration() As Integer
    ''' <summary>
    ''' График Параметров От Времени
    ''' </summary>
    ''' <returns></returns>
    Public Property IsGraphParameterByTime() As Boolean
    ''' <summary>
    ''' Реальный Режим двигателя
    ''' </summary>
    ''' <returns></returns>
    Public Property Regime As String
    ''' <summary>
    ''' Подсчет Разрешен
    ''' </summary>
    ''' <returns></returns>
    Public Property IsCalculateEnable As Boolean

    ''' <summary>
    ''' keyНомерКонтрТочки
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property KeyNumberKT() As Integer
        Get
            Return fMainFormBase.varFormMeasurementKT.keyNewNumberKT
        End Get
    End Property
    ''' <summary>
    ''' Server Рабочий Каталог
    ''' </summary>
    ''' <remarks></remarks>
    Public Property ServerWorkingFolder() As String
    ''' <summary>
    ''' частота фонового
    ''' </summary>
    ''' <remarks></remarks>
    Public Property FrequencyBackground() As Integer
    ''' <summary>
    ''' Режим Просмотра Снимков
    ''' </summary>
    ''' <returns></returns>
    Public Property IsSwohSnapshot() As Boolean

    Private mIsQueryChannelsValue As Boolean
    ''' <summary>
    ''' Запрос Значений Всех Каналов
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IsQueryChannelsValue() As Boolean
        Get
            Return mIsQueryChannelsValue
        End Get
    End Property

    Public Property ControlsForPhase() As Dictionary(Of String, Dictionary(Of String, MDBControlLibrary.IUserControl)) = New Dictionary(Of String, Dictionary(Of String, MDBControlLibrary.IUserControl))()
    ''' <summary>
    ''' Имена Этапов
    ''' </summary>
    ''' <returns></returns>
    Public Property StageNames() As String()

    Private varErrorPanelVisible As Boolean
    Public Property ErrorPanelVisible() As Boolean
        Set(ByVal value As Boolean)
            If varErrorPanelVisible <> value Then
                fMainFormBase.varFormGraf.tsTextBoxОшибкаРасчета.Visible = value
            End If
            varErrorPanelVisible = value
        End Set
        Get
            Return varErrorPanelVisible
        End Get
    End Property

    Private Units As String() ' для заполнения строк столбца ЕдиницаИзмеренияRow
    Private NameBaseParameters As String() ' для заполнения строк столбца ИмяПараметра
    Private errorDescription As String ' описание ошибки
    Private fMainFormBase As frmBaseKT

    Public Sub New(ByVal fMainFormBase As frmBaseKT)
        Me.fMainFormBase = fMainFormBase
    End Sub

    ''' <summary>
    ''' Заполнить выпадающие списки для ячеек таблицы.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub FillCombo()
        'вызывается из основного приложения в процедуре ПараметрыДляРежима или при смене количества поступивших параметров в Клиенте
        'из цепочки CWDataSocket1OnDataUpdated -> Форма.НовыйСнимокИмитатора() -> НовыйСнимокИмитатора -> mnuРегистратор_Click -> ПараметрыДляРежима()
        Dim I As Integer = 1

        ' связать ЕдиницаИзмеренияBindingSource.DataSource в этом методе, т.к. связывание в других формах производится в дизайнере.
        Using UnitBindingSource As BindingSource = New BindingSource()
            UnitBindingSource.DataMember = "ЕдиницаИзмерения"
            UnitBindingSource.DataSource = fMainFormBase.varMeasurementParameters.BaseFormDataSet
        End Using

        Using UnitTableAdapter As BaseFormKT.BaseFormDataSetTableAdapters.ЕдиницаИзмеренияTableAdapter = New BaseFormKT.BaseFormDataSetTableAdapters.ЕдиницаИзмеренияTableAdapter With {
                                                                                                                .ClearBeforeFill = True,
                                                                                                                .Connection = New OleDbConnection(BuildCnnStr(PROVIDER_JET, mPathSettingMdb))
                                                                                                            }
            UnitTableAdapter.Fill(fMainFormBase.varMeasurementParameters.BaseFormDataSet.ЕдиницаИзмерения)
        End Using

        '--- заполнить колонку ИмяПараметраИзмерени ---------------------------
        Using TempComboBoxColumnNameMeasurementParameter As DataGridViewComboBoxColumn = CType(fMainFormBase.varMeasurementParameters.DataGridViewMeasurement.Columns("ИмяКаналаИзмеренияDataGridViewTextBoxColumn"), DataGridViewComboBoxColumn)
            TempComboBoxColumnNameMeasurementParameter.Items.Clear() ' чтобы повторно не добавились
            TempComboBoxColumnNameMeasurementParameter.Items.AddRange(NameRegistrationParameters)
        End Using

        'ReDim_NameBaseParameters(MeasurementDataTable.Rows.Count)
        Re.Dim(NameBaseParameters, MeasurementDataTable.Rows.Count)
        ' (заполним массив теперь не каналами а входными параметрами ) - отмена
        NameBaseParameters(0) = ""

        For Each rowИзмеренныйПараметр As BaseFormDataSet.ИзмеренныеПараметрыRow In MeasurementDataTable.Rows
            NameBaseParameters(I) = rowИзмеренныйПараметр.ИмяПараметра
            I += 1
        Next

        Using TempComboBoxColumnNameBaseParameter As DataGridViewComboBoxColumn = CType(fMainFormBase.varMeasurementParameters.DataGridViewMeasurement.Columns("ИмяБазовогоПараметраDataGridViewTextBoxColumn"), DataGridViewComboBoxColumn)
            TempComboBoxColumnNameBaseParameter.Items.Clear()
            TempComboBoxColumnNameBaseParameter.Items.AddRange(NameBaseParameters)
        End Using

        '--- заполнить колонку ЕдИзмВходная -----------------------------------
        Dim tableUnit As BaseFormDataSet.ЕдиницаИзмеренияDataTable = fMainFormBase.varMeasurementParameters.BaseFormDataSet.ЕдиницаИзмерения
        'ReDim_Units(tableUnit.Rows.Count)
        Re.Dim(Units, tableUnit.Rows.Count)
        Units(0) = ""
        I = 1

        For Each row As BaseFormDataSet.ЕдиницаИзмеренияRow In tableUnit.Rows
            Units(I) = row.ЕдиницаИзмерения
            I += 1
        Next

        Using TempComboBoxColumnUnitInput As DataGridViewComboBoxColumn = CType(fMainFormBase.varMeasurementParameters.DataGridViewMeasurement.Columns("РазмерностьВходнаяDataGridViewTextBoxColumn"), DataGridViewComboBoxColumn)
            TempComboBoxColumnUnitInput.Items.Clear()
            TempComboBoxColumnUnitInput.Items.AddRange(Units)
        End Using

        '--- заполнить колонку ЕдИзмВыходнаяРасчетные таблицы РасчетныеПараметры
        Using TempComboBoxColumnUnitOutput As DataGridViewComboBoxColumn = CType(fMainFormBase.varCalculatedParameters.DataGridViewCalculated.Columns("РазмерностьВыходнаяDataGridViewTextBoxColumn"), DataGridViewComboBoxColumn)
            TempComboBoxColumnUnitOutput.Items.Clear()
            TempComboBoxColumnUnitOutput.Items.AddRange(Units)
        End Using

        '--- заполнить колонку ТипДавления ------------------------------------
        Using TempComboBoxColumnTypePressure As DataGridViewComboBoxColumn = CType(fMainFormBase.varMeasurementParameters.DataGridViewMeasurement.Columns("ТипДавленияDataGridViewTextBoxColumn"), DataGridViewComboBoxColumn)
            TempComboBoxColumnTypePressure.Items.Clear()
            TempComboBoxColumnTypePressure.Items.AddRange({"", "Разрежение", "Давление"})
        End Using

        ''занести на сетку
        'For I = 1 To UBound(arrСоответствие)
        '    'DGVСоответсвие.Rows.Add()
        '    Dim heter_row As DataGridViewRow = New DataGridViewRow
        '    ' создаем строку, считывая описания колонок с _grid
        '    heter_row.CreateCells(DGVСоответсвие)

        '    heter_row.Cells(0).Value = CType(arrСоответствие(I).strИмяРасчета, Object)
        '    heter_row.Cells(1).Value = CType(CStr(arrСоответствие(I).NРасчета), Object)
        '    heter_row.Cells(2).Value = TempComboBoxColumnИмяПараметраИзмерения.Items(TempComboBoxColumnИмяПараметраИзмерения.Items.IndexOf(arrСоответствие(I).strИмяБазы))
        '    heter_row.Cells(3).Value = CType(CStr(arrСоответствие(I).NБазы), Object)
        '    heter_row.Cells(4).Value = CType(arrСоответствие(I).strОписание, Object)
        '    heter_row.Cells(5).Value = TempComboBoxColumnИмяБазовогоПараметра.Items(TempComboBoxColumnИмяБазовогоПараметра.Items.IndexOf(arrСоответствие(I).strИмяБазовогоПараметра))
        '    heter_row.Cells(6).Value = TempComboBoxColumnЕдИзмВходная.Items(TempComboBoxColumnЕдИзмВходная.Items.IndexOf(arrСоответствие(I).strРазмерностьВходная))
        '    heter_row.Cells(7).Value = TempComboBoxColumnЕдИзмВыходная.Items(TempComboBoxColumnЕдИзмВыходная.Items.IndexOf(arrСоответствие(I).strРазмерностьВыходная))
        '    heter_row.Cells(8).Value = TempComboBoxColumnТипДавления.Items(TempComboBoxColumnТипДавления.Items.IndexOf(arrСоответствие(I).strТипДавления))

        '    DGVСоответсвие.Rows.Add(heter_row)
        'Next 

        SetCellValueChangedEvents()
        CheckParameters() ' здесь также заполнение массива

        If Not (IsSwohSnapshot) AndAlso fMainFormBase.varFormGraf.IsGraphAct Then
            fMainFormBase.varFormGraf.TuneViewParameters()
        End If
    End Sub

    ''' <summary>
    ''' Вызов методов обработчиков событий для настройки видимости зависящих ячеек.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetCellValueChangedEvents()
        'Dim I As Integer
        'Dim tableИзмеренныеПараметры As BaseFormDataSet.ИзмеренныеПараметрыDataTable = varИзмеренныеПараметры 'fMainFormBase.varfrmИзмеренныеПараметры.BaseFormDataSet.ИзмеренныеПараметры

        'For Each rowИзмеренныйПараметр As BaseFormDataSet.ИзмеренныеПараметрыRow In MeasurementDataTable.Rows
        For I As Integer = 0 To MeasurementDataTable.Rows.Count - 1
            fMainFormBase.varMeasurementParameters.DataGridViewMeasurement_CellValueChanged(fMainFormBase.varMeasurementParameters.DataGridViewMeasurement.Rows(I).Cells(ColumnIndex_UseConstant), New DataGridViewCellEventArgs(ColumnIndex_UseConstant, I))
            fMainFormBase.varMeasurementParameters.DataGridViewMeasurement_CellValueChanged(fMainFormBase.varMeasurementParameters.DataGridViewMeasurement.Rows(I).Cells(ColumnIndex_NameBaseParameter), New DataGridViewCellEventArgs(ColumnIndex_NameBaseParameter, I))
            'I += 1
        Next

        'Dim tableНастроечныеПараметры As BaseFormDataSet.НастроечныеПараметрыDataTable = fMainFormBase.varfrmНастроечныеПараметры.BaseFormDataSet.НастроечныеПараметры
        'I = 0
        'For Each rowНастроечныйПараметр As BaseFormDataSet.НастроечныеПараметрыRow In tableНастроечныеПараметры.Rows
        '    fMainFormBase.varfrmНастроечныеПараметры.DataGridViewНастроечные_CellValueChanged(fMainFormBase.varfrmНастроечныеПараметры.DataGridViewНастроечные.Rows(I).Cells(ИспользоватьЛогикуColumnIndex), New System.Windows.Forms.DataGridViewCellEventArgs(ИспользоватьЛогикуColumnIndex, I))
        '    I += 1
        'Next

        NeedToRewrite = False
    End Sub

    ''' <summary>
    ''' Проверка соответствия параметров измерения и базовых параметров для давления.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckParameters()
        Dim index, lineCount As Integer
        Dim messageErrors As New StringBuilder

        ' проверка соответствия
        ' если номера изменились, то перепишутся новые, если параметр отсутствует, то ему присвоится conПараметрОтсутствует
        NeedToRewrite = False
        mIsCheckPassed = False

        For Each rowИзмеренныйПараметр As BaseFormDataSet.ИзмеренныеПараметрыRow In MeasurementDataTable.Rows
            rowИзмеренныйПараметр.ИндексКаналаИзмерения = 0

            'If IsDBNull(rowИзмеренныйПараметр.ИмяБазовогоПараметра) Then
            '    rowИзмеренныйПараметр.ИмяБазовогоПараметра = ""
            'End If
            If rowИзмеренныйПараметр.ИмяБазовогоПараметра <> "" Then
                ' проверить есть ли такой в списке замеряемых
                index = Array.IndexOf(NameBaseParameters, rowИзмеренныйПараметр.ИмяБазовогоПараметра)

                If index = -1 Then rowИзмеренныйПараметр.ИмяБазовогоПараметра = ""
            End If

            'If IsDBNull(rowИзмеренныйПараметр.ИмяКаналаИзмерения) Then
            '    rowИзмеренныйПараметр.ИмяБазовогоПараметра = conПараметрОтсутствует
            'End If
            ' проверять только в случае если конкретный параметр
            If rowИзмеренныйПараметр.ИмяКаналаИзмерения <> PARAMETER_IS_NOTHING Then
                index = Array.IndexOf(NameRegistrationParameters, rowИзмеренныйПараметр.ИмяКаналаИзмерения)

                If index = -1 OrElse rowИзмеренныйПараметр.ИспользоватьКонстанту Then
                    rowИзмеренныйПараметр.ИмяКаналаИзмерения = PARAMETER_IS_NOTHING
                    rowИзмеренныйПараметр.ИндексКаналаИзмерения = 0
                Else
                    rowИзмеренныйПараметр.ИндексКаналаИзмерения = CShort(mIndexRegistrationParameters(index))
                End If
            End If

            If rowИзмеренныйПараметр.ИмяКаналаИзмерения = PARAMETER_IS_NOTHING AndAlso rowИзмеренныйПараметр.ИспользоватьКонстанту = False Then
                If lineCount >= 21 Then Exit For

                If lineCount < 20 Then
                    messageErrors.AppendLine(rowИзмеренныйПараметр.ИмяПараметра)
                Else
                    messageErrors.AppendLine("И так далее ...")
                End If

                lineCount += 1
            End If
        Next

        If messageErrors.Length <> 0 Then
            MessageBox.Show($"Для следующих параметров нет соответсвия с именами каналов базы данных:{vbCrLf}{messageErrors.ToString}Заполните таблицу в форме <Измеренные параметры>",
                            "Проверка соответствия параметров", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If Not IsSwohSnapshot Then fMainFormBase.varFormGraf.TextBoxCollect.Visible = False
        Else
            'MessageBox.Show("Проверка соответствия параметров выполнена успешно.", "Проверка соответствия параметров", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If FrequencyBackground <> 0 Then 'первый вход из frmMain Me.Manager.FillCombo()
                mIsCheckPassed = True
                If Not IsSwohSnapshot AndAlso Not fMainFormBase.varFormMeasurementKT.TSMenuItemUnlockSettingPages.Checked AndAlso IsCalculateEnable Then fMainFormBase.varFormGraf.TextBoxCollect.Visible = True
            End If
        End If

        CheckSiUnitForMeasurementParameters()
        CheckSiUnitForCalculationParameters()
    End Sub

#Region "Запись таблиц"
    ''' <summary>
    ''' Запись всех таблиц с проверкой соответствия.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SaveTable()
        SaveTableMeasurementParameters()
        SaveTableCalculatedParameters()
        CheckParameters()
        fMainFormBase.GetValueTuningParameters() 'переопределяемый в наследниках метод
        NeedToRewrite = False
    End Sub

    ''' <summary>
    ''' Запись Таблицы Измеренные Параметры
    ''' </summary>
    Private Sub SaveTableMeasurementParameters()
        fMainFormBase.varMeasurementParameters.Validate() ' проверяет значение элемента управления, потерявшего фокус
        fMainFormBase.varMeasurementParameters.ИзмеренныеПараметрыBindingSource.EndEdit() ' применяет ожидающие изменения к базовому источнику данных

        Dim deletedChildRecords = CType(fMainFormBase.varMeasurementParameters.BaseFormDataSet.ИзмеренныеПараметры.GetChanges(DataRowState.Deleted), BaseFormDataSet.ИзмеренныеПараметрыDataTable)
        Dim newChildRecords = CType(fMainFormBase.varMeasurementParameters.BaseFormDataSet.ИзмеренныеПараметры.GetChanges(DataRowState.Added), BaseFormDataSet.ИзмеренныеПараметрыDataTable)
        Dim modifiedChildRecords = CType(fMainFormBase.varMeasurementParameters.BaseFormDataSet.ИзмеренныеПараметры.GetChanges(DataRowState.Modified), BaseFormDataSet.ИзмеренныеПараметрыDataTable)

        With fMainFormBase.varMeasurementParameters
            Try
                If deletedChildRecords IsNot Nothing Then .MeasurementParametersTableAdapter.Update(deletedChildRecords)
                If modifiedChildRecords IsNot Nothing Then .MeasurementParametersTableAdapter.Update(modifiedChildRecords)
                If newChildRecords IsNot Nothing Then .MeasurementParametersTableAdapter.Update(newChildRecords)

                .BaseFormDataSet.AcceptChanges()
            Catch ex As Exception
                MsgBox($"Ошибка обновления в процедуре {NameOf(SaveTableMeasurementParameters)} в BaseForm.")
            Finally
                If Not deletedChildRecords Is Nothing Then deletedChildRecords.Dispose()
                If Not modifiedChildRecords Is Nothing Then modifiedChildRecords.Dispose()
                If Not newChildRecords Is Nothing Then newChildRecords.Dispose()
            End Try
        End With

        Application.DoEvents()
    End Sub

    ''' <summary>
    ''' Запись Таблицы Расчетные Параметры
    ''' </summary>
    Private Sub SaveTableCalculatedParameters()
        fMainFormBase.varCalculatedParameters.Validate() ' проверяет значение элемента управления, потерявшего фокус
        fMainFormBase.varCalculatedParameters.РасчетныеПараметрыBindingSource.EndEdit() ' применяет ожидающие изменения к базовому источнику данных

        Dim deletedChildRecords = CType(fMainFormBase.varCalculatedParameters.BaseFormDataSet.РасчетныеПараметры.GetChanges(DataRowState.Deleted), BaseFormDataSet.РасчетныеПараметрыDataTable)
        Dim newChildRecords = CType(fMainFormBase.varCalculatedParameters.BaseFormDataSet.РасчетныеПараметры.GetChanges(DataRowState.Added), BaseFormDataSet.РасчетныеПараметрыDataTable)
        Dim modifiedChildRecords = CType(fMainFormBase.varCalculatedParameters.BaseFormDataSet.РасчетныеПараметры.GetChanges(DataRowState.Modified), BaseFormDataSet.РасчетныеПараметрыDataTable)

        With fMainFormBase.varCalculatedParameters
            Try
                If deletedChildRecords IsNot Nothing Then .CalculatedParametersTableAdapter.Update(deletedChildRecords)
                If modifiedChildRecords IsNot Nothing Then .CalculatedParametersTableAdapter.Update(modifiedChildRecords)
                If newChildRecords IsNot Nothing Then .CalculatedParametersTableAdapter.Update(newChildRecords)

                .BaseFormDataSet.AcceptChanges()
            Catch ex As Exception
                MsgBox("Ошибка обновления в процедуре ЗаписьТаблицыРасчетныеПараметры в BaseForm.")
            Finally
                If deletedChildRecords IsNot Nothing Then deletedChildRecords.Dispose()
                If modifiedChildRecords IsNot Nothing Then modifiedChildRecords.Dispose()
                If newChildRecords IsNot Nothing Then newChildRecords.Dispose()
            End Try
        End With

        Application.DoEvents()
    End Sub
#End Region

#Region "Xml файл настроек"
    ''' <summary>
    ''' Создание по умолчанию Xml файл настроек.
    ''' </summary>
    ''' <param name="pathSettinngXml"></param>
    ''' <remarks></remarks>
    Private Sub CreateDocumentSettings(ByVal pathSettinngXml As String)
        'Dim nsLMZ As XNamespace = "urn:LMZ-org:lmz"
        'Dim nsProgrammers As XNamespace = "urn:LMZ-org:LmzProgrammers"

        ' создать документ
        'New XDeclaration("1.0", Nothing, Nothing), _
        Dim DocumentSettings As XDocument = New XDocument(
        New XElement("Settings",
                     New XElement("Location",
                                  New XAttribute("Left", 0),
                                  New XAttribute("Top", 0)),
                    New XElement("Size",
                                   New XAttribute("Width", 640),
                                   New XAttribute("Height", 480)),
                    New XElement("WindowState", "Normal"),
                    New XElement("keyКонфигурацияОтображения", "1"),
                    New XElement("Description", "Ввести описание модуля расчета")))

        DocumentSettings.Save(pathSettinngXml)
    End Sub

    ''' <summary>
    ''' Считать положение окна из файла настроек
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadConfiguration()
        Try
            ' использовать Attribute в полном пути Elements 
            'Dim result = document.<art>.<period>.@name

            ' использовать Attribute для потомка для получения первого аттрибута
            'Dim result = document.<Settings>.<Location>.@Width
            'Dim result = document...<Location>.@Width

            ' значени узла
            'result = document...<WindowState>.Value

            ' поиск узла по порядку
            'Dim periodElement = document...<period>(0)
            'result = periodElement.@name

            Dim DocumentSettings = New XDocument
            DocumentSettings = XDocument.Load(PathSettingXml)

            fMainFormBase.Left = CInt(DocumentSettings...<Location>.@Left)
            fMainFormBase.Top = CInt(DocumentSettings...<Location>.@Top)

            fMainFormBase.Width = CInt(DocumentSettings...<Size>.@Width)
            fMainFormBase.Height = CInt(DocumentSettings...<Size>.@Height)

            KeyConfiguration = CInt(DocumentSettings...<keyКонфигурацияОтображения>.Value)
            'Dim name As String = _
            '    System.Enum.GetName(GetType(System.Windows.Forms.FormWindowState), System.Windows.Forms.FormWindowState.Normal)

            Dim strWindowState As String = DocumentSettings...<WindowState>.Value
            Dim valuesFormWindowState As Array = [Enum].GetValues(GetType(FormWindowState))
            Dim tempFormWindowState As FormWindowState = FormWindowState.Normal ' по умолчанию

            For I As Integer = 0 To valuesFormWindowState.Length - 1
                If valuesFormWindowState.GetValue(I).ToString = strWindowState Then
                    tempFormWindowState = CType(valuesFormWindowState.GetValue(I), FormWindowState)
                    Exit For
                End If
            Next

            ' восстановить из сохранённых значений
            fMainFormBase.WindowState = CType(tempFormWindowState, FormWindowState)
            fMainFormBase.Description = DocumentSettings...<Description>.Value
            fMainFormBase.Text = fMainFormBase.Description
        Catch ex As Exception
            MessageBox.Show(CType(Me, IWin32Window), String.Format($"Ошибка в процедуре {NameOf(LoadConfiguration)}.{0}Error: {1}", Environment.NewLine, ex.Message), fMainFormBase.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
#End Region

#Region "Перевод размерностей"
    ''' <summary>
    ''' По входной размерности СГС определить выходную размерность в СИ
    ''' Определить Единицы СИ Для Измеренных Параметров
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckSiUnitForMeasurementParameters()
        Dim success As Boolean
        Dim siUnit As String = String.Empty ' Размерность СИ

        For Each rowИзмеренныйПараметр As BaseFormDataSet.ИзмеренныеПараметрыRow In MeasurementDataTable.Rows
            With rowИзмеренныйПараметр
                If .ИмяКаналаИзмерения <> PARAMETER_IS_NOTHING Then
                    success = ConversionUnit.CheckUnitInSI(.РазмерностьВходная, siUnit)
                    .РазмерностьСИ = siUnit
                    If Not success Then
                        errorDescription = $"Параметр {rowИзмеренныйПараметр.ИмяПараметра} нельзя перевести в единицы СИ."
                        Dim fireDataErrorEventArgs As New IClassCalculation.DataErrorEventArgs($"Процедура: {NameOf(CheckSiUnitForMeasurementParameters)}", errorDescription)
                        RaiseEvent DataError(Me, fireDataErrorEventArgs)
                    End If
                End If
            End With
        Next
    End Sub

    ''' <summary>
    ''' По входной размерности СГС определить выходную размерность в СИ
    ''' Определить Единицы СИ Для Расчетных Параметров
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckSiUnitForCalculationParameters()
        Dim success As Boolean
        Dim siUnit As String = String.Empty ' Размерность СИ

        For Each rowРасчетныйПараметр As BaseFormDataSet.РасчетныеПараметрыRow In CalculatedDataTable.Rows
            With rowРасчетныйПараметр
                .ВычисленноеПереведенноеЗначение = 0
                success = ConversionUnit.CheckUnitInSI(.РазмерностьВыходная, siUnit)
                .РазмерностьСИ = siUnit

                If Not success Then
                    errorDescription = String.Format("Параметр {0} нельзя перевести в единицы СИ.", .ИмяПараметра)
                    'Dim fireDataErrorEventArgs As New IClassCalculation.DataErrorEventArgs("Процедура: ОпределитьЕдиницыСИДляРасчетныхПараметров", errorDescription)
                    Dim fireDataErrorEventArgs As New IClassCalculation.DataErrorEventArgs($"Процедура: {NameOf(CheckSiUnitForCalculationParameters)}", errorDescription)
                    RaiseEvent DataError(Me, fireDataErrorEventArgs)
                End If
            End With
        Next
    End Sub

    ''' <summary>
    ''' По входной размерности СГС перевести входное значение в выходное значение в единицах СИ
    ''' Перевод В Единицы СИ Измеренные Параметры
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub СonversionToSiUnitMeasurementParameters()
        'Dim success As Boolean
        'Dim inRange As Boolean
        'Dim valueSI As Double

        For Each rowИзмеренныйПараметр As BaseFormDataSet.ИзмеренныеПараметрыRow In MeasurementDataTable.Rows
            'inRange = True
            With rowИзмеренныйПараметр
                If .ИмяКаналаИзмерения <> PARAMETER_IS_NOTHING OrElse .ИспользоватьКонстанту Then
                    .ЗначениеВСИ = .ИзмеренноеЗначение

                    '--- внимание закоментировал!!! ---------------------------
                    'success = ConversionUnit.СonversionValueToSI(.РазмерностьВходная, .ИзмеренноеЗначение, valueSI)
                    '.ЗначениеВСИ = valueSI

                    'If Not success Then
                    '    errorDescription = $"Параметр {rowИзмеренныйПараметр.ИмяПараметра} нельзя перевести в единицы СИ."
                    '    Dim fireDataErrorEventArgs As New IClassCalculation.DataErrorEventArgs($"Процедура: {NameOf(CheckSiUnitForCalculationParameters)}", errorDescription)
                    '    RaiseEvent DataError(Me, fireDataErrorEventArgs)
                    'End If

                    '--- внимание закоментировал!!! ---------------------------
                    'If .ТипДавления = Разрежение Then
                    '    blnВДиапазоне = РазрежениеВНорме(.РазмерностьВходная, .Item("ИзмеренноеЗначение"))
                    'ElseIf .ТипДавления = Давление Then
                    '    blnВДиапазоне = ДавлениеВНорме(.РазмерностьВходная, .Item("ИзмеренноеЗначение"))
                    'End If
                    'Select Case CStr(vKey)
                    '    Case "Вк"
                    '        blnВДиапазоне = ДавлениеВНорме(.РазмерностьВходная, .Item("ИзмеренноеЗначение"))
                    '    Case "Вб"
                    '        blnВДиапазоне = РазрежениеВНорме(.РазмерностьВходная, .Item("ИзмеренноеЗначение"))
                    'End Select

                    'If Not inRange Then
                    '    errorDescription = $"Параметр {rowИзмеренныйПараметр.ИмяПараметра} вне допустимого диапазона."
                    '    Dim fireDataErrorEventArgs As New IClassCalculation.DataErrorEventArgs($"Процедура: {NameOf(CheckSiUnitForCalculationParameters)}", errorDescription)
                    '    RaiseEvent DataError(Me, fireDataErrorEventArgs)
                    'End If
                End If
            End With
        Next
    End Sub

    ''' <summary>
    ''' Вычисления параметров из величины в СИ в выходную со специфической размерностью
    ''' Перевод В Настоечные Единицы Расчетных Параметров
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub СonversionToTuningUnitCalculationParameters()
        'Dim success As Boolean
        'Dim выхНастроечноеЗначение As Double

        For Each rowРасчетныйПараметр As BaseFormDataSet.РасчетныеПараметрыRow In CalculatedDataTable.Rows
            With rowРасчетныйПараметр
                ' не использую перевод в СИ, а сразу присваиваю значения
                .ВычисленноеПереведенноеЗначение = .ВычисленноеЗначениеВСИ

                '--- внимание закоментировал!!! ---------------------------
                'success = ConversionUnit.СonversionSIValueSIToTuningValue(.РазмерностьСИ, .ВычисленноеЗначениеВСИ, .РазмерностьВыходная, выхНастроечноеЗначение)
                '.ВычисленноеПереведенноеЗначение = выхНастроечноеЗначение
                'If Not success Then
                '    errorDescription = $"Параметр { .ИмяПараметра} нельзя перевести в настроечные единицы."
                '    Dim fireDataErrorEventArgs As New IClassCalculation.DataErrorEventArgs($"Процедура: {NameOf(СonversionToTuningUnitCalculationParameters)}", errorDescription)
                '    RaiseEvent DataError(Me, fireDataErrorEventArgs)
                'End If
            End With
        Next
    End Sub

    ''' <summary>
    ''' Для абсолютных давлений с учётом базового давления
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CalculationBasePressure()
        Dim rowFoundedByBase As BaseFormDataSet.ИзмеренныеПараметрыRow

        Try
            For Each rowИзмеренныйПараметр As BaseFormDataSet.ИзмеренныеПараметрыRow In MeasurementDataTable.Rows
                If rowИзмеренныйПараметр.ИмяБазовогоПараметра <> "" AndAlso (rowИзмеренныйПараметр.ИмяКаналаИзмерения <> PARAMETER_IS_NOTHING OrElse rowИзмеренныйПараметр.ИспользоватьКонстанту) Then

                    rowFoundedByBase = MeasurementDataTable.FindByИмяПараметра(rowИзмеренныйПараметр.ИмяБазовогоПараметра)

                    If rowFoundedByBase IsNot Nothing Then
                        If rowFoundedByBase.ИмяКаналаИзмерения = PARAMETER_IS_NOTHING AndAlso Not (rowFoundedByBase.ИспользоватьКонстанту) Then
                            rowИзмеренныйПараметр.ЗначениеВСИ = con9999999
                        Else
                            If rowИзмеренныйПараметр.ТипДавления = VACUUM Then
                                '"Вб""bРстМ1""bРстМ2""bРполнМ1""bРполнМ2""Н(бст)""В(мст)"
                                rowИзмеренныйПараметр.ЗначениеВСИ = Abs(rowFoundedByBase.ЗначениеВСИ) - Abs(rowИзмеренныйПараметр.ЗначениеВСИ)
                            ElseIf rowИзмеренныйПараметр.ТипДавления = PRESSURE Then
                                rowИзмеренныйПараметр.ЗначениеВСИ = Abs(rowFoundedByBase.ЗначениеВСИ) + Abs(rowИзмеренныйПараметр.ЗначениеВСИ)
                            End If
                        End If

                    End If 'IsNot Nothing
                End If 'rowИзмеренныйПараметр.ИмяБазовогоПараметра <> ""
            Next
        Catch ex As Exception
            errorDescription = $"Процедура: {NameOf(CalculationBasePressure)}"
            'перенаправление встроенной ошибки
            Dim fireDataErrorEventArgs As New IClassCalculation.DataErrorEventArgs(ex.Message, errorDescription)
            '  Теперь вызов события с помощью вызова делегата. Проходя в
            '  object которое инициирует  событие (Me) такое же как FireEventArgs. 
            '  Вызов обязан соответствовать сигнатуре FireEventHandler.
            RaiseEvent DataError(Me, fireDataErrorEventArgs)
        End Try
    End Sub

    ''' <summary>
    ''' Определение нахождения в разумном допуске значения величины замера.
    ''' </summary>
    ''' <param name="inUnit"></param>
    ''' <param name="inValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsPressureInRange(ByVal inUnit As String, ByVal inValue As Double) As Boolean
        inValue = Abs(inValue)

        Select Case inUnit
            Case "Па"
                '            If Значение < 0 Or Значение > 2500000 Then
                If inValue > 2500000 Then
                    Return False
                End If
                Exit Select
            Case "кПа"
                '            If Значение < 0 Or Значение > 2500 Then
                If inValue > 2500 Then
                    Return False
                End If
                Exit Select
            Case "Мпа"
                '            If Значение < 0 Or Значение > 2.5 Then
                If inValue > 2.5 Then
                    Return False
                End If
                Exit Select
            Case "Н/см^2"
                '            If Значение < 0 Or Значение > 250 Then
                If inValue > 250 Then
                    Return False
                End If
                Exit Select
            Case "дин/см^2"
                '            If Значение < 0 Or Значение > 25000000 Then
                If inValue > 25000000 Then
                    Return False
                End If
                Exit Select
            Case "бар"
                '            If Значение < 0 Or Значение > 25 Then
                If inValue > 25 Then
                    Return False
                End If
                Exit Select
            Case "кгс/м^2"
                '            If Значение < 0 Or Значение > 250000 Then
                If inValue > 250000 Then
                    Return False
                End If
                Exit Select
            Case "кгс/см^2"
                '            If Значение < 0 Or Значение > 25 Then
                If inValue > 25 Then
                    Return False
                End If
                Exit Select
            Case "мм.вод.ст"
                '            If Значение < 0 Or Значение > 250000 Then
                If inValue > 250000 Then
                    Return False
                End If
                Exit Select
            Case "мм.рт.ст"
                '            If Значение < 0 Or Значение > 19000 Then
                If inValue > 19000 Then
                    Return False
                End If
                Exit Select
            Case "атм"
                '            If Значение < 0 Or Значение > 25 Then
                If inValue > 25 Then
                    Return False
                End If
                Exit Select
            Case Else
                Exit Select
        End Select

        Return True
    End Function

    ''' <summary>
    ''' Определение нахождения в разумном допуске значения величины замера.
    ''' </summary>
    ''' <param name="inUnit"></param>
    ''' <param name="outValue">переводится в абсолютное значение</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsValueInRange(ByRef inUnit As String, ByRef outValue As Double) As Boolean
        outValue = Abs(outValue)

        Select Case inUnit
            Case "Па"
                '            If Значение < 0 Or Значение > 25000 Then
                If outValue > 25000 Then
                    Return False
                End If
                Exit Select
            Case "кПа"
                '            If Значение < 0 Or Значение > 25 Then
                If outValue > 25 Then
                    Return False
                End If
                Exit Select
            Case "Мпа"
                '            If Значение < 0 Or Значение > 0.025 Then
                If outValue > 0.025 Then
                    Return False
                End If
                Exit Select
            Case "Н/см^2"
                '            If Значение < 0 Or Значение > 2.5 Then
                If outValue > 2.5 Then
                    Return False
                End If
                Exit Select
            Case "дин/см^2"
                '            If Значение < 0 Or Значение > 250000 Then
                If outValue > 250000 Then
                    Return False
                End If
                Exit Select
            Case "бар"
                '            If Значение < 0 Or Значение > 0.25 Then
                If outValue > 0.25 Then
                    Return False
                End If
                Exit Select
            Case "кгс/м^2"
                '            If Значение < 0 Or Значение > 2500 Then
                If outValue > 2500 Then
                    Return False
                End If
                Exit Select
            Case "кгс/см^2"
                '            If Значение < 0 Or Значение > 0.25 Then
                If outValue > 0.25 Then
                    Return False
                End If
                Exit Select
            Case "мм.вод.ст"
                '            If Значение < 0 Or Значение > 2500 Then
                If outValue > 2500 Then
                    Return False
                End If
                Exit Select
            Case "мм.рт.ст"
                '            If Значение < 0 Or Значение > 190 Then
                If outValue > 190 Then
                    Return False
                End If
                Exit Select
            Case "атм"
                '            If Значение < 0 Or Значение > 0.25 Then
                If outValue > 0.25 Then
                    Return False
                End If
                Exit Select
            Case Else
                Exit Select
        End Select

        Return True
    End Function
#End Region

#Region "Накопление параметров и расчёт КТ"
    ' ИменаПараметровРегистратора сделать доступными не только для чтения
    ' если If СчетчикНакоплений = intНакопленийКТ-2 Then
    ' за одну единицу меньше чем окончание то
    ' выставить в BaseFormKT.frmBaseKT флаг ЗапросВсех на получение значения всех параметров
    ' в событии mФормаРегистраторЛокальнаяСсылка_frmMainAcquiredData
    ' в переменной If ЦиклBaseForm.Manager.ЗапросЗначенийВсехКаналов
    ' передать в свойство ЦиклBaseForm.Manager.ЗначенияВсехКаналов массив Dim arrAcquiredData As Double()
    ' в методе set свойства Manager.ЗначенияВсехКаналов сбросить флаг varЗапросЗначенийВсехКаналов
    ' чтобы в событии mФормаРегистраторЛокальнаяСсылка_frmMainAcquiredData больше не передавать массив arrAcquiredData
    ' в методе формы  fMainFormBase.varfrmСнятиеКТ.ЗаписатьКТ() массив Manager.ЗначенияВсехКаналов и массив Manager.ИменаПараметровРегистратора
    ' записывается в дополнительную таблицу связанную с КТ
    ' при желании эта таблица дополнительно выводится

    ''' <summary>
    ''' Накопить Значения Измеренных И Расчетных Параметров
    ''' </summary>
    Public Sub AcquisitionValueMeasurementAndCalculateParameters()
        ' вызывается из Calculate из ClassCalculation Implements BaseFormKT.IClassCalculation
        If fMainFormBase.varFormGraf.IsGraphAct Then fMainFormBase.varFormGraf.UpdateGraph()

        ' где-то в цикле накопления
        If IsCalculatingKT Then
            ' после перевода измеренных каналов в единицы Си и учета базовых величин rowИзмеренныйПараметр.ЗначениеВСИ идет только в расчет
            For Each row As BaseFormDataSet.ИзмеренныеПараметрыRow In MeasurementDataTable.Rows
                row.НакопленноеЗначение += row.ИзмеренноеЗначение
            Next

            For Each row As BaseFormDataSet.РасчетныеПараметрыRow In CalculatedDataTable.Rows
                row.НакопленноеЗначение += row.ВычисленноеПереведенноеЗначение
            Next

            If CounterAppend = CounterKT - 2 Then mIsQueryChannelsValue = True ' выставить флаг запроса значений всех каналов

            CounterAppend += 1
            fMainFormBase.varFormMeasurementKT.TSTextBoxRemainedUntilEnd.Text = CStr(CounterKT - CounterAppend)

            If CounterAppend >= CounterKT Then
                ' конец замера поля
                IsCalculatingKT = False

                For Each row As BaseFormDataSet.ИзмеренныеПараметрыRow In MeasurementDataTable.Rows
                    row.НакопленноеЗначение = row.НакопленноеЗначение / CounterAppend
                Next

                For Each row As BaseFormDataSet.РасчетныеПараметрыRow In CalculatedDataTable.Rows
                    row.НакопленноеЗначение = row.НакопленноеЗначение / CounterAppend
                Next

                ' здесь уведомить об окончании сбора
                fMainFormBase.varFormMeasurementKT.TSTextBoxRemainedUntilEnd.Text = "0"
                fMainFormBase.varFormMeasurementKT.ProcessAcquisitionKT()
                fMainFormBase.varFormMeasurementKT.TypeRecord = FormMeasurementKT.RecordType.Acquisition
                CounterAppend = 0
            End If
        End If
    End Sub

    ''' <summary>
    ''' Обнулить Накопленные Значения
    ''' </summary>
    Public Sub ClearAcquisitionValue()
        For Each row As BaseFormDataSet.ИзмеренныеПараметрыRow In MeasurementDataTable.Rows
            row.НакопленноеЗначение = 0
        Next

        For Each row As BaseFormDataSet.РасчетныеПараметрыRow In CalculatedDataTable.Rows
            row.НакопленноеЗначение = 0
        Next
    End Sub
#End Region

#Region "Получить Значение Настроечного Параметра"
    ''' <summary>
    ''' Получить Значение Настроечного Параметра
    ''' </summary>
    ''' <param name="StageType"></param>
    ''' <param name="keyControlName"></param>
    ''' <returns></returns>
    Public Function GetValueTuningParameter(ByVal StageType As String, ByVal keyControlName As String) As String
        Dim success As Boolean
        Dim valControl As String = con9999999.ToString

        If StageNames.Contains(StageType) Then
            If ControlsForPhase.Item(StageType).ContainsKey(keyControlName) Then
                success = True
                valControl = GetControlValue(ControlsForPhase.Item(StageType).Item(keyControlName))

                If valControl = "" Then valControl = con9999999.ToString
                'или
                'valControl = Me.ControlsForPhase.Item(ТипЭтапа).Item(keyControlName).ЗначениеПользователя
            End If
        End If

        If Not success Then
            errorDescription = $"Параметр: {keyControlName} для этапа: & {StageType} отсутствует.{vbCrLf}Произведите настройку конфигурации параметров!"
            Dim fireDataErrorEventArgs As New IClassCalculation.DataErrorEventArgs($"Процедура: {NameOf(GetValueTuningParameter)}", errorDescription)
            RaiseEvent DataError(Me, fireDataErrorEventArgs)
        End If

        Return valControl
    End Function

    ''' <summary>
    ''' Выдать Значение Контрола
    ''' </summary>
    ''' <param name="tuningControl"></param>
    ''' <returns></returns>
    Private Function GetControlValue(ByVal tuningControl As MDBControlLibrary.IUserControl) As String
        '    'Dim str As Type = Type.GetType("System.Double")
        '    ''valueControl = CType(valControl, str)
        '    'Select Case strType
        '    '    Case "System.Double"
        '    '        Return Double.Parse(T)
        '    '    Case "System.Int32"
        '    '    Case "System.String"
        '    '    Case "System.Boolean"
        '    'End Select

        Dim userValue As String = Nothing

        Select Case tuningControl.EnumOfType
            Case EnumTypeOfControls.CheckBox
                Dim value As Boolean
                Try
                    value = Boolean.Parse(tuningControl.UserValue)
                Catch ex As Exception
                    value = False
                End Try
                userValue = value.ToString
                Exit Select
            Case EnumTypeOfControls.DateBox
                Dim value As Date
                Try
                    value = Date.Parse(tuningControl.UserValue)
                Catch ex As Exception
                    value = Date.Today
                End Try
                userValue = value.ToShortDateString
                Exit Select
            Case EnumTypeOfControls.DigitalBox
                Dim value As Double
                Try
                    value = Double.Parse(tuningControl.UserValue)
                Catch ex As Exception
                    value = con9999999
                End Try
                userValue = value.ToString
                Exit Select
            Case EnumTypeOfControls.ListBox, EnumTypeOfControls.TextBox, EnumTypeOfControls.ComboBox
                Dim value As String
                Try
                    value = tuningControl.UserValue
                Catch ex As Exception
                    value = ""
                End Try
                userValue = value
                Exit Select
            Case EnumTypeOfControls.TimeBox
                Dim value As Date
                Try
                    value = Date.Parse(tuningControl.UserValue)
                Catch ex As Exception
                    value = Date.Now
                End Try
                userValue = value.ToShortTimeString
                Exit Select
        End Select

        Return userValue
    End Function
#End Region

    ''' <summary>
    ''' Показать Режим двигателя
    ''' </summary>
    ''' <param name="regime"></param>
    Public Sub ShowEngineRegime(ByVal regime As String)
        fMainFormBase.varFormGraf.TSTextBoxregime.Text = regime
    End Sub
End Class

'Public Sub ДобавитьКолонкиТаблицыИзмеренные()
'    '    Dim tableИзмеренныеПараметры As BaseFormDataSet.ИзмеренныеПараметрыDataTable = fMainFormBase.varfrmИзмеренныеПараметры.BaseFormDataSet.ИзмеренныеПараметры
'    '    'tableИзмеренныеПараметры.Columns.Add("ИзмеренноеЗначение", System.Type.GetType("Double"), 0)
'    '    'tableИзмеренныеПараметры.Columns.Add("ЗначениеПереведенное", System.Type.GetType("Double"), 0)
'    '    'tableИзмеренныеПараметры.Columns.Add("РазмерностьСИ", System.Type.GetType("String"), "")
'    '    'tableИзмеренныеПараметры.Columns.Add("ЗначениеВСИ", System.Type.GetType("Double"), 0)

'    Dim columnTemp As System.Data.DataColumn

'    '    columnTemp = New System.Data.DataColumn("ИзмеренноеЗначение", GetType(Double), Nothing, System.Data.MappingType.Element)
'    '    tableИзмеренныеПараметры.Columns.Add(columnTemp)

'    '    columnTemp = New System.Data.DataColumn("ЗначениеПереведенное", GetType(String), Nothing, System.Data.MappingType.Element)
'    '    tableИзмеренныеПараметры.Columns.Add(columnTemp)

'    '    columnTemp = New System.Data.DataColumn("РазмерностьСИ", GetType(String), Nothing, System.Data.MappingType.Element)
'    '    tableИзмеренныеПараметры.Columns.Add(columnTemp)

'    '    columnTemp = New System.Data.DataColumn("ЗначениеВСИ", GetType(Double), Nothing, System.Data.MappingType.Element)
'    '    tableИзмеренныеПараметры.Columns.Add(columnTemp)
'    columnTemp = New System.Data.DataColumn("ДопускМинимум", GetType(Double), Nothing, System.Data.MappingType.Element)
'    fMainFormBase.varfrmИзмеренныеПараметры.BaseFormDataSet.ИзмеренныеПараметры.Columns.Add(columnTemp)
'    columnTemp = New System.Data.DataColumn("ДопускМаксимум", GetType(Double), Nothing, System.Data.MappingType.Element)
'    fMainFormBase.varfrmИзмеренныеПараметры.BaseFormDataSet.ИзмеренныеПараметры.Columns.Add(columnTemp)


'    '    varИзмеренныеПараметры = fMainFormBase.varfrmИзмеренныеПараметры.BaseFormDataSet.ИзмеренныеПараметры
'End Sub