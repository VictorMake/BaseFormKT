Friend Class DirectoryNode
    Inherits Windows.Forms.TreeNode

    Public Property NodeType() As StageNodeType
    Public Property KeyId() As Integer
    Public Property SubDirectoriesAdded() As Boolean

    Public Sub New(ByVal [text] As String, ByVal NodeType As StageNodeType, ByVal KeyId As Integer)
        MyBase.New([text])
        Me.NodeType = NodeType
        Me.KeyId = KeyId
    End Sub
End Class

