using System;

namespace UIResource
{
    public enum FilterType
    {
        F_None = 0,
        F_First, // 一阶滤波
        F_Kalman, // 卡尔曼滤波
        F_Limit
    }

    public abstract class FilterFactory
    {
        public abstract FilterAlgorithm CreateFilter();
    }

    public abstract class FilterAlgorithm
    {
        public abstract void Initialize();
        public abstract double Filter(double measurement);
        public abstract double Filter(double measurement, double u);
    }

    public class FilterLimit : FilterAlgorithm
    {
        public override void Initialize()
        {
            final = 0;
        }

        public override double Filter(double measurement)
        {
            if (range < Math.Abs(final - measurement))
                final = (measurement + final) / 2;
            else
                final = measurement;
            return final;
        }

        public override double Filter(double measurement, double u)
        {
            _ = u;
            return Filter(measurement);
        }

        private double final = 0;
        private const int range = 10;
    }

    public class FilterFirstOrder : FilterAlgorithm
    {
        public override void Initialize()
        {

        }

        public override double Filter(double measurement)
        {
            final = a * measurement + (1 - a) * final;
            return final;
        }

        public override double Filter(double measurement, double u)
        {
            final = u * measurement + (1 - u) * final;
            return final;
        }

        private double final = 0; // 上次采集的数据
        private float a = 0.6f; // a为0~1之间的数，算法的灵敏度，a越大，新采集的值占的权重越大，算法越灵敏，但平顺性差；相反，a越小，新采集的值占的权重越小，灵敏度差，但平顺性好。
    }

    public class FilterKalman : FilterAlgorithm
    {
        public override void Initialize()
        {

        }

        public FilterKalman(double R, double Q, double A, double B, double H)
        {
            this.R = R;  // 过程噪声 
            this.Q = Q;  // 测量噪声

            this.A = A;  // 状态转移矩阵
            this.B = B;  // 控制矩阵  u为控制向量
            this.H = H;  // 将估计范围与单位转化为与系统变量(或者说测量值)一致的范围与单位

            this.cov = double.NaN;
            this.x = double.NaN; // estimated signal without noise
        }

        public FilterKalman(double R, double Q)
        {
            this.R = R;
            this.Q = Q;
        }

        public override double Filter(double measurement)
        {
            double u = 0;
            if (double.IsNaN(this.x))
            {
                this.x = (1 / this.H) * measurement;
                this.cov = (1 / this.H) * this.Q * (1 / this.H);
            }
            else
            {
                double predX = (this.A * this.x) + (this.B * u);
                double predCov = ((this.A * this.cov) * this.A) + this.R;

                // Kalman gain
                double K = predCov * this.H * (1 / ((this.H * predCov * this.H) + this.Q));

                // Correction
                this.x = predX + K * (measurement - (this.H * predX));
                this.cov = predCov - (K * this.H * predCov);
            }
            return this.x;
        }

        public override double Filter(double measurement, double u)
        {
            if (double.IsNaN(this.x))
            {
                this.x = (1 / this.H) * measurement;
                this.cov = (1 / this.H) * this.Q * (1 / this.H);
            }
            else
            {
                double predX = (this.A * this.x) + (this.B * u);
                double predCov = ((this.A * this.cov) * this.A) + this.Q;

                // Kalman gain
                double K = predCov * this.H * (1 / ((this.H * predCov * this.H) + this.Q));

                // Correction
                this.x = predX + K * (measurement - (this.H * predX));
                this.cov = predCov - (K * this.H * predCov);
            }
            return this.x;
        }

        public double lastMeasurement()
        {
            return this.x;
        }

        public void setMeasurementNoise(double noise)
        {
            this.Q = noise;
        }

        public void setProcessNoise(double noise)
        {
            this.R = noise;
        }

        private double A = 1;
        private double B = 0;
        private double H = 1;

        private double R;
        private double Q;

        private double cov = double.NaN;
        private double x = double.NaN;
    }

    public class LimitFactory : FilterFactory
    {
        public override FilterAlgorithm CreateFilter()
        {
            return new FilterLimit();
        }
    }

    public class FirstOrderFactory : FilterFactory
    {
        public override FilterAlgorithm CreateFilter()
        {
            return new FilterFirstOrder();
        }
    }

    public class KalmanFactory : FilterFactory
    {
        public override FilterAlgorithm CreateFilter()
        {
            return new FilterKalman(0.008, 0.1);
            /*
             * FilterAlgorithm filter = new FilterKalman(0.008, 0.1, 1, 1, 1);
             * double u = 0.2;
             * filter.filter(x, u);
            */
        }
    }
}
