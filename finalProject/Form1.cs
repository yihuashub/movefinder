using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace finalProject
{
    public partial class Form1 : Form
    {

        private string[,] read_data;
        private List<string> read_genre;
        private string[,] read_genre_with_color;
        private List<string> read_actor;
        private List<string> read_history;
        private List<string> read_comment;
        private List<string> dataList;
        private List<string> tmp;
        private List<Color> colors;
        private string globalFilename = "movies.xml";


        private int j = 0;
        private int jj = 0;

        public Form1()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "Ready";
            comboBox2.Text = "Watch List";
            backgroundWorker1.RunWorkerAsync();
            loadChecker();
            read_records(globalFilename);
            this.comboBox1.Text = "Title";
            label10.Text = (jj + 1).ToString();

            this.copy_pane();
            radioButton1.Checked = true;

            if (dataList!=null)
            {
                for(int i = 0; i<dataList.Count; i++)
                {
                    comboBox2.Items.Add(dataList[i]);

                }
            }

        }


        private void showAllList()
        {
            for (int i = 0; i <= jj; i++)
            {
                string[] items = new string[8];
                for (int k = 0; k < 8; k++)
                {
                    items[k] = this.read_data[i, k];
                }
                ListViewItem item2 = new ListViewItem(items);
                this.listView1.Items.Add(item2);
            }
            this.listView1.Refresh();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(this.comboBox2.Text))
            {
                XDocument doc = XDocument.Load(globalFilename);

                if (this.listView1.SelectedItems.Count > 1)
                {
                    ListView.SelectedIndexCollection c = listView1.SelectedIndices;
                    
                    for(int i = 0; i< listView1.SelectedItems.Count; i++)
                    {
                        addMoviesToList(doc, listView1.Items[c[i]].SubItems[0].Text);
                    }

                }
                else if (this.listView1.SelectedItems.Count == 1)
                {
                    ListView.SelectedIndexCollection c = listView1.SelectedIndices;

                    addMoviesToList(doc, listView1.Items[c[0]].SubItems[0].Text);

                }
                else
                {
                    MessageBox.Show("Please select a item.");
                }
            }
            else
            {
                MessageBox.Show("Please select a list.");
            }
        }

        private void addMoviesToList(XDocument doc, string target)
        {
            var node = doc.Descendants("movie").FirstOrDefault(movie => movie.Element("title").Value == target);

            if (node.Element("list") != null)
            {

                tmp = new List<string>();

                deleteNode("list", doc, target);


                if (!tmp.Contains(this.comboBox2.Text))
                {
                    node.Add(tmp.Select(l => new XElement("list", l)));
                    node.Add(new XElement("list", this.comboBox2.Text));
                    MessageBox.Show("Add the movie successfully.");

                }
                else
                {
                    node.Add(tmp.Select(l => new XElement("list", l)));
                    MessageBox.Show("This movie already in the list.");
                }

                Console.WriteLine(node);
                doc.Save(globalFilename);


            }
            else
            {
                node.Add(new XElement("list", this.comboBox2.Text));
                MessageBox.Show("Add the movie successfully.");
                Console.WriteLine(node);
                doc.Save(globalFilename);
            }
        }

        private void deleteNode(String dleteValue, XDocument doc, string target)
        {
            var node = doc.Descendants("movie").FirstOrDefault(movie => movie.Element("title").Value == target);
            XElement child3 = node.Element(dleteValue);
            if (child3 != null)
            {
                child3.Remove();
                tmp.Add(child3.Value);
                deleteNode(dleteValue, doc, target);
            }


        }

        private void checkList(XDocument doc, string value)
        {
            ListView.SelectedIndexCollection c = listView1.SelectedIndices;
            var node = doc.Descendants("movie").FirstOrDefault(movie => movie.Element("title").Value == listView1.Items[c[0]].SubItems[0].Text);

            node.Elements("list");

            if (node.Element("list").Value.CompareTo(value) != 0)
            {
                checkList(doc, value);
            }
            else
            {
                MessageBox.Show("ok i found");
            }

        }



        private void loadChecker()
        {
            if (!File.Exists("movies.xml"))
            {
                MessageBox.Show("There are no data file in the folder, a new XML file with defult data has been created.",
                    "Message");
                XmlTextWriter writer2 = new XmlTextWriter(@"movies.xml", System.Text.Encoding.UTF8);
                writer2.WriteStartDocument(true);
                writer2.Formatting = Formatting.Indented;
                writer2.Indentation = 2;
                writer2.WriteStartElement("movielist");

                creatXML("Adventures of Casanova", "1948", "83 min", "Approved", "Roberto Gavaldn", "1", "Action,Adventure,History,Romance,War,", "Arturo de C  rdova,Lucille Bremer,Turhan Bey,John Sutton,George Tobias,", writer2);
                creatXML("Amazon Quest ", "1954", "75 min", "N/A", "Steve Sekely", "7", "Action,Drama,", "Tom Neal, Carole Mathews,Carole Donne, Don Zelaya,Ralph Graves,", writer2);
                creatXML("American Ninja 3: Blood Hunt ", "1989", "89 min", "R", "Cedric Sundstrom", "7", "Action,Drama,", "David Bradley,Steve James,Marjoe Gortner,Michele B. Chan, Yehuda Efroni,", writer2);

                writer2.WriteEndElement();
                writer2.WriteEndDocument();
                writer2.Close();
            }
        }

        private void creatXML(String title, String year, string length, String certification, String director, string rating, String genre,String actor, XmlTextWriter writer)
        {
            writer.WriteStartElement("movie");
            writer.WriteStartElement("title");
            writer.WriteString(title);
            writer.WriteEndElement();

            writer.WriteStartElement("year");
            writer.WriteString(year);
            writer.WriteEndElement();

            writer.WriteStartElement("length");
            writer.WriteString(length);
            writer.WriteEndElement();

            if (certification != "N/A")
            {
                writer.WriteStartElement("certification");
                writer.WriteString(certification);
                writer.WriteEndElement();
            }

            writer.WriteStartElement("director");
            writer.WriteString(director);
            writer.WriteEndElement();

            writer.WriteStartElement("rating");
            writer.WriteString(rating);
            writer.WriteEndElement();


            creatSubLine("genre", genre, writer);
            creatSubLine("actor", actor, writer);


            writer.WriteEndElement();

        }

        private void creatSubLine(string type, string value, XmlTextWriter writer)
        {
            int position = 0;
            int start = 0;
            // Extract sentences from the string.
            do
            {
                position = value.IndexOf(',', start);
                if (position >= 0)
                {
                    writer.WriteStartElement(type);
                    writer.WriteString(value.Substring(start, position - start).Trim());
                    writer.WriteEndElement();
                    start = position + 1;
                }
            } while (position > 0);

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            getNew();
        }


        private void getNew()
        {
            int tmp = jj;

            New new1 = new New { MyParentForm = this };

            // new New { MyParentForm = this }.ShowDialog();
            new1.getFilename(globalFilename);
            new1.getGenre(read_genre);
            new1.getActor(read_actor);
            new1.ShowDialog();

            this.read_records(globalFilename);

            if (tmp != jj)
            {
                this.listView1.Items.Clear();
                this.read_records(globalFilename);
                this.listView1.Refresh();
                for (int i = 0; i <= this.jj; i++)
                {
                    string[] items = new string[8];
                    for (int j = 0; j < 8; j++)
                    {
                        items[j] = this.read_data[i, j];
                        label10.Text = (jj + 1).ToString();
                    }
                    ListViewItem item = new ListViewItem(items);
                    this.listView1.Items.Add(item);
                    this.listView1.Refresh();
                }


            }

            

        }

        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML Documents (*.xml)|*.xml|All Files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if(openFileDialog1 != null)
                {
                    this.listView1.Items.Clear();
                    read_records(openFileDialog1.FileName);
                    
                }
               // string stringname = openFileDialog1.FileName;

            }

            this.label10.Text = (jj+1).ToString();
        }

        // ******************************************  LOAD FUNCTION ******************************************

        private void read_records(string filename)
        {
            int count = 0;
            this.j = 0;
            this.jj = 0;
            read_data = new string[0xc350, 12];
            read_genre = new List<string>();
            read_actor = new List<string>();
            read_history = new List<string>();
            read_comment = new List<string>();
            dataList = new List<string>();


            this.dataList.Add("Watch List");
            this.dataList.Add("Purchase List");


            if (filename.CompareTo("movies.xml") != 0)
            {
                globalFilename = filename;
            }


            XmlTextReader reader = new XmlTextReader(globalFilename);
            XmlNodeType type;

            for (int i = 0; i < 6; i++)
            {
                //this.univ_years[i, 0, 0] = this.univ_years[i, 1, 0] = 0;
            }

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {

                    if (reader.Name != "movie")
                    {
                        if (reader.Name == "title")
                        {
                            reader.Read();
                            this.read_data[this.j, 0] = reader.Value;
                        }

                        if (reader.Name == "year")
                        {
                            reader.Read();
                            this.read_data[this.j, 1] = reader.Value;
                        }
                        else if (reader.Name == "length")
                        {
                            reader.Read();
                            this.read_data[this.j, 2] = reader.Value;

                        }
                        else if (reader.Name == "certification")
                        {
                            reader.Read();
                            this.read_data[this.j, 3] = reader.Value;

                        }
                        else if (reader.Name == "director")
                        {
                            reader.Read();
                            this.read_data[this.j, 4] = reader.Value;

                        }
                        else if (reader.Name == "rating")
                        {
                            reader.Read();
                            this.read_data[this.j, 5] = reader.Value;

                        }
                        else if (reader.Name == "genre")
                        {
                            reader.Read();
                            this.read_data[this.j, 6] += (reader.Value + "; ");
                            if(!read_genre.Contains(reader.Value))
                            {
                                this.read_genre.Add(reader.Value);
                            }

                        }
                        else if (reader.Name == "list")
                        {
                            reader.Read();
                            if (!dataList.Contains(reader.Value))
                            {
                                this.dataList.Add(reader.Value);
                            }

                            this.read_data[this.j, 8] += (reader.Value + ";");

                        }
                        else if (reader.Name == "history")
                        {
                            reader.Read();
                            if (!read_history.Contains(reader.Value))
                            {
                                if(read_data[this.j, 6] != null && read_data[this.j, 6].Any())
                                {
                                    this.read_history.Add(read_data[this.j, 6]);
                                }

                            }

                            this.read_data[this.j, 10] += (reader.Value);

                        }
                        else if (reader.Name == "comment")
                        {
                            reader.Read();

                            this.read_data[this.j, 11] += (reader.Value);

                        }
                        else if (reader.Name == "actor")
                        {
                            reader.Read();
                            count = count + 1;

                            this.read_data[this.j, 7] += (reader.Value + "; ");

                            if (!read_actor.Contains(reader.Value))
                            {
                                this.read_actor.Add(reader.Value);
                            }
                        }

                    }
                    else if(reader.Name == "movie")
                    {
                        if(count != 0)
                        {
                            if (read_data[j, 0] != null)
                            {
                                this.j++;
                            }
                        }
                        else
                        {
                            count++;
                        }

                    }
                }
                
            }
            reader.Close();
            this.jj = this.j;
            this.j = 0;

            getColorForGenre();
            putDataToCheckBox();

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
            {
                ListView.SelectedIndexCollection c = listView1.SelectedIndices;
                label3.Text = listView1.Items[c[0]].SubItems[0].Text;
                label4.Text = listView1.Items[c[0]].SubItems[5].Text;
                richTextBox1.Text = listView1.Items[c[0]].SubItems[6].Text;
                richTextBox2.Text = listView1.Items[c[0]].SubItems[7].Text;
                label8.Text = listView1.Items[c[0]].SubItems[4].Text;
            }
        }

        private void newMovieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getNew();

        }



        private void linkLabel1_LinkClicked(object sender, EventArgs e)
        {
            viewPanel newView = new viewPanel { MyParentForm1 = this };

            newView.getFilename(globalFilename);
            newView.setLabel1(this.listView1.FocusedItem.SubItems[0].Text);
            newView.setLabel2(this.listView1.FocusedItem.SubItems[1].Text);
            newView.setLabel3(this.listView1.FocusedItem.SubItems[2].Text);
            newView.setLabel4(this.listView1.FocusedItem.SubItems[3].Text);
            newView.setLabel5(this.listView1.FocusedItem.SubItems[4].Text);
            newView.setLabel6(this.listView1.FocusedItem.SubItems[5].Text);
            newView.setLabel7(creatSubLine(this.listView1.FocusedItem.SubItems[6].Text));
            newView.setLabel8(creatSubLine(this.listView1.FocusedItem.SubItems[7].Text));
            newView.showRating(this.listView1.FocusedItem.SubItems[5].Text);
            newView.getActor(read_actor);
            newView.getGenre(read_genre);
            for(int i=0; i<= jj; i++)
            {
                if(this.read_data[i,0].CompareTo(this.listView1.FocusedItem.SubItems[0].Text) == 0)
                {
                    if(this.read_data[i, 11] != null)
                    {
                        newView.getComment(this.read_data[i, 11]);
                    }
                }
            }
            

            newView.ShowDialog();
        }

        private List<String> creatSubLine(string richTextBox1)
        {
            int position = 0;
            int start = 0;
            var sentences = new List<String>();            // Extract sentences from the string.
            do
            {
                position = richTextBox1.IndexOf(';', start);
                if (position >= 0)
                {
                    sentences.Add(richTextBox1.Substring(start, position - start).Trim());
                    start = position + 1;

                }
            } while (position > 0);


            return sentences;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            String tmp1 = "";
            this.label13.Text = "0";
            int count = 0;
            this.label14.Text = "General Search";

            if (textBox1.Text.Length != 0)
            {
                this.listView1.Items.Clear();

                string[] items = new string[8];

                if(comboBox1.Text == "Title")
                {
                    if(tmp1.CompareTo(comboBox1.Text) != 0)
                    {
                        tmp1 = comboBox1.Text;
                    
                        for(int i =0; i<=jj;i++)
                        {

                            if(read_data[i,0] != null && read_data[i,0].Contains(textBox1.Text))
                            {
                                count++;

                                for (int k = 0; k < 8; k++)
                                {
                                    items[k] = this.read_data[i, k];
                                }
                                ListViewItem item2 = new ListViewItem(items);
                                this.listView1.Items.Add(item2);
                                this.listView1.Refresh();
                            }

                        }
                    }
                      else
                      {
                        MessageBox.Show("Searched in the list");
                      }
                    label13.Text = (count).ToString();
                    label13.ForeColor = Color.Red;

                }
                else if (comboBox1.Text == "Year")
                {
                    if (tmp1.CompareTo(comboBox1.Text) != 0)
                    {
                        tmp1 = comboBox1.Text;

                        for (int i = 0; i <= jj; i++)
                        {

                            if (read_data[i, 1] != null && read_data[i, 1].Contains(textBox1.Text))
                            {
                                count++;

                                for (int k = 0; k < 8; k++)
                                {
                                    items[k] = this.read_data[i, k];
                                }
                                ListViewItem item2 = new ListViewItem(items);
                                this.listView1.Items.Add(item2);
                                this.listView1.Refresh();


                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Searched in the list");
                    }
                    label13.Text = (count).ToString();
                    label13.ForeColor = Color.Red;



                }
                else if (comboBox1.Text == "Length")
                {
                    if (tmp1.CompareTo(comboBox1.Text) != 0)
                    {
                        tmp1 = comboBox1.Text;

                        for (int i = 0; i <= jj; i++)
                        {

                            if (read_data[i, 2] != null && read_data[i, 2].Contains(textBox1.Text))
                            {
                                count++;

                                for (int k = 0; k < 8; k++)
                                {
                                    items[k] = this.read_data[i, k];
                                }
                                ListViewItem item2 = new ListViewItem(items);
                                this.listView1.Items.Add(item2);
                                this.listView1.Refresh();


                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Searched in the list");
                    }
                    label13.Text = (count).ToString();
                }
                else if (comboBox1.Text == "Certification")
                {
                    if (tmp1.CompareTo(comboBox1.Text) != 0)
                    {
                        tmp1 = comboBox1.Text;

                        for (int i = 0; i <= jj; i++)
                        {

                            if (read_data[i, 3] != null && read_data[i, 3].Contains(textBox1.Text))
                            {
                                count++;

                                for (int k = 0; k < 8; k++)
                                {
                                    items[k] = this.read_data[i, k];
                                }
                                ListViewItem item2 = new ListViewItem(items);
                                this.listView1.Items.Add(item2);
                                this.listView1.Refresh();
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Searched in the list");
                    }
                    label13.Text = (count).ToString();
                    label13.ForeColor = Color.Red;
                }
                else if (comboBox1.Text == "Director")
                {
                    if (tmp1.CompareTo(comboBox1.Text) != 0)
                    {
                        tmp1 = comboBox1.Text;

                        for (int i = 0; i <= jj; i++)
                        {

                            if (read_data[i, 4] != null && read_data[i, 4].Contains(textBox1.Text))
                            {
                                count++;

                                for (int k = 0; k < 8; k++)
                                {
                                    items[k] = this.read_data[i, k];
                                }
                                ListViewItem item2 = new ListViewItem(items);
                                this.listView1.Items.Add(item2);
                                this.listView1.Refresh();
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Searched in the list");
                    }
                    label13.Text = (count).ToString();
                    label13.ForeColor = Color.Red;
                }
                else if (comboBox1.Text == "Reating")
                {
                    if (tmp1.CompareTo(comboBox1.Text) != 0)
                    {
                        tmp1 = comboBox1.Text;

                        for (int i = 0; i <= jj; i++)
                        {

                            if (read_data[i, 5] != null && read_data[i, 5].Contains(textBox1.Text))
                            {
                                count++;

                                for (int k = 0; k < 8; k++)
                                {
                                    items[k] = this.read_data[i, k];
                                }
                                ListViewItem item2 = new ListViewItem(items);
                                this.listView1.Items.Add(item2);
                                this.listView1.Refresh();
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Searched in the list");
                    }
                    label13.Text = (count).ToString();
                    label13.ForeColor = Color.Red;
                }

                if (count == 0)
                {
                    string strMsg = String.Format("Sorry, Cannot find '{0}' in file.", textBox1.Text);
                    MessageBox.Show(strMsg);
                    label13.ForeColor = Color.Black;
                }
            }
            else
            {
                MessageBox.Show("Please enter key word.");
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                // Wait 100 milliseconds.
                Thread.Sleep(100);
                // Report progress.
                //backgroundWorker1.ReportProgress(i);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            toolStripProgressBar1.Value = e.ProgressPercentage;
            // Set the text.
            this.Text = e.ProgressPercentage.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            read_records(globalFilename);
            label14.Text = "Recommender Search";
            string tmp = "";
            string gence = "";

            if (!read_history.Any())
            {
                MessageBox.Show("Sorry, Cannot find any data yet.\nPlease play any movie frist.");
            }
            else
            {
                this.listView1.Items.Clear();

                int maxLength = 20;
                if(jj < 20)
                {
                    maxLength = jj;
                }
                string[,] recommenderList = new string[maxLength, 8];
                for (int j = 0; j <= jj; j++)
                {
                    for (int i = 0; i < maxLength; i++) 
                    {
                        for (int k = 0; k < read_history.Count; k++)
                        {
                            if(read_data[j, 6].Contains(this.read_history[k]))
                            {
                                if(read_data[j,0] != tmp)
                                {
                                    string[] items = new string[8];
                                    for (int kk = 0; kk < 8; kk++)
                                    {
                                        items[kk] = this.read_data[j, kk];
                                    }
                                    ListViewItem item2 = new ListViewItem(items);
                                    this.listView1.Items.Add(item2);
                                    tmp = read_data[j, 0];
                                    if(!gence.Contains(read_history[k]))
                                    {
                                        gence += read_history[k] + " ";
                                    }

                                }
 

                            }
                        }
                    }
                }
                MessageBox.Show("Base on you like: " + gence + "those movies you may like.");
                this.listView1.Refresh();
            }


        }

        private void flashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
            read_records(globalFilename);
            for (int i = 0; i <= jj; i++)
            {
                string[] items = new string[8];
                for (int k = 0; k < 8; k++)
                {
                    items[k] = this.read_data[i, k];
                }
                ListViewItem item2 = new ListViewItem(items);
                this.listView1.Items.Add(item2);
                this.listView1.Refresh();
            }
        }

        public List<String> getGenre()
        {

              return read_genre;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            viewList new1 = new viewList { MyParentForm = this };
            List<string> tmpList = new List<string>();

            new1.setData(dataList);
            tmpList = new1.getList();

            var result = new1.ShowDialog();
            if (result == DialogResult.OK)
            {
                comboBox2.Items.Clear();
                for(int i = 0; i<tmpList.Count; i++)
                {
                    this.comboBox2.Items.Add(tmpList[i]);
                }
            }
        }

        public List<string> getDataList()
        {
            return dataList;
        }

        private void showList()
        {
            this.listView1.Items.Clear();
            read_records(globalFilename);

            for (int i = 0; i <= jj; i++)
            {
                if (this.read_data[i, 8] != null)
                {
                    string[] words = this.read_data[i, 8].Split(';');
                    foreach (string word in words)
                    {

                        if (word.CompareTo(this.comboBox2.Text) == 0)
                        {
                            string[] items = new string[8];
                            for (int k = 0; k < 8; k++)
                            {
                                items[k] = this.read_data[i, k];
                            }
                            ListViewItem item2 = new ListViewItem(items);
                            this.listView1.Items.Add(item2);

                            this.listView1.Refresh();
                        }


                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(radioButton3.Checked)
            {
                showList();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();

            this.textBox1.ReadOnly = false;
            this.button3.Enabled = true;
            this.button4.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();

            this.textBox1.ReadOnly = true;
            this.button3.Enabled = false;
            this.button4.Enabled = false;
            showAllList();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
            this.textBox1.ReadOnly = false;
            this.button3.Enabled = true;
            this.button4.Enabled = true;

            if (this.comboBox2.Text != "")
            {
                showList();
            }
        }

        private void copy_pane()
        {
            Graphics graphics = this.panel1.CreateGraphics();
            Rectangle rect = new Rectangle(00, 00, 517, 393);
            LinearGradientBrush brush = new LinearGradientBrush(rect, Color.White, Color.AntiqueWhite, LinearGradientMode.BackwardDiagonal);
            graphics.FillRectangle(brush, rect);
            Pen pen = new Pen(Color.Maroon, 2f);
            graphics.DrawLine(pen, 20, 55, 20, 359);     //Y

            int x01 = 328;

            for(int i=0; i < 10; i++)
            {
                graphics.DrawLine(pen, 14, x01, 26, x01);
                x01 = x01 - 30;
            }


            graphics.DrawLine(pen, 20, 359, 510, 359); // X

            int y01 = 59 ;

            for (int i = 0; i < 13; i++)
            {
                graphics.DrawLine(pen, y01, 353, y01, 365);
                y01 = y01 + 40;
            }


            Font font = new Font("Verdana", 10f);
            Font font2 = new Font("Verdana", 9f);
            graphics.DrawString("Reating", font, new SolidBrush(Color.Maroon), (float)0f, (float)0f);

            int data1 = 1; int x1 = 2; int y1 = 320;
            for (int i = 0; i < 10; i++)
            {
                graphics.DrawString(data1.ToString(), font2, new SolidBrush(Color.Maroon), x1, y1);
                y1 = y1 - 30;   //49
                data1 = data1 + 1;
            }

            graphics.DrawString("Years", font, new SolidBrush(Color.Maroon), (float)250f, (float)376);
            int data2 = 1900; int x2 = 40; int y2 = 366;
            for (int i = 0; i<12; i++)
            {
                graphics.DrawString(data2.ToString(), font2, new SolidBrush(Color.Maroon), x2, y2);
                x2 = x2 + 40;
                data2 = data2 + 10;
            }

            Pen pen2 = new Pen(Color.DarkBlue, 8f);
            graphics.DrawLine(pen2, 580, 40, 620, 40);
            graphics.DrawString("Majority Male", font2, new SolidBrush(Color.Maroon), (float)625f, (float)33f);
            Pen pen3 = new Pen(Color.LimeGreen, 8f);
            graphics.DrawLine(pen3, 580, 70, 620, 70);
            graphics.DrawString("Majority Female", font2, new SolidBrush(Color.Maroon), (float)625f, (float)63f);
            Pen pen4 = new Pen(Color.Red, 8f);
            graphics.DrawLine(pen4, 580, 110, 620, 110);
            graphics.DrawString("Equal Number of", font2, new SolidBrush(Color.Maroon), (float)625f, (float)96f);
            graphics.DrawString("Male and Female", font2, new SolidBrush(Color.Maroon), (float)625f, (float)110f);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            this.copy_pane();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            listView2.Items.Clear();

            if (this.trackBar1.Value == 0)
            {
                this.copy_pane();
                drawData('A', 'a');
                this.listView2.Refresh();
            }
            else if (this.trackBar1.Value == 1)
            {
                this.copy_pane();
                drawData('B', 'b');
                this.listView2.Refresh();
            }
            else if (this.trackBar1.Value == 2)
            {
                this.copy_pane();
                drawData('C', 'c');
                this.listView2.Refresh();
            }
            else if (this.trackBar1.Value == 3)
            {
                this.copy_pane();
                drawData('D', 'd');
                this.listView2.Refresh();
            }
            else if (this.trackBar1.Value == 4)
            {
                this.copy_pane();
                drawData('E', 'e');
                drawData('F', 'f');
                this.listView2.Refresh();
            }
            else if (this.trackBar1.Value == 5)
            {
                this.copy_pane();
                drawData('G', 'g');
                drawData('H', 'h');
                drawData('I', 'i');
                drawData('J', 'j');
                drawData('K', 'k');
                this.listView2.Refresh();
            }
            else if (this.trackBar1.Value == 6)
            {
                this.copy_pane();
                drawData('L', 'l');
                drawData('M', 'n');
                drawData('O', 'o');
                drawData('P', 'p');
                drawData('Q', 'q');
                this.listView2.Refresh();
            }
            else if (this.trackBar1.Value == 7)
            {
                this.copy_pane();
                drawData('R', 'r');
                drawData('S', 's');
                this.listView2.Refresh();
            }
            else if (this.trackBar1.Value == 8)
            {
                this.copy_pane();
                drawData('T', 't');
                drawData('U', 'u');
                drawData('V', 'v');
                this.listView2.Refresh();
            }
            else if (this.trackBar1.Value == 9)
            {
                this.copy_pane();
                drawData('W', 'w');
                drawData('X', 'x');
                drawData('Y', 'y');
                drawData('Z', 'z');
                this.listView2.Refresh();
            }
            else
            {
                MessageBox.Show("Error");
            }

        }

        private void draw_graph(String target, string year, string rating, string genre )
        {
            Graphics graphics = this.panel1.CreateGraphics();
            double size = 0.655;

            Pen pen = new Pen(Color.Red, 3f);
            double x = 0;

            if (Int32.Parse(year) < 1900)
            {
                x = 19 + 40 * getYearPercent(Int32.Parse(year), 1890, 1900);

                for (int i = 0; i < read_genre.Count; i++)
                {
                    if (genre.Contains(read_genre_with_color[i, 0]))
                    {
                        if(checkedListBox1.CheckedItems.Contains(read_genre_with_color[i, 0]))
                        {
                            pen = new Pen(colors[Int32.Parse(read_genre_with_color[i, 2]) % colors.Count], (float)(i*size));
                            draw_rating(pen, x, graphics, Int32.Parse(rating));
                        }
                    }
                }
            }
            else if (1900 < Int32.Parse(year) && Int32.Parse(year) < 1910)
            {
                x = 59 + 40 * getYearPercent(Int32.Parse(year), 1900, 1910);
                for (int i = 0; i < read_genre.Count; i++)
                {
                    if (genre.Contains(read_genre_with_color[i, 0]))
                    {
                        if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                        {
                            pen = new Pen(colors[Int32.Parse(read_genre_with_color[i, 2]) % colors.Count], (float)(i*size));
                            draw_rating(pen, x, graphics, Int32.Parse(rating));
                        }
                    }
                }
            }
            else if (1910 < Int32.Parse(year) && Int32.Parse(year) < 1920)
            {
                 x = 99 + 40 * getYearPercent(Int32.Parse(year), 1910, 1920);
                for (int i = 0; i < read_genre.Count; i++)
                {
                    if (genre.Contains(read_genre_with_color[i, 0]))
                    {
                        if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                        {
                            pen = new Pen(colors[Int32.Parse(read_genre_with_color[i, 2]) % colors.Count], (float)(i*size));
                            draw_rating(pen, x, graphics, Int32.Parse(rating));
                        }
                    }
                }
            }
            else if (1920 < Int32.Parse(year) && Int32.Parse(year) < 1930)
            {
                 x = 139 + 40 * getYearPercent(Int32.Parse(year), 1920, 1930);
                for (int i = 0; i < read_genre.Count; i++)
                {
                    if (genre.Contains(read_genre_with_color[i, 0]))
                    {
                        if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                        {
                            pen = new Pen(colors[Int32.Parse(read_genre_with_color[i, 2]) % colors.Count], (float)(i*size));
                            draw_rating(pen, x, graphics, Int32.Parse(rating));
                        }
                    }
                }
            }
            else if (1930 < Int32.Parse(year) && Int32.Parse(year) < 1940)
            {
                 x = 179 + 40 * getYearPercent(Int32.Parse(year), 1930, 1940);
                for (int i = 0; i < read_genre.Count; i++)
                {
                    if (genre.Contains(read_genre_with_color[i, 0]))
                    {
                        if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                        {
                            pen = new Pen(colors[Int32.Parse(read_genre_with_color[i, 2]) % colors.Count], (float)(i*size));
                            draw_rating(pen, x, graphics, Int32.Parse(rating));
                        }
                    }
                }
            }
            else if (1940 < Int32.Parse(year) && Int32.Parse(year) < 1950)
            {
                 x = 219 + 40 * getYearPercent(Int32.Parse(year), 1940, 1950);
                for (int i = 0; i < read_genre.Count; i++)
                {
                    if (genre.Contains(read_genre_with_color[i, 0]))
                    {
                        if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                        {
                            pen = new Pen(colors[Int32.Parse(read_genre_with_color[i, 2]) % colors.Count], (float)(i*size));
                            draw_rating(pen, x, graphics, Int32.Parse(rating));
                        }
                    }
                }
            }
            else if (1950 < Int32.Parse(year) && Int32.Parse(year) < 1960)
            {
                 x = 259 + 40 * getYearPercent(Int32.Parse(year), 1950, 1960);
                for (int i = 0; i < read_genre.Count; i++)
                {
                    if (genre.Contains(read_genre_with_color[i, 0]))
                    {
                        if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                        {
                            pen = new Pen(colors[Int32.Parse(read_genre_with_color[i, 2]) % colors.Count], (float)(i*size));
                            draw_rating(pen, x, graphics, Int32.Parse(rating));
                        }
                    }
                }
            }
            else if (1960 < Int32.Parse(year) && Int32.Parse(year) < 1970)
            {
                 x = 299 + 40 * getYearPercent(Int32.Parse(year), 1960, 1970);
                for (int i = 0; i < read_genre.Count; i++)
                {
                    if (genre.Contains(read_genre_with_color[i, 0]))
                    {
                        if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                        {
                            pen = new Pen(colors[Int32.Parse(read_genre_with_color[i, 2]) % colors.Count], (float)(i*size));
                            draw_rating(pen, x, graphics, Int32.Parse(rating));
                        }
                    }
                }
            }
            else if (1970 < Int32.Parse(year) && Int32.Parse(year) < 1980)
            {
                 x = 339 + 40 * getYearPercent(Int32.Parse(year), 1970, 1980);
                for (int i = 0; i < read_genre.Count; i++)
                {
                    if (genre.Contains(read_genre_with_color[i, 0]))
                    {
                        if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                        {
                            pen = new Pen(colors[Int32.Parse(read_genre_with_color[i, 2]) % colors.Count], (float)(i*size));
                            draw_rating(pen, x, graphics, Int32.Parse(rating));
                        }
                    }
                }
            }
            else if (1980 < Int32.Parse(year) && Int32.Parse(year) < 1990)
            {
                 x = 379 + 40 * getYearPercent(Int32.Parse(year), 1980, 1990);
                for (int i = 0; i < read_genre.Count; i++)
                {
                    if (genre.Contains(read_genre_with_color[i, 0]))
                    {
                        if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                        {
                            pen = new Pen(colors[Int32.Parse(read_genre_with_color[i, 2]) % colors.Count], (float)(i*size));
                            draw_rating(pen, x, graphics, Int32.Parse(rating));
                        }
                    }
                }
            }
            else if (1990 < Int32.Parse(year) && Int32.Parse(year) < 2000)
            {
                 x = 419 + 40 * getYearPercent(Int32.Parse(year), 1990, 2000);
                for (int i = 0; i < read_genre.Count; i++)
                {
                    if (genre.Contains(read_genre_with_color[i, 0]))
                    {
                        if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                        {
                            pen = new Pen(colors[Int32.Parse(read_genre_with_color[i, 2]) % colors.Count], (float)(i*size));
                            draw_rating(pen, x, graphics, Int32.Parse(rating));
                        }
                    }
                }
            }
            else if (2000 < Int32.Parse(year) && Int32.Parse(year) < 2010)
            {
                 x = 459 + 40 * getYearPercent(Int32.Parse(year), 2000, 2010);
                for (int i = 0; i < read_genre.Count; i++)
                {
                    if (genre.Contains(read_genre_with_color[i, 0]))
                    {
                        if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                        {
                            pen = new Pen(colors[Int32.Parse(read_genre_with_color[i, 2]) % colors.Count], (float)(i*size));
                            draw_rating(pen, x, graphics, Int32.Parse(rating));
                        }
                    }
                }
            }
            else
            {
                 x = 499 + 40 * getYearPercent(Int32.Parse(year),2010,2020);
                for (int i = 0; i < read_genre.Count; i++)
                {
                    if (genre.Contains(read_genre_with_color[i, 0]))
                    {
                        if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                        {
                            pen = new Pen(colors[Int32.Parse(read_genre_with_color[i, 2]) % colors.Count], (float)(i*size));
                            draw_rating(pen, x, graphics, Int32.Parse(rating));
                        }
                    }
                }
            }

        }

        private float getYearPercent(int thisYear, int startYear, int endYear)
        {
            float result = 0.0F;
            float part1 = (endYear - thisYear);
            float part2 = (startYear - endYear);
            result = Math.Abs((10 - part1)/10);

            return result;
        }

        private void draw_rating(Pen pen, double inX, Graphics graphics, int rating)
        {
            int x = Convert.ToInt32(inX);
            int y = 356;
            int range = 30;

            y = y - rating * range;


            if (rating == 0)
            {
                graphics.DrawEllipse(pen, x, y, 2, 2);
            }
            else if (rating == 1)
            {
                graphics.DrawEllipse(pen, x, y, 2, 2);

            }
            else if(rating == 2)
            {
                graphics.DrawEllipse(pen, x, y, 2, 2);

            }
            else if (rating == 3)
            {
                graphics.DrawEllipse(pen, x, y, 2, 2);

            }
            else if (rating == 4)
            {

                graphics.DrawEllipse(pen, x, y, 2, 2);

            }
            else if (rating == 5)
            {

                graphics.DrawEllipse(pen, x, y, 2, 2);

            }
            else if (rating == 6)
            {

                graphics.DrawEllipse(pen, x, y, 2, 2);

            }
            else if (rating == 7)
            {

                graphics.DrawEllipse(pen, x, y, 2, 2);

            }
            else if (rating == 8)
            {

                graphics.DrawEllipse(pen, x, y, 2, 2);

            }
            else if (rating == 9)
            {
                graphics.DrawEllipse(pen,x, y, 2, 2);

            }
            else if (rating == 10)
            {
                graphics.DrawEllipse(pen, x, y, 2, 2);

            }
        }

        private void putDataToCheckBox()
        {
            this.checkedListBox1.Items.Clear();
            if(read_genre_with_color != null)
            {
                for(int i = 0; i<read_genre.Count; i++)
                {
                    this.checkedListBox1.Items.Add(read_genre_with_color[i,0] + " " + read_genre_with_color[i,1]);
                    this.checkedListBox1.SetItemChecked(i, true);
                }
            }
        }

        private void getColorForGenre()
        {
            read_genre_with_color = new string[read_genre.Count,3];
            colors = new List<Color>();

            colors.Add(Color.Red);
            colors.Add(Color.AliceBlue);
            colors.Add(Color.LightSalmon);
            colors.Add(Color.AntiqueWhite);
            colors.Add(Color.LightSeaGreen);
            colors.Add(Color.Aqua);
            colors.Add(Color.LightSkyBlue);
            colors.Add(Color.Aquamarine);
            colors.Add(Color.LightSlateGray);
            colors.Add(Color.Azure);
            colors.Add(Color.LightSteelBlue);
            colors.Add(Color.Beige);
            colors.Add(Color.LightYellow);
            colors.Add(Color.Bisque);
            colors.Add(Color.Lime);
            colors.Add(Color.Black);
            colors.Add(Color.LimeGreen);
            colors.Add(Color.BlanchedAlmond);
            colors.Add(Color.Linen);
            colors.Add(Color.Blue);
            colors.Add(Color.BlueViolet);
            colors.Add(Color.Magenta);
            colors.Add(Color.Maroon);
            colors.Add(Color.Brown);
            colors.Add(Color.MediumAquamarine);
            colors.Add(Color.BurlyWood);
            colors.Add(Color.MediumBlue);
            colors.Add(Color.CadetBlue);
            colors.Add(Color.MediumOrchid);
            colors.Add(Color.Chartreuse);
            colors.Add(Color.MediumPurple);
            colors.Add(Color.Chocolate);
            colors.Add(Color.MediumSeaGreen);
            colors.Add(Color.Coral);
            colors.Add(Color.MediumSlateBlue);
            colors.Add(Color.CornflowerBlue);
            colors.Add(Color.MediumSpringGreen);
            colors.Add(Color.Cornsilk);
            colors.Add(Color.MediumTurquoise);
            colors.Add(Color.Crimson);
            colors.Add(Color.MediumVioletRed);
            colors.Add(Color.Cyan);
            colors.Add(Color.MidnightBlue);
            colors.Add(Color.DarkBlue);


            for(int i = 0; i < read_genre.Count; i++)
            {
                if(read_genre != null)
                {
                    read_genre_with_color[i, 0] = read_genre[i];

                    read_genre_with_color[i, 1] = ("  -   " + colors[i % colors.Count].ToString());
                    read_genre_with_color[i, 2] = (i.ToString());
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.copy_pane();
            for (int i = 0; i <= jj; i++)
            {
                if (read_data[i, 0] != null)
                {
                    draw_graph(read_data[i, 0], read_data[i, 1], read_data[i, 5], read_data[i, 6]);
                    string[] items = new string[8];
                    for (int j = 0; j < 8; j++)
                    {
                        items[j] = this.read_data[i, j];
                    }
                    ListViewItem item = new ListViewItem(items);
                    this.listView2.Items.Add(item);
                }
            }
        }


        private void button8_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.copy_pane();
            for (int i = 0; i <= jj; i++)
            {
                if (read_data[i, 0] != null)
                {
                    draw_graph(read_data[i, 0], read_data[i, 1], read_data[i, 5], read_data[i, 6]);
                }
            }
        }

        private void drawData(char targetBig, char targetLow)
        {
            string tmp1 = "";
            for (int i = 0; i <= jj; i++)
            {
                if (read_data[i, 0] != null)
                {
                    char tmp = read_data[i, 0][0];

                    if (tmp.CompareTo(targetBig) == 0 || tmp.CompareTo(targetLow) == 0)
                    {
                        for (int k = 0; k < read_genre.Count; k++)
                        {
                            if (read_data[i, 6].Contains(read_genre[k]))
                            {
                                if ((checkedListBox1.GetItemCheckState(k) == CheckState.Checked) && (tmp1 != read_data[i,0]))
                                {
                                    tmp1 = read_data[i, 0];
                                    draw_graph(read_data[i, 0], read_data[i, 1], read_data[i, 5], read_data[i, 6]);
                                    string[] items = new string[8];
                                    for (int j = 0; j < 8; j++)
                                    {
                                        items[j] = this.read_data[i, j];
                                    }
                                    ListViewItem item = new ListViewItem(items);
                                    this.listView2.Items.Add(item);
                                }
                            }
                        }

                    }
                }
            }
        }

        private void drawData2(int min, int max)
        {
            string tmp1 = "";
            for (int i = 0; i <= jj; i++)
            {
                if (read_data[i, 0] != null)
                {

                    string string1 = read_data[i, 2];
                    string string2 = " min";

                    string string1_part1 = string1.Substring(0, string1.IndexOf(string2));
                    string string1_part2 = string1.Substring(
                        string1.IndexOf(string2) + string2.Length, string1.Length - (string1.IndexOf(string2) + string2.Length));

                    string1 = string1_part1 + string1_part2;
                    int tmp = Int32.Parse(string1);

                    if (tmp > min && tmp < max)
                    {
                        for (int k = 0; k < read_genre.Count; k++)
                        {
                            if (read_data[i, 6].Contains(read_genre[k]))
                            {
                                if ((checkedListBox1.GetItemCheckState(k) == CheckState.Checked) && (tmp1 != read_data[i, 0]))
                                {
                                    tmp1 = read_data[i, 0];
                                    draw_graph(read_data[i, 0], read_data[i, 1], read_data[i, 5], read_data[i, 6]);
                                    string[] items = new string[8];
                                    for (int j = 0; j < 8; j++)
                                    {
                                        items[j] = this.read_data[i, j];
                                    }
                                    ListViewItem item = new ListViewItem(items);
                                    this.listView2.Items.Add(item);
                                }
                            }
                        }

                    }
                }
            }
        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            viewPanel newView = new viewPanel { MyParentForm1 = this };

            newView.getFilename(globalFilename);
            newView.setLabel1(this.listView2.FocusedItem.SubItems[0].Text);
            newView.setLabel2(this.listView2.FocusedItem.SubItems[1].Text);
            newView.setLabel3(this.listView2.FocusedItem.SubItems[2].Text);
            newView.setLabel4(this.listView2.FocusedItem.SubItems[3].Text);
            newView.setLabel5(this.listView2.FocusedItem.SubItems[4].Text);
            newView.setLabel6(this.listView2.FocusedItem.SubItems[5].Text);
            newView.setLabel7(creatSubLine(this.listView2.FocusedItem.SubItems[6].Text));
            newView.setLabel8(creatSubLine(this.listView2.FocusedItem.SubItems[7].Text));
            newView.showRating(this.listView2.FocusedItem.SubItems[5].Text);
            for (int i = 0; i <= jj; i++)
            {
                if(this.read_data[i,0] != null)
                {
                    if (this.read_data[i, 0].CompareTo(this.listView2.FocusedItem.SubItems[0].Text) == 0)
                    {
                        if (this.read_data[i, 11] != null)
                        {
                            newView.getComment(this.read_data[i, 11]);
                        }
                    }
                }

            }

            newView.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.listView2.Items.Clear();
            this.copy_pane();
            this.listView2.Refresh();
            this.trackBar1.Value = 0;
            this.trackBar2.Value = 0;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            listView2.Items.Clear();

            if (this.trackBar2.Value == 0)
            {
                this.copy_pane();
                drawData2(0, 20);
                this.listView2.Refresh();
            }
            else if (this.trackBar2.Value == 1)
            {
                this.copy_pane();
                drawData2(20, 50);
                this.listView2.Refresh();
            }
            else if (this.trackBar2.Value == 2)
            {
                this.copy_pane();
                drawData2(50, 70);
                this.listView2.Refresh();
            }
            else if (this.trackBar2.Value == 3)
            {
                this.copy_pane();
                drawData2(70, 90);
                this.listView2.Refresh();
            }
            else if (this.trackBar2.Value == 4)
            {
                this.copy_pane();
                drawData2(90, 110);
                this.listView2.Refresh();
            }
            else if (this.trackBar2.Value == 5)
            {
                this.copy_pane();
                drawData2(110, 150);

                this.listView2.Refresh();
            }
            else if (this.trackBar2.Value == 6)
            {
                this.copy_pane();
                drawData2(150, 180);

                this.listView2.Refresh();
            }
            else if (this.trackBar2.Value == 7)
            {
                this.copy_pane();
                drawData2(180, 200);

                this.listView2.Refresh();
            }
            else if (this.trackBar2.Value == 8)
            {
                this.copy_pane();
                drawData2(200, 230);

                this.listView2.Refresh();
            }
            else if (this.trackBar2.Value == 9)
            {
                this.copy_pane();
                drawData2(230, 1000);

                this.listView2.Refresh();
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            switch (this.tabControl1.SelectedIndex)
            {
                case 0:
                    listView2.SelectedItems.Clear();
                    ListView.SelectedIndexCollection c = listView1.SelectedIndices;
                    if (listView1.SelectedItems.Count > 0)
                    {
                        Edit edit = new Edit { MyParentForm3 = this };
                        edit.getFilename(globalFilename);


                        if (listView1.Items[c[0]].SubItems[2].Text.Contains(" min"))
                        {
                            string string1 = listView1.Items[c[0]].SubItems[2].Text;
                            string string2 = " min";

                            string string1_part1 = string1.Substring(0, string1.IndexOf(string2));
                            string string1_part2 = string1.Substring(
                            string1.IndexOf(string2) + string2.Length, string1.Length - (string1.IndexOf(string2) + string2.Length));
                            string1 = string1_part1 + string1_part2;
                            edit.setTextBox2(string1);
                        }

                        edit.setTextBox1(listView1.Items[c[0]].SubItems[0].Text);
                        edit.setComboBox1(listView1.Items[c[0]].SubItems[1].Text);
                        edit.setComboBox2(listView1.Items[c[0]].SubItems[3].Text);
                        edit.setComboBox3(listView1.Items[c[0]].SubItems[5].Text);
                        edit.setTextBox3(listView1.Items[c[0]].SubItems[4].Text);
                        edit.setRichTextBoxString1(listView1.Items[c[0]].SubItems[6].Text);
                        edit.setRichTextBoxString2(listView1.Items[c[0]].SubItems[7].Text);

                        edit.getGenre(read_genre);
                        edit.getActor(read_actor);

                        edit.ShowDialog();
                    }
                    read_records(globalFilename);
                    break;
                case 1:
                    listView1.SelectedItems.Clear();
                    ListView.SelectedIndexCollection c2 = listView2.SelectedIndices;

                    if (listView2.SelectedItems.Count > 0)
                    {
                        Edit edit = new Edit { MyParentForm3 = this };

                        edit.getFilename(globalFilename);


                        if (listView2.Items[c2[0]].SubItems[2].Text.Contains(" min"))
                        {
                            string string1 = listView2.Items[c2[0]].SubItems[2].Text;
                            string string2 = " min";

                            string string1_part1 = string1.Substring(0, string1.IndexOf(string2));
                            string string1_part2 = string1.Substring(
                                string1.IndexOf(string2) + string2.Length, string1.Length - (string1.IndexOf(string2) + string2.Length));

                            string1 = string1_part1 + string1_part2;

                            edit.setTextBox2(string1);

                        }
                        else
                        {
                            edit.setTextBox2(listView2.Items[c2[0]].SubItems[2].Text);
                        }

                        edit.setTextBox1(listView2.Items[c2[0]].SubItems[0].Text);
                        edit.setComboBox1(listView2.Items[c2[0]].SubItems[1].Text);
                        edit.setComboBox2(listView2.Items[c2[0]].SubItems[3].Text);
                        edit.setComboBox3(listView2.Items[c2[0]].SubItems[5].Text);
                        edit.setTextBox3(listView2.Items[c2[0]].SubItems[4].Text);
                        edit.setRichTextBoxString1(listView2.Items[c2[0]].SubItems[6].Text);
                        edit.setRichTextBoxString2(listView2.Items[c2[0]].SubItems[7].Text);

                        edit.getGenre(read_genre);
                        edit.getActor(read_actor);

                        edit.ShowDialog();
                    }
                    read_records(globalFilename);
                    break;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            switch (this.tabControl1.SelectedIndex)
            {
                case 0:
                    DialogResult result1 = MessageBox.Show("Do you want delete this item?","Question",MessageBoxButtons.YesNo);

                    if(result1 == DialogResult.Yes)
                    {
                        listView2.SelectedItems.Clear();

                        ListView.SelectedIndexCollection c = listView1.SelectedIndices;

                        if (listView1.FocusedItem != null)
                        {

                            XDocument xDocument = XDocument.Load(globalFilename);
                            foreach (var profileElement in xDocument.Descendants("movie").ToList())  // Iterates through the collection of "Profile" elements                                                                      // Copies the list (it's needed because we modify it in the foreach (when the element is removed)
                            {
                                if (profileElement.Element("title").Value == listView1.Items[c[0]].SubItems[0].Text)   // Checks the name of the profile
                                {
                                    profileElement.Remove(); // Removes the element
                                }
                            }
                            xDocument.Save(globalFilename);
                        }
                        this.listView1.FocusedItem.Remove();
                    }
                    read_records(globalFilename);
                    break;
                case 1:
                    DialogResult result2 = MessageBox.Show("Do you want delete this item?", "Question", MessageBoxButtons.YesNo);

                    if (result2 == DialogResult.Yes)
                    {
                        listView1.SelectedItems.Clear();

                        ListView.SelectedIndexCollection c2 = listView2.SelectedIndices;

                        if (listView2.FocusedItem != null)
                        {

                            XDocument xDocument = XDocument.Load(globalFilename);
                            foreach (var profileElement in xDocument.Descendants("movie")  // Iterates through the collection of "Profile" elements
                                                                    .ToList())               // Copies the list (it's needed because we modify it in the foreach (when the element is removed)
                            {
                                if (profileElement.Element("title").Value == listView2.Items[c2[0]].SubItems[0].Text)   // Checks the name of the profile
                                {
                                    profileElement.Remove(); // Removes the element
                                }
                            }
                            xDocument.Save(globalFilename);
                        }
                        this.listView2.FocusedItem.Remove();
                    }
                    read_records(globalFilename);
                    break;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("By Yihua huang\nV1.0\nGitHub:https://github.com/yihuashub/MovieFinder");
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
