using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;

public class CompanyEntity : BaseEntity
{
    public int Id { get; set; }
    public required string Uuid { get; set; }
    public required string Name { get; set; }
    public required string FaxCode { get; set; }
    public required string Email { get; set; }
    public required string Website { get; set; }
    public required string Phone { get; set; }
    public DateTime EstblishedDate { get; set; }
    public DateTime OperationDate { get; set; }
    public required string Summary { get; set; }
    public required string Description { get; set; }
    public int NumPeople { get; set; }
    public required string Logo { get; set; }
    public required string Address { get; set; }
    public int StateId { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }
    public required UserAccountEntity UserAccount { get; set; }
    public required RoleEntity Roles { get; set; }
    public required CountryEntity Country { get; set; }
    public required StateEntity State { get; set; }
    public required CityEntity City { get; set; }
    public required DistrictEntity District { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
    public required UserAccountEntity CreatedByUser { get; set; }
    public required UserAccountEntity UpdatedByUser { get; set; }
    //public required BaseEntity vaseEntity { get; set; }
}
