using DS.AsciiImport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DS.DataImporter
{
    class ImportSettingsDialogVievModel : INotifyPropertyChanged
    {
        private AsciiSettings asciiSettings;
        public AsciiSettings AsciiSettings { get; set; }
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
