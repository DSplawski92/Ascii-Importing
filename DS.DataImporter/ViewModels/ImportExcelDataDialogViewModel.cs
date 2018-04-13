using DS.ExcelImport;
using DS.Interfaces;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DS.DataImporter.ViewModels
{
    class ImportExcelDataDialogViewModel : INotifyPropertyChanged
    {
        private ExcelSettings excelSettings;
        public ExcelSettings ExcelSettings
        {
            get
            {
                return excelSettings;
            }
            set
            {
                if (excelSettings != value)
                {
                    excelSettings = value;
                    OnPropertyChanged();
                }
            }
        }
        public string[] DateTimeFormats
        {
            get
            {
                return new[]
                {
                    "dd.MM.yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss",
                    "yyyy.MM.dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "yyyy/MM/dd HH:mm:ss",
                };
            }
        }
        public ICommand FileDialog { get { return new RelayCommand(OpenFileDialogExecute, () => true); } }
        public ICommand SuccessCloseDialog { get { return new RelayCommand(SuccessCloseDialogExecute, CanCloseDialogExecute); } }
        public ICommand CancelCloseDialog { get { return new RelayCommand(CancelCloseDialogExecute, () => true); } }

        private void OpenFileDialogExecute(object parameter)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "Binary files (*.xls, *.xlsx, *.ods)|*.xls;*.xlsx;*.ods"
            };

            if (fileDialog.ShowDialog() == true)
            {
                ExcelSettings.FileName = fileDialog.FileName;
                OnPropertyChanged("ExcelSettings");
            }
        }

        private bool CanCloseDialogExecute()
        {
            if (string.IsNullOrWhiteSpace(ExcelSettings.DateTimeFormat))
            {
                return false;
            }
            else if (string.IsNullOrWhiteSpace(ExcelSettings.FileName))
            {
                return false;
            }

            return true;
        }

        private void SuccessCloseDialogExecute(object parameter)
        {
            (parameter as Window).DialogResult = true;
        }

        private void CancelCloseDialogExecute(object parameter)
        {
            (parameter as Window).DialogResult = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ImportExcelDataDialogViewModel()
        {
            ExcelSettings = new ExcelSettings();
        }
    }
}
