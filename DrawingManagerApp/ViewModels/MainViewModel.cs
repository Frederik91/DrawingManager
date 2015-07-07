using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows;
using DrawingManagerApp.Views;
using System.Windows.Input;
using System.ComponentModel;
using DrawingManagerApp.Commands;


namespace DrawingManagerApp.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        #region declarations

        string _xmlFilePath = Directory.GetCurrentDirectory() + "\\Datafile.xml";


        #endregion


        public MainViewModel()
        {
            
            //FORANDRING nummer to yolo
            //FORANDRING nummer to yolo
            //FORANDRING nummer to yolo//FORANDRING nummer to yolo
            //FORANDRING nummer to yolo
            startupSequence();
            FileFoldersCommand = new FileFoldersCommand(this);
        }


        #region Methods

        private void startupSequence()
        {

            //Sjekker for XML-fil og lager ny dersom den ikke eksisterer.
            if (!File.Exists(_xmlFilePath))
            {
                using (XmlWriter writer = XmlWriter.Create(_xmlFilePath))
                {
                    writer.WriteComment("Drawing Manager XML Datafile");
                    MessageBox.Show("No XML-file found. New file created in location:" + _xmlFilePath);
                }
            }

            //Leser inn innholdet i XML-filen. Dette skal lagres i en liste som skal vises i DataContext.
            using (XmlReader reader = XmlReader.Create(_xmlFilePath))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            //Kode lages etter at xml-struktur er bestemt.
                            break;
                        case XmlNodeType.Text:
                            //Kode lages etter at xml-struktur er bestemt.
                            break;
                        case XmlNodeType.EndElement:
                            //Kode lages etter at xml-struktur er bestemt.
                            break;
                    }
                }
            }

        }

        public void OpenFileFolderWindow(FileFoldersWindow window)
        {
            FileFoldersWindow win = new FileFoldersWindow();
            win.Show();
            OnPropertyChanged();
        }

        #endregion

        #region Properties

        public string XMLFilePath
        {
            get { return _xmlFilePath; }
            set
            {
                _xmlFilePath = value;
                OnPropertyChanged();
            }

        }

        public ICommand FileFoldersCommand { get; set; }

        #endregion


        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
