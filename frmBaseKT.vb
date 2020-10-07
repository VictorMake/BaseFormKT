Imports System.ComponentModel
Imports System.IO
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports TaskClientServerLibrary
Imports TaskClientServerLibrary.Clobal

' Расчетные параметры являются выходом класса(типа DLL) реализующего интерфейс (визуально наследующий форму BaseForm) через КлассГрафик.Manager.РасчетныеПараметры 
' Эти классы типа DLL и база параметров с тем же именем хранятся в специальном каталоге "Ресурсы\МодулиСбораКТ". 
' Программа при старте составляет список имен этих классов
' и сверяет их состав с содержимым в конфигурационном файлом предыдущего запуска
' Присутствующие добавляются в коллекцию, при отсутствии выдается сообщение (просто подключает при совпадении)
' В итоге есть коллекция DLL последней конфигурацией, готовая для расчетов сразу после запуска формы регистратора
' и список DLL всех находящихся в каталоге DLL

' каждая унаследованная DLL (форма) имеет описание Description для чего предназначена форма
' В MDI форме сделать пункт общее меню и 2 подменю с составом DLL
' 1-"конфигуратор расчетных модулей" вначале доступен а после конфигурирования и загрузки регистратора недоступен
' в меню система? доступен до первой загрузки окна регистратор после чего недоступен
' 2- в меню DLL для подключения (вначале недоступен) с составом подключенных после конфигурирования DLL 
' Как только окно Регистратора стартует то 
' После конфигурирования или после первой загрузки регистратора подменю  "конфигуратор расчетных модулей" недоступно 
' эти меню для визуального конфигуратора подключения DLL в список после определения которого 
' заново обновляется конфигурационный файл и заново заполняется коллекция

' 2 подменю заполнено но становится недоступно после нажатия ПУСК т.е. после считывания настроек где производится в цикле по коллекциям проверка соответствия входных каналов входным расчетным параметрам
' если найдены отсутствующие каналы то сообщение и отметить что неполное соответствие и так до конца коллекции
' после Стоп все подменю 2 становятся доступными
' если тест не пройден то запуск отменяется
' DLL имеет свойство видимости - невидима если простой расчет и видима для камер
' при нажатии на подпункт 2 меню если DLL невидима то вывести ее на передний фон и после нажатия ПУСК невидимые DLL скрываются
' Перед закрытием проверка что не записаны fMainFormBase.Manager.blnНадоПерезаписать
' в визуальной форме закладки редактирования невидимы после Запуска, а при остановке видимы - сделать свойство

' При ЗАПУСКЕ в видимой форме закладка становится невидимой, а в невидимая форма скрывается 
' т.е. при ЗАПУСКЕ в цикле по коллекции если DLL видима формы с настройками скрыть, а если DLL невидима скрыть всю DLL
' если При ЗАПУСКЕ  тест пройден все подменю 2 становятся недоступными, т.е. редактирование возможно только при остановке
' если модуль видим его окно на экране и чек в меню помечен
' если не видим и при отметки его в меню он становится видим для редактирования
' 
' Если при проверке DLL была ошибка, то сообщение и на выбор оператору или оставить и отредактировать или исключить
' Если ошибка в расчете то Dll пишет LOG файл(наверно медленная операция) или ALARM прокручиваемый список
' и вывод 999999
' форма окна DLL должна не реагировать кнопку закрытия только свернуть и развернуть
' 
' Не будет ли мигать окно DLL при работе регистратора (не должна т.к. форма не дочерняя)
' 
' Видимая DLL может иметь всё для работы обычного приложения а в режиме снимка она может быть отображена (по какому-то признаку)
' для например просмотра камеры (значит такие DLL должны иметь собственный каталог наверно с таким же именем или путь к этому каталогу хранится в конфигурации DLL)
' 
' Базовая DLL содержит ссылку на математическую библиотеку значит и все наследуемые также содержат ссылку.
' 
' При загрузки из конфигурационного файла или после Конфигуратора DLL 2 варианта
' 1 расчетные параметры добавляются к каналам то базу архивировать не надо
' 2 расчетные параметры удаляются (при модификации списка DLL) из базы архивировать обязательно надо
' При загрузке окна Регистратор вместе с DLL как и раньше проверка последней записи на наличие каналов
' и архивирование без подтверждения пользователя
' так же при загрузке регистратора загружаются формы модулей (они становятся видимы если установлены их соответствующие свойстваи отмечаются в пунктах меню)
' Каждая DLL хранит настроечные данные в базе данных имя которого соответствует имени DLL
' (соответствие входных параметров расчета каналам, массив выходных параметров (расчетных) настройки которых добавляются в базу Channel,
' и коллекция настроечных значение параметр - значение (цифра или логическая )
' т.е. все эти настойки где-то храняться)
' 
' сделать переменную остеживающую модификацию пользователем таблиц и предлагающую запись
' 1 Расчетные параметры - из свойств DLL в коллекции
' 2 Дискретные входные - из дополнительной таблицы в базе

