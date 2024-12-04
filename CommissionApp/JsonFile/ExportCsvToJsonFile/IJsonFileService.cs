namespace CommissionApp.JsonFile.ExportCsvToJsonFile;
public interface IJsonFileService<T>
{
    List<T> LoadFromFile();
    void SaveToFile(IEnumerable<T> data);
}
