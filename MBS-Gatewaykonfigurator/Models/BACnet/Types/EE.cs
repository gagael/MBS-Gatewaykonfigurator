namespace MBS_Gatewaykonfigurator.Models.BACnet.Types;

using MBS_Gatewaykonfigurator.Models.MBS;
using System.ComponentModel.DataAnnotations;
using System.Text;

public class EventEnrollment : ElementAppendix
{
    [Required]
    public EventType BacEventType { get; set; } = EventType.OutOfRange;

    [Required]
    public string BacEventParameters { get; set; } = "[5] (0.0,12.0,27.0,0.0)";

    //Alarming
    public Alarming.TrendLogAndEventEnrollment? Alarming { get; set; }

    public EventEnrollment()
    {
        this.NamenAppendix = "EE01";
        this.BeschreibungAppendix = " " + nameof(EventEnrollment);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"bac_event_type = {(int)BacEventType}");

        if (!string.IsNullOrWhiteSpace(BacEventParameters))
            sb.AppendLine($"bac_event_parameters = {BacEventParameters.ReplaceLineEndings("")}");

        if (Alarming != null)
            sb.Append(Alarming.ToString());

        return sb.ToString();
    }

    //Quelle: https://reference.opcfoundation.org/BACnet/v200/docs/10.4.12
    public enum EventType
    {
        ChangeOfBitstring = 0,
        ChangeOfState = 1,
        ChangeOfValue = 2,
        CommandFailure = 3,
        FloatingLimit = 4,
        OutOfRange = 5,
        ChangeOfLifeSafety = 8,
        Extended = 9,
        BufferReady = 10,
        UnsignedRange = 11,
        AccessEvent = 13,
        DoubleOutOfRange = 14,
        SignedOutOfRange = 15,
        UnsignedOutOfRange = 16,
        ChangeOfCharacterstring = 17,
        ChangeOfStatusFlags = 18
    }
}
