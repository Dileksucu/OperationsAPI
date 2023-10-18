using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationsAPI.Data;
using OperationsAPI.Models;
using System.Linq.Expressions;
using Operation = OperationsAPI.Models.Operation;

namespace OperationsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly OperationsAPIDbContext _context;
        public OperationsController(OperationsAPIDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetContacts()
        {
            var result = await _context.Operations.ToListAsync();
            return Ok(result); // status code 200

        }

        [HttpPost]
        public async Task<IActionResult> AddContact(AddOperationRequest addOperation)
        {
            Operation mapped = new()
            {
                FullName = addOperation.FullName,
                Email = addOperation.Email,
                Phone = addOperation.Phone,
                Address = addOperation.Address
            };

            _context.Operations.Add(mapped);
            await _context.SaveChangesAsync();
            return Ok(mapped);

        }

        [HttpPut]
        [Route("{id:guid}")]  //
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateOperationRequest updateOperation)
        {
            var contact = await _context.Operations.FirstOrDefaultAsync(x => x.Id == id);
            //FirstOrDefaultAsync- Operations tablosundan, veritabanındaki Id ile benim girdiğim id ile eşleşeni getir.
            //İki tip dönüşü var ; bulamazsa null döner  , bulursa nesneyi döner.

            if (contact is null)
                return BadRequest();

            // Bunların yerine context.Operations.Update(); yazılabilir miydi?
            contact.Email = updateOperation.Email; // Gelen veriyi veritabanındakine eşitlemem lazımdı. (sağdaki veri sola atanır.)
            contact.Phone = updateOperation.Phone;
            contact.FullName = updateOperation.FullName;
            contact.Address = updateOperation.Address;

            _context.Operations.Update(contact);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpPatch]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdatePatch([FromRoute] Guid id, [FromBody] UpdateOperationPatchCommand command) 
        {
            var contact =await  _context.Operations.FirstOrDefaultAsync(a=>a.Id==id);

            if(contact is null)
                return BadRequest();

            //Dışardan gelen email bilgisi ; boş("")- null- boşlulardan("  ") değilse git emaili güncelle.
            //Boş göndermediğimiz şey parçasal olarak güncellenmek istiyor demektir.
            //hangileri dolu geliyorsa onlar güncellenir.
            if (!string.IsNullOrWhiteSpace(command.Email))
            contact.Email = command.Email;

            if (!string.IsNullOrWhiteSpace(command.FullName))
                contact.FullName = command.FullName;

            if (!string.IsNullOrWhiteSpace(command.Address))
                contact.Address = command.Address;

            if(command.Phone.HasValue)
                contact.Phone = command.Phone.Value;

            _context.Operations.Update(contact);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete]
        public async Task<IActionResult> SoftDelete([FromRoute] Guid id)
        {
            // bir iletişim bilgisi dışarıdan id girilerek bulunmaya çalışılıyor
            var contact = await _context.Operations
                .FirstOrDefaultAsync(x => x.Id == id);

            // İlk başta olumsuz durumu değerlendir -> Fast Fail prensibi
            // iletişim bilgisi bulunamadıysa kullanıcı doğru şekilde veriyi aramadığından BadRequest() dönsün
            if(contact is null) return BadRequest("İletişim bilgisi bulunamadı!");

            // veri bulunduysa da alta geçilir, çünkü olumsuz duruma girilmemiştir
            // bu bir silme isteği olduğu için tabloda bulunan IsDeleted alanı true yapılarak kaydın silindiği belirtilir.
            contact.IsDeleted = true;
            // ardından bu alan kayıt üzerinde güncellenir.
            _context.Operations.Update(contact);
            await _context.SaveChangesAsync();
            return Ok("İletişim bilgileri silindi.");

        }



    }
}
