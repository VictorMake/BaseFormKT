Imports System.Data.OleDb
Imports System.Windows.Forms

Public Class FormHierarchicalTable
#Region "Members"
    Public Property FormParrent() As frmBaseKT
    ''' <summary>
    ''' Строка параметра отчёта, содержащая суммарную строку фильтра построенного запроса.
    ''' </summary>
    ''' <returns></returns>
    Public Property ReportParameter As String = String.Empty
    Private DialogFormBuildQuery As FormBuildQuery

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

    Private ds As New DataSet
    'Private dsCopyTables As New DataSet
    Private dsKT As DataSet
    'Private drDataRow As DataRow
    'Private drDataRowParent As DataRow
    Private cn As OleDbConnection

    Private oda1TypeEngine As OleDbDataAdapter
    Private oda2NumberEngine As OleDbDataAdapter
    Private oda3NumberBuild As OleDbDataAdapter
    Private oda4NumberStage As OleDbDataAdapter
    Private oda5NumberStarting As OleDbDataAdapter
    Private oda6NumberKT As OleDbDataAdapter

    Private odaMeasurement As OleDbDataAdapter
    Private odaCast As OleDbDataAdapter
    Private odaConvert As OleDbDataAdapter

    Private dt1TypeEngine As DataTable
    Private dt2NumberEngine As DataTable
    Private dt3NumberBuild As DataTable
    Private dt4NumberStage As DataTable
    Private dt5NumberStarting As DataTable
    Private dt6NumberKT As DataTable

    Private dt1CopyTypeEngine As DataTable
    Private dt2CopyNumberEngine As DataTable
    Private dt3CopyNumberBuild As DataTable
    Private dt4CopyNumberStage As DataTable
    Private dt5CopyNumberStarting As DataTable
    Private dt6CopyNumberKT As DataTable

    Private dtMeasurement As DataTable
    Private dtCast As DataTable
    Private dtConverting As DataTable

    Private keyNumberStarting As Integer
    Private keyNumberKT As Integer

    Private ReadOnly ControlStageNamesConst As String() = {TypeEngine, NumberEngine, NumberBuild, NumberStage, NumberStarting, NumberKT, MeasurementParameters, CastParameters, ConvertingParameters}
    Private ReadOnly SelectStageNamesConst As String() = {cType_Engine, cNumber_Engine, cNumber_Build, cNumber_Stage, cNumber_Starting, cNumber_KT}

    Private IsAllowSelectionChanged As Boolean
    Private Const ConstKeyTypeEngine As String = "keyТипИзделия"
    Private Const ConstKeyNumberEngine As String = "keyНомерИзделия"
    Private Const ConstKeyNumberBuild As String = "keyНомерСборки"
    Private Const ConstKeyNumberStage As String = "keyНомерПостановки"
    Private Const ConstKeyNumberStarting As String = "keyНомерЗапуска"
    Private Const ConstKeyNumberKT As String = "keyНомерКТ"
    Private Const ConstNumberKT As String = "НомерКТ"
#End Region

    Public Sub New(ByVal FormParrent As frmBaseKT)
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Me.MdiParent = FormParrent
        Me.FormParrent = FormParrent
    End Sub

    Private Sub FormHierarchicalTable_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        ' System.Threading.Thread.CurrentThread.ApartmentState = System.Threading.ApartmentState.STA
        InitializeMembers()
        DataGridMeasurementParameters.Visible = False
        DataGridCastParameters.Visible = False
        DataGridConvertingParameters.Visible = False
    End Sub

    Private Sub FormHierarchicalTable_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        If Not FormParrent.IsWindowClosed Then e.Cancel = True
    End Sub

    Private Sub FormHierarchicalTable_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        If DialogFormBuildQuery IsNot Nothing Then DialogFormBuildQuery.Close()
        DialogFormBuildQuery = Nothing
        FormParrent = Nothing
    End Sub

    Private Sub ToolStripButtonSelect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolStripButtonSelect.Click
        If DialogFormBuildQuery Is Nothing Then DialogFormBuildQuery = New FormBuildQuery(Me) ' там инициализация и загрузка сеток данными InitializeGrids
        DialogFormBuildQuery.Show(Me) ' не работает (в псевдо модальном режиме глюка с таблицей)
        ToolStripButtonReport.Enabled = True
    End Sub

#Region "Property запросов и сортировок"
    Public Sub SetQueryTypeEngine(ByVal query As String)
        _WhereTypeEngine = query
    End Sub
    Public Sub SetQueryNumberEngine(ByVal query As String)
        _WhereNumberEngine = query
    End Sub
    Public Sub SetQueryNumberBuild(ByVal query As String)
        _WhereNumberBuild = query
    End Sub
    Public Sub SetQeryNumberStage(ByVal query As String)
        _WhereNumberStage = query
    End Sub
    Public Sub SetQeryNumberStarting(ByVal query As String)
        _WhereNumberStarting = query
    End Sub
    Public Sub SetQueryNumberKT(ByVal query As String)
        _WhereNumberKT = query
    End Sub
    Public Sub SetQueryMeasurement(ByVal Value As String)
        _WhereMeasurement = Value
    End Sub
    Public Sub SetQeryCast(ByVal query As String)
        _WhereCast = query
    End Sub
    Public Sub SetQueryConverting(ByVal query As String)
        _WhereConverting = query
    End Sub

    Public Sub SetSortTypeEngine(ByVal query As String)
        _SortTypeEngine = query
    End Sub
    Public Sub SetSortNumberEngine(ByVal query As String)
        _SortNumberEngine = query
    End Sub
    Public Sub SetSortNumberBuild(ByVal query As String)
        _SortNumberBuild = query
    End Sub
    Public Sub SetSortNumberStage(ByVal query As String)
        _SortNumberStage = query
    End Sub
    Public Sub SetSortNumberStarting(ByVal query As String)
        _SortNumberStarting = query
    End Sub
    Public Sub SetSortyNumberKT(ByVal query As String)
        _SortNumberKT = query
    End Sub
    Public Sub SetSortMeasurement(ByVal query As String)
        _SortMeasurement = query
    End Sub
    Public Sub SetSortCast(ByVal query As String)
        _SortCast = query
    End Sub
    Public Sub SetSortConverting(ByVal query As String)
        _SortConverting = query
    End Sub

    Public Sub InitializeMembers()
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
        ' при смене базы данных OpenToolStripMenuItem_Click вызывается также эта подпрограмма
        ' и если когда-то вызывался DialogПостроитьЗапроса то в нем заново обновить настройки сеток
        If DialogFormBuildQuery IsNot Nothing Then
            DialogFormBuildQuery.RefreshGrids()
        End If
    End Sub
#End Region

