﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormAxesAdvanced
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormAxesAdvanced))
        Dim ScaleCustomDivision1 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ScaleCustomDivision2 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ScaleCustomDivision3 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ScaleCustomDivision4 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Me.ToolTipForm = New System.Windows.Forms.ToolTip(Me.components)
        Me.ButtonDeleteConfiguration = New System.Windows.Forms.Button()
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.ButtonRestoreConfiguration = New System.Windows.Forms.Button()
        Me.ButtonSaveConfiguration = New System.Windows.Forms.Button()
        Me.GroupBoxAxisX = New System.Windows.Forms.GroupBox()
        Me.NumericEditParamMax = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.NumericEditParamMin = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.ComboBoxTimeTail = New System.Windows.Forms.ComboBox()
        Me.ComboBoxParametersAxis = New System.Windows.Forms.ComboBox()
        Me.RadioButtonTypeAxisY = New System.Windows.Forms.RadioButton()
        Me.RadioButtonTypeAxisX = New System.Windows.Forms.RadioButton()
        Me.LabelMinAxisX = New System.Windows.Forms.Label()
        Me.LabelMaxAxisX = New System.Windows.Forms.Label()
        Me.LabelTimeTail = New System.Windows.Forms.Label()
        Me.LabelSelectParameterAxisX = New System.Windows.Forms.Label()
        Me.ShapeParameter = New System.Windows.Forms.Label()
        Me.ComboBoxFrequency = New System.Windows.Forms.ComboBox()
        Me.LabelFrequency = New System.Windows.Forms.Label()
        Me.FrameAxisY = New System.Windows.Forms.GroupBox()
        Me.SlideSelectAxis = New NationalInstruments.UI.WindowsForms.Slide()
        Me.NumericEditAxisYMax = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.NumericEditAxisYMin = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.LabelAxisYMin = New System.Windows.Forms.Label()
        Me.LabelAxisYMax = New System.Windows.Forms.Label()
        Me.SlidePositionNumeric = New NationalInstruments.UI.WindowsForms.Slide()
        Me.SlidePositionTicks = New NationalInstruments.UI.WindowsForms.Slide()
        Me.ButtonAddAxis = New System.Windows.Forms.Button()
        Me.ButtonRemoveAxis = New System.Windows.Forms.Button()
        Me.ButtonUpdateAxis = New System.Windows.Forms.Button()
        Me.LabelAxisColorNext = New System.Windows.Forms.Label()
        Me.SlidePlots = New NationalInstruments.UI.WindowsForms.Slide()
        Me.SlideAssignAxis = New NationalInstruments.UI.WindowsForms.Slide()
        Me.ButtonAddPlot = New System.Windows.Forms.Button()
        Me.ButtonRemovePlot = New System.Windows.Forms.Button()
        Me.ComboBoxParametersGraph = New System.Windows.Forms.ComboBox()
        Me.ButtonAssignPlotToAxis = New System.Windows.Forms.Button()
        Me.LabelTrend = New System.Windows.Forms.Label()
        Me.LabelGrafColorNext = New System.Windows.Forms.Label()
        Me.GroupBoxConfiguration = New System.Windows.Forms.GroupBox()
        Me.ComboBoxListConfigurations = New System.Windows.Forms.ComboBox()
        Me.ImageListTab = New System.Windows.Forms.ImageList(Me.components)
        Me.tabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPageAxisX = New System.Windows.Forms.TabPage()
        Me.LabelSelectX = New System.Windows.Forms.Label()
        Me.TabPageAxisY = New System.Windows.Forms.TabPage()
        Me.LabelAxisY = New System.Windows.Forms.Label()
        Me.TabPagePlots = New System.Windows.Forms.TabPage()
        Me.LabelPlots = New System.Windows.Forms.Label()
        Me.GroupBoxAxisX.SuspendLayout()
        CType(Me.NumericEditParamMax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericEditParamMin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FrameAxisY.SuspendLayout()
        CType(Me.SlideSelectAxis, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericEditAxisYMax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericEditAxisYMin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SlidePositionNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SlidePositionTicks, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SlidePlots, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SlideAssignAxis, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxConfiguration.SuspendLayout()
        Me.tabControl1.SuspendLayout()
        Me.TabPageAxisX.SuspendLayout()
        Me.TabPageAxisY.SuspendLayout()
        Me.TabPagePlots.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonDeleteConfiguration
        '
        Me.ButtonDeleteConfiguration.BackgroundImage = CType(resources.GetObject("ButtonDeleteConfiguration.BackgroundImage"), System.Drawing.Image)
        Me.ButtonDeleteConfiguration.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonDeleteConfiguration.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonDeleteConfiguration.ForeColor = System.Drawing.Color.Black
        Me.ButtonDeleteConfiguration.Location = New System.Drawing.Point(278, 111)
        Me.ButtonDeleteConfiguration.Name = "ButtonDeleteConfiguration"
        Me.ButtonDeleteConfiguration.Size = New System.Drawing.Size(88, 88)
        Me.ButtonDeleteConfiguration.TabIndex = 3
        Me.ButtonDeleteConfiguration.Text = "Удалить"
        Me.ButtonDeleteConfiguration.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonDeleteConfiguration, "Удалить выделенную конфигурацию из списка")
        Me.ButtonDeleteConfiguration.UseVisualStyleBackColor = True
        '
        'ButtonClose
        '
        Me.ButtonClose.BackgroundImage = CType(resources.GetObject("ButtonClose.BackgroundImage"), System.Drawing.Image)
        Me.ButtonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonClose.ForeColor = System.Drawing.Color.Black
        Me.ButtonClose.Location = New System.Drawing.Point(372, 111)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(88, 88)
        Me.ButtonClose.TabIndex = 1
        Me.ButtonClose.Text = "Готово"
        Me.ButtonClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonClose, "Закрыть Окно")
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'ButtonRestoreConfiguration
        '
        Me.ButtonRestoreConfiguration.BackgroundImage = CType(resources.GetObject("ButtonRestoreConfiguration.BackgroundImage"), System.Drawing.Image)
        Me.ButtonRestoreConfiguration.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonRestoreConfiguration.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonRestoreConfiguration.ForeColor = System.Drawing.Color.Black
        Me.ButtonRestoreConfiguration.Location = New System.Drawing.Point(278, 17)
        Me.ButtonRestoreConfiguration.Name = "ButtonRestoreConfiguration"
        Me.ButtonRestoreConfiguration.Size = New System.Drawing.Size(88, 88)
        Me.ButtonRestoreConfiguration.TabIndex = 0
        Me.ButtonRestoreConfiguration.Text = "Считать"
        Me.ButtonRestoreConfiguration.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonRestoreConfiguration, "Считать конфигурацию из базы")
        Me.ButtonRestoreConfiguration.UseVisualStyleBackColor = True
        '
        'ButtonSaveConfiguration
        '
        Me.ButtonSaveConfiguration.BackgroundImage = CType(resources.GetObject("ButtonSaveConfiguration.BackgroundImage"), System.Drawing.Image)
        Me.ButtonSaveConfiguration.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonSaveConfiguration.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonSaveConfiguration.ForeColor = System.Drawing.Color.Black
        Me.ButtonSaveConfiguration.Location = New System.Drawing.Point(372, 17)
        Me.ButtonSaveConfiguration.Name = "ButtonSaveConfiguration"
        Me.ButtonSaveConfiguration.Size = New System.Drawing.Size(88, 88)
        Me.ButtonSaveConfiguration.TabIndex = 2
        Me.ButtonSaveConfiguration.Text = "Записать"
        Me.ButtonSaveConfiguration.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonSaveConfiguration, "Записать настройки в базу под именем")
        Me.ButtonSaveConfiguration.UseVisualStyleBackColor = True
        '
        'GroupBoxAxisX
        '
        Me.GroupBoxAxisX.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBoxAxisX.Controls.Add(Me.NumericEditParamMax)
        Me.GroupBoxAxisX.Controls.Add(Me.NumericEditParamMin)
        Me.GroupBoxAxisX.Controls.Add(Me.ComboBoxTimeTail)
        Me.GroupBoxAxisX.Controls.Add(Me.ComboBoxParametersAxis)
        Me.GroupBoxAxisX.Controls.Add(Me.RadioButtonTypeAxisY)
        Me.GroupBoxAxisX.Controls.Add(Me.RadioButtonTypeAxisX)
        Me.GroupBoxAxisX.Controls.Add(Me.LabelMinAxisX)
        Me.GroupBoxAxisX.Controls.Add(Me.LabelMaxAxisX)
        Me.GroupBoxAxisX.Controls.Add(Me.LabelTimeTail)
        Me.GroupBoxAxisX.Controls.Add(Me.LabelSelectParameterAxisX)
        Me.GroupBoxAxisX.Controls.Add(Me.ShapeParameter)
        Me.GroupBoxAxisX.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBoxAxisX.Location = New System.Drawing.Point(3, 46)
        Me.GroupBoxAxisX.Name = "GroupBoxAxisX"
        Me.GroupBoxAxisX.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBoxAxisX.Size = New System.Drawing.Size(451, 208)
        Me.GroupBoxAxisX.TabIndex = 0
        Me.GroupBoxAxisX.TabStop = False
        Me.GroupBoxAxisX.Text = "Выбрать тип оси Х"
        '
        'NumericEditParamMax
        '
        Me.NumericEditParamMax.Enabled = False
        Me.NumericEditParamMax.Location = New System.Drawing.Point(157, 127)
        Me.NumericEditParamMax.Name = "NumericEditParamMax"
        Me.NumericEditParamMax.Size = New System.Drawing.Size(89, 20)
        Me.NumericEditParamMax.TabIndex = 3
        '
        'NumericEditParamMin
        '
        Me.NumericEditParamMin.Enabled = False
        Me.NumericEditParamMin.Location = New System.Drawing.Point(157, 98)
        Me.NumericEditParamMin.Name = "NumericEditParamMin"
        Me.NumericEditParamMin.Size = New System.Drawing.Size(89, 20)
        Me.NumericEditParamMin.TabIndex = 2
        '
        'ComboBoxTimeTail
        '
        Me.ComboBoxTimeTail.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxTimeTail.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxTimeTail.Enabled = False
        Me.ComboBoxTimeTail.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxTimeTail.Location = New System.Drawing.Point(157, 156)
        Me.ComboBoxTimeTail.Name = "ComboBoxTimeTail"
        Me.ComboBoxTimeTail.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxTimeTail.Size = New System.Drawing.Size(89, 21)
        Me.ComboBoxTimeTail.TabIndex = 4
        Me.ComboBoxTimeTail.Text = "Combo1"
        '
        'ComboBoxParametersAxis
        '
        Me.ComboBoxParametersAxis.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxParametersAxis.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxParametersAxis.Enabled = False
        Me.ComboBoxParametersAxis.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxParametersAxis.Location = New System.Drawing.Point(157, 64)
        Me.ComboBoxParametersAxis.Name = "ComboBoxParametersAxis"
        Me.ComboBoxParametersAxis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxParametersAxis.Size = New System.Drawing.Size(89, 21)
        Me.ComboBoxParametersAxis.TabIndex = 1
        Me.ComboBoxParametersAxis.Text = "Combo1"
        '
        'RadioButtonTypeAxisY
        '
        Me.RadioButtonTypeAxisY.BackColor = System.Drawing.Color.DarkGray
        Me.RadioButtonTypeAxisY.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButtonTypeAxisY.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButtonTypeAxisY.Location = New System.Drawing.Point(8, 44)
        Me.RadioButtonTypeAxisY.Name = "RadioButtonTypeAxisY"
        Me.RadioButtonTypeAxisY.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButtonTypeAxisY.Size = New System.Drawing.Size(109, 21)
        Me.RadioButtonTypeAxisY.TabIndex = 26
        Me.RadioButtonTypeAxisY.TabStop = True
        Me.RadioButtonTypeAxisY.Text = "Параметр"
        Me.RadioButtonTypeAxisY.UseVisualStyleBackColor = False
        '
        'RadioButtonTypeAxisX
        '
        Me.RadioButtonTypeAxisX.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButtonTypeAxisX.Checked = True
        Me.RadioButtonTypeAxisX.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButtonTypeAxisX.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButtonTypeAxisX.Location = New System.Drawing.Point(8, 16)
        Me.RadioButtonTypeAxisX.Name = "RadioButtonTypeAxisX"
        Me.RadioButtonTypeAxisX.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButtonTypeAxisX.Size = New System.Drawing.Size(77, 21)
        Me.RadioButtonTypeAxisX.TabIndex = 0
        Me.RadioButtonTypeAxisX.TabStop = True
        Me.RadioButtonTypeAxisX.Text = "Время"
        Me.RadioButtonTypeAxisX.UseVisualStyleBackColor = False
        '
        'LabelMinAxisX
        '
        Me.LabelMinAxisX.BackColor = System.Drawing.Color.DarkGray
        Me.LabelMinAxisX.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelMinAxisX.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelMinAxisX.Location = New System.Drawing.Point(8, 101)
        Me.LabelMinAxisX.Name = "LabelMinAxisX"
        Me.LabelMinAxisX.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelMinAxisX.Size = New System.Drawing.Size(109, 17)
        Me.LabelMinAxisX.TabIndex = 39
        Me.LabelMinAxisX.Text = "Min значение оси"
        '
        'LabelMaxAxisX
        '
        Me.LabelMaxAxisX.BackColor = System.Drawing.Color.DarkGray
        Me.LabelMaxAxisX.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelMaxAxisX.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelMaxAxisX.Location = New System.Drawing.Point(8, 127)
        Me.LabelMaxAxisX.Name = "LabelMaxAxisX"
        Me.LabelMaxAxisX.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelMaxAxisX.Size = New System.Drawing.Size(109, 17)
        Me.LabelMaxAxisX.TabIndex = 38
        Me.LabelMaxAxisX.Text = "Max значение оси"
        '
        'LabelTimeTail
        '
        Me.LabelTimeTail.BackColor = System.Drawing.Color.DarkGray
        Me.LabelTimeTail.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelTimeTail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelTimeTail.Location = New System.Drawing.Point(8, 156)
        Me.LabelTimeTail.Name = "LabelTimeTail"
        Me.LabelTimeTail.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelTimeTail.Size = New System.Drawing.Size(143, 32)
        Me.LabelTimeTail.TabIndex = 30
        Me.LabelTimeTail.Text = "Время отображения шлейфов сек."
        '
        'LabelSelectParameterAxisX
        '
        Me.LabelSelectParameterAxisX.BackColor = System.Drawing.Color.DarkGray
        Me.LabelSelectParameterAxisX.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelSelectParameterAxisX.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelSelectParameterAxisX.Location = New System.Drawing.Point(8, 68)
        Me.LabelSelectParameterAxisX.Name = "LabelSelectParameterAxisX"
        Me.LabelSelectParameterAxisX.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelSelectParameterAxisX.Size = New System.Drawing.Size(89, 17)
        Me.LabelSelectParameterAxisX.TabIndex = 29
        Me.LabelSelectParameterAxisX.Text = "Параметр оси Х"
        '
        'ShapeParameter
        '
        Me.ShapeParameter.BackColor = System.Drawing.Color.DarkGray
        Me.ShapeParameter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ShapeParameter.Location = New System.Drawing.Point(6, 40)
        Me.ShapeParameter.Name = "ShapeParameter"
        Me.ShapeParameter.Size = New System.Drawing.Size(439, 157)
        Me.ShapeParameter.TabIndex = 45
        '
        'ComboBoxFrequency
        '
        Me.ComboBoxFrequency.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxFrequency.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxFrequency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxFrequency.Location = New System.Drawing.Point(160, 271)
        Me.ComboBoxFrequency.Name = "ComboBoxFrequency"
        Me.ComboBoxFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxFrequency.Size = New System.Drawing.Size(89, 21)
        Me.ComboBoxFrequency.TabIndex = 1
        Me.ComboBoxFrequency.Text = "Combo1"
        '
        'LabelFrequency
        '
        Me.LabelFrequency.BackColor = System.Drawing.SystemColors.Control
        Me.LabelFrequency.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelFrequency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelFrequency.Location = New System.Drawing.Point(11, 274)
        Me.LabelFrequency.Name = "LabelFrequency"
        Me.LabelFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelFrequency.Size = New System.Drawing.Size(143, 34)
        Me.LabelFrequency.TabIndex = 20
        Me.LabelFrequency.Text = "Частота построения точек шлейфа Гц"
        '
        'FrameAxisY
        '
        Me.FrameAxisY.BackColor = System.Drawing.SystemColors.Control
        Me.FrameAxisY.Controls.Add(Me.SlideSelectAxis)
        Me.FrameAxisY.Controls.Add(Me.NumericEditAxisYMax)
        Me.FrameAxisY.Controls.Add(Me.NumericEditAxisYMin)
        Me.FrameAxisY.Controls.Add(Me.LabelAxisYMin)
        Me.FrameAxisY.Controls.Add(Me.LabelAxisYMax)
        Me.FrameAxisY.Controls.Add(Me.SlidePositionNumeric)
        Me.FrameAxisY.Controls.Add(Me.SlidePositionTicks)
        Me.FrameAxisY.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameAxisY.Location = New System.Drawing.Point(3, 78)
        Me.FrameAxisY.Name = "FrameAxisY"
        Me.FrameAxisY.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameAxisY.Size = New System.Drawing.Size(246, 304)
        Me.FrameAxisY.TabIndex = 19
        Me.FrameAxisY.TabStop = False
        Me.FrameAxisY.Text = "Редактировать ось"
        '
        'SlideSelectAxis
        '
        Me.SlideSelectAxis.AutoDivisionSpacing = False
        Me.SlideSelectAxis.Border = NationalInstruments.UI.Border.Raised
        Me.SlideSelectAxis.Caption = "Выбрать Ось"
        Me.SlideSelectAxis.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToDivisions
        Me.SlideSelectAxis.FillMode = NationalInstruments.UI.NumericFillMode.None
        Me.SlideSelectAxis.Location = New System.Drawing.Point(6, 19)
        Me.SlideSelectAxis.MajorDivisions.Interval = 1.0R
        Me.SlideSelectAxis.MinorDivisions.TickVisible = False
        Me.SlideSelectAxis.Name = "SlideSelectAxis"
        Me.SlideSelectAxis.Range = New NationalInstruments.UI.Range(0R, 1.0R)
        Me.SlideSelectAxis.ScalePosition = NationalInstruments.UI.NumericScalePosition.Bottom
        Me.SlideSelectAxis.Size = New System.Drawing.Size(234, 84)
        Me.SlideSelectAxis.SlideStyle = NationalInstruments.UI.SlideStyle.SunkenWithGrip
        Me.SlideSelectAxis.TabIndex = 0
        '
        'NumericEditAxisYMax
        '
        Me.NumericEditAxisYMax.Location = New System.Drawing.Point(132, 271)
        Me.NumericEditAxisYMax.Name = "NumericEditAxisYMax"
        Me.NumericEditAxisYMax.Size = New System.Drawing.Size(90, 20)
        Me.NumericEditAxisYMax.TabIndex = 4
        '
        'NumericEditAxisYMin
        '
        Me.NumericEditAxisYMin.Location = New System.Drawing.Point(132, 245)
        Me.NumericEditAxisYMin.Name = "NumericEditAxisYMin"
        Me.NumericEditAxisYMin.Size = New System.Drawing.Size(90, 20)
        Me.NumericEditAxisYMin.TabIndex = 3
        '
        'LabelAxisYMin
        '
        Me.LabelAxisYMin.BackColor = System.Drawing.SystemColors.Control
        Me.LabelAxisYMin.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelAxisYMin.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelAxisYMin.Location = New System.Drawing.Point(22, 245)
        Me.LabelAxisYMin.Name = "LabelAxisYMin"
        Me.LabelAxisYMin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelAxisYMin.Size = New System.Drawing.Size(109, 17)
        Me.LabelAxisYMin.TabIndex = 49
        Me.LabelAxisYMin.Text = "Min значение оси"
        '
        'LabelAxisYMax
        '
        Me.LabelAxisYMax.BackColor = System.Drawing.SystemColors.Control
        Me.LabelAxisYMax.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelAxisYMax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelAxisYMax.Location = New System.Drawing.Point(22, 271)
        Me.LabelAxisYMax.Name = "LabelAxisYMax"
        Me.LabelAxisYMax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelAxisYMax.Size = New System.Drawing.Size(109, 17)
        Me.LabelAxisYMax.TabIndex = 48
        Me.LabelAxisYMax.Text = "Max значение оси"
        '
        'SlidePositionNumeric
        '
        Me.SlidePositionNumeric.Border = NationalInstruments.UI.Border.Raised
        Me.SlidePositionNumeric.Caption = "Числа"
        Me.SlidePositionNumeric.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToDivisions
        ScaleCustomDivision1.Text = "Слева"
        ScaleCustomDivision1.Value = 1.0R
        ScaleCustomDivision2.Text = "Справа"
        ScaleCustomDivision2.Value = 2.0R
        Me.SlidePositionNumeric.CustomDivisions.AddRange(New NationalInstruments.UI.ScaleCustomDivision() {ScaleCustomDivision1, ScaleCustomDivision2})
        Me.SlidePositionNumeric.FillMode = NationalInstruments.UI.NumericFillMode.None
        Me.SlidePositionNumeric.InvertedScale = True
        Me.SlidePositionNumeric.Location = New System.Drawing.Point(132, 109)
        Me.SlidePositionNumeric.MajorDivisions.LabelVisible = False
        Me.SlidePositionNumeric.MajorDivisions.TickVisible = False
        Me.SlidePositionNumeric.MinorDivisions.TickVisible = False
        Me.SlidePositionNumeric.Name = "SlidePositionNumeric"
        Me.SlidePositionNumeric.Range = New NationalInstruments.UI.Range(1.0R, 2.0R)
        Me.SlidePositionNumeric.Size = New System.Drawing.Size(90, 130)
        Me.SlidePositionNumeric.SlideStyle = NationalInstruments.UI.SlideStyle.SunkenWithGrip
        Me.SlidePositionNumeric.TabIndex = 2
        Me.SlidePositionNumeric.Value = 1.0R
        '
        'SlidePositionTicks
        '
        Me.SlidePositionTicks.Border = NationalInstruments.UI.Border.Raised
        Me.SlidePositionTicks.Caption = "Риски"
        Me.SlidePositionTicks.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToDivisions
        ScaleCustomDivision3.Text = "Слева"
        ScaleCustomDivision3.Value = 1.0R
        ScaleCustomDivision4.Text = "Справа"
        ScaleCustomDivision4.Value = 2.0R
        Me.SlidePositionTicks.CustomDivisions.AddRange(New NationalInstruments.UI.ScaleCustomDivision() {ScaleCustomDivision3, ScaleCustomDivision4})
        Me.SlidePositionTicks.FillMode = NationalInstruments.UI.NumericFillMode.None
        Me.SlidePositionTicks.InvertedScale = True
        Me.SlidePositionTicks.Location = New System.Drawing.Point(25, 109)
        Me.SlidePositionTicks.MajorDivisions.LabelVisible = False
        Me.SlidePositionTicks.MajorDivisions.TickVisible = False
        Me.SlidePositionTicks.MinorDivisions.TickVisible = False
        Me.SlidePositionTicks.Name = "SlidePositionTicks"
        Me.SlidePositionTicks.Range = New NationalInstruments.UI.Range(1.0R, 2.0R)
        Me.SlidePositionTicks.Size = New System.Drawing.Size(90, 130)
        Me.SlidePositionTicks.SlideStyle = NationalInstruments.UI.SlideStyle.SunkenWithGrip
        Me.SlidePositionTicks.TabIndex = 1
        Me.SlidePositionTicks.Value = 1.0R
        '
        'ButtonAddAxis
        '
        Me.ButtonAddAxis.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonAddAxis.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonAddAxis.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonAddAxis.Image = CType(resources.GetObject("ButtonAddAxis.Image"), System.Drawing.Image)
        Me.ButtonAddAxis.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAddAxis.Location = New System.Drawing.Point(255, 161)
        Me.ButtonAddAxis.Name = "ButtonAddAxis"
        Me.ButtonAddAxis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonAddAxis.Size = New System.Drawing.Size(199, 25)
        Me.ButtonAddAxis.TabIndex = 0
        Me.ButtonAddAxis.Text = "Добавить &ось"
        Me.ButtonAddAxis.UseVisualStyleBackColor = False
        '
        'ButtonRemoveAxis
        '
        Me.ButtonRemoveAxis.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonRemoveAxis.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonRemoveAxis.Enabled = False
        Me.ButtonRemoveAxis.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonRemoveAxis.Image = CType(resources.GetObject("ButtonRemoveAxis.Image"), System.Drawing.Image)
        Me.ButtonRemoveAxis.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonRemoveAxis.Location = New System.Drawing.Point(255, 192)
        Me.ButtonRemoveAxis.Name = "ButtonRemoveAxis"
        Me.ButtonRemoveAxis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonRemoveAxis.Size = New System.Drawing.Size(199, 25)
        Me.ButtonRemoveAxis.TabIndex = 1
        Me.ButtonRemoveAxis.Text = "&Удалить ось"
        Me.ButtonRemoveAxis.UseVisualStyleBackColor = False
        '
        'ButtonUpdateAxis
        '
        Me.ButtonUpdateAxis.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonUpdateAxis.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonUpdateAxis.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonUpdateAxis.Image = CType(resources.GetObject("ButtonUpdateAxis.Image"), System.Drawing.Image)
        Me.ButtonUpdateAxis.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonUpdateAxis.Location = New System.Drawing.Point(255, 130)
        Me.ButtonUpdateAxis.Name = "ButtonUpdateAxis"
        Me.ButtonUpdateAxis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonUpdateAxis.Size = New System.Drawing.Size(199, 25)
        Me.ButtonUpdateAxis.TabIndex = 2
        Me.ButtonUpdateAxis.Text = "Обновить значения для оси"
        Me.ButtonUpdateAxis.UseVisualStyleBackColor = False
        '
        'LabelAxisColorNext
        '
        Me.LabelAxisColorNext.BackColor = System.Drawing.SystemColors.Control
        Me.LabelAxisColorNext.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelAxisColorNext.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelAxisColorNext.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelAxisColorNext.Location = New System.Drawing.Point(255, 97)
        Me.LabelAxisColorNext.Name = "LabelAxisColorNext"
        Me.LabelAxisColorNext.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelAxisColorNext.Size = New System.Drawing.Size(199, 21)
        Me.LabelAxisColorNext.TabIndex = 12
        Me.LabelAxisColorNext.Text = "Следующий цвет"
        Me.LabelAxisColorNext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SlidePlots
        '
        Me.SlidePlots.Border = NationalInstruments.UI.Border.Raised
        Me.SlidePlots.Caption = "Выбрать шлейф"
        Me.SlidePlots.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToDivisions
        Me.SlidePlots.Enabled = False
        Me.SlidePlots.FillMode = NationalInstruments.UI.NumericFillMode.None
        Me.SlidePlots.InvertedScale = True
        Me.SlidePlots.Location = New System.Drawing.Point(3, 35)
        Me.SlidePlots.MajorDivisions.LabelVisible = False
        Me.SlidePlots.MajorDivisions.TickVisible = False
        Me.SlidePlots.MinorDivisions.TickVisible = False
        Me.SlidePlots.Name = "SlidePlots"
        Me.SlidePlots.Range = New NationalInstruments.UI.Range(0R, 1.0R)
        Me.SlidePlots.Size = New System.Drawing.Size(246, 352)
        Me.SlidePlots.SlideStyle = NationalInstruments.UI.SlideStyle.SunkenWithGrip
        Me.SlidePlots.TabIndex = 3
        '
        'SlideAssignAxis
        '
        Me.SlideAssignAxis.AutoDivisionSpacing = False
        Me.SlideAssignAxis.Border = NationalInstruments.UI.Border.Raised
        Me.SlideAssignAxis.Caption = "К какой оси привязан шлейф"
        Me.SlideAssignAxis.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToDivisions
        Me.SlideAssignAxis.Enabled = False
        Me.SlideAssignAxis.FillMode = NationalInstruments.UI.NumericFillMode.None
        Me.SlideAssignAxis.Location = New System.Drawing.Point(255, 35)
        Me.SlideAssignAxis.MajorDivisions.Interval = 1.0R
        Me.SlideAssignAxis.MinorDivisions.TickVisible = False
        Me.SlideAssignAxis.Name = "SlideAssignAxis"
        Me.SlideAssignAxis.Range = New NationalInstruments.UI.Range(0R, 1.0R)
        Me.SlideAssignAxis.ScalePosition = NationalInstruments.UI.NumericScalePosition.Bottom
        Me.SlideAssignAxis.Size = New System.Drawing.Size(199, 84)
        Me.SlideAssignAxis.SlideStyle = NationalInstruments.UI.SlideStyle.SunkenWithGrip
        Me.SlideAssignAxis.TabIndex = 4
        '
        'ButtonAddPlot
        '
        Me.ButtonAddPlot.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonAddPlot.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonAddPlot.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonAddPlot.Image = CType(resources.GetObject("ButtonAddPlot.Image"), System.Drawing.Image)
        Me.ButtonAddPlot.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAddPlot.Location = New System.Drawing.Point(255, 219)
        Me.ButtonAddPlot.Name = "ButtonAddPlot"
        Me.ButtonAddPlot.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonAddPlot.Size = New System.Drawing.Size(199, 25)
        Me.ButtonAddPlot.TabIndex = 0
        Me.ButtonAddPlot.Text = "Добавить &шлейф"
        Me.ButtonAddPlot.UseVisualStyleBackColor = False
        '
        'ButtonRemovePlot
        '
        Me.ButtonRemovePlot.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonRemovePlot.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonRemovePlot.Enabled = False
        Me.ButtonRemovePlot.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonRemovePlot.Image = CType(resources.GetObject("ButtonRemovePlot.Image"), System.Drawing.Image)
        Me.ButtonRemovePlot.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonRemovePlot.Location = New System.Drawing.Point(255, 250)
        Me.ButtonRemovePlot.Name = "ButtonRemovePlot"
        Me.ButtonRemovePlot.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonRemovePlot.Size = New System.Drawing.Size(199, 25)
        Me.ButtonRemovePlot.TabIndex = 1
        Me.ButtonRemovePlot.Text = "У&далить шлейф"
        Me.ButtonRemovePlot.UseVisualStyleBackColor = False
        '
        'ComboBoxParametersGraph
        '
        Me.ComboBoxParametersGraph.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxParametersGraph.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxParametersGraph.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxParametersGraph.Location = New System.Drawing.Point(318, 160)
        Me.ComboBoxParametersGraph.Name = "ComboBoxParametersGraph"
        Me.ComboBoxParametersGraph.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxParametersGraph.Size = New System.Drawing.Size(136, 21)
        Me.ComboBoxParametersGraph.TabIndex = 14
        Me.ComboBoxParametersGraph.Text = "Combo1"
        '
        'ButtonAssignPlotToAxis
        '
        Me.ButtonAssignPlotToAxis.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonAssignPlotToAxis.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonAssignPlotToAxis.Enabled = False
        Me.ButtonAssignPlotToAxis.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonAssignPlotToAxis.Image = CType(resources.GetObject("ButtonAssignPlotToAxis.Image"), System.Drawing.Image)
        Me.ButtonAssignPlotToAxis.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAssignPlotToAxis.Location = New System.Drawing.Point(255, 125)
        Me.ButtonAssignPlotToAxis.Name = "ButtonAssignPlotToAxis"
        Me.ButtonAssignPlotToAxis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonAssignPlotToAxis.Size = New System.Drawing.Size(199, 25)
        Me.ButtonAssignPlotToAxis.TabIndex = 2
        Me.ButtonAssignPlotToAxis.Text = "Прикрепить шлейф выбранной  оси"
        Me.ButtonAssignPlotToAxis.UseVisualStyleBackColor = False
        '
        'LabelTrend
        '
        Me.LabelTrend.BackColor = System.Drawing.SystemColors.Control
        Me.LabelTrend.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelTrend.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelTrend.Location = New System.Drawing.Point(257, 163)
        Me.LabelTrend.Name = "LabelTrend"
        Me.LabelTrend.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelTrend.Size = New System.Drawing.Size(58, 17)
        Me.LabelTrend.TabIndex = 16
        Me.LabelTrend.Text = "Шлейф"
        '
        'LabelGrafColorNext
        '
        Me.LabelGrafColorNext.BackColor = System.Drawing.SystemColors.Control
        Me.LabelGrafColorNext.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelGrafColorNext.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelGrafColorNext.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelGrafColorNext.Location = New System.Drawing.Point(255, 188)
        Me.LabelGrafColorNext.Name = "LabelGrafColorNext"
        Me.LabelGrafColorNext.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelGrafColorNext.Size = New System.Drawing.Size(199, 25)
        Me.LabelGrafColorNext.TabIndex = 15
        Me.LabelGrafColorNext.Text = "Следующий цвет"
        Me.LabelGrafColorNext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBoxConfiguration
        '
        Me.GroupBoxConfiguration.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBoxConfiguration.Controls.Add(Me.ButtonDeleteConfiguration)
        Me.GroupBoxConfiguration.Controls.Add(Me.ButtonClose)
        Me.GroupBoxConfiguration.Controls.Add(Me.ComboBoxListConfigurations)
        Me.GroupBoxConfiguration.Controls.Add(Me.ButtonRestoreConfiguration)
        Me.GroupBoxConfiguration.Controls.Add(Me.ButtonSaveConfiguration)
        Me.GroupBoxConfiguration.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBoxConfiguration.Location = New System.Drawing.Point(12, 439)
        Me.GroupBoxConfiguration.Name = "GroupBoxConfiguration"
        Me.GroupBoxConfiguration.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBoxConfiguration.Size = New System.Drawing.Size(467, 205)
        Me.GroupBoxConfiguration.TabIndex = 32
        Me.GroupBoxConfiguration.TabStop = False
        Me.GroupBoxConfiguration.Text = "Существующие конфигурации"
        '
        'ComboBoxListConfigurations
        '
        Me.ComboBoxListConfigurations.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxListConfigurations.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxListConfigurations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboBoxListConfigurations.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxListConfigurations.Location = New System.Drawing.Point(9, 19)
        Me.ComboBoxListConfigurations.Name = "ComboBoxListConfigurations"
        Me.ComboBoxListConfigurations.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxListConfigurations.Size = New System.Drawing.Size(246, 181)
        Me.ComboBoxListConfigurations.TabIndex = 4
        '
        'ImageListTab
        '
        Me.ImageListTab.ImageStream = CType(resources.GetObject("ImageListTab.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListTab.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListTab.Images.SetKeyName(0, "")
        Me.ImageListTab.Images.SetKeyName(1, "")
        Me.ImageListTab.Images.SetKeyName(2, "")
        '
        'tabControl1
        '
        Me.tabControl1.Controls.Add(Me.TabPageAxisX)
        Me.tabControl1.Controls.Add(Me.TabPageAxisY)
        Me.tabControl1.Controls.Add(Me.TabPagePlots)
        Me.tabControl1.HotTrack = True
        Me.tabControl1.ImageList = Me.ImageListTab
        Me.tabControl1.ItemSize = New System.Drawing.Size(61, 19)
        Me.tabControl1.Location = New System.Drawing.Point(12, 12)
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.SelectedIndex = 0
        Me.tabControl1.Size = New System.Drawing.Size(469, 421)
        Me.tabControl1.TabIndex = 0
        Me.tabControl1.Text = "TabControl"
        '
        'TabPageAxisX
        '
        Me.TabPageAxisX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TabPageAxisX.Controls.Add(Me.LabelSelectX)
        Me.TabPageAxisX.Controls.Add(Me.GroupBoxAxisX)
        Me.TabPageAxisX.Controls.Add(Me.LabelFrequency)
        Me.TabPageAxisX.Controls.Add(Me.ComboBoxFrequency)
        Me.TabPageAxisX.ImageIndex = 0
        Me.TabPageAxisX.Location = New System.Drawing.Point(4, 23)
        Me.TabPageAxisX.Name = "TabPageAxisX"
        Me.TabPageAxisX.Size = New System.Drawing.Size(461, 394)
        Me.TabPageAxisX.TabIndex = 0
        Me.TabPageAxisX.Text = "Ось X"
        '
        'LabelSelectX
        '
        Me.LabelSelectX.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelSelectX.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelSelectX.ForeColor = System.Drawing.Color.Blue
        Me.LabelSelectX.Location = New System.Drawing.Point(0, 0)
        Me.LabelSelectX.Name = "LabelSelectX"
        Me.LabelSelectX.Size = New System.Drawing.Size(457, 43)
        Me.LabelSelectX.TabIndex = 35
        Me.LabelSelectX.Text = "Определить отображение шлейфов по времени или по значению параметра"
        '
        'TabPageAxisY
        '
        Me.TabPageAxisY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TabPageAxisY.Controls.Add(Me.LabelAxisY)
        Me.TabPageAxisY.Controls.Add(Me.FrameAxisY)
        Me.TabPageAxisY.Controls.Add(Me.ButtonUpdateAxis)
        Me.TabPageAxisY.Controls.Add(Me.ButtonRemoveAxis)
        Me.TabPageAxisY.Controls.Add(Me.ButtonAddAxis)
        Me.TabPageAxisY.Controls.Add(Me.LabelAxisColorNext)
        Me.TabPageAxisY.ImageIndex = 1
        Me.TabPageAxisY.Location = New System.Drawing.Point(4, 23)
        Me.TabPageAxisY.Name = "TabPageAxisY"
        Me.TabPageAxisY.Size = New System.Drawing.Size(461, 394)
        Me.TabPageAxisY.TabIndex = 1
        Me.TabPageAxisY.Text = "Оси Y"
        Me.TabPageAxisY.Visible = False
        '
        'LabelAxisY
        '
        Me.LabelAxisY.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelAxisY.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelAxisY.ForeColor = System.Drawing.Color.Blue
        Me.LabelAxisY.Location = New System.Drawing.Point(0, 0)
        Me.LabelAxisY.Name = "LabelAxisY"
        Me.LabelAxisY.Size = New System.Drawing.Size(457, 75)
        Me.LabelAxisY.TabIndex = 36
        Me.LabelAxisY.Text = "Определить необходимое количество осей, перекрывающих все диапазоны изменения физ" &
    "ических значений параметров (например ось №0:  0-10; ось №1:  0-100; ось №2:  0-" &
    "1000)"
        '
        'TabPagePlots
        '
        Me.TabPagePlots.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TabPagePlots.Controls.Add(Me.SlidePlots)
        Me.TabPagePlots.Controls.Add(Me.LabelPlots)
        Me.TabPagePlots.Controls.Add(Me.ButtonAssignPlotToAxis)
        Me.TabPagePlots.Controls.Add(Me.SlideAssignAxis)
        Me.TabPagePlots.Controls.Add(Me.ButtonAddPlot)
        Me.TabPagePlots.Controls.Add(Me.LabelGrafColorNext)
        Me.TabPagePlots.Controls.Add(Me.ButtonRemovePlot)
        Me.TabPagePlots.Controls.Add(Me.LabelTrend)
        Me.TabPagePlots.Controls.Add(Me.ComboBoxParametersGraph)
        Me.TabPagePlots.ImageIndex = 2
        Me.TabPagePlots.Location = New System.Drawing.Point(4, 23)
        Me.TabPagePlots.Name = "TabPagePlots"
        Me.TabPagePlots.Size = New System.Drawing.Size(461, 394)
        Me.TabPagePlots.TabIndex = 2
        Me.TabPagePlots.Text = "Шлейфы"
        Me.TabPagePlots.Visible = False
        '
        'LabelPlots
        '
        Me.LabelPlots.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelPlots.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelPlots.ForeColor = System.Drawing.Color.Blue
        Me.LabelPlots.Location = New System.Drawing.Point(0, 0)
        Me.LabelPlots.Name = "LabelPlots"
        Me.LabelPlots.Size = New System.Drawing.Size(457, 32)
        Me.LabelPlots.TabIndex = 52
        Me.LabelPlots.Text = "Добавить шлейф необходимого параметра и прикрепить его к оси с подходящим диапазо" &
    "ном"
        '
        'FormAxesAdvanced
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(491, 656)
        Me.Controls.Add(Me.tabControl1)
        Me.Controls.Add(Me.GroupBoxConfiguration)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormAxesAdvanced"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Настройка шлейфов для графика"
        Me.TopMost = True
        Me.GroupBoxAxisX.ResumeLayout(False)
        CType(Me.NumericEditParamMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericEditParamMin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FrameAxisY.ResumeLayout(False)
        CType(Me.SlideSelectAxis, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericEditAxisYMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericEditAxisYMin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SlidePositionNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SlidePositionTicks, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SlidePlots, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SlideAssignAxis, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxConfiguration.ResumeLayout(False)
        Me.tabControl1.ResumeLayout(False)
        Me.TabPageAxisX.ResumeLayout(False)
        Me.TabPageAxisY.ResumeLayout(False)
        Me.TabPagePlots.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolTipForm As System.Windows.Forms.ToolTip
    Public WithEvents GroupBoxAxisX As System.Windows.Forms.GroupBox
    Public WithEvents ComboBoxTimeTail As System.Windows.Forms.ComboBox
    Public WithEvents ComboBoxParametersAxis As System.Windows.Forms.ComboBox
    Public WithEvents RadioButtonTypeAxisY As System.Windows.Forms.RadioButton
    Public WithEvents ComboBoxFrequency As System.Windows.Forms.ComboBox
    Public WithEvents RadioButtonTypeAxisX As System.Windows.Forms.RadioButton
    Public WithEvents LabelMinAxisX As System.Windows.Forms.Label
    Public WithEvents LabelMaxAxisX As System.Windows.Forms.Label
    Public WithEvents LabelTimeTail As System.Windows.Forms.Label
    Public WithEvents LabelSelectParameterAxisX As System.Windows.Forms.Label
    Public WithEvents ShapeParameter As System.Windows.Forms.Label
    Public WithEvents LabelFrequency As System.Windows.Forms.Label
    Public WithEvents FrameAxisY As System.Windows.Forms.GroupBox
    Public WithEvents ButtonAddAxis As System.Windows.Forms.Button
    Public WithEvents ButtonRemoveAxis As System.Windows.Forms.Button
    Public WithEvents ButtonUpdateAxis As System.Windows.Forms.Button
    Public WithEvents LabelAxisColorNext As System.Windows.Forms.Label
    Public WithEvents ButtonAddPlot As System.Windows.Forms.Button
    Public WithEvents ButtonRemovePlot As System.Windows.Forms.Button
    Public WithEvents ComboBoxParametersGraph As System.Windows.Forms.ComboBox
    Public WithEvents ButtonAssignPlotToAxis As System.Windows.Forms.Button
    Public WithEvents LabelTrend As System.Windows.Forms.Label
    Public WithEvents LabelGrafColorNext As System.Windows.Forms.Label
    Public WithEvents GroupBoxConfiguration As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonDeleteConfiguration As System.Windows.Forms.Button
    Friend WithEvents ButtonClose As System.Windows.Forms.Button
    Public WithEvents ComboBoxListConfigurations As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonRestoreConfiguration As System.Windows.Forms.Button
    Friend WithEvents ButtonSaveConfiguration As System.Windows.Forms.Button
    Friend WithEvents NumericEditParamMax As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents NumericEditParamMin As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents SlidePositionTicks As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents NumericEditAxisYMax As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents NumericEditAxisYMin As NationalInstruments.UI.WindowsForms.NumericEdit
    Public WithEvents LabelAxisYMin As System.Windows.Forms.Label
    Public WithEvents LabelAxisYMax As System.Windows.Forms.Label
    Friend WithEvents SlidePositionNumeric As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents SlideSelectAxis As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents SlideAssignAxis As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents SlidePlots As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents ImageListTab As System.Windows.Forms.ImageList
    Protected WithEvents tabControl1 As System.Windows.Forms.TabControl
    Protected WithEvents TabPageAxisX As System.Windows.Forms.TabPage
    Friend WithEvents LabelSelectX As System.Windows.Forms.Label
    Protected WithEvents TabPageAxisY As System.Windows.Forms.TabPage
    Friend WithEvents LabelAxisY As System.Windows.Forms.Label
    Protected WithEvents TabPagePlots As System.Windows.Forms.TabPage
    Friend WithEvents LabelPlots As System.Windows.Forms.Label

End Class
