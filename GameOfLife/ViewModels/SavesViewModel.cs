using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GameOfLife.Data;
using GameOfLife.Data.Entities;

namespace GameOfLife.ViewModels
{
    public class SavesViewModel : MenuItemViewModel
    {
        private readonly IGameRepository _gameRepository;

        public SavesViewModel(IGameRepository gameRepository, MainViewModel mainViewModel) : base(mainViewModel)
        {
            _gameRepository = gameRepository;

            var saves = _gameRepository.GetAllSaves();
            Saves = new ObservableCollection<Save>(saves);
        }

        private ObservableCollection<Save> _saves;

        public ObservableCollection<Save> Saves
        {
            get => _saves;
            set
            {
                Set(() => Saves, ref _saves, value);
            }
        }

        private Save _selectedSave;

        public Save SelectedSave
        {
            get => _selectedSave;
            set
            {
                Set(() => SelectedSave, ref _selectedSave, value);
            }
        }

        private RelayCommand _loadCommand;

        public RelayCommand LoadCommand =>
            _loadCommand ??
            (_loadCommand = new RelayCommand(LoadSave));

        private RelayCommand _loadRandomCommand;

        public RelayCommand LoadRandomCommand =>
            _loadRandomCommand ??
            (_loadRandomCommand = new RelayCommand(LoadRandom));

        private RelayCommand _removeCommand;

        public RelayCommand RemoveCommand =>
            _removeCommand ??
            (_removeCommand = new RelayCommand(RemoveSave));

        private void LoadSave()
        {

        }

        private void LoadRandom()
        {

        }

        private void RemoveSave()
        {
            
        }
    }
}
