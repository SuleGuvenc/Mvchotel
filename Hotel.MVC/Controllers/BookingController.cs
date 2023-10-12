using AutoMapper;
using Hotel.MVC.DTO.Booking;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Otel.BL.Abstract;
using Otel.BL.Concrete;
using Otel.Entitiy.Concrete;
using Otel.Entity.Authentication;

namespace Hotel.MVC.Controllers
{
    public class BookingController : Controller
    {

        private HotelManager hotelManager;
        private RoomManager roomManager;
        private BookingManager bookingManager;
        private UserManager<AppUser> userManager;
        private readonly IMapper mapper;

        public BookingController(UserManager<AppUser> userManager, IMapper mapper)
        {
            bookingManager = new BookingManager();
            hotelManager = new HotelManager();
            roomManager = new RoomManager();
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Rezerve(int Id)
        {
            var room = await roomManager.GetByIdAsync(Id);

            ViewBag.Room = room;

            BookingCreateDTO createDTO = new();


            return View(createDTO);
        }

        [HttpPost, ActionName("Rezerve")]
        public async Task<IActionResult> Rezerve(int Id, BookingCreateDTO createDTO)
        {
            var room = await roomManager.GetByIdAsync(Id);

            if (room != null)
            {

                string otelId = room.HotelId.ToString();
                createDTO.HotelId = otelId;
                var diff = (createDTO.CheckOut - createDTO.CheckIn.Date).Days;

                var price = Convert.ToInt32(room.Price) * diff;
                createDTO.CalculatedPrice = price.ToString();


            }

            createDTO.RoomId = Id.ToString();



            //burayı ercan hoca yapar
            createDTO.AppUserId = "c10ea729-516d-46cd-b675-4dca1e1ae09a";


            var result = mapper.Map<Booking>(createDTO);
            await bookingManager.InsertAsync(result);
            //RedirectToRoute("/hotel");
            /*}*/
            return View(createDTO);



        }
    }
}
