Imports System.Data.OleDb
Imports System.Windows.Forms

Public Class FormFindChannel
    Private cn As OleDbConnection
    Private dt As DataTable
    Private DataSetChannels As New DataSet
    Private WithEvents ChannelsCurrencyManager As CurrencyManager
    Private DataAdapterChannels As OleDbDataAdapter
    Private rowPosition As Integer
    Private Const conTableNameValuesAllChannels As String = "ЗначенияВсехКаналов"
    Private KeyNumberKT As Integer
    Private Manager As ProjectManager

    Public Sub New(ByVal KeyNumberKT As Integer, ByVal Manager As ProjectManager)
        MyBase.New()
        Me.KeyNumberKT = KeyNumberKT
        Me.Manager = Manager
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub FormFindChannel_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        LoadTableValuesAllChannels()
        FormatDetailsGrid()

        If DataSetChannels.Tables(conTableNameValuesAllChannels).Rows.Count > 0 Then
            rowPosition = 0
            ShowCurrentRowTable()
        End If
    End Sub

    ''' <summary>
    ''' Загрузить Таблицу ЗначенияВсехКаналов и отобразить значения
    ''' </summary>
    Private Sub LoadTableValuesAllChannels()
        Dim strSQL As String = Nothing
        Try
            cn = New OleDbConnection(BuildCnnStr(PROVIDER_JET, Me.Manager.PathKT))
            strSQL = $"SELECT ЗначенияВсехКаналов.ИмяКанала, ЗначенияВсехКаналов.Значение FROM {conTableNameValuesAllChannels} WHERE (((ЗначенияВсехКаналов.keyНомерКТ)= {KeyNumberKT.ToString})) ORDER BY ЗначенияВсехКаналов.keyИмяКанала;"
            'cn.Open() - открывается автоматически и также автоматически закрывается если это делаю не я
            DataAdapterChannels = New OleDbDataAdapter(strSQL, cn)
            DataSetChannels = New DataSet
            DataAdapterChannels.Fill(DataSetChannels, conTableNameValuesAllChannels)
            dt = DataSetChannels.Tables(conTableNameValuesAllChannels)

            AddHandler Me.BindingContext(dt).PositionChanged, AddressOf dv_PositionChanged

            grdFindChannel.DataSource = dt
            ChannelsCurrencyManager = CType(grdFindChannel.BindingContext(dt), CurrencyManager)
        Catch ex As Exception
            MessageBox.Show(ex.ToString, $"Загрузить таблицу <{NameOf(conTableNameValuesAllChannels)}>", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            If (cn.State = ConnectionState.Open) Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub OK_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    'Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    '    Me.Close()
    'End Sub

    Private Sub Filter()
        With dt
            .DefaultView.RowFilter = $"ИмяКанала like '*{TextBoxFilter.Text}%'"

            If .DefaultView.Count = 0 Then
                MessageBox.Show("Не найдено ни одной записи.", "Фильтрация", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            ' При связывании grid к DataView, grid будет показывать только согласованные строки
            'grdFindChannel.DataSource = .DefaultView
        End With
    End Sub

    Private Sub TextBoxFilter_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxFilter.TextChanged
        Filter()
    End Sub

    Private Sub FormatDetailsGrid()
        'Dim strSQL As String = "SELECT ЗначенияВсехКаналов.ИмяКанала, ЗначенияВсехКаналов.Значение FROM " & conЗначенияВсехКаналов
        Try
            cn.Open()
            'добавить стиль
            'If grdFindChannel.TableStyles.Count = 0 Then grdFindChannel.TableStyles.Add(ДобавитьСтиль(cn, conЗначенияВсехКаналов, grdFindChannel, strSQL))
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Отображение запроса", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            cn.Close()
        End Try
    End Sub

    Protected Sub dv_PositionChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Me.IsHandleCreated Then
            Try
                rowPosition = Me.BindingContext(dt).Position
                ShowCurrentRowTable()
            Catch ex As Exception
                MessageBox.Show(ex.ToString, "Изменение активной строки таблицы", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End If
    End Sub

    Private Sub ShowCurrentRowTable()
        If rowPosition <> -1 Then
            lblTarRowPosition.Text = "Запись " & rowPosition + 1 & " из " & dt.Rows.Count
            'If intTabPosition <> 0 Then grdFindChannel.Select(grdFindChannel.CurrentRowIndex)
            'If intTabPosition <> 0 Then grdFindChannel. (grdFindChannel.CurrentRow.Index)
        Else
            lblTarRowPosition.Text = "Нет записей"
        End If
    End Sub
End Class
