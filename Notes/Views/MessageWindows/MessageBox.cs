using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Views.MessageWindows
{
    public static class MessageBox
    {
        public static void Show(Avalonia.Controls.Window Owner, string Message, string Title)
        {
            MessageBoxView window = new MessageBoxView(Message, Title);

            window.ShowDialog(Owner);
        }
    }
}
