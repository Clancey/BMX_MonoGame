using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Diagnostics;

namespace Svg
{
    /// <summary>
    /// The class that all SVG elements should derive from when they are to be rendered.
    /// </summary>
    public abstract partial class SvgVisualElement : SvgElement
    {
        private bool _dirty;
        private bool _requiresSmoothRendering;
    
//        public abstract RectangleF Bounds { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this element's <see cref="Path"/> is dirty.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the path is dirty; otherwise, <c>false</c>.
        /// </value>
        protected virtual bool IsPathDirty
        {
            get { return this._dirty; }
            set { this._dirty = value; }
        }

        /// <summary>
        /// Gets the associated <see cref="SvgClipPath"/> if one has been specified.
        /// </summary>
        [SvgAttribute("clip-path")]
        public virtual Uri ClipPath
        {
            get { return this.Attributes.GetAttribute<Uri>("clip-path"); }
            set { this.Attributes["clip-path"] = value; }
        }


        /// <summary>
        /// Gets or sets a value to determine if anti-aliasing should occur when the element is being rendered.
        /// </summary>
        protected virtual bool RequiresSmoothRendering
        {
            get { return this._requiresSmoothRendering; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgGraphicsElement"/> class.
        /// </summary>
        public SvgVisualElement()
        {
            this._dirty = true;
            this._requiresSmoothRendering = false;
        }


    }
}