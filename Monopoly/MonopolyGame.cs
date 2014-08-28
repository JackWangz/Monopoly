using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace Monopoly
{
    class MonopolyGame
    {
        List<Site> Datas = new List<Site>();

        public MonopolyGame()
        {
            SetSiteDatas();
            LoadSiteData();
        }

        public void SetSiteDatas()
        {
            // 蓋房子
            Datas.Add(new Site() { Type = "Others", No = 1, Name = "起點", Position = new Point(75, 75) });
            Datas.Add(new Site() { Type = "House", No = 2, Name = "新光三越", Price = 5000, Level = 1, Position = new Point(75 * 2, 75) });
            Datas.Add(new Site() { Type = "House", No = 3, Name = "帝寶", Price = 9000, Level = 1, Position = new Point(75 * 3, 75) });
            Datas.Add(new Site() { Type = "House", No = 4, Name = "涵碧樓", Price = 7500, Level = 1, Position = new Point(75 * 4, 75) });
            Datas.Add(new Site() { Type = "House", No = 5, Name = "故宮", Price = 3000, Level = 1, Position = new Point(75 * 5, 75) });
            Datas.Add(new Site() { Type = "House", No = 6, Name = "SOGO百貨", Price = 4500, Level = 1, Position = new Point(75 * 6, 75) });
            Datas.Add(new Site() { Type = "House", No = 7, Name = "台灣科技大學", Price = 2000, Level = 1, Position = new Point(75 * 7, 75) });
            Datas.Add(new Site() { Type = "House", No = 8, Name = "台電大樓", Price = 6000, Level = 1, Position = new Point(75 * 8, 75) });
            Datas.Add(new Site() { Type = "House", No = 9, Name = "中友百貨", Price = 5000, Level = 1, Position = new Point(75 * 9, 75) });
            Datas.Add(new Site() { Type = "House", No = 10, Name = "勤美誠品", Price = 4800, Level = 1, Position = new Point(75 * 9, 75 * 2) });
            Datas.Add(new Site() { Type = "House", No = 11, Name = "台北101", Price = 7500, Level = 1, Position = new Point(75 * 9, 75 * 3) });


        }

        public void LoadSiteData()
        {
            //Form1.CheckForIllegalCrossThreadCalls = false;
            const int side = 75;
            Label[] label = new Label[22];
            for (int i = 0; i < Datas.Count; i++)
            {
                //House
                if (Datas.Find(x => x.No == i + 1).Type == "House") {
                    label[i] = new Label();
                    label[i].Size = new Size(side, side);
                    label[i].BorderStyle = BorderStyle.FixedSingle;
                    label[i].Location = Datas.Find(x => x.No == i + 1).Position;
                    label[i].Text = Datas.Find(x => x.No == i + 1).Name + "\r\n" +
                                    "Level: " + Datas.Find(x => x.No == i + 1).Level + "\r\n" +
                                    "Price: " + Datas.Find(x => x.No == i + 1).Price;
                }
                else
                {   
                    //Others
                    label[i] = new Label();
                    label[i].Size = new Size(side, side);
                    label[i].BorderStyle = BorderStyle.FixedSingle;
                    label[i].Location = Datas.Find(x => x.No == i + 1).Position;
                    label[i].Text = Datas.Find(x => x.No == i + 1).Name + "\r\n";                
                }
                Form1._Form1.Controls.Add(label[i]);
            }
        }
    }
}
