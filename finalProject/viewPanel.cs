using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace finalProject
{
    public partial class viewPanel: Form
    {
        public Form1 MyParentForm1;

        String rating = "";
        string filename = "";
        string target = "";
        List<String> richText1 = new List<string>();
        List<String> richText2 = new List<string>();
        private List<string> read_genre;
        private List<string> read_actor;
        private Boolean newComment = false;


        public viewPanel()
        {
            InitializeComponent();

            
        }

        public void setLabel1(string str)
        {
            label1.Text = str;
            target = str;
        }
        public void setLabel2(string str)
        {
            label2.Text = str;
        }
        public void setLabel3(string str)
        {
            label3.Text = str;
        }
        public void setLabel4(string str)
        {
            label4.Text = str;
        }
        public void setLabel5(string str)
        {
            label5.Text = str;
        }
        public void setLabel6(string str)
        {
            rating = str;
        }
        public void setLabel7(List<String> str)
        {
            for (int i = 0; i < str.Count; i++)
            {
                listBox2.Items.Add(str[i]);
                richText1.Add(str[i]);
            }
        }
        public void setLabel8(List<String> str)
        {

            for (int i = 0; i <str.Count; i++)
            {
                listBox1.Items.Add(str[i]);
                richText2.Add(str[i]);

            }
        }

        public void getGenre(List<string> list)
        {
            this.read_genre = list;
        }

        public void getActor(List<string> list)
        {
            this.read_actor = list;
        }

        public void getFilename(string name)
        {
            this.filename = name;
        }

        public void getComment(string str)
        {
            richTextBox1.Text = str;
        }

        public void showRating(String value)
        {
            if (value.ToString() == "1")
            {
                pictureBox1.Image = imageList1.Images[0];
            }
            else if(value.ToString() == "2")
            {
                pictureBox1.Image = imageList1.Images[0];
                pictureBox2.Image = imageList1.Images[0];
            }
            else if (value.ToString() == "2")
            {
                pictureBox1.Image = imageList1.Images[0];
                pictureBox2.Image = imageList1.Images[0];
            }
            else if (value.ToString() == "3")
            {
                pictureBox1.Image = imageList1.Images[0];
                pictureBox2.Image = imageList1.Images[0];
                pictureBox3.Image = imageList1.Images[0];
            }
            else if (value.ToString() == "4")
            {
                pictureBox1.Image = imageList1.Images[0];
                pictureBox2.Image = imageList1.Images[0];
                pictureBox3.Image = imageList1.Images[0];
                pictureBox4.Image = imageList1.Images[0];
            }
            else if (value.ToString() == "5")
            {
                pictureBox1.Image = imageList1.Images[0];
                pictureBox2.Image = imageList1.Images[0];
                pictureBox3.Image = imageList1.Images[0];
                pictureBox4.Image = imageList1.Images[0];
                pictureBox5.Image = imageList1.Images[0];
            }
            else if (value.ToString() == "6")
            {
                pictureBox1.Image = imageList1.Images[0];
                pictureBox2.Image = imageList1.Images[0];
                pictureBox3.Image = imageList1.Images[0];
                pictureBox4.Image = imageList1.Images[0];
                pictureBox5.Image = imageList1.Images[0];
                pictureBox6.Image = imageList1.Images[0];
            }
            else if (value.ToString() == "7")
            {
                pictureBox1.Image = imageList1.Images[0];
                pictureBox2.Image = imageList1.Images[0];
                pictureBox3.Image = imageList1.Images[0];
                pictureBox4.Image = imageList1.Images[0];
                pictureBox5.Image = imageList1.Images[0];
                pictureBox6.Image = imageList1.Images[0];
                pictureBox7.Image = imageList1.Images[0];
            }
            else if (value.ToString() == "8")
            {
                pictureBox1.Image = imageList1.Images[0];
                pictureBox2.Image = imageList1.Images[0];
                pictureBox3.Image = imageList1.Images[0];
                pictureBox4.Image = imageList1.Images[0];
                pictureBox5.Image = imageList1.Images[0];
                pictureBox6.Image = imageList1.Images[0];
                pictureBox7.Image = imageList1.Images[0];
                pictureBox8.Image = imageList1.Images[0];
            }
            else if (value.ToString() == "9")
            {
                pictureBox1.Image = imageList1.Images[0];
                pictureBox2.Image = imageList1.Images[0];
                pictureBox3.Image = imageList1.Images[0];
                pictureBox4.Image = imageList1.Images[0];
                pictureBox5.Image = imageList1.Images[0];
                pictureBox6.Image = imageList1.Images[0];
                pictureBox7.Image = imageList1.Images[0];
                pictureBox8.Image = imageList1.Images[0];
                pictureBox9.Image = imageList1.Images[0];
            }
            else if (value.ToString() == "10")
            {
                pictureBox1.Image = imageList1.Images[0];
                pictureBox2.Image = imageList1.Images[0];
                pictureBox3.Image = imageList1.Images[0];
                pictureBox4.Image = imageList1.Images[0];
                pictureBox5.Image = imageList1.Images[0];
                pictureBox6.Image = imageList1.Images[0];
                pictureBox7.Image = imageList1.Images[0];
                pictureBox8.Image = imageList1.Images[0];
                pictureBox9.Image = imageList1.Images[0];
                pictureBox10.Image = imageList1.Images[0];
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result1 = MessageBox.Show("I guess you just clicked the play button, unfortunately I really dont have this function. It's not a requirement in my assignment.\nBut, I am trying to find something for you! \n\n\nCheers!!", "A message from author", MessageBoxButtons.YesNo);

            if (result1 == DialogResult.Yes)
            {
                XDocument doc = XDocument.Load(filename);
                String title = label1.Text;
                String url = "";
                title = title.Replace(" ", "+");
                url = "https://www.youtube.com/results?search_query=" + title;
                System.Diagnostics.Process.Start(url);

                var node = doc.Descendants("movie").FirstOrDefault(movie => movie.Element("title").Value == target);
                if (node.Element("history") != null)
                {
                    node.SetElementValue("history", DateTime.Now);
                }
                else
                {
                    node.Add(new XElement("history", DateTime.Now));
                }

                Console.WriteLine(node);
                doc.Save(filename);
            }




        }

        private void Edit_Click(object sender, EventArgs e)
        {

            Edit edit = new Edit { MyParentForm2 = this };

            edit.getFilename(filename);

            if(this.label3.Text.Contains(" min"))
            {
                string string1 = this.label3.Text;
                string string2 = " min";

                string string1_part1 = string1.Substring(0, string1.IndexOf(string2));
                string string1_part2 = string1.Substring(
                    string1.IndexOf(string2) + string2.Length, string1.Length - (string1.IndexOf(string2) + string2.Length));

                string1 = string1_part1 + string1_part2;

                edit.setTextBox2(string1);

            }
            else
            {
                edit.setTextBox2(this.label3.Text);

            }




            edit.setTextBox1(this.label1.Text);
            edit.setComboBox1(this.label2.Text);
            edit.setComboBox2(this.label4.Text);
            edit.setComboBox3(this.rating);
            edit.setTextBox3(this.label5.Text);
            edit.setRichTextBox1(this.richText1);
            edit.setRichTextBox2(this.richText2);
            edit.getGenre(read_genre);
            edit.getActor(read_actor);

            edit.ShowDialog();
        }



        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.newComment = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(newComment == true)
            {
                DialogResult result = MessageBox.Show("Do you want save this comment?","The Question",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);

                if(result ==  DialogResult.Yes)
                {
                    XDocument doc = XDocument.Load(filename);

                    var node = doc.Descendants("movie").FirstOrDefault(movie => movie.Element("title").Value == target);

                    if (node.Element("comment") != null)
                    {
                        node.SetElementValue("comment", richTextBox1.Text);
                    }
                    else
                    {
                        node.Add(new XElement("comment", richTextBox1.Text));
                    }

                    Console.WriteLine(node);
                    doc.Save(filename);
                    this.Close();
                }
                else if(result == DialogResult.No)
                {
                    this.Close();
                }

            }
            else
            {
                this.Close();
            }
        }
    }
}
