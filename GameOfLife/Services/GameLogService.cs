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
            bool result;
            try
            {
                _gameLogsRepository.AddLog(new Log()
                {
                    LogEvent = log.Event,
                    LogEventDateTime = log.EventDateTime,
                    LogData = log.Playground != null
                        ? ImageConverter.ImageToByteArray(log.Playground, ImageFormat.Bmp)
                        : null
                });


                result = await _gameLogsRepository.SaveAllAsync();
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public void Dispose()
        {
            _gameLogsRepository?.Dispose();
        }
    }
}
