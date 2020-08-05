using System;
using System.Collections.Generic;

namespace NeuralNetworks.Model
{
    public class Neuron
    {
        public List<double> Weights { get; }
        public List<double> Inputs { get; }
        public NeuronType NeuronType { get; }        
        public double Output { get; private set; }
        public double Delta { get; private set; }

        public Neuron(int inputCount, NeuronType type = NeuronType.Hidden)
        {
            if (inputCount <= 0)
            {
                throw new ArgumentException("Количество входных сигналов" +
                    " должно быть положительным числом!", nameof(inputCount));
            }

            NeuronType = type;
            Weights = new List<double>();
            Inputs = new List<double>();

            InitWeightsRandomValue(inputCount);
        }

        private void InitWeightsRandomValue(int inputCount)
        {
            var rnd = new Random();

            for (int i = 0; i < inputCount; i++)
            {
                var value = NeuronType == NeuronType.Input ? 1 : rnd.NextDouble();

                Weights.Add(value);
                Inputs.Add(0);
            }
        }

        public double FeedForward(List<double> inputs)
        {
            if (inputs.Count != Weights.Count)
            {
                throw new ArgumentException("Количество входных сигналов" +
                    " должно соответствовать числу весов!", nameof(inputs));
            }

            double sum = 0;

            for (int i = 0; i < inputs.Count; i++)
            {
                Inputs[i] = inputs[i];
                sum += inputs[i] * Weights[i];
            }

            Output = NeuronType != NeuronType.Input ? Sigmoid(sum) : sum;
            return Output;
        }

        private double Sigmoid(double valueX)
        {
            return 1.0 / (1.0 + Math.Exp(-valueX));
        }

        public double SigmoidDx(double valueX)
        {
            var sigmoid = Sigmoid(valueX);
            return sigmoid * (1 - sigmoid);
        }

        public void Learn(double error, double learningRate)
        {
            if (NeuronType == NeuronType.Input) return;

            Delta = error * SigmoidDx(Output);

            for (int i = 0; i < Weights.Count; i++)
            {
                var weight = Weights[i];
                var input = Inputs[i];

                var newWeight = weight - input * Delta * learningRate;
                Weights[i] = newWeight;
            }
        }

        public override string ToString()
        {
            return Output.ToString();
        }
    }
}