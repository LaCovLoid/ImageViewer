using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Image_Viewer {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            Form1_Resize(null, null);//위치 배정
        }

        private void button1_Click(object sender, EventArgs e) {
            folderBrowserDialog1.SelectedPath = null;
            folderBrowserDialog1.ShowDialog();
            //폴더 경로 가져옴
            if (!string.IsNullOrWhiteSpace(folderBrowserDialog1.SelectedPath)) {
                string Croot = @"C:\";
                string foldername = folderBrowserDialog1.SelectedPath;
                //string Hanyu = foldername.Split('\\')[0]; // 드라이브C루트를 전부 막음
                if (foldername == Croot) {
                    folderBrowserDialog1.SelectedPath = null;
                    MessageBox.Show("C드라이브(시스템 드라이브) 루트는 선택할 수 없습니다.");
                } else {
                    listBox1.Items.Clear();
                    label1.Text = foldername;
                    pictureBox1.Image = null;
                    pictureBox1.BackColor = Color.Transparent;
                    string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath);//파일들 가져옴
                    for (int i = 0; i < files.Length; ++i) {//목록 가져옴
                        string images = files[i];
                        if (Path.GetExtension(images) == ".jpg" || 
                            Path.GetExtension(images) == ".bmp" ||
                            Path.GetExtension(images) == ".png" ||
                            Path.GetExtension(images) == ".jpeg") {
                            string result = Path.GetFileName(images);
                            listBox1.Items.Add(result);
                        }
                    }
                    System.Windows.Forms.MessageBox.Show("파일 안의 이미지는 " + listBox1.Items.Count + "개 입니다", "Message");
                    listBox1.Visible = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) {//삭제
            string name = listBox1.GetItemText(listBox1.SelectedItem);
            string Fullname = label1.Text + "\\" + name;
            if (File.Exists(Fullname)) {
                pictureBox1.Image = null;  //이미지 삭제시 이미지 존재유무
                File.Delete(Fullname);
                listBox1.Items.Clear();//리스트 초기화
                string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath);//재설정
                for (int i = 0; i < files.Length; ++i) {//목록 가져옴
                    string images = files[i];
                    if (Path.GetExtension(images) == ".jpg" ||
                        Path.GetExtension(images) == ".bmp" ||
                        Path.GetExtension(images) == ".png" ||
                        Path.GetExtension(images) == ".jpeg") {
                        string result = Path.GetFileName(images);
                        listBox1.Items.Add(result);
                    }
                }
                pictureBox1.BackColor = Color.Transparent;
            } else MessageBox.Show("그런파일 없쟝.");//삭제실패
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {//리스트 선택시 발동돼는 함정카드
            try {
                pictureBox1.Image = null;
                string name = listBox1.GetItemText(listBox1.SelectedItem);
                string Fullname = label1.Text + "\\" + name;
                label2.Text = name;
                Image im = GetCopyImage(Fullname); //경로에서 이미지 추출

                pictureBox1.Image = im;
                pictureBox1.BackColor = Color.Black;
                label1.Location = new Point(label3.Width + button1.Width + label6.Width, ClientSize.Height - label1.Height - 5);//리스트 선택마다 경로와 이름위치 재설정
                label2.Location = new Point(ClientSize.Width / 2, ClientSize.Height - label1.Height - 5);
                label9.Location = new Point(ClientSize.Width / 2 + label2.Width, ClientSize.Height - label1.Height - 5);
                Int64 fileSize = new FileInfo(Fullname).Length;
                Double Filesized = Math.Round((Double)fileSize / 1024 / 1024, 2);
                label9.Text = Filesized + "MByte";
            } catch (Exception) {
                MessageBox.Show("이미지가 아닙니다.");
            }
        }
        private Image GetCopyImage(string path) {//비트맵 이미지 가져옴
            using (Image im = Image.FromFile(path)) {
                Bitmap bm = new Bitmap(im);
                return bm;

            }

        }
        //보이나 안보이나 >>>>>>>>>>>
        private void label3_Click(object sender, EventArgs e) {
            if (listBox1.Visible == true) {
                listBox1.Visible = false;
            } else listBox1.Visible = true; 
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            if (button1.Visible == true) {
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                listBox1.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
            } else {
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                button1.Visible = true;
                button2.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
                label9.Visible = true;
            }
        }
        //위치설정>>>>>>
        private void Form1_Resize(object sender, System.EventArgs e) {
            pictureBox1.Width = ClientSize.Width;
            pictureBox1.Height = ClientSize.Height;
            label3.Location = new Point(0, ClientSize.Height - label3.Height);
            listBox1.Location = new Point(0, ClientSize.Height - label3.Height - listBox1.Height);
            button1.Location = new Point(label3.Width, ClientSize.Height - label3.Height);
            button2.Location = new Point(ClientSize.Width - button2.Width, ClientSize.Height - button2.Height);
            label5.Location = new Point(ClientSize.Width - button2.Width - label5.Width, ClientSize.Height - button2.Height);
            label4.Location = new Point(0, ClientSize.Height - label3.Height);
            label1.Location = new Point(label3.Width + button1.Width + label6.Width, ClientSize.Height - label1.Height - 5);
            label2.Location = new Point( ClientSize.Width/2 , ClientSize.Height - label1.Height - 5);
            label6.Location = new Point(label3.Width + button1.Width, ClientSize.Height - label1.Height - 5);
            label7.Location = new Point(20, ClientSize.Height / 2 - label7.Height/2);
            label8.Location = new Point(ClientSize.Width - label8.Width - 20, ClientSize.Height / 2 - label8.Height/2);
            label9.Location = new Point(ClientSize.Width / 2 + label2.Width, ClientSize.Height - label1.Height - 5);
        }

        private void label1_Click(object sender, EventArgs e) {
        }

        private void label5_Click(object sender, EventArgs e) {//상세정보(폼2)띄우기
            Form2 detail = new Form2(label2.Text,label1.Text, Path.GetExtension(label1.Text+label2.Text));
            if (pictureBox1.Image != null) {
                detail.Show();
            } else MessageBox.Show("이미지가 없습니다");
        }

        private void label6_Click(object sender, EventArgs e) {
            if (label1.Text == "폴더 경로") {
                MessageBox.Show("경로가 잡히지 않았습니다.");
            } else {
                listBox1.Items.Clear();//리스트 초기화
                string[] files = Directory.GetFiles(label1.Text);//재설정
                for (int i = 0; i < files.Length; ++i) {//목록 가져옴
                    string images = files[i];
                    label2.Text = "";
                    if (Path.GetExtension(images) == ".jpg" ||
                        Path.GetExtension(images) == ".bmp" ||
                        Path.GetExtension(images) == ".png" ||
                        Path.GetExtension(images) == ".jpeg") {
                        string result = Path.GetFileName(images);
                        listBox1.Items.Add(result);
                    }
                }
            }
        }

        private void label7_Click(object sender, EventArgs e) {
            if (listBox1.SelectedIndex == 0) {
                MessageBox.Show("목록의 처음입니다."); 
            } else listBox1.SelectedIndex = listBox1.SelectedIndex - 1;
        }

        private void label8_Click(object sender, EventArgs e) {
            if (listBox1.SelectedIndex == listBox1.Items.Count-1) {
                MessageBox.Show("목록의 끝입니다.");
            } else listBox1.SelectedIndex = listBox1.SelectedIndex + 1;
        }

        private void label4_Click(object sender, EventArgs e) {

        }
    }
}
