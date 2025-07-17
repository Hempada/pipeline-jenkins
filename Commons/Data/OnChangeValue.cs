namespace Commons.Data
{
    public class OnChangeValue<T>
    {
        public Action? OnChange { get; set; }

        private T _value { get; set; } = default!;
        public T Value
        {
            get => _value;
            set
            {
                if ((_value is null && value is not null) || (_value is not null && !_value.Equals(value)))
                {
                    _value = value;

                    if (OnChange is not null)
                    {
                        OnChange();
                    }

                    return;
                }
            }
        }

        public OnChangeValue(T value, Action onChange)
        {
            Value = value;
            OnChange = onChange;
        }

        public OnChangeValue(Action onChange)
        {
            OnChange = onChange;
        }
    }
}
