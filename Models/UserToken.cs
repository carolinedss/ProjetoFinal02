namespace Projeto02.Models;

public class UserToken
{
    public string? Token { get; set; }
    public DateTime Expiration  { get; set; }
    public IList<string> Roles { get; internal set; }
    public int  IdCol { get; set; }

}
