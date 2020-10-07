Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms

' в конструкторе передаются массивы для заполнения combobox и устаналиваются флаги для применения стиля Лист если надо
' при добавлениии в набор Листа вызывается событие и по нему такжк добавляется значение в Лист родительского контрола
' сделать логические свойства для заполненных или установленных ComboBox повторяющие число ComboBox
' сделать событие CellValidating при смене или ComboBoxValue1_TextChanged которое вызывает CellValidating
' а в родительском контейнере подписаться на него и работать с ErrorText

'keyТипКонтрола-ТипКонтрола-Описание
' 1	TextBox:	Текстовое поле для примечаний
' не используется

' 2	ListBox:	Фиксированный список значений
' ComboBoxValue1.DropDownStyle = ComboBoxStyle.DropDownList
' не используется

' 4	CheckBox:	Логический чек
' ComboBoxValue1.DropDownStyle = ComboBoxStyle.DropDownList cо значениями True False
' не используется

' 5	ComboBox:	Список значений с полем ввода
' не использовать ComboBoxValue1.DropDownStyle = ComboBoxStyle.DropDown
' на самом деле добавлять не надо и использовать Фиксированный список значений
' ComboBoxValue1.DropDownStyle = ComboBoxStyle.DropDownList

' 6	DateBox:	Ввод даты
' Me.DateTimePicker1.Format = DateTimePickerFormat.Short
' Me.DateTimePicker1.ShowUpDown = false
' 16.04.2010

' 7	TimeBox:	Ввод времени
' Me.DateTimePicker1.ShowUpDown = True
' Me.DateTimePicker1.Format = DateTimePickerFormat.Time

' 8	DigitalBox:	Ввод цифр
' единственный контрол с таким стилем
' ComboBoxValue1.DropDownStyle = ComboBoxStyle.DropDown
' здесь проверка на цифру
' первый элемент должен быть равер ""

