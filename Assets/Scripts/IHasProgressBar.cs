using System;

public interface IHasProgressBar
{
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    public class OnProgressChangedEventArgs : EventArgs
    {
        public float ProgressNormalized;
    }
}