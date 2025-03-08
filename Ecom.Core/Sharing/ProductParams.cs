namespace Ecom.Core.Sharing;

public class ProductParams
{
    public string? sort { get; set; }
    public int? CategoryId { get; set; }
    public int MaxPageSize { get; set; } = 6;
    public string? Search { get; set; }
    private int _PageSize=3;

	public int pageSize
	{
		get { return _PageSize=3; }
		set { _PageSize = value > MaxPageSize?MaxPageSize:value; }
	}

    public int PageNumber { get; set; } = 1;
}
