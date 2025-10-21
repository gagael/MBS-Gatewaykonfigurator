namespace MBS_Gatewaykonfigurator.Models.BACnet.Types;

using MBS_Gatewaykonfigurator.Models.MBS;
using System.ComponentModel.DataAnnotations;
using System.Text;

public class Schedule : ElementAppendix
{
    [Required]
    public string BacEffectivePeriod { get; set; } = "((*,*-*-*),(*,*-*-*)) || WP";

    [Required]
    public string BacOutOfService { get; set; } = "0";

    [Required]
    public string BacScheduleDefault { get; set; } = "[4] 1 || WP";

    [Required]
    public string BacWeeklySchedule { get; set; } = "(((00:00:00.00,[4] 2),(06:00:00.00,[4] 1),(22:00:00.00,[4] 2)),((00:00:00.00,[4] 2),(06:00:00.00,[4] 1),(22:00:00.00,[4] 2)),((00:00:00.00,[4] 2),(06:00:00.00,[4] 1),(22:00:00.00,[4] 2)),((00:00:00.00,[4] 2),(06:00:00.00,[4] 1),(22:00:00.00,[4] 2)),((00:00:0 0.00,[4] 2),(06:00:00.00,[4] 1),(22:00:00.00,[4] 2)),((00:00:00.00,[4] 2),(06:00:00.00,[4] 1),(22:00:00.00,[4] 2)),((00:00:00.00,[4] 2),(06 :00:00.00,[4] 1),(22:00:00.00,[4] 2)))||WP";

    // Optional
    public string BacExceptionSchedule { get; set; } = "(([0] [0] (?,?-?-?),(),16),([0] [1] ((?,?-?-?),(?,?-?-?)),(),16),([0] [2] (?-?-?),(),16)) || WP";

    [Range(1, 16, ErrorMessage = "Der Wert muss zwischen 1 und 16 liegen.")]
    public uint? BacPriorityForWriting { get; set; }

    public Schedule()
    {
        this.NamenAppendix = "SC01";
        this.BeschreibungAppendix = " " + nameof(Schedule);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(BacEffectivePeriod))
            sb.AppendLine($"bac_effective_period = {BacEffectivePeriod}");

        if (!string.IsNullOrWhiteSpace(BacOutOfService))
            sb.AppendLine($"bac_out_of_service = {BacOutOfService} || WP");

        if (!string.IsNullOrWhiteSpace(BacScheduleDefault))
            sb.AppendLine($"bac_schedule_default = {BacScheduleDefault}");

        if (!string.IsNullOrWhiteSpace(BacWeeklySchedule))
            sb.AppendLine($"bac_weekly_schedule = {BacWeeklySchedule.ReplaceLineEndings("")}");

        if (!string.IsNullOrWhiteSpace(BacExceptionSchedule))
            sb.AppendLine($"bac_exception_schedule = {BacExceptionSchedule.ReplaceLineEndings("")}");

        if (BacPriorityForWriting > 0)
            sb.AppendLine($"bac_priority_for_writing = {BacPriorityForWriting} || WP");

        return sb.ToString();
    }
}
