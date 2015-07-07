using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DrawingManagerApp.ViewModels
{
    class FileFoldersViewModel
    {
        //Denne delen skal først lese eventuelle filbaner som er lagt inn i xml-filen og lagre de i en liste. 
        //Skal så ha mulighet for å lagre flere filbaner via user input. Lagres i filen når brukeren trykker på lagreknapp.

        private readonly MainViewModel _mainViewModel;

        List<string> folderPathList = new List<string>();

        public FileFoldersViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            for (int i = 0; i < 10; i++)
            {
                folderPathList.Add("testbane" + i);
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
            using (XmlWriter writer = XmlWriter.Create(_mainViewModel.XMLFilePath))
            {
                foreach (var folderPath in folderPathList)
                {
                    writer.WriteElementString("testelement", folderPath);
                }
            }
        }
    }
}
