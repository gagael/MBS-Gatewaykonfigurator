namespace MBS_Gatewaykonfigurator.Models.MBS;
using global::System.ComponentModel.DataAnnotations;

public class SystemMbs : Mbs, IDispatchable
{
    //MBS
    //Typ wird nicht benötig, muss aber gesetzt werden
    //MBS Mbs = new MBS("system", 1, "system1") { Typ = MBS.Types.M };
    public SystemMbs() : base("system", 1, "system1")
    {
    }

    //system

    //<datapoint-type>
    [Required]
    public SystemTypes TypeSystem { get; set; } = SystemTypes.error;

    public string toStringFile()
    {

        //keine Implementierung nötig
        return string.Empty;
    }


    public string toStringDriver()
    {

        //keine Implementierungb nötig
        return string.Empty;
    }

    public string toStringDispatch()
    {

        //[1 system cpuload]
        return base.RoutingAdresse.ToString() + " " + base.TreiberName.ToString() + " " + TypeSystem.ToString();

    }

    public enum SystemTypes
    {
        info,
        warning,
        error,
        fatal,
        button,
        relay,
        led,
        temp,
        freemem,
        cpuload,
        button1,
        button2,
        button3,
        sdcarduse,
        sdcard,
        mdmstate,
        mdmmoldo,
        mdmmoldi,
        runtime,
        watchdog
    }
}
