namespace BL.API
{
    public interface IBabyServiceBL
    {
        int BabysCurrentAge(string BabyId);
        public double GetPercentile(bool gender, int age, double weight);
        public double GetHeightPercentile(bool gender, int age, double height);
    }
}