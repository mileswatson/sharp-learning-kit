using System;
using SharpLearningKit;

namespace Demo
{
    class Program
    {
        static void Clear()
        {
            for (int i = 0; i < 50; i++)
            {
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            string userAnswer;
            Model m;
            Loader l;
            Parser p;
            Matrix questions, answers;
            while (true)
            {
                while (true)
                {
                    Clear();
                    try
                    {
                        Console.WriteLine("ACTION:");
                        Console.WriteLine("\tNEW - create new model");
                        Console.WriteLine("\tLOAD - load previous model");
                        userAnswer = Console.ReadLine().ToUpper();
                        Clear();
                        if (userAnswer == "NEW")
                        {
                            Console.WriteLine("MODEL STRUCTURE:");
                            m = new Model(Array.ConvertAll(Console.ReadLine().Split(), int.Parse));
                            Clear();
                            Console.WriteLine("NAME:");
                            l = new Loader(Console.ReadLine().ToLower() + ".slkm");
                            l.Save(m);
                            break;
                        }
                        else if (userAnswer == "LOAD")
                        {
                            Console.WriteLine("NAME:");
                            l = new Loader(Console.ReadLine().ToLower() + ".slkm");
                            m = l.Load();
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine(e);
                    }
                }
                while (true)
                {
                    Clear();
                    try
                    {
                        Console.WriteLine("ACTION:");
                        Console.WriteLine("\tTRAIN");
                        Console.WriteLine("\tPREDICT");
                        Console.WriteLine("\tEVALUATE");
                    }
                    catch (Exception e)
                    {
                        while (true)
                        {
                            Clear();
                            try
                            {
                                Console.WriteLine("CSV:");
                                p = new Parser(2, 2);
                                (questions, answers) = p.Load(Console.ReadLine().ToLower() + ".csv");
                                break;
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                }
                m.Train(questions, answers, 10000);
                l.Save(m);
                Console.WriteLine( m.Predict( questions ) );
            }
        }
    }
}