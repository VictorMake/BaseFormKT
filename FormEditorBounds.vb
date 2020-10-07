Imports System.Data.OleDb
Imports System.Globalization
Imports System.Threading
Imports System.Windows.Forms

''' <summary>
''' Редактор графиков параметр от параметра
''' </summary>
Friend Class FormEditorBounds
    ''' <summary>
    ''' Таблица Обновления
    ''' </summary>
    Private Enum WhatTableRowUpdate
        ParameterForBound ' Параметр Для Границ
        Plots
    End Enum
    Private mWhatTableRowUpdate As WhatTableRowUpdate

    ''' <summary>
    ''' были Изменения В Точках
    ''' </summary>
    Private isDirtyCurrentPoints As Boolean
    Private isFormClosing As Boolean
    ''' <summary>
    ''' ' были изменения в текущем графике
    ''' </summary>
    Private isDirtyCurrentGraph As Boolean
    ''' <summary>
    ''' индекс текущей редактируемой линии
    ''' </summary>
    Private indexSelectedLine As Integer
    Private connection As OleDbConnection
    Private keyNew As Integer
    Private XAxis1 As XAxis
    Private YAxis1 As YAxis
    Private TempPointAnnotation As XYPointAnnotation
    Private mFormParrent As FormGraf

    Private Const NameColumnIndex As String = "ColumnIndex"
    Private Const NameColumnValueX As String = "ColumnValueX"
    Private Const NameColumnValueY As String = "ColumnValueY"
    Private Const propertyLineStyle As String = "LineStyle"
    Private Const propertyLineColor As String = "LineColor"

    Private patternScatterPlot As ScatterPlot

    Public Sub New(ByVal FormParrent As FormGraf)
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        mFormParrent = FormParrent
    End Sub

    Private Sub FormEditorBounds_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        For Each row As BaseFormDataSet.РасчетныеПараметрыRow In mFormParrent.FormParrent.Manager.CalculatedDataTable.Rows
            ComboBoxParameters.Items.Add(row.ИмяПараметра)
        Next

        For Each row As BaseFormDataSet.ИзмеренныеПараметрыRow In mFormParrent.FormParrent.Manager.MeasurementDataTable.Rows
            ComboBoxParameters.Items.Add(row.ИмяПараметра)
        Next

        ComboBoxParameters.SelectedIndex = 0
        mFormParrent.ScatterGraphParameter.Annotations.Clear()
        mFormParrent.ScatterGraphParameter.Plots.Clear()

        ' удалить оси оставить 1 последние
        For I As Integer = mFormParrent.ScatterGraphParameter.YAxes.Count - 1 To 1 Step -1
            mFormParrent.ScatterGraphParameter.YAxes.RemoveAt(I)
        Next

        mFormParrent.ScatterGraphParameter.XAxes(0).Caption = ""
        mFormParrent.ScatterGraphParameter.YAxes(0).Caption = ""

        PopulateListBounds()
        isDirtyCurrentGraph = False

        XAxis1 = mFormParrent.ScatterGraphParameter.XAxes(0)
        YAxis1 = mFormParrent.ScatterGraphParameter.YAxes(0)
        XAxis1.Mode = AxisMode.AutoScaleLoose
        YAxis1.Mode = AxisMode.AutoScaleLoose
        XAxis1.Visible = True

        patternScatterPlot = New ScatterPlot With {
            .LineColor = Drawing.Color.White,
            .PointColor = Drawing.Color.Red,
            .PointStyle = PointStyle.EmptyDiamond,
            .XAxis = mFormParrent.ScatterGraphParameter.XAxes(0),
            .YAxis = mFormParrent.ScatterGraphParameter.YAxes(0)
        }
        mFormParrent.ScatterGraphParameter.Plots.Add(patternScatterPlot)

        PropertyEditorPlotColor.Source = New PropertyEditorSource(patternScatterPlot, propertyLineColor)
        PropertyEditorPlotStile.Source = New PropertyEditorSource(patternScatterPlot, propertyLineStyle)
        mFormParrent.TSButtonTuneTrand.Enabled = False
        mFormParrent.TSButtonBounds.Enabled = False
    End Sub

    Private Sub FormEditorBounds_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        If isDirtyCurrentGraph Then
            If MessageBox.Show($"Были изменения при редактировании текущего графика.{vbCrLf}Произвести сохранение изменений?",
                               "Сохранение изменений", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                UpdateLimitationGraphs()
                SaveDBaseGraphs()
            End If
        End If

        XAxis1.Mode = AxisMode.Fixed
        YAxis1.Mode = AxisMode.Fixed

        isFormClosing = True
        ' назначить дурь чтобы не было ошибки
        PropertyEditorPlotColor.Source = New PropertyEditorSource(Me.ButtonAddNewLimitationGraph, "Text")
        PropertyEditorPlotStile.Source = New PropertyEditorSource(Me.ButtonAddNewLimitationGraph, "Text")

        mFormParrent.TSButtonTuneTrand.Enabled = True
        mFormParrent.TSButtonBounds.Enabled = True
        mFormParrent.TSButtonBounds.Checked = False
        mFormParrent = Nothing
    End Sub

    Private Sub DataGridViewTablePoit_RowValidating(ByVal sender As Object, ByVal e As DataGridViewCellCancelEventArgs) Handles DataGridViewTablePoit.RowValidating
        Dim X, Y As Double
        Dim XObj, YObj As Object

        If e.ColumnIndex > 0 AndAlso e.RowIndex > 0 Then
            isDirtyCurrentGraph = True
            indexSelectedLine = ListBoxLimitationGraphs.SelectedIndex
            XObj = DataGridViewTablePoit.Rows(e.RowIndex).Cells(NameColumnValueX).Value
            YObj = DataGridViewTablePoit.Rows(e.RowIndex).Cells(NameColumnValueY).Value

            If XObj IsNot Nothing Then
                Try
                    X = Double.Parse(XObj.ToString, CultureInfo.CurrentCulture)
                Catch ex As FormatException
                    'Throw New FormatException("Значение X недопустимо", ex)
                    MessageBox.Show($"Значение X= {XObj} недопустимо", "Ввод данных", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    e.Cancel = True
                End Try
            Else
                MessageBox.Show("Необходимо ввести Значение X", "Ввод данных", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            If YObj IsNot Nothing Then
                Try
                    Y = Double.Parse(YObj.ToString, CultureInfo.CurrentCulture)
                Catch ex As FormatException
                    MessageBox.Show($"Значение Y= {YObj} недопустимо", "Ввод данных", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    'Throw New FormatException("Значение Y недопустимо", ex)
                    e.Cancel = True
                End Try
            Else
                MessageBox.Show("Необходимо ввести Значение Y", "Ввод данных", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If

        ShowMessageOnStatusPanel($"Введено X={X} Y={Y}")
    End Sub

    Private Sub DataGridViewTablePoit_UserAddedRow(ByVal sender As Object, ByVal e As DataGridViewRowEventArgs) Handles DataGridViewTablePoit.UserAddedRow
        DataGridViewTablePoit.Rows(e.Row.Index - 1).Cells(0).Value = DataGridViewTablePoit.RowCount - 1
        isDirtyCurrentPoints = True
        isDirtyCurrentGraph = True
        indexSelectedLine = ListBoxLimitationGraphs.SelectedIndex
    End Sub

    Private Sub DataGridViewTablePoit_UserDeletedRow(ByVal sender As Object, ByVal e As DataGridViewRowEventArgs) Handles DataGridViewTablePoit.UserDeletedRow
        For I As Integer = 0 To DataGridViewTablePoit.RowCount - 2
            DataGridViewTablePoit.Rows(I).Cells(0).Value = (I + 1).ToString
        Next

        isDirtyCurrentPoints = True
        isDirtyCurrentGraph = True
        indexSelectedLine = ListBoxLimitationGraphs.SelectedIndex
    End Sub

    Private Sub ButtonEditLimitationGraph_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonEditLimitationGraph.Click
        UpdateLimitationGraphs()
        isDirtyCurrentGraph = True
    End Sub

    Private Sub ButtonSaveGraph_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveGraph.Click
        UpdateLimitationGraphs()
        SaveDBaseGraphs()
        isDirtyCurrentGraph = False
    End Sub

    Private Sub ButtonLoadGraph_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonLoadGraph.Click
        LoadGraph()
    End Sub

    Private Sub ButtonAddGraph_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddGraph.Click
        AddGraph()
    End Sub

    Private Sub ButtonDeleteGraph_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteGraph.Click
        DeleteGraph()
    End Sub

    Private Sub LoadGraph()
        ' если читатьУсловие доступна, значит было выделение в листе
        Dim keyParameterName As Integer
        Dim namePlot As String
        Dim sSQL As String
        Dim rdr As OleDbDataReader
        Dim cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.FormParrent.Manager.PathKT))
        Dim cmd As OleDbCommand = cn.CreateCommand
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow

        ' очистка
        DataGridViewTablePoit.Rows.Clear()
        indexSelectedLine = -1
        ListBoxLimitationGraphs.Items.Clear()

        ' узнать все имена границ
        sSQL = "SELECT ПараметрДляГраниц.keyИмяПараметра, Plots.* " &
        "FROM ПараметрДляГраниц RIGHT JOIN Plots ON ПараметрДляГраниц.keyИмяПараметра = Plots.keyИмяПараметра " &
        "WHERE (((ПараметрДляГраниц.ИмяПараметра)='" & TextBoxParameterWithPlot.Text & "'));"
        cn.Open()
        odaDataAdapter = New OleDbDataAdapter(sSQL, cn)
        odaDataAdapter.Fill(dtDataTable)

        If dtDataTable.Rows.Count = 0 Then
            cn.Close()
        Else
            mFormParrent.ScatterGraphParameter.Annotations.Clear()
            ' удалить все Plot кроме первого в коллекции - это по сути ScatterPlot1 добавленный в frmРедакторГрафиковОтПараметров_Load
            For I As Integer = mFormParrent.ScatterGraphParameter.Plots.Count - 1 To 1 Step -1
                mFormParrent.ScatterGraphParameter.Plots.RemoveAt(I)
            Next
            ' Заполнить лист. Индицировать, какой из членов добавленных элементов 
            ' в ListBox.DataSource должен быть показан
            ListBoxLimitationGraphs.DisplayMember = "ИмяГрафикаГраниц"

            ' добавить имена Plots в коллекцию
            For Each drDataRow In dtDataTable.Rows
                namePlot = CStr(drDataRow("NamePlot"))
                keyParameterName = CInt(drDataRow("Plots.keyИмяПараметра"))
                ' по каждой границе считать точки
                sSQL = "SELECT Точки.НомерТочки, Точки.X, Точки.Y " &
                "FROM ПараметрДляГраниц RIGHT JOIN (Plots RIGHT JOIN Точки ON Plots.keyPlot = Точки.keyPlot) ON ПараметрДляГраниц.keyИмяПараметра = Plots.keyИмяПараметра " &
                "WHERE(((ПараметрДляГраниц.keyИмяПараметра) = " & keyParameterName.ToString & ") And ((Plots.NamePlot) = '" & namePlot & "')) " &
                "ORDER BY Точки.НомерТочки;"

                cmd.CommandType = CommandType.Text
                cmd.CommandText = sSQL
                rdr = cmd.ExecuteReader

                If rdr.HasRows Then
                    Dim arrXData As New List(Of Double)
                    Dim arrYData As New List(Of Double)

                    Do While rdr.Read
                        arrXData.Add(CDbl(rdr("X")))
                        arrYData.Add(CDbl(rdr("Y")))
                    Loop

                    AddScatterPlotForBound(namePlot, CStr(drDataRow("Color")), CStr(drDataRow(propertyLineStyle)), arrXData.ToArray, arrYData.ToArray)
                End If
                rdr.Close()
            Next

            cn.Close()
        End If

        If ListBoxLimitationGraphs.Items.Count > 0 Then
            ListBoxLimitationGraphs.SelectedIndex = ListBoxLimitationGraphs.Items.Count - 1
        End If

        ' проверка наличия параметра с именем считанным из базы
        If Not ComboBoxParameters.Items.Contains(TextBoxParameterWithPlot.Text) Then
            MessageBox.Show($"Параметра с именем {TextBoxParameterWithPlot.Text} не существует!{vbCrLf}Выберите параметр из списка.",
                            "Проверка имени параметра", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            TextBoxParameterWithPlot.Text = CStr(ComboBoxParameters.Items(0))
        Else
            ComboBoxParameters.SelectedIndex = ComboBoxParameters.FindString(TextBoxParameterWithPlot.Text)
        End If

        ButtonSaveGraph.Enabled = True
        ButtonDeleteGraph.Enabled = True
        ButtonClear.Enabled = True
        isDirtyCurrentGraph = False
    End Sub

    ''' <summary>
    ''' Добавить Plot Для Границ
    ''' </summary>
    ''' <param name="namePlot"></param>
    ''' <param name="color"></param>
    ''' <param name=propertyLineStyle></param>
    ''' <param name="xData"></param>
    ''' <param name="yData"></param>
    Private Sub AddScatterPlotForBound(ByVal namePlot As String, ByVal color As String, ByVal lineStyle As String, ByRef xData() As Double, ByRef yData() As Double)
        Dim newCurveGraph As CurveGraph = New CurveGraph(namePlot) With {.Color = color, .LineStyle = lineStyle}

        For I As Integer = 0 To xData.Length - 1
            newCurveGraph.Add(I + 1, xData(I), yData(I))
        Next

        ListBoxLimitationGraphs.Items.Add(newCurveGraph)

        Dim plot As ScatterPlot = New ScatterPlot With {
            .LineColor = Drawing.Color.FromName(color),
            .PointColor = Drawing.Color.Red,
            .PointStyle = PointStyle.EmptyDiamond
        }

        Dim values As Array = EnumObject.GetValues(patternScatterPlot.LineStyle.UnderlyingType)
        ' по умолчанию
        Dim tempValue As LineStyle = UI.LineStyle.Dot

        For I As Integer = 0 To values.Length - 1
            If values.GetValue(I).ToString = lineStyle Then
                tempValue = CType(values.GetValue(I), LineStyle)
                Exit For
            End If
        Next

        plot.LineStyle = CType(tempValue, LineStyle)
        plot.XAxis = Me.XAxis1
        plot.YAxis = Me.YAxis1
        plot.Tag = namePlot

        mFormParrent.ScatterGraphParameter.Plots.Add(plot)
        'Plot.LineColor = ColorsNet(NColors)
        'Plot.LineStyle = NationalInstruments.UI.LineStyle.Solid
        'Plot.PointColor = .YAxes(NAxes).CaptionForeColor
        'Plot.PointStyle = NationalInstruments.UI.PointStyle.EmptyDiamond
        'Plot.XAxis = Me.XAxis1
        'Plot.YAxis = .YAxes(NAxes)

        plot.PlotXY(xData, yData)
        AddXYPointAnnotation(plot, xData(xData.Length - 1), yData(yData.Length - 1))
    End Sub

    ''' <summary>
    ''' Добавить аннотацию к кривой
    ''' </summary>
    ''' <param name="plot"></param>
    ''' <param name="X"></param>
    ''' <param name="Y"></param>
    Private Sub AddXYPointAnnotation(ByRef plot As ScatterPlot, ByVal X As Double, ByVal Y As Double)
        Const xoffset As Single = 10
        Const yoffset As Single = -20

        TempPointAnnotation = New XYPointAnnotation With {
            .ArrowColor = plot.LineColor,
            .ArrowHeadStyle = ArrowStyle.SolidStealth,
            .ArrowLineWidth = 1.0!,
            .ArrowTailSize = New Drawing.Size(20, 15),
            .Caption = plot.Tag.ToString, ' NamePlot
            .CaptionAlignment = New AnnotationCaptionAlignment(BoundsAlignment.None, xoffset, yoffset),
            .CaptionFont = New Drawing.Font("Microsoft Sans Serif", 8.25!, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point, CType(0, Byte)),
            .CaptionForeColor = plot.LineColor,
            .ShapeFillColor = Drawing.Color.Red,
            .ShapeSize = New Drawing.Size(5, 5),
            .ShapeStyle = ShapeStyle.Oval,
            .ShapeZOrder = AnnotationZOrder.AbovePlot,
            .XAxis = Me.XAxis1,
            .YAxis = Me.YAxis1
            }
        TempPointAnnotation.SetPosition(X, Y)
        mFormParrent.ScatterGraphParameter.Annotations.Add(TempPointAnnotation)
    End Sub

    Private Sub AddGraph()
        Dim nameParameter As String = TextBoxParameterWithPlot.Text

        If nameParameter <> "" Then
            ' если имя новое, то новое Условие добавляется
            If Not ListBoxParametersWithPlot.Items.Contains(nameParameter) Then ' узла нет, значит можно записать новый
                SaveDBaseGraphs()
            Else
                MessageBox.Show($"Набор границ ограничений для параметра {nameParameter} уже существует!{vbCrLf}Необходимо ввести новое имя.",
                                "Новый набор ограничений", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ShowMessageOnStatusPanel("Повтор имени ограничений")
            End If

            isDirtyCurrentGraph = False
        Else
            Const text As String = "Необходимо ввести имя набора границ ограничений!"
            MessageBox.Show(text, "Новый график ограничения", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ShowMessageOnStatusPanel(text)
        End If
    End Sub

    Private Sub DeleteGraph()
        Dim strSQL As String
        Dim cn As OleDbConnection = Nothing
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim cb As OleDbCommandBuilder
        Dim nameParameter As String = TextBoxParameterWithPlot.Text
        Dim success As Boolean

        If nameParameter <> "" Then
            Dim message As String = $"Удаление набора границ {nameParameter} произведено успешно"
            If ListBoxParametersWithPlot.Items.Contains(nameParameter) Then
                success = True
            End If

            If success Then
                strSQL = $"SELECT ПараметрДляГраниц.* FROM ПараметрДляГраниц WHERE ИмяПараметра = '{nameParameter}'"
                Try
                    cn = New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.FormParrent.Manager.PathKT))
                    cn.Open()
                    odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
                    odaDataAdapter.Fill(dtDataTable)

                    If dtDataTable.Rows.Count > 0 Then
                        dtDataTable.Rows(0).Delete()
                        cb = New OleDbCommandBuilder(odaDataAdapter)
                        odaDataAdapter.Update(dtDataTable)
                        Thread.Sleep(1000)
                    End If
                Catch ex As Exception
                    MessageBox.Show(ex.ToString)
                Finally
                    If cn.State = ConnectionState.Open Then
                        cn.Close()
                    End If

                    PopulateListBounds()
                    TextBoxParameterWithPlot.Text = ""
                    ButtonLoadGraph.Enabled = False
                    ButtonSaveGraph.Enabled = False
                    ButtonDeleteGraph.Enabled = False
                    ShowMessageOnStatusPanel(message)
                End Try
            End If

            isDirtyCurrentGraph = False
        End If
    End Sub

    ' Event procedure for OnRowUpdated
    Private Sub OnRowUpdated(ByVal sender As Object, ByVal args As OleDbRowUpdatedEventArgs)
        ' Include a variable and a command to retrieve the identity value from the Access database.
        Dim idCMD As OleDbCommand = New OleDbCommand("SELECT @@IDENTITY", connection)

        If args.StatementType = StatementType.Insert Then
            ' Retrieve the identity value and store it in the args column.
            keyNew = CInt(idCMD.ExecuteScalar())
            Select Case mWhatTableRowUpdate
                Case WhatTableRowUpdate.ParameterForBound
                    args.Row("keyИмяПараметра") = keyNew
                Case WhatTableRowUpdate.Plots
                    args.Row("keyPlot") = keyNew
            End Select
        End If
    End Sub

    Private Sub SaveDBaseGraphs()
        Dim strSQL As String
        Dim cn As OleDbConnection
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow
        Dim cb As OleDbCommandBuilder
        Dim nameParameter As String = TextBoxParameterWithPlot.Text
        Dim success As Boolean

        ' если имя пусто вопрос  "введите имя" и так как модально нельзя, то выход из данной процедуры
        If nameParameter <> "" Then
            ButtonLoadGraph.Enabled = False
            ButtonSaveGraph.Enabled = False
            ButtonDeleteGraph.Enabled = False
            ' если переписать то удаление из базы старых и запись новых
            ' если имя новое, то новое Условие добавляется
            If ListBoxParametersWithPlot.Items.Contains(nameParameter) Then
                success = True
            End If

            ' Открыть подключение
            cn = New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.FormParrent.Manager.PathKT))
            If success Then
                strSQL = $"SELECT ПараметрДляГраниц.* FROM ПараметрДляГраниц WHERE ИмяПараметра = '{nameParameter}'"
            Else
                strSQL = "SELECT ПараметрДляГраниц.* FROM ПараметрДляГраниц"
            End If

            Try
                cn.Open()
                connection = cn ' для события вставки новой строки
                odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
                odaDataAdapter.Fill(dtDataTable)

                If success Then ' удаляется старая группа
                    If dtDataTable.Rows.Count > 0 Then
                        dtDataTable.Rows(0).Delete()
                    End If
                Else ' добавить в ListBox и в массив
                    ListBoxParametersWithPlot.Items.Add(nameParameter)
                End If

                ' добавляется новая конфигурация или перезаписывается старая
                drDataRow = dtDataTable.NewRow
                drDataRow.BeginEdit()
                drDataRow("ИмяПараметра") = nameParameter  'имя
                drDataRow.EndEdit()
                dtDataTable.Rows.Add(drDataRow)
                ' подключить событие для автообновления значения
                mWhatTableRowUpdate = WhatTableRowUpdate.ParameterForBound
                AddHandler odaDataAdapter.RowUpdated, New OleDbRowUpdatedEventHandler(AddressOf OnRowUpdated)

                cb = New OleDbCommandBuilder(odaDataAdapter)
                odaDataAdapter.Update(dtDataTable)
                Thread.Sleep(1000)
                Dim keyNameParameter As Integer = keyNew

                ' в цикле по границам plots(по коллекции ГрафикГраниц) добавлять по одной границе 
                ' внутри цикл по точкам добавлять сразу все записи в таблицу (keyNew будет известен)
                strSQL = "SELECT Plots.* FROM Plots WHERE keyИмяПараметра = " & keyNameParameter.ToString
                ' на данный момент таблица не содержит записей
                odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
                dtDataTable = New DataTable
                odaDataAdapter.Fill(dtDataTable)

                For I As Integer = 0 To ListBoxLimitationGraphs.Items.Count - 1
                    Dim itemCurveGraph As CurveGraph = CType(ListBoxLimitationGraphs.Items(I), CurveGraph)

                    drDataRow = dtDataTable.NewRow
                    drDataRow.BeginEdit()
                    drDataRow("keyИмяПараметра") = keyNameParameter
                    drDataRow("NamePlot") = itemCurveGraph.NameCurveGraph
                    drDataRow("Color") = itemCurveGraph.Color
                    drDataRow(propertyLineStyle) = itemCurveGraph.LineStyle
                    drDataRow.EndEdit()
                    dtDataTable.Rows.Add(drDataRow)

                    If I = 0 Then
                        ' подключить обработчик события только один раз
                        ' Include an event to fill in the Autonumber value.
                        mWhatTableRowUpdate = WhatTableRowUpdate.Plots
                        AddHandler odaDataAdapter.RowUpdated, New OleDbRowUpdatedEventHandler(AddressOf OnRowUpdated)
                    End If

                    cb = New OleDbCommandBuilder(odaDataAdapter)
                    odaDataAdapter.Update(dtDataTable)
                    Thread.Sleep(500)

                    Dim odaDataAdapterPoints As OleDbDataAdapter
                    Dim dtDataTablePoints As New DataTable
                    Dim drDataRowPoint As DataRow
                    Dim cbPoints As OleDbCommandBuilder

                    ' keyNew содержит keyPlot
                    strSQL = $"SELECT Точки.* FROM(Точки) WHERE (Точки.keyPlot= {keyNew} );"
                    odaDataAdapterPoints = New OleDbDataAdapter(strSQL, cn)
                    dtDataTablePoints = New DataTable
                    odaDataAdapterPoints.Fill(dtDataTablePoints)

                    For Each itemPoint As CurveGraph.PointCurve In itemCurveGraph.GetPointsCurve.Values
                        drDataRowPoint = dtDataTablePoints.NewRow
                        drDataRowPoint.BeginEdit()
                        drDataRowPoint("keyPlot") = keyNew
                        drDataRowPoint("НомерТочки") = itemPoint.Index
                        drDataRowPoint("X") = itemPoint.X
                        drDataRowPoint("Y") = itemPoint.Y
                        drDataRowPoint.EndEdit()
                        dtDataTablePoints.Rows.Add(drDataRowPoint)
                    Next

                    cbPoints = New OleDbCommandBuilder(odaDataAdapterPoints)
                    odaDataAdapterPoints.Update(dtDataTablePoints)
                    Thread.Sleep(500)
                Next
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            Finally
                ' очистить
                connection = Nothing
                If cn.State = ConnectionState.Open Then
                    cn.Close()
                End If
            End Try

            isDirtyCurrentGraph = False
            ShowMessageOnStatusPanel("Сохранение изменений произведено успешно")
            ButtonLoadGraph.Enabled = True
            ButtonSaveGraph.Enabled = True
            ButtonDeleteGraph.Enabled = True
        Else
            Const text As String = "Необходимо ввести имя набора границ ограничений!"
            MessageBox.Show(text, "Новый график ограничения", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ShowMessageOnStatusPanel(text)
        End If
    End Sub

    Private Sub ComboBoxParameters_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxParameters.SelectedIndexChanged
        TextBoxParameterWithPlot.Text = ComboBoxParameters.Text
    End Sub

    Private Sub ListBoxLimitationGraphs_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ListBoxLimitationGraphs.SelectedIndexChanged
        If ListBoxLimitationGraphs.SelectedIndex = -1 Then Exit Sub

        ' явно устанавливаются при добавлении и удалении
        ' здесь колличество в ГрафикГраниц равно DataGridViewГрафикГраниц.Rows и сравнивать можно
        If Not isDirtyCurrentPoints Then CheckOnDirtyPoint()

        If isDirtyCurrentPoints Then
            If MessageBox.Show($"Были изменения в таблице задания точек линии.{vbCrLf}Произвести сохранение изменений?",
                               "Смена линии", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                UpdateLimitationGraphs()
            End If
        End If

        isDirtyCurrentPoints = False
        indexSelectedLine = -1

        Dim selectedCurveGraph As CurveGraph = CType(ListBoxLimitationGraphs.SelectedItem, CurveGraph)
        DataGridViewTablePoit.Rows.Clear()

        Dim tempScatterPlot As ScatterPlot = Nothing
        For Each ScatterPlotTempPlots As ScatterPlot In mFormParrent.ScatterGraphParameter.Plots
            If CStr(ScatterPlotTempPlots.Tag) = selectedCurveGraph.NameCurveGraph Then tempScatterPlot = ScatterPlotTempPlots

            ScatterPlotTempPlots.LineWidth = 1
        Next

        If tempScatterPlot IsNot Nothing Then
            For Each Точка As CurveGraph.PointCurve In selectedCurveGraph.GetPointsCurve.Values
                Dim heter_row As DataGridViewRow = New DataGridViewRow
                ' создаем строку, считывая описания колонок с _grid
                heter_row.CreateCells(DataGridViewTablePoit)

                heter_row.Cells(0).Value = CType(Точка.Index, Object)
                heter_row.Cells(1).Value = CType(Точка.X, Object)
                heter_row.Cells(2).Value = CType(Точка.Y, Object)
                DataGridViewTablePoit.Rows.Add(heter_row)
            Next

            tempScatterPlot.LineWidth = 2
            TextBoxNameNewLimitationGraph.Text = selectedCurveGraph.NameCurveGraph
            PropertyEditorPlotColor.Source = New PropertyEditorSource(tempScatterPlot, propertyLineColor)
            PropertyEditorPlotStile.Source = New PropertyEditorSource(tempScatterPlot, propertyLineStyle)
        End If

        ShowMessageOnStatusPanel($"График границы {selectedCurveGraph.NameCurveGraph} выбран для редактирования")
    End Sub

    ''' <summary>
    ''' Обновить График Границ
    ''' </summary>
    Private Sub UpdateLimitationGraphs()
        If ListBoxLimitationGraphs.SelectedIndex = -1 Then Exit Sub

        Dim tempCurveGraph As CurveGraph

        If isDirtyCurrentPoints Then
            If indexSelectedLine = -1 Then
                MessageBox.Show("Необходимо вначале добавить график границ!",
                "Новый график ограничения", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            tempCurveGraph = CType(ListBoxLimitationGraphs.Items(indexSelectedLine), CurveGraph)
        Else ' редактировать границу или добавить новую границу
            tempCurveGraph = CType(ListBoxLimitationGraphs.SelectedItem, CurveGraph)
        End If

        Dim X, Y As Double
        Dim index As Integer
        tempCurveGraph.Clear()

        Dim tempScatterPlot As ScatterPlot = Nothing
        ' поиск
        isDirtyCurrentPoints = False
        indexSelectedLine = -1

        For Each itemScatterPlot As ScatterPlot In mFormParrent.ScatterGraphParameter.Plots
            If CStr(itemScatterPlot.Tag) = tempCurveGraph.NameCurveGraph Then
                tempScatterPlot = itemScatterPlot
                Exit For
            End If
        Next

        If tempScatterPlot IsNot Nothing Then
            tempScatterPlot.ClearData()

            For Each row As DataGridViewRow In DataGridViewTablePoit.Rows
                If row.Index < DataGridViewTablePoit.Rows.Count - 1 Then
                    index = CInt(row.Cells(NameColumnIndex).Value)
                    X = CDbl(row.Cells(NameColumnValueX).Value)
                    Y = CDbl(row.Cells(NameColumnValueY).Value)
                    ' перезапись в коллекцию
                    tempScatterPlot.PlotXYAppend(X, Y)
                    tempCurveGraph.Add(index, X, Y)
                End If
            Next

            ' поиск Annotation
            For Each itemPlot As XYPointAnnotation In mFormParrent.ScatterGraphParameter.Annotations
                If itemPlot.Caption = tempCurveGraph.NameCurveGraph Then
                    TempPointAnnotation = itemPlot
                    TempPointAnnotation.SetPosition(X, Y) ' X, Y - это последние с цикла
                    Exit For
                End If
            Next
        End If

        ShowMessageOnStatusPanel("Обновления графика границ произведено успешно")
    End Sub

    Private Sub ButtonDeleteSelectedLimitationGraph_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteSelectedLimitationGraph.Click
        If ListBoxLimitationGraphs.SelectedIndex = -1 Then Exit Sub

        Dim tempГрафикГраниц As CurveGraph = CType(ListBoxLimitationGraphs.SelectedItem, CurveGraph)
        Dim ScatterPlotTemp As ScatterPlot = Nothing

        ' поиск
        For Each itemPlot As ScatterPlot In mFormParrent.ScatterGraphParameter.Plots
            If CStr(itemPlot.Tag) = tempГрафикГраниц.NameCurveGraph Then
                ScatterPlotTemp = itemPlot
                Exit For
            End If
        Next

        If ScatterPlotTemp IsNot Nothing Then
            ' поиск Annotation
            For Each itemAnnotation As XYPointAnnotation In mFormParrent.ScatterGraphParameter.Annotations
                If itemAnnotation.Caption = tempГрафикГраниц.NameCurveGraph Then
                    TempPointAnnotation = itemAnnotation
                    Exit For
                End If
            Next

            mFormParrent.ScatterGraphParameter.Annotations.Remove(TempPointAnnotation)
            ShowMessageOnStatusPanel($"График границ {tempГрафикГраниц.NameCurveGraph} удален")
            mFormParrent.ScatterGraphParameter.Plots.Remove(ScatterPlotTemp)
            ListBoxLimitationGraphs.Items.Remove(ListBoxLimitationGraphs.SelectedItem)
            DataGridViewTablePoit.Rows.Clear()
        End If

        ButtonClear.Enabled = ListBoxLimitationGraphs.Items.Count > 0
    End Sub

    Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonClear.Click
        DataGridViewTablePoit.Rows.Clear()
        mFormParrent.ScatterGraphParameter.Annotations.Clear()

        For I As Integer = mFormParrent.ScatterGraphParameter.Plots.Count - 1 To 1 Step -1
            mFormParrent.ScatterGraphParameter.Plots.RemoveAt(I)
        Next

        ListBoxLimitationGraphs.Items.Clear()
        ButtonClear.Enabled = False
    End Sub

    Private Sub ButtonAddNewLimitationGraph_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddNewLimitationGraph.Click
        If isDirtyCurrentPoints Then
            If MessageBox.Show($"Были изменения в таблице задания точек линии.{vbCrLf}Произвести сохранение изменений?",
                               "Смена линии", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                UpdateLimitationGraphs()
            Else
                isDirtyCurrentPoints = False
                indexSelectedLine = -1
            End If
        End If

        If TextBoxNameNewLimitationGraph.Text = "" Then
            Const text As String = "Необходимо ввести имя графика линии границ !"
            MessageBox.Show(text, "Ввод нового графика границ", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ShowMessageOnStatusPanel(text)
            Exit Sub
        End If

        ' проверка на имя
        Dim tempCurveGraph As CurveGraph = New CurveGraph(TextBoxNameNewLimitationGraph.Text)
        Dim isGraphContain As Boolean

        For I As Integer = 0 To ListBoxLimitationGraphs.Items.Count - 1
            If CType(ListBoxLimitationGraphs.Items(I), CurveGraph).NameCurveGraph = TextBoxNameNewLimitationGraph.Text Then
                isGraphContain = True
                Exit For
            End If
        Next

        'If ListBoxLimitationGraphs.Items.Contains(tempГрафикГраниц) Then
        If isGraphContain Then
            MessageBox.Show($"График линии границ с именем {TextBoxNameNewLimitationGraph.Text} уже существует!{vbCrLf}Введите другое имя в поле имя линии границ.",
                            "Ввод нового графика границ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ShowMessageOnStatusPanel("Повтор имени линии границ")
        Else
            'plot.LineColor = System.Drawing.Color.FromName(Color)
            'If PropertyEditorPlotStile.Source.Value.ToString = "Nothing" Then
            '    plot.LineStyle = PropertyEditorPlotStile.Source.Value
            'Else
            '    plot.LineStyle = NationalInstruments.UI.LineStyle.Dot
            'End If
            Dim plot As ScatterPlot = New ScatterPlot With {
                .LineColor = CType(PropertyEditorPlotColor.Source.Value, Drawing.Color), ' System.Drawing.Color.White
                .PointColor = Drawing.Color.Red,
                .PointStyle = PointStyle.EmptyDiamond,
                .LineStyle = CType(PropertyEditorPlotStile.Source.Value, LineStyle), 'NationalInstruments.UI.LineStyle.Dot
                .XAxis = Me.XAxis1,
                .YAxis = Me.YAxis1,
                .Tag = TextBoxNameNewLimitationGraph.Text
            }

            PropertyEditorPlotColor.Source = New PropertyEditorSource(plot, propertyLineColor)
            PropertyEditorPlotStile.Source = New PropertyEditorSource(plot, propertyLineStyle)

            mFormParrent.ScatterGraphParameter.Plots.AddRange(New ScatterPlot() {plot})
            AddXYPointAnnotation(plot, 0, 0)

            'было tempCurveGraph.Color = PropertyEditorPlotColor.Source.Value.Name
            tempCurveGraph.Color = PropertyEditorPlotColor.Source.Value.ToString
            tempCurveGraph.LineStyle = PropertyEditorPlotStile.Source.Value.ToString
            ListBoxLimitationGraphs.Items.Add(tempCurveGraph)
            ListBoxLimitationGraphs.SelectedIndex = ListBoxLimitationGraphs.Items.Count - 1
            ShowMessageOnStatusPanel($"График линии границ {TextBoxNameNewLimitationGraph.Text} добавлен успешно")
            ButtonClear.Enabled = True
        End If
    End Sub

    Private Sub CheckOnDirtyPoint()
        If indexSelectedLine = -1 Then Exit Sub

        Dim index As Integer
        Dim selectedCurveGraph As CurveGraph = CType(ListBoxLimitationGraphs.Items.Item(indexSelectedLine), CurveGraph) ' SelectedItem

        For Each row As DataGridViewRow In DataGridViewTablePoit.Rows
            If row.Index < DataGridViewTablePoit.Rows.Count - 1 Then
                index = CInt(row.Cells(NameColumnIndex).Value)

                If CDbl(row.Cells(NameColumnValueX).Value) <> selectedCurveGraph.GetPointsCurve(index).X OrElse CDbl(row.Cells(NameColumnValueY).Value) <> selectedCurveGraph.GetPointsCurve(index).Y Then
                    isDirtyCurrentPoints = True
                    isDirtyCurrentGraph = True
                    Exit For
                End If
            End If
        Next
    End Sub

    Private Sub ListBoxParametersWithPlot_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ListBoxParametersWithPlot.SelectedIndexChanged
        If ListBoxParametersWithPlot.SelectedIndex <> -1 Then
            TrySaveDBaseGraphs() ' здесь модальное окно и в нем blnБылиИзмененияВТекущемГрафике = False сделать нельзя
            isDirtyCurrentGraph = False
            TextBoxParameterWithPlot.Text = ListBoxParametersWithPlot.Text
            ButtonLoadGraph.Enabled = True
            ButtonSaveGraph.Enabled = True
            ButtonDeleteGraph.Enabled = True
            ShowMessageOnStatusPanel("Выбран набор границ для " & TextBoxParameterWithPlot.Text)
        Else
            TextBoxParameterWithPlot.Text = ""
            ButtonLoadGraph.Enabled = False
            ButtonSaveGraph.Enabled = False
            ButtonDeleteGraph.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Сохранить изменения
    ''' </summary>
    Private Sub TrySaveDBaseGraphs()
        If isDirtyCurrentGraph Then
            If MessageBox.Show($"Были изменения при редактировании текущего набора границ ограничений.{vbCrLf}Произвести сохранение изменений?",
                               "Сохранение изменений", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                UpdateLimitationGraphs()
                SaveDBaseGraphs()
            Else
                ShowMessageOnStatusPanel("Сохранение отменено")
                Exit Sub
            End If
        End If
    End Sub

    Private Sub TextBoxParameterWithPlot_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxParameterWithPlot.TextChanged
        ButtonSaveGraph.Enabled = True
    End Sub

    ''' <summary>
    ''' Восстановить Список Границ
    ''' </summary>
    Private Sub PopulateListBounds()
        Dim cn As New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.FormParrent.Manager.PathKT))
        Dim rdr As OleDbDataReader
        Dim cmd As OleDbCommand = cn.CreateCommand

        cmd.CommandType = CommandType.Text
        ListBoxParametersWithPlot.Items.Clear()
        cn.Open()
        cmd.CommandText = " Select ПараметрДляГраниц.ИмяПараметра FROM ПараметрДляГраниц;"
        rdr = cmd.ExecuteReader

        Do While rdr.Read
            ListBoxParametersWithPlot.Items.Add(rdr("ИмяПараметра"))
        Loop

        rdr.Close()
        cn.Close()

        If ListBoxParametersWithPlot.Items.Count > 0 Then ListBoxParametersWithPlot.SelectedIndex = 0
    End Sub

    Private Sub PropertyEditorPlotColor_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorPlotColor.TextChanged
        If ListBoxLimitationGraphs.SelectedIndex = -1 OrElse isFormClosing Then Exit Sub

        Dim selectedCurveGraph As CurveGraph = CType(ListBoxLimitationGraphs.SelectedItem, CurveGraph)

        'было selectedCurveGraph.Color = PropertyEditorPlotColor.Source.Value.Name
        selectedCurveGraph.Color = PropertyEditorPlotColor.Source.Value.ToString
        isDirtyCurrentGraph = True
    End Sub

    Private Sub PropertyEditorPlotStile_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorPlotStile.TextChanged
        If ListBoxLimitationGraphs.SelectedIndex = -1 OrElse isFormClosing Then Exit Sub

        Dim selectedCurveGraph As CurveGraph = CType(ListBoxLimitationGraphs.SelectedItem, CurveGraph)

        selectedCurveGraph.LineStyle = PropertyEditorPlotStile.Source.Value.ToString
        isDirtyCurrentGraph = True
    End Sub

#Region "ToolTip"
    Private Sub ShowMessageOnStatusPanel(ByVal message As String)
        TSStatusLabelMessage.Text = message
    End Sub

    Private Sub CleareStatusLabelMessage()
        TSStatusLabelMessage.Text = ""
    End Sub

    'ButtonLoadGraph
    Private Sub ButtonLoadGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonLoadGraph.MouseEnter
        ShowMessageOnStatusPanel("Считать выбранный набор границ ограничений для параметра")
    End Sub

    Private Sub ButtonLoadGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonLoadGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub

    'ButtonSaveGraph
    Private Sub ButtonSaveGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveGraph.MouseEnter
        ShowMessageOnStatusPanel("Записать редактируемый набор границ ограничений для параметра")
    End Sub

    Private Sub ButtonSaveGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub
    'ButtonAddGraph
    Private Sub ButtonAddGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddGraph.MouseEnter
        ShowMessageOnStatusPanel("Добавить новый редактируемый набор границ ограничений для параметра")
    End Sub

    Private Sub ButtonAddGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub
    'ButtonDeleteGraph
    Private Sub ButtonDeleteGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteGraph.MouseEnter
        ShowMessageOnStatusPanel("Удалить данный редактируемый набор границ ограничений для параметра")
    End Sub

    Private Sub ButtonDeleteGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub

    'ListBoxLimitationGraphs
    Private Sub ListBoxLimitationGraphs_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ListBoxLimitationGraphs.MouseEnter
        ShowMessageOnStatusPanel("Выбор из списка границы ограничений для параметра для редактирования")
    End Sub

    Private Sub ListBoxLimitationGraphs_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ListBoxLimitationGraphs.MouseLeave
        CleareStatusLabelMessage()
    End Sub

    'ButtonEditLimitationGraph
    Private Sub ButtonEditLimitationGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonEditLimitationGraph.MouseEnter
        ShowMessageOnStatusPanel("Сохранение произведенных изменений выбранной границы ограничений для параметра")
    End Sub

    Private Sub ButtonEditLimitationGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonEditLimitationGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub

    'ButtonDeleteSelectedLimitationGraph
    Private Sub ButtonDeleteSelectedLimitationGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteSelectedLimitationGraph.MouseEnter
        ShowMessageOnStatusPanel("Удаление из списка выбранной границы ограничений для параметра")
    End Sub

    Private Sub ButtonDeleteSelectedLimitationGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteSelectedLimitationGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub
    'ButtonAddNewLimitationGraph
    Private Sub ButtonAddNewLimitationGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddNewLimitationGraph.MouseEnter
        ShowMessageOnStatusPanel("Добавление новой границы ограничения в список")
    End Sub

    Private Sub ButtonAddNewLimitationGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddNewLimitationGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub
    'TextBoxNameNewLimitationGraph
    Private Sub TextBoxNameNewLimitationGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxNameNewLimitationGraph.MouseEnter
        ShowMessageOnStatusPanel("Выбранное имя границы ограничения или ввод нового имени при добавлении")
    End Sub

    Private Sub TextBoxNameNewLimitationGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxNameNewLimitationGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub
    'PropertyEditorPlotColor
    Private Sub PropertyEditorPlotColor_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorPlotColor.MouseEnter
        ShowMessageOnStatusPanel("Назначение цвета для границы ограничения")
    End Sub

    Private Sub PropertyEditorPlotColor_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorPlotColor.MouseLeave
        CleareStatusLabelMessage()
    End Sub
    'PropertyEditorPlotStile
    Private Sub PropertyEditorPlotStile_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorPlotStile.MouseEnter
        ShowMessageOnStatusPanel("Назначение стиля линии для границы ограничения")
    End Sub

    Private Sub PropertyEditorPlotStile_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorPlotStile.MouseLeave
        CleareStatusLabelMessage()
    End Sub
    'ListBoxParametersWithPlot
    Private Sub ListBoxParametersWithPlot_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ListBoxParametersWithPlot.MouseEnter
        ShowMessageOnStatusPanel("Выбор из списка набора границ ограничений для параметра")
    End Sub

    Private Sub ListBoxParametersWithPlot_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ListBoxParametersWithPlot.MouseLeave
        CleareStatusLabelMessage()
    End Sub
    'ButtonClear
    Private Sub ButtonClear_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonClear.MouseEnter
        ShowMessageOnStatusPanel("Удалить все границы ограничений для параметра")
    End Sub

    Private Sub ButtonClear_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonClear.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

End Class