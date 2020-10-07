Public Class IntegerTextBox
    Inherits RegExTextBox

    ''' <summary>
    ''' Установить по умолчанию ValidationExpression. Tаким образом
    ''' этот контрол будет проверять содержимое TextBox в соответствии с шаблоном:
    ''' ^\s*[+-]?\d+\s*$" 'integer
    ''' "^\s*\d+\s*$"  'integer полож
    ''' </summary>
    Private Sub SetValidation()
        Me.ValidationExpression = "^\s*\d+\s*$"
        Me.ErrorMessage = "Значение поля должно быть цифровым!"
    End Sub
End Class
