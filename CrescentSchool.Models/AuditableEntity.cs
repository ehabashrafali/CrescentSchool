namespace CrescentSchool.Models;

public abstract class AuditableEntity
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    protected AuditableEntity()
    {

    }
    protected void SetCreated()
    {
        CreatedAt = SystemConstants.Now;
    }
    protected void SetUpdated()
    {
        UpdatedAt = SystemConstants.Now;
    }
    protected void SetDeleted()
    {
        DeletedAt = SystemConstants.Now;
    }
}
