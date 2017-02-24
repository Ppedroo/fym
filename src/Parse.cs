/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Peter
 * Datum: 01.03.2007
 * Zeit: 18:17
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;  // für NumberStyles
using System.Windows.Forms;

namespace FreeYourMind
{
	/// <summary>
	/// Description of Parse.
	/// </summary>
	public static class Parse
	{
//		public Parse()
//		{
//		}

		public const int NONE = 0;
		public const int HOOK = 1;
		public const int RICH = 2;

		public static int FindNodeStart(List<string> mapList, int index)
		{
			int indent = 1;

			if (!mapList[index].StartsWith("</node"))
		    {
				if (mapList[index].StartsWith("<node") && mapList[index].EndsWith("/>"))
				    return(index);
				else
		    		return(-1);
		    }

			while (true)
			{
				index--;

				if (mapList[index].StartsWith("<node"))
				{
					if (mapList[index].EndsWith("/>"))
				    {
				    	// indent +/- gleicht sich aus
				    }
				    else
				    	indent--;

				    if (indent == 0)
						break;
				}
				else
				{
					if (mapList[index].StartsWith("</node"))
						indent++;
				}
			}

			return(index);

		}

		public static int FindNodeEnd(List<string> mapList, int index)
		{
			int indent = 0;

			if (!mapList[index].StartsWith("<node"))
		    {
		    	return(-1);
		    }

			if (mapList[index].EndsWith("/>"))
				return(index);

			while (true)
			{
				index++;

				if (mapList[index].StartsWith("</node"))
				{
					if (indent == 0)
						break;
					else
						indent--;
				}
				else
				{
					if (mapList[index].StartsWith("<node"))
					{
					    if (mapList[index].EndsWith("/>"))
					    {
					    	// indent +/- gleicht sich aus
					    }
					    else
					    	indent++;
					}
				}
			}

			return(index);
		}

		public static int SplitNode(List<string> mapList, int index)
		{
			if (mapList[index].StartsWith("<node") && mapList[index].EndsWith("/>"))
			{
				mapList[index] = mapList[index].TrimEnd('/','>');
				mapList[index] = mapList[index] + ">";

				index++;

				mapList.Insert(index, "</node>");

				return(index);
			}
			else
				return(-1);
		}

		public static int JoinNode(List<string> mapList, int index)
		{
			int stopIndex = FindNodeEnd(mapList, index);

			if (stopIndex == (index+1))
			{
		     	mapList.RemoveAt(stopIndex);
				mapList[index] = mapList[index].TrimEnd('>');
				mapList[index] = mapList[index] + "/>";

				return(index);
			}
			else
				return(-1);
		}

		public static bool ThisIsTheRoot(List<string> mapList, int index)
		{
			if (!mapList[index].StartsWith("<node"))
				return(false);

			while(index > 0)
			{
				index--;

				if (mapList[index].StartsWith("<node"))
					return(false);
			}

			return(true);
		}

		public static int FindLastIcon(List<string> mapList, int startIndex)
		{
			int stopIndex = FindNodeEnd(mapList, startIndex);  // Ende des Knotens finden
			int lastIconIndex = -1;                            // Index des letzten gefundenen Icons

			if (startIndex != stopIndex)  // nur mehrzeilige Knoten können Icons haben
			{
				for (int i = startIndex+1; i < stopIndex; i++)  // Knoteninneres absuchen
				{
					if (mapList[i].StartsWith("<node") ||  // Childknoten beginnt, oder
					    mapList[i].StartsWith("</node"))   // Knoten ist zu Ende
					{
						break;
					}
	
					if (mapList[i].StartsWith("<icon"))  // Iconposition merken
						lastIconIndex = i;
				}
			}

			return(lastIconIndex);
		}

		public static bool HasIcons(List<string> mapList, int startIndex)
		{
			if (FindLastIcon(mapList, startIndex) == -1)
				return(false);
			else
				return(true);
		}

