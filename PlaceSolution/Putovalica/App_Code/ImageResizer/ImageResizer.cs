using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace PAB
{
    /// <summary>
    /// A class to resize uploaded images
    /// </summary>
    public class ImageResizer 
    {
        private int _imgQuality;
        private int _maxHeight;
        private int _maxWidth;
        private ImageFormat _outputFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageResizer"/> class.
        /// </summary>
        public ImageResizer()
        {
            this._maxWidth = 800;
            this._maxHeight = 800;
            this._imgQuality = 80;
            this._outputFormat = ImageFormat.Jpeg;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageResizer"/> class.
        /// </summary>
        /// <param name="maxWidth">Maximum Width .</param>
        /// <param name="maxHeight">Maximum Height</param>
        /// <param name="imgQuality">The image quality.</param>
        public ImageResizer(int maxWidth, int maxHeight, int imgQuality)
        {
            this._maxWidth = 800;
            this._maxHeight = 800;
            this._imgQuality = 80;
            this._outputFormat = ImageFormat.Jpeg;
            this._maxHeight = maxHeight;
            this._maxWidth = maxWidth;
            this._imgQuality = imgQuality;
        }

        /// <summary>
        /// Resizes the specified source image.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <returns></returns>
        internal System.Drawing.Image DoResize(System.Drawing.Image sourceImage)
        {
            System.Drawing.Image source = new Bitmap(sourceImage);
            int width = sourceImage.Width;
            int height = sourceImage.Height;
            if (width > this.MaxWidth)
            {
                height = (height * this.MaxWidth) / width;
                width = this.MaxWidth;
            }
            if (height > this.MaxHeight)
            {
                width = (width * this.MaxHeight) / height;
                height = this.MaxHeight;
            }
            if ((width != sourceImage.Width) || (height != sourceImage.Height))
            {
                source = new Bitmap(source, width, height);
            }
            return source;
        }

        /// <summary>
        /// Resizes by specified image path.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        public void Resize(string imagePath)
        {
            this.Resize(imagePath, imagePath);
        }

        /// <summary>
        /// Resizes the specified posted file.
        /// </summary>
        /// <param name="postedFile">The posted file.</param>
        /// <returns></returns>
        public byte[] Resize(HttpPostedFile postedFile)
        {
            if (postedFile.ContentLength == 0)
            {
                return new byte[0];
            }
            System.Drawing.Image sourceImage = System.Drawing.Image.FromStream(postedFile.InputStream);
            return Resize(sourceImage);
        }
        public byte[] Resize(System.Drawing.Image sourceImage)
        {
            System.Drawing.Image image2 = this.DoResize(sourceImage);
            sourceImage.Dispose();
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)this.ImgQuality);
            ImageCodecInfo encoder = ImageCodecInfo.GetImageEncoders()[(int)this.OutputFormat];
            MemoryStream stream = new MemoryStream();
            image2.Save(stream, encoder, encoderParams);
            byte[] buffer = stream.GetBuffer();
            image2.Dispose();
            stream.Close();
            return buffer;
        }
        
        public Image Resize2(HttpPostedFile postedFile)
        {
            if (postedFile.ContentLength == 0)
            {
                return null;
            }
            System.Drawing.Image sourceImage = System.Drawing.Image.FromStream(postedFile.InputStream);
            System.Drawing.Image image2 = this.DoResize(sourceImage);
            return image2;
        }

        /// <summary>
        /// Resizes the specified original image path.
        /// </summary>
        /// <param name="originalImagePath">The original image path.</param>
        /// <param name="resizedImagePath">The resized image path.</param>
        public void Resize(string originalImagePath, string resizedImagePath)
        {
            System.Drawing.Image image;
            try
            {
                image = System.Drawing.Image.FromFile(originalImagePath);
            }
            catch
            {
                if (!File.Exists(originalImagePath))
                {
                    throw new Exception("File " + originalImagePath + " doesn't exist; resize failed.");
                }
                throw new Exception("File " + originalImagePath + " is not a valid image file or No read permission on the file; resize failed.");
            }
            System.Drawing.Image image2 = this.DoResize(image);
            image.Dispose();
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, (long) this.ImgQuality);
            ImageCodecInfo encoder = ImageCodecInfo.GetImageEncoders()[(int) this.OutputFormat];
            try
            {
                image2.Save(resizedImagePath, encoder, encoderParams);
            }
            catch (Exception exception)
            {
                string userName;
                try
                {
                    userName = Environment.UserName;
                }
                catch
                {
                    userName = null;
                }
                if (String.IsNullOrEmpty( userName))
                {
                    userName = "'ASPNET' or 'Network Service'";
                }
                userName = userName + " windows account";
                throw new Exception("Could not save resized image to " + resizedImagePath + "; resize failed.\r\n" + exception.Message + "\nTry the following:\r\n1. Ensure that " + resizedImagePath + " is a valid file path.\r\n2. Ensure that the file " + resizedImagePath + " is not already being used by another process.\r\n3. Ensure that " + userName + " has write/modify permission on " + resizedImagePath + " file.\r\n");
            }
            finally
            {
                image2.Dispose();
            }
        }

        /// <summary>
        /// Resizes the specified posted file.
        /// </summary>
        /// <param name="postedFile">The posted file.</param>
        /// <param name="resizedImagePath">The resized image path.</param>
        public void Resize(HttpPostedFile postedFile, string resizedImagePath)
        {
            postedFile.SaveAs(resizedImagePath);
            this.Resize(resizedImagePath);
        }
        
        public int ImgQuality
        {
            get
            {
                return this._imgQuality;
            }
            set
            {
                if ((value < 2) || (value > 100))
                {
                    this._imgQuality = 80;
                }
                else
                {
                    this._imgQuality = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the max height.
        /// </summary>
        /// <value>The max height.</value>
        public int MaxHeight
        {
            get
            {
                return this._maxHeight;
            }
            set
            {
                this._maxHeight = value;
            }
        }


        /// <summary>
        /// Gets or sets the max width.
        /// </summary>
        /// <value>The max width.</value>
        public int MaxWidth
        {
            get
            {
                return this._maxWidth;
            }
            set
            {
                this._maxWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets the output format.
        /// </summary>
        /// <value>The output format.</value>
        public ImageFormat OutputFormat
        {
            get
            {
                return this._outputFormat;
            }
            set
            {
                this._outputFormat = value;
            }
        }
    }
}
