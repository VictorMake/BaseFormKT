Imports System.IO
Imports Microsoft.Office.Interop

'Module CreateExcelBook
'    Private currentColumn As Integer
'    Private currentRow As Integer
'    Private startBlock As Integer
'    Private indexRowValue As Integer
'    Private ColorIndex As Integer
'    ''' <summary>
'    ''' Заголовок
'    ''' </summary>
'    Private title As String = ""

'    ''' <summary>
'    ''' Тип Параметра
'    ''' </summary>
'    Private Enum TypeParameter
'        ''' <summary>
'        ''' Измерение
'        ''' </summary>
'        Measurement
'        ''' <summary>
'        ''' Приведение
'        ''' </summary>
'        PhysicalCast
'        ''' <summary>
'        ''' Пересчет
'        ''' </summary>
'        Converting
'    End Enum

'    Public Sub CreateExcelBookForKT(ByRef ControlsForPhase As Dictionary(Of String, Dictionary(Of String, MDBControlLibrary.IUserControl)),
'                                    ByRef StageConstNames() As String,
'                                    ByRef StageNames() As String,
'                                    ByVal varProjectManager As ProjectManager)
'        Dim J As Integer
'        Dim controlsCount As Integer

'        Dim excelApp As New Excel.Application()
'        Dim excelBook As Excel.Workbook = excelApp.Workbooks.Add
'        Dim excelWorksheet As Excel.Worksheet = CType(excelBook.Worksheets(1), Excel.Worksheet)

'        Dim MeasurementRows As New List(Of DataRow) ' Измерение
'        Dim CastRows As New List(Of DataRow) ' Приведение
'        Dim ConvertingRows As New List(Of DataRow) ' Пересчет

'        excelApp.Visible = False

'        'With excelWorksheet
'        '    ' Set the column headers and desired formatting for the spreadsheet.
'        '    .Columns().ColumnWidth = 21.71
'        '    .Range("A1").Value = "Item"
'        '    .Range("A1").Font.Bold = True
'        '    .Range("B1").Value = "Price"
'        '    .Range("B1").Font.Bold = True
'        '    .Range("C1").Value = "Calories"
'        '    .Range("C1").Font.Bold = True

'        '    ' Start the counter on the second row, following the column headers
'        '    Dim i As Integer = 2
'        '    ' Loop through the Rows collection of the DataSet and write the data
'        '    ' in each row to the cells in Excel. 
'        '    Dim dr As DataRow
'        '    For Each dr In dsMenu.Tables(0).Rows
'        '        .Range("A" & i.ToString).Value = dr("Item")
'        '        .Range("B" & i.ToString).Value = dr("Price")
'        '        .Range("C" & i.ToString).Value = dr("Calories")
'        '        i += 1
'        '    Next

'        '    ' Select and apply formatting to the cell that will display the calorie
'        '    ' average, then call the Average formula.  Note that the AVERAGE function
'        '    ' is localized, so the below code may need to be updated based on the 
'        '    ' locale the application is deployed to.
'        '    .Range("C7").Select()
'        '    .Range("C7").Font.Color = RGB(255, 0, 0)
'        '    .Range("C7").Font.Bold = True
'        '    excelApp.ActiveCell.FormulaR1C1 = "=AVERAGE(R[-5]C:R[-1]C)"
'        'End With

'        With excelApp
'            .Cells.Select()
'            .Selection.Clear()
'            .Columns("A:A").ColumnWidth = 0.67
'            .Rows("1:1").RowHeight = 4.5
'            .Application.ScreenUpdating = False

'            currentRow = 2
'            For I As Integer = 0 To StageNames.Count - 1
'                'I здесь этапы
'                currentColumn = 2
'                ' Тип изделия
'                .Range("B" & CStr(currentRow)).Select()
'                With .Selection
'                    .HorizontalAlignment = Excel.Constants.xlCenter
'                    .VerticalAlignment = Excel.Constants.xlTop
'                    .WrapText = True
'                    .Orientation = 0
'                    .AddIndent = False
'                    .IndentLevel = 0
'                    .ShrinkToFit = False
'                    .ReadingOrder = Excel.Constants.xlContext
'                    .MergeCells = False
'                End With

