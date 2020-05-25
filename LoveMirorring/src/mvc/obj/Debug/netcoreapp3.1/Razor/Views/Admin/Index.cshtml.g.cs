#pragma checksum "C:\Users\Paul\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Admin\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "aa0f72d8680e3f9d5f21101c39ded9f348f00dee"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_Index), @"mvc.1.0.view", @"/Views/Admin/Index.cshtml")]
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
#line 1 "C:\Users\Paul\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\mvc\Views\_ViewImports.cshtml"
using mvc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Paul\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\mvc\Views\_ViewImports.cshtml"
using mvc.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"aa0f72d8680e3f9d5f21101c39ded9f348f00dee", @"/Views/Admin/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a6a06753573638890b0ff2d42042a0bcf031bf36", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<mvc.ViewModels.Admin.IndexModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\Paul\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Admin\Index.cshtml"
  
    Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
    ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<!-- Begin Page Content -->\r\n<div class=\"container-fluid\">\r\n\r\n    <!-- Page Heading -->\r\n");
            WriteLiteral(@"
    <!-- Content Row -->
    <div class=""row"">

        <!-- Earnings (Monthly) Card Example -->
        <div class=""col-xl-3 col-md-6 mb-4"">
            <div class=""card border-left-primary shadow h-100 py-2"">
                <div class=""card-body"">
                    <div class=""row no-gutters align-items-center"">
                        <div class=""col mr-2"">
                            <div class=""text-xs font-weight-bold text-primary text-uppercase mb-1"">Earnings (Monthly)</div>
                            <div class=""h5 mb-0 font-weight-bold text-gray-800"">CHF ");
#nullable restore
#line 26 "C:\Users\Paul\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Admin\Index.cshtml"
                                                                               Write(Model.earningsMonthly);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</div>
                        </div>
                        <div class=""col-auto"">
                            <i class=""fas fa-calendar fa-2x text-gray-300""></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Earnings (Monthly) Card Example -->
        <div class=""col-xl-3 col-md-6 mb-4"">
            <div class=""card border-left-success shadow h-100 py-2"">
                <div class=""card-body"">
                    <div class=""row no-gutters align-items-center"">
                        <div class=""col mr-2"">
                            <div class=""text-xs font-weight-bold text-success text-uppercase mb-1"">Earnings (Annual)</div>
                            <div class=""h5 mb-0 font-weight-bold text-gray-800"">CHF ");
#nullable restore
#line 43 "C:\Users\Paul\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Admin\Index.cshtml"
                                                                               Write(Model.earningsAnnualy);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</div>
                        </div>
                        <div class=""col-auto"">
                            <i class=""fas fa-dollar-sign fa-2x text-gray-300""></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Earnings (Monthly) Card Example -->
        <div class=""col-xl-3 col-md-6 mb-4"">
            <div class=""card border-left-info shadow h-100 py-2"">
                <div class=""card-body"">
                    <div class=""row no-gutters align-items-center"">
                        <div class=""col mr-2"">
                            <div class=""text-xs font-weight-bold text-info text-uppercase mb-1"">Nombre d'utilisateurs</div>
                            <div class=""row no-gutters align-items-center"">
                                <div class=""col-auto"">
                                    <div class=""h5 mb-0 mr-3 font-weight-bold text-gray-800"">");
#nullable restore
#line 62 "C:\Users\Paul\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Admin\Index.cshtml"
                                                                                        Write(Model.nbUsers);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</div>
                                </div>
                                <div class=""col"">
                                    <div class=""progress progress-sm mr-2"">
                                        <div class=""progress-bar bg-info"" role=""progressbar"" style=""width: 50%""");
            BeginWriteAttribute("aria-valuenow", " aria-valuenow=\"", 3269, "\"", 3299, 1);
#nullable restore
#line 66 "C:\Users\Paul\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Admin\Index.cshtml"
WriteAttributeValue("", 3285, Model.nbUsers, 3285, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" aria-valuemin=""0"" aria-valuemax=""100""></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class=""col-auto"">
                            <i class=""fas fa-user fa-2x text-gray-300""></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class=""col-xl-3 col-md-6 mb-4"">
            <div class=""card border-left-success shadow h-100 py-2"">
                <div class=""card-body"">
                    <div class=""row no-gutters align-items-center"">
                        <div class=""col mr-2"">
                            <div class=""text-xs font-weight-bold text-success text-uppercase mb-1"">Nombre d'actions sur le site</div>
                            <div class=""h5 mb-0 font-weight-bold text-gray-800"">");
#nullable restore
#line 85 "C:\Users\Paul\Documents\GitHub\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Admin\Index.cshtml"
                                                                           Write(Model.nbConnexion);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</div>
                        </div>
                        <div class=""col-auto"">
                            <i class=""fas fa-link fa-2x text-gray-300""></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Content Row -->
    <div class=""row"">

        <!-- Content Column -->
        <div class=""col-lg-6 mb-4"">

            <!-- Project Card Example -->
            <div class=""card shadow mb-4"">
                <div class=""card-header py-3"">
                    <h6 class=""m-0 font-weight-bold text-primary"">Projects</h6>
                </div>
                <div class=""card-body"">
                    <h4 class=""small font-weight-bold"">Server Migration <span class=""float-right"">20%</span></h4>
                    <div class=""progress mb-4"">
                        <div class=""progress-bar bg-danger"" role=""progressbar"" style=""width: 20%"" aria-valuenow=""20"" aria-valuemin=""0"" aria-valuemax=""100""></di");
            WriteLiteral(@"v>
                    </div>
                    <h4 class=""small font-weight-bold"">Sales Tracking <span class=""float-right"">40%</span></h4>
                    <div class=""progress mb-4"">
                        <div class=""progress-bar bg-warning"" role=""progressbar"" style=""width: 40%"" aria-valuenow=""40"" aria-valuemin=""0"" aria-valuemax=""100""></div>
                    </div>
                    <h4 class=""small font-weight-bold"">Customer Database <span class=""float-right"">60%</span></h4>
                    <div class=""progress mb-4"">
                        <div class=""progress-bar"" role=""progressbar"" style=""width: 60%"" aria-valuenow=""60"" aria-valuemin=""0"" aria-valuemax=""100""></div>
                    </div>
                    <h4 class=""small font-weight-bold"">Payout Details <span class=""float-right"">80%</span></h4>
                    <div class=""progress mb-4"">
                        <div class=""progress-bar bg-info"" role=""progressbar"" style=""width: 80%"" aria-valuenow=""80"" aria-valuemin=""");
            WriteLiteral(@"0"" aria-valuemax=""100""></div>
                    </div>
                    <h4 class=""small font-weight-bold"">Account Setup <span class=""float-right"">Complete!</span></h4>
                    <div class=""progress"">
                        <div class=""progress-bar bg-success"" role=""progressbar"" style=""width: 100%"" aria-valuenow=""100"" aria-valuemin=""0"" aria-valuemax=""100""></div>
                    </div>
                </div>
            </div>

            <!-- Color System -->
            <div class=""row"">
                <div class=""col-lg-6 mb-4"">
                    <div class=""card bg-primary text-white shadow"">
                        <div class=""card-body"">
                            Primary
                            <div class=""text-white-50 small"">#4e73df</div>
                        </div>
                    </div>
                </div>
                <div class=""col-lg-6 mb-4"">
                    <div class=""card bg-success text-white shadow"">
                       ");
            WriteLiteral(@" <div class=""card-body"">
                            Success
                            <div class=""text-white-50 small"">#1cc88a</div>
                        </div>
                    </div>
                </div>
                <div class=""col-lg-6 mb-4"">
                    <div class=""card bg-info text-white shadow"">
                        <div class=""card-body"">
                            Info
                            <div class=""text-white-50 small"">#36b9cc</div>
                        </div>
                    </div>
                </div>
                <div class=""col-lg-6 mb-4"">
                    <div class=""card bg-warning text-white shadow"">
                        <div class=""card-body"">
                            Warning
                            <div class=""text-white-50 small"">#f6c23e</div>
                        </div>
                    </div>
                </div>
                <div class=""col-lg-6 mb-4"">
                    <div class=""card bg-da");
            WriteLiteral(@"nger text-white shadow"">
                        <div class=""card-body"">
                            Danger
                            <div class=""text-white-50 small"">#e74a3b</div>
                        </div>
                    </div>
                </div>
                <div class=""col-lg-6 mb-4"">
                    <div class=""card bg-secondary text-white shadow"">
                        <div class=""card-body"">
                            Secondary
                            <div class=""text-white-50 small"">#858796</div>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <div class=""col-lg-6 mb-4"">

            <!-- Illustrations -->
            <div class=""card shadow mb-4"">
                <div class=""card-header py-3"">
                    <h6 class=""m-0 font-weight-bold text-primary"">Illustrations</h6>
                </div>
                <div class=""card-body"">
                    <div class=");
            WriteLiteral("\"text-center\">\r\n                        <img class=\"img-fluid px-3 px-sm-4 mt-3 mb-4\" style=\"width: 25rem;\" src=\"img/undraw_posting_photo.svg\"");
            BeginWriteAttribute("alt", " alt=\"", 9515, "\"", 9521, 0);
            EndWriteAttribute();
            WriteLiteral(@">
                    </div>
                    <p>Add some quality, svg illustrations to your project courtesy of <a target=""_blank"" rel=""nofollow"" href=""https://undraw.co/"">unDraw</a>, a constantly updated collection of beautiful svg images that you can use completely free and without attribution!</p>
                    <a target=""_blank"" rel=""nofollow"" href=""https://undraw.co/"">Browse Illustrations on unDraw &rarr;</a>
                </div>
            </div>

            <!-- Approach -->
            <div class=""card shadow mb-4"">
                <div class=""card-header py-3"">
                    <h6 class=""m-0 font-weight-bold text-primary"">Development Approach</h6>
                </div>
                <div class=""card-body"">
                    <p>SB Admin 2 makes extensive use of Bootstrap 4 utility classes in order to reduce CSS bloat and poor page performance. Custom CSS classes are used to create custom components and custom utility classes.</p>
                    <p class=""mb-0""");
            WriteLiteral(">Before working with this theme, you should become familiar with the Bootstrap framework, especially the utility classes.</p>\r\n                </div>\r\n            </div>\r\n\r\n        </div>\r\n    </div>\r\n\r\n</div>\r\n<!-- /.container-fluid -->\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<mvc.ViewModels.Admin.IndexModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
