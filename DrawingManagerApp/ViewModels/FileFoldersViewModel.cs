using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using DrawingManagerApp.Commands;
using DrawingManagerApp.Models;

namespace DrawingManagerApp.ViewModels
{
    class FileFoldersViewModel : ViewModelBase
    {
        private string _xmlFilePath;

        private List<FolderPathList> _folderPathList = new List<FolderPathList>();


        //Denne delen skal først lese eventuelle filbaner som er lagt inn i xml-filen og lagre de i en liste. 
        //Skal så ha mulighet for å lagre flere filbaner via user input. Lagres i filen når brukeren trykker på lagreknapp.

        public FileFoldersViewModel(string xmlFilePath)
        {
            _xmlFilePath = xmlFilePath;

            SaveFolderPathCommand = new DelegateCommand(o => updateFolderPathList());

            readXMLFile();
        }

        public ICommand SaveFolderPathCommand { get; private set; }


        private void readXMLFile()
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
                            _folderPathList.Add(new FolderPathList { PathName = el.Attribute("PathName").Value, FolderPath = el.Attribute("FolderPath").Value });
                        }
                    }
                }
            }

            _folderPathList = _folderPathList.OrderBy(x => x.PathName).ThenBy(x => x.FolderPath).ToList();
        }

        private void updateFolderPathList()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_xmlFilePath);

            XmlNode node = doc.SelectSingleNode("//Folderpaths");

            if (node != null)
            {
                node.RemoveAll();
            }
            else
            {
                node = doc.SelectSingleNode("//Settings");

                XmlElement folderpathsElement = doc.CreateElement("Folderpaths");

                node.AppendChild(folderpathsElement);

                node = doc.SelectSingleNode("//Folderpaths");
            }

            

            foreach (var path in FolderPathList)
            {
                //Lag nytt element FolderPath
                XmlElement folderpathElement = doc.CreateElement("FolderPath");
                XmlAttribute PathNameAtt = doc.CreateAttribute("PathName");
                XmlAttribute FolderPathAtt = doc.CreateAttribute("FolderPath");

                PathNameAtt.Value = path.PathName;
                FolderPathAtt.Value = path.FolderPath;

                node.AppendChild(folderpathElement);

                folderpathElement.Attributes.Append(PathNameAtt);
                folderpathElement.Attributes.Append(FolderPathAtt);
            }

            string newXML = doc.OuterXml;

            doc.Save(_xmlFilePath);

        }

        private void writeListToXMLFile()
        {
            using (XmlWriter writer = XmlWriter.Create(_xmlFilePath))
            {
                writer.WriteStartElement("Folderpaths");
                foreach (var folderPath in _folderPathList)
                {
                    writer.WriteElementString("FolderPath", folderPath.FolderPath);
                }
                writer.WriteEndElement();
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

        public List<FolderPathList> FolderPathList
        {
            get { return _folderPathList; }
            set
            {
                _folderPathList = value;
                OnPropertyChanged("FolderPathList");
            }
        }

        #endregion

    }
}
