/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schmid_p
 * Datum: 12.11.2007
 * Zeit: 09:47
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */

// Der .net TreeView stellt Icons immer 16x16 dar, größere Icons werden einfach verkleinert.
// Auf VGA-Bildschirmen erscheinen Icons im .net TreeView also relativ klein.
// Im FYMgraphics TreeView könnte ich größere Icons darstellen.
//
//       FYM TreeView                FYMgraphics TreeView        Auswahlfenster bei beiden
// ----------------------------------------------------------------------------------------
// QVGA  16x16: OK                   16x16: OK                   16x16: OK
//
//       32x32: OK                   32x32: NOK                  32x32: NOK
//         (autom. -> 16x16)           (ist zu groß)               (ist zu groß)
//
// VGA   16x16: OK                   16x16: OK                   16x16: NOK
//         (erscheint klein)           (erscheint klein)           (ist zu klein)
//       32x32: OK                   32x32: OK                   32x32: OK
//         (autom. -> 16x16)

using System;
using System.Drawing;
using System.Collections.Generic;  // für List
using System.ComponentModel;       // für Resources
using System.Windows.Forms;

namespace FreeYourMind
{
	/// <summary>
	/// Description of MyImages.
	/// </summary>
	public class Images
	{
		ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
		public ImageList treeIconList = new ImageList();
		private List<string> iconNameList = new List<string>();

		private Form iconsDialog = new Form();  // Iconsdialog
		private Panel panelAddIcons = new Panel();
		private Label labelAdd = new Label();
		private string iconName;

		private Texts t;
		private int language;

		private PictureBox pbHelp = new PictureBox();
		private PictureBox pbMessageboxWarning = new PictureBox();
		private PictureBox pbIdea = new PictureBox();
		private PictureBox pbButtonOk = new PictureBox();
		private PictureBox pbButtonCancel = new PictureBox();
		private PictureBox pbFull_0 = new PictureBox();
		private PictureBox pbFull_1 = new PictureBox();
		private PictureBox pbFull_2 = new PictureBox();
		private PictureBox pbFull_3 = new PictureBox();
		private PictureBox pbFull_4 = new PictureBox();
		private PictureBox pbFull_5 = new PictureBox();
		private PictureBox pbFull_6 = new PictureBox();
		private PictureBox pbFull_7 = new PictureBox();
		private PictureBox pbFull_8 = new PictureBox();
		private PictureBox pbFull_9 = new PictureBox();
		private PictureBox pbBack = new PictureBox();
		private PictureBox pbForward = new PictureBox();
		private PictureBox pbAttach = new PictureBox();
		private PictureBox pbKsmiletris = new PictureBox();
		private PictureBox pbClanbomber = new PictureBox();
		private PictureBox pbDesktop_new = new PictureBox();
		private PictureBox pbFlag = new PictureBox();
		private PictureBox pbGohome = new PictureBox();
		private PictureBox pbKaddressbook = new PictureBox();
		private PictureBox pbKnotify = new PictureBox();
		private PictureBox pbKorn = new PictureBox();
		private PictureBox pbMail = new PictureBox();
		private PictureBox pbPassword = new PictureBox();
		private PictureBox pbPencil = new PictureBox();
		private PictureBox pbStop = new PictureBox();
		private PictureBox pbWizard = new PictureBox();
		private PictureBox pbXmag = new PictureBox();
		private PictureBox pbBell = new PictureBox();
		private PictureBox pbBookmark = new PictureBox();
		private PictureBox pbPenguin = new PictureBox();
		private PictureBox pbLicq = new PictureBox();

#if GRAPHICAL
		public Images(int languageParam, Texts texts, Color backColor)
#else
		public Images(int languageParam, Texts texts)
#endif
		{
			t = texts;
			language = languageParam;

//			int n = 1;  // QVGA
//
//			if (Screen.PrimaryScreen.Bounds.Height > 320)  // VGA
//				n = 2;

			int n = Const.GetVGAFactor();

			labelAdd.Text = t.t[t.ClickOnIcon, language];
//			labelAdd.Size = new Size(n*180,n*20);
			labelAdd.Size = new Size(n*180,n*35);
			// TODO: abhängig von ScreenOrientation machen
			labelAdd.Location = new Point(n*50,n*35);

//			panelAddIcons.BackColor = SystemColors.ActiveBorder;
			panelAddIcons.BackColor = Color.FromArgb(51, 51, 51);  // das schöne Grau von Adobe Reader
			panelAddIcons.Size = new Size(n*128,n*110);  // 7*18+2=128, 6*18+2=110
			// TODO: abhängig von ScreenOrientation machen
//			panelAddIcons.Location = new Point(n*56,n*56);  // 56+128+56=240; 2*56+2*128+2*56=2*240
			panelAddIcons.Location = new Point(n*56,n*70);  // 56+128+56=240; 2*56+2*128+2*56=2*240

			Bitmap bmDefault;
			if (n == 1)
				bmDefault = (Bitmap)resources.GetObject("Default");
			else
				bmDefault = (Bitmap)resources.GetObject("Default_VGA");
			treeIconList.Images.Add(bmDefault);  // jeder Knoten erhält per default das 0. Image
			iconNameList.Add("Default");

			Bitmap bmDummy = (Bitmap)resources.GetObject("Dummy");
			treeIconList.Images.Add(bmDummy);
			iconNameList.Add("Dummy");

			Bitmap bmHelp = (Bitmap)resources.GetObject("help");
			treeIconList.Images.Add(bmHelp);
			iconNameList.Add("help");
			pbHelp.Size = new Size(n*16,n*16);
			pbHelp.SizeMode = PictureBoxSizeMode.StretchImage;
			pbHelp.Image = bmHelp;
			pbHelp.Location = new Point(n*2,n*2);
			pbHelp.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbHelp);

