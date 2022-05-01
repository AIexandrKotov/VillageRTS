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

            HumansHumansLimitLabel.ForeColor = ResourcePanel.GetColor(Resource.Humans_HumansLimit);
            HumansHumansLabel.ForeColor = ResourcePanel.GetColor(Resource.Humans_Humans);
            HumansGoldLabel.ForeColor = ResourcePanel.GetColor(Resource.Humans_Gold);
            HumansWoodLabel.ForeColor = ResourcePanel.GetColor(Resource.Humans_Wood);
            HumansStoneLabel.ForeColor = ResourcePanel.GetColor(Resource.Humans_Stone);
            HumansIronLabel.ForeColor = ResourcePanel.GetColor(Resource.Humans_Iron);
            HumansFoodLabel.ForeColor = ResourcePanel.GetColor(Resource.Humans_Food);
            HumansFuelLabel.ForeColor = ResourcePanel.GetColor(Resource.Humans_Fuel);
            HumansToolsLabel.ForeColor = ResourcePanel.GetColor(Resource.Humans_Tools);
            HumansClothesLabel.ForeColor = ResourcePanel.GetColor(Resource.Humans_Clothes);

            StartNewGame(new Gameplay().Init());
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

                Fill(HumansHumansLimitCount, Resource.Humans_HumansLimit);
                Fill(HumansHumansCount, Resource.Humans_Humans);
                Fill(HumansGoldCount, Resource.Humans_Gold);
                Fill(HumansWoodCount, Resource.Humans_Wood);
                Fill(HumansStoneCount, Resource.Humans_Stone);
                Fill(HumansIronCount, Resource.Humans_Iron);
                Fill(HumansFoodCount, Resource.Humans_Food);
                Fill(HumansFuelCount, Resource.Humans_Fuel);
                Fill(HumansToolsCount, Resource.Humans_Tools);
                Fill(HumansClothesCount, Resource.Humans_Clothes);

                UpdateButtons();
            }
        }

        private Action CurrentAction;
        private int current_building;
        private ResourcePanel BuildingCost, BuildingProduction, ActionCost;
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
            Ticker.Interval = 1000;
        }

        private void SetX2Speed_Click(object sender, EventArgs e)
        {
            Ticker.Interval = 500;
        }

        private void SetX3Speed_Click(object sender, EventArgs e)
        {
            Ticker.Interval = 333;
        }

        public void UpdateAction()
        {
            if (CurrentGameplay != null && ActionsListBox.SelectedIndex != -1)
            {
                ActionCost.MakeResources(CurrentAction.ResourceAction);
            }
        }

        public void UpdateBuilding()
        {
            if (CurrentGameplay != null && BuildingListBox.SelectedIndex != -1)
            {
                BuildingCost.MakeResources(Building.Buildings[current_building].Cost);
                BuildingProduction.MakeResources(Building.Buildings[current_building].Production);
            }
        }
    }
}