Public Class frmBaseKT
    Friend varMeasurementParameters As FormMeasurementParameters
    Friend varCalculatedParameters As FormCalculatedParameters
    Public varFormGraf As FormGraf
    Friend varFormHierarchicalTable As FormHierarchicalTable
    Public varFormMeasurementKT As FormMeasurementKT
    Friend WithEvents ReaderWriterCommander As ReaderWriterCommand

    Public Sub New()
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'varProjectManager = New ProjectManager
        Me.Manager = New ProjectManager(Me)
    End Sub

    Public Property Manager() As ProjectManager

    Private varDescription As String = "Это Базовая форма для насследования"
    ''' <summary>
    ''' Описание предназначения плагина.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Description() As String
        Get
            Return varDescription
        End Get
        Set(ByVal value As String)
            varDescription = value
        End Set
    End Property

    Private varOwnCatalogue As String = "Это путь собственного каталога"
    Public Overridable Property OwnCatalogue() As String
        Get
            Return varOwnCatalogue
        End Get
        Set(ByVal value As String)
            varOwnCatalogue = value
        End Set
    End Property

    ''' <summary>
    ''' Видима DLL или нет, т.е. имеет ли другие окна или она только вычисляет
    ''' </summary>
    ''' <remarks></remarks>
    Private varIsDLLVisible As Boolean = False
    Public Overridable ReadOnly Property IsDLLVisible() As Boolean
        Get
            Return varIsDLLVisible
        End Get
    End Property

    ''' <summary>
    ''' Пользовательский расчётный класс реализующий интерфейс IClassCalculation.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property ClassCalculation() As IClassCalculation

    ''' <summary>
    ''' Свойство для управления родителем закрытия окон плагина.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsWindowClosed() As Boolean

    ''' <summary>
    ''' Считать из базы настроечные величины. Переопределяется в наследнике.
    ''' Получить Значения Настроечных Параметров
    ''' </summary>
    ''' <remarks></remarks>
    Public Overridable Sub GetValueTuningParameters()
    End Sub

    ''' <summary>
    ''' Загрузить вкладку РасчетныеПараметры
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadFormTuningParameters()
        varCalculatedParameters = New FormCalculatedParameters(Me)
        varCalculatedParameters.Show()
        varCalculatedParameters.ConfigureTableAdapter()
        Manager.CalculatedDataTable = varCalculatedParameters.BaseFormDataSet.РасчетныеПараметры
        'varCalculatedParameters.WindowState = FormWindowState.Maximized
        'varCalculatedParameters.Activate()
    End Sub

    ''' <summary>
    ''' Загрузить вкладку ИзмеренныеПараметры
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadFormCalculatedParameters()
        varMeasurementParameters = New FormMeasurementParameters(Me)
        varMeasurementParameters.Show()
        varMeasurementParameters.ConfigureTableAdapter()
        Manager.MeasurementDataTable = varMeasurementParameters.BaseFormDataSet.ИзмеренныеПараметры
    End Sub

    ''' <summary>
    ''' Загрузить Форму Графика
    ''' </summary>
    Private Sub LoadFormGraf()
        varFormGraf = New FormGraf(Me)
        varFormGraf.Show()
    End Sub

    ''' <summary>
    ''' Загрузить Форму Иерархической Таблицы
    ''' </summary>
    Private Sub LoadFormHierarchicalTable()
        varFormHierarchicalTable = New FormHierarchicalTable(Me)
        varFormHierarchicalTable.Show()
    End Sub

    ''' <summary>
    ''' Загрузить Форму Снятие КТ
    ''' </summary>
    Private Sub LoadFormMeasurementKT()
        varFormMeasurementKT = New FormMeasurementKT(Me)
        varFormMeasurementKT.Show()
    End Sub

    ''' <summary>
    ''' Загрузка производится родителем, иначе может быть ошибка в визуальном конструкторе.
    ''' </summary>
    Public Sub FrmBaseLoad()
        ' обработку ошибок убрать после отладки, чтобы если была ошибка, то форма бы не загружалась
        'Try
        LoadFormCalculatedParameters()
        LoadFormTuningParameters()
        Manager.IsEnabledTuningForms = False ' пока сделал для всех режимов

        If Manager.IsSwohSnapshot Then
            ' чтобы не было возможности что-либо испортить при смене базы
            LoadFormMeasurementKT()
            varFormMeasurementKT.TSMenuItemUnlockSettingPages.Enabled = False
            LoadFormHierarchicalTable()
        Else
            LoadFormGraf()
            LoadFormMeasurementKT()
            LoadFormHierarchicalTable()
            LoadFormClientServer()
        End If

        varFormMeasurementKT.TimerInitializeForm.Enabled = True

        'varProjectManager.ДобавитьКолонкиТаблицыИзмеренные()
        'WindowManagerPanel1.SetActiveWindow(0)
        'СчитатьНастройки()
        'Catch ex As Exception
        'MessageBox.Show(Me, $"Ошибка загрузки базовой формы.{Environment.NewLine}Error: {ex.Message}",
        '                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try
    End Sub

    Private Sub FrmBaseKT_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        If IsWindowClosed Then
            SavePathSettinngXml()
            If Manager.NeedToRewrite Then Manager.SaveTable()
            ReaderWriterCommandCloseAsync()
        Else
            e.Cancel = True
        End If

        varMeasurementParameters.Close()
        varCalculatedParameters.Close()
    End Sub

    ''' <summary>
    ''' Сохранить положение окна в файле настроек
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SavePathSettinngXml()
        Try
            'Dim nsLMZ As XNamespace = "urn:LMZ-org:lmz"
            'Dim nsProgrammers As XNamespace = "urn:LMZ-org:LmzProgrammers"

            ' создать документ
            Dim xmlDocumentSettings As XDocument = New XDocument()

            ' создать xml описание и установить в документе
            'document.Declaration = New XDeclaration("1.0", Nothing, Nothing)

            ' создать Settings element и добавить в документ
            Dim xmlSettings As XElement = New XElement("Settings")
            xmlDocumentSettings.Add(xmlSettings)

            ' создать order инструкцию добавить перед предыдущим элементом
            'Dim pi = New XProcessingInstruction("order", "alpha ascending")
            'Settings.AddBeforeSelf(pi)

            ' создать Location element и добавить в Settings element
            Dim xmlLocation = New XElement("Location")
            xmlSettings.Add(xmlLocation)
            Dim xmlSize = New XElement("Size")
            xmlSettings.Add(xmlSize)

            If WindowState <> FormWindowState.Minimized Then
                ' добавить аттрибуты размерности в Location и Size element 
                xmlLocation.SetAttributeValue("Left", Left)
                xmlLocation.SetAttributeValue("Top", Top)

                xmlSize.SetAttributeValue("Width", Width)
                xmlSize.SetAttributeValue("Height", Height)
                Dim xmlWindowState = New XElement("WindowState", [Enum].GetName(GetType(FormWindowState), WindowState))
                xmlSettings.Add(xmlWindowState)
            Else
                ' добавить аттрибуты размерности в Location и Size element 
                xmlLocation.SetAttributeValue("Left", 0)
                xmlLocation.SetAttributeValue("Top", 0)

                xmlSize.SetAttributeValue("Width", 640)
                xmlSize.SetAttributeValue("Height", 480)
                Dim WindowState = New XElement("WindowState", [Enum].GetName(GetType(FormWindowState), FormWindowState.Normal))
                xmlSettings.Add(WindowState)
            End If

            Dim KeyConfiguration As XElement = New XElement("keyКонфигурацияОтображения", Manager.KeyConfiguration)
            xmlSettings.Add(KeyConfiguration)

            Dim Description As XElement = New XElement("Description", Me.Description)
            xmlSettings.Add(Description)

            '' создать namespace declaration xmlns:a и добавить в Size элемент 
            'Dim nsdecl = New XAttribute(XNamespace.Xmlns + "a", nsProgrammers)
            'Size.Add(nsdecl)

            ' создать элемент под текстовым узлом
            'xmlSize.SetElementValue(nsProgrammers + "programmer", "Michelangelo")

            'Dim programmer = New XElement(nsProgrammers + "programmer", "Leonardo ", "da ", "Vinci")
            'Size.AddFirst(programmer)

            'programmer = New XElement(nsProgrammers + "programmer")
            'Size.Add(programmer)
            'Dim cdata = New XText("Donatello")
            'programmer.Add(cdata)

            '' создать комментарий и добавить элемент
            'Dim comment = New XComment("вставить Size здесь")
            'Settings.Add(comment)

            xmlDocumentSettings.Save(Manager.PathSettingXml)
        Catch ex As Exception
            MessageBox.Show(Me,
                            String.Format("Невозможно сохранить настройки в конфигурационном файле.{0}Error: {1}", Environment.NewLine, ex.Message),
                            Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

#Region "ReaderWriterCommand"
    ''' <summary>
    ''' Загрузить Форму Клиент Сервер
    ''' </summary>
    Private Sub LoadFormClientServer()
        Dim pathOptions As String = Path.Combine(Manager.PathResource, "Опции.ini")
        Dim standNumber As String = GetIni(pathOptions, "Stend", "Stend", "25")
        ' рабочие Каталоги Сервера и Клиента
        Dim serverWorkingFolder As String = GetIni(pathOptions, "Server", "Stend" & standNumber, "\\Stend_25\D\Registration\Store\Channels.mdb")
        serverWorkingFolder = Mid(serverWorkingFolder, 1, InStrRev(serverWorkingFolder, "Channels") - 1)
        Dim clientWorkingFolder As String = GetIni(pathOptions, "Client", "StendКлиент", "\\Stend_25\D\Registration\Store\Channels.mdb")
        clientWorkingFolder = Mid(clientWorkingFolder, 1, InStrRev(clientWorkingFolder, "Channels") - 1)

        ' Клиент всегда под номером 2
        ReaderWriterCommander = New ReaderWriterCommand(Me, Manager.PathResource, False, 2, serverWorkingFolder, clientWorkingFolder, AddressOf AppendOutput)
        ReaderWriterCommander.ShowFormCommand()
        ReaderWriterCommander.FormCommander.MdiParent = Me
        AddHandler ReaderWriterCommander.FormCommander.FormClosing, AddressOf FormCommand_Closing
    End Sub

    Private Sub FormCommand_Closing(sender As Object, e As CancelEventArgs)
        If Not Me.IsWindowClosed Then
            e.Cancel = True
        Else
            ReaderWriterCommander.FormCommander.MdiParent = Nothing
        End If
    End Sub

    Private Async Sub ReaderWriterCommandCloseAsync()
        If ReaderWriterCommander IsNot Nothing Then
            ReaderWriterCommander.Close()
            Await Task.Delay(1000)
            ReaderWriterCommander = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Logging командного обмена между компьютерами
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="RichTextBoxKey"></param>
    ''' <param name="selectionModeIcon"></param>
    Private Sub AppendOutput(message As String, RichTextBoxKey As Integer, selectionModeIcon As MessageBoxIcon)
        ' TODO: в случае небходимости доделать
        'Dim tempRichTextBox As RichTextBox = RichTextBoxClient 'richTextBoxDictionary(RichTextBoxKey)

        'If tempRichTextBox.TextLength > 0 Then
        '    tempRichTextBox.AppendText(ControlChars.NewLine)
        'End If

        'tempRichTextBox.AppendText(String.Format("{0} {1}", DateTime.Now.ToLongTimeString, message))
        'WriteTextToRichTextBox(tempRichTextBox, message, selectionModeIcon)
        'tempRichTextBox.ScrollToCaret()
    End Sub

    ''' <summary>
    ''' Чтение данных из файла INI - с возможностью записи значения по умолчанию где аргументы:
    ''' </summary>
    ''' <param name="sINIFile"></param>
    ''' <param name="sSection">sSection  = Название раздела</param>
    ''' <param name="sKey">sKey  = Название параметра</param>
    ''' <param name="sDefault">sDefault = Значение по умолчанию (на случай его отсутствия)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetIni(ByRef sINIFile As String, ByRef sSection As String, ByRef sKey As String, Optional ByRef sDefault As String = "") As String
        ' Значение возвращаемое функцией GetPrivateProfileString если искомое значение параметра не найдено
        Const NO_VALUE As String = ""
        Dim nLenght As Short ' Длина возвращаемой строки (функцией GetPrivateProfileString)
        Dim sTemp As String ' Возвращаемая строка

        Try
            ' Получаем значение из файла - если его нет будет возвращен 4й аргумент = strNoValue
            ' sTemp.Value = Space(256)
            sTemp = New String(Chr(0), 255)
            nLenght = CShort(GetPrivateProfileString(sSection, sKey, NO_VALUE, sTemp, 255, sINIFile))
            sTemp = Strings.Left(sTemp, nLenght)

            '' Определяем было найдено значение или нет (если возвращено знач. константы strNoValue то = НЕТ)
            'If sTemp = NO_VALUE Then ' Значение не было найдено
            '    If sDefault <> "" Then ' Если знач по умолчанию задано
            '        WriteINI(sINIFile, sSection, sKey, sDefault) ' Записываем заданное аргументом sDefault значение по умолчанию
            '        sTemp = sDefault ' и возвращаем его же
            '    End If
            'End If

            ' Возвращаем найденное
            Return sTemp
        Catch ex As ApplicationException
            Const CAPTION As String = "Ощибка чтения INI"
            Dim text As String = String.Format("Функция sGetIni привела к ошибке:{0}{1}", vbCrLf, ex.ToString)
            MessageBox.Show(text, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error)
            'RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED(String.Format("<{0}> {1}", CAPTION, text))
            Return ""
        End Try
    End Function

    ' для записи в Опции.INI
    Friend Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String,
                                                                                                     ByVal lpKeyName As String,
                                                                                                     ByVal lpDefault As String,
                                                                                                     ByVal lpReturnedString As String,
                                                                                                     ByVal nSize As Integer,
                                                                                                     ByVal lpFileName As String) As Integer