			Bitmap bmMessageboxWarning = (Bitmap)resources.GetObject("messagebox_warning");
			treeIconList.Images.Add(bmMessageboxWarning);
			iconNameList.Add("messagebox_warning");
			pbMessageboxWarning.Size = new Size(n*16,n*16);
			pbMessageboxWarning.SizeMode = PictureBoxSizeMode.StretchImage;
			pbMessageboxWarning.Image = bmMessageboxWarning;
			pbMessageboxWarning.Location = new Point(n*20,n*2);
			pbMessageboxWarning.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbMessageboxWarning);

			Bitmap bmIdea = (Bitmap)resources.GetObject("idea");
			treeIconList.Images.Add(bmIdea);
			iconNameList.Add("idea");
			pbIdea.Size = new Size(n*16,n*16);
			pbIdea.SizeMode = PictureBoxSizeMode.StretchImage;
			pbIdea.Image = bmIdea;
			pbIdea.Location = new Point(n*38,n*2);
			pbIdea.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbIdea);

			Bitmap bmButton_ok = (Bitmap)resources.GetObject("button_ok");
			treeIconList.Images.Add(bmButton_ok);
			iconNameList.Add("button_ok");
			pbButtonOk.Size = new Size(n*16,n*16);
			pbButtonOk.SizeMode = PictureBoxSizeMode.StretchImage;
			pbButtonOk.Image = bmButton_ok;
			pbButtonOk.Location = new Point(n*56,n*2);
			pbButtonOk.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbButtonOk);

			Bitmap bmButton_cancel = (Bitmap)resources.GetObject("button_cancel");
			treeIconList.Images.Add(bmButton_cancel);
			iconNameList.Add("button_cancel");
			pbButtonCancel.Size = new Size(n*16,n*16);
			pbButtonCancel.SizeMode = PictureBoxSizeMode.StretchImage;
			pbButtonCancel.Image = bmButton_cancel;
			pbButtonCancel.Location = new Point(n*74,n*2);
			pbButtonCancel.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbButtonCancel);

			Bitmap bmFull_0 = (Bitmap)resources.GetObject("full-0");
			treeIconList.Images.Add(bmFull_0);
			iconNameList.Add("full-0");
			pbFull_0.Size = new Size(n*16,n*16);
			pbFull_0.SizeMode = PictureBoxSizeMode.StretchImage;
			pbFull_0.Image = bmFull_0;
			pbFull_0.Location = new Point(n*92,n*2);
			pbFull_0.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbFull_0);

			Bitmap bmFull_1 = (Bitmap)resources.GetObject("full-1");
			treeIconList.Images.Add(bmFull_1);
			iconNameList.Add("full-1");
			pbFull_1.Size = new Size(n*16,n*16);
			pbFull_1.SizeMode = PictureBoxSizeMode.StretchImage;
			pbFull_1.Image = bmFull_1;
			pbFull_1.Location = new Point(n*110,n*2);
			pbFull_1.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbFull_1);

			Bitmap bmFull_2 = (Bitmap)resources.GetObject("full-2");
			treeIconList.Images.Add(bmFull_2);
			iconNameList.Add("full-2");
			pbFull_2.Size = new Size(n*16,n*16);
			pbFull_2.SizeMode = PictureBoxSizeMode.StretchImage;
			pbFull_2.Image = bmFull_2;
			pbFull_2.Location = new Point(n*2,n*20);
			pbFull_2.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbFull_2);

			Bitmap bmFull_3 = (Bitmap)resources.GetObject("full-3");
			treeIconList.Images.Add(bmFull_3);
			iconNameList.Add("full-3");
			pbFull_3.Size = new Size(n*16,n*16);
			pbFull_3.SizeMode = PictureBoxSizeMode.StretchImage;
			pbFull_3.Image = bmFull_3;
			pbFull_3.Location = new Point(n*20,n*20);
			pbFull_3.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbFull_3);

			Bitmap bmFull_4 = (Bitmap)resources.GetObject("full-4");
			treeIconList.Images.Add(bmFull_4);
			iconNameList.Add("full-4");
			pbFull_4.Size = new Size(n*16,n*16);
			pbFull_4.SizeMode = PictureBoxSizeMode.StretchImage;
			pbFull_4.Image = bmFull_4;
			pbFull_4.Location = new Point(n*38,n*20);
			pbFull_4.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbFull_4);

			Bitmap bmFull_5 = (Bitmap)resources.GetObject("full-5");
			treeIconList.Images.Add(bmFull_5);
			iconNameList.Add("full-5");
			pbFull_5.Size = new Size(n*16,n*16);
			pbFull_5.SizeMode = PictureBoxSizeMode.StretchImage;
			pbFull_5.Image = bmFull_5;
			pbFull_5.Location = new Point(n*56,n*20);
			pbFull_5.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbFull_5);

			Bitmap bmFull_6 = (Bitmap)resources.GetObject("full-6");
			treeIconList.Images.Add(bmFull_6);
			iconNameList.Add("full-6");
			pbFull_6.Size = new Size(n*16,n*16);
			pbFull_6.SizeMode = PictureBoxSizeMode.StretchImage;
			pbFull_6.Image = bmFull_6;
			pbFull_6.Location = new Point(n*74,n*20);
			pbFull_6.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbFull_6);

			Bitmap bmFull_7 = (Bitmap)resources.GetObject("full-7");
			treeIconList.Images.Add(bmFull_7);
			iconNameList.Add("full-7");
			pbFull_7.Size = new Size(n*16,n*16);
			pbFull_7.SizeMode = PictureBoxSizeMode.StretchImage;
			pbFull_7.Image = bmFull_7;
			pbFull_7.Location = new Point(n*92,n*20);
			pbFull_7.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbFull_7);

			Bitmap bmFull_8 = (Bitmap)resources.GetObject("full-8");
			treeIconList.Images.Add(bmFull_8);
			iconNameList.Add("full-8");
			pbFull_8.Size = new Size(n*16,n*16);
			pbFull_8.SizeMode = PictureBoxSizeMode.StretchImage;
			pbFull_8.Image = bmFull_8;
			pbFull_8.Location = new Point(n*110,n*20);
			pbFull_8.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbFull_8);

			Bitmap bmFull_9 = (Bitmap)resources.GetObject("full-9");
			treeIconList.Images.Add(bmFull_9);
			iconNameList.Add("full-9");
			pbFull_9.Size = new Size(n*16,n*16);
			pbFull_9.SizeMode = PictureBoxSizeMode.StretchImage;
			pbFull_9.Image = bmFull_9;
			pbFull_9.Location = new Point(n*2,n*38);
			pbFull_9.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbFull_9);

			Bitmap bmBack = (Bitmap)resources.GetObject("back");
			treeIconList.Images.Add(bmBack);
			iconNameList.Add("back");
			pbBack.Size = new Size(n*16,n*16);
			pbBack.SizeMode = PictureBoxSizeMode.StretchImage;
			pbBack.Image = bmBack;
			pbBack.Location = new Point(n*20,n*38);
			pbBack.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbBack);

			Bitmap bmForward = (Bitmap)resources.GetObject("forward");
			treeIconList.Images.Add(bmForward);
			iconNameList.Add("forward");
			pbForward.Size = new Size(n*16,n*16);
			pbForward.SizeMode = PictureBoxSizeMode.StretchImage;
			pbForward.Image = bmForward;
			pbForward.Location = new Point(n*38,n*38);
			pbForward.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbForward);

			Bitmap bmAttach = (Bitmap)resources.GetObject("attach");
			treeIconList.Images.Add(bmAttach);
			iconNameList.Add("attach");
			pbAttach.Size = new Size(n*16,n*16);
			pbAttach.SizeMode = PictureBoxSizeMode.StretchImage;
			pbAttach.Image = bmAttach;
			pbAttach.Location = new Point(n*56,n*38);
			pbAttach.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbAttach);

			Bitmap bmKsmiletris = (Bitmap)resources.GetObject("ksmiletris");
			treeIconList.Images.Add(bmKsmiletris);
			iconNameList.Add("ksmiletris");
			pbKsmiletris.Size = new Size(n*16,n*16);
			pbKsmiletris.SizeMode = PictureBoxSizeMode.StretchImage;
			pbKsmiletris.Image = bmKsmiletris;
			pbKsmiletris.Location = new Point(n*74,n*38);
			pbKsmiletris.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbKsmiletris);

			Bitmap bmClanbomber = (Bitmap)resources.GetObject("clanbomber");
			treeIconList.Images.Add(bmClanbomber);
			iconNameList.Add("clanbomber");
			pbClanbomber.Size = new Size(n*16,n*16);
			pbClanbomber.SizeMode = PictureBoxSizeMode.StretchImage;
			pbClanbomber.Image = bmClanbomber;
			pbClanbomber.Location = new Point(n*92,n*38);
			pbClanbomber.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbClanbomber);

			Bitmap bmDesktop_new = (Bitmap)resources.GetObject("desktop_new");
			treeIconList.Images.Add(bmDesktop_new);
			iconNameList.Add("desktop_new");
			pbDesktop_new.Size = new Size(n*16,n*16);
			pbDesktop_new.SizeMode = PictureBoxSizeMode.StretchImage;
			pbDesktop_new.Image = bmDesktop_new;
			pbDesktop_new.Location = new Point(n*110,n*38);
			pbDesktop_new.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbDesktop_new);

			Bitmap bmFlag = (Bitmap)resources.GetObject("flag");
			treeIconList.Images.Add(bmFlag);
			iconNameList.Add("flag");
			pbFlag.Size = new Size(n*16,n*16);
			pbFlag.SizeMode = PictureBoxSizeMode.StretchImage;
			pbFlag.Image = bmFlag;
			pbFlag.Location = new Point(n*2,n*56);
			pbFlag.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbFlag);

			Bitmap bmGohome = (Bitmap)resources.GetObject("gohome");
			treeIconList.Images.Add(bmGohome);
			iconNameList.Add("gohome");
			pbGohome.Size = new Size(n*16,n*16);
			pbGohome.SizeMode = PictureBoxSizeMode.StretchImage;
			pbGohome.Image = bmGohome;
			pbGohome.Location = new Point(n*20,n*56);
			pbGohome.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbGohome);

			Bitmap bmKaddressbook = (Bitmap)resources.GetObject("kaddressbook");
			treeIconList.Images.Add(bmKaddressbook);
			iconNameList.Add("kaddressbook");
			pbKaddressbook.Size = new Size(n*16,n*16);
			pbKaddressbook.SizeMode = PictureBoxSizeMode.StretchImage;
			pbKaddressbook.Image = bmKaddressbook;
			pbKaddressbook.Location = new Point(n*38,n*56);
			pbKaddressbook.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbKaddressbook);

			Bitmap bmKnotify = (Bitmap)resources.GetObject("knotify");
			treeIconList.Images.Add(bmKnotify);
			iconNameList.Add("knotify");
			pbKnotify.Size = new Size(n*16,n*16);
			pbKnotify.SizeMode = PictureBoxSizeMode.StretchImage;
			pbKnotify.Image = bmKnotify;
			pbKnotify.Location = new Point(n*56,n*56);
			pbKnotify.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbKnotify);

			Bitmap bmKorn = (Bitmap)resources.GetObject("korn");
			treeIconList.Images.Add(bmKorn);
			iconNameList.Add("korn");
			pbKorn.Size = new Size(n*16,n*16);
			pbKorn.SizeMode = PictureBoxSizeMode.StretchImage;
			pbKorn.Image = bmKorn;
			pbKorn.Location = new Point(n*74,n*56);
			pbKorn.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbKorn);

			Bitmap bmMail = (Bitmap)resources.GetObject("Mail");
			treeIconList.Images.Add(bmMail);
			iconNameList.Add("Mail");
			pbMail.Size = new Size(n*16,n*16);
			pbMail.SizeMode = PictureBoxSizeMode.StretchImage;
			pbMail.Image = bmMail;
			pbMail.Location = new Point(n*92,n*56);
			pbMail.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbMail);

			Bitmap bmPassword = (Bitmap)resources.GetObject("password");
			treeIconList.Images.Add(bmPassword);
			iconNameList.Add("password");
			pbPassword.Size = new Size(n*16,n*16);
			pbPassword.SizeMode = PictureBoxSizeMode.StretchImage;
			pbPassword.Image = bmPassword;
			pbPassword.Location = new Point(n*110,n*56);
			pbPassword.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbPassword);

			Bitmap bmPencil = (Bitmap)resources.GetObject("pencil");
			treeIconList.Images.Add(bmPencil);
			iconNameList.Add("pencil");
			pbPencil.Size = new Size(n*16,n*16);
			pbPencil.SizeMode = PictureBoxSizeMode.StretchImage;
			pbPencil.Image = bmPencil;
			pbPencil.Location = new Point(n*2,n*74);
			pbPencil.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbPencil);

			Bitmap bmStop = (Bitmap)resources.GetObject("stop");
			treeIconList.Images.Add(bmStop);
			iconNameList.Add("stop");
			pbStop.Size = new Size(n*16,n*16);
			pbStop.SizeMode = PictureBoxSizeMode.StretchImage;
			pbStop.Image = bmStop;
			pbStop.Location = new Point(n*20,n*74);
			pbStop.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbStop);

			Bitmap bmWizard = (Bitmap)resources.GetObject("wizard");
			treeIconList.Images.Add(bmWizard);
			iconNameList.Add("wizard");
			pbWizard.Size = new Size(n*16,n*16);
			pbWizard.SizeMode = PictureBoxSizeMode.StretchImage;
			pbWizard.Image = bmWizard;
			pbWizard.Location = new Point(n*38,n*74);
			pbWizard.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbWizard);

			Bitmap bmXmag = (Bitmap)resources.GetObject("xmag");
			treeIconList.Images.Add(bmXmag);
			iconNameList.Add("xmag");
			pbXmag.Size = new Size(n*16,n*16);
			pbXmag.SizeMode = PictureBoxSizeMode.StretchImage;
			pbXmag.Image = bmXmag;
			pbXmag.Location = new Point(n*56,n*74);
			pbXmag.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbXmag);

			Bitmap bmBell = (Bitmap)resources.GetObject("bell");
			treeIconList.Images.Add(bmBell);
			iconNameList.Add("bell");
			pbBell.Size = new Size(n*16,n*16);
			pbBell.SizeMode = PictureBoxSizeMode.StretchImage;
			pbBell.Image = bmBell;
			pbBell.Location = new Point(n*74,n*74);
			pbBell.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbBell);

			Bitmap bmBookmark = (Bitmap)resources.GetObject("bookmark");
			treeIconList.Images.Add(bmBookmark);
			iconNameList.Add("bookmark");
			pbBookmark.Size = new Size(n*16,n*16);
			pbBookmark.SizeMode = PictureBoxSizeMode.StretchImage;
			pbBookmark.Image = bmBookmark;
			pbBookmark.Location = new Point(n*92,n*74);
			pbBookmark.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbBookmark);

			Bitmap bmPenguin = (Bitmap)resources.GetObject("penguin");
			treeIconList.Images.Add(bmPenguin);
			iconNameList.Add("penguin");
			pbPenguin.Size = new Size(n*16,n*16);
			pbPenguin.SizeMode = PictureBoxSizeMode.StretchImage;
			pbPenguin.Image = bmPenguin;
			pbPenguin.Location = new Point(n*110,n*74);
			pbPenguin.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbPenguin);

			Bitmap bmLicq = (Bitmap)resources.GetObject("licq");
			treeIconList.Images.Add(bmLicq);
			iconNameList.Add("licq");
			pbLicq.Size = new Size(n*16,n*16);
			pbLicq.SizeMode = PictureBoxSizeMode.StretchImage;
			pbLicq.Image = bmLicq;
			pbLicq.Location = new Point(n*2,n*92);
			pbLicq.Click += new System.EventHandler(this.clickedOnAnIconToAdd);
			panelAddIcons.Controls.Add(pbLicq);

