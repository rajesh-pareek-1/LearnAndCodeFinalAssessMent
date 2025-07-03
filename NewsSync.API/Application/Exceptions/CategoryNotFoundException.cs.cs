namespace NewsSync.API.Application.Exceptions
{
    public class CategoryNotFoundException : Exception
    {
        public CategoryNotFoundException(string categoryName)
            : base($"Category '{categoryName}' was not found.") { }
    }
}
