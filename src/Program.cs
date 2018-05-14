using System;
using System.Diagnostics;
using SharpLearningKit;
//using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix a = new Matrix(2048,1024);
            Matrix b = new Matrix(4096,1024);
            Matrix c = new Matrix(4096,2048);
            Matrix d = new Matrix(4096,2048);
            Matrix temp = new Matrix();
            a.RandFill();
            b.RandFill();
            c.RandFill();
            //a.PDot(b);
            //Console.WriteLine(a);
            //c.Forward(a,b);
            //Console.WriteLine(c);
            
            Stopwatch t = new Stopwatch();

            t.Start();
            for (int i = 0; i < 1; i++) {
                d.Copy(a).Dot(temp.Copy(b).Transform()).DerivativeMultiply(c);
            }
            t.Stop();

            //Console.WriteLine(d);
            Console.WriteLine(t.ElapsedMilliseconds);

            t.Restart();
            for (int i = 0; i < 1; i++) {
                d.Backwards(a,b,c,true,4);
            }
            t.Stop();

            //Console.WriteLine(d);
            Console.WriteLine(t.ElapsedMilliseconds);
            
            
            /*
            Matrix l0 = new Matrix(100000,64);
            Matrix y = new Matrix(100000,10);

            Matrix testMatrix = new Matrix(new double[,] {{1}});
            testMatrix.ToString();

            Matrix syn0 = new Matrix(64, 128);
            Matrix syn1 = new Matrix(128, 10);

            syn0.RandFill();
            syn1.RandFill();

            Matrix l1 = new Matrix();
            Matrix l2 = new Matrix();

            Matrix d0 = new Matrix();
            Matrix d1 = new Matrix();

            Matrix temp = new Matrix();

            Console.WriteLine("Starting");
            Stopwatch t = new Stopwatch();
            t.Start();
            for (int i = 0; i < 1; i++)
            {
                l1.Copy(l0).Dot(syn0).Sigmoid();
                l2.Copy(l1).Dot(syn1).Sigmoid();
                d1.Copy(y).Subtract(l2).DerivativeMultiply(l2);
                d0.Copy(d1).Dot(temp.Copy(syn1).Transform()).DerivativeMultiply(l1);
                syn1.Add(temp.Copy(l1).Transform().Dot(d1));
                syn0.Add(temp.Copy(l0).Transform().Dot(d0));
            }

            t.Stop();
            Console.WriteLine("Finished");
            //Console.WriteLine(l2);
            Console.WriteLine("Training took {0} milliseconds", t.ElapsedMilliseconds);
            Console.ReadLine();
            
            
            Matrix a1 = new Matrix(80000, 64);//new double[,] { { 2, 3, 1 }, { 2, -7, 4 } });
            Matrix a2 = new Matrix(64, 256);// new double[,] { { 3, 4, 5 }, { 1, 1, 4 }, { 2, 1, 4 } });
            Matrix a3 = new Matrix(256, 128);
            Matrix a4 = new Matrix(128, 10);
            a1.RandFill();
            a2.RandFill();
            a3.RandFill();
            a4.RandFill();
            Matrix b1 = (new Matrix()).Copy(a1);
            Matrix b2 = (new Matrix()).Copy(a2);
            Matrix b3 = (new Matrix()).Copy(a3);
            Matrix b4 = (new Matrix()).Copy(a4);
            Stopwatch t = new Stopwatch();
            t.Start();
            for (int i = 0; i < 1; i++)
            {
                a1.Dot(a2);
                a1.Dot(a3);
                a1.Dot(a4);
            }
            t.Stop();
            Console.WriteLine(t.ElapsedMilliseconds);
            //Console.WriteLine(a1);
            t.Restart();
            for (int i = 0; i < 1; i++)
            {
                b1.PDot(b2);
                b1.PDot(b3);
                b1.PDot(b4);
            }
            t.Stop();
            Console.WriteLine(t.ElapsedMilliseconds);
            //Console.WriteLine(a1);
            //Console.WriteLine(b1);
            */
        }
    }
}
