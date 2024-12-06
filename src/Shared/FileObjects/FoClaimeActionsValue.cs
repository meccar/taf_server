namespace Shared.FileObjects;

/// <summary>
/// Represents predefined action constants that can be associated with user roles for resource permissions.
/// </summary>
public class FoClaimeActionsValue
{
    /// <summary>
    /// Action to view a resource.
    /// </summary>
    public const string View = "View";

    /// <summary>
    /// Action to read a resource.
    /// </summary>
    public const string Read = "Read";

    /// <summary>
    /// Action to update a resource.
    /// </summary>
    public const string Update = "Update";

    /// <summary>
    /// Action to delete a resource.
    /// </summary>
    public const string Delete = "Delete";

    /// <summary>
    /// Action to upload a resource.
    /// </summary>
    public const string Upload = "Upload";

    /// <summary>
    /// Action to download a resource.
    /// </summary>
    public const string Download = "Download";
}