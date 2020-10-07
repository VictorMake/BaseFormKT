<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormGraf
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormGraf))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ScatterGraphParameter = New NationalInstruments.UI.WindowsForms.ScatterGraph()
        Me.XyCursor1 = New NationalInstruments.UI.XYCursor()
        Me.ScatterPlot2 = New NationalInstruments.UI.ScatterPlot()
        Me.XAxis1 = New NationalInstruments.UI.XAxis()
        Me.YAxis1 = New NationalInstruments.UI.YAxis()
        Me.ListViewParameter = New BaseFormKT.DbListView(Me.components)
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
        Me.ToolStripBar = New System.Windows.Forms.ToolStrip()
        Me.TSButtonTuneTrand = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonBounds = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ButtonDetail = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TextBoxCollect = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSTextBoxregime = New System.Windows.Forms.ToolStripLabel()
        Me.tsTextBoxОшибкаРасчета = New System.Windows.Forms.ToolStripTextBox()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.ScatterGraphParameter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XyCursor1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.ToolStripBar.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.ScatterGraphParameter)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.ListViewParameter)
        Me.SplitContainer1.Size = New System.Drawing.Size(806, 348)
        Me.SplitContainer1.SplitterDistance = 674
        Me.SplitContainer1.TabIndex = 0
        '
        'ScatterGraphParameter
        '
        Me.ScatterGraphParameter.BackColor = System.Drawing.Color.DimGray
        Me.ScatterGraphParameter.Border = NationalInstruments.UI.Border.None
        Me.ScatterGraphParameter.Cursors.AddRange(New NationalInstruments.UI.XYCursor() {Me.XyCursor1})
        Me.ScatterGraphParameter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ScatterGraphParameter.InteractionMode = NationalInstruments.UI.GraphInteractionModes.DragAnnotationCaption
        Me.ScatterGraphParameter.Location = New System.Drawing.Point(0, 0)
        Me.ScatterGraphParameter.Name = "ScatterGraphParameter"
        Me.ScatterGraphParameter.Plots.AddRange(New NationalInstruments.UI.ScatterPlot() {Me.ScatterPlot2})
        Me.ScatterGraphParameter.Size = New System.Drawing.Size(670, 344)
        Me.ScatterGraphParameter.TabIndex = 124
        Me.ScatterGraphParameter.XAxes.AddRange(New NationalInstruments.UI.XAxis() {Me.XAxis1})
        Me.ScatterGraphParameter.YAxes.AddRange(New NationalInstruments.UI.YAxis() {Me.YAxis1})
        '
        'XyCursor1
        '
        Me.XyCursor1.Color = System.Drawing.Color.White
        Me.XyCursor1.HorizontalCrosshairMode = NationalInstruments.UI.CursorCrosshairMode.None
        Me.XyCursor1.PointStyle = NationalInstruments.UI.PointStyle.SolidTriangleUp
        Me.XyCursor1.SnapMode = NationalInstruments.UI.CursorSnapMode.Fixed
        '
        'ScatterPlot2
        '
        Me.ScatterPlot2.XAxis = Me.XAxis1
        Me.ScatterPlot2.YAxis = Me.YAxis1
        '
        'XAxis1
        '
        Me.XAxis1.AutoMinorDivisionFrequency = 5
        Me.XAxis1.CaptionForeColor = System.Drawing.Color.White
        Me.XAxis1.MajorDivisions.GridVisible = True
        Me.XAxis1.MajorDivisions.LabelForeColor = System.Drawing.Color.White
        Me.XAxis1.MajorDivisions.TickColor = System.Drawing.Color.White
        Me.XAxis1.MinorDivisions.GridVisible = True
        Me.XAxis1.MinorDivisions.TickColor = System.Drawing.Color.White
        Me.XAxis1.Mode = NationalInstruments.UI.AxisMode.Fixed
        Me.XAxis1.Range = New NationalInstruments.UI.Range(0R, 100.0R)
        '
        'YAxis1
        '
        Me.YAxis1.AutoMinorDivisionFrequency = 5
        Me.YAxis1.CaptionForeColor = System.Drawing.Color.White
        Me.YAxis1.MajorDivisions.GridVisible = True
        Me.YAxis1.MajorDivisions.LabelForeColor = System.Drawing.Color.White
        Me.YAxis1.MajorDivisions.TickColor = System.Drawing.Color.White
        Me.YAxis1.MinorDivisions.GridVisible = True
        Me.YAxis1.MinorDivisions.TickColor = System.Drawing.Color.White
        Me.YAxis1.Mode = NationalInstruments.UI.AxisMode.Fixed
        Me.YAxis1.Range = New NationalInstruments.UI.Range(0R, 100.0R)
        '
        'ListViewParametr
        '
        Me.ListViewParameter.BackColor = System.Drawing.Color.Black
        Me.ListViewParameter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewParameter.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold)
        Me.ListViewParameter.ForeColor = System.Drawing.Color.White
        Me.ListViewParameter.GridLines = True
        Me.ListViewParameter.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ListViewParameter.Location = New System.Drawing.Point(0, 0)
        Me.ListViewParameter.MultiSelect = False
        Me.ListViewParameter.Name = "ListViewParametr"
        Me.ListViewParameter.Size = New System.Drawing.Size(124, 344)
        Me.ListViewParameter.TabIndex = 137
        Me.ListViewParameter.UseCompatibleStateImageBehavior = False
        Me.ListViewParameter.View = System.Windows.Forms.View.Details
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.SplitContainer1)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(806, 348)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.Size = New System.Drawing.Size(806, 377)
        Me.ToolStripContainer1.TabIndex = 124
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        '
        'ToolStripContainer1.TopToolStripPanel
        '
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.ToolStripBar)
        '
        'ToolStripBar
        '
        Me.ToolStripBar.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStripBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStripBar.ImageScalingSize = New System.Drawing.Size(22, 22)
        Me.ToolStripBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSButtonTuneTrand, Me.TSButtonBounds, Me.ToolStripSeparator2, Me.ButtonDetail, Me.ToolStripSeparator1, Me.TextBoxCollect, Me.ToolStripSeparator3, Me.TSTextBoxregime, Me.tsTextBoxОшибкаРасчета})
        Me.ToolStripBar.Location = New System.Drawing.Point(3, 0)
        Me.ToolStripBar.Name = "ToolStripBar"
        Me.ToolStripBar.Size = New System.Drawing.Size(283, 29)
        Me.ToolStripBar.TabIndex = 36
        '
        'ButtonTuneTrand
        '
        Me.TSButtonTuneTrand.CheckOnClick = True
        Me.TSButtonTuneTrand.Enabled = False
        Me.TSButtonTuneTrand.Image = CType(resources.GetObject("ButtonTuneTrand.Image"), System.Drawing.Image)
        Me.TSButtonTuneTrand.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSButtonTuneTrand.Name = "ButtonTuneTrand"
        Me.TSButtonTuneTrand.Size = New System.Drawing.Size(82, 26)
        Me.TSButtonTuneTrand.Tag = ""
        Me.TSButtonTuneTrand.Text = "Шлейфы"
        Me.TSButtonTuneTrand.ToolTipText = "настроить шлейфы параметров"
        '
        'ToolStripButtonГраницы
        '
        Me.TSButtonBounds.CheckOnClick = True
        Me.TSButtonBounds.Enabled = False
        Me.TSButtonBounds.Image = CType(resources.GetObject("ToolStripButtonГраницы.Image"), System.Drawing.Image)
        Me.TSButtonBounds.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSButtonBounds.Name = "ToolStripButtonГраницы"
        Me.TSButtonBounds.Size = New System.Drawing.Size(82, 26)
        Me.TSButtonBounds.Text = "Границы"
        Me.TSButtonBounds.ToolTipText = "редактор границ ограничений для параметров"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 29)
        '
        'cmdПодробноВыборочно
        '
        Me.ButtonDetail.CheckOnClick = True
        Me.ButtonDetail.Image = CType(resources.GetObject("cmdПодробноВыборочно.Image"), System.Drawing.Image)
        Me.ButtonDetail.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonDetail.Name = "cmdПодробноВыборочно"
        Me.ButtonDetail.Size = New System.Drawing.Size(98, 26)
        Me.ButtonDetail.Tag = ""
        Me.ButtonDetail.Text = "Выборочно"
        Me.ButtonDetail.ToolTipText = "фильтр списка параметров"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 29)
        '
        'tsTextBoxСбор
        '
        Me.TextBoxCollect.BackColor = System.Drawing.Color.Lime
        Me.TextBoxCollect.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxCollect.Name = "tsTextBoxСбор"
        Me.TextBoxCollect.ReadOnly = True
        Me.TextBoxCollect.Size = New System.Drawing.Size(80, 29)
        Me.TextBoxCollect.Text = "Сбор"
        Me.TextBoxCollect.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.TextBoxCollect.Visible = False
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 29)
        '
        'tsTextBoxРежим
        '
        Me.TSTextBoxregime.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TSTextBoxregime.ForeColor = System.Drawing.Color.Blue
        Me.TSTextBoxregime.Name = "tsTextBoxРежим"
        Me.TSTextBoxregime.Size = New System.Drawing.Size(0, 26)
        '
        'tsTextBoxОшибкаРасчета
        '
        Me.tsTextBoxОшибкаРасчета.BackColor = System.Drawing.Color.Yellow
        Me.tsTextBoxОшибкаРасчета.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.tsTextBoxОшибкаРасчета.ForeColor = System.Drawing.Color.Red
        Me.tsTextBoxОшибкаРасчета.Name = "tsTextBoxОшибкаРасчета"
        Me.tsTextBoxОшибкаРасчета.ReadOnly = True
        Me.tsTextBoxОшибкаРасчета.Size = New System.Drawing.Size(140, 29)
        Me.tsTextBoxОшибкаРасчета.Text = "Ошибка расчета"
        Me.tsTextBoxОшибкаРасчета.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.tsTextBoxОшибкаРасчета.Visible = False
        '
        'frmGraf
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(806, 377)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmGraf"
        Me.Text = "График параметров"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.ScatterGraphParameter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XyCursor1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.ToolStripBar.ResumeLayout(False)
        Me.ToolStripBar.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Private WithEvents ToolStripBar As System.Windows.Forms.ToolStrip
    Friend WithEvents TSButtonTuneTrand As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ButtonDetail As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsTextBoxОшибкаРасчета As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ScatterGraphParameter As NationalInstruments.UI.WindowsForms.ScatterGraph
    Friend WithEvents XAxis1 As NationalInstruments.UI.XAxis
    Friend WithEvents YAxis1 As NationalInstruments.UI.YAxis
    Friend WithEvents XyCursor1 As NationalInstruments.UI.XYCursor
    Friend WithEvents ScatterPlot2 As NationalInstruments.UI.ScatterPlot
    Friend WithEvents TSButtonBounds As System.Windows.Forms.ToolStripButton
    Friend WithEvents TextBoxCollect As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSTextBoxregime As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ListViewParameter As DbListView
End Class
