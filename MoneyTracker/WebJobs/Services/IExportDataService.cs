using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebJobs.Services
{
    public interface IExportDataService
    {
        Task ExportCsvAsync(Guid accountId);
    }
}
