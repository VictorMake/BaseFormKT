Imports System.Data.OleDb
Imports System.Drawing
Imports System.Math
Imports System.Windows.Forms
Imports MathematicalLibrary

Public Class FormGraf
    Public Property FormParrent() As frmBaseKT

    Private Structure GraphParametersByParameter
        Dim NameParameter As String ' Наименование Параметра
        Dim NumberTail As Integer   ' номер шлейфа
        Dim NumberAxis As Integer   ' Номер Оси 
        Dim Value As Double         ' Значение Параметра
        Dim IndexColor As Integer   ' Цвет
        Dim IsTailVisible As Boolean ' Отображать Шлейф
        Dim UnitOfMeasure As String ' Единица Измерения
    End Structure

    Public ReadOnly Property IsGraphAct() As Boolean
        Get
            Return mIsGraphAct
        End Get
    End Property

    Private mAxesAdvanced As FormAxesAdvanced
    Private mFormEditorBounds As FormEditorBounds
    ''' <summary>
    ''' График Отображать
    ''' </summary>
    Private mIsGraphAct As Boolean
    ''' <summary>
    ''' Позиция Курсора График От Параметров
    ''' </summary>
    Private cursorPosition As Integer
    ''' <summary>
    ''' Имя Параметра Оси Х 
    ''' </summary>
    Private nameParameterOfAxisX As String
    ''' <summary>
    ''' Приведенное Оси Х 
    ''' если ось Х по параметру, а он приведенный то его расчитать
    ''' </summary>
    Private correctedParameterValueAxisX As Double
    Private arrGraphParametersByParameter As GraphParametersByParameter()
    ''' <summary>
    ''' Количество Графиков От Параметров 
    ''' </summary>
    Private plotsCount As Integer
    ''' <summary>
    ''' был очищен массив График От Параметров
    ''' </summary>
    Private isErasePlots As Boolean()
    Private arraysizeGraphParameter As Integer
    ''' <summary>
    ''' Был Очищен График От Параметров
    ''' </summary>
    Private isEraseParametersByParameter As Boolean
    ''' <summary>
    ''' данные для Графика От Параметров оси Х
    ''' </summary>
    Private XdataParameters As Double()
    Private YdataParameters As Double(,)
    Private Const arraySize As Integer = 1000
    ''' <summary>
    ''' Подробный Лист
    ''' </summary>
    Private isDetailedList As Boolean
    Private tempPointAnnotation As XYPointAnnotation

#Region "FormGraf"
    Public Sub New(ByVal FormParrent As frmBaseKT)
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Me.MdiParent = FormParrent
        Me.FormParrent = FormParrent
    End Sub

    Private Sub FormGraf_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        InitializeListViews()
        ChangeDetailList()
        ButtonDetail.Enabled = False
        TSButtonTuneTrand.Enabled = False
        TSButtonBounds.Enabled = False
    End Sub

    Private Sub FormGraf_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        FormParrent = Nothing
    End Sub

    Private Sub FormGraf_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        If Not FormParrent.IsWindowClosed Then e.Cancel = True
    End Sub

    Private Sub InitializeListViews()
        With ListViewParameter
            .Columns.Clear()
            .BorderStyle = BorderStyle.Fixed3D
            .View = View.Details
            .LabelEdit = False
            .AllowColumnReorder = False
            .CheckBoxes = False
            .GridLines = True

            .Columns.Add("Параметр", .Width * 5 \ 10, HorizontalAlignment.Left)
            .Columns.Add("Значение", .Width * 4 \ 10, HorizontalAlignment.Left)
            .Columns.Add("Ед.изм.", .Width - .Columns(0).Width - .Columns(1).Width - 8, HorizontalAlignment.Left)
        End With
    End Sub

    ''' <summary>
    ''' Изменить Представление Листа
    ''' </summary>
    Private Sub ChangeDetailList()
        If ButtonDetail.Checked Then
            ButtonDetail.Text = "Подробно"
            isDetailedList = False
        Else
            ButtonDetail.Text = "Выборочно"
            isDetailedList = True
        End If

        TuneListViewParameter()
    End Sub

    ''' <summary>
    ''' Настроить Лист Параметров
    ''' </summary>
    Private Sub TuneListViewParameter()
        Dim itmX As ListViewItem
        Dim I As Integer

        ListViewParameter.BeginUpdate()

        If mIsGraphAct = True Then
            ListViewParameter.Items.Clear()

            ' Создать переменную, чтобы добавлять объекты ListItem.
            If isDetailedList = True Then
                ListViewParameter.Font = New Font("Microsoft Sans Serif", 12, FontStyle.Bold, GraphicsUnit.Pixel)

                For I = 0 To UBound(arrGraphParametersByParameter)
                    itmX = New ListViewItem(arrGraphParametersByParameter(I).NameParameter) With {
                        .ForeColor = ColorsNet(arrGraphParametersByParameter(I).IndexColor)
                    }
                    itmX.SubItems.Add(CStr(0), Color.White, Color.Black, New Font("Microsoft Sans Serif", 12, FontStyle.Bold))
                    itmX.SubItems.Add(arrGraphParametersByParameter(I).UnitOfMeasure, Color.White, Color.Black, New Font("Microsoft Sans Serif", 12, FontStyle.Bold))
                    ListViewParameter.Items.Add(itmX)
                    itmX = Nothing
                Next
            Else
                ListViewParameter.Font = New Font("Tahoma", 24, FontStyle.Bold, GraphicsUnit.Pixel)
                For I = 0 To UBound(arrGraphParametersByParameter)
                    If arrGraphParametersByParameter(I).IsTailVisible = True Then
                        itmX = New ListViewItem(arrGraphParametersByParameter(I).NameParameter) With {
                            .ForeColor = ColorsNet(arrGraphParametersByParameter(I).IndexColor)
                        }
                        itmX.SubItems.Add(CStr(0), Color.White, Color.Black, New Font("Tahoma", 24, FontStyle.Bold))
                        itmX.SubItems.Add(arrGraphParametersByParameter(I).UnitOfMeasure, Color.White, Color.Black, New Font("Tahoma", 24, FontStyle.Bold))
                        ListViewParameter.Items.Add(itmX)
                        itmX = Nothing
                    End If
                Next
            End If
        End If

        ListViewParameter.EndUpdate()
    End Sub
