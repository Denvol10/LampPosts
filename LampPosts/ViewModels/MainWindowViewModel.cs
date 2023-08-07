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
        private string _dwgFileUniqueId = (string)Properties.Settings.Default["DwgFileUniqueId"];

        public string DwgFileUniqueId
        {
            get => _dwgFileUniqueId;
            set => Set(ref _dwgFileUniqueId, value);
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
        }


        #region Конструктор класса MainWindowViewModel
        public MainWindowViewModel(RevitModelForfard revitModel)
        {
            RevitModel = revitModel;

            //TODO Добавить инициализацию значения файла dwg из settings

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
