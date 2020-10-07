Public Interface IClassCalculation
    Event DataError(ByVal sender As IClassCalculation, ByVal e As DataErrorEventArgs)

    ''' <summary>
    ''' Пользовательское событие унаследованное от EventArgs
    ''' </summary>
    ''' <remarks></remarks>
    Class DataErrorEventArgs
        Inherits EventArgs

        Public Sub New(ByVal message As String, ByVal description As String)
            Me.Message = message
            Me.Description = description
        End Sub

        ' Это событие имеет 2 члена -- 
        ''' <summary>
        ''' 1) что за событие
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Message As String
        ''' <summary>
        ''' 2) какова его сущность.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Description As String
    End Class

    ''' <summary>
    ''' Расчёт выходных параметров.
    ''' </summary>
    ''' <remarks></remarks>
    Sub Calculate()
    ''' <summary>
    ''' Пересчёт КТ
    ''' </summary>
    Sub RecalculationKT()
    Property Manager() As ProjectManager
End Interface


