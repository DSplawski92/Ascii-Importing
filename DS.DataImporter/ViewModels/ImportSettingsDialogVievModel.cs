using DS.AsciiImport;
using DS.Interfaces;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DS.DataImporter
{
    class ImportSettingsDialogVievModel : INotifyPropertyChanged
    {
        public AsciiSettings AsciiSettings { get; set; }
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
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "Ascii files (*.txt; *.csv)|*.txt;*.csv|Binary files (*.xls, *.xlsx, *.ods)|*.xls;*.xlsx;*.ods";
            if (fileDialog.ShowDialog() == true)
            {
                AsciiSettings.FileName = fileDialog.FileName;
            }
        }

        private bool CanCloseDialogExecute()
        {
            if (string.IsNullOrWhiteSpace(AsciiSettings.ColumnDelimiter))
            {
                return false;
            }
            else if (string.IsNullOrWhiteSpace(AsciiSettings.DateTimeFormat))
            {
                return false;
            }
            else if(string.IsNullOrWhiteSpace(AsciiSettings.FileName))
            {
                return false;
            }
            else if (string.IsNullOrWhiteSpace(AsciiSettings.NumberDelimiter))
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

        public ImportSettingsDialogVievModel()
        {
            AsciiSettings = new AsciiSettings();
        }
    }
}
