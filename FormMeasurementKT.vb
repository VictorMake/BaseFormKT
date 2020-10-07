Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports MDBControlLibrary
Imports MDBControlLibrary.UserControl
Imports TaskClientServerLibrary
Imports TaskClientServerLibrary.Clobal

' менять ключи в app.config
' <add key="CalculationAssemblyFilename" value="G:\DiskD\ПрограммыVBNET\Registration\bin\Ресурсы\ПодсчетПараметров\CalculationDLLDotNet.NET\bin\CalculationDLLDotNet.dll" />
' <add key="CalculationClassName" value="CalculationDLLDotNet.ClassCalculation" />
' <add key="DiagramClassName" value="CalculationDLLDotNet.ClassDiagram" />
' <add key="clsСвойстваName" value="CalculationDLLDotNet.clsСвойства" />

Public Class FormMeasurementKT
    Public Enum RecordType
        ''' <summary>
        ''' Сбор
        ''' </summary>
        Acquisition
        ''' <summary>
        ''' Пересчет
        ''' </summary>
        Recalculation
    End Enum

    Public Property TypeRecord() As RecordType
    Private mFormParrent As frmBaseKT

    Public keyNewNumberKT As Integer
    Private keyNewTypeEngine As Integer
    Private keyNewNumberEngine As Integer
    Private keyNewNumberBuild As Integer
    Private keyNewNumberStage As Integer
    Private keyNewNumberStarting As Integer

    Private idTypeEngine As Integer
    Private idNumberEngine As Integer
    Private idNumberBuild As Integer
    Private idNumberStage As Integer
    Private idNumberStarting As Integer
    Private idNumberKT As Integer

    ''' <summary>
    ''' Время Начала
    ''' </summary>
    Private startAcquisition As Date
    ''' <summary>
    ''' Время Конца
    ''' </summary>
    Private stopAcquisition As Date

    ''' <summary>
    ''' Возможность возврата счётчика КТ при отмене
    ''' </summary>
    Private isAbleDecrementCountKT As Boolean
    ''' <summary>
    ''' блокировка Включена
    ''' </summary>
    Private isBlocking As Boolean
    ''' <summary>
    ''' загрузка Последней Записи
    ''' </summary>
    Private isLastRecord As Boolean
    Private isFormLoaded As Boolean
    Private isAfterSelectNeed As Boolean = True
    Private isBeforeExpandNeed As Boolean = True
    Private isSelectedIndexChangedNeed As Boolean = True

    Private StageNames As String() = {TypeEngine, NumberEngine, NumberBuild, NumberStage, NumberStarting, NumberKT}
    Private StageConstNames As String() = {cTypeEngine, cNumberEngine, cNumberBuild, cNumberStage, cNumberStarting, cNumberKT}

    Private WithEvents UserControlCheckBox As UserControlCheckBox
    Private WithEvents UserControlComboBox As UserControlComboBox
    Private WithEvents UserControlDateBox As UserControlDateBox
    Private WithEvents UserControlDigitalBox As UserControlDigitalBox
    Private WithEvents UserControlListBox As UserControlListBox
    Private WithEvents UserControlTextBox As UserControlTextBox
    Private WithEvents UserControlTimeBox As UserControlTimeBox
    Private mDataGridExcelBook As DataGridExcelBook
    ''' <summary>
    ''' для процедуры события OnRowUpdated
    ''' </summary>
    Private connection As OleDbConnection
    Private newID As Integer = 0
    Private keyTablePhase As String
    ''' <summary>
    ''' последний Номер КТ Максимала
    ''' </summary>
    Private lastNumberKTmaximum As Integer
    ''' <summary>
    ''' номер Первого Запуска При Включении
    ''' </summary>
    Private firstNumberStartingOnOpen As Integer
    Private pathExcelProtocolCalculateKT As String ' Путь Шаблон Подсчет КТ

    Private Const id1 As String = " ID1"
    Private Const id2 As String = " ID2"
    Private Const id3 As String = " ID3"
    Private Const id4 As String = " ID4"
    Private Const id5 As String = " ID5"
    Private Const id6 As String = " ID6"

    Public Sub New(ByVal inFormParrent As frmBaseKT)
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Me.MdiParent = inFormParrent
        mFormParrent = inFormParrent
    End Sub