'                .ActiveCell.FormulaR1C1 = StageConstNames(I) '"Имя этапа"

'                With .ActiveCell.Characters(Start:=1, Length:=Len(CStr(.ActiveCell.Value))).Font
'                    .Name = "Arial"
'                    .FontStyle = "обычный"
'                    .Size = 10
'                    .Strikethrough = False
'                    .Superscript = False
'                    .Subscript = False
'                    .OutlineFont = False
'                    .Shadow = False
'                    .Underline = Excel.XlUnderlineStyle.xlUnderlineStyleNone
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With

'                ' Примечание
'                .Range("C" & CStr(currentRow) & ":I" & CStr(currentRow)).Select()
'                With .Selection
'                    .HorizontalAlignment = Excel.Constants.xlCenter
'                    .VerticalAlignment = Excel.Constants.xlCenter
'                    .WrapText = True
'                    .Orientation = 0
'                    .AddIndent = False
'                    .IndentLevel = 0
'                    .ShrinkToFit = False
'                    .ReadingOrder = Excel.Constants.xlContext
'                    .MergeCells = True
'                End With

'                For Each keyControl As MDBControlLibrary.IUserControl In ControlsForPhase.Item(StageNames(I)).Values
'                    If keyControl.Name.IndexOf("Описание") <> -1 Then
'                        .ActiveCell.FormulaR1C1 = "0" 'keyControl.ЗначениеПользователя '"Примечание этапа"
'                        keyControl.Row = currentRow
'                        keyControl.Col = 3 ' currentRow всегда 3
'                        Exit For
'                    End If
'                Next

'                With .ActiveCell.Characters(Start:=1, Length:=Len(CStr(.ActiveCell.Value))).Font
'                    .Name = "Arial"
'                    .FontStyle = "обычный"
'                    .Size = 10
'                    .Strikethrough = False
'                    .Superscript = False
'                    .Subscript = False
'                    .OutlineFont = False
'                    .Shadow = False
'                    .Underline = Excel.XlUnderlineStyle.xlUnderlineStyleNone
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With

'                ' выделить область жирным
'                .Range("B" & CStr(currentRow) & ":I" & CStr(currentRow)).Select()
'                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
'                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone

'                With .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlThin
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlThin
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlThin
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlThin
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlThin
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With

'                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
'                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone

'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlMedium
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlMedium
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlMedium
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlMedium
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlThin
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With

