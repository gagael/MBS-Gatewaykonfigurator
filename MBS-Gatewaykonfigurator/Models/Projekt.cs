namespace MBS_Gatewaykonfigurator.Models;
using System;
using System.ComponentModel.DataAnnotations;

public class Projekt
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Beschreibung { get; set; } = string.Empty;

}
