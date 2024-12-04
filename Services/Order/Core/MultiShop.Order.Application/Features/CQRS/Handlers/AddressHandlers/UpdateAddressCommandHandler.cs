using MultiShop.Order.Application.Features.CQRS.Commands.AddressCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;
using System.Data;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.Addresshandlers
{
	public class UpdateAddressCommandHandler
	{
		private readonly IRepository<Address> _repository;

		public UpdateAddressCommandHandler(IRepository<Address> repository)
		{
			_repository = repository;
		}

		public async Task Handle(UpdateAddressCommand command)
		{
			var values = await _repository.GetByIdAsync(command.AddressId);
			values.Detail = command.Detail;
			values.District = command.District;
			values.District = command.District;
			values.City = command.City;
			values.UserId = command.UserId;

			await _repository.UpdateAsync(values);

		}
	}
}
