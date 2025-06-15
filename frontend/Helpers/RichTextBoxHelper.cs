using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using RichTextBox = System.Windows.Controls.RichTextBox;

namespace Lastik.Helpers;

public class RichTextBoxHelper
{
    public static readonly DependencyProperty HtmlTextProperty = DependencyProperty.RegisterAttached(
        "HtmlText", typeof(string), typeof(RichTextBoxHelper),
        new PropertyMetadata(default(string?), HtmlTextPropertyChangedCallback));

    public static void SetHtmlText(DependencyObject element, string? value)=>element.SetValue(HtmlTextProperty, value);

    public static string? GetHtmlText(DependencyObject element)=>(string?) element.GetValue(HtmlTextProperty);

    private static void HtmlTextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not string htmlText) return;

        switch (d)
        {
            case RichTextBox richTextBox:
                SetRichTextBoxText(richTextBox, htmlText);
                break;

            case TextBlock textBlock:
                SetTextBlockText(textBlock, htmlText);
                break;
        }
    }

    private static void SetTextBlockText(TextBlock textBlock, string htmlText)=>textBlock.Text = new HtmlFormatter(htmlText).Format();

    private static void SetRichTextBoxText(RichTextBox richTextBox, string htmlText)
    {
        richTextBox.Document.Blocks.Clear();
        richTextBox.Document.Blocks.Add(new Paragraph(new Run(new HtmlFormatter(htmlText).Format())));
    }
}