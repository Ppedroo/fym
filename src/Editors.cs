/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Peter
 * Datum: 08.03.2008
 * Zeit: 14:13
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */

using Microsoft.WindowsCE.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;  // für Resources
using System.Windows.Forms;

namespace FreeYourMind
{
	/// <summary>
	/// Description of Editors.
	/// </summary>
	public class Editors
	{
#if GRAPHICAL
		private MyTreeView treeView;
#else
		private TreeView treeView;
#endif
		ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));

		private Form        editDialog  = new Form();         // Edit Eingabedialog
		private TextBox     editTextBox = new TextBox();      // seine Textbox
		private MainMenu    editMenu    = new MainMenu();     // sein Menü
		private ContextMenu cmEdit      = new ContextMenu();  // sein Context Menü
		private MenuItem    cmItemCut   = new MenuItem();
		private MenuItem    cmItemCopy  = new MenuItem();
		private MenuItem    cmItemPaste = new MenuItem();

		private Form        editDialogHtml    = new Form();  // HTML Editor
		private MainMenu    editMenuHtml      = new MainMenu();
		private ContextMenu emptyContextMenu  = new ContextMenu();
		private TabControl  tabControlHtml;  // auf gut Glück geändert wg. Problem auf Palm 500v
		private TabPage     tabPageLayoutHtml;
		private TabPage     tabPageHtml;
		private WebBrowser  webBrowserHtml    = new WebBrowser();
		private TextBox     textBoxHtml       = new TextBox();

		private Form        editDialogOnePwd = new Form();  // One Password Editor
		private MainMenu    editMenuOnePwd   = new MainMenu();
		private Label       labelOnePwd      = new Label();
		private TextBox     textBoxOnePwd    = new TextBox();

		private Form        editDialogTwoPwd = new Form();  // Two Passwords Editor
		private MainMenu    editMenuTwoPwd   = new MainMenu();
		private Label       labelTwoPwd      = new Label();
		private TextBox     textBoxTwoPwd1   = new TextBox();
		private TextBox     textBoxTwoPwd2   = new TextBox();

		private InputPanel inputPanel              = new InputPanel();
		private bool       inputPanelVisibleAtTree = false;
		private bool       inputPanelVisibleAtHtml = true;

		private string strHtml = "";

//		private string strClip = "";

		private Texts t;
		private int language;
#if GRAPHICAL
		public Editors(MyTreeView treeViewParam, int languageParam, Texts texts, Color backColor)
#else
		public Editors(TreeView treeViewParam, int languageParam, Texts texts)
