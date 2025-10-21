namespace MBS_Gatewaykonfigurator.Models.MBUS;
using global::System;
using global::System.ComponentModel.DataAnnotations;
using global::System.Text;
using MBS_Gatewaykonfigurator.Models.MBS;

public class Mbus : Mbs, IDispatchable
{
    //MBS
    public Mbus() : base("mbus", 60, "mbus1.txt")
    {
    }

    //mbus

    //<slave>
    [Required, RegularExpression(@"^(S\d{8}|P([1-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|250))$", ErrorMessage = "Nur Formate P1–P250 oder S00000000–S99999999 sind erlaubt.")]
    public string SlaveGeräteAdresse { get; set; } = string.Empty;

    //<typ>
    [Required]
    public Types Typ { get; set; } = Types.value;

    //<nr>
    //0-127 / sBit 1-8
    [Range(0, 127, ErrorMessage = "Die Adresse muss zwischen 1 und 127 liegen für  \"value\" oder  \"vif\", für \"sBit\" zwischen 1 und 8 , sonst muss das Feld leer sein.")]
    public byte? RegisterAdresse { get; set; }

    //<format>
    private MbusFormat? _format;

    // Quelle: https://learn.microsoft.com/de-de/dotnet/csharp/programming-guide/classes-and-structs/using-properties
    public MbusFormat? FormatMbus
    {
        get => _format;
        set
        {
            _format = value;
            base.Format = value.ToString(); // automatisches aktualisieren der base Klasse
        }
    }

    public string toStringFile()
    {

        // [60.S mbus P5 Value 4]
        //query = pe
        //format = k
        //..


        var sb = new StringBuilder();
        sb.AppendLine("[" + toStringDriver() + "]");
        sb.Append(base.ToString());

        return sb.ToString();
    }

    private string _dispatch = string.Empty;

    public string toStringDriver()
    {
        //[S P5 Value 4]
        //base.Typ, SlaveGeräteAdresse, Register, [nr]

        //[S P5
        string prefix = base.TypMbs.ToString() + " " + SlaveGeräteAdresse.ToString();
        //[60.S mbus P5
        string prefixDispatch = base.RoutingAdresse.ToString() + "." + base.TypMbs.ToString() + " " + base.TreiberName.ToString() + " " + SlaveGeräteAdresse.ToString();

        //Typ, ohne <nr>
        if (Typ.Equals(Types.failure) || Typ.Equals(Types.ident) || Typ.Equals(Types.manufacturer) || Typ.Equals(Types.medium) || Typ.Equals(Types.accesno) || Typ.Equals(Types.signature) || Typ.Equals(Types.version) || Typ.Equals(Types.status))
        {
            _dispatch = prefixDispatch + " " + Typ.ToString();
            return prefix + " " + Typ.ToString();
        }

        //"sBit" <1-8>
        else if (Typ.Equals(Types.sBit))
        {
            if (Typ.Equals(Types.sBit) && (RegisterAdresse > 8 || RegisterAdresse < 1))
            {
                throw new ArgumentOutOfRangeException(nameof(RegisterAdresse), "sBit darf nur den Wert zwischen 1 und 8 haben.");
            }
            _dispatch = prefixDispatch + " " + Typ.ToString() + " " + RegisterAdresse.ToString();
            return prefix + " " + Typ.ToString() + " " + RegisterAdresse.ToString();
        }

        //Typ mit <nr>

        else
        {
            if (RegisterAdresse == null)
            {
                throw new ArgumentNullException(nameof(RegisterAdresse), "RegisterAddress darf nicht null sein.");
            }
            if (Typ.Equals(Types.sBit) && ((RegisterAdresse > 8 || RegisterAdresse < 1) || RegisterAdresse == null))
            {
                throw new ArgumentOutOfRangeException(nameof(RegisterAdresse), "sBit darf nur den Wert zwischen 1 und 8 haben.");

            }
            _dispatch = prefixDispatch + " " + Typ.ToString() + " " + RegisterAdresse.ToString();
            return prefix + " " + Typ.ToString() + " " + RegisterAdresse.ToString();
        }

    }

    public string toStringDispatch()
    {
        toStringDriver(); //Dispatch String erstellen
        return _dispatch;
    }



    public enum Types
    {
        failure,
        value,
        vif,
        ident,
        manufacturer,
        medium,
        accesno,
        signature,
        version,
        status,
        sBit //chase sensitive
    }
    public enum MbusFormat
    {
        u,
        m,
        k,
        M,
        G,
    }

}
