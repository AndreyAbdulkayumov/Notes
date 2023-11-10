using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Views.MessageWindows
{
    public static class MessageBox
    {
        public static void Show(Window Owner, string Message, string Title)
        {
            MessageBoxView window = new MessageBoxView(Message, Title, MessageBoxToolType.Default);

            window.ShowDialog(Owner);
        }

        public async static Task<MessageBoxResult> ShowYesNo(Window Owner, string Message, string Title)
        {
            MessageBoxView window = new MessageBoxView(Message, Title, MessageBoxToolType.YesNo);

            await window.ShowDialog(Owner).ConfigureAwait(false);

            return window.Result;
        }
    }
}
