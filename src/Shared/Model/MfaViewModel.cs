using System.ComponentModel.DataAnnotations;

namespace Shared.Model;

public class MfaViewModel
{
    [Required] 
    public string SharedKey { get; set; }

    [Required] 
    public string AuthenticatorUri { get; set; }
}