using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;

namespace DrawingManagerDll
{
    public class MainClass
    {
        #region Add ribbon

        [CommandMethod("DrawingManagerRibbon", CommandFlags.Transparent)]
        public void EnableRibbon()
        {
            RibbonControl ribbon = ComponentManager.Ribbon;
            if (ribbon != null)
            {
                RibbonTab rtab = ribbon.FindTab("TESTME");
                if (rtab != null)
                {
                    ribbon.Tabs.Remove(rtab);
                }
                rtab = new RibbonTab();
                rtab.Title = "Drawing manager";
                rtab.Id = "Drawing manager";
                //Add the Tab
                ribbon.Tabs.Add(rtab);
                addContent(rtab);
            }
        }

        static void addContent(RibbonTab rtab)
        {
            rtab.Panels.Add(AddOnePanel());
        }

        static RibbonPanel AddOnePanel()
        {
            RibbonButton plotThisButton;
            RibbonPanelSource rps = new RibbonPanelSource();
            rps.Title = "Plotting";
            RibbonPanel rp = new RibbonPanel();
            rp.Source = rps;

            //Create a Command Item that the Dialog Launcher can use,
            // for this test it is just a place holder.
            RibbonButton rci = new RibbonButton();
            //rci.Name = "TestCommand";


            //assign the Command Item to the DialgLauncher which auto-enables
            // the little button at the lower right of a Panel
            rps.DialogLauncher = rci;

            plotThisButton = new RibbonButton();
            plotThisButton.Name = "Plot";
            plotThisButton.ShowText = true;
            plotThisButton.Text = "Plot";
            plotThisButton.CommandHandler = new MyRibbonCommandHandler();
            plotThisButton.CommandParameter = "HelloWorld ";
            //Add the Button to the Tab
            rps.Items.Add(plotThisButton);
            return rp;
        }
        #endregion


        [CommandMethod("PlotThisPage")]
        public void plotThisPage()
        {
            PlottingCommands plot = new PlottingCommands();
            plot.PlotToPDF();
        }


        public class MyRibbonCommandHandler : System.Windows.Input.ICommand
        {
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                RibbonCommandItem btn = new RibbonCommandItem();
                if (btn != null)
                {
                    //execute an AutoCAD command, or your custom commandd defined by [CommandMethod]
                    Document dwg = Application.DocumentManager.MdiActiveDocument;
                    dwg.SendStringToExecute((string)btn.CommandParameter + "PlotThisPage" + " ", true, false, true);
                }
            }
        }

    }
}
