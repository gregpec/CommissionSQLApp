using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionApp.Services.FilesServices.JsonFile
{
    public interface IJsonServices
    {
        void ExportToJsonFileSqlRepo();
        void LoadDataFromJsonFiles();
    }
}