#Region "Скажи_текущее_время"
    ''' <summary>
    ''' Входящая задача с ответом подтверждения.
    ''' Тестовый метод для проверки прохождения команд.
    ''' </summary>
    ''' <param name="parameters"></param>
    Public Sub Скажи_текущее_время(ByVal ParamArray parameters() As String)
        Dim inHostName As String = parameters(0)
        Dim indexResponse As String = parameters(1)

        ' отправится из очереди ReaderWriterCommandClassReaderWriterCommand.SendRequestProgrammed("Установи текущее время", Parameters)
        ' varConfigPidAndChannelstargetForm.gTabPageCollection.Item("Идетификатор компьютера").ПослатьЗапросПрограммно("Установи текущее время", Parameters)
        ' передать Исходящую задачу- в ответе индекс передать тот же
        ReaderWriterCommander.ManagerAllTargets.Targets(inHostName).CommandWriterQueue.Enqueue(New NetCommandForTask(УстановиТекущееВремя, {Date.Now.ToLongTimeString}) With {.IsResponse = True, .IndexResponse = indexResponse})
    End Sub

    ''' <summary>
    ''' Исходящая задача.
    ''' Тестовый метод для проверки прохождения команд.
    ''' </summary>
    ''' <param name="TimeNow"></param>
    Public Sub Установи_текущее_время(TimeNow As DateTime, ByVal ParamArray parameters() As String)
        ' чего-то делаем
        'MessageBox.Show("Текущее время: " & TimeNow.ToShortTimeString, УстановиТекущееВремя, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region

