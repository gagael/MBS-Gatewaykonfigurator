namespace MBS_Gatewaykonfigurator.Models.BACnet.Types;

using System.ComponentModel.DataAnnotations;
using System.Text;

public class Alarming
{

    //Alarming
    public string? BacTimeDelay { get; set; }

    [Required]
    public string BacEventEnable { get; set; } = "(1,1,1)";

    [Required]
    public string BacNotifyType { get; set; } = "alarm";

    [Required]
    public byte BacNotificationClass { get; set; }
    public string BacEventMessageText { get; set; } = "(\"%D Alarm\", \"%D Fehler\", \"% D Normal\")";
    public string? BacRecipientList { get; set; }

    //Zusatz
    public string? IntrinsicReporting { get; set; }
    public override string ToString()
    {


        var sb = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(BacTimeDelay))
            sb.AppendLine($"bac_time_delay = {BacTimeDelay}");

        if (!string.IsNullOrWhiteSpace(BacEventEnable))
            sb.AppendLine($"bac_event_enable = {BacEventEnable}");

        if (!string.IsNullOrWhiteSpace(BacNotifyType))
            sb.AppendLine($"bac_notify_type = {BacNotifyType}");


        sb.AppendLine($"bac_notification_class = {BacNotificationClass}");

        if (!string.IsNullOrWhiteSpace(BacEventMessageText))
            sb.AppendLine($"bac_event_message_text = {BacEventMessageText}");

        if (!string.IsNullOrWhiteSpace(BacRecipientList))
            sb.AppendLine($"bac_recipient_list = {BacRecipientList}");

        if (!string.IsNullOrWhiteSpace(IntrinsicReporting))
            sb.AppendLine($"intrinsic_reporting = {IntrinsicReporting} || WP");

        return sb.ToString();
    }


    // Analog
    public class Analog : Alarming
    {
        public string? BacLimitEnable { get; set; }
        public string? BacLowLimit { get; set; }
        public string? BacHighLimit { get; set; }
        public string? BacDeadband { get; set; }
        public override string ToString()
        {


            var sb = new StringBuilder();

            sb.Append(base.ToString());

            if (!string.IsNullOrWhiteSpace(BacLimitEnable))
                sb.AppendLine($"bac_limit_enable = {BacLimitEnable} || WP");

            if (!string.IsNullOrWhiteSpace(BacLowLimit))
                sb.AppendLine($"bac_low_limit = {BacLowLimit}");

            if (!string.IsNullOrWhiteSpace(BacHighLimit))
                sb.AppendLine($"bac_high_limit = {BacHighLimit}");

            if (!string.IsNullOrWhiteSpace(BacDeadband))
                sb.AppendLine($"bac_deadband = {BacDeadband}");

            return sb.ToString();
        }
    }

    // Binary
    public class Binary : Alarming
    {
        [Required]
        public string BacAlarmValue { get; set; } = "1";
        public override string ToString()
        {


            var sb = new StringBuilder();

            sb.Append(base.ToString());

            if (!string.IsNullOrWhiteSpace(BacAlarmValue))
                sb.AppendLine($"bac_alarm_value = {BacAlarmValue} || WP");

            return sb.ToString();
        }
    }

    // Multistate
    public class Multistate : Alarming
    {
        public string BacAlarmValues { get; set; } = "()";
        public string BacFaultValues { get; set; } = "()";
        public override string ToString()
        {


            var sb = new StringBuilder();

            sb.Append(base.ToString());

            if (!string.IsNullOrWhiteSpace(BacAlarmValues))
                sb.AppendLine($"bac_alarm_values = {BacAlarmValues}");

            if (!string.IsNullOrWhiteSpace(BacFaultValues))
                sb.AppendLine($"bac_fault_values = {BacFaultValues}");

            return sb.ToString();
        }
    }

    // Life-Safety
    /*
    public class LifeSafety : Alarming
    {
        public string BacLifeSafetyAlarmValues { get; set; }
        public string BacAlarmValues { get; set; }
        public string BacFaultValues { get; set; }
        public override string ToString()
        {


            var sb = new StringBuilder();

            sb.Append(base.ToString());

            if (!string.IsNullOrWhiteSpace(BacLifeSafetyAlarmValues))
                sb.AppendLine($"bac_life_safety_alarm_values = {BacLifeSafetyAlarmValues}");

            if (!string.IsNullOrWhiteSpace(BacAlarmValues))
                sb.AppendLine($"bac_alarm_values = {BacAlarmValues}");

            if (!string.IsNullOrWhiteSpace(BacFaultValues))
                sb.AppendLine($"bac_fault_values = {BacFaultValues}");

            return sb.ToString();
        }

    }
    */

    //TR TrendLog und EE
    public class TrendLogAndEventEnrollment : Alarming
    {
        public string BacEventMessageTextsConfig { get; set; } = "(\"Anforderung Archivierung\",\"Anforderung Archivierung\",\"Anforderung Archivierung\")";
        public string BacNotificationThreshold { get; set; } = "240 || WP";
        public override string ToString()
        {


            var sb = new StringBuilder();

            sb.Append(base.ToString());

            if (!string.IsNullOrWhiteSpace(BacEventMessageTextsConfig))
                sb.AppendLine($"bac_event_message_texts_config = {BacEventMessageTextsConfig}");

            if (!string.IsNullOrWhiteSpace(BacNotificationThreshold))
                sb.AppendLine($"bac_notification_threshold = {BacNotificationThreshold}");

            return sb.ToString();
        }




    }
}


