#pragma checksum "C:\Users\Paul\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Views\Account\ConfirmPhoneSuccess.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5a4f04af1fcbbae0f21b542f1afba927bd2b526a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_ConfirmPhoneSuccess), @"mvc.1.0.view", @"/Views/Account/ConfirmPhoneSuccess.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Paul\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Views\_ViewImports.cshtml"
using IdentityServer4.Quickstart.UI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\Paul\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Views\Account\ConfirmPhoneSuccess.cshtml"
using static IdentityServerAspNetIdentity.Startup;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a4f04af1fcbbae0f21b542f1afba927bd2b526a", @"/Views/Account/ConfirmPhoneSuccess.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"57b49bb18fbe88f2fb7e20eb130e64338d7f2c37", @"/Views/_ViewImports.cshtml")]
    public class Views_Account_ConfirmPhoneSuccess : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\Paul\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Views\Account\ConfirmPhoneSuccess.cshtml"
  
    ViewData["Title"] = "Le numéro de téléphone est confirmé";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"jumbotron bg-gradient-primary\">\r\n    <div class=\"container\">\r\n        <h1>");
#nullable restore
#line 8 "C:\Users\Paul\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Views\Account\ConfirmPhoneSuccess.cshtml"
       Write(ViewData["Title"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n        <div>\r\n            <p>\r\n                Merci d\'avoir confirmé votre numéro.\r\n            </p>\r\n            <a");
            BeginWriteAttribute("href", " href=\"", 355, "\"", 392, 1);
#nullable restore
#line 13 "C:\Users\Paul\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Views\Account\ConfirmPhoneSuccess.cshtml"
WriteAttributeValue("", 362, Configuration["URLClientMVC"], 362, 30, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Retourner à l\'application</a>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
