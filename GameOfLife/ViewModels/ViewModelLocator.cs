using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
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

            IGameRepository gameRepository = new GameRepository(new GameContext());
            IGameSaveService saveService = new GameSaveService(gameRepository);

            var main = new MainViewModel();
            var playground = new PlaygroundViewModel(defaultConfiguration, saveService, main);
            var settings = new SettingsViewModel(defaultConfiguration, main);
            var saves = new SavesViewModel(saveService, main);
            var logs = new LogsViewModel(main);

            SimpleIoc.Default.Register<IGameRepository>(() => gameRepository);
            SimpleIoc.Default.Register<IGameSaveService>(() => saveService);
            SimpleIoc.Default.Register<MainViewModel>(() => main);
            SimpleIoc.Default.Register<PlaygroundViewModel>(() => playground);
            SimpleIoc.Default.Register<SettingsViewModel>(() => settings);
            SimpleIoc.Default.Register<SavesViewModel>(() => saves);
            SimpleIoc.Default.Register<LogsViewModel>(() => logs);

            main.CreateMenuItems(
                new List<MenuItemViewModel>()
                {
                    playground.SetStyle(new PackIconMaterial() { Kind = PackIconMaterialKind.MicrosoftXboxController }, "Play game"), 
                    saves.SetStyle(new PackIconMaterial() { Kind = PackIconMaterialKind.ContentSaveAll }, "Saved games"), 
                    logs.SetStyle(new PackIconMaterial() { Kind = PackIconMaterialKind.Server }, "Game logs")
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
    }
}
