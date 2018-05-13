using System;
using System.Text;
using System.Threading.Tasks;

namespace SharpLearningKit
{

    class Matrix
    {
        public double[] values;

        public int numRows;

        public int numColumns;

        public Matrix(double[,] someValues)
        {
            this.numRows = someValues.GetLength(0);
            this.numColumns = someValues.GetLength(1);
            this.values = new double[numRows * numColumns];
            for (int row = 0; row < this.numRows; row++)
            {
                for (int column = 0; column < this.numColumns; column++)
                {
                    this.values[(row * this.numColumns) + column] = someValues[row, column];
                }
            }
        }

        public Matrix(int rows = 0, int columns = 0)
        {
            this.numRows = rows;
            this.numColumns = columns;
            this.values = new double[rows * columns];
        }

        public Matrix Copy(Matrix aMatrix)
        {
            this.numRows = aMatrix.numRows;
            this.numColumns = aMatrix.numColumns;
            this.values = (double[])aMatrix.values.Clone();
            return this;
        }

        public Matrix RandFill()
        {
            Random randgen = new Random();
            for (int i = 0; i < this.values.Length; i++)
            {
                this.values[i] = (randgen.NextDouble() * 2) - 1;
            }
            return this;
        }

        public override string ToString()
        {
            int maxChars = 0;
            for (int row = 0; row < numRows; row++)
            {
                for (int column = 0; column < numColumns; column++)
                {
                    if (values[(row * numColumns) + column].ToString().Length > maxChars)
                    {
                        maxChars = values[(row * numColumns) + column].ToString().Length;
                    }
                }
            }
            StringBuilder builder = new StringBuilder(((numColumns + 1) * numRows * 22) - 1);
            for (int i = 0; i < (maxChars * numColumns) + (numColumns * 3) + 1; i++)
            {
                builder.Append("-");
            }
            builder.Append("\n");
            for (int row = 0; row < numRows; row++)
            {
                builder.Append("| ");
                for (int column = 0; column < numColumns; column++)
                {
                    builder.Append(values[(row * numColumns) + column].ToString());
                    for (int i = 0; i < (maxChars - values[(row * numColumns) + column].ToString().Length); i++)
                    {
                        builder.Append(" ");
                    }
                    builder.Append(" | ");
                }
                builder.Length--;
                builder.Append("\n");
            }
            for (int i = 0; i < (maxChars * numColumns) + (numColumns * 3) + 1; i++)
            {
                builder.Append("-");
            }
            builder.Append("\n");
            return builder.ToString();
        }

        public Matrix Add(Matrix aMatrix)
        {
            for (int i = 0; i < this.values.Length; i++)
            {
                this.values[i] += aMatrix.values[i]; 
            }
            return this;
        }

        public Matrix Subtract(Matrix aMatrix)   
        {
            for (int i = 0; i < this.values.Length; i++)
            {
                this.values[i] -= aMatrix.values[i]; 
            }
            return this;
        }

        public Matrix Multiply(Matrix aMatrix)
        {
            for (int i = 0; i < this.values.Length; i++)
            {
                this.values[i] *= aMatrix.values[i]; 
            }
            return this;
        }

        public Matrix Sigmoid()
        {
            for (int i = 0; i < this.values.Length; i++)
            {
                this.values[i] = 1 / (1 + Math.Pow(Math.E, -this.values[i]));
            }
            return this;
        }

        public Matrix Dot(Matrix aMatrix)
        {
            double[] someValues = (double[])this.values.Clone();
            this.values = new double[this.numRows * aMatrix.numColumns];
            this.numColumns = aMatrix.numColumns;
            int yPos = 0, currentRow = 0;
            for (int xPos = 0; xPos < someValues.Length; xPos++)
            {
                for (int zPos = currentRow; zPos < currentRow + this.numColumns; zPos++)
                {
                    this.values[zPos] += someValues[xPos] * aMatrix.values[yPos];
                    yPos++;
                }
                if (yPos >= aMatrix.values.Length)
                {
                    currentRow += this.numColumns;
                    yPos = 0;
                }
            }
            return this;
        }

        public Matrix DerivativeMultiply(Matrix aMatrix)
        {
            for (int i = 0; i < this.values.Length; i++)
            {
                this.values[i] *= (aMatrix.values[i] * (1 - aMatrix.values[i]));     
            }
            return this;
        }

        public Matrix Transform()
        {
            int aPos = 0, bPos = 1, temp;

            double[] someValues = this.values;
            this.values = new double[this.values.Length];
            for (int i = 0; i < this.values.Length; i++)
            {
                this.values[aPos] = someValues[i];
                aPos += this.numRows;
                if (aPos >= this.values.Length)
                {
                    aPos = bPos;
                    bPos += 1;
                }
            }
            temp = this.numRows;
            this.numRows = this.numColumns;
            this.numColumns = temp;
            return this;
        }

