using System;

namespace GiveCRM.DummyDataGenerator
{
    public interface IGenerator
    {
        event Action<object, EventArgs<string>> Update;
        void GenerateMembers(int countToGenerate);
        void LoadMembers();
        void GenerateCampaign();
        void GenerateDonations(int minAmount, int maxAmount, int donationCountMax);
    }
}