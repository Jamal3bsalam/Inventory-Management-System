using Inventory.Mostafa.Application.Abstraction.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Mostafa.Pl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseBackupController : ControllerBase
    {
        private readonly IDataBaseBackUpService _backupService;

        public DatabaseBackupController(IDataBaseBackUpService backupService)
        {
            _backupService = backupService;
        }

        /// <summary>
        /// Create database backup
        /// </summary>
        [HttpPost("backup")]
        public async Task<IActionResult> CreateBackup(string folderPath)
        {
            var backupPath = await _backupService.CreateBackupAsync(folderPath);

            return Ok(new
            {
                message = "Database backup created successfully",
                filePath = backupPath
            });
        }

        /// <summary>
        /// Restore database from a backup file
        /// </summary>
        [HttpPost("restore")]
        public async Task<IActionResult> RestoreDatabase([FromBody] string backUpFilePath)
        {
            if (string.IsNullOrWhiteSpace(backUpFilePath))
                return BadRequest("Backup file path is required");

            await _backupService.RestoreBackupAsync(backUpFilePath);

            return Ok(new
            {
                message = "Database restored successfully"
            });
        }

    }
}
