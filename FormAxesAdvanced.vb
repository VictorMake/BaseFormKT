Imports System.Windows.Forms
Imports System.Data.OleDb
Imports System.Threading
Imports System.Drawing
Imports MathematicalLibrary

Public Class FormAxesAdvanced
    Private connection As OleDbConnection
    Private mParrentForm As FormGraf
    Private countAxes As Integer
    Private countPlots As Integer

    Const cMaxAxes As Integer = 6 ' огранмчение добавляемых осей
    Private constShade As Color = Color.DarkGray
    Private constFace As Color = SystemColors.Control

    Private FrequencyGraph As Single()
    Private isNeedToSave As Boolean ' надо Записать
    Private isProcessDeletePlot As Boolean ' идет Удаление Шлейфа
    Private isNeedToUpdateCaption As Boolean ' надо Обновлять Надписи

#Region "Form"
    Sub New(ByVal inFormParrent As FormGraf)
        MyBase.New()
        ' This is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'Me.FormParrent = inFormParrent
        mParrentForm = inFormParrent
    End Sub

    'Private WriteOnly Property ParrentForm() As FormMain
    '    Set(ByVal Value As FormMain)
    '        mParrentForm = Value
    '    End Set
    'End Property

    Private Sub FormAxesAdvanced_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        InitializeForm()
        mParrentForm.TSButtonTuneTrand.Enabled = False
        mParrentForm.TSButtonBounds.Enabled = False
    End Sub

    Private Sub AxesAdvanced_Closed(ByVal eventSender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        If isNeedToSave Then SaveConfiguration()
        mParrentForm.FormParrent.SavePathSettinngXml() 'для сохранения lngkeyКонфигурация
        mParrentForm.TSButtonTuneTrand.Enabled = True
        mParrentForm.TSButtonBounds.Enabled = True
        mParrentForm.TSButtonTuneTrand.Checked = False
        mParrentForm = Nothing
    End Sub

    Private Sub ButtonClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonClose.Click
        Me.Close()
    End Sub

    Public Sub InitializeForm()
        countAxes = mParrentForm.ScatterGraphParameter.YAxes.Count
        SlideSelectAxis.Value = 0
        LabelAxisColorNext.BackColor = ColorsNet((countAxes) Mod 7)
        LabelGrafColorNext.BackColor = ColorsNet(0)

        For Each rowРасчетныйПараметр As BaseFormDataSet.РасчетныеПараметрыRow In mParrentForm.FormParrent.Manager.CalculatedDataTable.Rows
            ComboBoxParametersGraph.Items.Add(rowРасчетныйПараметр.ИмяПараметра)
            ComboBoxParametersAxis.Items.Add(rowРасчетныйПараметр.ИмяПараметра)
        Next

        For Each rowИзмеренныйПараметр As BaseFormDataSet.ИзмеренныеПараметрыRow In mParrentForm.FormParrent.Manager.MeasurementDataTable.Rows
            ComboBoxParametersGraph.Items.Add(rowИзмеренныйПараметр.ИмяПараметра)
            ComboBoxParametersAxis.Items.Add(rowИзмеренныйПараметр.ИмяПараметра)
        Next

        ComboBoxParametersAxis.SelectedIndex = 0
        ComboBoxParametersGraph.SelectedIndex = 0
        SetFrequencyGraph(mParrentForm.FormParrent.Manager.FrequencyBackground)

        For I = 0 To UBound(FrequencyGraph)
            ComboBoxFrequency.Items.Add(CStr(FrequencyGraph(I)))
        Next

        ComboBoxTimeTail.Items.Add("10")
        ComboBoxTimeTail.Items.Add("20")
        ComboBoxTimeTail.Items.Add("30")
        ComboBoxTimeTail.Items.Add("60")

        ComboBoxFrequency.SelectedIndex = 0
        ComboBoxTimeTail.SelectedIndex = 0

        ' здесь восстанавливаются все настройки если такие параметры есть
        LoadConfigurationFromDBase()
        RestoreConfiguration($"keyКонфигурацияОтображения)= {mParrentForm.FormParrent.Manager.KeyConfiguration}))")
    End Sub

    ''' <summary>
    ''' Загрузить Конфигурации Графиков
    ''' </summary>
    Private Sub LoadConfigurationFromDBase()
        Dim indexRow, lastSelectedIndex As Integer
        Dim cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mParrentForm.FormParrent.Manager.PathKT))
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow

        ' Открыть подключение
        Const strSQL As String = "SELECT DISTINCTROW КонфигурацииОтображения.keyКонфигурацияОтображения, КонфигурацииОтображения.ИмяКонфигурации " &
        "FROM(КонфигурацииОтображения) " &
        "ORDER BY КонфигурацииОтображения.keyКонфигурацияОтображения;"

        cn.Open()
        odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
        odaDataAdapter.Fill(dtDataTable)
        cn.Close()

        If dtDataTable.Rows.Count <> 0 Then
            indexRow = 0

            For Each drDataRow In dtDataTable.Rows
                ComboBoxListConfigurations.Items.Add(drDataRow("ИмяКонфигурации"))

                If CInt(drDataRow("keyКонфигурацияОтображения")) = mParrentForm.FormParrent.Manager.KeyConfiguration Then
                    lastSelectedIndex = indexRow
                End If

                indexRow += 1
            Next
        End If

        ComboBoxListConfigurations.Focus()
        ComboBoxListConfigurations.SelectedIndex = lastSelectedIndex
    End Sub

    ''' <summary>
    ''' Восстановить конфигурацию
    ''' </summary>
    ''' <param name="whereCondition"></param>
    Private Sub RestoreConfiguration(ByRef whereCondition As String)
        Dim cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mParrentForm.FormParrent.Manager.PathKT))
        Dim rdr As OleDbDataReader
        Dim cmd As OleDbCommand = cn.CreateCommand
        cmd.CommandType = CommandType.Text
        Dim strSQL As String
        Dim I As Integer
        Dim success As Boolean

        isProcessDeletePlot = True
        SlidePlots.CustomDivisions.Clear()
        mParrentForm.ScatterGraphParameter.Annotations.Clear()
        isProcessDeletePlot = False
        mParrentForm.ScatterGraphParameter.Plots.Clear()

        ' удалить оси оставить 1 последние
        For I = mParrentForm.ScatterGraphParameter.YAxes.Count - 1 To 1 Step -1
            mParrentForm.ScatterGraphParameter.YAxes.RemoveAt(I)
        Next

        mParrentForm.ScatterGraphParameter.XAxes(0).Caption = ""
        ' по умолчанию
        SlideSelectAxis.Value = 0
        SlideSelectAxis.Range = New Range(0, 1)
        SlideAssignAxis.Range = New Range(0, 1)
        ButtonAddAxis.Enabled = True

        ' на этом этапе все очищено
        ' Открыть подключение
        strSQL = "SELECT КонфигурацииОтображения.* FROM КонфигурацииОтображения WHERE (((КонфигурацииОтображения." & whereCondition
        cn.Open()
        cmd.CommandText = strSQL
        ' Создание recordset
        rdr = cmd.ExecuteReader

        If rdr.Read Then
            mParrentForm.FormParrent.Manager.KeyConfiguration = CInt(rdr("keyКонфигурацияОтображения"))
            RadioButtonTypeAxisX.Checked = CBool(rdr("ВремяИлиПараметр"))
            RadioButtonTypeAxisY.Checked = Not RadioButtonTypeAxisX.Checked
            success = False

            For I = 0 To ComboBoxFrequency.Items.Count - 1
                'проверка на существование
                If ComboBoxFrequency.Items(I).ToString = CStr(rdr("ЧастотаПостроения")) Then
                    success = True
                    Exit For
                End If
            Next

            If success Then
                ComboBoxFrequency.SelectedIndex = I
            Else
                ComboBoxFrequency.SelectedIndex = 0
            End If

            SetCounterLightByFrequencyBackground()

            success = False
            For I = 0 To ComboBoxTimeTail.Items.Count - 1
                ' проверка на существование
                If ComboBoxTimeTail.Items(I).ToString = CStr(rdr("ВремяСвечения")) Then
                    success = True
                    Exit For
                End If
            Next

            If success Then
                ComboBoxTimeTail.SelectedIndex = I
            Else
                ComboBoxTimeTail.SelectedIndex = 0
            End If

            SetCounterLightByUser()
            success = False

            For I = 0 To ComboBoxParametersAxis.Items.Count - 1
                ' проверка на существование
                If ComboBoxParametersAxis.Items(I).ToString = CStr(rdr("ИмяПараметраОсиХ")) Then
                    success = True
                    Exit For
                End If
            Next

            If success Then
                ComboBoxParametersAxis.SelectedIndex = I
                NumericEditParamMin.Value = CDbl(rdr("МинОсь"))
                NumericEditParamMax.Value = CDbl(rdr("МахОсь"))
            Else
                ComboBoxParametersAxis.SelectedIndex = 0
                NumericEditParamMin.Value = 0
                NumericEditParamMax.Value = 100
            End If
        End If

        rdr.Close()
        strSQL = "SELECT КонфигурацииОтображения.keyКонфигурацияОтображения, КонфигурацииОтображения.ИмяКонфигурации, Ось.* " &
            "FROM КонфигурацииОтображения RIGHT JOIN Ось ON КонфигурацииОтображения.keyКонфигурацияОтображения = Ось.keyКонфигурацияОтображения " &
            "WHERE (((КонфигурацииОтображения." & whereCondition &
            " ORDER BY Ось.НомерОси;"

        cmd.CommandText = strSQL
        rdr = cmd.ExecuteReader

        ' затем добавим по порядку
        Do While rdr.Read
            NumericEditAxisYMin.Value = CDbl(rdr("НижнееЗначение"))
            NumericEditAxisYMax.Value = CDbl(rdr("ВерхнееЗначение"))
            SlidePositionTicks.Value = CDbl(rdr("РасположениеМетки"))
            SlidePositionNumeric.Value = CDbl(rdr("РасположениеЧисла"))

            ' по умолчанию 0 ось уже есть и оси в возрастающем порядке и не повторяются
            If CInt(rdr("НомерОси")) > 0 Then mParrentForm.ScatterGraphParameter.YAxes.Add(New YAxis())

            SetAttributeAxisY()
        Loop

        rdr.Close()

        isNeedToUpdateCaption = False
        ' теперь с шлейфами, они уже очищены
        strSQL = "SELECT [КонфигурацииОтображения].[keyКонфигурацияОтображения], [КонфигурацииОтображения].[ИмяКонфигурации], ПараметрОтображения.* " &
            "FROM КонфигурацииОтображения RIGHT JOIN ПараметрОтображения ON [КонфигурацииОтображения].[keyКонфигурацияОтображения]=[ПараметрОтображения].[keyКонфигурацияОтображения] " &
            "WHERE ((([КонфигурацииОтображения]." & whereCondition
        cmd.CommandText = strSQL
        rdr = cmd.ExecuteReader

        ' добавим по порядку
        Do While rdr.Read
            Dim index As Integer = ComboBoxParametersGraph.Items.IndexOf(rdr("ИмяПараметра"))
            If index <> -1 Then
                ComboBoxParametersGraph.SelectedIndex = index 'выделим имя
                'Debug.Print(rdr("НомерОси"))
                'sldAssignAxis.Value = rdr("НомерОси")
                'System.Windows.Forms.Application.DoEvents()
                AddPlotToAxis(CInt(rdr("НомерОси")))
            End If
        Loop

        rdr.Close()
        cn.Close()
        isNeedToUpdateCaption = True

        UpdateCaptionAxes()
        isNeedToSave = False
    End Sub

    ''' <summary>
    ''' Добавить Шлейф В Слайдер
    ''' </summary>
    ''' <param name="numberAxis"></param>
    Private Sub AddPlotToAxis(ByVal numberAxis As Integer)
        Dim plot As ScatterPlot = New ScatterPlot
        Dim I, maxRange As Integer
        Dim arrX(), arrY() As Double
        Dim amplidude As Double

        For I = 0 To SlidePlots.CustomDivisions.Count - 1
            If SlidePlots.CustomDivisions(I).Text = ComboBoxParametersGraph.Text Then
                MessageBox.Show("Для этого параметра шлейф уже создан!", "Ошибка добавление графика", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
        Next

        ' добавить шлейф в график и получить номер
        mParrentForm.ScatterGraphParameter.Plots.Add(plot)
        countPlots = mParrentForm.ScatterGraphParameter.Plots.Count
        plot.PointStyle = PointStyle.SolidDiamond

        ' цвет точек как цвет оси
        If mParrentForm.ScatterGraphParameter.YAxes.Count = 1 Then numberAxis = 0

        plot.PointColor = mParrentForm.ScatterGraphParameter.YAxes(numberAxis).CaptionForeColor
        Dim NewScaleCustomDivision As ScaleCustomDivision = New ScaleCustomDivision With {
            .Text = ComboBoxParametersGraph.Text
        }
        SlidePlots.CustomDivisions.Add(NewScaleCustomDivision)
        SetRangeForSlidePlots(countPlots)
        NewScaleCustomDivision.Value = countPlots - 1
        ' цвет линии назначается автоматически
        plot.LineColor = ColorsNet((countPlots - 1) Mod 7)
        maxRange = CInt(mParrentForm.ScatterGraphParameter.XAxes(0).Range.Maximum)

        Dim divider As Single
        If maxRange < 1 Then
            divider = 0.01
        ElseIf maxRange >= 1 AndAlso maxRange < 10 Then
            divider = 0.1
        ElseIf maxRange >= 10 AndAlso maxRange < 100 Then
            divider = 1
        ElseIf maxRange >= 100 AndAlso maxRange < 1000 Then
            divider = 10
        ElseIf maxRange >= 1000 AndAlso maxRange < 10000 Then
            divider = 100
        ElseIf maxRange >= 10000 Then
            divider = 1000
        End If

        Dim steps As Integer = CInt(maxRange / divider)
        'ReDim_arrX(steps)
        'ReDim_arrY(steps)
        Re.Dim(arrX, steps)
        Re.Dim(arrY, steps)

        For I = 0 To steps 'Step 5
            arrX(I) = I * divider
        Next

        amplidude = mParrentForm.ScatterGraphParameter.YAxes(CInt(numberAxis)).Range.Maximum / 2 * Rnd()

        For I = 0 To steps 'Step 5
            arrY(I) = Math.Sin(I / 20.0# * Math.PI * 2) * amplidude + amplidude
        Next

        mParrentForm.ScatterGraphParameter.Plots(countPlots - 1).PlotXY(arrX, arrY)
        ButtonRemovePlot.Enabled = True
        ButtonAssignPlotToAxis.Enabled = True
        mParrentForm.ScatterGraphParameter.Plots(countPlots - 1).YAxis = mParrentForm.ScatterGraphParameter.YAxes(numberAxis)

        LabelGrafColorNext.BackColor = ColorsNet(countPlots Mod 7)
        SlidePlots.Value = countPlots - 1

        UpdateCaptionAxes()
    End Sub

    ''' <summary>
    ''' Назначить Диапазон Слайдера Шлейфов
    ''' </summary>
    ''' <param name="countPlots"></param>
    Private Sub SetRangeForSlidePlots(ByVal countPlots As Integer)
        If countPlots > 1 Then ' больше чем один шлейф
            SlidePlots.Enabled = True
            SlidePlots.Range = New Range(0, countPlots - 1)
        Else ' если один шлейф или меньше диапазон от 0 до 1, иначе ошибка
            SlidePlots.Enabled = False
            SlidePlots.Range = New Range(0, 1)
        End If
    End Sub

    ''' <summary>
    ''' Установить Аттрибуты Оси
    ''' </summary>
    Private Sub SetAttributeAxisY()
        With mParrentForm.ScatterGraphParameter.YAxes
            countAxes = .Count
            .Item(countAxes - 1).CaptionForeColor = ColorsNet((countAxes - 1) Mod 7)
            .Item(countAxes - 1).MajorDivisions.TickColor = ColorsNet((countAxes - 1) Mod 7)
            .Item(countAxes - 1).MajorDivisions.LabelForeColor = ColorsNet((countAxes - 1) Mod 7)
            .Item(countAxes - 1).MinorDivisions.TickColor = ColorsNet((countAxes - 1) Mod 7)
            .Item(countAxes - 1).Tag = CStr(countAxes - 1)
            .Item(countAxes - 1).Mode = AxisMode.Fixed
            SetRangeForSlideAssignAxis(countAxes)
            ButtonRemoveAxis.Enabled = True

            If SlidePositionTicks.Value = Disposition.Left Then
                .Item(countAxes - 1).Position = YAxisPosition.Left
            ElseIf SlidePositionTicks.Value = Disposition.Right Then
                .Item(countAxes - 1).Position = YAxisPosition.Right
            End If

            If SlidePositionNumeric.Value = Disposition.Left Then
                .Item(countAxes - 1).CaptionPosition = YAxisPosition.Left
            ElseIf SlidePositionNumeric.Value = Disposition.Right Then
                .Item(countAxes - 1).CaptionPosition = YAxisPosition.Right
            End If

            .Item(countAxes - 1).Range = New Range(NumericEditAxisYMin.Value, NumericEditAxisYMax.Value)
        End With

        SlideSelectAxis.Value = countAxes - 1
        LabelAxisColorNext.BackColor = ColorsNet((countAxes) Mod 7)
        ButtonAddAxis.Enabled = countAxes < cMaxAxes
        NumericEditAxisYMin.Value = 0 '4 - (NumAxes * 2)
        NumericEditAxisYMax.Value = 4 + (countAxes * 5) '14 - (NumAxes * 2)
    End Sub

    ''' <summary>
    ''' Назначить Диапазон Слайдеров Осей
    ''' </summary>
    ''' <param name="countAxis"></param>
    Private Sub SetRangeForSlideAssignAxis(ByVal countAxis As Integer)
        If countAxis > 1 Then
            SlideAssignAxis.Enabled = True
            SlideSelectAxis.Range = New Range(0, countAxis - 1)
            SlideAssignAxis.Range = New Range(0, countAxis - 1)
        Else
            SlideSelectAxis.Range = New Range(0, 1)
            SlideAssignAxis.Range = New Range(0, 1)
            SlideAssignAxis.Value = 1
            SlideAssignAxis.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Обновить Надписи Осей
    ''' </summary>
    Private Sub UpdateCaptionAxes()
        If isNeedToUpdateCaption Then
            With mParrentForm.ScatterGraphParameter
                For I As Integer = 0 To .YAxes.Count - 1
                    'просмотр всех графиков принадлежащих данной оси
                    Dim axisCaption As String = String.Empty

                    For J As Integer = 0 To .Plots.Count - 1
                        If .Plots(J).YAxis.Tag.ToString = .YAxes(I).Tag.ToString Then
                            If SlidePlots.CustomDivisions.Count > 0 Then
                                axisCaption &= SlidePlots.CustomDivisions(J).Text & "  "
                            End If
                        End If
                    Next

                    .YAxes(I).Caption = axisCaption
                    '.YAxes(I).CaptionForeColor = .YAxes(I).CaptionForeColor
                Next I
            End With
        End If
    End Sub

    ''' <summary>
    ''' Установить Счетчик Свечения Графика по фоновой частоте
    ''' </summary>   
    Private Sub SetCounterLightByFrequencyBackground()
        mParrentForm.FormParrent.Manager.CounterGraph = CInt(mParrentForm.FormParrent.Manager.FrequencyBackground / CSng(ComboBoxFrequency.Text))
    End Sub

    ''' <summary>
    ''' Установить Счетчик Свечения Графика по выбору пользователя
    ''' </summary>
    Private Sub SetCounterLightByUser()
        mParrentForm.FormParrent.Manager.CounterTailGraphByParameters = 10

        Try
            mParrentForm.FormParrent.Manager.CounterTailGraphByParameters = CInt(CSng(ComboBoxFrequency.Text) * CInt(ComboBoxTimeTail.Text))
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Частоты Построенния Графика
    ''' </summary>
    ''' <param name="inFrequencyBackground"></param>
    Private Sub SetFrequencyGraph(ByRef inFrequencyBackground As Integer)
        Select Case inFrequencyBackground
            Case 1
                'ReDim_FrequencyGraph(1)
                Re.Dim(FrequencyGraph, 1)
                FrequencyGraph(0) = 0.5
                FrequencyGraph(1) = 1
                Exit Select
            Case 2
                'ReDim_FrequencyGraph(2)
                Re.Dim(FrequencyGraph, 2)
                FrequencyGraph(0) = 0.5
                FrequencyGraph(1) = 1
                FrequencyGraph(2) = 2
                Exit Select
            Case 5
                'ReDim_FrequencyGraph(3)
                Re.Dim(FrequencyGraph, 3)
                FrequencyGraph(0) = 0.5
                FrequencyGraph(1) = 1
                FrequencyGraph(2) = 2.5
                FrequencyGraph(3) = 5
                Exit Select
            Case 10
                'ReDim_FrequencyGraph(4)
                Re.Dim(FrequencyGraph, 4)
                FrequencyGraph(0) = 0.5
                FrequencyGraph(1) = 1
                FrequencyGraph(2) = 2
                FrequencyGraph(3) = 5
                FrequencyGraph(4) = 10
                Exit Select
            Case 20
                'ReDim_FrequencyGraph(5)
                Re.Dim(FrequencyGraph, 5)
                FrequencyGraph(0) = 0.5
                FrequencyGraph(1) = 1
                FrequencyGraph(2) = 2
                FrequencyGraph(3) = 4
                FrequencyGraph(4) = 10
                FrequencyGraph(5) = 20
            Case 50
                'ReDim_FrequencyGraph(5)
                Re.Dim(FrequencyGraph, 5)
                FrequencyGraph(0) = 0.5
                FrequencyGraph(1) = 1
                FrequencyGraph(2) = 2
                FrequencyGraph(3) = 4
                FrequencyGraph(4) = 10
                FrequencyGraph(5) = 20
                Exit Select
            Case 100
                'ReDim_FrequencyGraph(5)
                Re.Dim(FrequencyGraph, 5)
                FrequencyGraph(0) = 0.5
                FrequencyGraph(1) = 1
                FrequencyGraph(2) = 2
                FrequencyGraph(3) = 4
                FrequencyGraph(4) = 10
                FrequencyGraph(5) = 20
                Exit Select
        End Select
    End Sub

    ''' <summary>
    ''' Обработчик события добавления строки.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    Private Sub OnRowUpdated(ByVal sender As Object, ByVal args As OleDbRowUpdatedEventArgs)
        ' команда получения нового идентификатора для новой добавленной строки
        'Dim newID As Integer = 0
        Dim idCMD As OleDbCommand = New OleDbCommand("SELECT @@IDENTITY", connection)

        If args.StatementType = StatementType.Insert Then
            ' записать этот полученный идентификатор в только что добавленную строку
            mParrentForm.FormParrent.Manager.KeyConfiguration = CInt(idCMD.ExecuteScalar())
            args.Row("keyКонфигурацияОтображения") = mParrentForm.FormParrent.Manager.KeyConfiguration
        End If
    End Sub

    ''' <summary>
    ''' Записать Список В Базу
    ''' </summary>
    Private Sub SaveConfiguration()
        Dim I, position As Integer
        Dim strSQL As String
        Dim cn As OleDbConnection
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow
        Dim cb As OleDbCommandBuilder
        Dim isConfigurationContain As Boolean
        Dim nameConfiguration As String = ComboBoxListConfigurations.Text

        ' если имя пусто вопрос  "введите имя" и так как модально нельзя то выход из данной процедуры
        If nameConfiguration <> "" Then
            ' проверка число осей должно быть больше 1, число параметров больше 0
            If SlidePlots.CustomDivisions.Count = 0 Then
                MessageBox.Show($"Отсутствуют шлейфы параметров для прикрепленния к осям!{vbCrLf}Изменения не будут записаны.",
                                "Запись конфигурации", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' если имя новое, то новая перекладка добавляется
            ' если переписать то удаление из базы старых и запись новых
            ' если добавить то запись в базу, очистка comСписки и считывание в лист заново
            For I = 0 To ComboBoxListConfigurations.Items.Count - 1
                If ComboBoxListConfigurations.Items(I).ToString = nameConfiguration Then
                    isConfigurationContain = True
                    Exit For
                End If
            Next

            ' Открыть подключение
            cn = New OleDbConnection(BuildCnnStr(PROVIDER_JET, mParrentForm.FormParrent.Manager.PathKT))
            If isConfigurationContain Then
                strSQL = $"SELECT КонфигурацииОтображения.* FROM КонфигурацииОтображения WHERE ИмяКонфигурации = '{nameConfiguration}'"
            Else
                strSQL = "SELECT КонфигурацииОтображения.* FROM КонфигурацииОтображения"
            End If

            ' Создание recordset
            cn.Open()
            connection = cn 'для события вставки новой строки
            odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
            odaDataAdapter.Fill(dtDataTable)

            Dim cmd As OleDbCommand = cn.CreateCommand
            cmd.CommandType = CommandType.Text

            If isConfigurationContain Then 'удаляется старая группа
                If dtDataTable.Rows.Count > 0 Then dtDataTable.Rows(0).Delete()
            Else ' добавить в комбо и в массив
                ComboBoxListConfigurations.Items.Add(nameConfiguration)
            End If

            ' добавляется новая конфигурация или перезаписывается старая
            drDataRow = dtDataTable.NewRow
            drDataRow.BeginEdit()
            drDataRow("ИмяКонфигурации") = nameConfiguration 'имя
            drDataRow("ЧастотаПостроения") = CSng(ComboBoxFrequency.Text)
            drDataRow("ВремяИлиПараметр") = RadioButtonTypeAxisX.Checked
            drDataRow("ИмяПараметраОсиХ") = ComboBoxParametersAxis.Text
            drDataRow("ВремяСвечения") = CInt(ComboBoxTimeTail.Text)
            drDataRow("МинОсь") = CDbl(NumericEditParamMin.Value)
            drDataRow("МахОсь") = CDbl(NumericEditParamMax.Value)
            drDataRow.EndEdit()
            dtDataTable.Rows.Add(drDataRow)

            ' подключить обработчик события генерации автоинкремента для добавляемой строки
            AddHandler odaDataAdapter.RowUpdated, New OleDbRowUpdatedEventHandler(AddressOf OnRowUpdated)
            cb = New OleDbCommandBuilder(odaDataAdapter)
            odaDataAdapter.Update(dtDataTable)
            Thread.Sleep(1000)

            'Dim rdr As OleDbDataReader
            'strSQL = "SELECT keyКонфигурацияОтображения FROM КонфигурацииОтображения WHERE ИмяКонфигурации = '" & strИмяСписки & "'"
            'cmd.CommandText = strSQL
            'rdr = cmd.ExecuteReader
            'If rdr.Read() = True Then
            '    lngkeyКонфигурация = rdr("keyКонфигурацияОтображения")
            'End If
            'rdr.Close()

            'очистить
            connection = Nothing

            'запись осей
            strSQL = "SELECT Ось.* FROM Ось WHERE keyКонфигурацияОтображения = " & mParrentForm.FormParrent.Manager.KeyConfiguration
            odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
            dtDataTable = New DataTable
            odaDataAdapter.Fill(dtDataTable)

            For I = 0 To mParrentForm.ScatterGraphParameter.YAxes.Count - 1
                drDataRow = dtDataTable.NewRow
                drDataRow.BeginEdit()
                drDataRow("keyКонфигурацияОтображения") = mParrentForm.FormParrent.Manager.KeyConfiguration
                drDataRow("НомерОси") = I
                drDataRow("НижнееЗначение") = mParrentForm.ScatterGraphParameter.YAxes(I).Range.Minimum
                drDataRow("ВерхнееЗначение") = mParrentForm.ScatterGraphParameter.YAxes(I).Range.Maximum
                drDataRow("НомерЦвета") = I Mod 7

                If mParrentForm.ScatterGraphParameter.YAxes(I).Position = YAxisPosition.Left Then
                    position = Disposition.Left
                ElseIf mParrentForm.ScatterGraphParameter.YAxes(I).Position = YAxisPosition.Right Then
                    position = Disposition.Right
                Else
                    position = Disposition.None
                End If

                drDataRow("РасположениеМетки") = position

                If mParrentForm.ScatterGraphParameter.YAxes(I).CaptionPosition = YAxisPosition.Left Then
                    position = Disposition.Left
                ElseIf mParrentForm.ScatterGraphParameter.YAxes(I).CaptionPosition = YAxisPosition.Right Then
                    position = Disposition.Right
                Else
                    position = Disposition.None
                End If

                drDataRow("РасположениеЧисла") = position
                drDataRow.EndEdit()
                dtDataTable.Rows.Add(drDataRow)
            Next

            cb = New OleDbCommandBuilder(odaDataAdapter)
            odaDataAdapter.Update(dtDataTable)
            Thread.Sleep(1000)

            strSQL = "SELECT ПараметрОтображения.* FROM ПараметрОтображения WHERE keyКонфигурацияОтображения = " & mParrentForm.FormParrent.Manager.KeyConfiguration
            odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
            dtDataTable = New DataTable
            odaDataAdapter.Fill(dtDataTable)

            For I = 0 To SlidePlots.CustomDivisions.Count - 1
                SlidePlots.Value = I  'начинается с 0
                drDataRow = dtDataTable.NewRow
                drDataRow.BeginEdit()
                drDataRow("keyКонфигурацияОтображения") = mParrentForm.FormParrent.Manager.KeyConfiguration
                Dim ИмяПараметра As String = Nothing
                ИмяПараметра = SlidePlots.CustomDivisions(I).Text
                drDataRow("ИмяПараметра") = ИмяПараметра
                'к какой оси приписан шлейф
                drDataRow("НомерОси") = Val(mParrentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).YAxis.Tag)
                drDataRow("НомерЦвета") = (CInt(SlidePlots.Value)) Mod 7
                drDataRow.EndEdit()
                dtDataTable.Rows.Add(drDataRow)
            Next

            cb = New OleDbCommandBuilder(odaDataAdapter)
            odaDataAdapter.Update(dtDataTable)
            Thread.Sleep(1000)
            cn.Close()
            isNeedToSave = False
            Application.DoEvents()
        Else ' надо ввести имя
            MessageBox.Show("Введите имя", "Запись конфигурации", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
#End Region

#Region "ButtonConfiguration Event"
    Private Sub ButtonRestoreConfiguration_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonRestoreConfiguration.Click
        If ComboBoxListConfigurations.Text <> "" Then
            RestoreConfiguration($"ИмяКонфигурации)= '{ComboBoxListConfigurations.Text}'))")
            isNeedToSave = False
        End If
    End Sub

    Private Sub ButtonSaveConfiguration_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveConfiguration.Click
        ButtonClose.Enabled = False
        Application.DoEvents()
        SaveConfiguration()
        ButtonClose.Enabled = True
        Application.DoEvents()
        isNeedToSave = False
    End Sub

    Private Sub ButtonDeleteConfiguration_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteConfiguration.Click
        Dim strSQL As String
        Dim isSelectedIndexLast As Boolean ' удаляется Последняя По Списку
        Dim cn As OleDbConnection
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim cb As OleDbCommandBuilder
        Dim nameConfiguration As String = ComboBoxListConfigurations.Text

        If nameConfiguration <> "" AndAlso ComboBoxListConfigurations.SelectedIndex <> -1 Then
            If ComboBoxListConfigurations.Items.Count > 1 Then

                If ComboBoxListConfigurations.SelectedIndex = ComboBoxListConfigurations.Items.Count - 1 Then isSelectedIndexLast = True

                ' удалить из листа
                ComboBoxListConfigurations.Items.RemoveAt(ComboBoxListConfigurations.SelectedIndex)
                cn = New OleDbConnection(BuildCnnStr(PROVIDER_JET, mParrentForm.FormParrent.Manager.PathKT))

                ' Открыть подключение
                strSQL = $"SELECT КонфигурацииОтображения.* FROM КонфигурацииОтображения WHERE ИмяКонфигурации = '{nameConfiguration}'"
                ' Создание recordset
                cn.Open()
                odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
                odaDataAdapter.Fill(dtDataTable)

                If dtDataTable.Rows.Count > 0 Then dtDataTable.Rows(0).Delete()

                cb = New OleDbCommandBuilder(odaDataAdapter)
                odaDataAdapter.Update(dtDataTable)
                cn.Close()
                Thread.Sleep(500)

                If isSelectedIndexLast = True Then
                    ComboBoxListConfigurations.SelectedIndex = ComboBoxListConfigurations.Items.Count - 1
                    ButtonRestoreConfiguration.PerformClick()
                End If
            Else
                Const caption As String = "Удаление Списка"
                Const text As String = "Последнюю запись удалять нельзя!"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
    End Sub
#End Region

#Region "RadioButtonTypeAxis Events"
    Private Sub RadioButtonTypeAxis_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonTypeAxisX.CheckedChanged, RadioButtonTypeAxisY.CheckedChanged
        SetControlsColorAndEnabled()
        isNeedToSave = True
    End Sub
    Private Sub SetControlsColorAndEnabled()
        SetControlsEnabled(Not RadioButtonTypeAxisX.Checked)

        If RadioButtonTypeAxisX.Checked Then
            SetControlsColor(constShade)
        Else
            SetControlsColor(constFace)
        End If
    End Sub

    Private Sub SetControlsEnabled(inEnabled As Boolean)
        ComboBoxTimeTail.Enabled = inEnabled
        NumericEditParamMin.Enabled = inEnabled
        NumericEditParamMax.Enabled = inEnabled
        ComboBoxParametersAxis.Enabled = inEnabled
    End Sub

    Private Sub SetControlsColor(inColor As Color)
        RadioButtonTypeAxisY.BackColor = inColor
        LabelSelectParameterAxisX.BackColor = inColor
        LabelTimeTail.BackColor = inColor
        ShapeParameter.BackColor = inColor
        LabelMinAxisX.BackColor = inColor
        LabelMaxAxisX.BackColor = inColor
    End Sub
#End Region

#Region "Controls Events"
    Private Sub ComboBoxParametersAxis_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxParametersAxis.SelectedIndexChanged
        isNeedToSave = True
    End Sub

    Private Sub ComboBoxParametersGraph_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBoxParametersGraph.SelectedIndexChanged
        ButtonAddPlot.Text = "Добавить шлейф " & ComboBoxParametersGraph.Text
        isNeedToSave = True
    End Sub

    Private Sub Controls_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles NumericEditParamMin.AfterChangeValue,
                                                                                                                    NumericEditParamMax.AfterChangeValue,
                                                                                                                    NumericEditAxisYMin.AfterChangeValue,
                                                                                                                    NumericEditAxisYMax.AfterChangeValue,
                                                                                                                    SlidePositionTicks.AfterChangeValue,
                                                                                                                    SlidePositionNumeric.AfterChangeValue
        isNeedToSave = True
    End Sub
#End Region

#Region "Plots"
    Private Sub ButtonAddPlot_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonAddPlot.Click
        AddPlotToAxis(CInt(SlideAssignAxis.Value))
        isNeedToSave = True
    End Sub

    Private Sub ButtonRemovePlot_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonRemovePlot.Click
        RemoveAtPlot(CInt(SlidePlots.Value))
        isNeedToSave = True
    End Sub

    ''' <summary>
    ''' Удалить Шлейф
    ''' </summary>
    ''' <param name="numberRemovedPlot"></param>
    Private Sub RemoveAtPlot(ByVal numberRemovedPlot As Integer)
        Dim I As Integer

        isProcessDeletePlot = True
        SlidePlots.CustomDivisions.RemoveAt(numberRemovedPlot)
        isProcessDeletePlot = False

        ' переопределить значения
        ' начинается с 0
        For I = 0 To SlidePlots.CustomDivisions.Count - 1
            SlidePlots.CustomDivisions(I).Value = I
        Next

        If SlidePlots.CustomDivisions.Count > 0 Then
            If SlidePlots.CustomDivisions.Count = 1 Then
                ButtonRemovePlot.Text = "Удалить шлейф " & SlidePlots.CustomDivisions(0).Text
            Else
                ButtonRemovePlot.Text = "Удалить шлейф " & SlidePlots.CustomDivisions(CInt(SlidePlots.Value) - 1).Text
            End If
        End If

        mParrentForm.ScatterGraphParameter.Plots.RemoveAt(numberRemovedPlot)

        countPlots = mParrentForm.ScatterGraphParameter.Plots.Count
        SetRangeForSlidePlots(countPlots)

        If mParrentForm.ScatterGraphParameter.Plots.Count > 0 Then
            If numberRemovedPlot > mParrentForm.ScatterGraphParameter.Plots.Count - 1 Then ' если удалялся последний график
                SlidePlots.Value = mParrentForm.ScatterGraphParameter.Plots.Count - 1
            Else 'если удалялся не последний график
                SlidePlots.Value = numberRemovedPlot
            End If

            For I = 0 To mParrentForm.ScatterGraphParameter.Plots.Count - 1
                mParrentForm.ScatterGraphParameter.Plots(I).LineColor = ColorsNet(I Mod 7)
            Next

            SlideAssignAxis.Value = Val(mParrentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).YAxis.Tag)
        End If

        'If PlotsCount > 0 Then sldPlot.Value = PlotsCount - 1
        SlidePlots.Enabled = countPlots > 1

        If countPlots = 0 Then
            ButtonRemovePlot.Enabled = False
            ButtonAssignPlotToAxis.Enabled = False
        End If

        LabelGrafColorNext.BackColor = ColorsNet(countPlots Mod 7)
        UpdateCaptionAxes()
    End Sub

    Private Sub SlidePlots_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlidePlots.AfterChangeValue
        If isProcessDeletePlot Then Exit Sub

        If Me.IsHandleCreated Then
            Dim I As Integer
            SlidePlots.PointerColor = ColorsNet(CInt(SlidePlots.Value Mod 7))
            'sldPlot переместился, значит присвоить новое значение sldAssignAxis.Value
            SlideAssignAxis.Value = Val(mParrentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).YAxis.Tag)

            For I = 0 To mParrentForm.ScatterGraphParameter.Plots.Count - 1
                mParrentForm.ScatterGraphParameter.Plots(I).LineWidth = 1
                mParrentForm.ScatterGraphParameter.Plots(I).PointStyle = PointStyle.SolidDiamond
            Next

            mParrentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).LineWidth = 2
            mParrentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).PointStyle = PointStyle.SolidSquare
            ButtonRemovePlot.Text = "Удалить шлейф " & SlidePlots.CustomDivisions(CInt(e.NewValue)).Text
            isNeedToSave = True
        End If
    End Sub

    Private Sub SlideAssignAxis_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlideAssignAxis.AfterChangeValue
        SlideAssignAxis.PointerColor = ColorsNet(CInt(e.NewValue Mod 7))
        ButtonAssignPlotToAxis.Text = "Прикрепить шлейф к оси " & CStr(e.NewValue)
        isNeedToSave = True
    End Sub

    ''' <summary>
    ''' Прикрепить График
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub ButtonAssignPlotToAxis_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonAssignPlotToAxis.Click
        mParrentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).YAxis = mParrentForm.ScatterGraphParameter.YAxes(CInt(SlideAssignAxis.Value))
        mParrentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).PointColor = mParrentForm.ScatterGraphParameter.YAxes(CInt(SlideAssignAxis.Value)).CaptionForeColor
        UpdateCaptionAxes()
        isNeedToSave = True
    End Sub
