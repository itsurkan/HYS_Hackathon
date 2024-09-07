using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Web.Components
{
    public partial class MeetingDetailsComponent : ComponentBase
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public Meeting Model { get; set; }

        protected override void OnInitialized()
        {
            StateHasChanged();
        }
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task CopyToClipboard()
        {
            await JS.InvokeVoidAsync("navigator.clipboard.writeText", Model.ZoomLink);
            Snackbar.Add("Скопійовано!", Severity.Success);
        }
    }

}
