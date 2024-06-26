﻿using Hotel.Domain.Data;
using Hotel.Domain.Entities.PermissionEntity;
using Hotel.Tests.IntegrationTests.Factories;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;

namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class PermissionControllerTests
{
    private static HotelWebApplicationFactory _factory = null!;
    private static HttpClient _client = null!;
    private static HotelDbContext _dbContext = null!;
    private static string _rootAdminToken = null!; //Allows access all endpoints
    private const string _baseUrl = "v1/permissions";

    [ClassInitialize]
    public static void ClassInitialize(TestContext? context)
    {
        _factory = new HotelWebApplicationFactory();
        _client = _factory.CreateClient();
        _dbContext = _factory.Services.GetRequiredService<HotelDbContext>();

        _rootAdminToken = _factory.LoginFullAccess().Result;
        _factory.Login(_client, _rootAdminToken);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _factory.Dispose();
    }

    [TestInitialize]
    public void TestInitialize()
    {
        _factory.Login(_client, _rootAdminToken);
    }


    [TestMethod]
    public async Task GetPermissions_ShouldReturn_OK()
    {
        //Arrange
        var permission = new Permission("PermissionEx", "Allows access");

        await _dbContext.Permissions.AddAsync(permission);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync($"{_baseUrl}?take=1&name=permission");

        //Assert
        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();
    }

    [TestMethod]
    public async Task GetPermissionById_ShouldReturn_OK()
    {
        //Arrange
        var permission = new Permission("UpdateAdminName", "Allows update admin name");

        await _dbContext.Permissions.AddAsync(permission);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync($"{_baseUrl}/{permission.Id}");

        //Assert
        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();
    }

}
