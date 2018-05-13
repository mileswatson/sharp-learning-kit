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
            }
        }

        public void StochasticGradientDescend(double[,] questions, double[,] answers)
        {
            Matrix a = new Matrix(answers);
            Matrix lastLayer = new Matrix();
            Matrix[] layers = new Matrix[this.synapses.Length];
            Matrix[] deltas = new Matrix[this.synapses.Length];
            layers[0] = new Matrix(questions);
            for (int i = 0; i < this.synapses.Length; i++)
            {
                layers[i + 1] = new Matrix();
                deltas[i] = new Matrix();
            }
            for (int i = 0; i < 10000; i++)
            {
                for (int location = 0; location < this.synapses.Length; location ++)
                {
                    layers[i + 1].Copy(layers[i]).Dot(this.synapses[i]).Sigmoid();
                }
                deltas[0].Copy(a).Subtract(layers[synapses.Length]).DerivativeMultiply(layers[synapses.Length]);
                for (int j = 0; j < synapses.Length - 1; j++)
                {

                }
            }
        }
    }
}