		public static void CharToXml(ref string str)  // reservierte XML-Zeichen ummappen
		{
			str = str.Replace("&" ,"&amp;");  // muss zuerst kommen
			str = str.Replace("'" ,"&apos;");
			str = str.Replace(">" ,"&gt;");
			str = str.Replace("<" ,"&lt;");
			str = str.Replace("\"","&quot;");
		}

		public static void XmlToChar(ref string str)  // reservierte XML-Zeichen ummappen
		{
			str = str.Replace("&quot;","\"");
			str = str.Replace("&lt;"  ,"<");
			str = str.Replace("&gt;"  ,">");
			str = str.Replace("&apos;","'");
			str = str.Replace("&amp;" ,"&");  // muss zuletzt kommen
		}

		public static void InsertChildNodes(List<string> mapList, int startIndex, List<string> nodeList)
		{
			int stopIndex = FindNodeEnd(mapList, startIndex);

			if (startIndex == stopIndex)
				stopIndex = SplitNode(mapList, startIndex);

			mapList.InsertRange(stopIndex, nodeList);
			
			return;
		}

		public static void RemoveChildNodes(List<string> mapList, int startIndex, int stopIndex)
		{
			bool found = false;
			int i;

			for (i = startIndex+1; i < stopIndex; i++)
			{
				if (mapList[i].StartsWith("<node"))
				{
					found = true;
					break;
				}
			}

			if (!found) return;

			mapList.RemoveRange(i, stopIndex-i);

			JoinNode(mapList, startIndex);
		}

		public static int NewSister(List<string> mapList, int index, string strLabel)
		{
			if (ThisIsTheRoot(mapList, index))
				return(-1);

        	CharToXml(ref strLabel);  // special characters ummappen

			string strC = GetCurrentDateUtc().ToString();
			string strM = " MODIFIED=\"" + strC + "\"";
			strC = " CREATED=\"" + strC + "\"";

			mapList.Insert(index, "<node" + strC + strM + " TEXT=\"" + strLabel + "\"/>");
			
			return(index+1);
		}

		public static int NewChild(List<string> mapList, int startIndex, string strLabel)
		{
			int stopIndex = FindNodeEnd(mapList, startIndex);

			if (startIndex == stopIndex)
				stopIndex = SplitNode(mapList, startIndex);

        	CharToXml(ref strLabel);  // special characters ummappen

			string strC = GetCurrentDateUtc().ToString();
			string strM = " MODIFIED=\"" + strC + "\"";
			strC = " CREATED=\"" + strC + "\"";

			mapList.Insert(stopIndex, "<node" + strC + strM + " TEXT=\"" + strLabel + "\"/>");
			
			return(startIndex);
		}

		public static int NewParent(List<string> mapList, int startIndex, string strLabel)
		{
			if (ThisIsTheRoot(mapList, startIndex))
				return(-1);

        	CharToXml(ref strLabel);  // special characters ummappen

			int stopIndex = FindNodeEnd(mapList, startIndex);

			mapList.Insert(stopIndex+1, "</node>");

			string strC = GetCurrentDateUtc().ToString();
			string strM = " MODIFIED=\"" + strC + "\"";
			strC = " CREATED=\"" + strC + "\"";

			mapList.Insert(startIndex, "<node" + strC + strM + " TEXT=\"" + strLabel + "\">");
			
			return(startIndex+1);
		}
		
		public static int NewBrother(List<string> mapList, int index, string strLabel)
		{
			if (ThisIsTheRoot(mapList, index))
				return(-1);

        	CharToXml(ref strLabel);  // special characters ummappen

			int stopIndex = FindNodeEnd(mapList, index);

			string strC = GetCurrentDateUtc().ToString();
			string strM = " MODIFIED=\"" + strC + "\"";
			strC = " CREATED=\"" + strC + "\"";

			mapList.Insert(stopIndex+1, "<node" + strC + strM + " TEXT=\"" + strLabel + "\"/>");
			
			return(index);
		}

