using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;
using System.ComponentModel;

namespace Svg
{
    /// <summary>
    /// Represents and SVG ellipse element.
    /// </summary>
    [SvgElement("ellipse")]
    public class SvgEllipse : SvgVisualElement
    {
        private SvgUnit _radiusX;
        private SvgUnit _radiusY;
        private SvgUnit _centerX;
        private SvgUnit _centerY;
        //private GraphicsPath _path;

        [SvgAttribute("cx")]
        public virtual SvgUnit CenterX
        {
            get { return this._centerX; }
            set
            {
                this._centerX = value;
                this.IsPathDirty = true;
            }
        }

        [SvgAttribute("cy")]
        public virtual SvgUnit CenterY
        {
            get { return this._centerY; }
            set
            {
                this._centerY = value;
                this.IsPathDirty = true;
            }
        }

        [SvgAttribute("rx")]
        public virtual SvgUnit RadiusX
        {
            get { return this._radiusX; }
            set { this._radiusX = value; this.IsPathDirty = true; }
        }

        [SvgAttribute("ry")]
        public virtual SvgUnit RadiusY
        {
            get { return this._radiusY; }
            set { this._radiusY = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets a value to determine if anti-aliasing should occur when the element is being rendered.
        /// </summary>
        /// <value></value>
        protected override bool RequiresSmoothRendering
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override RectangleF Bounds
        {
            get { return this.Path.GetBounds(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgEllipse"/> class.
        /// </summary>
        public SvgEllipse()
        {
        }
    }
}