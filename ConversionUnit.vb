'<System.Runtime.InteropServices.ProgId("ClassПереводЕдиниц_NET.ClassПереводЕдиниц")> Public Class ClassПереводЕдиниц
'    Implements IClassПереводЕдиниц
Public NotInheritable Class ConversionUnit
    Private Const Kelvin_273_15 As Double = 273.15 'абс. ноль
    ''' <summary>
    ''' В открытом или вложенном открытом типе объявляются только статические элементы и имеется открытый или защищенный конструктор по умолчанию.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()

    End Sub

    ''' <summary>
    ''' Определить Единицу В СИ
    ''' Модифицирует значение переменной outВыходнаяЕдСи переданное по ссылке и возвращает значение True.
    ''' </summary>
    ''' <param name="inputUnit"></param>
    ''' <param name="outputUnitSI"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckUnitInSI(ByVal inputUnit As String, ByRef outputUnitSI As String) As Boolean
        ' здесь однозначный перевод в СИ
        Dim success As Boolean

        Select Case inputUnit
            'перевод тяги
            Case "нет"
                outputUnitSI = "нет"
                success = True
                Exit Select
            Case "кгс"
                outputUnitSI = "кгс"
                success = True
                'перевод расхода топлива
                Exit Select
            Case "кг/час"
                outputUnitSI = "кг/час"
                success = True
                'перевод оборотов
                Exit Select
            Case "%"
                outputUnitSI = "%"
                success = True
                'перевод удельного расхода топлива
                Exit Select
            Case "кг/кгс*час"
                outputUnitSI = "кг/кгс*час"
                success = True
                'размерность углов
                Exit Select
            Case "град (рад)"
                outputUnitSI = "град (рад)"
                success = True

                Exit Select
            Case "Па"
                'паскаль переводить не надо, копируем как есть
                outputUnitSI = "Па"
                success = True
                Exit Select
            Case "кПа"
                'перевод мм водяного столба в паскаль
                outputUnitSI = "Па"
                success = True
                Exit Select
            Case "Мпа"
                outputUnitSI = "Па"
                success = True
                Exit Select
            Case "Вольт"
                'для дискретных параметров
                outputUnitSI = "Вольт"
                success = True
                Exit Select
            Case "Деления"
                'Для Дрс, а1,а2
                outputUnitSI = "Деления"
                success = True
                Exit Select
            Case "Н/см^2"
                'паскаль переводить не надо, копируем как есть
                outputUnitSI = "Па"
                success = True
                Exit Select
            Case "дин/см^2"
                outputUnitSI = "Па"
                success = True
                Exit Select
            Case "бар"
                outputUnitSI = "Па"
                success = True
                Exit Select
            Case "кгс/м^2"
                outputUnitSI = "Па"
                success = True
                Exit Select
            Case "кгс/см^2"
                outputUnitSI = "Па"
                success = True
                Exit Select
            Case "мм.вод.ст"
                outputUnitSI = "Па"
                success = True
                Exit Select
            Case "мм.рт.ст"
                outputUnitSI = "Па"
                success = True
                Exit Select
            Case "атм"
                outputUnitSI = "Па"
                success = True
                'Перевод в систему СИ по температуре
                Exit Select
            Case "K"
                outputUnitSI = "K"
                success = True
                Exit Select
            Case "град С"
                outputUnitSI = "K"
                success = True
                Exit Select
            Case "мм"
                outputUnitSI = "мм"
                success = True
                Exit Select

            Case Else 'нет такой входной единицы
                outputUnitSI = "Ненайдено"
                success = False
        End Select

        Return success
    End Function

    ''' <summary>
    ''' Перевести Значение В СИ
    ''' В случае успешного перевода модифицирует значение переменной outЗначениеСИ переданное по ссылке.
    ''' </summary>
    ''' <param name="inputUnit"></param>
    ''' <param name="inputValue"></param>
    ''' <param name="outputValueSI"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function СonversionValueToSI(ByVal inputUnit As String, ByVal inputValue As Double, ByRef outputValueSI As Double) As Boolean
        ' здесь однозначный перевод в СИ
        Dim success As Boolean

        Select Case inputUnit
            Case "нет"
                outputValueSI = inputValue
                success = True
                Exit Select
                'перевод тяги
            Case "кгс"
                outputValueSI = inputValue
                success = True
                'перевод расхода топлива
                Exit Select
            Case "кг/час"
                outputValueSI = inputValue
                success = True
                'перевод оборотов
                Exit Select
            Case "%"
                outputValueSI = inputValue
                success = True
                'перевод удельного расхода топлива
                Exit Select
            Case "кг/кгс*час"
                outputValueSI = inputValue
                success = True
                'размерность углов
                Exit Select
            Case "град (рад)"
                outputValueSI = inputValue
                success = True

                Exit Select
            Case "Па"
                'паскаль переводить не надо, копируем как есть
                outputValueSI = inputValue
                success = True
                Exit Select
            Case "кПа"
                'перевод мм водяного столба в паскаль
                outputValueSI = inputValue * 1000
                success = True
                Exit Select
            Case "Мпа"
                outputValueSI = inputValue * 1000000
                success = True
                Exit Select
            Case "Вольт"
                'для дискретных параметров
                outputValueSI = inputValue
                success = True
                Exit Select
            Case "Деления"
                'Для Дрс, а1,а2
                outputValueSI = inputValue
                success = True
                Exit Select
            Case "Н/см^2"
                'паскаль переводить не надо, копируем как есть
                outputValueSI = inputValue * 10000
                success = True
                Exit Select
            Case "дин/см^2"
                outputValueSI = inputValue * 0.1
                success = True
                Exit Select
            Case "бар"
                outputValueSI = inputValue * 100000
                success = True
                Exit Select
            Case "кгс/м^2"
                outputValueSI = inputValue * 9.806614
                success = True
                Exit Select
            Case "кгс/см^2"
                outputValueSI = inputValue * 98066.14
                success = True
                Exit Select
            Case "мм.вод.ст"
                outputValueSI = inputValue * 9.806614
                success = True
                Exit Select
            Case "мм.рт.ст"
                outputValueSI = inputValue * 133.3223
                success = True
                Exit Select
            Case "атм"
                outputValueSI = inputValue * 101325
                success = True
                'Перевод в систему СИ по температуре
                Exit Select
            Case "K"
                outputValueSI = inputValue
                success = True
                Exit Select
            Case "град С"
                outputValueSI = inputValue + Kelvin_273_15
                success = True
                Exit Select
            Case "мм"
                outputValueSI = inputValue
                success = True
                Exit Select
            Case Else 'нет такой входной единицы
                outputValueSI = 1024
                success = False
        End Select

        Return success
    End Function

    ''' <summary>
    ''' Перевести Значение В Настроечные
    ''' В случае успешного перевода модифицирует значение переменной outВыхНастроечноеЗначение переданное по ссылке
    ''' и возвращает значение True.
    ''' </summary>
    ''' <param name="inputUnitSI"></param>
    ''' <param name="inputValueSI"></param>
    ''' <param name="outputTuningUnit"></param>
    ''' <param name="outputTuningValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function СonversionSIValueSIToTuningValue(ByVal inputUnitSI As String,
                                                            ByVal inputValueSI As Double,
                                                            ByVal outputTuningUnit As String,
                                                            ByRef outputTuningValue As Double) As Boolean ' Implements IClassПереводЕдиниц.funПеревестиВНастроечные

        ' здесь многовариантный перевод из СИ в другие системы исчесления
        Dim success As Boolean

        Select Case inputUnitSI
            Case "нет"

                Select Case outputTuningUnit
                    Case "нет"
                        outputTuningValue = inputValueSI
                        success = True
                        Exit Select
                    Case Else
                        outputTuningValue = 1037
                        success = False
                End Select

                Exit Select
            Case "Па"
                Select Case outputTuningUnit
                    Case "Па"
                        outputTuningValue = inputValueSI
                        success = True
                        Exit Select
                    Case "кПа"
                        outputTuningValue = inputValueSI / 1000
                        success = True
                        Exit Select
                    Case "Мпа"
                        outputTuningValue = inputValueSI / 1000000
                        success = True
                        Exit Select
                    Case "Н/см^2"
                        outputTuningValue = inputValueSI / 10000
                        success = True
                        Exit Select
                    Case "дин/см^2"
                        outputTuningValue = inputValueSI / 0.1
                        success = True
                        Exit Select
                    Case "бар"
                        outputTuningValue = inputValueSI / 100000
                        success = True
                        Exit Select
                    Case "кгс/м^2"
                        outputTuningValue = inputValueSI / 9.806614
                        success = True
                        Exit Select
                    Case "кгс/см^2"
                        outputTuningValue = inputValueSI / 98066.14
                        success = True
                        Exit Select
                    Case "мм.вод.ст"
                        outputTuningValue = inputValueSI / 9.806614
                        success = True
                        Exit Select
                    Case "мм.рт.ст"
                        outputTuningValue = inputValueSI / 133.3223
                        success = True
                        Exit Select
                    Case "атм"
                        outputTuningValue = inputValueSI / 101325
                        success = True

                        Exit Select
                    Case Else
                        outputTuningValue = 1026
                        success = False
                End Select

                Exit Select
            Case "K"
                Select Case outputTuningUnit
                    Case "K"
                        outputTuningValue = inputValueSI
                        success = True
                        Exit Select
                    Case "град С"
                        outputTuningValue = inputValueSI - Kelvin_273_15
                        success = True
                        Exit Select
                    Case Else
                        outputTuningValue = 1027
                        success = False
                End Select


                Exit Select
            Case "Вольт"
                ' для дискретных параметров
                Select Case outputTuningUnit
                    Case "Вольт"
                        outputTuningValue = inputValueSI
                        success = True
                        Exit Select
                    Case Else
                        outputTuningValue = 1029
                        success = False
                End Select
                Exit Select
            Case "Деления"
                ' Для Дрс, а1,а2
                Select Case outputTuningUnit
                    Case "Деления"
                        outputTuningValue = inputValueSI
                        success = True
                        Exit Select
                    Case "град (рад)"
                        outputTuningValue = inputValueSI
                        success = True
                        Exit Select
                    Case "мм"
                        outputTuningValue = inputValueSI
                        success = True

                    Case Else
                        outputTuningValue = 1030
                        success = False
                End Select

                Exit Select
            Case "кгс"
                ' Для тяги
                Select Case outputTuningUnit
                    Case "кгс"
                        outputTuningValue = inputValueSI
                        success = True
                        Exit Select
                    Case Else
                        outputTuningValue = 1031
                        success = False
                End Select

                Exit Select
            Case "кг/час"
                ' Для расхода топлива
                Select Case outputTuningUnit
                    Case "кг/час"
                        outputTuningValue = inputValueSI
                        success = True
                        Exit Select
                    Case Else
                        outputTuningValue = 1032
                        success = False
                End Select

                Exit Select
            Case "%"
                ' Для оборотов
                Select Case outputTuningUnit
                    Case "%"
                        outputTuningValue = inputValueSI
                        success = True
                        Exit Select
                    Case Else
                        outputTuningValue = 1033
                        success = False
                End Select

                Exit Select
            Case "кг/кгс*час"
                ' Для удельного расхода топлива
                Select Case outputTuningUnit
                    Case "кг/кгс*час"
                        outputTuningValue = inputValueSI
                        success = True
                        Exit Select
                    Case Else
                        outputTuningValue = 1034
                        success = False
                End Select

                Exit Select
            Case "град (рад)"
                Select Case outputTuningUnit
                    Case "град (рад)"
                        outputTuningValue = inputValueSI
                        success = True
                        Exit Select
                    Case Else
                        outputTuningValue = 1035
                        success = False
                End Select


                Exit Select
            Case "кг/с"
                ' Для расхода воздуха
                Select Case outputTuningUnit
                    Case "кг/с"
                        outputTuningValue = inputValueSI
                        success = True
                        Exit Select
                    Case Else
                        outputTuningValue = 1036
                        success = False
                End Select

                Exit Select

            Case "мм"
                ' Для Дрс
                Select Case outputTuningUnit
                    Case "мм"
                        outputTuningValue = inputValueSI
                        success = True
                        Exit Select
                    Case Else
                        outputTuningValue = 1038
                        success = False
                End Select

                Exit Select
            Case Else ' нет такой входной единицы
                outputTuningValue = 1025
                success = False
        End Select

        Return success
    End Function

End Class