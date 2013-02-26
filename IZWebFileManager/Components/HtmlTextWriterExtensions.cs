//Copyright (c) 2009 Patrik Hägne

//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 

namespace Legend.Web
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;

    /// <summary>
    /// Provides extension methods for the HtmlTextWriterClass.
    /// </summary>
    internal static class HtmlTextWriterExtensions
    {

        /// <summary>
        /// Renders a start tag, the tag is closed by a call to the EndTag-method.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="tag">The tag to render the start tag of.</param>
        /// <param name="appender">A delegate that takes in an HtmlAttributeManager for appending
        /// attributes to the start tag.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Tag(this HtmlTextWriter writer, HtmlTextWriterTag tag, Func<HtmlAttributeManager, HtmlAttributeManager> appender = null)
        {
            if (appender != null)
            {
                var manager = new HtmlAttributeManager(writer);
                appender(manager);
            }

            writer.RenderBeginTag(tag);

            return writer;
        }

        /// <summary>
        /// Renders a Div start tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="attributes">A delegate that takes in an HtmlAttributeManager for appending
        /// attributes to the start tag.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Div(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes = null)
        {
            return writer.Tag(HtmlTextWriterTag.Div, attributes);
        }

        public static HtmlTextWriter Table(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes = null)
        {
            return writer.Tag(HtmlTextWriterTag.Table, attributes);
        }

        public static HtmlTextWriter Tr(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes = null)
        {
            return writer.Tag(HtmlTextWriterTag.Tr, attributes);
        }
        
        public static HtmlTextWriter Td(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes = null)
        {
            return writer.Tag(HtmlTextWriterTag.Td, attributes);
        }

        public static HtmlTextWriter Input(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes = null)
        {
            return writer.Tag(HtmlTextWriterTag.Input, attributes);
        }

        public static HtmlTextWriter Img(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes = null)
        {
            return writer.Tag(HtmlTextWriterTag.Img, attributes);
        }

        /// <summary>
        /// Renders a Body start tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="attributes">A delegate that takes in an HtmlAttributeManager for appending
        /// attributes to the start tag.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Body(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes = null)
        {
            return writer.Tag(HtmlTextWriterTag.Body, attributes);
        }

        /// <summary>
        /// Renders a Html start tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="attributes">A delegate that takes in an HtmlAttributeManager for appending
        /// attributes to the start tag.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Html(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes = null)
        {
            return writer.Tag(HtmlTextWriterTag.Html, attributes);
        }

        /// <summary>
        /// Renders a Span start tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="attributes">A delegate that takes in an HtmlAttributeManager for appending
        /// attributes to the start tag.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Span(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes = null)
        {
            return writer.Tag(HtmlTextWriterTag.Span, attributes);
        }

        public static HtmlTextWriter A(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes = null)
        {
            return writer.Tag(HtmlTextWriterTag.A, attributes);
        }

        /// <summary>
        /// Renders an anchor tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="url">The url of the hyperlink.</param>
        /// <param name="title">The title of the hyperlink.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Href(this HtmlTextWriter writer, string url, string title)
        {
            if (url == null)
                throw new ArgumentNullException("url");
            if (title == null)
                throw new ArgumentNullException("title");

            return writer.Tag(HtmlTextWriterTag.A, e => e.Attr(HtmlTextWriterAttribute.Href, url));
        }
        

        /// <summary>
        /// Closes the latest started tag.
        /// </summary>
        /// <param name="writer">The writer to render the end tag to.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter EndTag(this HtmlTextWriter writer)
        {
            writer.RenderEndTag();
            return writer;
        }

        /// <summary>
        /// Closes the latest started tag.
        /// </summary>
        /// <param name="writer">The writer to render the end tag to.</param>
        /// <param name="ignored">The tag this call closes, only specified for readability,
        /// this parameter is ignored.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter EndTag(this HtmlTextWriter writer, HtmlTextWriterTag ignored)
        {
            return writer.EndTag();
        }

        /// <summary>
        /// Renders a text literal to the writer.
        /// </summary>
        /// <param name="writer">The writer to render the text to.</param>
        /// <param name="text">The text to render.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Text(this HtmlTextWriter writer, string text)
        {
            return writer.Text(text, false);
        }

        /// <summary>
        /// Renders a text literal to the writer.
        /// </summary>
        /// <param name="writer">The writer to render the text to.</param>
        /// <param name="text">The text to render.</param>
        /// <param name="htmlEncode">If set to true the text will be html-encoded.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Text(this HtmlTextWriter writer, string text, bool htmlEncode)
        {
            if (htmlEncode)
            {
                writer.Write(HttpUtility.HtmlEncode(text));
            }
            else
            {
                writer.Write(text);
            }

            return writer;
        }

        /// <summary>
        /// Renders a text literal to the writer.
        /// </summary>
        /// <param name="writer">The writer to render the text to.</param>
        /// <param name="value">An object that represents the text to be written.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Text(this HtmlTextWriter writer, object value)
        {
            if (value != null)
            {
                IFormattable formattable = value as IFormattable;
                if (formattable != null)
                {
                    writer.Text(formattable.ToString(null, null));
                }
                else
                {
                        writer.Text(value.ToString());
                }
            }

            return writer;
        }

        /// <summary>
        /// Repeats over the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="collection">The collection to repeat over.</param>
        /// <param name="binder">A function that will be called for each of the elements
        /// in the collection, the first parameter is the item in the collection, the second
        /// parameter the index of the item in the collection, and the third is the writer
        /// to render to.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Bind<T>(this HtmlTextWriter writer,
            IEnumerable<T> collection, Func<T, int, HtmlTextWriter, HtmlTextWriter> binder)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (binder == null)
                throw new ArgumentNullException("binder");

            int index = 0;
            foreach (var item in collection)
            {
                binder(item, index++, writer);
            }

            return writer;
        }

        /// <summary>
        /// Repeats the specified number of times.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="times">The number of times to repeat.</param>
        /// <param name="binder">A function that will be called the specified number of times,
        /// the first parameter is the number of the call (starting with one), the
        /// second parameter is the writer to render to.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Repeat(this HtmlTextWriter writer, int times, 
            Func<int, HtmlTextWriter, HtmlTextWriter> binder)
        {
            if (binder == null)
                throw new ArgumentNullException("binder");

            if (times < 0) throw new ArgumentOutOfRangeException("times");

            for (var i = 1; i <= times; i++)
            {
                binder(i, writer);
            }

            return writer;
        }

        public static HtmlTextWriter RenderControl(this HtmlTextWriter writer, Control control)
        {
            control.RenderControl(writer);

            return writer;
        }
    }
}
