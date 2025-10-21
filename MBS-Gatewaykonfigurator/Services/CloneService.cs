using Newtonsoft.Json;

public static class CloneService
{
    // Globale Einstellungen für alle Klon-Vorgänge
    private static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.Auto,                     // für Polymorphie
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple, // kurze Typnamen
    };

    public static T? Clone<T>(T source)
    {
        if (source == null)
            return default;

        // Serialisieren
        var serialized = JsonConvert.SerializeObject(source, jsonSettings);

        // Deserialisieren → neues Objekt vom gleichen Typ
        return JsonConvert.DeserializeObject<T>(serialized, jsonSettings);
    }
}
