using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using System;
using System.Windows.Forms;
using static ILNumerics.ILMath;
using static ILNumerics.Globals;

namespace InteractiveColorbar {

    /// <summary>
    /// Example modifying the standard ILColorbar screen object for extended interaction. The data range for the colorbar is 
    /// editable with the mouse. The color data range of the attached surface object is changed with the color bar range and 
    /// presented on the fly. 
    /// <para>This example demonstrates the following aspects: scene setup with surface and colorbar, mouse event registration 
    /// on the colorbar, implementing a custom IILColormapProvider, event handling for dynamically updating the colorbar 
    /// provider data, updating surface objects for dynamic colormapped data ranges.</para>
    /// </summary>
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        // Attributes: 
        // initial mouse drag position
        int? m_mouseStartY;
        // factor increasing the step size for changes of upper /lower limits. Higher values: higher steps / pixel mouse movement
        float m_dragFactor = 10;

        readonly Array<float> A = localMember<float>(); 

        private void ilPanel1_Load(object sender, EventArgs e) {
            var plotCube = new PlotCube(twoDMode: false);

            // Definujte minimální a maximální hodnoty pro osy X a Y
            float minX = -100f;
            float maxX = 100f;
            float minY = -100f;
            float maxY = 100f;

            // Nastavte rozsahy os X a Y
            plotCube.Axes[0].Min = minX;
            plotCube.Axes[0].Max = maxX;
            plotCube.Axes[1].Min = minY;
            plotCube.Axes[1].Max = maxY;

       

            // Přidejte povrch s funkcí Sphere
            var surface = new Surface((x, y) => 10 + ((float)Math.Pow(x, 2) - 10 * (float)Math.Cos(2 * Math.PI * x) + (float)Math.Pow(y, 2) - 10 * (float)Math.Cos(2 * Math.PI * y)));
            plotCube.Add(surface);

            // Přidejte plotCube do scény
            ilPanel1.Scene.Add(plotCube);

            // Znovu vykreslete scénu
            ilPanel1.Refresh();
        }

        /// <summary>
        /// Updates the cursor icon while hovering over the color bar area.
        /// </summary>
        private void UpdateCursor(ILNumerics.Drawing.MouseEventArgs e, Colorbar cb)
        {
            if (m_mouseStartY.HasValue) return;

            float h = (e.LocationF.Y - cb.Location.Y) / cb.Height.GetValueOrDefault();
            // split the colorbar into 3 parts: 
            if (h < .3)
            {
                // upper part: change the upper limit only
                Cursor = Cursors.PanNorth;
            }
            else if (h < .7)
            {
                // mid part: change upper AND lower limits
                Cursor = Cursors.SizeNS;
            }
            else {
                // lower part: change lower limit only
                Cursor = Cursors.PanSouth;
            }
            // note that we "store" the state of the limit to change in the Cursor here. A production
            // application would implement a more sophisticated solution, mostly.
        }

        #region mouse event handlers

        private void colorbar_mouseLeave(object sender, ILNumerics.Drawing.MouseEventArgs e) {
            if (sender is Colorbar) {
                Cursor = Cursors.Default;
            }
        }
        private void colorbar_mouseDown(object sender, ILNumerics.Drawing.MouseEventArgs e) {
            if (sender is Colorbar) {
                m_mouseStartY = e.Location.Y;
            }
        }
        private void colorbar_mouseUp(object sender, ILNumerics.Drawing.MouseEventArgs e) {
            if (sender is Colorbar) {
                m_mouseStartY = null;
            }
        }
        // the move event does most of the work
        private void colorbar_mouseMove(object sender, ILNumerics.Drawing.MouseEventArgs e) {
            if (sender is Colorbar && e.DirectionUp) {

                var cb = sender as Colorbar;
                
                float cbCenter = (ilPanel1.Width * cb.Location.X) - (cb.Width.GetValueOrDefault() * 0.5f);

                if (e.Location.X > cbCenter)
                {
                    ilPanel1.Scene.First<Colorbar>().Movable = false;
                    // update cursor position 
                    UpdateCursor(e, cb);
                    // are we in a dragging operation? 
                    if (m_mouseStartY.HasValue)
                    {
                        // compute offset data 
                        var diff = -(m_mouseStartY.GetValueOrDefault() - e.Location.Y) * m_dragFactor;
                        m_mouseStartY = e.Location.Y;

                        updatePlot(
                                (Cursor == Cursors.PanSouth || Cursor == Cursors.SizeNS) ? diff : 0,
                                (Cursor == Cursors.PanNorth || Cursor == Cursors.SizeNS) ? diff : 0);

                        // do not process further events
                        e.Cancel = true;
                        // trigger a redraw of the scene
                        e.Refresh = true;
                    }
                }
                else
                {
                    Cursor = Cursors.SizeAll;
                    ilPanel1.Scene.First<Colorbar>().Movable = true;
                }               
            }
        }
        #endregion

