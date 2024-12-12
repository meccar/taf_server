using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace Domain.Aggregates;

public class NewsAggregate : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     public string Uuid { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [MaxLength(255)]
    public string ImageUrl { get; set; } = "";
    
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = "";
    
    [Required]
    [MaxLength(80)]
    public string SubTitle { get; set; } = "";
    
    [Required]
    [MaxLength(255)]
    public string Summary { get; set; } = "";
    
    [Required]
    public string Content { get; set; } = "";

    [Required]
    public bool IsPublished { get; set; }
    
    [Required]
    public int? CreatedByUserAccountId { get; set; }
    
    [Required]
    public int? UpdatedByUserAccountId { get; set; }
    
    // public virtual UserAccountAggregate CreatedByUserAccount { get; set; } = null!;
    //
    // public virtual UserAccountAggregate UpdatedByUserAccount { get; set; } = null!;
}