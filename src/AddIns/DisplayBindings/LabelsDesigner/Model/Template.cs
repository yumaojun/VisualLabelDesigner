using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// label模板
	/// </summary>
	public class Template
	{
		public string Brand { get; private set; }
		public string Part { get; private set; }
		public string Description { get; private set; }

		public string PaperId { get; private set; }
		public Distance PageWidth { get; private set; }
		public Distance PageHeight { get; private set; }
		public Distance RollWidth { get; private set; }
		public bool IsSizeIso { get; private set; }
		public bool IsSizeUs { get; private set; }
		public bool IsRoll { get; private set; }
		public bool IsSizeOther
		{
			get
			{
				return !IsSizeIso && !IsSizeUs;
			}
		}

		public bool IsUserDefined { get; private set; }

		public string EquivPart { get; set; }
		public string Name { get; private set; } = string.Empty;

		public string ProductUrl { get; set; }
		public List<string> CategoryIds { get; private set; }

		public List<Frame> Frames { get; private set; }

		//private string _brand;
		//private string _part;
		//private string _description;

		//private string _paperId;
		//private Distance _pageWidth;
		//private Distance _pageHeight;
		//private Distance _rollWidth;

		//private bool _isSizeIso;
		//private bool _isSizeUs;
		//private bool _isRoll;

		//private bool _isUserDefined;

		//private string _equivPart;
		//private string _name;

		//private string _productUrl;
		//private List<string> _categoryIds;

		//private List<Frame> _frames;

		public Template()
		{
			CategoryIds = new List<string>();
			Frames = new List<Frame>();
		}

		public Template(string brand, string part, string description, string paperId, Distance pageWidth, Distance pageHeight, Distance rollWidth, bool isUserDefined = false) : this()
		{
			Brand = brand;
			Part = part;
			Description = description;
			PaperId = paperId;
			PageWidth = pageWidth;
			PageHeight = pageHeight;
			RollWidth = rollWidth;
			IsUserDefined = isUserDefined;
			Name = brand + " " + part;

			if (Db.IsPaperIdKnown(paperId))
			{
				var paper = Db.LookupPaperFromId(paperId);
				IsSizeIso = paper.IsSizeIso();
				IsSizeUs = paper.IsSizeUs();
			}

			IsRoll = paperId == "roll";
		}

		public Template(Template other) : this()
		{
			Brand = other.Brand;
			Part = other.Part;
			Description = other.Description;
			PaperId = other.PaperId;
			PageWidth = other.PageWidth;
			PageHeight = other.PageHeight;
			RollWidth = other.RollWidth;
			EquivPart = other.EquivPart;
			ProductUrl = other.ProductUrl;
			IsUserDefined = other.IsUserDefined;
			Name = other.Name;

			foreach (var frame in other.Frames)
			{
				AddFrame(frame.Clone());
			}

			foreach (string categoryId in other.CategoryIds)
			{
				AddCategory(categoryId);
			}
		}

		//string brand() const;
		//string part() const;
		//string description() const;

		//string paperId() const;
		//Distance pageWidth() const;
		//Distance pageHeight() const;
		//Distance rollWidth() const;
		//bool isSizeIso() const;
		//bool isSizeUs() const;
		//bool isSizeOther() const;
		//bool isRoll() const;

		//bool isUserDefined() const;

		//string equivPart() const;
		//void setEquivPart( const QString& value );

		//public string productUrl() const;
		//public void setProductUrl( const QString& value );

		//public string name() const;

		public void AddFrame(Frame frame)
		{
			Frames.Add(frame);
		}

		public void AddCategory(string categoryId)
		{
			CategoryIds.Add(categoryId);
		}

		public static Template FromEquiv(string brand, string part, string equivPart)
		{
			Template other = Db.LookupTemplateFromBrandPart(brand, equivPart);
			if (other != null)
			{
				Template tmplate = new Template(other);
				tmplate.Part = part;
				tmplate.EquivPart = equivPart;
				tmplate.Name = "";
				tmplate.Name += brand + " " + part;
				return tmplate;
			}
			else
			{
				return null;
			}
		}

		public bool IsSimilarTo(Template other)
		{
			if (PaperId != other.PaperId || PageWidth != other.PageWidth || PageHeight != other.PageHeight)
			{
				return false;
			}

			Frame frame1 = Frames.First();
			Frame frame2 = other.Frames.First();
			if (!frame1.IsSimilarTo(frame2))
			{
				return false;
			}

			foreach (Layout layout1 in frame1.Layouts())
			{
				bool matchFound = false;
				foreach (Layout layout2 in frame2.Layouts())
				{
					if (layout1.IsSimilarTo(layout2))
					{
						matchFound = true;
						break;
					}
				}
				if (!matchFound)
				{
					return false;
				}
			}

			return true;
		}

		public override bool Equals(object other)
		{
			var _other = other as Template;
			return Brand == _other?.Brand && Part == _other?.Part;
		}

		public override int GetHashCode()
		{
			var hashCode = Brand.GetHashCode();
			var partHashCode = Part.GetHashCode();
			if (hashCode != partHashCode)
			{
				hashCode ^= partHashCode;
			}
			return hashCode;
		}

		public static bool operator ==(Template a, Template other) => a?.Brand == other?.Brand && a?.Part == other?.Part;

		public static bool operator !=(Template a, Template other) => a?.Brand != other?.Brand || a?.Part != other?.Part;

	}
}
