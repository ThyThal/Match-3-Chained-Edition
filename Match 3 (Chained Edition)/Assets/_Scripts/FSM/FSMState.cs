using System.Collections.Generic;

public class FSMState<T>
{
    private Dictionary<T, FSMState<T>> _states = new Dictionary<T, FSMState<T>>();

    /*
     * FSM State Functions
     */
    public virtual void Awake() { }
    public virtual void Execute() { }
    public virtual void Sleep() { }

    /*
     * FSM State Methods
     */
    public void AddTransition(T input, FSMState<T> state)
    {
        if (!_states.ContainsKey(input))
        {
            _states.Add(input, state);
        }
    }
    public void RemoveTransition(T input)
    {
        if (_states.ContainsKey(input))
        {
            _states.Remove(input);
        }
    }
    public FSMState<T> GetTransition(T input)
    {
        if (_states.ContainsKey(input))
        {
            return _states[input];
        }

        else
        {
            return null;
        }
    }
}