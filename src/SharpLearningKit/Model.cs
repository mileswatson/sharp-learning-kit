using System;

namespace SharpLearningKit
{
    public class Model
    {
        public Matrix[] synapses;

        public Model()
        {
            this.synapses = new Matrix[0];
        }

        public Model(int[] structure)
        {
            this.synapses = new Matrix[structure.Length - 1];
            for (int i = 0; i < structure.Length - 1; i++)
            {
                this.synapses[i] = new Matrix(structure[i], structure[i + 1]);
                this.synapses[i].RandFill();
            }
        }

        public void Train(Matrix questions, Matrix answers, int iterations, bool doParallel = false, int numCores = 4)
        {
            Matrix[] layers = new Matrix[this.synapses.Length+1];
            Matrix[] deltas = new Matrix[this.synapses.Length];
            layers[0] = questions;
            for (int i = 0; i < this.synapses.Length; i++)
            {
                layers[i+1] = new Matrix(answers.numRows,this.synapses[i].numColumns);
                deltas[i] = new Matrix(answers.numRows,this.synapses[i].numColumns);
            }
            for (int iterationNum = 0; iterationNum < iterations; iterationNum++) {

                for (int i = 0; i < this.synapses.Length; i++)
                {
                    layers[i+1].Forwards(layers[i],this.synapses[i]);
                }

                deltas[this.synapses.Length-1].FirstBackwards(answers, layers[this.synapses.Length]);
                synapses[this.synapses.Length-1].Adjust(layers[this.synapses.Length-1], deltas[this.synapses.Length-1]);
                for (int i = this.synapses.Length - 2; i >= 0; i--) {
                    deltas[i].Backwards(deltas[i+1], this.synapses[i+1], layers[i+1]);
                    synapses[i].Adjust(layers[i], deltas[i]);
                }
            }
        }

        public Matrix Predict(Matrix questions, bool doParallel = false, int numCores = 4)
        {
            Matrix[] layers = new Matrix[this.synapses.Length+1];
            layers[0] = questions;
            for (int i = 0; i < this.synapses.Length; i++)
            {
                layers[i+1] = new Matrix(layers[0].numRows,this.synapses[i].numColumns);
            }
            for (int i = 0; i < this.synapses.Length; i++)
            {
                layers[i+1].Forwards(layers[i],this.synapses[i]);
            }
            return layers[this.synapses.Length];
        }
    }
}
