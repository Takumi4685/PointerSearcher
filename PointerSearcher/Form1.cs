using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PointerSearcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            int maxDepth = 4;
            int maxOffsetNum = 1;
            long maxOffsetAddress = 0x800;
            textBoxDepth.Text = maxDepth.ToString();
            textBoxOffsetNum.Text = maxOffsetNum.ToString();
            textBoxOffsetAddress.Text = maxOffsetAddress.ToString("X");
            buttonSearch.Enabled = false;
            buttonNarrowDown.Enabled = false;
            result = new List<List<IReverseOrderPath>>();
        }
        private PointerInfo info;
        private int maxDepth;
        private int maxOffsetNum;
        private long maxOffsetAddress;
        private Task readTask;
        private Task searchTask;
        private List<List<IReverseOrderPath>> result;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                IDumpDataReader reader = CreateDumpDataReader(dataGridView1.Rows[0]);
                if(reader == null)
                {
                    return;
                }
                readTask = Task.Run(() =>
                {
                    ReadExec(reader);
                });
                buttonSearch.Enabled = false;
                buttonNarrowDown.Enabled = false;
            }
            catch
            {

            }
        }
        private void ReadExec(IDumpDataReader reader)
        {
            PointerInfo tmp = reader.Read();
            ReadEndDelegate de = new ReadEndDelegate(ReadEnd);
            this.Invoke(de, tmp);
        }
        delegate void ReadEndDelegate(PointerInfo result);
        private void ReadEnd(PointerInfo result)
        {
            info = result;
            buttonSearch.Enabled = true;
            System.Media.SystemSounds.Asterisk.Play();
        }
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            maxDepth = Convert.ToInt32(textBoxDepth.Text);
            maxOffsetNum = Convert.ToInt32(textBoxOffsetNum.Text);
            maxOffsetAddress = Convert.ToInt32(textBoxOffsetAddress.Text, 16);
            long heapStart = Convert.ToInt64(dataGridView1.Rows[0].Cells[3].Value.ToString(), 16); //0x4535400000;
            long targetAddress = Convert.ToInt64(dataGridView1.Rows[0].Cells[5].Value.ToString(), 16); //0x45824F6F38;
            Address address = new Address(MemoryType.HEAP, targetAddress - heapStart);

            searchTask = Task.Run(() =>
            {
                SearchExec(info, maxDepth, new List<IReverseOrderPath>(), address);
            });
            textBox1.Text = "";
            result.Clear();
        }
        private void SearchExec(PointerInfo info, int depth, List<IReverseOrderPath> path, Address current)
        {
            Search(info, maxDepth, new List<IReverseOrderPath>(), current);
            SearchEndDelegate de = new SearchEndDelegate(SearchEnd);
            this.Invoke(de);
        }
        delegate void SearchEndDelegate();
        private void SearchEnd()
        {
            PrintPath();
            buttonNarrowDown.Enabled = true;
            System.Media.SystemSounds.Asterisk.Play();
        }

        delegate void AddResultDelegate(List<IReverseOrderPath> path);
        private void AddResult(List<IReverseOrderPath> path)
        {
            result.Add(path);
        }
        private void PrintPath()
        {
            textBox1.Text = "";
            if (result.Count > 100)
            {
                textBox1.Text = "too many results";
            }
            else if (result.Count > 0)
            {
                foreach (List<IReverseOrderPath> path in result)
                {
                    String str = "main";
                    for (int i = path.Count - 1; i >= 0; i--)
                    {
                        str = path[i].ToString(str);
                    }
                    textBox1.Text += str + Environment.NewLine;
                }
            }
            else
            {
                textBox1.Text = "not found";
            }
        }
        private void Search(PointerInfo info, int depth, List<IReverseOrderPath> path, Address current)
        {
            IComparable<Address> icurrent = current;
            int nearest_index = info.FindNearest(icurrent);
            for (int i = nearest_index; (i >= 0) && (i > nearest_index - maxOffsetNum); i--)
            {
                Address nearest = info.pointedList[i].addr;
                long offset = current.OffsetFrom(nearest);
                if (IsLoop(path, nearest) || (offset >= maxOffsetAddress))
                {
                    continue;
                }
                if (offset > 0)
                {
                    ReverseOrderPathOffset add = new ReverseOrderPathOffset(offset);
                    add.addrMemo.Add(0, nearest);
                    path.Add(add);
                }
                foreach (Address next in info.pointedList[i].pointedfrom)
                {
                    if (IsLoop(path, next))
                    {
                        continue;
                    }
                    ReverseOrderPathPointerJump add = new ReverseOrderPathPointerJump();
                    add.addrMemo.Add(0, next);
                    path.Add(add);

                    if (next.type == MemoryType.MAIN)
                    {
                        ReverseOrderPathOffset frommain = new ReverseOrderPathOffset(next.offset);
                        path.Add(frommain);
                        AddResultDelegate de = new AddResultDelegate(AddResult);
                        this.Invoke(de, new List<IReverseOrderPath>(path));
                        path.RemoveAt(path.Count - 1);
                    }
                    else
                    {
                        if (depth > 0)
                        {
                            Search(info, depth - 1, path, next);
                        }
                    }
                    path.RemoveAt(path.Count - 1);
                }
                if (offset > 0)
                {
                    path.RemoveAt(path.Count - 1);
                }
            }
        }
        private bool IsLoop(List<IReverseOrderPath> path, Address checkAddress)
        {
            /*IComparable<Address> iaddress = checkAddress;
            foreach (IReverseOrderPath x in path)
            {
                if (iaddress.CompareTo(x.addrMemo[0]) == 0)
                {
                    return true;
                }
            }*/
            return false;
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = @"";
                //[ファイルの種類]に表示される選択肢を指定する
                //指定しないとすべてのファイルが表示される
                ofd.Filter = "NoexsDumpFile(*.dmp)|*.dmp|All Files(*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.Title = "select Noexs dump file";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ofd.FileName;
                }
            }
        }

        private void buttonNarrowDown_Click(object sender, EventArgs e)
        {
            Dictionary<IDumpDataReader, long> dumps = new Dictionary<IDumpDataReader, long>();
            for (int i = 1; i < dataGridView1.Rows.Count-1; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                IDumpDataReader reader = CreateDumpDataReader(row);
                if( reader != null) {
                    long target = Convert.ToInt64(row.Cells[5].Value.ToString(), 16);

                    dumps.Add(reader, target);
                }
            }
            for (int i = 0; i < result.Count; i++)
            {
                List<IReverseOrderPath> path = result[i];
                foreach (IDumpDataReader dump in dumps.Keys)
                {
                    try
                    {
                        if (dump.TryToParseAbs(path) != dumps[dump])
                        {
                            result.Remove(path);
                            i--;
                            break;
                        }
                    }
                    catch
                    {
                        result.Remove(path);
                        i--;
                        break;
                    }
                }
            }
            PrintPath();
            System.Media.SystemSounds.Asterisk.Play();
        }
        private IDumpDataReader CreateDumpDataReader(DataGridViewRow row)
        {
            bool canCreate = true;
            String path ="";
            long mainStart=-1;
            long mainEnd = -1;
            long heapStart = -1;
            long heapEnd = -1;
            long target = -1;

            for (int i = 0; i <= 5; i++)
            {
                if (row.Cells[i] == null)
                {
                    row.Cells[i].Style.BackColor = Color.Red;
                    canCreate = false;
                }
                else
                {
                    row.Cells[i].Style.BackColor = Color.White;
                }
            }

            if (row.Cells[0].Value !=null ) {
                path = row.Cells[0].Value.ToString();
            }
            if ((path == "") || !System.IO.File.Exists(path))
            {
                row.Cells[0].Style.BackColor = Color.Red;
                canCreate = false;
            }
            try
            {
                mainStart = Convert.ToInt64(row.Cells[1].Value.ToString(), 16);
            }
            catch
            {
                row.Cells[1].Style.BackColor = Color.Red;
                canCreate = false;
            }
            try
            {
                mainEnd = Convert.ToInt64(row.Cells[2].Value.ToString(), 16);
            }
            catch
            {
                row.Cells[2].Style.BackColor = Color.Red;
                canCreate = false;
            }
            try
            {
                heapStart = Convert.ToInt64(row.Cells[3].Value.ToString(), 16);
            }
            catch
            {
                row.Cells[3].Style.BackColor = Color.Red;
                canCreate = false;
            }
            try
            {
                heapEnd = Convert.ToInt64(row.Cells[4].Value.ToString(), 16);
            }
            catch
            {
                row.Cells[4].Style.BackColor = Color.Red;
                canCreate = false;
            }
            try
            {
                target = Convert.ToInt64(row.Cells[5].Value.ToString(), 16);
            }
            catch
            {
                row.Cells[5].Style.BackColor = Color.Red;
                canCreate = false;
            }
            if( !canCreate)
            {
                return null;
            }
            if(mainEnd <= mainStart)
            {
                row.Cells[1].Style.BackColor = Color.Red;
                row.Cells[2].Style.BackColor = Color.Red;
                canCreate = false;
            }
            if (heapEnd <= heapStart)
            {
                row.Cells[3].Style.BackColor = Color.Red;
                row.Cells[4].Style.BackColor = Color.Red;
                canCreate = false;
            }
            if((target < heapStart)||(heapEnd<target))
            {
                row.Cells[5].Style.BackColor = Color.Red;
                canCreate = false;
            }
            if (!canCreate)
            {
                return null;
            }
            return new NoexsDumpDataReader(path, mainStart, mainEnd, heapStart, heapEnd);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            dataGridView1.BeginEdit(true);
        }
    }
}
