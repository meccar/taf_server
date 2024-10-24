using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;

public class CompanyEntity : BaseEntity
{
    public int Id { get; set; }
    public string Uuid { get; set; } = "";
    public string Name { get; set; } = "";
    public string FaxCode { get; set; } = "";
    public string Email { get; set; } = "";
    public string Website { get; set; } = "";
    public string Phone { get; set; } = "";
    public DateTime EstblishedDate { get; set; }
    public DateTime OperationDate { get; set; }
    public string Summary { get; set; } = "";
    public string Description { get; set; } = "";
    public int NumPeople { get; set; }
    public string Logo { get; set; } = "";
    public string Address { get; set; } = "";
    public int StateId { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }
    public UserAccountEntity? UserAccount { get; set; }
    public RoleEntity? Roles { get; set; }
    public CountryEntity? Country { get; set; }
    public StateEntity? State { get; set; }
    public CityEntity? City { get; set; }
    public DistrictEntity? District { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
    public UserAccountEntity? CreatedByUser { get; set; }
    public UserAccountEntity? UpdatedByUser { get; set; }
}
