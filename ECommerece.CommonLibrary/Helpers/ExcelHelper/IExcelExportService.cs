namespace ECommerece.CommonLibrary.Helpers.ExcelHelper
{
    public interface IExcelExportService
    {
        Task<byte[]> ExportToExcel<T>(IEnumerable<T> data, string sheetName = "Sheet1", string fileName = "Export");
        Task<byte[]> ExportToExcelWithCustomHeaders<T>(IEnumerable<T> data, Dictionary<string, string> columnHeaders, string sheetName = "Sheet1");
    }
}
