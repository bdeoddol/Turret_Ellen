using System.Collections.Immutable;
using Microsoft.ML.OnnxRuntime;

public class Postprocessing
{
     private static ImmutableList<Detection> parseOutputData(IDisposableReadOnlyCollection<OrtValue> data)
    {
        OrtValue data_0 = data[0];
        ReadOnlySpan<float> dataSpan = data_0.GetTensorDataAsSpan<float>();
        
        List<Detection> List = new List<Detection>();
        for(int i = 0; i < dataSpan.Length; i+=6)
        {
            Detection newDet = new Detection(
                    dataSpan[i+0],
                    dataSpan[i+1],
                    dataSpan[i+2],
                    dataSpan[i+3],
                    dataSpan[i+4],
                    dataSpan[i+5]);
            List.Add(newDet);
        }
        ImmutableList<Detection> retList = List.ToImmutableList<Detection>();

        return retList;
    }

    private static ImmutableList<Detection> filterByConfidence(ImmutableList<Detection> outputData, double cfdThreshold)
    {
        List<Detection> List = new List<Detection>();
        for(int i = 0; i < outputData.Count; i++)
        {
            if(outputData[i].conf >= cfdThreshold){List.Add(outputData[i]);}
        }
        ImmutableList<Detection> retList = List.ToImmutableList();
        return retList;
    }

    private static ImmutableList<Detection> filterByClass(ImmutableList<Detection> outputData, int classID)
    {
        List<Detection> List = new List<Detection>();
        for(int i = 0; i < outputData.Count; i++)
        {
            if(outputData[i].classID == classID){List.Add(outputData[i]);}
        }
        ImmutableList<Detection> retList = List.ToImmutableList();
        return retList;
    }

}