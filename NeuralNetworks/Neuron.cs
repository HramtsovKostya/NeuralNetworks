using System;
using System.Collections.Generic;

namespace NeuralNetworks
{
    public class Neuron
    {
        public List<double> Weights { get; }
        public NeuronType NeuronType { get; }
        public double Output { get; private set; }

        public Neuron(int inputCount, NeuronType type = NeuronType.Normal)
        {
            if (inputCount <= 0)
            {
                throw new ArgumentException("Количество входных сигналов" +
                    " должно быть положительным числом!", nameof(inputCount));
            }

            NeuronType = type;
            Weights = new List<double>();

            for (int i = 0; i < inputCount; i++)
                Weights.Add(1);
        }

        public void SetWeights(params double[] weights)
        {
            if (weights.Length != Weights.Count)
            {
                throw new ArgumentException("Количество входных параметров" +
                    " должно соответствовать числу весов!", nameof(weights));
            }

            for (int i = 0; i < weights.Length; i++)
                Weights[i] = weights[i];
        }

        public double FeedForward(List<double> inputs)
        {
            if (inputs.Count != Weights.Count)
            {
                throw new ArgumentException("Количество входных сигналов" +
                    " должно соответствовать числу весов!", nameof(inputs));
            }

            var sum = 0.0;

            for (int i = 0; i < inputs.Count; i++)
                sum += inputs[i] * Weights[i];

            Output = Sigmoid(sum);
            return Output;
        }

        private double Sigmoid(double valueX)
        {
            return 1.0 / (1.0 + Math.Exp(-valueX));
        }

        public override string ToString()
        {
            return Output.ToString();
        }
    }
}