'                ''''''''''''''''''''''''''''''''''
'                currentRow += 1
'                ' по количеству контролов в этапе
'                controlsCount = ControlsForPhase.Item(StageNames(I)).Count

'                J = 0
'                If controlsCount > 0 Then
'                    For Each keyControl As MDBControlLibrary.IUserControl In ControlsForPhase.Item(StageNames(I)).Values
'                        If keyControl.Name.IndexOf("Описание") = -1 Then
'                            J += 1
'                            ' имя канала
'                            .Range(.Cells(currentRow, currentColumn), .Cells(currentRow, currentColumn)).Select()
'                            With .Selection.Interior
'                                .ColorIndex = 15
'                                .Pattern = Excel.Constants.xlSolid
'                            End With

'                            With .Selection
'                                .HorizontalAlignment = Excel.Constants.xlCenter
'                                .VerticalAlignment = Excel.Constants.xlTop
'                                .WrapText = True
'                                .Orientation = 0
'                                .AddIndent = False
'                                .IndentLevel = 0
'                                .ShrinkToFit = False
'                                .ReadingOrder = Excel.Constants.xlContext
'                                .MergeCells = False
'                            End With
'                            .ActiveCell.FormulaR1C1 = keyControl.Text '"Имя контрола"

'                            With .ActiveCell.Characters(Start:=1, Length:=Len(CStr(.ActiveCell.Value))).Font
'                                .Name = "Arial"
'                                .FontStyle = "обычный"
'                                .Size = 10
'                                .Strikethrough = False
'                                .Superscript = False
'                                .Subscript = False
'                                .OutlineFont = False
'                                .Shadow = False
'                                .Underline = Excel.XlUnderlineStyle.xlUnderlineStyleNone
'                                .ColorIndex = Excel.Constants.xlAutomatic
'                            End With

'                            ' значение канала
'                            .Range(.Cells(currentRow + 1, currentColumn), .Cells(currentRow + 1, currentColumn)).Select()
'                            keyControl.Row = currentRow + 1
'                            keyControl.Col = currentColumn
'                            .ActiveCell.FormulaR1C1 = "0" 'keyControl.ЗначениеПользователя 'значение контрола

'                            ' выделенный блок жирным
'                            .Range(.Cells(currentRow + 1, currentColumn), .Cells(currentRow, currentColumn)).Select()
'                            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
'                            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone

'                            With .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
'                                .LineStyle = Excel.XlLineStyle.xlContinuous
'                                .Weight = Excel.XlBorderWeight.xlThin
'                                .ColorIndex = Excel.Constants.xlAutomatic
'                            End With
'                            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
'                                .LineStyle = Excel.XlLineStyle.xlContinuous
'                                .Weight = Excel.XlBorderWeight.xlThin
'                                .ColorIndex = Excel.Constants.xlAutomatic
'                            End With
'                            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
'                                .LineStyle = Excel.XlLineStyle.xlContinuous
'                                .Weight = Excel.XlBorderWeight.xlThin
'                                .ColorIndex = Excel.Constants.xlAutomatic
'                            End With
'                            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
'                                .LineStyle = Excel.XlLineStyle.xlContinuous
'                                .Weight = Excel.XlBorderWeight.xlThin
'                                .ColorIndex = Excel.Constants.xlAutomatic
'                            End With
'                            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
'                                .LineStyle = Excel.XlLineStyle.xlContinuous
'                                .Weight = Excel.XlBorderWeight.xlThin
'                                .ColorIndex = Excel.Constants.xlAutomatic
'                            End With

'                            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
'                            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
'                            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
'                                .LineStyle = Excel.XlLineStyle.xlContinuous
'                                .Weight = Excel.XlBorderWeight.xlMedium
'                                .ColorIndex = Excel.Constants.xlAutomatic
'                            End With
'                            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
'                                .LineStyle = Excel.XlLineStyle.xlContinuous
'                                .Weight = Excel.XlBorderWeight.xlMedium
'                                .ColorIndex = Excel.Constants.xlAutomatic
'                            End With
'                            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
'                                .LineStyle = Excel.XlLineStyle.xlContinuous
'                                .Weight = Excel.XlBorderWeight.xlMedium
'                                .ColorIndex = Excel.Constants.xlAutomatic
'                            End With
'                            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
'                                .LineStyle = Excel.XlLineStyle.xlContinuous
'                                .Weight = Excel.XlBorderWeight.xlMedium
'                                .ColorIndex = Excel.Constants.xlAutomatic
'                            End With

'                            currentColumn += 1

'                            ' только 8 контролов в строке
'                            If J Mod 8 = 0 AndAlso J <> controlsCount Then
'                                currentColumn = 2
'                                currentRow += 2
'                            End If
'                        End If
'                    Next
'                    currentRow += 3
'                End If
'            Next I

'            ColorIndex = 4 'Зеленый'  33 'синий
'            title = "Исходные данные (измеренная информация)"
'            ' Измеренные
'            Dim resultMeasurementRows = From row In varProjectManager.MeasurementDataTable Select row

'            For Each itemDataRow In resultMeasurementRows
'                MeasurementRows.Add(itemDataRow)
'            Next

'            PrintParameters(excelApp, MeasurementRows, TypeParameter.Measurement)

'            '****************************************************************
'            ColorIndex = 8 'голубой
'            title = "Физические и приведенные параметры"
'            ' Приведение 
'            Dim resultCastRows = From row In varProjectManager.CalculatedDataTable
'                                 Where row.ПриведеныйПараметр = True
'                                 Select row

'            For Each itemDataRow In resultCastRows
'                CastRows.Add(itemDataRow)
'            Next

'            PrintParameters(excelApp, CastRows, TypeParameter.PhysicalCast)

'            '****************************************************************
'            ColorIndex = 45 'оранжевый
'            title = "Пересчитанные параметры"
'            ' Пересчет
'            Dim resultConvertingRows = From row In varProjectManager.CalculatedDataTable
'                                       Where row.ПриведеныйПараметр = False
'                                       Select row

'            For Each itemDataRow In resultConvertingRows
'                ConvertingRows.Add(itemDataRow)
'            Next

'            PrintParameters(excelApp, ConvertingRows, TypeParameter.Converting)

'            ' оставить позицию курсора в конце
'            .Range(.Cells(currentRow, 1), .Cells(currentRow, 1)).Select()
'            Try
'                ' колонтитулы если есть принтер
'                With .ActiveSheet.PageSetup
'                    .LeftHeader = "АО НПЦГ ""Салют"", цех №6"
'                    .CenterHeader = "Подсчет параметров"
'                    .RightHeader = "&D  &T"
'                    .LeftFooter = ""
'                    .CenterFooter = "&P из &N"
'                    .RightFooter = ""
'                    .LeftMargin = .Application.InchesToPoints(0.78740157480315)
'                    .RightMargin = .Application.InchesToPoints(0.78740157480315)
'                    .TopMargin = .Application.InchesToPoints(0.984251968503937)
'                    .BottomMargin = .Application.InchesToPoints(0.984251968503937)
'                    .HeaderMargin = .Application.InchesToPoints(0.511811023622047)
'                    .FooterMargin = .Application.InchesToPoints(0.511811023622047)
'                    .PrintHeadings = False
'                    .PrintGridlines = False
'                    .PrintComments = Excel.XlPrintLocation.xlPrintNoComments
'                    .PrintQuality = 600
'                    .CenterHorizontally = False
'                    .CenterVertically = False
'                    .Orientation = Excel.XlPageOrientation.xlPortrait
'                    .Draft = False
'                    .PaperSize = Excel.XlPaperSize.xlPaperA4
'                    .FirstPageNumber = Excel.Constants.xlAutomatic
'                    .Order = Excel.XlOrder.xlDownThenOver
'                    .BlackAndWhite = False
'                    .Zoom = 100
'                    .PrintErrors = Excel.XlPrintErrors.xlPrintErrorsDisplayed
'                End With
'            Catch ex As Exception
'            Finally
'                For I As Integer = .Sheets.Count To 2 Step -1
'                    .Sheets(I).Select()
'                    .ActiveWindow.SelectedSheets.Delete()
'                Next

'                '.Sheets("Лист2").Select()
'                '.ActiveWindow.SelectedSheets.Delete()
'                '.Sheets("Лист3").Select()
'                '.ActiveWindow.SelectedSheets.Delete()
'                With .ActiveWindow
'                    .DisplayGridlines = False
'                    .DisplayHeadings = False
'                End With

'                .Application.ScreenUpdating = True

'                'записать созданный файл
'                Dim templateCalculateKT As New FileInfo(PathTemplateCalculateKT)

'                If templateCalculateKT.Exists Then templateCalculateKT.Delete()

'                '.ActiveWorkbook.SaveAs(Filename:=PathTemplateCalculateKT,
'                '    FileFormat:=Excel.XlWindowState.xlNormal, Password:="", WriteResPassword:="",
'                '    ReadOnlyRecommended:=False, CreateBackup:=False)

'                .ActiveWorkbook.SaveAs(Filename:=PathTemplateCalculateKT, FileFormat:=Excel.XlFileFormat.xlOpenXMLWorkbook, CreateBackup:=False)
'                .Quit()

'                'If Me.FileFormat = Excel.XlFileFormat.xlWorkbookNormal Then
'                '    Me.SaveAs(Me.Path & "\XMLCopy.xls", _
'                '        Excel.XlFileFormat.xlXMLSpreadsheet, _
'                '        AccessMode:=Excel.XlSaveAsAccessMode.xlNoChange)
'                'End If
'            End Try
'        End With

'        excelApp = Nothing
'        GC.Collect()
'    End Sub

'    ''' <summary>
'    ''' Обработка Собранных Параметров
'    ''' </summary>
'    ''' <param name="excelApp"></param>
'    ''' <param name="DataRowParameters"></param>
'    ''' <param name="inTypeParameter"></param>
'    Private Sub PrintParameters(ByRef excelApp As Excel.Application, ByRef DataRowParameters As List(Of DataRow), ByVal inTypeParameter As TypeParameter)
'        Dim J As Integer
'        Dim rowCount As Integer = DataRowParameters.Count ' Количество Параметров Подсчета

