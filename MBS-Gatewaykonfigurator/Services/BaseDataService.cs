namespace MBS_Gatewaykonfigurator.Services;
using Newtonsoft.Json;

public class BaseDataService<T> where T : class
{
    private readonly string dataDirectory;

    public BaseDataService(string folderName)
    {
        string dir = @"C:\SIGREN\MBS-Gatewaykonfigurator";
        dataDirectory = Path.Combine(dir, folderName);
        Directory.CreateDirectory(dataDirectory);
    }

    private static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.Auto,   // Wichtig für Polymorphie
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple // kürzere Typnamen
    };

    public async Task SaveAsync(List<T> items)
    {
        foreach (var item in items)
        {
            var id = GetId(item);
            if (id == Guid.Empty)
                continue;

            string file = Path.Combine(dataDirectory, $"{id}.json");
            //string json = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true });
            string json = JsonConvert.SerializeObject(item, jsonSettings);
            await File.WriteAllTextAsync(file, json);
        }
    }

    public async Task<List<T>> LoadAsync()
    {
        var result = new List<T>();

        if (!Directory.Exists(dataDirectory))
            return result;

        var files = Directory.GetFiles(dataDirectory, "*.json");

        foreach (var file in files)
        {
            try
            {
                var json = await File.ReadAllTextAsync(file);
                var obj = JsonConvert.DeserializeObject<T>(json, jsonSettings);
                if (obj != null)
                    result.Add(obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden der Datei {file}: {ex.Message}");
            }
        }

        return result;
    }

    public async Task DeleteAsync(string id)
    {
        var file = Path.Combine(dataDirectory, $"{id}.json");
        if (File.Exists(file))
            File.Delete(file);
    }

    public async Task SaveOneAsync(T item)
    {
        var id = GetId(item);
        if (id == Guid.Empty)
            throw new ArgumentException($"{typeof(T).Name} muss eine gültige Id haben");

        string file = Path.Combine(dataDirectory, $"{id}.json");
        string json = JsonConvert.SerializeObject(item, jsonSettings);
        await File.WriteAllTextAsync(file, json);
    }

    public async Task<T?> LoadOneAsync(Guid id)
    {
        string file = Path.Combine(dataDirectory, $"{id}.json");

        if (!File.Exists(file))
            return null;

        var json = await File.ReadAllTextAsync(file);
        return JsonConvert.DeserializeObject<T>(json, jsonSettings);
    }

    private Guid GetId(T obj)
    {
        var prop = typeof(T).GetProperty("Id");
        if (prop != null && prop.PropertyType == typeof(Guid))
        {
            return (Guid)(prop.GetValue(obj) ?? Guid.Empty);
        }
        return Guid.Empty;
    }
}

