/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Peter
 * Datum: 06.02.2009
 * Zeit: 22:53
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */

#if GRAPHICAL

using System;
using System.Collections.Generic;
using System.ComponentModel;  // für Resources
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Microsoft.VisualBasic;

/*
 *  Neues Konzept:
 *                                        limitedWidth
 *    0                                        v
 * 0 +-----------------------------------------------------------------+
 *   |layoutRect                                                       |
 *   |                                                                 |
 *   |                                                                 |
 *   |                   +--------------------------------------+      |
 *   |                   |canvasRect                            |      |
 *   |                   |                                      |      |
 *   |                   |                                      |      |
 *   |                   |         +------------------------+   |      |
 *   |                   |         |cameraRect              |   |      |
 *   |                   |         |                        |   |      |
 *   |                   |         |                        |   |      |(< limitedHeight)
 *   |                   |         |                        |   |      |(   abgeschafft )
 *   |                   |         |                        |   |      |
 *   |                   |         |                        |   |      |
 *   |                   |         |                        |   |      |
 *   |                   |         |                        |   |      |
 *   |                   |         |                        |   |      |
 *   |                   |         +------------------------+   |      |
 *   |                   |                                      |      |
 *   |                   +--------------------------------------+      |
 *   |                                                                 |
 *   |                                                                 |
 *   +-----------------------------------------------------------------+
 */

/*
 * Aufrufreihenfolge:
 * 
 *   EnsureVisible(forceUpdate)
 *              |
 *              V
 * ValidateCameraRect(forceUpdate)
 *     |  (bei Bedarf         |
 *     V od. forceUpdate)     V
 * CenterCanvas()         DisplayCamera()
 *        |
 *        V
 *  DrawCanvas()
 * 
 */

namespace FreeYourMind
{
	/// <summary>
	/// Description of MyTreeView.
	/// </summary>
	public delegate void MyTreeViewEventHandler(object sender, MyTreeViewEventArgs e);

	public class MyTreeView
	{
		private Bitmap  bmCollapsed;

		private const int  nofButtons = 7;
		private MyButton[] button = new MyButton[nofButtons];
		private int        buttonPressed = -1;
		private Bitmap     bitmapMenuBar;
		private Bitmap     pressedBackground;
		private Bitmap     pressedBackgroundPattern;

		private QuickContext quickContext;
		private const int MoveUp    = 0;
		private const int Sister    = 1;
		private const int Notes     = 2;
		private const int NewParent = 3;
		private const int Format    = 4;
		private const int Child     = 5;
		private const int MoveDown  = 6;
		private const int Brother   = 7;
		private const int Icons     = 8;

		public  PictureBox           PictureBox;
		private ImageList            imageList;
		private MyTreeNode           selectedNode;
		public  MyTreeNodeCollection Nodes;
		private MyTreeViewEventArgs  myTreeViewEventArgs;

		List<string> mapList;

		public  bool mindmapLoaded;

		private bool showDate;

		private Bitmap    canvasBitmap;
		private Rectangle layoutRect;
		private Rectangle canvasRect;  // Bezug layoutRect
		private Rectangle cameraRect;  // Bezug layoutRect
		private Rectangle navigatorLayoutRect;
		private Rectangle navigatorCameraRect;
		private int       navigatorMaxLength;
		private float     navigatorScale;
		private int       maxX;
		private int       maxY;
		private int       limitedWidth;
		private double    scaling = 1.0;
		public  int       mouseX;
		private int       mouseY;
		private int       selNodeRelativeX;
		private int       selNodeRelativeY;
		private bool      fixSelectedNode;

		public  bool      forceCenter;

		public Font fontFirst;
		public Font fontSecond;
		public Font fontRegular;

//		private Color lineColor    = Color.Black;
		private Color lineColor    = Color.FromArgb(51, 51, 51);  // Acrobat Reader grau
		private Color selNodeColor = Color.Blue;
		public  Color backGroundColor;
		public  Color lightBackGroundColor;
		private int   menuBarHeight;

		private Timer           hourGlassTimer = new Timer();
		private int             hourGlassTimerTicks;
		private Point           mousePosition;
		private Bitmap          bmHourGlass;
		private ContextMenu     contextMenu;
		private ImageAttributes imageAttrTransparent = new ImageAttributes();
		private Timer           doubleClickTimer = new Timer();
//		private bool ignoreMouseUp;
//		private int[] distX = new int[10];
//		private int[] distY = new int[10];
//		//private double[] timeStamp = new double[10];
		private double kineticDistX;
		private double kineticDistY;
		private int firstPtr;
		private int wrPtr;
		private const int kineticDepth = 4;
		private int[] lastX = new int[10];
		private int[] lastY = new int[10];
		private int[] lastTime = new int[10];
		private double kineticSpeedX;
		private double kineticSpeedY;
		private const int kineticInterval = 50;
		private const double kineticDamping = 0.707;
		private Timer kineticTimer = new Timer();

		private Settings settings;

		private Texts t;
		private int language;

		public UndoRedo undoRedo;

