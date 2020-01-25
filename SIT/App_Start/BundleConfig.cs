using System.Web;
using System.Web.Optimization;

namespace SIT
{
	public class BundleConfig
	{
		// Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));


			bundles.Add(new ScriptBundle("~/bundles/myScripts")
				.Include("~/Scripts/my.js"));


			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap*"));


			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap*",
					  "~/Content/site.css"));
		}
	}
}
