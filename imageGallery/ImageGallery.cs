using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using C1.Win.C1Tile;

namespace imageGallery
{
    public partial class ImageGallery : Form
    {
        public SplitContainer C1Split = new SplitContainer();
        public TableLayoutPanel panelTableLayout = new TableLayoutPanel();
        public Panel panelImageGallery1 = new Panel();
        public Panel panelImageGallery2 = new Panel();
        public Panel panelImageGallery3 = new Panel();

        public TextBox txtSearchBox = new TextBox();
        public PictureBox SearchImageIcon = new PictureBox();
        public PictureBox DownloadImageIcon = new PictureBox();

        public PictureBox ExporttoPdfIcon = new PictureBox();
        public C1.Win.C1Tile.C1TileControl tileControl = new C1.Win.C1Tile.C1TileControl();
        public C1.Win.C1Tile.Group group1 = new C1.Win.C1Tile.Group();
        public C1.C1Pdf.C1PdfDocument pdfDocument = new C1.C1Pdf.C1PdfDocument();

        public StatusStrip statusStrip1 = new System.Windows.Forms.StatusStrip();
        public ToolStripProgressBar progressBar1 = new System.Windows.Forms.ToolStripProgressBar();

        //Instance of DataFechter class and a list of ImageItem class.
        DataFetcher datafetch1 = new DataFetcher();
        List<ImageItem> imagesList1;
        int checkedItems = 0;

        private void ImageGallery_Load(object sender, EventArgs e)
        {
            void controls1()
            {
                // UI controls dynamically.

                //Adding the Main Form 
                this.Text = "Image Gallery";
                this.MaximizeBox = false;
                this.Size = new System.Drawing.Size(780, 800);
                this.MaximumSize = new System.Drawing.Size(810, 810);
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                this.ShowIcon = false;
                this.Controls.Add(C1Split);

                // Adding the SplitContainer 

                this.C1Split.Dock = DockStyle.Fill;
                this.C1Split.Panel1.Show();
                this.C1Split.Panel2.Show();
                this.C1Split.SplitterDistance = 40;
                this.C1Split.Orientation = System.Windows.Forms.Orientation.Horizontal;
                this.C1Split.Visible = true;
                this.C1Split.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
                this.C1Split.Margin = new System.Windows.Forms.Padding(2);
                this.C1Split.IsSplitterFixed = true;
                this.C1Split.Panel1.Controls.Add(panelTableLayout);
                this.C1Split.Panel2.Controls.Add(tileControl);
                this.C1Split.Panel2.Controls.Add(this.statusStrip1);

                //Adding Table Layout Control

                this.panelTableLayout.ColumnCount = 3;
                this.panelTableLayout.Dock = DockStyle.Fill;
                this.panelTableLayout.Location = new System.Drawing.Point(0, 0);
                this.panelTableLayout.RowCount = 1;
                this.panelTableLayout.Size = new System.Drawing.Size(800, 40);
                this.panelTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
                this.panelTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
                this.panelTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.5F));

                //Add Pannels 1,2,3 to TableLayout to col 1, 2, 3.
                this.panelTableLayout.Controls.Add(panelImageGallery1, 1, 0);
                this.panelTableLayout.Controls.Add(panelImageGallery2, 2, 0);
                this.panelTableLayout.Controls.Add(panelImageGallery3, 0, 0);

                this.panelImageGallery1.Location = new System.Drawing.Point(477, 0);
                this.panelImageGallery1.Size = new System.Drawing.Size(287, 40);
                this.panelImageGallery1.Dock = DockStyle.Fill;
                this.panelImageGallery1.Paint += new System.Windows.Forms.PaintEventHandler(this.OnSearchPanelPaint);
                this.panelImageGallery1.Controls.Add(txtSearchBox);

                //Adding SEarBox to enter Text to search.
                this.txtSearchBox.Name = "_searchBox";
                this.txtSearchBox.BorderStyle = 0;
                this.txtSearchBox.Dock = DockStyle.Fill;
                this.txtSearchBox.Location = new System.Drawing.Point(16, 9);
                this.txtSearchBox.Size = new System.Drawing.Size(244, 16);
                this.txtSearchBox.Text = "Search Image";
                this.txtSearchBox.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);



                this.panelImageGallery2.Location = new System.Drawing.Point(479, 12);
                this.panelImageGallery2.Margin = new System.Windows.Forms.Padding(2, 12, 45, 12);
                this.panelImageGallery2.Size = new System.Drawing.Size(40, 16);
                this.panelImageGallery2.TabIndex = 1;
                this.panelImageGallery2.Controls.Add(SearchImageIcon);

