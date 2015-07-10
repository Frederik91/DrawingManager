using DrawingManagerApp.Commands;
using DrawingManagerApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace DrawingManagerApp.ViewModels
{
    class FileManagerViewModel : ViewModelBase
    {
        #region Declarations

        private string _xmlFilePath;
        private string _newFileName;
        private string __selectedPathName;

        private ObservableCollection<FilePathList> _filePathList = new ObservableCollection<FilePathList>();
        private List<string> _pathNameList = new List<string>();
        List<string> _folderPathNameList = new List<string>();

        #endregion

        public FileManagerViewModel(string filePath)
        {
            _xmlFilePath = filePath;
            SaveChangesCommand = new DelegateCommand(o => writeChangesToXMLfile());
            SubmitNewFileCommand = new DelegateCommand(o => addNewFileToList());

            checkXMLContaintsFilepaths();
            readFilesFromXML();

            getFilePathList();
        }



        #region Methods

        private void checkXMLContaintsFilepaths()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_xmlFilePath);

            XmlNode node = doc.SelectSingleNode("//Files");

            if (node == null)
            {
                node = doc.SelectSingleNode("//Settings");

                XmlElement folderpathsElement = doc.CreateElement("Files");

                node.AppendChild(folderpathsElement);

                node = doc.SelectSingleNode("//Files");
                string newXML = doc.OuterXml;

                doc.Save(_xmlFilePath);
            }
        }

        private void readFilesFromXML()
        {
            using (XmlReader reader = XmlReader.Create(_xmlFilePath))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "File")
                        {
                            XElement el = (XElement)XNode.ReadFrom(reader);
                            FilePathList.Add(new FilePathList { Filename = el.Attribute("Filename").Value, SelectedFolderPathName = el.Attribute("FolderPathName").Value, FolderPathName = _folderPathNameList });
                        }
                    }
                }
            }
        }

        private void writeChangesToXMLfile()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_xmlFilePath);
            XmlNode node = doc.SelectSingleNode("//Files");

            node.RemoveAll();

            foreach (var file in FilePathList)
            {
                //Lag nytt element FolderPath
                XmlElement filepathElement = doc.CreateElement("File");
                XmlAttribute fileNameAtt = doc.CreateAttribute("Filename");
                XmlAttribute folderPathNameAtt = doc.CreateAttribute("FolderPathName");

                fileNameAtt.Value = file.Filename;
                folderPathNameAtt.Value = file.SelectedFolderPathName;

                node.AppendChild(filepathElement);

                filepathElement.Attributes.Append(fileNameAtt);
                filepathElement.Attributes.Append(folderPathNameAtt);
            }

            string newXML = doc.OuterXml;

            doc.Save(_xmlFilePath);
        }

        private void getFilePathList()
        {

            using (XmlReader reader = XmlReader.Create(_xmlFilePath))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "FolderPath")
                        {
                            XElement el = (XElement)XNode.ReadFrom(reader);

                            _folderPathNameList.Add(el.Attribute("PathName").Value);
                        }
                    }
                }
            }
        }

        #endregion

        #region Properties         

        private void addNewFileToList()
        {
            var templist = new ObservableCollection<FilePathList>();

            templist = FilePathList;

            templist.Add(new FilePathList { SelectedFolderPathName = SelectedPathName, Filename = NewFileName, FolderPathName = _folderPathNameList });

            FilePathList = templist;
        }

        public ObservableCollection<FilePathList> FilePathList
        {
            get { return _filePathList; }
            set
            {
                _filePathList = value;
                OnPropertyChanged("FilePathList");
            }
        }

        public List<string> FolderPathNameList
        {
            get { return _folderPathNameList; }
            set
            {
                _folderPathNameList = value;
                OnPropertyChanged("FolderPathNameList");
            }
        }

        public string SelectedPathName
        {
            get { return __selectedPathName; }
            set
            {
                __selectedPathName = value;
                OnPropertyChanged("SelectedPathName");
            }
        }

        public string NewFileName
        {
            get { return _newFileName; }
            set
            {
                _newFileName = value;
                OnPropertyChanged("NewFileName");
            }
        }

        public ICommand SubmitNewFileCommand { get; private set; }
        public ICommand SaveChangesCommand { get; private set; }

        #endregion

    }
}
