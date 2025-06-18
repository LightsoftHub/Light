namespace Light.Blazor;

public class DataTable<T>
{
    private readonly Func<Task<Result<IEnumerable<T>>>>? _queryFunc;

    /// <summary>
    /// Load data from server api
    /// </summary>
    /// <param name="queryFunc"></param>
    public DataTable(Func<Task<Result<IEnumerable<T>>>> queryFunc)
    {
        _queryFunc = queryFunc;
    }

    /// <summary>
    /// Load data from local
    /// </summary>
    /// <param name="data"></param>
    public DataTable(IEnumerable<T> data)
    {
        _records = data;
    }

    private IEnumerable<T> _records = [];

    private Func<T, bool>? _searchFunc;

    public Func<string, T, bool>? SearchFunc { get; init; }

    public IEnumerable<T> Records { get; private set; } = [];

    public Paged Pagination { get; private set; } = new();

    public int PageSize { get; set; } = 20;

    public bool Processing { get; private set; } = false;

    public async Task ReloadAsync()
    {
        Processing = true;

        try
        {
            if (_queryFunc is not null)
            {
                var getData = await _queryFunc();

                if (getData.Succeeded)
                {
                    _records = getData.Data;
                }
            }

            DataPagination();
        }
        catch
        { }

        Processing = false;
    }

    public Task RunUpdateAsync(IPage? update)
    {
        if (update != null)
        {
            Pagination.Page = update.Page;
            Pagination.PageSize = update.PageSize;
        }

        DataPagination();

        return Task.CompletedTask;
    }

    public Task SearchAsync(string? value = null)
    {
        Pagination.Page = 1;

        if (string.IsNullOrEmpty(value) || SearchFunc is null)
        {
            _searchFunc = null;
        }
        else
        {
            _searchFunc = e => SearchFunc(value, e);
        }

        DataPagination();

        return Task.CompletedTask;
    }

    private void DataPagination()
    {
        var data = _records;

        if (_searchFunc != null)
        {
            data = data.Where(_searchFunc);
        }

        var pagedData = data.ToPaged(Pagination.Page, PageSize);

        Pagination = pagedData;
        Records = pagedData.Records;
    }
}
