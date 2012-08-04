using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Svg
{
    public abstract partial class SvgVisualElement
    {
        private static float FixOpacityValue(float value)
        {
            const float max = 1.0f;
            const float min = 0.0f;
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Gets or sets a value to determine whether the element will be rendered.
        /// </summary>
        [SvgAttribute("visibility")]
        public virtual bool Visible
        {
            get { return (this.Attributes["Visible"] == null) ? true : (bool)this.Attributes["Visible"]; }
            set { this.Attributes["Visible"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of this element's <see cref="Fill"/>.
        /// </summary>
        [SvgAttribute("fill-opacity")]
        public virtual float FillOpacity
        {
            get { return (this.Attributes["FillOpacity"] == null) ? this.Opacity : (float)this.Attributes["FillOpacity"]; }
            set { this.Attributes["FillOpacity"] = FixOpacityValue(value); }
        }

     
        [SvgAttribute("stroke-miterlimit")]
        public virtual float StrokeMiterLimit
        {
            get { return (this.Attributes["StrokeMiterLimit"] == null) ? 4.0f : (float)this.Attributes["StrokeMiterLimit"]; }
            set { this.Attributes["StrokeMiterLimit"] = value; }
        }


        /// <summary>
        /// Gets or sets the opacity of the stroke, if the <see cref="Stroke"/> property has been specified. 1.0 is fully opaque; 0.0 is transparent.
        /// </summary>
        [SvgAttribute("stroke-opacity")]
        public virtual float StrokeOpacity
        {
            get { return (this.Attributes["StrokeOpacity"] == null) ? this.Opacity : (float)this.Attributes["StrokeOpacity"]; }
            set { this.Attributes["StrokeOpacity"] = FixOpacityValue(value); }
        }

        /// <summary>
        /// Gets or sets the opacity of the element. 1.0 is fully opaque; 0.0 is transparent.
        /// </summary>
        [SvgAttribute("opacity")]
        public virtual float Opacity
        {
            get { return (this.Attributes["Opacity"] == null) ? 1.0f : (float)this.Attributes["Opacity"]; }
            set { this.Attributes["Opacity"] = FixOpacityValue(value); }
        }
    }
}