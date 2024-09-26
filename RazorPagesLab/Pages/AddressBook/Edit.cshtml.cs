using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesLab.Pages.AddressBook;

public class EditModel : PageModel
{
	private readonly IMediator _mediator;
	private readonly IRepo<AddressBookEntry> _repo;

	public EditModel(IRepo<AddressBookEntry> repo, IMediator mediator)
	{
		_repo = repo;
		_mediator = mediator;
	}

	[BindProperty]
	public UpdateAddressRequest UpdateAddressRequest { get; set; }

	public void OnGet(Guid id)
	{
		// Todo: Use repo to get address book entry, set UpdateAddressRequest fields.
		
		// 1. Get the adress book entry from the repo
		var existingAddress = _repo.Find(new EntryByIdSpecification(id)).FirstOrDefault();

		// 2. Set the UpdateAddressRequest fields (ID, Line1, Line2, City, State, Zip)
		if(existingAddress != null) //first check to make sure the ID exists if not return from the ONGet method
		{
			//create a new UpdateAddressRequest & list out the values
			UpdateAddressRequest = new UpdateAddressRequest
			{
				Id = existingAddress.Id,
				Line1 = existingAddress.Line1,
				Line2 = existingAddress.Line2,
				City = existingAddress.City,
				PostalCode = existingAddress.PostalCode,
				State = existingAddress.State
			};
		}
	}

    public async Task<ActionResult> OnPost() //updated to use async Task<> like the Create.cshtml.cs OnPost
    {
        if (ModelState.IsValid)
        {
            _ = await _mediator.Send(UpdateAddressRequest); //Use the UpdateAddress Request instead of the CreateAddressRequest 
            return RedirectToPage("Index");
        }
        return Page();
	}
}