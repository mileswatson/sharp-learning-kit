using System;
using SharpLearningKit;

namespace Demo
{
    class Program
    {
        static void Clear()
        {
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
        }

        static int Main(string[] args)
        {
            string userAnswer;
            Model m;
            Loader l;
            Parser p;
            Matrix questions, answers;
            Clear();
            while (true)
            {
                while (true)
                {
                    try
                    {
                        Console.WriteLine("ACTION:");
                        Console.WriteLine("\tCREATE");
                        Console.WriteLine("\tLOAD");
                        Console.WriteLine("\tEXIT");
                        userAnswer = Console.ReadLine().ToUpper();
                        Clear();
                        if (userAnswer == "CREATE")
                        {
                            Console.WriteLine("MODEL STRUCTURE:");
                            m = new Model(Array.ConvertAll(Console.ReadLine().Split(), int.Parse));
                            Clear();
                            Console.WriteLine("NAME:");
                            l = new Loader(Console.ReadLine().ToLower() + ".slkm");
                            Clear();
                            break;
                        }
                        else if (userAnswer == "LOAD")
                        {
                            Console.WriteLine("NAME:");
                            l = new Loader(Console.ReadLine().ToLower() + ".slkm");
                            Clear();
                            m = l.Load();
                            break;
                        }
                        else if (userAnswer == "EXIT")
                        {
                            Clear();
                           return 0;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Clear();
                    }
                }
                while (true)
                {
                    try
                    {
                        Console.WriteLine("ACTION:");
                        Console.WriteLine("\tTRAIN");
                        Console.WriteLine("\tPREDICT");
                        Console.WriteLine("\tEVALUATE");
                        Console.WriteLine("\tSAVE");
                        Console.WriteLine("\tBACK");
                        userAnswer = Console.ReadLine().ToUpper();
                        Clear();
                        if (userAnswer == "TRAIN")
                        {
                            p = new Parser(m.synapses[0].numRows,m.synapses[m.synapses.Length-1].numColumns);
                            Console.WriteLine("CSV:");
                            (questions, answers) = p.Load(Console.ReadLine().ToLower()+".csv");
                            Clear();
                            Console.WriteLine("ITERATIONS:");
                            m.Train(questions, answers, Int32.Parse(Console.ReadLine()));
                            Clear();
                        }
                        else if (userAnswer == "PREDICT")
                        {
                            p = new Parser(m.synapses[0].numRows,0);
                            Console.WriteLine("CSV:");
                            (questions, answers) = p.Load(Console.ReadLine().ToLower()+".csv");
                            Clear();
                            Console.WriteLine("DECIMAL PLACES:");
                            userAnswer = Console.ReadLine();
                            Clear();
                            Console.WriteLine( m.Predict(questions).Round(Int32.Parse(userAnswer)) );
                            Console.ReadLine();
                            Clear();
                        }
                        else if (userAnswer == "EVALUATE")
                        {
                            p = new Parser(m.synapses[0].numRows,m.synapses[m.synapses.Length-1].numColumns);
                            Console.WriteLine("CSV:");
                            (questions, answers) = p.Load(Console.ReadLine().ToLower()+".csv");
                            Clear();
                            Console.WriteLine("DECIMAL PLACES:");
                            userAnswer = Console.ReadLine();
                            Clear();
                            Console.WriteLine( answers );
                            Console.WriteLine( m.Predict( questions ).Round(Int32.Parse(userAnswer)) );
                            Console.ReadLine();
                            Clear();
                        }
                        else if (userAnswer == "SAVE")
                        {
                            l.Save(m);
                            Clear();
                        }
                        else if (userAnswer == "BACK")
                        {
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Clear();
                    }
                }
                
            }
        }
    }
}
