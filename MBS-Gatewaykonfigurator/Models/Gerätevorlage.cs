namespace MBS_Gatewaykonfigurator.Models;

using MBS_Gatewaykonfigurator.Models.BACnet;
using MBS_Gatewaykonfigurator.Models.MBS;
using MBS_Gatewaykonfigurator.Models.MBUS;
using MBS_Gatewaykonfigurator.Models.Modbus;
using System.ComponentModel.DataAnnotations;
using System.Text;

public class Gerätevorlage
{
    [Key, Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Key, Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public Guid ProjektId { get; set; }

    [Required]
    public string QuellProtokoll { get; set; } = string.Empty;

    [Required]
    public string ZielProtokoll { get; set; } = string.Empty;

    public List<GerätevorlageElement> GerätevorlageElements { get; set; } = new List<GerätevorlageElement>();

    // Gibt die letzte Adresse zurück, nachdem alle BACnet-Adressen gesetzt wurden
    public uint getLastAdressSetFirstAdressBacNet(uint adress)
    {
        for (int i = 0; i < GerätevorlageElements.Count; i++)
        {
            var element = GerätevorlageElements[i];

            if (element.Ziel is IDispatchable zielObj)
            {
                if (element.Ziel is BacNet bacNet)
                {
                    //NC Ausnahme statische Objektnummer
                    if (bacNet.TypBacNet == BacNet.Types.NC)
                    {
                        continue;
                    }
                    else
                    {
                        bacNet.ObjektNummer = adress;
                        adress++;
                    }
                }
            }
            else
            {
                throw new InvalidCastException($"Ziel von Element {i} implementiert IDispatchable nicht");
            }
        }

        return adress;
    }

    public void setNameDescriptionTagAdress(string name, string description, string tag, Mbs sourceAdressObject)
    {

        for (int i = 0; i < GerätevorlageElements.Count; i++)
        {
            var element = GerätevorlageElements[i];

            //Quelle
            if (element.Quelle is Mbs mbs)
            {
                //Ausnahme bei der Quelle wird die Beschreibung angezeigt nicht der Name der AT-Funktion
                mbs.Name = description;
                //Ausnahme bei der Quelle wird die Beschreibung angezeigt nicht der Name der AT-Funktion
                mbs.NamenAppendix = element.BeschreibungAppendix;
            }
            else
            {
                throw new InvalidCastException($"Quelle von Element {GerätevorlageElements[i].BeschreibungAppendix} erbt nicht MBS");
            }

            if (element.Quelle is Mbus mbus && sourceAdressObject is Mbus srcMbs)
            {
                mbus.SlaveGeräteAdresse = srcMbs.SlaveGeräteAdresse;
            }
            else if (element.Quelle is ModbusMasterSeriell modbusMasterSeriell && sourceAdressObject is ModbusMasterSeriell srcModbusMasterSeriell)
            {
                modbusMasterSeriell.SlaveGeräteAdresse = srcModbusMasterSeriell.SlaveGeräteAdresse;
            }
            else if (element.Quelle is ModbusTcpIp modbusTcpIp && sourceAdressObject is ModbusTcpIp srcModbusTcpIp)
            {
                modbusTcpIp.Host = srcModbusTcpIp.Host;
                modbusTcpIp.ServerAdresse = srcModbusTcpIp.ServerAdresse;
                modbusTcpIp.Port = srcModbusTcpIp.Port;
            }
            else
            {
                throw new InvalidCastException($"Quelle von Element {GerätevorlageElements[i].BeschreibungAppendix} stimmt nicht mit QuellAdressObjekt überein");
            }

            //Ziel
            if (element.Ziel is BacNet bacNet)
            {
                bacNet.Name = name;
                bacNet.NamenAppendix = element.NamenAppendix;
                bacNet.BacDescription = description;
                bacNet.BeschreibungAppendix = element.BeschreibungAppendix;
                bacNet.Tag = tag;
            }
            else
            {
                throw new InvalidCastException($"Quelle von Element {GerätevorlageElements[i].BeschreibungAppendix} ist nicht BACnet");
            }
        }
    }

    public string toStringDispatch()
    {
        var sb = new StringBuilder();

        for (int i = 0; i < GerätevorlageElements.Count; i++)
        {
            var element = GerätevorlageElements[i];
            sb.AppendLine(element.toStringDispatch());
        }

        return sb.ToString();
    }

    public string toStringPlant(uint tagIndex, out uint tagIndexOut)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < GerätevorlageElements.Count; i++)
        {
            var element = GerätevorlageElements[i];
            if (element.Ziel is BacNet bacnet)
            {
                if (!string.IsNullOrWhiteSpace(bacnet.Tag))
                {

                    sb.AppendLine("[" + bacnet.Tag + "]");
                    sb.AppendLine("id = " + tagIndex);
                    sb.AppendLine("name = " + bacnet.BacDescription);
                    sb.AppendLine();
                    tagIndex++;
                }
            }
        }

        tagIndexOut = tagIndex;

        return sb.ToString();
    }


    public string QuellProtokollToStringFile()
    {
        var sb = new StringBuilder();

        for (int i = 0; i < GerätevorlageElements.Count; i++)
        {
            var element = GerätevorlageElements[i];

            if (element.Quelle is IDispatchable quellObj)
            {
                sb.AppendLine(quellObj.toStringFile());
            }
            else
            {
                throw new InvalidCastException($"Quelle von Element {i} implementiert IDispatchable nicht");
            }
        }

        return sb.ToString();
    }

    public string ZielProtokollToStringFile()
    {
        var sb = new StringBuilder();

        for (int i = 0; i < GerätevorlageElements.Count; i++)
        {
            var element = GerätevorlageElements[i];

            if (element.Ziel is IDispatchable zielObj)
            {
                sb.AppendLine(zielObj.toStringFile());
            }
            else
            {
                throw new InvalidCastException($"Ziel von Element {i} implementiert IDispatchable nicht");
            }
        }

        return sb.ToString();
    }
}
