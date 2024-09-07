using Microsoft.AspNetCore.Components;
using ZoomRoom.Persistence.Models;
using ZoomRoom.Services.PersistenceServices;

namespace ZoomRoom.Web.Pages
{
    public partial class Index
    {
        [Inject] public IMeetingService MeetingService { get; set; } = default!;

        private IEnumerable<Meeting> _meetings;
        private bool _loading = true;
        protected override async Task OnInitializedAsync()
        {
             _meetings = await MeetingService.GetAllMeetingsAsync();

            _loading = false;   
        }
    }
}
