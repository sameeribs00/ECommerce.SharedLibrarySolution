
using System.Reflection;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ECommerece.CommonLibrary.Helpers.ExcelHelper
{
    public class ExcelExportService : IExcelExportService
    {
        public async Task<byte[]> ExportToExcel<T>(IEnumerable<T> data, string sheetName = "Sheet1", string fileName = "Export")
        {
            try
            {
                //Create the whole workbook
                using (var workbook = new XLWorkbook())
                {
                    //Create the worksheet
                    var ws = workbook.Worksheets.Add(sheetName);

                    //Get the generic data
                    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                              .Where(p => p.CanRead)
                                              .ToArray();
                    //Get headers based on the retrieved proprties 
                    for(int p = 0; p < properties.Length; p++)
                    {
                        ws.Cell(1, p + 1).Value = properties[p].Name;
                        ws.Cell(1, p + 1).Style.Font.Bold = true;
                        ws.Cell(1, p + 1).Style.Font.FontColor = XLColor.Black;
                    }

                    //Read the retrieved data
                    var dataList = data.ToList();
                    for(int row = 0; row < dataList.Count; row++)
                    {
                        for(int col = 0; col < properties.Length; col++)
                        {
                            var value = properties[col].GetValue(dataList[row]);
                            ws.Cell(row + 2,col + 1).Value = value?.ToString() ?? "";
                            ws.Cell(row + 2, col + 1).Style.Font.FontColor = XLColor.DarkGray;
                        }
                    }

                    //Adjust the column's width based on the cells content
                    ws.ColumnsUsed().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating Excel file: {ex.Message}", ex);
            }
        }
        public async Task<byte[]> ExportToExcelWithCustomHeaders<T>(IEnumerable<T> data, Dictionary<string, string> columnHeaders, string sheetName = "Sheet1")
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add(sheetName);

                    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                              .Where(p => p.CanRead)
                                              .ToArray();

                    for (int i = 0; i < properties.Length; i++)
                    {
                        var propertyName = properties[i].Name;
                        var headerName = columnHeaders.ContainsKey(propertyName) ? columnHeaders[propertyName] : propertyName;

                        ws.Cell(1, i + 1).Value = headerName;
                        ws.Cell(1, i + 1).Style.Font.Bold = true;
                        ws.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                    }

                    var dataList = data.ToList();
                    for (int row = 0; row < dataList.Count; row++)
                    {
                        for (int col = 0; col < properties.Length; col++)
                        {
                            var value = properties[col].GetValue(dataList[row]);
                            ws.Cell(row + 2, col + 1).Value = value?.ToString() ?? "";
                        }
                    }

                    ws.ColumnsUsed().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating Excel file with custom headers: {ex.Message}", ex);
            }
        }
    }
}
