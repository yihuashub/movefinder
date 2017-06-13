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
    public partial class viewList: Form
    {
        private List<string> dataList = new List<string>();
        public Form1 MyParentForm;

        public viewList()
        {
            InitializeComponent();



        }



        private void viewBox()
        {
            for(int i=0; i <dataList.Count; i++)
            {
                listBox1.Items.Add(dataList[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter a void name");
            }
            else if(dataList.Contains(textBox1.Text))
            {
                MessageBox.Show("this '" +textBox1.Text + "' is already exists.");

            }
            else
            {
                this.listBox1.Items.Add(this.textBox1.Text);
                dataList.Add(this.textBox1.Text);
            }


        }

        public List<string> getList()
        {
            return dataList;
        }

        public void setData(List<string> inData)
        {

            dataList = inData;

            for (int i = 0; i < dataList.Count; i++)
            {
                listBox1.Items.Add(dataList[i]);
            }

        }
    }
}
