namespace GA.Data.Model
{
	/// <summary>
	/// I has string identifier. For some of the items which the ID is a path, key, url.
	/// </summary>
    public interface IHasStringId
    {
        string Id { get; set; }
    }
}
