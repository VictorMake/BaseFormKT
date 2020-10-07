Imports System.Drawing

Module ModuleGlobal
    ' почему-то нельзя использовать глобальные классы форм вне контекста frmBase т.к. создаются какие-то теневые экземпляры
    ' здесь использовать только константы

    Public Const PARAMETER_IS_NOTHING As String = "Отсутствует"
    Public Const VACUUM As String = "Разрежение"
    Public Const PRESSURE As String = "Давление"
    Public Const PROVIDER_JET As String = "Provider=Microsoft.Jet.OLEDB.4.0;"
    Public Const con9999999 As Integer = 9999999
    ''' <summary>
    ''' количество знаков после точки
    ''' </summary>
    Public Const Precision As Integer = 2
    ''' <summary>
    ''' ColumnIndex_ИспользоватьКонстанту
    ''' </summary>
    Public Const ColumnIndex_UseConstant As Integer = 3
    ''' <summary>
    ''' ColumnIndex_ИмяБазовогоПараметра
    ''' </summary>
    Public Const ColumnIndex_NameBaseParameter As Integer = 6

    Public IsCalculatingKT As Boolean
    Public ColorsNet(7) As Color
    Public ColorsCaptionGrid(8) As Color

    ''' <summary>
    ''' Расположение
    ''' </summary>
    Public Enum Disposition
        Left = 1
        Right = 2
        None = 3
    End Enum

    ''' <summary>
    ''' Тип Узла
    ''' </summary>
    Public Enum StageNodeType
        ТипыИзделия1 = 1
        НомерИзделия2 = 2
        НомерСборки3 = 3
        НомерПостановки4 = 4
        НомерЗапуска5 = 5
        НомерКТ6 = 6
        НетПотомков = 7
    End Enum

    Public Enum StageGridType As Integer
        ТипыИзделия1 = 1
        НомерИзделия2 = 2
        НомерСборки3 = 3
        НомерПостановки4 = 4
        НомерЗапуска5 = 5
        НомерКТ6 = 6
        Измеренные = 7
        Приведенные = 8
        Пересчитанные = 9
    End Enum

    Public Const TypeEngine As String = "ТипИзделия"
    Public Const NumberEngine As String = "НомерИзделия"
    Public Const NumberBuild As String = "НомерСборки"
    Public Const NumberStage As String = "НомерПостановки"
    Public Const NumberStarting As String = "НомерЗапуска"
    Public Const NumberKT As String = "НомерКТ"
    Public Const MeasurementParameters As String = "ИзмеренныеПараметры"
    Public Const CastParameters As String = "ПриведенныеПараметры"
    Public Const ConvertingParameters As String = "ПересчитанныеПараметры"

    Public Const cTypeEngine As String = "Типы изделия"
    Public Const cNumberEngine As String = "Номера изделиий"
    Public Const cNumberBuild As String = "Номера сборок"
    Public Const cNumberStage As String = "Номера постановок"
    Public Const cNumberStarting As String = "Номера запусков"
    Public Const cNumberKT As String = "Контрольные точки"

    Public Const cType_Engine As String = "Тип_Изделия"
    Public Const cNumber_Engine As String = "Номер_Изделия"
    Public Const cNumber_Build As String = "Номер_Сборки"
    Public Const cNumber_Stage As String = "Номер_Постановки"
    Public Const cNumber_Starting As String = "Номер_Запуска"
    Public Const cNumber_KT As String = "Номер_КТ"

    Public Const cKey As String = "key"

    Public Function BuildCnnStr(ByVal provider As String, ByVal dataBase As String) As String
        'Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Registry Path=;Jet OLEDB:Database Locking Mode=1;Data Source="D:\ПрограммыVBNET\RUD\RUD.NET\bin\Ресурсы\Channels.mdb";Jet OLEDB:Engine Type=5;Provider="Microsoft.Jet.OLEDB.4.0";Jet OLEDB:System database=;Jet OLEDB:SFP=False;persist security info=False;Extended Properties=;Mode=Share Deny None;Jet OLEDB:Encrypt Database=False;Jet OLEDB:Create System Database=False;Jet OLEDB:Don't Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;User ID=Admin;Jet OLEDB:Global Bulk Transactions=1
        'Public Const strProviderJet As String = "Provider=Microsoft.Jet.OLEDB.4.0;"
        Return $"{provider}Data Source={dataBase};"
    End Function

    Public Function ConditionConvert(ByVal textCondition As String) As String
        Dim matchCondition As String = String.Empty

        Select Case textCondition
            Case "равно"
                matchCondition = "="
            Case "не равно"
                matchCondition = "<>"
            Case "больше"
                matchCondition = ">"
            Case "больше или равно"
                matchCondition = ">="
            Case "меньше"
                matchCondition = "<"
            Case "меньше или равно"
                matchCondition = "<="
        End Select

        Return matchCondition
    End Function
End Module