#Region "FormMeasurementKT_Load"
    Private Sub FormMeasurementKT_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        firstNumberStartingOnOpen = -1
        InitializeOnLoadFormAndChangedDBase()
        mFormParrent.Manager.StageNames = StageNames
        ' включить меню смены базы данных в режиме снимка
        TSMenuItemChangeCurrentDBase.Enabled = mFormParrent.Manager.IsSwohSnapshot
        StatusStripBar.Items("TSStatusLabelDateNow").Text = Date.Today.ToShortDateString
        StatusStripBar.Items("TSStatusLabelTimeNow").Text = Date.Now.ToShortTimeString

        ColorsNet(0) = Drawing.Color.White
        ColorsNet(1) = Drawing.Color.Lime
        ColorsNet(2) = Drawing.Color.Red
        ColorsNet(3) = Drawing.Color.Yellow
        ColorsNet(4) = Drawing.Color.DeepSkyBlue
        ColorsNet(5) = Drawing.Color.Cyan
        ColorsNet(6) = Drawing.Color.Magenta
        ColorsNet(7) = Drawing.Color.Silver

        ColorsCaptionGrid(0) = Drawing.Color.LightSkyBlue
        ColorsCaptionGrid(1) = Drawing.Color.LightBlue
        ColorsCaptionGrid(2) = Drawing.Color.LightCoral
        ColorsCaptionGrid(3) = Drawing.Color.LightCyan
        ColorsCaptionGrid(4) = Drawing.Color.LightGoldenrodYellow
        ColorsCaptionGrid(5) = Drawing.Color.LightGreen
        ColorsCaptionGrid(6) = Drawing.Color.LightPink
        ColorsCaptionGrid(7) = Drawing.Color.LightSalmon
        ColorsCaptionGrid(8) = Drawing.Color.LightSeaGreen

        mDataGridExcelBook = New DataGridExcelBook(DataGridViewReportKT)
        mDataGridExcelBook.GreateEmptyReport(StageConstNames, StageNames, mFormParrent.Manager)
        isFormLoaded = True
    End Sub

    Private Sub FormMeasurementKT_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        If Not mFormParrent.IsWindowClosed Then e.Cancel = True
    End Sub

    Private Sub FormMeasurementKT_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        IsCalculatingKT = False
        mFormParrent = Nothing
    End Sub

    ''' <summary>
    ''' Инициализация При Загрузке И Смене Базы
    ''' </summary>
    Private Sub InitializeOnLoadFormAndChangedDBase()
        Using cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
            cn.Open()
            PopulateControlsForPhase(cn)
            Populate1TypeEngine(cn)
        End Using
    End Sub

    ''' <summary>
    ''' Заполнить Контролы Этапов
    ''' </summary>
    ''' <param name="cn"></param>
    Private Sub PopulateControlsForPhase(ByRef cn As OleDbConnection)
        Dim strSQL, controlsForStage As String
        Dim rdr As OleDbDataReader
        Dim cmd As OleDbCommand = cn.CreateCommand
        Dim DictionaryOfPfase As Dictionary(Of String, IUserControl)

        mFormParrent.Manager.ControlsForPhase.Clear() ' чтобы при смене базы заново не добавить

        For I As Integer = 0 To StageNames.Count - 1
            DictionaryOfPfase = New Dictionary(Of String, IUserControl)

            controlsForStage = "КонтролыДля" & StageNames(I)
            strSQL = "SELECT ТипКонтрола.ТипКонтрола, " & controlsForStage & ".keyКонтролДляУровня, " & controlsForStage & ".МестоНаПанели, " & controlsForStage & ".Name, " & controlsForStage & ".Text, " & controlsForStage & ".Описание, " & controlsForStage & ".InputOrOutput, " & controlsForStage & ".Value, " & controlsForStage & ".Query, " & controlsForStage & ".ЛогическоеЗначение " &
                    "FROM ТипКонтрола " &
                    "RIGHT JOIN " & controlsForStage & " ON ТипКонтрола.keyТипКонтрола = " & controlsForStage & ".keyТипКонтрола " &
                    "ORDER BY " & controlsForStage & ".МестоНаПанели;"
            cmd.CommandText = strSQL
            rdr = cmd.ExecuteReader

            Do While rdr.Read()
                ' Создать контролы конкретного типа
                Select Case CStr(rdr("ТипКонтрола"))
                    Case EnumTypeOfControls.CheckBox.ToString
                        UserControlCheckBox = New UserControlCheckBox
                        SetFieldsForUserControl(cn, rdr, CType(UserControlCheckBox, IUserControl))
                        DictionaryOfPfase.Add(UserControlCheckBox.Name, UserControlCheckBox)
                        AddUserControlOnFlowLayoutPanel(CType(UserControlCheckBox, Control), I + 1)
                        AddHandler UserControlCheckBox.CheckBoxCheckedChanged, AddressOf Me.MouseClickOrValueChangedHandler
                        Exit Select
                    Case EnumTypeOfControls.ComboBox.ToString
                        UserControlComboBox = New UserControlComboBox
                        SetFieldsForUserControl(cn, rdr, CType(UserControlComboBox, IUserControl))
                        DictionaryOfPfase.Add(UserControlComboBox.Name, UserControlComboBox)
                        AddUserControlOnFlowLayoutPanel(CType(UserControlComboBox, Control), I + 1)

                        If UserControlComboBox.Query IsNot Nothing Then
                            AddHandler UserControlComboBox.ComboBoxSelectedIndexChanged, AddressOf Me.SelectedIndexChangedQueryHandler
                        Else
                            AddHandler UserControlComboBox.ComboBoxSelectedIndexChanged, AddressOf Me.MouseClickOrValueChangedHandler
                        End If
                        Exit Select
                    Case EnumTypeOfControls.DateBox.ToString
                        UserControlDateBox = New UserControlDateBox
                        SetFieldsForUserControl(cn, rdr, CType(UserControlDateBox, IUserControl))
                        DictionaryOfPfase.Add(UserControlDateBox.Name, UserControlDateBox)
                        AddUserControlOnFlowLayoutPanel(CType(UserControlDateBox, Control), I + 1)
                        Exit Select
                    Case EnumTypeOfControls.DigitalBox.ToString
                        UserControlDigitalBox = New UserControlDigitalBox
                        SetFieldsForUserControl(cn, rdr, CType(UserControlDigitalBox, IUserControl))
                        DictionaryOfPfase.Add(UserControlDigitalBox.Name, UserControlDigitalBox)
                        AddUserControlOnFlowLayoutPanel(CType(UserControlDigitalBox, Control), I + 1)
                        AddHandler UserControlDigitalBox.NumericEditAfterChangeValue, AddressOf Me.NumericEditAfterChangeValueHandler
                        Exit Select
                    Case EnumTypeOfControls.ListBox.ToString
                        UserControlListBox = New UserControlListBox
                        SetFieldsForUserControl(cn, rdr, CType(UserControlListBox, IUserControl))
                        DictionaryOfPfase.Add(UserControlListBox.Name, UserControlListBox)
                        AddUserControlOnFlowLayoutPanel(CType(UserControlListBox, Control), I + 1)

                        If UserControlListBox.Query IsNot Nothing Then
                            AddHandler UserControlListBox.ListBoxSelectedIndexChanged, AddressOf Me.SelectedIndexChangedQueryHandler
                        Else
                            AddHandler UserControlListBox.ListBoxSelectedIndexChanged, AddressOf Me.MouseClickOrValueChangedHandler
                        End If
                        Exit Select
                    Case EnumTypeOfControls.TextBox.ToString
                        UserControlTextBox = New UserControlTextBox
                        SetFieldsForUserControl(cn, rdr, CType(UserControlTextBox, IUserControl))
                        DictionaryOfPfase.Add(UserControlTextBox.Name, UserControlTextBox)
                        AddUserControlOnFlowLayoutPanel(CType(UserControlTextBox, Control), I + 1)
                        AddHandler UserControlTextBox.TextBoxTextChanged, AddressOf Me.MouseClickOrValueChangedHandler
                        Exit Select
                    Case EnumTypeOfControls.TimeBox.ToString
                        UserControlTimeBox = New UserControlTimeBox
                        SetFieldsForUserControl(cn, rdr, CType(UserControlTimeBox, IUserControl))
                        DictionaryOfPfase.Add(UserControlTimeBox.Name, UserControlTimeBox)
                        AddUserControlOnFlowLayoutPanel(CType(UserControlTimeBox, Control), I + 1)
                        Exit Select
                End Select
            Loop

            rdr.Close()
            mFormParrent.Manager.ControlsForPhase.Add(StageNames(I), DictionaryOfPfase)
        Next
    End Sub

    Private Sub MouseClickOrValueChangedHandler(ByVal sender As Object, ByVal e As EventArgs)
        mFormParrent.Manager.NeedToRewrite = True
    End Sub

    'должно быть без Option Strict Private Sub NumericEditAfterChangeValueHandler(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs)
    Private Sub NumericEditAfterChangeValueHandler(ByVal sender As Object, ByVal e As EventArgs)
        mFormParrent.Manager.NeedToRewrite = True
    End Sub

    ''' <summary>
    ''' Выполнить запрос для заполнения списка ComboBox или ListBox зашитый в тексте контрола
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SelectedIndexChangedQueryHandler(ByVal sender As Object, ByVal e As EventArgs)
        mFormParrent.Manager.NeedToRewrite = True
        'For Each item As Object In CheckedListMenu.DropDownItems
        '    If (TypeOf item Is ToolStripMenuItem) Then
        '        Dim itemObject As ToolStripMenuItem = CType(item, ToolStripMenuItem)
        '        itemObject.Checked = False
        '    End If
        'Next
        'Dim selectedItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        'selectedItem.Checked = True

        ''1 способ привести к типу
        ''typeof UserControlListBox2 is MDBControlLibrary.UserControlListBox вернет True
        'If (TypeOf sender Is MDBControlLibrary.UserControlListBox) Then
        '    'If (TypeOf sender Is ListBox) Then
        '    Dim SelectedObject As MDBControlLibrary.UserControlListBox = CType(sender, MDBControlLibrary.UserControlListBox)
        '    'обработка SelectedObject
        '    'MessageBox.Show(SelectedObject.EnumOfType)
        '    'MessageBox.Show(SelectedObject.EnumOfType.ToString)
        'ElseIf (TypeOf sender Is MDBControlLibrary.UserControlComboBox) Then
        '    'ElseIf (TypeOf sender Is ComboBox) Then
        '    Dim SelectedObject As MDBControlLibrary.UserControlComboBox = CType(sender, MDBControlLibrary.UserControlComboBox)
        '    'обработка UserControlComboBox
        '    'MessageBox.Show(SelectedObject.EnumOfType.ToString)
        'End If

        '2 способ привести к интерфейсу
        Dim SelectedObject2 As IUserControl = CType(sender, IUserControl)
        'MessageBox.Show(SelectedObject2.EnumOfType.ToString)
        'MessageBox.Show(SelectedObject2.ЗначениеПользователя)
        'MessageBox.Show(SelectedObject2.EnumOfType)
        'MessageBox.Show(SelectedObject2.GetDescriptionAttribute)


        'ТипИзделия_SelectedIndexChanged заполняется НомерИзделия
        'SELECT [2НомерИзделия].НомерИзделия FROM 1ТипИзделия RIGHT JOIN 2НомерИзделия ON [1ТипИзделия].keyТипИзделия = [2НомерИзделия].keyТипИзделия WHERE ((([1ТипИзделия].ТипИзделия)='<ТипИзделия/>') AND (([1ТипИзделия].keyТипИзделия)=<lngТипИзделия/>)) ORDER BY [2НомерИзделия].НомерИзделия;

        'НомерИзделия_SelectedIndexChanged заполняется НомерСборки
        'SELECT [3НомерСборки].НомерСборки FROM 2НомерИзделия RIGHT JOIN 3НомерСборки ON [2НомерИзделия].keyНомерИзделия = [3НомерСборки].keyНомерИзделия WHERE ((([2НомерИзделия].НомерИзделия)='<НомерИзделия/>') AND (([2НомерИзделия].keyТипИзделия)=<lngТипИзделия/>)) ORDER BY [3НомерСборки].НомерСборки;

        'НомерСборки_SelectedIndexChanged заполняется НомерПостановки
        'SELECT [4НомерПостановки].НомерПостановки FROM 3НомерСборки RIGHT JOIN 4НомерПостановки ON [3НомерСборки].keyНомерСборки = [4НомерПостановки].keyНомерСборки WHERE ((([3НомерСборки].НомерСборки)=<НомерСборки/>) AND (([3НомерСборки].keyНомерИзделия)=<lngНомерИзделия/>)) ORDER BY [4НомерПостановки].НомерПостановки;

        'НомерПостановки_SelectedIndexChanged заполняется НомерЗапуска
        'SELECT [5НомерЗапуска].НомерЗапуска FROM 4НомерПостановки RIGHT JOIN 5НомерЗапуска ON [4НомерПостановки].keyНомерПостановки = [5НомерЗапуска].keyНомерПостановки WHERE ((([4НомерПостановки].НомерПостановки)=<НомерПостановки/>) AND (([4НомерПостановки].keyНомерСборки)=<lngНомерСборки/>)) ORDER BY [5НомерЗапуска].НомерЗапуска;

        'НомерЗапуска_SelectedIndexChanged заполняется НомерКТ
        'SELECT [6НомерКТ].НомерКТ FROM 5НомерЗапуска RIGHT JOIN 6НомерКТ ON [5НомерЗапуска].keyНомерЗапуска = [6НомерКТ].keyНомерЗапуска WHERE ((([5НомерЗапуска].НомерЗапуска)=<НомерЗапуска/>) AND (([5НомерЗапуска].keyНомерПостановки)=<lngНомерПостановки/>)) ORDER BY [6НомерКТ].НомерКТ;

        'MessageBox.Show(SelectedObject2.Name)
        ' проверка на содержание от зависимого контрола '<ИмяКонтрола/>'
        ' вставка вместо </> значение пользователя (или текст) этого контрола
        ' выполнение запроса и заполнение Item

        Dim textQuery As String = SelectedObject2.Query.ToString

        If textQuery.IndexOf("<") <> -1 Then
            ' здесь оставляет скобки
            'Dim Expression As New Regex("<([\w-]+)(/>)")
            'Dim Matches As MatchCollection
            'If Expression.IsMatch(Text) Then
            '    Matches = Expression.Matches(Text)
            '    'For Each Match As Match In Matches

            '    'Next
            '    Dim Match As Match = Matches(0)
            '    MessageBox.Show(Match.Value)
            'End If

            'здесь скобки удаляет
            Dim expression As New Regex("<([\w-]+)(/>)", RegexOptions.IgnoreCase)
            Dim m As Match = expression.Match(textQuery)
            Dim nameOfItem As String
            Dim isControlFound As Boolean ' контрол Найден
            Dim phase As Integer = -1

            ' Код разбора строки. Поскольку строка длинная, 
            ' вместо использования Matches метода использовать более эффективный Match(), который
            ' возвращает только одно значение за раз. Значение свойтва Success определяет
            ' существуют ли ещё найденные значения. NextMatch() использовать в этом случае.
            While m.Success
                nameOfItem = m.Groups(1).Value
                isControlFound = False

                ' поиск контрола
                For I As Integer = 0 To mFormParrent.Manager.ControlsForPhase.Count - 1
                    For Each itemControl As IUserControl In mFormParrent.Manager.ControlsForPhase.Item(StageNames(I)).Values
                        If itemControl.Name = nameOfItem Then
                            ' заменить значением данного контрола
                            textQuery = expression.Replace(textQuery, itemControl.UserValue, 1)
                            isControlFound = True
                            phase = I
                            Exit For
                        End If
                    Next

                    If isControlFound Then Exit For
                Next

                ' заменить выражение значением переменной
                If textQuery.IndexOf("lng") <> -1 Then
                    Select Case nameOfItem
                        Case "lngТипИзделия"
                            textQuery = expression.Replace(textQuery, idTypeEngine.ToString, 1)
                            Exit Select
                        Case "lngНомерИзделия"
                            textQuery = expression.Replace(textQuery, idNumberEngine.ToString, 1)
                            Exit Select
                        Case "lngНомерСборки"
                            textQuery = expression.Replace(textQuery, idNumberBuild.ToString, 1)
                            Exit Select
                        Case "lngНомерПостановки"
                            textQuery = expression.Replace(textQuery, idNumberStage.ToString, 1)
                            Exit Select
                        Case "lngНомерЗапуска"
                            textQuery = expression.Replace(textQuery, idNumberStarting.ToString, 1)
                            Exit Select
                        Case "lngНомерКонтрТочки"
                            textQuery = expression.Replace(textQuery, idNumberKT.ToString, 1)
                            Exit Select
                    End Select
                End If
                ' продолжить в найденных значениях регулярного выражения.
                m = m.NextMatch()
            End While

            ' выполнить модифицированный запрос
            Dim cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
            Dim rdr As OleDbDataReader = Nothing
            Dim strSQL As String = textQuery
            Dim cmd As OleDbCommand = cn.CreateCommand
            Dim strList As String = vbNullString

            cmd.CommandType = CommandType.Text
            cmd.CommandText = strSQL
            cn.Open()

            Try
                rdr = cmd.ExecuteReader
                Do While rdr.Read()
                    strList &= CStr(rdr(0)) & ";"
                Loop
            Catch ex As Exception
                MessageBox.Show(ex.ToString, $"Обработка события <{NameOf(SelectedIndexChangedQueryHandler)}> в запросе Query элемента " &
                                mFormParrent.Manager.ControlsForPhase.Item(StageNames(phase)).Item(StageNames(phase)).Name, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Finally
                rdr.Close()
                cn.Close()
            End Try

            ' удалить последний ";"
            If strList IsNot Nothing Then
                strList = strList.Remove((strList.Length) - 1)
                ' индекс контрола в запросе, где нужно обновить список на 1 больше найденного Phase (запрос конечно не универсален)
                phase += 1
                mFormParrent.Manager.ControlsForPhase.Item(StageNames(phase)).Item(StageNames(phase)).EraseValue()
                mFormParrent.Manager.ControlsForPhase.Item(StageNames(phase)).Item(StageNames(phase)).Value = strList
            End If
        End If
    End Sub

    ''' <summary>
    ''' Добавить UserControl
    ''' здесь наследование от Control
    ''' </summary>
    ''' <param name="userControl"></param>
    ''' <param name="indexTabPages"></param>
    Private Sub AddUserControlOnFlowLayoutPanel(ByRef userControl As Control, ByVal indexTabPages As Integer)
        If indexTabPages = StageNodeType.НомерКТ6 Then
            FlowLayoutPanel6.Controls.Add(userControl)
        Else
            TabControlPhases.TabPages("TabPage" & (indexTabPages).ToString).Controls("FlowLayoutPanel" & (indexTabPages).ToString).Controls.Add(userControl)
        End If
    End Sub

    ''' <summary>
    ''' Заполнить Поля UserControl
    ''' здесь наследование от IControlsOfPfase
    ''' </summary>
    ''' <param name="cn"></param>
    ''' <param name="rdr"></param>
    ''' <param name="IControlsOfPfase"></param>
    Private Sub SetFieldsForUserControl(ByRef cn As OleDbConnection, ByRef rdr As OleDbDataReader, ByRef IControlsOfPfase As IUserControl)
        IControlsOfPfase.IndexLocationOnPanel = CInt(rdr("МестоНаПанели"))
        IControlsOfPfase.Name = CStr(rdr("Name"))

        If Not IsDBNull(rdr("Text")) Then
            IControlsOfPfase.Text = CStr(rdr("Text"))
        Else
            IControlsOfPfase.Text = CStr(0)
        End If

        If Not IsDBNull(rdr("Описание")) Then
            IControlsOfPfase.Description = CStr(rdr("Описание"))
        Else
            IControlsOfPfase.Description = CStr(0)
        End If

        IControlsOfPfase.InputOrOutput = CBool(rdr("InputOrOutput"))
        ' или выполнить запрос к таблице содержащей значения или список уже набран
        ' в последствии при передаче в IControlsOfPfase.Value он преобразуется в лист
        If Not IsDBNull(rdr("Value")) Then
            If IControlsOfPfase.EnumOfType = EnumTypeOfControls.ComboBox OrElse IControlsOfPfase.EnumOfType = EnumTypeOfControls.ListBox Then
                If rdr("Value").ToString.IndexOf("SELECT") = -1 Then
                    ' значение содержит список
                    IControlsOfPfase.Value = CStr(rdr("Value"))
                Else
                    If rdr("Value").ToString.IndexOf("<") = -1 Then
                        ' выполнить запрос на заполнение полей
                        Dim rdrSelect As OleDbDataReader
                        Dim cmd As OleDbCommand = cn.CreateCommand
                        Dim strList As String = vbNullString

                        cmd.CommandText = CStr(rdr("Value"))
                        rdrSelect = cmd.ExecuteReader

                        Do While rdrSelect.Read()
                            strList &= CStr(rdrSelect(0)) & ";"
                        Loop

                        rdrSelect.Close()

                        If strList IsNot Nothing Then strList = strList.Remove((strList.Length) - 1)

                        IControlsOfPfase.Value = strList
                    End If
                End If
            Else
                IControlsOfPfase.Query = CStr(rdr("Value"))
            End If
        End If

        If Not IsDBNull(rdr("Query")) Then
            ' значение списка зависит от зависимого контрола
            ' запрос нужно поместить без модификации
            IControlsOfPfase.Query = CStr(rdr("Query"))
        End If

        IControlsOfPfase.BooleanValue = CBool(rdr("ЛогическоеЗначение"))
        IControlsOfPfase.keyControlStage = CInt(rdr("keyКонтролДляУровня"))
    End Sub

    ''' <summary>
    ''' Заполнить 1ТипыИзделий
    ''' </summary>
    ''' <param name="cn"></param>
    Private Sub Populate1TypeEngine(ByRef cn As OleDbConnection)
        Dim odaDataAdapter As OleDbDataAdapter = New OleDbDataAdapter("SELECT * FROM [1ТипИзделия]", cn)
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow

        ' здесь должны быть все типы изделия
        odaDataAdapter.Fill(dtDataTable)
        TreeViewEngine.Nodes.Clear() ' чтобы не добавлялись при смене БД

        If dtDataTable.Rows.Count > 0 Then
            For Each drDataRow In dtDataTable.Rows
                idTypeEngine = CInt(drDataRow("keyТипИзделия"))
                Dim cRoot As New DirectoryNode("Тип изделия-" & CStr(drDataRow("ТипИзделия")), StageNodeType.ТипыИзделия1, idTypeEngine) With {
                    .SelectedImageIndex = 0,
                    .ImageIndex = 7,
                    .Tag = CStr(idTypeEngine) & id1
                }
                TreeViewEngine.Nodes.Add(cRoot)
                AddDirectories(cRoot, cn)
            Next
        End If
    End Sub

    Private Sub TimerInitializeForm_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles TimerInitializeForm.Tick
        If isFormLoaded Then
            TimerInitializeForm.Enabled = False
            ExpandTreeByLastKT()

            If Not mFormParrent.Manager.IsSwohSnapshot Then
                With mFormParrent.varFormGraf
                    .TSButtonTuneTrand.Enabled = True
                    .ButtonDetail.Enabled = True
                    .TSButtonBounds.Enabled = True
                    .Collect()
                End With
            End If
        End If
    End Sub

    ''' <summary>
    ''' Узнать Последнюю КТ и рекурсивно раскрыть все узлы
    ''' </summary>
    Private Sub ExpandTreeByLastKT()
        Dim cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
        Dim sTypeEngine As String = Nothing
        Dim strSQL As String = Nothing
        Dim rdr As OleDbDataReader
        Dim cmd As OleDbCommand = cn.CreateCommand
        Dim sNumberEngine As String = Nothing
        Dim sNumberBuild As String = Nothing
        Dim sNumberStage As String = Nothing
        Dim sNumberStarting As String = Nothing
        Dim sNumberKT As String = Nothing
        Dim success As Boolean

        idNumberKT = 0
        ' 1 узнать последнюю КТ
        ' 2 узнать keyНомерКТ
        strSQL = "SELECT TOP 1 [6НомерКТ].keyНомерКТ FROM 6НомерКТ ORDER BY [6НомерКТ].keyНомерКТ DESC;"

        cmd.CommandType = CommandType.Text
        cn.Open()
        cmd.CommandText = strSQL
        rdr = cmd.ExecuteReader

        If rdr.Read() Then
            success = True
            idNumberKT = CInt(rdr("keyНомерКТ"))
        End If

        rdr.Close()

        If success Then
            strSQL = "SELECT [1ТипИзделия].[keyТипИзделия], [2НомерИзделия].[keyНомерИзделия], [3НомерСборки].[keyНомерСборки], [4НомерПостановки].[keyНомерПостановки], [5НомерЗапуска].[keyНомерЗапуска], [6НомерКТ].[keyНомерКТ] " &
                "FROM ((((1ТипИзделия RIGHT JOIN 2НомерИзделия ON [1ТипИзделия].[keyТипИзделия]=[2НомерИзделия].[keyТипИзделия]) RIGHT JOIN 3НомерСборки ON [2НомерИзделия].[keyНомерИзделия]=[3НомерСборки].[keyНомерИзделия]) RIGHT JOIN 4НомерПостановки ON [3НомерСборки].[keyНомерСборки]=[4НомерПостановки].[keyНомерСборки]) RIGHT JOIN 5НомерЗапуска ON [4НомерПостановки].[keyНомерПостановки]=[5НомерЗапуска].[keyНомерПостановки]) RIGHT JOIN 6НомерКТ ON [5НомерЗапуска].[keyНомерЗапуска]=[6НомерКТ].[keyНомерЗапуска] " &
                "WHERE ((([6НомерКТ].[keyНомерКТ])=  " & idNumberKT & "))"
            cmd.CommandText = strSQL
            rdr = cmd.ExecuteReader

            If rdr.Read() Then
                idTypeEngine = CInt(rdr("keyТипИзделия"))
                sTypeEngine = CStr(idTypeEngine) & id1
                idNumberEngine = CInt(rdr("keyНомерИзделия"))
                sNumberEngine = CStr(idNumberEngine) & id2
                idNumberBuild = CInt(rdr("keyНомерСборки"))
                sNumberBuild = CStr(idNumberBuild) & id3
                idNumberStage = CInt(rdr("keyНомерПостановки"))
                sNumberStage = CStr(idNumberStage) & id4
                idNumberStarting = CInt(rdr("keyНомерЗапуска"))
                sNumberStarting = CStr(idNumberStarting) & id5
                idNumberKT = CInt(rdr("keyНомерКТ"))
                sNumberKT = CStr(idNumberKT) & id6
            End If

            rdr.Close()
            isLastRecord = False

            If TreeViewEngine.GetNodeCount(False) > 0 Then
                TreeViewEngine.Nodes(TreeViewEngine.GetNodeCount(False) - 1).Expand()
            End If

            For Each lNode As TreeNode In TreeViewEngine.Nodes
                If lNode.Tag.ToString = sTypeEngine Then
                    lNode.Expand()
                    ExpandNode(lNode, StageNodeType.НомерИзделия2, sNumberEngine, sNumberBuild, sNumberStage, sNumberStarting, sNumberKT)
                    isLastRecord = True
                    Exit For
                End If
            Next
        End If

        cn.Close()
        mFormParrent.Manager.NeedToRewrite = True ' чтобы обновить настроечные параметры для закладок
    End Sub

    ''' <summary>
    ''' Раскрыть Узел
    ''' </summary>
    ''' <param name="parrentNode"></param>
    ''' <param name="inPhase"></param>
    ''' <param name="tagNumberEngine"></param>
    ''' <param name="tagNumberBuild"></param>
    ''' <param name="tagNumberStage"></param>
    ''' <param name="tagNumberStarting"></param>
    ''' <param name="tagNumberKT"></param>
    Private Sub ExpandNode(ByVal parrentNode As TreeNode,
                           ByVal inPhase As Integer,
                           ByVal tagNumberEngine As String,
                           ByVal tagNumberBuild As String,
                           ByVal tagNumberStage As String,
                           ByVal tagNumberStarting As String,
                           ByVal tagNumberKT As String)
        parrentNode.Expand()

        For Each itemNode As TreeNode In parrentNode.Nodes
            Select Case inPhase
                Case StageNodeType.НомерИзделия2
                    If itemNode.Tag.ToString = tagNumberEngine Then
                        itemNode.Expand()
                        ExpandNode(itemNode, inPhase + 1, tagNumberEngine, tagNumberBuild, tagNumberStage, tagNumberStarting, tagNumberKT)
                        Exit Sub
                    End If
                    Exit Select
                Case StageNodeType.НомерСборки3
                    If itemNode.Tag.ToString = tagNumberBuild Then
                        itemNode.Expand()
                        ExpandNode(itemNode, inPhase + 1, tagNumberEngine, tagNumberBuild, tagNumberStage, tagNumberStarting, tagNumberKT)
                        Exit Sub
                    End If
                    Exit Select
                Case StageNodeType.НомерПостановки4
                    If itemNode.Tag.ToString = tagNumberStage Then
                        itemNode.Expand()
                        ExpandNode(itemNode, inPhase + 1, tagNumberEngine, tagNumberBuild, tagNumberStage, tagNumberStarting, tagNumberKT)
                        Exit Sub
                    End If
                    Exit Select
                Case StageNodeType.НомерЗапуска5
                    If itemNode.Tag.ToString = tagNumberStarting Then
                        itemNode.Expand()
                        ExpandNode(itemNode, inPhase + 1, tagNumberEngine, tagNumberBuild, tagNumberStage, tagNumberStarting, tagNumberKT)
                        Exit Sub
                    End If
                    Exit Select
                Case StageNodeType.НомерКТ6
                    If itemNode.Tag.ToString = tagNumberKT Then
                        'directoryTree_AfterSelect(tvTreeView, New System.Windows.Forms.TreeViewEventArgs(lNode, TreeViewAction.Expand))
                        TreeViewEngine.SelectedNode = itemNode
                        Exit Sub
                    End If
                    Exit Select
            End Select
        Next
    End Sub
#End Region

#Region "TreeView"
    Private Sub AddDirectories(ByVal node As DirectoryNode, ByRef cn As OleDbConnection)
        Dim rdr As OleDbDataReader
        Dim strSQL As String
        Dim cmd As OleDbCommand = cn.CreateCommand

        cmd.CommandType = CommandType.Text
        node.ImageIndex = StageNodeType.НетПотомков

        Select Case node.NodeType
            Case StageNodeType.ТипыИзделия1 ' родитель ТипыИзделия1
                idTypeEngine = node.KeyId
                strSQL = "SELECT * from [2НомерИзделия] Where keyТипИзделия =  " & idTypeEngine
                cmd.CommandText = strSQL
                rdr = cmd.ExecuteReader

                Do While rdr.Read()
                    node.ImageIndex = node.NodeType
                    idNumberEngine = CInt(rdr("keyНомерИзделия"))
                    node.Nodes.Add(New DirectoryNode("№ изделия-" & CStr(rdr("НомерИзделия")), StageNodeType.НомерИзделия2, idNumberEngine))
                    node.LastNode.SelectedImageIndex = 0
                    node.LastNode.ImageIndex = StageNodeType.НомерИзделия2
                    node.LastNode.Tag = CStr(idNumberEngine) & id2
                Loop
                rdr.Close()

            Case StageNodeType.НомерИзделия2 ' родитель НомерИзделия2
                idNumberEngine = node.KeyId
                strSQL = "SELECT * from [3НомерСборки] Where keyНомерИзделия =  " & idNumberEngine
                cmd.CommandText = strSQL
                rdr = cmd.ExecuteReader

                Do While rdr.Read()
                    node.ImageIndex = node.NodeType
                    idNumberBuild = CInt(rdr("keyНомерСборки"))
                    node.Nodes.Add(New DirectoryNode("№ сборки-" & CStr(rdr("НомерСборки")), StageNodeType.НомерСборки3, idNumberBuild))
                    node.LastNode.SelectedImageIndex = 0
                    node.LastNode.ImageIndex = StageNodeType.НомерСборки3
                    node.LastNode.Tag = CStr(idNumberBuild) & id3
                Loop
                rdr.Close()

            Case StageNodeType.НомерСборки3 ' родитель НомерСборки3
                idNumberBuild = node.KeyId
                strSQL = "SELECT * from [4НомерПостановки] Where keyНомерСборки =  " & idNumberBuild
                cmd.CommandText = strSQL
                rdr = cmd.ExecuteReader

                Do While rdr.Read()
                    node.ImageIndex = node.NodeType
                    idNumberStage = CInt(rdr("keyНомерПостановки"))
                    node.Nodes.Add(New DirectoryNode("№ постановки-" & CStr(rdr("НомерПостановки")), StageNodeType.НомерПостановки4, idNumberStage))
                    node.LastNode.SelectedImageIndex = 0
                    node.LastNode.ImageIndex = StageNodeType.НомерПостановки4
                    node.LastNode.Tag = CStr(idNumberStage) & id4
                Loop
                rdr.Close()

            Case StageNodeType.НомерПостановки4 ' родитель НомерПостановки4
                idNumberStage = node.KeyId
                strSQL = "SELECT * from [5НомерЗапуска] Where keyНомерПостановки =  " & idNumberStage
                cmd.CommandText = strSQL
                rdr = cmd.ExecuteReader

                Do While rdr.Read()
                    node.ImageIndex = node.NodeType
                    idNumberStarting = CInt(rdr("keyНомерЗапуска"))
                    node.Nodes.Add(New DirectoryNode("№ запуска-" & CStr(rdr("НомерЗапуска")), StageNodeType.НомерЗапуска5, idNumberStarting))
                    node.LastNode.SelectedImageIndex = 0
                    node.LastNode.ImageIndex = StageNodeType.НомерЗапуска5
                    node.LastNode.Tag = CStr(idNumberStarting) & id5
                Loop
                rdr.Close()

            Case StageNodeType.НомерЗапуска5 ' родитель НомерЗапуска5
                idNumberStarting = node.KeyId
                strSQL = "SELECT * from [6НомерКТ] Where keyНомерЗапуска =  " & idNumberStarting
                cmd.CommandText = strSQL
                rdr = cmd.ExecuteReader

                Do While rdr.Read()
                    node.ImageIndex = node.NodeType
                    idNumberKT = CInt(rdr("keyНомерКТ"))
                    node.Nodes.Add(New DirectoryNode("№ контр. точки-" & CStr(rdr("НомерКТ")), StageNodeType.НомерКТ6, idNumberKT))
                    node.LastNode.SelectedImageIndex = 0
                    node.LastNode.ImageIndex = StageNodeType.НомерКТ6
                    node.LastNode.Tag = CStr(idNumberKT) & id6
                Loop

                rdr.Close()
        End Select
    End Sub

    Private Sub AddSubDirectories(ByVal node As DirectoryNode)
        Using cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
            cn.Open()

            For I As Integer = 0 To node.Nodes.Count - 1
                AddDirectories(CType(node.Nodes(I), DirectoryNode), cn)
            Next
        End Using

        node.SubDirectoriesAdded = True
    End Sub

    Private Sub ListViewMemberNode_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles ListViewMemberNode.Resize
        For Each itemColumnHeader As ColumnHeader In ListViewMemberNode.Columns
            itemColumnHeader.Width = ListViewMemberNode.Width \ ListViewMemberNode.Columns.Count
        Next
    End Sub

    Private Sub ListViewMemberNode_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ListViewMemberNode.SelectedIndexChanged
        If TreeViewEngine.SelectedNode IsNot Nothing AndAlso CType(TreeViewEngine.SelectedNode, DirectoryNode).NodeType = StageNodeType.НомерКТ6 AndAlso isSelectedIndexChangedNeed Then
            Dim I As Integer
            Dim nodeSelect As DirectoryNode = CType(TreeViewEngine.SelectedNode, DirectoryNode)
            Dim parentNode As TreeNode = nodeSelect.Parent
            Dim nodeSelectInherits As DirectoryNode = Nothing ' получить расширенный узел
            Dim nodeKey As Integer
            Dim success As Boolean ' есть Совпадение

            For I = 0 To ListViewMemberNode.Items.Count - 1
                ListViewMemberNode.Items(I).ImageIndex = StageNodeType.НомерКТ6

                If ListViewMemberNode.Items(I).Selected Then
                    nodeKey = CInt(Val(ListViewMemberNode.Items(I).Tag))
                    ListViewMemberNode.Items(I).ImageIndex = 0
                    success = True
                End If
            Next

            If success Then
                For I = 0 To parentNode.Nodes.Count - 1
                    parentNode.Nodes(I).ImageIndex = StageNodeType.НомерКТ6
                    parentNode.Nodes(I).SelectedImageIndex = StageNodeType.НомерКТ6

                    If Val(parentNode.Nodes(I).Tag) = nodeKey Then nodeSelectInherits = CType(parentNode.Nodes(I), DirectoryNode)
                Next

                If nodeSelectInherits IsNot Nothing Then
                    nodeSelectInherits.ImageIndex = 0
                    TreeViewEngine.SelectedNode = nodeSelectInherits
                    idNumberKT = nodeSelectInherits.KeyId
                    keyNewNumberKT = idNumberKT
                    TSButtonStartAcquisitionKT.Visible = CBool(firstNumberStartingOnOpen = Val(parentNode.Tag))

                    Using cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
                        cn.Open()
                        PopulateListViewMembers(StageNodeType.НомерКТ6 + 1, idNumberKT, cn)
                        PopulateReportKT(idNumberKT, cn)
                    End Using

                    TabControlPhases.SelectedIndex = StageNodeType.НомерКТ6 - 1
                    TabControlPhases.TabPages.Item(StageNodeType.НомерКТ6 - 1).Select()
                    TSButtonRecalculationKT.Visible = mFormParrent.Manager.IsSwohSnapshot
                End If
            End If
        End If
    End Sub

#Region "Очистка Контролов Этапа"
    ''' <summary>
    ''' Очистка Контролов Этапа
    ''' </summary>
    ''' <param name="nameStage"></param>
    Private Sub ClearControlsStage(ByVal nameStage As String)
        For Each itemControl As IUserControl In mFormParrent.Manager.ControlsForPhase.Item(nameStage).Values
            itemControl.EraseValue()
        Next
    End Sub

    Private Sub ClearControlsFor_Number_Engine_Build_Stage_Starting()
        ClearControlsNumberEngine()
        ClearControlsFor_Number_Build_Stage_Starting()
    End Sub

    Private Sub ClearControlsFor_Number_Build_Stage_Starting()
        ClearControlsNumberBuild()
        ClearControlsFor_Number_Stage_Starting()
    End Sub

    Private Sub ClearControlsFor_Number_Stage_Starting()
        ClearControlsNumberStage()
        ClearControlsNumberStarting()
    End Sub

    ''' <summary>
    ''' Очистка Тип Изделия
    ''' </summary>
    Private Sub ClearControlsTypeEngine()
        keyNewTypeEngine = 0
        ClearControlsStage(StageNames(StageNodeType.ТипыИзделия1 - 1))
    End Sub

    ''' <summary>
    ''' Очистка Номер Изделия
    ''' </summary>
    Private Sub ClearControlsNumberEngine()
        keyNewNumberEngine = 0
        ClearControlsStage(StageNames(StageNodeType.НомерИзделия2 - 1))
    End Sub

    ''' <summary>
    ''' Очистка Номер Сборки
    ''' </summary>
    Private Sub ClearControlsNumberBuild()
        keyNewNumberBuild = 0
        ClearControlsStage(StageNames(StageNodeType.НомерСборки3 - 1))
    End Sub

    ''' <summary>
    ''' Очистка Номер Постановки
    ''' </summary>
    Private Sub ClearControlsNumberStage()
        keyNewNumberStage = 0
        ClearControlsStage(StageNames(StageNodeType.НомерПостановки4 - 1))
    End Sub

    ''' <summary>
    ''' Очистка Номер Запуска
    ''' </summary>
    Private Sub ClearControlsNumberStarting()
        keyNewNumberStarting = 0
        ClearControlsStage(StageNames(StageNodeType.НомерЗапуска5 - 1))
    End Sub

    ''' <summary>
    ''' Очистка Номер КТ
    ''' </summary>
    Private Sub ClearControlsNumberKT()
        keyNewNumberKT = 0
        ClearControlsStage(StageNames(StageNodeType.НомерКТ6 - 1))
        TSButtonRecalculationKT.Visible = False
    End Sub
#End Region

    Private Sub TreeViewEngine_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeViewEngine.AfterSelect
        If Not isAfterSelectNeed Then Exit Sub

        Dim nodeSelect As DirectoryNode = CType(e.Node, DirectoryNode)
        Dim listViewItemKT As ListViewItem = Nothing

        Using cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
            cn.Open()

            Select Case nodeSelect.NodeType
                Case StageNodeType.ТипыИзделия1
                    ClearControlsFor_Number_Engine_Build_Stage_Starting()
                    idTypeEngine = nodeSelect.KeyId
                    keyNewTypeEngine = idTypeEngine
                    PopulateListViewMembers(StageNodeType.НомерИзделия2, idTypeEngine, cn)
                    TabControlPhases.SelectedIndex = StageNodeType.ТипыИзделия1 - 1
                    TabControlPhases.TabPages.Item(StageNodeType.ТипыИзделия1 - 1).Select()
                Case StageNodeType.НомерИзделия2 ' "НомерИзделия"
                    ClearControlsFor_Number_Build_Stage_Starting()
                    idNumberEngine = nodeSelect.KeyId
                    keyNewNumberEngine = idNumberEngine
                    PopulateListViewMembers(StageNodeType.НомерСборки3, idNumberEngine, cn)
                    TabControlPhases.SelectedIndex = StageNodeType.НомерИзделия2 - 1
                    TabControlPhases.TabPages.Item(StageNodeType.НомерИзделия2 - 1).Select()
                Case StageNodeType.НомерСборки3 ' "НомерСборки"
                    ClearControlsFor_Number_Stage_Starting()
                    idNumberBuild = nodeSelect.KeyId
                    keyNewNumberBuild = idNumberBuild
                    PopulateListViewMembers(StageNodeType.НомерПостановки4, idNumberBuild, cn)
                    TabControlPhases.SelectedIndex = StageNodeType.НомерСборки3 - 1
                    TabControlPhases.TabPages.Item(StageNodeType.НомерСборки3 - 1).Select()
                Case StageNodeType.НомерПостановки4 ' "НомерПостановки"
                    ClearControlsNumberStarting()
                    idNumberStage = nodeSelect.KeyId
                    keyNewNumberStage = idNumberStage
                    PopulateListViewMembers(StageNodeType.НомерЗапуска5, idNumberStage, cn)
                    TabControlPhases.SelectedIndex = StageNodeType.НомерПостановки4 - 1
                    TabControlPhases.TabPages.Item(StageNodeType.НомерПостановки4 - 1).Select()
                Case StageNodeType.НомерЗапуска5 ' "НомерЗапуска"
                    'постановка = nodeSelect.Parent.Tag
                    idNumberStarting = nodeSelect.KeyId
                    keyNewNumberStarting = idNumberStarting
                    TSButtonStartAcquisitionKT.Visible = CBool(firstNumberStartingOnOpen = keyNewNumberStarting)
                    PopulateListViewMembers(StageNodeType.НомерКТ6, idNumberStarting, cn)
                    TabControlPhases.SelectedIndex = StageNodeType.НомерЗапуска5 - 1
                    TabControlPhases.TabPages.Item(StageNodeType.НомерЗапуска5 - 1).Select()
                    TSButtonRecalculationKT.Visible = False
                Case StageNodeType.НомерКТ6 '"НомерКТ"
                    Dim I As Integer

                    If ListViewMemberNode.Items.Count > 0 Then
                        Dim nodeKey As String = nodeSelect.KeyId.ToString & id1

                        For I = 0 To ListViewMemberNode.Items.Count - 1
                            ListViewMemberNode.Items(I).ImageIndex = StageNodeType.НомерКТ6

                            If ListViewMemberNode.Items(I).Tag.ToString = nodeKey Then listViewItemKT = ListViewMemberNode.Items(I)
                        Next
                    End If

                    If listViewItemKT IsNot Nothing Then
                        isSelectedIndexChangedNeed = False
                        listViewItemKT.ImageIndex = 0
                        listViewItemKT.EnsureVisible()
                        listViewItemKT.Selected = True

                        If Not isLastRecord Then ListViewMemberNode.Focus()

                        isSelectedIndexChangedNeed = True
                    End If

                    Dim lNode As TreeNode = e.Node.Parent

                    For I = 0 To lNode.Nodes.Count - 1
                        lNode.Nodes(I).ImageIndex = StageNodeType.НомерКТ6
                        lNode.Nodes(I).SelectedImageIndex = StageNodeType.НомерКТ6
                    Next

                    nodeSelect.ImageIndex = 0
                    nodeSelect.SelectedImageIndex = 0
                    idNumberKT = nodeSelect.KeyId
                    keyNewNumberKT = idNumberKT
                    TSButtonStartAcquisitionKT.Visible = CBool(firstNumberStartingOnOpen = Val(nodeSelect.Parent.Tag))
                    PopulateListViewMembers(StageNodeType.НомерКТ6 + 1, idNumberKT, cn)
                    PopulateReportKT(idNumberKT, cn)
                    TabControlPhases.SelectedIndex = StageNodeType.НомерКТ6 - 1
                    TabControlPhases.TabPages.Item(StageNodeType.НомерКТ6 - 1).Select()
                    TSButtonRecalculationKT.Visible = mFormParrent.Manager.IsSwohSnapshot
            End Select
        End Using

        isAfterSelectNeed = False
        If isBeforeExpandNeed Then e.Node.Expand()
        isAfterSelectNeed = True
        SetEnabledRemoveToolStripMenuItem(nodeSelect.NodeType)
    End Sub

    Private Sub TreeViewEngine_AfterCollapse(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles TreeViewEngine.AfterCollapse
        e.Node.BackColor = Drawing.Color.White
    End Sub

    Private Sub TreeViewEngine_BeforeExpand(ByVal sender As Object, ByVal e As TreeViewCancelEventArgs) Handles TreeViewEngine.BeforeExpand
        Dim nodeExpanding As DirectoryNode = CType(e.Node, DirectoryNode)

        If e.Node.Parent Is Nothing Then
            ' по самому первому уровню
            For Each itemNode As TreeNode In TreeViewEngine.Nodes
                If itemNode.IsExpanded Then itemNode.Collapse()
            Next
        Else
            For Each itemNode As TreeNode In e.Node.Parent.Nodes
                If itemNode.IsExpanded Then itemNode.Collapse()
            Next
        End If

        If Not nodeExpanding.SubDirectoriesAdded Then
            AddSubDirectories(nodeExpanding)
        End If

        e.Node.EnsureVisible()
        e.Node.BackColor = Drawing.Color.Gold

        isBeforeExpandNeed = False
        If isAfterSelectNeed Then TreeViewEngine_AfterSelect(sender, New TreeViewEventArgs(e.Node, TreeViewAction.Expand))
        isBeforeExpandNeed = True

        ' когда окно сбора КТ загружается сново, не надо делать новый запуск
        TSButtonStartAcquisitionKT.Visible = False

        If CType(e.Node, DirectoryNode).NodeType = StageNodeType.НомерЗапуска5 Then
            TSButtonStartAcquisitionKT.Visible = CBool(firstNumberStartingOnOpen = CType(e.Node, DirectoryNode).KeyId)
        End If
    End Sub
#End Region

    ''' <summary>
    ''' Обновить ListView после выделения в дереве
    ''' </summary>
    ''' <param name="numberPhase"></param>
    ''' <param name="key"></param>
    ''' <param name="cn"></param>
    Private Sub PopulateListViewMembers(ByVal numberPhase As Integer, ByVal key As Integer, ByRef cn As OleDbConnection)
        Dim rdr As OleDbDataReader
        Dim cmd As OleDbCommand = cn.CreateCommand
        cmd.CommandType = CommandType.Text
        Dim strSQL As String
        Dim mItem As ListViewItem

        If numberPhase = StageNodeType.ТипыИзделия1 Then
            Return
        ElseIf numberPhase <= StageNodeType.НомерКТ6 Then
            ' Очистите старые заглавия
            ListViewMemberNode.Items.Clear()

            If ListViewMemberNode.ListViewItemSorter IsNot Nothing Then
                CType(ListViewMemberNode.ListViewItemSorter, ListViewItemComparer).Col = 0
            End If

            ListViewMemberNode.Sorting = SortOrder.Ascending
            LabelTitle.Text = StageConstNames(numberPhase - 1)
            PopulateColumnsOnListViewMemberNode(StageNames(numberPhase - 1))

            ' это годится для заполнения контролов
            ' отбирает значения контролов с фильтром на предыдущий этап
            ' образец
            ' TRANSFORM First(ЗначенияКонтроловДляНомерИзделия.ЗначениеПользователя) AS [First-ЗначениеПользователя]
            ' SELECT [2НомерИзделия].НомерИзделия, First([1ТипИзделия].keyТипИзделия) AS [First-keyТипИзделия]
            ' FROM КонтролыДляНомерИзделия RIGHT JOIN ((1ТипИзделия RIGHT JOIN 2НомерИзделия ON [1ТипИзделия].keyТипИзделия = [2НомерИзделия].keyТипИзделия) RIGHT JOIN ЗначенияКонтроловДляНомерИзделия ON [2НомерИзделия].keyНомерИзделия = ЗначенияКонтроловДляНомерИзделия.keyУровень) ON КонтролыДляНомерИзделия.keyКонтролДляУровня = ЗначенияКонтроловДляНомерИзделия.keyКонтролДляУровня
            ' WHERE ((([1ТипИзделия].keyТипИзделия)=14))
            ' GROUP BY [2НомерИзделия].НомерИзделия
            ' PIVOT КонтролыДляНомерИзделия.Text;

            'strSQL = "TRANSFORM First(ЗначенияКонтроловДля" & ИменаЭтапов(iNumberPhase) & ".ЗначениеПользователя) AS [First-ЗначениеПользователя] " & _
            '"SELECT [" & iNumberPhasePlus.ToString & ИменаЭтапов(iNumberPhase) & "]." & ИменаЭтапов(iNumberPhase) & " " & _
            strSQL = "TRANSFORM First(ЗначенияКонтроловДля" & StageNames(numberPhase - 1) & ".ЗначениеПользователя) AS [First-ЗначениеПользователя] " &
            "SELECT [" & numberPhase.ToString & StageNames(numberPhase - 1) & "]." & StageNames(numberPhase - 1) & ", First([" & (numberPhase).ToString & StageNames(numberPhase - 1) & "].key" & StageNames(numberPhase - 1) & ") AS [First-key" & StageNames(numberPhase - 1) & "] " &
            "FROM КонтролыДля" & StageNames(numberPhase - 1) & " RIGHT JOIN ((" & (numberPhase - 1).ToString & StageNames(numberPhase - 2) & " RIGHT JOIN " & numberPhase.ToString & StageNames(numberPhase - 1) & " ON [" & (numberPhase - 1).ToString & StageNames(numberPhase - 2) & "].key" & StageNames(numberPhase - 2) & " = [" & numberPhase.ToString & StageNames(numberPhase - 1) & "].key" & StageNames(numberPhase - 2) & ") RIGHT JOIN ЗначенияКонтроловДля" & StageNames(numberPhase - 1) & " ON [" & numberPhase.ToString & StageNames(numberPhase - 1) & "].key" & StageNames(numberPhase - 1) & " = ЗначенияКонтроловДля" & StageNames(numberPhase - 1) & ".keyУровень) ON КонтролыДля" & StageNames(numberPhase - 1) & ".keyКонтролДляУровня = ЗначенияКонтроловДля" & StageNames(numberPhase - 1) & ".keyКонтролДляУровня " &
            "WHERE ((([" & (numberPhase - 1).ToString & StageNames(numberPhase - 2) & "].key" & StageNames(numberPhase - 2) & ")=" & key.ToString & ")) " &
            "GROUP BY [" & numberPhase.ToString & StageNames(numberPhase - 1) & "]." & StageNames(numberPhase - 1) &
            " PIVOT КонтролыДля" & StageNames(numberPhase - 1) & ".Text;"

            ' но порядок не по сортировке
            cmd.CommandText = strSQL
            rdr = cmd.ExecuteReader

            Do While rdr.Read()
                mItem = New ListViewItem(CStr(rdr(StageNames(numberPhase - 1))), numberPhase) With {
                    .Tag = CStr(rdr("First-key" & StageNames(numberPhase - 1))) & id1
                }

                ' просмотр по коллекции которая отсортирована
                For Each itemControl As IUserControl In mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 1)).Values
                    For Each itemColumnHeader As ColumnHeader In ListViewMemberNode.Columns
                        If itemColumnHeader.Index <> 0 AndAlso itemControl.IndexLocationOnPanel = itemColumnHeader.Index Then
                            mItem.SubItems.Add(CStr(rdr(itemControl.Text)))
                            Exit For
                        End If
                    Next
                Next

                ListViewMemberNode.Items.Add(mItem)
            Loop

            rdr.Close()
        End If

        ' создать запрос с уровнем и отбором Where на уровень меньше
        ' образец
        ' TRANSFORM First(ЗначенияКонтроловДляТипИзделия.ЗначениеПользователя) AS [First-ЗначениеПользователя]
        ' SELECT [1ТипИзделия].ТипИзделия, First([1ТипИзделия].keyТипИзделия) AS [First-keyТипИзделия]
        ' FROM КонтролыДляТипИзделия RIGHT JOIN (1ТипИзделия RIGHT JOIN ЗначенияКонтроловДляТипИзделия ON [1ТипИзделия].keyТипИзделия=ЗначенияКонтроловДляТипИзделия.keyУровень) ON КонтролыДляТипИзделия.keyКонтролДляУровня=ЗначенияКонтроловДляТипИзделия.keyКонтролДляУровня
        ' WHERE ((([1ТипИзделия].keyТипИзделия)=13))
        ' GROUP BY [1ТипИзделия].ТипИзделия
        ' PIVOT КонтролыДляТипИзделия.Name;

        strSQL = "TRANSFORM First(ЗначенияКонтроловДля" & StageNames(numberPhase - 2) & ".ЗначениеПользователя) AS [First-ЗначениеПользователя] " &
                "SELECT [" & (numberPhase - 1).ToString & StageNames(numberPhase - 2) & "]." & StageNames(numberPhase - 2) & ", First([" & (numberPhase - 1).ToString & StageNames(numberPhase - 2) & "].key" & StageNames(numberPhase - 2) & ") AS [First-key" & StageNames(numberPhase - 2) & "] " &
                "FROM КонтролыДля" & StageNames(numberPhase - 2) & " RIGHT JOIN (" & (numberPhase - 1).ToString & StageNames(numberPhase - 2) & " RIGHT JOIN ЗначенияКонтроловДля" & StageNames(numberPhase - 2) & " ON [" & (numberPhase - 1).ToString & StageNames(numberPhase - 2) & "].key" & StageNames(numberPhase - 2) & " = ЗначенияКонтроловДля" & StageNames(numberPhase - 2) & ".keyУровень) ON КонтролыДля" & StageNames(numberPhase - 2) & ".keyКонтролДляУровня = ЗначенияКонтроловДля" & StageNames(numberPhase - 2) & ".keyКонтролДляУровня " &
                "WHERE ((([" & (numberPhase - 1).ToString & StageNames(numberPhase - 2) & "].key" & StageNames(numberPhase - 2) & ")=" & key.ToString & ")) " &
                "GROUP BY [" & (numberPhase - 1).ToString & StageNames(numberPhase - 2) & "]." & StageNames(numberPhase - 2) &
                " PIVOT КонтролыДля" & StageNames(numberPhase - 2) & ".Name;"

        cmd.CommandText = strSQL
        rdr = cmd.ExecuteReader

        If rdr.Read() Then
            ' просмотр по коллекции которая отсортирована
            For I As Integer = 0 To rdr.FieldCount - 1
                Dim nameOfControl As String = rdr.GetName(I)

                If mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 2)).ContainsKey(nameOfControl) Then
                    Dim itemControl As IUserControl = mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 2)).Item(nameOfControl)
                    If itemControl IsNot Nothing Then
                        itemControl.UserValue = CStr(rdr(nameOfControl)) ' rdr(IControlsOfPfase.Name) 
                        'Debug.Print(NameOfControl)
                    End If
                End If
            Next
        End If

        rdr.Close()

        If numberPhase = StageNodeType.НомерКТ6 Then
            TSMenuItemShowAllChannels.Enabled = ListViewMemberNode.Items.Count > 0
        End If
    End Sub

    ''' <summary>
    ''' Определить Заголовки Столбцов Листа
    ''' </summary>
    ''' <param name="namePhase"></param>
    Private Sub PopulateColumnsOnListViewMemberNode(ByVal namePhase As String)
        Dim countControls As Integer = mFormParrent.Manager.ControlsForPhase.Item(namePhase).Count
        ' Очистка Columns коллекции.
        ListViewMemberNode.Columns.Clear()

        For Each itemControl As IUserControl In mFormParrent.Manager.ControlsForPhase.Item(namePhase).Values
            ListViewMemberNode.Columns.Add(itemControl.Text, ListViewMemberNode.Width \ countControls, HorizontalAlignment.Left)
        Next
    End Sub

    ''' <summary>
    ''' Обновить Контр Точку
    ''' </summary>
    ''' <param name="numberKT"></param>
    ''' <param name="cn"></param>
    Private Sub PopulateReportKT(ByVal numberKT As Integer, ByRef cn As OleDbConnection)
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow

        ' выбрать все по собранным данным для данной КТ и заполнить dtDataTable
        'strSQL = "SELECT [1ТипИзделия].ТипИзделия, [2НомерИзделия].НомерИзделия, [3НомерСборки].НомерСборки, [4НомерПостановки].НомерПостановки, [5НомерЗапуска].НомерЗапуска, [6НомерКТ].НомерКТ, [7ЗначенияПараметровКТ].Значение, СвойстваПараметров.ИмяПараметра " & _
        '"FROM СвойстваПараметров RIGHT JOIN ((((((1ТипИзделия RIGHT JOIN 2НомерИзделия ON [1ТипИзделия].keyТипИзделия = [2НомерИзделия].keyТипИзделия) RIGHT JOIN 3НомерСборки ON [2НомерИзделия].keyНомерИзделия = [3НомерСборки].keyНомерИзделия) RIGHT JOIN 4НомерПостановки ON [3НомерСборки].keyНомерСборки = [4НомерПостановки].keyНомерСборки) RIGHT JOIN 5НомерЗапуска ON [4НомерПостановки].keyНомерПостановки = [5НомерЗапуска].keyНомерПостановки) RIGHT JOIN 6НомерКТ ON [5НомерЗапуска].keyНомерЗапуска = [6НомерКТ].keyНомерЗапуска) RIGHT JOIN 7ЗначенияПараметровКТ ON [6НомерКТ].keyНомерКТ = [7ЗначенияПараметровКТ].keyНомерКТ) ON СвойстваПараметров.keyИмяПараметра = [7ЗначенияПараметровКТ].keyИмяПараметра " & _
        '"WHERE ((([1ТипИзделия].keyТипИзделия)= " & keyNEWТипИзделия & " ) AND (([2НомерИзделия].keyНомерИзделия)= " & keyNEWНомерИзделия & " ) AND (([3НомерСборки].keyНомерСборки)= " & keyNEWНомерСборки & " ) AND (([4НомерПостановки].keyНомерПостановки)= " & keyNEWНомерПостановки & " ) AND (([5НомерЗапуска].keyНомерЗапуска)= " & keyNEWНомерЗапуска & " ) AND (([6НомерКТ].keyНомерКТ)= " & keyNEWНомерКонтрТочки & " ));"

        Dim strSQL As String = "SELECT [1ТипИзделия].ТипИзделия, [2НомерИзделия].НомерИзделия, [3НомерСборки].НомерСборки, [4НомерПостановки].НомерПостановки, [5НомерЗапуска].НомерЗапуска, [6НомерКТ].НомерКТ, [7ЗначенияПараметровКТ].Значение, ИзмеренныеПараметры.ИмяПараметра, РасчетныеПараметры.ИмяПараметра " &
        "FROM РасчетныеПараметры RIGHT JOIN (ИзмеренныеПараметры RIGHT JOIN ((((((1ТипИзделия RIGHT JOIN 2НомерИзделия ON [1ТипИзделия].keyТипИзделия = [2НомерИзделия].keyТипИзделия) RIGHT JOIN 3НомерСборки ON [2НомерИзделия].keyНомерИзделия = [3НомерСборки].keyНомерИзделия) RIGHT JOIN 4НомерПостановки ON [3НомерСборки].keyНомерСборки = [4НомерПостановки].keyНомерСборки) RIGHT JOIN 5НомерЗапуска ON [4НомерПостановки].keyНомерПостановки = [5НомерЗапуска].keyНомерПостановки) RIGHT JOIN 6НомерКТ ON [5НомерЗапуска].keyНомерЗапуска = [6НомерКТ].keyНомерЗапуска) RIGHT JOIN 7ЗначенияПараметровКТ ON [6НомерКТ].keyНомерКТ = [7ЗначенияПараметровКТ].keyНомерКТ) ON ИзмеренныеПараметры.ИмяПараметра = [7ЗначенияПараметровКТ].ИмяПараметра) ON РасчетныеПараметры.ИмяПараметра = [7ЗначенияПараметровКТ].ИмяПараметра " &
        "WHERE ((([1ТипИзделия].keyТипИзделия)= " & keyNewTypeEngine & " ) AND (([2НомерИзделия].keyНомерИзделия)= " & keyNewNumberEngine & " ) AND (([3НомерСборки].keyНомерСборки)= " & keyNewNumberBuild & " ) AND (([4НомерПостановки].keyНомерПостановки)= " & keyNewNumberStage & " ) AND (([5НомерЗапуска].keyНомерЗапуска)= " & keyNewNumberStarting & " ) AND (([6НомерКТ].keyНомерКТ)= " & keyNewNumberKT & " ));"

        odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
        odaDataAdapter.Fill(dtDataTable)

        If dtDataTable.Rows.Count > 0 Then ' значения 7ЗначенияПараметровКТ.Значение есть данные
            drDataRow = dtDataTable.Rows(0)
            ' имя колонки совпадает с именем этапа и есть объект в коллекции
            ' конкретно для каждого этапа выбрать записанные значения контролов
            For I As Integer = 0 To StageNames.Count - 1
                If mFormParrent.Manager.ControlsForPhase.Item(StageNames(I)).Count > 0 Then
                    Dim rdr As OleDbDataReader
                    Dim cmd As OleDbCommand = cn.CreateCommand
                    cmd.CommandType = CommandType.Text
                    Dim numberPhase As Integer = I + 1
                    Dim key As Integer

                    Select Case numberPhase
                        Case StageNodeType.ТипыИзделия1 ' 1 Тип Изделия
                            key = keyNewTypeEngine
                            Exit Select
                        Case StageNodeType.НомерИзделия2 ' 2 Номер Изделия
                            key = keyNewNumberEngine
                            Exit Select
                        Case StageNodeType.НомерСборки3 ' 3 Номер Сборки
                            key = keyNewNumberBuild
                            Exit Select
                        Case StageNodeType.НомерПостановки4 ' 4 Номер Постановки
                            key = keyNewNumberStage
                            Exit Select
                        Case StageNodeType.НомерЗапуска5 ' 5 Номер Запуска
                            key = keyNewNumberStarting
                            Exit Select
                        Case StageNodeType.НомерКТ6 ' 6 Номер КТ
                            key = numberKT
                            Exit Select
                    End Select

                    ' образец запроса
                    ' strSQL = "SELECT ЗначенияКонтроловДляТипИзделия.ЗначениеПользователя, КонтролыДляТипИзделия.Name " & _
                    '         "FROM КонтролыДляТипИзделия RIGHT JOIN (1ТипИзделия RIGHT JOIN ЗначенияКонтроловДляТипИзделия ON [1ТипИзделия].keyТипИзделия = ЗначенияКонтроловДляТипИзделия.keyУровень) ON КонтролыДляТипИзделия.keyКонтролДляУровня = ЗначенияКонтроловДляТипИзделия.keyКонтролДляУровня " & _
                    '         "WHERE ((([1ТипИзделия].keyТипИзделия)=13));"
                    ' МестоНаПанели
                    strSQL = "SELECT ЗначенияКонтроловДля" & StageNames(I) & ".ЗначениеПользователя, КонтролыДля" & StageNames(I) & ".Name , КонтролыДля" & StageNames(I) & ".МестоНаПанели " &
                            "FROM КонтролыДля" & StageNames(I) & " RIGHT JOIN (" & (numberPhase).ToString & StageNames(I) & " RIGHT JOIN ЗначенияКонтроловДля" & StageNames(I) & " ON [" & (numberPhase).ToString & StageNames(I) & "].key" & StageNames(I) & " = ЗначенияКонтроловДля" & StageNames(I) & ".keyУровень) ON КонтролыДля" & StageNames(I) & ".keyКонтролДляУровня = ЗначенияКонтроловДля" & StageNames(I) & ".keyКонтролДляУровня " &
                            "WHERE ((([" & (numberPhase).ToString & StageNames(I) & "].key" & StageNames(I) & ")=" & key.ToString & ")) " &
                            "ORDER BY КонтролыДля" & StageNames(I) & ".МестоНаПанели;"

                    cmd.CommandText = strSQL
                    rdr = cmd.ExecuteReader

                    Do While rdr.Read()
                        ' имя контрола этапа в поле rdr("Name")
                        ' получить контрол
                        If mFormParrent.Manager.ControlsForPhase.Item(StageNames(I)).ContainsKey(CStr(rdr("Name"))) Then
                            Dim IControlsOfPfase As IUserControl = mFormParrent.Manager.ControlsForPhase.Item(StageNames(I)).Item(CStr(rdr("Name")))
                            If IControlsOfPfase IsNot Nothing Then mDataGridExcelBook.SetTextRC(IControlsOfPfase.Row, IControlsOfPfase.Col, CStr(rdr("ЗначениеПользователя")))
                        End If
                    Loop

                    rdr.Close()
                End If
            Next

            ' по строкам таблицы только для подсчитанных и измеренных параметров
            Dim success As Boolean ' параметр Найден

            For Each drDataRow In dtDataTable.Rows
                success = False

                If Not IsDBNull(drDataRow("ИзмеренныеПараметры.ИмяПараметра")) Then
                    For Each row As BaseFormDataSet.ИзмеренныеПараметрыRow In mFormParrent.Manager.MeasurementDataTable.Rows
                        If row.ИмяПараметра = CStr(drDataRow("ИзмеренныеПараметры.ИмяПараметра")) Then
                            mDataGridExcelBook.SetDoubleRC(row.Row, row.Col, CStr(drDataRow("Значение")))
                            success = True
                            Exit For
                        End If
                    Next
                End If

                If success Then Continue For

                If Not IsDBNull(drDataRow("РасчетныеПараметры.ИмяПараметра")) Then
                    For Each row As BaseFormDataSet.РасчетныеПараметрыRow In mFormParrent.Manager.CalculatedDataTable.Rows
                        If row.ИмяПараметра = CStr(drDataRow("РасчетныеПараметры.ИмяПараметра")) Then
                            mDataGridExcelBook.SetDoubleRC(row.Row, row.Col, CStr(drDataRow("Значение")))
                            Exit For
                        End If
                    Next
                End If
            Next
        Else
            MessageBox.Show("Не найдено собранных значений параметров для выделенной КТ",
                            "Обновление контрольной точки", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    ''' <summary>
    ''' cmdNewClickEvent
    ''' </summary>
    ''' <param name="numberPhase"></param>
    Private Sub ClearAndUnlockPhase(ByVal numberPhase As Integer)
        If numberPhase > 1 Then LockControls(numberPhase - 1) ' заблокировать все предыдущие

        Select Case numberPhase
            Case StageNodeType.ТипыИзделия1 ' 1 Тип Изделия
                TSButtonNewTypeEngine.Enabled = False
                TSButtonOKTypeEngine.Visible = True
                TSButtonCancelTypeEngine.Visible = True
                ClearControlsTypeEngine()
                UnlockPhaseTabPage(numberPhase)
                ShowMessageOnStatusPanel("Ввод нового типа изделия")
                Exit Select
            Case StageNodeType.НомерИзделия2 ' 2 Номер Изделия
                TSButtonNewNumberEngine.Enabled = False
                TSButtonOKNumberEngine.Visible = True
                TSButtonCancelNumberEngine.Visible = True
                ClearControlsNumberEngine()
                UnlockPhaseTabPage(numberPhase)
                ShowMessageOnStatusPanel("Ввод нового номера изделия")
                Exit Select
            Case StageNodeType.НомерСборки3 ' 3 Номер Сборки
                ' для текущего номера изделия делается запрос для определения numberBuild
                ' равного последнему номеру в последней записи набора
                ' к нему прибавляется счетчик
                Dim strSQL As String = $"SELECT TOP 1 * FROM [3НомерСборки] Where keyНомерИзделия = {idNumberEngine} ORDER BY [3НомерСборки].НомерСборки DESC;"
                Dim oldNumberBuild As Integer

                TSButtonNewNumberBuild.Enabled = False
                TSButtonOkNumberBuild.Visible = True
                TSButtonCancelNumberBuild.Visible = True

                Using cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
                    Dim cmd As OleDbCommand = cn.CreateCommand
                    Dim rdr As OleDbDataReader

                    cmd.CommandType = CommandType.Text
                    cmd.CommandText = strSQL
                    cn.Open()
                    rdr = cmd.ExecuteReader

                    If rdr.Read() Then oldNumberBuild = CInt(rdr("НомерСборки"))

                    rdr.Close()
                End Using

                ClearControlsNumberBuild()
                mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 1)).Item(StageNames(numberPhase - 1)).UserValue = CStr(oldNumberBuild + 1)
                mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase)).Item(StageNames(numberPhase)).UserValue = CStr(0)
                UnlockPhaseTabPage(numberPhase)
                ShowMessageOnStatusPanel("Ввод нового номера сборки")
                Exit Select
            Case StageNodeType.НомерПостановки4 ' 4 Номер Постановки
                Dim strSQL As String = $"SELECT TOP 1 * FROM [4НомерПостановки] Where keyНомерСборки = {idNumberBuild} ORDER BY [4НомерПостановки].НомерПостановки DESC;"
                Dim oldNumberStage As Integer

                TSButtonNewNumberStage.Enabled = False
                TSButtonOKNumberStage.Visible = True
                TSButtonCancelNumberStage.Visible = True

                Using cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
                    Dim cmd As OleDbCommand = cn.CreateCommand
                    Dim rdr As OleDbDataReader

                    cmd.CommandType = CommandType.Text
                    cmd.CommandText = strSQL
                    cn.Open()
                    rdr = cmd.ExecuteReader

                    If rdr.Read() Then oldNumberStage = CInt(rdr("НомерПостановки"))

                    rdr.Close()
                End Using

                ClearControlsNumberStage()
                ' Номер Постановки
                mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 1)).Item(StageNames(numberPhase - 1)).UserValue = CStr(oldNumberStage + 1)
                ' Номер Запуска
                mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase)).Item(StageNames(numberPhase)).UserValue = CStr(0)
                ' Номер КТ
                mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase + 1)).Item(StageNames(numberPhase + 1)).UserValue = CStr(0)
                UnlockPhaseTabPage(numberPhase)
                ShowMessageOnStatusPanel("Ввод нового номера постановки")
                Exit Select
            Case StageNodeType.НомерЗапуска5 ' 5 Номер Запуска
                Dim strSQL As String = $"SELECT TOP 1 * FROM [5НомерЗапуска] Where keyНомерПостановки = {idNumberStage} ORDER BY [5НомерЗапуска].НомерЗапуска DESC;"
                Dim oldNumberStarting As Integer

                TSButtonNewNumberStarting.Enabled = False
                TSButtonOKNumberStarting.Visible = True
                TSButtonCancelNumberStarting.Visible = True

                Using cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
                    Dim cmd As OleDbCommand = cn.CreateCommand
                    Dim rdr As OleDbDataReader

                    cmd.CommandType = CommandType.Text
                    cmd.CommandText = strSQL
                    cn.Open()
                    rdr = cmd.ExecuteReader

                    If rdr.Read() = True Then
                        oldNumberStarting = CInt(rdr("НомерЗапуска"))
                    End If

                    rdr.Close()
                End Using

                ClearControlsNumberStarting()
                ClearControlsNumberKT()
                ' Номер Запуска
                mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 1)).Item(StageNames(numberPhase - 1)).UserValue = CStr(oldNumberStarting + 1)
                UnlockPhaseTabPage(numberPhase)
                ShowMessageOnStatusPanel("Ввод нового номера запуска")
                mFormParrent.Manager.ClearAcquisitionValue()
                PopulateReportKT()
                Exit Select
        End Select
    End Sub

    ''' <summary>
    ''' Найти Родителя
    ''' </summary>
    ''' <param name="arrKeyID"></param>
    ''' <param name="ParrentStageNodeType"></param>
    ''' <returns></returns>
    Private Function GetParrentDirectoryNode(ByVal arrKeyID() As Integer, ByVal ParrentStageNodeType As StageNodeType) As DirectoryNode
        If ParrentStageNodeType = StageNodeType.ТипыИзделия1 Then
            For Each itemNode As DirectoryNode In TreeViewEngine.Nodes
                If itemNode.KeyId = arrKeyID(ParrentStageNodeType) Then
                    itemNode.SubDirectoriesAdded = True
                    itemNode.ImageIndex = StageNodeType.ТипыИзделия1
                    Return itemNode
                End If
            Next
        Else
            For Each itemNode As DirectoryNode In GetParrentDirectoryNode(arrKeyID, CType(ParrentStageNodeType - 1, StageNodeType)).Nodes
                If itemNode.KeyId = arrKeyID(ParrentStageNodeType) Then
                    itemNode.SubDirectoriesAdded = True
                    itemNode.ImageIndex = ParrentStageNodeType
                    Return itemNode
                End If
            Next
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' Обработчик события добавления строки.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    Private Sub OnRowUpdated(ByVal sender As Object, ByVal args As OleDbRowUpdatedEventArgs) 'Shared
        ' команда получения нового идентификатора для новой добавленной строки
        'Dim newID As Integer = 0
        Dim idCMD As OleDbCommand = New OleDbCommand("SELECT @@IDENTITY", connection)

        If args.StatementType = StatementType.Insert Then
            ' записать этот полученный идентификатор в только что добавленную строку
            newID = CInt(idCMD.ExecuteScalar())
            args.Row(keyTablePhase) = newID
        End If
    End Sub

    ''' <summary>
    ''' cmdOKClickEvent
    ''' </summary>
    ''' <param name="numberPhase"></param>
    Private Sub AddNewPhase(ByVal numberPhase As Integer)
        ' кнопка OK только когда новый
        If numberPhase > StageNodeType.ТипыИзделия1 AndAlso Not IsContainsParrentNode(numberPhase) Then Exit Sub
        If IsVerificationFieldsCompleted(numberPhase) Then Exit Sub

        Dim controlValue As String = mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 1)).Item(StageNames(numberPhase - 1)).UserValue
        Dim response As DialogResult

        If IsFieldEquality(numberPhase, controlValue, response) Then
            If response = Windows.Forms.DialogResult.No Then LockPhaseOnCancel(numberPhase)
            Exit Sub
        End If

        ' все нормально
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow
        Dim cb As OleDbCommandBuilder
        Dim mNode As DirectoryNode

        keyTablePhase = cKey & StageNames(numberPhase - 1)
        UnlockControls(numberPhase)
        Select Case numberPhase
            'Case StageNodeType.ТипыИзделия1 ' 1 Тип Изделия
            Case StageNodeType.НомерИзделия2 ' 2 Номер Изделия
                If keyNewTypeEngine = 0 Then
                    ShowMessageEmptyPhase("Тип Изделия")
                    LockPhaseOnCancel(numberPhase)
                    Exit Sub
                End If
                Exit Select
            Case StageNodeType.НомерСборки3 ' 3 Номер Сборки
                If keyNewNumberEngine = 0 Then
                    ShowMessageEmptyPhase("Номер Изделия")
                    LockPhaseOnCancel(numberPhase)
                    Exit Sub
                End If
                Exit Select
            Case StageNodeType.НомерПостановки4 ' 4 Номер Постановки
                If keyNewNumberBuild = 0 Then
                    ShowMessageEmptyPhase("Номер Сборки")
                    LockPhaseOnCancel(numberPhase)
                    Exit Sub
                End If
                Exit Select
            Case StageNodeType.НомерЗапуска5 ' 5 Номер Запуска
                If keyNewNumberStage = 0 Then
                    ShowMessageEmptyPhase("Номер Постановки")
                    LockPhaseOnCancel(numberPhase)
                    Exit Sub
                End If
                Exit Select
            Case StageNodeType.НомерКТ6 ' 5 Номер КТ
                If keyNewNumberStarting = 0 Then
                    ShowMessageEmptyPhase("Номер Запуска")
                    TSButtonCancelSaveKT_Click(TSButtonCancelSaveKT, New EventArgs)
                    Exit Sub
                End If
                Exit Select
        End Select

        Using cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
            ' общая для всех этапов
            Dim strSQL As String = $"SELECT * FROM [{(numberPhase).ToString}{StageNames(numberPhase - 1)}] WHERE {keyTablePhase}=1"

            cn.Open()
            connection = cn ' для события вставки новой строки
            odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
            odaDataAdapter.Fill(dtDataTable)
            drDataRow = dtDataTable.NewRow
            drDataRow.BeginEdit()
            drDataRow(StageNames(numberPhase - 1)) = controlValue

            Select Case numberPhase
            'Case StageNodeType.ТипыИзделия1 ' 1 Тип Изделия
                Case StageNodeType.НомерИзделия2 ' 2 Номер Изделия
                    drDataRow(cKey & StageNames(numberPhase - 2)) = keyNewTypeEngine
                    Exit Select
                Case StageNodeType.НомерСборки3 ' 3 Номер Сборки
                    drDataRow(cKey & StageNames(numberPhase - 2)) = keyNewNumberEngine
                    Exit Select
                Case StageNodeType.НомерПостановки4 ' 4 Номер Постановки
                    drDataRow(cKey & StageNames(numberPhase - 2)) = keyNewNumberBuild
                    Exit Select
                Case StageNodeType.НомерЗапуска5 ' 5 Номер Запуска
                    drDataRow(cKey & StageNames(numberPhase - 2)) = keyNewNumberStage
                    Exit Select
                Case StageNodeType.НомерКТ6 ' 6 Номер КТ
                    drDataRow(cKey & StageNames(numberPhase - 2)) = keyNewNumberStarting
                    Exit Select
            End Select
            drDataRow.EndEdit()

            dtDataTable.Rows.Add(drDataRow)
            ' Включить событие обновления значения автонумерации
            AddHandler odaDataAdapter.RowUpdated, New OleDbRowUpdatedEventHandler(AddressOf OnRowUpdated)

            cb = New OleDbCommandBuilder(odaDataAdapter)
            odaDataAdapter.Update(dtDataTable)
            ' теперь заполнить таблицу значений этапа по контролам
            strSQL = $"SELECT * FROM(ЗначенияКонтроловДля{StageNames(numberPhase - 1)}) WHERE (((keyЗначение)=1));"

            odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
            odaDataAdapter.Fill(dtDataTable)

            ' приблуда не нужная для наследования, а только для конкретной реализации
            If numberPhase = StageNodeType.НомерКТ6 Then
                If mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 1)).Keys.Contains("Режим") Then
                    mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 1)).Item("Режим").UserValue = mFormParrent.Manager.Regime

                    If mFormParrent.Manager.Regime = "М" Then
                        ' запомнить самый последний номер КТ с режимом "М"
                        lastNumberKTmaximum = Integer.Parse(mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 1)).Item("НомерКТ").UserValue)
                    ElseIf mFormParrent.Manager.Regime = "Ф" Then
                        If mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 1)).Keys.Contains("NктМаксимала") Then
                            mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 1)).Item("NктМаксимала").UserValue = lastNumberKTmaximum.ToString
                        End If
                    End If
                End If
            End If

            For Each itemControl As IUserControl In mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 1)).Values
                drDataRow = dtDataTable.NewRow
                drDataRow.BeginEdit()
                drDataRow("keyУровень") = newID
                drDataRow("keyКонтролДляУровня") = itemControl.keyControlStage
                drDataRow("ЗначениеПользователя") = itemControl.UserValue
                drDataRow.EndEdit()
                dtDataTable.Rows.Add(drDataRow)
            Next

            cb = New OleDbCommandBuilder(odaDataAdapter)
            odaDataAdapter.Update(dtDataTable)
            connection = Nothing ' очистить
        End Using

        isAfterSelectNeed = False ' здесь при пустых типах при вводе первого типа вызывается AfterSelect что приводит к ошибке
        Select Case numberPhase
            Case StageNodeType.ТипыИзделия1 ' 1 Тип Изделия
                ' Запись и получение keyNEWТипИзделия=keyТипИзделия
                ' дополнение дерева
                ' перерисовка дерева и раскрытие узла
                ' новый ТипИзделия
                LockPhaseTabPage(numberPhase)
                idTypeEngine = newID
                keyNewTypeEngine = idTypeEngine
                mNode = New DirectoryNode("Тип изделия-" & controlValue, CType(numberPhase, StageNodeType), idTypeEngine) With {
                        .SelectedImageIndex = 0,
                        .ImageIndex = StageNodeType.НетПотомков,
                        .Tag = CStr(idTypeEngine) & id1
                    }
                TreeViewEngine.Nodes.Add(mNode)
                mNode.Expand()
                mNode.EnsureVisible()
                TreeViewEngine.SelectedNode = mNode
                Exit Select
            Case StageNodeType.НомерИзделия2 ' 2 Номер Изделия
                ' Запись и получение keyNEWНомерИзделия=keyНомерИзделия
                LockPhaseTabPage(numberPhase)
                idNumberEngine = newID
                keyNewNumberEngine = idNumberEngine
                mNode = New DirectoryNode("№ изделия-" & controlValue, CType(numberPhase, StageNodeType), idNumberEngine) With {
                        .SelectedImageIndex = 0,
                        .ImageIndex = StageNodeType.НетПотомков,
                        .Tag = CStr(idNumberEngine) & id2
                    }

                Dim arrKeyID As Integer() = {0, keyNewTypeEngine, keyNewNumberEngine, keyNewNumberBuild, keyNewNumberStage, keyNewNumberStarting, keyNewNumberKT}
                GetParrentDirectoryNode(arrKeyID, CType(numberPhase - 1, StageNodeType)).Nodes.Add(mNode)
                mNode.Parent.Expand()
                mNode.EnsureVisible()
                TreeViewEngine.SelectedNode = mNode
                Exit Select
            Case StageNodeType.НомерСборки3 ' 3 Номер Сборки
                ' Запись и получение keyNEWНомерСборки=keyНомерСборки
                LockPhaseTabPage(numberPhase)
                idNumberBuild = newID
                keyNewNumberBuild = idNumberBuild
                mNode = New DirectoryNode("№ сборки-" & controlValue, CType(numberPhase, StageNodeType), idNumberBuild) With {
                    .SelectedImageIndex = 0,
                    .ImageIndex = StageNodeType.НетПотомков,
                    .Tag = CStr(idNumberBuild) & id3
                }

                Dim arrKeyID As Integer() = {0, keyNewTypeEngine, keyNewNumberEngine, keyNewNumberBuild, keyNewNumberStage, keyNewNumberStarting, keyNewNumberKT}
                GetParrentDirectoryNode(arrKeyID, CType(numberPhase - 1, StageNodeType)).Nodes.Add(mNode)
                mNode.Parent.Expand()
                mNode.EnsureVisible()
                TreeViewEngine.SelectedNode = mNode
                Exit Select
            Case StageNodeType.НомерПостановки4 ' 4 Номер Постановки
                ' Запись и получение keyNEWНомерПостановки=keyНомерПостановки
                LockPhaseTabPage(numberPhase)
                idNumberStage = newID
                keyNewNumberStage = idNumberStage
                mNode = New DirectoryNode("№ постановки-" & controlValue, CType(numberPhase, StageNodeType), idNumberStage) With {
                    .SelectedImageIndex = 0,
                    .ImageIndex = StageNodeType.НетПотомков,
                    .Tag = CStr(idNumberStage) & id4
                }

                Dim arrKeyID As Integer() = {0, keyNewTypeEngine, keyNewNumberEngine, keyNewNumberBuild, keyNewNumberStage, keyNewNumberStarting, keyNewNumberKT}
                GetParrentDirectoryNode(arrKeyID, CType(numberPhase - 1, StageNodeType)).Nodes.Add(mNode)
                mNode.Parent.Expand()
                mNode.EnsureVisible()
                TreeViewEngine.SelectedNode = mNode
                Exit Select
            Case StageNodeType.НомерЗапуска5 ' 5 Номер Запуска
                ' Запись и получение keyNEWНомерЗапуска=keyНомерЗапуска
                LockPhaseTabPage(numberPhase)
                idNumberStarting = newID
                keyNewNumberStarting = idNumberStarting
                firstNumberStartingOnOpen = idNumberStarting
                mNode = New DirectoryNode("№ запуска-" & controlValue, CType(numberPhase, StageNodeType), idNumberStarting) With {
                    .SelectedImageIndex = 0,
                    .ImageIndex = StageNodeType.НетПотомков,
                    .Tag = CStr(idNumberStarting) & id5
                }

                Dim arrKeyID As Integer() = {0, keyNewTypeEngine, keyNewNumberEngine, keyNewNumberBuild, keyNewNumberStage, keyNewNumberStarting, keyNewNumberKT}
                GetParrentDirectoryNode(arrKeyID, CType(numberPhase - 1, StageNodeType)).Nodes.Add(mNode)
                mNode.Parent.Expand()
                mNode.EnsureVisible()
                ShowMessageOnStatusPanel("Готов")
                TSButtonStartAcquisitionKT.Visible = True
                TreeViewEngine.SelectedNode = mNode
                TSButtonRecalculationKT.Visible = False
                Exit Select
            Case StageNodeType.НомерКТ6 ' 6 Номер КТ
                ' вызывается из кнопки Записать
                ' Запись и получение keyNEWНомерЗапуска=keyНомерЗапуска
                idNumberKT = newID
                keyNewNumberKT = idNumberKT
                mNode = New DirectoryNode("№ контр. точки-" & controlValue, CType(numberPhase, StageNodeType), idNumberKT) With {
                    .SelectedImageIndex = 0,
                    .ImageIndex = numberPhase,
                    .Tag = CStr(idNumberKT) & id6
                }

                Dim arrKeyID As Integer() = {0, keyNewTypeEngine, keyNewNumberEngine, keyNewNumberBuild, keyNewNumberStage, keyNewNumberStarting, keyNewNumberKT}
                GetParrentDirectoryNode(arrKeyID, CType(numberPhase - 1, StageNodeType)).Nodes.Add(mNode)
                mNode.Parent.Expand()
                mNode.EnsureVisible()
                TreeViewEngine.SelectedNode = mNode
                Exit Select
        End Select

        isAfterSelectNeed = True

        If numberPhase <> StageNodeType.НомерКТ6 Then
            ' перйти на следующую вкладку
            TabControlPhases.SelectedIndex = numberPhase
            TabControlPhases.TabPages(numberPhase).Select()
        End If

        Select Case numberPhase
            Case StageNodeType.ТипыИзделия1
                TSButtonNewTypeEngine.Checked = False
                TSButtonNewTypeEngine.Enabled = True
                TSButtonOKTypeEngine.Visible = False
                TSButtonCancelTypeEngine.Visible = False
                TSButtonNewNumberEngine.Checked = True
                TSButtonNewNumberEngine.Enabled = False
                ClearAndUnlockPhase(numberPhase + 1) ' следующий уровень
                Exit Select
            Case StageNodeType.НомерИзделия2
                TSButtonNewNumberEngine.Checked = False
                TSButtonNewNumberEngine.Enabled = True
                TSButtonOKNumberEngine.Visible = False
                TSButtonCancelNumberEngine.Visible = False
                TSButtonNewNumberBuild.Checked = True
                TSButtonNewNumberBuild.Enabled = False
                ClearAndUnlockPhase(numberPhase + 1) ' следующий уровень
                Exit Select
            Case StageNodeType.НомерСборки3
                TSButtonNewNumberBuild.Checked = False
                TSButtonNewNumberBuild.Enabled = True
                TSButtonOkNumberBuild.Visible = False
                TSButtonCancelNumberBuild.Visible = False
                TSButtonNewNumberStage.Checked = True
                TSButtonNewNumberStage.Enabled = False
                ClearAndUnlockPhase(numberPhase + 1) ' следующий уровень
                Exit Select
            Case StageNodeType.НомерПостановки4
                TSButtonNewNumberStage.Checked = False
                TSButtonNewNumberStage.Enabled = True
                TSButtonOKNumberStage.Visible = False
                TSButtonCancelNumberStage.Visible = False
                TSButtonNewNumberStarting.Checked = True
                TSButtonNewNumberStarting.Enabled = False
                ClearAndUnlockPhase(numberPhase + 1) ' следующий уровень
                Exit Select
            Case StageNodeType.НомерЗапуска5
                TSButtonNewNumberStarting.Checked = False
                TSButtonNewNumberStarting.Enabled = True
                TSButtonOKNumberStarting.Visible = False
                TSButtonCancelNumberStarting.Visible = False
                mFormParrent.Manager.IsCalculateEnable = True
                mFormParrent.varFormGraf.TextBoxCollect.Visible = Not mFormParrent.Manager.IsSwohSnapshot
                Exit Select
        End Select
    End Sub

    ''' <summary>
    ''' Сообщение о непройденном этапе родительского уровня
    ''' </summary>
    ''' <param name="field"></param>
    Private Sub ShowMessageEmptyPhase(ByVal field As String)
        MessageBox.Show($"Есть неэаполненное поле <{field}>!{vbCrLf}Необходимо заполнить верхний уровень.",
                        "Ввод новых данных", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

#Region "Блокировка"
    ''' <summary>
    ''' Блокировка Контролов Этапа
    ''' </summary>
    ''' <param name="numberPhase"></param>
    Private Sub LockPhaseTabPage(ByVal numberPhase As Integer)
        TabControlPhases.TabPages("TabPage" & (numberPhase).ToString).Controls("FlowLayoutPanel" & (numberPhase).ToString).Enabled = False
    End Sub

    ''' <summary>
    ''' Разблокировка Контролов Этапа
    ''' </summary>
    ''' <param name="numberPhase"></param>
    Private Sub UnlockPhaseTabPage(ByVal numberPhase As Integer)
        TabControlPhases.TabPages("TabPage" & (numberPhase).ToString).Controls("FlowLayoutPanel" & (numberPhase).ToString).Enabled = True
    End Sub

#End Region

    ''' <summary>
    ''' cmdCancelClickEvent
    ''' </summary>
    ''' <param name="numberPhase"></param>
    Private Sub LockPhaseOnCancel(ByVal numberPhase As Integer)
        UnlockControls(numberPhase)

        ' Номер Сборки, Номер Постановки, Номер Запуска
        Dim IControlsOfPfase As IUserControl = mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 1)).Item(StageNames(numberPhase - 1))

        Select Case numberPhase
            Case StageNodeType.ТипыИзделия1 ' 1 Тип Изделия
                LockPhaseTabPage(numberPhase)
                TSButtonNewTypeEngine.Checked = False
                TSButtonNewTypeEngine.Enabled = True
                TSButtonOKTypeEngine.Visible = False
                TSButtonCancelTypeEngine.Visible = False
                Exit Select
            Case StageNodeType.НомерИзделия2 ' 2 Номер Изделия
                LockPhaseTabPage(numberPhase)
                TSButtonNewNumberEngine.Checked = False
                TSButtonNewNumberEngine.Enabled = True
                TSButtonOKNumberEngine.Visible = False
                TSButtonCancelNumberEngine.Visible = False
                Exit Select
            Case StageNodeType.НомерСборки3 ' 3 Номер Сборки
                LockPhaseTabPage(numberPhase)
                ' возврат предыдущих данных на закладке
                IControlsOfPfase.UserValue = CStr(CInt(IControlsOfPfase.UserValue) - 1)
                TSButtonNewNumberBuild.Checked = False
                TSButtonNewNumberBuild.Enabled = True
                TSButtonOkNumberBuild.Visible = False
                TSButtonCancelNumberBuild.Visible = False
                Exit Select
            Case StageNodeType.НомерПостановки4 ' 4 Номер Постановки
                LockPhaseTabPage(numberPhase)
                ' возврат предыдущих данных на закладке
                IControlsOfPfase.UserValue = CStr(CInt(IControlsOfPfase.UserValue) - 1)
                TSButtonNewNumberStage.Checked = False
                TSButtonNewNumberStage.Enabled = True
                TSButtonOKNumberStage.Visible = False
                TSButtonCancelNumberStage.Visible = False
                Exit Select
            Case StageNodeType.НомерЗапуска5 ' 5 Номер Запуска
                LockPhaseTabPage(numberPhase)
                ' возврат предыдущих данных на закладке
                IControlsOfPfase.UserValue = CStr(CInt(IControlsOfPfase.UserValue) - 1)
                TSButtonNewNumberStarting.Checked = False
                TSButtonNewNumberStarting.Enabled = True
                TSButtonOKNumberStarting.Visible = False
                TSButtonCancelNumberStarting.Visible = False
                Exit Select
        End Select

        ShowMessageOnStatusPanel("Готов")
    End Sub

    ''' <summary>
    ''' Разблокировать Контролы
    ''' </summary>
    ''' <param name="numberPhase"></param>
    Private Sub UnlockControls(ByVal numberPhase As Integer)
        If Not isBlocking Then
            TreeViewEngine.Enabled = True
            LockTabs(True, numberPhase)
        End If

        ListViewMemberNode.Enabled = True
        DataGridViewReportKT.Enabled = True
        TSMenuItemFile.Enabled = True
    End Sub

    ''' <summary>
    ''' Заблокировать Контролы
    ''' </summary>
    ''' <param name="numberPhase"></param>
    Private Sub LockControls(ByVal numberPhase As Integer)
        TreeViewEngine.Enabled = False
        ListViewMemberNode.Enabled = False
        LockTabs(False, numberPhase)
        DataGridViewReportKT.Enabled = False
        TSMenuItemFile.Enabled = False
    End Sub

    ''' <summary>
    ''' Блокировка вкладок
    ''' </summary>
    ''' <param name="isEnabled"></param>
    ''' <param name="numberPhase"></param>
    Private Sub LockTabs(ByVal isEnabled As Boolean, ByVal numberPhase As Integer)
        For I As Integer = 0 To numberPhase - 1
            TabControlPhases.TabPages(I).Enabled = isEnabled
        Next
    End Sub

    ''' <summary>
    ''' Проверка Заполнения Полей Контролов Этапа
    ''' </summary>
    ''' <param name="nameControl"></param>
    ''' <returns></returns>
    Private Function CheckingCompletedPhaseControls(ByVal nameControl As String) As Boolean
        For Each itemControl As IUserControl In mFormParrent.Manager.ControlsForPhase.Item(nameControl).Values
            If Not itemControl.ValidatedUserValue Then
                MessageBox.Show($"В поле {itemControl.Text} ни чего не заведено!{vbCrLf}Необходимо заполнить все поля.",
                                "Ввод новых данных", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return True
                Exit Function
            End If
        Next

        Return False
    End Function

    ''' <summary>
    ''' Проверка Заполнения Полей
    ''' </summary>
    ''' <param name="numberPhase"></param>
    ''' <returns></returns>
    Private Function IsVerificationFieldsCompleted(numberPhase As Integer) As Boolean
        Return CheckingCompletedPhaseControls(StageNames(numberPhase - 1))
    End Function

    ''' <summary>
    ''' Проверка На Совпадение Полей
    ''' </summary>
    ''' <param name="numberPhase"></param>
    ''' <param name="controlValue"></param>
    ''' <param name="outResponse"></param>
    ''' <returns></returns>
    Private Function IsFieldEquality(ByVal numberPhase As Integer, controlValue As String, ByRef outResponse As DialogResult) As Boolean
        Dim success As Boolean 'есть Совпадение

        Using cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
            Dim cmd As OleDbCommand = cn.CreateCommand

            cmd.CommandType = CommandType.Text
            cn.Open()

            Select Case numberPhase
                Case StageNodeType.ТипыИзделия1 ' 1 Тип Изделия
                    cmd.CommandText = $"SELECT COUNT(*) FROM [1ТипИзделия] WHERE [1ТипИзделия].ТипИзделия ='{controlValue}'"

                    If CInt(cmd.ExecuteScalar) <> 0 Then
                        success = True
                        outResponse = MessageBox.Show($"Тип изделия {controlValue} уже существует!{vbCrLf}Необходимо ввести уникальный тип изделия.{vbCrLf}Продолжить?",
                                                   "Совпадение данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                    End If
                    Exit Select
                Case StageNodeType.НомерИзделия2 ' 2 Номер Изделия
                    cmd.CommandText = $"SELECT COUNT(*) FROM [2НомерИзделия] WHERE [2НомерИзделия].НомерИзделия ='{controlValue}'"

                    If CInt(cmd.ExecuteScalar) <> 0 Then
                        success = True
                        outResponse = MessageBox.Show($"Номер изделия {controlValue} уже существует!{vbCrLf}Необходимо ввести уникальный номер изделия.{vbCrLf}Продолжить?",
                                                   "Совпадение данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                    End If
                    Exit Select
            End Select
        End Using

        Return success
    End Function

    ''' <summary>
    ''' Проверка Наналичие Родительского Узла
    ''' </summary>
    ''' <param name="numberPhase"></param>
    ''' <returns></returns>
    Private Function IsContainsParrentNode(ByVal numberPhase As Integer) As Boolean
        Dim message As String
        Dim success As Boolean

        If numberPhase > StageNodeType.ТипыИзделия1 Then ' на 1 уровне(корня) проверки не надо
            If TreeViewEngine.SelectedNode IsNot Nothing Then
                If TreeViewEngine.SelectedNode.Level = numberPhase - 2 Then ' родительский уровень достигнут (здесь проверка при добавлении новых этапов)
                    success = True
                ElseIf numberPhase = StageNodeType.НомерЗапуска5 OrElse numberPhase = StageNodeType.НомерКТ6 Then ' здесь проверка при Записать КТ После Пересчета
                    Dim SelectedDirectoryNode As DirectoryNode = CType(TreeViewEngine.SelectedNode, DirectoryNode)

                    If SelectedDirectoryNode IsNot Nothing Then
                        If SelectedDirectoryNode.NodeType = StageNodeType.НомерЗапуска5 OrElse SelectedDirectoryNode.NodeType = StageNodeType.НомерКТ6 Then
                            success = True
                        Else
                            success = False
                        End If
                    Else
                        success = False
                    End If
                Else
                    success = TreeViewEngine.SelectedNode.Parent IsNot Nothing
                End If
            End If

            If Not success Then
                If numberPhase = StageNodeType.НомерКТ6 Then
                    message = $"КТ не может быть записана т.к. нарушена последовательность этапов.{vbCrLf}Пройдите заново цепочку в дереве по узлам от <типа изделия> до <номере запуска>,{vbCrLf}пересчитайте и запишите КТ заново."
                Else
                    message = $"Не введены данные на предыдущей закладке: {StageConstNames(numberPhase - 2)}{vbCrLf}Необходимо ввести предыдущие этапы."
                End If
                MessageBox.Show(message, "Пропуск этапа", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If

        Return success
    End Function

#Region "Снятие КТ"
    Private Sub TSButtonStartAcquisitionKT_CheckStateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonStartAcquisitionKT.CheckStateChanged
        If TSButtonStartAcquisitionKT.Checked Then StartAcquisitionKT()
    End Sub

    Private Sub StartAcquisitionKT()
        If keyNewNumberStarting = 0 Then
            ShowMessageEmptyPhase("Номер Запуска")
            TSButtonCancelSaveKT_Click(TSButtonCancelSaveKT, New EventArgs)
            Exit Sub
        End If

        ' проверка на введенные поля
        If IsVerificationFieldsCompleted(StageNodeType.ТипыИзделия1) OrElse
            IsVerificationFieldsCompleted(StageNodeType.НомерИзделия2) OrElse
            IsVerificationFieldsCompleted(StageNodeType.НомерСборки3) OrElse
            IsVerificationFieldsCompleted(StageNodeType.НомерПостановки4) OrElse
            IsVerificationFieldsCompleted(StageNodeType.НомерКТ6) Then

            TSButtonStartAcquisitionKT.Checked = False
            Exit Sub
        End If

        ' Это НомерКТ
        Dim controlKT As IUserControl = mFormParrent.Manager.ControlsForPhase.Item(StageNames(StageNames.Count - 1)).Item(StageNames(StageNames.Count - 1))
        Dim nKT As Integer = CInt(controlKT.UserValue) ' первоначальное присваивание

        If TSButtonStartAcquisitionKT.Checked = True Then
            Using cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
                Dim cmd As OleDbCommand = cn.CreateCommand
                Dim rdr As OleDbDataReader

                cmd.CommandType = CommandType.Text
                cn.Open()
                cmd.CommandText = $"SELECT TOP 1 * FROM [6НомерКТ] Where keyНомерЗапуска ={idNumberStarting} ORDER BY [6НомерКТ].НомерКТ DESC;"
                rdr = cmd.ExecuteReader

                If rdr.Read() Then nKT = CInt(rdr("НомерКТ"))

                rdr.Close()
            End Using

            keyNewNumberKT = 0
            ' эти KEY определяются и при новых и при щелчках на узлах
            ' зная эти индексы производим запись и вставляем в набор recordset strОбъединенныйЗапрос объединеняющей все поля таблиц
            ' очистка сетки
            ' блокировка "Пуск"
            ' блокировка, счетчик+1,
            controlKT.UserValue = CStr(nKT + 1)
            isAbleDecrementCountKT = True
            LockControlsDuringAcquisitionKT()
            mFormParrent.Manager.ClearAcquisitionValue()
            ' счетчик Время Сбора
            Dim timeAcquisition As Integer = CInt(mFormParrent.Manager.ControlsForPhase.Item(StageNames(StageNames.Length - 1)).Item("ВремяОсреднения").UserValue)
            ' индикатор Осталось До Конца обратного отсчета уменьшает счетчик в зависимости от количества
            ' накопленных данных и в зависимости от частоты сбора
            mFormParrent.Manager.CounterKT = CInt((timeAcquisition * (mFormParrent.Manager.FrequencyBackground / mFormParrent.Manager.CounterGraph)) / SetFrequencyWindowUpdate(mFormParrent.Manager.FrequencyBackground))

            If mFormParrent.Manager.CounterKT = 0 Then mFormParrent.Manager.CounterKT = 3 ' запас для выставления флага Запрос Значений Всех Каналов

            TSTextBoxRemainedUntilEnd.Text = mFormParrent.Manager.CounterKT.ToString
            mFormParrent.Manager.CounterAppend = 0
            ' посылка на сервер сообщение о начале сбора
            ShowMessageOnStatusPanel("Идет накопление данных")

            startAcquisition = TimeOfDay ' Время Сбора
            ' посылка на сервер о начале сбора
            If Not mFormParrent.Manager.IsSwohSnapshot Then
                'mFormParrent.ReaderWriterCommander.ManagerAllTargets.Targets("Клиент:2").CommandWriterQueue.Enqueue(New NetCommandForTask(ПоставитьМеткуКТ, {$"Начало КТ №:{controlKT.UserValue} время:{startAcquisition}"}))
                mFormParrent.ReaderWriterCommander.ManagerAllTargets.ListTargets(0).CommandWriterQueue.Enqueue(New NetCommandForTask(ПоставитьМеткуКТ, {$"Начало КТ №:{controlKT.UserValue} время:{Date.Now.ToLongTimeString}"}))
            End If

            mFormParrent.Manager.NeedToRewrite = True ' для страховки чтобы обновить при считывании настроечные параметры в расчете(в том числе и режиме Форсаж)
            TSButtonStartAcquisitionKT.Text = "Идёт сбор"
            IsCalculatingKT = True
        End If
    End Sub

    Private Function SetFrequencyWindowUpdate(ByVal frequencyBackground As Integer) As Double
        Select Case frequencyBackground
            Case 50
                Return 1.6 '5'не успевает
                Exit Select
            Case 100
                Return 3.2 '5'не успевает
                Exit Select
            Case Else ' 1, 2, 5, 10, 20
                Return 1
        End Select
    End Function

    ''' <summary>
    ''' Обработать Накопление
    ''' </summary>
    Public Sub ProcessAcquisitionKT()
        ' вызывается из менеджера после окончания накопления
        ' номерКТ
        Dim numberKT As String = mFormParrent.Manager.ControlsForPhase.Item(StageNames(StageNames.Length - 1)).Item(StageNames(StageNames.Length - 1)).UserValue

        stopAcquisition = TimeOfDay
        ShowMessageOnStatusPanel("Идет обработка данных")
        ' подсчет и заполнение сетки
        PopulateReportKT() ' явно обновить сетку т.к. КТ может быть не записана (после записи КТ заново подсвечиваетя)

        ' разблокировка "Записать" "Отменить"
        If mFormParrent.Manager.ErrorPanelVisible = True Then
            ShowMessageOnStatusPanel("Обработка произведена с ошибкой")
            TSButtonSaveKT.Visible = False
            TSButtonCancelSaveKT.Visible = False
        Else
            ShowMessageOnStatusPanel("Обработка завершена")
            TSButtonSaveKT.Visible = True
            TSButtonCancelSaveKT.Visible = True
        End If

        TSButtonStartAcquisitionKT.Text = "Готово"
        DataGridViewReportKT.Enabled = True ' предусмотреть возможность коррекции
        ' посылка на сервер о конце сбора
        If Not mFormParrent.Manager.IsSwohSnapshot Then
            'mFormParrent.ReaderWriterCommander.ManagerAllTargets.Targets("Клиент:2").CommandWriterQueue.Enqueue(New NetCommandForTask(ПоставитьМеткуКТ, {$"Конец КТ №:{numberKT} время:{stopAcquisition}"}))
            mFormParrent.ReaderWriterCommander.ManagerAllTargets.ListTargets(0).CommandWriterQueue.Enqueue(New NetCommandForTask(ПоставитьМеткуКТ, {$"Конец КТ №:{numberKT} время:{Date.Now.ToLongTimeString}"}))
        End If
    End Sub

    ''' <summary>
    ''' Заполнить Сетку
    ''' </summary>
    Private Sub PopulateReportKT()
        For I As Integer = 0 To StageNames.Count - 1
            If mFormParrent.Manager.ControlsForPhase.Item(StageNames(I)).Count > 0 Then
                For Each itemControl As IUserControl In mFormParrent.Manager.ControlsForPhase.Item(StageNames(I)).Values
                    mDataGridExcelBook.SetTextRC(itemControl.Row, itemControl.Col, itemControl.UserValue)
                Next
            End If
        Next

        For Each row As BaseFormDataSet.ИзмеренныеПараметрыRow In mFormParrent.Manager.MeasurementDataTable.Rows
            mDataGridExcelBook.SetDoubleRC(row.Row, row.Col, CStr(row.НакопленноеЗначение)) 'Format(row.НакопленноеЗначение, "### ##0.00"))
        Next

        For Each row As BaseFormDataSet.РасчетныеПараметрыRow In mFormParrent.Manager.CalculatedDataTable.Rows
            mDataGridExcelBook.SetDoubleRC(row.Row, row.Col, CStr(row.НакопленноеЗначение))
        Next
    End Sub

    Private Sub TSButtonSaveKT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonSaveKT.Click
        If TypeRecord = RecordType.Acquisition Then
            SaveAcquisitionKT()
        ElseIf TypeRecord = RecordType.Recalculation Then
            SaveAfterRecalculateKT()
        End If
    End Sub

    ''' <summary>
    ''' Записать КТ После Пересчета
    ''' Значения пользователя для контролов перезаписываются только с этого этапа, другие не берутся.
    ''' </summary>
    Private Sub SaveAfterRecalculateKT()
        Dim strSQL As String
        Dim cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
        Dim cb As OleDbCommandBuilder
        Dim dt7ParametersValueKT As DataTable
        Dim da7ParametersValueKT As OleDbDataAdapter
        Dim ds As New DataSet

        Dim dtDataTable As New DataTable
        Dim odaDataAdapter As OleDbDataAdapter

        UnlockControlsAfterAcquisitionKT()
        UnlockPhaseTabPage(StageNodeType.НомерЗапуска5)

        Const numberPhase As Integer = StageNodeType.НомерКТ6

        Try
            ' здесь записываются значения контролов этапа
            If Not IsContainsParrentNode(StageNodeType.НомерКТ6) Then Exit Sub
            If IsVerificationFieldsCompleted(numberPhase) Then Exit Sub

            UnlockControls(numberPhase)

            'strSQL = "SELECT ЗначенияКонтроловДля" & ИменаЭтапов(iNumberPhase) & ".ЗначениеПользователя, КонтролыДля" & ИменаЭтапов(iNumberPhase) & ".Name " & _
            '        "FROM КонтролыДля" & ИменаЭтапов(iNumberPhase) & " RIGHT JOIN (" & (iNumberPhasePlus).ToString & ИменаЭтапов(iNumberPhase) & " RIGHT JOIN ЗначенияКонтроловДля" & ИменаЭтапов(iNumberPhase) & " ON [" & (iNumberPhasePlus).ToString & ИменаЭтапов(iNumberPhase) & "].key" & ИменаЭтапов(iNumberPhase) & " = ЗначенияКонтроловДля" & ИменаЭтапов(iNumberPhase) & ".keyУровень) ON КонтролыДля" & ИменаЭтапов(iNumberPhase) & ".keyКонтролДляУровня = ЗначенияКонтроловДля" & ИменаЭтапов(iNumberPhase) & ".keyКонтролДляУровня " & _
            '        "WHERE ((([" & (iNumberPhasePlus).ToString & ИменаЭтапов(iNumberPhase) & "].key" & ИменаЭтапов(iNumberPhase) & ")=" & keyNEWНомерКонтрТочки.ToString & ")) "
            'strSQL = "SELECT КонтролыДляНомерКТ.Name, ЗначенияКонтроловДляНомерКТ.ЗначениеПользователя " & _
            '        "FROM КонтролыДляНомерКТ RIGHT JOIN (6НомерКТ RIGHT JOIN ЗначенияКонтроловДляНомерКТ ON [6НомерКТ].keyНомерКТ = ЗначенияКонтроловДляНомерКТ.keyУровень) ON КонтролыДляНомерКТ.keyКонтролДляУровня = ЗначенияКонтроловДляНомерКТ.keyКонтролДляУровня " & _
            '        "WHERE ((([6НомерКТ].keyНомерКТ)=" & keyNEWНомерКонтрТочки.ToString & "));"
            'strSQL = "SELECT КонтролыДляНомерКТ.Name, ЗначенияКонтроловДляНомерКТ.ЗначениеПользователя " & _
            '        "FROM КонтролыДляНомерКТ RIGHT JOIN ЗначенияКонтроловДляНомерКТ ON КонтролыДляНомерКТ.keyКонтролДляУровня = ЗначенияКонтроловДляНомерКТ.keyКонтролДляУровня " & _
            '        "WHERE (((ЗначенияКонтроловДляНомерКТ.keyУровень)=" & keyNEWНомерКонтрТочки.ToString & "));"

            strSQL = "Select * " &
            "FROM(ЗначенияКонтроловДляНомерКТ) " &
            "WHERE(((ЗначенияКонтроловДляНомерКТ.keyУровень) = " & keyNewNumberKT.ToString & ")) " &
            "ORDER BY ЗначенияКонтроловДляНомерКТ.keyКонтролДляУровня;"
            ' по всей видимости контролы добавлялись в соответствии с МестоНаПанели 'ORDER BY " & strЭтап & ".МестоНаПанели;"
            ' наверно также добавлены и в коллекцию
            ' должно сработать Update по одной таблице т.к. dtDataTable по нескольким не срабатывает

            cn.Open()
            odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
            odaDataAdapter.Fill(dtDataTable)

            If dtDataTable.Rows.Count = mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 1)).Values.Count Then
                For Each itemControl As IUserControl In mFormParrent.Manager.ControlsForPhase.Item(StageNames(numberPhase - 1)).Values
                    For Each itemDataRow As DataRow In dtDataTable.Rows
                        If CInt(itemDataRow("keyКонтролДляУровня")) = itemControl.keyControlStage Then
                            itemDataRow("ЗначениеПользователя") = itemControl.UserValue
                            Exit For
                        End If
                    Next
                Next

                cb = New OleDbCommandBuilder(odaDataAdapter)
                odaDataAdapter.UpdateCommand = cb.GetUpdateCommand
                odaDataAdapter.Update(dtDataTable)
                Thread.Sleep(500)
                dtDataTable.AcceptChanges()
            Else
                MessageBox.Show($"Размерность таблицы ЗначенияКонтроловДляНомерКТ не совпадает {vbCrLf}с размерностью настроечных контролов в ControlsForPhase для этапа КТ.{vbCrLf}Настройки для этапа КТ не будут перезаписаны.",
                                "Ошибка записи КТ в модуле <ЗаписатьКТПослеПересчета>!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            ' Исходные данные (измеренная информация)
            strSQL = "SELECT [7ЗначенияПараметровКТ].* FROM 7ЗначенияПараметровКТ Where keyНомерКТ =  " & keyNewNumberKT
            da7ParametersValueKT = New OleDbDataAdapter(strSQL, cn)
            da7ParametersValueKT.Fill(ds, "7ЗначенияПараметровКТ")
            dt7ParametersValueKT = ds.Tables("7ЗначенияПараметровКТ")

            For Each itemRow As DataRow In dt7ParametersValueKT.Rows
                Dim row As BaseFormDataSet.ИзмеренныеПараметрыRow = mFormParrent.Manager.MeasurementDataTable.FindByИмяПараметра(CStr(itemRow("ИмяПараметра")))
                If row IsNot Nothing Then itemRow("Значение") = row.НакопленноеЗначение

                ' повторить для расчетного параметра
                Dim rowCalc As BaseFormDataSet.РасчетныеПараметрыRow = mFormParrent.Manager.CalculatedDataTable.FindByИмяПараметра(CStr(itemRow("ИмяПараметра")))
                If rowCalc IsNot Nothing Then itemRow("Значение") = rowCalc.НакопленноеЗначение
            Next

            cb = New OleDbCommandBuilder(da7ParametersValueKT)
            da7ParametersValueKT.UpdateCommand = cb.GetUpdateCommand
            da7ParametersValueKT.Update(dt7ParametersValueKT)
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Ошибка записи КТ в модуле <ЗаписатьКТПослеПересчета>!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Close()
            TSButtonStartAcquisitionKT.Text = "Пуск"
            ShowMessageOnStatusPanel("КТ после пересчёта записана")
        End Try
    End Sub

    ''' <summary>
    ''' Записать КТ После Сбора
    ''' </summary>
    Private Sub SaveAcquisitionKT()
        ' вначале стандартная обработка для записи содержимого элементов закладки
        Dim strSQL As String
        Dim cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
        Dim drDataRow As DataRow
        Dim cb As OleDbCommandBuilder
        Dim dt7ParametersValueKT As DataTable
        Dim da7ParametersValueKT As OleDbDataAdapter
        Dim ds As New DataSet

        mFormParrent.Manager.ControlsForPhase.Item(StageNames(StageNames.Count - 1)).Item("ВремяНачала").UserValue = startAcquisition.ToLongTimeString
        mFormParrent.Manager.ControlsForPhase.Item(StageNames(StageNames.Count - 1)).Item("ВремяКонца").UserValue = stopAcquisition.ToLongTimeString
        UnlockControlsAfterAcquisitionKT()
        UnlockPhaseTabPage(StageNodeType.НомерЗапуска5)

        Try
            AddNewPhase(StageNodeType.НомерКТ6) ' здесь записываются значения контролов этапа
            Thread.Sleep(500)

            cn.Open()
            ' Исходные данные (измеренная информация)
            strSQL = "SELECT [7ЗначенияПараметровКТ].* FROM 7ЗначенияПараметровКТ Where keyНомерКТ =  " & keyNewNumberKT
            da7ParametersValueKT = New OleDbDataAdapter(strSQL, cn)
            da7ParametersValueKT.Fill(ds, "7ЗначенияПараметровКТ")
            dt7ParametersValueKT = ds.Tables("7ЗначенияПараметровКТ")

            For Each row As BaseFormDataSet.ИзмеренныеПараметрыRow In mFormParrent.Manager.MeasurementDataTable.Rows
                drDataRow = dt7ParametersValueKT.NewRow
                drDataRow.BeginEdit()
                drDataRow("keyНомерКТ") = keyNewNumberKT
                drDataRow("ИмяПараметра") = row.ИмяПараметра
                drDataRow("Значение") = row.НакопленноеЗначение
                drDataRow.EndEdit()
                dt7ParametersValueKT.Rows.Add(drDataRow)
            Next

            ' повторить для расчетного параметра
            For Each row As BaseFormDataSet.РасчетныеПараметрыRow In mFormParrent.Manager.CalculatedDataTable.Rows
                drDataRow = dt7ParametersValueKT.NewRow
                drDataRow.BeginEdit()
                drDataRow("keyНомерКТ") = keyNewNumberKT
                drDataRow("ИмяПараметра") = row.ИмяПараметра
                drDataRow("Значение") = row.НакопленноеЗначение
                drDataRow.EndEdit()
                dt7ParametersValueKT.Rows.Add(drDataRow)
            Next

            cb = New OleDbCommandBuilder(da7ParametersValueKT)
            da7ParametersValueKT.Update(dt7ParametersValueKT)

            Dim cmd As OleDbCommand = cn.CreateCommand
            cmd.CommandType = CommandType.Text
            With mFormParrent.Manager
                For I As Integer = 1 To .NameRegistrationParameters.Length - 1
                    strSQL = $"INSERT INTO ЗначенияВсехКаналов ( keyНомерКТ, ИмяКанала, Значение ) VALUES ({keyNewNumberKT}, '{ .NameRegistrationParameters(I)}', { .ChannelsValue(I)});"
                    cmd.CommandText = strSQL
                    cmd.ExecuteNonQuery()
                Next
            End With

            TSButtonRecalculationKT.Visible = mFormParrent.Manager.IsSwohSnapshot
            PopulateListViewMembers(StageNodeType.НомерКТ6, keyNewNumberStarting, cn)
        Catch ex As Exception
            MessageBox.Show(ex.ToString, $"Ошибка записи КТ в процедуре <{NameOf(SaveAcquisitionKT)}>!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Close()
            TSButtonStartAcquisitionKT.Text = "Пуск"
            ShowMessageOnStatusPanel("КТ записана")
        End Try
    End Sub

    Private Sub TSButtonCancelSaveKT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonCancelSaveKT.Click
        Dim nKT As Integer
        UnlockControlsAfterAcquisitionKT()

        If isAbleDecrementCountKT Then
            ' вернуть счетчик
            nKT = Integer.Parse(mFormParrent.Manager.ControlsForPhase.Item(StageNames(StageNames.Count - 1)).Item(StageNames(StageNames.Count - 1)).UserValue) - 1

            If nKT < 0 Then nKT = 0

            mFormParrent.Manager.ControlsForPhase.Item(StageNames(StageNames.Count - 1)).Item(StageNames(StageNames.Count - 1)).UserValue = CStr(nKT)
            isAbleDecrementCountKT = False
        End If

        TSButtonStartAcquisitionKT.Text = "Пуск"
        ShowMessageOnStatusPanel("Готов")
    End Sub

    ''' <summary>
    ''' Блокирока При Сборе
    ''' </summary>
    Private Sub LockControlsDuringAcquisitionKT()
        If TSButtonStartAcquisitionKT.Checked Then
            LockControls(StageNodeType.НомерЗапуска5)
            EnableBlocking()
            TSButtonStartAcquisitionKT.Enabled = False
            LabelRemainedUntilEnd.Visible = True
            TSTextBoxRemainedUntilEnd.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' Разблокирока После Сбора
    ''' </summary>
    Private Sub UnlockControlsAfterAcquisitionKT()
        If TSButtonStartAcquisitionKT.Checked OrElse TSButtonRecalculationKT.Visible Then
            UnlockControls(StageNodeType.НомерЗапуска5)
            TSButtonStartAcquisitionKT.Enabled = True
            TSButtonStartAcquisitionKT.Checked = False
            LabelRemainedUntilEnd.Visible = False
            TSTextBoxRemainedUntilEnd.Visible = False
            TSButtonSaveKT.Visible = False
            TSButtonCancelSaveKT.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' включить Блокировку
    ''' </summary>
    Private Sub EnableBlocking()
        TSMenuItemBlocking.Checked = True
        isBlocking = True
        TSMenuItemBlocking.Text = "Блокировка &включена"
        StatusStripBar.Items(1).Text = "Блокировка проводника включена"
    End Sub

    ''' <summary>
    ''' выключить Блокировку
    ''' </summary>
    Private Sub DisenableBlocking()
        isBlocking = False
        'mnuБлокировка.Checked = False
        TSMenuItemBlocking.Text = "Блокировка &выключена"
        StatusStripBar.Items(1).Text = "Блокировка проводника выключена"
    End Sub
#End Region

#Region "Handles.Click"
    ''' <summary>
    ''' Вывести окно сообщения по аналогии с окном MessageBox но без модальности
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="caption"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetFormMessageBox(message As String, caption As String) As FormMessageBox
        Dim frmMessageBox As FormMessageBox = New FormMessageBox(message, caption)

        frmMessageBox.Show()
        frmMessageBox.Activate()
        frmMessageBox.Refresh()

        Return frmMessageBox
    End Function

    Private Function CreateExcelBookForKTAsync(isPrintSheetExcel As Boolean) As Task
        Return Task.Run(Sub()
                            mDataGridExcelBook.CreateExcelBookForKT(StageConstNames, StageNames, mFormParrent.Manager, isPrintSheetExcel, pathExcelProtocolCalculateKT)
                        End Sub)
    End Function

    'Private Async Sub Handler(ByVal prms As Params)
    '    Dim r = Await Task.Run(Function() CalcSomething(prms))
    '    ProcessResult(r)
    'End Sub

    'Private Async Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs)
    '        Dim progressReporter = New Progress(Of Object)(AddressOf ReportProgress)
    '        Await DoSomeWorkAsync(progressReporter)
    '    End Sub

    '    Private Sub ReportProgress(ByVal value As Object)
    '        If Me.InvokeRequired Then Throw New Exception()
    '    End Sub

    '    Private Async Function DoSomeWorkAsync(ByVal progressReporter As IProgress(Of Object)) As Task
    '        progressReporter.Report(Nothing)
    '        Await Task.Delay(100).ConfigureAwait(False)
    '        progressReporter.Report(Nothing)
    '    End Function

    Private Async Sub CreateExcelBookForKT(isPrintSheetExcel As Boolean)
        TSMenuItemFileSaveAsExcel.Enabled = False
        TSMenuItemPrint.Enabled = False
        Dim frmMessageBox As FormMessageBox = GetFormMessageBox(If(isPrintSheetExcel, "Подождите, идёт печать протокола...", "Подождите, идёт сохранение протокола..."),
                                                                 If(isPrintSheetExcel, "Печать протокола контрольной точки", "Сохранение протокола контрольной точки"))

        Await CreateExcelBookForKTAsync(isPrintSheetExcel)
        TSMenuItemFileSaveAsExcel.Enabled = True
        TSMenuItemPrint.Enabled = True
        frmMessageBox.Hide()
        frmMessageBox.Close()
    End Sub

    Private Sub TSMenuItemFileSaveAsExcel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TSMenuItemFileSaveAsExcel.Click
        With Me.SaveFileDialogAsExcel
            .FileName = vbNullString
            .Title = "Сохранение протокола в формате Excel"
            .InitialDirectory = "D:\"
            .DefaultExt = ".xlsx"
            .RestoreDirectory = True
            ' установить флаг атрибутов
            .Filter = "Книга Excel (*.xlsx)|*.xlsx"

            If .ShowDialog() = DialogResult.OK AndAlso Len(.FileName) <> 0 Then
                pathExcelProtocolCalculateKT = .FileName
            End If
        End With

        If pathExcelProtocolCalculateKT Is Nothing Then Exit Sub

        CreateExcelBookForKT(False)
    End Sub

#Region "TypeEngine button events"
    Private Sub TSButtonNewTypeEngine_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonNewTypeEngine.CheckedChanged
        If TSButtonNewTypeEngine.Checked Then ClearAndUnlockPhase(StageNodeType.ТипыИзделия1)
    End Sub

    Private Sub TSButtonOKTypeEngine_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonOKTypeEngine.Click
        AddNewPhase(StageNodeType.ТипыИзделия1)
    End Sub

    Private Sub TSButtonCancelTypeEngine_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonCancelTypeEngine.Click
        LockPhaseOnCancel(StageNodeType.ТипыИзделия1)
    End Sub
#End Region

#Region "NumberEngine button events"
    Private Sub TSButtonNewNumberEngine_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonNewNumberEngine.CheckedChanged
        If TSButtonNewNumberEngine.Checked Then ClearAndUnlockPhase(StageNodeType.НомерИзделия2)
    End Sub

    Private Sub TSButtonOKNumberEngine_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonOKNumberEngine.Click
        AddNewPhase(StageNodeType.НомерИзделия2)
    End Sub

    Private Sub TSButtonCancelNumberEngine_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonCancelNumberEngine.Click
        LockPhaseOnCancel(StageNodeType.НомерИзделия2)
    End Sub
#End Region

#Region "NumberBuild button events"
    Private Sub TSButtonNewNumberBuild_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonNewNumberBuild.CheckedChanged
        If TSButtonNewNumberBuild.Checked Then ClearAndUnlockPhase(StageNodeType.НомерСборки3)
    End Sub

    Private Sub TSButtonOkNumberBuild_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonOkNumberBuild.Click
        AddNewPhase(StageNodeType.НомерСборки3)
    End Sub

    Private Sub TSButtonCancelNumberBuild_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonCancelNumberBuild.Click
        LockPhaseOnCancel(StageNodeType.НомерСборки3)
    End Sub
#End Region

#Region "NumberStage button events"
    Private Sub TSButtonNewNumberStage_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonNewNumberStage.CheckedChanged
        If TSButtonNewNumberStage.Checked Then ClearAndUnlockPhase(StageNodeType.НомерПостановки4)
    End Sub

    Private Sub TSButtonNewNumberStage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonOKNumberStage.Click
        AddNewPhase(StageNodeType.НомерПостановки4)
    End Sub

    Private Sub TSButtonCancelNumberStage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonCancelNumberStage.Click
        LockPhaseOnCancel(StageNodeType.НомерПостановки4)
    End Sub
#End Region

#Region "NumberStarting button events"
    Private Sub TSButtonNewNumberStarting_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonNewNumberStarting.CheckedChanged
        If TSButtonNewNumberStarting.Checked Then ClearAndUnlockPhase(StageNodeType.НомерЗапуска5)
    End Sub

    Private Sub TSButtonOKNumberStarting_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonOKNumberStarting.Click
        AddNewPhase(StageNodeType.НомерЗапуска5)
    End Sub

    Private Sub TSButtonCancelNumberStarting_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonCancelNumberStarting.Click
        LockPhaseOnCancel(StageNodeType.НомерЗапуска5)
    End Sub
#End Region

    Private Sub TSMenuItemChangeCurrentDBase_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemChangeCurrentDBase.Click
        ChangeCurrentDBase()
    End Sub

    ''' <summary>
    ''' Сменить текущую базу
    ''' </summary>
    Private Sub ChangeCurrentDBase()
        With Me.OpenFileDialogChangeCurrentDBase
            .FileName = vbNullString
            .Title = "Текущий каталог базы-> " & mFormParrent.Manager.PathKT
            .DefaultExt = "mdb"
            ' установить флаг атрибутов
            .Filter = CStr(mFormParrent.Tag) & " (*.mdb)|*.mdb"
            .RestoreDirectory = True

            If .ShowDialog() = Windows.Forms.DialogResult.OK AndAlso Len(.FileName) <> 0 Then
                mFormParrent.Manager.PathKT = .FileName

                For I As Integer = 1 To StageNames.Count
                    If I = StageNodeType.НомерКТ6 Then
                        FlowLayoutPanel6.Controls.Clear()
                    Else
                        TabControlPhases.TabPages("TabPage" & (I).ToString).Controls("FlowLayoutPanel" & (I).ToString).Controls.Clear()
                    End If
                Next

                InitializeOnLoadFormAndChangedDBase()
                mDataGridExcelBook = New DataGridExcelBook(DataGridViewReportKT)
                mDataGridExcelBook.GreateEmptyReport(StageConstNames, StageNames, mFormParrent.Manager)
                ExpandTreeByLastKT()
                UnlockControls(StageNodeType.НомерЗапуска5)
                EnableBlocking()
                TreeViewEngine.Enabled = True
                mFormParrent.varFormHierarchicalTable.InitializeMembers() ' сбросить настройки фильтров
            End If
        End With
    End Sub

    Private Sub TSMenuItemShowAllChannels_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemShowAllChannels.Click
        If idNumberKT = 0 Then
            MessageBox.Show("Нет выделенной контрольной точки.", "Отобразить все каналы", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            Dim frmFindChannel As New FormFindChannel(idNumberKT, mFormParrent.Manager)
            If frmFindChannel.ShowDialog = Windows.Forms.DialogResult.OK Then
                frmFindChannel.Dispose()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Показать настройку параметров
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMenuItemUnlockSettingPages_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemUnlockSettingPages.CheckedChanged
        ' здесь доступность меню и доступность таблиц в базовой форме модуля
        mFormParrent.Manager.IsEnabledTuningForms = TSMenuItemUnlockSettingPages.Checked
        TSButtonStartAcquisitionKT.Enabled = Not TSMenuItemUnlockSettingPages.Checked
        If Not mFormParrent.Manager.IsSwohSnapshot Then mFormParrent.varFormGraf.TextBoxCollect.Visible = Not TSMenuItemUnlockSettingPages.Checked
    End Sub

    Private Sub TSButtonRecalculationKT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonRecalculationKT.Click
        RecalculationKT()
    End Sub

    ''' <summary>
    ''' Пересчёт КТ
    ''' </summary>
    Private Sub RecalculationKT()
        If keyNewNumberKT = 0 Then
            MessageBox.Show("Снятая Контрольная Точка не была записана в базу.", "Пересчёт КТ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            ' проверить корректность введенных значенний
            For Each row As BaseFormDataSet.ИзмеренныеПараметрыRow In mFormParrent.Manager.MeasurementDataTable.Rows
                Try
                    Dim doubleRC As Double = mDataGridExcelBook.GetToDoubleRC(row.Row, row.Col)
                Catch ex As FormatException
                    MessageBox.Show($"Значение {row.ИмяПараметра} = {mDataGridExcelBook.GetToStringRC(row.Row, row.Col)} недопустимо", "Пересчет КТ", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End Try
            Next

            ' считать с ячеек значения и записать их в контрол
            For I As Integer = 0 To StageNames.Length - 1
                If mFormParrent.Manager.ControlsForPhase.Item(StageNames(I)).Count > 0 Then
                    For Each itemControl As IUserControl In mFormParrent.Manager.ControlsForPhase.Item(StageNames(I)).Values
                        itemControl.UserValue = mDataGridExcelBook.GetToStringRC(itemControl.Row, itemControl.Col)
                    Next
                End If
            Next

            ' считать значения после проверки
            For Each row As BaseFormDataSet.ИзмеренныеПараметрыRow In mFormParrent.Manager.MeasurementDataTable.Rows
                row.ИзмеренноеЗначение = mDataGridExcelBook.GetToDoubleRC(row.Row, row.Col)
            Next

            ' запустить пересчет
            mFormParrent.ClassCalculation.RecalculationKT()

            ' заново переписать в накопленные
            For Each row As BaseFormDataSet.ИзмеренныеПараметрыRow In mFormParrent.Manager.MeasurementDataTable.Rows
                row.НакопленноеЗначение = row.ИзмеренноеЗначение
            Next

            For Each row As BaseFormDataSet.РасчетныеПараметрыRow In mFormParrent.Manager.CalculatedDataTable.Rows
                row.НакопленноеЗначение = row.ВычисленноеПереведенноеЗначение
            Next

            ' вызвать показ кнопок для записи и ждать чтобы пользователь записал по желанию
            ProcessAcquisitionKT() ' там будет заново вызов ЗаполнитьСетку
            TypeRecord = FormMeasurementKT.RecordType.Recalculation
        End If
    End Sub

    Public Sub TSMenuItemPrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TSMenuItemPrint.Click
        Try
            CreateExcelBookForKT(True)
        Catch ex As Exception
            MessageBox.Show("Ошибка печати " & ex.ToString, "Печать", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub TSMenuItemBlocking_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemBlocking.CheckedChanged
        If TSMenuItemBlocking.Checked Then
            EnableBlocking()
            TreeViewEngine.Enabled = False
            LockTabs(False, StageNodeType.НомерЗапуска5)
        Else
            DisenableBlocking()
            UnlockControls(StageNodeType.НомерЗапуска5)
        End If
    End Sub

    Private Sub TabControlPhases_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TabControlPhases.SelectedIndexChanged
        Dim I As Integer = TabControlPhases.SelectedIndex

        For J As Integer = 0 To TabControlPhases.TabCount - 1
            TabControlPhases.TabPages.Item(J).ImageIndex = J
        Next

        TabControlPhases.SelectedTab.ImageIndex = I + 6

        If I = StageNodeType.НомерЗапуска5 - 1 Then ' Для нового запуска нужно перещёлкнуть узлы дерева
            If isLastRecord Then
                If CType(TreeViewEngine.SelectedNode, DirectoryNode).NodeType = StageNodeType.НомерКТ6 Then
                    TreeViewEngine.SelectedNode = TreeViewEngine.SelectedNode.Parent
                End If
            End If
        End If
    End Sub

    Private Sub ListViewMemberNode_ColumnClick(ByVal sender As Object, ByVal e As ColumnClickEventArgs) Handles ListViewMemberNode.ColumnClick
        ' установить свойство ListViewItemSorter в новый ListViewItemComparer объект.
        ListViewMemberNode.ListViewItemSorter = New ListViewItemComparer(e.Column)
        ' Sort метод вручную сортирует колонку на основании реализации ListViewItemComparer
        ListViewMemberNode.Sort()
    End Sub

    ''' <summary>
    ''' Реализует ручную сортировку элементов по столбцам
    ''' </summary>
    Class ListViewItemComparer
        Implements IComparer
        Public Property Col() As Integer

        Public Sub New()
            Col = 0
        End Sub

        Public Sub New(ByVal column As Integer)
            Col = column
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            Return [String].Compare(CType(x, ListViewItem).SubItems(Col).Text, CType(y, ListViewItem).SubItems(Col).Text)
        End Function
    End Class

#End Region

#Region "Удалить Узел"

    ''' <summary>
    ''' Очистить Пустые Запуски
    ''' </summary>
    Private Sub ClearEmptyStarting()
        Dim currentCursor As Cursor = Cursor.Current
        Cursor.Current = Cursors.WaitCursor
        Dim cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
        Dim listSqlDelete As New List(Of String) From {
            "DELETE * " &
        "FROM [6НомерКТ] " &
        "WHERE [6НомерКТ].keyНомерКТ IN " &
        "(SELECT [6НомерКТ].keyНомерКТ " &
        "FROM 6НомерКТ LEFT JOIN 7ЗначенияПараметровКТ ON [6НомерКТ].[keyНомерКТ] = [7ЗначенияПараметровКТ].[keyНомерКТ] " &
        "WHERE ((([7ЗначенияПараметровКТ].keyНомерКТ) Is Null)));",
            "DELETE * " &
        "FROM [5НомерЗапуска] " &
        "WHERE [5НомерЗапуска].keyНомерЗапуска IN " &
        "(SELECT [5НомерЗапуска].keyНомерЗапуска " &
        "FROM 5НомерЗапуска LEFT JOIN 6НомерКТ ON [5НомерЗапуска].[keyНомерЗапуска] = [6НомерКТ].[keyНомерЗапуска] " &
        "WHERE ((([6НомерКТ].keyНомерЗапуска) Is Null)));",
            "DELETE * " &
        "FROM [4НомерПостановки] " &
        "WHERE [4НомерПостановки].keyНомерПостановки IN " &
        "(SELECT [4НомерПостановки].keyНомерПостановки " &
        "FROM 4НомерПостановки LEFT JOIN 5НомерЗапуска ON [4НомерПостановки].[keyНомерПостановки] = [5НомерЗапуска].[keyНомерПостановки] " &
        "WHERE ((([5НомерЗапуска].keyНомерПостановки) Is Null)));",
            "DELETE * " &
        "FROM [3НомерСборки] " &
        "WHERE [3НомерСборки].keyНомерСборки IN " &
        "(SELECT [3НомерСборки].keyНомерСборки " &
        "FROM 3НомерСборки LEFT JOIN 4НомерПостановки ON [3НомерСборки].[keyНомерСборки] = [4НомерПостановки].[keyНомерСборки] " &
        "WHERE ((([4НомерПостановки].keyНомерСборки) Is Null)));",
            "DELETE * " &
        "FROM [2НомерИзделия] " &
        "WHERE [2НомерИзделия].keyНомерИзделия IN " &
        "(SELECT [2НомерИзделия].keyНомерИзделия " &
        "FROM 2НомерИзделия LEFT JOIN 3НомерСборки ON [2НомерИзделия].[keyНомерИзделия] = [3НомерСборки].[keyНомерИзделия] " &
        "WHERE ((([3НомерСборки].keyНомерИзделия) Is Null)));",
            "DELETE * " &
        "FROM [1ТипИзделия] " &
        "WHERE [1ТипИзделия].keyТипИзделия IN " &
        "(SELECT [1ТипИзделия].keyТипИзделия " &
        "FROM 1ТипИзделия LEFT JOIN 2НомерИзделия ON [1ТипИзделия].[keyТипИзделия] = [2НомерИзделия].[keyТипИзделия] " &
        "WHERE ((([2НомерИзделия].keyТипИзделия) Is Null)));"
        }

        Try
            cn.Open()
            For Each sqlDel As String In listSqlDelete
                Dim cmd As OleDbCommand = cn.CreateCommand
                cmd.CommandType = CommandType.Text
                cmd.CommandText = sqlDel
                cmd.ExecuteNonQuery()
                Thread.Sleep(1000)
                Application.DoEvents()
            Next

            Populate1TypeEngine(cn)
            cn.Close()

            ExpandTreeByLastKT()
            mFormParrent.varFormHierarchicalTable.InitializeMembers() ' сбросить настройки фильтров
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Ошибка при удалении пустых запусков без КТ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            Cursor.Current = currentCursor
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Удалить Узел
    ''' </summary>
    ''' <param name="nodeSelectType"></param>
    Private Sub RemoveNode(ByVal nodeSelectType As StageNodeType)
        Dim strSQL As String = Nothing
        Dim findId As Integer
        Dim success As Boolean ' узел Найден

        Select Case nodeSelectType
            Case StageNodeType.ТипыИзделия1
                strSQL = $"DELETE FROM [1ТипИзделия] WHERE (keyТипИзделия = {idTypeEngine})"
                findId = idTypeEngine
                idTypeEngine = 0
            Case StageNodeType.НомерИзделия2
                strSQL = $"DELETE FROM [2НомерИзделия] WHERE (keyНомерИзделия = {idNumberEngine})"
                findId = idNumberEngine
                idNumberEngine = 0
            Case StageNodeType.НомерСборки3
                strSQL = $"DELETE FROM [3НомерСборки] WHERE (keyНомерСборки = {idNumberBuild})"
                findId = idNumberBuild
                idNumberBuild = 0
            Case StageNodeType.НомерПостановки4
                strSQL = $"DELETE FROM [4НомерПостановки] WHERE (keyНомерПостановки = {idNumberStage})"
                findId = idNumberStage
                idNumberStage = 0
            Case StageNodeType.НомерЗапуска5
                strSQL = $"DELETE FROM [5НомерЗапуска] WHERE (keyНомерЗапуска = {idNumberStarting})"
                findId = idNumberStarting
                idNumberStarting = 0
            Case StageNodeType.НомерКТ6
                strSQL = $"DELETE FROM [6НомерКТ] WHERE (keyНомерКТ = {idNumberKT})"
                findId = idNumberKT
                idNumberKT = 0
        End Select

        Select Case nodeSelectType
            Case StageNodeType.ТипыИзделия1
                Dim nodeFound As DirectoryNode = Nothing
                ' ищем в корне
                For Each NodeLoop As DirectoryNode In TreeViewEngine.Nodes
                    If NodeLoop.KeyId = findId Then
                        nodeFound = NodeLoop
                        success = True
                        Exit For
                    End If
                Next

                If nodeFound IsNot Nothing Then TreeViewEngine.Nodes.Remove(nodeFound)
            Case Else
                ' ищем родителя
                Dim arrKeyID As Integer() = {0, idTypeEngine, idNumberEngine, idNumberBuild, idNumberStage, idNumberStarting, idNumberKT}

                For Each itemNode As DirectoryNode In GetParrentDirectoryNode(arrKeyID, CType(nodeSelectType - 1, StageNodeType)).Nodes
                    If itemNode.KeyId = findId Then
                        itemNode.Parent.Nodes.Remove(itemNode)
                        success = True
                        Exit For
                    End If
                Next
        End Select

        If success Then
            Dim cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathKT))
            cn.Open()
            Try
                Dim cmd As OleDbCommand = cn.CreateCommand
                cmd.CommandType = CommandType.Text
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                MessageBox.Show(ex.ToString, "Ошибка при удалении узла", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Finally
                cn.Close()
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Настроить Видимость Пунктов Меню Удаления
    ''' </summary>
    ''' <param name="nodeSelectType"></param>
    Private Sub SetEnabledRemoveToolStripMenuItem(ByVal nodeSelectType As StageNodeType)
        For Each tsMenuItem As ToolStripMenuItem In ContextMenuStripTreeDelete.Items
            tsMenuItem.Enabled = False
        Next
        For Each tsMenuItem As ToolStripMenuItem In RemoveToolStripMenuItem.DropDownItems
            tsMenuItem.Enabled = False
        Next

        TSMenuItemClearEmptyStarting.Enabled = True
        ContextMenuItemClearEmptyStarting.Enabled = True

        Select Case nodeSelectType
            Case StageNodeType.ТипыИзделия1
                TSMenuItemRemoveTypeEngine.Enabled = True
                ContextMenuItemTypeEngine.Enabled = True
            Case StageNodeType.НомерИзделия2
                TSMenuItemRemoveNumberEngine.Enabled = True
                ContextMenuItemRemoveNumberEngine.Enabled = True
            Case StageNodeType.НомерСборки3
                TSMenuItemRemoveNumberBuild.Enabled = True
                ContextMenuItemRemoveNumberBuild.Enabled = True
            Case StageNodeType.НомерПостановки4
                TSMenuItemRemoveNumberStage.Enabled = True
                ContextMenuItemRemoveNumberStage.Enabled = True
            Case StageNodeType.НомерЗапуска5
                TSMenuItemRemoveNumberStarting.Enabled = True
                ContextMenuItemRemoveNumberStarting.Enabled = True
            Case StageNodeType.НомерКТ6
                TSMenuItemRemoveNumberKT.Enabled = True
                ContextMenuItemRemoveNumberKT.Enabled = True
        End Select
    End Sub

    Private Sub RemoveTypeEngine_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemRemoveTypeEngine.Click, ContextMenuItemTypeEngine.Click
        RemoveNode(StageNodeType.ТипыИзделия1)
    End Sub

    Private Sub RemoveNumberEngine_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemRemoveNumberEngine.Click, ContextMenuItemRemoveNumberEngine.Click
        RemoveNode(StageNodeType.НомерИзделия2)
    End Sub

    Private Sub RemoveNumberBuild_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemRemoveNumberBuild.Click, ContextMenuItemRemoveNumberBuild.Click
        RemoveNode(StageNodeType.НомерСборки3)
    End Sub

    Private Sub RemoveNumberStage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemRemoveNumberStage.Click, ContextMenuItemRemoveNumberStage.Click
        RemoveNode(StageNodeType.НомерПостановки4)
    End Sub

    Private Sub RemoveNumberStarting_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemRemoveNumberStarting.Click, ContextMenuItemRemoveNumberStarting.Click
        RemoveNode(StageNodeType.НомерЗапуска5)
    End Sub

    Private Sub RemoveNumberKT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemRemoveNumberKT.Click, ContextMenuItemRemoveNumberKT.Click
        RemoveNode(StageNodeType.НомерКТ6)
    End Sub

    Private Sub ClearEmptyStarting_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemClearEmptyStarting.Click, ContextMenuItemClearEmptyStarting.Click
        ClearEmptyStarting()
    End Sub
#End Region

    Private Sub TimerStatusBar_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles TimerStatusBar.Tick
        StatusStripBar.Items(3).Text = Date.Today.ToShortDateString
        StatusStripBar.Items(5).Text = Date.Now.ToShortTimeString
    End Sub

    Private Sub ShowMessageOnStatusPanel(ByVal message As String)
        StatusStripBar.Items(0).Text = message
    End Sub
End Class


'''''''''''''''''''''''
'Private ts As TimeSpan
'Private dtStart As DateTime
'Private dtEnd As DateTime
'Private strВремяХодаТурели As String

'Private Sub ВремяСбора()
'    'вычислить разницу хода турели
'    dtEnd = DateTime.Now
'    ts = dtEnd.Subtract(dtStart).Duration
'    DisplayTSProperties(ts)
'End Sub

'Private Sub DisplayTSProperties(ByVal ts As TimeSpan)
'    ' Use instance properties of the TimeSpan type.
'    ' Demonstrates:
'    '  TimeSpan.Days
'    '  TimeSpan.Hours
'    '  TimeSpan.Milliseconds
'    '  TimeSpan.Minutes
'    '  TimeSpan.Seconds
'    '  TimeSpan.Ticks
'    '  TimeSpan.TotalDays
'    '  TimeSpan.TotalHours
'    '  TimeSpan.TotalMilliseconds
'    '  TimeSpan.TotalMinutes
'    '  TimeSpan.TotalSeconds
'    'Try
'    'lblDays.Text = ts.Days.ToString
'    'lblHours.Text = ts.Hours.ToString
'    'lblMilliseconds.Text = ts.Milliseconds.ToString
'    'lblMinutes.Text = ts.Minutes.ToString
'    'lblSeconds.Text = ts.Seconds.ToString
'    'lblTimeSpanTicks.Text = ts.Ticks.ToString
'    'lblTotalDays.Text = ts.TotalDays.ToString
'    'lblTotalHours.Text = ts.TotalHours.ToString
'    'lblTotalMilliseconds.Text = ts.TotalMilliseconds.ToString
'    'lblTotalMinutes.Text = ts.TotalMinutes.ToString
'    'lblTotalSeconds.Text = ts.TotalSeconds.ToString

'    strВремяХодаТурели = ts.Minutes.ToString & " мин. " & ts.Seconds.ToString & " сек."
'    'ПанельВремяХода.Text = strВремяХодаТурели
'    'Catch exp As Exception
'    '    MessageBox.Show(exp.Message, Me.Text)
'    'End Try
'End Sub



'вначале сделаем запрос на предыдущую КТ для того чтобы по последней записи узнать keyЗначенияПараметровКТ
''переместиться к последней записи
'lngЗначенияПараметровКТ = 1
'strSQL = "SELECT TOP 1 keyЗначенияПараметровКТ FROM 7ЗначенияПараметровКТ ORDER BY keyЗначенияПараметровКТ DESC;"
'cmd.CommandText = strSQL
'rdr = cmd.ExecuteReader
'If rdr.Read() = True Then
'    'Do While rdr.Read()
'    lngЗначенияПараметровКТ = rdr("keyЗначенияПараметровКТ")
'    'Loop
'End If
'rdr.Close()
'lngЗначенияПараметровКТ += 1



'Проверка чтобы не было 2 раза "Начальный режим" Or cmbРежим = "Конечный режим"
''Запись и получение keyNEWНомерКонтрТочки=keyНомерКТ
'strSQL = "SELECT [6НомерКТ].*, ЗначенияКонтроловДляНомерКТ.ЗначениеПользователя " & _
'"FROM КонтролыДляНомерКТ RIGHT JOIN (6НомерКТ RIGHT JOIN ЗначенияКонтроловДляНомерКТ ON [6НомерКТ].keyНомерКТ = ЗначенияКонтроловДляНомерКТ.keyУровень) ON КонтролыДляНомерКТ.keyКонтролДляУровня = ЗначенияКонтроловДляНомерКТ.keyКонтролДляУровня " & _
'"WHERE (((КонтролыДляНомерКТ.Name)='Режим') AND (([6НомерКТ].keyНомерЗапуска)=" & keyNEWНомерЗапуска & "));"

'cn.Open()
'odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
'odaDataAdapter.Fill(dtDataTable)
'Dim cmd As OleDbCommand = cn.CreateCommand
'cmd.CommandType = CommandType.Text

'If dtDataTable.Rows.Count > 0 Then
'    If cmbРежим = "Начальный режим" Or cmbРежим = "Конечный режим" Then
'        'должны быть только по 1 на данном запуске
'        aRows = dtDataTable.Select(strCriteria)
'        If aRows.Length > 0 Then
'            For Each drDataRow In aRows
'                Dim NodeLoop As DirectoryNode
'                'ищем родителя
'                Dim arrKeyID2() As Integer = {0, keyNEWТипИзделия, keyNEWНомерИзделия, keyNEWНомерСборки, keyNEWНомерПостановки, keyNEWНомерЗапуска, keyNEWНомерКонтрТочки}
'                Dim keyНомерУдаляемойКонтрТочки As Integer = drDataRow("keyНомерКТ")
'                For Each NodeLoop In НайтиРодителя(arrKeyID2, ТипУзла.НомерЗапуска5).Nodes
'                    If NodeLoop.KeyId = keyНомерУдаляемойКонтрТочки Then
'                        NodeLoop.Parent.Nodes.Remove(NodeLoop)
'                        Exit For
'                    End If
'                Next
'                'drDataRow.Delete()
'                Try
'                    strSQL = "DELETE FROM [6НомерКТ] WHERE keyНомерКТ = " & Str(keyНомерУдаляемойКонтрТочки)
'                    cmd.CommandText = strSQL
'                    cmd.ExecuteNonQuery()
'                Catch ex As Exception
'                    MessageBox.Show(ex.ToString, "Ошибка при удалении строк", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'                End Try
'            Next
'            'при обновлении почему-то ошибка, заменил явным "DELETE FROM
'            'cb = New OleDbCommandBuilder(odaDataAdapter)
'            'odaDataAdapter.Update(dtDataTable)
'            'dtDataTable.AcceptChanges()
'        End If
'    End If
'End If
'cn.Close()

'Private Sub ВключитьВыключитьButtonEnabled(ByVal blnEnabled As Boolean)
'    tsbПускСбора.Enabled = blnEnabled
'    tsbПускСбора.Checked = Not blnEnabled
'    tsbПрерватьЗамерПоля.Enabled = Not blnEnabled
'    LedОтсечка.Visible = Not blnEnabled
'End Sub

'Private Sub tsbПускСбора_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbПускСбора.CheckedChanged
'    'проверить, что сборщик запушен
'    If tsbПускСбора.Checked = True Then
'        mФормаРодителя.ВключитьВыключитьСчетчик(True)
'        'ВключитьВыключитьКурсоры(True)
'        ВключитьВыключитьButtonEnabled(False)
'        dtStart = DateTime.Now
'        sbStatusBar.Items("ПанельРегистратор").Text = "Запуск замера поля"
'        'sbStatusBar.Items("ПанельНомерПоля").Text = "Поле " & intНомерПоля
'        'sbStatusBar.Items.Item("ПанельУстановка").Text = "Установка " & intНомерСтенда
'        'зарегистрировать событие СобытиеПеремещения и включить счетчик
'        ОчисткаДляПоля()
'        XyCursorОтсечка.XPosition = ИндексОтсечекДляПоля
'        blnРисоватьГрафикСечений = True
'    Else
'        'ВключитьВыключитьКурсоры(False)
'        'снять регистрирацию событие СобытиеПеремещения и выключить счетчик
'        mФормаРодителя.ВключитьВыключитьСчетчик(False)
'    End If
'End Sub

'Параметр    n1пр     n2пр     Рвстат   Рвполн   Gвсумф   Gвсумпр Rпр
'            38,3     38,4     38,5     38,6     38,7     38,8   38,9
'Private Sub НадписьДавлений(ByVal blnСтолбы As Boolean, ByVal blnСечениеМ As Boolean)
'    If blnСтолбы = True Then
'        F1BookПодсчет.set_TextRC(37, 5, "Рб.ст.")
'        F1BookПодсчет.set_TextRC(37, 6, "Рм.ст.")
'    Else
'        If blnСечениеМ = True Then
'            F1BookПодсчет.set_TextRC(37, 5, "Рм")
'            F1BookПодсчет.set_TextRC(37, 6, "Рм*")
'        Else
'            F1BookПодсчет.set_TextRC(37, 5, "Рв")
'            F1BookПодсчет.set_TextRC(37, 6, "Рв*")
'        End If
'    End If
'End Sub

'это реализовано в FillCombo
'Public Sub ВыдатьПредупреждениеОНесоответствии()
'    Dim I As Integer
'    Dim Msg, TITLE As String
'    Dim strСтрока As String
'    'Dim blnНадоПерезаписать As Boolean
'    'если номера изменились, то перепишутся новые, если параметр отсутствует, то ему присвоится conПараметрОтсутствует

'    'blnНадоПерезаписать = funПроверкаСоответствия(cn)

'    strСтрока = vbNullString
'    For I = 1 To UBound(arrСоответствие)
'        If arrСоответствие(I).strИмяБазы = conПараметрОтсутствует Then strСтрока = strСтрока & arrСоответствие(I).strИмяРасчета & vbCrLf
'    Next I
'    If strСтрока <> "" Then
'        Msg = "Для следующих параметров расчета:" & vbCrLf & strСтрока & vbCrLf & "нет соответстветствующих измеряемых параметров." & vbCrLf & "Выполните Настройку Параметров КТ."
'        TITLE = "Проверка соответствия" ' по умолчанию
'        MessageBox.Show(Msg, TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning)
'    End If
'End Sub

'функция больше не нужна
'Private Function ВписатьЯчейку(ByRef strПараметр As String, ByRef strFields As String, ByRef dtDataTable As DataTable, ByRef strРазмерность As String) As Double 'As Variant
'    Dim dblЗначение As Double
'    Dim aFindValue(0) As Object
'    Dim drDataRow As DataRow

'    aFindValue(0) = strПараметр
'    drDataRow = dtDataTable.Rows.Find(aFindValue)
'    If Not drDataRow Is Nothing Then
'        'strРазмерность = drDataRow("РазмерностьВыходнаяКТ")
'        dblЗначение = drDataRow(strFields)
'        If dblЗначение = 0 Then dblЗначение = con9999999
'        Return dblЗначение
'    Else
'        Return CDbl(con9999999)
'    End If
'End Function

'Private Function ПроверкаНаПустоеПоле(ByVal mObject As String, ByVal strText As String) As Boolean
'    Dim Msg, TITLE As String
'    If strText = vbNullString Then
'        Msg = "В поле " & mObject & " ни чего не заведено!" & vbCrLf & "Необходимо заполнить все поля."
'        TITLE = "Ввод новых данных"
'        MessageBox.Show(Msg, TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning)
'        Return True
'    Else
'        Return False
'    End If
'End Function

'Private Function ПроверкаНеЦифра(ByVal mObject As String, ByVal strText As String) As Boolean
'    Dim Msg, TITLE As String
'    Dim sngПеревод As Single
'    Dim blnПроверкуНеПрошел As Boolean = False
'    Try
'        sngПеревод = CSng(strText)
'    Catch ex As Exception
'        Msg = "В поле " & mObject & " введена не цифра!"
'        TITLE = "Ввод новых данных"
'        MessageBox.Show(Msg, TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning)
'        blnПроверкуНеПрошел = True
'    End Try
'    Return blnПроверкуНеПрошел
'End Function

'Public Function ПараметрВДиапазоне(ByVal Значение As Double, ByVal Имя As String) As Double
'    If Значение = Double.NaN OrElse Значение = Double.NegativeInfinity OrElse Значение = Double.PositiveInfinity Then
'        Throw New System.Exception(Имя & " вне диапазона")
'        Return 0
'    Else
'        Return Значение
'    End If
'End Function

'Public Function ПараметрВДиапазонеНакопленные(ByVal Значение As Double) As Double
'    If Значение.ToString = Double.NaN.ToString OrElse Значение = Double.NegativeInfinity OrElse Значение = Double.PositiveInfinity Then
'        'Throw New System.Exception(Имя & " вне диапазона")
'        Return 9999999
'    Else
'        Return Значение
'    End If
'End Function


'Public Sub СделатьНачальныйРежим()
'    'проверить зачем он нужен
'    With mФормаРодителя.varfrmGraf
'        'январь
'        'Dim CalculationAsm As System.Reflection.Assembly
'        'CalculationAsm = System.Reflection.Assembly.LoadFrom(AssemblyName)
'        'Dim ClassName As String = System.Configuration.ConfigurationManager.AppSettings("DiagramClassName")
'        'Dim ПеременнаяName As String = System.Configuration.ConfigurationManager.AppSettings("clsСвойстваName")
'        ''создание экземпляра класса
'        '.КлассГрафик = CType(CalculationAsm.CreateInstance(ClassName), CalculationDLLDotNetInterfaces.IClassDiagram)
'        '.clsПеременная = CType(CalculationAsm.CreateInstance(ПеременнаяName), CalculationDLLDotNetInterfaces.IclsСвойства)
'        '.КлассГрафик.ПутьКБазеПараметров = strПутьКТ
'        '.КлассГрафик.СчитатьПараметры()
'        '.КлассГрафик.СчитатьФайлы()
'        '.КлассГрафик.ИменаЭтапов = ИменаЭтапов

'        'для извлечения всех значений контролов
'        'январь
'        '.КлассГрафик.ControlsForPhase = ControlsForPhase
'        blnНачальныйРежимСделан = True
'    End With
'End Sub

'Private Sub ПодсчетПараметров()
'    Dim cmbРежим As String = mФормаРодителя.Manager.ControlsForPhase.Item(ИменаЭтапов(ИменаЭтапов.Length - 1)).Item("Режим").ЗначениеПользователя

'    'вызов метода подсчет
'    If cmbРежим = "Начальный режим" Or cmbРежим = "Конечный режим" Then
'        If Not blnНачальныйРежимСделан Then
'            СделатьНачальныйРежим()
'        End If
'    End If
'End Sub


'Public Sub mnuFileSaveAsText_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFileSaveAsText.Click
'    'B3:H64
'    F1BookПодсчет.SetSelection(2, 2, ТекущаяСтрока, 9)
'    'F1BookПодсчет.CopyAll F1BookПодсчет.SS
'    F1BookПодсчет.EditCopy()
'    '    Clipboard.SetText frmSDI.txtNote.SelText
'    'strСтрокаСправки = Clipboard.GetText
'    'доработка
'    strСтрокаСправки = System.Windows.Forms.Clipboard.GetDataObject.GetData(System.Windows.Forms.DataFormats.Text)
'    F1BookПодсчет.SetSelection(1, 1, 1, 1)
'    mfrmSDI = New frmTextEditor
'    mfrmSDI.Show()
'    mfrmSDI.mnuFileSaveAs() '_Click(mfrmSDI.mnuFileSaveAs, New System.EventArgs)
'    mfrmSDI.Close()
'End Sub
