/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Peter
 * Datum: 27.03.2009
 * Zeit: 22:18
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */

using System;

namespace FreeYourMind
{
	/// <summary>
	/// Description of Texts.
	/// </summary>
	public class Texts
	{
		public int RestartForChanges  = 0;
		public int Cancel             = 1;
		public int EditDotDotDot      = 2;
		public int NewNode            = 3;
		public int Sister             = 4;
		public int Child              = 5;
		public int Parent             = 6;
		public int Brother            = 7;
		public int Note               = 8;
		public int NewViewEdit        = 9;
		public int Delete             = 10;
		public int Icons              = 11;
		public int Add                = 12;
		public int Encryption         = 13;
		public int EncryptDecrypt     = 14;
		public int Remove             = 15;
		public int MoveUp             = 16;
		public int MoveDown           = 17;
		public int Undo               = 18;
		public int Redo               = 19;
		public int Cut                = 20;
		public int Copy               = 21;
		public int PasteAs            = 22;
		public int Menu               = 23;
		public int File               = 24;
		public int New                = 25;
		public int Open               = 26;
		public int Save               = 27;
		public int SaveAs             = 28;
		public int Close              = 29;
		public int FindDotDotDot      = 30;
		public int FindNext           = 31;
		public int CollapseTree       = 32;
		public int ExpandTree         = 33;
		public int OptionsDotDotDot   = 34;
		public int UnlockDotDotDot    = 35;
		public int AboutDotDotDot     = 36;
		public int Exit               = 37;
		public int CouldNotFind       = 38;
		public int Message            = 39;
		public int DemoMessage        = 40;
		public int DemoVersion        = 41;
		public int EnterSearchString  = 42;
		public int Find               = 43;
		public int Language           = 44;
		public int Options            = 45;
		public int UnlockedMessage    = 46;
		public int Unlock             = 47;
		public int EnterLicenceKey    = 48;
		public int PurchaseLicenceKey = 49;
		public int InvalidLicenceKey  = 50;
		public int UnlockedNow        = 51;
		public int AboutText1         = 52;
		public int AboutText2         = 53;
		public int About              = 54;
		public int Info               = 55;
		public int SaveChanges        = 56;
		public int UnsavedChanges     = 57;
		public int OverwriteFile      = 58;
		public int SaveFile           = 59;
		public int Paste              = 60;
		public int Edit               = 61;
		public int EnterPassword      = 62;
		public int Password           = 63;
		public int EnterPasswordTwice = 64;
		public int PasswordsNotIdent  = 65;
		public int LoseChanges        = 66;
		public int CancelEdit         = 67;
		public int ClickOnIcon        = 68;
		public int AddIcon            = 69;
		public int NotEnoughMemory    = 70;
		public int Warning            = 71;
		public int PaintedOver        = 72;
		public int EditText           = 73;
		public int EditNote           = 74;
		public int NewChild           = 75;
		public int NewSister          = 76;
		public int NewParent          = 77;
		public int NewBrother         = 78;
		public int DeleteNode         = 79;
		public int PasteSister        = 80;
		public int PasteChild         = 81;
		public int DeleteIcon         = 82;
		public int AddNote            = 83;
		public int DeleteNote         = 84;
		public int Decryption         = 85;
		public int RemoveEncryption   = 86;
		public int DecryptionFailed   = 87;
		public int Error              = 88;
		public int FrenchText         = 89;
		public int FontSize           = 90;
		public int ExtraLarge         = 91;
		public int Large              = 92;
		public int Medium             = 93;
		public int Small              = 94;
		public int ExtraSmall         = 95;
		public int Navigator          = 96;
		public int NodeInfo           = 97;
		public int OpenRecent         = 98;
		public int Pim                = 99;
		public int Task               = 100;
		public int Appointment        = 101;
		public int Textsearch         = 102;
		public int Created            = 103;
		public int Modified           = 104;
		public int Till               = 105;
		public int At                 = 106;
		public int Since              = 107;
		public int QuickContext       = 108;
		public int Text               = 109;
		public int Background         = 110;
		public int Format             = 111;
		public int Standard           = 112;
		//t.t[t.FrenchText, language]

		public string[,] t = new string[113,5];

		private const int DE = 0;
		private const int EN = 1;
		private const int FR = 2;
		private const int IT = 3;
		private const int NL = 4;
		// 0 Deutsch
		// 1 Englisch
		// 2 Französisch
		// 3 Italienisch
		// 4 Niederländisch

