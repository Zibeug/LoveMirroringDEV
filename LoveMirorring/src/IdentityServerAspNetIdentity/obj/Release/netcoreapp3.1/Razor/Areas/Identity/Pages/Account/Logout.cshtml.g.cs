#pragma checksum "C:\repositories\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Areas\Identity\Pages\Account\Logout.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "717290da7507f9329619df9fc5a00aa15ca987a3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Identity_Pages_Account_Logout), @"mvc.1.0.razor-page", @"/Areas/Identity/Pages/Account/Logout.cshtml")]
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
#line 1 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Areas\Identity\Pages\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Areas\Identity\Pages\_ViewImports.cshtml"
using IdentityServerAspNetIdentity.Areas.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Areas\Identity\Pages\_ViewImports.cshtml"
using IdentityServerAspNetIdentity.Areas.Identity.Pages;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Areas\Identity\Pages\_ViewImports.cshtml"
using IdentityServerAspNetIdentity.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Areas\Identity\Pages\Account\_ViewImports.cshtml"
using IdentityServerAspNetIdentity.Areas.Identity.Pages.Account;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"717290da7507f9329619df9fc5a00aa15ca987a3", @"/Areas/Identity/Pages/Account/Logout.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0220c8ad2a68c3b4271314609ada132f09e3bc96", @"/Areas/Identity/Pages/_ViewImports.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6042a0659bbcb79dd0f392ab521b4c3a6dd47a1c", @"/Areas/Identity/Pages/Account/_ViewImports.cshtml")]
    public class Areas_Identity_Pages_Account_Logout : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Areas\Identity\Pages\Account\Logout.cshtml"
  
    ViewData["Title"] = "Log out";

#line default
#line hidden
#nullable disable
            WriteLiteral("\n<header>\n    <h1>");
#nullable restore
#line 8 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\IdentityServerAspNetIdentity\Areas\Identity\Pages\Account\Logout.cshtml"
   Write(ViewData["Title"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\n    <p>You have successfully logged out of the application.</p>\n</header>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<LogoutModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<LogoutModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<LogoutModel>)PageContext?.ViewData;
        public LogoutModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
