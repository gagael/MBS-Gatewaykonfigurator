namespace MBS_Gatewaykonfigurator.Models;

using MBS_Gatewaykonfigurator.Models.BACnet;
using MBS_Gatewaykonfigurator.Models.MBS;
using System.ComponentModel.DataAnnotations;

public class DatenpunktGlobal
{
    [Key, Required]
    public string Name { get; set; } = string.Empty;

    public string Tag { get; set; } = string.Empty;
    [Required]
    public string Beschreibung { get; set; } = string.Empty;

    //von Gerätevorlage
    [Required]
    public string QuellProtokoll { get; set; } = string.Empty;

    [Required]
    public string ZielProtokoll { get; set; } = string.Empty;


    [Required]
    public Dispatch Dispatch { get; set; } = new();

    public string Zieladresse { get; set; } = "Auto";

    public Mbs? Quelle { get; set; }

    public Mbs? Ziel { get; set; }


    public DatenpunktGlobal()
    {
    }

    //von GerätevorlageElement
    public DatenpunktGlobal(string sourceTypeName, string destinationTypeName)
    {

        Quelle = CreateInstance(sourceTypeName);
        Ziel = CreateInstance(destinationTypeName)!;
        //immer BACnet in release 1.0
        ZielProtokoll = nameof(BacNet);
    }


    private Mbs? CreateInstance(string typeName)
    {
        if (string.IsNullOrWhiteSpace(typeName))
            throw new ArgumentException("Typname darf nicht leer sein", nameof(typeName));

        Type type;
        switch (typeName)
        {
            case "Keine Quelle":
                return null;
            case nameof(BacNet):
                type = typeof(BacNet);
                break;
            case nameof(SystemMbs):
                type = typeof(SystemMbs);
                break;
            default:
                throw new ArgumentException($"Typ '{typeName}' ist nicht erlaubt");
        }

        Mbs newObj = (Mbs)Activator.CreateInstance(type)!;

        return newObj;
    }






}