		public Texts()
		{
			t[112,DE] = "Standard";
			t[112,EN] = "Standard";
			t[112,FR] = "Standard";
			t[112,IT] = "Standard";
			t[112,NL] = "Standaard"; 

			t[111,DE] = "Format";
			t[111,EN] = "Format";
			t[111,FR] = "Format";
			t[111,IT] = "Formato";
			t[111,NL] = "Formaat"; 

			t[110,DE] = "Hintergrund";
			t[110,EN] = "Background";
			t[110,FR] = "Fond";
			t[110,IT] = "Sfondo";
			t[110,NL] = "Achtergrond"; 

			t[109,DE] = "Text";
			t[109,EN] = "Text";
			t[109,FR] = "Texte";
			t[109,IT] = "Testo";
			t[109,NL] = "Tekst"; 

			t[108,DE] = "Quick Context Menu";  // TODO: übersetzen?
			t[108,EN] = "Quick Context Menu";  // TODO: Fr. Blieks Vorschlag: T-Option
			t[108,FR] = "Quick Context Menu";
			t[108,IT] = "Quick Context Menu";
			t[108,NL] = "Quick Context Menu"; 

			t[107,DE] = "seit";  // gemeint ist "seit einschließlich"
			t[107,EN] = "since";  // TODO: Fr. Bliek: Wie wäre es mit "nach dem"? Dann ließe sich das in allen Sprachen klar verstehen und eingeben...
			t[107,FR] = "depuis le";
			t[107,IT] = "fino dal";
			t[107,NL] = "sinds"; 

			t[106,DE] = "am";
			t[106,EN] = "at";
			t[106,FR] = "le"; 
			t[106,IT] = "il";
			t[106,NL] = "op"; 

			t[105,DE] = "bis";  // gemeint ist "bis einschließlich"
			t[105,EN] = "till";
			t[105,FR] = "avant le";
			t[105,IT] = "fino al"; 
			t[105,NL] = "tot"; 

			t[104,DE] = "Geändert";
			t[104,EN] = "Modified";
			t[104,FR] = "Modifié"; 
			t[104,IT] = "Modificato"; 
			t[104,NL] = "Veranderd";

			t[103,DE] = "Erstellt";
			t[103,EN] = "Created";
			t[103,FR] = "Crée";
			t[103,IT] = "Creato";
			t[103,NL] = "Opgesteld";

			t[102,DE] = "Textsuche";
			t[102,EN] = "Text search";
			t[102,FR] = "Recherche (texte)";
			t[102,IT] = "Ricerca (testo)";
			t[102,NL] = "Tekst zoeken"; 

			t[101,DE] = "Termin";
			t[101,EN] = "Appointment";
			t[101,FR] = "Évènement";
			t[101,IT] = "Appuntamento";
			t[101,NL] = "Appointment";  // TODO: Übersetzung fehlt

			t[100,DE] = "Aufgabe";
			t[100,EN] = "Task";
			t[100,FR] = "Tâche";
			t[100,IT] = "Task";  // TODO: Übersetzung ok?
			t[100,NL] = "Task";  // TODO: Übersetzung fehlt

			t[99,DE] = "PIM";  // Personal Information Manager
			t[99,EN] = "PIM";
			t[99,FR] = "PIM";  // TODO: Übersetzung ok?
			t[99,IT] = "PIM";  // TODO: Übersetzung ok?
			t[99,NL] = "PIM";  // TODO: Übersetzung ok?

			t[98,DE] = "Letzte Datei wieder öffnen";
			t[98,EN] = "Open recent file at start-up";
			t[98,FR] = "Auto-ouvrir dernière carte";
			t[98,IT] = "Auto-apri ultima mappa";
			t[98,NL] = "Auto-open laatste bestand";

			t[97,DE] = "Knoteninfo";
			t[97,EN] = "Node info";
			t[97,FR] = "Info nœud";
			t[97,IT] = "Info nodo";
			t[97,NL] = "Knoop info"; 

			t[96,DE] = "Navigator";
			t[96,EN] = "Navigator";
			t[96,FR] = "Navigateur";
			t[96,IT] = "Navigatore";
			t[96,NL] = "Navigator";

			t[95,DE] = "sehr klein";
			t[95,EN] = "extra small";
			t[95,FR] = "très petite";
			t[95,IT] = "molto piccola";
			t[95,NL] = "zeer klein";

			t[94,DE] = "klein";
			t[94,EN] = "small";
			t[94,FR] = "petite";
			t[94,IT] = "piccola";
			t[94,NL] = "klein";

			t[93,DE] = "mittel";
			t[93,EN] = "medium";
			t[93,FR] = "moyenne";
			t[93,IT] = "media";
			t[93,NL] = "medium";

			t[92,DE] = "groß";
			t[92,EN] = "large";
			t[92,FR] = "grand";
			t[92,IT] = "grande";
			t[92,NL] = "groot";

			t[91,DE] = "sehr groß";
			t[91,EN] = "extra large";
			t[91,FR] = "extra grande";
			t[91,IT] = "molto grande";
			t[91,NL] = "extra groot";

			t[90,DE] = "Schriftgröße";
			t[90,EN] = "Font size";
			t[90,FR] = "Caractère";
			t[90,IT] = "Carattere";
			t[90,NL] = "Lettergrootte";

//			t[90,DE] = "_ Schriftgröße";
//			t[90,EN] = "_ Font size";
//			t[90,FR] = "_ Caractères : taille";
//			t[90,IT] = "_ Carattere : dimensione";
//			t[90,NL] = "_ Lettergrootte";

			t[89,DE] = "Französische und niederländische Texte:\n" + 
			          "Ilga Bliek\n" +          
			          //"blieki@gymsul.ni.lo-net2.de";
			          "http://gymsul.ni.lo-net2.de/blieki";
			t[89,EN] = "French and Dutch texts:\n" +
			          "Ilga Bliek\n" + 
			          //"blieki@gymsul.ni.lo-net2.de";
			          "http://gymsul.ni.lo-net2.de/blieki";
			t[89,FR] = "Version française et néerlandaise :\n" +
			          "Ilga Bliek\n" + 
			          //"blieki@gymsul.ni.lo-net2.de";
			          "http://gymsul.ni.lo-net2.de/blieki";
			t[89,IT] = "Versione italiana:\n" +
			          "Ilga Bliek\n" + 
			          //"blieki@gymsul.ni.lo-net2.de";
			          "http://gymsul.ni.lo-net2.de/blieki";
			t[89,NL] = "Franse en Nederlandse versie:\n" +
			          "Ilga Bliek\n" + 
			          //"blieki@gymsul.ni.lo-net2.de";
			          "http://gymsul.ni.lo-net2.de/blieki";

			
			t[88,DE] = "Fehler";  // Titel eines Meldungsfensters
			t[88,EN] = "Error";
			t[88,FR] = "Erreur";
			t[88,IT] = "Errore";
			t[88,NL] = "Fout";

			t[87,DE] = "Entschlüsselung fehlgeschlagen!\n" +  // Text eines Meldungsfensters
			          "Eventuell falsches Kennwort.";
			t[87,EN] = "Decryption failed!\n" +
			          "Possibly wrong password.";
			t[87,FR] = "Décryptage impossible!\n" +
			          "Peut-être faux mot de passe.";
			t[87,IT] = "Non posso dicifrare!\n" +
			          "Password scorreta?";
			t[87,NL] = "Kan encryptie niet uitschakelen!\n" +
			          "Wachtwoord verkeert?";

			t[86,DE] = "Verschl. entfernen";  // Diese Texte erscheinen im Kontextmenü
			t[86,EN] = "Remove encryption";   // Beispiel: "Wiederherstellen: Verschl. entfernen"
			t[86,FR] = "Enlever l'encryption";// Diese Texte dürfen nicht zu lang werden
			t[86,IT] = "Togliere codificatione"; 
			t[86,NL] = "Encryptie uitschakelen"; 

			t[85,DE] = "Entschlüsselung";  // dto.
			t[85,EN] = "Decryption";
			t[85,FR] = "Décryptage";
			t[85,IT] = "Dicifrare";
			t[85,NL] = "Geen encryptie";

			t[84,DE] = "Notiz löschen";  // dto.
			t[84,EN] = "Delete note";
			t[84,FR] = "Supprimer note";
			t[84,IT] = "Cancellare nota";
			t[84,NL] = "Aantekening verw.";

			t[83,DE] = "Notiz hinzufügen";  // dto.
			t[83,EN] = "Add note";
			t[83,FR] = "Annoter";
			t[83,IT] = "Annotare";
			t[83,NL] = "Aantekenen";

			t[82,DE] = "Icon löschen";  // dto.
			t[82,EN] = "Delete icon";
			t[82,FR] = "Retirer icône";
			t[82,IT] = "Rimuovare icone";
			t[82,NL] = "Icoontje verw.";

			t[81,DE] = "Knoten danach einfügen";  //
			t[81,EN] = "Paste node behind";
			t[81,FR] = "Insérer nœud dessous";
			t[81,IT] = "Nuovo nodo dopo";
			t[81,NL] = "Nieuwe onderl. knoop";

			t[80,DE] = "Knoten oben einfügen";  // dto.
			t[80,EN] = "Paste node above";
			t[80,FR] = "Insérer nœud dessus";
			t[80,IT] = "Nuovo nodo sopra";
			t[80,NL] = "Nieuwe knoop ervoor";

			t[79,DE] = "Knoten löschen";  // dto.
			t[79,EN] = "Delete node";
			t[79,FR] = "Supprimer nœud";
			t[79,IT] = "Rimuovare nodo";
			t[79,NL] = "Knoop verwÿdern";

			t[78,DE] = "Neuer Knoten unten";  // dto.
			t[78,EN] = "New node below";
			t[78,FR] = "Nouveau nœud après";
			t[78,IT] = "Nuovo nodo sotto";
			t[78,NL] = "Nieuwe knoop ernaar";

			t[77,DE] = "Neuer Knoten davor";  // dto.
			t[77,EN] = "New node before";
			t[77,FR] = "Nouveau nœud avant";
			t[77,IT] = "Nuovo nodo davanti";
			t[77,NL] = "Nieuwe bovenl. knoop";

			t[76,DE] = "Neuer Knoten oben";  // dto.
			t[76,EN] = "New node above";
			t[76,FR] = "Nouveau nœud dessus";
			t[76,IT] = "Nuovo nodo sopra";
			t[76,NL] = "Nieuwe knoop ervoor";

			t[75,DE] = "Neuer Knoten danach";  // dto.
			t[75,EN] = "New node behind";
			t[75,FR] = "Nouveau nœud après";
			t[75,IT] = "Nuovo nodo dopo";
			t[75,NL] = "Nieuwe onderl. knoop";

			t[74,DE] = "Notiz bearbeiten";  // dto.
			t[74,EN] = "Edit note";
			t[74,FR] = "Éditer note";
			t[74,IT] = "Modificare nodo";
			t[74,NL] = "Aantekening bewerken";

			t[73,DE] = "Text bearbeiten";  // dto.
			t[73,EN] = "Edit text";
			t[73,FR] = "Éditer texte";
			t[73,IT] = "Modificare testo";
			t[73,NL] = "Text bewerken";

			t[72,DE] = "Demoversion!\n" +               // erscheint im übermalten Bereich der Mindmap
			          "Dieser Bereich ist übermalt.";
			t[72,EN] = "Demo version!\n" +
			          "This area is painted over.";
			t[72,FR] = "Version démo!\n" +
			          "Cette partie est recouvrie.";
			t[72,IT] = "Versione di prova!\n" +
			          "Questa parte è nasconda.";
			t[72,NL] = "Demo versie!\n" +
			          "Dit gedeelte is overschilderd.";

			t[71,DE] = "Achtung";  // Titel eines Meldungsfensters
			t[71,EN] = "Warning";
			t[71,FR] = "Attention";
			t[71,IT] = "Attentione";
			t[71,NL] = "Opgelet";

			t[70,DE] = "Nicht genügend Speicher um diese Aktion auszuführen!";  // Text eines Meldungsfensters
			t[70,EN] = "Not enough memory to perform this action!";
			t[70,FR] = "Pas assez de mémoire pour exécuter cette action!";
			t[70,IT] = "Capacità di memoria non sufficiente per questo!";
			t[70,NL] = "Niet genoeg geheugen voor deze aktie!";

			t[69,DE] = "Icon hinzufügen";  // Titel des Iconhinzufügefensters
			t[69,EN] = "Add icon";
			t[69,FR] = "Insérer icône";
			t[69,IT] = "Inseriscere icone";
			t[69,NL] = "Icoontje invoegen";

			t[68,DE] = "Klicken Sie auf ein Icon um es hinzuzufügen.";  // Text im Iconhinzufügefenster
			t[68,EN] = "Click on an icon to add it.";
			t[68,FR] = "Cliquer sur une icône pour l'insérer.";
			t[68,IT] = "Cliccare su l'icone per sceglierla.";
			t[68,NL] = "Klik op het icoontje om het te kiezen.";

			t[67,DE] = "Bearbeiten abbrechen";  // Titel eines Meldungsfensters
			t[67,EN] = "Cancel edit";
			t[67,FR] = "Annuler";
			t[67,IT] = "Annulla";
			t[67,NL] = "Annuleer";

			t[66,DE] = "Die Änderungen gehen verloren.\n" +  // Text eines Meldungsfensters
			          "Fortsetzen?";
			t[66,EN] = "Your changes will be lost.\n" +
			          "Continue anyway?";
			t[66,FR] = "Vos changements seront perdus.\n" +
			          "Continuer?";
			t[66,IT] = "Gli modificazione si perdono.\n" +
			          "Continuare?";
			t[66,NL] = "Veranderingen zallen verdwenen.\n" +
			          "Verder gaan?";

			t[65,DE] = "Entweder sind die zwei Kennwörter nicht identisch " +  // Text eines Meldungsfensters
			          "oder sie sind zu kurz.";
			t[65,EN] = "Either the two passwords are not identical " +
			          "or they are too short.";
			t[65,FR] = "Soit les deux mots de passe sont différents " +
			          "soit trop courts.";
			t[65,IT] = "O password non è identico " +
			          "o troppo breve.";
			t[65,NL] = "Of de wachtwoorden zÿn niet gelÿk " +
			          "of te kort.";

			t[64,DE] = "Geben Sie zweimal das Kennwort ein.";
			t[64,EN] = "Please enter the password twice.";
			t[64,FR] = "Entrez le mot de passe deux fois, svp.";
			t[64,IT] = "Digitare password due volte, per favore.";
			t[64,NL] = "Wachtwoord twee keer geven, a.u.b..";

			t[63,DE] = "Kennwort";  // Titel eines Fensters
			t[63,EN] = "Password";
			t[63,FR] = "Mot de passe";
			t[63,IT] = "Password";
			t[63,NL] = "Wachtwoord";

			t[62,DE] = "Geben Sie das Kennwort ein.";
			t[62,EN] = "Please enter the password.";
			t[62,FR] = "Entrez le mot de passe, svp.";
			t[62,IT] = "Digitare password, per favore";
			t[62,NL] = "Wachtwoord geven, a.u.b.";

			t[61,DE] = "Bearbeiten";
			t[61,EN] = "Edit";
			t[61,FR] = "Éditer";
			t[61,IT] = "Modificare";
			t[61,NL] = "Bewerken";

			t[60,DE] = "Einfügen";
			t[60,EN] = "Paste";
			t[60,FR] = "Coller";
			t[60,IT] = "Incollare";
			t[60,NL] = "Plakken";

			t[59,DE] = "Datei speichern";
			t[59,EN] = "Save file";
			t[59,FR] = "Enregistrer";
			t[59,IT] = "Salvare";
			t[59,NL] = "Opslaan";

			t[58,DE] = "Diese Datei ist bereits vorhanden.\n" +
			          "Möchten Sie sie ersetzen?";
			t[58,EN] = "This file already exists.\n" +
			          "Do you want to replace it?";
			t[58,FR] = "Ce fichier existe.\n" +
			          "Voulez-vous le replacer?";
			t[58,IT] = "Mappa esiste già.\n" +
			          "Sovrascriverla? ";
			t[58,NL] = "De mindmap bestaat al.\n" +
			          "Wilt u deze overschrÿven?";

			t[57,DE] = "Mindmap geändert";
			t[57,EN] = "Unsaved changes";
			t[57,FR] = "Carte modifiée";
			t[57,IT] = "Mappa modificata";
			t[57,NL] = "Mindmap veranderd";

			t[56,DE] = "Änderungen speichern?\n";
			t[56,EN] = "Save changes to file?\n";
			t[56,FR] = "Enregister la carte?";
			t[56,IT] = "Salvare la mappa?";
			t[56,NL] = "Mindmap opslaan?";

			t[55,DE] = "Info";  // Titel eines allgemeinen Informationsfensters
			t[55,EN] = "Info";
			t[55,FR] = "Info";
			t[55,IT] = "Info";
			t[55,NL] = "Info";

			t[54,DE] = "Info";  // Titel des "About"-Meldungsfensters
			t[54,EN] = "About";
			t[54,FR] = "À propos";
			t[54,IT] = "Info";
			t[54,NL] = "Info";

			t[53,DE] = "Peter Schmidt, München.\n" +
			          "Alle Rechte vorbehalten.";
			t[53,EN] = "Peter Schmidt, Munich.\n" +
			          "All rights reserved.";
			t[53,FR] = "Peter Schmidt, Munich.\n" +
			          "Tous droits réservés.";
			t[53,IT] = "Peter Schmidt, Monaco.\n" +
			          "Tutti i diritti riservati.";
			t[53,NL] = "Peter Schmidt, München.\n" +
			          "Alle rechten voorbehouden.";

			t[52,DE] = "FYMgraphics ist eine\n" +
			          "Mindmapping Software\n" +
			          "für Pocket PCs.\n";
			t[52,EN] = "FYMgraphics is a\n" +
			          "mind mapping software\n" +
			          "for Pocket PCs.\n";
			t[52,FR] = "FYMgraphics est un\n" +
			          "logiciel de cartes mentales\n" +
			          "pour Pocket PC.\n";
			t[52,IT] = "FYMgraphics - applicatovo\n" +
			          "per mappe mentale\n" +
			          "su Pocket PC.\n";
			t[52,NL] = "FYMgraphics - mindmaps\n" +
			          "maken en bekÿken\n" +
			          "met een Pocket PC.\n";

			t[51,DE] = "FYMgraphics ist nun freigeschaltet.\n" +
			          "Vielen Dank.";
			t[51,EN] = "FYMgraphics is unlocked now.\n" +
			          "Thanks for purchasing it.";
			t[51,FR] = "FYMgraphics est activé.\n" +
			          "Merci.";
			t[51,IT] = "FYMgraphics è sbloccata.\n" +
			          "Tante grazie.";
			t[51,NL] = "FYMgraphics is vrÿgegeven.\n" +
			          "Dank u wel.";

			t[50,DE] = "Ungültiger Freischaltcode!";
			t[50,EN] = "Invalid licence key!";
			t[50,FR] = "Clé d´enregistrement non valable!"; //d´enregistrement könnte aus Platzgründen entfallen
			t[50,IT] = "Codice seriale non valido!";
			t[50,NL] = "Registratiesleutel niet bruikbaar!";

			t[49,DE] = "Sie können einen Freischaltcode unter\n" +
			          "http://peterschmidt.name\n" +
			          "erwerben.";
			t[49,EN] = "If you don't have a licence key\n" +
			          "you can purchase one at\n" +
			          "http://peterschmidt.name";
			t[49,FR] = "Pas de clé d'enregistrement?\n" +
			          "Achetez en chez\n" +
			          "http://peterschmidt.name";
			t[49,IT] = "Un codice seriale \n" +
			          "si compro su:\n" +
			          "http://peterschmidt.name";
			t[49,NL] = "Geen registratiesleutel?\n" +
			          "Koop het programma hier:\n" +
			          "http://peterschmidt.name";

			t[48,DE] = "Bitte geben Sie hier\n" +
			          "Ihren Freischaltcode ein.";
			t[48,EN] = "Please enter your licence key\n" +
			          "to unlock all features\n" +
			          "of FYMgraphics.";
			t[48,FR] = "Entrez votre clé d'enregistrement\n" +
			          "pour fonctionnement complet\n" +
			          "de FYMgraphics.";
			t[48,IT] = "Inserire codice seriale\n" +
			          "per sbloccarne le funzioni\n" +
			          "de FYMgraphics.";
			t[48,NL] = "Registratiesleutel geven\n" +
			          "voorvolledige functionaliteit\n" +
			          "van FYMgraphics.";

			t[47,DE] = "Freischalten";
			t[47,EN] = "Unlock";
			t[47,FR] = "Enregister";
			t[47,IT] = "Sbloccare";
			t[47,NL] = "Vrÿgeven";

			t[46,DE] = "Diese Software ist freigeschaltet.\n" +
			          "Vielen Dank, dass Sie sie erworben haben.";
			t[46,EN] = "This software is unlocked.\n" +
			          "Thanks for purchasing it.";
			t[46,FR] = "Le logiciel est activé.\n" +
			          "Merci de l'avoir acheté.";
			t[46,IT] = "Applicazione è sbloccata.\n" +
			          "Grazie per averla comprata.";
			t[46,NL] = "Het programma is vrÿgegeven.\n" +
			          "Bedankt dat u het heeft gekoopt.";

			t[45,DE] = "Optionen";
			t[45,EN] = "Options";
			t[45,FR] = "Propriétés";
			t[45,IT] = "Preferenze";
			t[45,NL] = "Instellingen";

			t[44,DE] = "Sprache";
			t[44,EN] = "Language";
			t[44,FR] = "Langue";
			t[44,IT] = "Lingua";
			t[44,NL] = "Taal";

//			t[44,DE] = "_ Sprache";
//			t[44,EN] = "_ Language";
//			t[44,FR] = "_ Langue";
//			t[44,IT] = "_ Lingua";
//			t[44,NL] = "_ Taal";

			t[43,DE] = "Suchen";
			t[43,EN] = "Find";
			t[43,FR] = "Chercher";
			t[43,IT] = "Cercare";
			t[43,NL] = "Zoeken";

			t[42,DE] = "Suchbegriff:";
			t[42,EN] = "Enter a search string:";
			t[42,FR] = "Que chercher?";
			t[42,IT] = "Che cercare?";
			t[42,NL] = "Wat wilt u zoeken?";

			t[41,DE] = "Demo Version";
			t[41,EN] = "Demo version";
			t[41,FR] = "Version démo";
			t[41,IT] = "Versione di prova";
			t[41,NL] = "Demo versie";

			// TODO: ein Teil der Mindmap ist übermalt
			t[40,DE] = "Dieses ist die Demoversion von FYMgraphics.\n" +
			          "Die maximale Größe der Mindmap ist begrenzt, " +
			          "alle anderen Funktionen sind jedoch voll verfügbar.\n" +
			          "Die vollständige Version können Sie erwerben unter\n" +
			          "http://peterschmidt.name";
			t[40,EN] = "This is a demo version of FYMgraphics.\n" +
			          "The maximum size of the mind map is limited.\n" +
			          "All other features are available without any restrictions.\n" +
			          "You can purchase the full featured version at\n" +
			          "http://peterschmidt.name";
			t[40,FR] = "C'est une version démo de FYMgraphics.\n" +
			          "La taille maximum de la carte est limitée.\n" +
			          "À part cela, tout fonctionne.\n" +
			          "Pour l'activer Vous pouvez acheter ce logiciel chez\n" +
			          "http://peterschmidt.name";
			t[40,IT] = "Versione di prova dal programma FYMgraphics.\n" +
			          "Capienza maximale dei mappe mentale limitata.\n" +
			          "In oltre funzoni non limitati.\n" +
			          "La versione completa si acquista qui:\n" +
			          "http://peterschmidt.name";
			t[40,NL] = "Dit is een demo versie van FYMgraphics.\n" +
			          "Maximale grootte van mindmap berperkt.\n" +
			          "Behalve dit er is volledige functionaliteit.\n" +
			          "Een versie met volledige functionaliteit is hier te verkrÿgen:\n" +
			          "http://peterschmidt.name";

			t[39,DE] = "Meldung";  // Titel eines Meldungsfensters
			t[39,EN] = "Message";
			t[39,FR] = "Message";
			t[39,IT] = "Annuncio";
			t[39,NL] = "Mededeling";

			t[38,DE] = "\"{0}\" ist nicht vorhanden.";  // für {0} wird im Programmlauf ein Dateiname eingesetzt
			t[38,EN] = "Could not find \"{0}\".";
			t[38,FR] = "\"{0}\" ne peut pas être trouvé.";
			t[38,IT] = "\"{0}\" non esista."; 
			t[38,NL] = "\"{0}\" bestaat niet.";  

			t[37,DE] = "Beenden\tStrg+Q";  // Menüpunkt im Hauptmenü
			t[37,EN] = "Exit\tCtrl+Q";
			t[37,FR] = "Quitter\tCtrl+Q";
			t[37,IT] = "Chiudere\tCtrl+Q";
			t[37,NL] = "Afsluiten\tCtrl+Q";

			t[36,DE] = "Info...";  // dto.
			t[36,EN] = "About...";
			t[36,FR] = "À propos...";
			t[36,IT] = "Info...";
			t[36,NL] = "Info...";

			t[35,DE] = "Freischalten...";  // dto.
			t[35,EN] = "Unlock...";
			t[35,FR] = "Activer...";
			t[35,IT] = "Sbloccare...";
			t[35,NL] = "Vrÿgeven...";

			t[34,DE] = "Optionen...";  // dto.
			t[34,EN] = "Options...";
			t[34,FR] = "Propriétés...";
			t[34,IT] = "Preferenze...";
			t[34,NL] = "Instellen...";

			t[33,DE] = "Alles aufklappen";  // dto.
			t[33,EN] = "Expand tree";
			t[33,FR] = "Déplier tout";
			t[33,IT] = "Spandere tutti";
			t[33,NL] = "Alles uitvouwen";

			t[32,DE] = "Alles zuklappen";  // dto.
			t[32,EN] = "Collapse tree";
			t[32,FR] = "Plier tout";
			t[32,IT] = "Contrare tutti";
			t[32,NL] = "Alles opplooien";

			t[31,DE] = "Weitersuchen\tStrg+G";  // dto.
			t[31,EN] = "Find next\tCtrl+G";
			t[31,FR] = "Chercher suivant\tCtrl+G";
			t[31,IT] = "Cercare prossimo\tCtrl+G";
			t[31,NL] = "Volgende zoeken\tCtrl+G";

			t[30,DE] = "Suchen...\tStrg+F";  // dto.
			t[30,EN] = "Find...\tCtrl+F";
			t[30,FR] = "Chercher...\tCtrl+F";
			t[30,IT] = "Cercare...\tCtrl+F";
			t[30,NL] = "Zoeken...\tCtrl+F";

			t[29,DE] = "Schließen\tStrg+W";  // Menüpunkt im Datei-Menü
			t[29,EN] = "Close\tCtrl+W";
			t[29,FR] = "Fermer\tCtrl+W";
			t[29,IT] = "Chiudere\tCtrl+W";
			t[29,NL] = "Sluiten\tCtrl+W";

			t[28,DE] = "Speichern unter...\tStrg+Umschalt+S";  // dto.
			t[28,EN] = "Save as...\tCtrl+Shift+S";
			t[28,FR] = "Enregister sous...\tCtrl+Maj+S";
			t[28,IT] = "Salvare con nome...\tCtrl+Shift+S";
			t[28,NL] = "Opslaan als...\tCtrl+Shift+S";

			t[27,DE] = "Speichern\tStrg+S";  // dto.
			t[27,EN] = "Save\tCtrl+S";
			t[27,FR] = "Enregister\tCtrl+S";
			t[27,IT] = "Salvare\tCtrl+S";
			t[27,NL] = "Opslaan\tCtrl+S";

			t[26,DE] = "Öffnen...\tStrg+O";  // dto.
			t[26,EN] = "Open...\tCtrl+O";
			t[26,FR] = "Ouvrir...\tCtrl+O";
			t[26,IT] = "Aprire...\tCtrl+O";
			t[26,NL] = "Open...\tCtrl+O";

			t[25,DE] = "Neu\tStrg+N";  // dto.
			t[25,EN] = "New\tCtrl+N";
			t[25,FR] = "Nouveau\tCtrl+N";
			t[25,IT] = "Nuovo\tCtrl+N";
			t[25,NL] = "Nieuw\tCtrl+N";

			t[24,DE] = "Datei";  // Menüpunkt im Hauptmenü
			t[24,EN] = "File";
			t[24,FR] = "Fichier";
			t[24,IT] = "File";
			t[24,NL] = "Bestand";

			t[23,DE] = "Menü";  // Aufruf des Hauptmenüs in der unteren Menüleiste
			t[23,EN] = "Menu";
			t[23,FR] = "Menu";
			t[23,IT] = "Menu";
			t[23,NL] = "Menu";

			t[22,DE] = "Einfügen";  // Menüpunkt im Kontextmenü
			t[22,EN] = "Paste";
			t[22,FR] = "Coller";
			t[22,IT] = "Incolla";
			t[22,NL] = "Plakken";

			t[21,DE] = "Kopieren";  // dto.
			t[21,EN] = "Copy";
			t[21,FR] = "Copier";
			t[21,IT] = "Copia";
			t[21,NL] = "Kopiëer";

			t[20,DE] = "Ausschneiden";  // dto.
			t[20,EN] = "Cut";
			t[20,FR] = "Couper";
			t[20,IT] = "Talglia";
			t[20,NL] = "Knippen";

			t[19,DE] = "Wiederherst.";  // dto.
			t[19,EN] = "Redo";
			t[19,FR] = "Rétablir";
			t[19,IT] = "Ripristina";
			t[19,NL] = "Opnieuw";

			t[18,DE] = "Rückgängig";  // dto.
			t[18,EN] = "Undo";
			t[18,FR] = "Annuler";
			t[18,IT] = "Annula";
			t[18,NL] = "Annuleer";

			t[17,DE] = "Nach unten";  // dto.
			t[17,EN] = "Move down";
			t[17,FR] = "Descendre";
			t[17,IT] = "Giù";
			t[17,NL] = "Omlaag";

			t[16,DE] = "Nach oben";  // dto.
			t[16,EN] = "Move up";
			t[16,FR] = "Monter";
			t[16,IT] = "Su";
			t[16,NL] = "Omhoog";

			t[15,DE] = "Entfernen";  // dto.
			t[15,EN] = "Remove";
			t[15,FR] = "Supprimer";
			t[15,IT] = "Togliere";
			t[15,NL] = "Verwÿderen";

			t[14,DE] = "Ver-/Entschlüsseln";  // dto.
			t[14,EN] = "Encrypt/Decrypt";
			t[14,FR] = "Encryptage/décryptage";
			t[14,IT] = "Cifrato/Non cifrato";
			t[14,NL] = "Encryptie/geen encryptie";

			t[13,DE] = "Verschlüsselung";  // dto.
			t[13,EN] = "Encryption";
			t[13,FR] = "Encryptage";
			t[13,IT] = "Codificazione";
			t[13,NL] = "Encryptie";

			t[12,DE] = "Hinzufügen...";  // dto.
			t[12,EN] = "Add...";
			t[12,FR] = "Insérer...";
			t[12,IT] = "Inserisci...";
			t[12,NL] = "Invoegen...";

			t[11,DE] = "Icons";  // dto.
			t[11,EN] = "Icons";
			t[11,FR] = "Icône";
			t[11,IT] = "Icone"; 
			t[11,NL] = "Icoontje"; 

			t[10,DE] = "Löschen";  // dto.
			t[10,EN] = "Delete";
			t[10,FR] = "Supprimer";
			t[10,IT] = "Rimuovi";
			t[10,NL] = "Verwÿderen";

			t[9,DE] = "Bearbeiten...";  // dto. (Notiz-Untermenü)
			t[9,EN] = "New/View/Edit...";
			t[9,FR] = "Éditer/Regarder...";
			t[9,IT] = "Modifica...";
			t[9,NL] = "Bewerken...";

			t[8,DE] = "Notiz";  // dto.
			t[8,EN] = "Note";
			t[8,FR] = "Note";
			t[8,IT] = "Nota"; 
			t[8,NL] = "Aantekenen"; 

			t[7,DE] = "v unterhalb";  // dto.
			t[7,EN] = "v below";
			t[7,FR] = "v après";
			t[7,IT] = "v sotto";
			t[7,NL] = "v ernaar";

			t[6,DE] = "< davor";  // dto.
			t[6,EN] = "< before";
			t[6,FR] = "< dessus";
			t[6,IT] = "< davanti";
			t[6,NL] = "< bovenl.";

			t[5,DE] = "> danach";  // dto.
			t[5,EN] = "> behind";
			t[5,FR] = "> dessous";
			t[5,IT] = "> dopo";
			t[5,NL] = "> onderl.";

			t[4,DE] = "^ oberhalb";  // dto.
			t[4,EN] = "^ above";
			t[4,FR] = "^ avant";
			t[4,IT] = "^ sopra";
			t[4,NL] = "^ ervoor";

			t[3,DE] = "Neuer Knoten";  // dto.
			t[3,EN] = "New node";
			t[3,FR] = "Nouveau nœud";
			t[3,IT] = "Nuovo nodo";
			t[3,NL] = "Nieuwe knoop";

			t[2,DE] = "Bearbeiten...";  // dto.
			t[2,EN] = "Edit...";
			t[2,FR] = "Éditer...";
			t[2,IT] = "Modifica...";
			t[2,NL] = "Bewerken...";

			t[1,DE] = "Abbrechen";
			t[1,EN] = "Cancel";
			t[1,FR] = "Annuler";
			t[1,IT] = "Annulla";
			t[1,NL] = "Annuleer";

			t[0,DE] = "Sie müssen FYMgraphics neu starten damit die Änderungen übernommen werden.";
			t[0,EN] = "You need to restart FYMgraphics for the changes to take effect.";
			t[0,FR] = "Il faut redémarrer FYMgraphics pour afficher les nouvelles préférences.";
			t[0,IT] = "Per visualizzare gli effeti dei cambiamenti, devi riavviare FYMgraphics.";
			t[0,NL] = "Om de gewÿzigde instellingen te laten werken, FYMgraphics opnieuw starten.";
		}
	}
}
