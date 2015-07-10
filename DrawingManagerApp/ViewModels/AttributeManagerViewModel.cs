using DrawingManagerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DrawingManagerApp.ViewModels
{
    class AttributeManagerViewModel : ViewModelBase
    {
        private string _xmlFilePath;

        public AttributeManagerViewModel(string filePath)
        {
            _xmlFilePath = filePath;

            checkXMLContaintsAttributes();

        }

        private void checkXMLContaintsAttributes()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_xmlFilePath);

            XmlNode node = doc.SelectSingleNode("//Attributes");

            if (node == null)
            {
                node = doc.SelectSingleNode("//Settings");

                XmlElement folderpathsElement = doc.CreateElement("Attributes");

                node.AppendChild(folderpathsElement);

                node = doc.SelectSingleNode("//Attributes");

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
    }
}
