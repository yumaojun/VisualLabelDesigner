using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Backends.Merge;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 打印分页呈现
	/// </summary>
	public class PageRenderer
	{
		#region Private Data

		readonly SKColor labelOutlineColor = System.Drawing.Color.FromArgb(0, 0, 0).ToSKColor();
		const float labelOutlineWidth = 0.25f;
		const float tickOffset = 2.25f;
		const float tickLength = 18;

		private SKRect _pageRect;
		private Model _model;

		private Merge _merge;
		private Variables _variables;

		int _nCopies = 5; // todo: _nCopies test
		int _startItem;
		int _lastItem;
		int _nGroups;
		int _nItemsPerGroup;
		int _nPagesPerGroup;
		int _iPage;

		bool _isMerge;

		int _nItems;
		int _nPages;
		int _nItemsPerPage;

		bool _isCollated;
		bool _areGroupsContiguous;
		bool _printOutlines;
		bool _printCropMarks;
		bool _printReverse;

		Point[] _origins;

		#endregion

		public EventHandler Changed;

		/// <summary>
		/// Get or set current model when print
		/// </summary>
		public Model Model
		{
			get => _model;
			set
			{
				_model = value;
				_model.Changed += new EventHandler(OnModelChanged); // TODO: 事件需要移除?
				OnModelChanged(null, null);
				_variables = _model.Variables;
			}
		}

		public SKRect PageRect { get => _pageRect; set => _pageRect = value; }

		public int IPage { get => _iPage; set => _iPage = value; }

		private void OnModelChanged(object sender, EventArgs e)
		{
			_merge = _model.Merge;
			_origins = _model.Frame.GetOrigins();
			_nItemsPerPage = _model.Frame.LabelsCount; //  TODO: LabelsCount()是否改为属性
			_isMerge = false; //_merge != null (或==none)
			UpdateNPages();

			//emit changed();
			Changed?.Invoke(null, null);
		}

		/// <summary>
		/// Print page using persistent page number
		/// </summary>
		public void PrintPage(SKCanvas painter)
		{
			PrintPage(painter, IPage);
		}

		/// Print page
		public void PrintPage(SKCanvas painter, int iPage)
		{
			if (_model != null)
			{
				if (!_isMerge)
				{
					PrintSimplePage(painter, iPage);
				}
				else
				{
					if (_isCollated)
					{
						PrintCollatedMergePage(painter, iPage);
					}
					else
					{
						PrintUnCollatedMergePage(painter, iPage);
					}
				}
			}
		}

		private void UpdateNPages() { }

		private void PrintSimplePage(SKCanvas painter, int iPage)
		{
			PrintCropMarks(painter);

			int iCopy = 0;
			int iItem = _startItem;
			int iCurrentPage = 0;
			_variables.ResetVariables();

			while ((iCopy < _nCopies) && (iCurrentPage <= iPage))
			{
				if (iCurrentPage == iPage)
				{
					int i = iItem % _nItemsPerPage;

					painter.Save();

					painter.Translate(_origins[i].X.Pt(), _origins[i].Y.Pt());

					painter.Save();

					ClipLabel(painter);
					PrintLabel(painter, null, _variables);

					painter.Restore();  // From before clip

					PrintOutline(painter);

					painter.Restore();  // From before translation
				}

				// Next copy
				iCopy++;
				iItem++;
				iCurrentPage = iItem / _nItemsPerPage;

				// User variable book keeping
				_variables.IncrementVariablesOnItem();
				_variables.IncrementVariablesOnCopy();
				if ((iItem % _nItemsPerPage) == 0 /* starting a new page */ )
				{
					_variables.IncrementVariablesOnPage();
				}
			}
		}

		private void PrintCollatedMergePage(SKCanvas painter, int iPage)
		{
			PrintCropMarks(painter);

			int iCopy = 0;
			int iItem = _startItem;
			int iCurrentPage = 0;

			List<Backends.Merge.Record> records = _merge.SelectedRecords();
			int iRecord = 0;
			int nRecords = records.Count;

			if (nRecords == 0)
			{
				return;
			}

			_variables.ResetVariables();

			while ((iCopy < _nCopies) && (iCurrentPage <= iPage))
			{
				if (iCurrentPage == iPage)
				{
					int i = iItem % _nItemsPerPage;

					painter.Save();

					painter.Translate(_origins[i].X.Pt(), _origins[i].Y.Pt());

					painter.Save();

					ClipLabel(painter);
					PrintLabel(painter, records[iRecord], _variables);

					painter.Restore();  // From before clip

					PrintOutline(painter);

					painter.Restore();  // From before translation
				}

				// Next record
				iRecord = (iRecord + 1) % nRecords;
				if (iRecord == 0)
				{
					iCopy++;
					if (_areGroupsContiguous)
					{
						iItem++;
					}
					else
					{
						iItem = iCopy * _nPagesPerGroup * _nItemsPerPage + _startItem;
					}
				}
				else
				{
					iItem++;
				}
				iCurrentPage = iItem / _nItemsPerPage;

				// User variable book keeping
				_variables.IncrementVariablesOnItem();
				if (iRecord == 0)
				{
					_variables.IncrementVariablesOnCopy();
				}
				if ((iItem % _nItemsPerPage) == 0 /* starting a new page */ )
				{
					_variables.IncrementVariablesOnPage();
				}
			}
		}

		private void PrintUnCollatedMergePage(SKCanvas painter, int iPage) { }

		private void PrintCropMarks(SKCanvas painter) { }

		private void PrintOutline(SKCanvas painter)
		{
			if (_printOutlines)
			{
				painter.Save();

				using (SKPaint paint = new SKPaint()
				{
					Color = labelOutlineColor,
					Style = SKPaintStyle.Stroke,
					StrokeWidth = labelOutlineWidth,
					IsAntialias = true
				})
				{
					painter.DrawPath(_model.Frame.Path(), paint);
				}

				painter.Restore();
			}
		}

		private void ClipLabel(SKCanvas painter)
		{
			painter.ClipPath(_model.Frame.ClipPath());
		}

		private void PrintLabel(SKCanvas painter, Backends.Merge.Record record, Variables variables)
		{
			painter.Save();

			if (_model.Rotate)
			{
				painter.RotateDegrees(-90.0f);
				painter.Translate(-_model.Width.Pt(), 0);
			}

			if (_printReverse)
			{
				painter.Translate(_model.Width.Pt(), 0);
				painter.Scale(-1, 1);
			}

			_model.Draw(painter, false, record, variables);

			painter.Restore();
		}

		/// Print
		public void Print(PrintDocument printer) 
		{
			//QSizeF pageSize(mModel->tmplate()->pageWidth().pt(), mModel->tmplate()->pageHeight().pt() );
			//printer->setPageSize(QPageSize(pageSize, QPageSize::Point) );
			//printer->setFullPage( true );
			//printer->setPageMargins( 0, 0, 0, 0, QPrinter::Point );

			//QPainter painter(printer );

			//QRectF rectPx = printer->paperRect(QPrinter::DevicePixel);
			//QRectF rectPts = printer->paperRect(QPrinter::Point);
			//painter.scale(rectPx.width()/rectPts.width(), rectPx.height()/rectPts.height() );


			//for (int iPage = 0; iPage<mNPages; iPage++ )
			//{
			//	if (iPage )
			//	{
			//		printer->newPage();
			//	}

			//	printPage( &painter, iPage );
			//}
		}
	}
}
