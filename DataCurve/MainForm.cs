using LiteDB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using UIResource;

namespace DataCurve
{
    public partial class MainForm : Form
    {
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private Dictionary<string, AxisItem> _dict_Axis = new Dictionary<string, AxisItem>();
        private uint _firedCount = 0;
        private string _dbFilePath = @"DataCurve.db";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            if (System.IO.File.Exists(_dbFilePath))
            {
                System.IO.File.Delete(_dbFilePath);
            }

            dataCurveEx1.SetDynamicCurve(true);
            dataCurveEx1.CreateAxisX(60, 0, Color.Black, 12, "min");
            _dict_Axis.Add("speed01", dataCurveEx1.CreateAxisY(500, 0, Color.Red, 5, "速度01", 1, "ml/min"));
            _dict_Axis.Add("speed02", dataCurveEx1.CreateAxisY(300, 0, Color.Green, 5, "速度02", 1, "ml/min"));
            _dict_Axis.Add("speed03", dataCurveEx1.CreateAxisY(200, 0, Color.Blue, 5, "速度03", 1, "ml/min"));
            _dict_Axis.Add("speed04", dataCurveEx1.CreateAxisY(100, 0, Color.Orange, 5, "速度04", 1, "ml/min"));
            dataCurveEx1.SetBorderLine(1);
            dataCurveEx1.RefreshCurve();

            dataCurveEx1.DataCurveZoomData += OnDataCurveZoomData;
            dataCurveEx1.DataCurveMarkLine += OnDataCurveMarkLine;
            dataCurveEx1.DataCurveOutputMessage += OnDataCurveOutputMessage;

            timer1.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dataCurveEx1.Release();
            timer1.Stop();
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            Random random = new Random();
            float speed01 = (float)(random.NextDouble() * 500);
            float speed02 = (float)(random.NextDouble() * 300);
            float speed03 = (float)(random.NextDouble() * 200);
            float speed04 = (float)(random.NextDouble() * 100);

            var dictItem = new Dictionary<int, float>();
            dictItem[_dict_Axis["speed01"]._id] = speed01;
            dictItem[_dict_Axis["speed02"]._id] = speed02;
            dictItem[_dict_Axis["speed03"]._id] = speed03;
            dictItem[_dict_Axis["speed04"]._id] = speed04;
            dataCurveEx1.AddCurveData(dictItem);

            string labelName = "";
            if (0 == _firedCount % 60)
            {
                int length = 6;
                StringBuilder result = new StringBuilder(length);
                for (int i = 0; i < length; i++)
                {
                    result.Append(_chars[random.Next(_chars.Length)]);
                }
                labelName = result.ToString();
                dataCurveEx1.AddDataLabel((float)Math.Ceiling(_firedCount / 60.0), labelName);
            }
            dataCurveEx1.RefreshCurve();

            using (var db = new LiteDatabase(_dbFilePath))
            {
                var col = db.GetCollection<DataModel>("datas");
                var dataModel = new DataModel()
                {
                    Speed01 = speed01,
                    Speed02 = speed01,
                    Speed03 = speed01,
                    Speed04 = speed01,
                    Second = _firedCount,
                    LabelName = labelName
                };
                col.EnsureIndex(x => x.Id, true);
                col.Insert(dataModel);
            }

            _firedCount++;
        }

        private Dictionary<int, List<float>> OnDataCurveZoomData(int minIndex, int maxIndex, int step)
        {
            Dictionary<int, List<float>> dictResults = new Dictionary<int, List<float>>();
            using (var db = new LiteDatabase(@"DataCurve.db"))
            {
                var cols = db.GetCollection<DataModel>("datas");
                var query = cols.Find(x => x.Second > minIndex && x.Second < maxIndex);
                foreach (var item in query)
                {
                    if (0 == item.Second % step)
                    {
                        AddSpeedToDictionary(new List<float> { item.Speed01 }, "speed01", dictResults);
                        AddSpeedToDictionary(new List<float> { item.Speed02 }, "speed02", dictResults);
                        AddSpeedToDictionary(new List<float> { item.Speed03 }, "speed03", dictResults);
                        AddSpeedToDictionary(new List<float> { item.Speed04 }, "speed04", dictResults);
                    }
                }
            }
            return dictResults;
        }

        private (float, List<float>) OnDataCurveMarkLine(float posX)
        {
            float newPosX = 0;
            List<float> lstPosY = new List<float>();

            int second = (int)Math.Round(posX * 60.0F);
            using (var db = new LiteDatabase(@"DataCurve.db"))
            {
                var cols = db.GetCollection<DataModel>("datas");
                var query = cols.FindOne(x => x.Second == second);
                if (null != query)
                {
                    newPosX = query.Second / 60.0F;
                    lstPosY.Add(query.Speed01);
                    lstPosY.Add(query.Speed02);
                    lstPosY.Add(query.Speed03);
                    lstPosY.Add(query.Speed04);
                }
            }
            return (newPosX, lstPosY);
        }

        private void OnDataCurveOutputMessage(string message)
        {
            if (this.InvokeRequired)
            {
                Action<string> action = new Action<string>(OnDataCurveOutputMessage);
                this.Invoke(action, message);
            }
            else
            {
                MessageBox.Show(message);
            }
        }

        private void AddSpeedToDictionary(List<float> speeds, string axisKey, Dictionary<int, List<float>> dictResults)
        {
            int key = _dict_Axis[axisKey]._id;
            if (dictResults.ContainsKey(key))
            {
                dictResults[key].AddRange(speeds);
            }
            else
            {
                dictResults.Add(key, new List<float>(speeds));
            }
        }

        private void menuItemRestore_Click(object sender, EventArgs e)
        {
            dataCurveEx1.RestoreAxis();
        }

        private void menuItemMarker_Click(object sender, EventArgs e)
        {
            bool showMarkLine = !menuItemMarker.Checked;
            dataCurveEx1.ShowMaker(showMarkLine);
            menuItemMarker.Checked = showMarkLine;
        }

        private void menuItemCopy_Click(object sender, EventArgs e)
        {
            dataCurveEx1.CopyToClipboard();
        }

        private void menuItemPrint_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
            PrintDialog printDialog = new PrintDialog
            {
                Document = printDocument
            };
            if (DialogResult.OK == printDialog.ShowDialog())
            {
                printDocument.Print();
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            dataCurveEx1.DrawGraph(e.Graphics, e.PageBounds);
        }
    }
}
