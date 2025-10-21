namespace MBS_Gatewaykonfigurator.Models;

using MBS_Gatewaykonfigurator.Models.BACnet;
using MBS_Gatewaykonfigurator.Models.MBS;
using MBS_Gatewaykonfigurator.Models.MBUS;
using MBS_Gatewaykonfigurator.Models.Modbus;
using System.Text;

public class GerätevorlageElement : ElementAppendix
{
    public MBS.Dispatch Dispatch { get; set; } = new();

    public Mbs? Quelle { get; set; }
    public Mbs? Ziel { get; set; }

    public GerätevorlageElement()
    {

    }

    // Konstruktor prüft, ob die Typen erlaubt sind
    public GerätevorlageElement(string sourceTypeName, string destinationTypeName)
    {

        Quelle = CreateInstance(sourceTypeName);
        Ziel = CreateInstance(destinationTypeName);
    }

    private Mbs CreateInstance(string typeName)
    {
        if (string.IsNullOrWhiteSpace(typeName))
            throw new ArgumentException("Typname darf nicht leer sein", nameof(typeName));

        Type type;
        switch (typeName)
        {
            case nameof(BacNet):
                type = typeof(BacNet);
                break;
            case nameof(Mbus):
                type = typeof(Mbus);
                break;
            case nameof(ModbusMasterSeriell):
                type = typeof(ModbusMasterSeriell);
                break;
            case nameof(ModbusTcpIp):
                type = typeof(ModbusTcpIp);
                break;
            default:
                throw new ArgumentException($"Typ '{typeName}' ist nicht erlaubt");
        }

        Mbs newObj = (Mbs)Activator.CreateInstance(type)!;

        if (newObj is not Mbs)
            throw new InvalidCastException($"Typ '{typeName}' implementiert Mbs nicht");

        return newObj;
    }

    public string toStringDispatch()
    {
        var sb = new StringBuilder();

        if (Ziel is BacNet banet)
            sb.AppendLine("# " + banet.BacDescription);

        if (Quelle is IDispatchable quellObj)
            sb.AppendLine("[" + quellObj.toStringDispatch() + "]");

        if (Ziel is IDispatchable zielObj)
            sb.AppendLine("target = " + zielObj.toStringDispatch());

        sb.Append(Dispatch.ToString());

        return sb.ToString();
    }
}
