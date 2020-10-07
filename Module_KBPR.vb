'Imports System.Windows.Forms
'Imports System.Data.OleDb

'Module Module_KBPR

'Public Interface IclsСвойства
'    Property ИмяПараметра() As String
'    Property ГрафикПриведенный() As Boolean
'    Property ИмяПараметраИзмерения() As String
'    Property ТипДавления() As String
'    Property ДляЧегоПараметр() As String
'    Property РазмерностьВходная() As String
'    Property РазмерностьСИ() As String
'    Property РазмерностьВыходная() As String
'    Property ИмяБазовогоПараметра() As String
'    Property ИзмеренноеЗначение() As Double
'    Property ЗначениеВСИ() As Double
'    Property ЗначениеПереведенное() As Double
'    Property Row() As Integer
'    Property Col() As Integer
'    Property ЗначениеКонтрола() As String
'End Interface





'Public Structure typСоответствие
'    Dim strИмяРасчета As String
'    Dim strИмяБазы As String
'    Dim NРасчета As Integer
'    Dim NБазы As Integer
'    Dim strИмяБазовогоПараметра As String
'    Dim strОписание As String
'    'Dim sngПогрешностьТарировкиКанала As Single
'    Dim strРазмерностьВходная As String
'    Dim strРазмерностьВыходная As String
'    Dim strТипДавления As String
'End Structure
'Public arrСоответствие() As typСоответствие
'Public Structure typСвойства
'    Dim strИмяПараметра As String
'    Dim strОписание As String
'    Dim strДляЧегоПараметр As String
'    Dim strРазмерностьВходная As String
'    Dim strРазмерностьВыходная As String
'End Structure
'Public arrСвойства() As typСвойства










'Public Function funПроверкаСоответствия(ByRef cn As OleDbConnection) As Boolean
'    Dim blnНадоПерезаписать As Boolean
'    Dim I, J As Integer
'    Dim lngКоличество As Integer
'    Dim cmd As OleDbCommand = cn.CreateCommand
'    Dim strSQL As String
'    Dim blnНайден As Boolean

'    strSQL = "SELECT COUNT(*) FROM СвойстваПараметров WHERE ДляЧегоПараметр='Измерение';"
'    cmd.CommandType = CommandType.Text
'    cmd.CommandText = strSQL
'    lngКоличество = CInt(cmd.ExecuteScalar)
'    strSQL = "SELECT СоответствиеПараметров.*, СвойстваПараметров.ОписаниеПараметра, СвойстваПараметров.РазмерностьВходная, СвойстваПараметров.РазмерностьВыходная, СвойстваПараметров.ДляЧегоПараметр " & "FROM СвойстваПараметров RIGHT JOIN СоответствиеПараметров ON СвойстваПараметров.keyИмяПараметра = СоответствиеПараметров.keyИмяПараметра " & "WHERE (((СвойстваПараметров.ДляЧегоПараметр)='Измерение')) ORDER BY СоответствиеПараметров.НомерПараметраРасчета;"
'    If lngКоличество > 0 Then
'        ReDim_arrСоответствие(lngКоличество)
'        'считываем из базы и заносим из recordset в массив
'        cmd.CommandType = CommandType.Text
'        cmd.CommandText = strSQL
'        Dim rdr As OleDbDataReader = cmd.ExecuteReader
'        I = 1
'        While rdr.Read
'            'Do Until .EOF
'            arrСоответствие(I).strИмяРасчета = rdr("ИмяПараметраРасчета")
'            arrСоответствие(I).NРасчета = rdr("НомерПараметраРасчета")
'            arrСоответствие(I).strИмяБазы = rdr("ИмяПараметраИзмерения")
'            arrСоответствие(I).NБазы = rdr("НомерПараметраИзмерения")
'            If IsDBNull(rdr("ИмяБазовогоПараметра")) Then
'                arrСоответствие(I).strИмяБазовогоПараметра = "" 'vbNullString
'            Else
'                arrСоответствие(I).strИмяБазовогоПараметра = rdr("ИмяБазовогоПараметра")
'            End If
'            If IsDBNull(rdr("ОписаниеПараметра")) Then
'                arrСоответствие(I).strОписание = vbNullString
'            Else
'                arrСоответствие(I).strОписание = rdr("ОписаниеПараметра")
'            End If

'            If IsDBNull(rdr("РазмерностьВходная")) Then
'                arrСоответствие(I).strРазмерностьВходная = vbNullString
'            Else
'                arrСоответствие(I).strРазмерностьВходная = rdr("РазмерностьВходная")
'            End If
'            If IsDBNull(rdr("РазмерностьВыходная")) Then
'                arrСоответствие(I).strРазмерностьВыходная = vbNullString
'            Else
'                arrСоответствие(I).strРазмерностьВыходная = rdr("РазмерностьВыходная")
'            End If
'            If IsDBNull(rdr("ТипДавления")) Then
'                'доработка была почему-то эта строка и даже работала
'                'arrСоответствие(I).strРазмерностьВыходная = " "
'                arrСоответствие(I).strТипДавления = "" 'думаю что это правильно
'            Else
'                If rdr("ТипДавления") = vbNullString Then
'                    arrСоответствие(I).strТипДавления = ""
'                Else
'                    arrСоответствие(I).strТипДавления = rdr("ТипДавления")
'                End If
'            End If
'            I = I + 1
'        End While
'        rdr.Close()
'    End If
'    'запись погрешностей
'    'strSQL = "SELECT * FROM СоответствиеПараметров"
'    'Dim odaDataAdapter As New OleDbDataAdapter(strSQL, cn)

