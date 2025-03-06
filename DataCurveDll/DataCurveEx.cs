using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace UIResource
{
    /// <summary>
    /// 字体风格
    /// </summary>
    class FontFamily
    {
        public const string YaHei = "微软雅黑";
        public const string SimSun = "新宋体";
    }

    enum MenuItem
    {
        Restore = 0,
        MarkLine = 1
    }

    /// <summary>
    /// 坐标轴项
    /// </summary>
    public class AxisItem
    {
        public AxisItem()
        {
            _id = -1;
            _name = String.Empty;
            _label = String.Empty;
            _is_vertical = true;
        }

        /// <summary>
        /// 序号
        /// </summary>
        public int _id { get; set; }

        /// <summary>
        /// 名称GUID
        /// </summary>
        public string _name { get; set; }

        /// <summary>
        /// 坐标轴标记名称
        /// </summary>
        public string _label { get; set; }

        /// <summary>
        /// 是否为垂直轴
        /// </summary>
        public bool _is_vertical { get; set; }

        /// <summary>
        /// 坐标轴单位
        /// </summary>
        public string _unit { get; set; }

        /// <summary>
        /// 无效的坐标轴
        /// </summary>
        /// <returns> 无效返回true，有效返回false </returns>
        public bool IsInvalid()
        {
            if (-1 == _id || String.IsNullOrEmpty(_name))
                return true;

            return false;
        }

        /// <summary>
        /// 用于自定义类型的查找
        /// </summary>
        /// <param name="obj"> 类对象 </param>
        /// <returns> 找到返回true，否则返回false </returns>
        public override bool Equals(object obj)
        {
            if (obj is AxisItem property && property._name == this._name)
                return true;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }
    }

    /// <summary>
    /// 标题信息
    /// </summary>
    internal class DataTitle
    {
        public DataTitle()
        {
            _name = String.Empty;
            _color = Color.Black;
            _font = new Font(FontFamily.YaHei, 15);
        }

        /// <summary>
        /// 标题文本
        /// </summary>
        public string _name { get; set; }

        /// <summary>
        /// 标题颜色
        /// </summary>
        public Color _color { get; set; }

        /// <summary>
        /// 标题字体
        /// </summary>
        public Font _font { get; set; }
    }

    /// <summary>
    /// 数据项
    /// </summary>
    internal class DataItem
    {
        public DataItem()
        {
            _data = null;
            _line_thickness = 1.0f;
            _is_visible = true;
            _data_size = 0;
            _is_filter = false;
        }

        /// <summary>
        /// 线条粗细
        /// </summary>
        public float _line_thickness { get; set; }

        /// <summary>
        /// 线条颜色
        /// </summary>
        // public Color _line_color { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool _is_visible { get; set; }

        /// <summary>
        /// 坐标轴名称
        /// </summary>
        public string _axis_name { get; set; }

        /// <summary>
        /// 曲线数据
        /// </summary>
        public float[] _data;

        /// <summary>
        /// 记录真实数据大小
        /// </summary>
        public ulong _data_size { get; set; }

        /// <summary>
        /// 是否进行滤波
        /// </summary>
        public bool _is_filter { get; set; }
    }

    internal class DataLabel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string _name { get; set; }

        /// <summary>
        /// 数值
        /// </summary>
        public float _value { get; set; }
    }

    /// <summary>
    /// 标签项
    /// </summary>
    internal class LabelItem
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string _name { get; set; }

        /// <summary>
        /// 父容器
        /// </summary>
        public FlowLayoutPanel flow_layout_panel = null;

        /// <summary>
        /// 控制框
        /// </summary>
        public LinkLabel _link_label_ctrl = null;

        /// <summary>
        /// 标题文本
        /// </summary>
        public LinkLabel _link_label_title = null;
    }

    /// <summary>
    /// 曲线项
    /// </summary>
    internal class DataLabelStyle : IDisposable
    {
        public DataLabelStyle()
        {
            _color = Color.Orange;
            _thickness = 1;
            _height = 16;
            _pen = new Pen(_color);
            _brush = new SolidBrush(_color);
        }

        /// <summary>
        /// 颜色
        /// </summary>
        public Color _color { get; set; }

        /// <summary>
        /// 线条粗细
        /// </summary>
        public float _thickness { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public int _height { get; set; }

        /// <summary>
        /// 文本画笔
        /// </summary>
        public Pen _pen { get; set; }

        /// <summary>
        /// 文本画刷
        /// </summary>
        public Brush _brush { get; set; }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    _pen?.Dispose();
                    _brush.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                disposedValue = true;
            }
        }

        /// <summary>
        /// 释放内存信息
        /// </summary>
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }


    /// <summary>
    /// 坐标轴基类
    /// </summary>
    internal class AxisBase
    {
        public AxisBase()
        {
            _name = String.Empty;
            _max_value = 100.0f;
            _min_value = 0.0f;
            _segment = 20;
            _color = Color.Black;
            _font = new Font(FontFamily.YaHei, 9);
        }

        public AxisBase(string name, float max, float min, int seg, Color clr)
        {
            _name = name;
            _max_value = max;
            _min_value = min;
            _segment = seg;
            _color = clr;
            _font = new Font(FontFamily.YaHei, 9);
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string _name { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public float _max_value { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public float _min_value { get; set; }

        /// <summary>
        /// 分隔
        /// </summary>
        public int _segment { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public Color _color { get; set; }

        /// <summary>
        /// 文本字体
        /// </summary>
        public Font _font { get; set; }
    }

    /// <summary>
    /// 坐标轴缩放信息
    /// </summary>
    internal class AxisZoom : AxisBase
    {
        public AxisZoom()
        {
            _max_zoom_value = _max_value;
            _min_zoom_value = _min_value;
        }

        public AxisZoom(string name, float max, float min, int seg, Color clr)
            : base(name, max, min, seg, clr)
        {
            _max_zoom_value = max;
            _min_zoom_value = min;
        }

        /// <summary>
        /// 改变坐标轴数据
        /// </summary>
        /// <param name="cur_rect"> 当前选择的范围 </param>
        /// <param name="totel_w"> 总宽度 </param>
        /// <param name="totel_h"> 总高度 </param>
        /// <param name="sp_lr"> 左右留白 </param>
        /// <param name="sp_ud"> 上下留白 </param>
        public virtual void ChangeValue(Rectangle cur_rect, int totel_w, int totel_h, int sp_lr, int sp_ud)
        {

        }

        /// <summary>
        /// 还原坐标系
        /// </summary>
        public virtual void Restore()
        {

        }

        /// <summary>
        /// 重置坐标系
        /// </summary>
        public virtual void Reset()
        {

        }

        public virtual void SetZoomValue(int value)
        {
            _max_zoom_value = value;
            _update = true;
        }

        public virtual void CheckValue(ref float max, ref float min)
        {
            if (Math.Abs(max - min) <= 0.00001)
            {
                max += 1;
            }
        }

        public virtual void Update()
        {

        }

        protected int SubtractOrZero(int a, int b)
        {
            int result = a - b;
            return result < 0 ? 0 : result;
        }

        /// <summary>
        /// 最大缩放值
        /// </summary>
        public float _max_zoom_value;

        /// <summary>
        /// 最小缩放值
        /// </summary>
        public float _min_zoom_value;

        public bool _update { get; set; }
    }

    /// <summary>
    /// X坐标轴信息
    /// </summary>
    internal class AxisX : AxisZoom
    {
        public AxisX(string name, float max, float min, int seg, Color clr)
            : base(name, max, min, seg, clr)
        {
            _cur_max_value = max;
            _cur_min_value = min;
        }

        public override void ChangeValue(Rectangle cur_rect, int totel_w, int totel_h, int sp_lr, int sp_ud)
        {
            int begin = Math.Min(cur_rect.X, cur_rect.X + cur_rect.Width); // X轴开始点坐标
            int end = Math.Max(cur_rect.X + cur_rect.Width, cur_rect.X); // X轴结束点坐标

            float scale = (_cur_max_value - _cur_min_value) / (totel_w - sp_lr * 2); // 计算比例尺
            float cur_max = (end - sp_lr) * scale + _cur_min_value;
            float cur_min = SubtractOrZero(begin, sp_lr) * scale + _cur_min_value;

            _cur_max_value = (float)Math.Ceiling(cur_max);
            _cur_min_value = (float)Math.Floor(cur_min);
        }

        public override void Restore()
        {
            _cur_max_value = _max_zoom_value;
            _cur_min_value = _min_zoom_value;
        }

        public override void Reset()
        {
            _cur_max_value = _max_value;
            _cur_min_value = _min_value;

            _max_zoom_value = _max_value;
            _min_zoom_value = _min_value;
        }

        public override void Update()
        {
            if (_update)
            {
                _cur_max_value = _max_zoom_value;
                _update = false;
            }
        }

        /// <summary>
        /// 当前最大值
        /// </summary>
        public float _cur_max_value { get; set; }

        /// <summary>
        /// 当前最小值
        /// </summary>
        public float _cur_min_value { get; set; }
    }

    /// <summary>
    /// Y坐标轴信息
    /// </summary>
    internal class AxisY : AxisZoom
    {
        public AxisY(string name, float max, float min, int seg, Color clr)
            : base(name, max, min, seg, clr)
        {
            _cur_max_value = max;
            _cur_min_value = min;
        }

        public override void ChangeValue(Rectangle cur_rect, int totel_w, int totel_h, int sp_lr, int sp_ud)
        {
            int begin = Math.Min(cur_rect.Y, cur_rect.Y + cur_rect.Height); // Y轴开始点坐标
            int end = Math.Max(cur_rect.Y + cur_rect.Height, cur_rect.Y); // Y轴结束点坐标

            float scale = (_cur_max_value - _cur_min_value) / (totel_h - sp_ud * 2); // 计算比例尺
            float cur_max = (totel_h - begin - sp_ud) * scale + _cur_min_value;
            float cur_min = SubtractOrZero(totel_h, (end + sp_ud)) * scale + _cur_min_value;

            _cur_max_value = (float)Math.Ceiling(cur_max);
            _cur_min_value = (float)Math.Floor(cur_min);

            base.CheckValue(ref _cur_max_value, ref _cur_min_value);
        }

        public override void Restore()
        {
            _cur_max_value = _max_zoom_value;
            _cur_min_value = _min_zoom_value;
        }

        public override void Reset()
        {
            _cur_max_value = _max_value;
            _cur_min_value = _min_value;
        }

        /// <summary>
        /// 当前最大值
        /// </summary>
        public float _cur_max_value;

        /// <summary>
        /// 当前最小值
        /// </summary>
        public float _cur_min_value;
    }

    /// <summary>
    /// 动态标签
    /// </summary>
    internal class DyncmicLabel
    {
        public DyncmicLabel()
        {

        }

        public Label Create(int x, int y)
        {
            if (null == _label_ctrl)
                _label_ctrl = new Label();

            _label_ctrl.Width = 125;
            _label_ctrl.Font = new Font(FontFamily.YaHei, 9);
            _label_ctrl.TextAlign = ContentAlignment.MiddleCenter;
            // _label_ctrl.BackColor = Color.PowderBlue;
            _label_ctrl.BorderStyle = BorderStyle.FixedSingle;
            _label_ctrl.Location = new Point(x, y);
            _label_ctrl.Visible = false;

            return _label_ctrl;
        }

        public void Show()
        {
            do
            {
                if (null == _label_ctrl)
                    break;

                _label_ctrl.Visible = true;
            } while (false);
        }

        public void Hide()
        {
            do
            {
                if (null == _label_ctrl)
                    break;

                _label_ctrl.Text = String.Empty;
                _label_ctrl.Visible = false;
            } while (false);
        }

        public void SetText(string text)
        {
            do
            {
                if (null == _label_ctrl)
                    break;

                _label_ctrl.Text = text;
            } while (false);
        }

        public void SetTextColor(Color clr)
        {
            _label_ctrl.ForeColor = clr;
        }

        public Label _label_ctrl = null;
    }

    public partial class DataCurveEx : UserControl
    {
        public delegate Dictionary<int, List<float>> DataCurveZoomDataEvent(int min, int max, int step);
        public DataCurveZoomDataEvent DataCurveZoomData = null;

        public delegate (float, List<float>) DataCurveMarkLineEvent(float x);
        public DataCurveMarkLineEvent DataCurveMarkLine = null;

        public delegate void DataCurveOutputMessageEvent(string message);
        public DataCurveOutputMessageEvent DataCurveOutputMessage = null;

        public DataCurveEx()
        {
            InitializeComponent();
            InitializeDataCurve();
        }

        public void Release()
        {
            _toolTip?.Dispose();
        }

        public void RefreshCurve()
        {
            if (!_mouse_is_down)
            {
                DrawToBuffer();
            }
        }

        public void RestoreAxis()
        {
            // 还原坐标系
            foreach (var item in _dict_curve_axis_x)
            {
                item.Value.Restore();
            }

            foreach (var item in _dict_curve_axis_y)
            {
                item.Value.Restore();
            }

            // 清空缓存数据
            _dict_data_cache_items.Clear();
            _zoom_rect = System.Drawing.Rectangle.Empty;

            RefreshCurve();
        }

        public void ResetAxis()
        {
            // 重置坐标系
            foreach (var item in _dict_curve_axis_x)
            {
                item.Value.Reset();
            }

            foreach (var item in _dict_curve_axis_y)
            {
                item.Value.Reset();
            }

            // 清空缓存数据
            _dict_data_cache_items.Clear();
            _zoom_rect = System.Drawing.Rectangle.Empty;
        }

        public void ShowMaker(bool show)
        {
            if (null == _mark_line)
            {
                _mark_line = new Panel();
                _mark_line.Location = new Point(0, MAX_SPACE_UP_DOWN - 10);
                _mark_line.Width = 2;
                _mark_line.Height = this.Height - 2 * MAX_SPACE_UP_DOWN + 20;
                _mark_line.Visible = false;
                _mark_line.MouseDown += new MouseEventHandler(Line_MouseDown);
                _mark_line.MouseMove += new MouseEventHandler(Line_MouseMove);
                _mark_line.MouseUp += new MouseEventHandler(Line_MouseUp);
                _mark_line.MouseEnter += new EventHandler(Line_MouseEnter);
                _mark_line.MouseLeave += new EventHandler(Line_MouseLeave);
                this.Controls.Add(_mark_line);
            }

            // 如果需要显示标记线
            if (show)
            {
                // 获取鼠标位置
                Point pt = this.PointToClient(Control.MousePosition);
                _mark_line.Location = new Point(pt.X, MAX_SPACE_UP_DOWN - 10);

                // 获取当前 Y 轴颜色
                AxisY axis = null;
                if (GetCurAxisY(_cur_axis_y, out axis))
                {
                    _mark_line.BackColor = axis._color;
                }
                else
                {
                    _mark_line.BackColor = Color.Crimson; // 默认颜色
                }
            }
            _mark_line.Visible = show;

            // 隐藏工具提示
            _toolTip.Hide(this);
        }

        public void CopyToClipboard()
        {
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));
            Clipboard.SetImage(bitmap);
            bitmap.Dispose();
        }

        public void DrawGraph(Graphics g, Rectangle pageBounds)
        {
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            bitmap.SetResolution(800, 1024);
            this.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            float pageWidth = pageBounds.Width;
            float pageHeight = pageBounds.Height;
            float aspectRatioBitmap = (float)bitmap.Width / bitmap.Height;
            float aspectRatioPage = pageWidth / pageHeight;

            float drawWidth, drawHeight;
            if (aspectRatioBitmap > aspectRatioPage)
            {
                drawWidth = pageWidth;
                drawHeight = pageWidth / aspectRatioBitmap;
            }
            else
            {
                drawHeight = pageHeight;
                drawWidth = pageHeight * aspectRatioBitmap;
            }

            float drawX = (pageWidth - drawWidth) / 2;
            float drawY = (pageHeight - drawHeight) / 2;
            g.DrawImage(bitmap, drawX, drawY, drawWidth, drawHeight);
            bitmap.Dispose();
        }

        public void SetCurAxisX(AxisItem axisItem)
        {
            _cur_axis_x = axisItem;
        }

        public void SetCurAxisY(AxisItem axisItem)
        {
            _cur_axis_y = axisItem;
        }

        public AxisItem CreateAxisX(float max, float min, Color clr, int seg, string unit = "min")
        {
            _dict_curve_axis_x.Clear(); // 当前需求，仅有一个X坐标轴
            AxisItem item = new AxisItem()
            {
                _id = _dict_curve_axis_x.Count,
                _name = Guid.NewGuid().ToString(""),
                _is_vertical = false,
                _unit = unit
            };

            if (_cur_axis_x.IsInvalid())
                _cur_axis_x = item;

            AxisX axis = new AxisX(item._name, max, min, seg, clr);
            _dict_curve_axis_x.Add(item, axis);

            return item;
        }

        public AxisItem CreateAxisY(float max, float min, Color clr, int seg, string label, float thickness = 1, string unit = "", bool filter = false, bool visible = true)
        {
            AxisItem item = new AxisItem()
            {
                _id = _dict_curve_axis_y.Count,
                _name = Guid.NewGuid().ToString("N"),
                _label = label,
                _is_vertical = false,
                _unit = unit
            };

            if (_cur_axis_y.IsInvalid())
                _cur_axis_y = item;

            FilterFactory factory = CreateDataFilter(filter);
            if (null != factory)
                _dict_filter.Add(item._name, factory.CreateFilter());

            CreateAxisLabel(item, clr, ref visible);

            AxisY axis = new AxisY(item._name, max, min, seg, clr);
            _dict_curve_axis_y.Add(item, axis);
            _dict_data_items.Add(item._id, new DataItem { _axis_name = item._name, _data = new float[] { }, _is_filter = filter, _line_thickness = thickness, _is_visible = visible });

            return item;
        }

        public void UpdateZoomAxisX(float max, float min)
        {
            if (0 == _dict_curve_axis_x.Count)
                return;

            AxisX axis = null;
            if (!GetCurAxisX(_cur_axis_x, out axis))
                return;

            axis._cur_max_value = max;
            axis._cur_min_value = min;
            _dict_data_cache_items.Clear();

            ReloadCurveData(_dict_data_cache_items);
        }

        public void SetBorderLine(float thickness)
        {
            _border_thickness = thickness;
        }

        public void UpdateAxisY(AxisItem item, float max, float min, Color clr, bool visible = true)
        {
            AxisY axis = null;
            if (!GetCurAxisY(item, out axis))
                return;

            axis._min_value = min;
            axis._max_value = max;
            axis._cur_min_value = min;
            axis._cur_max_value = max;
            axis._min_zoom_value = min;
            axis._max_zoom_value = max;
            axis._color = clr;

            if (_dict_label_item.ContainsKey(item._id))
            {
                _dict_label_item[item._id].flow_layout_panel.Visible = visible;
                _dict_label_item[item._id]._link_label_ctrl.BackColor = clr;
                _dict_label_item[item._id]._link_label_title.LinkColor = clr;
            }

            if (_dict_data_items.ContainsKey(item._id))
            {
                _dict_data_items[item._id]._is_visible = visible;
            }
        }

        public void SetCurveTitle(string title)
        {
            _data_title._name = title;
        }

        public void SetDynamicCurve(bool b)
        {
            _is_dynamic_curve = b;
        }

        public void SetCurveVisible(AxisItem item, bool visible)
        {
            Dictionary<int, DataItem> tmp_data_items = 0 == _dict_data_cache_items.Count ? _dict_data_items : _dict_data_cache_items;

            DataItem data_item = null;
            tmp_data_items.TryGetValue(item._id, out data_item);
            if (null != data_item)
                data_item._is_visible = visible;

            RefreshCurve();
        }

        public void ResetCurve()
        {
            foreach (var it in _dict_data_items)
            {
                it.Value._data = new float[] { };
                it.Value._data_size = 0;
            }

            foreach (var it in _dict_data_cache_items)
            {
                it.Value._data = new float[] { };
                it.Value._data_size = 0;
            }

            _lst_data_label.Clear();
            _start_time = 0;
            ResetAxis();
        }

        public int GetVisibleCount()
        {
            return _dict_data_items.Count(p => p.Value._is_visible);
        }

        public void SetVisibleCount(int count)
        {
            _max_visible_count = count;
        }


        public void AddDataLabel(float pos, string name)
        {
            if (String.IsNullOrEmpty(name))
                return;

            if (0 == Math.Sign(pos))
                pos += 0.05f;

            DataLabel label = new DataLabel()
            {
                _name = name,
                _value = pos
            };
            _lst_data_label.Add(label);
        }

        public void AddCurveData(Dictionary<int, float> dict)
        {
            AddData(dict);
        }

        public void SetStartTimestamp(long timestamp)
        {
            _start_time = timestamp;
        }
        private void AddData(Dictionary<int, float> dict)
        {
            foreach (var it in dict)
            {
                int id = it.Key;
                float data = it.Value;

                if (!_dict_data_items.ContainsKey(id))
                    continue;

                DataItem item = null;
                _dict_data_items.TryGetValue(id, out item);
                if (null == item || null == item._data)
                    continue;

                AxisX axis = null;
                _dict_curve_axis_x.TryGetValue(_cur_axis_x, out axis);
                if (null == axis)
                    continue;

                // 1. 构建绘图数据
                if (item._data.Length < MAX_CURVE_POINT)
                {
                    int min_index = (int)(axis._min_zoom_value * 60);
                    int max_index = (int)(axis._max_zoom_value * 60);
                    int interval_point = (int)Math.Ceiling(((max_index - min_index) * 1.0f) / (MAX_CURVE_POINT * 1.0f));

                    if (0 == item._data_size % (ulong)(interval_point))
                        BasicFramework.AddArrayData(ref item._data, new float[] { data }, MAX_CURVE_POINT);
                }
                else
                {
                    axis.SetZoomValue((int)(axis._max_zoom_value * 2));
                    // if (_is_dynamic_curve) // 是否动态曲线
                    // {
                    //     ReloadCurveData(_dict_data_items, true);
                    // }
                    // else
                    {
                        ResetCurveData(_dict_data_items, true);
                    }
                }
                item._data_size++; // 记录一下真实的数据长度

                // 2. 构建缓存数据
                DataItem item_chche = null;
                _dict_data_cache_items.TryGetValue(id, out item_chche);
                if (null != item_chche && null != item_chche._data)
                {
                    int min_index = (int)(axis._cur_min_value * 60);
                    int max_index = (int)(axis._cur_max_value * 60);
                    int interval_point = (int)Math.Ceiling((Math.Abs(max_index - min_index) * 1.0f) / (MAX_CURVE_POINT * 1.0f));
                    int max_count = Math.Abs(max_index - min_index);
                    max_count /= interval_point;

                    if ((ulong)min_index <= item._data_size && item_chche._data.Length < max_count)
                    {
                        if (0 == item_chche._data_size % (ulong)(interval_point))
                            BasicFramework.AddArrayData(ref item_chche._data, new float[] { data }, max_count);
                    }
                    item_chche._data_size++;
                    continue;
                }

                axis.Update();
            }
        }

        private void InitializeComponent()
        {
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Load += new EventHandler(this.DataCurveEx_OnLoad);
            this.Paint += new PaintEventHandler(this.DataCurveEx_Paint);
            this.MouseMove += new MouseEventHandler(this.DataCurveEx_MouseMove);
            this.MouseDown += new MouseEventHandler(this.DataCurveEx_MouseDown);
            this.MouseUp += new MouseEventHandler(this.DataCurveEx_MouseUp);
        }

        private void InitializeDataCurve()
        {
            // 剧中对齐文本布局
            _format_center = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center,
            };

            // 右对齐文本布局
            _format_right = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Far,
            };

            // 垂直对齐文本布局
            _format_vertical = new StringFormat
            {
                FormatFlags = StringFormatFlags.DirectionVertical
            };

            _format_left = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near,
            };

            // 默认开启双缓存
            if (_is_double_cache_graphics)
            {
                this.DoubleBuffered = true;
                this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
                this.UpdateStyles();
            }

            if (null == _data_title)
                _data_title = new DataTitle();

            if (null == _cur_axis_x)
                _cur_axis_x = new AxisItem();

            if (null == _cur_axis_y)
                _cur_axis_y = new AxisItem();

            if (null == _dict_curve_axis_x)
                _dict_curve_axis_x = new Dictionary<AxisItem, AxisX>();

            if (null == _dict_curve_axis_y)
                _dict_curve_axis_y = new Dictionary<AxisItem, AxisY>();

            if (null == _dict_data_items)
                _dict_data_items = new Dictionary<int, DataItem>();

            if (null == _dict_data_cache_items)
                _dict_data_cache_items = new Dictionary<int, DataItem>();

            if (null == _lst_data_label)
                _lst_data_label = new List<DataLabel>();

            if (null == _data_label_style)
                _data_label_style = new DataLabelStyle();

            if (null == _flowLayoutPanel)
                _flowLayoutPanel = new FlowLayoutPanel();

            if (null == _dynamic_label)
                _dynamic_label = new DyncmicLabel();

            if (null == _dict_label_item)
                _dict_label_item = new Dictionary<int, LabelItem>();

            if (null == _toolTip)
            {
                _toolTip = new ToolTip();
                _toolTip.ShowAlways = true;
                _toolTip.UseFading = true;
                _toolTip.UseAnimation = true;
            }
        }

        private void OnMenuItemRestore_Click(object sender, EventArgs e)
        {
            RestoreAxis();
        }

        private void OnMenuItemMarkLine_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            bool showMarkLine = !item.Checked;
            ShowMaker(showMarkLine);
            item.Checked = showMarkLine;
        }

        private void DataCurveEx_OnLoad(object sender, EventArgs e)
        {
            _flowLayoutPanel.Padding = new Padding(10, 0, 0, 0);
            this.Controls.Add(_flowLayoutPanel);
            PanelResize();

            this.Resize += new EventHandler(this.DataCurveEx_Resize);

            _context = BufferedGraphicsManager.Current;
            _context.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            _grafx = _context.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));
            _bitmap = new Bitmap(this.Width, this.Height);

            SetBackColor(_grafx.Graphics);
        }

        private void DataCurveEx_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                _mutex_lock.WaitOne();
                _grafx.Render(e.Graphics);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            finally
            {
                _mutex_lock.ReleaseMutex();
            }
        }

        private void DataCurveEx_Resize(object sender, EventArgs e)
        {
            PanelResize();
            LineResize();
            ResetGraphics();
            DrawToBuffer();
        }

        private void DataCurveEx_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouse_is_down)
            {
                Rectangle chatRect = new Rectangle(this.Location.X + MAX_SPACE_LEFT_RIGHT, this.Location.Y + MAX_SPACE_UP_DOWN, this.Width, this.Height);
                using (Graphics g = Graphics.FromHwnd(this.Handle))
                {
                    Rectangle rect = this.CalcZoomRect(this._selection_start, this._selection_end);
                    SelectionBox_Draw(g, this.BackColor, rect);

                    _selection_end = Point.Round(this.BoundPointToRect(new Point(e.X, e.Y), chatRect));
                    rect = this.CalcZoomRect(this._selection_start, this._selection_end);
                    SelectionBox_Draw(g, this.BackColor, rect);
                }
            }
        }

        private void SelectionBox_Draw(Graphics g, Color backgroundColor, Rectangle rectangle)
        {
            RasterOperation mode;
            Color alternateColor;
            if (backgroundColor.GetBrightness() < 0.5)
            {
                mode = RasterOperation.NOTXORPEN;
                alternateColor = Color.White;
            }
            else
            {
                mode = RasterOperation.XORPEN;
                alternateColor = Color.Black;
            }

            var hdc = g.GetHdc();

            try
            {
                IntPtr pen = DataCurveFrame.CreatePen((int)PenStyle.Dot, 1, ColorTranslator.ToWin32(backgroundColor));

                int previousMode = DataCurveFrame.SetROP2(new HandleRef(null, hdc), (int)mode);
                IntPtr previousBrush = DataCurveFrame.SelectObject(new HandleRef(null, hdc), new HandleRef(null, DataCurveFrame.GetStockObject((int)StockObject.NullBrush)));
                IntPtr previousPen = DataCurveFrame.SelectObject(new HandleRef(null, hdc), new HandleRef(null, pen));
                DataCurveFrame.SetBkColor(new HandleRef(null, hdc), ColorTranslator.ToWin32(alternateColor));

                DataCurveFrame.Rectangle(new HandleRef(null, hdc), rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);

                DataCurveFrame.SetROP2(new HandleRef(null, hdc), previousMode);
                DataCurveFrame.SelectObject(new HandleRef(null, hdc), new HandleRef(null, previousBrush));
                DataCurveFrame.SelectObject(new HandleRef(null, hdc), new HandleRef(null, previousPen));

                if (IntPtr.Zero != pen)
                {
                    DataCurveFrame.DeleteObject(new HandleRef(null, pen));
                }
            }
            finally
            {
                g.ReleaseHdc();
            }
        }

        private void DataCurveEx_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Location.X < MAX_SPACE_LEFT_RIGHT - 10 || e.Location.Y < MAX_SPACE_UP_DOWN || e.Location.Y > this.Height - MAX_SPACE_UP_DOWN)
                return;

            if (MouseButtons.Left == e.Button)
            {
                _mouse_is_down = true;
                if (_is_can_zoom)
                {
                    this.Cursor = Cursors.SizeAll;
                    _selection_start = new Point(e.X, e.Y);
                    _selection_end = _selection_start;
                    _selection_end.Offset(1, 1);
                }
            }
        }

        private void DataCurveEx_MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseButtons.Left == e.Button)
            {
                HandleZoomData();
                _mouse_is_down = false;
                this.Cursor = Cursors.Default;
                _selection_start = Point.Empty;
                _selection_end = Point.Empty;
                RefreshCurve();
            }
        }

        private void HandleZoomData()
        {
            if (Math.Abs(_selection_end.X - _selection_start.X) < 10 && Math.Abs(_selection_end.Y - _selection_start.Y) < 10)
                return;

            // 处理坐标系
            {
                _zoom_rect = new Rectangle(
                    Math.Min(_selection_start.X, _selection_end.X),
                    Math.Min(_selection_start.Y, _selection_end.Y),
                    Math.Abs(_selection_start.X - _selection_end.X),
                    Math.Abs(_selection_start.Y - _selection_end.Y));

                foreach (var item in _dict_curve_axis_x)
                {
                    item.Value.ChangeValue(_zoom_rect, this.Width, this.Height, MAX_SPACE_LEFT_RIGHT, MAX_SPACE_UP_DOWN);
                }
                foreach (var item in _dict_curve_axis_y)
                {
                    item.Value.ChangeValue(_zoom_rect, this.Width, this.Height, MAX_SPACE_LEFT_RIGHT, MAX_SPACE_UP_DOWN);
                }
            }

            // 处理数据
            {
                _dict_data_cache_items.Clear();
                ReloadCurveData(_dict_data_cache_items);
            }
        }

        private bool GetCurAxisX(AxisItem item, out AxisX axis)
        {
            bool ret = false;

            do
            {
                axis = null;

                if (item.IsInvalid())
                    break;

                if (!_dict_curve_axis_x.ContainsKey(item))
                    break;

                axis = _dict_curve_axis_x[item];
                ret = true;
            } while (false);

            return ret;
        }

        private bool GetCurAxisY(AxisItem item, out AxisY axis)
        {
            bool ret = false;

            do
            {
                axis = null;

                if (item.IsInvalid())
                    break;

                if (!_dict_curve_axis_y.ContainsKey(item))
                    break;

                axis = _dict_curve_axis_y[item];
                ret = true;
            } while (false);

            return ret;
        }

        private void CreateAxisLabel(AxisItem item, Color clr, ref bool visible)
        {
            LinkLabel link_label_ctrl = new LinkLabel()
            {
                Text = " ",
                AutoSize = true,
                LinkBehavior = LinkBehavior.NeverUnderline,
                BackColor = clr,
                Tag = item._id,
                Font = new Font(FontFamily.SimSun, 10)
            };
            link_label_ctrl.Click += new EventHandler(LinkLabelCtrl_Click);

            LinkLabel link_label_title = new LinkLabel()
            {
                Text = item._label,
                AutoSize = true,
                LinkBehavior = LinkBehavior.NeverUnderline,
                LinkColor = clr,
                ActiveLinkColor = Color.FromArgb(0xFF - clr.R, 0xFF - clr.G, 0xFF - clr.B),
                Tag = item._id,
                Font = new Font(FontFamily.YaHei, 9)
            };
            link_label_title.Click += new EventHandler(LinkLabelTitle_Click);

            if (_max_visible_count - 1 < GetVisibleCount())
                visible = false;

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Padding = new Padding(-10, 0, 0, 0);
            panel.AutoSize = true;
            panel.Controls.Add(link_label_ctrl);
            panel.Controls.Add(link_label_title);
            panel.Visible = visible;
            _flowLayoutPanel.Controls.Add(panel);

            LabelItem label = new LabelItem
            {
                _name = item._name,
                flow_layout_panel = panel,
                _link_label_ctrl = link_label_ctrl,
                _link_label_title = link_label_title
            };
            _dict_label_item.Add(item._id, label);
        }

        private void LinkLabelTitle_Click(object sender, EventArgs e)
        {
            LinkLabel ctrl = sender as LinkLabel;
            do
            {
                int axis_id = (int)ctrl.Tag;

                AxisItem axis = null;
                foreach (var item in _dict_curve_axis_y)
                {
                    if (item.Key._id == axis_id)
                    {
                        axis = item.Key;
                        if (null != _mark_line)
                        {
                            _toolTip.Hide(this);
                            _mark_line.BackColor = item.Value._color;
                        }
                        break;
                    }
                }
                if (null != axis)
                {
                    _cur_axis_y = axis;
                }

                RefreshCurve();
            } while (false);
        }

        private void LinkLabelCtrl_Click(object sender, EventArgs e)
        {
            LinkLabel ctrl = sender as LinkLabel;

            do
            {
                int axis_id = (int)ctrl.Tag;
                if (!_dict_data_items.ContainsKey(axis_id))
                    break;

                DataItem item = null;
                _dict_data_items.TryGetValue(axis_id, out item);

                AxisY axis = null;
                if (!_dict_curve_axis_y.TryGetValue(new AxisItem { _id = axis_id, _name = item._axis_name }, out axis))
                    break;

                bool showData = !item._is_visible;
                if (showData && _max_visible_count - 1 < GetVisibleCount())
                {
                    DataCurveOutputMessage?.BeginInvoke("可见数量大于设定值", null, null);
                    break;
                }

                item._is_visible = showData;
                if (item._is_visible)
                    ctrl.BackColor = axis._color;
                else
                    ctrl.BackColor = Color.DimGray;

                if (_dict_data_cache_items.ContainsKey(axis_id))
                {
                    _dict_data_cache_items.TryGetValue(axis_id, out item);
                    item._is_visible = !item._is_visible;
                }

                RefreshCurve();
            } while (false);
        }

        private void ReloadCurveData(Dictionary<int, DataItem> dict, bool auto_zoom = false)
        {
            if (!CalcZoomPos(auto_zoom, out int min_index, out int max_index, out int interval_point))
                return;

            Dictionary<int, List<float>> dict_caches = DataCurveZoomData?.Invoke(min_index, max_index, interval_point);
            if (null == dict_caches || 0 == dict_caches.Count)
                return;

            foreach (var item in _dict_data_items)
            {
                if (!dict.ContainsKey(item.Key))
                {
                    dict.Add(
                        item.Key,
                        new DataItem()
                        {
                            _line_thickness = item.Value._line_thickness,
                            _is_visible = item.Value._is_visible,
                            _axis_name = item.Value._axis_name,
                            _data = dict_caches[item.Key].ToArray(),
                            _data_size = (ulong)dict_caches[item.Key].Count
                        });
                }
                else
                {
                    item.Value._data = dict_caches[item.Key].ToArray();
                    item.Value._data_size = (ulong)dict_caches[item.Key].Count;
                }
            }
        }

        private void ResetCurveData(Dictionary<int, DataItem> dict, bool auto_zoom = false)
        {
            if (!CalcZoomPos(auto_zoom, out int min_index, out int max_index, out int interval_point))
                return;

            foreach (var kvp in dict)
            {
                DataItem item = kvp.Value;
                if (null != item._data && item._data.Length > 0)
                {
                    float[] data = item._data.Where((value, index) => index % 2 == 0).ToArray();
                    item._data = data;
                }
            }
        }

        private bool CalcZoomPos(bool auto_zoom, out int min_index, out int max_index, out int interval_point)
        {
            min_index = 0;
            max_index = 0;
            interval_point = 0;

            AxisX axis = null;
            if (!GetCurAxisX(_cur_axis_x, out axis))
                return false;

            min_index = (int)((auto_zoom ? axis._min_zoom_value : axis._cur_min_value) * 60.0f);
            max_index = (int)((auto_zoom ? axis._max_zoom_value : axis._cur_max_value) * 60.0f);
            interval_point = (int)Math.Ceiling(((max_index - min_index) * 1.0f) / (MAX_CURVE_POINT * 1.0f));
            return true;
        }

        private void DrawToBuffer()
        {
            if (0 == this.Width || 0 == this.Height)
                return;

            try
            {
                if (null == _bitmap || _bitmap.Width != this.Width || _bitmap.Height != this.Height)
                {
                    _bitmap = new Bitmap(this.Width, this.Height);
                }

                using (Graphics g = Graphics.FromImage(_bitmap))
                {
                    SetBackColor(g);
                    DrawToBuffer(g);
                }

                _mutex_lock.WaitOne();
                try
                {
                    if (null != _grafx)
                    {
                        _grafx.Graphics.DrawImage(_bitmap, 0, 0);
                        _grafx.Render();
                    }
                }
                finally
                {
                    _mutex_lock.ReleaseMutex();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private void SetBackColor(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            g.Clear(BackColor);
        }

        private void ResetGraphics()
        {
            if (this.Width < 120 || this.Height < 60)
                return;

            try
            {
                _mutex_lock.WaitOne();

                if (_grafx == null || _grafx.Graphics.VisibleClipBounds.Size != new Size(this.Width, this.Height))
                {
                    _context.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                    _grafx?.Dispose();
                    _grafx = _context.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            finally
            {
                _mutex_lock.ReleaseMutex();
            }
        }

        private void DrawToBuffer(Graphics g)
        {
            // Stopwatch sw = new Stopwatch();
            // sw.Start();

            DrawCurveTitle(g);
            DrawCurveAxisX(g);
            DrawCurveAxisY(g);

            Dictionary<int, DataItem> dict_data = null;
            if (System.Drawing.Rectangle.Empty != _zoom_rect)
                dict_data = _dict_data_cache_items;
            else
                dict_data = _dict_data_items;
            DrawCurveData(g, dict_data);

            DrawCurveLabel(g, _lst_data_label, 0 != _dict_data_cache_items.Count);

            // sw.Stop();
        }

        private void DrawCurveTitle(Graphics g)
        {
            if (String.IsNullOrEmpty(_data_title._name))
                return;

            int totel_width = this.Width;
            if (!string.IsNullOrEmpty(_data_title._name))
            {
                using (Brush brush = new SolidBrush(_data_title._color))
                {
                    g.DrawString(_data_title._name, _data_title._font, brush, new Rectangle(0, 2, totel_width - 1, 20), _format_center);
                }
            }
        }

        private void DrawCurveAxisX(Graphics g)
        {
            AxisX axis = null;
            if (!GetCurAxisX(_cur_axis_x, out axis))
                return;

            int totalWidth = this.Width;
            int totalHeight = this.Height;

            // 创建画笔和画刷
            using (Pen pen = new Pen(axis._color, _border_thickness))
            using (Brush brush = new SolidBrush(axis._color))
            {
                // 计算极轴的起始和结束点
                Point axisStartPoint = new Point(MAX_SPACE_LEFT_RIGHT - 1, totalHeight - MAX_SPACE_UP_DOWN);
                Point axisEndPoint = new Point(totalWidth - MAX_SPACE_LEFT_RIGHT + 8, totalHeight - MAX_SPACE_UP_DOWN);

                // 绘制极轴
                g.DrawLines(pen, new Point[] { axisStartPoint, axisEndPoint });

                // 绘制X坐标轴三角形
                BasicFramework.PaintTriangle(g, brush, axisEndPoint, 4, BasicFramework.GraphDirection.Rightward);

                // 绘制单位
                SizeF unitSize = g.MeasureString(_cur_axis_x._unit, axis._font);
                RectangleF unitRect = new RectangleF(
                    totalWidth - MAX_SPACE_LEFT_RIGHT + 8,
                    totalHeight - MAX_SPACE_UP_DOWN,
                    MAX_SPACE_LEFT_RIGHT + unitSize.Width,
                    unitSize.Height);
                g.DrawString(_cur_axis_x._unit, axis._font, brush, unitRect, _format_left);

                int curSegment = axis._segment;
                float curMaxVal = axis._cur_max_value;
                float curMinVal = axis._cur_min_value;

                DateTime prevDateTime = DateTime.Now;
                // 计算刻度线的位置
                for (int i = 0; i <= curSegment; i++)
                {
                    float axisVal = i * (curMaxVal - curMinVal) / curSegment + curMinVal;
                    float paintPos = BasicFramework.ComputePaintLocationY(curMaxVal, curMinVal, (totalWidth - 2 * MAX_SPACE_LEFT_RIGHT), axisVal);

                    if (float.IsNaN(paintPos))
                        continue;

                    // 计算刻度文本的值
                    string textVal = "";
                    if (0 == _start_time)
                    {
                        textVal = ((float)Math.Round(axisVal, 2)).ToString();
                    }
                    else
                    {
                        long startTime = (long)(_start_time + (axisVal * 60 * 1000));
                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(startTime);
                        DateTime localDateTime = dateTimeOffset.LocalDateTime;
                        if (0 == i || prevDateTime.Date != localDateTime.Date)
                            textVal = localDateTime.ToString("yyyy-MM-dd HH:mm");
                        else
                            textVal = localDateTime.ToString("HH:mm");
                        prevDateTime = localDateTime;
                    }
                    SizeF textSize = g.MeasureString(textVal, axis._font);

                    // 计算刻度文本的位置
                    float textX = totalWidth - MAX_SPACE_LEFT_RIGHT - paintPos - textSize.Width / 2;
                    float textY = totalHeight - MAX_SPACE_UP_DOWN + 8;

                    // 绘制刻度线和刻度文本
                    g.DrawLine(pen, MAX_SPACE_LEFT_RIGHT + paintPos, totalHeight - MAX_SPACE_UP_DOWN, MAX_SPACE_LEFT_RIGHT + paintPos, totalHeight - MAX_SPACE_UP_DOWN + 4);
                    g.DrawString(textVal, axis._font, brush, textX, textY);
                }
            }
        }

        private void DrawCurveAxisY(Graphics g)
        {
            AxisY axis = null;
            if (!GetCurAxisY(_cur_axis_y, out axis))
                return;

            int totalHeight = this.Height;

            // 创建画笔和画刷
            using (Pen pen = new Pen(axis._color, _border_thickness))
            using (Brush brush = new SolidBrush(axis._color))
            {
                // 计算极轴的起始和结束点
                Point axisStartPoint = new Point(MAX_SPACE_LEFT_RIGHT - 1, MAX_SPACE_UP_DOWN - 8);
                Point axisEndPoint = new Point(MAX_SPACE_LEFT_RIGHT - 1, totalHeight - MAX_SPACE_UP_DOWN);

                // 绘制极轴
                g.DrawLines(pen, new Point[] { axisStartPoint, axisEndPoint });

                // 绘制纵坐标轴三角形
                BasicFramework.PaintTriangle(g, brush, axisStartPoint, 4, BasicFramework.GraphDirection.Upward);

                // 绘制单位
                SizeF unitSize = g.MeasureString(_cur_axis_y._unit, axis._font);
                RectangleF unitRect = new RectangleF(
                    MAX_SPACE_LEFT_RIGHT - unitSize.Width,
                    MAX_SPACE_UP_DOWN - 12 - unitSize.Height,
                    unitSize.Width,
                    unitSize.Height);
                g.DrawString(_cur_axis_y._unit, axis._font, brush, unitRect, _format_left);

                int curSegment = axis._segment;
                float curMaxVal = axis._cur_max_value;
                float curMinVal = axis._cur_min_value;

                // 计算刻度线的位置，以及绘制刻度线和刻度文本
                for (int i = 0; i <= curSegment; i++)
                {
                    float axisVal = i * (curMaxVal - curMinVal) / curSegment + curMinVal;
                    float paintPos = BasicFramework.ComputePaintLocationY(curMaxVal, curMinVal, (totalHeight - 2 * MAX_SPACE_UP_DOWN), axisVal) + MAX_SPACE_UP_DOWN;

                    // 绘制刻度线
                    g.DrawLine(pen, MAX_SPACE_LEFT_RIGHT - 4, paintPos, MAX_SPACE_LEFT_RIGHT - 1, paintPos);

                    // 绘制刻度文本
                    string textVal = ((float)Math.Round(axisVal, 2)).ToString();
                    SizeF textSize = g.MeasureString(textVal, axis._font);
                    RectangleF textRect = new RectangleF(0, paintPos - 9, MAX_SPACE_LEFT_RIGHT - 4, 20);
                    g.DrawString(textVal, axis._font, brush, textRect, _format_right);
                }
            }
        }

        private void DrawCurveData(Graphics g, Dictionary<int, DataItem> data_items)
        {
            int totle_width = this.Width;
            int totle_height = this.Height;
            int valid_width = totle_width - 2 * MAX_SPACE_LEFT_RIGHT;
            int valid_height = totle_height - 2 * MAX_SPACE_UP_DOWN;

            // 设置有效绘图范围
            RectangleF rc_range = new RectangleF(MAX_SPACE_LEFT_RIGHT, MAX_SPACE_UP_DOWN, valid_width, valid_height);
            g.SetClip(rc_range);

            // 绘制曲线
            foreach (var item in data_items.Values)
            {
                if (!item._is_visible)
                    continue;

                AxisY axis_y = null;
                if (!GetAxisY(item._axis_name, out axis_y))
                    continue;

                AxisX axis_x = null;
                if (!GetCurAxisX(_cur_axis_x, out axis_x))
                    continue;

                FilterAlgorithm filter = null;
                _dict_filter.TryGetValue(item._axis_name, out filter);
                filter?.Initialize();

                float cur_min_x = axis_x._cur_min_value;
                float cur_max_x = axis_x._cur_max_value;
                int max_point = Math.Abs((int)((cur_max_x - cur_min_x) * 60));
                max_point /= (int)Math.Ceiling((max_point * 1.0f) / (MAX_CURVE_POINT * 1.0f));

                if (MAX_CURVE_POINT < max_point)
                    max_point = MAX_CURVE_POINT;

                float[] tmp_data = item._data;
                if (1 < tmp_data.Length)
                {
                    float step_pt = (valid_width * 1.0f) / (((max_point < tmp_data.Length ? tmp_data.Length : max_point) - 1) * 1.0f);

                    PointF[] points = new PointF[tmp_data.Length];
                    for (int i = 0; i < tmp_data.Length; i++)
                    {
                        points[i].X = (MAX_SPACE_LEFT_RIGHT * 1.0f) + (step_pt * i * 1.0f);
#if DATA_FILTER
                        if (item._is_filter && null != filter)
                        {
                            points[i].Y = BasicFramework.ComputePaintLocationY(
                                            axis_y._cur_max_value,
                                            axis_y._cur_min_value,
                                            valid_height,
                                            (float)filter.Filter(tmp_data[i]));
                        }
                        else
                        {
                            points[i].Y = BasicFramework.ComputePaintLocationY(
                                            axis_y._cur_max_value,
                                            axis_y._cur_min_value,
                                            valid_height,
                                            tmp_data[i]);
                        }
#else 
                        points[i].Y = BasicFramework.ComputePaintLocationY(
                                        axis_y._cur_max_value,
                                        axis_y._cur_min_value,
                                        valid_height,
                                        tmp_data[i]);
#endif
                        points[i].Y += MAX_SPACE_UP_DOWN;
                    }

                    // Stopwatch sw = new Stopwatch();
                    // sw.Start();

                    using (Pen pen = new Pen(axis_y._color, item._line_thickness))
                    {
                        if (_is_smooth_curve)
                            g.DrawCurve(pen, points);
                        else
                            g.DrawLines(pen, points);
                    }
                    points = null;

                    // sw.Stop();
                    // Trace.WriteLine($"Draw curve：{sw.ElapsedMilliseconds} ms.");
                }
            }
        }

        private void DrawSelectionBox(Graphics g)
        {
            if (!_mouse_is_down)
                return;

            // Stopwatch sw = new Stopwatch();
            // sw.Start();

            int x = Math.Min(_selection_start.X, _selection_end.X);
            int y = Math.Min(_selection_start.Y, _selection_end.Y);
            int width = Math.Abs(_selection_start.X - _selection_end.X);
            int height = Math.Abs(_selection_start.Y - _selection_end.Y);
            Rectangle currentRegion = new Rectangle(x, y, width, height);
            using (Pen pen = new Pen(Color.Black, 1) { DashStyle = DashStyle.Dash })
            {
                g.DrawRectangle(pen, currentRegion);
            }

            // sw.Stop();
            // Trace.WriteLine($"Draw box：{sw.ElapsedMilliseconds} ms.");
        }

        private void DrawCurveLabel(Graphics g, List<DataLabel> data_label, bool is_big_font)
        {
            if (0 == data_label.Count)
                return;

            AxisX axis = null;
            if (!GetCurAxisX(_cur_axis_x, out axis))
                return;

            int totalWidth = this.Width;
            int totalHeight = this.Height;
            int axisWidth = totalWidth - 2 * MAX_SPACE_LEFT_RIGHT;

            Font font = is_big_font ? new Font(FontFamily.YaHei, 16, FontStyle.Bold) : new Font(FontFamily.YaHei, 10, FontStyle.Bold);

            float curMaxValue = axis._cur_max_value;
            float curMinValue = axis._cur_min_value;

            foreach (var item in data_label)
            {
                if (item._value <= curMinValue || item._value >= curMaxValue)
                    continue;

                float printPos = BasicFramework.ComputePaintLocationY(curMaxValue, curMinValue, axisWidth, item._value);
                float realPrintPos = axisWidth - printPos;

                if (realPrintPos < 0)
                    continue;

                using (Pen pen = new Pen(_data_label_style._color, _data_label_style._thickness))
                {
                    float labelHeight = _data_label_style._height;
                    float labelX = MAX_SPACE_LEFT_RIGHT + realPrintPos;
                    float labelY = totalHeight - MAX_SPACE_UP_DOWN - labelHeight;

                    g.DrawLine(pen, labelX, totalHeight - MAX_SPACE_UP_DOWN, labelX, labelY);

                    SizeF textSize = g.MeasureString(item._name, font, font.Height, _format_vertical);
                    RectangleF textRect = new RectangleF(labelX - textSize.Width / 2, labelY - textSize.Height, textSize.Width, textSize.Height);
                    g.DrawString(item._name, font, _data_label_style._brush, textRect, _format_vertical);
                }
            }
        }

        private bool GetAxisY(string name, out AxisY axis)
        {
            AxisItem item = new AxisItem()
            {
                _id = 0xFF,
                _name = name
            };
            return GetCurAxisY(item, out axis);
        }

        private FilterFactory CreateDataFilter(bool filter)
        {
            FilterFactory factory = null;

            do
            {
                if (!filter) break;
                switch (_filter_type)
                {
                    case FilterType.F_First:
                        factory = new FirstOrderFactory();
                        break;
                    case FilterType.F_Kalman:
                        factory = new KalmanFactory();
                        break;
                    case FilterType.F_Limit:
                        factory = new LimitFactory();
                        break;
                    default:
                        break;
                }
            } while (false);

            return factory;
        }

        private Rectangle CalcZoomRect(Point point1, Point point2)
        {
            Size size = new Size(point2.X - point1.X, point2.Y - point1.Y);
            Rectangle rect = new Rectangle(point1, size);

            return rect;
        }

        private PointF BoundPointToRect(Point mousePt, RectangleF rect)
        {
            PointF newPt = new PointF(mousePt.X, mousePt.Y);

            if (mousePt.X < rect.X) newPt.X = rect.X;
            if (mousePt.X > rect.Right) newPt.X = rect.Right;
            if (mousePt.Y < rect.Y) newPt.Y = rect.Y;
            if (mousePt.Y > rect.Bottom) newPt.Y = rect.Bottom;

            return newPt;
        }

        private void Line_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseButtons.Left == e.Button)
            {
                _mark_line_dragging = true;
                _mark_line_offset = e.Location;
                Cursor.Current = Cursors.SizeWE;
            }
        }

        private void Line_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mark_line_dragging)
            {
                Control control = sender as Control;
                int new_x = control.Left + (e.X - _mark_line_offset.X);
                new_x = Math.Max(MAX_SPACE_LEFT_RIGHT, new_x); // 左边界
                new_x = Math.Min(control.Parent.ClientSize.Width - control.Width - MAX_SPACE_LEFT_RIGHT, new_x); // 右边界
                control.Left = new_x;
                control.Parent.Refresh();

                AxisX axis_x = null;
                if (GetCurAxisX(_cur_axis_x, out axis_x))
                {
                    int valid_width = this.Width - 2 * MAX_SPACE_LEFT_RIGHT;
                    float cur_pos_x = BasicFramework.ComputeRealValueFromLoaction(
                                            axis_x._cur_max_value,
                                            axis_x._cur_min_value,
                                            valid_width,
                                            control.Left - MAX_SPACE_LEFT_RIGHT);

                    AxisY axis_y = null;
                    if (GetCurAxisY(_cur_axis_y, out axis_y))
                    {
                        if (null == DataCurveMarkLine)
                            return;

                        (float valueX, List<float> lstValueY) = DataCurveMarkLine.Invoke(cur_pos_x);
                        float cur_pos_y = 0;
                        if (0 != lstValueY.Count && _cur_axis_y._id < lstValueY.Count)
                        {
                            cur_pos_y = lstValueY[_cur_axis_y._id];
                        }
                        Point pt = new Point(_mark_line.Location.X, _mark_line.Location.Y - _toolTip.GetToolTip(this).Length);
                        if (0 == _start_time)
                        {
                            _toolTip.Show(String.Format("X: {0:f2}, Y: {1:f2}", cur_pos_x, cur_pos_y), this, pt);
                        }
                        else
                        {
                            long startTime = (long)(_start_time + (cur_pos_x * 60 * 1000));
                            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(startTime);
                            DateTime localDateTime = dateTimeOffset.LocalDateTime;
                            _toolTip.Show(String.Format("X: {0:f2}, Y: {1:f2}", localDateTime.ToString("yyyy-MM-dd HH:mm:ss"), cur_pos_y), this, pt);
                        }
                    }
                }
            }
        }

        private void Line_MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseButtons.Left == e.Button)
            {
                _mark_line_dragging = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private void Line_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.SizeWE;
        }

        private void Line_MouseLeave(object sender, EventArgs e)
        {
            if (!_mark_line_dragging)
                Cursor.Current = Cursors.Default;
        }

        private void ToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            e.DrawBorder();
            using (System.Drawing.StringFormat sf = new System.Drawing.StringFormat())
            {
                sf.Alignment = System.Drawing.StringAlignment.Near;
                sf.LineAlignment = System.Drawing.StringAlignment.Center;
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                sf.FormatFlags = System.Drawing.StringFormatFlags.NoWrap;
                sf.Trimming = System.Drawing.StringTrimming.None;
                using (System.Drawing.Font f = new System.Drawing.Font("微软雅黑", 12))
                {
                    TextRenderer.DrawText(e.Graphics, e.ToolTipText, f, e.Bounds, System.Drawing.Color.Black, TextFormatFlags.ExternalLeading);
                }
            }
        }

        private void ToolTip_Popup(object sender, PopupEventArgs e)
        {
            System.Drawing.Font f1 = new System.Drawing.Font("黑体", 12);
            System.Drawing.Size proposedSize = new System.Drawing.Size(int.MaxValue, int.MaxValue);
            e.ToolTipSize = TextRenderer.MeasureText(_toolTip.GetToolTip(e.AssociatedControl), f1, proposedSize, TextFormatFlags.ExternalLeading);
        }

        private void LineResize()
        {
            if (null != _mark_line && _mark_line.Visible)
            {
                _mark_line.Height = this.Height - 2 * MAX_SPACE_UP_DOWN + 20;
                _toolTip.Hide(this);
            }
        }

        private void PanelResize()
        {
            if (null != _flowLayoutPanel)
            {
                _flowLayoutPanel.Location = new Point(MAX_SPACE_LEFT_RIGHT, 25);
                _flowLayoutPanel.Width = this.Width - MAX_SPACE_LEFT_RIGHT;
                _flowLayoutPanel.Height = 0;
                _flowLayoutPanel.AutoSize = true;
            }
        }



        private float _border_thickness = 1.0f; // 边框线条
        private bool _is_can_zoom = true;

        private StringFormat _format_center = null; // 剧中文本布局
        private StringFormat _format_right = null; // 右对齐文本布局
        private StringFormat _format_left = null; // 左对齐文本布局
        private StringFormat _format_vertical = null; // 垂直对齐文本布局

        private bool _is_smooth_curve = false; // 平滑曲线
        private bool _is_double_cache_graphics = true; // 双缓存
        private bool _is_dynamic_curve = false; // 动态曲线

        private bool _mouse_is_down = false; // 鼠标是否按下
        private System.Drawing.Rectangle _zoom_rect = System.Drawing.Rectangle.Empty; // 缩放范围

        private DataTitle _data_title = null; // 曲线标题
        private AxisItem _cur_axis_x = null; // 保存当前的X轴信息
        private AxisItem _cur_axis_y = null; // 保存当前的Y轴信息
        private Dictionary<AxisItem, AxisX> _dict_curve_axis_x = null; // 保存创建的X轴信息
        private Dictionary<AxisItem, AxisY> _dict_curve_axis_y = null; // 保存创建的y轴信息
        private Dictionary<int, DataItem> _dict_data_items = null; // 待显示的曲线数据
        private Dictionary<int, DataItem> _dict_data_cache_items = null; // 缓存的曲线数据
        private List<DataLabel> _lst_data_label = null;
        private DataLabelStyle _data_label_style = null;
        private FlowLayoutPanel _flowLayoutPanel = null;
        private DyncmicLabel _dynamic_label = null;

        private const int MAX_SPACE_LEFT_RIGHT = 50; // 左右间距
        private const int MAX_SPACE_UP_DOWN = 50;    // 上下间距
        private const int MAX_CURVE_POINT = 3600; // 曲线显示的最大点数
        private const int MAX_CURVE_SCALE = 24; // 曲线最大间隔数

        private Dictionary<int, LabelItem> _dict_label_item = null;

        /* 滤波 */
        private Dictionary<string, FilterAlgorithm> _dict_filter = new Dictionary<string, FilterAlgorithm>();
        private FilterType _filter_type = FilterType.F_First;

        private BufferedGraphicsContext _context = null;
        private BufferedGraphics _grafx = null;
        private static readonly System.Threading.Mutex _mutex_lock = new System.Threading.Mutex();

        private Point _selection_start = Point.Empty;
        private Point _selection_end = Point.Empty;

        // mark line 操作
        private Panel _mark_line = null;
        private bool _mark_line_dragging = false;
        private Point _mark_line_offset = Point.Empty;
        private ToolTip _toolTip = null;

        private Bitmap _bitmap = null;

        private long _start_time = 0;
        private int _max_visible_count = int.MaxValue;
    }
}