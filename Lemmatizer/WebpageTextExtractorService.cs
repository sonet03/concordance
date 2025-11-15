using System;
using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lemmatizer;

public class WebpageTextExtractorService(HttpClient httpClient)
{
    public async Task<string> GetTextFromUrlAsync(string url)
    {
        try
        {
            var htmlContent = await httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//body");
            if (node != null)
            {
                return node.InnerText;
            }
            
            return htmlDoc.DocumentNode.InnerText;
        }
        catch (HttpRequestException ex)
        {
            return $"Error during page load: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Error during HTML processing: {ex.Message}";
        }
    }
}