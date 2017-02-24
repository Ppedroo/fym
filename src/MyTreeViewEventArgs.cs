/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Peter
 * Datum: 09.02.2009
 * Zeit: 18:59
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */

#if GRAPHICAL

using System;
using System.Windows.Forms;

namespace FreeYourMind
{
	/// <summary>
	/// Description of MyTreeViewEventArgs.
	/// </summary>
	public class MyTreeViewEventArgs : EventArgs
	{
		private MyTreeView treeView;

		public MyTreeViewEventArgs(MyTreeView tv)
		{
			treeView = tv;
		}

		public MyTreeNode Node
		{
			get { return treeView.SelectedNode; }
		}
	}
}
#endif
