using APIServerSmartHome.Entities;

namespace APIServerSmartHome.IRepository
{
    public interface IUserFacesRepository : IRepositoryBase<UserFaces>
    {
        Task<UserFaces> GetFaceDataOfUser(int userFaceId, int userId);
    }
}
