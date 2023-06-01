using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YProgramStudio.LabelsDesigner.Labels;
using YProgramStudio.LabelsDesigner.Model;
using YProgramStudio.LabelsDesigner.Properties;

namespace YProgramStudio.LabelsDesigner.Gui
{
	/// <summary>
	/// Label Editor Core
	/// </summary>
	public class LabelEditor : SkiaSharp.Views.Desktop.SKControl
	{
		int physicalDpiX = 158; // TODO: *获取设备物理DPI

		#region Private data

		const int nZoomLevels = 11;
		static readonly float[] zoomLevels = new float[nZoomLevels] { 8, 6, 4, 3, 2, 1.5F, 1, 0.75F, 0.67F, 0.50F, 0.33F };

		const float ZOOM_TO_FIT_PAD = 16.0F;

		static readonly SKColor backgroundColor = System.Drawing.Color.FromArgb(192, 192, 192).ToSKColor();

		static readonly SKColor shadowColor = System.Drawing.Color.FromArgb(128, 64, 64, 64).ToSKColor();
		const float shadowOffsetPixels = 4;

		static readonly SKColor labelColor = System.Drawing.Color.FromArgb(255, 255, 255).ToSKColor();
		static readonly SKColor labelOutlineColor = System.Drawing.Color.FromArgb(0, 0, 0).ToSKColor();
		const float labelOutlineWidthPixels = 0;

		static readonly SKColor gridLineColor = System.Drawing.Color.FromArgb(192, 192, 192).ToSKColor();
		const float gridLineWidthPixels = 0; // 原本是1px，但是还没有找到在Skia中固定一像素的方法，所以设置为0就会绘制为1像素（Skia的发际线模式）
		static readonly Distance gridSpacing = Distance.Pt(9); // TODO: *determine from locale.

		static readonly SKColor markupLineColor = System.Drawing.Color.FromArgb(240, 99, 99).ToSKColor();
		const float markupLineWidthPixels = 0;

		static readonly SKColor selectRegionFillColor = System.Drawing.Color.FromArgb(128, 192, 192, 255).ToSKColor();
		static readonly SKColor selectRegionOutlineColor = System.Drawing.Color.FromArgb(128, 0, 0, 255).ToSKColor();
		const float selectRegionOutlineWidthPixels = 3;

		//QScrollArea* mScrollArea;

		private Model.Model _model;
		private UndoRedoModel _undoRedoModel;

		private float _zoom;
		private bool _zoomToFitFlag;
		private float _scale;
		private Distance _x0; // 标签左上角的坐标（_x0, _y0）
		private Distance _y0;

		private bool _markupVisible;
		private bool _gridVisible;

		private float _gridSpacing;
		private Distance _stepSize;

		private OperateState _operateState;

		/* OperateState.ArrowSelectRegion state */
		private bool _selectRegionVisible;
		private Region _selectRegion;

		/* OperateState.ArrowMove state */
		private Distance _moveLastX;
		private Distance _moveLastY;

		/* OperateState.ArrowResize state */
		private ModelObject _resizeObject;
		private Handle _resizeHandle;
		private bool _resizeHonorAspect;

		/* OperateState.CreateDrag state */
		private CreateType _createObjectType;
		private ModelObject _createObject;
		private Distance _createX0;
		private Distance _createY0;

		#endregion

		#region Events

		public event EventHandler ContextMenuActivate;
		public event ZoomChangeEventHandler ZoomChanged;
		public event PointerMoveEventHandler PointerMoved;
		public event EventHandler PointerExited;
		public event EventHandler ModeChanged;

		#endregion

