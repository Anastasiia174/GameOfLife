using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using GameOfLife.Data;
using GameOfLife.Data.Entities;
using GameOfLife.Extensions;
using GameOfLife.Infrastructure;

namespace GameOfLife.Services
{
    public class GameLogService : IGameLogService
    {
        private readonly IGameLogsRepository _gameLogsRepository;

        public GameLogService(IGameLogsRepository gameLogsRepository)
        {
            _gameLogsRepository = gameLogsRepository;
        }

        public async Task<IEnumerable<GameLog>> GetAllGameLogsAsync()
        {
            var gameLogs = await _gameLogsRepository.GetAllLogsAsync();
            return gameLogs.Select(log => new GameLog()
            {
                Event = log.LogEvent,
                EventDateTime = log.LogEventDateTime
            }).ToList();
        }

        public async Task<bool> SaveGameLogAsync(GameLog log)
        {
            _gameLogsRepository.AddLog(new Log()
            {
                LogEvent = log.Event,
                LogEventDateTime = log.EventDateTime,
                LogData = log.Playground != null ? ImageConverter.ImageToByteArray(log.Playground, ImageFormat.Bmp) : null
            });

            return await _gameLogsRepository.SaveAllAsync();
        }
    }
}
