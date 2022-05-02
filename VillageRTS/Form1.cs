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
            BuildingCost = new ResourcePanel(BuildingCostPanel).SetColumns(3);
            BuildingProduction = new ResourcePanel(BuildingProductionPanel).SetColumns(3);
            ActionCost = new ResourcePanel(ActionPanel).SetColumns(3);
            ActionBuildings = new ResourcePanel(ActionBuildingsPanel).SetColumns(3);
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

            StartNewGame(new Gameplay().Init().TryLoad());
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
                BuildingListBox.Items.AddRange(CurrentGameplay.Buildings.Keys.Select(x => Building.Buildings[x].Name.Replace("_", " ")).ToArray());

                ActionsListBox.Items.Clear();
                ActionsListBox.Items.AddRange(Array.ConvertAll(Action.Actions, x => x.GetType().Name.Replace("_", " ")));
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

        public void Fill(TextBox tb, Resource resource)
        {
            tb.Text = CurrentGameplay.Current[resource].ToString();
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
            Ticker.Interval = 166;
        }

        public void UpdateAction()
        {
            if (CurrentGameplay != null && ActionsListBox.SelectedIndex != -1)
            {
                ActionCost.MakeResources(CurrentAction.ResourceAction);
                ActionBuildings.MakeBuildings(CurrentAction.BuildingAction);
            }
        }

        public void UpdateBuilding()
        {
            if (CurrentGameplay != null && BuildingListBox.SelectedIndex != -1)
            {
                BuildingCost.MakeResources(Building.Buildings[current_building].Cost);
                BuildingProduction.MakeResources(Building.Buildings[current_building].GetProduction(CurrentGameplay));
            }
        }
    }
}
