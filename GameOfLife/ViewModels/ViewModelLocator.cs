using System.Collections.Generic;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GameOfLife.Data;
using GameOfLife.Engine;
using GameOfLife.Infrastructure;
using GameOfLife.Services;
using MahApps.Metro.IconPacks;

namespace GameOfLife.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var defaultConfiguration = new GameConfiguration(20, 20, UniverseConfiguration.Limited, false);

            var context = new GameContext();

            bool isDbConnected = context.Database.Exists();

            IGameSavesRepository gameRepository = new GameSavesRepository(context);
            IGameSaveService saveService = new GameSaveService(gameRepository);
            IGameLogsRepository gameLogsRepository = new GameLogsRepository(context);
            IGameLogService gameLogService = new GameLogService(gameLogsRepository);
            IGameLayoutsRepository gameLayoutsRepository = new GameLayoutsRepository(context);
            IGameLayoutService layoutService = new GameLayoutService(gameLayoutsRepository);
            IDialogService dialogService = new DialogService();
            IGameLogger gameLogger = new GameGameLogger();

            if (!isDbConnected)
            {
                dialogService.ShowMessage("Could not connect to database. Saving, layouts and logging would not be available.");
            }

            var main = new MainViewModel();
            var playground = new PlaygroundViewModel(defaultConfiguration, gameLogger, main);
            var settings = new SettingsViewModel(defaultConfiguration, main);
            var saves = new SavesViewModel(saveService, dialogService, main);
            var logs = new LogsViewModel(isDbConnected ? gameLogService : null, dialogService, main);
            var layouts = new LayoutsViewModel(layoutService, dialogService, main);

            SimpleIoc.Default.Register<IGameSavesRepository>(() => gameRepository);
            SimpleIoc.Default.Register<IGameSaveService>(() => saveService);
            SimpleIoc.Default.Register<IGameLogsRepository>(() => gameLogsRepository);
            SimpleIoc.Default.Register<IGameLogService>(() => gameLogService);
            SimpleIoc.Default.Register<IDialogService>(() => dialogService);
            SimpleIoc.Default.Register<IGameLogger>(() => gameLogger);

            SimpleIoc.Default.Register<MainViewModel>(() => main);
            SimpleIoc.Default.Register<PlaygroundViewModel>(() => playground);
            SimpleIoc.Default.Register<SettingsViewModel>(() => settings);
            SimpleIoc.Default.Register<SavesViewModel>(() => saves);
            SimpleIoc.Default.Register<LogsViewModel>(() => logs);
            SimpleIoc.Default.Register<LayoutsViewModel>(() => layouts);

            main.CreateMenuItems(
                new List<MenuItemViewModel>()
                {
                    playground.SetStyle(new PackIconMaterial() { Kind = PackIconMaterialKind.GoogleController }, "Play game"), 
                    layouts.SetStyle(new PackIconMaterial() {Kind = PackIconMaterialKind.DotsHexagon}, "Layouts", isDbConnected),
                    saves.SetStyle(new PackIconMaterial() { Kind = PackIconMaterialKind.ContentSaveAll }, "Saved games", isDbConnected), 
                    logs.SetStyle(new PackIconMaterial() { Kind = PackIconMaterialKind.Server }, "Game logs", isDbConnected)
                },
                new List<MenuItemViewModel>()
                {
                    settings.SetStyle(new PackIconMaterial() { Kind = PackIconMaterialKind.Cog }, "Settings")
                });
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public PlaygroundViewModel Playground => ServiceLocator.Current.GetInstance<PlaygroundViewModel>();

        public SavesViewModel Saves => ServiceLocator.Current.GetInstance<SavesViewModel>();

        public LogsViewModel Logs => ServiceLocator.Current.GetInstance<LogsViewModel>();

        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public LayoutsViewModel Layouts => ServiceLocator.Current.GetInstance<LayoutsViewModel>();
    }
}
