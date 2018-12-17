using System;
using System.Text;
using System.IO;

namespace SharpLearningKit
{
    class Parser
    {
        public int questions;
        public int answers;

        public Parser(int questions, int answers)
        {
            this.questions = questions;
            this.answers = answers;
        }

        public (Matrix, Matrix) Load(string filename)
        {
            string full;
            using (StreamReader streamReader = new StreamReader(filename, Encoding.UTF8))
            {
                full = streamReader.ReadToEnd();
            }
            string[] lines = full.Split(new char[] {'\n'});
            double[,] questions = new double[lines.Length, this.questions];
            double[,] answers = new double[lines.Length, this.answers];
            string[] values;
            for (int i = 0; i < lines.Length; i++)
            {
                values = lines[i].Split(new char[] {','});
                for (int j = 0; j < values.Length; j++)
                {
                    if (j < this.questions)
                    {
                        questions[i,j] = Convert.ToDouble(values[j]);
                    }
                    else if (this.answers > 0)
                    {
                        answers[i,j-this.questions] = Convert.ToDouble(values[j]);
                    }
                }
            }
            return (new Matrix(questions), new Matrix(answers));
        }
    }
}
