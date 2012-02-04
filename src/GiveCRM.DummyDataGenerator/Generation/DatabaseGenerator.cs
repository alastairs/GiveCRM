using System;
using Simple.Data;

namespace GiveCRM.DummyDataGenerator.Generation
{
    using Ninject.Extensions.Logging;

    public class DatabaseGenerator : IDatabaseGenerator
    {
        private readonly RandomSource random = new RandomSource();
        private readonly dynamic db = Database.OpenNamedConnection("GiveCRM");
        private readonly IMemberGenerator memberGenerator;
        private readonly ICampaignGenerator campaignGenerator;
        private readonly IDonationGenerator donationGenerator;
        private readonly ILogger logger;

        public DatabaseGenerator(IMemberGenerator memberGenerator, ICampaignGenerator campaignGenerator, IDonationGenerator donationGenerator, ILogger logger)
        {
            if (memberGenerator == null)
            {
                throw new ArgumentNullException("memberGenerator");
            }

            if (campaignGenerator == null)
            {
                throw new ArgumentNullException("campaignGenerator");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this.memberGenerator = memberGenerator;
            this.campaignGenerator = campaignGenerator;
            this.donationGenerator = donationGenerator;
            this.logger = logger;
        }

        public void Generate()
        {
            EmptyDatabase();
            GenerateMembers();
            GenerateCampaigns();
            GenerateDonations();
            logger.Info("Database generated successfully");
        }

        private void EmptyDatabase()
        {
            logger.Info("Emptying database...");
            db.Donations.DeleteAll();
            db.CampaignRuns.DeleteAll();
            db.MemberSearchFilters.DeleteAll();
            db.Campaigns.DeleteAll();
            db.PhoneNumbers.DeleteAll();
            db.MemberFacetValues.DeleteAll();
            db.MemberFacets.DeleteAll();
            db.Members.DeleteAll();
            db.FacetValues.DeleteAll();
            db.Facets.DeleteAll();
        }

        private void GenerateMembers()
        {
            Generate(memberGenerator, 1000, 10000);
        }

        private void GenerateCampaigns()
        {
            Generate(campaignGenerator, 15, 100);
        }

        private void GenerateDonations()
        {
            donationGenerator.Generate();
        }

        private void Generate(IBaseGenerator generator, int minNumber, int maxNumber)
        {
            var numberToGenerate = random.NextInt(minNumber, maxNumber);
            generator.Generate(numberToGenerate);
        }
    }
}
