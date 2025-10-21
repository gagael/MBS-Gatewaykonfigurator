namespace MBS_Gatewaykonfigurator.Models.BACnet.Types;
using System.ComponentModel.DataAnnotations;
using System.Text;

public class Multistate
{
    [Required]
    public byte BacNumberOfStates { get; set; }
    public string BacStateText { get; set; }

    //Multistate Output /  Multistate Value
    public string BacRelinquishDefault { get; set; } = "0 -V";

    //Alarming
    public Alarming.Multistate? Alarming { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"bac_number_of_states = {BacNumberOfStates} || WP");

        if (!string.IsNullOrWhiteSpace(BacStateText))
            sb.AppendLine($"bac_state_text  = {BacStateText.ReplaceLineEndings("")}");

        //Ausgabe in BacNet Klasse
        //if (!string.IsNullOrWhiteSpace(BacRelinquishDefault))
        //    sb.AppendLine($"bac_relinquish_default = {BacRelinquishDefault}");

        if (Alarming != null)
            sb.Append(Alarming.ToString());

        return sb.ToString();
    }

}


