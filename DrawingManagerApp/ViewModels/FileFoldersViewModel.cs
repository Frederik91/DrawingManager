using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DrawingManagerApp.ViewModels
{
    class FileFoldersViewModel : ViewModelBase
    {
        private string _xmlFilePath;

        //Denne delen skal først lese eventuelle filbaner som er lagt inn i xml-filen og lagre de i en liste. 
        //Skal så ha mulighet for å lagre flere filbaner via user input. Lagres i filen når brukeren trykker på lagreknapp.

        List<string> folderPathList = new List<string>();

        public FileFoldersViewModel(string xmlFilePath)
        {
            _xmlFilePath = xmlFilePath;

            for (int i = 0; i < 10; i++)
            {
                folderPathList.Add("testbane" + i);
            }
            //writeXMLFile();
        }

        public string XMLFilePath
        {
            get { return _xmlFilePath; }
            set
            {
                _xmlFilePath = value;
                OnPropertyChanged("XMLFilePath");
            }
        }

        private void readXMLFile()
        {
            //using (XmlReader reader = XmlReader.Create(_mainViewModel.XMLFilePath))
            //{

            //}
        }

        private void writeXMLFile()
        {
            using (XmlWriter writer = XmlWriter.Create(Directory.GetCurrentDirectory() + "\\Datafile.xml"))
            {
                foreach (var folderPath in folderPathList)
                {
                    writer.WriteElementString("testelement", folderPath);
                }
            }
        }

    }
}