		public static bool IsEncrypted(List<string> mapList, int index)
		{
			if ((mapList[index].IndexOf(" PWD=\"") < 0) &&
			    (mapList[index].IndexOf(" ENCRYPTED_CONTENT=\"") > 0))
			{
				return(true);
			}
			else
			{
				return(false);
			}
		}

		public static bool IsDecrypted(List<string> mapList, int index)
		{
			if (mapList[index].IndexOf(" PWD=\"") > 0)
			{
				return(true);
			}
			else
			{
				return(false);
			}
		}

		public static string GetEncryptedContent(List<string> mapList, int index)
		{
			int i = mapList[index].IndexOf(" ENCRYPTED_CONTENT=\"") + 20;
			string str = mapList[index].Substring(i);
			i = str.IndexOf("\"");
			str = str.Substring(0,i);

			return(str);
		}

		public static string UpdateModificationTime(string strLine)
		{
			int i;
			string strFirst;
			string strLast;

			if ((i = strLine.IndexOf(" MODIFIED=\"")) >= 0)
			{
				strFirst = strLine.Substring(0,i+11);
				strLast = strLine.Substring(i+11);
				i = strLast.IndexOf("\"");
				strLast = strLast.Substring(i);
				return(strFirst + GetCurrentDateUtc().ToString() + strLast);
			}
			else
			{
				if ((i = strLine.IndexOf(" TEXT=\"")) > 0)
					return(strLine.Insert(i, " MODIFIED=\"" + GetCurrentDateUtc().ToString() + "\""));
				else
				{
					if (!strLine.EndsWith("/>"))
					{
						if ((i = strLine.IndexOf(">")) > 0)
							return(strLine.Insert(i, " MODIFIED=\"" + GetCurrentDateUtc().ToString() + "\""));
						else
							return(strLine);
					}
					else
						return(strLine);
				}
			}
		}

		public static void ReplaceText(List<string> mapList, int index, string str)
		{
			string strLine;   // die Zeile aus mapList
			string strFirst;  // der erste Teil bis TEXT="
			string strLast;   // der letzte Teil hinter dem Originaltext
			int    i;         // Hilfsindex im String

			strLine = mapList[index];

			i = strLine.IndexOf(" TEXT=\"") + 7;
			strFirst = strLine.Substring(0,i);
			strLast = strLine.Substring(i);
			i = strLast.IndexOf("\"");
			strLast = strLast.Substring(i);
			strLine = strFirst + str + strLast;

			mapList[index] = strLine;
		}

		public static void ReplaceFormat(List<string> mapList, int index, Color fc, Color bc)
		{
			string strLine;     // die Zeile aus mapList
			int    startindex;  // Hilfsindex im String
			int    stopindex;   // Hilfsindex im String

			strLine = mapList[index];

			if ((startindex = strLine.IndexOf(" COLOR=\"")) > 0)  // vorhandene Farbe entfernen
			{
				startindex++;
				stopindex = strLine.IndexOf("\"", startindex);
				stopindex = strLine.IndexOf("\"", stopindex+1);
				strLine = strLine.Remove(startindex, stopindex-startindex+2);
			}

			if ((startindex = strLine.IndexOf(" BACKGROUND_COLOR=\"")) > 0)  // vorhandene Farbe entfernen
			{
				startindex++;
				stopindex = strLine.IndexOf("\"", startindex);
				stopindex = strLine.IndexOf("\"", stopindex+1);
				strLine = strLine.Remove(startindex, stopindex-startindex+2);
			}

			string r;
			string g;
			string b;

			if (fc != Color.Transparent)  // Farbe eintragen
			{
				r = String.Format("{0:X2}", fc.R);
				g = String.Format("{0:X2}", fc.G);
				b = String.Format("{0:X2}", fc.B);
				strLine = strLine.Insert(6, "COLOR=\"#"+r+g+b+"\" ");
			}

			if (bc != Color.Transparent)  // Farbe eintragen
			{
				r = String.Format("{0:X2}", bc.R);
				g = String.Format("{0:X2}", bc.G);
				b = String.Format("{0:X2}", bc.B);
				strLine = strLine.Insert(6, "BACKGROUND_COLOR=\"#"+r+g+b+"\" ");
			}

			mapList[index] = strLine;
		}

