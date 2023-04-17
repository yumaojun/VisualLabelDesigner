using ICSharpCode.SharpDevelop.Workbench;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using YProgramStudio.ZPLTextEditor.Properties;

namespace YProgramStudio.ZPLTextEditor.Gui
{
	public partial class CommandPreview : UserControl, IPadContent
	{
		private static CommandPreview _instance;
		public static CommandPreview Instance
		{
			get { return _instance; }
		}

		private List<Image> _zplImageList = new List<Image>();

		private int? _index;

		private string _zoom;

		private int _rotate;


		public CommandPreview()
		{
			_instance = this;
			InitializeComponent();
		}

		public object Control => this;

		public object InitiallyFocusedControl => null;

		object IServiceProvider.GetService(Type serviceType)
		{
			return null;
		}

		private Image CurrentImage
		{
			get
			{
				return this.pbPreview.Image;
			}
			set
			{
				this.pbPreview.Image = value;
				this.SetZoom();
				this.SetRotation();
			}
		}

		private void SetRotation()
		{
			if (this.pbPreview.Image != null)
			{
				this.lblOrientation.Text = this._rotate + "°";
				int rotate = this._rotate;
				if (rotate <= 90)
				{
					if (rotate != 0)
					{
						if (rotate == 90)
						{
							this.pbPreview.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
						}
					}
					else
					{
						this.pbPreview.Image.RotateFlip(RotateFlipType.RotateNoneFlipNone);
					}
				}
				else if (rotate != 180)
				{
					if (rotate == 270)
					{
						this.pbPreview.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
					}
				}
				else
				{
					this.pbPreview.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
				}
				this.pbPreview.Size = this.pbPreview.Image.Size;
				this.SetZoom();
				this.pbPreview.Refresh();
			}
		}

		private void SetZoom()
		{
			decimal d;
			if (decimal.TryParse(this.cbZoom.Text.Replace('%', ' '), out d) && this.CurrentImage != null)
			{
				this.pbPreview.Height = (int)Math.Round(this.pbPreview.Image.Height / (100m / d));
				this.pbPreview.Width = (int)Math.Round(this.pbPreview.Image.Width / (100m / d));
				this.cbZoom.Text = this.cbZoom.Text.Replace('%', ' ').Trim() + "%";
				this._zoom = this.cbZoom.Text;
				return;
			}
			this.cbZoom.Text = this._zoom;
		}

		void RemoveAllImage()
		{
			this.CurrentImage = null;
			this._zplImageList.Clear();
			this._index = null;
			this.lblLabelPosition.Text = "0/0";
			this.btnPreviousLabel.Enabled = false;
			this.btnNextLabel.Enabled = false;
		}

		public unsafe static Bitmap AutoCropImage(Bitmap b)
		{
			BitmapData bitmapData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			int stride = bitmapData.Stride;
			IntPtr scan = bitmapData.Scan0;
			int num = 0;
			int num2 = 0;
			byte* ptr = (byte*)((void*)scan);
			int num3 = stride - b.Width * 3;
			for (int i = 0; i < b.Height; i++)
			{
				for (int j = 0; j < b.Width; j++)
				{
					int num4 = (int)(*ptr);
					byte b2 = ptr[1];
					byte b3 = ptr[2];
					if (num4 != 255 && b2 != 255 && b3 != 255)
					{
						num = ((j > num) ? j : num);
						num2 = ((i > num2) ? i : num2);
					}
					ptr += 3;
				}
				ptr += num3;
			}
			b.UnlockBits(bitmapData);
			num += 20;
			num2 += 20;
			if (num < b.Width && num2 < b.Height)
			{
				try
				{
					Rectangle rect = new Rectangle(0, 0, num + 20, num2 + 20);
					return b.Clone(rect, b.PixelFormat);
				}
				catch (OutOfMemoryException)
				{
					return b;
				}
				//return b;
			}
			return b;
		}

		void AddImage(Image img, bool Croppable, BorderStyle border)
		{
			this.pbPreview.BorderStyle = border;
			if (this.tsbAutoCrop.Checked && Croppable)
			{
				Task.Factory.StartNew(delegate ()
				{
					img = CommandPreview.AutoCropImage((Bitmap)img);
				}).Wait();
			}
			this._zplImageList.Add(img);
			if (this._index == null)
			{
				this._index = new int?(0);
			}
			else
			{
				this._index++;
			}
			this.DisplayImage(new int?(0));
		}

		void DisplayImage(int? index)
		{
			if (this._index != null)
			{
				int value = index.Value;
				Bitmap currentImage = new Bitmap(this._zplImageList[value]);
				this.CurrentImage = currentImage;
				this.lblLabelPosition.Text = string.Format("{0}/{1}", index + 1, this._zplImageList.Count<Image>().ToString());
				this.lblLabelPosition.ToolTipText = string.Format("Label {0} on {1}", index + 1, this._zplImageList.Count<Image>().ToString());
			}
			int? num;
			int num2;
			if (this._zplImageList.Count > 1)
			{
				num = index + 1;
				num2 = this._zplImageList.Count;
				if (!(num.GetValueOrDefault() == num2 & num != null))
				{
					this.btnNextLabel.Enabled = true;
					goto IL_145;
				}
			}
			this.btnNextLabel.Enabled = false;
		IL_145:
			num = index;
			num2 = 0;
			if (!(num.GetValueOrDefault() == num2 & num != null))
			{
				this.btnPreviousLabel.Enabled = true;
			}
			else
			{
				this.btnPreviousLabel.Enabled = false;
			}
			this._index = index;
		}