        public Matrix PDot(Matrix aMatrix)
        {
            int iterations = this.numRows * aMatrix.numColumns;
            int numCores = Math.Min(4,iterations);
            double[] someValues = (double[])this.values.Clone();
            this.values = new double[iterations];
            this.numColumns = aMatrix.numColumns;
            Parallel.For(0, numCores, i =>
            {
                int startPos = (iterations * i) / numCores;
                int endPos = (i == numCores-1) ? iterations : (iterations * (i + 1)) / numCores;
                int aPos = (startPos / this.numColumns) * aMatrix.numRows;
                int bPos = startPos % this.numColumns;
                int bReset = bPos;
                int aReset = aPos;
                double total;
                for (int cPos = startPos; cPos < endPos; cPos++)
                {
                    total = 0;
                    for (int j = 0; j < aMatrix.numRows; j++)
                    {
                        total += someValues[aPos] * aMatrix.values[bPos];
                        aPos++;
                        bPos += this.numColumns;
                    }
                    this.values[cPos] = total;
                    bReset++;
                    if (bReset == this.numColumns)
                    {
                        bReset = 0;
                        aReset += aMatrix.numRows;
                    }
                    aPos = aReset;
                    bPos = bReset;
                }
            });
            return this;
        }

        public Matrix Forwards(Matrix prevLayer, Matrix prevSynapse, bool doParallel = false, int numCores = 1)
        {
            //double[] someValues = (double[]) prevLayer.values.Clone();
            //this.values = new double[prevLayer.numRows*prevSynapse.numColumns];
            if ( doParallel )
            {
                numCores = Math.Min(numCores,this.values.Length);
                Parallel.For(0, numCores, i =>
                {
                    int startPos = (this.values.Length * i) / numCores;
                    int endPos = (i == numCores-1) ? this.values.Length : (this.values.Length * (i + 1)) / numCores;
                    int aPos = (startPos / prevSynapse.numColumns) * prevSynapse.numRows;
                    int bPos = startPos % prevSynapse.numColumns;
                    int bReset = bPos;
                    int aReset = aPos;
                    double total;
                    for (int cPos = startPos; cPos < endPos; cPos++)
                    {
                        total = 0;
                        for (int j = 0; j < prevSynapse.numRows; j++)
                        {
                            total += prevLayer.values[aPos] * prevSynapse.values[bPos];
                            aPos++;
                            bPos += prevSynapse.numColumns;
                        }
                        this.values[cPos] = 1.0d / (1.0d + Math.Exp(-total));
                        bReset++;
                        if (bReset == prevSynapse.numColumns)
                        {
                            bReset = 0;
                            aReset += prevSynapse.numRows;
                        }
                        aPos = aReset;
                        bPos = bReset;
                    }
                });
            }
            else
            {
                int bPos = 0, currentRow = 0;
                Array.Clear(this.values,0,this.values.Length);
                for (int aPos = 0; aPos < prevLayer.values.Length; aPos++)
                {
                    for (int cPos = currentRow; cPos < currentRow + prevSynapse.numColumns; cPos++)
                    {
                        this.values[cPos] += prevLayer.values[aPos] * prevSynapse.values[bPos];
                        bPos++;
                    }
                    if (bPos >= prevSynapse.values.Length)
                    {
                        currentRow += prevSynapse.numColumns;
                        bPos = 0;
                    }
                }
                for (int i = 0; i < this.values.Length; i++)
                {
                    this.values[i] = 1.0d / (1.0d + Math.Exp(-this.values[i]));
                }
            }
            return this;
        }

        public Matrix FirstBackwards(Matrix answers, Matrix lastLayer, bool doParallel=false, int numCores = 1)
        {
            if ( doParallel )
            {
                numCores = Math.Min(numCores,this.values.Length);
                Parallel.For(0, numCores, coreNum => 
                {
                    int startPos = (this.values.Length * coreNum) / numCores;
                    int endPos = (coreNum == numCores-1) ? this.values.Length : (this.values.Length * (coreNum + 1)) / numCores;
                    for ( int i = startPos; i < endPos; i++ )
                    {
                        this.values[i] = (answers.values[i] - lastLayer.values[i])
                                        * (lastLayer.values[i] * (1 - lastLayer.values[i]));
                    }
                });
            }
            else
            {
                for (int i = 0; i < this.values.Length; i++)
                {
                    this.values[i] = (answers.values[i] - lastLayer.values[i])
                                    * (lastLayer.values[i] * (1 - lastLayer.values[i]));
                }
            }
            return this;
        }

        public Matrix Backwards(Matrix nextDelta, Matrix nextLayer, Matrix nextSynapse)
        {
            return this;
        }
    }
}
































