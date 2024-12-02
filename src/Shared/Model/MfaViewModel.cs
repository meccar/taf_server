using System.ComponentModel.DataAnnotations;

namespace Shared.Model;

public class MfaViewModel
{
    [Required] 
    public string Token { get; set; }

    [Required] 
    public string Code { get; set; }
}