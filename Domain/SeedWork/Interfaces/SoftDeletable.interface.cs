namespace taf_server.Domain.SeedWork.Interfaces;
public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
}
