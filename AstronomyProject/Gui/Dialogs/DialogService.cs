using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Dialogs
{
    public class DialogService : IDialogService
    {
        public void ShowDialog(string title, string content)
        {
            var dialog = new Views.Dialog(title, content);
            dialog.ShowDialog();
        }
    }
}