#End Region

#Region "Axis"
    Private Sub AddAxis_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonAddAxis.Click
        mParrentForm.ScatterGraphParameter.YAxes.Add(New NationalInstruments.UI.YAxis())
        SetAttributeAxisY()
        UpdateCaptionAxes()
        isNeedToSave = True
    End Sub

    Private Sub ButtonRemoveAxis_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonRemoveAxis.Click
        Dim I, J As Integer
        Dim numberRemovedAxis As Integer = CInt(SlideSelectAxis.Value)

        If numberRemovedAxis = 0 Then Exit Sub

        ' удалить те шлейфы, где встречаются номера удаляемой оси
        For I = mParrentForm.ScatterGraphParameter.Plots.Count - 1 To 0 Step -1 ' сверху вниз
            For J = SlidePlots.CustomDivisions.Count - 1 To 0 Step -1
                SlidePlots.Value = SlidePlots.CustomDivisions(J).Value
                If Val(mParrentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).YAxis.Tag) = numberRemovedAxis Then
                    RemoveAtPlot(CInt(SlidePlots.Value))
                End If
            Next
        Next

        ' а затем удалить саму дополнительную ось
        If mParrentForm.ScatterGraphParameter.YAxes.Count > 0 Then mParrentForm.ScatterGraphParameter.YAxes.RemoveAt(numberRemovedAxis)

        'дать новые имена осям
        For I = 0 To mParrentForm.ScatterGraphParameter.YAxes.Count - 1
            mParrentForm.ScatterGraphParameter.YAxes(I).Tag = CStr(I)
        Next
        'переместиь ползунок
        If numberRemovedAxis > mParrentForm.ScatterGraphParameter.YAxes.Count - 1 Then
            SlideSelectAxis.Value = mParrentForm.ScatterGraphParameter.YAxes.Count - 1
        Else
            SlideSelectAxis.Value = numberRemovedAxis
        End If

        For I = 0 To mParrentForm.ScatterGraphParameter.YAxes.Count - 1
            'присвоить цвета по умолчанию
            mParrentForm.ScatterGraphParameter.YAxes(I).CaptionForeColor = ColorsNet(I Mod 7)
            mParrentForm.ScatterGraphParameter.YAxes(I).MajorDivisions.TickColor = ColorsNet(I Mod 7)
            mParrentForm.ScatterGraphParameter.YAxes(I).MajorDivisions.LabelForeColor = ColorsNet(I Mod 7)
            mParrentForm.ScatterGraphParameter.YAxes(I).MinorDivisions.TickColor = ColorsNet(I Mod 7)

            'просмотр всех графиков принадлежащих данной оси и установка цвета
            For J = SlidePlots.CustomDivisions.Count - 1 To 0 Step -1
                If mParrentForm.ScatterGraphParameter.Plots(J).YAxis.Tag.ToString = mParrentForm.ScatterGraphParameter.YAxes(I).Tag.ToString Then
                    mParrentForm.ScatterGraphParameter.Plots(J).PointColor = mParrentForm.ScatterGraphParameter.YAxes(I).CaptionForeColor
                End If
            Next
        Next

        countAxes = mParrentForm.ScatterGraphParameter.YAxes.Count

        If SlideAssignAxis.Value > countAxes - 1 Then SlideAssignAxis.Value = countAxes - 1
        If SlideSelectAxis.Value > countAxes - 1 Then SlideSelectAxis.Value = countAxes - 1

        SetRangeForSlideAssignAxis(countAxes)

        ButtonRemoveAxis.Enabled = countAxes > 1
        ButtonAddAxis.Enabled = countAxes < cMaxAxes
        LabelAxisColorNext.BackColor = ColorsNet((countAxes) Mod 7)
        UpdateCaptionAxes()
        isNeedToSave = True
    End Sub

    Private Sub ButtonUpdateAxis_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonUpdateAxis.Click
        If mParrentForm.ScatterGraphParameter.YAxes.Count = 1 AndAlso SlideSelectAxis.Value > 0 Then Exit Sub

        If SlidePositionTicks.Value = Disposition.Left Then
            mParrentForm.ScatterGraphParameter.YAxes(CInt(SlideSelectAxis.Value)).Position = YAxisPosition.Left
        ElseIf SlidePositionTicks.Value = Disposition.Right Then
            mParrentForm.ScatterGraphParameter.YAxes(CInt(SlideSelectAxis.Value)).Position = YAxisPosition.Right
        End If

        If SlidePositionNumeric.Value = Disposition.Left Then
            mParrentForm.ScatterGraphParameter.YAxes(CInt(SlideSelectAxis.Value)).CaptionPosition = YAxisPosition.Left
        ElseIf SlidePositionNumeric.Value = Disposition.Right Then
            mParrentForm.ScatterGraphParameter.YAxes(CInt(SlideSelectAxis.Value)).CaptionPosition = YAxisPosition.Right
        End If

        mParrentForm.ScatterGraphParameter.YAxes(CInt(SlideSelectAxis.Value)).Range = New Range(NumericEditAxisYMin.Value, NumericEditAxisYMax.Value)
        isNeedToSave = True
    End Sub

    Private Sub SlideSelectAxis_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlideSelectAxis.AfterChangeValue
        If Me.IsHandleCreated Then
            SlideSelectAxis.PointerColor = ColorsNet(CInt(e.NewValue Mod 7))

            If mParrentForm.ScatterGraphParameter.YAxes.Count = 1 AndAlso e.NewValue > 0 Then Exit Sub

            If mParrentForm.ScatterGraphParameter.YAxes(CInt(e.NewValue)).Position = YAxisPosition.Left Then
                SlidePositionTicks.Value = Disposition.Left
            ElseIf mParrentForm.ScatterGraphParameter.YAxes(CInt(e.NewValue)).Position = YAxisPosition.Right Then
                SlidePositionTicks.Value = Disposition.Right
            Else
                SlidePositionTicks.Value = Disposition.None
            End If

            If mParrentForm.ScatterGraphParameter.YAxes(CInt(e.NewValue)).CaptionPosition = YAxisPosition.Left Then
                SlidePositionNumeric.Value = Disposition.Left
            ElseIf mParrentForm.ScatterGraphParameter.YAxes(CInt(e.NewValue)).CaptionPosition = YAxisPosition.Right Then
                SlidePositionNumeric.Value = Disposition.Right
            Else
                SlidePositionNumeric.Value = Disposition.None
            End If

            NumericEditAxisYMin.Value = mParrentForm.ScatterGraphParameter.YAxes(CInt(e.NewValue)).Range.Minimum
            NumericEditAxisYMax.Value = mParrentForm.ScatterGraphParameter.YAxes(CInt(e.NewValue)).Range.Maximum

            If e.NewValue = 0 Then
                ButtonRemoveAxis.Enabled = False
                ButtonRemoveAxis.Text = "Удалить нельзя"
            Else
                ButtonRemoveAxis.Enabled = True
                ButtonRemoveAxis.Text = "Удалить ось " & CStr(e.NewValue)
            End If

            isNeedToSave = True
        End If
    End Sub
