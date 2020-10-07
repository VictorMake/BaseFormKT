Public Class Field
    Public Property ControlType() As ControlStageType
    Public Property NameField() As String
    Public ReadOnly Property Values() As List(Of String)

    Public Sub New(ByVal inNameField As String, ByVal inControlType As ControlStageType, ByVal inValueList As List(Of String))
        NameField = inNameField
        ControlType = inControlType
        Values = inValueList
    End Sub

    Public Overrides Function ToString() As String
        Return NameField
    End Function
End Class
