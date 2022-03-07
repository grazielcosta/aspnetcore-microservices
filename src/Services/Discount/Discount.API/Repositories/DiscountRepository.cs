﻿using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new Npgsql.NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            return coupon ?? new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new Npgsql.NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("Insert Into Coupon (ProductName, Description, Amount) Values (@ProductName, @Description, @Amount)",
                        new { ProductName = coupon?.ProductName, Description = coupon.Description, Amount = coupon?.Amount ?? 0 });

            return affected == 0 ? false : true;
        }
        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new Npgsql.NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("Update Coupon  SET ProductName = @ProductName, Description = @Description, Amount = @Amount Where Id = @Id",
                        new { ProductName = coupon?.ProductName, Description = coupon.Description, Amount = coupon?.Amount ?? 0, Id = coupon?.Id ?? int.MinValue });

            return affected == 0 ? false : true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new Npgsql.NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("Delete From Coupon Where ProductName = @ProductName",
                        new { ProductName = productName });

            return affected == 0 ? false : true;
        }
    }
}