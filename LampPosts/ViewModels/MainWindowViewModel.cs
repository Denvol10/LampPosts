using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using System.Collections.ObjectModel;
using System.Windows.Input;
using LampPosts.Infrastructure;
using LampPosts.Models;

namespace LampPosts.ViewModels
{
    internal class MainWindowViewModel : Base.ViewModel
    {
        private RevitModelForfard _revitModel;

        internal RevitModelForfard RevitModel
        {
            get => _revitModel;
            set => _revitModel = value;
        }

        #region Заголовок
        private string _title = "Фонари";

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        #endregion

        #region Id dwg файла
        private string _dwgFileUniqueId;

        public string DwgFileUniqueId
        {
            get => _dwgFileUniqueId;
            set => Set(ref _dwgFileUniqueId, value);
        }
        #endregion

        #region Список семейств и их типоразмеров
        private ObservableCollection<FamilySymbolSelector> _genericModelFamilySymbols = new ObservableCollection<FamilySymbolSelector>();
        public ObservableCollection<FamilySymbolSelector> GenericModelFamilySymbols
        {
            get => _genericModelFamilySymbols;
            set => Set(ref _genericModelFamilySymbols, value);
        }
        #endregion

        #region Выбранный типоразмер семейства
        private FamilySymbolSelector _familySymbolName;
        public FamilySymbolSelector FamilySymbolName
        {
            get => _familySymbolName;
            set => Set(ref _familySymbolName, value);
        }
        #endregion

        #region Команды

        #region Получение DWG файла
        public ICommand GetDWGFileCommand { get; }

        private void OnGetDWGFileCommandExecuted(object parameter)
        {
            RevitCommand.mainView.Hide();
            RevitModel.GetDWGFileBySelection();
            DwgFileUniqueId = RevitModel.DwgFileUniqueId;
            RevitCommand.mainView.ShowDialog();
        }

        private bool CanGetDWGFileCommandExecute(object parameter)
        {
            return true;
        }
        #endregion

        #region Закрыть окно
        public ICommand CloseWindow { get; }

        private void OnCloseWindowCommandExecuted(object parameter)
        {
            SaveSettings();
            RevitCommand.mainView.Close();
        }

        private bool CanCloseWindowCommandExecute(object parameter)
        {
            return true;
        }
        #endregion

        #endregion

        private void SaveSettings()
        {
            Properties.Settings.Default["DwgFileUniqueId"] = DwgFileUniqueId;
            Properties.Settings.Default.Save();
        }


        #region Конструктор класса MainWindowViewModel
        public MainWindowViewModel(RevitModelForfard revitModel)
        {
            RevitModel = revitModel;

            GenericModelFamilySymbols = RevitModel.GetFamilySymbolNames();

            #region Инициализация значения dwg файла
            if (!(Properties.Settings.Default["DwgFileUniqueId"] is null))
            {
                string dwgFileUniqueIdInSettings = Properties.Settings.Default["DwgFileUniqueId"].ToString();
                if (RevitModel.IsDwgFileExistInModel(dwgFileUniqueIdInSettings) && !string.IsNullOrEmpty(dwgFileUniqueIdInSettings))
                {
                    DwgFileUniqueId = dwgFileUniqueIdInSettings;
                    RevitModel.GetDWGFileBySettings(dwgFileUniqueIdInSettings);
                }
            }
            #endregion

            #region Команды
            GetDWGFileCommand = new LambdaCommand(OnGetDWGFileCommandExecuted, CanGetDWGFileCommandExecute);
            CloseWindow = new LambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
            #endregion
        }

        public MainWindowViewModel()
        { }
        #endregion
    }
}
