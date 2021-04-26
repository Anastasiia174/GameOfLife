using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Data;
using GameOfLife.Data.Entities;
using GameOfLife.Extensions;
using GameOfLife.Infrastructure;

namespace GameOfLife.Services
{
    public class GameLayoutService : IGameLayoutService
    {
        private readonly IGameLayoutsRepository _layoutsRepository;

        public GameLayoutService(IGameLayoutsRepository layoutsRepository)
        {
            _layoutsRepository = layoutsRepository;
        }
        public async Task<IEnumerable<GameLayout>> GetAllGameLayoutsAsync(bool includeLayout = false)
        {
            var gameLayouts = await _layoutsRepository.GetAllLayoutsAsync();

            return gameLayouts.Select(l => new GameLayout()
            {
                Title = l.LayoutTitle,
                Layout = includeLayout ? ImageConverter.ByteArrayToBitmap(l.LayoutData) : null,
                
            }).ToList();
        }

        public async Task<GameLayout> GetGameLayoutByTitleAsync(string title)
        {
            var repoLayout = await _layoutsRepository.GetLayoutByTitleAsync(title);

            if (repoLayout != null)
            {
                return new GameLayout()
                {
                    Title = repoLayout.LayoutTitle,
                    Layout = ImageConverter.ByteArrayToBitmap(repoLayout.LayoutData)
                };
            }

            return null;
        }

        public async Task<bool> SaveGameLayoutAsync(GameLayout layout)
        {
            _layoutsRepository.AddLayout(new Layout()
            {
                LayoutTitle = layout.Title,
                LayoutData = ImageConverter.ImageToByteArray(layout.Layout, ImageFormat.Bmp)
            });

            return await _layoutsRepository.SaveAllAsync();
        }

        public async Task<bool> RemoveGameLayoutAsync(GameLayout layout)
        {
            var repoLayout = await _layoutsRepository.GetLayoutByTitleAsync(layout.Title);

            if (repoLayout != null)
            {
                _layoutsRepository.RemoveLayout(repoLayout);
                return await _layoutsRepository.SaveAllAsync();
            }

            return false;
        }
    }
}
