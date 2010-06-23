using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.Adapters;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Adapters;

namespace IZ.WebFileManager
{
    internal class ToolbarMenu : Menu
    {
        private readonly ToolbarMenuAdapter toolbarMenuAdapter;

        public ToolbarMenu()
        {
            toolbarMenuAdapter = new ToolbarMenuAdapter(this);
        }

        protected override ControlAdapter ResolveAdapter()
        {
            return toolbarMenuAdapter;
        }

        internal class ToolbarMenuAdapter : MenuAdapter
        {
            private static readonly FieldInfo _control =
                typeof (ToolbarMenuAdapter).GetField("_control", BindingFlags.NonPublic | BindingFlags.Instance);

            private static readonly PropertyInfo _container =
                typeof (MenuItem).GetProperty("Container", BindingFlags.NonPublic | BindingFlags.Instance);

            public ToolbarMenuAdapter(ToolbarMenu toolbarMenu)
            {
                _control.SetValue(this, toolbarMenu);
            }

            protected override void RenderBeginTag(HtmlTextWriter writer)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            }
            protected override void RenderEndTag(HtmlTextWriter writer)
            {
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
            protected override void RenderItem(HtmlTextWriter writer, MenuItem item, int position)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                ((MenuItemTemplateContainer) _container.GetValue(item, null)).RenderControl(writer);

                if(item.ChildItems.Count> 0)
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "absolute");
                    Control.DynamicMenuStyle.AddAttributesToRender(writer);
                    writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                    writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);

                    for (int i = 0; i < item.ChildItems.Count; i++)
                    {
                        var childItem = item.ChildItems[i];
                        ((MenuItemTemplateContainer)_container.GetValue(childItem, null)).RenderControl(writer);
                    }
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }
                
                writer.RenderEndTag();
            }

        }
    }
}
