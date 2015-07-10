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
            startupSequence();
            FileFoldersCommand = new DelegateCommand(o => OpenFileFolderWindow());
            FileManagerCommand = new DelegateCommand(o => OpenFileManagerView());
            AttributeManagerCommand = new DelegateCommand(o => OpenAttributeManagerView());
        }





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
        }

        public void OpenFileFolderWindow()
        {
            CurrentViewModel = new FileFoldersViewModel(_xmlFilePath);
        }

        public void OpenFileManagerView()
        {
            CurrentViewModel = new FileManagerViewModel(_xmlFilePath);
        }

        public void OpenAttributeManagerView()
        {
            CurrentViewModel = new AttributeManagerViewModel(_xmlFilePath);
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

        public ICommand AttributeManagerCommand { get; private set; }
        public ICommand FileFoldersCommand { get; private set; }
        public ICommand FileManagerCommand { get; private set; }


        #endregion



    }
}
