using Edura.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.Infrastructure
{

    [HtmlTargetElement("nav", Attributes = "page-model")]
    public class PageLink : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        public PageLink(IUrlHelperFactory _urlHelperFactory)
        {
            urlHelperFactory = _urlHelperFactory;
        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PagingInfo PageModel { get; set; }
        public string PageAction { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            var result = new TagBuilder("div");
            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination justify-content-center");
            result.InnerHtml.AppendHtml(ul);

            /* sayfa numaralarından önce " < " etiketini basar. */
            var back = new TagBuilder("a");
            back.AddCssClass("page-link");
            back.InnerHtml.Append("«");
            back.Attributes["href"] = urlHelper.Action(PageAction, new { page = PageModel.CurrentPage - 1 }); // var olan current page'den bir önceki page'in linkini atar.
            var liback = new TagBuilder("li");
            liback.AddCssClass("page-item");
            if (PageModel.CurrentPage == 1)
            {  //eğer ilk sayfada isek bir sayfa geri gitmeye gerek yok. 
                back.Attributes["href"] = urlHelper.Action(PageAction, new { page = PageModel.CurrentPage });
                liback.AddCssClass("disabled");
            }
            liback.InnerHtml.AppendHtml(back);
            ul.InnerHtml.AppendHtml(liback);

            for (int i = 1; i <= PageModel.TotalPages(); i++)
            {
                var li = new TagBuilder("li");
                li.AddCssClass("page-item");
                ul.InnerHtml.AppendHtml(li);
                var tag = new TagBuilder("a");
                tag.AddCssClass("page-link");
                tag.Attributes["href"] = urlHelper.Action(PageAction, new { page = i });
                tag.InnerHtml.Append(i.ToString());
                if (i == PageModel.CurrentPage)
                {
                    li.AddCssClass("active");

                }
                else
                {
                    li.AddCssClass("");
                }
                li.InnerHtml.AppendHtml(tag);
            }
            var next = new TagBuilder("a");
            next.AddCssClass("page-link");
            next.InnerHtml.Append("»");
            next.Attributes["href"] = urlHelper.Action(PageAction, new { page = PageModel.CurrentPage + 1 });
            var linext = new TagBuilder("li");
            linext.AddCssClass("page-item");
            if (PageModel.CurrentPage == PageModel.TotalPages())
            {  //eğer son sayfada isek bir sayfa ileri gitmeye gerek yok. 
                next.Attributes["href"] = urlHelper.Action(PageAction, new { page = PageModel.CurrentPage });
                linext.AddCssClass("disabled");
            }
            linext.InnerHtml.AppendHtml(next);
            ul.InnerHtml.AppendHtml(linext);

            output.Content.AppendHtml(result.InnerHtml);
        }
    }

}