#End Region

#Region "Tune"
    Private Sub ButtonDetail_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDetail.CheckedChanged
        ChangeDetailList()
    End Sub

    Private Sub TSButtonTuneTrand_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonTuneTrand.CheckedChanged
        If Me.IsHandleCreated Then
            If TSButtonTuneTrand.Checked Then
                mIsGraphAct = False
                TextBoxCollect.Visible = False
                ' устанавливается в FillCombo
                If FormParrent.Manager.IsCheckPassed Then ShowFormAxesAdvanced()
            Else
                Collect()
            End If
        End If
    End Sub

    Private Sub TSButtonBounds_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonBounds.CheckedChanged
        If Me.IsHandleCreated Then
            If TSButtonBounds.Checked Then
                mIsGraphAct = False
                TextBoxCollect.Visible = False
                If FormParrent.Manager.IsCheckPassed Then ShowFormEditorBounds()
            Else
                Collect()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Настроить Графики
    ''' </summary>
    Private Sub ShowFormAxesAdvanced()
        mAxesAdvanced = New FormAxesAdvanced(Me)
        mAxesAdvanced.Show()
        mAxesAdvanced = Nothing
    End Sub

    ''' <summary>
    ''' Настроить Границы
    ''' </summary>
    Private Sub ShowFormEditorBounds()
        mFormEditorBounds = New FormEditorBounds(Me)
        mFormEditorBounds.Show()
        mFormEditorBounds = Nothing
    End Sub

    ''' <summary>
    ''' Сбор
    ''' </summary>
    Public Sub Collect()
        RestoreConfiguration()
        If mIsGraphAct = True Then TuneViewParameters()
    End Sub

    ''' <summary>
    ''' Восстановить Режим Графиков
    ''' </summary>
    Private Sub RestoreConfiguration()
        ' вызывается из cmdНастроитьГрафик - запуск сбора
        Dim sSQL As String
        Dim dispositionLabel, I, numberAxis, mPlotsCount As Integer
        Dim plot As ScatterPlot
        Dim nameParameter, caption As String
        Dim cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, FormParrent.Manager.PathKT))
        Dim rdr As OleDbDataReader

        cn.Open()
        Dim cmd As OleDbCommand = cn.CreateCommand
        cmd.CommandType = CommandType.Text

        'If lngkeyКонфигурация = 0 Then 'конкретной конфигурации нет
        '    'считывания,настройки и записи не было значит считываем последнюю конфигурацию
        '    sSQL = "SELECT TOP 1 КонфигурацииОтображения.keyКонфигурацияОтображения " & _
        '    "FROM(КонфигурацииОтображения) " & _
        '    "ORDER BY КонфигурацииОтображения.keyКонфигурацияОтображения DESC;"
        '    ' Создание recordset
        '    cmd.CommandText = sSQL
        '    rdr = cmd.ExecuteReader
        '    'lngkeyКонфигурация в последней строке последняя сохраненная конфигурация
        '    'Do While rdr.Read
        '    If rdr.Read() = True Then
        '        lngkeyКонфигурация = rdr("keyКонфигурацияОтображения")
        '    End If
        '    'Loop
        '    rdr.Close()
        'End If

        ' считываем конкретную конфигурацию где lngkeyКонфигурация считан из конфигурационного файла
        sSQL = "SELECT КонфигурацииОтображения.* FROM КонфигурацииОтображения " & "WHERE КонфигурацииОтображения.keyКонфигурацияОтображения = " & FormParrent.Manager.KeyConfiguration
        cmd.CommandText = sSQL
        rdr = cmd.ExecuteReader

        If rdr.Read() Then
            FormParrent.Manager.IsGraphParameterByTime = CBool(rdr("ВремяИлиПараметр"))
            FormParrent.Manager.CounterGraph = CInt(FormParrent.Manager.FrequencyBackground / CInt(rdr("ЧастотаПостроения")))
            FormParrent.Manager.CounterTailGraphByParameters = CInt(rdr("ЧастотаПостроения")) * CInt(rdr("ВремяСвечения"))

            If FormParrent.Manager.IsGraphParameterByTime Then
                XAxis1.Caption = vbNullString
                XAxis1.Visible = False
            Else
                nameParameterOfAxisX = CStr(rdr("ИмяПараметраОсиХ"))
                XAxis1.Caption = nameParameterOfAxisX
                XAxis1.Range = New Range(CDbl(rdr("МинОсь")), CDbl(rdr("МахОсь")))
                XAxis1.Visible = True
            End If

            rdr.Close()

            sSQL = "SELECT КонфигурацииОтображения.ИмяКонфигурации, Ось.* " &
            "FROM КонфигурацииОтображения RIGHT JOIN Ось ON КонфигурацииОтображения.keyКонфигурацияОтображения = Ось.keyКонфигурацияОтображения " &
            "WHERE(((КонфигурацииОтображения.keyКонфигурацияОтображения) = " & FormParrent.Manager.KeyConfiguration & ")) " &
            "ORDER BY Ось.НомерОси;"

            ' Создание recordset
            cmd.CommandText = sSQL
            rdr = cmd.ExecuteReader
            ' сначало удалим оси
            For I = ScatterGraphParameter.YAxes.Count - 1 To 1 Step -1
                ScatterGraphParameter.YAxes.RemoveAt(I)
            Next

            ' затем добавим по порядку
            Do While rdr.Read
                numberAxis = CInt(rdr("НомерОси"))

                If numberAxis <> 0 Then ScatterGraphParameter.YAxes.Add(New YAxis())

                ScatterGraphParameter.YAxes(numberAxis).Tag = CStr(numberAxis)
                dispositionLabel = CInt(rdr("РасположениеМетки"))

                If dispositionLabel = Disposition.Left Then
                    ScatterGraphParameter.YAxes(numberAxis).Position = YAxisPosition.Left
                ElseIf dispositionLabel = Disposition.Right Then
                    ScatterGraphParameter.YAxes(numberAxis).Position = YAxisPosition.Right
                Else ' dispositionLabel = 3
                End If

                dispositionLabel = CInt(rdr("РасположениеЧисла"))

                If dispositionLabel = Disposition.Left Then
                    ScatterGraphParameter.YAxes(numberAxis).CaptionPosition = YAxisPosition.Left
                ElseIf dispositionLabel = Disposition.Right Then
                    ScatterGraphParameter.YAxes(numberAxis).CaptionPosition = YAxisPosition.Right
                Else 'dispositionLabel = 3
                End If

                ScatterGraphParameter.YAxes(numberAxis).Mode = NationalInstruments.UI.AxisMode.Fixed
                ScatterGraphParameter.YAxes(numberAxis).Range = New Range(CDbl(rdr("НижнееЗначение")), CDbl(rdr("ВерхнееЗначение")))
                ScatterGraphParameter.YAxes(numberAxis).CaptionForeColor = ColorsNet(CInt(rdr("НомерЦвета")))
                ScatterGraphParameter.YAxes(numberAxis).MajorDivisions.TickColor = ColorsNet(CInt(rdr("НомерЦвета")))
                ScatterGraphParameter.YAxes(numberAxis).MajorDivisions.LabelForeColor = ColorsNet(CInt(rdr("НомерЦвета")))
                ScatterGraphParameter.YAxes(numberAxis).MinorDivisions.TickColor = ColorsNet(CInt(rdr("НомерЦвета")))
            Loop
            rdr.Close()

            'ReDim_arrGraphParametersByParameter((FormParrent.Manager.MeasurementDataTable.Rows.Count + FormParrent.Manager.CalculatedDataTable.Rows.Count) - 1)
            Re.Dim(arrGraphParametersByParameter, (FormParrent.Manager.MeasurementDataTable.Rows.Count + FormParrent.Manager.CalculatedDataTable.Rows.Count) - 1)
            I = 0

            For Each rowИзмеренныйПараметр As BaseFormDataSet.ИзмеренныеПараметрыRow In FormParrent.Manager.MeasurementDataTable.Rows
                arrGraphParametersByParameter(I).NameParameter = rowИзмеренныйПараметр.ИмяПараметра
                arrGraphParametersByParameter(I).IndexColor = 7
                arrGraphParametersByParameter(I).UnitOfMeasure = rowИзмеренныйПараметр.РазмерностьВходная
                arrGraphParametersByParameter(I).NumberAxis = -1
                arrGraphParametersByParameter(I).NumberTail = -1
                I += 1
            Next

            For Each rowРасчетныйПараметр As BaseFormDataSet.РасчетныеПараметрыRow In FormParrent.Manager.CalculatedDataTable.Rows
                arrGraphParametersByParameter(I).NameParameter = rowРасчетныйПараметр.ИмяПараметра
                arrGraphParametersByParameter(I).IndexColor = 7
                arrGraphParametersByParameter(I).UnitOfMeasure = rowРасчетныйПараметр.РазмерностьВыходная
                arrGraphParametersByParameter(I).NumberAxis = -1
                arrGraphParametersByParameter(I).NumberTail = -1
                I += 1
            Next

            ' теперь с графиками
            sSQL = "SELECT [КонфигурацииОтображения].[keyКонфигурацияОтображения], [КонфигурацииОтображения].[ИмяКонфигурации], ПараметрОтображения.* " & "FROM КонфигурацииОтображения RIGHT JOIN ПараметрОтображения ON [КонфигурацииОтображения].[keyКонфигурацияОтображения]=[ПараметрОтображения].[keyКонфигурацияОтображения] " & "WHERE ((([КонфигурацииОтображения].keyКонфигурацияОтображения)= " & FormParrent.Manager.KeyConfiguration & "));"
            ' Создание recordset
            cmd.CommandText = sSQL
            rdr = cmd.ExecuteReader
            Me.plotsCount = 0
            ' сначало удалим графики
            ScatterGraphParameter.Annotations.Clear()
            ScatterGraphParameter.Plots.Clear()

            ' затем добавим по порядку
            Do While rdr.Read
                Me.plotsCount += 1
                plot = New ScatterPlot
                ScatterGraphParameter.Plots.Add(plot)
                plot.XAxis = Me.XAxis1
                mPlotsCount = ScatterGraphParameter.Plots.Count
                nameParameter = CStr(rdr("ИмяПараметра"))

                For I = 0 To UBound(arrGraphParametersByParameter)
                    If arrGraphParametersByParameter(I).NameParameter = nameParameter Then
                        arrGraphParametersByParameter(I).IndexColor = CInt(rdr("НомерЦвета"))
                        arrGraphParametersByParameter(I).NumberTail = mPlotsCount - 1
                        arrGraphParametersByParameter(I).NumberAxis = CInt(rdr("НомерОси"))
                        arrGraphParametersByParameter(I).IsTailVisible = True
                        Exit For
                    End If
                Next

                plot.LineColor = ColorsNet(CInt(rdr("НомерЦвета")))
                plot.LineStyle = LineStyle.Solid

                If FormParrent.Manager.IsGraphParameterByTime Then
                    plot.PointStyle = PointStyle.None
                Else
                    plot.PointStyle = PointStyle.SolidDiamond
                    plot.PointColor = ScatterGraphParameter.YAxes(CInt(rdr("НомерОси"))).CaptionForeColor
                End If

                plot.YAxis = ScatterGraphParameter.YAxes(CInt(rdr("НомерОси")))
                tempPointAnnotation = New XYPointAnnotation With {
                    .ArrowColor = plot.LineColor,
                    .ArrowHeadStyle = ArrowStyle.SolidStealth,
                    .ArrowLineWidth = 1.0!,
                    .ArrowTailSize = New Size(20, 15),
                    .Caption = nameParameter,
                    .CaptionFont = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte)),
                    .CaptionForeColor = plot.LineColor,
                    .ShapeFillColor = Color.Red,
                    .ShapeSize = New Size(5, 5),
                    .ShapeStyle = ShapeStyle.Oval,
                    .ShapeZOrder = AnnotationZOrder.AbovePlot,
                    .XAxis = Me.XAxis1,
                    .YAxis = plot.YAxis
                }
                ScatterGraphParameter.Annotations.Add(tempPointAnnotation)

                'TempPointAnnotation.XPosition = arrТочкиX(I)
                ' смещение по моему в пикселях
                'TempPointAnnotation.CaptionAlignment = New NationalInstruments.UI.AnnotationCaptionAlignment(NationalInstruments.UI.BoundsAlignment.Auto, CSng(Math.Abs(maximumX - minimumX) * 0.01!), -CSng(Math.Abs(maximumY - minimumY) * 0.1!))
                'dblСмещениеХ = (XAxis1.Range.Maximum - XAxis1.Range.Minimum) / 50
                'TempPointAnnotation.CaptionAlignment=New NationalInstruments.UI.AnnotationCaptionAlignment(NationalInstruments.UI.BoundsAlignment.None,(xData(xData.Length - 1) + dblСмещениеХ, yData(yData.Length - 1) + (CWGraphParametr.YAxes(NAxes).Range.Maximum - CWGraphParametr.YAxes(NAxes).Range.Minimum) / 100))
                'TempPointAnnotation.CaptionAlignment = New NationalInstruments.UI.AnnotationCaptionAlignment(NationalInstruments.UI.BoundsAlignment.None, xoffset, yoffset)
                'TempPointAnnotation.YPosition = arrТочкиY(I)
                'TempPointAnnotation.SetPosition(xData(xData.Length - 1), yData(yData.Length - 1))
            Loop
            rdr.Close()

            If Me.plotsCount = 0 Then
                MessageBox.Show("Отсутствуют шлейфы параметров для прикрепленния к осям!" & vbCrLf & "Графики от параметров отображаться не будут.", "Считывание конфигурации графиков", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                mIsGraphAct = False
                Exit Sub
            Else
                XyCursor1.Plot = ScatterGraphParameter.Plots(0)
                XyCursor1.YPosition = XyCursor1.Plot.YAxis.Range.Minimum
            End If

            'CWGraphParametr.InteractionMode = NationalInstruments.UI.GraphInteractionModes.DragAnnotationCaption

            If Not FormParrent.Manager.IsGraphParameterByTime Then PopulatePlotsBound(cn)

            ' вернулись к графиками
            sSQL = "SELECT [КонфигурацииОтображения].[keyКонфигурацияОтображения], [КонфигурацииОтображения].[ИмяКонфигурации], ПараметрОтображения.* " & "FROM КонфигурацииОтображения RIGHT JOIN ПараметрОтображения ON [КонфигурацииОтображения].[keyКонфигурацияОтображения]=[ПараметрОтображения].[keyКонфигурацияОтображения] " & "WHERE ((([КонфигурацииОтображения].keyКонфигурацияОтображения)= " & FormParrent.Manager.KeyConfiguration & "));"
            Dim odaDataAdapter As OleDbDataAdapter
            Dim dtDataTable As New System.Data.DataTable
            Dim countTable As Integer

            odaDataAdapter = New OleDbDataAdapter(sSQL, cn)
            odaDataAdapter.Fill(dtDataTable)
            countTable = dtDataTable.Rows.Count

            For I = 0 To ScatterGraphParameter.YAxes.Count - 1
                ' просмотр всех графиков принадлежащих данной оси
                caption = vbNullString
                For J As Integer = 0 To countTable - 1
                    nameParameter = CStr(dtDataTable.Rows(J)("ИмяПараметра"))
                    If ScatterGraphParameter.YAxes(I).Tag.ToString = CStr(dtDataTable.Rows(J)("НомерОси")) Then
                        caption &= nameParameter & "  "
                    End If
                Next J
                ScatterGraphParameter.YAxes(I).Caption = caption
            Next I

            mIsGraphAct = True
        Else
            MessageBox.Show("Отсутствует запись с индексом № " & FormParrent.Manager.KeyConfiguration.ToString & " в таблице КонфигурацииОтображения." & vbCrLf &
                            "Индекс записан в файле настроек .XML в ключе <keyКонфигурацияОтображения>keyКонфигурацияОтображения</keyКонфигурацияОтображения>", "Процедура <ВосстановитьРежимГрафиков>", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        cn.Close()
    End Sub

    ''' <summary>
    ''' Нарисовать Границы
    ''' </summary>
    ''' <param name="cn"></param>
    Private Sub PopulatePlotsBound(ByRef cn As OleDbConnection)
        Dim sSQL As String
        Dim rdr As OleDbDataReader
        Dim cmd As OleDbCommand = cn.CreateCommand
        cmd.CommandType = CommandType.Text
        Dim I, numberAxis, keyNameParameter As Integer
        Dim nameParameter As String
        Dim Color As String = Nothing
        Dim LineStyle As String = Nothing

        For I = 0 To UBound(arrGraphParametersByParameter)
            If arrGraphParametersByParameter(I).IsTailVisible Then
                nameParameter = arrGraphParametersByParameter(I).NameParameter
                numberAxis = arrGraphParametersByParameter(I).NumberAxis
                ' узнать все имена границ
                sSQL = "SELECT ПараметрДляГраниц.keyИмяПараметра, Plots.NamePlot " &
                "FROM ПараметрДляГраниц RIGHT JOIN Plots ON ПараметрДляГраниц.keyИмяПараметра = Plots.keyИмяПараметра " &
                "WHERE (((ПараметрДляГраниц.ИмяПараметра)='" & nameParameter & "'));"
                cmd.CommandText = sSQL
                rdr = cmd.ExecuteReader

                If Not rdr.HasRows Then
                    rdr.Close()
                Else
                    ' добавить имена Plots в коллекцию
                    Dim NamePlots As New List(Of String)
                    Do While rdr.Read
                        NamePlots.Add(CStr(rdr("NamePlot")))
                        keyNameParameter = CInt(rdr("keyИмяПараметра"))
                    Loop
                    rdr.Close()

                    If NamePlots.Count <> 0 Then
                        ' по каждой границе считать точки
                        For Each itemNamePlot As String In NamePlots
                            sSQL = "SELECT Plots.Color, Plots.LineStyle, Точки.НомерТочки, Точки.X, Точки.Y " &
                            "FROM ПараметрДляГраниц RIGHT JOIN (Plots RIGHT JOIN Точки ON Plots.keyPlot = Точки.keyPlot) ON ПараметрДляГраниц.keyИмяПараметра = Plots.keyИмяПараметра " &
                            "WHERE(((ПараметрДляГраниц.keyИмяПараметра) = " & keyNameParameter.ToString & ") And ((Plots.NamePlot) = '" & itemNamePlot & "')) " &
                            "ORDER BY Точки.НомерТочки;"
                            cmd.CommandText = sSQL
                            rdr = cmd.ExecuteReader

                            If rdr.HasRows Then
                                Dim arrXData As New List(Of Double)
                                Dim arrYData As New List(Of Double)

                                Do While rdr.Read
                                    Color = CStr(rdr("Color"))
                                    LineStyle = CStr(rdr("LineStyle"))
                                    arrXData.Add(CDbl(rdr("X")))
                                    arrYData.Add(CDbl(rdr("Y")))
                                Loop
                                AddPlotBound(itemNamePlot, numberAxis, arrXData.ToArray, arrYData.ToArray, Color, LineStyle) ' intНомерОси Mod 7)
                            End If

                            rdr.Close()
                        Next 'Each NamePlot
                    End If 'count
                End If 'Not rdr.HasRows
            End If 'arrГрафикКБПР(I).blnГрафик
        Next I 'UBound(arrГрафикКБПР)
    End Sub

    ''' <summary>
    ''' Добавить Plot Для Границ
    ''' </summary>
    ''' <param name="inNamePlot"></param>
    ''' <param name="inNAxes"></param>
    ''' <param name="xData"></param>
    ''' <param name="yData"></param>
    ''' <param name="inColor"></param>
    ''' <param name="inLineStyle"></param>
    Private Sub AddPlotBound(ByVal inNamePlot As String, ByVal inNAxes As Integer, ByRef xData() As Double, ByRef yData() As Double, ByVal inColor As String, ByVal inLineStyle As String) 'ByVal NColors As Integer)
        Dim plot As ScatterPlot
        Dim xoffset As Single = 10
        Dim yoffset As Single = -20

        With ScatterGraphParameter
            plot = New ScatterPlot
            ScatterGraphParameter.Plots.Add(plot)
            plot.LineColor = Color.FromName(inColor)

            'plot.LineStyle = NationalInstruments.UI.LineStyle.Solid
            Dim values As Array = EnumObject.GetValues(plot.LineStyle.UnderlyingType)
            ' по умолчанию
            Dim valueTemp As LineStyle = LineStyle.Dot

            For I As Integer = 0 To values.Length - 1
                If values.GetValue(I).ToString = inLineStyle Then
                    valueTemp = CType(values.GetValue(I), LineStyle)
                    Exit For
                End If
            Next

            plot.LineStyle = CType(valueTemp, LineStyle)
            plot.PointColor = .YAxes(inNAxes).CaptionForeColor
            plot.PointStyle = PointStyle.EmptyDiamond
            plot.XAxis = Me.XAxis1
            plot.YAxis = .YAxes(inNAxes)
            plot.PlotXY(xData, yData)

            tempPointAnnotation = New XYPointAnnotation With {
                .ArrowColor = plot.LineColor,
                .ArrowHeadStyle = ArrowStyle.SolidStealth,
                .ArrowLineWidth = 1.0!,
                .ArrowTailSize = New Size(20, 15),
                .Caption = inNamePlot,
                .CaptionAlignment = New AnnotationCaptionAlignment(BoundsAlignment.None, xoffset, yoffset),
                .CaptionFont = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte)),
                .CaptionForeColor = plot.LineColor,
                .ShapeFillColor = Color.Red,
                .ShapeSize = New Size(5, 5),
                .ShapeStyle = ShapeStyle.Oval,
                .ShapeZOrder = AnnotationZOrder.AbovePlot,
                .XAxis = Me.XAxis1
            }
            tempPointAnnotation.YAxis = .YAxes(inNAxes)
            ' стрелку на последнюю точку
            tempPointAnnotation.SetPosition(xData(xData.Length - 1), yData(yData.Length - 1))
            .Annotations.Add(tempPointAnnotation)

            'смещение по моему в пикселях
            'TempPointAnnotation.CaptionAlignment = New NationalInstruments.UI.AnnotationCaptionAlignment(NationalInstruments.UI.BoundsAlignment.Auto, CSng(Math.Abs(maximumX - minimumX) * 0.01!), -CSng(Math.Abs(maximumY - minimumY) * 0.1!))
            'dblСмещениеХ = (XAxis1.Range.Maximum - XAxis1.Range.Minimum) / 50
            'TempPointAnnotation.CaptionAlignment=New NationalInstruments.UI.AnnotationCaptionAlignment(NationalInstruments.UI.BoundsAlignment.None,(xData(xData.Length - 1) + dblСмещениеХ, yData(yData.Length - 1) + (CWGraphParametr.YAxes(NAxes).Range.Maximum - CWGraphParametr.YAxes(NAxes).Range.Minimum) / 100))
        End With
    End Sub

    ''' <summary>
    ''' Настроить Отображение Параметров
    ''' </summary>
    Public Sub TuneViewParameters()
        If FormParrent.Manager.IsCheckPassed AndAlso FormParrent.Manager.IsCalculateEnable Then TextBoxCollect.Visible = True

        ' так же вызывается из FillCombo
        Dim I As Integer
        TuneListViewParameter()

        If FormParrent.Manager.IsGraphParameterByTime = True Then
            arraysizeGraphParameter = arraySize 'CInt(arraysize / mФормаРодителя.Manager.СчетчикГрафикаПараметров)
            'ReDim_YdataParameters(plotsCount - 1, arraysizeGraphParameter)
            'ReDim_XdataParameters(arraysizeGraphParameter)
            Re.Dim(YdataParameters, plotsCount - 1, arraysizeGraphParameter)
            Re.Dim(XdataParameters, arraysizeGraphParameter)

            For I = 0 To arraysizeGraphParameter
                XdataParameters(I) = I
            Next

            With ScatterGraphParameter
                '.Cursors(0).Visible = True
                XyCursor1.Visible = True
                'Me.XAxis1.AutoMinorDivisionFrequency = 5
                'Me.XAxis1.MajorDivisions.GridVisible = True
                'Me.XAxis1.MinorDivisions.GridVisible = True
                Me.XAxis1.Range = New Range(0, arraysizeGraphParameter)

                Me.YAxis1.AutoMinorDivisionFrequency = 5
                Me.YAxis1.CaptionForeColor = Color.White
                Me.YAxis1.MajorDivisions.GridVisible = True
                Me.YAxis1.MajorDivisions.LabelForeColor = Color.White
                Me.YAxis1.MajorDivisions.TickColor = Color.White
                Me.YAxis1.MinorDivisions.GridVisible = True
                Me.YAxis1.MinorDivisions.TickColor = Color.White
                'Me.YAxis1.Range = New NationalInstruments.UI.Range(0, 100)
            End With

            cursorPosition = 0
        Else
            'ReDim_isErasePlots(plotsCount - 1)
            'ReDim_YdataParameters(plotsCount - 1, FormParrent.Manager.CounterTailGraphByParameters - 1)
            Re.Dim(isErasePlots, plotsCount - 1)
            Re.Dim(YdataParameters, plotsCount - 1, FormParrent.Manager.CounterTailGraphByParameters - 1)
            isEraseParametersByParameter = True
            'ReDim_XdataParameters(FormParrent.Manager.CounterTailGraphByParameters - 1)
            Re.Dim(XdataParameters, FormParrent.Manager.CounterTailGraphByParameters - 1)

            With ScatterGraphParameter
                '.Cursors(0).Visible = False
                XyCursor1.Visible = False

                Me.YAxis1.AutoMinorDivisionFrequency = 5
                'Me.YAxis1.CaptionForeColor = System.Drawing.Color.White
                Me.YAxis1.MajorDivisions.GridVisible = True
                'Me.YAxis1.MajorDivisions.LabelForeColor = System.Drawing.Color.White
                'Me.YAxis1.MajorDivisions.TickColor = System.Drawing.Color.White
                Me.YAxis1.MinorDivisions.GridVisible = True
                'Me.YAxis1.MinorDivisions.TickColor = System.Drawing.Color.White

                'dblСмещениеХ = (.YAxes(0).Range.Maximum - .YAxes(0).Range.Minimum) / 50
            End With
        End If
    End Sub

#End Region

    ''' <summary>
    ''' Рисовать Графики После Расчета
    ''' </summary>
    Public Sub UpdateGraph()
        Dim intN As Integer = FormParrent.Manager.CounterTailGraphByParameters - 1
        Dim numberPlot, index As Integer
        Dim average As Double ' Среднее 

        PopulateArrayGraphParametersByParameter()
        ListViewParameter.BeginUpdate()

        With ListViewParameter ' обновить лист
            For J = 0 To UBound(arrGraphParametersByParameter)
                If isDetailedList Then
                    .Items(J).SubItems(1).Text = CStr(arrGraphParametersByParameter(J).Value)
                Else
                    ' лист содержит список параметров имеющих шлейфы 
                    If arrGraphParametersByParameter(J).IsTailVisible Then
                        .Items(index).SubItems(1).Text = CStr(arrGraphParametersByParameter(J).Value)
                        index += 1
                    End If
                End If

                If FormParrent.Manager.IsGraphParameterByTime Then
                    If arrGraphParametersByParameter(J).NumberTail <> -1 Then ' значит ему был привязан шлейф
                        average = arrGraphParametersByParameter(J).Value
                        YdataParameters(arrGraphParametersByParameter(J).NumberTail, cursorPosition) = average

                        With ScatterGraphParameter
                            tempPointAnnotation = CType(.Annotations(arrGraphParametersByParameter(J).NumberTail), XYPointAnnotation)
                            tempPointAnnotation.SetPosition(cursorPosition, average)
                            tempPointAnnotation.Caption = $"{arrGraphParametersByParameter(J).NameParameter}: {Round(average, Precision).ToString}"
                            tempPointAnnotation.CaptionAlignment = New AnnotationCaptionAlignment(BoundsAlignment.None, 20, -5)
                        End With
                    End If
                Else
                    If nameParameterOfAxisX = arrGraphParametersByParameter(J).NameParameter Then
                        average = correctedParameterValueAxisX

                        If isEraseParametersByParameter Then
                            For N As Integer = 0 To intN
                                XdataParameters(N) = average
                            Next
                            isEraseParametersByParameter = False
                        End If

                        ' сдвиг массива
                        For N As Integer = 0 To intN - 1
                            XdataParameters(N) = XdataParameters(N + 1)
                        Next

                        ' добавить к последнему элементу в массиве
                        XdataParameters(intN) = average '=график который на ось х
                    End If

                    If arrGraphParametersByParameter(J).NumberTail <> -1 Then
                        ' сдвиг массива
                        numberPlot = arrGraphParametersByParameter(J).NumberTail
                        average = arrGraphParametersByParameter(J).Value

                        If isErasePlots(numberPlot) = False Then ' был очищен массив
                            For N As Integer = 0 To intN
                                YdataParameters(numberPlot, N) = average
                            Next

                            isErasePlots(numberPlot) = True
                        End If

                        For N As Integer = 0 To intN - 1
                            YdataParameters(numberPlot, N) = YdataParameters(numberPlot, N + 1)
                        Next

                        ' добавить к последнему элементу в массиве
                        YdataParameters(numberPlot, intN) = average
                    End If
                End If
            Next J
        End With

        ' включить обновление элемента
        ListViewParameter.EndUpdate()

        If FormParrent.Manager.IsGraphParameterByTime Then
            XyCursor1.XPosition = cursorPosition
            ScatterGraphParameter.PlotXYMultiple(XdataParameters, YdataParameters)
            cursorPosition += 1

            If cursorPosition > arraysizeGraphParameter Then cursorPosition = 0
        Else
            For J = 1 To UBound(arrGraphParametersByParameter)
                numberPlot = arrGraphParametersByParameter(J).NumberTail

                If numberPlot <> -1 Then
                    average = YdataParameters(numberPlot, intN)

                    With ScatterGraphParameter
                        tempPointAnnotation = CType(.Annotations(arrGraphParametersByParameter(J).NumberTail), XYPointAnnotation)
                        tempPointAnnotation.SetPosition(XdataParameters(intN), average)
                        tempPointAnnotation.Caption = $"{arrGraphParametersByParameter(J).NameParameter}: {Round(average, Precision).ToString}"
                        tempPointAnnotation.CaptionAlignment = New AnnotationCaptionAlignment(BoundsAlignment.None, 20, -5)
                    End With
                End If
            Next

            ScatterGraphParameter.PlotXYMultiple(XdataParameters, YdataParameters)
        End If
    End Sub

    ''' <summary>
    ''' Заполнить Структуру Для Графика
    ''' </summary>
    Private Sub PopulateArrayGraphParametersByParameter()
        ' вызывается из цикла подсчета НакопитьЗначенияИзмеренныхИРасчетныхПараметров
        Dim I As Integer

        For Each rowИзмеренныйПараметр As BaseFormDataSet.ИзмеренныеПараметрыRow In FormParrent.Manager.MeasurementDataTable.Rows
            arrGraphParametersByParameter(I).Value = Round(rowИзмеренныйПараметр.ИзмеренноеЗначение, 3) ' + 10 * Rnd
            If FormParrent.Manager.IsGraphParameterByTime = False AndAlso rowИзмеренныйПараметр.ИмяПараметра = nameParameterOfAxisX Then correctedParameterValueAxisX = rowИзмеренныйПараметр.ИзмеренноеЗначение
            I += 1
        Next

        For Each rowРасчетныйПараметр As BaseFormDataSet.РасчетныеПараметрыRow In FormParrent.Manager.CalculatedDataTable.Rows
            arrGraphParametersByParameter(I).Value = Round(rowРасчетныйПараметр.ВычисленноеПереведенноеЗначение, 3) ' + 10 * Rnd
            If FormParrent.Manager.IsGraphParameterByTime = False AndAlso rowРасчетныйПараметр.ИмяПараметра = nameParameterOfAxisX Then correctedParameterValueAxisX = rowРасчетныйПараметр.ВычисленноеПереведенноеЗначение
            I += 1
        Next
    End Sub

    Private Sub ListViewResize()
        If Me.IsHandleCreated Then
            With ListViewParameter
                .Columns(0).Width = .Width * 5 \ 10
                .Columns(1).Width = .Width * 4 \ 10
                .Columns(2).Width = .Width - .Columns(0).Width - .Columns(1).Width - 8
            End With
        End If
    End Sub

    Private Sub FormGraf_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        ListViewResize()
    End Sub
End Class