'    'Dim ds As New DataSet
'    'odaDataAdapter.Fill(ds)
'    'Dim tlb As DataTable = ds.Tables(0)
'    'Dim aRows As DataRow()
'    'For I = 1 To UBound(arrСоответствие)
'    '    For J = 1 To UBound(arrПараметры)
'    '        If arrПараметры(J).strНаименованиеПараметра = arrСоответствие(I).strИмяБазы Then
'    '            arrСоответствие(I).sngПогрешностьТарировкиКанала = arrПараметры(J).sngПогрешность
'    '            aRows = tlb.Select("ИмяПараметраРасчета = '" & arrСоответствие(I).strИмяРасчета & "'")
'    '            If aRows.Length > 0 Then
'    '                aRows(0)("ПогрешностьТарировкиКанала") = arrПараметры(J).sngПогрешность
'    '            End If
'    '            Exit For
'    '        End If
'    '    Next J
'    'Next I
'    'Dim myDataRowsCommandBuilder As OleDbCommandBuilder = New OleDbCommandBuilder(odaDataAdapter)
'    'odaDataAdapter.UpdateCommand = myDataRowsCommandBuilder.GetUpdateCommand
'    'odaDataAdapter.Update(ds)
'    'ds.Tables(0).AcceptChanges()
'    'odaDataAdapter.UpdateCommand.Connection.Close()

'    'проверка соответствия
'    'если номера изменились, то перепишутся новые, если параметр отсутствует, то ему присвоится conПараметрОтсутствует
'    blnНадоПерезаписать = False
'    For I = 1 To UBound(arrСоответствие)
'        blnНайден = False
'        If arrСоответствие(I).strИмяБазы <> conПараметрОтсутствует Then
'            ' проверить есть ли такой в списке замеряемых
'            For J = 1 To UBound(arrСписПарамКопия)
'                'проверить на совпадение имен и номеров
'                If arrСоответствие(I).strИмяБазы = arrПараметры(arrСписПарамКопия(J)).strНаименованиеПараметра Then
'                    arrСоответствие(I).NБазы = arrСписПарамКопия(J) 'присвоить номер т.к. параметр может быть, номер не совпадать
'                    blnНайден = True
'                    Exit For
'                End If
'            Next J
'            If Not blnНайден Then
'                arrСоответствие(I).NБазы = 0
'                arrСоответствие(I).strИмяБазы = conПараметрОтсутствует
'                blnНадоПерезаписать = True
'            End If
'        End If
'    Next I
'    Return blnНадоПерезаписать
'End Function

'Public Sub УстановкаСвойств(ByRef cn As OleDbConnection)
'    Dim I As Integer
'    Dim strSQL As String
'    Dim cmd As OleDbCommand = cn.CreateCommand
'    strSQL = "SELECT COUNT(*) FROM [СвойстваПараметров] WHERE СвойстваПараметров.ДляЧегоПараметр <> 'Измерение'"
'    cmd.CommandType = CommandType.Text
'    cmd.CommandText = strSQL
'    ReDim_arrСвойства(CInt(cmd.ExecuteScalar))

'    'считываем из базы и заносим из recordset в массив
'    strSQL = "SELECT * from [СвойстваПараметров] WHERE СвойстваПараметров.ДляЧегоПараметр <> 'Измерение'"
'    cmd.CommandType = CommandType.Text
'    cmd.CommandText = strSQL
'    Dim rdr As OleDbDataReader = cmd.ExecuteReader

'    I = 1
'    Do While rdr.Read
'        arrСвойства(I).strИмяПараметра = rdr("ИмяПараметра")
'        If IsDBNull(rdr("ОписаниеПараметра")) Then
'            arrСвойства(I).strОписание = vbNullString
'        Else
'            arrСвойства(I).strОписание = rdr("ОписаниеПараметра")
'        End If
'        arrСвойства(I).strДляЧегоПараметр = rdr("ДляЧегоПараметр")

'        If IsDBNull(rdr("РазмерностьВходная")) Then
'            arrСвойства(I).strРазмерностьВходная = vbNullString
'        Else
'            arrСвойства(I).strРазмерностьВходная = rdr("РазмерностьВходная")
'        End If
'        If IsDBNull(rdr("РазмерностьВыходная")) Then
'            arrСвойства(I).strРазмерностьВыходная = vbNullString
'        Else
'            arrСвойства(I).strРазмерностьВыходная = rdr("РазмерностьВыходная")
'        End If
'        I = I + 1
'    Loop
'    rdr.Close()
'End Sub
'End Module
