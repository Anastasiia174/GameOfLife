using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using GameOfLife.Data;
using GameOfLife.Data.Entities;
using GameOfLife.Engine;
using GameOfLife.Extensions;

namespace GameOfLife.Services
{
    public class GameSaveService : IGameSaveService
    {
        private readonly IGameSavesRepository _gameRepository;

        public GameSaveService(IGameSavesRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<IEnumerable<GameSave>> GetAllGameSavesAsync(bool includePlayground = false)
        {
            var gameSaves = await _gameRepository.GetAllSavesAsync();

            return gameSaves.Select(s => new GameSave()
            {
                Title = s.SaveTitle,
                DateTime = s.SaveDtm,
                GameEnded = s.SaveGameEnded,
                GenerationNumber = s.SaveGeneration,
                Playground = includePlayground ? ImageConverter.ByteArrayToBitmap(s.SaveData) : null,
                UniverseConfiguration =
                    s.SaveIsClosUniv ? UniverseConfiguration.Closed : UniverseConfiguration.Limited
            }).ToList();
        }

        public async Task<GameSave> GetGameSaveByTitleAsync(string title)
        {
            var repoSave = await _gameRepository.GetSaveByTitleAsync(title);

            if (repoSave != null)
            {
                return new GameSave()
                {
                    Title = repoSave.SaveTitle,
                    DateTime = repoSave.SaveDtm,
                    GameEnded = repoSave.SaveGameEnded,
                    GenerationNumber = repoSave.SaveGeneration,
                    Playground = ImageConverter.ByteArrayToBitmap(repoSave.SaveData),
                    UniverseConfiguration = repoSave.SaveIsClosUniv ? UniverseConfiguration.Closed : UniverseConfiguration.Limited
                };
            }

            return null;
        }

        public async Task<bool> SaveGameSaveAsync(GameSave save)
        {
            _gameRepository.AddSave(new Save()
            {
                SaveDtm = save.DateTime,
                SaveGameEnded = save.GameEnded,
                SaveGeneration = save.GenerationNumber,
                SaveTitle = save.Title,
                SaveIsClosUniv = save.UniverseConfiguration == UniverseConfiguration.Closed ? true : false,
                SaveData = ImageConverter.ImageToByteArray(save.Playground, ImageFormat.Bmp)
            });

           return await _gameRepository.SaveAllAsync();
        }

        public async Task<bool> RemoveGameSaveAsync(GameSave save)
        {
            var repoSave = await _gameRepository.GetSaveByTitleAsync(save.Title);

            if (repoSave != null)
            {
                _gameRepository.RemoveSave(repoSave);
                return await _gameRepository.SaveAllAsync();
            }

            return false;
        }
    }
}
