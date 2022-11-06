using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VillageRTS
{
    public class ResourcePanel
    {
        public Panel Reference { get; private set; }

        public ResourcePanel(Panel reference) : base()
        {
            Reference = reference;
        }

        public List<Label> ResourceLabels { get; private set; } = new List<Label>();

        public int Columns { get; set; } = 1;

        public ResourcePanel SetColumns(int newc)
        {
            Columns = newc;
            return this;
        }

        public void Clear()
        {
            while (ResourceLabels.Count > 0)
            {
                Reference.Controls.Remove(ResourceLabels[0]);
                ResourceLabels.RemoveAt(0);
            }
        }

        public static Dictionary<Resource, (Color, (int, int))> UIRes = new Dictionary<Resource, (Color, (int, int))>();
        private static void copy_last(Resource res) => UIRes[res] = UIRes[UIRes.Keys.Last()];
        static ResourcePanel()
        {
            UIRes.Add(Resource.My_Power, (Color.DeepSkyBlue, (50, 200))); 
            UIRes.Add(Resource.My_HumansLimit, (Color.Red, (5, 20))); copy_last(Resource.Village_HumansLimit);
            UIRes.Add(Resource.My_Humans, (Color.Red, (10, 40))); copy_last(Resource.Village_Humans);
            UIRes.Add(Resource.My_Gold, (Color.Olive, (1000, 10000))); copy_last(Resource.Village_Gold);
            UIRes.Add(Resource.My_Wood, (Color.SaddleBrown, (400, 2000))); copy_last(Resource.Village_Wood);
            UIRes.Add(Resource.My_Stone, (Color.DimGray, (400, 2000))); copy_last(Resource.Village_Stone);
            UIRes.Add(Resource.My_Iron, (Color.Blue, (200, 1000))); copy_last(Resource.Village_Iron);
            UIRes.Add(Resource.My_Food, (Color.Green, (1000, 10000))); copy_last(Resource.Village_Food);
            UIRes.Add(Resource.My_Fuel, (Color.Magenta, (1000, 10000))); copy_last(Resource.Village_Fuel);
            UIRes.Add(Resource.My_Tools, (Color.DarkCyan, (50, 500))); copy_last(Resource.Village_Tools);
            UIRes.Add(Resource.My_Clothes, (Color.Crimson, (50, 500))); copy_last(Resource.Village_Clothes);
            UIRes.Add(Resource.My_Leather, (Color.Crimson, (50, 500))); copy_last(Resource.Village_Leather);
        }

        public static Color GetColor(Resource resource)
        {
            if (UIRes.ContainsKey(resource)) return UIRes[resource].Item1;
            return Color.Black;
        }

        public static Color GetCountColor(Resource resource, int count)
        {
            if (!UIRes.ContainsKey(resource)) return Color.White;
            var res = UIRes[resource].Item2;
            if (count < res.Item1) return Color.FromArgb(255, 192, 192);
            if (count < res.Item2) return Color.FromArgb(255, 225, 192);
            return Color.FromArgb(192, 225, 192);
        }

        public void MakeBuildings(IEnumerable<KeyValuePair<int, int>> buildings)
        {
            Clear();
            var i = 0;
            foreach (var x in buildings)
            {
                var label = new Label();
                label.AutoSize = false;
                label.Size = new System.Drawing.Size(200, 20);
                var left = 3 + (i % Columns) * 200;
                var right = 3 + 20 * (i / Columns);
                label.Location = new System.Drawing.Point(left, right);
                label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

                var s = $"{Building.Buildings[x.Key]}: +{x.Value}".Replace("_", " ").Replace("+-", "-");
                label.Text = s;
                label.ForeColor = Color.Black;
                label.TabIndex = i;
                ResourceLabels.Add(label);
                Reference.Controls.Add(label);
                i++;
            }
        }

        public void MakeResources(IEnumerable<KeyValuePair<Resource, int>> resources)
        {
            Clear();
            var i = 0;
            foreach (var x in resources)
            {
                var label = new Label();
                label.AutoSize = false;
                label.Size = new System.Drawing.Size(200, 20);
                var left = 3 + (i % Columns) * 200;
                var right = 3 + 20 * (i / Columns);
                label.Location = new System.Drawing.Point(left, right);
                label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

                var s = $"{Program.Text[x.Key]}: +{x.Value}".Replace("_", " ").Replace("+-", "-");
                label.Text = s;
                label.ForeColor = GetColor(x.Key);
                label.TabIndex = i;
                ResourceLabels.Add(label);
                Reference.Controls.Add(label);
                i++;
            }
        }

        public void MakeResources(IEnumerable<KeyValuePair<Resource, (int, int)>> resources)
        {
            Clear();
            var i = 0;
            foreach (var x in resources)
            {
                var label = new Label();
                label.AutoSize = false;
                label.Size = new System.Drawing.Size(200, 20);
                var left = 3 + (i % Columns) * 200;
                var right = 3 + 20 * (i / Columns);
                label.Location = new System.Drawing.Point(left, right);
                label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

                var s = $"{Program.Text[x.Key]}: +{x.Value.Item2}".Replace("_", " ").Replace("+-", "-");
                if (x.Value.Item1 > 1) s += "  /" + x.Value.Item1;
                label.Text = s;
                label.ForeColor = GetColor(x.Key);
                label.TabIndex = i;
                ResourceLabels.Add(label);
                Reference.Controls.Add(label);
                i++;
            }
        }
    }
}