		public bool findValid = false;

//		public MyTreeView(Settings s, int languageParam, Texts texts, UndoRedo ur)
		public MyTreeView(Settings s, int languageParam, Texts texts, List<string> mapListParam)
		{
			settings = s;
			language = languageParam;
			t = texts;
			mapList = mapListParam;
//			undoRedo = ur;

			SetFont();

			PictureBox   = new PictureBox();
			imageList    = new ImageList();
			selectedNode = null;
			Nodes        = new MyTreeNodeCollection();

			myTreeViewEventArgs = new MyTreeViewEventArgs(this);

			PictureBox.Dock = DockStyle.Fill;  // damit die Größe schon mal stimmt
			PictureBox.MouseMove += new MouseEventHandler(Event_MouseMove);
			PictureBox.MouseDown += new MouseEventHandler(Event_MouseDown);
			PictureBox.MouseUp += new MouseEventHandler(Event_MouseUp);
			//PictureBox.Resize += new EventHandler(Event_Resize);

			canvasBitmap   = new Bitmap(10, 10, PixelFormat.Format16bppRgb565);
			canvasRect     = new Rectangle(0, 0, (int)(2.5*(float)Const.GetMaxScreenWidth()), (int)(2.5*(float)Const.GetMaxScreenWidth()));
			layoutRect     = canvasRect;
			cameraRect     = new Rectangle(-1, 0, (int)(scaling*PictureBox.Width), (int)(scaling*PictureBox.Height));
			if (Const.GetVGAFactor() == 1)  // QVGA
				navigatorMaxLength = 22;
			else
				navigatorMaxLength = 48;
			limitedWidth = (int)(1.2 * (float)Const.GetMaxScreenWidth());
			maxX         = Const.xMargin;
			maxY         = Const.yMargin;

			forceCenter     = true;
			fixSelectedNode = false;
//			ignoreMouseUp = true;
			mindmapLoaded = false;

			ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
			bmCollapsed = (Bitmap)resources.GetObject("Collapsed");
			bmHourGlass = (Bitmap)resources.GetObject("Butterfly");
			imageAttrTransparent.SetColorKey(Color.Transparent, Color.Transparent);

			if (Const.GetVGAFactor() == 1)
			{
//				menuBarHeight = 26;
				menuBarHeight = 32;
				pressedBackgroundPattern = new Bitmap(48, menuBarHeight, PixelFormat.Format32bppRgb);
				bitmapMenuBar = new Bitmap(100, menuBarHeight, PixelFormat.Format32bppRgb);
				button[0] = new MyButton((Bitmap)resources.GetObject("lupe-_22"), (Bitmap)resources.GetObject("lupe-_22gray"), true, PictureBox.Size, 0, nofButtons, menuBarHeight);
				button[1] = new MyButton((Bitmap)resources.GetObject("lupe_22"), (Bitmap)resources.GetObject("lupe_22gray"), true, PictureBox.Size, 1, nofButtons, menuBarHeight);
				button[2] = new MyButton((Bitmap)resources.GetObject("kmenuedit_22"), (Bitmap)resources.GetObject("kmenuedit_22"), true, PictureBox.Size, 2, nofButtons, menuBarHeight);
				button[3] = new MyButton((Bitmap)resources.GetObject("redo_22"), (Bitmap)resources.GetObject("redo_22gray"), false, PictureBox.Size, 3, nofButtons, menuBarHeight);
				button[4] = new MyButton((Bitmap)resources.GetObject("undo_22"), (Bitmap)resources.GetObject("undo_22gray"), false, PictureBox.Size, 4, nofButtons, menuBarHeight);
				button[5] = new MyButton((Bitmap)resources.GetObject("FindNext_22"), (Bitmap)resources.GetObject("FindNext_22gray"), false, PictureBox.Size, 5, nofButtons, menuBarHeight);
				button[6] = new MyButton((Bitmap)resources.GetObject("filesave_22"), (Bitmap)resources.GetObject("filesave_22gray"), false, PictureBox.Size, 6, nofButtons, menuBarHeight);
			}
			else
			{
//				menuBarHeight = 56;
				menuBarHeight = 64;
				pressedBackgroundPattern = new Bitmap(48, menuBarHeight, PixelFormat.Format32bppRgb);
				bitmapMenuBar = new Bitmap(100, menuBarHeight, PixelFormat.Format32bppRgb);
				button[0] = new MyButton((Bitmap)resources.GetObject("kmag-_48"), (Bitmap)resources.GetObject("kmag-_48gray"), true, PictureBox.Size, 0, nofButtons, menuBarHeight);
				button[1] = new MyButton((Bitmap)resources.GetObject("kmag_48"), (Bitmap)resources.GetObject("kmag_48gray"), true, PictureBox.Size, 1, nofButtons, menuBarHeight);
				button[2] = new MyButton((Bitmap)resources.GetObject("kmenuedit_48"), (Bitmap)resources.GetObject("kmenuedit_48"), true, PictureBox.Size, 2, nofButtons, menuBarHeight);
				button[3] = new MyButton((Bitmap)resources.GetObject("redo_48"), (Bitmap)resources.GetObject("redo_48gray"), false, PictureBox.Size, 3, nofButtons, menuBarHeight);
				button[4] = new MyButton((Bitmap)resources.GetObject("undo_48"), (Bitmap)resources.GetObject("undo_48gray"), false, PictureBox.Size, 4, nofButtons, menuBarHeight);
				button[5] = new MyButton((Bitmap)resources.GetObject("FindNext_48"), (Bitmap)resources.GetObject("FindNext_48gray"), false, PictureBox.Size, 5, nofButtons, menuBarHeight);
				button[6] = new MyButton((Bitmap)resources.GetObject("filesave_48"), (Bitmap)resources.GetObject("filesave_48gray"), false, PictureBox.Size, 6, nofButtons, menuBarHeight);
			}
			pressedBackground = new Bitmap(2, menuBarHeight, PixelFormat.Format32bppRgb);

			quickContext = new QuickContext();

			hourGlassTimer.Interval = 100;
			hourGlassTimer.Tick += new EventHandler(Event_HourGlassTimerTick);

			doubleClickTimer.Interval = 300;
			doubleClickTimer.Tick += new EventHandler(Event_DoubleClickTimerTick);

			kineticTimer.Interval = kineticInterval;
			kineticTimer.Tick += new EventHandler(Event_KineticTimerTick);

			int r = SystemColors.ActiveCaption.R;
			int g = SystemColors.ActiveCaption.G;
			int b = SystemColors.ActiveCaption.B;
			int newR;
			int newG;
			int newB;

			// R5G6B5-Farben, also kann man die 3 LSBs getrost auf 0 lassen
			Graphics gr = Graphics.FromImage(bitmapMenuBar);
			Graphics gp  = Graphics.FromImage(pressedBackgroundPattern);
			int i;

			// oben 1-2 Pixel etwas dunkler (gesehen beim iPhone)
			for (i = 0; i < Const.GetVGAFactor(); i++)
			{
				gr.DrawLine(new Pen(Color.FromArgb(0x40, 0x40, 0x40)), 0, i, bitmapMenuBar.Width-1, i);
				gp.DrawLine(new Pen(Color.FromArgb(0x40, 0x40, 0x40)), 0, i, pressedBackgroundPattern.Width-1, i);
			}

			// dann Farbverlauf bis zur Hälfte
			//          y2 - y1
			// y = y1 + -------(x - x1)
			//          x2 - x1
			//
			//           lC - uC
			// aC = uC + -------(i - Breite des oberen Streifens)
			//           Breite
			int upperColor = 0x68;
			int upperColorPressed = 0x90;
			int actualColor = upperColor;
			int actualColorPressed = upperColorPressed;
			for (; i < menuBarHeight/2; i++)
			{
				//actualColor = upperColor + (int)( (float)(lowerColor-upperColor)/(float)(menuBarHeight/2-Const.GetVGAFactor()) * (float)(i-Const.GetVGAFactor()));
				gr.DrawLine(new Pen(Color.FromArgb(actualColor, actualColor, actualColor)), 0, i, bitmapMenuBar.Width-1, i);
				actualColor -= 4/Const.GetVGAFactor();
				gp.DrawLine(new Pen(Color.FromArgb(actualColorPressed, actualColorPressed, actualColorPressed)), 0, i, pressedBackgroundPattern.Width-1, i);
				actualColorPressed -= 4/Const.GetVGAFactor();
			}

			// und schließlich konstant fast schwarz, beim pressedBackground heller
			for (; i < menuBarHeight-Const.GetVGAFactor(); i++)
			{
				gr.DrawLine(new Pen(Color.FromArgb(0x08, 0x08, 0x08)), 0, i, bitmapMenuBar.Width-1, i);
				gp.DrawLine(new Pen(Color.FromArgb(0x30, 0x30, 0x30)), 0, i, pressedBackgroundPattern.Width-1, i);
			}

			// beim pressedBackground zum Schluss auch
			for (; i < menuBarHeight; i++)
			{
				gr.DrawLine(new Pen(Color.FromArgb(0x08, 0x08, 0x08)), 0, i, bitmapMenuBar.Width-1, i);
				gp.DrawLine(new Pen(Color.FromArgb(0x08, 0x08, 0x08)), 0, i, pressedBackgroundPattern.Width-1, i);
			}

			gr.Dispose();
			gp.Dispose();

			float p = 65f;
			newR = (int)((float)(255-r)*p/100f) + r;
			newG = (int)((float)(255-g)*p/100f) + g;
			newB = (int)((float)(255-b)*p/100f) + b;
			backGroundColor = Color.FromArgb(newR, newG, newB);

			p = 95f;
			newR = (int)((float)(255-r)*p/100f) + r;
			newG = (int)((float)(255-g)*p/100f) + g;
			newB = (int)((float)(255-b)*p/100f) + b;
			lightBackGroundColor = Color.FromArgb(newR, newG, newB);
		}

		public event EventHandler DoubleClick;
		public event MyTreeViewEventHandler AfterCollapse;
		public event MyTreeViewEventHandler AfterExpand;
		public event MyTreeViewEventHandler ShowMainMenu;
		public event MyTreeViewEventHandler Redo;
		public event MyTreeViewEventHandler Undo;
		public event MyTreeViewEventHandler FindNext;  // Find Next (vorher Note)
		public event MyTreeViewEventHandler SaveFile;
		//public event EventHandler Resize;
		public event MyTreeViewEventHandler NotesHandler;
		public event MyTreeViewEventHandler IconsHandler;
		public event MyTreeViewEventHandler FormatHandler;
		public event MyTreeViewEventHandler MoveUpHandler;
		public event MyTreeViewEventHandler MoveDownHandler;
		public event MyTreeViewEventHandler SisterHandler;
		public event MyTreeViewEventHandler BrotherHandler;
		public event MyTreeViewEventHandler ChildHandler;
		public event MyTreeViewEventHandler NewParentHandler;

		protected virtual void OnDoubleClick(EventArgs e)
		{
			EventHandler handler = DoubleClick;
			if (handler != null)
				handler(this, e);
		}

		protected virtual void OnAfterCollapse(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = AfterCollapse;
			if (handler != null)
				handler(this, e);
		}

		protected virtual void OnAfterExpand(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = AfterExpand;
			if (handler != null)
				handler(this, e);
		}

