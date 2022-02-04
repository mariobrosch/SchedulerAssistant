namespace SchedulerAssistant.Data.Data
{
    public static class Filter
    {
        public static string CreateFilter(string column, string value)
        {
            return "?filter[" + column + "]=" + value;
        }

        public static string CreateFilter(FilterObject filter)
        {
            if (filter == null || filter.Column == null || filter.Value == null)
            {
                return "";
            }
            return CreateFilter(filter.Column, filter.Value);
        }

        public static FilterObject Deserialize(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return new FilterObject();
            }
            FilterObject filterObject = new();

            filter = filter.Replace("?filter[", "");
            filter = filter.Replace("]", "");
            string[] parts = filter.Split('=');

            filterObject.Column = parts[0];
            filterObject.Value = parts[1];

            return filterObject;
        }
    }
}
