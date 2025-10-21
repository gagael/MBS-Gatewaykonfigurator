namespace MBS_Gatewaykonfigurator.Models.BACnet.Types;
using System.Text;

public class Calender
{
    //CA
    public string BacDateList { get; set; } = "([0](?, 01 - January - 2017), [0](?, 08 - April - 2017)) || WP";

    public override string ToString()
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(BacDateList))
            sb.AppendLine($"bac_date_list = {BacDateList.ReplaceLineEndings("")}");

        return sb.ToString();
    }

}


