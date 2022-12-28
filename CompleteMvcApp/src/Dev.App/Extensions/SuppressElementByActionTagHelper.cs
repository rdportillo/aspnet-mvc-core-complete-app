using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dev.App.Extensions
{
    [HtmlTargetElement("*", Attributes = "suppress-by-action")]
    public class SuppressElementByActionTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        [HtmlAttributeName("suppress-by-action")]
        public string ActionName { get; set; }

        public SuppressElementByActionTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (output == null)
                throw new ArgumentNullException(nameof(output));

            var action = _contextAccessor.HttpContext.GetRouteData().Values["action"].ToString();

            if (ActionName.Contains(action)) return;

            output.SuppressOutput();
        }
    }
}
