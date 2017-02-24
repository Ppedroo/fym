/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Peter
 * Datum: 10.07.2009
 * Zeit: 21:07
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */

using System;

namespace FreeYourMind
{
	/// <summary>
	/// Description of MyTreeParameters.
	/// </summary>
	public class MyTreeParameters
	{
		private int[] privFontSizeFirst   = {15, 14, 13, 12, 11};
		private int[] privFontSizeSecond  = {11, 10,  9,  8,  7};
		private int[] privFontSizeRegular = {11, 10,  9,  8,  7};
		private int[] privYSpacing     = {11, 10,  9,  8,  7};

		public int fontSizeFirst;
		public int fontSizeSecond;
		public int fontSizeRegular;
		public int ySpacing;

		public MyTreeParameters(int fontSize)
		{
			fontSizeFirst   = privFontSizeFirst[fontSize];
			fontSizeSecond  = privFontSizeSecond[fontSize];
			fontSizeRegular = privFontSizeRegular[fontSize];
//			ySpacing        = privYSpacing[fontSize];
		}
	}
}
