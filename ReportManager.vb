Imports System.ComponentModel

Public Class RecordReport
    ''' <summary>
    ''' Этап
    ''' </summary>
    ''' <returns></returns>
    Public Property Stage() As String
    ''' <summary>
    ''' Номер КТ
    ''' </summary>
    ''' <returns></returns>
    Public Property NumberKT() As String
    ''' <summary>
    ''' Параметр
    ''' </summary>
    ''' <returns></returns>
    Public Property Parameter() As String
    ''' <summary>
    ''' Значение Пользователя
    ''' </summary>
    ''' <returns></returns>
    Public Property CustomerValue() As String

    Public Sub New(ByVal inStage As String, ByVal inNumberKT As String, ByVal inParameter As String, ByVal inCustomerValue As String)
        Stage = inStage
        NumberKT = inNumberKT
        Parameter = inParameter
        CustomerValue = inCustomerValue
    End Sub
End Class

''' <summary>
''' Коллекция записей типа RecordReport для источник данных BindingSource.DataSource
''' </summary>
Public Class Reports
    Inherits BindingList(Of RecordReport)

    'Private Sub Reports_AddingNew(sender As Object, e As AddingNewEventArgs) Handles Me.AddingNew
    '    e.NewObject = New Report("Этап", "НомерКТ", "Parametr", "ЗначениеПользователя")
    'End Sub
End Class

''' <summary>
''' Прокси класс для доступа к общиму свойству - коллекции привязки данных
''' </summary>
Public Class ReportManager
    Private Shared ReportsValue As New Reports
    ''' <summary>
    ''' коллекция привязки данных
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property ListReports() As Reports
        Get
            Return ReportsValue
        End Get
        Set(ByVal value As Reports)
            ReportsValue = value
        End Set
    End Property

    ' Тест
    'Public Shared Function GetAllReports() As Reports
    '    Dim ReportList As New Reports()
    '    'Этап	НомерКТ	Parametr	ЗначениеПользователя
    '    ReportList.Add(New Report("1ТипИзделия", "0", "НомКТПар2	", "2"))
    '    ReportList.Add(New Report("2НомерИзделия", "0", "НомКТПар1	", "159"))
    '    ReportList.Add(New Report("3НомерСборки", "0", "НомКТПар21	", "15:58:25"))
    '    ReportList.Add(New Report("4НомерПостановки", "0", "НомКТПар31	", "True"))
    '    ReportList.Add(New Report("5НомерЗапуска", "0", "НомКТПар42	", "0"))
    '    ReportList.Add(New Report("6НомерКТ", "0", "НомКТПар51	", "2"))
    '    ReportList.Add(New Report("ИзмеренныеПараметры", "1", "НомКТПар61	", "1.233"))
    '    ReportList.Add(New Report("ИзмеренныеПараметры", "2", "НомКТПар61	", "1.726"))
    '    ReportList.Add(New Report("ИзмеренныеПараметры", "3", "НомКТПар61	", "2.21899999999999"))
    '    ReportList.Add(New Report("ПриведенныеПараметры", "1", "НомКТПар71	", "1.403"))
    '    ReportList.Add(New Report("ПриведенныеПараметры", "2", "НомКТПар71	", "1.896"))
    '    ReportList.Add(New Report("ПриведенныеПараметры", "3", "НомКТПар71	", "2.38899999999999"))
    '    ReportList.Add(New Report("ПересчитанныеПараметры", "1", "НомКТПар81	", "1.573"))
    '    ReportList.Add(New Report("ПересчитанныеПараметры", "2", "НомКТПар81	", "2.06599999999999"))
    '    ReportList.Add(New Report("ПересчитанныеПараметры", "3", "НомКТПар81	", "2.55899999999999"))
    '    Return ReportList
    'End Function
End Class
