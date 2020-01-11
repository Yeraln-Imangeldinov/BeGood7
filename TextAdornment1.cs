using Microsoft.VisualStudio.Text.Editor;
using System.Windows.Input;

namespace BeGood3
{
    internal class ButtonKeyProc : KeyProcessor
    {
       
        private readonly ITextView view;
        Request_Data data;
        public ButtonKeyProc(ITextView textView)
        {
            view = textView;
            data = new Request_Data();
            data.SetValue("");
        }

        public override void KeyDown(KeyEventArgs args)
        {
            if (args.Key == Key.RightCtrl)
            {
               
                ////Entire text output
                //string result = view.Selection.StreamSelectionSpan.Snapshot.GetText();
                //Debug.WriteLine("view.Selection.StreamSelectionSpan.Snapshot.GetText() -"+result);

                string result2 = view.Selection.StreamSelectionSpan.SnapshotSpan.GetText();
             
                data.SetValue(result2);
                //int Start = view.Selection.ActivePoint.Position;
                //int End = view.Selection.AnchorPoint.Position;

                //VirtualSnapshotSpan[] result3 = new VirtualSnapshotSpan[view.Selection.VirtualSelectedSpans.Count];
                //view.Selection.VirtualSelectedSpans.CopyTo(result3, 0);

                //SnapshotSpan[] result4 = new SnapshotSpan[view.Selection.SelectedSpans.Count];
                //view.Selection.SelectedSpans.CopyTo(result4, 0);
                //string result5 = view.Selection.TextView.TextSnapshot.GetText(Start, End);
                //Debug.WriteLine("view.Selection.TextView.TextSnapshot.GetText(Start, End) -"+result5);
                //string result6 = view.Selection.TextView.TextSnapshot.GetText();
                //Debug.WriteLine("view.Selection.TextView.TextSnapshot.GetText() -"+result6);
                //char[] result7 = new char[view.Selection.TextView.TextSnapshot.Length];
                //result7 = view.Selection.TextView.TextSnapshot.ToCharArray(Start, view.Selection.TextView.TextSnapshot.Length);
                //string result77 = new string (result7);
                //Debug.WriteLine("view.Selection.TextView.TextSnapshot.ToCharArray(Start, view.Selection.TextView.TextSnapshot.Length) -"+result77);

                //int total = view.Selection.StreamSelectionSpan.Length;
                //if (total <= 0) return;
                //char[] abs = new char[total];
                //for (int i = 0; i < total; i++)
                //{
                //    abs[i] = view.Selection.StreamSelectionSpan.Snapshot.GetText()

                //}

               
            }
        }

        public bool IsAlt
        {
            get { return Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt); }
        }
    }
}
