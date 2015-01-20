namespace SGI.Core.GameInfo
{
    public class GameInfo
    {
        public string Name { get; set; }
        public string ActivationInfo { get; set; }

        public override string ToString()
        {
            return string.Format("{0} | {1}", Name, ActivationInfo);
        }
    }
}
