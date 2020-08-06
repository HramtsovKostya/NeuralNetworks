using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworks.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NeuralNetworks.Tests
{
    [TestClass()]
    public class NeuralNetworkTests
    {
        [TestMethod()]
        public void PredictTest()
        {
            // Результат: 1 - пациент болен, 0 - пациент здоров

            // T - температура
            // A - хороший возраст
            // S - курит
            // F - правильно питается

            var outputs = new double[]
            { 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1 };

            var inputs = new double[,]
            {
                { 0, 0, 0, 0 },
                { 0, 0, 0, 1 },
                { 0, 0, 1, 0 },
                { 0, 0, 1, 1 },
                { 0, 1, 0, 0 },
                { 0, 1, 0, 1 },
                { 0, 1, 1, 0 },
                { 0, 1, 1, 1 },
                { 1, 0, 0, 0 },
                { 1, 0, 0, 1 },
                { 1, 0, 1, 0 },
                { 1, 0, 1, 1 },
                { 1, 1, 0, 0 },
                { 1, 1, 0, 1 },
                { 1, 1, 1, 0 },
                { 1, 1, 1, 1 }
            };

            var topology = new Topology(4, 1, 0.1, 2);
            var neuralNetwork = new NeuralNetwork(topology);
            neuralNetwork.Learn(outputs, inputs, 100000);

            var results = new List<double>();

            for (int i = 0; i < outputs.Length; i++)
            {
                var row = NeuralNetwork.GetRow(inputs, i);
                var result = neuralNetwork.Predict(row).Output;
                results.Add(result);
            }

            for (int i = 0; i < results.Count; i++)
            {
                var expected = Math.Round(outputs[i], 2);
                var actual = Math.Round(results[i], 2);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod()]
        public void DatasetTest()
        {
            var outputs = new List<double>();
            var inputs = new List<double[]>();

            using (var sr = new StreamReader(@"..\..\heart.csv"))
            {
                var header = sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    var row = sr.ReadLine();
                    var values = row.Split(',')
                        .Select(v => Convert.ToDouble(v.Replace(".", ","))).ToList();
                    
                    var output = values.Last();
                    var input = values.Take(values.Count - 1).ToArray();

                    outputs.Add(output);
                    inputs.Add(input);
                }
            }

            var inputSignals = new double[inputs.Count, inputs[0].Length];

            for (int i = 0; i < inputSignals.GetLength(0); i++)
            {
                for (int j = 0; j < inputSignals.GetLength(1); j++)
                    inputSignals[i, j] = inputs[i][j];
            }

            var topology = new Topology(inputs[0].Length, 1, 0.1, inputs[0].Length / 2);
            var neuralNetwork = new NeuralNetwork(topology);
            neuralNetwork.Learn(outputs.ToArray(), inputSignals, 10000);

            var results = new List<double>();

            for (int i = 0; i < outputs.Count; i++)
            {
                var result = neuralNetwork.Predict(inputs[i]).Output;
                results.Add(result);
            }

            for (int i = 0; i < results.Count; i++)
            {
                var expected = Math.Round(outputs[i], 2);
                var actual = Math.Round(results[i], 2);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod()]
        public void RecognizeImageTest()
        {
            var parasitizedPath = @"C:\Users\User\Downloads\cell_images\Parasitized\";
            var uninfectedPath = @"C:\Users\User\Downloads\cell_images\Uninfected\";
            var countOfImages = 1000;

            var converter = new PictureConverter();
            var parasitizedImageInput = converter.Convert(@"..\..\Images\Parasitized.png");
            var uninfectedImageInput = converter.Convert(@"..\..\Images\Uninfected.png");

            var count = parasitizedImageInput.Count;
            var topology = new Topology(count, 1, 0.1, count / 2);
            var neuralNetwork = new NeuralNetwork(topology);

            var parasitizedInputs = GetData(parasitizedPath, converter, parasitizedImageInput, countOfImages);
            neuralNetwork.Learn(new double[] { 1 }, parasitizedInputs, 10);

            var uninfectedInputs = GetData(uninfectedPath, converter, uninfectedImageInput, countOfImages);
            neuralNetwork.Learn(new double[] { 0 }, uninfectedInputs, 10);

            var parasitized = neuralNetwork.Predict(ToArrayDouble(parasitizedImageInput)).Output;
            var uninfected = neuralNetwork.Predict(ToArrayDouble(uninfectedImageInput)).Output;

            Assert.AreEqual(1, Math.Round(parasitized, 2));
            Assert.AreEqual(0, Math.Round(uninfected, 2));
        }

        private static double[] ToArrayDouble(IEnumerable<int> items)
        {
            return items.Select(i => (double)i).ToArray();
        }

        private static double[,] GetData(string path, 
            PictureConverter converter, List<int> imageInput, int size)
        {
            var images = Directory.GetFiles(path);
            var inputs = new double[size, imageInput.Count];

            for (int i = 0; i < size; i++)
            {
                var image = converter.Convert(images[i]);

                for (int j = 0; j < image.Count; j++)
                    inputs[i, j] = image[j];
            }
            return inputs;
        }
    }
}