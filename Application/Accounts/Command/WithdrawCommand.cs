﻿using Domain.DTO;
using Domain.Entity;
using FluentValidation;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.AccountCommand
{
    public sealed record WithdrawCommand(Guid AccountId, double Amount) : IRequest<ResponseModel>;
    public sealed class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, ResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _context;
        public WithdrawCommandHandler(IMiniCoreBankingDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel> Handle(WithdrawCommand command, CancellationToken cancellationToken)
        {
            //Withdraw Money
            Account existingUser = await _context.Accounts.FirstOrDefaultAsync(y => y.Id == command.AccountId);
            if (existingUser == null)
            {
                return new ResponseModel { Message = "Account does not exist", Success = false };

            }
            if (command.Amount > existingUser.Balance)
            {
                return new ResponseModel { Message = "Amount exceeds balance", Success = false };
            }

            existingUser.Balance -= command.Amount;
            _context.Accounts.Update(existingUser);

            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseModel { Message = "Amount successfully withdrawn", Success = true };
        }
    }
}

