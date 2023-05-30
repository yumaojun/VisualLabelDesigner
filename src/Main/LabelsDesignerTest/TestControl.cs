using SkiaSharp;
using SkiaSharp.SimpleText;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace LabelsDesignerTest
{
	public class TestControl : SkiaSharp.Views.Desktop.SKControl
	{
		//SkiaSharp.SKPath _path;

		public TestControl()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// TemplateControl
			// 
			this.BackColor = System.Drawing.Color.White;
			this.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs>(this.TemplateControl_PaintSurface);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.TemplateControl_Paint);
			this.ResumeLayout(false);

		}

		//System.Threading.Timer t1;

		private void Load()
		{
			//pageIsActive = true;
			//stopwatch.Start();
			//t1 = new System.Threading.Timer((x) =>
			//{
			//	double t = stopwatch.Elapsed.TotalMilliseconds % cycleTime /
			//	cycleTime;
			//	angle = (float)(360 * t);
			//	//canvasView.InvalidateSurface();
			//	Refresh();
			//	if (!pageIsActive)
			//	{
			//		stopwatch.Stop();
			//	}
			//	//return pageIsActive;
			//}
			//);
			//t1.

			//	.StartTimer(TimeSpan.FromMilliseconds(33), );
		}

		//public void UpdateView(SkiaSharp.SKPath path)
		//{
		//	//_path = path ?? new SkiaSharp.SKPath();
		//	//_path.AddCircle(50, 50, 20);
		//	//Refresh();
		//}

		private void TemplateControl_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
		{
			var imgInfo = e.Info;
			var surface = e.Surface;
			var canvas = surface.Canvas;
			canvas.Clear();

			//TranslateTextEffects(canvas);
			//HendecagramArray(canvas, imgInfo);
			//ScaleDemo(canvas);
			//ScaleCustomCenter(canvas, imgInfo);

			//DrawHorizontalRuler(canvas, 500f, 50, 50, 10);
			//DrawGrid(canvas, 500f, 100f);
			//SimpleText(canvas);
			MeasureText(canvas, imgInfo);
		}

		[DllImport("user32.dll")]
		private static extern uint GetDpiForWindow([In] IntPtr hmonitor);

		private void TemplateControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			// Dpi(e.Graphics);
		}

		private void Dpi(Graphics g)
		{
			// var path = new System.Drawing.Drawing2D.GraphicsPath();
			//path.AddRectangle(new System.Drawing.RectangleF(20, 20, 100, 50));
			//g.DrawPath(new System.Drawing.Pen(System.Drawing.Color.Red), path);
			var x = g.DpiX;
			var y = g.DpiY;

			g.DrawString($"{x},{y}", new System.Drawing.Font("宋体", 14f), System.Drawing.Brushes.Red, 1f, 1f);

			var g1 = CreateGraphics();
			//var vs = System.Environment.GetEnvironmentVariables();
			var x1 = g1.DpiX;
			var y1 = g1.DpiY;
			g1.DrawString($"{x1},{y1}", new System.Drawing.Font("宋体", 14f), System.Drawing.Brushes.Red, 1f, 14f);
			g1.Dispose();

			var u = GetDpiForWindow(Handle);
			g.DrawString($"{u},{u}", new System.Drawing.Font("宋体", 14f), System.Drawing.Brushes.Red, 1f, 28f);

			var dpiPropertyx = typeof(SystemParameters).GetProperty("DpiX", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			var dpix = (int)dpiPropertyx.GetValue(null, null);
			g.DrawString($"{dpix},{dpix}", new System.Drawing.Font("宋体", 14f), System.Drawing.Brushes.Red, 1f, 44f);
		}

		#region 平移

		// 累积平移
		private void AccumulatedTranslate(SKCanvas canvas, SKImageInfo imgInfo)
		{
			using (SKPaint strokePaint = new SKPaint())
			{
				strokePaint.Color = SKColors.Black;
				strokePaint.Style = SKPaintStyle.Stroke;
				strokePaint.StrokeWidth = 3;

				int rectCount = 20;
				SKRect rect1 = new SKRect(0, 0, 250, 250);

				float tdx = (imgInfo.Width - rect1.Width) / rectCount - 1;
				float tdy = (imgInfo.Height - rect1.Height) / rectCount - 1;

				for (int i = 0; i < rectCount; i++)
				{
					canvas.DrawRect(rect1, strokePaint);
					canvas.Translate(tdx, tdy);
				}
			}
		}

		// 具有阴影的文字
		private void TranslateTextEffects(SKCanvas canvas)
		{
			float textSize = 150;
			using (SKPaint textPaint = new SKPaint())
			{
				textPaint.Style = SKPaintStyle.Fill;
				textPaint.TextSize = textSize;
				textPaint.FakeBoldText = true;
				float x = 10;
				float y = textSize;
				// Shadow
				canvas.Translate(10, 10);
				textPaint.Color = SKColors.Black;
				canvas.DrawText("SHADOW", x, y, textPaint);
				canvas.Translate(-10, -10);
				textPaint.Color = SKColors.Pink;
				canvas.DrawText("SHADOW", x, y, textPaint);
				y += 2 * textSize;
				// Engrave
				canvas.Translate(-5, -5);
				textPaint.Color = SKColors.Black;
				canvas.DrawText("ENGRAVE", x, y, textPaint);
				canvas.ResetMatrix(); // 所有转换返回到其默认状态
				textPaint.Color = SKColors.White;
				canvas.DrawText("ENGRAVE", x, y, textPaint);
				y += 2 * textSize;
				// Emboss
				canvas.Save();
				canvas.Translate(5, 5);
				textPaint.Color = SKColors.Black;
				canvas.DrawText("EMBOSS", x, y, textPaint);
				canvas.Restore();
				textPaint.Color = SKColors.White;
				canvas.DrawText("EMBOSS", x, y, textPaint);
			}
		}

		private SKPath CreateHendecagramPath()
		{
			// Create 11-pointed star
			var hendecagramPath = new SKPath();
			for (int i = 0; i < 11; i++)
			{
				double angle = 5 * i * 2 * Math.PI / 11;
				SKPoint pt = new SKPoint(100 * (float)Math.Sin(angle),
				-100 * (float)Math.Cos(angle));
				if (i == 0)
				{
					hendecagramPath.MoveTo(pt);
				}
				else
				{
					hendecagramPath.LineTo(pt);
				}
			}
			hendecagramPath.Close();
			return hendecagramPath;
		}

		private void HendecagramArray(SKCanvas canvas, SKImageInfo info)
		{
			Random random = new Random();
			var hendecagramPath = CreateHendecagramPath();
			using (SKPaint paint = new SKPaint())
			{
				for (int x = 100; x < info.Width + 100; x += 200)
					for (int y = 100; y < info.Height + 100; y += 200)
					{
						// Set random color
						byte[] bytes = new byte[3];
						random.NextBytes(bytes);
						paint.Color = new SKColor(bytes[0], bytes[1], bytes[2]);
						// Display the hendecagram
						canvas.Save();
						canvas.Translate(x, y);
						canvas.DrawPath(hendecagramPath, paint);
						canvas.Restore();
					}
			}
			hendecagramPath.Dispose();
		}

		const double cycleTime = 5000; // in milliseconds
									   //SKCanvasView canvasView;
		Stopwatch stopwatch = new Stopwatch();
		bool pageIsActive;
		float angle;
		private void HendecagramAnimation(SKCanvas canvas, SKImageInfo info)
		{
			var hendecagramPath = CreateHendecagramPath();
			canvas.Translate(info.Width / 2, info.Height / 2);
			float radius = (float)Math.Min(info.Width, info.Height) / 2 - 100;
			using (SKPaint paint = new SKPaint())
			{
				paint.Style = SKPaintStyle.Fill;
				paint.Color = SKColor.FromHsl(angle, 100, 50);
				float x = radius * (float)Math.Sin(Math.PI * angle / 180);
				float y = -radius * (float)Math.Cos(Math.PI * angle / 180);
				canvas.Translate(x, y);
				canvas.DrawPath(hendecagramPath, paint);
			}
		}

		#endregion

		#region 缩放

		private void ScaleDemo(SKCanvas canvas)
		{
			var xScaleSlider = 0.5;
			var yScaleSlider = 2;
			var sTitle = "Basic Scale";
			using (SKPaint strokePaint = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColors.Red,
				StrokeWidth = 3,
				PathEffect = SKPathEffect.CreateDash(new float[] { 7, 7 }, 0) // 虚线
			})
			using (SKPaint textPaint = new SKPaint
			{
				Style = SKPaintStyle.Fill,
				Color = SKColors.Blue,
				TextSize = 50
			}) // 多个using语句可以堆叠
			{
				canvas.Scale((float)xScaleSlider, (float)yScaleSlider);
				SKRect textBounds = new SKRect();
				textPaint.MeasureText(sTitle, ref textBounds); // 测量文本
				float margin = 10;
				SKRect borderRect = SKRect.Create(new SKPoint(margin, margin), textBounds.Size);
				canvas.DrawRoundRect(borderRect, 20, 20, strokePaint);
				canvas.DrawText(sTitle, margin, -textBounds.Top + margin, textPaint);
			}
		}

		// 垂直缩放的负值，导致对象在穿过缩放中心的水平轴上翻转。水平缩放的负值，会导致对象在穿过缩放中心的垂直轴上翻转
		private void ScaleCustomCenter(SKCanvas canvas, SKImageInfo info)
		{
			var xScaleSlider = 0.5;
			var yScaleSlider = -2; // 负值导致翻转
			var sTitle = "Basic Scale";
			using (SKPaint strokePaint = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColors.Red,
				StrokeWidth = 3,
				PathEffect = SKPathEffect.CreateDash(new float[] { 7, 7 }, 0)
			})
			using (SKPaint textPaint = new SKPaint
			{
				Style = SKPaintStyle.Fill,
				Color = SKColors.Blue,
				TextSize = 50
			})
			{
				SKRect textBounds = new SKRect();
				textPaint.MeasureText(sTitle, ref textBounds);

				float margin = (info.Width - textBounds.Width) / 2;
				float sx = (float)xScaleSlider;
				float sy = (float)yScaleSlider;
				float px = margin + textBounds.Width / 2;
				float py = margin + textBounds.Height / 2;
				canvas.Scale(sx, sy, px, py);
				/*等效写法
				canvas.Translate(px, py);
				canvas.Scale(sx, sy);
				canvas.Translate(–px, –py);
				 */
				SKRect borderRect = SKRect.Create(new SKPoint(margin, margin),
				textBounds.Size);
				canvas.DrawRoundRect(borderRect, 20, 20, strokePaint);
				canvas.DrawText(sTitle, margin, -textBounds.Top + margin, textPaint);
			}
		}

		#endregion

		#region 矩阵

		private void TestMatrix()
		{
			float[] a = new float[9] { 1, 0, 0, 1, 0, 0, 0, 0, 1 };


		}

		#endregion

		#region 测试

		// 路径绘制
		private void DrawPath(SKCanvas canvas)
		{
			//var path =  new SkiaSharp.SKPath();
			//var rect = new SkiaSharp.SKRect(10, 10, 100, 60);
			//path.AddRect(rect);
			var paint = new SkiaSharp.SKPaint();
			var _path = new SkiaSharp.SKPath();
			var outerPath = new SkiaSharp.SKPath();
			var clipPath = new SkiaSharp.SKPath();
			//e.Surface.Canvas.Save();
			//e.Surface.Canvas.Translate(10, 20);
			//e.Surface.Canvas.DrawPath(path, paint);
			//e.Surface.Canvas.Restore();
			//path.Dispose();
			float r1 = 160.1575f;
			float r2 = 52.44094f;
			float wReal = 320.315f;
			float hReal = 320.315f;

			var left = (wReal / 2f - r1);
			var top = (hReal / 2f - r1);
			var right = (wReal / 2f - r1) + (2 * r1);
			var bottom = (hReal / 2f - r1) + (2 * r1);

			var outerRect = new SkiaSharp.SKRect(left, top, right, bottom);
			outerPath.AddOval(outerRect);

			var clipRect = new SkiaSharp.SKRect(0, 0, wReal, hReal);
			clipPath.AddRect(clipRect);

			//outerPath.Op(clipPath, SkiaSharp.SKPathOp.Intersect);
			//_path.AddPath(clipPath); // 取交集
			_path.Close();

			/*
			 * Add inner subpath
			 */
			var left1 = (wReal / 2f - r2);
			var top1 = (hReal / 2f - r2);
			var right1 = (wReal / 2f - r2) + (2 * r2);
			var bottom1 = (hReal / 2f - r2) + (2 * r2);
			var rect = new SkiaSharp.SKRect(left1, top1, right1, bottom1);
			_path.AddOval(rect);
			canvas.DrawPath(_path, paint);
		}

		/// <summary>
		/// 画尺
		/// </summary>
		/// <param name="canvas">画布</param>
		/// <param name="canvasWidth">画布宽度</param>
		/// <param name="RulerWidth">尺高</param>
		/// <param name="LargeSteps">最大刻度间距</param>
		/// <param name="SmallSteps">最小刻度间距</param>
		private void DrawHorizontalRuler(SKCanvas canvas, float canvasWidth, int RulerWidth, int LargeSteps, int SmallSteps)
		{
			var _displayScale = 1;
			var FontSize = 12;
			using (var paint = new SKPaint
			{
				Color = SKColors.Black,
				StrokeWidth = 1 * _displayScale,
				TextAlign = SKTextAlign.Center,
			})
			{
				canvas.DrawLine(0, 0, canvasWidth, 0, paint);
				canvas.DrawLine(0, RulerWidth, canvasWidth, RulerWidth, paint);

				for (int x = 0; x < canvasWidth; x += LargeSteps)
				{
					for (int x1 = x + SmallSteps; x1 < x + LargeSteps; x1 += SmallSteps)
					{
						canvas.DrawLine(x1, 0, x1, 10, paint);
					}
					canvas.DrawLine(x, 0, x, 20, paint);

					var typeface = SKTypeface.FromFamilyName(FontFamily.Families.First().Name);
					var font = new SKFont(typeface, (float)FontSize * _displayScale);
					var measurementText = SKTextBlob.Create($"{x}", font);
					canvas.DrawText(measurementText, x, 30, paint);
				}
			}
		}

		private void DrawGrid(SKCanvas canvas, float canvasWidth, float canvasHeight)
		{
			SKPaint paint = new SKPaint() { };

			paint.PathEffect = SKPathEffect.CreateDash(new float[] { 20, 20 }, 0);

			for (int i = 0; i <= 8; i++)
			{
				var x = i * canvasWidth / 8;
				var y = i * canvasHeight / 8;

				int textOffset;
				if (i > 0 && i < 8)
				{
					canvas.DrawLine(new SKPoint(x, 0), new SKPoint(x, canvasHeight), paint);
					canvas.DrawLine(new SKPoint(0, y), new SKPoint(canvasWidth, y), paint);
					textOffset = 0;
				}
				else if (i == 0)
				{
					textOffset = 15;
				}
				else
				{
					textOffset = -15;
				}

				var font = new SKFont(SKTypeface.Default, 12.0f);
				var measurementText = SKTextBlob.Create($"{i * 10}", font);
				canvas.DrawText(measurementText, x + textOffset, 10, paint);
				canvas.DrawText(measurementText, 5, y + textOffset, paint);
			}
		}

		#endregion

		#region 文本

		private void SimpleText(SKCanvas painter)
		{
			painter.Save();

			SKColor color = new SKColor(255, 1, 1);
			float mW = 200f;
			float mH = 100f;

			var rect = new SKRect(0, 0, mW, mH);
			//painter.ClipRect(rect );
			var paint = new SKPaint() { Color = color, Style = SKPaintStyle.Stroke, StrokeWidth = 1 };
			painter.DrawRect(rect, paint); // 外框

			string mText = "Hello \nWrold!"; // 文本
			string mFontFamily = "宋体"; // 字体
			float mFontSize = 16; // 字体大小(单位磅)
			STFontWeight mFontWeight = STFontWeight.Normal; // 字体粗细
			bool mFontItalicFlag = true; // 是否斜体
			bool mFontUnderlineFlag = true; // 下划线
			STAlignment mTextHAlign = STAlignment.AlignRight; // 水平居中
			STAlignment mTextVAlign = STAlignment.AlignVCenter; // 垂直居中
			STWrapMode mTextWrapMode = STWrapMode.NoWrap; // 自动换行：Word, AnyWhere, NoWrap
			float mTextLineSpacing = 1; // 行间距
			float marginPts = 3;

			STFont font = new STFont();
			font.Family = mFontFamily;
			font.PointSize = mFontSize;
			font.Weight = mFontWeight;
			font.Italic = mFontItalicFlag;
			font.Underline = mFontUnderlineFlag;

			STTextOption textOption = new STTextOption();
			textOption.Alignment = mTextHAlign;
			textOption.WrapMode = mTextWrapMode;

			STFontMetrics fontMetrics = new STFontMetrics(font);
			float dy = fontMetrics.LineSpacing * mTextLineSpacing;

			STTextDocument document = new STTextDocument(mText);

			List<STTextLayout> layouts = new List<STTextLayout>();

			// Pass #1 -- do initial layouts
			float x = 0f;
			float y = 0f;
			SKRect boundingRect = SKRect.Empty;
			for (int i = 0; i < document.BlockCount; i++)
			{
				STTextLayout layout = new STTextLayout(document.FindBlockByNumber(i).Text);

				layout.Font = font;
				layout.TextOption = textOption;
				layout.CacheEnabled = true;

				layout.BeginLayout();
				for (STTextLine l = layout.CreateLine(); l.IsValid(); l = layout.CreateLine())
				{
					l.LineWidth = mW - 2 * marginPts;
					l.Position = new SKPoint(x, y);
					y += dy;
				}
				layout.EndLayout();

				layouts.Add(layout);

				//boundingRect = layout->boundingRect().united( boundingRect );
				var temp = boundingRect;
				boundingRect = layout.Bounds;
				boundingRect.Union(temp);
			}
			var h = boundingRect.Height;

			// Pass #2 -- adjust layout positions for vertical alignment
			x = marginPts;
			switch (mTextVAlign)
			{
				case STAlignment.AlignVCenter:
					y = mH / 2 - h / 2;
					break;
				case STAlignment.AlignBottom:
					y = mH - h - marginPts;
					break;
				default:
					y = marginPts;
					break;
			}

			foreach (STTextLayout layout in layouts)
			{
				for (int j = 0; j < layout.LineCount; j++)
				{
					STTextLine l = layout.LineAt(j);
					l.Position = new SKPoint(x, y);
					y += dy;
				}
			}

			// Draw layouts
			//painter->setPen(QPen(color));
			foreach (STTextLayout layout in layouts)
			{
				layout.Draw(painter, new SKPoint(0, 0));
			}

			// Cleanup
			layouts.Clear();

			painter.Restore();

		}

		private void MeasureText(SKCanvas painter, SKImageInfo info)
		{
			string str = "Hello SkiaSharp!";

			// Create an SKPaint object to display the text
			SKPaint textPaint = new SKPaint
			{
				Color = SKColors.Chocolate
			};

			// Adjust TextSize property so text is 90% of screen width
			float textWidth = textPaint.MeasureText(str);
			textPaint.TextSize = 0.9f * info.Width * textPaint.TextSize / textWidth;

			// Find the text bounds
			SKRect textBounds = new SKRect();
			textPaint.MeasureText(str, ref textBounds);

			//painter.DrawText(str, -7, 56, textPaint);// x,y 以文本基线为坐标
			//textPaint.Style = SKPaintStyle.Stroke;
			//textPaint.StrokeWidth = 1;
			//painter.DrawRect(textBounds, textPaint);

			// Calculate offsets to center the text on the screen
			float xText = info.Width / 2 - textBounds.MidX;
			float yText = info.Height / 2 - textBounds.MidY;

			// And draw the text
			painter.DrawText(str, xText, yText, textPaint);
		}

		#endregion
	}
}
