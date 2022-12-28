using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dev.App.Extensions
{
    [HtmlTargetElement("*", Attributes = "suppress-by-claim-name")]
    [HtmlTargetElement("*", Attributes = "suppress-by-claim-value")]
    public class SuppressElementByClaimTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        [HtmlAttributeName("suppress-by-claim-name")]
        public string IdentityClaimName { get; set; }

        [HtmlAttributeName("suppress-by-claim-value")]
        public string IdentityClaimValue { get; set; }

        public SuppressElementByClaimTagHelper(IHttpContextAccessor contextAcessor)
        {
            _contextAccessor = contextAcessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(context == null)
                throw new ArgumentNullException(nameof(context));

            if (output == null)
                throw new ArgumentNullException(nameof(output));

            var hasAccess = CustomAuthorization.ValidateUserClaim(_contextAccessor.HttpContext, IdentityClaimName, IdentityClaimValue);

            if (hasAccess) return;

            output.SuppressOutput();
        }
    }
}
