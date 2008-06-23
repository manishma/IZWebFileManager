using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace IZ.WebFileManager
{
	class HiddenItemStyle : Style
	{
		protected override void FillStyleAttributes (System.Web.UI.CssStyleCollection attributes, System.Web.UI.IUrlResolutionService urlResolver) {
			base.FillStyleAttributes (attributes, urlResolver);
			attributes.Add ("opacity", "0.40");
			attributes.Add ("filter", "alpha(opacity=40)");
			attributes.Add ("-moz-opacity", ".40");
		}
	}
}
