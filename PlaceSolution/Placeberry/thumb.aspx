<%@ Page language="c#" %>
<script runat="server">
	
		public System.Drawing.Size GetImgSize(string filename)
		{
			System.Drawing.Image img;
			try
			{
				img = System.Drawing.Image.FromFile(filename);
			}
			catch
			{
				return new System.Drawing.Size(0,0);
			}

			System.Drawing.Size imgSize = GetImgSize(img);

			img.Dispose();

			return imgSize;
		}


		public System.Drawing.Size GetImgSize(System.Drawing.Image img)
		{
			return new System.Drawing.Size(img.Width, img.Height);
		}

		public System.Drawing.Image GetThumb(System.Drawing.Image img, int maxWidth, int maxHeight, bool crop, string galleryThumbnail, int paddingOnMaxDim)
		{
			int thumbWidth = img.Width;
			int thumbHeight = img.Height;

            //Ukoliko je height 10 puta manji od weighta ili obrnuto
            //if (thumbWidth > (thumbHeight * 10) || thumbHeight > (thumbWidth * 10))
            //    crop = true;
            
            //dodao zbog AM - ako je slika manja onda je povecaj bez cropa
            if (thumbWidth <= (decimal)maxWidth && thumbHeight <= (decimal)maxHeight)
                galleryThumbnail = "w";
            
            if (!String.IsNullOrEmpty(galleryThumbnail))
            {
                switch (galleryThumbnail.ToLower())
                {
                    case "h":
                        thumbHeight = maxHeight - paddingOnMaxDim;
                        thumbWidth = (int)((decimal)(img.Width) * (decimal)thumbHeight / (decimal)img.Height);
                        break;
                    case "w":
                        thumbWidth = maxWidth - paddingOnMaxDim;
                        thumbHeight = (int)((decimal)(img.Height) * (decimal)thumbWidth / (decimal)img.Width);
                        break;
                    case "a":
                        if (img.Width > img.Height)
                        {
                            thumbWidth = maxWidth - paddingOnMaxDim;
                            thumbHeight = (int)((decimal)(img.Height) * (decimal)thumbWidth / (decimal)img.Width);
                        }
                        else
                        {
                            thumbHeight = maxHeight - paddingOnMaxDim;
                            thumbWidth = (int)((decimal)(img.Width) * (decimal)thumbHeight / (decimal)img.Height);
                        }
                        break;
                }
            }
            else
            {
                //ako je slika manja vrati sliku - AM trazio da to maknem tj da poveca sliku
                //if (thumbWidth <= (decimal)maxWidth && thumbHeight <= (decimal)maxHeight)
                   // return img;

                if (thumbWidth > maxWidth)	// kresemo po widthu
                {
                    thumbWidth = maxWidth;
                    thumbHeight = (int)((decimal)(img.Height) * (decimal)thumbWidth / (decimal)img.Width);
                }

                if (!crop)
                {
                    if (thumbHeight > maxHeight) // ako nejde po widthu, onda ide po heightu
                    {
                        thumbHeight = maxHeight;
                        thumbWidth = (int)((decimal)(img.Width) * (decimal)thumbHeight / (decimal)img.Height);
                    }
                }
                else
                {
                    if (thumbHeight < maxHeight)
                    {
                        thumbHeight = maxHeight;
                        thumbWidth = (int)((decimal)(img.Width) * (decimal)thumbHeight / (decimal)img.Height);
                    }
                }
            }
			
			// NORMAL THUMB
			System.Drawing.Bitmap thumb = new System.Drawing.Bitmap(thumbWidth, thumbHeight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(thumb);
            
			g.InterpolationMode=System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			g.DrawImage(img, 0, 0, thumbWidth, thumbHeight);
			
			if (!crop)
				return thumb;

			// CROP
			System.Drawing.Bitmap cropBmp = new System.Drawing.Bitmap(maxWidth, maxHeight);
			System.Drawing.Graphics g2 = System.Drawing.Graphics.FromImage(cropBmp);
            //dodao da bude bijela pozadina kod kropanja
            g2.FillRegion(System.Drawing.Brushes.White, new System.Drawing.Region(new System.Drawing.RectangleF(0, 0, maxWidth, maxHeight)));
            
			g2.DrawImage(thumb, new System.Drawing.Rectangle(0,0,maxWidth,maxHeight), new System.Drawing.Rectangle((thumbWidth-maxWidth)/2, (thumbHeight-maxHeight)/2, maxWidth, maxHeight), System.Drawing.GraphicsUnit.Pixel);

			thumb.Dispose();
			
			return cropBmp;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.Clear();
			Response.ContentType = "image/jpeg";

			// input
			string imgSrc = Request.QueryString["src"];
			
			bool crop = false;
			if (Request.QueryString["crop"] == "1")
				crop = true;

            string galleryThumb = "";
            if (Request.QueryString["gth"] == "1")
                galleryThumb = "h";
            else
            {
                galleryThumb = Request.QueryString["gth"]; //H-postavlja po height, W-po width, A-auto, tj po vecoj smanjujemo tj veca mora stat
            }

            int paddingOnMaxDim = 0; //ako smanjuje sliku po vecoj stranici da stavi napust
            try
            {
                paddingOnMaxDim = int.Parse(Request.QueryString["pd"]);
            }
            catch
            {
                paddingOnMaxDim = 0;
            }
            
			// uzimamo relative path
            /*
            if(!String.IsNullOrEmpty(imgSrc) && imgSrc.Length >= "http://".Length)
                if (imgSrc.ToLower().Substring(0, "http://".Length) == "http://")
                    imgSrc = imgSrc.Substring(imgSrc.IndexOf("/", "http://".Length+1));
             */
            
			int maxWidth = int.MaxValue;
			int maxHeight = int.MaxValue;
			try
			{
				maxWidth = int.Parse(Request.QueryString["mw"]);
			}
			catch
			{
				maxWidth = 600;
			}
			
			try
			{
				maxHeight = int.Parse(Request.QueryString["mh"]);
			}
			catch
			{
				maxHeight = 400;
			}

            if (imgSrc.StartsWith("/"))
            {
                //from file
                if (System.IO.File.Exists(Server.MapPath(imgSrc)))
                 {
                     System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath(imgSrc));

                     //throw new Exception(Server.MapPath(Server.MapPath(imgSrc)));

                     System.Drawing.Image optImg = GetThumb(img, maxWidth, maxHeight, crop, galleryThumb, paddingOnMaxDim);
                     optImg.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                     img.Dispose();
                     optImg.Dispose();
                 }
                 else
                 {
                     System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath("/resources/images/noimg.jpg"));

                     //throw new Exception(Server.MapPath(pathTags.NoImage));

                     System.Drawing.Image optImg = GetThumb(img, maxWidth, maxHeight, crop, galleryThumb, 0);
                     optImg.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                     img.Dispose();
                     optImg.Dispose();
                 }
            }
            else
            {
                //from url
                try
                {
                    System.Net.WebRequest req = System.Net.WebRequest.Create(imgSrc);
                    System.Net.WebResponse response = req.GetResponse();
                    System.IO.Stream stream = response.GetResponseStream();

                    System.Drawing.Image img = System.Drawing.Image.FromStream(stream);

                    System.Drawing.Image optImg = GetThumb(img, maxWidth, maxHeight, crop, galleryThumb, paddingOnMaxDim);
                    optImg.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    img.Dispose();
                    optImg.Dispose();
                }
                catch (Exception)
                {
                    System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath("/resources/images/noimg.jpg"));

                    System.Drawing.Image optImg = GetThumb(img, maxWidth, maxHeight, crop, galleryThumb, 0);
                    optImg.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    img.Dispose();
                    optImg.Dispose();
                }
            }
		}
</script>
