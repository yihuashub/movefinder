using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace finalProject
{
    public partial class getInsertBox1 : Form
    {
        public New MyParentForm;
        public Edit MyParentForm2;

        public string ReturnValue1 { get; set; }
        public string ReturnValue2 { get; set; }
        private List<String> list = new List<string>();


        public getInsertBox1()
        {
            InitializeComponent();


        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
                this.DialogResult = DialogResult.OK;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string error = null;
            if (textBox1.Text.Length == 0)
            {
                error = "Please enter a name";
                errorProvider1.SetError((Control)sender, error);

            }
            else
            {
                this.ReturnValue1 = textBox1.Text + ",";
                this.ReturnValue2 = DateTime.Now.ToString(); //example
                this.Close();
            }
            // first argument is the control that the error is associated with
            // the second argument is the error message

  
        }

        public void getData(List<string> data)
        {
            list = data;

            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    listBox1.Items.Add(list[i]);
                }
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            this.textBox1.Text = listBox1.Text;
            this.ReturnValue1 = textBox1.Text + ",";
            this.ReturnValue2 = DateTime.Now.ToString(); //example
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox1.Text = listBox1.Text;
        }
    }
}
