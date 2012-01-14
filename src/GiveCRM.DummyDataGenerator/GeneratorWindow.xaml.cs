using System;
using System.Linq;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Simple.Data;
using GiveCRM.DummyDataGenerator.Generation;

namespace GiveCRM.DummyDataGenerator
{
    public partial class GeneratorWindow
    {
        private readonly IDatabaseGenerator databaseGenerator;
        private readonly IMemberGenerator memberGenerator;
        private readonly ICampaignGenerator campaignGenerator;

        private volatile dynamic db;

        internal GeneratorWindow(IDatabaseGenerator databaseGenerator, IMemberGenerator memberGenerator, ICampaignGenerator campaignGenerator)
        {
            if (databaseGenerator == null)
            {
                throw new ArgumentNullException("databaseGenerator");
            }

            if (memberGenerator == null)
            {
                throw new ArgumentNullException("memberGenerator");
            }

            if (campaignGenerator == null)
            {
                throw new ArgumentNullException("campaignGenerator");
            }

            this.databaseGenerator = databaseGenerator;
            this.memberGenerator = memberGenerator;
            this.campaignGenerator = campaignGenerator;

            InitializeComponent();

            var connectionStringSetting = ConfigurationManager.ConnectionStrings["GiveCRM"];
            DatbaseConnectionStringTextBox.Text = connectionStringSetting.ConnectionString;
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = DatbaseConnectionStringTextBox.Text;
            TaskScheduler uiContext = TaskScheduler.FromCurrentSynchronizationContext();

            Task.Factory.StartNew(() =>
                                      {
                                          Log("Connecting to database...");
                                          db = Database.OpenConnection(connectionString);
                                          Log("Connected to database successfully");
                                      }, TaskCreationOptions.LongRunning)
                        .ContinueWith(_ => RefreshStats(uiContext))
                        .ContinueWith(_ =>
                                          {
                                              ConnectionDockPanel.IsEnabled = false;
                                              TabControl.IsEnabled = true;
                                          }, uiContext);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            TaskScheduler uiContext = TaskScheduler.FromCurrentSynchronizationContext();
            RefreshStats(uiContext);
        }

        private void RefreshStats(TaskScheduler uiContext)
        {
            Task.Factory.StartNew(() => Log("Refreshing database statistics..."), CancellationToken.None, TaskCreationOptions.None, uiContext)
                        .ContinueWith(t => new DatabaseStatisticsLoader().Load(db), TaskContinuationOptions.LongRunning)
                        .ContinueWith(t =>
                                          {
                                              DatabaseStatistics dbStats = t.Result;
                                              Log("Database statistics refreshed successfully");
                                              NumberOfMembersLabel.Content = dbStats.NumberOfMembers.ToString();
                                              NumberOfCampaignsLabel.Content = dbStats.NumberOfCampaigns.ToString();
                                              NumberOfSearchFiltersLabel.Content = dbStats.NumberOfSearchFilters.ToString();
                                              NumberOfDonationsLabel.Content = dbStats.NumberOfDonations.ToString();
                                          }, uiContext);
        }

        private void GenerateMembersButton_Click(object sender, RoutedEventArgs e)
        {
            int numberOfMembersToGenerate = Convert.ToInt32(NumberOfMembersTextBox.Text);
            RunGeneration(() => memberGenerator.Generate(numberOfMembersToGenerate));
        }
        
        private void GenerateCampaignsButton_Click(object sender, RoutedEventArgs e)
        {
            int numberOfCampaignsToGenerate = Convert.ToInt32(NumberOfCampaignsTextBox.Text);
            RunGeneration(() => campaignGenerator.Generate(numberOfCampaignsToGenerate));
        }

        private void GenerateAllButton_Click(object sender, RoutedEventArgs e)
        {
            RunGeneration(databaseGenerator.Generate);
        }

        private void RunGeneration(Action generationCallback)
        {
            // runs a generation task, disabling the generate buttons to prevent a new generate 
            //  task being started while one is still in progress
            TaskScheduler uiContext = TaskScheduler.FromCurrentSynchronizationContext();
            var task = Task.Factory.StartNew(() => SetGenerateButtonsState(false), CancellationToken.None, TaskCreationOptions.None, uiContext)
                                   .ContinueWith(_ => generationCallback(), TaskContinuationOptions.LongRunning);
            task.ContinueWith(_ =>
                                  {
                                      RefreshStats(uiContext);
                                      SetGenerateButtonsState(true);
                                  }, CancellationToken.None, TaskContinuationOptions.LongRunning, uiContext);
            task.ContinueWith(t =>
                                  {
                                      LogTaskExceptions(t);
                                      SetGenerateButtonsState(true);
                                  }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, uiContext);
        }

        private void SetGenerateButtonsState(bool state)
        {
            GenerateDatabaseButton.IsEnabled = state;
            GenerateMembersButton.IsEnabled = state;
            GenerateCampaignsButton.IsEnabled = state;
        }

        private void LogTaskExceptions(Task t)
        {
            string errorMessage = t.Exception == null
                                    ? "(No exception found)"
                                    : string.Join(Environment.NewLine, t.Exception.InnerExceptions.Select(ex => ex.Message));
            Log("Error: " + errorMessage);
        }

        public void Log(string text)
        {
            Action logAction = () =>
                                   {
                                       logArea.Text += Environment.NewLine + text;
                                       logArea.ScrollToEnd();
                                   };
            Dispatcher.Invoke(logAction);
        }
    }
}