#endif
		{
			treeView = treeViewParam;
			t = texts;
			language = languageParam;

			tabControlHtml    = new TabControl();
			tabPageLayoutHtml = new TabPage();
			tabPageHtml       = new TabPage();

			// Context Menus
			cmItemCut.Text = t.t[t.Cut, language];
			cmItemCut.Click += new System.EventHandler(this.cmItemCut_Click);

			cmItemCopy.Text = t.t[t.Copy, language];
			cmItemCopy.Click += new System.EventHandler(this.cmItemCopy_Click);

			cmItemPaste.Text = t.t[t.Paste, language];
			cmItemPaste.Click += new System.EventHandler(this.cmItemPaste_Click);

			cmEdit.Popup += new EventHandler(cmPopupEvent);

			// Main Menu Editor
			MenuItem emItemCancel = new MenuItem();
			emItemCancel.Text = t.t[t.Cancel, language];
			emItemCancel.Click += new System.EventHandler(this.emItemCancel_Click);

			MenuItem emItemOk = new MenuItem();
			emItemOk.Text = "OK";
			emItemOk.Click += new System.EventHandler(this.emItemOk_Click);

			editMenu.MenuItems.Add(emItemCancel);
			editMenu.MenuItems.Add(emItemOk);

			// Main Menu HTML Editor
			MenuItem ehItemCancel = new MenuItem();
			ehItemCancel.Text = t.t[t.Cancel, language];
			ehItemCancel.Click += new System.EventHandler(this.ehItemCancel_Click);

			MenuItem ehItemOk = new MenuItem();
			ehItemOk.Text = "OK";
			ehItemOk.Click += new System.EventHandler(this.ehItemOk_Click);

			editMenuHtml.MenuItems.Add(ehItemCancel);
			editMenuHtml.MenuItems.Add(ehItemOk);

			// Main Menu One Password Editor
			MenuItem onePwdItemCancel = new MenuItem();
			onePwdItemCancel.Text = t.t[t.Cancel, language];
			onePwdItemCancel.Click += new System.EventHandler(this.onePwdItemCancel_Click);

			MenuItem onePwdItemOk = new MenuItem();
			onePwdItemOk.Text = "OK";
			onePwdItemOk.Click += new System.EventHandler(this.onePwdItemOk_Click);

			editMenuOnePwd.MenuItems.Add(onePwdItemCancel);
			editMenuOnePwd.MenuItems.Add(onePwdItemOk);

			// Main Menu Two Passwords Editor
			MenuItem twoPwdItemCancel = new MenuItem();
			twoPwdItemCancel.Text = t.t[t.Cancel, language];
			twoPwdItemCancel.Click += new System.EventHandler(this.twoPwdItemCancel_Click);

			MenuItem twoPwdItemOk = new MenuItem();
			twoPwdItemOk.Text = "OK";
			twoPwdItemOk.Click += new System.EventHandler(this.twoPwdItemOk_Click);

			editMenuTwoPwd.MenuItems.Add(twoPwdItemCancel);
			editMenuTwoPwd.MenuItems.Add(twoPwdItemOk);

			inputPanel.EnabledChanged += new EventHandler(inputPanel_EnabledChanged);
			inputPanel.Enabled = false;

			tabControlHtml.SelectedIndexChanged += new EventHandler(tabControlHtml_SelIndexChanged);

			webBrowserHtml.Dock = DockStyle.Fill;
			webBrowserHtml.ContextMenu = emptyContextMenu;  // TODO: funktioniert noch nicht

			textBoxHtml.BorderStyle = BorderStyle.None;
			textBoxHtml.Multiline = true;
			textBoxHtml.AcceptsReturn = true;
			textBoxHtml.AcceptsTab = true;
			textBoxHtml.WordWrap = true;  // CR0012
//			textBoxHtml.WordWrap = false;
			textBoxHtml.Dock = DockStyle.Fill;
			textBoxHtml.ScrollBars = ScrollBars.Vertical;  // CR0012
//			textBoxHtml.ScrollBars = ScrollBars.Both;
			textBoxHtml.ContextMenu = cmEdit;

			editTextBox.Multiline = true;
			editTextBox.AcceptsReturn = true;
			editTextBox.Dock = DockStyle.Top;
			editTextBox.ScrollBars = ScrollBars.Vertical;
			editTextBox.WordWrap = true;
			editTextBox.ContextMenu = cmEdit;

			tabPageLayoutHtml.Text = "Layout";
			tabPageLayoutHtml.Controls.Add(webBrowserHtml);

			tabPageHtml.Text = "HTML";
			tabPageHtml.Controls.Add(textBoxHtml);

			tabControlHtml.TabPages.Add(tabPageLayoutHtml);
			tabControlHtml.TabPages.Add(tabPageHtml);
			tabControlHtml.Dock = DockStyle.Top;

#if GRAPHICAL
			editDialogHtml.Icon = ((System.Drawing.Icon)(resources.GetObject("FYMgraphicsIcon")));
        	editDialogHtml.BackColor = backColor;
#else
			editDialogHtml.Icon = ((System.Drawing.Icon)(resources.GetObject("FYMIcon")));
        	editDialogHtml.BackColor = Color.LightGray;
#endif
			editDialogHtml.Controls.Clear();
			editDialogHtml.Controls.Add(tabControlHtml);
			editDialogHtml.Menu = editMenuHtml;
			editDialogHtml.Text = t.t[t.Edit, language];

#if GRAPHICAL
			editDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("FYMgraphicsIcon")));
        	editDialog.BackColor = backColor;
