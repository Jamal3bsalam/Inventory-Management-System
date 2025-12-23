

namespace Inventory.Mostafa.Application.Abstraction.DataBase
{
    public interface IDataBaseBackUpService
    {
        Task<string> CreateBackupAsync(string folderPath);
        Task RestoreBackupAsync(string backupFilePath);
    }
}
