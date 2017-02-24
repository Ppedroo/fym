/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Peter
 * Datum: 09.02.2009
 * Zeit: 18:56
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */

#if GRAPHICAL

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FreeYourMind
{
	/// <summary>
	/// Description of MyTreeNode.
	/// </summary>
	public class MyTreeNode
	{
		private Object           tag;
		private MyTreeNode       parent;
		private MyTreeNodeCollection nodes;
		private string           text;
		private Color            backColor;
		private Color            foreColor;
		public  List<int>        iconIndexList;
//		private int              imageIndex;
//		private int              selectedImageIndex;
		private int              index;
		private int              level;
		private Rectangle        textBox;
		private int              nextX;
		private Point            point1;
		private Point            point2;
		public  int              nextYInclSubNodes;
		public  bool             expanded;
		private Rectangle        expander;
		public  long             created;
		public  long             modified;

		public MyTreeNode(string str, Color fc, Color bc)
		{
			tag   = new Object();
			nodes = new MyTreeNodeCollection();
			text  = str;

			if (fc == Color.Transparent)  // Standard
				foreColor = Color.Black;
			else
				foreColor = fc;

			if (bc == Color.Transparent)  // Standard
//				backColor = Color.Beige;
//				backColor = Color.FromArgb(255, 255, 225);  // das Tooltip-Gelb ist schöner als beige
				backColor = Color.White;
			else
				backColor = bc;

			iconIndexList = new List<int>();
		}

		public MyTreeNode AddNode(string str, Rectangle boxParam, long createdParam, long modifiedParam, Color fc, Color bc)
		{
			nodes.Add(new MyTreeNode(str, fc, bc));

			MyTreeNode newNode = nodes[nodes.Count-1];
			newNode.Parent = this;
			newNode.Index = nodes.Count-1;
			newNode.Level = level+1;
			newNode.TextBox = boxParam;
			newNode.Expander = new Rectangle(0, 0, Const.xSpacing-Const.chamfer, newNode.TextBox.Height+Const.ySpacing/2);
			newNode.created = createdParam;
			newNode.modified = modifiedParam;

			return(newNode);
		}

		public void AddIcon(int iconIndex)
		{
			iconIndexList.Add(iconIndex);
		}

		public int Point1X
		{
			get { return point1.X; }
			set { point1.X = value; }
		}

		public Point Point1
		{
			get { return point1; }
			set { point1 = value; }
		}

		public Point Point2
		{
			get { return point2; }
			set { point2 = value; }
		}

		public int NextX
		{
			get { return nextX; }
			set { nextX = value; }
		}

		public int ExpanderX
		{
			get { return expander.X; }
			set { expander.X = value; }
		}

		public int ExpanderY
		{
			get { return expander.Y; }
			set { expander.Y = value; }
		}

		public Rectangle Expander
		{
			get { return expander; }
			set { expander = value; }
		}

		public int TextBoxX
		{
			get { return textBox.X; }
			set { textBox.X = value; }
		}

		public int TextBoxY
		{
			get { return textBox.Y; }
			set { textBox.Y = value; }
		}

		public Rectangle TextBox
		{
			get { return textBox; }
			set { textBox = value; }
		}

		public int Level
		{
			get { return level; }
			set { level = value; }
		}
/*
		public int SelectedImageIndex
		{
			get { return selectedImageIndex; }
			set { selectedImageIndex = value; }
		}

		public int ImageIndex
		{
			get { return imageIndex; }
			set { imageIndex = value; }
		}
*/
		public Color ForeColor
		{
			get { return foreColor; }
			set { foreColor = value; }
		}

		public Color BackColor
		{
			get { return backColor; }
			set { backColor = value; }
		}

		public void Expand()
		{
			expanded = true;
		}

		public void Collapse()
		{
			expanded = false;
		}

		public string Text
		{
			get { return text; }
			set { text = value; }
		}

		public int Index
		{
			get { return(index); }
			set { index = value; }
		}

		public MyTreeNodeCollection Nodes
		{
			get { return nodes; }
			set { nodes = value; }
		}

		public int GetNodeCount(bool includeAllGenerations)
		{
			if (includeAllGenerations)
				return(-1);  // TODO: rekursiv alle Generationen
			else
				return(Nodes.Count);
		}

		public MyTreeNode Parent
		{
			get { return parent; }
			set { parent = value; }
		}

		public Object Tag
		{
			get { return tag; }
			set { tag = value; }
		}
	}
}
#endif
