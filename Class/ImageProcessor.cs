using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace web_api_managemen_user.Class
{
    public static class ImageProcessor
    {
        public static Bitmap SetOpacity(Bitmap input_bm, float opacity)
        {
            // Make the new bitmap.
            Bitmap output_bm = new Bitmap(
                input_bm.Width, input_bm.Height);

            // Make an associated Graphics object.
            using (Graphics gr = Graphics.FromImage(output_bm))
            {
                // Make a ColorMatrix with the opacity.
                ColorMatrix color_matrix = new ColorMatrix();
                color_matrix.Matrix33 = opacity;

                // Make the ImageAttributes object.
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(color_matrix,
                    ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                // Draw the input bitmap onto the Graphics object.
                Rectangle rect = new Rectangle(0, 0,
                    output_bm.Width, output_bm.Height);

                gr.DrawImage(input_bm, rect,
                    0, 0, input_bm.Width, input_bm.Height,
                    GraphicsUnit.Pixel, attributes);
            }
            return output_bm;
        }

        public static Image ImageOpacity(Bitmap watermark, float Opacity)
        {
            // Create a sample image.
            // Make a background image.
            Bitmap sample_bm = MakeCheckerboard(
                    watermark.Width,
                    watermark.Height, 64);

            // Draw the adjusted image over the checkerboard.
            //AdjustedImage = SetOpacity(watermark, Opacity);
            //using (Graphics gr = Graphics.FromImage(sample_bm))
            //{
            //    gr.DrawImage(AdjustedImage, 0, 0);
            //}

            return sample_bm;
        }

        private static Bitmap MakeCheckerboard(int width, int height, int size)
        {
            int num_rows = height / size + 1;
            int num_cols = width / size + 1;
            Bitmap bm = new Bitmap(width, height);
            return bm;
        }

    }
}
