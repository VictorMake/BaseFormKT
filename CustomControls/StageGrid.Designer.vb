<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StageGrid
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me._dgv = New System.Windows.Forms.DataGridView
        Me.FieldColumn = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Stage = New BaseFormKT.DataGridStageColumn
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridStageColumn1 = New BaseFormKT.DataGridStageColumn
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.lblCaption = New System.Windows.Forms.Label
        CType(Me._dgv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_dgv
        '
        Me._dgv.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me._dgv.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.FieldColumn, Me.Stage})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me._dgv.DefaultCellStyle = DataGridViewCellStyle1
        Me._dgv.Dock = System.Windows.Forms.DockStyle.Fill
        Me._dgv.Location = New System.Drawing.Point(14, 26)
        Me._dgv.Name = "_dgv"
        Me._dgv.Size = New System.Drawing.Size(966, 174)
        Me._dgv.TabIndex = 6
        '
        'FieldColumn
        '
        Me.FieldColumn.HeaderText = "Поле"
        Me.FieldColumn.Name = "FieldColumn"
        Me.FieldColumn.Width = 150
        '
        'Stage
        '
        Me.Stage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.Stage.HeaderText = "Фильтр"
        Me.Stage.Name = "Stage"
        Me.Stage.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Имя"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 150
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Фамилия"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 250
        '
        'DataGridStageColumn1
        '
        Me.DataGridStageColumn1.HeaderText = "Паспортные данные"
        Me.DataGridStageColumn1.Name = "DataGridStageColumn1"
        Me.DataGridStageColumn1.Width = 144
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.LightSteelBlue
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Left
        Me.TextBox1.Location = New System.Drawing.Point(0, 26)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(14, 174)
        Me.TextBox1.TabIndex = 15
        Me.TextBox1.Text = "Условие^AND"
        '
        'lblCaption
        '
        Me.lblCaption.BackColor = System.Drawing.Color.LightSteelBlue
        Me.lblCaption.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCaption.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblCaption.Location = New System.Drawing.Point(0, 0)
        Me.lblCaption.Name = "lblCaption"
        Me.lblCaption.Size = New System.Drawing.Size(980, 26)
        Me.lblCaption.TabIndex = 14
        Me.lblCaption.Text = "Заголовок типа этапа где натраивается фильтр"
        Me.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'StageGrid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Controls.Add(Me._dgv)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.lblCaption)
        Me.Name = "StageGrid"
        Me.Size = New System.Drawing.Size(980, 200)
        CType(Me._dgv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents _dgv As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridStageColumn1 As DataGridStageColumn
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents lblCaption As System.Windows.Forms.Label
    Friend WithEvents FieldColumn As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Stage As BaseFormKT.DataGridStageColumn

End Class
