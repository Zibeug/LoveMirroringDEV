#pragma checksum "C:\Users\paugillet\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Views\Account\SignUpSuccess.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "09342fb30b032beed7ca0b8e978fa28eb679989c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_SignUpSuccess), @"mvc.1.0.view", @"/Views/Account/SignUpSuccess.cshtml")]
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
#line 1 "C:\Users\paugillet\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Views\_ViewImports.cshtml"
using IdentityServer4.Quickstart.UI;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09342fb30b032beed7ca0b8e978fa28eb679989c", @"/Views/Account/SignUpSuccess.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"57b49bb18fbe88f2fb7e20eb130e64338d7f2c37", @"/Views/_ViewImports.cshtml")]
    public class Views_Account_SignUpSuccess : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("   \r\n    <div class=\"container\">\r\n        <div class=\"page-header\">\r\n            <h1>Votre inscription est terminée</h1>\r\n        </div>\r\n\r\n        <p>Vous recevrez un email à cette adresse : <strong>");
#nullable restore
#line 7 "C:\Users\paugillet\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Views\Account\SignUpSuccess.cshtml"
                                                       Write(Html.ViewData.Model.Email);

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong> pour vous informer quand votre compte sera actif.</p>\r\n    </div>");
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
