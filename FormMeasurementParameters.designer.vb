﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMeasurementParameters
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
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMeasurementParameters))
        Me.Label1 = New System.Windows.Forms.Label
        Me.DataGridViewMeasurement = New System.Windows.Forms.DataGridView
        Me.ИмяПараметраDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ОписаниеПараметраDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ИмяКаналаИзмеренияDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.ИспользоватьКонстантуDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.ЗначениеКонстантыDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.РазмерностьВходнаяDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.ИмяБазовогоПараметраDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.ТипДавленияDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.ИзмеренныеПараметрыBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.BaseFormDataSet = New BaseFormKT.BaseFormDataSet
        Me.BindingNavigator1 = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorAddNewItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorDeleteItem = New System.Windows.Forms.ToolStripButton
        Me.SaveToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.TSButtonHelp = New System.Windows.Forms.ToolStripButton
        Me.MeasurementParametersTableAdapter = New BaseFormKT.BaseFormDataSetTableAdapters.ИзмеренныеПараметрыTableAdapter
        CType(Me.DataGridViewMeasurement, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ИзмеренныеПараметрыBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BaseFormDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingNavigator1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BindingNavigator1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(820, 29)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Соответствие имен входных параметров расчета и имен измерительных каналов"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DataGridViewИзмеренные
        '
        Me.DataGridViewMeasurement.AllowUserToAddRows = False
        Me.DataGridViewMeasurement.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSteelBlue
        Me.DataGridViewMeasurement.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridViewMeasurement.AutoGenerateColumns = False
        Me.DataGridViewMeasurement.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridViewMeasurement.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridViewMeasurement.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewMeasurement.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewMeasurement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewMeasurement.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ИмяПараметраDataGridViewTextBoxColumn, Me.ОписаниеПараметраDataGridViewTextBoxColumn, Me.ИмяКаналаИзмеренияDataGridViewTextBoxColumn, Me.ИспользоватьКонстантуDataGridViewCheckBoxColumn, Me.ЗначениеКонстантыDataGridViewTextBoxColumn, Me.РазмерностьВходнаяDataGridViewTextBoxColumn, Me.ИмяБазовогоПараметраDataGridViewTextBoxColumn, Me.ТипДавленияDataGridViewTextBoxColumn})
        Me.DataGridViewMeasurement.DataSource = Me.ИзмеренныеПараметрыBindingSource
        Me.DataGridViewMeasurement.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewMeasurement.Location = New System.Drawing.Point(0, 54)
        Me.DataGridViewMeasurement.MultiSelect = False
        Me.DataGridViewMeasurement.Name = "DataGridViewИзмеренные"
        Me.DataGridViewMeasurement.RowHeadersVisible = False
        Me.DataGridViewMeasurement.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.DataGridViewMeasurement.Size = New System.Drawing.Size(820, 525)
        Me.DataGridViewMeasurement.TabIndex = 8
        '
        'ИмяПараметраDataGridViewTextBoxColumn
        '
        Me.ИмяПараметраDataGridViewTextBoxColumn.DataPropertyName = "ИмяПараметра"
        Me.ИмяПараметраDataGridViewTextBoxColumn.HeaderText = "Имя параметра"
        Me.ИмяПараметраDataGridViewTextBoxColumn.Name = "ИмяПараметраDataGridViewTextBoxColumn"
        Me.ИмяПараметраDataGridViewTextBoxColumn.ReadOnly = True
        '
        'ОписаниеПараметраDataGridViewTextBoxColumn
        '
        Me.ОписаниеПараметраDataGridViewTextBoxColumn.DataPropertyName = "ОписаниеПараметра"
        Me.ОписаниеПараметраDataGridViewTextBoxColumn.HeaderText = "Описание параметра"
        Me.ОписаниеПараметраDataGridViewTextBoxColumn.Name = "ОписаниеПараметраDataGridViewTextBoxColumn"
        Me.ОписаниеПараметраDataGridViewTextBoxColumn.ReadOnly = True
        '
        'ИмяКаналаИзмеренияDataGridViewTextBoxColumn
        '
        Me.ИмяКаналаИзмеренияDataGridViewTextBoxColumn.DataPropertyName = "ИмяКаналаИзмерения"
        Me.ИмяКаналаИзмеренияDataGridViewTextBoxColumn.HeaderText = "Имя канала измерения"
        Me.ИмяКаналаИзмеренияDataGridViewTextBoxColumn.Name = "ИмяКаналаИзмеренияDataGridViewTextBoxColumn"
        Me.ИмяКаналаИзмеренияDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ИмяКаналаИзмеренияDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'ИспользоватьКонстантуDataGridViewCheckBoxColumn
        '
        Me.ИспользоватьКонстантуDataGridViewCheckBoxColumn.DataPropertyName = "ИспользоватьКонстанту"
        Me.ИспользоватьКонстантуDataGridViewCheckBoxColumn.HeaderText = "Использовать константу"
        Me.ИспользоватьКонстантуDataGridViewCheckBoxColumn.Name = "ИспользоватьКонстантуDataGridViewCheckBoxColumn"
        '
        'ЗначениеКонстантыDataGridViewTextBoxColumn
        '
        Me.ЗначениеКонстантыDataGridViewTextBoxColumn.DataPropertyName = "ЗначениеКонстанты"
        Me.ЗначениеКонстантыDataGridViewTextBoxColumn.HeaderText = "Значение константы"
        Me.ЗначениеКонстантыDataGridViewTextBoxColumn.Name = "ЗначениеКонстантыDataGridViewTextBoxColumn"
        Me.ЗначениеКонстантыDataGridViewTextBoxColumn.ReadOnly = True
        '
        'РазмерностьВходнаяDataGridViewTextBoxColumn
        '
        Me.РазмерностьВходнаяDataGridViewTextBoxColumn.DataPropertyName = "РазмерностьВходная"
        Me.РазмерностьВходнаяDataGridViewTextBoxColumn.HeaderText = "Ед. изм. выходная"
        Me.РазмерностьВходнаяDataGridViewTextBoxColumn.Name = "РазмерностьВходнаяDataGridViewTextBoxColumn"
        Me.РазмерностьВходнаяDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.РазмерностьВходнаяDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'ИмяБазовогоПараметраDataGridViewTextBoxColumn
        '
        Me.ИмяБазовогоПараметраDataGridViewTextBoxColumn.DataPropertyName = "ИмяБазовогоПараметра"
        Me.ИмяБазовогоПараметраDataGridViewTextBoxColumn.HeaderText = "Имя базового параметра"
        Me.ИмяБазовогоПараметраDataGridViewTextBoxColumn.Name = "ИмяБазовогоПараметраDataGridViewTextBoxColumn"
        Me.ИмяБазовогоПараметраDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ИмяБазовогоПараметраDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'ТипДавленияDataGridViewTextBoxColumn
        '
        Me.ТипДавленияDataGridViewTextBoxColumn.DataPropertyName = "ТипДавления"
        Me.ТипДавленияDataGridViewTextBoxColumn.HeaderText = "Тип давления"
        Me.ТипДавленияDataGridViewTextBoxColumn.Name = "ТипДавленияDataGridViewTextBoxColumn"
        Me.ТипДавленияDataGridViewTextBoxColumn.ReadOnly = True
        Me.ТипДавленияDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ТипДавленияDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'ИзмеренныеПараметрыBindingSource
        '
        Me.ИзмеренныеПараметрыBindingSource.DataMember = "ИзмеренныеПараметры"
        Me.ИзмеренныеПараметрыBindingSource.DataSource = Me.BaseFormDataSet
        '
        'BaseFormDataSet
        '
        Me.BaseFormDataSet.DataSetName = "BaseFormDataSet"
        Me.BaseFormDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'BindingNavigator1
        '
        Me.BindingNavigator1.AddNewItem = Nothing
        Me.BindingNavigator1.BindingSource = Me.ИзмеренныеПараметрыBindingSource
        Me.BindingNavigator1.CountItem = Me.BindingNavigatorCountItem
        Me.BindingNavigator1.DeleteItem = Nothing
        Me.BindingNavigator1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem, Me.BindingNavigatorSeparator2, Me.BindingNavigatorAddNewItem, Me.BindingNavigatorDeleteItem, Me.SaveToolStripButton, Me.toolStripSeparator1, Me.TSButtonHelp})
        Me.BindingNavigator1.Location = New System.Drawing.Point(0, 29)
        Me.BindingNavigator1.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
        Me.BindingNavigator1.MoveLastItem = Me.BindingNavigatorMoveLastItem
        Me.BindingNavigator1.MoveNextItem = Me.BindingNavigatorMoveNextItem
        Me.BindingNavigator1.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
        Me.BindingNavigator1.Name = "BindingNavigator1"
        Me.BindingNavigator1.PositionItem = Me.BindingNavigatorPositionItem
        Me.BindingNavigator1.Size = New System.Drawing.Size(820, 25)
        Me.BindingNavigator1.TabIndex = 9
        Me.BindingNavigator1.Text = "BindingNavigator1"
        '
        'BindingNavigatorCountItem
        '
        Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
        Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(35, 22)
        Me.BindingNavigatorCountItem.Text = "of {0}"
        Me.BindingNavigatorCountItem.ToolTipText = "Total number of items"
        '
        'BindingNavigatorMoveFirstItem
        '
        Me.BindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveFirstItem.Image = CType(resources.GetObject("BindingNavigatorMoveFirstItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveFirstItem.Name = "BindingNavigatorMoveFirstItem"
        Me.BindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveFirstItem.Text = "Move first"
        '
        'BindingNavigatorMovePreviousItem
        '
        Me.BindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMovePreviousItem.Image = CType(resources.GetObject("BindingNavigatorMovePreviousItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMovePreviousItem.Name = "BindingNavigatorMovePreviousItem"
        Me.BindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMovePreviousItem.Text = "Move previous"
        '
        'BindingNavigatorSeparator
        '
        Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
        Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorPositionItem
        '
        Me.BindingNavigatorPositionItem.AccessibleName = "Position"
        Me.BindingNavigatorPositionItem.AutoSize = False
        Me.BindingNavigatorPositionItem.Name = "BindingNavigatorPositionItem"
        Me.BindingNavigatorPositionItem.Size = New System.Drawing.Size(50, 21)
        Me.BindingNavigatorPositionItem.Text = "0"
        Me.BindingNavigatorPositionItem.ToolTipText = "Current position"
        '
        'BindingNavigatorSeparator1
        '
        Me.BindingNavigatorSeparator1.Name = "BindingNavigatorSeparator1"
        Me.BindingNavigatorSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorMoveNextItem
        '
        Me.BindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveNextItem.Image = CType(resources.GetObject("BindingNavigatorMoveNextItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveNextItem.Name = "BindingNavigatorMoveNextItem"
        Me.BindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveNextItem.Text = "Move next"
        '
        'BindingNavigatorMoveLastItem
        '
        Me.BindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveLastItem.Image = CType(resources.GetObject("BindingNavigatorMoveLastItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveLastItem.Name = "BindingNavigatorMoveLastItem"
        Me.BindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveLastItem.Text = "Move last"
        '
        'BindingNavigatorSeparator2
        '
        Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
        Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorAddNewItem
        '
        Me.BindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorAddNewItem.Enabled = False
        Me.BindingNavigatorAddNewItem.Image = CType(resources.GetObject("BindingNavigatorAddNewItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorAddNewItem.Name = "BindingNavigatorAddNewItem"
        Me.BindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorAddNewItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorAddNewItem.Text = "Add new"
        Me.BindingNavigatorAddNewItem.Visible = False
        '
        'BindingNavigatorDeleteItem
        '
        Me.BindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorDeleteItem.Enabled = False
        Me.BindingNavigatorDeleteItem.Image = CType(resources.GetObject("BindingNavigatorDeleteItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorDeleteItem.Name = "BindingNavigatorDeleteItem"
        Me.BindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorDeleteItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorDeleteItem.Text = "Delete"
        Me.BindingNavigatorDeleteItem.Visible = False
        '
        'SaveToolStripButton
        '
        Me.SaveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.SaveToolStripButton.Image = CType(resources.GetObject("SaveToolStripButton.Image"), System.Drawing.Image)
        Me.SaveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveToolStripButton.Name = "SaveToolStripButton"
        Me.SaveToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.SaveToolStripButton.Text = "&Save"
        '
        'toolStripSeparator1
        '
        Me.toolStripSeparator1.Name = "toolStripSeparator1"
        Me.toolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsButtonHelp
        '
        Me.TSButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonHelp.Image = CType(resources.GetObject("tsButtonHelp.Image"), System.Drawing.Image)
        Me.TSButtonHelp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSButtonHelp.Name = "tsButtonHelp"
        Me.TSButtonHelp.Size = New System.Drawing.Size(23, 22)
        Me.TSButtonHelp.Text = "Помощь"
        '
        'ИзмеренныеПараметрыTableAdapter
        '
        Me.MeasurementParametersTableAdapter.ClearBeforeFill = True
        '
        'frmИзмеренныеПараметры
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(820, 579)
        Me.Controls.Add(Me.DataGridViewMeasurement)
        Me.Controls.Add(Me.BindingNavigator1)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmИзмеренныеПараметры"
        Me.Text = "Измеренные параметры"
        CType(Me.DataGridViewMeasurement, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ИзмеренныеПараметрыBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BaseFormDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingNavigator1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BindingNavigator1.ResumeLayout(False)
        Me.BindingNavigator1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents KeyПараметрDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BindingNavigator1 As System.Windows.Forms.BindingNavigator
    Friend WithEvents BindingNavigatorAddNewItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
    Friend WithEvents BindingNavigatorDeleteItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents BindingNavigatorSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ИндексПараметраИзмеренияDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ИзмеренныеПараметрыBindingSource As System.Windows.Forms.BindingSource
    Public WithEvents DataGridViewMeasurement As System.Windows.Forms.DataGridView
    Public WithEvents MeasurementParametersTableAdapter As BaseFormKT.BaseFormDataSetTableAdapters.ИзмеренныеПараметрыTableAdapter
    Public WithEvents BaseFormDataSet As BaseFormKT.BaseFormDataSet


    Friend WithEvents РазмерностьВыходнаяDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents TSButtonHelp As System.Windows.Forms.ToolStripButton
    Friend WithEvents SaveToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ИмяПараметраDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ОписаниеПараметраDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ИмяКаналаИзмеренияDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents ИспользоватьКонстантуDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents ЗначениеКонстантыDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents РазмерностьВходнаяDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents ИмяБазовогоПараметраDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents ТипДавленияDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewComboBoxColumn
End Class