		public static Color GetForeColor(List<string> mapList, int i)
		{
			int index;

			if ((index = mapList[i].IndexOf(" COLOR=\"")) > 0)
			{
				string str = mapList[i].Substring(index+9, 6);
				int r, g, b;

				try
				{
					r = int.Parse(str.Substring(0, 2), NumberStyles.AllowHexSpecifier);
					g = int.Parse(str.Substring(2, 2), NumberStyles.AllowHexSpecifier);
					b = int.Parse(str.Substring(4, 2), NumberStyles.AllowHexSpecifier);
				}
				catch
				{
					r = 0;
					g = 0;
					b = 0;
				}

				return(Color.FromArgb(r, g, b));
			}
			else
				return(Color.Transparent);  // auf Standard setzen
		}

		public static Color GetBackColor(List<string> mapList, int i)
		{
			int index;

			if ((index = mapList[i].IndexOf(" BACKGROUND_COLOR=\"")) > 0)
			{
				string str = mapList[i].Substring(index+20, 6);
				int r, g, b;

				try
				{
					r = int.Parse(str.Substring(0, 2), NumberStyles.AllowHexSpecifier);
					g = int.Parse(str.Substring(2, 2), NumberStyles.AllowHexSpecifier);
					b = int.Parse(str.Substring(4, 2), NumberStyles.AllowHexSpecifier);
				}
				catch
				{
					r = 255;
					g = 255;
					b = 255;
				}

				return(Color.FromArgb(r, g, b));
			}
			else
				return(Color.Transparent);  // auf Standard setzen
		}

		public static long GetCreationDate(List<string> mapList, int index)
		{
			return(DateWorker(mapList, index, " CREATED=\""));
		}

		public static long GetModificationDate(List<string> mapList, int index)
		{
			return(DateWorker(mapList, index, " MODIFIED=\""));
		}

		private static long DateWorker(List<string> mapList, int index, string strParam)
		{
			string strLine;  // die Zeile aus mapList
			int    i;        // Hilfsindex im String

			strLine = mapList[index];
			if ((i = strLine.IndexOf(strParam)) > 0)
			{
				strLine = strLine.Substring(i+strParam.Length);
				i = strLine.IndexOf("\"");
				strLine = strLine.Substring(0, i);
				return(Convert.ToInt64(strLine));
			}
			else
			{
				return(0);
			}
		}

