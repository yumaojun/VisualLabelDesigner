using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualLabelDesigner.ZplTextEditor.Services
{
	/// <summary>
	/// Converter Image to Zpl
	/// </summary>
	public class ZplImageConverter
	{
		private int blackLimit = 125;

		private int total;

		private int widthBytes;

		private bool compressHex;

		private Image _image;

		public int BlacknessLimitPercentage
		{
			get
			{
				return this.blackLimit * 100 / 255;
			}
			set
			{
				this.blackLimit = value * 255 / 100;
			}
		}

		public bool CompressHex
		{
			get
			{
				return this.compressHex;
			}
			set
			{
				this.compressHex = value;
			}
		}

		public Image Image
		{
			get
			{
				return this._image;
			}
			set
			{
				this._image = value;
			}
		}

		public string Value
		{
			get
			{
				if (this._image == null)
				{
					return "^A0,10,8^FDError converting image to ZPL!";
				}
				return this.FromImage((Bitmap)this._image);
			}
		}

		public string FromImage(Bitmap image)
		{
			string text = this.CreateBody(image);
			if (this.compressHex)
			{
				text = this.HexToAscii(text);
			}
			return this.HeadDoc() + text + "^FS";
		}

		private string CreateBody(Bitmap orginalImage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Graphics.FromImage(orginalImage).DrawImage(orginalImage, 0, 0);
			int height = orginalImage.Height;
			int width = orginalImage.Width;
			int num = 0;
			char[] array = new char[]
			{
				'0',
				'0',
				'0',
				'0',
				'0',
				'0',
				'0',
				'0'
			};
			this.widthBytes = width / 8;
			if (width % 8 > 0)
			{
				this.widthBytes = width / 8 + 1;
			}
			else
			{
				this.widthBytes = width / 8;
			}
			this.total = this.widthBytes * height;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					Color pixel = orginalImage.GetPixel(j, i);
					int r = (int)pixel.R;
					int g = (int)pixel.G;
					int b = (int)pixel.B;
					char c = '1';
					if ((r + g + b) / 3 > this.blackLimit || (int)pixel.A <= this.blackLimit)
					{
						c = '0';
					}
					array[num] = c;
					num++;
					if (num == 8 || j == width - 1)
					{
						stringBuilder.Append(this.ByteBinary(new string(array)));
						array = new char[]
						{
							'0',
							'0',
							'0',
							'0',
							'0',
							'0',
							'0',
							'0'
						};
						num = 0;
					}
				}
				stringBuilder.Append("\n");
			}
			return stringBuilder.ToString();
		}

		private string ByteBinary(string binary)
		{
			int num = Convert.ToInt32(binary, 2);
			if (num > 15)
			{
				return num.ToString("X").ToUpper();
			}
			return "0" + num.ToString("X").ToUpper();
		}

		private string HexToAscii(string code)
		{
			int num = this.widthBytes * 2;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			string value = null;
			int num2 = 1;
			char c = code[0];
			bool flag = false;
			for (int i = 1; i < code.Length; i++)
			{
				char c2 = code[i];
				if (flag)
				{
					c = code[i];
					flag = false;
				}
				else if (code[i] == '\n')
				{
					if (num2 >= num && c == '0')
					{
						stringBuilder2.Append(",");
					}
					else if (num2 >= num && c == 'F')
					{
						stringBuilder2.Append("!");
					}
					else if (num2 > 20)
					{
						int key = num2 / 20 * 20;
						int num3 = num2 % 20;
						stringBuilder2.Append(ZplImageConverter.mapCode[key]);
						if (num3 != 0)
						{
							stringBuilder2.Append(ZplImageConverter.mapCode[num3] + c.ToString());
						}
						else
						{
							stringBuilder2.Append(c);
						}
					}
					else
					{
						stringBuilder2.Append(ZplImageConverter.mapCode[num2] + c.ToString());
						string text = ZplImageConverter.mapCode[num2];
					}
					num2 = 1;
					flag = true;
					if (stringBuilder2.ToString().Equals(value))
					{
						stringBuilder.Append(":");
					}
					else
					{
						stringBuilder.Append(stringBuilder2.ToString());
					}
					value = stringBuilder2.ToString();
					stringBuilder2.Clear();
				}
				else if (c == code[i])
				{
					num2++;
				}
				else
				{
					if (num2 > 20)
					{
						int key2 = num2 / 20 * 20;
						int num4 = num2 % 20;
						stringBuilder2.Append(ZplImageConverter.mapCode[key2]);
						if (num4 != 0)
						{
							stringBuilder2.Append(ZplImageConverter.mapCode[num4] + c.ToString());
						}
						else
						{
							stringBuilder2.Append(c);
						}
					}
					else
					{
						stringBuilder2.Append(ZplImageConverter.mapCode[num2] + c.ToString());
					}
					num2 = 1;
					c = code[i];
				}
			}
			return stringBuilder.ToString();
		}

		private string HeadDoc()
		{
			return string.Concat(new object[]
			{
				"^FO10,10^GFA,",
				this.total,
				",",
				this.total,
				",",
				this.widthBytes,
				", \n"
			});
		}

		private static Dictionary<int, string> mapCode = new Dictionary<int, string>
		{
			{ 1, "G" },
			{ 2, "H" },
			{ 3, "I" },
			{ 4, "J" },
			{ 5, "K" },
			{ 6, "L" },
			{ 7, "M" },
			{ 8, "N" },
			{ 9, "O" },
			{ 10, "P" },
			{ 11, "Q" },
			{ 12, "R" },
			{ 13, "S" },
			{ 14, "T" },
			{ 15, "U" },
			{ 16, "V" },
			{ 17, "W" },
			{ 18, "X" },
			{ 19, "Y" },
			{ 20, "g" },
			{ 40, "h" },
			{ 60, "i" },
			{ 80, "j" },
			{ 100, "k" },
			{ 120, "l" },
			{ 140, "m" },
			{ 160, "n" },
			{ 180, "o" },
			{ 200, "p" },
			{ 220, "q" },
			{ 240, "r" },
			{ 260, "s" },
			{ 280, "t" },
			{ 300, "u" },
			{ 320, "v" },
			{ 340, "w" },
			{ 360, "x" },
			{ 380, "y" },
			{ 400, "z" }
		};
	}
}
