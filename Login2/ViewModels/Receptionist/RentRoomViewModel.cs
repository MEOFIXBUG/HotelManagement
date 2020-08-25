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
        private List<service> _listService;
        private IRepository<room> roomRepository = null;
        public RentRoomViewModel()
        {
            roomRepository = new BaseRepository<room>();
            Messenger.Default.Register<Parameter>(this, res => Room = GetRoom(((int)res.param)));
            ListService = getFullService();
        }

        public room Room { get => _room; set { _room = value; RaisePropertyChanged(); } }

        public List<service> ListService { get => _listService; set { _listService = value; RaisePropertyChanged(); } }

        private room GetRoom(int roomID)
        {
            room r;
            using (var db = new hotelEntities()) {
                r = db.rooms.Include("room_status").Include("room_type").Where<room>(x => x.ID == roomID).FirstOrDefault();
            }
            
            return r;
        }

        private List<service> getFullService()
        {
            List<service> list;
            using(var db = new hotelEntities())
            {
                list = db.services.ToList();
            }
            return list;
        }

    }
}
