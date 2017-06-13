using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace finalProject
{
    public partial class Edit : Form
    {
        public viewPanel MyParentForm2;
        public Form1 MyParentForm3;
        private String target = "";
        private string filename = "";
        private List<string> read_genre;
        private List<string> read_actor;
        private bool isChanged = false;

        public Edit()
        {
            InitializeComponent();
            loadDate();
        }

        public void getGenre(List<string> list)
        {
            this.read_genre = list;
        }

        public void getActor(List<string> list)
        {
            this.read_actor = list;
        }

        public void setTextBox1(string str)
        {
            this.textBox1.Text = str;
            target = str;
        }

        public void setComboBox1(string str)
        {
            this.comboBox1.Text = str;
        }

        public void setTextBox2(string str)
        {
            this.textBox2.Text = str;
        }

        public void setComboBox2(string str)
        {
            this.comboBox2.Text = str;
        }

        public void setComboBox3(string str)
        {
            this.comboBox3.Text = str;
        }

        public void setTextBox3(string str)
        {
            this.textBox3.Text = str;
        }

        public void setRichTextBox1(List<String> str)
        {
            for (int i = 0; i < str.Count; i++)
            {
                this.richTextBox1.Text += (str[i] + ",");

            }
        }

        public void setRichTextBox2(List<String> str)
        {
            for (int i = 0; i < str.Count; i++)
            {
                this.richTextBox2.Text += (str[i] + ",");

            }
        }

        public void setRichTextBoxString1(string str)
        {
            this.richTextBox1.Text = str.Replace("; ", ",");
        }

        public void setRichTextBoxString2(string str)
        {
            this.richTextBox2.Text = str.Replace("; ", ","); 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
        }

        public void getFilename(string globalFilename)
        {
            this.filename = globalFilename;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(isChanged == true)
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    errorProvider1.SetError(this.textBox1, "No any text");
                }
                else if (string.IsNullOrWhiteSpace(comboBox1.Text))
                {
                    errorProvider1.SetError(this.comboBox1, "No any text");

                }
                else if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    errorProvider1.SetError(this.textBox2, "No any text");

                }
                else if (string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    errorProvider1.SetError(this.textBox3, "No any text");

                }
                else if (string.IsNullOrWhiteSpace(richTextBox1.Text))
                {
                    errorProvider1.SetError(this.richTextBox1, "No any text");

                }
                else if (string.IsNullOrWhiteSpace(richTextBox2.Text))
                {
                    errorProvider1.SetError(this.richTextBox2, "No any text");

                }
                else
                {

                    XDocument doc = XDocument.Load(filename);

                    var node = doc.Descendants("movie").FirstOrDefault(movie => movie.Element("title").Value == target);
                    List<String> list = creatSubLine(richTextBox1.Text);
                    List<String> list2 = creatSubLine(richTextBox2.Text);

                    node.SetElementValue("title", this.textBox1.Text);
                    node.SetElementValue("year", this.comboBox1.Text);
                    node.SetElementValue("length", this.textBox2.Text + " min");
                    node.SetElementValue("certification", this.comboBox2.Text);
                    node.SetElementValue("director", this.textBox3.Text);
                    node.SetElementValue("rating", this.comboBox3.Text);


                    deleteNode("genre", doc);

                    node.Add(list.Select(l => new XElement("genre", l)));



                    deleteNode("actor", doc);

                    node.Add(list2.Select(l => new XElement("actor", l)));

                    Console.WriteLine(node);
                    doc.Save(filename);
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }


        }

        private void deleteNode(String dleteValue, XDocument doc)
        {
            var node = doc.Descendants("movie").FirstOrDefault(movie => movie.Element("title").Value == target);
            if(node != null)
            {
                XElement child3 = node.Element(dleteValue);
                if (child3 != null)
                {
                    child3.Remove();
                    deleteNode(dleteValue, doc);
                }
            }



        }

        private List<String> creatSubLine(string richTextBox1)
        {
            int position = 0;
            int start = 0;
            var sentences = new List<String>();            // Extract sentences from the string.
            do
            {
                position = richTextBox1.IndexOf(',', start);
                if (position >= 0)
                {
                    sentences.Add(richTextBox1.Substring(start, position - start).Trim());
                    start = position + 1;

                }
            } while (position > 0);


            return sentences;

        }

        private void loadDate()
        {
            for (int i = 1900; i <= 2015; i++)
            {
                comboBox1.Items.Add(i);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var form = new getInsertBox1 { MyParentForm2 = this })
            {
                form.getData(read_genre);
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string val = form.ReturnValue1;            //values preserved after close
                    string dateString = form.ReturnValue2;
                    //Do something here with these values
                    String tmp = "";
                    if (val != null && !richTextBox1.Text.Contains(val))
                    {
                        tmp = richTextBox1.Text;
                        this.richTextBox1.Text = tmp + val;
                    }
                    else
                    {
                        MessageBox.Show("The Genre '" + val + "' already insert.");
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var form = new getInsertBox1 { MyParentForm2 = this })
            {
                form.getData(read_actor);
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string val = form.ReturnValue1;            //values preserved after close
                    string dateString = form.ReturnValue2;
                    //Do something here with these values

                    String tmp = "";
                    if (val != null && !richTextBox2.Text.Contains(val))
                    {
                        tmp = richTextBox2.Text;
                        this.richTextBox2.Text = tmp + val;
                    }
                    else
                    {
                        MessageBox.Show("The Actor '" + val + "' already insert.");
                    }

                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.richTextBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string text = textBox2.Text;
            bool hasDigit = false;

            foreach (char letter in text)
            {
                if (char.IsDigit(letter))
                {
                    hasDigit = true;
                    break;
                }
            }

            // Call SetError or Clear on the ErrorProvider.
            if (!hasDigit)
            {
                errorProvider2.SetError(textBox2, "Please enter digit");
            }
            else if (hasDigit)

            {

                if (text.Length > 3)
                {
                    errorProvider2.SetError(textBox2, "Number of digit is four numbers");
                }
                else if (text.Length == 0)
                {
                    errorProvider2.SetError(textBox2, "Enter void length");
                }
                else
                    errorProvider2.Clear();
            }
            isChanged = true;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            isChanged = true;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isChanged = true;

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            isChanged = true;

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            isChanged = true;

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            isChanged = true;

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            isChanged = true;

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            isChanged = true;

        }
    }
}