'        If rowCount > 0 Then
'            With excelApp
'                currentColumn = 2
'                startBlock = currentRow
'                ' заголовок текущего блока
'                .Range(.Cells(currentRow, currentColumn), .Cells(currentRow, currentColumn + 8)).Select()
'                With .Selection
'                    .HorizontalAlignment = Excel.Constants.xlCenter
'                    .VerticalAlignment = Excel.Constants.xlCenter
'                    .WrapText = False
'                    .Orientation = 0
'                    .AddIndent = False
'                    .IndentLevel = 0
'                    .ShrinkToFit = False
'                    .ReadingOrder = Excel.Constants.xlContext
'                    .MergeCells = True
'                End With
'                With .Selection.Font
'                    .Name = "Arial"
'                    .FontStyle = "полужирный"
'                    .Size = 10
'                    .Strikethrough = False
'                    .Superscript = False
'                    .Subscript = False
'                    .OutlineFont = False
'                    .Shadow = False
'                    .Underline = Excel.XlUnderlineStyle.xlUnderlineStyleNone
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
'                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlThin
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlThin
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlThin
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlThin
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.Constants.xlNone
'                With .Selection.Interior
'                    '            .ColorIndex = 4
'                    .Pattern = Excel.Constants.xlSolid
'                    .PatternColorIndex = Excel.Constants.xlAutomatic
'                End With
'                ' цвет и заголовок
'                .Selection.Interior.ColorIndex = ColorIndex
'                .ActiveCell.FormulaR1C1 = title

