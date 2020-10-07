'Imports System.ComponentModel

''' <summary>
''' Универсальная запись для в общем списке отфильтрованной выборки для всех таблиц
''' </summary>
'Public Class RecordReport
'    ''' <summary>
'    ''' Этап
'    ''' </summary>
'    ''' <returns></returns>
'    Public Property Stage() As String
'    ''' <summary>
'    ''' Номер КТ
'    ''' </summary>
'    ''' <returns></returns>
'    Public Property NumberKT() As String
'    ''' <summary>
'    ''' Параметр
'    ''' </summary>
'    ''' <returns></returns>
'    Public Property Parameter() As String
'    ''' <summary>
'    ''' Значение Пользователя
'    ''' </summary>
'    ''' <returns></returns>
'    Public Property CustomerValue() As String

'    Public Sub New(ByVal inStage As String, ByVal inNumberKT As String, ByVal inParameter As String, ByVal inCustomerValue As String)
'        Stage = inStage
'        NumberKT = inNumberKT
'        Parameter = inParameter
'        CustomerValue = inCustomerValue
'    End Sub
'End Class

'''' <summary>
'''' Коллекция записей типа RecordReport для источник данных BindingSource.DataSource
'''' </summary>
'Public Class Reports
'    Inherits BindingList(Of RecordReport)

'    'Private Sub Reports_AddingNew(sender As Object, e As AddingNewEventArgs) Handles Me.AddingNew
'    '    e.NewObject = New Report("Этап", "НомерКТ", "Parametr", "ЗначениеПользователя")
'    'End Sub
'End Class