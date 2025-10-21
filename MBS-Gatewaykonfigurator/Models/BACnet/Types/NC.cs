namespace MBS_Gatewaykonfigurator.Models.BACnet.Types;
using System.Text;

public class NotificationClass
{
    //NC
    public string BacAckRequired { get; set; } = "(0, 0, 0) || WP";
    public string BacPriority { get; set; } = "(99, 99, 99) || WP";
    public string BacRecipientList { get; set; } = "(((1,1,1,1,1,1,1),00:00:00.00,23:59:59.99,[0](8,DEVICE_ID),0,1,(1,1,1)) || WP";

    public override string ToString()
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(BacAckRequired))
            sb.AppendLine($"bac_ack_required = {BacAckRequired}");

        if (!string.IsNullOrWhiteSpace(BacPriority))
            sb.AppendLine($"bac_priority = {BacPriority}");

        if (!string.IsNullOrWhiteSpace(BacRecipientList))
            sb.AppendLine($"bac_recipient_list = {BacRecipientList.ReplaceLineEndings("")}");

        return sb.ToString();
    }

}