'                PrintTitleParameterValueUnit(excelApp)
'                currentColumn = 3
'                currentRow = indexRowValue

'                For Each dataRowParameter As DataRow In DataRowParameters
'                    J += 1
'                    ' Имя параметра
'                    .Range(.Cells(currentRow, currentColumn), .Cells(currentRow, currentColumn)).Select()
'                    With .Selection
'                        .HorizontalAlignment = Excel.Constants.xlCenter
'                        .VerticalAlignment = Excel.Constants.xlTop
'                        .WrapText = True
'                        .Orientation = 0
'                        .AddIndent = False
'                        .IndentLevel = 0
'                        .ShrinkToFit = False
'                        .ReadingOrder = Excel.Constants.xlContext
'                        .MergeCells = False
'                    End With
'                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
'                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
'                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
'                        .LineStyle = Excel.XlLineStyle.xlContinuous
'                        .Weight = Excel.XlBorderWeight.xlThin
'                        .ColorIndex = Excel.Constants.xlAutomatic
'                    End With
'                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
'                        .LineStyle = Excel.XlLineStyle.xlContinuous
'                        .Weight = Excel.XlBorderWeight.xlThin
'                        .ColorIndex = Excel.Constants.xlAutomatic
'                    End With
'                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
'                        .LineStyle = Excel.XlLineStyle.xlContinuous
'                        .Weight = Excel.XlBorderWeight.xlThin
'                        .ColorIndex = Excel.Constants.xlAutomatic
'                    End With
'                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
'                        .LineStyle = Excel.XlLineStyle.xlContinuous
'                        .Weight = Excel.XlBorderWeight.xlThin
'                        .ColorIndex = Excel.Constants.xlAutomatic
'                    End With
'                    With .Selection.Interior
'                        .ColorIndex = 15
'                        .Pattern = Excel.Constants.xlSolid
'                        .PatternColorIndex = Excel.Constants.xlAutomatic
'                    End With

