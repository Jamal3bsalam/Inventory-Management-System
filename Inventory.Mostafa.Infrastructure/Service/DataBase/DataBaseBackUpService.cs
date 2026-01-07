using Inventory.Mostafa.Application.Abstraction.DataBase;
using Inventory.Mostafa.Domain.Shared;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Inventory.Mostafa.Infrastructure.Service.DataBase
{
    public class DataBaseBackUpService:IDataBaseBackUpService
    {
        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly string _backupFolder;

        public DataBaseBackUpService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("DefaultConnection not found");

            // الأفضل تجيبه من الـ ConnectionString
            _databaseName = new SqlConnectionStringBuilder(_connectionString).InitialCatalog;

            _backupFolder = @"D:\DbBackups";
        }

        //public async Task<string> CreateBackupAsync()
        //{
        //    if (!Directory.Exists(_backupFolder))
        //        Directory.CreateDirectory(_backupFolder);

        //    var fileName = $"{_databaseName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.bak";
        //    var fullPath = Path.Combine(_backupFolder, fileName);

        //    var sql = $@"
        //    BACKUP DATABASE [{_databaseName}]
        //    TO DISK = N'{fullPath}'
        //    WITH INIT, FORMAT, COMPRESSION;";

        //    using var connection = new SqlConnection(_connectionString);
        //    await connection.OpenAsync();

        //    using var command = new SqlCommand(sql, connection);
        //    await command.ExecuteNonQueryAsync();

        //    return fullPath;
        //}

        public async Task<Result<string>> CreateBackupAsync(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new Exception("Folder path is required");

            var root = Path.GetPathRoot(folderPath);

            if (root is null )
            {
                return Result<string>.Failure("Invalid folder path");
            }

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{_databaseName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.bak";
            var fullPath = Path.Combine(folderPath, fileName);

            var sql = $@"
            BACKUP DATABASE [{_databaseName}]
            TO DISK = N'{fullPath}'
            WITH INIT, FORMAT, COMPRESSION;";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();

            return Result<string>.Success(fullPath);
        }


        public async Task RestoreBackupAsync(string backupFilePath)
        {
            if (!File.Exists(backupFilePath))
                throw new FileNotFoundException("Backup file not found", backupFilePath);

            var masterConnection = new SqlConnectionStringBuilder(_connectionString)
            {
                InitialCatalog = "master"
            }.ConnectionString;

            // اسم الداتابيز الجديدة = اسم الباك-اب بدون .bak
            string newDatabaseName = Path.GetFileNameWithoutExtension(backupFilePath);

            // تحديد مكان ملفات MDF و LDF
            string dataFile = Path.Combine(_backupFolder, $"{newDatabaseName}.mdf");
            string logFile = Path.Combine(_backupFolder, $"{newDatabaseName}_log.ldf");

            var sql = $@"
            RESTORE DATABASE [{newDatabaseName}]
            FROM DISK = N'{backupFilePath}'
            WITH 
                MOVE 'InventorySystem' TO '{dataFile}',
                MOVE 'InventorySystem_log' TO '{logFile}',
                REPLACE;
            ";

            using var connection = new SqlConnection(masterConnection);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}
