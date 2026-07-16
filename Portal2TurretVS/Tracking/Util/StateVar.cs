using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

public class StateVar
{

    public StateVar()
    {
        ActiveTargets = ImmutableList.Create<Detection>();
        trackCycle = new Queue<int>(); 
        timer = new Stopwatch();
        cameraInit = new CameraInit();
        
    }
    
    public ImmutableList<Detection> ActiveTargets {get; set;}
    //always keep this updated with every frame

    public  Queue<int> trackCycle{get;set;}
    //the order in which our detections will be tracked
    //Rule: Track the next highest confidence detection
    //update this track cycle if: 
        //The cycle has been exhausted                                 //use this _stateVariable?._trackCycle.Last() == currvalue we are looking at
        //The cycle is empty                                          //use this _stateVariable._trackCycle.Count == 0
        //The current tracking detection is lost in active targets   //use this _stateVariable?.ActiveTargets.Exists(x => x.detID == [id value]);
    public Stopwatch timer {get;set;} //utility

    public CameraInit cameraInit {get;set;} //the state variable will have a camera initialization which holds all the camera calibrations

    




}