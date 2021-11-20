
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Stream_Info_Handler
{
    public static class ComboBoxTools
    {
        // Margins around owner drawn ComboBoxes.
        private const int MarginWidth = 4;
        private const int MarginHeight = 4;

        #region Display Images

        // Set up the ComboBox to display images.
        public static void DisplayImages(this ComboBox cbo, Image[] images, int sizing)
        {
            // Make the ComboBox owner-drawn.
            cbo.DrawMode = DrawMode.OwnerDrawVariable;

            // Add the images to the ComboBox's items.
            cbo.Items.Clear();
            foreach (Image image in images)
            {
                Image update_image = ResizeImage(image, sizing, sizing);
                cbo.Items.Add(update_image);
            }

            // Subscribe to the DrawItem event.
            cbo.MeasureItem += cboDrawImage_MeasureItem;
            cbo.DrawItem += cboDrawImage_DrawItem;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        // Measure a ComboBox item that is displaying an image.
        private static void cboDrawImage_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0) return;

            // Get this item's image.
            ComboBox cbo = sender as ComboBox;
            Image img = (Image)cbo.Items[e.Index];
            e.ItemHeight = img.Height + 2 * MarginHeight;
            e.ItemWidth = img.Width + 2 * MarginWidth;
        }

        // Draw a ComboBox item that is displaying an image.
        private static void cboDrawImage_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            // Clear the background appropriately.
            e.DrawBackground();

            // Draw the image.
            ComboBox cbo = sender as ComboBox;
            Image img = (Image)cbo.Items[e.Index];
            float hgt = e.Bounds.Height - 2 * MarginHeight;
            float scale = hgt / img.Height;
            float wid = img.Width * scale;
            RectangleF rect = new RectangleF(
                e.Bounds.X + MarginWidth,
                e.Bounds.Y + MarginHeight,
                img.Width, img.Height);
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.DrawImage(img, rect);

            // Draw the focus rectangle if appropriate.
            e.DrawFocusRectangle();
        }

        #endregion Display Images
    }
}
