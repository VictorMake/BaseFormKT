Public Class DoubleTextBox
    Inherits RegExTextBox

    ''' <summary>
    ''' Установить по умолчанию ValidationExpression. Tаким образом
    ''' этот контрол будет проверять содержимое TextBox в соответствии с шаблоном:
    ''' "^\s*-?\d+[\.|\,]?\d+\s*$" 'decimal 
    ''' "^\s*\d+[\.]?\d*?\s*$" 'decimal полож
    ''' </summary>
    Private Sub SetValidation()
        Me.ValidationExpression = "^\s*\d+[\.]?\d*?\s*$"
        Me.ErrorMessage = "Значение поля должно быть цифровым!"
    End Sub
End Class
