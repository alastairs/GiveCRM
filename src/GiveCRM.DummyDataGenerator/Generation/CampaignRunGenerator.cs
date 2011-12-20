using System.Linq;
using Simple.Data;
using GiveCRM.BusinessLogic;

namespace GiveCRM.DummyDataGenerator.Generation
{
    public sealed class CampaignRunGenerator : ICampaignRunGenerator
    {
        private readonly dynamic db = Database.OpenNamedConnection("GiveCRM");
        private readonly IMemberService memberService;

        internal CampaignRunGenerator(IMemberService memberService)
        {
            this.memberService = memberService;
        }

        public void GenerateCampaignRun(int campaignId)
        {
            var campaignMembers = this.memberService.SearchByCampaignId(campaignId);
            var memberCampaignMemberships = campaignMembers.Select(member => new {CampaignId = campaignId, MemberId = member.Id}).ToList();

            if (memberCampaignMemberships.Any())
            {
                db.CampaignRuns.Insert(memberCampaignMemberships);
            }
        }
    }
}