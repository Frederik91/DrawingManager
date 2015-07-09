using DrawingManagerApp.Commands;
using DrawingManagerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace DrawingManagerApp.ViewModels
{
    class FileManagerViewModel : ViewModelBase
    {
        private string _xmlFilePath;

        private List<FilePathList> _filePathList = new List<Models.FilePathList>();
        private List<string> _pathNameList = new List<string>();


        public FileManagerViewModel(string filePath)
        {
            _xmlFilePath = filePath;
            SubmitNewFileCommand = new DelegateCommand(o => addNewFileToList());


            startupSequence();
        }

        public ICommand SubmitNewFileCommand;

        private void startupSequence()
        {
            List<string> folderPathNameList = new List<string>();
            for (int i = 0; i < 2; i++)
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
                                if (i == 0)
                                {
                                    XElement el = (XElement)XNode.ReadFrom(reader);

                                    PathNameList.Add(el.Attribute("PathName").Value);
                                }
                                if (i == 1)
                                {
                                    XElement el = (XElement)XNode.ReadFrom(reader);

                                    _filePathList.Add(new FilePathList { FolderPathName = PathNameList, FolderPath = el.Attribute("FolderPath").Value });
                                }

                            }
                        }
                    }
                }
            }

            _filePathList = _filePathList.OrderBy(x => x.FolderPathName).ThenBy(x => x.Filename).ToList();
        }

        private void addNewFileToList()
        {
            MessageBox.Show("test");
        }

        public List<FilePathList> FilePathList
        {
            get { return _filePathList; }
            set
            {
                _filePathList = value;
                OnPropertyChanged("FilePathList");
            }
        }

        public List<string> PathNameList
        {
            get { return _pathNameList; }
            set
            {
                _pathNameList = value;
                OnPropertyChanged("PathNameList");
            }
        }

    }
}
