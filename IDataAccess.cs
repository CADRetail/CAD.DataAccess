namespace CAD.DataAccess;
public interface IDataAccess
{
    string ConnStr { get; set; }
    string GetConnstr(string IP, string Db, string userName, string password);
    string GetConnStr(string ConnectionStr = "");
    bool PingHost(string nameOrAddress);
    void Execute<T>(string sql, T parameter, string ConnectionStr = "");
    Task ExecuteAsync<T>(string sql, T parameter, string ConnectionStr = "");
    void Execute(string sql, string ConnectionStr = "");
    Task ExecuteAsync(string sql, string ConnectionStr = "");
    long Insert<T>(T data, string ConnectionStr = "") where T : class;
    Task<int> InsertAsync<T>(T data, string ConnectionStr = "") where T : class;
    Task<int> InsertAsync<T>(List<T> data, string ConnectionStr = "") where T : class;
    bool Update<T>(T data, string ConnectionStr = "") where T : class;
    Task<bool> UpdateAsync<T>(T data, string ConnectionStr = "") where T : class;
    Task<bool> UpdateAsync<T>(List<T> data, string ConnectionStr = "") where T : class;
    IEnumerable<T> QueryData<T, U>(string sql, U parameter, string ConnectionStr = "");
    Task<IEnumerable<T>> QueryDataAsync<T, U>(string sql, U parameter, string ConnectionStr = "");
    IEnumerable<T> QueryData<T>(string sql, string ConnectionStr = "");
    Task<IEnumerable<T>> QueryDataAsync<T>(string sql, string ConnectionStr = "");
    T QueryScalar<T>(string sql, string ConnectionStr = "");
    Task<T> QueryScalarAsync<T>(string sql, string ConnectionStr = "");
    T QuerySingle<T>(string sql, string ConnectionStr = "");
    Task<T> QuerySingleAsync<T>(string sql, string ConnectionStr = "");
}