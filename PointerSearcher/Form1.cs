using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
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
            buttonCancel.Enabled = false;
            progressBar1.Maximum = 100;

            result = new List<List<IReverseOrderPath>>();
        }
        private PointerInfo info;
        private int maxDepth;
        private int maxOffsetNum;
        private long maxOffsetAddress;
        private List<List<IReverseOrderPath>> result;
        private CancellationTokenSource cancel = null;
        private double progressTotal;
        private async void buttonRead_Click(object sender, EventArgs e)
        {
            SetProgressBar(0);
            try
            {
                buttonRead.Enabled = false;


                IDumpDataReader reader = CreateDumpDataReader(dataGridView1.Rows[0]);
                if(reader == null)
                {
                    throw new Exception("Invalid input" + Environment.NewLine + "Check highlighted cell");
                }
                buttonSearch.Enabled = false;
                buttonNarrowDown.Enabled = false;
                buttonCancel.Enabled = true;

                cancel = new CancellationTokenSource();
                Progress<int> prog = new Progress<int>(SetProgressBar);

                info = await Task.Run(() => reader.Read(cancel.Token, prog));

                SetProgressBar(100);
                System.Media.SystemSounds.Asterisk.Play();

                buttonSearch.Enabled = true;
            }
            catch(System.OperationCanceledException ex)
            {
                SetProgressBar(0);
                System.Media.SystemSounds.Asterisk.Play();
            }
            catch (Exception ex)
            {
                SetProgressBar(0);
                MessageBox.Show("Read Failed" + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (cancel != null)
                {
                    cancel.Dispose();
                }

                buttonCancel.Enabled = false;
                buttonRead.Enabled = true;
            }
        }
        private async void buttonSearch_Click(object sender, EventArgs e)
        {
            result.Clear();
            textBox1.Text = "";
            buttonRead.Enabled = false;
            buttonSearch.Enabled = false;
            buttonNarrowDown.Enabled = false;
            SetProgressBar(0);
            try
            {
                maxDepth = Convert.ToInt32(textBoxDepth.Text);
                maxOffsetNum = Convert.ToInt32(textBoxOffsetNum.Text);
                maxOffsetAddress = Convert.ToInt32(textBoxOffsetAddress.Text, 16);
                long heapStart = Convert.ToInt64(dataGridView1.Rows[0].Cells[3].Value.ToString(), 16);
                long targetAddress = Convert.ToInt64(dataGridView1.Rows[0].Cells[5].Value.ToString(), 16);
                Address address = new Address(MemoryType.HEAP, targetAddress - heapStart);

                if (maxOffsetNum <= 0)
                {
                    MessageBox.Show("Offset Num must be greater than 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (maxOffsetAddress < 0)
                {
                    MessageBox.Show("Offset Range must be greater or equal to 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else {
                    buttonCancel.Enabled = true;
                    
                    cancel = new CancellationTokenSource();
                    Progress<double> prog = new Progress<double>(AddProgressBar);

                    FindPath find = new FindPath(maxOffsetNum, maxOffsetAddress);

                    await Task.Run(() =>
                    {
                        find.Search(cancel.Token, prog,100.0, info, maxDepth, new List<IReverseOrderPath>(), address, result);
                    });

                    SetProgressBar(100);
                    PrintPath();
                    System.Media.SystemSounds.Asterisk.Play();

                    buttonNarrowDown.Enabled = true;
                }
            }
            catch (System.OperationCanceledException ex)
            {
                SetProgressBar(0);
                System.Media.SystemSounds.Asterisk.Play();
            }
            catch (Exception ex)
            {
                SetProgressBar(0);
                MessageBox.Show("Read Failed" + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                buttonCancel.Enabled = false;
                cancel.Dispose();
            }

            buttonRead.Enabled = true;
            buttonSearch.Enabled = true;
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

        private async void buttonNarrowDown_Click(object sender, EventArgs e)
        {

            try
            {
                SetProgressBar(0);
                Dictionary<IDumpDataReader, long> dumps = new Dictionary<IDumpDataReader, long>();
                for (int i = 1; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];
                    ClearRowBackColor(row);
                    if ( IsBlankRow(row))
                    {
                        continue;
                    }
                    IDumpDataReader reader = CreateDumpDataReader(row);
                    if (reader != null)
                    {
                        long target = Convert.ToInt64(row.Cells[5].Value.ToString(), 16);

                        dumps.Add(reader, target);
                    }
                }
                if (dumps.Count == 0)
                {
                    throw new Exception("Fill out 2nd line to narrow down");
                }
                buttonRead.Enabled = false;
                buttonSearch.Enabled = false;
                buttonNarrowDown.Enabled = false;
                buttonCancel.Enabled = true;

                cancel = new CancellationTokenSource();
                Progress<int> prog = new Progress<int>(SetProgressBar);

                List<List<IReverseOrderPath>> copyList = new List<List<IReverseOrderPath>>(result);

                result = await Task.Run(() => FindPath.NarrowDown(cancel.Token, prog, result, dumps));

                PrintPath();
                SetProgressBar(100);
                System.Media.SystemSounds.Asterisk.Play();
            }
            catch (System.OperationCanceledException ex)
            {
                SetProgressBar(0);
                System.Media.SystemSounds.Asterisk.Play();
            }
            catch (Exception ex)
            {
                SetProgressBar(0);
                MessageBox.Show(Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (cancel != null)
                {
                    cancel.Dispose();
                }

                buttonRead.Enabled = true;
                buttonSearch.Enabled = true;
                buttonNarrowDown.Enabled = true;
                buttonCancel.Enabled = false;
            }
        }
        private bool IsBlankRow(DataGridViewRow row)
        {
            for (int i = 0; i <= 5; i++)
            {
                if (row.Cells[i].Value == null)
                {
                    continue;
                }
                if (row.Cells[i].Value.ToString() != "")
                {
                    return false;
                }
            }
            return true;
        }
        private void ClearRowBackColor(DataGridViewRow row)
        {
            for (int i = 0; i <= 5; i++)
            {
                row.Cells[i].Style.BackColor = Color.White;
            }
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
        private void SetProgressBar(int percent)
        {
            progressBar1.Value = percent;
            progressTotal = percent;
        }
        private void AddProgressBar(double percent)
        {
            progressTotal += percent;
            if(progressTotal> 100)
            {
                progressTotal = 100;
            }
            progressBar1.Value = (int)progressTotal;
        }

        private void buttonCancel_Click_1(object sender, EventArgs e)
        {
            if (cancel != null)
            {
                cancel.Cancel();
            }
        }
    }
}
