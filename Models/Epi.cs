using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Projeto02.Models;

public partial class Epi
{
    public int CodigoEpi { get; set; }

    public string Nome { get; set; } = null!;

    public int FormaDu { get; set; }
    [JsonIgnore]
    public virtual ICollection<Entrega> Entregas { get; } = new List<Entrega>();
}
