using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.FileProviders;

namespace TelescopeShop.TagHelpers;

/// <summary>
/// Подставляет изображение товара по конвенции /Product/telescope-{id}.{расширение};
/// если файла нет — заглушку /Product/placeholder.svg.
/// </summary>
[HtmlTargetElement("img", Attributes = ProductSrcAttributeName)]
public sealed class ProductImageTagHelper : TagHelper
{
    private const string ProductSrcAttributeName = "asp-product-src";

    private static readonly string[] Extensions = [".png", ".webp", ".jpg", ".jpeg"];

    private readonly IWebHostEnvironment _environment;

    public ProductImageTagHelper(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HtmlAttributeName(ProductSrcAttributeName)]
    public int ProductId { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var provider = _environment.WebRootFileProvider;
        IFileInfo? file = null;

        foreach (var extension in Extensions)
        {
            var candidate = provider.GetFileInfo($"/Product/telescope-{ProductId}{extension}");
            if (candidate.Exists)
            {
                file = candidate;
                break;
            }
        }

        file ??= provider.GetFileInfo("/Product/placeholder.svg");

        var src = $"/Product/{file.Name}";
        if (file.Exists)
        {
            src += $"?v={file.LastModified.UtcTicks}";
        }

        output.Attributes.SetAttribute("src", src);
    }
}
