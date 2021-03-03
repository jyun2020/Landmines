using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Landmines
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<int> LandminesList = new List<int>(); //存放炸彈編號
        List<Landmines> LandminesBt = new List<Landmines>(); //存放按鈕編號
        Random random = new Random(); //新增亂數物件
        int Row; //列數
        int Col; //行數
        int LandminesAmount; //炸彈數
        private string mark = System.Environment.CurrentDirectory + "\\mark.png"; //炸彈圖片路徑
        private string question = System.Environment.CurrentDirectory + "\\question.png"; //標記圖片路徑
        private void Form1_Load(object sender, EventArgs e)
        {   //開啟時加入item進combobox
            RankChange.Items.Add("簡單");
            RankChange.Items.Add("中等");
            RankChange.Items.Add("困難");
        }
        private void RankChange_SelectedIndexChanged(object sender, EventArgs e)
        {   //選擇時設定行、列、炸彈數，並呼叫NewGame布局
            if (RankChange.SelectedItem == "簡單")
            {
                Row = 10;
                Col = 8;
                LandminesAmount = 10;
                label2.Text = LandminesAmount.ToString();
                NewGame(Row, Col, LandminesAmount);
            }
            if (RankChange.SelectedItem == "中等")
            {
                Row = 18;
                Col = 14;
                LandminesAmount = 40;
                label2.Text = LandminesAmount.ToString();
                NewGame(Row, Col, LandminesAmount);
            }
            if (RankChange.SelectedItem == "困難")
            {
                Row = 30;
                Col = 14;
                LandminesAmount = 99;
                label2.Text = LandminesAmount.ToString();
                NewGame(Row, Col, LandminesAmount);
            }
        }
        private void NewGame(int Row, int Col, int landminesAmount)
        {
            this.splitContainer1.Panel2.Controls.Clear();
            LandminesBt.Clear();
            LandminesList.Clear();//換難度時要清除變數和布局
            LandminesProduce();//產生炸彈編號方法
            AddLandminesBt();//新增按鈕並布置炸彈方法
            TextSetup();//判斷周圍炸彈數方法
        }
        private void landminesBt_Click(object sender, MouseEventArgs e)
        {
            Landmines landmines = (Landmines)sender;//把傳入的Landmines用landmines接住
            if (e.Button == MouseButtons.Left)//如果點擊左鍵
            {
                landmines.BackgroundImage = null;//先把圖片清除,因為剛剛可能點過右鍵
                if (landmines.Text == "")//如果周圍都沒有炸彈
                {
                    OpenJudge(landmines.x, landmines.y);//開啟一整片按鈕的判斷方法(忍術遞迴術!)
                    WinJudge();//勝利判斷
                }
                else if (landmines.Text == "X")//如果踩到炸彈把所有按鈕顯示並無法點擊
                {
                    MessageBox.Show("你已經死了");
                    for (int k = 0; k < LandminesBt.Count; k++)
                    {
                        LandminesBt[k].BackColor = Color.White;
                        LandminesBt[k].Enabled = false;
                    }
                }
                else
                {   //單純按到數字,改為顯示並無法點擊
                    landmines.BackColor = Color.White;
                    landmines.Enabled = false;
                    WinJudge();
                }
            }
            if (e.Button == MouseButtons.Right)//如果點擊右鍵
            {
                if (Convert.ToInt32(landmines.Tag) == 0)//點擊第一次
                {
                    landmines.BackgroundImage = Image.FromFile(mark);//標記炸彈
                    landmines.Tag = 1;//改變TAG值
                }
                else if (Convert.ToInt32(landmines.Tag) == 1)//點擊第二次
                {
                    landmines.BackgroundImage = Image.FromFile(question);//標記旗子
                    landmines.Tag = 2;//改變TAG值
                }
                else//點擊第三次
                {
                    landmines.BackgroundImage = null;//移除圖片
                    landmines.Tag = 0;//重設TAG值
                }
            }
        }
        private void LandminesProduce()
        {
            for (int i = 0; i < LandminesAmount; i++)
            {
                int r = random.Next(0, Row * Col);
                if (LandminesList.Contains(r))  //如果產生到重複的數
                {
                    i--;  //i--相當於這次沒跑回圈
                }
                else
                {
                    LandminesList.Add(r);
                }
            }
            int LandminesNumber = 0;
        }
        private void AddLandminesBt()
        {
            int LandminesNumber = 0; //按鈕編號,用來判斷是否等於炸彈編號
            for (int y = 0; y < Col; y++)
            {
                for (int x = 0; x < Row; x++)
                {
                    Landmines landmines = new Landmines(x, y); //建立landmines物件(landmines繼承於button),功能和button一樣,但有補充設定屬性,可減少主程式code
                    for (int i = 0; i < LandminesList.Count; i++)//如果按鈕編號=炸彈編號,就設定為炸彈
                    {
                        if (LandminesNumber == LandminesList[i])
                        {
                            landmines.Text = "X";
                        }
                    }
                    landmines.MouseDown += new MouseEventHandler(landminesBt_Click);//共用點擊事件
                    this.splitContainer1.Panel2.Controls.Add(landmines);//加入視窗
                    LandminesBt.Add(landmines);//加入串列,相當於給每個button編碼,方便等等判斷用
                    LandminesNumber++;//按鈕編號 +1
                }
            }
        }
        private void TextSetup()
        {
            for (int i = 0; i < LandminesBt.Count; i++)
            {
                if (LandminesBt[i].Text != "X")//不是炸彈才進去
                {
                    int TextNumber = 0;//計算炸彈數輛變數
                    if (LandminesBt[i].x - 1 < 0 && LandminesBt[i].y - 1 < 0)//左上角
                    {
                        if (LandminesBt[i + 1].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i + Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i + 1 + Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (TextNumber > 0)
                        {
                            LandminesBt[i].Text = TextNumber.ToString();//判斷完之後轉成字串設定text
                        }
                        else
                        {
                            LandminesBt[i].Text = null;//如果都沒有炸彈設定為null
                        }
                    }
                    else if (LandminesBt[i].x + 1 == Row && LandminesBt[i].y - 1 < 0)//右上角
                    {
                        if (LandminesBt[i - 1].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i + Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - 1 + Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (TextNumber > 0)
                        {
                            LandminesBt[i].Text = TextNumber.ToString();
                        }
                        else
                        {
                            LandminesBt[i].Text = null;
                        }
                    }
                    else if (LandminesBt[i].x - 1 < 0 && LandminesBt[i].y + 1 == Col)//左下角
                    {
                        if (LandminesBt[i + 1].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i + 1 - Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (TextNumber > 0)
                        {
                            LandminesBt[i].Text = TextNumber.ToString();
                        }
                        else
                        {
                            LandminesBt[i].Text = null;
                        }
                    }
                    else if (LandminesBt[i].x + 1 == Row && LandminesBt[i].y + 1 == Col)//右下角
                    {
                        if (LandminesBt[i - 1].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - 1 - Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (TextNumber > 0)
                        {
                            LandminesBt[i].Text = TextNumber.ToString();
                        }
                        else
                        {
                            LandminesBt[i].Text = null;
                        }
                    }
                    else if (LandminesBt[i].y - 1 < 0 && LandminesBt[i].x > 0 && LandminesBt[i].x < Row - 1)//上(不包含角)
                    {
                        if (LandminesBt[i + 1].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - 1].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i + Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i + 1 + Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - 1 + Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (TextNumber > 0)
                        {
                            LandminesBt[i].Text = TextNumber.ToString();
                        }
                        else
                        {
                            LandminesBt[i].Text = null;
                        }
                    }
                    else if (LandminesBt[i].x + 1 == Row && LandminesBt[i].y > 0 && LandminesBt[i].y < Col - 1)//右(不包含角)
                    {
                        if (LandminesBt[i - 1].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i + Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - 1 + Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - 1 - Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (TextNumber > 0)
                        {
                            LandminesBt[i].Text = TextNumber.ToString();
                        }
                        else
                        {
                            LandminesBt[i].Text = null;
                        }
                    }
                    else if (LandminesBt[i].y + 1 == Col && LandminesBt[i].x > 0 && LandminesBt[i].x < Row - 1)//下(不包含角)
                    {
                        if (LandminesBt[i + 1].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - 1].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - 1 - Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i + 1 - Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (TextNumber > 0)
                        {
                            LandminesBt[i].Text = TextNumber.ToString();
                        }
                        else
                        {
                            LandminesBt[i].Text = null;
                        }
                    }
                    else if (LandminesBt[i].x - 1 < 0 && LandminesBt[i].y > 0 && LandminesBt[i].y < Col - 1)//左(不包含角)
                    {
                        if (LandminesBt[i + 1].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i + Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i + 1 - Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - 1 + Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (TextNumber > 0)
                        {
                            LandminesBt[i].Text = TextNumber.ToString();
                        }
                        else
                        {
                            LandminesBt[i].Text = null;
                        }
                    }
                    else
                    {
                        if (LandminesBt[i + 1].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - 1].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i + Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i + 1 - Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - 1 - Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i + 1 + Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (LandminesBt[i - 1 + Row].Text == "X")
                        {
                            TextNumber += 1;
                        }
                        if (TextNumber > 0)
                        {
                            LandminesBt[i].Text = TextNumber.ToString();
                        }
                        else
                        {
                            LandminesBt[i].Text = null;
                        }
                    }
                }
            }
        }
        private void OpenJudge(int x, int y)
        {
            int i = x + y * Row; //編號=X+Yx行數
            if (LandminesBt[i].Text == "")//先把按下的那顆改變狀態
            {
                LandminesBt[i].Enabled = false;
                LandminesBt[i].BackColor = Color.White;
            }
            //開始遞迴判斷,判斷順序為上、右、下、左,如果判斷遇到無雷,就在呼叫一次自己,並從上右下左順序開始判斷..重複循環

            if (y - 1 >= 0)//往上判斷
            {
                if (LandminesBt[x + (y - 1) * Row].Text == "" && LandminesBt[x + (y - 1) * Row].Enabled == true && Convert.ToInt32(LandminesBt[x + (y - 1) * Row].Tag) == 0 )
                {
                    LandminesBt[x + (y - 1) * Row].Enabled = false;
                    LandminesBt[x + (y - 1) * Row].BackColor = Color.White;
                    OpenJudge(x, y - 1);
                }
                //如果遇到有數字的button就把他開啟,這時就不用再呼叫自己
                if (LandminesBt[x + (y - 1) * Row].Text != "" && LandminesBt[x + (y - 1) * Row].Enabled == true)
                {
                    LandminesBt[x + (y - 1) * Row].Enabled = false;
                    LandminesBt[x + (y - 1) * Row].BackColor = Color.White;
                }
            }
            if (x - 1 >= 0)//往右判斷
            {
                if (LandminesBt[x - 1 + y * Row].Text == "" && LandminesBt[x - 1 + y * Row].Enabled == true && Convert.ToInt32(LandminesBt[x-1 + y* Row].Tag) == 0)
                {
                    LandminesBt[x - 1 + y * Row].Enabled = false;
                    LandminesBt[x - 1 + y * Row].BackColor = Color.White;
                    OpenJudge(x - 1, y);
                }
                if (LandminesBt[x - 1 + y * Row].Text != "" && LandminesBt[x - 1 + y * Row].Enabled == true)
                {
                    LandminesBt[x - 1 + y * Row].Enabled = false;
                    LandminesBt[x - 1 + y * Row].BackColor = Color.White;
                }
            }
            if (y + 1 < Col)//往下判斷
            {
                if (LandminesBt[x + (y + 1) * Row].Text == "" && LandminesBt[x + (y + 1) * Row].Enabled == true && Convert.ToInt32(LandminesBt[x + (y + 1) * Row].Tag) == 0)
                {
                    LandminesBt[x + (y + 1) * Row].Enabled = false;
                    LandminesBt[x + (y + 1) * Row].BackColor = Color.White;
                    OpenJudge(x, y + 1);
                }
                if (LandminesBt[x + (y + 1) * Row].Text != "" && LandminesBt[x + (y + 1) * Row].Enabled == true)
                {
                    LandminesBt[x + (y + 1) * Row].Enabled = false;
                    LandminesBt[x + (y + 1) * Row].BackColor = Color.White;
                }
            }
            if (x + 1 < Row)//往左判斷
            {
                if (LandminesBt[x + 1 + y * Row].Text == "" && LandminesBt[x + 1 + y * Row].Enabled == true && Convert.ToInt32(LandminesBt[(x+1) + y* Row].Tag) == 0)
                {
                    LandminesBt[x + 1 + y * Row].Enabled = false;
                    LandminesBt[x + 1 + y * Row].BackColor = Color.White;
                    OpenJudge(x + 1, y);
                }
                if (LandminesBt[x + 1 + y * Row].Text != "" && LandminesBt[x + 1 + y * Row].Enabled == true)
                {
                    LandminesBt[x + 1 + y * Row].Enabled = false;
                    LandminesBt[x + 1 + y * Row].BackColor = Color.White;
                }
            }
        }
        private void WinJudge()
        {
            int j = 0;
            for (int i = 0; i < LandminesBt.Count; i++)
            {
                if (LandminesBt[i].Enabled == false)
                {
                    j += 1;//找到已經按過的按鈕就+1
                }
                if (j == LandminesBt.Count - LandminesAmount)//勝利判斷為按過的按紐=總隔數-炸彈數
                {
                    MessageBox.Show("您真牛逼!");
                    for (int k = 0; k < LandminesBt.Count; k++)
                    {
                        LandminesBt[k].BackColor = Color.White;
                        LandminesBt[k].Enabled = false;
                    }
                }
            }
        }
    }
}
