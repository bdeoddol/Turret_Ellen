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
}