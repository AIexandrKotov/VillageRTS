using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VillageRTS
{
    public partial class Form1 : Form
    {
        public Gameplay CurrentGameplay = new Gameplay().Init();

        public Form1()
        {
            InitializeComponent();
            UpdateLocale();
            BuildingCost = new ResourcePanel(BuildingCostPanel).SetColumns(2);
            BuildingProduction = new ResourcePanel(BuildingProductionPanel).SetColumns(2);
            ActionCost = new ResourcePanel(ActionPanel).SetColumns(2);
            ActionBuildings = new ResourcePanel(ActionBuildingsPanel).SetColumns(2);
            //EnvironmentRes = new ResourcePanel(EnvironmentPanel);

            Ticker.Interval = 1000;

            MyPowerLabel.ForeColor = ResourcePanel.GetColor(Resource.My_Power);
            MyHumansLimitLabel.ForeColor = ResourcePanel.GetColor(Resource.My_HumansLimit);
            MyHumansLabel.ForeColor = ResourcePanel.GetColor(Resource.My_Humans);
            MyGoldLabel.ForeColor = ResourcePanel.GetColor(Resource.My_Gold);
            MyWoodLabel.ForeColor = ResourcePanel.GetColor(Resource.My_Wood);
            MyStoneLabel.ForeColor = ResourcePanel.GetColor(Resource.My_Stone);
            MyIronLabel.ForeColor = ResourcePanel.GetColor(Resource.My_Iron);
            MyFoodLabel.ForeColor = ResourcePanel.GetColor(Resource.My_Food);
            MyFuelLabel.ForeColor = ResourcePanel.GetColor(Resource.My_Fuel);
            MyToolsLabel.ForeColor = ResourcePanel.GetColor(Resource.My_Tools);
            MyClothesLabel.ForeColor = ResourcePanel.GetColor(Resource.My_Clothes);
            MyLeatherLabel.ForeColor = ResourcePanel.GetColor(Resource.My_Leather);

            VillageHumansLimitLabel.ForeColor = ResourcePanel.GetColor(Resource.Village_HumansLimit);
            VillageHumansLabel.ForeColor = ResourcePanel.GetColor(Resource.Village_Humans);
            VillageGoldLabel.ForeColor = ResourcePanel.GetColor(Resource.Village_Gold);
            VillageWoodLabel.ForeColor = ResourcePanel.GetColor(Resource.Village_Wood);
            VillageStoneLabel.ForeColor = ResourcePanel.GetColor(Resource.Village_Stone);
            VillageIronLabel.ForeColor = ResourcePanel.GetColor(Resource.Village_Iron);
            VillageFoodLabel.ForeColor = ResourcePanel.GetColor(Resource.Village_Food);
            VillageFuelLabel.ForeColor = ResourcePanel.GetColor(Resource.Village_Fuel);
            VillageToolsLabel.ForeColor = ResourcePanel.GetColor(Resource.Village_Tools);
            VillageClothesLabel.ForeColor = ResourcePanel.GetColor(Resource.Village_Clothes);
            VillageLeatherLabel.ForeColor = ResourcePanel.GetColor(Resource.Village_Leather);

            tabControl1.TabPages.RemoveAt(2);

            StartNewGame(new Gameplay().Init().TryLoad());
        }

        public void UpdateLocale()
        {
            GameTS.Text = Program.Text["M_Game"];
            NewGameSMI.Text = Program.Text["M_Game_New"];
            MyResourcesGroup.Text = Program.Text["T_MYRES"];
            VillageResourcesGroup.Text = Program.Text["T_VLGRES"];
            EnvironmentResourcesGroup.Text = Program.Text["T_ENVRES"];
            ActionsGroup.Text = Program.Text["T_ACTSG"];

            MyPowerLabel.Text = Program.Text["Res_Power"];
            MyHumansLimitLabel.Text = Program.Text["Res_HumansLimit"];
            MyHumansLabel.Text = Program.Text["Res_Humans"];
            MyGoldLabel.Text = Program.Text["Res_Gold"];
            MyWoodLabel.Text = Program.Text["Res_Wood"];
            MyStoneLabel.Text = Program.Text["Res_Stone"];
            MyIronLabel.Text = Program.Text["Res_Iron"];
            MyFoodLabel.Text = Program.Text["Res_Food"];
            MyFuelLabel.Text = Program.Text["Res_Fuel"];
            MyToolsLabel.Text = Program.Text["Res_Tools"];
            MyClothesLabel.Text = Program.Text["Res_Clothes"];
            MyLeatherLabel.Text = Program.Text["Res_Leather"];

            VillageHumansLimitLabel.Text = Program.Text["Res_HumansLimit"];
            VillageHumansLabel.Text = Program.Text["Res_Humans"];
            VillageGoldLabel.Text = Program.Text["Res_Gold"];
            VillageWoodLabel.Text = Program.Text["Res_Wood"];
            VillageStoneLabel.Text = Program.Text["Res_Stone"];
            VillageIronLabel.Text = Program.Text["Res_Iron"];
            VillageFoodLabel.Text = Program.Text["Res_Food"];
            VillageFuelLabel.Text = Program.Text["Res_Fuel"];
            VillageToolsLabel.Text = Program.Text["Res_Tools"];
            VillageClothesLabel.Text = Program.Text["Res_Clothes"];
            VillageLeatherLabel.Text = Program.Text["Res_Leather"];

            EnvFree_SpaceLabel.Text = Program.Text["Res_Free_Space"];
            EnvFieldsLabel.Text = Program.Text["Res_Fields"];
            EnvForestsLabel.Text = Program.Text["Res_Forests"];
            EnvRiversLabel.Text = Program.Text["Res_Rivers"];
            EnvMountainsLabel.Text = Program.Text["Res_Mountains"];
            EnvTerritoryLabel.Text = Program.Text["Res_Territory"];

            ActionDoButton.Text = Program.Text["B_Do"];
            ActionReverseDoButton.Text = Program.Text["B_DoReverse"];
            BuildingBuildButton.Text = Program.Text["B_Build"];
            BuildingDestructButton.Text = Program.Text["B_Destruct"];

            UpdateBuildBuilding();
        }

        public void StartNewGame(Gameplay gameplay)
        {
            CurrentGameplay = gameplay;
            UpdateBuildBuilding();
            Ticker.Start();
        }

        public void UpdateBuildBuilding()
        {
            if (CurrentGameplay != null)
            {
                BuildingListBox.Items.Clear();
                BuildingListBox.Items.AddRange(CurrentGameplay.Buildings.Keys.Select(x => Building.Buildings[x].Title).ToArray());

                ActionsListBox.Items.Clear();
                ActionsListBox.Items.AddRange(Array.ConvertAll(Action.Actions, x => x.Title));
            }
        }

        private void Ticker_Tick(object sender, EventArgs e)
        {
            if (CurrentGameplay != null)
            {
                CurrentGameplay.Tick();
                UpdateInterface();
                if (CurrentGameplay.GameOver())
                {
                    CurrentGameplay = null;
                    Ticker.Stop();
                }
            }
        }


        private static string Kround(int a)
        {
            if (a > 1000000) return (a / 10000 / 100.0).ToString() + "kk";
            else if (a > 1000) return (a / 10 / 100.0).ToString() + "k";
            else return a.ToString();
        }

        public void Fill(TextBox tb, Resource resource)
        {
            tb.Text = Kround(CurrentGameplay.Current[resource]);
            tb.BackColor = ResourcePanel.GetCountColor(resource, CurrentGameplay.Current[resource]);
        }

        public void UpdateInterface()
        {
            if (CurrentGameplay != null)
            {
                Fill(MyPowerCount, Resource.My_Power);
                Fill(MyHumansLimitCount, Resource.My_HumansLimit);
                Fill(MyHumansCount, Resource.My_Humans);
                Fill(MyGoldCount, Resource.My_Gold);
                Fill(MyWoodCount, Resource.My_Wood);
                Fill(MyStoneCount, Resource.My_Stone);
                Fill(MyIronCount, Resource.My_Iron);
                Fill(MyFoodCount, Resource.My_Food);
                Fill(MyFuelCount, Resource.My_Fuel);
                Fill(MyToolsCount, Resource.My_Tools);
                Fill(MyClothesCount, Resource.My_Clothes);
                Fill(MyLeatherCount, Resource.My_Leather);

                Fill(VillageHumansLimitCount, Resource.Village_HumansLimit);
                Fill(VillageHumansCount, Resource.Village_Humans);
                Fill(VillageGoldCount, Resource.Village_Gold);
                Fill(VillageWoodCount, Resource.Village_Wood);
                Fill(VillageStoneCount, Resource.Village_Stone);
                Fill(VillageIronCount, Resource.Village_Iron);
                Fill(VillageFoodCount, Resource.Village_Food);
                Fill(VillageFuelCount, Resource.Village_Fuel);
                Fill(VillageToolsCount, Resource.Village_Tools);
                Fill(VillageClothesCount, Resource.Village_Clothes);
                Fill(VillageLeatherCount, Resource.Village_Leather);

                Fill(EnvFree_SpaceCount, Resource.Env_Free_Space);
                Fill(EnvFieldsCount, Resource.Env_Fields);
                Fill(EnvForestsCount, Resource.Env_Forests);
                Fill(EnvRiversCount, Resource.Env_Rivers);
                Fill(EnvMountainsCount, Resource.Env_Mountains);
                Fill(EnvTerritoryCount, Resource.Env_Territory);

                UpdateButtons();
            }
        }

        private Action CurrentAction;
        private int current_building;
        private ResourcePanel BuildingCost, BuildingProduction, ActionCost, ActionBuildings;
        private void BuildingListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentGameplay != null && BuildingListBox.SelectedIndex != -1)
            {
                current_building = CurrentGameplay.Buildings.Keys.ElementAt(BuildingListBox.SelectedIndex);
                UpdateBuilding();
                UpdateButtons();
            }
        }

        public void UpdateButtons()
        {
            if (CurrentGameplay != null && BuildingListBox.SelectedIndex != -1)
            {
                BuildingBuildButton.Enabled = CurrentGameplay.Buildings[current_building] < Building.Buildings[current_building].MaxCount && Building.Buildings[current_building].Cost.All(x => x.Value >= 0 || CurrentGameplay.Current[x.Key] >= -x.Value);
                BuildingDestructButton.Enabled = !Building.Buildings[current_building].Indestructible && CurrentGameplay.Buildings[current_building] > 0 && Building.Buildings[current_building].Cost.All(x => x.Value <= 0 || CurrentGameplay.Current[x.Key] >= x.Value);
                NowBuldings.Text = CurrentGameplay.Buildings[current_building].ToString();
            }
            if (CurrentGameplay != null && CurrentAction != null && ActionsListBox.SelectedIndex != -1)
            {
                ActionDoButton.Enabled = CurrentAction.AllowDo(CurrentGameplay, 1);
                ActionX10DoButton.Enabled = CurrentAction.AllowDo(CurrentGameplay, 10);
                ActionX100DoButton.Enabled = CurrentAction.AllowDo(CurrentGameplay, 100);
                ActionReverseDoButton.Enabled = CurrentAction.AllowReverseDo(CurrentGameplay, 1);
                ActionX10ReverseDoButton.Enabled = CurrentAction.AllowReverseDo(CurrentGameplay, 10);
                ActionX100ReverseDoButton.Enabled = CurrentAction.AllowReverseDo(CurrentGameplay, 100);
            }
        }

        private void BuildingDestructButton_Click(object sender, EventArgs e)
        {
            if (CurrentGameplay != null && BuildingListBox.SelectedIndex != -1)
            {
                Building.Buildings[current_building].OnDestruction(CurrentGameplay);
                CurrentGameplay.Buildings[current_building] -= 1;
                UpdateButtons();
            }
        }

        private void BuildingListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BuildingBuildButton_Click(sender, e);
        }

        private void ActionDoButton_Click(object sender, EventArgs e)
        {
            if (CurrentGameplay != null && ActionsListBox.SelectedIndex != -1)
            {
                CurrentAction.Do(CurrentGameplay, 1);
                UpdateButtons();
            }
        }

        private void ActionX10DoButton_Click(object sender, EventArgs e)
        {
            if (CurrentGameplay != null && ActionsListBox.SelectedIndex != -1)
            {
                CurrentAction.Do(CurrentGameplay, 10);
                UpdateButtons();
            }
        }

        private void ActionX100DoButton_Click(object sender, EventArgs e)
        {
            if (CurrentGameplay != null && ActionsListBox.SelectedIndex != -1)
            {
                CurrentAction.Do(CurrentGameplay, 100);
                UpdateButtons();
            }
        }

        private void ActionReverseDoButton_Click(object sender, EventArgs e)
        {
            if (CurrentGameplay != null && ActionsListBox.SelectedIndex != -1)
            {
                CurrentAction.ReverseDo(CurrentGameplay, 1);
                UpdateButtons();
            }
        }

        private void ActionX10ReverseDoButton_Click(object sender, EventArgs e)
        {
            if (CurrentGameplay != null && ActionsListBox.SelectedIndex != -1)
            {
                CurrentAction.ReverseDo(CurrentGameplay, 10);
                UpdateButtons();
            }
        }

        private void ActionX100ReverseDoButton_Click(object sender, EventArgs e)
        {
            if (CurrentGameplay != null && ActionsListBox.SelectedIndex != -1)
            {
                CurrentAction.ReverseDo(CurrentGameplay, 100);
                UpdateButtons();
            }
        }

        private void BuildingBuildButton_Click(object sender, EventArgs e)
        {
            if (CurrentGameplay != null && BuildingListBox.SelectedIndex != -1)
            {
                Building.Buildings[current_building].OnBuilding(CurrentGameplay);
                CurrentGameplay.Buildings[current_building] += 1;
                UpdateButtons();
            }
        }

        private void ActionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentGameplay != null && ActionsListBox.SelectedIndex != -1)
            {
                CurrentAction = Action.Actions[ActionsListBox.SelectedIndex];
                UpdateAction();
                UpdateButtons();
            }
        }

        private void SetX1Speed_Click(object sender, EventArgs e)
        {
            Ticker.Interval = 500;
        }

        private void SetX2Speed_Click(object sender, EventArgs e)
        {
            Ticker.Interval = 250;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CurrentGameplay != null)
            {
                CurrentGameplay.Save();
            }
        }

        private void NewGameSMI_Click(object sender, EventArgs e)
        {
            CurrentGameplay = new Gameplay().Init();
        }

        private void SetX3Speed_Click(object sender, EventArgs e)
        {
            Ticker.Interval = 125;
        }

        public void UpdateAction()
        {
            if (CurrentGameplay != null && ActionsListBox.SelectedIndex != -1)
            {
                ActionDescription.Text = CurrentAction.Description;
                ActionCost.MakeResources(CurrentAction.ResourceAction);
                ActionBuildings.MakeBuildings(CurrentAction.BuildingAction);
            }
        }

        public void UpdateBuilding()
        {
            if (CurrentGameplay != null && BuildingListBox.SelectedIndex != -1)
            {
                BuildingDescription.Text = Building.Buildings[current_building].Description;
                BuildingCost.MakeResources(Building.Buildings[current_building].Cost);
                BuildingProduction.MakeResources(Building.Buildings[current_building].GetProduction(CurrentGameplay));
            }
        }
    }
}
