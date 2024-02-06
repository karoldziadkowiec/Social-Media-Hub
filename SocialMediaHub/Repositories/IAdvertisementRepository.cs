using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public interface IAdvertisementRepository
    {
        Task<IQueryable<Advertisement>> GetAllAdvertisements();
        Task<Advertisement> GetAdvertisement(int advertisementId);
        Task AddAdvertisement(Advertisement advertisement);
        Task UpdateAdvertisement(Advertisement advertisement);
        Task RemoveAdvertisement(int advertisementId);
        Task<byte[]> GetAdvertisementsCsvBytes();
    }
}
