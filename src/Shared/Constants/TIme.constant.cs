namespace Shared.Constants;

/// <summary>
/// Contains regex patterns for validating date, time, and datetime formats.
/// </summary>
public static class TimeConstants
{
    /// <summary>
    /// Regular expression for validating a date in MM/DD/YYYY or MM-DD-YYYY format.
    /// </summary>
    public static readonly string DateFormatRegex = "^(1[0-2]|0?[1-9])(\\/|-)(3[01]|[12][0-9]|0?[1-9])\\2([0-9]{2})?[0-9]{2}$";
    
    /// <summary>
    /// Regular expression for validating a datetime in YYYY-MM-DD HH:mm:ss format.
    /// </summary>
    public static readonly string DateTimeFormatRegex = "^(19|20)\\d\\d-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01]) (0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])$";
    
    /// <summary>
    /// Regular expression for validating a time in HH:mm:ss format.
    /// </summary>
    public static readonly string TimeFormatRegex = @"^(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])$";
}
