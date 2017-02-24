/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schmid_p
 * Datum: 13.03.2007
 * Zeit: 14:02
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FreeYourMind
{
	/// <summary>
	/// Description of Settings.
	/// </summary>
	public class Settings
	{
		private string strPlatform;
		private string strDeviceId;
#if DEBUG
#if GRAPHICAL
		private const string iniFilePath = "C:\\Users\\Peter\\Projects\\Application Data\\FYMgraphics\\FYMgraphics.ini";
		private const string unlockFilePath = "C:\\Users\\Peter\\Projects\\Application Data\\FYMgraphics\\Unlock.txt";
#else
		private const string iniFilePath = "C:\\Users\\Peter\\Projects\\Application Data\\FreeYourMind\\FYM.ini";
#endif
#else
#if GRAPHICAL
		private const string iniFilePath = "\\Application Data\\FYMgraphics\\FYMgraphics.ini";
		private const string unlockFilePath = "\\Application Data\\FYMgraphics\\Unlock.txt";
#else
		private const string iniFilePath = "\\Application Data\\FreeYourMind\\FYM.ini";
#endif
#endif
//		private const string strDisableRecentFiles = "[DisableRecentFiles]";
		private const string strRecentFiles         = "[RecentFiles]";
		private const string strOpenRecentAtStartup = "[OpenRecentAtStartup]";
#if !GRAPHICAL
		private const string strShowIcons           = "[ShowIcons]";
		private const string strColouredNodes       = "[ColouredNodes]";
#endif
#if GRAPHICAL
		private const string strLanguage            = "[Language]";
		private const string strNavigator           = "[Navigator]";
		private const string strNodeInfo            = "[NodeInfo]";
		private const string strQuickContext        = "[QuickContext]";
#endif
		private const string strFontSize            = "[FontSize]";
//		private bool disableRecentFiles;
		private string openRecentAtStartup;
		private List<string> recentFilesList = new List<string>(4);
#if !GRAPHICAL
		private bool showIcons;
		private bool colouredNodes;
#endif
#if GRAPHICAL
		private string language;
		private bool navigator;
		private bool nodeInfo;
		private bool quickContext;
		private bool unlocked;
#endif
		private string fontSize;

		private bool theVeryFirstTime;

		public Settings()
		{
			ExploreDevice();  // Platform und DeviceId

			// vorbelegen
			InitSettingsParameters();

#if GRAPHICAL
			unlocked = false;
			theVeryFirstTime = !File.Exists(iniFilePath);

			if (File.Exists(unlockFilePath))
		    {
	            using (StreamReader sr = new StreamReader(unlockFilePath))
	            {
	            	string str = sr.ReadLine();

	            	if (Unlock.CheckValidCodeA(str) == Unlock.NOERROR)
	            		unlocked = true;

	            	sr.Close();
	            }
    		}
#endif

			// überschreiben mit ini-Datei
			if (File.Exists(iniFilePath))
		    {
		    	try
		    	{
		            using (StreamReader sr = new StreamReader(iniFilePath))
		            {
		                string str;
		                int state = 0;

		                while ((str = sr.ReadLine()) != null)
		                {
		                	switch (str)
		                	{
//			                	case strDisableRecentFiles:
//		                			state = 1;
//			                		break;
			                	case strRecentFiles:
		                			state = 2;
			                		break;
			                	case strOpenRecentAtStartup:
		                			state = 3;
			                		break;
#if !GRAPHICAL
			                	case strShowIcons:
		                			state = 4;
			                		break;
			                	case strColouredNodes:
		                			state = 5;
			                		break;
#endif
#if GRAPHICAL
			                	case strLanguage:
		                			state = 6;
			                		break;
#endif
			                	case strFontSize:
		                			state = 7;
			                		break;
#if GRAPHICAL
			                	case strNavigator:
		                			state = 8;
			                		break;

			                	case strNodeInfo:
		                			state = 9;
			                		break;

			                	case strQuickContext:
		                			state = 10;
			                		break;
#endif
			                	default:
			                		switch (state)
			                		{
//			                			case 1:
//			                				state = 0;
//			                				if (str == "true")
//			                					disableRecentFiles = true;
//			                				else
//			                					disableRecentFiles = false;
//			                				break;
			                			case 2:
			                				if ((str == "") || (str.StartsWith("[")))
			                					state = 0;
			                				else
			                					recentFilesList.Add(str);
			                				break;
			                			case 3:
			                				state = 0;
			                				openRecentAtStartup = str;
			                				break;
#if !GRAPHICAL
			                			case 4:
			                				state = 0;
			                				if (str == "true")
			                					showIcons = true;
			                				else
			                					showIcons = false;
			                				break;
			                			case 5:
			                				state = 0;
			                				if (str == "true")
			                					colouredNodes = true;
			                				else
			                					colouredNodes = false;
			                				break;
#endif
#if GRAPHICAL
			                			case 6:
			                				state = 0;
		                					language = str;
			                				break;
#endif
			                			case 7:
			                				state = 0;
		                					fontSize = str;
			                				break;
#if GRAPHICAL
			                			case 8:
			                				state = 0;
			                				if (str == "true")
			                					navigator = true;
			                				else
			                					navigator = false;
			                				break;

			                			case 9:
			                				state = 0;
			                				if (str == "true")
			                					nodeInfo = true;
			                				else
			                					nodeInfo = false;
			                				break;

			                			case 10:
			                				state = 0;
			                				if (str == "true")
			                					quickContext = true;
			                				else
			                					quickContext = false;
			                				break;
#endif
			                			default:
			                				break;
			                		}
			                		break;
		                	}
		                }
		            }
		    	}
		    	catch
		    	{
					InitSettingsParameters();
					File.Delete(iniFilePath);
		    	}
		    }
		}
		
		private void InitSettingsParameters()
		{
//			disableRecentFiles = false;
			recentFilesList.Clear();
			openRecentAtStartup = "true";
#if GRAPHICAL
			language = "English";
			navigator = true;
			nodeInfo = false;
			quickContext = true;
#else
			showIcons = true;
			colouredNodes = true;
#endif
			fontSize = "2";
		}

		public void SetMostRecentPath(string fileName)
		{
			// checken, ob es den Path in der Liste schon gibt
			for (int i = 0; i < recentFilesList.Count; i++)
			{
				if (recentFilesList[i] == fileName)
				{
					// entfernen, wenn er existiert
					recentFilesList.RemoveAt(i);
					break;
				}
			}

			// Path auf oberster Position hinzufügen
			recentFilesList.Insert(0,fileName);

			// Liste auf max. 4 Einträge kürzen
			while (recentFilesList.Count > 4)
			{
				recentFilesList.RemoveAt(4);
			}

			// und gleich wegspeichern
			Save();
		}

#if GRAPHICAL
		public void SaveUnlock(string keyCode)
		{
			if (!Directory.Exists(Path.GetDirectoryName(unlockFilePath)))
				Directory.CreateDirectory(Path.GetDirectoryName(unlockFilePath));

			using (StreamWriter sw = new StreamWriter(unlockFilePath, false))
	        {
	        	sw.WriteLine(keyCode);
	        }

	        unlocked = true;
		}
#endif

		public void Save()
		{
			if (!Directory.Exists(Path.GetDirectoryName(iniFilePath)))
				Directory.CreateDirectory(Path.GetDirectoryName(iniFilePath));

	        using (StreamWriter sw = new StreamWriter(iniFilePath, false)) 
	        {
//	        	sw.WriteLine(strDisableRecentFiles);
//	        	if (disableRecentFiles)
//	        		sw.WriteLine("true");
//	        	else
//	        		sw.WriteLine("false");

	        	sw.WriteLine(strRecentFiles);
	        	for (int i = 0; i < recentFilesList.Count; i++)
	        		sw.WriteLine(recentFilesList[i]);

	        	sw.WriteLine(strOpenRecentAtStartup);
        		sw.WriteLine(openRecentAtStartup);
#if !GRAPHICAL
	        	sw.WriteLine(strShowIcons);
	        	if (showIcons)
	        		sw.WriteLine("true");
	        	else
	        		sw.WriteLine("false");

	        	sw.WriteLine(strColouredNodes);
	        	if (colouredNodes)
	        		sw.WriteLine("true");
	        	else
	        		sw.WriteLine("false");
#endif
#if GRAPHICAL
	        	sw.WriteLine(strLanguage);
        		sw.WriteLine(language);

        		sw.WriteLine(strNavigator);
	        	if (navigator)
	        		sw.WriteLine("true");
	        	else
	        		sw.WriteLine("false");

        		sw.WriteLine(strNodeInfo);
	        	if (nodeInfo)
	        		sw.WriteLine("true");
	        	else
	        		sw.WriteLine("false");

        		sw.WriteLine(strQuickContext);
	        	if (quickContext)
	        		sw.WriteLine("true");
	        	else
	        		sw.WriteLine("false");
#endif
	        	sw.WriteLine(strFontSize);
        		sw.WriteLine(fontSize);
	        }
		}

//		public bool DisableRecentFiles
//		{
//			set { disableRecentFiles = value; }
//			get { return(disableRecentFiles); }
//		}

#if GRAPHICAL
		public bool Unlocked
		{
			get { return(unlocked); }
		}

		public bool TheVeryFirstTime
		{
			get { return(theVeryFirstTime); }
		}
#endif

		public int Language
		{
#if GRAPHICAL
			set
			{
				if (value == 0)
					language = "Deutsch";
				else
				{
					if (value == 1)
						language = "English";
					else
					{
						if (value == 2)
							language = "Français";
						else
						{
							if (value == 3)
								language = "Italiano";
							else
								language = "Nederlands";
						}
					}
				}
				Save();
			}
			get
			{
				if (language == "Deutsch")
					return(0);
				else
				{
					if (language == "English")
						return(1);
					else
					{
						if (language == "Français")
							return(2);
						else
						{
							if (language == "Italiano")
								return(3);
							else
								return(4);
						}
					}
				}
			}
#else
//			set { language = "English"; Save(); }
			get { return(1); }  // Englisch
#endif
		}

		public int FontSize
		{
			set
			{
				fontSize = value.ToString();
				Save();
			}
			get
			{
				return(Convert.ToInt32(fontSize));
			}
		}

		public bool ShowIcons
		{
#if GRAPHICAL
			set { ; }
			get { return(true); }
#else
			set { showIcons = value; Save(); }
			get { return(showIcons); }
#endif
		}

#if GRAPHICAL
		public bool Navigator
		{
			set { navigator = value; Save(); }
			get { return(navigator); }
		}

		public bool NodeInfo
		{
			set { nodeInfo = value; Save(); }
			get { return(nodeInfo); }
		}

		public bool QuickContext
		{
			set { quickContext = value; Save(); }
			get { return(quickContext); }
		}
#endif

		public bool ColouredNodes
		{
#if GRAPHICAL
			set { ; }
			get { return(true); }
#else
			set { colouredNodes = value; Save(); }
			get { return(colouredNodes); }
#endif
		}

		public string OpenRecentAtStartup
		{
			set { openRecentAtStartup = value; Save(); }
			get { return(openRecentAtStartup); }
		}

		public void UpdateOpenRecentAtStartup(bool mindmapLoaded)
		{
			if (!mindmapLoaded)
			{
				if (openRecentAtStartup == "true")
				{
					openRecentAtStartup = "true_no_file_loaded";
					Save();
				}
			}
			else
			{
				if (openRecentAtStartup == "true_no_file_loaded")
				{
					openRecentAtStartup = "true";
					Save();
				}
			}
		}

		public int nrOfRecentFiles
		{
			get
			{
//				if (DisableRecentFiles)
//					return(0);
//				else
					return (recentFilesList.Count);
			}
		}

		public void DeleteRecentPath(int index)
		{
			// wird z.B. gelöscht, wenn festgestellt wurde, dass es ihn nicht gibt
			recentFilesList.RemoveAt(index);

			// und gleich wegspeichern
			Save();
		}

		public string GetRecentPath(int index)
		{
			if (index >= nrOfRecentFiles)
				return("");
			else
				return(recentFilesList[index]);
		}

		public string GetPlatform
		{
			get { return(strPlatform); }
		}

		public string GetDeviceId
		{
			get { return(strDeviceId); }
		}

		private void ExploreDevice()
        {
			string str = Device.GetPlatform();

			switch (str)
			{
				case "Win32NT":
				case "Win32S":
				case "Win32Windows":
					strPlatform = "Desktop";
					break;
				case "Unknown":
					strPlatform = "unknown";
					break;
				default:
					strPlatform = str;
					break;
			}

			try
			{
				strDeviceId = Device.GetDeviceID();
			}
			catch
			{
				strDeviceId = "void";
			}
        }
	}
}
