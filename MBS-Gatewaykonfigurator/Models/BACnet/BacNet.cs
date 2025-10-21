namespace MBS_Gatewaykonfigurator.Models.BACnet;
using global::System;
using global::System.ComponentModel.DataAnnotations;
using global::System.Text;
using MBS_Gatewaykonfigurator.Models.BACnet.Types;
using MBS_Gatewaykonfigurator.Models.MBS;

public class BacNet : Mbs, IDispatchable
{
    //test
    //MBS
    //public MBS Mbs = new MBS("bac", 940, "bac1");
    public BacNet() : base("bac", 940, "bac1.txt")
    {

    }

    //BACnet

    //<device>
    [Range(0, 4194302, ErrorMessage = "Die Adresse muss zwischen 0 und 4194302 liegen oder leer sein für \"local\".")]
    public uint? GeräteID { get; set; }

    //<type>
    [Required]
    public Types TypBacNet { get; set; }

    //<no>
    [Required, Range(0, 4194303, ErrorMessage = "Die Adresse muss zwischen 0 und 4194303 liegen.")]
    public uint ObjektNummer { get; set; }

    public string BacDescription = string.Empty;

    public string? Tag;


    //Quelle id: https://reference.opcfoundation.org/BACnet/v200/docs/10.4.21

    public enum Types
    {
        //AC,
        //ACR,
        //AD,
        //AE,
        //AG,
        AI = 0,
        AO = 1,
        //AP,
        //AR,
        //AU,
        //AULOG,
        //AUREP,
        AV = 2,
        //AZ,
        BI = 3,
        //BLIO,
        BO = 4,
        //BSV,
        BV = 5,
        CA = 6,
        //CDI,
        //CH,
        //CO,
        //CSV,
        //DE,
        //DPV,
        //DTPV,
        //DTV,
        //DV,
        //EE,
        //EL,
        //ELGRP,
        //ESC,
        FAILURE,
        //FI,
        //GG,
        //GR,
        IV = 45,
        LAV = 46,
        //LC,
        //LIFT,
        //LIO,
        //LO,
        //LP,
        //LZ,
        MI = 13,
        MO = 14,
        MV = 19,
        NC = 15,
        //NF,
        //NP,
        //NS,
        //OSV,
        //PC,
        PIV = 48,
        //PR,
        //SC,
        //STG,
        //SV,
        //TI,
        //TM,
        //TPV,
        //TR,
        //TV,
    }

    public NotificationClass? NotificationClass;

    public Analog? Analog;
    public Binary? Binary;
    public Multistate? Multistate;
    public Calender? Calender;

    //inherit ElementAppendix
    public TrendLog? Trendlog;
    public Schedule? Schedule;
    public EventEnrollment? EventEnrollment;




