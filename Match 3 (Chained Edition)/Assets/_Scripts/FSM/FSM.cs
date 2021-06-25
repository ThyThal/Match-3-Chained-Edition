public class FSM<T>
{
    private FSMState<T> _currentState;

    /*
     * FSM Methods
     */
    public void SetInitState(FSMState<T> initialState)
    {
        _currentState = initialState;
        _currentState.Awake();
    }
    public void OnUpdate()
    {
        if (_currentState != null)
        {
            _currentState.Execute();
        }
    }
    public void Transition(T input)
    {
        FSMState<T> newState = _currentState.GetTransition(input);

        if (newState != null)
        {
            _currentState.Sleep();
            _currentState = newState;
            _currentState.Awake();
        }
    }
    public FSMState<T> GetCurrentState()
    {
        return _currentState;
    }
}