#else
			editDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("FYMIcon")));
        	editDialog.BackColor = Color.LightGray;
#endif
			editDialog.Controls.Clear();
			editDialog.Controls.Add(editTextBox);   // Dialogbox aufbauen
			editDialog.Menu = editMenu;
			editDialog.Text = t.t[t.Edit, language];

			textBoxOnePwd.PasswordChar = '*';
			textBoxOnePwd.Width = (8*editDialog.Width)/10;
			textBoxOnePwd.Location = new Point((editDialog.Width-textBoxOnePwd.Width)/2,
			                                   editDialog.Height/8);

			int n = Const.GetVGAFactor();

			labelOnePwd.Text = t.t[t.EnterPassword, language];
			labelOnePwd.Size = new Size(n*250,n*20);
			labelOnePwd.Location = new Point((editDialog.Width-textBoxOnePwd.Width)/2,
			                                 editDialog.Height/25);

#if GRAPHICAL
			editDialogOnePwd.Icon = ((System.Drawing.Icon)(resources.GetObject("FYMgraphicsIcon")));
			editDialogOnePwd.BackColor = backColor;
#else
			editDialogOnePwd.Icon = ((System.Drawing.Icon)(resources.GetObject("FYMIcon")));
			editDialogOnePwd.BackColor = Color.LightGray;
#endif
			editDialogOnePwd.Controls.Clear();
			editDialogOnePwd.Controls.Add(textBoxOnePwd);
			editDialogOnePwd.Controls.Add(labelOnePwd);
			editDialogOnePwd.Menu = editMenuOnePwd;
			editDialogOnePwd.Text = t.t[t.Password, language];

			textBoxTwoPwd1.PasswordChar = '*';
			textBoxTwoPwd1.Width = (8*editDialog.Width)/10;
			textBoxTwoPwd1.Location = new Point((editDialog.Width-textBoxOnePwd.Width)/2,
			                                     editDialog.Height/8);

			textBoxTwoPwd2.PasswordChar = '*';
			textBoxTwoPwd2.Width = (8*editDialog.Width)/10;
			textBoxTwoPwd2.Location = new Point((editDialog.Width-textBoxOnePwd.Width)/2,
			                                     editDialog.Height/8 + 2*textBoxOnePwd.Height);

			labelTwoPwd.Text = t.t[t.EnterPasswordTwice, language];
			labelTwoPwd.Size = new Size(n*250,n*20);
			labelTwoPwd.Location = new Point((editDialog.Width-textBoxOnePwd.Width)/2,
			                                 editDialog.Height/25);

#if GRAPHICAL
			editDialogTwoPwd.Icon = ((System.Drawing.Icon)(resources.GetObject("FYMgraphicsIcon")));
			editDialogTwoPwd.BackColor = backColor;
#else
			editDialogTwoPwd.Icon = ((System.Drawing.Icon)(resources.GetObject("FYMIcon")));
			editDialogTwoPwd.BackColor = Color.LightGray;
#endif
			editDialogTwoPwd.Menu = editMenuTwoPwd;
			editDialogTwoPwd.Text = t.t[t.Password, language];

