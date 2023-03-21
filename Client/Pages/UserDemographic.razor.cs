using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace HealthyHands.Client.Pages
{
    public partial class UserDemographic
    {
        [Parameter]
        public UserDemographics Demographics { get; set; }
    }
}
