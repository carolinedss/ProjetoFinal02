using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Projeto02.Models;

public partial class Entrega
{
    public int CodigoEntrega { get; set; }

    public int CodigoEpi { get; set; }

    public int CodigoColab { get; set; }

    public DateOnly DtValidade { get; set; }

    public DateOnly DtEntrega { get; set; }
    [JsonIgnore]
    public virtual Colaborador CodigoEntregaNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual Epi CodigoEpiNavigation { get; set; } = null!;
}