		protected virtual void OnShowMainMenu(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = ShowMainMenu;
			if (handler != null)
				handler(this, e);
		}

		protected virtual void OnRedo(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = Redo;
			if (handler != null)
				handler(this, e);
		}

		protected virtual void OnUndo(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = Undo;
			if (handler != null)
				handler(this, e);
		}

		protected virtual void OnFindNext(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = FindNext;
			if (handler != null)
				handler(this, e);
		}

		protected virtual void OnSaveFile(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = SaveFile;
			if (handler != null)
				handler(this, e);
		}

//		protected virtual void OnResize(EventArgs e)
//		{
//			EventHandler handler = Resize;
//			if (handler != null)
//				handler(this, e);
//		}

		protected virtual void OnNotes(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = NotesHandler;
			if (handler != null)
				handler(this, e);
		}

		protected virtual void OnIcons(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = IconsHandler;
			if (handler != null)
				handler(this, e);
		}

		protected virtual void OnFormat(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = FormatHandler;
			if (handler != null)
				handler(this, e);
		}

		protected virtual void OnMoveUp(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = MoveUpHandler;
			if (handler != null)
				handler(this, e);
		}

		protected virtual void OnMoveDown(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = MoveDownHandler;
			if (handler != null)
				handler(this, e);
		}

		protected virtual void OnSister(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = SisterHandler;
			if (handler != null)
				handler(this, e);
		}

		protected virtual void OnBrother(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = BrotherHandler;
			if (handler != null)
				handler(this, e);
		}

		protected virtual void OnChild(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = ChildHandler;
			if (handler != null)
				handler(this, e);
		}

		protected virtual void OnNewParent(MyTreeViewEventArgs e)
		{
			MyTreeViewEventHandler handler = NewParentHandler;
			if (handler != null)
				handler(this, e);
		}

		public void SetFont()
		{
			int[] privFontSizeFirst = new int[5];
			privFontSizeFirst[0] = 15;
			privFontSizeFirst[1] = 14;
			privFontSizeFirst[2] = 13;
			privFontSizeFirst[3] = 12;
			privFontSizeFirst[4] = 11;

			int[] privFontSizeSecond  = new int[5];
			privFontSizeSecond[0] = 11;
			privFontSizeSecond[1] = 10;
			privFontSizeSecond[2] = 9;
			privFontSizeSecond[3] = 8;
			privFontSizeSecond[4] = 7;

			int[] privFontSizeRegular = new int[5];
			privFontSizeRegular[0] = 11;
			privFontSizeRegular[1] = 10;
			privFontSizeRegular[2] = 9;
			privFontSizeRegular[3] = 8;
			privFontSizeRegular[4] = 7;

			fontFirst   = new Font("Arial", privFontSizeFirst[settings.FontSize],   FontStyle.Bold);
#if GRAPHICAL
			fontSecond  = new Font("Arial", privFontSizeSecond[settings.FontSize],  FontStyle.Regular);
#else
			fontSecond  = new Font("Arial", privFontSizeSecond[settings.FontSize],  FontStyle.Bold);
#endif
			fontRegular = new Font("Arial", privFontSizeRegular[settings.FontSize], FontStyle.Regular);
		}

		public Font GetFont(int level)
		{
			switch (level)
			{
			case 0:
				return(fontFirst);
			case 1:
				return(fontSecond);
			case 2:
			default:
				return(fontRegular);
			}
		}

		public void ExpandAll()
		{
			MyTreeNode node = Nodes[0];

			do
			{
				selectedNode = node;  // damit OnAfterExpand darauf zugreifen kann
				node.Expand();
				OnAfterExpand(new MyTreeViewEventArgs(this));
				node = GetNextNode(node, true);
			} while (node != null);
		}

		public void CollapseAll()
		{
			MyTreeNode node = Nodes[0];

			while ((node = GetNextNode(node, true)) != null)  // der Root-Node soll nicht collapsed werden
			{
				if (!node.expanded) continue;         // node ist schon collapsed
				if (node.Nodes.Count == 0) continue;  // node hat keine Children

				selectedNode = node;  // damit OnAfterCollapse darauf zugreifen kann
				node.Collapse();
				OnAfterCollapse(new MyTreeViewEventArgs(this));
			};
		}

		public void Expand(MyTreeNode node)
		{
			node.Expand();
			OnAfterExpand(new MyTreeViewEventArgs(this));
		}

		public void Collapse(MyTreeNode node)
		{
			node.Collapse();
			OnAfterCollapse(new MyTreeViewEventArgs(this));
		}

		private int EffectiveHeight()
		{
			return(PictureBox.Image.Height - bitmapMenuBar.Height);
		}

		private void DisplayButtons(Graphics gr, Bitmap pressedBackground)
		{
			int x = 0;
			int y = EffectiveHeight();
#if DEBUG
			int screenWidth = PictureBox.Width;
#else
			int screenWidth = Screen.PrimaryScreen.Bounds.Width;
#endif
			button[3].enabled = undoRedo.CanRedo;
			button[4].enabled = undoRedo.CanUndo;
			button[5].enabled = !(selectedNode == null) && findValid;  // Find Next (vorher Note)

			while (x < PictureBox.Image.Width)
			{
				gr.DrawImage(bitmapMenuBar, x, y);
				x += bitmapMenuBar.Width;
			}

			if (mindmapLoaded)  // Zoom nur wenn Mindmap geladen ist
			{
				for (int i = 0; i < 2; i++)
				{
//					button[i].RePosition(PictureBox.Image.Size);
					button[i].RePosition(new Size(screenWidth, PictureBox.Image.Height));
					if (!button[i].enabled)
						gr.DrawImage(button[i].grayBitmap,
						             button[i].rectangle,
						             0, 0,
						             button[i].rectangle.Width, button[i].rectangle.Height,
						             GraphicsUnit.Pixel,
						             imageAttrTransparent);
					else
					{
						gr.DrawImage(button[i].normalBitmap,
						             button[i].rectangle,
						             0, 0,
						             button[i].rectangle.Width, button[i].rectangle.Height,
						             GraphicsUnit.Pixel,
						             imageAttrTransparent);
						if (button[i].pressed)
							gr.DrawRectangle(new Pen(Color.Blue), button[i].rectangle);
					}
				}
			}

			button[2].RePosition(new Size(screenWidth, PictureBox.Image.Height));
			int relX = (button[2].rectangle.Width - button[2].normalBitmap.Width) / 2;
			int relY = (menuBarHeight - button[2].normalBitmap.Width) / 2;

			for (int i = 2; i < nofButtons; i++)
			{
				button[i].RePosition(new Size(screenWidth, PictureBox.Image.Height));
				if (!button[i].enabled)
					gr.DrawImage(button[i].grayBitmap,
					             new Rectangle(button[i].rectangle.X+relX, button[i].rectangle.Y+relY, button[i].normalBitmap.Width, button[i].normalBitmap.Height),
					             0, 0,
					             button[i].normalBitmap.Width, button[i].normalBitmap.Height,
					             GraphicsUnit.Pixel,
					             imageAttrTransparent);
				else
				{
					if (button[i].pressed)
					{
						if (pressedBackground.Width != button[i].rectangle.Width)
						{
							pressedBackground = new Bitmap(button[i].rectangle.Width, menuBarHeight, PixelFormat.Format32bppRgb);
							int j = 0;
							Graphics gp = Graphics.FromImage(pressedBackground);
							do
							{
								gp.DrawImage(pressedBackgroundPattern, j, 0);
								j += pressedBackgroundPattern.Width;
							} while (j < button[i].rectangle.Width);
							gp.Dispose();
						}
						gr.DrawImage(pressedBackground, button[i].rectangle.X, button[i].rectangle.Y);
					}
					gr.DrawImage(button[i].normalBitmap,
					             new Rectangle(button[i].rectangle.X+relX, button[i].rectangle.Y+relY, button[i].normalBitmap.Width, button[i].normalBitmap.Height),
					             0, 0,
					             button[i].normalBitmap.Width, button[i].normalBitmap.Height,
					             GraphicsUnit.Pixel,
					             imageAttrTransparent);
				}
			}

			if (mindmapLoaded && settings.Navigator)  // Navigator bei Bedarf anzeigen
			{
				//int offsetX = PictureBox.Image.Width  - 24*Const.GetVGAFactor() - 2;
				//int offsetY = PictureBox.Image.Height - 24*Const.GetVGAFactor() - 2;
				int offsetX = 3*Const.GetVGAFactor();
				//int offsetY = 3*Const.GetVGAFactor();
				int offsetY = PictureBox.Image.Height - navigatorLayoutRect.Height - 4*Const.GetVGAFactor() - menuBarHeight;
				navigatorLayoutRect.X = offsetX;
				navigatorLayoutRect.Y = offsetY;
				//gr.FillRectangle(new SolidBrush(backGroundColor), navigatorLayoutRect);
				gr.DrawRectangle(new Pen(Color.FromArgb(51, 51, 51), (float)Const.GetVGAFactor()), navigatorLayoutRect);
				//gr.DrawRectangle(new Pen(Color.Gray), navigatorLayoutRect);
				navigatorCameraRect = new Rectangle((int)((float)cameraRect.X/navigatorScale), (int)((float)cameraRect.Y/navigatorScale), (int)Math.Ceiling((float)cameraRect.Width/navigatorScale), (int)Math.Ceiling((float)cameraRect.Height/navigatorScale));
				navigatorCameraRect.Width = Math.Max(navigatorCameraRect.Width, Const.GetVGAFactor());
				navigatorCameraRect.Height = Math.Max(navigatorCameraRect.Height, Const.GetVGAFactor());
				navigatorCameraRect.Offset(offsetX, offsetY);
				gr.DrawRectangle(new Pen(Color.FromArgb(51, 51, 51)), navigatorCameraRect);
				//gr.FillRectangle(new SolidBrush(Color.White), navigatorCameraRect);
			}
		}

