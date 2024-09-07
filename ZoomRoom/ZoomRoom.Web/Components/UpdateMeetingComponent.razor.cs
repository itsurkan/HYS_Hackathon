using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using ZoomRoom.Persistence.Models;
using ZoomRoom.Services.PersistenceServices;
namespace ZoomRoom.Web.Components
{
    public partial class UpdateMeetingComponent : ComponentBase
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private IMeetingService MeetingService { get; set; } = default!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public Meeting Model { get; set; } = new Meeting();

        private TimeSpan? _time;

        protected override void OnInitialized()
        {

            if (Model.ScheduledTime != default)
            {
                _time = Model.ScheduledTime.TimeOfDay;
            }
            else
            {
                _time = new TimeSpan(00, 45, 00);
            }
        }

        private void Confirm()
        {
            MudDialog.Close(DialogResult.Ok(true));
        }

        private void Cancel()
        {
            MudDialog.Close(DialogResult.Cancel());
        }

        private void SetSchedule(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                Model.ScheduledTime = new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day, _time?.Hours ?? 0, _time?.Minutes ?? 0, 0);
            }
        }

        private void SetTime(TimeSpan? timeSpan)
        {
            if (timeSpan.HasValue)
            {
                _time = timeSpan.Value;
                Model.ScheduledTime = Model.ScheduledTime.Date + _time.Value;
            }
        }

        private void SetSchedule(DateTime obj)
        {
            throw new NotImplementedException();
        }
    }
}
