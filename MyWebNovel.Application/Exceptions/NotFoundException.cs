namespace MyWebNovel.Application.Exceptions
{
    public class NotFoundException(string entityName, string identifierName, object identifier) : Exception($"The {entityName} with {identifierName} '{identifier}' was not found.")
    {
    }


}
