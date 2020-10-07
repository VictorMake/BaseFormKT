Imports System.Text.RegularExpressions
Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' Расширение TextBox с настраиваемым регулярноым выражением.
''' </summary>
''' <remarks></remarks>
Public Class RegExTextBox
    Inherits TextBox

    ''' <summary>
    ''' Сообщение которое должно быть использовано если текст не соответствует шаблону.
    ''' Разрешено разработчику установить сообщение ошибки в дизайнере или во время выполненеия.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ErrorMessage() As String

    ''' <summary>
    ''' Цвет текста в TextBox при несоответствии RegEx.
    ''' По умолчанию цвет для использования в TextBox установлен как неправильный.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ErrorColor() As Color = Color.Red

    ''' <summary>
    ''' Позволить разработчику определить текст в TextBox при соответствии регулярного выражения.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsValid() As Boolean
        Get
            If Not mValidationExpression Is Nothing Then
                Return mValidationExpression.IsMatch(Text)
            Else
                Return True
            End If
        End Get
    End Property

    ''' <summary>
    ''' Позволить разработчику специализировать регулярное выражение
    ''' как строку, которая будет использованна для проверки текста в TextBox.
    ''' Важно, что свойство устанавливается как строка (против  RegEx object),
    ''' так что разработчик может специфицировать RegEx шаблон используя окно свойств в дизайнгере.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ValidationExpression() As String
        Get
            Return validationPattern
        End Get
        Set(ByVal Value As String)
            mValidationExpression = New Regex(Value)
            validationPattern = Value
        End Set
    End Property

    ''' <summary>
    ''' Если текст не прошел проверку RegEx, тогда изменить цвет на цвет ошибки.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnValidated(ByVal e As EventArgs)
        If Not IsValid Then
            ForeColor = ErrorColor
        Else
            ForeColor = DefaultForeColor
        End If

        ' В любом случае для наследуемого контрола переопределяющего одно из
        ' On... subs подпрограмм, важно вызвать On... метода
        ' в базовом классе или контроле для необходимого последующего вызова события.
        MyBase.OnValidated(e)
    End Sub

    ' Строка представляющая RegEx, которое будет использовано при проверке текста TextBox. 
    ' Защищённость строки необходима для использования в режиме дизайна.
    Protected validationPattern As String

    ' RegEx object используемый для проверки 
    Protected mValidationExpression As Regex

End Class