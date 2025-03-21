﻿using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlazorAdmin.Models;
using Microsoft.eShopWeb;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PublicApiIntegrationTests.RoleManagementEndpoints;

[TestClass]
public class RoleListEndpoint
{
    [TestMethod]
    public async Task ReturnsUnauthorizedForAnonymousAccess()
    {
        var client = ProgramTest.NewClient;
        var response = await client.GetAsync("/api/roles");
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);        
    }


    [TestMethod]
    public async Task ReturnsForbiddenForGeneralAuthorizedAccess()
    {
        var token = ApiTokenHelper.GetNormalUserToken();

        var client = ProgramTest.NewClient;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("/api/roles");
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [TestMethod]
    public async Task ReturnsSuccessAndRolesForAdministratorAccess()
    {
        var token = ApiTokenHelper.GetAdminUserToken();

        var client = ProgramTest.NewClient;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("/api/roles");
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var model = stringResponse.FromJson<RoleListResponse>();
        Assert.IsNotNull(model);
        Assert.IsNotNull(model.Roles);
        Assert.IsTrue(model.Roles.Count > 0);
    }
}
