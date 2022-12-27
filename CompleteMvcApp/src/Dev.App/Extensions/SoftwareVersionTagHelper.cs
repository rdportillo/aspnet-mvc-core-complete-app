using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dev.App.Extensions
{
    public class SoftwareVersionTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var softwareVersion = "1.0.0";
            output.Content.SetContent(softwareVersion);
        }
    }
}
