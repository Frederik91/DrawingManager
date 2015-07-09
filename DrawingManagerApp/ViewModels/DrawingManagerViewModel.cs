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
    public class DrawingManagerViewModel : ViewModelBase
    {
        #region declarations

        private ViewModelBase _currentViewModel;
        string _xmlFilePath = Directory.GetCurrentDirectory() + "\\Datafile.xml";

        #endregion


        public DrawingManagerViewModel()
        {
            //FORANDRING nummer to 123
            //FORANDRING nummer to 123
            //dette er noe sykt kult som heter merge,
            //det er ikke skummelt
            startupSequence();
            FileFoldersCommand = new DelegateCommand(o => OpenFileFolderWindow());
        }


        public ICommand FileFoldersCommand { get; private set; }



        #region Methods

        private void startupSequence()
        {


            //Sjekker for XML-fil og lager ny dersom den ikke eksisterer.
            if (!File.Exists(_xmlFilePath))
            {

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;

                using (XmlWriter writer = XmlWriter.Create(_xmlFilePath, settings))
                {
                    writer.WriteStartElement("DrawingManagerXML");

                    writer.WriteStartElement("Settings");
                    writer.WriteEndElement();
                    writer.WriteStartElement("Data");
                    writer.WriteEndElement();

                    writer.WriteEndDocument();

                    MessageBox.Show("No XML-file found. New file created in location:" + _xmlFilePath);
                }
            }

            //Leser inn innholdet i XML-filen. Dette skal lagres i en liste som skal vises i DataContext.
            //using (XmlReader reader = XmlReader.Create(_xmlFilePath))
            //{
                //while (reader.Read())
                //{
                //    switch (reader.NodeType)
                //    {
                //        case XmlNodeType.Element:
                //            //Kode lages etter at xml-struktur er bestemt.
                //            break;
                //        case XmlNodeType.Text:
                //            //Kode lages etter at xml-struktur er bestemt.
                //            break;
                //        case XmlNodeType.EndElement:
                //            //Kode lages etter at xml-struktur er bestemt.
                //            break;
                //    }
                //}
            //}

        }

        public void OpenFileFolderWindow()
        {
            FileFoldersViewModel model = new FileFoldersViewModel(_xmlFilePath);
            CurrentViewModel = model;
        }

        #endregion

        #region Properties

        public string XMLFilePath
        {
            get { return _xmlFilePath; }
            set
            {
                _xmlFilePath = value;
                OnPropertyChanged("XMLFilePath");
            }

        }

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                this.OnPropertyChanged("CurrentViewModel");
            }
        }


        #endregion



    }
}