#Region "Handles SelectionChanged"
    Private Sub DataGrid1TypeEngine_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DataGrid1TypeEngine.SelectionChanged
        If IsAllowSelectionChanged Then DataGrid1TypeEngine_CurrentRowChanged()
    End Sub
    Private Sub DataGrid2NumberEngine_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DataGrid2NumberEngine.SelectionChanged
        If IsAllowSelectionChanged Then DataGrid2NumberEngine_CurrentRowChanged()
    End Sub
    Private Sub DataGrid3NumberBuild_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DataGrid3NumberBuild.SelectionChanged
        If IsAllowSelectionChanged Then DataGrid3NumberBuild_CurrentRowChanged()
    End Sub
    Private Sub DataGrid4NumberStage_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DataGrid4NumberStage.SelectionChanged
        If IsAllowSelectionChanged Then DataGrid4NumberStage_CurrentRowChanged()
    End Sub
    Private Sub DataGrid5NumberStarting_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DataGrid5NumberStarting.SelectionChanged
        If IsAllowSelectionChanged Then DataGrid5NumberStarting_CurrentRowChanged()
    End Sub
    Private Sub DataGrid6NumberKT_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DataGrid6NumberKT.SelectionChanged
        If IsAllowSelectionChanged Then If keyNumberStarting <> 0 Then DataGrid6NumberKT_CurrentRowChanged()
    End Sub

    ''' <summary>
    ''' При смене строки в родительской таблицы по ключу в этой строке
    ''' производится фильтрация дочерней таблицы и её DefaultView является источником копии таблицы этапа
    ''' грид дочерней таблицы связывается с копией
    ''' </summary>
    ''' <param name="numberPhase"></param>
    ''' <param name="currentBindingSource"></param>
    ''' <param name="nextBindingSource"></param>
    ''' <param name="dt"></param>
    ''' <param name="dtCopy"></param>
    ''' <param name="dgViewNext"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TableChangeChild(ByVal numberPhase As Integer,
                                      ByVal currentBindingSource As BindingSource,
                                      ByRef nextBindingSource As BindingSource,
                                      ByVal dt As DataTable,
                                      ByRef dtCopy As DataTable,
                                      ByRef dgViewNext As DataGridView) As Boolean
        Dim keyFK As Integer
        Dim row As DataRow = CType(currentBindingSource.Current, DataRowView).Row
        Dim keyColumn As String = Nothing
        Dim isChildContent As Boolean = False

        keyColumn = cKey & ControlStageNamesConst(numberPhase - 1)
        keyFK = CInt(row(keyColumn))

        If keyFK > 0 Then
            If dt.Rows.Count > 0 Then
                dt.DefaultView.RowFilter = $"{keyColumn} = {keyFK}"
                dtCopy = dt.DefaultView.ToTable()
            End If

            IsAllowSelectionChanged = False
            nextBindingSource.DataSource = dtCopy
            dgViewNext.DataSource = nextBindingSource
            dgViewNext.Refresh()
            IsAllowSelectionChanged = True

            If dtCopy.Rows.Count > 0 Then
                dgViewNext.Visible = True
                isChildContent = True
            End If
        End If

        Return isChildContent
    End Function

    Private Sub DataGrid1TypeEngine_CurrentRowChanged()
        'DataGrid2NumberEngine.Visible = DataGrid2NumberEngine.CurrentRowIndex = 0
        'If DataGrid2NumberEngine.SelectedRows.Count > 0 Then DataGrid2NumberEngine.Visible = DataGrid2NumberEngine.SelectedRows(0).Index = 0
        'DataGrid2NumberEngine.Visible = DataGrid2NumberEngine.SelectedRows.Count > 0

        DataGrid2NumberEngine.Visible = False
        DataGrid3NumberBuild.Visible = False
        DataGrid4NumberStage.Visible = False
        DataGrid5NumberStarting.Visible = False
        DataGrid6NumberKT.Visible = False
        DataGridMeasurementParameters.Visible = False
        DataGridCastParameters.Visible = False
        DataGridConvertingParameters.Visible = False

        If TableChangeChild(1, TypeEngine1BindingSource, NumberEngine2BindingSource, dt2NumberEngine, dt2CopyNumberEngine, DataGrid2NumberEngine) Then DataGrid2NumberEngine_CurrentRowChanged()
    End Sub

    Private Sub DataGrid2NumberEngine_CurrentRowChanged()
        DataGrid3NumberBuild.Visible = False
        DataGrid4NumberStage.Visible = False
        DataGrid5NumberStarting.Visible = False
        DataGrid6NumberKT.Visible = False
        DataGridMeasurementParameters.Visible = False
        DataGridCastParameters.Visible = False
        DataGridConvertingParameters.Visible = False

        If TableChangeChild(2, NumberEngine2BindingSource, NumberBuild3BindingSource, dt3NumberBuild, dt3CopyNumberBuild, DataGrid3NumberBuild) Then DataGrid3NumberBuild_CurrentRowChanged()
    End Sub

    Private Sub DataGrid3NumberBuild_CurrentRowChanged()
        DataGrid4NumberStage.Visible = False
        DataGrid5NumberStarting.Visible = False
        DataGrid6NumberKT.Visible = False
        DataGridMeasurementParameters.Visible = False
        DataGridCastParameters.Visible = False
        DataGridConvertingParameters.Visible = False

        If TableChangeChild(3, NumberBuild3BindingSource, NumberStage4BindingSource, dt4NumberStage, dt4CopyNumberStage, DataGrid4NumberStage) Then DataGrid4NumberStage_CurrentRowChanged()
    End Sub

    Private Sub DataGrid4NumberStage_CurrentRowChanged()
        DataGrid5NumberStarting.Visible = False
        DataGrid6NumberKT.Visible = False
        DataGridMeasurementParameters.Visible = False
        DataGridCastParameters.Visible = False
        DataGridConvertingParameters.Visible = False

        If TableChangeChild(4, NumberStage4BindingSource, NumberStarting5BindingSource, dt5NumberStarting, dt5CopyNumberStarting, DataGrid5NumberStarting) Then DataGrid5NumberStarting_CurrentRowChanged()
    End Sub

    Private Sub DataGrid5NumberStarting_CurrentRowChanged()
        Dim measurementSQL As String
        Dim castSQL As String
        Dim convertingSQL As String
        Dim isChildKT As Boolean ' Есть Дочерние КТ

        keyNumberStarting = 0
        keyNumberKT = 0

        If TableChangeChild(5, NumberStarting5BindingSource, NumberKT6BindingSource, dt6NumberKT, dt6CopyNumberKT, DataGrid6NumberKT) Then
            If DataGrid5NumberStarting.SelectedRows.Count = 0 Then Exit Sub
            keyNumberStarting = CInt(dt5CopyNumberStarting.Rows(NumberStarting5BindingSource.Position).Item(ConstKeyNumberStarting))
        End If

        isChildKT = dt6CopyNumberKT.Rows.Count > 0

        DataGrid6NumberKT.Visible = isChildKT
        DataGridMeasurementParameters.Visible = isChildKT
        DataGridCastParameters.Visible = isChildKT
        DataGridConvertingParameters.Visible = isChildKT

        IsAllowSelectionChanged = False
        DataGridMeasurementParameters.DataSource = Nothing
        DataGridCastParameters.DataSource = Nothing
        DataGridConvertingParameters.DataSource = Nothing

        dtMeasurement = New DataTable
        dtCast = New DataTable
        dtConverting = New DataTable

        If isChildKT AndAlso keyNumberStarting > 0 Then
            Try
                'measurementSQL = "TRANSFORM First([7ЗначенияПараметровКТ].Значение) AS [First-Значение] " & "SELECT [6НомерКТ].НомерКТ " & "FROM ИзмеренныеПараметры RIGHT JOIN (6НомерКТ RIGHT JOIN 7ЗначенияПараметровКТ ON [6НомерКТ].[keyНомерКТ] = [7ЗначенияПараметровКТ].[keyНомерКТ]) ON ИзмеренныеПараметры.ИмяПараметра = [7ЗначенияПараметровКТ].ИмяПараметра " & "WHERE ((([6НомерКТ].keyНомерЗапуска)= " & lngKeyНомерЗапуска & ")" & strПодзапросНомерКонтрТочки & ")" & "GROUP BY [6НомерКТ].НомерКТ " & "PIVOT ИзмеренныеПараметры.ИмяПараметра;"
                'castSQL = "TRANSFORM First([7ЗначенияПараметровКТ].[Значение]) AS [First-Значение] " & "SELECT [6НомерКТ].[НомерКТ] " & "FROM СвойстваПараметров RIGHT JOIN (6НомерКТ RIGHT JOIN 7ЗначенияПараметровКТ ON  [6НомерКТ].[keyНомерКТ]= [7ЗначенияПараметровКТ].[keyНомерКТ]) ON [СвойстваПараметров].[keyИмяПараметра]=[7ЗначенияПараметровКТ].[keyИмяПараметра] " & "Where ((([СвойстваПараметров].[ДляЧегоПараметр]) = 'Приведение') And (([6НомерКТ].[keyНомерЗапуска]) = " & lngKeyНомерЗапуска & ")" & strПодзапросНомерКонтрТочки & ")" & "GROUP BY [6НомерКТ].[НомерКТ] " & "PIVOT [СвойстваПараметров].[ИмяПараметра];"
                'convertingSQL = "TRANSFORM First([7ЗначенияПараметровКТ].[Значение]) AS [First-Значение] " & "SELECT [6НомерКТ].[НомерКТ] " & "FROM СвойстваПараметров RIGHT JOIN (6НомерКТ RIGHT JOIN 7ЗначенияПараметровКТ ON  [6НомерКТ].[keyНомерКТ]= [7ЗначенияПараметровКТ].[keyНомерКТ]) ON [СвойстваПараметров].[keyИмяПараметра]=[7ЗначенияПараметровКТ].[keyИмяПараметра] " & "Where ((([СвойстваПараметров].[ДляЧегоПараметр]) = 'Пересчет') And (([6НомерКТ].[keyНомерЗапуска]) = " & lngKeyНомерЗапуска & ")" & strПодзапросНомерКонтрТочки & ")" & "GROUP BY [6НомерКТ].[НомерКТ] " & "PIVOT [СвойстваПараметров].[ИмяПараметра];"
                'measurementSQL = "TRANSFORM First([7ЗначенияПараметровКТ].Значение) AS [First-Значение] " & "SELECT [6НомерКТ].НомерКТ " & "FROM ИзмеренныеПараметры INNER JOIN (6НомерКТ RIGHT JOIN 7ЗначенияПараметровКТ ON [6НомерКТ].[keyНомерКТ] = [7ЗначенияПараметровКТ].[keyНомерКТ]) ON ИзмеренныеПараметры.ИмяПараметра = [7ЗначенияПараметровКТ].ИмяПараметра " & "WHERE ((([6НомерКТ].keyНомерЗапуска)= " & lngKeyНомерЗапуска & ")" & strПодзапросНомерКонтрТочки & ")" & "GROUP BY [6НомерКТ].НомерКТ " & "PIVOT ИзмеренныеПараметры.ИмяПараметра;"
                'castSQL = "TRANSFORM First([7ЗначенияПараметровКТ].Значение) AS [First-Значение] " & "SELECT [6НомерКТ].НомерКТ " & "FROM РасчетныеПараметры INNER JOIN (6НомерКТ RIGHT JOIN 7ЗначенияПараметровКТ ON [6НомерКТ].[keyНомерКТ] = [7ЗначенияПараметровКТ].[keyНомерКТ]) ON РасчетныеПараметры.[ИмяПараметра] = [7ЗначенияПараметровКТ].[ИмяПараметра] " & "WHERE (((РасчетныеПараметры.ПриведеныйПараметр)=True) AND (([6НомерКТ].keyНомерЗапуска)= " & lngKeyНомерЗапуска & ")" & strПодзапросНомерКонтрТочки & ") " & "GROUP BY [6НомерКТ].НомерКТ " & "PIVOT РасчетныеПараметры.ИмяПараметра;"
                'convertingSQL = "TRANSFORM First([7ЗначенияПараметровКТ].Значение) AS [First-Значение] " & "SELECT [6НомерКТ].НомерКТ " & "FROM РасчетныеПараметры INNER JOIN (6НомерКТ RIGHT JOIN 7ЗначенияПараметровКТ ON [6НомерКТ].[keyНомерКТ] = [7ЗначенияПараметровКТ].[keyНомерКТ]) ON РасчетныеПараметры.[ИмяПараметра] = [7ЗначенияПараметровКТ].[ИмяПараметра] " & "WHERE (((РасчетныеПараметры.ПриведеныйПараметр)=False) AND (([6НомерКТ].keyНомерЗапуска) = " & lngKeyНомерЗапуска & ")" & strПодзапросНомерКонтрТочки & ") " & "GROUP BY [6НомерКТ].НомерКТ " & "PIVOT РасчетныеПараметры.ИмяПараметра;"

                measurementSQL = "TRANSFORM First([7ЗначенияПараметровКТ].Значение) AS [First-Значение] " & "SELECT [6НомерКТ].keyНомерКТ, [6НомерКТ].НомерКТ " & "FROM ИзмеренныеПараметры INNER JOIN (6НомерКТ RIGHT JOIN 7ЗначенияПараметровКТ ON [6НомерКТ].[keyНомерКТ] = [7ЗначенияПараметровКТ].[keyНомерКТ]) ON ИзмеренныеПараметры.ИмяПараметра = [7ЗначенияПараметровКТ].ИмяПараметра " & "WHERE ((([6НомерКТ].keyНомерЗапуска)= " & keyNumberStarting & "))" & "GROUP BY [6НомерКТ].keyНомерКТ, [6НомерКТ].НомерКТ " & "PIVOT ИзмеренныеПараметры.ИмяПараметра;"
                castSQL = "TRANSFORM First([7ЗначенияПараметровКТ].Значение) AS [First-Значение] " & "SELECT [6НомерКТ].keyНомерКТ, [6НомерКТ].НомерКТ " & "FROM РасчетныеПараметры INNER JOIN (6НомерКТ RIGHT JOIN 7ЗначенияПараметровКТ ON [6НомерКТ].[keyНомерКТ] = [7ЗначенияПараметровКТ].[keyНомерКТ]) ON РасчетныеПараметры.[ИмяПараметра] = [7ЗначенияПараметровКТ].[ИмяПараметра] " & "WHERE (((РасчетныеПараметры.ПриведеныйПараметр)=True) AND (([6НомерКТ].keyНомерЗапуска)= " & keyNumberStarting & ")) " & "GROUP BY [6НомерКТ].keyНомерКТ, [6НомерКТ].НомерКТ " & "PIVOT РасчетныеПараметры.ИмяПараметра;"
                convertingSQL = "TRANSFORM First([7ЗначенияПараметровКТ].Значение) AS [First-Значение] " & "SELECT [6НомерКТ].keyНомерКТ, [6НомерКТ].НомерКТ " & "FROM РасчетныеПараметры INNER JOIN (6НомерКТ RIGHT JOIN 7ЗначенияПараметровКТ ON [6НомерКТ].[keyНомерКТ] = [7ЗначенияПараметровКТ].[keyНомерКТ]) ON РасчетныеПараметры.[ИмяПараметра] = [7ЗначенияПараметровКТ].[ИмяПараметра] " & "WHERE (((РасчетныеПараметры.ПриведеныйПараметр)=False) AND (([6НомерКТ].keyНомерЗапуска) = " & keyNumberStarting & ")) " & "GROUP BY [6НомерКТ].keyНомерКТ, [6НомерКТ].НомерКТ " & "PIVOT РасчетныеПараметры.ИмяПараметра;"

                cn = New OleDbConnection(BuildCnnStr(PROVIDER_JET, FormParrent.Manager.PathKT))
                cn.Open()
                dsKT = New DataSet

                odaMeasurement = New OleDbDataAdapter(measurementSQL, cn)
                odaMeasurement.Fill(dsKT, MeasurementParameters)
                dtMeasurement = dsKT.Tables(MeasurementParameters)

                odaCast = New OleDbDataAdapter(castSQL, cn)
                odaCast.Fill(dsKT, CastParameters)
                dtCast = dsKT.Tables(CastParameters)

                odaConvert = New OleDbDataAdapter(convertingSQL, cn)
                odaConvert.Fill(dsKT, ConvertingParameters)
                dtConverting = dsKT.Tables(ConvertingParameters)

                If dtMeasurement.Rows.Count > 0 AndAlso dtCast.Rows.Count > 0 AndAlso dtConverting.Rows.Count > 0 Then
                    ClearMeasurementCastConvertingTable()
                End If

                ' связывание
                MeasurementBindingSource.DataSource = dtMeasurement
                CastBindingSource.DataSource = dtCast
                ConvertingBindingSource.DataSource = dtConverting

                DataGridMeasurementParameters.DataSource = MeasurementBindingSource
                DataGridMeasurementParameters.Refresh()
                DataGridCastParameters.DataSource = CastBindingSource
                DataGridCastParameters.Refresh()
                DataGridConvertingParameters.DataSource = ConvertingBindingSource
                DataGridConvertingParameters.Refresh()
                IsAllowSelectionChanged = True

                If dtMeasurement.Rows.Count > 0 Then SetColumStyleByType(cn, DataGridMeasurementParameters, measurementSQL)
                If dtCast.Rows.Count > 0 Then SetColumStyleByType(cn, DataGridCastParameters, castSQL)
                If dtConverting.Rows.Count > 0 Then SetColumStyleByType(cn, DataGridConvertingParameters, convertingSQL)

                cn.Close()
            Catch ex As Exception
                MessageBox.Show(ex.ToString, $"Процедура {NameOf(DataGrid5NumberStarting_CurrentRowChanged)}", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Finally
                If cn.State = ConnectionState.Open Then
                    cn.Close()
                End If
                IsAllowSelectionChanged = True
            End Try
        End If

        DataGrid6NumberKT_CurrentRowChanged()
    End Sub

    ''' <summary>
    ''' подсветить строку при смене позициии в сетке DataGrid5НомерЗапуска
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DataGrid6NumberKT_CurrentRowChanged()
        Dim numberKT As Integer
        Try
            TSButtonShowFindChannels.Enabled = dtMeasurement.Rows.Count > 0
            If dtMeasurement.Rows.Count > 0 Then
                numberKT = CInt(DataGrid6NumberKT.SelectedCells(2).Value) 'Item(ConstNumberKT)
                keyNumberKT = CInt(DataGrid6NumberKT.SelectedCells(1).Value) 'Item(ConstKeyNumberKT))
                SetPositionDataGrid(dtMeasurement, MeasurementBindingSource, DataGridMeasurementParameters, numberKT)
                SetPositionDataGrid(dtCast, CastBindingSource, DataGridCastParameters, numberKT)
                SetPositionDataGrid(dtConverting, ConvertingBindingSource, DataGridConvertingParameters, numberKT)
                ShowCurrentRecordNumber()
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region "Очистка таблиц"
    ''' <summary>
    ''' Очистить Родительскую Таблицу
    ''' к дочерней таблице применяется фильтр и записи в родительской таблице не вречающиеся в представлении дочерней удаляются
    ''' делать не взирая на наличие фильтра
    ''' </summary>
    ''' <param name="numberPhase"></param>
    ''' <param name="DataTableParent"></param>
    ''' <param name="DataTableChild"></param>
    ''' <param name="dataTableChildRowFilter"></param>
    ''' <param name="rowSort"></param>
    ''' <remarks></remarks>
    Private Sub ClearParenTable(ByVal numberPhase As Integer,
                                ByRef DataTableParent As DataTable,
                                ByRef DataTableChild As DataTable,
                                ByVal dataTableChildRowFilter As String,
                                ByVal rowSort As String)
        If dataTableChildRowFilter <> "" Then
            DataTableChild.DefaultView.RowFilter = dataTableChildRowFilter
            If rowSort <> "" Then DataTableChild.DefaultView.Sort = rowSort
        End If

        Dim dataViewTableChild As DataView = DataTableChild.DefaultView

        For Each itemParenDataRow As DataRow In DataTableParent.Rows
            Dim isDelete As Boolean = True

            For indexRow As Integer = 0 To dataViewTableChild.Count - 1
                ' сравниваем поля ключей записи (они во всех запросах на разных позициях)
                If numberPhase = StageGridType.ТипыИзделия1 Then
                    If CInt(itemParenDataRow.Item(0)) = CInt(dataViewTableChild(indexRow).Row.ItemArray(0)) Then
                        isDelete = False
                        Exit For
                    End If
                Else
                    If CInt(itemParenDataRow.Item(1)) = CInt(dataViewTableChild(indexRow).Row.ItemArray(0)) Then
                        isDelete = False
                        Exit For
                    End If
                End If
            Next indexRow

            If isDelete Then itemParenDataRow.Delete()
        Next

        DataTableParent.AcceptChanges()
    End Sub

    ''' <summary>
    ''' Очистить Таблицу По Общей
    ''' очистка от грязных записей (без КТ) в таблицах не содержащихся в сборной dtCommon
    ''' </summary>
    ''' <param name="numberPhase"></param>
    ''' <param name="refStageDataTable"></param>
    ''' <param name="dtCommon"></param>
    ''' <remarks></remarks>
    Private Sub ClearTableByCommonTable(ByVal numberPhase As Integer,
                                        ByRef refStageDataTable As DataTable,
                                        ByVal dtCommon As DataTable)
        Dim keyColumn As String

        If numberPhase = StageGridType.НомерКТ6 Then
            keyColumn = cKey & ControlStageNamesConst(numberPhase - 2)
        Else
            keyColumn = cKey & ControlStageNamesConst(numberPhase - 1)
        End If

        For Each itemRow As DataRow In refStageDataTable.Rows
            Dim isDelete As Boolean = True

            For Each itemRowCommon As DataRow In dtCommon.Rows
                If CInt(itemRowCommon(keyColumn)) = CInt(itemRow(keyColumn)) Then
                    isDelete = False
                    Exit For
                End If
            Next

            If isDelete Then itemRow.Delete()
        Next

        refStageDataTable.AcceptChanges()
    End Sub

    ''' <summary>
    ''' Очистить Таблицу
    ''' если к таблице применен фильтр, то записи вне фильтра удалить
    ''' </summary>
    ''' <param name="numberPhase"></param>
    ''' <param name="refStageDataTable"></param>
    ''' <param name="rowFilter"></param>
    ''' <remarks></remarks>
    Private Sub ClearTable(ByVal numberPhase As Integer,
                           ByRef refStageDataTable As DataTable,
                           ByVal rowFilter As String)
        Dim DataViewStage As DataView = Nothing

        ' пример
        'Dim view As DataView = New DataView
        'With view
        '    .Table = DataSet1.Tables("Suppliers")
        '    .AllowDelete = True
        '    .AllowEdit = True
        '    .AllowNew = True
        '    .RowFilter = "City = 'Berlin'"
        '    .RowStateFilter = DataViewRowState.ModifiedCurrent
        '    .Sort = "CompanyName DESC"
        'End With

        If rowFilter <> "" Then
            DataViewStage = refStageDataTable.DefaultView
        End If

        If rowFilter <> "" Then
            For Each itemRow As DataRow In refStageDataTable.Rows
                Dim isDelete As Boolean = True

                For indexRow As Integer = 0 To DataViewStage.Count - 1
                    If numberPhase = StageGridType.ТипыИзделия1 Then
                        If CInt(itemRow.ItemArray(0)) = CInt(DataViewStage(indexRow).Row.ItemArray(0)) Then
                            isDelete = False
                            Exit For
                        End If
                    Else
                        ' сравниваем поля ключей записи (они во всех запросах на 2 позиции)
                        If CInt(itemRow.ItemArray(1)) = CInt(DataViewStage(indexRow).Row.ItemArray(1)) Then
                            isDelete = False
                            Exit For
                        End If
                    End If
                Next

                If isDelete Then itemRow.Delete()
            Next

            refStageDataTable.AcceptChanges()
        End If
    End Sub

    ''' <summary>
    ''' Очистить Дочернюю Таблицу
    '''  если к родительской таблице применен фильтр значить очистить записи в дочерней таблице не попавшие в фильтр
    ''' применить вне зависимости от наличия фильтра - каскадно
    ''' </summary>
    ''' <param name="numberPhase"></param>
    ''' <param name="DataTableParent"></param>
    ''' <param name="DataTableChild"></param>
    ''' <remarks></remarks>
    Private Sub ClearChildTable(ByVal numberPhase As Integer,
                                ByRef DataTableParent As DataTable,
                                ByRef DataTableChild As DataTable)
        Dim dataViewTableParent As DataView = DataTableParent.DefaultView

        For Each itemChildDataRow As DataRow In DataTableChild.Rows
            Dim isDelete As Boolean = True

            For indexRow As Integer = 0 To dataViewTableParent.Count - 1
                If numberPhase = StageGridType.ТипыИзделия1 Then
                    If CInt(itemChildDataRow.Item(0)) = CInt(dataViewTableParent(indexRow).Row.ItemArray(0)) Then
                        isDelete = False
                        Exit For
                    End If
                Else
                    If CInt(itemChildDataRow.Item(0)) = CInt(dataViewTableParent(indexRow).Row.ItemArray(1)) Then
                        isDelete = False
                        Exit For
                    End If
                End If
            Next

            If isDelete Then itemChildDataRow.Delete()
        Next

        DataTableChild.AcceptChanges()
    End Sub

    ''' <summary>
    ''' Очистить Расчетные Таблицы
    ''' очистка расчётных таблиц между собой при наличии у любой из них фильтра
    ''' и последующая очистка их с dt6CopyНомерКонтрольнойТочки при наличии _ЗапросНомерКонтрТочки
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearMeasurementCastConvertingTable()
        Dim MeasurementSortListKey As New SortedList(Of Integer, Integer)
        Dim CastSortListKey As New SortedList(Of Integer, Integer)
        Dim ConvertingSortListKey As New SortedList(Of Integer, Integer)
        Dim listKeys As New List(Of Integer)

        If _WhereMeasurement <> "" OrElse _WhereCast <> "" OrElse _WhereConverting <> "" Then
            If _WhereMeasurement <> "" Then
                dtMeasurement.DefaultView.RowFilter = _WhereMeasurement
                If _SortMeasurement <> "" Then
                    dtMeasurement.DefaultView.Sort = _SortMeasurement
                End If
            End If

            If _WhereCast <> "" Then
                dtCast.DefaultView.RowFilter = _WhereCast
                If _SortCast <> "" Then
                    dtCast.DefaultView.Sort = _SortCast
                End If
            End If

            If _WhereConverting <> "" Then
                dtConverting.DefaultView.RowFilter = _WhereConverting
                If _SortConverting <> "" Then
                    dtConverting.DefaultView.Sort = _SortConverting
                End If
            End If

            ' применить фильтры
            ClearTable(1, dtMeasurement, _WhereMeasurement)
            ClearTable(1, dtCast, _WhereCast)
            ClearTable(1, dtConverting, _WhereConverting)

            ' теперь каждая таблица очищена, но надо их синхронизировать между собой
            ' заполнить с первой таблицы по умолчанию
            For Each itemRow As DataRow In dtMeasurement.Rows
                MeasurementSortListKey.Add(CInt(itemRow(0)), CInt(itemRow(0)))
            Next
            ' остальные при наличии фильтра
            If _WhereCast <> "" Then
                For Each itemRow As DataRow In dtCast.Rows
                    CastSortListKey.Add(CInt(itemRow(0)), CInt(itemRow(0)))
                Next
            End If

            If _WhereConverting <> "" Then
                For Each itemRow As DataRow In dtConverting.Rows
                    ConvertingSortListKey.Add(CInt(itemRow(0)), CInt(itemRow(0)))
                Next
            End If

            For Each kvp As KeyValuePair(Of Integer, Integer) In MeasurementSortListKey
                'kvp.Key, kvp.Value
                If _WhereCast = "" Then
                    If IsContainsConvertingKeyInSortListKey(ConvertingSortListKey, kvp.Key, _WhereConverting) Then
                        ' содержатся в 3 таблицах значить добавить
                        listKeys.Add(kvp.Key)
                    End If
                Else
                    If CastSortListKey.ContainsKey(kvp.Key) Then
                        If IsContainsConvertingKeyInSortListKey(ConvertingSortListKey, kvp.Key, _WhereConverting) Then
                            ' содержатся в 3 таблицах значить добавить
                            listKeys.Add(kvp.Key)
                        End If
                    End If
                End If
            Next kvp

            ' получили ilistKeys, теперь по каждой таблице очистить
            ClearTableAtListKeys(dtMeasurement, listKeys)
            ClearTableAtListKeys(dtCast, listKeys)
            ClearTableAtListKeys(dtConverting, listKeys)
        Else
            ' индивидуальных фильтров 3 расчетных таблиц нет
            ' заполнить с первой таблицы по умолчанию
            For Each drRow As DataRow In dtMeasurement.Rows
                listKeys.Add(CInt(drRow(0)))
            Next
        End If

        ' в итоге ilistKeys содержит ключи или после применения фильтров или сразу все для данного выбранного номера запуска
        ' теперь если есть фильтр _ЗапросНомерКонтрТочки опять все 3 таблицы надо очистить
        If _WhereNumberKT <> "" Then
            Dim listKeysForDeleted As New List(Of Integer)

            For Each key As Integer In listKeys
                Dim isDelete As Boolean = True

                For Each itemRow As DataRow In dt6CopyNumberKT.Rows
                    If CInt(itemRow(ConstKeyNumberKT)) = key Then
                        isDelete = False
                        Exit For
                    End If
                Next

                If isDelete Then listKeysForDeleted.Add(key)
            Next

            For Each keyDeleted As Integer In listKeysForDeleted
                ' здесь делается копия переменнной, в противном случае из обобщенной коллекции не удалить
                'ilistKeys.RemoveAt(ilistKeys.IndexOf(KeyDeleted))
                listKeys.Remove(keyDeleted)
            Next

            ClearTableAtListKeys(dtMeasurement, listKeys)
            ClearTableAtListKeys(dtCast, listKeys)
            ClearTableAtListKeys(dtConverting, listKeys)
        End If

        dsKT.AcceptChanges()
    End Sub

    ''' <summary>
    ''' Проверка наличия ключа для записи таблицы Пересчитанные в SortListKey
    ''' </summary>
    ''' <param name="SortListKey"></param>
    ''' <param name="key"></param>
    ''' <param name="rowFilter"></param>
    ''' <returns></returns>
    Private Function IsContainsConvertingKeyInSortListKey(ByVal SortListKey As SortedList(Of Integer, Integer),
                                                      ByVal key As Integer,
                                                      ByVal rowFilter As String) As Boolean
        Dim ContainsKey As Boolean = False

        If rowFilter = "" Then
            ContainsKey = True
        Else
            If SortListKey.ContainsKey(key) Then
                ContainsKey = True
            End If
        End If

        Return ContainsKey
    End Function

    ''' <summary>
    ''' Очистить Каждую Расчетную Таблицу
    ''' </summary>
    ''' <param name="TempDataTable"></param>
    ''' <param name="listKeys"></param>
    Private Sub ClearTableAtListKeys(ByRef TempDataTable As DataTable, ByVal listKeys As List(Of Integer))
        For Each drRow As DataRow In TempDataTable.Rows
            If Not listKeys.Contains(CInt((drRow(0)))) Then
                drRow.Delete()
            End If
        Next

        TempDataTable.AcceptChanges()
    End Sub
#End Region

    ''' <summary>
    ''' Выдать Строку Запроса Для Таблицы Этапа
    ''' </summary>
    ''' <param name="numberPhase"></param>
    ''' <returns></returns>
    Private Function GetSqlSelectForTablePhase(ByVal numberPhase As Integer) As String
        Dim strSQL As String = String.Empty

        If numberPhase = StageGridType.ТипыИзделия1 Then
            strSQL = "TRANSFORM First(ЗначенияКонтроловДляТипИзделия.ЗначениеПользователя) AS [First-ЗначениеПользователя] " &
            "SELECT [1ТипИзделия].keyТипИзделия, [1ТипИзделия].ТипИзделия AS Тип_Изделия " &
            "FROM КонтролыДляТипИзделия RIGHT JOIN (1ТипИзделия RIGHT JOIN ЗначенияКонтроловДляТипИзделия ON [1ТипИзделия].keyТипИзделия = ЗначенияКонтроловДляТипИзделия.keyУровень) ON КонтролыДляТипИзделия.keyКонтролДляУровня = ЗначенияКонтроловДляТипИзделия.keyКонтролДляУровня " &
            "GROUP BY [1ТипИзделия].keyТипИзделия, [1ТипИзделия].ТипИзделия " &
            "PIVOT КонтролыДляТипИзделия.Name;"
        Else
            'TRANSFORM First(ЗначенияКонтроловДляНомерИзделия.ЗначениеПользователя) AS [First-ЗначениеПользователя]
            'SELECT [1ТипИзделия].keyТипИзделия, [2НомерИзделия].keyНомерИзделия, [2НомерИзделия].НомерИзделия AS Номер_Изделия
            'FROM КонтролыДляНомерИзделия RIGHT JOIN ((1ТипИзделия RIGHT JOIN 2НомерИзделия ON [1ТипИзделия].keyТипИзделия = [2НомерИзделия].keyТипИзделия) RIGHT JOIN ЗначенияКонтроловДляНомерИзделия ON [2НомерИзделия].keyНомерИзделия = ЗначенияКонтроловДляНомерИзделия.keyУровень) ON КонтролыДляНомерИзделия.keyКонтролДляУровня = ЗначенияКонтроловДляНомерИзделия.keyКонтролДляУровня
            'GROUP BY [1ТипИзделия].keyТипИзделия, [2НомерИзделия].keyНомерИзделия, [2НомерИзделия].НомерИзделия
            'PIVOT КонтролыДляНомерИзделия.Name;

            strSQL = "TRANSFORM First(ЗначенияКонтроловДля" & ControlStageNamesConst(numberPhase - 1) & ".ЗначениеПользователя) AS [First-ЗначениеПользователя] " &
            "SELECT [" & (numberPhase - 1).ToString & ControlStageNamesConst(numberPhase - 2) & "].key" & ControlStageNamesConst(numberPhase - 2) & ", [" & (numberPhase).ToString & ControlStageNamesConst(numberPhase - 1) & "].key" & ControlStageNamesConst(numberPhase - 1) & ", [" & (numberPhase).ToString & ControlStageNamesConst(numberPhase - 1) & "]." & ControlStageNamesConst(numberPhase - 1) & " AS " & SelectStageNamesConst(numberPhase - 1) & " " &
            "FROM КонтролыДля" & ControlStageNamesConst(numberPhase - 1) & " RIGHT JOIN ((" & (numberPhase - 1).ToString & ControlStageNamesConst(numberPhase - 2) & " RIGHT JOIN " & (numberPhase).ToString & ControlStageNamesConst(numberPhase - 1) & " ON [" & (numberPhase - 1).ToString & ControlStageNamesConst(numberPhase - 2) & "].key" & ControlStageNamesConst(numberPhase - 2) & " = [" & (numberPhase).ToString & ControlStageNamesConst(numberPhase - 1) & "].key" & ControlStageNamesConst(numberPhase - 2) & ") RIGHT JOIN ЗначенияКонтроловДля" & ControlStageNamesConst(numberPhase - 1) & " ON [" & (numberPhase).ToString & ControlStageNamesConst(numberPhase - 1) & "].key" & ControlStageNamesConst(numberPhase - 1) & " = ЗначенияКонтроловДля" & ControlStageNamesConst(numberPhase - 1) & ".keyУровень) ON КонтролыДля" & ControlStageNamesConst(numberPhase - 1) & ".keyКонтролДляУровня = ЗначенияКонтроловДля" & ControlStageNamesConst(numberPhase - 1) & ".keyКонтролДляУровня " &
            "GROUP BY [" & (numberPhase - 1).ToString & ControlStageNamesConst(numberPhase - 2) & "].key" & ControlStageNamesConst(numberPhase - 2) & ", [" & (numberPhase).ToString & ControlStageNamesConst(numberPhase - 1) & "].key" & ControlStageNamesConst(numberPhase - 1) & ", [" & (numberPhase).ToString & ControlStageNamesConst(numberPhase - 1) & "]." & ControlStageNamesConst(numberPhase - 1) & " " &
            "PIVOT КонтролыДля" & ControlStageNamesConst(numberPhase - 1) & ".Name;"
        End If

        Return strSQL
    End Function

    ''' <summary>
    ''' Обновить таблицы после составления запроса
    ''' </summary>
    Public Sub ShowQuery()
        Dim currentCursor As Cursor = Cursor.Current
        Dim dtCommon As DataTable
        Dim odaCommon As OleDbDataAdapter
        Dim dsCommon As New DataSet

        Cursor.Current = Cursors.WaitCursor
        BindingNavigatorPositionItem.Text = ""

        ' 1 самый последний здесь нет TRANSFORM потому что нет контролов?
        ' число записей соответствует числу записей КТ
        Dim sqlCommon As String = "SELECT [1ТипИзделия].keyТипИзделия, [2НомерИзделия].keyНомерИзделия, [3НомерСборки].keyНомерСборки, [4НомерПостановки].keyНомерПостановки, [5НомерЗапуска].keyНомерЗапуска " &
                                    "FROM ((((1ТипИзделия RIGHT JOIN 2НомерИзделия ON [1ТипИзделия].keyТипИзделия=[2НомерИзделия].keyТипИзделия) RIGHT JOIN 3НомерСборки ON [2НомерИзделия].keyНомерИзделия=[3НомерСборки].keyНомерИзделия) RIGHT JOIN 4НомерПостановки ON [3НомерСборки].keyНомерСборки=[4НомерПостановки].keyНомерСборки) RIGHT JOIN 5НомерЗапуска ON [4НомерПостановки].keyНомерПостановки=[5НомерЗапуска].keyНомерПостановки) RIGHT JOIN 6НомерКТ ON [5НомерЗапуска].keyНомерЗапуска=[6НомерКТ].keyНомерЗапуска "
        '& _
        '       strWHEREЗапросНомерКонтрТочки

        IsAllowSelectionChanged = False
        TypeEngine1BindingSource.DataSource = Nothing
        NumberEngine2BindingSource.DataSource = Nothing
        NumberBuild3BindingSource.DataSource = Nothing
        NumberStage4BindingSource.DataSource = Nothing
        NumberStarting5BindingSource.DataSource = Nothing
        NumberKT6BindingSource.DataSource = Nothing

        Try
            ds = New DataSet

            dt1TypeEngine = New DataTable
            dt2NumberEngine = New DataTable
            dt3NumberBuild = New DataTable
            dt4NumberStage = New DataTable
            dt5NumberStarting = New DataTable
            dt6NumberKT = New DataTable

            cn = New OleDbConnection(BuildCnnStr(PROVIDER_JET, FormParrent.Manager.PathKT))
            cn.Open()

            oda1TypeEngine = New OleDbDataAdapter(GetSqlSelectForTablePhase(StageGridType.ТипыИзделия1), cn)
            oda2NumberEngine = New OleDbDataAdapter(GetSqlSelectForTablePhase(StageGridType.НомерИзделия2), cn)
            oda3NumberBuild = New OleDbDataAdapter(GetSqlSelectForTablePhase(StageGridType.НомерСборки3), cn)
            oda4NumberStage = New OleDbDataAdapter(GetSqlSelectForTablePhase(StageGridType.НомерПостановки4), cn)
            oda5NumberStarting = New OleDbDataAdapter(GetSqlSelectForTablePhase(StageGridType.НомерЗапуска5), cn)
            oda6NumberKT = New OleDbDataAdapter(GetSqlSelectForTablePhase(StageGridType.НомерКТ6), cn)

            oda1TypeEngine.Fill(ds, TypeEngine)
            oda2NumberEngine.Fill(ds, NumberEngine)
            oda3NumberBuild.Fill(ds, NumberBuild)
            oda4NumberStage.Fill(ds, NumberStage)
            oda5NumberStarting.Fill(ds, NumberStarting)
            oda6NumberKT.Fill(ds, NumberKT)

            dt1TypeEngine = ds.Tables(TypeEngine)
            dt2NumberEngine = ds.Tables(NumberEngine)
            dt3NumberBuild = ds.Tables(NumberBuild)
            dt4NumberStage = ds.Tables(NumberStage)
            dt5NumberStarting = ds.Tables(NumberStarting)
            dt6NumberKT = ds.Tables(NumberKT)

            odaCommon = New OleDbDataAdapter(sqlCommon, cn)
            odaCommon.Fill(dsCommon, "Общая")
            dtCommon = dsCommon.Tables("Общая")
            cn.Close()

            ' удалить все строки из таблиц, если не встречаются ни одной записи КТ
            ClearTableByCommonTable(StageGridType.ТипыИзделия1, dt1TypeEngine, dtCommon)
            ClearTableByCommonTable(StageGridType.НомерИзделия2, dt2NumberEngine, dtCommon)
            ClearTableByCommonTable(StageGridType.НомерСборки3, dt3NumberBuild, dtCommon)
            ClearTableByCommonTable(StageGridType.НомерПостановки4, dt4NumberStage, dtCommon)
            ClearTableByCommonTable(StageGridType.НомерЗапуска5, dt5NumberStarting, dtCommon)
            ClearTableByCommonTable(StageGridType.НомерКТ6, dt6NumberKT, dtCommon)
            ds.AcceptChanges()

            ' так как ни где не присваивается для этой таблицы
            Dim dataViewStage As DataView = Nothing

            If _WhereTypeEngine <> "" Then
                dt1TypeEngine.DefaultView.RowFilter = _WhereTypeEngine
                dataViewStage = dt1TypeEngine.DefaultView

                If _SortTypeEngine <> "" Then
                    dataViewStage.Sort = _SortTypeEngine
                End If
            End If

            ClearParenTable(StageGridType.НомерЗапуска5, dt5NumberStarting, dt6NumberKT, _WhereNumberKT, _SortNumberKT)
            ClearParenTable(StageGridType.НомерПостановки4, dt4NumberStage, dt5NumberStarting, _WhereNumberStarting, _SortNumberStarting)
            ClearParenTable(StageGridType.НомерСборки3, dt3NumberBuild, dt4NumberStage, _WhereNumberStage, _SortNumberStage)
            ClearParenTable(StageGridType.НомерИзделия2, dt2NumberEngine, dt3NumberBuild, _WhereNumberBuild, _SortNumberBuild)
            ClearParenTable(StageGridType.ТипыИзделия1, dt1TypeEngine, dt2NumberEngine, _WhereNumberEngine, _SortNumberEngine)
            ds.AcceptChanges()

            ClearTable(StageGridType.НомерКТ6, dt6NumberKT, _WhereNumberKT)
            ClearTable(StageGridType.НомерЗапуска5, dt5NumberStarting, _WhereNumberStarting)
            ClearTable(StageGridType.НомерПостановки4, dt4NumberStage, _WhereNumberStage)
            ClearTable(StageGridType.НомерСборки3, dt3NumberBuild, _WhereNumberBuild)
            ClearTable(StageGridType.НомерИзделия2, dt2NumberEngine, _WhereNumberEngine)
            ClearTable(StageGridType.ТипыИзделия1, dt1TypeEngine, _WhereTypeEngine)
            ds.AcceptChanges()

            ClearChildTable(StageGridType.ТипыИзделия1, dt1TypeEngine, dt2NumberEngine)
            ClearChildTable(StageGridType.НомерИзделия2, dt2NumberEngine, dt3NumberBuild)
            ClearChildTable(StageGridType.НомерСборки3, dt3NumberBuild, dt4NumberStage)
            ClearChildTable(StageGridType.НомерПостановки4, dt4NumberStage, dt5NumberStarting)
            ClearChildTable(StageGridType.НомерЗапуска5, dt5NumberStarting, dt6NumberKT)
            ds.AcceptChanges()

            ' создаем отношения между таблицами
            'Dim parentColumn As DataColumn = dt1TypeEngine.Columns(ConstKeyTypeEngine)
            'Dim childColumn As DataColumn = dt2NumberEngine.Columns(ConstKeyTypeEngine)
            'parentColumn.ReadOnly = True
            'parentColumn.AutoIncrement = True
            'parentColumn.Unique = True
            'Dim PrimaryKeyColumns(0) As DataColumn
            'PrimaryKeyColumns(0) = dt1ТипИзделия.Columns(ConstKeyTypeEngine)
            'dt1ТипИзделия.PrimaryKey = PrimaryKeyColumns
            'ds.Relations.Add("CustomerOrder", New DataColumn() {customerTable.Columns(0)}, New DataColumn() {orderTable.Columns(1)}, True)
            'ds.Tables(НомерИзделия).Columns.Add("1ТипИзделия_2НомерИзделия", GetType(String), "Parent(1ТипИзделия_2НомерИзделия).keyТипИзделия")

            ds.Relations.Add("1ТипИзделия_2НомерИзделия", dt1TypeEngine.Columns(ConstKeyTypeEngine), dt2NumberEngine.Columns(ConstKeyTypeEngine), True)
            ds.Relations.Add("2НомерИзделия_3НомерСборки", dt2NumberEngine.Columns(ConstKeyNumberEngine), dt3NumberBuild.Columns(ConstKeyNumberEngine), True)
            ds.Relations.Add("3НомерСборки_4НомерПостановки", dt3NumberBuild.Columns(ConstKeyNumberBuild), dt4NumberStage.Columns(ConstKeyNumberBuild), True)
            ds.Relations.Add("4НомерПостановки_5НомерЗапуска", dt4NumberStage.Columns(ConstKeyNumberStage), dt5NumberStarting.Columns(ConstKeyNumberStage), True)
            ds.Relations.Add("5НомерЗапуска_6НомерКТ", dt5NumberStarting.Columns(ConstKeyNumberStarting), dt6NumberKT.Columns(ConstKeyNumberStarting), True)

            ' установить источник данных и связи для иерархических обновлений
            dt1CopyTypeEngine = dt1TypeEngine.Copy
            dt2CopyNumberEngine = dt2NumberEngine.Copy
            dt3CopyNumberBuild = dt3NumberBuild.Copy
            dt4CopyNumberStage = dt4NumberStage.Copy
            dt5CopyNumberStarting = dt5NumberStarting.Copy
            dt6CopyNumberKT = dt6NumberKT.Copy

            ''dsCopyTables.Clear() не работает
            'dsCopyTables = New DataSet

            'dsCopyTables.Tables.Add(dt1CopyTypeEngine)
            'dsCopyTables.Tables.Add(dt2CopyNumberEngine)
            'dsCopyTables.Tables.Add(dt3CopyNumberBuild)
            'dsCopyTables.Tables.Add(dt4CopyNumberStage)
            'dsCopyTables.Tables.Add(dt5CopyNumberStarting)
            'dsCopyTables.Tables.Add(dt6CopyNumberKT)

            TypeEngine1BindingSource.DataSource = dt1CopyTypeEngine
            NumberEngine2BindingSource.DataSource = dt2CopyNumberEngine
            NumberBuild3BindingSource.DataSource = dt3CopyNumberBuild
            NumberStage4BindingSource.DataSource = dt4CopyNumberStage
            NumberStarting5BindingSource.DataSource = dt5CopyNumberStarting
            NumberKT6BindingSource.DataSource = dt6CopyNumberKT

            DataGrid1TypeEngine.DataSource = TypeEngine1BindingSource
            DataGrid1TypeEngine.Refresh()
            DataGrid2NumberEngine.DataSource = NumberEngine2BindingSource
            DataGrid2NumberEngine.Refresh()
            DataGrid3NumberBuild.DataSource = NumberBuild3BindingSource
            DataGrid3NumberBuild.Refresh()
            DataGrid4NumberStage.DataSource = NumberStage4BindingSource
            DataGrid4NumberStage.Refresh()
            DataGrid5NumberStarting.DataSource = NumberStarting5BindingSource
            DataGrid5NumberStarting.Refresh()
            DataGrid6NumberKT.DataSource = NumberKT6BindingSource
            DataGrid6NumberKT.Refresh()
            IsAllowSelectionChanged = True

            ' таблица может не содержать записей при первом запуске поэтому сделано обновления (TableStyles - карта столбцов) при каждом новом запросе
            If dt1CopyTypeEngine.Rows.Count > 0 Then SetColumnVisible(dt1CopyTypeEngine, DataGrid1TypeEngine)
            If dt2CopyNumberEngine.Rows.Count > 0 Then SetColumnVisible(dt2CopyNumberEngine, DataGrid2NumberEngine)
            If dt3CopyNumberBuild.Rows.Count > 0 Then SetColumnVisible(dt3CopyNumberBuild, DataGrid3NumberBuild)
            If dt4CopyNumberStage.Rows.Count > 0 Then SetColumnVisible(dt4CopyNumberStage, DataGrid4NumberStage)
            If dt5CopyNumberStarting.Rows.Count > 0 Then SetColumnVisible(dt5CopyNumberStarting, DataGrid5NumberStarting)
            If dt6CopyNumberKT.Rows.Count > 0 Then SetColumnVisible(dt6CopyNumberKT, DataGrid6NumberKT)

            ' чтобы отобразить таблицы КТ если они есть  по цепочке
            DataGrid1TypeEngine_CurrentRowChanged()
        Catch ex As Exception
            Cursor.Current = currentCursor
            MessageBox.Show(ex.ToString, "Отображение запроса", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            Cursor.Current = currentCursor
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
    End Sub

#Region "Навигация"
    Private Sub FormHierarchicalTable_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Right Then NextRecord()
        If e.KeyCode = Keys.Left Then PreviousRecord()
        If e.KeyCode = Keys.Home Then FirstRecord()
        If e.KeyCode = Keys.End Then LastRecord()
    End Sub

    Protected Sub ShowCurrentRecordNumber()
        'lblPosition.Text = "Контр.точка №: " & _
        '    Me.BindingContext(dtИзмеренные).Position + 1 & " из " & _
        '        dtИзмеренные.Rows.Count
        BindingNavigatorPositionItem.Text = "Контр.точка №: " & MeasurementBindingSource.Position + 1
        'BindingNavigatorCountItem.Text = "из {" & dtИзмеренные.Rows.Count & "}"
        BindingNavigatorCountItem.Text = String.Format("из {{{0}}}", MeasurementBindingSource.Count)
        'lblPosition.Text = "Row " & (mCurrencyManager.Position + 1) & " of " & mCurrencyManager.Count
    End Sub

    ''' <summary>
    ''' Подсветить DataGrid6 Номер Контрольной Точки
    ''' </summary>
    Private Sub SetPositionDataGrid6NumberKT()
        Dim success As Boolean = False
        Dim numberKT As Integer = CInt(dtMeasurement.Rows(MeasurementBindingSource.Position).Item(ConstNumberKT))
        Dim position As Integer

        keyNumberKT = CInt(dtMeasurement.Rows(MeasurementBindingSource.Position).Item(ConstKeyNumberKT))

        For I As Integer = 0 To dt6CopyNumberKT.Rows.Count - 1
            If CInt(dt6CopyNumberKT.Rows(I).Item(2)) = numberKT Then
                success = True
                position = I
                Exit For
            End If
        Next

        If success Then
            IsAllowSelectionChanged = False
            NumberKT6BindingSource.Position = position
            IsAllowSelectionChanged = True
        End If
    End Sub

    ''' <summary>
    ''' Подсветить Расчетные DataGrid
    ''' </summary>
    ''' <param name="tbl"></param>
    ''' <param name="bndSource"></param>
    ''' <param name="dgView"></param>
    ''' <param name="numberKT"></param>
    Private Sub SetPositionDataGrid(ByVal tbl As DataTable,
                                            ByVal bndSource As BindingSource,
                                            ByVal dgView As DataGridView,
                                            ByVal numberKT As Integer)
        ' поиск не в таблице а в сетке
        For rowIndex As Integer = 0 To dgView.Rows.Count - 1
            If CInt(dgView.Item(1, rowIndex).Value) = numberKT Then
                bndSource.Position = rowIndex
                Exit For
            End If
        Next
    End Sub

    Private Sub FirstRecord()
        Try
            If MeasurementBindingSource.Count > 0 Then
                MeasurementBindingSource.MoveFirst()
                Dim numberKT As Integer = CInt(DataGridMeasurementParameters.Item(1, DataGridMeasurementParameters.CurrentRow.Index).Value)
                SetPositionDataGrid(dtCast, CastBindingSource, DataGridCastParameters, numberKT)
                SetPositionDataGrid(dtConverting, ConvertingBindingSource, DataGridConvertingParameters, numberKT)
            End If

            SetPositionDataGrid6NumberKT()
            ShowCurrentRecordNumber()
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Навигация", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub LastRecord()
        Try
            If MeasurementBindingSource.Count > 0 Then
                MeasurementBindingSource.MoveLast()
                Dim numberKT As Integer = CInt(DataGridMeasurementParameters.Item(1, DataGridMeasurementParameters.CurrentRow.Index).Value)
                SetPositionDataGrid(dtCast, CastBindingSource, DataGridCastParameters, numberKT)
                SetPositionDataGrid(dtConverting, ConvertingBindingSource, DataGridConvertingParameters, numberKT)
            End If

            SetPositionDataGrid6NumberKT()
            ShowCurrentRecordNumber()
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Навигация", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub PreviousRecord()
        Try
            If MeasurementBindingSource.Count > 0 Then
                If MeasurementBindingSource.Position > 0 Then
                    MeasurementBindingSource.MovePrevious()
                    Dim numberKT As Integer = CInt(DataGridMeasurementParameters.Item(1, DataGridMeasurementParameters.CurrentRow.Index).Value)
                    SetPositionDataGrid(dtCast, CastBindingSource, DataGridCastParameters, numberKT)
                    SetPositionDataGrid(dtConverting, ConvertingBindingSource, DataGridConvertingParameters, numberKT)
                Else
                    Beep()
                End If
            End If

            SetPositionDataGrid6NumberKT()
            ShowCurrentRecordNumber()
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Навигация", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub NextRecord()
        Try
            If MeasurementBindingSource.Count > 0 Then
                If MeasurementBindingSource.Position + 1 < MeasurementBindingSource.Count Then
                    MeasurementBindingSource.MoveNext()
                    Dim numberKT As Integer = CInt(DataGridMeasurementParameters.Item(1, DataGridMeasurementParameters.CurrentRow.Index).Value)
                    SetPositionDataGrid(dtCast, CastBindingSource, DataGridCastParameters, numberKT)
                    SetPositionDataGrid(dtConverting, ConvertingBindingSource, DataGridConvertingParameters, numberKT)
                Else
                    Beep()
                End If
            End If

            SetPositionDataGrid6NumberKT()
            ShowCurrentRecordNumber()
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Навигация", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub BindingNavigatorMoveFirstItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BindingNavigatorMoveFirstItem.Click
        FirstRecord()
    End Sub

    Private Sub BindingNavigatorMovePreviousItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BindingNavigatorMovePreviousItem.Click
        PreviousRecord()
    End Sub

    Private Sub BindingNavigatorMoveNextItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BindingNavigatorMoveNextItem.Click
        NextRecord()
    End Sub

    Private Sub BindingNavigatorMoveLastItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BindingNavigatorMoveLastItem.Click
        LastRecord()
    End Sub

    Private Sub TSButtonShowFindChannels_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonShowFindChannels.Click
        If keyNumberKT = 0 Then
            MessageBox.Show("Нет выделенной контрольной точки.", "Отобразить все каналы", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            Dim mfrmFindChannel As New FormFindChannel(keyNumberKT, FormParrent.Manager)
            If mfrmFindChannel.ShowDialog = DialogResult.OK Then
                mfrmFindChannel.Dispose()
            End If
        End If
    End Sub
#End Region

#Region "Стиль"
    ''' <summary>
    ''' Скрыть ключевые стобцы
    ''' </summary>
    ''' <param name="tbl"></param>
    ''' <param name="datagrid"></param>
    Private Sub SetColumnVisible(ByVal tbl As DataTable, ByVal datagrid As DataGridView)
        ' Подбираем в цикле ширину всех колонок после чего ширину менять нельзя
        For Each col As DataColumn In tbl.Columns
            'datagrid.Columns(col.Caption).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            If col.Caption.IndexOf(cKey) <> -1 Then datagrid.Columns(col.Caption).Visible = False
        Next
    End Sub

    ''' <summary>
    ''' Установить стиль ячеек таблицы
    ''' </summary>
    ''' <param name="cn"></param>
    ''' <param name="datagrid"></param>
    ''' <param name="strSQL"></param>
    Public Sub SetColumStyleByType(ByRef cn As OleDbConnection, ByRef datagrid As DataGridView, ByVal strSQL As String)
        Dim cmd As OleDbCommand = cn.CreateCommand
        Dim rdr As OleDbDataReader = Nothing
        Dim row As DataRow
        Dim colunmName As String
        Dim tblMetaData As DataTable

        cmd.CommandText = strSQL
        ' получить схему таблицы
        Try
            rdr = cmd.ExecuteReader(CommandBehavior.KeyInfo Or CommandBehavior.SchemaOnly)
        Catch ex As Exception
            Try
                rdr = cmd.ExecuteReader(CommandBehavior.SchemaOnly)
            Catch ex2 As Exception
                MessageBox.Show(ex2.Message, "Невозможно собрать метаданные для результирующего запроса",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Try

        tblMetaData = rdr.GetSchemaTable

        'Dim col As DataColumn
        'For Each col In tblMetaData.Columns
        '    Console.WriteLine(col.Caption)
        'Next
        'Console.WriteLine(tblMetaData.Columns("ColumnName"))
        'ColumnName()
        'ColumnOrdinal()
        'ColumnSize()
        'NumericPrecision()
        'NumericScale()
        'DataType()
        'ProviderType()
        'IsLong()
        'AllowDBNull()
        'IsReadOnly()
        'IsRowVersion()
        'IsUnique()
        'IsKey()
        'IsAutoIncrement()
        'BaseSchemaName()
        'BaseCatalogName()
        'BaseTableName()
        'BaseColumnName()

        ' для каждого столбца создаем стиль
        For Each row In tblMetaData.Rows
            Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
            colunmName = CStr(row("ColumnName"))
            Dim column As DataGridViewColumn = datagrid.Columns(colunmName)

            Select Case CType(row("ProviderType"), OleDbType)
                Case OleDbType.VarWChar
                    column.Width = 50
                    Exit Select
                Case OleDbType.LongVarWChar
                    column.Width = 150
                    Exit Select
                Case OleDbType.Single
                    column.Width = 70
                    column.DefaultCellStyle.Format = "###0.000"
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    Exit Select
                Case OleDbType.Double
                    column.Width = 70
                    column.DefaultCellStyle.Format = "###0.000"
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    Exit Select
                Case OleDbType.SmallInt
                    column.Width = 40
                    Exit Select
                Case OleDbType.Integer
                    column.Width = 40
                    Exit Select
                Case OleDbType.Date
                    column.Width = 80

                    'Dim cell As DataGridViewCell = New DataGridViewTextBoxCell()

                    ''cell.Style.BackColor = Color.Wheat
                    ''1
                    'cell.Style.Format = "F"
                    'column.CellTemplate = cell
                    ''2
                    'datagrid.Columns(strИмяСтолбца).DefaultCellStyle.Format = "###0.000" ' "F"
                    ''3
                    'Dim myStyle As DataGridViewCellStyle = New DataGridViewCellStyle()
                    'myStyle.BackColor = Color.DarkOrange
                    'column.DefaultCellStyle = myStyle

                    If colunmName.IndexOf("Дата") <> -1 Then
                        column.DefaultCellStyle.Format = "dd-MM-yyyy"
                    Else
                        column.DefaultCellStyle.Format = "T"
                    End If
                    Exit Select
                Case OleDbType.Boolean
                    column.Width = 30
                    Exit Select
            End Select

            If colunmName.IndexOf(cKey) <> -1 OrElse colunmName.IndexOf("Код") <> -1 Then
                column.Visible = False
            End If
        Next

        rdr.Close()
    End Sub
#End Region

#Region "Отчёт"
    ''' <summary>
    ''' Сформировать ReportManager.ListReports на основе копий таблиц по результатам применения фильтра.
    ''' Вывести форму с шалоном отчёта использующим 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ToolStripButtonReport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolStripButtonReport.Click
        ReportManager.ListReports.Clear()

        AddReportManagerListReports(StageGridType.ТипыИзделия1, dt1CopyTypeEngine, TypeEngine1BindingSource)
        AddReportManagerListReports(StageGridType.НомерИзделия2, dt2CopyNumberEngine, NumberEngine2BindingSource)
        AddReportManagerListReports(StageGridType.НомерСборки3, dt3CopyNumberBuild, NumberBuild3BindingSource)
        AddReportManagerListReports(StageGridType.НомерПостановки4, dt4CopyNumberStage, NumberStage4BindingSource)
        AddReportManagerListReports(StageGridType.НомерЗапуска5, dt5CopyNumberStarting, NumberStarting5BindingSource)
        AddReportManagerListReports(StageGridType.НомерКТ6, dt6CopyNumberKT, NumberKT6BindingSource)

        AddReportManagerListReports(StageGridType.Измеренные, dtMeasurement, MeasurementBindingSource)
        AddReportManagerListReports(StageGridType.Приведенные, dtCast, CastBindingSource)
        AddReportManagerListReports(StageGridType.Пересчитанные, dtConverting, ConvertingBindingSource)

        Dim mReportDialog = New ReportDialog With {
            .ReportParameter = ReportParameter
        } ' там инициализация и загрузка сеток данными InitializeGrids
        mReportDialog.ShowDialog(Me)
    End Sub

    ''' <summary>
    ''' Заполнить источник данных для отчёта
    ''' </summary>
    ''' <param name="numberPhase"></param>
    ''' <param name="tbl"></param>
    ''' <param name="TableBindingSource"></param>
    Private Sub AddReportManagerListReports(ByVal numberPhase As Integer, ByVal tbl As DataTable, ByVal TableBindingSource As BindingSource)
        If numberPhase <= StageGridType.НомерЗапуска5 Then
            Dim row As DataRow = CType(TableBindingSource.Current, DataRowView).Row

            For Each col As DataColumn In tbl.Columns
                If col.Caption.IndexOf(cKey) = -1 AndAlso col.Caption.IndexOf("_") = -1 Then
                    ReportManager.ListReports.Add(New RecordReport(numberPhase.ToString & ControlStageNamesConst(numberPhase - 1),
                                                             "0",
                                                             col.ColumnName,
                                                             row(col.ColumnName).ToString))
                End If
            Next
        Else
            ' при конструировании шаблона отчёта Report1.rdlc при задании свойства текстового поля таблицы 
            ' выражением <<Expr>>:
            ' =First(Round(CDbl(Fields!ЗначениеПользователя.Value),3))
            ' =Round(CDbl(Fields!CustomerValue.Value), 3)
            ' Свойство текстового поля - Число
            ' Пользовательский формат ячейки - F
            ' при выводе второго листа отчёта появляются ошибка в значениях ячеек.
            ' Поэтому применено предварительное форматирование для цифровых значений параметров: Измеренные, Приведенные, Пересчитанные.
            For Each row As DataRow In tbl.Rows
                For Each col As DataColumn In tbl.Columns
                    If col.Caption.IndexOf(cKey) = -1 AndAlso col.Caption.IndexOf(cNumber_KT) = -1 Then
                        ReportManager.ListReports.Add(New RecordReport(ControlStageNamesConst(numberPhase - 1),
                                                                 row(ConstNumberKT).ToString,
                                                                 col.ColumnName,
                                                                 If(numberPhase = StageGridType.НомерКТ6,
                                                                 CStr(row(col.ColumnName)),
                                                                 CStr(Math.Round(CDbl(row(col.ColumnName).ToString), 3)))))
                    End If
                Next
            Next
        End If
    End Sub
#End Region
End Class

'DataGrid.SelectedRows(0).Cells(1)
'DataGrid.Rows(2).Cells(1).Value
'DataGrid.SelectedCells(1).Value
'DataGrid.Item(ByVal columnIndex As Integer, ByVal rowIndex As Integer) As System.Windows.Forms.DataGridViewCell
'DataGrid.Item(DataGrid.CurrentCell.RowIndex, 1).Value
'DataGrid.CurrentCell.RowIndex
'DataGrid.CurrentCell.Selected = True
'DataGrid.CurrentRow.Selected = True
'DataGrid.CurrentRow.Index > 0


'    Прописывают базу данных поля, прописываю саму базу данных. А потом загружают его во вьюер напрямую: reportViewer.LocalReport.LoadReportDefinition(...). 


'Я не до конца понял, что Вам нужно в конечном итоге? если нужно создавать отчеты "на лету" изучайте Report Definition Language http://msdn.microsoft.com/en-us/library/dd297486.aspx - именно его и использует Microsoft Reporting Services. 
'А если же интересует замещение датасетов на кастомные, то вот код которые замещает оных: 

'SqlDataAdapter adapter = new SqlDataAdapter( sqlCommand );
'dataTable.Clear();
'adapter.Fill( dataTable );
'reportDataSource = new ReportDataSource( reportDataSetName );
'reportDataSource.Value = dataTable;
'reportViewer.ProcessingMode = ProcessingMode.Local;
'reportViewer.LocalReport.ReportPath = reportPath;
'reportViewer.LocalReport.DataSources.Clear();
'reportViewer.LocalReport.DataSources.Add( reportDataSource );
'reportViewer.RefreshReport();



'А так я сам использую в своих проектах вот эту opensource отчетную систему http://www.fyireporting.com/



'*******************************
'public report(DataTable tbl)
' {
'   ReportViewer reportviewer = new ReportViewer();
'   reportviewer.Parent = this;
'   // Указываем путь к макету отчета – файлу с расширением .rdlc
'   reportviewer.LocalReport.ReportPath = @"...\Report1.rdlc";
'   // Добавляем источник данных
'   // tbl – объект DataTable с данными
'   // DataSet1_shipment – название источника данных в макете отчета
'   reportviewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_shipment", tbl));
'   reportviewer.RefreshReport();
' }


'Me.BindingSourceAll.DataSource = Me.ds.Tables(ТипИзделия)
''или
''Me.BindingSourceAll.DataSource = Me.ds
''Me.BindingSourceAll.DataMember = ТипИзделия


'DataGrid1ТипИзделия.DataSource = Me.BindingSourceAll

'Me.bindingNavigator.BindingSource = Me.BindingSourceAll
'DataGrid1ТипИзделия.DataSource = Me.BindingSourceAll
'BindingSourceAll.CurrencyManager.Position
'BindingSourceAll.Position

'pageCountTextBox.DataBindings.Add("Text", bookInfoList, "PageCount")
'Таким образом, при необходимости привязки свойства Text к колонке ProductName таблицы Products мы можем смело писать:
'label1.DataBindings.Add(New Binding("Text", BindingSourceAll, "ProductName", True))
'            Так же обстоит дело со сложной привязкой той же колонки к control-у, поддерживающему подобную привязку:

'this.comboBox1.DataSource = _biSour;
'this.comboBox1.DisplayMember = "ProductName";


'dataGridView.AutoGenerateColumns = false; 
'BindingSource bs = new BindingSource(); 
'bs.DataSource = dataGridStruct; 
'dataGridView.DataSource = bs; 
'dataGridView.Refresh(); 






'Sub BindProductsGrid()
'    ' Get the current SupplierID by using the DataGrid's CurrentRowIndex, i.e.,
'    ' the currently selected row, and using it to match the row in the 
'    ' DataView. Then access the "SupplierID" column to get the SupplierID 
'    ' for that DataRowView.
'    Dim strCurrentSupplierID As String = _
'        dvSupplier(grdSuppliers.CurrentRowIndex)("SupplierID").ToString
'    ' Filter the OrderDetails data based on the currently selected OrderID.
'    '   Since empty SupplierID's are possible (if the user is adding a new supplier)
'    '   these must be taken into consideration
'    If strCurrentSupplierID = "" Then
'        strCurrentSupplierID = "-1"
'    End If
'    ' Filter the products to display only those with the appropriate
'    '   SupplierID
'    dvProduct.RowFilter = "SupplierID = " & strCurrentSupplierID
'    ' Put a caption on the Grid, and attach a DataSource
'    With grdProducts
'        .CaptionText = "Products"
'        .DataSource = dvProduct
'    End With
'End Sub


'Dim columns() As String = {"ID", "IntegerValue2"}
'If myMainDataset.Tables("MyDataViewTable").Rows.Count > 0 Then
'    Dim myDataTable As DataTable = myMainDataset.Tables("MyDataViewTable").DefaultView.ToTable("MySmallTable", True, columns)
'    ' связатьновую DataTable к DataGridView показывая только 2 колонки
'    dataViewDataGridView.DataSource = myDataTable

'    GroupsOfArticlesDataGridView.AutoGenerateColumns = false ;
'    BindingSource bindings = new BindingSource();
'    bindings.DataSource = groupsTable;  

'    this.GroupsOfArticlesDataGridView.DataSource= bindings;
'    this.GroupsOfArticlesDataGridView.Columns[0].DataPropertyName="nameCategoryCustom";



'Private Sub Panel1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel1.Resize
'    Dim arrGrid() As DataGrid = New DataGrid() {DataGrid1ТипИзделия, DataGrid2НомерИзделия, DataGrid3НомерСборки, DataGrid4НомерПостановки, DataGrid5НомерЗапуска, DataGrid6НомерКонтрольнойТочки}
'    Dim cx As Integer = Panel1.Width / 3
'    Dim cy As Integer = Panel1.Height / 2
'    Dim row, col As Integer
'    For row = 0 To 1
'        For col = 0 To 2
'            arrGrid(col + row * 3).SetBounds(cx * col, cy * row, cx, cy)
'            'Console.WriteLine(col + row * 3)
'        Next
'    Next
'    'For row = 0 To 2
'    '    For col = 0 To 2
'    '        'arrGrid(col * 3 + row).SetBounds(cx * row, cy * col, cx, cy)
'    '    Next
'    'Next
'End Sub

'Private Sub ОтобразитьЗапросOLD()
'    'strSQLЗапрос = " " & _
'    '" " & _
'    '" " & _
'    'strWHERE & _
'    '" " & _
'    '""
'    'Dim strИмяТаблицы As String
'    'strShape = "SHAPE {SELECT НомерИзделия,Описание,keyНомерИзделия FROM `2НомерИзделия` " & strЗапросНомерИзделия & " }  AS НомерИзделия " & "APPEND (( SHAPE {SELECT НомерСборки,Цель,Отличие,ПараметрFса,keyНомерИзделия,keyНомерСборки FROM `3НомерСборки` " & strЗапросНомерСборки & " }  AS НомерСборки " & "APPEND (( SHAPE {SELECT НомерПостановки,Цель,НомерБокса,НомерВходногоУстройства,ТипТоплива,Описание,keyНомерСборки,keyНомерПостановки FROM `4НомерПостановки` " & strЗапросНомерПостановки & " }  AS НомерПостановки " & "APPEND (( SHAPE {SELECT НомерЗапуска,Цель,ОтличиеОтПредыдущего,format(Дата,'dd.mm.yy'),Format(Время, 'h:m:s'),Т4охл,Т4ХП,DрсНаНМУ,DрсНаВМУ,keyНомерПостановки,keyНомерЗапуска FROM `5НомерЗапуска` " & strЗапросНомерЗапуска & " }  AS НомерЗапуска " & "APPEND ({SELECT НомерКонтрТочки,Режим,ВидИспытания,ТипИспытания,ВремяОсреднения,ВремяНачала,ВремяКонца,n1огр,n2огр,n2ВклОхл,ВклФорсажа,Т4огр,Т4Фогр,NктМаксимала,СтолбыИЛИдавленияМиП,СечениеМилиСечениеВ,ЭлектрическоеОсреднение,ВторойПересчет,ИмяАлгоритмаРасчета,К1_n1сау,К2_n2сау,К3_Gвсумсау,К4_R,К5_Gт,К6_Rсау,К7_Crсау,К8_PIКНДсау,К9_PIКсумсау,К10_mсау,К12_Т4сау,К13_PIтсау,К14_Т3,К15_Т3сау,К16_асумпер,keyНомерЗапуска,keyНомерКонтрТочки FROM `6НомерКонтрольнойТочки` " & strЗапросНомерКонтрТочки & " }  AS НомерКТ " & "RELATE 'keyНомерЗапуска' TO 'keyНомерЗапуска') AS НомерКТ) AS НомерЗапуска " & "RELATE 'keyНомерПостановки' TO 'keyНомерПостановки') AS НомерЗапуска) AS НомерПостановки " & "RELATE 'keyНомерСборки' TO 'keyНомерСборки') AS НомерПостановки) AS НомерСборки " & "RELATE 'keyНомерИзделия' TO 'keyНомерИзделия') AS НомерСборки"
'    Dim strWHEREЗапросТипИзделия As String = ""
'    Dim strWHEREЗапросНомерИзделия As String = ""
'    Dim strWHEREЗапросНомерСборки As String = ""
'    Dim strWHEREЗапросНомерПостановки As String = ""
'    Dim strWHEREЗапросНомерЗапуска As String = ""
'    Dim strWHEREЗапросНомерКонтрТочки As String = ""

'    Dim strSQLОбщая As String
'    Dim strSQLЗапросТипИзделия As String
'    Dim strSQLЗапросНомерИзделия As String
'    Dim strSQLЗапросНомерСборки As String
'    Dim strSQLЗапросНомерПостановки As String
'    Dim strSQLЗапросНомерЗапуска As String
'    Dim strSQLЗапросНомерКонтрТочки As String

'    Dim currentCursor As Cursor = Windows.Forms.Cursor.Current
'    Windows.Forms.Cursor.Current = Cursors.WaitCursor
'    BindingNavigatorPositionItem.Text = ""

'    '1**************************

'    If _ЗапросТипИзделия <> "" Then
'        strWHEREЗапросТипИзделия = " WHERE (" & _ЗапросТипИзделия & ") "
'    End If
'    strSQLЗапросТипИзделия = "TRANSFORM First(ЗначенияКонтроловДляТипИзделия.ЗначениеПользователя) AS [First-ЗначениеПользователя] " & _
'    "SELECT [1ТипИзделия].keyТипИзделия, [1ТипИзделия].ТипИзделия " & _
'    "FROM КонтролыДляТипИзделия RIGHT JOIN (1ТипИзделия RIGHT JOIN ЗначенияКонтроловДляТипИзделия ON [1ТипИзделия].keyТипИзделия = ЗначенияКонтроловДляТипИзделия.keyУровень) ON КонтролыДляТипИзделия.keyКонтролДляУровня = ЗначенияКонтроловДляТипИзделия.keyКонтролДляУровня " & _
'    strWHEREЗапросТипИзделия & _
'    "GROUP BY [1ТипИзделия].keyТипИзделия, [1ТипИзделия].ТипИзделия " & _
'    "PIVOT КонтролыДляТипИзделия.Text;"


'    '2**************************
'    'If strЗапросНомерИзделия <> "" Then
'    '    strWHEREЗапросНомерИзделия = " WHERE (" & strЗапросНомерИзделия & ") "
'    'End If

'    strWHEREЗапросНомерИзделия = strWHEREЗапросТипИзделия
'    If _ЗапросНомерИзделия <> "" Then
'        If strWHEREЗапросНомерИзделия.IndexOf("WHERE") > 0 Then
'            strWHEREЗапросНомерИзделия &= " AND (" & _ЗапросНомерИзделия & ") "
'        Else
'            strWHEREЗапросНомерИзделия = " WHERE (" & _ЗапросНомерИзделия & ") "
'        End If
'    End If
'    'strSQLЗапросНомерИзделия = "SELECT [2НомерИзделия].* " & _
'    '"FROM 1ТипИзделия RIGHT JOIN 2НомерИзделия ON [1ТипИзделия].keyТипИзделия=[2НомерИзделия].keyТипИзделия " & _
'    'strWHEREЗапросНомерИзделия

'    strSQLЗапросНомерИзделия = "TRANSFORM First(ЗначенияКонтроловДляНомерИзделия.ЗначениеПользователя) AS [First-ЗначениеПользователя] " & _
'    "SELECT [2НомерИзделия].keyТипИзделия, [2НомерИзделия].keyНомерИзделия, [2НомерИзделия].НомерИзделия " & _
'    "FROM КонтролыДляНомерИзделия RIGHT JOIN (2НомерИзделия RIGHT JOIN ЗначенияКонтроловДляНомерИзделия ON [2НомерИзделия].keyНомерИзделия = ЗначенияКонтроловДляНомерИзделия.keyУровень) ON КонтролыДляНомерИзделия.keyКонтролДляУровня = ЗначенияКонтроловДляНомерИзделия.keyКонтролДляУровня " & _
'    strWHEREЗапросНомерИзделия & _
'    "GROUP BY [2НомерИзделия].keyТипИзделия, [2НомерИзделия].keyНомерИзделия, [2НомерИзделия].НомерИзделия " & _
'    "PIVOT КонтролыДляНомерИзделия.Text;"

'    '3*************************
'    strWHEREЗапросНомерСборки = strWHEREЗапросНомерИзделия
'    If _ЗапросНомерСборки <> "" Then
'        If strWHEREЗапросНомерСборки.IndexOf("WHERE") > 0 Then
'            strWHEREЗапросНомерСборки &= " AND (" & _ЗапросНомерСборки & ") "
'        Else
'            strWHEREЗапросНомерСборки = " WHERE (" & _ЗапросНомерСборки & ") "
'        End If
'    End If
'    'strSQLЗапросНомерСборки = "SELECT [3НомерСборки].* " & _
'    '"FROM (1ТипИзделия RIGHT JOIN 2НомерИзделия ON [1ТипИзделия].keyТипИзделия=[2НомерИзделия].keyТипИзделия) RIGHT JOIN 3НомерСборки ON [2НомерИзделия].keyНомерИзделия=[3НомерСборки].keyНомерИзделия " & _
'    'strWHEREЗапросНомерСборки

'    'TRANSFORM First(ЗначенияКонтроловДляНомерСборки.ЗначениеПользователя) AS [First-ЗначениеПользователя]
'    'SELECT [3НомерСборки].keyНомерИзделия, [3НомерСборки].keyНомерСборки, [3НомерСборки].НомерСборки
'    'FROM 2НомерИзделия RIGHT JOIN (КонтролыДляНомерСборки RIGHT JOIN (3НомерСборки RIGHT JOIN ЗначенияКонтроловДляНомерСборки ON [3НомерСборки].keyНомерСборки = ЗначенияКонтроловДляНомерСборки.keyУровень) ON КонтролыДляНомерСборки.keyКонтролДляУровня = ЗначенияКонтроловДляНомерСборки.keyКонтролДляУровня) ON [2НомерИзделия].keyНомерИзделия = [3НомерСборки].keyНомерИзделия
'    'WHERE ((([3НомерСборки].НомерСборки)=1) AND (([2НомерИзделия].НомерИзделия)="Первое в серии"))
'    'GROUP BY [3НомерСборки].keyНомерИзделия, [3НомерСборки].keyНомерСборки, [3НомерСборки].НомерСборки
'    'PIVOT КонтролыДляНомерСборки.Text;

'    strSQLЗапросНомерСборки = "TRANSFORM First(ЗначенияКонтроловДляНомерСборки.ЗначениеПользователя) AS [First-ЗначениеПользователя] " & _
'    "SELECT [3НомерСборки].keyНомерИзделия, [3НомерСборки].keyНомерСборки, [3НомерСборки].НомерСборки " & _
'    "FROM 2НомерИзделия RIGHT JOIN (КонтролыДляНомерСборки RIGHT JOIN (3НомерСборки RIGHT JOIN ЗначенияКонтроловДляНомерСборки ON [3НомерСборки].keyНомерСборки = ЗначенияКонтроловДляНомерСборки.keyУровень) ON КонтролыДляНомерСборки.keyКонтролДляУровня = ЗначенияКонтроловДляНомерСборки.keyКонтролДляУровня) ON [2НомерИзделия].keyНомерИзделия = [3НомерСборки].keyНомерИзделия " & _
'    strWHEREЗапросНомерСборки & _
'    "GROUP BY [3НомерСборки].keyНомерИзделия, [3НомерСборки].keyНомерСборки, [3НомерСборки].НомерСборки " & _
'    "PIVOT КонтролыДляНомерСборки.Text;"


'    '4***************************
'    strWHEREЗапросНомерПостановки = strWHEREЗапросНомерСборки
'    If _ЗапросНомерПостановки <> "" Then
'        If strWHEREЗапросНомерПостановки.IndexOf("WHERE") > 0 Then
'            strWHEREЗапросНомерПостановки &= " AND (" & _ЗапросНомерПостановки & ") "
'        Else
'            strWHEREЗапросНомерПостановки = " WHERE (" & _ЗапросНомерПостановки & ") "
'        End If
'    End If
'    'strSQLЗапросНомерПостановки = "SELECT [4НомерПостановки].* " & _
'    '"FROM ((1ТипИзделия RIGHT JOIN 2НомерИзделия ON [1ТипИзделия].keyТипИзделия=[2НомерИзделия].keyТипИзделия) RIGHT JOIN 3НомерСборки ON [2НомерИзделия].keyНомерИзделия=[3НомерСборки].keyНомерИзделия) RIGHT JOIN 4НомерПостановки ON [3НомерСборки].keyНомерСборки=[4НомерПостановки].keyНомерСборки " & _
'    'strWHEREЗапросНомерПостановки

'    'TRANSFORM First(ЗначенияКонтроловДляНомерПостановки.ЗначениеПользователя) AS [First-ЗначениеПользователя]
'    'SELECT [4НомерПостановки].keyНомерСборки, [4НомерПостановки].keyНомерПостановки, [4НомерПостановки].НомерПостановки
'    'FROM (2НомерИзделия RIGHT JOIN 3НомерСборки ON [2НомерИзделия].keyНомерИзделия = [3НомерСборки].keyНомерИзделия) RIGHT JOIN (КонтролыДляНомерПостановки RIGHT JOIN (4НомерПостановки RIGHT JOIN ЗначенияКонтроловДляНомерПостановки ON [4НомерПостановки].keyНомерПостановки = ЗначенияКонтроловДляНомерПостановки.keyУровень) ON КонтролыДляНомерПостановки.keyКонтролДляУровня = ЗначенияКонтроловДляНомерПостановки.keyКонтролДляУровня) ON [3НомерСборки].keyНомерСборки = [4НомерПостановки].keyНомерСборки
'    'WHERE ((([2НомерИзделия].НомерИзделия)="Первое в серии") AND (([3НомерСборки].НомерСборки)=1) AND (([4НомерПостановки].НомерПостановки)=1))
'    'GROUP BY [4НомерПостановки].keyНомерСборки, [4НомерПостановки].keyНомерПостановки, [4НомерПостановки].НомерПостановки
'    'PIVOT КонтролыДляНомерПостановки.Text;

'    strSQLЗапросНомерПостановки = "TRANSFORM First(ЗначенияКонтроловДляНомерПостановки.ЗначениеПользователя) AS [First-ЗначениеПользователя] " & _
'    "SELECT [4НомерПостановки].keyНомерСборки, [4НомерПостановки].keyНомерПостановки, [4НомерПостановки].НомерПостановки " & _
'    "FROM (2НомерИзделия RIGHT JOIN 3НомерСборки ON [2НомерИзделия].keyНомерИзделия = [3НомерСборки].keyНомерИзделия) RIGHT JOIN (КонтролыДляНомерПостановки RIGHT JOIN (4НомерПостановки RIGHT JOIN ЗначенияКонтроловДляНомерПостановки ON [4НомерПостановки].keyНомерПостановки = ЗначенияКонтроловДляНомерПостановки.keyУровень) ON КонтролыДляНомерПостановки.keyКонтролДляУровня = ЗначенияКонтроловДляНомерПостановки.keyКонтролДляУровня) ON [3НомерСборки].keyНомерСборки = [4НомерПостановки].keyНомерСборки " & _
'    strWHEREЗапросНомерПостановки & _
'    "GROUP BY [4НомерПостановки].keyНомерСборки, [4НомерПостановки].keyНомерПостановки, [4НомерПостановки].НомерПостановки " & _
'    "PIVOT КонтролыДляНомерПостановки.Text;"

'    '5*****************
'    strWHEREЗапросНомерЗапуска = strWHEREЗапросНомерПостановки
'    If _ЗапросНомерЗапуска <> "" Then
'        If strWHEREЗапросНомерЗапуска.IndexOf("WHERE") > 0 Then
'            strWHEREЗапросНомерЗапуска &= " AND (" & _ЗапросНомерЗапуска & ") "
'        Else
'            strWHEREЗапросНомерЗапуска = " WHERE (" & _ЗапросНомерЗапуска & ") "
'        End If
'    End If
'    'strSQLЗапросНомерЗапуска = "SELECT [5НомерЗапуска].* " & _
'    '"FROM (((1ТипИзделия RIGHT JOIN 2НомерИзделия ON [1ТипИзделия].keyТипИзделия=[2НомерИзделия].keyТипИзделия) RIGHT JOIN 3НомерСборки ON [2НомерИзделия].keyНомерИзделия=[3НомерСборки].keyНомерИзделия) RIGHT JOIN 4НомерПостановки ON [3НомерСборки].keyНомерСборки=[4НомерПостановки].keyНомерСборки) RIGHT JOIN 5НомерЗапуска ON [4НомерПостановки].keyНомерПостановки=[5НомерЗапуска].keyНомерПостановки " & _
'    'strWHEREЗапросНомерЗапуска

'    'TRANSFORM First(ЗначенияКонтроловДляНомерЗапуска.ЗначениеПользователя) AS [First-ЗначениеПользователя]
'    'SELECT [5НомерЗапуска].keyНомерПостановки, [5НомерЗапуска].keyНомерЗапуска, [5НомерЗапуска].НомерЗапуска
'    'FROM ((2НомерИзделия RIGHT JOIN 3НомерСборки ON [2НомерИзделия].keyНомерИзделия = [3НомерСборки].keyНомерИзделия) RIGHT JOIN 4НомерПостановки ON [3НомерСборки].keyНомерСборки = [4НомерПостановки].keyНомерСборки) RIGHT JOIN (КонтролыДляНомерЗапуска RIGHT JOIN (5НомерЗапуска RIGHT JOIN ЗначенияКонтроловДляНомерЗапуска ON [5НомерЗапуска].keyНомерЗапуска = ЗначенияКонтроловДляНомерЗапуска.keyУровень) ON КонтролыДляНомерЗапуска.keyКонтролДляУровня = ЗначенияКонтроловДляНомерЗапуска.keyКонтролДляУровня) ON [4НомерПостановки].keyНомерПостановки = [5НомерЗапуска].keyНомерПостановки
'    'WHERE ((([2НомерИзделия].НомерИзделия)="Первое в серии") AND (([3НомерСборки].НомерСборки)=1) AND (([4НомерПостановки].НомерПостановки)=1) AND (([5НомерЗапуска].НомерЗапуска)=1))
'    'GROUP BY [5НомерЗапуска].keyНомерПостановки, [5НомерЗапуска].keyНомерЗапуска, [5НомерЗапуска].НомерЗапуска
'    'PIVOT КонтролыДляНомерЗапуска.Text;

'    strSQLЗапросНомерЗапуска = "TRANSFORM First(ЗначенияКонтроловДляНомерЗапуска.ЗначениеПользователя) AS [First-ЗначениеПользователя] " & _
'    "SELECT [5НомерЗапуска].keyНомерПостановки, [5НомерЗапуска].keyНомерЗапуска, [5НомерЗапуска].НомерЗапуска " & _
'    "FROM ((2НомерИзделия RIGHT JOIN 3НомерСборки ON [2НомерИзделия].keyНомерИзделия = [3НомерСборки].keyНомерИзделия) RIGHT JOIN 4НомерПостановки ON [3НомерСборки].keyНомерСборки = [4НомерПостановки].keyНомерСборки) RIGHT JOIN (КонтролыДляНомерЗапуска RIGHT JOIN (5НомерЗапуска RIGHT JOIN ЗначенияКонтроловДляНомерЗапуска ON [5НомерЗапуска].keyНомерЗапуска = ЗначенияКонтроловДляНомерЗапуска.keyУровень) ON КонтролыДляНомерЗапуска.keyКонтролДляУровня = ЗначенияКонтроловДляНомерЗапуска.keyКонтролДляУровня) ON [4НомерПостановки].keyНомерПостановки = [5НомерЗапуска].keyНомерПостановки " & _
'    strWHEREЗапросНомерЗапуска & _
'    "GROUP BY [5НомерЗапуска].keyНомерПостановки, [5НомерЗапуска].keyНомерЗапуска, [5НомерЗапуска].НомерЗапуска " & _
'    "PIVOT КонтролыДляНомерЗапуска.Text;"

'    '6****************
'    'должен быть первым
'    'убрал так как таких полей нет 
'    'If strЗапросНомерКонтрТочки <> "" Then
'    '    strПодзапросНомерКонтрТочки = " AND " & strЗапросНомерКонтрТочки
'    'Else
'    '    strПодзапросНомерКонтрТочки = vbNullString
'    'End If

'    strWHEREЗапросНомерКонтрТочки = strWHEREЗапросНомерЗапуска
'    If _ЗапросНомерКонтрТочки <> "" Then
'        If strWHEREЗапросНомерКонтрТочки.IndexOf("WHERE") > 0 Then
'            strWHEREЗапросНомерКонтрТочки &= " AND (" & _ЗапросНомерКонтрТочки & ") "
'        Else
'            strWHEREЗапросНомерКонтрТочки = " WHERE (" & _ЗапросНомерКонтрТочки & ") "
'        End If
'    End If
'    'strSQLЗапросНомерКонтрТочки = "SELECT [6НомерКТ].* " & _
'    '"FROM ((((1ТипИзделия RIGHT JOIN 2НомерИзделия ON [1ТипИзделия].keyТипИзделия=[2НомерИзделия].keyТипИзделия) RIGHT JOIN 3НомерСборки ON [2НомерИзделия].keyНомерИзделия=[3НомерСборки].keyНомерИзделия) RIGHT JOIN 4НомерПостановки ON [3НомерСборки].keyНомерСборки=[4НомерПостановки].keyНомерСборки) RIGHT JOIN 5НомерЗапуска ON [4НомерПостановки].keyНомерПостановки=[5НомерЗапуска].keyНомерПостановки) RIGHT JOIN 6НомерКТ ON [5НомерЗапуска].keyНомерЗапуска=[6НомерКТ].keyНомерЗапуска " & _
'    'strWHEREЗапросНомерКонтрТочки

'    'TRANSFORM First(ЗначенияКонтроловДляНомерКТ.ЗначениеПользователя) AS [First-ЗначениеПользователя]
'    'SELECT [6НомерКТ].keyНомерЗапуска, [6НомерКТ].keyНомерКТ, [6НомерКТ].НомерКТ
'    'FROM (((2НомерИзделия RIGHT JOIN 3НомерСборки ON [2НомерИзделия].keyНомерИзделия = [3НомерСборки].keyНомерИзделия) RIGHT JOIN 4НомерПостановки ON [3НомерСборки].keyНомерСборки = [4НомерПостановки].keyНомерСборки) RIGHT JOIN 5НомерЗапуска ON [4НомерПостановки].keyНомерПостановки = [5НомерЗапуска].keyНомерПостановки) RIGHT JOIN (КонтролыДляНомерКТ RIGHT JOIN (6НомерКТ RIGHT JOIN ЗначенияКонтроловДляНомерКТ ON [6НомерКТ].keyНомерКТ = ЗначенияКонтроловДляНомерКТ.keyУровень) ON КонтролыДляНомерКТ.keyКонтролДляУровня = ЗначенияКонтроловДляНомерКТ.keyКонтролДляУровня) ON [5НомерЗапуска].keyНомерЗапуска = [6НомерКТ].keyНомерЗапуска
'    'WHERE ((([2НомерИзделия].НомерИзделия)="Первое в серии") AND (([3НомерСборки].НомерСборки)=1) AND (([4НомерПостановки].НомерПостановки)=1) AND (([5НомерЗапуска].НомерЗапуска)=1) AND (([6НомерКТ].НомерКТ)=159))
'    'GROUP BY [6НомерКТ].keyНомерЗапуска, [6НомерКТ].keyНомерКТ, [6НомерКТ].НомерКТ
'    'PIVOT КонтролыДляНомерКТ.Text;

'    strSQLЗапросНомерКонтрТочки = "TRANSFORM First(ЗначенияКонтроловДляНомерКТ.ЗначениеПользователя) AS [First-ЗначениеПользователя] " & _
'    "SELECT [6НомерКТ].keyНомерЗапуска, [6НомерКТ].keyНомерКТ, [6НомерКТ].НомерКТ " & _
'    "FROM (((2НомерИзделия RIGHT JOIN 3НомерСборки ON [2НомерИзделия].keyНомерИзделия = [3НомерСборки].keyНомерИзделия) RIGHT JOIN 4НомерПостановки ON [3НомерСборки].keyНомерСборки = [4НомерПостановки].keyНомерСборки) RIGHT JOIN 5НомерЗапуска ON [4НомерПостановки].keyНомерПостановки = [5НомерЗапуска].keyНомерПостановки) RIGHT JOIN (КонтролыДляНомерКТ RIGHT JOIN (6НомерКТ RIGHT JOIN ЗначенияКонтроловДляНомерКТ ON [6НомерКТ].keyНомерКТ = ЗначенияКонтроловДляНомерКТ.keyУровень) ON КонтролыДляНомерКТ.keyКонтролДляУровня = ЗначенияКонтроловДляНомерКТ.keyКонтролДляУровня) ON [5НомерЗапуска].keyНомерЗапуска = [6НомерКТ].keyНомерЗапуска " & _
'    strWHEREЗапросНомерКонтрТочки & _
'    "GROUP BY [6НомерКТ].keyНомерЗапуска, [6НомерКТ].keyНомерКТ, [6НомерКТ].НомерКТ " & _
'    "PIVOT КонтролыДляНомерКТ.Text;"

'    '1 самый последний здесь нет TRANSFORM потому что нет контролов?
'    'это неправильно
'    strSQLОбщая = "SELECT [1ТипИзделия].keyТипИзделия, [2НомерИзделия].keyНомерИзделия, [3НомерСборки].keyНомерСборки, [4НомерПостановки].keyНомерПостановки, [5НомерЗапуска].keyНомерЗапуска " & _
'    "FROM ((((1ТипИзделия RIGHT JOIN 2НомерИзделия ON [1ТипИзделия].keyТипИзделия=[2НомерИзделия].keyТипИзделия) RIGHT JOIN 3НомерСборки ON [2НомерИзделия].keyНомерИзделия=[3НомерСборки].keyНомерИзделия) RIGHT JOIN 4НомерПостановки ON [3НомерСборки].keyНомерСборки=[4НомерПостановки].keyНомерСборки) RIGHT JOIN 5НомерЗапуска ON [4НомерПостановки].keyНомерПостановки=[5НомерЗапуска].keyНомерПостановки) RIGHT JOIN 6НомерКТ ON [5НомерЗапуска].keyНомерЗапуска=[6НомерКТ].keyНомерЗапуска " & _
'    strWHEREЗапросНомерКонтрТочки

'    Try
'        ds = New DataSet

'        dt1ТипИзделия = New DataTable
'        dt2НомерИзделия = New DataTable
'        dt3НомерСборки = New DataTable
'        dt4НомерПостановки = New DataTable
'        dt5НомерЗапуска = New DataTable
'        dt6НомерКонтрольнойТочки = New DataTable

'        cn = New OleDbConnection(BuildCnnStr(strProviderJet, strПутьКТ))
'        cn.Open()

'        'добавить стиль
'        'strИмяТаблицы = "1ТипИзделия"
'        'strSQL = "SELECT * FROM " & ТипИзделия

'        'TRANSFORM First(ЗначенияКонтроловДляТипИзделия.ЗначениеПользователя) AS [First-ЗначениеПользователя]
'        'SELECT [1ТипИзделия].keyТипИзделия
'        'FROM КонтролыДляТипИзделия RIGHT JOIN (1ТипИзделия RIGHT JOIN ЗначенияКонтроловДляТипИзделия ON [1ТипИзделия].keyТипИзделия = ЗначенияКонтроловДляТипИзделия.keyУровень) ON КонтролыДляТипИзделия.keyКонтролДляУровня = ЗначенияКонтроловДляТипИзделия.keyКонтролДляУровня
'        'GROUP BY [1ТипИзделия].keyТипИзделия
'        'PIVOT КонтролыДляТипИзделия.Text;
'        'было
'        'strSQL = "TRANSFORM First(ЗначенияКонтроловДляТипИзделия.ЗначениеПользователя) AS [First-ЗначениеПользователя] " & _
'        '"SELECT [1ТипИзделия].keyТипИзделия " & _
'        '"FROM КонтролыДляТипИзделия RIGHT JOIN (1ТипИзделия RIGHT JOIN ЗначенияКонтроловДляТипИзделия ON [1ТипИзделия].keyТипИзделия = ЗначенияКонтроловДляТипИзделия.keyУровень) ON КонтролыДляТипИзделия.keyКонтролДляУровня = ЗначенияКонтроловДляТипИзделия.keyКонтролДляУровня " & _
'        '"GROUP BY [1ТипИзделия].keyТипИзделия " & _
'        '"PIVOT КонтролыДляТипИзделия.Text;"

'        'If DataGrid1ТипИзделия.TableStyles.Count = 0 Then DataGrid1ТипИзделия.TableStyles.Add(ДобавитьСтиль(cn, ТипИзделия, DataGrid1ТипИзделия, strSQL))
'        'oda1ТипИзделия = New OleDbDataAdapter(strSQL, cn)

'        If DataGrid1ТипИзделия.TableStyles.Count = 0 Then DataGrid1ТипИзделия.TableStyles.Add(ДобавитьСтиль(cn, ТипИзделия, DataGrid1ТипИзделия, strSQLЗапросТипИзделия))
'        oda1ТипИзделия = New OleDbDataAdapter(strSQLЗапросТипИзделия, cn)
'        If DataGrid2НомерИзделия.TableStyles.Count = 0 Then DataGrid2НомерИзделия.TableStyles.Add(ДобавитьСтиль(cn, НомерИзделия, DataGrid2НомерИзделия, strSQLЗапросНомерИзделия))
'        oda2НомерИзделия = New OleDbDataAdapter(strSQLЗапросНомерИзделия, cn)
'        If DataGrid3НомерСборки.TableStyles.Count = 0 Then DataGrid3НомерСборки.TableStyles.Add(ДобавитьСтиль(cn, НомерСборки, DataGrid3НомерСборки, strSQLЗапросНомерСборки))
'        oda3НомерСборки = New OleDbDataAdapter(strSQLЗапросНомерСборки, cn)
'        If DataGrid4НомерПостановки.TableStyles.Count = 0 Then DataGrid4НомерПостановки.TableStyles.Add(ДобавитьСтиль(cn, НомерПостановки, DataGrid4НомерПостановки, strSQLЗапросНомерПостановки))
'        oda4НомерПостановки = New OleDbDataAdapter(strSQLЗапросНомерПостановки, cn)
'        If DataGrid5НомерЗапуска.TableStyles.Count = 0 Then DataGrid5НомерЗапуска.TableStyles.Add(ДобавитьСтиль(cn, НомерЗапуска, DataGrid5НомерЗапуска, strSQLЗапросНомерЗапуска))
'        oda5НомерЗапуска = New OleDbDataAdapter(strSQLЗапросНомерЗапуска, cn)
'        If DataGrid6НомерКонтрольнойТочки.TableStyles.Count = 0 Then DataGrid6НомерКонтрольнойТочки.TableStyles.Add(ДобавитьСтиль(cn, НомерКТ, DataGrid6НомерКонтрольнойТочки, strSQLЗапросНомерКонтрТочки))
'        oda6НомерКонтрольнойТочки = New OleDbDataAdapter(strSQLЗапросНомерКонтрТочки, cn)

'        oda1ТипИзделия.Fill(ds, ТипИзделия)
'        oda2НомерИзделия.Fill(ds, НомерИзделия)
'        oda3НомерСборки.Fill(ds, НомерСборки)
'        oda4НомерПостановки.Fill(ds, НомерПостановки)
'        oda5НомерЗапуска.Fill(ds, НомерЗапуска)
'        oda6НомерКонтрольнойТочки.Fill(ds, НомерКТ)

'        dt1ТипИзделия = ds.Tables(ТипИзделия)
'        dt2НомерИзделия = ds.Tables(НомерИзделия)
'        dt3НомерСборки = ds.Tables(НомерСборки)
'        dt4НомерПостановки = ds.Tables(НомерПостановки)
'        dt5НомерЗапуска = ds.Tables(НомерЗапуска)
'        dt6НомерКонтрольнойТочки = ds.Tables(НомерКТ)
'        'создаем отношения между таблицами
'        Dim colIDРодитель As DataColumn = dt1ТипИзделия.Columns(ConstKeyTypeEngine)
'        Dim colIDДочерний As DataColumn = dt2НомерИзделия.Columns(ConstKeyTypeEngine)
'        'colIDРодитель.ReadOnly = True
'        'colIDРодитель.AutoIncrement = True
'        'colIDРодитель.Unique = True
'        'Dim PrimaryKeyColumns(0) As DataColumn
'        'PrimaryKeyColumns(0) = dt1ТипИзделия.Columns(ConstKeyTypeEngine)
'        'dt1ТипИзделия.PrimaryKey = PrimaryKeyColumns
'        ds.Relations.Add("1ТипИзделия_2НомерИзделия", colIDРодитель, colIDДочерний, True)

'        colIDРодитель = dt2НомерИзделия.Columns(ConstKeyNumberEngine)
'        colIDДочерний = dt3НомерСборки.Columns(ConstKeyNumberEngine)
'        ds.Relations.Add("2НомерИзделия_3НомерСборки", colIDРодитель, colIDДочерний, True)

'        colIDРодитель = dt3НомерСборки.Columns(ConstKeyNumberBuild)
'        colIDДочерний = dt4НомерПостановки.Columns(ConstKeyNumberBuild)
'        ds.Relations.Add("3НомерСборки_4НомерПостановки", colIDРодитель, colIDДочерний, True)

'        colIDРодитель = dt4НомерПостановки.Columns(ConstKeyNumberStage)
'        colIDДочерний = dt5НомерЗапуска.Columns(ConstKeyNumberStage)
'        ds.Relations.Add("4НомерПостановки_5НомерЗапуска", colIDРодитель, colIDДочерний, True)

'        colIDРодитель = dt5НомерЗапуска.Columns(ConstKeyNumberStarting)
'        colIDДочерний = dt6НомерКонтрольнойТочки.Columns(ConstKeyNumberStarting)
'        ds.Relations.Add("5НомерЗапуска_6НомерКТ", colIDРодитель, colIDДочерний, True)
'        'удалить все строки из drDataRowТипИзделия если не встречаются ни одной записи КТ
'        Dim drRow As DataRow
'        Dim drDataRowОбщая As DataRow
'        Dim blnУдалить As Boolean
'        Dim dtОбщая As DataTable
'        Dim odaОбщая As OleDbDataAdapter
'        Dim dsОбщая As New DataSet

'        odaОбщая = New OleDbDataAdapter(strSQLОбщая, cn)
'        odaОбщая.Fill(dsОбщая, "Общая")
'        dtОбщая = dsОбщая.Tables("Общая")

'        Dim dv4НомерПостановки As DataView = Nothing
'        Dim dv5НомерЗапуска As DataView = Nothing
'        Dim dv6НомерКонтрольнойТочки As DataView = Nothing


'        'пример
'        'Dim view As DataView = New DataView
'        'With view
'        '    .Table = DataSet1.Tables("Suppliers")
'        '    .AllowDelete = True
'        '    .AllowEdit = True
'        '    .AllowNew = True
'        '    .RowFilter = "City = 'Berlin'"
'        '    .RowStateFilter = DataViewRowState.ModifiedCurrent
'        '    .Sort = "CompanyName DESC"
'        'End With



'        If _ЗапросНомерПостановки <> "" Then
'            dt4НомерПостановки.DefaultView.RowFilter = _ЗапросНомерПостановки
'            dv4НомерПостановки = dt4НомерПостановки.DefaultView
'            'DataGrid4НомерПостановки.DataSource = dt4НомерПостановки.DefaultView
'        End If
'        If _ЗапросНомерЗапуска <> "" Then
'            dt5НомерЗапуска.DefaultView.RowFilter = _ЗапросНомерЗапуска
'            dv5НомерЗапуска = dt5НомерЗапуска.DefaultView
'            'DataGrid5НомерЗапуска.DataSource = dt5НомерЗапуска.DefaultView
'        End If
'        If _ЗапросНомерКонтрТочки <> "" Then
'            dt6НомерКонтрольнойТочки.DefaultView.RowFilter = _ЗапросНомерКонтрТочки
'            dv6НомерКонтрольнойТочки = dt6НомерКонтрольнойТочки.DefaultView
'            'DataGrid6НомерКонтрольнойТочки.DataSource = dt6НомерКонтрольнойТочки.DefaultView
'        End If

'        For Each drRow In dt6НомерКонтрольнойТочки.Rows
'            'blnУдалить = True
'            'For Each drDataRowОбщая In dtОбщая.Rows
'            '    If drDataRowОбщая(ConstKeyNumberStarting) = drRow(ConstKeyNumberStarting) Then
'            '        blnУдалить = False
'            '        Exit For
'            '    End If
'            'Next
'            'If blnУдалить Then
'            '    drRow.Delete()
'            'End If
'            If _ЗапросНомерКонтрТочки <> "" Then
'                'If blnУдалить = False Then
'                blnУдалить = True
'                Dim drRow2 As DataRowView
'                For intCounter As Integer = 0 To dv6НомерКонтрольнойТочки.Count - 1
'                    drRow2 = dv6НомерКонтрольнойТочки(intCounter)
'                    'сравниваем поля ключей записи (они во всех запросах на 2 позиции)
'                    If drRow.ItemArray(1) = drRow2.Row.ItemArray(1) Then
'                        'If drRow.Equals(drRow2) Then
'                        'If drRow Is drRow2 Then
'                        'drRow.Delete()
'                        blnУдалить = False
'                        Exit For
'                    End If
'                Next intCounter
'                If blnУдалить Then
'                    drRow.Delete()
'                End If
'                'End If
'            End If
'        Next

'        For Each drRow In dt5НомерЗапуска.Rows
'            blnУдалить = True
'            For Each drDataRowОбщая In dtОбщая.Rows
'                If drDataRowОбщая(ConstKeyNumberStarting) = drRow(ConstKeyNumberStarting) Then
'                    blnУдалить = False
'                    Exit For
'                End If
'            Next
'            If blnУдалить Then
'                drRow.Delete()
'            End If
'            If _ЗапросНомерЗапуска <> "" Then
'                If blnУдалить = False Then
'                    blnУдалить = True
'                    Dim drRow2 As DataRowView
'                    For intCounter As Integer = 0 To dv5НомерЗапуска.Count - 1
'                        drRow2 = dv5НомерЗапуска(intCounter)
'                        'сравниваем поля ключей записи (они во всех запросах на 2 позиции)
'                        If drRow.ItemArray(1) = drRow2.Row.ItemArray(1) Then
'                            'If drRow.Equals(drRow2) Then
'                            'If drRow Is drRow2 Then
'                            'drRow.Delete()
'                            blnУдалить = False
'                            Exit For
'                        End If
'                    Next intCounter
'                    If blnУдалить Then
'                        drRow.Delete()
'                    End If
'                End If
'            End If
'        Next

'        For Each drRow In dt4НомерПостановки.Rows
'            blnУдалить = True
'            For Each drDataRowОбщая In dtОбщая.Rows
'                If drDataRowОбщая(ConstKeyNumberStage) = drRow(ConstKeyNumberStage) Then
'                    blnУдалить = False
'                    Exit For
'                End If
'            Next
'            If blnУдалить Then
'                drRow.Delete()
'            End If
'            If _ЗапросНомерПостановки <> "" Then
'                If blnУдалить = False Then
'                    blnУдалить = True
'                    Dim drRow2 As DataRowView
'                    For intCounter As Integer = 0 To dv4НомерПостановки.Count - 1
'                        drRow2 = dv4НомерПостановки(intCounter)
'                        'сравниваем поля ключей записи (они во всех запросах на 2 позиции)
'                        If drRow.ItemArray(1) = drRow2.Row.ItemArray(1) Then
'                            'If drRow.Equals(drRow2) Then
'                            'If drRow Is drRow2 Then
'                            'drRow.Delete()
'                            blnУдалить = False
'                            Exit For
'                        End If
'                    Next intCounter
'                    If blnУдалить Then
'                        drRow.Delete()
'                    End If
'                End If
'            End If
'        Next

'        dt6НомерКонтрольнойТочки.AcceptChanges()
'        dt5НомерЗапуска.AcceptChanges()
'        dt4НомерПостановки.AcceptChanges()

'        For Each drRow In dt3НомерСборки.Rows
'            blnУдалить = True
'            For Each drDataRowОбщая In dtОбщая.Rows
'                If drDataRowОбщая(ConstKeyNumberBuild) = drRow(ConstKeyNumberBuild) Then
'                    blnУдалить = False
'                    Exit For
'                End If
'            Next
'            If blnУдалить Then
'                drRow.Delete()
'            End If
'        Next

'        For Each drRow In dt2НомерИзделия.Rows
'            blnУдалить = True
'            For Each drDataRowОбщая In dtОбщая.Rows
'                If drDataRowОбщая(ConstKeyNumberEngine) = drRow(ConstKeyNumberEngine) Then
'                    blnУдалить = False
'                    Exit For
'                End If
'            Next
'            If blnУдалить Then
'                drRow.Delete()
'            End If
'        Next

'        For Each drRow In dt1ТипИзделия.Rows
'            blnУдалить = True
'            For Each drDataRowОбщая In dtОбщая.Rows
'                If drDataRowОбщая(ConstKeyTypeEngine) = drRow(ConstKeyTypeEngine) Then
'                    blnУдалить = False
'                    Exit For
'                End If
'            Next
'            If blnУдалить Then
'                drRow.Delete()
'            End If
'        Next

'        dt5НомерЗапуска.AcceptChanges()
'        dt4НомерПостановки.AcceptChanges()
'        dt3НомерСборки.AcceptChanges()
'        dt2НомерИзделия.AcceptChanges()
'        dt1ТипИзделия.AcceptChanges()
'        ds.AcceptChanges()

'        'устанвить источник данных и связи для иерархических обновлений
'        DataGrid1ТипИзделия.SetDataBinding(dt1ТипИзделия, "")
'        DataGrid2НомерИзделия.SetDataBinding(dt1ТипИзделия, "1ТипИзделия_2НомерИзделия")
'        DataGrid3НомерСборки.SetDataBinding(dt1ТипИзделия, "1ТипИзделия_2НомерИзделия.2НомерИзделия_3НомерСборки")
'        DataGrid4НомерПостановки.SetDataBinding(dt1ТипИзделия, "1ТипИзделия_2НомерИзделия.2НомерИзделия_3НомерСборки.3НомерСборки_4НомерПостановки")
'        DataGrid5НомерЗапуска.SetDataBinding(dt1ТипИзделия, "1ТипИзделия_2НомерИзделия.2НомерИзделия_3НомерСборки.3НомерСборки_4НомерПостановки.4НомерПостановки_5НомерЗапуска")
'        DataGrid6НомерКонтрольнойТочки.SetDataBinding(dt1ТипИзделия, "1ТипИзделия_2НомерИзделия.2НомерИзделия_3НомерСборки.3НомерСборки_4НомерПостановки.4НомерПостановки_5НомерЗапуска.5НомерЗапуска_6НомерКТ")

'        'чтобы отобразить таблицы КТ если они есть  по цепочке
'        DataGrid1ТипИзделия_CurrentRowChanged()

'    Catch ex As Exception
'        Windows.Forms.Cursor.Current = currentCursor
'        MessageBox.Show(ex.ToString, "Отображение запроса", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'    Finally
'        Windows.Forms.Cursor.Current = currentCursor
'        cn.Close()
'    End Try
'End Sub
