using System;
using System.Drawing;

namespace UIResource
{
    class BasicFramework
    {
        /// <summary>
        /// 图形的方向
        /// </summary>
        public enum GraphDirection
        {
            /// <summary>
            /// 向上
            /// </summary>
            Upward = 1,
            /// <summary>
            /// 向下
            /// </summary>
            Downward = 2,
            /// <summary>
            /// 向左
            /// </summary>
            Ledtward = 3,
            /// <summary>
            /// 向右
            /// </summary>
            Rightward = 4,

        }

        /// <summary>
        /// 根据指定的方向绘制一个箭头
        /// </summary>
        /// <param name="g"> 绘图对象 </param>
        /// <param name="brush"> 画刷 </param>
        /// <param name="point"> 点 </param>
        /// <param name="size"> 大小 </param>
        /// <param name="direction"> 箭头方向 </param>
        public static void PaintTriangle(Graphics g, Brush brush, Point point, int size, GraphDirection direction)
        {
            Point[] points = new Point[4];
            if (direction == GraphDirection.Ledtward)
            {
                points[0] = new Point(point.X, point.Y - size);
                points[1] = new Point(point.X, point.Y + size);
                points[2] = new Point(point.X - 2 * size, point.Y);
            }
            else if (direction == GraphDirection.Rightward)
            {
                points[0] = new Point(point.X, point.Y - size);
                points[1] = new Point(point.X, point.Y + size);
                points[2] = new Point(point.X + 2 * size, point.Y);
            }
            else if (direction == GraphDirection.Upward)
            {
                points[0] = new Point(point.X - size, point.Y);
                points[1] = new Point(point.X + size, point.Y);
                points[2] = new Point(point.X, point.Y - 2 * size);
            }
            else
            {
                points[0] = new Point(point.X - size, point.Y);
                points[1] = new Point(point.X + size, point.Y);
                points[2] = new Point(point.X, point.Y + 2 * size);
            }

            points[3] = points[0];
            g.FillPolygon(brush, points);
        }

        /// <summary>
        /// 计算绘图时的相对偏移值
        /// </summary>
        /// <param name="max"> 0-100分的最大值，就是指准备绘制的最大值 </param>
        /// <param name="min"> 0-100分的最小值，就是指准备绘制的最小值 </param>
        /// <param name="height"> 实际绘图区域的高度 </param>
        /// <param name="value"> 需要绘制数据的当前值 </param>
        /// <returns> 相对于0的位置，还需要增加上面的偏值 </returns>
        public static float ComputePaintLocationY(int max, int min, int height, int value)
        {
            return height - (value - min) * 1.0f / (max - min) * height;
        }

        /// <summary>
        /// 计算绘图时的相对偏移值
        /// </summary>
        /// <param name="max"> 0-100分的最大值，就是指准备绘制的最大值 </param>
        /// <param name="min"> 0-100分的最小值，就是指准备绘制的最小值 </param>
        /// <param name="height"> 实际绘图区域的高度 </param>
        /// <param name="value"> 需要绘制数据的当前值 </param>
        /// <returns> 相对于0的位置，还需要增加上面的偏值 </returns>
        public static float ComputePaintLocationY(float max, float min, int height, float value)
        {
            return height - (value - min) / (max - min) * height;
        }

        /// <summary>
        /// 计算坐标轴上的真实值
        /// </summary>
        /// <param name="max"> 坐标轴绘制的最大值 </param>
        /// <param name="min"> 最标轴绘制的最小值 </param>
        /// <param name="height"> 实际坐标轴的大小 </param>
        /// <param name="pos"> 当前位置 </param>
        /// <returns> 坐标轴上的真实值 </returns>
        public static float ComputeRealValueFromLoaction(float max, float min, int height, int pos)
        {
            return max - ((height * 1.0f - pos * 1.0f) / (height * 1.0f)) * (max - min);
        }

        /// <summary>
        /// 一个通用的数组新增个数方法，会自动判断越界情况，越界的情况下，会自动的截断或是填充 -> 
        /// </summary>
        /// <typeparam name="T"> 数据类型 </typeparam>
        /// <param name="array"> 原数据 </param>
        /// <param name="data"> 等待新增的数据 </param>
        /// <param name="max"> 原数据的最大值 </param>
        public static void AddArrayData<T>(ref T[] array, T[] data, int max)
        {
            if (data == null) return;           // 数据为空
            if (data.Length == 0) return;       // 数据长度为空

            if (array.Length == max)
            {
                Array.Copy(array, data.Length, array, 0, array.Length - data.Length);
                Array.Copy(data, 0, array, array.Length - data.Length, data.Length);
            }
            else
            {
                if ((array.Length + data.Length) > max)
                {
                    T[] tmp = new T[max];
                    for (int i = 0; i < (max - data.Length); i++)
                    {
                        tmp[i] = array[i + (array.Length - max + data.Length)];
                    }
                    for (int i = 0; i < data.Length; i++)
                    {
                        tmp[tmp.Length - data.Length + i] = data[i];
                    }
                    // 更新数据
                    array = tmp;
                }
                else
                {
                    T[] tmp = new T[array.Length + data.Length];
                    for (int i = 0; i < array.Length; i++)
                    {
                        tmp[i] = array[i];
                    }
                    for (int i = 0; i < data.Length; i++)
                    {
                        tmp[tmp.Length - data.Length + i] = data[i];
                    }

                    array = tmp;
                }
            }
        }
    }
}
