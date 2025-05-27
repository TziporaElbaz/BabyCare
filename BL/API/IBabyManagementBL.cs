namespace BL.API
{
    public interface IBabyManagementBL
    {
        int BabysCurrentAge(string BabyId);
        Task<double> GetHeightPercentile(bool gender, int ageMonths, double height);
        Task<double> GetWeightPercentile(bool gender, int ageMonths, double weight);
    }
}