                //Adding Search Item.
                this.SearchImageIcon.Name = "_search";
                this.SearchImageIcon.Image = global::imageGallery.Properties.Resources.search_icon;
                this.SearchImageIcon.Dock = DockStyle.Left;
                this.SearchImageIcon.Location = new System.Drawing.Point(0, 0);
                this.SearchImageIcon.Margin = new System.Windows.Forms.Padding(0);
                this.SearchImageIcon.Size = new System.Drawing.Size(40, 16);
                this.SearchImageIcon.SizeMode = PictureBoxSizeMode.Zoom;
                this.SearchImageIcon.BorderStyle = BorderStyle.FixedSingle;
                this.SearchImageIcon.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
                this.SearchImageIcon.Click += new System.EventHandler(this.searchimg_Click);


                this.panelImageGallery3.Dock = DockStyle.Fill;
                this.panelImageGallery3.TabIndex = 1;
                this.panelImageGallery3.Controls.Add(ExporttoPdfIcon);
                this.panelImageGallery3.Controls.Add(DownloadImageIcon);
                this.panelImageGallery3.TabIndex = 2;

                //Adding Export Image Button.
                this.ExporttoPdfIcon.Name = "_exportImage";
                this.ExporttoPdfIcon.Image = global::imageGallery.Properties.Resources.export;
                this.ExporttoPdfIcon.Location = new System.Drawing.Point(0, 3);
                this.ExporttoPdfIcon.Size = new System.Drawing.Size(100, 30);
                this.ExporttoPdfIcon.SizeMode = PictureBoxSizeMode.StretchImage;
                this.ExporttoPdfIcon.Click += new System.EventHandler(this.OnExportClick);
                this.ExporttoPdfIcon.Visible = false;
                this.ExporttoPdfIcon.BorderStyle = BorderStyle.FixedSingle;
                this.ExporttoPdfIcon.Paint += new System.Windows.Forms.PaintEventHandler(this.OnExportImagePaint);

                //Adding Download Image.
                this.DownloadImageIcon.Name = "_DownloadImage";
                this.DownloadImageIcon.Image = global::imageGallery.Properties.Resources.download;
                this.DownloadImageIcon.Location = new System.Drawing.Point(105, 3);
                this.DownloadImageIcon.Size = new System.Drawing.Size(100, 30);
                this.DownloadImageIcon.SizeMode = PictureBoxSizeMode.StretchImage;
                this.DownloadImageIcon.Click += new System.EventHandler(this.DownloadImage_Click);
                this.DownloadImageIcon.Visible = false;
                this.DownloadImageIcon.BorderStyle = BorderStyle.FixedSingle;


                this.tileControl.CellHeight = 78;
                this.tileControl.CellSpacing = 11;
                this.tileControl.CellWidth = 78;
                this.tileControl.Name = "tileControl1";
                this.tileControl.Dock = DockStyle.Fill;
                this.tileControl.Size = new System.Drawing.Size(764, 718);
                this.tileControl.SurfacePadding = new System.Windows.Forms.Padding(12, 4, 12, 4);
                this.tileControl.SwipeDistance = 20;
                this.tileControl.SwipeRearrangeDistance = 98;
                this.tileControl.Groups.Add(this.group1);
                this.tileControl.TileChecked += new System.EventHandler<C1.Win.C1Tile.TileEventArgs>(this.OnTileChecked);
                this.tileControl.TileUnchecked += new System.EventHandler<C1.Win.C1Tile.TileEventArgs>(this.OnTileUnchecked);
                this.tileControl.Paint += new System.Windows.Forms.PaintEventHandler(this.OnTileControlPaint);
                this.tileControl.AllowChecking = true;
                this.tileControl.Orientation = LayoutOrientation.Vertical;

