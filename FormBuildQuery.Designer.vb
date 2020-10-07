<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormBuildQuery
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormBuildQuery))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.ButtonRefreshQuery = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.ButtonSaveQuery = New System.Windows.Forms.Button
        Me.ButtonRestoreQuery = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.ButtonResetQuery = New System.Windows.Forms.Button
        Me.TableLayoutPanelStageGrid = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 6
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonRefreshQuery, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 4, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonSaveQuery, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonRestoreQuery, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 5, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonResetQuery, 2, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(6, 422)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(990, 34)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'btnRefresh
        '
        Me.ButtonRefreshQuery.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ButtonRefreshQuery.Image = CType(resources.GetObject("btnRefresh.Image"), System.Drawing.Image)
        Me.ButtonRefreshQuery.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonRefreshQuery.Location = New System.Drawing.Point(509, 3)
        Me.ButtonRefreshQuery.Name = "btnRefresh"
        Me.ButtonRefreshQuery.Size = New System.Drawing.Size(130, 28)
        Me.ButtonRefreshQuery.TabIndex = 4
        Me.ButtonRefreshQuery.Text = "Обновить"
        Me.ToolTip1.SetToolTip(Me.ButtonRefreshQuery, "повтроная загрузка таблиц после модификации базы КТ")
        Me.ButtonRefreshQuery.UseVisualStyleBackColor = True
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Image = CType(resources.GetObject("OK_Button.Image"), System.Drawing.Image)
        Me.OK_Button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.OK_Button.Location = New System.Drawing.Point(673, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(130, 28)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        Me.ToolTip1.SetToolTip(Me.OK_Button, "применить фильтр")
        '
        'ButtonЗаписатьФильтр
        '
        Me.ButtonSaveQuery.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ButtonSaveQuery.Image = CType(resources.GetObject("ButtonЗаписатьФильтр.Image"), System.Drawing.Image)
        Me.ButtonSaveQuery.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonSaveQuery.Location = New System.Drawing.Point(17, 3)
        Me.ButtonSaveQuery.Name = "ButtonЗаписатьФильтр"
        Me.ButtonSaveQuery.Size = New System.Drawing.Size(130, 28)
        Me.ButtonSaveQuery.TabIndex = 1
        Me.ButtonSaveQuery.Text = "Записать"
        Me.ToolTip1.SetToolTip(Me.ButtonSaveQuery, "записать удачную конфигурацию фильтра")
        Me.ButtonSaveQuery.UseVisualStyleBackColor = True
        '
        'ButtonВосстановитьФильтр
        '
        Me.ButtonRestoreQuery.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ButtonRestoreQuery.Image = CType(resources.GetObject("ButtonВосстановитьФильтр.Image"), System.Drawing.Image)
        Me.ButtonRestoreQuery.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonRestoreQuery.Location = New System.Drawing.Point(181, 3)
        Me.ButtonRestoreQuery.Name = "ButtonВосстановитьФильтр"
        Me.ButtonRestoreQuery.Size = New System.Drawing.Size(130, 28)
        Me.ButtonRestoreQuery.TabIndex = 2
        Me.ButtonRestoreQuery.Text = "Восстановить"
        Me.ToolTip1.SetToolTip(Me.ButtonRestoreQuery, "восстановить сохранённую конфигурацию фильтра")
        Me.ButtonRestoreQuery.UseVisualStyleBackColor = True
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Image = CType(resources.GetObject("Cancel_Button.Image"), System.Drawing.Image)
        Me.Cancel_Button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Cancel_Button.Location = New System.Drawing.Point(840, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(130, 28)
        Me.Cancel_Button.TabIndex = 5
        Me.Cancel_Button.Text = "Отмена"
        Me.ToolTip1.SetToolTip(Me.Cancel_Button, "отменить")
        '
        'ButtonСбросФильтра
        '
        Me.ButtonResetQuery.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ButtonResetQuery.Image = CType(resources.GetObject("ButtonСбросФильтра.Image"), System.Drawing.Image)
        Me.ButtonResetQuery.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonResetQuery.Location = New System.Drawing.Point(345, 3)
        Me.ButtonResetQuery.Name = "ButtonСбросФильтра"
        Me.ButtonResetQuery.Size = New System.Drawing.Size(130, 28)
        Me.ButtonResetQuery.TabIndex = 3
        Me.ButtonResetQuery.Text = "Сброс"
        Me.ToolTip1.SetToolTip(Me.ButtonResetQuery, "очистка конфигурации фильтра в памяти")
        Me.ButtonResetQuery.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelStageGrid
        '
        Me.TableLayoutPanelStageGrid.AutoScroll = True
        Me.TableLayoutPanelStageGrid.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelStageGrid.ColumnCount = 1
        Me.TableLayoutPanelStageGrid.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelStageGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelStageGrid.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelStageGrid.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanelStageGrid.Name = "TableLayoutPanelStageGrid"
        Me.TableLayoutPanelStageGrid.RowCount = 1
        Me.TableLayoutPanelStageGrid.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanelStageGrid.Size = New System.Drawing.Size(996, 413)
        Me.TableLayoutPanelStageGrid.TabIndex = 18
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetDouble
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel1, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanelStageGrid, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(1002, 462)
        Me.TableLayoutPanel2.TabIndex = 20
        '
        'frmDialogПостроитьЗапроса
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(1002, 462)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDialogПостроитьЗапроса"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Построитель запроса на выборку данных"
        Me.TopMost = True
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Public WithEvents TableLayoutPanelStageGrid As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ButtonRestoreQuery As System.Windows.Forms.Button
    Friend WithEvents ButtonSaveQuery As System.Windows.Forms.Button
    Friend WithEvents ButtonResetQuery As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ButtonRefreshQuery As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

End Class
