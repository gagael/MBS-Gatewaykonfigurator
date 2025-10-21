namespace MBS_Gatewaykonfigurator.Models;

using MBS_Gatewaykonfigurator.Models.MBS;
using System.ComponentModel.DataAnnotations;

public class Datenpunkt
{
    [Key, Required]
    public string Name { get; set; } = string.Empty;

    public string Tag { get; set; } = string.Empty;
    [Required]
    public string Beschreibung { get; set; } = string.Empty;
    [Required]
    public string Gerätevorlage { get; set; } = string.Empty;
    [Required]
    public string Quelladresse { get; set; } = string.Empty;
    [Required]
    public string Zieladresse { get; set; } = "Auto";

    //Speichert die Adressen von Mbus, ModbusMasterSeriell und ModbusTcpIp
    public Mbs? Quelle { get; set; }

}
