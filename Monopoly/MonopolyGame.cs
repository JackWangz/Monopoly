using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Threading;

namespace Monopoly
{
    class MonopolyGame
    {
        //controls
        Button btnStart, btnDice;
        Label[] label = new Label[22];
        Label lbl_P1, lbl_P2;
        PictureBox picbox_P1, picbox_P2;

        Dictionary<int, Point> Map = new Dictionary<int, Point>();
        List<Site> Datas = new List<Site>();
        Player P1, P2, PlayerTurn; 

        public MonopolyGame()
        {
            Initial();
            LoadSiteData();
        }

        private void Transaction(Player player, int index)
        {
            Site house = Datas.Find(x => x.No == index);
            int priceToBuy = (int)(house.Price * 0.2); //購入價為原價 * 0.2

            //判斷該房子被賣出了沒
            if (house.Owned == null)
            {
                DialogResult result =
                    MessageBox.Show(string.Format("{0}\r\n收購價 {1}元\r\n確定要購買？", house.Name, priceToBuy), "購入房屋", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    if (player.Cash >= priceToBuy)
                    {
                        house.Owned = player;
                        player.Cash -= priceToBuy;
                        label[house.No - 1].ForeColor = player.Color;
                    }
                    else
                        MessageBox.Show("$$不夠，想出老千？");
                }
            }
            else
            {
                //若房屋擁有者為玩家，則詢問要不要升級
                if (house.Owned == player)
                {
                    LevelUp(player, house);
                }
                //否則付過路費
                else
                {
                    MessageBox.Show(string.Format("房子為{0}所有\r\n請支付 {1} 元過路費。", house.Owned.Name, house.Price));

                    //錢不夠則 Game Over
                    if (player.Cash < house.Price)
                        GameOver(player);
                    else
                    {
                        player.Cash -= house.Price;
                        house.Owned.Cash += house.Price;
                    }
                }
            }

        }

        private void LevelUp(Player player, Site house)
        {
            if(house.Level == 5)
            {
                MessageBox.Show(string.Format("{0}已經是最高等級！", house.Name));
                return;
            }

            int fee = house.Level * 1000;
            DialogResult result =
                MessageBox.Show(string.Format("請問你要升級{0}嗎？\r\n費用為{1}", house.Name, fee), "房屋升級", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                if (fee > player.Cash)
                {
                    MessageBox.Show("現金不足，無法升級！", "錢不夠還敢撒野");
                    return;
                }
                else
                {
                    house.Level += 1;
                    house.Price += 3000;
                }
            }
        }

        private void Move(Player player, int numbertomove)
        {
            string siteType;
            int nextPosition = (player.PositionIndex + numbertomove) % 22 == 0
               ? nextPosition = 22 
               : (player.PositionIndex + numbertomove) % 22;

            player.Position = Map[nextPosition];
            player.PositionIndex = nextPosition;
            siteType = Datas.Find(x => x.No == player.PositionIndex).Type;  //取得某一玩家現在位置上的Site類型

            if (player.Name == "P1")
            {
                picbox_P1.Location = player.Position;

                //觸發購買，付款或其他事件
                if (siteType == "House")
                {
                    Transaction(player, nextPosition);
                }
                else
                {
                    //...機會命運
                }

                PlayerTurn = P2; //遊戲權換P2
            }
            else if (player.Name == "P2")
            {
                picbox_P2.Location = player.Position;

                //觸發購買，付款或其他事件
                if (siteType == "House")
                {
                    Transaction(player, nextPosition);
                }
                else
                {
                    //...機會命運
                }

                PlayerTurn = P1; //遊戲權換P1
            }

            Update();
        }

        private void GameOver(Player loser)
        {
            MessageBox.Show(string.Format("輸家為{0}", loser.Name), "遊戲結束");
            btnDice.Enabled = false;

            DialogResult result =
                MessageBox.Show("在玩一局？", "Monopoly", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes) Initial();
            else Application.Exit();
        }

