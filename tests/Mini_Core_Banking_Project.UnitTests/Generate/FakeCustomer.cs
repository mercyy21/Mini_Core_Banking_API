﻿using Application.Domain.Entity;
using Application.Domain.Enums;

namespace API.Test.Generate;

public static class FakeCustomer
{
    public static List<Customer> GenerateCustomer()
    {
        return new List<Customer>
        {
            new Customer
            {
                FirstName = "Mercy",
                LastName = "Johnson",
                Email = "cyxsa20@gmail.com",
                PhoneNumber = "0912786534",
                Address = "21A, foster street",
                CreatedAt = DateTime.Now,
                Id =Guid.Parse("ee99627b-a78d-47df-8bc1-94bd3501a4fd"),
                Status = Status.Active ,
                UpdatedAt= DateTime.Now,
                Salt="e28ehbq9ihyqoyw9q=qwjwn0"
           
            },
            

        };
    }
    public static List<Customer> GenerateEmptyCustomer()
    {
        return new List<Customer>() { };
    }
    public static List<Customer> GenerateCustomerDifferentId()
    {
        return new List<Customer>()
        {
            new Customer
            {
                FirstName = "Mercy",
                LastName = "Johnson",
                Email = "cyxsa20@gmail.com",
                PhoneNumber = "0912786534",
                Address = "21A, foster street",
                CreatedAt = DateTime.Now,
                Id = Guid.Parse("fefc120c-d9f2-4072-ab9f-fe357a2f9cc3"),
                Status = Status.Active,
                UpdatedAt = DateTime.Now

            }
        };
     }
}