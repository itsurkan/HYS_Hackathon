using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using System.Reflection.Metadata;
using ZoomRoom.Persistence.Models;
using ZoomRoom.Services.PersistenceServices;
using ZoomRoom.Web.Components;
using static MudBlazor.CategoryTypes;
using ComponentBase = Microsoft.AspNetCore.Components.ComponentBase;


namespace ZoomRoom.Web.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject] public IDialogService DialogService { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [Inject] public IMeetingService MeetingService { get; set; } = default!;

        private IEnumerable<Meeting> _meetings;
        private bool _loading = true;
        protected override async Task OnInitializedAsync()
        {
             _meetings = await MeetingService.GetAllMeetingsAsync();

            _loading = false;   
        }

        private async void GetDetails(Meeting meeting)
        {
           
            var options = new DialogOptions { CloseOnEscapeKey = true, FullWidth = true, MaxWidth = MaxWidth.Small };

            var parameter = new DialogParameters
            {
                { "Model", meeting }
             
            };

            var dialog = await DialogService.ShowAsync<MeetingDetailsComponent>($"Деталі", parameter, options);

        }

        private async void DeleteMeeting(Meeting meeting)
        {
           

            var dialog = await DialogService.ShowAsync<ConfirmActionComponent>($"Підтвердіть дії");
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await MeetingService.DeleteMeetingAsync(meeting.Id);
                _meetings = await MeetingService.GetAllMeetingsAsync();
                StateHasChanged();
                Snackbar.Add("Видалено!", Severity.Success);
            }

        }

        private async void EditMeeting(Meeting meeting)
        {

           

        }
    }
}