//			strClip = "";
		}

		public bool EnterOnePwd(ref string pwd)
		{
			bool ret = false;
#if (!DEBUG)
			bool inputPanelWasVisible = inputPanelVisibleAtTree;
			inputPanel.Enabled = true;
#endif
			textBoxOnePwd.Text = "";

			editDialogOnePwd.ShowDialog();

			pwd = textBoxOnePwd.Text;

			if (editDialogOnePwd.DialogResult == DialogResult.OK)
				ret = true;

#if (!DEBUG)
			inputPanelVisibleAtTree = inputPanelWasVisible;
			TreeViewHeight();
#endif
			return(ret);
		}

		public bool EnterTwoPwd(ref string pwd)
		{
			bool ret = false;
#if (!DEBUG)
			bool inputPanelWasVisible = inputPanelVisibleAtTree;
			inputPanel.Enabled = true;
#endif
			while(true)
			{
				// TODO: place outside the loop. It is here 'cause I didn't know Focus()
				editDialogTwoPwd.Controls.Clear();  // neu aufbauen - meine einzige Chance
				editDialogTwoPwd.Controls.Add(textBoxTwoPwd1);
				editDialogTwoPwd.Controls.Add(textBoxTwoPwd2);
				editDialogTwoPwd.Controls.Add(labelTwoPwd);
				editDialogTwoPwd.Controls[0].Focus();  // den Fokus auf Pwd1 zu setzen

				textBoxTwoPwd1.Text = "";
				textBoxTwoPwd2.Text = "";

				editDialogTwoPwd.ShowDialog();
	
				pwd = textBoxTwoPwd1.Text;

				if (editDialogTwoPwd.DialogResult == DialogResult.OK)  // OK gedrückt
				{
					if ((pwd == textBoxTwoPwd2.Text) &&    // und beide Passwörter sind identisch
					    (textBoxTwoPwd2.Text.Length > 1))  // und min. 2 Zeichen lang
					{
							ret = true;
							break;
					}
					else
					{
						MessageBox.Show(t.t[t.PasswordsNotIdent, language],
						                t.t[t.Info, language]);
					}
				}
				else  // cancel
				{
					break;
				}
			}
#if (!DEBUG)
			inputPanelVisibleAtTree = inputPanelWasVisible;
			TreeViewHeight();
#endif
			return(ret);
		}

	   	public string EditDialog(string strOriginal, ref bool canceled)
		{
			String str = strOriginal;  // Return String

#if !GRAPHICAL
			editTextBox.Font = treeView.Font;
#endif
			editTextBox.Text = strOriginal;  // Originaltext eintragen
			editTextBox.Modified = false;    // könnte vom vorherigen Aufruf noch modified sein
#if (!DEBUG)
			bool inputPanelWasVisible = inputPanelVisibleAtTree;
			inputPanel.Enabled = true;
			editTextBox.Height = inputPanel.VisibleDesktop.Height;
#endif
			editDialog.ShowDialog();                 // Dialog anzeigen

			if (editDialog.DialogResult == DialogResult.OK)
			{
				str = editTextBox.Text;              // geänderten Text übernehmen
				canceled = false;
			}
			else
			{
				canceled = true;
			}
#if (!DEBUG)
			inputPanelVisibleAtTree = inputPanelWasVisible;
			TreeViewHeight();
#endif
			return(str);
		}

		public bool EditDialogHtml(ref List<string> listHtml)
		{
#if !GRAPHICAL
			textBoxHtml.Font = treeView.Font;
#endif
			strHtml = "";
			foreach (string line in listHtml)
			{
				strHtml += line + "\r\n";
			}

			textBoxHtml.Text = strHtml;
			textBoxHtml.Modified = false;    // könnte vom vorherigen Aufruf noch modified sein
			tabControlHtml.SelectedIndex = 0;
			//webBrowserHtml.DocumentText wird bei tabControlHtml_SelIndexChanged geladen
#if (!DEBUG)
			bool inputPanelWasVisible = inputPanelVisibleAtTree;
			inputPanel.Enabled = false;
			//inputPanelVisibleAtHtml = false;
			tabControlHtml.Height = tabControlHtml.Parent.Height;
#endif
			editDialogHtml.ShowDialog();                 // Dialog anzeigen
#if (!DEBUG)
			inputPanelVisibleAtTree = inputPanelWasVisible;
			TreeViewHeight();
#endif
			if (editDialogHtml.DialogResult != DialogResult.OK)
				return(false);

			strHtml = textBoxHtml.Text;
			listHtml.Clear();
			int startIndex = 0;
			int stopIndex = 0;
			while ((stopIndex = strHtml.IndexOf("\r\n",startIndex)) >= 0)
			{
				listHtml.Add(strHtml.Substring(startIndex,stopIndex-startIndex));
				startIndex = stopIndex + 2;
				if (startIndex >= strHtml.Length) break;
			}

			return(true);
		}

		private void cmPopupEvent(object sender, System.EventArgs e)
		{
			TextBox tb;

//			while (cmEdit.MenuItems.Count != 3)
//			{
//				cmEdit.MenuItems.RemoveAt(3);  // wegen "Übersetzen..." von SlovoEd
//			}
// Die Versuche bzgl. "Übersetzen..." waren erfolglos. Es wird immer wieder aufs neue angehängt.
			cmEdit.MenuItems.Clear();
			cmEdit.MenuItems.Add(cmItemCut);
			cmEdit.MenuItems.Add(cmItemCopy);
			cmEdit.MenuItems.Add(cmItemPaste);

			if (editTextBox.Visible)
				tb = editTextBox;
			else
				tb = textBoxHtml;

			if (tb.SelectedText == "")
			{
				cmItemCut.Enabled = false;
				cmItemCopy.Enabled = false;
			}
			else
			{
				cmItemCut.Enabled = true;
				cmItemCopy.Enabled = true;
			}

			IDataObject iData = Clipboard.GetDataObject();
			if(!iData.GetDataPresent(DataFormats.Text))
//			if (strClip == "")
				cmItemPaste.Enabled = false;
			else
				cmItemPaste.Enabled = true;
		}

		private void cmItemCut_Click(object sender, System.EventArgs e)
		{
			TextBox tb;

			if (editTextBox.Visible)
				tb = editTextBox;
			else
				tb = textBoxHtml;

//			strClip = tb.SelectedText;
			Clipboard.SetDataObject((object)tb.SelectedText);

			tb.SelectedText = "";
		}

		private void cmItemCopy_Click(object sender, System.EventArgs e)
		{
			TextBox tb;

			if (editTextBox.Visible)
				tb = editTextBox;
			else
				tb = textBoxHtml;

//			strClip = tb.SelectedText;
			Clipboard.SetDataObject((object)tb.SelectedText);
		}

		private void cmItemPaste_Click(object sender, System.EventArgs e)
		{
			TextBox tb;

			if (editTextBox.Visible)
				tb = editTextBox;
			else
				tb = textBoxHtml;

//			tb.SelectedText = strClip;
			IDataObject iData = Clipboard.GetDataObject();
			if(iData.GetDataPresent(DataFormats.Text))
				tb.SelectedText = (String)iData.GetData(DataFormats.Text);
		}

		private void onePwdItemCancel_Click(object sender, System.EventArgs e)
		{
	        editDialogOnePwd.DialogResult = DialogResult.Cancel;
		}

		private void onePwdItemOk_Click(object sender, System.EventArgs e)
		{
	        editDialogOnePwd.DialogResult = DialogResult.OK;
		}

		private void twoPwdItemCancel_Click(object sender, System.EventArgs e)
		{
	        editDialogTwoPwd.DialogResult = DialogResult.Cancel;
		}

		private void twoPwdItemOk_Click(object sender, System.EventArgs e)
		{
	        editDialogTwoPwd.DialogResult = DialogResult.OK;
		}

		private void emItemCancel_Click(object sender, System.EventArgs e)
		{
			if (editTextBox.Modified)
			{
				DialogResult result = MessageBox.Show(t.t[t.LoseChanges, language],
				                                      t.t[t.CancelEdit, language],
				                                      MessageBoxButtons.YesNo,
				                                      MessageBoxIcon.Exclamation,
				                                      MessageBoxDefaultButton.Button2);

				if (result == DialogResult.Yes)
			    {
			        editDialog.DialogResult = DialogResult.Cancel;
		        }
			}
			else
		        editDialog.DialogResult = DialogResult.Cancel;
		}

		private void ehItemCancel_Click(object sender, System.EventArgs e)
		{
			if (textBoxHtml.Modified)
			{
				DialogResult result = MessageBox.Show(t.t[t.LoseChanges, language],
				                                      t.t[t.CancelEdit, language],
				                                      MessageBoxButtons.YesNo,
				                                      MessageBoxIcon.Exclamation,
				                                      MessageBoxDefaultButton.Button2);

				if (result == DialogResult.Yes)
			    {
			        editDialogHtml.DialogResult = DialogResult.Cancel;
		        }
			}
			else
		        editDialogHtml.DialogResult = DialogResult.Cancel;
		}

		private void emItemOk_Click(object sender, System.EventArgs e)
		{
			editDialog.DialogResult = DialogResult.OK;
		}

		private void ehItemOk_Click(object sender, System.EventArgs e)
		{
			editDialogHtml.DialogResult = DialogResult.OK;
		}

		public void TreeViewHeight()
		{
#if (!DEBUG)
#if GRAPHICAL
			inputPanel.Enabled = false;
			treeView.Height = treeView.Parent.Height;
#else
			Rectangle VisibleRect;

			if (inputPanelVisibleAtTree)
			{
				inputPanel.Enabled = true;
				VisibleRect = inputPanel.VisibleDesktop;
				treeView.Height = VisibleRect.Height;
			}
			else
			{
				inputPanel.Enabled = false;
				treeView.Height = treeView.Parent.Height;
			}
#endif
#endif
		}

 		public bool InputPanelVisibleAtTree
		{
			set { inputPanelVisibleAtTree = value; }
			get { return(inputPanelVisibleAtTree); }
		}

		private void tabControlHtml_SelIndexChanged(object sender, EventArgs e)
		{
			if (tabControlHtml.SelectedIndex == 0)
			{
				inputPanel.Enabled = false;
				strHtml = textBoxHtml.Text;
				webBrowserHtml.DocumentText = strHtml;
			}
			else
			{
				inputPanel.Enabled = inputPanelVisibleAtHtml;
			}
		}

		public void inputPanel_EnabledChanged(object sender, EventArgs e)
		{
#if (!DEBUG)
			Rectangle VisibleRect;

			if (editDialog.Visible)
			{
				if (inputPanel.Enabled)
				{
					VisibleRect = inputPanel.VisibleDesktop;
					editTextBox.Height = VisibleRect.Height;
				}
				else
				{
					editTextBox.Height = editTextBox.Parent.Height;
				}
			}
			else
			{
				if (editDialogHtml.Visible)
				{
					if (inputPanel.Enabled)
					{
						VisibleRect = inputPanel.VisibleDesktop;
						tabControlHtml.Height = VisibleRect.Height;
					}
					else
					{
						tabControlHtml.Height = tabControlHtml.Parent.Height;
					}
					if (tabControlHtml.SelectedIndex == 1)  // HTML Editor
						inputPanelVisibleAtHtml = inputPanel.Enabled;
				}
				else
				{
#if GRAPHICAL
					inputPanelVisibleAtTree = false;
					treeView.Event_Resize(sender, e);
#else
					if (inputPanel.Enabled)
					{
						inputPanelVisibleAtTree = true;
						VisibleRect = inputPanel.VisibleDesktop;
						treeView.Dock = DockStyle.Top;
						treeView.Height = VisibleRect.Height;
					}
					else
					{
						inputPanelVisibleAtTree = false;
						treeView.Dock = DockStyle.Fill;
						//treeView.Height = treeView.Parent.Height;
					}
#endif
				}
			}
#endif
		}

		public void InputPanelEnabled(bool enabled)
		{
#if (!DEBUG)
			inputPanel.Enabled = enabled;
#endif
		}

		public int InputPanelVisibleDesktopHeight()
		{
#if (!DEBUG)
			return(inputPanel.VisibleDesktop.Height);
#else
			return(300);
#endif
		}
	}
}
