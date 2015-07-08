using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using DrawingManagerApp.Commands;
using System.Windows;

namespace DrawingManagerApp.ViewModels
{
    class FileFoldersViewModel : ViewModelBase
    {
        private string _xmlFilePath;
        private string _selectedFolderPath;
        private string _editedFolderPath;

        private List<string> _folderPathList = new List<string>();
        //Denne delen skal først lese eventuelle filbaner som er lagt inn i xml-filen og lagre de i en liste. 
        //Skal så ha mulighet for å lagre flere filbaner via user input. Lagres i filen når brukeren trykker på lagreknapp.

        public FileFoldersViewModel(string xmlFilePath)
        {
            _xmlFilePath = xmlFilePath;

            NewFolderPathCommand = new DelegateCommand(o => MessageBox.Show("Test"));
            DeleteFolderPathCommand = new DelegateCommand(o => MessageBox.Show("Test"));
            EditFolderPathCommand = new DelegateCommand(o => updateFolderPathList());

            readXMLFile();
        }

        public ICommand EditFolderPathCommand { get; private set; }
        public ICommand NewFolderPathCommand { get; private set; }
        public ICommand DeleteFolderPathCommand { get; private set; }


        private void readXMLFile()
        {
            using (XmlReader reader = XmlReader.Create(_xmlFilePath))
            {
                reader.ReadStartElement("Folderpaths");
                while (reader.Name == "FolderPath")
                {
                    XElement el = (XElement)XNode.ReadFrom(reader);
                    _folderPathList.Add(el.FirstNode.ToString());
                }

                reader.ReadEndElement();
            }
        }

        private void writeListToXMLFile()
        {
            using (XmlWriter writer = XmlWriter.Create(_xmlFilePath))
            {
                writer.WriteStartElement("Folderpaths");
                foreach (var folderPath in _folderPathList)
                {
                    writer.WriteElementString("FolderPath", folderPath);
                }
                writer.WriteEndElement();
            }
        }

        public void updateFolderPathList()
        {
            if (EditedFolderPath != null)
            {
                List<string> tempFolderPathList = new List<string>();

                foreach (var path in FolderPathList)
                {
                    if (path == SelectedFolderPath)
                    {
                        tempFolderPathList.Add(EditedFolderPath);
                    }
                    else
                    {
                        tempFolderPathList.Add(path);
                    }
                }
                FolderPathList = tempFolderPathList.Distinct().ToList();
            }
        }

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

        public List<string> FolderPathList
        {
            get { return _folderPathList; }
            set
            {
                _folderPathList = value;
                OnPropertyChanged("FolderPathList");
            }
        }

        public string SelectedFolderPath
        {
            get { return _selectedFolderPath; }
            set
            {
                _selectedFolderPath = value;
                EditedFolderPath = value;
                OnPropertyChanged("SelectedFolderPath");
            }
        }

        public string EditedFolderPath
        {
            get
            {
                return _editedFolderPath;
            }
            set
            {
                _editedFolderPath = value;
                OnPropertyChanged("EditedFolderPath");
            }
        }



        #endregion

    }
}