		public void FileSaveButtonEnabled(bool ena, bool display)
		{
			button[6].enabled = ena;
			if (display)
				DisplayCamera();
		}

		private bool ButtonDown(System.Windows.Forms.MouseEventArgs e)
		{
			for (int i = 0; i < nofButtons; i++)
			{
				if (button[i].rectangle.Contains(e.X, e.Y))
				{
					if (buttonPressed >= 0)  // dürfte nicht vorkommen
						button[buttonPressed].pressed = false;
					button[i].pressed = true;
					buttonPressed = i;
					DisplayCamera();
					return(true);
				}
			}

			return(false);
		}

		private bool ButtonMove(System.Windows.Forms.MouseEventArgs e)
		{
			if (buttonPressed == -1)
				return(false);
			if (buttonPressed == -2)
				return(true);

			if (!button[buttonPressed].rectangle.Contains(e.X, e.Y))
			{
				button[buttonPressed].pressed = false;
				buttonPressed = -2;
				DisplayCamera();
			}

			return(true);
		}

		private bool ButtonUp(System.Windows.Forms.MouseEventArgs e)
		{
			double oldScaling = scaling;

			if (buttonPressed == -1)
				return(false);
			if (buttonPressed == -2)
			{
				buttonPressed = -1;
				return(true);
			}

			if (button[buttonPressed].rectangle.Contains(e.X, e.Y) &&
			    button[buttonPressed].enabled)
			{
				// Aktion auslösen
				switch(buttonPressed)
				{
					case 0:
						if (scaling < 1.33)
							scaling /= 0.9;
						break;
					case 1:
						if (scaling > 0.75)
							scaling *= 0.9;
						break;
					case 2:
						OnShowMainMenu(new MyTreeViewEventArgs(this));
						break;
					case 3:
						OnRedo(new MyTreeViewEventArgs(this));
						break;
					case 4:
						OnUndo(new MyTreeViewEventArgs(this));
						break;
					case 5:
						OnFindNext(new MyTreeViewEventArgs(this));
						break;
					case 6:
						OnSaveFile(new MyTreeViewEventArgs(this));
						break;
					default:
						break;
				}

				button[0].enabled = true;
				button[1].enabled = true;
				if (scaling > 1.33)
					button[0].enabled = false;
				if (scaling < 0.75)
					button[1].enabled = false;

				if (buttonPressed <= 1)
				{
					double oldWidth  = (double)cameraRect.Width;
					double oldHeight = (double)cameraRect.Height;
					cameraRect.X -= (int)(oldWidth/2.0*(scaling-oldScaling));
					cameraRect.Y -= (int)(oldHeight/2.0*(scaling-oldScaling));
					cameraRect.Width  = (int)(scaling*PictureBox.Width);
					cameraRect.Height = (int)(scaling*PictureBox.Height);
					ValidateCameraRect(false);
				}
			}
			button[buttonPressed].pressed = false;
			buttonPressed = -1;
			DisplayCamera();
			return(true);
		}

		public void EnsureVisible(MyTreeNode node, bool forceUpdate)
		{
			if (fixSelectedNode)
			{
				fixSelectedNode = false;
				cameraRect.X = selectedNode.TextBox.X - selNodeRelativeX;
				cameraRect.Y = selectedNode.TextBox.Y - selNodeRelativeY;
			}
			if ((!cameraRect.IntersectsWith(node.TextBox)) || forceCenter)
			{
				forceCenter  = false;
				cameraRect.X = node.TextBox.X - Const.xMargin;
				cameraRect.Y = node.TextBox.Y + node.TextBox.Height/2 - EffectiveHeight()/2;
			}
			ValidateCameraRect(forceUpdate);
		}

		private void ValidateCameraRect(bool forceUpdate)
		{
			cameraRect.X = Math.Max(cameraRect.X, 0);
			cameraRect.Y = Math.Max(cameraRect.Y, 0);
			cameraRect.X = Math.Min(cameraRect.X, layoutRect.Width-cameraRect.Width);
			cameraRect.Y = Math.Min(cameraRect.Y, layoutRect.Height-cameraRect.Height);

			if ((cameraRect.Left   < canvasRect.Left) ||  // wenn cameraRect (teilweise) außerhalb von canvasRect liegt
			    (cameraRect.Top    < canvasRect.Top) ||
			    (cameraRect.Bottom > canvasRect.Bottom) ||
			    (cameraRect.Right  > canvasRect.Right) ||
			    (forceUpdate))
			{
				CenterCanvas();
			}

			DisplayCamera();
		}

		private void CenterCanvas()
		{
			canvasRect.X = cameraRect.X + (cameraRect.Width  - canvasRect.Width)  / 2;   // canvasRect um cameraRect zentrieren
			canvasRect.Y = cameraRect.Y + (cameraRect.Height - canvasRect.Height) / 2;

			canvasRect.X = Math.Max(canvasRect.X, 0);  // canvasRect validieren
			canvasRect.Y = Math.Max(canvasRect.Y, 0);
			canvasRect.X = Math.Min(canvasRect.X, layoutRect.Width-canvasRect.Width);
			canvasRect.Y = Math.Min(canvasRect.Y, layoutRect.Height-canvasRect.Height);

			DrawCanvas();  // canvas neu zeichnen
		}

