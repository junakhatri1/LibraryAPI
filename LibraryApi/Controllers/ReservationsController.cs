using LibraryApi.Domain;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class ReservationsController : ControllerBase
    {

        LibraryDataContext Context;
        ISystemTime Clock;
        IWriteToTheMessageQueue Rabbit;

        public ReservationsController(LibraryDataContext context, ISystemTime clock, IWriteToTheMessageQueue rabbit)
        {
            Context = context;
            Clock = clock;
            Rabbit = rabbit;
        }



        // POST /reservations - create a reservation
        [HttpPost("/reservations")]
        public async Task<ActionResult> AddReservation([FromBody] ReservationCreate reservationToAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reservation = new Reservation
            {
                For = reservationToAdd.For,
                Books = reservationToAdd.Books,
                ReservationCreated = Clock.GetCurrent(),
                Status = ReservationStatus.Pending
            };
            Context.Reservations.Add(reservation);
            await Context.SaveChangesAsync();
            await Rabbit.Write(reservation);

            return CreatedAtRoute("get#reservation", new { Id = reservation.Id }, reservation);
        }

        // GET /reservations - get all reservations
        [HttpGet("/reservations")]
        public async Task<ActionResult> GetReservations()
        {
            var reservations = await Context.Reservations.ToListAsync();
            return Ok(new { data = reservations });
        }

        // POST /reservations/approved - approve a reservation (should be only 
        //                               available to background worker)

        [HttpGet("/reservations/approved")]
        public async Task<ActionResult> GetApprovedReservations()
        {
            var reservations = await Context.Reservations
                .Where(r => r.Status == ReservationStatus.Approved)
                .ToListAsync();
            return Ok(new { data = reservations });
        }
        // POST /reservations/denied - cancel a reservation (should be only 
        //                               available to background worker)

        [HttpPost("/reservations/denied")]
        public async Task<ActionResult> DenyReservation([FromBody] Reservation reservation)
        {
            var storedReservation = await Context.Reservations.SingleOrDefaultAsync(r => r.Id == reservation.Id);
            if (storedReservation == null)
            {
                return BadRequest("No Reservation For that Id");
            }
            else
            {
                storedReservation.Status = ReservationStatus.Cancelled;
                await Context.SaveChangesAsync();
                return NoContent();
            }
        }

        // GET /reservations/approved - show all approved reservations

        [HttpPost("/reservations/approved")]
        public async Task<ActionResult> ApproveReservation([FromBody] Reservation reservation)
        {
            var storedReservation = await Context.Reservations.SingleOrDefaultAsync(r => r.Id == reservation.Id);
            if (storedReservation == null)
            {
                return BadRequest("No Reservation For that Id");
            }
            else
            {
                storedReservation.Status = ReservationStatus.Approved;
                await Context.SaveChangesAsync();
                return NoContent();
            }
        }

        // GET /reservations/denied - show all cancelled reservations
        [HttpGet("/reservations/denied")]
        public async Task<ActionResult> GetDeniedReservations()
        {
            var reservations = await Context.Reservations
                .Where(r => r.Status == ReservationStatus.Cancelled)
                .ToListAsync();
            return Ok(new { data = reservations });
        }

        [HttpGet("/reservations/pending")]
        public async Task<ActionResult> GetPendingReservations()
        {
            var reservations = await Context.Reservations
                .Where(r => r.Status == ReservationStatus.Pending)
                .ToListAsync();
            return Ok(new { data = reservations });
        }

        // GET /reservations/{id} - show a particular reservation
        [HttpGet("/reservations/{id:int}", Name = "get#reservation")]
        public async Task<ActionResult> GetAReservation(int id)
        {
            var reservation = await Context.Reservations.SingleOrDefaultAsync(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(reservation);
            }
        }
    }

    public class ReservationCreate
    {
        [Required]
        public string For { get; set; }
        [Required]
        public string Books { get; set; }
    }
}