#Region "Set_Polynomial_Channel"
    ''' <summary>
    ''' Входящая задача с ответом подтверждения.
    ''' Обновить коэффициенты полинома заданного канала.
    ''' </summary>
    ''' <param name="НомерКанала"></param>
    ''' <param name="K0"></param>
    ''' <param name="K1"></param>
    ''' <param name="K2"></param>
    ''' <param name="K3"></param>
    ''' <param name="K4"></param>
    ''' <param name="K5"></param>
    ''' <param name="parameters"></param>
    Public Sub Set_Polynomial_Channel(НомерКанала As Integer,
                                      K0 As Double,
                                      K1 As Double,
                                      K2 As Double,
                                      K3 As Double,
                                      K4 As Double,
                                      K5 As Double,
                                      ByVal ParamArray parameters() As String)
        Dim inHostName As String = parameters(0)
        Dim indexResponse As String = parameters(1)
        ' чего-то делаем
        ' передать Исходящую задачу- в ответе индекс передать тот же
        ReaderWriterCommander.ManagerAllTargets.Targets(inHostName).CommandWriterQueue.Enqueue(New NetCommandForTask(OkSetPolynomialChannel, {"обновление коэффициенов полинома заданного канала произведен"}) With {.IsResponse = True, .IndexResponse = indexResponse})
    End Sub

    ''' <summary>
    ''' Ответ - обновление коэффициенов полинома заданного канала произведено.
    ''' Исходящая задача.
    ''' </summary>
    ''' <param name="textMessage"></param>
    ''' <param name="parameters"></param>
    Public Sub Ok_Set_Polynomial_Channel(ByVal textMessage As String, ByVal ParamArray parameters() As String)
        ' чего-то делаем
        'MessageBox.Show(textMessage, $"Подтверждение от {parameters(0)}", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region

