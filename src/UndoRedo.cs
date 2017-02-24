/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Peter
 * Datum: 10.03.2007
 * Zeit: 00:38
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace FreeYourMind
{
	/// <summary>
	/// Description of UndoRedo.
	/// </summary>
	/*
	 *	cmdPtr			zeigt auf
	 * 					- den nächsten zu beschreibenden Platz, oder
	 * 					- den Redo-Platz, oder
	 * 					- eins über dem Undo-Platz
	 * 	firstCmdPtr		zeigt auf
	 * 					- das erste Kommando, oder
	 * 					- kein Kommando, wenn == lastCmdPtr
	 * 	lastCmdPtr		zeigt auf
	 * 					- eins über dem höchsten Kommando, oder
	 * 					- dieses nicht, wenn == firstCmdPtr
	 * 	command[]		enthält die Kommandos
	 * 	mapListPtr[]	zeigt auf den jeweils gemeinten Knoten (ist leider nicht immer so, deshalb gibt es helpIndex)
	 * 	helpIndex[]		zeigt in manchen Fällen auf den gemeinten Knoten TODO: aufräumen
	 * 	nrOfStrings[]	Anzahl der Strings, die für dieses Kommando in strList gespeichert wurden
	 * 	strListPtr		zeigt in der strList auf den jeweils relevanten Bereich
	 * 	strList			enthält die für das jeweilige Kommando gespeicherten Strings
	 */
	public class UndoRedo
	{
		private const int UNDODEPTH = 20;

		private int cmdPtr;
		private int firstCmdPtr;
		private int lastCmdPtr;
		private int[] command     = new int[UNDODEPTH];
		private int[] mapListPtr  = new int[UNDODEPTH];
		private int[] helpIndex   = new int[UNDODEPTH];
		private int[] nrOfStrings = new int[UNDODEPTH];
		private int strListPtr;
		private List<string> strList = new List<string>();
		
		private const int EMPTY              =  0;
		private const int REPLACELINE        =  1;
		private const int NEWCHILD           =  2;
		private const int NEWSISTER          =  3;
		private const int NEWPARENT          =  4;
		private const int NEWBROTHER         =  5;
		private const int DELETE             =  6;
		private const int DELETEJOIN         =  7;
		private const int PASTEASSISTER      =  8;
		private const int PASTEASCHILD       =  9;
		private const int MOVEUP             = 10;
		private const int MOVEDOWN           = 11;
		private const int DELETELASTICON     = 12;
		private const int DELETELASTICONJOIN = 13;
		private const int ADDICON            = 14;
		private const int ADDICONSPLIT       = 15;
		private const int EDITHTMLNODE       = 16;
		private const int EDITHTMLNOTE       = 17;
		private const int EDITNOTE           = 18;
		private const int ADDNOTE            = 19;
		private const int ADDNOTESPLIT       = 20;
		private const int DELETENOTE         = 21;
		private const int DELETENOTEJOIN     = 22;
		private const int DECRYPT            = 23;
		private const int ENCRYPT            = 24;
		private const int REMOVEENCRYPTION   = 25;
		private const int FORMAT             = 26;

		Editors editors;

		private Texts t;
		private int language;

		public UndoRedo(Editors edit, int languageParam, Texts texts)
		{
			editors = edit;
			t = texts;
			language = languageParam;

			Clear();
		}

		public void Clear()
		{
			cmdPtr = 0;
			firstCmdPtr = 0;
			lastCmdPtr = 0;

			strListPtr = 0;
			strList.Clear();
		}
		
		public bool CanUndo
		{
			get { return (cmdPtr != firstCmdPtr); }
		}

		public bool CanRedo
		{
			get { return (cmdPtr != lastCmdPtr); }
		}

		public string UndoText
		{
			get
			{
				return(Text(command[MinusEins(cmdPtr)]));
			}
		}

		public string RedoText
		{
			get
			{
				return(Text(command[cmdPtr]));
			}
		}

		private string Text(int command)
		{
			switch (command)
			{
			case REPLACELINE:
			case EDITHTMLNODE:
				return(t.t[t.EditText, language]);
			case EDITHTMLNOTE:
			case EDITNOTE:
				return(t.t[t.EditNote, language]);
			case NEWCHILD:
				return(t.t[t.NewChild, language]);
			case NEWSISTER:
				return(t.t[t.NewSister, language]);
			case NEWPARENT:
				return(t.t[t.NewParent, language]);
			case NEWBROTHER:
				return(t.t[t.NewBrother, language]);
			case DELETE:
			case DELETEJOIN:
				return(t.t[t.DeleteNode, language]);
			case PASTEASSISTER:
				return(t.t[t.PasteSister, language]);
			case PASTEASCHILD:
				return(t.t[t.PasteChild, language]);
			case MOVEUP:
				return(t.t[t.MoveUp, language]);
			case MOVEDOWN:
				return(t.t[t.MoveDown, language]);
			case DELETELASTICON:
			case DELETELASTICONJOIN:
				return(t.t[t.DeleteIcon, language]);
			case ADDICON:
			case ADDICONSPLIT:
				return(t.t[t.AddIcon, language]);
			case ADDNOTE:
			case ADDNOTESPLIT:
				return(t.t[t.AddNote, language]);
			case DELETENOTE:
			case DELETENOTEJOIN:
				return(t.t[t.DeleteNote, language]);
			case DECRYPT:
				return(t.t[t.Decryption, language]);
			case ENCRYPT:
				return(t.t[t.Encryption, language]);
			case REMOVEENCRYPTION:
				return(t.t[t.RemoveEncryption, language]);
			case FORMAT:
				return(t.t[t.Format, language]);
			default:
				return("n/a");
			}
		}

		private int MinusEins(int value)
		{
			if (value == 0)
				return (UNDODEPTH-1);
			else
				return (value-1);
		}
		
		private int PlusEins(int value)
		{
			if (value == (UNDODEPTH-1))
				return (0);
			else
				return (value+1);
		}

		public bool Undo(List<string> mapList)
		{
			if (!CanUndo) return(false);

			cmdPtr = MinusEins(cmdPtr);
			
			switch (command[cmdPtr])
			{
			case FORMAT:
			case REPLACELINE:
				UndoReplaceLine(mapList);
				break;
			case NEWCHILD:
				UndoNewChild(mapList);
				break;
			case NEWSISTER:
				UndoNewSister(mapList);
				break;
			case NEWPARENT:
				UndoNewParent(mapList);
				break;
			case NEWBROTHER:
				UndoNewBrother(mapList);
				break;
			case DELETE:
				UndoDelete(mapList);
				break;
			case DELETEJOIN:
				UndoDeleteJoin(mapList);
				break;
			case PASTEASSISTER:
				UndoPasteAsSister(mapList);
				break;
			case PASTEASCHILD:
				UndoPasteAsChild(mapList);
				break;
			case MOVEUP:
				UndoMoveUp(mapList);
				break;
			case MOVEDOWN:
				UndoMoveDown(mapList);
				break;
			case DELETELASTICON:
				UndoDelete(mapList);
				break;
			case DELETELASTICONJOIN:
				UndoDeleteJoin(mapList);
				break;
			case ADDICON:
			case ADDICONSPLIT:
				UndoAddIcon(mapList, command[cmdPtr]);
				break;
			case EDITHTMLNODE:
				UndoEditHtmlNode(mapList);
				break;
			case EDITHTMLNOTE:
			case EDITNOTE:
				UndoEditHtmlNote(mapList);
				break;
			case ADDNOTE:
				UndoAddNote(mapList);
				break;
			case ADDNOTESPLIT:
				UndoAddNoteSplit(mapList);
				break;
			case DELETENOTE:
				UndoDelete(mapList);
				break;
			case DELETENOTEJOIN:
				UndoDeleteJoin(mapList);
				break;
			case DECRYPT:
				UndoDecryptNode(mapList);
				break;
			case ENCRYPT:
				UndoEncryptNode(mapList);
				break;
			case REMOVEENCRYPTION:
				UndoRemoveEncryption(mapList);
				break;
			default:
				Clear();  // Fehlerfall darf nicht vorkommen
				break;
			}
			return(true);
		}
		
		public bool Redo(List<string> mapList)
		{
			if (!CanRedo) return(false);
			
			switch (command[cmdPtr])
			{
			case FORMAT:
			case REPLACELINE:
				RedoReplaceLine(mapList);
				break;
			case NEWCHILD:
				RedoNewChild(mapList);
				break;
			case NEWSISTER:
				RedoNewSister(mapList);
				break;
			case NEWPARENT:
				RedoNewParent(mapList);
				break;
			case NEWBROTHER:
				RedoNewBrother(mapList);
				break;
			case DELETE:
				RedoDelete(mapList);
				break;
			case DELETEJOIN:
				RedoDeleteJoin(mapList);
				break;
			case PASTEASSISTER:
				RedoPasteAsSister(mapList);
				break;
			case PASTEASCHILD:
				RedoPasteAsChild(mapList);
				break;
			case MOVEUP:
				RedoMoveUp(mapList);
				break;
			case MOVEDOWN:
				RedoMoveDown(mapList);
				break;
			case DELETELASTICON:
				RedoDelete(mapList);
				break;
			case DELETELASTICONJOIN:
				RedoDeleteJoin(mapList);
				break;
			case ADDICON:
			case ADDICONSPLIT:
				RedoAddIcon(mapList, command[cmdPtr]);
				break;
			case EDITHTMLNODE:
				RedoEditHtmlNode(mapList);
				break;
			case EDITHTMLNOTE:
			case EDITNOTE:
				RedoEditHtmlNote(mapList);
				break;
			case ADDNOTE:
				RedoAddNote(mapList);
				break;
			case ADDNOTESPLIT:
				RedoAddNoteSplit(mapList);
				break;
			case DELETENOTE:
				RedoDelete(mapList);
				break;
			case DELETENOTEJOIN:
				RedoDeleteJoin(mapList);
				break;
			case DECRYPT:
				RedoDecryptNode(mapList);
				break;
			case ENCRYPT:
				RedoEncryptNode(mapList);
				break;
			case REMOVEENCRYPTION:
				RedoRemoveEncryption(mapList);
				break;
			default:
				Clear();
				break;
			}

			cmdPtr = PlusEins(cmdPtr);

			return(true);
		}

		private void UpdateCmdPointers()
		{
			cmdPtr = PlusEins(cmdPtr);  // cmdPtr inkrementieren
			lastCmdPtr = cmdPtr;        // lastCmdPtr hierhin setzen
			if (cmdPtr == firstCmdPtr)  // firstCmdPtr wurde eingeholt
			{
				strList.RemoveRange(0, nrOfStrings[cmdPtr]);  // strList am Anfang aufräumen
				strListPtr -= nrOfStrings[cmdPtr];            // strListPtr runtersetzen
				firstCmdPtr = PlusEins(firstCmdPtr);          // firstCmdPtr inkrementieren
			}
		}

		private void AddNewCommand(List<string> mapList, int index, int com)
		{
			// das neue Kommando löscht alle evtl. vorhandenen höheren
			command[cmdPtr] = com;        // neues Kommando eintragen
			mapListPtr[cmdPtr] = index;   // soll immer auf Basis-Knoten zeigen
			nrOfStrings[cmdPtr] = 1;      // ursprünglicher Standard war '1'
			strList.RemoveRange(strListPtr, strList.Count-strListPtr);  // strList nach oben löschen
			strList.Add(mapList[index]);  // teilweise Dummy, macht aber nichts
			strListPtr++;                 // strListPtr anpassen

			UpdateCmdPointers();
		}

		public void MoveUp(List<string> mapList, int myIndex, int indexFirst, int indexLast)
		{
			// Knoten nach oben verschieben
			mapListPtr[cmdPtr] = myIndex;  // Knotenposition merken
			string str = String.Format("{0:D} {1:D}", indexFirst, indexLast);
			str = MoveUpWorker(mapList, str);

			command[cmdPtr] = MOVEUP;
			nrOfStrings[cmdPtr] = 1;
			strList.RemoveRange(strListPtr, strList.Count-strListPtr);
			strList.Add(str);
			strListPtr++;
			
			UpdateCmdPointers();
		}

		private string MoveUpWorker(List<string> mapList, string indexes)
		{
			string [] split = indexes.Split(new Char [] {' '});
			int indexFirst = Convert.ToInt32(split[0]);
			int indexLast = Convert.ToInt32(split[1]);

			int myIndex = mapListPtr[cmdPtr];

			int i; // Laufvariable

			// End-Index und Länge des selektierten Knotens
			int myEnd = Parse.FindNodeEnd(mapList, myIndex);
			int myLength = myEnd - myIndex + 1;

			if (myIndex == indexFirst)
			{
				// der selektierte Knoten ist der Oberste
				// den selektierten Knoten hinter dem letzten Knoten einschieben
				indexLast = Parse.FindNodeEnd(mapList, indexLast) + 1;
				for (i = myEnd; i >= myIndex; i--)
					mapList.Insert(indexLast, mapList[i]);

				// Original entfernen
				mapList.RemoveRange(myIndex, myLength);

				// Indizes anpassen
				indexLast -= myLength;
				myIndex = indexLast;
			}
			else
			{
				// der selektierte Knoten ist nicht der Oberste
				// den selektierten Knoten vor dem Vorgänger einschieben
				// also dessen Anfang suchen
				int indexPrev = myIndex - 1;
				indexPrev = Parse.FindNodeStart(mapList, indexPrev);

				for (i = 0; i < myLength; i++)
					mapList.Insert(indexPrev, mapList[myEnd]);

				// Original entfernen
				mapList.RemoveRange(myEnd+1, myLength);

				// Index anpassen
				myIndex = indexPrev;
			}

			mapListPtr[cmdPtr] = myIndex;
			string str = String.Format("{0:D} {1:D}", indexFirst, indexLast);
			
			return(str);
		}

		private void UndoMoveUp(List<string> mapList)
		{
			strListPtr--;
			strList[strListPtr] = MoveDownWorker(mapList, strList[strListPtr]);
		}

		private void RedoMoveUp(List<string> mapList)
		{
			strList[strListPtr] = MoveUpWorker(mapList, strList[strListPtr]);
			strListPtr++;
		}

		private string MoveDownWorker(List<string> mapList, string indexes)
		{
			string [] split = indexes.Split(new Char [] {' '});
			int indexFirst = Convert.ToInt32(split[0]);
			int indexLast = Convert.ToInt32(split[1]);

			int myIndex = mapListPtr[cmdPtr];

			int i; // Laufvariable

			// End-Index und Länge des selektierten Knotens
			int myEnd = Parse.FindNodeEnd(mapList, myIndex);
			int myLength = myEnd - myIndex + 1;

			if (myIndex == indexLast)
			{
				// der selektierte Knoten ist der Unterste
				// den selektierten Knoten vor dem ersten Knoten einschieben
				for (i = 0; i < myLength; i++)
					mapList.Insert(indexFirst, mapList[myEnd]);

				// Original entfernen
				mapList.RemoveRange(myEnd+1, myLength);

				// Indizes anpassen
				indexLast = myEnd;
				indexLast = Parse.FindNodeStart(mapList, indexLast);
				myIndex = indexFirst;
			}
			else
			{
				// der selektierte Knoten ist nicht der Unterste
				// den selektierten Knoten hinter dem Nachfolger einschieben
				// also dessen Ende suchen
				int indexNext = myEnd + 1;
				indexNext = Parse.FindNodeEnd(mapList, indexNext) + 1;

				for (i = 0; i < myLength; i++)
					mapList.Insert(indexNext, mapList[myEnd-i]);

				// Original entfernen
				mapList.RemoveRange(myIndex, myLength);

				// Indizes anpassen
				myIndex = indexNext - myLength;
				if (indexLast < indexNext) indexLast = myIndex;
			}

			mapListPtr[cmdPtr] = myIndex;
			string str = String.Format("{0:D} {1:D}", indexFirst, indexLast);
			
			return(str);
		}

		public void MoveDown(List<string> mapList, int myIndex, int indexFirst, int indexLast)
		{
			mapListPtr[cmdPtr] = myIndex;
			string str = String.Format("{0:D} {1:D}", indexFirst, indexLast);
			str = MoveDownWorker(mapList, str);

			// UndoRedo-Merker updaten
			command[cmdPtr] = MOVEDOWN;
			nrOfStrings[cmdPtr] = 1;
			strList.RemoveRange(strListPtr, strList.Count-strListPtr);
			strList.Add(str);
			strListPtr++;
			
			UpdateCmdPointers();
		}

		private void UndoMoveDown(List<string> mapList)
		{
			strListPtr--;
			strList[strListPtr] = MoveUpWorker(mapList, strList[strListPtr]);
		}

		private void RedoMoveDown(List<string> mapList)
		{
			strList[strListPtr] = MoveDownWorker(mapList, strList[strListPtr]);
			strListPtr++;
		}

		public int PasteAsSister(List<string> mapList, int startIndex, List<string> clipboard)
		{
			// das Clipboard soll als Sister eingefügt werden

			string str;  // Hilfsstring

			command[cmdPtr] = PASTEASSISTER;  // Kommando merken
			mapListPtr[cmdPtr] = startIndex;  // zeigt auf selektierten Knoten (hier soll der neue hin)
			nrOfStrings[cmdPtr] = clipboard.Count;  // Anzahl der Strings
			strList.RemoveRange(strListPtr, strList.Count-strListPtr);  // strList abschneiden

			// das Clipboard reinpasten
			for (int i = clipboard.Count-1; i >= 0 ; i--)
			{
				str = clipboard[i];               // von hinten aufrollen
				strList.Insert(strListPtr, str);  // in die strList
				mapList.Insert(startIndex, str);  // in die mapList
			}

			// strListPtr ans Ende der Liste
			strListPtr += clipboard.Count;

			UpdateCmdPointers();
			
			return(startIndex);
		}

		private void UndoPasteAsSister(List<string> mapList)
		{
			// Startindex des zu entfernenden Knotens
			int index = mapListPtr[cmdPtr];

			// den Knoten entfernen
			for (int i = 0; i < nrOfStrings[cmdPtr]; i++)
				mapList.RemoveAt(index);

			// strListPtr an den Anfang des Blocks
			strListPtr -= nrOfStrings[cmdPtr];
		}

		private void RedoPasteAsSister(List<string> mapList)
		{
			// der Bereich aus strList muss als Sister eingefügt werden
			int startIndex = mapListPtr[cmdPtr];

			// den Bereich aus strList reinpasten
			// strListPtr ans Ende des Blocks
			strListPtr += nrOfStrings[cmdPtr];
			for (int i = strListPtr-1; i >= strListPtr-nrOfStrings[cmdPtr]; i--)
			{
				mapList.Insert(startIndex, strList[i]);  // in die mapList
			}
		}

		public void RemoveEncryption(List<string> mapList, int nodeIndex, bool undoable)
		{
			// der Knoten ist bei Aufruf dieser Methode immer im Zustand "decrypted"

			if (undoable) AddNewCommand(mapList, nodeIndex, REMOVEENCRYPTION);  // u.a. Zeile merken

			int start = mapList[nodeIndex].IndexOf("PWD=\"");                   // Passwort aus node entfernt
			int length = mapList[nodeIndex].Substring(start+5).IndexOf("\"");
			mapList[nodeIndex] = mapList[nodeIndex].Remove(start, length+7);

			start = mapList[nodeIndex].IndexOf("ENCRYPTED_CONTENT=\"");         // ENCRYPTED_CONTENT entfernen
			length = mapList[nodeIndex].Substring(start+19).IndexOf("\"");
			mapList[nodeIndex] = mapList[nodeIndex].Remove(start, length+21);
		}

		private void UndoRemoveEncryption(List<string> mapList)
		{
			strListPtr--;

			mapList[mapListPtr[cmdPtr]] = strList[strListPtr];  // alte Zeile wieder einsetzen
		}

		private void RedoRemoveEncryption(List<string> mapList)
		{
			RemoveEncryption(mapList, mapListPtr[cmdPtr], false);

			strListPtr++;
		}

		public bool DecryptNode(List<string> mapList, int nodeIndex, string pwd, bool undoable)
		{
			String encryptedString = Parse.GetEncryptedContent(mapList, nodeIndex);
			String decryptedString = "";
			string str;
			string tempString;
			List<string> nodeList = new List<string>();
			int i;

			if (!Encryption.Decrypt(encryptedString, pwd, ref decryptedString))
			{
				MessageBox.Show(t.t[t.DecryptionFailed, language],
				                t.t[t.Error, language]);
				return(false);
			}

			if ((!decryptedString.StartsWith("<node ")) && (decryptedString != ""))
			{
				MessageBox.Show(t.t[t.DecryptionFailed, language],
				                t.t[t.Error, language]);
				return(false);
			}

			Cursor.Current = Cursors.WaitCursor;  // kann dauern

			while(true)
			{
				if (decryptedString == "") break;

				i = decryptedString.IndexOf(">\n");
				tempString = decryptedString.Substring(0, i);  // ">\n" entfernen
				tempString += ">";  // unverstandener Work-around weil sonst immer noch Extrazeichen am Ende des Strings erschienen
				nodeList.Add(tempString);
				decryptedString = decryptedString.Substring(i+2);
				if (decryptedString.StartsWith("<nodeseparator>"))
					decryptedString = decryptedString.Substring(15);  // "<nodeseparator>" entfernen
			}

			Cursor.Current = Cursors.Default;

			if (undoable) AddNewCommand(mapList, nodeIndex, DECRYPT);

			str = pwd;
			Parse.CharToXml(ref str);
			str = "PWD=\"" + str + "\" ";
			mapList[nodeIndex] = mapList[nodeIndex].Insert(6, str);

			if (nodeList.Count == 0)
				return(true);

			Parse.InsertChildNodes(mapList, nodeIndex, nodeList);

			return(true);
		}

		private void UndoDecryptNode(List<string> mapList)
		{
			strListPtr--;

			int startIndex = mapListPtr[cmdPtr];
			int stopIndex = Parse.FindNodeEnd(mapList, startIndex);

			if (startIndex == stopIndex)
			{
				mapList[mapListPtr[cmdPtr]] = strList[strListPtr];
				return;
			}

			Parse.RemoveChildNodes(mapList, startIndex, stopIndex);

			mapList[mapListPtr[cmdPtr]] = strList[strListPtr];
		}

		private void RedoDecryptNode(List<string> mapList)
		{
			string pwd = "";

			if (!editors.EnterOnePwd(ref pwd))
			{
				cmdPtr = MinusEins(cmdPtr);  // wird in der aufrufenden Methode immer inkrementiert
				return;  // Passworteingabe abgebrochen
			}

			DecryptNode(mapList, mapListPtr[cmdPtr], pwd, false);

			strListPtr++;
		}

		public bool EncryptNode(List<string> mapList, int nodeIndex, bool undoable)
		{
			string pwd = "";
			string strToEncrypt = "";
			string encryptedContent = "";
			bool found = false;
			int start = 0;
			int length = 0;
			int stopIndex = Parse.FindNodeEnd(mapList, nodeIndex);
			int index;
			int indentLevel = 0;

			if (Parse.IsEncrypted(mapList, nodeIndex))  // ist schon verschlüsselt
				return(false);

			if (Parse.IsDecrypted(mapList, nodeIndex))  // ist entschlüsselt
			{
				start = mapList[nodeIndex].IndexOf(" PWD=\"") + 6;  // Passwort suchen
				length = mapList[nodeIndex].Substring(start).IndexOf("\"");
				pwd = mapList[nodeIndex].Substring(start, length);
				Parse.XmlToChar(ref pwd);  // Passwort rauskopiert
				mapList[nodeIndex] = mapList[nodeIndex].Remove(start-5, length+7);  // Passwort aus node entfernt
				// ENCRYPTED_CONTENT entfernen
				start = mapList[nodeIndex].IndexOf(" ENCRYPTED_CONTENT=\"") + 20;
				length = mapList[nodeIndex].Substring(start).IndexOf("\"");
				mapList[nodeIndex] = mapList[nodeIndex].Remove(start-19, length+21);
			}
			else  // ist noch gar nicht verschlüsselt
			{
				if (!editors.EnterTwoPwd(ref pwd))  // Passwort anfordern
					return(false);
			}

			if (undoable)  // auf Wunsch in UndoRedo-Liste merken
			{
				AddNewCommand(mapList, nodeIndex, ENCRYPT);
			}

			if (stopIndex > nodeIndex)  // es kann Unterknoten geben
			{
				for (index = nodeIndex+1; index < stopIndex; index++)  // Unterknoten suchen
				{
					if (mapList[index].StartsWith("<node"))
					{
						found = true;
						break;
					}
				}
	
				if (found)  // index zeigt auf gefundenen Unterknoten
				{
					int i = index;

					while(true)
					{
						if ((mapList[i].StartsWith("<node")) && (indentLevel == 0))
						{
							strToEncrypt += "<nodeseparator>";
							if (!mapList[i].EndsWith("/>"))
								indentLevel++;
						}
						else if (mapList[i].StartsWith("</node"))
							indentLevel--;

						strToEncrypt += mapList[i] + '\n';

						if (++i == stopIndex)
							break;
					}

					if (strToEncrypt.StartsWith("<nodeseparator>"))  // ersten nodeseparator ggf. entfernen
					    strToEncrypt = strToEncrypt.Remove(0, 15);

					mapList.RemoveRange(index, stopIndex-index);  // mapList aufräumen
					Parse.JoinNode(mapList, nodeIndex);
				}
			}

			Encryption.Encrypt(strToEncrypt, pwd, ref encryptedContent);

			encryptedContent = "ENCRYPTED_CONTENT=\"" + encryptedContent + "\" ";

			if ((index = mapList[nodeIndex].IndexOf(" CREATED=\"")) <= 0)
			{
				index = 6;  // CREATED=" nicht gefunden
			}
			else
			{
				index += 10;  // hinter das öffnende "
				index += mapList[nodeIndex].Substring(index).IndexOf("\"");  // auf das schließende "
				index += 2;  // endgültige Einfügeposition
			}

			mapList[nodeIndex] = mapList[nodeIndex].Insert(index, encryptedContent);

			return(true);
		}

		private void UndoEncryptNode(List<string> mapList)
		{
			string str = mapList[mapListPtr[cmdPtr]];  // Original zwischenspeichern
			string pwd = "";

			if (!editors.EnterOnePwd(ref pwd))
			{
				cmdPtr = PlusEins(cmdPtr);  // wird in der aufrufenden Methode immer dekrementiert
				return;  // Passworteingabe abgebrochen
			}

			if (!DecryptNode(mapList, mapListPtr[cmdPtr], pwd, false))
			{
				cmdPtr = PlusEins(cmdPtr);  // wird in der aufrufenden Methode immer dekrementiert
				return;  // Decryption failed
			}

			strList[--strListPtr] = str;  // verschlüsselten Knoten merken
		}

		private void RedoEncryptNode(List<string> mapList)
		{
			int stopIndex = Parse.FindNodeEnd(mapList, mapListPtr[cmdPtr]);

			if (stopIndex > mapListPtr[cmdPtr])  // Knoten auf eine Zeile reduzieren
				mapList.RemoveRange(mapListPtr[cmdPtr]+1, stopIndex-mapListPtr[cmdPtr]);

			mapList[mapListPtr[cmdPtr]] = strList[strListPtr++];  // verschlüsselte Zeile einfügen
		}

		public void EditHtmlNode(List<string> mapList, int nodeIndex, List<string> listHtml)
		{
			// listHtml soll den bisherigen HTML-Text ersetzen

			// den HTML-Text suchen
			int startIndex = 0;
			int stopIndex = 0;
			Parse.FindHtmlNode(mapList, nodeIndex, ref startIndex, ref stopIndex);

			command[cmdPtr] = EDITHTMLNODE;  // Kommando merken
			mapListPtr[cmdPtr] = nodeIndex;  // zeigt auf Basis-Node
			nrOfStrings[cmdPtr] = stopIndex - startIndex + 1;  // Anzahl der Original-Strings
			strList.RemoveRange(strListPtr, strList.Count-strListPtr);  // strList hinten abschneiden

			// Original-Strings in die strList
			for (int i = startIndex; i <= stopIndex; i++)
			{
				strList.Add(mapList[i]);  // in die strList
			}
			mapList.RemoveRange(startIndex, nrOfStrings[cmdPtr]);  // Original entfernen

			// strListPtr ans Ende der Liste
			strListPtr += nrOfStrings[cmdPtr];

			mapList.InsertRange(startIndex, listHtml);  // neue Strings einfügen

			mapList[nodeIndex] = Parse.UpdateModificationTime(mapList[nodeIndex]);

			UpdateCmdPointers();
		}

		private void UndoEditHtmlNode(List<string> mapList)
		{
			// HTML-Text in mapList und HTML-Text in strList gegeneinander vertauschen

			List<string> listOrig = new List<string>();  // Zwischenspeicher für original HTML-Text aus mapList

			// den HTML-Text suchen
			int startIndex = 0;  // Anfangsindex des HTML-Texts
			int stopIndex = 0;   // Endindex des HTML-Texts
			Parse.FindHtmlNode(mapList, mapListPtr[cmdPtr], ref startIndex, ref stopIndex);
			int lengthOrig = stopIndex - startIndex + 1;  // Länge des original HTML-Texts in mapList

			listOrig = mapList.GetRange(startIndex, lengthOrig);  // Original zwischenspeichern
			mapList.RemoveRange(startIndex, lengthOrig);          // Original entfernen

			strListPtr -= nrOfStrings[cmdPtr];  // strListPtr an den Anfang des Blocks
			mapList.InsertRange(startIndex, strList.GetRange(strListPtr, nrOfStrings[cmdPtr]));  // Bereich einfügen
			strList.RemoveRange(strListPtr, nrOfStrings[cmdPtr]);  // Bereich entfernen

			strList.InsertRange(strListPtr, listOrig);  // Originalstring aus Zwischenspeicher einfügen
			nrOfStrings[cmdPtr] = lengthOrig;           // Länge merken

			mapList[mapListPtr[cmdPtr]] = Parse.UpdateModificationTime(mapList[mapListPtr[cmdPtr]]);
		}

		private void RedoEditHtmlNode(List<string> mapList)
		{
			// HTML-Text in mapList und HTML-Text in strList gegeneinander vertauschen

			List<string> listOrig = new List<string>();  // Zwischenspeicher für original HTML-Text aus mapList

			// den HTML-Text suchen
			int startIndex = 0;  // Anfangsindex des HTML-Texts
			int stopIndex = 0;   // Endindex des HTML-Texts
			Parse.FindHtmlNode(mapList, mapListPtr[cmdPtr], ref startIndex, ref stopIndex);
			int lengthOrig = stopIndex - startIndex + 1;  // Länge des original HTML-Texts in mapList

			listOrig = mapList.GetRange(startIndex, lengthOrig);  // Original zwischenspeichern
			mapList.RemoveRange(startIndex, lengthOrig);          // Original entfernen

			mapList.InsertRange(startIndex, strList.GetRange(strListPtr, nrOfStrings[cmdPtr]));  // Bereich einfügen
			strList.RemoveRange(strListPtr, nrOfStrings[cmdPtr]);  // Bereich entfernen

			strList.InsertRange(strListPtr, listOrig);  // Originalstring aus Zwischenspeicher einfügen
			nrOfStrings[cmdPtr] = lengthOrig;           // Länge merken
			strListPtr += lengthOrig;  // strListPtr ans Ende des Blocks

			mapList[mapListPtr[cmdPtr]] = Parse.UpdateModificationTime(mapList[mapListPtr[cmdPtr]]);
		}

		public void EditHtmlNote(List<string> mapList, int nodeIndex, List<string> listHtml)
		{
			// listHtml soll den bisherigen HTML-Text ersetzen

			// den HTML-Text suchen
			int startIndex = 0;
			int stopIndex = 0;
			Parse.FindNote(mapList, nodeIndex, ref startIndex, ref stopIndex);

			command[cmdPtr] = EDITHTMLNOTE;  // Kommando merken
			mapListPtr[cmdPtr] = nodeIndex;  // zeigt auf Basis-Node
			nrOfStrings[cmdPtr] = stopIndex - startIndex + 1;  // Anzahl der Original-Strings
			strList.RemoveRange(strListPtr, strList.Count-strListPtr);  // strList hinten abschneiden

			// Original-Strings in die strList
			for (int i = startIndex; i <= stopIndex; i++)
			{
				strList.Add(mapList[i]);  // in die strList
			}
			mapList.RemoveRange(startIndex, nrOfStrings[cmdPtr]);  // Original entfernen

			// strListPtr ans Ende der Liste
			strListPtr += nrOfStrings[cmdPtr];

			mapList.InsertRange(startIndex, listHtml);  // neue Strings einfügen

			mapList[nodeIndex] = Parse.UpdateModificationTime(mapList[nodeIndex]);

			UpdateCmdPointers();
		}

		private void UndoEditHtmlNote(List<string> mapList)
		{
			// HTML-Text in mapList und HTML-Text in strList gegeneinander vertauschen

			List<string> listOrig = new List<string>();  // Zwischenspeicher für original HTML-Text aus mapList

			// den HTML-Text suchen
			int startIndex = 0;  // Anfangsindex des HTML-Texts
			int stopIndex = 0;   // Endindex des HTML-Texts
			Parse.FindNote(mapList, mapListPtr[cmdPtr], ref startIndex, ref stopIndex);
			int lengthOrig = stopIndex - startIndex + 1;  // Länge des original HTML-Texts in mapList

			listOrig = mapList.GetRange(startIndex, lengthOrig);  // Original zwischenspeichern
			mapList.RemoveRange(startIndex, lengthOrig);          // Original entfernen

			strListPtr -= nrOfStrings[cmdPtr];  // strListPtr an den Anfang des Blocks
			mapList.InsertRange(startIndex, strList.GetRange(strListPtr, nrOfStrings[cmdPtr]));  // Bereich einfügen
			strList.RemoveRange(strListPtr, nrOfStrings[cmdPtr]);  // Bereich entfernen

			strList.InsertRange(strListPtr, listOrig);  // Originalstring aus Zwischenspeicher einfügen
			nrOfStrings[cmdPtr] = lengthOrig;           // Länge merken

			mapList[mapListPtr[cmdPtr]] = Parse.UpdateModificationTime(mapList[mapListPtr[cmdPtr]]);
		}

		private void RedoEditHtmlNote(List<string> mapList)
		{
			// HTML-Text in mapList und HTML-Text in strList gegeneinander vertauschen

			List<string> listOrig = new List<string>();  // Zwischenspeicher für original HTML-Text aus mapList

			// den HTML-Text suchen
			int startIndex = 0;  // Anfangsindex des HTML-Texts
			int stopIndex = 0;   // Endindex des HTML-Texts
			Parse.FindNote(mapList, mapListPtr[cmdPtr], ref startIndex, ref stopIndex);
			int lengthOrig = stopIndex - startIndex + 1;  // Länge des original HTML-Texts in mapList

			listOrig = mapList.GetRange(startIndex, lengthOrig);  // Original zwischenspeichern
			mapList.RemoveRange(startIndex, lengthOrig);          // Original entfernen

			mapList.InsertRange(startIndex, strList.GetRange(strListPtr, nrOfStrings[cmdPtr]));  // Bereich einfügen
			strList.RemoveRange(strListPtr, nrOfStrings[cmdPtr]);  // Bereich entfernen

			strList.InsertRange(strListPtr, listOrig);  // Originalstring aus Zwischenspeicher einfügen
			nrOfStrings[cmdPtr] = lengthOrig;           // Länge merken
			strListPtr += lengthOrig;  // strListPtr ans Ende des Blocks

			mapList[mapListPtr[cmdPtr]] = Parse.UpdateModificationTime(mapList[mapListPtr[cmdPtr]]);
		}

		public void EditNote(List<string> mapList, int nodeIndex, List<string> listNote)
		{
			// listNote soll die bisherige Note ersetzen

			// Note suchen
			int startIndex = 0;
			int stopIndex = 0;
			Parse.FindNote(mapList, nodeIndex, ref startIndex, ref stopIndex);

			command[cmdPtr] = EDITNOTE;  // Kommando merken
			mapListPtr[cmdPtr] = nodeIndex;  // zeigt auf Basis-Node
			nrOfStrings[cmdPtr] = stopIndex - startIndex + 1;  // Anzahl der Original-Strings
			strList.RemoveRange(strListPtr, strList.Count-strListPtr);  // strList hinten abschneiden

			// Original-Strings in die strList
			for (int i = startIndex; i <= stopIndex; i++)
			{
				strList.Add(mapList[i]);  // in die strList
			}
			mapList.RemoveRange(startIndex, nrOfStrings[cmdPtr]);  // Original entfernen

			// strListPtr ans Ende der Liste
			strListPtr += nrOfStrings[cmdPtr];

			mapList.InsertRange(startIndex, listNote);  // neue Strings einfügen

			mapList[nodeIndex] = Parse.UpdateModificationTime(mapList[nodeIndex]);

			UpdateCmdPointers();
		}

		public int PasteAsChild(List<string> mapList, int startIndex, List<string> clipboard)
		{
			// das Clipboard soll als Child eingefügt werden

			string str;  // Hilfsstring

			command[cmdPtr] = PASTEASCHILD;  // Kommando merken
			mapListPtr[cmdPtr] = startIndex;  // zeigt auf Basis-Node
			nrOfStrings[cmdPtr] = clipboard.Count;  // Anzahl der Strings
			strList.RemoveRange(strListPtr, strList.Count-strListPtr);  // strList abschneiden

			// das Ende des Parents suchen
			int stopIndex = Parse.FindNodeEnd(mapList, startIndex);

			// den Parent bei Bedarf splitten
			if (startIndex == stopIndex)
				stopIndex = Parse.SplitNode(mapList, startIndex);

			// das Clipboard reinpasten
			for (int i = clipboard.Count-1; i >= 0 ; i--)
			{
				str = clipboard[i];               // von hinten aufrollen
				strList.Insert(strListPtr, str);  // in die strList
				mapList.Insert(stopIndex, str);   // in die mapList
			}

			// strListPtr ans Ende der Liste
			strListPtr += clipboard.Count;

			UpdateCmdPointers();
			
			return(startIndex);
		}

		private void UndoPasteAsChild(List<string> mapList)
		{
			// gepastet wurde das unterste Child
			// das muss nun gelöscht werden

			// die Knotengrenzen suchen
			int index = mapListPtr[cmdPtr];  // Start des Parents
			index = Parse.FindNodeEnd(mapList, index);  // Stop des Parents
			index--;  // Stop des Childs
			index = Parse.FindNodeStart(mapList, index);  // Start des Childs

			// den Knoten löschen
			for (int i = 0; i < nrOfStrings[cmdPtr]; i++)
				mapList.RemoveAt(index);

			// strListPtr an den Anfang des Blocks
			strListPtr -= nrOfStrings[cmdPtr];

			// Parent bei Bedarf joinen
			Parse.JoinNode(mapList, mapListPtr[cmdPtr]);
		}

		private void RedoPasteAsChild(List<string> mapList)
		{
			// der Bereich aus strList muss als Child eingefügt werden

			// das Ende des Parents suchen
			int startIndex = mapListPtr[cmdPtr];
			int stopIndex = Parse.FindNodeEnd(mapList, startIndex);

			// den Parent bei Bedarf splitten
			if (startIndex == stopIndex)
				stopIndex = Parse.SplitNode(mapList, startIndex);

			// den Bereich aus strList reinpasten
			// strListPtr ans Ende des Blocks
			strListPtr += nrOfStrings[cmdPtr];
			for (int i = strListPtr-1; i >= strListPtr-nrOfStrings[cmdPtr]; i--)
			{
				mapList.Insert(stopIndex, strList[i]);   // in die mapList
			}
		}

		public void Delete(List<string> mapList, int startIndex)
		{
			int stopIndex = Parse.FindNodeEnd(mapList, startIndex);
			int count = stopIndex - startIndex + 1;
			int i = count;

			helpIndex[cmdPtr] = startIndex;

			// strList ab aktuellem strListPtr löschen
			strList.RemoveRange(strListPtr, strList.Count-strListPtr);
			// count Zeilen von mapList nach strList (natürliche Reihenfolge)
			while (i-- > 0)
			{
				strList.Add(mapList[startIndex]);
				strListPtr++;
				mapList.RemoveAt(startIndex);
			}

			// wenn es einen umschließenden (Parent-)Knoten gibt,
			// diesen bei Bedarf vereinigen
			if (mapList[startIndex].StartsWith("</node") &&
			    mapList[startIndex-1].StartsWith("<node") &&
			    !mapList[startIndex-1].EndsWith("/>"))
			{
				startIndex--;
				Parse.JoinNode(mapList, startIndex);
				command[cmdPtr] = DELETEJOIN;
			}
			else
				command[cmdPtr] = DELETE;
			
			// DELETEJOIN : mapListPtr zeigt auf den vereinigten Knoten
			// DELETE     : mapListPtr zeigt dorthin, wo eingefügt werden soll
			mapListPtr[cmdPtr] = startIndex;
			// Anzahl der gelöschten Strings merken
			nrOfStrings[cmdPtr] = count;
			
			UpdateCmdPointers();
		}

		private void UndoDelete(List<string> mapList)
		{
			// Anzahl der gelöschten Strings
			int count = nrOfStrings[cmdPtr];

			// die gelöschten Strings wieder herstellen
			// mapListPtr zeigt bei DELETE schon auf die richtige Position
			// Letzter wird zuerst eingefügt
			while (count-- > 0)
			{
				strListPtr--;
				mapList.Insert(mapListPtr[cmdPtr], strList[strListPtr]);
			}

			mapList[helpIndex[cmdPtr]] = Parse.UpdateModificationTime(mapList[helpIndex[cmdPtr]]);
		}

		private void UndoDeleteJoin(List<string> mapList)
		{
			// zunächst muss der vereinigte Knoten wieder aufgesplittet werden
			// mapListPtr zeigt bei DELETEJOIN auf diesen Knoten
			// anschließend zeigt mapListPtr auf die Einfügeposition (wie bei DELETE)
			mapListPtr[cmdPtr] = Parse.SplitNode(mapList, mapListPtr[cmdPtr]);

			// jetzt passt alles für UndoDelete()
			UndoDelete(mapList);
		}

		private void RedoDelete(List<string> mapList)
		{
			mapList[helpIndex[cmdPtr]] = Parse.UpdateModificationTime(mapList[helpIndex[cmdPtr]]);

			// Anzahl der zu löschenden Strings holen
			int count = nrOfStrings[cmdPtr];
			// strListPtr entsprechend hochsetzen
			strListPtr += count;
			// count Zeilen löschen
			for (int i = 0; i < count; i++)
				mapList.RemoveAt(mapListPtr[cmdPtr]);
		}

		private void RedoDeleteJoin(List<string> mapList)
		{
			// mapListPtr zeigt auf den zu entfernenden Bereich
			RedoDelete(mapList);
			// mapListPtr soll auf umschließenden (Parent-)Knoten zeigen,
			// der dann vereinigt wird
			mapListPtr[cmdPtr]--;
			Parse.JoinNode(mapList, mapListPtr[cmdPtr]);
		}

		public void ReplaceLine(List<string> mapList, int index)
		{
			AddNewCommand(mapList, index, REPLACELINE);

			mapList[index] = Parse.UpdateModificationTime(mapList[index]);
		}
		
		private void UndoReplaceLine(List<string> mapList)
		{
			string str;

			str = mapList[mapListPtr[cmdPtr]];
			strListPtr--;
			mapList[mapListPtr[cmdPtr]] = strList[strListPtr];
			strList[strListPtr] = str;

			mapList[mapListPtr[cmdPtr]] = Parse.UpdateModificationTime(mapList[mapListPtr[cmdPtr]]);
		}

		private void RedoReplaceLine(List<string> mapList)
		{
			string str;

			str = mapList[mapListPtr[cmdPtr]];
			mapList[mapListPtr[cmdPtr]] = strList[strListPtr];
			strList[strListPtr] = str;
			strListPtr++;

			mapList[mapListPtr[cmdPtr]] = Parse.UpdateModificationTime(mapList[mapListPtr[cmdPtr]]);
		}
		
		public void Format(List<string> mapList, int index)
		{
			AddNewCommand(mapList, index, FORMAT);

			mapList[index] = Parse.UpdateModificationTime(mapList[index]);
		}

		public void NewParent(List<string> mapList, int index)
		{
			AddNewCommand(mapList, index, NEWPARENT);
		}
		
		private void UndoNewParent(List<string> mapList)
		{
			int startIndex = mapListPtr[cmdPtr] - 1;
			int stopIndex = Parse.FindNodeEnd(mapList, startIndex);

			strListPtr--;
			strList[strListPtr] = mapList[startIndex];

			mapList.RemoveAt(stopIndex);
			mapList.RemoveAt(startIndex);

			mapListPtr[cmdPtr] = startIndex;
		}

		private void RedoNewParent(List<string> mapList)
		{
			int stopIndex = Parse.FindNodeEnd(mapList, mapListPtr[cmdPtr]);

			mapList.Insert(stopIndex+1, "</node>");
			mapList.Insert(mapListPtr[cmdPtr], strList[strListPtr]);

			mapListPtr[cmdPtr]++;

			strListPtr++;
		}

		public void NewBrother(List<string> mapList, int index)
		{
			AddNewCommand(mapList, index, NEWBROTHER);
		}
		
		private void UndoNewBrother(List<string> mapList)
		{
			int index = Parse.FindNodeEnd(mapList, mapListPtr[cmdPtr]) + 1;

			strListPtr--;
			strList[strListPtr] = mapList[index];

			mapList.RemoveAt(index);
		}

		private void RedoNewBrother(List<string> mapList)
		{
			int index = Parse.FindNodeEnd(mapList, mapListPtr[cmdPtr]) + 1;

			mapList.Insert(index, strList[strListPtr]);
			
			strListPtr++;
		}

		public void NewSister(List<string> mapList, int index)
		{
			AddNewCommand(mapList, index, NEWSISTER);
		}
		
		private void UndoNewSister(List<string> mapList)
		{
			strListPtr--;
			mapListPtr[cmdPtr]--;
			
			strList[strListPtr] = mapList[mapListPtr[cmdPtr]];
			mapList.RemoveAt(mapListPtr[cmdPtr]);
		}

		private void RedoNewSister(List<string> mapList)
		{
			mapList.Insert(mapListPtr[cmdPtr], strList[strListPtr]);
			strListPtr++;
			mapListPtr[cmdPtr]++;
		}

		public void NewChild(List<string> mapList, int index)
		{
			AddNewCommand(mapList, index, NEWCHILD);
		}

		private void UndoNewChild(List<string> mapList)
		{
			int index = Parse.FindNodeEnd(mapList, mapListPtr[cmdPtr]) - 1;

			strListPtr--;
			strList[strListPtr] = mapList[index];

			mapList.RemoveAt(index);

			if (index == (mapListPtr[cmdPtr]+1))
				Parse.JoinNode(mapList, mapListPtr[cmdPtr]);
		}

		private void RedoNewChild(List<string> mapList)
		{
			int stopIndex = Parse.FindNodeEnd(mapList, mapListPtr[cmdPtr]);

			if (mapListPtr[cmdPtr] == stopIndex)
				stopIndex = Parse.SplitNode(mapList, mapListPtr[cmdPtr]);

			mapList.Insert(stopIndex, strList[strListPtr]);
			
			strListPtr++;
		}

		public void AddNote(List<string> mapList, int startIndex, List<string> listNote)
		{
			// listNote an folgender Position einfügen:
			// - vor einem Icon (nur 0.9'er Map), oder
			// - vor einem Childknoten, oder
			// - am Ende des Knotens

			int stopIndex = Parse.FindNodeEnd(mapList, startIndex);  // Ende des Knotens finden
			int noteIndex = -1;                                      // Index des Icons
			bool htmlNote = mapList[0].StartsWith("<map version=\"0.9");

			mapList[startIndex] = Parse.UpdateModificationTime(mapList[startIndex]);
			helpIndex[cmdPtr] = startIndex;

			if (startIndex == stopIndex)  // einzeiliger Knoten
			{
				command[cmdPtr] = ADDNOTESPLIT;                    // neues Kommando eintragen
				noteIndex = Parse.SplitNode(mapList, startIndex);  // Knoten aufspalten
			}
			else  // mehrzeiliger Knoten
			{
				command[cmdPtr] = ADDNOTE;                       // neues Kommando eintragen
				for (int i = startIndex+1; i <= stopIndex; i++)  // Knoteninneres absuchen
				{
					if ((mapList[i].StartsWith("<icon") && htmlNote) ||  // Icon mit 0.9'er Map, oder
					     mapList[i].StartsWith("<node") ||               // Childknoten beginnt, oder
					     mapList[i].StartsWith("</node"))                // Knoten ist zu Ende
					{
						noteIndex = i;  // neue Note hier einfügen

						break;
					}
				}
			}

			mapListPtr[cmdPtr] = noteIndex;        // zeigt auf die Einfügestelle
			nrOfStrings[cmdPtr] = listNote.Count;  // Anzahl der Strings
			strList.RemoveRange(strListPtr, strList.Count-strListPtr);  // strList abschneiden

			// listNote reinkopieren
			for (int i = listNote.Count-1; i >= 0 ; i--)  // von hinten aufrollen
			{
				strList.Insert(strListPtr, listNote[i]);  // in die strList
				mapList.Insert(noteIndex, listNote[i]);   // in die mapList
			}

			strListPtr += listNote.Count;  // strListPtr ans Ende der Liste

			UpdateCmdPointers();
		}

		private void UndoAddNote(List<string> mapList)
		{
			int count = nrOfStrings[cmdPtr];  // Anzahl der zu löschenden Strings holen

			strListPtr -= count;  // strListPtr entsprechend runtersetzen

			for (int i = 0; i < count; i++)  // count Zeilen löschen
				mapList.RemoveAt(mapListPtr[cmdPtr]);

			mapList[helpIndex[cmdPtr]] = Parse.UpdateModificationTime(mapList[helpIndex[cmdPtr]]);
		}

		private void UndoAddNoteSplit(List<string> mapList)
		{
			UndoAddNote(mapList);  // Note entfernen

			Parse.JoinNode(mapList, --mapListPtr[cmdPtr]);  // Knoten vereinigen
		}

		private void RedoAddNote(List<string> mapList)
		{
			int count = nrOfStrings[cmdPtr];  // Anzahl der gelöschten Strings

			// die gelöschten Strings wieder herstellen
			// mapListPtr zeigt schon auf die richtige Position
			// Letzter wird zuerst eingefügt
			strListPtr += count;
			for (int i = 1; i <= count; i++)
			{
				mapList.Insert(mapListPtr[cmdPtr], strList[strListPtr-i]);
			}

			mapList[helpIndex[cmdPtr]] = Parse.UpdateModificationTime(mapList[helpIndex[cmdPtr]]);
		}

		private void RedoAddNoteSplit(List<string> mapList)
		{
			// zunächst muss der vereinigte Knoten wieder aufgesplittet werden
			// mapListPtr zeigt auf diesen Knoten
			// anschließend zeigt mapListPtr auf die Einfügeposition
			mapListPtr[cmdPtr] = Parse.SplitNode(mapList, mapListPtr[cmdPtr]);

			// jetzt passt alles für RedoAddNote()
			RedoAddNote(mapList);
		}

		public void DeleteNote(List<string> mapList, int startIndex)
		{
			int noteStart = 0;
			int noteStop  = 0;

			if (Parse.FindNote(mapList, startIndex, ref noteStart, ref noteStop) == Parse.NONE)
				return;  // Note-Lage ;-) ermitteln, und wenns keine Note gibt returnen

			int count = noteStop - noteStart + 1;  // Anzahl der Zeilen

			// strList ab aktuellem strListPtr löschen
			strList.RemoveRange(strListPtr, strList.Count-strListPtr);

			for (int i = 0; i < count; i++)  // count Zeilen von mapList nach strList verschieben
			{
				strList.Add(mapList[noteStart]);  // Zeile kopieren
				mapList.RemoveAt(noteStart);      // Original löschen
			}

			strListPtr += count;          // strListPtr updaten
			nrOfStrings[cmdPtr] = count;  // Anzahl der gelöschten Strings merken

			// umschließenden Knoten bei Bedarf vereinigen
			if (mapList[noteStart].StartsWith("</node") &&
			    mapList[noteStart-1].StartsWith("<node") &&
			    !mapList[noteStart-1].EndsWith("/>"))
			{
				noteStart--;
				Parse.JoinNode(mapList, noteStart);
				command[cmdPtr] = DELETENOTEJOIN;
			}
			else
				command[cmdPtr] = DELETENOTE;
			
			// DELETENOTEJOIN : mapListPtr zeigt auf den vereinigten Knoten
			// DELETENOTE     : mapListPtr zeigt dorthin, wo eingefügt werden soll
			mapListPtr[cmdPtr] = noteStart;

			mapList[startIndex] = Parse.UpdateModificationTime(mapList[startIndex]);
			helpIndex[cmdPtr] = startIndex;

			UpdateCmdPointers();
		}
		public void AddIcon(List<string> mapList, int startIndex, string line)
		{
			// line an folgender Position einfügen:
			// - hinter dem letzten Icon, oder
			// - vor einem hook, oder
			// - vor einem Childknoten, oder
			// - am Ende des Knotens

			int stopIndex = Parse.FindNodeEnd(mapList, startIndex);  // Ende des Knotens finden
			int iconIndex = -1;                                      // Index des Icons

			mapList[startIndex] = Parse.UpdateModificationTime(mapList[startIndex]);
			helpIndex[cmdPtr] = startIndex;

			if (startIndex == stopIndex)  // einzeiliger Knoten
			{
				command[cmdPtr] = ADDICONSPLIT;  // neues Kommando eintragen
				iconIndex = Parse.SplitNode(mapList, startIndex);  // Knoten aufspalten
			}
			else  // mehrzeiliger Knoten
			{
				command[cmdPtr] = ADDICON;  // neues Kommando eintragen
				for (int i = startIndex+1; i <= stopIndex; i++)  // Knoteninneres absuchen
				{
					if (mapList[i].StartsWith("<hook") ||  // Note
						mapList[i].StartsWith("<node") ||  // Childknoten beginnt, oder
					    mapList[i].StartsWith("</node"))   // Knoten ist zu Ende
					{
						if (iconIndex == -1)  // wenn kein Icon gefunden wurde
							iconIndex = i;    // neues Icon hier einfügen

						break;
					}
	
					if (mapList[i].StartsWith("<icon"))  // Iconposition merken
						iconIndex = i+1;
				}
			}

			mapList.Insert(iconIndex, line);  // Icon einfügen

			mapListPtr[cmdPtr] = iconIndex;   // zeigt auf eingefügte Zeile
			nrOfStrings[cmdPtr] = 1;          // Platzhalter für Undo
			strList.RemoveRange(strListPtr, strList.Count-strListPtr);  // strList nach oben löschen
			strList.Add(mapList[iconIndex]);  // Platzhalter für Undo
			strListPtr++;                     // strListPtr anpassen

			UpdateCmdPointers();
		}

		private void UndoAddIcon(List<string> mapList, int command)
		{
			int iconIndex = mapListPtr[cmdPtr];

			strListPtr--;
			strList[strListPtr] = mapList[iconIndex];  // diese Zeile merken

			mapList.RemoveAt(iconIndex);  // diese Zeile entfernen

			// umschließenden Knoten bei Bedarf vereinigen
			if (command == ADDICONSPLIT)
			{
				mapListPtr[cmdPtr] = iconIndex-1;
				Parse.JoinNode(mapList, iconIndex-1);  // Knoten vereinen
			}

			mapList[helpIndex[cmdPtr]] = Parse.UpdateModificationTime(mapList[helpIndex[cmdPtr]]);
		}

		private void RedoAddIcon(List<string> mapList, int command)
		{
			int iconIndex = mapListPtr[cmdPtr];

			if (command == ADDICONSPLIT)
			{
				iconIndex = Parse.SplitNode(mapList, iconIndex);
				mapListPtr[cmdPtr] = iconIndex;
			}

			mapList.Insert(iconIndex, strList[strListPtr]);  // Zeile einfügen
			
			strListPtr++;

			mapList[helpIndex[cmdPtr]] = Parse.UpdateModificationTime(mapList[helpIndex[cmdPtr]]);
		}

		public void DeleteLastIcon(List<string> mapList, int startIndex)
		{
			int lastIconIndex = Parse.FindLastIcon(mapList, startIndex);

			if (lastIconIndex == -1) return;  // darf nicht vorkommen

			mapList[startIndex] = Parse.UpdateModificationTime(mapList[startIndex]);
			helpIndex[cmdPtr] = startIndex;

			// strList ab aktuellem strListPtr löschen
			strList.RemoveRange(strListPtr, strList.Count-strListPtr);
			// eine Zeile von mapList nach strList
			strList.Add(mapList[lastIconIndex]);
			strListPtr++;
			mapList.RemoveAt(lastIconIndex);

			// umschließenden Knoten bei Bedarf vereinigen
			if (mapList[lastIconIndex].StartsWith("</node") &&
			    mapList[lastIconIndex-1].StartsWith("<node") &&
			    !mapList[lastIconIndex-1].EndsWith("/>"))
			{
				lastIconIndex--;
				Parse.JoinNode(mapList, lastIconIndex);
				command[cmdPtr] = DELETELASTICONJOIN;
			}
			else
				command[cmdPtr] = DELETELASTICON;
			
			// DELETELASTICONJOIN : mapListPtr zeigt auf den vereinigten Knoten
			// DELETELASTICON     : mapListPtr zeigt dorthin, wo eingefügt werden soll
			mapListPtr[cmdPtr] = lastIconIndex;
			// Anzahl der gelöschten Strings merken
			nrOfStrings[cmdPtr] = 1;
			
			UpdateCmdPointers();
		}
	}
}
