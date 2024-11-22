using Duende.IdentityServer.Events;

public class LoginSuccessEvent : Event
{
    public string Eid { get; set; }
    public string Email { get; set; }
    public LoginSuccessEvent(string eid, string email)
        : base(EventCategories.Authentication,
            "User Login Failure",
            EventTypes.Failure, 
            EventIds.UserLoginFailure)
    {
        Eid = eid;
        Email = email;
    }
}