using System.Text;

namespace MBS_Gatewaykonfigurator.Models.MBS;
public class Dispatch
{
    public string? Threshold { get; set; }
    public string? Value { get; set; }

    //bacnet only
    public uint? Prio { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(Threshold))
            sb.AppendLine($"threshold = {Threshold}");

        if (!string.IsNullOrWhiteSpace(Value))
            sb.AppendLine($"value = {Value}");

        if (Prio != null)
            sb.AppendLine($"prio = {Prio}");

        return sb.ToString();
    }
}
