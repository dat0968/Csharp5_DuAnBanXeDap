    using APIBanXeDap.ViewModels;

    namespace APIBanXeDap.Repository.UpdateProfile
    {
        public interface IUpdateProfileRepository
        {
            public void UpdateProflie(int id, KhachHangVM proflie);
        }
    }
