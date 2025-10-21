namespace MBS_Gatewaykonfigurator.Models;

using System;
using System.ComponentModel.DataAnnotations;
public class Gateway
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ProjektId { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Beschreibung { get; set; } = string.Empty;

    [Required]
    public int GeräteId { get; set; }

    public int AnzahlDispatch { get; set; }

    public List<Datenpunkt> Datenpunkte { get; set; } = new();

    public List<DatenpunktGlobal> DatenpunkteGlobal { get; set; } = new();
}
