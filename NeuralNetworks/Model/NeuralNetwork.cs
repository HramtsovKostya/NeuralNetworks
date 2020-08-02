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
                    var neuron = new Neuron(lastLayer.Count);
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
                var neuron = new Neuron(lastLayer.Count, type);
                outputNeurons.Add(neuron);
            }

            var outputLayer = new Layer(outputNeurons, type);
            Layers.Add(outputLayer);
        }

        public Neuron FeedForward(List<double> inputSignals)
        {
            if (inputSignals.Count != Topology.InputCount)
            {
                throw new ArgumentException("Количество входных сигналов" +
                    " должно соответствовать числу входных нейронов!", nameof(inputSignals));
            }

            SendSignalsToInputNeurons(inputSignals);
            FeedForwardAllLayersAfterInput();

            return Topology.OutputCount == 1 ? Layers.Last().Neurons[0]
                : Layers.Last().Neurons.OrderByDescending(neuron => neuron.Output).First();
        }

        private void SendSignalsToInputNeurons(List<double> inputSignals)
        {
            for (int i = 0; i < inputSignals.Count; i++)
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
    }
}