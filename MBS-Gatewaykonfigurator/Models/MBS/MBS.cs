namespace MBS_Gatewaykonfigurator.Models.MBS;
using global::System.ComponentModel.DataAnnotations;
using global::System.Text;



public class Mbs : ElementAppendix
{

    [Required]
    public string TreiberName { get; }
    [Required]
    public uint RoutingAdresse { get; }
    [Required]
    public string DatenPunktDatei { get; }

    public Mbs(string treiberName, uint routingAdresse, string datenPunktDatei)
    {
        TreiberName = treiberName;
        RoutingAdresse = routingAdresse;
        DatenPunktDatei = datenPunktDatei;
    }

    [Required]
    public TypesMBS TypMbs { get; set; } = TypesMBS.S;

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Format { get; set; }

    public string? Query { get; set; } = "pe";

    public string? WriteCache { get; set; }

    public string? ZusätzlicheConfigs { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(Name))
            sb.AppendLine($"name = {Name}{NamenAppendix}");

        if (!string.IsNullOrWhiteSpace(Format))
            sb.AppendLine($"format = {Format}");

        if (!string.IsNullOrWhiteSpace(Query))
            sb.AppendLine($"query = {Query}");

        if (!string.IsNullOrWhiteSpace(WriteCache))
            sb.AppendLine($"writecache = {WriteCache}");

        if (!string.IsNullOrWhiteSpace(ZusätzlicheConfigs))
            sb.AppendLine($"{ZusätzlicheConfigs}");

        return sb.ToString();
    }

    public enum TypesMBS
    {
        M,
        S,
        X,
        Y,
        A //option: disabled im GUI
    }
}