'                    ' ИмяПараметра
'                    .ActiveCell.FormulaR1C1 = dataRowParameter("ИмяПараметра")
'                    With .ActiveCell.Characters(Start:=1, Length:=8).Font
'                        .Name = "Arial"
'                        .FontStyle = "обычный"
'                        .Size = 10
'                        .Strikethrough = False
'                        .Superscript = False
'                        .Subscript = False
'                        .OutlineFont = False
'                        .Shadow = False
'                        .Underline = Excel.XlUnderlineStyle.xlUnderlineStyleNone
'                        .ColorIndex = Excel.Constants.xlAutomatic
'                    End With
'                    '           иэмеренное значение
'                    .Range(.Cells(currentRow + 1, currentColumn), .Cells(currentRow + 1, currentColumn)).Select()
'                    With .Selection
'                        .HorizontalAlignment = Excel.Constants.xlCenter
'                        .VerticalAlignment = Excel.Constants.xlCenter
'                        .WrapText = False
'                        .Orientation = 0
'                        .AddIndent = False
'                        .IndentLevel = 0
'                        .ShrinkToFit = False
'                        .ReadingOrder = Excel.Constants.xlContext
'                        .MergeCells = False
'                    End With
'                    With .Selection.Font
'                        .Name = "Arial"
'                        .FontStyle = "полужирный"
'                        .Size = 10
'                        .Strikethrough = False
'                        .Superscript = False
'                        .Subscript = False
'                        .OutlineFont = False
'                        .Shadow = False
'                        .Underline = Excel.XlUnderlineStyle.xlUnderlineStyleNone
'                        .ColorIndex = Excel.Constants.xlAutomatic
'                    End With
'                    With .Selection.Interior
'                        .ColorIndex = 36
'                        .Pattern = Excel.Constants.xlSolid
'                        .PatternColorIndex = Excel.Constants.xlAutomatic
'                    End With

'                    dataRowParameter("Row") = currentRow + 1
'                    dataRowParameter("Col") = currentColumn

'                    .ActiveCell.FormulaR1C1 = "0.00"

'                    ' размерность измеренной величины
'                    .Range(.Cells(currentRow + 2, currentColumn), .Cells(currentRow + 2, currentColumn)).Select()

'                    With .Selection
'                        .HorizontalAlignment = Excel.Constants.xlCenter
'                        .VerticalAlignment = Excel.Constants.xlCenter
'                        .WrapText = False
'                        .Orientation = 0
'                        .AddIndent = False
'                        .IndentLevel = 0
'                        .ShrinkToFit = False
'                        .ReadingOrder = Excel.Constants.xlContext
'                        .MergeCells = False
'                    End With
'                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
'                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
'                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
'                        .LineStyle = Excel.XlLineStyle.xlContinuous
'                        .Weight = Excel.XlBorderWeight.xlThin
'                        .ColorIndex = Excel.Constants.xlAutomatic
'                    End With
'                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
'                        .LineStyle = Excel.XlLineStyle.xlContinuous
'                        .Weight = Excel.XlBorderWeight.xlThin
'                        .ColorIndex = Excel.Constants.xlAutomatic
'                    End With
'                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
'                        .LineStyle = Excel.XlLineStyle.xlContinuous
'                        .Weight = Excel.XlBorderWeight.xlThin
'                        .ColorIndex = Excel.Constants.xlAutomatic
'                    End With
'                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
'                        .LineStyle = Excel.XlLineStyle.xlContinuous
'                        .Weight = Excel.XlBorderWeight.xlThin
'                        .ColorIndex = Excel.Constants.xlAutomatic
'                    End With

'                    If inTypeParameter = TypeParameter.Measurement Then
'                        .ActiveCell.FormulaR1C1 = dataRowParameter("РазмерностьВходная")
'                    Else
'                        .ActiveCell.FormulaR1C1 = dataRowParameter("РазмерностьВыходная")
'                    End If

'                    ' форматировать выделенный блок
'                    .Range(.Cells(currentRow, currentColumn), .Cells(currentRow + 2, currentColumn)).Select()

'                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
'                    .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
'                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
'                        .LineStyle = Excel.XlLineStyle.xlContinuous
'                        .Weight = Excel.XlBorderWeight.xlThin
'                        .ColorIndex = Excel.Constants.xlAutomatic
'                    End With
'                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
'                        .LineStyle = Excel.XlLineStyle.xlContinuous
'                        .Weight = Excel.XlBorderWeight.xlThin
'                        .ColorIndex = Excel.Constants.xlAutomatic
'                    End With
'                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
'                        .LineStyle = Excel.XlLineStyle.xlContinuous
'                        .Weight = Excel.XlBorderWeight.xlThin
'                        .ColorIndex = Excel.Constants.xlAutomatic
'                    End With
'                    With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
'                        .LineStyle = Excel.XlLineStyle.xlContinuous
'                        .Weight = Excel.XlBorderWeight.xlThin
'                        .ColorIndex = Excel.Constants.xlAutomatic
'                    End With
'                    With .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
'                        .LineStyle = Excel.XlLineStyle.xlContinuous
'                        .Weight = Excel.XlBorderWeight.xlThin
'                        .ColorIndex = Excel.Constants.xlAutomatic
'                    End With

