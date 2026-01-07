

using Inventory.Mostafa.Domain.Shared;

namespace Inventory.Mostafa.Application.Abstraction.DataBase
{
    public interface IDataBaseBackUpService
    {
        Task<Result<string>> CreateBackupAsync(string folderPath);
        Task RestoreBackupAsync(string backupFilePath);
    }
}
