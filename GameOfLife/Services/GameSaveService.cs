﻿using System;
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
        private readonly IGameSavesRepository _gameRepository;

        public GameSaveService(IGameSavesRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<IEnumerable<GameSave>> GetAllGameSavesAsync()
        {
            var gameSaves = await _gameRepository.GetAllSavesAsync();

            return gameSaves.Select(s => new GameSave()
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