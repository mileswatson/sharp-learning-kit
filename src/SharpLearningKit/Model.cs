using System;

namespace SharpLearningKit
{
    class Model
    {
        public Matrix[] synapses;

        public Model(int[] structure)
        {
            this.synapses = new Matrix[structure.Length - 1];
            for (int i = 0; i < structure.Length - 1; i++)
            {
                this.synapses[i] = new Matrix(structure[i], structure[i + 1]);
                this.synapses[i].RandFill();
                //Console.WriteLine(this.synapses[i]);
            }
        }

        public void Train(double[,] questions, double[,] answers, int iterations, bool doParallel = false, int numCores = 4)
        {
            Matrix[] layers = new Matrix[this.synapses.Length+1];
            Matrix[] deltas = new Matrix[this.synapses.Length];
            Matrix solution = new Matrix(answers);
            layers[0] = new Matrix(questions);
            for (int i = 0; i < this.synapses.Length; i++)
            {
                layers[i+1] = new Matrix(solution.numRows,this.synapses[i].numColumns);
                Console.WriteLine("layers[{0}] = {1} x {2}",i+1,solution.numRows,this.synapses[i].numColumns);
                deltas[i] = new Matrix(solution.numRows,this.synapses[i].numColumns);
                Console.WriteLine("deltas[{0}] = {1} x {2}",i,solution.numRows,this.synapses[i].numColumns);
            }
            for (int iterationNum = 0; iterationNum < iterations; iterationNum++) {

                for (int i = 0; i < this.synapses.Length; i++)
                {
                    layers[i+1].Forwards(layers[i],this.synapses[i]);
                }

                deltas[this.synapses.Length-1].FirstBackwards(solution, layers[this.synapses.Length]);
                synapses[this.synapses.Length-1].Adjust(layers[this.synapses.Length-1], deltas[this.synapses.Length-1]);
                for (int i = this.synapses.Length - 2; i >= 0; i--) {
                    deltas[i].Backwards(deltas[i+1], this.synapses[i+1], layers[i+1]);
                    synapses[i].Adjust(layers[i], deltas[i]);
                }
            }
            Console.WriteLine(layers[this.synapses.Length]);
        }
    }
}