        private void Update()
        {
            //Update label_Player
            lbl_P1.Text = "P1\r\n" + "現金: " + P1.Cash + "\r\n" + "擁有房屋: ";
            lbl_P2.Text = "P2\r\n" + "現金: " + P2.Cash + "\r\n" + "擁有房屋: ";

            //Update all house's data.
            for (int i = 0; i < Datas.Count; i++)
            {
                if (Datas.Find(x => x.No == i + 1).Type == "House")
                {
                    label[i].Text = Datas.Find(x => x.No == i + 1).Name + "\r\n" +
                                    "Level: " + Datas.Find(x => x.No == i + 1).Level + "\r\n" +
                                    "Price: " + Datas.Find(x => x.No == i + 1).Price + "\r\n";
                }
            }
        }

        private void Initial()
        {
            SetSiteDatas();
            int initialCash = 50000;


            //button
            btnStart = new Button();
            btnStart.Location = new Point(0, 0);
            btnStart.Size = new Size(50, 50);
            btnStart.Text = "開始";
            btnStart.Click += btnStart_Click;

            btnDice = new Button();
            btnDice.Enabled = true;
            btnDice.Location = new Point(200, 180);
            btnDice.Size = new Size(75, 75);
            btnDice.Text = "擲骰子";
            btnDice.Click += btnDice_Click;

            Form1._Form1.Controls.AddRange(new Button[]{btnDice});
            //Form1._Form1.Controls.AddRange(new Button[] { btnStart, btnDice });

            //player
            P1 = new Player();
            P1.Name = "P1";
            P1.Color = Color.Red;
            P1.Position = Map[1];
            P1.PositionIndex = 1;
            P1.Cash = initialCash;

            P2 = new Player();
            P2.Name = "P2";
            P2.Color = Color.Blue;
            P2.Position = new Point(Map[1].X + 15, Map[2].Y);
            P2.PositionIndex = 1;
            P2.Cash = initialCash;

            PlayerTurn = P1;

            //picturebox. Show the location of players.
            picbox_P1 = new PictureBox();
            picbox_P1.Location = P1.Position;
            picbox_P1.Size = new Size(10, 10);
            picbox_P1.BackColor = P1.Color;

            picbox_P2 = new PictureBox();
            picbox_P2.Location = P2.Position;
            picbox_P2.Size = new Size(10, 10);
            picbox_P2.BackColor = P2.Color;

            Form1._Form1.Controls.AddRange(new PictureBox[] { picbox_P1, picbox_P2 });

            //label
            //show players' data
            lbl_P1 = new Label();
            lbl_P1.Size = new Size(200, 100);
            lbl_P1.BorderStyle = BorderStyle.FixedSingle;
            lbl_P1.Location = new Point(75, 400);
            lbl_P1.Text = "P1\r\n" + "現金: " + P1.Cash + "\r\n" + "擁有房屋: ";

            lbl_P2 = new Label();
            lbl_P2.Size = new Size(200, 100);
            lbl_P2.BorderStyle = BorderStyle.FixedSingle;
            lbl_P2.Location = new Point(275, 400);
            lbl_P2.Text = "P2\r\n" + "現金: " + P2.Cash + "\r\n" + "擁有房屋: ";

            Form1._Form1.Controls.AddRange(new Label[] { lbl_P1, lbl_P2 });
        }

