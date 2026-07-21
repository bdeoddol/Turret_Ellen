using System.Diagnostics.Eventing.Reader;
using System.Reflection;

public class StateProcessing
{
    public static List<int> RebuildTrackCycle(List<Detection> activeTargets)
    {
        //call like this                         _stateVariable.trackCycle = StateProcessing.RebuildTrackCycle(_stateVariable.ActiveTargets.ToList());
        List<int> retList = new();
        // activeTargets.Sort((x,y) => y.conf.CompareTo(x.conf)); 
        // //sort in descending order. given the two elements in order x,y
        // //if the calling instance value (y.conf) is greater than the compared to value (x.conf) return a greater than zero value, then swap the order such that it is now y,x
        // //if y.conf is less than x.conf, return a less than and leave them alone -> (x,y)
        // //if equal, do nothing
        List<Detection> sortedTargets = activeTargets.OrderByDescending(x => x.conf).ToList();

        for(int i = 0; i < sortedTargets.Count; i++)
        {
            retList.Add(sortedTargets[i].detID);
        }
        
        return retList;
    }

    public static int FindRemainingValidIDX(ref StateVar statevariable)
    {

        int startIdx = statevariable.cycleCurrIdx + 1;
        int cycleCount = statevariable.trackCycle.Count;
        List<int> cycle = statevariable.trackCycle; //copy by reference
        int foundIdx = -1;
        for(int i = startIdx; i < cycleCount; i++)
        {
            Console.WriteLine("checking for existing detection for index: " + i);
            if(statevariable.ActiveTargets.Exists(x=>x.detID == cycle[i]) == true)
            {
                //we have found an existing detId
                Console.WriteLine("Found detection at cycleidx: " + i);
                foundIdx = i;

                return foundIdx;
            }
            
        }
        return foundIdx;
    }

    //reset the tracktimer and begin at a given index called "resetIdx"
    public static void resetTrackcycle(ref StateVar statevariable, int resetIdx)
    {
        statevariable.cycleCurrIdx = resetIdx;
        statevariable.timer.Reset();
        statevariable.timer.Start();
    }

    public static void StartDebounce(ref StateVar statevariable)
    {
        statevariable.debounce = true; 
        statevariable.debounceTimer.Restart();
    }
    public static void StopDebounce(ref StateVar statevariable)
    {
        statevariable.debounce = false;
        statevariable.debounceTimer.Reset();
    }

    public static void AdvanceNextValidIDX(ref StateVar statevariable)
    {
        int idx = FindRemainingValidIDX(ref statevariable); //attempt to find the next valid idx in the remainder of the cycle
        if(idx == -1) //if no following valid, rebuild and reset
        {
            Console.WriteLine("could not find next valid trackidx, rebuilding cycle...");
            statevariable.trackCycle = RebuildTrackCycle(statevariable.ActiveTargets.ToList()); //rebuild trackcycle
            resetTrackcycle(ref statevariable, 0);
        }
        else //if found valid, reset timer and begin at valid idx
        {
            resetTrackcycle(ref statevariable, idx);  
        }
    }

    public static void wipeStateVar(ref StateVar statevariable)
    {
        statevariable.ActiveTargets = statevariable.ActiveTargets.Clear();
        statevariable.trackCycle.Clear();
        statevariable.timer.Reset();
        statevariable.debounceTimer.Reset();
        statevariable.debounce = false;
        statevariable.cycleCurrIdx = 0;
        statevariable.centered = false;

    }


    // public static bool FindNextValidID(ref StateVar statevariable)
    // {
    //     // if (statevariable.trackCycle == null || statevariable.trackCycle.Count == 0)
    //     // {
    //     //     statevariable.trackCycle = RebuildTrackCycle(statevariable.ActiveTargets.ToList());
    //     //     return false;   
    //     // }
    //     // int startIdx = statevariable.cycleCurrIdx;
    //     // int count = statevariable.trackCycle.Count;

    //     // for (int i = 0; i < count; i++)
    //     // {
    //     //     statevariable.cycleCurrIdx = (startIdx + i) % count;
    //     //     int id = statevariable.currDetId;

    //     //     var det = statevariable.ActiveTargets.Find(x => x.detID == id);
    //     //     if (det != null)
    //     //     {
    //     //         statevariable.currDet = det;
    //     //         return true;
    //     //     }
    //     // }

    //     // // no valid active IDs found
    //     // statevariable.trackCycle = RebuildTrackCycle(statevariable.ActiveTargets.ToList());
    //     // return false;



    // }


    
}

