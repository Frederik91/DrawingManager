using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.PlottingServices;

namespace DrawingManagerDll
{
    class PlottingCommands
    {
        public PlottingCommands()
        {

        }

        [CommandMethod("ListLayouts")]
        public string GetFirstLayout()
        {
            List<string> firstSheet = new List<string>();

            // Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;


            // Get the layout dictionary of the current database
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                DBDictionary lays =
                    acTrans.GetObject(acCurDb.LayoutDictionaryId,
                        OpenMode.ForRead) as DBDictionary;


                // Step through and list each named layout and Model
                foreach (DBDictionaryEntry item in lays)
                {
                    firstSheet.Add(item.Key);
                }

                // Abort the changes to the database
                acTrans.Abort();
            }
            firstSheet.Distinct();
            return firstSheet[0];
        }


        #region Use existing layout

        [CommandMethod("PLOTTOPDF")]
        public void PlotToPDF()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;


            //var filename = doc.Name.Split('.');
            
            var filePathArray = db.Filename.Split('\\');
            var filename = filePathArray[filePathArray.Length - 1].Split('.')[0];


            Transaction tr =
              db.TransactionManager.StartTransaction();
            using (tr)
            {

                // We'll be plotting the current layout

                Application.DocumentManager.MdiActiveDocument = doc;
                LayoutManager.Current.CurrentLayout = GetFirstLayout();

                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForRead);
                Layout lo = (Layout)tr.GetObject(btr.LayoutId, OpenMode.ForRead);

                // We need a PlotInfo object
                // linked to the layout

                PlotInfo pi = new PlotInfo();
                pi.Layout = btr.LayoutId;

                // We need a PlotSettings object
                // based on the layout settings
                // which we then customize

                PlotSettings ps = new PlotSettings(lo.ModelType);
                ps.CopyFrom(lo);

                // The PlotSettingsValidator helps
                // create a valid PlotSettings object

                PlotSettingsValidator psv =
                  PlotSettingsValidator.Current;

                // We'll plot the extents, centered and
                // scaled to fit

                psv.SetPlotType(
                  ps,
                  Autodesk.AutoCAD.DatabaseServices.PlotType.Extents
                );
                psv.SetUseStandardScale(ps, true);
                psv.SetStdScaleType(ps, StdScaleType.ScaleToFit);
                psv.SetPlotCentered(ps, true);
                psv.RefreshLists(ps);
                psv.SetCurrentStyleSheet(lo, "C:\\ProgramData\\Autodesk\\ACA 2015\\enu\\Plotters\\Plot Styles\\COWI_EL-v00.ctb");

                // We'll use the standard DWF PC3, as
                // for today we're just plotting to file

                psv.SetPlotConfigurationName(
                  ps,
                  "DWG to PDF.pc3",
                  "ISO_A0_(841.00_x_1189.00_MM)"
                );

                // We need to link the PlotInfo to the
                // PlotSettings and then validate it

                pi.OverrideSettings = ps;
                PlotInfoValidator piv = new PlotInfoValidator();
                piv.MediaMatchingPolicy = MatchingPolicy.MatchEnabled;
                piv.Validate(pi);

                // A PlotEngine does the actual plotting
                // (can also create one for Preview)

                if (PlotFactory.ProcessPlotState ==
                    ProcessPlotState.NotPlotting)
                {
                    PlotEngine pe = PlotFactory.CreatePublishEngine();
                    using (pe)
                    {
                        // Create a Progress Dialog to provide info
                        // and allow the user to cancel

                        PlotProgressDialog ppd = new PlotProgressDialog(true, 1, true);
                        using (ppd)
                        {
                            ppd.set_PlotMsgString(
                              PlotMessageIndex.DialogTitle,
                              "Custom Plot Progress"
                            );
                            ppd.set_PlotMsgString(
                              PlotMessageIndex.CancelJobButtonMessage,
                              "Cancel Job"
                            );
                            ppd.set_PlotMsgString(
                              PlotMessageIndex.CancelSheetButtonMessage,
                              "Cancel Sheet"
                            );
                            ppd.set_PlotMsgString(
                              PlotMessageIndex.SheetSetProgressCaption,
                              "Sheet Set Progress"
                            );
                            ppd.set_PlotMsgString(
                              PlotMessageIndex.SheetProgressCaption,
                              "Sheet Progress"
                            );
                            ppd.LowerPlotProgressRange = 0;
                            ppd.UpperPlotProgressRange = 100;
                            ppd.PlotProgressPos = 0;

                            // Let's start the plot, at last

                            ppd.OnBeginPlot();
                            ppd.IsVisible = true;
                            pe.BeginPlot(ppd, null);

                            // We'll be plotting a single document

                            pe.BeginDocument(
                              pi,
                              doc.Name,
                              null,
                              1,
                              true, // Let's plot to file
                              "E:\\Plottfefiløving\\PDF\\" + filename + ".pdf"
                            );

                            // Which contains a single sheet

                            ppd.OnBeginSheet();

                            ppd.LowerSheetProgressRange = 0;
                            ppd.UpperSheetProgressRange = 100;
                            ppd.SheetProgressPos = 0;

                            PlotPageInfo ppi = new PlotPageInfo();
                            pe.BeginPage(
                              ppi,
                              pi,
                              true,
                              null
                            );
                            pe.BeginGenerateGraphics(null);
                            pe.EndGenerateGraphics(null);

                            // Finish the sheet
                            pe.EndPage(null);
                            ppd.SheetProgressPos = 100;
                            ppd.OnEndSheet();

                            // Finish the document

                            pe.EndDocument(null);

                            // And finish the plot

                            ppd.PlotProgressPos = 100;
                            ppd.OnEndPlot();
                            pe.EndPlot(null);
                        }
                    }
                }
                else
                {
                    ed.WriteMessage(
                      "\nAnother plot is in progress."
                    );
                }
            }
        }
    }

        #endregion
}
