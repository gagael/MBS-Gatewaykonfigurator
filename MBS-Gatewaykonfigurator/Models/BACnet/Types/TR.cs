namespace MBS_Gatewaykonfigurator.Models.BACnet.Types;

using MBS_Gatewaykonfigurator.Models.MBS;
using System.ComponentModel.DataAnnotations;
using System.Text;

public class TrendLog : ElementAppendix
{
    [Required]
    public string BacLogEnable { get; set; } = "1";

    [Required]
    public string BacStopWhenFull { get; set; } = "0";

    [Required]
    public string BacBufferSize { get; set; } = "14400 || WP";

    [Required]
    public string BacLoggingType { get; set; } = "1";

    [Required]
    public string BacLogInterval { get; set; } = "0 || WP  # 100 = 1sek";

    // Optional
    public string? BacAlignIntervals { get; set; }
    public string? BacIntervalOffset { get; set; }

    //Alarming
    public Alarming.TrendLogAndEventEnrollment? Alarming { get; set; }

    public TrendLog()
    {
        this.NamenAppendix = "TL01";
        this.BeschreibungAppendix = " " + nameof(TrendLog);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(BacLogEnable))
            sb.AppendLine($"bac_log_enable = {BacLogEnable}");

        if (!string.IsNullOrWhiteSpace(BacStopWhenFull))
            sb.AppendLine($"bac_stop_when_full = {BacStopWhenFull}");

        if (!string.IsNullOrWhiteSpace(BacBufferSize))
            sb.AppendLine($"bac_buffer_size = {BacBufferSize}");

        if (!string.IsNullOrWhiteSpace(BacLoggingType))
            sb.AppendLine($"bac_logging_type = {BacLoggingType} || WP");

        if (!string.IsNullOrWhiteSpace(BacLogInterval))
            sb.AppendLine($"bac_log_interval = {BacLogInterval}");

        if (!string.IsNullOrWhiteSpace(BacAlignIntervals))
            sb.AppendLine($"bac_align_intervals = {BacAlignIntervals} || WP");

        if (!string.IsNullOrWhiteSpace(BacIntervalOffset))
            sb.AppendLine($"bac_interval_offset = {BacIntervalOffset}");

        if (Alarming != null)
            sb.Append(Alarming.ToString());

        return sb.ToString();
    }
}
