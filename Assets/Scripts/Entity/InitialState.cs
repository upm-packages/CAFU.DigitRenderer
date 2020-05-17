namespace CAFU.NumberRenderer.Entity
{
    public struct InitialState
    {
        public InitialState(bool shouldRender, int value)
        {
            ShouldRender = shouldRender;
            Value = value;
        }

        public bool ShouldRender { get; }
        public int Value { get; }
    }
}