#Region "Поставить_метку_КТ"
    ''' <summary>
    ''' Входящая задача с ответом подтверждения.
    ''' </summary>
    ''' <param name="captionKT"></param>
    ''' <param name="parameters"></param>
    Public Sub Поставить_метку_КТ(captionKT As String, ByVal ParamArray parameters() As String)
        Dim inHostName As String = parameters(0)
        Dim indexResponse As String = parameters(1)
        ' чего-то делаем
        ' передать Исходящую задачу- в ответе индекс передать тот же
        ReaderWriterCommander.ManagerAllTargets.Targets(inHostName).CommandWriterQueue.Enqueue(New NetCommandForTask(ОтветПоставитьМеткуКТ, {"Отметка КТ произведена"}) With {.IsResponse = True, .IndexResponse = indexResponse})
    End Sub

    ''' <summary>
    ''' Ответ.
    ''' Исходящая задача.
    ''' </summary>
    ''' <param name="textMessage"></param>
    ''' <param name="parameters"></param>
    Public Sub Ответ_Поставить_метку_КТ(ByVal textMessage As String, ByVal ParamArray parameters() As String)
        ' чего-то делаем
        'MessageBox.Show(textMessage, $"Подтверждение от {parameters(0)}", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region

#Region "Stop_Client"
    ''' <summary>
    ''' Входящая задача с ответом подтверждения.
    ''' </summary>
    ''' <param name="parameters"></param>
    Public Sub Stop_Client(ByVal ParamArray parameters() As String)
        Dim inHostName As String = parameters(0)
        Dim indexResponse As String = parameters(1)
        ' чего-то делаем
        ' передать Исходящую задачу- в ответе индекс передать тот же
        ReaderWriterCommander.ManagerAllTargets.Targets(inHostName).CommandWriterQueue.Enqueue(New NetCommandForTask(OkStopClient, {$"Остановка клиента {inHostName} произведена"}) With {.IsResponse = True, .IndexResponse = indexResponse})
    End Sub
    ''' <summary>
    ''' Ответ.
    ''' Исходящая задача.
    ''' </summary>
    ''' <param name="textMessage"></param>
    ''' <param name="parameters"></param>
    Public Sub Ok_Stop_Client(ByVal textMessage As String, ByVal ParamArray parameters() As String)
        ' чего-то делаем
        'MessageBox.Show(textMessage, $"Подтверждение от {parameters(0)}", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region

    ''' <summary>
    ''' не удалять, вызывается косвенно
    ''' </summary>
    Public Sub Очистка_линии(ByVal ParamArray parameters() As String)
        ' ни чего не делаем
    End Sub