''' <summary>
''' Тип Контрола
''' </summary>
Public Enum ControlStageType
    TextBox
    ListBox
    CheckBox
    ComboBox
    DateBox
    TimeBox
    DigitalBox
End Enum

#Region "EditControl"
' Как проистекает процесс редактирования в гриде? Вот пользователь оказывается на (пока не существующей) паспорт-ячейке и нажимает F2. 
' Что произойдет? Будет вызван специальный метод и ему будет сообщено, что содержит данная ячейка в данный момент в своем свойстве Value. 
' Там может быть или null, или уже существующий объект-паспорт. Мы из этого метода будем вызывать метод разрабатываемого класса StageEdit, 
' который называется SetupControls(PassportCellFind passportCellFind). Он получает на вход как раз содержимое ячейки до редактирования. 
' Только вместо null будем передавать фиктивный (или некий ”начальный”) паспорт. 
' Метод SetupControls инициализирует редактируемые control-ы значениями полей переданного объекта PassportCellFind. 
' Таким образом, если ячейка уже содержала данные, то при нажатии F2 в выпадающем списке будет выделено именно это значение. 
' Чтобы впоследствии можно было отказаться от изменений, мы будем редактировать не непосредственно переданный объект, а его временную копию. 
' Эта копия создается в методе SetupControls и помещается в переменную _tempPassport. 
Partial Public Class StageEdit
    Inherits UserControl
    Implements IDataGridViewEditingControl

    Private tmpStageForEditSession As PassportCellFind
    Private Const IncorrectCondition As String = "Некорректно заданы условия"

    Public Sub New()
        InitializeComponent()
    End Sub

#Region "IDataGridViewEditingControl Members"
    ' Но это еще была "надводная часть айсберга". 
    ' А без реализации интерфейса IDataGridViewEditingControl не будет ни малейшего шанса поместить control в какую-либо ячейку. 
    ' Вот описание этого интерфейса:
    ' Все свойства/методы могут быть вызваны только в фазе редактирования. 
    ' В обычном режиме объекта типа StageEdit просто не существует, а поэтому и никакие его члены не вызываются.

    ' --- ApplyCellStyleToEditingControl --------------------------------------
    ' Этот метод вызывается grid-ом в момент перехода ячейки в фазу редактирования и дает возможность использовать стиль текущей ячейки в редактирующем control-е. 
    ' Кстати, здесь же проделываем еще одну важную вещь: устанавливаем свойство MinimumSize редактора равным его реальным размерам. 
    ' Но на самом деле интересует только высота. Как будет показано ниже, на время редактирования пытаемся увеличить высоту ячейки для более корректного отображения в ней редактирующих control-ов. 
    ' При этом родительский grid пытается не ячейку подогнать под control, а, напротив, – control под ячейку, и тем самым срезает пару пикселов снизу у каждого из редактирующих control-ов. 
    ' Устанавливая MinimumSize, заявляем, что скорректировать (в сторону увеличения) все же придется именно высоту строки, т.к. решительно против попытки уменьшить высоту редактора. 
    ' Дальнейшее развитие этой темы см. в описании метода InitializeEditingControl в следующей таблице.
    Public Sub ApplyCellStyleToEditingControl(ByVal dataGridViewCellStyle As DataGridViewCellStyle) Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl
        'Me._dtp.Font = dataGridViewCellStyle.Font
        'Me._mskEdit.Font = dataGridViewCellStyle.Font
        'Me._cb.Font = dataGridViewCellStyle.Font

        Me.MinimumSize = Me.Size
    End Sub

    ' --- EditingControlDataGridView ------------------------------------------
    ' Позволяет установить или считать ссылку на DataGridView. Значение этого свойства задает grid в момент входа ячейки в фазу редактирования. 
    ' Через него родительский grid дает ссылку на самого себя. Именно благодаря этому у появляется возможность вызова DataGridView.NotifyCurrentCellDirty в "паспорт-редакторе".
    Public Property EditingControlDataGridView() As DataGridView Implements IDataGridViewEditingControl.EditingControlDataGridView

    ' --- EditingControlFormattedValue ----------------------------------------
    ' Свойство чрезвычайной важности. Зачем нужен setter, неясно, за все время исследований не был вызван ни кем, ни разу. 
    ' Напротив, getter вызывается при малейших изменениях в редактируемом объекте (очевидно, для отображения значения свойства на экране – прим. ред.). 
    ' Дело в том, что любая ячейка содержит сразу 2 значения: абсолютное (в свойстве Value) и форматированное (в свойстве FormattedValue). 
    ' Причем, как было показано выше, мало того, что эти свойства могут возвращать различные значения, так еще и тип возвращаемого значения может отличаться. 
    ' EditingControlFormattedValue возвращает как раз форматированное значение. Данное свойство должно быть согласовано со свойством ячейки FormattedValueType. 
    ' Обычно ячейка возвращает через Value объект типа object, а через FormattedValue тип string. Я решил, что в нашем случае они оба будут возвращать тип Passport. 
    ' Поэтому анализируемое свойство интерфейса просто возвращает временный объект, на который ссылается _tempPassport.
    Public Property EditingControlFormattedValue() As Object Implements IDataGridViewEditingControl.EditingControlFormattedValue
        Get
            Return Me.tmpStageForEditSession
        End Get
        '    'nothing to do...
        Set(ByVal value As Object)

        End Set
    End Property

    ' --- EditingPanelCursor --------------------------------------------------
    ' При входе в фазу редактирования в ячейку сначала помещается панель (экземпляр класса System.Windows.Forms.Panel), занимающая всю доступную площадь ячейки. 
    ' Форму курсора над этой панелью и определяет данное свойство. Однако обычно всю площадь панели занимает редактирующий control. 
    ' Так что на практике значение данного свойства особого смысла не имеет.
    Public ReadOnly Property EditingPanelCursor() As Cursor Implements IDataGridViewEditingControl.EditingPanelCursor
        Get
            Return MyBase.Cursor
        End Get
    End Property

    ' --- GetEditingControlFormattedValue -------------------------------------
    ' Фактически дублирует поведение свойства EditingControlFormattedValue. Неудивительно поэтому, что характерная реализация данного метода 
    ' состоит из одной строчки - return EditingControlFormattedValue.
    Public Function GetEditingControlFormattedValue(ByVal context As DataGridViewDataErrorContexts) As Object Implements IDataGridViewEditingControl.GetEditingControlFormattedValue
        Return Me.EditingControlFormattedValue
    End Function

    ' --- PrepareEditingControlForEdit ----------------------------------------
    ' Метод вызывается grid-ом в момент входа ячейки в фазу редактирования и дает возможность подготовить редактор к процессу редактирования.
    Public Sub PrepareEditingControlForEdit(ByVal selectAll As Boolean) Implements IDataGridViewEditingControl.PrepareEditingControlForEdit
    End Sub

    ' --- RepositionEditingControlOnValueChange -------------------------------
    ' Значение true говорит grid-у о необходимости изменения позиционирования ячейки при изменении ее содержимого. 
    ' Например, если бы мы создавали свою реализацию DataGridViewTextBoxCell и хотели, чтобы при достижении правой границы ячейки текст 
    ' переносился на следующую строчку, можно было бы вернуть в нужный момент true в этом свойстве.
    Public ReadOnly Property RepositionEditingControlOnValueChange() As Boolean Implements IDataGridViewEditingControl.RepositionEditingControlOnValueChange
        Get
            Return False
        End Get
    End Property

    ' --- EditingControlRowIndex ----------------------------------------------
    ' Значение свойства задается grid-ом в момент входа ячейки в фазу редактирования и сообщает редактору индекс редактируемой строки
    Public Property EditingControlRowIndex() As Integer Implements IDataGridViewEditingControl.EditingControlRowIndex

    ' --- EditingControlValueChanged ------------------------------------------
    ' Если значение ячейки изменено, данное свойство должно возвращать true.
    Public Property EditingControlValueChanged() As Boolean Implements IDataGridViewEditingControl.EditingControlValueChanged

    ' --- EditingControlWantsInputKey -----------------------------------------
    ' Возвращая из этого метода true, мы говорим, что хотим использовать данное сочетание клавишей. 
    Public Function EditingControlWantsInputKey(ByVal keyData As Keys, ByVal dataGridViewWantsInputKey As Boolean) As Boolean Implements IDataGridViewEditingControl.EditingControlWantsInputKey
        Select Case keyData And Keys.KeyCode
            Case Keys.Down, Keys.Right, Keys.Left, Keys.Up, Keys.Home, Keys.[End]
                Return True
            Case Else
                Return False
        End Select
    End Function
#End Region

    ''' <summary>
    ''' получает на вход содержимое ячейки до редактирования
    ''' </summary>
    ''' <param name="ps"></param>
    Public Sub SetupControls(ByVal ps As PassportCellFind)
        'Me._cb.SelectedItem = ps.Series
        'Me._mskEdit.Text = ps.Number
        'Me._dtp.Value = ps.IssueDate
        Me.tmpStageForEditSession = New PassportCellFind() With {.TypeControlStage = ps.TypeControlStage}

        Select Case ps.TypeControlStage
            Case ControlStageType.DigitalBox
                ComboBoxValue1.Visible = True
                ComboBoxValue2.Visible = True
                ComboBoxValue3.Visible = True

                ComboBoxValue1.DropDownStyle = ComboBoxStyle.DropDown
                ComboBoxValue2.DropDownStyle = ComboBoxStyle.DropDown
                ComboBoxValue3.DropDownStyle = ComboBoxStyle.DropDown

                DateTimePicker1.Visible = False
                DateTimePicker2.Visible = False
                DateTimePicker3.Visible = False

                Exit Select
            Case ControlStageType.DateBox
                ComboBoxValue1.Visible = False
                ComboBoxValue2.Visible = False
                ComboBoxValue3.Visible = False

                DateTimePicker1.Visible = True
                DateTimePicker1.Format = DateTimePickerFormat.Short
                DateTimePicker1.ShowUpDown = False

                DateTimePicker2.Visible = True
                DateTimePicker2.Format = DateTimePickerFormat.Short
                DateTimePicker2.ShowUpDown = False

                DateTimePicker3.Visible = True
                DateTimePicker3.Format = DateTimePickerFormat.Short
                DateTimePicker3.ShowUpDown = False

                Exit Select
            Case ControlStageType.TimeBox
                ComboBoxValue1.Visible = False
                ComboBoxValue2.Visible = False
                ComboBoxValue3.Visible = False

                DateTimePicker1.Visible = True
                DateTimePicker1.Format = DateTimePickerFormat.Time
                DateTimePicker1.ShowUpDown = True

                DateTimePicker2.Visible = True
                DateTimePicker2.Format = DateTimePickerFormat.Time
                DateTimePicker2.ShowUpDown = True

                DateTimePicker3.Visible = True
                DateTimePicker3.Format = DateTimePickerFormat.Time
                DateTimePicker3.ShowUpDown = True

                Exit Select
            Case Else
                ComboBoxValue1.Visible = True
                ComboBoxValue2.Visible = True
                ComboBoxValue3.Visible = True

                ComboBoxValue1.DropDownStyle = ComboBoxStyle.DropDownList
                ComboBoxValue2.DropDownStyle = ComboBoxStyle.DropDownList
                ComboBoxValue3.DropDownStyle = ComboBoxStyle.DropDownList

                DateTimePicker1.Visible = False
                DateTimePicker2.Visible = False
                DateTimePicker3.Visible = False
        End Select

        ' в начале заполнить массивы
        If ps.ComboBoxItems1.Count <> 0 Then ComboBoxValue1.Items.Clear()
        If ps.ComboBoxItems2.Count <> 0 Then ComboBoxValue2.Items.Clear()
        If ps.ComboBoxItems3.Count <> 0 Then ComboBoxValue3.Items.Clear()

        ComboBoxValue1.Items.AddRange(ps.ComboBoxItems1.ToArray)
        ComboBoxValue2.Items.AddRange(ps.ComboBoxItems2.ToArray)
        ComboBoxValue3.Items.AddRange(ps.ComboBoxItems3.ToArray)

        tmpStageForEditSession.ComboBoxItems1.Clear()
        tmpStageForEditSession.ComboBoxItems2.Clear()
        tmpStageForEditSession.ComboBoxItems3.Clear()

        tmpStageForEditSession.ComboBoxItems1.AddRange(ps.ComboBoxItems1.ToArray)
        tmpStageForEditSession.ComboBoxItems2.AddRange(ps.ComboBoxItems2.ToArray)
        tmpStageForEditSession.ComboBoxItems3.AddRange(ps.ComboBoxItems3.ToArray)

        ' затем присвоить свойства иначе в событии OnValueChanged может быть вызвана проверка на цифру для листа с тектом
        ComboBoxSort.SelectedItem = ps.SortValue

        ComboBoxCondition1.SelectedItem = ps.Condition1
        ComboBoxCondition2.SelectedItem = ps.Condition2
        ComboBoxCondition3.SelectedItem = ps.Condition3

        ComboBoxValue1.SelectedItem = ps.ComboBoxText1
        ComboBoxValue2.SelectedItem = ps.ComboBoxText2
        ComboBoxValue3.SelectedItem = ps.ComboBoxText3

        DateTimePicker1.Value = ps.DateTimePickerText1
        DateTimePicker2.Value = ps.DateTimePickerText2
        DateTimePicker3.Value = ps.DateTimePickerText3
    End Sub

#Region "Events OnValueChanged"
    ' Каждый из подчиненных control-ов при любом изменении своего содержимого вызывает метод OnValueChanged(). 
    ' В нем, во-первых, данные, введенные пользователем, помещаются во временный объект. Во-вторых, производится оповещение grid-а, 
    ' что временный объект подвергся изменениям. Делается это элементарно – вызовом метода NotifyCurrentCellDirty(true). 
    ' Этот указывает, что были произведены изменения, и grid должен учитывать это при окончании редактирования ячейки.
    Private Sub OnValueChanged()
        Me.EditingControlValueChanged = True

        'Me.tmpStageForEditSession.Series = IIf(Me._cb.SelectedItem Is Nothing, String.Empty, Me._cb.SelectedItem.ToString())
        'Me.tmpStageForEditSession.Number = (IIf(Me._mskEdit.Text Is Nothing, String.Empty, Me._mskEdit.Text))
        'Me.tmpStageForEditSession.IssueDate = Me._dtp.Value

        If ComboBoxSort.SelectedItem Is Nothing Then
            tmpStageForEditSession.SortValue = String.Empty
        Else
            tmpStageForEditSession.SortValue = ComboBoxSort.SelectedItem.ToString()
        End If

        If ComboBoxCondition1.SelectedItem Is Nothing Then
            tmpStageForEditSession.Condition1 = String.Empty
        Else
            tmpStageForEditSession.Condition1 = ComboBoxCondition1.SelectedItem.ToString()
        End If

        If ComboBoxCondition2.SelectedItem Is Nothing Then
            tmpStageForEditSession.Condition2 = String.Empty
        Else
            tmpStageForEditSession.Condition2 = ComboBoxCondition2.SelectedItem.ToString()
        End If

        If ComboBoxCondition3.SelectedItem Is Nothing Then
            tmpStageForEditSession.Condition3 = String.Empty
        Else
            tmpStageForEditSession.Condition3 = ComboBoxCondition3.SelectedItem.ToString()
        End If

        If ComboBoxValue1.SelectedItem Is Nothing Then
            tmpStageForEditSession.ComboBoxText1 = String.Empty
        Else
            tmpStageForEditSession.ComboBoxText1 = ComboBoxValue1.SelectedItem.ToString()
        End If

        If ComboBoxValue2.SelectedItem Is Nothing Then
            tmpStageForEditSession.ComboBoxText2 = String.Empty
        Else
            tmpStageForEditSession.ComboBoxText2 = ComboBoxValue2.SelectedItem.ToString()
        End If

        If ComboBoxValue3.SelectedItem Is Nothing Then
            tmpStageForEditSession.ComboBoxText3 = String.Empty
        Else
            tmpStageForEditSession.ComboBoxText3 = ComboBoxValue3.SelectedItem.ToString()
        End If

        tmpStageForEditSession.DateTimePickerText1 = DateTimePicker1.Value
        tmpStageForEditSession.DateTimePickerText2 = DateTimePicker2.Value
        tmpStageForEditSession.DateTimePickerText3 = DateTimePicker3.Value

        Dim dgv As DataGridView = Me.EditingControlDataGridView
        dgv.Rows(dgv.CurrentRow.Index).ErrorText = ""

        tmpStageForEditSession.IsSetUpCondition1 = False
        tmpStageForEditSession.IsSetUpCondition2 = False
        tmpStageForEditSession.IsSetUpCondition3 = False

        If tmpStageForEditSession.TypeControlStage = ControlStageType.DateBox OrElse tmpStageForEditSession.TypeControlStage = ControlStageType.TimeBox Then
            ' логическое значение установки условия только по факту наличия непустого значения
            If tmpStageForEditSession.Condition1 <> "" Then
                tmpStageForEditSession.IsSetUpCondition1 = True
            Else
                tmpStageForEditSession.IsSetUpCondition1 = False
            End If

            If tmpStageForEditSession.Condition2 <> "" Then
                tmpStageForEditSession.IsSetUpCondition2 = True
            Else
                tmpStageForEditSession.IsSetUpCondition2 = False
            End If

            If tmpStageForEditSession.Condition3 <> "" Then
                tmpStageForEditSession.IsSetUpCondition3 = True
            Else
                tmpStageForEditSession.IsSetUpCondition3 = False
            End If
        Else
            ' логическое значение установки условия только по факту наличия двух непустых свойств: условия и значения
            If tmpStageForEditSession.Condition1 <> "" AndAlso tmpStageForEditSession.ComboBoxText1 <> "" Then
                tmpStageForEditSession.IsSetUpCondition1 = True
            ElseIf (tmpStageForEditSession.Condition1 = "" AndAlso tmpStageForEditSession.ComboBoxText1 <> "") OrElse (tmpStageForEditSession.Condition1 <> "" AndAlso tmpStageForEditSession.ComboBoxText1 = "") Then
                dgv.Rows(dgv.CurrentRow.Index).ErrorText = IncorrectCondition
                tmpStageForEditSession.IsSetUpCondition1 = False
            End If

            If tmpStageForEditSession.Condition2 <> "" AndAlso tmpStageForEditSession.ComboBoxText2 <> "" Then
                tmpStageForEditSession.IsSetUpCondition2 = True
            ElseIf (tmpStageForEditSession.Condition2 = "" AndAlso tmpStageForEditSession.ComboBoxText2 <> "") OrElse (tmpStageForEditSession.Condition2 <> "" AndAlso tmpStageForEditSession.ComboBoxText2 = "") Then
                dgv.Rows(dgv.CurrentRow.Index).ErrorText = IncorrectCondition
                tmpStageForEditSession.IsSetUpCondition2 = False
            End If

            If tmpStageForEditSession.Condition3 <> "" AndAlso tmpStageForEditSession.ComboBoxText3 <> "" Then
                tmpStageForEditSession.IsSetUpCondition3 = True
            ElseIf (tmpStageForEditSession.Condition3 = "" AndAlso tmpStageForEditSession.ComboBoxText3 <> "") OrElse (tmpStageForEditSession.Condition3 <> "" AndAlso tmpStageForEditSession.ComboBoxText3 = "") Then
                dgv.Rows(dgv.CurrentRow.Index).ErrorText = IncorrectCondition
                tmpStageForEditSession.IsSetUpCondition3 = False
            End If
        End If

        ' переписывать только DigitalBox, так как только там возможно добавление в источник
        If tmpStageForEditSession.TypeControlStage = ControlStageType.DigitalBox Then
            tmpStageForEditSession.ComboBoxItems1.Clear()
            tmpStageForEditSession.ComboBoxItems2.Clear()
            tmpStageForEditSession.ComboBoxItems3.Clear()
            Dim I As Integer

            If ComboBoxValue1.Items.Count > 0 Then
                For I = 0 To ComboBoxValue1.Items.Count - 1
                    tmpStageForEditSession.ComboBoxItems1.Add(CStr(ComboBoxValue1.Items(I)))
                Next
            End If

            If ComboBoxValue2.Items.Count > 0 Then
                For I = 0 To ComboBoxValue2.Items.Count - 1
                    tmpStageForEditSession.ComboBoxItems2.Add(CStr(ComboBoxValue2.Items(I)))
                Next
            End If

            If ComboBoxValue3.Items.Count > 0 Then
                For I = 0 To ComboBoxValue3.Items.Count - 1
                    tmpStageForEditSession.ComboBoxItems3.Add(CStr(ComboBoxValue3.Items(I)))
                Next
            End If
        End If

        tmpStageForEditSession.Sort = Not (tmpStageForEditSession.SortValue = "(отсутствует)" OrElse tmpStageForEditSession.SortValue = String.Empty)

        If dgv IsNot Nothing Then
            dgv.NotifyCurrentCellDirty(True)
        End If
    End Sub

    ' для условий только обработка SelectedIndexChanged
    Private Sub ComboBoxCondition_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxCondition1.SelectedIndexChanged,
                                                                                                            ComboBoxCondition2.SelectedIndexChanged,
                                                                                                            ComboBoxCondition3.SelectedIndexChanged,
                                                                                                            ComboBoxSort.SelectedIndexChanged
        OnValueChanged()
    End Sub


    ' для значений возможно добавление в список цифры - обработка TextChanged для цифровых Combo
    Private Sub ComboBoxValue_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxValue1.TextChanged,
                                                                                                ComboBoxValue2.TextChanged,
                                                                                                ComboBoxValue3.TextChanged
        If tmpStageForEditSession.TypeControlStage = ControlStageType.DigitalBox Then
            ComboBoxValidating(sender, New CancelEventArgs)
        Else
            OnValueChanged()
        End If
    End Sub

    ' добавить в источник новую цифру введенную пользователем
    Private Sub ComboBoxValue_LostFocus(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxValue1.LostFocus,
                                                                                                ComboBoxValue2.LostFocus,
                                                                                                ComboBoxValue3.LostFocus
        If tmpStageForEditSession.TypeControlStage = ControlStageType.DigitalBox Then
            ComboBoxLostFocus(sender, New EventArgs)
        End If
    End Sub

    Private Sub ComboBoxLostFocus(ByVal sender As Object, ByVal e As EventArgs)
        Dim combo As ComboBox = CType(sender, ComboBox)

        If IsNumeric(combo.Text) Then
            'If combo.Text.Contains(",") Then combo.Text.Replace(","c, "."c)

            If Not combo.Items.Contains(combo.Text) Then
                'Combo.Items.Insert(0, Combo.Text)
                Dim strDigital As String = combo.Text
                Dim digitalValue As Double = CDbl(strDigital)
                'Double.TryParse(strDigital, DigitalDouble) не работает 'здесь запятая переводится в точку

                'If strDigital.Contains(",") Then strDigital.Replace(","c, "."c)

                combo.Items.Insert(0, digitalValue.ToString)
                combo.SelectedIndex = 0
                OnValueChanged()
            End If
        Else
            ComboBoxValidating(sender, New CancelEventArgs)
        End If
    End Sub

    ' попытка выловить ввод точки в ComboBox не увенчалась успехом - символ не вводился
    ' проблема перевода цифры с запятой в цифру с точкой
    ' '  Boolean флаг используется для определения символа отличного от числа
    ' Private nonNumberEntered As Boolean = False
    ' '  Handle the KeyDown event to determine the type of character entered into the control.
    ' Private Sub ComboBoxValue1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboBoxValue1.KeyDown
    '    ' установить флаг false.
    '    nonNumberEntered = False
    '    'Keys.Decimal
    '    ' Определить какая клавиша была введен с клавиатуры
    '    If e.KeyCode < Keys.D0 OrElse e.KeyCode > Keys.D9 Then
    '        ' Определить нажатую клавишу с keypad.
    '        If e.KeyCode < Keys.NumPad0 OrElse e.KeyCode > Keys.NumPad9 Then
    '            ' определить клавиша пробел.
    '            If e.KeyCode <> Keys.Back Then
    '                ' не числовая клавиша была нажата. 
    '                ' установить флаг true в событии нажатия.
    '                nonNumberEntered = True
    '            End If
    '        End If
    '    End If
    'End Sub

    '' Это событие вызывается KeyDown и может быть использовано
    '' для предупреждения ввода символов в контрол.
    'Private Sub ComboBoxValue1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ComboBoxValue1.KeyPress
    '    ' Проверить флаг который был установлен в событии KeyDown.
    '    If nonNumberEntered = True Then
    '        ' Остановить ввод символа введенного в контрол т.к. он не цифровой.
    '        e.Handled = True
    '    End If
    'End Sub

    Private Sub ComboBoxValidating(ByVal sender As Object, ByVal e As CancelEventArgs)
        Dim comboBox As ComboBox = CType(sender, ComboBox)
        'If Combo.Text Is Nothing OrElse String.IsNullOrEmpty(Combo.Text) Then
        '    Return
        'End If

        If Not comboBox.Text = "" Then
            If Not IsNumeric(comboBox.Text) Then
                'Dim result As Double
                'If Not Double.TryParse(Combo.Text, result) Then
                ErrorProvider1.SetError(comboBox, "Только цифра разрешена")

                Dim dgv As DataGridView = Me.EditingControlDataGridView
                If dgv.Rows(dgv.CurrentRow.Index).ErrorText = "" Then
                    ' текст ошибки только если нет ошибки условия
                    dgv.Rows(dgv.CurrentRow.Index).ErrorText = "Только цифра разрешена"
                End If

                e.Cancel = True
            Else
                ' очистить ошибку.
                ErrorProvider1.SetError(comboBox, "")
                OnValueChanged()
            End If
        Else
            Dim dgv As DataGridView = Me.EditingControlDataGridView
            dgv.Rows(dgv.CurrentRow.Index).ErrorText = ""
            OnValueChanged()
        End If
    End Sub

    ' для даты обработка ValueChanged
    Private Sub DateTimePicker_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DateTimePicker1.ValueChanged,
                                                                                                    DateTimePicker2.ValueChanged,
                                                                                                    DateTimePicker3.ValueChanged
        OnValueChanged()
    End Sub

    'Private Sub _cb_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _cb.SelectedIndexChanged
    '    Me.OnValueChanged()
    'End Sub

    'Private Sub _dtp_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _dtp.ValueChanged
    '    Me.OnValueChanged()
    'End Sub

    'Private Sub _mskEdit_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _mskEdit.Validating
    '    If Me._mskEdit.MaskCompleted Then
    '        Me.OnValueChanged()
    '    End If
    '    e.Cancel = Not Me._mskEdit.MaskCompleted
    'End Sub
#End Region

End Class
#End Region

#Region "Custom Cell"
' Создание специализированного типа ячейки и колонки
' создание специальной ячейки, использующей для редактирования данных control StageEdit. 
' Объявим новый класс DataGridViewStageCell, наследника DataGridViewTextBoxCell. Если в свойстве Value данной ячейки не содержится объекта типа PassportCellFind, 
' то она должна в фазе отображения показывать строку "Фильтр отбора для этапа не установлен...". 
' перечислены унаследованные от DataGridViewTextBoxCell члены, которые нужно переопределить.

Public Class DataGridViewStageCell
    Inherits DataGridViewTextBoxCell

    Private Const DEFAULT_STRING As String = "Фильтр отбора для этапа не установлен..."
    Private _heightOfRowBeforeEditMode As Integer

    Public Sub New()
        MyBase.New()
    End Sub

    ' --- InitializeEditingControl --------------------------------------------
    ' Вызывается grid-ом в момент перехода ячейки в фазу редактирования. Назначение этого метода – подготовить редактор к процессу редактирования. 
    ' В процессе подготовки к редактированию производится коррекция высоты строки так, чтобы редактирующий control при отображении в ней не обрезался. 
    ' Далее получаем объект типа PassportCellFind, хранящийся в ячейке, и передаем его методу SetupControls().
    Public Overloads Overrides Sub InitializeEditingControl(ByVal rowIndex As Integer, ByVal initialFormattedValue As Object, ByVal dataGridViewCellStyle As DataGridViewCellStyle)
        MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle)
        Dim ctrlStageEdit As StageEdit = TryCast(Me.DataGridView.EditingControl, StageEdit)

        Me._heightOfRowBeforeEditMode = Me.OwningRow.Height
        Me.OwningRow.Height = ctrlStageEdit.Height
        Dim pasInCell As PassportCellFind = TryCast(Me.Value, PassportCellFind)

        If pasInCell Is Nothing Then pasInCell = New PassportCellFind()

        ctrlStageEdit.SetupControls(pasInCell)
    End Sub

    Public Overloads Overrides Sub DetachEditingControl()
        If Me._heightOfRowBeforeEditMode > 0 Then
            Me.OwningRow.Height = Me._heightOfRowBeforeEditMode
        End If
        MyBase.DetachEditingControl()
    End Sub

    ' --- EditType ------------------------------------------------------------
    ' Тип редактора, создаваемого для редактирования значения данного типа, в нашем случае typeof(StageEdit).
    Public Overloads Overrides ReadOnly Property EditType() As Type
        Get
            Return GetType(StageEdit)
        End Get
    End Property

    ' --- ValueType -----------------------------------------------------------
    ' Тип значения, хранящегося данной ячейкой. Поскольку создаем спец-ячейку только для объекта PassportCellFind, 
    ' то и хранить она будет только объекты такого типа. Поэтому возвращаем typeof(PassportCellFind).
    Public Overloads Overrides ReadOnly Property ValueType() As Type
        Get
            Return GetType(PassportCellFind)
        End Get
    End Property

    ' --- FormattedValueType --------------------------------------------------
    ' Тип форматированного значения данной ячейки. Вообще, по идее, должны были бы возвращать здесь typeof(string), 
    ' в силу того, что базовая DataGridViewTextBoxCell в фазе отображения умеет отрисовывать именно такие данные. 
    ' А картинки, к примеру, не умеет. Равно как и объекты типа PassportCellFind. Но решаем взять часть работы на себя и 
    ' предоставить ей уже готовый для отображения объект – см. метод GetFormattedValue чуть ниже. 
    ' Поэтому можем вернуть абсолютно то же значение, что и предыдущее свойство, и ни о чем более не беспокоиться.
    Public Overloads Overrides ReadOnly Property FormattedValueType() As Type
        Get
            Return GetType(PassportCellFind)
        End Get
    End Property

    ' --- DefaultNewRowValue --------------------------------------------------
    ' Возвращает значение, отображаемое в новой строке. В этом свойстве мы возвращаем уже упоминавшуюся строку "Фильтр отбора для этапа не установлен...".
    Public Overloads Overrides ReadOnly Property DefaultNewRowValue() As Object
        Get
            Return DEFAULT_STRING
        End Get
    End Property

    ' --- GetFormattedValue ---------------------------------------------------
    ' В этом методе можно возвратить непосредственно объект PassportCellFind, если таковой существует, и строку  "Фильтр отбора для этапа не установлен...", если значение равно null.
    Protected Overloads Overrides Function GetFormattedValue(ByVal value As Object, ByVal rowIndex As Integer, ByRef cellStyle As DataGridViewCellStyle, ByVal valueTypeConverter As TypeConverter, ByVal formattedValueTypeConverter As TypeConverter, ByVal context As DataGridViewDataErrorContexts) As Object
        If value Is Nothing Then
            Return DEFAULT_STRING
        Else
            Return TypeDescriptor.GetConverter(value).ConvertToString(value)
        End If
    End Function
End Class
#End Region

#Region "Custom Column"
' Теперь создадим колонку DataGridStageColumn. Опять же заметим, что могли бы свободно создать нужную колонку, 
' просто создав экземпляр типа DataGridViewColumn через перегруженный конструктор последнего, принимающий объект типа DataGridViewCell, 
' т.е. можно было бы написать нечто вроде: new DataGridViewColumn(new DataGridViewStageCell())
' Это бы установило свойство CellTemplate новой колонки в нужное нам значение. уже разбирали важность этого свойства и причины пагубности описанного подхода. 
' И это еще без учета того, что описанный сценарий лишает новый тип колонки design time-поддержки. А вот в случае создания колонки собственного типа дизайнер её распознает, что вскоре и увидим. 
' Создадим public-класс с именем DataGridStageColumn и унаследуем его от DataGridViewColumn. 
' Какие из членов базового класса требуется переопределить? На этот раз это единственное свойство CellTemplate. 
' В конструкторе без параметров класс вызывает конструктор базового класса, передавая ему в качестве параметра шаблон ячейки (экземпляр класса DataGridViewStageCell).
Public Class DataGridStageColumn
    Inherits DataGridViewColumn

    Public Sub New()
        MyBase.New(New DataGridViewStageCell())
    End Sub

    Public Overloads Overrides Property CellTemplate() As DataGridViewCell
        Get
            Return MyBase.CellTemplate
        End Get
        Set(ByVal value As DataGridViewCell)
            If value IsNot Nothing AndAlso Not value.[GetType]().IsAssignableFrom(GetType(DataGridViewStageCell)) Then
                Throw New InvalidCastException("Cell must be a DataGridViewCell")
            End If
            MyBase.CellTemplate = value
        End Set
    End Property
End Class
#End Region

#Region "PassportCellFind"
' Пока оставим этот класс как есть, и начнем работу с создания центрального объекта, вокруг которого все и будет крутиться. 
' Оформим его в виде public-класса PassportCellFind. Он содержит private-поля. 
' Доступ к ним производится через public-свойства тех же типов, причем setter-ы первых двух полей проверяют корректность 
' предлагаемых значений в соответствии с наложенными ранее ограничениями. У класса также имеются два public-конструктора. 
' Один, с параметрами, просто инициализирует ими поля. Второй же, без параметров, инициализирует поля значениями, принятыми по умолчанию. 
' Собственно, ядро решения готово. Но надо помнить одну важную вещь создадим свой "PassportCellFind-редактор" и научим grid правильно работать с объектом. 
' Но все это будет происходить в фазе редактирования. В фазе же отображения grid захочет показывать объект совершенно самостоятельно. 
' Для сценария вполне подойдет отображение в этом режиме некоторой информативной строки. Именно поэтому "спец-ячейку" 
' унаследуем от DataGridViewTextBoxCell. Здесь вполне устраивает поведение этой ячейки в фазе отображения, но не нравятся ее способности при редактировании. 
' Вот именно этот момент изменим. Здесь также возможны варианты. Я выбрал путь изготовления собственного конвертора. 

<TypeConverter(GetType(PassportConverter))>
Public Class PassportCellFind
    Const OnlyDigital As String = "Значение должно содержать только цифры"

    Public Sub New()
        Me.New(ControlStageType.ComboBox, "", "", "", "", "", "", "", New DateTime(Now.Year, Now.Month, Now.Day), New DateTime(Now.Year, Now.Month, Now.Day), New DateTime(Now.Year, Now.Month, Now.Day), New List(Of String))
    End Sub

    Public Sub New(ByVal typeControlStage As ControlStageType,
                   ByVal sortValue As String,
                   ByVal inCondition1 As String,
                   ByVal inCondition2 As String,
                   ByVal inCondition3 As String,
                   ByVal comboBoxText1 As String,
                   ByVal comboBoxText2 As String,
                   ByVal comboBoxText3 As String,
                   ByVal dateTimePickerText1 As DateTime,
                   ByVal dateTimePickerText2 As DateTime,
                   ByVal dateTimePickerText3 As DateTime,
                   ByVal comboBoxItems As List(Of String))

        Me.TypeControlStage = typeControlStage
        Me.SortValue = sortValue
        Me.Condition1 = inCondition1
        Me.Condition2 = inCondition2
        Me.Condition3 = inCondition3
        Me.ComboBoxText1 = comboBoxText1
        Me.ComboBoxText2 = comboBoxText2
        Me.ComboBoxText3 = comboBoxText3
        Me.DateTimePickerText1 = dateTimePickerText1
        Me.DateTimePickerText2 = dateTimePickerText2
        Me.DateTimePickerText3 = dateTimePickerText3

        ' надо обязательно клонировать
        Me.ComboBoxItems1.AddRange(comboBoxItems.ToArray)
        Me.ComboBoxItems2.AddRange(comboBoxItems.ToArray)
        Me.ComboBoxItems3.AddRange(comboBoxItems.ToArray)

        IsSetUpCondition1 = inCondition1 <> ""
        IsSetUpCondition2 = inCondition2 <> ""
        IsSetUpCondition3 = inCondition3 <> ""
    End Sub

    Public Property TypeControlStage() As ControlStageType
    Public Property Sort() As Boolean
    Public Property SortValue() As String
    Public Property Condition1() As String
    Public Property Condition2() As String
    Public Property Condition3() As String
    Public Property IsSetUpCondition1() As Boolean
    Public Property IsSetUpCondition2() As Boolean
    Public Property IsSetUpCondition3() As Boolean

    Private varComboBoxText1 As String
    Public Property ComboBoxText1() As String
        Get
            Return varComboBoxText1
        End Get
        Set(ByVal value As String)
            If TypeControlStage = ControlStageType.DigitalBox Then
                If Not String.IsNullOrEmpty(value) Then
                    Dim result As Double
                    If Not Double.TryParse(value, result) Then
                        Throw New ArgumentOutOfRangeException("ComboBoxText1", OnlyDigital)
                    End If
                End If
            End If

            varComboBoxText1 = value
        End Set
    End Property

    Private varComboBoxText2 As String
    Public Property ComboBoxText2() As String
        Get
            Return varComboBoxText2
        End Get
        Set(ByVal value As String)
            If TypeControlStage = ControlStageType.DigitalBox Then
                If Not String.IsNullOrEmpty(value) Then
                    Dim result As Double
                    If Not Double.TryParse(value, result) Then
                        Throw New ArgumentOutOfRangeException("ComboBoxText2", OnlyDigital)
                    End If
                End If
            End If

            varComboBoxText2 = value
        End Set
    End Property

    Private varComboBoxText3 As String
    Public Property ComboBoxText3() As String
        Get
            Return varComboBoxText3
        End Get
        Set(ByVal value As String)
            If TypeControlStage = ControlStageType.DigitalBox Then
                If Not String.IsNullOrEmpty(value) Then
                    Dim result As Double
                    If Not Double.TryParse(value, result) Then
                        Throw New ArgumentOutOfRangeException("ComboBoxText3", OnlyDigital)
                    End If
                End If
            End If

            varComboBoxText3 = value
        End Set
    End Property
    Public Property DateTimePickerText1() As DateTime
    Public Property DateTimePickerText2() As DateTime
    Public Property DateTimePickerText3() As DateTime
    Public Property ComboBoxItems1() As List(Of String) = New List(Of String)
    Public Property ComboBoxItems2() As List(Of String) = New List(Of String)
    Public Property ComboBoxItems3() As List(Of String) = New List(Of String)
End Class
#End Region

#Region "PassportConverter"
' Наследуем класс PassportConverter от TypeConverter и переопределяем всего один метод – ConvertTo(). В нем будут передавать объект-PassportCellFind, 
' будем "красиво" раскладывать его свойства в форматированной строке и возвращать последнюю. А как управляться с ней, DataGridViewTextBoxCell и сама знает. 
' Разумеется, после создания своего конвертора надо не забыть сообщить об этом всем интересующимся путем "навешивания" на центральный класс (PassportCellFind) атрибута:
' [TypeConverter(typeof(PassportConverter))]
Public Class PassportConverter
    Inherits TypeConverter

    Private Const DEFAULT_STRING As String = "Фильтр отбора для этапа не установлен..."
    ' Переопределить ConvertTo метод TypeConverter.
    Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext,
                                                  ByVal culture As Globalization.CultureInfo,
                                                  ByVal value As Object,
                                                  ByVal destinationType As Type) As Object
        If destinationType Is GetType(String) Then
            Dim pas As PassportCellFind = TryCast(value, PassportCellFind)
            Dim sb As StringWriter = New StringWriter()
            Dim value1String, value2String, value3String As String

            Select Case pas.TypeControlStage
                Case ControlStageType.DigitalBox
                    value1String = pas.ComboBoxText1
                    value2String = pas.ComboBoxText2
                    value3String = pas.ComboBoxText3
                Case ControlStageType.DateBox
                    ' вводится =#16.04.2010#
                    ' SQL должен быть WHERE (((БазаСнимков.Дата)=#4/16/2010#)
                    ' возможно здесь нужна замена точки на /
                    value1String = $"#{pas.DateTimePickerText1.ToShortDateString}#"
                    value2String = $"#{pas.DateTimePickerText2.ToShortDateString}#"
                    value3String = $"#{pas.DateTimePickerText3.ToShortDateString}#"
                Case ControlStageType.TimeBox
                    ' WHERE (((БазаСнимков.ВремяНачалаСбора)<#14:15:08#));
                    ' знак равенства не использовать
                    ' возможно здесь нужна замена точки на :$"#{pas.DateTimePickerText1.ToLongTimeString}#"
                    value1String = $"#{pas.DateTimePickerText1.ToLongTimeString}#"
                    value2String = $"#{pas.DateTimePickerText2.ToLongTimeString}#"
                    value3String = $"#{pas.DateTimePickerText3.ToLongTimeString}#"
                Case Else
                    value1String = $"'{pas.ComboBoxText1}'"
                    value2String = $"'{pas.ComboBoxText2}'"
                    value3String = $"'{pas.ComboBoxText3}'"
            End Select

            If pas.IsSetUpCondition1 Then sb.Write($"{ConditionConvert(pas.Condition1)} {value1String}")

            If pas.IsSetUpCondition2 Then
                If sb.ToString <> "" Then sb.Write(" ИЛИ ")
                sb.Write($"{ConditionConvert(pas.Condition2)} {value2String}")
            End If

            If pas.IsSetUpCondition3 Then
                If sb.ToString <> "" Then sb.Write(" ИЛИ ")
                sb.Write($"{ConditionConvert(pas.Condition3)} {value3String}")
            End If

            If sb.ToString <> "" Then
                Return sb.ToString()
            Else
                Return DEFAULT_STRING
            End If
        End If

        'Return String.Format(DEFAULT_FORMAT_STRING, pas.Series, pas.Number, pas.IssueDate.ToString("d"))
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function
End Class
#End Region