		public LabelEditor()
		{
			InitializeComponent();
			InitData();
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// LabelEditor
			// 
			this.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs>(this.LabelEditor_PaintSurface);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LabelEditor_KeyDown);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LabelEditor_MouseDown);
			this.MouseLeave += new System.EventHandler(this.LabelEditor_MouseLeave);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LabelEditor_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LabelEditor_MouseUp);
			this.Resize += new System.EventHandler(this.LabelEditor_Resize);
			this.ResumeLayout(false);
		}

		private void InitData()
		{
			_selectRegion = new Region();
			_zoom = 1;
			_zoomToFitFlag = false;
			_scale = 1;
			_markupVisible = true;
			_gridVisible = true;
			_gridSpacing = 18;

			_operateState = OperateState.IdleState;

			_selectRegionVisible = false;
			_resizeObject = null;
			_resizeHandle = null;
			_resizeHonorAspect = false;
			_createObjectType = CreateType.Box;
			_createObject = null;
		}

		#region Event handlers

		private void LabelEditor_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
		{
			if (_model != null)
			{
				//QPainter painter(this );
				//painter.setRenderHint(QPainter::Antialiasing, true);
				//painter.setRenderHint(QPainter::TextAntialiasing, true);
				//painter.setRenderHint(QPainter::SmoothPixmapTransform, true);

				/* Fill background before any transformations */
				using (SKPaint paint = new SKPaint()
				{
					Color = backgroundColor,
					Style = SKPaintStyle.Fill,
					IsAntialias = true
				})
				{
					e.Surface.Canvas.DrawRect(ClientRectangle.ToSKRect(), paint);
				}

				/* Transform. */
				e.Surface.Canvas.Scale(_scale, _scale);
				e.Surface.Canvas.Translate(_x0.Pt(), _y0.Pt()); // 将画布的原点调整到标签左上角的坐标（_x0, _y0），之后所有的绘制以此为参考点

				/* Now draw from the bottom layer up. */
				DrawBackgroundLayer(e.Surface.Canvas);
				DrawGridLayer(e.Surface.Canvas);
				DrawMarkupLayer(e.Surface.Canvas);
				DrawObjectsLayer(e.Surface.Canvas);
				DrawForegroundLayer(e.Surface.Canvas);
				DrawHighlightLayer(e.Surface.Canvas);
				DrawSelectRegionLayer(e.Surface.Canvas);
			}
		}

		private void LabelEditor_Resize(object sender, EventArgs e)
		{
			if (_model != null)
			{
				if (_zoomToFitFlag)
				{
					ZoomToFit();
				}
				else
				{
					/* Re-adjust origin to center label in widget. */
					_x0 = (Width / _scale - _model.Width) / 2F;
					_y0 = (Height / _scale - _model.Height) / 2F;

					Refresh();
				}
			}
		}

		private void LabelEditor_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (_model != null)
			{
				/*
				 * Transform to label coordinates 转换为标签坐标（目的是为了从鼠标坐标点转为标签上的坐标点）
				 */
				SKMatrix matrix = SKMatrix.CreateScaleTranslation(_scale, _scale, _x0.Pt() * _scale, _y0.Pt() * _scale);
				SKMatrix inverseMatrix;
				bool success = matrix.TryInvert(out inverseMatrix);
				SKPoint pWorld = inverseMatrix.MapPoint(e.Location.ToSKPoint());

				Distance xWorld = Distance.Pt(pWorld.X);
				Distance yWorld = Distance.Pt(pWorld.Y);

				if (e.Button == MouseButtons.Left)
				{
					//
					// LEFT BUTTON
					//
					switch (_operateState)
					{
						case OperateState.IdleState:
							{
								ModelObject modelObject;
								Handle handle;
								if (_model.IsSelectionAtomic() && (handle = _model.HandleAt(_scale, xWorld, yWorld)) != null)
								{
									//
									// Start an object resize
									//
									_resizeObject = handle.Owner;
									_resizeHandle = handle;
									_resizeHonorAspect = (ModifierKeys & Keys.Control) == Keys.Control;

									if (_resizeObject.LockAspectRatio)
									{
										_resizeHonorAspect = !_resizeHonorAspect;
									}

									_operateState = OperateState.ArrowResize;
								}
								else if ((modelObject = _model.ObjectAt(_scale, xWorld, yWorld)) != null)
								{
									//
									// Start a Move Selection (adjusting selection if necessary)
									//
									if ((ModifierKeys & Keys.Control) == Keys.Control)
									{
										if (modelObject.Selected)
										{
											// Un-selecting a selected item
											_model.UnselectObject(modelObject);
										}
										else
										{
											// Add to current selection
											_model.SelectObject(modelObject);
										}
									}
									else
									{
										if (!modelObject.Selected)
										{
											// Replace current selection with this object
											_model.UnselectAll();
											_model.SelectObject(modelObject);
										}
									}

									_moveLastX = xWorld;
									_moveLastY = yWorld;

									_operateState = OperateState.ArrowMove;
								}
								else
								{
									//
									// Start a Select Region
									//
									if ((ModifierKeys & Keys.Control) != Keys.Control)
									{
										_model.UnselectAll();
									}

									_selectRegionVisible = true;
									_selectRegion.X1 = xWorld;
									_selectRegion.Y1 = yWorld;
									_selectRegion.X2 = xWorld;
									_selectRegion.Y2 = yWorld;

									_operateState = OperateState.ArrowSelectRegion;
									Refresh();
								}
							}
							break;

						case OperateState.CreateIdle:
							{
								switch (_createObjectType)
								{
									case CreateType.Box:
										_createObject = new ModelBoxObject();
										break;
									case CreateType.Ellipse:
										_createObject = new ModelEllipseObject();
										break;
									case CreateType.Line:
										_createObject = new ModelLineObject();
										break;
									case CreateType.Image:
										_createObject = new ModelImageObject();
										break;
									case CreateType.Text:
										_createObject = new ModelTextObject();
										break;
									case CreateType.Barcode:
										_createObject = new ModelBarcodeObject();
										break;
									default:
										//"LabelEditor::mousePressEvent: Invalid creation type. Should not happen!";
										break;
								}

								_createObject.SetPosition(xWorld, yWorld);
								_createObject.SetSize(0, 0); // TODO: 新增的时候好像大小有问题导致显示不出来
								_model.AddObject(_createObject);

								_model.UnselectAll();
								_model.SelectObject(_createObject);

								_createX0 = xWorld;
								_createY0 = yWorld;

								_operateState = OperateState.CreateDrag;
							}
							break;

						default:
							{
								// "LabelEditor::mousePressEvent: Invalid state. Should not happen!";
							}
							break;
					}
				}
				else if (e.Button == MouseButtons.Right)
				{
					//
					// RIGHT BUTTON
					//
					if (_operateState == OperateState.IdleState)
					{
						ContextMenuActivate?.Invoke(this, new EventArgs());
					}
				}
			}
		}

		private void LabelEditor_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (_model != null)
			{
				/*
				 * Transform to label coordinates
				 */
				SKMatrix matrix = SKMatrix.CreateScaleTranslation(_scale, _scale, _x0.Pt() * _scale, _y0.Pt() * _scale);
				SKMatrix inverseMatrix;
				bool success = matrix.TryInvert(out inverseMatrix);
				SKPoint pWorld = inverseMatrix.MapPoint(e.Location.ToSKPoint());

				Distance xWorld = Distance.Pt(pWorld.X);
				Distance yWorld = Distance.Pt(pWorld.Y);

				/*
				 * Emit signal regardless of mode
				 */
				if (PointerMoved != null)
				{
					PointerMoved(this, new PointerMoveEventArgs(xWorld, yWorld));
				}

				/*
				 * Handle event as appropriate for state
				 */
				switch (_operateState)
				{
					case OperateState.IdleState:
						Handle handle;
						if (_model.IsSelectionAtomic() && (handle = _model.HandleAt(_scale, xWorld, yWorld)) != null)
						{
							if (handle is HandleNorth || handle is HandleSouth)
							{
								Cursor = Cursors.SizeNS;
							}
							else if (handle is HandleWest || handle is HandleEast)
							{
								Cursor = Cursors.SizeWE;
							}
							else if (handle is HandleNorthEast || handle is HandleSouthWest)
							{
								Cursor = Cursors.SizeNESW;
							}
							else if (handle is HandleNorthWest || handle is HandleSouthEast)
							{
								Cursor = Cursors.SizeNWSE;
							}
						}
						else if (_model.ObjectAt(_scale, xWorld, yWorld) != null)
						{
							Cursor = Cursors.SizeAll; // 四个方向有箭头
						}
						else
						{
							Cursor = Cursors.Arrow;
						}
						break;

					case OperateState.ArrowSelectRegion:
						_selectRegion.X2 = xWorld;
						_selectRegion.Y2 = yWorld;
						Refresh();
						break;

					case OperateState.ArrowMove:
						_undoRedoModel.Checkpoint(TranslateHelper.Tr("Move"));
						_model.MoveSelection((xWorld - _moveLastX), (yWorld - _moveLastY));
						_moveLastX = xWorld;
						_moveLastY = yWorld;
						break;

					case OperateState.ArrowResize:
						_undoRedoModel.Checkpoint(TranslateHelper.Tr("Resize"));
						HandleResizeMotion(xWorld, yWorld);
						break;

					case OperateState.CreateIdle:
						break;

					case OperateState.CreateDrag:
						switch (_createObjectType)
						{
							case CreateType.Box:
							case CreateType.Ellipse:
							case CreateType.Image:
							case CreateType.Text:
							case CreateType.Barcode:
								_createObject.SetPosition(Math.Min(xWorld, _createX0), Math.Min(yWorld, _createY0));
								_createObject.SetSize(Math.Max(xWorld, _createX0) - Math.Min(xWorld, _createX0), Math.Max(yWorld, _createY0) - Math.Min(yWorld, _createY0));
								break;
							case CreateType.Line:
								_createObject.SetSize(xWorld - _createX0, yWorld - _createY0);
								break;
							default:
								// "LabelEditor::mouseMoveEvent: Invalid creation mode. Should not happen!";
								break;
						}
						break;

					default:
						// "LabelEditor::mouseMoveEvent: Invalid state. Should not happen!";
						break;
				}
			}
		}

		private void LabelEditor_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (_model != null)
			{
				/*
				 * Transform to label coordinates
				 */
				SKMatrix matrix = SKMatrix.CreateScaleTranslation(_scale, _scale, _x0.Pt() * _scale, _y0.Pt() * _scale);
				SKMatrix inverseMatrix;
				bool success = matrix.TryInvert(out inverseMatrix);
				SKPoint pWorld = inverseMatrix.MapPoint(e.Location.ToSKPoint());

				Distance xWorld = Distance.Pt(pWorld.X);
				Distance yWorld = Distance.Pt(pWorld.Y);

				if (e.Button == MouseButtons.Left)
				{
					//
					// LEFT BUTTON Release
					//
					switch (_operateState)
					{
						case OperateState.ArrowResize:
							_operateState = OperateState.IdleState;
							break;

						case OperateState.ArrowSelectRegion:
							_selectRegionVisible = false;
							_selectRegion.X2 = xWorld;
							_selectRegion.Y2 = yWorld;

							_model.SelectRegion(_selectRegion);

							_operateState = OperateState.IdleState;
							Refresh();
							break;

						case OperateState.CreateDrag:
							if ((Math.Abs(_createObject.Width) < 4) && (Math.Abs(_createObject.Height) < 4))
							{
								switch (_createObjectType)
								{
									case CreateType.Text:
										_createObject.SetSize(72, 36);
										break;
									case CreateType.Barcode:
										_createObject.SetSize(72, 36);
										break;
									case CreateType.Line:
										_createObject.SetSize(72, 0);
										break;
									default:
										_createObject.SetSize(72, 72);
										break;
								}
							}

							Cursor = Cursors.Arrow;
							_operateState = OperateState.IdleState;
							break;

						default:
							_operateState = OperateState.IdleState;
							break;

					}

				}
			}
		}

		private void LabelEditor_MouseLeave(object sender, EventArgs e)
		{
			if (_model != null)
			{
				if (PointerExited != null)
				{
					PointerExited(this, new EventArgs());
				}
			}
		}

		private void LabelEditor_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (_operateState == OperateState.IdleState)
			{
				switch (e.KeyCode)
				{
					case Keys.Left:
						_undoRedoModel.Checkpoint(TranslateHelper.Tr("Move"));
						_model.MoveSelection(-_stepSize, new Distance(0));
						break;

					case Keys.Up:
						_undoRedoModel.Checkpoint(TranslateHelper.Tr("Move"));
						_model.MoveSelection(new Distance(0), -_stepSize);
						break;

					case Keys.Right:
						_undoRedoModel.Checkpoint(TranslateHelper.Tr("Move"));
						_model.MoveSelection(_stepSize, new Distance(0));
						break;

					case Keys.Down:
						_undoRedoModel.Checkpoint(TranslateHelper.Tr("Move"));
						_model.MoveSelection(new Distance(0), _stepSize);
						break;

					case Keys.Delete:
						_undoRedoModel.Checkpoint(TranslateHelper.Tr("Delete"));
						_model.DeleteSelection();
						Cursor = Cursors.Arrow;
						break;

					default:
						//QWidget::keyPressEvent( event );
						break;

				}
			}
		}

		private void OnSettingsChanged()
		{
			Units units = Settings.Units();

			_stepSize = new Distance(units.Resolution(), units);
		}

		private void OnModelSizeChanged()
		{
			if (_zoomToFitFlag)
			{
				Panel panel = Parent as Panel;

				float wPixels = panel.ClientSize.Width; // _scrollArea->maximumViewportSize().width();
				float hPixels = panel.ClientSize.Height; // _scrollArea->maximumViewportSize().height();

				float x_scale = (wPixels - ZOOM_TO_FIT_PAD) / _model.Width.Pt();
				float y_scale = (hPixels - ZOOM_TO_FIT_PAD) / _model.Height.Pt();
				float newZoom = Math.Min(x_scale, y_scale) * Constants.PTS_PER_INCH / physicalDpiX;

				// Limits
				newZoom = Math.Min(newZoom, zoomLevels[0]);
				newZoom = Math.Max(newZoom, zoomLevels[nZoomLevels - 1]);

				_zoom = newZoom;
			}

			/* Actual scale depends on DPI of display (assume DpiX == DpiY). */
			_scale = _zoom * physicalDpiX / Constants.PTS_PER_INCH;

			//setMinimumSize(_scale * _model.Width.Pt() + ZOOM_TO_FIT_PAD, _scale * _model.Height.Pt() + ZOOM_TO_FIT_PAD);
			MinimumSize = new System.Drawing.Size((int)(_scale * _model.Width.Pt() + ZOOM_TO_FIT_PAD), (int)(_scale * _model.Height.Pt() + ZOOM_TO_FIT_PAD));

			/* Adjust origin to center label in widget. */
			_x0 = (Width / _scale - _model.Width) / 2f; // 标签左上角的坐标（_x0, _y0）
			_y0 = (Height / _scale - _model.Height) / 2f;

			Refresh();

			ZoomChanged(this, new ZoomChangeEventArgs(_x0 * _scale, _y0 * _scale));
		}

		#endregion

		#region Model & Visibility operations

		public void SetModel(Model.Model model, UndoRedoModel undoRedoModel)
		{
			_model = model;
			_undoRedoModel = undoRedoModel;

			if (model != null)
			{
				ZoomToFit();

				model.Changed += (s, e) => Refresh();
				model.SelectionChanged += (s, e) => Refresh();
				model.SizeChanged += (s, e) => OnModelSizeChanged();

				Refresh();
			}
		}

		public void SetGridVisible(bool visibleFlag)
		{
			_gridVisible = visibleFlag;
			Refresh();
		}

		public void SetMarkupVisible(bool visibleFlag)
		{
			_markupVisible = visibleFlag;
			Refresh();
		}

		#endregion

		#region Zoom operations

		public bool IsZoomMaX => _zoom >= zoomLevels[0];

		public bool IsZoomMin => _zoom <= zoomLevels[nZoomLevels - 1];

		public void Zoom1To1() => SetZoomReal(1.0F, false);

		/// <summary>
		/// Zoom In "One Notch"
		/// </summary>
		public void ZoomIn()
		{
			// Find closest standard zoom level to our current zoom
			// Start with 2nd largest scale
			int min = 1;
			float distMin = Math.Abs(zoomLevels[1] - _zoom);

			for (int i = 2; i < nZoomLevels; i++)
			{
				float dist = Math.Abs(zoomLevels[i] - _zoom);
				if (dist < distMin)
				{
					min = i;
					distMin = dist;
				}
			}

			// Zoom in one notch
			SetZoomReal(zoomLevels[min - 1], false);
		}

		/// <summary>
		/// Zoom Out "One Notch"
		/// </summary>
		public void ZoomOut()
		{
			// Find closest standard zoom level to our current zoom
			// Start with largest scale, end on 2nd smallest
			int min = 0;
			float distMin = Math.Abs(zoomLevels[0] - _zoom);

			for (int i = 1; i < (nZoomLevels - 1); i++)
			{
				float dist = Math.Abs(zoomLevels[i] - _zoom);
				if (dist < distMin)
				{
					min = i;
					distMin = dist;
				}
			}

			// Zoom out one notch
			SetZoomReal(zoomLevels[min + 1], false);
		}

		/// <summary>
		/// 适合窗口
		/// </summary>
		public void ZoomToFit()
		{
			Panel panel = Parent as Panel;

			float wPixels = panel.ClientSize.Width; //panel.MaximumSize.Width;// ClientRectangle.Width; //mScrollArea->maximumViewportSize().width();
			float hPixels = panel.ClientSize.Height; //panel.MaximumSize.Height;// ClientSize.Height; //mScrollArea->maximumViewportSize().height();

			float x_scale = (wPixels - ZOOM_TO_FIT_PAD) / _model.Width.Pt();
			float y_scale = (hPixels - ZOOM_TO_FIT_PAD) / _model.Height.Pt();
			float newZoom = Math.Min(x_scale, y_scale) * Constants.PTS_PER_INCH / physicalDpiX;

			// Limits
			newZoom = Math.Min(newZoom, zoomLevels[0]);
			newZoom = Math.Max(newZoom, zoomLevels[nZoomLevels - 1]);

			SetZoomReal(newZoom, true);
		}

		private void SetZoomReal(float zoom, bool zoomToFitFlag)
		{
			_zoom = zoom;
			_zoomToFitFlag = zoomToFitFlag;

			/* Actual scale depends on DPI of display (assume DpiX == DpiY). */
			_scale = zoom * physicalDpiX / Constants.PTS_PER_INCH;

			//SetMinimumSize(_scale * _model.Width.Pt() + ZOOM_TO_FIT_PAD, _scale * _model.Height.Pt() + ZOOM_TO_FIT_PAD);
			MinimumSize = new System.Drawing.Size((int)(_scale * _model.Width.Pt() + ZOOM_TO_FIT_PAD), (int)(_scale * _model.Height.Pt() + ZOOM_TO_FIT_PAD));

			/* Adjust origin to center label in widget. */
			_x0 = (Width / _scale - _model.Width) / 2F; // 标签左上角的坐标（_x0, _y0）
			_y0 = (Height / _scale - _model.Height) / 2F;

			Refresh();

			ZoomChanged?.Invoke(this, new ZoomChangeEventArgs(_x0 * _scale, _y0 * _scale));
		}

		#endregion

		#region Mode operations

		public void ArrowMode()
		{
			Cursor = Cursors.Arrow;
			_operateState = OperateState.IdleState;
		}

		public void CreateBoxMode()
		{
			Cursor = new Cursor(Resource.cursor_box.GetHicon());
			_createObjectType = CreateType.Box;
			_operateState = OperateState.CreateIdle;
		}

		public void CreateEllipseMode()
		{
			Cursor = new Cursor(Resource.cursor_ellipse.GetHicon());
			_createObjectType = CreateType.Ellipse;
			_operateState = OperateState.CreateIdle;
		}

		public void CreateLineMode()
		{
			Cursor = new Cursor(Resource.cursor_line.GetHicon());
			_createObjectType = CreateType.Line;
			_operateState = OperateState.CreateIdle;
		}

		public void CreateImageMode()
		{
			Cursor = new Cursor(Resource.cursor_image.GetHicon());
			_createObjectType = CreateType.Image;
			_operateState = OperateState.CreateIdle;
		}

		public void CreateTextMode()
		{
			Cursor = new Cursor(Resource.cursor_text.GetHicon());
			_createObjectType = CreateType.Text;
			_operateState = OperateState.CreateIdle;
		}

		public void CreateBarcodeMode()
		{
			Cursor = new Cursor(Resource.cursor_barcode.GetHicon());
			_createObjectType = CreateType.Barcode;
			_operateState = OperateState.CreateIdle;
		}

		#endregion

		#region Private methods & Draw methods

		private void HandleResizeMotion(Distance xWorld, Distance yWorld)
		{
			SKPoint p = new SKPoint(xWorld.Pt(), yWorld.Pt());
			Location location = _resizeHandle.Location;

			/*
			 * Change point to object relative coordinates
			 */
			p -= new SKPoint(_resizeObject.X0.Pt(), _resizeObject.Y0.Pt());
			SKMatrix inverseMatrix;
			bool success = _resizeObject.Matrix.TryInvert(out inverseMatrix);
			p = inverseMatrix.MapPoint(p);

			/*
			 * Initialize origin and 2 corners in object relative coordinates.
			 */
			float x0 = 0.0f;
			float y0 = 0.0f;

			float x1 = 0.0f;
			float y1 = 0.0f;

			float x2 = _resizeObject.Width.Pt();
			float y2 = _resizeObject.Height.Pt();

			/*
			 * Calculate new size
			 */
			float w, h;
			switch (location)
			{
				case Labels.Location.NW:
					w = Math.Max(x2 - p.X, 0.0f);
					h = Math.Max(y2 - p.Y, 0.0f);
					break;
				case Labels.Location.N:
					w = x2 - x1;
					h = Math.Max(y2 - p.Y, 0.0f);
					break;
				case Labels.Location.NE:
					w = Math.Max(p.X - x1, 0.0f);
					h = Math.Max(y2 - p.Y, 0.0f);
					break;
				case Labels.Location.E:
					w = Math.Max(p.X - x1, 0.0f);
					h = y2 - y1;
					break;
				case Labels.Location.SE:
					w = Math.Max(p.X - x1, 0.0f);
					h = Math.Max(p.Y - y1, 0.0f);
					break;
				case Labels.Location.S:
					w = x2 - x1;
					h = Math.Max(p.Y - y1, 0.0f);
					break;
				case Labels.Location.SW:
					w = Math.Max(x2 - p.X, 0.0f);
					h = Math.Max(p.Y - y1, 0.0f);
					break;
				case Labels.Location.W:
					w = Math.Max(x2 - p.X, 0.0f);
					h = y2 - y1;
					break;
				case Labels.Location.P1:
					x1 = p.X;
					y1 = p.Y;
					w = x2 - p.X;
					h = y2 - p.Y;
					x0 = x0 + x1;
					y0 = y0 + y1;
					break;
				case Labels.Location.P2:
					w = p.X - x1;
					h = p.Y - y1;
					x0 = x0 + x1;
					y0 = y0 + y1;
					break;
				default:
					w = 0f;
					h = 0f;
					break; // "LabelEditor::handleResizeMotion: Invalid Handle Location. Should not happen!";
			}

			/*
			 * Set size
			 */
			if (!(location == Labels.Location.P1) && !(location == Labels.Location.P2))
			{
				if (_resizeHonorAspect)
				{
					switch (location)
					{
						case Labels.Location.E:
						case Labels.Location.W:
							_resizeObject.SetWHonorAspect(Distance.Pt(w));
							break;
						case Labels.Location.N:
						case Labels.Location.S:
							_resizeObject.SetHHonorAspect(Distance.Pt(h));
							break;
						default:
							_resizeObject.SetSizeHonorAspect(Distance.Pt(w), Distance.Pt(h));
							break;
					}
				}
				else
				{
					_resizeObject.SetSize(Distance.Pt(w), Distance.Pt(h));
				}

				/*
				 * Adjust origin, if needed.
				 */
				switch (location)
				{
					case Labels.Location.NW:
						x0 += x2 - _resizeObject.Width.Pt();
						y0 += y2 - _resizeObject.Height.Pt();
						break;
					case Labels.Location.N:
					case Labels.Location.NE:
						y0 += y2 - _resizeObject.Height.Pt();
						break;
					case Labels.Location.W:
					case Labels.Location.SW:
						x0 += x2 - _resizeObject.Width.Pt();
						break;
					default:
						break;
				}
			}
			else
			{
				_resizeObject.SetSize(Distance.Pt(w), Distance.Pt(h));
			}

			/*
			 * Put new origin back into world coordinates and set.
			 */
			SKPoint p0 = new SKPoint(x0, y0);
			p0 = _resizeObject.Matrix.MapPoint(p0);
			p0 += new SKPoint(_resizeObject.X0.Pt(), _resizeObject.Y0.Pt());
			_resizeObject.SetPosition(Distance.Pt(p0.X), Distance.Pt(p0.Y));
		}

		/// <summary>
		/// 绘制标签背景层
		/// </summary>
		private void DrawBackgroundLayer(SKCanvas painter)
		{
			/*
			 * Draw shadow
			 */
			painter.Save();

			painter.Translate(shadowOffsetPixels / _scale, shadowOffsetPixels / _scale);

			if (_model.Rotate)
			{
				painter.RotateDegrees(-90);
				painter.Translate(-_model.Frame.Width.Pt(), 0);
			}

			using (SKPaint paint = new SKPaint()
			{
				Color = shadowColor,
				Style = SKPaintStyle.Fill,
				IsAntialias = true
			})
			{
				painter.DrawPath(_model.Frame.Path(), paint);
			}

			painter.Restore();

			/*
			 * Draw label
			 */
			painter.Save();

			if (_model.Rotate)
			{
				painter.RotateDegrees(-90);
				painter.Translate(-_model.Frame.Width.Pt(), 0);
			}

			using (SKPaint paint = new SKPaint()
			{
				Color = labelColor,
				Style = SKPaintStyle.Fill,
				IsAntialias = true
			})
			{
				painter.DrawPath(_model.Frame.Path(), paint);
			}

			painter.Restore();
		}

		/// <summary>
		/// 绘制网格
		/// </summary>
		private void DrawGridLayer(SKCanvas painter)
		{
			if (_gridVisible)
			{
				Distance w = _model.Frame.Width;
				Distance h = _model.Frame.Height;

				Distance x0, y0;
				if (_model.Frame is FrameRect)
				{
					x0 = gridSpacing;
					y0 = gridSpacing;
				}
				else
				{
					/* round labels, adjust grid to line up with center of label. */
					x0 = Distance.Mod(w / 2f, gridSpacing);
					y0 = Distance.Mod(h / 2f, gridSpacing);
				}

				painter.Save();

				if (_model.Rotate)
				{
					painter.RotateDegrees(-90);
					painter.Translate(-_model.Frame.Width.Pt(), 0);
				}

				painter.ClipPath(_model.Frame.Path());

				using (SKPaint paint = new SKPaint()
				{
					Color = gridLineColor,
					Style = SKPaintStyle.Stroke,
					StrokeWidth = gridLineWidthPixels,
					IsAntialias = true
				})
				{
					for (Distance x = x0; x < w; x += gridSpacing)
					{
						painter.DrawLine(x.Pt(), 0, x.Pt(), h.Pt(), paint);
					}

					for (Distance y = y0; y < h; y += gridSpacing)
					{
						painter.DrawLine(0, y.Pt(), w.Pt(), y.Pt(), paint);
					}
				}

				painter.Restore();
			}
		}

		/// <summary>
		/// 绘制边线层
		/// </summary>
		private void DrawMarkupLayer(SKCanvas painter)
		{
			if (_markupVisible)
			{
				painter.Save();

				if (_model.Rotate)
				{
					painter.RotateDegrees(-90);
					painter.Translate(-_model.Frame.Width.Pt(), 0);
				}

				using (SKPaint paint = new SKPaint()
				{
					Color = markupLineColor,
					Style = SKPaintStyle.Stroke,
					StrokeWidth = markupLineWidthPixels,
					IsAntialias = true
				})
				{
					foreach (Markup markup in _model.Frame.Markups())
					{
						painter.DrawPath(markup.Path(_model.Frame), paint);
					}
				}

				painter.Restore();
			}
		}

		/// <summary>
		/// Draw Objects Layer
		/// </summary>
		private void DrawObjectsLayer(SKCanvas painter)
		{
			_model.Draw(painter, true, null, null);
		}

		/// <summary>
		/// Draw Foreground Layer
		/// </summary>
		private void DrawForegroundLayer(SKCanvas painter)
		{
			/*
			 * Draw label outline
			 */
			painter.Save();

			using (SKPaint paint = new SKPaint()
			{
				Color = labelOutlineColor,
				Style = SKPaintStyle.Stroke,
				StrokeWidth = labelOutlineWidthPixels,
				IsAntialias = true
			})
			{
				if (_model.Rotate)
				{
					painter.RotateDegrees(-90);
					painter.Translate(-_model.Frame.Width.Pt(), 0);
				}
				painter.DrawPath(_model.Frame.Path(), paint);
			}

			painter.Restore();
		}

		/// <summary>
		/// 绘制选中高亮框
		/// </summary>
		private void DrawHighlightLayer(SKCanvas painter)
		{
			painter.Save();

			foreach (ModelObject modelObject in _model.ObjectList)
			{
				if (modelObject.Selected)
				{
					modelObject.DrawSelectionHighlight(painter, _scale);
				}
			}

			painter.Restore();
		}

		/// <summary>
		/// 绘制鼠标选取框
		/// </summary>
		private void DrawSelectRegionLayer(SKCanvas painter)
		{
			if (_selectRegionVisible)
			{
				painter.Save();

				using (SKPaint paint = new SKPaint()
				{
					Color = selectRegionFillColor,
					Style = SKPaintStyle.Fill,
					IsAntialias = true
				})
				{
					painter.DrawRect(_selectRegion.Rect, paint);
					paint.Color = selectRegionOutlineColor;
					paint.Style = SKPaintStyle.Stroke;
					paint.StrokeWidth = selectRegionOutlineWidthPixels;
					painter.DrawRect(_selectRegion.Rect, paint);
				}

				painter.Restore();
			}
		}

		#endregion
	}
}
