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
    public class GameLayoutService : IGameLayoutService
    {
        private readonly IGameLayoutsRepository _layoutsRepository;

        public GameLayoutService(IGameLayoutsRepository layoutsRepository)
        {
            _layoutsRepository = layoutsRepository;
        }

        public async Task<IEnumerable<GameLayout>> GetAllGameLayoutsAsync(bool includeLayout = false)
        {
            IEnumerable<GameLayout> result;
            try
            {
                var gameLayouts = await _layoutsRepository.GetAllLayoutsAsync();

                result = gameLayouts.Select(l => new GameLayout()
                {
                    Title = l.LayoutTitle,
                    Layout = includeLayout ? ImageConverter.ByteArrayToBitmap(l.LayoutData) : null,

                }).ToList();
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public async Task<GameLayout> GetGameLayoutByTitleAsync(string title)
        {
            GameLayout result;
            try
            {
                var repoLayout = await _layoutsRepository.GetLayoutByTitleAsync(title);

                if (repoLayout != null)
                {
                    result = new GameLayout()
                    {
                        Title = repoLayout.LayoutTitle,
                        Layout = ImageConverter.ByteArrayToBitmap(repoLayout.LayoutData)
                    };
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public async Task<bool> SaveGameLayoutAsync(GameLayout layout)
        {
            bool result;
            try
            {
                _layoutsRepository.AddLayout(new Layout()
                {
                    LayoutTitle = layout.Title,
                    LayoutData = ImageConverter.ImageToByteArray(layout.Layout, ImageFormat.Bmp)
                });

                result = await _layoutsRepository.SaveAllAsync();
            }
            catch
            {
                result = false;
            }

            return result;
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

        public void Dispose()
        {
            _layoutsRepository?.Dispose();
        }
    }
}
