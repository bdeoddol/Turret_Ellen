using Microsoft.ML.OnnxRuntime;

class ModelInfo
{
    public static void Run(string modelPath)
    {
        InferenceSession currModel = new InferenceSession(modelPath);
        getModelInfo(currModel);
    }

        private static void getModelInfo(InferenceSession model)
    {
        // check input data info
        IReadOnlyDictionary<string, NodeMetadata> inputMdat = model.InputMetadata;
        NodeMetadata valuePair;
        string[] symbolicDims;

        Console.WriteLine("Model Input Metadata: ");
        foreach(var value in inputMdat)
        {
            Console.WriteLine(value.Key + "-> ");
                valuePair = value.Value;

                symbolicDims = valuePair.SymbolicDimensions;
                Console.WriteLine("\t Value Pair Symbolic Dims represented as: ");
                Console.Write("\t  ( ");
                foreach(string variable in symbolicDims)
                {
                    Console.Write(variable + " ");
                }
                Console.Write(" )\n\n");


                Console.WriteLine("\t Input Element Type: ");
                Console.WriteLine("\t " + valuePair.ElementDataType + "\n");        
        }        
        
        // check output data info
        IReadOnlyDictionary<string, NodeMetadata> outputMdat =  model.OutputMetadata;
        Console.WriteLine("Model Output Metadata: ");
        foreach(var value in outputMdat)
        {
            Console.WriteLine(value.Key + "-> ");
                valuePair = value.Value;

                symbolicDims = valuePair.SymbolicDimensions;
                Console.WriteLine("\t Value Pair Symbolic Dims represented as: ");
                Console.Write("\t  ( ");
                foreach(string variable in symbolicDims)
                {
                    Console.Write(variable + " ");
                }
                Console.Write(" )\n\n");


                Console.WriteLine("\t Input Element Type: ");
                Console.WriteLine("\t " + valuePair.ElementDataType + "\n");       

        }
    }
}