        /// <summary>
        /// Update the plot and colorbar with new range data
        /// </summary>
        /// <param name="minOffs">range offset, to be added to the existing lower range limit</param>
        /// <param name="maxOffs">range offset, to be added to the existing upper range limit</param>
        void updatePlot(float minOffs, float maxOffs) {
            // update the colorbar 
            var surface = ilPanel1.SceneSyncRoot.First<FastSurface>();
            if (surface != null) {

                float min = surface.GetRangeMinValue(AxisNames.CAxis) + minOffs;
                float max = surface.GetRangeMaxValue(AxisNames.CAxis) + maxOffs;

                // update the surface plot 
                surface.Update(minmaxColorRange: Tuple.Create(min, max));

            }
        }

        private void comboBoxFunctions_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Přetypujte 'sender' na ComboBox
            ComboBox comboBox = sender as ComboBox;


            var plotCube = new PlotCube(twoDMode: false);

           

            // Definujte minimální a maximální hodnoty pro osy X a Y
            float minX = -100f;
            float maxX = 100f;
            float minY = -100f;
            float maxY = 100f;

            // Nastavte rozsahy os X a Y
            plotCube.Axes[0].Min = minX;
            plotCube.Axes[0].Max = maxX;
            plotCube.Axes[1].Min = minY;
            plotCube.Axes[1].Max = maxY;

            // Přidejte povrch s funkcí Sphere
          