        private void SetSiteDatas()
        {
            // House
            Datas.Add(new Site() { Type = "Others", No = 1, Name = "起點", Position = new Point(75, 75) });
            Datas.Add(new Site() { Type = "House", No = 2, Name = "新光三越", Price = 5000, Level = 1, Position = new Point(75 * 2, 75) });
            Datas.Add(new Site() { Type = "House", No = 3, Name = "帝寶", Price = 9000, Level = 1, Position = new Point(75 * 3, 75) });
            Datas.Add(new Site() { Type = "House", No = 4, Name = "涵碧樓", Price = 7500, Level = 1, Position = new Point(75 * 4, 75) });
            Datas.Add(new Site() { Type = "House", No = 5, Name = "故宮", Price = 3000, Level = 1, Position = new Point(75 * 5, 75) });
            Datas.Add(new Site() { Type = "House", No = 6, Name = "SOGO百貨", Price = 4500, Level = 1, Position = new Point(75 * 6, 75) });
            Datas.Add(new Site() { Type = "House", No = 7, Name = "NTUST", Price = 2500, Level = 1, Position = new Point(75 * 7, 75) });
            Datas.Add(new Site() { Type = "House", No = 8, Name = "台電大樓", Price = 6000, Level = 1, Position = new Point(75 * 8, 75) });
            Datas.Add(new Site() { Type = "House", No = 9, Name = "中友百貨", Price = 5000, Level = 1, Position = new Point(75 * 9, 75) });
            Datas.Add(new Site() { Type = "Others", No = 10, Name = "機會", Position = new Point(75 * 9, 75 * 2) });
            Datas.Add(new Site() { Type = "House", No = 11, Name = "台北101", Price = 7500, Level = 1, Position = new Point(75 * 9, 75 * 3) });
            Datas.Add(new Site() { Type = "House", No = 12, Name = "西堤牛排", Price = 6700, Level = 1, Position = new Point(75 * 9, 75 * 4) });
            Datas.Add(new Site() { Type = "House", No = 13, Name = "小蒙牛火鍋店", Price = 5700, Level = 1, Position = new Point(75 * 8, 75 * 4) });
            Datas.Add(new Site() { Type = "House", No = 14, Name = "85度C", Price = 3400, Level = 1, Position = new Point(75 * 7, 75 * 4) });
            Datas.Add(new Site() { Type = "House", No = 15, Name = "星巴克", Price = 4700, Level = 1, Position = new Point(75 * 6, 75 * 4) });
            Datas.Add(new Site() { Type = "House", No = 16, Name = "台中牛排館", Price = 2800, Level = 1, Position = new Point(75 * 5, 75 * 4) });
            Datas.Add(new Site() { Type = "House", No = 17, Name = "勤美誠品", Price = 3800, Level = 1, Position = new Point(75 * 4, 75 * 4) });
            Datas.Add(new Site() { Type = "Others", No = 18, Name = "命運", Position = new Point(75 * 3, 75 * 4) });
            Datas.Add(new Site() { Type = "House", No = 19, Name = "太平洋百貨", Price = 7400, Level = 1, Position = new Point(75 * 2, 75 * 4) });
            Datas.Add(new Site() { Type = "House", No = 20, Name = "鬍鬚張", Price = 3300, Level = 1, Position = new Point(75, 75 * 4) });
            Datas.Add(new Site() { Type = "Others", No = 21, Name = "公園", Position = new Point(75, 75 * 3) });
            Datas.Add(new Site() { Type = "House", No = 22, Name = "Google Inc.", Price = 10000, Level = 1, Position = new Point(75, 75 * 2) });

            // Map
            for (int i = 1; i <= 22; i++)
            {
                if (i < 10)
                    Map.Add(i, new Point(80 + 75 * (i - 1), 130));

                else if (i == 10 || i == 11)
                    Map.Add(i, new Point(80 + 75 * 8, 130 + 75 * ( i == 10 ? 1 : 2 )));

                else if (i >= 12 & i <= 20)
                    Map.Add(i, new Point(80 + 75 * (20 - i), 130 + 75 * 3 ));

                else
                    Map.Add(i, new Point(80, 130 + 75 * ( i == 21 ? 2 : 1 )));
            }

        }

        public void LoadSiteData()
        {
            const int side = 75;
            for (int i = 0; i < Datas.Count; i++)
            {
                label[i] = new Label();

                //House
                if (Datas.Find(x => x.No == i + 1).Type == "House") {
                    label[i].Text = Datas.Find(x => x.No == i + 1).Name + "\r\n" +
                                    "Level: " + Datas.Find(x => x.No == i + 1).Level + "\r\n" +
                                    "Price: " + Datas.Find(x => x.No == i + 1).Price + "\r\n";
                }
                else
                {   
                //Others
                    label[i].Text = Datas.Find(x => x.No == i + 1).Name + "\r\n";                
                }

                label[i].Size = new Size(side, side);
                label[i].BorderStyle = BorderStyle.FixedSingle;
                label[i].Location = Datas.Find(x => x.No == i + 1).Position;

                Form1._Form1.Controls.Add(label[i]);
            }
        }

        private void btnDice_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            int NumberToMove = r.Next(1, 7);
            btnDice.Text = NumberToMove.ToString();
            Move(PlayerTurn, NumberToMove);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }

    }
}
