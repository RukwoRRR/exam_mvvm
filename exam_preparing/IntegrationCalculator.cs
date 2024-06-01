namespace exam_preparing
{
    public static class IntegrationCalculator
    {
        public static double Integrate(double a, double b, int n)
        {
            double h = (b - a) / n;
            double sum = 0.0;

            Parallel.For(0, n, i =>
            {
                double x0 = a + i * h;
                double x1 = a + (i + 1) * h;
                double y0 = 1 / Math.Sqrt(Math.Pow(1 + Math.Pow(x0, 2), 3));
                double y1 = 1 / Math.Sqrt(Math.Pow(1 + Math.Pow(x1, 2), 3));
                double area = (y0 + y1) * h / 2;
                sum += area;
            });

            return sum;
        }
    }
}
