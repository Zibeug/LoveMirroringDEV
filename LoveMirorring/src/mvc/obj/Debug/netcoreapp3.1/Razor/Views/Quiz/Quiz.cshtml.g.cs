#pragma checksum "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "830c19fca983691a7643125053350ba0a29bc3a7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Quiz_Quiz), @"mvc.1.0.view", @"/Views/Quiz/Quiz.cshtml")]
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
#line 1 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\_ViewImports.cshtml"
using mvc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\_ViewImports.cshtml"
using mvc.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"830c19fca983691a7643125053350ba0a29bc3a7", @"/Views/Quiz/Quiz.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a6a06753573638890b0ff2d42042a0bcf031bf36", @"/Views/_ViewImports.cshtml")]
    public class Views_Quiz_Quiz : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-primary"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", new global::Microsoft.AspNetCore.Html.HtmlString("button"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "QuizSubmit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route", "Quiz", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
  
    ViewData["Title"] = "Quizz";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Laissez nous établir votre plan de match</h1>\r\n<pre>");
#nullable restore
#line 7 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
Write(ViewBag.Json);

#line default
#line hidden
#nullable disable
            WriteLiteral("</pre>\r\n\r\n");
#nullable restore
#line 9 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
 if (ViewData["message"].Equals("success"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <p>Bien joué vous avez répondu au Quizz</p>\r\n");
#nullable restore
#line 12 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "830c19fca983691a7643125053350ba0a29bc3a75878", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 16 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
          
            int cpt = 0;
        

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
         foreach (Question question in ViewData["questions"] as List<Question>)
        {

#line default
#line hidden
#nullable disable
                WriteLiteral("            <div class=\"container-fluid\"");
                BeginWriteAttribute("id", " id=\"", 457, "\"", 482, 1);
#nullable restore
#line 21 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
WriteAttributeValue("", 462, question.QuestionId, 462, 20, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n                <div class=\"modal-dialog\">\r\n                    <div class=\"modal-content\">\r\n                        <div class=\"modal-header\">\r\n                            <h3><span class=\"label label-warning\" id=\"q@question.QuestionId\">");
#nullable restore
#line 25 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
                                                                                        Write(question.QuestionId);

#line default
#line hidden
#nullable disable
                WriteLiteral(" </span>");
#nullable restore
#line 25 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
                                                                                                                    Write(question.QuestionText);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"</h3>
                        </div>
                        <div class=""modal-body"">
                            <div class=""col-xs-3 col-xs-offset-5"">
                                <div id=""loadbar"" style=""display: none;"">
                                    <div class=""blockG"" id=""rotateG_01""></div>
                                    <div class=""blockG"" id=""rotateG_02""></div>
                                    <div class=""blockG"" id=""rotateG_03""></div>
                                    <div class=""blockG"" id=""rotateG_04""></div>
                                    <div class=""blockG"" id=""rotateG_05""></div>
                                    <div class=""blockG"" id=""rotateG_06""></div>
                                    <div class=""blockG"" id=""rotateG_07""></div>
                                    <div class=""blockG"" id=""rotateG_08""></div>
                                </div>
                            </div>

                            <div class=""quiz""");
                BeginWriteAttribute("id", " id=\"", 1769, "\"", 1794, 1);
#nullable restore
#line 41 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
WriteAttributeValue("", 1774, question.QuestionId, 1774, 20, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" data-toggle=\"buttons\">\r\n\r\n\r\n");
#nullable restore
#line 44 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
                                 foreach (Answer answer in ViewData["answer"] as List<Answer>)
                                {

                                    if (question.QuestionId == answer.QuestionId)
                                    {

#line default
#line hidden
#nullable disable
                WriteLiteral(@"                                        <label class=""element-animation1 btn btn-lg btn-primary btn-block"">
                                            <span class=""btn-label"">
                                                <i class=""glyphicon glyphicon-chevron-right""></i>
                                            </span>
                                            <input type=""radio""");
                BeginWriteAttribute("name", " name=\"", 2473, "\"", 2492, 3);
                WriteAttributeValue("", 2480, "answer[", 2480, 7, true);
#nullable restore
#line 53 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
WriteAttributeValue("", 2487, cpt, 2487, 4, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 2491, "]", 2491, 1, true);
                EndWriteAttribute();
                BeginWriteAttribute("value", " value=\"", 2493, "\"", 2520, 1);
#nullable restore
#line 53 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
WriteAttributeValue("", 2501, answer.AnswerValue, 2501, 19, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral("> ");
#nullable restore
#line 53 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
                                                                                                            Write(answer.AnswerText);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                        </label>\r\n");
#nullable restore
#line 55 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
                                    }
                                }

#line default
#line hidden
#nullable disable
                WriteLiteral("                            </div>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n");
#nullable restore
#line 62 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
            cpt++;
        }

#line default
#line hidden
#nullable disable
                WriteLiteral("        <div class=\"form-group\">\r\n            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("button", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "830c19fca983691a7643125053350ba0a29bc3a712092", async() => {
                    WriteLiteral("Soumettre mes réponses");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.Action = (string)__tagHelperAttribute_2.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n        </div>\r\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Route = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 68 "C:\DEV\LoveMirroringDEV\LoveMirorring\src\mvc\Views\Quiz\Quiz.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n");
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
