Imports Microsoft.Reporting.WinForms

Public Class ReportDialog
    Public Property ReportParameter As String

    Private Sub ReportDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ReportDialog))
        'BindingSource.DataSource = ReportManager.GetAllReports()
        ' источник данных формы
        BindingSource.DataSource = ReportManager.ListReports()
        ' источник данных для отчёта
        Dim ReportDataSource1 As ReportDataSource = New ReportDataSource With {
            .Name = "DataSetReportKT",
            .Value = Me.BindingSource
        }
        Me.ReportViewer1.LocalReport.DataSources.Add(ReportDataSource1)
        Me.ReportViewer1.LocalReport.SetParameters(New ReportParameter("ReportParameter1", ReportParameter))
        Me.ReportViewer1.RefreshReport()
    End Sub

    Private Sub ReportDialog_FormClosing(ByVal sender As Object, ByVal e As Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
End Class