    public string toStringFile()
    {

        //[Y mod 13 holding 6]
        //query = pe
        //format = k
        //..

        var sb = new StringBuilder();

        //NC
        if (NotificationClass != null)
        {
            string device = (this.GeräteID == null) ? "local" : this.GeräteID.ToString()!;

            sb.AppendLine($"[A {device}.NC {this.ObjektNummer}]");

            sb.Append(base.ToString());

            if (!string.IsNullOrWhiteSpace(BacDescription))
                sb.AppendLine($"bac_description = {BacDescription}{base.BeschreibungAppendix}");


            sb.Append(NotificationClass.ToString());
        }
        //default
        else
        {

            sb.AppendLine("[" + toStringDriver() + "]");

            sb.Append(base.ToString());

            if (!string.IsNullOrWhiteSpace(BacDescription))
                sb.AppendLine($"bac_description = {BacDescription}{base.BeschreibungAppendix}");

            if (Analog != null)
            {
                if (!string.IsNullOrWhiteSpace(Analog.BacRelinquishDefault) && TypBacNet != Types.AI)
                {
                    sb.AppendLine($"bac_relinquish_default = {Analog.BacRelinquishDefault}");
                }

                sb.Append(Analog.ToString());
            }
            else if (Binary != null)
            {
                if (!string.IsNullOrWhiteSpace(Binary.BacRelinquishDefault) && TypBacNet != Types.BI)
                {
                    sb.AppendLine($"bac_relinquish_default = {Binary.BacRelinquishDefault}");
                }

                sb.Append(Binary.ToString());
            }
            else if (Multistate != null)
            {
                if (!string.IsNullOrWhiteSpace(Multistate.BacRelinquishDefault) && TypBacNet != Types.MI)
                {
                    sb.AppendLine($"bac_relinquish_default = {Multistate.BacRelinquishDefault}");
                }

                sb.Append(Multistate.ToString());
            }
            else if (Calender != null)
            {
                sb.Append(Calender.ToString());
            }
        }



        //add Tag
        if (!string.IsNullOrWhiteSpace(Tag))
            sb.AppendLine($"tag = {Tag}");




        //TrendLog
        if (Trendlog != null)
        {
            //add emty line
            sb.AppendLine();

            string device = (this.GeräteID == null) ? "local" : this.GeräteID.ToString()!;

            sb.AppendLine($"[A {device}.TR {this.ObjektNummer}]");

            if (!string.IsNullOrWhiteSpace(base.Name))
                sb.AppendLine($"name = {base.Name}{Trendlog.NamenAppendix}");

            if (!string.IsNullOrWhiteSpace(BacDescription))
                sb.AppendLine($"bac_description = {BacDescription}{Trendlog.BeschreibungAppendix}");

            if (!string.IsNullOrWhiteSpace(base.Query))
                sb.AppendLine($"query = {base.Query}");

            sb.AppendLine($"bac_log_device_object_property = (({(uint)this.TypBacNet},{this.ObjektNummer}),85)  || WP");

            sb.Append(Trendlog.ToString());
        }

        if (Schedule != null)
        {
            //add emty line
            sb.AppendLine();

            string device = (this.GeräteID == null) ? "local" : this.GeräteID.ToString()!;

            sb.AppendLine($"[Y {device}.SC {this.ObjektNummer}]");

            if (!string.IsNullOrWhiteSpace(base.Name))
                sb.AppendLine($"name = {base.Name}{Schedule.NamenAppendix}");

            if (!string.IsNullOrWhiteSpace(BacDescription))
                sb.AppendLine($"bac_description = {BacDescription}{Schedule.BeschreibungAppendix}");

            if (!string.IsNullOrWhiteSpace(base.Query))
                sb.AppendLine($"query = {base.Query}");

            sb.AppendLine($"bac_list_of_object_property_references = ((({(uint)this.TypBacNet},{this.ObjektNummer}),85)) || WP");


            sb.Append(Schedule.ToString());
        }

        if (EventEnrollment != null)
        {
            //add emty line
            sb.AppendLine();

            string device = (this.GeräteID == null) ? "local" : this.GeräteID.ToString()!;

            sb.AppendLine($"[M {device}.EE {this.ObjektNummer}]");

            if (!string.IsNullOrWhiteSpace(base.Name))
                sb.AppendLine($"name = {base.Name}{EventEnrollment.NamenAppendix}");


            if (!string.IsNullOrWhiteSpace(BacDescription))
                sb.AppendLine($"bac_description = {BacDescription}{EventEnrollment.BeschreibungAppendix}");

            if (!string.IsNullOrWhiteSpace(base.Query))
                sb.AppendLine($"query = {base.Query}");

            sb.AppendLine($"bac_object_property_reference = (({(uint)this.TypBacNet},{this.ObjektNummer}),85) || WP");


            sb.Append(EventEnrollment.ToString());
        }

        return sb.ToString();
    }

    //940.Y bac local.BV 01
    private string _dispatch = string.Empty;



    public string toStringDriver()
    {

        string device = (GeräteID == null) ? "local" : GeräteID.ToString()!;

        //Kommunikationsstatus
        if (TypBacNet.Equals(Types.FAILURE))
        {
            //ohne Objektnummer
            //[Y local failure]
            _dispatch = base.RoutingAdresse.ToString() + "." + base.TypMbs.ToString() + " " + base.TreiberName.ToString() + " " + device + " " + TypBacNet.ToString().ToLower();

            //[940.Y bac local failure]
            return base.TypMbs.ToString() + " " + device + " " + TypBacNet.ToString().ToLower();
        }
        else
        {

            //[940.Y bac local.BV 1]
            _dispatch = base.RoutingAdresse.ToString() + "." + base.TypMbs.ToString() + " " + base.TreiberName.ToString() + " " + device + "." + TypBacNet.ToString() + " " + ObjektNummer.ToString();

            //[Y local.BV 1]
            return base.TypMbs.ToString() + " " + device + "." + TypBacNet.ToString() + " " + ObjektNummer.ToString();

        }
    }

    public string toStringDispatch()
    {
        //Dispatch string erstellen
        toStringDriver();

        //[940.Y bac local.BV 1]
        return _dispatch;
    }

}
