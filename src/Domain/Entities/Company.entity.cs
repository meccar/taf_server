namespace Domain.Entities;

/// <summary>
/// Represents a company entity in the system.
/// This class stores essential details about a company, including its unique identifiers, contact information, 
/// address, and associated geographical location (country, state, city, district).
/// </summary>
public class CompanyEntity : EntityBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the company entity.
    /// This is the primary key for the company entity and is used to uniquely identify each company.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the globally unique identifier (Ulid) for the company.
    /// This provides an alternative unique identifier for the company.
    /// </summary>
    public string Ulid { get; set; } = "";

    /// <summary>
    /// Gets or sets the name of the company.
    /// This property holds the full official name of the company.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Gets or sets the fax code of the company.
    /// This is a unique fax code used by the company, typically used in communication systems.
    /// </summary>
    public string FaxCode { get; set; } = "";

    /// <summary>
    /// Gets or sets the email address of the company.
    /// This property holds the main contact email address for the company.
    /// </summary>
    public string Email { get; set; } = "";

    /// <summary>
    /// Gets or sets the website URL of the company.
    /// This property stores the official website link for the company.
    /// </summary>
    public string Website { get; set; } = "";

    /// <summary>
    /// Gets or sets the phone number of the company.
    /// This property holds the main contact phone number for the company.
    /// </summary>
    public string Phone { get; set; } = "";

    /// <summary>
    /// Gets or sets the date when the company was established.
    /// This property holds the founding date of the company.
    /// </summary>
    public DateTime EstblishedDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the company started operations.
    /// This property holds the date when the company began its business operations.
    /// </summary>
    public DateTime OperationDate { get; set; }

    /// <summary>
    /// Gets or sets a brief summary of the company.
    /// This property provides a short description or overview of the company.
    /// </summary>
    public string Summary { get; set; } = "";

    /// <summary>
    /// Gets or sets a detailed description of the company.
    /// This property stores an in-depth description of the company, its services, and other relevant information.
    /// </summary>
    public string Description { get; set; } = "";

    /// <summary>
    /// Gets or sets the number of people employed by the company.
    /// This property stores the total number of employees working for the company.
    /// </summary>
    public int NumPeople { get; set; }

    /// <summary>
    /// Gets or sets the logo of the company.
    /// This property stores the company's logo, typically a URL or a file path to the logo image.
    /// </summary>
    public string Logo { get; set; } = "";

    /// <summary>
    /// Gets or sets the address of the company.
    /// This property holds the full physical address of the company.
    /// </summary>
    public string Address { get; set; } = "";

    /// <summary>
    /// Gets or sets the ID of the state where the company is located.
    /// This establishes a relationship between the company and the state in which it operates.
    /// </summary>
    public int StateId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the city where the company is located.
    /// This establishes a relationship between the company and the city in which it operates.
    /// </summary>
    public int CityId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the district where the company is located.
    /// This establishes a relationship between the company and the district within the city.
    /// </summary>
    public int DistrictId { get; set; }
        
    // public UserAccountEntity? UserAccount { get; set; }
    
    /// <summary>
    /// Gets or sets the country entity to which the company belongs.
    /// This property represents the country where the company is located.
    /// </summary>
    public CountryEntity? Country { get; set; }

    /// <summary>
    /// Gets or sets the state entity to which the company belongs.
    /// This property represents the state where the company is located.
    /// </summary>
    public StateEntity? State { get; set; }

    /// <summary>
    /// Gets or sets the city entity to which the company belongs.
    /// This property represents the city where the company is located.
    /// </summary>
    public CityEntity? City { get; set; }

    /// <summary>
    /// Gets or sets the district entity to which the company belongs.
    /// This property represents the district within the city where the company is located.
    /// </summary>
    public DistrictEntity? District { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user who created the company entity.
    /// This property links to the user who created this company record.
    /// </summary>
    public int CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user who last updated the company entity.
    /// This property links to the user who last modified this company record.
    /// </summary>
    public int UpdatedBy { get; set; }
    
    // public UserAccountEntity? CreatedByUser { get; set; }
    // public UserAccountEntity? UpdatedByUser { get; set; }
}