		public static long GetCurrentDateUtc()
		{
			DateTime now = DateTime.UtcNow;
			return((long)now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
		}

		public static int FindNote(List<string> mapList, int index, ref int startIndex, ref int stopIndex)
		{
			int noteType = NONE;
			int stopNode = FindNodeEnd(mapList, index);

			if (stopNode == index) return(NONE);

			startIndex = index;
			stopIndex = index;

			for (int i = index+1; i < stopNode; i++)
			{
				if (mapList[i].StartsWith("<node"))
					return(NONE);

				if (mapList[i].StartsWith("<richcontent TYPE=\"NOTE\"><html>") ||
				    mapList[i].StartsWith("<hook NAME="))
					startIndex = i;

				if (mapList[i].StartsWith("</richcontent>") &&
				    (startIndex != index))
				{
					noteType = RICH;
					stopIndex = i;
					break;
				}

				if (mapList[i].StartsWith("</hook>"))
				{
					noteType = HOOK;
					stopIndex = i;
					break;
				}
			}

			if ((startIndex != index) && (stopIndex > startIndex))
				return(noteType);
			else
				return(NONE);
		}
		
		public static bool FindHtmlNode(List<string> mapList, int index, ref int startIndex, ref int stopIndex)
		{
			int stopNode = FindNodeEnd(mapList, index);

			if (stopNode == index) return(false);

			startIndex = index;
			stopIndex = index;

			for (int i = index+1; i < stopNode; i++)
			{
				if (mapList[i].StartsWith("<node"))
					return(false);

				if ((mapList[i].StartsWith("<richcontent TYPE=\"NODE\"><html>")) ||
				    (mapList[i].StartsWith("<richcontent>")))
					startIndex = i;

				if (mapList[i].StartsWith("</richcontent>") &&
				    (startIndex != index))
				{
					stopIndex = i;
					break;
				}
			}

			if ((startIndex != index) && (stopIndex > startIndex))
				return(true);
			else
				return(false);
		}

		public static string HtmlTextExtractor(List<string> mapList, int startIndex, int stopIndex, bool getAll)
		{
			bool bodyOpen = false;
			bool finish = false;
			bool writeWindowOpen = true;
			int j;
			string str;
			string returnStr = "";

			for (int i = startIndex+1; i <= stopIndex; i++)
			{
				str = mapList[i].TrimStart(null);  // remove whitespace
				if (!bodyOpen)
				{
					if (str.StartsWith("<body"))
						bodyOpen = true;
				}
				else
				{
					if (str.StartsWith("</body>"))
						finish = true;
					else
					{
						for (j = 0; j < str.Length; j++)
						{
							if (writeWindowOpen)
							{
								if (str[j] == '<')
									writeWindowOpen = false;
								else
									returnStr += str[j];
							}
							else
							{
								if (str[j] == '>')
								{
									writeWindowOpen = true;
#if GRAPHICAL
									if (!returnStr.EndsWith("\n"))
										returnStr += '\n';
#else
									if (!returnStr.EndsWith("|"))
										returnStr += '|';
#endif
								}
							}
							if ((returnStr.Length > 55) && (!getAll))
								finish = true;
						}
					}
				}
				if (finish) break;
			}

#if GRAPHICAL
			char[] a = {'\n', '\r', ' ', '\t'};
			returnStr = returnStr.TrimStart(a);
			returnStr = returnStr.TrimEnd(a);
#else
			if (returnStr.StartsWith("|"))
				returnStr = returnStr.TrimStart('|');

			if (returnStr.EndsWith("|"))
				returnStr = returnStr.TrimEnd('|');
#endif

			return(returnStr);
		}

		public static string ExtractTextFromHtml(List<string> mapList, int index)
		{
			int startIndex = index;
			int stopIndex = index;
			int startIndexDummy = 0;
			int stopIndexDummy = 0;
			string str;
			string returnStr = "";

			if (!Parse.FindHtmlNode(mapList, index, ref startIndex, ref stopIndex))
				return("[SYNTAX ERROR IN MINDMAP]");

			returnStr = HtmlTextExtractor(mapList, startIndex, stopIndex, false);

			// returnStr nachbearbeiten
			if (returnStr.Length > 50)
			{
				returnStr.Remove(50,returnStr.Length-50);
				returnStr += "...";
			}

			str = "[H";

			if (FindNote(mapList, index, ref startIndexDummy, ref stopIndexDummy) != NONE)
				str += "n";

			if (IsEncrypted(mapList, index))
				str += "e";

			if (IsDecrypted(mapList, index))
				str += "d";

			returnStr = str + "] " + returnStr;;

			return(returnStr);
		}

#if GRAPHICAL
		public static void ListToTree(List<string> mapList, MyTreeView treeView, Images images, bool colouredNodes, MyTreeNode nodeToSelect)
#else
		public static void ListToTree(List<string> mapList, TreeView treeView, Images images, bool colouredNodes, TreeNode nodeToSelect)
#endif
		{
			// Indexkette zum zu selektierenden Node aufbauen
			Stack<int> selNodeIndexStack = new Stack<int>();

			if (nodeToSelect != null)
			{
				while(true)
				{
					selNodeIndexStack.Push(nodeToSelect.Index);
					nodeToSelect = nodeToSelect.Parent;
					if (nodeToSelect == null) break;
				}
			}
			// Indexkette befindet sich nun in selNodeIndexStack

#if GRAPHICAL
			Stack<MyTreeNode> nodeStack = new Stack<MyTreeNode>();
			MyTreeNode node = null;

			Bitmap b = new Bitmap(10, 10);
			Graphics g = Graphics.FromImage(b);
			SizeF boxSize;
			Rectangle box;
#else
			Stack<TreeNode> nodeStack = new Stack<TreeNode>();
			TreeNode node = null;
#endif
			int i;
			int index = 0;
			int startIndexDummy = 0;
			int stopIndexDummy = 0;
			string str = "";
			List<int> nodeTraceList = new List<int>();  // mittracen für collapse/expand
			const int STEPOUT   = -10001;  // für die nodeTraceList
			const int STEPIN    = -10002;
			const int COLLAPSED = -1000;
			const int EXPANDED  = -1001;

			treeView.BeginUpdate();  // Treedarstellung einfrieren, im Hintergrund updaten

#if GRAPHICAL
			treeView.Clear(false);
#else
			treeView.Nodes.Clear();
#endif
	    	nodeTraceList.Clear();

			foreach (string line in mapList)
			{
				if (line.StartsWith("<node"))
				{
					if ((i = line.IndexOf(" TEXT=\"")) >= 0)  // plain Text extrahieren
					{
						i += 7;
						str = line.Substring(i);
						i = str.IndexOf("\"");
						str = str.Substring(0,i);
						if (FindNote(mapList, index, ref startIndexDummy, ref stopIndexDummy) != NONE)
						{
							if (IsEncrypted(mapList, index))
								str = "[ne] " + str;
							else
							{
								if (IsDecrypted(mapList, index))
									str = "[nd] " + str;
								else
									str = "[n] " + str;
							}
						}
						else
						{
							if (IsEncrypted(mapList, index))
								str = "[e] " + str;
							else
							{
								if (IsDecrypted(mapList, index))
									str = "[d] " + str;
							}
						}
					}
					else  // HTML formatierter Text
					{
						str = ExtractTextFromHtml(mapList,index);
					}

					XmlToChar(ref str);  // special characters ummappen

#if GRAPHICAL
					boxSize = g.MeasureString(str, treeView.GetFont(nodeStack.Count));
					box = new Rectangle(0, 0, (int)boxSize.Width+Const.textBoxOversize, (int)boxSize.Height);
#endif
					if (nodeStack.Count == 0)  // der Root-Node
				    {
#if GRAPHICAL
						node = treeView.AddNode(str, box,
						                        GetCreationDate(mapList, index),
						                        GetModificationDate(mapList, index),
						                        GetForeColor(mapList, index),
						                        GetBackColor(mapList, index));
#else
				    	node = treeView.Nodes.Add(str);
#endif
				    }
				    else                       // alle anderen Nodes
				    {
#if GRAPHICAL
						node = node.AddNode(str, box,
						                    GetCreationDate(mapList, index),
						                    GetModificationDate(mapList, index),
						                    GetForeColor(mapList, index),
						                    GetBackColor(mapList, index));
#else
				    	node = node.Nodes.Add(str);
#endif
				    }

					if (line.IndexOf(" FOLDED=\"true\" ") > 0)
				    	nodeTraceList.Add(COLLAPSED);
					else
				    	nodeTraceList.Add(EXPANDED);

					node.Tag = index;
				    nodeStack.Push(node);

#if !GRAPHICAL
					if (colouredNodes)
				    {
					    switch (nodeStack.Count)
					    {
					    	case 1:
					    		node.BackColor = Color.Black;
					    		node.ForeColor = Color.White;
					    		break;
					    	case 2:
					    		node.ForeColor = Color.Red;
					    		break;
					    	case 3:
					    		node.ForeColor = Color.Green;
					    		break;
					    	case 4:
					    		node.ForeColor = Color.Blue;
					    		break;
					    	default:
					    		break;
				    	}
				    }
#endif

				    if (line.EndsWith("/>"))
				    {
				        nodeStack.Pop();
				        if (nodeStack.Count > 0)
				        {
					        node = nodeStack.Peek();
				        }
				    }
				    else
				    {
				    	nodeTraceList.Add(STEPIN);  // eine Ebene tiefer
				    }
				}
				else if (line.StartsWith("</node"))
			    {
			        nodeStack.Pop();
			        if (nodeStack.Count > 0)
			        {
				        node = nodeStack.Peek();
			        }
			    	nodeTraceList.Add(STEPOUT);  // eine Ebene höher
			    }
				else if (line.StartsWith("<icon"))
			    {
					ChooseIcon(ref node, line, images);
				}
				
				index++;
			}

			// Hier kommt die expand / collapse - Geschichte, die mich
			// Stunden gekostet hat. Die Lösung war: expand / collapse
			// wirkt nur vernünftig, wenn der Tree fertig aufgebaut ist.
			// Jedes Hinzufügen eines Nodes macht wieder alles kaputt.
			int nodeIndex = -1;
			Stack<int> nodeIndexStack = new Stack<int>();
			bool rootNode = true;
			node = treeView.Nodes[0];
			nodeTraceList.ForEach(delegate(int listEntry)
					{
						switch (listEntry)
						{
							case STEPIN:
								if (rootNode)
									rootNode = false;
								else
									node = node.Nodes[nodeIndex];
								nodeIndexStack.Push(nodeIndex);
								nodeIndex = -1;
								break;
							case STEPOUT:
								nodeIndex = nodeIndexStack.Pop();
								node = node.Parent;
								break;
							case EXPANDED:
								if (rootNode)
									node.Expand();
								else
									node.Nodes[++nodeIndex].Expand();
								break;
							case COLLAPSED:
								if (rootNode)
									node.Collapse();
								else
									node.Nodes[++nodeIndex].Collapse();
								break;
						  	default:
						  		break;
						}
					});

			// ehemals selektierten Knoten (oder ggf. einen Nachbarn) wieder selektieren
			if (selNodeIndexStack.Count != 0)
			{
				bool success;
				int suchIndex;

				node = treeView.Nodes[selNodeIndexStack.Pop()];
				while(true)
				{
					if (selNodeIndexStack.Count == 0) break;

					suchIndex = selNodeIndexStack.Pop();
					success = false;
					
					while (!success)
					{
						try
						{
							node = node.Nodes[suchIndex];
							success = true;
						}
						catch
						{
							if (suchIndex > 0)
								suchIndex--;
							else
								success = true;
						}
					}
				}
				treeView.SelectedNode = node;
#if !GRAPHICAL
				treeView.SelectedNode.EnsureVisible();  // SelectedNode soll sichtbar sein
#endif
			}

#if GRAPHICAL
			// ListToTree() wird von verschiedensten Stellen aufgerufen.
			// Reaktion im Fehlerfall ist daher CollapseAll.
//			if (!treeView.EndUpdate(true))  // zu wenig Speicher um die Mindmap zu zeichnen
//			{
//				treeView.CollapseAll();
//				treeView.SelectedNode = treeView.Nodes[0];  // den Root-Knoten selektieren
//				treeView.EndUpdate(true);
//			}
			treeView.Layout();
			treeView.EnsureVisible(treeView.SelectedNode, true);  // hat Rieseneinfluss, aber noch mal überprüfen
#else
			treeView.EndUpdate();  // Treedarstellung wieder "live"
#endif
		}

#if GRAPHICAL
		private static void ChooseIcon(ref MyTreeNode node, string line, Images images)
#else
		private static void ChooseIcon(ref TreeNode node, string line, Images images)
#endif
		{
#if GRAPHICAL
			node.AddIcon(images.FindIndexOf(line));
#else
			node.ImageIndex = images.FindIndexOf(line);
			node.SelectedImageIndex = node.ImageIndex;
#endif
		}
	}
}