		public void DrawCanvas()
		{
			canvasBitmap.Dispose();
			try
			{
				canvasBitmap = new Bitmap(canvasRect.Width, canvasRect.Height, PixelFormat.Format16bppRgb565);
			}
			catch
			{
				canvasBitmap = new Bitmap(1, 1, PixelFormat.Format16bppRgb565);
				MessageBox.Show(t.t[t.NotEnoughMemory, language],
				                t.t[t.Warning, language],
				                MessageBoxButtons.OK,
				                MessageBoxIcon.Exclamation,
				                MessageBoxDefaultButton.Button1);
				Application.Exit();
				return;
			}

			Graphics gr = Graphics.FromImage(canvasBitmap);
			gr.Clear(Color.White);

			MyTreeNode node = Nodes[0];

			int offsetX = canvasRect.X;
			int offsetY = canvasRect.Y;

			int tempY;
			Point[] point = new Point[4];

			int iconHeight = 16;
			int iconWidth  = 16;
			if (settings.FontSize < 4)  // also größer als extra small
			{
				iconHeight *= Const.GetVGAFactor();
				iconWidth  *= Const.GetVGAFactor();
			}

			Rectangle destRect;
			Rectangle srcRect;

			do  // Knoten und Verbindungen zeichnen
			{
				// Muss komischerweise immer berechnet werden, auch wenn der Knoten
				// dann nicht gezeichnet wird (gibt sonst senkrechte "Geisterlinien").
				// Ist eigentlich klar: ein Parent liegt außerhalb, wird nicht berechnet,
				// muss dann aber verbunden werden (falsche Koordinaten 0|0 + offset).
				// Das Ganze gehört ja eigentlich nach Layout().
				node.Point1 = new Point(node.TextBoxX-node.iconIndexList.Count*(iconWidth+Const.iconMargin), node.TextBoxY+node.TextBox.Height+1);
				if (node.Parent != null)
					node.Point1X -= Const.leftMargin;
				node.Point2 = new Point(node.TextBoxX + node.TextBox.Width, node.Point1.Y);

				node.ExpanderX = node.TextBox.X + node.TextBox.Width;
				node.ExpanderY = node.TextBox.Y;

				if (!node.TextBox.IntersectsWith(canvasRect))  // Knoten ist außerhalb
				{
					if (node.Parent != null)  // es gibt einen Parent
					{
						if (!node.Parent.TextBox.IntersectsWith(canvasRect))  // Parent ist außerhalb
						{
							if ((node.Point1.X >= canvasRect.Left) &&  // der Knoten liegt in der Nord-/Südverlängerung
							    (node.Point1.X <= canvasRect.Right))
							{
								if (node.Point1.Y >= canvasRect.Top)  // Knoten liegt oberhalb
								{
									if (node.Parent.Point1.Y >= canvasRect.Top)  // Parent liegt oberhalb
									{
										node = GetNextNode(node, node.expanded);  // den nächsten Knoten holen
									    continue;
									}
								}
								else  // Knoten liegt unterhalb
								{
									if (node.Parent.Point1.Y <= canvasRect.Bottom)  // Parent liegt unterhalb
									{
										node = GetNextNode(node, node.expanded);  // den nächsten Knoten holen
									    continue;
									}
								}
							}
							else  // der Knoten liegt links bzw. rechts außerhalb
							{
								node = GetNextNode(node, node.expanded);  // den nächsten Knoten holen
							    continue;
							}
						}
						else  // Parent ist innerhalb
						{
							// Knoten ist außerhalb, aber der Parent ist innerhalb, also zeichnen
							// fall-thru zum Bearbeiten des Knotens
						}
					}
					else  // es gibt keinen Parent
					{
						node = GetNextNode(node, node.expanded);  // den nächsten Knoten holen
						continue;
					}
				}

				// Hintergrund des Knotens einfärben
				gr.FillRectangle(new SolidBrush(node.BackColor), node.TextBox.X-offsetX, node.TextBox.Y-offsetY, node.TextBox.Width, node.TextBox.Height);

				// Knotentext und Icons unterstreichen
				gr.DrawLine(new Pen(lineColor), node.Point1.X-offsetX, node.Point1.Y-offsetY, node.Point2.X-offsetX, node.Point2.Y-offsetY);

				// Verbindungslinie zum Elternknoten zeichnen
				if (node.Parent != null)
				{
					if (node.Point1.Y > node.Parent.Point1.Y)
						tempY = -Const.chamfer;
					else if (node.Point1.Y < node.Parent.Point1.Y)
						tempY = Const.chamfer;
					else
						tempY = 0;

					point[0] = node.Point1;
					point[1] = new Point(node.Point1.X-Const.chamfer, node.Point1.Y+tempY);
					point[2] = new Point(node.Point1.X-Const.chamfer, node.Parent.Point2.Y);
					point[3] = node.Parent.Point2;
					for (int i = 0; i < 4; i++)
						point[i].Offset(-offsetX, -offsetY);

					gr.DrawLines(new Pen(lineColor, 2), point);
				}

				// Knotentext schreiben
				gr.DrawString(node.Text, GetFont(node.Level), new SolidBrush(node.ForeColor), node.TextBoxX+Const.textBoxOversize/2-offsetX, node.TextBoxY-offsetY);
				//gr.DrawEllipse(new Pen(Color.Black), node.TextBox.X-offsetX, node.TextBox.Y-offsetY, node.TextBox.Width, node.TextBox.Height);

				// Icons malen
				for (int i = 0; i < node.iconIndexList.Count; i++)
				{
					destRect = new Rectangle(node.Point1.X+i*(iconWidth+Const.iconMargin)-offsetX,
					                         node.Point1.Y-iconHeight-offsetY,
					                         iconWidth,
					                         iconHeight);
					if (node.Parent != null)
						destRect.X += Const.leftMargin;

					srcRect = new Rectangle(0, 0, 16, 16);

					gr.DrawImage(imageList.Images[node.iconIndexList[i]],
					             destRect, srcRect, GraphicsUnit.Pixel);
				}

				// Expander zeichnen
				// TODO: größerer Expander für VGA
				if (!node.expanded)
					gr.DrawImage(bmCollapsed, node.Point2.X-offsetX, node.Point2.Y-4-offsetY);

				node = GetNextNode(node, node.expanded);
			} while (node != null);

			// Selektierten Knoten umranden
			gr.DrawRectangle(new Pen(selNodeColor),
			                 selectedNode.TextBox.X-offsetX,
			                 selectedNode.TextBox.Y-offsetY,
			                 selectedNode.TextBox.Width,
			                 selectedNode.TextBox.Height);

			// bei der Demoversion einen Bereich übermalen
			if (!settings.Unlocked)
			{
				string demoMessage = t.t[t.PaintedOver, language];
				Font   messageFont = new Font("Arial", 10, FontStyle.Bold);
				int    startX = limitedWidth - canvasRect.Left;
				int    startLine = startX;
				int    y = (canvasRect.Top & ~(8*Const.GetVGAFactor()-1)) - canvasRect.Top;
				int    offset = Const.GetVGAFactor() * 5;

				if (startX < canvasRect.Width)
				{
					if (startLine < 0)
						startLine = 0;

					while(y < canvasRect.Height)
					{
						gr.DrawLine(new Pen(Color.LightGray, 4f*Const.GetVGAFactor()), startLine, y, canvasRect.Width, y);
						y += 8*Const.GetVGAFactor();
					};

					y = (canvasRect.Top & ~(128*Const.GetVGAFactor()-1)) - canvasRect.Top;
					while(y < canvasRect.Height)
					{
						gr.DrawString(demoMessage, messageFont, new SolidBrush(Color.Red), startX+offset, y);
						y += 128*Const.GetVGAFactor();
					};
				}
			}

			gr.Dispose();
		}

		public void DisplayCamera()
		{
			Graphics g = Graphics.FromImage(PictureBox.Image);
			// für scaling
			Rectangle destRect = new Rectangle(0, 0, PictureBox.Image.Width, PictureBox.Image.Height);
			Rectangle srcRect = cameraRect;
			srcRect.X -= canvasRect.X;
			srcRect.Y -= canvasRect.Y;
			g.DrawImage(canvasBitmap, destRect, srcRect, GraphicsUnit.Pixel);
			DisplayButtons(g, pressedBackground);

			g.Dispose();
			PictureBox.Refresh();
		}

