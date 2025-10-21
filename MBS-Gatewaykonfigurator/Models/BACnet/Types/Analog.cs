namespace MBS_Gatewaykonfigurator.Models.BACnet.Types;
using System.ComponentModel.DataAnnotations;
using System.Text;

public class Analog
{

    [Required]
    public uint BacUnits { get; set; } = 95;
    public string? BacCovIncrement { get; set; }
    public string? BacResolution { get; set; }
    public string? BacMinPresValue { get; set; }
    public string? BacMaxPresValue { get; set; }

    //Analog Output / Analog Value
    public string BacRelinquishDefault { get; set; } = "0 -V";

    //Alarming
    public Alarming.Analog? Alarming { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"bac_units = {BacUnits} || WP");

        if (!string.IsNullOrWhiteSpace(BacCovIncrement))
            sb.AppendLine($"bac_cov_increment = {BacCovIncrement}");

        if (!string.IsNullOrWhiteSpace(BacResolution))
            sb.AppendLine($"bac_resolution = {BacResolution}");

        if (!string.IsNullOrWhiteSpace(BacMinPresValue))
            sb.AppendLine($"bac_min_pres_value = {BacMinPresValue}");

        if (!string.IsNullOrWhiteSpace(BacMaxPresValue))
            sb.AppendLine($"bac_max_pres_value = {BacMaxPresValue}");

        //Ausgabe in BacNet Klasse
        //if (!string.IsNullOrWhiteSpace(BacRelinquishDefault))
        //    sb.AppendLine($"bac_relinquish_default = {BacRelinquishDefault}");

        if (Alarming != null)
            sb.Append(Alarming.ToString());


        return sb.ToString();
    }

}


