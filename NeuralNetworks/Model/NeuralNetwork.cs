using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworks.Model
{
    public class NeuralNetwork
    {
        public Topology Topology { get; }
        public List<Layer> Layers { get; }

        public NeuralNetwork(Topology topology)
        {
            Topology = topology ?? throw new ArgumentNullException
                ("Топология нейросети не может быть равной null!", nameof(topology));

            Layers = new List<Layer>();
            CreateInputLayer();
            CreateHiddenLayers();
            CreateOutputLayer();
        }

        private void CreateInputLayer()
        {
            var inputNeurons = new List<Neuron>();
            var type = NeuronType.Input;

            for (int i = 0; i < Topology.InputCount; i++)
            {
                var neuron = new Neuron(1, type);
                inputNeurons.Add(neuron);
            }

            var inputLayer = new Layer(inputNeurons, type);
            Layers.Add(inputLayer);
        }

        private void CreateHiddenLayers()
        {
            foreach (int hiddenCount in Topology.HiddenLayers)
            {
                var hiddenNeurons = new List<Neuron>();
                var lastLayer = Layers.Last();

                for (int j = 0; j < hiddenCount; j++)
                {
                    var neuron = new Neuron(lastLayer.NeuronCount);
                    hiddenNeurons.Add(neuron);
                }

                var hiddenLayer = new Layer(hiddenNeurons);
                Layers.Add(hiddenLayer);
            }
        }

        private void CreateOutputLayer()
        {
            var outputNeurons = new List<Neuron>();
            var lastLayer = Layers.Last();
            var type = NeuronType.Output;

            for (int i = 0; i < Topology.OutputCount; i++)
            {
                var neuron = new Neuron(lastLayer.NeuronCount, type);
                outputNeurons.Add(neuron);
            }

            var outputLayer = new Layer(outputNeurons, type);
            Layers.Add(outputLayer);
        }

        public Neuron Predict(params double[] inputSignals)
        {
            if (inputSignals.Length != Topology.InputCount)
            {
                throw new ArgumentException("Количество входных сигналов" +
                    " должно соответствовать числу входных нейронов!", nameof(inputSignals));
            }

            SendSignalsToInputNeurons(inputSignals);
            FeedForwardAllLayersAfterInput();

            return Topology.OutputCount == 1 ? Layers.Last().Neurons[0]
                : Layers.Last().Neurons.OrderByDescending(neuron => neuron.Output).First();
        }

        private void SendSignalsToInputNeurons(params double[] inputSignals)
        {
            for (int i = 0; i < inputSignals.Length; i++)
            {
                var signal = new List<double>() { inputSignals[i] };
                var neuron = Layers[0].Neurons[i];

                neuron.FeedForward(signal);
            }
        }

        private void FeedForwardAllLayersAfterInput()
        {
            for (int i = 1; i < Layers.Count; i++)
            {
                var previousLayerSignals = Layers[i - 1].GetSignals();
                var layer = Layers[i];

                foreach (var neuron in layer.Neurons)
                    neuron.FeedForward(previousLayerSignals);
            }
        }

        public double Learn(double[] expected, double[,] inputs, int epoch)
        {
            var inputSignals = Scaling(inputs);
            double error = 0;

            for (int i = 0; i < epoch; i++)
            {
                for (int j = 0; j < expected.Length; j++)
                {
                    var output = expected[j];
                    var input = GetRow(inputSignals, j);

                    error += BackPropagation(output, input);
                }
            }

            return error / epoch;
        }

        public static double[] GetRow(double[,] matrix, int row)
        {
            var array = new double[matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(1); i++)
                array[i] = matrix[row, i];

            return array;
        }

        public static double[] GetColumn(double[,] matrix, int column)
        {
            var array = new double[matrix.GetLength(0)];

            for (int i = 0; i < matrix.GetLength(0); i++)
                array[i] = matrix[i, column];

            return array;
        }

        private double BackPropagation(double expected, params double[] inputs)
        {
            var actual = Predict(inputs).Output;
            var difference = actual - expected;

            foreach (var neuron in Layers.Last().Neurons)
                neuron.Learn(difference, Topology.LearningRate);

            for (int i = Layers.Count - 2; i >= 0; i--)
            {
                var layer = Layers[i];
                var previousLayer = Layers[i + 1];

                for (int j = 0; j < layer.NeuronCount; j++)
                {
                    var neuron = layer.Neurons[j];

                    for (int k = 0; k < previousLayer.NeuronCount; k++)
                    {
                        var previousNeuron = previousLayer.Neurons[k];
                        var error = previousNeuron.Weights[j] * previousNeuron.Delta;

                        neuron.Learn(error, Topology.LearningRate);
                    }
                }
            }

            return difference * difference;
        }

        private double[,] Scaling(double[,] inputs)
        {
            var result = new double[inputs.GetLength(0), inputs.GetLength(1)];

            for (int column = 0; column < inputs.GetLength(1); column++)
            {
                var min = inputs[0, column];
                var max = inputs[0, column];

                for (int row = 1; row < inputs.GetLength(0); row++)
                {
                    var item = inputs[row, column];

                    if (item < min) min = item;
                    if (item > max) max = item;
                }

                var divider = max - min;

                for (int row = 0; row < inputs.GetLength(0); row++)
                {
                    result[row, column] = divider != 0 
                        ? (inputs[row, column] - min) / divider : 0;
                }
            }

            return result;
        }

        private double[,] Normalization(double[,] inputs)
        {
            var result = new double[inputs.GetLength(0), inputs.GetLength(1)];

            for (int column = 0; column < inputs.GetLength(1); column++)
            {
                double sum = 0, error = 0;

                // Среднее значение сигнала нейрона.
                for (int row = 0; row < inputs.GetLength(0); row++)
                    sum += inputs[row, column];

                var average = sum / inputs.GetLength(0);
                
                // Стандартное квадратичное отклонение.
                for (int row = 0; row < inputs.GetLength(0); row++)
                    error += Math.Pow(inputs[row, column] - average, 2);

                var standardError = Math.Sqrt(error / inputs.GetLength(0));

                for (int row = 0; row < inputs.GetLength(0); row++)
                    result[row, column] = (inputs[row, column] - average) / standardError;
            }

            return result;
        }
    }
}