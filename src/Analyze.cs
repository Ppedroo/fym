/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Peter
 * Datum: 28.02.2007
 * Zeit: 18:48
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */

using System;

namespace MindMap
{
	/// <summary>
	/// Description of Analyze.
	/// </summary>
	public class Analyze
	{
		private String str;
		private int indent;

		public Analyze()
		{
			str = "";
			indent = -1;
		}
		
		public void Line(String input)
		{
			int i;
			String hStr;
			
			if (input.StartsWith("<node"))
			{
				indent++;
				for (int j = 0; j < indent; j++)
				{
					if (j == (indent-1))
						str += "|-";
					else if (j == 0)
						str += "| ";
					else
						str += "  ";
				}
				i = input.IndexOf(" TEXT=\"") + 7;
				hStr = input.Substring(i);
				i = hStr.IndexOf("\"");
				hStr = hStr.Substring(0,i);
			    str += hStr + "\r\n";
			    if (input.EndsWith("/>"))
			        indent--;
			}
			else if (input.StartsWith("</node"))
				indent--;
/*
			str = "<map version=\"0.8.0\">\r\n";
			str += "<!-- To view this file, download free mind mapping software FreeMind from http://freemind.sourceforge.net -->\r\n";
			str += "<node CREATED=\"1172674542393\" ID=\"Freemind_Link_1205879482\" MODIFIED=\"1172674542393\" TEXT=\"Neue Mindmap\">\r\n";
			str += "<node CREATED=\"1172674546222\" ID=\"_\" MODIFIED=\"1172674557959\" POSITION=\"right\" TEXT=\"1.1\"/>\r\n";
			str += "<node CREATED=\"1172674558443\" ID=\"Freemind_Link_186604898\" MODIFIED=\"1172674560881\" POSITION=\"right\" TEXT=\"1.2\">\r\n";
			str += "<node CREATED=\"1172674561881\" ID=\"Freemind_Link_202033165\" MODIFIED=\"1172674564570\" TEXT=\"1.2.1\"/>\r\n";
			str += "<node CREATED=\"1172674566195\" ID=\"Freemind_Link_423063605\" MODIFIED=\"1172674570946\" TEXT=\"1.2.2\"/>\r\n";
			str += "</node>\r\n";
			str += "</node>\r\n";
			str += "</map>\r\n";
*/
		}
		
		public String GetResult()
		{
			return(str);
		}
	}
}
