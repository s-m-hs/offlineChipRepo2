using System;
using System.Threading.Tasks;

namespace CY_WebApi.Services
{
    public class RecaptchaService
    {
        //private readonly string _projectId = "chip-yab-1724158547751";
        private readonly string _recaptchaKey = "6LeD3SsqAAAAAPB1zdlTs86khRO9r3mTSul1PGYz";

        public async Task<bool> VerifyCaptcha(string captchaResponse)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_recaptchaKey}&response={captchaResponse}");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
              
                return true;
            }
            return false;
        }
        //public async Task<Assessment> CreateAssessmentAsync(string token, string recaptchaAction)
        //{
        //    RecaptchaEnterpriseServiceClient client = RecaptchaEnterpriseServiceClient.Create();
        //    ProjectName projectName = new ProjectName(_projectId);

        //    CreateAssessmentRequest createAssessmentRequest = new CreateAssessmentRequest
        //    {
        //        Assessment = new Assessment
        //        {
        //            Event = new Event
        //            {
        //                SiteKey = _recaptchaKey,
        //                Token = token,
        //                ExpectedAction = recaptchaAction
        //            },
        //        },
        //        ParentAsProjectName = projectName
        //    };

        //    Assessment response = await client.CreateAssessmentAsync(createAssessmentRequest);

        //    return response;
        //}
    }
}
