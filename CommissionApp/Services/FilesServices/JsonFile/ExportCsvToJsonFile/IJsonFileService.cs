namespace CommissionApp.Services.FilesServices.JsonFile.ExportCsvToJsonFile;
public interface IJsonFileService<T>
{
    List<T> LoadFromFile();
    void SaveToFile(IEnumerable<T> data);
}
