using System.Text;
using System.Text.RegularExpressions;

namespace Lastik.Helpers;

public record HtmlFormatter(string Html)
{

    public string Format() => DecodeSpecialCharacters()
        .FormatWhiteSpaces()
        .FormatLineBreaks()
        .FormatHtmlLists()
        .StripUnformattedTags().Html;

    

    private HtmlFormatter FormatHtmlLists() => FormatHtmlList("ol", i => $"{i + 1}.")
        .FormatHtmlList("ul", _ => "•");

    private HtmlFormatter FormatWhiteSpaces() =>new(
        new Regex(@"(>|$)(\W|\n|\r)+<", RegexOptions.Multiline).Replace(Html, "><"));

    private HtmlFormatter FormatLineBreaks() => new(
        new Regex(@"<(br|BR)\s{0,1}\/{0,1}>",RegexOptions.Multiline).Replace(Html,Environment.NewLine));

    private HtmlFormatter StripUnformattedTags() => new(
        new Regex("<[^>]*(>|$)",RegexOptions.Multiline).Replace(Html,string.Empty));

    private HtmlFormatter DecodeSpecialCharacters() => new(System.Net.WebUtility.HtmlDecode(Html));

    private HtmlFormatter FormatHtmlList(string listTag, Func<int, string> enumNumber)
    {
        var formattedHtml = Html;
        var list = @$"<{listTag}>(?:(?:<li>(.+?)<\/li>)+)<\/{listTag}>";
        var listRegex = new Regex(list, RegexOptions.Multiline);
        foreach (Match item in listRegex.Matches(Html))
        {
            var s = new StringBuilder();
            if (item.Groups.Count != 2) return this;
            var captures = item.Groups[1].Captures;
            
            for (var i = 0; i < captures.Count; i++)
            {
                s.Append($"{enumNumber(i)} {captures[i].Value}\n");
            }

            formattedHtml = listRegex.Replace(formattedHtml, s.ToString());
        }

        return new HtmlFormatter(formattedHtml);
    }
}