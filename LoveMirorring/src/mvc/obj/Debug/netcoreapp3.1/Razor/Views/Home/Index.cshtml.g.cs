#pragma checksum "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7ef1bef88264dccc4ac50b4af2056db264bc8c30"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
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
#line 1 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\_ViewImports.cshtml"
using mvc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\_ViewImports.cshtml"
using mvc.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Home\Index.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Home\Index.cshtml"
using Microsoft.AspNetCore.Authentication;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7ef1bef88264dccc4ac50b4af2056db264bc8c30", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a6a06753573638890b0ff2d42042a0bcf031bf36", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Home\Index.cshtml"
  
    ViewData["Title"] = Localizer["TitlePage"];

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral("\r\n    <h2>");
#nullable restore
#line 10 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Home\Index.cshtml"
   Write(Localizer["Title1"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n\r\n<dl>\r\n");
#nullable restore
#line 13 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Home\Index.cshtml"
     foreach (var claim in User.Claims)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <dt>");
#nullable restore
#line 15 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Home\Index.cshtml"
       Write(claim.Type);

#line default
#line hidden
#nullable disable
            WriteLiteral("</dt>\r\n        <dd>");
#nullable restore
#line 16 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Home\Index.cshtml"
       Write(claim.Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</dd>\r\n");
#nullable restore
#line 17 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Home\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</dl>\r\n\r\n<h2>");
#nullable restore
#line 20 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Home\Index.cshtml"
Write(Localizer["Title2"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n\r\n<dl>\r\n");
#nullable restore
#line 23 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Home\Index.cshtml"
     foreach (var prop in (await Context.AuthenticateAsync()).Properties.Items)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <dt>");
#nullable restore
#line 25 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Home\Index.cshtml"
       Write(prop.Key);

#line default
#line hidden
#nullable disable
            WriteLiteral("</dt>\r\n        <dd>");
#nullable restore
#line 26 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Home\Index.cshtml"
       Write(prop.Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</dd>\r\n");
#nullable restore
#line 27 "C:\repositories\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Home\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</dl>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IViewLocalizer Localizer { get; private set; }
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
