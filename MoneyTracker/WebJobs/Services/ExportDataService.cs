using System.ComponentModel;
using System.Data;
using System.Text;
using WebJobs.Services;

namespace WebJobs
{
    public class ExportDataService : IExportDataService
    {
        private readonly IAccountService _accountService;

        public ExportDataService(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public DataTable ConvertListToDataTable<T>(List<T> list)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public async Task ExportCsvAsync(Guid accountId)
        {
            string csvCompletePath = @"C:\Users\Iryna\source\repos\spa\Reports\" + accountId + ".csv";
            var list = _accountService.GetReport(accountId);
            var data = ConvertListToDataTable(list);
            var stringBuilder = new StringBuilder();

            IEnumerable<string> columnNames = data.Columns.Cast<DataColumn>().
                                  Select(column => column.ColumnName);
            stringBuilder.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in data.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                var newLine = string.Join(";", fields);
                stringBuilder.AppendLine(newLine);
            }
            await File.WriteAllTextAsync(csvCompletePath, stringBuilder.ToString());
        }
    }
}
