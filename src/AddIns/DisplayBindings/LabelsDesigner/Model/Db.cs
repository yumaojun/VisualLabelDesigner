using ICSharpCode.SharpDevelop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 读取模板数据类
	/// </summary>
	public class Db
	{
		public static List<Paper> Papers { get; private set; } = new List<Paper>();
		public static List<string> PaperIds { get; private set; } = new List<string>();
		public static List<string> PaperNames { get; private set; } = new List<string>();

		public static List<Category> Categories { get; private set; } = new List<Category>();
		public static List<string> CategoryIds { get; private set; } = new List<string>();
		public static List<string> CategoryNames { get; private set; } = new List<string>();

		public static List<Vendor> Vendors { get; private set; } = new List<Vendor>();
		public static List<string> VendorNames { get; private set; } = new List<string>();
		public static List<Template> Templates { get; private set; } = new List<Template>();

		private Db()
		{
			LoadPapers();
			LoadCategories();
			LoadVendors();
			LoadTemplates();
		}

		public static void Init()
		{
			Instance();
		}

		public static Db Instance()
		{
			var db = new Db();
			return db;
		}

		#region 公开方法

		public static void RegisterPaper(Paper paper)
		{
			if (!IsPaperIdKnown(paper.Id))
			{
				Papers.Add(paper);
				PaperIds.Add(paper.Id);
				PaperNames.Add(paper.Name);
			}
		}

		public static Paper LookupPaperFromName(string name)
		{
			return Papers.FirstOrDefault(x => string.IsNullOrEmpty(name) || x.Name == name);
		}

		public static Paper LookupPaperFromId(string id)
		{
			return Papers.FirstOrDefault(x => string.IsNullOrEmpty(id) || x.Id == id);
		}

		public static string LookupPaperIdFromName(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				if (name == "Other") //tr("Other")
				{
					return "other";
				}
				else if (name == "Roll") //tr("Roll")
				{
					return "roll";
				}

				Paper paper = LookupPaperFromName(name);
				if (paper != null)
				{
					return paper.Id;
				}
			}

			return string.Empty;
		}

		public static string LookupPaperNameFromId(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				if (id == "roll")
				{
					return "Roll"; //tr("Roll")
				}
				else if (id == "Other")
				{
					return "Other"; //tr("Other")
				}

				Paper paper = LookupPaperFromId(id);
				if (paper != null)
				{
					return paper.Name;
				}
			}

			return string.Empty;
		}

		public static bool IsPaperIdKnown(string id)
		{
			return Papers.Any(x => x.Id == id);
		}

		public static void RegisterCategory(Category category)
		{
			if (!IsCategoryIdKnown(category.Id))
			{
				Categories.Add(category);
				CategoryIds.Add(category.Id);
				CategoryNames.Add(category.Name);
			}
		}

		public static Category LookupCategoryFromName(string name)
		{
			return Categories.FirstOrDefault(x => string.IsNullOrEmpty(name) || x.Name == name);
		}

		public static Category LookupCategoryFromId(string id)
		{
			return Categories.FirstOrDefault(x => string.IsNullOrEmpty(id) || x.Id == id);
		}

		public static string LookupCategoryIdFromName(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				Category category = LookupCategoryFromName(name);
				if (category != null)
				{
					return category.Id;
				}
			}
			return string.Empty;
		}

		public static string LookupCategoryNameFromId(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				Category category = LookupCategoryFromId(id);
				if (category != null)
				{
					return category.Name;
				}
			}
			return string.Empty;
		}

		public static bool IsCategoryIdKnown(string id)
		{
			return Categories.Any(x => x.Id == id);
		}

		public static void RegisterVendor(Vendor vendor)
		{
			if (!IsVendorNameKnown(vendor.Name))
			{
				Vendors.Add(vendor);
				VendorNames.Add(vendor.Name);
			}
		}

		public static Vendor LookupVendorFromName(string name)
		{
			return Vendors.FirstOrDefault(x => string.IsNullOrEmpty(name) || x.Name == name);
		}

		public static string LookupVendorUrlFromName(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				Vendor vendor = LookupVendorFromName(name);
				if (vendor != null)
				{
					return vendor.Url;
				}
			}
			return string.Empty;
		}

		public static bool IsVendorNameKnown(string name)
		{
			return Vendors.Any(x => x.Name == name);
		}

		public static void RegisterTemplate(Template tmplate)
		{
			if (!IsTemplateKnown(tmplate.Brand, tmplate.Part))
			{
				Templates.Add(tmplate);
			}
		}

		public static Template LookupTemplateFromName(string name)
		{
			return Templates.FirstOrDefault(x => string.IsNullOrEmpty(name) || x.Name == name);
		}

		public static Template LookupTemplateFromBrandPart(string brand, string part)
		{
			return Templates.FirstOrDefault(x => string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(part) || (x.Brand == brand && x.Part == part));
		}

		public static bool IsTemplateKnown(string brand, string part)
		{
			return Templates.Any(x => x.Brand == brand && x.Part == part);
		}

		static bool IsSystemTemplateKnown(string brand, string part)
		{
			return Templates.Any(x => x.Brand == brand && x.Part == part && !x.IsUserDefined);
		}

		static List<string> GetNameListOfSimilarTemplates(string name)
		{
			List<string> list = new List<string>();

			Template tmplate1 = LookupTemplateFromName(name);
			if (tmplate1 == null)
			{
				return list;
			}

			foreach (Template tmplate2 in Templates)
			{
				if (tmplate1.Name != tmplate2.Name)
				{
					if (tmplate1.IsSimilarTo(tmplate2))
					{
						list.Add(tmplate2.Name);
					}
				}
			}

			return list;
		}

		static string UserTemplateFilename(string brand, string part)
		{
			string fileName = brand + "_" + part + ".template";
			string filePath = FileUtil.UserTemplatesDir();
			return Path.Combine(filePath, fileName);
		}

		static void RegisterUserTemplate(Template tmplate)
		{
			string filename = UserTemplateFilename(tmplate.Brand, tmplate.Part);

			if (new XmlTemplateCreator().WriteTemplate(tmplate, filename))
			{
				RegisterTemplate(tmplate);
				//Settings::addToRecentTemplateList(tmplate.Name);
			}
		}

		static void DeleteUserTemplateByBrandPart(string brand, string part)
		{
			Template tmplate = null;
			foreach (Template candidate in Templates)
			{
				if (candidate.IsUserDefined && candidate.Brand == brand && candidate.Part == part)
				{
					tmplate = candidate;
					break;
				}
			}

			if (tmplate != null)
			{
				Templates.Remove(tmplate);
				string filename = UserTemplateFilename(brand, part);
				File.Delete(filename);
			}
		}

		#endregion

		#region 内部方法

		void LoadPapers()
		{
			var path = FileUtil.SystemTemplatesDir();
			LoadPapersFromDir(path);
		}

		void LoadPapersFromDir(string directory)
		{
			string[] files = System.IO.Directory.GetFiles(directory, "paper-sizes.xml");
			XmlPaperParser parser = new XmlPaperParser();

			foreach (string fileName in files)
			{
				parser.ReadFile(fileName);
			}
		}

		void LoadCategories()
		{
			LoadCategoriesFromDir(FileUtil.SystemTemplatesDir());
		}

		void LoadCategoriesFromDir(string directory)
		{
			string[] files = System.IO.Directory.GetFiles(directory, "categories.xml");
			XmlCategoryParser parser = new XmlCategoryParser();

			foreach (string fileName in files)
			{
				parser.ReadFile(fileName);
			}
		}

		void LoadVendors()
		{
			LoadVendorsFromDir(FileUtil.SystemTemplatesDir());
		}

		void LoadVendorsFromDir(string directory)
		{
			string[] files = System.IO.Directory.GetFiles(directory, "vendors.xml");
			XmlVendorParser parser = new XmlVendorParser();

			foreach (string fileName in files)
			{
				parser.ReadFile(fileName);
			}
		}

		/// <summary>
		/// 加载模板
		/// </summary>
		void LoadTemplates()
		{
			LoadTemplatesFromDir(FileUtil.SystemTemplatesDir(), false);
		}

		/// <summary>
		/// 从目录加载模板
		/// </summary>
		/// <param name="directory"></param>
		/// <param name="isUserDefined"></param>
		void LoadTemplatesFromDir(string directory, bool isUserDefined)
		{
			string[] files = System.IO.Directory.GetFiles(directory, "*-templates.xml"); // ".template"
			XmlTemplateParser parser = new XmlTemplateParser();

			foreach (string fileName in files)
			{
				parser.ReadFile(fileName, isUserDefined);
			}
		}

		#endregion

	}
}
