namespace MBS_Gatewaykonfigurator.Models.Modbus;
using global::System;
using global::System.ComponentModel.DataAnnotations;
using global::System.Text;
using MBS_Gatewaykonfigurator.Models.MBS;

public class ModbusTcpIp : Mbs, IDispatchable
{
    //MBS
    //public MBS Mbs = new MBS("mbtcp", 860, "mbtcp1");
    public ModbusTcpIp() : base("mbtcp", 860, "mbtcp1.txt")
    {
    }

    //modbus
    //<ip / hostname>
    [Required]
    public string Host { get; set; } = "0.0.0.0";

    //<port>
    [Range(0, 65535, ErrorMessage = "Der Port muss zwischen 0 und 65535 liegen oder leer sein")]
    public uint? Port { get; set; }

    //<server/unit_ID>
    [Required, Range(1, 255, ErrorMessage = "Die Adresse muss zwischen 1 und 255 liegen oder leer sein bei \"failure\".")]
    public byte ServerAdresse { get; set; }

    //<Register>
    [Required]
    public ModbusRegister RegisterTyp { get; set; } = ModbusRegister.coil;

    //<no>
    [Range(0, 65535, ErrorMessage = "Die Adresse muss zwischen 0 und 65535 liegen oder leer sein bei \"failure\".")]
    public ushort? RegisterAdresse { get; set; }

    //[<bit>]
    [Range(0, 15, ErrorMessage = "Das Bit muss zwischen 0 und 15 liegen oder leer sein.")]
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
            base.Format = toStringFormat(); // automatisches aktualisieren der base Klasse
        }
    }

    public enum ModbusFormat
    {
        BIT,
        UINT8,
        UINT16,
        UINT32,
        UINT64,
        SINT8,
        SINT16,
        SINT32,
        SINT64,
        FLOAT32,
        FLOAT64,
        FIXED32,
        FIXED48,
    }

    //format = <format> [S:<swap>] [F:<fact>] [O:<ofs>] [M:<mode>]
    private Swaps? _swap;
    public Swaps? Swap
    {
        get => _swap;
        set
        {
            _swap = value;
            base.Format = toStringFormat(); // automatisches Aktualisieren
        }
    }

    private string? _factor;
    public string? Factor //format unbekannt float, int, unint64 etc -> string wurde gewählt
    {
        get => _factor;
        set
        {
            _factor = value;
            base.Format = toStringFormat();
        }
    }

    private string? _offset;
    public string? Offset //format unbekannt float, int, unint64 etc -> string wurde gewählt
    {
        get => _offset;
        set
        {
            _offset = value;
            base.Format = toStringFormat();
        }
    }

    private Modes? _mode;
    public Modes? Mode
    {
        get => _mode;
        set
        {
            _mode = value;
            base.Format = toStringFormat();
        }
    }
    private string toStringFormat()
    {
        if (FormatModbus == null) return string.Empty;

        string? format = FormatModbus.ToString();

        if (Swap != null)
        {
            format += " S:" + (int)Swap;
        }
        if (!string.IsNullOrWhiteSpace(Factor))
        {
            format += " F:" + Factor.ToString();
        }
        if (!string.IsNullOrWhiteSpace(Offset))
        {
            format += " O:" + Offset.ToString();
        }
        if (Mode != null)
        {
            format += " M:" + (int)Mode;
        }

        return format?.ToString() ?? string.Empty;

    }
    public enum Swaps
    {
        LittleEndian = 0,
        BigEndian = 1,
        LittleEndianReversed = 2,
        BigEndianReversed = 3,
    }



    public enum Modes
    {
        MultiWrite = 0,
        SingleWrite = 1,
        Masked = 2,
    }


    public string toStringFile()
    {

        //[80.Y mod 13 holding 6]
        //query = pe
        //format = k
        //..


        var sb = new StringBuilder();
        sb.AppendLine("[" + toStringDriver() + "]");
        base.Format = toStringFormat();
        sb.Append(base.ToString());

        return sb.ToString();
    }

    //[80.Y mod 11 holding 6]
    private string _dispatch = string.Empty;

    public string toStringDriver()
    {
        //[Y 0.0.0.0:502 11 holding 6.2] <ip>[:<port>] <server/unit_ID> <register> <no>[.<bit>]
        string adresse = base.TypMbs.ToString() + " " + Host.ToString();
        //[80.Y mod 0.0.0.0:502 11 holding 6.2]
        _dispatch = base.RoutingAdresse.ToString() + "." + base.TypMbs.ToString() + " " + base.TreiberName.ToString() + " " + Host.ToString();

        //append optional port
        adresse += (Port != null) ? ":" + Port.ToString() : "";
        _dispatch += (Port != null) ? ":" + Port.ToString() : "";

        //append ServerAdresse
        adresse += " " + ServerAdresse.ToString();
        _dispatch += " " + ServerAdresse.ToString();

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

        //[80.Y 0.0.0.0:502 11 holding 6.2]
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