#End Region

#Region "Frequency"
    Private Sub ComboBoxTimeTail_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBoxTimeTail.SelectedIndexChanged
        If Me.IsHandleCreated Then
            SetCounterLightByUser()
            isNeedToSave = True
        End If
    End Sub

    Private Sub ComboBoxFrequency_SelectedIndexChanged(ByVal ByVeventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBoxFrequency.SelectedIndexChanged
        If Me.IsHandleCreated Then
            If ComboBoxTimeTail.SelectedIndex = -1 Then Exit Sub

            SetCounterLightByUser()
            isNeedToSave = True
        End If
    End Sub
#End Region

End Class

'убрал
'Dim CalculationAsm As System.Reflection.Assembly
'Dim AssemblyName As String = System.Configuration.ConfigurationManager.AppSettings("CalculationAssemblyFilename") '.AppSettings("CalculationAssemblyFilename")
'CalculationAsm = System.Reflection.Assembly.LoadFrom(AssemblyName)
'Dim sItem As CalculationDLLDotNetInterfaces.IclsСвойства
'Dim ПеременнаяName As String = System.Configuration.ConfigurationManager.AppSettings("clsСвойстваName")
''создание экземпляра класса
'sItem = CType(CalculationAsm.CreateInstance(ПеременнаяName), CalculationDLLDotNetInterfaces.IclsСвойства)
