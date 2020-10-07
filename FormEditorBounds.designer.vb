<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormEditorBounds
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormEditorBounds))
        Me.ToolTipForm = New System.Windows.Forms.ToolTip(Me.components)
        Me.TextBoxParameterWithPlot = New System.Windows.Forms.TextBox()
        Me.ButtonAddGraph = New System.Windows.Forms.Button()
        Me.ButtonDeleteGraph = New System.Windows.Forms.Button()
        Me.ButtonLoadGraph = New System.Windows.Forms.Button()
        Me.ButtonSaveGraph = New System.Windows.Forms.Button()
        Me.ButtonAddNewLimitationGraph = New System.Windows.Forms.Button()
        Me.ButtonDeleteSelectedLimitationGraph = New System.Windows.Forms.Button()
        Me.ButtonClear = New System.Windows.Forms.Button()
        Me.ButtonEditLimitationGraph = New System.Windows.Forms.Button()
        Me.ComboBoxParameters = New System.Windows.Forms.ComboBox()
        Me.ListBoxParametersWithPlot = New System.Windows.Forms.ListBox()
        Me.ImageListNode = New System.Windows.Forms.ImageList(Me.components)
        Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel()
        Me.LabelAvailableLimitationGrap = New System.Windows.Forms.Label()
        Me.PanelControlGraphParam = New System.Windows.Forms.Panel()
        Me.ListBoxLimitationGraphs = New System.Windows.Forms.ListBox()
        Me.LabelCaptionEditorLine = New System.Windows.Forms.Label()
        Me.PanelParameters = New System.Windows.Forms.Panel()
        Me.LabelParameters = New System.Windows.Forms.Label()
        Me.LabelSelectParameter = New System.Windows.Forms.Label()
        Me.TableLayoutPanelButtons = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelEditGraph = New System.Windows.Forms.Label()
        Me.TableLayoutPanelEditPoints = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelNameLine = New System.Windows.Forms.Label()
        Me.LabelColorLine = New System.Windows.Forms.Label()
        Me.LabelStyleLine = New System.Windows.Forms.Label()
        Me.TextBoxNameNewLimitationGraph = New System.Windows.Forms.TextBox()
        Me.PropertyEditorPlotColor = New NationalInstruments.UI.WindowsForms.PropertyEditor()
        Me.PropertyEditorPlotStile = New NationalInstruments.UI.WindowsForms.PropertyEditor()
        Me.StatusStripForm = New System.Windows.Forms.StatusStrip()
        Me.TSStatusLabelMessage = New System.Windows.Forms.ToolStripStatusLabel()
        Me.PanelParameterPlot = New System.Windows.Forms.Panel()
        Me.PanelGrid = New System.Windows.Forms.Panel()
        Me.DataGridViewTablePoit = New System.Windows.Forms.DataGridView()
        Me.LabelCaptionEditorPoints = New System.Windows.Forms.Label()
        Me.TableLayoutPanelBottonsEdit = New System.Windows.Forms.TableLayoutPanel()
        Me.ColumnIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnValueX = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnValueY = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PanelControlGraphParam.SuspendLayout()
        Me.PanelParameters.SuspendLayout()
        Me.TableLayoutPanelButtons.SuspendLayout()
        Me.TableLayoutPanelEditPoints.SuspendLayout()
        Me.StatusStripForm.SuspendLayout()
        Me.PanelParameterPlot.SuspendLayout()
        Me.PanelGrid.SuspendLayout()
        CType(Me.DataGridViewTablePoit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanelBottonsEdit.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBoxParameterWithPlot
        '
        Me.TextBoxParameterWithPlot.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TextBoxParameterWithPlot.Location = New System.Drawing.Point(0, 348)
        Me.TextBoxParameterWithPlot.Name = "TextBoxParameterWithPlot"
        Me.TextBoxParameterWithPlot.ReadOnly = True
        Me.TextBoxParameterWithPlot.Size = New System.Drawing.Size(188, 20)
        Me.TextBoxParameterWithPlot.TabIndex = 13
        Me.ToolTipForm.SetToolTip(Me.TextBoxParameterWithPlot, "Наименование условия")
        '
        'ButtonAddGraph
        '
        Me.ButtonAddGraph.BackColor = System.Drawing.Color.Silver
        Me.ButtonAddGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonAddGraph.Image = CType(resources.GetObject("ButtonAddGraph.Image"), System.Drawing.Image)
        Me.ButtonAddGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAddGraph.Location = New System.Drawing.Point(3, 40)
        Me.ButtonAddGraph.Name = "ButtonAddGraph"
        Me.ButtonAddGraph.Size = New System.Drawing.Size(88, 31)
        Me.ButtonAddGraph.TabIndex = 19
        Me.ButtonAddGraph.Text = "&Добавить"
        Me.ToolTipForm.SetToolTip(Me.ButtonAddGraph, "Добавить  набор границ для параметра")
        Me.ButtonAddGraph.UseVisualStyleBackColor = False
        '
        'ButtonDeleteGraph
        '
        Me.ButtonDeleteGraph.BackColor = System.Drawing.Color.Silver
        Me.ButtonDeleteGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonDeleteGraph.Enabled = False
        Me.ButtonDeleteGraph.Image = CType(resources.GetObject("ButtonDeleteGraph.Image"), System.Drawing.Image)
        Me.ButtonDeleteGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonDeleteGraph.Location = New System.Drawing.Point(97, 40)
        Me.ButtonDeleteGraph.Name = "ButtonDeleteGraph"
        Me.ButtonDeleteGraph.Size = New System.Drawing.Size(88, 31)
        Me.ButtonDeleteGraph.TabIndex = 18
        Me.ButtonDeleteGraph.Text = "&Удалить"
        Me.ToolTipForm.SetToolTip(Me.ButtonDeleteGraph, "Удалить  набор границ для параметра")
        Me.ButtonDeleteGraph.UseVisualStyleBackColor = False
        '
        'ButtonLoadGraph
        '
        Me.ButtonLoadGraph.BackColor = System.Drawing.Color.Silver
        Me.ButtonLoadGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonLoadGraph.Enabled = False
        Me.ButtonLoadGraph.Image = CType(resources.GetObject("ButtonLoadGraph.Image"), System.Drawing.Image)
        Me.ButtonLoadGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonLoadGraph.Location = New System.Drawing.Point(3, 3)
        Me.ButtonLoadGraph.Name = "ButtonLoadGraph"
        Me.ButtonLoadGraph.Size = New System.Drawing.Size(88, 31)
        Me.ButtonLoadGraph.TabIndex = 15
        Me.ButtonLoadGraph.Text = "&Считать"
        Me.ToolTipForm.SetToolTip(Me.ButtonLoadGraph, "Считать набор границ для параметра")
        Me.ButtonLoadGraph.UseVisualStyleBackColor = False
        '
        'ButtonSaveGraph
        '
        Me.ButtonSaveGraph.BackColor = System.Drawing.Color.Silver
        Me.ButtonSaveGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonSaveGraph.Enabled = False
        Me.ButtonSaveGraph.Image = CType(resources.GetObject("ButtonSaveGraph.Image"), System.Drawing.Image)
        Me.ButtonSaveGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonSaveGraph.Location = New System.Drawing.Point(97, 3)
        Me.ButtonSaveGraph.Name = "ButtonSaveGraph"
        Me.ButtonSaveGraph.Size = New System.Drawing.Size(88, 31)
        Me.ButtonSaveGraph.TabIndex = 14
        Me.ButtonSaveGraph.Text = "&Записать"
        Me.ToolTipForm.SetToolTip(Me.ButtonSaveGraph, "Записать  набор границ для параметра")
        Me.ButtonSaveGraph.UseVisualStyleBackColor = False
        '
        'ButtonAddNewLimitationGraph
        '
        Me.ButtonAddNewLimitationGraph.BackColor = System.Drawing.Color.Silver
        Me.ButtonAddNewLimitationGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonAddNewLimitationGraph.Image = CType(resources.GetObject("ButtonAddNewLimitationGraph.Image"), System.Drawing.Image)
        Me.ButtonAddNewLimitationGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAddNewLimitationGraph.Location = New System.Drawing.Point(137, 3)
        Me.ButtonAddNewLimitationGraph.Name = "ButtonAddNewLimitationGraph"
        Me.ButtonAddNewLimitationGraph.Size = New System.Drawing.Size(128, 40)
        Me.ButtonAddNewLimitationGraph.TabIndex = 33
        Me.ButtonAddNewLimitationGraph.Text = "&Добавить новую границу"
        Me.ToolTipForm.SetToolTip(Me.ButtonAddNewLimitationGraph, "Добавить новую границу ограничения")
        Me.ButtonAddNewLimitationGraph.UseVisualStyleBackColor = False
        '
        'ButtonDeleteSelectedLimitationGraph
        '
        Me.ButtonDeleteSelectedLimitationGraph.BackColor = System.Drawing.Color.Silver
        Me.ButtonDeleteSelectedLimitationGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonDeleteSelectedLimitationGraph.Image = CType(resources.GetObject("ButtonDeleteSelectedLimitationGraph.Image"), System.Drawing.Image)
        Me.ButtonDeleteSelectedLimitationGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonDeleteSelectedLimitationGraph.Location = New System.Drawing.Point(3, 49)
        Me.ButtonDeleteSelectedLimitationGraph.Name = "ButtonDeleteSelectedLimitationGraph"
        Me.ButtonDeleteSelectedLimitationGraph.Size = New System.Drawing.Size(128, 40)
        Me.ButtonDeleteSelectedLimitationGraph.TabIndex = 32
        Me.ButtonDeleteSelectedLimitationGraph.Text = "&Удалить выделение"
        Me.ToolTipForm.SetToolTip(Me.ButtonDeleteSelectedLimitationGraph, "Удалить выделенную границу ограничения")
        Me.ButtonDeleteSelectedLimitationGraph.UseVisualStyleBackColor = False
        '
        'ButtonClear
        '
        Me.ButtonClear.BackColor = System.Drawing.Color.Silver
        Me.ButtonClear.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonClear.Enabled = False
        Me.ButtonClear.Image = CType(resources.GetObject("ButtonClear.Image"), System.Drawing.Image)
        Me.ButtonClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonClear.Location = New System.Drawing.Point(137, 49)
        Me.ButtonClear.Name = "ButtonClear"
        Me.ButtonClear.Size = New System.Drawing.Size(128, 40)
        Me.ButtonClear.TabIndex = 25
        Me.ButtonClear.Text = "&Удалить все"
        Me.ToolTipForm.SetToolTip(Me.ButtonClear, "Удалить все границы")
        Me.ButtonClear.UseVisualStyleBackColor = False
        '
        'ButtonEditLimitationGraph
        '
        Me.ButtonEditLimitationGraph.BackColor = System.Drawing.Color.Silver
        Me.ButtonEditLimitationGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonEditLimitationGraph.Image = CType(resources.GetObject("ButtonEditLimitationGraph.Image"), System.Drawing.Image)
        Me.ButtonEditLimitationGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonEditLimitationGraph.Location = New System.Drawing.Point(3, 3)
        Me.ButtonEditLimitationGraph.Name = "ButtonEditLimitationGraph"
        Me.ButtonEditLimitationGraph.Size = New System.Drawing.Size(128, 40)
        Me.ButtonEditLimitationGraph.TabIndex = 40
        Me.ButtonEditLimitationGraph.Text = "&Обновить границу"
        Me.ToolTipForm.SetToolTip(Me.ButtonEditLimitationGraph, "Обновить границу ограничения")
        Me.ButtonEditLimitationGraph.UseVisualStyleBackColor = False
        '
        'ComboBoxParameters
        '
        Me.ComboBoxParameters.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxParameters.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxParameters.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxParameters.Location = New System.Drawing.Point(136, 9)
        Me.ComboBoxParameters.Name = "ComboBoxParameters"
        Me.ComboBoxParameters.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxParameters.Size = New System.Drawing.Size(124, 21)
        Me.ComboBoxParameters.TabIndex = 23
        Me.ComboBoxParameters.Text = "Combo1"
        Me.ToolTipForm.SetToolTip(Me.ComboBoxParameters, "К какому параметру назначены ограничения")
        '
        'ListBoxParametersWithPlot
        '
        Me.ListBoxParametersWithPlot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBoxParametersWithPlot.FormattingEnabled = True
        Me.ListBoxParametersWithPlot.Location = New System.Drawing.Point(0, 34)
        Me.ListBoxParametersWithPlot.Name = "ListBoxParametersWithPlot"
        Me.ListBoxParametersWithPlot.Size = New System.Drawing.Size(188, 298)
        Me.ListBoxParametersWithPlot.TabIndex = 109
        Me.ToolTipForm.SetToolTip(Me.ListBoxParametersWithPlot, "Выбор из списка набора границ ограничений для параметра")
        '
        'ImageListNode
        '
        Me.ImageListNode.ImageStream = CType(resources.GetObject("ImageListNode.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListNode.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListNode.Images.SetKeyName(0, "")
        Me.ImageListNode.Images.SetKeyName(1, "")
        Me.ImageListNode.Images.SetKeyName(2, "")
        Me.ImageListNode.Images.SetKeyName(3, "")
        Me.ImageListNode.Images.SetKeyName(4, "")
        Me.ImageListNode.Images.SetKeyName(5, "")
        Me.ImageListNode.Images.SetKeyName(6, "")
        Me.ImageListNode.Images.SetKeyName(7, "")
        Me.ImageListNode.Images.SetKeyName(8, "AcroForm_api_494.ico")
        Me.ImageListNode.Images.SetKeyName(9, "")
        Me.ImageListNode.Images.SetKeyName(10, "semsys_rll_202.ico")
        Me.ImageListNode.Images.SetKeyName(11, "bakdevs_exe_108.ico")
        Me.ImageListNode.Images.SetKeyName(12, "VB6_EXE_1270.ico")
        Me.ImageListNode.Images.SetKeyName(13, "GRAPH04.ICO")
        Me.ImageListNode.Images.SetKeyName(14, "package_graphics.png")
        Me.ImageListNode.Images.SetKeyName(15, "kspaceduel.png")
        Me.ImageListNode.Images.SetKeyName(16, "semsys_rll_221.ico")
        '
        'BottomToolStripPanel
        '
        Me.BottomToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.BottomToolStripPanel.Name = "BottomToolStripPanel"
        Me.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.BottomToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.BottomToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'TopToolStripPanel
        '
        Me.TopToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.TopToolStripPanel.Name = "TopToolStripPanel"
        Me.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.TopToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.TopToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'RightToolStripPanel
        '
        Me.RightToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.RightToolStripPanel.Name = "RightToolStripPanel"
        Me.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.RightToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.RightToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'LeftToolStripPanel
        '
        Me.LeftToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.LeftToolStripPanel.Name = "LeftToolStripPanel"
        Me.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.LeftToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.LeftToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'ContentPanel
        '
        Me.ContentPanel.Size = New System.Drawing.Size(902, 570)
        '
        'LabelAvailableLimitationGrap
        '
        Me.LabelAvailableLimitationGrap.BackColor = System.Drawing.Color.LightSteelBlue
        Me.LabelAvailableLimitationGrap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelAvailableLimitationGrap.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelAvailableLimitationGrap.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelAvailableLimitationGrap.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelAvailableLimitationGrap.Location = New System.Drawing.Point(0, 0)
        Me.LabelAvailableLimitationGrap.Name = "LabelAvailableLimitationGrap"
        Me.LabelAvailableLimitationGrap.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelAvailableLimitationGrap.Size = New System.Drawing.Size(188, 34)
        Me.LabelAvailableLimitationGrap.TabIndex = 18
        Me.LabelAvailableLimitationGrap.Text = "Доступные границы для параметров"
        Me.LabelAvailableLimitationGrap.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PanelControlGraphParam
        '
        Me.PanelControlGraphParam.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelControlGraphParam.Controls.Add(Me.ListBoxLimitationGraphs)
        Me.PanelControlGraphParam.Controls.Add(Me.LabelCaptionEditorLine)
        Me.PanelControlGraphParam.Controls.Add(Me.PanelParameters)
        Me.PanelControlGraphParam.Controls.Add(Me.LabelSelectParameter)
        Me.PanelControlGraphParam.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControlGraphParam.Location = New System.Drawing.Point(192, 0)
        Me.PanelControlGraphParam.Name = "PanelControlGraphParam"
        Me.PanelControlGraphParam.Size = New System.Drawing.Size(268, 235)
        Me.PanelControlGraphParam.TabIndex = 17
        '
        'ListBoxLimitationGraphs
        '
        Me.ListBoxLimitationGraphs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBoxLimitationGraphs.FormattingEnabled = True
        Me.ListBoxLimitationGraphs.Location = New System.Drawing.Point(0, 93)
        Me.ListBoxLimitationGraphs.Name = "ListBoxLimitationGraphs"
        Me.ListBoxLimitationGraphs.Size = New System.Drawing.Size(264, 138)
        Me.ListBoxLimitationGraphs.TabIndex = 3
        '
        'LabelCaptionEditorLine
        '
        Me.LabelCaptionEditorLine.BackColor = System.Drawing.Color.LightSteelBlue
        Me.LabelCaptionEditorLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelCaptionEditorLine.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelCaptionEditorLine.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelCaptionEditorLine.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelCaptionEditorLine.Location = New System.Drawing.Point(0, 77)
        Me.LabelCaptionEditorLine.Name = "LabelCaptionEditorLine"
        Me.LabelCaptionEditorLine.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelCaptionEditorLine.Size = New System.Drawing.Size(264, 16)
        Me.LabelCaptionEditorLine.TabIndex = 19
        Me.LabelCaptionEditorLine.Text = "Выделить границу из списка для редактирования"
        Me.LabelCaptionEditorLine.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PanelParameters
        '
        Me.PanelParameters.Controls.Add(Me.ComboBoxParameters)
        Me.PanelParameters.Controls.Add(Me.LabelParameters)
        Me.PanelParameters.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelParameters.Location = New System.Drawing.Point(0, 34)
        Me.PanelParameters.Name = "PanelParameters"
        Me.PanelParameters.Size = New System.Drawing.Size(264, 43)
        Me.PanelParameters.TabIndex = 25
        '
        'LabelParameters
        '
        Me.LabelParameters.BackColor = System.Drawing.SystemColors.Control
        Me.LabelParameters.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelParameters.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelParameters.Location = New System.Drawing.Point(4, 13)
        Me.LabelParameters.Name = "LabelParameters"
        Me.LabelParameters.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelParameters.Size = New System.Drawing.Size(136, 17)
        Me.LabelParameters.TabIndex = 24
        Me.LabelParameters.Text = "Границы для параметра"
        '
        'LabelSelectParameter
        '
        Me.LabelSelectParameter.BackColor = System.Drawing.Color.LightSteelBlue
        Me.LabelSelectParameter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelSelectParameter.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelSelectParameter.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelSelectParameter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelSelectParameter.Location = New System.Drawing.Point(0, 0)
        Me.LabelSelectParameter.Name = "LabelSelectParameter"
        Me.LabelSelectParameter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelSelectParameter.Size = New System.Drawing.Size(264, 34)
        Me.LabelSelectParameter.TabIndex = 20
        Me.LabelSelectParameter.Text = "Выбрать параметр для которого задаются границы"
        Me.LabelSelectParameter.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'TableLayoutPanelButtons
        '
        Me.TableLayoutPanelButtons.ColumnCount = 2
        Me.TableLayoutPanelButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelButtons.Controls.Add(Me.ButtonAddGraph, 0, 1)
        Me.TableLayoutPanelButtons.Controls.Add(Me.ButtonDeleteGraph, 0, 1)
        Me.TableLayoutPanelButtons.Controls.Add(Me.ButtonLoadGraph, 0, 0)
        Me.TableLayoutPanelButtons.Controls.Add(Me.ButtonSaveGraph, 1, 0)
        Me.TableLayoutPanelButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanelButtons.Location = New System.Drawing.Point(0, 368)
        Me.TableLayoutPanelButtons.Name = "TableLayoutPanelButtons"
        Me.TableLayoutPanelButtons.RowCount = 2
        Me.TableLayoutPanelButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelButtons.Size = New System.Drawing.Size(188, 74)
        Me.TableLayoutPanelButtons.TabIndex = 107
        '
        'LabelEditGraph
        '
        Me.LabelEditGraph.BackColor = System.Drawing.Color.LightSteelBlue
        Me.LabelEditGraph.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelEditGraph.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelEditGraph.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LabelEditGraph.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelEditGraph.Location = New System.Drawing.Point(0, 332)
        Me.LabelEditGraph.Name = "LabelEditGraph"
        Me.LabelEditGraph.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelEditGraph.Size = New System.Drawing.Size(188, 16)
        Me.LabelEditGraph.TabIndex = 108
        Me.LabelEditGraph.Text = "Выбран для параметра"
        Me.LabelEditGraph.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'TableLayoutPanelEditPoints
        '
        Me.TableLayoutPanelEditPoints.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset
        Me.TableLayoutPanelEditPoints.ColumnCount = 2
        Me.TableLayoutPanelEditPoints.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelEditPoints.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelEditPoints.Controls.Add(Me.LabelNameLine, 0, 0)
        Me.TableLayoutPanelEditPoints.Controls.Add(Me.LabelColorLine, 0, 1)
        Me.TableLayoutPanelEditPoints.Controls.Add(Me.LabelStyleLine, 0, 2)
        Me.TableLayoutPanelEditPoints.Controls.Add(Me.TextBoxNameNewLimitationGraph, 1, 0)
        Me.TableLayoutPanelEditPoints.Controls.Add(Me.PropertyEditorPlotColor, 1, 1)
        Me.TableLayoutPanelEditPoints.Controls.Add(Me.PropertyEditorPlotStile, 1, 2)
        Me.TableLayoutPanelEditPoints.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanelEditPoints.Location = New System.Drawing.Point(192, 327)
        Me.TableLayoutPanelEditPoints.Name = "TableLayoutPanelEditPoints"
        Me.TableLayoutPanelEditPoints.RowCount = 3
        Me.TableLayoutPanelEditPoints.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelEditPoints.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelEditPoints.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelEditPoints.Size = New System.Drawing.Size(268, 119)
        Me.TableLayoutPanelEditPoints.TabIndex = 22
        '
        'LabelNameLine
        '
        Me.LabelNameLine.AutoSize = True
        Me.LabelNameLine.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelNameLine.Location = New System.Drawing.Point(5, 2)
        Me.LabelNameLine.Name = "LabelNameLine"
        Me.LabelNameLine.Size = New System.Drawing.Size(125, 37)
        Me.LabelNameLine.TabIndex = 34
        Me.LabelNameLine.Text = "Имя линии границы"
        Me.LabelNameLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelColorLine
        '
        Me.LabelColorLine.AutoSize = True
        Me.LabelColorLine.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelColorLine.Location = New System.Drawing.Point(5, 41)
        Me.LabelColorLine.Name = "LabelColorLine"
        Me.LabelColorLine.Size = New System.Drawing.Size(125, 37)
        Me.LabelColorLine.TabIndex = 35
        Me.LabelColorLine.Text = "Цвет линии границы"
        Me.LabelColorLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelStyleLine
        '
        Me.LabelStyleLine.AutoSize = True
        Me.LabelStyleLine.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelStyleLine.Location = New System.Drawing.Point(5, 80)
        Me.LabelStyleLine.Name = "LabelStyleLine"
        Me.LabelStyleLine.Size = New System.Drawing.Size(125, 37)
        Me.LabelStyleLine.TabIndex = 36
        Me.LabelStyleLine.Text = "Тип линии границы"
        Me.LabelStyleLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextBoxNameNewLimitationGraph
        '
        Me.TextBoxNameNewLimitationGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxNameNewLimitationGraph.Location = New System.Drawing.Point(138, 5)
        Me.TextBoxNameNewLimitationGraph.Multiline = True
        Me.TextBoxNameNewLimitationGraph.Name = "TextBoxNameNewLimitationGraph"
        Me.TextBoxNameNewLimitationGraph.Size = New System.Drawing.Size(125, 31)
        Me.TextBoxNameNewLimitationGraph.TabIndex = 37
        '
        'PropertyEditorPlotColor
        '
        Me.PropertyEditorPlotColor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PropertyEditorPlotColor.Location = New System.Drawing.Point(138, 44)
        Me.PropertyEditorPlotColor.Name = "PropertyEditorPlotColor"
        Me.PropertyEditorPlotColor.Size = New System.Drawing.Size(125, 20)
        Me.PropertyEditorPlotColor.TabIndex = 38
        '
        'PropertyEditorPlotStile
        '
        Me.PropertyEditorPlotStile.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PropertyEditorPlotStile.Location = New System.Drawing.Point(138, 83)
        Me.PropertyEditorPlotStile.Name = "PropertyEditorPlotStile"
        Me.PropertyEditorPlotStile.Size = New System.Drawing.Size(125, 20)
        Me.PropertyEditorPlotStile.TabIndex = 39
        '
        'StatusStripForm
        '
        Me.StatusStripForm.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSStatusLabelMessage})
        Me.StatusStripForm.Location = New System.Drawing.Point(0, 446)
        Me.StatusStripForm.Name = "StatusStripForm"
        Me.StatusStripForm.Size = New System.Drawing.Size(686, 22)
        Me.StatusStripForm.TabIndex = 22
        Me.StatusStripForm.Text = "StatusStrip1"
        '
        'TSStatusLabelMessage
        '
        Me.TSStatusLabelMessage.Image = CType(resources.GetObject("TSStatusLabelMessage.Image"), System.Drawing.Image)
        Me.TSStatusLabelMessage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.TSStatusLabelMessage.Name = "TSStatusLabelMessage"
        Me.TSStatusLabelMessage.Size = New System.Drawing.Size(671, 17)
        Me.TSStatusLabelMessage.Spring = True
        Me.TSStatusLabelMessage.Text = "Готово"
        '
        'PanelParameterPlot
        '
        Me.PanelParameterPlot.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelParameterPlot.Controls.Add(Me.ListBoxParametersWithPlot)
        Me.PanelParameterPlot.Controls.Add(Me.LabelEditGraph)
        Me.PanelParameterPlot.Controls.Add(Me.TextBoxParameterWithPlot)
        Me.PanelParameterPlot.Controls.Add(Me.LabelAvailableLimitationGrap)
        Me.PanelParameterPlot.Controls.Add(Me.TableLayoutPanelButtons)
        Me.PanelParameterPlot.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelParameterPlot.Location = New System.Drawing.Point(0, 0)
        Me.PanelParameterPlot.Name = "PanelParameterPlot"
        Me.PanelParameterPlot.Size = New System.Drawing.Size(192, 446)
        Me.PanelParameterPlot.TabIndex = 36
        '
        'PanelGrid
        '
        Me.PanelGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelGrid.Controls.Add(Me.DataGridViewTablePoit)
        Me.PanelGrid.Controls.Add(Me.LabelCaptionEditorPoints)
        Me.PanelGrid.Dock = System.Windows.Forms.DockStyle.Right
        Me.PanelGrid.Location = New System.Drawing.Point(460, 0)
        Me.PanelGrid.Name = "PanelGrid"
        Me.PanelGrid.Size = New System.Drawing.Size(226, 446)
        Me.PanelGrid.TabIndex = 37
        '
        'DataGridViewTablePoit
        '
        Me.DataGridViewTablePoit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridViewTablePoit.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        Me.DataGridViewTablePoit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewTablePoit.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColumnIndex, Me.ColumnValueX, Me.ColumnValueY})
        Me.DataGridViewTablePoit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewTablePoit.Location = New System.Drawing.Point(0, 34)
        Me.DataGridViewTablePoit.Name = "DataGridViewTablePoit"
        Me.DataGridViewTablePoit.Size = New System.Drawing.Size(222, 408)
        Me.DataGridViewTablePoit.TabIndex = 21
        '
        'LabelCaptionEditorPoints
        '
        Me.LabelCaptionEditorPoints.BackColor = System.Drawing.Color.LightSteelBlue
        Me.LabelCaptionEditorPoints.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelCaptionEditorPoints.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelCaptionEditorPoints.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelCaptionEditorPoints.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelCaptionEditorPoints.Location = New System.Drawing.Point(0, 0)
        Me.LabelCaptionEditorPoints.Name = "LabelCaptionEditorPoints"
        Me.LabelCaptionEditorPoints.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelCaptionEditorPoints.Size = New System.Drawing.Size(222, 34)
        Me.LabelCaptionEditorPoints.TabIndex = 22
        Me.LabelCaptionEditorPoints.Text = "Ввести по точкам X и Y график границы"
        Me.LabelCaptionEditorPoints.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'TableLayoutPanelBottonsEdit
        '
        Me.TableLayoutPanelBottonsEdit.ColumnCount = 2
        Me.TableLayoutPanelBottonsEdit.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelBottonsEdit.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelBottonsEdit.Controls.Add(Me.ButtonClear, 1, 1)
        Me.TableLayoutPanelBottonsEdit.Controls.Add(Me.ButtonEditLimitationGraph, 0, 0)
        Me.TableLayoutPanelBottonsEdit.Controls.Add(Me.ButtonAddNewLimitationGraph, 1, 0)
        Me.TableLayoutPanelBottonsEdit.Controls.Add(Me.ButtonDeleteSelectedLimitationGraph, 0, 1)
        Me.TableLayoutPanelBottonsEdit.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanelBottonsEdit.Location = New System.Drawing.Point(192, 235)
        Me.TableLayoutPanelBottonsEdit.Name = "TableLayoutPanelBottonsEdit"
        Me.TableLayoutPanelBottonsEdit.RowCount = 2
        Me.TableLayoutPanelBottonsEdit.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelBottonsEdit.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelBottonsEdit.Size = New System.Drawing.Size(268, 92)
        Me.TableLayoutPanelBottonsEdit.TabIndex = 38
        '
        'ColumnIndex
        '
        Me.ColumnIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.ColumnIndex.HeaderText = "Номер точки"
        Me.ColumnIndex.Name = "ColumnIndex"
        Me.ColumnIndex.ReadOnly = True
        '
        'ColumnValueX
        '
        Me.ColumnValueX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.ColumnValueX.HeaderText = "Значение X"
        Me.ColumnValueX.Name = "ColumnValueX"
        '
        'ColumnValueY
        '
        Me.ColumnValueY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.ColumnValueY.HeaderText = "Значение Y"
        Me.ColumnValueY.Name = "ColumnValueY"
        '
        'FormEditorBounds
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(686, 468)
        Me.Controls.Add(Me.PanelControlGraphParam)
        Me.Controls.Add(Me.TableLayoutPanelBottonsEdit)
        Me.Controls.Add(Me.TableLayoutPanelEditPoints)
        Me.Controls.Add(Me.PanelGrid)
        Me.Controls.Add(Me.PanelParameterPlot)
        Me.Controls.Add(Me.StatusStripForm)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormEditorBounds"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Редактор границ ограничений для параметров"
        Me.TopMost = True
        Me.PanelControlGraphParam.ResumeLayout(False)
        Me.PanelParameters.ResumeLayout(False)
        Me.TableLayoutPanelButtons.ResumeLayout(False)
        Me.TableLayoutPanelEditPoints.ResumeLayout(False)
        Me.TableLayoutPanelEditPoints.PerformLayout()
        Me.StatusStripForm.ResumeLayout(False)
        Me.StatusStripForm.PerformLayout()
        Me.PanelParameterPlot.ResumeLayout(False)
        Me.PanelParameterPlot.PerformLayout()
        Me.PanelGrid.ResumeLayout(False)
        CType(Me.DataGridViewTablePoit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanelBottonsEdit.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTipForm As System.Windows.Forms.ToolTip
    Friend WithEvents ImageListNode As System.Windows.Forms.ImageList
    Friend WithEvents ButtonSaveGraph As System.Windows.Forms.Button
    Public WithEvents LabelAvailableLimitationGrap As System.Windows.Forms.Label
    Friend WithEvents PanelControlGraphParam As System.Windows.Forms.Panel
    Friend WithEvents ButtonLoadGraph As System.Windows.Forms.Button
    Friend WithEvents TextBoxParameterWithPlot As System.Windows.Forms.TextBox
    Friend WithEvents ListBoxLimitationGraphs As System.Windows.Forms.ListBox
    Friend WithEvents TableLayoutPanelButtons As System.Windows.Forms.TableLayoutPanel
    Public WithEvents LabelCaptionEditorLine As System.Windows.Forms.Label
    Friend WithEvents ButtonAddGraph As System.Windows.Forms.Button
    Friend WithEvents ButtonDeleteGraph As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanelEditPoints As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ButtonDeleteSelectedLimitationGraph As System.Windows.Forms.Button
    Friend WithEvents PropertyEditorPlotStile As NationalInstruments.UI.WindowsForms.PropertyEditor
    Private WithEvents PropertyEditorPlotColor As NationalInstruments.UI.WindowsForms.PropertyEditor
    Friend WithEvents TextBoxNameNewLimitationGraph As System.Windows.Forms.TextBox
    Friend WithEvents LabelStyleLine As System.Windows.Forms.Label
    Friend WithEvents LabelColorLine As System.Windows.Forms.Label
    Friend WithEvents LabelNameLine As System.Windows.Forms.Label
    Friend WithEvents ButtonAddNewLimitationGraph As System.Windows.Forms.Button
    Public WithEvents LabelEditGraph As System.Windows.Forms.Label
    Public WithEvents LabelSelectParameter As System.Windows.Forms.Label
    Public WithEvents LabelParameters As System.Windows.Forms.Label
    Public WithEvents ComboBoxParameters As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonClear As System.Windows.Forms.Button
    Friend WithEvents ButtonEditLimitationGraph As System.Windows.Forms.Button
    Friend WithEvents StatusStripForm As System.Windows.Forms.StatusStrip
    Friend WithEvents TSStatusLabelMessage As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ListBoxParametersWithPlot As System.Windows.Forms.ListBox
    Friend WithEvents BottomToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents TopToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents RightToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents LeftToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ContentPanel As System.Windows.Forms.ToolStripContentPanel
    Friend WithEvents PanelParameters As System.Windows.Forms.Panel
    Friend WithEvents PanelParameterPlot As System.Windows.Forms.Panel
    Friend WithEvents PanelGrid As System.Windows.Forms.Panel
    Friend WithEvents DataGridViewTablePoit As System.Windows.Forms.DataGridView
    Public WithEvents LabelCaptionEditorPoints As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanelBottonsEdit As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ColumnIndex As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColumnValueX As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColumnValueY As Windows.Forms.DataGridViewTextBoxColumn
End Class
