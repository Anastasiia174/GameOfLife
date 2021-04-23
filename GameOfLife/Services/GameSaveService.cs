using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Data;
using GameOfLife.Data.Entities;
using GameOfLife.Engine;
using GameOfLife.Extensions;

namespace GameOfLife.Services
{
    public class GameSaveService : IGameSaveService
    {
        private readonly IGameRepository _gameRepository;

        public GameSaveService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public IEnumerable<GameSave> GetAllGameSaves()
        {
            return _gameRepository.GetAllSaves().Select(s => new GameSave()
            {
                Title = s.SaveTitle,
                DateTime = s.SaveDtm,
                GameEnded = s.SaveGameEnded,
                GenerationNumber = s.SaveGeneration,
                Playground = ImageConverter.ByteArrayToBitmap(s.SaveData),
                UniverseConfiguration =
                    s.SaveIsClosUniv ? UniverseConfiguration.Closed : UniverseConfiguration.Limited
            }).ToList();
        }

        public void SaveGameSave(GameSave save)
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

            _gameRepository.SaveAll();
        }

        public void RemoveGameSave(GameSave save)
        {
            var repoSave = _gameRepository.GetSaveByTitle(save.Title);

            if (repoSave != null)
            {
                _gameRepository.RemoveSave(repoSave);
                _gameRepository.SaveAll();
            }
        }
    }
}