'                    currentColumn = currentColumn + 1
'                    ' не более 8 параметров в строке
'                    If J Mod 8 = 0 AndAlso J <> rowCount Then
'                        currentRow += 2
'                        PrintTitleParameterValueUnit(excelApp)
'                        currentColumn = 3
'                        currentRow = indexRowValue
'                    End If
'                Next

'                currentRow += 3
'                ' пустая строка
'                .Range(.Cells(currentRow, 2), .Cells(currentRow, 10)).Select()
'                .Selection.Merge()
'                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
'                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlThin
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlThin
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlThin
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlThin
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.Constants.xlNone

'                ' форматировать блок
'                .Range(.Cells(startBlock, 2), .Cells(currentRow, 10)).Select()
'                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
'                .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlMedium
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlMedium
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlMedium
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With
'                With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
'                    .LineStyle = Excel.XlLineStyle.xlContinuous
'                    .Weight = Excel.XlBorderWeight.xlMedium
'                    .ColorIndex = Excel.Constants.xlAutomatic
'                End With

'                currentRow += 1
'            End With
'        End If
'    End Sub

'    ''' <summary>
'    ''' Заголовок Параметр Значение Размерность
'    ''' </summary>
'    ''' <param name="excelApp"></param>
'    Private Sub PrintTitleParameterValueUnit(ByRef excelApp As Excel.Application)
'        currentColumn = 2
'        currentRow += 1
'        indexRowValue = currentRow

'        With excelApp
'            .Range(.Cells(currentRow, currentColumn), .Cells(currentRow, currentColumn)).Select()
'            .ActiveCell.FormulaR1C1 = "Параметр"
'            With .Selection
'                .HorizontalAlignment = Excel.Constants.xlCenter
'                .VerticalAlignment = Excel.Constants.xlCenter
'                .WrapText = False
'                .Orientation = 0
'                .AddIndent = False
'                .IndentLevel = 0
'                .ShrinkToFit = False
'                .ReadingOrder = Excel.Constants.xlContext
'                .MergeCells = False
'            End With

'            currentRow += 1
'            .Range(.Cells(currentRow, currentColumn), .Cells(currentRow, currentColumn)).Select()
'            .ActiveCell.FormulaR1C1 = "Значение"

'            currentRow += 1
'            .Range(.Cells(currentRow, currentColumn), .Cells(currentRow, currentColumn)).Select()
'            .ActiveCell.FormulaR1C1 = "Размерность"

'            .Range(.Cells(currentRow - 2, currentColumn), .Cells(currentRow, currentColumn)).Select()
'            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
'            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
'            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
'                .LineStyle = Excel.XlLineStyle.xlContinuous
'                .Weight = Excel.XlBorderWeight.xlThin
'                .ColorIndex = Excel.Constants.xlAutomatic
'            End With
'            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
'                .LineStyle = Excel.XlLineStyle.xlContinuous
'                .Weight = Excel.XlBorderWeight.xlThin
'                .ColorIndex = Excel.Constants.xlAutomatic
'            End With
'            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
'                .LineStyle = Excel.XlLineStyle.xlContinuous
'                .Weight = Excel.XlBorderWeight.xlThin
'                .ColorIndex = Excel.Constants.xlAutomatic
'            End With
'            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
'                .LineStyle = Excel.XlLineStyle.xlContinuous
'                .Weight = Excel.XlBorderWeight.xlThin
'                .ColorIndex = Excel.Constants.xlAutomatic
'            End With
'            With .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
'                .LineStyle = Excel.XlLineStyle.xlContinuous
'                .Weight = Excel.XlBorderWeight.xlThin
'                .ColorIndex = Excel.Constants.xlAutomatic
'            End With
'        End With
'    End Sub
'End Module