            // Zkontrolujte, zda bylo přetypování úspěšné
            if (comboBox != null)
            {
                // Získání vybrané hodnoty z ComboBoxu
                string selectedValue = comboBox.SelectedItem.ToString();
                if (selectedValue == "F1 / Rastrigin Function")
                {
                 var surface = new Surface((x, y) => 10 * 2 + ((float)Math.Pow(x, 2) - 10 * (float)Math.Cos(2 * Math.PI * x) + (float)Math.Pow(y, 2) - 10 * (float)Math.Cos(2 * Math.PI * y)));
                 plotCube.Add(surface);
                }
                if (selectedValue == "F2 / Rosenbrock Function")
                {
                    var surface = new Surface((x, y) => (float)Math.Pow((1 - x),2) + 100 * (float)Math.Pow((y- (float)Math.Pow(x, 2)),2) );
                    plotCube.Add(surface);
                }
                if (selectedValue == "F3 / Sphere Function")
                {
                    var surface = new Surface((x, y) => x*x + y*y);
                    plotCube.Add(surface);
                }
                if (selectedValue == "F4 / Schwefel Function")
                {
                    var surface = new Surface((x, y) => (float)418.9829 * 2 -  (x * (float)Math.Sin((float)Math.Sqrt(Math.Abs(x))) + y * (float)Math.Sin((float)Math.Sqrt(Math.Abs(y)))) );
                    plotCube.Add(surface);
                }
                if (selectedValue == "F5 / Michalewicz Function")
                {
                    var surface = new Surface((x, y) => ((float)Math.Sin(x) * (float)Math.Pow((float)Math.Sin(((x*x) / Math.PI)),2*10)) - ((float)Math.Sin(y) * (float)Math.Pow((float)Math.Sin((2*(y * y) / Math.PI)), 2*10)));
                    plotCube.Add(surface);
                }
                if (selectedValue == "F6 / Styblinski – Tang Function")
                {
                    var surface = new Surface((x, y) => (((float)Math.Pow(x,4) - 16 * (float)Math.Pow(x, 2) + 5 * x) + ((float)Math.Pow(y, 4) - 16 * (float)Math.Pow(y, 2) + 5 * y) ) / 2 );
                    plotCube.Add(surface);
                }

                if (selectedValue == "F7 / Alpine01 Function")
                {
                    var surface = new Surface((x, y) => Math.Abs(x * (float)Math.Sin(x) + (float)0.1 * x) + Math.Abs(y * (float)Math.Sin(y) + (float)0.1 * y));
                    plotCube.Add(surface);
                }

                if (selectedValue == "F8 / Sum of different powers function")
                {
                    var surface = new Surface((x, y) => (float)Math.Pow(Math.Abs(x),2) + (float)Math.Pow(Math.Abs(y), 3));
                    plotCube.Add(surface);
                }

                if (selectedValue == "F9 / Dixon - price function")
                {
                    var surface = new Surface((x, y) => (float)Math.Pow((x -1),2) + 2 * (float)Math.Pow(2*(y*y) - x,2));
                    plotCube.Add(surface);
                }

                if (selectedValue == "F10 / CosineMixture Function")
                {
                    var surface = new Surface((x, y) => ((float)-0.1  * ((float)Math.Cos(5*Math.PI * x) + (float)Math.Cos(5 * Math.PI * y))) - (x*x + y*y) );
                    plotCube.Add(surface);
                }

                if (selectedValue == "F11 / Mishra07 Function")
                {
                    var surface = new Surface((x, y) => (float)Math.Pow( Math.Abs( (x+y)-2),2) );
                    plotCube.Add(surface);
                }

                if (selectedValue == "F12 / Mishra11 Function")
                {
                    var surface = new Surface((x, y) => (float)Math.Pow(Math.Abs( (0.5 * ( Math.Abs(x) + (float)Math.Abs(y)) ) - Math.Pow((Math.Abs(x) + (float)Math.Abs(y)), 0.5) ), 2));
                    plotCube.Add(surface);
                }

                if (selectedValue == "F13 / Plateau Function")
                {
                    var surface = new Surface((x, y) =>  30 + (float)Math.Abs(x) + (float)Math.Abs(y));
                    plotCube.Add(surface);
                }

                if (selectedValue == "F14 / Qing Function")
                {
                    var surface = new Surface((x, y) => (float)Math.Pow(Math.Pow(x,2) - 1, 2) + (float)Math.Pow(Math.Pow(y, 2) - 2, 2));
                    plotCube.Add(surface);
                }

                if (selectedValue == "F15 / Rana Function")
                {
                    var surface = new Surface((x, y) => Math.Abs((x * (float)Math.Sin(Math.Sqrt(Math.Abs(x-x+1))) * (float)Math.Cos(Math.Sqrt(Math.Abs(x + x + 1))) + (x + 1) * (float)Math.Sin(Math.Sqrt(Math.Abs(x + x + 1))) * (float)Math.Cos(Math.Sqrt(Math.Abs(x - x + 1))))) 
                    + Math.Abs((y * (float)Math.Sin(Math.Sqrt(Math.Abs(x - y + 1))) * (float)Math.Cos(Math.Sqrt(Math.Abs(x + y + 1))) + (x + 1) * (float)Math.Sin(Math.Sqrt(Math.Abs(x + y + 1))) * (float)Math.Cos(Math.Sqrt(Math.Abs(x - y + 1)))))
                    );
                    plotCube.Add(surface);
                }

                if (selectedValue == "F16 / Trid Function")
                {
                    var surface = new Surface((x, y) => ((float)Math.Pow(x-1,2) + (float)Math.Pow(y - 1, 2)) - (y*x-1));
                    plotCube.Add(surface);
                }

                if (selectedValue == "F17 / YaoLiu09 Function")
                {
                    var surface = new Surface((x, y) => (float)Math.Abs(Math.Pow(x,2) - 10 * (float)Math.Cos(2 * Math.PI*x) + 10) + (float)Math.Abs(Math.Pow(y, 2) - 10 * (float)Math.Cos(2 * Math.PI * y) + 10));
                    plotCube.Add(surface);
                }

                if (selectedValue == "F18 / Bent Cigar Function")
                {
                    var surface = new Surface((x, y) => x*x + (float)Math.Pow(10,6) * (y*y)  );
                    plotCube.Add(surface);
                }

                if (selectedValue == "F19 / Deb's Function No.01")
                {
                    var surface = new Surface((x, y) => (float)0.5 *( (float)Math.Pow(Math.Sin(5 * Math.PI * x),6)+ (float)Math.Pow(Math.Sin(5 * Math.PI * y), 6)));
                    plotCube.Add(surface);
                }

                if (selectedValue == "F20 / Quartic Function")
                {
                    var surface = new Surface((x, y) => (float)Math.Pow(x,4) + 2 * (float)Math.Pow(y,4));
                    plotCube.Add(surface);
                }

                if (selectedValue == "F21 / Hyper-Ellipsoid Function")
                {
                    var surface = new Surface((x, y) => (float)Math.Pow(x,2) + 4 * (float)Math.Pow(y,2) );
                    plotCube.Add(surface);
                }


                if (selectedValue == "F22 / Egg-Holder Function")
                {
                    var surface = new Surface((x, y) =>  (-x * (float)Math.Sin(Math.Sqrt(Math.Abs(x - y - 47)))) - ((y + 47) * (float)Math.Sin(Math.Sqrt(Math.Abs(x / 2 + y + 47))))  );
                    plotCube.Add(surface);
                }

                if (selectedValue == "F23 / Chung-Reynolds' Function")
                {
                    var surface = new Surface((x, y) => (float)Math.Pow((x*x + y*y),2) );
                    plotCube.Add(surface);
                }

                if (selectedValue == "F24 / Moved-Axis Parallel Hyper-Ellipsoid Function")
                {
                    var surface = new Surface((x, y) => (float)Math.Pow( 1 * (x-5*1), 2) + (float)Math.Pow(2 * (y - 5 * 2), 2) );
                    plotCube.Add(surface);
                }

                if (selectedValue == "F25 / Generalized Schwefel's Function No.2.26")
                {
                    var surface = new Surface((x, y) => -1 * ((float)Math.Abs(x * Math.Sin(Math.Sqrt(Math.Abs(x)))) + (float)Math.Abs(y * Math.Sin(Math.Sqrt(Math.Abs(y))))));
                    plotCube.Add(surface);
                }
                //
            }





            // Přidejte plotCube do scény
            ilPanel1.Scene.First<Scene>().Children.Clear();
            ilPanel1.Scene.Add(plotCube);
            
            // Znovu vykreslete scénu
            ilPanel1.Refresh();
        }
    }
}
