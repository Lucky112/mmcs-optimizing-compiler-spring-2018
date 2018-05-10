using System.Drawing;
using System.Windows.Forms;

namespace Compiler.IDE
{
    static class Utils
    {
        static double SCALE_MIN = 0.1;
        static double SCALE_MAX = 1;

        public static double TrackBarToScale(TrackBar bar)
        {
            return MapToRange(bar.Value, bar.Minimum, bar.Maximum, SCALE_MIN, SCALE_MAX);
        }

        public static double MapToRange(double x, double in_min, double in_max, double out_min, double out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        public static Image ScaleImage(Image original, double scaleValue)
        {
            Size size = new Size((int) (original.Width * scaleValue), (int) (original.Height * scaleValue));
            return new Bitmap(original, size);
        }
    }
}