                this.statusStrip1.Visible = false;
                this.statusStrip1.Dock = DockStyle.Bottom;
                this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.progressBar1 });
                this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;

                this.group1.Name = "Group1";
               

            }
            controls1();

        }

        public ImageGallery()
        {
            InitializeComponent();
        }
        //To fetch the images using DataFetcher class.
        private async void searchimg_Click(object sender, EventArgs e)
        {

            statusStrip1.Visible = true;
            imagesList1 = await
            datafetch1.GetImageData(txtSearchBox.Text);
            AddTiles(imagesList1);
            statusStrip1.Visible = false;

        }
        //Loop through all the images and add it to the tile control
        private void AddTiles(List<ImageItem> imageList1)
        {
            tileControl.Groups[0].Tiles.Clear();

            foreach (var imageitem in imageList1)
            {
                Tile tile = new Tile();
                tile.HorizontalSize = 2;
                tile.VerticalSize = 2;
                tileControl.Groups[0].Tiles.Add(tile);

                Image img = Image.FromStream(new MemoryStream(imageitem.Base64));

                Template tl = new Template();
                ImageElement ie = new ImageElement();
                ie.ImageLayout = ForeImageLayout.Stretch;
                tl.Elements.Add(ie);
                tile.Template = tl;
                tile.Image = img;
            }
        }


        //Callback for click event for export button.
        private void OnExportClick(object sender, EventArgs e)
        {
            List<Image> images = new List<Image>();
            foreach (Tile tile in tileControl.Groups[0].Tiles)
            {
                if (tile.Checked)
                {
                    images.Add(tile.Image);
                }
            }
            ConvertToPdf(images);
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "pdf";
            saveFile.Filter = "PDF files (*.pdf)|*.pdf*";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {

                pdfDocument.Save(saveFile.FileName);

            }

        }

        //Iterates through all the tiles, gets it’s image and save this list of images to disk with given ImageName and Location.
        private void DownloadImage_Click(object sender, EventArgs e)
        {
            List<Image> images = new List<Image>();
            foreach (Tile tile in tileControl.Groups[0].Tiles)
            {
                if (tile.Checked)
                {
                    images.Add(tile.Image);
                }
            }

            SaveFileDialog s1 = new SaveFileDialog();
            s1.DefaultExt = "jpg";
            s1.Filter = "jpg files (*.jpg)|*.jpg";
            if (s1.ShowDialog() == DialogResult.OK)
            {
                int len;
                string s = s1.FileName;
                len = s.Length;

                //Store Filename to String.
                string fname = "";
                for (int t = 0; t < len - 4; t++)
                {
                    fname += s[t];
                }
                int count = 1;

                //Give unique FileName if there are Multiple Images. 
                foreach (var selectedimg in images)
                {
                    string temp = fname;
                    if (count != 1)
                    {
                        temp += count;
                    }
                    selectedimg.Save(temp + ".jpg");
                    count++;
                }
            }

        }

        //iterates through all the tiles, gets it’s image and convert this list of images to PDF.
        private void ConvertToPdf(List<Image> images)
        {
            RectangleF rect = pdfDocument.PageRectangle;
            bool firstPage = true;
            foreach (var selectedimg in images)
            {
                if (!firstPage)
                {
                    pdfDocument.NewPage();
                }
                firstPage = false;
                rect.Inflate(-72, -72);
                // opens a SaveFileDialog to save the image.
                pdfDocument.DrawImage(selectedimg, rect);
            }

        }

        //To add a grey border to the search box.
        public void OnSearchPanelPaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Rectangle r = txtSearchBox.Bounds;
            r.Inflate(3, 3);
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawRectangle(p, r);
        }

        //used for drawing a grey border for export to pdf button.
        public void OnExportImagePaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Rectangle r = new Rectangle(ExporttoPdfIcon.Location.X, ExporttoPdfIcon.Location.Y, ExporttoPdfIcon.Width,
            ExporttoPdfIcon.Height);
            r.X -= 29;
            r.Y -= 3;
            r.Width--;
            r.Height--;
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawRectangle(p, r);
            e.Graphics.DrawLine(p, new Point(0, 43), new
            Point(this.Width, 43));
        }

        //Change Visiblity of ExporttoPDF and DownloadImage Buttons and increment selected items.
        private void OnTileChecked(object sender, C1.Win.C1Tile.TileEventArgs e)
        {
            checkedItems = checkedItems + 1;
            ExporttoPdfIcon.Visible = true;
            DownloadImageIcon.Visible = true;
        }

        ////Change Visiblity of ExporttoPDF and DownloadImage Buttons and decrement selected items.
        private void OnTileUnchecked(object sender, C1.Win.C1Tile.TileEventArgs e)
        {
            checkedItems = checkedItems - 1;
            ExporttoPdfIcon.Visible = (checkedItems > 0);
            DownloadImageIcon.Visible = (checkedItems > 0);
        }

        //used to draw a separator
        private void OnTileControlPaint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawLine(p, 0, 43, 800, 43);
        }

    }
}
