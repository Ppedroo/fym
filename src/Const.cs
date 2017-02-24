/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schmid_p
 * Datum: 20.02.2009
 * Zeit: 10:17
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace FreeYourMind
{
	/// <summary>
	/// Description of Const.
	/// </summary>
	public static class Const
	{
		public const int xMargin  = 15;
		public const int yMargin  = 15;

		public const int xSpacing = 15;
		public const int ySpacing = 9;

		public const int chamfer    = 4;
		public const int leftMargin = 4;

		public const int textBoxOversize = 6;

		public const int iconMargin = 1;

		public static int GetVGAFactor()
		{
#if DEBUG
			return(1);  // QVGA
#else
			if (Math.Max(Screen.PrimaryScreen.Bounds.Height, Screen.PrimaryScreen.Bounds.Width) >= 640)
				return(2);
			else
				return(1);
#endif
		}

		public static int GetMaxScreenWidth()
		{
#if DEBUG
			return(320);  // QVGA
#else
			return(Math.Max(Screen.PrimaryScreen.Bounds.Height, Screen.PrimaryScreen.Bounds.Width));
#endif
		}
	}
}
