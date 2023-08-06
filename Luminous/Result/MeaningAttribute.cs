namespace Luminous
{
    public class MeaningAttribute : Attribute
    {
        public MeaningAttribute(string mean)
        {
            Mean = mean;
        }

        public string Mean { get; }
    }
}
