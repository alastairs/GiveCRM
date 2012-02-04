using System;
using GiveCRM.BusinessLogic;
using GiveCRM.Models;
using GiveCRM.DummyDataGenerator.Data;

namespace GiveCRM.DummyDataGenerator.Generation
{
    using Ninject.Extensions.Logging;

    public sealed class CampaignGenerator : BaseGenerator, ICampaignGenerator
    {
        internal override string GeneratedItemType{get {return "campaigns";}}

        private readonly RandomSource random = new RandomSource();
        private readonly IRepository<Campaign> campaignRepository;
        private readonly IMemberSearchFilterGenerator memberSearchFilterGenerator;
        private readonly ICampaignRunGenerator campaignRunGenerator;

        public CampaignGenerator(IRepository<Campaign> campaignRepository, IMemberSearchFilterGenerator memberSearchFilterGenerator, ICampaignRunGenerator campaignRunGenerator, ILogger logger) : base(logger)
        {
            if (campaignRepository == null)
            {
                throw new ArgumentNullException("campaignRepository");
            }

            if (memberSearchFilterGenerator == null)
            {
                throw new ArgumentNullException("memberSearchFilterGenerator");
            }

            if (campaignRunGenerator == null)
            {
                throw new ArgumentNullException("campaignRunGenerator");
            }

            this.campaignRepository = campaignRepository;
            this.memberSearchFilterGenerator = memberSearchFilterGenerator;
            this.campaignRunGenerator = campaignRunGenerator;
        }

        public override void Generate(int numberToGenerate)
        {
            GenerateMultiple(numberToGenerate, () =>
                                                   {
                                                       var campaign = GenerateCampaign();
                                                       campaign = campaignRepository.Insert(campaign);
                                                       memberSearchFilterGenerator.GenerateMemberSearchFilters(campaign.Id);

                                                       if (campaign.IsCommitted)
                                                       {
                                                           // if the campaign is committed, the members that meet the campaign's search criteria also need creating
                                                           campaignRunGenerator.GenerateCampaignRun(campaign.Id);
                                                       }
                                                   });
        }

        private Campaign GenerateCampaign()
        {
            string campaignName = string.Format("{0} {1}", random.PickFromList(FemaleNames.Data), random.PickFromList(CampaignNames.CampaignSuffix));

            // close 10% of campaigns
            bool isClosed = random.Percent(10);

            // run (commit) 50% of campaigns, but all of the ones that are closed
            DateTime? runOn = isClosed || random.Percent(50) ? DateTime.Today : (DateTime?) null;

            var campaign = new Campaign
                               {
                                           Name = campaignName,
                                           Description = "A test campaign",
                                           IsClosed = isClosed  ? "Y" : "N",
                                           RunOn = runOn
                               };
            return campaign;
        }
    }
}
