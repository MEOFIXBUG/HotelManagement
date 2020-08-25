using GalaSoft.MvvmLight.Messaging;
using Login2.Auxiliary.Helpers;
using Login2.Auxiliary.Repository;
using Login2.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Login2.ViewModels.Receptionist
{
    public class RentRoomViewModel : MyBaseViewModel
    {
        private room _room;

        private IRepository<room> roomRepository = null;
        public RentRoomViewModel()
        {
            roomRepository = new BaseRepository<room>();
            Messenger.Default.Register<Parameter>(this, res => Room = GetRoom(((int)res.param)));

        }

        public room Room { get => _room; set { _room = value; RaisePropertyChanged(); } }

        private room GetRoom(int roomID)
        {
            var r = roomRepository.Get(u => u.ID == roomID).FirstOrDefault();
            return r;
        }


    }
}
