namespace QEF.Context.Dtos
{
    /// <summary>
    /// Number of entities per ID.
    /// </summary>
    /// <param name="Id">Entity ID.</param>
    /// <param name="Count">Number of entities.</param>
    public record EntityCounter(int Id, int Count);
}
