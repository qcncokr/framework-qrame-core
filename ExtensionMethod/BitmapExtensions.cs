using System.Drawing;
using System.Drawing.Drawing2D;

namespace Qrame.CoreFX.ExtensionMethod
{
    /// <summary>
    /// Extension methods for the System.Drawing.Bitmap class
    /// </summary>
    public static class BitmapExtensions
    {

        /// <summary>
        /// Scales the @this to the passed target size without respecting the aspect.
        /// </summary>
        /// <param name="@this">The source @this.</param>
        /// <param name="size">The target size.</param>
        /// <returns>The scaled @this</returns>
        /// <example><code>
        /// var @this = new Bitmap("image.png");
        /// var thumbnail = @this.ScaleToSize(100, 100);
        /// </code></example>
        public static Bitmap ScaleToSize(this Bitmap @this, Size size)
        {
            return @this.ScaleToSize(size.Width, size.Height);
        }

        /// <summary>
        /// Scales the @this to the passed target size without respecting the aspect.
        /// </summary>
        /// <param name="@this">The source @this.</param>
        /// <param name="width">The target width.</param>
        /// <param name="height">The target height.</param>
        /// <returns>The scaled @this</returns>
        /// <example><code>
        /// var @this = new Bitmap("image.png");
        /// var thumbnail = @this.ScaleToSize(100, 100);
        /// </code></example>
        public static Bitmap ScaleToSize(this Bitmap @this, int width, int height)
        {
            var scaledBitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(scaledBitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(@this, 0, 0, width, height);
            }
            return scaledBitmap;
        }

        /// <summary>
        /// Scales the @this to the passed target size by respecting the aspect.
        /// </summary>
        /// <param name="@this">The source @this.</param>
        /// <param name="size">The target size.</param>
        /// <returns>The scaled @this</returns>
        /// <example><code>
        /// var @this = new Bitmap("image.png");
        /// var thumbnail = @this.ScaleProportional(100, 100);
        /// </code></example>
        /// <remarks>Please keep in mind that the returned bitmaps size might not match the desired size due to original bitmaps aspect.</remarks>
        public static Bitmap ScaleProportional(this Bitmap @this, Size size)
        {
            return @this.ScaleProportional(size.Width, size.Height);
        }

        /// <summary>
        /// Scales the @this to the passed target size by respecting the aspect.
        /// </summary>
        /// <param name="@this">The source @this.</param>
        /// <param name="width">The target width.</param>
        /// <param name="height">The target height.</param>
        /// <returns>The scaled @this</returns>
        /// <example><code>
        /// var @this = new Bitmap("image.png");
        /// var thumbnail = @this.ScaleProportional(100, 100);
        /// </code></example>
        /// <remarks>Please keep in mind that the returned bitmaps size might not match the desired size due to original bitmaps aspect.</remarks>
        public static Bitmap ScaleProportional(this Bitmap @this, int width, int height)
        {
            float proportionalWidth, proportionalHeight;

            if (width.Equals(0))
            {
                proportionalWidth = ((float)height) / @this.Size.Height * @this.Width;
                proportionalHeight = height;
            }
            else if (height.Equals(0))
            {
                proportionalWidth = width;
                proportionalHeight = ((float)width) / @this.Size.Width * @this.Height;
            }
            else if (((float)width) / @this.Size.Width * @this.Size.Height <= height)
            {
                proportionalWidth = width;
                proportionalHeight = ((float)width) / @this.Size.Width * @this.Height;
            }
            else
            {
                proportionalWidth = ((float)height) / @this.Size.Height * @this.Width;
                proportionalHeight = height;
            }

            return @this.ScaleToSize((int)proportionalWidth, (int)proportionalHeight);
        }

        /// <summary>
        /// Scales the @this to the passed target size by respecting the aspect. The overlapping background is filled with the given background color.
        /// </summary>
        /// <param name="@this">The source @this.</param>
        /// <param name="size">The target size.</param>
        /// <returns>The scaled @this</returns>
        /// <example><code>
        /// var @this = new Bitmap("image.png");
        /// var thumbnail = @this.ScaleToSizeProportional(100, 100);
        /// </code></example>
        public static Bitmap ScaleToSizeProportional(this Bitmap @this, Size size)
        {
            return @this.ScaleToSizeProportional(Color.White, size);
        }

        /// <summary>
        /// Scales the @this to the passed target size by respecting the aspect. The overlapping background is filled with the given background color.
        /// </summary>
        /// <param name="@this">The source @this.</param>
        /// <param name="backgroundColor">The color of the background.</param>
        /// <param name="size">The target size.</param>
        /// <returns>The scaled @this</returns>
        /// <example><code>
        /// var @this = new Bitmap("image.png");
        /// var thumbnail = @this.ScaleToSizeProportional(100, 100);
        /// </code></example>
        public static Bitmap ScaleToSizeProportional(this Bitmap @this, Color backgroundColor, Size size)
        {
            return @this.ScaleToSizeProportional(backgroundColor, size.Width, size.Height);
        }

        /// <summary>
        /// Scales the @this to the passed target size by respecting the aspect. The overlapping background is filled with the given background color.
        /// </summary>
        /// <param name="@this">The source @this.</param>
        /// <param name="width">The target width.</param>
        /// <param name="height">The target height.</param>
        /// <returns>The scaled @this</returns>
        /// <example><code>
        /// var @this = new Bitmap("image.png");
        /// var thumbnail = @this.ScaleToSizeProportional(100, 100);
        /// </code></example>
        public static Bitmap ScaleToSizeProportional(this Bitmap @this, int width, int height)
        {
            return @this.ScaleToSizeProportional(Color.White, width, height);
        }

        /// <summary>
        /// Scales the @this to the passed target size by respecting the aspect. The overlapping background is filled with the given background color.
        /// </summary>
        /// <param name="@this">The source @this.</param>
        /// <param name="backgroundColor">The color of the background.</param>
        /// <param name="width">The target width.</param>
        /// <param name="height">The target height.</param>
        /// <returns>The scaled @this</returns>
        /// <example><code>
        /// var @this = new Bitmap("image.png");
        /// var thumbnail = @this.ScaleToSizeProportional(100, 100);
        /// </code></example>
        public static Bitmap ScaleToSizeProportional(this Bitmap @this, Color backgroundColor, int width, int height)
        {
            var scaledBitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(scaledBitmap))
            {
                g.Clear(backgroundColor);

                var proportionalBitmap = @this.ScaleProportional(width, height);

                var imagePosition = new Point(
                    (int)((width - proportionalBitmap.Width) / 2m),
                    (int)((height - proportionalBitmap.Height) / 2m)
                    );
                g.DrawImage(proportionalBitmap, imagePosition);
            }

            return scaledBitmap;
        }
    }
}
