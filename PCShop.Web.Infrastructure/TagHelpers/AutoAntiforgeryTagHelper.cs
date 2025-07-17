using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PCShop.Web.Infrastructure.TagHelpers
{
    [HtmlTargetElement("form", Attributes = "method", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AutoAntiforgeryTagHelper : TagHelper
    {
        private readonly IHtmlGenerator _htmlGenerator;

        public AutoAntiforgeryTagHelper(IHtmlGenerator htmlGenerator)
        {
            this._htmlGenerator = htmlGenerator;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = default!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Only apply to POST forms
            if (output.Attributes.TryGetAttribute("method", out var methodAttribute) 
                && methodAttribute.Value?.ToString()?.ToLower() == "post")
            {
                var antiforgeryTag = this._htmlGenerator.GenerateAntiforgery(ViewContext);
                output.PostContent.AppendHtml(antiforgeryTag);
            }
        }
    }
}