		public async void Render(string zplText, LabelFormat lf, LabelaryDpmm ldpmm)
		{
			//bool bPrint = ((ToolStripButton)sender).Name == "btnPrintLabel";
			//this.btnPreview.Enabled = false;
			//this.btnPrintLabel.Enabled = false;
			//this.tsProgressBar.Value = 0;
			int num;
			try
			{
				string text = ZPLCode.Clean(zplText);
				if (!text.StartsWith("^XA") || !text.EndsWith("^XZ"))
				{
					throw new Exception("Invalid ZPL Code. Should start with ^XA and end with ^XZ!");
				}
				LabelFormat labelFormat = lf;//(LabelFormat)this.cbFormatLabel.SelectedItem;
											 //if (Settings.Default.VariableSubstitutionEnabled)
											 //{
											 //	text = ZPLCode.ReplaceVariable(text, this._dicVariableSubstitution, Settings.Default.VariableStartChars, Settings.Default.VariableEndChars);
											 //}
											 //if (bPrint && Settings.Default.zplRenderEngine == "L")
											 //{
											 //	if (string.IsNullOrEmpty(this.cbLocalPrinter.Text))
											 //	{
											 //		throw new Exception("Please select a local Zebra printer!");
											 //	}
											 //	RawPrinterHelper.SendStringToPrinter(this.cbLocalPrinter.Text, text);
											 //}
											 //else
				{
					this.RemoveAllImage();
					string[] tzplCode = Regex.Split(text, "\\^XZ");
					int i = 0;
					//this.tsProgressBar.Step = 1;
					//this.tsProgressBar.Minimum = 0;
					//this.tsProgressBar.Value = 0;
					//this.tsProgressBar.Maximum = tzplCode.Count<string>() - 1;
					string[] array = tzplCode;

					for (int j = 0; j < array.Length; j++)
					{
						string zplCode = array[j];
						num = i;
						i = num + 1;
						//this.tsProgressBar.PerformStep();
						//this.tsStatusApplication.Text = string.Format("Rendering label {0} of {1} ...", i.ToString(), (tzplCode.Count<string>() - 1).ToString());
						if (!string.IsNullOrEmpty(zplCode.Replace("\r\n", string.Empty)))
						{
							//if (Settings.Default.zplRenderEngine == "L")
							{
								LabelaryDpmm dpm = ldpmm;// (LabelaryDpmm)this.cbLabelaryWSdpmm.SelectedItem;
								string _width = "0";
								string _height = "0";
								_width = labelFormat.Width.ToInch().ToString().Replace(',', '.');
								_height = labelFormat.Height.ToInch().ToString().Replace(',', '.');
								//PictureBox pictureBox = this.pbPreview;
								Image img = await Task.Run<Image>(() => Labelary.LoadImageFromWebService(zplCode + "^XZ", dpm.Code, _width, _height));
								AddImage(img, false, BorderStyle.FixedSingle);
								//pictureBox = null;
							}
							//else
							//{
							//	int width = (int)labelFormat.Width.ToMillimeter();
							//	int height = (int)labelFormat.Height.ToMillimeter();
							//	zPictureBox zPictureBox = this.zpbPreview;
							//	Image img = await Task.Run<Image>(() => this._zp.ZplToPngAndLabel(zplCode + "^XZ", bPrint, width, height));
							//	zPictureBox.AddImage(img, true, BorderStyle.None);
							//	zPictureBox = null;
							//}
						}
					}
					array = null;
					tzplCode = null;
				}
				labelFormat = null;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.StackTrace.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			finally
			{
				//this.tsProgressBar.Value = this.tsProgressBar.Maximum;
				//this.btnPrintLabel.Enabled = (this.bConnected || Settings.Default.zplRenderEngine == "L");
				//this.btnPreview.Enabled = true;
				//this.DisplayStatus();
			}
			//ToolStripProgressBar toolStripProgressBar = this.tsProgressBar;
			//num = await this.ResetProgressBar();
			//toolStripProgressBar.Value = num;
			//toolStripProgressBar = null;
		}

		private void cbZoom_SelectedIndexChanged(object sender, EventArgs e)
		{
			Settings.Default.ZoomPercent = this.cbZoom.Text;
			Settings.Default.Save();
			this.SetZoom();
		}

		private void cbZoom_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				this.SetZoom();
			}
		}

		private void btnZoomPlus_Click(object sender, EventArgs e)
		{
			if (this.cbZoom.SelectedIndex != 0 && this.cbZoom.SelectedIndex != -1)
			{
				this.cbZoom.SelectedIndex = this.cbZoom.SelectedIndex - 1;
			}
		}

		private void btnZoomMinus_Click(object sender, EventArgs e)
		{
			if (this.cbZoom.SelectedIndex != 20)
			{
				this.cbZoom.SelectedIndex = this.cbZoom.SelectedIndex + 1;
			}
		}

		private void pbPreview_Click(object sender, EventArgs e)
		{
			if (((MouseEventArgs)e).Button == MouseButtons.Right && this.pbPreview.Image != null)
			{
				this.contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
			}
		}

		private void btnCopyImage_Click(object sender, EventArgs e)
		{
			if (this.pbPreview.Image != null)
			{
				Clipboard.SetImage(this.pbPreview.Image);
			}
		}

		private void tsmiCopyImage_Click(object sender, EventArgs e)
		{
			this.btnCopyImage.PerformClick();
		}

		private void btnChangeLabel_Click(object sender, EventArgs e)
		{
			if (((ToolStripButton)sender).Name == "btnPreviousLabel")
			{
				this._index--;
			}
			else
			{
				this._index++;
			}
			this.DisplayImage(this._index);
		}

		private void tsbAutoCrop_Click(object sender, EventArgs e)
		{
			this.tsbAutoCrop.Checked = !this.tsbAutoCrop.Checked;
			this.tsbAutoCrop.Text = (this.tsbAutoCrop.Checked ? "On" : "Off");
		}
	}
}
