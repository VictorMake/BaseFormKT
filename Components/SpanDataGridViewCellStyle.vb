Imports System.Drawing
Imports System.Windows.Forms

Public Class SpanDataGridViewCellStyle
    Public Property TypeDataGridViewCellStyle As DataGridViewCellStyle
    Public Property ColumnSpan As Integer
    Public Property RowSpan As Integer
    Public Property IsReadOnly As Boolean = True

    Public Sub New()
        Me.New(1, 1)
    End Sub

    Public Sub New(inColumnSpan As Integer, inRowSpan As Integer)
        ColumnSpan = inColumnSpan
        RowSpan = inRowSpan

        TypeDataGridViewCellStyle = New DataGridViewCellStyle With {
        .Alignment = DataGridViewContentAlignment.MiddleCenter,' Возвращает или задает значение, указывающее местоположение содержимого ячейки в пределах ячейки DataGridView. 
        .Font = TextFont, ' Получает или задает шрифт, применимый к текстовому содержимому ячейки DataGridView. 
        .ForeColor = Color.Black, ' Получает или задает основной цвет ячейки DataGridView. 
        .BackColor = Color.WhiteSmoke, ' Получает или задает цвет фона ячейки DataGridView. 
        .SelectionForeColor = Color.Black, ' Получает или задает основной цвет, используемый ячейкой DataGridView, когда она выбрана. 
        .SelectionBackColor = Color.White,' Получает или задает цвет фона, используемый ячейкой DataGridView, когда она выбрана. 
        .WrapMode = DataGridViewTriState.False ' Получает или задает значение, показывающее, переносится ли текстовое содержимое ячейки DataGridView на последующие строки или обрезается, когда оно слишком длинное и не помещается на одной строке. 
        }
        '.Format = "F3", ' Получает или задает строку формата, применимую к текстовому содержимому ячейки DataGridView. 
        '.DataSourceNullValue ' Получает или задает значение, сохраняемое в источнике данных, когда пользователь вводит в ячейку значение null. 
        '.FormatProvider ' Получает или задает объект, используемый для обеспечения форматирования значений ячеек DataGridView в соответствии с языком и региональными параметрами. 
        '.IsDataSourceNullValueDefault ' Получает значение, показывающее, было ли установлено свойство DataSourceNullValue. 
        '.IsFormatProviderDefault ' Получает значение, показывающее, было ли установлено свойство FormatProvider. 
        '.IsNullValueDefault ' Получает значение, показывающее, было ли установлено свойство NullValue. 
        '.NullValue ' Получает или задает отображаемое значение ячейки DataGridView, соответствующее значению ячейки DBNull.Value или значениеNothing. 
        '.Padding ' Получает или задает расстояние между краем ячейки DataGridViewCell и ее содержимым. 
        '.Tag ' Получает или задает объект, содержащий дополнительные данные, которые относятся к DataGridViewCellStyle. 
    End Sub

    Friend Shared TitleFont As New Font("Arial", 10, FontStyle.Bold)
    Friend Shared DigitalFont As New Font("Arial", 10, FontStyle.Italic Or FontStyle.Bold)
    Friend Shared TextFont As New Font("Arial", 10, FontStyle.Regular)
    Friend Const LimitControls As Integer = 8 ' только 8 контролов в строке
End Class

#Region "Digital"
Public Class DigitalSpanDataGridViewCellStyle
    Inherits SpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New(1, 1)

        With TypeDataGridViewCellStyle
            .Font = DigitalFont
            .Format = "F3"
        End With
    End Sub
End Class

Public Class MeasurementParameterValueSpanDataGridViewCellStyle
    Inherits DigitalSpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New

        With TypeDataGridViewCellStyle
            .BackColor = Color.LightGreen
            .SelectionForeColor = Color.Blue
            .SelectionBackColor = Color.Orange
        End With

        IsReadOnly = False
    End Sub
End Class

Public Class PhysicalCastParameterSpanDataGridViewCellStyle
    Inherits DigitalSpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New
        TypeDataGridViewCellStyle.BackColor = Color.LightSkyBlue
    End Sub
End Class

Public Class ConvertingParameterValueSpanDataGridViewCellStyle
    Inherits DigitalSpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New
        TypeDataGridViewCellStyle.BackColor = Color.LightGoldenrodYellow
    End Sub
End Class
#End Region

#Region "Title"
Public Class ColumnsRowsEmptyTitleSpanDataGridViewCellStyle
    Inherits SpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New
    End Sub

    Public Sub New(inColumnSpan As Integer, inRowSpan As Integer)
        MyBase.New(inColumnSpan, inRowSpan)
    End Sub
End Class

Public Class EmptyTitleSpanDataGridViewCellStyle
    Inherits ColumnsRowsEmptyTitleSpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New(LimitControls + 1, 1)

        With TypeDataGridViewCellStyle
            .Font = TitleFont
            .SelectionForeColor = Color.LightGray
            .BackColor = Color.White
        End With
    End Sub
End Class

Public Class MeasurementTitleSpanDataGridViewCellStyle
    Inherits EmptyTitleSpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New
        TypeDataGridViewCellStyle.BackColor = Color.Lime
    End Sub
End Class

Public Class PhysicalCastTitleSpanDataGridViewCellStyle
    Inherits EmptyTitleSpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New
        TypeDataGridViewCellStyle.BackColor = Color.Aqua
    End Sub
End Class

Public Class ConvertingTitleSpanDataGridViewCellStyle
    Inherits EmptyTitleSpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New
        TypeDataGridViewCellStyle.BackColor = Color.Orange
    End Sub
End Class
#End Region

Public Class StageNameSpanDataGridViewCellStyle
    Inherits SpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New(1, 1)

        With TypeDataGridViewCellStyle
            .SelectionBackColor = Color.LightGray
            .WrapMode = DataGridViewTriState.True
        End With
    End Sub
End Class

Public Class DescriptionStageSpanDataGridViewCellStyle
    Inherits SpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New(LimitControls, 1)

        With TypeDataGridViewCellStyle
            .SelectionBackColor = Color.LightGray
            .WrapMode = DataGridViewTriState.True
            '.Alignment = DataGridViewContentAlignment.MiddleLeft
        End With
    End Sub
End Class

#Region "ControlTex, ParameterName"
Public Class ControlTextSpanDataGridViewCellStyle
    Inherits SpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New(1, 1)

        With TypeDataGridViewCellStyle
            .BackColor = Color.LightGray
            .SelectionBackColor = Color.LightGray
            .WrapMode = DataGridViewTriState.True
        End With
    End Sub
End Class

Public Class ParameterNameSpanDataGridViewCellStyle
    Inherits ControlTextSpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New
    End Sub
End Class
#End Region

#Region "ControlValue, Caption, ParameterUnit"
Public Class ControlValueSpanDataGridViewCellStyle
    Inherits SpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New(1, 1)

        With TypeDataGridViewCellStyle
            .Alignment = DataGridViewContentAlignment.MiddleLeft
            .SelectionBackColor = Color.LightGray
            .WrapMode = DataGridViewTriState.True
        End With
    End Sub
End Class

Public Class CaptionSpanDataGridViewCellStyle
    Inherits ControlValueSpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New
        TypeDataGridViewCellStyle.BackColor = Color.LightSteelBlue
    End Sub
End Class

Public Class ParameterUnitSpanDataGridViewCellStyle
    Inherits ControlValueSpanDataGridViewCellStyle

    Public Sub New()
        MyBase.New
        TypeDataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub
End Class
#End Region



