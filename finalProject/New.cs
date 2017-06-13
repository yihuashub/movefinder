using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace finalProject
{
    public partial class New : Form
    {
        public Form1 MyParentForm;
        private List<string> read_genre;
        private List<string> read_actor;
        private string filename;
        private bool isChanged = false;


        public New()
        {
            InitializeComponent();
            loadDate();
            comboBox3.Text = "0";
            comboBox2.Text = "N/A";
            comboBox1.Text = "2015";

        }

        public void getFilename(string name)
        {
            this.filename = name;
        }

        private void loadDate()
        {
            for (int i = 1900; i <= 2015; i++)
            {
                comboBox1.Items.Add(i);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string error = null;
            if (textBox1.Text.Length == 0)
            {
                error = "Please enter a name";
            }
            // first argument is the control that the error is associated with
            // the second argument is the error message
            errorProvider1.SetError((Control)sender, error);
            isChanged = true;
        }

        void textBox1_TextChanged(object sender, CancelEventArgs e)
        {
            Regex re = new Regex(@"^\(\d{3}\) \d{3}-\d{4}$");
            string toolTip = toolTip1.GetToolTip((Control)sender);
            if (!re.IsMatch(((TextBox)sender).Text))
            {
                errorProvider1.SetError((Control)sender, toolTip);
                helpProvider1.SetHelpString((Control)sender, null);
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError((Control)sender, null);
                helpProvider1.SetHelpString((Control)sender, toolTip);
            }
            isChanged = true;

        }

        void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string error = null;
            if (textBox1.Text.Length == 0)
            {
                error = "Please enter a name";
                e.Cancel = true; // need to set this to avoid losing focus
            }
            // first argument is the control that the error is associated with
            // the second argument is the error message
            errorProvider1.SetError((Control)sender, error);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBox1.Text))
            {
                errorProvider1.SetError(this.textBox1, "No any text");
            }
            else if(string.IsNullOrWhiteSpace(comboBox1.Text))
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
                List<String> list = creatSubLine(richTextBox1.Text);
                List<String> list2 = creatSubLine(richTextBox2.Text);


                XDocument doc = XDocument.Load(filename);
                XElement NameList = doc.Element("movielist");
                NameList.Add(new XElement("movie", new XElement("title", this.textBox1.Text), new XElement("year", this.comboBox1.Text)
                    , new XElement("length", this.textBox2.Text + " min"), new XElement("certification", this.comboBox2.Text), new XElement("director", this.textBox3.Text), new XElement("rating", this.comboBox3.Text)
                    , list.Select(l => new XElement("genre", l)), list2.Select(l => new XElement("actor", l))));


                doc.Save(filename);
                Close();
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

        private void New_Load(object sender, EventArgs e)
        {
            // Use tooltips to populate the "information provider"
            foreach (Control control in this.Controls)
            {
                string toolTip = toolTip1.GetToolTip(control);
                if (toolTip.Length == 0)
                    continue;
                helpProvider1.SetHelpString(control, toolTip);
            }
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
                else if(text.Length == 0)
                {
                    errorProvider2.SetError(textBox2, "Enter void length");
                }
                else
                    errorProvider2.Clear();
            }
            isChanged = true;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isChanged = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
           // new getInsertBox { MyParentForm = this }.ShowDialog();

            using (var form = new getInsertBox1 { MyParentForm = this })
            {
                form.getData(read_genre);
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string val = form.ReturnValue1;            //values preserved after close
                    string dateString = form.ReturnValue2;
                    //Do something here with these values
                    String tmp = "";
                    if(val != null && !richTextBox1.Text.Contains(val))
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
            using (var form = new getInsertBox1 { MyParentForm = this })
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

        private void button5_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.richTextBox2.Text = "";

        }

        public void getGenre(List<string> list)
        {
            this.read_genre = list;
        }

        public void getActor(List<string> list)
        {
            this.read_actor = list;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string error = null;
            if (textBox3.Text.Length == 0)
            {
                error = "Please enter a name";
            }
            // first argument is the control that the error is associated with
            // the second argument is the error message
            errorProvider1.SetError((Control)sender, error);
            isChanged = true;

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
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