#Region "Send_Message"
    ''' <summary>
    ''' Тестовый метод для проверки прохождения команд.
    ''' </summary>
    ''' <param name="textMessage"></param>
    Public Sub Send_Message(ByVal textMessage As String, ByVal ParamArray parameters() As String)
        Dim inHostName As String = parameters(0)
        Dim indexResponse As String = parameters(1)

        ' чего-то делаем
        MessageBox.Show(textMessage, $"Пришло сообщение от {parameters(0)}", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ' послать что принято к сведению
        ReaderWriterCommander.ManagerAllTargets.Targets(inHostName).CommandWriterQueue.Enqueue(New NetCommandForTask(OKSendMessage, {$"Сообщение от {parameters(0)} принято к исполнению."}) With {.IsResponse = True, .IndexResponse = indexResponse})
    End Sub
    ''' <summary>
    ''' Ответ.
    ''' Исходящая задача.
    ''' </summary>
    ''' <param name="textMessage"></param>
    ''' <param name="parameters"></param>
    Public Sub Ok_Send_Message(ByVal textMessage As String, ByVal ParamArray parameters() As String)
        ' послать что клиент принял к сведению
        MessageBox.Show(textMessage, $"Подтверждение от {parameters(0)}", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region
#End Region

End Class


'Private Sub lblDescription_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles lblDescription.Paint
'    Dim g As Graphics = e.Graphics
'    Dim rect As New Rectangle(sender.Left, sender.Top, sender.Width, sender.Height)
'    'Dim brush As New Drawing2D.LinearGradientBrush(rect, color1, color2, angle)
'    Dim brush As New Drawing2D.LinearGradientBrush(rect, Color.DarkRed, Color.Gold, Drawing2D.LinearGradientMode.Vertical)

'    g.FillRectangle(brush, rect)
'End Sub

'Private Sub СчитатьНастройки()
'Try
'    ' Перекрываем ненужные исключения...
'    ' Считываем расположение и размеры главной формы.
'    Using rk As RegistryKey = Registry.CurrentUser
'        Using rkAscSearch As RegistryKey = rk.CreateSubKey("Software\OPTIM.RU\AscSearch")
'            Top = CInt(rkAscSearch.GetValue("Top"))
'            Left = CInt(rkAscSearch.GetValue("Left"))
'            Height = CInt(rkAscSearch.GetValue("Height"))
'            Width = CInt(rkAscSearch.GetValue("Width"))
'            WindowState = CType(rkAscSearch.GetValue("WindowState"), FormWindowState)
'            splHorPrivew.SplitPosition = CInt(rkAscSearch.GetValue("splHorPrivew.SplitPosition"))
'            splVerPrivew.SplitPosition = CInt(rkAscSearch.GetValue("splVerPrivew.SplitPosition"))
'            splHorSetings.SplitPosition = CInt(rkAscSearch.GetValue("splHorSetings.SplitPosition"))
'        End Using
'    End Using
'    ' Считываем список ранее открывавшихся файлов.
'    RecentPersistInRegLoad()
'Catch
'End Try
'' Перекрываем ненужные исключения...
'OpenConfig(m_sCurrentConfig)
'Visible = True

'    Me.Left = CSng(GetSetting(System.Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "ViewTextLeft", CStr(0)))
'    Me.Top = CSng(GetSetting(System.Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "ViewTextTop", CStr(0)))
'    Me.Width = CSng(GetSetting(System.Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "ViewTextWidth", CStr(640)))
'    Me.Height = CSng(GetSetting(System.Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "ViewTextHeight", CStr(480)))
''frmТекстовыйКонтроль_Resize(Me, New System.EventArgs)

'End Sub


'Public Sub Init(ByVal sShrFileName As String, ByVal sPath As String)
'    Visible = False

'    If sShrFileName IsNot Nothing AndAlso sShrFileName.Length > 0 Then
'        m_sCurrentConfig = sShrFileName
'        'Else
'        '    ' Если строка не задана в качестве параметра, считываем ее из рееста.
'        '    Try
'        '        ' Перекрываем ненужные исключения...
'        '        Using rk As RegistryKey = Registry.CurrentUser
'        '            Using rkAscSearch As RegistryKey = rk.CreateSubKey("Software\OPTIM.RU\AscSearch")
'        '                m_sCurrentConfig = DirectCast(rkAscSearch.GetValue("m_sCurrentConfig"), String)
'        '            End Using
'        '        End Using
'        '    Catch
'        '        ' Перекрываем ненужные исключения...
'        '    End Try
'    End If
'    OpenConfig(m_sCurrentConfig)
'    'If sPath IsNot Nothing AndAlso sPath.Length > 0 Then
'    '    tbPath.Text = sPath
'    'End If
'End Sub

'''' <summary>
'''' Открывает XML-файл настроек.
'''' </summary>
'''' <param name="sName">Имя XML-файла. Null или пустая строка
'''' приведет к созданию нового документа.</param>
'Private Sub OpenConfig(ByVal sName As String)


'    If sName Is Nothing OrElse sName.Length = 0 Then
'        SetDirty(False)
'        FileNew()
'        Return
'    End If
'    m_bLoading = True
'    Try
'        Dim doc As New XmlDocument()
'        doc.Load(sName)
'        Dim d As XmlElement = doc.DocumentElement

'        ' Перебераем элементы управления и считываем для каждого из них
'        ' значение из конфигурационного файла.
'        For Each ctrl As Control In GetStatefullCtrls(tpSetings.Controls)
'            ' Каждый контрол имеет двухбуквенный префикс (cb или tb)
'            ' Поэтому имя тега можно получить отбросом первый двух
'            ' символов имени контрола.
'            Dim Name As String = ctrl.Name.Remove(0, 2)
'            If TypeOf ctrl Is CheckBox Then
'                ' Для преобразования строки в bool нужно воспользоваться bool.Parse()
'                Try
'                    TryCast(ctrl, CheckBox).Checked = Boolean.Parse(d.SelectSingleNode(Name).InnerText)
'                Catch
'                End Try
'            ElseIf TypeOf ctrl Is TextBox Then
'                ' операции чтения взяты в try{ } catch{}, так как если значений нет 
'                ' в xml не нужно выдвать сообщений об ошибке, ведь это скорее всего новый элемент.
'                Try
'                    TryCast(ctrl, TextBox).Text = d.SelectSingleNode(Name).InnerText
'                Catch
'                End Try
'            Else
'                Throw New Exception(String.Format("Unknown control type.  Name={0}, Info={1}", ctrl.Name, ctrl.ToString()))
'            End If
'        Next
'        Try
'            m_sCurrentTvPath = d.SelectSingleNode("m_sCurrentTvPath").InnerText
'        Catch
'        End Try
'        m_sCurrentConfig = sName
'        ' Запоминаем имя конфигурационного файла
'        ' Обновляем список открывавшихся файлов
'        RecentAdd(sName)
'    Catch ex As Exception
'        Throw New Exception("Can not open the configuration file""" & sName & """" & vbLf & vbLf & "Error: " & ex.Message)
'    Finally
'        m_bLoading = False
'    End Try
'    ' Если открытие файла настроек прошла удачно...
'    SetDirty(False)
'    ' ...помечаем состояние как "записанное".
'End Sub


'Private Sub СчитатьНастройки()
'    Try

'        ' use Attribute property as well as Elements property to retrive first attribute named 'name' 
'        ' inside element named 'period' within an XDocument object
'        'Dim result = document.<art>.<period>.@name
'        'Console.WriteLine("'name' Attribute: " & result)

'        ' use Attribute property as well as Descendants property to retrive first attribute named 'name'
'        ' inside element named 'period' within an XDocument object
'        'Dim result = document.<Settings>.<Location>.@Width
'        'Dim result = document...<Location>.@Width

'        'Console.WriteLine("'Width' Attribute: " & result)

'        'result = document...<artist>.Value
'        'Console.WriteLine("artist: " & result)


'        ' use Attribute property as well as Descendant property to retrive first attribute named 'name'
'        ' inside element named 'period' within an XElement object
'        'Dim periodElement = document...<period>(0)
'        'result = periodElement.@name
'        'Console.WriteLine("'name' Attribute: " & result)

'        Dim DocumentSettings = New XDocument
'        DocumentSettings = XDocument.Load(varProjectManager.ПутьКНастойкам)

'        Me.Left = CInt(DocumentSettings...<Location>.@Left)
'        Me.Top = CInt(DocumentSettings...<Location>.@Top)

'        Me.Width = CInt(DocumentSettings...<Size>.@Width)
'        Me.Height = CInt(DocumentSettings...<Size>.@Height)

'        'Dim name As String = _
'        '    System.Enum.GetName(GetType(System.Windows.Forms.FormWindowState), System.Windows.Forms.FormWindowState.Normal)

'        Dim strWindowState As String = DocumentSettings...<WindowState>.Value

'        'Dim values As Array = FormWindowState.GetValues(GetType(FormWindowState))
'        Dim values As Array = System.Enum.GetValues(GetType(FormWindowState))
'        Dim i As Integer
'        'по умолчанию
'        Dim valueTemp As FormWindowState = FormWindowState.Normal
'        For i = 0 To values.Length - 1
'            If values.GetValue(i).ToString = strWindowState Then
'                valueTemp = values.GetValue(i)
'                Exit For
'            End If
'        Next i

'        Me.WindowState = CType(valueTemp, FormWindowState)
'    Catch ex As Exception
'        MessageBox.Show(Me, "Ошибка в процедуре СчитатьНастройки." & vbCr & vbLf & "Error: " & ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
'    End Try

'End Sub

'''' <summary>
'''' Предлагает пользователю ввести имя файла в диалоге "Save As"
'''' и производит попытку записи в выбранный файл.
'''' </summary>
'Private Sub SaveConfigAs()
'    ' Если пользователь подтвердил переименование файла (нажал Save)...
'    'If DialogResult.OK = SaveFile.ShowDialog(Me) Then
'    Try
'        ' Пытаемся записаться...
'        'SaveConfig(SaveFile.FileName)
'        '' Если удается, запоминаем имя открывтого файла...
'        'm_sCurrentConfig = SaveFile.FileName
'        '' и добавляем путь к Recent-списку.
'        'RecentAdd(m_sCurrentConfig)
'        SaveConfig()
'    Catch e As Exception
'        MessageBox.Show(Me, "Невозможно сохранить настройки в конфигурационном файле." & vbCr & vbLf & "Error: " & e.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.[Error])
'    End Try
'    'End If
'End Sub

'Теперь нам нужно каким-то образом вызвать метод SaveConfig, но этому методу требуется передать имя открываемого файла. 
'В коде уже фигурировала переменная m_sCurrentConfig. Это строковая переменная, областью видимости которой является экземпляр формы. 
'В ней хранится путь к файлу настроек, который открыт в данный момент. Поэтому для записи достаточно вызвать SaveConfig(m_sCurrentConfig);. 
'Однако не все так просто. В первый раз путь к файлу будет пустым. К тому же может случиться, что по каким-то причинам файл не может быть записан (например, файл или каталог защищен от записи). 
'При этом неминуемо будет возбуждено исключение и, если его не перехватить, программа, повозмущавшись, закроется. 
'Можно конечно взять вызов функции в Try/Catch-блок и выдать сообщение пользователю, объяснив попутно, мол, извини, дорогой, так получилось... не обессудь. 
'Но вряд ли от такой программы будет прок, разве что поиздеваться над кем-нибудь. Правильным выходом будет в случае неудачи предложить пользователю записать файл под другим именем, выдав диалог «Save As». 
'К тому же диалог «Save As» сам по себе должен присутствовать в приличной программе.
'Еще одно соображение заключается в том, что реализовывать и простую запись, и «Save As» лучше не в обработчиках событий, а в отдельных методах. 
'Во-первых, интерфейсный код может вызываться из нескольких мест. В нашем случае из: меню, тулбара и командной строки. 
'А, во - вторых, к этому призывают принципы грамотного программирования. В конце концов, это отдельный функционально законченный кусок кода. 
'Сначала делается попытка записать файл под именем, указанным в переменной m_sCurrentConfig, и, если это не удается, вызывается метод, 
'предлагающий пользователю сохранить файл под другим именем.

'Private Sub SaveCurrentConfig()
'    Try
'        SaveConfig(m_sCurrentConfig)
'    Catch
'        SaveConfigAs()
'    End Try
'End Sub


''После записи настроек из описанных мною элементов управления происходит запись пути в дереве каталогов с закладки Preview. 
''О нем речь пойдет ниже. Сейчас скажу только, что свойство FullPath позволяет получить путь к выделенной ветке в виде строки.
'''' <summary>
'''' Записывает настройки приложения в XML-файл.
'''' sName: Имя файла.
'''' </summary>
'''' <param name="sName">Имя XML-файла.</param>
'Private Sub SaveConfig(ByVal sName As String)
'    Dim doc As New XmlDocument()
'    Dim root As XmlNode = doc.CreateNode(XmlNodeType.Element, "shr", "")
'    'XmlNode cdata = doc.CreateNode(XmlNodeType.CDATA, "", "");
'    doc.AppendChild(root)

'    ' Перебераем элементы управления и записываем значение каждого 
'    ' в конфигурационный файл. Комментарии см. в ф-и OpenConfig.
'    For Each ctrl As Control In GetStatefullCtrls(tpSetings.Controls)
'        If TypeOf ctrl Is CheckBox Then
'            SaveToXmlCheckBox(doc, TryCast(ctrl, CheckBox))
'        ElseIf TypeOf ctrl Is TextBox Then
'            SaveToXmlControlText(doc, ctrl)
'        Else
'            Throw New Exception(String.Format("Unknown control type.  Name={0}, Info={1}", ctrl.Name, ctrl.ToString()))
'        End If
'    Next

'    ' Сохраняем выделенную ветку в Preview-дереве.
'    If tvFiles.SelectedNode IsNot Nothing Then
'        Dim Elem As XmlNode = doc.CreateNode(XmlNodeType.Element, "m_sCurrentTvPath", "")
'        Dim ElemTxt As XmlNode = doc.CreateNode(XmlNodeType.Text, "", "")
'        ElemTxt.Value = tvFiles.SelectedNode.FullPath
'        Elem.AppendChild(ElemTxt)
'        root.AppendChild(Elem)
'    End If

'    doc.Save(sName)
'    SetDirty(False)
'End Sub

'Этот метод, во-первых, устанавливает переменную формы m_bIsDirty, говорящую, что конфигурационные данные изменены и их требуется сохранить (при m_bIsDirty = TRUE),
'или не требуется сохранять (при m_bIsDirty = False). Во-вторых, он формирует заголовок окна, используя непереводимый C-шный фольклор. :)
'Подробности можно узнать из комментариев.
'Переменная-флаг m_bLoading поднимается при загрузке файла с настройками. Пока загружается файл, поведение программы несколько отличается от обычного. 
'В данном случае при загрузке не нужно модифицировать переменную m_bIsDirty и дописывать звездочку в заголовок.

'Private Sub SetDirty(ByVal bIsDirty As Boolean)
'    If Not m_bLoading Then
'        m_bIsDirty = bIsDirty
'    End If
'    Text = csAppName & " - " & (If((m_sCurrentConfig IsNot Nothing AndAlso m_sCurrentConfig.Length > 0), m_sCurrentConfig, "<New search>")) & (If(m_bIsDirty AndAlso Not m_bLoading, " *", ""))
'End Sub
