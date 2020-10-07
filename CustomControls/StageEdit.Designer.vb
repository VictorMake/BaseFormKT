<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StageEdit
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ComboBoxSort = New System.Windows.Forms.ComboBox
        Me.LabelSort = New System.Windows.Forms.Label
        Me.LabelCondition1 = New System.Windows.Forms.Label
        Me.ComboBoxCondition1 = New System.Windows.Forms.ComboBox
        Me.LabelValue1 = New System.Windows.Forms.Label
        Me.ComboBoxValue1 = New System.Windows.Forms.ComboBox
        Me.LabelCondition2 = New System.Windows.Forms.Label
        Me.ComboBoxCondition2 = New System.Windows.Forms.ComboBox
        Me.ComboBoxValue2 = New System.Windows.Forms.ComboBox
        Me.LabelValue2 = New System.Windows.Forms.Label
        Me.ComboBoxValue3 = New System.Windows.Forms.ComboBox
        Me.LabelValue3 = New System.Windows.Forms.Label
        Me.LabelCondition3 = New System.Windows.Forms.Label
        Me.ComboBoxCondition3 = New System.Windows.Forms.ComboBox
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker
        Me.DateTimePicker3 = New System.Windows.Forms.DateTimePicker
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ComboBoxSort
        '
        Me.ComboBoxSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxSort.FormattingEnabled = True
        Me.ComboBoxSort.Items.AddRange(New Object() {"", "(отсутствует)", "по возрастанию", "по убыванию"})
        Me.ComboBoxSort.Location = New System.Drawing.Point(657, 25)
        Me.ComboBoxSort.Name = "ComboBoxSort"
        Me.ComboBoxSort.Size = New System.Drawing.Size(91, 21)
        Me.ComboBoxSort.TabIndex = 6
        '
        'LabelSort
        '
        Me.LabelSort.AutoSize = True
        Me.LabelSort.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelSort.Location = New System.Drawing.Point(654, 3)
        Me.LabelSort.Name = "LabelSort"
        Me.LabelSort.Size = New System.Drawing.Size(81, 16)
        Me.LabelSort.TabIndex = 19
        Me.LabelSort.Text = "Сортировка:"
        '
        'LabelCondition1
        '
        Me.LabelCondition1.AutoSize = True
        Me.LabelCondition1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelCondition1.Location = New System.Drawing.Point(3, 3)
        Me.LabelCondition1.Name = "LabelCondition1"
        Me.LabelCondition1.Size = New System.Drawing.Size(61, 16)
        Me.LabelCondition1.TabIndex = 21
        Me.LabelCondition1.Text = "Условие:"
        '
        'ComboBoxCondition1
        '
        Me.ComboBoxCondition1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCondition1.FormattingEnabled = True
        Me.ComboBoxCondition1.Items.AddRange(New Object() {"", "равно", "не равно", "больше", "больше или равно", "меньше", "меньше или равно"})
        Me.ComboBoxCondition1.Location = New System.Drawing.Point(3, 25)
        Me.ComboBoxCondition1.Name = "ComboBoxCondition1"
        Me.ComboBoxCondition1.Size = New System.Drawing.Size(103, 21)
        Me.ComboBoxCondition1.TabIndex = 0
        '
        'LabelValue1
        '
        Me.LabelValue1.AutoSize = True
        Me.LabelValue1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelValue1.Location = New System.Drawing.Point(109, 3)
        Me.LabelValue1.Name = "LabelValue1"
        Me.LabelValue1.Size = New System.Drawing.Size(69, 16)
        Me.LabelValue1.TabIndex = 22
        Me.LabelValue1.Text = "Значение:"
        '
        'ComboBoxValue1
        '
        Me.ComboBoxValue1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxValue1.FormattingEnabled = True
        Me.ComboBoxValue1.Location = New System.Drawing.Point(112, 25)
        Me.ComboBoxValue1.Name = "ComboBoxValue1"
        Me.ComboBoxValue1.Size = New System.Drawing.Size(103, 21)
        Me.ComboBoxValue1.TabIndex = 1
        '
        'LabelCondition2
        '
        Me.LabelCondition2.AutoSize = True
        Me.LabelCondition2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelCondition2.Location = New System.Drawing.Point(218, 3)
        Me.LabelCondition2.Name = "LabelCondition2"
        Me.LabelCondition2.Size = New System.Drawing.Size(86, 16)
        Me.LabelCondition2.TabIndex = 25
        Me.LabelCondition2.Text = "Условие или:"
        '
        'ComboBoxCondition2
        '
        Me.ComboBoxCondition2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCondition2.FormattingEnabled = True
        Me.ComboBoxCondition2.Items.AddRange(New Object() {"", "равно", "не равно", "больше", "больше или равно", "меньше", "меньше или равно"})
        Me.ComboBoxCondition2.Location = New System.Drawing.Point(221, 25)
        Me.ComboBoxCondition2.Name = "ComboBoxCondition2"
        Me.ComboBoxCondition2.Size = New System.Drawing.Size(103, 21)
        Me.ComboBoxCondition2.TabIndex = 2
        '
        'ComboBoxValue2
        '
        Me.ComboBoxValue2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxValue2.FormattingEnabled = True
        Me.ComboBoxValue2.Location = New System.Drawing.Point(330, 25)
        Me.ComboBoxValue2.Name = "ComboBoxValue2"
        Me.ComboBoxValue2.Size = New System.Drawing.Size(103, 21)
        Me.ComboBoxValue2.TabIndex = 3
        '
        'LabelValue2
        '
        Me.LabelValue2.AutoSize = True
        Me.LabelValue2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelValue2.Location = New System.Drawing.Point(327, 3)
        Me.LabelValue2.Name = "LabelValue2"
        Me.LabelValue2.Size = New System.Drawing.Size(69, 16)
        Me.LabelValue2.TabIndex = 26
        Me.LabelValue2.Text = "Значение:"
        '
        'ComboBoxValue3
        '
        Me.ComboBoxValue3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxValue3.FormattingEnabled = True
        Me.ComboBoxValue3.Location = New System.Drawing.Point(548, 25)
        Me.ComboBoxValue3.Name = "ComboBoxValue3"
        Me.ComboBoxValue3.Size = New System.Drawing.Size(103, 21)
        Me.ComboBoxValue3.TabIndex = 5
        '
        'LabelValue3
        '
        Me.LabelValue3.AutoSize = True
        Me.LabelValue3.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelValue3.Location = New System.Drawing.Point(545, 3)
        Me.LabelValue3.Name = "LabelValue3"
        Me.LabelValue3.Size = New System.Drawing.Size(69, 16)
        Me.LabelValue3.TabIndex = 30
        Me.LabelValue3.Text = "Значение:"
        '
        'LabelCondition3
        '
        Me.LabelCondition3.AutoSize = True
        Me.LabelCondition3.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelCondition3.Location = New System.Drawing.Point(436, 3)
        Me.LabelCondition3.Name = "LabelCondition3"
        Me.LabelCondition3.Size = New System.Drawing.Size(86, 16)
        Me.LabelCondition3.TabIndex = 29
        Me.LabelCondition3.Text = "Условие или:"
        '
        'ComboBoxCondition3
        '
        Me.ComboBoxCondition3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCondition3.FormattingEnabled = True
        Me.ComboBoxCondition3.Items.AddRange(New Object() {"", "равно", "не равно", "больше", "больше или равно", "меньше", "меньше или равно"})
        Me.ComboBoxCondition3.Location = New System.Drawing.Point(439, 25)
        Me.ComboBoxCondition3.Name = "ComboBoxCondition3"
        Me.ComboBoxCondition3.Size = New System.Drawing.Size(103, 21)
        Me.ComboBoxCondition3.TabIndex = 4
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker1.Location = New System.Drawing.Point(112, 26)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(103, 20)
        Me.DateTimePicker1.TabIndex = 31
        Me.DateTimePicker1.Visible = False
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker2.Location = New System.Drawing.Point(330, 26)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(103, 20)
        Me.DateTimePicker2.TabIndex = 32
        Me.DateTimePicker2.Visible = False
        '
        'DateTimePicker3
        '
        Me.DateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker3.Location = New System.Drawing.Point(548, 26)
        Me.DateTimePicker3.Name = "DateTimePicker3"
        Me.DateTimePicker3.Size = New System.Drawing.Size(103, 20)
        Me.DateTimePicker3.TabIndex = 33
        Me.DateTimePicker3.Visible = False
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'StageEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.DateTimePicker3)
        Me.Controls.Add(Me.DateTimePicker2)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Controls.Add(Me.ComboBoxValue3)
        Me.Controls.Add(Me.LabelValue3)
        Me.Controls.Add(Me.LabelCondition3)
        Me.Controls.Add(Me.ComboBoxCondition3)
        Me.Controls.Add(Me.ComboBoxValue2)
        Me.Controls.Add(Me.LabelValue2)
        Me.Controls.Add(Me.LabelCondition2)
        Me.Controls.Add(Me.ComboBoxCondition2)
        Me.Controls.Add(Me.ComboBoxValue1)
        Me.Controls.Add(Me.LabelValue1)
        Me.Controls.Add(Me.LabelCondition1)
        Me.Controls.Add(Me.ComboBoxCondition1)
        Me.Controls.Add(Me.LabelSort)
        Me.Controls.Add(Me.ComboBoxSort)
        Me.Name = "StageEdit"
        Me.Size = New System.Drawing.Size(766, 49)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboBoxSort As System.Windows.Forms.ComboBox
    Private WithEvents LabelSort As System.Windows.Forms.Label
    Private WithEvents LabelCondition1 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxCondition1 As System.Windows.Forms.ComboBox
    Private WithEvents LabelValue1 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxValue1 As System.Windows.Forms.ComboBox
    Private WithEvents LabelCondition2 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxCondition2 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxValue2 As System.Windows.Forms.ComboBox
    Private WithEvents LabelValue2 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxValue3 As System.Windows.Forms.ComboBox
    Private WithEvents LabelValue3 As System.Windows.Forms.Label
    Private WithEvents LabelCondition3 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxCondition3 As System.Windows.Forms.ComboBox
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimePicker2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimePicker3 As System.Windows.Forms.DateTimePicker
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider

End Class
