namespace MBS_Gatewaykonfigurator.Models.BACnet.Types;
using System.ComponentModel.DataAnnotations;
using System.Text;

public class Binary
{

    [Required, Range(0, 1, ErrorMessage = "Nur \"0\" oder \"1\" sind erlaubt")]
    public uint BacPolarity { get; set; }
    public string BacActiveText { get; set; }
    public string BacInactiveText { get; set; }

    //Binary Output / Binary Value
    public string BacRelinquishDefault { get; set; } = "0 -V";

    //Alarming
    public Alarming.Binary? Alarming { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"bac_polarity = {BacPolarity} || WP");

        if (!string.IsNullOrWhiteSpace(BacActiveText))
            sb.AppendLine($"bac_active_text = {BacActiveText}");

        if (!string.IsNullOrWhiteSpace(BacInactiveText))
            sb.AppendLine($"bac_inactive_text = {BacInactiveText}");

        //Ausgabe in BacNet Klasse
        //if (!string.IsNullOrWhiteSpace(BacRelinquishDefault))
        //    sb.AppendLine($"bac_relinquish_default = {BacRelinquishDefault}");

        if (Alarming != null)
            sb.Append(Alarming.ToString());

        return sb.ToString();
    }

}