		public void Layout()
		{
			int iconWidth  = 16;
			if (settings.FontSize < 4)  // also größer als extra small
				iconWidth *= Const.GetVGAFactor();

			MyTreeNode node = Nodes[0];

			maxX = Const.xMargin;
			maxY = Const.yMargin;

			Nodes[0].TextBoxX = maxX;
			Nodes[0].TextBoxX += Nodes[0].iconIndexList.Count*(iconWidth + Const.iconMargin);
			Nodes[0].NextX = Nodes[0].TextBoxX + Nodes[0].TextBox.Width + Const.xSpacing;
			Nodes[0].TextBoxY = maxY;

//			if (Nodes[0].Nodes.Count == 0)  // keine Children, Layout fertig
//				return;

			node = GetNextNode(node, node.expanded);

			do
			{
				if (node == null)
					break;

				if (node.Index == 0)
				{
					node.TextBoxY = node.Parent.TextBoxY;  // auf gleicher Höhe wie Parent
				}
				else
				{
					node.TextBoxY = node.Parent.Nodes[node.Index-1].nextYInclSubNodes;  // unter dem Vorgänger
				}
	
				node.nextYInclSubNodes = node.TextBoxY + node.TextBox.Height + Const.ySpacing;
				maxY = Math.Max(maxY, node.nextYInclSubNodes);
	
				MyTreeNode tempNode = node.Parent;  // Parent des Aktuellen
				do  // rekursiv nextYInclSubNodes updaten
				{
					if (tempNode.nextYInclSubNodes < node.nextYInclSubNodes)
						tempNode.nextYInclSubNodes = node.nextYInclSubNodes;

					tempNode = tempNode.Parent;
				} while (tempNode != null);
	
				int newYPos;

				tempNode = node;
				while (tempNode.Parent != null)  // alle Parents zentrieren
				{
					newYPos  = tempNode.Parent.Nodes[0].TextBox.Y;
					newYPos += tempNode.TextBox.Y + tempNode.TextBox.Height;
					newYPos /= 2;
					newYPos -= tempNode.Parent.TextBox.Height/2;
					if (newYPos > tempNode.Parent.TextBox.Y)  // nur nach unten schieben erlauben
						tempNode.Parent.TextBoxY = newYPos;

					tempNode = tempNode.Parent;
				};

				node.TextBoxX = node.Parent.NextX + Const.leftMargin;
				node.TextBoxX += node.iconIndexList.Count*(iconWidth + Const.iconMargin);
				node.NextX = node.TextBoxX + node.TextBox.Width + Const.xSpacing;
				maxX = Math.Max(node.NextX, maxX);

				node = GetNextNode(node, node.expanded);
			} while (node != null);

			// PR0009: Der unterste Knoten ist zu weit unten, insbesondere bei VGA schon in den Icons
			// wird seit dem neuen Konzept hier abgehandelt
			layoutRect.Width  = Math.Max(maxX, canvasRect.Width);
			layoutRect.Height = Math.Max(maxY+Const.GetVGAFactor()*Const.yMargin*2, canvasRect.Height);

			navigatorScale = (float)Math.Max(layoutRect.Width, layoutRect.Height) / (float)navigatorMaxLength;
			navigatorLayoutRect = new Rectangle(0, 0, (int)Math.Ceiling((float)layoutRect.Width/navigatorScale), (int)Math.Ceiling((float)layoutRect.Height/navigatorScale));
		}

		public MyTreeNode AddNode(string str, Rectangle boxParam, long createdParam, long modifiedParam, Color fc, Color bc)
		{
			Nodes.Add(new MyTreeNode(str, fc, bc));

			MyTreeNode newNode = Nodes[Nodes.Count-1];
			newNode.Index = Nodes.Count-1;
			newNode.Level = 0;
			newNode.TextBox = boxParam;
			newNode.TextBoxX = Const.xMargin;
			newNode.TextBoxY = maxY;
			newNode.NextX = newNode.TextBox.X + newNode.TextBox.Width + Const.xSpacing;
			newNode.nextYInclSubNodes = newNode.TextBoxY + newNode.TextBox.Height + Const.ySpacing;
			maxY += newNode.nextYInclSubNodes;
			newNode.created = createdParam;
			newNode.modified = modifiedParam;

			selectedNode = newNode;

			return(newNode);
		}

		private void Event_KineticTimerTick(object sender, EventArgs e)
		{
			cameraRect.X += (int)kineticDistX;
			cameraRect.Y += (int)kineticDistY;

			ValidateCameraRect(false);

			kineticDistX *= kineticDamping;
			kineticDistY *= kineticDamping;
			if ((Math.Abs(kineticDistX) < 1.0) && (Math.Abs(kineticDistY) < 1.0))
			{
				kineticTimer.Enabled = false;
				ValidateCameraRect(true);
			}
		}

		private void Event_DoubleClickTimerTick(object sender, EventArgs e)
		{
			Graphics g = Graphics.FromImage(PictureBox.Image);  // create graphics
			bool newShowDate = showDate;
			Rectangle r = new Rectangle(0, 0, 0, 0);

			doubleClickTimer.Enabled = false;

			if ((showDate) && settings.NodeInfo && !hourGlassTimer.Enabled)
			{
				DateTime date;
				string strC = "C: ";
				string strM = "M: ";

				newShowDate = false;

				long longDate = selectedNode.created;  // creation date (UTC)
				if (longDate < 1000)
					strC += "---";
				else
				{
					date = new DateTime(1970, 1, 1);  // based on 01.01.1970
					date = date.AddMilliseconds((double)longDate).ToLocalTime();
					strC += date.ToString();
				}

				longDate = selectedNode.modified;  // modification date (UTC)
				if (longDate < 1000)
					strM += "---";
				else
				{
					date = new DateTime(1970, 1, 1);   // based on 01.01.1970
					date = date.AddMilliseconds((double)longDate).ToLocalTime();
					strM += date.ToString();
				}

				SizeF sizeC = g.MeasureString(strC, GetFont(2));  // measure strings
				SizeF sizeM = g.MeasureString(strM, GetFont(2));
				int width = (int)Math.Max(sizeC.Width, sizeM.Width);
				int height = (int)sizeC.Height;

				r = new Rectangle((int)((selectedNode.TextBoxX-cameraRect.X)/scaling),
				                            (int)((selectedNode.TextBoxY-cameraRect.Y)/scaling),
				                            width+Const.textBoxOversize, 2*height);
				r.Y -= r.Height;  // set on top of textbox

				int h = r.Right + 4 - PictureBox.Width;  // validate right edge
				if (h > 0) r.X -= h;

				r.X = Math.Max(r.X, 4);  // validate left edge

				h = r.Top - 4;  // validate top edge
				if (h < 0) r.Y -= h;

				if (settings.QuickContext)
				{
					if (settings.Navigator)
					{
						r.X = navigatorLayoutRect.X + navigatorLayoutRect.Width + Const.GetVGAFactor()*4;
						r.Y = navigatorLayoutRect.Bottom - r.Height;
					}
					else
					{
						r.X = Const.GetVGAFactor()*4;
						r.Y = PictureBox.Image.Height - Const.GetVGAFactor()*4 - menuBarHeight - r.Height;
					}
				}

				g.FillRectangle(new SolidBrush(Color.FromArgb(221, 245, 255)), r);  // background
				g.DrawRectangle(new Pen(Color.Black), r);                           // border
				h = Const.GetVGAFactor() * 2;  // helper
				g.DrawString(strC, GetFont(2), new SolidBrush(Color.Black), r.X + h, r.Y + h);  // write strings
				g.DrawString(strM, GetFont(2), new SolidBrush(Color.Black), r.X + h, r.Y + height);
			}

			if ((showDate) && !hourGlassTimer.Enabled)
			{
				newShowDate = false;

				if (settings.QuickContext)
				{
					// immer enabled
					quickContext.buttonEnabled[Notes]  = 1;
					quickContext.buttonEnabled[Icons]  = 1;
					quickContext.buttonEnabled[Format] = 1;

					// per default enabled
					quickContext.buttonEnabled[Sister]    = 1;
				    quickContext.buttonEnabled[Child]     = 1;
				    quickContext.buttonEnabled[NewParent] = 1;
				    quickContext.buttonEnabled[Brother]   = 1;
				    if (Parse.IsEncrypted(mapList, (int)SelectedNode.Tag))
					{
					    quickContext.buttonEnabled[Child] = 0;  // nicht erlaubt wenn der Knoten verschlüsselt ist
					}
					if (Parse.ThisIsTheRoot(mapList, (int)SelectedNode.Tag))
					{
					    quickContext.buttonEnabled[Sister]    = 0;  // diese sind bei der Root nicht möglich
					    quickContext.buttonEnabled[NewParent] = 0;
					    quickContext.buttonEnabled[Brother]   = 0;
					}

					// per default disabled
					quickContext.buttonEnabled[MoveUp]   = 0;
					quickContext.buttonEnabled[MoveDown] = 0;
					if (SelectedNode.Parent != null)
					{
						if (SelectedNode.Parent.GetNodeCount(false) > 1)
						{
							// nur enabled, wenn es min. einen Geschwisterknoten gibt
							quickContext.buttonEnabled[MoveUp]   = 1;
							quickContext.buttonEnabled[MoveDown] = 1;
						}
					}

					// effektive Höhe zum Zentrieren
					int bottom = PictureBox.Image.Height - menuBarHeight;
					if (settings.Navigator)
						bottom = navigatorLayoutRect.Y;
					if (settings.NodeInfo)
						bottom = Math.Min(bottom, r.Y);

					Bitmap quickContextBmp = quickContext.Display(canvasBitmap, cameraRect.X-canvasRect.X, cameraRect.Y-canvasRect.Y, bottom);
					g.DrawImage(quickContextBmp, quickContext.outerRectangle.X, quickContext.outerRectangle.Y);
					//g.DrawRectangle(new Pen(Color.Black), quickContext.outerRectangle);
				}
				else
					quickContext.visible = false;
			}

			showDate = newShowDate;

			PictureBox.Refresh();  // refresh visual port

			g.Dispose();  // dispose graphics
		}

