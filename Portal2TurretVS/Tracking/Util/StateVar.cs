using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

public class StateVar
{

    public StateVar()
    {
        ActiveTargets = ImmutableList.Create<Detection>();
        _trackCycle = new Queue<int>();
        _timer = new Stopwatch();
    }
    
    public ImmutableList<Detection> ActiveTargets {get; set;}

    public  Queue<int> _trackCycle;
    public Stopwatch _timer;




}