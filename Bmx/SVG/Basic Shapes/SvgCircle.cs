using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;
using System.ComponentModel;

namespace Svg
{
    /// <summary>
    /// An SVG element to render circles to the document.
    /// </summary>
    [SvgElement("circle")]
    public class SvgCircle : SvgVisualElement
    {
       // private GraphicsPath _path;

        /// <summary>
        /// Gets the center point of the circle.
        /// </summary>
        /// <value>The center.</value>
        public SvgPoint Center
        {
            get { return new SvgPoint(this.CenterX, this.CenterY); }
        }

        /// <summary>
        /// Gets or sets the center X co-ordinate.
        /// </summary>
        /// <value>The center X.</value>
        [SvgAttribute("cx")]
        public SvgUnit CenterX
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("cx"); }
            set
            {
                if (this.Attributes.GetAttribute<SvgUnit>("cx") != value)
                {
                    this.Attributes["cx"] = value;
                    this.IsPathDirty = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the center Y co-ordinate.
        /// </summary>
        /// <value>The center Y.</value>
        [SvgAttribute("cy")]
        public SvgUnit CenterY
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("cy"); }
            set
            {
                if (this.Attributes.GetAttribute<SvgUnit>("cy") != value)
                {
                    this.Attributes["cy"] = value;
                    this.IsPathDirty = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the radius of the circle.
        /// </summary>
        /// <value>The radius.</value>
        [SvgAttribute("r")]
        public SvgUnit Radius
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("r"); }
            set
            {
                if (this.Attributes.GetAttribute<SvgUnit>("r") != value)
                {
                    this.Attributes["r"] = value;
                    this.IsPathDirty = true;
                }
            }
        }

        /// <summary>
        /// Gets the bounds of the circle.
        /// </summary>
        /// <value>The rectangular bounds of the circle.</value>
        public override RectangleF Bounds
        {
			get { return null;}// this.Path.GetBounds(); }
        }

        /// <summary>
        /// Gets a value indicating whether the circle requires anti-aliasing when being rendered.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the circle requires anti-aliasing; otherwise, <c>false</c>.
        /// </value>
        protected override bool RequiresSmoothRendering
        {
            get { return true; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SvgCircle"/> class.
        /// </summary>
        public SvgCircle()
        {
            
        }
    }
}