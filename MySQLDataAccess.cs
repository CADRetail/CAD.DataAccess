using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net.NetworkInformation;

namespace CAD.DataAccess;

public class MySQLDataAccess : IDataAccess
{
    public string ConnStr { get; set; }
    public ILogger _logger { get; set; }
    [ActivatorUtilitiesConstructor]
    public MySQLDataAccess(IConfiguration config, ILogger<MySQLDataAccess> logger)
    {
        ConnStr = config.GetConnectionString("DefaultConnection");
        ConnStr = GetConnStrFromEnv();
        _logger = logger;
    }
    public MySQLDataAccess(string ConnectionStr)
    {
        ConnStr = GetConnStrFromEnv();
        if (ConnectionStr != "")
            ConnStr = ConnectionStr;
    }

    public IEnumerable<T> QueryData<T, U>(string sql, U parameter, string ConnectionStr = "")
    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));
        try
        {
            return connection.Query<T>(sql, parameter);
        }
        catch (Exception e)
        {
            if (_logger != null)
                _logger.Log(LogLevel.Error, e.Message);
            return Enumerable.Empty<T>();
        }
    }

    public async Task<IEnumerable<T>> SpQueryData<T, U>(string storedProcedure, U Parameters, string ConnectionStr = "")

    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));

        try
        {
            return await connection.QueryAsync<T>(storedProcedure, Parameters, commandType: CommandType.StoredProcedure);
        }
        catch (Exception e)
        {
            if (_logger != null)
                _logger.Log(LogLevel.Error, e.Message);
            return Enumerable.Empty<T>();
        }
    }

    public async Task<IEnumerable<T>> SpQueryData<T>(string storedProcedure, string ConnectionStr = "")

    {
        return await SpQueryData<T, dynamic>(storedProcedure, new { }, ConnectionStr);
    }

    public IEnumerable<T> QueryData<T>(string sql, string ConnectionStr = "")
    {
        return QueryData<T, dynamic>(sql, new { }, ConnectionStr);
    }
    public async Task<IEnumerable<T>> QueryDataAsync<T, U>(string sql, U parameter, string ConnectionStr = "")
    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));
        try
        {
            return await connection.QueryAsync<T>(sql, parameter);
        }
        catch (Exception e)
        {
            if (_logger != null)
                _logger.Log(LogLevel.Error, e.Message);
            return Enumerable.Empty<T>();
        }
    }
    public async Task<IEnumerable<T>> QueryDataAsync<T>(string sql, string ConnectionStr = "")
    {
        return await QueryDataAsync<T, dynamic>(sql, new { }, ConnectionStr);
    }
    public void Execute<T>(string sql, T parameter, string ConnectionStr = "")
    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));
        try
        {
            connection.Execute(sql, parameter);
        }
        catch (Exception e)
        {
            if (_logger != null) _logger.Log(LogLevel.Error, e.Message);
        }
    }
    public void Execute(string sql, string ConnectionStr = "")
    {
        Execute<dynamic>(sql, new { }, ConnectionStr);
    }
    public async Task ExecuteAsync<T>(string sql, T parameter, string ConnectionStr = "")
    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));
        try
        {
            await connection.ExecuteAsync(sql, parameter);
        }
        catch (Exception e)
        {
            if (_logger != null) _logger.Log(LogLevel.Error, e.Message);
        }
    }

    public async Task ExecuteAsync(string sql, string ConnectionStr = "")
    {
        await ExecuteAsync<dynamic>(sql, new { }, ConnectionStr);
    }
    public T QueryScalar<T>(string sql, string ConnectionStr = "")
    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));
        try
        {
            return connection.ExecuteScalar<T>(sql);
        }
        catch (Exception e)
        {
            if (_logger != null) _logger.Log(LogLevel.Error, e.Message);
            return default;
        }
    }
    public async Task<T> QueryScalarAsync<T>(string sql, string ConnectionStr = "")
    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));
        try
        {
            return await connection.ExecuteScalarAsync<T>(sql);
        }
        catch (Exception e)
        {
            if (_logger != null) _logger.Log(LogLevel.Error, e.Message);
            return default;
        }
    }
    public T QuerySingle<T>(string sql, string ConnectionStr = "")
    {
        return QuerySingle<T, dynamic>(sql, new { }, ConnectionStr);
    }

    public T QuerySingle<T,U>(string sql, U parameter, string ConnectionStr = "")
    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));
        try
        {
            return connection.QuerySingle<T>(sql, parameter);
        }
        catch (Exception e)
        {
            if (_logger != null) _logger.Log(LogLevel.Error, e.Message);
            return default;
        }
    }
    public async Task<T> QuerySingleAsync<T>(string sql, string ConnectionStr = "")
    {
        return await QuerySingleAsync<T, dynamic>(sql, new { }, ConnectionStr);
    }

    public async Task<T> QuerySingleAsync<T, U>(string sql, U parameter, string ConnectionStr = "")
    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));
        try
        {
            return await connection.QuerySingleAsync<T>(sql);
        }
        catch (Exception e)
        {
            if (_logger != null) _logger.Log(LogLevel.Error, e.Message);
            return default;
        }
    }

    public long Insert<T>(T data, string ConnectionStr = "") where T : class
    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));
        try
        {
            return connection.Insert<T>(data);
        }
        catch (Exception e)
        {
            if (_logger != null) _logger.Log(LogLevel.Error, e.Message);
            return default;
        }
    }
    public long Insert<T>(List<T> data, string ConnectionStr = "") where T : class
    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));
        try
        {
            return connection.Insert<List<T>>(data);
        }
        catch (Exception e)
        {
            if (_logger != null) _logger.Log(LogLevel.Error, e.Message);
            return 0;
        }
    }
    public async Task<int> InsertAsync<T>(T data, string ConnectionStr = "") where T : class
    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));
        try
        {
            return await connection.InsertAsync<T>(data);
        }
        catch (Exception e)
        {
            if (_logger != null) _logger.Log(LogLevel.Error, e.Message);
            return 0;
        }

    }
    public async Task<int> InsertAsync<T>(List<T> data, string ConnectionStr = "") where T : class
    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));
        try
        {
            return await connection.InsertAsync<List<T>>(data);
        }
        catch (Exception e)
        {
            if (_logger != null) _logger.Log(LogLevel.Error, e.Message);
            return 0;
        }

    }
    public bool PingHost(string nameOrAddress)
    {
        bool pingable = false;
        Ping pinger = pinger = new Ping();

        try
        {
            PingReply reply = pinger.Send(nameOrAddress);
            pingable = reply.Status == IPStatus.Success;
        }
        catch (PingException)
        {
            // Discard PingExceptions and return false;
        }
        finally
        {
            if (pinger != null)
            {
                pinger.Dispose();
            }
        }
        return pingable;
    }
    public string GetConnstr(string IP, string Db, string userName, string password)
    {
        return $"User ID ={userName}; Password ={password}; Data Source ={IP}; Database ={Db};SslMode=none";
    }
    public string GetConnStr(string ConnectionStr = "")
    {
        var _connstr = ConnectionStr;
        _connstr = _connstr == "" ? ConnStr : ConnectionStr;
        return _connstr;
    }
    string GetConnStrFromEnv()
    {
        var DbIp = Environment.GetEnvironmentVariable("DBIP");
        var Dbname = Environment.GetEnvironmentVariable("DBNAME");
        var DbUser = Environment.GetEnvironmentVariable("DBUSER");
        var DbPass = Environment.GetEnvironmentVariable("DBPASS");

        if (DbIp == null || DbIp == "" || Dbname == null || Dbname == "" || DbUser == null || DbUser == "" || DbPass == null || DbPass == "")
        {
            return ConnStr;
        }
        return GetConnstr(DbIp, Dbname, DbUser, DbPass);
    }
    public bool Update<T>(T data, string ConnectionStr = "") where T : class
    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));
        try
        {
            return connection.Update<T>(data);
        }
        catch (Exception e)
        {
            if (_logger != null) _logger.Log(LogLevel.Error, e.Message);
            return false;
        }
    }

    public async Task<bool> UpdateAsync<T>(T data, string ConnectionStr = "") where T : class
    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));
        try
        {
            return await connection.UpdateAsync<T>(data);
        }
        catch (Exception e)
        {
            if (_logger != null) _logger.Log(LogLevel.Error, e.Message);
            return false;
        }
    }

    public async Task<bool> UpdateAsync<T>(List<T> data, string ConnectionStr = "") where T : class
    {
        using IDbConnection connection = new MySqlConnection(GetConnStr(ConnectionStr));
        try
        {
            return await connection.UpdateAsync<List<T>>(data);
        }
        catch (Exception e)
        {
            if (_logger != null) _logger.Log(LogLevel.Error, e.Message);
            return false;
        }
    }


}