		private void Event_HourGlassTimerTick(object sender, EventArgs e)
		{
			if (++hourGlassTimerTicks < 3) return;

			Graphics g = Graphics.FromImage(PictureBox.Image);
			Rectangle destRect = new Rectangle(0, 0, PictureBox.Image.Width, PictureBox.Image.Height);
			Rectangle srcRect = cameraRect;
			srcRect.X -= canvasRect.X;
			srcRect.Y -= canvasRect.Y;

			if (++hourGlassTimerTicks < 9)
			{
				g.DrawImage(canvasBitmap, destRect, srcRect, GraphicsUnit.Pixel);
				g.DrawImage(bmHourGlass,
				            new Rectangle(mousePosition.X-20, mousePosition.Y-25, 35, 40),
				            (hourGlassTimerTicks-3)*35, 0, 35, 40,
				            GraphicsUnit.Pixel,
				            imageAttrTransparent);
				DisplayButtons(g, pressedBackground);
				g.Dispose();
				PictureBox.Refresh();
				
				return;
			}

			hourGlassTimer.Enabled = false;

			showDate = false;

			// Sanduhr wieder löschen (kleines Rechteck würde reichen)
			g.DrawImage(canvasBitmap, destRect, srcRect, GraphicsUnit.Pixel);
			DisplayButtons(g, pressedBackground);
			g.Dispose();
			PictureBox.Refresh();

			contextMenu.Show(PictureBox, new Point(mouseX, mouseY));

			mouseX = -1;  // gegen Probleme beim Abbrechen des ContextMenus (-> Event_MouseMove)
		}

		public void Event_Resize(object sender, EventArgs e)
		{
			if (PictureBox.Image == null) return;

			//if (PictureBox.Image.Size != PictureBox.Size)
			//{
				//PictureBox.Resize -= new EventHandler(Event_Resize);

				PictureBox.Width = PictureBox.Parent.Width;
				PictureBox.Height = PictureBox.Parent.Height;
				PictureBox.Image.Dispose();
				PictureBox.Image = new Bitmap(PictureBox.Width, PictureBox.Height, PixelFormat.Format16bppRgb565);
				cameraRect.Width  = (int)(scaling*PictureBox.Width);
				cameraRect.Height = (int)(scaling*PictureBox.Height);

				//PictureBox.Resize += new EventHandler(Event_Resize);

				DisplayCamera();
			//}

			//OnResize(e);
		}

		private void Event_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
#if DEBUG
			if (e.Button != MouseButtons.Left) return;
#endif
			if (sender != PictureBox) return;  // absolut notwendig, da es sonst die Darstellung verrissen hat
			                                   // unklar ob das wirklich so ist (find "verrissen")

			if (ButtonMove(e)) return;

			if (!mindmapLoaded) return;
			if (PictureBox.Image == null) return;

			// Move Event wird scheints auch durch Abbrechen des ContextMenus ausgelöst.
			// Ist so! Ohne diesen Mechanismus gibts Probleme.
			// Signalisierung über mouseX < 0
			if (mouseX < 0)
			{
				return;
			}

			int x = mouseX - e.X;
			int y = mouseY - e.Y;

			int wackelGrenze = Const.GetVGAFactor() * 4;
			if (hourGlassTimer.Enabled && (Math.Abs(x) < wackelGrenze) && (Math.Abs(y) < wackelGrenze))  // nur gezittert
				return;

			hourGlassTimer.Enabled = false;

			showDate = false;

			lastX[wrPtr] = e.X;
			lastY[wrPtr] = e.Y;
			lastTime[wrPtr] = Environment.TickCount;
			if (++wrPtr >= kineticDepth)
				wrPtr = 0;
			if (wrPtr == firstPtr)
			{
				if (++firstPtr >= kineticDepth)
					firstPtr = 0;
			}

			cameraRect.X += (int)(scaling*x);
			cameraRect.Y += (int)(scaling*y);

			ValidateCameraRect(false);

			mouseX = e.X;
			mouseY = e.Y;

			// Viel probiert um das Geflacker zu beseitigen. Wenn man die Grafik
			// hier aufbaut funktionierts.
			//DisplayCamera();
		}

		private void Event_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (sender != PictureBox) return;  // absolut notwendig, da es sonst die Darstellung verrissen hat
			                                   // unklar ob das wirklich so ist (find "verrissen")

			kineticTimer.Enabled = false;
			wrPtr = 1;
			firstPtr = 0;
			lastX[0] = e.X;
			lastY[0] = e.Y;
			lastTime[0] = Environment.TickCount;
			for (int i = 1; i < kineticDepth; i++)
			{
				lastX[i] = lastX[0];
				lastY[i] = lastY[0];
				lastTime[i] = lastTime[0];
			}

			if (ButtonDown(e)) return;

			int action = quickContext.Pressed(e);
			if (action >= 0)
			{
				switch (action)
				{
					case MoveUp:
						OnMoveUp(new MyTreeViewEventArgs(this));
						break;
					case MoveDown:
						OnMoveDown(new MyTreeViewEventArgs(this));
						break;
					case Notes:
						OnNotes(new MyTreeViewEventArgs(this));
						break;
					case Icons:
						OnIcons(new MyTreeViewEventArgs(this));
						break;
					case Format:
						OnFormat(new MyTreeViewEventArgs(this));
						break;
					case Sister:
						OnSister(new MyTreeViewEventArgs(this));
						break;
					case Brother:
						OnBrother(new MyTreeViewEventArgs(this));
						break;
					case Child:
						OnChild(new MyTreeViewEventArgs(this));
						break;
					case NewParent:
						OnNewParent(new MyTreeViewEventArgs(this));
						break;
					default:  // kein enableter Button
						break;
				}

				quickContext.visible = false;
				doubleClickTimer.Enabled = false;

				DisplayCamera();
#if GRAPHICAL
				mouseX = -1;  // hilft eindeutig gegen das Verreissen
#endif
				return;
			}
			quickContext.visible = false;

			if (!mindmapLoaded) return;
			if (PictureBox.Image == null) return;

//			ignoreMouseUp = false;
//			if (e.Y >= EffectiveHeight())  // MenuBar
//			{
//				ignoreMouseUp = true;
//				return;
//			}

//			distX[0] = 0;
//			distY[0] = 0;
//			//timeStamp[0] = 0.0;
//			firstPtr = 0;
//			wrPtr = 0;

//			int x = e.X + cameraWindow.X;
//			int y = e.Y + cameraWindow.Y;
			int x = (int)(scaling*e.X) + cameraRect.X;
			int y = (int)(scaling*e.Y) + cameraRect.Y;

			bool expandChanged = false;

			if (doubleClickTimer.Enabled)  // Doppelklick
			{
				if (selectedNode.TextBox.Contains(x, y))  // auf dem selektierten Knoten
				{
					showDate = false;
					doubleClickTimer.Enabled = false;
					OnDoubleClick(new EventArgs());
					return;
				}
			}
			doubleClickTimer.Enabled = true;
			hourGlassTimerTicks = 0;
			hourGlassTimer.Enabled = true;

			mouseX = e.X;  // merken für move
			mouseY = e.Y;
			mousePosition = new Point(e.X, e.Y);  // merken für Sanduhr

