using Login2.Auxiliary.Helpers;
using Login2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login2.ViewModels.Sales
{
    public class RoomListViewModel : MyBaseViewModel
    {
        private ObservableCollection<room> _roomList;
        public ObservableCollection<room> RoomList
        {
            get { return _roomList; }
            set { _roomList = value; }
        }
        private IRepository<room> roomRepository = null;
        public RoomListViewModel()
        {
            roomRepository = new BaseRepository<room>();
            var res = roomRepository.Get(null, null, "room_status,room_type");
            _roomList = new ObservableCollection<room>();
            res.Distinct().ToList().ForEach(i => _roomList.Add(i));
        }
    }
}