#if GRAPHICAL
			iconsDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("FYMgraphicsIcon")));
			iconsDialog.BackColor = backColor;
#else
			iconsDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("FYMIcon")));
#endif
			iconsDialog.Controls.Add(labelAdd);
			iconsDialog.Controls.Add(panelAddIcons);
			iconsDialog.Text = t.t[t.AddIcon, language];
		}

		public void clickedOnAnIconToAdd(object sender, System.EventArgs e)
		{
			if (sender == pbHelp)
				iconName = "help";
			else if (sender == pbMessageboxWarning)
				iconName = "messagebox_warning";
			else if (sender == pbIdea)
				iconName = "idea";
			else if (sender == pbButtonOk)
				iconName = "button_ok";
			else if (sender == pbButtonCancel)
				iconName = "button_cancel";
			else if (sender == pbFull_0)
				iconName = "full-0";
			else if (sender == pbFull_1)
				iconName = "full-1";
			else if (sender == pbFull_2)
				iconName = "full-2";
			else if (sender == pbFull_3)
				iconName = "full-3";
			else if (sender == pbFull_4)
				iconName = "full-4";
			else if (sender == pbFull_5)
				iconName = "full-5";
			else if (sender == pbFull_6)
				iconName = "full-6";
			else if (sender == pbFull_7)
				iconName = "full-7";
			else if (sender == pbFull_8)
				iconName = "full-8";
			else if (sender == pbFull_9)
				iconName = "full-9";
			else if (sender == pbBack)
				iconName = "back";
			else if (sender == pbForward)
				iconName = "forward";
			else if (sender == pbAttach)
				iconName = "attach";
			else if (sender == pbKsmiletris)
				iconName = "ksmiletris";
			else if (sender == pbClanbomber)
				iconName = "clanbomber";
			else if (sender == pbDesktop_new)
				iconName = "desktop_new";
			else if (sender == pbFlag)
				iconName = "flag";
			else if (sender == pbGohome)
				iconName = "gohome";
			else if (sender == pbKaddressbook)
				iconName = "kaddressbook";
			else if (sender == pbKnotify)
				iconName = "knotify";
			else if (sender == pbKorn)
				iconName = "korn";
			else if (sender == pbMail)
				iconName = "Mail";
			else if (sender == pbPassword)
				iconName = "password";
			else if (sender == pbPencil)
				iconName = "pencil";
			else if (sender == pbStop)
				iconName = "stop";
			else if (sender == pbWizard)
				iconName = "wizard";
			else if (sender == pbXmag)
				iconName = "xmag";
			else if (sender == pbBell)
				iconName = "bell";
			else if (sender == pbBookmark)
				iconName = "bookmark";
			else if (sender == pbPenguin)
				iconName = "penguin";
			else if (sender == pbLicq)
				iconName = "licq";
			else
				iconName = "";

			iconsDialog.DialogResult = DialogResult.OK;
		}

		public string getIconName()
		{
			iconName = "";

			iconsDialog.ShowDialog();  // Dialog anzeigen

			return(iconName);
		}

		public int FindIndexOf(string str)
		{
			if (str.StartsWith("<icon BUILTIN="))
		    {
				string subStr = str.Substring(str.IndexOf('"') + 1);

				for (int i = 2; i < iconNameList.Count; i++)  // Default und Dummy sind die ersten beiden
				{
					if (subStr.StartsWith(iconNameList[i]))
						return(i);
				}
		    }

			return(1);  // Dummy wenn das Icon nicht bekannt ist
		}
	}
}
