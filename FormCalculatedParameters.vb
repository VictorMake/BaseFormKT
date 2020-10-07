Imports System.Data.OleDb
Imports System.Windows.Forms

Friend Class FormCalculatedParameters

    Private mFormParrent As frmBaseKT
    Public WriteOnly Property FormParrent() As frmBaseKT
        Set(ByVal Value As frmBaseKT)
            mFormParrent = Value
        End Set
    End Property

    Sub New(ByVal FormParrent As frmBaseKT)
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Me.MdiParent = FormParrent
        Me.FormParrent = FormParrent
    End Sub

    ''' <summary>
    ''' Загрузка и конфигурация адаптеров формы.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ConfigureTableAdapter()
        ' строка подключения сделал сам т.к. в дизайнере ссылка на строку созданную при создании дизайнера
        Me.CalculatedParametersTableAdapter.Connection = New OleDbConnection(BuildCnnStr(PROVIDER_JET, mFormParrent.Manager.PathSettingMdb))

        ' This line of code loads data into the 'BaseFormDataSet.РасчетныеПараметры' table. You can move, or remove it, as needed.
        Me.CalculatedParametersTableAdapter.Fill(Me.BaseFormDataSet.РасчетныеПараметры)
    End Sub

    Private Sub FormCalculatedParameters_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        If Not mFormParrent.IsWindowClosed Then e.Cancel = True
    End Sub

    Private Sub FormCalculatedParameters_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        mFormParrent = Nothing
    End Sub

    Private Sub TSButtonHelp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonHelp.Click
        MessageBox.Show($"Если в формуле расчёта используются единицы СИ, то для каждого расчётного параметра установить <Выходную единицу измерения>,{vbCrLf}в противном случае установить <нет>.",
                        "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub SaveToolStripButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SaveToolStripButton.Click
        mFormParrent.Manager.SaveTable()
    End Sub

    Private Sub DataGridViewCalculated_CellValueChanged(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridViewCalculated.CellValueChanged
        If Me.IsHandleCreated Then
            mFormParrent.Manager.NeedToRewrite = True
        End If
    End Sub
End Class


'Private blnНадоПерезаписать As Boolean
'Private arrЕдИзмерения() As String

''Private Sub frmСоответствие_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Me.Load
''    FillCombo()
''End Sub

'Public Sub FillCombo()
'    Dim I As Integer
'    Dim strSQL As String
'    Dim cn As New OleDbConnection(BuildCnnStr(strProviderJet, strПутьКТ))

'    cn.Open()
'    blnНадоПерезаписать = funПроверкаСоответствия(cn) 'здесь также заполнение массива

'    Dim arrИменаПараметров() As String
'    'занести в список. эдесь работаем с копией
'    ReDim_arrИменаПараметров(UBound(arrСписПарамКопия))
'    arrИменаПараметров(0) = conПараметрОтсутствует
'    For I = 1 To UBound(arrСписПарамКопия)
'        arrИменаПараметров(I) = arrПараметры(arrСписПарамКопия(I)).strНаименованиеПараметра
'    Next
'    Dim TempComboBoxColumnИмяПараметраИзмерения As DataGridViewComboBoxColumn = CType(DGVСоответсвие.Columns("ColumnИмяПараметраИзмерения"), DataGridViewComboBoxColumn)
'    TempComboBoxColumnИмяПараметраИзмерения.Items.AddRange(arrИменаПараметров)

'    ReDim_arrИменаПараметров(UBound(arrСоответствие))
'    arrИменаПараметров(0) = ""
'    For I = 1 To UBound(arrСоответствие)
'        arrИменаПараметров(I) = arrСоответствие(I).strИмяРасчета
'    Next
'    Dim TempComboBoxColumnИмяБазовогоПараметра As DataGridViewComboBoxColumn = CType(DGVСоответсвие.Columns("ColumnИмяБазовогоПараметра"), DataGridViewComboBoxColumn)
'    TempComboBoxColumnИмяБазовогоПараметра.Items.AddRange(arrИменаПараметров)

'    Dim cmd As OleDbCommand = cn.CreateCommand
'    strSQL = "SELECT COUNT(*) FROM ЕдиницаИзмерения"
'    cmd.CommandType = CommandType.Text
'    cmd.CommandText = strSQL
'    If (cn.State = ConnectionState.Closed) Then
'        cn.Open()
'    End If

'    ReDim_arrЕдИзмерения(CInt(cmd.ExecuteScalar))

'    strSQL = "SELECT * from [ЕдиницаИзмерения]"
'    cmd.CommandType = CommandType.Text
'    cmd.CommandText = strSQL
'    Dim rdr As OleDbDataReader = cmd.ExecuteReader
'    I = 0
'    arrЕдИзмерения(I) = ""
'    I = I + 1
'    Do While rdr.Read
'        arrЕдИзмерения(I) = rdr("ЕдиницаИзмерения")
'        I = I + 1
'    Loop
'    rdr.Close()
'    cn.Close()
'    Dim TempComboBoxColumnЕдИзмВходная As DataGridViewComboBoxColumn = CType(DGVСоответсвие.Columns("ColumnЕдИзмВходная"), DataGridViewComboBoxColumn)
'    TempComboBoxColumnЕдИзмВходная.Items.AddRange(arrЕдИзмерения)
'    Dim TempComboBoxColumnЕдИзмВыходная As DataGridViewComboBoxColumn = CType(DGVСоответсвие.Columns("ColumnЕдИзмВыходная"), DataGridViewComboBoxColumn)
'    TempComboBoxColumnЕдИзмВыходная.Items.AddRange(arrЕдИзмерения)

'    Dim TempComboBoxColumnТипДавления As DataGridViewComboBoxColumn = CType(DGVСоответсвие.Columns("ColumnТипДавления"), DataGridViewComboBoxColumn)
'    TempComboBoxColumnТипДавления.Items.AddRange(New String() {"", "Разрежение", "Давление"})

'    'занести на сетку
'    For I = 1 To UBound(arrСоответствие)
'        'DGVСоответсвие.Rows.Add()
'        Dim heter_row As DataGridViewRow = New DataGridViewRow
'        ' создаем строку, считывая описания колонок с _grid
'        heter_row.CreateCells(DGVСоответсвие)

'        heter_row.Cells(0).Value = CType(arrСоответствие(I).strИмяРасчета, Object)
'        heter_row.Cells(1).Value = CType(CStr(arrСоответствие(I).NРасчета), Object)
'        heter_row.Cells(2).Value = TempComboBoxColumnИмяПараметраИзмерения.Items(TempComboBoxColumnИмяПараметраИзмерения.Items.IndexOf(arrСоответствие(I).strИмяБазы))
'        heter_row.Cells(3).Value = CType(CStr(arrСоответствие(I).NБазы), Object)
'        heter_row.Cells(4).Value = CType(arrСоответствие(I).strОписание, Object)
'        heter_row.Cells(5).Value = TempComboBoxColumnИмяБазовогоПараметра.Items(TempComboBoxColumnИмяБазовогоПараметра.Items.IndexOf(arrСоответствие(I).strИмяБазовогоПараметра))
'        heter_row.Cells(6).Value = TempComboBoxColumnЕдИзмВходная.Items(TempComboBoxColumnЕдИзмВходная.Items.IndexOf(arrСоответствие(I).strРазмерностьВходная))
'        heter_row.Cells(7).Value = TempComboBoxColumnЕдИзмВыходная.Items(TempComboBoxColumnЕдИзмВыходная.Items.IndexOf(arrСоответствие(I).strРазмерностьВыходная))
'        heter_row.Cells(8).Value = TempComboBoxColumnТипДавления.Items(TempComboBoxColumnТипДавления.Items.IndexOf(arrСоответствие(I).strТипДавления))

'        DGVСоответсвие.Rows.Add(heter_row)
'    Next I

'    If blnНадоПерезаписать Then mnuЗаписатьСоответствие_Click(mnuЗаписатьСоответствие, New System.EventArgs)
'End Sub

''Private Sub frmСоответствие_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
''    ПроверкаНаИзменение()
''End Sub

'Public Sub mnuЗаписатьСоответствие_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuЗаписатьСоответствие.Click
'    'сменить фокус чтобы прошли изменения
'    DGVСоответсвие.Rows(0).Cells(0).Selected = True

'    Dim cn As New OleDbConnection(BuildCnnStr(strProviderJet, strПутьКТ))
'    Dim ds As New DataSet
'    Dim drDataRow As DataRow
'    Dim drDataRowParent As DataRow
'    Dim aFindValue(0) As Object
'    Dim dcDataColumn(1) As DataColumn

'    'Dim strSQL As String
'    Dim I As Integer
'    'запись изменений в структуре и в базе
'    For I = 1 To UBound(arrСоответствие)
'        arrСоответствие(I).strИмяРасчета = DGVСоответсвие.Rows(I - 1).Cells("ColumnИмяПараметраРасчета").Value 'F1View.TextSRC(1, I + conRowOffset, 2)
'        arrСоответствие(I).NРасчета = Cint(DGVСоответсвие.Rows(I - 1).Cells("ColumnНомерПараметраВРасчете").Value) 'Cint(F1View.TextSRC(1, I + conRowOffset, 3))
'        arrСоответствие(I).strИмяБазы = DGVСоответсвие.Rows(I - 1).Cells("ColumnИмяПараметраИзмерения").Value 'F1View.TextSRC(1, I + conRowOffset, 4)
'        arrСоответствие(I).NБазы = Cint(DGVСоответсвие.Rows(I - 1).Cells("ColumnНомерПараметраИзмерения").Value) 'Cint(F1View.TextSRC(1, I + conRowOffset, 5))
'        arrСоответствие(I).strОписание = DGVСоответсвие.Rows(I - 1).Cells("ColumnОписаниеПараметра").Value 'F1View.TextSRC(1, I + conRowOffset, 6)
'        arrСоответствие(I).strИмяБазовогоПараметра = DGVСоответсвие.Rows(I - 1).Cells("ColumnИмяБазовогоПараметра").Value ' F1View.TextSRC(1, I + conRowOffset, 7)
'        arrСоответствие(I).strРазмерностьВходная = DGVСоответсвие.Rows(I - 1).Cells("ColumnЕдИзмВходная").Value 'F1View.TextSRC(1, I + conRowOffset, 8)
'        arrСоответствие(I).strРазмерностьВыходная = DGVСоответсвие.Rows(I - 1).Cells("ColumnЕдИзмВыходная").Value 'F1View.TextSRC(1, I + conRowOffset, 9)
'        arrСоответствие(I).strТипДавления = DGVСоответсвие.Rows(I - 1).Cells("ColumnТипДавления").Value ' F1View.TextSRC(1, I + conRowOffset, 10)
'    Next I

'    'Запись и получение keyNEWНомерКонтрТочки=keyНомерКонтрТочки
'    'strSQL = "SELECT СоответствиеПараметров.*, СвойстваПараметров.ОписаниеПараметра, СвойстваПараметров.РазмерностьВходная, СвойстваПараметров.РазмерностьВыходная, СвойстваПараметров.ДляЧегоПараметр " & "FROM СвойстваПараметров RIGHT JOIN СоответствиеПараметров ON СвойстваПараметров.keyИмяПараметра = СоответствиеПараметров.keyИмяПараметра " & "WHERE (((СвойстваПараметров.ДляЧегоПараметр)='Измерение')) ORDER BY СоответствиеПараметров.НомерПараметраРасчета;"

'    cn.Open()
'    Dim odaСвойстваПараметров As New OleDbDataAdapter("SELECT * FROM СвойстваПараметров WHERE ((СвойстваПараметров.ДляЧегоПараметр)='Измерение')", cn)
'    Dim odaСоответствиеПараметров As New OleDbDataAdapter("SELECT * FROM СоответствиеПараметров", cn)
'    odaСвойстваПараметров.Fill(ds, "СвойстваПараметров")
'    odaСоответствиеПараметров.Fill(ds, "СоответствиеПараметров")
'    Dim tlbСвойстваПараметров As DataTable = ds.Tables("СвойстваПараметров")
'    Dim tlbСоответствиеПараметров As DataTable = ds.Tables("СоответствиеПараметров")
'    Dim colIdСвойстваПараметров As DataColumn = tlbСвойстваПараметров.Columns("keyИмяПараметра")
'    Dim colIdСоответствиеПараметров As DataColumn = tlbСоответствиеПараметров.Columns("keyИмяПараметра")
'    ds.Relations.Add("rel", colIdСвойстваПараметров, colIdСоответствиеПараметров, True)

'    dcDataColumn(0) = tlbСоответствиеПараметров.Columns("ИмяПараметраРасчета")
'    tlbСоответствиеПараметров.PrimaryKey = dcDataColumn

'    Try
'        For I = 1 To UBound(arrСоответствие)
'            aFindValue(0) = arrСоответствие(I).strИмяРасчета
'            drDataRow = tlbСоответствиеПараметров.Rows.Find(aFindValue)
'            If Not drDataRow Is Nothing Then
'                drDataRow.BeginEdit()
'                drDataRow("ИмяПараметраИзмерения") = arrСоответствие(I).strИмяБазы
'                drDataRow("НомерПараметраИзмерения") = arrСоответствие(I).NБазы
'                If arrСоответствие(I).strИмяБазовогоПараметра = " " Then
'                    drDataRow("ИмяБазовогоПараметра") = vbNullString
'                Else
'                    drDataRow("ИмяБазовогоПараметра") = arrСоответствие(I).strИмяБазовогоПараметра
'                End If
'                If arrСоответствие(I).strТипДавления = "" Or arrСоответствие(I).strТипДавления = vbNullString Then
'                    drDataRow("ТипДавления") = vbNullString
'                Else
'                    drDataRow("ТипДавления") = arrСоответствие(I).strТипДавления
'                End If
'                drDataRow.EndEdit()
'                drDataRowParent = drDataRow.GetParentRow("rel")
'                If Not drDataRowParent Is Nothing Then
'                    drDataRow.BeginEdit()
'                    drDataRowParent("РазмерностьВходная") = arrСоответствие(I).strРазмерностьВходная
'                    drDataRowParent("РазмерностьВыходная") = arrСоответствие(I).strРазмерностьВыходная
'                    drDataRow.EndEdit()
'                End If
'            End If
'        Next I
'        Dim CommandBuilderСвойстваПараметров As OleDbCommandBuilder = New OleDbCommandBuilder(odaСвойстваПараметров)
'        'odaСвойстваПараметров.UpdateCommand = CommandBuilderСвойстваПараметров.GetUpdateCommand
'        odaСвойстваПараметров.Update(tlbСвойстваПараметров)
'        'ds.Tables(0).AcceptChanges()
'        'odaСвойстваПараметров.UpdateCommand.Connection.Close()

'        Dim CommandBuilderСоответствиеПараметров As OleDbCommandBuilder = New OleDbCommandBuilder(odaСоответствиеПараметров)
'        odaСоответствиеПараметров.Update(tlbСоответствиеПараметров)
'    Catch ex As Exception
'        MessageBox.Show(ex.ToString, "Ошибка обновления таблицы СоответствиеПараметров", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'    Finally
'        If (cn.State = ConnectionState.Open) Then
'            cn.Close()
'        End If
'    End Try

'    System.Threading.Thread.Sleep(300)
'    System.Windows.Forms.Application.DoEvents()
'    If Not mФормаРодителя Is Nothing Then mФормаРодителя.НовыйСнимокИмитатора()
'End Sub

''Public Sub mnuПечать_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuПечать.Click
''    Try
''        'F1Book1.FilePrint(True)
''    Catch ex As Exception
''        MessageBox.Show("Ошибка печати " + ex.ToString, "Печать", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''    End Try
''End Sub

'Private Sub ПроверкаНаИзменение()
'    Dim I As Integer
'    blnНадоПерезаписать = False
'    For I = 1 To UBound(arrСоответствие)
'        If arrСоответствие(I).strИмяБазы <> DGVСоответсвие.Rows(I - 1).Cells("ColumnИмяПараметраИзмерения").Value Then
'            blnНадоПерезаписать = True
'            Exit For
'        End If
'        If arrСоответствие(I).strИмяБазовогоПараметра <> DGVСоответсвие.Rows(I - 1).Cells("ColumnИмяБазовогоПараметра").Value Then
'            blnНадоПерезаписать = True
'            Exit For
'        End If
'        If arrСоответствие(I).strРазмерностьВходная <> DGVСоответсвие.Rows(I - 1).Cells("ColumnЕдИзмВходная").Value Then
'            blnНадоПерезаписать = True
'            Exit For
'        End If
'        If arrСоответствие(I).strРазмерностьВыходная <> DGVСоответсвие.Rows(I - 1).Cells("ColumnЕдИзмВыходная").Value Then
'            blnНадоПерезаписать = True
'            Exit For
'        End If
'        If arrСоответствие(I).strТипДавления <> DGVСоответсвие.Rows(I - 1).Cells("ColumnТипДавления").Value Then
'            blnНадоПерезаписать = True
'            Exit For
'        End If
'    Next I
'    If blnНадоПерезаписать Then
'        ' пользователь нажал да.
'        If MessageBox.Show("Были произведены изменения в таблице свойств." & vbCrLf & "Записать измерения ?", "Соответствие", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
'            mnuЗаписатьСоответствие_Click(mnuЗаписатьСоответствие, New System.EventArgs)
'        End If
'    End If
'End Sub

''Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
''    Me.Close()
''End Sub

''Private Sub frmСоответствие_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
''    frmSheetName.Height = Me.ClientRectangle.Height
''    frmSheetName.Width = Me.ClientRectangle.Width
''    F1Book1.SetBounds(frmSheetName.Left + 5, frmSheetName.Top + 5, frmSheetName.Width - 50, frmSheetName.Height - 100)
''End Sub



'Private Sub ToolStripButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonДобавитьУстройство.Click
'    ТаблицаЗаполнена = False
'    DGVСоответсвие.Rows.Clear()
'    DGVСоответсвие.AutoGenerateColumns = False



'    'Dim comboCellЗначениеЛогики As DataGridViewComboBoxCell = New DataGridViewComboBoxCell
'    'comboCellЗначениеЛогики.Items.AddRange(New String() {"0", "1"})
'    ''заполнить из базы каналов или из параметров принятых по сети
'    'Dim comboCellNameOfParametr As DataGridViewComboBoxCell = New DataGridViewComboBoxCell
'    'comboCellNameOfParametr.Items.AddRange(New String() {"N1", "N2", "N3", "N4"})

'    'Dim НижнееЗначениеВключения, ВерхнееЗначениеВыключения As Double

'    'CType(DGVСоответсвие, ISupportInitialize).BeginInit()


'    For I As Integer = 0 To 7
'        If I Mod 2 = 0 Then
'            СтрокаТаблицыПоЛогикеTrue(I)
'        Else
'            СтрокаТаблицыПоЛогикеFalse(I)
'        End If


'        'НижнееЗначениеВключения = I * 10
'        'ВерхнееЗначениеВыключения = I * 100

'        'Dim comboCellЗначениеЛогики As DataGridViewComboBoxCell = New DataGridViewComboBoxCell
'        'comboCellЗначениеЛогики.Items.AddRange(New String() {"0", "1"})
'        ''заполнить из базы каналов или из параметров принятых по сети
'        'Dim comboCellNameOfParametr As DataGridViewComboBoxCell = New DataGridViewComboBoxCell
'        'comboCellNameOfParametr.Items.AddRange(New String() {"N1", "N2", "N3", "N4"})
'        'Dim comboCellЗначениеПараметраПоСети As DataGridViewComboBoxCell = New DataGridViewComboBoxCell
'        'comboCellЗначениеПараметраПоСети.Items.AddRange(New String() {"Измеряется", "По сети"})

'        'Dim newRow As DataGridViewRow = New DataGridViewRow()
'        ''0
'        'Dim cell0 As DataGridViewTextBoxCell = New DataGridViewTextBoxCell
'        'cell0.Value = I.ToString
'        'newRow.Cells.Add(cell0)
'        ''1
'        'Dim cell1 As DataGridViewCheckBoxCell = New DataGridViewCheckBoxCell
'        'cell1.Value = True 'заполнить нужным значением
'        'newRow.Cells.Add(cell1)
'        ''2
'        'newRow.Cells.Add(comboCellЗначениеЛогики)
'        'comboCellЗначениеЛогики.Value = comboCellЗначениеЛогики.Items(0) 'заполнить нужным значением
'        ''3
'        'newRow.Cells.Add(comboCellNameOfParametr)
'        'comboCellNameOfParametr.Value = comboCellNameOfParametr.Items(0) 'заполнить нужным значением
'        ''comboCellNameOfParametr.Visible = False
'        ''newRow.Cells(3).en()
'        ''4
'        'newRow.Cells.Add(comboCellЗначениеПараметраПоСети)
'        'comboCellЗначениеПараметраПоСети.Value = comboCellЗначениеПараметраПоСети.Items(0) 'заполнить нужным значением
'        ''5
'        'Dim cell5 As DataGridViewTextBoxCell = New DataGridViewTextBoxCell
'        'cell5.Value = Format(НижнееЗначениеВключения, "Fixed")
'        'newRow.Cells.Add(cell5)
'        ''6
'        'Dim cell6 As DataGridViewTextBoxCell = New DataGridViewTextBoxCell
'        'cell6.Value = Format(ВерхнееЗначениеВыключения, "Fixed")
'        'newRow.Cells.Add(cell6)
'        ''в конце добавить строку
'        'DGVСоответсвие.Rows.Add(newRow)
'    Next
'    'CType(DGVСоответсвие, ISupportInitialize).EndInit()


'    'DGVСоответсвие.Columns(3).Visible = False

'    'AddHandler DGVСоответсвие.Validating, AddressOf Me.MenuOption_Click


'    'grid.CellValuePushed += _dataGridView_CellValuePushed;

'    '' Создаем ячейку типа CheckBox
'    'Dim checkCell As DataGridViewCheckBoxCell = New DataGridViewCheckBoxCell()
'    'checkCell.Value = True
'    '' Добавляем в качестве первой ячейки новой строки ячейку типа CheckBox
'    'newRow.Cells.Add(checkCell)
'    '' Остальные ячейки заполняем ячейками типа TextBox
'    'newRow.Cells.Add(New DataGridViewTextBoxCell())
'    'newRow.Cells.Add(New DataGridViewTextBoxCell())
'    '' эта строчка будет с переключателем в первой колонке
'    'DGVСоответсвие.Rows.Add(newRow)






'    'Dim row0 As DataGridViewRow = DGVСоответсвие.Rows(0)

'    'Dim cell0 As DataGridViewTextBoxCell = CType(row0.Cells(0), DataGridViewTextBoxCell)
'    'cell0.Value = "dotNET"

'    'Dim cell1 As DataGridViewLinkCell = CType(row0.Cells(1), DataGridViewLinkCell)
'    'cell1.Value = "RSDN.ru"

'    'Dim cell2 As DataGridViewButtonCell = CType(row0.Cells(2), DataGridViewButtonCell)
'    'cell2.Value = "Accept"

'    'Dim cell3 As DataGridViewCheckBoxCell = CType(row0.Cells(3), DataGridViewCheckBoxCell)
'    'cell3.Value = True

'    'Dim cell4 As DataGridViewComboBoxCell = CType(row0.Cells(4), DataGridViewComboBoxCell)
'    'cell4.Items.AddRange(New String() {"Trace", "Debug", "Release"})
'    'cell4.Value = "Release"

'    'Dim cell5 As DataGridViewImageCell = CType(row0.Cells(5), DataGridViewImageCell)
'    'cell5.ImageLayout = DataGridViewImageCellLayout.Zoom
'    'cell5.Value = Image.FromFile("C:\WINDOWS\Blue Lace 16.bmp")

'    ТаблицаЗаполнена = True
'    ВключитьDGVСоответсвие(False)

'End Sub

'Private Sub СтрокаТаблицыПоЛогикеTrue(ByVal I As Integer)
'    Dim НижнееЗначениеВключения, ВерхнееЗначениеВыключения As Double

'    НижнееЗначениеВключения = I * 10
'    ВерхнееЗначениеВыключения = I * 100

'    Dim comboCellЗначениеЛогики As DataGridViewComboBoxCell = New DataGridViewComboBoxCell
'    comboCellЗначениеЛогики.Items.AddRange(New String() {"0", "1"})
'    'заполнить из базы каналов или из параметров принятых по сети
'    'Dim comboCellNameOfParametr As DataGridViewComboBoxCell = New DataGridViewComboBoxCell
'    'comboCellNameOfParametr.Items.AddRange(New String() {"N1", "N2", "N3", "N4"})
'    'Dim comboCellЗначениеПараметраПоСети As DataGridViewComboBoxCell = New DataGridViewComboBoxCell
'    'comboCellЗначениеПараметраПоСети.Items.AddRange(New String() {"Измеряется", "По сети"})

'    Dim newRow As DataGridViewRow = New DataGridViewRow()
'    '0
'    Dim cell0 As DataGridViewTextBoxCell = New DataGridViewTextBoxCell
'    cell0.Value = I.ToString
'    newRow.Cells.Add(cell0)
'    '1
'    Dim cell1 As DataGridViewCheckBoxCell = New DataGridViewCheckBoxCell
'    cell1.Value = True 'заполнить нужным значением
'    newRow.Cells.Add(cell1)
'    '2
'    newRow.Cells.Add(comboCellЗначениеЛогики)
'    comboCellЗначениеЛогики.Value = comboCellЗначениеЛогики.Items(0) 'заполнить нужным значением
'    '3
'    Dim cell3 As DataGridViewTextBoxCell = New DataGridViewTextBoxCell
'    newRow.Cells.Add(cell3)
'    cell3.ReadOnly = True
'    '4
'    Dim cell4 As DataGridViewTextBoxCell = New DataGridViewTextBoxCell
'    newRow.Cells.Add(cell4)
'    cell4.ReadOnly = True
'    '5
'    Dim cell5 As DataGridViewTextBoxCell = New DataGridViewTextBoxCell
'    newRow.Cells.Add(cell5)
'    cell5.ReadOnly = True
'    '6
'    Dim cell6 As DataGridViewTextBoxCell = New DataGridViewTextBoxCell
'    newRow.Cells.Add(cell6)
'    cell6.ReadOnly = True
'    'в конце добавить строку
'    'DGVСоответсвие.Rows.Add(newRow)
'    DGVСоответсвие.Rows.Insert(I, newRow)
'    'DGVСоответсвие.AutoGenerateColumns=True
'End Sub

'Private Sub СтрокаТаблицыПоЛогикеFalse(ByVal I As Integer)
'    Dim НижнееЗначениеВключения, ВерхнееЗначениеВыключения As Double

'    НижнееЗначениеВключения = I * 10
'    ВерхнееЗначениеВыключения = I * 100

'    Dim comboCellЗначениеЛогики As DataGridViewComboBoxCell = New DataGridViewComboBoxCell
'    comboCellЗначениеЛогики.Items.AddRange(New String() {"0", "1"})
'    'заполнить из базы каналов или из параметров принятых по сети
'    Dim comboCellNameOfParametr As DataGridViewComboBoxCell = New DataGridViewComboBoxCell
'    comboCellNameOfParametr.Items.AddRange(New String() {"N1", "N2", "N3", "N4"})
'    Dim comboCellЗначениеПараметраПоСети As DataGridViewComboBoxCell = New DataGridViewComboBoxCell
'    comboCellЗначениеПараметраПоСети.Items.AddRange(New String() {"Измеряется", "По сети"})

'    Dim newRow As DataGridViewRow = New DataGridViewRow()
'    '0
'    Dim cell0 As DataGridViewTextBoxCell = New DataGridViewTextBoxCell
'    cell0.Value = I.ToString
'    newRow.Cells.Add(cell0)
'    '1
'    Dim cell1 As DataGridViewCheckBoxCell = New DataGridViewCheckBoxCell
'    cell1.Value = False 'заполнить нужным значением
'    newRow.Cells.Add(cell1)
'    '2
'    Dim cell2 As DataGridViewTextBoxCell = New DataGridViewTextBoxCell
'    newRow.Cells.Add(cell2)
'    cell2.ReadOnly = True
'    '3
'    newRow.Cells.Add(comboCellNameOfParametr)
'    comboCellNameOfParametr.Value = comboCellNameOfParametr.Items(0) 'заполнить нужным значением
'    'comboCellNameOfParametr.Visible = False
'    'newRow.Cells(3).en()
'    '4
'    newRow.Cells.Add(comboCellЗначениеПараметраПоСети)
'    comboCellЗначениеПараметраПоСети.Value = comboCellЗначениеПараметраПоСети.Items(0) 'заполнить нужным значением
'    '5
'    Dim cell5 As DataGridViewTextBoxCell = New DataGridViewTextBoxCell
'    cell5.Value = Format(НижнееЗначениеВключения, "Fixed")
'    newRow.Cells.Add(cell5)
'    '6
'    Dim cell6 As DataGridViewTextBoxCell = New DataGridViewTextBoxCell
'    cell6.Value = Format(ВерхнееЗначениеВыключения, "Fixed")
'    newRow.Cells.Add(cell6)
'    'в конце добавить строку
'    'DGVСоответсвие.Rows.Add(newRow)
'    DGVСоответсвие.Rows.Insert(I, newRow)
'End Sub

'Private Sub DGVСоответсвие_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles DGVСоответсвие.CellValidating
'    'if (_values.TryGetValue(CalcKey(e.RowIndex, e.ColumnIndex), out value))
'    'e.Value = value
'    'If e.Empty Then
'    'OrElse 
'    If (e.ColumnIndex = 5 OrElse e.ColumnIndex = 6) AndAlso CBool(DGVСоответсвие.Rows(e.RowIndex).Cells(1).Value) = False Then
'        'If Not (e.GetType Is GetType(Double)) Then
'        '    MsgBox("Должно быть число!", CType(MsgBoxStyle.Information & MsgBoxStyle.OkOnly, MsgBoxStyle), "Ошибка ввода")
'        'End If
'        'returnValue = Conversion.Val(InputStr)
'        If Not IsNumeric(e.FormattedValue) Then
'            MsgBox("Должно быть число!", CType(MsgBoxStyle.Information & MsgBoxStyle.OkOnly, MsgBoxStyle), "Ошибка ввода")
'            e.Cancel = True
'        End If

'    End If


'    'If Not IsNumeric(e.va) Then

'    'If Not e.Value Is numeric Then

'    'End If



'End Sub

''Private Sub DGVСоответсвие_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGVСоответсвие.CellClick
''    If ТаблицаЗаполнена AndAlso e.ColumnIndex = 1 Then
''        'If CType(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex), DataGridViewCheckBoxCell).Value = True Then
''        'If CBool(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) = True Then
''        '    MessageBox.Show("True")
''        'Else
''        '    MessageBox.Show("false")
''        'End If
''        DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Not CBool(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
''        Dim cell1 As DataGridViewCheckBoxCell = CType(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex).Clone, DataGridViewCheckBoxCell)
''        If CBool(cell1.Value) = True Then
''            MessageBox.Show("True")
''        Else
''            MessageBox.Show("false")
''        End If
''        DGVСоответсвие.EndEdit()

''    End If
''End Sub


'Private Sub DGVСоответсвие_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DGVСоответсвие.CellMouseClick
'    If ТаблицаЗаполнена AndAlso e.ColumnIndex = 1 Then
'        'If CType(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex), DataGridViewCheckBoxCell).Value = True Then
'        'If CBool(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) = True Then
'        '    MessageBox.Show("True")
'        'Else
'        '    MessageBox.Show("false")
'        'End If
'        DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Not CBool(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

'        mColumnIndex = e.ColumnIndex
'        mRowIndex = e.RowIndex

'        Timer1.Enabled = True
'        'Dim cell1 As DataGridViewCheckBoxCell = CType(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex).Clone, DataGridViewCheckBoxCell)
'        'If CBool(cell1.Value) = True Then
'        '    MessageBox.Show("True")
'        'Else
'        '    MessageBox.Show("false")
'        'End If
'        'DGVСоответсвие.EndEdit()
'    End If
'End Sub

''Private Sub DGVСоответсвие_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DGVСоответсвие.CellMouseClick
''    If ТаблицаЗаполнена AndAlso e.ColumnIndex = 1 Then
''        'If CType(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex), DataGridViewCheckBoxCell).Value = True Then
''        If CBool(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) = True Then
''            'MessageBox.Show("True")
''            RadioButton2.Checked = CBool(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
''        Else
''            'MessageBox.Show("false")
''            RadioButton2.Checked = CBool(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
''        End If
''    End If
''End Sub



''Private Sub DGVСоответсвие_CellMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DGVСоответсвие.CellMouseUp
''    If ТаблицаЗаполнена AndAlso e.ColumnIndex = 1 Then
''        'If CType(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex), DataGridViewCheckBoxCell).Value = True Then
''        If CBool(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) = True Then
''            MessageBox.Show("True")
''        Else
''            MessageBox.Show("false")
''        End If
''    End If
''End Sub


''Private Sub DGVСоответсвие_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGVСоответсвие.CellValidated
''    If ТаблицаЗаполнена AndAlso e.ColumnIndex = 1 Then
''        'If CType(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex), DataGridViewCheckBoxCell).Value = True Then
''        If CBool(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) = True Then
''            MessageBox.Show("True")
''        Else
''            MessageBox.Show("false")
''        End If
''    End If
''End Sub

''Private Sub DGVСоответсвие_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DGVСоответсвие.CellPainting
''    If ТаблицаЗаполнена AndAlso e.ColumnIndex = 1 Then
''        'If CType(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex), DataGridViewCheckBoxCell).Value = True Then
''        If CBool(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) = True Then
''            MessageBox.Show("True")
''        Else
''            MessageBox.Show("false")
''        End If
''    End If

''End Sub


''Private Sub DGVСоответсвие_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGVСоответсвие.CellValueChanged
''    If ТаблицаЗаполнена AndAlso e.ColumnIndex = 1 Then
''        'If CType(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex), DataGridViewCheckBoxCell).Value = True Then
''        If CBool(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) = True Then
''            MessageBox.Show("True")
''        Else
''            MessageBox.Show("false")
''        End If
''    End If

''End Sub


''Private Sub DGVСоответсвие_CellStateChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellStateChangedEventArgs) Handles DGVСоответсвие.CellStateChanged
''    If ТаблицаЗаполнена AndAlso e.Cell.ColumnIndex = 1 Then
''        'If CType(DGVСоответсвие.Rows(e.RowIndex).Cells(e.ColumnIndex), DataGridViewCheckBoxCell).Value = True Then
''        If CBool(DGVСоответсвие.Rows(e.Cell.RowIndex).Cells(e.Cell.ColumnIndex).Value) = True Then
''            'MessageBox.Show("True")
''        Else
''            'MessageBox.Show("false")
''        End If
''        RadioButton2.Checked = CBool(DGVСоответсвие.Rows(e.Cell.RowIndex).Cells(e.Cell.ColumnIndex).Value)

''    End If

''End Sub

'Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
'    Timer1.Enabled = False
'    DGVСоответсвие.EndEdit()
'    If CBool(DGVСоответсвие.Rows(mRowIndex).Cells(mColumnIndex).Value) = True Then
'        'MessageBox.Show("True")
'        DGVСоответсвие.Rows.RemoveAt(mRowIndex)
'        СтрокаТаблицыПоЛогикеTrue(mRowIndex)

'    Else
'        'MessageBox.Show("false")
'        DGVСоответсвие.Rows.RemoveAt(mRowIndex)
'        СтрокаТаблицыПоЛогикеFalse(mRowIndex)

'    End If
'    'DGVСоответсвие.Rows(mRowIndex).Selected = True
'    DGVСоответсвие.CurrentCell = DGVСоответсвие.Rows(mRowIndex).Cells(mColumnIndex)
'    'RadioButton2.Checked = CBool(DGVСоответсвие.Rows(mRowIndex).Cells(mColumnIndex).Value)


'End Sub

'Private Sub ВключитьDGVСоответсвие(ByVal Доступ As Boolean)
'    If Доступ Then
'        DGVСоответсвие.Enabled = True
'        DGVСоответсвие.DefaultCellStyle.BackColor = Color.White
'        DGVСоответсвие.DefaultCellStyle.SelectionForeColor = Color.White
'        DGVСоответсвие.DefaultCellStyle.SelectionBackColor = Color.Blue 'Color.FromName("Highlight")
'        ToolStripButtonЗаписатьВеличину.Enabled = True
'    Else
'        DGVСоответсвие.Enabled = False
'        DGVСоответсвие.DefaultCellStyle.BackColor = Color.Gainsboro
'        DGVСоответсвие.DefaultCellStyle.SelectionBackColor = Color.Gainsboro
'        ToolStripButtonЗаписатьВеличину.Enabled = False
'    End If
'End Sub

