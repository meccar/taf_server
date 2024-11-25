namespace Infrastructure.SeedWork.Constants
{
    public static class TimeConstants
    {
        public static readonly string DateFormatRegex = "^(1[0-2]|0?[1-9])(\\/|-)(3[01]|[12][0-9]|0?[1-9])\\2([0-9]{2})?[0-9]{2}$";
        public static readonly string DateTimeFormatRegex = "^(19|20)\\d\\d-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01]) (0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])$";
        public static readonly string TimeFormatRegex = @"^(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])$";
    }
}