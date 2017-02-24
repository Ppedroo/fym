/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Peter
 * Datum: 28.02.2007
 * Zeit: 17:40
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System.Drawing;
using System.Windows.Forms;

namespace FreeYourMind
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			//this.treeView1 = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			//this.treeView1.Location = new System.Drawing.Point(0, 0);
			//this.treeView1.Name = "treeView1";
			//this.treeView1.TabIndex = 2;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(238, 295);
			//this.Controls.Add(this.treeView1);
#if GRAPHICAL
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("FYMgraphicsIcon")));
#else
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("FYMIcon")));
#endif
			this.Name = "MainForm";
#if GRAPHICAL
			this.Text = "FYMgraphics";
#else
			this.Text = "FreeYourMind";
#endif
			this.ResumeLayout(false);
		}
		//private System.Windows.Forms.TreeView treeView1;
	}
}
