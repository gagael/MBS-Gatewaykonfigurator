namespace MBS_Gatewaykonfigurator.Models.Modbus;
using global::System;
using global::System.ComponentModel.DataAnnotations;
using global::System.Text;
using MBS_Gatewaykonfigurator.Models.MBS;

public class ModbusMasterSeriell : Mbs, IDispatchable
{
    //MBS
    //public MBS Mbs = new MBS("mod", 80, "modmster1");
    public ModbusMasterSeriell() : base("mod", 80, "modmster1.txt")
    {
    }

    //modbus

    //<slave>
    [Required, Range(0, 247, ErrorMessage = "Die Adresse muss zwischen 0 und 247 liegen.")]
    public ushort SlaveGeräteAdresse { get; set; }

    //<Register>
    [Required]
    public ModbusRegister RegisterTyp { get; set; } = ModbusRegister.coil;

    //<no>
    [Range(0, 65535, ErrorMessage = "Die Adresse muss zwischen 0 und 65535 liegen oder leer sein bei \"failure\".")]
    public ushort? RegisterAdresse { get; set; }

    //[<bit>]
    [Range(1, 16, ErrorMessage = "Das Bit muss zwischen 1 und 16 liegen oder leer sein.")]
    public byte? RegisterAdresseBit { get; set; }

    //<format>
    private ModbusFormat? _format;

    // Quelle: https://learn.microsoft.com/de-de/dotnet/csharp/programming-guide/classes-and-structs/using-properties
    public ModbusFormat? FormatModbus
    {
        get => _format;
        set
        {
            _format = value;
            base.Format = value.ToString(); // automatisches aktualisieren der base Klasse
        }
    }

    public enum ModbusFormat
    {
        u,
        ut,
        uh,
        um,
        u32,
        u32s,
        u64,
        c,
        ct,
        ch,
        cm,
        ci,
        f,
        fi,
        j,
        ji,
        d,
        dm,
        sbit06,
        sbit16,
        s,
        st,
        sh,
        sm,
        bcd,
    }

    public string toStringFile()
    {

        //[Y mod 13 holding 6]
        //query = pe
        //format = k
        //..


        var sb = new StringBuilder();
        sb.AppendLine("[" + toStringDriver() + "]");
        sb.Append(base.ToString());

        return sb.ToString();
    }

    //[80.Y mod 11 holding 6]
    private string _dispatch = string.Empty;

    public string toStringDriver()
    {
        //[Y 11 holding 6]
        string adresse = base.TypMbs.ToString() + " " + SlaveGeräteAdresse.ToString();
        //[80.Y mod 11 holding 6]
        _dispatch = base.RoutingAdresse.ToString() + "." + base.TypMbs.ToString() + " " + base.TreiberName.ToString() + " " + SlaveGeräteAdresse.ToString();

        //[Y 11 failure]
        if (RegisterTyp.Equals(ModbusRegister.failure))
        {
            _dispatch += " " + RegisterTyp.ToString();
            return adresse + " " + RegisterTyp.ToString();
        }

        //exception bei leerer RegisterAddresse
        if (RegisterAdresse == null)
        {
            throw new ArgumentNullException(nameof(RegisterAdresse), "RegisterAdresse darf nicht leer sein.");
        }

        //append Registertyp und Adresse
        _dispatch += " " + RegisterTyp.ToString() + " " + RegisterAdresse.ToString();
        adresse += " " + RegisterTyp.ToString() + " " + RegisterAdresse.ToString();

        //append optional bit
        adresse += (RegisterAdresseBit != null) ? "." + RegisterAdresseBit.ToString() : "";
        _dispatch += (RegisterAdresseBit != null) ? "." + RegisterAdresseBit.ToString() : "";

        return adresse;
    }

    public string toStringDispatch()
    {
        //Dispatch String erstellen
        toStringDriver();

        //[80.Y mod 13 holding 6]
        return _dispatch;

    }

    public enum ModbusRegister
    {
        coil = 1,
        status = 2,
        holding = 3,
        input = 4,
        failure = 5
    }

}
