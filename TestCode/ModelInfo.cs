using Microsoft.ML.OnnxRuntime;

class ModelInfo
{
    public static void Run(string modelPath)
    {
        // InferenceSession currModel = new InferenceSession(modelPath);
        // SessionOptions options = new SessionOptions(); TODO append necessary options
        using var  opt = SessionOptions.MakeSessionOptionWithCudaProvider(0);
        InferenceSession currModel = new InferenceSession(modelPath, opt);

        getModelInfo(currModel);
    }

        private static void getModelInfo(InferenceSession model)
    {
        

        IReadOnlyCollection<string> EPAvail = OrtEnv.Instance().GetAvailableProviders();
        Console.WriteLine("Software Check: List of all avilable Executions Provider support on this device: ");
        foreach(var val in EPAvail)
        {
            Console.Write(val + " ");
        }

        Console.WriteLine("\n\n");

        IReadOnlyCollection<OrtEpDevice> EPDeviceAvail = OrtEnv.Instance().GetEpDevices();
        OrtKeyValuePairs valPair;
        IReadOnlyDictionary<string,string> entries;
        Console.WriteLine("Hardware Check: List of all Executions Providers for hardware on this device: ");
        foreach(var val in EPDeviceAvail)
        {
            Console.Write("EpName: " + val.EpName + "\n\t"
                         + "Ep Hardware Device Type: " + val.HardwareDevice.Type + "\n\t"
                         + "Ep Hardware Device ID: " + val.HardwareDevice.DeviceId + "\n\t"
                         
            );

            valPair = val.EpOptions;
            entries = valPair.Entries;
            foreach(var entry in entries)
            {
                Console.WriteLine(entry.Key + " " + entry.Value);
            }
        }
        Console.WriteLine("");


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
                Console.Write(")\n\n");


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
                Console.Write(")\n\n");


                Console.WriteLine("\t Input Element Type: ");
                Console.WriteLine("\t " + valuePair.ElementDataType + "\n");       

        }
    }
}