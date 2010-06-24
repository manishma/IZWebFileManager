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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var script =
@"

function IZWebFileManager_ShowElement(id) {
    var el = WebForm_GetElementById(id);
    el.style.display = 'block';
}

function IZWebFileManager_HideElement(id) {
    var el = WebForm_GetElementById(id);
    el.style.display = 'none';
}

";
            Page.ClientScript.RegisterClientScriptBlock(typeof(ToolbarMenu), "show_hide_submenu", script, true);
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
                var hasChildren = item.ChildItems.Count > 0;
                var itemControl = ((MenuItemTemplateContainer) _container.GetValue(item, null));
                var submenuClientId = itemControl.ClientID + "_s";

                if(hasChildren)
                {
                    writer.AddAttribute("onmouseover", "IZWebFileManager_ShowElement('" + submenuClientId + "')");
                    writer.AddAttribute("onmouseout", "IZWebFileManager_HideElement('" + submenuClientId + "')");
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Td);


                itemControl.RenderControl(writer);

                if(hasChildren)
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "absolute");
                    Control.DynamicMenuStyle.AddAttributesToRender(writer);
                    writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                    writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, submenuClientId);
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);

                    for (int i = 0; i < item.ChildItems.Count; i++)
                    {
                        Control.DynamicMenuItemStyle.AddAttributesToRender(writer);
                        writer.AddAttribute("onmouseover", "WebForm_AppendToClassName(this, '" + Control.DynamicHoverStyle.RegisteredCssClass + "')");
                        writer.AddAttribute("onmouseout", "WebForm_RemoveClassName(this, '" + Control.DynamicHoverStyle.RegisteredCssClass + "')");
                        writer.AddAttribute("onclick", "IZWebFileManager_HideElement('" + submenuClientId + "')");
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        var childItem = item.ChildItems[i];
                        ((MenuItemTemplateContainer)_container.GetValue(childItem, null)).RenderControl(writer);
                        writer.RenderEndTag();
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
