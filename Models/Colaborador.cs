using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Projeto02.Models;

public partial class Colaborador
{
    public int CodigoColab { get; set; }

    public string Telefone { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Ctps { get; set; } = null!;

    public DateOnly Dtadmissao { get; set; }

    public string Gerente { get; set; } = null!;

    public string Email { get; set; } = null!;

    public decimal Cpf { get; set; }

    [JsonIgnore]
    public virtual Entrega? Entrega { get; set; }
}
