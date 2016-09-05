using System;
using System.Diagnostics;
using System.Windows.Forms;
using ThinkGeo.MapSuite.Core;

namespace DrawCustomException
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();

            // This is a warning to let people know the sample will not run properly if the debugger is attached.
            if (Debugger.IsAttached)
            {
                MessageBox.Show("For this sample to work properly you need to run it without the debugger attached.  You can use Ctrl-F5 or in the debug menu choose 'Start Without Debugging.'");
            }
        }

        private void TestForm_Load(object sender, EventArgs e)
        {           
            // Set the full extent and the background color
            winformsMap1.CurrentExtent = new RectangleShape(-124.89,49.36,-66.79,20.01);
            winformsMap1.BackgroundOverlay.BackgroundBrush = new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean);

            CustomWorldMapKitWmsDesktopOverlay worldMapKitDesktopOverlay = new CustomWorldMapKitWmsDesktopOverlay();
            winformsMap1.Overlays.Add("WorldOverlay",worldMapKitDesktopOverlay);

            // Draw the map image on the screen
            winformsMap1.Refresh();
        }

        // If the user changes the status we change the online property in the overlay.  Note this is just
        // to simulate going on or offline.  If this were not a sample this code would not be necessary
        private void rbnOnline_CheckedChanged(object sender, EventArgs e)
        {
            ((CustomWorldMapKitWmsDesktopOverlay)winformsMap1.Overlays["WorldOverlay"]).Online = rbnOnline.Checked;
            
            winformsMap1.Refresh();
        }

        // If the user changes the status we change the drawing exception mode of the overlay.  Note this is just
        // to show different options.  If this were not a sample this code would not be necessary.  You would just
        // set the one mode you want and leave it.
        private void rbnException_CheckedChanged(object sender, EventArgs e)
        {
            // In this case we want to throw the exception
            if (rbnThrowException.Checked)
            {
                ((CustomWorldMapKitWmsDesktopOverlay)winformsMap1.Overlays["WorldOverlay"]).DrawingExceptionMode = DrawingExceptionMode.ThrowException;
            }
            
            // In this case we want to draw the excpetion using the deault drawing we provide in the overlay
            if (rbnDrawException.Checked)
            {
                ((CustomWorldMapKitWmsDesktopOverlay)winformsMap1.Overlays["WorldOverlay"]).DrawingExceptionMode = DrawingExceptionMode.DrawException;
                ((CustomWorldMapKitWmsDesktopOverlay)winformsMap1.Overlays["WorldOverlay"]).DrawCustomException = false;
            }

            // In this case we want to do our own custom exception drawing
            if (rbnDrawCustomException.Checked)
            {
                ((CustomWorldMapKitWmsDesktopOverlay)winformsMap1.Overlays["WorldOverlay"]).DrawingExceptionMode = DrawingExceptionMode.DrawException;
                ((CustomWorldMapKitWmsDesktopOverlay)winformsMap1.Overlays["WorldOverlay"]).DrawCustomException = true;
            }

            winformsMap1.Refresh();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void winformsMap1_MouseMove(object sender, MouseEventArgs e)
        {
            //Displays the X and Y in screen coordinates.
            statusStrip1.Items["toolStripStatusLabelScreen"].Text = "X:" + e.X + " Y:" + e.Y;

            //Gets the PointShape in world coordinates from screen coordinates.
            PointShape pointShape = ExtentHelper.ToWorldCoordinate(winformsMap1.CurrentExtent, new ScreenPointF(e.X, e.Y), winformsMap1.Width, winformsMap1.Height);

            //Displays world coordinates.
            statusStrip1.Items["toolStripStatusLabelWorld"].Text = "(world) X:" + Math.Round(pointShape.X, 4) + " Y:" + Math.Round(pointShape.Y, 4);
        }
    }
}
