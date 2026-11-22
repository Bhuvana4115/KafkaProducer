using KafkaProducer.Data;
using KafkaProducer.Models;
using KafkaProducer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KafkaProducer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SampleTableController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IKafkaProducerService _kafkaProducer;

        public SampleTableController(AppDbContext context, IKafkaProducerService kafkaProducer)
        {
            _context = context;
            _kafkaProducer = kafkaProducer;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SampleTable>>> GetAll()
        {
            return Ok(await _context.SampleTables.AsNoTracking().ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<SampleTable>> Create(SampleTable record)
        {
            _context.SampleTables.Add(record);
            await _context.SaveChangesAsync();

            await _kafkaProducer.ProduceAsync(record.UniqueId.ToString(), record);

            return CreatedAtAction(nameof(GetAll), new { id = record.UniqueId }, record);
        }
    }
}