			showDate = false;

			MyTreeNode node = Nodes[0];
			do
			{
				if (node.TextBox.Contains(x, y))
				{
//					ChangeSelectedNode(node);
					showDate = true;
					Graphics gr = Graphics.FromImage(canvasBitmap);
					Rectangle selNodeRect = selectedNode.TextBox;
					selNodeRect.Offset(-canvasRect.X, -canvasRect.Y);
					gr.DrawRectangle(new Pen(selectedNode.BackColor), selNodeRect);
					selectedNode = node;
					selNodeRect = selectedNode.TextBox;
					selNodeRect.Offset(-canvasRect.X, -canvasRect.Y);
					gr.DrawRectangle(new Pen(selNodeColor), selNodeRect);
					gr.Dispose();
					DisplayCamera();

					break;
				}

				if (node.Expander.Contains(x, y))
				{
					expandChanged = true;
					break;
				}

				node = GetNextNode(node, node.expanded);
			} while (node != null);

			if ((node != null) && expandChanged)
			{
				if (node.Nodes.Count != 0)
				{
					node.expanded = !node.expanded;
					selectedNode = node;
					fixSelectedNode = true;
					selNodeRelativeX = selectedNode.TextBox.X - cameraRect.X;
					selNodeRelativeY = selectedNode.TextBox.Y - cameraRect.Y;
//					if (EndUpdate(true))  // der Speicher hat gereicht
					{
						Layout();
						EnsureVisible(selectedNode, true);
						if (node.expanded)
							OnAfterExpand(new MyTreeViewEventArgs(this));
						else
							OnAfterCollapse(new MyTreeViewEventArgs(this));
					}
//					else  // zu wenig Speicher für diese Aktion (vermutlich Expand)
//					{
//						node.expanded = !node.expanded;
//						EndUpdate(true);  // muss jetzt klappen
//						EnsureVisible(selectedNode);
//					}
				}
			}
		}

		private void Event_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (sender != PictureBox) return;  // absolut notwendig, da es sonst die Darstellung verrissen hat
			                                   // unklar ob das wirklich so ist (find "verrissen")

			if (ButtonUp(e)) return;

			if (!mindmapLoaded) return;
			if (PictureBox.Image == null) return;
//			if (ignoreMouseUp) return;

			hourGlassTimer.Enabled = false;

			int deltaTime = Environment.TickCount - lastTime[firstPtr];
			if (deltaTime == 0) deltaTime = 1;
			kineticSpeedX = -((double)(e.X - lastX[firstPtr]))/((double)deltaTime);
			kineticSpeedY = -((double)(e.Y - lastY[firstPtr]))/((double)deltaTime);
			if ((Math.Abs(kineticSpeedX) > 0.005) || (Math.Abs(kineticSpeedY) > 0.005))
			{
//				kineticDistX = Math.Sign(kineticSpeedX) * 8.0;
//				kineticDistY = Math.Sign(kineticSpeedY) * 8.0;
//				if (settings.Navigator)
//					MessageBox.Show("kineticSpeed\n" + kineticSpeedX.ToString() + " " + kineticSpeedY.ToString() +
//					                "\nup/downTime\n" + upTime.ToString() + " " + downTime.ToString());
				kineticDistX = kineticSpeedX * kineticInterval;
				kineticDistY = kineticSpeedY * kineticInterval;
				kineticTimer.Enabled = true;
				return;
			}
//			if (wrPtr != firstPtr)  // nur dann kinetic scrolling
//			{
//				if (--wrPtr < 0)
//					wrPtr = 9;
//
//				if (wrPtr == firstPtr)  // es gibt nur einen Wert
//				{
//					;  // dann kein kinetic scrolling
//				}
//				else
//				{
//					//double time = timeStamp[wrPtr] - timeStamp[firstPtr];
//					//if (time < 0)
//					//	time += 24*3600;
//	
//					int x = 0;
//					int y = 0;
//					int entries = 0;
//					do
//					{
//						x += distX[firstPtr];
//						y += distY[firstPtr];
//						if (++firstPtr >= 10)
//							firstPtr = 0;
//						entries++;
//					} while (firstPtr != wrPtr);
//
//					//kineticDistX = (double)x / entries * ((double)kineticInterval/1000);
//					//kineticDistY = (double)y / entries * ((double)kineticInterval/1000);
//					kineticDistX = (double)x / entries;
//					kineticDistY = (double)y / entries;
//
//					kineticTimer.Enabled = true;
//
//					return;
//				}
//			}

			CenterCanvas();
			DisplayCamera();
		}

		public void BeginUpdate()
		{
			// TODO: BeginUpdate fehlt noch
		}

		public void Clear(bool inclCameraWindow)
		{
			// TODO: alle Nodes löschen oder macht das die Garbage Collection?
			Nodes.Clear();
			SelectedNode = null;
			fixSelectedNode = false;
			scaling = 1.0;
			button[0].enabled = true;
			button[1].enabled = true;
			if (inclCameraWindow)
			{
				mindmapLoaded = false;
				cameraRect = new Rectangle(-1, 0, (int)(scaling*PictureBox.Width), (int)(scaling*PictureBox.Height));
			}
			maxX = Const.xMargin;
			maxY = Const.yMargin;

			if (inclCameraWindow)
			{
				if (PictureBox.Image != null)
					PictureBox.Image.Dispose();
				PictureBox.Image = new Bitmap(PictureBox.Width, PictureBox.Height, PixelFormat.Format16bppRgb565);
				canvasRect.Location = new Point(0, 0);
				canvasBitmap.Dispose();
				canvasBitmap = new Bitmap(canvasRect.Width, canvasRect.Height, PixelFormat.Format16bppRgb565);
				cameraRect.Location = new Point(-1, 0);
				Graphics gr = Graphics.FromImage(canvasBitmap);
				gr.FillRectangle(new SolidBrush(backGroundColor), new Rectangle(-1, 0, PictureBox.Width, PictureBox.Height));
				gr.Dispose();
				DisplayCamera();
			}
		}

		public MyTreeNode GetNextNode(MyTreeNode node, bool inclSubNodes)
		{
			MyTreeNode tempNode = node;

			if ((tempNode.Nodes.Count != 0) &&  // es gibt Children
			    (inclSubNodes))                 // und sie sollen ausgewertet werden
				return(tempNode.Nodes[0]);      // -> first child returnen

			do
			{
				if (tempNode.Parent == null)  // es gibt keinen Parent
					return(null);             // -> fertig
	
				if (tempNode.Index < (tempNode.Parent.Nodes.Count-1))  // der Parent hat weitere Children
					return(tempNode.Parent.Nodes[tempNode.Index+1]);   // -> next brother returnen
	
				tempNode = tempNode.Parent;  // auf den Parent wechseln
			} while (true);
		}

		public void ChangeSelectedNode(MyTreeNode node)
		{
			Graphics gr = Graphics.FromImage(canvasBitmap);
			gr.DrawRectangle(new Pen(selectedNode.BackColor), selectedNode.TextBox);
			selectedNode = node;
			gr.DrawRectangle(new Pen(selNodeColor), selectedNode.TextBox);
			gr.Dispose();
			EnsureVisible(node, true);
		}

		public MyTreeNode SelectedNode
		{
			get { return selectedNode; }
			set { selectedNode = value; }
		}

		public Color BackColor
		{
			get { return PictureBox.BackColor; }
			set { PictureBox.BackColor = value; }
		}

		public ContextMenu ContextMenu
		{
			get { return contextMenu; }
			set { contextMenu = value; }
		}

		public DockStyle Dock
		{
			get { return PictureBox.Dock; }
			set { PictureBox.Dock = value; }
		}

		public ImageList ImageList
		{
			get { return imageList; }
			set { imageList = value; }
		}

		public Point Location
		{
			get { return PictureBox.Location; }
			set { PictureBox.Location = value; }
		}

		public int Height
		{
			get { return PictureBox.Height; }
			set { PictureBox.Height = value; }
		}

		public Control Parent
		{
			get { return PictureBox.Parent; }
			set { PictureBox.Parent = value; }
		}
	}
}
#endif
