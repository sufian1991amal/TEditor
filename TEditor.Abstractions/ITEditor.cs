using System;
using System.Threading.Tasks;
using static TEditor.Abstractions.TEditorToolbarItem;

namespace TEditor.Abstractions
{
    public interface ITEditor : IDisposable
    {
        Task<TEditorResponse> ShowTEditor(string html, ToolbarBuilder toolbarBuilder = null, EventHandler<ToolbarBuilderEventArgs> toolbarBuilderOnClick = null, bool autoFocusInput = false);

    }

    public class ToolbarBuilderEventArgs : EventArgs
    {
        public EnumAction Action { get; set; }
    }
}
