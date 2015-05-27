namespace GA.Data.Model
{
	/// <summary>
	/// I has long identifier. This is for objects that have a "long" identifier. 
	/// </summary>
    public interface IHasLongId
    {
        long Id { get; set; }
    }
}
