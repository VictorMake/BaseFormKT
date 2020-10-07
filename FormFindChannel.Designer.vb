<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormFindChannel
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormFindChannel))
        Me.OK_Button = New System.Windows.Forms.Button
        Me.TextBoxFilter = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblTarRowPosition = New System.Windows.Forms.Label
        Me.grdFindChannel = New System.Windows.Forms.DataGridView
        Me.LabelCaptionGrid = New System.Windows.Forms.Label
        CType(Me.grdFindChannel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(155, 538)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'txtFilter
        '
        Me.TextBoxFilter.Location = New System.Drawing.Point(138, 11)
        Me.TextBoxFilter.Name = "txtFilter"
        Me.TextBoxFilter.Size = New System.Drawing.Size(84, 20)
        Me.TextBoxFilter.TabIndex = 10
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label1.Location = New System.Drawing.Point(9, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Предполагаемое имя:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTarRowPosition
        '
        Me.lblTarRowPosition.AutoSize = True
        Me.lblTarRowPosition.Location = New System.Drawing.Point(9, 538)
        Me.lblTarRowPosition.Name = "lblTarRowPosition"
        Me.lblTarRowPosition.Size = New System.Drawing.Size(0, 13)
        Me.lblTarRowPosition.TabIndex = 12
        '
        'grdFindChannel
        '
        Me.grdFindChannel.AllowUserToAddRows = False
        Me.grdFindChannel.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Teal
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.PaleGreen
        Me.grdFindChannel.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.grdFindChannel.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.grdFindChannel.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.grdFindChannel.BackgroundColor = System.Drawing.Color.Lavender
        Me.grdFindChannel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.grdFindChannel.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.DarkBlue
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.PaleGreen
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Teal
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.grdFindChannel.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.grdFindChannel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Teal
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.PaleGreen
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.grdFindChannel.DefaultCellStyle = DataGridViewCellStyle3
        Me.grdFindChannel.GridColor = System.Drawing.Color.RoyalBlue
        Me.grdFindChannel.Location = New System.Drawing.Point(12, 59)
        Me.grdFindChannel.MultiSelect = False
        Me.grdFindChannel.Name = "grdFindChannel"
        Me.grdFindChannel.ReadOnly = True
        Me.grdFindChannel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.grdFindChannel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.grdFindChannel.Size = New System.Drawing.Size(210, 469)
        Me.grdFindChannel.TabIndex = 20
        '
        'LabelCaptionGrid
        '
        Me.LabelCaptionGrid.BackColor = System.Drawing.Color.RoyalBlue
        Me.LabelCaptionGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelCaptionGrid.ForeColor = System.Drawing.Color.White
        Me.LabelCaptionGrid.Location = New System.Drawing.Point(12, 38)
        Me.LabelCaptionGrid.Name = "LabelCaptionGrid"
        Me.LabelCaptionGrid.Size = New System.Drawing.Size(210, 22)
        Me.LabelCaptionGrid.TabIndex = 21
        Me.LabelCaptionGrid.Text = "Значения каналов"
        Me.LabelCaptionGrid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmFindChannel
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(233, 568)
        Me.Controls.Add(Me.LabelCaptionGrid)
        Me.Controls.Add(Me.grdFindChannel)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.lblTarRowPosition)
        Me.Controls.Add(Me.TextBoxFilter)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFindChannel"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Значения всех каналов"
        CType(Me.grdFindChannel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents TextBoxFilter As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblTarRowPosition As System.Windows.Forms.Label
    Friend WithEvents grdFindChannel As System.Windows.Forms.DataGridView
    Friend WithEvents LabelCaptionGrid As System.Windows.Forms.Label

End Class
