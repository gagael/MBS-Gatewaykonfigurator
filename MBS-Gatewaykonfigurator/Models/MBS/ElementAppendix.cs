namespace MBS_Gatewaykonfigurator.Models.MBS;
using global::System.ComponentModel.DataAnnotations;



public class ElementAppendix
{
    public ElementAppendix()
    {

    }

    [Required]
    public string NamenAppendix { get; set; } = string.Empty;

    [Required]
    public string BeschreibungAppendix { get; set; } = string.Empty;

}






