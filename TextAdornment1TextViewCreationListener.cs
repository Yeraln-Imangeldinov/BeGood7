using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
namespace BeGood3
{

    [Export(typeof(IKeyProcessorProvider))]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [ContentType("any")]
    [Name("ButtonProvider")]
    [Order(Before = "default")]
    internal class ButtonProvider : IKeyProcessorProvider
    {
        [ImportingConstructor]
        public ButtonProvider()
        {
        }

        public KeyProcessor GetAssociatedProcessor(IWpfTextView wpfTextView)
        {
            return new ButtonKeyProc(wpfTextView);
        }
    }
}
